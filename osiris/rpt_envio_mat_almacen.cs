// created on 07/02/2008 at 09:34 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Jesus Buentello G. (Programacion)
//				  Ing. Daniel Olivares C. (Adecuaciones y mejoras) arcangeldoc@gmail.com 05/05/2007
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
	public class rpt_envio_almacen
	{
		//[Widget] Gtk.Entry entry_fecha_inicio;
		[Widget] Gtk.Window envio_almacenes = null;
		[Widget] Gtk.Entry entry_dia_inicio;
		[Widget] Gtk.Entry entry_mes_inicio;
		[Widget] Gtk.Entry entry_ano_inicio;
		
		//[Widget] Gtk.Entry entry_fecha_termino;
		
		[Widget] Gtk.Entry entry_dia_termino;
		[Widget] Gtk.Entry entry_mes_termino;
		[Widget] Gtk.Entry entry_ano_termino;
		[Widget] Gtk.HBox hbox1 = null;
		
		[Widget] Gtk.CheckButton checkbutton_todos_envios;
		[Widget] Gtk.CheckButton checkbutton_seleccion_presupuestos;
		[Widget] Gtk.TreeView lista_almacenes;
		[Widget] Gtk.Button button_buscar;
		[Widget] Gtk.Button button_rep;
		[Widget] Gtk.Button button_salir;
		
		string query_fechas = " ";
		string rango1 = "";
		string rango2 = "";
		string query_general = "";
		string nombrebd;
		int idsubalmacen;
		int filas=690;
		int contador = 1;
		int numpage = 1;
		
		private ListStore treeViewEnginesolicitud;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		string connectionString;
		
		class_conexion conexion_a_DB = new class_conexion();
						
		public rpt_envio_almacen(string nombrebd_,int idsubalmacen_)		
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			idsubalmacen = idsubalmacen_;
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "envio_almacenes", null);
			gxml.Autoconnect (this);
			
			entry_dia_inicio.Text = DateTime.Now.ToString("dd");
			entry_mes_inicio.Text = DateTime.Now.ToString("MM");
			entry_ano_inicio.Text = DateTime.Now.ToString("yyyy");
			
			entry_dia_termino.Text = DateTime.Now.ToString("dd");
			entry_mes_termino.Text = DateTime.Now.ToString("MM");
			entry_ano_termino.Text = DateTime.Now.ToString("yyyy");
			
			//hbox1.Hide();
			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
            button_buscar.Clicked += new EventHandler(on_buscar_clicked);
           	button_rep.Clicked += new EventHandler(on_button_rep_clicked);
          	checkbutton_todos_envios.Clicked += new EventHandler(on_checkbutton_todos_envios);
          	checkbutton_seleccion_presupuestos.Hide();
          	
          	crea_treeview_solicitud();
		}
            
		void crea_treeview_solicitud()
		{
			treeViewEnginesolicitud = new ListStore(typeof(bool),//0
														typeof(string),
														typeof(string),
														typeof(string),
														typeof(string),
														typeof(string),
			                                        	typeof(string),typeof(string),typeof(string));
				
			lista_almacenes.Model = treeViewEnginesolicitud;
			lista_almacenes.RulesHint = true;
				
			TreeViewColumn col_seleccion = new TreeViewColumn();
			CellRendererToggle cellr0 = new CellRendererToggle();
			col_seleccion.Title = "Seleccion"; // titulo de la cabecera de la columna, si está visible
			col_seleccion.PackStart(cellr0, true);
			col_seleccion.AddAttribute (cellr0, "active", 0);
			cellr0.Activatable = true;
			cellr0.Toggled += selecciona_fila_grupo;
			col_seleccion.SortColumnId = (int) column_reporte.col_seleccion;
		
			TreeViewColumn col_solicito = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_solicito.Title = "Solicitud"; // titulo de la cabecera de la columna, si está visible
			col_solicito.PackStart(cellr1, true);
			col_solicito.AddAttribute (cellr1, "text", 1);
			cellr1.Foreground = "darkblue";
			col_solicito.SortColumnId = (int) column_reporte.col_solicito;
			
			TreeViewColumn col_sub = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_sub.Title = "Sub Almacen"; // titulo de la cabecera de la columna, si está visible
			col_sub.PackStart(cellr2, true);
			col_sub.AddAttribute (cellr2, "text", 2);
			cellr2.Foreground = "darkblue";
			col_sub.SortColumnId = (int) column_reporte.col_sub;
						
			TreeViewColumn col_fecha_envio = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_fecha_envio.Title = "Fecha Envio"; // titulo de la cabecera de la columna, si está visible
			col_fecha_envio.PackStart(cellr3, true);
			col_fecha_envio.AddAttribute (cellr3, "text", 3);
			cellr3.Foreground = "darkblue";
			col_fecha_envio.SortColumnId = (int) column_reporte.col_fecha_envio;
			
			TreeViewColumn col_id_sol = new TreeViewColumn();
			CellRendererText cellr4 = new CellRendererText();
			col_id_sol.Title = "id Solicito"; // titulo de la cabecera de la columna, si está visible
			col_id_sol.PackStart(cellr4, true);
			col_id_sol.AddAttribute (cellr4, "text", 4);
			cellr4.Foreground = "darkblue";
			col_id_sol.SortColumnId = (int) column_reporte.col_id_sol;
			
			TreeViewColumn col_numeroatencion = new TreeViewColumn();
			CellRendererText cellr6 = new CellRendererText();
			col_numeroatencion.Title = "N° Atencion"; // titulo de la cabecera de la columna, si está visible
			col_numeroatencion.PackStart(cellr6, true);
			col_numeroatencion.AddAttribute (cellr6, "text", 6);
			cellr6.Foreground = "darkblue";
			col_numeroatencion.SortColumnId = (int) column_reporte.col_numeroatencion;
			
			TreeViewColumn col_pidpaciente = new TreeViewColumn();
			CellRendererText cellr7 = new CellRendererText();
			col_pidpaciente.Title = "PID"; // titulo de la cabecera de la columna, si está visible
			col_pidpaciente.PackStart(cellr7, true);
			col_pidpaciente.AddAttribute (cellr7, "text", 7);
			cellr7.Foreground = "darkblue";
			col_pidpaciente.SortColumnId = (int) column_reporte.col_pidpaciente;
			
			TreeViewColumn col_nombrepaciente = new TreeViewColumn();
			CellRendererText cellr8 = new CellRendererText();
			col_nombrepaciente.Title = "Nombre Paciente"; // titulo de la cabecera de la columna, si está visible
			col_nombrepaciente.PackStart(cellr8, true);
			col_nombrepaciente.AddAttribute (cellr8, "text", 8);
			cellr8.Foreground = "darkblue";
			col_nombrepaciente.SortColumnId = (int) column_reporte.col_nombrepaciente;
			
			lista_almacenes.AppendColumn(col_seleccion);
			lista_almacenes.AppendColumn(col_solicito);
			lista_almacenes.AppendColumn(col_sub);
			lista_almacenes.AppendColumn(col_fecha_envio);
			lista_almacenes.AppendColumn(col_id_sol);
			lista_almacenes.AppendColumn(col_numeroatencion);
			lista_almacenes.AppendColumn(col_pidpaciente);
			lista_almacenes.AppendColumn(col_nombrepaciente);
		}
		
		enum column_reporte
		{
			col_seleccion,
			col_solicito,
			col_sub,
			col_fecha_envio,
			col_id_sol,
			col_numeroatencion,
			col_pidpaciente,
			col_nombrepaciente,
		}
		
		void llenando_lista_de_solicitudes()
		{
			treeViewEnginesolicitud.Clear();
			//treeViewEnginesolicitud.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
					comando.CommandText = "SELECT COUNT(osiris_his_solicitudes_deta.folio_de_solicitud) AS cantidad_solicitud,to_char(folio_de_solicitud,'9999999999') AS foliodesolicitud,"+
								"to_char(osiris_his_solicitudes_deta.id_almacen,'999999999') AS idalmacen,osiris_almacenes.descripcion_almacen AS descripcionalmacen,"+
								"to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'yyyy-MM-dd HH24:mi') AS fecha_envio,osiris_his_solicitudes_deta.id_empleado,"+
								"osiris_his_solicitudes_deta.folio_de_servicio AS foliodeatencion,"+
								"osiris_his_solicitudes_deta.pid_paciente AS pidpaciente,"+
								"nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente "+
								"FROM osiris_his_solicitudes_deta,osiris_almacenes,osiris_his_paciente "+								
								"WHERE osiris_his_solicitudes_deta.id_almacen = osiris_almacenes.id_almacen "+
								"AND osiris_his_solicitudes_deta.folio_de_solicitud > 0 "+
								"AND status = 'true' "+
								"AND osiris_his_paciente.pid_paciente = osiris_his_solicitudes_deta.pid_paciente "+
								""+query_fechas+" "+
								"GROUP BY osiris_his_solicitudes_deta.folio_de_solicitud,osiris_his_solicitudes_deta.id_almacen,"+
								"osiris_almacenes.descripcion_almacen,to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'yyyy-MM-dd HH24:mi'),osiris_his_solicitudes_deta.id_empleado,"+
								"osiris_his_solicitudes_deta.folio_de_servicio,"+
								"osiris_his_solicitudes_deta.pid_paciente,nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente "+
								"ORDER BY to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'yyyy-MM-dd HH24:mi'),osiris_his_solicitudes_deta.id_almacen,osiris_his_solicitudes_deta.folio_de_solicitud;";
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();

				while (lector.Read()){							
					treeViewEnginesolicitud.AppendValues (false, //0
													(string) lector["foliodesolicitud"],//1
													(string) lector["descripcionalmacen"], // 2
													(string) lector["fecha_envio"],//3
													(string) lector["id_empleado"],//4
													(string) lector["idalmacen"],
					                                (string) lector["foliodeatencion"].ToString().Trim(),
					                                (string) lector["pidpaciente"].ToString().Trim(),
					                                (string) lector["nombre1_paciente"].ToString().Trim()+" "+
													(string) lector["nombre2_paciente"].ToString().Trim()+" "+
													(string) lector["apellido_paterno_paciente"].ToString().Trim()+" "+
													(string) lector["apellido_materno_paciente"].ToString().Trim());
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
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
		
		//Seleccionar todos los del treeview, un check_button 
		void on_checkbutton_todos_envios(object sender, EventArgs args)
		{
			if ((bool)checkbutton_todos_envios.Active == true){
				TreeIter iter2;
				if (treeViewEnginesolicitud.GetIterFirst (out iter2)){
					lista_almacenes.Model.SetValue(iter2,0,true);
					while (treeViewEnginesolicitud.IterNext(ref iter2)){
						lista_almacenes.Model.SetValue(iter2,0,true);
					}
				}
			}else{
				TreeIter iter2;
				if (treeViewEnginesolicitud.GetIterFirst (out iter2)){
					lista_almacenes.Model.SetValue(iter2,0,false);
					while (treeViewEnginesolicitud.IterNext(ref iter2)){
						lista_almacenes.Model.SetValue(iter2,0,false);
					}
				}
			}
		}
		
		void on_buscar_clicked (object sender, EventArgs args)
		{
			if (checkbutton_todos_envios.Active == true) {
				query_fechas = " ";	 
				rango1 = "";
				rango2 = "";
			}else{
				rango1 = entry_ano_inicio.Text+"-"+entry_mes_inicio.Text+"-"+entry_dia_inicio.Text;
				rango2 = entry_ano_termino.Text+"-"+entry_mes_termino.Text+"-"+entry_dia_termino.Text;
				query_fechas = " AND to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'yyyy-MM-dd') >= '"+rango1+"' "+
								"AND to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'yyyy-MM-dd') <= '"+rango2+"' ";
				}	
				llenando_lista_de_solicitudes();
			}
			
		void on_button_rep_clicked(object sender, EventArgs args)
		{
			string numeros_seleccionado = "";
			string almacenes_seleccionados = ""; 
			string variable_paso_03 = "";
			string variable_paso_04 = "";
			int variable_paso_02_1 = 0;
			string query_in_num = "";
 			string query_in_almacen = ""; 
			
			if (this.checkbutton_todos_envios.Active == true) { 
				query_fechas = " ";	 
				rango1 = "";
				rango2 = "";
			}else{
				rango1 = entry_ano_inicio.Text+"-"+entry_mes_inicio.Text+"-"+entry_dia_inicio.Text;
				rango2 = entry_ano_termino.Text+"-"+entry_mes_termino.Text+"-"+entry_dia_termino.Text;				
				query_fechas = " AND to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'yyyy-MM-dd') >= '"+rango1+"' "+
								"AND to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'yyyy-MM-dd') <= '"+rango2+"' ";
			}
		
			//poder elegir una fila del treeview
			TreeIter iter;
			if (treeViewEnginesolicitud.GetIterFirst (out iter)){
			
 				if ((bool) lista_almacenes.Model.GetValue (iter,0) == true){
					numeros_seleccionado = (string) lista_almacenes.Model.GetValue (iter,1);
 					almacenes_seleccionados = (string) lista_almacenes.Model.GetValue (iter,5);
 					variable_paso_02_1 += 1;		
 				}
 				while (treeViewEnginesolicitud.IterNext(ref iter)){
 					if ((bool) lista_almacenes.Model.GetValue (iter,0) == true){
						if (variable_paso_02_1 == 0){ 				    	
 							numeros_seleccionado = (string) lista_almacenes.Model.GetValue (iter,1);
 							almacenes_seleccionados = (string) lista_almacenes.Model.GetValue (iter,5);
 							variable_paso_02_1 += 1;
 						}else{
 							variable_paso_03 = (string) lista_almacenes.Model.GetValue (iter,1);
 							variable_paso_04 = (string) lista_almacenes.Model.GetValue (iter,5);
 							numeros_seleccionado = numeros_seleccionado.Trim() + "','" + variable_paso_03.Trim();
 							almacenes_seleccionados = almacenes_seleccionados.Trim() + "','" + variable_paso_04.Trim();
 						}
 					}
 				}
 			}
 						
 			if (variable_paso_02_1 > 0){
	 			query_in_num = " AND osiris_his_solicitudes_deta.folio_de_solicitud IN ('"+numeros_seleccionado+"') ";
				query_in_almacen = " AND osiris_his_solicitudes_deta.id_almacen IN ('"+almacenes_seleccionados+"') ";
			}
			if (treeViewEnginesolicitud.GetIterFirst (out iter)){
				if (variable_paso_02_1 > 0){
					new osiris.rpt_solicitud_subalmacenes(idsubalmacen,query_in_num,query_in_almacen,query_fechas);
				}
			}
		}
			
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}	
	}
}                	    	             

