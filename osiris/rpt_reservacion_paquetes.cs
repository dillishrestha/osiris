// rpt_reservacion_paquetes.cs created with MonoDevelop
// User: ipena at 06:19 p 10/06/2008
//
// Autor    	: Israel Peña Gonzalez - el_rip@hotmail.com (Programacion Mono)
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using Gtk;
using Npgsql;
using Cairo;
using Pango;

namespace osiris
{
	public class rpt_reservacion_paquete
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 162;
		int separacion_linea = 10;
		int numpage = 1;
		
		string nombrebd;
		string LoginEmpleado;
		string NomEmp_;
		string NomEmpleado = "";
		string AppEmpleado = "";
		string ApmEmpleado = "";
		
		string entry_dia1;
		string entry_mes1;
		string entry_anno1;
		string entry_nombre_paciente;
		string entry_paq_pres;
		string entry_precio_paquete;
		string cantidad_en_letras;
		
		string connectionString;
					
		class_public classpublic = new class_public();
		
		public rpt_reservacion_paquete(string entry_dia1_,string entry_mes1_,string entry_anno1_,string entry_nombre_paciente_,string entry_paq_pres_,string entry_precio_paquete_,string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_) 
		{
			entry_dia1 = entry_dia1_;
			entry_mes1 = entry_mes1_;
			entry_anno1 = entry_anno1_;
			entry_nombre_paciente = entry_nombre_paciente_;
			entry_paq_pres = entry_paq_pres_;
			entry_precio_paquete = entry_precio_paquete_;
			LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		nombrebd = _nombrebd_;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			
			print = new PrintOperation ();
			print.JobName = "Reservacion de Paquetes";
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);				
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
			PrintContext context = args.Context;
			ejecutar_consulta_reporte(context);
		}
		
		void ejecutar_consulta_reporte(PrintContext context)
		{
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;						
					
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
	}
}
