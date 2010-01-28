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
// Programa		: hscmty.cs
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
		//Declarando ventana de captura de facturas de ordenes de compra
		[Widget] Gtk.Window captura_facturas_orden_compra;
		//[Widget] Gtk.CheckButton checkbutton_factura_sin_orden;
		//[Widget] Gtk.Entry entry_orden_de_compra;
		[Widget] Gtk.Button button_busca_orden_compra;
		[Widget] Gtk.Entry entry_fecha_orden_compra;
		[Widget] Gtk.Entry entry_id_quien_hizo;
		[Widget] Gtk.Entry entry_proveedor;
		[Widget] Gtk.Button button_busca_proveedor;
		[Widget] Gtk.Entry entry_direccion_proveedor;
		[Widget] Gtk.Entry entry_tel_proveedor;
		[Widget] Gtk.Entry entry_contacto_proveedor;
		[Widget] Gtk.Entry entry_forma_pago_prove;
		[Widget] Gtk.TreeView treeview_lista_productos;
		[Widget] Gtk.Entry entry_producto;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.Button button_quitar_producto;
		[Widget] Gtk.Button button_guardar;
		[Widget] Gtk.Button button_salir;
		[Widget] Gtk.Statusbar statusbar_captura_factura_orden_compra;
		
		//Declarando ventanas de busqueda
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_buscar_busqueda;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.TreeView lista_de_busqueda;
		[Widget] Gtk.TreeView lista_de_medicos;
		[Widget] Gtk.ComboBox combobox_tipo_busqueda;
		
		//Declaracion de ventana de busqueda de productos
		[Widget] Gtk.Window busca_producto;
		[Widget] Gtk.RadioButton radiobutton_nombre;
		[Widget] Gtk.RadioButton radiobutton_codigo;
		[Widget] Gtk.TreeView lista_de_producto;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombreempleado;
		string connectionString;
		string nombrebd;	
		class_conexion conexion_a_DB = new class_conexion();
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		private TreeStore treeViewEngineproveedores;
		private TreeStore treeViewEngineBusca2;
		
		//declaracion de columnas y celdas de treeview de busqueda
		TreeViewColumn col_idproducto;			CellRendererText cellr0;
		TreeViewColumn col_desc_producto;	CellRendererText cellr1;
		TreeViewColumn col_precioprod;			CellRendererText cellrt2;
		TreeViewColumn col_ivaprod;				CellRendererText cellrt3;
		TreeViewColumn col_totalprod;			CellRendererText cellrt4;
		TreeViewColumn col_descuentoprod;	CellRendererText cellrt5;
		TreeViewColumn col_preciocondesc;	CellRendererText cellrt6;
		TreeViewColumn col_grupoprod;			CellRendererText cellrt7;
		TreeViewColumn col_grupo1prod;		CellRendererText cellrt8;
		TreeViewColumn col_grupo2prod;		CellRendererText cellrt9;
		TreeViewColumn col_costoprod_uni;	CellRendererText cellrt12;
		TreeViewColumn col_aplica_iva;			CellRendererText cellrt19;
		TreeViewColumn col_cobro_activo;		CellRendererText cellrt20;
	
		public factura_orden_compra(string LoginEmp_,string nombreempleado_,string nombrebd_)
		{
			LoginEmpleado = LoginEmp_;
			nombreempleado = nombreempleado_;
			//NomEmpleado = NomEmpleado_;
			//AppEmpleado = AppEmpleado_;
			//ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "captura_facturas_orden_compra", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        captura_facturas_orden_compra.Show();
			//crea_treeview_abonos();
			//llenando_lista_de_abonos();
			//button_guardar.Clicked += new EventHandler(on_button_guardar_clicked);
			//button_imprimir.Clicked += new EventHandler(on_button_imprimir_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_busca_proveedor.Clicked += new EventHandler(on_busca_proveedores);
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			//checkbutton_nuevo_abono.Clicked += new EventHandler(on_checkbutton_nuevo_abono_clicked);
			/*entry_monto_abono.Sensitive = false;
			entry_recibo_caja.Sensitive = false;
			entry_presupuesto.Sensitive = false;
			entry_paquete.Sensitive = false;
			entry_dia.Sensitive = false;
			entry_dia.Text = DateTime.Now.ToString("dd");
			entry_mes.Sensitive = false;
			entry_mes.Text = DateTime.Now.ToString("MM");
			entry_ano.Sensitive = false;
			entry_ano.Text = DateTime.Now.ToString("yyyy");
			entry_concepto_abono.Sensitive = false;
			button_guardar.Sensitive = false;
			combobox_formapago.Sensitive = false;*/
			statusbar_captura_factura_orden_compra.Pop(0);
			statusbar_captura_factura_orden_compra.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+nombreempleado);
			statusbar_captura_factura_orden_compra.HasResizeGrip = false;
		}
		
		void on_busca_proveedores(object sender, EventArgs args)
			{
				Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador", null);
				gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
		        button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_proveedores);
		        entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
		        button_selecciona.Clicked += new EventHandler(on_selecciona_proveedor);
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
				
			lista_de_busqueda.RowActivated += on_selecciona_proveedor;  // Doble click selecciono paciente
			
			
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
		
		void on_llena_lista_proveedores(object sender, EventArgs args)
			{
				llenando_lista_de_proveedores();
			}
		
		void llenando_lista_de_proveedores()
			{
				treeViewEngineproveedores.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
	            // Verifica que la base de datos este conectada
				try
				{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				if ((string) entry_expresion.Text.ToUpper() == "*")
				{
					comando.CommandText = "SELECT descripcion_proveedor,direccion_proveedor,rfc_proveedor,curp_proveedor, "+
								"colonia_proveedor,municipio_proveedor,estado_proveedor,telefono1_proveedor, "+ 
								"telefono2_proveedor,celular_proveedor,cp_proveedor, proveedor_activo, "+
								"id_proveedor,contacto1_proveedor,mail_proveedor,pagina_web_proveedor,"+
								"osiris_erp_proveedores.id_forma_de_pago, descripcion_forma_de_pago AS descripago "+
								"FROM osiris_erp_proveedores, osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_proveedores.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
								"ORDER BY descripcion_proveedor;";															
				}
				else
				{
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
				while (lector.Read())
				{	
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
			}
			
				catch (NpgsqlException ex)
				{
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
				}
			conexion.Close ();
		}
		
		void on_selecciona_proveedor(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_busqueda.Selection.GetSelected(out model, out iterSelected)) 
 			{
 				//int tomaidproveedor = (int) model.GetValue(iterSelected, 0);
 				//entry_id_proveedor.Text = tomaidproveedor.ToString();
 				entry_proveedor.Text = (string) model.GetValue(iterSelected, 1);
				entry_direccion_proveedor.Text = (string) model.GetValue(iterSelected, 2)+" "+(string) model.GetValue(iterSelected, 3)+" "+(string) model.GetValue(iterSelected, 4)+" "+(string) model.GetValue(iterSelected, 5); 
				entry_tel_proveedor.Text = (string) model.GetValue(iterSelected, 6);
				entry_contacto_proveedor.Text = (string) model.GetValue(iterSelected, 7);
				entry_forma_pago_prove.Text = (string) model.GetValue(iterSelected, 12);
				
				button_guardar.Sensitive = false;
				
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
		
		void on_button_busca_producto_clicked(object sender, EventArgs a)
		{
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			busca_producto.Show();
			crea_treeview_busqueda();
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
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
				
				col_idproducto = new TreeViewColumn();
				cellr0 = new CellRendererText();
				col_idproducto.Title = "ID Producto"; // titulo de la cabecera de la columna, si está visible
				col_idproducto.PackStart(cellr0, true);
				col_idproducto.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
				col_idproducto.SortColumnId = (int) Column_prod.col_idproducto;
			
				col_desc_producto = new TreeViewColumn();
				cellr1 = new CellRendererText();
				col_desc_producto.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
				col_desc_producto.PackStart(cellr1, true);
				col_desc_producto.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
				col_desc_producto.SortColumnId = (int) Column_prod.col_desc_producto;
				col_desc_producto.Resizable = true;
				//cellr0.Editable = true;   // Permite edita este campo
            
				col_precioprod = new TreeViewColumn();
				cellrt2 = new CellRendererText();
				col_precioprod.Title = "Precio Producto";
				col_precioprod.PackStart(cellrt2, true);
				col_precioprod.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
				col_precioprod.SortColumnId = (int) Column_prod.col_precioprod;
            
				col_ivaprod = new TreeViewColumn();
				cellrt3 = new CellRendererText();
				col_ivaprod.Title = "I.V.A.";
				col_ivaprod.PackStart(cellrt3, true);
				col_ivaprod.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 2 en vez de 3
				col_ivaprod.SortColumnId = (int) Column_prod.col_ivaprod;
            
				col_totalprod = new TreeViewColumn();
				cellrt4 = new CellRendererText();
				col_totalprod.Title = "Total";
				col_totalprod.PackStart(cellrt4, true);
				col_totalprod.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 3 en vez de 4
				col_totalprod.SortColumnId = (int) Column_prod.col_totalprod;
            
				col_descuentoprod = new TreeViewColumn();
				cellrt5 = new CellRendererText();
				col_descuentoprod.Title = "% Descuento";
				col_descuentoprod.PackStart(cellrt5, true);
				col_descuentoprod.AddAttribute (cellrt5, "text", 5); // la siguiente columna será 5 en vez de 6
				col_descuentoprod.SortColumnId = (int) Column_prod.col_descuentoprod;
      
				col_preciocondesc = new TreeViewColumn();
				cellrt6 = new CellRendererText();
				col_preciocondesc.Title = "Precio con Desc.";
				col_preciocondesc.PackStart(cellrt6, true);
				col_preciocondesc.AddAttribute (cellrt6, "text", 6);     // la siguiente columna será 6 en vez de 7
				col_preciocondesc.SortColumnId = (int) Column_prod.col_preciocondesc;
            
				col_grupoprod = new TreeViewColumn();
				cellrt7 = new CellRendererText();
				col_grupoprod.Title = "Grupo Producto";
				col_grupoprod.PackStart(cellrt7, true);
				col_grupoprod.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 7 en vez de 8
				col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
            
				col_grupo1prod = new TreeViewColumn();
				cellrt8 = new CellRendererText();
				col_grupo1prod.Title = "Grupo1 Producto";
				col_grupo1prod.PackStart(cellrt8, true);
				col_grupo1prod.AddAttribute (cellrt8, "text", 8); // la siguiente columna será 9 en vez de 
				col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
                        
				col_grupo2prod = new TreeViewColumn();
				cellrt9 = new CellRendererText();
				col_grupo2prod.Title = "Grupo2 Producto";
				col_grupo2prod.PackStart(cellrt9, true);
				col_grupo2prod.AddAttribute (cellrt9, "text", 9); // la siguiente columna será 10 en vez de 9
				col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;
				
				col_costoprod_uni = new TreeViewColumn();
				cellrt12 = new CellRendererText();
				col_costoprod_uni.Title = "Precio Unitario";
				col_costoprod_uni.PackStart(cellrt12, true);
				col_costoprod_uni.AddAttribute (cellrt12, "text", 12); // la siguiente columna será 1 en vez de 2
				col_costoprod_uni.SortColumnId = (int) Column_prod.col_costoprod_uni;
            				
				col_aplica_iva = new TreeViewColumn();
				cellrt19 = new CellRendererText();
				col_aplica_iva.Title = "Iva Activo?";
				col_aplica_iva.PackStart(cellrt19, true);
				col_aplica_iva.AddAttribute (cellrt19, "text", 19); // la siguiente columna será 10 en vez de 9
				col_aplica_iva.SortColumnId = (int) Column_prod.col_aplica_iva;
				
				col_cobro_activo = new TreeViewColumn();
				cellrt20 = new CellRendererText();
				col_cobro_activo.Title = "Prod. Activo?";
				col_cobro_activo.PackStart(cellrt20, true);
				col_cobro_activo.AddAttribute (cellrt20, "text", 20); // la siguiente columna será 10 en vez de 9
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
			col_costo_prod,			col_cant_embalaje,
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
				float valoriva = 15;
							
				while (lector.Read()){
					calculodeiva = 0;
					preciomasiva = 0;
					tomaprecio = float.Parse((string) lector["preciopublico"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					tomadescue = float.Parse((string) lector["porcentagesdesc"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					preciocondesc = tomaprecio;
					if ((bool) lector["aplicar_iva"])
					{
						calculodeiva = (tomaprecio * valoriva)/100;
					}
					if ((bool) lector["aplica_descuento"])
					{
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
					col_idproducto.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_desc_producto.SetCellDataFunc(cellr1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
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
			string toma_valor;
			TreeModel model;
			TreeIter iterSelected;
			//ListStore store_grupo = new ListStore( typeof (string), typeof (int));
				if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
				
					//utilidad_anterior = decimal.Parse((string) model.GetValue(iterSelected,14));
							
	 				//toma_valor = (string) model.GetValue(iterSelected, 0);
	 				
	 				//this.entry_codigo_producto.Text = (string) model.GetValue(iterSelected, 0);
					//this.entry_descripcion.Text = (string) model.GetValue(iterSelected, 1);
					//this.entry_nombre_articulo.Text = (string) model.GetValue(iterSelected, 10); 
					//this.entry_nombre_generico.Text = (string) model.GetValue(iterSelected, 11);
					//toma_valor = (string) model.GetValue(iterSelected, 2);
					
					/*this.entry_precio_publico.Text = toma_valor.TrimStart();
					toma_valor = (string) model.GetValue(iterSelected, 5);
					
					this.entry_descuento.Text = toma_valor.TrimStart();
					toma_valor = (string) model.GetValue(iterSelected, 12);
					
					this.entry_precio_unitario.Text = toma_valor.TrimStart();
					toma_valor = (string) model.GetValue(iterSelected, 13);
					
					this.entry_porciento_utilidad.Text = toma_valor.TrimStart();
					toma_valor = (string) model.GetValue(iterSelected, 14);
					
					
					this.entry_costo.Text = toma_valor.TrimStart();
					toma_valor = (string) model.GetValue(iterSelected, 15);
					
					this.entry_embalaje.Text = toma_valor.TrimStart();
					
					// Lineas especial para municipio de San Nicolas
					toma_valor = (string) model.GetValue(iterSelected, 22);
					this.entry_precios_costunitario_sannico.Text = toma_valor.Trim();

					
					// llenar conbobox con los nombres de los grupos
					this.combobox_grupo.Clear();
					this.combobox_grupo1.Clear();
					this.combobox_grupo2.Clear();
					
					llenado_de_cmbox_grupos();
					
					this.descripgrupo = (string) model.GetValue(iterSelected, 7);
					this.idtipogrupo = long.Parse((string) model.GetValue(iterSelected, 16));
					this.descripgrupo1 = (string) model.GetValue(iterSelected, 8);
					this.idtipogrupo1 = long.Parse((string) model.GetValue(iterSelected, 17));
					this.descripgrupo2 = (string) model.GetValue(iterSelected, 9);
					this.idtipogrupo2 = long.Parse((string) model.GetValue(iterSelected, 18));
					this.entry_tipo_grupo.Text = descripgrupo.ToString(); 
					this.entry_tipo_grupo1.Text = descripgrupo1.ToString();
					this.entry_tipo_grupo2.Text = descripgrupo2.ToString();
					
					toma_valor = (string) model.GetValue(iterSelected, 14);
					this.entry_precios_costunitario_sannico.Text = (string) model.GetValue(iterSelected, 22);*/
					
					//calculando_utilidad();
					
					//this.button_actualizar.Sensitive = true;
					//this.button_grabar.Sensitive = false;
					
					/*this.checkbutton_apl_iva.Active = (bool) model.GetValue(iterSelected, 19);
					this.checkbutton_prod_activo.Active = (bool) model.GetValue(iterSelected, 20);
					
					if ((bool) model.GetValue(iterSelected, 21) == true){ 
						this.radiobutton_desc_si.Active = true;
						apldesc = "true";
					}else{
						this.radiobutton_desc_no.Active = true;
						apldesc = "false";	
					}*/
					
					//cierra la ventana despues que almaceno la informacion en variables
					Widget win = (Widget) sender;
					win.Toplevel.Destroy();
			}
		}
		
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((bool)lista_de_producto.Model.GetValue (iter,20)==true) { 
				if ((bool)lista_de_producto.Model.GetValue (iter,19)==true) { (cell as Gtk.CellRendererText).Foreground = "blue";
				}else{ (cell as Gtk.CellRendererText).Foreground = "black"; }
			}else{	(cell as Gtk.CellRendererText).Foreground = "red";  }
		}
		
		void cambia_colores_proveedor(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//if ((bool) lista_de_busqueda.Model.GetValue(iter,10) == false)
			//{(cell as Gtk.CellRendererText).Foreground = "darkgreen";		}
		}
		
		///////////////////////////////////////BOTON general de busqueda por enter///////////////////////////////////////////////		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;		
				llenando_lista_de_proveedores();
			}
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}					
	}
}