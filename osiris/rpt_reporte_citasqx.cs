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
using System;
using Gtk;
using Npgsql;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class rpt_reporte_citasqx
	{
		string connectionString;
        string nombrebd;
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int separacionlineas = 12;
		
		Gtk.TreeView treeview_lista_agenda = null;
		TreeStore treeViewEngineListaCitas = null;
		
		class_public classpublic = new class_public();
		
		public rpt_reporte_citasqx (object _treeview_lista_agenda_, object _treeViewEngineListaCitas_)
		{
			escala_en_linux_windows = classpublic.escala_linux_windows;
			treeview_lista_agenda = _treeview_lista_agenda_ as Gtk.TreeView;
			treeViewEngineListaCitas = _treeViewEngineListaCitas_ as Gtk.TreeStore;
			
			print = new PrintOperation ();
						
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);
		}
		
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			PrintContext context = args.Context;											
			print.NPages = 1;  // crea cantidad de copias del reporte			
			// para imprimir horizontalmente el reporte
			print.PrintSettings.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{			
			PrintContext context = args.Context;
			
			ejecutar_consulta_reporte(context);
			
			// crea una pagina nueva
			/*
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");
			
			cr.ShowPage();
			layout = null;
			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);
			layout.FontDescription = desc;
			cr.MoveTo(100,100);	layout.SetText("Prueba de Impresion--------------------------------");		Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.ShowPage();
			cr.MoveTo(100,100);	layout.SetText("Prueba de Impresion--------------------------------");		Pango.CairoHelper.ShowLayout (cr, layout);*/
		}
		
		void ejecutar_consulta_reporte(PrintContext context)
		{
			imprime_encabezado(context);
			int comienzo_linea = 70;
			int separacion_linea = 10;
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;			
			
			TreeIter iter;
			if (treeViewEngineListaCitas.GetIterFirst (out iter)){
				
				//ContextoImp.Show();
				layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,0));			Pango.CairoHelper.ShowLayout (cr, layout);
				layout.FontDescription.Weight = Weight.Normal;		// Letra normal
				comienzo_linea += separacion_linea;
				
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,1));			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(34*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,2));			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(74*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,3));			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(114*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,4));			Pango.CairoHelper.ShowLayout (cr, layout);
				//cr.MoveTo(114*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,5));			Pango.CairoHelper.ShowLayout (cr, layout);
				//cr.MoveTo(300*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,6));			Pango.CairoHelper.ShowLayout (cr, layout);
				//cr.MoveTo(114*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,7));			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(300*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,8));			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,9));			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(480*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,12));			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(570*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,4));			Pango.CairoHelper.ShowLayout (cr, layout);
				comienzo_linea += separacion_linea;
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("Motivo Consulta :"+(string) treeview_lista_agenda.Model.GetValue (iter,13));			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(253*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("Observaciones :"+(string) treeview_lista_agenda.Model.GetValue (iter,14));			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(501*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("Referido por:"+(string) treeview_lista_agenda.Model.GetValue (iter,15));			Pango.CairoHelper.ShowLayout (cr, layout);
				comienzo_linea += separacion_linea;
				comienzo_linea += separacion_linea;
				
				while (treeViewEngineListaCitas.IterNext(ref iter)){
					cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,1));			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(34*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,2));			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(74*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,3));			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(114*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,4));			Pango.CairoHelper.ShowLayout (cr, layout);
					//cr.MoveTo(114*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,5));			Pango.CairoHelper.ShowLayout (cr, layout);
					//cr.MoveTo(300*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,6));			Pango.CairoHelper.ShowLayout (cr, layout);
					//cr.MoveTo(114*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,7));			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(300*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,8));			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,9));			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(480*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,12));			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(570*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) treeview_lista_agenda.Model.GetValue (iter,4));			Pango.CairoHelper.ShowLayout (cr, layout);
					comienzo_linea += separacion_linea;
					cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("Mot.Consulta:"+(string) treeview_lista_agenda.Model.GetValue (iter,13));			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(253*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("Obse.:"+(string) treeview_lista_agenda.Model.GetValue (iter,14));			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(501*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("Ref. por:"+(string) treeview_lista_agenda.Model.GetValue (iter,15));			Pango.CairoHelper.ShowLayout (cr, layout);
					comienzo_linea += separacion_linea;
					comienzo_linea += separacion_linea;
				}
			}
		}
		
		void imprime_encabezado(PrintContext context)
		{
			//Console.WriteLine("entra en la impresion del encabezado");
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			context.PageSetup.Orientation = PageOrientation.Landscape;
			
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
			fontSize = 8.0;		layout = null;							layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText(classpublic.nombre_empresa);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText(classpublic.direccion_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,25*escala_en_linux_windows);			layout.SetText(classpublic.telefonofax_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;		layout = null;							layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 10.0;											layout = context.CreatePangoLayout ();		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(230*escala_en_linux_windows, 25*escala_en_linux_windows);			layout.SetText("CALENDARIO DE CITAS Y CIRUGIAS PROGRAMADAS");					Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra Normal
			fontSize = 8.0;		layout = null;							layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			
			// Creando el Cuadro de Titulos
			cr.Rectangle (05*escala_en_linux_windows, 50*escala_en_linux_windows, 750*escala_en_linux_windows, 15*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.5;
			cr.Stroke();
			
			cr.MoveTo(09*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Fech/Hora");			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(74*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("N° Exp.");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(114*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Nombre del Paciente");	Pango.CairoHelper.ShowLayout (cr, layout);

		}		
			
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
	}
}
