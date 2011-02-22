//////////////////////////////////////////////////////////
// created on 08/02/2008 at 08:39 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
//				 
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
using Cairo;
using Pango;

namespace osiris
{
	public class imprime_consumo_productos
	{
		
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 70;
		int separacion_linea = 10;
		int numpage = 1;
		
		// Declarando el treeview
		Gtk.TreeView lista_resumen_productos;
		Gtk.TreeStore treeViewEngineResumen;
		
		string titulo = "REPORTE DE CONSUMO DE PRODUCTOS";
		
		string ano_consumo = "";
		
		class_public classpublic = new class_public();
		
		public imprime_consumo_productos(object _lista_resumen_productos_,object _treeViewEngineResumen_, string _ano_consumo_)
		{
			lista_resumen_productos = _lista_resumen_productos_ as Gtk.TreeView;
			treeViewEngineResumen =  _treeViewEngineResumen_ as Gtk.TreeStore;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			ano_consumo = _ano_consumo_;
			
			print = new PrintOperation ();
			print.JobName = titulo;	// Name of the report
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
			string toma_descrip_prod = "";
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			TreeIter iter;
			if (treeViewEngineResumen.GetIterFirst (out iter)){
				imprime_encabezado(cr,layout);
				toma_descrip_prod = (string) lista_resumen_productos.Model.GetValue (iter,1);
				if(toma_descrip_prod.Length > 61){
					toma_descrip_prod = toma_descrip_prod.Substring(0,60);
				}
				/*
				ContextoImp.MoveTo(120, filas);	ContextoImp.Show(toma_descrip_prod);
				ContextoImp.MoveTo(180, filas);	ContextoImp.Show((string) lista_resumen_productos.Model.GetValue (iter,2));
				// Meses
				ContextoImp.MoveTo(385, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,3)).ToString().Trim());
				ContextoImp.MoveTo(410, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,4)).ToString().Trim());
				ContextoImp.MoveTo(435, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,5)).ToString().Trim());
				ContextoImp.MoveTo(460, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,6)).ToString().Trim());
				ContextoImp.MoveTo(485, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,7)).ToString().Trim());
				ContextoImp.MoveTo(510, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,8)).ToString().Trim());
				ContextoImp.MoveTo(535, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,9)).ToString().Trim());
				ContextoImp.MoveTo(560, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,10)).ToString().Trim());
				ContextoImp.MoveTo(585, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,11)).ToString().Trim());
				ContextoImp.MoveTo(610, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,12)).ToString().Trim());
				ContextoImp.MoveTo(635, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,13)).ToString().Trim());
				ContextoImp.MoveTo(660, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,14)).ToString().Trim());
				ContextoImp.MoveTo(685, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,15)).ToString().Trim());
				ContextoImp.MoveTo(710, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,18)).ToString("F").Trim());
				ContextoImp.MoveTo(743, filas);	ContextoImp.Show((string) lista_resumen_productos.Model.GetValue (iter,16));
				*/
				while (treeViewEngineResumen.IterNext(ref iter)){
					if(toma_descrip_prod.Length > 61){
						toma_descrip_prod = toma_descrip_prod.Substring(0,60);
					}
					/*
					ContextoImp.MoveTo(120, filas);	ContextoImp.Show(toma_descrip_prod);
					ContextoImp.MoveTo(180, filas);	ContextoImp.Show((string) lista_resumen_productos.Model.GetValue (iter,2));
					// Meses
					ContextoImp.MoveTo(385, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,3)).ToString().Trim());
					ContextoImp.MoveTo(410, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,4)).ToString().Trim());
					ContextoImp.MoveTo(435, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,5)).ToString().Trim());
					ContextoImp.MoveTo(460, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,6)).ToString().Trim());
					ContextoImp.MoveTo(485, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,7)).ToString().Trim());
					ContextoImp.MoveTo(510, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,8)).ToString().Trim());
					ContextoImp.MoveTo(535, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,9)).ToString().Trim());
					ContextoImp.MoveTo(560, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,10)).ToString().Trim());
					ContextoImp.MoveTo(585, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,11)).ToString().Trim());
					ContextoImp.MoveTo(610, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,12)).ToString().Trim());
					ContextoImp.MoveTo(635, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,13)).ToString().Trim());
					ContextoImp.MoveTo(660, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,14)).ToString().Trim());
					ContextoImp.MoveTo(685, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,15)).ToString().Trim());
					ContextoImp.MoveTo(710, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,18)).ToString("F").Trim());
					ContextoImp.MoveTo(743, filas);	ContextoImp.Show((string) lista_resumen_productos.Model.GetValue (iter,16));
					*/
				}
			}
			
			
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
		{
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
			cr.MoveTo(240*escala_en_linux_windows, 25*escala_en_linux_windows);			layout.SetText(titulo);					Pango.CairoHelper.ShowLayout (cr, layout);
						
			// Creando el Cuadro de Titulos
			cr.Rectangle (05*escala_en_linux_windows, 50*escala_en_linux_windows, 750*escala_en_linux_windows, 15*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.5;
			cr.Stroke();
			
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			//cr.MoveTo(09*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("N° Aten.");			Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(74*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Ingreso");				Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(114*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("N° Expe.");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(300*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Nombre Paciente");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(400*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Habitacion");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(480*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Saldo");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Abono");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("S. Pend.");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Doctor");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Diagnostico");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Tipo Pac.");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Empresa");	Pango.CairoHelper.ShowLayout (cr, layout);
			/*
			ContextoImp.MoveTo(385, filas);	ContextoImp.Show("ENE");
			ContextoImp.MoveTo(410, filas);	ContextoImp.Show("FEB");
			ContextoImp.MoveTo(435, filas);	ContextoImp.Show("MAR");
			ContextoImp.MoveTo(460, filas);	ContextoImp.Show("ABR");
			ContextoImp.MoveTo(485, filas);	ContextoImp.Show("MAY");
			ContextoImp.MoveTo(510, filas);	ContextoImp.Show("JUN");
			ContextoImp.MoveTo(535, filas);	ContextoImp.Show("JUL");
			ContextoImp.MoveTo(560, filas);	ContextoImp.Show("AGO");
			ContextoImp.MoveTo(585, filas);	ContextoImp.Show("SEP");
			ContextoImp.MoveTo(610, filas);	ContextoImp.Show("OCT");
			ContextoImp.MoveTo(635, filas);	ContextoImp.Show("NOV");
			ContextoImp.MoveTo(660, filas);	ContextoImp.Show("DIC");
			ContextoImp.MoveTo(685, filas);	ContextoImp.Show("TOTAL");
			ContextoImp.MoveTo(710, filas);	ContextoImp.Show("PROME.");
			ContextoImp.MoveTo(743, filas);	ContextoImp.Show("$ TOTAL");
			*/
			
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
