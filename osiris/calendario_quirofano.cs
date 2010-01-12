// calendario_quirofano.cs created with MonoDevelop
// User: mgaspar at 12:48 p 14/07/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
 {
	public class calendario_quirofano
	{				
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		
		//Nueva programacion
		[Widget] Gtk.CheckButton checkbutton_nueva_cirugia;
		
		// Boton busqueda de medicos
		[Widget] Gtk.TreeView lista_de_busqueda;
		[Widget] Gtk.TreeView lista_de_medicos;
		[Widget] Gtk.ComboBox combobox_tipo_busqueda;
		
		//Datos del paciente
		[Widget] Gtk.Entry entry_pid_paciente;
		[Widget] Gtk.Entry entry_folio_servicio;
		[Widget] Gtk.Entry entry_nombre_paciente;		
		[Widget] Gtk.Entry entry_edad;
		[Widget] Gtk.Entry entry_sexo;
		[Widget] Gtk.CheckButton checkbutton_asignar_folio;
		[Widget] Gtk.ComboBox combobox_sala_1;
		
		//Buscar cirugia		
		[Widget] Gtk.Entry entry_id_cirugia;
		[Widget] Gtk.Entry entry_cirugia;
		[Widget] Gtk.Button button_buscar_cirugia;
		[Widget] Gtk.TreeView lista_cirugia;
		[Widget] Gtk.Button button_llena_cirugias;
		[Widget] Gtk.Entry entry_id_especialidad; 
		[Widget] Gtk.Entry entry_descripcion_especialidad;
		[Widget] Gtk.Button button_selec_id;
		
		// Ventana Principal //
		[Widget] Gtk.Window programacion_cirugias;
		[Widget] Gtk.Button button_buscar_medico;
		[Widget] Gtk.Entry entry_medico;
		[Widget]Gtk.Entry entry_programacion;
		[Widget]Gtk.Entry entry_dia;
		[Widget]Gtk.Entry entry_mes;
		[Widget]Gtk.Entry entry_año;
		[Widget]Gtk.Entry entry_hora;
		[Widget]Gtk.Entry entry_paciente_prog;
		[Widget]Gtk.Entry entry_sexo_prog;
		[Widget]Gtk.Entry entry_edad_prog;		
		[Widget]Gtk.ComboBox combobox_sala_2;
		[Widget]Gtk.Entry entry_dia2;
		[Widget]Gtk.Entry entry_mes2;
		[Widget]Gtk.Entry entry_año2;
		[Widget]Gtk.Entry entry_hora2;
		[Widget] Gtk.Button button_cancelar_progra;
		[Widget]Gtk.Entry entry_aseguradora;
		[Widget]Gtk.Entry entry_tipo_paciente;
		[Widget]Gtk.Entry entry_especialidadmed;
	    //[Widget]Gtk.Entry entry_id_medico;
		[Widget]Gtk.Button button_busca_paciente_prog;
		[Widget]Gtk.Button button_guardar_prog;
		
		//buscar diagnostico
		[Widget]Gtk.Button button_busc_diag;
	    [Widget]Gtk.Button button_selec_id_diag;
		[Widget]Gtk.Entry entry_id_diagnostico;
		[Widget]Gtk.TreeView lista_diagnostico;
		[Widget]Gtk.Button button_llena_diagnostico;
		[Widget]Gtk.Entry entry_diagnostico;
		
		//busqueda diagnostico cie-10
		[Widget] Gtk.Window busca_producto;                  
		[Widget] Gtk.TreeView lista_de_producto;		
		[Widget] Gtk.Entry entry_expresion_busca;
		[Widget] Gtk.RadioButton radiobutton_nombre;
		[Widget] Gtk.RadioButton radiobutton_codigo;
				
		/////// Ventana Busqueda de paciente\\\\\\\\
		[Widget] Gtk.Window busca_paciente;
		[Widget] Gtk.TreeView lista_de_Pacientes;		
		[Widget] Gtk.Button button_nuevo_paciente;
		[Widget] Gtk.RadioButton radiobutton_busca_apellido;
		[Widget] Gtk.RadioButton radiobutton_busca_nombre;
		[Widget] Gtk.RadioButton radiobutton_busca_expediente;
		
		//buscar programacion
		[Widget]Gtk.Button button_buscar_programacion;
		[Widget]Gtk.Button button_seleccionar_programacion;
		
		//busqueda de empleados		
		[Widget]Gtk.Button button_buscar_cirujano;
		[Widget]Gtk.Button button_buscar_neonatologo;
		[Widget]Gtk.Button button_buscar_circulante1;
		[Widget]Gtk.Button button_buscar_ayudante;
		[Widget]Gtk.Button button_buscar_anestesiologo;
		[Widget]Gtk.Button button_buscar_circulante2;
		[Widget]Gtk.Button button_buscar_internista;
		[Widget]Gtk.Entry entry_cirujano;
        [Widget]Gtk.Entry entry_ayudante;
        [Widget]Gtk.Entry entry_neonatologo;
        [Widget]Gtk.Entry entry_anestesiologo;
        [Widget]Gtk.Entry entry_circulante1;
        [Widget]Gtk.Entry entry_circulante2;
        [Widget]Gtk.Entry entry_internista;
		
		//ventana busca_empleado
		[Widget] Gtk.Window busca_empleado;
		[Widget] Gtk.Button button_buscar_emp;	     
		[Widget] Gtk.RadioButton radiobutton_busca_numero;
		[Widget] Gtk.TreeView lista_de_empleados;
	    
		//horarios, notas e instrumentos de quirofano
		[Widget]Gtk.Entry entry_entrada_sala;
		[Widget]Gtk.Entry entry_salida_sala;
		[Widget]Gtk.Entry entry_especialidad_cirugia;
		[Widget]Gtk.Entry entry_inicio_cirugia;
		[Widget]Gtk.Entry entry_termino_cirugia;		
		[Widget]Gtk.Entry entry_intrs_especial;
		[Widget]Gtk.Entry entry_notas;
		[Widget]Gtk.Entry entry_tipo_anestecia;
		[Widget]Gtk.Calendar calandario_quirofano;
		
		int idtipocirugia = 1;	        			// Toma el valor de numero de atencion de paciente
		string cirugia;
		int idtipoesp = 1;
		string especialidad;
		int iddiagnostico = 1;
		string diagnostico;
		int id_medico = 1;
		
		int id_cirujano = 1;
		int id_neonatologo = 1;
		int id_ayudante = 1;
		int id_anestesiologo = 1;
		int id_circulante1 = 1;
		int id_circulante2 = 1;
		int id_internista = 1;
		int id_seleccion = 1;
		
		int id_esp_medico = 1;
		string iddiag;
		string  selcampo = "";

		int ultimaprogramacion = 1;
		
		//variables principales
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		string nomcatalogo;
		string busqueda = "";
		string nombmedico = "";
		string nombanestesiologo = "";
		string especialidadmed ="";
		string paciente_prog = "";		
		int tipobusqueda_doctor = 1;
		int tipobusqueda_empleado = 1;
		int sala_2 = 1;
		int sala_1 = 1;		
		int iddiagnosticocie10 = 1;	// Toma el valor del id de tabla de diagnosticos
		int iddiagnosticofinal = 1;	// toma el valor del id del diagnostico final
		int idpresupuesto_qx = 1;		
		int folio_servicio = 1;
		string tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";
		
		string connectionString = "Server=localhost;" +
									"Port=5432;" +
									 "User ID=admin;" +
									"Password=1qaz2wsx;";		
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		private TreeStore treeViewEngineMedicos;
		private TreeStore treeViewEngineEspecialidad;
		private TreeStore treeViewEngineBusca;
		private TreeStore treeViewEngineBusca2;
		private TreeStore treeViewEngineServicio;
		private TreeStore treeViewEngineBusca3;
		
		///Declaracion de columnas de busqueda de Medicos
		TreeViewColumn col_idmedico;		TreeViewColumn col_nomb1medico;		
		TreeViewColumn col_nomb2medico;		TreeViewColumn col_appmedico;		
		TreeViewColumn col_apmmedico;		TreeViewColumn col_espemedico;		
		TreeViewColumn col_telmedico;		TreeViewColumn col_cedulamedico;		
		TreeViewColumn col_telOfmedico;		TreeViewColumn col_celmedico;		
		TreeViewColumn col_celmedico2;		TreeViewColumn col_nextelmedico;		
		TreeViewColumn col_beepermedico;	TreeViewColumn col_empresamedico;		
		TreeViewColumn col_estadomedico;		
		
		//Declarando las celdas
		CellRendererText cellr0;			CellRendererText cellrt1;
		CellRendererText cellrt2;			CellRendererText cellrt3;
		CellRendererText cellrt4;			CellRendererText cellrt5;
		CellRendererText cellrt6;			CellRendererText cellrt7;
		CellRendererText cellrt8;			CellRendererText cellrt9;
		CellRendererText cellrt10;			CellRendererText cellrt11;
		CellRendererText cellrt12;			CellRendererText cellrt13;
		CellRendererText cellrt37;
		
		public calendario_quirofano(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = _nombrebd_;
						
			Glade.XML gxml = new Glade.XML (null, "quirofano.glade", "programacion_cirugias", null);
			gxml.Autoconnect (this);        
			
			////// Muestra ventana de Glade
			programacion_cirugias.Show();
			checkbutton_asignar_folio.Sensitive = false;
			entry_circulante1.Sensitive = false;
			entry_circulante2.Sensitive= false;
			entry_internista.Sensitive= false;
			entry_nombre_paciente.Sensitive = false;					
			entry_edad.Sensitive = false;		
			entry_sexo.Sensitive = false;	
			entry_tipo_anestecia.Sensitive = false;
			combobox_sala_1.Sensitive = false;			
			entry_pid_paciente.Sensitive = true;			
			entry_folio_servicio.Sensitive = false;
			entry_pid_paciente.Sensitive = false;
			button_buscar_circulante1.Sensitive = false;
			button_buscar_circulante2.Sensitive = false;
			button_buscar_internista.Sensitive = false;
			entry_paciente_prog.Sensitive = false;
			button_busca_paciente_prog.Sensitive = false;
			button_guardar_prog.Sensitive = false;
			button_cancelar_progra.Sensitive = false;
			this.entry_sexo_prog.Sensitive = false;		
			this.entry_edad_prog.Sensitive = false;
			llena_combobox_sala_2();
			llena_combobox_sala_1();
			
			checkbutton_nueva_cirugia.Clicked += new EventHandler(on_checkbutton_nueva_cirugia_clicked);
			button_buscar_medico.Clicked += new EventHandler(on_button_buscar_medico_clicked);
		    checkbutton_asignar_folio.Clicked += new EventHandler(on_checkbutton_asignar_folio_clicked);
			button_buscar_cirugia.Clicked += new EventHandler(on_button_buscar_cirugia_clicked);
			button_busc_diag.Clicked += new EventHandler(on_button_busc_diag_clicked);			
			button_selec_id.Clicked += new EventHandler(on_selec_id_clicked);
			button_selec_id_diag.Clicked += new EventHandler(on_selec_id_diag_clicked);			
			//button_buscar_programacion.Clicked += new EventHandler(on_button_buscar_programacion_cliked);
			button_guardar_prog.Clicked += new EventHandler(on_button_guardar_prog_clicked);
			button_buscar_cirujano.Clicked += new EventHandler(on_button_buscar_cirujano_clicked);
			button_buscar_neonatologo.Clicked += new EventHandler(on_button_buscar_neonatologo_clicked);
			button_buscar_circulante1.Clicked += new EventHandler(on_button_buscar_circulante1_clicked);
			button_buscar_ayudante.Clicked += new EventHandler(on_button_buscar_ayudante_clicked);
			button_buscar_anestesiologo.Clicked += new EventHandler(on_button_buscar_anestesiologo_clicked);
			button_buscar_circulante2.Clicked += new EventHandler(on_button_buscar_circulante2_clicked);
			button_buscar_internista.Clicked += new EventHandler(on_button_buscar_internista_clicked);
 			button_busca_paciente_prog.Clicked += new EventHandler(on_button_busca_paciente_prog_clicked);
			button_seleccionar_programacion.Clicked += new EventHandler(on_button_seleccionar_programacion_cliked);
			//calendario_quirofano.SelectDay += new EventHandler(on_calendario_quirofano_selectday);			
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			activa_desactiva_entry(false);
		}
			
		/*void on_calendario_quirofano_selectday(object sender, EventArgs args)
		{
		}
		*/
		void on_checkbutton_nueva_cirugia_clicked(object sender, EventArgs args)
		{
			if (checkbutton_nueva_cirugia.Active == false){
				// Buscando el ultimo numero de programacion en quirofano
		 		NpgsqlConnection conexion;
				conexion = new NpgsqlConnection (connectionString+nombrebd );
				// Verifica que la base de datos este conectada		
					activa_desactiva_entry(false);
					this.button_guardar_prog.Sensitive = false;
				    this.entry_dia.Text = " ";
					this.entry_mes.Text = " ";
					this.entry_año.Text = " ";
					this.entry_hora.Text = " ";
					this.entry_anestesiologo.Text = " ";
					this.entry_aseguradora.Text = " ";
					this.entry_ayudante.Text = " ";
					this.entry_circulante1.Text = " ";
					this.entry_circulante2.Text = " ";
					this.entry_cirugia.Text = " ";
					this.entry_cirujano.Text = " ";				
					this.entry_diagnostico.Text = " ";
					this.entry_edad.Text = " ";
					this.entry_edad_prog.Text = " ";
					this.entry_especialidadmed.Text = " ";
					this.entry_folio_servicio.Text = " ";
					this.entry_id_cirugia.Text = " ";
					this.entry_id_diagnostico.Text = " ";				
					this.entry_internista.Text = " ";
					this.entry_medico.Text = " ";
					this.entry_neonatologo.Text = " ";
					this.entry_nombre_paciente.Text = " ";
					this.entry_paciente_prog.Text = " ";
					this.entry_pid_paciente.Text = " ";
					this.entry_sexo.Text = " ";
					this.entry_sexo_prog.Text = " ";
					this.entry_tipo_paciente.Text = " ";
					this.entry_dia2.Text = " ";
					this.entry_mes2.Text = " ";
					this.entry_año2.Text = " ";
					this.entry_hora2.Text = " ";
					this.entry_entrada_sala.Text = " ";
					this.entry_salida_sala.Text = " ";
					this.entry_especialidad_cirugia.Text = " ";
					this.entry_inicio_cirugia.Text = " ";
					this.entry_termino_cirugia.Text = " ";		
					this.entry_notas.Text = " ";
					this.entry_intrs_especial.Text= " ";
					this.entry_tipo_anestecia.Text=" ";
				    this.entry_programacion.Text =" ";
				
			}else{
				this.button_guardar_prog.Sensitive = true;
				NpgsqlConnection conexion;
				conexion = new NpgsqlConnection (connectionString+nombrebd );
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
				
					// asigna el numero de paciente (PID)
					comando.CommandText = "SELECT id_numero_programacion FROM osiris_his_calendario_quirofano ORDER BY id_numero_programacion DESC LIMIT 1;";
					NpgsqlDataReader lector = comando.ExecuteReader ();
						
					if ((bool) lector.Read()){
							ultimaprogramacion = (int) lector["id_numero_programacion"] + 1;
					}else{
							ultimaprogramacion = 1;
					}
					
					//Console.WriteLine(this.ultimaprogramacion.ToString());
					this.entry_programacion.Text = this.ultimaprogramacion.ToString();
					//this.checkbutton_asignar_folio.Sensitive = true;
					activa_desactiva_entry(true);			
					this.entry_dia.Text = DateTime.Now.ToString("dd");
					this.entry_mes.Text = DateTime.Now.ToString("MM");
					this.entry_año.Text = DateTime.Now.ToString("yyyy");
					this.entry_hora.Text = DateTime.Now.ToString("HH:mm");
					
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, 
							ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();						msgBoxError.Destroy();
				}
			}				
		
		}	

		void activa_desactiva_entry(bool activa_desactiva)
		{
			entry_cirugia.Editable = activa_desactiva;	
            entry_id_cirugia.Editable = activa_desactiva;
	        //entry_programacion.Editable = activa_desactiva;
			entry_dia.Editable = activa_desactiva;
			entry_mes.Editable = activa_desactiva;
			entry_año.Editable = activa_desactiva;
			entry_hora.Editable = activa_desactiva;
			entry_medico.Editable = activa_desactiva;
			entry_especialidadmed.Editable = activa_desactiva;
			//entry_paciente_prog.Editable = activa_desactiva;
			entry_sexo_prog.Editable = activa_desactiva;
			entry_edad_prog.Editable = activa_desactiva;
			entry_diagnostico.Editable = activa_desactiva;
			this.combobox_sala_2.Sensitive = activa_desactiva;
			entry_cirujano.Editable = activa_desactiva;
	        entry_ayudante.Editable = activa_desactiva;
	        entry_neonatologo.Editable = activa_desactiva;
	        entry_anestesiologo.Editable = activa_desactiva;
	        entry_circulante1.Editable = activa_desactiva;
			entry_circulante2.Editable = activa_desactiva;
			entry_internista.Editable = activa_desactiva;
			entry_tipo_anestecia.Sensitive = false;			
			button_buscar_cirujano.Sensitive = activa_desactiva;
			button_buscar_neonatologo.Sensitive = activa_desactiva;			
			button_buscar_ayudante.Sensitive = activa_desactiva;
			button_buscar_anestesiologo.Sensitive = activa_desactiva;			
			button_buscar_cirugia.Sensitive = activa_desactiva;
			button_busc_diag.Sensitive = activa_desactiva;
			entry_tipo_paciente.Editable = activa_desactiva;
			entry_aseguradora.Editable = activa_desactiva;
			button_selec_id_diag.Sensitive = activa_desactiva;
			entry_id_diagnostico.Editable = activa_desactiva;
			button_selec_id.Sensitive = activa_desactiva;
			button_buscar_medico.Sensitive = activa_desactiva;
			entry_dia2.Sensitive = activa_desactiva;
			entry_mes2.Sensitive = activa_desactiva;
			entry_año2.Sensitive = activa_desactiva;
			entry_hora2.Sensitive = activa_desactiva;
			//button_busca_paciente_prog.Sensitive = activa_desactiva;
			entry_entrada_sala.Sensitive = false;
			entry_salida_sala.Sensitive = false;
			entry_especialidad_cirugia.Sensitive = false;
			entry_inicio_cirugia.Sensitive = false;
			entry_termino_cirugia.Sensitive = false;		
			entry_notas.Sensitive = false;
			entry_intrs_especial.Sensitive = false;
			entry_tipo_paciente.Editable = activa_desactiva;
			//button_guardar_prog.Sensitive = activa_desactiva;
			button_cancelar_progra.Sensitive = activa_desactiva;
		}
		
		void on_checkbutton_asignar_folio_clicked(object sender, EventArgs args)
		{
			if (checkbutton_asignar_folio.Active == true){
				//Console.WriteLine("Entre aqui y no se porque");
				//llena_valores_programacion();
				this.button_guardar_prog.Sensitive = true;
				entry_nombre_paciente.Sensitive = true;				
				entry_edad.Sensitive = true;
				entry_sexo.Sensitive = true;
				combobox_sala_1.Sensitive = true;
				entry_pid_paciente.Sensitive = true;
				entry_folio_servicio.Sensitive = true;
				entry_circulante1.Sensitive = true;
				entry_circulante2.Sensitive = true;
				entry_internista.Sensitive = true;
				button_buscar_circulante1.Sensitive = true;
				button_buscar_circulante2.Sensitive = true;
				button_buscar_internista.Sensitive = true;			
				entry_cirugia.Sensitive = false;	
	            entry_id_cirugia.Sensitive = false;
		        entry_programacion.Sensitive = false;				
				entry_hora.Sensitive = false;
				entry_medico.Sensitive = false;
				entry_especialidadmed.Sensitive= false;
				entry_paciente_prog.Sensitive = true;
				entry_sexo_prog.Sensitive = false;
				entry_edad_prog.Sensitive = false;
				entry_diagnostico.Sensitive = false;
				this.combobox_sala_2.Sensitive = false;
				entry_cirujano.Sensitive = true;
		        entry_ayudante.Sensitive = true;
		        entry_neonatologo.Sensitive = true;
		        entry_anestesiologo.Sensitive = true;	
				button_buscar_cirujano.Sensitive = true;
				button_buscar_neonatologo.Sensitive = true;				
				button_buscar_ayudante.Sensitive = true;
				button_buscar_anestesiologo.Sensitive = true;			
				button_buscar_cirugia.Sensitive = false;
				button_busc_diag.Sensitive = false;
				entry_tipo_paciente.Sensitive = false;
				entry_aseguradora.Sensitive = false;
				button_selec_id_diag.Sensitive = false;
				entry_id_diagnostico.Sensitive = false;
				button_selec_id.Sensitive = false;
				button_buscar_medico.Sensitive = false;
				entry_dia2.Sensitive = false;
				entry_mes2.Sensitive = false;
				entry_año2.Sensitive = false;
				entry_hora2.Sensitive = false;			
				button_busca_paciente_prog.Sensitive = true;
				entry_entrada_sala.Sensitive = true;
				entry_salida_sala.Sensitive = true;
				entry_especialidad_cirugia.Sensitive = true;
				entry_inicio_cirugia.Sensitive = true;
				entry_termino_cirugia.Sensitive = true;		
				entry_notas.Sensitive = true;
				entry_intrs_especial.Sensitive = true;
				entry_dia.Sensitive = false;
				entry_mes.Sensitive = false;
				entry_año.Sensitive = false;
				entry_hora.Sensitive = false;
				this.entry_sexo_prog.Sensitive = true;		
			    this.entry_edad_prog.Sensitive = true;
				this.entry_tipo_anestecia.Sensitive = true;
			}else{           
				entry_nombre_paciente.Sensitive = false;				
				entry_edad.Sensitive = false;
				entry_sexo.Sensitive = false;
				combobox_sala_1.Sensitive = false;
				entry_pid_paciente.Sensitive = false;
				entry_folio_servicio.Sensitive = false;
				entry_circulante1.Sensitive = false;
				entry_circulante2.Sensitive = false;
				entry_internista.Sensitive = false;
				button_buscar_circulante1.Sensitive = false;
				button_buscar_circulante2.Sensitive = false;
				button_buscar_internista.Sensitive = false;					
				entry_cirugia.Sensitive = true;	
		        entry_id_cirugia.Sensitive = true;
			    entry_programacion.Sensitive = true;				
				entry_hora.Sensitive = true;
				entry_medico.Sensitive = true;
				entry_especialidadmed.Sensitive= true;
				entry_paciente_prog.Sensitive = false;
				entry_sexo_prog.Sensitive = true;
				entry_edad_prog.Sensitive = true;
				entry_diagnostico.Sensitive = true;
				this.combobox_sala_2.Sensitive = true;
				entry_cirujano.Sensitive = true;
			    entry_ayudante.Sensitive = true;
			    entry_neonatologo.Sensitive = true;
			    entry_anestesiologo.Sensitive = true;			
				button_buscar_cirujano.Sensitive = true;
				button_buscar_neonatologo.Sensitive = true;				
				button_buscar_ayudante.Sensitive = true;
				button_buscar_anestesiologo.Sensitive = true;			
				button_buscar_cirugia.Sensitive = true;
				button_busc_diag.Sensitive = true;
				entry_tipo_paciente.Sensitive = true;
				entry_aseguradora.Sensitive = true;
				button_selec_id_diag.Sensitive = true;
				entry_id_diagnostico.Sensitive = true;
				button_selec_id.Sensitive = true;
				button_buscar_medico.Sensitive = true;
				entry_dia2.Sensitive = true;
				entry_mes2.Sensitive = true;
				entry_año2.Sensitive = true;
				entry_hora2.Sensitive = true;
				button_busca_paciente_prog.Sensitive = false;
				entry_entrada_sala.Sensitive = false;
				entry_salida_sala.Sensitive = false;
				entry_especialidad_cirugia.Sensitive = false;
				entry_inicio_cirugia.Sensitive = false;
				entry_termino_cirugia.Sensitive = false;		
				entry_notas.Sensitive = false;
				entry_intrs_especial.Sensitive = false;
				this.entry_sexo_prog.Sensitive = false;		
				this.entry_edad_prog.Sensitive = false;
				this.entry_tipo_anestecia.Sensitive = false;
			}
		}
		// buscar paciente		
		void on_button_busca_paciente_prog_clicked(object sender, EventArgs a)
		{
			busca_pacientes_prog();
		}
		void busca_pacientes_prog()
		{
			Console.WriteLine("Busca Paciente");
			busqueda ="paciente";
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "busca_paciente", null);
			gxml.Autoconnect (this);
        	busca_paciente.Show();
           	
			crea_treeview_busqueda3();
			this.button_nuevo_paciente.Sensitive = false;
            //button_nuevo_paciente.Clicked += new EventHandler(on_button_nuevo_paciente_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_buscar_busqueda.Clicked += new EventHandler(on_buscar_busqueda_clicked);
			if(entry_id_cirugia.Text  == "" || entry_id_cirugia.Text == " "){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close, 
				"Debe de llenar el campo de id cirugia con uno \n"+"existente para que los datos se muestren \n");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}else{
				//Console.WriteLine ("on_selec_id selecciono el ID");
				idtipocirugia = int.Parse(entry_id_cirugia.Text.ToString());
				llenado_de_cirugia(entry_id_cirugia.Text );
			}
			//button_selecciona.Clicked += new EventHandler(on_selecciona_paciente_clicked);
			//entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
		}
		// activa busqueda con boton busqueda
		void on_buscar_busqueda_clicked (object sender, EventArgs a)
		{
			llena_lista_paciente();
		}
		
		void llena_lista_paciente()
		{
			treeViewEngineBusca3.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				entry_expresion.Text = this.entry_expresion.Text.Trim();    	               	
				
				if ((string) entry_expresion.Text.Trim() == ""){
					comando.CommandText = "SELECT osiris_his_paciente.pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
								"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
								"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
								"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
								"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
								"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
								"WHERE alta_paciente = 'false' "+
								"AND pagado = 'false' "+
								"AND cerrado = 'false' "+
								"AND reservacion = 'false' "+
								"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
								"AND osiris_erp_cobros_enca.alta_paciente = false "+
								"AND osiris_erp_cobros_enca.cancelado = false "+
								"AND activo = 'true' ORDER BY osiris_his_paciente.pid_paciente;";
				}else{
					if (radiobutton_busca_apellido.Active == true){
						comando.CommandText = "SELECT osiris_his_paciente.pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
									"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
								"WHERE alta_paciente = 'false' "+
								"AND pagado = 'false' "+
								"AND cerrado = 'false' "+
								"AND reservacion = 'false' "+
								"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
								"AND osiris_erp_cobros_enca.alta_paciente = false "+
								"AND osiris_erp_cobros_enca.cancelado = false "+								
								"AND apellido_paterno_paciente  LIKE '"+entry_expresion.Text.ToUpper()+"%' "+
									"AND activo = 'true'  ORDER BY osiris_his_paciente.pid_paciente;";
									
					}
					if (radiobutton_busca_nombre.Active == true){
						comando.CommandText = "SELECT osiris_his_paciente.pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
									"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
								"WHERE alta_paciente = 'false' "+
								"AND pagado = 'false' "+
								"AND cerrado = 'false' "+
								"AND reservacion = 'false' "+
								"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
								"AND osiris_erp_cobros_enca.alta_paciente = false "+
								"AND osiris_erp_cobros_enca.cancelado = false "+								
								"AND nombre1_paciente  LIKE '"+entry_expresion.Text.ToUpper()+"%' "+
									"AND activo = 'true'  ORDER BY osiris_his_paciente.pid_paciente;";
					}
					if (radiobutton_busca_expediente.Active == true){
						comando.CommandText = "SELECT osiris_his_paciente.pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
									"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
								"WHERE alta_paciente = 'false' "+
								"AND pagado = 'false' "+
								"AND cerrado = 'false' "+
								"AND reservacion = 'false' "+
								"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
								"AND osiris_erp_cobros_enca.alta_paciente = false "+
								"AND osiris_erp_cobros_enca.cancelado = false "+							
								"AND osiris_his_paciente.pid_paciente  LIKE '"+entry_expresion.Text.ToUpper()+"%' "+
									"AND activo = 'true'  ORDER BY osiris_his_paciente.pid_paciente;";					
					}
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//Console.WriteLine(comando.CommandText);
				//string toma_edad;
				while (lector.Read())
				{
					treeViewEngineBusca3.AppendValues ((int) lector["pid_paciente"], 
										(string) lector["nombre1_paciente"],(string) lector["nombre2_paciente"],
										(string) lector["apellido_paterno_paciente"], (string) lector["apellido_materno_paciente"],
										(string) lector["fech_nacimiento"],(string) lector["edad"]+" Años y "+(string) lector["mesesedad"]+" Meses",
										(string) lector["sexo_paciente"],(string) lector["fech_creacion"],
										(string) lector["id_quienlocreo_paciente"]);
				}
				//if(ventana_principal == true) {/*Console.WriteLine("activo boton");*/ button_nuevo_paciente.Sensitive = true;}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();	msgBoxError.Destroy();
            }
            conexion.Close ();
		}
		
		void on_selecciona_paciente_clicked(object sender, EventArgs a)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_Pacientes.Selection.GetSelected(out model, out iterSelected)) {
				
				//id_medico = (int) model.GetValue(iterSelected, 0);
 								
				entry_paciente_prog.Text =(string) model.GetValue(iterSelected, 1)+" "+(string) model.GetValue(iterSelected, 2)+" "+
					(string) model.GetValue(iterSelected, 3)+" "+ (string) model.GetValue(iterSelected, 4);
				entry_edad_prog.Text = (string) model.GetValue(iterSelected, 6);				
				entry_sexo_prog.Text = (string) model.GetValue(iterSelected, 7);
				entry_pid_paciente.Text = Convert.ToString((int) model.GetValue(iterSelected, 0));
			}	        
			// destruye la ventana de busqueda
 			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
 		}
 		
		void crea_treeview_busqueda3()
		{
			treeViewEngineBusca3 = new TreeStore(typeof(int),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(string),typeof(string));
			lista_de_Pacientes.Model = treeViewEngineBusca3;
			
			lista_de_Pacientes.RulesHint = true;
			
			lista_de_Pacientes.RowActivated += on_selecciona_paciente_clicked;  // Doble click selecciono paciente*/
			
			TreeViewColumn col_PidPaciente = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_PidPaciente.Title = "PID Paciente"; // titulo de la cabecera de la columna, si está visible
			col_PidPaciente.PackStart(cellr0, true);
			col_PidPaciente.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
			col_PidPaciente.SortColumnId = (int) Column1.col_PidPaciente;
			//cellr0.Editable = true;   // Permite edita este campo
            
			TreeViewColumn col_Nombre1_Paciente = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_Nombre1_Paciente.Title = "Nombre 1";
			col_Nombre1_Paciente.PackStart(cellrt1, true);
			col_Nombre1_Paciente.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 1 en vez de 2
			col_Nombre1_Paciente.SortColumnId = (int) Column1.col_Nombre1_Paciente;
            
			TreeViewColumn col_Nombre2_Paciente = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_Nombre2_Paciente.Title = "Nombre 2";
			col_Nombre2_Paciente.PackStart(cellrt2, true);
			col_Nombre2_Paciente.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 2 en vez de 3
			col_Nombre2_Paciente.SortColumnId = (int) Column1.col_Nombre2_Paciente;
            
			TreeViewColumn col_app_Paciente = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_app_Paciente.Title = "Apellido Paterno";
			col_app_Paciente.PackStart(cellrt3, true);
			col_app_Paciente.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 3 en vez de 4
			col_app_Paciente.SortColumnId = (int) Column1.col_app_Paciente;
            
			TreeViewColumn col_apm_Paciente = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_apm_Paciente.Title = "Apellido Materno";
			col_apm_Paciente.PackStart(cellrt4, true);
			col_apm_Paciente.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 5 en vez de 6
			col_apm_Paciente.SortColumnId = (int) Column1.col_apm_Paciente;
      
			TreeViewColumn col_fechanacimiento_Paciente = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_fechanacimiento_Paciente.Title = "Fecha Nacimiento";
			col_fechanacimiento_Paciente.PackStart(cellrt5, true);
			col_fechanacimiento_Paciente.AddAttribute (cellrt5, "text", 5);     // la siguiente columna será 6 en vez de 7
			col_fechanacimiento_Paciente.SortColumnId = (int) Column1.col_fechanacimiento_Paciente;
            
			TreeViewColumn col_edad_Paciente = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_edad_Paciente.Title = "Edad";
			col_edad_Paciente.PackStart(cellrt6, true);
			col_edad_Paciente.AddAttribute (cellrt6, "text", 6); // la siguiente columna será 7 en vez de 8
			col_edad_Paciente.SortColumnId = (int) Column1.col_edad_Paciente;
            
			TreeViewColumn col_sexo_Paciente = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_sexo_Paciente.Title = "Sexo";
			col_sexo_Paciente.PackStart(cellrt7, true);
			col_sexo_Paciente.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 8 en vez de 9
			col_sexo_Paciente.SortColumnId = (int) Column1.col_sexo_Paciente;
                        
			TreeViewColumn col_creacion_Paciente = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_creacion_Paciente.Title = "Fecha creacion";
			col_creacion_Paciente.PackStart(cellrt8, true);
			col_creacion_Paciente.AddAttribute (cellrt8, "text", 8); // la siguiente columna será 8 en vez de 9
			col_creacion_Paciente.SortColumnId = (int) Column1.col_creacion_Paciente;
			
			TreeViewColumn col_id_empleado = new TreeViewColumn();
			CellRendererText cellrt9 = new CellRendererText();
			col_id_empleado.Title = "Quien Registro";
			col_id_empleado.PackStart(cellrt9, true);
			col_id_empleado.AddAttribute (cellrt9, "text", 9); // la siguiente columna será 8 en vez de 9
			col_id_empleado.SortColumnId = (int) Column1.col_id_empleado;
                              		
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
		
		enum Column1
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
		
		//cerrar ventana
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		void on_button_buscar_medico_clicked(object sender, EventArgs args)
		{
			buscando_medico(1);
		}
		
		//busqueda de medicos dependiendo especialidad
		void on_button_buscar_cirujano_clicked (object sender, EventArgs args)
		{	
			buscando_medico(2);			
		}
		
		void on_button_buscar_neonatologo_clicked(object sender, EventArgs args)
		{
			buscando_medico(3);
		}
		
		void on_button_buscar_anestesiologo_clicked(object sender, EventArgs args)
		{
			buscando_medico(4);
		}
		
		void on_button_buscar_ayudante_clicked(object sender, EventArgs args)
		{
			buscando_medico(5);
		}
		
		void buscando_medico(int tipo_de_click)
		{
			tipobusqueda_doctor = tipo_de_click;
			
			busqueda = "medicos";
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador_medicos", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        llenado_cmbox_tipo_busqueda();
	        entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_medicos);
			button_selecciona.Clicked += new EventHandler(on_selecciona_medico);
	        button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
			
			treeViewEngineMedicos = new TreeStore( typeof(int),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(bool),typeof(bool),typeof(bool),typeof(bool),
												typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),
												typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),
												typeof(string));
			lista_de_medicos.Model = treeViewEngineMedicos;
			lista_de_medicos.RulesHint = true;
				
			lista_de_medicos.RowActivated += on_selecciona_medico;  // Doble click selecciono paciente*/
				
			col_idmedico = new TreeViewColumn();
			cellr0 = new CellRendererText();
			col_idmedico.Title = "ID Medico"; // titulo de la cabecera de la columna, si está visible
			col_idmedico.PackStart(cellr0, true);
			col_idmedico.AddAttribute (cellr0, "text", 0);
			col_idmedico.SortColumnId = (int) Coldatos_medicos.col_idmedico;    
            
			col_nomb1medico = new TreeViewColumn();
			cellrt1 = new CellRendererText();
			col_nomb1medico.Title = "1º Nombre";
			col_nomb1medico.PackStart(cellrt1, true);
			col_nomb1medico.AddAttribute (cellrt1, "text", 1);
			col_nomb1medico.SortColumnId = (int) Coldatos_medicos.col_nomb1medico; 
            
            col_nomb2medico = new TreeViewColumn();
			cellrt2 = new CellRendererText();
			col_nomb2medico.Title = "2º Nombre";
			col_nomb2medico.PackStart(cellrt2, true);
			col_nomb2medico.AddAttribute (cellrt2, "text", 2);
			col_nomb2medico.SortColumnId = (int) Coldatos_medicos.col_nomb2medico; 
			
			col_appmedico = new TreeViewColumn();
			cellrt3 = new CellRendererText();
			col_appmedico.Title = "Apellido Paterno";
			col_appmedico.PackStart(cellrt3, true);
			col_appmedico.AddAttribute (cellrt3, "text", 3);
			col_appmedico.SortColumnId = (int) Coldatos_medicos.col_appmedico;
			
			col_apmmedico = new TreeViewColumn();
			cellrt4 = new CellRendererText();
			col_apmmedico.Title = "Apellido Materno";
			col_apmmedico.PackStart(cellrt4, true);
			col_apmmedico.AddAttribute (cellrt4, "text", 4);
			col_apmmedico.SortColumnId = (int) Coldatos_medicos.col_apmmedico;
            
			col_espemedico = new TreeViewColumn();
			cellrt5 = new CellRendererText();
			col_espemedico.Title = "Especialidad";
			col_espemedico.PackStart(cellrt5, true);
			col_espemedico.AddAttribute (cellrt5, "text", 5);
			col_espemedico.SortColumnId = (int) Coldatos_medicos.col_espemedico;
            
			col_telmedico = new TreeViewColumn();
			cellrt6 = new CellRendererText();
			col_telmedico.Title = "Cedula Medica";
			col_telmedico.PackStart(cellrt6, true);
			col_telmedico.AddAttribute (cellrt6, "text", 6); 
			col_telmedico.SortColumnId = (int) Coldatos_medicos.col_telmedico;
            
			col_cedulamedico = new TreeViewColumn();
			cellrt7 = new CellRendererText();
			col_cedulamedico.Title = "Telefono Casa";
			col_cedulamedico.PackStart(cellrt7, true);
			col_cedulamedico.AddAttribute (cellrt7, "text", 7); 
			col_cedulamedico.SortColumnId = (int) Coldatos_medicos.col_cedulamedico;
			
			col_telOfmedico = new TreeViewColumn();
			cellrt8 = new CellRendererText();
			col_telOfmedico.Title = "Telefono Oficina";
			col_telOfmedico.PackStart(cellrt8, true);
			col_telOfmedico.AddAttribute (cellrt8, "text", 8);
			col_telOfmedico.SortColumnId = (int) Coldatos_medicos.col_telOfmedico; 
			
			col_celmedico = new TreeViewColumn();
			cellrt9 = new CellRendererText();
			col_celmedico.Title = "Celular 1";
			col_celmedico.PackStart(cellrt9, true);
			col_celmedico.AddAttribute (cellrt9, "text", 9); 
			col_celmedico.SortColumnId = (int) Coldatos_medicos.col_celmedico;
			
			col_celmedico2 = new TreeViewColumn();
			cellrt10 = new CellRendererText();
			col_celmedico2.Title = "Celular 2";
			col_celmedico2.PackStart(cellrt10, true);
			col_celmedico2.AddAttribute (cellrt10, "text", 10);
			col_celmedico2.SortColumnId = (int) Coldatos_medicos.col_celmedico2;
									
			col_nextelmedico = new TreeViewColumn();
			cellrt11 = new CellRendererText();
			col_nextelmedico.Title = "Nextel";
			col_nextelmedico.PackStart(cellrt11, true);
			col_nextelmedico.AddAttribute (cellrt11, "text", 11);
			col_nextelmedico.SortColumnId = (int) Coldatos_medicos.col_nextelmedico;
			
			col_beepermedico = new TreeViewColumn();
			cellrt12 = new CellRendererText();
			col_beepermedico.Title = "Beeper";
			col_beepermedico.PackStart(cellrt12, true);
			col_beepermedico.AddAttribute (cellrt12, "text", 12);
			col_beepermedico.SortColumnId = (int) Coldatos_medicos.col_beepermedico;
			
			col_empresamedico = new TreeViewColumn();
			cellrt13 = new CellRendererText();
			col_empresamedico.Title = "Empresa";
			col_empresamedico.PackStart(cellrt13, true);
			col_empresamedico.AddAttribute (cellrt13, "text", 13);
			col_empresamedico.SortColumnId = (int) Coldatos_medicos.col_empresamedico;
			
			col_estadomedico = new TreeViewColumn();
			cellrt37 = new CellRendererText();
			col_estadomedico.Title = "Estado";
			col_estadomedico.PackStart(cellrt37, true);
			col_estadomedico.AddAttribute (cellrt37, "text", 37);
			col_estadomedico.SortColumnId = (int) Coldatos_medicos.col_estadomedico;
			      
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
			lista_de_medicos.AppendColumn(col_estadomedico);
		}
		
		enum Coldatos_medicos
		{
			col_idmedico,			col_nomb1medico,
			col_nomb2medico,		col_appmedico,
			col_apmmedico,			col_espemedico,
			col_cedulamedico,		col_telmedico,
			col_telOfmedico,		col_celmedico,
			col_celmedico2,			col_nextelmedico,
			col_beepermedico,		col_empresamedico,
			col_estadomedico
		}
		
		
		void on_llena_lista_medicos(object sender, EventArgs args)
		{
			llenando_lista_de_medicos();
		}
		
		void llenando_lista_de_medicos() 
		{
			TreeIter iter;
			if (combobox_tipo_busqueda.GetActiveIter(out iter)){
				if((int) combobox_tipo_busqueda.Model.GetValue(iter,1) > 0) {
					treeViewEngineMedicos.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
		            // Verifica que la base de datos este conectada
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						if ((string) entry_expresion.Text.ToUpper().Trim() == ""){
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
										"ORDER BY id_medico;";
							//Console.WriteLine("cuery tipo de busqueda medico"+comando.CommandText);
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
								  		tipobusqueda+(string) entry_expresion.Text.Trim().ToUpper()+"%' "+
										"ORDER BY id_medico;";
						//Console.WriteLine("cueri 2" +comando.CommandText);
						}
						NpgsqlDataReader lector = comando.ExecuteReader ();
						string estado_medico = "NO AUTORIZADO";
						while (lector.Read())
						{
							if((bool) lector["autorizado"] == false) { estado_medico = "NO AUTORIZADO"; }
							if((bool) lector["autorizado"] == true) { estado_medico = "AUTORIZADO"; }
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
										(bool) lector["autorizado"],//36
										(string) estado_medico);//37
							//cambio los colores de las filas dependiendo si el medico esta activo o no
							col_idmedico.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_fila));			
							col_nomb1medico.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_nomb2medico.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_appmedico.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_apmmedico.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_espemedico.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_telmedico.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_cedulamedico.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_fila));		
							col_telOfmedico.SetCellDataFunc(cellrt8, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_celmedico.SetCellDataFunc(cellrt9, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_celmedico2.SetCellDataFunc(cellrt10, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_nextelmedico.SetCellDataFunc(cellrt11, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_beepermedico.SetCellDataFunc(cellrt12, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_empresamedico.SetCellDataFunc(cellrt13, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_estadomedico.SetCellDataFunc(cellrt37, new Gtk.TreeCellDataFunc(cambia_colores_fila));
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
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((bool)lista_de_medicos.Model.GetValue (iter,34)==true) {
				if ((bool)lista_de_medicos.Model.GetValue (iter,36)==true) {(cell as Gtk.CellRendererText).Foreground = "darkgreen";
				}else{ (cell as Gtk.CellRendererText).Foreground = "black"; }
			}else{	(cell as Gtk.CellRendererText ).Foreground = "red"; }
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
			store1.AppendValues ("ID_MEDICO",7);
				              
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
			if(numbusqueda == 1)  { tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";	}
			if(numbusqueda == 2)  { tipobusqueda = "AND osiris_his_medicos.nombre2_medico LIKE '";	}
			if(numbusqueda == 3)  { tipobusqueda = "AND osiris_his_medicos.apellido_paterno_medico LIKE '";	}
			if(numbusqueda == 4)  { tipobusqueda = "AND osiris_his_medicos.apellido_materno_medico LIKE '";	}
			if(numbusqueda == 5)  { tipobusqueda = "AND osiris_his_medicos.cedula_medico LIKE '";	}
			if(numbusqueda == 6)  { tipobusqueda = "AND osiris_his_tipo_especialidad.descripcion_especialidad LIKE '";	}
			if(numbusqueda == 7)  { tipobusqueda = "AND osiris_his_medicos.id_medico LIKE '";	}
		}
		
		void on_selecciona_medico(object sender, EventArgs args) 
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_medicos.Selection.GetSelected(out model, out iterSelected)) 
 			{
				if (tipobusqueda_doctor ==1)
				{
			    id_medico = (int) model.GetValue(iterSelected, 0);
 				//entry_medico.Text = nombmedico;				
				nombmedico = (string) model.GetValue(iterSelected, 1)+" "+(string) model.GetValue(iterSelected, 2)+" "+
					(string) model.GetValue(iterSelected, 3)+" "+ (string) model.GetValue(iterSelected, 4);
				especialidadmed = (string) model.GetValue(iterSelected, 5);
				entry_medico.Text = nombmedico.ToString();
				entry_especialidadmed.Text = especialidadmed.ToString();
				//Console.WriteLine (especialidadmed);
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
			
			if (tipobusqueda_doctor == 2)
			{
 				id_cirujano = (int) model.GetValue(iterSelected, 0);
				//entry_cirujano.Text = nombmedico;				
				nombmedico = (string) model.GetValue(iterSelected, 1)+" "+(string) model.GetValue(iterSelected, 2)+" "+
					(string) model.GetValue(iterSelected, 3)+" "+ (string) model.GetValue(iterSelected, 4);	
				entry_cirujano.Text = nombmedico.ToString();				
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
			if	(tipobusqueda_doctor == 3)
			{
				id_neonatologo = (int) model.GetValue(iterSelected, 0);
								
				nombmedico = (string) model.GetValue(iterSelected, 1)+" "+(string) model.GetValue(iterSelected, 2)+" "+
					(string) model.GetValue(iterSelected, 3)+" "+ (string) model.GetValue(iterSelected, 4);	
				entry_neonatologo.Text = nombmedico.ToString();				
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
			
			if (tipobusqueda_doctor == 4)
			{
 				id_anestesiologo = (int) model.GetValue(iterSelected, 0);								
				nombanestesiologo = (string) model.GetValue(iterSelected, 1)+" "+(string) model.GetValue(iterSelected, 2)+" "+
					(string) model.GetValue(iterSelected, 3)+" "+ (string) model.GetValue(iterSelected, 4);	
				entry_anestesiologo.Text = nombanestesiologo.ToString();				
				//Console.WriteLine (nombanestesiologo);
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
			if (tipobusqueda_doctor == 5)
			{
 				id_ayudante = (int) model.GetValue(iterSelected, 0);								
				nombmedico = (string) model.GetValue(iterSelected, 1)+" "+(string) model.GetValue(iterSelected, 2)+" "+
					(string) model.GetValue(iterSelected, 3)+" "+ (string) model.GetValue(iterSelected, 4);	
				entry_ayudante.Text = nombmedico.ToString();				
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
			}
			
		}
		
		
		void on_button_buscar_especialidad_clicked(object sender, EventArgs args)
		{
			busqueda = "especialidad";
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_especialidad);
			button_selecciona.Clicked += new EventHandler(on_selecciona_especialidad);
	        button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
			
			treeViewEngineEspecialidad = new TreeStore( typeof(int),typeof(string));
								
			lista_de_busqueda.Model = treeViewEngineEspecialidad;
			lista_de_busqueda.RulesHint = true;
				
			lista_de_busqueda.RowActivated += on_selecciona_especialidad;  // Doble click selecciono paciente*/
				
			TreeViewColumn col_idespecialidad = new TreeViewColumn();
			cellr0 = new CellRendererText();
			col_idespecialidad.Title = "ID Especialidad"; // titulo de la cabecera de la columna, si está visible
			col_idespecialidad.PackStart(cellr0, true);
			col_idespecialidad.AddAttribute (cellr0, "text", 0);
			col_idespecialidad.SortColumnId = (int) Column_especialidad.col_idespecialidad;   
			
			TreeViewColumn col_especialidad = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_especialidad.Title = "Especialidad";
			col_especialidad.PackStart(cellr1,true);
			col_especialidad.AddAttribute(cellr1, "text", 1);
			col_especialidad.SortColumnId = (int) Column_especialidad.col_especialidad;
			
			lista_de_busqueda.AppendColumn(col_idespecialidad);
			lista_de_busqueda.AppendColumn(col_especialidad);
		}
		
		enum Column_especialidad {	col_idespecialidad,	col_especialidad	}
		
		void on_llena_lista_especialidad(object sender, EventArgs args)
		{
			llenando_lista_de_especialidad();
		}
		
		void llenando_lista_de_especialidad()
		{
			//TreeIter iter;
			treeViewEngineEspecialidad.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				if ((string) entry_expresion.Text.ToUpper().Trim() == "")
				{
					comando.CommandText = "SELECT id_especialidad,descripcion_especialidad "+
										" FROM osiris_his_tipo_especialidad "+
										"ORDER BY id_especialidad;";
				}else{
					comando.CommandText = "SELECT id_especialidad,descripcion_especialidad "+
										" FROM osiris_his_tipo_especialidad "+
										"WHERE descripcion_especialidad LIKE '"+entry_expresion.Text.ToUpper().Trim()+"%' ;"+
										"ORDER BY id_especialidad;";
				}						
					
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read())
				{
					treeViewEngineEspecialidad.AppendValues ((int) lector["id_especialidad"],(string) lector["descripcion_especialidad"]);
				}
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_selecciona_especialidad (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			
			if (lista_de_busqueda.Selection.GetSelected(out model, out iterSelected)) 
 			{
 				id_esp_medico  = (int) model.GetValue(iterSelected, 0);
 				entry_especialidadmed.Text = (string) model.GetValue(iterSelected, 1);
				//Widget win = (Widget) sender;
				//win.Toplevel.Destroy();
			}
		}
		
		void on_button_buscar_cirugia_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "busca_cirugias", null);
			gxml.Autoconnect (this);
			tipobusqueda ="cirugia";
			// Activa la salida de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			// Activa la seleccion de cirugia
			button_selecciona.Clicked += new EventHandler(on_selecciona_cirugia_clicked);
			// Activa boton de busqueda
			button_llena_cirugias.Clicked += new EventHandler(on_button_llena_cirugias_clicked);
	        
			treeViewEngineBusca = new TreeStore( typeof(int), typeof(string),typeof(string),typeof(string));
			lista_cirugia.Model = treeViewEngineBusca;
			
			lista_cirugia.RulesHint = true;
			
			lista_cirugia.RowActivated += on_selecciona_cirugia_clicked;  // Doble click selecciono paciente
			
			TreeViewColumn col_idcirugia = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idcirugia.Title = "ID Cirugia"; // titulo de la cabecera de la columna, si está visible
			col_idcirugia.PackStart(cellr0, true);
			col_idcirugia.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
			
			TreeViewColumn col_descripcirugia = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_descripcirugia.Title = "Descripcion de Cirugia";
			col_descripcirugia.PackStart(cellrt1, true);
			col_descripcirugia.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 1 en vez de 2
			
			TreeViewColumn col_tienepaquete = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_tienepaquete.Title = "Paquete";
			col_tienepaquete.PackStart(cellrt2, true);
			col_tienepaquete.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
			
			TreeViewColumn col_preciobase = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_preciobase.Title = "P. Base";
			col_preciobase.PackStart(cellrt3, true);
			col_preciobase.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 1 en vez de 2
			
			lista_cirugia.AppendColumn(col_idcirugia);
			lista_cirugia.AppendColumn(col_descripcirugia);
			lista_cirugia.AppendColumn(col_tienepaquete);
			lista_cirugia.AppendColumn(col_preciobase);
		}
			void on_selecciona_cirugia_clicked (object sender, EventArgs args)
		{
			TreeModel model;			TreeIter iterSelected;
			if (lista_cirugia.Selection.GetSelected(out model, out iterSelected)) {
				if(tipobusqueda == "cirugia")  {
					idtipocirugia = (int) model.GetValue(iterSelected, 0);
					cirugia = (string) model.GetValue(iterSelected, 1);
					entry_id_cirugia.Text = idtipocirugia.ToString(); 
					entry_cirugia.Text = cirugia;
					llenado_de_cirugia(idtipocirugia.ToString());
					// cierra la ventana despues que almaceno la informacion en variables
					Widget win = (Widget) sender;
					win.Toplevel.Destroy();
				}
				if(tipobusqueda == "especialidad") {
					idtipoesp  = (int) model.GetValue(iterSelected, 0);
					especialidad = (string) model.GetValue(iterSelected, 1);
					entry_id_especialidad.Text = idtipoesp.ToString(); 
					entry_descripcion_especialidad.Text = especialidad;
					// cierra la ventana despues que almaceno la informacion en variables
					Widget win = (Widget) sender;
					win.Toplevel.Destroy();
				}
		}
		
		}
			void on_button_llena_cirugias_clicked(object sender, EventArgs args)
		{
			llena_lista_de_busqueda();
		}
		
		void llena_lista_de_busqueda() 
		{
			 if(tipobusqueda == "cirugia") {
				treeViewEngineBusca.Clear();// Limpia el treeview cuando realiza una nueva busqueda
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
           		// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
	              	if ((string) entry_expresion.Text.ToUpper() == "*")	{
						comando.CommandText ="SELECT id_tipo_cirugia,descripcion_cirugia,tiene_paquete,to_char(valor_paquete,'999999999.99') AS valorpaquete "+
											"FROM osiris_his_tipo_cirugias "+
											"ORDER BY id_tipo_cirugia;";
					}else{
						comando.CommandText ="SELECT id_tipo_cirugia,descripcion_cirugia,tiene_paquete,to_char(valor_paquete,'999999999.99') AS valorpaquete FROM osiris_his_tipo_cirugias "+
											"WHERE descripcion_cirugia LIKE '%"+entry_expresion.Text.ToUpper()+"%' "+
											"ORDER BY id_tipo_cirugia;";
					}
					NpgsqlDataReader lector = comando.ExecuteReader ();
					string paquete_sino = "";
					while (lector.Read()){
						if((bool) lector["tiene_paquete"]){
							paquete_sino = "ES PAQUETE";
						}else{
							paquete_sino = "";
						}
						treeViewEngineBusca.AppendValues ((int) lector["id_tipo_cirugia"],
									(string) lector["descripcion_cirugia"],
									paquete_sino,
									(string) lector["valorpaquete"]
									);//TreeIter iter =
					}
				}catch (NpgsqlException ex){
	   					Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
	   						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
						msgBoxError.Run ();		msgBoxError.Destroy();
				}
				conexion.Close ();
			}
			if(tipobusqueda =="especialidad") {
				treeViewEngineBusca.Clear();// Limpia el treeview cuando realiza una nueva busqueda
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
           		// Verifica que la base de datos este conectada
				try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
		              	if ((string) entry_expresion.Text.ToUpper() == "*")	{
							comando.CommandText ="SELECT * FROM osiris_his_tipo_especialidad "+
												" ORDER BY id_especialidad;";
						}else{
							comando.CommandText ="SELECT * FROM osiris_his_tipo_especialidad "+
												"WHERE descripcion_especialidad LIKE '%"+entry_expresion.Text.ToUpper()+"%' "+
												" ORDER BY id_especialidad;";
						}
						NpgsqlDataReader lector = comando.ExecuteReader ();
						while (lector.Read())	{
							treeViewEngineBusca.AppendValues ((int) lector["id_especialidad"],(string) lector["descripcion_especialidad"]);//TreeIter iter =
						}
					}catch (NpgsqlException ex){
		   					Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
							msgBoxError.Run ();		msgBoxError.Destroy();
					}
				conexion.Close ();
			}
			if(tipobusqueda =="medico") {
				treeViewEngineBusca.Clear();// Limpia el treeview cuando realiza una nueva busqueda
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
           		// Verifica que la base de datos este conectada
				try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
		              	if ((string) entry_expresion.Text.ToUpper() == "*")	{
							comando.CommandText ="SELECT * FROM osiris_his_medicos "+
												" ORDER BY id_medico;";
						}else{
							comando.CommandText ="SELECT * FROM osiris_his_medicos "+
												"WHERE nombre_medico LIKE '%"+entry_expresion.Text.ToUpper()+"%' "+
												" ORDER BY id_medico;";
						}
						NpgsqlDataReader lector = comando.ExecuteReader ();
						while (lector.Read())	{
							treeViewEngineBusca.AppendValues ((int) lector["id_medico"],(string) lector["nombre_medico"]);//TreeIter iter =
						}
					}catch (NpgsqlException ex){
		   					Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
							msgBoxError.Run ();		msgBoxError.Destroy();
					}
				conexion.Close ();
			}
		}
			
		void on_selec_id_clicked(object sender, EventArgs args)
		{
			if(entry_id_cirugia.Text  == "" || entry_id_cirugia.Text == " "){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close, 
				"Debe de llenar el campo de id cirugia con uno \n"+"existente para que los datos se muestren \n");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}else{
				//Console.WriteLine ("on_selec_id selecciono el ID");
				idtipocirugia = int.Parse(entry_id_cirugia.Text.ToString());
				llenado_de_cirugia(entry_id_cirugia.Text );
			}
		}
		///////////////Busqueda de diagnostico Final//////////////////////////////
		void on_button_busc_diag_final_clicked (object sender, EventArgs args)
		{
			busqueda = "diagnostico";
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda_diag("diagnostico");
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_diag_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enterbucar_busqueda;
			button_selecciona.Clicked += new EventHandler(on_selecciona_diag_clicked);
								
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta la final de la classe*/
		}
		
		///////////////Busqueda de diagnostico CIE-10//////////////////////////////	      
		void on_button_busc_diag_clicked(object sender, EventArgs args)
		{
			busqueda = "diagnostico";
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda_diag("diagnostico");
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_diag_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enterbucar_busqueda;
			button_selecciona.Clicked += new EventHandler(on_selecciona_diag_clicked);
			radiobutton_nombre.Hide();
		    radiobutton_codigo.Hide();
		
								
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta la final de la classe*/
		}
		
		
		void on_llena_lista_diag_clicked (object sender, EventArgs args)
		{
			if(this.entry_expresion.Text.Trim() != ""){
			llenando_lista_de_productos();
			}else{
			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Info,ButtonsType.Close, " Favor de escribir el Diagnostico a Buscar ");
					msgBoxError.Run ();			msgBoxError.Destroy();
			}
		}
		void crea_treeview_busqueda_diag(string tipo_busqueda)
		{
			if (tipo_busqueda == "diagnostico"){
				treeViewEngineBusca2 = new TreeStore(typeof(string),
				                                         typeof(string),
														 typeof(string),
														 //typeof(string),
														 typeof(string));
				                                     
                lista_de_producto.Model = treeViewEngineBusca2;
				
				lista_de_producto.RulesHint = true;
				
				lista_de_producto.RowActivated += on_selecciona_diag_clicked;  // Doble click selecciono paciente
				
				TreeViewColumn col_iddiagnostico = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_iddiagnostico.Title = "ID Diagnostico"; // titulo de la cabecera de la columna, si está visible
				col_iddiagnostico.PackStart(cellr0, true);
				col_iddiagnostico.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
				//col_iddiagnostico.SortColumnId = (int) Column_prod.col_iddiagnostico;
			
				TreeViewColumn col_desc_diagnostico = new TreeViewColumn();
				CellRendererText cellr1 = new CellRendererText();
				col_desc_diagnostico.Title = "Descripcion Diagnostico"; // titulo de la cabecera de la columna, si está visible
				col_desc_diagnostico.PackStart(cellr1, true);
				col_desc_diagnostico.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
				//col_desc_diagnostico.SortColumnId = (int) Column_prod.col_desc_diagnostico;
				
	       
				TreeViewColumn col_id_cie_10 = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_id_cie_10.Title = "ID CIE-10";
				col_id_cie_10.PackStart(cellrt2, true);
				col_id_cie_10.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
				//col_id_cie_10.SortColumnId = (int) Column_prod.col_id_cie_10;
	       
				TreeViewColumn col_id_cie_10_grupo = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_id_cie_10_grupo.Title = "ID CIE-10 Grupo";
				col_id_cie_10_grupo.PackStart(cellrt3, true);
				col_id_cie_10_grupo.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 2 en vez de 3
				//col_id_cie_10_grupo.SortColumnId = (int) Column_prod.col_id_cie_10_grupo;
					
			
				lista_de_producto.AppendColumn(col_iddiagnostico);
				lista_de_producto.AppendColumn(col_desc_diagnostico);
				lista_de_producto.AppendColumn(col_id_cie_10);
				lista_de_producto.AppendColumn(col_id_cie_10_grupo);
				
			}
		}
		void llenando_lista_de_productos()			
		{   
			treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			//Verifica que la base de datos este conectada
			string query_tipo_busqueda = "";
							           
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(osiris_his_tipo_diagnosticos.id_diagnostico,'999999999999') AS iddiagnostico,"+
					                  "osiris_his_tipo_diagnosticos.descripcion_diagnostico,"+
                                      "osiris_his_tipo_diagnosticos.id_cie_10,"+
                                      "to_char(osiris_his_tipo_diagnosticos.id_cie_10_grupo,'999999999999') AS idcie10grupo,"+
                                      "osiris_his_tipo_diagnosticos.sub_grupo "+
                                      "FROM osiris_his_tipo_diagnosticos "+
                                      "WHERE osiris_his_tipo_diagnosticos.sub_grupo = 'false' "+
                                       query_tipo_busqueda;
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
				        treeViewEngineBusca2.AppendValues (
											(string) lector["iddiagnostico"],
											(string) lector["descripcion_diagnostico"],
				                            (string) lector["id_cie_10"],
											(string) lector["idcie10grupo"]);
											//(string) lector["sub_grupo"]);
											
				}
			}catch (NpgsqlException ex){
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
									ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();
                                      
       }
		
	 	[GLib.ConnectBefore ()] 
		public void onKeyPressEvent_enterbucar_busqueda(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				llenando_lista_de_productos();
				//Console.WriteLine ("key press");
								
			}
		}
		
		
		void on_selecciona_diag_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;

			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
					this.entry_diagnostico.Text = (string) model.GetValue(iterSelected, 1);
		 			iddiagnosticocie10 = int.Parse((string) model.GetValue(iterSelected, 0));
								    
					diagnostico = (string) model.GetValue(iterSelected, 1);
					entry_id_diagnostico.Text = iddiagnosticocie10.ToString(); 
					entry_diagnostico.Text = diagnostico;
			}else{
 				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, 
							ButtonsType.Close, "NO existen diagnosticos para seleccionar");
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			Widget win =(Widget) sender;
			win.Toplevel.Destroy();
		}
		void on_selec_id_diag_clicked(object sender, EventArgs args)
		{
			if(entry_id_diagnostico.Text  == "" || entry_id_diagnostico.Text == " "){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close, 
				"Debe de llenar el campo de id diagnostico con uno \n"+"existente para que los datos se muestren \n");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}else{
				//Console.WriteLine ("on_selec_id_diag selecciono el ID");
				iddiagnostico = int.Parse(entry_id_diagnostico.Text.ToString());
				llenado_de_diagnostico(entry_id_diagnostico.Text );
			}
		}
		
		void llenado_de_diagnostico(string iddiagnostico)
		{
					
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	           
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
	              	
				comando.CommandText = "SELECT to_char(osiris_his_tipo_diagnosticos.id_diagnostico,'999999999999') AS iddiagnostico,"+
					                  "osiris_his_tipo_diagnosticos.descripcion_diagnostico,"+
                                      "osiris_his_tipo_diagnosticos.id_cie_10,"+
                                      "to_char(osiris_his_tipo_diagnosticos.id_cie_10_grupo,'999999999999') AS idcie10grupo,"+
                                      "osiris_his_tipo_diagnosticos.sub_grupo "+
                                      "FROM osiris_his_tipo_diagnosticos "+
                                      "WHERE osiris_his_tipo_diagnosticos.sub_grupo = 'false'"+
						              "AND osiris_his_tipo_diagnosticos.id_diagnostico = '"+entry_id_diagnostico.Text+"' ;";
                //Console.WriteLine(comando.CommandText.ToString());				                       							
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if(lector.Read())
				{
					entry_diagnostico.Text = (string) lector["descripcion_diagnostico"];
								
	           	}else{
	           		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "NO existe Diagnostico, Verifique...");
					msgBoxError.Run (); 	msgBoxError.Destroy();
	           	}
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run (); 	msgBoxError.Destroy();
		    }
	       	conexion.Close ();
       	}
		
		// Este toma los valores para llenar el encabezado del procedimiento
		// Aqui lleno el detalle de los servicios que se va aplicar para su cobro
		void llenado_de_cirugia(string idcirugia)
		{
					
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	           
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
	              	
				comando.CommandText = "SELECT osiris_his_tipo_cirugias.id_tipo_cirugia, "+
									"descripcion_cirugia,descripcion_especialidad, "+
									"to_char(precio_de_venta,'99999999') AS precioventa, "+
									"to_char(deposito_minimo,'99999999') AS depominimo, "+
									"to_char(dias_internamiento,'99999999') AS diasinternamiento, "+
									"to_char(osiris_his_tipo_cirugias.id_especialidad,'999999') AS idespecialidad  "+
									"FROM osiris_his_tipo_cirugias,osiris_his_tipo_especialidad  "+
					            	"WHERE osiris_his_tipo_cirugias.id_especialidad = osiris_his_tipo_especialidad.id_especialidad  "+
					            	"AND osiris_his_tipo_cirugias.id_tipo_cirugia = '"+(string)  idcirugia.ToString()+"' ;";
				//Console.WriteLine("query llenado cirugia: "+comando.CommandText.ToString());				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if(lector.Read())
				{
					idtipocirugia = int.Parse(idcirugia);
					entry_cirugia.Text = (string) lector["descripcion_cirugia"];
					
	           	}else{
	           		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "NO existe TAL paquete");
					msgBoxError.Run (); 	msgBoxError.Destroy();
	           	}
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run (); 	msgBoxError.Destroy();
		    }
	       	conexion.Close ();
       	}				
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;		
				if(busqueda == "medicos") { llenando_lista_de_medicos(); }
				if(busqueda == "especialidad") {llenando_lista_de_especialidad();}
						
			}
		}
		//void buscar_empleado; 
		//busqueda de medicos dependiendo especialidad
		void on_button_buscar_circulante1_clicked (object sender, EventArgs args)
		{	
			buscando_empleado(1);			
		}
		
		void on_button_buscar_circulante2_clicked(object sender, EventArgs args)
		{
			buscando_empleado(2);
		}
		void on_button_buscar_internista_clicked(object sender, EventArgs args)
		{
			buscando_empleado(3);
		}
		
		void buscando_empleado(int tipo_de_click)
		{
			tipobusqueda_empleado = tipo_de_click;
		
			Glade.XML gxml = new Glade.XML (null, "recursos_humanos.glade", "busca_empleado", null);
			gxml.Autoconnect (this);
			busca_empleado.Show();
			
			crea_treeview_busqueda();
	        button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
			button_buscar_emp.Clicked += new EventHandler(on_buscar_busquedaemp_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_empleado_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enterbucar_empleado;
	
		}
	
		void on_buscar_busquedaemp_clicked (object sender, EventArgs a)
		{
			llena_lista_empleados();
		}
		
		[GLib.ConnectBefore ()] 
		public void onKeyPressEvent_enterbucar_empleado(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				llena_lista_empleados();
				//Console.WriteLine ("key press");							
			}
		}
		
		void llena_lista_empleados()
		{
		
			treeViewEngineBusca.Clear();
						
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				entry_expresion.Text = this.entry_expresion.Text.Trim();    	               	
				
				if ((string) entry_expresion.Text.Trim() == ""){
					comando.CommandText = "SELECT osiris_empleado.id_empleado,nombre1_empleado,nombre2_empleado, "+
									  "apellido_paterno_empleado, "+
									  "apellido_materno_empleado, estado_del_empleado "+
								"FROM osiris_empleado, osiris_empleado_detalle,osiris_erp_puestos " +
								"WHERE osiris_empleado.id_empleado = osiris_empleado_detalle.id_empleado "+ 
								"AND osiris_empleado_detalle.id_puesto = osiris_erp_puestos.id_puesto "+ 
								"AND osiris_erp_puestos.id_puesto IN('20','21') "+
								"AND accseso_quirofano = 'true';";
									  				
				}else{
					if (radiobutton_busca_apellido.Active == true){
						comando.CommandText = "SELECT osiris_empleado.id_empleado,nombre1_empleado,nombre2_empleado, "+
									  "apellido_paterno_empleado, "+
									  "apellido_materno_empleado,estado_del_empleado "+
									  "FROM osiris_empleado,osiris_empleado_detalle,osiris_erp_puestos " +
									  "WHERE osiris_empleado.id_empleado = osiris_empleado_detalle.id_empleado "+ 
								"AND osiris_empleado_detalle.id_puesto = osiris_erp_puestos.id_puesto "+ 
								"AND osiris_erp_puestos.id_puesto IN('20','21') "+
								"AND accseso_quirofano = 'true' "+
								 "AND (apellido_paterno_empleado LIKE '"+entry_expresion.Text.ToUpper()+"%' OR apellido_materno_empleado LIKE '"+entry_expresion.Text.ToUpper()+"%') "+
									  "ORDER BY id_empleado;";
					}
					if (radiobutton_busca_nombre.Active == true){
						comando.CommandText = "SELECT osiris_empleado.id_empleado,nombre1_empleado,nombre2_empleado, "+
									  "apellido_paterno_empleado, "+
									  "apellido_materno_empleado,estado_del_empleado "+
									   "FROM osiris_empleado, osiris_empleado_detalle,osiris_erp_puestos " +
									  "WHERE osiris_empleado.id_empleado = osiris_empleado_detalle.id_empleado "+ 
								"AND osiris_empleado_detalle.id_puesto = osiris_erp_puestos.id_puesto "+ 
								"AND osiris_erp_puestos.id_puesto IN('20','21') "+
								"AND accseso_quirofano = 'true' "+
									"AND (nombre1_empleado LIKE '"+entry_expresion.Text.ToUpper()+"%' OR nombre2_empleado LIKE '"+entry_expresion.Text.ToUpper()+"%') "+
									  "ORDER BY id_empleado;";
					}
					if (radiobutton_busca_numero.Active == true){
					//Console.WriteLine ("Radio jala");
						comando.CommandText = "SELECT osiris_empleado.id_empleado,nombre1_empleado,nombre2_empleado, "+
									  "apellido_paterno_empleado, "+
									  "apellido_materno_empleado,estado_del_empleado "+
									 "FROM osiris_empleado, osiris_empleado_detalle,osiris_erp_puestos " +
									  "WHERE osiris_empleado.id_empleado = osiris_empleado_detalle.id_empleado "+ 
								"AND osiris_empleado_detalle.id_puesto = osiris_erp_puestos.id_puesto "+ 
								"AND osiris_erp_puestos.id_puesto IN('20','21') "+
								"AND accseso_quirofano = 'true' "+ 
									"AND (id_empleado LIKE '"+entry_expresion.Text.ToUpper()+"%') "+
									  "ORDER BY id_empleado;";				
					}
				}
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//string toma_edad;
				while (lector.Read()){
					treeViewEngineBusca.AppendValues ((string) lector["id_empleado"], 
										(string) lector["nombre1_empleado"],(string) lector["nombre2_empleado"],
										(string) lector["apellido_paterno_empleado"], (string) lector["apellido_materno_empleado"]);
				}
				//if(ventana_principal == true) {/*Console.WriteLine("activo boton"); button_nuevo_paciente.Sensitive = true;}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();	msgBoxError.Destroy();
            }
            conexion.Close ();
		}
	
		void crea_treeview_busqueda()
		{
			treeViewEngineBusca = new TreeStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(string));
			lista_de_empleados.Model = treeViewEngineBusca;
			
			lista_de_empleados.RulesHint = true;
			
			lista_de_empleados.RowActivated += on_selecciona_empleado_clicked;  // Doble click selecciono paciente
			
			TreeViewColumn col_idEmpleado = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idEmpleado.Title = "ID Empleado"; // titulo de la cabecera de la columna, si está visible
			col_idEmpleado.PackStart(cellr0, true);
			col_idEmpleado.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
			col_idEmpleado.SortColumnId = (int) Column.col_idEmpleado;
			//cellr0.Editable = true;   // Permite edita este campo
            
			TreeViewColumn col_Nombre1_Empleado = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_Nombre1_Empleado.Title = "Nombre 1";
			col_Nombre1_Empleado.PackStart(cellrt1, true);
			col_Nombre1_Empleado.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 1 en vez de 2
			col_Nombre1_Empleado.SortColumnId = (int) Column.col_Nombre1_Empleado;
            
			TreeViewColumn col_Nombre2_Empleado = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_Nombre2_Empleado.Title = "Nombre 2";
			col_Nombre2_Empleado.PackStart(cellrt2, true);
			col_Nombre2_Empleado.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 2 en vez de 3
			col_Nombre2_Empleado.SortColumnId = (int) Column.col_Nombre2_Empleado;
            
			TreeViewColumn col_app_Empleado = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_app_Empleado.Title = "Apellido Paterno";
			col_app_Empleado.PackStart(cellrt3, true);
			col_app_Empleado.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 3 en vez de 4
			col_app_Empleado.SortColumnId = (int) Column.col_app_Empleado;
            
			TreeViewColumn col_apm_Empleado = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_apm_Empleado.Title = "Apellido Materno";
			col_apm_Empleado.PackStart(cellrt4, true);
			col_apm_Empleado.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 5 en vez de 6
			col_apm_Empleado.SortColumnId = (int) Column.col_apm_Empleado;
      
			                              		
			lista_de_empleados.AppendColumn(col_idEmpleado);
			lista_de_empleados.AppendColumn(col_Nombre1_Empleado);
			lista_de_empleados.AppendColumn(col_Nombre2_Empleado);
			lista_de_empleados.AppendColumn(col_app_Empleado);
			lista_de_empleados.AppendColumn(col_apm_Empleado);
			
		}
		
		enum Column
		{
			col_idEmpleado,
			col_Nombre1_Empleado,
			col_Nombre2_Empleado,
			col_app_Empleado,
			col_apm_Empleado,
			
		}
		
		void on_selecciona_empleado_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_empleados.Selection.GetSelected(out model, out iterSelected)) 
			{			
				//id_seleccion = (int) model.GetValue(iterSelected, 0);
				selcampo = (string) model.GetValue(iterSelected, 1)+" "+(string)model.GetValue(iterSelected,2)+" "+(string)model.GetValue(iterSelected,3);
				//Console.WriteLine (selcampo);
				busca_empleado.Destroy();
								
		    }
		if (tipobusqueda_empleado == 1)
			{
 				id_circulante1 = id_seleccion;
				selcampo = (string) model.GetValue(iterSelected, 1)+" "+(string)model.GetValue(iterSelected,2)+" "+(string)model.GetValue(iterSelected,3);
				Console.WriteLine (selcampo);
				busca_empleado.Destroy();
				entry_circulante1.Text = selcampo;					
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
			if	(tipobusqueda_empleado == 2)
			{
				id_circulante2 = id_seleccion;
				selcampo = (string) model.GetValue(iterSelected, 1)+" "+(string)model.GetValue(iterSelected,2)+" "+(string)model.GetValue(iterSelected,3);
				Console.WriteLine (selcampo);
				busca_empleado.Destroy();
				entry_circulante2.Text = selcampo;				
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
			
			if (tipobusqueda_empleado == 3)
			{
 				id_internista = id_seleccion;
				selcampo = (string) model.GetValue(iterSelected, 1)+" "+(string)model.GetValue(iterSelected,2)+" "+(string)model.GetValue(iterSelected,3);
				//Console.WriteLine (selcampo);
				busca_empleado.Destroy();
				entry_internista.Text = selcampo;					
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			
			}
		}
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEventcodigopostal(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮ";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace  && args.Event.Key != Gdk.Key.Delete)
			{
				args.RetVal = true;
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEventactual(object o, Gtk.KeyPressEventArgs args)
		{
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮ";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace  && args.Event.Key != Gdk.Key.Delete)
			{
				args.RetVal = true;
			}
		}

		// controla la tecla ENTRER para buscar
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_empleado(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				//llenado_de_factura();
				//if (checkbutton_empleado_nuevo.Active == true)
					//{checkbutton_empleado_nuevo.Active = false;}
				//llena_empleado();
				//protegecampos();
			// actrivar sensitive de editar, no funciona dentro de llena_empleado
			
			}
			string misDigitos = "0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮ)(ｔｒｓｑnpexTNPEXTt";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace && args.Event.Key != Gdk.Key.Delete)
			{
				args.RetVal = true;
			}
		}
		void llena_combobox_sala_2()
		{
			//COMBO SALA 2
			combobox_sala_2.Clear();
			CellRendererText cell1= new CellRendererText();
			combobox_sala_2.PackStart(cell1, true);
			combobox_sala_2.AddAttribute(cell1,"text",0);
   
			ListStore store2 = new ListStore(typeof(string),typeof(int));
			combobox_sala_2.Model = store2;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_his_habitaciones "+
               						"WHERE id_tipo_admisiones = '700' ORDER BY id_habitacion;";
               						
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read())
				{
					store2.AppendValues ((string) lector["descripcion_cuarto"]+
					                     Convert.ToString((int) lector["numero_cuarto"]),"-", (int) lector["id_habitacion"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
					
			TreeIter iter1;
			if (store2.GetIterFirst(out iter1)){
				combobox_sala_2.SetActiveIter (iter1);
				
			}
			combobox_sala_2.Changed += new EventHandler (onComboBoxChanged_sala_2);
		}
		
		void onComboBoxChanged_sala_2 (object sender, EventArgs args)
		{
			ComboBox combobox_sala_2 = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_sala_2.GetActiveIter (out iter)) {
				sala_2 = (int) this.combobox_sala_2.Model.GetValue(iter,1);
				
			}
		}
				void llena_combobox_sala_1()
		{
			//COMBO SALA 1
			combobox_sala_1.Clear();
			CellRendererText cell1= new CellRendererText();
			combobox_sala_1.PackStart(cell1, true);
			combobox_sala_1.AddAttribute(cell1,"text",0);
   
			ListStore store1 = new ListStore(typeof(string),typeof(int));
			combobox_sala_1.Model = store1;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_his_habitaciones "+
               						"WHERE id_tipo_admisiones = '700'"+
								"AND disponible = 'true' ORDER BY id_habitacion;";
               						
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read())
				{
					store1.AppendValues ((string) lector["descripcion_cuarto"]+
					                     Convert.ToString((int) lector["numero_cuarto"]),"-",(int) lector["id_habitacion"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
					
			TreeIter iter1;
			if (store1.GetIterFirst(out iter1)){
				combobox_sala_1.SetActiveIter (iter1);
				
			}
			combobox_sala_1.Changed += new EventHandler (onComboBoxChanged_sala_1);
		}
		
		void onComboBoxChanged_sala_1 (object sender, EventArgs args)
		{
			ComboBox combobox_sala_1 = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_sala_1.GetActiveIter (out iter)) {
				sala_1 = (int) this.combobox_sala_1.Model.GetValue(iter,1);							
			}
		}
	 
		void on_button_seleccionar_programacion_cliked(object sender, EventArgs args)
		{
			this.checkbutton_nueva_cirugia.Sensitive = false;
			llena_valores_programacion();
		}

		void llena_valores_programacion()
		{
			if(entry_programacion.Text  == "" || this.entry_programacion.Text == " "){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close, 
				"Debe de llenar el campo con el numero de programacion \n"+"existente para que los datos se muestren \n");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}else{
				
			
			
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			//if();{
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				// asigna el numero de paciente (PID)
				comando.CommandText = "SELECT cirujano,neonatologo,ayudante,anestesiologo,hora_programacion,aseguradora,tipo_paciente,"+
						"to_char(fecha_programacion,'yyyy') AS ano_programacion,to_char(fecha_programacion,'MM') AS mes_programacion,to_char(fecha_programacion,'dd') AS dia_programacion,"+
						"medico,descripcion_especialidad,diagnostico,cirugia,id_diagnostico,id_tipo_cirugia,circulante1,circulante2,internista,tipo_anestecia,inicio_cirugia,termino_cirugia,entrada_sala,"+
						"salida_sala,especialidad_cirugia,instrumentacion_especial,notas "+
						"FROM osiris_his_calendario_quirofano"+
						" WHERE id_numero_programacion = '"+(string) entry_programacion.Text+"' ;";					
				NpgsqlDataReader lector = comando.ExecuteReader ();
					
				Console.WriteLine(comando.CommandText.ToString());
					
				if(lector.Read()){
					this.entry_cirujano.Text = (string) lector["cirujano"];								
					this.entry_neonatologo.Text= (string) lector["neonatologo"];
					this.entry_ayudante.Text= (string) lector["ayudante"];
					this.entry_anestesiologo.Text= (string) lector["anestesiologo"];							
					this.entry_hora2.Text= (string) lector["hora_programacion"];
					this.entry_aseguradora.Text=(string)lector["aseguradora"];
					this.entry_tipo_paciente.Text=(string)lector["tipo_paciente"];
					this.entry_año2.Text = (string)lector["ano_programacion"];
					this.entry_mes2.Text = (string)lector["mes_programacion"];
					this.entry_dia2.Text = (string)lector["dia_programacion"];					
					this.entry_medico.Text=(string)lector["medico"];
					this.entry_especialidadmed.Text=(string)lector["descripcion_especialidad"];
					this.entry_diagnostico.Text=(string)lector["diagnostico"];
					this.entry_cirugia.Text=(string)lector["cirugia"];
					this.entry_id_diagnostico.Text=Convert.ToString((int)lector["id_diagnostico"]);
					this.entry_id_cirugia.Text= Convert.ToString((int)lector["id_tipo_cirugia"]);
					this.entry_circulante1.Text=(string)lector["circulante1"];
					this.entry_circulante2.Text=(string)lector["circulante2"];
					this.entry_internista.Text=(string)lector["internista"];
					this.entry_tipo_anestecia.Text=(string)lector["tipo_anestecia"];
					this.entry_inicio_cirugia.Text=(string)lector["inicio_cirugia"];
					this.entry_termino_cirugia.Text=(string)lector["termino_cirugia"];
					this.entry_entrada_sala.Text=(string)lector["entrada_sala"];
					this.entry_salida_sala.Text=(string)lector["salida_sala"];
					this.entry_especialidad_cirugia.Text=(string)lector["especialidad_cirugia"];
					this.entry_intrs_especial.Text=(string)lector["instrumentacion_especial"];
					this.entry_notas.Text=(string)lector["notas"];
				    this.checkbutton_asignar_folio.Sensitive = true;
				}else{
				Console.WriteLine("ENTRO");	
					
				}
				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, 
						ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run (); 				msgBoxError.Destroy();      		
			}
			conexion.Close ();	
			}
		}
		
		void on_button_guardar_prog_clicked(object sender, EventArgs args)
		{
			
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de guardar esta informacion ?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
	 		if(entry_año2.Text  == "" || entry_año2.Text == " "){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close, 
				"Debe de llenar el campo de fecha de programacion \n"+"para poder guardar los datos \n");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}else{	
	 		if (miResultado == ResponseType.Yes){
				if(this.checkbutton_asignar_folio.Sensitive == false){
				//Console.WriteLine("voy a guardar");
	 			NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
			    // Verifica que la base de datos este conectada
			    try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					comando.CommandText = "INSERT INTO osiris_his_calendario_quirofano ("+
                            "fechahora_creacion,"+							
							"id_cirujano,"+
							"id_neonatologo,"+
							"id_ayudante,"+
							"id_anestesiologo,"+
							"id_circulante1,"+
							"id_circulante2,"+
							"id_internista,"+
							"id_tipo_cirugia,"+
							"id_diagnostico,"+
							"sala2,"+																				
							"cirujano,"+
							"neonatologo,"+
							"ayudante,"+
							"anestesiologo,"+							
							"hora_programacion,"+
							"fecha_programacion,"+
							"tipo_paciente,"+
							"aseguradora,"+
							"diagnostico,"+
							"cirugia,"+
							"descripcion_especialidad,"+
							"medico,"+
							"circulante1,"+
							"circulante2,"+
							"internista,"+
							"notas,"+
							"inicio_cirugia, "+
							"termino_cirugia, "+
							"entrada_sala, "+
							"salida_sala, "+							
							"instrumentacion_especial,"+							
							"tipo_anestecia )"+
							"VALUES ('"+							
							DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+//fecha de creacion							
							this.id_cirujano+"','"+
							this.id_neonatologo+"','"+
							this.id_ayudante+"','"+
							this.id_anestesiologo+"','"+
							this.id_circulante1+"','"+
							this.id_circulante2+"','"+
							this.id_internista+"','"+
							this.idtipocirugia+"','"+
							this.iddiagnostico+"','"+
							this.sala_2.ToString()+"','"+																		
							this.entry_cirujano.Text+"','"+
							this.entry_neonatologo.Text+"','"+
							this.entry_ayudante.Text+"','"+
							this.entry_anestesiologo.Text+"','"+							
							this.entry_hora2.Text+"','"+
							this.entry_año2.Text+"-"+this.entry_mes2.Text+"-"+this.entry_dia2.Text+"','"+						
							this.entry_tipo_paciente.Text+"','"+
							this.entry_aseguradora.Text+"','"+
							this.entry_diagnostico.Text+"','"+
							this.entry_cirugia.Text+"','"+
							this.entry_especialidadmed.Text+"','"+
							this.entry_medico.Text+"','"+
							this.entry_circulante1.Text+"','"+
							this.entry_circulante2.Text+"','"+
							this.entry_internista.Text+"','"+
							this.entry_notas.Text+"','"+
					        this.entry_inicio_cirugia.Text+"','"+
							this.entry_termino_cirugia.Text+"','"+
							this.entry_entrada_sala.Text+"','"+
							this.entry_salida_sala.Text+"','"+
							this.entry_intrs_especial.Text+"','"+							
							this.entry_tipo_anestecia
							+"');";
							comando.ExecuteNonQuery();
					        comando.Dispose();
					
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
					         //Console.WriteLine(comando.CommandText.ToString());
					        // limpiar todos los entrys 
							this.entry_dia.Text = " ";
							this.entry_mes.Text = " ";
							this.entry_año.Text = " ";
							this.entry_hora.Text = " ";
							this.entry_anestesiologo.Text = " ";
							this.entry_aseguradora.Text = " ";
							this.entry_ayudante.Text = " ";
							this.entry_circulante1.Text = " ";
							this.entry_circulante2.Text = " ";
							this.entry_cirugia.Text = " ";
							this.entry_cirujano.Text = " ";				
							this.entry_diagnostico.Text = " ";
							this.entry_edad.Text = " ";
							this.entry_edad_prog.Text = " ";
							this.entry_especialidadmed.Text = " ";
							this.entry_folio_servicio.Text = " ";
							this.entry_id_cirugia.Text = " ";
							this.entry_id_diagnostico.Text = " ";				
							this.entry_internista.Text = " ";
							this.entry_medico.Text = " ";
							this.entry_neonatologo.Text = " ";
							this.entry_nombre_paciente.Text = " ";
							this.entry_paciente_prog.Text = " ";
							this.entry_pid_paciente.Text = " ";
							this.entry_sexo.Text = " ";
							this.entry_sexo_prog.Text = " ";
							this.entry_tipo_paciente.Text = " ";
							this.entry_dia2.Text = " ";
							this.entry_mes2.Text = " ";
							this.entry_año2.Text = " ";
							this.entry_hora2.Text = " ";
							this.entry_entrada_sala.Text = " ";
							this.entry_salida_sala.Text = " ";
							this.entry_especialidad_cirugia.Text = " ";
							this.entry_inicio_cirugia.Text = " ";
							this.entry_termino_cirugia.Text = " ";		
							this.entry_notas.Text = " ";
							this.entry_intrs_especial.Text= " ";
							this.entry_tipo_anestecia.Text=" ";
						    this.entry_programacion.Text =" ";

			}else{
							
				            Console.WriteLine("se conecta ala base de datos");
				 			NpgsqlConnection conexion; 
							conexion = new NpgsqlConnection (connectionString+nombrebd);
						    // Verifica que la base de datos este conectada
			    try{
								
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					comando.CommandText = "UPDATE osiris_his_calendario_quirofano SET cirujano = '"+this.entry_cirujano.Text+"', "+
							"neonatologo = '"+this.entry_neonatologo.Text+"', "+
							"ayudante = '"+this.entry_ayudante.Text+"', "+
							"anestesiologo = '"+this.entry_anestesiologo.Text+"', "+
							"circulante1 = '"+this.entry_circulante1.Text+"', "+
							"circulante2 = '"+this.entry_circulante2.Text+"', "+
							"internista = '"+this.entry_internista.Text+"', "+
							"especialidad_cirugia = '"+this.entry_especialidad_cirugia.Text+"', "+
							"tipo_anestecia = '"+this.entry_tipo_anestecia.Text+"', "+
							"inicio_cirugia = '"+this.entry_inicio_cirugia.Text+"', "+
							"termino_cirugia = '"+this.entry_termino_cirugia.Text+"', "+
							"entrada_sala= '"+this.entry_entrada_sala.Text+"', "+
							"salida_sala = '"+this.entry_salida_sala.Text+"', "+
							"instrumentacion_especial = '"+this.entry_intrs_especial.Text+"', "+
                            "notas = '"+this.entry_notas.Text+"', "+
							"sala1 = '"+this.sala_2.ToString()+"' , "+
							"pid_paciente = '"+entry_pid_paciente.Text+" ' "+
						"WHERE id_numero_programacion = '"+this.entry_programacion.Text+"';";
							Console.WriteLine(comando.CommandText);
							
						comando.ExecuteNonQuery();        			comando.Dispose();
						this.checkbutton_asignar_folio.Active = false;
						this.checkbutton_asignar_folio.Sensitive = false;
						this.checkbutton_nueva_cirugia.Sensitive = false;
						this.entry_dia.Text = " ";
						this.entry_mes.Text = " ";
						this.entry_año.Text = " ";
						this.entry_hora.Text = " ";
						this.entry_anestesiologo.Text = " ";
						this.entry_aseguradora.Text = " ";
						this.entry_ayudante.Text = " ";
						this.entry_circulante1.Text = " ";
						this.entry_circulante2.Text = " ";
						this.entry_cirugia.Text = " ";
						this.entry_cirujano.Text = " ";				
						this.entry_diagnostico.Text = " ";
						this.entry_edad.Text = " ";
						this.entry_edad_prog.Text = " ";
						this.entry_especialidadmed.Text = " ";
						this.entry_folio_servicio.Text = " ";
						this.entry_id_cirugia.Text = " ";
						this.entry_id_diagnostico.Text = " ";				
						this.entry_internista.Text = " ";
						this.entry_medico.Text = " ";
						this.entry_neonatologo.Text = " ";
						this.entry_nombre_paciente.Text = " ";
						this.entry_paciente_prog.Text = " ";
						this.entry_pid_paciente.Text = " ";
						this.entry_sexo.Text = " ";
						this.entry_sexo_prog.Text = " ";
						this.entry_tipo_paciente.Text = " ";
						this.entry_dia2.Text = " ";
						this.entry_mes2.Text = " ";
						this.entry_año2.Text = " ";
						this.entry_hora2.Text = " ";
						this.entry_entrada_sala.Text = " ";
						this.entry_salida_sala.Text = " ";
						this.entry_especialidad_cirugia.Text = " ";
						this.entry_inicio_cirugia.Text = " ";
						this.entry_termino_cirugia.Text = " ";		
						this.entry_notas.Text = " ";
						this.entry_intrs_especial.Text= " ";
						this.entry_tipo_anestecia.Text=" ";
						this.entry_programacion.Text =" ";
						this.button_guardar_prog.Sensitive = false;
					}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
				   }
					conexion.Close ();
				}
			}
			}
		}
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_busqueda(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;				
				llenando_lista_de_medicos();
			}
		}	
	}
}