///////////////////////////////////////////////////////
// created on 04/03/2009 at 05:20 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion) arcangeldoc@gmail.com
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
	public class consulta_ordencompra
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		// Declarando ventana de consulta de ordenes de compra
		[Widget] Gtk.Window consulta_ordenes_compra = null;
		[Widget] Gtk.Entry entry_dia_inicio = null;
		[Widget] Gtk.Entry entry_mes_inicio = null;
		[Widget] Gtk.Entry entry_ano_inicio = null;
		[Widget] Gtk.Entry entry_dia_termino = null;
		[Widget] Gtk.Entry entry_mes_termino = null;
		[Widget] Gtk.Entry entry_ano_termino = null;
		[Widget] Gtk.CheckButton checkbutton_todasfechas = null;
		[Widget] Gtk.Entry entry_id_proveedor = null;
		[Widget] Gtk.Entry entry_nombre_proveedor = null;
		[Widget] Gtk.Button button_consultar_OC = null;
		[Widget] Gtk.CheckButton checkbutton_todos_proveedores = null;
		[Widget] Gtk.Button button_busca_proveedor = null;
		[Widget] Gtk.TreeView treeview_lista_ordencompra = null;
		
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
		string connectionString;
		string nombrebd;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		private ListStore treeViewEngineListaOC;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		
		public consulta_ordencompra(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_)
		{
			LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
    		
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "consulta_ordenes_compra", null);
			gxml.Autoconnect (this);
			////// Muestra ventana de Glade
			consulta_ordenes_compra.Show();
			button_busca_proveedor.Clicked += new EventHandler(on_busca_proveedor_clicked);
			checkbutton_todasfechas.Clicked += new EventHandler(on_checkbutton_todasfechas_clicked);
			checkbutton_todos_proveedores.Clicked += new EventHandler(on_checkbutton_todos_proveedores_clicked);
			button_consultar_OC.Clicked += new EventHandler(on_button_consultar_OC_clicked);
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			entry_dia_inicio.Text = DateTime.Now.ToString("dd");
			entry_mes_inicio.Text = DateTime.Now.ToString("MM");
			entry_ano_inicio.Text = DateTime.Now.ToString("yyyy");
			
			entry_dia_termino.Text = DateTime.Now.ToString("dd");
			entry_mes_termino.Text = DateTime.Now.ToString("MM");
			entry_ano_termino.Text = DateTime.Now.ToString("yyyy");
			entry_nombre_proveedor.IsEditable = false;
			
			crea_treeview_ordencompra();
		}
		
		void on_checkbutton_todasfechas_clicked(object sender, EventArgs args)
		{
			if(checkbutton_todasfechas.Active == true){
				entry_dia_inicio.Sensitive = false;
				entry_mes_inicio.Sensitive = false;
				entry_ano_inicio.Sensitive = false;			
				entry_dia_termino.Sensitive = false;
				entry_mes_termino.Sensitive = false;
				entry_ano_termino.Sensitive = false;
			}else{
				entry_dia_inicio.Sensitive = true;
				entry_mes_inicio.Sensitive = true;
				entry_ano_inicio.Sensitive = true;			
				entry_dia_termino.Sensitive = true;
				entry_mes_termino.Sensitive = true;
				entry_ano_termino.Sensitive = true;
			}
		}
						
		void on_checkbutton_todos_proveedores_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todos_proveedores.Active == true){
				entry_id_proveedor.Sensitive = false;
				entry_nombre_proveedor.Sensitive = false;
				button_busca_proveedor.Sensitive = false;
			}else{
				entry_id_proveedor.Sensitive = true;
				entry_nombre_proveedor.Sensitive = true;
				button_busca_proveedor.Sensitive = true;
			}
		}
		
		void on_button_consultar_OC_clicked(object sender, EventArgs args)
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
 				comando.CommandText = "SELECT numero_orden_compra,"+
											"to_char(fechahora_creacion,'yyyy-MM-dd') AS fechahoracreacion,descripcion_proveedor "+
											" FROM osiris_erp_ordenes_compras_enca ORDER BY fechahora_creacion DESC;";
				Console.WriteLine(comando.CommandText);
				comando.ExecuteNonQuery();    comando.Dispose();
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){
					treeViewEngineListaOC.AppendValues(false,
				                                    Convert.ToString((int) lector["numero_orden_compra"]),
				                                   	(string) lector["fechahoracreacion"],
				                                   	(string) lector["descripcion_proveedor"]);
				}
			}catch(NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error, 
									ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close();	
		}
		
		void crea_treeview_ordencompra()
		{
			treeViewEngineListaOC = new ListStore(typeof(bool),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));
			treeview_lista_ordencompra.Model = treeViewEngineListaOC;
			treeview_lista_ordencompra.RulesHint = true;
						
			TreeViewColumn col_autorizar = new TreeViewColumn();
			CellRendererToggle cel_autorizar = new CellRendererToggle();
			col_autorizar.Title = "Seleccion"; // titulo de la cabecera de la columna, si está visible
			col_autorizar.PackStart(cel_autorizar, true);
			col_autorizar.AddAttribute (cel_autorizar, "active", 0);
			cel_autorizar.Activatable = true;
			cel_autorizar.Toggled += selecciona_fila;
			col_autorizar.SortColumnId = (int) col_ordencompra.col_autorizar;
			
			TreeViewColumn col_orden_compra = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_orden_compra.Title = "Nº O.C."; // titulo de la cabecera de la columna, si está visible
			col_orden_compra.PackStart(cellr1, true);
			col_orden_compra.AddAttribute (cellr1, "text", 1);
			col_orden_compra.SortColumnId = (int) col_ordencompra.col_orden_compra;
			
			TreeViewColumn col_fecha_compra = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_fecha_compra.Title = "Fecha O.C."; // titulo de la cabecera de la columna, si está visible
			col_fecha_compra.PackStart(cellr2, true);
			col_fecha_compra.AddAttribute (cellr2, "text", 2);
			col_fecha_compra.SortColumnId = (int) col_ordencompra.col_fecha_compra;
			
			TreeViewColumn col_proveedor1 = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_proveedor1.Title = "Proveedor 1"; // titulo de la cabecera de la columna, si está visible
			col_proveedor1.PackStart(cellr3, true);
			col_proveedor1.AddAttribute (cellr3, "text", 3);
			col_proveedor1.SortColumnId = (int) col_ordencompra.col_proveedor1;
			
			treeview_lista_ordencompra.AppendColumn(col_autorizar);
			treeview_lista_ordencompra.AppendColumn(col_orden_compra);
			treeview_lista_ordencompra.AppendColumn(col_fecha_compra);
			treeview_lista_ordencompra.AppendColumn(col_proveedor1);
		}
		
		// Cuando seleccion campos para la autorizacion de compras  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (this.treeview_lista_ordencompra.Model.GetIter (out iter, path)){					
				bool old = (bool) treeview_lista_ordencompra.Model.GetValue(iter,0);
				treeview_lista_ordencompra.Model.SetValue(iter,0,!old);
			}				
		}
		
		enum col_ordencompra
		{
			col_autorizar,
			col_orden_compra,
			col_fecha_compra,
			col_proveedor1			
		}
		
		void on_busca_proveedor_clicked (object sender, EventArgs args)
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
			string[] parametros_string = {};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_proveedores_catalogo_producto"," ORDER BY descripcion_proveedor;","%' ",0);			
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}
