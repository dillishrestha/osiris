// created on 31/01/2010 at 10:00 am
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Pre-Programacion y Ajustes)
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
	public class movimientos_productos
	{
		// Boton general para salir de las ventanas
		[Widget] Gtk.Button button_salir = null;
		[Widget] Gtk.Button button_buscar_busqueda = null;
		[Widget] Gtk.Entry entry_expresion = null;
		[Widget] Gtk.Button button_selecciona = null;
		
		//Declaracion de ventana de busqueda de productos
		[Widget] Gtk.Window busca_producto = null;
		[Widget] Gtk.TreeView lista_de_producto = null;
		[Widget] Gtk.RadioButton radiobutton_nombre = null;
		[Widget] Gtk.RadioButton radiobutton_codigo = null;
		
		// Declarando ventana para ver productos enviados a los sub-almacenes
		[Widget] Gtk.Window mov_productos = null;
		[Widget] Gtk.Entry entry_descrip_producto = null;
		[Widget] Gtk.Entry entry_dia1 = null;                     
		[Widget] Gtk.Entry entry_mes1 = null;
		[Widget] Gtk.Entry entry_ano1 = null;
		[Widget] Gtk.Entry entry_dia2 = null;
		[Widget] Gtk.Entry entry_mes2 = null;
		[Widget] Gtk.Entry entry_ano2 = null;
		[Widget] Gtk.CheckButton checkbutton_todos_productos = null;
		[Widget] Gtk.CheckButton checkbutton_todos_departamentos = null;
		[Widget] Gtk.ComboBox combobox_departamentos = null;
		[Widget] Gtk.Button button_busca_producto = null;
		[Widget] Gtk.TreeView lista_producto_seleccionados = null;
		[Widget] Gtk.TreeView lista_resumen_productos = null;
		[Widget] Gtk.Button button_consultar_costos = null;
		[Widget] Gtk.Button button_imprimir_movimiento = null;
		[Widget] Gtk.Button button_quitar_producto = null;
		[Widget] Gtk.Label label136 = null;
		[Widget] Gtk.Entry entry1 = null;
		[Widget] Gtk.Label label137 = null;
		[Widget] Gtk.Entry entry2 = null;
		[Widget] Gtk.Entry entry_total_aplicado = null;
		[Widget] Gtk.ComboBox combobox_grupo;
		[Widget] Gtk.ComboBox combobox_grupo1;
		[Widget] Gtk.ComboBox combobox_grupo2;
		[Widget] Gtk.CheckButton checkbutton_grupo = null;
		[Widget] Gtk.CheckButton checkbutton_grupo1 = null;
		[Widget] Gtk.CheckButton checkbutton_grupo2 = null;
		[Widget] Gtk.Button button_filtros_grupos = null;
		
		string connectionString;						
		string nombrebd;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string tipo_reporte;
		
		string[] args_args = {""};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10};
		
		//string[] almacena_totales;
			    	
		string query_departamento = "AND osiris_his_tipo_admisiones.descripcion_admisiones = '0' ";
		int id_tipo_admisiones = 0; 
		string query_consulta = "" ;
		string rango_de_fecha = "";
		string titulopagina= "MOVIMIENTOS DE PRODUCOS";
		string sql_filtro_grupos = "";
		
		TreeStore treeViewEngineBusca2;	// Para la busqueda de Productos
		TreeStore treeViewEngineSelec;	// Lista de Productos seleccionados
		TreeStore treeViewEngineResumen;	// Lista de Productos seleccionados
		
		//Declaracion de ventana de error:
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public movimientos_productos (string LoginEmp_,string NomEmpleado_,string AppEmpleado_,string ApmEmpleado_,string nombrebd_,string tipo_reporte_,string query_sql_)
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			tipo_reporte = tipo_reporte_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
    		
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "mov_productos", null);
			gxml.Autoconnect (this);
			// Muestra ventana de Glade:
			mov_productos.Show();
			mov_productos.Title = "Movimientos de Productos";
			entry_dia1.Text = DateTime.Now.ToString("dd");
			entry_mes1.Text = DateTime.Now.ToString("MM");
			entry_ano1.Text = DateTime.Now.ToString("yyyy");				
			entry_dia2.Text = DateTime.Now.ToString("dd");
			entry_mes2.Text = DateTime.Now.ToString("MM");
			entry_ano2.Text = DateTime.Now.ToString("yyyy");
			entry_descrip_producto.IsEditable = false;
			label136.Hide();
			entry1.Hide();
			label137.Hide();
			entry2.Hide();
			llenado_combobox(0,"",combobox_grupo,"sql","SELECT * FROM osiris_grupo_producto ORDER BY descripcion_grupo_producto;","descripcion_grupo_producto","id_grupo_producto",args_args,args_id_array);
			llenado_combobox(0,"",combobox_grupo1,"sql","SELECT * FROM osiris_grupo1_producto ORDER BY descripcion_grupo1_producto;","descripcion_grupo1_producto","id_grupo1_producto",args_args,args_id_array);
			llenado_combobox(0,"",combobox_grupo2,"sql","SELECT * FROM osiris_grupo2_producto ORDER BY descripcion_grupo2_producto;","descripcion_grupo2_producto","id_grupo2_producto",args_args,args_id_array);
			
			if(tipo_reporte_ == "envios_subalamcenes"){
				crea_treeview_selec("envios_subalamcenes");
				mov_productos.Title = "Movimientos de Productos Enviados a los Sub-Almacenes";
				crea_treeview_envios();
				llenado_combobox(1,"",combobox_departamentos,"sql","SELECT * FROM osiris_almacenes WHERE activo = 'true' ORDER BY descripcion_almacen;","descripcion_almacen","id_almacen",args_args,args_id_array);
				button_consultar_costos.Clicked += new EventHandler(on_button_consultar_costos_clicked);
			}
			if(tipo_reporte == "cargos_pacientes"){
				crea_treeview_selec("cargos_pacientes");
				mov_productos.Title = "Cargos de Productos a Pacientes (Sub-Almacenes)";
				crea_treeview_cargos_pacientes();
				llenado_combobox(1,"",combobox_departamentos,"sql","SELECT * FROM osiris_his_tipo_admisiones "+
		               						"WHERE cuenta_mayor = 4000 "+
		               						"ORDER BY descripcion_admisiones;","descripcion_admisiones","id_tipo_admisiones",args_args,args_id_array);
				button_imprimir_movimiento.Clicked += new EventHandler(on_button_imprimir_movimiento_clicked);
				button_consultar_costos.Clicked += new EventHandler(on_button_consultar_costos_clicked);
			}
			if(tipo_reporte == "productos_requisados"){
				crea_treeview_selec("productos_requisados");
				mov_productos.Title = "Mov. Prod. Requisados de Almacen a Compras";
				crea_treeview_envios_subalmacen();
				llenado_combobox(1,"",combobox_departamentos,"sql","SELECT * FROM osiris_his_tipo_admisiones "+
						         						"ORDER BY descripcion_admisiones;","descripcion_admisiones","id_tipo_admisiones",args_args,args_id_array);
			}
			if(tipo_reporte == "cargos_x_fecha"){
				crea_treeview_selec("movimiento_totales_x_fecha");
				mov_productos.Title = "Mov.Tot. Prod. Cargados por Fechas";
				entry_descrip_producto.IsEditable = false;
				button_busca_producto.Sensitive = false;
				query_departamento = " AND osiris_grupo_producto.id_grupo_producto = '0' ";
				llenado_combobox(1,"",combobox_departamentos,"sql","SELECT * FROM osiris_grupo_producto "+
		               						"WHERE 	id_grupo_producto > '0' AND activo = 'true' "+
		               						"ORDER BY descripcion_grupo_producto;","descripcion_grupo_producto","id_grupo_producto",args_args,args_id_array);
				button_consultar_costos.Clicked += new EventHandler(on_button_consultar_costos_clicked);
				button_imprimir_movimiento.Clicked += new EventHandler(on_button_rpt_movcargosfecha_clicked);
			}
			if(tipo_reporte == "productos_comprados"){
				crea_treeview_selec("productos_comprados");
				mov_productos.Title = "Mov.Tot. Prod. Cargados por Fechas";
				entry_descrip_producto.IsEditable = false;
				button_busca_producto.Sensitive = false;
				llenado_combobox(1,"",combobox_departamentos,"sql","SELECT * FROM osiris_his_tipo_admisiones "+
						         						"ORDER BY descripcion_admisiones;","descripcion_admisiones","id_tipo_admisiones",args_args,args_id_array);
			}
			//  Sale de la ventana:
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			checkbutton_todos_departamentos.Clicked += new EventHandler(on_checkbutton_todos_departamentos_clicked);
			checkbutton_todos_productos.Clicked += new EventHandler(on_checkbutton_todos_productos_clicked);
			button_quitar_producto.Clicked += new EventHandler(on_button_quitar_producto_clicked);
			//button_consultar_costos.Clicked += new EventHandler(on_button_consultar_costos_clicked);
			entry_dia1.KeyPressEvent += onKeyPressEventactual;
			entry_mes1.KeyPressEvent += onKeyPressEventactual;
			entry_ano1.KeyPressEvent += onKeyPressEventactual;
			entry_dia2.KeyPressEvent += onKeyPressEventactual;
			entry_mes2.KeyPressEvent += onKeyPressEventactual;
			entry_ano2.KeyPressEvent += onKeyPressEventactual;
			combobox_grupo.Sensitive = false;
			combobox_grupo1.Sensitive = false;
			combobox_grupo2.Sensitive = false;
			button_filtros_grupos.Sensitive = false;
			checkbutton_grupo.Clicked += new EventHandler(on_checkbutton_grupos_clicked);
			checkbutton_grupo1.Clicked += new EventHandler(on_checkbutton_grupos_clicked);
			checkbutton_grupo2.Clicked += new EventHandler(on_checkbutton_grupos_clicked);
			button_filtros_grupos.Clicked += new EventHandler(on_button_filtros_grupos_clicked);
		}
		
		void on_checkbutton_grupos_clicked(object sender, EventArgs args)
		{
			Gtk.CheckButton checkbutton_recive = (object) sender as Gtk.CheckButton;
			if(checkbutton_recive.Name == "checkbutton_grupo"){
				button_filtros_grupos.Sensitive = (bool) checkbutton_grupo.Active;
				combobox_grupo.Sensitive = (bool) checkbutton_grupo.Active;
			}
			if(checkbutton_recive.Name == "checkbutton_grupo1"){
				combobox_grupo1.Sensitive = (bool) checkbutton_grupo1.Active;
			}
			if(checkbutton_recive.Name == "checkbutton_grupo2"){
				combobox_grupo2.Sensitive = (bool) checkbutton_grupo2.Active;
			}
		}
		
		void on_button_filtros_grupos_clicked(object sender, EventArgs args)
		{
			llenando_productos();
		}		
		
		void on_checkbutton_todos_departamentos_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todos_departamentos.Active == true){
				combobox_departamentos.Sensitive = false;
				query_departamento = "  "; //
		    }else{
				combobox_departamentos.Sensitive = true;
				if(tipo_reporte == "envios_subalamcenes"){
					query_departamento = "AND osiris_his_solicitudes_deta.id_almacen = '"+id_tipo_admisiones.ToString()+"' ";
				}
				if(tipo_reporte == "cargos_pacientes"){
					query_departamento = "AND osiris_erp_cobros_deta.id_tipo_admisiones = '"+id_tipo_admisiones.ToString()+"' ";
				}
				if(tipo_reporte == "productos_requisados"){
					query_departamento = "";
				}
				if(tipo_reporte == "cargos_x_fecha"){
					query_departamento = " AND osiris_grupo_producto.id_grupo_producto = '"+id_tipo_admisiones.ToString()+"' ";
				}
			}
		}
		
		void on_checkbutton_todos_productos_clicked (object sender, EventArgs args)
		{ 	
			if ( checkbutton_todos_productos.Active == true ){
				button_busca_producto.Sensitive = false;
				entry_descrip_producto.Sensitive = false;
			}else{
				button_busca_producto.Sensitive = true;
				entry_descrip_producto.Sensitive = true;
			}
		}
		
		void llenado_combobox(int tipodellenado,string descrip_defaul,object obj,string sql_or_array,string query_SQL,string name_field_desc,string name_field_id,string[] args_array,int[] args_id_array)
		{			
			Gtk.ComboBox combobox_llenado = (Gtk.ComboBox) obj;
			//Gtk.ComboBox combobox_pos_neg = obj as Gtk.ComboBox;
			combobox_llenado.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_llenado.PackStart(cell, true);
			combobox_llenado.AddAttribute(cell,"text",0);	        
			ListStore store = new ListStore( typeof (string),typeof (int));
			combobox_llenado.Model = store;			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) descrip_defaul,0);
			}			
			if(sql_or_array == "array"){			
				for (int colum_field = 0; colum_field < args_array.Length; colum_field++){
					store.AppendValues (args_array[colum_field],args_id_array[colum_field]);
				}
			}
			if(sql_or_array == "sql"){			
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					comando.CommandText = query_SQL;					
					NpgsqlDataReader lector = comando.ExecuteReader ();
					while (lector.Read()){
						store.AppendValues ((string) lector[ name_field_desc ], (int) lector[ name_field_id]);
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
				conexion.Close ();
			}			
			TreeIter iter;
			if (store.GetIterFirst(out iter)){
				combobox_llenado.SetActiveIter (iter);
			}
			combobox_llenado.Changed += new EventHandler (onComboBoxChanged_llenado);			
		}
		
		void onComboBoxChanged_llenado (object sender, EventArgs args)
		{
			ComboBox onComboBoxChanged = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (onComboBoxChanged.GetActiveIter (out iter)){
				switch (onComboBoxChanged.Name.ToString()){	
				case "combobox_departamentos":
					id_tipo_admisiones = (int) combobox_departamentos.Model.GetValue(iter,1);
					if(tipo_reporte == "envios_subalamcenes"){
						query_departamento = " AND osiris_his_solicitudes_deta.id_almacen = '"+Convert.ToString((int) combobox_departamentos.Model.GetValue(iter,1)).ToString()+"' ";	    			    	
					}
					if(tipo_reporte == "cargos_pacientes"){
						query_departamento = " AND osiris_erp_cobros_deta.id_tipo_admisiones = '"+Convert.ToString((int) combobox_departamentos.Model.GetValue(iter,1)).ToString()+"' ";
					}
					if(tipo_reporte == "productos_requisados"){
						query_departamento = "";
					}					
					if (tipo_reporte == "cargos_x_fecha"){
						query_departamento = " AND osiris_grupo_producto.id_grupo_producto = '"+Convert.ToString((int) combobox_departamentos.Model.GetValue(iter,1)).ToString()+"' ";
					}
					break;
				}
				//Console.WriteLine(query_departamento);
			}
		}
		
		void on_button_imprimir_movimiento_clicked(object sender, EventArgs args)
		{
			TreeIter iter;
			if (this.treeViewEngineResumen.GetIterFirst (out iter)){
				new osiris.imprime_mov_productos (entry_total_aplicado.Text,entry_dia1.Text,entry_mes1.Text,entry_ano1.Text,entry_dia2.Text,entry_mes2.Text,entry_ano2.Text, this.lista_resumen_productos,this.treeViewEngineResumen,query_consulta,this.nombrebd,this.titulopagina);
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, 
								ButtonsType.Close, "NO existe nada para imprimir");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
		}
		
		void on_button_rpt_movcargosfecha_clicked(object sender, EventArgs args)
		{
			TreeIter iter;
			if (treeViewEngineSelec.GetIterFirst (out iter)){
				new osiris.inventario_almacen_reporte (1,"","01","0000",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,tipo_reporte,query_consulta,
							entry_dia1.Text,entry_mes1.Text,entry_ano1.Text,entry_dia2.Text,entry_mes2.Text,entry_ano2.Text);
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, 
								ButtonsType.Close, "NO existe nada para imprimir");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
		}
		
		void on_button_quitar_producto_clicked (object o, EventArgs args)
		{
		   	TreeIter iter;
			TreeModel model;
			if (lista_producto_seleccionados.Selection.GetSelected (out model, out iter)) {
				treeViewEngineSelec.Remove (ref iter);
					
			}else{
 				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, 
					ButtonsType.Close, "NO existen productos para quitar o favor de seleccionar algun producto");
				msgBoxError.Run ();	msgBoxError.Destroy();
			}
		}
		
		/////LISTA_PRODUCTO_SELECCIONADOS/////
		void crea_treeview_selec(string nombre_reporte)
		{
			Console.WriteLine(nombre_reporte);
			if(nombre_reporte == "envios_subalamcenes" || nombre_reporte == "cargos_pacientes" || nombre_reporte == "productos_requisados" || nombre_reporte == "productos_comprados"){
				treeViewEngineSelec = new TreeStore(typeof(string), 
								typeof(string),typeof(string),typeof(string),typeof(string));												
				lista_producto_seleccionados.Model = treeViewEngineSelec;			
				lista_producto_seleccionados.RulesHint = true;
				
				TreeViewColumn col_00 = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_00.Title = "Codigo Prod."; // titulo de la cabecera de la columna, si está visible
				col_00.PackStart(cellr0, true);
				col_00.AddAttribute (cellr0, "text", 0);
	
				TreeViewColumn col_01 = new TreeViewColumn();
				CellRendererText cellr1 = new CellRendererText();
				col_01.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
				col_01.PackStart(cellr1, true);
				col_01.AddAttribute (cellr1, "text", 1);
				
				TreeViewColumn col_02 = new TreeViewColumn();
				CellRendererText cellr2 = new CellRendererText();
				col_02.Title = "Grupo"; // titulo de la cabecera de la columna, si está visible
				col_02.PackStart(cellr2, true);
				col_02.AddAttribute (cellr2, "text", 2);
				
				TreeViewColumn col_03 = new TreeViewColumn();
				CellRendererText cellr3 = new CellRendererText();
				col_03.Title = "Grupo 1"; // titulo de la cabecera de la columna, si está visible
				col_03.PackStart(cellr3, true);
				col_03.AddAttribute (cellr3, "text", 3);
				
				TreeViewColumn col_04 = new TreeViewColumn();
				CellRendererText cellr4 = new CellRendererText();
				col_04.Title = "Grupo 2"; // titulo de la cabecera de la columna, si está visible
				col_04.PackStart(cellr4, true);
				col_04.AddAttribute (cellr4, "text", 4);				
				
				lista_producto_seleccionados.AppendColumn(col_00);
				lista_producto_seleccionados.AppendColumn(col_01);
				lista_producto_seleccionados.AppendColumn(col_02);
				lista_producto_seleccionados.AppendColumn(col_03);
				lista_producto_seleccionados.AppendColumn(col_04);
			}
			if(nombre_reporte == "movimiento_totales_x_fecha"){
				treeViewEngineSelec = new TreeStore(typeof(string), 
								typeof(string),
								typeof(string),
								typeof(string),
								typeof(string),
								typeof(string),
								typeof(string),
								typeof(string));												
				lista_producto_seleccionados.Model = treeViewEngineSelec;			
				lista_producto_seleccionados.RulesHint = true;
				
				TreeViewColumn col_00 = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_00.Title = "Codigo Prod.";
				col_00.PackStart(cellr0, true);
				col_00.AddAttribute (cellr0, "text", 0);
	
				TreeViewColumn col_01 = new TreeViewColumn();
				CellRendererText cellr1 = new CellRendererText();
				col_01.Title = "Descripcion de Producto";
				col_01.PackStart(cellr1, true);
				col_01.AddAttribute (cellr1, "text", 1);
				col_01.Resizable = true;
				cellr1.Width = 250;
				
				TreeViewColumn col_02 = new TreeViewColumn();
				CellRendererText cellr2 = new CellRendererText();
				col_02.Title = "Cargados";
				col_02.PackStart(cellr2, true);
				col_02.AddAttribute (cellr2, "text", 2);
				
				TreeViewColumn col_03 = new TreeViewColumn();
				CellRendererText cellr3 = new CellRendererText();
				col_03.Title = "Precio Prod.";
				col_03.PackStart(cellr3, true);
				col_03.AddAttribute (cellr3, "text", 3);
				
				TreeViewColumn col_04 = new TreeViewColumn();
				CellRendererText cellr4 = new CellRendererText();
				col_04.Title = "Pack";
				col_04.PackStart(cellr4, true);
				col_04.AddAttribute (cellr4, "text", 4);
				
				TreeViewColumn col_05 = new TreeViewColumn();
				CellRendererText cellr5 = new CellRendererText();
				col_05.Title = "Costo Unit.";
				col_05.PackStart(cellr5, true);
				col_05.AddAttribute (cellr5, "text", 5);
				
				TreeViewColumn col_06 = new TreeViewColumn();
				CellRendererText cellr6 = new CellRendererText();
				col_06.Title = "$ Consumo";
				col_06.PackStart(cellr6, true);
				col_06.AddAttribute (cellr6, "text", 6);
				
				TreeViewColumn col_07 = new TreeViewColumn();
				CellRendererText cellr7 = new CellRendererText();
				col_07.Title = "Grupo Producto";
				col_07.PackStart(cellr7, true);
				col_07.AddAttribute (cellr7, "text", 7);
				
				lista_producto_seleccionados.AppendColumn(col_00);
				lista_producto_seleccionados.AppendColumn(col_01);
				lista_producto_seleccionados.AppendColumn(col_02);
				lista_producto_seleccionados.AppendColumn(col_03);
				lista_producto_seleccionados.AppendColumn(col_04);
				lista_producto_seleccionados.AppendColumn(col_05);
				lista_producto_seleccionados.AppendColumn(col_06);
				lista_producto_seleccionados.AppendColumn(col_07);
			}
		}
		
		void on_button_consultar_costos_clicked (object sender, EventArgs args)         
		{
			if(tipo_reporte != "cargos_x_fecha"){
				llenando_productos();
			}else{
				string query_fechas = "AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') >= '"+entry_ano1.Text.Trim()+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
							"AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') <= '"+entry_ano2.Text.Trim()+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";
				llena_treeview_aplicados(" ",query_fechas);
			}
		}
		
		void llenando_productos()
		{
			string query_fechas = "";
			string productos_seleccionado = "";
			string var_paso = "";
			string campo_filtrado = "";			
			if(tipo_reporte == "envios_subalamcenes"){
				query_fechas = "AND to_char(osiris_his_solicitudes_deta.fechahora_solicitud,'yyyy-MM-dd') >= '"+entry_ano1.Text.Trim()+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
					"AND to_char(osiris_his_solicitudes_deta.fechahora_solicitud,'yyyy-MM-dd') <= '"+entry_ano2.Text.Trim()+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";
				campo_filtrado = "AND osiris_his_solicitudes_deta.id_producto IN('";
			}
			if(tipo_reporte == "cargos_pacientes"){
				query_fechas = "AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') >= '"+entry_ano1.Text.Trim()+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
						"AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') <= '"+entry_ano2.Text.Trim()+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";
				campo_filtrado = "AND osiris_erp_cobros_deta.id_producto IN('";
			}
			if(tipo_reporte == "productos_requisados"){
				query_fechas = " ";
				campo_filtrado = " ";
			}
						
			// Validadndo que tenga algun producto seleccionado en la lista
			treeViewEngineResumen.Clear();
			TreeIter iter;
			if (this.checkbutton_todos_productos.Active == false ){
				if (this.treeViewEngineSelec.GetIterFirst (out iter)){
					if (id_tipo_admisiones != 0 || this.checkbutton_todos_departamentos.Active == true){ 
						// Llenando string de productos
						var_paso = (string) lista_producto_seleccionados.Model.GetValue (iter,0);
						productos_seleccionado = var_paso.Trim();							
			 			while (treeViewEngineSelec.IterNext(ref iter)){
		 					var_paso = (string) lista_producto_seleccionados.Model.GetValue (iter,0);
		 					productos_seleccionado += "','"+var_paso.Trim();
		 				}
		 				llena_treeview_aplicados(campo_filtrado+productos_seleccionado+"') ",query_fechas);					
					}else{
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, 
								ButtonsType.Close, "Seleccione un departamento");
						msgBoxError.Run ();				msgBoxError.Destroy();
					}					
				}
			}else{
				if (id_tipo_admisiones != 0 || this.checkbutton_todos_departamentos.Active == true){
 					llena_treeview_aplicados(" ",query_fechas);
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, 
								ButtonsType.Close, "seleccione un departamento");
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
			}
		}
		
		void llena_treeview_aplicados(string productos_seleccionado_,string query_fechas_)
		{	
			treeViewEngineSelec.Clear();
			query_consulta = "";
			float total_aplicado = 0;
			if(tipo_reporte == "envios_subalamcenes"){
				query_consulta =  "SELECT to_char(osiris_his_solicitudes_deta.id_producto,'999999999999') AS idproducto,descripcion_almacen," +
						"descripcion_producto,folio_de_solicitud," +
						"to_char(osiris_his_solicitudes_deta.pid_paciente,'9999999999') AS pidpaciente," +
						"osiris_his_solicitudes_deta.folio_de_servicio AS foliodeatencion,"+
						"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo,"+
						"cantidad_solicitada,to_char(osiris_his_solicitudes_deta.fechahora_solicitud,'dd-MM-yyyy HH24:mi') AS fechahorasolic," +
						"cantidad_autorizada,to_char(osiris_his_solicitudes_deta.fechahora_autorizado,'dd-MM-yyyy HH24:mi') AS fechahoraautoriz," +
						"osiris_his_solicitudes_deta.id_almacen "+
						"FROM osiris_his_solicitudes_deta,osiris_productos,osiris_almacenes,osiris_his_paciente " +
						"WHERE osiris_his_solicitudes_deta.id_producto = osiris_productos.id_producto "+
						"AND osiris_his_solicitudes_deta.id_almacen = osiris_almacenes.id_almacen " +
						"AND osiris_his_solicitudes_deta.pid_paciente = osiris_his_paciente.pid_paciente "+
						productos_seleccionado_+
						 query_fechas_+
						query_departamento+
						" ORDER BY folio_de_solicitud,osiris_his_solicitudes_deta.id_almacen;";
			}
			
			if(tipo_reporte == "cargos_pacientes"){
				query_consulta = "SELECT to_char(SUM(cantidad_aplicada),'999999999.99') AS cantidadaplicada,to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,descripcion_producto,"+
						"to_char(osiris_erp_cobros_deta.folio_de_servicio,'9999999999') AS foliodeservicio,to_char(osiris_erp_cobros_deta.fechahora_creacion,'dd-MM-yyyy HH24:mi') AS fechahoracreacion,"+
						"to_char(osiris_erp_cobros_deta.pid_paciente,'9999999999') AS pidpaciente,osiris_his_paciente.nombre1_paciente || ' ' || "+  
						"osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_paciente,"+
						"to_char(osiris_erp_cobros_deta.id_tipo_admisiones,'9999999999') AS idtipoadmisiones,descripcion_admisiones,descripcion_empresa "+
						"FROM osiris_erp_cobros_deta,osiris_productos,osiris_his_paciente,osiris_his_tipo_admisiones,osiris_erp_cobros_enca,osiris_empresas "+
						"WHERE osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto AND "+ 
						"osiris_erp_cobros_deta.pid_paciente = osiris_his_paciente.pid_paciente AND "+ 
						"osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones AND "+
						"osiris_erp_cobros_deta.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio AND "+
						"osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa AND "+
						"osiris_erp_cobros_deta.cantidad_aplicada > '0' AND "+
						"osiris_erp_cobros_deta.eliminado = false "+ 
						productos_seleccionado_+
						query_fechas_+
						query_departamento+
						"GROUP BY osiris_erp_cobros_deta.id_producto,descripcion_producto,osiris_erp_cobros_deta.folio_de_servicio,osiris_erp_cobros_deta.fechahora_creacion,osiris_erp_cobros_deta.pid_paciente,osiris_his_paciente.nombre1_paciente || ' ' || "+  
						"osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente,osiris_erp_cobros_deta.id_tipo_admisiones,descripcion_admisiones,descripcion_empresa "+
						"ORDER BY osiris_erp_cobros_deta.id_tipo_admisiones,osiris_erp_cobros_deta.id_producto,"+
						"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente;";
			}
			
			if(tipo_reporte == "productos_requisados"){
				query_consulta = "";	
			}
			
			if(tipo_reporte == "cargos_x_fecha"){
				query_consulta = "SELECT to_char(SUM(cantidad_aplicada),'999999999.99') AS cantidadaplicada," +
					"to_char(SUM(cantidad_aplicada),'999999999.99') AS stock," +
						"to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,descripcion_producto," +
						"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto,"+
						"to_char(osiris_productos.cantidad_de_embalaje,'999999999.99') AS embalaje,"+
						"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porcentageganancia,"+
						"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario," +
						"osiris_productos.id_grupo_producto AS idgrupoproducto,descripcion_grupo_producto "+						
						"FROM osiris_erp_cobros_deta,osiris_productos,osiris_grupo_producto,osiris_his_paciente,osiris_his_tipo_admisiones,osiris_erp_cobros_enca,osiris_empresas "+
						"WHERE osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+
						"AND osiris_erp_cobros_deta.pid_paciente = osiris_his_paciente.pid_paciente "+
						"AND osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
						"AND osiris_erp_cobros_deta.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
						"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+ 
						"AND osiris_erp_cobros_deta.cantidad_aplicada > '0' "+ 
						"AND osiris_erp_cobros_deta.eliminado = false " +
						"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto " +
						//"AND osiris_erp_cobros_deta.id_producto IN('52502300009','52502300008')
						query_fechas_+
						query_departamento+
						"GROUP BY osiris_erp_cobros_deta.id_producto,descripcion_producto,costo_producto,cantidad_de_embalaje,porcentage_ganancia,costo_por_unidad,osiris_productos.id_grupo_producto,descripcion_grupo_producto " +
						"ORDER BY osiris_productos.id_grupo_producto,descripcion_producto;";
			}
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
		            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = query_consulta;
				Console.WriteLine(query_consulta);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){					
					if(tipo_reporte == "envios_subalamcenes"){
						total_aplicado += float.Parse(((string) lector["cantidad_autorizada"].ToString().Trim()));
						treeViewEngineResumen.AppendValues ((string) lector["descripcion_producto"],
										(string) lector["idproducto"],
										(string) lector["folio_de_solicitud"].ToString(),
										(string) lector["descripcion_almacen"].ToString(),
										(string) lector["cantidad_solicitada"].ToString(),
										(string) lector["fechahorasolic"].ToString(),
										(string) lector["cantidad_autorizada"].ToString(),
										(string) lector["fechahoraautoriz"].ToString(),
										(string) lector["foliodeatencion"].ToString(),
										(string) lector["nombre_completo"].ToString(),
										(string) lector["pidpaciente"].ToString());
					}
					if(tipo_reporte == "cargos_pacientes"){
						total_aplicado += float.Parse(((string) lector["cantidadaplicada"]).ToString().Trim());
						treeViewEngineResumen.AppendValues ((string) lector["cantidadaplicada"],
										(string) lector["idproducto"],
										(string) lector["descripcion_producto"],
										(string) lector["foliodeservicio"],
										(string) lector["pidpaciente"],
										(string) lector["nombre_paciente"],
										(string) lector["idtipoadmisiones"],
										(string) lector["descripcion_admisiones"],
										(string) lector["fechahoracreacion"]);									
					}					
					if(tipo_reporte == "productos_requisados"){
					
					}
					
					if(tipo_reporte == "cargos_x_fecha"){
						total_aplicado += float.Parse((string) lector["costoproductounitario"].ToString()) * float.Parse((string) lector["cantidadaplicada"].ToString());
						treeViewEngineSelec.AppendValues ((string) lector["idproducto"],
										(string) lector["descripcion_producto"].ToString(),
										(string) lector["cantidadaplicada"].ToString(),
										string.Format("{0:C}",decimal.Parse((string) lector["costoproducto"].ToString())),
										string.Format("{0:F}",decimal.Parse((string) lector["embalaje"].ToString())),
										string.Format("{0:C}",decimal.Parse((string) lector["costoproductounitario"].ToString())),
										string.Format("{0:C}",decimal.Parse((string) lector["costoproductounitario"].ToString()) * decimal.Parse((string) lector["cantidadaplicada"].ToString())),
										(string) lector["descripcion_grupo_producto"].ToString());
					}
				}
				this.entry_total_aplicado.Text = total_aplicado.ToString(); 
			}catch (NpgsqlException ex){
	 			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error, 
												ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void crea_treeview_envios()
		{
			treeViewEngineResumen = new TreeStore(typeof(string), 
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
												
			lista_resumen_productos.Model = treeViewEngineResumen;
			lista_resumen_productos.RulesHint = true;
			TreeViewColumn col_producto = new TreeViewColumn();
			
			CellRendererText cellr0 = new CellRendererText();
			col_producto.Title = "Producto";
			col_producto.PackStart(cellr0, true);
			col_producto.AddAttribute (cellr0, "text", 0);
			col_producto.SortColumnId = (int) Column_resumen.col_producto;
            
			TreeViewColumn col_id_producto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_id_producto.Title = "Codigo";
			col_id_producto.PackStart(cellr1, true);
			col_id_producto.AddAttribute (cellr1, "text", 1);
			col_id_producto.SortColumnId = (int) Column_resumen.col_id_producto;
			
			TreeViewColumn col_nrosolicitud = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_nrosolicitud.Title = "N° Solicitud";
			col_nrosolicitud.PackStart(cellr2, true);
			col_nrosolicitud.AddAttribute (cellr2, "text", 2);
			col_nrosolicitud.SortColumnId = (int) Column_resumen.col_nrosolicitud;
			
			TreeViewColumn col_subalmacen = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_subalmacen.Title = "Sub-Almacen";
			col_subalmacen.PackStart(cellr3, true);
			col_subalmacen.AddAttribute (cellr3, "text", 3);
			col_subalmacen.SortColumnId = (int) Column_resumen.col_subalmacen;
			
			TreeViewColumn col_cantsolicitada = new TreeViewColumn();
			CellRendererText cellr4 = new CellRendererText();
			col_cantsolicitada.Title = "Canti. Solic.";
			col_cantsolicitada.PackStart(cellr4, true);
			col_cantsolicitada.AddAttribute (cellr4, "text", 4);
			col_cantsolicitada.SortColumnId = (int) Column_resumen.col_cantsolicitada;
			
			TreeViewColumn col_fechsolicitada = new TreeViewColumn();
			CellRendererText cellr5 = new CellRendererText();
			col_fechsolicitada.Title = "Fech.Solic.";
			col_fechsolicitada.PackStart(cellr5, true);
			col_fechsolicitada.AddAttribute (cellr5, "text", 5);
			col_fechsolicitada.SortColumnId = (int) Column_resumen.col_fechsolicitada;
			
			TreeViewColumn col_cantautorizada = new TreeViewColumn();
			CellRendererText cellr6 = new CellRendererText();
			col_cantautorizada.Title = "Cant.Autoriz."; // titulo de la cabecera de la columna, si está visible
			col_cantautorizada.PackStart(cellr6, true);
			col_cantautorizada.AddAttribute (cellr6, "text", 6);
			col_cantautorizada.SortColumnId = (int) Column_resumen.col_cantautorizada;
			
			TreeViewColumn col_fechautorizada = new TreeViewColumn();
			CellRendererText cellr7 = new CellRendererText();
			col_fechautorizada.Title = "Fech.Autoriz.";
			col_fechautorizada.PackStart(cellr7, true);
			col_fechautorizada.AddAttribute (cellr7, "text", 7);
			col_fechautorizada.SortColumnId = (int) Column_resumen.col_fechautorizada;
			
			TreeViewColumn col_nroatencion = new TreeViewColumn();
			CellRendererText cellr8 = new CellRendererText();
			col_nroatencion.Title = "N° Atencion";
			col_nroatencion.PackStart(cellr8, true);
			col_nroatencion.AddAttribute (cellr8, "text", 8);
			col_nroatencion.SortColumnId = (int) Column_resumen.col_nroatencion;
			
			TreeViewColumn col_nombre_paciente = new TreeViewColumn();
			CellRendererText cellr9 = new CellRendererText();
			col_nombre_paciente.Title = "Nombre Paciente";
			col_nombre_paciente.PackStart(cellr9, true);
			col_nombre_paciente.AddAttribute (cellr9, "text", 9);
			col_nombre_paciente.SortColumnId = (int) Column_resumen.col_nombre_paciente;
			
			TreeViewColumn col_nro_expediente = new TreeViewColumn();
			CellRendererText cellr10 = new CellRendererText();
			col_nro_expediente.Title = "N° Expediente";
			col_nro_expediente.PackStart(cellr10, true);
			col_nro_expediente.AddAttribute (cellr10, "text", 10);
			col_nro_expediente.SortColumnId = (int) Column_resumen.col_nro_expediente;
			
			lista_resumen_productos.AppendColumn(col_producto);
			lista_resumen_productos.AppendColumn(col_id_producto);
			lista_resumen_productos.AppendColumn(col_nrosolicitud);
			lista_resumen_productos.AppendColumn(col_subalmacen);
			lista_resumen_productos.AppendColumn(col_cantsolicitada);
			lista_resumen_productos.AppendColumn(col_fechsolicitada);
			lista_resumen_productos.AppendColumn(col_cantautorizada);       
			lista_resumen_productos.AppendColumn(col_fechautorizada);
			lista_resumen_productos.AppendColumn(col_nroatencion);
			lista_resumen_productos.AppendColumn(col_nombre_paciente);
			lista_resumen_productos.AppendColumn(col_nro_expediente);
		}
		
		//  lista_de_resumen:
		enum Column_resumen
		{        
			col_producto,
		    col_id_producto,
            col_nrosolicitud,
            col_subalmacen,
            col_cantsolicitada,
            col_fechsolicitada,
            col_cantautorizada,
            col_fechautorizada,
            col_nroatencion,
			col_nombre_paciente,
			col_nro_expediente
		}
		
				
		void crea_treeview_cargos_pacientes()
		{
			treeViewEngineResumen = new TreeStore(typeof(string), 
								typeof(string),
								typeof(string),
								typeof(string),
								typeof(string),
								typeof(string),
								typeof(string),
								typeof(string),
								typeof(string));
												
			lista_resumen_productos.Model = treeViewEngineResumen;
			
			lista_resumen_productos.RulesHint = true;
			
			TreeViewColumn col_cantidad = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_cantidad.Title = "Cantidad Aplicada"; // titulo de la cabecera de la columna, si está visible
			col_cantidad.PackStart(cellr0, true);
			col_cantidad.AddAttribute (cellr0, "text", 0);
			col_cantidad.SortColumnId = (int) Column_resumen1.col_cantidad;
			
			TreeViewColumn col_id_producto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_id_producto.Title = "ID Producto."; // titulo de la cabecera de la columna, si está visible
			col_id_producto.PackStart(cellr1, true);
			col_id_producto.AddAttribute (cellr1, "text", 1);
			col_id_producto.SortColumnId = (int) Column_resumen1.col_id_producto;
			
			TreeViewColumn col_descripcion = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_descripcion.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
			col_descripcion.PackStart(cellr2, true);
			col_descripcion.AddAttribute (cellr2, "text", 2);
			col_descripcion.SortColumnId = (int) Column_resumen1.col_descripcion;
			
			TreeViewColumn col_folio_de_servicio = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_folio_de_servicio.Title = "Num.Atencion"; // titulo de la cabecera de la columna, si está visible
			col_folio_de_servicio.PackStart(cellr3, true);
			col_folio_de_servicio.AddAttribute (cellr3, "text", 3);
			col_folio_de_servicio.SortColumnId = (int) Column_resumen1. col_folio_de_servicio;
			
			TreeViewColumn col_pid_paciente = new TreeViewColumn();
			CellRendererText cellr4 = new CellRendererText();
			col_pid_paciente.Title = "PID Paciente"; // titulo de la cabecera de la columna, si está visible
			col_pid_paciente.PackStart(cellr4, true);
			col_pid_paciente.AddAttribute (cellr4, "text", 4);
			col_pid_paciente.SortColumnId = (int) Column_resumen1. col_pid_paciente;
			
			TreeViewColumn col_nombre_paciente = new TreeViewColumn();
			CellRendererText cellr5 = new CellRendererText();
			col_nombre_paciente.Title = "Nombre Paciente"; // titulo de la cabecera de la columna, si está visible
			col_nombre_paciente.PackStart(cellr5, true);
			col_nombre_paciente.AddAttribute (cellr5, "text", 5);
			col_nombre_paciente.SortColumnId = (int) Column_resumen1. col_nombre_paciente;
			
			TreeViewColumn col_id_departamento = new TreeViewColumn();
			CellRendererText cellr6 = new CellRendererText();
			col_id_departamento.Title = "id Departamento"; // titulo de la cabecera de la columna, si está visible
			col_id_departamento.PackStart(cellr6, true);
			col_id_departamento.AddAttribute (cellr6, "text", 6);
			col_id_departamento.SortColumnId = (int) Column_resumen1.col_id_departamento;
			
			TreeViewColumn col_departamento = new TreeViewColumn();
			CellRendererText cellr7 = new CellRendererText();
			col_departamento.Title = "Departamento"; // titulo de la cabecera de la columna, si está visible
			col_departamento.PackStart(cellr7, true);
			col_departamento.AddAttribute (cellr7, "text", 7);
			col_departamento.SortColumnId = (int) Column_resumen1.col_departamento;
			
			TreeViewColumn col_fecha_cargo = new TreeViewColumn();
			CellRendererText cellr8 = new CellRendererText();
			col_fecha_cargo.Title = "Fecha de Cargo"; // titulo de la cabecera de la columna, si está visible
			col_fecha_cargo.PackStart(cellr8, true);
			col_fecha_cargo.AddAttribute (cellr8, "text", 8);
			col_fecha_cargo.SortColumnId = (int) Column_resumen1.col_fecha_cargo;
			
			lista_resumen_productos.AppendColumn(col_cantidad);
			lista_resumen_productos.AppendColumn(col_id_producto);
			lista_resumen_productos.AppendColumn(col_descripcion);
			lista_resumen_productos.AppendColumn(col_folio_de_servicio); //num atencion
			lista_resumen_productos.AppendColumn(col_pid_paciente);
			lista_resumen_productos.AppendColumn(col_nombre_paciente);
			lista_resumen_productos.AppendColumn(col_id_departamento); //(id_tipo_admision);       
			lista_resumen_productos.AppendColumn(col_departamento);   // (descripcion_admision)
			lista_resumen_productos.AppendColumn(col_fecha_cargo);
		}
		
		enum Column_resumen1
		{        
			col_cantidad, //
			col_id_producto,
			col_descripcion,
			col_folio_de_servicio, //num atencion
			col_pid_paciente,
			col_nombre_paciente,
			col_id_departamento, //(tipo_admision),
			col_departamento,   //descripcion-admicion
			col_fecha_cargo
		}
		
		void crea_treeview_envios_subalmacen()
		{
			treeViewEngineResumen = new TreeStore(typeof(string), 
								typeof(string),
								typeof(string),
								typeof(string),
								typeof(string),
								typeof(string),
								typeof(string),
								typeof(string),
								typeof(string));
												
			lista_resumen_productos.Model = treeViewEngineResumen;
			
			lista_resumen_productos.RulesHint = true;
		}
		
		////////////////////////////////////////VENTANA BUSQUEDA DE PRODUCTOS/////////////////////////////////////////////////	
		void on_button_busca_producto_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda("producto");
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enterbucar_busqueda;
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
								
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta la final de la classe
		}
		
		/////////BUSQUEDA DE PRODUCTOS(lista_de_producto)///////////////////
		void crea_treeview_busqueda(string tipo_busqueda)
		{ 
			if (tipo_busqueda == "producto"){
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
								 typeof(string));
						
				lista_de_producto.Model = treeViewEngineBusca2;
				
				lista_de_producto.RulesHint = true;
				
				lista_de_producto.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono paciente*/
					
				TreeViewColumn col_idproducto = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_idproducto.Title = "ID Producto"; // titulo de la cabecera de la columna, si está visible
				col_idproducto.PackStart(cellr0, true);
				col_idproducto.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
				col_idproducto.SortColumnId = (int) Column_prod.col_idproducto;
			
				TreeViewColumn col_desc_producto = new TreeViewColumn();
				CellRendererText cellr1 = new CellRendererText();
				col_desc_producto.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
				col_desc_producto.PackStart(cellr1, true);
				col_desc_producto.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
				col_desc_producto.SortColumnId = (int) Column_prod.col_desc_producto;
				//cellr0.Editable = true;   // Permite edita este campo
	       
				TreeViewColumn col_precioprod = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_precioprod.Title = "Precio Producto";
				col_precioprod.PackStart(cellrt2, true);
				col_precioprod.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
				col_precioprod.SortColumnId = (int) Column_prod.col_precioprod;
	       
				TreeViewColumn col_ivaprod = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_ivaprod.Title = "I.V.A.";
				col_ivaprod.PackStart(cellrt3, true);
				col_ivaprod.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 2 en vez de 3
				col_ivaprod.SortColumnId = (int) Column_prod.col_ivaprod;
	       
				TreeViewColumn col_totalprod = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_totalprod.Title = "Total";
				col_totalprod.PackStart(cellrt4, true);
				col_totalprod.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 3 en vez de 4
				col_totalprod.SortColumnId = (int) Column_prod.col_totalprod;
	       
				TreeViewColumn col_descuentoprod = new TreeViewColumn();
				CellRendererText cellrt5 = new CellRendererText();
				col_descuentoprod.Title = "% Descuento";
				col_descuentoprod.PackStart(cellrt5, true);
				col_descuentoprod.AddAttribute (cellrt5, "text", 5); // la siguiente columna será 5 en vez de 6
				col_descuentoprod.SortColumnId = (int) Column_prod.col_descuentoprod;
	 
				TreeViewColumn col_preciocondesc = new TreeViewColumn();
				CellRendererText cellrt6 = new CellRendererText();
				col_preciocondesc.Title = "$Descuento sin IVA";
				col_preciocondesc.PackStart(cellrt6, true);
				col_preciocondesc.AddAttribute (cellrt6, "text", 6);     // la siguiente columna será 6 en vez de 7
				col_preciocondesc.SortColumnId = (int) Column_prod.col_preciocondesc;
	       
				TreeViewColumn col_grupoprod = new TreeViewColumn();
				CellRendererText cellrt7 = new CellRendererText();
				col_grupoprod.Title = "Grupo Producto";
				col_grupoprod.PackStart(cellrt7, true);
				col_grupoprod.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 7 en vez de 8
				col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
	       
				TreeViewColumn col_grupo1prod = new TreeViewColumn();
				CellRendererText cellrt8 = new CellRendererText();
				col_grupo1prod.Title = "Grupo1 Producto";
				col_grupo1prod.PackStart(cellrt8, true);
				col_grupo1prod.AddAttribute (cellrt8, "text", 8); // la siguiente columna será 8 en vez de 9
				col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
	                   
				TreeViewColumn col_grupo2prod = new TreeViewColumn();
				CellRendererText cellrt9 = new CellRendererText();
				col_grupo2prod.Title = "Grupo2 Producto";
				col_grupo2prod.PackStart(cellrt9, true);
				col_grupo2prod.AddAttribute (cellrt9, "text", 9); // la siguiente columna será 8 en vez de 9
				col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;

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
			}
		}
		
		//  lista_de_productos:
		enum Column_prod
		{
			col_idproducto,
			col_desc_producto,
			col_precioprod,
			col_ivaprod,
			col_totalprod,
			col_descuentoprod,
			col_preciocondesc,
			col_grupoprod,
			col_grupo1prod,
			col_grupo2prod
		}
		
		////////// llena la lista de productos//////////////////////////////
	 	void on_llena_lista_producto_clicked (object sender, EventArgs args)
	 	{       
	 		llenando_lista_de_productos();
	 	}
	 		
	 	void llenando_lista_de_productos()
	 	{
			treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			//Verifica que la base de datos este conectada
			string query_tipo_busqueda = "";			
			if(radiobutton_nombre.Active == true) {
				query_tipo_busqueda = "AND osiris_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_producto; ";
			}			
			if(radiobutton_codigo.Active == true){
				query_tipo_busqueda = "AND osiris_productos.id_producto LIKE '"+entry_expresion.Text.Trim()+"%'  ORDER BY id_producto; ";
			}	           
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
									"osiris_productos.descripcion_producto,to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
									"to_char(precio_producto_publico1,'99999999.99') AS preciopublico1,"+
									"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,"+
									"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
									"to_char(porcentage_ganancia,'99999.99') AS porcentageutilidad,to_char(costo_producto,'999999999.99') AS costoproducto, "+
									"osiris_grupo_producto.agrupacion "+
									"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
									"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
									"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
									"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
									"AND cobro_activo = 'true' "+
						            query_tipo_busqueda;
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//Console.WriteLine(comando.CommandText.ToString());
						
				float tomaprecio;
				float calculodeiva;
				float preciomasiva;
				float tomadescue;
				float preciocondesc;
																
				while (lector.Read()){
					calculodeiva = 0;
					preciomasiva = 0;
					
					tomaprecio = float.Parse((string) lector["preciopublico"]);
										
					tomadescue = float.Parse((string) lector["porcentagesdesc"]);
					preciocondesc = tomaprecio;
					
					if ((bool) lector["aplicar_iva"]){
						calculodeiva = (tomaprecio * float.Parse(classpublic.ivaparaaplicar))/100;
					}
					if ((bool) lector["aplica_descuento"]){
						preciocondesc = tomaprecio-((tomaprecio*tomadescue)/100);
					}
					preciomasiva = tomaprecio + calculodeiva; 
					treeViewEngineBusca2.AppendValues (
									(string) lector["codProducto"] ,
									(string) lector["descripcion_producto"],
									tomaprecio.ToString("F").PadLeft(10),
									calculodeiva.ToString("F").PadLeft(10),
									preciomasiva.ToString("F").PadLeft(10),
									(string) lector["porcentagesdesc"],
									preciocondesc.ToString("F").PadLeft(10),
									(string) lector["descripcion_grupo_producto"],
									(string) lector["descripcion_grupo1_producto"],
									(string) lector["descripcion_grupo2_producto"],
									(string) lector["costoproductounitario"],
									(string) lector["porcentageutilidad"],
									(string) lector["costoproducto"],
									(string) lector["agrupacion"]);
					
				}
			}catch (NpgsqlException ex){
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
									ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		////////////////BUTTON SELECCIONA//////////////////////////////////
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;

			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
		 		entry_descrip_producto.Text = (string) model.GetValue(iterSelected, 1);
				treeViewEngineSelec.AppendValues ((string) model.GetValue(iterSelected, 0),(string) model.GetValue(iterSelected, 1));
			}else{
 				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, 
							ButtonsType.Close, "NO existen productos para seleccionar");
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
		}
		
	 	[GLib.ConnectBefore ()] 
		public void onKeyPressEvent_enterbucar_busqueda(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llenando_lista_de_productos();
				//Console.WriteLine ("key press");
								
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza en ventana de carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEventactual(object o, Gtk.KeyPressEventArgs args)
		{
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"  && args.Event.Key != Gdk.Key.Delete)
			{
				args.RetVal = true;
			}
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}
