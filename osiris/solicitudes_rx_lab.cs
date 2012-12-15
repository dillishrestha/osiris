// created on 16/08/2010 at 06:10 p
//////////////////////////////////////////////////////////////////////
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. (Programacion) arcangeldoc@gmail.com
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

using GLib;
using System.Collections;

namespace osiris
{
	public class solicitudes_enfermeria
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		[Widget] Gtk.Window solicitar_examen_labrx = null;
		[Widget] Gtk.CheckButton checkbutton_nueva_solicitud = null;
		[Widget] Gtk.Entry entry_numero_solicitud = null;
		[Widget] Gtk.Button button_selec_solilabrx = null;
		[Widget] Gtk.Button button_enviar_solicitud_labrx = null;
		[Widget] Gtk.Button button_imprimir_solilabrx = null;
		[Widget] Gtk.RadioButton radiobutton_soli_interna = null;
		[Widget] Gtk.RadioButton radiobutton_soli_externa = null;		
		[Widget] Gtk.Entry entry_id_proveedor = null;
		[Widget] Gtk.Entry entry_nombre_proveedor = null;
		[Widget] Gtk.Button button_buscar_proveedor = null;
		[Widget] Gtk.Button button_busca_producto = null;
		[Widget] Gtk.Button button_quitar_examen = null;
		[Widget] Gtk.TreeView treeview_solicitud_labrx = null;
		[Widget] Gtk.TreeView treeview_estudios_solicitados = null;
		[Widget] Gtk.Entry entry_folio_servicio = null;
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_nombre_paciente = null;
		[Widget] Gtk.Entry entry_id_doctor = null;
		[Widget] Gtk.Entry entry_doctor = null;
		[Widget] Gtk.Entry entry_diagnostico = null;
		[Widget] Gtk.Entry entry_id_habitacion = null;
		[Widget] Gtk.Statusbar statusbar_solicitud_labrx = null;
		[Widget] Gtk.Entry entry_observacion = null;
		[Widget] Gtk.ComboBox combobox_turnos = null;
		
		/////// Ventana Busqueda de productos\\\\\\\\
		[Widget] Gtk.Window busca_producto = null;
		[Widget] Gtk.TreeView lista_de_producto;
		[Widget] Gtk.Entry entry_cantidad_aplicada;
		[Widget] Gtk.ComboBox combobox_tipo_admision;
		[Widget] Gtk.Entry entry_fecha_solicitud;
		[Widget] Gtk.Entry entry_hora_solicitud;
		[Widget] Gtk.Entry entry_folio_laboratorio;
		[Widget] Gtk.Label label_cantidad = null;
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		
		string agrupacion_lab_rx;
		string descripinternamiento;
		int id_tipoadmisiones;
		int id_tipopaciente;
		int idempresa_paciente;
		int idaseguradora_paciente;
		int PidPaciente;
		int folioservicio;
		string departament;
			//********    //nuevo lista de precios multiples//   *****************
		bool aplica_precios_aseguradoras = false;// Toma el valor de si se tiene creado la lista de precio en la tabla de Productos
		bool aplica_precios_empresas = false;	// Toma el valor de si se tiene creado la lista de precio en la tabla de Productos
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		
		string connectionString;
		string nombrebd;
		
		float valoriva;
		
		string turnos_tipocomida = "";
		
		string[] args_args = {""};
		string[] args_turnos = {"","MATUTINO/DIA","VESPERTINO/TARDE","NOCTURNO/NOCHE","PILOTO"};
		string[] args_tiempos_comida = {"","DESAYUNO","COMIDA","CENA","RECUPERACION"};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20};
		TreeStore treeViewEngineEstudios;
		TreeStore treeViewEngineEstudiosSoli;
		TreeStore treeViewEngineBusca2;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		ArrayList columns = new ArrayList ();
		Gtk.TreeIter iter;
		
		public solicitudes_enfermeria(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_,
		                              string departament_,int id_tipoadmisiones_,string agrupacion_lab_rx_,string descripinternamiento_,
		                              int id_tipopaciente_,int idempresa_paciente_,int idaseguradora_paciente_,
		                              int PidPaciente_,int folioservicio_,string nombrepaciente_,string iddoctor_,string nombremedico_,
		                              string diag_admision_,string habitacion_,bool estatus_procedimiento)
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			agrupacion_lab_rx = agrupacion_lab_rx_;
			descripinternamiento = descripinternamiento_;
			id_tipopaciente = id_tipopaciente_;
			id_tipoadmisiones = id_tipoadmisiones_;
			idempresa_paciente = idempresa_paciente_;
			idaseguradora_paciente = idaseguradora_paciente_;
			PidPaciente = PidPaciente_;
			folioservicio = folioservicio_;
			departament = departament_;
			valoriva = float.Parse(classpublic.ivaparaaplicar);
			
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "solicitar_examen_labrx", null);
			gxml.Autoconnect (this); 
			
			// show the window
			solicitar_examen_labrx.Show();
			solicitar_examen_labrx.Title = departament_+"/"+descripinternamiento;
			entry_id_proveedor.Sensitive = false;
			entry_nombre_proveedor.Sensitive = false;
			button_buscar_proveedor.Sensitive = false;
			entry_id_proveedor.Text = "1";
			entry_nombre_proveedor.Text = "SOLICITUD INTERNA";
			entry_folio_servicio.Text = folioservicio.ToString();
			entry_pid_paciente.Text = PidPaciente.ToString();
			entry_nombre_paciente.Text = nombrepaciente_;
			entry_id_doctor.Text = iddoctor_; 
			entry_doctor.Text = nombremedico_;
			entry_diagnostico.Text = diag_admision_;
			entry_id_habitacion.Text = habitacion_;
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			//radiobutton_soli_interna
			radiobutton_soli_externa.Clicked += new EventHandler(on_radiobutton_soli_externa_clicked);
			button_buscar_proveedor.Clicked += new EventHandler(on_button_buscar_proveedor_clicked);
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			button_enviar_solicitud_labrx.Clicked += new EventHandler(on_button_enviar_solicitud_labrx_clicked);
			checkbutton_nueva_solicitud.Clicked += new EventHandler(on_checkbutton_nueva_solicitud_clicked);
			button_imprimir_solilabrx.Clicked += new EventHandler(on_button_imprimir_solilabrx_clicked);				
			entry_id_proveedor.Sensitive = false;
			entry_nombre_proveedor.Sensitive = false;
			button_buscar_proveedor.Sensitive = false;
			button_enviar_solicitud_labrx.Sensitive = false;
			button_busca_producto.Sensitive = false;
			button_quitar_examen.Sensitive = false;
			//button_imprimir_solilabrx.Sensitive = false;
			if ((bool) estatus_procedimiento == false){
				checkbutton_nueva_solicitud.Sensitive = false;
			}
			
			crea_treeview_estudios();
			crea_treeview_estudios_solicitados();
			carga_estudios_solicitados();
			// expand all rows after the treeview widget has been realized
			treeview_estudios_solicitados.ExpandAll();
			entry_numero_solicitud.ModifyBase(StateType.Normal, new Gdk.Color(0,255,0)); // Color Amarillo
			
			statusbar_solicitud_labrx.Pop(0);
			statusbar_solicitud_labrx.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_solicitud_labrx.HasResizeGrip = false;
			if(agrupacion_lab_rx == "NUT"){
				llenado_combobox(0,"",combobox_turnos,"array","","","",args_tiempos_comida,args_id_array,"");
			}else{
				llenado_combobox(0,"",combobox_turnos,"array","","","",args_turnos,args_id_array,"");
			}
		}
		
		void on_button_imprimir_solilabrx_clicked(object sender, EventArgs args)
	    {
			new osiris.rpt_solicitud_labrx(departament, id_tipoadmisiones,agrupacion_lab_rx," AND folio_de_solicitud = '"+this.entry_numero_solicitud.Text+"'");
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
					treeViewEngineEstudios.Clear();
					ultimasolicitud = classpublic.lee_ultimonumero_registrado("osiris_his_solicitudes_labrx","folio_de_solicitud","WHERE id_tipo_admisiones2 = '"+id_tipoadmisiones.ToString().Trim()+"' ");
					entry_numero_solicitud.Text = ultimasolicitud;
					button_enviar_solicitud_labrx.Sensitive = true;
					button_busca_producto.Sensitive = true;
					button_quitar_examen.Sensitive = true;
					button_imprimir_solilabrx.Sensitive = false;
				}else{
					checkbutton_nueva_solicitud.Active = false;
					button_busca_producto.Sensitive = false;
					button_quitar_examen.Sensitive = false;
					button_imprimir_solilabrx.Sensitive = false;
				}
			}else{
				button_enviar_solicitud_labrx.Sensitive = false;
				button_busca_producto.Sensitive = false;
				button_quitar_examen.Sensitive = false;
				button_imprimir_solilabrx.Sensitive = false;
			}
		}				
		
		void on_radiobutton_soli_externa_clicked(object sender, EventArgs args)
		{
			if(radiobutton_soli_externa.Active == true){
				entry_id_proveedor.Sensitive = true;
				entry_nombre_proveedor.Sensitive = true;
				button_buscar_proveedor.Sensitive = true;
				entry_id_proveedor.Text = "1";
				entry_nombre_proveedor.Text = "";
			}else{
				entry_id_proveedor.Sensitive = false;
				entry_nombre_proveedor.Sensitive = false;
				button_buscar_proveedor.Sensitive = false;
				entry_id_proveedor.Text = "1";
				entry_nombre_proveedor.Text = "SOLICITUD INTERNA";
			}			
		}
		
		void llenado_combobox(int tipodellenado,string descrip_defaul,object obj,string sql_or_array,string query_SQL,string name_field_desc,string name_field_id,string[] args_array,int[] args_id_array,string name_field_id2)
		{			
			Gtk.ComboBox combobox_llenado = (Gtk.ComboBox) obj;
			//Gtk.ComboBox combobox_pos_neg = obj as Gtk.ComboBox;
			combobox_llenado.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_llenado.PackStart(cell, true);
			combobox_llenado.AddAttribute(cell,"text",0);	        
			ListStore store = new ListStore( typeof (string),typeof (int),typeof (int));
			combobox_llenado.Model = store;			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) descrip_defaul,0);
			}			
			if(sql_or_array == "array"){			
				for (int colum_field = 0; colum_field < args_array.Length; colum_field++){
					store.AppendValues (args_array[colum_field],args_id_array[colum_field],0);
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
						if(name_field_id2 == ""){
							store.AppendValues ((string) lector[ name_field_desc ], (int) lector[ name_field_id],0);
						}else{
							store.AppendValues ((string) lector[ name_field_desc ], (int) lector[ name_field_id],(int) lector[ name_field_id2]);
						}
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
				case "combobox_turnos":
					turnos_tipocomida = (string) combobox_turnos.Model.GetValue(iter,0);
					//idmunicipio = (int) combobox_municipios.Model.GetValue(iter,1);					
					break;
				}
			}
		}
		
		void on_button_buscar_proveedor_clicked(object sender, EventArgs args){
			object[] parametros_objetos = {entry_id_proveedor,entry_nombre_proveedor};
			string[] parametros_sql = {"SELECT descripcion_proveedor,direccion_proveedor,rfc_proveedor,curp_proveedor, "+
								"colonia_proveedor,municipio_proveedor,estado_proveedor,telefono1_proveedor, "+ 
								"telefono2_proveedor,celular_proveedor,rfc_proveedor, proveedor_activo, "+
								"id_proveedor,contacto1_proveedor,mail_proveedor,pagina_web_proveedor,"+
								"osiris_erp_proveedores.id_forma_de_pago,descripcion_forma_de_pago,fax_proveedor "+
								"FROM osiris_erp_proveedores, osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_proveedores.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago ",				
								"SELECT descripcion_proveedor,direccion_proveedor,rfc_proveedor,curp_proveedor, "+
								"colonia_proveedor,municipio_proveedor,estado_proveedor,telefono1_proveedor, "+ 
								"telefono2_proveedor,celular_proveedor,rfc_proveedor, proveedor_activo, "+
								"id_proveedor,contacto1_proveedor,mail_proveedor,pagina_web_proveedor, "+
								"osiris_erp_proveedores.id_forma_de_pago,descripcion_forma_de_pago,fax_proveedor "+
								"FROM osiris_erp_proveedores, osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_proveedores.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
								"AND descripcion_proveedor LIKE '%"};
			string[] parametros_string = {};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_proveedores_catalogo_producto"," ORDER BY descripcion_proveedor;","%' ",0);
		}
		
		void on_button_busca_producto_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "laboratorio.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			label_cantidad.Text = "Cantidad Solicitada";
			crea_treeview_busqueda("producto");
			busca_producto.SetPosition(WindowPosition.CenterOnParent);
			
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_entry_expresion;
			
			entry_fecha_solicitud.Text = DateTime.Now.ToString("yyyy-MM-dd");
			entry_hora_solicitud.Text = DateTime.Now.ToString("HH:mm:ss");
			entry_folio_laboratorio.Text = entry_numero_solicitud.Text;
			
						
			// Validando que sean solo numeros
			entry_cantidad_aplicada.KeyPressEvent += onKeyPressEvent;
			entry_folio_laboratorio.KeyPressEvent += onKeyPressEvent;
			
			//SE LLENA EL COMBO BOX	
			combobox_tipo_admision.Clear();
			CellRendererText cell2 = new CellRendererText();
			combobox_tipo_admision.PackStart(cell2, true);
			combobox_tipo_admision.AddAttribute(cell2,"text",0);
	        
			ListStore store2 = new ListStore( typeof (string), typeof (int));
			combobox_tipo_admision.Model = store2;
			
			// si es * se llena para hace solicitudes desde cualquier departamento, para que la classe
			// se pueda llamar desde otro modulo
			if(descripinternamiento == "*"){	        
		      	NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
	            // Verifica que la base de datos este conectada
	            try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
	               	comando.CommandText = "SELECT * FROM osiris_his_tipo_admisiones WHERE servicio_directo = 'false' "+
		           							"AND cuenta_mayor = 4000 "+
		               						" ORDER BY id_tipo_admisiones;";
					
					NpgsqlDataReader lector = comando.ExecuteReader ();
					store2.AppendValues ("", 0);
	               	while (lector.Read()){
						store2.AppendValues ((string) lector["descripcion_admisiones"], (int) lector["id_tipo_admisiones"]);
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
				conexion.Close ();
			}else{
				store2.AppendValues (descripinternamiento,id_tipoadmisiones);
			}
			TreeIter iter2;
			if (store2.GetIterFirst(out iter2)) {
				//Console.WriteLine(iter2);
				combobox_tipo_admision.SetActiveIter (iter2);
			}
			combobox_tipo_admision.Changed += new EventHandler (onComboBoxChanged_tipo_admision);	    
		}
		
		void onComboBoxChanged_tipo_admision(object sender, EventArgs args)
		{
	    	TreeIter iter;
			ComboBox combobox_tipo_admision = sender as ComboBox;			
			if (sender == null) { return; }
	  		if (combobox_tipo_admision.GetActiveIter (out iter)){
				id_tipoadmisiones = (int) combobox_tipo_admision.Model.GetValue(iter,1);
		    	descripinternamiento = (string) combobox_tipo_admision.Model.GetValue(iter,0);
		    	//Console.WriteLine(id_tipoadmisiones.ToString()+" "+descripinternamiento);
	     	}
		}
		
		void on_button_enviar_solicitud_labrx_clicked(object sender, EventArgs args)
		{
			TreeIter iter;
			string ultimasolicitud = classpublic.lee_ultimonumero_registrado("osiris_his_solicitudes_labrx","folio_de_solicitud","WHERE id_tipo_admisiones2 = '"+id_tipoadmisiones.ToString().Trim()+"' ");
			entry_numero_solicitud.Text = ultimasolicitud;
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de enviar esta Solicitud Numero :"+ultimasolicitud+"?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy(); 
			if (miResultado == ResponseType.Yes){
				if(turnos_tipocomida.Trim() != ""){
					if (this.treeViewEngineEstudios.GetIterFirst (out iter)){
						//for (int i = 0; i < treeViewEngineEstudios.NColumns; i++)
	        			//Console.WriteLine((string) this.treeview_solicitud_labrx.Model.GetValue(iter,i));  
											
						ultimasolicitud = classpublic.lee_ultimonumero_registrado("osiris_his_solicitudes_labrx","folio_de_solicitud","WHERE id_tipo_admisiones2 = '"+id_tipoadmisiones.ToString().Trim()+"' ");
						NpgsqlConnection conexion; 
						conexion = new NpgsqlConnection (connectionString+nombrebd);
						try{
							conexion.Open ();
							NpgsqlCommand comando; 
							comando = conexion.CreateCommand ();
							comando.CommandText =  "INSERT INTO osiris_his_solicitudes_labrx("+
													"folio_de_solicitud,"+
													"folio_de_servicio,"+
													"pid_paciente,"+									
													"id_producto,"+
													"precio_producto_publico,"+
													"costo_por_unidad,"+
													"cantidad_solicitada,"+
													//"cantidad_autorizada"+												
													"fechahora_solicitud,"+
													"id_quien_solicito,"+
													//"fechahora_autorizado,"+
													//"id_quien_autorizo,"+
													//"status,"+
													"id_proveedor,"+
													"id_tipo_admisiones,"+
													"folio_interno_labrx,"+
													"area_quien_solicita,"+
													"id_tipo_admisiones2," +
													"observaciones_solicitud," +
													"turno)"+
															" VALUES ('"+
													ultimasolicitud+"','"+
													folioservicio.ToString()+"','"+
													PidPaciente.ToString()+"','"+										
													(string) this.treeview_solicitud_labrx.Model.GetValue(iter,1)+"','"+
													(string) this.treeview_solicitud_labrx.Model.GetValue(iter,8)+"','"+
													(string) this.treeview_solicitud_labrx.Model.GetValue(iter,9)+"','"+
													(string) this.treeview_solicitud_labrx.Model.GetValue(iter,0)+"','"+
													//
													DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
													LoginEmpleado+"','"+
													//
													//
													//
													entry_id_proveedor.Text.Trim()+"','"+
													id_tipopaciente.ToString().Trim()+"','"+
													(string) this.treeview_solicitud_labrx.Model.GetValue(iter,10).ToString().Trim()+"','"+
													(string) descripinternamiento+"','"+
													id_tipoadmisiones.ToString().Trim()+"','"+
													entry_observacion.Text.Trim().ToUpper()+"','"+
													turnos_tipocomida+"');";
							//Console.WriteLine(comando.CommandText);
							comando.ExecuteNonQuery();
							comando.Dispose();					
							while (this.treeViewEngineEstudios.IterNext(ref iter)){
								comando.CommandText = "INSERT INTO osiris_his_solicitudes_labrx("+
													"folio_de_solicitud,"+
													"folio_de_servicio,"+
													"pid_paciente,"+									
													"id_producto,"+
													"precio_producto_publico,"+
													"costo_por_unidad,"+
													"cantidad_solicitada,"+
													//"cantidad_autorizada"+												
													"fechahora_solicitud,"+
													"id_quien_solicito,"+
													//"fechahora_autorizado,"+
													//"id_quien_autorizo,"+
													//"status,"+
													"id_proveedor,"+
													"id_tipo_admisiones,"+
													"folio_interno_labrx,"+
													"area_quien_solicita,"+
													"id_tipo_admisiones2," +
													"observaciones_solicitud," +
													"turno)"+
															" VALUES ('"+
													ultimasolicitud+"','"+
													folioservicio.ToString()+"','"+
													PidPaciente.ToString()+"','"+										
													(string) this.treeview_solicitud_labrx.Model.GetValue(iter,1)+"','"+
													(string) this.treeview_solicitud_labrx.Model.GetValue(iter,8)+"','"+
													(string) this.treeview_solicitud_labrx.Model.GetValue(iter,9)+"','"+
													(string) this.treeview_solicitud_labrx.Model.GetValue(iter,0)+"','"+
													//
													DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
													LoginEmpleado+"','"+
													//
													//
													//
													entry_id_proveedor.Text.Trim()+"','"+
													id_tipopaciente.ToString().Trim()+"','"+
													(string) this.treeview_solicitud_labrx.Model.GetValue(iter,10).ToString().Trim()+"','"+
													(string) descripinternamiento+"','"+
													id_tipoadmisiones.ToString().Trim()+"','"+
													entry_observacion.Text.Trim().ToUpper()+"','"+
													turnos_tipocomida+"');";
								//Console.WriteLine(comando.CommandText);
								comando.ExecuteNonQuery();
								comando.Dispose();			
							}
							checkbutton_nueva_solicitud.Active = false;
							button_imprimir_solilabrx.Sensitive = true;
							MessageDialog msgBoxError = new MessageDialog(MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
						                                              ButtonsType.Close, "La solicitud se envio con Exito...");
							msgBoxError.Run ();	msgBoxError.Destroy();
							carga_estudios_solicitados();
							// expand all rows after the treeview widget has been realized
							treeview_estudios_solicitados.ExpandAll();
						}catch (NpgsqlException ex){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
														MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();				msgBoxError.Destroy();
						}
						conexion.Close();
					}else{
						MessageDialog msgBoxError = new MessageDialog(MyWinError,DialogFlags.DestroyWithParent,MessageType.Error,
						                                              ButtonsType.Close, "NO puede crear una solicitud, no ha seleccionado ningun estudio, verifique...");
						msgBoxError.Run ();	msgBoxError.Destroy();
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog(MyWinError,DialogFlags.DestroyWithParent,MessageType.Error,
						                                              ButtonsType.Close, "NO puede crear una solicitud, Turno o Tiempo de Comida esta vacio, verifique...");
					msgBoxError.Run ();	msgBoxError.Destroy();
				}
			}
		}
		
		void crea_treeview_estudios()
		{
			treeViewEngineEstudios = new TreeStore(typeof(string),
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
			treeview_solicitud_labrx.Model = treeViewEngineEstudios;			
			treeview_solicitud_labrx.RulesHint = true;
			
			Gtk.TreeViewColumn col_request = new TreeViewColumn();		
			Gtk.CellRendererText cellrt0 = new Gtk.CellRendererText();
			col_request.Title = "Cant.Solicitado";
			col_request.PackStart(cellrt0, true);
			col_request.AddAttribute (cellrt0, "text", 0);
			col_request.Resizable = true;
			
			TreeViewColumn col_idproducto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_idproducto.Title = "ID Producto"; // titulo de la cabecera de la columna, si está visible
			col_idproducto.PackStart(cellr1, true);
			col_idproducto.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
			col_idproducto.Resizable = true;
						
			TreeViewColumn col_desc_producto = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_desc_producto.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
			col_desc_producto.PackStart(cellr2, true);
			col_desc_producto.AddAttribute (cellr2, "text", 2);    // la siguiente columna será 1 en vez de 1
			col_desc_producto.Resizable = true;
			
			Gtk.TreeViewColumn col_depart = new TreeViewColumn();		
			Gtk.CellRendererText cellrt3 = new Gtk.CellRendererText();
			col_depart.Title = "Departamento";
			col_depart.PackStart(cellrt3, true);
			col_depart.AddAttribute (cellrt3, "text", 3);
			col_depart.Resizable = true;
			
			Gtk.TreeViewColumn col_gabinete = new TreeViewColumn();		
			Gtk.CellRendererText cellrt4 = new Gtk.CellRendererText();
			col_gabinete.Title = "Gabinete";
			col_gabinete.PackStart(cellrt4, true);
			col_gabinete.AddAttribute (cellrt4, "text", 4);
			col_gabinete.Resizable = true;
			
			Gtk.TreeViewColumn col_fechasol = new TreeViewColumn();		
			Gtk.CellRendererText cellrt5 = new Gtk.CellRendererText();
			col_fechasol.Title = "Fecha Solicitud";
			col_fechasol.PackStart(cellrt5, true);
			col_fechasol.AddAttribute (cellrt5, "text", 5);
			col_fechasol.Resizable = true;
			
			Gtk.TreeViewColumn col_horasol = new TreeViewColumn();		
			Gtk.CellRendererText cellrt6 = new Gtk.CellRendererText();
			col_horasol.Title = "Hora Solicitud";
			col_horasol.PackStart(cellrt6, true);
			col_horasol.AddAttribute (cellrt6, "text", 6);
			col_horasol.Resizable = true;
			
			Gtk.TreeViewColumn col_quiensolicita = new TreeViewColumn();		
			Gtk.CellRendererText cellrt7 = new Gtk.CellRendererText();
			col_quiensolicita.Title = "Quien Solicita";
			col_quiensolicita.PackStart(cellrt7, true);
			col_quiensolicita.AddAttribute (cellrt7, "text", 7);
			col_quiensolicita.Resizable = true;
			
			Gtk.TreeViewColumn col_foliointerno = new TreeViewColumn();		
			Gtk.CellRendererText cellrt10 = new Gtk.CellRendererText();
			col_foliointerno.Title = "Folio "+agrupacion_lab_rx;
			col_foliointerno.PackStart(cellrt10, true);
			col_foliointerno.AddAttribute (cellrt10, "text", 10);
			col_foliointerno.Resizable = true;
			
			treeview_solicitud_labrx.AppendColumn(col_request); 		// 0
			treeview_solicitud_labrx.AppendColumn(col_idproducto);  	// 1
			treeview_solicitud_labrx.AppendColumn(col_desc_producto);	// 2
			treeview_solicitud_labrx.AppendColumn(col_depart);	 		// 3
			treeview_solicitud_labrx.AppendColumn(col_gabinete); 		// 4
			treeview_solicitud_labrx.AppendColumn(col_fechasol); 		// 5
			treeview_solicitud_labrx.AppendColumn(col_horasol); 		// 6
			treeview_solicitud_labrx.AppendColumn(col_quiensolicita); 	// 7
			treeview_solicitud_labrx.AppendColumn(col_foliointerno); 	// 8
		}
		
		void crea_treeview_estudios_solicitados()
		{
			Gtk.CellRendererText text;
			foreach (TreeViewColumn tvc in treeview_estudios_solicitados.Columns)
							treeview_estudios_solicitados.RemoveColumn(tvc);
			treeViewEngineEstudiosSoli = new TreeStore(typeof(string),
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
			treeview_estudios_solicitados.Model = treeViewEngineEstudiosSoli;
			treeview_estudios_solicitados.RulesHint = true;
			treeview_estudios_solicitados.Selection.Mode = SelectionMode.Multiple;
			
			// column for holiday names
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column0 = new TreeViewColumn("N° Soli./Estudio",text,"text",Column.solicitud_estudio);
			column0.Resizable = true;
			column0.SortColumnId = (int) Column.solicitud_estudio;
			treeview_estudios_solicitados.InsertColumn (column0, (int) Column.solicitud_estudio);
			
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column1 = new TreeViewColumn("Cant.Soli",text,"text",Column.cant_solicitado);
			column1.Resizable = true;
			column1.SortColumnId = (int) Column.cant_solicitado;
			treeview_estudios_solicitados.InsertColumn (column1, (int) Column.cant_solicitado);
			
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column2 = new TreeViewColumn("Fecha Solicitud",text,"text",Column.col_fechasol);
			column2.Resizable = true;
			column2.SortColumnId = (int) Column.col_fechasol;
			treeview_estudios_solicitados.InsertColumn (column2, (int) Column.col_fechasol);
			
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column3 = new TreeViewColumn("Gabinete",text,"text",Column.col_gabinete);
			column3.Resizable = true;
			column3.SortColumnId = (int) Column.col_gabinete;
			treeview_estudios_solicitados.InsertColumn (column3, (int) Column.col_gabinete);
			
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column4 = new TreeViewColumn("Quien Solicito",text,"text",Column.col_quiensolicito);
			column4.Resizable = true;
			column4.SortColumnId = (int) Column.col_quiensolicito;
			treeview_estudios_solicitados.InsertColumn (column4, (int) Column.col_quiensolicito);
		}
		
		enum Column
		{
			solicitud_estudio,
			cant_solicitado,
			col_fechasol,
			col_gabinete,
			col_quiensolicito
		}
		
		private void ExpandRows (object obj, EventArgs args)
		{
			TreeView treeView = obj as TreeView;
			treeView.ExpandAll ();
		}
		
		void carga_estudios_solicitados()
		{			
			int toma_numerosolicitud = 0;
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT osiris_his_solicitudes_labrx.folio_de_servicio,osiris_his_solicitudes_labrx.id_secuencia,osiris_his_solicitudes_labrx.pid_paciente AS pidpaciente,"+
										"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo,"+
										"osiris_his_solicitudes_labrx.id_producto,osiris_productos.descripcion_producto,folio_de_solicitud,id_tipo_admisiones2,"+
										"cantidad_solicitada,cantidad_autorizada,fechahora_solicitud,"+
										"to_char(osiris_his_solicitudes_labrx.fechahora_solicitud,'dd-mm-yyyy HH24:mi') AS fechahorasolicitud,"+
										"area_quien_solicita,id_quien_solicito,osiris_his_solicitudes_labrx.id_proveedor,descripcion_proveedor "+
										"FROM osiris_his_solicitudes_labrx,osiris_his_paciente,osiris_productos,osiris_erp_proveedores "+
										"WHERE osiris_his_solicitudes_labrx.pid_paciente = osiris_his_paciente.pid_paciente "+
										"AND osiris_his_solicitudes_labrx.id_producto = osiris_productos.id_producto "+
										"AND osiris_his_solicitudes_labrx.id_proveedor = osiris_erp_proveedores.id_proveedor "+
										"AND status = 'false' "+
										"AND eliminado = 'false' "+
										"AND folio_de_servicio = '"+folioservicio+"' "+
										"AND id_tipo_admisiones2 = '"+id_tipoadmisiones.ToString().Trim()+"';";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if((bool) lector.Read()){
					toma_numerosolicitud = (int) lector["folio_de_solicitud"];
					iter = treeViewEngineEstudiosSoli.AppendValues (lector["folio_de_solicitud"].ToString().Trim(),
								    null,
				                    lector["fechahorasolicitud"].ToString().Trim(),
				                    lector["descripcion_proveedor"].ToString().Trim(),
								    lector["id_quien_solicito"].ToString().Trim(),
				                    null);
				
					treeViewEngineEstudiosSoli.AppendValues (iter,
							   lector["descripcion_producto"].ToString().Trim(),
							    lector["cantidad_solicitada"].ToString().Trim(),
				                null,
				                null,
							    null,
							    null);
					while((bool) lector.Read()){
						if (toma_numerosolicitud != (int) lector["folio_de_solicitud"]){	
							iter = treeViewEngineEstudiosSoli.AppendValues (lector["folio_de_solicitud"].ToString().Trim(),
								    null,
				                    lector["fechahorasolicitud"].ToString().Trim(),
				                    lector["descripcion_proveedor"].ToString().Trim(),
								    lector["id_quien_solicito"].ToString().Trim(),
				                    null);
							toma_numerosolicitud = (int) lector["folio_de_solicitud"];
								
							treeViewEngineEstudiosSoli.AppendValues (iter,lector["descripcion_producto"].ToString().Trim(),
							    	lector["cantidad_solicitada"].ToString().Trim(),
				                	null,
				                	null,
							    	null,
							    	null);
						}else{
							treeViewEngineEstudiosSoli.AppendValues (iter,lector["descripcion_producto"].ToString().Trim(),
							    	lector["cantidad_solicitada"].ToString().Trim(),
				                	null,
				                	null,
							    	null,
							    	null);
						}
					}
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();						msgBoxError.Destroy();
			}
			conexion.Close();		
		}
		
		void crea_treeview_busqueda(string tipo_busqueda)
		{
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
				col_idproducto.Resizable = true;
			
				TreeViewColumn col_desc_producto = new TreeViewColumn();
				CellRendererText cellr1 = new CellRendererText();
				col_desc_producto.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
				col_desc_producto.PackStart(cellr1, true);
				col_desc_producto.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
				col_desc_producto.SortColumnId = (int) Column_prod.col_desc_producto;
				col_desc_producto.Resizable = true;
            	
				TreeViewColumn col_grupoprod = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_grupoprod.Title = "Grupo Producto";//Precio Producto
				col_grupoprod.PackStart(cellrt2, true);
				col_grupoprod.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
				col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
            	col_grupoprod.Resizable = true;
				
				TreeViewColumn col_grupo1prod = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_grupo1prod.Title = "Grupo1 Producto";//I.V.A.
				col_grupo1prod.PackStart(cellrt3, true);
				col_grupo1prod.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 2 en vez de 3
				col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
				col_grupo1prod.Resizable = true;
            
				TreeViewColumn col_grupo2prod = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_grupo2prod.Title = "Grupo2 Producto";//Total
				col_grupo2prod.PackStart(cellrt4, true);
				col_grupo2prod.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 3 en vez de 4
				col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;
				col_grupo2prod.Resizable = true;
            	
				lista_de_producto.AppendColumn(col_idproducto);  // 0
				lista_de_producto.AppendColumn(col_desc_producto); // 1
				lista_de_producto.AppendColumn(col_grupoprod);	//7
				lista_de_producto.AppendColumn(col_grupo1prod);	//8
				lista_de_producto.AppendColumn(col_grupo2prod);	//9							
			}
		}
		
		enum Column_prod
		{
			col_idproducto,
			col_desc_producto,
			col_grupoprod,
			col_grupo1prod,
			col_grupo2prod
		}
		
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			int validacantidad = 0;
			TreeModel model;	TreeIter iterSelected;
			if(entry_cantidad_aplicada.Text != "" && entry_cantidad_aplicada.Text != "0"){
				if ((float) float.Parse(entry_cantidad_aplicada.Text) > 0){
					if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
						treeViewEngineEstudios.AppendValues((string) entry_cantidad_aplicada.Text.ToString().Trim(),
													(string) lista_de_producto.Model.GetValue (iterSelected,0),
													(string) lista_de_producto.Model.GetValue (iterSelected,1),
				                                    descripinternamiento,
				                                    entry_nombre_proveedor.Text,
				                                    entry_fecha_solicitud.Text,
				                                    entry_hora_solicitud.Text,
				                                    LoginEmpleado,
				                                    (string) lista_de_producto.Model.GetValue (iterSelected,5),
				                                    (string) lista_de_producto.Model.GetValue (iterSelected,10),
				                                    entry_folio_laboratorio.Text.Trim());
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, ButtonsType.Close, "La cantidad solicitada NO \n"+
												"puede quedar vacia, intente de nuevo");
								msgBoxError.Run ();					msgBoxError.Destroy();
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, ButtonsType.Close, "La cantidad solicitada NO \n"+
												"puede quedar vacia, intente de nuevo");
								msgBoxError.Run ();					msgBoxError.Destroy();
			}
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
			string query_lab_rx = "";
			float tomaprecio;
			float calculodeiva;
			float preciomasiva;
			float tomadescue;
			float preciocondesc;
			
			if(agrupacion_lab_rx == "LAB"){
				query_lab_rx = "AND osiris_grupo_producto.agrupacion2 = 'LAB' ";
			} 			
			if(agrupacion_lab_rx == "IMG"){
				query_lab_rx = "AND osiris_grupo_producto.agrupacion3 = 'IMG' ";
			}
			if(agrupacion_lab_rx == "VIS"){
				query_lab_rx = "AND osiris_grupo_producto.agrupacion5 = 'VIS' ";
			}
			if(agrupacion_lab_rx == "NUT"){
				query_lab_rx = "AND osiris_grupo_producto.agrupacion7 = 'NUT' ";
			}
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
					precio_a_tomar = "precio_producto_"+id_tipopaciente.ToString().Trim()+idaseguradora_paciente.ToString().Trim();
				
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
			//Console.WriteLine(precio_a_tomar);			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();				
				comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
						"osiris_productos.descripcion_producto,"+
						"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
						//"to_char(precio_producto_publico1,'99999999.99') AS preciopublico1,"+
						"to_char("+precio_a_tomar+",'99999999.99') AS preciopublico_cliente,"+
						"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,"+
						"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,"+
						"to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
						"to_char(porcentage_ganancia,'99999.99') AS porcentageutilidad,"+
						"to_char(costo_producto,'999999999.99') AS costoproducto "+
						"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
						"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
						"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
						"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
						query_lab_rx+
						"AND osiris_productos.cobro_activo = 'true' "+
						"AND osiris_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_producto;";
					
				//Console.WriteLine(comando.CommandText);				
				NpgsqlDataReader lector = comando.ExecuteReader ();										
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
					tomadescue = float.Parse((string) lector["porcentagesdesc"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					preciocondesc = tomaprecio;
					if ((bool) lector["aplicar_iva"]){
						calculodeiva = (tomaprecio * valoriva)/100;
					}
					if ((bool) lector["aplica_descuento"]){
						preciocondesc = tomaprecio-((tomaprecio*tomadescue)/100);
					}
					preciomasiva = tomaprecio + calculodeiva; 
					treeViewEngineBusca2.AppendValues (//TreeIter iter = 
											(string) lector["codProducto"],//0
											(string) lector["descripcion_producto"],//1
											(string) lector["descripcion_grupo_producto"],//2
											(string) lector["descripcion_grupo1_producto"],//3
											(string) lector["descripcion_grupo2_producto"],//4
											tomaprecio.ToString("F").PadLeft(10),//2-5
											calculodeiva.ToString("F").PadLeft(10),//3-6
											preciomasiva.ToString("F").PadLeft(10),//4-7
											(string) lector["porcentagesdesc"],//8
											preciocondesc.ToString("F").PadLeft(10),//9
											(string) lector["costoproductounitario"],//10
											(string) lector["porcentageutilidad"],//11
											(string) lector["costoproducto"]);//12
					
				}
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			}
			conexion.Close ();
		}
		
		// carga de producto
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
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}
		
		void on_cierraventanas_clicked(object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// 
	/// Esta classe es la que se encarga de llenar las solictudes de 
	/// Laboratorio y Rayos X
	/// 
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class solicitudes_rx_lab
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		[Widget] Gtk.Window solicitudes_examenes_labrx = null;
		[Widget] Gtk.Button button_consultar = null;
 		[Widget] Gtk.RadioButton radiobutton_estud_carg = null;
		[Widget] Gtk.RadioButton radiobutton_estud_solic = null;
		[Widget] Gtk.Notebook notebook1 = null;
		[Widget] Gtk.Entry entry_fecha_inicio = null;
		[Widget] Gtk.Entry entry_fecha_termino = null;
		[Widget] Gtk.CheckButton checkbutton_rango_fecha = null;
		[Widget] Gtk.CheckButton checkbutton_filtro_paciente = null;
		[Widget] Gtk.Entry entry_nro_expediente = null;
		[Widget] Gtk.Entry entry_npmbre_paciente = null;
		[Widget] Gtk.Button button_busca_paciente = null;
				
		// Tab number one application form request LAB RX
		[Widget] Gtk.TreeView treeview_lista_solicitados = null;
		[Widget] Gtk.Button button_cargar_examen = null;
		[Widget] Gtk.ToggleButton togglebutton_por_paciente = null;
		
		// Tab number two Charges to patients
		[Widget] Gtk.TreeView treeview_lista_cargosvalid = null;
		[Widget] Gtk.Button button_validar_examen = null;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
				
		string connectionString;
		string nombrebd;
		
		int idtipoadmisiones;
		
		ArrayList columns = new ArrayList ();
		Gtk.TreeStore treeViewEnginesolicitados;
		
		Gtk.TreeIter iter;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		class_conexion conexion_a_DB = new class_conexion();
		
		public solicitudes_rx_lab(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_,string departament_,int idtipoadmisiones_)
		{			
			Glade.XML gxml = new Glade.XML (null, "imagenologia.glade", "solicitudes_examenes_labrx", null);
			gxml.Autoconnect (this);	        
			// Muestra ventana de Glade
			solicitudes_examenes_labrx.Show();
			solicitudes_examenes_labrx.Title = departament_;
			idtipoadmisiones = idtipoadmisiones_;
			
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = conexion_a_DB._nombrebd;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			radiobutton_estud_carg.Clicked += new EventHandler(on_changetab_clicked);
			radiobutton_estud_solic.Clicked += new EventHandler(on_changetab_clicked);
			button_consultar.Clicked += new EventHandler(on_button_consultar_clicked);
			button_cargar_examen.Clicked += new EventHandler(on_button_cargar_examen_clicked);
			button_consultar.Clicked += new EventHandler(on_button_consultar_clicked);
			entry_fecha_inicio.Text = DateTime.Now.ToString("yyyy-MM-dd");
			entry_fecha_termino.Text = DateTime.Now.ToString("yyyy-MM-dd");
			togglebutton_por_paciente.Clicked += new EventHandler(on_togglebutton_por_paciente_clicked);
			checkbutton_rango_fecha.Clicked  += new EventHandler(on_checkbutton_rango_fecha_clicked);
			checkbutton_filtro_paciente.Clicked += new EventHandler(on_checkbutton_filtro_paciente_clicked);
			//checkbutton_rango_fecha.Active = true;
			entry_fecha_inicio.Sensitive = (bool) checkbutton_rango_fecha.Active;
			entry_fecha_termino.Sensitive = (bool) checkbutton_rango_fecha.Active;
			entry_nro_expediente.Sensitive = (bool) checkbutton_filtro_paciente.Active;
			entry_npmbre_paciente.Sensitive = (bool) checkbutton_filtro_paciente.Active;
			button_busca_paciente.Sensitive = (bool) checkbutton_filtro_paciente.Active;
			create_treeview_solicitudes((bool) togglebutton_por_paciente.Active);
			create_treeview_cargados();
		}
		
		void on_button_consultar_clicked(object sender, EventArgs args)
		{
			if(radiobutton_estud_carg.Active == true){
				notebook1.CurrentPage = 1;
			}
			if(radiobutton_estud_solic.Active == true){
				notebook1.CurrentPage = 0;
				create_treeview_solicitudes((bool) togglebutton_por_paciente.Active);
			}
		}
		
		void on_togglebutton_por_paciente_clicked(object sender, EventArgs args)
		{
			create_treeview_solicitudes((bool) togglebutton_por_paciente.Active);
		}
		
		void on_changetab_clicked(object sender, EventArgs args)
		{
			//Console.WriteLine(radiobutton_seltab.Active.ToString());
			Gtk.RadioButton radiobutton_seltab = (Gtk.RadioButton) sender;
			
			if(radiobutton_seltab.Name.ToString() ==  "radiobutton_estud_carg"){
				notebook1.CurrentPage = 1;
			}
			if(radiobutton_seltab.Name.ToString() ==  "radiobutton_estud_solic"){
				notebook1.CurrentPage = 0;
			}
		}
		
		void on_checkbutton_rango_fecha_clicked(object sender, EventArgs args)
		{
			entry_fecha_inicio.Sensitive = (bool) checkbutton_rango_fecha.Active;
			entry_fecha_termino.Sensitive = (bool) checkbutton_rango_fecha.Active;
		}
		
		void on_checkbutton_filtro_paciente_clicked(object sender, EventArgs args)
		{
			entry_nro_expediente.Sensitive = (bool) checkbutton_filtro_paciente.Active;
			entry_npmbre_paciente.Sensitive = (bool) checkbutton_filtro_paciente.Active;
			button_busca_paciente.Sensitive = (bool) checkbutton_filtro_paciente.Active;
		}
		
		void on_button_cargar_examen_clicked(object sender, EventArgs args)
		{
			TreeIter iter;
 			//if (this.treeview_lista_solicitados.Selection.GetSelected(out model, out iterSelected)){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de cargar los Estudios Seleccionados");
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
						// actualizan la lista de producto por numero de solicitud
						if(togglebutton_por_paciente.Active == false){
							//Console.WriteLine("Treeview es por estudios");							
							if (treeViewEnginesolicitados.GetIterFirst(out iter)){
								if ((bool)treeview_lista_solicitados.Model.GetValue (iter,0) == true){
								
								
									//Console.WriteLine(treeview_lista_solicitados.Model.GetValue(iter,1)+ " - "+treeview_lista_solicitados.Model.GetValue(iter,13));
									//comando.CommandText = "INSERT INTO osiris_erp_cobros_deta("+
									//						"id_producto,"+
				 					//						"folio_de_servicio,"+
				 					//						"pid_paciente,"+
									//						") VALUES ('"+
				 					//						double.Parse((string) treeview_lista_solicitados.Model.GetValue(iter,2))+//id_producto		
									//						"');";
									/*
									comando.CommandText = "INSERT INTO osiris_erp_cobros_deta("+
			 														"id_producto,"+
			 														"folio_de_servicio,"+
			 														"pid_paciente,"+
			 														"cantidad_aplicada,"+
			 														"id_tipo_admisiones,"+
			 														"precio_producto, "+
			 														//"precio_por_cantidad,"+
			 														"iva_producto,"+
			 														"precio_costo_unitario,"+
			 														"porcentage_utilidad,"+
			 														"porcentage_descuento,"+
			 														"id_empleado,"+
			 														"fechahora_creacion,"+
			 														"porcentage_iva,"+
			 														"id_almacen,"+
			 														"precio_costo) "+
			 														"VALUES ('"+
			 														double.Parse((string) treeview_lista_solicitados.Model.GetValue(iter,2))+"','"+//id_producto
			 														folioservicio+"','"+//folio_de_servicio
			 														int.Parse((string)entry_pid_paciente.Text)+"','"+//pid_paciente
			 														(float) treeview_lista_solicitados.Model.GetValue(iter,1)+"','"+//cantidad_aplicada
			 														(int)treeview_lista_solicitados.Model.GetValue(iter,13)+"','"+//id_tipo_admisiones
			 														double.Parse((string)treeview_lista_solicitados.Model.GetValue(iter,6))+"','"+//precio_producto
			 														//double.Parse((string)lista_de_servicios.Model.GetValue(iter,7))+"','"+//precio_por_cantidad
			 														double.Parse((string)treeview_lista_solicitados.Model.GetValue(iter,8))+"','"+//iva_producto
			 														double.Parse((string)treeview_lista_solicitados.Model.GetValue(iter,15))+"','"+//precio_costo_unitario
			 														float.Parse((string)treeview_lista_solicitados.Model.GetValue(iter,16))+"','"+//porcentage_utilidad
			 														float.Parse((string) treeview_lista_solicitados.Model.GetValue(iter,10))+"','"+//porcentage_descuento
			 														LoginEmpleado+"','"+//id_empleado
			 														DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+//fechahora_creacion
			 														valoriva+"','"+//porcentage_iva
			 														this.idsubalmacen.ToString().Trim()+"','"+
			 														float.Parse((string)treeview_lista_solicitados.Model.GetValue(iter,17))+//precio_costo
			 														"');";*/
								
								
								}								
								while (treeViewEnginesolicitados.IterNext(ref iter)){									
									if ((bool)treeview_lista_solicitados.Model.GetValue (iter,0) == true){
										//Console.WriteLine(treeview_lista_solicitados.Model.GetValue(iter,1)+ " - "+treeview_lista_solicitados.Model.GetValue(iter,13));
										//comando.CommandText = "INSERT INTO osiris_erp_cobros_deta("+
										//						"id_producto,"+
					 					//						"folio_de_servicio,"+
					 					//						"pid_paciente,"+
										//						") VALUES ('"+
					 					//						double.Parse((string) treeview_lista_solicitados.Model.GetValue(iter,2))+//id_producto		
										//						"');";
										/*
										comando.CommandText = "INSERT INTO osiris_erp_cobros_deta("+
			 														"id_producto,"+
			 														"folio_de_servicio,"+
			 														"pid_paciente,"+
			 														"cantidad_aplicada,"+
			 														"id_tipo_admisiones,"+
			 														"precio_producto, "+
			 														//"precio_por_cantidad,"+
			 														"iva_producto,"+
			 														"precio_costo_unitario,"+
			 														"porcentage_utilidad,"+
			 														"porcentage_descuento,"+
			 														"id_empleado,"+
			 														"fechahora_creacion,"+
			 														"porcentage_iva,"+
			 														"id_almacen,"+
			 														"precio_costo) "+
			 														"VALUES ('"+
			 														double.Parse((string) treeview_lista_solicitados.Model.GetValue(iter,2))+"','"+//id_producto
			 														folioservicio+"','"+//folio_de_servicio
			 														int.Parse((string)entry_pid_paciente.Text)+"','"+//pid_paciente
			 														(float) treeview_lista_solicitados.Model.GetValue(iter,1)+"','"+//cantidad_aplicada
			 														(int)treeview_lista_solicitados.Model.GetValue(iter,13)+"','"+//id_tipo_admisiones
			 														double.Parse((string)treeview_lista_solicitados.Model.GetValue(iter,6))+"','"+//precio_producto
			 														//double.Parse((string)lista_de_servicios.Model.GetValue(iter,7))+"','"+//precio_por_cantidad
			 														double.Parse((string)treeview_lista_solicitados.Model.GetValue(iter,8))+"','"+//iva_producto
			 														double.Parse((string)treeview_lista_solicitados.Model.GetValue(iter,15))+"','"+//precio_costo_unitario
			 														float.Parse((string)treeview_lista_solicitados.Model.GetValue(iter,16))+"','"+//porcentage_utilidad
			 														float.Parse((string) treeview_lista_solicitados.Model.GetValue(iter,10))+"','"+//porcentage_descuento
			 														LoginEmpleado+"','"+//id_empleado
			 														DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+//fechahora_creacion
			 														valoriva+"','"+//porcentage_iva
			 														this.idsubalmacen.ToString().Trim()+"','"+
			 														float.Parse((string)treeview_lista_solicitados.Model.GetValue(iter,17))+//precio_costo
			 														"');";*/
									}								
									comando.ExecuteNonQuery();
									comando.Dispose();
									/*
									comando.CommandText = "UPDATE osiris_erp_cobros_enca "+
										"SET bloqueo_de_folio = 'true' ,"+
										"historial_de_bloqueo = historial_de_bloqueo ||'"+LoginEmpleado+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n' "+
				 						"WHERE  folio_de_servicio =  '"+this.folioservicio+"';";
									comando.ExecuteNonQuery();
					        		comando.Dispose();
					        		*/
								}
							}						
						}else{
							// View for patients
							// Es por la lista de pacientes
							
						}
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
															MessageType.Error, 
															ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();				conexion.Close();		
					}
					conexion.Close();
				}
			//}else{
			//	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
			//				MessageType.Error, ButtonsType.Close,"No hay solicitudes para cargar...","Solicitudes");
			//	msgBoxError.Run ();						msgBoxError.Destroy();
			//}
		}
			
		void create_treeview_solicitudes(bool tipo_treeview)
		{
			Gtk.CellRendererText text;
			Gtk.CellRendererToggle toggle;			
			// Erase all columns
			foreach (TreeViewColumn tvc in this.treeview_lista_solicitados.Columns)
			this.treeview_lista_solicitados.RemoveColumn(tvc);
			// create treeview List the request
			if(tipo_treeview == false){
				// por numero de solicitud
				treeViewEnginesolicitados = new TreeStore(typeof(bool),typeof(string),typeof(string),typeof(string),typeof(string),
														typeof(string),typeof(string),typeof(string),typeof(string),
				                                        typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
				                                        typeof(string),typeof(string),typeof(string));
				treeview_lista_solicitados.Model = treeViewEnginesolicitados;
				treeview_lista_solicitados.RulesHint = true;
				
				Gtk.TreeViewColumn col_request0 = new TreeViewColumn(); 	Gtk.CellRendererToggle cellrt0 = new CellRendererToggle();		
				col_request0.Title = "Seleccion";
				col_request0.PackStart(cellrt0, true);
				col_request0.AddAttribute (cellrt0, "active", 0);
				col_request0.Resizable = true;
				col_request0.SortColumnId = (int) coldatos_request.col_request0;
				cellrt0.Toggled += selecciona_examen; 
				
				Gtk.TreeViewColumn col_request1 = new TreeViewColumn();		Gtk.CellRendererText cellrt1 = new Gtk.CellRendererText();		
				col_request1.Title = "N° Atencion";
				col_request1.PackStart(cellrt1, true);
				col_request1.AddAttribute (cellrt1, "text", 1);
				col_request1.Resizable = true;
				col_request1.SortColumnId = (int) coldatos_request.col_request1;
				
				Gtk.TreeViewColumn col_request2 = new TreeViewColumn();		Gtk.CellRendererText cellrt2 = new Gtk.CellRendererText();		
				col_request2.Title = "Expediente";
				col_request2.PackStart(cellrt2, true);
				col_request2.AddAttribute (cellrt2, "text", 2);
				col_request2.Resizable = true;
				col_request2.SortColumnId = (int) coldatos_request.col_request2;
				
				Gtk.TreeViewColumn col_request3 = new TreeViewColumn();		Gtk.CellRendererText cellrt3 = new Gtk.CellRendererText();		
				col_request3.Title = "Paciente";
				col_request3.PackStart(cellrt3, true);
				col_request3.AddAttribute (cellrt3, "text", 3);
				col_request3.Resizable = true;
				col_request3.SortColumnId = (int) coldatos_request.col_request3;
				
				Gtk.TreeViewColumn col_request4 = new TreeViewColumn();		Gtk.CellRendererText cellrt4 = new Gtk.CellRendererText();		
				col_request4.Title = "N° Solic.";
				col_request4.PackStart(cellrt4, true);
				col_request4.AddAttribute (cellrt4, "text", 4);
				col_request4.Resizable = true;
				col_request4.SortColumnId = (int) coldatos_request.col_request4;
				
				Gtk.TreeViewColumn col_request5 = new TreeViewColumn();		Gtk.CellRendererText cellrt5 = new Gtk.CellRendererText();		
				col_request5.Title = "Codigo";
				col_request5.PackStart(cellrt5, true);
				col_request5.AddAttribute (cellrt5, "text", 5);
				col_request5.Resizable = true;
				col_request5.SortColumnId = (int) coldatos_request.col_request5;
				
				Gtk.TreeViewColumn col_request6 = new TreeViewColumn();		Gtk.CellRendererText cellrt6 = new Gtk.CellRendererText();		
				col_request6.Title = "Estudio Solicitado";
				col_request6.PackStart(cellrt6, true);
				col_request6.AddAttribute (cellrt6, "text", 6);
				col_request6.Resizable = true;
				col_request6.SortColumnId = (int) coldatos_request.col_request6;
				
				Gtk.TreeViewColumn col_request7 = new TreeViewColumn();		Gtk.CellRendererText cellrt7 = new Gtk.CellRendererText();		
				col_request7.Title = "Cant.Solicitado";
				col_request7.PackStart(cellrt7, true);
				col_request7.AddAttribute (cellrt7, "text", 7);
				col_request7.Resizable = true;
				col_request7.SortColumnId = (int) coldatos_request.col_request7;
				
				Gtk.TreeViewColumn col_request8 = new TreeViewColumn();		Gtk.CellRendererText cellrt8 = new Gtk.CellRendererText();		
				col_request8.Title = "Cant.Autorizada";
				col_request8.PackStart(cellrt8, true);
				col_request8.AddAttribute (cellrt8, "text", 8);
				col_request8.Resizable = true;
				col_request8.SortColumnId = (int) coldatos_request.col_request8;
				cellrt8.Editable = true;
				cellrt8.Edited += NumberCellEdited_Autorizado;
				
				Gtk.TreeViewColumn col_request9 = new TreeViewColumn();		Gtk.CellRendererText cellrt9 = new Gtk.CellRendererText();		
				col_request9.Title = "Fech.Hora Soli.";
				col_request9.PackStart(cellrt9, true);
				col_request9.AddAttribute (cellrt9, "text", 9);
				col_request9.Resizable = true;
				col_request9.SortColumnId = (int) coldatos_request.col_request9;
				
				Gtk.TreeViewColumn col_request10 = new TreeViewColumn();		Gtk.CellRendererText cellrt10 = new Gtk.CellRendererText();		
				col_request10.Title = "Area quien solicta";
				col_request10.PackStart(cellrt10, true);
				col_request10.AddAttribute (cellrt10, "text", 10);
				col_request10.Resizable = true;
				col_request10.SortColumnId = (int) coldatos_request.col_request10;
				
				Gtk.TreeViewColumn col_request11 = new TreeViewColumn();		Gtk.CellRendererText cellrt11 = new Gtk.CellRendererText();		
				col_request11.Title = "Quien Solicito";
				col_request11.PackStart(cellrt11, true);
				col_request11.AddAttribute (cellrt11, "text", 11);
				col_request11.Resizable = true;
				col_request11.SortColumnId = (int) coldatos_request.col_request11;
				
				Gtk.TreeViewColumn col_request12 = new TreeViewColumn();		Gtk.CellRendererText cellrt12 = new Gtk.CellRendererText();		
				col_request12.Title = "Gabinete";
				col_request12.PackStart(cellrt12, true);
				col_request12.AddAttribute (cellrt12, "text", 12);
				col_request12.Resizable = true;
				col_request12.SortColumnId = (int) coldatos_request.col_request12;
							
				treeview_lista_solicitados.AppendColumn(col_request0);
				treeview_lista_solicitados.AppendColumn(col_request1);
				treeview_lista_solicitados.AppendColumn(col_request2);
				treeview_lista_solicitados.AppendColumn(col_request3);
				treeview_lista_solicitados.AppendColumn(col_request4);
				treeview_lista_solicitados.AppendColumn(col_request5);
				treeview_lista_solicitados.AppendColumn(col_request6);
				treeview_lista_solicitados.AppendColumn(col_request7);
				treeview_lista_solicitados.AppendColumn(col_request8);
				treeview_lista_solicitados.AppendColumn(col_request9);
				treeview_lista_solicitados.AppendColumn(col_request10);
				treeview_lista_solicitados.AppendColumn(col_request11);
				treeview_lista_solicitados.AppendColumn(col_request12);
				
				llenado_treeview_solicitudes((bool) togglebutton_por_paciente.Active,treeViewEnginesolicitados);
				
			}else{
				treeViewEnginesolicitados = new TreeStore(typeof(string),typeof(bool),typeof(string),typeof(string),typeof(string),
				                                          typeof(bool),typeof(bool));
				treeview_lista_solicitados.Model = treeViewEnginesolicitados;
				treeview_lista_solicitados.RulesHint = true;
				treeview_lista_solicitados.Selection.Mode = SelectionMode.Multiple;
				
				// column for holiday names
				text = new CellRendererText ();
				text.Xalign = 0.0f;
			 	columns.Add (text);
				TreeViewColumn column0 = new TreeViewColumn("Paciente/Estudio", text,
								    "text", Column.paciente_estudio);
				column0.Resizable = true;
				column0.SortColumnId = (int) Column.paciente_estudio;
				treeview_lista_solicitados.InsertColumn (column0, (int) Column.paciente_estudio);
												
				toggle = new CellRendererToggle ();
				toggle.Xalign = 0.0f;
				columns.Add (toggle);
				toggle.Toggled += new ToggledHandler (ItemToggled);
				TreeViewColumn column1 = new TreeViewColumn ("Seleccion", toggle,
							     "active", (int) Column.seleccion,
							     "visible", (int) Column.Visible,
							     "activatable", (int) Column.World);
				column1.Sizing = TreeViewColumnSizing.Fixed;
				column1.FixedWidth = 65;
				column1.Clickable = true;
				column1.Resizable = true;
				column1.SortColumnId = (int) Column.seleccion;
				treeview_lista_solicitados.InsertColumn (column1, (int) Column.seleccion);
				
				text = new CellRendererText ();
				text.Xalign = 0.0f;
			 	columns.Add (text);
				TreeViewColumn column2 = new TreeViewColumn("Nº Solicitud", text,
								    "text", Column.nro_solicitud);
				column2.Resizable = true;
				column2.SortColumnId = (int) Column.nro_solicitud;
				treeview_lista_solicitados.InsertColumn (column2, (int) Column.nro_solicitud);
				
				text = new CellRendererText ();
				text.Xalign = 0.0f;
			 	columns.Add (text);
				TreeViewColumn column3 = new TreeViewColumn("Cant.Solicitado", text,
								    "text", Column.cantsolicitada);
				column3.Resizable = true;
				column3.SortColumnId = (int) Column.cantsolicitada;
				treeview_lista_solicitados.InsertColumn (column3, (int) Column.cantsolicitada);				
				
				text = new CellRendererText ();
				text.Xalign = 0.0f;
			 	columns.Add (text);
				TreeViewColumn column4 = new TreeViewColumn("Cant.Autorizada", text,
								    "text", Column.cantautorizada);
				column4.Resizable = true;
				column4.SortColumnId = (int) Column.cantautorizada;
				text.Editable = true;
				text.Edited += NumberCellEdited_Autorizado_1;
				treeview_lista_solicitados.InsertColumn (column4, (int) Column.cantautorizada);
				
				llenado_treeview_solicitudes((bool) togglebutton_por_paciente.Active,treeViewEnginesolicitados);			
			}
		}
		
		enum coldatos_request
		{	
			col_request0,
			col_request1,
			col_request2,
			col_request3,
			col_request4,
			col_request5,
			col_request6,
			col_request7,
			col_request8,
			col_request9,
			col_request10,
			col_request11,
			col_request12,
		}
		
		enum Column
		{
			paciente_estudio,
			seleccion,
			nro_solicitud,
			cantsolicitada,
			cantautorizada,
			Visible,
			World,			
		}
		
		void llenado_treeview_solicitudes(bool tipo_treeview, object obj)
		{
			Gtk.TreeStore treeViewEnginesolicitados = (Gtk.TreeStore) obj;
			string queryorder = "";
			int toma_pidpaciente;
			
			// llenado por lista
			if(tipo_treeview == false){
				queryorder = "ORDER BY folio_de_solicitud DESC;";
			}
			// llenado por paciente
			if(tipo_treeview == true){
				queryorder = "ORDER BY osiris_his_solicitudes_labrx.pid_paciente ASC;";
			}
			
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT osiris_his_solicitudes_labrx.folio_de_servicio,osiris_his_solicitudes_labrx.id_secuencia,osiris_his_solicitudes_labrx.pid_paciente AS pidpaciente,"+
										"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo,"+
										"osiris_his_solicitudes_labrx.id_producto,osiris_productos.descripcion_producto,folio_de_solicitud,"+
										"cantidad_solicitada,cantidad_autorizada,fechahora_solicitud,"+
										"to_char(osiris_his_solicitudes_labrx.fechahora_solicitud,'dd-mm-yyyy HH24:mi') AS fechahorasolicitud,"+
										"area_quien_solicita,id_quien_solicito,osiris_his_solicitudes_labrx.id_proveedor,descripcion_proveedor "+
										"FROM osiris_his_solicitudes_labrx,osiris_his_paciente,osiris_productos,osiris_erp_proveedores "+
										"WHERE osiris_his_solicitudes_labrx.pid_paciente = osiris_his_paciente.pid_paciente "+
										"AND osiris_his_solicitudes_labrx.id_producto = osiris_productos.id_producto "+
										"AND osiris_his_solicitudes_labrx.id_proveedor = osiris_erp_proveedores.id_proveedor "+
										"AND status = 'false' "+
										"AND eliminado = 'false' "+
										"AND id_tipo_admisiones2 = '"+idtipoadmisiones.ToString().Trim()+"' "+queryorder;
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if((bool) lector.Read()){
					toma_pidpaciente = (int) lector["pidpaciente"];
					// llenado de lista de solicitudes
					if(tipo_treeview == false){
						treeViewEnginesolicitados.AppendValues(false,
						                                       lector["folio_de_servicio"].ToString().Trim(),
						                                       lector["pidpaciente"].ToString().Trim(),
						                                       lector["nombre_completo"].ToString().Trim(),
						                                       lector["folio_de_solicitud"].ToString().Trim(),
						                                       lector["id_producto"].ToString().Trim(),
						                                       lector["descripcion_producto"].ToString().Trim(),
						                                       float.Parse(lector["cantidad_solicitada"].ToString().Trim()).ToString("F"),
						                                       float.Parse(lector["cantidad_autorizada"].ToString().Trim()).ToString("F"),
						                                       lector["fechahorasolicitud"].ToString().Trim(),
						                                       lector["area_quien_solicita"].ToString().Trim(),
						                                       lector["id_quien_solicito"].ToString().Trim(),
						                                       lector["descripcion_proveedor"].ToString().Trim(),
						                                       lector["id_secuencia"].ToString().Trim());
					}					
					// llenado por paciente
					if(tipo_treeview == true){
						iter = treeViewEnginesolicitados.AppendValues (lector["nombre_completo"].ToString().Trim(),
								    false,
				                    null,
				                    null,
						            null,
								    false,
				                    false);
				
						treeViewEnginesolicitados.AppendValues (iter,
							   lector["descripcion_producto"].ToString().Trim(),
							    false,
				                lector["folio_de_solicitud"].ToString().Trim(),
				                float.Parse(lector["cantidad_solicitada"].ToString().Trim()).ToString("F"),
						        "0.00",             
							    true,
							    true);
					}					
					while((bool) lector.Read()){					
						// llenado de lista de solicitudes
						if(tipo_treeview == false){
							treeViewEnginesolicitados.AppendValues(false,
						                                       lector["folio_de_servicio"].ToString().Trim(),
						                                       lector["pidpaciente"].ToString().Trim(),
						                                       lector["nombre_completo"].ToString().Trim(),
						                                       lector["folio_de_solicitud"].ToString().Trim(),
						                                       lector["id_producto"].ToString().Trim(),
						                                       lector["descripcion_producto"].ToString().Trim(),
						                                       float.Parse(lector["cantidad_solicitada"].ToString().Trim()).ToString("F"),
						                                       float.Parse(lector["cantidad_autorizada"].ToString().Trim()).ToString("F"),
						                                       lector["fechahorasolicitud"].ToString().Trim(),
						                                       lector["area_quien_solicita"].ToString().Trim(),
						                                       lector["id_quien_solicito"].ToString().Trim(),
						                                       lector["descripcion_proveedor"].ToString().Trim(),
						                                       lector["id_secuencia"].ToString().Trim());
						}
					
						// llenado por paciente
						if(tipo_treeview == true){
							if (toma_pidpaciente != (int) lector["pidpaciente"]){	
								iter = treeViewEnginesolicitados.AppendValues (lector["nombre_completo"].ToString().Trim(),
								    false,
				                    null,
				                    null,
								    null,
								    false,
				                    false);
								toma_pidpaciente = (int) lector["pidpaciente"];
								
								treeViewEnginesolicitados.AppendValues (iter,
							   		lector["descripcion_producto"].ToString().Trim(),
							    	false,
				                	lector["folio_de_solicitud"].ToString().Trim(),
				                	float.Parse(lector["cantidad_solicitada"].ToString().Trim()).ToString("F"),
								    "0.00",
							    	true,
							    	true);
							}else{
								treeViewEnginesolicitados.AppendValues (iter,
							   		lector["descripcion_producto"].ToString().Trim(),
							    	false,
				                	lector["folio_de_solicitud"].ToString().Trim(),
				                	float.Parse(lector["cantidad_solicitada"].ToString().Trim()).ToString("F"),
								    "0.00",
							    	true,
							    	true);
							}
						}
					}
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();						msgBoxError.Destroy();
			}
			conexion.Close();				
		}
				
		private void ItemToggled (object sender, ToggledArgs args)
		{
			Gtk.TreeIter iter; 			
			TreePath path = new TreePath (args.Path);
			if (treeview_lista_solicitados.Model.GetIter (out iter, path)){					
				bool old = (bool) treeview_lista_solicitados.Model.GetValue(iter,1);
				treeview_lista_solicitados.Model.SetValue(iter,1,!old);
			}						
		}
		
		private void selecciona_examen (object sender, ToggledArgs args)
		{
			Gtk.TreeIter iter; 			
			TreePath path = new TreePath (args.Path);
			if (treeview_lista_solicitados.Model.GetIter (out iter, path)){					
				bool old = (bool) treeview_lista_solicitados.Model.GetValue(iter,0);
				treeview_lista_solicitados.Model.SetValue(iter,0,!old);
			}						
		}
		
		void create_treeview_cargados()
		{
			//treeViewEnginecargosvalid = new TreeStore(typeof(bool),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
			//									typeof(string),typeof(string),typeof(string),typeof(string));
			//treeview_lista_cargosvalid.Model = treeViewEnginecargosvalid;
			//treeview_lista_cargosvalid.RulesHint = true;
		}
		
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		void NumberCellEdited_Autorizado (object sender, EditedArgs args)
		{
			Gtk.TreeIter iter;
			//Gtk.CellRendererText  cellobj = (Gtk.CellRendererText) sender;
			bool esnumerico = false;
			int var_paso = 0;
			int largo_variable = args.NewText.ToString().Length;
			string toma_variable = args.NewText.ToString();
						
			treeViewEnginesolicitados.GetIter (out iter, new Gtk.TreePath (args.Path));
			
			while (var_paso < largo_variable){				
				if ((string) toma_variable.Substring(var_paso,1).ToString() == "." || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "0" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "1" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "2" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "3" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "4" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "5" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "6" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "7" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "8" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "9") {
					esnumerico = true;
				}else{
				 	esnumerico = false;
				 	var_paso = largo_variable;
				}
				var_paso += 1;
			}
			if (esnumerico == true){		
				treeViewEnginesolicitados.SetValue(iter,(int) coldatos_request.col_request8,args.NewText);
				//bool old = (bool) treeview_lista_solicitados.Model.GetValue (iter,0);							
			}
			
		}
		
		void NumberCellEdited_Autorizado_1 (object sender, EditedArgs args)
		{
			Gtk.TreeIter iter;
			//Gtk.CellRendererText  cellobj = (Gtk.CellRendererText) sender;
			bool esnumerico = false;
			int var_paso = 0;
			int largo_variable = args.NewText.ToString().Length;
			string toma_variable = args.NewText.ToString();
						
			treeViewEnginesolicitados.GetIter (out iter, new Gtk.TreePath (args.Path));
			
			while (var_paso < largo_variable){				
				if ((string) toma_variable.Substring(var_paso,1).ToString() == "." || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "0" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "1" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "2" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "3" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "4" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "5" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "6" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "7" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "8" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "9") {
					esnumerico = true;
				}else{
				 	esnumerico = false;
				 	var_paso = largo_variable;
				}
				var_paso += 1;
			}
			if (esnumerico == true){
				treeViewEnginesolicitados.SetValue(iter,(int) Column.cantautorizada,args.NewText);
				//bool old = (bool) treeview_lista_solicitados.Model.GetValue (iter,1);							
			}
 		}
	}	
}