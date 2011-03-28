// created on 20/06/2010
///////////////////////////////////////////////////////////
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. (Programacion Base y Ajustes)
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
		[Widget] Gtk.Button button_imprimir_notas = null;
		[Widget] Gtk.Entry entry_fechanotas = null;
		[Widget] Gtk.TreeView treeview_listanotas = null;
		[Widget] Gtk.ComboBox combobox_hora_nota = null;
		[Widget] Gtk.ComboBox combobox_minutos_nota = null;
		[Widget] Gtk.CheckButton checkbutton_selectall = null;
		
		TextBuffer buffer = new TextBuffer (null);
		TextIter insertIter;
		
		// Somatometria
		[Widget] Gtk.Entry entry_presion_arterial = null;
		[Widget] Gtk.SpinButton spinbutton_pulso = null;
		[Widget] Gtk.SpinButton spinbutton_frecrespiratoria = null;
		[Widget] Gtk.SpinButton spinbutton_temperatura = null;
		[Widget] Gtk.SpinButton spinbutton_sat_oxigeno = null;
		[Widget] Gtk.Entry entry_diuresis = null;
		[Widget] Gtk.Entry entry_evacuacion = null;
		[Widget] Gtk.ComboBox combobox_hora_somato = null;
		[Widget] Gtk.ComboBox combobox_minutos_somato = null;
		[Widget] Gtk.TreeView treeview_lista_somatometria = null;
		[Widget] Gtk.Button button_guardar_somato = null;
			
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
		string diagnosticoadmision;
		
		string hora_nota = "";
		string minutos_nota = "";
		string hora_somatometria = "";
		string minutos_somatometria = "";
		
		string sql_general = "SELECT notas_de_enfermeria,notas_de_evolucion,indicaciones_medicas,nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad," +
							"to_char(osiris_his_paciente.fecha_nacimiento_paciente,'dd-mm-yyyy') AS fechanacimiento_pac,"+
							"to_char(fecha_anotacion,'dd-MM-yyyy') AS fechaanotacion,hora_anotacion AS horaanotacion,osiris_his_informacion_medica.id_secuencia,"+
							"alegias_paciente,osiris_his_paciente.pid_paciente,to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-mm-yyyy HH:mi') AS fechadeingreso,"+
							"to_char(osiris_erp_cobros_enca.fecha_alta_paciente,'dd-mm-yyyy HH:mi') AS fechadeegreso,osiris_his_paciente.sexo_paciente,"+
							"osiris_erp_cobros_enca.id_habitacion,osiris_his_habitaciones.descripcion_cuarto,osiris_his_habitaciones.numero_cuarto,"+
							"nombre1_empleado || ' ' || nombre2_empleado || ' ' || apellido_paterno_empleado || ' ' || apellido_materno_empleado AS nombreempleado "+
							"FROM osiris_his_informacion_medica,osiris_his_paciente,osiris_empleado,osiris_erp_cobros_enca,osiris_his_habitaciones "+
									"WHERE osiris_his_informacion_medica.pid_paciente = osiris_his_paciente.pid_paciente "+
									"AND osiris_his_informacion_medica.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
									"AND id_empleado_creacion = login_empleado "+
									"AND osiris_erp_cobros_enca.id_habitacion = osiris_his_habitaciones.id_habitacion ";
		string sql_pidpaciente;
		string sql_folioservicio;
		string sql_filtronotasblanco;
		
		private TreeStore treeViewEngineListaNotas;
		private TreeStore treeViewEngineSomatometria;
		
		TreeViewColumn col_00;		CellRendererToggle cellrt00;
		TreeViewColumn col_01;		CellRendererText cellrt01;
		TreeViewColumn col_02;		CellRendererText cellrt02;
		TreeViewColumn col_03;		CellRendererText cellrt03;
		TreeViewColumn col_04;		CellRendererText cellrt04;
		TreeViewColumn col_05;		CellRendererText cellrt05;
			
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public notas_medicas (string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_,
		                      string title_window, string name_field_,string pidpaciente_,string folioservicio_,string iddoctor_,string nombredoctor_,string nombrepaciente_,
		                      bool altapaciente_,string edadpaciente_, string diagnosticoadmision_)
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
			diagnosticoadmision = diagnosticoadmision_;
			
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
			
			button_imprimir_notas.Clicked += new EventHandler(on_button_imprimir_notas_clicked);
			checkbutton_selectall.Clicked += new EventHandler(on_checkbutton_selectall_clicked);
			entry_fechanotas.Text = (string) DateTime.Now.ToString("yyyy-MM-dd");
			entry_pid_paciente.Text = (string) pidpaciente;
			entry_nombre_paciente.Text = (string) nombrepaciente_;
			entry_numerotencion.Text = (string) folioservicio;
			entry_id_doctor.Text = (string) iddoctor_;
			entry_doctor.Text = (string) nombredoctor_;
			entry_edad_paciente.Text = (string) edadpaciente_;
			
			// action somatometria
			button_guardar_somato.Clicked += new EventHandler(on_button_guardar_somato_clicked);
			
			// Cambiando el color del fondo para distinguir la ventana
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
			if (altapaciente_ == false){
				button_guardar.Sensitive = false;
				textview2.Sensitive = false;
				entry_fechanotas.Sensitive = false;
				combobox_hora_nota.Sensitive = false;
				combobox_minutos_nota.Sensitive = false;
			}
			crea_treeview_notas();
			crea_treeview_somatometria();
			llena_horas_notas();
			llenando_informacion();
		}
		
		void on_button_imprimir_notas_clicked (object sender, EventArgs args)
		{
			string numeros_seleccionado = "";
			string almacenes_seleccionados = ""; 
			string variable_paso_03 = "";
			int variable_paso_02_1 = 0;
			string query_in_num = "";
			
			//poder elegir una fila del treeview
			TreeIter iter;
			if (treeViewEngineListaNotas.GetIterFirst (out iter)){			
 				if ((bool) treeview_listanotas.Model.GetValue (iter,0) == true){
					numeros_seleccionado = (string) treeview_listanotas.Model.GetValue (iter,1);
 					variable_paso_02_1 += 1;		
 				}
 				while (treeViewEngineListaNotas.IterNext(ref iter)){
 					if ((bool) treeview_listanotas.Model.GetValue (iter,0) == true){
						if (variable_paso_02_1 == 0){ 				    	
 							numeros_seleccionado = (string) treeview_listanotas.Model.GetValue (iter,1);
 							variable_paso_02_1 += 1;
 						}else{
 							variable_paso_03 = (string) treeview_listanotas.Model.GetValue (iter,1);
 							numeros_seleccionado = numeros_seleccionado.Trim() + "','" + variable_paso_03.Trim();
 						}
 					}
 				}
 			}
			if (variable_paso_02_1 > 0){
	 			query_in_num = " AND id_secuencia IN('"+numeros_seleccionado+"') ";
			}
			if ( treeViewEngineListaNotas.GetIterFirst (out iter)){
				if (variable_paso_02_1 > 0){
					Console.WriteLine(query_in_num);
					new osiris.rpt_notas_medicas(folioservicio,name_field,sql_general+sql_pidpaciente+sql_folioservicio+sql_filtronotasblanco+query_in_num+" ORDER BY to_char(fecha_anotacion,'yyyy-MM-dd'),hora_anotacion DESC;", diagnosticoadmision);
				}
			}
		}
		
		void llenando_informacion()
		{
			buffer = textview1.Buffer;
			classpublic.CreateTags(buffer);
			insertIter = buffer.StartIter;
			buffer.Clear();		
			treeViewEngineListaNotas.Clear();
						
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = sql_general+sql_pidpaciente+sql_folioservicio+sql_filtronotasblanco+" ORDER BY to_char(fecha_anotacion,'yyyy-MM-dd'),hora_anotacion DESC;";
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
						buffer.InsertWithTagsByName (ref insertIter, "Hora de Nota : "+(string) lector["horaanotacion"].ToString().Trim()+" \n\n", "bold");
						buffer.Insert (ref insertIter, (string) lector[name_field].ToString().ToUpper()+"\n\n\n");
						treeViewEngineListaNotas.AppendValues(false,
						                                      (string) lector["id_secuencia"].ToString().Trim(),
						                                      (string) lector["fechaanotacion"].ToString().Trim(),
						                                      (string) lector["horaanotacion"].ToString().Trim(),
						                                      (string) lector["nombreempleado"].ToString().Trim());
						
					}
					while(lector.Read()){
						if((string) lector[name_field].ToString() != ""){
							buffer.InsertWithTagsByName (ref insertIter, "Fecha de Nota: "+(string) lector["fechaanotacion"].ToString().Trim()+"    Nº de NOTA :"+(string) lector["id_secuencia"].ToString().Trim()+"\n", "bold");
							buffer.InsertWithTagsByName (ref insertIter, "Hora de Nota : "+(string) lector["horaanotacion"].ToString().Trim()+" \n\n", "bold");
							buffer.Insert (ref insertIter, (string) lector[name_field].ToString().ToUpper()+"\n\n\n");
							treeViewEngineListaNotas.AppendValues(false,
							                                      (string) lector["id_secuencia"].ToString().Trim(),
							                                      (string) lector["fechaanotacion"].ToString().Trim(),
							                                      (string) lector["horaanotacion"].ToString().Trim(),
							                                      (string) lector["nombreempleado"].ToString().Trim());
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
			
			// Llenado de Somatometria
			
			
			
		}
		
		void crea_treeview_notas()
		{
			treeViewEngineListaNotas = new TreeStore( typeof(bool),
													typeof(string),
													typeof(string),
			                                        typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));
				
			treeview_listanotas.Model = treeViewEngineListaNotas;
			treeview_listanotas.RulesHint = true;
			
			col_00 = new TreeViewColumn();
			cellrt00 = new CellRendererToggle();
			col_00.Title = "Selecciona"; // titulo de la cabecera de la columna, si está visible
			col_00.PackStart(cellrt00, true);
			col_00.AddAttribute (cellrt00, "active", 0);
			cellrt00.Activatable = true;
			cellrt00.Toggled += selecciona_fila;
			//col_00.SortColumnId = (int) Column_notas.col_00;
			
			col_01 = new TreeViewColumn();
			cellrt01 = new CellRendererText();
			col_01.Title = "N° Nota"; // titulo de la cabecera de la columna, si está visible
			col_01.PackStart(cellrt01, true);
			col_01.AddAttribute (cellrt01, "text", 1);
			//col_01.SortColumnId = (int) Column_notas.col_01;
			
			col_02 = new TreeViewColumn();
			cellrt02 = new CellRendererText();
			col_02.Title = "Fecha Nota"; // titulo de la cabecera de la columna, si está visible
			col_02.PackStart(cellrt02, true);
			col_02.AddAttribute (cellrt02, "text", 2);
			//col_03.SortColumnId = (int) Column_notas.col_01;
			
			col_03 = new TreeViewColumn();
			cellrt03 = new CellRendererText();
			col_03.Title = "Hora Nota"; // titulo de la cabecera de la columna, si está visible
			col_03.PackStart(cellrt03, true);
			col_03.AddAttribute (cellrt03, "text", 3);
			//col_03.SortColumnId = (int) Column_notas.col_02;
			
			col_04 = new TreeViewColumn();
			cellrt04 = new CellRendererText();
			col_04.Title = "Quien Realizo"; // titulo de la cabecera de la columna, si está visible
			col_04.PackStart(cellrt04, true);
			col_04.AddAttribute (cellrt04, "text", 4);
			//col_03.SortColumnId = (int) Column_notas.col_03;
			
			treeview_listanotas.AppendColumn(col_00);
			treeview_listanotas.AppendColumn(col_01);
			treeview_listanotas.AppendColumn(col_02);
			treeview_listanotas.AppendColumn(col_03);
			treeview_listanotas.AppendColumn(col_04);
		}
		
		void crea_treeview_somatometria()
		{
			treeViewEngineSomatometria = new TreeStore(typeof(string),
													typeof(string),
			                                        typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));
				
			treeview_lista_somatometria.Model = treeViewEngineSomatometria;
			treeview_lista_somatometria.RulesHint = true;
			
			col_01 = new TreeViewColumn();
			cellrt01 = new CellRendererText();
			col_01.Title = "Fecha";
			col_01.PackStart(cellrt00, true);
			col_01.AddAttribute (cellrt00, "text", 0);
			//col_01.SortColumnId = (int) Column_notas.col_01;
			
			col_02 = new TreeViewColumn();
			cellrt02 = new CellRendererText();
			col_02.Title = "Hora";
			col_02.PackStart(cellrt02, true);
			col_02.AddAttribute (cellrt02, "text", 1);
			//col_03.SortColumnId = (int) Column_notas.col_02;
			
			col_03 = new TreeViewColumn();
			cellrt03 = new CellRendererText();
			col_03.Title = "Tension Arterial";
			col_03.PackStart(cellrt03, true);
			col_03.AddAttribute (cellrt03, "text", 2);
			//col_03.SortColumnId = (int) Column_notas.col_03;
			
			col_04 = new TreeViewColumn();
			cellrt04 = new CellRendererText();
			col_04.Title = "Pulso";
			col_04.PackStart(cellrt04, true);
			col_04.AddAttribute (cellrt04, "text", 3);
			//col_03.SortColumnId = (int) Column_notas.col_04;
			
			col_05 = new TreeViewColumn();
			cellrt05 = new CellRendererText();
			col_05.Title = "Fr. Resp.";
			col_05.PackStart(cellrt05, true);
			col_05.AddAttribute (cellrt05, "text", 4);
			//col_03.SortColumnId = (int) Column_notas.col_04;
			
			treeview_lista_somatometria.AppendColumn(col_01);
			treeview_lista_somatometria.AppendColumn(col_02);
			treeview_lista_somatometria.AppendColumn(col_03);
			treeview_lista_somatometria.AppendColumn(col_04);
			treeview_lista_somatometria.AppendColumn(col_05);
		}
		
		// Cuando seleccion el treeview de cargos extras para cargar los productos  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			if (treeview_listanotas.Model.GetIter (out iter,new TreePath (args.Path))) {
				bool old = (bool) treeview_listanotas.Model.GetValue (iter,0);
				treeview_listanotas.Model.SetValue(iter,0,!old);
			}	
		}
		
		void on_button_guardar_clicked(object sender, EventArgs args)
		{
			if(textview2.Buffer.Text.ToString()!=""){				
				if(hora_nota != "" && minutos_nota != ""){
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
												MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de Almacenar esta NOTA ?");
					ResponseType miResultado = (ResponseType)
					msgBox.Run ();				msgBox.Destroy();
		 			if (miResultado == ResponseType.Yes){				
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
							textview2.Buffer.Text.ToString().ToUpper()+"','"+
							DateTime.Now.ToString("yyyy-MM-dd")+"','"+
							hora_nota+":"+minutos_nota
							+"')";
							//Console.WriteLine(comando.CommandText);
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
					}			
					//Console.WriteLine(textview2.Buffer.Text.ToString());
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
										MessageType.Error,ButtonsType.Close,"La nota no tiene hora o minutos, verifique...");
					msgBoxError.Run ();						msgBoxError.Destroy();	
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
										MessageType.Error,ButtonsType.Close,"La nota no contiene informacion, verifique...");
				msgBoxError.Run ();						msgBoxError.Destroy();
			}
		}
		
		void llena_horas_notas()
		{
			combobox_hora_nota.Clear();
			CellRendererText cell2 = new CellRendererText();
			combobox_hora_nota.PackStart(cell2, true);
			combobox_hora_nota.AddAttribute(cell2,"text",0);	        
			ListStore store2 = new ListStore( typeof (string), typeof (int));
			combobox_hora_nota.Model = store2;
			for(int i = 1; i < (int)classpublic.horario_24_horas+1 ; i++){				
				store2.AppendValues ((string)i.ToString("00").Trim());
			}
			combobox_hora_nota.Changed += new EventHandler (onComboBoxChanged_hora_minutos_cita);			
			
			combobox_minutos_nota.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_minutos_nota.PackStart(cell3, true);
			combobox_minutos_nota.AddAttribute(cell3,"text",0);	        
			ListStore store3 = new ListStore( typeof (string), typeof (int));
			combobox_minutos_nota.Model = store3;			
			for(int i = (int) 0; i < 60 ; i=i+(int) classpublic.intervalo_minutos){				
				store3.AppendValues ((string)i.ToString("00").Trim());
			}
			combobox_minutos_nota.Changed += new EventHandler (onComboBoxChanged_hora_minutos_cita);
			
			combobox_hora_somato.Clear();
			CellRendererText cell4 = new CellRendererText();
			combobox_hora_somato.PackStart(cell4, true);
			combobox_hora_somato.AddAttribute(cell4,"text",0);	        
			ListStore store4 = new ListStore( typeof (string), typeof (int));
			combobox_hora_somato.Model = store4;
			for(int i = 1; i < (int)classpublic.horario_24_horas+1 ; i++){				
				store4.AppendValues ((string)i.ToString("00").Trim());
			}
			combobox_hora_somato.Changed += new EventHandler (onComboBoxChanged_hora_minutos_cita);
			
			combobox_minutos_somato.Clear();
			CellRendererText cell5 = new CellRendererText();
			combobox_minutos_somato.PackStart(cell5, true);
			combobox_minutos_somato.AddAttribute(cell5,"text",0);	        
			ListStore store5 = new ListStore( typeof (string), typeof (int));
			combobox_minutos_somato.Model = store5;			
			for(int i = (int) 0; i < 60 ; i=i+(int) classpublic.intervalo_minutos){				
				store5.AppendValues ((string)i.ToString("00").Trim());
			}
			combobox_minutos_somato.Changed += new EventHandler (onComboBoxChanged_hora_minutos_cita);
		}
		
		void onComboBoxChanged_hora_minutos_cita(object sender, EventArgs args)
		{
			//Gtk.ComboBox hora_minutos_cita = (Gtk.ComboBox) sender;
			Gtk.ComboBox hora_minutos = sender as Gtk.ComboBox;			
			if (sender == null){
				return;
			}
			TreeIter iter;
			if (hora_minutos.GetActiveIter (out iter)){
				if(hora_minutos.Name.ToString() == "combobox_hora_nota"){				
					hora_nota = (string) hora_minutos.Model.GetValue(iter,0);
				}			
				if(hora_minutos.Name.ToString() == "combobox_minutos_nota"){
					minutos_nota = (string) hora_minutos.Model.GetValue(iter,0);
				}
				if(hora_minutos.Name.ToString() == "combobox_hora_somato"){				
					hora_somatometria = (string) hora_minutos.Model.GetValue(iter,0);
				}			
				if(hora_minutos.Name.ToString() == "combobox_minutos_somato"){
					minutos_somatometria = (string) hora_minutos.Model.GetValue(iter,0);
				}
			}
		}
		
		//Seleccionar todos los del treeview, un check_button 
		void on_checkbutton_selectall_clicked(object sender, EventArgs args)
		{
			if ((bool)checkbutton_selectall.Active == true){
				TreeIter iter2;
				if (treeViewEngineListaNotas.GetIterFirst (out iter2)){
					treeview_listanotas.Model.SetValue(iter2,0,true);
					while (treeViewEngineListaNotas.IterNext(ref iter2)){
						treeview_listanotas.Model.SetValue(iter2,0,true);
					}
				}
			}else{
				TreeIter iter2;
				if (treeViewEngineListaNotas.GetIterFirst (out iter2)){
					treeview_listanotas.Model.SetValue(iter2,0,false);
					while (treeViewEngineListaNotas.IterNext(ref iter2)){
						treeview_listanotas.Model.SetValue(iter2,0,false);
					}
				}
			}
		}
		
		void on_button_guardar_somato_clicked(object sender, EventArgs args)
		{
			if(hora_somatometria != "" && minutos_somatometria != ""){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
											MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de Almacenar esta SOMATOMETRIA ?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
	 			if (miResultado == ResponseType.Yes){
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
	            	try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand();
						
						comando.CommandText = "INSERT INTO osiris_his_somatometria (pid_paciente,folio_de_servicio,"+
							"fechahora_creacion,id_empleado_creacion,hora_somatometria,"+
							"tension_arterial,pulso,frecuencia_respiratoria,temperatura,saturacion_oxigeno,"+
							"diuresis,evacuacion"+
							") VALUES ('"+
							(string) entry_pid_paciente.Text.ToString().Trim()+"','"+
							(string) entry_numerotencion.Text.ToString().Trim()+"','"+
							DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
							LoginEmpleado+"','"+
							hora_somatometria+":"+minutos_somatometria+"','"+
							entry_presion_arterial.Text.Trim()+"','"+
							spinbutton_pulso.Text.Trim()+"','"+
							spinbutton_frecrespiratoria.Text.Trim()+"','"+
							spinbutton_temperatura.Text.Trim()+"','"+
							spinbutton_sat_oxigeno.Text.Trim()+"','"+
							entry_diuresis.Text.Trim().ToUpper()+"','"+
							entry_evacuacion.Text.Trim().ToUpper()								
							+"')";
						Console.WriteLine(comando.CommandText);
						comando.ExecuteNonQuery();
						comando.Dispose();						
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
				 							ButtonsType.Close, "Los datos se guardaron con EXITO");
						msgBoxError.Run ();			msgBoxError.Destroy();						
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
							MessageType.Error, 
							ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();					msgBoxError.Destroy();	   			
		       		}
	       			conexion.Close ();
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Error,ButtonsType.Close,"La Somatometria no tiene hora o minutos, verifique...");
				msgBoxError.Run ();						msgBoxError.Destroy();
			}
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked(object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}