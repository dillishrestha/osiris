// created on 20/06/2010
///////////////////////////////////////////////////////////
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	:  Ing. Daniel Olivares C. (Programacion Base y Ajustes)
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
using Npgsql;
using Gtk;
using Glade;
using Gdk;

namespace osiris
{
	public class notas_medicas
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		[Widget] Gtk.Window notas_medicas_enfermeria = null;
		[Widget] Gtk.TextView textview1 = null;
		[Widget] Gtk.TextView textview2 = null;
		
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_nombre_paciente = null;
		[Widget] Gtk.Entry entry_edad_paciente = null;		
		[Widget] Gtk.Entry entry_numerotencion = null;
		[Widget] Gtk.Entry entry_id_doctor = null;
		[Widget] Gtk.Entry entry_doctor = null;
		[Widget] Gtk.Button button_guardar = null;
		[Widget] Gtk.Entry entry_fechanotas = null;
		
		TextBuffer buffer = new TextBuffer (null);
		
		string connectionString;
		string nombrebd;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string name_field;
		string pidpaciente;
		string folioservicio;
		string nombredoctor;
		
		string sql_general = "SELECT notas_de_enfermeria,notas_de_evolucion,indicaciones_medicas,nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad," +
							"to_char(fecha_anotacion,'yyyy-MM-dd') AS fechaanotacion,id_secuencia "+
							""+
							"FROM osiris_his_informacion_medica,osiris_his_paciente "+
									"WHERE osiris_his_informacion_medica.pid_paciente = osiris_his_paciente.pid_paciente ";
		string sql_pidpaciente;
		string sql_folioservicio;
		string sql_filtronotasblanco;
			
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		public notas_medicas (string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_,
		                      string title_window, string name_field_,string pidpaciente_,string folioservicio_,string iddoctor_,string nombredoctor_,string nombrepaciente_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			LoginEmpleado = LoginEmp;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			name_field = name_field_;
			pidpaciente = pidpaciente_;
			folioservicio = folioservicio_;
			nombredoctor = nombredoctor_;
			
			sql_pidpaciente = " AND osiris_his_informacion_medica.pid_paciente = '"+pidpaciente+"' ";
			sql_folioservicio = " AND osiris_his_informacion_medica.folio_de_servicio = '"+folioservicio+"' ";
			sql_filtronotasblanco = " AND "+name_field+" <> '' ";
			
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "notas_medicas_enfermeria", null);
			gxml.Autoconnect (this);
			notas_medicas_enfermeria.Show();
			notas_medicas_enfermeria.SetPosition(WindowPosition.Center);	// centra la ventana en la pantalla
			notas_medicas_enfermeria.Title = title_window;
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_guardar.Clicked += new EventHandler(on_button_guardar_clicked);
			entry_fechanotas.Text = (string) DateTime.Now.ToString("yyyy-MM-dd");
			entry_pid_paciente.Text = (string) pidpaciente;
			entry_nombre_paciente.Text = (string) nombrepaciente_;
			entry_numerotencion.Text = (string) folioservicio;
			entry_id_doctor.Text = (string) iddoctor_;
			entry_doctor.Text = (string) nombredoctor_;
			switch (name_field){	
				case "notas_de_evolucion":
					textview1.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
				break;
				case "notas_de_enfermeria":
					textview1.ModifyBase(StateType.Normal, new Gdk.Color(255,179,235)); // Color Rosa
				break;
				case "indicaciones_medicas":
					textview1.ModifyBase(StateType.Normal, new Gdk.Color(152,255,255)); // Color Rosa
				break;
			}			
			llenando_informacion();			
		}
		
		void llenando_informacion()
		{
			buffer.Clear();
			buffer = textview1.Buffer;			
			TextIter insertIter = buffer.StartIter;
			classpublic.CreateTags(buffer);
						
			string texto_hitoria_clinica = "";
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = sql_general+sql_pidpaciente+sql_folioservicio+sql_filtronotasblanco+" ORDER BY id_secuencia DESC;";
				Console.WriteLine(comando.CommandText);					
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if(lector.Read()){
					entry_pid_paciente.Text = pidpaciente;
										entry_nombre_paciente.Text = (string) lector["nombre1_paciente"].ToString().Trim()+" "+
											(string) lector["nombre2_paciente"].ToString().Trim()+" "+
											(string) lector["apellido_paterno_paciente"].ToString().Trim()+" "+
											(string) lector["apellido_materno_paciente"].ToString().Trim();
					entry_edad_paciente.Text = (string) lector["edad"].ToString();
					entry_numerotencion.Text = folioservicio.Trim();
					entry_doctor.Text = nombredoctor;
					if((string) lector[name_field].ToString() != ""){
						buffer.InsertWithTagsByName (ref insertIter, "Fecha de Nota: "+(string) lector["fechaanotacion"].ToString().Trim()+"    Nº de NOTA :"+(string) lector["id_secuencia"].ToString().Trim()+"\n", "bold");
						buffer.InsertWithTagsByName (ref insertIter, "Hora de Nota : \n\n", "bold");
						buffer.Insert (ref insertIter, (string) lector[name_field].ToString()+"\n\n\n");
					}
					while(lector.Read()){
						if((string) lector[name_field].ToString() != ""){
							buffer.InsertWithTagsByName (ref insertIter, "Fecha de Nota: "+(string) lector["fechaanotacion"].ToString().Trim()+"    Nº de NOTA :"+(string) lector["id_secuencia"].ToString().Trim()+"\n", "bold");
							buffer.InsertWithTagsByName (ref insertIter, "Hora de Nota : \n\n", "bold");
							buffer.Insert (ref insertIter, (string) lector[name_field].ToString()+"\n\n\n");
						}
					}
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
						MessageType.Error, 
						ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
	   			
	       	}
       		conexion.Close ();
			//buffer.InsertWithTagsByName (ref insertIter, "\nThis line has center justification.\n", "center");			
		}
		
		void on_button_guardar_clicked(object sender, EventArgs args)
		{
			if(textview2.Buffer.Text.ToString()!=""){
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
            	try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand();
					comando.CommandText = "INSERT INTO osiris_his_informacion_medica (pid_paciente,folio_de_servicio,"+
					"fechahora_creacion,id_empleado_creacion,id_medico,"+name_field+",fecha_anotacion,hora_anotacion)"+
					" VALUES ('"+
					(string) entry_pid_paciente.Text.ToString().Trim()+"','"+
					(string) entry_numerotencion.Text.ToString().Trim()+"','"+
					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
					LoginEmpleado+"','"+
					(string) entry_id_doctor.Text.ToString().Trim()+"','"+
					textview2.Buffer.Text.ToString()+"','"+
					DateTime.Now.ToString("yyyy-MM-dd")+"','"+
					"12:00"	+"')";
					Console.WriteLine(comando.CommandText);
					comando.ExecuteNonQuery();
					comando.Dispose();
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
			 							ButtonsType.Close, "Los datos se guardaron con EXITO");
					msgBoxError.Run ();			msgBoxError.Destroy();
					textview2.Buffer.Clear();
					llenando_informacion();
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
						MessageType.Error, 
						ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();					msgBoxError.Destroy();	   			
	       		}
       			conexion.Close ();      							
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
								MessageType.Error,ButtonsType.Close,"La nota no contiene informacion, verifique...");
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}			
			//Console.WriteLine(textview2.Buffer.Text.ToString());
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked(object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}
