// rpt_reservacion_paquetes.cs created with MonoDevelop
// User: ipena at 06:19 p 10/06/2008
//
// Autor    	: Israel Peña Gonzalez - el_rip@hotmail.com (Programacion Mono)
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using Gnome;
using System.Collections;

namespace osiris
{
	public class rpt_reservacion_paquete
	{
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
		
		// Declarando variable de fuente para la impresion
		Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
			
		
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
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob(Gnome.PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog(trabajo, "RESERVACION", 0);
        	
        	int respuesta = dialogo.Run ();
        	
        	if (respuesta == (int) PrintButtons.Cancel) 
			{
				dialogo.Hide (); 
				dialogo.Dispose (); 
				return;
			}
			Gnome.PrintContext ctx = trabajo.Context;
        	ComponerPagina(ctx, trabajo); 
			trabajo.Close();
            switch (respuesta)
        	{
				case (int) PrintButtons.Print:   
                trabajo.Print (); 
                break;
                case (int) PrintButtons.Preview:
                new PrintJobPreview(trabajo, "RESERVACION").Show();
                break;
        	}
			dialogo.Hide (); dialogo.Dispose ();		
		}
		
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{ 
			ContextoImp.BeginPage("Pagina 1");
			
           	cantidad_en_letras = "";    //traduce_numeros(entry_precio_paquete);
		 	float preciopaqpresu = float.Parse(entry_precio_paquete);
			
			Gnome.Print.Setfont (ContextoImp, fuente2);          //Nombre del Usuario y/o demandante del servicio:  
			//ContextoImp.MoveTo(20, 710);         ContextoImp.Show("                                                                                                                           "                         +        entry_nombre_paciente);
			ContextoImp.MoveTo(325, 709);         ContextoImp.Show(entry_nombre_paciente);
			                                                     //nombre del paquete: 
			ContextoImp.MoveTo(190, 689);         ContextoImp.Show(entry_paq_pres);
			                                                     //v.-precio y forma de pago: $                                      +
			ContextoImp.MoveTo(195, 548);         ContextoImp.Show(preciopaqpresu.ToString("C"));                                                       //  + entry_precio_paquete);
			                                                     //(________________________________)
			ContextoImp.MoveTo(65, 538.6);       ContextoImp.Show(          cantidad_en_letras ); 
			                                                     //En Monterrey N.L. a                    de       "             del
			ContextoImp.MoveTo(392, 231);        ContextoImp.Show(entry_dia1);   
			ContextoImp.MoveTo(440, 231);        ContextoImp.Show(entry_mes1);
			ContextoImp.MoveTo(521, 231);        ContextoImp.Show(entry_anno1.Substring(3,1));
			
			
			ContextoImp.ShowPage();							
					
		}				     
	}
}
