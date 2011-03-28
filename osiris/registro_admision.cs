//////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
//				  Daniel Olivares - arcangeldoc@gmail.com (Diseño de Pantallas Glade)
// 				  
// Licencia		: GLP
// S.O. 		: 
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
// Programa	 			: hscmty.cs
//
// Programa de Clase	: registro_admision.cs
//	  Nombre de Clase	: registro_paciente_busca					
//	
// Proposito			: Registro, admsion de pacintes y busqueda de paciente 
//////////////////////////////////////////////////////////
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using System.Collections;

namespace osiris
{	
	public class registro_paciente_busca
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion = null;
		[Widget] Gtk.Button button_selecciona = null;
		
		/////// Ventana Busqueda de paciente\\\\\\\\
		[Widget] Gtk.Window busca_paciente = null;
		[Widget] Gtk.TreeView lista_de_Pacientes = null;
		[Widget] Gtk.Button button_buscar_busqueda = null;
		[Widget] Gtk.Button button_nuevo_paciente = null;
		[Widget] Gtk.RadioButton radiobutton_busca_apellido = null;
		[Widget] Gtk.RadioButton radiobutton_busca_nombre = null;
		[Widget] Gtk.RadioButton radiobutton_busca_expediente = null;
						
		private TreeStore treeViewEngineBusca;
		
		//// Ventana Busqueda de Empresas \\\\\\
		[Widget] Gtk.Window busca_empresas = null;
		[Widget] Gtk.TreeView lista_empresas = null;
		[Widget] Gtk.Button button_busca_empresas = null;
		
		private ListStore treeViewEngineBuscaEmpresa;
		
		////// Ventana de Registro y Admision \\\\\\\\
		[Widget] Gtk.Window registro = null;
		
		// Widget de Combos
		[Widget] Gtk.ComboBox combobox_estado_civil = null;
		[Widget] Gtk.ComboBox combobox_tipo_paciente = null;
		[Widget] Gtk.ComboBox combobox_aseguradora = null;
		[Widget] Gtk.ComboBox combobox_municipios = null;
		[Widget] Gtk.ComboBox combobox_estado = null;
		[Widget] Gtk.ComboBox combobox_tipo_busqueda = null;
						
		// Widget butons
		[Widget] Gtk.Button button_buscar_paciente = null;
				
		[Widget] Gtk.Button button_grabar = null;
		[Widget] Gtk.CheckButton checkbutton_modificar = null;
		//[Widget] Gtk.Button button_eliminar;
		//[Widget] Gtk.Button button_limpiar;
		//[Widget] Gtk.Button button_tarjeta_descuento;
		[Widget] Gtk.Button button_responsable = null;
		[Widget] Gtk.Button button_imprimir_protocolo = null;
		[Widget] Gtk.Button button_cancelar_pid = null;
				
		[Widget] Gtk.Button button_admision = null;
		[Widget] Gtk.Button button_contrata_paquete = null;
		//[Widget] Gtk.Button button_imprimir;
								
		// Widget Entry, radio y check
		[Widget] Gtk.Entry entry_nombre_1 = null;
		[Widget] Gtk.Entry entry_nombre_2 = null;
		[Widget] Gtk.Entry entry_apellido_paterno = null;
		[Widget] Gtk.Entry entry_apellido_materno = null;
		[Widget] Gtk.Entry entry_dia_nacimiento = null;
		[Widget] Gtk.Entry entry_mes_nacimiento = null;
		[Widget] Gtk.Entry entry_ano_nacimiento = null;
		[Widget] Gtk.Entry entry_rfc = null;
		[Widget] Gtk.Entry entry_curp = null;
		
		[Widget] Gtk.Entry entry_ocupacion = null;		
		[Widget] Gtk.Entry entry_empresa = null;
		[Widget] Gtk.Button button_lista_empresas = null;
		
		[Widget] Gtk.RadioButton radiobutton_masculino = null;
		[Widget] Gtk.RadioButton radiobutton_femenino = null;
		
		[Widget] Gtk.Entry entry_email = null;
		
		// Direccion de Paciente
		[Widget] Gtk.Entry entry_calle = null;
		[Widget] Gtk.Entry entry_numero = null;
		[Widget] Gtk.Entry entry_colonia = null;
		[Widget] Gtk.Entry entry_CP = null;
		[Widget] Gtk.Entry entry_telcasa = null;
		[Widget] Gtk.Entry entry_teloficina = null;
		[Widget] Gtk.Entry entry_telcelular = null;
		[Widget] Gtk.Entry entry_religion_paciente = null;
		[Widget] Gtk.Entry entry_alergia_paciente = null;
		[Widget] Gtk.Entry entry_observacion_ingreso = null;
		[Widget] Gtk.Entry entry_lugar_nacimiento = null;
		
		[Widget] Gtk.CheckButton checkbutton_consulta = null;
		[Widget] Gtk.Entry entry_medico_cm = null;
		[Widget] Gtk.Button button_busca_medicos = null;
		[Widget] Gtk.Entry entry_esp_med_cm = null; 
		[Widget] Gtk.CheckButton checkbutton_laboratorio = null;
		[Widget] Gtk.CheckButton checkbutton_imagenologia = null;
		[Widget] Gtk.CheckButton checkbutton_rehabilitacion = null;
		[Widget] Gtk.CheckButton checkbutton_checkup = null;
		[Widget] Gtk.CheckButton checkbutton_otros_servicios = null;
		[Widget] Gtk.Entry entry_observacion_otros_serv = null;
				
		[Widget] Gtk.TreeView treeview_servicios = null;
		
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_folio_paciente = null;
		[Widget] Gtk.Entry entry_folio_interno_dep = null;
		
		[Widget] Gtk.Statusbar statusbar_registro = null;
				
		[Widget] Gtk.Button button_separa_folio = null;
		// Ventana de Internar al paciente en urgencias, hospital, etc.
		[Widget] Gtk.Window admision = null;
		[Widget] Gtk.Button button_graba_admision = null;
		[Widget] Gtk.Entry entry_pid_admision = null;
		[Widget] Gtk.Entry entry_paciente_admision = null;
		[Widget] Gtk.Entry entry_descrip_cirugia = null;
		//[Widget] Gtk.Button button_busca_cirugia;
		[Widget] Gtk.Entry entry_diag_admision = null;
		[Widget] Gtk.Entry entry_id_medico = null;
		[Widget] Gtk.Entry entry_nombre_medico = null;
		[Widget] Gtk.Entry entry_especialidad_medico = null;
		[Widget] Gtk.Entry entry_tel_medico = null;
		[Widget] Gtk.Entry entry_cedula_medico = null;
		[Widget] Gtk.Button button_busca_medico = null;
		[Widget] Gtk.ComboBox combobox_tipo_admision = null;
		[Widget] Gtk.ComboBox combobox_tipo_cirugia = null;
				
		// Ventana Busqueda de cirugias
		[Widget] Gtk.Window busca_cirugias = null;
		[Widget] Gtk.TreeView lista_cirugia = null;
		[Widget] Gtk.Button button_llena_cirugias = null;
		
		// Ventana Busqueda de Medicos
		[Widget] Gtk.Window buscador_medicos = null;
		//[Widget] Gtk.TreeView lista_medicos;
		//[Widget] Gtk.Button button_llena_medicos;
		[Widget] Gtk.TreeView lista_de_medicos = null;
		private TreeStore treeViewEngineMedicos;
		
		// Ventana de datos del responsable de la cuenta
		[Widget] Gtk.Window datos_del_responsable = null;
		[Widget] Gtk.Button button_graba_responsable = null;
		[Widget] Gtk.Button button_paciente_responsable = null;
		[Widget] Gtk.Button button_misma_direccion = null;
		[Widget] Gtk.Button button_asignacion_habitacion = null;
		
		[Widget] Gtk.Entry entry_nombre_responsable = null;
		[Widget] Gtk.Entry entry_telefono_responsable = null;
		[Widget] Gtk.Entry entry_direcc_responsable = null;
		[Widget] Gtk.Entry entry_empresa_responsable = null;
		[Widget] Gtk.Entry entry_aseguradora_responsable = null;
		[Widget] Gtk.Entry entry_ocupacion_responsable = null;
		[Widget] Gtk.Entry entry_dire_emp_responsable = null;
		[Widget] Gtk.Entry entry_poliza_responsable = null;
		[Widget] Gtk.Entry entry_certificado_acta = null;
		[Widget] Gtk.Entry entry_tel_emp_responsable = null;
		
		[Widget] Gtk.ComboBox combobox_parent_responsable = null;
		
		
		// Cambio para VENEZUELA
		[Widget] Gtk.Label label37 = null;
		[Widget] Gtk.Label label43 = null;		
		
		// Declaracion de variables publicas
		int PidPaciente = 0;		 // Toma la actualizacion del pid del paciente
		string nomnaciente;		 // Toma el valor del nombre completo del paciente
		string fechanacpaciente;  // Toma la fecha de nacimiento
		string edadpaciente;		 // Toma la edad del paciente
		string fecharegadmision;  // Toma el valor de la fecha de admision
		string horaregadmision;   // Toma el valor de la hora de admision
		string tipopaciente = ""; // toma el valor del texto de tipo de paciente
		int id_tipopaciente = 0;  // toma el valor del tipo de paciente Privado = 200
		int folioservicio = 0;	 // Toma el valor de numero de atencion de paciente
		
		string tipointernamiento = "URGENCIAS";//"Urgencias";  // Toma el valor del tipo de internamiento
		int idtipointernamiento = 0;       // Toma el valor del id de internamiento
		bool grabainternamiento = false; // me indica que debe grabar el internamiento del paciente
		bool grabarespocuenta = false;  // me indica si grabo los datos del responsable de la cuenta
		bool almacen_encabezado = true; // banadera de almacenamiento de cobro
		
		string estadocivil = "Casado";   // toma el valor del texto del estahdo civil
		string sexopaciente = "H"; 		// toma el valor del sexo del paciente
		string municipios = " ";	// toma el valor del municipio del paciente (direccion de su casa)
		string estado = " ";	// Toma el valor del estadoo de donde vive el paciente
		int idestado = 1;
		string nombre_aseguradora ="";
		int idaseguradora = 1;
		int idempresa_paciente = 1;
		string descripcion_empresa_paciente ="";
		bool boolaseguradora = false;
		int idcirugia = 1;   // Toma el valor id de la cirugia
		string decirugia = ""; // toma la descripcion de la
		bool editar = false;
		string busqueda = "";
		string tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";
		bool ventana_principal = true;
		
		// Variables publicas medico quien interna
		int idmedico = 1;
 		string nombmedico = "";
 		string especialidadmed = "";
 		string telmedico = "";
 		string cedmedico = "";
 		string diagnostico="";
 			
		// Variables publicas del responsable de cuenta
		string nombr_respo = "";
		string telef_respo = "";
		string direc_respo = "";
		string empre_respo = "";
		string ocupa_respo = "";
		string asegu_respo = "";
		string poliz_respo = "";
		string certif_respo = "";
		string direc_empre_respo = "";
		string telef_empre_respo = "";
		string parentezcoresponsable = "Sin Parentesco";
		
		// Variables publicas para grabar el encargado de la cuenta
		string _tipo_="";   // que tipo de entrada es nuevo o esta buscando
		string LoginEmpleado;
		string NomEmp_;
		string NomEmpleado = "";
		string AppEmpleado = "";
		string ApmEmpleado = "";		
		string connectionString;
		string nombrebd;        
		
		protected Gtk.Window MyWin;
		protected Gtk.Window MyWinError;
		
		private TreeStore treeViewEngine;
		private ListStore store_aseguradora;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public registro_paciente_busca(string _tipo, string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_,string pidpaciente_) 
		{
			LoginEmpleado = LoginEmp;
			NomEmp_ = NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			_tipo_ = _tipo;
			ventana_principal = true;
			if(_tipo == "selecciona"){
				llena_datos_del_paciente("seleccion_no_admision",pidpaciente_);
			}else{
				busca_pacientes();
			}
		}
		
		void busca_pacientes()
		{
			//Console.WriteLine(ventana_principal.ToString());
			busqueda ="paciente";
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "busca_paciente", null);
			gxml.Autoconnect (this);
        	busca_paciente.Show();
           	
			crea_treeview_busqueda();
			button_nuevo_paciente.Sensitive = false;
           	button_nuevo_paciente.Clicked += new EventHandler(on_button_nuevo_paciente_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
			button_buscar_busqueda.Clicked += new EventHandler(on_buscar_busqueda_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_paciente_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
		}
		
		void on_button_cancelar_pid_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro que desea Cancelar este PID ?");

			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
 
			if (miResultado == ResponseType.Yes){			
				NpgsqlConnection conexion = new NpgsqlConnection(connectionString+nombrebd);
				try{
					conexion.Open();
					NpgsqlCommand comando = conexion.CreateCommand ();
					comando.CommandText = "UPDATE osiris_his_paciente SET "+
										"activo = 'false' WHERE pid_paciente = '"+this.PidPaciente.ToString()+"' ;";
					Console.WriteLine(comando.CommandText);
					comando.ExecuteNonQuery();	        comando.Dispose();
					conexion.Close();
				}catch(NpgsqlException ex){
					Console.WriteLine("PostgresSQL error: {0}",ex.Message);
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();		msgBoxError.Destroy();
				}
			}
		}
		
		void on_button_asignacion_habitacion_clicked(object sender, EventArgs args)
		{
		   new osiris.asignacion_de_habitacion(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,0);
		}
		
		void on_button_nuevo_paciente_clicked(object sender, EventArgs args)
		{
			busca_paciente.Destroy();
			nuevo_paciente();
		}
				
		void nuevo_paciente()
		{
			_tipo_ = "nuevo";
			PidPaciente = 0;
			//registro_paciente_busca("nuevo",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
			
			//if (_tipo_ == "nuevo")//void on_button_nuevo_paciente_clicked(object sender, EventArgs args)//
			//{
				//Widget win = (Widget) sender;
				//win.Toplevel.Destroy();
				
				Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "registro", null);
	        	gxml.Autoconnect (this);
	        
		        	// Muestra ventana de Glade
				registro.Show();
					        	
				llena_Ventana_de_datos(PidPaciente.ToString().Trim());
	        	
	        		// activa boton de grabacion de informacion
				button_grabar.Clicked += new EventHandler(on_graba_informacion_clicked);
	        	
				// Activa boton de responsable
				button_responsable.Clicked += new EventHandler(on_button_responsable_clicked);
	        	
				//Activa boton para imprimir el protocolo
				button_imprimir_protocolo.Clicked += new EventHandler(on_button_imprimir_protocolo_clicked);
				button_imprimir_protocolo.Sensitive = false;
	        	
				// desactiva Boton de Internamiento de Paciente tiene que grabar primero
				button_admision.Sensitive = false;
	        	
	        	//Desactiva campos de PID y de FOLIO para que no se escriba en ellos
	        	entry_pid_paciente.Sensitive = false;
	        	entry_folio_paciente.Sensitive = false;
	        	entry_folio_interno_dep.Sensitive = false;
	        	
	        	//Lista a las empresas con convenio
	        	button_lista_empresas.Clicked += new EventHandler(on_button_lista_empresas_clicked);
	        	
				// Contratacion de paquetes
				button_contrata_paquete.Clicked += new EventHandler(on_button_contrata_paquete_clicked);
				button_contrata_paquete.Sensitive = false;
	        	
				// Centro Medico
				entry_medico_cm.Sensitive = false;
				checkbutton_consulta.Clicked += new EventHandler(on_checkbutton_consulta_clicked);
				button_busca_medicos.Sensitive = false;
				entry_esp_med_cm.Sensitive = false;
				
				// Observacion y otros servicios
				checkbutton_consulta.Clicked += new EventHandler(on_checkbutton_consulta_clicked);
	        	//entry_observacion.Sensitive = false;
	        	
	        	// lenado de los ComboBox
				llenado_estadocivil();	        	
	        	//llenado_municipios();
	        	llenado_estados();
	        	
				// Sexo Paciente
				radiobutton_masculino.Clicked += new EventHandler(on_cambioHM_clicked);
				radiobutton_femenino.Clicked += new EventHandler(on_cambioHM_clicked);
						
				// Cambio para VENEZUELA
				//label37.Text = "RIF";			// RFC
				//label43.Text = "C.I.";		// CURP
	        		
			//}
		}
		
		// Estado Civil
		void llenado_estadocivil()
		{
			combobox_estado_civil.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_estado_civil.PackStart(cell, true);
			combobox_estado_civil.AddAttribute(cell,"text",0);
	        
			ListStore store = new ListStore( typeof (string));
			combobox_estado_civil.Model = store;
	        
			store.AppendValues ("Casado(a)");
			store.AppendValues ("Soltero(a)");
			store.AppendValues ("Separado(a)");
			store.AppendValues ("Viudo(a)");
			store.AppendValues ("Union Libre");
			store.AppendValues ("Divorciado(a)");
	        
			TreeIter iter;
			if (store.GetIterFirst(out iter)){
				combobox_estado_civil.SetActiveIter (iter);
			}
			combobox_estado_civil.Changed += new EventHandler (onComboBoxChanged_estadocivil);
		}
		
		// Llenado de Municipios
		void llenado_municipios()
		{
			combobox_municipios.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_municipios.PackStart(cell3, true);
			combobox_municipios.AddAttribute(cell3,"text",0);
	        
			ListStore store3 = new ListStore( typeof (string),typeof (int));
			combobox_municipios.Model = store3;
	        
	        NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_municipios WHERE id_estado = '"+idestado.ToString()+"' "+
               						"ORDER BY descripcion_municipio;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read())
				{
					store3.AppendValues ((string) lector["descripcion_municipio"],(int) lector["id_municipio"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
	        
			TreeIter iter3;
			if (store3.GetIterFirst(out iter3))	{ combobox_municipios.SetActiveIter (iter3); }
			
			combobox_municipios.Changed += new EventHandler (onComboBoxChanged_municipios);
		}
		
		// Llenado de Estados
		void llenado_estados()
		{
			combobox_estado.Clear();
			CellRendererText cell4 = new CellRendererText();
			combobox_estado.PackStart(cell4, true);
			combobox_estado.AddAttribute(cell4,"text",0);
	        
	        ListStore store4 = new ListStore( typeof (string),typeof (int));
			combobox_estado.Model = store4;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_estados ORDER BY descripcion_estado;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read())
				{
					store4.AppendValues ((string) lector["descripcion_estado"], (int) lector["id_estado"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
			        	        
			TreeIter iter4;
			if (store4.GetIterFirst(out iter4))	{	combobox_estado.SetActiveIter (iter4);	}
			combobox_estado.Changed += new EventHandler (onComboBoxChanged_estado);
		}
		
		void llena_cmbox_aseguradora()
		{
			//store_aseguradora.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				comando.CommandText = "SELECT * FROM osiris_aseguradoras WHERE activa = 'true' ORDER BY descripcion_aseguradora;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	
				while (lector.Read()){
					store_aseguradora.AppendValues ((string) lector["descripcion_aseguradora"],(int) lector["id_aseguradora"]);
				}
				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_checkbutton_consulta_clicked (object sender, EventArgs a)
		{
			if (checkbutton_consulta.Active == true){
				checkbutton_laboratorio.Sensitive = false;
				checkbutton_imagenologia.Sensitive = false;
				checkbutton_rehabilitacion.Sensitive = false;
				checkbutton_checkup.Sensitive = false;
				checkbutton_otros_servicios.Sensitive = false;
				checkbutton_consulta.Sensitive = true;
				entry_medico_cm.Sensitive = true;
				button_busca_medicos.Sensitive = true;
				entry_esp_med_cm.Sensitive = true;
				if (_tipo_=="busca1"){
					button_admision.Sensitive = false;
				}
			}else{
				if (_tipo_=="busca1"){
					button_admision.Sensitive = true;
				}
				checkbutton_laboratorio.Sensitive = true;
				checkbutton_imagenologia.Sensitive = true;
				checkbutton_rehabilitacion.Sensitive = true;
				checkbutton_checkup.Sensitive = true;
				checkbutton_otros_servicios.Sensitive = true;
				entry_medico_cm.Sensitive = false;
				button_busca_medicos.Sensitive = false;
				entry_esp_med_cm.Sensitive = false;
			}
		}
		
		// Funcion para grabar informacion del paciente, cuando es nuevo
		// Paciente
		void on_graba_informacion_clicked (object sender, EventArgs a)
		{
			// Validando Informacion vacia, minimo Nombre1-App-Apm-Fecha Nacimiento
			bool validainfocaptura = (bool) verifica_datos();
			if(validainfocaptura == false){	
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
											ButtonsType.Close, "Complete Informacion de Nombres, Apellidos, Fecha y RFC \n"+
											", Direccion del Paciente o Responsable de Cuenta o \n"+
											"TIPO DE PACIENTE, Nombre o Codigo de Empresa, Aseguradora ");
				msgBoxError.Run ();			msgBoxError.Destroy();
			
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");

				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
 
				if (miResultado == ResponseType.Yes){					
					if (_tipo_ == "nuevo"){						
						bool verifica_pac = (bool) verifica_paciente();          		        		        		
						if (verifica_pac == true){
							// Alamacena los datos del paciente cuando es nuevo
							// Ademas valida los datos
								
							if ((bool) grabar_informacion()){
	        					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Info, 
												ButtonsType.Close, "Nombre de Paciente : "+entry_nombre_1.Text.Trim()+" "+entry_apellido_paterno.Text.Trim()+"\n"+	
												entry_apellido_materno.Text.Trim()+"\n Pid del Paciente : "+PidPaciente.ToString());
								msgBoxError.Run ();
								msgBoxError.Destroy();
								
								activa_los_entry(false);
	               				combobox_tipo_paciente.Sensitive = false;
	               				button_admision.Sensitive = true;  // Activando Boton de Internamiento de Paciente
	               				entry_pid_paciente.Text = PidPaciente.ToString();
								_tipo_ = "busca1";
								//Console.WriteLine(_tipo_);
									
							}else{
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info, 
									ButtonsType.Close, "Verifique la fecha de nacimiento o la informacion");
								msgBoxError.Run ();								msgBoxError.Destroy();
							}	
						}
					}
										
					// Almaceno el encabezado de cobro
					bool almaceno_encabezado = false;
					
					if (_tipo_ == "busca1"){						
						if (almacen_encabezado){
							if (checkbutton_laboratorio.Active == true ||
								checkbutton_imagenologia.Active == true ||
								checkbutton_rehabilitacion.Active == true ||
								checkbutton_checkup.Active == true ||
								checkbutton_consulta.Active == true ||
								checkbutton_otros_servicios.Active == true ||
								grabainternamiento ==  true){

								// asignando folio de servicio
								this.folioservicio = ultimo_numero_atencion();
								this.button_imprimir_protocolo.Sensitive = true;
								
								almaceno_encabezado = almacena_encabezado_de_cobro();
							}
						}
						
						if (almaceno_encabezado == true){
							// Almacenando en mov_cargos
							if (checkbutton_consulta.Active == true){
								graba_admision("CON-",16);
								checkbutton_consulta.Active = false;
								checkbutton_consulta.Sensitive = false;
								entry_folio_paciente.Text = folioservicio.ToString();}
							if (checkbutton_laboratorio.Active == true && checkbutton_consulta.Active == false){
								graba_admision("LAB-",400);
								checkbutton_laboratorio.Active = false;
								checkbutton_laboratorio.Sensitive = false;
								entry_folio_paciente.Text = folioservicio.ToString();}
							if (checkbutton_imagenologia.Active == true && checkbutton_consulta.Active == false){
								graba_admision("IMG-",300);
								checkbutton_imagenologia.Active = false;
								checkbutton_imagenologia.Sensitive = false;
								entry_folio_paciente.Text = folioservicio.ToString();}
							if (checkbutton_rehabilitacion.Active == true && checkbutton_consulta.Active == false){
								graba_admision("REH-",200);
								checkbutton_rehabilitacion.Active = false;
								checkbutton_rehabilitacion.Sensitive = false;
								entry_folio_paciente.Text = folioservicio.ToString();}
							if (checkbutton_checkup.Active == true && checkbutton_consulta.Active == false){
								graba_admision("CHE-",200);
								checkbutton_checkup.Active = false;
								checkbutton_checkup.Sensitive = false;
								entry_folio_paciente.Text = folioservicio.ToString();}
							if (checkbutton_otros_servicios.Active == true && checkbutton_consulta.Active == false){
								graba_admision("OTS-",920);
								checkbutton_otros_servicios.Active = false;
								checkbutton_otros_servicios.Sensitive = false;
								entry_folio_paciente.Text = folioservicio.ToString();}
							// Almacena los datos que esta en intermanieto
							if (grabainternamiento ==  true && checkbutton_consulta.Active == false){
								if ((bool)button_admision.Sensitive == true){
									button_imprimir_protocolo.Sensitive = true;
									//button_cons_informado.Sensitive = true;
									//button_contrato_prest.Sensitive = true;
									if (idtipointernamiento == 100){//Urgencias
										this.button_admision.Sensitive = false;
										this.button_grabar.Sensitive = false;
										grabainternamiento =  false;
										graba_admision("URG-",100);
			        					entry_folio_paciente.Text = folioservicio.ToString();}
					      			if (idtipointernamiento == 500){//Hospital
					      				this.button_admision.Sensitive = false;
					      				this.button_grabar.Sensitive = false;
					      				grabainternamiento =  false;
										graba_admision("HOS-",500);
										entry_folio_paciente.Text = folioservicio.ToString();}
					        		if (idtipointernamiento == 600){//Ginecologia-Tococirugia
										this.button_admision.Sensitive = false;
										this.button_grabar.Sensitive = false;
										grabainternamiento =  false;
										graba_admision("GINE-",600);
			        					entry_folio_paciente.Text = folioservicio.ToString();}
					        		if (idtipointernamiento == 700){//Quirofano
										this.button_admision.Sensitive = false;
										this.button_grabar.Sensitive = false;
										grabainternamiento =  false;
										graba_admision("QX-",700);
										entry_folio_paciente.Text = folioservicio.ToString();}
									if (idtipointernamiento == 810){//Terapia Adulto
										this.button_admision.Sensitive = false;
										this.button_grabar.Sensitive = false;
										grabainternamiento =  false;
										graba_admision("TAD-",810);
										entry_folio_paciente.Text = folioservicio.ToString();}
									if (idtipointernamiento == 820){//Terapia Pedriatrica
										this.button_admision.Sensitive = false;
										this.button_grabar.Sensitive = false;
										grabainternamiento =  false;
										graba_admision("TPE-",820);
										entry_folio_paciente.Text = folioservicio.ToString();}
				        			if (idtipointernamiento == 830){//Terapia Neonatal
										this.button_admision.Sensitive = false;
										this.button_grabar.Sensitive = false;
										grabainternamiento =  false;
										graba_admision("TNE-",830);
										entry_folio_paciente.Text = folioservicio.ToString();}
				        			if (idtipointernamiento == 710){//Endoscopia
										this.button_admision.Sensitive = false;
										this.button_grabar.Sensitive = false;
										graba_admision("END-",710);
										entry_folio_paciente.Text = folioservicio.ToString();}
									if (idtipointernamiento == 930){//dontologia
										this.button_admision.Sensitive = false;
										this.button_grabar.Sensitive = false;
										grabainternamiento =  false;
										graba_admision("ODO-",930);
										entry_folio_paciente.Text = folioservicio.ToString();}
									if (idtipointernamiento == 940){//OFTALMOLOGIA
										this.button_admision.Sensitive = false;
										this.button_grabar.Sensitive = false;
										grabainternamiento =  false;
										graba_admision("OFT-",940);
										entry_folio_paciente.Text = folioservicio.ToString();}
									if (idtipointernamiento == 950){// CONSULTA MEDICA
										this.button_admision.Sensitive = false;
										this.button_grabar.Sensitive = false;
										grabainternamiento =  false;
										graba_admision("CON-",950);
										entry_folio_paciente.Text = folioservicio.ToString();}
				        		}
							}//if de checkeo de internamiento
							llena_servicios_realizados(PidPaciente.ToString().Trim());
							button_asignacion_habitacion.Clicked += new EventHandler(on_button_asignacion_habitacion_clicked);
							
						}else{
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info,ButtonsType.Close, 
									"No se almacenan registros de cobranza ya que no eligio ningun tipo de ADMISION,"+
									" solo se guardo la informacion del PACIENTE, NO se creo folio de Atencion");
							msgBoxError.Run ();				msgBoxError.Destroy();
						}
					}
				}
			}
		}
		
		public int ultimo_numero_atencion()
		{
			int folioservicio_ = 0;
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();

				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT folio_de_servicio FROM osiris_erp_movcargos "+
									"ORDER BY folio_de_servicio DESC LIMIT 1;";
                              	
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if ((bool) lector.Read())
				{
					folioservicio_ = (int) lector["folio_de_servicio"] + 1;
					lector.Close ();
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();
			return folioservicio_;
		}	
	    // Imprime protocolo de admision
		void on_button_imprimir_protocolo_clicked (object sender, EventArgs args)
		{
			new osiris.impr_doc_pacientes(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,entry_folio_paciente.Text,1);//,nombmedico);
		}
	    
		// busco un paciente pantalla de ingreso de nuevo paciente
		void on_button_buscar_paciente_clicked(object sender, EventArgs a)
		{
			ventana_principal = false;
			busca_pacientes();
		}
	    
		// cambia el estatus del sexo del paciente
		void on_cambioHM_clicked (object sender, EventArgs args)
		{
			//Console.WriteLine(radiobutton_masculino.Active.ToString());
			Gtk.RadioButton radiobutton_paciente_cita = (Gtk.RadioButton) sender;
			if(radiobutton_paciente_cita.Name.ToString() == "radiobutton_masculino"){
				if (radiobutton_masculino.Active == true){
					sexopaciente = "H";
				}else{
					sexopaciente = "M";}
			}
			if(radiobutton_paciente_cita.Name.ToString() == "radiobutton_femenino"){
				if (radiobutton_femenino.Active == true){
					sexopaciente = "M";
				}else{
					sexopaciente = "H";
				}
			}
			Console.WriteLine(sexopaciente);
		}
			    		
		// Admite a paciente Urgencias y Hospital
		// Internamiento de paciente
		void on_button_admision_clicked (object sender, EventArgs args)
		{
	    	Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "admision", null);
			gxml.Autoconnect (this);
	        	                	
			// Muestra ventana de Glade
			admision.Show();
	        
	        
			entry_pid_admision.Text = PidPaciente.ToString();
			entry_paciente_admision.Text = entry_nombre_1.Text.ToString()+" "+
							entry_nombre_2.Text.ToString()+" "+
							entry_apellido_paterno.Text.ToString()+" "+
							entry_apellido_materno.Text.ToString();
        	
			entry_pid_admision.Sensitive = false;
			entry_paciente_admision.Sensitive = false;
			//entry_descrip_cirugia.Sensitive = false;
			entry_id_medico.Sensitive = false;
			//entry_descrip_cirugia.Text = decirugia;
			entry_diag_admision.Text = diagnostico;
	        decirugia = "";
			// Activa busqueda de cirugia
			//button_busca_cirugia.Clicked += new EventHandler(on_button_busca_cirugia_clicked);
			// Activa la salida de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			//Activa grabacion de internamiento  
			button_graba_admision.Clicked += new EventHandler(on_graba_admision_clicked);
			// Activa el boton de busqueda de medicos
			button_busca_medico.Clicked += new EventHandler(on_button_busca_medicos_clicked);
			
			// Llenado de combobox con los tipos de Admisiones y centros de costos
			combobox_tipo_admision.Clear();
			CellRendererText cell2 = new CellRendererText();
			combobox_tipo_admision.PackStart(cell2, true);
			combobox_tipo_admision.AddAttribute(cell2,"text",0);
	        
			ListStore store2 = new ListStore( typeof (string), typeof (int));
			combobox_tipo_admision.Model = store2;
	        
	      	NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
            try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_his_tipo_admisiones WHERE servicio_directo = 'false' "+
	           							"AND cuenta_mayor = '4000' "+
	           							//"AND cuenta_mayor_ingreso = '4000' "+
	           							//"AND grupo = 'MED' "+
	               						"ORDER BY id_tipo_admisiones;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				store2.AppendValues ("", 0);
               	while (lector.Read())
				{
					store2.AppendValues ((string) lector["descripcion_admisiones"], (int) lector["id_tipo_admisiones"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();	        
			
			TreeIter iter2;
			if (store2.GetIterFirst(out iter2)) {
				//Console.WriteLine(iter2);
				combobox_tipo_admision.SetActiveIter (iter2);
			}
			combobox_tipo_admision.Changed += new EventHandler (onComboBoxChanged_tipo_admision);
			
			// Llenado de combobox con los tipos de cirugia
			this.combobox_tipo_cirugia.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_tipo_cirugia.PackStart(cell3, true);
			combobox_tipo_cirugia.AddAttribute(cell3,"text",0);
	        
			ListStore store3 = new ListStore( typeof (string), typeof (int));
			combobox_tipo_cirugia.Model = store3;
			store3.AppendValues ("", 0);
			store3.AppendValues ("CIRUGIA AMBULATORIA", 0);
			store3.AppendValues ("CIRUGIA PROGRAMADA", 0);
			store3.AppendValues ("SIN CIRUGIA", 0);
			
			TreeIter iter;
			if (store3.GetIterFirst(out iter)){
				combobox_tipo_cirugia.SetActiveIter (iter);
			}
			
			combobox_tipo_cirugia.Changed += new EventHandler (onComboBoxChanged_combobox_tipo_cirugia);
		}
		
		void onComboBoxChanged_combobox_tipo_cirugia(object sender, EventArgs args)
		{
	    	ComboBox combobox_tipo_cirugia = sender as ComboBox;
			if (sender == null)	{	return;	}
			TreeIter iter;			
			int numbusqueda = 0;
			if (combobox_tipo_cirugia.GetActiveIter (out iter)){
				decirugia = (string) combobox_tipo_cirugia.Model.GetValue(iter,0);
			}
		}
		
		
		// Ventana de Busqueda de Medico
		void on_button_busca_medicos_clicked (object sender, EventArgs args)
		{
			busqueda = "medicos";
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador_medicos", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
			buscador_medicos.Show();
	        llenado_cmbox_tipo_busqueda();
	        entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			button_buscar_busqueda.Clicked += new EventHandler(on_button_llena_medicos_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_medico_clicked);
	        button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
			
			treeViewEngineMedicos = new TreeStore(typeof(int),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(bool),typeof(bool),typeof(bool),typeof(bool),
												typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),
												typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool));
			lista_de_medicos.Model = treeViewEngineMedicos;
			lista_de_medicos.RulesHint = true;
		
			lista_de_medicos.RowActivated += on_selecciona_medico_clicked;  // Doble click selecciono paciente
			
			TreeViewColumn col_idmedico = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idmedico.Title = "ID Medico"; // titulo de la cabecera de la columna, si está visible
			col_idmedico.PackStart(cellr0, true);
			col_idmedico.AddAttribute (cellr0, "text", 0);
			col_idmedico.SortColumnId = (int) Coldatos_medicos.col_idmedico;    
            
			TreeViewColumn col_nomb1medico = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_nomb1medico.Title = "1º Nombre";
			col_nomb1medico.PackStart(cellrt1, true);
			col_nomb1medico.AddAttribute (cellrt1, "text", 1);
			col_nomb1medico.SortColumnId = (int) Coldatos_medicos.col_nomb1medico; 
            
            TreeViewColumn col_nomb2medico = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_nomb2medico.Title = "2º Nombre";
			col_nomb2medico.PackStart(cellrt2, true);
			col_nomb2medico.AddAttribute (cellrt2, "text", 2);
			col_nomb2medico.SortColumnId = (int) Coldatos_medicos.col_nomb2medico; 
			
			TreeViewColumn col_appmedico = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_appmedico.Title = "Apellido Paterno";
			col_appmedico.PackStart(cellrt3, true);
			col_appmedico.AddAttribute (cellrt3, "text", 3);
			col_appmedico.SortColumnId = (int) Coldatos_medicos.col_appmedico;
			
			TreeViewColumn col_apmmedico = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_apmmedico.Title = "Apellido Materno";
			col_apmmedico.PackStart(cellrt4, true);
			col_apmmedico.AddAttribute (cellrt4, "text", 4);
			col_apmmedico.SortColumnId = (int) Coldatos_medicos.col_apmmedico;
            
			TreeViewColumn col_espemedico = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_espemedico.Title = "Especialidad";
			col_espemedico.PackStart(cellrt5, true);
			col_espemedico.AddAttribute (cellrt5, "text", 5);
			col_espemedico.SortColumnId = (int) Coldatos_medicos.col_espemedico;
            
			TreeViewColumn col_telmedico = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_telmedico.Title = "Cedula Medica";
			col_telmedico.PackStart(cellrt6, true);
			col_telmedico.AddAttribute (cellrt6, "text", 6); 
			col_telmedico.SortColumnId = (int) Coldatos_medicos.col_telmedico;
            
			TreeViewColumn col_cedulamedico = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_cedulamedico.Title = "Telefono Casa";
			col_cedulamedico.PackStart(cellrt7, true);
			col_cedulamedico.AddAttribute (cellrt7, "text", 7); 
			col_cedulamedico.SortColumnId = (int) Coldatos_medicos.col_cedulamedico;
			
			TreeViewColumn col_telOfmedico = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_telOfmedico.Title = "Telefono Oficina";
			col_telOfmedico.PackStart(cellrt8, true);
			col_telOfmedico.AddAttribute (cellrt8, "text", 8);
			col_telOfmedico.SortColumnId = (int) Coldatos_medicos.col_telOfmedico; 
			
			TreeViewColumn col_celmedico = new TreeViewColumn();
			CellRendererText cellrt9 = new CellRendererText();
			col_celmedico.Title = "Celular 1";
			col_celmedico.PackStart(cellrt9, true);
			col_celmedico.AddAttribute (cellrt9, "text", 9); 
			col_celmedico.SortColumnId = (int) Coldatos_medicos.col_celmedico;
			
			TreeViewColumn col_celmedico2 = new TreeViewColumn();
			CellRendererText cellrt10 = new CellRendererText();
			col_celmedico2.Title = "Celular 2";
			col_celmedico2.PackStart(cellrt10, true);
			col_celmedico2.AddAttribute (cellrt10, "text", 10);
			col_celmedico2.SortColumnId = (int) Coldatos_medicos.col_celmedico2;
									
			TreeViewColumn col_nextelmedico = new TreeViewColumn();
			CellRendererText cellrt11 = new CellRendererText();
			col_nextelmedico.Title = "Nextel";
			col_nextelmedico.PackStart(cellrt11, true);
			col_nextelmedico.AddAttribute (cellrt11, "text", 11);
			col_nextelmedico.SortColumnId = (int) Coldatos_medicos.col_nextelmedico;
			
			TreeViewColumn col_beepermedico = new TreeViewColumn();
			CellRendererText cellrt12 = new CellRendererText();
			col_beepermedico.Title = "Beeper";
			col_beepermedico.PackStart(cellrt12, true);
			col_beepermedico.AddAttribute (cellrt12, "text", 12);
			col_beepermedico.SortColumnId = (int) Coldatos_medicos.col_beepermedico;
			
			TreeViewColumn col_empresamedico = new TreeViewColumn();
			CellRendererText cellrt13 = new CellRendererText();
			col_empresamedico.Title = "Empresa";
			col_empresamedico.PackStart(cellrt13, true);
			col_empresamedico.AddAttribute (cellrt13, "text", 13);
			col_empresamedico.SortColumnId = (int) Coldatos_medicos.col_empresamedico;
			                        
			lista_de_medicos.AppendColumn(col_idmedico);
			lista_de_medicos.AppendColumn(col_nomb1medico);
			lista_de_medicos.AppendColumn(col_nomb2medico);
			lista_de_medicos.AppendColumn(col_appmedico);
			lista_de_medicos.AppendColumn(col_apmmedico);
			lista_de_medicos.AppendColumn(col_espemedico);
			lista_de_medicos.AppendColumn(col_cedulamedico);
			lista_de_medicos.AppendColumn(col_telmedico);
			lista_de_medicos.AppendColumn(col_telOfmedico);
			lista_de_medicos.AppendColumn(col_celmedico);
			lista_de_medicos.AppendColumn(col_celmedico2);
			lista_de_medicos.AppendColumn(col_nextelmedico);
			lista_de_medicos.AppendColumn(col_beepermedico);
			lista_de_medicos.AppendColumn(col_empresamedico);
		}
		
		enum Coldatos_medicos
		{
			col_idmedico,
			col_nomb1medico,
			col_nomb2medico,
			col_appmedico,
			col_apmmedico,
			col_espemedico,
			col_cedulamedico,
			col_telmedico,
			col_telOfmedico,
			col_celmedico,
			col_celmedico2,
			col_nextelmedico,
			col_beepermedico,
			col_empresamedico
		}
		
		void on_button_llena_medicos_clicked (object sender, EventArgs args)
		{
			llenando_lista_de_medicos();
		}
        
        void llenando_lista_de_medicos() 
		{
			TreeIter iter;
			if (combobox_tipo_busqueda.GetActiveIter(out iter))
			{
				if((int) combobox_tipo_busqueda.Model.GetValue(iter,1) > 0) {
					treeViewEngineMedicos.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
		            // Verifica que la base de datos este conectada
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						if ((string) entry_expresion.Text.ToUpper().Trim() == "")
						{
							comando.CommandText = "SELECT id_medico, "+
										"to_char(id_empresa,'999999') AS idempresa, "+
										"to_char(osiris_his_tipo_especialidad.id_especialidad,'999999') AS idespecialidad, "+
										"nombre_medico,descripcion_empresa,descripcion_especialidad,centro_medico, "+
										"nombre1_medico,nombre2_medico,apellido_paterno_medico,apellido_materno_medico, "+
										"telefono1_medico,cedula_medico,telefono2_medico,celular1_medico,celular2_medico, "+
										"nextel_medico,beeper_medico,cedula_medico,direccion_medico,direccion_consultorio_medico, "+
										"to_char(fecha_ingreso_medico,'dd-mm-yyyy') AS fecha_ingreso, "+
										"to_char(fecha_revision_medico,'dd-mm-yyyy') AS fecha_revision, "+
										"titulo_profesional_medico,cedula_profecional_medico,diploma_especialidad_medico, "+
										"diploma_subespecialidad_medico,copia_identificacion_oficial_medico,copia_cedula_rfc_medico, "+
										"diploma_cursos_adiestramiento_medico,certificacion_recertificacion_consejo_subespecialidad_medico, "+
										"copia_comprobante_domicilio_medico,diploma_seminarios_medico,diploma_cursos_medico, "+
										"diplomas_extranjeros_medico,constancia_congresos_medico,cedula_especialidad_medico, "+
										"medico_activo,autorizado "+
										"FROM osiris_his_medicos,osiris_his_tipo_especialidad,osiris_empresas "+
										"WHERE osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad "+
										"AND osiris_his_medicos.id_empresa_medico = osiris_empresas.id_empresa "+
										"AND medico_activo = 'true' "+
										"ORDER BY id_medico;";
						}else{
							comando.CommandText = "SELECT id_medico, "+
										"to_char(id_empresa,'999999') AS idempresa, "+
										"to_char(osiris_his_tipo_especialidad.id_especialidad,'999999') AS idespecialidad, "+
										"nombre_medico,descripcion_empresa,descripcion_especialidad,centro_medico, "+
										"nombre1_medico,nombre2_medico,apellido_paterno_medico,apellido_materno_medico, "+
										"telefono1_medico,cedula_medico,telefono2_medico,celular1_medico,celular2_medico, "+
										"nextel_medico,beeper_medico,cedula_medico,direccion_medico,direccion_consultorio_medico, "+
										"to_char(fecha_ingreso_medico,'dd-mm-yyyy') AS fecha_ingreso, "+
										"to_char(fecha_revision_medico,'dd-mm-yyyy') AS fecha_revision, "+
										"titulo_profesional_medico,cedula_profecional_medico,diploma_especialidad_medico, "+
										"diploma_subespecialidad_medico,copia_identificacion_oficial_medico,copia_cedula_rfc_medico, "+
										"diploma_cursos_adiestramiento_medico,certificacion_recertificacion_consejo_subespecialidad_medico, "+
										"copia_comprobante_domicilio_medico,diploma_seminarios_medico,diploma_cursos_medico, "+
										"diplomas_extranjeros_medico,constancia_congresos_medico,cedula_especialidad_medico, "+
										"medico_activo,autorizado "+
										"FROM osiris_his_medicos,osiris_his_tipo_especialidad,osiris_empresas "+
										"WHERE osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad "+
										"AND osiris_his_medicos.id_empresa_medico = osiris_empresas.id_empresa "+
										"AND medico_activo = 'true' "+
								  		tipobusqueda+(string) entry_expresion.Text.Trim().ToUpper()+"%' "+
										"ORDER BY id_medico;";
						}
						NpgsqlDataReader lector = comando.ExecuteReader ();
						//Console.WriteLine(comando.CommandText);
						while (lector.Read())
						{
							treeViewEngineMedicos.AppendValues ((int) lector["id_medico"],//0
										(string) lector["nombre1_medico"],//1
										(string) lector["nombre2_medico"],//2
										(string) lector["apellido_paterno_medico"],//3
										(string) lector["apellido_materno_medico"],//4
										(string) lector["descripcion_especialidad"],//5
										(string) lector["cedula_medico"],//6
										(string) lector["telefono1_medico"],//7
										(string) lector["telefono2_medico"],//8
										(string) lector["celular1_medico"],//9
										(string) lector["celular2_medico"],//10
										(string) lector["nextel_medico"],//11
										(string) lector["beeper_medico"],//12
										(string) lector["descripcion_empresa"],//13
										(string) lector["idespecialidad"],//14
										(string) lector["idempresa"],//15
										(string) lector["fecha_ingreso"],//16
										(string) lector["fecha_revision"],//17
										(string) lector["direccion_medico"],//18
										(string) lector["direccion_consultorio_medico"],//19
										(bool) lector["titulo_profesional_medico"],//20
										(bool) lector["cedula_profecional_medico"],//21
										(bool) lector["diploma_especialidad_medico"], //22
										(bool) lector["diploma_subespecialidad_medico"],//23
										(bool) lector["copia_identificacion_oficial_medico"],//24
										(bool) lector["copia_cedula_rfc_medico"], //25
										(bool) lector["diploma_cursos_adiestramiento_medico"],//26
										(bool) lector["certificacion_recertificacion_consejo_subespecialidad_medico"],//27
										(bool) lector["copia_comprobante_domicilio_medico"],//28
										(bool) lector["diploma_seminarios_medico"],//29
										(bool) lector["diploma_cursos_medico"],//30
										(bool) lector["diplomas_extranjeros_medico"],//31
										(bool) lector["constancia_congresos_medico"],//32
										(bool) lector["cedula_especialidad_medico"],//33
										(bool) lector["medico_activo"],//34
										(bool) lector["centro_medico"],//35
										(bool) lector["autorizado"]//36
										);
						}
					}catch (NpgsqlException ex){
			   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();			msgBoxError.Destroy();
					}
					conexion.Close ();
				}else{	
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Info,ButtonsType.Close, " selecione un tipo de busqueda ");
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
			}
		}
		
		void llenado_cmbox_tipo_busqueda()
		{
			combobox_tipo_busqueda.Clear();
			CellRendererText cell1 = new CellRendererText();
			combobox_tipo_busqueda.PackStart(cell1, true);
			combobox_tipo_busqueda.AddAttribute(cell1,"text",0);
	        
			ListStore store1 = new ListStore( typeof (string),typeof (int));
			combobox_tipo_busqueda.Model = store1;
	        
			//store1.AppendValues ("",0);
			store1.AppendValues ("PRIMER NOMBRE",1);
			store1.AppendValues ("SEGUNDO NOMBRE",2);
			store1.AppendValues ("APELLIDO PATERNO",3);
			store1.AppendValues ("APELLIDO MATERNO",4);
			store1.AppendValues ("CEDULA MEDICA",5);
			store1.AppendValues ("ESPECIALIDAD",6);
				              
			TreeIter iter1;
			if (store1.GetIterFirst(out iter1)){
				combobox_tipo_busqueda.SetActiveIter (iter1);
			}
			combobox_tipo_busqueda.Changed += new EventHandler (onComboBoxChanged_tipo_busqueda);
		}
		
		void onComboBoxChanged_tipo_busqueda (object sender, EventArgs args)
		{
	    	ComboBox combobox_tipo_busqueda = sender as ComboBox;
			if (sender == null)	{	return;	}
			TreeIter iter;			int numbusqueda = 0;
			if (combobox_tipo_busqueda.GetActiveIter (out iter))
			{
				numbusqueda = (int) combobox_tipo_busqueda.Model.GetValue(iter,1);
				tipo_de_busqueda_de_medico(numbusqueda);
				llenando_lista_de_medicos();
			}
		}
		
		void tipo_de_busqueda_de_medico(int numbusqueda)
		{
			if(numbusqueda == 1)  { tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";	}//Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 2)  { tipobusqueda = "AND osiris_his_medicos.nombre2_medico LIKE '";	}//Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 3)  { tipobusqueda = "AND osiris_his_medicos.apellido_paterno_medico LIKE '";	}//Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 4)  { tipobusqueda = "AND osiris_his_medicos.apellido_materno_medico LIKE '";	}//Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 5)  { tipobusqueda = "AND osiris_his_medicos.cedula_medico LIKE '";	}//Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 6)  { tipobusqueda = "AND osiris_his_tipo_especialidad.descripcion_especialidad LIKE '";	}//Console.WriteLine(tipobusqueda); }
		}
        
        void on_selecciona_medico_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_medicos.Selection.GetSelected(out model, out iterSelected)) 
 			{
 				idmedico =(int) model.GetValue(iterSelected, 0);
 				nombmedico = (string) model.GetValue(iterSelected, 1)+" "+(string) model.GetValue(iterSelected, 2)+" "+
							(string) model.GetValue(iterSelected, 3)+" "+(string) model.GetValue(iterSelected,4);
				especialidadmed = (string) model.GetValue(iterSelected, 5);
				cedmedico = (string) model.GetValue(iterSelected, 6);
				if((string) model.GetValue(iterSelected, 7) != "") {telmedico = (string) model.GetValue(iterSelected, 7);}
				else{
					if((string) model.GetValue(iterSelected,8) != "") {telmedico = (string) model.GetValue(iterSelected,8);}
					else{
						if((string) model.GetValue(iterSelected,9) != "") {telmedico = (string) model.GetValue(iterSelected,9);}
						else{
							if((string) model.GetValue(iterSelected,10) != "") {telmedico = (string) model.GetValue(iterSelected,10);}
							else{
								if((string) model.GetValue(iterSelected,11) != "") {telmedico = (string) model.GetValue(iterSelected,11);}
								else{
									if((string) model.GetValue(iterSelected,12) != "") {telmedico = (string) model.GetValue(iterSelected,12);}
								}
							}
						}
					}
				}
				entry_id_medico.Text = idmedico.ToString();
				entry_nombre_medico.Text = nombmedico;
				entry_especialidad_medico.Text = especialidadmed;
				entry_tel_medico.Text = telmedico;
				entry_cedula_medico.Text = cedmedico;
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
        	    
		void on_graba_admision_clicked (object sender, EventArgs args)
		{
			if((string) entry_nombre_medico.Text == "" || entry_diag_admision.Text == "" || idtipointernamiento == 0 || decirugia == "")
			{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "Debe poner una Admision, ni de dejar el nombre de medico o el diagnostico vacios, ni la cirugia vacia");
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}else{
				if (grabainternamiento == false){
					grabainternamiento = true;
				}
				diagnostico = (string) entry_diag_admision.Text.ToUpper();
				nombmedico = (string) entry_nombre_medico.Text.ToUpper();
				
				// cierra la ventana despues que almaceno la informacion en variables
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
	    
		void onComboBoxChanged_tipo_admision (object sender, EventArgs args)
		{
			ComboBox combobox_tipo_admision = sender as ComboBox;
			if (sender == null){
				return;
			}
			TreeIter iter;
			if (combobox_tipo_admision.GetActiveIter (out iter)){
				tipointernamiento = (string) combobox_tipo_admision.Model.GetValue(iter,0);//Console.WriteLine(tipointernamiento);
				idtipointernamiento = (int) combobox_tipo_admision.Model.GetValue(iter,1);//Console.WriteLine(idtipointernamiento);
			}
		}

		void on_button_contrata_paquete_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "admision", null);
			gxml.Autoconnect (this);
	        
			entry_pid_admision.Text = PidPaciente.ToString();
			entry_paciente_admision.Text = entry_nombre_1.Text.ToString()+" "+
			entry_nombre_2.Text.ToString()+" "+
			entry_apellido_paterno.Text.ToString()+" "+
        	entry_apellido_materno.Text.ToString();
        	
			// Muestra ventana de Glade
			admision.Show();
	        
			entry_pid_admision.Sensitive = false;
			entry_paciente_admision.Sensitive = false;
			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs  			
		}
	    // Crea la lista de empresas que tienen convenio con el hospital
	    void on_button_lista_empresas_clicked(object sender, EventArgs args)
	    {
	    	Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "busca_empresas", null);
			gxml.Autoconnect (this);
	        
	        // Muestra ventana de Glade
			busca_empresas.Show();
			
			// Activa la salida de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			// Activa la seleccion de Medico
			button_selecciona.Clicked += new EventHandler(on_selecciona_empresa_clicked);
			// Llena treeview 
			button_busca_empresas.Clicked += new EventHandler(on_button_llena_empresas_clicked);
			
			treeViewEngineBuscaEmpresa = new ListStore( typeof(int), typeof(string));
			lista_empresas.Model = treeViewEngineBuscaEmpresa;
			lista_empresas.RulesHint = true;
			
			lista_empresas.RowActivated += on_selecciona_empresa_clicked;  // Doble click selecciono empresa*/
			
			TreeViewColumn col_idempresa = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idempresa.Title = "ID Empresa"; // titulo de la cabecera de la columna, si está visible
			col_idempresa.PackStart(cellr0, true);
			col_idempresa.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1
            
			TreeViewColumn col_nombrempresa = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_nombrempresa.Title = "Nombre Empresa";
			col_nombrempresa.PackStart(cellrt1, true);
			col_nombrempresa.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
			
			lista_empresas.AppendColumn(col_idempresa);
			lista_empresas.AppendColumn(col_nombrempresa);
		}
		
		void on_button_llena_empresas_clicked(object sender, EventArgs args)
		{
			treeViewEngineBuscaEmpresa.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				if ((string) entry_expresion.Text.ToUpper() == "*")				{
					comando.CommandText = "SELECT * FROM osiris_empresas "+
								"ORDER BY descripcion_empresa;";
				}else{
					comando.CommandText = "SELECT * FROM osiris_empresas "+
								"WHERE descripcion_empresa LIKE '%"+(string) entry_expresion.Text.ToUpper()+"%' "+
								"ORDER BY descripcion_empresa;";
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read())				{
					treeViewEngineBuscaEmpresa.AppendValues ((int) lector["id_empresa"],//TreeIter iter = 
											(string)lector["descripcion_empresa"]);
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
		
		void on_selecciona_empresa_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_empresas.Selection.GetSelected(out model, out iterSelected)) {
				idempresa_paciente = (int) model.GetValue(iterSelected, 0);
				empre_respo = (string) model.GetValue(iterSelected, 1);
				
				entry_empresa.Text = empre_respo.Trim();
				//entry_descrip_cirugia.Text = (string) model.GetValue(iterSelected, 1);
				// cierra la ventana despues que almaceno la informacion en variables
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
	    
		// Procedimiento para el llenado de los datos del paciente
		public void llena_inf_de_paciente(string pidpaciente_)
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				comando.CommandText = "SELECT pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,"+
							"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'dd') AS dia_nacimiento,"+
							"to_char(fecha_nacimiento_paciente,'MM') AS mes_nacimiento,"+
							"to_char(fecha_nacimiento_paciente,'yyyy') AS ano_nacimiento, sexo_paciente, ocupacion_paciente,"+
							"rfc_paciente, curp_paciente, estado_civil_paciente, direccion_paciente, numero_casa_paciente, "+
							"colonia_paciente, codigo_postal_paciente, telefono_particular1_paciente,"+
							"telefono_trabajo1_paciente, celular1_paciente, municipio_paciente, estado_paciente, "+
							"osiris_his_paciente.id_empresa AS idempresapaciente, osiris_empresas.descripcion_empresa,"+
							"religion_paciente,alegias_paciente,lugar_nacimiento_paciente "+
							"FROM osiris_his_paciente, osiris_empresas WHERE osiris_his_paciente.id_empresa=osiris_empresas.id_empresa "+
							"AND pid_paciente = '"+pidpaciente_.ToString()+"'"+
							"AND activo = 'true' "+
							" ORDER BY pid_paciente;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if ((bool) lector.Read()){
					entry_nombre_1.Text = (string) lector["nombre1_paciente"];
					entry_nombre_2.Text = (string) lector["nombre2_paciente"];
					entry_apellido_paterno.Text = (string) lector["apellido_paterno_paciente"];
					entry_apellido_materno.Text = (string) lector["apellido_materno_paciente"];
					entry_dia_nacimiento.Text = (string) lector["dia_nacimiento"];
					entry_mes_nacimiento.Text = (string) lector["mes_nacimiento"];
					entry_ano_nacimiento.Text = (string) lector["ano_nacimiento"];
					entry_rfc.Text = (string) lector["rfc_paciente"];
					entry_curp.Text = (string) lector["curp_paciente"];
					entry_ocupacion.Text = (string) lector["ocupacion_paciente"];
					descripcion_empresa_paciente = (string) lector["descripcion_empresa"];
					idempresa_paciente = (int) lector["idempresapaciente"];
					entry_empresa.Text = (string) descripcion_empresa_paciente.ToString().Trim();
					empre_respo = descripcion_empresa_paciente;
					
					sexopaciente = (string) lector["sexo_paciente"];
					if (sexopaciente == "H"){
						radiobutton_masculino.Active = true;						
					}else{
						radiobutton_femenino.Active = true;
					}
					
					entry_calle.Text = (string) lector["direccion_paciente"];
					entry_numero.Text = (string) lector["numero_casa_paciente"];
					entry_colonia.Text = (string) lector["colonia_paciente"];
					entry_CP.Text = (string) lector["codigo_postal_paciente"];
					entry_telcasa.Text = (string) lector["telefono_particular1_paciente"];
					entry_teloficina.Text = (string) lector["telefono_trabajo1_paciente"];
					entry_telcelular.Text = (string) lector["celular1_paciente"];
					entry_religion_paciente.Text = (string) lector["religion_paciente"];
					entry_alergia_paciente.Text = (string) lector["alegias_paciente"];
					entry_lugar_nacimiento.Text = (string) lector["lugar_nacimiento_paciente"];
					
					estadocivil = (string) lector["estado_civil_paciente"];
					// Estado Civil
					combobox_estado_civil.Clear();
					CellRendererText cell = new CellRendererText();
					combobox_estado_civil.PackStart(cell, true);
					combobox_estado_civil.AddAttribute(cell,"text",0);
	        
					ListStore store = new ListStore( typeof (string));
					combobox_estado_civil.Model = store;
	        
					store.AppendValues ((string) lector["estado_civil_paciente"]);
	        		
					TreeIter iter;
					if (store.GetIterFirst(out iter)){
						combobox_estado_civil.SetActiveIter (iter);
					}
					//combobox_estado_civil.Changed += new EventHandler (onComboBoxChanged_estadocivil);
	        		
					// Llenado de Municipios
					municipios = (string) lector["municipio_paciente"];
					combobox_municipios.Clear();
					CellRendererText cell3 = new CellRendererText();
					combobox_municipios.PackStart(cell3, true);
					combobox_municipios.AddAttribute(cell3,"text",0);
	        
					ListStore store3 = new ListStore( typeof (string));
					combobox_municipios.Model = store3;
	        		
					store3.AppendValues ((string) municipios);
	        		
					TreeIter iter3;
					if (store3.GetIterFirst(out iter3)){
						combobox_municipios.SetActiveIter (iter3);
					}
					//combobox_municipios.Changed += new EventHandler (onComboBoxChanged_municipios);
					
					// Llenado de Estados
					estado = (string) lector["estado_paciente"];
					combobox_estado.Clear();
					CellRendererText cell4 = new CellRendererText();
					combobox_estado.PackStart(cell4, true);
					combobox_estado.AddAttribute(cell4,"text",0);
	        
					ListStore store4 = new ListStore( typeof (string));
					combobox_estado.Model = store4;
	        		
					store4.AppendValues ((string) estado);
	        		
					TreeIter iter4;
					if (store4.GetIterFirst(out iter4)){
						combobox_estado.SetActiveIter (iter4);
					}
					//combobox_estado.Changed += new EventHandler (onComboBoxChanged_estado);
	        		
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
	    
		// Activa desactiva combobox de aseguradora
		void onComboBoxChanged_tipopaciente(object sender, EventArgs args)
		{
			ComboBox combobox_tipo_paciente = sender as ComboBox;
			if (sender == null){
				return;
			}
			TreeIter iter;
			if (combobox_tipo_paciente.GetActiveIter (out iter)){
				tipopaciente = (string) combobox_tipo_paciente.Model.GetValue(iter,0);
				id_tipopaciente = (int) combobox_tipo_paciente.Model.GetValue(iter,1);
				if (id_tipopaciente == 400)  // Aseguradora
				{
					boolaseguradora = true;
					combobox_aseguradora.Sensitive = true;
				}else{
					boolaseguradora = false;
					combobox_aseguradora.Sensitive = false;
				}
				if (id_tipopaciente == 102 || id_tipopaciente == 300 || id_tipopaciente == 500 )  // Empresa-Hscmty-Municipio
				{
					//if (_tipo_ == "nuevo"){
					//	this.entry_empresa.Sensitive = true;
					//}else{
						this.entry_empresa.Sensitive = false;
					//}
				}else{
					this.entry_empresa.Sensitive = true;
				}
			}	
		}
	    
		// Combobox de aseguradoras
		void onComboBoxChanged_aseguradora (object sender, EventArgs args)
		{
			ComboBox comboboxe_aseguradora = sender as ComboBox;
			if (sender == null){
				return;
			}
			TreeIter iter;
			if (comboboxe_aseguradora.GetActiveIter (out iter)){
				nombre_aseguradora = (string) combobox_aseguradora.Model.GetValue(iter,0);
				idaseguradora = (int) combobox_aseguradora.Model.GetValue(iter,1);	
			}
		}
	    
		// Combobox de municipios
		void onComboBoxChanged_municipios (object sender, EventArgs args)
		{
			ComboBox combobox_municipios = sender as ComboBox;
			if (sender == null) {	return; }
			TreeIter iter;
			if (combobox_municipios.GetActiveIter (out iter)){
				municipios = (string) combobox_municipios.Model.GetValue(iter,0);
				//idmunicipio = (int) combobox_municipios.Model.GetValue(iter,1);
			}
		}
	    
		// Combobox de los estados del pais
		void onComboBoxChanged_estado (object sender, EventArgs args)
		{
			ComboBox combobox_estado = sender as ComboBox;
			if (sender == null) { return;	}
			TreeIter iter;
			if (combobox_estado.GetActiveIter (out iter)){
				estado = (string) combobox_estado.Model.GetValue(iter,0); //Console.WriteLine(estado.ToString());
				idestado = (int) combobox_estado.Model.GetValue(iter,1);
				llenado_municipios();
			}
		}
	    
		// Combobox de estado civl
		void onComboBoxChanged_estadocivil (object sender, EventArgs args)
		{
			ComboBox combobox_estado_civil = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (combobox_estado_civil.GetActiveIter (out iter)){
				estadocivil = (string) combobox_estado_civil.Model.GetValue(iter,0);
			}
		}
	    
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs a)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	    
		// Valida entradas que solo sean numericas
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			Console.WriteLine(args.Event.Key.ToString());
			if (args.Event.Key == Gdk.Key.Return){
				//Console.WriteLine("Presione Enter");
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace){
				args.RetVal = true;
			}
		}
		
    	// Funcion para verificar que los datos sean llenados correctamente
		public bool verifica_datos()
		{
			if ((string) entry_nombre_1.Text.Trim() == "" || (string) entry_apellido_paterno.Text.Trim() == "" ||	
				(string) entry_apellido_materno.Text.Trim() == "" || (string) entry_dia_nacimiento.Text.Trim() == "" ||
				(string) entry_mes_nacimiento.Text.Trim() == "" || (string) entry_ano_nacimiento.Text.Trim() == "" ||
			    (string) entry_religion_paciente.Text.Trim() == "" || entry_alergia_paciente.Text.Trim() == "" ||
				(string) entry_rfc.Text.Trim() =="" || grabarespocuenta == false || id_tipopaciente == 0 || (string) this.entry_empresa.Text.ToString().Trim() == ""){
				return false;
			}else{
				if (id_tipopaciente == 400)  // Aseguradora
				{	
					if(idaseguradora == 1){
						return false;
					}else{
						return true;
					}
				}else{			
					return true;
				}
				if (id_tipopaciente == 500)	{
					if(idempresa_paciente <= 1){
						return false;
					}else{
						return true;
					}
				}
			}	
		}
    	
		// Funcion para verificar que el paciente exista
		public bool verifica_paciente()
		{
	   		NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );            
			// Verifica que la base de datos este conectada
			try{
			// Transformando al Mayuscula datos
				entry_nombre_1.Text = (string) entry_nombre_1.Text.ToUpper();
				entry_nombre_2.Text = (string) entry_nombre_2.Text.ToUpper();
				entry_apellido_paterno.Text = (string) entry_apellido_paterno.Text.ToUpper();
				entry_apellido_materno.Text = (string) entry_apellido_materno.Text.ToUpper();
				entry_rfc.Text = (string) entry_rfc.Text.ToUpper();
				entry_curp.Text = (string) entry_curp.Text.ToUpper();
        				
				string rfcpaciente =  "";            //crea_rfc();
        		
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand (); 
             
				comando.CommandText = "SELECT rfc_paciente,activo "+
							"FROM osiris_his_paciente "+ 
							"WHERE activo = true and rfc_paciente = '"+entry_rfc.Text.ToUpper()+"';";
            	
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	
				// Verificando la consulta si esta vacia
				if ((bool) lector.Read()){
					if ((bool) lector["activo"] == true){
						// Asignacion de Variables para verificar RFC
						rfcpaciente   =  (string) lector["rfc_paciente"];
						// Verificando si el paciente ya esta registrado
						if ((string) entry_rfc.Text.ToUpper() == rfcpaciente ){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
												ButtonsType.Close, "Este paciente ya existe en nuestros registros");
							msgBoxError.Run ();
							msgBoxError.Destroy();
						}
						lector.Close ();
						conexion.Close ();
						return false;
					}else{
						lector.Close ();
						conexion.Close ();
						return true;	
					}
				}else{
					conexion.Close ();
					return true;	
				}
				
	   		}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
				conexion.Close ();
				return false; 
			}
			
		}
		
		//---------------------------------------------------------
		public string crea_rfc()
		{
			
			return "RFC";
		}
		
	 	void crea_treeview_busqueda()
		{
			treeViewEngineBusca = new TreeStore(typeof(int),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(string),typeof(string));
			lista_de_Pacientes.Model = treeViewEngineBusca;
			
			lista_de_Pacientes.RulesHint = true;
			
			lista_de_Pacientes.RowActivated += on_selecciona_paciente_clicked;  // Doble click selecciono paciente*/
			
			TreeViewColumn col_PidPaciente = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_PidPaciente.Title = "PID Expediente"; // titulo de la cabecera de la columna, si está visible
			col_PidPaciente.PackStart(cellr0, true);
			col_PidPaciente.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
			col_PidPaciente.SortColumnId = (int) Column.col_PidPaciente;
			//cellr0.Editable = true;   // Permite edita este campo
            
			TreeViewColumn col_Nombre1_Paciente = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_Nombre1_Paciente.Title = "Nombre 1";
			col_Nombre1_Paciente.PackStart(cellrt1, true);
			col_Nombre1_Paciente.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 1 en vez de 2
			col_Nombre1_Paciente.SortColumnId = (int) Column.col_Nombre1_Paciente;
            
			TreeViewColumn col_Nombre2_Paciente = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_Nombre2_Paciente.Title = "Nombre 2";
			col_Nombre2_Paciente.PackStart(cellrt2, true);
			col_Nombre2_Paciente.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 2 en vez de 3
			col_Nombre2_Paciente.SortColumnId = (int) Column.col_Nombre2_Paciente;
            
			TreeViewColumn col_app_Paciente = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_app_Paciente.Title = "Apellido Paterno";
			col_app_Paciente.PackStart(cellrt3, true);
			col_app_Paciente.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 3 en vez de 4
			col_app_Paciente.SortColumnId = (int) Column.col_app_Paciente;
            
			TreeViewColumn col_apm_Paciente = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_apm_Paciente.Title = "Apellido Materno";
			col_apm_Paciente.PackStart(cellrt4, true);
			col_apm_Paciente.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 5 en vez de 6
			col_apm_Paciente.SortColumnId = (int) Column.col_apm_Paciente;
      
			TreeViewColumn col_fechanacimiento_Paciente = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_fechanacimiento_Paciente.Title = "Fecha Nacimiento";
			col_fechanacimiento_Paciente.PackStart(cellrt5, true);
			col_fechanacimiento_Paciente.AddAttribute (cellrt5, "text", 5);     // la siguiente columna será 6 en vez de 7
			col_fechanacimiento_Paciente.SortColumnId = (int) Column.col_fechanacimiento_Paciente;
            
			TreeViewColumn col_edad_Paciente = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_edad_Paciente.Title = "Edad";
			col_edad_Paciente.PackStart(cellrt6, true);
			col_edad_Paciente.AddAttribute (cellrt6, "text", 6); // la siguiente columna será 7 en vez de 8
			col_edad_Paciente.SortColumnId = (int) Column.col_edad_Paciente;
            
			TreeViewColumn col_sexo_Paciente = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_sexo_Paciente.Title = "Sexo";
			col_sexo_Paciente.PackStart(cellrt7, true);
			col_sexo_Paciente.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 8 en vez de 9
			col_sexo_Paciente.SortColumnId = (int) Column.col_sexo_Paciente;
                        
			TreeViewColumn col_creacion_Paciente = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_creacion_Paciente.Title = "Fecha creacion";
			col_creacion_Paciente.PackStart(cellrt8, true);
			col_creacion_Paciente.AddAttribute (cellrt8, "text", 8); // la siguiente columna será 8 en vez de 9
			col_creacion_Paciente.SortColumnId = (int) Column.col_creacion_Paciente;
			
			TreeViewColumn col_id_empleado = new TreeViewColumn();
			CellRendererText cellrt9 = new CellRendererText();
			col_id_empleado.Title = "Quien Registro";
			col_id_empleado.PackStart(cellrt9, true);
			col_id_empleado.AddAttribute (cellrt9, "text", 9); // la siguiente columna será 8 en vez de 9
			col_id_empleado.SortColumnId = (int) Column.col_id_empleado;
                              		
			lista_de_Pacientes.AppendColumn(col_PidPaciente);
			lista_de_Pacientes.AppendColumn(col_Nombre1_Paciente);
			lista_de_Pacientes.AppendColumn(col_Nombre2_Paciente);
			lista_de_Pacientes.AppendColumn(col_app_Paciente);
			lista_de_Pacientes.AppendColumn(col_apm_Paciente);
			lista_de_Pacientes.AppendColumn(col_fechanacimiento_Paciente);
			lista_de_Pacientes.AppendColumn(col_edad_Paciente);
			lista_de_Pacientes.AppendColumn(col_sexo_Paciente);
			lista_de_Pacientes.AppendColumn(col_creacion_Paciente);
			lista_de_Pacientes.AppendColumn(col_id_empleado);
		}
		
		enum Column
		{
			col_PidPaciente,
			col_Nombre1_Paciente,
			col_Nombre2_Paciente,
			col_app_Paciente,
			col_apm_Paciente,
			col_fechanacimiento_Paciente,
			col_edad_Paciente,
			col_sexo_Paciente,
			col_creacion_Paciente,
			col_id_empleado
		}
		
		// Graba informacion datos el paciente asignandole un numero de expediente
		// ademas verifica que que si marco alguna admision grabe los datos correspondiente
		// para que los pueda ver caja u urgencias, hospitalizacion, terapias
		public bool grabar_informacion()
		{
			//bool agregar_registro = false;
			
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				// asigna el numero de paciente (PID)
				comando.CommandText = "SELECT pid_paciente FROM osiris_his_paciente ORDER BY pid_paciente DESC LIMIT 1;";
                              	
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if ((bool) lector.Read()){
					PidPaciente = (int) lector["pid_paciente"] + 1;
					lector.Close ();
				}else{
					PidPaciente = 1;
				}
				// Agregando el nuevo registro de paciente
				comando.CommandText = "INSERT INTO osiris_his_paciente (fechahora_registro_paciente,"+
             						"nombre1_paciente,nombre2_paciente,"+
                					"apellido_paterno_paciente,apellido_materno_paciente,"+
                					"fecha_nacimiento_paciente,rfc_paciente,curp_paciente,"+
                					"direccion_paciente,numero_casa_paciente,colonia_paciente,"+
                					"codigo_postal_paciente,telefono_particular1_paciente,telefono_trabajo1_paciente,"+
                					"celular1_paciente,email_paciente,estado_civil_paciente,"+
                					"sexo_paciente,municipio_paciente,estado_paciente,ocupacion_paciente,"+
                					"id_quienlocreo_paciente,pid_paciente,id_empresa,activo,"+
									"religion_paciente,alegias_paciente,lugar_nacimiento_paciente) VALUES ('"+
                					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
                					entry_nombre_1.Text.ToUpper().Trim()+"','"+
                					entry_nombre_2.Text.ToUpper().Trim()+"','"+
                					entry_apellido_paterno.Text.ToUpper().Trim()+"','"+
                					entry_apellido_materno.Text.ToUpper().Trim()+"','"+
                					entry_ano_nacimiento.Text+"-"+entry_mes_nacimiento.Text+"-"+entry_dia_nacimiento.Text+"','"+
                					entry_rfc.Text.ToUpper().Trim()+"','"+
                					entry_curp.Text.ToUpper().Trim()+"','"+
                					entry_calle.Text.ToUpper().Trim()+"','"+
                					entry_numero.Text+"','"+
                					entry_colonia.Text.ToUpper().Trim()+"','"+
                					entry_CP.Text+"','"+
                					entry_telcasa.Text+"','"+
                					entry_teloficina.Text+"','"+
                					entry_telcelular.Text+"','"+
                					entry_email.Text+"','"+
                					estadocivil+"','"+
                					sexopaciente+"','"+
                					municipios+"','"+
                					estado+"','"+
                					entry_ocupacion.Text.ToUpper().Trim()+"','"+
                					LoginEmpleado+"','"+
                					PidPaciente+"','"+
                					idempresa_paciente+"','"+
                					"true"+"','"+
									entry_religion_paciente.Text.ToUpper()+"','"+
									entry_alergia_paciente.Text.ToUpper()+"','"+
									entry_lugar_nacimiento.Text.ToUpper()+"');";
                	//Console.WriteLine("Grabo informacion del Paciente "+PidPaciente.ToString());
					comando.ExecuteNonQuery();					comando.Dispose();
					return true;
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
				return false;
			}
		}
		
		// Actualizando la tabla de movimiento de servicios
		// para que caja pueda lee la informacion
		// Se dan de alta valores en movimientos por departamentos 
		void graba_admision( string tiposervicio , int idtipoadmision)
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			           
			// Verifica que la base de datos este conectada
						
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				// Agregando Folios internos por departamento
				comando.CommandText = "SELECT id_tipo_admisiones,folio_de_servicio_dep "+
							"FROM osiris_erp_movcargos WHERE id_tipo_admisiones = '"+idtipoadmision.ToString()+"'"+
							" ORDER BY folio_de_servicio_dep DESC LIMIT 1;";
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector1 = comando.ExecuteReader ();
				if ((bool) lector1.Read()){
					int foliointernodep = (int) lector1["folio_de_servicio_dep"] + 1;
               			
					entry_folio_interno_dep.Text = entry_folio_interno_dep.Text+tiposervicio+foliointernodep.ToString()+" | ";
               			
					lector1.Close();
               			
					// Agregando el nuevo registro al de movimientos
					comando.CommandText = "INSERT INTO osiris_erp_movcargos (id_tipo_admisiones, id_empleado,"+
								"fechahora_admision_registro,folio_de_servicio,folio_de_servicio_dep,pid_paciente,id_tipo_paciente,"+
								"id_tipo_cirugia,nombre_de_cirugia,tipo_cirugia,descripcion_diagnostico_movcargos ) VALUES ('"+
								idtipoadmision+"', '"+
								LoginEmpleado+"', '"+
								DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
								folioservicio+"','"+
								foliointernodep+"','"+
								PidPaciente+"','"+
								id_tipopaciente+"','"+
								idcirugia+"','"+
								"','"+
								decirugia+"','"+
								diagnostico.ToUpper().Trim()+"');";
					Console.WriteLine(comando.CommandText);	
					comando.ExecuteNonQuery();					comando.Dispose();
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		public bool almacena_encabezado_de_cobro()
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				// Este proceso se utiliza para cerrar las numeros de atencion que se usan en el centro medico 
				bool cerrar_folio;
				bool facturacion_folio;
				string id_quien_cierra_centro_medico;
				string his_de_cerrado_centro_medico;
				if (checkbutton_consulta.Active == true){
					cerrar_folio = true;
					facturacion_folio = true;
					id_quien_cierra_centro_medico = LoginEmpleado;
					his_de_cerrado_centro_medico = LoginEmpleado+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					
				}else{
					cerrar_folio = false;
					facturacion_folio = false;
					id_quien_cierra_centro_medico = "";
					his_de_cerrado_centro_medico = "";
				}

				// Creando el registro de de encabezado para que lo pueda buscar
				// y realizar los cargos correspondiens
				comando.CommandText = "INSERT INTO osiris_erp_cobros_enca (folio_de_servicio,pid_paciente,fechahora_creacion,"+
							"id_empleado_admision,"+
							"responsable_cuenta,"+
							"telefono1_responsable_cuenta,"+
							"direccion_responsable_cuenta,"+
							"empresa_labora_responsable,"+
							"ocupacion_responsable,"+
							"id_aseguradora,"+
							"paciente_asegurado,"+
							"numero_poliza,"+
							"numero_certificado,"+
							"direccion_emp_responsable,"+
							"telefono_emp_responsable,"+
							"parentezco, id_medico,id_empresa,nombre_medico_encabezado,"+
							"cerrado,facturacion,"+
							"observacion_ingreso,"+
							"nombre_empresa_encabezado ) VALUES ('"+
							folioservicio+"','"+
							PidPaciente+"','"+
							DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
							LoginEmpleado+"','"+
							nombr_respo.ToUpper().Trim()+"','"+
							telef_respo+"','"+
							direc_respo.ToUpper().Trim()+"','"+
							empre_respo.ToUpper().Trim()+"','"+
							ocupa_respo.ToUpper().Trim()+"','"+
							idaseguradora+"','"+
							boolaseguradora+"','"+
							poliz_respo.Trim()+"','"+
							certif_respo.Trim()+"','"+
							direc_empre_respo.ToUpper().Trim()+"','"+
							telef_empre_respo.ToUpper().Trim()+"','"+
							parentezcoresponsable+"','"+
							idmedico+"','"+
							idempresa_paciente+"','"+
							nombmedico.ToUpper().Trim()+"','"+
							cerrar_folio+"','"+
							facturacion_folio+"','"+
							entry_observacion_ingreso.Text.ToUpper()+"','"+
							this.entry_empresa.Text.ToString().Trim().ToUpper()+"');";
				//Console.WriteLine("Graba Encabezado");
				comando.ExecuteNonQuery();			comando.Dispose();
				return true;
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
				return false;
			}
			//conexion.Close ();		//return true;
		}
		
		// activa busqueda con boton busqueda
		void on_buscar_busqueda_clicked (object sender, EventArgs a)
		{
			llena_lista_paciente();
		}
		
		void llena_lista_paciente()
		{
			treeViewEngineBusca.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				entry_expresion.Text = this.entry_expresion.Text.Trim();    	               	
				
				if ((string) entry_expresion.Text.Trim() == ""){
					comando.CommandText = "SELECT pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
								"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
								"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mi:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
								"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mi:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
								"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
								"FROM osiris_his_paciente  "+
								"WHERE activo = 'true'  ORDER BY pid_paciente;";
				}else{
					if (radiobutton_busca_apellido.Active == true){
						comando.CommandText = "SELECT pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
									"FROM osiris_his_paciente WHERE apellido_paterno_paciente  LIKE '"+entry_expresion.Text.ToUpper()+"%' "+
									"AND activo = 'true'  ORDER BY pid_paciente;";
									
					}
					if (radiobutton_busca_nombre.Active == true){
						comando.CommandText = "SELECT pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
									"FROM osiris_his_paciente WHERE nombre1_paciente  LIKE '"+entry_expresion.Text.ToUpper()+"%' "+
									"AND activo = 'true'  ORDER BY pid_paciente;";
					}
					if (radiobutton_busca_expediente.Active == true){
						comando.CommandText = "SELECT pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
									"FROM osiris_his_paciente WHERE pid_paciente  = '"+entry_expresion.Text.ToUpper()+"' "+
									"AND activo = 'true'  ORDER BY pid_paciente;";					
					}
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//Console.WriteLine(comando.CommandText);
				//string toma_edad;
				while (lector.Read()){
					treeViewEngineBusca.AppendValues ((int) lector["pid_paciente"], 
										(string) lector["nombre1_paciente"],(string) lector["nombre2_paciente"],
										(string) lector["apellido_paterno_paciente"], (string) lector["apellido_materno_paciente"],
										(string) lector["fech_nacimiento"],(string) lector["edad"]+" Años y "+(string) lector["mesesedad"]+" Meses",
										(string) lector["sexo_paciente"],(string) lector["fech_creacion"],
										(string) lector["id_quienlocreo_paciente"]);
				}
				if(ventana_principal == true) {/*Console.WriteLine("activo boton");*/ button_nuevo_paciente.Sensitive = true;}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();	msgBoxError.Destroy();
            }
            conexion.Close ();
		}
				
		void on_selecciona_paciente_clicked(object sender, EventArgs a)
		{
			llena_datos_del_paciente("seleccion_admision","");
			// destruye la ventana de busqueda
	 		Widget win = (Widget) sender;
			win.Toplevel.Destroy();	
		}
		
		void llena_datos_del_paciente(string opcion_,string pidpaciente_)
		{
			if(opcion_ == "seleccion_admision"){
				TreeModel model;
				TreeIter iterSelected;
				if (lista_de_Pacientes.Selection.GetSelected(out model, out iterSelected)) {
					if(ventana_principal == false) { registro.Destroy(); }
					PidPaciente = (int) model.GetValue(iterSelected, 0);
					//llena_valores_entry();		
					Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "registro", null);
					gxml.Autoconnect (this);
		        		        
					// Muestra ventana de Glade
					registro.Show();
					
					llena_Ventana_de_datos(PidPaciente.ToString().Trim());
					llena_inf_de_paciente(PidPaciente.ToString().Trim());
					activa_los_entry(false);
									
					button_admision.Sensitive = true;  // Activando Boton de Internamiento de Paciente
					//Activa el boton para editar datos de paciente
					checkbutton_modificar.Sensitive = false;
					if(LoginEmpleado == "DOLIVARES" || LoginEmpleado == "ADMIN") { 
		           		checkbutton_modificar.Sensitive = true;
		           		checkbutton_modificar.Clicked += new EventHandler(on_modifica_informacion_clicked);
		           		button_cancelar_pid.Sensitive = true;
		           		button_cancelar_pid.Clicked += new EventHandler(on_button_cancelar_pid_clicked);
		           	}
					// activa boton de grabacion de informacion
					button_grabar.Clicked += new EventHandler(on_graba_informacion_clicked);
					
					// Activa boton de responsable
					button_responsable.Clicked += new EventHandler(on_button_responsable_clicked);
					// Activa boton de admision Urgenacias/Hospital/Quirofano
					//button_admision.Clicked += new EventHandler(on_button_admision_clicked);
		        	// Activacion de boton de busqueda
					button_buscar_paciente.Clicked += new EventHandler(on_button_buscar_paciente_clicked);
					// Centro Medico
					entry_medico_cm.Sensitive = false;
					button_busca_medicos.Sensitive = false;
					entry_esp_med_cm.Sensitive = false;
					checkbutton_consulta.Clicked += new EventHandler(on_checkbutton_consulta_clicked);
		        	
					//Activa boton para imprimir el protocolo
					button_imprimir_protocolo.Clicked += new EventHandler(on_button_imprimir_protocolo_clicked);
					button_imprimir_protocolo.Sensitive = false;
		        	
					// Contratacion de paquetes
					button_contrata_paquete.Clicked += new EventHandler(on_button_contrata_paquete_clicked);
					button_contrata_paquete.Sensitive = false;
					
					//Desactiva campos de PID y de FOLIO para que no se escriba en ellos
		        	entry_pid_paciente.Sensitive = false;
		        	entry_folio_paciente.Sensitive = false;
		        	entry_folio_interno_dep.Sensitive = false;
		        	
		        	// Asugnacion de Cuarto o Cubiculo
		        	button_asignacion_habitacion.Clicked += new EventHandler(on_button_asignacion_habitacion_clicked);
		        	
		        	//Lista a las empresas con convenio
		        	button_lista_empresas.Clicked += new EventHandler(on_button_lista_empresas_clicked);
		        	
		        	// este entry se activa cuando el paciente solicita otros servicios
		       	    //entry_observacion.Sensitive = false;
		       	    
		       	    button_separa_folio.Clicked += new EventHandler(on_button_separa_folio_clicked);
		        	
					entry_pid_paciente.Text = PidPaciente.ToString();
					entry_observacion_ingreso.Sensitive = true;
					
					// Cambio para VENEZUELA
					//label37.Text = "RIF";			// RFC
					//label43.Text = "C.I.";		// CURP					
	 			}
			}
			if(opcion_ == "seleccion_no_admision"){
				Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "registro", null);
				gxml.Autoconnect (this);
		        		        
				// Muestra ventana de Glade
				registro.Show();
				
				// Cambio para VENEZUELA
				//label37.Text = "RIF";			// RFC
				//label43.Text = "C.I.";		// CURP
					
				llena_Ventana_de_datos(pidpaciente_.ToString().Trim());
				llena_inf_de_paciente(pidpaciente_.ToString().Trim());
				activa_los_entry(false);
			}
		}
		
		void on_button_separa_folio_clicked(object sender, EventArgs a)
		{
			int folioservicio = 0;
			TreeModel model;
			TreeIter iterSelected;
			if (this.treeview_servicios.Selection.GetSelected(out model, out iterSelected)){
				folioservicio = int.Parse((string) model.GetValue(iterSelected, 4));
				new osiris.reservacion_de_paquetes(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,folioservicio,true);
			}	//treeViewEngine,descripcion_cirugia
		}

		// Cuando el paciente no es nuevo viene a este 
		void llena_Ventana_de_datos(string pidpaciente_)
		{
			// Cierra Ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_cancelar_pid.Sensitive = false;
			button_admision.Clicked += new EventHandler(on_button_admision_clicked);
			// desactiva botton de intermaniento de paciente
			button_admision.Sensitive = true;
	        //Entrada de Fecha de Nacimiento valida solo numeros
			//Dia
			entry_dia_nacimiento.KeyPressEvent += onKeyPressEvent;
			//Mes
			entry_mes_nacimiento.KeyPressEvent += onKeyPressEvent;
			//Ano
			entry_ano_nacimiento.KeyPressEvent += onKeyPressEvent;
			// Disable Internar a Centro Medico
			checkbutton_consulta.Sensitive = false;
			// llenado de comobobox
			// Tipos de Paciente
			combobox_tipo_paciente.Clear();
			CellRendererText cell1 = new CellRendererText();
			combobox_tipo_paciente.PackStart(cell1, true);
			combobox_tipo_paciente.AddAttribute(cell1,"text",0);
	        
			ListStore store1 = new ListStore( typeof (string),typeof (int));
			combobox_tipo_paciente.Model = store1;
			store1.Clear();
			
			store1.AppendValues ("",0);
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			//this.PidPaciente
			try{
				conexion.Open ();
				NpgsqlCommand comando;
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT * FROM osiris_his_tipo_pacientes ORDER BY descripcion_tipo_paciente;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					store1.AppendValues ((string) lector["descripcion_tipo_paciente"].ToString().ToUpper(),(int) lector["id_tipo_paciente"]);
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
			
			TreeIter iter1;
			if (store1.GetIterFirst(out iter1)){
				combobox_tipo_paciente.SetActiveIter (iter1);
			}
			combobox_tipo_paciente.Changed += new EventHandler (onComboBoxChanged_tipopaciente);  // activa casilla de 
	        																			  // Aseguradora 
			entry_empresa.Sensitive = false;
			
			// Tipos de Aseguradoras
			combobox_aseguradora.Clear();
			CellRendererText cell2 = new CellRendererText();
			combobox_aseguradora.PackStart(cell2, true);
			combobox_aseguradora.AddAttribute(cell2,"text",0);
	        
			store_aseguradora = new ListStore( typeof (string), typeof (int) );
			combobox_aseguradora.Model = store_aseguradora;
			combobox_aseguradora.Sensitive = false;
	        
			// Llenar con tabla de aseguradoras
			llena_cmbox_aseguradora();
			//store_aseguradora.AppendValues ("111",1);
	        
			TreeIter iter2;
			if (store_aseguradora.GetIterFirst(out iter2)){
				combobox_aseguradora.SetActiveIter (iter2);
			}
			combobox_aseguradora.Changed += new EventHandler (onComboBoxChanged_aseguradora);
	        		        
			// Creacion de Liststore
			treeViewEngine = new TreeStore(typeof (string),typeof (string),typeof (string), typeof (string), 
							typeof (string),typeof (string),typeof (string), typeof (string), typeof (string),typeof (string),typeof (bool));
	        							   
			treeview_servicios.Model = treeViewEngine;
			treeViewEngine.SetSortColumnId (0, Gtk.SortType.Ascending);
			                            
			TreeViewColumn col_fecha = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();  // aplica a todas la columnas
			col_fecha.Title = "Fecha-Hora"; // titulo de la cabecera de la columna, si está visible
			col_fecha.PackStart(cellrt1, true);
			col_fecha.AddAttribute (cellrt1, "text", 0);
			col_fecha.SortColumnId = (int) Column_serv.col_fecha;
             
			TreeViewColumn col_servicio = new TreeViewColumn();
			col_servicio.Title = "Servicio";
			col_servicio.PackStart(cellrt1, true);
			col_servicio.AddAttribute (cellrt1, "text", 1);
			col_servicio.SortColumnId = (int) Column_serv.col_servicio ;
      
			TreeViewColumn col_desc_servicio = new TreeViewColumn();
			col_desc_servicio.Title = "Diagnostico Admision";
			col_desc_servicio.PackStart(cellrt1, true);
			col_desc_servicio.AddAttribute (cellrt1, "text", 2);
			col_desc_servicio.SortColumnId = (int) Column_serv.col_desc_servicio;
            
			TreeViewColumn col_valor = new TreeViewColumn();
			col_valor.Title = "Tipo de Cirugia";
			col_valor.PackStart(cellrt1, true);
			col_valor.AddAttribute (cellrt1, "text", 3);
			col_valor.SortColumnId = (int) Column_serv.col_valor;
            
			TreeViewColumn col_folio_ingreso = new TreeViewColumn();
			col_folio_ingreso.Title = "Folio Ingreso";
			col_folio_ingreso.PackStart(cellrt1, true);
			col_folio_ingreso.AddAttribute (cellrt1, "text", 4);
			col_folio_ingreso.SortColumnId = (int) Column_serv.col_folio_ingreso;
			
			TreeViewColumn col_num_factura = new TreeViewColumn();
			col_num_factura.Title = "N. Factura";
			col_num_factura.PackStart(cellrt1, true);
			col_num_factura.AddAttribute (cellrt1, "text", 5);
			col_num_factura.SortColumnId = (int) Column_serv.col_num_factura;			
            
			TreeViewColumn col_folio_ingreso_dep = new TreeViewColumn();
			col_folio_ingreso_dep.Title = "Folio Departamento";
			col_folio_ingreso_dep.PackStart(cellrt1, true);
			col_folio_ingreso_dep.AddAttribute (cellrt1, "text", 6);
			col_folio_ingreso_dep.SortColumnId = (int) Column_serv.col_folio_ingreso_dep;
			
			TreeViewColumn col_tipo_paciente = new TreeViewColumn();
			col_tipo_paciente.Title = "Tipo de Paciente";
			col_tipo_paciente.PackStart(cellrt1, true);
			col_tipo_paciente.AddAttribute (cellrt1, "text", 7);
			col_tipo_paciente.SortColumnId = (int) Column_serv.col_tipo_paciente;
			
			TreeViewColumn col_empresaasegu = new TreeViewColumn();
			col_empresaasegu.Title = "Empresa/Aseguradora";
			col_empresaasegu.PackStart(cellrt1, true);
			col_empresaasegu.AddAttribute (cellrt1, "text", 8);
			col_empresaasegu.SortColumnId = (int) Column_serv.col_empresaasegu;
						 
			TreeViewColumn col_admitio = new TreeViewColumn();
			col_admitio.Title = "Admitio";
			col_admitio.PackStart(cellrt1, true);
			col_admitio.AddAttribute (cellrt1, "text", 9);
			col_admitio.SortColumnId = (int) Column_serv.col_admitio;
			
			TreeViewColumn col_separacion = new TreeViewColumn();
			CellRendererToggle cellrtogg = new  CellRendererToggle();
			col_separacion.Title = "Separacion PQ.";
			col_separacion.PackStart(cellrtogg, true);
			col_separacion.AddAttribute (cellrtogg, "active", 10);
			col_separacion.SortColumnId = (int) Column_serv.col_separacion;
                        
			treeview_servicios.AppendColumn(col_fecha);
			treeview_servicios.AppendColumn(col_servicio);
			treeview_servicios.AppendColumn(col_desc_servicio);
			treeview_servicios.AppendColumn(col_valor);
			treeview_servicios.AppendColumn(col_folio_ingreso);
			treeview_servicios.AppendColumn(col_num_factura);
			treeview_servicios.AppendColumn(col_folio_ingreso_dep);
			treeview_servicios.AppendColumn(col_tipo_paciente);
			treeview_servicios.AppendColumn(col_empresaasegu);
			treeview_servicios.AppendColumn(col_admitio);
		 	treeview_servicios.AppendColumn(col_separacion);
			
			//Llena treview de servicio realizados
			// _tipo_ es una variable publica esta al inicio del programa
			if (_tipo_=="busca1"){
  				llena_servicios_realizados(pidpaciente_);
				//Console.WriteLine("llenando informacion");
			}
			if(_tipo_ == "selecciona"){
				llena_servicios_realizados(pidpaciente_);
			}

			// Actulizando statusbar
			statusbar_registro.Pop(0);
			statusbar_registro.Push(1, "login: "+LoginEmpleado+"| Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);// NomEmp_);
			statusbar_registro.HasResizeGrip = false;		
		}
		
		enum Column_serv
		{
			col_fecha,
			col_servicio,
			col_desc_servicio,
			col_valor,
			col_folio_ingreso,
			col_num_factura,
			col_folio_ingreso_dep,
			col_tipo_paciente,
			col_empresaasegu,
			col_admitio,
			col_separacion
		}
		
		void llena_servicios_realizados(string pidpaciente_)
		{
			//Console.WriteLine("llenando informacion "+PidPaciente.ToString());
			treeViewEngine.Clear();
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			//this.PidPaciente
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd HH24:mi') AS fechahoraadm, "+
									"osiris_his_tipo_admisiones.descripcion_admisiones,osiris_erp_cobros_enca.cancelado, "+
									"osiris_erp_movcargos.descripcion_diagnostico_movcargos, osiris_erp_movcargos.id_tipo_cirugia, "+
									//"osiris_his_tipo_cirugias.descripcion_cirugia, "+
									"to_char(osiris_erp_cobros_enca.folio_de_servicio,'9999999') AS folioserv, "+
									"to_char(osiris_erp_movcargos.folio_de_servicio_dep,'9999999') AS folioservdep, "+
									"osiris_his_tipo_pacientes.descripcion_tipo_paciente,osiris_erp_cobros_enca.id_empleado_admision,"+
									"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,"+
									"osiris_erp_cobros_enca.id_empresa,descripcion_empresa,"+
									"osiris_erp_cobros_enca.reservacion,"+
									"osiris_erp_cobros_enca.fecha_reservacion,"+
									"osiris_his_tipo_cirugias.descripcion_cirugia,"+
									"to_char(osiris_erp_cobros_enca.numero_factura,'9999999999') AS numerofactura "+
									"FROM "+
									"osiris_erp_cobros_enca,osiris_erp_movcargos,osiris_his_tipo_pacientes,osiris_his_tipo_cirugias,osiris_his_tipo_admisiones,osiris_aseguradoras,osiris_empresas "+
									"WHERE "+
									" osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_movcargos.folio_de_servicio "+
									"AND osiris_erp_movcargos.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
									"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
									"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
									"AND osiris_erp_cobros_enca.pid_paciente = '"+pidpaciente_.ToString() +"' "+
									"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
									"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+  
									"ORDER BY to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd HH24:mm') ;";
				//Console.WriteLine("Querry: "+comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				string diagnostico_cirugia = "";
				string aseguradora_empresa = "";
				string foliosseparados = "";
				bool folioreservado = false;
				while (lector.Read()){
					
					diagnostico_cirugia = (string) lector["descripcion_diagnostico_movcargos"];
									
					if((int) lector ["id_aseguradora"] > 1){
						aseguradora_empresa = (string) lector["descripcion_aseguradora"];
					}else{
						aseguradora_empresa = (string) lector["descripcion_empresa"];						
					}
					
					if (!(bool) lector["cancelado"]){ 
						treeViewEngine.AppendValues ((string) lector["fechahoraadm"],
															(string) lector["descripcion_admisiones"],
															diagnostico_cirugia,
															(string) lector["descripcion_cirugia"],
															(string) lector["folioserv"],
															(string) lector["numerofactura"],
															(string) lector["folioservdep"],
															(string) lector["descripcion_tipo_paciente"],
															aseguradora_empresa,
															(string) lector["id_empleado_admision"],
						                             		(bool) lector["reservacion"]);
					}
					if (folioreservado == false && (bool) lector["reservacion"] == true){
						folioreservado = true;
						foliosseparados += (string) lector["folioserv"].ToString().Trim()+ " - ";
					}
				}
				if (folioreservado == true){
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
								MessageType.Info,ButtonsType.Ok,"El paciente tiene separado un folio para un paquete quirurgico N Folio: "+foliosseparados);
					msgBox.Run ();			msgBox.Destroy();
				
				}
			}catch (NpgsqlException ex){
	   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_modifica_informacion_clicked(object sender, EventArgs a)
		{
	     	if(checkbutton_modificar.Active == true) {
	     		//Console.WriteLine("modificar activado"); 
	     		activa_los_entry(true);// editar = true; 
	     	}
	     	if(checkbutton_modificar.Active == false) {
	     		//Console.WriteLine("modificar desactivado");
	     		activa_los_entry(false); //editar = false;
	     	}
		}
		
		void activa_los_entry(bool valor)
		{
			entry_nombre_1.Sensitive = valor;
			entry_nombre_2.Sensitive = valor;
			entry_apellido_paterno.Sensitive = valor;
			entry_apellido_materno.Sensitive = valor;
			entry_dia_nacimiento.Sensitive = valor;
			entry_mes_nacimiento.Sensitive = valor;
			entry_ano_nacimiento.Sensitive = valor;
			entry_rfc.Sensitive = valor;
			entry_curp.Sensitive = valor;
					
			entry_ocupacion.Sensitive = valor;		
			entry_empresa.Sensitive = valor;
					
			entry_email.Sensitive = valor;
			entry_calle.Sensitive = valor;
			entry_numero.Sensitive = valor;
			entry_colonia.Sensitive = valor;
			entry_CP.Sensitive = valor;
			entry_telcasa.Sensitive = valor;
			entry_teloficina.Sensitive = valor;
			entry_telcelular.Sensitive = valor;
			combobox_estado_civil.Sensitive = valor;
			entry_religion_paciente.Sensitive = valor;
			entry_alergia_paciente.Sensitive = valor;
			entry_observacion_ingreso.Sensitive = valor;
			entry_lugar_nacimiento.Sensitive = valor;
			
			combobox_aseguradora.Sensitive = valor;
			combobox_municipios.Sensitive = valor;
			combobox_estado.Sensitive = valor;
		}
		
		void on_button_responsable_clicked (object sender, EventArgs a) 
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "datos_del_responsable", null);
			gxml.Autoconnect (this);
	                	
			// Muestra ventana de Glade
			datos_del_responsable.Show();
			
			// Salir de ventana de responsable
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
					
			// Paciente es el Responsable
			//button_paciente_responsable.Sensitive = false; 
			button_paciente_responsable.Clicked += new EventHandler(on_button_paciente_responsable_clicked);
			
			// Misma direccion del responsable
			button_misma_direccion.Clicked += new EventHandler(on_button_misma_direccion_clicked);
			
			// Traspasa aseguradora si se selecciono aseguradora
			entry_aseguradora_responsable.Text = (string) nombre_aseguradora;
			
			// graba datos del responsable de la cuenta
			button_graba_responsable.Clicked += new EventHandler(on_button_graba_responsable_clicked);
			// Verifica si grabo anteriormente y traspasa los valores
			
			// Parentezco del Responsable de la cuenta
			combobox_parent_responsable.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_parent_responsable.PackStart(cell3, true);
			combobox_parent_responsable.AddAttribute(cell3,"text",0);
	        
			ListStore store3 = new ListStore( typeof (string));
			combobox_parent_responsable.Model = store3;
	        		
			store3.AppendValues ("Sin Parentesco");
			store3.AppendValues ("Esposo(a)");
			store3.AppendValues ("Papa");
			store3.AppendValues ("Mama");
			store3.AppendValues ("Abuelo(a)");
			store3.AppendValues ("Hermano(a)");
			store3.AppendValues ("Primo(a)");
			store3.AppendValues ("Tio(a)");
			store3.AppendValues ("Cuñado(a)");
			store3.AppendValues ("Tutor");
			store3.AppendValues ("Hijo(a)");
			store3.AppendValues ("Concuño(a)");
	        		
			TreeIter iter3;
			if (store3.GetIterFirst(out iter3)){
				combobox_parent_responsable.SetActiveIter (iter3);
			}
			combobox_parent_responsable.Changed += new EventHandler (onComboBoxChanged_parent_responsable);
	        
			if (grabarespocuenta == true)
			{
				entry_nombre_responsable.Text = (string) nombr_respo;
				entry_telefono_responsable.Text = (string) telef_respo;
				entry_direcc_responsable.Text = (string) direc_respo;
				entry_empresa_responsable.Text = (string) empre_respo;
				entry_ocupacion_responsable.Text = (string) ocupa_respo;
				entry_aseguradora_responsable.Text = (string) asegu_respo;
				entry_poliza_responsable.Text = (string) poliz_respo;
				entry_certificado_acta.Text = (string) certif_respo;
				entry_dire_emp_responsable.Text = (string) direc_empre_respo;
				entry_tel_emp_responsable.Text = (string) telef_empre_respo;
				//combobox_parent_responsable.Text = (string) parentezcoresponsable;
			}
		}
		
		void on_button_paciente_responsable_clicked (object sender, EventArgs a)
		{
			entry_nombre_responsable.Text = (string) entry_nombre_1.Text+" "+(string) entry_nombre_2.Text+" "+
							(string) entry_apellido_paterno.Text+" "+(string) entry_apellido_materno.Text;
			
			entry_direcc_responsable.Text = (string) entry_calle.Text+" "+entry_numero.Text+" Col. "+entry_colonia.Text+" CP. "+
							(string) entry_CP.Text+", "+(string) municipios+", "+(string) estado;
			entry_empresa_responsable.Text = (string) entry_empresa.Text;
			entry_ocupacion_responsable.Text = (string) entry_ocupacion.Text;
			
		}
		
		void on_button_misma_direccion_clicked (object sender, EventArgs a)
		{
			entry_direcc_responsable.Text = (string) entry_calle.Text+" "+entry_numero.Text+" Col. "+entry_colonia.Text+" CP. "+
							(string) entry_CP.Text+", "+(string) municipios+", "+(string) estado;
			entry_telefono_responsable.Text = (string) entry_telcasa.Text;
			
		}
		
		void on_button_graba_responsable_clicked (object sender, EventArgs a)
		{
			if(entry_nombre_responsable.Text.Trim() != "" && entry_poliza_responsable.Text.Trim() != "" &&
				entry_certificado_acta.Text.Trim() != "" )
			{
				nombr_respo = (string) entry_nombre_responsable.Text;
				telef_respo = (string) entry_telefono_responsable.Text;
				direc_respo = (string) entry_direcc_responsable.Text;
				empre_respo = (string) entry_empresa_responsable.Text;
				ocupa_respo = (string) entry_ocupacion_responsable.Text;
				asegu_respo = (string) nombre_aseguradora;
				poliz_respo = (string) entry_poliza_responsable.Text;
				certif_respo = (string) entry_certificado_acta.Text;
				direc_empre_respo = (string) entry_dire_emp_responsable.Text;
				telef_empre_respo = (string) entry_tel_emp_responsable.Text;
				grabarespocuenta = true;
				
				// cierra la ventana despues que almaceno la informacion en variables
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close,"No debe dejar los campos de nombre, de certificado \n"+
							"y/o de numero de poliza en blanco.");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
		}
		// Parentezco del paciente
		public void onComboBoxChanged_parent_responsable (object sender, EventArgs args)
		{
			ComboBox combobox_parent_responsable = sender as ComboBox;
			if (sender == null){
				return;
			}
			TreeIter iter;
			if (combobox_parent_responsable.GetActiveIter (out iter)){
				parentezcoresponsable = (string) combobox_parent_responsable.Model.GetValue(iter,0);
			}
		}
		
		// Activa en enter en la busqueda de los productos
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
		//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				if(busqueda == "paciente") { llena_lista_paciente();}
				if(busqueda == "medicos") {llenando_lista_de_medicos();}
				//if(busqueda == "cirugia") {llenando_lista_de_cirugias();}				
			}
		}
	}
}
