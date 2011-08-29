// created on 05/08/2011 at 09:06 am
//////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 am
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. (Programacion) arcangeldoc@gmail.com
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
	public class rpt_analisis_devoluciones
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		PrintContext context;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 105;
		int separacion_linea = 10;
		int numpage = 1;
		Pango.FontDescription desc;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		
		string nombrebd;
		string connectionString;
		
		string numeroatencion;
		string pidpaciente;
		string nombrepaciente;
		Gtk.TreeView lista_cargos_desde_stock = null;
		Gtk.TreeView lista_solicitado_no_cargado = null;
		Gtk.TreeView lista_solicitados_y_cargados = null;
		
		private TreeStore treeViewEngine1;
		private TreeStore treeViewEngine2;
		private TreeStore treeViewEngine3;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public rpt_analisis_devoluciones (object[] args,string numeroatencion_,string pidpaciente_,string nombrepaciente_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			
			lista_cargos_desde_stock = (object) args[0] as Gtk.TreeView;
			lista_solicitado_no_cargado = (object) args[1] as Gtk.TreeView;
			lista_solicitados_y_cargados = (object) args[2] as Gtk.TreeView;		
			treeViewEngine1 = (object) args[3] as Gtk.TreeStore;
			treeViewEngine2 = (object) args[4] as Gtk.TreeStore;
			treeViewEngine3 = (object) args[5] as Gtk.TreeStore;
			numeroatencion =numeroatencion_;
			pidpaciente= pidpaciente_;
			nombrepaciente = nombrepaciente_;
			
			print = new PrintOperation ();
			print.JobName = "Devolucion de Productos";	// Name of the report
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run(PrintOperationAction.PrintDialog, null);
		}
		
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1;  // crea cantidad de copias del reporte			
			// para imprimir horizontalmente el reporte
			//print.PrintSettings.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{			
			context = args.Context;			
			ejecutar_consulta_reporte(context);
		}
		
		void ejecutar_consulta_reporte(PrintContext context)
		{   
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout();
			desc = Pango.FontDescription.FromString ("Sans");
			
			imprime_encabezado(cr,layout);			
			fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;
			
			string descripcion_producto_aplicado = "";
			TreeIter iter;
			if (treeViewEngine1.GetIterFirst (out iter)){
				layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Producto Cargados Desde el Stock del Sub-Almacen (sin solicitud)");	Pango.CairoHelper.ShowLayout (cr, layout);
				comienzo_linea += separacion_linea;
				layout.FontDescription.Weight = Weight.Normal;		// Letra normal
				descripcion_producto_aplicado = (string) lista_cargos_desde_stock.Model.GetValue (iter,1).ToString().Trim();
				if(descripcion_producto_aplicado.Length > 62) { descripcion_producto_aplicado = descripcion_producto_aplicado.Substring(0,62); }
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_cargos_desde_stock.Model.GetValue (iter,0).ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(70*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText(descripcion_producto_aplicado);	Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(360*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_cargos_desde_stock.Model.GetValue (iter,2).ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);	
				cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_cargos_desde_stock.Model.GetValue (iter,3).ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(440*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_cargos_desde_stock.Model.GetValue (iter,4).ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(500*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_cargos_desde_stock.Model.GetValue (iter,5).ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
				comienzo_linea += separacion_linea;
				while (treeViewEngine1.IterNext(ref iter)){
					descripcion_producto_aplicado = (string) lista_cargos_desde_stock.Model.GetValue (iter,1).ToString().Trim();
					if(descripcion_producto_aplicado.Length > 62) { descripcion_producto_aplicado = descripcion_producto_aplicado.Substring(0,62); }
					cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_cargos_desde_stock.Model.GetValue (iter,0));	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(70*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText(descripcion_producto_aplicado);	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(360*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_cargos_desde_stock.Model.GetValue (iter,2).ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);	
					cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_cargos_desde_stock.Model.GetValue (iter,3).ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(440*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_cargos_desde_stock.Model.GetValue (iter,4).ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(500*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_cargos_desde_stock.Model.GetValue (iter,5).ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
					comienzo_linea += separacion_linea;
					salto_de_pagina(cr,layout);
				}
			}
			
			if (treeViewEngine2.GetIterFirst (out iter)){
				comienzo_linea += separacion_linea;
				salto_de_pagina(cr,layout);
				comienzo_linea += separacion_linea;
				salto_de_pagina(cr,layout);
				layout.FontDescription.Weight = Weight.Bold;
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Producto Solicitados y NO Cargados");	Pango.CairoHelper.ShowLayout (cr, layout);
				layout.FontDescription.Weight = Weight.Normal;
				comienzo_linea += separacion_linea;
				salto_de_pagina(cr,layout);
				while (treeViewEngine2.IterNext(ref iter)){
					descripcion_producto_aplicado = (string) lista_solicitado_no_cargado.Model.GetValue (iter,1).ToString().Trim();
					if(descripcion_producto_aplicado.Length > 62) { descripcion_producto_aplicado = descripcion_producto_aplicado.Substring(0,62); }
					cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_solicitado_no_cargado.Model.GetValue (iter,0));	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(70*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText(descripcion_producto_aplicado);	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(360*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_solicitado_no_cargado.Model.GetValue (iter,2).ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);	
					cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_solicitado_no_cargado.Model.GetValue (iter,3).ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(440*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_solicitado_no_cargado.Model.GetValue (iter,4).ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(500*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_solicitado_no_cargado.Model.GetValue (iter,5).ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
					comienzo_linea += separacion_linea;
					salto_de_pagina(cr,layout);
				}
			}
			if (treeViewEngine3.GetIterFirst (out iter)){
				comienzo_linea += separacion_linea;
				salto_de_pagina(cr,layout);
				comienzo_linea += separacion_linea;
				salto_de_pagina(cr,layout);
				layout.FontDescription.Weight = Weight.Bold;
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Producto Solicitados y Cargados (Devoluciones)");	Pango.CairoHelper.ShowLayout (cr, layout);
				layout.FontDescription.Weight = Weight.Normal;
				comienzo_linea += separacion_linea;
				salto_de_pagina(cr,layout);
				while (treeViewEngine3.IterNext(ref iter)){
					descripcion_producto_aplicado = (string) lista_solicitados_y_cargados.Model.GetValue (iter,1).ToString().Trim();
					if(descripcion_producto_aplicado.Length > 62) { descripcion_producto_aplicado = descripcion_producto_aplicado.Substring(0,62); }
					cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_solicitados_y_cargados.Model.GetValue (iter,0));	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(70*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText(descripcion_producto_aplicado);	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(360*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_solicitados_y_cargados.Model.GetValue (iter,2).ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);	
					cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_solicitados_y_cargados.Model.GetValue (iter,3).ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(440*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_solicitados_y_cargados.Model.GetValue (iter,4).ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(500*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lista_solicitados_y_cargados.Model.GetValue (iter,5).ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
					comienzo_linea += separacion_linea;
					salto_de_pagina(cr,layout);
				}				
			}
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
		{
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra normal			
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			cr.MoveTo(05*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText(classpublic.nombre_empresa);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText(classpublic.direccion_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,25*escala_en_linux_windows);			layout.SetText(classpublic.telefonofax_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(479*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(479*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page :"+numpage.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 10.0;		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			layout.Alignment = Pango.Alignment.Center;			
			double width = context.Width;
			layout.Width = (int) width;
			layout.Alignment = Pango.Alignment.Center;
			//layout.Wrap = Pango.WrapMode.Word;
			//layout.SingleParagraphMode = true;
			layout.Justify =  false;
			cr.MoveTo(width/2,45*escala_en_linux_windows);	layout.SetText("DEVOLUCIONES");	Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra normal
			cr.MoveTo(05*escala_en_linux_windows,65*escala_en_linux_windows);		layout.SetText("N° Atencion: "+numeroatencion);					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(120*escala_en_linux_windows,65*escala_en_linux_windows);		layout.SetText("N° Expe.: "+pidpaciente);						Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(220*escala_en_linux_windows,65*escala_en_linux_windows);		layout.SetText("Nombre Paciente: "+nombrepaciente);				Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			cr.MoveTo(05*escala_en_linux_windows,75*escala_en_linux_windows);		layout.SetText("Procedimiento: ");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(300*escala_en_linux_windows,75*escala_en_linux_windows);		layout.SetText("Diagnostico: ");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.Rectangle (05*escala_en_linux_windows, 85*escala_en_linux_windows, 565*escala_en_linux_windows, 15*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.5;
			cr.Stroke();
			layout.FontDescription.Weight = Weight.Bold;		// Letra normal
			cr.MoveTo(18*escala_en_linux_windows, 88*escala_en_linux_windows);			layout.SetText("Codigo");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(150*escala_en_linux_windows, 88*escala_en_linux_windows);			layout.SetText("Descripción Producto");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(350*escala_en_linux_windows, 88*escala_en_linux_windows);			layout.SetText("Cargado");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(390*escala_en_linux_windows, 88*escala_en_linux_windows);			layout.SetText("Solicitado");		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(435*escala_en_linux_windows, 88*escala_en_linux_windows);			layout.SetText("Devolucion");		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(500*escala_en_linux_windows, 88*escala_en_linux_windows);			layout.SetText("Departamento");		Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal		
		}
		
		void salto_de_pagina(Cairo.Context cr,Pango.Layout layout)			
		{
			if(comienzo_linea > 700){
				cr.ShowPage();
				comienzo_linea = 105;
				imprime_encabezado(cr,layout);
			}
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
	}
}

