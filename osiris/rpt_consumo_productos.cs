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
		
		int espacio_mese = 23;
		int comienzo_mese = 250;
		
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
				cr.MoveTo(09*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(toma_descrip_prod);			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(120*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lista_resumen_productos.Model.GetValue (iter,2));			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(comienzo_mese+(espacio_mese*1)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,3)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(comienzo_mese+(espacio_mese*2)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,4)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(comienzo_mese+(espacio_mese*3)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,5)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(comienzo_mese+(espacio_mese*4)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,6)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(comienzo_mese+(espacio_mese*5)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,7)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(comienzo_mese+(espacio_mese*6)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,8)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(comienzo_mese+(espacio_mese*7)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,9)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(comienzo_mese+(espacio_mese*8)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,10)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(comienzo_mese+(espacio_mese*9)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,11)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(comienzo_mese+(espacio_mese*10)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,12)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(comienzo_mese+(espacio_mese*11)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,13)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(comienzo_mese+(espacio_mese*12)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,14)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(685*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,15)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(710*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,18)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
								comienzo_linea += separacion_linea;
				while (treeViewEngineResumen.IterNext(ref iter)){
					toma_descrip_prod = (string) lista_resumen_productos.Model.GetValue (iter,1);
					if(toma_descrip_prod.Length > 61){
						toma_descrip_prod = toma_descrip_prod.Substring(0,60);
					}
					cr.MoveTo(09*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(toma_descrip_prod);			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(120*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lista_resumen_productos.Model.GetValue (iter,2));			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(comienzo_mese+(espacio_mese*1)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,3)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(comienzo_mese+(espacio_mese*2)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,4)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(comienzo_mese+(espacio_mese*3)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,5)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(comienzo_mese+(espacio_mese*4)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,6)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(comienzo_mese+(espacio_mese*5)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,7)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(comienzo_mese+(espacio_mese*6)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,8)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(comienzo_mese+(espacio_mese*7)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,9)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(comienzo_mese+(espacio_mese*8)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,10)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(comienzo_mese+(espacio_mese*9)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,11)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(comienzo_mese+(espacio_mese*10)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,12)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(comienzo_mese+(espacio_mese*11)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,13)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(comienzo_mese+(espacio_mese*12)*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,14)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(685*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,15)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(710*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,18)).ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
									
					comienzo_linea += separacion_linea;
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
			
			cr.MoveTo(09*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("ID Producto");			Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(74*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Ingreso");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(comienzo_mese+(espacio_mese*1)*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("ENE");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(comienzo_mese+(espacio_mese*2)*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("FEB");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(comienzo_mese+(espacio_mese*3)*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("MAR");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(comienzo_mese+(espacio_mese*4)*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("ABR");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(comienzo_mese+(espacio_mese*5)*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("MAY");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(comienzo_mese+(espacio_mese*6)*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("JUN");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(comienzo_mese+(espacio_mese*7)*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("JUL");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(comienzo_mese+(espacio_mese*8)*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("AGO");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(comienzo_mese+(espacio_mese*9)*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("SEP");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(comienzo_mese+(espacio_mese*10)*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("OCT");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(comienzo_mese+(espacio_mese*11)*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("NUV");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(comienzo_mese+(espacio_mese*12)*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("DIC");	Pango.CairoHelper.ShowLayout (cr, layout);
					
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