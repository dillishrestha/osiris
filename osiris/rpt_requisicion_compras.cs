///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
// created on 10/10/2008 at 10:21 a
// 				: Ing. R. Israel Peña Gonzalez
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
/////////////////////////////////////////////////////////
using System;
using Gtk;
using Npgsql;
using Cairo;
using Pango;

namespace osiris
{
	public class rpt_requisicion_compras
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 162;
		int separacion_linea = 10;
		int numpage = 1;
		
		string connectionString;						
		string nombrebd;
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
    	
    	string querylist = "";
    	
    	// Declarando el treeview
		Gtk.TreeView lista_requisicion_productos;
		Gtk.TreeStore treeViewEngineRequisicion;
    		
    	string titulo = "REQUISICION DE COMPRAS ";
    	string numero_requisicion;
		string status_requisicion;
		string fecha_solicitud;
		string fecha_requerida;
		string observaciones;
		string totalitems_productos;
		string solicitado_por;
		string motivo_de_requi;
		string nombre_proveedor1;
		string nombre_proveedor2;
		string nombre_proveedor3;
		
		string descripcion_tipo_requi;
    	string descripinternamiento;
		string descripinternamiento2;
		string nombrepaciente;
		string folioservicio;
		string pidpaciente;
    				
		//Declaracion de ventana de error:
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
	
		public rpt_requisicion_compras(string nombrebd_,string numero_requisicion_,string status_requisicion_,string fecha_solicitud_,string fecha_requerida_,
									string observaciones_,string totalitems_productos_,
									object lista_requisicion_productos_,object treeViewEngineRequisicion_,string solicitado_por_,string motivo_de_requi_,
		                            string nombre_proveedor1_,string nombre_proveedor2_,string nombre_proveedor3_,
		                            string descripcion_tipo_requi_,
		                            string descripinternamiento_,string descripinternamiento2_,string nombrepaciente_,string folioservicio_,string pidpaciente_)
    	{
    	 	connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
    	 	numero_requisicion = numero_requisicion_;
    		status_requisicion = status_requisicion_;
    		fecha_solicitud = fecha_solicitud_;
    		fecha_requerida = fecha_requerida_;
    		observaciones = observaciones_;
    		lista_requisicion_productos = lista_requisicion_productos_ as Gtk.TreeView;
    		treeViewEngineRequisicion = treeViewEngineRequisicion_ as Gtk.TreeStore;
			solicitado_por = solicitado_por_;
			motivo_de_requi = motivo_de_requi_;
			nombre_proveedor1 = nombre_proveedor1_;
			nombre_proveedor2 = nombre_proveedor2_;
			nombre_proveedor3 = nombre_proveedor3_;
			descripcion_tipo_requi = descripcion_tipo_requi_;
			descripinternamiento = descripinternamiento_;
			descripinternamiento2 = descripinternamiento2_;
			nombrepaciente = nombrepaciente_;
			folioservicio = folioservicio_;
			pidpaciente = pidpaciente_;
			
			print = new PrintOperation ();
			print.JobName = titulo;
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);
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
			//string nombre = "";
			int contador = 1;
			//TreeModel model;
			TreeIter iter;
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			imprime_encabezado(cr,layout);
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;					
			if (this.treeViewEngineRequisicion.GetIterFirst (out iter)){			
				// N. partida
				cr.MoveTo(07*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(contador.ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
				// cantidad solicitada
				cr.MoveTo(27*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) this.lista_requisicion_productos.Model.GetValue (iter,0).ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
				// unidad de medida
				cr.MoveTo(60*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) this.lista_requisicion_productos.Model.GetValue (iter,4).ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
				// Embalaje
				cr.MoveTo(102*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) this.lista_requisicion_productos.Model.GetValue (iter,3).ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
				// Descripcion del Producto
				cr.MoveTo(140*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) this.lista_requisicion_productos.Model.GetValue (iter,1).ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
				desc = Pango.FontDescription.FromString ("Courier New");									
				fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
				desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
				// Precio del Producto
				cr.MoveTo(477*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",float.Parse((string) this.lista_requisicion_productos.Model.GetValue (iter,22).ToString().Trim())));					Pango.CairoHelper.ShowLayout (cr, layout);	
				desc = Pango.FontDescription.FromString ("Sans");									
				fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
				desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
				
				contador += 1;
				comienzo_linea += separacion_linea;		
				while(this.treeViewEngineRequisicion.IterNext (ref iter)){
					// N. partida
					cr.MoveTo(07*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(contador.ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
					// Cantidad Solicitada
					cr.MoveTo(27*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) this.lista_requisicion_productos.Model.GetValue (iter,0).ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
					// unidad de medida
					cr.MoveTo(60*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) this.lista_requisicion_productos.Model.GetValue (iter,4).ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
					// Embalaje
					cr.MoveTo(102*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) this.lista_requisicion_productos.Model.GetValue (iter,3).ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
					// Descripcion del Producto
					cr.MoveTo(140*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) this.lista_requisicion_productos.Model.GetValue (iter,1).ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
					
					desc = Pango.FontDescription.FromString ("Courier New");									
					// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
					fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
					desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
					// Precio del Producto
					cr.MoveTo(477*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",float.Parse((string) this.lista_requisicion_productos.Model.GetValue (iter,22).ToString().Trim())));					Pango.CairoHelper.ShowLayout (cr, layout);
					
					desc = Pango.FontDescription.FromString ("Sans");									
					// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
					fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
					desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
					contador += 1;
					comienzo_linea += separacion_linea;
					salto_de_pagina(cr,layout);
				}
			}
		}
				
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
		{
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
			cr.MoveTo(290*escala_en_linux_windows, 25*escala_en_linux_windows);			layout.SetText("REQUISICION DE COMPRAS");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(05*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(05,550);		// vertical 1
			
			cr.MoveTo(750*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(750,550);		// vertical 2
			
			cr.MoveTo(550*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(550,140);		// vertical 3
			
			cr.MoveTo(650*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(650,140);		// vertical 4
			
			cr.MoveTo(25*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(25,510);		// vertical 5
			
			cr.MoveTo(57*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(57,510);		// vertical 6
			
			cr.MoveTo(100*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(100,510);		// vertical 7
			
			cr.MoveTo(138*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(138,510);		// vertical 8
			
			cr.MoveTo(475*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(475,550);		// vertical 9
			
			cr.MoveTo(530*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(530,550);		// vertical 10
			
			cr.MoveTo(585*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(585,550);		// vertical 11
			
			cr.MoveTo(640*escala_en_linux_windows, 150*escala_en_linux_windows);
			cr.LineTo(640,510);		// vertical 12
			
			cr.MoveTo(695*escala_en_linux_windows, 150*escala_en_linux_windows);
			cr.LineTo(695,510);		// vertical 13
			
			cr.MoveTo(420*escala_en_linux_windows, 510*escala_en_linux_windows);
			cr.LineTo(420,550);		// vertical 14
			
			cr.MoveTo(750*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(05,60);		// Linea Horizontal 1
			
			cr.MoveTo(750*escala_en_linux_windows, 100*escala_en_linux_windows);
			cr.LineTo(05,100);		// Linea Horizontal 2
			
			cr.MoveTo(750*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(05,140);		// Linea Horizontal 3
			
			cr.MoveTo(750*escala_en_linux_windows, 160*escala_en_linux_windows);
			cr.LineTo(05,160);		// Linea Horizontal 4
			
			cr.MoveTo(750*escala_en_linux_windows, 510*escala_en_linux_windows);
			cr.LineTo(05,510);		// Linea Horizontal 5
			
			cr.MoveTo(750*escala_en_linux_windows, 550*escala_en_linux_windows);
			cr.LineTo(05,550);		// Linea Horizontal 6
			
			cr.MoveTo(750*escala_en_linux_windows, 150*escala_en_linux_windows);
			cr.LineTo(585,150);		// Linea Horizontal 6
			
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.3;
			cr.Stroke();
			
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(07*escala_en_linux_windows, 62*escala_en_linux_windows);			layout.SetText("SOLICITANTE");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(250*escala_en_linux_windows, 62*escala_en_linux_windows);			layout.SetText("DEPARTAMENTO");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(420*escala_en_linux_windows, 62*escala_en_linux_windows);			layout.SetText("CUENTA DE CARGO");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(555*escala_en_linux_windows, 62*escala_en_linux_windows);			layout.SetText("Fecha Requisicion");					Pango.CairoHelper.ShowLayout (cr, layout);
						
			cr.MoveTo(07*escala_en_linux_windows, 85*escala_en_linux_windows);			layout.SetText("Motivo Requisicion:");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(555*escala_en_linux_windows, 105*escala_en_linux_windows);		layout.SetText("Fecha Requerida");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(655*escala_en_linux_windows, 105*escala_en_linux_windows);			layout.SetText("Tipo de Requisicion");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows, 105*escala_en_linux_windows);			layout.SetText("Observacion: "+status_requisicion);					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows, 125*escala_en_linux_windows);			layout.SetText("Paciente: ");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(370*escala_en_linux_windows, 125*escala_en_linux_windows);			layout.SetText("N° Atencion: ");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(460*escala_en_linux_windows, 125*escala_en_linux_windows);			layout.SetText("N° Exp.: ");					Pango.CairoHelper.ShowLayout (cr, layout);

			fontSize = 9.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(655*escala_en_linux_windows, 62*escala_en_linux_windows);		layout.SetText("N° REQUISICION");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(660*escala_en_linux_windows, 72*escala_en_linux_windows);		layout.SetText(numero_requisicion);					Pango.CairoHelper.ShowLayout (cr, layout);

			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal		
			
			cr.MoveTo(07*escala_en_linux_windows,72*escala_en_linux_windows);			layout.SetText(solicitado_por);					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(250*escala_en_linux_windows,72*escala_en_linux_windows);			layout.SetText(descripinternamiento);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,72*escala_en_linux_windows);			layout.SetText(descripinternamiento2);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(555*escala_en_linux_windows, 72*escala_en_linux_windows);			layout.SetText(fecha_solicitud);				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(90*escala_en_linux_windows,85*escala_en_linux_windows);			layout.SetText(motivo_de_requi.ToUpper());				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(555*escala_en_linux_windows, 115*escala_en_linux_windows);		layout.SetText(fecha_requerida);				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(655*escala_en_linux_windows, 115*escala_en_linux_windows);			layout.SetText(descripcion_tipo_requi);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows, 115*escala_en_linux_windows);			layout.SetText(observaciones.ToUpper());					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(50*escala_en_linux_windows, 125*escala_en_linux_windows);			layout.SetText(nombrepaciente);					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(425*escala_en_linux_windows, 125*escala_en_linux_windows);			layout.SetText(folioservicio);					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(495*escala_en_linux_windows, 125*escala_en_linux_windows);			layout.SetText(pidpaciente);					Pango.CairoHelper.ShowLayout (cr, layout);

			cr.MoveTo(07*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("N°");							Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("Part.");						Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(07*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("100");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(27*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("Cantid.");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(27*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText(" Soli.");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(27*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("1000.00");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(60*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("Unidad de");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(60*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("Medida");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(60*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("PIEZA");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(102*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("Empaque");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(102*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("Produc.");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(102*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("1000.00");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(140*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("Descripcion del");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(140*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("Producto");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(140*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("BOLSA RECOLECTORA DE ORINA UROTEK DE 2 LTS.");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(477*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText(" Precio");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(477*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("Unitario");					Pango.CairoHelper.ShowLayout (cr, layout);
						
			cr.MoveTo(532*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("Importe");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(642*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("COTIZACIONES");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(587*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("PROV. A");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(642*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("PROV. B");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(697*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("PROV. C");					Pango.CairoHelper.ShowLayout (cr, layout);
						
			cr.MoveTo(07*escala_en_linux_windows, 515*escala_en_linux_windows);			layout.SetText("Proveedor A: "+nombre_proveedor1);					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows, 525*escala_en_linux_windows);			layout.SetText("Proveedor B:");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows, 535*escala_en_linux_windows);			layout.SetText("Proveedor C:");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(422*escala_en_linux_windows, 515*escala_en_linux_windows);			layout.SetText("SUB-TOTAL");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(422*escala_en_linux_windows, 525*escala_en_linux_windows);			layout.SetText(classpublic.ivaparaaplicar+"% I.V.A.");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(422*escala_en_linux_windows, 535*escala_en_linux_windows);			layout.SetText("TOTAL");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(587*escala_en_linux_windows, 515*escala_en_linux_windows);			layout.SetText("Fecha y Hora Recibido en Dep. Compras");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(55*escala_en_linux_windows, 565*escala_en_linux_windows);			layout.SetText("SOLICITANTE");					Pango.CairoHelper.ShowLayout (cr, layout);			
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);			layout.FontDescription = desc;
		}
		
		void salto_de_pagina(Cairo.Context cr,Pango.Layout layout)           
        {
            if(comienzo_linea >510){
                cr.ShowPage();
                Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");                               
                fontSize = 8.0;        desc.Size = (int)(fontSize * pangoScale);                    layout.FontDescription = desc;
                comienzo_linea = 162;
                numpage += 1;
                imprime_encabezado(cr,layout);				
            }
        }
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
   	}
}