// asignacion_de_habitacion.cs created with MonoDevelop
// User: egonzalez at 10:39 a 25/04/2008
// Monterrey - Mexico
//
// Autor    	: 	Ing. Erik Gonzalez (Programacion)
//					Ing. Daniel Olivares (Ajuste y Colaboracion)
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
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using System.Collections;

namespace osiris
{	
	public class asignacion_de_habitacion
	{       		
       	[Widget] Gtk.Window asignacion_habitacion = null;
		[Widget] Gtk.Button button_salir = null;
		[Widget] Gtk.Button button_aceptar = null;
		[Widget] Gtk.Button button_cancelar = null;
		[Widget] Gtk.Button button_reporte = null;
        [Widget] Gtk.Button button_lista_espera = null;
		[Widget] Gtk.Entry entry_id_habitacion = null;
		[Widget] Gtk.Entry entry_area = null;
		[Widget] Gtk.Entry entry_folio = null;
		[Widget] Gtk.Entry entry_numero_habitacion = null;
		[Widget] Gtk.Entry entry_descripcion = null;
		[Widget] Gtk.Entry entry_paciente = null;
		[Widget] Gtk.Entry entry_dia_ocupacion = null;
		[Widget] Gtk.Entry entry_mes_ocupacion = null;
		[Widget] Gtk.Entry entry_anno_ocupacion = null;
		[Widget] Gtk.TreeView treeview_habitaciones = null;
		[Widget] Gtk.CheckButton checkbutton_todas = null;
        [Widget] Gtk.ComboBox combobox_area = null; 		
		[Widget] Gtk.CheckButton check_cambio_habitacion = null;
		[Widget] Gtk.CheckButton check_lista_espera = null;
        	/////// Elementos Busqueda de paciente\\\\\\\\
		[Widget] Gtk.Window busca_paciente = null;
		[Widget] Gtk.TreeView lista_de_Pacientes = null;
		[Widget] Gtk.TreeView lista_de_Pacientes_asignados = null;
		[Widget] Gtk.Button button_nuevo_paciente = null;
		[Widget] Gtk.RadioButton radiobutton_busca_apellido = null;
		[Widget] Gtk.RadioButton radiobutton_busca_nombre = null;
		[Widget] Gtk.RadioButton radiobutton_busca_expediente = null;
		[Widget] Gtk.Entry entry_expresion = null;
		[Widget] Gtk.Button button_selecciona = null;
		[Widget] Gtk.Button button_buscar_busqueda = null;		
				
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string NomEmpleados;
		int tipo_admicion_clase;			
					
		string connectionString;
		string nombrebd;
		class_conexion conexion_a_DB = new class_conexion();
		
		string query_areas;
		string query_fecha_de_ocupacion;
		string Combonombrearea;
        int pid;
		int folio_atencion;
		string descripcion_area;
		int id_habitacion_proveniente;
       
        int pid_pasingado;
		int folio_atencion_pasignado;		
		
		int conteocheckpaciente = 0;
        bool activo = false;
		bool activoPac = false;
		bool cambio = false;
        int idpacientehistorial = 0;
		
		private TreeStore treeViewEngineBuscahabitacion;
		private TreeStore treeViewEngineBusca;    	
        private TreeStore treeViewEngineBuscaAsignacion;        	
		
    	protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public asignacion_de_habitacion(string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_ , int tipo_admicion) 
		{
			LoginEmpleado = LoginEmp;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			NomEmpleados = NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado;
			tipo_admicion_clase = tipo_admicion;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "asignacion_habitacion", null);
			gxml.Autoconnect (this);
	        asignacion_habitacion.Show();
			
            button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);		
            checkbutton_todas.Clicked  += new EventHandler(on_check_todas_clicked);			
            check_cambio_habitacion.Clicked += new EventHandler(on_check_cambio__habitacion_clicked);			
            check_lista_espera.Clicked += new EventHandler(on_check_lista__espera_clicked);						
			 			
			button_cancelar.Clicked += new EventHandler(on_cancelar_clicked);			
			button_aceptar.Clicked += new EventHandler(on_aceptar_clicked);		
            button_reporte.Clicked += new EventHandler(on_reporte_clicked);	
            button_lista_espera.Clicked += new EventHandler(on_lista_clicked);			
			
			button_buscar_busqueda.Clicked += new EventHandler(on_buscar_busqueda_clicked);
			
			entry_area.IsEditable = false;
			entry_descripcion.IsEditable = false;
			entry_id_habitacion.IsEditable = false;
			entry_paciente.IsEditable = false;
            entry_dia_ocupacion.IsEditable = false;
			entry_mes_ocupacion.IsEditable = false;
			entry_anno_ocupacion.IsEditable = false;
			button_lista_espera.Sensitive = false;
			
			entry_folio.ModifyBase(StateType.Normal, new Gdk.Color(170,220,170));
			entry_numero_habitacion.ModifyBase(StateType.Normal, new Gdk.Color(170,220,170));
			entry_area.ModifyBase(StateType.Normal, new Gdk.Color(170,220,170));
			entry_descripcion.ModifyBase(StateType.Normal, new Gdk.Color(170,220,170));
			entry_id_habitacion.ModifyBase(StateType.Normal, new Gdk.Color(170,220,170));
			entry_paciente.ModifyBase(StateType.Normal, new Gdk.Color(170,220,170));
            entry_dia_ocupacion.ModifyBase(StateType.Normal, new Gdk.Color(170,220,170));
			entry_mes_ocupacion.ModifyBase(StateType.Normal, new Gdk.Color(170,220,170));
			entry_anno_ocupacion.ModifyBase(StateType.Normal, new Gdk.Color(170,220,170));
			
			llenacombo_areas();
			crea_treeview_busqueda_habitacion();//treeview habitacion
			crea_treeview_busqueda();//treeview busqueda de pacientes
			crea_treeview_busqueda_asignacion();//treeview busqueda de paciente con cuartos
			on_buscar_paciente_clicked();
			on_busca_paciente_Asigancion();
            			
			lista_de_Pacientes_asignados.Sensitive = false;
            			
			if(this.tipo_admicion_clase != 0){
				this.checkbutton_todas.Sensitive = false;
				this.combobox_area.Sensitive = false;			
				query_areas = "AND osiris_his_habitaciones.id_tipo_admisiones = '"+tipo_admicion_clase.ToString().Trim()+"' ORDER BY osiris_his_habitaciones.id_tipo_admisiones,osiris_his_habitaciones.numero_cuarto;";
				llena_lista_habitaciones();
			}
			
		}

		void on_cancelar_clicked(object sender, EventArgs args)
		{
          limpia_variables();
		}	
		
		void on_check_lista__espera_clicked(object sender, EventArgs args)
		{
			if (this.check_lista_espera.Active == true){
				this.treeViewEngineBuscahabitacion.Clear();
               // this.treeViewEngineBusca.Clear();   
				this.button_lista_espera.Sensitive = true;
				this.treeview_habitaciones.Sensitive=false;
				this.lista_de_Pacientes.Sensitive=false;
				this.lista_de_Pacientes_asignados.Sensitive=true;
				this.button_aceptar.Sensitive =false;
				this.button_cancelar.Sensitive =false;
				this.button_reporte.Sensitive=false;
				this.check_cambio_habitacion.Sensitive = false;
				this.checkbutton_todas.Sensitive = false;
				this.combobox_area.Sensitive=false;
				
		    }else{
				this.button_lista_espera.Sensitive = false;
                this.treeview_habitaciones.Sensitive=true;
				this.lista_de_Pacientes.Sensitive=true;
				this.lista_de_Pacientes_asignados.Sensitive=false;				
                this.button_aceptar.Sensitive =true;
				this.button_cancelar.Sensitive =true;
				this.button_reporte.Sensitive=true;
				this.check_cambio_habitacion.Sensitive = true;
				this.checkbutton_todas.Sensitive = true;
                this.combobox_area.Sensitive=true;				
				limpia_variables();
		    }
		}

        void on_lista_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
			MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de que  desea mover el paciente {0} hacia la lista de espera {1}  ?" , this.entry_paciente.Text, this.entry_numero_habitacion.Text, descripcion_area);
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
				
			if (miResultado == ResponseType.Yes){
			
			
				query_fecha_de_ocupacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
				
					comando.CommandText = "UPDATE osiris_his_habitaciones SET disponible ='True', folio_de_servicio = '0', pid_paciente = '0' "+ 
															      "WHERE id_habitacion =  '"+this.id_habitacion_proveniente+"';";
				
 
						
					comando.ExecuteNonQuery();
					comando.Dispose();
						
                    comando = conexion.CreateCommand ();
					comando.CommandText = "UPDATE osiris_erp_cobros_enca SET id_habitacion ='1' "+ 
											  "WHERE folio_de_servicio =  '"+this.entry_folio.Text+"';";
						
					comando.ExecuteNonQuery();
					comando.Dispose();
						
			        MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
					                          ButtonsType.Ok,"El paciente paso a lista de espera y la habitacion se libero satisfactoreamente");										
						                    msgBox1.Run ();
					                     msgBox1.Destroy();		
					conexion.Close ();
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError1 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError1.Run ();			msgBoxError1.Destroy();
				}
			
				//NpgsqlConnection conexion;
				conexion = new NpgsqlConnection (connectionString+nombrebd );
				try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();						 
						comando.CommandText = "INSERT INTO osiris_his_historial_habitaciones ("+
								"id_quien_asigna,"+
		             			"folio_de_servicio,"+
		                		"pid_paciente,"+
		                		"fecha_de_asignacion,"+
		                		"traspaso,"+
		                		"id_habitacion,"+
		                		"id_habitacion_anterior,"+
		                		"dias_de_ocupacion) "+
		                		"VALUES ('"+this.LoginEmpleado+"',"+
		      					"'"+this.entry_folio.Text+"',"+
		      					"'"+this.pid_pasingado+"',"+
		      					"'"+query_fecha_de_ocupacion+"',"+
		      					"'true',"+
		      					"'0',"+
		      					"'"+this.id_habitacion_proveniente+"',"+
		      					"'1');"; 
		      					
		                			
						comando.ExecuteNonQuery();
						comando.Dispose();						
						 conexion.Close ();
				 	  }catch (NpgsqlException ex){
										   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																MessageType.Error, 
																ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
												msgBoxError.Run ();			msgBoxError.Destroy();
											}
							
			         
			
              limpia_variables();
			
			}else{			   
				MessageDialog msgBox2 = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
				                          ButtonsType.Ok,"El paciente no se modifico y quedo en su habitacion");										
				msgBox2.Run ();		msgBox2.Destroy();			     
			}			
		}
		
		void on_aceptar_clicked(object sender, EventArgs args)
		{
			//if (this.entry_id_habitacion.Text != "" && this.entry_paciente.Text != "")
			if(activo ==true && activoPac ==true){
			  if (this.check_cambio_habitacion.Active == true){
                    MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
			        MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de que  desea cambiar el paciente {0} hacia la habitacion {1} - {2} ?" , this.entry_paciente.Text, this.entry_numero_habitacion.Text, descripcion_area);
		        	ResponseType miResultado = (ResponseType)msgBox.Run ();
			        msgBox.Destroy();
				
					 	if (miResultado == ResponseType.Yes){
				 			Console.WriteLine("acepto");
							pid = this.pid_pasingado;
						    query_fecha_de_ocupacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                   
								        NpgsqlConnection conexion; 
										conexion = new NpgsqlConnection (connectionString+nombrebd);
								    	// libera la habitacion anterior
										try{
											conexion.Open ();
											NpgsqlCommand comando; 
											comando = conexion.CreateCommand ();
											comando.CommandText = "UPDATE osiris_his_habitaciones SET disponible ='True', folio_de_servicio = '0', pid_paciente = '0' "+ 
															      "WHERE id_habitacion =  '"+this.id_habitacion_proveniente+"';";
											
											comando.ExecuteNonQuery();
											comando.Dispose();					
							              
							               //Console.WriteLine(comando.CommandText+"------------");
					                   	   conexion.Close ();
									   	}catch (NpgsqlException ex){
										   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																MessageType.Error, 
																ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
												msgBoxError.Run ();			msgBoxError.Destroy();
										}
						
						cambio = true;
						idpacientehistorial = pid_pasingado;   
						
						guarda_cambios();
						                              						
						 }else{
						limpia_variables();
						     Console.WriteLine("no acepto"); }
		       }else{
					
				   MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
			        MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro que desea asignar el paciente {0}  la habitacion {1}  ?" , this.entry_paciente.Text, this.entry_numero_habitacion.Text);
		        	ResponseType miResultado = (ResponseType)msgBox.Run ();
			        msgBox.Destroy();
			                  	if (miResultado == ResponseType.Yes){	
			                   	   query_fecha_de_ocupacion= this.entry_anno_ocupacion.Text+"-"+this.entry_mes_ocupacion.Text+"-"+this.entry_dia_ocupacion.Text+ " " +DateTime.Now.ToString("HH:mm:ss");
				               		idpacientehistorial = this.pid;	
								    guarda_cambios();	
								}else{
					              	limpia_variables();
						     	 }
			}						
				
		  }else	{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
											ButtonsType.Ok,"Favor de seleccionar un paciente y una habitacion");
						                    msgBox.Run();
				                            msgBox.Destroy();
		    }
		}
		
		void guarda_cambios()
		{
			
			        NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
			    	// Verifica que la base de datos este conectada
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						comando.CommandText = "UPDATE osiris_his_habitaciones SET disponible ='False',"+ 
											"fecha_de_ocupacion = '"+query_fecha_de_ocupacion+"', "+
											"pid_paciente = '"+this.pid+"', "+
											"folio_de_servicio = '"+this.entry_folio.Text+"' "+
							                "WHERE id_habitacion =  '"+this.entry_id_habitacion.Text+"';";
						
						comando.ExecuteNonQuery();
						comando.Dispose();
					    //Console.WriteLine(comando.CommandText+"------------");
						
                        comando = conexion.CreateCommand ();
						comando.CommandText = "UPDATE osiris_erp_cobros_enca SET id_habitacion ='"+this.entry_id_habitacion.Text+"' "+ 
											  "WHERE folio_de_servicio =  '"+this.entry_folio.Text+"';";
						
						comando.ExecuteNonQuery();
						comando.Dispose();
					    //Console.WriteLine(comando.CommandText+"------------");
																
					
			        	MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
					                          ButtonsType.Ok,"La habitacion se asigno satisfactoreamente...");										
						                    msgBox.Run ();
					                     msgBox.Destroy();		
						conexion.Close ();
			        }catch (NpgsqlException ex){
				   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();			msgBoxError.Destroy();
					}
			
			
			
					//NpgsqlConnection conexion;
					conexion = new NpgsqlConnection (connectionString+nombrebd );
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();						 
						comando.CommandText = "INSERT INTO osiris_his_historial_habitaciones ("+
								"id_quien_asigna,"+
		             			"folio_de_servicio,"+
		                		"pid_paciente,"+
		                		"fecha_de_asignacion,"+
		                		"traspaso,"+
		                		"id_habitacion,"+
		                		"id_habitacion_anterior,"+
		                		"dias_de_ocupacion) "+
		                		"VALUES ('"+this.LoginEmpleado+"',"+
		      					"'"+this.entry_folio.Text+"',"+
		      					"'"+this.idpacientehistorial+"',"+
		      					"'"+query_fecha_de_ocupacion+"',"+
		      					"'"+cambio+"',"+
		      					"'"+this.entry_id_habitacion.Text+"',"+
		      					"'"+this.id_habitacion_proveniente+"',"+
		      					"'1');"; 
		      					
		                			
						comando.ExecuteNonQuery();
						comando.Dispose();						
						 conexion.Close ();
				 	  }catch (NpgsqlException ex){
										   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																MessageType.Error, 
																ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
												msgBoxError.Run ();			msgBoxError.Destroy();
											}
							
			         
			
              limpia_variables();
			
         }			
		
		void on_check_cambio__habitacion_clicked(object sender, EventArgs args)
		{ 
           if (this.check_cambio_habitacion.Active == true){
				//this.treeViewEngineBuscahabitacion.Clear();
				this.lista_de_Pacientes.Sensitive = false;
				this.lista_de_Pacientes_asignados.Sensitive = true;
				this.treeview_habitaciones.Sensitive = false;
				this.check_lista_espera.Sensitive=false;
		    }else{
				this.lista_de_Pacientes.Sensitive = true;
				this.lista_de_Pacientes_asignados.Sensitive = false;
				this.treeview_habitaciones.Sensitive = true;
				this.check_lista_espera.Sensitive=true;
		    }
		}
		
		void on_check_todas_clicked(object sender, EventArgs args)
		{ 
			if (this.checkbutton_todas.Active == true){
				this.combobox_area.Sensitive = false;
				query_areas = "AND 	osiris_his_tipo_admisiones.id_tipo_admisiones != '0' ORDER BY osiris_his_tipo_admisiones.id_tipo_admisiones,osiris_his_habitaciones.numero_cuarto;";
				llena_lista_habitaciones();
				Combonombrearea = "";
		    }else{
				this.combobox_area.Sensitive = true;
				this.treeViewEngineBuscahabitacion.Clear();
		    }
		}		
		
		void on_buscar_busqueda_clicked(object sender, EventArgs args)
		{
          on_buscar_paciente_clicked();		
		}
		
		
		void llenacombo_areas()
		{
			combobox_area.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_area.PackStart(cell, true);
			combobox_area.AddAttribute(cell,"text",0);
			
			ListStore store = new ListStore( typeof (string),typeof (int));
			combobox_area.Model = store;
		    
			
            NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
           
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				//osiris_empleado.id_empleado
                comando.CommandText = "SELECT descripcion_admisiones,id_tipo_admisiones "+
									  "FROM osiris_his_tipo_admisiones " +
									  "WHERE habitaciones = 'true' AND grupo = 'MED';";
               			             
				NpgsqlDataReader lector = comando.ExecuteReader ();				
				while (lector.Read()){
					store.AppendValues ((string) lector ["descripcion_admisiones"], (int) lector["id_tipo_admisiones"]) ;
			  	}				
			}catch(NpgsqlException ex){
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();	msgBoxError.Destroy();
            }
            conexion.Close ();

			TreeIter iter;
			if (store.GetIterFirst(out iter)){
				combobox_area.SetActiveIter (iter);
			}
			combobox_area.Changed += new EventHandler (onComboBoxChanged_areas);
		}
		
		void onComboBoxChanged_areas (object sender, EventArgs args)
		{
			Console.WriteLine("Entre");
			ComboBox combobox_area = sender as ComboBox;
		    if (sender == null) {	return;	}
			
			TreeIter iter;
			if (combobox_area.GetActiveIter (out iter)) {
				Combonombrearea = (string) this.combobox_area.Model.GetValue(iter,0);
				query_areas = "AND osiris_his_tipo_admisiones.id_tipo_admisiones = '"+(int) this.combobox_area.Model.GetValue(iter,1)+"' ORDER BY osiris_his_tipo_admisiones.id_tipo_admisiones,osiris_his_habitaciones.numero_cuarto;";
				llena_lista_habitaciones();
			}
		}
				
		//llenado del tree view de los que tienen habitacion
		void on_busca_paciente_Asigancion()
		{
			this.treeViewEngineBuscaAsignacion.Clear();	
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);         
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				//osiris_empleado.id_empleado
	             comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente,descripcion_admisiones,numero_cuarto, "+
								" nombre1_paciente,nombre2_paciente, apellido_paterno_paciente, "+
								"apellido_materno_paciente, to_char(fecha_de_ocupacion,'dd-MM-yyyy HH24:mi') AS fech_ocupacion,"+
								"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
								"sexo_paciente,to_char(fechahora_registro_paciente,'dd-MM-yyyy HH24:mi') AS fech_creacion "+
								"FROM osiris_his_paciente,osiris_erp_cobros_enca,osiris_his_habitaciones,osiris_his_tipo_admisiones "+
								"WHERE alta_paciente = 'false' "+
						        "AND osiris_his_habitaciones.id_habitacion = osiris_erp_cobros_enca.id_habitacion "+
						        "AND osiris_his_habitaciones.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones " +
								"AND pagado = 'false' "+
								"AND cerrado = 'false' "+
								"AND reservacion = 'false' "+
						        "AND cancelado = 'false' "+
								"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
								"AND osiris_erp_cobros_enca.alta_paciente = false "+
								"AND osiris_erp_cobros_enca.cancelado = false "+
								"AND osiris_erp_cobros_enca.id_habitacion != 1 "+						
								"ORDER BY folio_de_servicio;";					  
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					this.treeViewEngineBuscaAsignacion.AppendValues ((bool) false,(int) lector["folio_de_servicio"],//TreeIter iter =
										(int) lector["pid_paciente"],
					                    (string) lector["descripcion_admisiones"] + " (" +Convert.ToString( lector["numero_cuarto"]) +")",
										(string) (lector["nombre1_paciente"] + " " + lector["nombre2_paciente"] + " "+ lector["apellido_paterno_paciente"] + " " +  lector["apellido_materno_paciente"]),
										(string) lector["fech_ocupacion"], (string) lector["edad"],
										(string) lector["sexo_paciente"],
										(string) lector["fech_creacion"]); 
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();	msgBoxError.Destroy();
            }
            conexion.Close ();
		}		
		
		void llena_lista_habitaciones()
		{
			treeViewEngineBuscahabitacion.Clear();
			activo = false;
			this.entry_numero_habitacion.Text = "";
	        this.entry_area.Text = "";
			this.entry_descripcion.Text = "";
			this.entry_id_habitacion.Text = "";			
			
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
               comando.CommandText = "SELECT 	osiris_his_habitaciones.id_tipo_admisiones,disponible,descripcion_cuarto,id_habitacion,numero_cuarto,pid_paciente,descripcion_admisiones "+
									"FROM osiris_his_habitaciones,osiris_his_tipo_admisiones " +
									"WHERE (osiris_his_tipo_admisiones.id_tipo_admisiones = osiris_his_habitaciones.id_tipo_admisiones) AND disponible = 'true'AND habitaciones = 'true' AND grupo = 'MED' "+
                                    query_areas;
						             
														 									  
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				while (lector.Read()){
					this.treeViewEngineBuscahabitacion.AppendValues ((bool) false,(int) lector["id_habitacion"],(int) lector["numero_cuarto"],(string) lector["descripcion_admisiones"],(int) lector["id_tipo_admisiones"],(string) lector["descripcion_cuarto"]);
				}				
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();	msgBoxError.Destroy();
            }
            conexion.Close ();
		}
		
		void crea_treeview_busqueda_habitacion()
		{
            treeViewEngineBuscahabitacion = new TreeStore(typeof(bool),typeof(int),typeof(int),typeof(string),typeof(int),typeof(string));
			this.treeview_habitaciones.Model = treeViewEngineBuscahabitacion;
			
			treeview_habitaciones.RulesHint = true;
			
			//treeview_habitaciones.RowActivated += on_selecciona_habitacion_clicked;  // Doble click selecciono paciente
			
			TreeViewColumn col_check = new TreeViewColumn();
			CellRendererToggle cel_check = new CellRendererToggle();
			
			col_check.Title = "Seleccionar"; // titulo de la cabecera de la columna, si está visible
			col_check.PackStart(cel_check, true);
			col_check.AddAttribute (cel_check, "active", 0);
			cel_check.Activatable = true;
			cel_check.Toggled += selecciona_fila;
			col_check.SortColumnId = (int) Column2.col_check;
			
			TreeViewColumn id_habitacion = new TreeViewColumn();
			CellRendererText cellr5 = new CellRendererText();
			id_habitacion.Title = "ID Habitacion"; // titulo de la cabecera de la columna, si está visible
			id_habitacion.PackStart(cellr5, true);
		    id_habitacion.AddAttribute (cellr5, "text", 1);    // la siguiente columna será 1 en vez de 1
			id_habitacion.SortColumnId = (int) Column2.id_habitacion;
			id_habitacion.Visible = false; //columna escondida
				
            TreeViewColumn habitacion = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			habitacion.Title = "Habitacion"; // titulo de la cabecera de la columna, si está visible
			habitacion.PackStart(cellr0, true);
		    habitacion.AddAttribute (cellr0, "text", 2);    // la siguiente columna será 1 en vez de 1
			habitacion.SortColumnId = (int) Column2.habitacion;
				
			
			TreeViewColumn area = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			area.Title = "Area"; // titulo de la cabecera de la columna, si está visible
			area.PackStart(cellr1, true);
			area.AddAttribute (cellr1, "text", 3);    // la siguiente columna será 1 en vez de 1
			area.SortColumnId = (int) Column2.area;
			   // Permite edita este campo
            
			TreeViewColumn Tipo_admicion = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			Tipo_admicion.Title = "Tipo Admision";
			Tipo_admicion.PackStart(cellrt2, true);
			Tipo_admicion.AddAttribute (cellrt2, "text", 4); // la siguiente columna será 1 en vez de 2
			Tipo_admicion.SortColumnId = (int) Column2.Tipo_admicion;
            
			TreeViewColumn descripcion = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			descripcion.Title = "Descripcion";
			descripcion.PackStart(cellrt3, true);
			descripcion.AddAttribute (cellrt3, "text", 5); // la siguiente columna será 2 en vez de 3
			descripcion.SortColumnId = (int) Column2.descripcion;			
			
            treeview_habitaciones.AppendColumn(col_check);
            treeview_habitaciones.AppendColumn(id_habitacion);
			treeview_habitaciones.AppendColumn(habitacion);
			treeview_habitaciones.AppendColumn(area);
			treeview_habitaciones.AppendColumn(Tipo_admicion);
			treeview_habitaciones.AppendColumn(descripcion);			
		}
		
		enum Column2
		{	
			col_check,	
			id_habitacion,
			habitacion,
			area,
			Tipo_admicion,
			descripcion
		}
		
		void selecciona_fila(object sender, ToggledArgs args)
		{	
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_habitaciones.Model.GetIter (out iter, path)) {
				bool old = (bool) treeview_habitaciones.Model.GetValue (iter,0);
				treeview_habitaciones.Model.SetValue(iter,0,!old);
                if (old == false && activo == true){
                	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Info, 
					ButtonsType.Ok,"Ya tiene seleccionada una habitacion, Desmarquela para seleccionar una nueva ");
					msgBoxError.Run ();
					msgBoxError.Destroy();
				    treeview_habitaciones.Model.SetValue (iter,0,false);
				}
				if (old == false && activo == false){
					//	int seleccion = int.Parse(path.ToString());
                    activo = true;					
				    this.entry_id_habitacion.Text = Convert.ToString(treeview_habitaciones.Model.GetValue(iter,1));
					this.entry_numero_habitacion.Text = Convert.ToString(treeview_habitaciones.Model.GetValue(iter,2));
			    	this.entry_area.Text = Convert.ToString(treeview_habitaciones.Model.GetValue(iter,4));
			    	this.entry_descripcion.Text = (string) treeview_habitaciones.Model.GetValue(iter,5);
					descripcion_area = this.entry_descripcion.Text = (string) treeview_habitaciones.Model.GetValue(iter,3);
				}
				if (old == true && activo == true){
					activo = false;
					this.entry_area.Text = "";
					this.entry_descripcion.Text = "";
					this.entry_numero_habitacion.Text = "";
					this.entry_id_habitacion.Text = "";
				}
			}	
		}
		
		void crea_treeview_busqueda_asignacion()
		{
			
			treeViewEngineBuscaAsignacion = new TreeStore(typeof(bool),
			                                        typeof(int),
													typeof(int),
			                                        typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));
													
			lista_de_Pacientes_asignados.Model = treeViewEngineBuscaAsignacion;
			
			lista_de_Pacientes_asignados.RulesHint = true;
			
			//lista_de_Pacientes.RowActivated += on_selecciona_paciente_clicked;  // Doble click selecciono paciente
	            
            TreeViewColumn col_checkPac = new TreeViewColumn();
			CellRendererToggle cel_checkPac = new CellRendererToggle();			
			
   			col_checkPac.Title = "Seleccionar"; // titulo de la cabecera de la columna, si está visible
			col_checkPac.PackStart(cel_checkPac, true);
			col_checkPac.AddAttribute (cel_checkPac, "active", 0);
			cel_checkPac.Activatable = true;
			cel_checkPac.Toggled += selecciona_fila_paciente_asigando;
			col_checkPac.SortColumnId = (int) Column3.col_checkPac;
					
		    TreeViewColumn col_foliodeatencion = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_foliodeatencion.Title = "Folio A."; // titulo de la cabecera de la columna, si está visible
			col_foliodeatencion.PackStart(cellr0, true);
			col_foliodeatencion.AddAttribute (cellr0, "text", 1);    // la siguiente columna será 1 en vez de 1
			col_foliodeatencion.SortColumnId = (int) Column3.col_foliodeatencion;
		
			TreeViewColumn col_PidPaciente = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_PidPaciente.Title = "PID"; // titulo de la cabecera de la columna, si está visible
			col_PidPaciente.PackStart(cellr1, true);
			col_PidPaciente.AddAttribute (cellr1, "text", 2);    // la siguiente columna será 1 en vez de 1
			col_PidPaciente.SortColumnId = (int) Column3.col_PidPaciente;
			//cellr0.Editable = true;   // Permite edita este campo
       
           	TreeViewColumn col_area = new TreeViewColumn();
			CellRendererText cellrt10 = new CellRendererText();
			col_area.Title = "Area";
			col_area.PackStart(cellrt10, true);
			col_area.AddAttribute (cellrt10, "text", 3); // la siguiente columna será 8 en vez de 9
			col_area.SortColumnId = (int) Column3.col_area; 			
		
			TreeViewColumn col_Nombre1_Paciente = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_Nombre1_Paciente.Title = "Nombre Paciente";
			col_Nombre1_Paciente.PackStart(cellrt2, true);
			col_Nombre1_Paciente.AddAttribute (cellrt2, "text", 4); // la siguiente columna será 1 en vez de 2
			col_Nombre1_Paciente.SortColumnId = (int) Column3.col_Nombre1_Paciente;
       
		
			TreeViewColumn col_fechanacimiento_Paciente = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_fechanacimiento_Paciente.Title = "Fecha Ocupacion";
			col_fechanacimiento_Paciente.PackStart(cellrt6, true);
			col_fechanacimiento_Paciente.AddAttribute (cellrt6, "text", 5);     // la siguiente columna será 6 en vez de 7
			col_fechanacimiento_Paciente.SortColumnId = (int) Column3.col_fechanacimiento_Paciente;
       
			TreeViewColumn col_edad_Paciente = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_edad_Paciente.Title = "Edad";
			col_edad_Paciente.PackStart(cellrt7, true);
			col_edad_Paciente.AddAttribute (cellrt7, "text", 6); // la siguiente columna será 7 en vez de 8
			col_edad_Paciente.SortColumnId = (int) Column3.col_edad_Paciente;
       
			TreeViewColumn col_sexo_Paciente = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_sexo_Paciente.Title = "Sexo";
			col_sexo_Paciente.PackStart(cellrt8, true);
			col_sexo_Paciente.AddAttribute (cellrt8, "text", 7); // la siguiente columna será 8 en vez de 9
			col_sexo_Paciente.SortColumnId = (int) Column3.col_sexo_Paciente;
                   
			TreeViewColumn col_creacion_Paciente = new TreeViewColumn();
			CellRendererText cellrt9 = new CellRendererText();
			col_creacion_Paciente.Title = "Fecha de Admision";
			col_creacion_Paciente.PackStart(cellrt9, true);
			col_creacion_Paciente.AddAttribute (cellrt9, "text", 8); // la siguiente columna será 8 en vez de 9
			col_creacion_Paciente.SortColumnId = (int) Column3.col_creacion_Paciente;
		
			lista_de_Pacientes_asignados.AppendColumn(col_checkPac);
			lista_de_Pacientes_asignados.AppendColumn(col_foliodeatencion);
			lista_de_Pacientes_asignados.AppendColumn(col_PidPaciente);
			lista_de_Pacientes_asignados.AppendColumn(col_area);
			lista_de_Pacientes_asignados.AppendColumn(col_Nombre1_Paciente);
		
			lista_de_Pacientes_asignados.AppendColumn(col_fechanacimiento_Paciente);
			lista_de_Pacientes_asignados.AppendColumn(col_edad_Paciente);
			lista_de_Pacientes_asignados.AppendColumn(col_sexo_Paciente);
			lista_de_Pacientes_asignados.AppendColumn(col_creacion_Paciente);
		}
			
      	enum Column3
		{   
			col_checkPac,
			col_foliodeatencion,
			col_PidPaciente,
		    col_area,
			col_Nombre1_Paciente,
			col_Nombre2_Paciente,
			col_app_Paciente,
			col_apm_Paciente,
			col_fechanacimiento_Paciente,
			col_edad_Paciente,
			col_sexo_Paciente,
			col_creacion_Paciente		
		}
		
		
		void crea_treeview_busqueda()
		{

			treeViewEngineBusca = new TreeStore(typeof(bool),
			                                        typeof(int),
													typeof(int),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));
													
			lista_de_Pacientes.Model = treeViewEngineBusca;
			
			lista_de_Pacientes.RulesHint = true;
			
			//lista_de_Pacientes.RowActivated += on_selecciona_paciente_clicked;  // Doble click selecciono paciente*/
	                
			
			TreeViewColumn col_checkPac = new TreeViewColumn();
			CellRendererToggle cel_checkPac = new CellRendererToggle();			
			
				col_checkPac.Title = "Seleccionar"; // titulo de la cabecera de la columna, si está visible
			col_checkPac.PackStart(cel_checkPac, true);
			col_checkPac.AddAttribute (cel_checkPac, "active", 0);
			cel_checkPac.Activatable = true;
			cel_checkPac.Toggled += selecciona_fila_paciente;
			col_checkPac.SortColumnId = (int) Column.col_checkPac;
				
			TreeViewColumn col_foliodeatencion = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_foliodeatencion.Title = "Folio de Antencion"; // titulo de la cabecera de la columna, si está visible
			col_foliodeatencion.PackStart(cellr0, true);
			col_foliodeatencion.AddAttribute (cellr0, "text", 1);    // la siguiente columna será 1 en vez de 1
			col_foliodeatencion.SortColumnId = (int) Column.col_foliodeatencion;
		
			TreeViewColumn col_PidPaciente = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_PidPaciente.Title = "PID Paciente"; // titulo de la cabecera de la columna, si está visible
			col_PidPaciente.PackStart(cellr1, true);
			col_PidPaciente.AddAttribute (cellr1, "text", 2);    // la siguiente columna será 1 en vez de 1
			col_PidPaciente.SortColumnId = (int) Column.col_PidPaciente;
			//cellr0.Editable = true;   // Permite edita este campo
       
			TreeViewColumn col_Nombre1_Paciente = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_Nombre1_Paciente.Title = "Nombre 1";
			col_Nombre1_Paciente.PackStart(cellrt2, true);
			col_Nombre1_Paciente.AddAttribute (cellrt2, "text", 3); // la siguiente columna será 1 en vez de 2
			col_Nombre1_Paciente.SortColumnId = (int) Column.col_Nombre1_Paciente;
       
			TreeViewColumn col_Nombre2_Paciente = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_Nombre2_Paciente.Title = "Nombre 2";
			col_Nombre2_Paciente.PackStart(cellrt3, true);
			col_Nombre2_Paciente.AddAttribute (cellrt3, "text", 4); // la siguiente columna será 2 en vez de 3
			col_Nombre2_Paciente.SortColumnId = (int) Column.col_Nombre2_Paciente;
       
			TreeViewColumn col_app_Paciente = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_app_Paciente.Title = "Apellido Paterno";
			col_app_Paciente.PackStart(cellrt4, true);
			col_app_Paciente.AddAttribute (cellrt4, "text", 5); // la siguiente columna será 3 en vez de 4
			col_app_Paciente.SortColumnId = (int) Column.col_app_Paciente;
       
			TreeViewColumn col_apm_Paciente = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_apm_Paciente.Title = "Apellido Materno";
			col_apm_Paciente.PackStart(cellrt5, true);
			col_apm_Paciente.AddAttribute (cellrt5, "text", 6); // la siguiente columna será 5 en vez de 6
			col_apm_Paciente.SortColumnId = (int) Column.col_apm_Paciente;
 
			TreeViewColumn col_fechanacimiento_Paciente = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_fechanacimiento_Paciente.Title = "Fecha de Nacimiento";
			col_fechanacimiento_Paciente.PackStart(cellrt6, true);
			col_fechanacimiento_Paciente.AddAttribute (cellrt6, "text", 7);     // la siguiente columna será 6 en vez de 7
			col_fechanacimiento_Paciente.SortColumnId = (int) Column.col_fechanacimiento_Paciente;
       
			TreeViewColumn col_edad_Paciente = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_edad_Paciente.Title = "Edad";
			col_edad_Paciente.PackStart(cellrt7, true);
			col_edad_Paciente.AddAttribute (cellrt7, "text", 8); // la siguiente columna será 7 en vez de 8
			col_edad_Paciente.SortColumnId = (int) Column.col_edad_Paciente;
       
			TreeViewColumn col_sexo_Paciente = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_sexo_Paciente.Title = "Sexo";
			col_sexo_Paciente.PackStart(cellrt8, true);
			col_sexo_Paciente.AddAttribute (cellrt8, "text", 9); // la siguiente columna será 8 en vez de 9
			col_sexo_Paciente.SortColumnId = (int) Column.col_sexo_Paciente;
                   
			TreeViewColumn col_creacion_Paciente = new TreeViewColumn();
			CellRendererText cellrt9 = new CellRendererText();
			col_creacion_Paciente.Title = "Fecha creacion";
			col_creacion_Paciente.PackStart(cellrt9, true);
			col_creacion_Paciente.AddAttribute (cellrt9, "text", 10); // la siguiente columna será 8 en vez de 9
			col_creacion_Paciente.SortColumnId = (int) Column.col_creacion_Paciente;
           
		    lista_de_Pacientes.AppendColumn(col_checkPac);
			lista_de_Pacientes.AppendColumn(col_foliodeatencion);
			lista_de_Pacientes.AppendColumn(col_PidPaciente);
			lista_de_Pacientes.AppendColumn(col_Nombre1_Paciente);
			lista_de_Pacientes.AppendColumn(col_Nombre2_Paciente);
			lista_de_Pacientes.AppendColumn(col_app_Paciente);
			lista_de_Pacientes.AppendColumn(col_apm_Paciente);
			lista_de_Pacientes.AppendColumn(col_fechanacimiento_Paciente);
			lista_de_Pacientes.AppendColumn(col_edad_Paciente);
			lista_de_Pacientes.AppendColumn(col_sexo_Paciente);
			lista_de_Pacientes.AppendColumn(col_creacion_Paciente);
		}		
		
		enum Column
		{   
			col_checkPac,
			col_foliodeatencion,
			col_PidPaciente,
			col_Nombre1_Paciente,
			col_Nombre2_Paciente,
			col_app_Paciente,
			col_apm_Paciente,
			col_fechanacimiento_Paciente,
			col_edad_Paciente,
			col_sexo_Paciente,
			col_creacion_Paciente
		}		
		
		void selecciona_fila_paciente(object sender, ToggledArgs args)
		{			
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (lista_de_Pacientes.Model.GetIter (out iter, path)){
				bool oldPac = (bool) lista_de_Pacientes.Model.GetValue (iter,0);
				lista_de_Pacientes.Model.SetValue(iter,0,!oldPac);
				
                if (oldPac == false && activoPac == true){
                	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Info, 
					ButtonsType.Ok,"Ya tiene seleccionada un paciente, Desmarquelo para seleccionar uno nuevo ");
					msgBoxError.Run ();
					msgBoxError.Destroy();
				    lista_de_Pacientes.Model.SetValue (iter,0,false); }	
				
               if (oldPac == false && activoPac == false){
                    activoPac = true;	
					entry_paciente.Text = lista_de_Pacientes.Model.GetValue(iter,3) + " " +lista_de_Pacientes.Model.GetValue(iter,4) + " "+ lista_de_Pacientes.Model.GetValue(iter,5) + " " + lista_de_Pacientes.Model.GetValue(iter,6);
 			        pid = (int) lista_de_Pacientes.Model.GetValue(iter,2);
				    folio_atencion = (int) lista_de_Pacientes.Model.GetValue(iter,1);						
					
					entry_folio.Text =  Convert.ToString(lista_de_Pacientes.Model.GetValue(iter,1)); 
					entry_dia_ocupacion.Text = DateTime.Today.ToString("dd");
					entry_mes_ocupacion.Text = DateTime.Today.ToString("MM");
					entry_anno_ocupacion.Text = DateTime.Today.ToString("yyyy");
				}
				
				if (oldPac == true && activoPac == true){
					activoPac = false;
					entry_folio.Text = "";	
					entry_anno_ocupacion.Text = "";
					entry_dia_ocupacion.Text = "";
					entry_mes_ocupacion.Text = "";
					entry_paciente.Text = "";
					folio_atencion = 0;
					pid = 0;				
				}
			}
		}		
		
		void selecciona_fila_paciente_asigando(object sender, ToggledArgs args)
		{
			
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (lista_de_Pacientes_asignados.Model.GetIter (out iter, path)){
				bool oldPac = (bool) lista_de_Pacientes_asignados.Model.GetValue (iter,0);
				lista_de_Pacientes_asignados.Model.SetValue(iter,0,!oldPac);
				
                if (oldPac == false && activoPac == true){
                	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info, 
							ButtonsType.Ok,"Ya tiene seleccionada un paciente, Desmarquelo para seleccionar uno nuevo ");
					msgBoxError.Run ();					msgBoxError.Destroy();
					lista_de_Pacientes_asignados.Model.SetValue (iter,0,false); }	
				
					if (oldPac == false && activoPac == false){
						NpgsqlConnection conexion;
						conexion = new NpgsqlConnection (connectionString+nombrebd );
					    // Verifica que la base de datos este conectada
						try{
							conexion.Open ();
							NpgsqlCommand comando; 
							comando = conexion.CreateCommand ();
											
							comando.CommandText = "SELECT osiris_erp_cobros_enca.pid_paciente,osiris_his_habitaciones.id_habitacion, "+
														          "numero_cuarto,descripcion_cuarto,id_tipo_admisiones, "+
															      "to_char(fecha_de_ocupacion,'dd-MM-yyyy') as fechadeocupacion "+
														          "FROM osiris_erp_cobros_enca,osiris_his_habitaciones "+
															      "WHERE (osiris_erp_cobros_enca.folio_de_servicio = osiris_his_habitaciones.folio_de_servicio)"+
														          "AND osiris_erp_cobros_enca.folio_de_servicio ='"+(int) lista_de_Pacientes_asignados.Model.GetValue(iter,1)+"'; ";
													           // "AND (osiris_erp_cobros_enca.pid_paciente = osiris_his_habitaciones.pid_paciente);";
											        
							NpgsqlDataReader lector = comando.ExecuteReader ();
												
							if ((bool) lector.Read()){
								id_habitacion_proveniente =  ((int) lector["id_habitacion"]); //guarda en la variable la habitacion de la que proviene (cambio de habitacion)
								entry_id_habitacion.Text = Convert.ToString ((int) lector["id_habitacion"]);
								entry_area.Text = Convert.ToString ((int) lector["id_tipo_admisiones"]); 			
								entry_numero_habitacion.Text = Convert.ToString ((int) lector["numero_cuarto"]);		
								entry_descripcion.Text = ((string) lector["descripcion_cuarto"]);			
								entry_dia_ocupacion.Text = Convert.ToString((string) lector["fechadeocupacion"]).Substring(0,2);
								entry_mes_ocupacion.Text = Convert.ToString((string) lector["fechadeocupacion"]).Substring(3,2);
								entry_anno_ocupacion.Text = Convert.ToString((string) lector["fechadeocupacion"]).Substring(6,4);
							}									
						}catch (NpgsqlException ex){
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,	MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();		msgBoxError.Destroy();
						}       
						conexion.Close ();
										
						activoPac = true;		
						treeview_habitaciones.Sensitive = true;
						pid_pasingado = (int) lista_de_Pacientes_asignados.Model.GetValue(iter,2);
						folio_atencion_pasignado = (int) lista_de_Pacientes_asignados.Model.GetValue(iter,1);						
						entry_folio.Text =  Convert.ToString(lista_de_Pacientes_asignados.Model.GetValue(iter,1)); 
                        entry_paciente.Text = (string) lista_de_Pacientes_asignados.Model.GetValue(iter,4);
				}				
			  
				if (oldPac == true && activoPac == true){
					activoPac = false;
					treeview_habitaciones.Sensitive = false;
					treeViewEngineBuscahabitacion.Clear();
					descripcion_area = "";
					entry_id_habitacion.Text = "";
					entry_area.Text = "";	
					entry_descripcion.Text = "";
					entry_numero_habitacion.Text = "";
					entry_folio.Text = "";
				    entry_anno_ocupacion.Text = "";
				    entry_dia_ocupacion.Text = "";
	        	    entry_mes_ocupacion.Text = "";
				    entry_paciente.Text = "";
				    folio_atencion = 0;
				    pid_pasingado = 0;				
					llena_lista_habitaciones();				
				}
			}	
        }		
		
		
		void on_buscar_paciente_clicked ()
		{
			treeViewEngineBusca.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
	   			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	               	
				if ((string) entry_expresion.Text.ToString() == ""){
					comando.CommandText = "SELECT DISTINCT(osiris_erp_movcargos.folio_de_servicio),osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, "+
							" nombre1_paciente,nombre2_paciente, apellido_paterno_paciente, "+
							"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
							"sexo_paciente,to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,osiris_his_habitaciones.id_tipo_admisiones "+
							"FROM osiris_his_paciente,osiris_erp_cobros_enca,osiris_erp_movcargos,osiris_his_habitaciones "+
							"WHERE osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
							"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
							"AND alta_paciente = 'false' "+
							"AND osiris_his_habitaciones.id_habitacion = osiris_erp_cobros_enca.id_habitacion "+
						    //"AND osiris_his_habitaciones.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones " +
							"AND pagado = 'false' "+
							"AND cerrado = 'false' "+
							"AND reservacion = 'false' "+
							"AND cancelado = 'false' "+
							"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
							"AND osiris_erp_cobros_enca.alta_paciente = false "+
							"AND osiris_erp_cobros_enca.cancelado = false "+
							"AND osiris_erp_cobros_enca.id_habitacion = 1 "+
								"AND osiris_erp_movcargos.id_tipo_admisiones > '16' "+
								"AND osiris_erp_movcargos.id_tipo_admisiones <> '940' "+
								"AND osiris_erp_movcargos.id_tipo_admisiones <> '930' "+
								"AND osiris_erp_movcargos.id_tipo_admisiones <> '920' "+
								"AND osiris_erp_movcargos.id_tipo_admisiones <> '950' "+
								"AND osiris_erp_movcargos.id_tipo_admisiones <> '300' "+
								"AND osiris_erp_movcargos.id_tipo_admisiones <> '400' "+
							"ORDER BY osiris_erp_cobros_enca.folio_de_servicio;";
					Console.WriteLine(comando.CommandText);
				}else{              	
					if (radiobutton_busca_apellido.Active == true){
						comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, "+
							" nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_habitacion, "+
							"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
							"sexo_paciente,to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
							"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
							"WHERE alta_paciente = 'false' "+
							"AND pagado = 'false' "+
							"AND cerrado = 'false' "+
							"AND reservacion = 'false' "+
							"AND cancelado = 'false' "+
							"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
							"AND osiris_erp_cobros_enca.alta_paciente = false "+
							"AND osiris_erp_cobros_enca.cancelado = false "+
							"AND osiris_erp_cobros_enca.id_habitacion = 1 "+	
							"AND apellido_paterno_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' ORDER BY folio_de_servicio;";
					}
					if (radiobutton_busca_nombre.Active == true){
						comando.CommandText =  "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, "+
							" nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_habitacion, "+
							"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
							"sexo_paciente,to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
							"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
							"WHERE alta_paciente = 'false' "+
							"AND pagado = 'false' "+
							"AND cerrado = 'false' "+
							"AND reservacion = 'false' "+
							"AND cancelado = 'false' "+
							"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
							"AND osiris_erp_cobros_enca.alta_paciente = false "+
							"AND osiris_erp_cobros_enca.cancelado = false "+
							"AND osiris_erp_cobros_enca.id_habitacion = 1 "+
							"AND nombre1_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' ORDER BY folio_de_servicio;";
					}
					if (radiobutton_busca_expediente.Active == true){
						comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, "+
							" nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_habitacion, "+
							"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
							"sexo_paciente,to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
							"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
							"WHERE alta_paciente = 'false' "+
							"AND pagado = 'false' "+
							"AND cerrado = 'false' "+
							"AND reservacion = 'false' "+
							"AND cancelado = 'false' "+
							"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
							"AND osiris_erp_cobros_enca.alta_paciente = false "+
							"AND osiris_erp_cobros_enca.cancelado = false "+
							"AND osiris_erp_cobros_enca.id_habitacion = 1 "+
							"AND osiris_his_paciente.pid_paciente = '"+entry_expresion.Text+"' ORDER BY folio_de_servicio;";			
					}
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					treeViewEngineBusca.AppendValues ((bool) false,(int) lector["folio_de_servicio"],//TreeIter iter =
										(int) lector["pid_paciente"],
										(string) lector["nombre1_paciente"],(string) lector["nombre2_paciente"],
										(string) lector["apellido_paterno_paciente"], (string) lector["apellido_materno_paciente"],
										(string) lector["fech_nacimiento"], (string) lector["edad"],
										(string) lector["sexo_paciente"],
										(string) lector["fech_creacion"]);
						
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_selecciona_paciente_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
 			if (lista_de_Pacientes.Selection.GetSelected(out model, out iterSelected)){
 				this.entry_paciente.Text = model.GetValue(iterSelected,2) + " " + model.GetValue(iterSelected,3) + " "+ model.GetValue(iterSelected,4) + " " + model.GetValue(iterSelected,5);
 			    pid = (int) model.GetValue(iterSelected,1);
				folio_atencion = (int) model.GetValue(iterSelected,0);
			}          
			this.entry_dia_ocupacion.Text = DateTime.Today.ToString("dd");
			this.entry_mes_ocupacion.Text = DateTime.Today.ToString("MM");
			this.entry_anno_ocupacion.Text = DateTime.Today.ToString("yyyy");
 		}		
		
      
		void limpia_variables()
		{			        
			this.treeViewEngineBuscahabitacion.Clear();
			on_buscar_paciente_clicked();
			this.on_busca_paciente_Asigancion();
			activo = false;
			activoPac = false;
			llena_lista_habitaciones();
			this.entry_folio.Text = "";	
			this.entry_anno_ocupacion.Text = "";
			this.entry_dia_ocupacion.Text = "";
			this.entry_mes_ocupacion.Text = "";
			this.entry_paciente.Text = "";
			this.folio_atencion = 0;
			this.pid = 0;			
			query_fecha_de_ocupacion = "";
			descripcion_area = "";
			id_habitacion_proveniente = 0;
			cambio = false;
			pid_pasingado = 0;
			idpacientehistorial = 0;
		}

		void on_reporte_clicked(object sender, EventArgs args)
		{
			new osiris.reporte_pacientes_sin_alta(nombrebd);
		}
		
		void on_cierraventanas_clicked(object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}
