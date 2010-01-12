// alta_productos.cs created with MonoDevelop
// User: jbuentello at 12:42 a 18/07/2008
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	 Ing.Jesus Buentello Garza (programacion Mono)		
//	 				  
// Licencia		: GLP
// S.O. 		: GNU/Linux RH4 ES
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
//////////////////////////////////////////////////////////////

using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class alta_productos_proveedores
	{
		
		//nuevos productos
		[Widget] Gtk.Window alta_producto = null;
		[Widget] Gtk.Button button_nuevo_producto = null;
		[Widget] Gtk.CheckButton checkbutton_nuevo_producto = null;
		[Widget] Gtk.Entry entry_tipo_unidad = null;
		[Widget] Gtk.Entry entry_embalaje = null;
		[Widget] Gtk.Entry entry_codigo = null;
		[Widget] Gtk.Entry entry_cod_barras = null;
		[Widget] Gtk.Button button_busca_provedor = null;
		[Widget] Gtk.Button button_guarda = null;
		[Widget] Gtk.Button button_editar = null;		
		[Widget] Gtk.Button button_salir = null;
		[Widget] Gtk.Entry entry_provedor = null;
		[Widget] Gtk.Entry entry_producto = null;
		[Widget] Gtk.Entry entry_precio = null;
		[Widget] Gtk.TreeView lista_productos_agregados = null;
		[Widget] Gtk.Button button_agrega = null;
		[Widget] Gtk.Button button_quita = null;
		[Widget] Gtk.Button button_aprobar = null;
		[Widget] Gtk.Entry entry_clave = null;
		[Widget] Gtk.TreeView lista_precios_proveedor = null;
		[Widget] Gtk.CheckButton checkbutton_aprobar = null;
		[Widget] Gtk.Entry entry_id_provedor = null;
		[Widget] Gtk.Label label_titulo_cantidad = null;
		[Widget] Gtk.ComboBox combobox_tipo_unidad = null;
		[Widget] Gtk.Statusbar statusbar_alta_producto = null;
		
		/////// Ventana Busqueda de productos\\\\\\\\
		[Widget] Gtk.TreeView lista_de_producto = null;
		[Widget] Gtk.RadioButton radiobutton_nombre = null;
		[Widget] Gtk.RadioButton radiobutton_codigo = null;
		[Widget] Gtk.Label label_cantidad = null;
		//[Widget] Gtk.Button button_agrega_extra;
		[Widget] Gtk.Entry entry_cantidad_aplicada = null;
		
		//provedores
		[Widget] Gtk.Window catalogo_proveedore  = null;
		[Widget] Gtk.Button button_buscar_busqueda = null;
		[Widget] Gtk.Entry entry_expresion = null;
		[Widget] Gtk.Button button_selecciona = null;
			
		[Widget] Gtk.TreeView lista_de_busqueda = null;
		
		public string LoginEmpleado;
		public string NomEmpleado;
		public string AppEmpleado;
		public string ApmEmpleado;
		public string nombrebd;
		public string codigohscmty = "";
		public string variable = "";
		public string tipounidadproducto = "";
		public string combo = "";
		public bool edita = false;
		public string secuencial = "";
		
		public int id_provedor;
		
		public string connectionString = "Server=localhost;" +
			                             "Port=5432;" +
						                 "User ID=admin;" +
						                 "Password=1qaz2wsx;";
		
		//Declaracion de ventana de error y pregunta
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		private TreeStore treeViewEngineproveedores;
		private TreeStore treeViewEngineproductos;
		private TreeStore treeViewEngineaprobados;
		private TreeStore treeViewEngineBusca2;

		//declaracion de columnas y celdas de treeview de busqueda de productos
		public TreeViewColumn col_idproducto;		public CellRendererText cellr0;
		public TreeViewColumn col_desc_producto;	public CellRendererText cellr1;
		public TreeViewColumn col_precioprod;		public CellRendererText cellrt2;
		public TreeViewColumn col_ivaprod;			public CellRendererText cellrt3;
		public TreeViewColumn col_totalprod;		public CellRendererText cellrt4;
		public TreeViewColumn col_descuentoprod;	public CellRendererText cellrt5;
		public TreeViewColumn col_preciocondesc;	public CellRendererText cellrt6;
		public TreeViewColumn col_stock_actual;		public CellRendererText cellrt7;
		public TreeViewColumn col_grupoprod;		public CellRendererText cellrt8;
		public TreeViewColumn col_grupo1prod;		public CellRendererText cellrt9;
		public TreeViewColumn col_grupo2prod;		public CellRendererText cellrt10;
		public TreeViewColumn col_aplica_iva;		public CellRendererText cellrt20;
		public TreeViewColumn col_cobro_activo;		public CellRendererText cellrt21;
		
		public alta_productos_proveedores(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = _nombrebd_;
			
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "alta_producto", null);
			gxml.Autoconnect (this);        
			alta_producto.Show();
			crea_lista_productos_agregados();
			crea_treeview_autoriza();
			button_busca_provedor.Clicked += new EventHandler(on_busca_provedor_clicked);
			this.button_guarda.Clicked += new EventHandler(on_button_guarda_clicked);
			this.checkbutton_nuevo_producto.Clicked += new EventHandler(on_checkbutton_nuevo_producto);
			this.checkbutton_aprobar.Clicked += new EventHandler(on_checkbutton_aprobar);
			button_selecciona.Clicked += new EventHandler(on_button_selecciona_clicked);
			this.button_agrega.Clicked += new EventHandler(on_button_agrega_clicked);
			this.button_quita.Clicked += new EventHandler(on_button_quitar_aplicados_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			this.button_aprobar.Clicked += new EventHandler(on_button_aprobar_clicked);
			this.entry_id_provedor.KeyPressEvent += onKeyPressEvent_enter_provedor;
			this.button_editar.Clicked += new EventHandler(on_editar_producto_clicked);
			this.entry_embalaje.KeyPressEvent += onKeyPressEvent_enter_valida;
			this.entry_precio.KeyPressEvent += onKeyPressEvent_enter_valida;
			
			llena_combo_tipounidad();

			this.button_editar.Sensitive = false;
			this.entry_clave.Sensitive = false;
			this.button_aprobar.Sensitive = false;
			this.button_agrega.Sensitive = false;
			this.button_quita.Sensitive = false;
			this.entry_producto.Sensitive = false;
			entry_precio.Sensitive = false;
			entry_embalaje.Sensitive = false;
			entry_codigo.Sensitive = false;
			entry_cod_barras.Sensitive = false;
			this.button_selecciona.Sensitive = false;
			this.lista_productos_agregados.Sensitive = false;
			this.lista_precios_proveedor.Sensitive = false;
			this.entry_provedor.Sensitive = false;
			this.entry_id_provedor. Sensitive = false;
			this.button_busca_provedor.Sensitive = false;
			this.button_guarda.Sensitive = false;
			this.checkbutton_aprobar.Active = true;
			statusbar_alta_producto.Pop(0);
			statusbar_alta_producto.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_alta_producto.HasResizeGrip = false;
		}
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		void onKeyPressEvent_enter_provedor(object o, Gtk.KeyPressEventArgs args)
		{
			
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				
				if(this.checkbutton_nuevo_producto.Active == true){
					this.entry_id_provedor.Sensitive = false;
					this.entry_provedor.Sensitive = false;
					llenando_lista_de_aprobados();

				}

				if(this.checkbutton_aprobar.Active == true){
					llenando_lista_de_aprobados();
				}
				
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				// Verifica que la base de datos este conectada
				try
				{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
				
								comando.CommandText = "SELECT descripcion_proveedor,direccion_proveedor,rfc_proveedor,curp_proveedor, "+
								"colonia_proveedor,municipio_proveedor,estado_proveedor,telefono1_proveedor, "+ 
								"telefono2_proveedor,celular_proveedor,cp_proveedor, proveedor_activo, "+
								"id_proveedor,contacto1_proveedor,mail_proveedor,pagina_web_proveedor, "+
								"osiris_erp_proveedores.id_forma_de_pago, descripcion_forma_de_pago AS descripago "+
								"FROM osiris_erp_proveedores, osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_proveedores.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
							    "AND id_proveedor = '"+(string) this.entry_id_provedor.Text+"' "+
								"ORDER BY descripcion_proveedor;";
				
					NpgsqlDataReader lector = comando.ExecuteReader ();
					if (lector.Read())
					{	
						this.entry_provedor.Text = (string) lector["descripcion_proveedor"];//12
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					                                               MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
				conexion.Close ();
		
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
			{
				args.RetVal = true;
			}
		}
		
		void llena_combo_tipounidad()
		{
			combobox_tipo_unidad.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_tipo_unidad.PackStart(cell3, true);
			combobox_tipo_unidad.AddAttribute(cell3,"text",0);
	        
			ListStore store3 = new ListStore( typeof (string));
			combobox_tipo_unidad.Model = store3;
			
				store3.AppendValues (" ");
				store3.AppendValues ("PIEZA");
				store3.AppendValues ("KILO");
				store3.AppendValues ("LITRO");
				store3.AppendValues ("GRAMO");
				store3.AppendValues ("METRO");
				store3.AppendValues ("CENTIMETRO");
				store3.AppendValues ("CAJA");
				store3.AppendValues ("PULGADA");
			
	      
			TreeIter iter3;
			if (store3.GetIterFirst(out iter3)){
				combobox_tipo_unidad.SetActiveIter (iter3);
			}
			combobox_tipo_unidad.Changed += new EventHandler (onComboBoxChanged_tipo_unidad);
		}
		
		void onComboBoxChanged_tipo_unidad (object sender, EventArgs args)
		{
	    	ComboBox combobox_tipo_unidad = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_tipo_unidad.GetActiveIter (out iter)){
				tipounidadproducto = (string) combobox_tipo_unidad.Model.GetValue(iter,0);    		 
			}
		}		
		
		void on_button_quitar_aplicados_clicked (object sender, EventArgs args)
		{
		
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de Eliminar este producto?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
		 	if (miResultado == ResponseType.Yes){
		 	
				if(this.checkbutton_aprobar.Active == false && this.checkbutton_nuevo_producto.Active == true)
				{
					TreeIter iter;
					TreeModel model;
					if (this.checkbutton_nuevo_producto.Active == true){
						if (this.lista_productos_agregados.Selection.GetSelected (out model, out iter)) {
							this.treeViewEngineproductos.Remove (ref iter); 					
						}
					}
				}
				
				//eliminar un producto que ya estaba guardado
				if(this.checkbutton_aprobar.Active == true && this.checkbutton_nuevo_producto.Active == false)
				{

					TreeIter iter;
					TreeModel model;
					NpgsqlConnection conexion2; 
					conexion2 = new NpgsqlConnection (connectionString+nombrebd);
					try{
						conexion2.Open ();
						NpgsqlCommand comando2; 
						comando2 = conexion2.CreateCommand();
						if (this.treeViewEngineaprobados.GetIterFirst(out iter)){
							if (this.lista_precios_proveedor.Selection.GetSelected (out model, out iter)) {
								//if ((bool)lista_precios_proveedor.Model.GetValue (iter,0) == true){	
								comando2.CommandText = "SELECT id_proveedor,codigo_producto_proveedor,codigo_de_barra FROM osiris_catalogo_productos_proveedores "+
							     	                   "WHERE codigo_producto_proveedor = '"+(string) this.lista_precios_proveedor.Model.GetValue (iter,3)+"' "+
								    	               "AND id_proveedor = '"+this.entry_id_provedor.Text+"' ;";

								//Console.WriteLine(comando2.CommandText.ToString());
								NpgsqlDataReader lector2 = comando2.ExecuteReader ();
								
								if(lector2.Read()){			
									NpgsqlConnection conexion3; 
									conexion3 = new NpgsqlConnection (connectionString+nombrebd);
								
									try{
										conexion3.Open ();
										NpgsqlCommand comando3; 
										comando3 = conexion3.CreateCommand();
										comando3.CommandText =  "UPDATE osiris_catalogo_productos_proveedores SET eliminado = 'true',"+
						     			                        "fecha_eliminado = '"+DateTime.Now.ToString("yyyy-MM-dd")+"', "+
										                        "id_quien_elimino = ' "+LoginEmpleado+" ' "+
									                            "WHERE codigo_producto_proveedor = '"+(string) lista_precios_proveedor.Model.GetValue (iter,3)+"' "+
					             					            "AND id_proveedor = '"+this.entry_id_provedor.Text+"' ;";
										comando3.ExecuteNonQuery(); 
										comando3.Dispose();
										conexion3.Close();

										this.treeViewEngineaprobados.Remove (ref iter); 					

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
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						                                               MessageType.Error, 
						                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
					}
					conexion2.Close();		
				}
			}
						
		}
		
		void on_button_agrega_clicked(object sender, EventArgs args)	
		{
		
			this.button_guarda.Sensitive = true;
			if(entry_embalaje.Text == "" || entry_embalaje.Text == "0" || entry_codigo.Text == "" ||
			   entry_clave.Text == "" || entry_producto.Text == "" || entry_id_provedor.Text == "" ||
			   entry_provedor.Text == "" || this.entry_precio.Text == ""){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Info,ButtonsType.Close, "Favor de llenar toda la informacion");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}else{
				
				string embalaje = "";
				string precio = "";
				decimal precio_unitario = 0;
				
			
				precio = entry_precio.Text; 
				embalaje = entry_embalaje.Text;
				precio_unitario = Convert.ToDecimal(precio)/Convert.ToDecimal(embalaje);


				this.treeViewEngineproductos.AppendValues (false,
				                                           id_provedor,
				                                           entry_producto.Text.ToUpper(),
				                                           float.Parse(this.entry_precio.Text).ToString("F"),precio_unitario.ToString("F"),
				                                          tipounidadproducto,
				                                           this.entry_embalaje.Text,this.entry_clave.Text.ToUpper(),
				                                           this.entry_cod_barras.Text,
				                                           this.entry_codigo.Text.ToUpper());
				                                           
				                     			                      
				this.entry_producto.Text = "";
				entry_precio.Text = "0";
				entry_embalaje.Text = "0";
				entry_codigo.Text = "";
				entry_cod_barras.Text = "";
				this.entry_clave.Text = "";                                         
			}

		}
		
		void on_checkbutton_nuevo_producto(object sender, EventArgs args)		
		{
			this.entry_producto.Text = "";
			entry_precio.Text = "";
			entry_embalaje.Text = "";
			entry_codigo.Text = "";
			entry_cod_barras.Text = "";
			this.entry_clave.Text = "";
			this.entry_id_provedor.Text = "";
			this.entry_provedor.Text = "";
			this.treeViewEngineaprobados.Clear();
			this.treeViewEngineproductos.Clear();
			
			if(this.checkbutton_nuevo_producto.Active == true)
			{
				
				this.entry_id_provedor.Sensitive = true;
				this.entry_id_provedor.Sensitive = true;
				this.button_busca_provedor.Sensitive = true;
				this.button_guarda.Sensitive = false;
				entry_precio.Sensitive = true;
				entry_embalaje.Sensitive = true;
				entry_codigo.Sensitive = true;
				entry_cod_barras.Sensitive = true;
				this.entry_clave.Sensitive = true;
				this.button_aprobar.Sensitive = false;
				this.lista_productos_agregados.Sensitive = true;
				this.entry_producto.Sensitive = true;
				this.checkbutton_aprobar.Sensitive = false;
				this.button_agrega.Sensitive = true;
				this.button_quita.Sensitive = true;
				this.entry_id_provedor.Sensitive = true;
				this.entry_provedor.Sensitive = true;
				this.button_selecciona.Sensitive = true;
				this.button_editar.Sensitive = false;
				this.entry_embalaje.Text = "1";
				//this.entry_precio.Text = "1";
			}else{
				this.button_guarda.Sensitive = false;
				entry_precio.Sensitive = false;
				entry_embalaje.Sensitive = false;
				entry_codigo.Sensitive = false;
				entry_cod_barras.Sensitive = false;
				this.button_quita.Sensitive = false;
				this.entry_clave.Sensitive = false;
				this.button_aprobar.Sensitive = false;
				this.lista_productos_agregados.Sensitive = false;
				this.entry_producto.Sensitive = false;
				this.checkbutton_aprobar.Sensitive = true;
				this.button_agrega.Sensitive = false;
				this.entry_id_provedor.Sensitive = true;
				this.entry_provedor.Sensitive = true;
				this.entry_id_provedor.Sensitive = false;
				this.entry_provedor.Sensitive = false;
				this.button_busca_provedor.Sensitive = false;	
				this.button_selecciona.Sensitive = false;
				this.button_editar.Sensitive = true;
				this.entry_embalaje.Text = "";
			//	this.entry_precio.Text = "";
				
				
			}
		}
		void on_checkbutton_aprobar(object sender, EventArgs args)	
		{
			entry_id_provedor.Text = "";
			entry_provedor.Text = "";
			entry_producto.Text = "";
			entry_precio.Text = "";
			entry_embalaje.Text = "";
			entry_codigo.Text = "";
			entry_cod_barras.Text = "";
			entry_clave.Text = "";
			treeViewEngineaprobados.Clear();
			treeViewEngineproductos.Clear();
			
			if(this.checkbutton_aprobar.Active == true)
			{
				
				this.button_quita.Sensitive = true;
				this.button_selecciona.Sensitive = true;
				this.lista_precios_proveedor.Sensitive = true;
				this.button_guarda.Sensitive = true;
				this.button_aprobar.Sensitive = true;
				this.checkbutton_nuevo_producto.Sensitive = false;
				this.entry_id_provedor.Sensitive = true;
				this.entry_provedor.Sensitive = true;
				this.button_busca_provedor.Sensitive = true;
				this.button_editar.Sensitive = true;
				
			}else{
				this.button_quita.Sensitive = false;
				this.button_selecciona.Sensitive = false;
				this.lista_precios_proveedor.Sensitive = false;
				this.button_guarda.Sensitive = false;
				this.button_aprobar.Sensitive = false;
				this.checkbutton_nuevo_producto.Sensitive = true;
				this.entry_id_provedor.Sensitive = false;
				this.entry_provedor.Sensitive = false;
				this.button_busca_provedor.Sensitive = false;
				this.button_editar.Sensitive = false;
				
			}
		}
			
		void on_busca_provedor_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_proveedores);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			button_selecciona.Clicked += new EventHandler(on_selecciona_proveedor);
			//checkbutton_proveedor_nuevo.Active = false;
			crea_treeview_proveedores();
			button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
		}

		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (this.lista_precios_proveedor.Model.GetIter (out iter, path)) {
				bool old = (bool) lista_precios_proveedor.Model.GetValue (iter,0);
				lista_precios_proveedor.Model.SetValue(iter,0,!old);
			}	
		}
		
		void crea_lista_productos_agregados()
		{
			treeViewEngineproductos = new TreeStore(typeof(bool),
			                                        typeof(int),
													typeof(string),
													typeof(string),
			                                        typeof(string),
													typeof(string),
			                                        typeof(string),
													typeof(string),
			                                        typeof(string),
			                                        typeof(string),
													typeof(string));
													
												
			lista_productos_agregados.Model = treeViewEngineproductos;
			
			lista_productos_agregados.RulesHint = true;
			//lista_de_busqueda.RowActivated += on_selecciona_proveedor;  // Doble click selecciono paciente*/
			
			TreeViewColumn col_autorizar = new TreeViewColumn();
			CellRendererToggle cel_autorizar = new CellRendererToggle();
			col_autorizar.Title = "Autorizar"; // titulo de la cabecera de la columna, si está visible
			col_autorizar.PackStart(cel_autorizar, true);
			col_autorizar.AddAttribute (cel_autorizar, "active", 0);
			cel_autorizar.Activatable = true;
			//cel_autorizar.Toggled += selecciona_fila;
			//col_autorizar.SortColumnId = (int) Col_productos.col_autorizar;
			
			TreeViewColumn col_idproveedor = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idproveedor.Title = "ID Proveedor"; // titulo de la cabecera de la columna, si está visible
			col_idproveedor.PackStart(cellr0, true);
			col_idproveedor.AddAttribute (cellr0, "text", 1);    // la siguiente columna será 1
			//col_idproveedor.SortColumnId = (int) Col_productos.col_idproveedor;
			
			TreeViewColumn col_proveedor = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_proveedor.Title = "Descripcion";
			col_proveedor.PackStart(cellrt1, true);
			col_proveedor.AddAttribute (cellrt1, "text", 2); // la siguiente columna será 2
			col_proveedor.SortColumnId = (int) Col_productos.col_proveedor;
			
			TreeViewColumn col_precio = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_precio.Title = "Precio ";
			col_precio.PackStart(cellrt2, true);
			col_precio.AddAttribute (cellrt2, "text", 3); // la siguiente columna será 3
			col_precio.SortColumnId = (int) Col_productos.col_precio;
			
			TreeViewColumn col_unitario = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_unitario.Title = "Precio Unitario ";
			col_unitario.PackStart(cellrt3, true);
			col_unitario.AddAttribute (cellrt3, "text", 4); // la siguiente columna será 3
			col_unitario.SortColumnId = (int) Col_productos.col_unitario;
			
			TreeViewColumn col_tipo_unidad = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_tipo_unidad.Title = "Tipo Unidad";
			col_tipo_unidad.PackStart(cellrt4, true);
			col_tipo_unidad.AddAttribute (cellrt4, "text", 5); // la siguiente columna será 4
			col_tipo_unidad.SortColumnId = (int) Col_productos.col_tipo_unidad;
			
			TreeViewColumn col_embalaje = new TreeViewColumn();
            CellRendererText cellrt5 = new CellRendererText();
            col_embalaje.Title = "Embalaje";
            col_embalaje.PackStart(cellrt5, true);
			col_embalaje.AddAttribute(cellrt5,"text", 6); // la siguiente columna será 5
			col_embalaje.SortColumnId = (int) Col_productos.col_embalaje;
			
			TreeViewColumn col_clave = new TreeViewColumn();
            CellRendererText cellrt6 = new CellRendererText();
            col_clave.Title = "Clave";
            col_clave.PackStart(cellrt6, true);
			col_clave.AddAttribute(cellrt6,"text", 7); // la siguiente columna será 5
			col_clave.SortColumnId = (int) Col_productos.col_clave;
			
            TreeViewColumn col_codigo_barras = new TreeViewColumn();
            CellRendererText cellrt7 = new CellRendererText();
            col_codigo_barras.Title = "CodigoBarras";
            col_codigo_barras.PackStart(cellrt7, true);
            col_codigo_barras.AddAttribute(cellrt7,"text", 8); // la siguiente columna será 6
			col_codigo_barras.SortColumnId = (int) Col_productos.col_codigo_barras;
			
            TreeViewColumn col_codigo_provedor = new TreeViewColumn();
            CellRendererText cellrt8 = new CellRendererText();
            col_codigo_provedor.Title = "Codigo-Provedor";
            col_codigo_provedor.PackStart(cellrt8, true);
            col_codigo_provedor.AddAttribute(cellrt8,"text", 9); // la siguiente columna será 7
            col_codigo_provedor.SortColumnId = (int) Col_productos.col_codigo_provedor;
            
			//lista_productos_agregados.AppendColumn(col_autorizar);
			//lista_productos_agregados.AppendColumn(col_idproveedor);
			lista_productos_agregados.AppendColumn(col_proveedor);
			lista_productos_agregados.AppendColumn(col_precio);
			lista_productos_agregados.AppendColumn(col_unitario);
			lista_productos_agregados.AppendColumn(col_tipo_unidad);
			lista_productos_agregados.AppendColumn(col_embalaje);
			lista_productos_agregados.AppendColumn(col_clave);
			lista_productos_agregados.AppendColumn(col_codigo_barras);
			lista_productos_agregados.AppendColumn(col_codigo_provedor);
				
		}
		
		enum Col_productos
		{			
			col_proveedor,
			col_precio,
		    col_unitario,
			col_tipo_unidad,
			col_embalaje,
		    col_clave,
			col_codigo_barras,
			col_codigo_provedor
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
			
			lista_de_busqueda.RowActivated += on_selecciona_proveedor;  // Doble click selecciono paciente*/
			
			
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
	
		
		void crea_treeview_autoriza()
		{
			this.treeViewEngineaprobados = new TreeStore(typeof(bool),
													     typeof(string),
													     typeof(string),
			                                             typeof(string),
			                                             typeof(string),
			                                             typeof(string),
			                                             typeof(string),
			                                             typeof(string),
			                                             typeof(string),
			                                             typeof(string));
													
												
			this.lista_precios_proveedor.Model = this.treeViewEngineaprobados;
			
			this.lista_precios_proveedor.RulesHint = true;
				
			lista_precios_proveedor.RowActivated += on_button_aprobar_clicked;  // Doble click selecciono paciente*/
			
			TreeViewColumn col_autorizar = new TreeViewColumn();
			CellRendererToggle cel_autorizar = new CellRendererToggle();
			col_autorizar.Title = "Autorizar"; // titulo de la cabecera de la columna, si está visible
			col_autorizar.PackStart(cel_autorizar, true);
			col_autorizar.AddAttribute (cel_autorizar, "active", 0);
			cel_autorizar.Activatable = true;
			cel_autorizar.Toggled += selecciona_fila;
			col_autorizar.SortColumnId = (int) Col_autoriza.col_autorizar;
			//col_autorizar.SetCellDataFunc(cel_autorizar, new Gtk.TreeCellDataFunc(cambia_colores));
			
			TreeViewColumn col_descripcion = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_descripcion.Title = "Descripcion Proveedor"; // titulo de la cabecera de la columna, si está visible
			col_descripcion.PackStart(cellr0, true);
			col_descripcion.AddAttribute (cellr0, "text", 1);    // la siguiente columna será 1
			col_descripcion.SortColumnId = (int) Col_autoriza.col_descripcion;
			//col_descripcion.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores));
			
			TreeViewColumn col_precio_prov = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_precio_prov.Title = "Precio Proveedor";
			col_precio_prov.PackStart(cellrt1, true);
			col_precio_prov.AddAttribute (cellrt1, "text", 2); // la siguiente columna será 2
			col_precio_prov.SortColumnId = (int) Col_autoriza.col_precio_prov;
			//col_precio_prov.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores));
			
			TreeViewColumn col_precio_xuni_prov = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_precio_xuni_prov.Title = "Precio X Unidad";
			col_precio_xuni_prov.PackStart(cellrt2, true);
			col_precio_xuni_prov.AddAttribute (cellrt2, "text", 3); // la siguiente columna será 2
			//col_precio_xuni_prov.SortColumnId = (int) Col_autoriza.col_precio_xuni_prov;
			//col_precio_xuni_prov.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores));
			
			TreeViewColumn col_cod_provedor = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_cod_provedor.Title = "Cod.Provedor ";
			col_cod_provedor.PackStart(cellrt3, true);
			col_cod_provedor.AddAttribute (cellrt3, "text", 4); // la siguiente columna será 3
			//col_cod_provedor.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores));
			col_cod_provedor.SortColumnId = (int) Col_autoriza.col_cod_provedor;
			
			TreeViewColumn col_precio_hscmty = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_precio_hscmty.Title = "Precio HSCMTY ";
			col_precio_hscmty.PackStart(cellrt4, true);
			col_precio_hscmty.AddAttribute (cellrt4, "text", 5); // la siguiente columna será 3
			//col_precio_hscmty.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores));
			col_precio_hscmty.SortColumnId = (int) Col_autoriza.col_precio_hscmty;
			
			TreeViewColumn col_precio_xuni_hscmty = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_precio_xuni_hscmty.Title = "Precio X Unidad ";
			col_precio_xuni_hscmty.PackStart(cellrt5, true);
			col_precio_xuni_hscmty.AddAttribute (cellrt5, "text", 6); // la siguiente columna será 3
			//col_precio_xuni_hscmty.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores));
			//col_precio_xuni_hscmty.SortColumnId = (int) Col_autoriza.col_precio_xuni_hscmty;
			
			TreeViewColumn col_cod_hscmty = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_cod_hscmty.Title = "Cod.HSCMTY ";
			col_cod_hscmty.PackStart(cellrt6, true);
			col_cod_hscmty.AddAttribute (cellrt6, "text", 7); // la siguiente columna será 3
			//col_cod_hscmty.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores));
			col_cod_hscmty.SortColumnId = (int) Col_autoriza.col_cod_hscmty;
			
			TreeViewColumn col_descripcion_hsc = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_descripcion_hsc.Title = "DescripcionHSCMTY";
			col_descripcion_hsc.PackStart(cellrt7, true);
			col_descripcion_hsc.AddAttribute (cellrt7, "text", 8); // la siguiente columna será 3
			col_descripcion_hsc.SortColumnId = (int) Col_autoriza.col_descripcion_hsc;
			//col_descripcion_hsc.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores));
			
			lista_precios_proveedor.AppendColumn(col_autorizar);			//0
			lista_precios_proveedor.AppendColumn(col_descripcion);			//1
			lista_precios_proveedor.AppendColumn(col_precio_prov);			//2
			lista_precios_proveedor.AppendColumn(col_precio_xuni_prov);		//3
			lista_precios_proveedor.AppendColumn(col_cod_provedor);			//4
			lista_precios_proveedor.AppendColumn(col_precio_hscmty);		//5
			lista_precios_proveedor.AppendColumn(col_precio_xuni_hscmty);	//6
			lista_precios_proveedor.AppendColumn(col_cod_hscmty);			//7
			lista_precios_proveedor.AppendColumn(col_descripcion_hsc);		//8
		}
		
		enum Col_autoriza
		{
			col_autorizar,
		    col_descripcion,
	     	col_precio_prov,
			col_cod_provedor,
			col_precio_hscmty,
		    col_cod_hscmty,
		    col_descripcion_hsc			
		}		
		
		void on_button_selecciona_clicked(object sender, EventArgs args)
		{
			if ((string) this.entry_id_provedor.Text == ""){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						                                   MessageType.Info,ButtonsType.Close, "Debe seleccionar un Proveedor... verifique...");
				msgBoxError.Run ();			msgBoxError.Destroy();			
			}else{			
				if(this.checkbutton_nuevo_producto.Active == true){
					//this.entry_id_provedor.Sensitive = false;
					this.entry_provedor.Sensitive = false;
				}
				
				llenando_lista_de_aprobados();
				
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
				
					comando.CommandText = "SELECT descripcion_proveedor,direccion_proveedor,rfc_proveedor,curp_proveedor, "+
								"colonia_proveedor,municipio_proveedor,estado_proveedor,telefono1_proveedor, "+ 
								"telefono2_proveedor,celular_proveedor,cp_proveedor, proveedor_activo, "+
								"id_proveedor,contacto1_proveedor,mail_proveedor,pagina_web_proveedor, "+
								"osiris_erp_proveedores.id_forma_de_pago, descripcion_forma_de_pago AS descripago "+
								"FROM osiris_erp_proveedores, osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_proveedores.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
							    "AND id_proveedor = '"+(string) this.entry_id_provedor.Text+"' "+
								"ORDER BY descripcion_proveedor;";
				
					NpgsqlDataReader lector = comando.ExecuteReader ();
					if (lector.Read()){	
						this.entry_provedor.Text = (string) lector["descripcion_proveedor"];//12
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					                                               MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
				conexion.Close ();
			}		
		}
			
		void llenando_lista_de_aprobados()
		{
			this.treeViewEngineaprobados.Clear();
			
			id_provedor = Convert.ToInt16(this.entry_id_provedor.Text);
			
		
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT osiris_catalogo_productos_proveedores.descripcion_producto,"+
					                "to_char(osiris_catalogo_productos_proveedores.precio_costo, '999999999.99') AS preciocosto,"+
					                "to_char(osiris_catalogo_productos_proveedores.precio_costo_unitario, '999999999.99') AS preciocostouni,"+
						            "to_char(osiris_catalogo_productos_proveedores.id_producto, '999999999999999') AS idproducto,"+
               						"osiris_catalogo_productos_proveedores.codigo_producto_proveedor,osiris_catalogo_productos_proveedores.descripcion_producto_hsc,"+
               						"to_char(osiris_catalogo_productos_proveedores.id_secuencia,'9999999999') AS idsecuencia "+
               						//"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario,"+
               						//"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto "+
               						"FROM osiris_catalogo_productos_proveedores "+
               						"WHERE osiris_catalogo_productos_proveedores.id_proveedor = '"+this.id_provedor.ToString().Trim()+"' " + 
               						"AND osiris_catalogo_productos_proveedores.id_producto = 0 " + 
						            "AND osiris_catalogo_productos_proveedores.eliminado = 'false';";   
				//Console.WriteLine(comando.CommandText);				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){					
					treeViewEngineaprobados.AppendValues(false,
					                                     (string) lector["descripcion_producto"],
					                                     (string) lector["preciocosto"],
					                                     (string) lector["preciocostouni"],
					                                     (string) lector["codigo_producto_proveedor"],"","",
					                                    // (string) lector["costoproducto"],
					                                     //(string) lector["costoproductounitario"],
					                                     (string) lector["idproducto"],
					                                     (string) lector["descripcion_producto_hsc"],
					                                     (string) lector["idsecuencia"]);
													
				}	
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			
			conexion.Close ();				
			NpgsqlConnection conexion1; 
			conexion1 = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion1.Open ();
				NpgsqlCommand comando1; 
				comando1 = conexion1.CreateCommand ();
               	comando1.CommandText = "SELECT osiris_catalogo_productos_proveedores.descripcion_producto,"+
					                "to_char(osiris_catalogo_productos_proveedores.precio_costo, '999999999.99') AS preciocosto,"+
					                "to_char(osiris_catalogo_productos_proveedores.precio_costo_unitario, '999999999.99') AS preciocostouni,"+
						            "to_char(osiris_catalogo_productos_proveedores.id_producto, '999999999999999') AS idproducto,"+
               						"osiris_catalogo_productos_proveedores.codigo_producto_proveedor,osiris_catalogo_productos_proveedores.descripcion_producto_hsc,"+
               						"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario,"+
               						"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto,"+
               						"to_char(osiris_catalogo_productos_proveedores.id_secuencia,'9999999999') AS idsecuencia "+
               						"FROM osiris_catalogo_productos_proveedores, osiris_productos "+
               						"WHERE osiris_catalogo_productos_proveedores.id_proveedor = '"+this.id_provedor.ToString().Trim()+"' " + 
               						"AND osiris_catalogo_productos_proveedores.id_producto = osiris_productos.id_producto " + 
						            "AND osiris_catalogo_productos_proveedores.eliminado = 'false';";   
				//Console.WriteLine(comando1.CommandText);				
				NpgsqlDataReader lector1 = comando1.ExecuteReader ();
				while(lector1.Read()){
					treeViewEngineaprobados.AppendValues(false,
					                                     (string) lector1["descripcion_producto"],
					                                     (string) lector1["preciocosto"],
					                                     (string) lector1["preciocostouni"],
					                                     (string) lector1["codigo_producto_proveedor"],
					                                     (string) lector1["costoproducto"],
					                                     (string) lector1["costoproductounitario"],
					                                     (string) lector1["idproducto"],
					                                     (string) lector1["descripcion_producto_hsc"],
					                                     (string) lector1["idsecuencia"]);
													
				}										
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion1.Close ();		
		}
		
		void cambia_colores_proveedor(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//if ((bool) lista_de_busqueda.Model.GetValue(iter,10) == false)
			//{(cell as Gtk.CellRendererText).Foreground = "darkgreen";		}
		}
		
		void cambia_colores(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if (Convert.ToInt64((string) this.lista_precios_proveedor.Model.GetValue(iter,7))==0)
			{				
				(cell as Gtk.CellRendererText).Foreground = "red";
			}else{		
				(cell as Gtk.CellRendererText).Foreground = "black";		
			}
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
		
		void on_selecciona_proveedor(object sender, EventArgs args)
		{
			if(this.checkbutton_nuevo_producto.Active == true){
				this.entry_id_provedor.Sensitive = false;
				this.entry_provedor.Sensitive = false;
			}
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_busqueda.Selection.GetSelected(out model, out iterSelected)){				
				this.entry_provedor.Text = (string) model.GetValue(iterSelected, 1); 
				id_provedor = (int) model.GetValue(iterSelected, 0); 
				this.entry_id_provedor.Text = Convert.ToString(id_provedor);
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
				
				if(this.checkbutton_aprobar.Active == true && checkbutton_nuevo_producto.Active == false){
					llenando_lista_de_aprobados();
				}
				
				if(this.checkbutton_nuevo_producto.Active == true && this.checkbutton_aprobar.Active == false){
					this.entry_provedor.Text = (string) model.GetValue(iterSelected, 1); 
					id_provedor = (int) model.GetValue(iterSelected, 0); 
					this.entry_id_provedor.Text = Convert.ToString(id_provedor);
					llenando_lista_de_aprobados();
				}								
			}
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
		
		///////////////////////////////////////BOTON general de busqueda por enter///////////////////////////////////////////////		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione  
		public void onKeyPressEvent_enter_entry_expresion(object o, Gtk.KeyPressEventArgs args)
		{                                                                                     
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;	
				llenando_lista_de_productos();
			}
		}
		
	void on_button_guarda_clicked (object sender, EventArgs args)
		{
			
			if(edita == true){
				
					NpgsqlConnection conexion3; 
					conexion3 = new NpgsqlConnection (connectionString+nombrebd);
					try{
						conexion3.Open ();
						NpgsqlCommand comando3; 
						comando3 = conexion3.CreateCommand();
						comando3.CommandText =  "UPDATE osiris_catalogo_productos_proveedores SET clave = '"+this.entry_clave.Text+"', "+
											    "descripcion_producto = '"+this.entry_producto.Text+"', "+
											    "precio_costo = '"+this.entry_precio.Text+"', "+
											    "cantidad_de_embalaje = '"+this.entry_embalaje.Text+"', "+
											    "codigo_de_barra = '"+this.entry_cod_barras.Text+"', "+
							                    "precio_costo_unitario = '"+Convert.ToString(float.Parse(this.entry_precio.Text)/float.Parse(this.entry_embalaje.Text))+"', "+
	                                            "tipo_unidad_producto = '"+tipounidadproducto.ToString().ToUpper()+"', "+
	                                            "codigo_producto_proveedor 	 = '"+this.entry_codigo.Text.ToUpper()+"', "+
												"historial_movimientos = historial_movimientos || '"+this.entry_precio.Text.Trim()+";"+this.entry_embalaje.Text.Trim()+";"+LoginEmpleado+";"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n' "+
										/*	    "fecha_asigno_hscmty = '"+DateTime.Now.ToString("yyyy-MM-dd")+"', "+
								                "id_quien_asigno_hscmty = ' "+LoginEmpleado+" ' "+*/
							                    "WHERE id_secuencia = '"+this.secuencial+"' "+
							                    "AND codigo_producto_proveedor = '"+this.entry_codigo.Text+"' ;"; 
							                    Console.WriteLine("este"+comando3.CommandText.ToString());

						comando3.ExecuteNonQuery();
						comando3.Dispose();
						conexion3.Close();
										
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						                                               MessageType.Error, 
						                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
					}
					conexion3.Close();				

				}
				
			TreeIter iterSelected;
			//TreeModel model;
			TreeIter iter;
			
			MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro que desea Guardar ?");
			ResponseType miResultado = (ResponseType)msgBox1.Run ();
			msgBox1.Destroy();
		 	if (miResultado == ResponseType.Yes){
			
				if(this.checkbutton_nuevo_producto.Active == false && this.checkbutton_aprobar.Active == true)
				{
					NpgsqlConnection conexion2; 
					conexion2 = new NpgsqlConnection (connectionString+nombrebd);
					
					try{
						conexion2.Open ();
						NpgsqlCommand comando2; 
						comando2 = conexion2.CreateCommand();
						
						if (this.treeViewEngineaprobados.GetIterFirst(out iterSelected)){
							if ((bool)lista_precios_proveedor.Model.GetValue (iterSelected,0) == true){		
								
								comando2.CommandText = "SELECT id_proveedor,codigo_producto_proveedor,codigo_de_barra FROM osiris_catalogo_productos_proveedores "+
												"WHERE codigo_producto_proveedor = '"+(string) this.lista_precios_proveedor.Model.GetValue (iterSelected,3)+"' "+
												"AND id_proveedor = '"+this.entry_id_provedor.Text+"' ;";
								//Console.WriteLine("GUARDA     "+comando2.CommandText.ToString());
								NpgsqlDataReader lector2 = comando2.ExecuteReader ();
								
								if(lector2.Read()){			
									NpgsqlConnection conexion3; 
									conexion3 = new NpgsqlConnection (connectionString+nombrebd);
									try{
										conexion3.Open ();
										NpgsqlCommand comando3; 
										comando3 = conexion3.CreateCommand();
										comando3.CommandText =  "UPDATE osiris_catalogo_productos_proveedores SET id_producto  = '"+(string) this.lista_precios_proveedor.Model.GetValue (iterSelected,4)+"', "+
											                    "descripcion_producto_hsc = '"+(string) this.lista_precios_proveedor.Model.GetValue (iterSelected,5)+"', "+											                   
											                    "fecha_asigno_hscmty = '"+DateTime.Now.ToString("yyyy-MM-dd")+"', "+
												                "id_quien_asigno_hscmty = ' "+LoginEmpleado+" ' "+
											                    "WHERE codigo_producto_proveedor = '"+(string) lista_precios_proveedor.Model.GetValue (iterSelected,3)+"' "+
							             					    "AND id_proveedor = '"+this.entry_id_provedor.Text+"' ;";
										comando3.ExecuteNonQuery();
										comando3.Dispose();
										conexion3.Close();
										
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
						
						while (treeViewEngineaprobados.IterNext(ref iterSelected))
						{							
							if ((bool)lista_precios_proveedor.Model.GetValue (iterSelected,0) == true){
								comando2.CommandText = "SELECT id_proveedor,codigo_producto_proveedor,codigo_de_barra FROM osiris_catalogo_productos_proveedores "+
								                       "WHERE codigo_producto_proveedor = '"+(string) this.lista_precios_proveedor.Model.GetValue (iterSelected,3)+"' "+
										               "AND id_proveedor = '"+this.entry_id_provedor.Text+"' ;";
								//Console.WriteLine(comando2.CommandText.ToString());
								NpgsqlDataReader lector2 = comando2.ExecuteReader ();
								
								if(lector2.Read()){			
									NpgsqlConnection conexion3; 
									conexion3 = new NpgsqlConnection (connectionString+nombrebd);
								
									try{
										conexion3.Open ();
										NpgsqlCommand comando3; 
										comando3 = conexion3.CreateCommand();
										comando3.CommandText =  "UPDATE osiris_catalogo_productos_proveedores SET id_producto  = '"+(string) this.lista_precios_proveedor.Model.GetValue (iterSelected,4)+"', "+
											                    "descripcion_producto_hsc = '"+(string) this.lista_precios_proveedor.Model.GetValue (iterSelected,5)+"', "+											                   
											                    "fecha_asigno_hscmty = '"+DateTime.Now.ToString("yyyy-MM-dd")+"', "+
												                "id_quien_asigno_hscmty = ' "+LoginEmpleado+" ' "+
											                    "WHERE codigo_producto_proveedor = '"+(string) lista_precios_proveedor.Model.GetValue (iterSelected,3)+"' "+
							             					    "AND id_proveedor = '"+this.entry_id_provedor.Text+"' ;";
										comando3.ExecuteNonQuery();
										comando3.Dispose();
										conexion3.Close();
									
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
						
						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						                                          MessageType.Info,ButtonsType.Ok,"Los codigos del HSCMTY se Actualizaron ");
						msgBox.Run ();
						msgBox.Destroy();
						
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						                                               MessageType.Error, 
						                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
					}
					conexion2.Close();
				
				}
				
				if(this.checkbutton_nuevo_producto.Active == true && this.checkbutton_aprobar.Active == false)
				{ 			
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
				  
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand();
						if (this.treeViewEngineproductos.GetIterFirst (out iterSelected)){ 
							comando.CommandText = "INSERT INTO osiris_catalogo_productos_proveedores("+
								            "fechahora_creacion,"+
									        "id_quien_creo,"+
								            "id_proveedor,"+
											"codigo_de_barra,"+ 
											"codigo_producto_proveedor,"+
											"cantidad_de_embalaje,"+
											"tipo_unidad_producto,"+
									        "precio_costo_unitario,"+
								            "precio_costo,"+
								            "clave,"+
											"descripcion_producto) "+
											"VALUES ('"+
									        DateTime.Now.ToString("yyyy-MM-dd")+"','"+
									        LoginEmpleado+"','"+
											this.entry_id_provedor.Text+"','"+//(int)this.lista_productos_agregados.Model.GetValue (iterSelected,1)+"','"+
											(string)this.lista_productos_agregados.Model.GetValue (iterSelected,8)+"','"+
											(string)this.lista_productos_agregados.Model.GetValue (iterSelected,9)+"','"+
									        (string)this.lista_productos_agregados.Model.GetValue (iterSelected,6)+"','"+
											(string)this.lista_productos_agregados.Model.GetValue (iterSelected,5)+"','"+
											(string)this.lista_productos_agregados.Model.GetValue (iterSelected,4)+"','"+
											(string)this.lista_productos_agregados.Model.GetValue (iterSelected,3)+"','"+
											(string)this.lista_productos_agregados.Model.GetValue (iterSelected,7)+"','"+
											(string)this.lista_productos_agregados.Model.GetValue (iterSelected,2)+"');";
											Console.WriteLine(comando.CommandText);
							comando.ExecuteNonQuery();
							comando.Dispose();	
						
							while (treeViewEngineproductos.IterNext(ref iterSelected)){
								comando.CommandText = "INSERT INTO osiris_catalogo_productos_proveedores("+
									            "fechahora_creacion,"+
									            "id_quien_creo,"+
											    "id_proveedor,"+
							            		"codigo_de_barra,"+ 
						                        "codigo_producto_proveedor,"+
						                        "cantidad_de_embalaje,"+
						                        "tipo_unidad_producto,"+
						                        "precio_costo_unitario,"+
									            "precio_costo,"+
									            "clave,"+
											    "descripcion_producto) "+
											    "VALUES ('"+
										        DateTime.Now.ToString("yyyy-MM-dd")+"','"+
									            LoginEmpleado+"','"+
										        this.entry_id_provedor.Text+"','"+//(int)this.lista_productos_agregados.Model.GetValue (iterSelected,1)+"','"+
											    (string)this.lista_productos_agregados.Model.GetValue (iterSelected,8)+"','"+
									            (string)this.lista_productos_agregados.Model.GetValue (iterSelected,9)+"','"+
											    (string)this.lista_productos_agregados.Model.GetValue (iterSelected,6)+"','"+
											    (string)this.lista_productos_agregados.Model.GetValue (iterSelected,5)+"','"+
											    (string)this.lista_productos_agregados.Model.GetValue (iterSelected,4)+"','"+
											    (string)this.lista_productos_agregados.Model.GetValue (iterSelected,3)+"','"+
												(string)this.lista_productos_agregados.Model.GetValue (iterSelected,7)+"','"+
												(string)this.lista_productos_agregados.Model.GetValue (iterSelected,2)+"');";
								comando.ExecuteNonQuery();
								comando.Dispose();		
							}
						}
						
						//Console.WriteLine(comando.CommandText.ToString());
						//Console.WriteLine("INSERT en catagologo");

						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						                                               MessageType.Info,ButtonsType.Close, " Los Productos se Grabaron Correctamente ");
						msgBoxError.Run ();			msgBoxError.Destroy();
						
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						                                               MessageType.Error, 
					                                                   ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
					}
					conexion.Close();		
				}
			}
			///  Limpiando variables despuez de guardar  ///
			if(edita == true){
				llenando_lista_de_aprobados();
				entry_producto.Text = "";
				entry_precio.Text = "";
				entry_embalaje.Text = "";
				entry_codigo.Text = "";
				entry_cod_barras.Text = "";
				entry_clave.Text = "";
			}else{
				entry_id_provedor.Text = "";
				entry_provedor.Text = "";
				entry_producto.Text = "";
				entry_precio.Text = "";
				entry_embalaje.Text = "";
				entry_codigo.Text = "";
				entry_cod_barras.Text = "";
				entry_clave.Text = "";
				//treeViewEngineaprobados.Clear();
				treeViewEngineproductos.Clear();
				
			}
		}
		
		void on_editar_producto_clicked (object sender, EventArgs args)
		{
			this.button_aprobar.Sensitive = false;
			edita = true;
			this.entry_codigo.Sensitive = false;
			this.entry_producto.Sensitive = true;
			entry_precio.Sensitive = true;
			entry_embalaje.Sensitive = true;
			entry_codigo.Sensitive = false;
			entry_cod_barras.Sensitive = true;
			this.entry_clave.Sensitive = true;
			this.entry_id_provedor.Sensitive = false;
			this.entry_provedor.Sensitive = false;
			
			TreeModel model;
			TreeIter iter;
			string codigo = "";
			if (this.lista_precios_proveedor.Selection.GetSelected(out model, out iter)){
				codigo = (string) model.GetValue(iter, 4);
				//jbuConsole.WriteLine(codigo);
			}
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT osiris_catalogo_productos_proveedores.id_proveedor,"+
					                "osiris_catalogo_productos_proveedores.descripcion_producto,"+
						            "to_char(osiris_catalogo_productos_proveedores.precio_costo, '999999.999') AS preciocosto,"+
							        "to_char(osiris_catalogo_productos_proveedores.id_secuencia, '999999') AS secuencia,"+
						            "to_char(osiris_catalogo_productos_proveedores.cantidad_de_embalaje, '99999') AS cantidadembalaje,"+
						            "osiris_catalogo_productos_proveedores.clave,"+
						            "osiris_catalogo_productos_proveedores.codigo_producto_proveedor,"+
						            "osiris_catalogo_productos_proveedores.codigo_de_barra,"+
						
               						"osiris_catalogo_productos_proveedores.tipo_unidad_producto "+
               						"FROM osiris_catalogo_productos_proveedores "+
               						"WHERE codigo_producto_proveedor = '"+codigo+"' "+
               						"AND id_proveedor  = '"+this.entry_id_provedor.Text+"' ;"; 
               						
				//Console.WriteLine(comando.CommandText);
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if(lector.Read()){	
										
					this.secuencial = (string) lector["secuencia"];			
					this.entry_producto.Text = (string) lector["descripcion_producto"].ToString().ToUpper();
					this.entry_precio.Text = (string) lector["preciocosto"];
					this.entry_embalaje.Text = (string) lector["cantidadembalaje"];
					this.entry_codigo.Text = (string) lector["codigo_producto_proveedor"].ToString().ToUpper();
					this.entry_cod_barras.Text = (string) lector["codigo_de_barra"];		
					this.entry_clave.Text = (string) lector["clave"].ToString().ToUpper();	
					//this.combobox_tipo_unidad = (string) lector["tipo_unidad_producto"];	
					
					llena_combo_tipounidad();
					//relleno de tipo de empleado
	 				combobox_tipo_unidad.Clear();
	 				CellRendererText cell33 = new CellRendererText();
					combobox_tipo_unidad.PackStart(cell33, true);
					combobox_tipo_unidad.AddAttribute(cell33,"text",0);
	        
					ListStore store33 = new ListStore( typeof (string));
					combobox_tipo_unidad.Model = store33;
	        		
					store33.AppendValues ((string) lector["tipo_unidad_producto"]);
					store33.AppendValues ("PIEZA");
					store33.AppendValues ("KILO");
					store33.AppendValues ("LITRO");
					store33.AppendValues ("GRAMO");
					store33.AppendValues ("METRO");
					store33.AppendValues ("CENTIMETRO");
					store33.AppendValues ("CAJA");
					store33.AppendValues ("PULGADA");
					store33.AppendValues ("FRASCO");
	 				
					TreeIter iter33;
					if (store33.GetIterFirst(out iter33)){
						combobox_tipo_unidad.SetActiveIter (iter33);
					}
				}				
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion.Close ();			
		}
		
		void on_button_aprobar_clicked (object sender, EventArgs args)
		{

			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda();
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			button_selecciona.Label = "Seleccionar";

			this.label_cantidad.Hide();
			this.entry_cantidad_aplicada.Hide();

			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter_entry_expresion;
			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // 
		}
		
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
				
				col_stock_actual = new TreeViewColumn();
				cellrt7 = new CellRendererText();
				col_stock_actual.Title = "Stock Almacen";
				col_stock_actual.PackStart(cellrt7, true);
				col_stock_actual.AddAttribute (cellrt7, "text", 7);     // la siguiente columna será 6 en vez de 7
				col_stock_actual.SortColumnId = (int) Column_prod.col_stock_actual;
            	            	
				col_grupoprod = new TreeViewColumn();
				cellrt8 = new CellRendererText();
				col_grupoprod.Title = "Grupo Producto";
				col_grupoprod.PackStart(cellrt8, true);
				col_grupoprod.AddAttribute (cellrt8, "text", 8); // la siguiente columna será 7 en vez de 8
				col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
            
				col_grupo1prod = new TreeViewColumn();
				cellrt9 = new CellRendererText();
				col_grupo1prod.Title = "Grupo1 Producto";
				col_grupo1prod.PackStart(cellrt9, true);
				col_grupo1prod.AddAttribute (cellrt9, "text", 9); // la siguiente columna será 9 en vez de 
				col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
                        
				col_grupo2prod = new TreeViewColumn();
				cellrt10 = new CellRendererText();
				col_grupo2prod.Title = "Grupo2 Producto";
				col_grupo2prod.PackStart(cellrt10, true);
				col_grupo2prod.AddAttribute (cellrt10, "text", 10); // la siguiente columna será 10 en vez de 9
				col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;
								
				lista_de_producto.AppendColumn(col_idproducto);  // 0
				lista_de_producto.AppendColumn(col_desc_producto); // 1
				//lista_de_producto.AppendColumn(col_precioprod);	//2
				//lista_de_producto.AppendColumn(col_ivaprod);	// 3
				//lista_de_producto.AppendColumn(col_totalprod); // 4
				//lista_de_producto.AppendColumn(col_descuentoprod); //5
				//lista_de_producto.AppendColumn(col_preciocondesc); // 6
				//lista_de_producto.AppendColumn(col_stock_actual); // 7
				lista_de_producto.AppendColumn(col_grupoprod);	//8
				lista_de_producto.AppendColumn(col_grupo1prod);	//9
				lista_de_producto.AppendColumn(col_grupo2prod);	//10
			
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
			col_costo_prod,			col_stock_actual,
			col_cant_embalaje,
			col_id_gpo_prod,		col_id_gpo_prod1,
			col_id_gpo_prod2,		col_aplica_iva,
			col_cobro_activo,		col_aplica_desc
		}
		
			[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_valida(object o, Gtk.KeyPressEventArgs args)
		{		
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace){
				args.RetVal = true;
			}
		}	
	
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
							"AND osiris_productos.cobro_activo = 'true' "+
							query_tipo_busqueda;
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
							
				while (lector.Read()){
			
					 
					treeViewEngineBusca2.AppendValues (
									(string) lector["codProducto"] ,//0
									(string) lector["nombre_articulo"],
									(string) lector["preciopublico"],
									"",
									"",
									(string) lector["porcentagesdesc"],
									"",
									(string) lector["tipo_unidad_producto"],
									(string) lector["descripcion_grupo_producto"],
									(string) lector["descripcion_grupo1_producto"],
									(string) lector["descripcion_grupo2_producto"],
									(string) lector["nombre_articulo"],
									(string) lector["nombre_articulo"],
									(string) lector["costoproductounitario"],//13
									(string) lector["porcentageutilidad"],
									(string) lector["costoproducto"],//15
									(string) lector["cantidadembalaje"],
									(string) lector["idgrupoproducto"],
									(string) lector["idgrupo1producto"],
									(string) lector["idgrupo2producto"],
									(bool) lector["aplicar_iva"],
									(bool) lector["cobro_activo"],
									(bool) lector["aplica_descuento"],
									(string) lector["preciopublico1"]);
					
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
	
/////	
	void on_selecciona_producto_clicked (object sender, EventArgs args)
		{

			TreeModel model;
			TreeIter iter;
			
			TreeModel model1;
			TreeIter iter1;
			
			if (this.lista_de_producto.Selection.GetSelected(out model, out iter)){
				if (this.lista_precios_proveedor.Selection.GetSelected(out model1, out iter1)){
				
					this.lista_precios_proveedor.Model.SetValue(iter1,7,(string) model.GetValue(iter, 0));
					this.lista_precios_proveedor.Model.SetValue(iter1,0,true);
					this.lista_precios_proveedor.Model.SetValue(iter1,8,(string) model.GetValue(iter, 1));
					this.lista_precios_proveedor.Model.SetValue(iter1,6,(string) model.GetValue(iter, 13));				
					this.lista_precios_proveedor.Model.SetValue(iter1,5,(string) model.GetValue(iter, 15));
				}
			}
			
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();			
 		}
	
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}	
	}
}
