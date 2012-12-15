////////////////////////////////////////////////////////////
// created on 08/05/2007 at 09:26 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
//				  Daniel Olivares - arcangeldoc@gmail.com (Diseño de Pantallas Glade)
// 				  
// Licencia		: GLP
// S.O. 		: GNU/Linux
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
// Programa		: 
// Proposito	: Solicitud de materiales para los diferentes centros de costos medicos 
// Objeto		: 
//////////////////////////////////////////////////////////
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class solicitud_material
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		
		// Declarando ventana principal de solicitud de Materiales
		[Widget] Gtk.Window solicitud_materiales;
		[Widget] Gtk.Entry entry_numero_solicitud;
		[Widget] Gtk.Entry entry_quien_solicita;
		[Widget] Gtk.Entry entry_fecha_solicitud;
		[Widget] Gtk.Entry entry_status_solicitud;
		[Widget] Gtk.Button button_selecciona_solicitud;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.Button button_guardar_solicitud;
		[Widget] Gtk.Button button_envio_solicitud;
		[Widget] Gtk.Button button_quitar_productos;
		[Widget] Gtk.Button button_imprime_solicitud;
		[Widget] Gtk.Button button_buscar_solicitudes;
		[Widget] Gtk.Entry entry_folio_servicio = null;
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_nombre_paciente = null;
		[Widget] Gtk.Button button_busca_paciente = null;
		[Widget] Gtk.CheckButton checkbutton_nueva_solicitud;
		[Widget] Gtk.CheckButton checkbutton_sol_parastock = null;
		[Widget] Gtk.CheckButton checkbutton_presolicitud = null;
		[Widget] Gtk.Entry entry_id_cirugia = null;
		[Widget] Gtk.Entry entry_cirugia = null;
		[Widget] Gtk.Entry entry_diagnostico = null;
		[Widget] Gtk.TreeView lista_produc_solicitados = null;
		[Widget] Gtk.Button button_selecciona_pq = null;
		[Widget] Gtk.Entry entry_observacion = null;
		[Widget] Gtk.CheckButton checkbutton_solicitud_paquete = null;
		[Widget] Gtk.ComboBox combobox_tipo_solicitud = null;
		[Widget] Gtk.Button button_autoriza_solicitud = null;
		[Widget] Gtk.Entry entry_rojo = null;
		[Widget] Gtk.Entry entry_azul = null;
		[Widget] Gtk.Entry entry_verde = null;
		
		//Declarando la barra de estado
		[Widget] Gtk.Statusbar statusbar_hospital;
		
		/////// Ventana Busqueda de productos\\\\\\\\
		[Widget] Gtk.TreeView lista_de_producto;
		//[Widget] Gtk.Button button_agrega_extra;
		[Widget] Gtk.Entry entry_cantidad_aplicada;
		[Widget] Gtk.Label label_titulo_cantidad;
		
		//private TreeStore treeViewEngineBusca;
		private TreeStore treeViewEngineBusca2;
		private ListStore treeViewEngineSolicitud;
		
		//private ArrayList arraysolicitudmat;		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		string connectionString;

		float valoriva;
		int idalmacen;    // Esta variable almacena el codigo del almacen de esta clase, se recibe como parametro de la clase
		string filtro_query_alta = "AND osiris_erp_cobros_enca.alta_paciente = 'false' "; // solo caja podra pedir por paciente 
		
		int ultimasolicitud;		// Toma el ultimo numero de solictud
		
		bool editar = true;				// me indica si puedo agregar mas productos a la solicitud
		
		int filas=690;
		int contador = 1;
		int numpage = 1;
		string tipodesolicitud = "";
		string[] args_args = {""};
		string[] args_tiposolicitud ={"","ORDINARIA","URGENTE"};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8};
		
		//Declaracion de ventana de error y pregunta
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();	
		
		public solicitud_material(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_,int idalmacen_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			idalmacen = idalmacen_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			valoriva = float.Parse(classpublic.ivaparaaplicar);
						
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "solicitud_materiales", null);
			gxml.Autoconnect (this);        
			////// Muestra ventana de Glade
			solicitud_materiales.Show();
			
			// acciones de botones
			// Validando que sen solo numeros
			entry_numero_solicitud.KeyPressEvent += onKeyPressEvent_enter_solicitud;
			// Busqueda de Productos
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			// Quitar productos editar tiene que estar en true
			button_quitar_productos.Clicked += new EventHandler(on_button_quitar_productos_clicked);
			// Button nueva solicitud
			checkbutton_nueva_solicitud.Clicked += new EventHandler(on_checkbutton_nueva_solicitud_clicked);
			//Button Guardar Solicitud
			button_guardar_solicitud.Clicked += new EventHandler(on_button_guardar_solicitud_clicked);
			//Button Seleccion una Solicitud
			button_selecciona_solicitud.Clicked += new EventHandler(on_button_selecciona_solicitud_clicked);
			//Button envio de solicitud para alamcen
			button_envio_solicitud.Clicked += new EventHandler(on_button_envio_solicitud_clicked);
			//button_buscar_solicitudes.Clicked += new EventHandler(on_button_buscar_solicitudes_clicked);
			checkbutton_sol_parastock.Clicked += new EventHandler(on_checkbutton_sol_parastock_clicked);
			checkbutton_presolicitud.Clicked += new EventHandler(on_checkbutton_presolicitud_clicked);
			//buscar pacientes
			button_busca_paciente.Clicked += new EventHandler(on_button_busca_paciente_clicked);
			button_imprime_solicitud.Clicked += new EventHandler(on_button_imprime_solicitud_clicked);
			button_selecciona_pq.Clicked += new EventHandler(on_button_selecciona_pq_clicked);
			button_guardar_solicitud.Sensitive = false;
			button_envio_solicitud.Sensitive = false;
			button_quitar_productos.Sensitive = false;
			button_busca_producto.Sensitive = false;
			button_imprime_solicitud.Sensitive = true;
			entry_id_cirugia.Sensitive = false;
			entry_cirugia.Sensitive = false;
			entry_diagnostico.Sensitive = false;
			checkbutton_presolicitud.Sensitive = false;
			checkbutton_sol_parastock.Sensitive = false;
			entry_id_cirugia.IsEditable = false;
						
			entry_quien_solicita.Text = NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado;
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			// Colores de los cuadros		
			entry_rojo.ModifyBase(StateType.Normal, new Gdk.Color(255,0,0));
			entry_azul.ModifyBase(StateType.Normal, new Gdk.Color(0,0,255));
			entry_verde.ModifyBase(StateType.Normal, new Gdk.Color(0,255,0));			
			statusbar_hospital.Pop(0);
			statusbar_hospital.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_hospital.HasResizeGrip = false;			
			//entry_folio_servicio.Text = "ejemplo";			
			crea_treeview_solicitud();
			if (idalmacen_ == 16){
				filtro_query_alta = " ";
			}
			// Verificando que pueda crear pre-solicitudes
			checkbutton_presolicitud.Sensitive = (bool) verifica_sub_almacenes(idalmacen_);
		}
		
		bool verifica_sub_almacenes(int idalmacen_)
		{
			bool acceso_a_presolicitud = false;
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	    	// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT * FROM osiris_almacenes WHERE id_almacen = '"+idalmacen_.ToString().Trim()+"' " +
					"AND pre_solicitudes = 'true';";
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if(lector.Read()){
					acceso_a_presolicitud = true;
				}
			}catch (NpgsqlException ex){
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, 
								ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			conexion.Close();
			return acceso_a_presolicitud;
		}
		
		void llenado_combobox(int tipodellenado,string descrip_defaul,object obj,string sql_or_array,string query_SQL,string name_field_desc,string name_field_id,string[] args_array,int[] args_id_array,string name_field_id2)
		{			
			Gtk.ComboBox combobox_llenado = (Gtk.ComboBox) obj;
			//Gtk.ComboBox combobox_pos_neg = obj as Gtk.ComboBox;
			combobox_llenado.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_llenado.PackStart(cell, true);
			combobox_llenado.AddAttribute(cell,"text",0);	        
			ListStore store = new ListStore( typeof (string),typeof (int),typeof(bool));
			combobox_llenado.Model = store;			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) descrip_defaul,0);
			}			
			if(sql_or_array == "array"){			
				for (int colum_field = 0; colum_field < args_array.Length; colum_field++){
					store.AppendValues (args_array[colum_field],args_id_array[colum_field]);
				}
			}
			if(sql_or_array == "sql"){			
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
	            // Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
	               	comando.CommandText = query_SQL;					
					NpgsqlDataReader lector = comando.ExecuteReader ();
	               	while (lector.Read()){
						store.AppendValues ((string) lector[ name_field_desc ], (int) lector[ name_field_id],false);
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
				conexion.Close ();
			}			
			TreeIter iter;
			if (store.GetIterFirst(out iter)){
				combobox_llenado.SetActiveIter (iter);
			}
			combobox_llenado.Changed += new EventHandler (onComboBoxChanged_llenado);			
		}
		
		void onComboBoxChanged_llenado (object sender, EventArgs args)
		{
			ComboBox onComboBoxChanged = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (onComboBoxChanged.GetActiveIter (out iter)){
				switch (onComboBoxChanged.Name.ToString()){	
				case "combobox_tipo_solicitud":
					tipodesolicitud = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
				}
			}
		}
		
		void on_button_selecciona_pq_clicked(object sender, EventArgs args)
		{
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion
			// la clase recibe tambien el orden del query
			// es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo
			object[] parametros_objetos = {entry_id_cirugia,entry_cirugia,lista_produc_solicitados,treeViewEngineSolicitud};
			string[] parametros_sql = {"SELECT id_tipo_cirugia,descripcion_cirugia,tiene_paquete,to_char(valor_paquete,'999999999.99') AS valorpaquetereal,"+
										"to_char(precio_de_venta,'999999999.99') AS valorpaquete "+
										"FROM osiris_his_tipo_cirugias ",															
										"SELECT id_tipo_cirugia,descripcion_cirugia,tiene_paquete,to_char(valor_paquete,'999999999.99') AS valorpaquetereal,"+
										"to_char(precio_de_venta,'999999999.99') AS valorpaquete "+
										"FROM osiris_his_tipo_cirugias "+
										"WHERE descripcion_cirugia LIKE '%"};
			string[] parametros_string = {};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_cirugia_paquetes_soliprod"," ORDER BY id_tipo_cirugia","%' ",0);
		}
		
		void on_button_busca_producto_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "busca_producto", null);
			gxml.Autoconnect (this);			
			crea_treeview_busqueda("producto");			
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
			label_titulo_cantidad.Text = "Cantidad Solicitada";	
			entry_expresion.KeyPressEvent += onKeyPressEvent_entry_expresion;			
			// Validando que sen solo numeros
			entry_cantidad_aplicada.KeyPressEvent += onKeyPressEvent;
	    }
	    
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_entry_expresion(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llenando_lista_de_productos();			
			}
		}
	    void on_checkbutton_nueva_solicitud_clicked(object sender, EventArgs args)
	    {
	    	string ultimasolicitud;
			if (checkbutton_nueva_solicitud.Active == true){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de CREAR una Nueva SOLICITUD ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
	    			ultimasolicitud = classpublic.lee_ultimonumero_registrado("osiris_his_solicitudes_deta","folio_de_solicitud","WHERE id_almacen = '"+idalmacen.ToString().Trim()+"' ");
					entry_numero_solicitud.Text = ultimasolicitud.ToString().Trim();
					entry_fecha_solicitud.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");	    			
	    			treeViewEngineSolicitud.Clear(); // Limpia el treeview
	    			button_guardar_solicitud.Sensitive = true;
					button_quitar_productos.Sensitive = true;
					button_busca_producto.Sensitive = true;
					button_selecciona_solicitud.Sensitive = false;
					button_buscar_solicitudes.Sensitive = false;
					entry_numero_solicitud.IsEditable = false;	
					entry_status_solicitud.Text = "";
					// Verificando que pueda crear pre-solicitudes
					checkbutton_presolicitud.Sensitive = (bool) verifica_sub_almacenes(idalmacen);
					checkbutton_sol_parastock.Sensitive = true;
					entry_folio_servicio.Text = "0";
					entry_pid_paciente.Text = "0";
					entry_nombre_paciente.Text = "";
					llenado_combobox(0,"",combobox_tipo_solicitud,"array","","","",args_tiposolicitud,args_id_array,"");
	     		}else{
	     			checkbutton_nueva_solicitud.Active = false;
	     		}
	    	}else{
	    		button_guardar_solicitud.Sensitive = false;
				button_envio_solicitud.Sensitive = false;
				button_quitar_productos.Sensitive = false;
				button_busca_producto.Sensitive = false;
				button_selecciona_solicitud.Sensitive = true;
				button_buscar_solicitudes.Sensitive = true;
				entry_numero_solicitud.IsEditable = true;
				checkbutton_presolicitud.Sensitive = false;
				checkbutton_sol_parastock.Sensitive = false;
		 	}
	    }
		
		void on_button_busca_paciente_clicked(object sender, EventArgs args)
		{
			string sql1 = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo " +
									"FROM osiris_his_paciente,osiris_erp_cobros_enca WHERE activo = 'true' "+
										"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
										"AND osiris_erp_cobros_enca.cancelado = 'false' "+
										filtro_query_alta+
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cerrado = 'false' "+
										"AND osiris_erp_cobros_enca.reservacion = 'false' ";
			string sql2 = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo " +
									"FROM osiris_his_paciente,osiris_erp_cobros_enca WHERE activo = 'true' "+
										"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
										"AND osiris_erp_cobros_enca.cancelado = 'false' "+
										filtro_query_alta+
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cerrado = 'false' "+
										"AND osiris_erp_cobros_enca.reservacion = 'false' "+
										"AND apellido_paterno_paciente LIKE '%";
			string sql3 = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo " +
									"FROM osiris_his_paciente,osiris_erp_cobros_enca WHERE activo = 'true' "+
										"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
										"AND osiris_erp_cobros_enca.cancelado = 'false' "+
										filtro_query_alta+
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cerrado = 'false' "+
										"AND osiris_erp_cobros_enca.reservacion = 'false' "+
										"AND nombre1_paciente LIKE '%";
			string sql4 = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo " +
									"FROM osiris_his_paciente,osiris_erp_cobros_enca WHERE activo = 'true' "+
										"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
										"AND osiris_erp_cobros_enca.cancelado = 'false' "+
										filtro_query_alta+
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cerrado = 'false' "+
										"AND osiris_erp_cobros_enca.reservacion = 'false' "+
										"AND osiris_his_paciente.pid_paciente = '";
			object[] parametros_objetos = {entry_folio_servicio,entry_pid_paciente,entry_nombre_paciente};
			string[] parametros_sql = {sql1, sql2, sql3, sql4};
			string[] parametros_string = {};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_paciente"," ORDER BY osiris_his_paciente.pid_paciente","%' ",1);
		}
	    
	    void on_button_selecciona_solicitud_clicked(object sender, EventArgs args)
	    {
	    	llena_solicitud_material(entry_numero_solicitud.Text);
	    }
	    
	    void on_button_guardar_solicitud_clicked(object sender, EventArgs args)
	    {
	    	if (checkbutton_nueva_solicitud.Active == true){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de GUARDAR esta SOLICITUD ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
					if((bool) checkbutton_presolicitud.Active == true){
						if(entry_nombre_paciente.Text != ""){
							almacena_productos_solicitados();
							//editar = true;
		 					//entry_status_solicitud.Text = "NO ESTA ENVIADA";
						}else{
							msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
											MessageType.Error,ButtonsType.Close,"No tiene definido el nombre del paciente verifique...");										
							msgBox.Run ();			msgBox.Destroy();
						}
					}else{
		 				editar = true;
		 				entry_status_solicitud.Text = "NO ESTA ENVIADA";
						almacena_productos_solicitados();
					}		 			
		 		}
		 	 }
		 	 
		 	 if (editar == false){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de GUARDAR esta SOLICITUD ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
					if((bool) checkbutton_presolicitud.Active == true){
						Console.WriteLine("presolicitud");
						if(entry_nombre_paciente.Text != ""){
							almacena_productos_solicitados();
						}else{
							msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
											MessageType.Error,ButtonsType.Close,"No tiene definido el nombre del paciente verifique...");										
							msgBox.Run ();			msgBox.Destroy();
						}
					}else{
		 				almacena_productos_solicitados();
					}
		 		}
		 	 }
		 }
		
		 void almacena_productos_solicitados()
		 {
			string ultimasolicitud;
			int folio_de_servicio = int.Parse((string) entry_folio_servicio.Text.ToString().Trim());
			TreeIter iter;
			if((bool) checkbutton_sol_parastock.Active == true){
				folio_de_servicio = 1;
			}
			if((bool) checkbutton_presolicitud.Active == true){
				folio_de_servicio = 1;
			}
			if(folio_de_servicio != 0){
				if(editar == true){
					ultimasolicitud = classpublic.lee_ultimonumero_registrado("osiris_his_solicitudes_deta","folio_de_solicitud","WHERE id_almacen = '"+idalmacen.ToString().Trim()+"' ");
					entry_numero_solicitud.Text = ultimasolicitud.ToString().Trim();
				}
				if (this.treeViewEngineSolicitud.GetIterFirst (out iter)){
					if(tipodesolicitud.Trim() != ""){
						button_envio_solicitud.Sensitive = true;
						// Verifica que la base de datos este conectada
						NpgsqlConnection conexion; 
						conexion = new NpgsqlConnection (connectionString+nombrebd);
						try{
							conexion.Open ();
							NpgsqlCommand comando; 
							comando = conexion.CreateCommand ();
							if ((bool) this.lista_produc_solicitados.Model.GetValue (iter,8) == false){
								comando.CommandText = "INSERT INTO osiris_his_solicitudes_deta("+
																			"folio_de_solicitud,"+
																			"id_producto,"+
																			"precio_producto_publico,"+
																			"costo_por_unidad,"+
																			"cantidad_solicitada,"+
																			"fechahora_solicitud,"+
																			"id_quien_solicito,"+
																			"id_almacen,folio_de_servicio,pid_paciente,"+
																			"solicitud_stock,"+
																			"pre_solicitud,"+
																			"nombre_paciente,"+
																			"procedimiento_qx,"+
																			"diagnostico_qx," +
																			"observaciones_solicitud," +
																			"tipo_solicitud) "+
																			"VALUES ('"+
																			this.entry_numero_solicitud.Text+"','"+
																			(string) this.lista_produc_solicitados.Model.GetValue(iter,1)+"','"+
																			(string) this.lista_produc_solicitados.Model.GetValue(iter,7)+"','"+
																			(string) this.lista_produc_solicitados.Model.GetValue(iter,6)+"','"+
																			(string) this.lista_produc_solicitados.Model.GetValue(iter,2)+"','"+
																			DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																			LoginEmpleado+"','"+
																			this.idalmacen.ToString()+"','"+
																			(string) entry_folio_servicio.Text.ToString().Trim()+"','"+
																			(string) entry_pid_paciente.Text.ToString().Trim()+"','"+
																			(bool) checkbutton_sol_parastock.Active+"','"+
																			(bool) checkbutton_presolicitud.Active+"','"+
																			(string) entry_nombre_paciente.Text.ToString().Trim().ToUpper()+"','"+
																			(string) entry_cirugia.Text.ToString().Trim().ToUpper()+"','"+
																			(string) entry_diagnostico.Text.ToString().Trim().ToUpper()+"','"+
																			(string) entry_observacion.Text.ToString().Trim().ToUpper()+"','"+
																			tipodesolicitud+"');";
																		
								//Console.WriteLine(comando.CommandText);
								comando.ExecuteNonQuery();
								comando.Dispose();
							}
							while (this.treeViewEngineSolicitud.IterNext(ref iter)){						
								if ((bool) this.lista_produc_solicitados.Model.GetValue (iter,8) == false){
									comando.CommandText = "INSERT INTO osiris_his_solicitudes_deta("+
																			"folio_de_solicitud,"+
																			"id_producto,"+
																			"precio_producto_publico,"+
																			"costo_por_unidad,"+
																			"cantidad_solicitada,"+
																			"fechahora_solicitud,"+
																			"id_quien_solicito,"+
																			"id_almacen,folio_de_servicio,pid_paciente,"+
																			"solicitud_stock,"+
																			"pre_solicitud,"+
																			"nombre_paciente,"+
																			"procedimiento_qx,"+
																			"diagnostico_qx," +
																			"observaciones_solicitud," +
																			"tipo_solicitud) "+
																			"VALUES ('"+
																			this.entry_numero_solicitud.Text+"','"+
																			(string) this.lista_produc_solicitados.Model.GetValue(iter,1)+"','"+
																			(string) this.lista_produc_solicitados.Model.GetValue(iter,7)+"','"+
																			(string) this.lista_produc_solicitados.Model.GetValue(iter,6)+"','"+
																			(string) this.lista_produc_solicitados.Model.GetValue(iter,2)+"','"+
																			DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																			LoginEmpleado+"','"+
																			this.idalmacen.ToString()+"','"+
																			(string) entry_folio_servicio.Text.ToString().Trim()+"','"+
																			(string) entry_pid_paciente.Text.ToString().Trim()+"','"+
																			(bool) checkbutton_sol_parastock.Active+"','"+
																			(bool) checkbutton_presolicitud.Active+"','"+
																			(string) entry_nombre_paciente.Text.ToString().Trim().ToUpper()+"','"+
																			(string) entry_cirugia.Text.ToString().Trim().ToUpper()+"','"+
																			(string) entry_diagnostico.Text.ToString().Trim().ToUpper()+"','"+
																			(string) entry_observacion.Text.ToString().Trim().ToUpper()+"','"+
																			tipodesolicitud+"');";
																		
								//Console.WriteLine(comando.CommandText);
									comando.ExecuteNonQuery();
									comando.Dispose();
								}
							
							}
							checkbutton_nueva_solicitud.Active = false;
							llena_solicitud_material(entry_numero_solicitud.Text);
						}catch (NpgsqlException ex){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
															MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();				msgBoxError.Destroy();
						}
						conexion.Close();
					}else{
						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
												MessageType.Error,ButtonsType.Close,"NO ha seleccionado el TIPO DE SOLICITUD, NO se ha podido grabar la solicitud...");										
								msgBox.Run ();			msgBox.Destroy();
					}
				}else{
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
												MessageType.Error,ButtonsType.Close,"No ha pedido ningun producto, NO se ha podido grabar la solicitud...");										
					msgBox.Run ();			msgBox.Destroy();
				}
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Error,ButtonsType.Ok,"Debe elegir un paciente para realizar la solicitud, verifique...");
				msgBox.Run (); 	msgBox.Destroy();
			}
	    }
		 
		 void on_button_quitar_productos_clicked(object sender, EventArgs args)
		 {
		 	TreeModel model;
			TreeIter iterSelected;
 			if (this.lista_produc_solicitados.Selection.GetSelected(out model, out iterSelected)){
 				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta quitar el producto "+(string) this.lista_produc_solicitados.Model.GetValue (iterSelected,0));
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){					
					if ((bool) this.lista_produc_solicitados.Model.GetValue (iterSelected,8) == false){
						treeViewEngineSolicitud.Remove (ref iterSelected);
						msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
											MessageType.Info,ButtonsType.Ok,"El Producto se quito satisfactoreamente...");										
						msgBox.Run ();			msgBox.Destroy();
					}else{
			 			NpgsqlConnection conexion; 
						conexion = new NpgsqlConnection (connectionString+nombrebd);
				    	// Verifica que la base de datos este conectada
						try{
							conexion.Open ();
							NpgsqlCommand comando; 
							comando = conexion.CreateCommand ();
							comando.CommandText = "UPDATE osiris_his_solicitudes_deta SET id_quien_elimino ='"+LoginEmpleado+"',"+ 
												"fechahora_elimando = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
												"eliminado = 'true' "+
												"WHERE id_secuencia =  '"+(string) this.lista_produc_solicitados.Model.GetValue (iterSelected,10)+"';";
							
							comando.ExecuteNonQuery();
							comando.Dispose();
							treeViewEngineSolicitud.Remove (ref iterSelected);
				        	msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
											MessageType.Info,ButtonsType.Ok,"El Producto se quito satisfactoreamente...");										
							msgBox.Run ();						msgBox.Destroy();
				        }catch (NpgsqlException ex){
					   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
											ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();
							msgBoxError.Destroy();
						}
						conexion.Close ();
					}
		 		}
 			}
		 }
	    
	    void on_button_envio_solicitud_clicked(object sender, EventArgs args)
	    {
	    	if (checkbutton_nueva_solicitud.Active == false){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de ENVIAR esta SOLICITUD ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
	    			NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						comando.CommandText = "UPDATE osiris_his_solicitudes_deta SET status = true,"+
											"fecha_envio_almacen = '"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
											"id_empleado = '"+LoginEmpleado+"' "+
											"WHERE osiris_his_solicitudes_deta.folio_de_solicitud = '"+(string) entry_numero_solicitud.Text+"' "+ 
											"AND id_almacen = '"+this.idalmacen.ToString().Trim()+"';";
						//Console.WriteLine(comando.CommandText);
						comando.ExecuteNonQuery();
						comando.Dispose();
						
						msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
								MessageType.Info,ButtonsType.Ok,"La SOLICITUD de ENVIO satisfactoriamente...");
						miResultado = (ResponseType)msgBox.Run (); 	msgBox.Destroy();
						this.button_envio_solicitud.Sensitive = false;
						this.button_busca_producto.Sensitive = false;
						this.button_guardar_solicitud.Sensitive = false;
						this.button_quitar_productos.Sensitive = false;
						entry_status_solicitud.Text = "ENVIADA ALMACEN";
					}catch (NpgsqlException ex){
		   				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
							msgBoxError.Run ();					msgBoxError.Destroy();
					}
					conexion.Close();
				}					
			}
	    } 
	    
	    void llena_solicitud_material(string numerodesolicutud)
	    {
        	this.treeViewEngineSolicitud.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			editar = true;
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT osiris_his_solicitudes_deta.folio_de_solicitud,to_char(osiris_his_solicitudes_deta.id_producto,'999999999999') AS idproductos,"+
               						"to_char(cantidad_solicitada,'999999.999') AS cantidadsolicitada,tipo_solicitud,"+
               						"to_char(osiris_his_solicitudes_deta.precio_producto_publico,'999999999.99') AS precioproductopublico,"+
               						"to_char(osiris_his_solicitudes_deta.costo_por_unidad,'999999999.99') AS costoporunidad,"+
               						"to_char(cantidad_autorizada,'999999.999') AS cantidadautorizada,id_quien_autorizo, "+
               						"to_char(fechahora_solicitud,'dd-MM-yyyy') AS fechahorasolicitud,"+
               						"to_char(fechahora_autorizado,'dd-MM-yyyy') AS fechahoraautorizado,procedimiento_qx,diagnostico_qx,"+
               						"status,surtido,osiris_productos.descripcion_producto,"+
               						"to_char(osiris_his_solicitudes_deta.id_secuencia,'9999999999') AS idsecuencia,"+
									"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,"+
									"osiris_his_solicitudes_deta.folio_de_servicio AS foliodeatencion,"+
									"osiris_his_solicitudes_deta.pid_paciente AS pidpaciente,nombre_paciente,observaciones_solicitud,pre_solicitud,solicitud_stock,"+
									"nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente "+
               						"FROM osiris_his_solicitudes_deta,osiris_his_paciente,osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
               						"WHERE osiris_his_solicitudes_deta.folio_de_solicitud = '"+(string) numerodesolicutud+"' "+
               						"AND eliminado = 'false' "+
               						"AND osiris_his_paciente.pid_paciente = osiris_his_solicitudes_deta.pid_paciente "+
									"AND osiris_his_solicitudes_deta.id_producto = osiris_productos.id_producto "+
               						"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
									"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
									"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
									"AND id_almacen = '"+this.idalmacen.ToString().Trim()+"' "+
									"ORDER BY osiris_his_solicitudes_deta.id_secuencia;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if(lector.Read()){
					entry_folio_servicio.Text = (string) lector["foliodeatencion"].ToString().Trim();
					entry_pid_paciente.Text = (string) lector["pidpaciente"].ToString().Trim();
					entry_nombre_paciente.Text = (string) lector["nombre1_paciente"].ToString().Trim()+" "+
												(string) lector["nombre2_paciente"].ToString().Trim()+" "+
												(string) lector["apellido_paterno_paciente"].ToString().Trim()+" "+
												(string) lector["apellido_materno_paciente"].ToString().Trim();
					entry_diagnostico.Text = (string) lector["diagnostico_qx"].ToString().Trim();
					entry_cirugia.Text = (string) lector["procedimiento_qx"].ToString().Trim();
					entry_observacion.Text = (string) lector["observaciones_solicitud"];
					entry_fecha_solicitud.Text = (string) lector["fechahorasolicitud"];
					llenado_combobox(1,(string) lector["tipo_solicitud"],combobox_tipo_solicitud,"array","","","",args_tiposolicitud,args_id_array,"");
					if((bool) lector["solicitud_stock"] == true){
						checkbutton_sol_parastock.Active = true;
						entry_nombre_paciente.Text = (string) lector["nombre_paciente"].ToString().Trim();
					}
					if((bool) lector["pre_solicitud"] == true){
						checkbutton_presolicitud.Active = true;
						entry_nombre_paciente.Text = (string) lector["nombre_paciente"].ToString().Trim();
					}
					if((bool) lector["status"] == false){
						editar = false;
					}
					treeViewEngineSolicitud.AppendValues((string) lector["descripcion_producto"],
													(string) lector["idproductos"],
													(string) lector["cantidadsolicitada"],
													(string) lector["fechahorasolicitud"],
													(string) lector["cantidadautorizada"],
													(string) lector["fechahoraautorizado"],
													(string) lector["costoporunidad"],
													(string) lector["precioproductopublico"],
													true,
													(bool) lector["surtido"],
													(string) lector["idsecuencia"]);
				
					while(lector.Read()){
						if((bool) lector["status"] == false){
							editar = false;
						}
						treeViewEngineSolicitud.AppendValues((string) lector["descripcion_producto"],
														(string) lector["idproductos"],
														(string) lector["cantidadsolicitada"],
														(string) lector["fechahorasolicitud"],
														(string) lector["cantidadautorizada"],
														(string) lector["fechahoraautorizado"],
														(string) lector["costoporunidad"],
														(string) lector["precioproductopublico"],
														true,
														(bool) lector["surtido"],
														(string) lector["idsecuencia"]);
					}
					if(editar == false){
						this.button_envio_solicitud.Sensitive = true;
						this.button_busca_producto.Sensitive = true;
						this.button_guardar_solicitud.Sensitive = true;
						this.button_quitar_productos.Sensitive = true;
						entry_status_solicitud.Text = "NO ESTA ENVIADA";
					}else{
						this.button_envio_solicitud.Sensitive = false;
						this.button_busca_producto.Sensitive = false;
						this.button_guardar_solicitud.Sensitive = false;
						this.button_quitar_productos.Sensitive = false;
						entry_status_solicitud.Text = "ENVIADA ALMACEN";
						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"Esta SOLICITUD ya se envio para ALMACEN, Verifique por favor");
						msgBox.Run (); 	msgBox.Destroy();
					}
				}else{
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Error,ButtonsType.Ok,"Esta SOLICITUD NO EXISTE, Verifique por favor");
					msgBox.Run (); 	msgBox.Destroy();
				}					
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
							msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion.Close ();
	    }
	    		
		void crea_treeview_busqueda(string tipo_busqueda)
		{
			if (tipo_busqueda == "solicitud"){
				
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
            	
				TreeViewColumn col_grupoprod = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_grupoprod.Title = "Grupo Producto";//Precio Producto
				col_grupoprod.PackStart(cellrt2, true);
				col_grupoprod.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
				col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
            
				TreeViewColumn col_grupo1prod = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_grupo1prod.Title = "Grupo1 Producto";//I.V.A.
				col_grupo1prod.PackStart(cellrt3, true);
				col_grupo1prod.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 2 en vez de 3
				col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
            
				TreeViewColumn col_grupo2prod = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_grupo2prod.Title = "Grupo2 Producto";//Total
				col_grupo2prod.PackStart(cellrt4, true);
				col_grupo2prod.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 3 en vez de 4
				col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;
            	
				lista_de_producto.AppendColumn(col_idproducto);  // 0
				lista_de_producto.AppendColumn(col_desc_producto); // 1
				lista_de_producto.AppendColumn(col_grupoprod);	//7
				lista_de_producto.AppendColumn(col_grupo1prod);	//8
				lista_de_producto.AppendColumn(col_grupo2prod);	//9							
			}
		}
		
		// llena la lista de productos
 		void on_llena_lista_producto_clicked (object sender, EventArgs args)
 		{
 			llenando_lista_de_productos();
		}
		
		void llenando_lista_de_productos()
		{
			string acceso_a_grupos = classpublic.lee_registro_de_tabla("osiris_almacenes","id_almacen"," WHERE osiris_almacenes.id_almacen = '"+this.idalmacen.ToString().Trim()+"' ","acceso_grupo_producto","int");
			
			treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();               	
				comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
						"osiris_productos.descripcion_producto,to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
						"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,"+
						"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
						"to_char(porcentage_ganancia,'99999.99') AS porcentageutilidad,to_char(costo_producto,'999999999.99') AS costoproducto "+
						"FROM osiris_productos,osiris_catalogo_almacenes,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
						"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
						"AND osiris_productos.id_producto = osiris_catalogo_almacenes.id_producto "+
						"AND osiris_catalogo_almacenes.id_almacen = '"+this.idalmacen.ToString().Trim()+"' "+
						"AND osiris_catalogo_almacenes.eliminado = 'false' "+	
						"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
						"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
						"AND osiris_productos.cobro_activo = true "+
						//"AND osiris_grupo_producto.agrupacion IN(= 'MD1' "+
						"AND osiris_productos.id_grupo_producto IN("+acceso_a_grupos+") "+
						"AND osiris_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper()+"%' ORDER BY descripcion_producto; ";
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				float tomaprecio;
				float calculodeiva;
				float preciomasiva;
				float tomadescue;
				float preciocondesc;							
				while (lector.Read()){
					calculodeiva = 0;
					preciomasiva = 0;
					tomaprecio = float.Parse((string) lector["preciopublico"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					tomadescue = float.Parse((string) lector["porcentagesdesc"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					preciocondesc = tomaprecio;
					if ((bool) lector["aplicar_iva"]){
						calculodeiva = (tomaprecio * valoriva)/100;
					}
					if ((bool) lector["aplica_descuento"]){
						preciocondesc = tomaprecio-((tomaprecio*tomadescue)/100);
					}
					preciomasiva = tomaprecio + calculodeiva; 
					treeViewEngineBusca2.AppendValues (
											(string) lector["codProducto"],//0
											(string) lector["descripcion_producto"],//1
											(string) lector["descripcion_grupo_producto"],//2
											(string) lector["descripcion_grupo1_producto"],//3
											(string) lector["descripcion_grupo2_producto"],//4
											(string) lector["preciopublico"],//5
											calculodeiva.ToString("F").PadLeft(10),//6
											preciomasiva.ToString("F").PadLeft(10),//7
											(string) lector["porcentagesdesc"],//8
											preciocondesc.ToString("F").PadLeft(10),//9
											(string) lector["costoproductounitario"],//10
											(string) lector["porcentageutilidad"],//11
											(string) lector["costoproducto"]);//12
					
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		enum Column_prod
		{
			col_idproducto,
			col_desc_producto,
			col_grupoprod,
			col_grupo1prod,
			col_grupo2prod
		}
		
		void crea_treeview_solicitud()
		{
			//arraysolicitudmat = new ArrayList();
			
			treeViewEngineSolicitud = new ListStore(typeof(string),		// 0 
													typeof(string),		// 1
													typeof(string),		// 2
													typeof(string),		// 3
													typeof(string),		// 4
													typeof(string),		// 5
													typeof(string),		// costo_por_unidad
													typeof(string),		// precio_prodcuto_publico
													typeof(bool),		// Almacena cambia de Color
													typeof(bool),		// Cambia color verde cuando esta surtido
													typeof(string)		// Almacena el el numero de secuencia
													);
												
			lista_produc_solicitados.Model = treeViewEngineSolicitud;
			
			lista_produc_solicitados.RulesHint = true;
			
			TreeViewColumn col_descripcion = new TreeViewColumn();
			CellRendererText cel_descripcion = new CellRendererText();
			col_descripcion.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
			col_descripcion.PackStart(cel_descripcion, true);
			col_descripcion.AddAttribute (cel_descripcion, "text", 0);
			col_descripcion.SetCellDataFunc(cel_descripcion,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			col_descripcion.Resizable = true;
			col_descripcion.SortColumnId = (int) colum_solicitudes.col_descripcion;
			
			TreeViewColumn col_codigo_prod = new TreeViewColumn();
			CellRendererText cel_codigo_prod = new CellRendererText();
			col_codigo_prod.Title = "Codigo"; // titulo de la cabecera de la columna, si está visible
			col_codigo_prod.PackStart(cel_codigo_prod, true);
			col_codigo_prod.AddAttribute (cel_codigo_prod, "text", 1);
			col_codigo_prod.SetCellDataFunc(cel_codigo_prod,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_cant_solicitado = new TreeViewColumn();
			CellRendererText cel_cant_solicitado = new CellRendererText();
			col_cant_solicitado.Title = "Solicitado"; // titulo de la cabecera de la columna, si está visible
			col_cant_solicitado.PackStart(cel_cant_solicitado, true);
			col_cant_solicitado.AddAttribute (cel_cant_solicitado, "text", 2);
			col_cant_solicitado.SetCellDataFunc(cel_cant_solicitado,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_fecha_solicitado = new TreeViewColumn();
			CellRendererText cel_fecha_solicitado = new CellRendererText();
			col_fecha_solicitado.Title = "Fecha Solicitado"; // titulo de la cabecera de la columna, si está visible
			col_fecha_solicitado.PackStart(cel_fecha_solicitado, true);
			col_fecha_solicitado.AddAttribute (cel_fecha_solicitado, "text", 3);
			col_fecha_solicitado.SetCellDataFunc(cel_fecha_solicitado,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_cant_recibido = new TreeViewColumn();
			CellRendererText cel_cant_recibido = new CellRendererText();
			col_cant_recibido.Title = "Recibido"; // titulo de la cabecera de la columna, si está visible
			col_cant_recibido.PackStart(cel_cant_recibido, true);
			col_cant_recibido.AddAttribute (cel_cant_recibido, "text", 4);
			col_cant_recibido.SetCellDataFunc(cel_cant_recibido,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_fecha_recibido = new TreeViewColumn();
			CellRendererText cel_fecha_recibido = new CellRendererText();
			col_fecha_recibido.Title = "Fecha Recibido"; // titulo de la cabecera de la columna, si está visible
			col_fecha_recibido.PackStart(cel_fecha_recibido, true);
			col_fecha_recibido.AddAttribute (cel_fecha_recibido, "text", 5);
			col_fecha_recibido.SetCellDataFunc(cel_fecha_recibido,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			lista_produc_solicitados.AppendColumn(col_descripcion);
			lista_produc_solicitados.AppendColumn(col_codigo_prod);
			lista_produc_solicitados.AppendColumn(col_cant_solicitado);
			lista_produc_solicitados.AppendColumn(col_fecha_solicitado);
			lista_produc_solicitados.AppendColumn(col_cant_recibido);
			lista_produc_solicitados.AppendColumn(col_fecha_recibido);						
		}
		
		enum colum_solicitudes
		{
			col_descripcion,
			col_codigo_prod,
			col_cant_solicitado,
			col_fecha_solicitado,
			col_cant_recibido,
			cel_fecha_recibido
		}
		
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
 			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
 				if ((float) float.Parse((string) entry_cantidad_aplicada.Text) > 0){
 					treeViewEngineSolicitud.AppendValues ((string) model.GetValue(iterSelected, 1),
 														(string) model.GetValue(iterSelected, 0),
 														entry_cantidad_aplicada.Text,
 														(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
 														"",
 														"",
 														(string) model.GetValue(iterSelected, 10),
 														(string) model.GetValue(iterSelected, 5),
 														false,
 														false,
 														"");
 					
 					entry_cantidad_aplicada.Text = "0";
 				}else{
 					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error,ButtonsType.Close, 
											"La cantidad que quiere solicitar debe ser \n"+"distinta a cero, intente de nuevo");
					msgBoxError.Run ();					msgBoxError.Destroy();
 				}
 			} 			
 		}
 		
 		// Valida entradas que solo sean numericas, se utiliza en ventana de
		// carga de numero de solicitud
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_solicitud(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llena_solicitud_material((string) this.entry_numero_solicitud.Text);				
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1){
				args.RetVal = true;
			}
		}
	    
	    // Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1){
				args.RetVal = true;
			}
		}
		
		//ACCION QUE CAMBIA EL COLOR DEL TEXTO PARA CUANDO SE GUARDA EN LA BASE DE DATOS 
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//Console.WriteLine( cell.GetType().ToString());
			//descripcion_producto descrip = (descripcion_producto) model.GetValue (iter, 14);
			if ((bool) this.lista_produc_solicitados.Model.GetValue (iter,8)==true){
				(cell as Gtk.CellRendererText).Foreground = "darkblue";
				if ((bool) this.lista_produc_solicitados.Model.GetValue (iter,9) == true){
					(cell as Gtk.CellRendererText).Foreground = "darkgreen";
				}
			}else{				
				(cell as Gtk.CellRendererText).Foreground = "red";
			}			
		}
				
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		void on_button_imprime_solicitud_clicked(object sender, EventArgs args)
		{		
			string query_in_num = " AND osiris_his_solicitudes_deta.folio_de_solicitud = '"+entry_numero_solicitud.Text+"' ";
			string query_in_almacen = " AND osiris_almacenes.id_almacen = '"+this.idalmacen.ToString().Trim()+"' ";
			string query_fechas = "";
			new osiris.rpt_solicitud_subalmacenes(idalmacen,query_in_num,query_in_almacen,query_fechas);
		}
		
		void on_checkbutton_sol_parastock_clicked(object sender, EventArgs args)
		{
			if(checkbutton_sol_parastock.Active == true){
				checkbutton_presolicitud.Active = false;
				entry_folio_servicio.Sensitive = false;
				entry_pid_paciente.Sensitive = false;
				entry_nombre_paciente.Sensitive = false;
				button_busca_paciente.Sensitive = false;
				entry_id_cirugia.Sensitive = false;
				entry_cirugia.Sensitive = false;
				entry_diagnostico.Sensitive = false;
				checkbutton_presolicitud.Active = false;
			}else{
				entry_folio_servicio.Sensitive = true;
				entry_pid_paciente.Sensitive = true;
				entry_nombre_paciente.Sensitive = true;
				button_busca_paciente.Sensitive = true;
				entry_id_cirugia.Sensitive = false;
				entry_cirugia.Sensitive = false;
				entry_diagnostico.Sensitive = false;
			}
		}
		
		void on_checkbutton_presolicitud_clicked(object sender, EventArgs args)
		{
			if(checkbutton_presolicitud.Active == true){
				checkbutton_sol_parastock.Active = false;
				entry_folio_servicio.Sensitive = false;
				entry_pid_paciente.Sensitive = false;
				entry_nombre_paciente.IsEditable = true;
				button_busca_paciente.Sensitive = true;
				entry_id_cirugia.Sensitive = true;
				entry_cirugia.Sensitive = true;
				entry_diagnostico.Sensitive = true;
			}else{
				entry_folio_servicio.Sensitive = true;
				entry_pid_paciente.Sensitive = true;
				entry_nombre_paciente.IsEditable = false;
				button_busca_paciente.Sensitive = true;
				entry_id_cirugia.Sensitive = false;
				entry_cirugia.Sensitive = false;
				entry_diagnostico.Sensitive = false;
			}
		}
	}
}