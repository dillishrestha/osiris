 /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// created on 19/10/2007 at 04:10 p                                                                           ////
// Sistema Hospitalario OSIRIS                                                                                            ////
// Monterrey - Mexico                                                                                                ////
//                                                                                                                            ////
// Autor    	: Ing. Armando Leon (Programacion)                                                           ////
//				  Ing. Daniel Olivares (Ajustes y Reprogramacion)                                         ////
//                Ing. Erick Eduardo Gonzalez Reyes (Programation & Glade's window)           ////  
//                Ing. R. Israel Peña Gonzalez	(Programation & Glade's window)	              ////
//				                                                                                                              ////
// 				                                                                                                              ////
// Licencia		: GLP                                                                                                  ////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
// Programa		: recursos_humanos.cs
// Proposito	: Administracion de personal
// Objeto		: 
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class recursoshumanos
	{	// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando ventana del menu de recursos humanos
		[Widget] Gtk.Window menu_recursos_humanos;
		[Widget] Gtk.Button button_empleados;
		[Widget] Gtk.Button button_servicios;		
		[Widget] Gtk.Button button_rpt_1;
		[Widget] Gtk.Button button_rpt_2;
		//Widget] Gtk.ComboBox combo_tipocontrato;
		//[Widget] Gtk.Button button_nuevos_productos;
		
		string connectionString = "Server=localhost;" +
						"Port=5432;" +
						 "User ID=admin;" +
						"Password=1qaz2wsx;";
		string nombrebd;
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
    	
    	//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public recursoshumanos(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_ )
		{
			LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		nombrebd = _nombrebd_; 
    		
			Glade.XML gxml = new Glade.XML (null, "recursos_humanos.glade", "menu_recursos_humanos", null);
			gxml.Autoconnect (this);
	        
			// Muestra ventana de Glade
			menu_recursos_humanos.Show();
			// movimiento del personal
			button_rpt_1.Clicked += new EventHandler(on_button_reportes_clicked);
			button_rpt_2.Clicked += new EventHandler(on_button_reportes_bajas_clicked);
			button_empleados.Clicked += new EventHandler(on_button_empleados_clicked);
			button_servicios.Clicked  += new EventHandler(on_button_servicios_clicked);
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);			
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		void on_button_empleados_clicked (object sender, EventArgs args)
		{
			new osiris.alta_de_personal(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_reportes_clicked (object sender, EventArgs args){
			//new osiris.reportes_empleados(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"Contrato");
		}
		
		void on_button_reportes_bajas_clicked (object sender, EventArgs args){
			//new osiris.reportes_empleados(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"Bajas");
		}
		
		void on_button_servicios_clicked (object sender, EventArgs args)
		{
		
		}
		
	}
	
	//////////////////////////////////////////////////////////////////////////
	// Clase de Altas de Personal ///////////////////////////////
	//////////////////////////////////////////////////////////////////////////
	public class alta_de_personal
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		// Declarando ventana de asignacion contratos
		// --- Ventana
		[Widget] Gtk.Window catalogo_empleado;
		[Widget] Gtk.Window contrato_empleado;
		// --- Botones
		[Widget] Gtk.Button button_contrato_empleado;		
		[Widget] Gtk.Button button_vacaciones_empleado;
		[Widget] Gtk.Button button_baja_empleado;
		[Widget] Gtk.Button button_sistemas_empleado;
		[Widget] Gtk.Button button_buscar_empleado;
		[Widget] Gtk.Button button_guarda_empleado;
		[Widget] Gtk.Button button_editar_empleado;
		[Widget] Gtk.Button button_sel;
				
		[Widget] Gtk.CheckButton checkbutton_empleado_activo;
		[Widget] Gtk.CheckButton checkbutton_empleado_nuevo;
		
		[Widget] Gtk.ComboBox combobox_estado_civil;
		[Widget] Gtk.ComboBox combobox_tipo_empleado;
		[Widget] Gtk.ComboBox combobox_num_hijos_empleado;
		[Widget] Gtk.ComboBox combobox_casa_empleado;
		[Widget] Gtk.ComboBox combobox_escolaridad_empleado;
		[Widget] Gtk.ComboBox combobox_estado;
		[Widget] Gtk.ComboBox combobox_municipios;
		[Widget] Gtk.RadioButton  radiobutton_masculino_empleado;
		[Widget] Gtk.RadioButton  radiobutton_femenino_empleado;
		
		// --- campos
		[Widget] Gtk.Entry entry_id_empleado;
		[Widget] Gtk.Entry entry_estatus_empleado;
		[Widget] Gtk.Entry entry_nombre1_empleado;
		[Widget] Gtk.Entry entry_nombre2_empleado;
		[Widget] Gtk.Entry entry_apellido_paterno_empleado;
		[Widget] Gtk.Entry entry_apellido_materno_empleado;
		[Widget] Gtk.Entry entry_dia_nac;
		[Widget] Gtk.Entry entry_mes_nac;
		[Widget] Gtk.Entry entry_anno_nac;
		[Widget] Gtk.Entry entry_lugar_nac;
		[Widget] Gtk.Entry entry_nacionalidad;
		[Widget] Gtk.Entry entry_titulo_empleado;
		[Widget] Gtk.Entry entry_religion_empleado;
		[Widget] Gtk.Entry entry_rfc_empleado;
		[Widget] Gtk.Entry entry_curp_empleado;
		[Widget] Gtk.Entry entry_imss_empleado;
		[Widget] Gtk.Entry entry_infonavit_empleado;
		[Widget] Gtk.Entry entry_afore_empleado;
		[Widget] Gtk.Entry entry_cartilla_militar_empleado;
		[Widget] Gtk.Entry entry_talla_pantalon;
		[Widget] Gtk.Entry entry_talla_chaqueta;
		[Widget] Gtk.Entry entry_talla_zapatos;
		[Widget] Gtk.Entry entry_peso_empleado;
		[Widget] Gtk.Entry entry_estatura_empleado;
		[Widget] Gtk.Entry entry_tipo_sangre_empleado;
		[Widget] Gtk.Entry entry_residencia_empleado;
		[Widget] Gtk.Entry entry_nom_padre_empleado;
		[Widget] Gtk.Entry entry_nom_madre_empleado;
		[Widget] Gtk.Entry entry_nom_conyuge_empleado;
		[Widget] Gtk.Entry entry_calle_empleado;
		[Widget] Gtk.Entry entry_numcalle_empleado;
		[Widget] Gtk.Entry entry_codigo_postal_empleado;
		[Widget] Gtk.Entry entry_colonia_empleado;
		[Widget] Gtk.Entry entry_tel1_empleado;
		[Widget] Gtk.Entry entry_tel2_empleado;
		[Widget] Gtk.Entry entry_celular_empleado;
		[Widget] Gtk.Entry entry_email1_empleado;
		[Widget] Gtk.Entry entry_email2_empleado;
		[Widget] Gtk.Entry entry_fax_empleado;
		[Widget] Gtk.Entry entry_avisar_a_empleado;
		[Widget] Gtk.Entry entry_tel_accidente_empleado;
		[Widget] Gtk.Entry entry_notas_empleado;
		[Widget] Gtk.Entry entry_dia_ingreso;
		[Widget] Gtk.Entry entry_mes_ingreso;
		[Widget] Gtk.Entry entry_anno_ingreso;
		[Widget] Gtk.Entry entry_contrato_empleado;
		[Widget] Gtk.Entry entry_nombrepuesto_empleado;
		[Widget] Gtk.Entry entry_jornada_empleado;
		[Widget] Gtk.Entry entry_tipo_funcion;
		[Widget] Gtk.Entry entry_tiempo_comida;
		[Widget] Gtk.Entry entry_numero_locker;
		[Widget] Gtk.Entry entry_d_disfrute_empleado;
		[Widget] Gtk.Entry entry_depto_empleado;
		[Widget] Gtk.Entry entry_id_empleado_etiqueta;
		[Widget] Gtk.Entry entry_edad;
		[Widget] Gtk.Entry entry_dia_venc_cont;
		[Widget] Gtk.Entry entry_mes_venc_cont;
		[Widget] Gtk.Entry entry_anno_venc_cont;
		//--------------------------------------------------
		
		//CONTROLES DE LA VENTANA DE CAMBIO DE CONTRATO
		[Widget] Gtk.Button button_actualiza;
		[Widget] Gtk.ComboBox combobox_tipo_contrato;
		[Widget] Gtk.ComboBox combobox_tipo_pago_empleado;
		[Widget] Gtk.ComboBox combobox_jornada_empleado;
		[Widget] Gtk.ComboBox combobox_departamento_empleado;
		[Widget] Gtk.ComboBox combobox_puesto_empleado;
		[Widget] Gtk.ComboBox combobox_funcion_empleado;
		[Widget] Gtk.ComboBox combobox_tiempo_comida_empleado;
		
		
		[Widget] Gtk.Entry entry_dia_contrato;
		[Widget] Gtk.Entry entry_mes_contrato;
		[Widget] Gtk.Entry entry_anno_contrato;
		
		[Widget] Gtk.Entry entry_sueldo_min_empleado;
		[Widget] Gtk.Entry entry_sueldo_max_empleado;
		[Widget] Gtk.Entry entry_sueldo_actual_empleado;
		[Widget] Gtk.Entry entry_id_puesto;
		[Widget] Gtk.Entry entry_id_depto;
		[Widget] Gtk.Entry entry_lockers;
		//--------------------------------------------------
			
	   //ventana busca_empleado
	   [Widget] Gtk.Window busca_empleado;
	   [Widget] Gtk.Button button_buscar_emp;
	   [Widget] Gtk.Button button_selecciona;
	   //[Widget] Gtk.Button button_salir;
	   [Widget] Gtk.TreeView lista_de_empleados;
	   [Widget] Gtk.RadioButton radiobutton_busca_apellido;
	   [Widget] Gtk.RadioButton radiobutton_busca_nombre;
	   [Widget] Gtk.RadioButton radiobutton_busca_numero;
	   [Widget] Gtk.Entry entry_expresion;
	   //vacaciones:
	  //Widget] Gtk.Entry entry_dias_gozados;
	   [Widget] Gtk.Entry entry_dias_goce;
	   [Widget] Gtk.Entry entry_dias_por_gozar;
	   [Widget] Gtk.Entry entry_dia_vacaciones;
	   [Widget] Gtk.Entry entry_mes_vacaciones;
	   [Widget] Gtk.Button button_guardar_vac;
	   [Widget] Gtk.Button button_actualizar_vac;
	   //bajas:
	   [Widget] Gtk.Window baja_empleado;
	   [Widget] Gtk.Entry entry_dia_baja;
	   [Widget] Gtk.Entry entry_mes_baja;
	   [Widget] Gtk.Entry entry_anno_baja;
	   [Widget] Gtk.Entry entry_notas_baja;
	   [Widget] Gtk.Entry entry_causa_baja;
	   [Widget] Gtk.Button button_dar_baja;
	   [Widget] Gtk.ComboBox combobox_baja;
	   [Widget] Gtk.Entry entry_dd_ingreso;
	   [Widget] Gtk.Entry entry_mm_ingreso;
	   [Widget] Gtk.Entry entry_aaaa_ingreso;
	   [Widget] Gtk.Entry entry_depto;
	   [Widget] Gtk.Entry entry_puesto;
	   //contrato empleado
	   //fecha contrato
	   [Widget] Gtk.Entry entry_cont_dd;
	   [Widget] Gtk.Entry entry_cont_mm;
	   [Widget] Gtk.Entry entry_cont_aa;
		
	   //baja empleado:	
	   [Widget] Gtk.Entry entry_baja_dd;
	   [Widget] Gtk.Entry entry_baja_mes;
	   [Widget] Gtk.Entry entry_baja_aa;
		
	   
	   [Widget] Gtk.Button button_imprimir;
	   [Widget] Gtk.Window imp_cont_o_reg_alta;
	   [Widget] Gtk.Entry entry_nomb_imp;
	   [Widget] Gtk.Entry entry_direc_imp;
	   [Widget] Gtk.Entry entry_fech_nac_imp;
	   [Widget] Gtk.Entry entry_id_emp_imp;
	   [Widget] Gtk.Entry entry_naci_imp;
	   [Widget] Gtk.Entry entry_edad_imp;
	   [Widget] Gtk.Button button_imp_cont;
	   [Widget] Gtk.Button button_imp_reg;
	   
	 private TreeStore treeViewEngineBusca;
		
		public string connectionString = "Server=localhost;" +
						"Port=5432;" +
						 "User ID=admin;" +
						"Password=1qaz2wsx;";
		public string nombrebd;
		public string LoginEmpleado;
    	public string NomEmpleado;
    	public string AppEmpleado;
    	public string ApmEmpleado;
    	public int idestado = 1;
    	//variables de datos
		public string estado = "";
		public string estado_a_guardar = "";
		public string municipios = "";
		public string municipios_a_guardar = "";
		public string sexopaciente = "H";
    	public bool editando = false;
    	// ALMACENAR DATOS DE COMBOS EN VARIABLES GLOBALES PARA EDICION DE DATOS
		public string  tmp_estado = "";
		public string  tmp_id_estado = "";
		public string  tmp_municipios = "";
		public string  tmp_id_municipios = "";
		public bool banderanuevo = true;  //V = agrega empleado nuevo, F= modifica empleado
		public string  var_tipo_empleado = ""; 
		public string  var_clave_tipo_empleado = "";
		public string  var_clave_tipo_empleado_a_guardar = "";
		public string  tipo_empleado_a_guardar="";
		public string  var_baja_emp = "";
		public string  tmp_estado_civil = "";
		public string  tmp_estado_civil_a_guardar= "";
    	public string  var_tipo_casa = "";
    	public string  var_escolaridad = "";
    	public string  var_casa_empleado_a_guardar = "";
    	public string  var_num_hijos = "";
    	public string  var_num_hijos_a_guardar = "";
    	//DECLARACION DE VARIABLES OCULTAS
		public string  dia_contrato_oculta="";
		public string  mes_contrato_oculta="";
		public string  anno_contrato_oculta="";
		//fecha bajas
		public string  tipo_contrato_oculta="";
		public string  tipo_pago_oculta="";
		public string  jornada_oculta="";
	    public string  funcion_de_empleado="";
	    public string  tiempo_de_comida = "";
		public string  departamento_oculta="";
		public string  puesto_oculta="";
		public string  id_departamento_oculta="";
		public string  id_puesto_oculta="";
		public string  sueldo_minimo_oculta=""; 
    	public string  sueldo_maximo_oculta="";
    	public string  sueldo_actual_oculta="";
    	public string  tipofuncion_oculta="";
    	public string  tiempocomida_oculta="";
    	public string  numlocker_oculta="";
    	public string  selcampo = "";
    	public string  selidemp= "";
    	public int edadanno = 0;
    	public string historialcontrato= "";
    	public string historialcambio = "";
    	public string tiempo_vencimiento_contrato="";
    	//public  fecha_vencimiento_contrato="";
    	
    	//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		private TreeStore treeViewEngine;
		//private ListStore store_aseguradora;
		
		public alta_de_personal(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_ )
		{
			LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		nombrebd = _nombrebd_; 
    		
			Glade.XML gxml = new Glade.XML (null, "recursos_humanos.glade", "catalogo_empleado", null);
			gxml.Autoconnect (this);
	        
			// Muestra ventana de Glade
			catalogo_empleado.Show();

			//	Console.WriteLine( DateTime.Today.AddDays(DateTime.DaysInMonth (2008 , 03)) );
			
			//desactiva los controles de la pantallatipo_ca
			protegecampos();
			editando = false;
			
			//Editar datos del empleado
			button_editar_empleado.Clicked += new EventHandler(on_button_editar_empleado_clicked);
			//Guarda datos del empleado
			button_guarda_empleado.Clicked += new EventHandler(on_button_guarda_empleado_clicked);
			//entrar a contratos
			//button_contrato_empleado.Clicked += new EventHandler(on_button_contrato_empleado_clicked);
			button_contrato_empleado.Clicked += new EventHandler(on_button_contrato_empleado_clicked);
			//entrar a vacaciones
			button_vacaciones_empleado.Clicked += new EventHandler(on_button_vacaciones_empleado_clicked);
			//boton imprimir
			button_imprimir.Clicked += new EventHandler(on_button_imprimir_clicked);
			//entrar a bajas
			button_baja_empleado.Clicked += new EventHandler(on_button_baja_empleado_clicked);
			//selccionar numero de id
			button_sel.Clicked += new EventHandler(on_button_sel_clicked);
			//para crear un empleado nuevo
			checkbutton_empleado_nuevo.Clicked  += new EventHandler(on_checkbutton_empleado_nuevo_clicked);
			//check de vigente ( empleado activo)
			checkbutton_empleado_activo.Clicked  += new EventHandler(on_checkbutton_empleado_activo_clicked);
			//entrar a bajas
			button_sistemas_empleado.Clicked += new EventHandler(on_button_sistemas_empleado_clicked);
			//buscando empleados
			button_buscar_empleado.Clicked += new EventHandler(on_button_buscar_empleado_clicked);
			// Validando que se presione enter
			entry_id_empleado.KeyPressEvent += onKeyPressEvent_enter_empleado;
			// Validando que solo se escriben numeros
			entry_codigo_postal_empleado.KeyPressEvent += onKeyPressEventactual;
			//modificando el tipo de empleado
			//entry's numericos
			this.entry_numcalle_empleado.KeyPressEvent += onKeyPressEventactual;
			this.entry_tel1_empleado.KeyPressEvent += onKeyPressEventactual;
			this.entry_tel2_empleado.KeyPressEvent += onKeyPressEventactual;
			this.entry_tel_accidente_empleado.KeyPressEvent += onKeyPressEventactual;
			this.entry_anno_nac.KeyPressEvent += onKeyPressEventactual;
			this.entry_mes_nac.KeyPressEvent += onKeyPressEventactual;
		
		    this.entry_dia_nac.KeyPressEvent += onKeyPressEventactual;
			this.entry_estatura_empleado.KeyPressEvent += onKeyPressEventactual;
			this.entry_peso_empleado.KeyPressEvent += onKeyPressEventactual;
			this.entry_fax_empleado.KeyPressEvent += onKeyPressEventactual;
			this.entry_celular_empleado.KeyPressEvent += onKeyPressEventactual;
			   	
	   		this.entry_anno_ingreso.KeyPressEvent += onKeyPressEventactual;
	  		this.entry_mes_ingreso.KeyPressEvent += onKeyPressEventactual;
	    	this.entry_dia_ingreso.KeyPressEvent += onKeyPressEventactual;
	    	
	    	this.entry_anno_ingreso.Text = DateTime.Now.ToString("yyyy");
			this.entry_mes_ingreso.Text = DateTime.Now.ToString("MM"); 
	    	this.entry_dia_ingreso.Text = DateTime.Now.ToString("dd");
			
			combobox_tipo_empleado.Changed += new EventHandler(onComboBoxChanged_tipo_empleado);
			//modificando el combo de estados MODIFICANDO
			combobox_estado.Changed += new EventHandler(onComboBoxChanged_estado);
			//combobox_municipios.Changed += new EventHandler(onComboBoxChanged_municipios);
				
		   	// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);	
		}
		
		void edita_empleado()
		{
			// ABRIR CAMPOS
			abrecampos();
			if (this.checkbutton_empleado_activo.Active ==true){
				this.button_contrato_empleado.Sensitive = false;
			}else{
				this.button_contrato_empleado.Sensitive = true;
			}
			editando = true;
			//llena el combo de tipo de emplado
			llenado_tipo_empleado(var_tipo_empleado);
			llenado_escolaridad_empleado(var_escolaridad);
			combobox_tipo_empleado.Sensitive = false;
			llenado_estadocivil(tmp_estado_civil);
			llenado_tipo_casa(var_tipo_casa);
			llenado_num_hijos(var_num_hijos);
			llenado_estados(estado,0);
			llenado_municipios(municipios, idestado);
			//Contiene el tipo de empleado para ser almacenado
			tipo_empleado_a_guardar = var_tipo_empleado;  //correcta
			var_clave_tipo_empleado_a_guardar = var_clave_tipo_empleado;
			//Contiene el estado civil para ser almacenado
			tmp_estado_civil_a_guardar = tmp_estado_civil;
			//contiene el tipo de casa del empleado
			var_casa_empleado_a_guardar = var_tipo_casa;
			//contiene el numero de hijos del empleado
			var_num_hijos_a_guardar = var_num_hijos;
			//datos de geografia
			estado_a_guardar = estado;
			municipios_a_guardar = municipios;
			entry_dia_ingreso.Sensitive = false;
			entry_mes_ingreso.Sensitive = false;
			entry_anno_ingreso.Sensitive = false;
		}
		
		string ultimoregistro(string letra)
		{
			string cadtemp="";
			string strsql = "";
			string varsalida="";
			int valortemp=0;
			
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				strsql = "SELECT SUBSTR(id_empleado,2,6) AS ultimonumero "+
						"from osiris_empleado "+
						"where SUBSTR(id_empleado,1,1) = '"+letra.ToUpper()+"' "+
						"ORDER BY  id_empleado DESC LIMIT 1;";
				//Console.WriteLine(strsql);
                comando.CommandText = strsql;					
                
                NpgsqlDataReader lector = comando.ExecuteReader ();
				if ((bool) lector.Read()){
					cadtemp = (string) lector["ultimonumero"];
                	valortemp = int.Parse(cadtemp);
                	valortemp = valortemp + 1;
                }else {valortemp = 1;}
                
				comando.ExecuteNonQuery();
				comando.Dispose();
			}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Info, 
												ButtonsType.Ok,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
			}
			conexion.Close();
			
			//cadtemp = ToString(valortemp);
			cadtemp= Convert.ToString(valortemp);
			//Console.WriteLine("longitud inicial: "+cadtemp.Length);
			while (cadtemp.Length < 6){
				cadtemp = '0' + cadtemp;
			}			
			varsalida= letra + cadtemp;
			return varsalida;
		}

		void guardar_empleado()
		{		
			if (entry_id_empleado_etiqueta.Text == "EMPLEADO NUEVO" ){
				//IMPORTANTE
				//FALTA OBTENER EL NUEVO NUMERO DE EMPLEADO
				
				string strsql = "";
				string nuevoidcliente="";
				
				//preguntar por el tipo de empleado
				if (var_tipo_empleado == ""){
					//no ha definido el tipo de empleado
					MessageDialog msgBox_te = new MessageDialog (MyWin,DialogFlags.Modal,
												MessageType.Info ,ButtonsType.Ok,"No ha seleccionado un tipo de empleado");
					msgBox_te.Run();
					msgBox_te.Destroy();
				}else{
					//string nuevoidcliente = ultimoregistro("F");
					
					//nuevoidcliente = nuevoidcliente.ToUpper();
					nuevoidcliente = ultimoregistro(var_clave_tipo_empleado);
					
					NpgsqlConnection conexion;
					conexion = new NpgsqlConnection (connectionString+nombrebd );
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();						 
						strsql = "INSERT INTO osiris_empleado ("+
								"id_empleado,"+
		             			"login_empleado,"+
		                		"nombre1_empleado,"+
		                		"nombre2_empleado,"+
		                		"apellido_paterno_empleado,"+
		                		"apellido_materno_empleado,"+
		                		"password_empleado,"+
		                		"estado_del_empleado) "+
		                		"VALUES ('"+nuevoidcliente+"',"+
		      					"'',"+//login_empleado lo da sistemas
		      					"'"+ entry_nombre1_empleado.Text.Trim().ToUpper()+"',"+
		      					"'"+ entry_nombre2_empleado.Text.Trim().ToUpper()+"',"+
		      					"'"+ entry_apellido_paterno_empleado.Text.Trim().ToUpper()+"',"+
		      					"'"+ entry_apellido_materno_empleado.Text.Trim().ToUpper()+"',"+
		      					"'',"; //pwd lo da sistemas
		      					
		      			if (checkbutton_empleado_activo.Active == true ){
		      				strsql = strsql + "'1');"; //estadodelempleado 1=activo 
		      			}else{
		      				strsql = strsql + "'0');"; //0=inactivo
		      			}
		      					
		                //Console.WriteLine(strsql);
		                comando.CommandText = strsql;					
		                					
						comando.ExecuteNonQuery();
						comando.Dispose();
					
						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
													MessageType.Info,ButtonsType.Ok,"El registro "+nuevoidcliente+" se creó satisfactoriamente");
						msgBox.Run();
						msgBox.Destroy();
								
						//insertando el detalle
						NpgsqlConnection conexion2;
						conexion2 = new NpgsqlConnection (connectionString+nombrebd );
						try{
							conexion2.Open ();
							NpgsqlCommand comando2; 
							comando2 = conexion2.CreateCommand ();
							strsql = "INSERT INTO osiris_empleado_detalle ("+
									"id_empleado,"+
			                		"lugar_nacimiento, " +
			                		"nacionalidad, "+
			                		"municipio,"+
			                		"estado,"+
									"escolaridad, "+ 
									"titulo, "+ 
									"religion, "+ 
									"rfc, "+ 
									"curp, "+ 
									"imss, "+ 
									"infonavit, "+ 
									"afore, "+ 
									"cartilla_militar, "+ 
									"talla_pantalon, "+ 
									"talla_chaqueta, "+ 
									"talla_zapatos, "+ 
									"calle, "+ 
									"numero, "+ 
									"colonia, "+ 
									"fecha_de_nacimiento, "+ 
									"fecha_de_ingreso, "+
						            "codigo_postal, "+ 
									"peso_empleado, "+ 
									"estatura_empleado, "+ 
									"tipo_sangre, "+ 
									"residencia, "+ 
									"tipo_empleado, "+
									"nombre_padre, "+ 
									"nombre_madre, "+ 
									"nombre_conyuge, "+ 
									"telefono_1, "+ 
									"telefono_2, "+ 
									"celular, "+ 
									"email1, "+ 
									"email2, "+ 
									"fax, "+ 
									"accidente_avisar_a, "+ 
									"accidente_telefono, "+ 
									"notas_empleado, "+
									"genero, "+
									"estado_civil, "+
									"tipo_casa, "+
									"num_hijos, "+
									"fecha_de_registro) "+
								    "VALUES ('"+nuevoidcliente+"',"+
			      					"'"+ entry_lugar_nac.Text.ToUpper()+"',"+
			      					"'"+ entry_nacionalidad.Text.ToUpper()+"',";
			      			
	      					if (estado == ""){
	      						strsql = strsql + "'', ";
	      					}else{
	      						strsql = strsql + "'"+ estado+"', ";
	      					}
	      					
	      					if (municipios == ""){
	      						strsql = strsql + "'', ";
	      					}else{
	      						strsql = strsql + "'"+ municipios+"', ";
	      					}
	      									
			      			strsql = strsql +
			      					
			      					"'"+var_escolaridad+"',"+
			      					"'"+ entry_titulo_empleado.Text.ToUpper()+"',"+
			      					"'"+ entry_religion_empleado.Text.ToUpper()+"', "+
			      					"'"+ entry_rfc_empleado.Text.ToUpper()+"', "+
			      					"'"+ entry_curp_empleado.Text.ToUpper()+"', "+
			      					"'"+ entry_imss_empleado.Text.ToUpper()+"', "+
			      					"'"+ entry_infonavit_empleado.Text.ToUpper()+"', "+
			      					"'"+ entry_afore_empleado.Text.ToUpper()+"', "+
			      					"'"+ entry_cartilla_militar_empleado.Text.ToUpper()+"', "+
			      					"'"+ entry_talla_pantalon.Text.ToUpper()+"', "+
			      					"'"+ entry_talla_chaqueta.Text.ToUpper()+"', "+
			      					"'"+ entry_talla_zapatos.Text.ToUpper()+"', "+
			      					"'"+ entry_calle_empleado.Text.ToUpper()+"', "+
			      					"'"+ entry_numcalle_empleado.Text.ToUpper()+"', "+
			      					"'"+ entry_colonia_empleado.Text.ToUpper()+"', ";
			      					if ((entry_anno_nac.Text=="") || (entry_mes_nac.Text=="") || (entry_dia_nac.Text==""))
			      						{strsql = strsql + "'2000-01-01',";}
			      					else {
			      						strsql = strsql + "'"+ entry_anno_nac.Text.ToUpper()+"-"+ entry_mes_nac.Text.ToUpper()+"-"+ entry_dia_nac.Text.ToUpper()+"', ";;
			      					}
			      					
			      					//strsql = strsql + "'"+ DateTime.Now.ToString("yyyy-MM-dd")+"', ";		//fecha de ingreso
			      					
			      					strsql = strsql + "'"+entry_anno_ingreso.Text+"-"+entry_mes_ingreso.Text+"-"+entry_dia_ingreso.Text+"', ";		//fecha de ingreso
			      					//strsql = strsql + "'"+ DateTime.Now.ToString("yyyy-MM-dd")+"', ";		// fecha del contrato
			      					
			      					
			      					//string cadtemp = "";
			      					//cadtemp = entry_codigo_postal_empleado.Text.Trim();
			      					if (( entry_codigo_postal_empleado.Text.Length == 0) || (entry_codigo_postal_empleado.Text.Length < 5) || (entry_codigo_postal_empleado.Text == "") || (entry_codigo_postal_empleado.Text == "0"))
			      					{
			      						strsql = strsql + "'0',";
			      					}else{
				      					strsql = strsql + "'"+ int.Parse(entry_codigo_postal_empleado.Text)+"',";
			      					}
			      					
			      					if (entry_peso_empleado.Text==""){
			      						strsql = strsql + "'0.00',";
			      					}else{
			      						strsql = strsql + "'"+ float.Parse(entry_peso_empleado.Text)+"', ";
			      					}
			      					
			      					if (entry_estatura_empleado.Text==""){
			      						strsql = strsql + "'0.00',";
			      					}else{
			      						strsql = strsql + "'"+ float.Parse(entry_estatura_empleado.Text.ToUpper())+"', ";
			      					}
			      								      					
			      					if (entry_tipo_sangre_empleado.Text == ""){
			      						strsql = strsql + "'', ";
			      					}else{
			      						strsql = strsql + "'"+  entry_tipo_sangre_empleado.Text.ToUpper()+"', ";
			      					}
			      					
			      					if (entry_residencia_empleado.Text==""){
			      						strsql = strsql + "'0',";
			      					}else{
			      						strsql = strsql + "'"+ int.Parse(entry_residencia_empleado.Text.ToUpper())+"', ";
			      					}
			      					    					
			      					if (var_tipo_empleado == ""){
			      						strsql = strsql + "'',";
			      					}else{
			      						strsql = strsql + "'"+ var_tipo_empleado+"', ";
			      					}
			      						      					
			      					strsql = strsql + "'"+ entry_nom_padre_empleado.Text.ToUpper()+"', "+
					      					"'"+ entry_nom_madre_empleado.Text.ToUpper()+"', "+
					      					"'"+ entry_nom_conyuge_empleado.Text.ToUpper()+"', "+
					      					"'"+ entry_tel1_empleado.Text.ToUpper()+"', "+
					      					"'"+ entry_tel2_empleado.Text.ToUpper()+"', "+
					      					"'"+ entry_celular_empleado.Text.ToUpper()+"', "+
					      					"'"+ entry_email1_empleado.Text.ToLower()+"', "+
					      					"'"+ entry_email2_empleado.Text.ToLower()+"', "+
					      					"'"+ entry_fax_empleado.Text.ToUpper()+"', "+
					      					"'"+ entry_avisar_a_empleado.Text.ToUpper()+"', "+
					      					"'"+ entry_tel_accidente_empleado.Text.ToUpper()+"', "+
					      					"'"+ entry_notas_empleado.Text.ToUpper()+"', ";
			      					
									if (radiobutton_masculino_empleado.Active == true){
			      	                 	strsql = strsql +"'H',";
			      	                }else{
			      	                 	strsql = strsql +"'M',";
			      	                }
			      					
			      					if (tmp_estado_civil == ""){
			      						strsql = strsql + "'',";
			      					}else{
			      						strsql = strsql + "'"+ tmp_estado_civil+"', ";
			      					}
			      					
			      					if (var_tipo_casa == "")
			      					{
			      						strsql = strsql + "'',";
			      					}else{
			      						strsql = strsql + "'"+ var_tipo_casa+"', ";
			      					}
			      					
			      					if (var_num_hijos== "")
			      					{
			      						strsql = strsql + "'0',";
			      					}else{
			      						strsql = strsql + "'"+ var_num_hijos+"', ";
			      					}
			      				
			      				strsql = strsql + "'"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"'); ";
			      				
			      					
			              
			                comando2.CommandText = strsql;					
			                					
							comando2.ExecuteNonQuery();
							comando2.Dispose();
							
							MessageDialog msgBox2 = new MessageDialog (MyWin,DialogFlags.Modal,
													MessageType.Info,ButtonsType.Ok,"El detalle del registro "+nuevoidcliente+" se creó satisfactoriamente");
							msgBox2.Run();
							msgBox2.Destroy();
							
							checkbutton_empleado_nuevo.Active = false;
							 
							}catch (NpgsqlException ex){
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
															MessageType.Info, 
															ButtonsType.Ok,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
								msgBoxError.Destroy();
							}
						//fin del detalle
						
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Info, 
													ButtonsType.Ok,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
					}
					conexion.Close();
					protegecampos();
				}//fin del tipo de empleado	
			}
		
			//validar que sea nuevo
			//leer datos
			//guardar
			editando = false;
			
		}
		
		void actualiza_empleado()
		{	NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
			    // Verifica que la base de datos este conectada
			    
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
			 		string strsql = "";
					strsql = "UPDATE osiris_empleado "+
											"SET " +
											"nombre1_empleado = '" + entry_nombre1_empleado.Text.ToUpper()+"', "+
											"nombre2_empleado = '" + entry_nombre2_empleado.Text.ToUpper()+"', "+
											"apellido_paterno_empleado = '"+entry_apellido_paterno_empleado.Text.ToUpper()+"', "+
											"apellido_materno_empleado = '"+entry_apellido_materno_empleado.Text.ToUpper()+"', ";

                                            
											if (historialcontrato != "")
											{strsql = strsql + "historial_de_contrato = historial_de_contrato || '"+historialcontrato+"', ";}
											
											historialcambio = "Actualizo: " + LoginEmpleado + ";El dia: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n";
									      	strsql = strsql + "historial_de_cambios = historial_de_cambios  || '"+historialcambio+"',  ";

                                                                              
		 									if (checkbutton_empleado_activo.Active == true)
		 									{strsql = strsql + "estado_del_empleado = '"+1+"' ";}
		 									else
		 									{strsql = strsql + "estado_del_empleado = '"+0+"' ";}
		 									
		 			strsql = strsql + "WHERE id_empleado =  '"+entry_id_empleado_etiqueta.Text+"';";
		        	comando.CommandText = strsql; 
		 			//"nombre2_empleado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
		 			//Console.WriteLine(strsql);
					comando.ExecuteNonQuery();
			        comando.Dispose();
					//almacena el detalle			        
					comando = conexion.CreateCommand ();
			 		strsql = "";
					strsql = "UPDATE osiris_empleado_detalle "+
							"SET " +
					        "lugar_nacimiento = '" + entry_lugar_nac.Text.ToUpper()+"', "+
							"nacionalidad = '"+ entry_nacionalidad.Text.ToUpper()+"', "+
							"escolaridad = '"+ var_escolaridad.ToUpper()+"', "+
							"titulo = '"+ entry_titulo_empleado.Text.ToUpper()+"', "+
							"religion = '"+ entry_religion_empleado.Text.ToUpper()+"', "+
							"rfc = '"+ entry_rfc_empleado.Text.ToUpper()+"', "+
							"curp = '"+ entry_curp_empleado.Text.ToUpper()+"', "+
							"imss = '"+ entry_imss_empleado.Text.ToUpper()+"', "+
							"infonavit = '"+ entry_infonavit_empleado.Text.ToUpper()+"', "+
							"afore = '"+ entry_afore_empleado.Text.ToUpper()+"', "+
							"cartilla_militar = '"+ entry_cartilla_militar_empleado.Text.ToUpper()+"', "+
							"talla_pantalon = '"+ entry_talla_pantalon.Text.ToUpper()+"', "+
							"talla_chaqueta = '"+ entry_talla_chaqueta.Text.ToUpper()+"', "+
							"talla_zapatos = '"+ entry_talla_zapatos.Text.ToUpper()+"', "+
							"calle = '"+ entry_calle_empleado.Text.ToUpper()+"', "+
							"numero = '"+ entry_numcalle_empleado.Text.ToUpper()+"', "+
							"colonia = '"+ entry_colonia_empleado.Text.ToUpper()+"', ";
					
					if (radiobutton_masculino_empleado.Active == true)
					{strsql = strsql +"genero = 'H',"; }
					else
					{strsql = strsql +"genero = 'M',";}
										
					if ((entry_anno_nac.Text=="") || (entry_mes_nac.Text=="") || (entry_dia_nac.Text==""))
 					{strsql = strsql + "fecha_de_nacimiento = '2000-01-01',";}
 					else 
 					{strsql = strsql + "fecha_de_nacimiento = '"+ entry_anno_nac.Text.ToUpper()+"-"+ entry_mes_nac.Text.ToUpper()+"-"+ entry_dia_nac.Text.ToUpper()+"', ";}
 					
					string cadtemp = "";
 					cadtemp = entry_codigo_postal_empleado.Text.Trim();
 					if ((cadtemp.Length == 0) || (cadtemp.Length < 5) || (cadtemp == "") || (cadtemp == "0"))
 					{strsql = strsql + "codigo_postal = '0',";}
 					else
 					{strsql = strsql + "codigo_postal = '"+ int.Parse(entry_codigo_postal_empleado.Text)+"', ";
 					 //strsql = strsql + "codigo_postal = '"+ cadtemp.ToUpper().Substring(0,5)+"', ";}
                     }
					if (entry_peso_empleado.Text=="")
 					{strsql = strsql + "peso_empleado = '0.0',";}
 					else
 					{strsql = strsql + "peso_empleado = '"+ float.Parse(entry_peso_empleado.Text.ToUpper())+"', ";}
 					
 					if (entry_estatura_empleado.Text=="")
 					{strsql = strsql + "estatura_empleado = '0.0',";}
 					else
 					{strsql = strsql + "estatura_empleado = '"+ float.Parse(entry_estatura_empleado.Text.ToUpper())+"', ";}
										
					strsql = strsql + "tipo_sangre = '"+ entry_tipo_sangre_empleado.Text.ToUpper()+"', ";
					
					if (entry_residencia_empleado.Text=="")
 					{strsql = strsql + "residencia = '0',";}
 					else
 					{strsql = strsql + "residencia = '"+ int.Parse(entry_residencia_empleado.Text.Trim())+"', ";}
										
					strsql = strsql + 
							"nombre_padre = '"+ entry_nom_padre_empleado.Text.ToUpper()+"', "+
							"nombre_madre = '"+ entry_nom_madre_empleado.Text.ToUpper()+"', "+
							"nombre_conyuge = '"+ entry_nom_conyuge_empleado.Text.ToUpper()+"', "+
							"telefono_1 = '"+ entry_tel1_empleado.Text.ToUpper()+"', "+
							"telefono_2 = '"+ entry_tel2_empleado.Text.ToUpper()+"', "+
							"celular = '"+ entry_celular_empleado.Text.ToUpper()+"', "+
							"email1 = '"+ entry_email1_empleado.Text.ToLower()+"', "+
							"email2 = '"+ entry_email2_empleado.Text.ToLower()+"', "+
							"fax = '"+ entry_fax_empleado.Text.ToUpper()+"', "+
							"accidente_avisar_a = '"+ entry_avisar_a_empleado.Text.ToUpper()+"', "+
							"accidente_telefono = '"+ entry_tel_accidente_empleado.Text.ToUpper()+"', "+
							"notas_empleado = '"+ entry_notas_empleado.Text.ToUpper()+"', ";
							//"tipo_contrato = '"+ entry_contrato_empleado.Text.ToUpper()+"', ";
							//"departamento = '"+ entry_depto_empleado.Text.ToUpper()+"', "+
							//"puesto = '"+ entry_nombrepuesto_empleado.Text.ToUpper()+"', ";
							//"jornada = '"+ entry_jornada_empleado.Text.ToUpper()+"', ";
							//"diasvacpordisfrutar = '"+ entry_d_disfrute_empleado.Text+"', " 
							
					
					////// TIPO DE EMPLEADO
					if (tipo_empleado_a_guardar == var_tipo_empleado){//no se ha modificado el tipo de empleado, no se actualiza
						strsql = strsql + "tipo_empleado =  '"+tipo_empleado_a_guardar+"', ";
					}else{//se almacena en tipo_empleado_a_guardar el valor que selecciono el usuario
						tipo_empleado_a_guardar = var_tipo_empleado;//tipo_empleado_a_guardar es el que se usa en el update
						strsql = strsql + "tipo_empleado =  '"+tipo_empleado_a_guardar+"', ";
					}					
					///////////ESTADO CIVIL
					if (tmp_estado_civil_a_guardar == tmp_estado_civil){
						//no se ha modificado el estado_civil, no se actualiza
						strsql = strsql + "estado_civil =  '"+tmp_estado_civil_a_guardar+"', ";
					}else{//se almacena en tmp_estado_civil_a_guardar el valor que selecciono el usuario
						tmp_estado_civil_a_guardar = tmp_estado_civil;//tmp_estado_civil_a_guardar es el que se usa en el update
						strsql = strsql + "estado_civil =  '"+tmp_estado_civil_a_guardar+"', ";
					}
		        	///////////TIPO DE CASA
					if (var_casa_empleado_a_guardar == var_tipo_casa){
						//no se ha modificado el tipo_casa, no se actualiza
						strsql = strsql + "tipo_casa =  '"+var_casa_empleado_a_guardar+"', ";
					}else{
						//se almacena en var_casa_empleado_a_guardar el valor que selecciono el usuario
						var_casa_empleado_a_guardar = var_tipo_casa;//tmp_estado_civil_a_guardar es el que se usa en el update
						strsql = strsql + "tipo_casa =  '"+var_casa_empleado_a_guardar+"', ";
					}
					///////////NUMERO DE HIJOS
					if (var_num_hijos_a_guardar == var_num_hijos){
						//no se ha modificado el numero de hijos, no se actualiza  //VALIDAR QUE SE GUARDE UN CERO AL NO SELECCIONAR.
						if (var_num_hijos_a_guardar == ""){
							strsql = strsql + "num_hijos =  '0', ";
						}else{
							strsql = strsql + "num_hijos =  '"+var_num_hijos_a_guardar+"', ";
						}
					}else{
						//se almacena en var_casa_empleado_a_guardar el valor que selecciono el usuario
						var_num_hijos_a_guardar = var_num_hijos;//tmp_estado_civil_a_guardar es el que se usa en el update
						if (var_num_hijos_a_guardar == "")
						{strsql = strsql + "num_hijos =  '0', ";}
						else{strsql = strsql + "num_hijos =  '"+var_num_hijos_a_guardar+"', ";}
					}
		            ///////////GEOGRAFIA
					if (estado_a_guardar == estado){//no se ha modificado el estado, no se actualiza
						strsql = strsql + "estado =  '"+estado_a_guardar+"', ";
					}else{//se almacena en estado_a_guardar el valor que selecciono el usuario
						estado_a_guardar = estado;
						strsql = strsql + "estado =  '"+estado_a_guardar+"', ";
					}
					
					if (municipios_a_guardar == municipios){
						//no se ha modificado el municipios, no se actualiza
						strsql = strsql + "municipio =  '"+municipios_a_guardar+"', ";
					}else{
						//se almacena en municipios_a_guardar el valor que selecciono el usuario
						municipios_a_guardar = municipios;
						strsql = strsql + "municipio =  '"+municipios_a_guardar+"', ";
					}
					
					//ACTUALIZANDO DATOS DE LAS VENTANAS
					if (entry_contrato_empleado.Text != tipo_contrato_oculta){
						strsql = strsql + "tipo_contrato =  '"+tipo_contrato_oculta+"', ";
						//linea para guardar cuando se hacen modificaciones en el tipo de contrato 	
					}
					
					if ((dia_contrato_oculta=="") || (mes_contrato_oculta=="") || (anno_contrato_oculta=="")){
						strsql = strsql + "fecha_de_contrato = '2000-01-01',";
					}else{
						strsql = strsql + "fecha_de_contrato = '"+ anno_contrato_oculta.ToUpper()+"-"+ mes_contrato_oculta.ToUpper()+"-"+ dia_contrato_oculta.ToUpper()+"', ";
					}
										
					strsql = strsql + "jornada =  '"+jornada_oculta+"', "+
				    "tipo_pago =  '"+tipo_pago_oculta+"', ";
										
					if (sueldo_minimo_oculta=="" || sueldo_minimo_oculta=="0"){
						strsql = strsql + "sueldo_min = '0.0', ";
					}else{
						strsql = strsql + "sueldo_min = '"+sueldo_minimo_oculta+"', ";
					}
 					
 					if (sueldo_maximo_oculta=="" || sueldo_maximo_oculta=="0"){
 						strsql = strsql + "sueldo_max = '0.0', ";
 					}else{
 						strsql = strsql + "sueldo_max = '"+sueldo_maximo_oculta+"', ";
 					}
 					
 					if (sueldo_actual_oculta=="" || sueldo_actual_oculta=="0"){
 						strsql = strsql + "sueldo_actual = '0.0', ";
 					}else{
 						strsql = strsql + "sueldo_actual = '"+sueldo_actual_oculta+"', ";
 					}
 					
 					if (tiempo_vencimiento_contrato=="I")//indeterminado
 					{strsql = strsql + "contrato_indeterminado='true', ";}
 					
 					string fecha_vencimiento_contrato;
 					if (tiempo_vencimiento_contrato=="1")// 1 mes
 					{
 						fecha_vencimiento_contrato = Convert.ToDateTime(anno_contrato_oculta+"/"+mes_contrato_oculta+"/"+dia_contrato_oculta).AddMonths(1).ToShortDateString().ToString();
 					
 						strsql = strsql + "fecha_vencimiento_contrato = '"+Convert.ToDateTime(fecha_vencimiento_contrato).Year.ToString()+"-"+
 																		   Convert.ToDateTime(fecha_vencimiento_contrato).Month.ToString()+"-"+
 																		   Convert.ToDateTime(fecha_vencimiento_contrato).Day.ToString()+"', ";
 																		   
 						strsql = strsql + "contrato_indeterminado='false', ";												   
 																		   
 					}
 					if (tiempo_vencimiento_contrato=="2"){
 						fecha_vencimiento_contrato = Convert.ToDateTime(anno_contrato_oculta+"/"+mes_contrato_oculta+"/"+dia_contrato_oculta).AddMonths(2).ToShortDateString();
 						strsql = strsql + "fecha_vencimiento_contrato = '"+Convert.ToDateTime(fecha_vencimiento_contrato).Year.ToString()+"-"+
 																		   Convert.ToDateTime(fecha_vencimiento_contrato).Month.ToString()+"-"+
 																		   Convert.ToDateTime(fecha_vencimiento_contrato).Day.ToString()+"', ";
 																		   
 						strsql = strsql + "contrato_indeterminado='false', ";													   
 					}
 					if (tiempo_vencimiento_contrato=="3"){
 						fecha_vencimiento_contrato = Convert.ToDateTime(anno_contrato_oculta+"/"+mes_contrato_oculta+"/"+dia_contrato_oculta).AddMonths(3).ToShortDateString();
 						strsql = strsql + "fecha_vencimiento_contrato = '"+Convert.ToDateTime(fecha_vencimiento_contrato).Year.ToString()+"-"+
 																		   Convert.ToDateTime(fecha_vencimiento_contrato).Month.ToString()+"-"+
 																		   Convert.ToDateTime(fecha_vencimiento_contrato).Day.ToString()+"', ";
 																		   
 						strsql = strsql + "contrato_indeterminado='false', ";													   
 					}
 					
 					//estas lineas cambian de tabla
 					//if (historialcontrato != "")
				    //{strsql = strsql + "historial_de_contrato = historial_de_contrato || '"+historialcontrato+"', ";}
					
					//historialcambio = "Actualizo: " + LoginEmpleado + ";El dia: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n";
			      	//strsql = strsql + "historial_de_cambios = historial_de_cambios  || '"+historialcambio+"',  ";

 					strsql = strsql + "departamento =  '"+departamento_oculta+"', ";
					
					strsql = strsql + "tipo_funcion =  '"+tipofuncion_oculta+"', ";
					strsql = strsql + "tiempo_comida =  '"+tiempocomida_oculta+"', ";
					strsql = strsql + "num_lockers =  '"+numlocker_oculta+"', ";
					
					strsql = strsql + "id_departamento =  '"+int.Parse(id_departamento_oculta.Trim())+"', ";
					strsql = strsql + "puesto =  '"+puesto_oculta+"', ";
					strsql = strsql + "id_puesto =  '"+int.Parse(id_puesto_oculta.Trim())+"' ";
					strsql = strsql + "WHERE id_empleado =  '"+entry_id_empleado_etiqueta.Text+"';";
			        
			        comando.CommandText = strsql; 
		 						//"nombre2_empleado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
		 			//Console.WriteLine(strsql+"------------");
					comando.ExecuteNonQuery();
			        comando.Dispose();
			        
		           MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
								MessageType.Info,ButtonsType.Ok,"Se modificó empleado: "+entry_id_empleado_etiqueta.Text+" satisfactoriamente");
					msgBox.Run ();
					msgBox.Destroy();
	       			conexion.Close ();			        
			        protegecampos();
					
			}catch (NpgsqlException ex){
				   	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
			}
			editando = false;
			historialcontrato="";
				
			llena_empleado();
		}
		
		void llena_empleado()
		{
		
		    string micadena = "";
			string dianac = "";
			string mesnac = "";
			string anonac = "";
			string varestadocivil ="";
			
			editando = false;
			//Se busca el empleado introducido en el campo id_empleado
			
			// Buscando el ultimo numero de factura
		 	NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
			
					// asigna el numero de paciente (PID)
				micadena = entry_id_empleado.Text.Trim();
				micadena = micadena.ToUpper(); 
				comando.CommandText = "SELECT m.id_empleado, m.login_empleado, m.nombre1_empleado, m.nombre2_empleado, "+
									  "m.apellido_paterno_empleado, "+
									  "m.apellido_materno_empleado, m.baja_empleado, m.estado_del_empleado, d.calle, d.numero, d.colonia, "+
									  "d.municipio, d.estado, to_char(d.codigo_postal,'99999999999') AS codigopostal, d.telefono_1, "+
									  "d.telefono_2, d.email1, d.email2, d.celular, d.fax, d.rfc, d.curp, d.imss, d.infonavit, "+
									  "d.afore, d.cartilla_militar, "+
						              "d.escolaridad, d.titulo, d.religion, to_char(d.residencia,'99') as residencia1, "+
									  "d.nombre_padre, d.nombre_madre, d.nombre_conyuge, to_char(d.num_hijos,'999') as numhijos, "+
									  "to_char(d.fecha_de_nacimiento,'dd-MM-yyyy HH24:mm') as fechadenacimiento, "+
									  "d.departamento, d.puesto, "+
									  "to_char(d.id_departamento,'9999999') as iddepartamento, "+
									  "to_char(d.id_puesto,'9999999') as idpuesto, "+
									  "to_char(d.fecha_vencimiento_contrato,'dd-MM-yyyy') as fecha_vencimiento, d.tipo_contrato, to_char(d.fecha_de_ingreso,'dd-MM-yyyy HH24:mm') as fechadeingreso, "+
									  "to_char(d.fecha_de_contrato,'dd-MM-yyyy HH24:mm') as fechadecontrato, "+
									  "to_char(d.fecha_de_baja,'dd-MM-yyyy') as fechadebaja, d.estado_civil, d.lugar_nacimiento, "+
									  "d.nacionalidad, d.genero, d.talla_pantalon, d.talla_chaqueta, "+
									  "d.talla_zapatos, to_char(d.peso_empleado,'999.99') as pesoempleado, to_char(d.estatura_empleado, '999.99') as estaturaempleado, "+
									  "d.tipo_sangre, d.accidente_avisar_a, d.accidente_telefono, "+
									  "d.jornada, d.tipo_funcion, d.num_lockers, d.tiempo_comida, d.tipo_pago, to_char(d.sueldo_actual,'99999999.99') as sueldoactual, to_char(d.sueldo_min,'99999999.99') as sueldomin, "+
									  "to_char(d.sueldo_max,'99999999.99') as sueldomax, d.causa_baja, d.notas_baja, d.notas_empleado, "+
									  "d.dias_vac_goce, d.dias_vac_gozados, to_char(d.dias_vac_por_disfrutar,'99999') as diasvacpordisfrutar, "+
									  "d.tipo_casa, d.tipo_empleado "+
									  "FROM osiris_empleado m, osiris_empleado_detalle d " +
									  "WHERE ((m.id_empleado = d.id_empleado) and m.id_empleado = '"+micadena+"' );";
									  
									  //"FROM osiris_empleado m, osiris_empleado_detalle d " +
									  //"WHERE ((osiris_empleado.id_empleado = osiris_empleado_detalle.id_empleado) and osiris_empleado.id_empleado = '"+micadena+"' );";
									  //"WHERE ((m.id_empleado = d.id_empleado) and m.id_empleado = '"+entry_id_empleado.Text.Trim()+"' );";
				Console.WriteLine(comando.CommandText.ToString());					  
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if ((bool) lector.Read()){
					string fechadelabaja = ((string) lector["fechadebaja"]).Substring (0,2);
					string fecha_delabaja = ((string) lector["fechadebaja"]).Substring (3,2);
					string fecha_de_la_baja = ((string) lector["fechadebaja"]).Substring (6,4);
					Console.WriteLine( ((string) lector["fechadebaja"]).Substring (3,2));
					entry_id_empleado_etiqueta.Text = (string) lector["id_empleado"];
					entry_nombre1_empleado.Text = (string) lector["nombre1_empleado"];
		 			entry_nombre2_empleado.Text = (string) lector["nombre2_empleado"];
		 			entry_apellido_paterno_empleado.Text = (string) lector["apellido_paterno_empleado"];
		 			entry_apellido_materno_empleado.Text = (string) lector["apellido_materno_empleado"];
		 			
		 			
		 			if ((string) lector["estado_del_empleado"] == "1"){
		 				checkbutton_empleado_activo.Active = true;		 				
		 			}else{
		 				checkbutton_empleado_activo.Active = false;
		 			}
		 			
		 			if (Convert.ToString((bool) lector["baja_empleado"]) == "True"){
		 				this.entry_estatus_empleado.Text = "Baja";
		 			}else{
		 				this.entry_estatus_empleado.Text = "Activo";
		 			}
		 			
		 			entry_lugar_nac.Text = (string) lector["lugar_nacimiento"];
					entry_nacionalidad.Text = (string) lector["nacionalidad"];
					var_escolaridad = (string) lector["escolaridad"];
					entry_titulo_empleado.Text = (string) lector["titulo"];
					entry_religion_empleado.Text = (string) lector["religion"];
					entry_rfc_empleado.Text = (string) lector["rfc"];
					entry_curp_empleado.Text = (string) lector["curp"];
					entry_imss_empleado.Text = (string) lector["imss"];
					entry_infonavit_empleado.Text = (string) lector["infonavit"];
					entry_afore_empleado.Text = (string) lector["afore"];
					entry_cartilla_militar_empleado.Text = (string) lector["cartilla_militar"];
					entry_talla_pantalon.Text = (string) lector["talla_pantalon"];
					entry_talla_chaqueta.Text = (string) lector["talla_chaqueta"];
					entry_talla_zapatos.Text = (string) lector["talla_zapatos"];
					entry_calle_empleado.Text = (string) lector["calle"];
					entry_numcalle_empleado.Text = (string) lector["numero"];
					entry_codigo_postal_empleado.Text = ((string) lector["codigopostal"]).ToString().Trim();
					entry_colonia_empleado.Text = (string) lector["colonia"];
					entry_peso_empleado.Text =  ((string) lector["pesoempleado"]).ToString().Trim();
					entry_estatura_empleado.Text = (string) (lector["estaturaempleado"]).ToString().Trim();
					entry_tipo_sangre_empleado.Text = (string) lector["tipo_sangre"];
					entry_residencia_empleado.Text = (string) lector["residencia1"];
					entry_nom_padre_empleado.Text = (string) lector["nombre_padre"];
					entry_nom_madre_empleado.Text = (string) lector["nombre_madre"];
					entry_nom_conyuge_empleado.Text = (string) lector["nombre_conyuge"];
					entry_tel1_empleado.Text = (string) lector["telefono_1"];
					entry_tel2_empleado.Text = (string) lector["telefono_2"];
					entry_celular_empleado.Text = (string) lector["celular"];
					entry_email1_empleado.Text = (string) lector["email1"];
					entry_email2_empleado.Text = (string) lector["email2"];
					entry_fax_empleado.Text = (string) lector["fax"];
					entry_avisar_a_empleado.Text = (string) lector["accidente_avisar_a"];
					entry_tel_accidente_empleado.Text = (string) lector["accidente_telefono"];
					entry_notas_empleado.Text = (string) lector["notas_empleado"];
					entry_depto_empleado.Text = (string) lector["departamento"];
					departamento_oculta  = (string) lector["departamento"];
					id_departamento_oculta = (string) lector["iddepartamento"];
					entry_contrato_empleado.Text = (string) lector["tipo_contrato"];
					entry_nombrepuesto_empleado.Text = (string) lector["puesto"];
					id_puesto_oculta= (string) lector["idpuesto"];
					puesto_oculta = (string) lector["puesto"];
					entry_jornada_empleado.Text = (string) lector["jornada"];
					entry_d_disfrute_empleado.Text = (string) lector["diasvacpordisfrutar"];
					
				    //Console.WriteLine(lector["tipo_funcion"]);
				    entry_tipo_funcion.Text = (string) lector["tipo_funcion"];
			        entry_numero_locker.Text = (string) lector["num_lockers"];
		            numlocker_oculta=(string) lector["num_lockers"];
		            entry_tiempo_comida.Text = (string) lector["tiempo_comida"];
					tipofuncion_oculta = (string) lector["tipo_funcion"];
					tiempocomida_oculta = (string) lector["tiempo_comida"];
					
	 				sexopaciente = (string) lector["genero"];
	 				
					if (sexopaciente == "H"){
						radiobutton_masculino_empleado.Active = true;				
					}else{
						radiobutton_femenino_empleado.Active = true;
					}
	 				
	 				entry_dia_nac.Text = ((string) lector["fechadenacimiento"]).Substring (0,2);
	 				entry_mes_nac.Text = ((string) lector["fechadenacimiento"]).Substring (3,2);
	 				entry_anno_nac.Text = ((string) lector["fechadenacimiento"]).Substring (6,4);
	 				
	 				entry_dia_ingreso.Text = ((string) lector["fechadeingreso"]).Substring (0,2);
					entry_mes_ingreso.Text = ((string) lector["fechadeingreso"]).Substring (3,2);
					entry_anno_ingreso.Text = ((string) lector["fechadeingreso"]).Substring (6,4);
					
	 				//relleno de tipo de empleado
	 				combobox_tipo_empleado.Clear();
	 				CellRendererText cell33 = new CellRendererText();
					combobox_tipo_empleado.PackStart(cell33, true);
					combobox_tipo_empleado.AddAttribute(cell33,"text",0);
	        
					ListStore store33 = new ListStore( typeof (string));
					combobox_tipo_empleado.Model = store33;
	        		
					store33.AppendValues ((string) lector["tipo_empleado"]);
	 				var_tipo_empleado = ((string) lector["tipo_empleado"]);
	 				
	 				TreeIter iter33;
					if (store33.GetIterFirst(out iter33)){
						combobox_tipo_empleado.SetActiveIter (iter33);
					}
					
					//Para llenar combo con datos DE escolaridad
					combobox_escolaridad_empleado.Clear();
					CellRendererText cell34 = new CellRendererText();
					combobox_escolaridad_empleado.PackStart(cell34, true);
					combobox_escolaridad_empleado.AddAttribute(cell34,"text",0);
	        
					ListStore store34 = new ListStore( typeof (string));
					combobox_escolaridad_empleado.Model = store34;
					
			        store34.AppendValues ((string) lector["escolaridad"]);
			        
			        
					TreeIter iter34;
					
					if (store34.GetIterFirst(out iter34)){
						combobox_escolaridad_empleado.SetActiveIter (iter34);
					}
			        //Fin para cerrar combo de escolaridad				
					
	 				//Para llenar combo con datos DE ESTADO CIVIL
					combobox_estado_civil.Clear();
					CellRendererText cell1 = new CellRendererText();
					combobox_estado_civil.PackStart(cell1, true);
					combobox_estado_civil.AddAttribute(cell1,"text",0);
	        
					ListStore store1 = new ListStore( typeof (string));
					combobox_estado_civil.Model = store1;
					
			        store1.AppendValues ((string) lector["estado_civil"]);
			        tmp_estado_civil = ((string) lector["estado_civil"]);
			        
					TreeIter iter1;
					
					if (store1.GetIterFirst(out iter1)){
						combobox_estado_civil.SetActiveIter (iter1);
					}
			        //Fin para cerrar combo de ESTADO CIVIL						
					
	 				//Para llenar combo con datos DE TIPO DE CASA
					combobox_casa_empleado.Clear();
					CellRendererText cell2 = new CellRendererText();
					combobox_casa_empleado.PackStart(cell2, true);
					combobox_casa_empleado.AddAttribute(cell2,"text",0);
	        
					ListStore store2 = new ListStore( typeof (string));
					combobox_casa_empleado.Model = store2;
					
			        store2.AppendValues ((string) lector["tipo_casa"]);
			        var_tipo_casa = ((string) lector["tipo_casa"]);
										
					TreeIter iter2;
					if (store2.GetIterFirst(out iter2)){
						combobox_casa_empleado.SetActiveIter (iter2);
					}
					//combobox_estado_civil.Changed += new EventHandler (onComboBoxChanged_estado_civil);  // activa casilla de 
			        //Fin para cerrar combo de TIPO DE CASA
	 				
	 				//Para llenar combo con datos DE NUMERO DE HIJOS
					combobox_num_hijos_empleado.Clear();
					CellRendererText cell22 = new CellRendererText();
					combobox_num_hijos_empleado.PackStart(cell22, true);
					combobox_num_hijos_empleado.AddAttribute(cell22,"text",0);
	        
					ListStore store22 = new ListStore( typeof (string));
					combobox_num_hijos_empleado.Model = store22;
					
			        store22.AppendValues ((string) lector["numhijos"]);
			        var_num_hijos = ((string) lector["numhijos"]);
										
					TreeIter iter22;
					if (store22.GetIterFirst(out iter22)){
						combobox_num_hijos_empleado.SetActiveIter (iter22);
					}
	 				
	 				//RELLENO DEL COMBOBOX PARA ESTADOS y municipios////////////////////
	 				
	 				// Llenado de Estados
					estado = (string) lector["estado"];
					
					combobox_estado.Clear();
					CellRendererText cell24 = new CellRendererText();
					combobox_estado.PackStart(cell24, true);
					combobox_estado.AddAttribute(cell24,"text",0);
					
					ListStore store24 = new ListStore( typeof (string),typeof (int));
					combobox_estado.Model = store24;
					
					store24.AppendValues ((string) estado, 0);
	        		tmp_estado = estado;
	        		// ||||||||||||||estado_a_guardar = estado;
					estado_a_guardar = estado;
					
					TreeIter iter24;
					if (store24.GetIterFirst(out iter24)){
						combobox_estado.SetActiveIter (iter24);
					}
	 				
	 				combobox_municipios.Clear();
	 				municipios = (string) lector["municipio"];
	 				
					CellRendererText cell23 = new CellRendererText();
					combobox_municipios.PackStart(cell23, true);
					combobox_municipios.AddAttribute(cell23,"text",0);
	        
					ListStore store23 = new ListStore( typeof (string));
					combobox_municipios.Model = store23;
	        		
					store23.AppendValues ((string) municipios);
					
	        		tmp_municipios = municipios;
	        		// ||||||||||||||||municipios_a_guardar = municipios;
	        		municipios_a_guardar = municipios;
	        		
					TreeIter iter23;
					if (store23.GetIterFirst(out iter23)){
						combobox_municipios.SetActiveIter (iter23);
					}
					
					//SE LLENAN LAS VARIABLES OCULTAS
					dia_contrato_oculta = ((string) lector["fechadecontrato"]).Substring (0,2);
					mes_contrato_oculta = ((string) lector["fechadecontrato"]).Substring (3,2);
					anno_contrato_oculta = ((string) lector["fechadecontrato"]).Substring (6,4);
					
					//SE LLENAN LAS VARIABLES OCULTAS
					//dia_baja_oculta = ((string) lector["fechadecontrato"]).Substring (0,2);
					//mes_baja_oculta = ((string) lector["fechadecontrato"]).Substring (3,2);
					//anno_baja_oculta = ((string) lector["fechadecontrato"]).Substring (6,4);
					
					
					
					if (((string) lector["fechadecontrato"]).Substring (0,10) != "01-01-2000"){
					this.entry_dia_venc_cont.Text = ((string) lector["fecha_vencimiento"]).Substring (0,2);
	 				this.entry_mes_venc_cont.Text = ((string) lector["fecha_vencimiento"]).Substring (3,2);
	 				this.entry_anno_venc_cont.Text = ((string) lector["fecha_vencimiento"]).Substring (6,4);
	 				}else{
	 				this.entry_dia_venc_cont.Text ="";
	 				this.entry_mes_venc_cont.Text = "";
	 				this.entry_anno_venc_cont.Text ="";}
					
					this.entry_cont_aa.Text = anno_contrato_oculta;
                    this.entry_cont_mm.Text = mes_contrato_oculta;
                    this.entry_cont_dd.Text = dia_contrato_oculta;
					
					//fecha baja
					this.entry_baja_dd.Text = fechadelabaja;
					this.entry_baja_mes.Text = fecha_delabaja;
					this.entry_baja_aa.Text = fecha_de_la_baja;
					
					
					sueldo_minimo_oculta = (string) lector["sueldomin"];
					sueldo_maximo_oculta = (string) lector["sueldomax"];
					sueldo_actual_oculta = (string) lector["sueldoactual"];
					
					//sueldo_minimo_oculta = (((string) lector["sueldomin"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")));
					//sueldo_maximo_oculta = (((string) lector["sueldomax"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")));
					//sueldo_actual_oculta = (((string) lector["sueldoactual"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")));
					////subtotal_al_0.ToString("C").PadLeft(10);
					
					tipo_contrato_oculta  = ((string) lector["tipo_contrato"]);
					tipo_pago_oculta  = ((string) lector["tipo_pago"]);
					jornada_oculta  = ((string) lector["jornada"]);
							
		 		}
		 		//entry_id_empleado.Text = "";
		 		button_editar_empleado.Sensitive = true;
		 		conexion.Close();
		 		
		 		if (entry_anno_nac.Text != ""){
		 			edadanno =  DateTime.Now.Year - int.Parse(entry_anno_nac.Text);
		 		    if (int.Parse(entry_mes_nac.Text) < DateTime.Now.Month)
		 		          {edadanno = DateTime.Now.Year - int.Parse(entry_anno_nac.Text);}
		 		    if (int.Parse(entry_mes_nac.Text) > DateTime.Now.Month)
		 		          {edadanno = edadanno -1 ; }
		 		    if (int.Parse(entry_mes_nac.Text) == DateTime.Now.Month)
		 		           {
		 		          		 		if (int.Parse(entry_dia_nac.Text) < DateTime.Now.Day)
		 		                          { edadanno = edadanno -1 ; }
		 		           }
		 		 this.entry_edad.Text = edadanno + " años" ;
		 		
		 		}		 
		 		
		 	}catch (NpgsqlException ex){			
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, 
						ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			
			if (micadena == this.entry_id_empleado_etiqueta.Text){
				//Console.WriteLine (micadena);
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
													MessageType.Info,ButtonsType.Ok,"El ID: {0} Es Incorrecto", micadena);
				msgBox.Run();		msgBox.Destroy();
				limpiacampos();
				this.entry_id_empleado.Text = "";
				this.entry_id_empleado_etiqueta.Text = "";
			}
		}

		
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////|
///////// DEFINICION DE EVENTOS //////////////////////////////////////////////////////////////////////|
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////|		
    	// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		void on_button_actualiza_clicked (object sender, EventArgs args)
		{
		
		 TreeIter iterSelected;
		
		this.combobox_departamento_empleado.GetActiveIter(out iterSelected);
		string nueva_departamento = ((ListStore) combobox_departamento_empleado.Model).GetValue (iterSelected, 0).ToString();
		this.combobox_tipo_contrato.GetActiveIter(out iterSelected);
		string nueva_tipo_contrato = ((ListStore) combobox_tipo_contrato.Model).GetValue (iterSelected, 0).ToString();
		this.combobox_puesto_empleado.GetActiveIter(out iterSelected);
		string nueva_puesto = ((ListStore)combobox_puesto_empleado.Model).GetValue (iterSelected, 0).ToString();
		this.combobox_tiempo_comida_empleado.GetActiveIter(out iterSelected);
		string nueva_tiempocomida = ((ListStore)combobox_tiempo_comida_empleado.Model).GetValue (iterSelected, 0).ToString();
		this.combobox_tipo_pago_empleado.GetActiveIter(out iterSelected);
		string nueva_tipo_pago = ((ListStore)combobox_tipo_pago_empleado.Model).GetValue (iterSelected, 0).ToString();
		this.combobox_jornada_empleado.GetActiveIter(out iterSelected);
		string nueva_jornada = ((ListStore)combobox_jornada_empleado.Model).GetValue (iterSelected, 0).ToString();				  
		this.combobox_funcion_empleado.GetActiveIter(out iterSelected);
		string nueva_tipofuncion = ((ListStore)combobox_funcion_empleado.Model).GetValue (iterSelected, 0).ToString();
		
		if (nueva_puesto  == "" || 
		   	nueva_departamento == "" ||
			nueva_tipofuncion == "" || 
			nueva_tiempocomida == "" ||
			nueva_tipo_contrato == "" ||
			nueva_jornada == "" ||
			nueva_tipo_pago == "")
			{
		   
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Error,ButtonsType.Ok,"favor de llenar todos los datos");
			msgBox.Run();	msgBox.Destroy();
		
		}else{
			sueldo_minimo_oculta = entry_sueldo_min_empleado.Text;
		    sueldo_maximo_oculta = entry_sueldo_max_empleado.Text;
		    sueldo_actual_oculta = entry_sueldo_actual_empleado.Text;	
		    dia_contrato_oculta = this.entry_dia_contrato.Text;
		    mes_contrato_oculta = this.entry_mes_contrato.Text;
			anno_contrato_oculta = this.entry_anno_contrato.Text;
			 
			departamento_oculta = nueva_departamento;
			puesto_oculta = nueva_puesto;
			tipofuncion_oculta = nueva_tipofuncion;
			tiempocomida_oculta = nueva_tiempocomida;
			tipo_contrato_oculta = nueva_tipo_contrato;
			jornada_oculta = nueva_jornada;
			tipo_pago_oculta = nueva_tipo_pago;
			  
			numlocker_oculta = this.entry_lockers.Text;
			if (numlocker_oculta==""){numlocker_oculta="0";}
			 
			Widget win = (Widget) sender;
		    win.Toplevel.Destroy();
		    this.checkbutton_empleado_activo.Active = true;
		    //historialcontrato = ";Registro: " + LoginEmpleado + ";El dia: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm")+ ";Fecha de contrato: "+  
		    //anno_contrato_oculta + "/"+  mes_contrato_oculta+ "/"+  dia_contrato_oculta   + ";Tipo de contrato: " + tipo_contrato_oculta  +"\n";
		    historialcontrato = ";" + LoginEmpleado + ";" + DateTime.Now.ToString("yyyy-MM-dd HH:mm")+ "; "+  anno_contrato_oculta + "/"+  mes_contrato_oculta+ "/"+  dia_contrato_oculta   +
		      ";" + tipo_contrato_oculta  + ";" + sueldo_actual_oculta+ ";"+ departamento_oculta +";"+puesto_oculta+";"+jornada_oculta+";"+tipofuncion_oculta+
		      ";"+tipo_pago_oculta+";"+tiempocomida_oculta+";"+numlocker_oculta+";"+edadanno+"\n";
		      //Console.WriteLine(historialcontrato);
		    if (tipo_contrato_oculta == "INDETERMINADO"){
				tiempo_vencimiento_contrato="I";
			}else{
		      	tiempo_vencimiento_contrato = tipo_contrato_oculta.Substring( tipo_contrato_oculta.IndexOf("(")+1 , 1 );
			}
		}
	}
	   
		void on_checkbutton_empleado_activo_clicked(object sender, EventArgs args)
		{
			if (checkbutton_empleado_activo.Active == false){
	   			this.button_contrato_empleado.Sensitive = true;
	   		}else{
	   			this.button_contrato_empleado.Sensitive = false;}
	    }
	    	
		void on_checkbutton_empleado_nuevo_clicked(object sender, EventArgs args)
		{
			entry_dia_ingreso.Sensitive = false;
			entry_mes_ingreso.Sensitive = false;
			entry_anno_ingreso.Sensitive = false;
						
			if (checkbutton_empleado_nuevo.Active == true){					
		 		protegecampos();
				//abrecampos();
				limpiacampos();
				//falta llenar los combos
				var_tipo_empleado = "";
				llenado_tipo_empleado("");
				entry_id_empleado_etiqueta.Text = "EMPLEADO NUEVO";
				
				this.entry_anno_ingreso.Text = DateTime.Now.ToString("yyyy");
				this.entry_mes_ingreso.Text = DateTime.Now.ToString("MM"); 
	    		this.entry_dia_ingreso.Text = DateTime.Now.ToString("dd");
	    		
				llenado_estadocivil("");
				llenado_tipo_casa("");
				llenado_num_hijos("");
				llenado_escolaridad_empleado("");
				llenado_estados("",0);	
				combobox_municipios.Clear();

				departamento_oculta="";
				puesto_oculta="";
				id_departamento_oculta="";
				id_puesto_oculta="";
				tipofuncion_oculta="";
				tiempocomida_oculta = "";
				numlocker_oculta="";
				
				this.entry_id_empleado.Text = "";
				this.button_baja_empleado.Sensitive = false;
				this.button_editar_empleado.Sensitive = false;
				this.button_contrato_empleado.Sensitive =false;
				//this.entry_id_empleado.Editable = false;
				this.combobox_tipo_empleado.Sensitive = true;
				this.button_buscar_empleado.Sensitive = false;
				this.button_sel.Sensitive = false;
				this.radiobutton_femenino_empleado.Sensitive = false;
				this.radiobutton_masculino_empleado.Sensitive = false;
				//this.combobox_tipo_empleado.Focused = true;
				
			}else{
			
			    this.button_buscar_empleado.Sensitive = true;
				this.button_sel.Sensitive = true;
				//this.entry_id_empleado.Editable = true;
				this.entry_id_empleado.Text = "";
				entry_id_empleado_etiqueta.Text = "";
				limpiacampos();
				protegecampos();	
			
			}
		}
		
		void on_button_editar_empleado_clicked (object sender, EventArgs args)
		{
		    if (this.entry_id_empleado_etiqueta.Text == "" )
		       {
				       	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, 
						ButtonsType.Close,"Por Favor Ingrese un ID correcto");
						msgBoxError.Run ();
						msgBoxError.Destroy();
					this.entry_id_empleado.Text = "";
					
		       }
		      else
		 	   {edita_empleado();}
		}
		
		void on_button_guarda_empleado_clicked (object sender, EventArgs args)
		{
		
			TreeIter iterSelected;
			this.combobox_num_hijos_empleado.GetActiveIter(out iterSelected);
			string nueva_numhijos = ((ListStore) this.combobox_num_hijos_empleado.Model).GetValue (iterSelected, 0).ToString();
			this.combobox_casa_empleado.GetActiveIter(out iterSelected);
			string nueva_casa = ((ListStore) this.combobox_casa_empleado.Model).GetValue (iterSelected, 0).ToString();
			this.combobox_estado_civil.GetActiveIter(out iterSelected);
			string nueva_estadocivil = ((ListStore) this.combobox_estado_civil.Model).GetValue (iterSelected, 0).ToString();
			this.combobox_estado.GetActiveIter(out iterSelected);
			string nueva_estado = ((ListStore) this.combobox_estado.Model).GetValue (iterSelected, 0).ToString();
			this.combobox_escolaridad_empleado.GetActiveIter(out iterSelected);
			string nueva_escolaridad = ((ListStore) this.combobox_escolaridad_empleado.Model).GetValue (iterSelected, 0).ToString();
		 		 
			if (nueva_numhijos == "" ||
	            nueva_casa == "" ||
	            nueva_estadocivil == "" ||
	            nueva_estado == "" ||		  
	          	nueva_escolaridad == "" ||	  		 		 
				this.entry_nombre1_empleado.Text == "" ||
				this.entry_apellido_paterno_empleado.Text == "" ||
				this.entry_apellido_materno_empleado.Text == "" ||
				this.entry_anno_ingreso.Text == "" ||
				this.entry_mes_ingreso.Text == "" ||
				this.entry_dia_ingreso.Text == "" ||
				this.entry_anno_nac.Text == "" ||
				this.entry_mes_nac.Text == "" ||
				this.entry_dia_nac.Text == "" ||
				this.entry_lugar_nac.Text == "" ||
				this.entry_nacionalidad.Text == "" ||
				this.entry_calle_empleado.Text == "" ||
				this.entry_numcalle_empleado.Text == "" ||
				this.entry_colonia_empleado.Text == "" ){
           
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close," Favor de llenar toda la informaciòn correspondiente ");
					msgBoxError.Run ();				msgBoxError.Destroy();
           
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de GUARDAR este registro ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 			
				if (miResultado == ResponseType.Yes){
					if (checkbutton_empleado_nuevo.Active == false){
	 				//Si ya existe el empleado, se actualiza
					actualiza_empleado();
					
				}else{
					//si es nuevo se ejecuta guardar_empleado
					
					//PREGUNTAR EL TIPO DE EMPLEADO QUE SE VA A CAPTURAR (ES POR TIPO DE EMPLEADO?)
					guardar_empleado();	
					//protegecampos();
				}
          	}else{
          	  // no hace nada cierra la ventana
			 	
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			}		
		}				
							
		}
		
		
		void llena_combo_tiempo_comida(string activo)
		{
			//COMBO TIPO DE PAGO
			combobox_tiempo_comida_empleado.Clear();
			CellRendererText cell14 = new CellRendererText();
			combobox_tiempo_comida_empleado.PackStart(cell14, true);
			combobox_tiempo_comida_empleado.AddAttribute(cell14,"text",0);
   
			ListStore store14 = new ListStore( typeof (string));
			combobox_tiempo_comida_empleado.Model = store14;
			if (activo != "")
			{
				store14.AppendValues (activo);
			}
			
	        store14.AppendValues ((string) "30 minutos");
	        store14.AppendValues ((string) "1 hora");
	        store14.AppendValues ((string) "2 horas");
	        store14.AppendValues ((string) "Sin hora");
	        
	        
	        
	        
			TreeIter iter14;
			if (store14.GetIterFirst(out iter14)){
				combobox_tiempo_comida_empleado.SetActiveIter (iter14);
			}
			combobox_tiempo_comida_empleado.Changed += new EventHandler (onComboBoxChanged_tiempo_comida);

		}
		
		void onComboBoxChanged_tiempo_comida (object sender, EventArgs args)
		{
			string fechas="";
			ComboBox combobox_tiempo_comida_empleado = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_tiempo_comida_empleado.GetActiveIter (out iter)) {
				tiempo_de_comida = ((string) this.combobox_tiempo_comida_empleado.Model.GetValue(iter,0));
				tiempocomida_oculta = tiempo_de_comida;
				

			}
		}
		
		void llena_combo_tipo_funcion(string activo)
		{
			//COMBO TIPO DE PAGO
			combobox_funcion_empleado.Clear();
			CellRendererText cell13 = new CellRendererText();
			combobox_funcion_empleado.PackStart(cell13, true);
			combobox_funcion_empleado.AddAttribute(cell13,"text",0);
   
			ListStore store13 = new ListStore( typeof (string));
			combobox_funcion_empleado.Model = store13;
			if (activo != "")
			{
				store13.AppendValues (activo);
			}
			
	        store13.AppendValues ((string) "ADMINISTRATIVA");
	        store13.AppendValues ((string) "OPERATIVA");
	        store13.AppendValues ((string) "ENFERMERIA");
	        store13.AppendValues ((string) "ADMINISTRATIVA Y OPERATIVA");
	        
	        
	        
	        
			TreeIter iter13;
			if (store13.GetIterFirst(out iter13)){
				combobox_funcion_empleado.SetActiveIter (iter13);
			}
			combobox_funcion_empleado.Changed += new EventHandler (onComboBoxChanged_tipo_funcion);

		}
		
		void onComboBoxChanged_tipo_funcion (object sender, EventArgs args)
		{
			string fechas="";
			ComboBox combobox_funcion_empleado = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_funcion_empleado.GetActiveIter (out iter)) {
				funcion_de_empleado = ((string) this.combobox_funcion_empleado.Model.GetValue(iter,0));
				//Console.WriteLine(funcion_de_empleado);
				tipofuncion_oculta = funcion_de_empleado;
				

			}
		}
		
		

		void llena_combo_jornada(string activo)
		{
			//COMBO JORNADA DE TRABAJO
			combobox_jornada_empleado.Clear();
			CellRendererText cell12 = new CellRendererText();
			combobox_jornada_empleado.PackStart(cell12, true);
			combobox_jornada_empleado.AddAttribute(cell12,"text",0);
   
			ListStore store12 = new ListStore( typeof (string));
			combobox_jornada_empleado.Model = store12;
			
			if (activo != "")
			{
				store12.AppendValues (activo);
			}
	        store12.AppendValues ((string) "3 hrs");
	        store12.AppendValues ((string) "4 hrs");
	        store12.AppendValues ((string) "5 hrs");
	        store12.AppendValues ((string) "6 1/2 hrs");
	        store12.AppendValues ((string) "7 hrs");
	        store12.AppendValues ((string) "7 1/2 hrs");
	        store12.AppendValues ((string) "8 hrs");
	        store12.AppendValues ((string) "12 hrs");
	        
			TreeIter iter12;
			if (store12.GetIterFirst(out iter12)){
				combobox_jornada_empleado.SetActiveIter (iter12);
			}
			combobox_jornada_empleado.Changed += new EventHandler (onComboBoxChanged_jornada);
		}
		
		void onComboBoxChanged_jornada (object sender, EventArgs args)
		{
			string fechas="";
			ComboBox combobox_jornada_empleado = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_jornada_empleado.GetActiveIter (out iter)) {
				jornada_oculta = ((string) combobox_jornada_empleado.Model.GetValue(iter,0));
				
				fechas = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
				
			}
		}

		void llena_combo_tipo_pago(string activo)
		{
			//COMBO TIPO DE PAGO
			combobox_tipo_pago_empleado.Clear();
			CellRendererText cell11 = new CellRendererText();
			combobox_tipo_pago_empleado.PackStart(cell11, true);
			combobox_tipo_pago_empleado.AddAttribute(cell11,"text",0);
   
			ListStore store11 = new ListStore( typeof (string));
			combobox_tipo_pago_empleado.Model = store11;
			if (activo != "")
			{
				store11.AppendValues (activo);
			}
	        store11.AppendValues ((string) "SINDICALIZADO (SEMANAL)");
	        store11.AppendValues ((string) "ADMINISTRATIVO (SEMANAL)");
	        store11.AppendValues ((string) "ADMINISTRATIVO (QUINCENAL)");
	        store11.AppendValues ((string) "PRACTICAS");
	        
	        
			TreeIter iter11;
			if (store11.GetIterFirst(out iter11)){
				combobox_tipo_pago_empleado.SetActiveIter (iter11);
			}
			combobox_tipo_pago_empleado.Changed += new EventHandler (onComboBoxChanged_tipo_pago);

		}
		
		void onComboBoxChanged_tipo_pago (object sender, EventArgs args)
		{
			ComboBox combobox_tipo_pago_empleado = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_tipo_pago_empleado.GetActiveIter (out iter)) {
				tipo_pago_oculta = ((string) combobox_tipo_pago_empleado.Model.GetValue(iter,0));
				
				string fechas = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
				//entry_dia_contrato.Text = fechas.Substring(0,2);
				//entry_mes_contrato.Text = fechas.Substring(3,2); 
				//entry_anno_contrato.Text = fechas.Substring(6,4);
	            //dia_contrato_oculta = fechas.Substring(0,2);
				//mes_contrato_oculta = fechas.Substring(3,2);
				//anno_contrato_oculta = fechas.Substring(6,4);
			}
		}		
						
										
		void llena_combo_tipo_contrato(string activo)
		{
			combobox_tipo_contrato.Clear();
			CellRendererText cell10 = new CellRendererText();
			combobox_tipo_contrato.PackStart(cell10, true);
			combobox_tipo_contrato.AddAttribute(cell10,"text",0);
   
			ListStore store10 = new ListStore( typeof (string));
			combobox_tipo_contrato.Model = store10;
			
			if (activo != "")
			{
				store10.AppendValues (activo);
			}
	        store10.AppendValues ((string) "DETERMINADO (1 MES)");
	        store10.AppendValues ((string) "DETERMINADO (2 MESES)");
	        store10.AppendValues ((string) "DETERMINADO (3 MESES)");
	        store10.AppendValues ((string) "INDETERMINADO");
	        store10.AppendValues ((string) "HONORARIOS (ASIMILABLES)");
	        store10.AppendValues ((string) "PRACTICAS");
	        
			TreeIter iter10;
			if (store10.GetIterFirst(out iter10)){
				combobox_tipo_contrato.SetActiveIter (iter10);
			}
			combobox_tipo_contrato.Changed += new EventHandler (onComboBoxChanged_tipo_contrato);
		}
				
		void onComboBoxChanged_tipo_contrato (object sender, EventArgs args)
		{
			string fechas="";
			ComboBox combobox_tipo_contrato = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_tipo_contrato.GetActiveIter (out iter)) {
				tipo_contrato_oculta = ((string) combobox_tipo_contrato.Model.GetValue(iter,0));
				fechas = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
    			dia_contrato_oculta = fechas.Substring(0,2);
				mes_contrato_oculta = fechas.Substring(3,2);
				anno_contrato_oculta = fechas.Substring(6,4);
				
				
				
			}
		}
		
		
		void on_button_contrato_empleado_clicked (object sender, EventArgs args)
		{
			//Si se usaran clases....
			//new osiris.contratopersonal(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
			//editando
			Glade.XML gxml = new Glade.XML (null, "recursos_humanos.glade", "contrato_empleado", null);
			gxml.Autoconnect (this);
			contrato_empleado.Show();
						
			//boton actualizar
			button_actualiza.Clicked += new EventHandler(on_button_actualiza_clicked);
			
			
			// Validando que solo se escriben numeros
			this.entry_sueldo_min_empleado.KeyPressEvent += onKeyPressEventactual;
			this.entry_sueldo_max_empleado.KeyPressEvent += onKeyPressEventactual;
			this.entry_sueldo_actual_empleado.KeyPressEvent += onKeyPressEventactual;			
			this.entry_dia_contrato.KeyPressEvent += onKeyPressEventactual;
	   		this.entry_mes_contrato.KeyPressEvent += onKeyPressEventactual;
	   		this.entry_anno_contrato.KeyPressEvent += onKeyPressEventactual;
	   	
			if (entry_id_empleado_etiqueta.Text == "EMPLEADO NUEVO" )
			{
				button_actualiza.Sensitive = true;
				//entry_dia_contrato.Sensitive = false;
				//entry_mes_contrato.Sensitive = false;
				//entry_anno_contrato.Sensitive = false;
				//En el caso de ser nuevo, se muestran todos los combos
			
				//combo TIPO DE CONTRATO
				llena_combo_tipo_contrato("");
				llena_combo_tipo_pago("");
				llena_combo_jornada("");
				llena_combo_tipo_funcion("");
				llena_combo_tiempo_comida("");

				entry_id_depto.Sensitive = false;
				entry_id_puesto.Sensitive = false;
				//llena combo para departamento
				combo_deptos_contrato("","");
				entry_id_depto.Text = id_departamento_oculta;
				//llena combo para puesto
				combo_puesto_contrato("","");
				entry_id_puesto.Text = id_puesto_oculta;
				
				sueldo_minimo_oculta = "0"; 
    			sueldo_maximo_oculta = "0";
		    	sueldo_actual_oculta = "0";

			}else{
				//cuando ya existe el empleado
				
				if (editando == true){
					button_actualiza.Sensitive = true;
					combobox_tipo_contrato.Sensitive = true;
					combobox_tipo_pago_empleado.Sensitive = true;
					combobox_jornada_empleado.Sensitive = true;
					entry_dia_contrato.Sensitive = true;
					entry_mes_contrato.Sensitive = true;
					entry_anno_contrato.Sensitive = true;
					entry_sueldo_min_empleado.Sensitive = true;
					entry_sueldo_max_empleado.Sensitive = true;
					entry_sueldo_actual_empleado.Sensitive = true;
					entry_id_depto.Sensitive = false;
					entry_id_puesto.Sensitive = false;
					combobox_departamento_empleado.Sensitive = true;
					combobox_puesto_empleado.Sensitive = true;
				}else{
					button_actualiza.Sensitive = false;
					combobox_tipo_contrato.Sensitive = false;
					combobox_tipo_pago_empleado.Sensitive = false;
					combobox_jornada_empleado.Sensitive = false;
					entry_dia_contrato.Sensitive = false;
					entry_mes_contrato.Sensitive = false;
					entry_anno_contrato.Sensitive = false;
					entry_sueldo_min_empleado.Sensitive = false;
					entry_sueldo_max_empleado.Sensitive = false;
					entry_sueldo_actual_empleado.Sensitive = false;
					entry_id_depto.Sensitive = false;
					entry_id_puesto.Sensitive = false;
					combobox_departamento_empleado.Sensitive = false;
					combobox_puesto_empleado.Sensitive = false;
				}
				
					//entry_dia_contrato.Text = dia_contrato_oculta;
					//entry_mes_contrato.Text = mes_contrato_oculta;
					//entry_anno_contrato.Text = anno_contrato_oculta;
				
								//fechas = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
				entry_dia_contrato.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Substring(0,2);
				entry_mes_contrato.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Substring(3,2); 
				entry_anno_contrato.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Substring(6,4);
				
					entry_sueldo_min_empleado.Text = sueldo_minimo_oculta;
					entry_sueldo_max_empleado.Text = sueldo_maximo_oculta;
					entry_sueldo_actual_empleado.Text = sueldo_actual_oculta;
					
					//llena combo para tipo de contrato, tipo de pago y jornada
					
					llena_combo_tipo_contrato(tipo_contrato_oculta);
					llena_combo_tipo_pago(tipo_pago_oculta);
					llena_combo_jornada(jornada_oculta);
					llena_combo_tiempo_comida(tiempocomida_oculta);
					llena_combo_tipo_funcion(tipofuncion_oculta);
					
				    //llena combo para departamento
					combo_deptos_contrato(departamento_oculta, id_departamento_oculta);
					entry_id_depto.Text = id_departamento_oculta;

					//llena combo para puesto
					combo_puesto_contrato(puesto_oculta, id_puesto_oculta);
					entry_id_puesto.Text = id_puesto_oculta;
	                entry_sueldo_min_empleado.Text =  sueldo_minimo_oculta; 
    				entry_sueldo_max_empleado.Text = sueldo_maximo_oculta;
    				entry_sueldo_actual_empleado.Text = sueldo_actual_oculta;
                    // numero de lokers
                    entry_lockers.Text = numlocker_oculta;
               

			}
			
		
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
		}
		
		void combo_deptos_contrato(string activo, string idactivo)
		{
				//COMBO DEPARTAMENTO
                combobox_departamento_empleado.Clear();
				CellRendererText cell3 = new CellRendererText();
				combobox_departamento_empleado.PackStart(cell3, true);
				combobox_departamento_empleado.AddAttribute(cell3,"text",0);
		        
				ListStore store3 = new ListStore( typeof (string),typeof (int));
				combobox_departamento_empleado.Model = store3;
		        
		        NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
	            // Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
	               	comando.CommandText = "SELECT * FROM osiris_his_tipo_admisiones WHERE grupo in ('ADM', 'MED') ORDER BY grupo, descripcion_admisiones";
	               	//"WHERE id_estado = '"+idestado.ToString()+"' "+
	     	
					NpgsqlDataReader lector = comando.ExecuteReader ();
					if (activo != "")
						{
						store3.AppendValues (activo,Convert.ToInt32(idactivo));
						}
					else
					{store3.AppendValues ("",0);}
	               	while (lector.Read())
					{
						store3.AppendValues ((string) lector["descripcion_admisiones"],(int) lector["id_tipo_admisiones"]);
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
				conexion.Close ();
		        
				TreeIter iter3;
				if (store3.GetIterFirst(out iter3))	
					{ combobox_departamento_empleado.SetActiveIter (iter3); }
				
				combobox_departamento_empleado.Changed += new EventHandler (onComboBoxChanged_departamento);
		}
		
		void onComboBoxChanged_departamento (object sender, EventArgs args)
		{
			ComboBox combobox_departamento_empleado = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_departamento_empleado.GetActiveIter (out iter)) {
				entry_id_depto.Text = Convert.ToString((int) combobox_departamento_empleado.Model.GetValue(iter,1));
				id_departamento_oculta = Convert.ToString((int) combobox_departamento_empleado.Model.GetValue(iter,1));
				departamento_oculta = ((string) combobox_departamento_empleado.Model.GetValue(iter,0));
			}
		}
		
		void combo_puesto_contrato(string activo, string idactivo)
		{
				//COMBO puesto
				combobox_puesto_empleado.Clear();
				CellRendererText cell3 = new CellRendererText();
				combobox_puesto_empleado.PackStart(cell3, true);
				combobox_puesto_empleado.AddAttribute(cell3,"text",0);
		        
				ListStore store3 = new ListStore( typeof (string),typeof (int));
				combobox_puesto_empleado.Model = store3;
		        
		        NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
	            // Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
	               	comando.CommandText = "SELECT * FROM osiris_erp_puestos WHERE activo = TRUE ORDER BY puesto";
	               	//"WHERE id_estado = '"+idestado.ToString()+"' "+
	               					
					NpgsqlDataReader lector = comando.ExecuteReader ();
					
					if (activo != ""){
						store3.AppendValues (activo,Convert.ToInt32(idactivo));
					}else{
						store3.AppendValues ("",0);
					}
	               	while (lector.Read())
					{
						store3.AppendValues ((string) lector["puesto"],(int) lector["id_puesto"]);
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
				conexion.Close ();
		        
				TreeIter iter3;
				if (store3.GetIterFirst(out iter3))	
					{ combobox_puesto_empleado.SetActiveIter (iter3); }
				
				combobox_puesto_empleado.Changed += new EventHandler (onComboBoxChanged_puesto);
		}

		void onComboBoxChanged_puesto (object sender, EventArgs args)
		{
			ComboBox combobox_puestos_empleado = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_puesto_empleado.GetActiveIter (out iter)) {	
				 entry_id_puesto.Text = Convert.ToString((int) combobox_puesto_empleado.Model.GetValue(iter,1));
				 id_puesto_oculta = Convert.ToString((int) combobox_puesto_empleado.Model.GetValue(iter,1));
				 puesto_oculta = ((string) combobox_puesto_empleado.Model.GetValue(iter,0));
			}
		}
				
		void on_button_vacaciones_empleado_clicked (object sender, EventArgs args)
		{
		new osiris.vacacionespersonal(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_imprimir_clicked (object sender, EventArgs args)
		{
		
		Glade.XML gxml = new Glade.XML (null, "recursos_humanos.glade", "imp_cont_o_reg_alta", null);
		gxml.Autoconnect (this);
	        
			// Muestra ventana de Glade
			catalogo_empleado.Show();
			
	    this.entry_nomb_imp.Text = this.entry_apellido_paterno_empleado.Text+ " " + this.entry_apellido_materno_empleado.Text+" "+ this.entry_nombre1_empleado.Text+ " "+this.entry_nombre2_empleado.Text;		
		this.entry_id_emp_imp.Text = this.entry_id_empleado_etiqueta.Text;
		this.entry_direc_imp.Text = this.entry_calle_empleado.Text+ " #"+ this.entry_numcalle_empleado.Text+ " , "+this.entry_colonia_empleado.Text;
		this.entry_edad_imp.Text = this.entry_edad.Text;
		this.entry_naci_imp.Text = this.entry_nacionalidad.Text;
		this.entry_fech_nac_imp.Text = this.entry_dia_nac.Text + "/"+this.entry_mes_nac.Text + "/"+this.entry_anno_nac.Text;
		
		this.entry_nomb_imp.Editable = false;
		this.entry_id_emp_imp.Editable = false;
		this.entry_direc_imp.Editable = false;
		this.entry_edad_imp.Editable = false;
		this.entry_naci_imp.Editable = false;
		this.entry_fech_nac_imp.Editable = false;
		
		button_imp_cont.Clicked += new EventHandler(on_button_imp_cont_clicked);
		button_imp_reg.Clicked += new EventHandler(on_button_imp_reg_clicked);
            
		}
		
		void on_button_imp_reg_clicked(object sender, EventArgs args)
		{
		
			imp_cont_o_reg_alta.Destroy();
			/*
			new rpt_detalle_empleados(entry_contrato_empleado.Text,
										entry_apellido_paterno_empleado.Text,
										entry_apellido_materno_empleado.Text,
										entry_nombre1_empleado.Text,
										entry_nombre2_empleado.Text,
										entry_dia_nac.Text, 
										entry_mes_nac.Text,
	    								entry_anno_nac.Text,
										entry_lugar_nac.Text,
										entry_rfc_empleado.Text,
										entry_curp_empleado.Text,
										entry_imss_empleado.Text,
										entry_infonavit_empleado.Text,
										entry_afore_empleado.Text,
	    								entry_residencia_empleado.Text,
										entry_nom_padre_empleado.Text,
										entry_nom_madre_empleado.Text,
										entry_calle_empleado.Text,
										entry_codigo_postal_empleado.Text,
										entry_colonia_empleado.Text,
										entry_tel1_empleado.Text,
										entry_notas_empleado.Text,
										entry_dia_ingreso.Text, 
										entry_mes_ingreso.Text,
										entry_anno_ingreso.Text,
										entry_nombrepuesto_empleado.Text,
										entry_depto_empleado.Text,
										entry_id_empleado_etiqueta.Text,
										entry_edad.Text,
										entry_numcalle_empleado.Text,
										tmp_estado_civil,
										tmp_municipios,
										tmp_estado,
										var_tipo_casa,
										tipo_contrato_oculta,
										tipo_pago_oculta,
										entry_numero_locker.Text,
										sueldo_actual_oculta.Trim());
		
			*/
		}
		
		void on_button_imp_cont_clicked (object sender, EventArgs args)
		{
		
			imp_cont_o_reg_alta.Destroy();
		
			//crea formato dd/mes/aa
			//string month = Convert.ToString( DateTime.Parse( mes_contrato_oculta+"/01/2000").ToLongDateString());
			//string fechacontrato = dia_contrato_oculta + "/" + month.Substring(5,month.Length-13).ToUpper() +"/"+anno_contrato_oculta;
		
			// crea formato largo de fecha
			string fechacontrato = Convert.ToString( Convert.ToDateTime(dia_contrato_oculta + "/" + mes_contrato_oculta +"/"+anno_contrato_oculta).ToLongDateString());
		
			/*
			new rpt_contrato_empleado(entry_contrato_empleado.Text,
						  		entry_apellido_paterno_empleado.Text,
						  		entry_apellido_materno_empleado.Text,
								entry_nombre1_empleado.Text,
								entry_nombre2_empleado.Text,
							    this.entry_edad.Text,
							    this.entry_calle_empleado.Text,
							    this.entry_colonia_empleado.Text,
							    this.entry_numcalle_empleado.Text,
							    tipofuncion_oculta,
							    this.entry_depto_empleado.Text,
							    this.entry_nombrepuesto_empleado.Text,
							    this.entry_jornada_empleado.Text,
							    this.entry_tiempo_comida.Text,
							    fechacontrato,
							    sueldo_actual_oculta.Trim(),
							    tipo_pago_oculta,
							    this.entry_nacionalidad.Text
							    
							    //feca de cobro
							    
								);
			*/
		}
		   
		void on_button_baja_empleado_clicked (object sender, EventArgs args)
		{
			//new osiris.bajapersonal(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
			Glade.XML gxml = new Glade.XML (null, "recursos_humanos.glade", "baja_empleado", null);
			gxml.Autoconnect (this);
			this.entry_puesto.Sensitive = false;
			this.entry_depto.Sensitive = false;
			this.entry_puesto.Text = this.entry_nombrepuesto_empleado.Text;
			this.entry_depto.Text = this.entry_depto_empleado.Text;
	        this.entry_dd_ingreso.Text  = this.entry_dia_ingreso.Text;
	        this.entry_mm_ingreso.Text = this.entry_mes_ingreso.Text;
	        this.entry_aaaa_ingreso.Text = this.entry_anno_ingreso.Text;
	        llenacombobaja();
	        
	        this.entry_dia_baja.KeyPressEvent += onKeyPressEventdd;
	        this.entry_mes_baja.KeyPressEvent += onKeyPressEventdd;
	        this.entry_anno_baja.KeyPressEvent += onKeyPressEventdd;
	        
			// Muestra ventana de Glade
			baja_empleado.Show();
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_dar_baja.Clicked += new EventHandler(on_dar_baja_clicked);
			
			entry_dia_baja.Text = DateTime.Now.ToString("dd");
			entry_mes_baja.Text = DateTime.Now.ToString("MM");
			entry_anno_baja.Text = DateTime.Now.ToString("yyyy");
		}
			
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEventdd(object o, Gtk.KeyPressEventArgs args)
		{
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮ";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace  && args.Event.Key != Gdk.Key.Delete )
			{

			args.RetVal = true;

			}
		}
		
		void llenacombobaja()
		{  
		   combobox_baja.Clear();
			CellRendererText cell25 = new CellRendererText();
			combobox_baja.PackStart(cell25, true);
			combobox_baja.AddAttribute(cell25,"text",0);
			
	        
			ListStore store25 = new ListStore( typeof (string));
			combobox_baja.Model = store25;
			
			store25.AppendValues ("");
			store25.AppendValues ("RENUNCIA");
			store25.AppendValues ("VENCIMIENTO DE CONTRATO");
			store25.AppendValues ("ABANDONO DE TRABAJO");
			store25.AppendValues ("FALLECIMIENTO");
			store25.AppendValues ("DESPIDO");
			        	        
			TreeIter iter25;
			if (store25.GetIterFirst(out iter25))	
			{ combobox_baja.SetActiveIter (iter25); }
			combobox_baja.Changed += new EventHandler (onComboBoxChanged_baja);
		}
		
		void onComboBoxChanged_baja(object sender, EventArgs args)
		{
			ComboBox combobox_baja = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_baja.GetActiveIter (out iter)){var_baja_emp = (string) combobox_baja.Model.GetValue(iter,0);}
		}
		
		void on_button_sistemas_empleado_clicked (object sender, EventArgs args)
		{
			//Aqui crea el objeto para controlar los permisos de acceso a los sistemas
			//new osiris.sistemaspersonal(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_sel_clicked (object sender, EventArgs args)
		{
			llena_empleado();			protegecampos();
		}
		
		void on_dar_baja_clicked (object sender, EventArgs args)
		{
	    	if (var_baja_emp == ""){
	     		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"Selecione una causa de baja");
					msgBoxError.Run ();				msgBoxError.Destroy();
				     
	    	}else{
				string strsql = "";
		    	NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
			    // Verifica que la base de datos este conectada
			    
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
			 		
			 		
			 		//almacena el detalle			        
					
			 		
					strsql = "UPDATE osiris_empleado "+
						"SET "+ 
						"baja_empleado = 'true'";
						strsql = strsql +"WHERE (id_empleado LIKE '"+entry_id_empleado_etiqueta.Text+"') ";
			        	comando.CommandText = strsql; 
		 			
					comando.ExecuteNonQuery();
			        comando.Dispose();
							        
					comando = conexion.CreateCommand ();
			 		strsql = "";
					strsql = "UPDATE osiris_empleado_detalle "+
											"SET "+ 
											"notas_baja = '" + entry_notas_baja.Text.ToUpper()+"',"+
											         //if (var_baja_emp == "") 
											          // else
											          //{strsql = strsql + 
											   "causa_baja = '"+ var_baja_emp+"',";
											         //}
											 if(this.entry_dia_baja.Text=="" || this.entry_mes_baja.Text=="" || this.entry_anno_baja.Text=="")
 						                       {  }
 						                       else                         {
 					                           strsql = strsql + "fecha_de_baja = '"+ entry_anno_baja.Text.ToUpper()+"-"+ entry_mes_baja.Text.ToUpper()+"-"+ entry_dia_baja.Text.ToUpper()+"'";
 					                           }
 					               strsql = strsql +"WHERE (id_empleado LIKE '"+entry_id_empleado_etiqueta.Text+"') ";
	
		 			
		 			comando.CommandText = strsql; 
		 															
		 			//Console.WriteLine(strsql);
					comando.ExecuteNonQuery();
			        comando.Dispose();
				}catch (NpgsqlException ex){
				     MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
				conexion.Close ();
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
		
		void on_button_buscar_empleado_clicked (object sender, EventArgs args)
		{
			if (checkbutton_empleado_nuevo.Active == true)
				{checkbutton_empleado_nuevo.Active = false;}
			
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
					comando.CommandText = "SELECT id_empleado,nombre1_empleado,nombre2_empleado, "+
									  "apellido_paterno_empleado, "+
									  "apellido_materno_empleado, estado_del_empleado "+
									  "FROM osiris_empleado " +
									  "WHERE (baja_empleado = 'false');";
									  				
				}else{
					if (radiobutton_busca_apellido.Active == true){
						comando.CommandText = "SELECT m.id_empleado, m.nombre1_empleado, m.nombre2_empleado, "+
									  "m.apellido_paterno_empleado, "+
									  "m.apellido_materno_empleado, m.estado_del_empleado "+
									  "FROM osiris_empleado m " +
									  "WHERE (apellido_paterno_empleado LIKE '"+entry_expresion.Text.ToUpper()+"%' OR apellido_materno_empleado LIKE '"+entry_expresion.Text.ToUpper()+"%') "+
									  "ORDER BY id_empleado;";
					}
					if (radiobutton_busca_nombre.Active == true){
						comando.CommandText = "SELECT m.id_empleado, m.nombre1_empleado, m.nombre2_empleado, "+
									  "m.apellido_paterno_empleado, "+
									  "m.apellido_materno_empleado, m.estado_del_empleado "+
									  "FROM osiris_empleado m " +
									  "WHERE (nombre1_empleado LIKE '"+entry_expresion.Text.ToUpper()+"%' OR nombre2_empleado LIKE '"+entry_expresion.Text.ToUpper()+"%') "+
									  "ORDER BY id_empleado;";
					}
					if (radiobutton_busca_numero.Active == true){
					//Console.WriteLine ("Radio jala");
						comando.CommandText = "SELECT m.id_empleado, m.nombre1_empleado, m.nombre2_empleado, "+
									  "m.apellido_paterno_empleado, "+
									  "m.apellido_materno_empleado, m.estado_del_empleado "+
									  "FROM osiris_empleado m " +
									  "WHERE (id_empleado LIKE '"+entry_expresion.Text.ToUpper()+"%') "+
									  "ORDER BY id_empleado;";				
					}
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//Console.WriteLine(comando.CommandText);
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
			
			lista_de_empleados.RowActivated += on_selecciona_empleado_clicked;  // Doble click selecciono paciente*/
			
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
			if (lista_de_empleados.Selection.GetSelected(out model, out iterSelected)) {
				selcampo = (string) model.GetValue(iterSelected, 0);
				//Console.WriteLine (selcampo);
				busca_empleado.Destroy();
				entry_id_empleado.Text = selcampo;
				llena_empleado();	protegecampos();
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
				if (checkbutton_empleado_nuevo.Active == true)
					{checkbutton_empleado_nuevo.Active = false;}
				llena_empleado();
				protegecampos();
			// actrivar sensitive de editar, no funciona dentro de llena_empleado
			
			}
			string misDigitos = "0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮ)(ｔｒｓｑnpexTNPEXTt";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace && args.Event.Key != Gdk.Key.Delete)
			{
				args.RetVal = true;
			}
		}
		
//////////////////////////////////////////////////////////////////////////////////////////////////////
///////// DEFINICION DE LLENADO DE COMBOS ////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////		
		
		void llenado_tipo_empleado(string activo)
		{
			combobox_tipo_empleado.Clear();
			CellRendererText cell6 = new CellRendererText();
			combobox_tipo_empleado.PackStart(cell6, true);
			combobox_tipo_empleado.AddAttribute(cell6,"text",0);
			
	        
			ListStore store6 = new ListStore( typeof (string));
			combobox_tipo_empleado.Model = store6;
			
			if (activo == "")
			{store6.AppendValues ("");}
			else {store6.AppendValues (activo);}
			store6.AppendValues ("N - EMPLEADO");
			store6.AppendValues ("E - EXTERNO");
			store6.AppendValues ("P - PRACTICAS");
			store6.AppendValues ("T - TEMPORAL");
			        	        
			TreeIter iter6;
			if (store6.GetIterFirst(out iter6))	
			{ combobox_tipo_empleado.SetActiveIter (iter6); }
			combobox_tipo_empleado.Changed += new EventHandler (onComboBoxChanged_tipo_empleado);
		}
		
		void onComboBoxChanged_tipo_empleado (object sender, EventArgs args)
		{
		
     	int longitudcad=0;
		
			ComboBox combobox_tipo_empleado = sender as ComboBox;
			if (sender == null) {	return;}
			TreeIter iter;
			
			if (combobox_tipo_empleado.GetActiveIter (out iter)) {	
				var_tipo_empleado = (string) combobox_tipo_empleado.Model.GetValue(iter,0);
				
				longitudcad = (int) var_tipo_empleado.Length;
				if (longitudcad > 0){
					string solotipo = var_tipo_empleado;
					var_tipo_empleado = var_tipo_empleado.Substring(4,longitudcad - 4);
					if (var_tipo_empleado == "EMPLEADO") 
						{var_clave_tipo_empleado = "N";
						abrecampos();
						this.entry_id_empleado.Text = ultimoregistro(var_clave_tipo_empleado);
						//this.entry_id_empleado.Editable = false;
						this.button_baja_empleado.Sensitive = false;
						this.button_editar_empleado.Sensitive = false;
						this.button_contrato_empleado.Sensitive =false;
						this.button_vacaciones_empleado.Sensitive = false;
						}
					else if (var_tipo_empleado == "EXTERNO") 
						{var_clave_tipo_empleado = "E";
						abrecampos();
						this.entry_id_empleado.Text = ultimoregistro(var_clave_tipo_empleado);
						//this.entry_id_empleado.Editable = false;
						this.button_baja_empleado.Sensitive = false;
						this.button_editar_empleado.Sensitive = false;
						this.button_contrato_empleado.Sensitive =false;
						this.button_vacaciones_empleado.Sensitive = false;
						}
					else if (var_tipo_empleado == "PRACTICAS") 
						{var_clave_tipo_empleado = "P";
						abrecampos();
						this.entry_id_empleado.Text = ultimoregistro(var_clave_tipo_empleado);
						//this.entry_id_empleado.Editable = false;
						this.button_baja_empleado.Sensitive = false;
						this.button_editar_empleado.Sensitive = false;
						this.button_contrato_empleado.Sensitive =false;
						this.button_vacaciones_empleado.Sensitive = false;
						}
					else if (var_tipo_empleado == "TEMPORAL") 
						{var_clave_tipo_empleado = "T";
						abrecampos();
						this.button_baja_empleado.Sensitive = false;
						this.button_editar_empleado.Sensitive = false;
						this.button_contrato_empleado.Sensitive =false;
						this.button_vacaciones_empleado.Sensitive = false;
						this.entry_id_empleado.Text = ultimoregistro(var_clave_tipo_empleado);
						//this.entry_id_empleado.Editable = false;
						}
					else {var_tipo_empleado = solotipo;}
					
				}
			}
		}
		
		void llenado_escolaridad_empleado(string activo)
		{
			combobox_escolaridad_empleado.Clear();
			CellRendererText cell34 = new CellRendererText();
			combobox_escolaridad_empleado.PackStart(cell34, true);
			combobox_escolaridad_empleado.AddAttribute(cell34,"text",0);
	        
			ListStore store34 = new ListStore( typeof (string));
			combobox_escolaridad_empleado.Model = store34;
	        
	        if (activo == "")
			{store34.AppendValues ("");}
			else {store34.AppendValues (activo);}
			store34.AppendValues ("PRIMARIA");
			store34.AppendValues ("SECUNDARIA");
			store34.AppendValues ("PREPARATORIA");
			store34.AppendValues ("PREPARATORIA TECNICA");
			store34.AppendValues ("TECNICO UNIVERSITARIO");
			store34.AppendValues ("UNIVERSITARIO");
			store34.AppendValues ("POST-GRADO");
			store34.AppendValues ("DOCTORADO");
	        
			TreeIter iter34;
			if (store34.GetIterFirst(out iter34))
			{
				combobox_escolaridad_empleado.SetActiveIter (iter34);
			}
			combobox_escolaridad_empleado.Changed += new EventHandler (onComboBoxChanged_escolaridad);
		}
		
		void onComboBoxChanged_escolaridad (object sender, EventArgs args)
		{
			ComboBox combobox_escolaridad_empleado = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_escolaridad_empleado.GetActiveIter (out iter)) {	
				var_escolaridad = (string) combobox_escolaridad_empleado.Model.GetValue(iter,0);
			}
		}
		
	
		void llenado_estadocivil(string activo)
		{
			combobox_estado_civil.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_estado_civil.PackStart(cell, true);
			combobox_estado_civil.AddAttribute(cell,"text",0);
	        
			ListStore store = new ListStore( typeof (string));
			combobox_estado_civil.Model = store;
	        
	        if (activo == "")
			{store.AppendValues ("");}
			else {store.AppendValues (activo);}
			store.AppendValues ("CASADO(A)");
			store.AppendValues ("SOLTERO(A)");
			store.AppendValues ("SEPARADO(A)");
			store.AppendValues ("VIUDO(A)");
			store.AppendValues ("UNION LIBRE");
			store.AppendValues ("DIVORCIADO(A)");
	        
			TreeIter iter;
			if (store.GetIterFirst(out iter))
			{
				combobox_estado_civil.SetActiveIter (iter);
			}
			combobox_estado_civil.Changed += new EventHandler (onComboBoxChanged_estadocivil);
		}
		
		void onComboBoxChanged_estadocivil (object sender, EventArgs args)
		{
			ComboBox combobox_estado_civil = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_estado_civil.GetActiveIter (out iter)) {	
				tmp_estado_civil = (string) combobox_estado_civil.Model.GetValue(iter,0);
			}
		}
		
		
		void llenado_tipo_casa(string activo)
		{
			combobox_casa_empleado.Clear();
			CellRendererText cell7 = new CellRendererText();
			combobox_casa_empleado.PackStart(cell7, true);
			combobox_casa_empleado.AddAttribute(cell7,"text",0);
			
	        
			ListStore store7 = new ListStore( typeof (string));
			combobox_casa_empleado.Model = store7;
			
			if (activo == "")
			{store7.AppendValues ("");}
			else {store7.AppendValues (activo);}
			store7.AppendValues ("PROPIA");
			store7.AppendValues ("RENTA");
			store7.AppendValues ("ALLEGADO");
			store7.AppendValues ("CREDITO INFONAVIT");
			store7.AppendValues ("CREDITO BANCARIO");
			        	        
			TreeIter iter7;
			if (store7.GetIterFirst(out iter7))	
			{ combobox_casa_empleado.SetActiveIter (iter7); }
			combobox_casa_empleado.Changed += new EventHandler (onComboBoxChanged_casa_empleado);
		}
		
		void onComboBoxChanged_casa_empleado (object sender, EventArgs args)
		{
			ComboBox combobox_casa_empleado = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_casa_empleado.GetActiveIter (out iter)) {	
				var_tipo_casa = (string) combobox_casa_empleado.Model.GetValue(iter,0);
			}
		}
		
		
		void llenado_num_hijos(string activo)
		{
			combobox_num_hijos_empleado.Clear();
			CellRendererText cell8 = new CellRendererText();
			combobox_num_hijos_empleado.PackStart(cell8, true);
			combobox_num_hijos_empleado.AddAttribute(cell8,"text",0);
			
	    	ListStore store8 = new ListStore( typeof (string));
			combobox_num_hijos_empleado.Model = store8;
			
			if (activo == "")
			{store8.AppendValues ("");}
			else {store8.AppendValues (activo);}
			store8.AppendValues ("0");
			store8.AppendValues ("1");
			store8.AppendValues ("2");
			store8.AppendValues ("3");
			store8.AppendValues ("4");
			store8.AppendValues ("5");
			store8.AppendValues ("6");
			store8.AppendValues ("7");
			store8.AppendValues ("8");
			store8.AppendValues ("9");
			store8.AppendValues ("10");
			store8.AppendValues ("11");
			store8.AppendValues ("12");
			store8.AppendValues ("13");
			store8.AppendValues ("14");
			store8.AppendValues ("15");
			store8.AppendValues ("16");
			store8.AppendValues ("17");
			store8.AppendValues ("18");
			store8.AppendValues ("19");
			store8.AppendValues ("20");
			
		TreeIter iter8;
			if (store8.GetIterFirst(out iter8))	
			{ combobox_num_hijos_empleado.SetActiveIter (iter8); }
			combobox_num_hijos_empleado.Changed += new EventHandler (onComboBoxChanged_numhijos_empleado);
	    	}
		
		void onComboBoxChanged_numhijos_empleado (object sender, EventArgs args)
		{
			ComboBox combobox_num_hijos_empleado = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_num_hijos_empleado.GetActiveIter (out iter)) {	
				var_num_hijos = (string) combobox_num_hijos_empleado.Model.GetValue(iter,0);
			}
		}
		
		void llenado_municipios(string municipioactivo, int idmunicipioactivo)
		{
			//llenado_municipios(municipios,idestado);
			combobox_municipios.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_municipios.PackStart(cell3, true);
			combobox_municipios.AddAttribute(cell3,"text",0);
	        
			ListStore store3 = new ListStore( typeof (string));
			combobox_municipios.Model = store3;
	      
	        NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	//comando.CommandText = "SELECT descripcion_municipio FROM osiris_municipios WHERE id_estado = '"+idestado.ToString()+"' "+
               	comando.CommandText = "SELECT descripcion_municipio FROM osiris_municipios WHERE id_estado = '"+idmunicipioactivo+"' "+
               						"ORDER BY descripcion_municipio;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if (municipioactivo== "")
					{store3.AppendValues ("");}
				else{store3.AppendValues ((string) municipioactivo);}
               	while (lector.Read())
				{store3.AppendValues ((string) lector["descripcion_municipio"]);}
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
		
		void onComboBoxChanged_municipios (object sender, EventArgs args)
		{
			ComboBox combobox_municipios = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_municipios.GetActiveIter (out iter)) {	
			municipios = (string) combobox_municipios.Model.GetValue(iter,0); }
	   }
	   	
		void llenado_estados(string estadoactivo, int idestadoactivo)
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
				if (estadoactivo == ""){
					store4.AppendValues ("", 0);}
				else {
					store4.AppendValues ((string) estadoactivo, 0);
					}
               	while (lector.Read()){
					store4.AppendValues ((string) lector["descripcion_estado"], (int) lector["id_estado"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
	      	        	        
			TreeIter iter4;
			if (store4.GetIterFirst(out iter4))	{ combobox_estado.SetActiveIter (iter4); }
			combobox_estado.Changed += new EventHandler (onComboBoxChanged_estado);
		}
		
		void onComboBoxChanged_estado (object sender, EventArgs args)
		{
			ComboBox combobox_estado = sender as ComboBox;
			if (sender == null){return;}
			TreeIter iter;
			if (combobox_estado.GetActiveIter (out iter)) {	
				estado = (string) combobox_estado.Model.GetValue(iter,0); 
				idestado = (int) combobox_estado.Model.GetValue(iter,1);
				
				//llenado_municipios("",0);
				if (idestado == 0 ){
					llenado_municipios(municipios,0);}
				else{llenado_municipios("",idestado);
					llenado_municipios(municipios,idestado);
				}
			}
		}
		
		public void protegecampos()
		{
			checkbutton_empleado_activo.Sensitive = false;
			entry_id_empleado_etiqueta.Sensitive = false;
			entry_estatus_empleado.Sensitive = false;
			entry_nombre1_empleado.Sensitive = false;
			entry_nombre2_empleado.Sensitive = false;
			entry_apellido_paterno_empleado.Sensitive = false;
			entry_apellido_materno_empleado.Sensitive = false;
			entry_dia_nac.Sensitive = false;
			entry_mes_nac.Sensitive = false;
			entry_anno_nac.Sensitive = false;
			entry_lugar_nac.Sensitive = false;
			entry_nacionalidad.Sensitive = false;
			entry_titulo_empleado.Sensitive = false;
			entry_religion_empleado.Sensitive = false;
			entry_rfc_empleado.Sensitive = false;
			entry_curp_empleado.Sensitive = false;
			entry_imss_empleado.Sensitive = false;
			entry_infonavit_empleado.Sensitive = false;
			entry_afore_empleado.Sensitive = false;
			entry_cartilla_militar_empleado.Sensitive = false;
			entry_talla_pantalon.Sensitive = false;
			entry_talla_chaqueta.Sensitive = false;
			entry_talla_zapatos.Sensitive = false;
			entry_peso_empleado.Sensitive = false;
			entry_estatura_empleado.Sensitive = false;
			entry_tipo_sangre_empleado.Sensitive = false;
			entry_residencia_empleado.Sensitive = false;
			entry_nom_padre_empleado.Sensitive = false;
			entry_nom_madre_empleado.Sensitive = false;
			entry_nom_conyuge_empleado.Sensitive = false;
			entry_calle_empleado.Sensitive = false;
			entry_numcalle_empleado.Sensitive= false;
			entry_codigo_postal_empleado.Sensitive = false;
			entry_colonia_empleado.Sensitive = false;
			entry_tel1_empleado.Sensitive = false;
			entry_tel2_empleado.Sensitive = false;
			entry_celular_empleado.Sensitive = false;
			entry_email1_empleado.Sensitive = false;
			entry_email2_empleado.Sensitive = false;
			entry_fax_empleado.Sensitive = false;
			entry_avisar_a_empleado.Sensitive = false;
			entry_tel_accidente_empleado.Sensitive = false;
			entry_notas_empleado.Sensitive = false;
			entry_dia_ingreso.Sensitive = false;
			entry_mes_ingreso.Sensitive = false;
			entry_anno_ingreso.Sensitive = false;
			//Estos no se modifican
			entry_contrato_empleado.Sensitive = false;
			entry_depto_empleado.Sensitive = false;
			entry_nombrepuesto_empleado.Sensitive = false;
			entry_jornada_empleado.Sensitive = false;
			entry_d_disfrute_empleado.Sensitive = false;
			entry_tipo_funcion.Sensitive = false;
			entry_tiempo_comida.Sensitive = false;
			entry_numero_locker.Sensitive = false;
			entry_cont_dd.Sensitive = false;
			entry_cont_mm.Sensitive = false;
			entry_cont_aa.Sensitive = false;
			this.entry_baja_dd.Sensitive = false;
			this.entry_baja_mes.Sensitive = false;
			this.entry_baja_aa.Sensitive = false;
			entry_dia_venc_cont.Sensitive = false;
			entry_mes_venc_cont.Sensitive = false;
			entry_anno_venc_cont.Sensitive = false;
			//entry_tipo_empleado.Sensitive = false;
			entry_cont_dd.Sensitive = false;
		    entry_cont_mm.Sensitive = false;
			entry_cont_aa.Sensitive = false;
			this.entry_baja_dd.Sensitive = false;
			this.entry_baja_mes.Sensitive = false;
			this.entry_baja_aa.Sensitive = false;
			radiobutton_femenino_empleado.Sensitive = false;
			radiobutton_masculino_empleado.Sensitive = false;
			combobox_num_hijos_empleado.Sensitive = false;
			combobox_tipo_empleado.Sensitive = false;
			combobox_casa_empleado.Sensitive = false;
			combobox_estado.Sensitive = false;
			combobox_municipios.Sensitive = false;
			combobox_estado_civil.Sensitive = false;
			combobox_tipo_empleado.Sensitive = false;
			combobox_escolaridad_empleado.Sensitive = false;
			radiobutton_masculino_empleado.Sensitive = false;
			radiobutton_femenino_empleado.Sensitive = false;
			button_contrato_empleado.Sensitive = false;		
			button_vacaciones_empleado.Sensitive = false;
			button_baja_empleado.Sensitive = false;
			button_sistemas_empleado.Sensitive = false;
			button_guarda_empleado.Sensitive = false;
			button_editar_empleado.Sensitive = true;
			entry_edad.Sensitive = false;
		}
		
		
		public void abrecampos()
		{
			checkbutton_empleado_activo.Sensitive = true;
			//este siempre esta cerrado
			entry_id_empleado_etiqueta.Sensitive = false;
			entry_estatus_empleado.Sensitive = false;
			entry_nombre1_empleado.Sensitive  = true;
			entry_nombre2_empleado.Sensitive  = true;
			entry_apellido_paterno_empleado.Sensitive  = true;
			entry_apellido_materno_empleado.Sensitive  = true;
			entry_dia_nac.Sensitive  = true;
			entry_mes_nac.Sensitive  = true;
			entry_anno_nac.Sensitive  = true;
			entry_lugar_nac.Sensitive  = true;
			entry_nacionalidad.Sensitive  = true;
			entry_titulo_empleado.Sensitive  = true;
			entry_religion_empleado.Sensitive  = true;
			entry_rfc_empleado.Sensitive = true;
			entry_curp_empleado.Sensitive  = true;
			entry_imss_empleado.Sensitive  = true;
			entry_infonavit_empleado.Sensitive = true;
			entry_afore_empleado.Sensitive = true;
			entry_cartilla_militar_empleado.Sensitive = true;
			entry_talla_pantalon.Sensitive = true;
			entry_talla_chaqueta.Sensitive = true;
			entry_talla_zapatos.Sensitive = true;
			entry_peso_empleado.Sensitive = true;
			entry_estatura_empleado.Sensitive = true;
			entry_tipo_sangre_empleado.Sensitive = true;
			entry_residencia_empleado.Sensitive = true;
			entry_nom_padre_empleado.Sensitive = true;
			entry_nom_madre_empleado.Sensitive = true;
			entry_nom_conyuge_empleado.Sensitive = true;
			entry_calle_empleado.Sensitive = true;
			entry_numcalle_empleado.Sensitive = true;
			entry_codigo_postal_empleado.Sensitive = true;
			entry_colonia_empleado.Sensitive = true;
			entry_tel1_empleado.Sensitive = true;
			entry_tel2_empleado.Sensitive = true;
			entry_celular_empleado.Sensitive = true;
			entry_email1_empleado.Sensitive = true;
			entry_email2_empleado.Sensitive = true;
			entry_fax_empleado.Sensitive = true;
			entry_avisar_a_empleado.Sensitive = true;
			entry_tel_accidente_empleado.Sensitive = true;
			entry_notas_empleado.Sensitive = true;
			entry_dia_ingreso.Sensitive = true;
			entry_mes_ingreso.Sensitive = true;
			entry_anno_ingreso.Sensitive = true;
			//estos son los datos informativos. NO SE HABILITAN
			entry_contrato_empleado.Sensitive = false;
			entry_depto_empleado.Sensitive = false;
			entry_tipo_funcion.Sensitive = false;
			entry_tiempo_comida.Sensitive = false;
			entry_numero_locker.Sensitive = false;
			entry_nombrepuesto_empleado.Sensitive = false;
			entry_jornada_empleado.Sensitive = false;
			entry_d_disfrute_empleado.Sensitive = false;
			entry_dia_venc_cont.Sensitive = false;
			entry_mes_venc_cont.Sensitive = false;
			entry_anno_venc_cont.Sensitive = false;
			//entry_tipo_empleado.Sensitive = false;
		    entry_cont_dd.Sensitive = false;
		    entry_cont_mm.Sensitive = false;
			entry_cont_aa.Sensitive = false;
			this.entry_baja_dd.Sensitive = false;
			this.entry_baja_mes.Sensitive = false;
			this.entry_baja_aa.Sensitive = false;
			combobox_num_hijos_empleado.Sensitive = true;
			combobox_tipo_empleado.Sensitive = true;
			combobox_casa_empleado.Sensitive = true;
			combobox_estado.Sensitive = true;
			combobox_municipios.Sensitive = true;
			combobox_estado_civil.Sensitive = true;
			combobox_tipo_empleado.Sensitive = true;
			combobox_escolaridad_empleado.Sensitive = true;
			radiobutton_masculino_empleado.Sensitive = true;
			radiobutton_femenino_empleado.Sensitive = true;
			button_contrato_empleado.Sensitive = true;		
			button_vacaciones_empleado.Sensitive = true;
			button_baja_empleado.Sensitive = true;
			button_sistemas_empleado.Sensitive = false;
			button_guarda_empleado.Sensitive = true;
			button_editar_empleado.Sensitive = true;
			entry_edad.Sensitive = false;
		}

		public void limpiacampos()
		{
			checkbutton_empleado_activo.Sensitive = false;
			//este siempre esta cerrado
			entry_id_empleado_etiqueta.Sensitive = false;
			entry_estatus_empleado.Text = "";
			entry_nombre1_empleado.Text = "";
			entry_nombre2_empleado.Text = "";
			entry_apellido_paterno_empleado.Text = "";
			entry_apellido_materno_empleado.Text = "";
			entry_dia_nac.Text = "";
			entry_mes_nac.Text = "";
			entry_anno_nac.Text = "";
			entry_lugar_nac.Text = "";
			entry_nacionalidad.Text = "";
			entry_titulo_empleado.Text = "";
			entry_religion_empleado.Text = "";
			entry_rfc_empleado.Text = "";
			entry_curp_empleado.Text = "";
			entry_imss_empleado.Text = "";
			entry_infonavit_empleado.Text = "";
			entry_afore_empleado.Text = "";
			entry_cartilla_militar_empleado.Text = "";
			entry_talla_pantalon.Text = "";
			entry_talla_chaqueta.Text = "";
			entry_talla_zapatos.Text = "";
			entry_peso_empleado.Text = "";
			entry_estatura_empleado.Text = "";
			entry_tipo_sangre_empleado.Text = "";
			entry_residencia_empleado.Text = "";
			entry_nom_padre_empleado.Text = "";
			entry_nom_madre_empleado.Text = "";
			entry_nom_conyuge_empleado.Text = "";
			entry_calle_empleado.Text = "";
			entry_numcalle_empleado.Text = "";
			entry_codigo_postal_empleado.Text = "";
			entry_colonia_empleado.Text = "";
			entry_tel1_empleado.Text = "";
			entry_tel2_empleado.Text = "";
			entry_celular_empleado.Text = "";
			entry_email1_empleado.Text = "";
			entry_email2_empleado.Text = "";
			entry_fax_empleado.Text = "";
			entry_avisar_a_empleado.Text = "";
			entry_tel_accidente_empleado.Text = "";
			entry_notas_empleado.Text = "";
			entry_dia_ingreso.Text = "";
			entry_mes_ingreso.Text = "";
			entry_anno_ingreso.Text = "";
			//estos son los datos informativos.
			entry_contrato_empleado.Text = "";
			entry_depto_empleado.Text = "";
			entry_nombrepuesto_empleado.Text = "";
			entry_jornada_empleado.Text = "";
			entry_tipo_funcion.Text = "";
			entry_tiempo_comida.Text = "";
			entry_numero_locker.Text = "";
			entry_d_disfrute_empleado.Text = "";
			
			entry_cont_dd.Text = "";
		    entry_cont_mm.Text = "";
			entry_cont_aa.Text = "";
			
			entry_edad.Text = "";
			entry_dia_venc_cont.Text = "";
			entry_mes_venc_cont.Text = "";
			entry_anno_venc_cont.Text = "";
			combobox_num_hijos_empleado.Clear();
			combobox_tipo_empleado.Clear();
			combobox_casa_empleado.Clear();
			combobox_escolaridad_empleado.Clear();
			combobox_estado.Clear();
			combobox_municipios.Clear();
			combobox_estado_civil.Clear();
	    	radiobutton_masculino_empleado.Sensitive = true;
			radiobutton_femenino_empleado.Sensitive = true;
		}											
	}		
	
	//////////////////////////////////////////////////////////////////////////
	// Clase de vacaciones 
	//////////////////////////////////////////////////////////////////////////
	public class vacacionespersonal
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando ventana del menu de costos
		[Widget] Gtk.Window vacaciones_empleado;
		
		public string connectionString = "Server=localhost;" +
						"Port=5432;" +
						 "User ID=admin;" +
						"Password=1qaz2wsx;";
		public string nombrebd;
		public string LoginEmpleado;
    	public string NomEmpleado;
    	public string AppEmpleado;
    	public string ApmEmpleado;
    	
    	//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
				
		public vacacionespersonal(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_ )
		{
			LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		nombrebd = _nombrebd_; 
    		
			Glade.XML gxml = new Glade.XML (null, "recursos_humanos.glade", "vacaciones_empleado", null);
			gxml.Autoconnect (this);
	        
			// Muestra ventana de Glade
			vacaciones_empleado.Show();
			
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);	
		
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}	
}