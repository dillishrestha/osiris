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
		[Widget] Gtk.Button button_salir = null;
		
		//Declarando ventana de captura de facturas de ordenes de compra
		[Widget] Gtk.Window captura_facturas_orden_compra = null;
		[Widget] Gtk.CheckButton checkbutton_factura_sin_orden = null;
		[Widget] Gtk.Entry entry_orden_de_compra = null;
		[Widget] Gtk.Button button_selecciona_ordencompra = null;
		[Widget] Gtk.Button button_busca_orden_compra = null;
		[Widget] Gtk.Entry entry_fecha_orden_compra = null;
		[Widget] Gtk.Entry entry_id_quien_hizo = null;
		[Widget] Gtk.Entry entry_num_factura_proveedor = null;
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
		
		[Widget] Gtk.Statusbar statusbar_captura_factura_orden_compra = null;
		
		//Declaracion de ventana de busqueda de productos
		[Widget] Gtk.Window busca_producto = null;
		[Widget] Gtk.Button button_buscar_busqueda = null;
		[Widget] Gtk.Button button_selecciona = null;
		[Widget] Gtk.Entry entry_expresion = null;
		[Widget] Gtk.RadioButton radiobutton_nombre = null;
		[Widget] Gtk.RadioButton radiobutton_codigo = null;
		
		//[Widget] Gtk
		[Widget] Gtk.TreeView lista_de_producto = null;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombreempleado;
		string connectionString;
		string nombrebd;	
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();
		//class_public classpublic = new class_public();
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		TreeStore treeViewEngineListaProdRequi;	// Lista de proctos que se van a comprar
		TreeStore treeViewEngineBusca2;
		
		// declaro treevie de productos para la requisicion
		TreeViewColumn col_recibido;
		TreeViewColumn col_numerofactura;
		TreeViewColumn col_cant_ordencompra;
		TreeViewColumn col_cant_recibida;
		TreeViewColumn col_idproducto_provee;
		TreeViewColumn col_empaqueproducto;
				
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
		TreeViewColumn col_costoprod_uni;		CellRendererText cellrt12;
		TreeViewColumn col_aplica_iva;			CellRendererText cellrt19;
		TreeViewColumn col_cobro_activo;		CellRendererText cellrt20;
		
		public factura_orden_compra(string LoginEmp_,string nombreempleado_,string nombrebd_)
		{
			LoginEmpleado = LoginEmp_;
			nombreempleado = nombreempleado_;
			
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "captura_facturas_orden_compra", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
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
						
			entry_num_factura_proveedor.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
			entry_orden_de_compra.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
			entry_num_factura_proveedor.ModifyFont(fontdesc);  // Arial y Negrita
			entry_orden_de_compra.ModifyFont(fontdesc);  // Arial y Negrita
			
			entry_producto.Sensitive = false;
			button_busca_producto.Sensitive = false;
			button_quitar_producto.Sensitive = false;
			button_busca_proveedor.Sensitive = false;
			button_guardar.Sensitive = false;
			crea_treeview_capturafactura();
						
			statusbar_captura_factura_orden_compra.Pop(0);
			statusbar_captura_factura_orden_compra.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+nombreempleado);
			statusbar_captura_factura_orden_compra.HasResizeGrip = false;
		}
			
		// Ademas controla la tecla ENTRER para ver el procedimiento
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione
		void onKeyPressEvent_enter_ordencompra(object o, Gtk.KeyPressEventArgs args)
		{
			Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				llenado_orden_de_compra();
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace){
				args.RetVal = true;
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
			}else{
				button_busca_proveedor.Sensitive = false;
				button_busca_producto.Sensitive = false;
				button_quitar_producto.Sensitive = false;
				button_busca_proveedor.Sensitive = false;
				entry_fecha_orden_compra.Text = "";
		 	}
		}
		
		void crea_treeview_capturafactura()
		{
			treeViewEngineListaProdRequi = new TreeStore(typeof(bool), 
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
						
			col_recibido = new TreeViewColumn();
			CellRendererToggle cellrToggle = new CellRendererToggle();
			col_recibido.Title = "Seleccion"; // titulo de la cabecera de la columna, si está visible
			col_recibido.PackStart(cellrToggle, true);
			col_recibido.AddAttribute (cellrToggle, "active", 0);
			cellrToggle.Activatable = true;
			cellrToggle.Toggled += selecciona_fila;
			col_recibido.SortColumnId = (int) col_productos_recibidos.col_recibido;			
			
			
			col_numerofactura = new TreeViewColumn();
			CellRendererText cel_numerofactura = new CellRendererText();
			col_numerofactura.Title = "N° Factura"; // titulo de la cabecera de la columna, si está visible
			col_numerofactura.PackStart(cel_numerofactura, true);
			col_numerofactura.AddAttribute(cel_numerofactura, "text", 1);
			col_numerofactura.SortColumnId = (int) col_productos_recibidos.col_numerofactura;
			cel_numerofactura.Edited += NumberCellEdited_NroFactura;
			cel_numerofactura.Editable = true;
										
			col_cant_ordencompra = new TreeViewColumn();
			cellrt2 = new CellRendererText();
			col_cant_ordencompra.Title = "Cant. O.C.";
			col_cant_ordencompra.PackStart(cellrt2, true);
			col_cant_ordencompra.AddAttribute (cellrt2, "text", 2);
			col_cant_ordencompra.SortColumnId = (int) col_productos_recibidos.col_cant_ordencompra;
			
			col_cant_recibida = new TreeViewColumn();
			CellRendererText cel_cant_recibida = new CellRendererText();
			col_cant_recibida.Title = "Cant.Recibida";
			col_cant_recibida.PackStart(cel_cant_recibida, true);
			col_cant_recibida.AddAttribute(cel_cant_recibida, "text", 3);
			col_cant_recibida.SortColumnId = (int) col_productos_recibidos.col_cant_recibida;
			cel_cant_recibida.Edited += NumberCellEdited_Recibida;
			cel_cant_recibida.Editable = true;
			
			col_precioprod = new TreeViewColumn();
			cellrt0 = new CellRendererText();
			col_precioprod.Title = "Precio Producto";
			col_precioprod.PackStart(cellrt2, true);
			col_precioprod.AddAttribute (cellrt2, "text", 4);
			col_precioprod.SortColumnId = (int) col_productos_recibidos.col_precioprod;
			
			col_idproducto_provee = new TreeViewColumn();
			cellrt1 = new CellRendererText();
			col_idproducto_provee.Title = "Cod.Prod.Prov.";
			col_idproducto_provee.PackStart(cellrt0, true);
			col_idproducto_provee.AddAttribute (cellrt0, "text", 5);
			col_idproducto_provee.SortColumnId = (int) col_productos_recibidos.col_idproducto_provee;
			
			col_idproducto = new TreeViewColumn();
			cellrt2 = new CellRendererText();
			col_idproducto.Title = "ID Producto";
			col_idproducto.PackStart(cellrt2, true);
			col_idproducto.AddAttribute (cellrt2, "text", 6);
			col_idproducto.SortColumnId = (int) col_productos_recibidos.col_idproducto;
			
			col_desc_producto = new TreeViewColumn();
			cellrt3 = new CellRendererText();
			col_desc_producto.Title = "Descripcion de Producto";
			col_desc_producto.PackStart(cellrt3, true);
			col_desc_producto.AddAttribute (cellrt3, "text", 7);
			col_desc_producto.SortColumnId = (int) col_productos_recibidos.col_desc_producto;
			col_desc_producto.Resizable = true;
			
			col_empaqueproducto = new TreeViewColumn();
			cellrt4 = new CellRendererText();
			col_empaqueproducto.Title = "Empaque";
			col_empaqueproducto.PackStart(cellrt4, true);
			col_empaqueproducto.AddAttribute (cellrt4, "text", 8);
			col_empaqueproducto.SortColumnId = (int) col_productos_recibidos.col_empaqueproducto;
			
			lista_productos_a_recibir.AppendColumn(col_recibido);				// 0
			lista_productos_a_recibir.AppendColumn(col_numerofactura);			// 1
			lista_productos_a_recibir.AppendColumn(col_cant_ordencompra);		// 2
			lista_productos_a_recibir.AppendColumn(col_cant_recibida);			// 3
			lista_productos_a_recibir.AppendColumn(col_precioprod);				// 4
			lista_productos_a_recibir.AppendColumn(col_idproducto_provee);		// 5
			lista_productos_a_recibir.AppendColumn(col_idproducto);				// 6
			lista_productos_a_recibir.AppendColumn(col_desc_producto);			// 7
			lista_productos_a_recibir.AppendColumn(col_empaqueproducto);		// 8
		}
		
		enum col_productos_recibidos{
			col_recibido,
			col_numerofactura,
			col_cant_ordencompra,
			col_cant_recibida,
			col_precioprod,
			col_idproducto_provee,
			col_idproducto,
			col_desc_producto,
			col_empaqueproducto
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
		
		void NumberCellEdited_NroFactura (object o, EditedArgs args)
		{
			Gtk.TreeIter iter;
			treeViewEngineListaProdRequi.GetIter (out iter, new Gtk.TreePath (args.Path));
			treeViewEngineListaProdRequi.SetValue(iter,(int) col_productos_recibidos.col_numerofactura,args.NewText);			
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
				treeViewEngineListaProdRequi.SetValue(iter,(int) col_productos_recibidos.col_cant_recibida,args.NewText);
				bool old = (bool) lista_productos_a_recibir.Model.GetValue (iter,0);
				lista_productos_a_recibir.Model.SetValue(iter,0,!old);
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
			object[] parametros_objetos = {entry_id_proveedor,entry_nombre_proveedor,entry_formapago,entry_direccion_proveedor};
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
			classfind_data.buscandor(parametros_objetos,parametros_sql,"find_proveedores_OC"," ORDER BY descripcion_proveedor;","%' ");
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
			cellrt0 = new CellRendererText();
			col_idproducto.Title = "ID Producto"; // titulo de la cabecera de la columna, si está visible
			col_idproducto.PackStart(cellrt0, true);
			col_idproducto.AddAttribute (cellrt0, "text", 0);    // la siguiente columna será 1 en vez de 1
			col_idproducto.SortColumnId = (int) Column_prod.col_idproducto;
			
			col_desc_producto = new TreeViewColumn();
			cellrt1 = new CellRendererText();
			col_desc_producto.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
			col_desc_producto.PackStart(cellrt1, true);
			col_desc_producto.AddAttribute (cellrt1, "text", 1);    // la siguiente columna será 1 en vez de 1
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
				float valoriva = float.Parse(classpublic.ivaparaaplicar);
							
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
			string toma_valor;
			TreeModel model;
			TreeIter iterSelected;			
			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){				
				//cierra la ventana despues que almaceno la informacion en variables
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
		
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((bool)lista_de_producto.Model.GetValue (iter,20)==true){ 
				if ((bool)lista_de_producto.Model.GetValue (iter,19)==true) { (cell as Gtk.CellRendererText).Foreground = "blue";
				}else{ (cell as Gtk.CellRendererText).Foreground = "black"; }
			}else{	(cell as Gtk.CellRendererText).Foreground = "red";  }
		}
		
		///////////////////////////////////////BOTON general de busqueda por enter///////////////////////////////////////////////		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;		
				//llenando_lista_de_proveedores();
			}
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}					
	}
}