// created on 17/05/2010 at 09:06 am
///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. cambio a GTKPrint con Pango y Cairo arcangeldoc@gmail.com
//				  				  
// Licencia		: GLP
//////////////////////////////////////////////////////////
//
// proyect osiris is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// proyect osiris is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Foobar; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
//////////////////////////////////////////////////////////
// Programa		: 
// Proposito	: 
// Objeto		: 
//////////////////////////////////////////////////////////

using System;
using Gtk;
using Npgsql;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class rpt_ocupacion_hospitalaria
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 70;
		int separacion_linea = 10;
		int numpage = 1;
		
		string tiporeporte = "SINALTA";
		string titulo = "REPORTE DE PACIENTES SIN ALTA";
						
		decimal sumacuenta = 0;
		decimal totabono = 0;
		
		private TreeStore treeViewEngineocupacion;
		
		class_public classpublic = new class_public();
		
		public rpt_ocupacion_hospitalaria (object treeViewEngineocupacion_,decimal sumacuenta_,decimal totabono_)
		{
			escala_en_linux_windows = classpublic.escala_linux_windows;
			treeViewEngineocupacion = (object) treeViewEngineocupacion_ as Gtk.TreeStore;
			sumacuenta = sumacuenta_;
			totabono = totabono_;
			
			print = new PrintOperation ();
			print.JobName = "Reporte de Ocupacion Hospitalaria";	// Name of the report
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run(PrintOperationAction.PrintDialog, null);			
		}
		
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1;  // crea cantidad de copias del reporte			
			// para imprimir horizontalmente el reporte
			print.PrintSettings.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{			
			PrintContext context = args.Context;			
			ejecutar_consulta_reporte(context);
		}
						
		void ejecutar_consulta_reporte(PrintContext context)
		{	
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			imprime_encabezado(cr,layout);
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;	
						
			TreeIter iter;
			string tomovalor1 = "";
			int contadorprocedimientos = 0;
			if (this.treeViewEngineocupacion.GetIterFirst (out iter)){
				imprime_encabezado(cr,layout);
				
				while (this.treeViewEngineocupacion.IterNext(ref iter)){
					
				}
			}
			
					/*
					Gnome.Print.Setfont (ContextoImp, fuente6);
					ContextoImp.MoveTo(45, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,2));//pid
					ContextoImp.MoveTo(70, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,1));//folio
					ContextoImp.MoveTo(105, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,3));//fecha ingreso
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,0);
					if(tomovalor1.Length > 30){
						tomovalor1 = tomovalor1.Substring(0,30); 
					}
					ContextoImp.MoveTo(160,fila);					ContextoImp.Show(tomovalor1);//nombre
					
					ContextoImp.MoveTo(285, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,8));//habitacion
					ContextoImp.MoveTo(325, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,4));//saldo
					ContextoImp.MoveTo(365, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,5));//abono
					
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,6);
					if(tomovalor1.Length > 30){
						tomovalor1 = tomovalor1.Substring(0,30); 
					}
					ContextoImp.MoveTo(415,fila);					ContextoImp.Show(tomovalor1);//a pagar
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,7);
					if(tomovalor1.Length > 20){
					tomovalor1 = tomovalor1.Substring(0,20); 
					}
					ContextoImp.MoveTo(460,fila);					ContextoImp.Show(tomovalor1);//medico
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,9);//diagnostico
					if(tomovalor1.Length > 30){
						tomovalor1 = tomovalor1.Substring(0,30); 
					}
					ContextoImp.MoveTo(540,fila);					ContextoImp.Show(tomovalor1);
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,10);
					if(tomovalor1.Length > 20){
						tomovalor1 = tomovalor1.Substring(0,20); 
					}
					ContextoImp.MoveTo(660,fila);					ContextoImp.Show(tomovalor1);//topo paciente
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,11);
					if(tomovalor1.Length > 25){
						tomovalor1 = tomovalor1.Substring(0,25); 
					}
					ContextoImp.MoveTo(715,fila);					ContextoImp.Show(tomovalor1);//topo paciente
						
					//ContextoImp.MoveTo(725, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,10));//empresa
					fila -= 08;
					//contador+=1;
					contadorprocedimientos += 0;
					salto_pagina(ContextoImp,trabajoImpresion);
					
					while (this.treeViewEngineocupacion.IterNext(ref iter)){
						Gnome.Print.Setfont (ContextoImp, fuente6);
						ContextoImp.MoveTo(45, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,2));//pid
						ContextoImp.MoveTo(70, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,1));//folio
						ContextoImp.MoveTo(105, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,3));//fecha ingreso
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,0);
						if(tomovalor1.Length > 30){
							tomovalor1 = tomovalor1.Substring(0,30); 
						}
						ContextoImp.MoveTo(160,fila);					ContextoImp.Show(tomovalor1);//nombre
						
						ContextoImp.MoveTo(285, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,8));//habitacion
						ContextoImp.MoveTo(325, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,4));//saldo
						ContextoImp.MoveTo(365, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,5));//abono
						
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,6);
						if(tomovalor1.Length > 30){
							tomovalor1 = tomovalor1.Substring(0,30); 
						}
						ContextoImp.MoveTo(415,fila);					ContextoImp.Show(tomovalor1);//a pagar
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,7);
						if(tomovalor1.Length > 20){
						tomovalor1 = tomovalor1.Substring(0,20); 
						}
						ContextoImp.MoveTo(460,fila);					ContextoImp.Show(tomovalor1);//medico
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,9);//diagnostico
						if(tomovalor1.Length > 30){
							tomovalor1 = tomovalor1.Substring(0,30); 
						}
						ContextoImp.MoveTo(540,fila);					ContextoImp.Show(tomovalor1);
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,10);
						if(tomovalor1.Length > 20){
						tomovalor1 = tomovalor1.Substring(0,20); 
						}
						ContextoImp.MoveTo(660,fila);					ContextoImp.Show(tomovalor1);//topo paciente
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,11);
						if(tomovalor1.Length > 25){
						tomovalor1 = tomovalor1.Substring(0,25); 
						}
						ContextoImp.MoveTo(715,fila);					ContextoImp.Show(tomovalor1);//topo paciente
							
						//ContextoImp.MoveTo(725, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,10));//empresa
										
					}
					*/	
				//}
				/*
				Gnome.Print.Setfont (ContextoImp, fuente9);
				ContextoImp.MoveTo(99.5,fila);				ContextoImp.Show("TOTAL PROC. "+contadorprocedimientos.ToString());
				ContextoImp.MoveTo(100,fila);				ContextoImp.Show("TOTAL PROC. "+contadorprocedimientos.ToString());
				ContextoImp.MoveTo(219.5,fila);				ContextoImp.Show("TOT. SALDOS" );
				ContextoImp.MoveTo(220,fila);				ContextoImp.Show("TOT. SALDOS" );
				ContextoImp.MoveTo(295.5,fila);				ContextoImp.Show(sumacuenta.ToString("C"));
				ContextoImp.MoveTo(296,fila);				ContextoImp.Show(sumacuenta.ToString("C"));
				ContextoImp.MoveTo(384.5,fila);				ContextoImp.Show("TOT. ABONOS" );
				ContextoImp.MoveTo(385,fila);				ContextoImp.Show("TOT. ABONOS" );
				ContextoImp.MoveTo(459.5,fila);				ContextoImp.Show(totabono.ToString("C"));
				ContextoImp.MoveTo(460,fila);				ContextoImp.Show(totabono.ToString("C"));
				//contadorprocedimientos += 1;
				salto_pagina(ContextoImp,trabajoImpresion);
				*/
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
		{
			//Gtk.Image image5 = new Gtk.Image();
            //image5.Name = "image5";
			//image5.Pixbuf = new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "osiris.jpg"));
			//image5.Pixbuf = new Gdk.Pixbuf("/opt/osiris/bin/OSIRISLogo.jpg");   // en Linux
			//image5.Pixbuf.ScaleSimple(128, 128, Gdk.InterpType.Bilinear);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf,1,-30);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(145, 50, Gdk.InterpType.Bilinear),1,1);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(180, 64, Gdk.InterpType.Hyper),1,1);
			//cr.Fill();
			//cr.Paint();
			//cr.Restore();
								
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText(classpublic.nombre_empresa);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText(classpublic.direccion_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,25*escala_en_linux_windows);			layout.SetText(classpublic.telefonofax_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(650*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(650*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page :"+numpage.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 10.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(240*escala_en_linux_windows, 25*escala_en_linux_windows);			layout.SetText("REPORTE OCUPACION HOSPITALARIA");					Pango.CairoHelper.ShowLayout (cr, layout);
						
			// Creando el Cuadro de Titulos
			cr.Rectangle (05*escala_en_linux_windows, 50*escala_en_linux_windows, 750*escala_en_linux_windows, 15*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.5;
			cr.Stroke();
			
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(09*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("N° Aten.");			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(74*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Ingreso");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(114*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("N° Expe.");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(300*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Nombre Paciente");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Habitacion");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(480*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Saldo");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Abono");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("S. Pend.");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Doctor");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Diagnostico");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Tipo Pac.");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Empresa");	Pango.CairoHelper.ShowLayout (cr, layout);
			
			layout.FontDescription.Weight = Weight.Normal;		// Letra Normal
		}
		
		void salto_de_pagina(Cairo.Context cr,Pango.Layout layout)			
		{
			if(comienzo_linea >530){
				cr.ShowPage();
				Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
				fontSize = 8.0;		desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				comienzo_linea = 70;
				numpage += 1;
				imprime_encabezado(cr,layout);
			}
		}
			
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
	}
}
