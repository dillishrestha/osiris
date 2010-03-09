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
		[Widget] Gtk.Entry entry_id_proveedor = null;
		[Widget] Gtk.Entry entry_nombre_proveedor = null;
		[Widget] Gtk.CheckButton checkbutton_todos_proveedores = null;
		[Widget] Gtk.Button button_busca_proveedor = null;
		[Widget] Gtk.TreeView treeview_lista_ordencompra = null;
		
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
		string connectionString;
		string nombrebd;
		
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
			checkbutton_todos_proveedores.Clicked += new EventHandler(on_checkbutton_todos_proveedores_clicked);
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
			//cel_autorizar.Toggled += selecciona_fila;
			
			TreeViewColumn col_orden_compra = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_orden_compra.Title = "Nº O.C."; // titulo de la cabecera de la columna, si está visible
			col_orden_compra.PackStart(cellr1, true);
			col_orden_compra.AddAttribute (cellr1, "text", 1);
			//col_orden_compra.SortColumnId = (int) col_requisicion.col_orden_compra;
			
			TreeViewColumn col_fecha_compra = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_fecha_compra.Title = "Fecha O.C."; // titulo de la cabecera de la columna, si está visible
			col_fecha_compra.PackStart(cellr2, true);
			col_fecha_compra.AddAttribute (cellr2, "text", 2);
			//col_fecha_compra.SortColumnId = (int) col_requisicion.col_fecha_compra;
			
			TreeViewColumn col_proveedor1 = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_proveedor1.Title = "Proveedor 1"; // titulo de la cabecera de la columna, si está visible
			col_proveedor1.PackStart(cellr3, true);
			col_proveedor1.AddAttribute (cellr3, "text", 3);
			//col_proveedor1.SortColumnId = (int) col_requisicion.col_proveedor1;
			
			treeview_lista_ordencompra.AppendColumn(col_autorizar);
			treeview_lista_ordencompra.AppendColumn(col_orden_compra);
			treeview_lista_ordencompra.AppendColumn(col_fecha_compra);
			treeview_lista_ordencompra.AppendColumn(col_proveedor1);
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
			classfind_data.buscandor(parametros_objetos,parametros_sql,"find_proveedores_catalogo_producto"," ORDER BY descripcion_proveedor;","%' ");			
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}
