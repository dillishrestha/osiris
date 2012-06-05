//////////////////////////////////////////////////////////////////////
// created on 28/03/2008 at 02:41 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. (Modificaciones y Ajustes)
//				  Tec. Homero Montoya Galvan (Programaion)
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
using System.IO;
using Gtk;
using Npgsql;
using System.Data;
using Glade;

namespace osiris
{
	public class factura_orden_compra
	{
		[Widget] Gtk.Button button_salir = null;		
		//Declarando ventana de captura de facturas de ordenes de compra
		[Widget] Gtk.Window captura_facturas_orden_compra = null;
		[Widget] Gtk.CheckButton checkbutton_factura_sin_orden = null;
		[Widget] Gtk.Entry entry_orden_de_compra = null;
		[Widget] Gtk.Button button_selecciona_ordencompra = null;
		[Widget] Gtk.Button button_busca_orden_compra = null;
		[Widget] Gtk.Entry entry_fecha_orden_compra = null;
		[Widget] Gtk.Entry entry_estatus_oc = null;
		[Widget] Gtk.Entry entry_id_quien_hizo = null;
		[Widget] Gtk.Entry entry_num_factura_proveedor = null;
		[Widget] Gtk.Entry entry_ano_fechafactura = null;
		[Widget] Gtk.Entry entry_mes_fechafactura = null;
		[Widget] Gtk.Entry entry_dia_fechafactura = null;
		[Widget] Gtk.Entry entry_id_proveedor = null;
		[Widget] Gtk.Entry entry_nombre_proveedor = null;
		[Widget] Gtk.Button button_busca_proveedor = null;
		[Widget] Gtk.Entry entry_direccion_proveedor = null;
		[Widget] Gtk.Entry entry_tel_proveedor = null;
		[Widget] Gtk.Entry entry_contacto_proveedor = null;
		[Widget] Gtk.Entry entry_formapago = null;
		[Widget] Gtk.TreeView lista_productos_a_recibir = null;
		
		[Widget] Gtk.Entry entry_producto = null;
		[Widget] Gtk.Button button_busca_producto = null;
		[Widget] Gtk.Button button_quitar_producto = null;
		[Widget] Gtk.Button button_guardar = null;
		[Widget] Gtk.Button button_reporte = null;
		
		[Widget] Gtk.Statusbar statusbar_captura_factura_orden_compra = null;
		
		//Declaracion de ventana de busqueda de productos
		[Widget] Gtk.Window busca_producto = null;
		[Widget] Gtk.Button button_buscar_busqueda = null;
		[Widget] Gtk.Button button_selecciona = null;
		[Widget] Gtk.Entry entry_expresion = null;
		[Widget] Gtk.RadioButton radiobutton_nombre = null;
		[Widget] Gtk.RadioButton radiobutton_codigo = null;
		[Widget] Gtk.Entry entry_lote = null;
		[Widget] Gtk.Entry entry_caducidad = null;
		[Widget] Gtk.Entry entry_cantidad_aplicada = null;
		[Widget] Gtk.Entry entry_embalaje_pack = null;
		[Widget] Gtk.Entry entry_producto_proveedor = null;
		
		[Widget] Gtk.Entry entry_codprod_proveedor = null;	
		[Widget] Gtk.Entry entry_precio = null;
		[Widget] Gtk.ComboBox combobox_tipo_unidad2 = null;
		//[Widget] Gtk
		[Widget] Gtk.TreeView lista_de_producto = null;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombreempleado;
		string connectionString;
		string nombrebd;
		string tipounidadproducto = " ";
		int idsubalmacen = 1;
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();
				
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
				
		TreeStore treeViewEngineListaProdRequi;	// Lista de proctos que se van a comprar
		TreeStore treeViewEngineBusca2;
		
		// declaro treevie de productos para la requisicion
		TreeViewColumn col_00;
		TreeViewColumn col_01;
		TreeViewColumn col_02;
		TreeViewColumn col_03;
		TreeViewColumn col_04;
		TreeViewColumn col_05;
		TreeViewColumn col_06;
		TreeViewColumn col_07;
		TreeViewColumn col_08;
		TreeViewColumn col_09;
		TreeViewColumn col_10;
		TreeViewColumn col_11;
		TreeViewColumn col_12;
		TreeViewColumn col_13;
		TreeViewColumn col_14;
		TreeViewColumn col_15;
		
		//declaracion de columnas y celdas de treeview de busqueda
		TreeViewColumn col_idproducto;			CellRendererText cellrt0;
		TreeViewColumn col_desc_producto;		CellRendererText cellrt1;
		TreeViewColumn col_precioprod;			CellRendererText cellrt2;
		TreeViewColumn col_ivaprod;				CellRendererText cellrt3;
		TreeViewColumn col_totalprod;			CellRendererText cellrt4;
		TreeViewColumn col_descuentoprod;		CellRendererText cellrt5;
		TreeViewColumn col_preciocondesc;		CellRendererText cellrt6;
		TreeViewColumn col_grupoprod;			CellRendererText cellrt7;
		TreeViewColumn col_grupo1prod;			CellRendererText cellrt8;
		TreeViewColumn col_grupo2prod;			CellRendererText cellrt9;
		CellRendererText cellrt10;
		CellRendererText cellrt11;
		TreeViewColumn col_costoprod_uni;		CellRendererText cellrt12;
		CellRendererText cellrt13;
		CellRendererText cellrt14;
		TreeViewColumn col_embalajeprod;		CellRendererText cellrt15;
		//CellRendererCombo cellrt15;
		TreeViewColumn col_aplica_iva;			CellRendererText cellrt19;
		TreeViewColumn col_cobro_activo;		CellRendererText cellrt20;
		
		Gtk.ListStore cell_combox_store;
		
		public factura_orden_compra(string LoginEmp_,string nombreempleado_,string nombrebd_)
		{
			LoginEmpleado = LoginEmp_;
			nombreempleado = nombreempleado_;
			
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "captura_facturas_orden_compra", null);
			gxml.Autoconnect(this);
			captura_facturas_orden_compra.Show();			
			captura_facturas_orden_compra.SetPosition(WindowPosition.Center);	// centra la ventana en la pantalla
			
			Pango.FontDescription fontdesc = Pango.FontDescription.FromString("Arial 10");  // Cambia el tipo de Letra
			fontdesc.Weight = Pango.Weight.Bold; // Letra Negrita			
			
			entry_orden_de_compra.KeyPressEvent += onKeyPressEvent_enter_ordencompra;
			button_selecciona_ordencompra.Clicked += new EventHandler(on_button_selecciona_ordencompra_clicked);
			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_busca_proveedor.Clicked += new EventHandler(on_busca_proveedores_clicked);
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			checkbutton_factura_sin_orden.Clicked += new EventHandler(on_checkbutton_factura_sin_orden_clicked);
			button_guardar.Clicked += new EventHandler(on_button_guardar_clicked);
			button_reporte.Clicked += new EventHandler(on_button_reporte_clicked);
			button_quitar_producto.Clicked += new EventHandler(on_button_quitar_producto_clicked);
						
			entry_num_factura_proveedor.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
			entry_orden_de_compra.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
			entry_num_factura_proveedor.ModifyFont(fontdesc);  // Arial y Negrita
			entry_orden_de_compra.ModifyFont(fontdesc);  // Arial y Negrita
			
			entry_producto.Sensitive = false;
			button_busca_producto.Sensitive = false;
			button_quitar_producto.Sensitive = false;
			button_busca_proveedor.Sensitive = false;
			button_guardar.Sensitive = false;
			
			entry_dia_fechafactura.Text = DateTime.Now.ToString("dd");
			entry_mes_fechafactura.Text = DateTime.Now.ToString("MM");
			entry_ano_fechafactura.Text = DateTime.Now.ToString("yyyy");
			crea_treeview_capturafactura();
						
			statusbar_captura_factura_orden_compra.Pop(0);
			statusbar_captura_factura_orden_compra.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+nombreempleado);
			statusbar_captura_factura_orden_compra.HasResizeGrip = false;
			
			cell_combox_store = new ListStore(typeof (string));
			cell_combox_store.AppendValues (" ");
			cell_combox_store.AppendValues ("PIEZA");
			cell_combox_store.AppendValues ("KILO");
			cell_combox_store.AppendValues ("LITRO");
			cell_combox_store.AppendValues ("GRAMO");
			cell_combox_store.AppendValues ("METRO");
			cell_combox_store.AppendValues ("CENTIMETRO");
			cell_combox_store.AppendValues ("CAJA");
			cell_combox_store.AppendValues ("PULGADA");
			cell_combox_store.AppendValues ("FRASCO");
			cell_combox_store.AppendValues ("GALON");
			cell_combox_store.AppendValues ("BOLSA");
			cell_combox_store.AppendValues ("BOTE");
		}
			
		// Ademas controla la tecla ENTRER para ver el procedimiento
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione
		void onKeyPressEvent_enter_ordencompra(object o, Gtk.KeyPressEventArgs args)
		{
			Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llenado_orden_de_compra();
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}
		
		void on_button_reporte_clicked(object sender, EventArgs args)
		{
			new osiris.rpt_conceptos_proveedores();
		}
		
		void on_button_quitar_producto_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
 			if (lista_productos_a_recibir.Selection.GetSelected(out model, out iterSelected)){
 				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta quitar el producto "+(string) lista_productos_a_recibir.Model.GetValue (iterSelected,9));
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
					treeViewEngineListaProdRequi.Remove (ref iterSelected);
				}
			}
		}
		
		void on_button_guardar_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
			MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro ingresar la factura?");
			ResponseType miResultado = (ResponseType)
			msgBox.Run ();				msgBox.Destroy();
	 		if (miResultado == ResponseType.Yes){
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
			    // Verifica que la base de datos este conectada
			    try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					TreeIter iter;
					if (treeViewEngineListaProdRequi.GetIterFirst (out iter)){
						// verificando la 
						if((bool) checkbutton_factura_sin_orden.Active == false){
							// Actualizar la orden de compra
							comando.CommandText = "UPDATE osiris_erp_requisicion_deta( "+
			 									"id_producto, "+
			 									"cantidad_comprada, "+
			 									"id_quien_compro, "+
			 									"fechahora_compra, "+
			 									"id_quien_recibio," +
			 									"fechahora_recibido," +
			 									"recibido," +
			 									"id_almacen," +
			 									"id_proveedor," +
			 									"numero_factura_proveedor," +
			 									"costo_producto," +
			 									"costo_por_unidad," +
			 									"precio_producto_publico," +
			 									"porcentage_ganancia," +
			 									"cantidad_de_embalaje" +
			 									"autorizada," +
			 									"id_quien_autorizo," +
			 									"id_tipo_admisiones," +
			 									"factura_sin_oc," +
			 									"id_producto_proveedor," +
			 									"lote_producto," +
			 									"caducidad_producto) "+
			 									"VALUES ('";
 												//int.Parse((string) lista_de_honorarios.Model.GetValue(iter,0))+"','"+
 												//decimal.Parse((string) lista_de_honorarios.Model.GetValue(iter,3))+"','"+
 												//folioservicio+"','"+
 												//DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','LoginEmpleado+"');";
		 					//comando.ExecuteNonQuery();
					    	//comando.Dispose();							
						}else{
							comando.CommandText = "INSERT INTO osiris_erp_factura_compra_enca(" +
										//"numero_orden_compra," +
										//"fechahora_orden_compra," +
										"id_quien_creo," +
										//"cancelado," +
										//"fechahora_cancelado," +
										"id_proveedor," +
										"factura_sin_orden_compra," +
										//"id_secuencia," +
										"numero_factura_proveedor," +
										"fecha_factura," +
										//"id_quien_cancelo," +
										"fechahora_creacion," +
										"numero_serie_cfd," +
										"ano_aprobacion_cfd," +
										"numero_aprobacion_cfd) " +
										"VALUES " +
										"('"+LoginEmpleado+"','"+
											entry_id_proveedor.Text.Trim()+"','"+
											checkbutton_factura_sin_orden.Active+"','"+
											entry_num_factura_proveedor.Text.Trim().ToUpper()+"','"+
											entry_ano_fechafactura.Text.Trim()+"-"+entry_mes_fechafactura.Text.Trim()+"-"+entry_dia_fechafactura.Text.Trim()+"','"+
											DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
										"''," +
										"''," +
										"'')";
							Console.WriteLine(comando.CommandText);
							comando.ExecuteNonQuery();
						    comando.Dispose();
							
							comando.CommandText = "INSERT INTO osiris_erp_requisicion_deta( "+
			 									"id_producto,"+
			 									"cantidad_solicitada,"+
			 									"cantidad_comprada," +
			 									"cantidad_recibida,"+
			 									"comprado,"+
			 									"recibido," +
			 									"id_proveedor," +
			 									"numero_factura_proveedor," +
			 									"costo_producto," +
			 									"costo_por_unidad, " +									
			 									"precio_producto_publico," +
												"porcentage_ganancia," +
												"cantidad_de_embalaje," +
												"id_quien_compro," +
			 									"fechahora_compra," +
			 									"id_quien_recibio," +
			 									"fechahora_recibido," +
			 									"autorizada," +
			 									"id_quien_autorizo," +
			 									"fechahora_autorizado," +
												"descripcion_producto_proveedor,"+
			 									"id_producto_proveedor," +
												"tipo_unidad_producto,"+
			 									"factura_sin_orden_compra," +
			 									"precio_costo_prov_selec," +
			 									"lote_producto," +
			 									"caducidad_producto," +
			 									"costo_producto_osiris," +
			 									"cantidad_de_embalaje_osiris) "+
			 									"VALUES ('"+												
												lista_productos_a_recibir.Model.GetValue(iter,10).ToString().Trim()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,3).ToString().Trim()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,3).ToString().Trim()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,5).ToString().Trim()+"','"+
												"true','"+
												"true','"+
												entry_id_proveedor.Text.Trim()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,1).ToString().Trim().ToUpper()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,6).ToString().Trim()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,7).ToString().Trim()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,8).ToString().Trim()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,16).ToString().Trim()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,4).ToString().Trim()+"','"+
												LoginEmpleado+"','"+
												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
												LoginEmpleado+"','"+
												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
												"true','"+
												LoginEmpleado+"','"+
												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,11)+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,12)+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,15).ToString().Trim()+"','"+
												"true','"+
												lista_productos_a_recibir.Model.GetValue(iter,6)+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,13)+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,14)+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,17)+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,18)+
												"');";
							Console.WriteLine(comando.CommandText);
		 					comando.ExecuteNonQuery();
					    	comando.Dispose();
							
							// Verificando que el producto se encuentre en el catalogo del almacen general
							// si existe el producto agrega la cantidad capturada
							// sino agrega el producto al catalogo
							NpgsqlConnection conexion1;
							conexion1 = new NpgsqlConnection (connectionString+nombrebd);
			 				try{
								conexion1.Open ();
								NpgsqlCommand comando1; 
								comando1 = conexion.CreateCommand();
								comando1.CommandText = "SELECT id_producto,id_almacen,stock FROM osiris_catalogo_almacenes "+
													"WHERE id_almacen = '"+idsubalmacen.ToString()+"' "+
													"AND eliminado = 'false' "+														
													"AND id_producto = '"+(string) lista_productos_a_recibir.Model.GetValue(iter,10).ToString().Trim()+"' ;";
								//Console.WriteLine(comando.CommandText);
								NpgsqlDataReader lector1 = comando1.ExecuteReader ();
								if(lector1.Read()){
									//Console.WriteLine("if ");
									NpgsqlConnection conexion2; 
									conexion2 = new NpgsqlConnection (connectionString+nombrebd);
			 						try{
										conexion2.Open ();
										NpgsqlCommand comando2;
										comando2 = conexion2.CreateCommand();
										// verificando que asigne un paciente para que la pre-solicitud se pueda surtir
										
										comando2.CommandText = "UPDATE osiris_catalogo_almacenes SET stock  = stock + '"+(string) lista_productos_a_recibir.Model.GetValue(iter,5).ToString().Trim()+"',"+
																//"historial_surtido_material = historial_surtido_material || '"+LoginEmpleado+" "+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n',"+
																"fechahora_ultimo_surtimiento = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
																"WHERE id_almacen = '"+idsubalmacen.ToString()+"' "+
																"AND id_producto = '"+(string) lista_productos_a_recibir.Model.GetValue(iter,10).ToString().Trim()+"' ;";
										//Console.WriteLine(comando1.CommandText);
										comando2.ExecuteNonQuery();
										
										comando2.Dispose();
										conexion2.Close();
									}catch (NpgsqlException ex){
							   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error, 
													ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
										msgBoxError.Run ();
									}
									conexion2.Close();																				
								}else{
									//Console.WriteLine("else"+idalmacenorigen);
									NpgsqlConnection conexion2;
									conexion2 = new NpgsqlConnection (connectionString+nombrebd);
			 						try{
										conexion2.Open ();
										NpgsqlCommand comando2; 
										comando2 = conexion2.CreateCommand();
										comando2.CommandText = "INSERT INTO osiris_catalogo_almacenes("+
																"id_almacen,"+
																"id_producto,"+
																"stock,"+
																"id_quien_creo,"+
																"fechahora_alta,"+
																"fechahora_ultimo_surtimiento)"+
																"VALUES ('"+
																idsubalmacen.ToString()+"','"+
																(string) lista_productos_a_recibir.Model.GetValue(iter,10).ToString().Trim()+"','"+
																(string) lista_productos_a_recibir.Model.GetValue(iter,5).ToString().Trim()+"','"+
																LoginEmpleado+"','"+
																DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');";
										comando2.ExecuteNonQuery();
										comando2.Dispose();																							
									}catch (NpgsqlException ex){
							   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error, 
													ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
										msgBoxError.Run ();
										msgBoxError.Destroy();
									}
									conexion2.Close();																																															
								}									
							}catch (NpgsqlException ex){
							   	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error, 
													ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
								msgBoxError.Destroy();
							}
							conexion1.Close();
							
							
							// ###################
							// Verificando en el catalogo general de proveedores
							// la verificacion se realiza mediante codigo del producto
							/*
							NpgsqlConnection conexion3;
							conexion3 = new NpgsqlConnection (connectionString+nombrebd);
			 				try{
								conexion3.Open ();
								NpgsqlCommand comando3; 
								comando3 = conexion.CreateCommand();
								comando3.CommandText = "SELECT codigo_producto_proveedor FROM osiris_catalogo_productos_proveedores "+
													"WHERE id_proveedor = '"+entry_id_proveedor.Text.ToString()+"' "+
													"AND eliminado = 'false' "+														
													"AND codigo_producto_proveedor = '"+(string) lista_productos_a_recibir.Model.GetValue(iter,12).ToString().Trim()+"' ;";
								//Console.WriteLine(comando.CommandText);
								NpgsqlDataReader lector3 = comando3.ExecuteReader ();
								if(!lector3.Read()){
									NpgsqlConnection conexion4;
									conexion4 = new NpgsqlConnection (connectionString+nombrebd);
			 						try{
										conexion4.Open ();
										NpgsqlCommand comando4; 
										comando4 = conexion4.CreateCommand();
										comando4.CommandText = "INSERT INTO osiris_catalogo_productos_proveedores("+
																"id_almacen,"+
																"id_producto,"+
																"stock,"+
																"id_quien_creo,"+
																"fechahora_alta,"+
																"fechahora_ultimo_surtimiento)"+
																"VALUES ('"+
																idsubalmacen.ToString()+"','"+
																(string) lista_productos_a_recibir.Model.GetValue(iter,10).ToString().Trim()+"','"+
																(string) lista_productos_a_recibir.Model.GetValue(iter,5).ToString().Trim()+"','"+
																LoginEmpleado+"','"+
																DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');";
										comando4.ExecuteNonQuery();
										comando4.Dispose();																							
									}catch (NpgsqlException ex){
							   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error, 
													ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
										msgBoxError.Run ();
										msgBoxError.Destroy();
									}
									conexion4.Close();																																															
								}									
							}catch (NpgsqlException ex){
							   	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error, 
													ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
								msgBoxError.Destroy();
							}
							conexion3.Close();*/
						}
					}
					while (treeViewEngineListaProdRequi.IterNext(ref iter)){
						if((bool) checkbutton_factura_sin_orden.Active == false){
							
						}else{
							comando.CommandText = "INSERT INTO osiris_erp_requisicion_deta( "+
			 									"id_producto,"+
			 									"cantidad_solicitada,"+
			 									"cantidad_comprada," +
			 									"cantidad_recibida,"+
			 									"comprado,"+
			 									"recibido," +
			 									"id_proveedor," +
			 									"numero_factura_proveedor," +
			 									"costo_producto," +
			 									"costo_por_unidad, " +									
			 									"precio_producto_publico," +
												"porcentage_ganancia," +
												"cantidad_de_embalaje," +
												"id_quien_compro," +
			 									"fechahora_compra," +
			 									"id_quien_recibio," +
			 									"fechahora_recibido," +
			 									"autorizada," +
			 									"id_quien_autorizo," +
			 									"fechahora_autorizado," +
												"descripcion_producto_proveedor,"+
			 									"id_producto_proveedor," +
												"tipo_unidad_producto,"+
			 									"factura_sin_orden_compra," +
			 									"precio_costo_prov_selec," +
			 									"lote_producto," +
			 									"caducidad_producto," +
			 									"costo_producto_osiris," +
			 									"cantidad_de_embalaje_osiris) "+
			 									"VALUES ('"+												
												lista_productos_a_recibir.Model.GetValue(iter,10).ToString().Trim()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,3).ToString().Trim()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,3).ToString().Trim()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,5).ToString().Trim()+"','"+
												"true','"+
												"true','"+
												entry_id_proveedor.Text.Trim()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,1).ToString().Trim().ToUpper()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,6).ToString().Trim()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,7).ToString().Trim()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,8).ToString().Trim()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,16).ToString().Trim()+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,4).ToString().Trim()+"','"+
												LoginEmpleado+"','"+
												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
												LoginEmpleado+"','"+
												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
												"true','"+
												LoginEmpleado+"','"+
												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,11)+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,12)+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,15).ToString().Trim()+"','"+
												"true','"+
												lista_productos_a_recibir.Model.GetValue(iter,6)+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,13)+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,14)+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,17)+"','"+
												lista_productos_a_recibir.Model.GetValue(iter,18)+
												"');";
							//Console.WriteLine(comando.CommandText);
		 					comando.ExecuteNonQuery();
					    	comando.Dispose();
							
							NpgsqlConnection conexion1;
							conexion1 = new NpgsqlConnection (connectionString+nombrebd);
			 				try{
								conexion1.Open ();
								NpgsqlCommand comando1; 
								comando1 = conexion.CreateCommand();
								comando1.CommandText = "SELECT id_producto,id_almacen,stock FROM osiris_catalogo_almacenes "+
													"WHERE id_almacen = '"+idsubalmacen.ToString()+"' "+
													"AND eliminado = 'false' "+														
													"AND id_producto = '"+(string) lista_productos_a_recibir.Model.GetValue(iter,10).ToString().Trim()+"' ;";
								//Console.WriteLine(comando.CommandText);
								NpgsqlDataReader lector1 = comando1.ExecuteReader ();
								if(lector1.Read()){
									//Console.WriteLine("if ");
									NpgsqlConnection conexion2; 
									conexion2 = new NpgsqlConnection (connectionString+nombrebd);
			 						try{
										conexion2.Open ();
										NpgsqlCommand comando2;
										comando2 = conexion2.CreateCommand();
										// verificando que asigne un paciente para que la pre-solicitud se pueda surtir
										
										comando2.CommandText = "UPDATE osiris_catalogo_almacenes SET stock  = stock + '"+(string) lista_productos_a_recibir.Model.GetValue(iter,5).ToString().Trim()+"',"+
																//"historial_surtido_material = historial_surtido_material || '"+LoginEmpleado+" "+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n',"+
																"fechahora_ultimo_surtimiento = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
																"WHERE id_almacen = '"+idsubalmacen.ToString()+"' "+
																"AND id_producto = '"+(string) lista_productos_a_recibir.Model.GetValue(iter,10).ToString().Trim()+"' ;";
										//Console.WriteLine(comando1.CommandText);
										comando2.ExecuteNonQuery();
										
										comando2.Dispose();
										conexion2.Close();
									}catch (NpgsqlException ex){
							   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error, 
													ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
										msgBoxError.Run ();
									}
									conexion2.Close();		
								}else{
									//Console.WriteLine("else"+idalmacenorigen);
									NpgsqlConnection conexion2; 
									conexion2 = new NpgsqlConnection (connectionString+nombrebd);
			 						try{
										conexion2.Open ();
										NpgsqlCommand comando2; 
										comando2 = conexion1.CreateCommand();
										comando2.CommandText = "INSERT INTO osiris_catalogo_almacenes("+
																"id_almacen,"+
																"id_producto,"+
																"stock,"+
																"id_quien_creo,"+
																"fechahora_alta,"+
																"fechahora_ultimo_surtimiento)"+
																"VALUES ('"+
																idsubalmacen.ToString()+"','"+
																(string) lista_productos_a_recibir.Model.GetValue(iter,10).ToString().Trim()+"','"+
																(string) lista_productos_a_recibir.Model.GetValue(iter,5).ToString().Trim()+"','"+
																LoginEmpleado+"','"+
																DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');";
										comando2.ExecuteNonQuery();
										comando2.Dispose();																							
									}catch (NpgsqlException ex){
							   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error, 
													ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
										msgBoxError.Run ();
										msgBoxError.Destroy();
									}
									conexion2.Close();																																															
								}									
							}catch (NpgsqlException ex){
							   	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error, 
													ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
								msgBoxError.Destroy();
							}
							conexion1.Close();							
						}
					}
					button_guardar.Sensitive = false;
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
				conexion.Close();
			}			
		}
		
		void on_checkbutton_factura_sin_orden_clicked(object sender, EventArgs args)
		{
			if (checkbutton_factura_sin_orden.Active == true){
				button_busca_proveedor.Sensitive = true;
				button_busca_producto.Sensitive = true;
				button_quitar_producto.Sensitive = true;
				button_busca_proveedor.Sensitive = true;
				entry_fecha_orden_compra.Text = DateTime.Now.ToString("yyyy-MM-dd");//("dd-MM-yyyy");
				// Desactivando las ordenes de Compra
				entry_orden_de_compra.Sensitive = false;
				button_selecciona_ordencompra.Sensitive = false;
				button_busca_orden_compra.Sensitive = false;
				entry_fecha_orden_compra.Sensitive = false;
				entry_id_quien_hizo.Sensitive = false;
				entry_estatus_oc.Sensitive = false;
				button_guardar.Sensitive = true;
				treeViewEngineListaProdRequi.Clear();
				entry_id_proveedor.Text = "";
				entry_nombre_proveedor.Text = "";
				entry_direccion_proveedor.Text = "";
				entry_tel_proveedor.Text = "";
				entry_contacto_proveedor.Text = "";
				entry_formapago.Text = "";
				entry_num_factura_proveedor.Text = "";
			}else{
				button_busca_proveedor.Sensitive = false;
				button_busca_producto.Sensitive = false;
				button_quitar_producto.Sensitive = false;
				button_busca_proveedor.Sensitive = false;
				entry_fecha_orden_compra.Text = "";
				// Desactiv las ordenes de Compra
				entry_orden_de_compra.Sensitive = true;
				button_selecciona_ordencompra.Sensitive = true;
				button_busca_orden_compra.Sensitive = true;
				entry_fecha_orden_compra.Sensitive = true;
				entry_id_quien_hizo.Sensitive = true;
				entry_estatus_oc.Sensitive = true;
		 	}
		}
		
		void crea_treeview_capturafactura()
		{
			/*cell_combox_store = new ListStore(typeof (string));
			cell_combox_store.AppendValues (" ");
			cell_combox_store.AppendValues ("PIEZA");
			cell_combox_store.AppendValues ("KILO");
			cell_combox_store.AppendValues ("LITRO");
			cell_combox_store.AppendValues ("GRAMO");
			cell_combox_store.AppendValues ("METRO");
			cell_combox_store.AppendValues ("CENTIMETRO");
			cell_combox_store.AppendValues ("CAJA");
			cell_combox_store.AppendValues ("PULGADA");
			cell_combox_store.AppendValues ("FRASCO");
			cell_combox_store.AppendValues ("GALON");*/
			
			treeViewEngineListaProdRequi = new TreeStore(typeof(bool), 
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
									typeof(string));
			
			lista_productos_a_recibir.Model = treeViewEngineListaProdRequi;			
			lista_productos_a_recibir.RulesHint = true;
						
			col_00 = new TreeViewColumn();
			CellRendererToggle cellrToggle = new CellRendererToggle();
			col_00.Title = "Seleccion";
			col_00.PackStart(cellrToggle, true);
			col_00.AddAttribute (cellrToggle, "active", 0);
			cellrToggle.Activatable = true;
			cellrToggle.Toggled += selecciona_fila;
			col_00.SortColumnId = (int) col_productos_recibidos.col_00;			
									
			col_01 = new TreeViewColumn();
			cellrt1 = new CellRendererText();
			col_01.Title = "N° Factura";
			col_01.PackStart(cellrt1, true);
			col_01.AddAttribute(cellrt1, "text", 1);
			col_01.SortColumnId = (int) col_productos_recibidos.col_01;
			cellrt1.Edited += NumberCellEdited;
			cellrt1.Editable = true;
										
			col_02 = new TreeViewColumn();
			cellrt2 = new CellRendererText();
			col_02.Title = "Cant. O.C.";
			col_02.PackStart(cellrt2, true);
			col_02.AddAttribute (cellrt2, "text", 2);
			col_02.SortColumnId = (int) col_productos_recibidos.col_02;
			
			col_03 = new TreeViewColumn();
			cellrt3 = new CellRendererText();
			col_03.Title = "Cant.Recibida";
			col_03.PackStart(cellrt3, true);
			col_03.AddAttribute(cellrt3, "text", 3);
			col_03.SortColumnId = (int) col_productos_recibidos.col_03;
			cellrt3.Edited += NumberCellEdited_Recibida;
			cellrt3.Editable = true;
			
			col_04 = new TreeViewColumn();
			cellrt4 = new CellRendererText();
			col_04.Title = "Pack/Embalaje";
			col_04.PackStart(cellrt4, true);
			col_04.AddAttribute (cellrt4, "text", 4);
			cellrt4.Edited += NumberCellEdited;
			cellrt4.Editable = true;
			col_04.SortColumnId = (int) col_productos_recibidos.col_04;
			
			col_05 = new TreeViewColumn();
			cellrt5 = new CellRendererText();
			col_05.Title = "Ingreso Stock";
			col_05.PackStart(cellrt5, true);
			col_05.AddAttribute (cellrt5, "text", 5);
			cellrt5.Edited += NumberCellEdited;
			cellrt5.Editable = true;
			col_05.SortColumnId = (int) col_productos_recibidos.col_05;
			
			col_06 = new TreeViewColumn();
			cellrt6 = new CellRendererText();
			col_06.Title = "Precio";
			col_06.PackStart(cellrt6, true);
			col_06.AddAttribute (cellrt6, "text", 6);
			col_06.SortColumnId = (int) col_productos_recibidos.col_06;
			cellrt6.Editable = true;
			
			col_07 = new TreeViewColumn();
			cellrt7 = new CellRendererText();
			col_07.Title = "Pre.Unitario";
			col_07.PackStart(cellrt7, true);
			col_07.AddAttribute (cellrt7, "text", 7);
			col_07.SortColumnId = (int) col_productos_recibidos.col_07;
			
			col_08 = new TreeViewColumn();
			cellrt8 = new CellRendererText();
			col_08.Title = "Pre.Unit.Osiris";
			col_08.PackStart(cellrt8, true);
			col_08.AddAttribute (cellrt8, "text", 8);
			col_08.SortColumnId = (int) col_productos_recibidos.col_08;
			col_08.Resizable = true;
			
			col_09 = new TreeViewColumn();
			cellrt9 = new CellRendererText();
			col_09.Title = "Descripcion de Producto";
			col_09.PackStart(cellrt9, true);
			col_09.AddAttribute (cellrt9, "text", 9);
			col_09.SortColumnId = (int) col_productos_recibidos.col_09;
			
			col_10 = new TreeViewColumn();
			cellrt10 = new CellRendererText();
			col_10.Title = "id Producto";
			col_10.PackStart(cellrt10, true);
			col_10.AddAttribute (cellrt10, "text", 10);
			col_10.SortColumnId = (int) col_productos_recibidos.col_10;
			cellrt10.Editable = true;
			
			col_11 = new TreeViewColumn();
			cellrt11 = new CellRendererText();
			col_11.Title = "Desc. Prod. Proveedor";
			col_11.PackStart(cellrt11, true);
			col_11.AddAttribute (cellrt11, "text", 11);
			col_11.SortColumnId = (int) col_productos_recibidos.col_11;
			cellrt11.Editable = true;
			
			col_12 = new TreeViewColumn();
			cellrt12 = new CellRendererText();
			col_12.Title = "Id Prod.Prove.";
			col_12.PackStart(cellrt12, true);
			col_12.AddAttribute (cellrt12, "text", 12);
			col_12.SortColumnId = (int) col_productos_recibidos.col_12;
			cellrt12.Editable = true;
			
			col_13 = new TreeViewColumn();
			cellrt13 = new CellRendererText();
			col_13.Title = "Lote";
			col_13.PackStart(cellrt13, true);
			col_13.AddAttribute (cellrt13, "text", 13);
			col_13.SortColumnId = (int) col_productos_recibidos.col_13;
			cellrt13.Editable = true;
			
			col_14 = new TreeViewColumn();
			cellrt14 = new CellRendererText();
			col_14.Title = "Caducidad";
			col_14.PackStart(cellrt14, true);
			col_14.AddAttribute (cellrt14, "text", 14);
			col_14.SortColumnId = (int) col_productos_recibidos.col_14;
			cellrt14.Editable = true;
			
			
			// ComboBox dentro del treeview
			col_15 = new TreeViewColumn();
			cellrt15 = new CellRendererText();
			col_15.Title = "Presentacion";
			col_15.PackStart(cellrt15, true);
			col_15.AddAttribute (cellrt15, "text", 15);
			col_15.SortColumnId = (int) col_productos_recibidos.col_15;
			cellrt15.Editable = true;
			
			/*
			cellrt15 = new Gtk.CellRendererCombo ();
			(cellrt15 as Gtk.CellRendererCombo).Editable = true;
			(cellrt15 as Gtk.CellRendererCombo).HasEntry = false;
			(cellrt15 as Gtk.CellRendererCombo).Edited += OnTaskPriorityEdited;
			(cellrt15 as Gtk.CellRendererCombo).Model = cell_combox_store;
			TreeIter iter1;
			if (cell_combox_store.GetIterFirst(out iter1)){
				cellrt15.Text = "";			
			}
			(cellrt15 as Gtk.CellRendererCombo).TextColumn = 0;
			//cellrt15.Xalign = 0.5f;
			col_15.PackStart(cellrt15, false);
			col_15.SetCellDataFunc (cellrt15, new Gtk.TreeCellDataFunc (TaskPriorityCellDataFunc));
			*/
			
			lista_productos_a_recibir.AppendColumn(col_00);
			lista_productos_a_recibir.AppendColumn(col_01);
			lista_productos_a_recibir.AppendColumn(col_02);
			lista_productos_a_recibir.AppendColumn(col_03);
			lista_productos_a_recibir.AppendColumn(col_04);
			lista_productos_a_recibir.AppendColumn(col_05);
			lista_productos_a_recibir.AppendColumn(col_06);
			lista_productos_a_recibir.AppendColumn(col_07);
			lista_productos_a_recibir.AppendColumn(col_08);
			lista_productos_a_recibir.AppendColumn(col_09);
			lista_productos_a_recibir.AppendColumn(col_10);
			lista_productos_a_recibir.AppendColumn(col_11);
			lista_productos_a_recibir.AppendColumn(col_12);
			lista_productos_a_recibir.AppendColumn(col_13);
			lista_productos_a_recibir.AppendColumn(col_14);
			lista_productos_a_recibir.AppendColumn(col_15);
		}
		
		enum col_productos_recibidos
		{
			col_00,
			col_01,
			col_02,
			col_03,
			col_04,
			col_05,
			col_06,
			col_07,
			col_08,
			col_09,
			col_10,
			col_11,
			col_12,
			col_13,
			col_14,
			col_15
		}
		
		void TaskPriorityCellDataFunc (Gtk.TreeViewColumn tree_column,Gtk.CellRenderer cell,Gtk.TreeModel tree_model,Gtk.TreeIter iter)
		{
			// TODO: Add bold (for high), light (for None), and also colors to priority?
			//Gtk.CellRendererCombo crc = cell as Gtk.CellRendererCombo;
			//Console.WriteLine((string) crc.Model.GetValue(iter,0));
									
			//if (tree_column.ge   combobox_tipo_unidad2.GetActiveIter (out iter)){
	    	//	tipounidadproducto = (string) combobox_tipo_unidad2.Model.GetValue(iter,0);    		 
			//}
			
			//Console.Write((string) crc.Model.GetValue(iter,0));
			//crc.Text = "Hola";
			//if(tree_model.GetValue(iter,0)){
			//if (crc.Model.GetIter (out iter,0)){					
				//bool old = (bool) this.lista_productos_a_recibir.Model.GetValue(iter,0);
				//this.lista_productos_a_recibir.Model.SetValue(iter,0,!old);
			
			//}
			
						
			
			//ITask task = Model.GetValue (iter, 0) as ITask;
			//switch (task.Priority) {
			//	case TaskPriority.Low:
					//crc.Text = Catalog.GetString ("3");
			//		break;
			//	case TaskPriority.Medium:
					//crc.Text = Catalog.GetString ("2");
			//		break;
			//	case TaskPriority.High:
					//crc.Text = Catalog.GetString ("1");
			//		break;
			//	default:
					//crc.Text = Catalog.GetString ("-");
			//		break;
                  	//}
		}
		
		void OnTaskPriorityEdited (object sender, Gtk.EditedArgs args)
		{
			   //Gtk.TreeIter iter;
			   //Gtk.TreePath path = new TreePath (args.Path);
			//Console.WriteLine( args.NewText.ToString());
			   //cellrt15.Text = args.NewText.ToString();
			//if ( Model.GetIter (out iter, path))
			//TreeIter iter;
			return;
			
			/*
			TaskPriority newPriority;
			if (args.NewText.CompareTo (Catalog.GetString ("3")) == 0)
				newPriority = TaskPriority.Low;
			else if (args.NewText.CompareTo (Catalog.GetString ("2")) == 0)
				newPriority = TaskPriority.Medium;
			else if (args.NewText.CompareTo (Catalog.GetString ("1")) == 0)
				newPriority = TaskPriority.High;
			else
				newPriority = TaskPriority.None;

			// Update the priority if it's different
			ITask task = Model.GetValue (iter, 0) as ITask;
			if (task.Priority != newPriority)
				task.Priority = newPriority;
			*/
		}
				
		// Cuando seleccion campos para la autorizacion de compras  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			if (this.lista_productos_a_recibir.Model.GetIter (out iter, new TreePath (args.Path))){					
				bool old = (bool) this.lista_productos_a_recibir.Model.GetValue(iter,0);
				this.lista_productos_a_recibir.Model.SetValue(iter,0,!old);
			}				
		}
		
		void NumberCellEdited (object sender, EditedArgs args)
		{
			Console.WriteLine(args.ToString());
			Console.WriteLine(args.Path.ToString());
			Console.WriteLine(sender.ToString());
			if (sender.ToString() == "Gtk.CellRendererText"){
				Gtk.CellRendererText cellRenderer_treeview = (object) sender as Gtk.CellRendererText;
				//cellRenderer_treeview.Data.
			}
			Gtk.TreeIter iter;
			//treeViewEngineListaProdRequi.GetIter (out iter, new Gtk.TreePath (args.Path));
			//treeViewEngineListaProdRequi.SetValue(iter,(int) col_productos_recibidos.col_numerofactura,args.NewText);			
		}
		
		void NumberCellEdited_Recibida (object o, EditedArgs args)
		{
			Gtk.TreeIter iter;
			bool esnumerico = false;
			int var_paso = 0;			
			int largo_variable = args.NewText.ToString().Length;
			string toma_variable = args.NewText.ToString();			
			treeViewEngineListaProdRequi.GetIter (out iter, new Gtk.TreePath (args.Path));			
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
				//treeViewEngineListaProdRequi.SetValue(iter,(int) col_productos_recibidos.col_cant_recibida,args.NewText);
				//bool old = (bool) lista_productos_a_recibir.Model.GetValue (iter,0);
				//lista_productos_a_recibir.Model.SetValue(iter,0,!old);
			}
 		}
		
		void on_button_selecciona_ordencompra_clicked(object sender, EventArgs args)
		{
			llenado_orden_de_compra();
		}
		
		void llenado_orden_de_compra()
		{
			treeViewEngineListaProdRequi.AppendValues(false,"99099","10","10");
			
			//osiris_erp_requisicion_deta
			treeViewEngineListaProdRequi.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada		
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT numero_orden_compra,id_proveedor,descripcion_proveedor,direccion_proveedor,"+
					"faxnextel_proveedor,contacto_proveedor,condiciones_de_pago,fechahora_creacion "+
					"FROM osiris_erp_ordenes_compras_enca WHERE numero_orden_compra = '"+entry_orden_de_compra.Text.Trim()+"';";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();							
				if (lector.Read()){
					entry_id_proveedor.Text = Convert.ToString((int) lector["id_proveedor"]).ToString().Trim();
					entry_nombre_proveedor.Text = (string) lector["descripcion_proveedor"];
					entry_direccion_proveedor.Text = (string) lector["direccion_proveedor"];
					entry_tel_proveedor.Text = (string) lector["faxnextel_proveedor"];
					entry_contacto_proveedor.Text  = (string) lector["contacto_proveedor"];
					entry_formapago.Text  = (string) lector["condiciones_de_pago"];
				}
				comando = conexion.CreateCommand ();				
				comando.CommandText = "SELECT * "+
					"FROM osiris_erp_requisicion_deta WHERE numero_orden_compra = '"+entry_orden_de_compra.Text.Trim()+"';";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector1 = comando.ExecuteReader ();							
				while (lector1.Read()){
					treeViewEngineListaProdRequi.AppendValues (false,this.entry_num_factura_proveedor.Text.Trim(),
					                                           float.Parse(Convert.ToString((decimal) lector1["cantidad_solicitada"]).ToString()).ToString("F"));
				}
								
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
								msgBoxError.Destroy();
			}
			conexion.Close ();

		}
		
		void on_busca_proveedores_clicked(object sender, EventArgs args)
		{
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion
			// la clase recibe tambien el orden del query
			// es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo
			object[] parametros_objetos = {entry_id_proveedor,entry_nombre_proveedor,entry_direccion_proveedor,entry_tel_proveedor,entry_contacto_proveedor,entry_formapago};
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
			classfind_data.buscandor(parametros_objetos,parametros_sql,"find_proveedores_OC"," ORDER BY descripcion_proveedor;","%' ",0);
		}
				
		void on_button_busca_producto_clicked(object sender, EventArgs a)
		{
			if(entry_num_factura_proveedor.Text != ""  && entry_id_proveedor.Text != "" && entry_id_proveedor.Text != "1"){
				Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "busca_producto", null);
				gxml.Autoconnect (this);
				busca_producto.Show();
				crea_treeview_busqueda();
				button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
				button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
				entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
				button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
				entry_cantidad_aplicada.KeyPressEvent += onKeyPressEvent_numeric; 
				entry_embalaje_pack.KeyPressEvent += onKeyPressEvent_numeric;
				entry_precio.KeyPressEvent += onKeyPressEvent_numeric;
				CellRendererText cell3 = new CellRendererText();
				combobox_tipo_unidad2.PackStart(cell3, true);
				combobox_tipo_unidad2.AddAttribute(cell3,"text",0);
		        combobox_tipo_unidad2.Model = cell_combox_store;
				TreeIter iter3;
				if (cell_combox_store.GetIterFirst(out iter3)){
					combobox_tipo_unidad2.SetActiveIter (iter3);
				}
				combobox_tipo_unidad2.Changed += new EventHandler (onComboBoxChanged_tipo_unidad);
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info, 
									ButtonsType.Close, "NO tiene un numero de FACTURA o NO ha elegido un PROVEEDOR... Verifique");
				msgBoxError.Run ();								msgBoxError.Destroy();
			}
		}
				
		void onComboBoxChanged_tipo_unidad (object sender, EventArgs args)
		{
	    	ComboBox combobox_tipo_unidad2 = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_tipo_unidad2.GetActiveIter (out iter)){
	    		tipounidadproducto = (string) combobox_tipo_unidad2.Model.GetValue(iter,0);    		 
			}
		}
				
		// declara y crea el treeviev de Producto en la busqueda
		void crea_treeview_busqueda()
		{
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
								typeof(bool),
								typeof(bool),
								typeof(bool),
								typeof(string));
			lista_de_producto.Model = treeViewEngineBusca2;
			
			lista_de_producto.RulesHint = true;
			
			lista_de_producto.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono producto*/
			lista_de_producto.MoveCursor += on_packproductos_clicked;
			
			col_idproducto = new TreeViewColumn();
			cellrt0 = new CellRendererText();
			col_idproducto.Title = "ID Producto";
			col_idproducto.PackStart(cellrt0, true);
			col_idproducto.AddAttribute (cellrt0, "text", 0);
			col_idproducto.SortColumnId = (int) Column_prod.col_idproducto;
			
			col_desc_producto = new TreeViewColumn();
			cellrt1 = new CellRendererText();
			col_desc_producto.Title = "Descripcion de Producto";
			col_desc_producto.PackStart(cellrt1, true);
			col_desc_producto.AddAttribute (cellrt1, "text", 1);
			col_desc_producto.SortColumnId = (int) Column_prod.col_desc_producto;
			col_desc_producto.Resizable = true;
			//cellr0.Editable = true;   // Permite edita este campo
            
			col_precioprod = new TreeViewColumn();
			cellrt2 = new CellRendererText();
			col_precioprod.Title = "Precio Producto";
			col_precioprod.PackStart(cellrt2, true);
			col_precioprod.AddAttribute (cellrt2, "text", 2);
			col_precioprod.SortColumnId = (int) Column_prod.col_precioprod;
			
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
			
			col_grupoprod = new TreeViewColumn();
			cellrt7 = new CellRendererText();
			col_grupoprod.Title = "Grupo Producto";
			col_grupoprod.PackStart(cellrt7, true);
			col_grupoprod.AddAttribute (cellrt7, "text", 7);
			col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
			
			col_grupo1prod = new TreeViewColumn();
			cellrt8 = new CellRendererText();
			col_grupo1prod.Title = "Grupo1 Producto";
			col_grupo1prod.PackStart(cellrt8, true);
			col_grupo1prod.AddAttribute (cellrt8, "text", 8);
			col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
			
			col_grupo2prod = new TreeViewColumn();
			cellrt9 = new CellRendererText();
			col_grupo2prod.Title = "Grupo2 Producto";
			col_grupo2prod.PackStart(cellrt9, true);
			col_grupo2prod.AddAttribute (cellrt9, "text", 9);
			col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;
			
			col_costoprod_uni = new TreeViewColumn();
			cellrt12 = new CellRendererText();
			col_costoprod_uni.Title = "Precio Unitario";
			col_costoprod_uni.PackStart(cellrt12, true);
			col_costoprod_uni.AddAttribute (cellrt12, "text", 12);
			col_costoprod_uni.SortColumnId = (int) Column_prod.col_costoprod_uni;
			
			col_embalajeprod = new TreeViewColumn();
			cellrt15 = new CellRendererText();
			col_embalajeprod.Title = "Embalaje/Pack";
			col_embalajeprod.PackStart(cellrt15, true);
			col_embalajeprod.AddAttribute (cellrt15, "text", 15);
			col_embalajeprod.SortColumnId = (int) Column_prod.col_embalajeprod;
				
			col_aplica_iva = new TreeViewColumn();
			cellrt19 = new CellRendererText();
			col_aplica_iva.Title = "Iva Activo";
			col_aplica_iva.PackStart(cellrt19, true);
			col_aplica_iva.AddAttribute (cellrt19, "text", 19);
			col_aplica_iva.SortColumnId = (int) Column_prod.col_aplica_iva;
				
			col_cobro_activo = new TreeViewColumn();
			cellrt20 = new CellRendererText();
			col_cobro_activo.Title = "Prod. Activo";
			col_cobro_activo.PackStart(cellrt20, true);
			col_cobro_activo.AddAttribute (cellrt20, "text", 20);
			col_cobro_activo.SortColumnId = (int) Column_prod.col_cobro_activo;
				
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
			lista_de_producto.AppendColumn(col_costoprod_uni); //12
			lista_de_producto.AppendColumn(col_embalajeprod);	//15
			lista_de_producto.AppendColumn(col_aplica_iva);//19
			lista_de_producto.AppendColumn(col_cobro_activo);//20
		}
		
		enum Column_prod
		{
			col_idproducto,			col_desc_producto,
			col_precioprod,			col_ivaprod,
			col_totalprod,			col_descuentoprod,
			col_preciocondesc,		col_grupoprod,
			col_grupo1prod,			col_grupo2prod,
			col_nom_art,			col_nom_gen,
			col_costoprod_uni,		col_porc_util,
			col_costo_prod,
			col_embalajeprod,
			col_cant_embalaje,
			col_id_gpo_prod,		col_id_gpo_prod1,
			col_id_gpo_prod2,		col_aplica_iva,
			col_cobro_activo,		col_aplica_desc
		}
		
		// llena la lista de productos
 		void on_llena_lista_producto_clicked (object sender, EventArgs args)
 		{
 			llena_la_lista_de_productos();
 		}
 		
 		void llena_la_lista_de_productos()
 		{
 			treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			string query_tipo_busqueda = "";
			if(radiobutton_nombre.Active == true) {query_tipo_busqueda = "AND osiris_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_producto; "; }
			if(radiobutton_codigo.Active == true) {query_tipo_busqueda = "AND osiris_productos.id_producto LIKE '"+entry_expresion.Text.Trim()+"%'  ORDER BY id_producto; "; }
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
							"osiris_productos.descripcion_producto,osiris_productos.nombre_articulo,osiris_productos.nombre_generico_articulo, "+
							"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(precio_producto_publico1,'99999999.99') AS preciopublico1,"+
							"to_char(cantidad_de_embalaje,'99999999.99') AS cantidadembalaje,"+
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
							"AND osiris_productos.cobro_activo = 'true' "+
							query_tipo_busqueda;
				//Console.WriteLine(comando.CommandText);
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
					col_idproducto.SetCellDataFunc(cellrt0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_desc_producto.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_precioprod.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_ivaprod.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_totalprod.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_descuentoprod.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_preciocondesc.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_grupoprod.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_grupo1prod.SetCellDataFunc(cellrt8, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_grupo2prod.SetCellDataFunc(cellrt9, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_aplica_iva.SetCellDataFunc(cellrt19, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_cobro_activo.SetCellDataFunc(cellrt20, new Gtk.TreeCellDataFunc(cambia_colores_fila));
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
								msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{	
			if(float.Parse(entry_precio.Text.Trim()) > 0){
				TreeModel model;
				TreeIter iterSelected;
				if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
					
					// buscando el codigo del proveedor para agregarlo en su catalogo
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
		               	comando.CommandText = "";
					}catch (NpgsqlException ex){
		   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
									msgBoxError.Run ();
									msgBoxError.Destroy();
					}
					conexion.Close ();
					
					treeViewEngineListaProdRequi.AppendValues (true,
								entry_num_factura_proveedor.Text.Trim().ToUpper(),
								"0",
								entry_cantidad_aplicada.Text.Trim(),
								entry_embalaje_pack.Text.Trim(),
								Convert.ToString(float.Parse(entry_cantidad_aplicada.Text.Trim()) * float.Parse(entry_embalaje_pack.Text.Trim())),
								entry_precio.Text.Trim(),
								string.Format("{0:F}",float.Parse(entry_precio.Text.Trim())/float.Parse(entry_embalaje_pack.Text.Trim())),
								(string) model.GetValue(iterSelected, 12),
								(string) model.GetValue(iterSelected, 1),
								(string) model.GetValue(iterSelected, 0),
								(string) entry_producto_proveedor.Text.Trim().ToUpper(),
								(string) entry_codprod_proveedor.Text.Trim().ToUpper(),
								(string) entry_lote.Text.Trim().ToUpper(),
								(string) entry_caducidad.Text.Trim().ToUpper(),
								tipounidadproducto,
								(string) model.GetValue(iterSelected, 13),
								(string) model.GetValue(iterSelected, 14),
								(string) model.GetValue(iterSelected, 15));
					entry_cantidad_aplicada.Text = "0";
					entry_embalaje_pack.Text = "1";
					entry_precio.Text = "0";
					
					//cellrt15.Text = tipounidadproducto;
					//cierra la ventana despues que almaceno la informacion en variables
					//Widget win = (Widget) sender;
					//win.Toplevel.Destroy();
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info, 
									ButtonsType.Close, "No ha seleccionado ningun PRODUCTO... Verifique...");
				msgBoxError.Run ();								msgBoxError.Destroy();
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info, 
									ButtonsType.Close, "No tiene precio el PRODUCTO... Verifique...");
				msgBoxError.Run ();								msgBoxError.Destroy();
			}
		}
		
		void on_packproductos_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
				entry_embalaje_pack.Text = (string) model.GetValue(iterSelected, 15);
			}
		}
		
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((bool)lista_de_producto.Model.GetValue (iter,20)==true){ 
				if ((bool)lista_de_producto.Model.GetValue (iter,19)==true) { (cell as Gtk.CellRendererText).Foreground = "blue";
				}else{
				(cell as Gtk.CellRendererText).Foreground = "black";}
			}else{
				(cell as Gtk.CellRendererText).Foreground = "red";}
		}
		
		///////////////////////////////////////BOTON general de busqueda por enter///////////////////////////////////////////////		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;		
				llena_la_lista_de_productos();
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		void onKeyPressEvent_numeric(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}					
	}
}