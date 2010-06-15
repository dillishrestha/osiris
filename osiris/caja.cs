///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
//				: ing. Juan Antonio Peña Gonzalez (Adecuaciones y Colaboracion) 
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
// Programa		: caja.cs
// Proposito	: Pagos en Caja 
// Objeto		: caja.cs
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using Gtk;
using Glade;
using System.Collections;

namespace osiris
{
	public class caja_cobro
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion = null;
		[Widget] Gtk.Button button_selecciona = null;
		[Widget] Gtk.Button button_buscar_busqueda = null;
		
		// Declarando ventana principal de pago
		[Widget] Gtk.Window caja = null;
		[Widget] Gtk.Entry entry_folio_servicio = null;
		
		[Widget] Gtk.Entry entry_ingreso = null;
		[Widget] Gtk.Entry entry_egreso = null;
		[Widget] Gtk.Entry entry_numero_factura = null;
		
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_nombre_paciente = null;
		[Widget] Gtk.Entry entry_telefono_paciente = null;
		[Widget] Gtk.Entry entry_descrip_cirugia = null;
		//entry_descrip_cirugia
		[Widget] Gtk.Entry entry_doctor = null;
		[Widget] Gtk.Entry entry_especialidad = null;
		[Widget] Gtk.Entry entry_tipo_paciente = null;
		[Widget] Gtk.Entry entry_aseguradora = null;
		[Widget] Gtk.Entry entry_poliza = null;
		[Widget] Gtk.Entry entry_total_abonos_caja = null;
		[Widget] Gtk.Entry entry_habitacion;
		[Widget] Gtk.Entry entry_diagnostico;
		
		[Widget] Gtk.TreeView lista_de_servicios;
		[Widget] Gtk.TreeView lista_cargos_extras;
		[Widget] Gtk.ProgressBar progressbar_status_llenado;
		[Widget] Gtk.Button button_quitar_aplicados;
		[Widget] Gtk.Button button_actualizar;
		[Widget] Gtk.Button button_buscar_paciente;
		[Widget] Gtk.Button button_selec_folio;
		[Widget] Gtk.Button button_graba_pago;
		[Widget] Gtk.Entry entry_desc_producto;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.Button button_removerItem;
		[Widget] Gtk.Button button_traspasa_productos;
		[Widget] Gtk.Button button_aplica_cargos;
		[Widget] Gtk.Button button_honorario_medico;
		[Widget] Gtk.Button button_procedimiento_cobrz;
		[Widget] Gtk.Button button_procedimiento_totales;
		[Widget] Gtk.Button button_imprimir_protocolo;
		[Widget] Gtk.Button button_compro_caja;
		[Widget] Gtk.Button button_compro_serv;
		[Widget] Gtk.Button button_bloquea_cuenta;
		[Widget] Gtk.Button button_alta_paciente;
		[Widget] Gtk.Button button_abonar;
		[Widget] Gtk.Button button_abre_folio;
		[Widget] Gtk.Button button_compara_extras;
		//[Widget] Gtk.Button button_costeo_procedimiento;
				
		[Widget] Gtk.Button button_cierre_cuenta;
		
		[Widget] Gtk.Entry entry_subtotal_al_15;
		[Widget] Gtk.Entry entry_subtotal_al_0;
		[Widget] Gtk.Entry entry_total_iva;
		[Widget] Gtk.Entry entry_subtotal;
		[Widget] Gtk.Entry entry_deducible_caja;
		[Widget] Gtk.Entry entry_coaseguro_caja;
		[Widget] Gtk.Entry entry_totaldescuento;
		[Widget] Gtk.Entry entry_total;
		[Widget] Gtk.Entry entry_a_pagar;
		[Widget] Gtk.Entry entry_honorario_med_caja;
		[Widget] Gtk.Entry entry_ultimo_pago;
			
		// Declarando ventana de pago
		//[Widget] Gtk.Window cierre_cuenta;
		
		//Declarando la barra de estado
		[Widget] Gtk.Statusbar statusbar_caja;
		
		/////// Ventana Busqueda de paciente\\\\\\\\
		//[Widget] Gtk.Window busca_paciente;
		[Widget] Gtk.TreeView lista_de_Pacientes;
		[Widget] Gtk.RadioButton radiobutton_busca_apellido;
		[Widget] Gtk.RadioButton radiobutton_busca_nombre;
		[Widget] Gtk.RadioButton radiobutton_busca_expediente;
		
		/////// Ventana Busqueda de productos\\\\\\\\
		//[Widget] Gtk.Window busca_producto;
		[Widget] Gtk.TreeView lista_de_producto;
		[Widget] Gtk.ComboBox combobox_tipo_admision;
		//[Widget] Gtk.Button button_agrega_extra;
		[Widget] Gtk.Entry entry_cantidad_aplicada;
		
		// Ventana de Rango de Fecha
		[Widget] Gtk.Window rango_de_fecha;
		[Widget] Gtk.Entry entry_dia1;
		[Widget] Gtk.Entry entry_dia2;
		[Widget] Gtk.Entry entry_mes1;
		[Widget] Gtk.Entry entry_mes2;
		[Widget] Gtk.Entry entry_ano1;
		[Widget] Gtk.Entry entry_ano2;
		[Widget] Gtk.Entry entry_referencia_inicial;
		[Widget] Gtk.Entry entry_cliente;
		[Widget] Gtk.Label label_orden;
		[Widget] Gtk.Label label_nom_cliente;
		[Widget] Gtk.RadioButton radiobutton_cliente;
		[Widget] Gtk.RadioButton radiobutton_fecha;
		[Widget] Gtk.Button button_busca_cliente;
		[Widget] Gtk.Button button_imprime_rangofecha;
		[Widget] Gtk.CheckButton checkbutton_impr_todo_proce;
		[Widget] Gtk.CheckButton checkbutton_todos_los_clientes;
				
		// Ventana de Honorario Medico y facturacion
		[Widget] Gtk.Window honorario_medico;
		[Widget] Gtk.TreeView lista_de_honorarios;
		[Widget] Gtk.Entry entry_valor_honorario;
		[Widget] Gtk.Entry entry_medico;
		[Widget] Gtk.Entry entry_totalbonos_medicos;
		[Widget] Gtk.Button button_guardar_honorario;
		[Widget] Gtk.Button button_quitar_honorario;
		[Widget] Gtk.Button button_busca_doctores;
		[Widget] Gtk.Button button_imprime_honorarios;
		[Widget] Gtk.Button button_solicitud_cheque;
		[Widget] Gtk.Button button_pago_honorario;
		
		//ventana de busqueda de medicos en honorarios medicos
		[Widget] Gtk.ComboBox combobox_tipo_busqueda;
		[Widget] Gtk.TreeView lista_de_medicos;
				
		[Widget] Gtk.TreeView lista_medicos;
		//[Widget] Gtk.Button button_llena_medicos; 
		
		// Ventana de pago en caja (comprobante de caja)
		[Widget] Gtk.Window comprobante_pago;
		[Widget] Gtk.Entry entry_numero_comprobante;
		[Widget] Gtk.Entry entry_total_comprobante;
		[Widget] Gtk.Entry entry_hora_compr;
		[Widget] Gtk.Button button_guardar_pago;
		[Widget] Gtk.Label label_pago;
		[Widget] Gtk.Label label_comp_remision;
		[Widget] Gtk.ComboBox combobox_formapago;
		
		/////// Ventana Alta de Paciente\\\\\\\\
		[Widget] Gtk.Window causa_egreso;
		[Widget] Gtk.RadioButton radiobutton_mejoria;
		[Widget] Gtk.RadioButton radiobutton_evolucion;
		[Widget] Gtk.RadioButton radiobutton_traslado;
		[Widget] Gtk.RadioButton radiobutton_voluntaria;
		[Widget] Gtk.RadioButton radiobutton_no_mejoria;
		[Widget] Gtk.RadioButton radiobutton_defuncion;
		[Widget] Gtk.Button button_acepta_alta;
		[Widget] Gtk.Entry entry_observacion_egreso;
		
		//Ventana de Traspaso de productos entre folios
		[Widget] Gtk.Window cancelador_folios;
		[Widget] Gtk.Button button_cancelar;
		[Widget] Gtk.Entry entry_folio;
		[Widget] Gtk.Entry entry_motivo;
		[Widget] Gtk.Label label_motivo;
									
		private TreeStore treeViewEngineBusca;		// Pacientes
		private TreeStore treeViewEngineBusca2;		// Productos 
		//private TreeStore treeViewEngineBusca3;		// Clientes
		private TreeStore treeViewEngineBusca4;		// Doctores
		private TreeStore treeViewEngineServicio;	// Lista de los servicios aplicados
		private ListStore treeViewEngineExtras;		// Lista de los cargos extras
		private TreeStore treeViewEngineHonorarios;	// Lista de los cargos extras
		private TreeStore treeViewEngineMedicos;
		
		private ArrayList arraycargosextras;		// Para editar cargos extras
		
		// Declaracion de variables publicas  
		int folioservicio = 0;	        		// Toma el valor de numero de atencion de paciente
		int PidPaciente = 0;		   				// Toma la actualizacion del pid del paciente
		int id_tipopaciente = 0;           		// Toma el valor del tipo de paciente
		int idempresa_paciente = 0;				// Toma el valor de la empresa que el hospital tiene convenio
		int idhabitacion = 0;					// Toma el valor de la id de la habitacion
		
		//pago del procedimiento 
		bool procedimiento_pagado;				// Toma el valor de si el procedimiento fue pagado
		int idformadepago;
		bool pagodehonorario = false;			// esta variable indica que tipo de pago es que realizo el cajero
		string idhonorariomedico = "";
		bool agregarmasabonos = true;
		bool agregarmashonorario = true;
				
		//    //nuevo lista de precios multiples//   
		int idaseguradora_paciente = 0;			// Toma el valor de la aseguradora que ingreso el paciente
		bool aplica_precios_aseguradoras = false;// Toma el valor de si se tiene creado la lista de precio en la tabla de Productos
		bool aplica_precios_empresas = false;	// Toma el valor de si se tiene creado la lista de precio en la tabla de Productos
		//
		
		int idtipointernamiento = 10; 	 		// Toma el valor del tipo de internamiento
		string descripinternamiento = "";  		// Toma la descripcion del internamiento
		int numerofactura = 0;					// Toma el numero de factura
		int idcliente = 1;						// Toma el id del cliente que se va facturar
		int numeronotacredito = 0;				// Toma el numero de la nota de credito 
		float valornotacredito = 0;			// Toma el valor de la nota de credito incluyendo el iva
		
		// Variable para la alta del paciente
		string causa_de_alta_paciente = "Por Mejoria";
		string observacionesalta = "";
		
		string numerosecuencia_deta_cobro = "";
		
		string edadpac;
		string mesespac;
		string fecha_nacimiento;
		string dir_pac;
		string cirugia;
		string empresapac;
		
		int idmedico = 0;
 		string nombmedico = "";
 		string especialidadmed ="";  
		
		bool cuenta_bloqueada;
		bool cuenta_cerrada;
		
		float valoriva;
		bool aplicar_descuento = true;
		bool aplicar_siempre = false;
		
		string tipodereporte = "";

		string id_produ = "";
		string desc_produ = "";
		string precio_produ ="";
		string iva_produ ="";
		string total_produ ="";
		string descuent_produ ="";
		string pre_con_desc_produ ="";
		float valor_descuento = 0;
		string costo_unitario_producto;
		string porcentage_utilidad_producto;
		string costo_total_producto;
		float ppcantidad = 0;
		string ppcant ="";
		float totalhonorarios = 0;
		string busqueda;
		string tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";
		
		// Sumas Totales para los calculos
		float subtotal_al_15 = 0;
		float subtotal_al_0 = 0;
		float total_iva = 0;
		float sub_total = 0;
		float totaldescuento = 0;
		float deducible_caja = 0;
		float coaseguro_caja = 0;
		float honorariomedico = 0;
		
		// Variables publicas para le rango de fecha procedimiento
		string fecha_rango_1;
		string fecha_rango_2;
		
		bool aplico_cargos = false;
				
		string LoginEmpleado;
		string NomEmpleado = "";
		string AppEmpleado = "";
		string ApmEmpleado = "";
			
		string connectionString;
		string nombrebd;		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
					
		CellRendererText cel_descripcion;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public caja_cobro(string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_ ) 
		{
			LoginEmpleado = LoginEmp;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			valoriva = float.Parse(classpublic.ivaparaaplicar);	
			
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "caja", null);
			gxml.Autoconnect (this);
	        
			// Muestra ventana de Glade
			caja.Show();

			// Creacion de los treeview para la pantalla	
			crea_treeview_servicio();
			crea_treeview_cargextra();

	    	// Voy a buscar el folio que capturo
			button_selec_folio.Clicked += new EventHandler(on_selec_folio_clicked);
			
			// Validando que sen solo numeros
			entry_folio_servicio.KeyPressEvent += onKeyPressEvent_enter_folio;
			
			// Activacion de grabacion de informacion
	    	button_graba_pago.Clicked += new EventHandler(on_button_graba_pago_clicked);
	    	
			// Activacion de boton de busqueda
			button_buscar_paciente.Clicked += new EventHandler(on_button_buscar_paciente_clicked);
			
			// Graba el pago para el cierre de esta cuenta
			button_cierre_cuenta.Clicked += new EventHandler(on_button_cierre_cuenta_clicked);
			
			//graba el bloqueo de esta cuanta para que solo caja pueda realizar cargos
			button_bloquea_cuenta.Clicked += new EventHandler(on_button_bloquea_cuenta_clicked);
			
			//Alta de paciente
			button_alta_paciente.Clicked += new EventHandler(on_button_alta_paciente_clicked);
			
			// Imprime resumen de honorarios Medicos
			button_honorario_medico.Clicked += new EventHandler(on_button_honorario_medico_clicked);
			// Imprimime protocolo
			button_imprimir_protocolo.Clicked += new EventHandler(on_button_imprimir_protocolo_clicked);
			// Imprime Procedimiento
			button_procedimiento_cobrz.Clicked += new EventHandler(on_button_procedimiento_cobrz_clicked);
			//imprime totales de productos
			button_procedimiento_totales.Clicked += new EventHandler(on_button_procedimiento_totales_clicked);
			// Imprimime comprobante de caja
			button_compro_caja.Clicked += new EventHandler(on_button_compro_caja_clicked);
			// Imprimime comprobante de caja
			button_compro_serv.Clicked += new EventHandler(on_button_compro_serv_clicked);
			//quita lementos aplicados
			button_quitar_aplicados.Clicked += new EventHandler(on_button_quitar_aplicados_clicked);
			// Busqueda de Productos
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			// Aplica productos al cargo
			button_aplica_cargos.Clicked += new EventHandler(on_button_aplica_cargos_clicked);
			// Boton que quita cargos extras
			button_removerItem.Clicked += new EventHandler(on_button_removerItem_clicked);
			// Actualiza lista de cobros aplicados
			button_actualizar.Clicked += new EventHandler(on_button_actualizar_clicked);
			// Abonar a Procedimientos
			button_abonar.Clicked += new EventHandler(on_button_abonar_clicked);
			// Abre el folio de atencion para que se pueda alterarse
			button_abre_folio.Clicked += new EventHandler(on_button_abre_folio_clicked);
			// Comparacion de Cargos Extras
			button_compara_extras.Clicked += new EventHandler(on_button_compara_extras_clicked);
			// Traspasa cargos de un folio a otros mientras sea el mismo paciente
			button_traspasa_productos.Clicked += new EventHandler(on_button_traspasa_productos_clicked);
			
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
						
			// Desactivando Botones de operacion se activa cuando selecciona una atencion
			button_busca_producto.Sensitive = false;
			button_removerItem.Sensitive = false;
			button_graba_pago.Sensitive = false;
			button_aplica_cargos.Sensitive = false;
			button_abonar.Sensitive = false;
			button_alta_paciente.Sensitive = false;
			button_bloquea_cuenta.Sensitive = false;
			button_cierre_cuenta.Sensitive = false;
			button_honorario_medico.Sensitive = false;	
			button_imprimir_protocolo.Sensitive = false;
			button_procedimiento_cobrz.Sensitive = false;
			button_procedimiento_totales.Sensitive = false;
			button_abre_folio.Sensitive = false;
			button_traspasa_productos.Sensitive = false;
						
			button_compro_caja.Sensitive = false;
			button_compro_serv.Sensitive = false;
			button_quitar_aplicados.Sensitive = false;
			button_actualizar.Sensitive = false;
						
			statusbar_caja.Pop(0);
			statusbar_caja.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_caja.HasResizeGrip = false;
	    	
			// pone color a los entry
			entry_a_pagar.ModifyBase(StateType.Normal, new Gdk.Color(54,180,221));
			entry_total_iva.ModifyBase(StateType.Normal, new Gdk.Color(254,253,152));
							    
			// convierte un string a numero
			//string  prueba;
			//prueba=this.folioservicio.ToString();
			//prueba=int.Parse("787878");
		}
		
		void on_button_traspasa_productos_clicked(object sender, EventArgs args)
		{
			numerosecuencia_deta_cobro = "";
			if(LoginEmpleado =="DOLIVARES" || LoginEmpleado =="ADMIN"){
 				TreeIter iter;
 				TreeModel model;
	 			if (lista_de_servicios.Selection.GetSelected (out model, out iter)){	 					
					Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "cancelador_folios", null);
					gxml.Autoconnect (this);
			        
					// Muestra ventana de Glade
					cancelador_folios.Show();
					
					cancelador_folios.Title = "TRASPASO DE CARGOS";
					label_motivo.Text = "Producto a Traspasar ";
					
					entry_folio.KeyPressEvent += onKeyPressEvent_enter_folio;
					
					button_cancelar.Clicked += new EventHandler(on_traspasa_producto_clicked);
					// Sale de la ventana
					button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
					
					this.button_cancelar.Label = "Traspasar";
					
					entry_motivo.Text = (string) lista_de_servicios.Model.GetValue (iter,0);
					numerosecuencia_deta_cobro = (string) lista_de_servicios.Model.GetValue (iter,18);
					entry_motivo.IsEditable = false;
					
				}else{
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"Debe elejir un producto para poder realizar el traspaso, verifique...");
					msgBox.Run ();		msgBox.Destroy();
				}
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"No tiene Permiso para esta Opcion");
				msgBox.Run ();msgBox.Destroy();
			}
		}
		
		void on_traspasa_producto_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de traspasar el producto "+entry_motivo.Text+" ?");
			ResponseType miResultado = (ResponseType) msgBox.Run();
			msgBox.Destroy();
			
			if (miResultado == ResponseType.Yes){
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
    	        // Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
 					comando.CommandText = "SELECT pid_paciente FROM osiris_erp_cobros_enca WHERE pid_paciente = '"+this.entry_pid_paciente.Text.Trim()+"' "+
 								"AND folio_de_servicio = '"+this.entry_folio.Text.Trim()+"';";
 					comando.ExecuteNonQuery();    comando.Dispose();
					//Console.WriteLine(comando.CommandText);
					NpgsqlDataReader lector = comando.ExecuteReader ();
					if(lector.Read()){
						 NpgsqlConnection conexion1; 
						conexion1 = new NpgsqlConnection (connectionString+nombrebd);
		    	        // Verifica que la base de datos este conectada
						try{
							conexion1.Open ();
							NpgsqlCommand comando1; 
							comando1 = conexion1.CreateCommand ();
		 					comando1.CommandText = "UPDATE osiris_erp_cobros_deta "+
											"SET folio_de_servicio = '"+this.entry_folio.Text.Trim()+"' "+
											"WHERE id_secuencia =  '"+numerosecuencia_deta_cobro.Trim()+"';";
							//Console.WriteLine(comando.CommandText);
							comando1.ExecuteNonQuery();	    comando1.Dispose();
		    	    	}catch(NpgsqlException ex){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
											ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();		msgBoxError.Destroy();
						}
						conexion1.Close();
						cancelador_folios.Destroy();
						
						MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"El producto se traspaso satisfactoriamente...");
						msgBox1.Run ();		msgBox1.Destroy();
						
						llenado_de_productos_aplicados( (string) entry_folio_servicio.Text );
						
					}else{
						MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"No concuerda el PID con numero de atencion donde quiere traspasar");
						msgBox1.Run ();		msgBox1.Destroy();
					}
					//Console.WriteLine(comando.CommandText);
				}catch(NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();		msgBoxError.Destroy();
				}
				conexion.Close();
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
		    		idtipointernamiento = (int) combobox_tipo_admision.Model.GetValue(iter,1);
		    		descripinternamiento = (string) combobox_tipo_admision.Model.GetValue(iter,0);
	     	}
		}
		
		// Alta de paciente
		void on_button_alta_paciente_clicked(object sender, EventArgs a)
		{
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "causa_egreso", null);
			gxml.Autoconnect (this);
	        
			// Muestra ventana de Glade
			causa_egreso.Show();
			
			radiobutton_mejoria.Clicked += new EventHandler(on_radiobutton_mejoria_clicked);
			radiobutton_evolucion.Clicked += new EventHandler(on_radiobutton_evolucion_clicked);
			radiobutton_traslado.Clicked += new EventHandler(on_radiobutton_traslado_clicked);
			radiobutton_voluntaria.Clicked += new EventHandler(on_radiobutton_voluntaria_clicked);
			radiobutton_no_mejoria.Clicked += new EventHandler(on_radiobutton_no_mejoria_clicked);
			radiobutton_defuncion.Clicked += new EventHandler(on_radiobutton_defuncion_clicked);
			button_acepta_alta.Clicked += new EventHandler(on_button_acepta_alta_clicked);
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);	
		}
		
		void on_button_acepta_alta_clicked (object sender, EventArgs args)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Desea dar de alta al paciente ?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
			observacionesalta = entry_observacion_egreso.Text;
			if (miResultado == ResponseType.Yes){
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
    	        // Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
 					comando.CommandText = "UPDATE osiris_erp_cobros_enca "+
									"SET alta_paciente = 'true', "+
									//"id_habitacion = '1', "+
									"fecha_alta_paciente = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
									"id_empleado_alta_paciente = '"+LoginEmpleado+"', "+
									"motivo_alta_paciente = '"+causa_de_alta_paciente.ToString()+"', "+
									"observaciones_de_alta = ' "+observacionesalta.ToString()+" ' "+
									"WHERE  folio_de_servicio =  '"+this.folioservicio+"';";
					Console.WriteLine(comando.CommandText);
					comando.ExecuteNonQuery();
    	    	    comando.Dispose();
    	    	    conexion.Close();
    	    	    
    	    	    NpgsqlConnection conexion1; 
					conexion1 = new NpgsqlConnection (connectionString+nombrebd);
	    	        // Verifica que la base de datos este conectada
					try{
						conexion1.Open ();
						NpgsqlCommand comando1; 
						comando1 = conexion1.CreateCommand ();
	 					comando1.CommandText = "UPDATE osiris_his_habitaciones "+
										"SET disponible = 'true', "+
										"folio_de_servicio = '0',"+
										"pid_paciente = '0' "+
										"WHERE id_habitacion = '"+this.idhabitacion.ToString().Trim()+"';";
						//Console.WriteLine(comando.CommandText);
						comando1.ExecuteNonQuery();
	    	    	    comando1.Dispose();
	    	    	    conexion1.Close();
	    	    	    button_busca_producto.Sensitive = true;
			        	button_alta_paciente.Sensitive = false;
						button_cierre_cuenta.Sensitive = true;
						button_compro_caja.Sensitive = true;
						button_compro_serv.Sensitive = true;
					}catch(NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();		msgBoxError.Destroy();
					}
					conexion1.Close();
    	    	}catch(NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
				conexion.Close();
				causa_de_alta_paciente = "Por Mejoria";
    	    	causa_egreso.Destroy();
			}
		}
		
		void on_radiobutton_mejoria_clicked(object sender, EventArgs args)
		{
			causa_de_alta_paciente = "Por Mejoria";
		}
		void on_radiobutton_evolucion_clicked(object sender, EventArgs args)
		{
			causa_de_alta_paciente = "Por Evolucion";
		}
		void on_radiobutton_traslado_clicked(object sender, EventArgs args)
		{
			causa_de_alta_paciente = "Por Traslado";
		}
		void on_radiobutton_voluntaria_clicked(object sender, EventArgs args)
		{
			causa_de_alta_paciente = "Por Voluntaria";
		}
		void on_radiobutton_no_mejoria_clicked(object sender, EventArgs args)
		{			
			causa_de_alta_paciente = "Por NO Mejoria";
		}
		void on_radiobutton_defuncion_clicked(object sender, EventArgs args)
		{
			causa_de_alta_paciente = "Por Defuncion";
		}
		
		// Cierre de cuanta nadie va poder realizar cargos
		void on_button_cierre_cuenta_clicked (object sender, EventArgs a)
		{
			cierre_de_procedimiento();
		}
		
		void cierre_de_procedimiento()
		{
		   	MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
								ButtonsType.YesNo,"¿ Desea CERRAR esta cuenta ?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
	 		decimal totalproc = decimal.Parse(entry_a_pagar.Text) - decimal.Parse(entry_honorario_med_caja.Text);
	 		//Console.WriteLine(totalproc+" y "+totalproc);
	 		if(miResultado == ResponseType.Yes){
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
			    // Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
			 		comando.CommandText = "UPDATE osiris_erp_cobros_enca "+
											"SET cerrado = 'true', "+
											"total_procedimiento = '"+totalproc.ToString("F")+"',"+
											"subtotal15 = '"+this.entry_subtotal_al_15.Text.Trim()+"',"+
											"subtotal0 = '"+this.entry_subtotal_al_0.Text.Trim()+"',"+
											"fechahora_cerrado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
											"contador_cerrados = contador_cerrados + 1,"+ 
											"historial_de_cerrado = historial_de_cerrado || '"+LoginEmpleado+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n' "+
		 									"WHERE  folio_de_servicio =  '"+this.folioservicio+"';";
		 			//Console.WriteLine(comando.CommandText.ToString());
					comando.ExecuteNonQuery();			        comando.Dispose();
					
			        msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
								MessageType.Info,ButtonsType.Ok,"El Folio "+folioservicio+" se cerro satisfactoriamente");
					msgBox.Run ();			msgBox.Destroy();
	       			conexion.Close ();
			        //Console.WriteLine(comando.CommandText.ToString());
			        button_busca_producto.Sensitive = false;
			        button_honorario_medico.Sensitive = true;
			       	button_cierre_cuenta.Sensitive = false;
			        button_bloquea_cuenta.Sensitive = false;
			        button_procedimiento_totales.Sensitive = true;
			        button_compro_caja.Sensitive = false;
					button_compro_serv.Sensitive = false;
					button_abre_folio.Sensitive = true;
			        
				}catch (NpgsqlException ex){
				   	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
				}				
			}
		}
		
		// Accion de Bloque de una cuenta para que no realicen mas cargos exepto el cajero
		void on_button_bloquea_cuenta_clicked(object sender, EventArgs a)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Question,ButtonsType.YesNo,"¿ Desea BLOQUEAR esta cuenta ?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
	 			
	 		if (miResultado == ResponseType.Yes){
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
			    // Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
			 		comando.CommandText = "UPDATE osiris_erp_cobros_enca "+
								"SET bloqueo_de_folio = 'true' ,"+
								"historial_de_bloqueo = historial_de_bloqueo ||'"+LoginEmpleado+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n' "+
		 						"WHERE  folio_de_servicio =  '"+this.folioservicio+"';";
					comando.ExecuteNonQuery();
			        comando.Dispose();
			        //Console.WriteLine(comando.CommandText.ToString());
			        //button_busca_producto.Sensitive = false;
			        msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
								MessageType.Info,ButtonsType.Ok,"El Folio "+folioservicio+" se bloqueo satisfactoriamente");
					miResultado = (ResponseType)msgBox.Run ();
					msgBox.Destroy();  			conexion.Close ();
	       			button_bloquea_cuenta.Sensitive = false;
			        	
			    }catch (NpgsqlException ex){
				   	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				}
	       	}
		}
		
		void on_button_abre_folio_clicked(object sender, EventArgs args)
		{
			if(LoginEmpleado == "DOLIVARES" || LoginEmpleado =="ADMIN" ){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
							MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de Abrir este Nº de Atencion ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 			
		 		if (miResultado == ResponseType.Yes){
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
				    // Verifica que la base de datos este conectada
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
				 		comando.CommandText = "UPDATE osiris_erp_cobros_enca "+
									"SET cerrado = 'false' "+
									"WHERE  folio_de_servicio =  '"+this.folioservicio+"';";
						comando.ExecuteNonQuery();
				        comando.Dispose();
				        //Console.WriteLine(comando.CommandText.ToString());
				        //button_busca_producto.Sensitive = false;
				        msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"El Folio "+folioservicio+" se abrio satisfactoriamente");
						miResultado = (ResponseType)msgBox.Run ();
						msgBox.Destroy();
		       			conexion.Close ();
		       			this.button_cierre_cuenta.Sensitive = true;
		       			this.button_busca_producto.Sensitive = true;
		       			this.button_compro_caja.Sensitive = true;
		       			this.button_compro_serv.Sensitive = true;
		       			this.button_bloquea_cuenta.Sensitive = true;
		       			this.button_honorario_medico.Sensitive = false;
		       			this.button_procedimiento_totales.Sensitive = false;
		       			this.button_abre_folio.Sensitive = false;
		       			this.button_abonar.Sensitive = true;
		       			this.button_quitar_aplicados.Sensitive = true;
		       			this.button_traspasa_productos.Sensitive = true;
				        	
				    }catch (NpgsqlException ex){
					   	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
											ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
					}
		       	}
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"No tiene Permiso para esta Opcion");
				msgBox.Run ();msgBox.Destroy();
			}
		}
				
		void on_button_compara_extras_clicked(object sender, EventArgs args)
		{
			if (this.folioservicio == 0 ){	
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "Debe de llenar el campo de Folio de Atencion con uno existente ");
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}else{
				new osiris.validacion_cargos_extras(this.folioservicio.ToString().Trim(),this.nombrebd);	
			}			
		}
		
		// Abonando a los procedimientos
		void on_button_abonar_clicked(object sender, EventArgs args)
		{
			if (this.folioservicio == 0 ){	
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "Debe de llenar el campo de Folio de Atencion con uno existente");
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}else{
				new osiris.abonos(PidPaciente,this.folioservicio,nombrebd,
						entry_ingreso.Text,entry_egreso.Text,entry_numero_factura.Text,
						entry_nombre_paciente.Text,entry_telefono_paciente.Text,entry_doctor.Text,
						entry_tipo_paciente.Text,entry_aseguradora.Text,edadpac+" Años y "+mesespac+" Meses",fecha_nacimiento,
						dir_pac,cirugia,empresapac,id_tipopaciente,NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado,this.LoginEmpleado,this.agregarmasabonos);
			}
		}
		
		// Imprime protocolo de admision
		void on_button_imprimir_protocolo_clicked(object sender, EventArgs args)
		{
			if (this.folioservicio == 0 ){	
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "Debe de llenar el campo de Folio con uno \n"+
							"existente para que el protocolo se muestre \n"+"o no a pulsado el boton ''Seleccionar''");
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}else{
				new protocolo_admision(PidPaciente,this.folioservicio,nombrebd,"");   // rpt_prot_admision.cs
			}
		}
		
		// Imprime honorarios medicos
		void on_button_honorario_medico_clicked(object sender, EventArgs args)
		{
			//Console.WriteLine(this.folioservicio);
			if (this.folioservicio == 0){	
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "Debe de llenar el campo de Folio con uno \n"+
							"existente para poder agregar un honorario ");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}else{
				Glade.XML gxml = new Glade.XML (null, "caja.glade", "honorario_medico", null);
				gxml.Autoconnect (this);
	        
				// Muestra ventana de Glade
				honorario_medico.Show();
				entry_valor_honorario.GrabFocus();			
				entry_valor_honorario.KeyPressEvent += onKeyPressEvent;
				entry_totalbonos_medicos.Text = totalhonorarios.ToString();
				//abre la ventana de busqueda de medicos
				button_busca_doctores.Clicked += new EventHandler(on_button_busca_doctores_clicked);
				if (this.agregarmashonorario == true){
					button_guardar_honorario.Sensitive = true;
					button_busca_doctores.Sensitive = true;
				}else{
					button_guardar_honorario.Sensitive = false;
					button_busca_doctores.Sensitive = false;
				}
				button_guardar_honorario.Clicked += new EventHandler(on_button_guardar_honorario_clicked);
				
				button_quitar_honorario.Clicked += new EventHandler(on_quita_honorario_clicked);
				button_pago_honorario.Clicked += new EventHandler(on_button_pago_honorario_clicked);
				// Imprime formato para la solicitud de cheque
				button_solicitud_cheque.Clicked += new EventHandler(on_button_solicitud_cheque_clicked);
				//Imprimir la hoja de comprobante de honorarios medicoc
				button_imprime_honorarios.Clicked += new EventHandler(on_button_imprime_honorarios_clicked);
				
				button_imprime_honorarios.Sensitive = true;
				
				//Sale de la ventana
				button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
				
				treeViewEngineHonorarios = new TreeStore( typeof(string),
													typeof(string),
													typeof(string),typeof(string),
													typeof(string),
													typeof(bool),
													typeof(string),
													typeof(string),
													typeof(bool));
				
				lista_de_honorarios.Model = treeViewEngineHonorarios;
				lista_de_honorarios.RulesHint = true;
				
				TreeViewColumn col_idmedico = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_idmedico.Title = "ID Medico"; // titulo de la cabecera de la columna, si está visible
				col_idmedico.PackStart(cellr0, true);
				col_idmedico.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1
				col_idmedico.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_fila_hono));
	            
				TreeViewColumn col_nombrmedico = new TreeViewColumn();
				CellRendererText cellrt1 = new CellRendererText();
				col_nombrmedico.Title = "Nombre Medico";
				col_nombrmedico.PackStart(cellrt1, true);
				col_nombrmedico.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
	            col_nombrmedico.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_fila_hono));
	            
				TreeViewColumn col_espemedico = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_espemedico.Title = "Especialidad";
				col_espemedico.PackStart(cellrt2, true);
				col_espemedico.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 3
				col_espemedico.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores_fila_hono));
	            
				TreeViewColumn col_hono_medico = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_hono_medico.Title = "Honorario";
				col_hono_medico.PackStart(cellrt3, true);
				col_hono_medico.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 4
				col_hono_medico.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_fila_hono));
				
				TreeViewColumn col_fechahora_hono = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_fechahora_hono.Title = "Fecha-Hora Honorario";
				col_fechahora_hono.PackStart(cellrt4,true);
				col_fechahora_hono.AddAttribute (cellrt4, "text", 4);
				col_fechahora_hono.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_fila_hono));
				
				TreeViewColumn col_fechahora_pago = new TreeViewColumn();
				CellRendererText cellrt6 = new CellRendererText();
				col_fechahora_pago.Title = "Fecha/Hora Pago";
				col_fechahora_pago.PackStart(cellrt6,true);
				col_fechahora_pago.AddAttribute (cellrt6, "text", 6);
				col_fechahora_pago.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_fila_hono));
				
				lista_de_honorarios.AppendColumn(col_idmedico);
				lista_de_honorarios.AppendColumn(col_nombrmedico);
				lista_de_honorarios.AppendColumn(col_espemedico);
				lista_de_honorarios.AppendColumn(col_hono_medico);
				lista_de_honorarios.AppendColumn(col_fechahora_hono);
				lista_de_honorarios.AppendColumn(col_fechahora_pago);
				
				llenado_de_honorarios();
			}
		}
		
		void llenado_de_honorarios()
		{
			totalhonorarios = 0;  // limpia variable
			treeViewEngineHonorarios.Clear();
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(osiris_erp_honorarios_medicos.id_medico,'999999') AS idmedico, "+ 
									"to_char(monto_del_abono,'9999999.99') AS montodelabono, "+
									"to_char(fechahora_abono,'dd-mm-yyyy HH24:mi:ss') AS fechaabono, "+
									"to_char(id_abono,'999999999') AS idabono, "+
									"to_char(fecha_pago,'dd-mm-yyyy') AS fechapago,"+
									"pagado,"+
									"nombre_medico,descripcion_especialidad "+
									"FROM osiris_erp_honorarios_medicos,osiris_his_medicos,osiris_his_tipo_especialidad "+
									"WHERE osiris_erp_honorarios_medicos.folio_de_servicio = '"+folioservicio+"' "+
									"AND osiris_erp_honorarios_medicos.eliminado = 'false' "+
									"AND osiris_erp_honorarios_medicos.id_medico = osiris_his_medicos.id_medico "+
									"AND osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad ;";
				comando.ExecuteNonQuery();    comando.Dispose();
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				float toma_honorario = 0;
				string toma_fecha_pago = "";
				while(lector.Read())
				{
					toma_honorario = float.Parse((string) lector["montodelabono"]);
					totalhonorarios += toma_honorario; 
					entry_totalbonos_medicos.Text = totalhonorarios.ToString();
					toma_fecha_pago = "";
					if ((bool) lector["pagado"] == true){
						toma_fecha_pago = (string) lector["fechapago"];
					}
					
					treeViewEngineHonorarios.AppendValues((string) lector["idmedico"],
														(string) lector["nombre_medico"],
														(string) lector["descripcion_especialidad"],
														(string) lector["montodelabono"],
														(string) lector["fechaabono"],
														(bool) lector["pagado"],
														toma_fecha_pago,
														(string) lector["idabono"],
														true);
				}
				
			}catch (NpgsqlException ex){
				//Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message );
				msgBoxError.Run ();			msgBoxError.Destroy();
	   		}
			conexion.Close ();
		}
		
		void on_button_pago_honorario_clicked(object sender, EventArgs args)
		{
			TreeIter iter; 
			TreeModel model;
			pagodehonorario = true;
			if (this.lista_de_honorarios.Selection.GetSelected (out model, out iter)){
				if ((bool) this.lista_de_honorarios.Model.GetValue (iter,5) == false){
					Glade.XML gxml = new Glade.XML (null, "caja.glade", "comprobante_pago", null);
					gxml.Autoconnect (this);
					comprobante_pago.Show();
						                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
					entry_numero_comprobante.KeyPressEvent += onKeyPressEvent;
					entry_total_comprobante.KeyPressEvent += onKeyPressEvent;
					
					label_pago.Text = "Valor Honorario $";
					label_comp_remision.Text = "Numero Remision ";
					
					entry_total_comprobante.Text = (string)this.lista_de_honorarios.Model.GetValue (iter,3);
					idhonorariomedico = (string)this.lista_de_honorarios.Model.GetValue (iter,7);
					
					this.entry_dia1.Text = DateTime.Now.ToString("dd");
					this.entry_mes1.Text = DateTime.Now.ToString("MM");
					this.entry_ano1.Text = DateTime.Now.ToString("yyyy");
					entry_hora_compr.Text = DateTime.Now.ToString("HH:mm:ss");
					
					button_guardar_pago.Clicked += new EventHandler(on_button_guardar_pago_clicked);
					
					button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
					llenado_formapago();
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close, "El Honorario Medico seleccionado ya esta pagado, verifique...");
					msgBoxError.Run ();	msgBoxError.Destroy();
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close, "Seleccione un Honorario Medico");
				msgBoxError.Run ();	msgBoxError.Destroy();
			}
		}
		
		void on_button_solicitud_cheque_clicked(object sender, EventArgs args)
		{			
			TreeIter iter;
 			TreeModel model;
 			 			
 			if (this.lista_de_honorarios.Selection.GetSelected (out model, out iter)){ 				
				if((bool) this.lista_de_honorarios.Model.GetValue (iter,8)){					
					//new osiris.rpt_solicitud_cheque((string) this.lista_de_honorarios.Model.GetValue (iter,1),
				    //                            (string) this.lista_de_honorarios.Model.GetValue (iter,3), entry_tipo_paciente.Text.Trim(), entry_aseguradora.Text.Trim(), entry_nombre_paciente.Text.Trim(), entry_folio_servicio.Text.Trim());
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					                                               MessageType.Error,ButtonsType.Close, "Debe de estar guardado el honorario");
					msgBoxError.Run ();			msgBoxError.Destroy();	
				}		
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Error,ButtonsType.Close, "Debe seleccionar un doctor para poder realizar la solicitud");
				msgBoxError.Run ();			msgBoxError.Destroy();
			
			}
		}

		
		void on_button_busca_doctores_clicked(object sender, EventArgs args)
		{
			if(entry_valor_honorario.Text.Trim() == "0" || entry_valor_honorario.Text.Trim() == "" ){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "Debe de llenar el campo de honorario medico con un valor mayor a cero ");
				msgBoxError.Run ();			msgBoxError.Destroy();
				entry_valor_honorario.GrabFocus();
			}else{
				busqueda = "medicos";
				Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador_medicos", null);
				gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          (this);
		        llenado_cmbox_tipo_busqueda();
		        entry_expresion.KeyPressEvent += onKeyPressEvent_busqueda;
				button_buscar_busqueda.Clicked += new EventHandler(on_button_llena_medicos_clicked);
				button_selecciona.Clicked += new EventHandler(on_selecciona_medico_clicked);
		        button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
				
				treeViewEngineMedicos = new TreeStore( typeof(int),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
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
			if (combobox_tipo_busqueda.GetActiveIter (out iter)){
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
			if(numbusqueda == 7)  { tipobusqueda = "AND osiris_his_medicos.id_medico LIKE '"; }//Console.WriteLine(tipobusqueda); }
		}		
		
		void on_selecciona_medico_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			float toma_valor = 0;
			if (lista_de_medicos.Selection.GetSelected(out model, out iterSelected)){
				idmedico =(int) model.GetValue(iterSelected, 0);
 				nombmedico = (string) model.GetValue(iterSelected, 1)+" "+(string) model.GetValue(iterSelected, 2)+" "+
 							(string) model.GetValue(iterSelected, 3)+" "+(string) model.GetValue(iterSelected, 4);
 				especialidadmed = (string) model.GetValue(iterSelected, 5);
			  	toma_valor = float.Parse(entry_valor_honorario.Text);
			  	entry_medico.Text = nombmedico;
			  	//Console.WriteLine("toma_valor "+toma_valor.ToString());
			  	totalhonorarios += toma_valor;
			  	//Console.WriteLine("totalhonorarios "+totalhonorarios.ToString());
			  	entry_totalbonos_medicos.Text = totalhonorarios.ToString();
			  	treeViewEngineHonorarios.AppendValues(idmedico.ToString(),
			  											nombmedico,
			  											especialidadmed,
			  											toma_valor.ToString(),
														DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
														false,
														"",
														"",
														false);
				entry_valor_honorario.Text = "";
				button_quitar_aplicados.Sensitive = true;
 				// cierra la ventana despues que almaceno la informacion en variables
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
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
						if ((string) entry_expresion.Text.ToUpper().Trim() == ""){
							comando.CommandText = "SELECT id_medico, "+
										"to_char(id_empresa,'999999') AS idempresa, "+
										"to_char(osiris_his_tipo_especialidad.id_especialidad,'999999') AS idespecialidad, "+
										"nombre_medico,descripcion_empresa,descripcion_especialidad,centro_medico, "+
										"nombre1_medico,nombre2_medico,apellido_paterno_medico,apellido_materno_medico, "+
										"telefono1_medico,cedula_medico,telefono2_medico,celular1_medico,celular2_medico, "+
										"nextel_medico,beeper_medico,cedula_medico,direccion_medico,direccion_consultorio_medico, "+
										"to_char(fecha_ingreso_medico,'dd-MM-yyyy') AS fecha_ingreso, "+
										"to_char(fecha_revision_medico,'dd-MM-yyyy') AS fecha_revision, "+
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
										"to_char(fecha_ingreso_medico,'dd-MM-yyyy') AS fecha_ingreso, "+
										"to_char(fecha_revision_medico,'dd-MM-yyyy') AS fecha_revision, "+
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
						//Console.WriteLine("medicos"+comando.CommandText);
						while (lector.Read()){
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
		
		void on_button_imprime_honorarios_clicked(object sender, EventArgs args)
		{
			//string  query = " ";			
			new rpt_honorario_med(PidPaciente,this.folioservicio,nombrebd,
						entry_ingreso.Text,entry_egreso.Text,entry_numero_factura.Text,
						entry_nombre_paciente.Text,entry_telefono_paciente.Text,entry_doctor.Text,
						entry_tipo_paciente.Text,entry_aseguradora.Text,edadpac+" Años y "+mesespac+" Meses",fecha_nacimiento,
						dir_pac,cirugia,empresapac,id_tipopaciente,entry_totalbonos_medicos.Text,"0");   // rpt_honorarios_medicos.cs
			honorario_medico.Destroy();
		}
						
		void on_button_guardar_honorario_clicked(object sender, EventArgs args)
		{
			bool graboinformacion = false;
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de guardar esta informacion ?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
	 			
	 		if (miResultado == ResponseType.Yes){
	 		
	 			NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
			    // Verifica que la base de datos este conectada
			    try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					TreeIter iter;
					if (treeViewEngineHonorarios.GetIterFirst (out iter)){
						if ((bool)lista_de_honorarios .Model.GetValue (iter,8) == false){		
										
							comando.CommandText = "INSERT INTO osiris_erp_honorarios_medicos( "+
			 									"id_medico, "+
			 									"monto_del_abono, "+
			 									"folio_de_servicio, "+
			 									"fechahora_abono, "+
			 									"id_quien_abono) "+
			 									"VALUES ('"+
 												int.Parse((string) lista_de_honorarios.Model.GetValue(iter,0))+"','"+
 												decimal.Parse((string) lista_de_honorarios.Model.GetValue(iter,3))+"','"+
 												folioservicio+"','"+
 												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
 												LoginEmpleado+"');";
		 					comando.ExecuteNonQuery();
					    	comando.Dispose();
					    	graboinformacion = true;
				    	}
				    	while (treeViewEngineHonorarios.IterNext(ref iter)){
				   			if ((bool)lista_de_honorarios.Model.GetValue (iter,8) == false){
				   				comando.CommandText = "INSERT INTO osiris_erp_honorarios_medicos( "+
			 									"id_medico, "+
			 									"monto_del_abono, "+
			 									"folio_de_servicio, "+
			 									"fechahora_abono, "+
			 									"id_quien_abono) "+
			 									"VALUES ('"+
 												int.Parse((string) lista_de_honorarios.Model.GetValue(iter,0))+"','"+
 												decimal.Parse((string) lista_de_honorarios.Model.GetValue(iter,3))+"','"+
 												folioservicio+"','"+
 												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
 												LoginEmpleado+"');";
	 							comando.ExecuteNonQuery();
				    			comando.Dispose();
				    			graboinformacion = true;
				    		}
				    	}
				    }
				    conexion.Close();
				    if(graboinformacion == true){
					    NpgsqlConnection conexion1; 
						conexion1 = new NpgsqlConnection (connectionString+nombrebd);
					    try{
					    	conexion1.Open ();
							NpgsqlCommand comando1; 
							comando1 = conexion1.CreateCommand ();
							
							comando1.CommandText = "UPDATE osiris_erp_cobros_enca "+
										"SET honorario_medico = '"+entry_totalbonos_medicos.Text.Trim()+"' "+
										"WHERE  folio_de_servicio =  '"+this.folioservicio+"';";
							comando1.ExecuteNonQuery();
					        comando1.Dispose();
					     
					     	button_imprime_honorarios.Sensitive = true;
					     	entry_honorario_med_caja.Text = entry_valor_honorario.Text.Trim();
							
					        //Console.WriteLine(comando.CommandText.ToString());
					        
					        msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"El Honorario Medico con folio : "+folioservicio+" se GUARDO satisfactoriamente");
							miResultado = (ResponseType)msgBox.Run ();
							msgBox.Destroy();
							
			       			conexion1.Close ();
			       			
			       			// Desactivando boton de quitar productos 
							button_quitar_aplicados.Sensitive = false;
					       	llenado_de_honorarios(); 	
					    }catch (NpgsqlException ex){
					   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();		msgBoxError.Destroy();
						}
					}else{
						msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Error,ButtonsType.Ok,"El Honorario ya se encuentra Grabado, verifique...");
						miResultado = (ResponseType) msgBox.Run ();
						msgBox.Destroy();
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
			}	
		}
		
		void on_quita_honorario_clicked (object sender, EventArgs args)
		{
			TreeIter iter;
 			TreeModel model;
 			if (lista_de_honorarios.Selection.GetSelected (out model, out iter)) {
 				float toma_honorario = float.Parse((string) this.lista_de_honorarios.Model.GetValue(iter,3));
 				
 				if (!(bool) lista_de_honorarios.Model.GetValue (iter,5)){
 					totalhonorarios -= toma_honorario;
					entry_totalbonos_medicos.Text = totalhonorarios.ToString();
 					treeViewEngineHonorarios.Remove (ref iter);
				}
			}
		}		
								
		// Imprime procedimiento de cobranza por rango de fechas
		void on_button_procedimiento_cobrz_clicked(object sender, EventArgs args)
		{
			imprime_proc_por_fecha_totales("procedimiento");
		}
		
		// Imprime el resumen de la factura
		void on_button_procedimiento_totales_clicked(object sender, EventArgs args)
		{
			imprime_proc_por_fecha_totales("resumen_factura");
		}
		
		void imprime_proc_por_fecha_totales(string tipo_de_reporte)
		{
			tipodereporte = tipo_de_reporte;
			if ((string) entry_folio_servicio.Text == "" || (string) entry_pid_paciente.Text == "" ){	
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "Debe de llenar el campo de Folio con uno \n"+
							"existente para que el procedimiento se muestre \n"+"o no a pulsado el boton ''Seleccionar''");
				msgBoxError.Run ();				msgBoxError.Destroy();
			}else{
				
				Glade.XML gxml = new Glade.XML (null, "caja.glade", "rango_de_fecha", null);
				gxml.Autoconnect (this);
	        
				// Muestra ventana de Glade
				rango_de_fecha.Show();
				label_orden.ChildVisible = false;
				radiobutton_cliente.ChildVisible = false;
				radiobutton_fecha.ChildVisible = false;
				label_nom_cliente.ChildVisible = false;
				entry_cliente.ChildVisible = false;
				button_busca_cliente.ChildVisible = false;
				checkbutton_todos_los_clientes.ChildVisible = false;
		
				entry_dia1.Text = fecha_rango_2.Substring(0,2);
				entry_dia1.KeyPressEvent += onKeyPressEvent;				
				entry_dia2.Text = fecha_rango_2.Substring(0,2);
				entry_dia2.KeyPressEvent += onKeyPressEvent;
				
				entry_mes1.Text = fecha_rango_2.Substring(3,2);
				entry_mes1.KeyPressEvent += onKeyPressEvent;				
				entry_mes2.Text = fecha_rango_2.Substring(3,2);
				entry_mes2.KeyPressEvent += onKeyPressEvent;
				
				entry_ano1.Text = fecha_rango_2.Substring(6,4);
				entry_ano1.KeyPressEvent += onKeyPressEvent;
				entry_ano2.Text = fecha_rango_2.Substring(6,4);
				entry_ano2.KeyPressEvent += onKeyPressEvent;
				
				if (tipodereporte == "resumen_factura"){
					checkbutton_impr_todo_proce.Active = true;
				}
				
				entry_referencia_inicial.Text = fecha_rango_1;
				button_imprime_rangofecha.Clicked += new EventHandler(on_button_imprime_rangofecha_clicked);
				
				// Sale de la ventana
				button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			}	
		}
		
		void on_button_imprime_rangofecha_clicked(object sender, EventArgs args)
		{
			string query;
			if (checkbutton_impr_todo_proce.Active == true){
				query = " ";
			}else{
				query = "AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') >= '"+entry_ano1.Text.Trim()+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
						"AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') <= '"+entry_ano2.Text.Trim()+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";
			}
			
			if (tipodereporte == "procedimiento"){
				
				new osiris. proc_cobranza (PidPaciente,folioservicio,nombrebd,
						entry_ingreso.Text,entry_egreso.Text,entry_numero_factura.Text,
						entry_nombre_paciente.Text,entry_telefono_paciente.Text,entry_doctor.Text,
						entry_tipo_paciente.Text,entry_aseguradora.Text,edadpac+" Años y "+mesespac+" Meses",fecha_nacimiento,dir_pac,
						cirugia,empresapac,id_tipopaciente,query);   // rpt_proc_cobranza.cs
				
			}
			if (tipodereporte == "resumen_factura"){
				// rpt_proc_totales.cs
				new proc_totales (PidPaciente,this.folioservicio,nombrebd,
						entry_ingreso.Text,entry_egreso.Text,entry_numero_factura.Text,
						entry_nombre_paciente.Text,entry_telefono_paciente.Text,entry_doctor.Text,
						entry_tipo_paciente.Text,entry_aseguradora.Text,edadpac+" Años y "+mesespac+" Meses",fecha_nacimiento,dir_pac,
						cirugia,empresapac,id_tipopaciente,query);				
			}			
			rango_de_fecha.Destroy();
		}
				
		void on_button_compro_caja_clicked(object sender, EventArgs args)
		{
			if ((string) entry_folio_servicio.Text == "" || (string) entry_pid_paciente.Text == "" )
		    {	
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, ButtonsType.Close, "Debe de llenar el campo de Folio con uno \n"+
							"existente para que el comprobante se muestre \n"+"o no a pulsado el boton ''Seleccionar''");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}else{
				pagodehonorario = false;
				Glade.XML gxml = new Glade.XML (null, "caja.glade", "comprobante_pago", null);
				gxml.Autoconnect (this);
				comprobante_pago.Show();
				
				entry_numero_comprobante.KeyPressEvent += onKeyPressEvent;
				entry_total_comprobante.KeyPressEvent += onKeyPressEvent;
				entry_total_comprobante.Text = this.entry_a_pagar.Text;
				
				this.entry_dia1.Text = DateTime.Now.ToString("dd");
				this.entry_mes1.Text = DateTime.Now.ToString("MM");
				this.entry_ano1.Text = DateTime.Now.ToString("yyyy");
				this.entry_hora_compr.Text = DateTime.Now.ToString("HH:mm:ss");
				
				this.entry_dia1.IsEditable = false;
				this.entry_mes1.IsEditable = false;
				this.entry_ano1.IsEditable = false;
				entry_hora_compr.IsEditable = false;
				
				button_guardar_pago.Clicked += new EventHandler(on_button_guardar_pago_clicked);
				button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
				llenado_formapago();
			}
		}
		
		void on_button_guardar_pago_clicked(object sender, EventArgs args)
		{
			string descrippago = "";
			
			if (entry_numero_comprobante.Text.Trim() != "" && idformadepago > 1){
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();				
				// Actualiza informacion en pagos y abonos
				if (idformadepago > 1 ){
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
								MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de realizar esta operacion de Pago ?");
								ResponseType miResultado = (ResponseType) msgBox.Run ();
					msgBox.Destroy();
	 				//Console.WriteLine(miResultado.ToString());
	 				if (miResultado == ResponseType.Yes){
				
						//if (guado_el_abono == false){
						if (pagodehonorario == false){
							descrippago = "PAGO DE PROCEDIMIENTO";
						}else{
							descrippago = "PAGO DE HONORARIO MEDICO";
						}
						
	 					NpgsqlConnection conexion; 
						conexion = new NpgsqlConnection (connectionString+nombrebd);
						// Verifica que la base de datos este conectada
						try{
							conexion.Open ();
							NpgsqlCommand comando; 
							comando = conexion.CreateCommand ();
				 			comando.CommandText = "INSERT INTO osiris_erp_abonos("+
				 								"monto_de_abono_procedimiento,"+
				 								"folio_de_servicio,"+
				 								"concepto_del_abono,"+
				 								"fecha_abono,"+
				 								"id_quien_creo,"+
				 								"id_forma_de_pago,"+
				 								"pago,"+
				 								"fechahora_registro,"+
				 								"honorario_medico,"+
				 								"pago_honorario_medico,"+
				 								"numero_factura,"+
				 								"numero_recibo_caja) "+
				 								"VALUES ('"+
				 								(string) this.entry_total_comprobante.Text.Trim()+"','"+
				 								folioservicio+"','"+
				 								descrippago+"','"+
				 								DateTime.Now.ToString("yyyy-MM-dd")+"','"+
	 											LoginEmpleado+"','"+
	 											idformadepago.ToString()+"','"+
	 											pagodehonorario+"','"+
	 											DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
	 											pagodehonorario+"','"+
	 											"true','"+
	 											this.entry_numero_factura.Text+"','"+
	 											(string) entry_numero_comprobante.Text.Trim()+"');";
	 											
			 				comando.ExecuteNonQuery();    	    	       	comando.Dispose();
			 				if (pagodehonorario == false){
				 				NpgsqlConnection conexion1; 
								conexion1 = new NpgsqlConnection (connectionString+nombrebd);
								// Verifica que la base de datos este conectada
								try{
									conexion1.Open ();
									NpgsqlCommand comando1; 
									comando1 = conexion1.CreateCommand ();
						 			comando1.CommandText = "UPDATE osiris_erp_cobros_enca "+
											"SET pagado = 'true',"+	
											"total_pago = '"+(string) this.entry_total_comprobante.Text.Trim()+"' "+							
											"WHERE  folio_de_servicio =  '"+this.folioservicio+"';";
						 			comando1.ExecuteNonQuery();    	    	       	comando1.Dispose();
						 			
						 			cierre_de_procedimiento();				 			
						 			comprobante_de_caja_pago();
						 							 			 	  
				 				}catch(NpgsqlException ex){
					   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
														MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
									msgBoxError.Run ();					msgBoxError.Destroy();
					       		}
					       		conexion1.Close();
					       	}else{
					       		// Marca el pago del honorario en la tabla de honorarios medicos
					       		NpgsqlConnection conexion1; 
								conexion1 = new NpgsqlConnection (connectionString+nombrebd);
								// Verifica que la base de datos este conectada
								try{
									conexion1.Open ();
									NpgsqlCommand comando1; 
									comando1 = conexion1.CreateCommand ();
						 			comando1.CommandText = "UPDATE osiris_erp_honorarios_medicos "+
											"SET pagado = 'true',"+	
											"fecha_pago = '"+this.entry_ano1.Text+"-"+this.entry_mes1.Text+"-"+this.entry_dia1.Text+"' "+			
											"WHERE id_abono = '"+idhonorariomedico+"';";
						 			comando1.ExecuteNonQuery();    	    	       	comando1.Dispose();
						 			
						 			comando1.CommandText = "UPDATE osiris_erp_cobros_enca "+
											"SET total_abonos = total_abonos + '"+(string) this.entry_total_comprobante.Text.Trim()+"' "+							
											"WHERE  folio_de_servicio =  '"+this.folioservicio+"';";
						 			comando1.ExecuteNonQuery();    	    	       	comando1.Dispose();
						 			
						 			llenado_de_honorarios();
						 							 			 	  
				 				}catch(NpgsqlException ex){
					   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
														MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
									msgBoxError.Run ();					msgBoxError.Destroy();
					       		}
					       		conexion1.Close();
					       	}
						}catch(NpgsqlException ex){
				   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();					msgBoxError.Destroy();
				       	}
				       	conexion.Close();
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close, "Especifique la Forma de Pago....");
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close, "Falta comprobante de caja o la Forma de Pago....");
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
		}
		
		void comprobante_de_caja_pago()
		{
			new caja_comprobante ( PidPaciente,this.folioservicio,nombrebd,
					entry_ingreso.Text,entry_egreso.Text,entry_numero_factura.Text,
					entry_nombre_paciente.Text,entry_telefono_paciente.Text,entry_doctor.Text,
					entry_tipo_paciente.Text,entry_aseguradora.Text,edadpac+" Años y "+mesespac+" Meses",
					fecha_nacimiento,dir_pac,cirugia,empresapac,id_tipopaciente,NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado,
					this.entry_total_abonos_caja.Text.Trim());
		}
		
		void on_button_compro_serv_clicked(object sender, EventArgs args)
		{
			if ((string) entry_folio_servicio.Text == "" || (string) entry_pid_paciente.Text == "" )
		    {	
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close, "Debe de llenar el campo de Folio con uno \n"+
							"existente para que el comprobante se muestre \n"+"o no a pulsado el boton ''Seleccionar''");
				msgBoxError.Run ();				msgBoxError.Destroy();
			}else{
				// rpt_comprobante_serv.cs
				new comprobante_serv ( PidPaciente,this.folioservicio,nombrebd,
						entry_ingreso.Text,entry_egreso.Text,entry_numero_factura.Text,
						entry_nombre_paciente.Text,entry_telefono_paciente.Text,entry_doctor.Text,
						entry_tipo_paciente.Text,entry_aseguradora.Text,edadpac+" Años y "+mesespac+" Meses",fecha_nacimiento,dir_pac,cirugia,empresapac,id_tipopaciente);
			}
		}
		
		void llenado_formapago()
		{
			combobox_formapago.Clear();
			
			CellRendererText cell3 = new CellRendererText();
			combobox_formapago.PackStart(cell3, true);
			combobox_formapago.AddAttribute(cell3,"text",0);
	        
			ListStore store5 = new ListStore( typeof (string), typeof (int));
			combobox_formapago.Model = store5;
						
	        NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_erp_forma_de_pago "+
               						"WHERE proveedor = false "+	
               						"ORDER BY descripcion_forma_de_pago;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read())
				{
					store5.AppendValues ((string) lector["descripcion_forma_de_pago"],
									 	(int) lector["id_forma_de_pago"] );
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
	        
			TreeIter iter5;
			if (store5.GetIterFirst(out iter5)){
				combobox_formapago.SetActiveIter (iter5);
			}
			combobox_formapago.Changed += new EventHandler (onComboBoxChanged_formapago);
		}
		
		void onComboBoxChanged_formapago (object sender, EventArgs args)
		{
			ComboBox combobox_formapago = sender as ComboBox;
			if (sender == null) {return;}
			TreeIter iter;
			if (combobox_formapago.GetActiveIter (out iter)){ 
				idformadepago = (int) combobox_formapago.Model.GetValue(iter,1);
			}
		}
		
		// Crea el treeview de los servicios y/o productos aplicados
		void crea_treeview_servicio()
		{
			treeViewEngineServicio = new TreeStore(typeof(string), 
													typeof(float),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string), 
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(int),
													typeof(bool),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));
												
			lista_de_servicios.Model = treeViewEngineServicio;
			lista_de_servicios.RulesHint = true;
				
			TreeViewColumn col_descripcion_hc = new TreeViewColumn();
			CellRendererText cel_descripcion = new CellRendererText();
			col_descripcion_hc.Title = "Servicio/Producto"; // titulo de la cabecera de la columna, si está visible
			col_descripcion_hc.PackStart(cel_descripcion, true);
			col_descripcion_hc.AddAttribute (cel_descripcion, "text", 0);
			col_descripcion_hc.SortColumnId = (int) Column_serv.col_descripcion_hc ;
			col_descripcion_hc.Resizable = true;
			
			col_descripcion_hc.SetCellDataFunc(cel_descripcion, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			//cel_descripcion.Foreground = "darkblue";
									
			TreeViewColumn col_cantidad_hc = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_cantidad_hc.Title = "Cantidad"; // titulo de la cabecera de la columna, si está visible
			col_cantidad_hc.PackStart(cellr1, true);
			col_cantidad_hc.AddAttribute (cellr1, "text", 1);
			col_cantidad_hc.SortColumnId = (int) Column_serv.col_cantidad_hc;
						
			TreeViewColumn col_codigo_prod_hc = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_codigo_prod_hc.Title = "Codigo Prod."; // titulo de la cabecera de la columna, si está visible
			col_codigo_prod_hc.PackStart(cellr2, true);
			col_codigo_prod_hc.AddAttribute (cellr2, "text", 2);
			col_codigo_prod_hc.SortColumnId = (int) Column_serv.col_codigo_prod_hc;
			        
			TreeViewColumn col_precio_hc = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_precio_hc.Title = "P.Unitario"; // titulo de la cabecera de la columna, si está visible
			col_precio_hc.PackStart(cellr3, true);
			col_precio_hc.AddAttribute (cellr3, "text", 3);
			col_precio_hc.SortColumnId = (int) Column_serv.col_precio_hc;
			
			TreeViewColumn col_ppor_cantidad_hc = new TreeViewColumn();
			CellRendererText cellr4 = new CellRendererText();
			col_ppor_cantidad_hc.Title = "Sub-Total"; // titulo de la cabecera de la columna, si está visible
			col_ppor_cantidad_hc.PackStart(cellr4, true);
			col_ppor_cantidad_hc.AddAttribute (cellr4, "text", 4);
			col_ppor_cantidad_hc.SortColumnId = (int) Column_serv.col_ppor_cantidad_hc;
        
			TreeViewColumn col_iva_hc = new TreeViewColumn();
			CellRendererText cellr5 = new CellRendererText();
			col_iva_hc.Title = "I.V.A."; // titulo de la cabecera de la columna, si está visible
			col_iva_hc.PackStart(cellr5, true);
			col_iva_hc.AddAttribute (cellr5, "text", 5);
			col_iva_hc.SortColumnId = (int) Column_serv.col_iva_hc;
        
			TreeViewColumn col_sub_total_hc = new TreeViewColumn();
			CellRendererText cellr6 = new CellRendererText();
			col_sub_total_hc.Title = "Total"; // titulo de la cabecera de la columna, si está visible
			col_sub_total_hc.PackStart(cellr6, true);
			col_sub_total_hc.AddAttribute (cellr6, "text", 6);
			col_sub_total_hc.SortColumnId = (int) Column_serv.col_sub_total_hc;

			TreeViewColumn col_por_desc_hc = new TreeViewColumn();
			CellRendererText cellr7 = new CellRendererText();
			col_por_desc_hc.Title = "% Desc."; // titulo de la cabecera de la columna, si está visible
			col_por_desc_hc.PackStart(cellr7, true);
			col_por_desc_hc.AddAttribute (cellr7, "text", 7);
			col_por_desc_hc.SortColumnId = (int) Column_serv.col_por_desc_hc;

			TreeViewColumn col_valor_desc_hc = new TreeViewColumn();
			CellRendererText cellr8 = new CellRendererText();
			col_valor_desc_hc.Title = "Valor Desc."; // titulo de la cabecera de la columna, si está visible
			col_valor_desc_hc.PackStart(cellr8, true);
			col_valor_desc_hc.AddAttribute (cellr8, "text", 8);
			col_valor_desc_hc.SortColumnId = (int) Column_serv.col_valor_desc_hc;

			TreeViewColumn col_total_hc = new TreeViewColumn();
			CellRendererText cellr9 = new CellRendererText();
			col_total_hc.Title = "Precio c/Desc."; // titulo de la cabecera de la columna, si está visible
			col_total_hc.PackStart(cellr9, true);
			col_total_hc.AddAttribute (cellr9, "text", 9);
			col_total_hc.SortColumnId = (int) Column_serv.col_total_hc;
        
			TreeViewColumn col_quien_cargo_hc = new TreeViewColumn();
			CellRendererText cellr10 = new CellRendererText();
			col_quien_cargo_hc.Title = "Quien cargo"; // titulo de la cabecera de la columna, si está visible
			col_quien_cargo_hc.PackStart(cellr10, true);
			col_quien_cargo_hc.AddAttribute (cellr10, "text", 10);
			col_quien_cargo_hc.SortColumnId = (int) Column_serv.col_quien_cargo_hc;
        
			TreeViewColumn col_fecha_hora_hc = new TreeViewColumn();
			CellRendererText cellr11 = new CellRendererText();
			col_fecha_hora_hc.Title = "Fecha/Hora"; // titulo de la cabecera de la columna, si está visible
			col_fecha_hora_hc.PackStart(cellr11, true);
			col_fecha_hora_hc.AddAttribute (cellr11, "text", 11);
			col_fecha_hora_hc.SortColumnId = (int) Column_serv.col_fecha_hora_hc;

			TreeViewColumn col_asignado_hc = new TreeViewColumn();
			CellRendererText cellr12 = new CellRendererText();
			col_asignado_hc.Title = "Asignado a"; // titulo de la cabecera de la columna, si está visible
			col_asignado_hc.PackStart(cellr12, true);
			col_asignado_hc.AddAttribute (cellr12, "text", 12);
			col_asignado_hc.SortColumnId = (int) Column_serv.col_asignado_hc;
			
			//TreeViewColumn col_idasignado_hc = new TreeViewColumn();
			
			//TreeViewColumn col_almacenado = new TreeViewColumn();
        
			lista_de_servicios.AppendColumn(col_descripcion_hc);
			lista_de_servicios.AppendColumn(col_cantidad_hc);
			lista_de_servicios.AppendColumn(col_codigo_prod_hc);
			lista_de_servicios.AppendColumn(col_precio_hc);
			lista_de_servicios.AppendColumn(col_ppor_cantidad_hc);
			lista_de_servicios.AppendColumn(col_iva_hc);
			lista_de_servicios.AppendColumn(col_sub_total_hc);
			lista_de_servicios.AppendColumn(col_por_desc_hc);
			lista_de_servicios.AppendColumn(col_valor_desc_hc);
			lista_de_servicios.AppendColumn(col_total_hc);
			lista_de_servicios.AppendColumn(col_quien_cargo_hc);
			lista_de_servicios.AppendColumn(col_fecha_hora_hc);
			lista_de_servicios.AppendColumn(col_asignado_hc);
			
			//lista_de_servicios.RowExpanded += on_expandrows_RowExpanded;
		}
		
		enum Column_serv
		{
			col_descripcion_hc,
			col_cantidad_hc,
			col_codigo_prod_hc,
			col_precio_hc,
			col_ppor_cantidad_hc,
			col_iva_hc,
			col_sub_total_hc,
			col_por_desc_hc,
			col_valor_desc_hc,
			col_total_hc,
			col_quien_cargo_hc,
			col_fecha_hora_hc,
			col_asignado_hc
		}
		
		// funcion para cambiar el color de una fila y columna
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//descripcion_producto descrip = (descripcion_producto) model.GetValue (iter, 14);
			if ((bool)lista_de_servicios.Model.GetValue (iter,14)==true){
				(cell as Gtk.CellRendererText).Foreground = "darkblue";
			}else{
				(cell as Gtk.CellRendererText).Foreground = "red";
			}
			
		}
		
		void cambia_colores_fila_hono(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((bool)lista_de_honorarios.Model.GetValue (iter,5)==true){
				(cell as Gtk.CellRendererText).Foreground = "darkblue";
			}else{
			
				(cell as Gtk.CellRendererText).Foreground = "red";
			}
			
			if ((bool)lista_de_honorarios.Model.GetValue (iter,8)==false && (bool)lista_de_honorarios.Model.GetValue (iter,5)==false){
				(cell as Gtk.CellRendererText).Foreground = "black";
			}
			
		}

		void crea_treeview_cargextra()
		{
			arraycargosextras = new ArrayList();
			
			treeViewEngineExtras = new ListStore(typeof(bool), 
													typeof(float),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string), 
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(int),
													typeof(string),
													typeof(string),
													typeof(string));
												
			lista_cargos_extras.Model = treeViewEngineExtras;
			
			lista_cargos_extras.RulesHint = true;
				
			TreeViewColumn col_seleccion = new TreeViewColumn();
			CellRendererToggle cellr0 = new CellRendererToggle();
			col_seleccion.Title = "Seleccion"; // titulo de la cabecera de la columna, si está visible
			col_seleccion.PackStart(cellr0, true);
			//col_seleccion.SetCellDataFunc(cellr0, new TreeCellDataFunc (BoolCellDataFunc));  // funcion de columna
			col_seleccion.AddAttribute (cellr0, "active", 0);
			cellr0.Activatable = true;
			cellr0.Toggled += selecciona_fila; 
		
			TreeViewColumn col_cantidad = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_cantidad.Title = "Cantidad"; // titulo de la cabecera de la columna, si está visible
			col_cantidad.PackStart(cellr1, true);
			col_cantidad.AddAttribute (cellr1, "text", 1);
			cellr1.Editable = true;   // Permite edita este campo
			cellr1.Edited += new EditedHandler (NumberCellEdited);
			cellr1.Foreground = "darkblue";
			
			TreeViewColumn col_codigo_prod = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_codigo_prod.Title = "Codigo Prod."; // titulo de la cabecera de la columna, si está visible
			col_codigo_prod.PackStart(cellr2, true);
			col_codigo_prod.AddAttribute (cellr2, "text", 2);

			TreeViewColumn col_descripcion = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_descripcion.Title = "Descripcion"; // titulo de la cabecera de la columna, si está visible
			col_descripcion.PackStart(cellr3, true);
			col_descripcion.AddAttribute (cellr3, "text", 3);
        
			TreeViewColumn col_precio = new TreeViewColumn();
			CellRendererText cellr4 = new CellRendererText();
			col_precio.Title = "P.Unitario"; // titulo de la cabecera de la columna, si está visible
			col_precio.PackStart(cellr4, true);
			col_precio.AddAttribute (cellr4, "text", 4);
			
			TreeViewColumn col_ppor_cantidad = new TreeViewColumn();
			CellRendererText cellr5 = new CellRendererText();
			col_ppor_cantidad.Title = "Sub-Total"; // titulo de la cabecera de la columna, si está visible
			col_ppor_cantidad.PackStart(cellr5, true);
			col_ppor_cantidad.AddAttribute (cellr5, "text", 5);
        
			TreeViewColumn col_iva = new TreeViewColumn();
			CellRendererText cellr6 = new CellRendererText();
			col_iva.Title = "I.V.A."; // titulo de la cabecera de la columna, si está visible
			col_iva.PackStart(cellr6, true);
			col_iva.AddAttribute (cellr6, "text", 6);
        
			TreeViewColumn col_sub_total = new TreeViewColumn();
			CellRendererText cellr7 = new CellRendererText();
			col_sub_total.Title = "Total"; // titulo de la cabecera de la columna, si está visible
			col_sub_total.PackStart(cellr7, true);
			col_sub_total.AddAttribute (cellr7, "text", 7);

			TreeViewColumn col_por_desc = new TreeViewColumn();
			CellRendererText cellr8 = new CellRendererText();
			col_por_desc.Title = "% Desc."; // titulo de la cabecera de la columna, si está visible
			col_por_desc.PackStart(cellr8, true);
			col_por_desc.AddAttribute (cellr8, "text", 8);

			TreeViewColumn col_valor_desc = new TreeViewColumn();
			CellRendererText cellr9 = new CellRendererText();
			col_valor_desc.Title = "Valor Desc."; // titulo de la cabecera de la columna, si está visible
			col_valor_desc.PackStart(cellr9, true);
			col_valor_desc.AddAttribute (cellr9, "text", 9);

			TreeViewColumn col_total = new TreeViewColumn();
			CellRendererText cellr10 = new CellRendererText();
			col_total.Title = "Total"; // titulo de la cabecera de la columna, si está visible
			col_total.PackStart(cellr10, true);
			col_total.AddAttribute (cellr10, "text", 10);
        
			TreeViewColumn col_quien_cargo = new TreeViewColumn();
			CellRendererText cellr11 = new CellRendererText();
			col_quien_cargo.Title = "Quien cargo"; // titulo de la cabecera de la columna, si está visible
			col_quien_cargo.PackStart(cellr11, true);
			col_quien_cargo.AddAttribute (cellr11, "text", 11);
        
			TreeViewColumn col_fecha_hora = new TreeViewColumn();
			CellRendererText cellr12 = new CellRendererText();
			col_fecha_hora.Title = "Fecha/Hora"; // titulo de la cabecera de la columna, si está visible
			col_fecha_hora.PackStart(cellr12, true);
			col_fecha_hora.AddAttribute (cellr12, "text", 12);

			TreeViewColumn col_asignado = new TreeViewColumn();
			CellRendererText cellr13 = new CellRendererText();
			col_asignado.Title = "Asignado a"; // titulo de la cabecera de la columna, si está visible
			col_asignado.PackStart(cellr13, true);
			col_asignado.AddAttribute (cellr13, "text", 13);
			cellr13.CellBackground = "red";
			
        	//TreeViewColumn col_idasignado = new TreeViewColumn();
			//CellRendererText cellr14 = new CellRendererText();
			
			lista_cargos_extras.AppendColumn(col_seleccion);
			lista_cargos_extras.AppendColumn(col_cantidad);
			lista_cargos_extras.AppendColumn(col_codigo_prod);
			lista_cargos_extras.AppendColumn(col_descripcion);
			lista_cargos_extras.AppendColumn(col_precio);
			lista_cargos_extras.AppendColumn(col_ppor_cantidad);
			lista_cargos_extras.AppendColumn(col_iva);
			lista_cargos_extras.AppendColumn(col_sub_total);
			lista_cargos_extras.AppendColumn(col_por_desc);
			lista_cargos_extras.AppendColumn(col_valor_desc);
			lista_cargos_extras.AppendColumn(col_total);
			lista_cargos_extras.AppendColumn(col_quien_cargo);
			lista_cargos_extras.AppendColumn(col_fecha_hora);
			lista_cargos_extras.AppendColumn(col_asignado);
			//lista_cargos_extras.AppendColumn(col_idasignado);
			
		}
		
		// Cuando seleccion el treeview de cargos extras para cargar los productos  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (lista_cargos_extras.Model.GetIter (out iter, path)) {
				bool old = (bool) lista_cargos_extras.Model.GetValue (iter,0);
				lista_cargos_extras.Model.SetValue(iter,0,!old);
			}	
		}
		/*
		void on_expandrows_RowExpanded (object sender, EventArgs args)
		{
			//Gtk.TreeView mitree = sender as Gtk.TreeView;
		}
		*/
		void on_button_graba_pago_clicked(object sender, EventArgs args)
		{
			if ((bool) aplico_cargos){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");

				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
 
				if (miResultado == ResponseType.Yes){
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
    	        	// Verifica que la base de datos este conectada
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
 						TreeIter iter;
 						if ((int) folioservicio > 0){  // Validando que seleccione un folio de atencion
							if (treeViewEngineServicio.GetIterFirst (out iter)){
								if ((bool)lista_de_servicios.Model.GetValue (iter,14)==false){
									comando.CommandText = "INSERT INTO osiris_erp_cobros_deta("+
 												"id_producto,"+
 												"folio_de_servicio,"+
 												"pid_paciente,"+
 												"cantidad_aplicada,"+
 												"id_tipo_admisiones,"+
 												"precio_producto, "+
 												//"precio_por_cantidad,"+
 												//"iva_producto,"+
 												"precio_costo_unitario,"+
 												"porcentage_utilidad,"+
 												"porcentage_descuento,"+
 												"id_empleado,"+
 												"fechahora_creacion,"+
 												"porcentage_iva,"+
 												"precio_costo) "+
 												"VALUES ('"+
 												double.Parse((string) lista_de_servicios.Model.GetValue(iter,2))+"','"+
 												folioservicio+"','"+
 												int.Parse((string)entry_pid_paciente.Text)+"','"+
 												(float) lista_de_servicios.Model.GetValue(iter,1)+"','"+
 												(int)lista_de_servicios.Model.GetValue(iter,13)+"','"+
 												double.Parse((string)lista_de_servicios.Model.GetValue(iter,3))+"','"+
 												//double.Parse((string)lista_de_servicios.Model.GetValue(iter,4))+"','"+
 												//double.Parse((string)lista_de_servicios.Model.GetValue(iter,5))+"','"+
 												double.Parse((string)lista_de_servicios.Model.GetValue(iter,15))+"','"+
 												float.Parse((string)lista_de_servicios.Model.GetValue(iter,16))+"','"+
 												float.Parse((string) lista_de_servicios.Model.GetValue(iter,7))+"','"+
 												LoginEmpleado+"','"+
 												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
 												valoriva+"','"+
 												float.Parse((string)lista_de_servicios.Model.GetValue(iter,17))+
 												"');";
 									comando.ExecuteNonQuery();
    	    	       				comando.Dispose();
    	    	       			}	
    	    	       			while (treeViewEngineServicio.IterNext(ref iter)){
    	    	       				if ((bool)lista_de_servicios.Model.GetValue (iter,14) == false){
 										comando.CommandText = "INSERT INTO osiris_erp_cobros_deta("+
 														"id_producto,"+
 														"folio_de_servicio,"+
 														"pid_paciente,"+
 														"cantidad_aplicada,"+
 														"id_tipo_admisiones,"+
 														"precio_producto, "+
 														//"precio_por_cantidad,"+
 														//"iva_producto,"+
 														"precio_costo_unitario,"+
 														"porcentage_utilidad,"+
 														"porcentage_descuento,"+
 														"id_empleado,"+
 														"fechahora_creacion,"+
 														"porcentage_iva,"+
 														"precio_costo) "+
 														"VALUES ('"+
 														double.Parse((string) lista_de_servicios.Model.GetValue(iter,2))+"','"+
 														folioservicio+"','"+
 														int.Parse((string)entry_pid_paciente.Text)+"','"+
 														(float) lista_de_servicios.Model.GetValue(iter,1)+"','"+
 														(int)lista_de_servicios.Model.GetValue(iter,13)+"','"+
 														double.Parse((string)lista_de_servicios.Model.GetValue(iter,3))+"','"+
 														//double.Parse((string)lista_de_servicios.Model.GetValue(iter,4))+"','"+
 														//double.Parse((string)lista_de_servicios.Model.GetValue(iter,5))+"','"+
 														double.Parse((string)lista_de_servicios.Model.GetValue(iter,15))+"','"+
 														float.Parse((string)lista_de_servicios.Model.GetValue(iter,16))+"','"+
 														float.Parse((string) lista_de_servicios.Model.GetValue(iter,7))+"','"+
 														LoginEmpleado+"','"+
 														DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
 														valoriva+"','"+
 														float.Parse((string)lista_de_servicios.Model.GetValue(iter,17))+
 														"');";
 										comando.ExecuteNonQuery();
    	    	       					comando.Dispose();
 									}
 								}
 							}
 						}else{
 							//Console.WriteLine(folioservicio.ToString());
 							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Info, 
											ButtonsType.Close, "Seleccione algun folio de atencion...");
							msgBoxError.Run ();
							msgBoxError.Destroy();
 				
 						}
					}catch (NpgsqlException ex){
	   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
	       			}
       				conexion.Close ();
 					llenado_de_productos_aplicados( (string) entry_folio_servicio.Text );
 				}
 			}else{
 				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info, 
							ButtonsType.Close, "No grabo, ya que NO CARGO NADA");
				msgBoxError.Run ();
				msgBoxError.Destroy();
 			}
		}
		
		void on_button_aplica_cargos_clicked (object sender, EventArgs args)
		{
			// leyendo treeview de productos extras para cargar
			string toma_valor1;
			string toma_valor2;
			string toma_valor3;
			float toma_a_pagar = 0;
			aplico_cargos = true;
											
			TreeIter iter;
 			if (treeViewEngineExtras.GetIterFirst (out iter)){
 				//bool old1 = (bool) lista_cargos_extras.Model.GetValue (iter,0);
 				if ((float) lista_cargos_extras.Model.GetValue (iter,1) > 0){
 					if ((bool)lista_cargos_extras.Model.GetValue (iter,0)){
 						lista_cargos_extras.Model.SetValue(iter,0,false);
 						toma_valor1 = (string) lista_cargos_extras.Model.GetValue (iter,5);  // toma precio publico
 						toma_valor2 = (string) lista_cargos_extras.Model.GetValue (iter,6);  // toma el iva
 						toma_valor3 = (string) lista_cargos_extras.Model.GetValue (iter,9);  // toma el descuento
 						 					
 						if ((float) float.Parse(toma_valor2) > 0){
 							subtotal_al_15 = subtotal_al_15 + float.Parse(toma_valor1);
 						}else{
 					 		subtotal_al_0 = subtotal_al_0 + float.Parse(toma_valor1);
 						}
 						 						
 						// Verificando si aplica descuento por tarjeta de Descuento
 						if ((int) lista_cargos_extras.Model.GetValue (iter,14) == 100 && (int) id_tipopaciente == 101 && aplicar_siempre == false
					  		||(int) lista_cargos_extras.Model.GetValue (iter,14) == 300 && (int) id_tipopaciente == 101 && aplicar_siempre == false
					  		||(int) lista_cargos_extras.Model.GetValue (iter,14) == 400 && (int) id_tipopaciente == 101 && aplicar_siempre == false){
							aplicar_descuento = true;
						}else{
							if (aplicar_siempre == false){
								aplicar_descuento = false;
								aplicar_siempre = true;
							}
						}	
 						
 						total_iva = total_iva + float.Parse(toma_valor2);
 						totaldescuento = totaldescuento + float.Parse(toma_valor3)+((float.Parse(toma_valor3)*valoriva)/100);
 						 					 					 					
 						// Traspaso los valores de valores extras para que se carguen a la cuenta del
 						// paciente
 						treeViewEngineServicio.AppendValues ((string) lista_cargos_extras.Model.GetValue (iter,3),
 														(float) lista_cargos_extras.Model.GetValue (iter,1),
 														(string) lista_cargos_extras.Model.GetValue (iter,2),
 														(string) lista_cargos_extras.Model.GetValue (iter,4),
 														(string) lista_cargos_extras.Model.GetValue (iter,5),
 														(string) lista_cargos_extras.Model.GetValue (iter,6),
 														(string) lista_cargos_extras.Model.GetValue (iter,7),
 														(string) lista_cargos_extras.Model.GetValue (iter,8),
 														(string) lista_cargos_extras.Model.GetValue (iter,9),
 														(string) lista_cargos_extras.Model.GetValue (iter,10),
 														(string) lista_cargos_extras.Model.GetValue (iter,11),
 														(string) lista_cargos_extras.Model.GetValue (iter,12),
 														(string) lista_cargos_extras.Model.GetValue (iter,13),
 														(int) lista_cargos_extras.Model.GetValue (iter,14),
 														(bool) false,
 														(string) lista_cargos_extras.Model.GetValue (iter,15),
 														(string) lista_cargos_extras.Model.GetValue (iter,16),
 														(string) lista_cargos_extras.Model.GetValue (iter,17),
 														(string) "" );
 														
 					}
 				}
 				while (treeViewEngineExtras.IterNext(ref iter)){
 					if ((float) lista_cargos_extras.Model.GetValue (iter,1) > 0){
 						if ((bool)lista_cargos_extras.Model.GetValue (iter,0)){
 					
 							lista_cargos_extras.Model.SetValue(iter,0,false);
 							toma_valor1 = (string) lista_cargos_extras.Model.GetValue (iter,5);  // toma precio publico
 							toma_valor2 = (string) lista_cargos_extras.Model.GetValue (iter,6);  // toma el iva
 							toma_valor3 = (string) lista_cargos_extras.Model.GetValue (iter,9);  // toma el precio con descuento
 							
 							if ((float) float.Parse(toma_valor2) > 0){
 								subtotal_al_15 = subtotal_al_15 + float.Parse(toma_valor1);
 							}else{
 					 			subtotal_al_0 = subtotal_al_0 + float.Parse(toma_valor1);
 							}
 							if ((int) lista_cargos_extras.Model.GetValue (iter,14) == 100 && (int) id_tipopaciente == 101 && aplicar_siempre == false
					  			||(int) lista_cargos_extras.Model.GetValue (iter,14) == 300 && (int) id_tipopaciente == 101 && aplicar_siempre == false
					  			||(int) lista_cargos_extras.Model.GetValue (iter,14) == 400 && (int) id_tipopaciente == 101 && aplicar_siempre == false){
								aplicar_descuento = true;
							}else{
								if (aplicar_siempre == false){
									aplicar_descuento = false;
									aplicar_siempre = true;
								}
							}
							
 							total_iva = total_iva + float.Parse(toma_valor2);
 							totaldescuento = totaldescuento + float.Parse(toma_valor3)+((float.Parse(toma_valor3)*valoriva)/100);
 							 						
 							treeViewEngineServicio.AppendValues (
 										(string) lista_cargos_extras.Model.GetValue (iter,3),
 										(float) lista_cargos_extras.Model.GetValue (iter,1),
 										(string) lista_cargos_extras.Model.GetValue (iter,2),
 										(string) lista_cargos_extras.Model.GetValue (iter,4),
 										(string) lista_cargos_extras.Model.GetValue (iter,5),
 										(string) lista_cargos_extras.Model.GetValue (iter,6),
 										(string) lista_cargos_extras.Model.GetValue (iter,7),
 										(string) lista_cargos_extras.Model.GetValue (iter,8),
 										(string) lista_cargos_extras.Model.GetValue (iter,9),
 										(string) lista_cargos_extras.Model.GetValue (iter,10),
 										(string) lista_cargos_extras.Model.GetValue (iter,11),
 										(string) lista_cargos_extras.Model.GetValue (iter,12),
 										(string) lista_cargos_extras.Model.GetValue (iter,13),
 										(int) lista_cargos_extras.Model.GetValue (iter,14),
 										(bool) false,
 										(string) lista_cargos_extras.Model.GetValue (iter,15),
 										(string) lista_cargos_extras.Model.GetValue (iter,16),
 										(string) lista_cargos_extras.Model.GetValue (iter,17),
 										(string) "");
 						}
 					}
 				}
 				if (aplicar_descuento == false){
					totaldescuento = 0;
				}
							
				// Realizando las restas
				sub_total = subtotal_al_15 + subtotal_al_0+total_iva;
				toma_a_pagar = sub_total - totaldescuento;
				
 				entry_subtotal_al_15.Text = subtotal_al_15.ToString("F");
 				entry_subtotal_al_0.Text = subtotal_al_0.ToString("F");
 				entry_total_iva.Text = total_iva.ToString("F");
 				entry_subtotal.Text = sub_total.ToString("F");
 				entry_totaldescuento.Text = totaldescuento.ToString("F");
 				entry_total.Text = sub_total.ToString("F");
 				entry_a_pagar.Text = toma_a_pagar.ToString("F");
 			}
 		}
		
		// Remueve productos aplicados que ya han sido cargados pero no actualizados
		// en la tabla de detalle
		void on_button_quitar_aplicados_clicked (object sender, EventArgs args)
		{
			TreeIter iter;
 			TreeModel model;
 			string toma_valor1;
 			string toma_valor2;
 			string toma_valor3;
 			float toma_a_pagar = 0;
 			
 			if (lista_de_servicios.Selection.GetSelected (out model, out iter)) {
 				toma_valor1 = (string) lista_de_servicios.Model.GetValue(iter,4);	
				toma_valor2 = (string) lista_de_servicios.Model.GetValue (iter,5);  // toma el iva
				toma_valor3 = (string) lista_de_servicios.Model.GetValue (iter,8);  // toma el descuento
				
				// verifica el codigo de admision no sea laboratorio
				if ((int) lista_de_servicios.Model.GetValue (iter,13) != 400 ){  				
					if (!(bool) lista_de_servicios.Model.GetValue (iter,14)){
									
	 					treeViewEngineServicio.Remove (ref iter);
	 				 					
	 					if ((float) float.Parse(toma_valor2) > 0){
	 						subtotal_al_15 = subtotal_al_15 - float.Parse(toma_valor1);
	 					}else{
	 						subtotal_al_0 = subtotal_al_0 - float.Parse(toma_valor1);
	 					}
	 					total_iva = total_iva - float.Parse(toma_valor2);
					
						sub_total = subtotal_al_15 + subtotal_al_0 + total_iva;
						totaldescuento -= (float.Parse(toma_valor3) + ((float.Parse(toma_valor3) * valoriva)/100));
						toma_a_pagar = sub_total - totaldescuento;
						
						entry_subtotal_al_15.Text = subtotal_al_15.ToString("F");
	 					entry_subtotal_al_0.Text = subtotal_al_0.ToString("F");
	 					entry_total_iva.Text = total_iva.ToString("F");
	 					entry_subtotal.Text = sub_total .ToString("F");
	 					entry_totaldescuento.Text = totaldescuento.ToString("F");
	 					entry_total.Text = sub_total.ToString("F");
	 					entry_a_pagar.Text = toma_a_pagar.ToString("F");
	 				}else{
	 					string descripcionprod = (string) lista_de_servicios.Model.GetValue (iter,0);
	 					if (LoginEmpleado =="DOLIVARES" || LoginEmpleado =="ADMIN"){
	 						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
							MessageType.Question,ButtonsType.YesNo,"¿ Desea DEVOLVER este producto ? \n"+"Descripcion: "+descripcionprod);
							ResponseType miResultado = (ResponseType)msgBox.Run ();
							msgBox.Destroy();
		 					
		 					if (miResultado == ResponseType.Yes){
								NpgsqlConnection conexion; 
								conexion = new NpgsqlConnection (connectionString+nombrebd);
				    			// Verifica que la base de datos este conectada
								try{
									conexion.Open ();
									NpgsqlCommand comando; 
									comando = conexion.CreateCommand ();
									// Marcando registro para su eliminacion
									comando.CommandText = "UPDATE osiris_erp_cobros_deta "+
											"SET eliminado = 'true' , "+
											"fechahora_eliminado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
											"id_quien_elimino = '"+LoginEmpleado+"' "+								
					 						"WHERE id_secuencia =  '"+(string) lista_de_servicios.Model.GetValue (iter,18)+"';";
											comando.ExecuteNonQuery();
						        			comando.Dispose();
					 				
					 				// Duplicando registro registro pero con cantidad negativa y elimacion activa			        			
						        	comando.CommandText = "INSERT INTO osiris_erp_cobros_deta("+
						        							"id_producto,"+
						        							"folio_de_servicio,"+
						        							"pid_paciente,"+
						        											"eliminado,"+
						        											"fechahora_eliminado,"+
						        											"id_quien_elimino,"+
						        											"id_secuencia,"+
						        							"id_tipo_admisiones,"+
						        							"precio_producto,"+
						        							//"iva_producto,"+
						        							"id_tipo_admisiones2,"+
						        							"precio_costo_unitario,"+
						        							"porcentage_utilidad,"+
						        							"porcentage_descuento,"+
						        							"porcentage_iva,"+
						        							"precio_costo,"+
						        										"fechahora_creacion,"+
										        			"id_empleado,"+
										        			"cantidad_aplicada,"+
										        			"id_almacen) "+
										        			//"precio_por_cantidad )"+
										        			//"numero_factura) "+
										        			
						        							"SELECT "+
						        							"id_producto,"+
						        							"folio_de_servicio,"+
						        							"pid_paciente,"+
						        											"'true',"+
						        											"'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
						        											"'"+LoginEmpleado+"',"+					        							
						        											"NEXTVAL('osiris_erp_cobros_deta_id_secuencia_seq'),"+
						        							"id_tipo_admisiones,"+
						        							"precio_producto * -1,"+
						        							//"iva_producto * -1,"+
						        							"id_tipo_admisiones2,"+
						        							"precio_costo_unitario * -1,"+
						        							"porcentage_utilidad,"+
						        							"porcentage_descuento,"+
						        							"porcentage_iva,"+
						        							"precio_costo,"+
						        											"'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
						        											"'"+LoginEmpleado+"',"+	
										        			"cantidad_aplicada * -1,"+
										        			"id_almacen "+
										        			//"precio_por_cantidad * -1 "+
										        			//"numero_factura "+
										        			"FROM osiris_erp_cobros_deta "+
						        							"WHERE id_secuencia =  '"+(string) lista_de_servicios.Model.GetValue (iter,18)+"';";
						        			//Console.WriteLine(comando.CommandText.ToString());
						        	comando.ExecuteNonQuery();     	comando.Dispose();
				        			msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
											MessageType.Info,ButtonsType.Ok,"El Producto "+descripcionprod+
											"\n se devolvio satisfactoriamente");
									msgBox.Run ();		msgBox.Destroy();
									
									llenado_de_productos_aplicados( (string) entry_folio_servicio.Text );
									
									conexion.Close ();
				        		}catch (NpgsqlException ex){
					   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
											ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
									msgBoxError.Run ();
									msgBoxError.Destroy();
					   		
								}
	 							//Console.WriteLine((string) lista_de_servicios.Model.GetValue (iter,18));
	 						}
	 					}else{
	 						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
							MessageType.Error,ButtonsType.Ok,"No esta autorizado para esta opcion...");
							msgBox.Run();	msgBox.Destroy();
	 					}
	 				}
 				}else{
 					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
					MessageType.Error,ButtonsType.Ok,"No esta autorizado para esta opcion, verifique con LABORATORIO... ");
					msgBox.Run();	msgBox.Destroy();
 				}
 			}
		}
		
		void on_selec_folio_clicked(object sender, EventArgs args)
		{
			llenado_de_productos_aplicados( (string) entry_folio_servicio.Text );
		}
		
		void on_button_actualizar_clicked(object sender, EventArgs args)
		{
			llenado_de_productos_aplicados( (string) entry_folio_servicio.Text );
		}
		
		// Este toma los valores para llenar el encabezado del procedimiento
		// Aqui lleno el detalle de los servicios que se va aplicar para su cobro
		void llenado_de_productos_aplicados(string foliodeserv)
		{
			subtotal_al_15 = 0;
			subtotal_al_0 = 0;
			total_iva = 0;
			totaldescuento = 0;
			sub_total = 0;
			id_tipopaciente = 0;
			idempresa_paciente = 0;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio AS foliodeatencion, "+
								"osiris_erp_cobros_enca.pagado,"+
								"osiris_erp_cobros_enca.cancelado,"+
								"osiris_erp_cobros_enca.cerrado,"+
								"osiris_erp_cobros_enca.alta_paciente,"+
								"osiris_erp_cobros_enca.bloqueo_de_folio,"+
								"to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente,"+
				            	"nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente,"+
				            	"telefono_particular1_paciente,numero_poliza,folio_de_servicio_dep,osiris_empresas.descripcion_empresa,"+
				            	"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH24:mi:ss') AS fecha_ingreso,"+
				            	"to_char(osiris_erp_cobros_enca.fecha_alta_paciente,'dd-MM-yyyy HH24:mi:ss') AS fecha_egreso,"+
				            	"to_char(osiris_erp_cobros_enca.numero_factura,'9999999999') AS numerofactura,"+
				            	"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fecha_nac_pa, "+
				            	"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy'),'9999'),'9999') AS edad,"+
								"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
				            	"to_char(osiris_erp_cobros_enca.deducible,'9999999.99') AS deduciblecaja, "+
				            	"to_char(osiris_erp_cobros_enca.total_abonos,'99999999.99') AS totalabonos, "+
				            	"to_char(osiris_erp_cobros_enca.total_pago,'99999999.99') AS totalpago, "+
				            	"to_char(osiris_erp_cobros_enca.coaseguro,'999.99') AS coasegurocaja,"+
				            	"to_char(osiris_erp_cobros_enca.numero_factura,'9999999999') AS numfactura,"+
				            	"osiris_erp_movcargos.id_tipo_paciente AS idtipopaciente, "+
				            	"osiris_erp_movcargos.id_tipo_admisiones AS idtipoadmision, "+
				            	"osiris_his_paciente.direccion_paciente,osiris_his_paciente.numero_casa_paciente,osiris_his_paciente.numero_departamento_paciente, "+
								"osiris_his_paciente.colonia_paciente,osiris_his_paciente.municipio_paciente,osiris_his_paciente.codigo_postal_paciente,osiris_his_paciente.estado_paciente,  "+
            					"descripcion_tipo_paciente,osiris_his_tipo_cirugias.descripcion_cirugia,"+
            					"osiris_his_paciente.id_empresa AS idempresa,osiris_empresas.lista_de_precio AS listadeprecio_empresa,"+   ///
            					"descripcion_admisiones,osiris_his_tipo_especialidad.descripcion_especialidad,"+
				            	"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,osiris_aseguradoras.lista_de_precio AS listadeprecio_aseguradora,"+   ///
				            	"osiris_erp_cobros_enca.id_medico,nombre_medico, "+
				            	"osiris_erp_movcargos.descripcion_diagnostico_movcargos,osiris_his_tipo_cirugias.id_tipo_cirugia,nombre_medico_encabezado,"+
				            	"osiris_erp_cobros_enca.facturacion, "+
				            	"osiris_erp_cobros_enca.pagado,"+
				            	
				            	"osiris_erp_cobros_enca.id_habitacion,to_char(osiris_his_habitaciones.numero_cuarto,'999999999') AS numerocuarto,osiris_his_habitaciones.descripcion_cuarto,osiris_his_habitaciones.id_tipo_admisiones AS idtipoadmisiones_habitacion,"+
				            	
				            	"numero_ntacred,to_char(osiris_erp_cobros_enca.valor_total_notacredito,'99999999.99') AS valortotalnotacredito,"+
				            	
				            	"ltrim(to_char(osiris_erp_cobros_enca.honorario_medico,'999999999.99')) AS honorariomedico "+				            	
				            	
				            	"FROM "+ 
				            	"osiris_erp_cobros_enca,osiris_his_paciente,osiris_erp_movcargos,osiris_his_tipo_admisiones,osiris_his_tipo_pacientes, "+
				            	"osiris_his_habitaciones,"+
				            	"osiris_aseguradoras, osiris_his_medicos,osiris_his_tipo_cirugias,osiris_his_tipo_especialidad,osiris_empresas "+
				            	"WHERE "+
				            	"osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
				            	"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
				            	"AND osiris_erp_cobros_enca.id_medico = osiris_his_medicos.id_medico "+ 
								"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
								"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
								"AND osiris_erp_movcargos.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
								"AND osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad  "+
								"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
								"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
								"AND osiris_erp_cobros_enca.id_habitacion = osiris_his_habitaciones.id_habitacion "+
								"AND osiris_erp_cobros_enca.folio_de_servicio = "+(string) foliodeserv+";";
								
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//Console.WriteLine(comando.CommandText.ToString());
				bool procedimiento_cerrado = false;
				if(lector.Read()){
					button_removerItem.Sensitive = true;
					button_graba_pago.Sensitive = true;
					button_aplica_cargos.Sensitive = true;
					button_abonar.Sensitive = true;
					button_bloquea_cuenta.Sensitive = true;
					button_alta_paciente.Sensitive = true;
					button_cierre_cuenta.Sensitive = true;
					button_honorario_medico.Sensitive = false;
					button_imprimir_protocolo.Sensitive = true;
					button_procedimiento_cobrz.Sensitive = true;
					button_procedimiento_totales.Sensitive = false;
					button_compro_caja.Sensitive = false;
					button_compro_serv.Sensitive = false;
					button_quitar_aplicados.Sensitive = true;
					button_actualizar.Sensitive = true;
					button_abre_folio.Sensitive = false;
					button_traspasa_productos.Sensitive = true;
								
					entry_ingreso.Text = (string) lector["fecha_ingreso"];
					entry_egreso.Text = (string) lector["fecha_egreso"];
					entry_numero_factura.Text = (string) lector["numerofactura"];
					entry_nombre_paciente.Text = (string) lector["nombre1_paciente"]+" "+(string) lector["nombre2_paciente"]+" "+(string) lector["apellido_paterno_paciente"]+" "+(string) lector["apellido_materno_paciente"];
					entry_pid_paciente.Text = (string) lector["pidpaciente"];
					entry_telefono_paciente.Text = (string) lector["telefono_particular1_paciente"];
					entry_honorario_med_caja.Text = (string) lector["honorariomedico"];
					idempresa_paciente = (int) lector["idempresa"];
					entry_total_abonos_caja.Text = (string) lector["totalabonos"];
					entry_ultimo_pago.Text =  (string) lector["totalpago"];
					
					entry_habitacion.Text = (string) lector["numerocuarto"];
					entry_habitacion.Text = entry_habitacion.Text.Trim()+"/"+(string) lector["descripcion_cuarto"];
					
					////////////////////////////////////////////////////////////////////
					// Verifica si el procedimiento esta pagado
					procedimiento_pagado = (bool) lector["pagado"];
					procedimiento_cerrado = (bool) lector ["cerrado"];
					////////////////////////////////////////////////////////////////////
					  
					////////////////////////////////////////////////////////////////////
					// nuevo para multiples listas de precios
					idaseguradora_paciente = (int) lector["id_aseguradora"];
					aplica_precios_aseguradoras = (bool) lector["listadeprecio_aseguradora"];
					aplica_precios_empresas = (bool) lector["listadeprecio_empresa"];
					////////////////////////////////////////////////////////////////////
					
					if((int) lector["id_medico"] > 1){
						entry_doctor.Text = (string) lector["nombre_medico"];
					}else{
						entry_doctor.Text = (string) lector["nombre_medico_encabezado"];
					}
					
					entry_especialidad.Text = (string) lector["descripcion_especialidad"];
					entry_tipo_paciente.Text = (string) lector["descripcion_tipo_paciente"];
					
					idempresa_paciente = (int) lector["idempresa"];
					if((int) lector ["id_aseguradora"] > 1){
						entry_aseguradora.Text = (string) lector["descripcion_aseguradora"];
					}else{
						entry_aseguradora.Text = (string) lector["descripcion_empresa"];						
					}
					
					entry_poliza.Text =  (string) lector["numero_poliza"];
					id_tipopaciente = (int) lector["idtipopaciente"];
					
					edadpac = (string) lector["edad"];
					mesespac = (string) lector["mesesedad"];
					 
            		fecha_nacimiento = (string) lector ["fecha_nac_pa"];
			       	
					cirugia = (string) lector["descripcion_cirugia"];
					
			       	this.entry_descrip_cirugia.Text = cirugia;
					this.entry_diagnostico.Text = (string) lector ["descripcion_diagnostico_movcargos"];
			       	
            		dir_pac = (string) lector["direccion_paciente"]+"  "+(string) lector["numero_casa_paciente"]+"  "+
            				(string) lector["numero_departamento_paciente"]+",  COL. "+(string) lector["colonia_paciente"]+
            					",  CP.  "+(string) lector["codigo_postal_paciente"]+",  "+(string) lector["municipio_paciente"]+",   "+(string) lector["estado_paciente"];
            					
					this.entry_descrip_cirugia.Text = cirugia;
					entry_diagnostico.Text = (string) lector ["descripcion_diagnostico_movcargos"];
					
					
					//cuenta_bloqueada = (bool) lector["bloqueo_de_folio"];
					//cuenta_cerrada = (bool) lector["pagado"];
					
					deducible_caja = float.Parse((string) lector["deduciblecaja"]); 
					coaseguro_caja = float.Parse((string) lector["coasegurocaja"]);
					honorariomedico = float.Parse((string) lector["honorariomedico"]);
					
					// Tomando valores de la nota de credito
					numeronotacredito = (int) lector["numero_ntacred"];
					valornotacredito =  float.Parse((string) lector["valortotalnotacredito"]);
					
            		empresapac = (string) lector["descripcion_empresa"];
            		
					//int foliointernodep = (int) lector["folio_de_servicio_dep"];
					
					PidPaciente = int.Parse(entry_pid_paciente.Text);		    // Toma la actualizacion del pid del paciente
					folioservicio = int.Parse(foliodeserv);
					numerofactura = int.Parse((string) lector["numfactura"]);
										
					// Validando que tenga asignada una habitacion
            		idhabitacion = (int) lector["id_habitacion"];
            		if ((int) lector["idtipoadmision"] == 300 || (int) lector["idtipoadmision"] == 400  || 
            		(int) lector["idtipoadmision"] == 930 || (int) lector["idtipoadmision"] == 200 && idhabitacion == 1 || 
            		(int) lector["idtipoadmision"] == 920){            		
	            		button_alta_paciente.Sensitive = true;	            		
	            	}else{
	            		button_alta_paciente.Sensitive = false;
	            		if (idhabitacion == 1){
	            			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
											ButtonsType.Close, "Este paciente NO tiene una HABITACION o CUBICULO asignado, favor de asignar...");
							msgBoxError.Run ();		msgBoxError.Destroy();							
						}
	            	}
            							
					if((int) lector["idtipoadmision"] > 16){
						if((bool) lector ["cancelado"] == false){ 
							if((bool) lector ["facturacion"] == false ){
								if ((bool) lector ["pagado"] == false){
									button_busca_producto.Sensitive = true;
																		
									if (procedimiento_cerrado == false){
									
										button_busca_producto.Sensitive = true;
										if ((bool) lector ["alta_paciente"] == false){
											button_cierre_cuenta.Sensitive = false;
											// habilitando boton para poder realizar mas cargos
											if ((bool) lector ["bloqueo_de_folio"] == false){
												button_busca_producto.Sensitive = true;
												if(LoginEmpleado =="DOLIVARES" || LoginEmpleado =="ADMIN"){
													button_busca_producto.Sensitive = true;
												}else{
													button_cierre_cuenta.Sensitive = false;
													button_bloquea_cuenta.Sensitive = false;
													button_busca_producto.Sensitive = true;
												}
											}else{
												if(LoginEmpleado =="DOLIVARES" || LoginEmpleado =="ADMIN"){
													
													button_bloquea_cuenta.Sensitive = false;
												}else{
													button_cierre_cuenta.Sensitive = false;
													button_bloquea_cuenta.Sensitive = false;
													button_busca_producto.Sensitive = false;
												}
												MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error,ButtonsType.Close,"Este procedimiento se encuentra BLOQUEADO \n"+
																					"solo el personal de CAJA podra agregar Cargos");
												msgBoxError.Run ();											msgBoxError.Destroy();
											}
										}else{
											button_busca_producto.Sensitive = true;
											button_alta_paciente.Sensitive = false;
											button_cierre_cuenta.Sensitive = true;
											button_procedimiento_totales.Sensitive = false;
											button_honorario_medico.Sensitive = true;
											button_compro_caja.Sensitive = true;
											button_compro_serv.Sensitive = true;
											button_traspasa_productos.Sensitive = true;
											MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error,ButtonsType.Close,"Ya se dio el ALTA a este paciente, CIERRE la \n"+
																					"cuenta de este paciente para no hacer mas cargos");
											msgBoxError.Run ();										msgBoxError.Destroy();
										}
									}else{
										button_abre_folio.Sensitive = true;
										button_busca_producto.Sensitive = false;
										button_cierre_cuenta.Sensitive = false;
										button_bloquea_cuenta.Sensitive = false;
										button_honorario_medico.Sensitive = true;
										button_alta_paciente.Sensitive = false;
										button_procedimiento_totales.Sensitive = true;
										button_traspasa_productos.Sensitive = true;
										MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"Este procedimiento se encuentra CERRADO \n"+
																		"NO podra agregar mas cargos a esta cuenta...");
										msgBoxError.Run ();							msgBoxError.Destroy();
									}
								}else{
									button_bloquea_cuenta.Sensitive = false;
									button_procedimiento_totales.Sensitive = true;
									button_cierre_cuenta.Sensitive = false;
									button_honorario_medico.Sensitive = true;
									button_abre_folio.Sensitive = true;
									button_alta_paciente.Sensitive = false;
									agregarmasabonos = false;
									button_traspasa_productos.Sensitive = false;
									//button_quitar_aplicados.Sensitive = false;
									//button_abonar.Sensitive = false;
									MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,									
									MessageType.Error,ButtonsType.Close,"Este procedimiento se encuentra PAGADO \n"+
																			"NO podra agregar mas CARGOS NI ABONO a esta cuenta");
									msgBoxError.Run ();							msgBoxError.Destroy();
								}
							}else{
								button_removerItem.Sensitive = false;
								button_quitar_aplicados.Sensitive = false;
								button_bloquea_cuenta.Sensitive = false;
								button_cierre_cuenta.Sensitive = false;
								button_busca_producto.Sensitive = false;
								button_alta_paciente.Sensitive = false;
								button_procedimiento_totales.Sensitive = true;
								agregarmasabonos = false;
								agregarmashonorario = false;
								button_abre_folio.Sensitive = false;
								button_honorario_medico.Sensitive = true;
								button_traspasa_productos.Sensitive = false;
								//button_abonar.Sensitive = false;
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, ButtonsType.Close, "Este procedimiento se encuentra FACTURADO \n"+
																		"NO podra agregar mas cargos ni abonos a esta cuenta, \n"+
																		"ni tampoco DEVOLVER productos ");
								msgBoxError.Run ();						msgBoxError.Destroy();
							}
						}else{
							button_quitar_aplicados.Sensitive = false;
							button_removerItem.Sensitive = false;
							button_bloquea_cuenta.Sensitive = false;
							button_cierre_cuenta.Sensitive = false;
							button_busca_producto.Sensitive = false;
							button_procedimiento_totales.Sensitive = true;
							button_abonar.Sensitive = false;
							button_alta_paciente.Sensitive = false;
							button_traspasa_productos.Sensitive = false;
							button_abonar.Sensitive = false;
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error,ButtonsType.Close,"Este procedimiento se encuentra CANCELADO \n"+
																"NO podra agregar mas cargos a esta cuenta");
							msgBoxError.Run ();							msgBoxError.Destroy();
						}
					}else{
						button_removerItem.Sensitive = false;
						button_quitar_aplicados.Sensitive = false;
						button_bloquea_cuenta.Sensitive = false;
						button_cierre_cuenta.Sensitive = false;
						button_busca_producto.Sensitive = false;
						button_alta_paciente.Sensitive = false;
						button_procedimiento_totales.Sensitive = true;
						button_abonar.Sensitive = false;
						button_abre_folio.Sensitive = false;
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error,ButtonsType.Close,"Este procedimiento pertenece al CENTRO MEDICO  \n"+
															"NO podra agregar cargos a esta cuenta");
						msgBoxError.Run ();				msgBoxError.Destroy();
					}
					llenado_de_material_aplicado(foliodeserv);						
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error,ButtonsType.Close,"El Numero de Atencion Seleccionado NO EXISTE... ");
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
	   			
	       	}
       		conexion.Close ();
		}
		
		// llenando el detalle de procedimiento de cobranza
		void llenado_de_material_aplicado(string foliodeserv)
		{	
			// Limpiando Varibles a valor 0
			subtotal_al_15 = 0;
			subtotal_al_0 = 0;
			total_iva = 0;
			sub_total = 0;
			totaldescuento = 0;
			aplicar_descuento = true;
			aplicar_siempre = false;
			
			// Limpio el treeview de los productos que se han aplicado y estan
			// grabado en la tabla detalle
			treeViewEngineServicio.Clear();
			// Limpio el treeview de los productos 
			treeViewEngineExtras.Clear();
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT descripcion_producto,to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto, "+
							"to_char(osiris_erp_cobros_deta.cantidad_aplicada,'99999.99') AS cantidadaplicada,"+
							"to_char(osiris_erp_cobros_deta.precio_producto,'99999999.99') AS preciounitario,"+
							//"to_char(osiris_erp_cobros_deta.precio_por_cantidad,'99999999.99') AS precioporcant,"+
							"to_char(osiris_erp_cobros_deta.cantidad_aplicada * osiris_erp_cobros_deta.precio_producto,'99999999.99') AS ppcantidad_,"+							
							//"to_char(osiris_erp_cobros_deta.iva_producto,'99999999.99') AS ivaproducto,"+
							"to_char(osiris_erp_cobros_deta.porcentage_iva,'99999999.99') AS porcentageiva,"+
							"to_char(osiris_erp_cobros_deta.porcentage_descuento,'999.99') AS pdescuento,"+
							"id_empleado,"+
							"to_char(osiris_erp_cobros_deta.fechahora_creacion,'dd-MM-yyyy HH24:mi:ss') AS fechcreacion, "+
							"osiris_his_tipo_admisiones.descripcion_admisiones, "+
							"osiris_erp_cobros_deta.id_tipo_admisiones AS idtipoadmision,"+
							"eliminado,osiris_productos.aplicar_iva,"+
							"to_char(osiris_erp_cobros_deta.id_secuencia,'9999999999') AS secuencia "+
							"FROM osiris_erp_cobros_deta,osiris_productos,osiris_his_tipo_admisiones "+
							"WHERE osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+
							"AND osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
							"AND osiris_erp_cobros_deta.eliminado = false "+ 
							"AND folio_de_servicio = '"+(string) foliodeserv+"' "+
							//"AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'MM') <= '04' "+
							"ORDER BY to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd HH24:mm:ss');";
							//"ORDER BY  to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') ASC, osiris_erp_cobros_deta.id_tipo_admisiones ASC, osiris_productos.id_grupo_producto,osiris_erp_cobros_deta.id_secuencia; ";
				//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
				float toma_cantaplicada = 0;
				float toma_subtotal = 0;
				float toma_a_pagar = 0;
				float preciocondesc = 0;
				float valordescuento = 0;
				float calculo_del_iva_producto = 0;
				string fecha_paso = "";
				string toma_descrip_prod;
				float totaldeabonos = float.Parse((string) entry_total_abonos_caja.Text);
				float totaldepago = float.Parse((string) this.entry_ultimo_pago.Text);
				while (lector.Read()){
					progressbar_status_llenado.Pulse();
					
					if (!(bool) lector["eliminado"]){
					
						toma_cantaplicada = float.Parse((string) lector["cantidadaplicada"]);
						if ((bool) lector["aplicar_iva"]){	
							calculo_del_iva_producto = (float.Parse((string) lector["ppcantidad_"])*float.Parse((string) lector["porcentageiva"]))/100;
						}else{
							calculo_del_iva_producto = 0;
						}
						valordescuento = 0;
						preciocondesc = 0;
						if (float.Parse((string) lector["pdescuento"]) > 0){
							// Calculando el Descuento
							valordescuento = ((float.Parse((string) lector["ppcantidad_"])*float.Parse((string) lector["pdescuento"]))/100);					
							preciocondesc = float.Parse((string) lector["ppcantidad_"])-valordescuento;
						}
						if ((bool) lector["aplicar_iva"]){
 							subtotal_al_15 = subtotal_al_15 + float.Parse((string) lector["ppcantidad_"]);
 						}else{
 					 		subtotal_al_0 = subtotal_al_0 + float.Parse((string) lector["ppcantidad_"]);
 						}
 						
 						toma_subtotal = float.Parse((string) lector["ppcantidad_"]) + calculo_del_iva_producto;
 						
 						// Verificando si aplica descuento por targeta de Descuento
 						
 						if ((int) lector["idtipoadmision"] == 100 && (int) id_tipopaciente == 101 && aplicar_siempre == false
					  		||(int) lector["idtipoadmision"] == 200 && (int) id_tipopaciente == 101 && aplicar_siempre == false
					  		||(int) lector["idtipoadmision"] == 300 && (int) id_tipopaciente == 101 && aplicar_siempre == false
					  		||(int) lector["idtipoadmision"] == 400 && (int) id_tipopaciente == 101 && aplicar_siempre == false
					  		||(int) lector["idtipoadmision"] == 920 && (int) id_tipopaciente == 101 && aplicar_siempre == false){
							aplicar_descuento = true;
						}else{
							if (aplicar_siempre == false){
								aplicar_siempre = true;
								aplicar_descuento = false;							
							}
						}
						//Console.WriteLine((string) Convert.ToString((int) lector["idtipoadmision"]));
					
						totaldescuento = totaldescuento + valordescuento + ((valordescuento*valoriva)/100);
 					 					
						total_iva = total_iva + calculo_del_iva_producto;
						
						toma_descrip_prod = (string) lector["descripcion_producto"];
						
						if(toma_descrip_prod.Length > 68){
							toma_descrip_prod = toma_descrip_prod.Substring(0,67);
						}  
						
						fecha_rango_2 = (string) lector["fechcreacion"];
						if (fecha_paso ==""){
							fecha_rango_1 = (string) lector["fechcreacion"];
							fecha_paso = (string) lector["fechcreacion"];
						} 
						
						treeViewEngineServicio.AppendValues (toma_descrip_prod,
														toma_cantaplicada,
														(string) lector["idproducto"],
														(string) lector["preciounitario"],
														(string) lector["ppcantidad_"],
														calculo_del_iva_producto.ToString("F") ,
														toma_subtotal.ToString("F"),
														(string) lector["pdescuento"],
														valordescuento.ToString("F"),
														preciocondesc.ToString("F"),
														(string) lector["id_empleado"],
														(string) lector["fechcreacion"],
														(string) lector["descripcion_admisiones"],
														(int) lector["idtipoadmision"],
														(bool) true,
														(string) "",
														(string) "",
														(string) "",
														(string) lector["secuencia"]);
															
					}
				}
				if (aplicar_descuento == false){
					totaldescuento = 0;
				}
								
				entry_subtotal_al_15.Text = subtotal_al_15.ToString("F");
 				entry_subtotal_al_0.Text = subtotal_al_0.ToString("F");
 				entry_total_iva.Text = total_iva.ToString("F");
 				entry_subtotal.Text = sub_total.ToString("F");
 				
 				if ((int) numeronotacredito > 0){
					entry_totaldescuento.Text = valornotacredito.ToString("F");
					totaldescuento = valornotacredito;
				}else{ 				
 					entry_totaldescuento.Text = totaldescuento.ToString("F");
 				}
 				
 				// Realizando las restas
				sub_total = subtotal_al_15 + subtotal_al_0 + total_iva;
				toma_a_pagar = ((((sub_total - totaldescuento) - deducible_caja) + honorariomedico) - totaldeabonos)-totaldepago;
 				
 				entry_total.Text = sub_total.ToString("F");
 				entry_a_pagar.Text = toma_a_pagar.ToString("F");
 				
 				// Valores de Aseguradoras
 				entry_deducible_caja.Text = deducible_caja.ToString("F");
				entry_coaseguro_caja.Text = coaseguro_caja.ToString("F");
 								
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		// busco un paciente pantalla de ingreso de nuevo paciente
		void on_button_buscar_paciente_clicked(object sender, EventArgs args)
	    {
			busqueda = "paciente";
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "busca_paciente", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda("paciente");
			entry_expresion.KeyPressEvent += onKeyPressEvent_busqueda;
			button_buscar_busqueda.Clicked += new EventHandler(on_buscar_paciente_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_paciente_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
		}
	    
		void on_button_busca_producto_clicked (object sender, EventArgs args)
		{
			busqueda = "productos";
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda("producto");
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_busqueda;
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			// Boton que agrega cargos extras
			//button_agrega_extra.Clicked += new EventHandler(on_button_agrega_extra_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
			
			// Validando que sen solo numeros
			entry_cantidad_aplicada.KeyPressEvent += onKeyPressEvent;
			
			// Llenado de combobox
			combobox_tipo_admision.Clear();
			CellRendererText cell2 = new CellRendererText();
			combobox_tipo_admision.PackStart(cell2, true);
			combobox_tipo_admision.AddAttribute(cell2,"text",0);
	        
			ListStore store2 = new ListStore( typeof (string), typeof (int));
			combobox_tipo_admision.Model = store2;
							        	        
			// lleno de la tabla de his_tipo_de_admisiones
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_his_tipo_admisiones "+
               						"WHERE cuenta_mayor = 4000 "+
               						"AND id_tipo_admisiones IN('200','300','400','920','930') "+
               						"ORDER BY descripcion_admisiones;";
				
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
			if (store2.GetIterFirst(out iter2))
			{
				//Console.WriteLine(iter2);
				combobox_tipo_admision.SetActiveIter (iter2);
			}
			combobox_tipo_admision.Changed += new EventHandler (onComboBoxChanged_tipo_admision);
		}

		void crea_treeview_busqueda(string tipo_busqueda)
		{
			if (tipo_busqueda == "paciente"){
				treeViewEngineBusca = new TreeStore(typeof(int),
													typeof(int),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string) );
				lista_de_Pacientes.Model = treeViewEngineBusca;
			
				lista_de_Pacientes.RulesHint = true;
			
				lista_de_Pacientes.RowActivated += on_selecciona_paciente_clicked;  // Doble click selecciono paciente*/

				TreeViewColumn col_foliodeatencion = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_foliodeatencion.Title = "Folio de Antencion"; // titulo de la cabecera de la columna, si está visible
				col_foliodeatencion.PackStart(cellr0, true);
				col_foliodeatencion.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
				col_foliodeatencion.SortColumnId = (int) Column.col_foliodeatencion;
			
				TreeViewColumn col_PidPaciente = new TreeViewColumn();
				CellRendererText cellr1 = new CellRendererText();
				col_PidPaciente.Title = "PID Paciente"; // titulo de la cabecera de la columna, si está visible
				col_PidPaciente.PackStart(cellr1, true);
				col_PidPaciente.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
				col_PidPaciente.SortColumnId = (int) Column.col_PidPaciente;
				//cellr0.Editable = true;   // Permite edita este campo
            
				TreeViewColumn col_Nombre1_Paciente = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_Nombre1_Paciente.Title = "Nombre 1";
				col_Nombre1_Paciente.PackStart(cellrt2, true);
				col_Nombre1_Paciente.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
				col_Nombre1_Paciente.SortColumnId = (int) Column.col_Nombre1_Paciente;
            
				TreeViewColumn col_Nombre2_Paciente = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_Nombre2_Paciente.Title = "Nombre 2";
				col_Nombre2_Paciente.PackStart(cellrt3, true);
				col_Nombre2_Paciente.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 2 en vez de 3
				col_Nombre2_Paciente.SortColumnId = (int) Column.col_Nombre2_Paciente;
            
				TreeViewColumn col_app_Paciente = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_app_Paciente.Title = "Apellido Paterno";
				col_app_Paciente.PackStart(cellrt4, true);
				col_app_Paciente.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 3 en vez de 4
				col_app_Paciente.SortColumnId = (int) Column.col_app_Paciente;
            
				TreeViewColumn col_apm_Paciente = new TreeViewColumn();
				CellRendererText cellrt5 = new CellRendererText();
				col_apm_Paciente.Title = "Apellido Materno";
				col_apm_Paciente.PackStart(cellrt5, true);
				col_apm_Paciente.AddAttribute (cellrt5, "text", 5); // la siguiente columna será 5 en vez de 6
				col_apm_Paciente.SortColumnId = (int) Column.col_apm_Paciente;
      
				TreeViewColumn col_fechanacimiento_Paciente = new TreeViewColumn();
				CellRendererText cellrt6 = new CellRendererText();
				col_fechanacimiento_Paciente.Title = "Fecha Nacimiento";
				col_fechanacimiento_Paciente.PackStart(cellrt6, true);
				col_fechanacimiento_Paciente.AddAttribute (cellrt6, "text", 6);     // la siguiente columna será 6 en vez de 7
				col_fechanacimiento_Paciente.SortColumnId = (int) Column.col_fechanacimiento_Paciente;
            
				TreeViewColumn col_edad_Paciente = new TreeViewColumn();
				CellRendererText cellrt7 = new CellRendererText();
				col_edad_Paciente.Title = "Edad";
				col_edad_Paciente.PackStart(cellrt7, true);
				col_edad_Paciente.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 7 en vez de 8
				col_edad_Paciente.SortColumnId = (int) Column.col_edad_Paciente;
            
				TreeViewColumn col_sexo_Paciente = new TreeViewColumn();
				CellRendererText cellrt8 = new CellRendererText();
				col_sexo_Paciente.Title = "Sexo";
				col_sexo_Paciente.PackStart(cellrt8, true);
				col_sexo_Paciente.AddAttribute (cellrt8, "text", 8); // la siguiente columna será 8 en vez de 9
				col_sexo_Paciente.SortColumnId = (int) Column.col_sexo_Paciente;
                        
				TreeViewColumn col_creacion_Paciente = new TreeViewColumn();
				CellRendererText cellrt9 = new CellRendererText();
				col_creacion_Paciente.Title = "Fecha creacion";
				col_creacion_Paciente.PackStart(cellrt9, true);
				col_creacion_Paciente.AddAttribute (cellrt9, "text", 9); // la siguiente columna será 8 en vez de 9
				col_creacion_Paciente.SortColumnId = (int) Column.col_creacion_Paciente;

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
			if (tipo_busqueda == "producto"){
				treeViewEngineBusca2 = new TreeStore(typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));
				lista_de_producto.Model = treeViewEngineBusca2;
			
				lista_de_producto.RulesHint = true;
			
				lista_de_producto.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono paciente*/
				
				TreeViewColumn col_idproducto = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_idproducto.Title = "ID Producto"; // titulo de la cabecera de la columna, si está visible
				col_idproducto.PackStart(cellr0, true);
				col_idproducto.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
				col_idproducto.SortColumnId = (int) Column_prod.col_idproducto;
			
				TreeViewColumn col_desc_producto = new TreeViewColumn();
				CellRendererText cellr1 = new CellRendererText();
				col_desc_producto.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
				col_desc_producto.PackStart(cellr1, true);
				col_desc_producto.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
				col_desc_producto.SortColumnId = (int) Column_prod.col_desc_producto;
				//cellr0.Editable = true;   // Permite edita este campo
            
				TreeViewColumn col_precioprod = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_precioprod.Title = "Precio Producto";
				col_precioprod.PackStart(cellrt2, true);
				col_precioprod.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
				col_precioprod.SortColumnId = (int) Column_prod.col_precioprod;
            
				TreeViewColumn col_ivaprod = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_ivaprod.Title = "I.V.A.";
				col_ivaprod.PackStart(cellrt3, true);
				col_ivaprod.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 2 en vez de 3
				col_ivaprod.SortColumnId = (int) Column_prod.col_ivaprod;
            
				TreeViewColumn col_totalprod = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_totalprod.Title = "Total";
				col_totalprod.PackStart(cellrt4, true);
				col_totalprod.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 3 en vez de 4
				col_totalprod.SortColumnId = (int) Column_prod.col_totalprod;
            
				TreeViewColumn col_descuentoprod = new TreeViewColumn();
				CellRendererText cellrt5 = new CellRendererText();
				col_descuentoprod.Title = "% Descuento";
				col_descuentoprod.PackStart(cellrt5, true);
				col_descuentoprod.AddAttribute (cellrt5, "text", 5); // la siguiente columna será 5 en vez de 6
				col_descuentoprod.SortColumnId = (int) Column_prod.col_descuentoprod;
      
				TreeViewColumn col_preciocondesc = new TreeViewColumn();
				CellRendererText cellrt6 = new CellRendererText();
				col_preciocondesc.Title = "$Descuento sin IVA";
				col_preciocondesc.PackStart(cellrt6, true);
				col_preciocondesc.AddAttribute (cellrt6, "text", 6);     // la siguiente columna será 6 en vez de 7
				col_preciocondesc.SortColumnId = (int) Column_prod.col_preciocondesc;
            
				TreeViewColumn col_grupoprod = new TreeViewColumn();
				CellRendererText cellrt7 = new CellRendererText();
				col_grupoprod.Title = "Grupo Producto";
				col_grupoprod.PackStart(cellrt7, true);
				col_grupoprod.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 7 en vez de 8
				col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
            
				TreeViewColumn col_grupo1prod = new TreeViewColumn();
				CellRendererText cellrt8 = new CellRendererText();
				col_grupo1prod.Title = "Grupo1 Producto";
				col_grupo1prod.PackStart(cellrt8, true);
				col_grupo1prod.AddAttribute (cellrt8, "text", 8); // la siguiente columna será 8 en vez de 9
				col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
                        
				TreeViewColumn col_grupo2prod = new TreeViewColumn();
				CellRendererText cellrt9 = new CellRendererText();
				col_grupo2prod.Title = "Grupo2 Producto";
				col_grupo2prod.PackStart(cellrt9, true);
				col_grupo2prod.AddAttribute (cellrt9, "text", 9); // la siguiente columna será 8 en vez de 9
				col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;

				lista_de_producto.AppendColumn(col_idproducto);  // 0
				lista_de_producto.AppendColumn(col_desc_producto); // 1
				lista_de_producto.AppendColumn(col_precioprod);	//2
				lista_de_producto.AppendColumn(col_ivaprod);	// 3
				lista_de_producto.AppendColumn(col_totalprod); // 4
				lista_de_producto.AppendColumn(col_descuentoprod); //5
				lista_de_producto.AppendColumn(col_preciocondesc); // 6
				lista_de_producto.AppendColumn(col_grupoprod);	//7
				lista_de_producto.AppendColumn(col_grupo1prod);	//8
				lista_de_producto.AppendColumn(col_grupo2prod);	//9							
			}
			if (tipo_busqueda == "medicos"){
				treeViewEngineBusca4 = new TreeStore( typeof(int), typeof(string), typeof(string), typeof(string), typeof(string) );
				lista_medicos.Model = treeViewEngineBusca4;
				lista_medicos.RulesHint = true;
				
				//lista_medicos.RowActivated += on_selecciona_medico_clicked;  // Doble click selecciono paciente*/
				
				TreeViewColumn col_idmedico = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_idmedico.Title = "ID Medico"; // titulo de la cabecera de la columna, si está visible
				col_idmedico.PackStart(cellr0, true);
				col_idmedico.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1
	            
				TreeViewColumn col_nombrmedico = new TreeViewColumn();
				CellRendererText cellrt1 = new CellRendererText();
				col_nombrmedico.Title = "Nombre Medico";
				col_nombrmedico.PackStart(cellrt1, true);
				col_nombrmedico.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
	            
				TreeViewColumn col_espemedico = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_espemedico.Title = "Especialidad";
				col_espemedico.PackStart(cellrt2, true);
				col_espemedico.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 3
	            
				TreeViewColumn col_telmedico = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_telmedico.Title = "Telefono";
				col_telmedico.PackStart(cellrt3, true);
				col_telmedico.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 4
	            
				TreeViewColumn col_cedulamedico = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_cedulamedico.Title = "Cedula Medica";
				col_cedulamedico.PackStart(cellrt4, true);
				col_cedulamedico.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 3
	                        
				lista_medicos.AppendColumn(col_idmedico);
				lista_medicos.AppendColumn(col_nombrmedico);
				lista_medicos.AppendColumn(col_espemedico);
				lista_medicos.AppendColumn(col_telmedico);
				lista_medicos.AppendColumn(col_cedulamedico);				
			}
		}
			
		enum Column
		{
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
		
		enum Column_prod
		{
			col_idproducto,
			col_desc_producto,
			col_precioprod,
			col_ivaprod,
			col_totalprod,
			col_descuentoprod,
			col_preciocondesc,
			col_grupoprod,
			col_grupo1prod,
			col_grupo2prod
		}
		
		enum Column_cliente
		{
			col_idcliente,
			col_rfc_cliente,
			col_desc_cliente,
			col_direc_cliente,
			col_colonia_cliente,
			col_municipio_cliente,
			col_estado_clientes,
			col_tel_cliente
		}
		
		enum Colum_cargos_extras
		{
			col_seleccion,
			col_cantidad,
			col_codigo_prod,
			col_descripcion,
			col_precio,
			col_ppor_cantidad,
			col_iva,
			col_sub_total,
			col_por_desc,
			col_valor_desc,
			col_total,
			col_quien_cargo,
			col_fecha_hora,
			col_asignado,
			col_costounitario,
			col_porceutilidad,
			col_costoproducto
		}
		
		// activa busqueda con boton busqueda de paciente
		// y llena la lista con los pacientes		
		void on_buscar_paciente_clicked (object sender, EventArgs args)
		{
			llenando_lista_de_pacientes();
		}
		
		void llenando_lista_de_pacientes()
		{
			treeViewEngineBusca.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				if(entry_expresion.Text.Trim() == "") {
					comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,"+
											"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
											"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, sexo_paciente,"+
											"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
											"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
											"WHERE osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+ 
											//"AND osiris_erp_cobros_enca.pagado = 'false' "+
											"AND osiris_erp_cobros_enca.facturacion = 'false' "+
											//"AND osiris_erp_cobros_enca.cerrado = 'false' "+
											" ORDER BY pid_paciente;";
											//"AND apellido_paterno_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' ORDER BY pid_paciente;";
				}else{
					if (radiobutton_busca_apellido.Active == true){
						comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,"+
											"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
											"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, sexo_paciente,"+
											"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
											"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
											"WHERE osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+ 
											//"AND osiris_erp_cobros_enca.pagado = 'false' "+
											"AND osiris_erp_cobros_enca.facturacion = 'false' "+
											//"AND osiris_erp_cobros_enca.cerrado = 'false' "+
											"AND apellido_paterno_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' ORDER BY pid_paciente;";
					}
					if (radiobutton_busca_nombre.Active == true){
						comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,"+
											"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
											"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, sexo_paciente,"+
											"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
											"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
											"WHERE osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+ 
											//"AND osiris_erp_cobros_enca.pagado = 'false' "+ 
											"AND osiris_erp_cobros_enca.facturacion = 'false' "+
											//"AND osiris_erp_cobros_enca.cerrado = 'false' "+
											"AND nombre1_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' ORDER BY osiris_erp_cobros_enca.pid_paciente;";
					}
					if (radiobutton_busca_expediente.Active == true){
						comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,"+
											"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
											"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, sexo_paciente,"+
											"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
											"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
											"WHERE osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+ 
											//"AND osiris_erp_cobros_enca.pagado = 'false' "+
											"AND osiris_erp_cobros_enca.facturacion = 'false' "+
											//"AND osiris_erp_cobros_enca.cerrado = 'false' "+
											"AND osiris_erp_cobros_enca.pid_paciente = '"+entry_expresion.Text+"' ORDER BY osiris_erp_cobros_enca.pid_paciente;";					
					}
				}
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//Console.WriteLine(comando.CommandText.ToString());
				while (lector.Read()){
					treeViewEngineBusca.AppendValues ((int) lector["folio_de_servicio"],
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
 				 folioservicio = (int) model.GetValue(iterSelected, 0);
 				 entry_folio_servicio.Text = folioservicio.ToString();
 				 llenado_de_productos_aplicados(folioservicio.ToString());
 			}
 			// cierra la ventana despues que almaceno la informacion en variables
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
 		}
 		
 		// llena la lista de productos
 		void on_llena_lista_producto_clicked (object sender, EventArgs args)
 		{
 			llenando_lista_de_productos();
 		}
 			
 		void llenando_lista_de_productos()
 		{
 			treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			string precio_a_tomar = "";    // en esta variable dejo el precio que va tomar para los direfentes clientes
			
			//// para las diferentes listas de precios \\\\\\\\\\\\\			
			if (id_tipopaciente == 500 || id_tipopaciente == 102) {  // Municipio y Empresas			
				// verifica si ese cliente tiene una lista de precio asignada
				if (this.aplica_precios_empresas == true || aplica_precios_aseguradoras == true){     
					precio_a_tomar = "precio_producto_"+id_tipopaciente.ToString().Trim()+idempresa_paciente.ToString().Trim();
					//precio_a_tomar = "precio_producto_publico1";
				}else{
					precio_a_tomar = "precio_producto_publico";
				}
			}else{				
				if (id_tipopaciente == 400 ) { // Aseguradora
					// verifica si ese cliente tiene una lista de precio asignada
					if (this.aplica_precios_empresas == true || aplica_precios_aseguradoras == true){    
						precio_a_tomar = "precio_producto_"+id_tipopaciente.ToString().Trim()+this.idaseguradora_paciente.ToString().Trim();
						//precio_a_tomar = "precio_producto_publico1";
					}else{
						precio_a_tomar = "precio_producto_publico";
					}
				}else{
					precio_a_tomar = "precio_producto_publico";
				}
			}
			//////////////////////////////////
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
							"osiris_productos.descripcion_producto,to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
							//"to_char(precio_producto_publico1,'99999999.99') AS preciopublico1,"+
							"to_char("+precio_a_tomar+",'99999999.99') AS preciopublico_cliente,"+
							"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,"+
							"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(porcentage_ganancia,'99999.99') AS porcentageutilidad,to_char(costo_producto,'999999999.99') AS costoproducto, "+
							"osiris_grupo_producto.agrupacion,osiris_productos.cantidad_de_embalaje "+
							"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
							"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
							"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
							"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
							"AND cobro_activo = 'true' "+
							"AND osiris_productos.id_grupo_producto IN('6','7','10','11','12','13','14','15','16','17','19','91') "+
							"AND osiris_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_producto; ";
				//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				float tomaprecio;
				float calculodeiva;
				float preciomasiva;
				float tomadescue;
				float preciocondesc;
															
				while (lector.Read()){
					calculodeiva = 0;
					preciomasiva = 0;
					
					///////////////////////////////////////////////////////////
					// ---- nuevo para las multiples listas de precio					
					if (float.Parse((string) lector["preciopublico_cliente"]) > 0){
						tomaprecio = float.Parse((string) lector["preciopublico_cliente"]);
					}else{
						tomaprecio = float.Parse((string) lector["preciopublico"]);
					}					
					////////////////********************************************					
					tomadescue = float.Parse((string) lector["porcentagesdesc"]);
					preciocondesc = tomaprecio;					
					if ((bool) lector["aplicar_iva"]){
						calculodeiva = (tomaprecio * valoriva)/100;
					}
					if ((bool) lector["aplica_descuento"]){
						preciocondesc = tomaprecio-((tomaprecio*tomadescue)/100);
					}
					preciomasiva = tomaprecio + calculodeiva; 
					treeViewEngineBusca2.AppendValues (
											(string) lector["codProducto"] ,
											(string) lector["descripcion_producto"],
											tomaprecio.ToString("F").PadLeft(10),
											calculodeiva.ToString("F").PadLeft(10),
											preciomasiva.ToString("F").PadLeft(10),
											(string) lector["porcentagesdesc"],
											preciocondesc.ToString("F").PadLeft(10),
											(string) lector["descripcion_grupo_producto"],
											(string) lector["descripcion_grupo1_producto"],
											(string) lector["descripcion_grupo2_producto"],
											(string) lector["costoproductounitario"],
											(string) lector["porcentageutilidad"],
											(string) lector["costoproducto"],
											(string) lector["agrupacion"]);
					
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

 		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			//Verificar que el paciente no este en alta
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			//conexion = new NpgsqlConnection (connectionString+"Database=hscmty");
			
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				comando.CommandText = "SELECT alta_paciente,folio_de_servicio FROM osiris_erp_cobros_enca "+ 
									"WHERE folio_de_servicio = '"+this.folioservicio.ToString().Trim()+"' "+
									"AND cerrado = 'false';";				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if (lector.Read()){
					TreeModel model;
					TreeIter iterSelected;
		 			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
		 				id_produ = (string) model.GetValue(iterSelected, 0);
						desc_produ = (string) model.GetValue(iterSelected, 1);
						precio_produ = (string) model.GetValue(iterSelected, 2);
						iva_produ = (string) model.GetValue(iterSelected, 3);
						total_produ = (string) model.GetValue(iterSelected, 4);
						descuent_produ = (string) model.GetValue(iterSelected, 5);
						pre_con_desc_produ = (string) model.GetValue(iterSelected, 6);
						//valor_descuento = float.Parse(this.precio_produ)-float.Parse(this.pre_con_desc_produ);
						costo_unitario_producto = (string) model.GetValue(iterSelected, 10); 
						porcentage_utilidad_producto = (string) model.GetValue(iterSelected, 11);
						costo_total_producto = (string) model.GetValue(iterSelected, 12);
						
						entry_desc_producto.Text = desc_produ;
						string constante = entry_cantidad_aplicada.Text;
						//varibles numericas
						ppcantidad = float.Parse(precio_produ)*float.Parse(constante);
						 
						float ivaproducto = float.Parse(iva_produ)*float.Parse(constante);
						float suma_total = ppcantidad+ivaproducto;
						float preciocondesc = float.Parse(pre_con_desc_produ)*float.Parse(constante);
						valor_descuento = ppcantidad-preciocondesc;
						float costotprodu = preciocondesc;
						
						if ((string) constante != ""){
							if ((float) float.Parse(constante) > 0){
								if ((int) idtipointernamiento >= 20){
									bool error_de_captura = false;
									if((string) model.GetValue(iterSelected, 13) != "IMG" && idtipointernamiento == 300){
										error_de_captura = true;
									}
									if((string) model.GetValue(iterSelected, 13) != "LAB" && idtipointernamiento == 400){
										error_de_captura = true;
									}
									if((string) model.GetValue(iterSelected, 13) == "LAB" && idtipointernamiento != 400){
										error_de_captura = true;
									}
									if((string) model.GetValue(iterSelected, 13) == "IMG" && idtipointernamiento != 300){
										error_de_captura = true;
									}
									if((string) model.GetValue(iterSelected, 13) == "OTR"){
										error_de_captura = false;
									}
									//if(int.Parse( (string) this.edadpac) == 0 && ){
									
									//}
									/*if((string) model.GetValue(iterSelected, 13) == "IMG"){
										idtipointernamiento = 300;
										descripinternamiento = "Imagenologia-RX";
									}
									if((string) model.GetValue(iterSelected, 13) == "LAB"){
										idtipointernamiento = 400;
										descripinternamiento = "Laboratorio";
									}*/							
									if(!error_de_captura){
										if ((string) entry_desc_producto.Text.Trim() == ""){}else{
											Item foo;
											foo = new Item (true,
												float.Parse(constante),
												this.id_produ,
												this.desc_produ,
												this.precio_produ.PadLeft(10),
												ppcantidad.ToString(),
												ivaproducto.ToString("F").PadLeft(10),
												suma_total.ToString("F").PadLeft(10),
												this.descuent_produ.PadLeft(10),
												this.valor_descuento.ToString("F"),
												preciocondesc.ToString("F").PadLeft(10),
												LoginEmpleado,
												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
												descripinternamiento,
												idtipointernamiento,
												costo_unitario_producto,
												porcentage_utilidad_producto,
												costotprodu.ToString("F").PadLeft(10));
							
											arraycargosextras.Add(foo);
											treeViewEngineExtras.AppendValues (true,
																float.Parse(constante),
																this.id_produ,
																this.desc_produ,
																this.precio_produ.PadLeft(10),
																ppcantidad.ToString(),
																ivaproducto.ToString("F").PadLeft(10),
																suma_total.ToString("F").PadLeft(10),
																this.descuent_produ.PadLeft(10),
																this.valor_descuento.ToString("F").PadLeft(10),
																preciocondesc.ToString("F").PadLeft(10),
																LoginEmpleado,
																DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
																descripinternamiento,
																idtipointernamiento,
																costo_unitario_producto,
																porcentage_utilidad_producto,
																costotprodu.ToString("F").PadLeft(10));
												//Console.WriteLine(id_produ+" "+desc_produ+" "+constante+" "+precio_produ+" "+ppcantidad.ToString()+" "+ivaprodu+" "+totalprodu+" "+
											 // descuent_produ+" "+valor_descuento+" "+costo_total_producto);
										}
									}else{
										MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close, "No puede cargar este produto ya que  \n"+
															"NO corresponde a este grupo, verique donde \n"+
															"se Realizo el cargo");
										msgBoxError.Run ();
										msgBoxError.Destroy();
									}
													
								}else{
									MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error, 
									ButtonsType.Close, "Seleccione el lugar o departamento donde \n"+
											"se genero el cargo para el paciente");
									msgBoxError.Run ();
									msgBoxError.Destroy();
								}
							}else{
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, 
								ButtonsType.Close, "La cantidad que quiere aplicar debe ser \n"+
											"mayor que cero, intente de nuevo");
								msgBoxError.Run ();
								msgBoxError.Destroy();
							}
						}else{
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, 
							ButtonsType.Close, "La cantidad que quiere aplicar NO \n"+
											"puede quedar vacia, intente de nuevo");
							msgBoxError.Run ();
							msgBoxError.Destroy();
						}
		 			}
		 		}else{
		 			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close, "La cuenta se encuentra cerrada \n"+
											"NO podra realizar mas cargos...");
							msgBoxError.Run ();
							msgBoxError.Destroy();
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
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
			{
				args.RetVal = true;
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza en ventana
		// Principal cuando selecciona el folio de productos
		// Ademas controla la tecla ENTRER para ver el procedimiento
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_folio(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				llenado_de_productos_aplicados( (string) entry_folio_servicio.Text );				
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
			{
				args.RetVal = true;
			}
		}
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_busqueda(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				if(busqueda == "productos") { llenando_lista_de_productos(); }
				if(busqueda == "medicos") { llenando_lista_de_medicos(); } 		
				if(busqueda == "paciente") { llenando_lista_de_pacientes(); }
			}
		}		
		
		struct Item
 		{
 			public bool col_seleccion{
				get { return col0_car_extr; }
				set { col0_car_extr = value; }
			}
			public float col_cantidad{
				get { return col1_car_extr; }
				set { col1_car_extr = value; }
			}
			public string col_codigo_prod{
				get { return col2_car_extr; }
				set { col2_car_extr = value; }
			}
			public string col_descripcion{
				get { return col3_car_extr; }
				set { col3_car_extr = value; }
			}
			public string col_precio{
				get { return col4_car_extr; }
				set { col4_car_extr = value; }
			}
			public string col_ppor_cantidad{
				get { return col5_car_extr; }
				set { col5_car_extr = value; }
			}	
			public string col_iva{
				get { return col6_car_extr; }
				set { col6_car_extr = value; }
			}
			public string col_sub_total{
				get { return col7_car_extr; }
				set { col7_car_extr = value; }
			}
			public string col_por_desc{
				get { return col8_car_extr; }
				set { col8_car_extr = value; }
			}
			public string col_valor_desc{
				get { return col9_car_extr; }
				set { col9_car_extr = value; }
			}
			public string col_total{
				get { return col10_car_extr; }
				set { col10_car_extr = value; }
			}
			public string col_quien_cargo{
				get { return col11_car_extr; }
				set { col11_car_extr = value; }
			}
			public string col_fecha_hora{
				get { return col12_car_extr; }
				set { col12_car_extr = value; }
			}
			public string col_asignado{
				get { return col13_car_extr; }
				set { col13_car_extr = value; }
			}
			public int col_idasignado{
				get { return col14_car_extr; }
				set { col14_car_extr = value; }
			}
			
			public string col_costounitario{
				get { return col15_car_extr; }
				set { col15_car_extr = value; }
			}
			
			public string col_porceutilidad{
				get { return col16_car_extr; }
				set { col16_car_extr = value; }
			}
			
			public string col_costoproducto{
				get { return col17_car_extr; }
				set { col17_car_extr = value; }
			}
			
			private bool col0_car_extr;
			private float col1_car_extr;
			private string col2_car_extr;
			private string col3_car_extr;
			private string col4_car_extr;
			private string col5_car_extr;
			private string col6_car_extr;
			private string col7_car_extr;
			private string col8_car_extr;
			private string col9_car_extr;
			private string col10_car_extr;
			private string col11_car_extr;
			private string col12_car_extr;
			private string col13_car_extr;
			private int col14_car_extr;
			private string col15_car_extr;
			private string col16_car_extr;
			private string col17_car_extr;
			
			public Item (bool col0_car_extr,float col1_car_extr,string col2_car_extr,string col3_car_extr,
					string col4_car_extr,
					string col5_car_extr,string col6_car_extr,string col7_car_extr,string col8_car_extr,
					string col9_car_extr,string col10_car_extr,string col11_car_extr,string col12_car_extr,
					string col13_car_extr,int col14_car_extr,string col15_car_extr,string col16_car_extr,
					string col17_car_extr )
			{
				this.col0_car_extr = col0_car_extr;
				this.col1_car_extr = col1_car_extr;
				this.col2_car_extr = col2_car_extr;
				this.col3_car_extr = col3_car_extr;
				this.col4_car_extr = col4_car_extr;
				this.col5_car_extr = col5_car_extr;
				this.col6_car_extr = col6_car_extr;
				this.col7_car_extr = col7_car_extr;
				this.col8_car_extr = col8_car_extr;
				this.col9_car_extr = col9_car_extr;
				this.col10_car_extr = col10_car_extr;
				this.col11_car_extr = col11_car_extr;
				this.col12_car_extr = col12_car_extr;
				this.col13_car_extr = col13_car_extr;
				this.col14_car_extr = col14_car_extr;
				this.col15_car_extr = col15_car_extr;
				this.col16_car_extr = col16_car_extr;
				this.col17_car_extr = col17_car_extr;
				
			}
 		}
 		
		private void on_button_removerItem_clicked (object o, EventArgs args)
		{
 			TreeIter iter;
 			TreeModel model;

 			if (lista_cargos_extras.Selection.GetSelected (out model, out iter)) {
 				int position = treeViewEngineExtras.GetPath (iter).Indices[0];
 				treeViewEngineExtras.Remove (ref iter);
				arraycargosextras.RemoveAt (position);
			}
		}
		
		void NumberCellEdited (object o, EditedArgs args)
		{
			TreePath path = new TreePath (args.Path);
 			TreeIter iter;
 			treeViewEngineExtras.GetIter (out iter, path);
			int i = path.Indices[0];
			float precio_linea;
			float precioprod;
			float iva_linea;
			float total1_linea;
			float valor_descuento;
			float precio_con_desc;
			Item foo;
			try {
				foo = (Item) arraycargosextras[i];
 				foo.col_cantidad = float.Parse(args.NewText,System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo("es-MX"));
 				precio_linea = float.Parse(foo.col_precio) * float.Parse(args.NewText);
 				precioprod = float.Parse(foo.col_precio);
 				
 				valor_descuento = ( precioprod * float.Parse(foo.col_por_desc)/100) * float.Parse(args.NewText);
 				precio_con_desc = (precioprod-(precioprod * float.Parse(foo.col_por_desc)/100)) * float.Parse(args.NewText);
 				
 				if (float.Parse(foo.col_iva) > 0){
 					iva_linea = float.Parse(foo.col_iva) * float.Parse(args.NewText);
 				}else{
 					iva_linea = float.Parse(foo.col_iva);
 				}
 				total1_linea = precio_linea + iva_linea; 
 				
			} catch (Exception e) {
				Console.WriteLine(e.Message);
				return;
			}
 			treeViewEngineExtras.SetValue (iter, (int) Colum_cargos_extras.col_cantidad, foo.col_cantidad);
 			treeViewEngineExtras.SetValue (iter, (int) Colum_cargos_extras.col_ppor_cantidad,precio_linea.ToString("F"));
 			treeViewEngineExtras.SetValue (iter, (int) Colum_cargos_extras.col_iva,iva_linea.ToString("F"));
 			treeViewEngineExtras.SetValue (iter, (int) Colum_cargos_extras.col_sub_total,total1_linea.ToString("F"));
 			
 			treeViewEngineExtras.SetValue (iter, (int) Colum_cargos_extras.col_valor_desc,valor_descuento.ToString("F"));
 			treeViewEngineExtras.SetValue (iter, (int) Colum_cargos_extras.col_total,precio_con_desc.ToString("F"));
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}		
	}
}