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
		int comienzo_linea = 70;
		int separacion_linea = 10;
		int numpage = 1;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		
		string nombrebd;
		string connectionString;
		
		Gtk.ListStore treeViewEnginegrupos;
		Gtk.TreeView lista_grupo;
		Gtk.ListStore treeViewEnginegrupos1;
		Gtk.TreeView lista_grupo1;
		Gtk.ListStore treeViewEnginegrupos2;
		Gtk.TreeView lista_grupo2;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public rpt_analisis_devoluciones ()
		{
			/*
			treeViewEnginegrupos = treeview_Engine_grupos_ as Gtk.ListStore;
			lista_grupo = lista_grupo_ as Gtk.TreeView;
			
			treeViewEnginegrupos1 = treeview_Engine_grupos1_ as Gtk.ListStore;
			lista_grupo1 = lista_grupo1_ as Gtk.TreeView;
			
			treeViewEnginegrupos2 = treeview_Engine_grupos2_ as Gtk.ListStore;			
			lista_grupo2 = lista_grupo2_ as Gtk.TreeView; 
			*/
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
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
			Pango.Layout layout = context.CreatePangoLayout ();
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
		
	}
}

