///////////////////////////////////////////////////////// 
// created on 18/11/2010 at 17:02
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. arcangeldoc@gmail.com
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
//////////////////////////////////////////////////////

using System;
using Gtk;
using Npgsql;
using System.Data;
using Glade;
using System.Collections;

namespace osiris
{
	public class lista_ordenes_compra
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		// Ventana seleccion de Ordenes de Compra
		[Widget] Gtk.Window envio_almacenes = null;
		[Widget] Gtk.Entry entry_dia_inicio = null;
		[Widget] Gtk.Entry entry_mes_inicio = null;
		[Widget] Gtk.Entry entry_ano_inicio = null;		
		[Widget] Gtk.Entry entry_dia_termino = null;
		[Widget] Gtk.Entry entry_mes_termino = null;
		[Widget] Gtk.Entry entry_ano_termino = null;
		[Widget] Gtk.HBox hbox1 = null;
		[Widget] Gtk.CheckButton checkbutton_todos_envios = null;
		[Widget] Gtk.Label label3 = null;
		[Widget] Gtk.Entry entry_desc_producto = null;
		[Widget] Gtk.Button button_buscar_prodreq = null;
		
		[Widget] Gtk.CheckButton checkbutton_seleccion_presupuestos = null;
		[Widget] Gtk.TreeView lista_almacenes = null;
		[Widget] Gtk.Button button_buscar = null;
		[Widget] Gtk.Button button_rep = null;
		
		private ListStore treeViewEngineordendecompra;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		string connectionString;
		string nombrebd;
		
		string query_fechas = " ";
		string rango1 = "";
		string rango2 = "";
		string query_general = "";
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public lista_ordenes_compra ()
		{
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "envio_almacenes", null);
			gxml.Autoconnect (this);
						
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
		
			envio_almacenes.Title = "LISTA ORDENES DE COMPRA";
			entry_dia_inicio.Text = DateTime.Now.ToString("dd");
			entry_mes_inicio.Text = DateTime.Now.ToString("MM");
			entry_ano_inicio.Text = DateTime.Now.ToString("yyyy");
				
			entry_dia_termino.Text = DateTime.Now.ToString("dd");
			entry_mes_termino.Text = DateTime.Now.ToString("MM");
			entry_ano_termino.Text = DateTime.Now.ToString("yyyy");
				
			hbox1.Hide();
			label3.Hide();
			entry_desc_producto.Hide();
			button_buscar_prodreq.Hide();
			
			checkbutton_seleccion_presupuestos.Hide();
			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
            button_buscar.Clicked += new EventHandler(on_buscar_ordencompra_clicked);
           	button_rep.Clicked += new EventHandler(on_button_rep_clicked);
          	checkbutton_todos_envios.Clicked += new EventHandler(on_checkbutton_todos_envios);
			
			crea_treeview_ordendecompra();
		}
		
		void on_buscar_ordencompra_clicked(object sender, EventArgs args)
		{
			treeViewEngineordendecompra.Clear();			
			if (checkbutton_todos_envios.Active == true) {
				query_fechas = " ";				
			}else{
				query_fechas = "AND to_char(osiris_erp_ordenes_compras_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+(string) entry_ano_inicio.Text.ToString()+"-"+(string) entry_mes_inicio.Text.ToString()+"-"+(string) entry_dia_inicio.Text.ToString()+"' "+
									"AND to_char(osiris_erp_ordenes_compras_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+(string) entry_ano_termino.Text.ToString()+"-"+(string) entry_mes_termino.Text.ToString()+"-"+(string) entry_dia_termino.Text.ToString()+"' ";
			}
						
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT numero_orden_compra,osiris_erp_ordenes_compras_enca.id_proveedor,osiris_erp_ordenes_compras_enca.descripcion_proveedor," +
					"to_char(fechahora_creacion,'yyyy-MM-dd') AS fechahoracreacion "+
					"FROM osiris_erp_ordenes_compras_enca,osiris_erp_proveedores " +
					"WHERE osiris_erp_ordenes_compras_enca.id_proveedor = osiris_erp_proveedores.id_proveedor " +
					query_fechas+";";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					treeViewEngineordendecompra.AppendValues(false,
					                                         lector["numero_orden_compra"].ToString().Trim(),
					                                         lector["fechahoracreacion"].ToString(),
					                                          lector["id_proveedor"].ToString().Trim(),
					                                         lector["descripcion_proveedor"].ToString().Trim());
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
								msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void crea_treeview_ordendecompra()
		{
			treeViewEngineordendecompra = new ListStore(typeof(bool),//0
														typeof(string),
														typeof(string),
														typeof(string),
														typeof(string),
														typeof(string),
			                                        	typeof(string),
			                                            typeof(string),
			                                            typeof(string));
				
			lista_almacenes.Model = treeViewEngineordendecompra;
			lista_almacenes.RulesHint = true;
				
			TreeViewColumn col_seleccion = new TreeViewColumn();
			CellRendererToggle cellr0 = new CellRendererToggle();
			col_seleccion.Title = "Seleccion";
			col_seleccion.PackStart(cellr0, true);
			col_seleccion.AddAttribute (cellr0, "active", 0);
			cellr0.Activatable = true;
			cellr0.Toggled += selecciona_fila_grupo; 
		
			TreeViewColumn col_nro_oc = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_nro_oc.Title = "N° O.C.";
			col_nro_oc.PackStart(cellr1, true);
			col_nro_oc.AddAttribute (cellr1, "text", 1);
			cellr1.Foreground = "darkblue";
			
			TreeViewColumn col_sub = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_sub.Title = "Fecha O.C.";
			col_sub.PackStart(cellr2, true);
			col_sub.AddAttribute (cellr2, "text", 2);
			cellr2.Foreground = "darkblue";
						
			TreeViewColumn col_fecha_envio = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_fecha_envio.Title = "ID Proveedor";
			col_fecha_envio.PackStart(cellr3, true);
			col_fecha_envio.AddAttribute (cellr3, "text", 3);
			cellr3.Foreground = "darkblue";
			
			TreeViewColumn col_id_sol = new TreeViewColumn();
			CellRendererText cellr4 = new CellRendererText();
			col_id_sol.Title = "Nombre Proveedor";
			col_id_sol.PackStart(cellr4, true);
			col_id_sol.AddAttribute (cellr4, "text", 4);
			cellr4.Foreground = "darkblue";
			
			TreeViewColumn col_numeroatencion = new TreeViewColumn();
			CellRendererText cellr6 = new CellRendererText();
			col_numeroatencion.Title = "N° Atencion"; // titulo de la cabecera de la columna, si está visible
			col_numeroatencion.PackStart(cellr6, true);
			col_numeroatencion.AddAttribute (cellr6, "text", 6);
			cellr6.Foreground = "darkblue";
			
			TreeViewColumn col_pidpaciente = new TreeViewColumn();
			CellRendererText cellr7 = new CellRendererText();
			col_pidpaciente.Title = "Codigo"; // titulo de la cabecera de la columna, si está visible
			col_pidpaciente.PackStart(cellr7, true);
			col_pidpaciente.AddAttribute (cellr7, "text", 7);
			cellr7.Foreground = "darkblue";
			
			TreeViewColumn col_nombrepaciente = new TreeViewColumn();
			CellRendererText cellr8 = new CellRendererText();
			col_nombrepaciente.Title = "Nombre Proveedor"; // titulo de la cabecera de la columna, si está visible
			col_nombrepaciente.PackStart(cellr8, true);
			col_nombrepaciente.AddAttribute (cellr8, "text", 8);
			cellr8.Foreground = "darkblue";
			
			lista_almacenes.AppendColumn(col_seleccion);
			lista_almacenes.AppendColumn(col_nro_oc);
			lista_almacenes.AppendColumn(col_sub);
			lista_almacenes.AppendColumn(col_fecha_envio);
			lista_almacenes.AppendColumn(col_id_sol);
			lista_almacenes.AppendColumn(col_numeroatencion);
			lista_almacenes.AppendColumn(col_pidpaciente);
			lista_almacenes.AppendColumn(col_nombrepaciente);		
		}
		
		//Seleccionar todos los del treeview, un check_button 
		void on_checkbutton_todos_envios(object sender, EventArgs args)
		{
			if ((bool)checkbutton_todos_envios.Active == true){
				TreeIter iter2;
				if (treeViewEngineordendecompra.GetIterFirst (out iter2)){
					lista_almacenes.Model.SetValue(iter2,0,true);
					while (treeViewEngineordendecompra.IterNext(ref iter2)){
						lista_almacenes.Model.SetValue(iter2,0,true);
					}
				}
			}else{
				TreeIter iter2;
				if (treeViewEngineordendecompra.GetIterFirst (out iter2)){
					lista_almacenes.Model.SetValue(iter2,0,false);
					while (treeViewEngineordendecompra.IterNext(ref iter2)){
						lista_almacenes.Model.SetValue(iter2,0,false);
					}
				}
			}
		}
		
		void selecciona_fila_grupo(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (lista_almacenes.Model.GetIter (out iter, path)) {
				bool old = (bool) lista_almacenes.Model.GetValue (iter,0);
				lista_almacenes.Model.SetValue(iter,0,!old);
			}
		}
		
		void on_button_rep_clicked(object sender, EventArgs args)
		{
			string numeros_seleccionado = "";
			int variable_paso_02_1 = 0;
			string query_in_num = "";
 				
			if (this.checkbutton_todos_envios.Active == true) { 
				query_fechas = " ";	 
				rango1 = "";
				rango2 = "";
			}else{
				rango1 = entry_ano_inicio.Text+"-"+entry_mes_inicio.Text+"-"+entry_dia_inicio.Text;
				rango2 = entry_ano_termino.Text+"-"+entry_mes_termino.Text+"-"+entry_dia_termino.Text;				
				query_fechas = " AND to_char(osiris_erp_ordenes_compras_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+rango1+"' "+
								"AND to_char(osiris_erp_ordenes_compras_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+rango2+"' ";
			}
		
			//poder elegir una fila del treeview
			TreeIter iter;
			if(treeViewEngineordendecompra.GetIterFirst (out iter)){
			
 				if ((bool) lista_almacenes.Model.GetValue (iter,0) == true){
					numeros_seleccionado = (string) lista_almacenes.Model.GetValue (iter,1);
 					variable_paso_02_1 += 1;		
 				}
 				while(treeViewEngineordendecompra.IterNext(ref iter)){
 					if ((bool) lista_almacenes.Model.GetValue (iter,0) == true){
						if (variable_paso_02_1 == 0){ 				    	
 							numeros_seleccionado = (string) lista_almacenes.Model.GetValue (iter,1);
 							variable_paso_02_1 += 1;
 						}else{
 							numeros_seleccionado = numeros_seleccionado.Trim() + "','" + (string) lista_almacenes.Model.GetValue (iter,1);
 						}
 					}
 				}
 			}
 						
 			if (variable_paso_02_1 > 0){
	 			query_in_num = " AND osiris_erp_ordenes_compras_enca.numero_orden_compra IN ('"+numeros_seleccionado+"') ";
			}
			if (treeViewEngineordendecompra.GetIterFirst (out iter)){
				if (variable_paso_02_1 > 0){
					//new osiris.rpt_solicitud_subalmacenes(idsubalmacen,query_in_num,query_in_almacen,query_fechas);
					new osiris.rpt_orden_compras(query_in_num,query_fechas);   // imprime la orden de compra
				}
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
