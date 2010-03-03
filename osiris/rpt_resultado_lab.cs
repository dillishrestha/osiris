// created on 05/02/2008 at 06:54 p
///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
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
// Programa		: hscmty.cs
// Proposito	: Impresion del procedimiento de cobranza 
// Objeto		: rpt_resultado_lab.cs
using System;
using Gtk;
using Gnome;
using Npgsql;
using System.Data;
using Glade;
using System.Collections;
using GtkSharp;

namespace osiris
{
	public class imprime_resultadolab
	{
		string connectionString;
        string nombrebd;
		int PidPaciente = 0;
		int folioservicio = 0;
		string LoginEmpleado;
		string dir_paciente;
		string edadpac;
		string empresapac;
		string folio_laboratorio;
		string fecha_solucitud;
		string nombre_paciente;
		string quimicoautorizado;
		string fecha_nac;
		string tipo_examen;
		string tipo_paciente;
		string hora_solicitud_estudio;
		string sexopaciente;
		string procedencia;
		string medicotratante;
		string nombre_estudio;
		string observa;
		string cedulaquimico;
		bool checkbutton_parametros;
		
		Gtk.ListStore treeViewEngineresultados;
		Gtk.TreeView lista_de_resultados;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		//public System.Drawing.Image myimage;
		
		class_conexion conexion_a_DB = new class_conexion();
	
		public imprime_resultadolab (object _lista_de_resultados_,object _treeViewEngineresultados_,string _LoginEmpleado_,string nombrebd_,
									string _dir_paciente_,string _edadpac_,string _empresapac_,string entry_folio_laboratorio_res,
									string _entry_fecha_solicitud_res_,int PidPaciente_,string _entry_nombre_paciente_,
									string _quimicoaut_,int _folioservicio_,string _fecha_nacimiento_,
									string _tipo_examen_,string _entry_tipo_paciente_,string _hora_solicitud_estudio_,
									string _sexopaciente_, string _procedencia_,string _medicotratante_,string _nombre_estudio_,
									string _observa_,string _cedulaquimico_)
		{
			lista_de_resultados = _lista_de_resultados_ as Gtk.TreeView;
			treeViewEngineresultados = _treeViewEngineresultados_ as Gtk.ListStore;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;			
			PidPaciente = PidPaciente_;
			folioservicio = _folioservicio_;
			LoginEmpleado = _LoginEmpleado_;
			dir_paciente = _dir_paciente_;
			edadpac = _edadpac_;
			empresapac = _empresapac_;
			folio_laboratorio = entry_folio_laboratorio_res;
			fecha_solucitud = _entry_fecha_solicitud_res_;
			nombre_paciente = _entry_nombre_paciente_;
			quimicoautorizado = _quimicoaut_;
			fecha_nac = _fecha_nacimiento_;
			tipo_examen = _tipo_examen_;
			tipo_paciente = _entry_tipo_paciente_;
			hora_solicitud_estudio = _hora_solicitud_estudio_;
			sexopaciente = _sexopaciente_ ;
			procedencia = _procedencia_;
			medicotratante = _medicotratante_;
			nombre_estudio = _nombre_estudio_;
			observa = _observa_;
			cedulaquimico = _cedulaquimico_;
			checkbutton_parametros = true; //_checkbutton_parametros_;
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, "RESULTADOS DE LABORATORIO", 0);
        	int         respuesta = dialogo.Run ();
        
			if (respuesta == (int) PrintButtons.Cancel)	{
				dialogo.Hide (); 		dialogo.Dispose (); 
				return;
			}

        	Gnome.PrintContext ctx = trabajo.Context;
        
        	ComponerPagina(ctx, trabajo); 

        	trabajo.Close();
             
        	switch (respuesta){
				case (int) PrintButtons.Print:
					trabajo.Print (); 
					break;
				case (int) PrintButtons.Preview:
					new PrintJobPreview(trabajo, "RESULTADOS DE EXAMENES").Show();
					break;
        	}
			dialogo.Hide (); dialogo.Dispose ();
       }
      
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
		
			ContextoImp.BeginPage("Pagina 1");
			//NUEVO
			// Crear una fuente de tipo Impact
			
			Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
			Gnome.Font fuente3 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
	    	Gnome.Font fuente4 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
	    	Gnome.Font fuente5 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
	    	
			//se cambia el tamaño de texto por ser titulo
	    	int linea = 720;	    	
	    	
			Gnome.Print.Setfont (ContextoImp, fuente2);
			ContextoImp.MoveTo(225.5, linea);
			ContextoImp.Show("RESULTADOS DE LABORATORIO");
			ContextoImp.MoveTo(225, linea);
			ContextoImp.Show("RESULTADOS DE LABORATORIO");
	    
	    	Gnome.Print.Setfont (ContextoImp, fuente3);
	    
			ContextoImp.MoveTo(490, linea);					ContextoImp.Show("Folio LAB. "+this.folio_laboratorio.Trim() );
			ContextoImp.MoveTo(490, linea-10);				ContextoImp.Show("PID "+PidPaciente.ToString().Trim());
		
			Gnome.Print.Setfont (ContextoImp, fuente5);
			ContextoImp.MoveTo(20, linea-13);  				ContextoImp.Show("____________________________");
    	
    		Gnome.Print.Setfont (ContextoImp, fuente4);
    		ContextoImp.MoveTo(239.5, linea-25);		
    		ContextoImp.Show("DATOS GENERALES DEL PACIENTE");
    		ContextoImp.MoveTo(240, linea-25);		
    		ContextoImp.Show("DATOS GENERALES DEL PACIENTE");
    												
			ContextoImp.MoveTo(19.5, linea-35.5);		ContextoImp.Show("Tipo de paciente:  "+this.tipo_paciente);
			ContextoImp.MoveTo(20, linea-35);			ContextoImp.Show("Tipo de paciente:  "+this.tipo_paciente);
			ContextoImp.MoveTo(250, linea-35);			ContextoImp.Show("Folio Atencion: "+this.folioservicio.ToString().Trim());
			ContextoImp.MoveTo(250.5, linea-35);		ContextoImp.Show("Folio Atencion: "+this.folioservicio.ToString().Trim());
			ContextoImp.MoveTo(420, linea-35);			ContextoImp.Show("Fecha/Hora Solicitud: "+this.fecha_solucitud+" / "+this.hora_solicitud_estudio);
													
			ContextoImp.MoveTo(20, linea-45);			ContextoImp.Show("Nombre Paciente: "+nombre_paciente);
											
			ContextoImp.MoveTo(290, linea-45);			ContextoImp.Show("F. de Nac.: "+fecha_nac);
			ContextoImp.MoveTo(420, linea-45);			ContextoImp.Show("Fecha/Hora Reporte: "+DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
			
			ContextoImp.MoveTo(20, linea-55);			ContextoImp.Show("Tipo de Examen: "+this.tipo_examen);
			ContextoImp.MoveTo(290, linea-55);			ContextoImp.Show("Edad : "+this.edadpac);
			
			if(sexopaciente == "H"){
				ContextoImp.MoveTo(420, linea-55);		ContextoImp.Show("Sexo: Masculino");
			}else{
				ContextoImp.MoveTo(420, linea-55);		ContextoImp.Show("Sexo: Femenino");
			}
			
			ContextoImp.MoveTo(20, linea-65);			ContextoImp.Show("Direccion : "+dir_paciente);
			ContextoImp.MoveTo(420, linea-65);			ContextoImp.Show("Solicitado : "+this.procedencia);
			
			ContextoImp.MoveTo(20, linea-75);			ContextoImp.Show("Nombre del Medico : "+this.medicotratante);
			ContextoImp.MoveTo(420,linea-75);			ContextoImp.Show("Habitacion : ");
			
			Gnome.Print.Setfont (ContextoImp, fuente3);
			ContextoImp.MoveTo(20,linea-91);			ContextoImp.Show("PRUEBA : "+this.nombre_estudio);
			ContextoImp.MoveTo(20,linea-91.5);			ContextoImp.Show("PRUEBA : "+this.nombre_estudio);
			Gnome.Print.Setfont (ContextoImp, fuente4);			
			
			ContextoImp.MoveTo(100,linea-105);			ContextoImp.Show("PARAMETROS");
			ContextoImp.MoveTo(100,linea-105.5);		ContextoImp.Show("PARAMETROS");
			ContextoImp.MoveTo(250,linea-105);			ContextoImp.Show("RESULTADOS");
			ContextoImp.MoveTo(250,linea-105.5);		ContextoImp.Show("RESULTADOS");
			ContextoImp.MoveTo(400,linea-105);			ContextoImp.Show("V.R.");
			ContextoImp.MoveTo(400,linea-105.5);		ContextoImp.Show("V.R.");
			
			linea = linea - 115;
			
			TreeIter iter;
			if ( this.treeViewEngineresultados.GetIterFirst (out iter)){
				if((bool) this.lista_de_resultados.Model.GetValue (iter,5) == true){
					ContextoImp.MoveTo(65, linea);	ContextoImp.Show((string) this.lista_de_resultados.Model.GetValue (iter,0));
					ContextoImp.MoveTo(230,linea);	ContextoImp.Show((string) this.lista_de_resultados.Model.GetValue (iter,1));
					ContextoImp.MoveTo(385,linea);	ContextoImp.Show((string) this.lista_de_resultados.Model.GetValue (iter,2));
					linea -= 8;
				}	
				while (treeViewEngineresultados.IterNext(ref iter)){
					if((bool) this.lista_de_resultados.Model.GetValue (iter,5) == true){
						ContextoImp.MoveTo(65, linea);	ContextoImp.Show((string) this.lista_de_resultados.Model.GetValue (iter,0));
						ContextoImp.MoveTo(230,linea);	ContextoImp.Show((string) this.lista_de_resultados.Model.GetValue (iter,1));
						ContextoImp.MoveTo(385,linea);	ContextoImp.Show((string) this.lista_de_resultados.Model.GetValue (iter,2));
						linea -= 8;
					}
				}
			}
			ContextoImp.MoveTo(20,linea-8);			ContextoImp.Show("Observacion :"+this.observa);
			
			Gnome.Print.Setfont (ContextoImp, fuente4);
			ContextoImp.MoveTo(100, 161); 		ContextoImp.Show("----------------------------------------------------------");
			ContextoImp.MoveTo(100, 153);		ContextoImp.Show("  Q.B.P. DANIEL OLIVARES");
			ContextoImp.MoveTo(100, 145);		ContextoImp.Show("     CED. PROFESIONAL 1234567");
			ContextoImp.MoveTo(100, 137);		ContextoImp.Show("               AUTORIZA");
			
			ContextoImp.MoveTo(350, 161);		ContextoImp.Show("----------------------------------------------------------");
			ContextoImp.MoveTo(350, 153);		ContextoImp.Show(" "+this.quimicoautorizado);
			ContextoImp.MoveTo(350, 145);		ContextoImp.Show("     CED. PROFESIONAL "+this.cedulaquimico);
			ContextoImp.MoveTo(350, 137);		ContextoImp.Show("               REALIZA");
			ContextoImp.ShowPage();
		}
 	}    
 }