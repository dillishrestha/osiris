// created on 20/09/2007 at 06:06 p
//////////////////////////////////////////////////////////
// created on 21/06/2007 at 01:40 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
//				 
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

namespace osiris
{
	public class requisicion_materiales_compras
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		//imprimir
		[Widget] Gtk.Button button_imprimir;
		[Widget] Gtk.Button button_busca_requisiones = null;
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_cantidad_aplicada;
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		
		// Declarando ventana del menu de Requisiciones
		[Widget] Gtk.Window requisicion_materiales;
		[Widget] Gtk.CheckButton checkbutton_nueva_requisicion;
		[Widget] Gtk.Entry entry_requisicion;
		[Widget] Gtk.Entry entry_status_requisicion;
		[Widget] Gtk.Button button_selecciona_requisicion;
		[Widget] Gtk.Button button_busca_requisicion;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.Button button_quitar_productos;
		[Widget] Gtk.Entry entry_fecha_solicitud;
		[Widget] Gtk.Button button_envio_compras;
		[Widget] Gtk.Button button_enviopara_autorizar;
		[Widget] Gtk.Button button_autorizar_compra;
		[Widget] Gtk.Button button_busca_proveedores1;
		[Widget] Gtk.Button button_busca_proveedores2;
		[Widget] Gtk.Button button_busca_proveedores3;
	
		[Widget] Gtk.Entry entry_dia_requerida;
		[Widget] Gtk.Entry entry_mes_requerida;
		[Widget] Gtk.Entry entry_ano_requerida;
		
		[Widget] Gtk.ComboBox combobox_tipo_requisicion;
		[Widget] Gtk.Button button_exportar = null;
		[Widget] Gtk.Button button_productos_requi = null;
		
		[Widget] Gtk.ComboBox combobox_tipo_admision;
		[Widget] Gtk.ComboBox combobox_tipo_admision2;
		[Widget] Gtk.Entry entry_motivo = null;
		[Widget] Gtk.Entry entry_observaciones;
		[Widget] Gtk.Entry entry_solicitado_por = null;
		[Widget] Gtk.Entry entry_nombre_prodrequisado;		
		[Widget] Gtk.Entry entry_totalitems_productos;
		
		[Widget] Gtk.Button button_guardar_requisicion;
		[Widget] Gtk.TreeView lista_requisicion_productos;
		
		[Widget] Gtk.Entry entry_folio_servicio = null;
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_nombre_paciente = null;
		[Widget] Gtk.Button button_busca_paciente = null;
		
		/////// Ventana Busqueda de productos\\\\\\\\
		[Widget] Gtk.Window busca_producto = null;
		[Widget] Gtk.RadioButton radiobutton_nombre;
		[Widget] Gtk.RadioButton radiobutton_codigo;
		[Widget] Gtk.TreeView lista_de_producto;
		[Widget] Gtk.Label label_cantidad = null;
		[Widget] Gtk.HBox hbox3 = null;
		[Widget] Gtk.Label label3 = null;
		[Widget] Gtk.Entry entry_lote = null;
		[Widget] Gtk.Label label4 = null;
		[Widget] Gtk.Entry entry_caducidad = null;		
		[Widget] Gtk.Label label5 = null;
		[Widget] Gtk.Entry entry_precio = null;
		[Widget] Gtk.Label label_pack = null;
		[Widget] Gtk.Entry entry_embalaje_pack = null;
		[Widget] Gtk.Label label_desc_proveedor = null;
		[Widget] Gtk.Entry entry_producto_proveedor = null;
		[Widget] Gtk.Label label_codprod_proveedor = null;
		[Widget] Gtk.Entry entry_codprod_proveedor = null;
		[Widget] Gtk.Label label390 = null;
 		[Widget] Gtk.ComboBox combobox_tipo_unidad2 = null;		
		
		//Declarando la barra de estado
		[Widget] Gtk.Statusbar statusbar_almacen_requi;
		
		// Para la busqueda de proveedores
		[Widget] Gtk.TreeView lista_de_busqueda;
		
		
		/// <summary>
		/// Ventana de busqueda de requisiciones
		/// </summary>
		//[Widget] Gtk.Entry entry_fecha_inicio;
		[Widget] Gtk.Window envio_almacenes = null;
		[Widget] Gtk.Entry entry_dia_inicio;
		[Widget] Gtk.Entry entry_mes_inicio;
		[Widget] Gtk.Entry entry_ano_inicio;		
		[Widget] Gtk.Entry entry_dia_termino;
		[Widget] Gtk.Entry entry_mes_termino;
		[Widget] Gtk.Entry entry_ano_termino;
		[Widget] Gtk.HBox hbox1 = null;
		[Widget] Gtk.HBox hbox2 = null;
		
		[Widget] Gtk.CheckButton checkbutton_todos_envios;
		[Widget] Gtk.CheckButton checkbutton_seleccion_presupuestos;
		[Widget] Gtk.TreeView lista_almacenes;
		[Widget] Gtk.Button button_buscar;
		[Widget] Gtk.Button button_rep;
		[Widget] Gtk.Button button_buscar_prodreq = null;
			
		string connectionString;
		string nombrebd;
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
    	
    	string querylist = "";
    	
    	// Declarando las variables de publicas para uso dentro de classe
    	int idtipointernamiento;
    	int idcentrocosto;
		string descripinternamiento = "";	// Descripcion de Centro de Costos - Solicitado por
		
		int idtipointernamiento2 = 0; 		// Centro de Costos - con Cargo a
		string descripinternamiento2 = "";	// Descripcion de Centro de Costos - con Cargo a
		
		string descripcion_tipo_requi = "";	// descripcion del tipo de requisicion
		bool requi_ordinaria = false;
		bool requi_urgente = false;
		int idtiporequi;
		bool enviadacompras = false;			// Verifica que la requisicion este enviada a compras
				
		string nombre_proveedor1;
		string nombre_proveedor2;
		string nombre_proveedor3;
		
		string centrocosto = "";
    	string campoacceso = "";
		string accesocentrocosto = "";
    	
    	int idrequisicion = 0;
    	int accesomodulo = 0;
    	
    	int [] array_idtipoadmisiones;    	
    	int tipobusquedaprove = 1; 
		string filtro_query_alta = "AND osiris_erp_cobros_enca.alta_paciente = 'false' ";
    	
    	bool editar = true;
    	
    	int contador_items_requisados = 0;	// cuenta los items que son requisados
    	int contador_items_autorizadoscompra = 0;	// cuenta los items autorizados para comprar
		
		string[] args_args = {""};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9};
		    	
    	TreeStore treeViewEngineBusca2;				// Para la busqueda de Productos
    	TreeStore treeViewEngineRequisicion; 		// Lista de proctos en una requisicion
    	TreeStore treeViewEngineproveedores;		// Lista de proveedores en el treeview
    	
    	//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		//declaracion de columnas y celdas de treeview de busqueda de productos
		TreeViewColumn col_idproducto;		CellRendererText cellr0;
		TreeViewColumn col_desc_producto;	CellRendererText cellr1;
		TreeViewColumn col_precioprod;		CellRendererText cellrt2;
		TreeViewColumn col_ivaprod;			CellRendererText cellrt3;
		TreeViewColumn col_totalprod;		CellRendererText cellrt4;
		TreeViewColumn col_descuentoprod;	CellRendererText cellrt5;
		TreeViewColumn col_preciocondesc;	CellRendererText cellrt6;
		TreeViewColumn col_stock_actual;	CellRendererText cellrt7;
		TreeViewColumn col_grupoprod;		CellRendererText cellrt8;
		TreeViewColumn col_grupo1prod;		CellRendererText cellrt9;
		TreeViewColumn col_grupo2prod;		CellRendererText cellrt10;
		TreeViewColumn col_empaque;			CellRendererText cellrt16;
		TreeViewColumn col_aplica_iva;		CellRendererText cellrt20;
		TreeViewColumn col_cobro_activo;	CellRendererText cellrt21;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		class_buscador classfind_data = new class_buscador();
		
		public requisicion_materiales_compras(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_,
											  string centrocosto_,int idcentrocosto_, string campoacceso_,int[] array_idtipoadmisiones_, int accesomodulo_)
		{
			LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		centrocosto = centrocosto_;							// esta variable almacen el nombre del centro de costo
    		idcentrocosto = idcentrocosto_;   					// esta variable toma el cento de costo del departamento
    		idtipointernamiento = idcentrocosto;
    		campoacceso = campoacceso_;   						// este campo determina el tipo de busqueda cuando busca productos
    		array_idtipoadmisiones = array_idtipoadmisiones_;  // arreglo para seleccionar los centros de costos
    		accesomodulo = accesomodulo_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
    		
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "requisicion_materiales", null);
			gxml.Autoconnect (this);
			
			// Muestra ventana de Glade
			requisicion_materiales.Show();
			entry_requisicion.KeyPressEvent += onKeyPressEvent_enter_requisicion;
			button_selecciona_requisicion.Clicked += new EventHandler(on_button_selecciona_requisicion_clicked);
			// Creacion de una Nueva Requisicion
			checkbutton_nueva_requisicion.Clicked +=  new EventHandler(on_checkbutton_nueva_requisicion_clicked);
			button_busca_paciente.Clicked += new EventHandler(on_button_busca_paciente_clicked);
			// Asignando valores de Fechas
			entry_fecha_solicitud.Text = (string) DateTime.Now.ToString("yyyy-MM-dd");
			entry_solicitado_por.Text = NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado;
			entry_dia_requerida.Text = (string) DateTime.Now.ToString("dd");
			entry_mes_requerida.Text = (string) DateTime.Now.ToString("MM");
			entry_ano_requerida.Text = (string) DateTime.Now.ToString("yyyy");
			entry_dia_requerida.KeyPressEvent += onKeyPressEvent;
			entry_mes_requerida.KeyPressEvent += onKeyPressEvent;
			entry_ano_requerida.KeyPressEvent += onKeyPressEvent;
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			//imprime la informacion:
			button_imprimir.Clicked += new EventHandler(on_imprime_requisicion_clicked);
			button_busca_requisiones.Clicked += new EventHandler(on_button_busca_requisiones_clicked);
			button_productos_requi.Clicked += new EventHandler(on_button_productos_requi_clicked);
			// Activacion de boton de busqueda
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			// Guarda la requisiscion
			button_guardar_requisicion.Clicked += new EventHandler(on_button_guardar_requisicion_clicked);
			// quitar produtos 
			button_quitar_productos.Clicked += new EventHandler(on_button_quitar_productos_clicked);
			// Envio requisicion a compras
			button_envio_compras.Clicked  += new EventHandler(on_button_envio_compras_clicked);
			// Envio requisicion para su autorizacion de compras 
			button_enviopara_autorizar.Clicked += new EventHandler(on_button_enviopara_autorizar_clicked);
			// Autorizar productos para poder comprar
			button_autorizar_compra.Clicked += new EventHandler(on_button_autorizar_compra_clicked);
			// Busca proveedores
			button_busca_proveedores1.Clicked += new EventHandler(on_button_busca_proveedores1_clicked);
			button_busca_proveedores2.Clicked += new EventHandler(on_button_busca_proveedores2_clicked);
			button_busca_proveedores3.Clicked += new EventHandler(on_button_busca_proveedores3_clicked);
			// Exportar a .ODS
			button_exportar.Clicked += new EventHandler(on_button_exportar_clicked);
			// Desactivando Entrys y Combobox
			entry_fecha_solicitud.Sensitive = false;
			entry_dia_requerida.Sensitive = false;
			entry_mes_requerida.Sensitive = false;
			entry_ano_requerida.Sensitive = false;
			combobox_tipo_admision.Sensitive = false;
			combobox_tipo_admision2.Sensitive = false;
			entry_observaciones.Sensitive = false;
			entry_motivo.Sensitive = false;
			entry_solicitado_por.Sensitive = false;
			button_guardar_requisicion.Sensitive = false;
			button_envio_compras.Sensitive = false;
			button_autorizar_compra.Sensitive = false;
			button_enviopara_autorizar.Sensitive = false;
			combobox_tipo_requisicion.Sensitive = false;
			button_busca_proveedores1.Sensitive = false;
			button_busca_proveedores2.Sensitive = false;
			button_busca_proveedores3.Sensitive = false;
			button_exportar.Sensitive = false;
			entry_folio_servicio.Sensitive = false;
			entry_pid_paciente.Sensitive = false;
			entry_nombre_paciente.Sensitive = false;
			button_busca_paciente.Sensitive = false;
						
			bool primero = true;
			foreach (int i in array_idtipoadmisiones){
				if (primero == true){
					accesocentrocosto = i.ToString();
					primero = false;
				}else{
					accesocentrocosto = accesocentrocosto + "','"+i.ToString();
				}				
			}									
			statusbar_almacen_requi.Pop(0);
			statusbar_almacen_requi.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_almacen_requi.HasResizeGrip = false;			
			// Creacion del treeview
			crea_treeview_requisicion();
		}
		
		void on_button_exportar_clicked(object sender, EventArgs args)
		{
			if(LoginEmpleado == "DOLIVARES" || LoginEmpleado =="ADMIN" || LoginEmpleado == "SANDRASALASL"){
				string query_sql = "SELECT osiris_erp_requisicion_deta.id_requisicion,to_char(osiris_erp_requisicion_deta.id_producto,'999999999999') AS idproducto,"+
										"to_char(cantidad_solicitada,'999999.99') AS cantidadsolicitada,comprado,"+
										"osiris_productos.descripcion_producto,to_char(osiris_productos.cantidad_de_embalaje,'9999.99') AS cantidadembalaje,"+
										"osiris_productos.tipo_unidad_producto,to_char(numero_orden_compra,'9999999999') AS numeroordencompra,"+
										"autorizada,to_char(fechahora_autorizado,'yyyy-MM-dd') AS fechahoraautorizado," +
										"to_char(osiris_erp_requisicion_deta.fechahora_requisado,'yyyy-MM-dd') AS fecharequisado,"+										
										"to_char(osiris_erp_requisicion_deta.costo_por_unidad,'999999999.99') AS costoporunidad,"+
										"to_char(osiris_erp_requisicion_deta.costo_producto,'999999999.99') AS costoproducto,"+
										"to_char(fechahora_compra,'yyyy-MM-dd') AS fechahoracompra,to_char(osiris_erp_requisicion_deta.id_secuencia,'9999999999') AS idsecuencia,"+
										"to_char(id_proveedor1,'9999999999') AS idproveedor1,osiris_erp_proveedores.descripcion_proveedor,"+
										"to_char(id_proveedor2,'9999999999') AS idproveedor2,descripcion_proveedor2,"+
										"to_char(id_proveedor3,'9999999999') AS idproveedor3,descripcion_proveedor3,"+
										"to_char(osiris_erp_requisicion_deta.porcentage_ganancia,'9999.99') AS porcentageganancia,osiris_erp_requisicion_enca.id_quien_requiso, "+
										"osiris_empleado.nombre1_empleado || ' ' || "+"osiris_empleado.nombre2_empleado || ' ' || "+"osiris_empleado.apellido_paterno_empleado || ' ' || "+ 
										"osiris_empleado.apellido_materno_empleado AS quienrequiso,osiris_erp_requisicion_enca.id_tipo_requisicion_compra,osiris_erp_requisicion_enca.descripcion_tipo_requisicion AS tipo_de_requi "+
										"FROM osiris_erp_requisicion_deta,osiris_productos,osiris_erp_proveedores,osiris_empleado,osiris_erp_requisicion_enca,osiris_erp_tipo_requisiciones_compra "+
										"WHERE osiris_erp_requisicion_deta.id_producto = osiris_productos.id_producto " +
										"AND osiris_erp_requisicion_enca.id_requisicion = osiris_erp_requisicion_deta.id_requisicion "+
										"AND osiris_erp_requisicion_deta.id_proveedor1 = osiris_erp_proveedores.id_proveedor "+
										"AND osiris_empleado.login_empleado = osiris_erp_requisicion_enca.id_quien_requiso " +
										"AND osiris_erp_requisicion_enca.id_tipo_requisicion_compra = osiris_erp_tipo_requisiciones_compra.id_tipo_requisicion_compra "+
										"AND eliminado = 'false' "+
										"AND osiris_erp_requisicion_deta.id_requisicion = '"+this.entry_requisicion.Text.Trim()+"' ";
				string[] args_names_field = {"id_requisicion","cantidadsolicitada","idproducto","descripcion_producto","tipo_unidad_producto","cantidadembalaje","fecharequisado","quienrequiso","tipo_de_requi"};
				string[] args_type_field = {"float","float","float","string","string","float","string","string","string"};
				// class_crea_ods.cs
				new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field);
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"No tiene Permiso para esta Opcion");
				msgBox.Run ();msgBox.Destroy();
			}
		}
		
		void on_imprime_requisicion_clicked(object sender, EventArgs args)
		{
		  if(this.entry_requisicion.Text == ""){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
				                                               ButtonsType.Close,"Favor de llenar toda la informaciòn correspondiente");
				msgBoxError.Run ();					msgBoxError.Destroy();			
			}else{ 
		  		new osiris.rpt_requisicion_compras(this.nombrebd,entry_requisicion.Text,entry_status_requisicion.Text,entry_fecha_solicitud.Text,
				                                   entry_ano_requerida.Text+"-"+entry_mes_requerida.Text+"-"+entry_dia_requerida.Text,entry_observaciones.Text,
				                                   entry_totalitems_productos.Text,lista_requisicion_productos,treeViewEngineRequisicion,
				                                   entry_solicitado_por.Text,entry_motivo.Text,nombre_proveedor1,nombre_proveedor2,nombre_proveedor3,
				                                   descripcion_tipo_requi,descripinternamiento,descripinternamiento2);
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
		
		void on_button_productos_requi_clicked(object sender, EventArgs args)
		{
			new osiris.movimientos_productos(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"productos_requisados","");
		}
		
		void on_button_busca_requisiones_clicked(object sender, EventArgs args)
		{
		  	Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "envio_almacenes", null);
			gxml.Autoconnect (this);
			envio_almacenes.Title = "Buscar Requisiciones por Fecha";				
			entry_dia_inicio.Text = DateTime.Now.ToString("dd");
			entry_mes_inicio.Text = DateTime.Now.ToString("MM");
			entry_ano_inicio.Text = DateTime.Now.ToString("yyyy");				
			entry_dia_termino.Text = DateTime.Now.ToString("dd");
			entry_mes_termino.Text = DateTime.Now.ToString("MM");
			entry_ano_termino.Text = DateTime.Now.ToString("yyyy");				
			hbox1.Hide();
			hbox2.Hide();			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			//button_buscar.Clicked += new EventHandler(on_buscar_clicked);
           	//button_rep.Clicked += new EventHandler(on_button_rep_clicked);
          	//checkbutton_todos_envios.Clicked += new EventHandler(on_checkbutton_todos_envios);
			checkbutton_seleccion_presupuestos.Hide();
		}
				
		void on_checkbutton_nueva_requisicion_clicked(object sender, EventArgs args)
		{
			if (checkbutton_nueva_requisicion.Active == true){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de CREAR una Nueva REQUISICION ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
		 			// Limpiando TreeView
		 			treeViewEngineRequisicion.Clear();
		 			// Desactivando los botones
		 			entry_requisicion.Text = "";
		 			button_busca_requisicion.Sensitive = false;
		 			entry_fecha_solicitud.Sensitive = false;		 			
					entry_dia_requerida.Sensitive = true;
					entry_mes_requerida.Sensitive = true;
					entry_ano_requerida.Sensitive = true;			
					combobox_tipo_admision.Sensitive = true;
					combobox_tipo_admision2.Sensitive = true;
					entry_observaciones.Sensitive = true;
					button_guardar_requisicion.Sensitive = true;
					entry_requisicion.Sensitive = false;
					button_selecciona_requisicion.Sensitive = false;
					button_enviopara_autorizar.Sensitive = false;
					combobox_tipo_requisicion.Sensitive = true;
					button_busca_producto.Sensitive = true;
					button_busca_proveedores1.Sensitive = true;
					button_busca_proveedores2.Sensitive = true;
					button_busca_proveedores3.Sensitive = true;
					entry_motivo.Sensitive = true;
					button_exportar.Sensitive = false;
					entry_solicitado_por.Sensitive = true;
					// Limpiando Variables
					entry_observaciones.Text = "";
					entry_status_requisicion.Text = "";					
					editar = false;										
					//creacion_del_combobox(1,"","",false,false,"");
					llenado_combobox(0,"",combobox_tipo_admision,"sql","SELECT * FROM osiris_his_tipo_admisiones WHERE id_tipo_admisiones IN('"+accesocentrocosto+"') AND id_tipo_admisiones ='"+idcentrocosto.ToString().Trim()+"' ORDER BY descripcion_admisiones;","descripcion_admisiones","id_tipo_admisiones",args_args,args_id_array);
					llenado_combobox(0,"",combobox_tipo_admision2,"sql","SELECT * FROM osiris_his_tipo_admisiones WHERE id_tipo_admisiones IN('"+accesocentrocosto+"') ORDER BY descripcion_admisiones;","descripcion_admisiones","id_tipo_admisiones",args_args,args_id_array);
					llenado_combobox(0,"",combobox_tipo_requisicion,"sql","SELECT * FROM osiris_erp_tipo_requisiciones_compra WHERE activo = 'true' ORDER BY id_tipo_requisicion_compra;","descripcion_tipo_requisicion","id_tipo_requisicion_compra",args_args,args_id_array);
					requi_ordinaria = false;
					requi_urgente = false;
					enviadacompras = false;
					descripinternamiento = "";
					descripinternamiento2 = "";
					descripcion_tipo_requi = "";													
		 		}else{
		 			checkbutton_nueva_requisicion.Active = false;
		 			entry_requisicion.Sensitive = true;
		 		}
		 	}else{
		 		button_busca_requisicion.Sensitive = true;
		 		entry_fecha_solicitud.Sensitive = false;
				entry_dia_requerida.Sensitive = false;
				entry_mes_requerida.Sensitive = false;
				entry_ano_requerida.Sensitive = false;			
				combobox_tipo_admision.Sensitive = false;
				combobox_tipo_admision2.Sensitive = false;
				entry_observaciones.Sensitive = false;
				button_guardar_requisicion.Sensitive = false;
				entry_requisicion.Sensitive = true;
				button_selecciona_requisicion.Sensitive = true;
				combobox_tipo_requisicion.Sensitive = false;
				button_busca_proveedores1.Sensitive = false;
				button_busca_proveedores2.Sensitive = false;
				button_busca_proveedores3.Sensitive = false;
				entry_motivo.Sensitive = false;
				entry_solicitado_por.Sensitive = false;
				button_exportar.Sensitive = false;
		 	}
		}
		
		void on_button_busca_producto_clicked (object sender, EventArgs args)
		{
			Gtk.Button button_buscarproducto = sender as Gtk.Button;
			Console.WriteLine(button_buscarproducto.Name.ToString());
			//Gtk.Button button_busca_producto = (Gtk.Button) sender;			
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda("producto");
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_entry_expresion;
			//this.entry_cantidad_aplicada.CanFocus = true;
			if(button_buscarproducto.Name == "button_busca_producto"){
				label_pack.Hide();
				entry_embalaje_pack.Hide();
				label_desc_proveedor.Hide();
				entry_producto_proveedor.Hide();
				label_codprod_proveedor.Hide();
				entry_codprod_proveedor.Hide();
				label3.Hide();
				entry_lote.Hide();
				label4.Hide();
				entry_caducidad.Hide();
				label390.Hide();
				combobox_tipo_unidad2.Hide();
				label5.Show();
				entry_precio.Show();
				button_selecciona.Label = "Requisar";
				button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);			
				entry_cantidad_aplicada.KeyPressEvent += onKeyPressEvent;
				entry_precio.KeyPressEvent += onKeyPressEvent;
			}
			if(button_buscarproducto.Name == "button_buscar_prodreq"){
				label_cantidad.Hide();
				entry_cantidad_aplicada.Hide();
				label_pack.Hide();
				entry_embalaje_pack.Hide();
				label_desc_proveedor.Hide();
				entry_producto_proveedor.Hide();
				label_codprod_proveedor.Hide();
				entry_codprod_proveedor.Hide();
				hbox3.Hide();
				label390.Hide();
				combobox_tipo_unidad2.Hide();
			}
			busca_producto.Show();			
		}
		
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			if (float.Parse(entry_cantidad_aplicada.Text) > 0){
				if(float.Parse(entry_precio.Text) > 0){
					TreeModel model;
					TreeIter iterSelected;				
					// Llenando el TreeView para la requisicion
	 				if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){ 				
	 					contador_items_requisados  += 1;
						entry_totalitems_productos.Text = contador_items_requisados.ToString().Trim();
						entry_nombre_prodrequisado.Text = (string) model.GetValue(iterSelected, 1);
						float costounitario_prod = float.Parse(entry_precio.Text)/float.Parse((string) model.GetValue(iterSelected, 16));
	 					
						treeViewEngineRequisicion.AppendValues(entry_cantidad_aplicada.Text,
															(string) model.GetValue(iterSelected, 1), 
															(string) model.GetValue(iterSelected, 0),
															(string) model.GetValue(iterSelected, 16),		// Embalaje del producto
															(string) model.GetValue(iterSelected, 7),		// Tipo de Unidad
						                                     "",
						                                     "",
						                                     "",
						                                     "0",
						                                     "",					// fecha de compra
						                                     true,
						                                     false,
						                                     false,
						                                     "",					// fecha de autorizacion
						                                     "0",					// id de la secuencia
						                                     "0",					
						                                     "0",
						                                     "0",
															 "",			// descripcion proveedor 1 
						                                     "",			// descripcion proveedor 2
						                                     "",			// descripcion proveedor 3
						                                     costounitario_prod.ToString("F"),					// Precio costo unitario
						                                     float.Parse(entry_precio.Text.Trim()).ToString("F"),
						                                     "1",			// id proveedor 1
						                                     "1",
						                                     "1",
						                                     (string) model.GetValue(iterSelected, 14),	// Porcentage de Ganancia
						                                     false);
						entry_cantidad_aplicada.Text = "0";
						//this.entry_cantidad_aplicada.StartEditing(0);
						entry_cantidad_aplicada.CanFocus = true;
						entry_precio.Text = "0";
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, 
						ButtonsType.Close, "Precio del producto debe ser \n"+
										   "mayor que cero, intente de nuevo...");
					msgBoxError.Run ();
					msgBoxError.Destroy();
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, 
						ButtonsType.Close, "La cantidad que quiere requisar debe ser \n"+
										   "mayor que cero, intente de nuevo...");
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
 		}
 		
 		void on_button_quitar_productos_clicked (object sender, EventArgs args)
 		{
 			TreeModel model;
			TreeIter iterSelected;
 			if (lista_requisicion_productos.Selection.GetSelected(out model, out iterSelected)){
 				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta quitar el producto "+(string) this.lista_requisicion_productos.Model.GetValue (iterSelected,1));
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
		 			if(enviadacompras == false){
		 				// Valida que pueda quitar antes de enviar a compras o sin guardar la requiscion		 				
		 				if ((string) lista_requisicion_productos.Model.GetValue (iterSelected,14) == "0"){
		 					contador_items_requisados -= 1;
							entry_totalitems_productos.Text = contador_items_requisados.ToString().Trim();							
							treeViewEngineRequisicion.Remove(ref iterSelected);  // quita elementos del treeview
		 				}else{
				 			NpgsqlConnection conexion; 
							conexion = new NpgsqlConnection (connectionString+nombrebd);
							try{
								conexion.Open ();
								NpgsqlCommand comando; 
								comando = conexion.CreateCommand ();
								comando.CommandText = "UPDATE osiris_erp_requisicion_deta SET eliminado = 'true',"+
										"id_quien_elimino = '"+LoginEmpleado+"',"+
										"fechahora_eliminado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
										"WHERE id_secuencia = '"+(string) lista_requisicion_productos.Model.GetValue (iterSelected,14)+"';";																	
									//Console.WriteLine(comando.CommandText);						
								comando.ExecuteNonQuery();
								comando.Dispose();
								contador_items_requisados -= 1;
								entry_totalitems_productos.Text = contador_items_requisados.ToString().Trim();							
								treeViewEngineRequisicion.Remove(ref iterSelected);  // quita elementos del treeview						
							}catch (NpgsqlException ex){
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();			msgBoxError.Destroy();
							}
							conexion.Close();
						}			 			
			 		}
		 		}
		 	}
 		}
 		
 		// Autorizando los productos que deben comprarse y que se van a mostrar en la
 		// Pantalla de Orden de Compra
 		void on_button_autorizar_compra_clicked(object sender, EventArgs args)
 		{
 			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de AUTORIZAR los productos para su COMPRA? ");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
		 	if (miResultado == ResponseType.Yes){
		 		if((string) LoginEmpleado == "DOLIVARES" || (string) LoginEmpleado == "ADMIN"){		 		
			 		NpgsqlConnection conexion;
					conexion = new NpgsqlConnection (connectionString+nombrebd);
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						TreeIter iter;
						if (treeViewEngineRequisicion.GetIterFirst (out iter)){					
							// Verificando que la casilla de autorizado este marcada
							//Console.WriteLine(Convert.ToString((bool) this.lista_requisicion_productos.Model.GetValue (iter,11)));
							if((bool) lista_requisicion_productos.Model.GetValue (iter,11) == true){
								comando.CommandText = "UPDATE osiris_erp_requisicion_deta SET autorizada = 'true',"+
									"id_quien_autorizo = '"+LoginEmpleado+"',"+
									"fechahora_autorizado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
									"WHERE id_secuencia = '"+(string) lista_requisicion_productos.Model.GetValue (iter,14)+"';";																	
								//Console.WriteLine(comando.CommandText);
								comando.ExecuteNonQuery();		comando.Dispose();
								contador_items_autorizadoscompra += 1;
							}
						}					
						while (treeViewEngineRequisicion.IterNext(ref iter)){
							//Console.WriteLine(Convert.ToString((bool) this.lista_requisicion_productos.Model.GetValue (iter,11)));
							if((bool) lista_requisicion_productos.Model.GetValue (iter,11) == true){
								comando.CommandText = "UPDATE osiris_erp_requisicion_deta SET autorizada = 'true',"+
									"id_quien_autorizo = '"+LoginEmpleado+"',"+
									"fechahora_autorizado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
									"WHERE id_secuencia = '"+(string) lista_requisicion_productos.Model.GetValue (iter,14)+"';";																
								//Console.WriteLine(comando.CommandText);
								comando.ExecuteNonQuery();		comando.Dispose();
								contador_items_autorizadoscompra += 1;
							}
						}
						
						comando.CommandText = "UPDATE osiris_erp_requisicion_enca SET  items_autorizados_paracomprar = '"+this.contador_items_autorizadoscompra.ToString()+"' "+
											"WHERE id_requisicion = '"+idrequisicion.ToString()+"';";																	
						comando.ExecuteNonQuery();
						comando.Dispose();
						
						llenado_de_requisicion();
						
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();						msgBoxError.Destroy();
					}
			 		conexion.Close();					
			 	}else{
			 		MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Error,ButtonsType.Ok,"NO esta autorizado para esta opcion...");
					msgBox1.Run ();		msgBox1.Destroy();
			 	}	
		 	}
 		} 		
 		
 		void on_button_enviopara_autorizar_clicked(object sender, EventArgs args)
 		{
 			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta ENVIAR Requisicion, para su autorizacion de compra de Materiales ? ");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
		 	if (miResultado == ResponseType.Yes){
		 		NpgsqlConnection conexion;
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					comando.CommandText = "UPDATE osiris_erp_requisicion_enca SET autorizacion_para_comprar = 'true',"+
								"id_quien_envio_autorizacion_compra = '"+LoginEmpleado+"',"+
								"fechahora_autorizacion_comprar = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
								"WHERE id_requisicion = '"+this.idrequisicion.ToString()+"';";																	
					//Console.WriteLine(comando.CommandText);
					entry_status_requisicion.Text = "AUTORIZADA EN COMPRAS "+DateTime.Now.ToString("dd-MM-yyyy");	
					comando.ExecuteNonQuery();
					comando.Dispose();					
					button_busca_producto.Sensitive = false;
					MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"Se autorizo la compra de materiales o insumos con Exito...");
					msgBox1.Run ();		msgBox1.Destroy();					
					button_guardar_requisicion.Sensitive = false;
					button_envio_compras.Sensitive = false;
					button_enviopara_autorizar.Sensitive = false;					
					if((string) LoginEmpleado == "DOLIVARES" || (string) LoginEmpleado == "ADMIN"){
						button_autorizar_compra.Sensitive = true;
					}							
					llenado_de_requisicion();					
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error, 
									ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();						msgBoxError.Destroy();
				}
		 		conexion.Close();		 	
		 	}
 		}
 		
 		void on_button_envio_compras_clicked(object sender, EventArgs args)
 		{
 			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta ENVIAR requicision a Compras, para su autorizacion ? ");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
		 	if (miResultado == ResponseType.Yes){
		 		NpgsqlConnection conexion;
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					comando.CommandText = "UPDATE osiris_erp_requisicion_enca SET enviada_a_compras = 'true',"+
								"id_quien_envio_compras = '"+LoginEmpleado+"',"+
								"total_items_solicitados = '"+this.contador_items_requisados.ToString()+"',"+
								"fechahora_envio_compras = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
								"WHERE id_requisicion = '"+this.idrequisicion.ToString()+"';";																	
					//Console.WriteLine(comando.CommandText);
					entry_status_requisicion.Text = "ENVIADA A COMPRAS "+DateTime.Now.ToString("dd-MM-yyyy");	
					comando.ExecuteNonQuery();
					comando.Dispose();					
					button_busca_producto.Sensitive = false;
					MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"La requisicion se envio a Comnpras con Exito...");
					msgBox1.Run ();		msgBox1.Destroy();					
					button_guardar_requisicion.Sensitive = false;
					button_envio_compras.Sensitive = false;					
					if((string) LoginEmpleado == "DOLIVARES" || (string) LoginEmpleado == "ADMIN" || (string) LoginEmpleado == "SANDRASALASL"){
						button_enviopara_autorizar.Sensitive = true;			
					}					
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error, 
									ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();						msgBoxError.Destroy();
				}
		 		conexion.Close();	
		 	}
 		}
 		
		void on_button_selecciona_requisicion_clicked(object sender, EventArgs args)
		{
			llenado_de_requisicion();
		}
 		
 		void llenado_de_requisicion()
 		{
 			string tiporequisicion_ = "";
 			bool validar_centro_costos = false;
			int proveedor_proveedor;
			bool activar_botton_autoriza = true;
			bool autorizaparacompra = false;
			idtipointernamiento = idcentrocosto;
 			contador_items_autorizadoscompra = 0;
 			treeViewEngineRequisicion.Clear();
 			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();		
				// Buscando numero de requisicion
				comando.CommandText = "SELECT id_requisicion,fechahora_creacion_requisicion,"+
									"osiris_erp_requisicion_enca.id_tipo_admisiones,osiris_his_tipo_admisiones.descripcion_admisiones,"+
									"descripcion_admisiones_cargada,"+
									"to_char(fechahora_envio_compras,'yyyy-MM-dd') AS fechahoraenviocompras,"+
									"to_char(fechahora_creacion_requisicion,'yyyy-MM-dd') AS fechacrearequisicion,"+
									"to_char(fecha_requerida,'yyyy') AS ano_fecha_requerida,"+
									"to_char(fecha_requerida,'MM') AS mes_fecha_requerida,"+
									"to_char(fecha_requerida,'dd') AS dia_fecha_requerida,"+
									"requisicion_ordinaria,requisicion_urgente,"+
									"enviada_a_compras,fechahora_envio_compras,osiris_erp_requisicion_enca.observaciones,motivo_requisicion,"+
									"cancelado,nombre1_empleado,nombre2_empleado,apellido_paterno_empleado,apellido_materno_empleado,"+
									"fechahora_autorizacion_comprar,autorizacion_para_comprar,items_autorizados_paracomprar,total_items_comprados," +
									"osiris_erp_requisicion_enca.folio_de_servicio AS foliodeatencion,"+
									"to_char(osiris_erp_requisicion_enca.pid_paciente,'9999999999') AS pidpaciente,"+
				            		"nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente,"+
									"osiris_erp_requisicion_enca.id_tipo_requisicion_compra AS idtiporequicompra,osiris_erp_tipo_requisiciones_compra.descripcion_tipo_requisicion "+
				  					"FROM osiris_erp_requisicion_enca,osiris_his_tipo_admisiones,osiris_empleado,osiris_erp_tipo_requisiciones_compra,osiris_his_paciente "+
									"WHERE osiris_erp_requisicion_enca.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
									"AND osiris_erp_requisicion_enca.id_quien_requiso = osiris_empleado.login_empleado " +
									"AND osiris_erp_requisicion_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
									"AND cancelado = 'false' " +
									"AND osiris_erp_requisicion_enca.id_tipo_requisicion_compra = osiris_erp_tipo_requisiciones_compra.id_tipo_requisicion_compra "+
									"AND id_requisicion = '"+this.entry_requisicion.Text.Trim()+"';";
				
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if ((bool) lector.Read()){
					contador_items_autorizadoscompra = (int) lector["items_autorizados_paracomprar"];
					editar = true;
					button_exportar.Sensitive = true;
					if (centrocosto == "COMPRAS" && accesomodulo == 0){
						tiporequisicion_ = (string) lector["descripcion_tipo_requisicion"];						
						llenado_combobox(1,(string) lector["descripcion_admisiones"],combobox_tipo_admision,"sql","SELECT * FROM osiris_his_tipo_admisiones WHERE id_tipo_admisiones IN('"+accesocentrocosto+"') AND id_tipo_admisiones ='"+idcentrocosto.ToString().Trim()+"' ORDER BY descripcion_admisiones;","descripcion_admisiones","id_tipo_admisiones",args_args,args_id_array);
						llenado_combobox(1,(string) lector["descripcion_admisiones_cargada"],combobox_tipo_admision2,"sql","SELECT * FROM osiris_his_tipo_admisiones WHERE id_tipo_admisiones IN('"+accesocentrocosto+"') ORDER BY descripcion_admisiones;","descripcion_admisiones","id_tipo_admisiones",args_args,args_id_array);
						llenado_combobox(1,(string) lector["descripcion_tipo_requisicion"],combobox_tipo_requisicion,"sql","SELECT * FROM osiris_erp_tipo_requisiciones_compra WHERE activo = 'true' ORDER BY id_tipo_requisicion_compra;","descripcion_tipo_requisicion","id_tipo_requisicion_compra",args_args,args_id_array);
						idrequisicion = int.Parse((string) this.entry_requisicion.Text.Trim());
						entry_motivo.Text = (string) lector["motivo_requisicion"].ToString().Trim();
						entry_observaciones.Text = (string) lector["observaciones"].ToString().Trim();
						descripinternamiento = (string) lector["descripcion_admisiones"];	// Descripcion de Centro de Costos - Solicitado por
						descripinternamiento2 = (string) lector["descripcion_admisiones_cargada"];	// Descripcion de Centro de Costos - con Cargo a
						descripcion_tipo_requi = (string) tiporequisicion_;	// descripcion del tipo de requisicion
						entry_solicitado_por.Text = (string) lector["nombre1_empleado"].ToString().Trim()+" "+
														(string) lector["nombre2_empleado"].ToString().Trim()+" "+
														(string) lector["apellido_paterno_empleado"].ToString().Trim()+" "+
														(string) lector["apellido_materno_empleado"].ToString().Trim();
						entry_fecha_solicitud.Text = (string) lector["fechacrearequisicion"];
						entry_dia_requerida.Text = (string) lector["dia_fecha_requerida"];
						entry_mes_requerida.Text = (string) lector["mes_fecha_requerida"];
						entry_ano_requerida.Text = (string) lector["ano_fecha_requerida"];
						entry_nombre_paciente.Text = (string) lector["nombre1_paciente"].ToString().Trim()+" "+(string) lector["nombre2_paciente"].ToString().Trim()+" "+(string) lector["apellido_paterno_paciente"]+" "+(string) lector["apellido_materno_paciente"].ToString().Trim();
						entry_folio_servicio.Text = (string) lector["foliodeatencion"].ToString().Trim();
						entry_pid_paciente.Text = (string) lector["pidpaciente"].ToString().Trim();
						autorizaparacompra = (bool) lector["autorizacion_para_comprar"];
						button_autorizar_compra.Sensitive = false;
						if ((bool) lector["enviada_a_compras"] == true && (bool) lector["cancelado"] == false){
							button_guardar_requisicion.Sensitive = false;
							button_envio_compras.Sensitive = false;
							button_enviopara_autorizar.Sensitive = false;
							button_busca_producto.Sensitive = false;
							button_quitar_productos.Sensitive = false;
							button_busca_proveedores1.Sensitive = false;
							button_busca_proveedores2.Sensitive = false;
							button_busca_proveedores3.Sensitive = false;
							button_autorizar_compra.Sensitive = false;
							entry_status_requisicion.Text = "ENVIADA A COMPRAS "+(string) lector["fechahoraenviocompras"];
							enviadacompras = true;														
							if ((bool) lector["autorizacion_para_comprar"] == false){
								MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,
								MessageType.Info,ButtonsType.Ok,"Envie su Autorizacion para Generar ORDEN DE COMPRA "+(string) lector["fechahoraenviocompras"]);
								msgBox1.Run ();		msgBox1.Destroy();
								//Console.WriteLine("envio para autorizar");
								if((string) LoginEmpleado == "DOLIVARES" || (string) LoginEmpleado == "ADMIN" || (string) LoginEmpleado == "SANDRASALASL"){									
									button_autorizar_compra.Sensitive = false;
									button_enviopara_autorizar.Sensitive = true;									
								}							
							}else{
								MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,
								MessageType.Info,ButtonsType.Ok,"Requisicion Autorizada para Generar la ORDEN DE COMPRA");
								msgBox1.Run ();		msgBox1.Destroy();
								entry_status_requisicion.Text = "AUTORIZADA PARA GENERAR ORDEN ED COMPRA";
								if((string) LoginEmpleado == "DOLIVARES"  || (string) LoginEmpleado == "ADMIN"){
									button_autorizar_compra.Sensitive = true;
									button_enviopara_autorizar.Sensitive = false;
								}
							}							
						}else{
							if ((bool) lector["cancelado"] == true){
								button_guardar_requisicion.Sensitive = false;
								button_envio_compras.Sensitive = false;
								button_enviopara_autorizar.Sensitive = false;
								button_busca_producto.Sensitive = false;
								button_quitar_productos.Sensitive = false;
								button_autorizar_compra.Sensitive = false;
								button_busca_proveedores1.Sensitive = false;
								button_busca_proveedores2.Sensitive = false;
								button_busca_proveedores3.Sensitive = false;							
								entry_status_requisicion.Text = "REQUISICION CANCELADA";
								MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"La requisicion se encuentra Cancelada, verifique... ");
								msgBox1.Run ();		msgBox1.Destroy();
						
							}else{
								button_guardar_requisicion.Sensitive = true;
								button_envio_compras.Sensitive = true;
								button_enviopara_autorizar.Sensitive = false;
								button_busca_producto.Sensitive = true;
								button_quitar_productos.Sensitive = true;
								button_autorizar_compra.Sensitive = false;
								button_busca_proveedores1.Sensitive = true;
								button_busca_proveedores2.Sensitive = true;
								button_busca_proveedores3.Sensitive = true;							
								entry_status_requisicion.Text = "PENDIENTE DE ENVIAR A COMPRAS";
							}
							enviadacompras = false;
							editar = true;
						}						
						// llenado del detalle de la requisicion u orden de compra
						comando.CommandText  = "SELECT id_requisicion,to_char(osiris_erp_requisicion_deta.id_producto,'999999999999') AS idproducto,"+
										"to_char(cantidad_solicitada,'999999.99') AS cantidadsolicitada,comprado,"+
										"osiris_productos.descripcion_producto,to_char(osiris_productos.cantidad_de_embalaje,'9999.99') AS cantidadembalaje,"+
										"osiris_productos.tipo_unidad_producto,to_char(numero_orden_compra,'9999999999') AS numeroordencompra,"+
										"autorizada,to_char(fechahora_autorizado,'yyyy-MM-dd') AS fechahoraautorizado,"+
										"to_char(osiris_erp_requisicion_deta.costo_por_unidad,'999999999.99') AS costoporunidad,"+
										"to_char(osiris_erp_requisicion_deta.costo_producto,'999999999.99') AS costoproducto,"+
										"to_char(fechahora_compra,'yyyy-MM-dd') AS fechahoracompra,to_char(osiris_erp_requisicion_deta.id_secuencia,'9999999999') AS idsecuencia,"+
										"to_char(id_proveedor1,'9999999999') AS idproveedor1,osiris_erp_proveedores.descripcion_proveedor,"+
										"to_char(id_proveedor2,'9999999999') AS idproveedor2,descripcion_proveedor2,"+
										"to_char(id_proveedor3,'9999999999') AS idproveedor3,descripcion_proveedor3,"+
										"to_char(osiris_erp_requisicion_deta.porcentage_ganancia,'9999.99') AS porcentageganancia,id_quien_requiso "+							
										"FROM osiris_erp_requisicion_deta,osiris_productos,osiris_erp_proveedores "+
										"WHERE osiris_erp_requisicion_deta.id_producto = osiris_productos.id_producto "+
										"AND osiris_erp_requisicion_deta.id_proveedor1 = osiris_erp_proveedores.id_proveedor "+
										"AND eliminado = 'false' "+
										"AND id_requisicion = '"+this.entry_requisicion.Text.Trim()+"' ";
							
						//Console.WriteLine(comando.CommandText);
						NpgsqlDataReader lector1 = comando.ExecuteReader ();
						string fechahoracompra_ = "";
						string fechahoraautorizado_ = "";
						contador_items_requisados = 0;
						contador_items_autorizadoscompra = 0;
						while ((bool) lector1.Read()){
							contador_items_requisados += 1;							
							if((bool) lector1["comprado"] == true){
								fechahoracompra_ = (string) lector1["fechahoracompra"];
							}else{
								fechahoracompra_ = "";
							}							
							if ((bool) lector1["autorizada"] == true ){
								fechahoraautorizado_ = (string) lector1["fechahoraautorizado"];
							}else{
								fechahoraautorizado_ = "";
							}							
							nombre_proveedor1 = (string) lector1["descripcion_proveedor"].ToString();
							nombre_proveedor2 = (string) lector1["descripcion_proveedor2"].ToString();
							nombre_proveedor3 = (string) lector1["descripcion_proveedor3"].ToString();							
							treeViewEngineRequisicion.AppendValues((string) lector1["cantidadsolicitada"],
														(string) lector1["descripcion_producto"], 
														(string) lector1["idproducto"],
														(string) lector1["cantidadembalaje"],
														(string) lector1["tipo_unidad_producto"],
														"",
														"",
														"",
														(string) lector1["numeroordencompra"],
														fechahoracompra_,
														false,
														true,
														(bool) lector1["autorizada"],
														fechahoraautorizado_,
														(string) lector1["idsecuencia"],
														"0",
														"0",
														"0",
														(string) lector1["descripcion_proveedor"],
														(string) lector1["descripcion_proveedor2"],
														(string) lector1["descripcion_proveedor3"],
														(string) lector1["costoporunidad"],
														(string) lector1["costoproducto"],
														(string) lector1["idproveedor1"],
														(string) lector1["idproveedor2"],
														(string) lector1["idproveedor3"],
														(string) lector1["porcentageganancia"],
														false);
							if(activar_botton_autoriza == true){
								if((bool) lector1["autorizada"] == true){
									activar_botton_autoriza = false;
								}
							}
						}
						if(autorizaparacompra == true){
							button_autorizar_compra.Sensitive = activar_botton_autoriza;
						}
						entry_totalitems_productos.Text = contador_items_requisados.ToString().Trim();					
    				}else{
    					if ( idcentrocosto == (int) lector["id_tipo_admisiones"]){
							tiporequisicion_ = (string) lector["descripcion_tipo_requisicion"];						
							llenado_combobox(1,(string) lector["descripcion_admisiones"],combobox_tipo_admision,"sql","SELECT * FROM osiris_his_tipo_admisiones WHERE id_tipo_admisiones IN('"+accesocentrocosto+"') AND id_tipo_admisiones ='"+idcentrocosto.ToString().Trim()+"' ORDER BY descripcion_admisiones;","descripcion_admisiones","id_tipo_admisiones",args_args,args_id_array);
							llenado_combobox(1,(string) lector["descripcion_admisiones_cargada"],combobox_tipo_admision2,"sql","SELECT * FROM osiris_his_tipo_admisiones WHERE id_tipo_admisiones IN('"+accesocentrocosto+"') ORDER BY descripcion_admisiones;","descripcion_admisiones","id_tipo_admisiones",args_args,args_id_array);
							llenado_combobox(1,(string) lector["descripcion_tipo_requisicion"],combobox_tipo_requisicion,"sql","SELECT * FROM osiris_erp_tipo_requisiciones_compra WHERE activo = 'true' ORDER BY id_tipo_requisicion_compra;","descripcion_tipo_requisicion","id_tipo_requisicion_compra",args_args,args_id_array);
							idrequisicion = int.Parse((string) entry_requisicion.Text.Trim());
							entry_motivo.Text = (string) lector["motivo_requisicion"].ToString().Trim();
							entry_observaciones.Text = (string) lector["observaciones"].ToString().Trim();
							descripinternamiento = (string) lector["descripcion_admisiones"];	// Descripcion de Centro de Costos - Solicitado por
							descripinternamiento2 = (string) lector["descripcion_admisiones_cargada"];	// Descripcion de Centro de Costos - con Cargo a
							descripcion_tipo_requi = (string) tiporequisicion_;	// descripcion del tipo de requisicion
							//entry_solicitado_por.Text = (string) lector["nombre1_empleado"].ToString().Trim()+" "+
							//							(string) lector["nombre2_empleado"].ToString().Trim()+" "+
							//							(string) lector["apellido_paterno_empleado"].ToString().Trim()+" "+
							//							(string) lector["apellido_materno_empleado"].ToString().Trim();
							entry_fecha_solicitud.Text = (string) lector["fechacrearequisicion"];
							entry_dia_requerida.Text = (string) lector["dia_fecha_requerida"];
							entry_mes_requerida.Text = (string) lector["mes_fecha_requerida"];
							entry_ano_requerida.Text = (string) lector["ano_fecha_requerida"];
							entry_nombre_paciente.Text = (string) lector["nombre1_paciente"].ToString().Trim()+" "+(string) lector["nombre2_paciente"].ToString().Trim()+" "+(string) lector["apellido_paterno_paciente"]+" "+(string) lector["apellido_materno_paciente"].ToString().Trim();
							entry_folio_servicio.Text = (string) lector["foliodeatencion"].ToString().Trim();
							entry_pid_paciente.Text = (string) lector["pidpaciente"].ToString().Trim();
							autorizaparacompra = (bool) lector["autorizacion_para_comprar"];
							button_autorizar_compra.Sensitive = false;
							if ((bool) lector["enviada_a_compras"] == true){
								button_autorizar_compra.Sensitive = false;
								button_guardar_requisicion.Sensitive = false;
								button_envio_compras.Sensitive = false;
								button_enviopara_autorizar.Sensitive = false;
								button_busca_producto.Sensitive = false;	
								button_busca_proveedores1.Sensitive = false;
								button_busca_proveedores2.Sensitive = false;
								button_busca_proveedores3.Sensitive = false;
								entry_status_requisicion.Text = "ENVIADA A COMPRAS "+(string) lector["fechahoraenviocompras"];														
								enviadacompras = true;							
								MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"La requisicion se envio a Compras con fecha "+(string) lector["fechahoraenviocompras"]);
								msgBox1.Run ();		msgBox1.Destroy();
								if ((bool) lector["autorizacion_para_comprar"] == false){									
									MessageDialog msgBox2 = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"Envie su Autorizacion para Generar ORDEN DE COMPRA "+(string) lector["fechahoraenviocompras"]);
									msgBox2.Run ();		msgBox2.Destroy();
									//Console.WriteLine("envio para autorizar");
									if((string) LoginEmpleado == "DOLIVARES" || (string) LoginEmpleado == "ADMIN" || (string) LoginEmpleado == "SANDRASALASL"){									
										button_autorizar_compra.Sensitive = false;
										button_enviopara_autorizar.Sensitive = true;									
									}							
								}else{
									MessageDialog msgBox2 = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"Requisicion Autorizada para Generar la ORDEN DE COMPRA");
									msgBox2.Run ();		msgBox2.Destroy();
									entry_status_requisicion.Text = "AUTORIZADA PARA GENERAR ORDEN DE COMPRA";
									if((string) LoginEmpleado == "DOLIVARES"  || (string) LoginEmpleado == "ADMIN"){
										button_enviopara_autorizar.Sensitive = false;
										button_autorizar_compra.Sensitive = true;
									}
								}
							}else{
								button_guardar_requisicion.Sensitive = true;
								button_envio_compras.Sensitive = true;
								button_enviopara_autorizar.Sensitive = false;
								button_busca_producto.Sensitive = true;
								button_autorizar_compra.Sensitive = false;
								button_busca_proveedores1.Sensitive = true;
								button_busca_proveedores2.Sensitive = true;
								button_busca_proveedores3.Sensitive = true;
								entry_status_requisicion.Text = "PENDIENTE DE ENVIAR A COMPRAS";
								enviadacompras = false;
								editar = true;
							}							
							comando.CommandText = "SELECT id_requisicion,to_char(osiris_erp_requisicion_deta.id_producto,'999999999999') AS idproducto,"+
										"to_char(cantidad_solicitada,'999999.99') AS cantidadsolicitada,comprado,"+
										"osiris_productos.descripcion_producto,to_char(osiris_productos.cantidad_de_embalaje,'9999.99') AS cantidadembalaje,"+
										"osiris_productos.tipo_unidad_producto,to_char(numero_orden_compra,'9999999999') AS numeroordencompra,"+
										"autorizada,to_char(fechahora_autorizado,'yyyy-MM-dd') AS fechahoraautorizado,"+
										"to_char(osiris_erp_requisicion_deta.costo_por_unidad,'999999999.99') AS costoporunidad,to_char(osiris_erp_requisicion_deta.costo_producto,'999999999.99') AS costoproducto,"+
										"to_char(fechahora_compra,'yyyy-MM-dd') AS fechahoracompra,to_char(osiris_erp_requisicion_deta.id_secuencia,'9999999999') AS idsecuencia,"+
										"to_char(id_proveedor1,'9999999999') AS idproveedor1,osiris_erp_proveedores.descripcion_proveedor,"+
										"to_char(id_proveedor2,'9999999999') AS idproveedor2,descripcion_proveedor2,"+
										"to_char(id_proveedor3,'9999999999') AS idproveedor3,descripcion_proveedor3,"+
										"to_char(osiris_erp_requisicion_deta.porcentage_ganancia,'9999.99') AS porcentageganancia "+							
										"FROM osiris_erp_requisicion_deta,osiris_productos,osiris_erp_proveedores "+
										"WHERE osiris_erp_requisicion_deta.id_producto = osiris_productos.id_producto "+
										"AND osiris_erp_requisicion_deta.id_proveedor1 = osiris_erp_proveedores.id_proveedor "+
										"AND eliminado = 'false' "+ 
										"AND id_requisicion = '"+this.entry_requisicion.Text.Trim()+"';";								
							//Console.WriteLine(comando.CommandText);
							NpgsqlDataReader lector1 = comando.ExecuteReader ();
							string fechahoracompra_ = "";
							string fechahoraautorizado_ = "";
							contador_items_requisados = 0;
							while ((bool) lector1.Read()){
								contador_items_requisados += 1;	
								if ((string) lector1["fechahoracompra"] == "2000-01-02"){
									fechahoracompra_ = "";
								}else{
									fechahoracompra_ = (string) lector1["fechahoracompra"];
								}
								
								if ((string) lector1["fechahoraautorizado"] == "2000-01-02"){
									fechahoraautorizado_ = "";
								}else{
									fechahoraautorizado_ = (string) lector1["fechahoraautorizado"];
								}								
								nombre_proveedor1 = (string) lector1["descripcion_proveedor"].ToString();
								nombre_proveedor2 = (string) lector1["descripcion_proveedor2"].ToString();
								nombre_proveedor3 = (string) lector1["descripcion_proveedor3"].ToString();								
								treeViewEngineRequisicion.AppendValues((string) lector1["cantidadsolicitada"],
															(string) lector1["descripcion_producto"], 
															(string) lector1["idproducto"],
															(string) lector1["cantidadembalaje"],
															(string) lector1["tipo_unidad_producto"],
															"",
															"",
															"",
															(string) lector1["numeroordencompra"],
															fechahoracompra_,
															false,
															true,
															(bool) lector1["autorizada"],
															fechahoraautorizado_,
															(string) lector1["idsecuencia"],
															"0",
															"0",
															"0",
															(string) lector1["descripcion_proveedor"],
															(string) lector1["descripcion_proveedor2"],
															(string) lector1["descripcion_proveedor3"],															
															(string) lector1["costoporunidad"],
															(string) lector1["costoproducto"],
															(string) lector1["idproveedor1"],
															(string) lector1["idproveedor2"],
															(string) lector1["idproveedor3"],
															(string) lector1["porcentageganancia"],
															false);
								if(activar_botton_autoriza == true){
									if((bool) lector1["autorizada"] == true){
										activar_botton_autoriza = true;
									}
								}
							}
							if(autorizaparacompra == true){
								
							}
							entry_totalitems_productos.Text = contador_items_requisados.ToString().Trim();
							button_autorizar_compra.Sensitive = activar_botton_autoriza;
						}else{
							MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"La requisicion NO es de este Centro de Costos...");
							msgBox.Run ();		msgBox.Destroy();
						}
    				}
				}else{
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"La requisicion NO existe, verifique...");
						msgBox.Run ();		msgBox.Destroy();
				}				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, 
					ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();						msgBoxError.Destroy();
			}
			conexion.Close();
 		}
		
		void llenado_combobox(int tipodellenado,string descrip_defaul,object obj,string sql_or_array,string query_SQL,string name_field_desc,string name_field_id,string[] args_array,int[] args_id_array)
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
						if(combobox_llenado.Name == "combobox_tipo_requisicion"){
							store.AppendValues ((string) lector[ name_field_desc ], (int) lector[ name_field_id],(bool) lector["selecciona_paciente"]);
						}else{
							store.AppendValues ((string) lector[ name_field_desc ], (int) lector[ name_field_id],false);
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
				case "combobox_tipo_admision":
					idtipointernamiento = (int) onComboBoxChanged.Model.GetValue(iter,1);
		    		descripinternamiento = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
				case "combobox_tipo_admision2":
					idtipointernamiento2 = (int) onComboBoxChanged.Model.GetValue(iter,1);
		    		descripinternamiento2 = (string) onComboBoxChanged.Model.GetValue(iter,0);					
					break;
				case "combobox_tipo_requisicion":
					idtiporequi = (int) onComboBoxChanged.Model.GetValue(iter,1);
					descripcion_tipo_requi = (string) onComboBoxChanged.Model.GetValue(iter,0);
					if(idtiporequi == 2){
						requi_ordinaria = true;
						requi_urgente = false;
					}
					if(idtiporequi == 3){
						requi_ordinaria = false;
						requi_urgente = true;
					}
					if((bool) combobox_tipo_requisicion.Model.GetValue(iter,2) == true){
						entry_folio_servicio.Sensitive = true;
						entry_pid_paciente.Sensitive = true;
						entry_nombre_paciente.Sensitive = true;
						button_busca_paciente.Sensitive = true;
					}else{
						entry_folio_servicio.Sensitive = false;
						entry_pid_paciente.Sensitive = false;
						entry_nombre_paciente.Sensitive = false;
						button_busca_paciente.Sensitive = false;
					}
					break;
				}
			}
		}
 		 		
 		void on_button_guardar_requisicion_clicked(object sender, EventArgs args)
 		{
 			string mensajebox = "";
 			if (editar == true){
 				 mensajebox = "¿ Esta seguro de agregas mas productos a esta REQUISICION ?";
 			}else{
 				 mensajebox = "¿ Esta seguro de GUARDAR esta REQUISICION ?";
 			}
 			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,mensajebox);
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
		 	if (miResultado == ResponseType.Yes){
		 		almacena_productos_requisados();
		 	}
 		}
 		
 		void almacena_productos_requisados()
		{
			string ultima_requisicion = entry_requisicion.Text;
			// Validando que capture toda la informacion
			if(descripinternamiento2 != "" && descripcion_tipo_requi != "" && entry_observaciones.Text != ""){			
				if(enviadacompras == false){
					if (editar == false){
						entry_requisicion.Text = classpublic.lee_ultimonumero_registrado("osiris_erp_requisicion_enca","id_requisicion","");
						ultima_requisicion = entry_requisicion.Text;
					}
					// Grabando Detalle de la requisicion
					TreeIter iter; 		
					if (treeViewEngineRequisicion.GetIterFirst (out iter)){  // revisa que el treview contenga valores
						NpgsqlConnection conexion; 
						conexion = new NpgsqlConnection (connectionString+nombrebd);
						try{
							conexion.Open ();
							NpgsqlCommand comando; 
							comando = conexion.CreateCommand ();
							//Console.WriteLine((string) Convert.ToString((bool) this.lista_requisicion_productos.Model.GetValue(iter,10)));
							// Verifica si el producto se acaba de agregar
							
							if ((bool) lista_requisicion_productos.Model.GetValue(iter,10) == true){
								comando.CommandText = "INSERT INTO osiris_erp_requisicion_deta ("+
											"id_requisicion,"+
											"id_producto,"+
											"cantidad_solicitada,"+
											"fechahora_requisado,"+
											"id_tipo_admisiones,"+
											"costo_producto,"+
											"costo_por_unidad,"+
											"cantidad_de_embalaje,"+
											"porcentage_ganancia,"+
											"id_proveedor,"+
											"id_proveedor1,"+
											"id_proveedor2,"+
											"id_proveedor3,"+
											"descripcion_proveedor2,"+
											"descripcion_proveedor3,"+
											"id_quien_requiso) "+
											"VALUES ('"+
											entry_requisicion.Text.Trim()+"','"+										
											(string) lista_requisicion_productos.Model.GetValue(iter,2)+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,0)+"','"+
											DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
											idtipointernamiento.ToString().Trim()+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,22)+"','"+   // precio del proveedor
											(string) lista_requisicion_productos.Model.GetValue(iter,21)+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,3)+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,26)+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,23)+"','"+ // id_proveedor
											(string) lista_requisicion_productos.Model.GetValue(iter,23)+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,24)+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,25)+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,19)+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,20)+"','"+										
											LoginEmpleado+"');";																	
								//Console.WriteLine(comando.CommandText);
								comando.ExecuteNonQuery();
								comando.Dispose();
							}else{
								// revisando que actualizacion de proveedores antes de enviar a Compras					
								if((bool) lista_requisicion_productos.Model.GetValue(iter,27) == true){
									// Realizando el Update en el registro para actualizar el proveedor
									comando.CommandText = comando.CommandText = "UPDATE osiris_erp_requisicion_deta SET id_proveedor = '"+(string) this.lista_requisicion_productos.Model.GetValue(iter,23)+"','"+
														"id_proveedor = '"+(string) this.lista_requisicion_productos.Model.GetValue(iter,23)+"' "+
														"WHERE id_secuencia = '"+(string) this.lista_requisicion_productos.Model.GetValue (iter,14)+"';";
									//Console.WriteLine(comando.CommandText);
									comando.ExecuteNonQuery();					comando.Dispose();
								}
							}
							while(treeViewEngineRequisicion.IterNext(ref iter)){
								//Console.WriteLine((string) Convert.ToString((bool) this.lista_requisicion_productos.Model.GetValue(iter,10)));
								if ((bool) lista_requisicion_productos.Model.GetValue(iter,10) == true){
									comando.CommandText = "INSERT INTO osiris_erp_requisicion_deta ("+
											"id_requisicion,"+
											"id_producto,"+
											"cantidad_solicitada,"+
											"fechahora_requisado,"+
											"id_tipo_admisiones,"+
											"costo_producto,"+
											"costo_por_unidad,"+
											"cantidad_de_embalaje,"+
											"porcentage_ganancia,"+
											"id_proveedor,"+
											"id_proveedor1,"+
											"id_proveedor2,"+
											"id_proveedor3,"+
											"descripcion_proveedor2,"+
											"descripcion_proveedor3,"+
											"id_quien_requiso) "+
											"VALUES ('"+
											entry_requisicion.Text.Trim()+"','"+										
											(string) lista_requisicion_productos.Model.GetValue(iter,2)+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,0)+"','"+
											DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
											idtipointernamiento.ToString().Trim()+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,22)+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,21)+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,3)+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,26)+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,23)+"','"+ // id_proveedor
											(string) lista_requisicion_productos.Model.GetValue(iter,23)+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,24)+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,25)+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,19)+"','"+
											(string) lista_requisicion_productos.Model.GetValue(iter,20)+"','"+										
											LoginEmpleado+"');";																		
									//Console.WriteLine(comando.CommandText);
									comando.ExecuteNonQuery();
								}else{
									// revisando que actualizacion de proveedores antes de enviar a Compras					
									if((bool) lista_requisicion_productos.Model.GetValue(iter,27) == true){
										comando.CommandText = comando.CommandText = "UPDATE osiris_erp_requisicion_deta SET id_proveedor = '"+(string) this.lista_requisicion_productos.Model.GetValue(iter,23)+"','"+
														"id_proveedor = '"+(string) this.lista_requisicion_productos.Model.GetValue(iter,23)+"' "+
														"WHERE id_secuencia = '"+(string) this.lista_requisicion_productos.Model.GetValue (iter,14)+"';";
										//Console.WriteLine(comando.CommandText);
										comando.ExecuteNonQuery();					comando.Dispose();
									}
								}
								comando.Dispose();
							}
							
							if (editar == false){			
								comando.CommandText = "INSERT INTO osiris_erp_requisicion_enca ("+
											"fechahora_creacion_requisicion,"+
											"id_requisicion,"+
											"fecha_requerida,"+
											"id_tipo_admisiones,"+
											"id_tipo_admisiones_cargada,"+
											"descripcion_admisiones_cargada,"+
											"id_quien_requiso,"+
											"observaciones,"+
											"motivo_requisicion,"+
											"requisicion_ordinaria,"+
											"requisicion_urgente,"+
											"descripcion_tipo_requisicion," +
											"id_tipo_requisicion_compra,"+
											"total_items_solicitados,folio_de_servicio,pid_paciente) "+
											"VALUES ('"+
											DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
											entry_requisicion.Text.Trim()+"','"+
											entry_ano_requerida.Text+"-"+this.entry_mes_requerida.Text+"-"+this.entry_dia_requerida.Text+"','"+
											idtipointernamiento.ToString().Trim()+"','"+
											idtipointernamiento2.ToString().Trim()+"','"+
											descripinternamiento2.Trim()+"','"+
											LoginEmpleado+"','"+
											entry_observaciones.Text.ToUpper().Trim()+"','"+
											entry_motivo.Text.ToString().Trim().ToUpper()+"','"+
											requi_ordinaria.ToString()+"','"+
											requi_urgente.ToString()+"','"+
											descripcion_tipo_requi+"','"+
											idtiporequi.ToString().Trim()+"','"+
											entry_totalitems_productos.Text+"','"+
											entry_folio_servicio.Text.Trim()+"','"+
											entry_pid_paciente.Text.Trim()+"');";																	
								//Console.WriteLine(comando.CommandText);						
								comando.ExecuteNonQuery();
								
								MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"La requisicion se guardo exitosamente, con el Nº "+this.entry_requisicion.Text.Trim());
								msgBox.Run ();		msgBox.Destroy();								
							}
							comando.Dispose();					
							editar = true;
							button_envio_compras.Sensitive = true;
			 				entry_status_requisicion.Text = "NO ESTA ENVIADA A COMPRAS";			 				
			 				// Actualizando el treeview			 				
						}catch (NpgsqlException ex){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
														MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();				msgBoxError.Destroy();
						}			
						conexion.Close();
						button_busca_requisicion.Sensitive = true;
				 		entry_fecha_solicitud.Sensitive = false;
						entry_dia_requerida.Sensitive = false;
						entry_mes_requerida.Sensitive = false;
						entry_ano_requerida.Sensitive = false;			
						combobox_tipo_admision.Sensitive = false;
						combobox_tipo_admision2.Sensitive = false;
						entry_observaciones.Sensitive = false;
						button_guardar_requisicion.Sensitive = true;
						button_enviopara_autorizar.Sensitive = true;
						button_busca_producto.Sensitive = true;
						entry_requisicion.Sensitive = true;
						button_selecciona_requisicion.Sensitive = true;
						checkbutton_nueva_requisicion.Active = false;
						entry_requisicion.Text = ultima_requisicion;
						// Actualizando el treeview
			 			llenado_de_requisicion();
					}else{
						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"Esta requisicion no tiene productos, debe agregar por lo menos uno...");
						msgBox.Run ();		msgBox.Destroy();
					}
				}else{
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"Esta enviada a compras no puede agregar mas productos");
					msgBox.Run ();		msgBox.Destroy();
				}
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"Favor de llenar toda la informacion solicitada...");
				msgBox.Run ();		msgBox.Destroy();
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
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			string query_tipo_busqueda = "";
			if(radiobutton_nombre.Active == true) {query_tipo_busqueda = "AND osiris_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_producto; "; }
			if(radiobutton_codigo.Active == true) {query_tipo_busqueda = "AND to_char(osiris_productos.id_producto,'999999999999') LIKE '%"+entry_expresion.Text.Trim()+"%'  ORDER BY id_producto; "; }
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
							"osiris_productos.descripcion_producto,osiris_productos.nombre_articulo,osiris_productos.nombre_generico_articulo, "+
							"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(precio_producto_publico1,'99999999.99') AS preciopublico1,"+
							"to_char(cantidad_de_embalaje,'99999999.99') AS cantidadembalaje,"+
							"tipo_unidad_producto,"+
							"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,cobro_activo,costo_unico,"+
							"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(osiris_productos.id_grupo_producto,'99999') AS idgrupoproducto,osiris_productos.id_grupo_producto, "+
							"to_char(osiris_productos.id_grupo1_producto,'99999') AS idgrupo1producto,osiris_productos.id_grupo1_producto, "+
							"to_char(osiris_productos.id_grupo2_producto,'99999') AS idgrupo2producto,osiris_productos.id_grupo2_producto, "+
							"to_char(porcentage_ganancia,'99999.999') AS porcentageutilidad,to_char(costo_producto,'999999999.99') AS costoproducto "+
							"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
							"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+							
							"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
							"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
							this.campoacceso+
							"AND osiris_productos.cobro_activo = 'true' "+
							query_tipo_busqueda;
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				float calculodeiva;
				float preciomasiva;
				float preciocondesc;
				float tomaprecio;
				float tomadescue;
				float valoriva = float.Parse(classpublic.ivaparaaplicar);							
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
									(string) lector["codProducto"] ,//0
									(string) lector["descripcion_producto"],
									(string) lector["preciopublico"],
									calculodeiva.ToString("F").PadLeft(10).Replace(",","."),
									preciomasiva.ToString("F").PadLeft(10).Replace(",","."),
									(string) lector["porcentagesdesc"],
									preciocondesc.ToString("F").PadLeft(10).Replace(",","."),
									(string) lector["tipo_unidad_producto"],
									(string) lector["descripcion_grupo_producto"],
									(string) lector["descripcion_grupo1_producto"],
									(string) lector["descripcion_grupo2_producto"],
									(string) lector["nombre_articulo"],
									(string) lector["nombre_articulo"],
									(string) lector["costoproductounitario"],
									(string) lector["porcentageutilidad"],
									(string) lector["costoproducto"],
									(string) lector["cantidadembalaje"],
									(string) lector["idgrupoproducto"],
									(string) lector["idgrupo1producto"],
									(string) lector["idgrupo2producto"],
									(bool) lector["aplicar_iva"],
									(bool) lector["cobro_activo"],
									(bool) lector["aplica_descuento"],
									(string) lector["preciopublico1"]);
					col_idproducto.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_desc_producto.SetCellDataFunc(cellr1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_empaque.SetCellDataFunc(cellrt16, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_ivaprod.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_totalprod.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_descuentoprod.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					/*col_preciocondesc.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_stock_actual.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_grupoprod.SetCellDataFunc(cellrt8, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_grupo1prod.SetCellDataFunc(cellrt9, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_grupo2prod.SetCellDataFunc(cellrt10, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_aplica_iva.SetCellDataFunc(cellrt20, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_cobro_activo.SetCellDataFunc(cellrt21, new Gtk.TreeCellDataFunc(cambia_colores_fila));*/
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((bool)lista_de_producto.Model.GetValue (iter,20)==true){
				(cell as Gtk.CellRendererText).Foreground = "blue";
			}else{
				(cell as Gtk.CellRendererText).Foreground = "black";
			}			
		}
		
		// Creando el treeview para la requisicion
		void crea_treeview_requisicion()
		{
			treeViewEngineRequisicion = new TreeStore(typeof(string), 	// Camdidad de unidades solicitadas en compras
													typeof(string),
													typeof(string),
													typeof(string),		// Embalaje de Producto
													typeof(string),		// unidades compradas en la orden de compra
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(bool),		// indica es nuevo el producto
													typeof(bool),		// bandera indicativa de autorizacion		
													typeof(bool),		// indica si el producto ya esta autorizado
													typeof(string),		// fecha de autorizacion para su compra
													typeof(string),		// guarda el numero de secuencia
													typeof(string),		// stock	
													typeof(string),		// stock
													typeof(string),		// stock
													typeof(string),		// Descripcion proveedor1						18
													typeof(string),		// Descripcion proveedor2						19
													typeof(string),		// Descripcion proveedor3						20
													typeof(string),		// Guardo el Costo unitario del producto		21
													typeof(string),		// Guardo el Costo total del producto			22
													typeof(string),		// id proveedor 1 requisicion					23
													typeof(string),		// id proveedor 2 requisicion					24
													typeof(string),		// id proveedor 3 requisicion					25
													typeof(string),		// Porcentage de ganancia						26
													typeof(bool));		// indica si actualizo algun proveedor antes de enviar a compras 	27
												
			lista_requisicion_productos.Model = treeViewEngineRequisicion;
			
			lista_requisicion_productos.RulesHint = true;
			
			TreeViewColumn col_cantidad = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_cantidad.Title = "Solicitado"; // titulo de la cabecera de la columna, si está visible
			col_cantidad.PackStart(cellr0, true);
			col_cantidad.AddAttribute (cellr0, "text", 0);
			col_cantidad.SortColumnId = (int) col_requisicion.col_cantidad;
			
			TreeViewColumn col_descripcion = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_descripcion.Title = "Descripcion"; // titulo de la cabecera de la columna, si está visible
			col_descripcion.PackStart(cellr1, true);
			col_descripcion.AddAttribute (cellr1, "text", 1);
			col_descripcion.SortColumnId = (int) col_requisicion.col_descripcion;
			col_descripcion.Resizable = true;
			cellr1.Width = 350;
									
			TreeViewColumn col_codigo_prod = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_codigo_prod.Title = "Codigo Prod."; // titulo de la cabecera de la columna, si está visible
			col_codigo_prod.PackStart(cellr2, true);
			col_codigo_prod.AddAttribute (cellr2, "text", 2);
			col_codigo_prod.SortColumnId = (int) col_requisicion.col_codigo_prod;
			
			TreeViewColumn col_precio_compra = new TreeViewColumn();
			CellRendererText cellr22 = new CellRendererText();
			col_precio_compra.Title = "Precio Prod."; // titulo de la cabecera de la columna, si está visible
			col_precio_compra.PackStart(cellr22, true);
			col_precio_compra.AddAttribute (cellr22, "text", 22);
			//col_embalaje.SortColumnId = (int) col_requisicion.col_embalaje;
			cellr22.Editable = true;
			cellr22.Edited += NumberCellEdited;
			
			TreeViewColumn col_embalaje = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_embalaje.Title = "Embalaje"; // titulo de la cabecera de la columna, si está visible
			col_embalaje.PackStart(cellr3, true);
			col_embalaje.AddAttribute (cellr3, "text", 3);
			col_embalaje.SortColumnId = (int) col_requisicion.col_embalaje;
			
			TreeViewColumn col_unidades = new TreeViewColumn();
			CellRendererText cellr4 = new CellRendererText();
			col_unidades.Title = "Unidades"; // titulo de la cabecera de la columna, si está visible
			col_unidades.PackStart(cellr4, true);
			col_unidades.AddAttribute (cellr4, "text", 4);
			col_unidades.SortColumnId = (int) col_requisicion.col_unidades;
			
			TreeViewColumn col_comprado = new TreeViewColumn();
			CellRendererText cellr5 = new CellRendererText();
			col_comprado.Title = "Uni.Compradas"; // titulo de la cabecera de la columna, si está visible
			col_comprado.PackStart(cellr5, true);
			col_comprado.AddAttribute (cellr5, "text", 5);
			col_comprado.SortColumnId = (int) col_requisicion.col_comprado;
			
			/*
			TreeViewColumn col_preciounitario = new TreeViewColumn();
			CellRendererText cellr6 = new CellRendererText();
			col_preciounitario.Title = "Precio Unitario"; // titulo de la cabecera de la columna, si está visible
			col_preciounitario.PackStart(cellr6, true);
			col_preciounitario.AddAttribute (cellr6, "text", 6);
			col_preciounitario.SortColumnId = (int) col_requisicion.col_preciounitario;
			
			TreeViewColumn col_valor_requisado = new TreeViewColumn();
			CellRendererText cellr7 = new CellRendererText();
			col_valor_requisado.Title = "$ Requisado"; // titulo de la cabecera de la columna, si está visible
			col_valor_requisado.PackStart(cellr7, true);
			col_valor_requisado.AddAttribute (cellr7, "text", 7);
			col_valor_requisado.SortColumnId = (int) col_requisicion.col_valor_requisado;*/
						
			TreeViewColumn col_orden_compra = new TreeViewColumn();
			CellRendererText cellr8 = new CellRendererText();
			col_orden_compra.Title = "Nº O.C."; // titulo de la cabecera de la columna, si está visible
			col_orden_compra.PackStart(cellr8, true);
			col_orden_compra.AddAttribute (cellr8, "text", 8);
			col_orden_compra.SortColumnId = (int) col_requisicion.col_orden_compra;
			
			TreeViewColumn col_fecha_compra = new TreeViewColumn();
			CellRendererText cellr9 = new CellRendererText();
			col_fecha_compra.Title = "Fecha O.C."; // titulo de la cabecera de la columna, si está visible
			col_fecha_compra.PackStart(cellr9, true);
			col_fecha_compra.AddAttribute (cellr9, "text", 9);
			col_fecha_compra.SortColumnId = (int) col_requisicion.col_fecha_compra;
			
			TreeViewColumn col_autorizar = new TreeViewColumn();
			CellRendererToggle cel_autorizar = new CellRendererToggle();
			col_autorizar.Title = "Autorizar"; // titulo de la cabecera de la columna, si está visible
			col_autorizar.PackStart(cel_autorizar, true);
			col_autorizar.AddAttribute (cel_autorizar, "active", 11);
			cel_autorizar.Activatable = true;
			cel_autorizar.Toggled += selecciona_fila;
			col_autorizar.SortColumnId = (int) col_requisicion.col_autorizar;
			
			TreeViewColumn col_autorizado = new TreeViewColumn();
			CellRendererToggle cel_autorizado = new CellRendererToggle();
			col_autorizado.Title = "Autorizado"; // titulo de la cabecera de la columna, si está visible
			col_autorizado.PackStart(cel_autorizado, true);
			col_autorizado.AddAttribute (cel_autorizado, "active", 12);
			cel_autorizado.Activatable = true;
			col_autorizado.SortColumnId = (int) col_requisicion.col_autorizado;
			
			TreeViewColumn col_fecha_autorizacion = new TreeViewColumn();
			CellRendererText cellr13 = new CellRendererText();
			col_fecha_autorizacion.Title = "Fec.Autorizacion"; // titulo de la cabecera de la columna, si está visible
			col_fecha_autorizacion.PackStart(cellr13, true);
			col_fecha_autorizacion.AddAttribute (cellr13, "text", 13);
			col_fecha_autorizacion.SortColumnId = (int) col_requisicion.col_fecha_autorizacion;
			
			// guarda numero de la secuencia 14
			
			TreeViewColumn col_stock_subalmacenes = new TreeViewColumn();
			CellRendererText cellr15 = new CellRendererText();
			col_stock_subalmacenes.Title = "Stock Sub-Almacenes"; // titulo de la cabecera de la columna, si está visible
			col_stock_subalmacenes.PackStart(cellr15, true);
			col_stock_subalmacenes.AddAttribute (cellr15, "text", 15);
			col_stock_subalmacenes.SortColumnId = (int) col_requisicion.col_stock_subalmacenes;
			
			TreeViewColumn col_stock_almageneral = new TreeViewColumn();
			CellRendererText cellr16 = new CellRendererText();
			col_stock_almageneral.Title = "Stock Alma.Genaral"; // titulo de la cabecera de la columna, si está visible
			col_stock_almageneral.PackStart(cellr16, true);
			col_stock_almageneral.AddAttribute (cellr16, "text", 16);
			col_stock_almageneral.SortColumnId = (int) col_requisicion.col_stock_almageneral;
			
			TreeViewColumn col_total_stock = new TreeViewColumn();
			CellRendererText cellr17 = new CellRendererText();
			col_total_stock.Title = "Total Stock"; // titulo de la cabecera de la columna, si está visible
			col_total_stock.PackStart(cellr17, true);
			col_total_stock.AddAttribute (cellr17, "text", 17);
			col_total_stock.SortColumnId = (int) col_requisicion.col_total_stock;
			
			TreeViewColumn col_proveedor1 = new TreeViewColumn();
			CellRendererText cellr18 = new CellRendererText();
			col_proveedor1.Title = "Proveedor 1"; // titulo de la cabecera de la columna, si está visible
			col_proveedor1.PackStart(cellr18, true);
			col_proveedor1.AddAttribute (cellr18, "text", 18);
			col_proveedor1.SortColumnId = (int) col_requisicion.col_proveedor1;
			
			TreeViewColumn col_proveedor2 = new TreeViewColumn();
			CellRendererText cellr19 = new CellRendererText();
			col_proveedor2.Title = "Proveedor 2"; // titulo de la cabecera de la columna, si está visible
			col_proveedor2.PackStart(cellr19, true);
			col_proveedor2.AddAttribute (cellr19, "text", 19);
			col_proveedor2.SortColumnId = (int) col_requisicion.col_proveedor2;
			
			TreeViewColumn col_proveedor3 = new TreeViewColumn();
			CellRendererText cellr20 = new CellRendererText();
			col_proveedor3.Title = "Proveedor 3"; // titulo de la cabecera de la columna, si está visible
			col_proveedor3.PackStart(cellr20, true);
			col_proveedor3.AddAttribute (cellr20, "text", 20);
			col_proveedor3.SortColumnId = (int) col_requisicion.col_proveedor3;
			
			//TreeViewColumn col_prueba = new TreeViewColumn();
			//CellRendererCombo cellr14 = new CellRendererCombo();
			//col_prueba.Title = "Fec.Autorizacion"; // titulo de la cabecera de la columna, si está visible
			//col_prueba.PackStart(cellr14, true);
			//col_prueba.AddAttribute (cellr14, "active", 14);
			//col_fecha_autorizacion.SortColumnId = (int) col_requisicion.col_fecha_autorizacion;
			//CellRendererCombo*/
			
			lista_requisicion_productos.AppendColumn(col_cantidad);				// 0
			lista_requisicion_productos.AppendColumn(col_descripcion);			// 1
			lista_requisicion_productos.AppendColumn(col_codigo_prod);			// 2
			lista_requisicion_productos.AppendColumn(col_precio_compra);		// 22
			lista_requisicion_productos.AppendColumn(col_embalaje);				// 3
			lista_requisicion_productos.AppendColumn(col_unidades);				// 4
			lista_requisicion_productos.AppendColumn(col_comprado);				// 5
			//lista_requisicion_productos.AppendColumn(col_preciounitario);		// 6
			//lista_requisicion_productos.AppendColumn(col_valor_requisado);	// 7
			lista_requisicion_productos.AppendColumn(col_orden_compra);			// 8
			lista_requisicion_productos.AppendColumn(col_fecha_compra);			// 9
			// nuevo producto a requiscion										// 10
			lista_requisicion_productos.AppendColumn(col_autorizar);			// 11
			lista_requisicion_productos.AppendColumn(col_autorizado);			// 12
			lista_requisicion_productos.AppendColumn(col_fecha_autorizacion);	// 13
			// almacena el numero de la secuencia								// 14
			lista_requisicion_productos.AppendColumn(col_stock_subalmacenes);	// 15
			lista_requisicion_productos.AppendColumn(col_stock_almageneral);	// 16
			lista_requisicion_productos.AppendColumn(col_total_stock);			// 17
			lista_requisicion_productos.AppendColumn(col_proveedor1);			// 18
			lista_requisicion_productos.AppendColumn(col_proveedor2);			// 19
			lista_requisicion_productos.AppendColumn(col_proveedor3);			// 20
		}
		
		void NumberCellEdited (object o, EditedArgs args)
		{
			TreeModel model;
			TreeIter iter;	
			bool esnumerico = false;
			int var_paso = 0;
			int largo_variable = args.NewText.ToString().Length;
			string toma_variable = args.NewText.ToString();
			
			treeViewEngineRequisicion.GetIter (out iter, new Gtk.TreePath (args.Path));
			
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
				treeViewEngineRequisicion.SetValue(iter,22,float.Parse((string) args.NewText).ToString("F"));
				treeViewEngineRequisicion.SetValue(iter,21,float.Parse(Convert.ToString(float.Parse((string) args.NewText) / float.Parse((string) treeViewEngineRequisicion.GetValue(iter, 3)))).ToString("F"));
				Console.WriteLine(treeViewEngineRequisicion.GetValue(iter, 21));
			}
 		}
		
		// Cuando seleccion campos para la autorizacion de compras  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			if ((bool) this.checkbutton_nueva_requisicion.Active == false){
				TreeIter iter;
				TreePath path = new TreePath (args.Path);
				if (this.lista_requisicion_productos.Model.GetIter (out iter, path)){					
					if ((bool) this.lista_requisicion_productos.Model.GetValue(iter,12) == false){
						bool old = (bool) this.lista_requisicion_productos.Model.GetValue(iter,11);
						this.lista_requisicion_productos.Model.SetValue(iter,11,!old);
					}else{
						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"No puede autorizar este producto ya estaba autorizado...");
						msgBox.Run ();		msgBox.Destroy();
					}
				}
			}	
		}
		
		enum col_requisicion
		{
			col_cantidad,	
			col_descripcion,
			col_codigo_prod,
			col_embalaje,
			col_unidades,
			col_comprado,
			col_orden_compra,
			col_fecha_compra,
			col_autorizar,
			col_autorizado,
			col_fecha_autorizacion,
			col_stock_subalmacenes,
			col_stock_almageneral,
			col_total_stock,
			col_proveedor1,
			col_proveedor2,
			col_proveedor3
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
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(bool),
													typeof(bool),
													typeof(bool),
													typeof(string));
				lista_de_producto.Model = treeViewEngineBusca2;
			
				lista_de_producto.RulesHint = true;
			
				//lista_de_producto.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono producto*/
				lista_de_producto.MoveCursor += on_packproductos_clicked;
				lista_de_producto.CursorChanged += on_packproductos_clicked;
				
				col_idproducto = new TreeViewColumn();
				cellr0 = new CellRendererText();
				col_idproducto.Title = "ID Producto";
				col_idproducto.PackStart(cellr0, true);
				col_idproducto.AddAttribute (cellr0, "text", 0);
				col_idproducto.SortColumnId = (int) Column_prod.col_idproducto;
			
				col_desc_producto = new TreeViewColumn();
				cellr1 = new CellRendererText();
				col_desc_producto.Title = "Descripcion de Producto";
				col_desc_producto.PackStart(cellr1, true);
				col_desc_producto.AddAttribute (cellr1, "text", 1);
				col_desc_producto.SortColumnId = (int) Column_prod.col_desc_producto;
				col_desc_producto.Resizable = true;
				cellr1.Width = 380;
				//cellr0.Editable = true;   // Permite edita este campo
            
				col_empaque = new TreeViewColumn();
				cellrt16 = new CellRendererText();
				col_empaque.Title = "Empaque/Pack";
				col_empaque.PackStart(cellrt16, true);
				col_empaque.AddAttribute (cellrt16, "text", 16);
				col_empaque.SortColumnId = (int) Column_prod.col_empaque;
            
				col_ivaprod = new TreeViewColumn();
				cellrt3 = new CellRendererText();
				col_ivaprod.Title = "I.V.A.";
				col_ivaprod.PackStart(cellrt3, true);
				col_ivaprod.AddAttribute (cellrt3, "text", 3);
				col_ivaprod.SortColumnId = (int) Column_prod.col_ivaprod;
            
				col_totalprod = new TreeViewColumn();
				cellrt4 = new CellRendererText();
				col_totalprod.Title = "Total";
				col_totalprod.PackStart(cellrt4, true);
				col_totalprod.AddAttribute (cellrt4, "text", 4);
				col_totalprod.SortColumnId = (int) Column_prod.col_totalprod;
            
				col_descuentoprod = new TreeViewColumn();
				cellrt5 = new CellRendererText();
				col_descuentoprod.Title = "% Descuento";
				col_descuentoprod.PackStart(cellrt5, true);
				col_descuentoprod.AddAttribute (cellrt5, "text", 5);
				col_descuentoprod.SortColumnId = (int) Column_prod.col_descuentoprod;
      
				col_preciocondesc = new TreeViewColumn();
				cellrt6 = new CellRendererText();
				col_preciocondesc.Title = "Precio con Desc.";
				col_preciocondesc.PackStart(cellrt6, true);
				col_preciocondesc.AddAttribute (cellrt6, "text", 6);
				col_preciocondesc.SortColumnId = (int) Column_prod.col_preciocondesc;
				
				col_stock_actual = new TreeViewColumn();
				cellrt7 = new CellRendererText();
				col_stock_actual.Title = "Stock Almacen";
				col_stock_actual.PackStart(cellrt7, true);
				col_stock_actual.AddAttribute (cellrt7, "text", 7);
				col_stock_actual.SortColumnId = (int) Column_prod.col_stock_actual;
            	            	
				col_grupoprod = new TreeViewColumn();
				cellrt8 = new CellRendererText();
				col_grupoprod.Title = "Grupo Producto";
				col_grupoprod.PackStart(cellrt8, true);
				col_grupoprod.AddAttribute (cellrt8, "text", 8);
				col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
            
				col_grupo1prod = new TreeViewColumn();
				cellrt9 = new CellRendererText();
				col_grupo1prod.Title = "Grupo1 Producto";
				col_grupo1prod.PackStart(cellrt9, true);
				col_grupo1prod.AddAttribute (cellrt9, "text", 9);
				col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
                        
				col_grupo2prod = new TreeViewColumn();
				cellrt10 = new CellRendererText();
				col_grupo2prod.Title = "Grupo2 Producto";
				col_grupo2prod.PackStart(cellrt10, true);
				col_grupo2prod.AddAttribute (cellrt10, "text", 10);
				col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;
								
				lista_de_producto.AppendColumn(col_idproducto);  // 0
				lista_de_producto.AppendColumn(col_desc_producto); // 1
				lista_de_producto.AppendColumn(col_empaque);	//2
				//lista_de_producto.AppendColumn(col_ivaprod);	// 3
				//lista_de_producto.AppendColumn(col_totalprod); // 4
				//lista_de_producto.AppendColumn(col_descuentoprod); //5
				//lista_de_producto.AppendColumn(col_preciocondesc); // 6
				//lista_de_producto.AppendColumn(col_stock_actual); // 7
				lista_de_producto.AppendColumn(col_grupoprod);	//8
				lista_de_producto.AppendColumn(col_grupo1prod);	//9
				lista_de_producto.AppendColumn(col_grupo2prod);	//10
			}
		}
		
		enum Column_prod
		{
			col_idproducto,			col_desc_producto,
			col_empaque,			col_ivaprod,
			col_totalprod,			col_descuentoprod,
			col_preciocondesc,		col_grupoprod,
			col_grupo1prod,			col_grupo2prod,
			col_nom_art,			col_nom_gen,
			col_costoprod_uni,		col_porc_util,
			col_costo_prod,			col_stock_actual,
			col_cant_embalaje,
			col_id_gpo_prod,		col_id_gpo_prod1,
			col_id_gpo_prod2,		col_aplica_iva,
			col_cobro_activo,		col_aplica_desc
		}
		
		void on_packproductos_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
				entry_precio.Text = (string) model.GetValue(iterSelected, 15);
			}
		}
		
		void on_button_busca_proveedores1_clicked(object sender, EventArgs args)
		{
			busca_selecciona_proveedores(1);
		}
		
		void on_button_busca_proveedores2_clicked(object sender, EventArgs args)
		{
			busca_selecciona_proveedores(2);
		}
		
		void on_button_busca_proveedores3_clicked(object sender, EventArgs args)
		{
			busca_selecciona_proveedores(3);
		}
		
		void busca_selecciona_proveedores(int tipobusqueda){
			//busqueda = "proveedores";
			this.tipobusquedaprove = tipobusqueda;
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_proveedores_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			button_selecciona.Clicked += new EventHandler(on_selecciona_proveedor_clicked);
			//checkbutton_proveedor_nuevo.Active = false;
			crea_treeview_proveedores();
			button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
		}
		
		void crea_treeview_proveedores()
		{
			treeViewEngineproveedores = new TreeStore(typeof(int),//0
													typeof(string),//1
													typeof(string),//2
													typeof(string),//3
													typeof(string),//4
													typeof(string),//5
													typeof(string),//6
													typeof(string),//7
													typeof(string),//8
													typeof(string),//9
													typeof(int), // 10
													typeof(bool),//11
													typeof(string));//12
												
			lista_de_busqueda.Model = treeViewEngineproveedores;
			
			lista_de_busqueda.RulesHint = true;
				
			lista_de_busqueda.RowActivated += on_selecciona_proveedor_clicked;  // Doble click
			
			TreeViewColumn col_idproveedor = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idproveedor.Title = "ID Proveedores"; // titulo de la cabecera de la columna, si está visible
			col_idproveedor.PackStart(cellr0, true);
			col_idproveedor.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1
			col_idproveedor.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_idproveedor.SortColumnId = (int) Col_proveedores.col_idproveedor;
			
			TreeViewColumn col_proveedor = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_proveedor.Title = "Proveedores";
			col_proveedor.PackStart(cellrt1, true);
			col_proveedor.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
			col_proveedor.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_proveedor.SortColumnId = (int) Col_proveedores.col_proveedor;
			
			TreeViewColumn col_calle = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_calle.Title = "Calle";
			col_calle.PackStart(cellrt2, true);
			col_calle.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 3
			col_calle.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_calle.SortColumnId = (int) Col_proveedores.col_calle;
			
			TreeViewColumn col_colonia = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_colonia.Title = "Colonia";
			col_colonia.PackStart(cellrt3, true);
			col_colonia.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 4
			col_colonia.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_colonia.SortColumnId = (int) Col_proveedores.col_colonia;
			
            TreeViewColumn col_municipio = new TreeViewColumn();
            CellRendererText cellrt4 = new CellRendererText();
            col_municipio.Title = "Municipio";
            col_municipio.PackStart(cellrt4, true);
			col_municipio.AddAttribute(cellrt4,"text", 4); // la siguiente columna será 5
			col_municipio.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_municipio.SortColumnId = (int) Col_proveedores.col_municipio;
			
            TreeViewColumn col_estado = new TreeViewColumn();
            CellRendererText cellrt5 = new CellRendererText();
            col_estado.Title = "Estado";
            col_estado.PackStart(cellrt5, true);
            col_estado.AddAttribute(cellrt5,"text", 5); // la siguiente columna será 6
            col_estado.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_estado.SortColumnId = (int) Col_proveedores.col_estado;
			
            TreeViewColumn col_telefono = new TreeViewColumn();
            CellRendererText cellrt6 = new CellRendererText();
            col_telefono.Title = "Telefono";
            col_telefono.PackStart(cellrt6, true);
            col_telefono.AddAttribute(cellrt6,"text", 6); // la siguiente columna será 7
            col_telefono.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
            col_telefono.SortColumnId = (int) Col_proveedores.col_telefono;
            
            TreeViewColumn col_contacto = new TreeViewColumn();
            CellRendererText cellrt7 = new CellRendererText();
            col_contacto.Title = "Contacto";
            col_contacto.PackStart(cellrt7, true);
            col_contacto.AddAttribute(cellrt7,"text", 7);// la siguiente columna será 8
            col_contacto.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_contacto.SortColumnId = (int) Col_proveedores.col_contacto;
			
            TreeViewColumn col_cp = new TreeViewColumn();
            CellRendererText cellrt8 = new CellRendererText();
            col_cp.Title = "Codigo Postal";
            col_cp.PackStart(cellrt8, true);
            col_cp.AddAttribute(cellrt8,"text", 8);// la siguiente columna será 9
            col_cp.SetCellDataFunc(cellrt8, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
            col_cp.SortColumnId = (int) Col_proveedores.col_cp;
			
            TreeViewColumn col_web = new TreeViewColumn();
            CellRendererText cellrt9 = new CellRendererText();
            col_web.Title = "Pag. Web";
            col_web.PackStart(cellrt9, true);
            col_web.AddAttribute(cellrt9,"text", 9);// la siguiente columna será 10
            col_web.SetCellDataFunc(cellrt9, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
            col_web.SortColumnId = (int) Col_proveedores.col_web;
            		           
			lista_de_busqueda.AppendColumn(col_idproveedor);
			lista_de_busqueda.AppendColumn(col_proveedor);
			lista_de_busqueda.AppendColumn(col_calle);
			lista_de_busqueda.AppendColumn(col_colonia);
			lista_de_busqueda.AppendColumn(col_municipio);
			lista_de_busqueda.AppendColumn(col_estado);
			lista_de_busqueda.AppendColumn(col_telefono);
			lista_de_busqueda.AppendColumn(col_contacto);
			lista_de_busqueda.AppendColumn(col_cp);
			lista_de_busqueda.AppendColumn(col_web);
						
		}
		
		enum Col_proveedores
		{
			col_idproveedor,
			col_proveedor,
			col_calle,
			col_colonia,
			col_municipio,
			col_estado,
			col_telefono,
			col_contacto,
			col_cp,
			col_web
		}
		
		void on_llena_lista_proveedores_clicked(object sender, EventArgs args)
		{
			llenando_lista_de_proveedores();
		}
		
		void llenando_lista_de_proveedores()
		{
			treeViewEngineproveedores.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				if ((string) entry_expresion.Text.ToUpper() == "*"){
					comando.CommandText = "SELECT descripcion_proveedor,direccion_proveedor,rfc_proveedor,curp_proveedor, "+
								"colonia_proveedor,municipio_proveedor,estado_proveedor,telefono1_proveedor, "+ 
								"telefono2_proveedor,celular_proveedor,cp_proveedor, proveedor_activo, "+
								"id_proveedor,contacto1_proveedor,mail_proveedor,pagina_web_proveedor,"+
								"osiris_erp_proveedores.id_forma_de_pago, descripcion_forma_de_pago AS descripago "+
								"FROM osiris_erp_proveedores, osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_proveedores.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
								"ORDER BY descripcion_proveedor;";															
				}else{
					comando.CommandText = "SELECT descripcion_proveedor,direccion_proveedor,rfc_proveedor,curp_proveedor, "+
								"colonia_proveedor,municipio_proveedor,estado_proveedor,telefono1_proveedor, "+ 
								"telefono2_proveedor,celular_proveedor,cp_proveedor, proveedor_activo, "+
								"id_proveedor,contacto1_proveedor,mail_proveedor,pagina_web_proveedor, "+
								"osiris_erp_proveedores.id_forma_de_pago, descripcion_forma_de_pago AS descripago "+
								"FROM osiris_erp_proveedores, osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_proveedores.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
								"AND descripcion_proveedor LIKE '%"+(string) entry_expresion.Text.ToUpper()+"%' "+
								"ORDER BY descripcion_proveedor;";
				}
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){	
					treeViewEngineproveedores.AppendValues ((int) lector["id_proveedor"],//0
													(string) lector["descripcion_proveedor"],//1
													(string) lector["direccion_proveedor"],//2
													(string) lector["colonia_proveedor"],//3
													(string) lector["municipio_proveedor"],//4
													(string) lector["estado_proveedor"],//5
													(string) lector["telefono1_proveedor"],//6
													(string) lector["contacto1_proveedor"],//7
													(string) lector["cp_proveedor"],//8
													(string) lector["pagina_web_proveedor"],//9
													(int) lector["id_forma_de_pago"],//10
													(bool) lector["proveedor_activo"], // 11
													(string) lector["descripago"]);//12
					
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_selecciona_proveedor_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iter;
			
			TreeModel model1;
			TreeIter iter1;
			
			if (lista_de_busqueda.Selection.GetSelected(out model, out iter)){
				if (this.lista_requisicion_productos.Selection.GetSelected(out model1, out iter1)){
	 				if(this.tipobusquedaprove == 1){
	 					this.lista_requisicion_productos.Model.SetValue(iter1,18,(string) model.GetValue(iter, 1));
	 					this.lista_requisicion_productos.Model.SetValue(iter1,23,Convert.ToString((int) model.GetValue(iter, 0)));
	 					this.lista_requisicion_productos.Model.SetValue(iter1,27,(bool) true);
	 				}
	 				if(this.tipobusquedaprove == 2){
	 					this.lista_requisicion_productos.Model.SetValue(iter1,19,(string) model.GetValue(iter, 1));
	 					this.lista_requisicion_productos.Model.SetValue(iter1,24,Convert.ToString((int) model.GetValue(iter, 0)));
	 					this.lista_requisicion_productos.Model.SetValue(iter1,27,(bool) true);
	 				}
	 				if(this.tipobusquedaprove == 3){
	 					this.lista_requisicion_productos.Model.SetValue(iter1,20,(string) model.GetValue(iter, 1));
	 					this.lista_requisicion_productos.Model.SetValue(iter1,25,Convert.ToString((int) model.GetValue(iter, 0)));
	 					this.lista_requisicion_productos.Model.SetValue(iter1,27,(bool) true);
	 				}
	 			}
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
		
		void cambia_colores_proveedor(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//if ((bool) lista_de_busqueda.Model.GetValue(iter,10) == false)
			//{(cell as Gtk.CellRendererText).Foreground = "darkgreen";		}
		}
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_requisicion(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llenado_de_requisicion();				
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace")
			{
				args.RetVal = true;
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
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
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;		
				llenando_lista_de_proveedores();			
			}
		}
				
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}