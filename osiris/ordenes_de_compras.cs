// created on 04/09/2008 at 05:38 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
//                Ing. Jesus Buentello Garza (Programacion)
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
// Programa		: ordenes_de_compra
// Proposito	: Creacion de Ordenes de Compra
// Objeto		: 
//////////////////////////////////////////////////////////
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using GtkSharp;

namespace osiris
{
	public class crea_ordenes_de_compra
	{
	
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		//BOTON IMPRIMIR
        [Widget] Gtk.Button button_imprimir = null;
		
		// Declarando ventana del menu de costos
		[Widget] Gtk.Window crea_ordenes_compras = null;
		[Widget] Gtk.CheckButton checkbutton_all_deptos = null;
		[Widget] Gtk.ComboBox combobox_condicion_oc = null;
		[Widget] Gtk.TreeView lista_productos_a_comprar = null;
		[Widget] Gtk.Button button_busca_proveedores = null;
		[Widget] Gtk.Button button_asignar_proveedor = null;
		[Widget] Gtk.Button button_mov_productos_oc = null;
		[Widget] Gtk.Button button_orden_compra = null;
		[Widget] Gtk.Entry entry_id_proveedor = null;
		[Widget] Gtk.Entry entry_nombre_proveedor = null;
		[Widget] Gtk.Entry entry_formapago = null;
		[Widget] Gtk.Entry entry_direccion_proveedor = null;
		[Widget] Gtk.Entry entry_tel_proveedor = null;
		[Widget] Gtk.Entry entry_rfc_proveedor = null;
		[Widget] Gtk.Entry entry_dia_oc = null;
		[Widget] Gtk.Entry entry_mes_oc = null;
		[Widget] Gtk.Entry entry_ano_oc = null;
		[Widget] Gtk.Entry entry_dia_fentrega = null;
		[Widget] Gtk.Entry entry_mes_fentrega = null;
		[Widget] Gtk.Entry entry_ano_fentrega = null;
		[Widget] Gtk.Entry entry_lugar_entrega = null;
		[Widget] Gtk.Entry entry_observaciones = null;
		[Widget] Gtk.ComboBox combobox_facturar_a = null;
		
		[Widget] Gtk.Statusbar statusbar = null;
		[Widget] Gtk.TreeView treeview_lista_departamentos = null;
		
		[Widget] Gtk.Button button_prod_comprado = null;
		[Widget] Gtk.Button button_libera_producto = null; 
				
		string connectionString;
		string nombrebd;
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
		int contador_prod_asignados = 0;
		int contador_prod_noasignad = 0;
		string departamentos_seleccionados = "";
    	
    	int ultimaorden = 0;
		int idreceptor = 1;
		int selecciono_productos = 0;
    	
    	// Declarando las variables de publicas para uso dentro de classe
    	int idtipointernamiento;
    	string descripinternamiento = "";	// Descripcion de Centro de Costos - Solicitado por
		
		string tipodeordencompra = "";
		string[] args_tipoordencompra = {"","ORDINARIA","URGENTE"};
		string[] args_args = {""};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14};
		
		TreeStore treeViewEngineProductosaComprar;	// Lista de proctos que se van a comprar
		TreeStore treeViewEngineListaDepartamentos;
				
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;		
				
		TreeViewColumn col_00;	CellRendererToggle cellrt_00;
		TreeViewColumn col_01;	CellRendererText cellrt_01;
		TreeViewColumn col_02;	CellRendererText cellrt_02;
		TreeViewColumn col_17;	CellRendererText cellrt_17;
		TreeViewColumn col_18;	CellRendererText cellrt_18;
		TreeViewColumn col_03;	CellRendererText cellrt_03;
		TreeViewColumn col_09;	CellRendererText cellrt_09;
		TreeViewColumn col_04;	CellRendererText cellrt_04;
		TreeViewColumn col_05;	CellRendererText cellrt_05;
		TreeViewColumn col_06;	CellRendererText cellrt_06;
		TreeViewColumn col_07;	CellRendererText cellrt_07;
		TreeViewColumn col_08;	CellRendererText cellrt_08;
		TreeViewColumn col_10;	CellRendererText cellrt_10;
		TreeViewColumn col_11;	CellRendererText cellrt_11;
		TreeViewColumn col_12;	CellRendererText cellrt_12;
		TreeViewColumn col_13;	CellRendererText cellrt_13;
		TreeViewColumn col_14;	CellRendererText cellrt_14;
		TreeViewColumn col_19;	CellRendererText cellrt_19;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		class_buscador classfind_data = new class_buscador();
		
		public crea_ordenes_de_compra(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_ )
		{
			LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
    		
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "crea_ordenes_compras", null);
			gxml.Autoconnect (this);
			////// Muestra ventana de Glade
			crea_ordenes_compras.Show();
			//this.entry_formapago.Text = "30 DIAS";
			entry_dia_oc.Text = DateTime.Now.ToString("dd");
			entry_mes_oc.Text = DateTime.Now.ToString("MM");
			entry_ano_oc.Text = DateTime.Now.ToString("yyyy");
			
			entry_dia_fentrega.Text = DateTime.Now.ToString("dd");
			entry_mes_fentrega.Text = DateTime.Now.ToString("MM");
			entry_ano_fentrega.Text = DateTime.Now.ToString("yyyy");
			
			entry_lugar_entrega.Text = "ALMACEN GENERAL";
			
			button_orden_compra.Clicked += new EventHandler(on_button_orden_compra_clicked);
			// 
			button_asignar_proveedor.Clicked += new EventHandler(on_button_asignar_proveedor_clicked);
			// Busca proveedores
			button_busca_proveedores.Clicked += new EventHandler(on_button_busca_proveedores_clicked);
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			//imprime la informacion:
			button_imprimir.Clicked += new EventHandler(on_imprime_orden_clicked);
			button_mov_productos_oc.Clicked += new EventHandler(on_button_mov_productos_oc_clicked);
			//button_orden_compra
			button_prod_comprado.Clicked += new EventHandler(on_button_prod_comprado_clicked);
			checkbutton_all_deptos.Clicked += new EventHandler(on_checkbutton_all_deptos_clicked);
			crea_treeview_ordencompra();
			cree_treeview_departamentos();
			llenado_treeview_departamentos();
			
			llenado_combobox(0,"",combobox_condicion_oc,"array","","","",args_tipoordencompra,args_id_array,"");
			llenado_combobox(0,"",combobox_facturar_a,"sql","SELECT * FROM osiris_erp_emisor ORDER BY emisor;","emisor","id_emisor",args_args,args_id_array,"");
			
			statusbar.Pop(0);
			statusbar.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar.HasResizeGrip = false;
		}
		
		void on_button_mov_productos_oc_clicked(object sender, EventArgs args)
		{
			new osiris.movimientos_productos(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"productos_comprados","");	
		}
		
		void llenado_combobox(int tipodellenado,string descrip_defaul,object obj,string sql_or_array,string query_SQL,string name_field_desc,string name_field_id,string[] args_array,int[] args_id_array,string name_field_id2)
		{			
			Gtk.ComboBox combobox_llenado = (Gtk.ComboBox) obj;
			//Gtk.ComboBox combobox_pos_neg = obj as Gtk.ComboBox;
			//Console.WriteLine((string) combobox_llenado.GetType().ToString());
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
						store.AppendValues ((string) lector[name_field_desc ], (int) lector[name_field_id],false);
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
				case "combobox_condicion_oc":
					tipodeordencompra = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
				case "combobox_facturar_a":
					idreceptor = (int) onComboBoxChanged.Model.GetValue(iter,1);
					break;
				}
			}
		}
		
		void on_button_orden_compra_clicked(object sender, EventArgs args)
		{
			if (contador_prod_asignados != 0 ){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de CREAR una Nueva ORDEN DE COMPRA ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
					if(idreceptor != 1){
						ultimaorden = int.Parse(classpublic.lee_ultimonumero_registrado("osiris_erp_ordenes_compras_enca","numero_orden_compra",""));
						TreeIter iterSelected;
						//this.treeViewEngineProductosaComprar.GetSortColumnId(out iterSelected,			
						if (treeViewEngineProductosaComprar.GetIterFirst(out iterSelected)){
							if(selecciono_productos > 0){
								// creando encabezado de la orden de compra
								NpgsqlConnection conexion1;
								conexion1 = new NpgsqlConnection (connectionString+nombrebd );
								// Verifica que la base de datos este conectada
								try{
									conexion1.Open ();
									NpgsqlCommand comando; 
									comando = conexion1.CreateCommand ();
									
									comando.CommandText = "INSERT INTO osiris_erp_ordenes_compras_enca ("+
															"id_proveedor,"+
			 												"fechahora_creacion,"+
			 												"fecha_solicitud,"+
			 												"fecha_de_entrega," +
			 												"fecha_deorden_compra,"+
			 												"id_quien_creo,"+
			 												"lugar_de_entrega,"+
			 												"embarque,"+
			 												"condiciones_de_pago,"+
			 												"descripcion_proveedor,"+
			 												"numero_requisiciones,"+
			 												"direccion_proveedor,"+
			 												"telefonos_proveedor,"+
			 												"contacto_proveedor,"+
			 												"correo_electronico,"+
			 												"rfc_proveedor,"+
			 												"faxnextel_proveedor,"+
			 												"numero_orden_compra," +
															"observaciones," +
			 												"tipo_orden_compra," +
			 												"id_emisor) "+
															"VALUES ('"+
			 												entry_id_proveedor.Text.ToString().Trim()+"','"+//id_prod
					 										DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
															entry_ano_fentrega.Text.Trim()+"-"+entry_mes_fentrega.Text.Trim()+"-"+entry_dia_fentrega.Text.Trim()+"','"+
					 										DateTime.Now.ToString("yyyy-MM-dd")+"','"+//fechahora_solicitud
					 										entry_ano_oc.Text+"-"+entry_mes_oc.Text+"-"+entry_dia_oc.Text+"','"+
					 										LoginEmpleado+"','"+//id_empleado
					 										entry_lugar_entrega.Text.ToUpper()+"','"+
					 										entry_lugar_entrega.Text.ToUpper()+"','"+
					 										entry_formapago.Text+"','"+
			 												entry_nombre_proveedor.Text.Trim().ToUpper()+"','"+
			 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,02).ToString().Trim()+"','"+
			 												entry_direccion_proveedor.Text.ToString().Trim()+"','"+
			 												entry_tel_proveedor.Text.ToString().ToUpper()+"','"+
			 												"','"+
			 												"','"+
			 												entry_rfc_proveedor.Text.ToString().Trim()+"','"+
															entry_tel_proveedor.Text.ToString().ToUpper()+"','"+		 												
															int.Parse(classpublic.lee_ultimonumero_registrado("osiris_erp_ordenes_compras_enca","numero_orden_compra","")).ToString().Trim()+"','"+	
			 												entry_observaciones.Text.ToUpper().Trim()+"','"+
															tipodeordencompra+"','"+											
															idreceptor.ToString().Trim()+"');";
									Console.WriteLine(comando.CommandText);							
									comando.ExecuteNonQuery(); 	    comando.Dispose();								
									if ((bool)lista_productos_a_comprar.Model.GetValue (iterSelected,0) == true){
										NpgsqlConnection conexion3; 
										conexion3 = new NpgsqlConnection (connectionString+nombrebd);
										try{
											conexion3.Open ();
											NpgsqlCommand comando3; 
											comando3 = conexion3.CreateCommand();
											comando3.CommandText =  "UPDATE osiris_erp_requisicion_deta SET id_quien_compro = ' "+LoginEmpleado+"', "+
															    "fechahora_compra = '"+DateTime.Now.ToString("yyyy-MM-dd")+"', "+
															    "comprado = 'true'," +
															    "id_emisor = '"+idreceptor.ToString().Trim()+"',"+
																"cantidad_comprada = '"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,3).ToString().Trim()+"', "+
															    "id_proveedor = '"+entry_id_proveedor.Text.ToString().Trim()+"', "+
															    "numero_orden_compra = '"+ultimaorden.ToString()+"',"+
																"precio_costo_prov_selec ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,10).ToString().Trim()+"',"+
																"precio_unitario_prov_selec ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,11).ToString().Trim()+"' "+
											                    "WHERE id_requisicion ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,2).ToString().Trim()+"' "+
											                    "AND id_producto ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,6).ToString().Trim()+"' ;"; 
											Console.WriteLine(comando3.CommandText);
											comando3.ExecuteNonQuery();
											comando3.Dispose();
										}catch (NpgsqlException ex){
											MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										                                               MessageType.Error,
										                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
											msgBoxError.Run ();
										}
										conexion3.Close();
									}
									while (this.treeViewEngineProductosaComprar.IterNext(ref iterSelected)){
										if ((bool)this.lista_productos_a_comprar.Model.GetValue (iterSelected,0) == true){
											NpgsqlConnection conexion3; 
											conexion3 = new NpgsqlConnection (connectionString+nombrebd);
											try{
												conexion3.Open ();
												NpgsqlCommand comando3; 
												comando3 = conexion3.CreateCommand();
												comando3.CommandText =  "UPDATE osiris_erp_requisicion_deta SET id_quien_compro = ' "+LoginEmpleado+"', "+
																    "fechahora_compra = '"+DateTime.Now.ToString("yyyy-MM-dd")+"', "+
																    "comprado = 'true'," +
														 			"id_emisor = '"+idreceptor.ToString().Trim()+"',"+
																    "cantidad_comprada = '"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,3).ToString().Trim()+"', "+
																    "id_proveedor = '"+entry_id_proveedor.Text.ToString().Trim()+"', "+
																    "numero_orden_compra = '"+ultimaorden.ToString()+"',"+
																	"precio_costo_prov_selec ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,10).ToString().Trim()+"',"+
																	"precio_unitario_prov_selec ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,11).ToString().Trim()+"' "+
												                    "WHERE id_requisicion ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,2).ToString().Trim()+"' "+
												                    "AND id_producto ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,6).ToString().Trim()+"' ;"; 
												Console.WriteLine(comando3.CommandText);
												comando3.ExecuteNonQuery();
												comando3.Dispose();
											}catch (NpgsqlException ex){
												MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											                                               MessageType.Error,
											                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
												msgBoxError.Run ();
											}
											conexion3.Close();
										}
									}
									llena_requiciones_para_comprar(departamentos_seleccionados);
								}catch (NpgsqlException ex){
									MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																		MessageType.Error, 
																		ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
									msgBoxError.Run ();
								}
							}else{
								MessageDialog msgBoxError1 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									                                    MessageType.Error,ButtonsType.Close, "ERROR usted seleccionado ningun producto para generar la orden de compra, verifique...");
								msgBoxError1.Run ();			msgBoxError1.Destroy();
							}	
						}
					}else{
						MessageDialog msgBoxError1 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									                                    MessageType.Error,ButtonsType.Close, "ERROR usted no asigno ningun RECEPTOR...");
						msgBoxError1.Run ();			msgBoxError1.Destroy();
					}
				}else{
					MessageDialog msgBoxError1 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									                                    MessageType.Error,ButtonsType.Close, "ERROR usted no asigno ningun PROVEEDOR...");
					msgBoxError1.Run ();			msgBoxError1.Destroy();
				}
			}
		}
		
		void on_checkbutton_all_deptos_clicked(object sender, EventArgs args)
		{			
			verifica_grupo_prodctos();
		}
		
		void verifica_grupo_prodctos()
		{
			TreeIter iter2;
			string departamentos_seleccionados = "";
			if ((bool) checkbutton_all_deptos.Active == true){
				if (treeViewEngineListaDepartamentos.GetIterFirst (out iter2)){
					treeview_lista_departamentos.Model.SetValue(iter2,0,true);
					departamentos_seleccionados = Convert.ToString((int) treeview_lista_departamentos.Model.GetValue (iter2,2));
					while (treeViewEngineListaDepartamentos.IterNext(ref iter2)){
						treeview_lista_departamentos.Model.SetValue(iter2,0,true);
					}
				}
				//query_in_grupo = "";
			}else{
				if (treeViewEngineListaDepartamentos.GetIterFirst (out iter2)){
					treeview_lista_departamentos.Model.SetValue(iter2,0,false);
					while (treeViewEngineListaDepartamentos.IterNext(ref iter2)){
						treeview_lista_departamentos.Model.SetValue(iter2,0,false);
					}
				}
			}
			llena_requiciones_para_comprar(departamentos_seleccionados);
		}
		
		void on_button_prod_comprado_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
			MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro marcar los productos como comprados?");
			ResponseType miResultado = (ResponseType)
			msgBox.Run ();				msgBox.Destroy();
	 		if (miResultado == ResponseType.Yes){
				TreeModel model;
				TreeIter iterSelected;
 				if (lista_productos_a_comprar.Selection.GetSelected(out model, out iterSelected)){
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					// Verifica que la base de datos este conectada
					try{							
						if ((bool)lista_productos_a_comprar.Model.GetValue (iterSelected,21) == true){
							comando.CommandText = "UPDATE osiris_erp_requisicion_deta SET id_quien_compro = ' "+LoginEmpleado+"', "+
												"fechahora_recibido = '"+DateTime.Now.ToString("yyyy-MM-dd")+"', "+
											    "recibido = 'true' "+
												//"id_proveedor = '"+variable_paso_02+"', "+
											    //"numero_orden_compra = '"+ultimaorden.ToString()+"',"+
												//"precio_costo_prov_selec ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,10).ToString().Trim()+"',"+
												//"precio_unitario_prov_selec ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,11).ToString().Trim()+"' "+
							                    "WHERE id_secuencia ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,16).ToString().Trim()+"';";
							                    //"AND id_producto ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,6).ToString().Trim()+"' ;"; 
							Console.WriteLine(comando.CommandText);    
							comando.ExecuteNonQuery();
							comando.Dispose();
							llena_requiciones_para_comprar(departamentos_seleccionados);
						}
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();				msgBoxError.Destroy();
					}
					conexion.Close ();
				}
			}
		}
		
		void on_imprime_orden_clicked(object sender, EventArgs args)
		{
			new osiris.lista_ordenes_compra();
		}	
		
		void on_button_asignar_proveedor_clicked (object sender, EventArgs args)
		{
			TreeIter iter;
			if (treeViewEngineProductosaComprar.GetIterFirst (out iter)){
				// buscar el producto en el catalogo del proveedor
				contador_prod_asignados = 0;
				contador_prod_noasignad = 0;
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				// Verifica que la base de datos este conectada
				try{							
					if ((bool) this.lista_productos_a_comprar.Model.GetValue (iter,0) == true){
               			comando.CommandText = "SELECT osiris_catalogo_productos_proveedores.id_proveedor,"+
               								"to_char(osiris_catalogo_productos_proveedores.precio_costo,'9999999.99') AS preciocosto,"+ 
               								"to_char(osiris_catalogo_productos_proveedores.precio_costo_unitario,'9999999.99') AS preciocostounitario,"+ 
               								"osiris_catalogo_productos_proveedores.id_producto,osiris_catalogo_productos_proveedores.codigo_producto_proveedor,"+
               								"osiris_catalogo_productos_proveedores.codigo_de_barra,clave,osiris_catalogo_productos_proveedores.marca_producto,"+
               								"osiris_erp_proveedores.direccion_proveedor,osiris_erp_proveedores.telefono1_proveedor,"+
               								"osiris_erp_proveedores.contacto1_proveedor,osiris_erp_proveedores.mail_proveedor,"+
               								"osiris_erp_proveedores.rfc_proveedor,osiris_erp_proveedores.fax_proveedor "+
               								"FROM osiris_catalogo_productos_proveedores,osiris_erp_proveedores,osiris_productos "+
               								"WHERE osiris_catalogo_productos_proveedores.id_producto = '"+(string) this.lista_productos_a_comprar.Model.GetValue (iter,6).ToString().Trim()+"' "+
               								"AND osiris_catalogo_productos_proveedores.id_proveedor = '"+(string) this.entry_id_proveedor.Text.ToString().Trim()+"' "+
											"AND osiris_catalogo_productos_proveedores.id_producto = osiris_productos.id_producto "+
               								"AND osiris_catalogo_productos_proveedores.id_proveedor = osiris_erp_proveedores.id_proveedor;";
						Console.WriteLine(comando.CommandText);
						NpgsqlDataReader lector = comando.ExecuteReader ();
						
               			if (lector.Read()){						
							lista_productos_a_comprar.Model.SetValue(iter,0,true);							
							lista_productos_a_comprar.Model.SetValue(iter,10,(string) lector["preciocosto"]);					// precio prov
							lista_productos_a_comprar.Model.SetValue(iter,11,(string) lector["preciocostounitario"]);			// precio unitario
							lista_productos_a_comprar.Model.SetValue(iter,13,(string) lector["codigo_producto_proveedor"]);	// Codigo
							lista_productos_a_comprar.Model.SetValue(iter,14,(string) lector["codigo_de_barra"]);				// Barras						
							lista_productos_a_comprar.Model.SetValue(iter,12,(string) entry_nombre_proveedor.Text);		// Nombre Proveedor
							lista_productos_a_comprar.Model.SetValue(iter,15,(string) entry_id_proveedor.Text);			// Id Proveedor							
							lista_productos_a_comprar.Model.SetValue(iter,16,(string) lector["direccion_proveedor"]);
							contador_prod_asignados += 1;
						}else{
							lista_productos_a_comprar.Model.SetValue(iter,0,false);
							contador_prod_noasignad += 1;
						}
					}						
					while (this.treeViewEngineProductosaComprar.IterNext(ref iter)){
						if ((bool)lista_productos_a_comprar.Model.GetValue (iter,0) == true){
				
							// buscar el producto en el catalogo del proveedor
							comando.CommandText = "SELECT osiris_catalogo_productos_proveedores.id_proveedor,"+
               								"to_char(osiris_catalogo_productos_proveedores.precio_costo,'9999999.99') AS preciocosto,"+ 
               								"to_char(osiris_catalogo_productos_proveedores.precio_costo_unitario,'9999999.99') AS preciocostounitario,"+ 
               								"osiris_catalogo_productos_proveedores.id_producto,osiris_catalogo_productos_proveedores.codigo_producto_proveedor,"+
               								"osiris_catalogo_productos_proveedores.codigo_de_barra,clave,osiris_catalogo_productos_proveedores.marca_producto,"+
               								"osiris_erp_proveedores.direccion_proveedor,osiris_erp_proveedores.telefono1_proveedor,"+
               								"osiris_erp_proveedores.contacto1_proveedor,osiris_erp_proveedores.mail_proveedor,"+
               								"osiris_erp_proveedores.rfc_proveedor,osiris_erp_proveedores.fax_proveedor "+
               								"FROM osiris_catalogo_productos_proveedores,osiris_erp_proveedores,osiris_productos "+
               								"WHERE osiris_catalogo_productos_proveedores.id_producto = '"+(string) this.lista_productos_a_comprar.Model.GetValue (iter,6).ToString().Trim()+"' "+
               								"AND osiris_catalogo_productos_proveedores.id_proveedor = '"+(string) this.entry_id_proveedor.Text.ToString().Trim()+"' "+
											"AND osiris_catalogo_productos_proveedores.id_producto = osiris_productos.id_producto "+
               								"AND osiris_catalogo_productos_proveedores.id_proveedor = osiris_erp_proveedores.id_proveedor;";
							Console.WriteLine(comando.CommandText);
							NpgsqlDataReader lector = comando.ExecuteReader ();						
               				if (lector.Read()){					
								lista_productos_a_comprar.Model.SetValue(iter,0,true);
								lista_productos_a_comprar.Model.SetValue(iter,10,(string) lector["preciocosto"]);				// precio prov
								lista_productos_a_comprar.Model.SetValue(iter,11,(string) lector["preciocostounitario"]);		// precio unitario
								lista_productos_a_comprar.Model.SetValue(iter,13,(string) lector["codigo_producto_proveedor"]);	// Codigo
								lista_productos_a_comprar.Model.SetValue(iter,14,(string) lector["codigo_de_barra"]);			// Barras
								lista_productos_a_comprar.Model.SetValue(iter,12,(string) entry_nombre_proveedor.Text);			// actualiza treeview con el nombre del proveedor
								lista_productos_a_comprar.Model.SetValue(iter,15,(string) entry_id_proveedor.Text);  			// almacena el id del proveedor							
								lista_productos_a_comprar.Model.SetValue(iter,16,(string) lector["direccion_proveedor"]);
								contador_prod_asignados += 1;
							}else{
								lista_productos_a_comprar.Model.SetValue(iter,0,false);
								contador_prod_noasignad += 1;
							}				
						}
					}					
					MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info,ButtonsType.Close,"Productos Asignados a Proveedor = "+contador_prod_asignados.ToString().Trim()+"\n"+
					                                          "No Asigandos = "+contador_prod_noasignad.ToString().Trim());
					msgBox.Run ();				msgBox.Destroy();					
					Console.WriteLine("contador_prod_asignados = "+contador_prod_asignados.ToString());
					Console.WriteLine("contador_prod_noasignad = "+contador_prod_noasignad.ToString());					
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
				conexion.Close ();
			}
		}
		
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if((bool) this.lista_productos_a_comprar.Model.GetValue (iter,21) == false){
				if((int) Convert.ToSingle ((string) this.lista_productos_a_comprar.Model.GetValue (iter,15)) > 1){
					if(cell.GetType().ToString() == "Gtk.CellRendererToggle"){
						(cell as Gtk.CellRendererToggle).CellBackground = "yellow";
					}
					if(cell.GetType().ToString() == "Gtk.CellRendererText"){
						(cell as Gtk.CellRendererText).CellBackground = "yellow";
					}
				}else{
					if(cell.GetType().ToString() == "Gtk.CellRendererToggle"){
						(cell as Gtk.CellRendererToggle).CellBackground = "white";
						
					}
					if(cell.GetType().ToString() == "Gtk.CellRendererText"){
						(cell as Gtk.CellRendererText).CellBackground = "white";
					}
				}
				cellrt_00.Activatable = true;
				cell.Sensitive = true;
			}else{
				cell.Sensitive = false;
				if(cell.GetType().ToString() == "Gtk.CellRendererToggle"){
					(cell as Gtk.CellRendererToggle).CellBackground = "red";
					cellrt_00.Activatable = false;					
				}
				if(cell.GetType().ToString() == "Gtk.CellRendererText"){
					(cell as Gtk.CellRendererText).CellBackground = "red";
				}
			}
		}
		
		void llena_requiciones_para_comprar(string departamentos_seleccionados)
		{
			treeViewEngineProductosaComprar.Clear();
			if(departamentos_seleccionados != ""){
				// lleno de la tabla de his_tipo_de_admisiones
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
	               	comando.CommandText = "SELECT to_char(osiris_erp_requisicion_deta.id_secuencia,'9999999999') AS idsecuencia," +
	               				"id_requisicion,to_char(osiris_erp_requisicion_deta.id_producto,'999999999999') AS idproducto,"+
								"to_char(cantidad_solicitada,'999999.99') AS cantidadsolicitada,comprado,recibido,"+
								"to_char(id_requisicion,'9999999999') AS idrequisicion,"+
								"osiris_productos.descripcion_producto,to_char(osiris_productos.cantidad_de_embalaje,'9999.99') AS cantidadembalaje,"+
								"osiris_productos.tipo_unidad_producto,to_char(numero_orden_compra,'9999999999') AS numeroordencompra," +
								"to_char(fechahora_requisado,'yyyy-MM-dd') AS fechahorarequisados,"+
								"autorizada,to_char(fechahora_autorizado,'yyyy-MM-dd') AS fechahoraautorizado,"+
								"to_char(fechahora_compra,'yyyy-MM-dd') AS fechahoracompra,"+
								"to_char(osiris_productos.costo_por_unidad,'99999999.99') AS costoporunidad,"+
								"to_char(osiris_productos.costo_producto,'99999999.99') AS costoproducto,"+
								"to_char(osiris_productos.cantidad_de_embalaje,'9999999.99') AS cantidaddeembalaje,"+
								"to_char(osiris_erp_requisicion_deta.id_proveedor,'9999999999') AS idproveedor,descripcion_proveedor,"+
								"osiris_erp_requisicion_deta.id_tipo_admisiones,descripcion_admisiones "+
								"FROM osiris_erp_requisicion_deta,osiris_productos,osiris_his_tipo_admisiones,osiris_erp_proveedores "+
								"WHERE osiris_erp_requisicion_deta.id_producto = osiris_productos.id_producto "+
								"AND osiris_his_tipo_admisiones.id_tipo_admisiones = osiris_erp_requisicion_deta.id_tipo_admisiones "+
								"AND osiris_erp_requisicion_deta.id_proveedor = osiris_erp_proveedores.id_proveedor "+
								"AND osiris_erp_requisicion_deta.id_tipo_admisiones IN('"+departamentos_seleccionados+"') "+
								"AND autorizada = 'true' "+
								"AND eliminado = 'false' "+
							    "AND recibido = 'false' "+
								"ORDER BY id_requisicion DESC;";
					Console.WriteLine(comando.CommandText.ToString());
					NpgsqlDataReader lector = comando.ExecuteReader ();
	               	while (lector.Read()){
						treeViewEngineProductosaComprar.AppendValues(false,
												(string) lector["descripcion_admisiones"],
												(string) lector["idrequisicion"],
												(string) lector["cantidadsolicitada"],
												(string) lector["descripcion_producto"],
												(string) lector["tipo_unidad_producto"],
												(string) lector["idproducto"],
												(string) lector["costoporunidad"],
												(string) lector["costoproducto"],
												(string) lector["cantidaddeembalaje"],
												"",
												"",
												(string) lector["descripcion_proveedor"],
												"",
												"",
												(string) lector["idproveedor"],
												(string) lector["idsecuencia"],
						                        (string) lector["fechahorarequisados"],
						                        (string) lector["fechahoraautorizado"],
						                        (string) lector["numeroordencompra"],
						                        (bool) lector["recibido"],
						                        (bool) lector["comprado"]);
										
						col_00.SetCellDataFunc(cellrt_00, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_01.SetCellDataFunc(cellrt_01, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_19.SetCellDataFunc(cellrt_19, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_02.SetCellDataFunc(cellrt_02, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_17.SetCellDataFunc(cellrt_17, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_18.SetCellDataFunc(cellrt_18, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_03.SetCellDataFunc(cellrt_03, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_09.SetCellDataFunc(cellrt_09, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_04.SetCellDataFunc(cellrt_04, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_05.SetCellDataFunc(cellrt_05, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_06.SetCellDataFunc(cellrt_06, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_07.SetCellDataFunc(cellrt_07, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_08.SetCellDataFunc(cellrt_08, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_10.SetCellDataFunc(cellrt_10, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_11.SetCellDataFunc(cellrt_11, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_12.SetCellDataFunc(cellrt_12, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_13.SetCellDataFunc(cellrt_13, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_14.SetCellDataFunc(cellrt_14, new Gtk.TreeCellDataFunc(cambia_colores_fila));						
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
				conexion.Close ();
			}
		}
		
		void llenado_treeview_departamentos()
		{
			// lleno de la tabla de his_tipo_de_admisiones
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_his_tipo_admisiones "+
               						//"WHERE id_tipo_admisiones IN('"+accesocentrocosto+"')"+
               						"ORDER BY descripcion_admisiones;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read()){
					if((string) lector["descripcion_admisiones"].ToString().Trim() != ""){
						treeViewEngineListaDepartamentos.AppendValues(false,(string) lector["descripcion_admisiones"], (int) lector["id_tipo_admisiones"]);
					}
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		// Creando el treeview para la requisicion
		void crea_treeview_ordencompra()
		{
			treeViewEngineProductosaComprar = new TreeStore(typeof(bool), 
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
													typeof(bool));							
												
			lista_productos_a_comprar.Model = treeViewEngineProductosaComprar;			
			lista_productos_a_comprar.RulesHint = true;
						
			col_00 = new TreeViewColumn();
			cellrt_00 = new CellRendererToggle();
			col_00.Title = "Seleccion";
			col_00.PackStart(cellrt_00, true);
			col_00.AddAttribute (cellrt_00, "active", 0);
			cellrt_00.Activatable = true;
			cellrt_00.Toggled += selecciona_fila;
			col_00.SortColumnId = (int) col_ordencompra.col_00;
			
			col_01 = new TreeViewColumn();
			cellrt_01 = new CellRendererText();
			col_01.Title = "Solicitado Por";
			col_01.PackStart(cellrt_01, true);
			col_01.AddAttribute (cellrt_01, "text", 1);
			col_01.SortColumnId = (int) col_ordencompra.col_01;
												
			col_02 = new TreeViewColumn();
			cellrt_02 = new CellRendererText();
			col_02.Title = "Nº Requi.";
			col_02.PackStart(cellrt_02, true);
			col_02.AddAttribute (cellrt_02, "text", 2);
			col_02.SortColumnId = (int) col_ordencompra.col_02;
			
			col_03 = new TreeViewColumn();
			cellrt_03 = new CellRendererText();
			col_03.Title = "Cantidad";
			col_03.PackStart(cellrt_03, true);
			col_03.AddAttribute (cellrt_03, "text", 3);
			col_03.SortColumnId = (int) col_ordencompra.col_03;
			cellrt_03.Editable = true;
			cellrt_03.Edited += NumberCellEdited_Autorizado_1;
			
			col_04 = new TreeViewColumn();
			cellrt_04 = new CellRendererText();
			col_04.Title = "Descripcion producto";
			col_04.PackStart(cellrt_04, true);
			col_04.AddAttribute (cellrt_04, "text", 4);
			col_04.SortColumnId = (int) col_ordencompra.col_04;
			col_04.Resizable = true;
			cellrt_04.Width = 350;
			
			col_05 = new TreeViewColumn();
			cellrt_05 = new CellRendererText();
			col_05.Title = "Unidad Prod.";
			col_05.PackStart(cellrt_05, true);
			col_05.AddAttribute (cellrt_05, "text", 5);
			col_05.SortColumnId = (int) col_ordencompra.col_05;
						
			col_06 = new TreeViewColumn();
			cellrt_06 = new CellRendererText();
			col_06.Title = "Codigo Producto";
			col_06.PackStart(cellrt_06, true);
			col_06.AddAttribute (cellrt_06, "text", 6);
			col_06.SortColumnId = (int) col_ordencompra.col_06;
						
			col_07 = new TreeViewColumn();
			cellrt_07 = new CellRendererText();
			col_07.Title = "Precio Unit.";
			col_07.PackStart(cellrt_07, true);
			col_07.AddAttribute (cellrt_07, "text", 7);
			col_07.SortColumnId = (int) col_ordencompra.col_07;
			
			col_08 = new TreeViewColumn();
			cellrt_08 = new CellRendererText();
			col_08.Title = "Precio Produ.";
			col_08.PackStart(cellrt_08, true);
			col_08.AddAttribute (cellrt_08, "text", 8);
			col_08.SortColumnId = (int) col_ordencompra.col_08;
			cellrt_08.Editable = true;
			
			col_09 = new TreeViewColumn();
			cellrt_09 = new CellRendererText();
			col_09.Title = "Embalaje";
			col_09.PackStart(cellrt_09, true);
			col_09.AddAttribute (cellrt_09, "text", 9);
			col_09.SortColumnId = (int) col_ordencompra.col_09;
			cellrt_09.Editable = true;
			cellrt_09.Edited += NumberCellEdited_Autorizado_2;
			
			col_10 = new TreeViewColumn();
			cellrt_10 = new CellRendererText();
			col_10.Title = "Precio Prov.";
			col_10.PackStart(cellrt_10, true);
			col_10.AddAttribute (cellrt_10, "text", 10);
			col_10.SortColumnId = (int) col_ordencompra.col_10;
			
			col_11 = new TreeViewColumn();
			cellrt_11 = new CellRendererText();
			col_11.Title = "Precio Unit.Prov.";
			col_11.PackStart(cellrt_11, true);
			col_11.AddAttribute (cellrt_11, "text", 11);
			col_11.SortColumnId = (int) col_ordencompra.col_11;
									
			col_12 = new TreeViewColumn();
			cellrt_12 = new CellRendererText();
			col_12.Title = "Nombre Proveedor";
			col_12.PackStart(cellrt_12, true);
			col_12.AddAttribute (cellrt_12, "text", 12);
			col_12.SortColumnId = (int) col_ordencompra.col_12;
			
			col_13 = new TreeViewColumn();
			cellrt_13 = new CellRendererText();
			col_13.Title = "Cod.Prod.Prove.";
			col_13.PackStart(cellrt_13, true);
			col_13.AddAttribute (cellrt_13, "text", 13);
			col_13.SortColumnId = (int) col_ordencompra.col_13;
			
			col_14 = new TreeViewColumn();
			cellrt_14 = new CellRendererText();
			col_14.Title = "Cod. Barras";
			col_14.PackStart(cellrt_14, true);
			col_14.AddAttribute (cellrt_14, "text", 14);
			col_14.SortColumnId = (int) col_ordencompra.col_14;
			
			col_17 = new TreeViewColumn();
			cellrt_17 = new CellRendererText();
			col_17.Title = "Fech/Requi.";
			col_17.PackStart(cellrt_17, true);
			col_17.AddAttribute (cellrt_17, "text", 17);
			col_17.SortColumnId = (int) col_ordencompra.col_17;
			
			col_18 = new TreeViewColumn();
			cellrt_18 = new CellRendererText();
			col_18.Title = "Fech/Autori.";
			col_18.PackStart(cellrt_18, true);
			col_18.AddAttribute (cellrt_18, "text", 18);
			col_18.SortColumnId = (int) col_ordencompra.col_18;
			
			col_19 = new TreeViewColumn();
			cellrt_19 = new CellRendererText();
			col_19.Title = "N° O.C.";
			col_19.PackStart(cellrt_19, true);
			col_19.AddAttribute (cellrt_19, "text", 19);
			col_19.SortColumnId = (int) col_ordencompra.col_19;
			
			lista_productos_a_comprar.AppendColumn(col_00);
			lista_productos_a_comprar.AppendColumn(col_01);
			lista_productos_a_comprar.AppendColumn(col_19);
			lista_productos_a_comprar.AppendColumn(col_02);
			lista_productos_a_comprar.AppendColumn(col_17);
			lista_productos_a_comprar.AppendColumn(col_18);
			lista_productos_a_comprar.AppendColumn(col_03);
			lista_productos_a_comprar.AppendColumn(col_09);
			lista_productos_a_comprar.AppendColumn(col_04);
			lista_productos_a_comprar.AppendColumn(col_05);
			lista_productos_a_comprar.AppendColumn(col_06);
			lista_productos_a_comprar.AppendColumn(col_07);
			lista_productos_a_comprar.AppendColumn(col_08);
			lista_productos_a_comprar.AppendColumn(col_10);
			lista_productos_a_comprar.AppendColumn(col_11);			
			lista_productos_a_comprar.AppendColumn(col_12);
			lista_productos_a_comprar.AppendColumn(col_13);
			lista_productos_a_comprar.AppendColumn(col_14);
		}
		
		enum col_ordencompra
		{
			col_00,	
			col_01,
			col_19,
			col_02,
			col_17,
			col_18,
			col_03,
			col_09,
			col_04,
			col_05,
			col_06,
			col_07,
			col_08,
			col_10,
			col_11,
			col_12,
			col_13,
			col_14
		}
		
		void cree_treeview_departamentos()
		{
			treeViewEngineListaDepartamentos = new TreeStore(typeof(bool), 
													typeof(string),
													typeof(int));
			
			treeview_lista_departamentos.Model = treeViewEngineListaDepartamentos;			
			treeview_lista_departamentos.RulesHint = true;
						
			Gtk.TreeViewColumn col00 = new TreeViewColumn();
			Gtk.CellRendererToggle celrt00 = new CellRendererToggle();
			col00.Title = "Seleccion";
			col00.PackStart(celrt00, true);
			col00.AddAttribute (celrt00, "active", 0);
			celrt00.Activatable = true;
			celrt00.Toggled += selecciona_departamento;
						
			Gtk.TreeViewColumn col01 = new TreeViewColumn();
			Gtk.CellRendererText cellr1 = new CellRendererText();
			col01.Title = "Departamento";
			col01.PackStart(cellr1, true);
			col01.AddAttribute (cellr1, "text", 1);
			
			treeview_lista_departamentos.AppendColumn(col00);
			treeview_lista_departamentos.AppendColumn(col01);			
		}
		
		void NumberCellEdited_Autorizado_1(object o, EditedArgs args)
		{
			Gtk.CellRendererText onRendererChanged = o as Gtk.CellRendererText;
			Console.WriteLine(onRendererChanged.ToString());
			Gtk.TreeIter iter;
			bool esnumerico = false;
			int var_paso = 0;
			int largo_variable = args.NewText.ToString().Length;
			string toma_variable = args.NewText.ToString();
						
			treeViewEngineProductosaComprar.GetIter (out iter, new Gtk.TreePath (args.Path));
			
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
				treeViewEngineProductosaComprar.SetValue(iter,(int) col_ordencompra.col_03,args.NewText);
			}
 		}
		
		void NumberCellEdited_Autorizado_2(object o, EditedArgs args)
		{
			Gtk.CellRendererText onRendererChanged = o as Gtk.CellRendererText;
			Console.WriteLine(onRendererChanged.ToString());
			Gtk.TreeIter iter;
			bool esnumerico = false;
			int var_paso = 0;
			int largo_variable = args.NewText.ToString().Length;
			string toma_variable = args.NewText.ToString();
						
			treeViewEngineProductosaComprar.GetIter (out iter, new Gtk.TreePath (args.Path));
			
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
				treeViewEngineProductosaComprar.SetValue(iter,(int) col_ordencompra.col_09,args.NewText);
			}
 		}
		
		void on_button_busca_proveedores_clicked(object sender, EventArgs args)
		{
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion
			// la clase recibe tambien el orden del query
			// es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo
			object[] parametros_objetos = {entry_id_proveedor,entry_nombre_proveedor,entry_formapago,entry_direccion_proveedor,entry_tel_proveedor,entry_rfc_proveedor};
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
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_proveedores_newoc"," ORDER BY descripcion_proveedor;","%' ",0);
		}
			
		// Cuando seleccion campos para la autorizacion de compras  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (lista_productos_a_comprar.Model.GetIter (out iter, path)){					
				bool old = (bool) this.lista_productos_a_comprar.Model.GetValue(iter,0);
				if(!old == true){
					selecciono_productos += 1;
				}else{
					selecciono_productos -= 1;
				}
				//Console.WriteLine(selecciono_productos.ToString());
				lista_productos_a_comprar.Model.SetValue(iter,0,!old);
			}				
		}
		
		void selecciona_departamento(object sender, ToggledArgs args)
		{
			int variable_paso_02_1 = 0;
			departamentos_seleccionados = "";
			TreeIter iter;
			TreePath path = new TreePath (args.Path);	
			if (treeview_lista_departamentos.Model.GetIter (out iter, path)){					
				bool old = (bool) treeview_lista_departamentos.Model.GetValue(iter,0);
				treeview_lista_departamentos.Model.SetValue(iter,0,!old);
				idtipointernamiento = (int) treeview_lista_departamentos.Model.GetValue(iter,2);
		    	descripinternamiento = (string) treeview_lista_departamentos.Model.GetValue(iter,1);				
				if (treeViewEngineListaDepartamentos.GetIterFirst (out iter)){			
	 				if ((bool) treeview_lista_departamentos.Model.GetValue(iter,0) == true){
						departamentos_seleccionados = departamentos_seleccionados + Convert.ToString((int) treeview_lista_departamentos.Model.GetValue (iter,2));
	 					variable_paso_02_1 += 1;		
	 				}
	 				while (treeViewEngineListaDepartamentos.IterNext(ref iter)){
	 					if ((bool) treeview_lista_departamentos.Model.GetValue(iter,0) == true){
							if (variable_paso_02_1 == 0){ 
	 							departamentos_seleccionados = departamentos_seleccionados + Convert.ToString((int) treeview_lista_departamentos.Model.GetValue (iter,2));
	 							variable_paso_02_1 += 1;
	 						}else{
	 							departamentos_seleccionados = departamentos_seleccionados + "','" + Convert.ToString((int) treeview_lista_departamentos.Model.GetValue (iter,2));
	 						}
	 					}
	 				}
					//Console.WriteLine(departamentos_seleccionados);
		    		llena_requiciones_para_comprar(departamentos_seleccionados);
				}				
			}				
		}
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;				
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