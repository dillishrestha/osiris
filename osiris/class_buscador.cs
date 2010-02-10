/// <summary>
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
// Autor    	: Daniel Olivares Cuevas - arcangeldoc@gmail.com (Programacion Mono)
//		  		  Daniel Olivares Cuevas - arcangeldoc@gmail.com (Diseño de Pantallas Glade)
// 				  
// Licencia		: GPL
// S.O. 		: GNU/Linux
//
// proyect Facturador is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// proyect is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Foobar; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Coment
//
// This module is copy the Project Sistema Hospitalario OSIRIS
//
/// </summary>

using Gtk;
using Gdk;
using System;
using Glade;
using Npgsql;

namespace osiris
{		
	public class class_buscador
	{		
		//Declarando ventanas de busqueda
		[Widget] Gtk.Window buscador = null;
		[Widget] Gtk.RadioButton radiobutton1  = null;
		[Widget] Gtk.RadioButton radiobutton2  = null;
		[Widget] Gtk.RadioButton radiobutton3  = null;
		[Widget] Gtk.Entry entry_expresion = null;
		[Widget] Gtk.Button button_buscar_busqueda = null;
		[Widget] Gtk.Button button_selecciona = null;
		[Widget] Gtk.TreeView lista_de_busqueda = null;
		[Widget] Gtk.Button button_salir = null;
		[Widget] Gtk.ComboBox combobox_busqueda = null;
		[Widget] Gtk.Label labelbusqueda1 = null;
		[Widget] Gtk.Label labelbusqueda2 = null;
		[Widget] Gtk.Label labelcantidad = null;
		[Widget] Gtk.Entry entry_cantidad_producto = null;
								
		TreeStore treeViewEngineBuscador;	// Busqueda de Clientes
		
		// Busqueda de Clientes para los reportes
		Gtk.Entry entry_nombre_cliente = null;
		Gtk.Entry entry_id_cliente = null;
		
		// Busqueda de Estados y Regiones para los catalogos
		Gtk.Entry entry_id_estado = null;
		Gtk.Entry entry_estado = null;
		Gtk.ToggleButton togglebutton_editar_estado = null;
		
		Gtk.Entry entry_id_municipio = null; 
		Gtk.Entry entry_municipio = null;
		Gtk.Frame frame2 = null;		
		Gtk.ToggleButton togglebutton_editar_municipio = null;
		Gtk.Button button_guardar_municipio = null; 
		
		// Busqueda de Grupo de Grupo de Productos 0
		Gtk.Entry entry_id_grupo = null;
		Gtk.Entry entry_descripcion_grupo = null;
		Gtk.ToggleButton togglebutton_editar_grupo = null;
		Gtk.Button button_guardar_grupo = null;
		Gtk.CheckButton checkbutton_activar_grupo = null;
		Gtk.Entry entry_porcentage_utilidad = null;
		Gtk.Entry entry_id_centrocosto = null;
		Gtk.Entry entry_descripcion_centrocosto = null;
		Gtk.Button button_buscar_centrocosto = null;
		
		// Busqueda de Grupo1 o Familia1 de Productos
		Gtk.Entry entry_id_grupo1 = null;
		Gtk.Entry entry_descripcion_grupo1 = null;
		Gtk.ToggleButton togglebutton_editar_grupo1 = null;
		Gtk.Button button_guardar_grupo1 = null;
		Gtk.Button button_buscar_grupo1 = null;
		Gtk.CheckButton checkbutton_activar_grupo1 = null;
		
		// Busqueda de Grupo2 o Familia1 de Productos
		Gtk.Entry entry_id_grupo2 = null;
		Gtk.Entry entry_descripcion_grupo2 = null;
		Gtk.ToggleButton togglebutton_editar_grupo2 = null;
		Gtk.Button button_guardar_grupo2 = null;
		Gtk.Button button_buscar_grupo2 = null;
		Gtk.CheckButton checkbutton_activar_grupo2 = null;
		
		// Busqueda de Marca de Productos
		Gtk.Entry entry_idmarca_producto = null;
		Gtk.Entry entry_descripcion_marca_producto = null;
		
		// Busqueda de Proveedores
		Gtk.Entry entry_id_proveedor = null;
		Gtk.Entry entry_nombre_proveedor = null;
		Gtk.Entry entry_formapago = null;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		string consulta_sql1 = "";
		string consulta_sql2 = "";
		string type_find = "";
		string order_sql = "";
		string comodin = "";
		
		//declaracion de columnas y celdas de treeview de busqueda
		TreeViewColumn col_idlista;		CellRendererText cellrt0;
		TreeViewColumn col_descripcion;	CellRendererText cellrt1;
			
		//Declaracion de ventana de error y mensaje
		protected Gtk.Window MyWinError;
		
		public void buscandor(object[] args, string[] args_sql, string type_find_,string order_sql_,string comodin_)		
		{
			Glade.XML gxml = new Glade.XML (null, "osiris.glade", "buscador", null);
			gxml.Autoconnect(this);
	        //Muestra ventana de Glade
			buscador.Show();
			
			//Console.WriteLine("nº de argumentos: {0}", args.Length);
			//for (int i = 0; i < args.Length; i++)
        	//Console.WriteLine("args[{0}] = {1} {2}", i, args[i],@args[i]);  
			switch (type_find_){	
				case "find_client":
					entry_id_cliente = (object) args[0] as Gtk.Entry;
					entry_nombre_cliente = (object) args[1] as Gtk.Entry;				
				break;			
			
				case "find_estado_region":
					entry_id_estado = (object) args[0] as Gtk.Entry;
					entry_estado = (object) args[1] as Gtk.Entry;
					togglebutton_editar_estado = (object) args[2] as Gtk.ToggleButton;
					frame2 = (object) args[3] as Gtk.Frame;
					togglebutton_editar_municipio = (object) args[4] as Gtk.ToggleButton;
					button_guardar_municipio = (object) args[5] as Gtk.Button;
					entry_municipio = (object) args[6] as Gtk.Entry;
					entry_id_municipio = (object) args[7] as Gtk.Entry;
				break;			
			
				case "find_municipio":
					entry_id_municipio = (object) args[0] as Gtk.Entry;
					entry_municipio = (object) args[1] as Gtk.Entry;
					togglebutton_editar_municipio = (object) args[2] as Gtk.ToggleButton;
					button_guardar_municipio = (object) args[3] as Gtk.Button;
				break;
				
				case "find_grupo_producto":
					entry_id_grupo = (object) args[0] as Gtk.Entry;
					entry_descripcion_grupo = (object) args[1] as Gtk.Entry;
					togglebutton_editar_grupo = (object) args[2] as Gtk.ToggleButton;
					button_guardar_grupo = (object) args[3] as Gtk.Button;
					checkbutton_activar_grupo  = (object) args[4] as Gtk.CheckButton;
					entry_porcentage_utilidad = (object) args[5] as Gtk.Entry;
					entry_id_centrocosto = (object) args[6] as Gtk.Entry;
					entry_descripcion_centrocosto = (object) args[7] as Gtk.Entry;
					button_buscar_centrocosto  = (object) args[8] as Gtk.Button;
				break;
				
				case "find_grupo1_producto":
					entry_id_grupo1 = (object) args[0] as Gtk.Entry;
					entry_descripcion_grupo1 = (object) args[1] as Gtk.Entry;
					togglebutton_editar_grupo1 = (object) args[2] as Gtk.ToggleButton;
					button_guardar_grupo1 = (object) args[3] as Gtk.Button;
					checkbutton_activar_grupo1  = (object) args[4] as Gtk.CheckButton;				
				break;
				
				case "find_grupo2_producto":
					entry_id_grupo2 = (object) args[0] as Gtk.Entry;
					entry_descripcion_grupo2 = (object) args[1] as Gtk.Entry;
					togglebutton_editar_grupo2 = (object) args[2] as Gtk.ToggleButton;
					button_guardar_grupo2 = (object) args[3] as Gtk.Button;
					checkbutton_activar_grupo2  = (object) args[4] as Gtk.CheckButton;				
				break;
				
				case "find_centrodecosto":
					entry_id_centrocosto = (object) args[0] as Gtk.Entry;
					entry_descripcion_centrocosto = (object) args[1] as Gtk.Entry;
				break;
							
				case "find_marca_producto":
					entry_idmarca_producto = (object) args[0] as Gtk.Entry;
					entry_descripcion_marca_producto = (object) args[1] as Gtk.Entry;
				break;
				
				case "find_proveedores":
					entry_id_proveedor = (object) args[0] as Gtk.Entry;
					entry_nombre_proveedor = (object) args[1] as Gtk.Entry;
					entry_formapago = (object) args[2] as Gtk.Entry;
				break;
			}			
			consulta_sql1 = (string) args_sql[0];
			consulta_sql2 =	(string) args_sql[1];
			type_find = type_find_;
			order_sql = order_sql_;
			comodin = comodin_;						
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
	  		button_selecciona.Clicked += new EventHandler(on_selecciona_busqueda);
			radiobutton1.Hide();
			radiobutton2.Hide();
			radiobutton3.Hide();
			combobox_busqueda.Hide();
			labelbusqueda1.Hide();
			labelbusqueda2.Hide();
			labelcantidad.Hide();
			entry_cantidad_producto.Hide();
	       	crea_treeview_busqueda();
			button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
		}
		
		void crea_treeview_busqueda()
		{
			treeViewEngineBuscador = new TreeStore(typeof(int),//0
													typeof(string),//1
													typeof(bool),//2
													typeof(string),//3
													typeof(string),//4
													typeof(string),//5
													typeof(string),//6
													typeof(string),//7
													typeof(string),//8
													typeof(string),//9
													typeof(string),//10
													typeof(string),//11
													typeof(bool),//12
													typeof(int),//13
													typeof(string),//14
													typeof(bool),// 15
			                                        typeof(string), // 16
			                                        typeof(string), // 17
			                                        typeof(string)); // 18
												
			lista_de_busqueda.Model = treeViewEngineBuscador;
			
			lista_de_busqueda.RulesHint = true;
							
			lista_de_busqueda.RowActivated += on_selecciona_busqueda;  // Doble click selecciono cliente*/
			col_idlista = new TreeViewColumn();
			cellrt0 = new CellRendererText();
			col_idlista.Title = "ID"; // titulo de la cabecera de la columna, si está visible
			col_idlista.PackStart(cellrt0, true);
			col_idlista.AddAttribute (cellrt0, "text", 0);    // la siguiente columna será 1
			//col_idcliente.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_idlista.SortColumnId = (int) col_treview.col_idlista;
			col_idlista.Resizable = true;
			
			col_descripcion = new TreeViewColumn();
			cellrt1 = new CellRendererText();
			col_descripcion.Title = "Descripcion";
			col_descripcion.PackStart(cellrt1, true);
			col_descripcion.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
			//col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_descripcion.SortColumnId = (int) col_treview.col_descripcion;
			col_descripcion.Resizable = true;
				            
			lista_de_busqueda.AppendColumn(col_idlista);
			lista_de_busqueda.AppendColumn(col_descripcion);			
		}
		
		enum col_treview
		{
			col_idlista,
			col_descripcion			
		}
		
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((bool) this.lista_de_busqueda.Model.GetValue (iter,2) == false){
					(cell as Gtk.CellRendererText).Foreground = "red";
				}else{		
					(cell as Gtk.CellRendererText).Foreground = "black";
				}
			}
		
		void on_llena_lista(object sender, EventArgs args)
		{
			llenando_lista_de_lista();
		}
		
		void llenando_lista_de_lista()
		{
			if((string) entry_expresion.Text.Trim() !=""){
				string connectionString = conexion_a_DB._url_servidor+
										conexion_a_DB._port_DB+
										conexion_a_DB._usuario_DB+
										conexion_a_DB._passwrd_user_DB;
				string nombrebd = conexion_a_DB._nombrebd;
									
				treeViewEngineBuscador.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
            	// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					if ((string) entry_expresion.Text.ToUpper() == "*"){
						comando.CommandText = consulta_sql1+order_sql;
					}else{
						comando.CommandText = consulta_sql2+(string) entry_expresion.Text.ToUpper()+comodin+order_sql;
					}
					//Console.WriteLine(comando.CommandText);
					NpgsqlDataReader lector = comando.ExecuteReader ();				
					while (lector.Read()){
						switch (type_find){	
							case "find_client":
								treeViewEngineBuscador.AppendValues ((int) lector["id_cliente"],//0
													(string) lector["descripcion_cliente"]);
							break;		
			
							case "find_estado_region":
								treeViewEngineBuscador.AppendValues ((int) lector["id_estado"],//0
													(string) lector["descripcion_estado"]);//1
							break;
			
							case "find_municipio":
								treeViewEngineBuscador.AppendValues( (int) lector["id_municipio"],
														(string) lector["descripcion_municipio"]);
							break;
							
							case "find_grupo_producto":
								treeViewEngineBuscador.AppendValues( (int) lector["id_grupo_producto"],
														(string) lector["descripcion_grupo_producto"],
							                            (bool) lector["activo_gp"],
							                            (string) Convert.ToString(lector["porcentage_utilidad_grupo"]),
							                            (string) Convert.ToString(lector["idcentrodecosto"]),
							                            (string) lector["descripcion_centro_de_costo"]);
							
								col_idlista.SetCellDataFunc(cellrt0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							break;
							
							case "find_grupo1_producto":
								treeViewEngineBuscador.AppendValues( (int) lector["id_grupo1_producto"],
														(string) lector["descripcion_grupo1_producto"],
							                            (bool) lector["activo"]);
							
								col_idlista.SetCellDataFunc(cellrt0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							break;
							
							case "find_grupo2_producto":
								treeViewEngineBuscador.AppendValues( (int) lector["id_grupo2_producto"],
														(string) lector["descripcion_grupo2_producto"],
							                            (bool) lector["activo"]);
							
								col_idlista.SetCellDataFunc(cellrt0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							break;
							
							case "find_centrodecosto":
								treeViewEngineBuscador.AppendValues( (int) lector["id_centro_de_costos"],
														(string) lector["descripcion_centro_de_costo"]);
							
							break;
							
							case "find_marca_producto":
								treeViewEngineBuscador.AppendValues ((int) lector["id_marca_producto"], //0
													(string) lector["descripcion"]);//1
							break;
							
							case "find_proveedores":
								treeViewEngineBuscador.AppendValues ((int) lector["id_proveedor"],//0
													(string) lector["descripcion_proveedor"],//1
							                        (bool) lector["proveedor_activo"], //, // 2
													(string) lector["direccion_proveedor"],//3
													(string) lector["colonia_proveedor"],//4
													(string) lector["municipio_proveedor"],//5
													(string) lector["estado_proveedor"],//6
													(string) lector["telefono1_proveedor"],//7
													(string) lector["contacto1_proveedor"],//8
													(string) lector["rfc_proveedor"],//9
													(string) lector["pagina_web_proveedor"],//10
													(string) lector["descripcion_forma_de_pago"]);//11
													//(string) lector["fax_proveedor"], //12
							                        //(int) lector["id_forma_de_pago"]);//13
							break;
						}
					}
				}catch (NpgsqlException ex){
	   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
				conexion.Close ();
			}
		}
		
		void on_selecciona_busqueda(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			
			if (lista_de_busqueda.Selection.GetSelected(out model, out iterSelected)){										
				int tomaid = (int) model.GetValue(iterSelected, 0);												
				switch (type_find){	
					case "find_client":
						entry_id_cliente.Text = tomaid.ToString();
						entry_nombre_cliente.Text = (string) model.GetValue(iterSelected, 1);
					break;
					
					case "find_estado_region":
						entry_id_estado.Text = tomaid.ToString();
						entry_estado.Text = (string) model.GetValue(iterSelected, 1);
						frame2.Sensitive = true;
						togglebutton_editar_estado.Sensitive = true;
						togglebutton_editar_estado.Active = false;
						togglebutton_editar_municipio.Sensitive = false;
						togglebutton_editar_municipio.Active = false;
						button_guardar_municipio.Sensitive = false;
						entry_municipio.Sensitive = false;
						entry_municipio.Text = "";
						entry_id_municipio.Text = "";
					break;
					
					case "find_municipio":
						entry_id_municipio.Text = tomaid.ToString();
						entry_municipio.Text = (string) model.GetValue(iterSelected, 1);
						togglebutton_editar_municipio.Sensitive = true;
						togglebutton_editar_municipio.Active = false;
						button_guardar_municipio.Sensitive = false;
					break;	
					
					case "find_grupo_producto":
						entry_id_grupo.Text = tomaid.ToString();
						entry_descripcion_grupo.Text = (string) model.GetValue(iterSelected, 1);
						checkbutton_activar_grupo.Active = (bool) model.GetValue(iterSelected, 2);
						entry_porcentage_utilidad.Text = (string) model.GetValue(iterSelected, 3);
						entry_id_centrocosto.Text = (string) model.GetValue(iterSelected, 4);
						entry_descripcion_centrocosto.Text = (string) model.GetValue(iterSelected, 5);
						button_guardar_grupo.Sensitive = false;
						togglebutton_editar_grupo.Sensitive = true;
						togglebutton_editar_grupo.Active = false;
						checkbutton_activar_grupo.Sensitive = false;
						entry_porcentage_utilidad.Sensitive = false;
						entry_id_centrocosto.Sensitive = false;
						entry_descripcion_centrocosto.Sensitive = false;
						button_buscar_centrocosto.Sensitive = false;
					break;
					
					case "find_grupo1_producto":
						entry_id_grupo1.Text = tomaid.ToString();
						entry_descripcion_grupo1.Text = (string) model.GetValue(iterSelected, 1);
						checkbutton_activar_grupo1.Active = (bool) model.GetValue(iterSelected, 2);						
						button_guardar_grupo1.Sensitive = false;
						togglebutton_editar_grupo1.Sensitive = true;
						togglebutton_editar_grupo1.Active = false;
						checkbutton_activar_grupo1.Sensitive = false;
					break;
					
					case "find_grupo2_producto":
						entry_id_grupo2.Text = tomaid.ToString();
						entry_descripcion_grupo2.Text = (string) model.GetValue(iterSelected, 1);
						checkbutton_activar_grupo2.Active = (bool) model.GetValue(iterSelected, 2);						
						button_guardar_grupo2.Sensitive = false;
						togglebutton_editar_grupo2.Sensitive = true;
						togglebutton_editar_grupo2.Active = false;
						checkbutton_activar_grupo2.Sensitive = false;
					break;
					
					case "find_centrodecosto":
						entry_id_centrocosto.Text = tomaid.ToString();
						entry_descripcion_centrocosto.Text = (string) model.GetValue(iterSelected, 1);
					break;	
					
					case "find_marca_producto":
						entry_idmarca_producto.Text = tomaid.ToString();
						entry_descripcion_marca_producto.Text = (string) model.GetValue(iterSelected, 1);
					break;
					
					case "find_proveedores":
						entry_id_proveedor.Text = tomaid.ToString();
						entry_nombre_proveedor.Text = (string) model.GetValue(iterSelected, 1);
						entry_formapago.Text = (string) model.GetValue(iterSelected, 11);
					break;					
				}				
			}
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();			
		}
					
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		void onKeyPressEvent_enter(object sender, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;				
				 llenando_lista_de_lista();
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