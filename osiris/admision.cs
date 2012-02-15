// created on 4/27/2007 at 12:22 PM
////////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
//				  Juan Antonio Peña Gzz. - gjuanzz@gmail.com (Programacion Mono)
// 				  
// Licencia		: GLP
// S.O. 		: Linux
//////////////////////////////////////////////////////////
//
// proyect osiris is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// proyect osirir is distributed in the hope that it will be useful,
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
// Proposito	: Pagos en Caja 
// Objeto		: cargos_hospitalizacion.cs
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using System.Collections;

namespace osiris
{
	public class admision
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		// Declarando ventana principal de Admision
		[Widget] Gtk.Window menu_admision = null;
		[Widget] Gtk.Button button_reg_admision = null;
		[Widget] Gtk.Button button_reportes_regadmin = null;
		[Widget] Gtk.Button button_imprime_prot = null;
		[Widget] Gtk.Button button_paquetes = null;
		[Widget] Gtk.Button button_presupuestos = null;
		[Widget] Gtk.Button button_cambia_datos_paciente = null;
		[Widget] Gtk.Button button_cancela_folios = null;
		[Widget] Gtk.Button button_reportes_de_ocupacion = null;
		[Widget] Gtk.Button button_rpt_pacientes_alta = null;
		[Widget] Gtk.Button button_rpt_presupuestos = null;
		[Widget] Gtk.Button button_rpt_separacion_paquetes = null;
		[Widget] Gtk.Button button_asignacion_habitacion = null;	
		[Widget] Gtk.Button button_separa_folio = null;	
		[Widget] Gtk.Button button_cita_paciente = null;
				
		//Ventana de cancelacion de folios
		[Widget] Gtk.Window cancelador_folios = null;
		[Widget] Gtk.Button button_cancelar = null;
		[Widget] Gtk.Entry entry_folio = null;
		[Widget] Gtk.Entry entry_motivo = null;
		
		// Pregunta de Admision
		//[Widget] Gtk.Window nuevo_paciente_si_no;
		//[Widget] Gtk.Button button_respuesta_no;
		//[Widget] Gtk.Button button_respuesta_si;
		//[Widget] Gtk.Button button_salir_pregunta;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;		
		string connectionString;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		protected Gtk.Window MyWin;
		
		public admision (string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;					
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "menu_admision", null);
			gxml.Autoconnect (this);        
			////// Muestra ventana de Glade
			menu_admision.Show();
			button_reg_admision.Clicked += new EventHandler(on_button_reg_admision_clicked);
			button_reportes_regadmin.Clicked += new EventHandler(on_button_reportes_regadmin_click);
			button_imprime_prot.Clicked += new EventHandler(on_button_imprime_prot_clicked);
			button_paquetes.Clicked += new EventHandler(on_button_paquetes_clicked);
			button_presupuestos.Clicked += new EventHandler(on_button_presupuestos_clicked);
			button_cancela_folios.Clicked += new EventHandler(on_button_cancela_folios_clicked);
			button_cambia_datos_paciente.Clicked += new EventHandler(on_button_cambia_datos_paciente_clicked);
			button_reportes_de_ocupacion.Clicked += new EventHandler(on_button_reportes_de_ocupacion_clicked);
			button_rpt_pacientes_alta.Clicked += new EventHandler(on_button_rpt_pacientes_alta_clicked);
			button_rpt_presupuestos.Clicked += new EventHandler(on_button_rpt_presupuestos_clicked);
			button_cita_paciente.Clicked += new EventHandler(on_button_cita_paciente_clicked);
			
			button_rpt_separacion_paquetes.Clicked += new EventHandler(on_button_rpt_separacion_paquetes_clicked);
			
			button_separa_folio.Clicked += new EventHandler(on_button_separa_folio_clicked);
			
			button_asignacion_habitacion.Clicked += new EventHandler(on_button_asignacion_habitacion_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_separa_folio.Sensitive = false;
		}
		
		void on_button_reportes_de_ocupacion_clicked(object sender, EventArgs args)
		{
			new osiris.reporte_pacientes_sin_alta(nombrebd);
		}
		
		void on_button_rpt_separacion_paquetes_clicked(object sender, EventArgs args)
		{
			new osiris.rpt_separacion_de_paquetes(nombrebd);
		}
		
		void on_button_separa_folio_clicked(object sender, EventArgs a)
		{
			new osiris.reservacion_de_paquetes(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,0,false);
		}
		
		void on_button_asignacion_habitacion_clicked(object sender, EventArgs args)
		{
		   new osiris.asignacion_de_habitacion(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,0);
		}
		
		void on_button_cita_paciente_clicked(object sender, EventArgs a)
		{
			new osiris.calendario_citas(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,1);
		}
		public void on_button_reg_admision_clicked (object sender, EventArgs a)
		{
			//Glade.XML gxml = new Glade.XML (null, "hscmty.glade", "nuevo_paciente_si_no", null);
			//gxml.Autoconnect (this);
			//nuevo_paciente_si_no.Show();
			//button_respuesta_no.Clicked   += new EventHandler(on_button_respuesta_no_clicked);
			//button_respuesta_si.Clicked   += new EventHandler(on_button_respuesta_si_clicked);
			//button_salir_pregunta.Clicked += new EventHandler(on_cierraventanas_clicked);
			new osiris.registro_paciente_busca("busca1",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"");
		}
		
		/*
		// Cuando no es nuevo se va a ventana de busqueda
		// y encuentra al paciente que necesita, llena los valores en la pantalla
		// admision registro
		public void on_button_respuesta_no_clicked (object sender, EventArgs a)
		{
			new osiris.registro_paciente_busca("busca1",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		// Cuando es nuevos el Paciente, ingresa los valores
		// registro_admision.cs
		public void on_button_respuesta_si_clicked (object sender, EventArgs a)
		{
			new osiris.registro_paciente_busca("nuevo",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}*/
		
		public void on_button_paquetes_clicked(object sender, EventArgs a)
		{
			if (LoginEmpleado =="DOLIVARES" || LoginEmpleado =="ADMIN" ){
 				new osiris.paquetes_cirugias (LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
				MessageType.Error,ButtonsType.Ok,"No esta autorizado para esta opcion...");
				msgBox.Run();			msgBox.Destroy();
			}
		}
				
		public void on_button_imprime_prot_clicked (object sender, EventArgs a)
		{
			string folioserv = "";
			new osiris.impr_doc_pacientes(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,folioserv,2);  // rpd_doc_pacientes.cs
		}
		
		void on_button_reportes_regadmin_click(object sender, EventArgs args)
		{
			new osiris.rptAdmision(nombrebd,"impresora");  // rpt_rep1_admision.cs
		}
		
		void on_button_cambia_datos_paciente_clicked (object sender, EventArgs args)
		{
			new osiris.cambia_paciente(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);  // cambia_pacientes.cs
		}
		
		void on_button_rpt_pacientes_alta_clicked(object sender, EventArgs args)
		{
			new osiris.reporte_pacientes_con_alta(nombrebd);
		}
				
		void on_button_presupuestos_clicked (object sender, EventArgs args)
		{
			new osiris.presupuestos_cirugias(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_rpt_presupuestos_clicked(object sender, EventArgs args)
		{
			new osiris.rpt_presupuesto(nombrebd);
		}
		
		void on_button_cancela_folios_clicked(object sender, EventArgs args)
		{
			if (LoginEmpleado =="DOLIVARES" || LoginEmpleado =="ADMIN" || LoginEmpleado =="RIOSGARCIA" || LoginEmpleado == "CMARQUEZ"){
				menu_admision.Destroy();
				Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "cancelador_folios", null);
				gxml.Autoconnect (this);
				cancelador_folios.Show();
				
				button_cancelar.Clicked += new EventHandler(on_button_cancelar_clicked);
				button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Error,
										ButtonsType.Ok,"No esta autorizado para esta opcion...");
				msgBox.Run();				msgBox.Destroy(); 
			}
		}
		
		void on_button_cancelar_clicked(object sender, EventArgs args)
		{
			Npgsql.NpgsqlConnection conexion;
			conexion = new NpgsqlConnection(connectionString+nombrebd);
			try{
				conexion.Open();
				NpgsqlCommand comando;
				comando = conexion.CreateCommand();
				comando.CommandText = "SELECT id_producto "+
									"FROM osiris_erp_cobros_deta "+
									"WHERE folio_de_servicio = '"+entry_folio.Text.Trim()+"' ;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if(lector.Read() == true){
					if(entry_motivo.Text.Trim() != "" || entry_folio.Text.Trim() != "" ){
						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Question,ButtonsType.YesNo,"¿ El Nº de Atencion contiene productos cargados\n"+
										" esta seguro que desea CANCELARLO?");
						ResponseType miResultado = (ResponseType)msgBox.Run ();
						msgBox.Destroy();
				 		//Console.WriteLine(miResultado.ToString());
				 		if (miResultado == ResponseType.Yes){
							//Npgsql.NpgsqlConnection conexion;
							conexion = new NpgsqlConnection(connectionString+nombrebd);
							try{
								conexion.Open();
								//Npgsql.NpgsqlCommand comando;
								comando = conexion.CreateCommand();
								
								comando.CommandText = "UPDATE osiris_erp_cobros_enca SET "+
													"cancelado = 'true' , "+
													"fechahora_cancelacion = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
													"motivo_cancelacion = '"+entry_motivo.Text.Trim()+"',"+
													"id_quien_cancelo = '"+LoginEmpleado+"' "+
													"WHERE folio_de_servicio = '"+entry_folio.Text.Trim()+"' ";
								comando.ExecuteNonQuery();                  comando.Dispose();
								//Console.WriteLine(comando.CommandText);
								msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
													ButtonsType.Ok,"El Nº de Atencion se CANCELO con exito!!");
								msgBox.Run();				msgBox.Destroy();
								entry_folio.Text = "";				entry_motivo.Text = "";
								//cancelador_folios.Destroy();
								conexion.Close();
							}catch(Npgsql.NpgsqlException ex){
								Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
								msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Error,
														ButtonsType.Ok,"PostgresSQL error: {0}",ex.Message);
								msgBox.Run();				msgBox.Destroy();
							}
						}
					}else{
						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
												ButtonsType.Ok,"Seleccione un Nº de Atencion y/o un motivo de cancelacion!!");
						msgBox.Run();				msgBox.Destroy();
					}
				}else{//REALIZA ESTO SI EL FOLIO NO CONTIENE PRODUCTOS APLICADOS
					conexion = new NpgsqlConnection(connectionString+nombrebd);
					try{
						conexion.Open();
						//Npgsql.NpgsqlCommand comando;
						comando = conexion.CreateCommand();
						
						comando.CommandText = "UPDATE osiris_erp_cobros_enca SET "+
											"cancelado = 'true' , "+
											"fechahora_cancelacion = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
											"motivo_cancelacion = '"+entry_motivo.Text.Trim()+"',"+
											"id_quien_cancelo = '"+LoginEmpleado+"' "+
											"WHERE folio_de_servicio = '"+entry_folio.Text.Trim()+"' ";
						comando.ExecuteNonQuery();                  comando.Dispose();
						//Console.WriteLine(comando.CommandText);
						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
											ButtonsType.Ok,"El Nº de Atencion se CANCELO con exito!!");
						msgBox.Run();				msgBox.Destroy();
						entry_folio.Text = "";				entry_motivo.Text = "";
						//cancelador_folios.Destroy();
						conexion.Close();
					}catch(Npgsql.NpgsqlException ex){
						Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Error,
												ButtonsType.Ok,"PostgresSQL error: {0}",ex.Message);
						msgBox.Run();				msgBox.Destroy();
					}
				}
			}catch(Npgsql.NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Error,
										ButtonsType.Ok,"PostgresSQL error: {0}",ex.Message);
				msgBox.Run();				msgBox.Destroy();
			}
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}