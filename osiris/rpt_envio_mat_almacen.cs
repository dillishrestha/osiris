// created on 07/02/2008 at 09:34 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Jesus Buentello G. (Programacion)
//				  Ing. Daniel Olivares C. (Adecuaciones y mejoras) arcangeldoc@gmail.com 05/10/2010
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
		
		TreeViewColumn col_00;		CellRendererToggle cellr00;
		TreeViewColumn col_01;		CellRendererText cellr01;
		TreeViewColumn col_02;		CellRendererText cellr02;
		TreeViewColumn col_03;		CellRendererText cellr03;
		TreeViewColumn col_04;		CellRendererText cellr04;
		TreeViewColumn col_06;		CellRendererText cellr06;
		TreeViewColumn col_07;		CellRendererText cellr07;
		TreeViewColumn col_08;		CellRendererText cellr08;
		TreeViewColumn col_09;		CellRendererText cellr09;
		TreeViewColumn col_10;		CellRendererText cellr10;
		TreeViewColumn col_11;		CellRendererText cellr11;
		TreeViewColumn col_12;		CellRendererText cellr12;
		
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
			                                        	typeof(string),
			                                        typeof(string),
			                                        typeof(string),
			                                        typeof(string),
			                                        typeof(string),
			                                        typeof(string),
			                                        typeof(string));
				
			lista_almacenes.Model = treeViewEnginesolicitud;
			lista_almacenes.RulesHint = true;
				
			col_00 = new TreeViewColumn();
			cellr00 = new CellRendererToggle();
			col_00.Title = "Seleccion";
			col_00.PackStart(cellr00, true);
			col_00.AddAttribute (cellr00, "active", 0);
			cellr00.Activatable = true;
			cellr00.Toggled += selecciona_fila_grupo;
			col_00.SortColumnId = (int) column_reporte.col_00;
		
			col_01 = new TreeViewColumn();
			cellr01 = new CellRendererText();
			col_01.Title = "Solicitud";
			col_01.PackStart(cellr01, true);
			col_01.AddAttribute (cellr01, "text", 1);
			cellr01.Foreground = "darkblue";
			col_01.SortColumnId = (int) column_reporte.col_01;
			
			col_02 = new TreeViewColumn();
			cellr02 = new CellRendererText();
			col_02.Title = "Sub Almacen";
			col_02.PackStart(cellr02, true);
			col_02.AddAttribute (cellr02, "text", 2);
			cellr02.Foreground = "darkblue";
			col_02.SortColumnId = (int) column_reporte.col_02;
						
			col_03 = new TreeViewColumn();
			cellr03 = new CellRendererText();
			col_03.Title = "Fecha Envio";
			col_03.PackStart(cellr03, true);
			col_03.AddAttribute (cellr03, "text", 3);
			cellr03.Foreground = "darkblue";
			col_03.SortColumnId = (int) column_reporte.col_03;
			
			col_04 = new TreeViewColumn();
			cellr04 = new CellRendererText();
			col_04.Title = "id Solicito";
			col_04.PackStart(cellr04, true);
			col_04.AddAttribute (cellr04, "text", 4);
			cellr04.Foreground = "darkblue";
			col_04.SortColumnId = (int) column_reporte.col_04;
			
			col_06 = new TreeViewColumn();
			cellr06 = new CellRendererText();
			col_06.Title = "N° Atencion";
			col_06.PackStart(cellr06, true);
			col_06.AddAttribute (cellr06, "text", 6);
			cellr06.Foreground = "darkblue";
			col_06.SortColumnId = (int) column_reporte.col_06;
			
			col_07 = new TreeViewColumn();
			cellr07 = new CellRendererText();
			col_07.Title = "PID";
			col_07.PackStart(cellr07, true);
			col_07.AddAttribute (cellr07, "text", 7);
			cellr07.Foreground = "darkblue";
			col_07.SortColumnId = (int) column_reporte.col_07;
			
			col_08 = new TreeViewColumn();
			cellr08 = new CellRendererText();
			col_08.Title = "Nombre Paciente";
			col_08.PackStart(cellr08, true);
			col_08.AddAttribute (cellr08, "text", 8);
			cellr08.Foreground = "darkblue";
			col_08.SortColumnId = (int) column_reporte.col_08;
			
			col_09 = new TreeViewColumn();
			cellr09 = new CellRendererText();
			col_09.Title = "Procedimiento Qx.";
			col_09.PackStart(cellr09, true);
			col_09.AddAttribute (cellr09, "text", 9);
			cellr09.Foreground = "darkblue";
			col_09.SortColumnId = (int) column_reporte.col_09;
			
			col_10 = new TreeViewColumn();
			cellr10 = new CellRendererText();
			col_10.Title = "Motivo de Ingreso";
			col_10.PackStart(cellr10, true);
			col_10.AddAttribute (cellr10, "text", 10);
			cellr10.Foreground = "darkblue";
			col_10.SortColumnId = (int) column_reporte.col_10;			
			
			col_11 = new TreeViewColumn();
			cellr11 = new CellRendererText();
			col_11.Title = "Observacion";
			col_11.PackStart(cellr11, true);
			col_11.AddAttribute (cellr11, "text", 11);
			cellr11.Foreground = "darkblue";
			col_11.SortColumnId = (int) column_reporte.col_11;
			
			col_12 = new TreeViewColumn();
			cellr12 = new CellRendererText();
			col_12.Title = "Tipo Solicitud";
			col_12.PackStart(cellr12, true);
			col_12.AddAttribute (cellr12, "text", 12);
			cellr12.Foreground = "darkblue";
			col_12.SortColumnId = (int) column_reporte.col_12;
			
			lista_almacenes.AppendColumn(col_00);
			lista_almacenes.AppendColumn(col_01);
			lista_almacenes.AppendColumn(col_02);
			lista_almacenes.AppendColumn(col_03);
			lista_almacenes.AppendColumn(col_04);
			lista_almacenes.AppendColumn(col_06);
			lista_almacenes.AppendColumn(col_07);
			lista_almacenes.AppendColumn(col_08);
			lista_almacenes.AppendColumn(col_09);
			lista_almacenes.AppendColumn(col_10);
			lista_almacenes.AppendColumn(col_11);
			lista_almacenes.AppendColumn(col_12);
		}
		
		enum column_reporte
		{
			col_00,
			col_01,
			col_02,
			col_03,
			col_04,
			col_06,
			col_07,
			col_08,
			col_09,
			col_10,
			col_11,
			col_12
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
								"osiris_his_solicitudes_deta.pid_paciente AS pidpaciente,tipo_solicitud,"+
								"osiris_his_solicitudes_deta.nombre_paciente,procedimiento_qx,diagnostico_qx,observaciones_solicitud "+
								"FROM osiris_his_solicitudes_deta,osiris_almacenes,osiris_his_paciente "+								
								"WHERE osiris_his_solicitudes_deta.id_almacen = osiris_almacenes.id_almacen "+
								"AND osiris_his_solicitudes_deta.folio_de_solicitud > 0 "+
								"AND status = 'true' "+
								"AND osiris_his_paciente.pid_paciente = osiris_his_solicitudes_deta.pid_paciente "+
								""+query_fechas+" "+
								"GROUP BY osiris_his_solicitudes_deta.folio_de_solicitud,osiris_his_solicitudes_deta.id_almacen,"+
								"osiris_almacenes.descripcion_almacen,to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'yyyy-MM-dd HH24:mi'),osiris_his_solicitudes_deta.id_empleado,"+
								"osiris_his_solicitudes_deta.folio_de_servicio,"+
								"osiris_his_solicitudes_deta.pid_paciente,osiris_his_solicitudes_deta.nombre_paciente,procedimiento_qx,diagnostico_qx,observaciones_solicitud,osiris_his_solicitudes_deta.tipo_solicitud "+
								"ORDER BY to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'yyyy-MM-dd HH24:mi'),osiris_his_solicitudes_deta.id_almacen,osiris_his_solicitudes_deta.folio_de_solicitud;";
				//Console.WriteLine(comando.CommandText);
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
					                                (string) lector["nombre_paciente"].ToString().Trim(),
					                                 (string) lector["procedimiento_qx"].ToString().Trim(),
					                                 (string) lector["diagnostico_qx"].ToString().Trim(),
					                                (string) lector["observaciones_solicitud"].ToString().Trim(),
					                                (string) lector["tipo_solicitud"].ToString().Trim());
					col_00.SetCellDataFunc(cellr00, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_01.SetCellDataFunc(cellr01, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_02.SetCellDataFunc(cellr02, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_03.SetCellDataFunc(cellr03, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_04.SetCellDataFunc(cellr04, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					//col_05.SetCellDataFunc(cellr05, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_06.SetCellDataFunc(cellr06, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_07.SetCellDataFunc(cellr07, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_08.SetCellDataFunc(cellr08, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_09.SetCellDataFunc(cellr09, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_10.SetCellDataFunc(cellr10, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_11.SetCellDataFunc(cellr11, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_12.SetCellDataFunc(cellr12, new Gtk.TreeCellDataFunc(cambia_colores_fila));	
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		//ACCION QUE CAMBIA EL COLOR DEL TEXTO PARA CUANDO SE GUARDA EN LA BASE DE DATOS 
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{			
			if ((string) lista_almacenes.Model.GetValue (iter,12) == "URGENTE"){
				if(cell.GetType().ToString() == "Gtk.CellRendererToggle"){
					(cell as Gtk.CellRendererToggle).CellBackground = "red";
				}
				if(cell.GetType().ToString() == "Gtk.CellRendererText"){
					(cell as Gtk.CellRendererText).CellBackground = "red";
				}
			}else{
				if(cell.GetType().ToString() == "Gtk.CellRendererToggle"){
					(cell as Gtk.CellRendererToggle).CellBackground = "white";
				}
				if(cell.GetType().ToString() == "Gtk.CellRendererText"){
					(cell as Gtk.CellRendererText).CellBackground = "white";
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
 							numeros_seleccionado = numeros_seleccionado.Trim() + "','" + (string) lista_almacenes.Model.GetValue (iter,1);
 							almacenes_seleccionados = almacenes_seleccionados.Trim() + "','" + (string) lista_almacenes.Model.GetValue (iter,5);
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

