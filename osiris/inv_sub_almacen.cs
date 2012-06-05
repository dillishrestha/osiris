////////////////////////////////////////////////////////////
// created on 05/03/2008 at 04:30 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Tec. Homero Montoya Galvan (Programacion)
//				  Ing. Daniel Olivares (Preprogramacion)
// 				  
// Licencia		: GLP
// S.O. 		: GNU/Linux
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
	public class inventario_sub_almacen
	{
		// Declarando ventana de inventario sub almacen
		[Widget] Gtk.Window inventario_sub_almacenes;
		[Widget] Gtk.ComboBox combobox_tipo_almacen;
		[Widget] Gtk.ComboBox combobox_grupo;
		[Widget] Gtk.ComboBox combobox_grupo1;
		[Widget] Gtk.ComboBox combobox_grupo2;
		[Widget] Gtk.ComboBox combobox_almacen_destino;
		[Widget] Gtk.CheckButton checkbutton_sin_stock;
		[Widget] Gtk.CheckButton checkbutton_articulos_con_stock;
		[Widget] Gtk.CheckButton checkbutton_enviar_articulos;
		[Widget] Gtk.CheckButton checkbutton_ajuste_de_articulos;
		[Widget] Gtk.TreeView lista_almacenes;
		[Widget] Gtk.Button button_actualizar;
		[Widget] Gtk.Button button_imprimir;
		[Widget] Gtk.Button button_salir;
		[Widget] Gtk.Button button_enviar;
		[Widget] Gtk.Button button_quitar;
		[Widget] Gtk.Button button_imprime_traspaso;
		[Widget] Gtk.Label label_almacen_destino;
		[Widget] Gtk.Label numtraspaso;
		[Widget] Gtk.Entry entry_numero_de_traspaso;
		[Widget] Gtk.Entry entry_filter = null;
		[Widget] Gtk.Statusbar statusbar_inv_sub_hosp;
		
		string connectionString;
		string nombrebd;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;		
		
		int idsubalmacen;
		int idalmacendestino;
		string descsubalmacen;
		int tipoalmacen;
				
		int idtipogrupo = 0;
		int idtipogrupo1 = 0;
		int idtipogrupo2 = 0;
		int idalmacen = 0;
		string descripgrupo = "";
		string descripgrupo1 =  "";
		string descripgrupo2 = "";
		string tiposeleccion = "";
		string descripcionalmacen = "";
		
		string query_sql = "SELECT osiris_catalogo_almacenes.id_almacen," +
							"to_char(osiris_productos.id_producto,'999999999999') AS idproducto,"+
							"osiris_productos.descripcion_producto, "+
							"to_char(osiris_catalogo_almacenes.stock,'999999999999.99') AS stock,"+
							"to_char(osiris_catalogo_almacenes.minimo_stock,'999999999999.99') AS minstock,"+
							"to_char(osiris_catalogo_almacenes.maximo,'999999999999.99') AS maxstock,"+
							"to_char(osiris_catalogo_almacenes.punto_de_reorden,'999999999999.99') AS reorden,"+
							"to_char(osiris_catalogo_almacenes.fechahora_ultimo_surtimiento,'yyyy-MM-dd HH24:mi:ss') AS fechsurti, "+
							"osiris_productos.id_grupo_producto AS idgrupoproducto,descripcion_grupo_producto,"+ //descripcion_grupo1_producto,descripcion_grupo2_producto, "+
							"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario,"+
							"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto,"+
							"to_char(osiris_productos.cantidad_de_embalaje,'999999999.99') AS embalaje, "+
							"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porcentageganancia, "+
							"osiris_catalogo_almacenes.tiene_stock "+
							"FROM osiris_catalogo_almacenes,osiris_productos,osiris_grupo_producto "+ //,osiris_grupo1_producto,osiris_grupo2_producto "+
							"WHERE osiris_catalogo_almacenes.id_producto = osiris_productos.id_producto "+ 
							"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
							"AND osiris_productos.cobro_activo = 'true' "+
							//"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
							//"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
							"AND osiris_grupo_producto.agrupacion_4 = 'true'"+
							"AND osiris_catalogo_almacenes.eliminado = 'false' ";
		string query_grupo = " ";
		string query_grupo1 = " ";
		string query_grupo2 = " ";
		string query_stock = " ";
		string tiporeporte = "STOCK";
		string titulo = "REPORTE DE STOCK HOSPITALIZACION";
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		private ListStore treeViewEngineBusca;
		private ListStore treeViewEngineBusca2;
		Gtk.TreeModelFilter filter;
		
		//declaracion de columnas y celdas de treeview de busqueda
		TreeViewColumn col_descrip;		CellRendererText cellrt0;
		TreeViewColumn col_existencia;	CellRendererText cellrt1;
		TreeViewColumn col_codigo;		CellRendererText cellrt2;
		TreeViewColumn col_minimo;		CellRendererText cellrt3;
		TreeViewColumn col_maximo;		CellRendererText cellrt4;
		TreeViewColumn col_reorden;		CellRendererText cellrt5;
		TreeViewColumn col_fecha;		CellRendererText cellrt6;
		TreeViewColumn col_embalaje;	CellRendererText cellrt7;
		TreeViewColumn col_enviar;       //public CellRendererText cellrt8;
		TreeViewColumn col_cantenviar;   //public CellRendererText cellrt9;	
		TreeViewColumn col_descripcion;  CellRendererText cellrt8;
		TreeViewColumn col_costo;
		TreeViewColumn col_cantidad;
		TreeViewColumn col_precio;
		TreeViewColumn col_quitar;		CellRendererToggle cel_quitar;
		TreeViewColumn col_es_stock;	CellRendererToggle cel_es_stock;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		string[] args_args = {""};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8};
	
		public inventario_sub_almacen(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, 
									string ApmEmpleado_, string nombrebd_, int _idsubalmacen_, string _descsubalmacen_, int tipoalmacen_)
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			idsubalmacen = _idsubalmacen_;
			descsubalmacen =_descsubalmacen_;
			tipoalmacen = tipoalmacen_;   // 1 = inventario sub-almacenes   2 = Traspasos de Sub-Almacenes   3 = AMBOS para almacen general
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "inventario_sub_almacenes", null);
			gxml.Autoconnect (this);
			inventario_sub_almacenes.Show();
			//Console.WriteLine(descsubalmacen+"   almacen");
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_imprimir.Clicked += new EventHandler(imprime_reporte_stock);
			button_imprime_traspaso.Clicked += new EventHandler(imprime_reporte_traspaso);
			button_actualizar.Clicked += new EventHandler(actualizar);
			checkbutton_sin_stock.Clicked += new EventHandler(on_checkbutton_articulos_sin_stock_clicked);
			checkbutton_articulos_con_stock.Clicked += new EventHandler(on_checkbutton_articulos_con_stock_clicked);
			checkbutton_enviar_articulos.Clicked += new EventHandler(on_checkbutton_enviar_clicked);
			checkbutton_ajuste_de_articulos.Clicked += new EventHandler(on_checkbutton_enviar_clicked);
			button_enviar.Clicked += new EventHandler(on_button_enviar_articulos_clicked);
			button_quitar.Clicked += new EventHandler(on_button_quitar_clicked);
			entry_numero_de_traspaso.KeyPressEvent += onKeyPressEvent_numero_traspaso;
			
			llenado_combobox(1,"",combobox_grupo,"sql","SELECT * FROM osiris_grupo_producto ORDER BY descripcion_grupo_producto;","descripcion_grupo_producto","id_grupo_producto",args_args,args_id_array);
			llenado_combobox(1,"",combobox_grupo1,"sql","SELECT * FROM osiris_grupo1_producto ORDER BY descripcion_grupo1_producto;","descripcion_grupo1_producto","id_grupo1_producto",args_args,args_id_array);
			llenado_combobox(1,"",combobox_grupo2,"sql","SELECT * FROM osiris_grupo2_producto ORDER BY descripcion_grupo2_producto;","descripcion_grupo2_producto","id_grupo2_producto",args_args,args_id_array);
	
			statusbar_inv_sub_hosp.Pop(0);
			statusbar_inv_sub_hosp.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_inv_sub_hosp.HasResizeGrip = false;
			
			if(this.tipoalmacen==1){
				this.checkbutton_enviar_articulos.Hide();
				this.label_almacen_destino.Hide();
				this.combobox_almacen_destino.Hide();
				this.button_enviar.Hide();
				this.numtraspaso.Hide();
				this.entry_numero_de_traspaso.Hide();
				this.checkbutton_ajuste_de_articulos.Hide();  
				this.query_stock = "";
				crea_treeview_inventarios();
				entry_filter.Changed += OnFilterEntryTextChanged;
				//INVENTARIO SUB ALMACENES 
			}
			
			if(this.tipoalmacen==2){
				this.combobox_tipo_almacen.Sensitive = false;
				this.combobox_grupo.Sensitive = false;
				this.combobox_grupo1.Sensitive = false;
				this.combobox_grupo2.Sensitive = false;
				this.checkbutton_articulos_con_stock.Hide();
				this.checkbutton_sin_stock.Hide();
				this.button_actualizar.Hide();
				this.button_quitar.Hide();
				this.checkbutton_ajuste_de_articulos.Hide();
				this.query_stock = " AND osiris_catalogo_almacenes.stock > 0";
				crea_treeview_traspaso();
				entry_filter.Changed += OnFilterEntryTextChanged;
				//TRASPASO DE SUB ALMACENES
			}
			
			if(this.tipoalmacen == 3){
				this.button_quitar.Hide();
				this.query_stock = " AND osiris_catalogo_almacenes.stock > 0";
				crea_treeview_traspaso();
				entry_filter.Changed += OnFilterEntryTextChanged;
				//TODAS LAS OPCIONES
			}

			llenado_combobox(1,"",combobox_tipo_almacen,"sql","SELECT id_almacen,descripcion_almacen,sub_almacen FROM osiris_almacenes WHERE sub_almacen = 'true' ORDER BY descripcion_almacen;","descripcion_almacen","id_almacen",args_args,args_id_array);
			llenado_combobox(1,"",combobox_almacen_destino,"sql","SELECT id_almacen,descripcion_almacen,sub_almacen FROM osiris_almacenes WHERE sub_almacen = 'true' ORDER BY descripcion_almacen;","descripcion_almacen","id_almacen",args_args,args_id_array);
			
			llenando_busqueda_productos();
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
				case "combobox_tipo_almacen":
					idsubalmacen = (int) combobox_tipo_almacen.Model.GetValue(iter,1);
					descripcionalmacen = (string) combobox_tipo_almacen.Model.GetValue(iter,0);
					entry_filter.Text = "";
		    		llenando_busqueda_productos();
					break;
				case "combobox_almacen_destino":
					idalmacendestino = (int) this.combobox_almacen_destino.Model.GetValue(iter,1);
					break;
				case "combobox_grupo":
			    	idtipogrupo = (int) combobox_grupo.Model.GetValue(iter,1);
			    	descripgrupo = (string) combobox_grupo.Model.GetValue(iter,0);
			    	query_grupo = "AND osiris_productos.id_grupo_producto = '"+idtipogrupo.ToString()+"' ";
			    	this.llenando_busqueda_productos();
					break;
				case "combobox_grupo1":
					idtipogrupo1 = (int) combobox_grupo1.Model.GetValue(iter,1);
		    		descripgrupo1 = (string) combobox_grupo1.Model.GetValue(iter,0);
		    		query_grupo1 = "AND osiris_productos.id_grupo1_producto = '"+idtipogrupo1.ToString()+"' ";
		    		llenando_busqueda_productos();
					break;
				case "combobox_grupo2":
					idtipogrupo2 = (int) combobox_grupo2.Model.GetValue(iter,1);
		    		descripgrupo2 = (string) combobox_grupo2.Model.GetValue(iter,0);
		    		query_grupo2 = "AND osiris_productos.id_grupo2_producto = '"+idtipogrupo2.ToString()+"' ";
		    		llenando_busqueda_productos();
					break;				
				}
			}
		}
		
		
		void crea_treeview_traspaso()
		{			
			treeViewEngineBusca2 = new ListStore(typeof(bool),
												typeof(string),
			                                    typeof(string),
			                                    typeof(string),
			                                    typeof(string),
			                                    typeof(string),
			                                    typeof(string),
			                                    typeof(string),
			                                    typeof(string),
			                                    typeof(string));
			
			lista_almacenes.Model = treeViewEngineBusca2;			
			lista_almacenes.RulesHint = true;			
			//lista_almacenes.RowActivated += on_selecciona_almacen_clicked;  // Doble click selecciono paciente
			
			TreeViewColumn col_surtir = new TreeViewColumn();
			CellRendererToggle cel_surtir = new CellRendererToggle();
			col_surtir.Title = "Surtir"; // titulo de la cabecera de la columna, si está visible
			col_surtir.PackStart(cel_surtir, true);
			col_surtir.AddAttribute (cel_surtir, "active", 0);
			cel_surtir.Activatable = true;
			cel_surtir.Toggled += selecciona_fila;
			col_surtir.SortColumnId = (int) Col_traspaso.col_surtir;
			
			TreeViewColumn col_autorizado = new TreeViewColumn();
			CellRendererText cel_autorizado = new CellRendererText();
			col_autorizado.Title = "Autorizado/Ajuste"; // titulo de la cabecera de la columna, si está visible
			col_autorizado.PackStart(cel_autorizado, true);
			col_autorizado.AddAttribute (cel_autorizado, "text", 1);
			col_autorizado.SortColumnId = (int) Col_traspaso.col_autorizado;
			cel_autorizado.Editable = true;
			cel_autorizado.Edited += NumberCellEdited_Autorizado;
			
			col_existencia = new TreeViewColumn();
			cellrt1 = new CellRendererText();
			col_existencia.Title = "Existencia"; // titulo de la cabecera de la columna, si está visible
			col_existencia.PackStart(cellrt1, true);
			col_existencia.AddAttribute (cellrt1, "text", 2);    // la siguiente columna será 1 en vez de 1
			col_existencia.SortColumnId = (int) Col_traspaso.col_existencia;
			
			col_codigo = new TreeViewColumn();
			cellrt2 = new CellRendererText();
			col_codigo.Title = "Codigo";
			col_codigo.PackStart(cellrt2, true);
			col_codigo.AddAttribute (cellrt2, "text", 3); // la siguiente columna será 1 en vez de 2
			col_codigo.SortColumnId = (int) Col_traspaso.col_codigo;
			
			col_descripcion = new TreeViewColumn();
			cellrt8 = new CellRendererText();
			col_descripcion.Title = "Descripcion";
			col_descripcion.PackStart(cellrt8, true);
			col_descripcion.AddAttribute (cellrt8, "text", 4);
			col_descripcion.SortColumnId = (int) Col_traspaso.col_descripcion;
			
			col_minimo = new TreeViewColumn();
			cellrt3 = new CellRendererText();
			col_minimo.Title = "Minimo Stock";
			col_minimo.PackStart(cellrt3, true);
			col_minimo.AddAttribute (cellrt3, "text", 7);
			col_minimo.SortColumnId = (int) Col_traspaso.col_minimo;
			cellrt3.Editable = true;
			
			col_maximo = new TreeViewColumn();
			cellrt4 = new CellRendererText();
			col_maximo.Title = "Maximo Stock";
			col_maximo.PackStart(cellrt4, true);
			col_maximo.AddAttribute (cellrt4, "text", 8);
			col_maximo.SortColumnId = (int) Col_traspaso.col_maximo;
			cellrt4.Editable = true;
			
			col_reorden = new TreeViewColumn();
			cellrt5 = new CellRendererText();
			col_reorden.Title = "Punto de Reorden";
			col_reorden.PackStart(cellrt5, true);
			col_reorden.AddAttribute (cellrt5, "text", 9);
			col_reorden.SortColumnId = (int) Col_traspaso.col_reorden;
			cellrt5.Editable = true;
						
			lista_almacenes.AppendColumn(col_surtir);
			lista_almacenes.AppendColumn(col_autorizado);
			lista_almacenes.AppendColumn(col_existencia);
			lista_almacenes.AppendColumn(col_codigo);
			lista_almacenes.AppendColumn(col_descripcion);
			lista_almacenes.AppendColumn(col_minimo);
			lista_almacenes.AppendColumn(col_maximo);
			lista_almacenes.AppendColumn(col_reorden);			
		}
		
		enum Col_traspaso
		{
			col_surtir,
			col_autorizado,
			col_existencia,
			col_codigo,
			col_descripcion,
			col_minimo,
			col_maximo,
			col_reorden
		}
		
		void on_button_quitar_clicked (object sender, EventArgs args)		
		{
			if(LoginEmpleado =="DOLIVARES" || LoginEmpleado =="ADMIN" || LoginEmpleado == "ROLVEDAFLORES" || LoginEmpleado == "AGUTIERREZV"){
					MessageDialog msgBox2 = new MessageDialog (MyWin,DialogFlags.Modal,
						                 MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de Borrar los Materiales Seleccionados?");
					ResponseType miResultado2 = (ResponseType)msgBox2.Run ();
				msgBox2.Destroy();					
				if (miResultado2 == ResponseType.Yes){
					TreeIter iterSelected;
					if(this.treeViewEngineBusca.GetIterFirst (out iterSelected)){
						if((bool) filter.Model.GetValue (iterSelected,8) == true){
							NpgsqlConnection conexion;
							conexion = new NpgsqlConnection (connectionString+nombrebd);
							try{
								conexion.Open ();
								NpgsqlCommand comando; 
								comando = conexion.CreateCommand();
								comando.CommandText = "UPDATE osiris_catalogo_almacenes SET eliminado = 'true' "+
									"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
									"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,2)+"' ;";
								comando.ExecuteNonQuery();
								comando.Dispose();
								conexion.Close();
							}catch (NpgsqlException ex){
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								                                               MessageType.Error, 
								                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
							}
							conexion.Close();
						}
						if ((bool) filter.Model.GetValue (iterSelected,9) == true){
							NpgsqlConnection conexion;
							conexion = new NpgsqlConnection (connectionString+nombrebd);
							try{
								conexion.Open ();
								NpgsqlCommand comando; 
								comando = conexion.CreateCommand();
								comando.CommandText = "UPDATE osiris_catalogo_almacenes SET tiene_stock = 'true' "+
									"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
									"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,2)+"' ;";
								comando.ExecuteNonQuery();
								comando.Dispose();
								conexion.Close();
							}catch (NpgsqlException ex){
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								                                               MessageType.Error, 
								                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
							}
							conexion.Close();
						}
					}
					while (treeViewEngineBusca.IterNext(ref iterSelected)){
						if ((bool) filter.Model.GetValue (iterSelected,8) == true){
							NpgsqlConnection conexion;
							conexion = new NpgsqlConnection (connectionString+nombrebd);
							try{
								conexion.Open ();
								NpgsqlCommand comando; 
								comando = conexion.CreateCommand();
								comando.CommandText = "UPDATE osiris_catalogo_almacenes SET eliminado = 'true' "+
									"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
									"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,2)+"' ;";
								comando.ExecuteNonQuery();
								comando.Dispose();
								conexion.Close();
							}catch (NpgsqlException ex){
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								                                               MessageType.Error, 
								                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();									
							}
							conexion.Close();
						}
						if ((bool) filter.Model.GetValue (iterSelected,9) == true){
							NpgsqlConnection conexion;
							conexion = new NpgsqlConnection (connectionString+nombrebd);
							try{
								conexion.Open ();
								NpgsqlCommand comando; 
								comando = conexion.CreateCommand();
								comando.CommandText = "UPDATE osiris_catalogo_almacenes SET tiene_stock = 'true' "+
									"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
									"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,2)+"' ;";
								comando.ExecuteNonQuery();
								comando.Dispose();
								conexion.Close();
							}catch (NpgsqlException ex){
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								                                               MessageType.Error, 
								                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();									
							}
							conexion.Close();
						}
					}
				}
				llenando_busqueda_productos();
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
					MessageType.Info,ButtonsType.Ok,"No tiene Permiso para Borrar Articulos");
				msgBox.Run ();msgBox.Destroy();
			}
		}
		
		void on_checkbutton_enviar_clicked(object sender, EventArgs args)
		{
			if(this.checkbutton_enviar_articulos.Active == true){
				checkbutton_ajuste_de_articulos.Sensitive = false;
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
				MessageType.Question,ButtonsType.YesNo,"¿ A Continuacion se Enviaran al Sub almacen Destino"+ 
				                                          "los Articulos Seleccionados¡¡");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
			}
			
			if(this.checkbutton_ajuste_de_articulos.Active == true){
				checkbutton_enviar_articulos.Sensitive = false;
				this.combobox_almacen_destino.Sensitive = false;
				this.label_almacen_destino.Sensitive = false; 
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
				MessageType.Question,ButtonsType.YesNo,"¿A Continuacion se Realizara el Ajuste a"+ 
				                                          "los Articulos Seleccionados¡¡");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
			}
		}
		
		// Cuando seleccion el treeview de cargos extras para cargar los productos  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			
			//if (lista_almacenes.Model.GetIter (out iter, path)) {
			//	bool old = (bool) lista_almacenes.Model.GetValue (iter,0);
			//	lista_almacenes.Model.SetValue(iter,0,!old);			
			//}
			
			if (filter.Model.GetIter(out iter, path)) {
				bool old = (bool) filter.Model.GetValue(iter,0);
				filter.Model.SetValue(iter,0,!old);				
			}
		}
		
		void selecciona_fila2(object obj, ToggledArgs args)
		{
			Gtk.CellRendererToggle check_toggle = (Gtk.CellRendererToggle) obj;
			// Gtk.ComboBox combobox_almacen_origen = obj as ComboBox;
			// Gtk.RadioButton radiobutton_filtros = (Gtk.RadioButton) obj;
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (filter.Model.GetIter (out iter, path)){
				bool old = (bool) filter.Model.GetValue (iter,8);
				//lista_almacenes.Model.SetValue(iter,8,!old);
				filter.Model.SetValue(iter,8,!old);
			}	
		}
		
		void selecciona_fila3(object obj, ToggledArgs args)
		{
			Gtk.CellRendererToggle check_toggle = (Gtk.CellRendererToggle) obj;
			// Gtk.ComboBox combobox_almacen_origen = obj as ComboBox;
			// Gtk.RadioButton radiobutton_filtros = (Gtk.RadioButton) obj;
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (filter.Model.GetIter (out iter, path)){				
				bool old = (bool) filter.Model.GetValue (iter,9);
				//lista_almacenes.Model.SetValue(iter,9,!old);
				filter.Model.SetValue(iter,9,!old);
			}	
		}
						
		void NumberCellEdited_Autorizado (object o, EditedArgs args)
		{
			Gtk.TreeIter iter;
			bool esnumerico = false;
			int var_paso = 0;
			int largo_variable = args.NewText.ToString().Length;
			string toma_variable = args.NewText.ToString();
			
			this.treeViewEngineBusca2.GetIter (out iter, new Gtk.TreePath (args.Path));
			
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
				treeViewEngineBusca2.SetValue(iter,(int) Col_traspaso.col_autorizado,args.NewText);
				bool old = (bool) filter.Model.GetValue (iter,0);
				filter.Model.SetValue(iter,0,!old);
			}
 		}
		
		void crea_treeview_inventarios()
		{			
			treeViewEngineBusca = new ListStore(typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(bool),
			                                    typeof(bool));
			lista_almacenes.Model = treeViewEngineBusca;
			
			lista_almacenes.RulesHint = true;
			
			//lista_almacenes.RowActivated += on_selecciona_almacen_clicked;  // Doble click selecciono paciente
			
			col_descrip = new TreeViewColumn();
			cellrt0 = new CellRendererText();
			col_descrip.Title = "Descripcion";
			col_descrip.PackStart(cellrt0, true);
			col_descrip.AddAttribute (cellrt0, "text", 0); // la siguiente columna será 1 en vez de 2
			col_descrip.Resizable = true;
			cellrt0.Width = 600;
			col_descrip.SortColumnId = (int) Col_inv_sub_almacen.col_descrip;
						
			col_cantidad = new TreeViewColumn();
			cellrt1 = new CellRendererText();
			col_cantidad.Title = "Existencia"; // titulo de la cabecera de la columna, si está visible
			col_cantidad.PackStart(cellrt1, true);
			col_cantidad.AddAttribute (cellrt1, "text", 1);    // la siguiente columna será 1 en vez de 1
			col_cantidad.SortColumnId = (int) Col_inv_sub_almacen.col_cantidad;
						
			col_codigo = new TreeViewColumn();
			cellrt2 = new CellRendererText();
			col_codigo.Title = "Codigo";
			col_codigo.PackStart(cellrt2, true);
			col_codigo.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
			col_codigo.SortColumnId = (int) Col_inv_sub_almacen.col_codigo;
			
			col_minimo = new TreeViewColumn();
			cellrt3 = new CellRendererText();
			col_minimo.Title = "Minimo Stock";
			col_minimo.PackStart(cellrt3, true);
			col_minimo.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 1 en vez de 2
			col_minimo.SortColumnId = (int) Col_inv_sub_almacen.col_minimo;
			
			col_maximo = new TreeViewColumn();
			cellrt4 = new CellRendererText();
			col_maximo.Title = "Maximo Stock";
			col_maximo.PackStart(cellrt4, true);
			col_maximo.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 1 en vez de 2
			col_maximo.SortColumnId = (int) Col_inv_sub_almacen.col_maximo;
			
			col_reorden = new TreeViewColumn();
			cellrt5 = new CellRendererText();
			col_reorden.Title = "Punto de Reorden";
			col_reorden.PackStart(cellrt5, true);
			col_reorden.AddAttribute (cellrt5, "text", 5); // la siguiente columna será 1 en vez de 2
			col_reorden.SortColumnId = (int) Col_inv_sub_almacen.col_reorden;
						
			col_fecha = new TreeViewColumn();
			cellrt6 = new CellRendererText();
			col_fecha.Title = "Fec. Ultimo Surtido";
			col_fecha.PackStart(cellrt6, true);
			col_fecha.AddAttribute (cellrt6, "text", 6); // la siguiente columna será 1 en vez de 2
			col_fecha.SortColumnId = (int) Col_inv_sub_almacen.col_fecha;
			
			col_embalaje = new TreeViewColumn();
			cellrt7 = new CellRendererText();
			col_embalaje.Title = "Embalaje";
			col_embalaje.PackStart(cellrt7, true);
			col_embalaje.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 1 en vez de 2
			col_embalaje.SortColumnId = (int) Col_inv_sub_almacen.col_embalaje;
			
			col_quitar = new TreeViewColumn();
			cel_quitar = new CellRendererToggle();
			col_quitar.Title = "Borrar"; // titulo de la cabecera de la columna, si está visible
			col_quitar.PackStart(cel_quitar, true);
			col_quitar.AddAttribute (cel_quitar, "active", 8);
			cel_quitar.Activatable = true;
			cel_quitar.Toggled += selecciona_fila2;
			col_quitar.SortColumnId = (int) Col_inv_sub_almacen.col_quitar;
			
			col_es_stock = new TreeViewColumn();
			cel_es_stock = new CellRendererToggle();
			col_es_stock.Title = "Es de Stock"; // titulo de la cabecera de la columna, si está visible
			col_es_stock.PackStart(cel_es_stock, true);
			col_es_stock.AddAttribute (cel_es_stock, "active", 9);
			cel_es_stock.Activatable = true;
			cel_es_stock.Toggled += selecciona_fila3;
			col_es_stock.SortColumnId = (int) Col_inv_sub_almacen.col_es_stock;
			
			lista_almacenes.AppendColumn(col_descrip);
			lista_almacenes.AppendColumn(col_cantidad);
			lista_almacenes.AppendColumn(col_codigo);
			lista_almacenes.AppendColumn(col_minimo);
			lista_almacenes.AppendColumn(col_maximo);
			lista_almacenes.AppendColumn(col_reorden);
			lista_almacenes.AppendColumn(col_fecha);
			lista_almacenes.AppendColumn(col_embalaje);
			lista_almacenes.AppendColumn(col_quitar);
			lista_almacenes.AppendColumn(col_es_stock);						
		}
		
		void OnFilterEntryTextChanged (object o, System.EventArgs args)
		{
			// Since the filter text changed, tell the filter to re-determine which rows to display
			filter.Refilter ();
		}
	 
		bool FilterTree (Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if (entry_filter.Text == "")
				return true;
			string artistName = model.GetValue (iter, 0).ToString ();
			if (artistName.IndexOf (entry_filter.Text.ToString().ToUpper()) > -1)
				return true;
			else
				return false;			
		}		
		
		bool FilterTree2 (Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if (entry_filter.Text == "")
				return true;
			string artistName = model.GetValue (iter, 4).ToString ();
			if (artistName.IndexOf (entry_filter.Text.ToString().ToUpper()) > -1)
				return true;
			else
				return false;	
		}
		
		enum Col_inv_sub_almacen
		{
			col_descrip,
			col_cantidad,
			col_codigo,
			col_minimo,
			col_maximo,
			col_reorden,
			col_fecha,
			col_embalaje,
			col_quitar,col_es_stock
		}
		
		void actualizar(object sender, EventArgs args)
		{
			llenando_busqueda_productos();
		}	
		
		void llenando_busqueda_productos()
		{
			if(this.tipoalmacen==1){
				treeViewEngineBusca.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			}
			if(this.tipoalmacen==2){
				treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			}
			if(this.tipoalmacen==3){
				treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			}	
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	       
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();	          	
				comando.CommandText = query_sql+
							query_grupo+
							//query_grupo1+
							//query_grupo2+
							query_stock+
							" AND osiris_catalogo_almacenes.id_almacen = '"+idsubalmacen.ToString().Trim()+"' "+
							"ORDER BY descripcion_producto;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//string tienestock = "";
				while (lector.Read()){
					//tienestock = Convert.ToString((bool) lector["tiene_stock"]);
					//Console.WriteLine(tienestock);
					if(this.tipoalmacen==1){
						treeViewEngineBusca.AppendValues ((string) lector["descripcion_producto"],
														(string) lector["stock"], 
														(string) lector["idproducto"],
														(string) lector["minstock"],
														(string) lector["maxstock"],
														(string) lector["reorden"],
														(string) lector["fechsurti"],
														(string) lector["embalaje"],
														false,
						                                false);
													
						this.col_descrip.SetCellDataFunc(cellrt0, new Gtk.TreeCellDataFunc(cambia_colores_fila_productos));
						this.col_cantidad.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_fila_productos));
						this.col_codigo.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores_fila_productos));
						this.col_minimo.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_fila_productos));
						this.col_maximo.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_fila_productos));
						this.col_reorden.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_fila_productos));
						this.col_fecha.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_fila_productos));
						this.col_embalaje.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_fila_productos));						
					}
				
					if(this.tipoalmacen==2){
						treeViewEngineBusca2.AppendValues (false,
							                                   "0",
							                                   (string) lector["stock"],
							                                   (string) lector["idproducto"],
							                                   (string) lector["descripcion_producto"],
							                                   (string) lector["costoproductounitario"],
							                                   (string) lector["preciopublico"]);
					}
				
					if(this.tipoalmacen==3){
						treeViewEngineBusca2.AppendValues (false,
							                                   "0",
							                                   (string) lector["stock"],
							                                   (string) lector["idproducto"],
							                                   (string) lector["descripcion_producto"],
							                                   (string) lector["costoproductounitario"],
							                                   (string) lector["preciopublico"],
						                                   		(string) lector["minstock"],
																(string) lector["maxstock"],
																(string) lector["reorden"]);
					}	
				}
				if(tipoalmacen == 1){
					filter = new Gtk.TreeModelFilter (treeViewEngineBusca, null);
					filter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTree);
					lista_almacenes.Model = filter;
				}
				if(tipoalmacen == 2){
					filter = new Gtk.TreeModelFilter (treeViewEngineBusca2, null);
					filter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTree2);
					lista_almacenes.Model = filter;
				}
				if(tipoalmacen == 3){
					filter = new Gtk.TreeModelFilter (treeViewEngineBusca2, null);
					filter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTree2);
					lista_almacenes.Model = filter;
				}
					
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		// funcion para cambiar el color de una fila y columna
		void cambia_colores_fila_productos(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//descripcion_producto descrip = (descripcion_producto) model.GetValue (iter, 14);
			if ( float.Parse((string) this.lista_almacenes.Model.GetValue (iter,1)) > 0 ){
				(cell as Gtk.CellRendererText).Foreground = "black";
			}else{
				(cell as Gtk.CellRendererText).Foreground = "red";
			}
		}
		
		// funcion para cambiar el color de una fila y columna
		void cambia_colores_fila_productos2(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//descripcion_producto descrip = (descripcion_producto) model.GetValue (iter, 14);
			if ( float.Parse((string) this.lista_almacenes.Model.GetValue (iter,2)) > 0 ){
				(cell as Gtk.CellRendererText).Foreground = "black";
			}else{
				(cell as Gtk.CellRendererText).Foreground = "red";
			}
		}
		
		void on_button_enviar_articulos_clicked(object sender, EventArgs args)
		{
			// Aqui se validara el numero de traspaso por departamento ****************************
			//if (this.entry_numero_de_traspaso.Text == "" )	{
				//MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				//MessageType.Error, 
				//ButtonsType.Close, "Debe de llenar el campo de Numero de Envio");
				//msgBoxError.Run ();
				//msgBoxError.Destroy();
			//}else{
				if(LoginEmpleado =="DOLIVARES" || LoginEmpleado =="ADMIN" || LoginEmpleado == "ROLVEDAFLORES" || LoginEmpleado == "AGUTIERREZV"){
					if (this.checkbutton_enviar_articulos.Active == true){
						if (idsubalmacen != 1){
							MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
					                         MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de traspasar los Materiales Seleccionados?");
							ResponseType miResultado = (ResponseType)msgBox.Run ();
							msgBox.Destroy();					
							if (miResultado == ResponseType.Yes){
								TreeIter iterSelected;
								if (this.treeViewEngineBusca2.GetIterFirst (out iterSelected)){
									if ((bool) filter.Model.GetValue (iterSelected,0) == true){
										if (decimal.Parse((string) filter.Model.GetValue (iterSelected,2)) > 0 ){
											NpgsqlConnection conexion;
											conexion = new NpgsqlConnection (connectionString+nombrebd);
					 						try{
												conexion.Open ();
												NpgsqlCommand comando; 
												comando = conexion.CreateCommand();
												comando.CommandText = "SELECT id_producto,id_almacen,stock FROM osiris_catalogo_almacenes "+
															"WHERE id_almacen = '"+this.idalmacendestino.ToString()+"' "+
															"AND eliminado = 'false' "+														
															"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
										
												NpgsqlDataReader lector = comando.ExecuteReader ();
													if(lector.Read()){
														NpgsqlConnection conexion5;
														conexion5 = new NpgsqlConnection (connectionString+nombrebd);
														try{
															conexion5.Open ();
															NpgsqlCommand comando5; 
															comando5 = conexion5.CreateCommand();
															comando5.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = stock - '"+(string)filter.Model.GetValue(iterSelected,1)+"' "+
																"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																"AND eliminado = 'false'" +
																"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
															
															comando5.ExecuteNonQuery();
															comando5.Dispose();
															conexion5.Close();
														
															NpgsqlConnection conexion1; 
															conexion1 = new NpgsqlConnection (connectionString+nombrebd);
															try{
																conexion1.Open ();
																NpgsqlCommand comando1;
																comando1 = conexion1.CreateCommand();
																comando1.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = stock + '"+(string) filter.Model.GetValue(iterSelected,1)+"' "+
																	"WHERE id_almacen = '"+this.idalmacendestino.ToString()+"' "+
																	"AND eliminado = 'false'" +
																	"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
																		
																comando1.ExecuteNonQuery();
																comando1.Dispose();
																conexion1.Close();
																	
																NpgsqlConnection conexion4; 
																conexion4 = new NpgsqlConnection (connectionString+nombrebd);
																try{
																	conexion4.Open ();
																	NpgsqlCommand comando4; 
																	comando4 = conexion4.CreateCommand ();
																	comando4.CommandText = "INSERT INTO osiris_his_solicitudes_deta("+
																					"folio_de_solicitud,"+
																					"id_producto,"+
																					"cantidad_autorizada,"+
																					"fechahora_traspaso,"+
																					"id_quien_traspaso,"+
																					"id_almacen_origen,"+
																					"id_almacen,"+
																					"costo_por_unidad,"+
																					"cantidad_solicitada,"+
																					"precio_producto_publico,"+
																					"numero_de_traspaso,"+
																					"traspaso ) "+
																					"VALUES ("+
																					"0,'"+
																					(string) filter.Model.GetValue(iterSelected,3)+"','"+//+" ,'"+
																					(string) filter.Model.GetValue(iterSelected,1)+"','"+
																					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																					LoginEmpleado+"','"+
																					this.idsubalmacen.ToString()+"','"+
																					this.idalmacendestino.ToString()+"' ,'"+
																					(string) filter.Model.GetValue(iterSelected,5)+"','"+
																					(string) filter.Model.GetValue(iterSelected,1)+"','"+
																					(string) filter.Model.GetValue(iterSelected,6)+"','"+
																					(string) this.entry_numero_de_traspaso.Text.ToUpper()+"','"+	
																					"true');";
																		comando4.ExecuteNonQuery();
																		comando4.Dispose();
																	
																	}catch (NpgsqlException ex){
																	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																	                                               MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
																	msgBoxError.Run ();				msgBoxError.Destroy();
																	}
																	conexion4.Close();
															}catch (NpgsqlException ex){
																MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																                                               MessageType.Error, 
																                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
																msgBoxError.Run ();
															}
															conexion1.Close();
														}catch (NpgsqlException ex){
														MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													                                               MessageType.Error, 
													                                              ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
														msgBoxError.Run ();
														}
														conexion.Close();
													}else{
														NpgsqlConnection conexion1; 
														conexion1 = new NpgsqlConnection (connectionString+nombrebd);
								 							try{
																conexion1.Open ();
																NpgsqlCommand comando1; 
																comando1 = conexion1.CreateCommand();
																comando1.CommandText = "INSERT INTO osiris_catalogo_almacenes("+
																						"id_almacen,"+
																						"id_producto,"+
																						"stock,"+
																						"id_quien_creo,"+
																						"fechahora_alta,"+
																						"fechahora_ultimo_surtimiento)"+
																						"VALUES ('"+
																						this.idalmacendestino.ToString()+"','"+
																						(string) filter.Model.GetValue (iterSelected,3)+"','"+
																						(string) filter.Model.GetValue (iterSelected,1)+"','"+
																						this.LoginEmpleado+"','"+
																						DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																						DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');";
																comando1.ExecuteNonQuery();
																comando1.Dispose();
															}catch (NpgsqlException ex){
												   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																			MessageType.Error, 
																			ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
																msgBoxError.Run ();
																msgBoxError.Destroy();
															}
															conexion1.Close();
															NpgsqlConnection conexion6; 
															conexion6 = new NpgsqlConnection (connectionString+nombrebd);
																try{
																	conexion6.Open ();
																	NpgsqlCommand comando1;
																	comando1 = conexion6.CreateCommand();
																	comando1.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = '"+(string) filter.Model.GetValue(iterSelected,1)+"' "+
																			"WHERE id_almacen = '"+this.idalmacendestino.ToString()+"' "+
																			"AND eliminado = 'false'" +
																			"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
																					
																	comando1.ExecuteNonQuery();
																	comando1.Dispose();
																	conexion6.Close();
																	
																	NpgsqlConnection conexion7;
																	conexion7 = new NpgsqlConnection (connectionString+nombrebd);
																	try{
																		conexion7.Open ();
																		NpgsqlCommand comando7; 
																		comando7 = conexion7.CreateCommand();
																		comando7.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = stock - '"+(string) filter.Model.GetValue(iterSelected,1)+"' "+
																			"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																			"AND eliminado = 'false'" +
																			"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
																		comando7.ExecuteNonQuery();
																		comando7.Dispose();
																		conexion7.Close();
																		
																		NpgsqlConnection conexion4; 
																		conexion4 = new NpgsqlConnection (connectionString+nombrebd);
																		try{
																			conexion4.Open ();
																			NpgsqlCommand comando4; 
																			comando4 = conexion4.CreateCommand ();
																			comando4.CommandText = "INSERT INTO osiris_his_solicitudes_deta("+
																							"folio_de_solicitud,"+
																							"id_producto,"+
																							"cantidad_autorizada,"+
																							"fechahora_traspaso,"+
																							"id_quien_traspaso,"+
																							"id_almacen_origen,"+
																							"id_almacen,"+
																							"costo_por_unidad,"+
																							"cantidad_solicitada,"+
																							"precio_producto_publico,"+
																							"numero_de_traspaso,"+
																							"traspaso ) "+
																							"VALUES ("+
																							"0,'"+
																							(string) filter.Model.GetValue(iterSelected,3)+"','"+//+" ,'"+
																							(string) filter.Model.GetValue(iterSelected,1)+"','"+
																							DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																							LoginEmpleado+"','"+
																							this.idsubalmacen.ToString()+"','"+
																							this.idalmacendestino.ToString()+"' ,'"+
																							(string) filter.Model.GetValue(iterSelected,5)+"','"+
																							(string) filter.Model.GetValue(iterSelected,1)+"','"+
																							(string) filter.Model.GetValue(iterSelected,6)+"','"+
																							(string) entry_numero_de_traspaso.Text.ToUpper()+"','"+	
																							"true');";
																				comando4.ExecuteNonQuery();
																				comando4.Dispose();
																			}catch (NpgsqlException ex){
																			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																			                                               MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
																			msgBoxError.Run ();				msgBoxError.Destroy();
																			}
																			conexion4.Close();
																		}catch (NpgsqlException ex){
														   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																					MessageType.Error, 
																					ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
																		msgBoxError.Run ();
																		msgBoxError.Destroy();
																	}
																}catch (NpgsqlException ex){
														   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																					MessageType.Error, 
																					ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
																		msgBoxError.Run ();
																		msgBoxError.Destroy();
																}
													}
											}catch (NpgsqlException ex){
												MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												                                               MessageType.Error, 
												                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
												msgBoxError.Run ();                                               
											}
										}
									}	
								}
								while (treeViewEngineBusca2.IterNext(ref iterSelected)){
									if ((bool) filter.Model.GetValue (iterSelected,0) == true){
										if (decimal.Parse((string) filter.Model.GetValue (iterSelected,2)) > 0 ){
											NpgsqlConnection conexion;
											conexion = new NpgsqlConnection (connectionString+nombrebd);
					 						try{
												conexion.Open ();
												NpgsqlCommand comando; 
												comando = conexion.CreateCommand();
												comando.CommandText = "SELECT id_producto,id_almacen,stock FROM osiris_catalogo_almacenes "+
															"WHERE id_almacen = '"+this.idalmacendestino.ToString()+"' "+
															"AND eliminado = 'false' "+														
															"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
										
												NpgsqlDataReader lector = comando.ExecuteReader ();
													if(lector.Read()){
														NpgsqlConnection conexion5;
														conexion5 = new NpgsqlConnection (connectionString+nombrebd);
														try{
															conexion5.Open ();
															NpgsqlCommand comando5; 
															comando5 = conexion5.CreateCommand();
															comando5.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = stock - '"+(string) filter.Model.GetValue(iterSelected,1)+"' "+
																"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																"AND eliminado = 'false'" +
																"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
															
															comando5.ExecuteNonQuery();
															comando5.Dispose();
															conexion5.Close();
														
															NpgsqlConnection conexion1; 
															conexion1 = new NpgsqlConnection (connectionString+nombrebd);
															try{
																conexion1.Open ();
																NpgsqlCommand comando1;
																comando1 = conexion1.CreateCommand();
																comando1.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = stock + '"+(string) filter.Model.GetValue(iterSelected,1)+"' "+
																	"WHERE id_almacen = '"+this.idalmacendestino.ToString()+"' "+
																	"AND eliminado = 'false'" +
																	"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
																		
																comando1.ExecuteNonQuery();
																comando1.Dispose();
																conexion1.Close();
																	
																NpgsqlConnection conexion4; 
																conexion4 = new NpgsqlConnection (connectionString+nombrebd);
																try{
																	conexion4.Open ();
																	NpgsqlCommand comando4; 
																	comando4 = conexion4.CreateCommand ();
																	comando4.CommandText = "INSERT INTO osiris_his_solicitudes_deta("+
																					"folio_de_solicitud,"+
																					"id_producto,"+
																					"cantidad_autorizada,"+
																					"fechahora_traspaso,"+
																					"id_quien_traspaso,"+
																					"id_almacen_origen,"+
																					"id_almacen,"+
																					"costo_por_unidad,"+
																					"cantidad_solicitada,"+
																					"precio_producto_publico,"+
																					"numero_de_traspaso,"+
																					"traspaso ) "+
																					"VALUES ("+
																					"0,'"+
																					(string) filter.Model.GetValue(iterSelected,3)+"','"+//+" ,'"+
																					(string) filter.Model.GetValue(iterSelected,1)+"','"+
																					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																					LoginEmpleado+"','"+
																					this.idsubalmacen.ToString()+"','"+
																					this.idalmacendestino.ToString()+"' ,'"+
																					(string) filter.Model.GetValue(iterSelected,5)+"','"+
																					(string) filter.Model.GetValue(iterSelected,1)+"','"+
																					(string) filter.Model.GetValue(iterSelected,6)+"','"+
																					(string) this.entry_numero_de_traspaso.Text.ToUpper()+"','"+	
																					"true');";
																		comando4.ExecuteNonQuery();
																		comando4.Dispose();
																	
																	}catch (NpgsqlException ex){
																	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																	                                               MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
																	msgBoxError.Run ();				msgBoxError.Destroy();
																	}
																	conexion4.Close();
															}catch (NpgsqlException ex){
																MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																                                               MessageType.Error, 
																                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
																msgBoxError.Run ();
															}
															conexion1.Close();
														}catch (NpgsqlException ex){
														MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													                                               MessageType.Error, 
													                                              ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
														msgBoxError.Run ();
														}
														conexion.Close();
													}else{
														NpgsqlConnection conexion1; 
														conexion1 = new NpgsqlConnection (connectionString+nombrebd);
								 							try{
																conexion1.Open ();
																NpgsqlCommand comando1; 
																comando1 = conexion1.CreateCommand();
																comando1.CommandText = "INSERT INTO osiris_catalogo_almacenes("+
																						"id_almacen,"+
																						"id_producto,"+
																						"stock,"+
																						"id_quien_creo,"+
																						"fechahora_alta,"+
																						"fechahora_ultimo_surtimiento)"+
																						"VALUES ('"+
																						this.idalmacendestino.ToString()+"','"+
																						(string) filter.Model.GetValue (iterSelected,3)+"','"+
																						(string) filter.Model.GetValue (iterSelected,1)+"','"+
																						this.LoginEmpleado+"','"+
																						DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																						DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');";
																comando1.ExecuteNonQuery();
																comando1.Dispose();
															}catch (NpgsqlException ex){
												   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																			MessageType.Error, 
																			ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
																msgBoxError.Run ();
																msgBoxError.Destroy();
															}
															conexion1.Close();
															NpgsqlConnection conexion6; 
															conexion6 = new NpgsqlConnection (connectionString+nombrebd);
																try{
																	conexion6.Open ();
																	NpgsqlCommand comando1;
																	comando1 = conexion6.CreateCommand();
																	comando1.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = '"+(string) filter.Model.GetValue(iterSelected,1)+"' "+
																			"WHERE id_almacen = '"+this.idalmacendestino.ToString()+"' "+
																			"AND eliminado = 'false'" +
																			"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
																					
																	comando1.ExecuteNonQuery();
																	comando1.Dispose();
																	conexion6.Close();
																	
																	NpgsqlConnection conexion7;
																	conexion7 = new NpgsqlConnection (connectionString+nombrebd);
																	try{
																		conexion7.Open ();
																		NpgsqlCommand comando7; 
																		comando7 = conexion7.CreateCommand();
																		comando7.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = stock - '"+(string) filter.Model.GetValue(iterSelected,1)+"' "+
																			"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																			"AND eliminado = 'false'" +
																			"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
																		comando7.ExecuteNonQuery();
																		comando7.Dispose();
																		conexion7.Close();
																		
																		NpgsqlConnection conexion4; 
																		conexion4 = new NpgsqlConnection (connectionString+nombrebd);
																		try{
																			conexion4.Open ();
																			NpgsqlCommand comando4; 
																			comando4 = conexion4.CreateCommand ();
																			comando4.CommandText = "INSERT INTO osiris_his_solicitudes_deta("+
																							"folio_de_solicitud,"+
																							"id_producto,"+
																							"cantidad_autorizada,"+
																							"fechahora_traspaso,"+
																							"id_quien_traspaso,"+
																							"id_almacen_origen,"+
																							"id_almacen,"+
																							"costo_por_unidad,"+
																							"cantidad_solicitada,"+
																							"precio_producto_publico,"+
																							"numero_de_traspaso,"+
																							"traspaso ) "+
																							"VALUES ("+
																							"0,'"+
																							(string) filter.Model.GetValue(iterSelected,3)+"','"+//+" ,'"+
																							(string) filter.Model.GetValue(iterSelected,1)+"','"+
																							DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																							LoginEmpleado+"','"+
																							this.idsubalmacen.ToString()+"','"+
																							this.idalmacendestino.ToString()+"' ,'"+
																							(string) filter.Model.GetValue(iterSelected,5)+"','"+
																							(string) filter.Model.GetValue(iterSelected,1)+"','"+
																							(string) filter.Model.GetValue(iterSelected,6)+"','"+
																							(string) this.entry_numero_de_traspaso.Text.ToUpper()+"','"+	
																							"true');";
																				comando4.ExecuteNonQuery();
																				comando4.Dispose();
																			
																			}catch (NpgsqlException ex){
																			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																			                                               MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
																			msgBoxError.Run ();				msgBoxError.Destroy();
																			}
																			conexion4.Close();
																		}catch (NpgsqlException ex){
														   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																					MessageType.Error, 
																					ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
																		msgBoxError.Run ();
																		msgBoxError.Destroy();
																	}
																}catch (NpgsqlException ex){
														   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																					MessageType.Error, 
																					ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
																		msgBoxError.Run ();
																		msgBoxError.Destroy();
																}
															conexion6.Close();
													}
											}catch (NpgsqlException ex){
												MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												                                               MessageType.Error, 
												                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
												msgBoxError.Run ();                                               
											}
										}
									}
								}
							}
							checkbutton_enviar_articulos.Active = false;
							checkbutton_ajuste_de_articulos.Active = false;
							checkbutton_ajuste_de_articulos.Sensitive = true;
							llenando_busqueda_productos();
							entry_numero_de_traspaso.Text = "0";
						}						
					}
					
					if (this.checkbutton_ajuste_de_articulos.Active == true){
							MessageDialog msgBox2 = new MessageDialog (MyWin,DialogFlags.Modal,
								                 MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de Realizar el Ajuste de los Materiales Seleccionados?");
							ResponseType miResultado2 = (ResponseType)msgBox2.Run ();
							msgBox2.Destroy();					
							if (miResultado2 == ResponseType.Yes){
								TreeIter iterSelected2;								
								if (this.treeViewEngineBusca2.GetIterFirst (out iterSelected2)){
									if ((bool) filter.Model.GetValue (iterSelected2,0) == true){
										//if (decimal.Parse((string) this.lista_almacenes.Model.GetValue (iterSelected2,1)) != 0 ){
											NpgsqlConnection conexion;
											conexion = new NpgsqlConnection (connectionString+nombrebd);
											try{
												conexion.Open ();
												NpgsqlCommand comando; 
												comando = conexion.CreateCommand();
												comando.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = '"+(string) filter.Model.GetValue(iterSelected2,1)+"',"+
													"historial_ajustes = historial_ajustes || '"+LoginEmpleado.Trim()+";"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Trim()+";"+
													Convert.ToString((string) filter.Model.GetValue (iterSelected2,2)).Trim()+";"+
													Convert.ToString((string) filter.Model.GetValue (iterSelected2,1)).Trim()+"\n' "+
													"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
													"AND eliminado = 'false' " +
													"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected2,3)+"' ;";
												//Console.WriteLine(comando.CommandText.ToString());
												comando.ExecuteNonQuery();
												comando.Dispose();
												
											}catch (NpgsqlException ex){
												MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												                                               MessageType.Error, 
												                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
												msgBoxError.Run ();
											}
											conexion.Close();
										//}
									}
								}
								
								while (treeViewEngineBusca2.IterNext(ref iterSelected2)){
									if ((bool) filter.Model.GetValue (iterSelected2,0) == true){
										//if (decimal.Parse((string) this.lista_almacenes.Model.GetValue (iterSelected2,2)) != 0 ){
											NpgsqlConnection conexion;
											conexion = new NpgsqlConnection (connectionString+nombrebd);
											try{
												conexion.Open ();
												NpgsqlCommand comando; 
												comando = conexion.CreateCommand();
												comando.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = '"+(string) filter.Model.GetValue(iterSelected2,1)+"',"+
													"historial_ajustes = historial_ajustes || '"+LoginEmpleado.Trim()+";"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Trim()+";"+
													Convert.ToString((string) filter.Model.GetValue (iterSelected2,2)).Trim()+";"+
													Convert.ToString((string) filter.Model.GetValue (iterSelected2,1)).Trim()+"\n' "+
													"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
													"AND eliminado = 'false' " +
													"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected2,3)+"' ;";
												comando.ExecuteNonQuery();
												comando.Dispose();
																									
											}catch (NpgsqlException ex){
												MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												                                               MessageType.Error, 
												                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
												msgBoxError.Run ();
											}
											conexion.Close();
										//}
									}
								}
							//}
						//}
						}
					}
				}else{
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Info,ButtonsType.Ok,"No tiene Permiso para Enviar/Ajustar Articulos");
					msgBox.Run ();msgBox.Destroy();
				}
				checkbutton_enviar_articulos.Sensitive = true;
				checkbutton_ajuste_de_articulos.Sensitive = true;
				combobox_almacen_destino.Sensitive = true;
				label_almacen_destino.Sensitive = true;
				checkbutton_enviar_articulos.Active = false;
				checkbutton_ajuste_de_articulos.Active = false;
				llenando_busqueda_productos();
				this.entry_numero_de_traspaso.Text = "0";
		}	
		
		void imprime_reporte_traspaso(object sender, EventArgs args)
		{
			
		}
				
		void imprime_reporte_stock(object sender, EventArgs args)
		{
			new osiris.inventario_almacen_reporte (idsubalmacen,descripcionalmacen,"01","0000",
													LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"inventario_actual",
							query_sql+query_grupo+query_stock+" AND osiris_catalogo_almacenes.id_almacen = '"+idsubalmacen.ToString().Trim()+"' "+"ORDER BY osiris_productos.id_grupo_producto,osiris_productos.descripcion_producto;","","","","","","");
		}
				
		void on_checkbutton_articulos_sin_stock_clicked(object sender, EventArgs args)
		{			
			if(this.checkbutton_sin_stock.Active == true) {
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
				MessageType.Question,ButtonsType.YesNo,"¿ A Continuacion se Mostraran Todos los Articulos que Actualmente Estan sin Existencia¡¡");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
	 			if (miResultado == ResponseType.Yes){
					checkbutton_articulos_con_stock.Active = false;
	 				query_stock = "AND osiris_catalogo_almacenes.stock <= '0' ";
	 				this.llenando_busqueda_productos();
				}else{
					this.checkbutton_sin_stock.Active = false;
				}
			}else{
				if(this.checkbutton_articulos_con_stock.Active == false) {
					this.checkbutton_sin_stock.Active = false;
					query_stock = " ";
					this.llenando_busqueda_productos();
				}
			}			
		}
		
		void on_checkbutton_articulos_con_stock_clicked(object sender, EventArgs args)
		{			
			if(this.checkbutton_articulos_con_stock.Active == true) {
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
				MessageType.Question,ButtonsType.YesNo,"¿ A Continuacion se Mostraran Todos los Articulos que Actualmente Tienen Existencia¡¡");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
	 			if (miResultado == ResponseType.Yes){
	 				checkbutton_sin_stock.Active = false;
					query_stock = " AND osiris_catalogo_almacenes.stock > '0' ";
	 				this.llenando_busqueda_productos();
				}else{
					this.checkbutton_articulos_con_stock.Active = false;
				}
			}else{
				if(this.checkbutton_sin_stock.Active == false) {
					this.checkbutton_articulos_con_stock.Active = false;
					query_stock = " ";
					this.llenando_busqueda_productos();
				}
			}			
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
      	public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione
		public void onKeyPressEvent_numero_traspaso(object o, Gtk.KeyPressEventArgs args)
		{ 
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace")
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