// created on 00/06/2008 at 00:00 p
///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
//
// Monterrey - Mexico
//
// Autor    	: Marcos Irak Gaspar Avila (Programacion) ing.gaspar@gmail.com
// 				  Daniel Olivares Cuevas (Pre-Programacion, Colaboracion y Ajustes) arcangeldoc@gmail.com
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
using Gnome;
using Npgsql;
using System.Data;
using Glade;

namespace osiris
{
	public class rpt_solicitud_cheque
	{
		string nombredoctor = "";
		float hono_medico = 0;
		float descuento_hono_medico = 0;
		string cantidad_de_letras = "";
		string folio_servicio = "";
		float total_descuento = 0;
		string tipo_paciente = "";
		string nombre_paciente = "";
		string aseguradora = "";
		
		string nombrebd;
		string connectionString;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public rpt_solicitud_cheque(string nombredoctor_, string hono_medico_, string tipo_paciente_, string aseguradora_, string nombre_paciente_, string folio_servicio_)		
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			folio_servicio = folio_servicio_;
			nombre_paciente = nombre_paciente_;
			tipo_paciente = tipo_paciente_;
			aseguradora = aseguradora_;	
			nombredoctor = nombredoctor_;			
			hono_medico = float.Parse(hono_medico_);
			descuento_hono_medico = (hono_medico * 10 )  / 100;
			total_descuento = hono_medico - descuento_hono_medico;
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default ());
   		    Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, "Solicitud de cheques", 0);
       	    int respuesta = dialogo.Run ();

			if (respuesta == (int) PrintButtons.Cancel){   //boton Cancelar
				dialogo.Hide (); 
				dialogo.Dispose (); 
				return;
			}
	        Gnome.PrintContext ctx = trabajo.Context;
   	     	ComponerPagina(ctx, trabajo); 
	       	trabajo.Close();
			switch (respuesta)
			{   //imprimir
				case (int) PrintButtons.Print:   
					trabajo.Print (); 
				break;
				//vista previa
				case (int) PrintButtons.Preview:
					new PrintJobPreview(trabajo, "Solicitud de cheque").Show();
				break;
			}
			dialogo.Hide (); dialogo.Dispose ();
		}
		
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{ 
			for (int i1=0; i1 <= 1; i1++){
				ContextoImp.BeginPage("Pagina 1");
				
				cantidad_de_letras = classpublic.ConvertirCadena(total_descuento.ToString("F"),"PESO");
				Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
				//Gnome.Font fuente3 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
				
				// ESTA FUNCION ES PARA QUE EL TEXTO SALGA EN NEGRITA
				//Gnome.Font fuente4 = Gnome.Font.FindClosestFromWeightSlant("Bitstream Vera Sans", FontWeight.Bold ,false, 12);
				//Gnome.Font fuente5 = Gnome.Font.FindClosestFromWeightSlant("Bitstream Vera Sans", FontWeight.Bold ,false, 10);
				Gnome.Print.Setfont (ContextoImp, fuente2);
	
				ContextoImp.MoveTo(400, 780);			ContextoImp.Show(DateTime.Now.ToString("dd        MM     yyyy") );			 			
				ContextoImp.MoveTo(130, 740);			ContextoImp.Show(nombredoctor);
				ContextoImp.MoveTo(530, 710);			ContextoImp.Show("X" ); 
			    ContextoImp.MoveTo(70, 660);			ContextoImp.Show("Pagos de Honorarios Medicos / Folio :"+ folio_servicio.Trim()+" Paciente :"+nombre_paciente.Trim());
	            ContextoImp.MoveTo(100, 650);			ContextoImp.Show(tipo_paciente);
	            ContextoImp.MoveTo(70, 640);			ContextoImp.Show(aseguradora);
			    ContextoImp.MoveTo(130, 690);			ContextoImp.Show("("+ cantidad_de_letras + ")"); 
			    ContextoImp.MoveTo(405, 620);			ContextoImp.Show(hono_medico.ToString("C"));    
			    ContextoImp.MoveTo(460, 620);			ContextoImp.Show(descuento_hono_medico.ToString("C"));
			    ContextoImp.MoveTo(445, 620);			ContextoImp.Show(" - ");
			    ContextoImp.MoveTo(170, 720);			ContextoImp.Show(total_descuento.ToString("C"));
			    ContextoImp.MoveTo(370, 620);			ContextoImp.Show("Total:");
			    ContextoImp.MoveTo(500, 620);			ContextoImp.Show(total_descuento.ToString("C"));
				ContextoImp.ShowPage();
			}
		}
	}
}			
