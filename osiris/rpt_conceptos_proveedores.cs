//////////////////////////////////////////////////////////////////////
// created on 19/04/2012 at 02:41 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. (Modificaciones y Ajustes)
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
	public class rpt_conceptos_proveedores
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando la ventana
		[Widget] Gtk.Window reporte_proveedores = null;
		[Widget] Gtk.Entry entry_id_proveedor = null;
		[Widget] Gtk.Entry entry_dia_inicial = null;
		[Widget] Gtk.Entry entry_mes_inicial = null;
		[Widget] Gtk.Entry entry_ano_inicial = null;
		[Widget] Gtk.Entry entry_dia_final = null;
		[Widget] Gtk.Entry entry_mes_final = null;
		[Widget] Gtk.Entry entry_ano_final = null;
		[Widget] Gtk.CheckButton checkbutton_todos_proveedores = null;
		[Widget] Gtk.CheckButton checkbutton_todas_fechas = null;
		[Widget] Gtk.Entry entry_nombre_proveedor = null;
		[Widget] Gtk.Button button_busca_proveedor = null;
		[Widget] Gtk.TreeView treeview_lista_grupoprod = null;
		[Widget] Gtk.Button button_exportar_sheet = null;
		
		string connectionString = "";
		string nombrebd = "";
		string query_rango_fechas = "AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+DateTime.Now.ToString("yyyy")+"-"+DateTime.Now.ToString("MM")+"-"+DateTime.Now.ToString("dd")+"' "+
										"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+DateTime.Now.ToString("yyyy")+"-"+DateTime.Now.ToString("MM")+"-"+DateTime.Now.ToString("dd")+"' "; 
		string query_proveedores = "";
		
		TreeStore treeViewEngineListaGrupoProd;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();
		
		public rpt_conceptos_proveedores ()
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "impr_documentos.glade", "reporte_proveedores", null);
			gxml.Autoconnect(this);
			reporte_proveedores.Show();
			entry_dia_inicial.Text = DateTime.Now.ToString("dd");
			entry_mes_inicial.Text = DateTime.Now.ToString("MM");
			entry_ano_inicial.Text = DateTime.Now.ToString("yyyy");			
			entry_dia_final.Text = DateTime.Now.ToString("dd");
			entry_mes_final.Text = DateTime.Now.ToString("MM");
			entry_ano_final.Text = DateTime.Now.ToString("yyyy");
			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_busca_proveedor.Clicked += new EventHandler(on_busca_proveedores_clicked);
			checkbutton_todas_fechas.Clicked += new EventHandler(on_checkbutton_todas_fechas_clicked);
			checkbutton_todos_proveedores.Clicked += new EventHandler(on_checkbutton_todos_proveedore_clicked);
			button_exportar_sheet.Clicked += new EventHandler(on_button_exportar_sheet_clicked);
			entry_id_proveedor.KeyPressEvent += onKeyPressEvent_enter;
			
			creartreeviewgrupoprod();
			llenadogrupoprod();
		}
		
		// Valida entradas que solo sean numericas, se utiliza en ventana
		// Principal cuando selecciona el folio de productos
		// Ademas controla la tecla ENTRER para ver el procedimiento
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				entry_nombre_proveedor.Text = classpublic.lee_registro_de_tabla("osiris_erp_proveedores","id_proveedor"," WHERE osiris_erp_proveedores.id_proveedor = '"+entry_id_proveedor.Text.Trim()+"' AND proveedor_activo = 'true' ","descripcion_proveedor");
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace){
				args.RetVal = true;
			}
		}
		
		void on_button_exportar_sheet_clicked(object sender, EventArgs args)
		{
			string query_sql = "SELECT  to_char(osiris_erp_factura_compra_enca.fecha_factura,'yyyy-MM-dd') AS fechafactura,osiris_erp_factura_compra_enca.numero_factura_proveedor AS numerofactura," +
				"osiris_erp_factura_compra_enca.id_proveedor,descripcion_proveedor," +
				"to_char(osiris_erp_requisicion_deta.id_producto,'999999999999') AS idproducto_osiris,descripcion_producto AS descrip_prod_osiris," +
				"osiris_erp_requisicion_deta.precio_producto_publico,costo_producto_osiris,osiris_erp_requisicion_deta.precio_producto_publico AS costo_unitario_osiris,cantidad_de_embalaje_osiris," +
				"osiris_erp_requisicion_deta.costo_producto AS costo_producto_compra,cantidad_recibida,osiris_erp_requisicion_deta.costo_por_unidad AS costoxunidad_compra,osiris_erp_requisicion_deta.cantidad_de_embalaje," +
				"osiris_erp_requisicion_deta.precio_costo_prov_selec AS precio_prove,id_producto_proveedor,descripcion_producto_proveedor,osiris_erp_requisicion_deta.tipo_unidad_producto,lote_producto,caducidad_producto " +
				"FROM osiris_erp_factura_compra_enca,osiris_erp_proveedores,osiris_erp_requisicion_deta,osiris_productos " +
				"WHERE osiris_erp_factura_compra_enca.id_proveedor = osiris_erp_proveedores.id_proveedor " +
				"AND osiris_erp_factura_compra_enca.numero_factura_proveedor = osiris_erp_requisicion_deta.numero_factura_proveedor " +
				"AND osiris_erp_requisicion_deta.id_producto = osiris_productos.id_producto ORDER BY to_char(osiris_erp_factura_compra_enca.fecha_factura,'yyyy-MM-dd');";
			string[] args_names_field = {"fechafactura","numerofactura","descripcion_proveedor","idproducto_osiris","descrip_prod_osiris","costo_producto_compra","cantidad_de_embalaje","costoxunidad_compra","cantidad_recibida","costo_producto_osiris","cantidad_de_embalaje_osiris","costo_unitario_osiris","id_producto_proveedor","descripcion_producto_proveedor","tipo_unidad_producto","lote_producto","caducidad_producto"};
			string[] args_type_field = {"string","string","string","string","string","float","float","float","float","float","float","float","string","string","string","string","string"};
			
			// class_crea_ods.cs
			new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field);
		}
		
		void on_checkbutton_todos_proveedore_clicked(object sender, EventArgs args)
		{
			bool active_checkbutton;			
			if(checkbutton_todos_proveedores.Active == true){
				active_checkbutton = false;
			}else{
				active_checkbutton = true;
			}
			entry_id_proveedor.Sensitive = active_checkbutton;
			entry_nombre_proveedor.Sensitive = active_checkbutton;
			button_busca_proveedor.Sensitive = active_checkbutton;
		}
		
		void on_checkbutton_todas_fechas_clicked(object sender, EventArgs args)
		{
			bool active_checkbutton;
			if(checkbutton_todas_fechas.Active == true){
				query_rango_fechas= " ";
				active_checkbutton = false;
			}else{
				query_rango_fechas = "AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+(string) entry_ano_inicial.Text.ToString()+"-"+(string) entry_mes_inicial.Text.ToString()+"-"+(string) entry_dia_inicial.Text.ToString()+"' "+
									"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+(string) entry_ano_final.Text.ToString()+"-"+(string) entry_mes_final.Text.ToString()+"-"+(string) entry_dia_final.Text.ToString()+"' ";
				active_checkbutton = true;
			}			
			entry_dia_inicial.Sensitive = active_checkbutton;
			entry_mes_inicial.Sensitive = active_checkbutton;
			entry_ano_inicial.Sensitive = active_checkbutton;			
			entry_dia_final.Sensitive = active_checkbutton;
			entry_mes_final.Sensitive = active_checkbutton;
			entry_ano_final.Sensitive = active_checkbutton;
			
			query_rango_fechas = "AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+(string) entry_ano_inicial.Text.ToString()+"-"+(string) entry_mes_inicial.Text.ToString()+"-"+(string) entry_dia_inicial.Text.ToString()+"' "+
									"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+(string) entry_ano_final.Text.ToString()+"-"+(string) entry_mes_final.Text.ToString()+"-"+(string) entry_dia_final.Text.ToString()+"' ";
		}		
		
		void on_busca_proveedores_clicked(object sender, EventArgs args)
		{
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion
			// la clase recibe tambien el orden del query
			// es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo
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
			classfind_data.buscandor(parametros_objetos,parametros_sql,"find_proveedores_catalogo_producto"," ORDER BY descripcion_proveedor;","%' ",0);
		}
		
		void creartreeviewgrupoprod()
		{
			treeViewEngineListaGrupoProd = new TreeStore(typeof(bool),
											typeof(string),
											typeof(string));
			treeview_lista_grupoprod.Model = treeViewEngineListaGrupoProd;
			
			treeview_lista_grupoprod.RulesHint = true;
			
			Gtk.TreeViewColumn col_00 = new TreeViewColumn();
			Gtk.CellRendererToggle cellrt00 = new CellRendererToggle();
			col_00.Title = "Surtir"; // titulo de la cabecera de la columna, si está visible
			col_00.PackStart(cellrt00, true);
			col_00.AddAttribute (cellrt00, "active", 0);
			cellrt00.Activatable = true;
			cellrt00.Toggled += selecciona_fila;
			
			Gtk.TreeViewColumn col_01 = new TreeViewColumn();
			Gtk.CellRendererText cellrt01 = new CellRendererText();
			col_01.Title = "Solicitado"; // titulo de la cabecera de la columna, si está visible
			col_01.PackStart(cellrt01, true);
			col_01.AddAttribute (cellrt01, "text", 1);
			
			treeview_lista_grupoprod.AppendColumn(col_00);
			treeview_lista_grupoprod.AppendColumn(col_01);
		}
		
		void llenadogrupoprod()
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT id_grupo_producto,descripcion_grupo_producto FROM osiris_grupo_producto ORDER BY descripcion_grupo_producto";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){
					treeViewEngineListaGrupoProd.AppendValues(false,
													(string) lector["descripcion_grupo_producto"],
													(string) lector["id_grupo_producto"].ToString().Trim());
				}				
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		// Cuando seleccion el treeview de cargos extras para cargar los productos  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			if (treeview_lista_grupoprod.Model.GetIter (out iter,new TreePath (args.Path))) {
				bool old = (bool) treeview_lista_grupoprod.Model.GetValue (iter,0);
				treeview_lista_grupoprod.Model.SetValue(iter,0,!old);
			}	
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
	}
}