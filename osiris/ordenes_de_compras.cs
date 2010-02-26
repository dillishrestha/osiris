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

namespace osiris
{
	public class crea_ordenes_de_compra
	{
	
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		//BOTON IMPRIMIR
        [Widget] Gtk.Button button_imprimir;
		
		// Declarando ventana del menu de costos
		[Widget] Gtk.Window crea_ordenes_compras;
		[Widget] Gtk.ComboBox combobox_tipo_admision;
		[Widget] Gtk.TreeView lista_productos_a_comprar;
		[Widget] Gtk.Button button_busca_proveedores;
		[Widget] Gtk.Button button_asignar_proveedor;
		[Widget] Gtk.Button button_orden_compra;
		[Widget] Gtk.Entry entry_id_proveedor;
		[Widget] Gtk.Entry entry_nombre_proveedor;
		[Widget] Gtk.Entry entry_formapago;	
		[Widget] Gtk.Entry entry_dia;
		[Widget] Gtk.Entry entry_mes;
		[Widget] Gtk.Entry entry_ano;
		[Widget] Gtk.Statusbar statusbar;
				
		string connectionString;
		string nombrebd;
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
    	
    	int ultimaorden = 0;
    	
    	// Declarando las variables de publicas para uso dentro de classe
    	int idtipointernamiento;
    	string descripinternamiento = "";	// Descripcion de Centro de Costos - Solicitado por
		
		TreeStore treeViewEngineProductosaComprar;	// Lista de proctos que se van a comprar
				
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
			this.entry_dia.Text = DateTime.Now.ToString("dd");
			this.entry_mes.Text = DateTime.Now.ToString("MM");
			this.entry_ano.Text = DateTime.Now.ToString("yyyy");
			
			button_orden_compra.Clicked += new EventHandler(on_button_orden_compra_clicked);
			// 
			button_asignar_proveedor.Clicked += new EventHandler(on_button_asignar_proveedor_clicked);
			// Busca proveedores
			button_busca_proveedores.Clicked += new EventHandler(on_button_busca_proveedores_clicked);
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			//imprime la informacion:
			this.button_imprimir.Clicked += new EventHandler(on_imprime_orden_clicked);
			
			llenado_comobox();
			crea_treeview_ordencompra();
			
			statusbar.Pop(0);
			statusbar.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar.HasResizeGrip = false;
		}
		
		void on_button_orden_compra_clicked(object sender, EventArgs args)
		{
			ultimaorden = int.Parse(classpublic.lee_ultimonumero_registrado("osiris_erp_ordenes_compras_enca","numero_orden_compra",""));
			bool variable_paso_01 = true;
			string variable_paso_02 = "";   // se usa para asignar el proveedor de que se utilizo en la requisicion
			TreeIter iterSelected;
			TreeModel model;
			TreeIter iter;
			//this.treeViewEngineProductosaComprar.GetSortColumnId(out iterSelected,			
			if (this.treeViewEngineProductosaComprar.GetIterFirst(out iterSelected)){
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
	 												"numero_orden_compra) "+
	 												"VALUES ('"+
	 												int.Parse((string) this.lista_productos_a_comprar.Model.GetValue(iterSelected,15))+"','"+//id_prod
			 										DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+//fechahora_creacion
			 										DateTime.Now.ToString("yyyy-MM-dd")+"','"+//fechahora_solicitud
			 										this.entry_ano.Text+"-"+this.entry_mes.Text+"-"+this.entry_dia.Text+"','"+
			 										LoginEmpleado+"','"+//id_empleado
			 										"HOSPITAL"+"','"+
			 										"SU CONDUCTO"+"','"+
			 										this.entry_formapago.Text+"','"+
	 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,12)+"','"+
	 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,2).ToString().Trim()+"','"+
	 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,16)+"','"+
	 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,17)+"','"+
	 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,18)+"','"+
	 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,19)+"','"+
	 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,20)+"','"+
	 												(string) lista_productos_a_comprar.Model.GetValue(iterSelected,21)+"','"+
	 													 												
	 												this.ultimaorden.ToString()+

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
											    "numero_orden_compra = '"+this.ultimaorden.ToString()+"' "+
							                    "WHERE id_requisicion ='"+(string) this.lista_productos_a_comprar.Model.GetValue (iterSelected,2)+"' "+
							                    "AND id_producto ='"+(string) this.lista_productos_a_comprar.Model.GetValue (iterSelected,6)+"' ;"; 
							    
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
	 												"numero_orden_compra) "+
	 												"VALUES ('"+
	 												int.Parse((string) this.lista_productos_a_comprar.Model.GetValue(iterSelected,15))+"','"+//id_prod
			 										DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+//fechahora_creacion
			 										DateTime.Now.ToString("yyyy-MM-dd")+"','"+//fechahora_solicitud
			 										this.entry_ano.Text+"-"+this.entry_mes.Text+"-"+this.entry_dia.Text+"','"+
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
	 													 												
	 												this.ultimaorden.ToString()+

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
											    "numero_orden_compra = '"+this.ultimaorden.ToString()+"' "+
												//"cantidad_comprada = '"+
							                    "WHERE id_requisicion ='"+(string) this.lista_productos_a_comprar.Model.GetValue (iterSelected,2)+"' "+
							                    "AND id_producto ='"+(string) this.lista_productos_a_comprar.Model.GetValue (iterSelected,6)+"' ;"; 
							    
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
							                                               MessageType.Info,ButtonsType.Close, " Las ORDEN DE COMPRA SE CREO CORRECTAMENTE");
			msgBoxError1.Run ();			msgBoxError1.Destroy();
			
			llena_requiciones_para_comprar();
		}
		
		void on_imprime_orden_clicked(object sender, EventArgs args)
		{
			//new osiris.rpt_orden_compras(this.nombrebd,this.lista_productos_a_comprar,this.treeViewEngineProductosaComprar);
		}	
		
		void onComboBoxChanged_tipo_admision (object sender, EventArgs args)
		{
    		ComboBox combobox_tipo_admision = sender as ComboBox;
			if (sender == null){return;}
	  		TreeIter iter;
	  		if (combobox_tipo_admision.GetActiveIter (out iter)){
		    	idtipointernamiento = (int) combobox_tipo_admision.Model.GetValue(iter,1);
		    	descripinternamiento = (string) combobox_tipo_admision.Model.GetValue(iter,0);
		    	llena_requiciones_para_comprar();
	     	}
		}
		
		void on_button_asignar_proveedor_clicked (object sender, EventArgs args)
		{
			TreeIter iter;
			if (this.treeViewEngineProductosaComprar.GetIterFirst (out iter)){
				// buscar el producto en el catalogo del proveedor
				int contador_prod_asignados = 0;
				int contador_prod_noasignad = 0;
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
               								"FROM osiris_catalogo_productos_proveedores,osiris_erp_proveedores "+
               								"WHERE osiris_catalogo_productos_proveedores.id_producto = '"+(string) this.lista_productos_a_comprar.Model.GetValue (iter,6)+"' "+
               								"AND osiris_catalogo_productos_proveedores.id_proveedor = '"+(string) this.entry_id_proveedor.Text+"' "+
               								"AND osiris_catalogo_productos_proveedores.id_proveedor = osiris_erp_proveedores.id_proveedor;";
               			//Console.WriteLine(comando.CommandText);
						NpgsqlDataReader lector = comando.ExecuteReader ();
						
               			if (lector.Read()){
							this.lista_productos_a_comprar.Model.SetValue(iter,0,true);
							
							this.lista_productos_a_comprar.Model.SetValue(iter,10,(string) lector["preciocosto"]);					// precio prov
							this.lista_productos_a_comprar.Model.SetValue(iter,11,(string) lector["preciocostounitario"]);		// precio unitario
							this.lista_productos_a_comprar.Model.SetValue(iter,13,(string) lector["codigo_producto_proveedor"]);	// Codigo
							this.lista_productos_a_comprar.Model.SetValue(iter,14,(string) lector["codigo_de_barra"]);				// Barras						
							this.lista_productos_a_comprar.Model.SetValue(iter,12,(string) this.entry_nombre_proveedor.Text);		// Nombre Proveedor
							this.lista_productos_a_comprar.Model.SetValue(iter,15,(string) this.entry_id_proveedor.Text);			// Id Proveedor
							
							this.lista_productos_a_comprar.Model.SetValue(iter,16,(string) lector["direccion_proveedor"]);					
							this.lista_productos_a_comprar.Model.SetValue(iter,17,(string) lector["telefono1_proveedor"]);		
							this.lista_productos_a_comprar.Model.SetValue(iter,18,(string) lector["contacto1_proveedor"]);	
							this.lista_productos_a_comprar.Model.SetValue(iter,19,(string) lector["mail_proveedor"]);	
							this.lista_productos_a_comprar.Model.SetValue(iter,20,(string) lector["rfc_proveedor"]);	
							this.lista_productos_a_comprar.Model.SetValue(iter,21,(string) lector["fax_proveedor"]);
							contador_prod_asignados += 1;
						}else{
							contador_prod_noasignad += 1;
						}
					}
						
					while (this.treeViewEngineProductosaComprar.IterNext(ref iter)){
						if ((bool) this.lista_productos_a_comprar.Model.GetValue (iter,0) == true){
				
							// buscar el producto en el catalogo del proveedor
							comando.CommandText = "SELECT osiris_catalogo_productos_proveedores.id_proveedor,"+
               								"to_char(osiris_catalogo_productos_proveedores.precio_costo,'9999999.99') AS preciocosto,"+ 
               								"to_char(osiris_catalogo_productos_proveedores.precio_costo_unitario,'9999999.99') AS preciocostounitario,"+ 
               								"osiris_catalogo_productos_proveedores.id_producto,osiris_catalogo_productos_proveedores.codigo_producto_proveedor,"+
               								"osiris_catalogo_productos_proveedores.codigo_de_barra,clave,osiris_catalogo_productos_proveedores.marca_producto,"+
               								"osiris_erp_proveedores.direccion_proveedor,osiris_erp_proveedores.telefono1_proveedor,"+
               								"osiris_erp_proveedores.contacto1_proveedor,osiris_erp_proveedores.mail_proveedor,"+
               								"osiris_erp_proveedores.rfc_proveedor,osiris_erp_proveedores.fax_proveedor "+
               								"FROM osiris_catalogo_productos_proveedores,osiris_erp_proveedores "+
               								"WHERE osiris_catalogo_productos_proveedores.id_producto = '"+(string) this.lista_productos_a_comprar.Model.GetValue (iter,6)+"' "+
               								"AND osiris_catalogo_productos_proveedores.id_proveedor = '"+(string) this.entry_id_proveedor.Text+"' "+
               								"AND osiris_catalogo_productos_proveedores.id_proveedor = osiris_erp_proveedores.id_proveedor;";
							//Console.WriteLine(comando.CommandText);
							NpgsqlDataReader lector = comando.ExecuteReader ();						
               				if (lector.Read()){					
								this.lista_productos_a_comprar.Model.SetValue(iter,0,true);
								this.lista_productos_a_comprar.Model.SetValue(iter,10,(string) lector["preciocosto"]);					// precio prov
								this.lista_productos_a_comprar.Model.SetValue(iter,11,(string) lector["preciocostounitario"]);		// precio unitario
								this.lista_productos_a_comprar.Model.SetValue(iter,13,(string) lector["codigo_producto_proveedor"]);	// Codigo
								this.lista_productos_a_comprar.Model.SetValue(iter,14,(string) lector["codigo_de_barra"]);				// Barras
								this.lista_productos_a_comprar.Model.SetValue(iter,12,(string) this.entry_nombre_proveedor.Text);		// actualiza treeview con el nombre del proveedor
								this.lista_productos_a_comprar.Model.SetValue(iter,15,(string) this.entry_id_proveedor.Text);  			// almacena el id del proveedor
							
								this.lista_productos_a_comprar.Model.SetValue(iter,16,(string) lector["direccion_proveedor"]);					
								this.lista_productos_a_comprar.Model.SetValue(iter,17,(string) lector["telefono1_proveedor"]);		
								this.lista_productos_a_comprar.Model.SetValue(iter,18,(string) lector["contacto1_proveedor"]);	
								this.lista_productos_a_comprar.Model.SetValue(iter,19,(string) lector["mail_proveedor"]);	
								this.lista_productos_a_comprar.Model.SetValue(iter,20,(string) lector["rfc_proveedor"]);	
								this.lista_productos_a_comprar.Model.SetValue(iter,21,(string) lector["fax_proveedor"]);	
								contador_prod_asignados += 1;
							}else{
								contador_prod_noasignad += 1;
							}				
						}
					}
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
		
		void llena_requiciones_para_comprar()
		{
			this.treeViewEngineProductosaComprar.Clear();
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
							"osiris_productos.tipo_unidad_producto,to_char(numero_orden_compra,'9999999999') AS numeroordencompra,"+
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
							"AND osiris_erp_requisicion_deta.id_tipo_admisiones = '"+this.idtipointernamiento.ToString().Trim()+"' "+
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
											(string) lector["idsecuencia"]);
					//this.col_autorizar.SetCellDataFunc(cel_autorizar, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					this.col_solicitado_por.SetCellDataFunc(cellr1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					this.col_numero_req.SetCellDataFunc(cellr2, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					this.col_cantidadcomprar.SetCellDataFunc(cellr3, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					this.col_descripcion.SetCellDataFunc(cellr4, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					this.col_unidades.SetCellDataFunc(cellr5, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					this.col_codigo_prod.SetCellDataFunc(cellr6, new Gtk.TreeCellDataFunc(cambia_colores_fila));
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void llenado_comobox()
		{
			// Declarando Combobox
			combobox_tipo_admision.Clear();
			CellRendererText cell1 = new CellRendererText();
			combobox_tipo_admision.PackStart(cell1, true);
			combobox_tipo_admision.AddAttribute(cell1,"text",0);
			ListStore store1 = new ListStore( typeof (string), typeof (int));
			combobox_tipo_admision.Model = store1;
			
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
					store1.AppendValues ((string) lector["descripcion_admisiones"], (int) lector["id_tipo_admisiones"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
			
			TreeIter iter1;
			if (store1.GetIterFirst(out iter1)){
				//Console.WriteLine(iter2);
				combobox_tipo_admision.SetActiveIter (iter1);
			}
			combobox_tipo_admision.Changed += new EventHandler (onComboBoxChanged_tipo_admision);
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
													typeof(string),
													typeof(string),			// id_proveedor
													typeof(string));		// id_secuencia							
												
			lista_productos_a_comprar.Model = treeViewEngineProductosaComprar;			
			lista_productos_a_comprar.RulesHint = true;
						
			col_autorizar = new TreeViewColumn();
			cel_autorizar = new CellRendererToggle();
			col_autorizar.Title = "Seleccion"; // titulo de la cabecera de la columna, si está visible
			col_autorizar.PackStart(cel_autorizar, true);
			col_autorizar.AddAttribute (cel_autorizar, "active", 0);
			cel_autorizar.Activatable = true;
			cel_autorizar.Toggled += selecciona_fila;
			col_autorizar.SortColumnId = (int) col_ordencompra.col_autorizar;
			
			col_solicitado_por = new TreeViewColumn();
			cellr1 = new CellRendererText();
			col_solicitado_por.Title = "Solicitado Por"; // titulo de la cabecera de la columna, si está visible
			col_solicitado_por.PackStart(cellr1, true);
			col_solicitado_por.AddAttribute (cellr1, "text", 1);
			col_solicitado_por.SortColumnId = (int) col_ordencompra.col_solicitado_por;
												
			col_numero_req = new TreeViewColumn();
			cellr2 = new CellRendererText();
			col_numero_req.Title = "Nº Requi."; // titulo de la cabecera de la columna, si está visible
			col_numero_req.PackStart(cellr2, true);
			col_numero_req.AddAttribute (cellr2, "text", 2);
			col_numero_req.SortColumnId = (int) col_ordencompra.col_numero_req;
			
			col_cantidadcomprar = new TreeViewColumn();
			cellr3 = new CellRendererText();
			col_cantidadcomprar.Title = "Cantidad"; // titulo de la cabecera de la columna, si está visible
			col_cantidadcomprar.PackStart(cellr3, true);
			col_cantidadcomprar.AddAttribute (cellr3, "text", 3);
			col_cantidadcomprar.SortColumnId = (int) col_ordencompra.col_cantidadcomprar;
			
			col_descripcion = new TreeViewColumn();
			cellr4 = new CellRendererText();
			col_descripcion.Title = "Descripcion producto"; // titulo de la cabecera de la columna, si está visible
			col_descripcion.PackStart(cellr4, true);
			col_descripcion.AddAttribute (cellr4, "text", 4);
			col_descripcion.SortColumnId = (int) col_ordencompra.col_descripcion;
			col_descripcion.Resizable = true;
			cellr4.Width = 350;
			
			col_unidades = new TreeViewColumn();
			cellr5 = new CellRendererText();
			col_unidades.Title = "Unidad Prod."; // titulo de la cabecera de la columna, si está visible
			col_unidades.PackStart(cellr5, true);
			col_unidades.AddAttribute (cellr5, "text", 5);
			col_unidades.SortColumnId = (int) col_ordencompra.col_unidades;
						
			col_codigo_prod = new TreeViewColumn();
			cellr6 = new CellRendererText();
			col_codigo_prod.Title = "Codigo Producto"; // titulo de la cabecera de la columna, si está visible
			col_codigo_prod.PackStart(cellr6, true);
			col_codigo_prod.AddAttribute (cellr6, "text", 6);
			col_codigo_prod.SortColumnId = (int) col_ordencompra.col_codigo_prod;
						
			TreeViewColumn col_precio_unit_hsc = new TreeViewColumn();
			CellRendererText cellr7 = new CellRendererText();
			col_precio_unit_hsc.Title = "Precio Unit."; // titulo de la cabecera de la columna, si está visible
			col_precio_unit_hsc.PackStart(cellr7, true);
			col_precio_unit_hsc.AddAttribute (cellr7, "text", 7);
			col_precio_unit_hsc.SortColumnId = (int) col_ordencompra.col_precio_unit_hsc;
			
			TreeViewColumn col_precio_prod_hsc = new TreeViewColumn();
			CellRendererText cellr8 = new CellRendererText();
			col_precio_prod_hsc.Title = "Precio Produ."; // titulo de la cabecera de la columna, si está visible
			col_precio_prod_hsc.PackStart(cellr8, true);
			col_precio_prod_hsc.AddAttribute (cellr8, "text", 8);
			col_precio_prod_hsc.SortColumnId = (int) col_ordencompra.col_precio_prod_hsc;
			
			TreeViewColumn col_embalaje = new TreeViewColumn();
			CellRendererText cellr9 = new CellRendererText();
			col_embalaje.Title = "Embalaje"; // titulo de la cabecera de la columna, si está visible
			col_embalaje.PackStart(cellr9, true);
			col_embalaje.AddAttribute (cellr9, "text", 9);
			col_embalaje.SortColumnId = (int) col_ordencompra.col_embalaje;
			
			TreeViewColumn col_precioprove = new TreeViewColumn();
			CellRendererText cell10 = new CellRendererText();
			col_precioprove.Title = "Precio Prov."; // titulo de la cabecera de la columna, si está visible
			col_precioprove.PackStart(cell10, true);
			col_precioprove.AddAttribute (cell10, "text", 10);
			col_precioprove.SortColumnId = (int) col_ordencompra.col_precioprove;
			
			TreeViewColumn col_preciouniprov = new TreeViewColumn();
			CellRendererText cellr11 = new CellRendererText();
			col_preciouniprov.Title = "Precio Unit.Prov."; // titulo de la cabecera de la columna, si está visible
			col_preciouniprov.PackStart(cellr11, true);
			col_preciouniprov.AddAttribute (cellr11, "text", 11);
			col_preciouniprov.SortColumnId = (int) col_ordencompra.col_preciouniprov;
									
			TreeViewColumn col_descrprove = new TreeViewColumn();
			CellRendererText cellr12 = new CellRendererText();
			col_descrprove.Title = "Nombre Proveedor"; // titulo de la cabecera de la columna, si está visible
			col_descrprove.PackStart(cellr12, true);
			col_descrprove.AddAttribute (cellr12, "text", 12);
			col_descrprove.SortColumnId = (int) col_ordencompra.col_descrprove;
			
			TreeViewColumn col_codprodprov = new TreeViewColumn();
			CellRendererText cellr13 = new CellRendererText();
			col_codprodprov.Title = "Cod.Prod.Prove."; // titulo de la cabecera de la columna, si está visible
			col_codprodprov.PackStart(cellr13, true);
			col_codprodprov.AddAttribute (cellr13, "text", 13);
			col_codprodprov.SortColumnId = (int) col_ordencompra.col_codprodprov;
			
			TreeViewColumn col_codigbarras = new TreeViewColumn();
			CellRendererText cellr14 = new CellRendererText();
			col_codigbarras.Title = "Cod. Barras"; // titulo de la cabecera de la columna, si está visible
			col_codigbarras.PackStart(cellr14, true);
			col_codigbarras.AddAttribute (cellr14, "text", 14);
			col_codigbarras.SortColumnId = (int) col_ordencompra.col_codigbarras;
		
						
			lista_productos_a_comprar.AppendColumn(col_autorizar);				// 0
			lista_productos_a_comprar.AppendColumn(col_solicitado_por);			// 1
			lista_productos_a_comprar.AppendColumn(col_numero_req);				// 2
			lista_productos_a_comprar.AppendColumn(col_cantidadcomprar);		// 3
			lista_productos_a_comprar.AppendColumn(col_descripcion);			// 4
			lista_productos_a_comprar.AppendColumn(col_unidades);				// 5
			lista_productos_a_comprar.AppendColumn(col_codigo_prod);			// 6
			lista_productos_a_comprar.AppendColumn(col_precio_unit_hsc);		// 7
			lista_productos_a_comprar.AppendColumn(col_precio_prod_hsc);		// 8
			lista_productos_a_comprar.AppendColumn(col_embalaje);				// 9			
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
			col_codigbarras
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
			classfind_data.buscandor(parametros_objetos,parametros_sql,"find_proveedores"," ORDER BY descripcion_proveedor;","%' ");
		}
			
		// Cuando seleccion campos para la autorizacion de compras  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (this.lista_productos_a_comprar.Model.GetIter (out iter, path)){					
				bool old = (bool) this.lista_productos_a_comprar.Model.GetValue(iter,0);
				this.lista_productos_a_comprar.Model.SetValue(iter,0,!old);
			}				
		}
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
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