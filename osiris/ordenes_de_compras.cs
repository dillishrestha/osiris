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
		[Widget] Gtk.ComboBox combobox_tipo_admision = null;
		[Widget] Gtk.TreeView lista_productos_a_comprar = null;
		[Widget] Gtk.Button button_busca_proveedores = null;
		[Widget] Gtk.Button button_asignar_proveedor = null;
		[Widget] Gtk.Button button_mov_productos_oc = null;
		[Widget] Gtk.Button button_orden_compra = null;
		[Widget] Gtk.Button button_buscar_precio = null;
		[Widget] Gtk.Entry entry_id_proveedor = null;
		[Widget] Gtk.Entry entry_nombre_proveedor = null;
		[Widget] Gtk.Entry entry_formapago = null;
		[Widget] Gtk.Entry entry_dia_oc = null;
		[Widget] Gtk.Entry entry_mes_oc = null;
		[Widget] Gtk.Entry entry_ano_oc = null;
		[Widget] Gtk.Statusbar statusbar = null;
		[Widget] Gtk.TreeView treeview_lista_departamentos = null;
		
		[Widget] Gtk.Button button_prod_comprado = null;
				
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
    	
    	// Declarando las variables de publicas para uso dentro de classe
    	int idtipointernamiento;
    	string descripinternamiento = "";	// Descripcion de Centro de Costos - Solicitado por
		
		TreeStore treeViewEngineProductosaComprar;	// Lista de proctos que se van a comprar
		TreeStore treeViewEngineListaDepartamentos;
				
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;		
		
		TreeViewColumn col_autorizar;		CellRendererToggle cel_autorizar;
		TreeViewColumn col_solicitado_por;	CellRendererText cellr1;
		TreeViewColumn col_numero_req;		CellRendererText cellr2;
		TreeViewColumn col_cantidadcomprar;	CellRendererText cellr3;
		TreeViewColumn col_descripcion;		CellRendererText cellr4;
		TreeViewColumn col_unidades;		CellRendererText cellr5;
		TreeViewColumn col_codigo_prod;		CellRendererText cellr6;
		
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
			this.entry_dia_oc.Text = DateTime.Now.ToString("dd");
			this.entry_mes_oc.Text = DateTime.Now.ToString("MM");
			this.entry_ano_oc.Text = DateTime.Now.ToString("yyyy");
			
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
			
			statusbar.Pop(0);
			statusbar.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar.HasResizeGrip = false;
		}
		
		void on_button_mov_productos_oc_clicked(object sender, EventArgs args)
		{
			new osiris.movimientos_productos(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"productos_comprados","");	
		}
		
		void on_button_orden_compra_clicked(object sender, EventArgs args)
		{
			if (contador_prod_asignados != 0 ){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de CREAR una Nueva ORDEN DE COMPRA ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){							
					ultimaorden = int.Parse(classpublic.lee_ultimonumero_registrado("osiris_erp_ordenes_compras_enca","numero_orden_compra",""));
					bool variable_paso_01 = true;
					string variable_paso_02 = "";   // se usa para asignar el proveedor de que se utilizo en la requisicion
					TreeIter iterSelected;
					//this.treeViewEngineProductosaComprar.GetSortColumnId(out iterSelected,			
					if (treeViewEngineProductosaComprar.GetIterFirst(out iterSelected)){
						if ((bool)lista_productos_a_comprar.Model.GetValue (iterSelected,0) == true){
							if (variable_paso_01 == true){
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
			 												"fecha_de_entrega,"+
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
															//"subtotal_orden_compra,"+
															//"iva_orden_compra,"+
															//"total_orden_compra,"+
			 												"numero_orden_compra) "+
															"VALUES ('"+
			 												int.Parse((string) this.lista_productos_a_comprar.Model.GetValue(iterSelected,15))+"','"+//id_prod
					 										DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+//fechahora_creacion
					 										DateTime.Now.ToString("yyyy-MM-dd")+"','"+//fechahora_solicitud
					 										entry_ano_oc.Text+"-"+entry_mes_oc.Text+"-"+entry_dia_oc.Text+"','"+
					 										LoginEmpleado+"','"+//id_empleado
					 										"HOSPITAL"+"','"+
					 										"SU CONDUCTO"+"','"+
					 										entry_formapago.Text+"','"+
			 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,12).ToString().Trim()+"','"+
			 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,02).ToString().Trim()+"','"+
			 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,16).ToString().Trim()+"','"+
			 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,17).ToString().Trim()+"','"+
			 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,18).ToString().Trim()+"','"+
			 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,19).ToString().Trim()+"','"+
			 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,20).ToString().Trim()+"','"+
			 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,21).ToString().Trim()+"','"+
			 													 												
			 												//this.ultimaorden.ToString()+
															int.Parse(classpublic.lee_ultimonumero_registrado("osiris_erp_ordenes_compras_enca","numero_orden_compra","")).ToString()+	
			 												"');";
									//Console.WriteLine(comando.CommandText);							
									comando.ExecuteNonQuery(); 	    comando.Dispose();
									variable_paso_01 = false;
									
								}catch (NpgsqlException ex){
									MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																		MessageType.Error, 
																		ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
									msgBoxError.Run ();
								}
							}							
							if(variable_paso_01 == false){
								// Validando que seleccione un proveedor
								if (this.entry_id_proveedor.Text.Trim() == ""){
										variable_paso_02 = (string) this.lista_productos_a_comprar.Model.GetValue (iterSelected,15);
								}else{
										variable_paso_02 = this.entry_id_proveedor.Text;
								}
								
								NpgsqlConnection conexion3; 
								conexion3 = new NpgsqlConnection (connectionString+nombrebd);
								try{
									conexion3.Open ();
									NpgsqlCommand comando3; 
									comando3 = conexion3.CreateCommand();
									comando3.CommandText =  "UPDATE osiris_erp_requisicion_deta SET id_quien_compro = ' "+LoginEmpleado+"', "+
													    "fechahora_compra = '"+DateTime.Now.ToString("yyyy-MM-dd")+"', "+
													    "comprado = 'true', "+
													    "id_proveedor = '"+variable_paso_02+"', "+
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
					}					
					while (this.treeViewEngineProductosaComprar.IterNext(ref iterSelected)){							
						if ((bool)this.lista_productos_a_comprar.Model.GetValue (iterSelected,0) == true){
							if (variable_paso_01 == true){
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
			 												"fecha_de_entrega,"+
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
															//"subtotal_orden_compra,"+
															//"iva_orden_compra,"+
															//"total_orden_compra,"+
			 												"numero_orden_compra) "+
			 												"VALUES ('"+
			 												int.Parse((string) this.lista_productos_a_comprar.Model.GetValue(iterSelected,15))+"','"+//id_prod
					 										DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+//fechahora_creacion
					 										DateTime.Now.ToString("yyyy-MM-dd")+"','"+//fechahora_solicitud
					 										this.entry_ano_oc.Text+"-"+this.entry_mes_oc.Text+"-"+this.entry_dia_oc.Text+"','"+
					 										LoginEmpleado+"','"+//id_empleado
					 										"HOSPITAL"+"','"+
					 										"SU CONDUCTO"+"','"+
					 										this.entry_formapago.Text+"','"+
			 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,12)+"','"+
			 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,2)+"','"+
			 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,16)+"','"+
			 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,17)+"','"+
			 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,18)+"','"+
			 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,19)+"','"+
			 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,20)+"','"+
			 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,21)+"','"+
			 													 												
			 												ultimaorden.ToString()+
		
			 												"');";
									//Console.WriteLine(comando.CommandText);							
									comando.ExecuteNonQuery(); 	    comando.Dispose();
									variable_paso_01 = false;
									
								}catch (NpgsqlException ex){
									MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																		MessageType.Error, 
																		ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
									msgBoxError.Run ();
								}
							}							
							if(variable_paso_01 == false){
								// Validando que seleccione un proveedor
								if (entry_id_proveedor.Text.Trim() == ""){
									variable_paso_02 = (string)lista_productos_a_comprar.Model.GetValue (iterSelected,15);
								}else{
									variable_paso_02 = this.entry_id_proveedor.Text;
								}
								
								NpgsqlConnection conexion3; 
								conexion3 = new NpgsqlConnection (connectionString+nombrebd);
								try{
									conexion3.Open ();
									NpgsqlCommand comando3; 
									comando3 = conexion3.CreateCommand();
									comando3.CommandText =  "UPDATE osiris_erp_requisicion_deta SET id_quien_compro = ' "+LoginEmpleado+"', "+
													    "fechahora_compra = '"+DateTime.Now.ToString("yyyy-MM-dd")+"', "+
													    "comprado = 'true', "+
													    "id_proveedor = '"+variable_paso_02+"', "+
													    "numero_orden_compra = '"+ultimaorden.ToString()+"',"+
														"precio_costo_prov_selec ='"+(string) lista_productos_a_comprar.Model.GetValue (iterSelected,10).ToString().Trim()+"',"+
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
					}				
					MessageDialog msgBoxError1 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								                                               MessageType.Info,ButtonsType.Close, " La ORDEN DE COMPRA se creo CORRECTAMENTE con el Numero :"+ultimaorden.ToString().Trim());
					msgBoxError1.Run ();			msgBoxError1.Destroy();				
					llena_requiciones_para_comprar(departamentos_seleccionados);
				}
			}else{
					MessageDialog msgBoxError1 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								                                    MessageType.Error,ButtonsType.Close, "ERROR usted no asigno ningun PROVEEDOR...");
					msgBoxError1.Run ();			msgBoxError1.Destroy();
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
				TreeIter iterSelected;
				if (treeViewEngineProductosaComprar.GetIterFirst(out iterSelected)){
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					// Verifica que la base de datos este conectada
					try{							
						if ((bool)lista_productos_a_comprar.Model.GetValue (iterSelected,0) == true){
							comando.CommandText = "UPDATE osiris_erp_requisicion_deta SET id_quien_compro = ' "+LoginEmpleado+"', "+
												"fechahora_compra = '"+DateTime.Now.ToString("yyyy-MM-dd")+"', "+
											    "comprado = 'true' "+
												//"id_proveedor = '"+variable_paso_02+"', "+
											    //"numero_orden_compra = '"+ultimaorden.ToString()+"',"+
												//"precio_costo_prov_selec ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,10).ToString().Trim()+"',"+
												//"precio_unitario_prov_selec ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,11).ToString().Trim()+"' "+
							                    "WHERE id_secuencia ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,16).ToString().Trim()+"';";
							                    //"AND id_producto ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,6).ToString().Trim()+"' ;"; 
							Console.WriteLine(comando.CommandText);    
							comando.ExecuteNonQuery();
							comando.Dispose();
						}
						while (this.treeViewEngineProductosaComprar.IterNext(ref iterSelected)){							
							if ((bool)this.lista_productos_a_comprar.Model.GetValue (iterSelected,0) == true){
								comando.CommandText = "UPDATE osiris_erp_requisicion_deta SET id_quien_compro = ' "+LoginEmpleado+"', "+
												"fechahora_compra = '"+DateTime.Now.ToString("yyyy-MM-dd")+"', "+
											    "comprado = 'true' "+
												//"id_proveedor = '"+variable_paso_02+"', "+
											    //"numero_orden_compra = '"+ultimaorden.ToString()+"',"+
												//"precio_costo_prov_selec ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,10).ToString().Trim()+"',"+
												//"precio_unitario_prov_selec ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,11).ToString().Trim()+"' "+
							                    "WHERE id_secuencia ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,16).ToString().Trim()+"';";
							                    //"AND id_producto ='"+(string)lista_productos_a_comprar.Model.GetValue (iterSelected,6).ToString().Trim()+"' ;"; 
								Console.WriteLine(comando.CommandText);    
								comando.ExecuteNonQuery();
								comando.Dispose();
							}
						}
						llena_requiciones_para_comprar(departamentos_seleccionados);
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
							lista_productos_a_comprar.Model.SetValue(iter,17,(string) lector["telefono1_proveedor"]);		
							lista_productos_a_comprar.Model.SetValue(iter,18,(string) lector["contacto1_proveedor"]);	
							lista_productos_a_comprar.Model.SetValue(iter,19,(string) lector["mail_proveedor"]);	
							lista_productos_a_comprar.Model.SetValue(iter,20,(string) lector["rfc_proveedor"]);	
							lista_productos_a_comprar.Model.SetValue(iter,21,(string) lector["fax_proveedor"]);
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
								lista_productos_a_comprar.Model.SetValue(iter,10,(string) lector["preciocosto"]);					// precio prov
								lista_productos_a_comprar.Model.SetValue(iter,11,(string) lector["preciocostounitario"]);			// precio unitario
								lista_productos_a_comprar.Model.SetValue(iter,13,(string) lector["codigo_producto_proveedor"]);	// Codigo
								lista_productos_a_comprar.Model.SetValue(iter,14,(string) lector["codigo_de_barra"]);				// Barras
								lista_productos_a_comprar.Model.SetValue(iter,12,(string) entry_nombre_proveedor.Text);		// actualiza treeview con el nombre del proveedor
								lista_productos_a_comprar.Model.SetValue(iter,15,(string) entry_id_proveedor.Text);  			// almacena el id del proveedor							
								lista_productos_a_comprar.Model.SetValue(iter,16,(string) lector["direccion_proveedor"]);					
								lista_productos_a_comprar.Model.SetValue(iter,17,(string) lector["telefono1_proveedor"]);		
								lista_productos_a_comprar.Model.SetValue(iter,18,(string) lector["contacto1_proveedor"]);	
								lista_productos_a_comprar.Model.SetValue(iter,19,(string) lector["mail_proveedor"]);	
								lista_productos_a_comprar.Model.SetValue(iter,20,(string) lector["rfc_proveedor"]);	
								lista_productos_a_comprar.Model.SetValue(iter,21,(string) lector["fax_proveedor"]);	
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
			if ( (int) Convert.ToSingle ((string) this.lista_productos_a_comprar.Model.GetValue (iter,15)) > 1){ 
				(cell as Gtk.CellRendererText).Foreground = "blue";
			}else{ 
				(cell as Gtk.CellRendererText).Foreground = "black";
			}
		}
		
		void llena_requiciones_para_comprar(string departamentos_seleccionados)
		{
			treeViewEngineProductosaComprar.Clear();
			// lleno de la tabla de his_tipo_de_admisiones
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT to_char(osiris_erp_requisicion_deta.id_secuencia,'9999999999') AS idsecuencia,id_requisicion,to_char(osiris_erp_requisicion_deta.id_producto,'999999999999') AS idproducto,"+
							"to_char(cantidad_solicitada,'999999.99') AS cantidadsolicitada,comprado,"+
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
							departamentos_seleccionados+"') "+
							"AND autorizada = 'true' "+
							"AND eliminado = 'false' "+
						    "AND comprado = 'false' "+
							"ORDER BY id_requisicion DESC;";
				Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read()){
					this.treeViewEngineProductosaComprar.AppendValues(false,
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
					                        (string) lector["fechahoraautorizado"]);
					//this.col_autorizar.SetCellDataFunc(cel_autorizar, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_solicitado_por.SetCellDataFunc(cellr1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_numero_req.SetCellDataFunc(cellr2, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_cantidadcomprar.SetCellDataFunc(cellr3, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_descripcion.SetCellDataFunc(cellr4, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_unidades.SetCellDataFunc(cellr5, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_codigo_prod.SetCellDataFunc(cellr6, new Gtk.TreeCellDataFunc(cambia_colores_fila));
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
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
													typeof(string),	// 15 id proveedor
													typeof(string),	// 16 secuencia	
													typeof(string),	// fecha requisado
													typeof(string),	// fecha autorizado
													typeof(string),
													typeof(string),
													typeof(string));							
												
			lista_productos_a_comprar.Model = treeViewEngineProductosaComprar;			
			lista_productos_a_comprar.RulesHint = true;
						
			col_autorizar = new TreeViewColumn();
			cel_autorizar = new CellRendererToggle();
			col_autorizar.Title = "Seleccion";
			col_autorizar.PackStart(cel_autorizar, true);
			col_autorizar.AddAttribute (cel_autorizar, "active", 0);
			cel_autorizar.Activatable = true;
			cel_autorizar.Toggled += selecciona_fila;
			col_autorizar.SortColumnId = (int) col_ordencompra.col_autorizar;
			
			col_solicitado_por = new TreeViewColumn();
			cellr1 = new CellRendererText();
			col_solicitado_por.Title = "Solicitado Por";
			col_solicitado_por.PackStart(cellr1, true);
			col_solicitado_por.AddAttribute (cellr1, "text", 1);
			col_solicitado_por.SortColumnId = (int) col_ordencompra.col_solicitado_por;
												
			col_numero_req = new TreeViewColumn();
			cellr2 = new CellRendererText();
			col_numero_req.Title = "Nº Requi.";
			col_numero_req.PackStart(cellr2, true);
			col_numero_req.AddAttribute (cellr2, "text", 2);
			col_numero_req.SortColumnId = (int) col_ordencompra.col_numero_req;
			
			col_cantidadcomprar = new TreeViewColumn();
			cellr3 = new CellRendererText();
			col_cantidadcomprar.Title = "Cantidad";
			col_cantidadcomprar.PackStart(cellr3, true);
			col_cantidadcomprar.AddAttribute (cellr3, "text", 3);
			col_cantidadcomprar.SortColumnId = (int) col_ordencompra.col_cantidadcomprar;
			cellr3.Editable = true;
			cellr3.Edited += NumberCellEdited_Autorizado_1;
			
			col_descripcion = new TreeViewColumn();
			cellr4 = new CellRendererText();
			col_descripcion.Title = "Descripcion producto";
			col_descripcion.PackStart(cellr4, true);
			col_descripcion.AddAttribute (cellr4, "text", 4);
			col_descripcion.SortColumnId = (int) col_ordencompra.col_descripcion;
			col_descripcion.Resizable = true;
			cellr4.Width = 350;
			
			col_unidades = new TreeViewColumn();
			cellr5 = new CellRendererText();
			col_unidades.Title = "Unidad Prod.";
			col_unidades.PackStart(cellr5, true);
			col_unidades.AddAttribute (cellr5, "text", 5);
			col_unidades.SortColumnId = (int) col_ordencompra.col_unidades;
						
			col_codigo_prod = new TreeViewColumn();
			cellr6 = new CellRendererText();
			col_codigo_prod.Title = "Codigo Producto";
			col_codigo_prod.PackStart(cellr6, true);
			col_codigo_prod.AddAttribute (cellr6, "text", 6);
			col_codigo_prod.SortColumnId = (int) col_ordencompra.col_codigo_prod;
						
			TreeViewColumn col_precio_unit_hsc = new TreeViewColumn();
			CellRendererText cellr7 = new CellRendererText();
			col_precio_unit_hsc.Title = "Precio Unit.";
			col_precio_unit_hsc.PackStart(cellr7, true);
			col_precio_unit_hsc.AddAttribute (cellr7, "text", 7);
			col_precio_unit_hsc.SortColumnId = (int) col_ordencompra.col_precio_unit_hsc;
			
			TreeViewColumn col_precio_prod_hsc = new TreeViewColumn();
			CellRendererText cellr8 = new CellRendererText();
			col_precio_prod_hsc.Title = "Precio Produ.";
			col_precio_prod_hsc.PackStart(cellr8, true);
			col_precio_prod_hsc.AddAttribute (cellr8, "text", 8);
			col_precio_prod_hsc.SortColumnId = (int) col_ordencompra.col_precio_prod_hsc;
			cellr8.Editable = true;
			
			TreeViewColumn col_embalaje = new TreeViewColumn();
			CellRendererText cellr9 = new CellRendererText();
			col_embalaje.Title = "Embalaje";
			col_embalaje.PackStart(cellr9, true);
			col_embalaje.AddAttribute (cellr9, "text", 9);
			col_embalaje.SortColumnId = (int) col_ordencompra.col_embalaje;
			cellr9.Editable = true;
			cellr9.Edited += NumberCellEdited_Autorizado_2;
			
			TreeViewColumn col_precioprove = new TreeViewColumn();
			CellRendererText cell10 = new CellRendererText();
			col_precioprove.Title = "Precio Prov.";
			col_precioprove.PackStart(cell10, true);
			col_precioprove.AddAttribute (cell10, "text", 10);
			col_precioprove.SortColumnId = (int) col_ordencompra.col_precioprove;
			
			TreeViewColumn col_preciouniprov = new TreeViewColumn();
			CellRendererText cellr11 = new CellRendererText();
			col_preciouniprov.Title = "Precio Unit.Prov.";
			col_preciouniprov.PackStart(cellr11, true);
			col_preciouniprov.AddAttribute (cellr11, "text", 11);
			col_preciouniprov.SortColumnId = (int) col_ordencompra.col_preciouniprov;
									
			TreeViewColumn col_descrprove = new TreeViewColumn();
			CellRendererText cellr12 = new CellRendererText();
			col_descrprove.Title = "Nombre Proveedor";
			col_descrprove.PackStart(cellr12, true);
			col_descrprove.AddAttribute (cellr12, "text", 12);
			col_descrprove.SortColumnId = (int) col_ordencompra.col_descrprove;
			
			TreeViewColumn col_codprodprov = new TreeViewColumn();
			CellRendererText cellr13 = new CellRendererText();
			col_codprodprov.Title = "Cod.Prod.Prove.";
			col_codprodprov.PackStart(cellr13, true);
			col_codprodprov.AddAttribute (cellr13, "text", 13);
			col_codprodprov.SortColumnId = (int) col_ordencompra.col_codprodprov;
			
			TreeViewColumn col_codigbarras = new TreeViewColumn();
			CellRendererText cellr14 = new CellRendererText();
			col_codigbarras.Title = "Cod. Barras";
			col_codigbarras.PackStart(cellr14, true);
			col_codigbarras.AddAttribute (cellr14, "text", 14);
			col_codigbarras.SortColumnId = (int) col_ordencompra.col_codigbarras;
			
			TreeViewColumn col_fecharequisado = new TreeViewColumn();
			CellRendererText cellr17 = new CellRendererText();
			col_fecharequisado.Title = "Fech/Requi.";
			col_fecharequisado.PackStart(cellr17, true);
			col_fecharequisado.AddAttribute (cellr17, "text", 17);
			col_fecharequisado.SortColumnId = (int) col_ordencompra.col_fecharequisado;
			
			TreeViewColumn col_fechaautorizado = new TreeViewColumn();
			CellRendererText cellr18 = new CellRendererText();
			col_fechaautorizado.Title = "Fech/Autori.";
			col_fechaautorizado.PackStart(cellr18, true);
			col_fechaautorizado.AddAttribute (cellr18, "text", 18);
			col_fechaautorizado.SortColumnId = (int) col_ordencompra.col_fechaautorizado;
			
			lista_productos_a_comprar.AppendColumn(col_autorizar);				// 0
			lista_productos_a_comprar.AppendColumn(col_solicitado_por);			// 1
			lista_productos_a_comprar.AppendColumn(col_numero_req);				// 2
			lista_productos_a_comprar.AppendColumn(col_fecharequisado);
			lista_productos_a_comprar.AppendColumn(col_fechaautorizado);
			lista_productos_a_comprar.AppendColumn(col_cantidadcomprar);		// 3
			lista_productos_a_comprar.AppendColumn(col_embalaje);
			lista_productos_a_comprar.AppendColumn(col_descripcion);			// 4
			lista_productos_a_comprar.AppendColumn(col_unidades);				// 5
			lista_productos_a_comprar.AppendColumn(col_codigo_prod);			// 6
			lista_productos_a_comprar.AppendColumn(col_precio_unit_hsc);		// 7
			lista_productos_a_comprar.AppendColumn(col_precio_prod_hsc);		// 9			
			lista_productos_a_comprar.AppendColumn(col_precioprove);			// 10
			lista_productos_a_comprar.AppendColumn(col_preciouniprov);			// 11			
			lista_productos_a_comprar.AppendColumn(col_descrprove);				// 12
			lista_productos_a_comprar.AppendColumn(col_codprodprov);			// 13
			lista_productos_a_comprar.AppendColumn(col_codigbarras);			// 14
		}
		
		enum col_ordencompra
		{
			col_autorizar,	
			col_solicitado_por,
			col_numero_req,
			col_cantidadcomprar,
			col_descripcion,
			col_unidades,
			col_codigo_prod,
			col_precio_unit_hsc,
			col_precio_prod_hsc,
			col_embalaje,
			col_precioprove,
			col_preciouniprov,
			col_descrprove,
			col_codprodprov,
			col_codigbarras,col_fecharequisado,col_fechaautorizado
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
				treeViewEngineProductosaComprar.SetValue(iter,(int) col_ordencompra.col_cantidadcomprar,args.NewText);
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
				treeViewEngineProductosaComprar.SetValue(iter,(int) col_ordencompra.col_embalaje,args.NewText);
			}
 		}
		
		void on_button_busca_proveedores_clicked(object sender, EventArgs args)
		{
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion
			// la clase recibe tambien el orden del query
			// es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo
			object[] parametros_objetos = {entry_id_proveedor,entry_nombre_proveedor,entry_formapago};
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
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_proveedores"," ORDER BY descripcion_proveedor;","%' ",0);
		}
			
		// Cuando seleccion campos para la autorizacion de compras  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (lista_productos_a_comprar.Model.GetIter (out iter, path)){					
				bool old = (bool) this.lista_productos_a_comprar.Model.GetValue(iter,0);
				lista_productos_a_comprar.Model.SetValue(iter,0,!old);
			}				
		}
		
		void selecciona_departamento(object sender, ToggledArgs args)
		{
			int variable_paso_02_1 = 0;
			departamentos_seleccionados = "AND osiris_erp_requisicion_deta.id_tipo_admisiones IN('";
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