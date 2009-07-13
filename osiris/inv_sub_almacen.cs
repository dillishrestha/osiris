////////////////////////////////////////////////////////////
// created on 05/03/2008 at 04:30 p
// Hospital Santa Cecilia
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
// Programa		: hscmty.cs
// Proposito	: Pagos en Caja 
// Objeto		: cargos_hospitalizacion.cs
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using Gnome;
using System.Collections;
using GtkSharp;

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
		[Widget] Gtk.Statusbar statusbar_inv_sub_hosp;
		
		public string connectionString = "Server=localhost;" +
									"Port=5432;" +
									 "User ID=admin;" +
									"Password=1qaz2wsx;";
		
		public string LoginEmpleado;
		public string NomEmpleado;
		public string AppEmpleado;
		public string ApmEmpleado;
		public string nombrebd;
		
		public int idsubalmacen;
		public int idalmacendestino;
		public string descsubalmacen;
		public int tipoalmacen;
		
		public int columna = 0;
		public int fila = -90;
		public int filas = 690;
		public int contador = 1;
		public int numpage = 1;
		
		public int idtipogrupo = 0;
		public int idtipogrupo1 = 0;
		public int idtipogrupo2 = 0;
		public int idalmacen = 0;
		public string descripgrupo = "";
		public string descripgrupo1 =  "";
		public string descripgrupo2 = "";
		public string tiposeleccion = "";
		public string descripcionalmacen = "";
		
		public string query_grupo = " ";
		public string query_grupo1 = " ";
		public string query_grupo2 = " ";
		public string query_stock = " ";
		public string tiporeporte = "STOCK";
		public string titulo = "REPORTE DE STOCK HOSPITALIZACION";
		
		// Declaracion de fuentes tipo Bitstream Vera sans
		public Gnome.Font fuente6 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
		public Gnome.Font fuente8 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
		public Gnome.Font fuente12 = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
		public Gnome.Font fuente10 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
		public Gnome.Font fuente11 = Gnome.Font.FindClosest("Bitstream Vera Sans", 11);
		public Gnome.Font fuente36 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
		public Gnome.Font fuente7 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
		public Gnome.Font fuente9 = Gnome.Font.FindClosest("Bitstream Vera Sans", 9);
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		private TreeStore treeViewEngineBusca;
		private TreeStore treeViewEngineBusca2;
		
		//declaracion de columnas y celdas de treeview de busqueda
		public TreeViewColumn col_descrip;		public CellRendererText cellrt0;
		public TreeViewColumn col_existencia;	public CellRendererText cellrt1;
		public TreeViewColumn col_codigo;		public CellRendererText cellrt2;
		public TreeViewColumn col_minimo;		public CellRendererText cellrt3;
		public TreeViewColumn col_maximo;		public CellRendererText cellrt4;
		public TreeViewColumn col_reorden;		public CellRendererText cellrt5;
		public TreeViewColumn col_fecha;		public CellRendererText cellrt6;
		public TreeViewColumn col_embalaje;		public CellRendererText cellrt7;
		public TreeViewColumn col_enviar;       //public CellRendererText cellrt8;
		public TreeViewColumn col_cantenviar;   //public CellRendererText cellrt8;	
		public TreeViewColumn col_descripcion;  //public CellRendererText cellrt8;
		public TreeViewColumn col_costo;
		public TreeViewColumn col_cantidad;
		public TreeViewColumn col_precio;	
	
		public inventario_sub_almacen(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, 
									string ApmEmpleado_, string _nombrebd_, int _idsubalmacen_, string _descsubalmacen_, int tipoalmacen_)
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = _nombrebd_;
			idsubalmacen = _idsubalmacen_;
			descsubalmacen =_descsubalmacen_;
			tipoalmacen = tipoalmacen_;   // 1 = inventario sub-almacenes   2 = Traspasos de Sub-Almacenes   3 = AMBOS para almacen general
			
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "inventario_sub_almacenes", null);
			gxml.Autoconnect (this);
			inventario_sub_almacenes.Show();
			Console.WriteLine(descsubalmacen+"   almacen");
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
						
			llenado_grupo("selecciona",descripgrupo,idtipogrupo);
		 	llenado_grupo1("selecciona",descripgrupo1,idtipogrupo1);
			llenado_grupo2("selecciona",descripgrupo2,idtipogrupo2);
			
			
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
				this.query_stock = " AND hscmty_catalogo_almacenes.stock > 0";
				crea_treeview_traspaso();
				//TRASPASO DE SUB ALMACENES
			}
			
			if(this.tipoalmacen == 3){
				this.button_quitar.Hide();
				this.query_stock = " AND hscmty_catalogo_almacenes.stock > 0";
				crea_treeview_traspaso();
				//TODAS LAS OPCIONES
			}
						
			// Llenado de combobox1 
			combobox_tipo_almacen.Clear();
			combobox_almacen_destino.Clear();
			CellRendererText cell1 = new CellRendererText();
			
			combobox_tipo_almacen.PackStart(cell1, true);
			combobox_almacen_destino.PackStart(cell1, true);
			
			combobox_tipo_almacen.AddAttribute(cell1,"text",0);
			combobox_almacen_destino.AddAttribute(cell1,"text",0);
			
			ListStore store2 = new ListStore( typeof (string), typeof (int));
			ListStore store3 = new ListStore( typeof (string), typeof (int));
			
			combobox_tipo_almacen.Model = store2;
			combobox_almacen_destino.Model = store3;
							        
			// lleno de la tabla de his_tipo_de_admisiones
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT id_almacen,descripcion_almacen,sub_almacen FROM hscmty_almacenes "+
               						"WHERE sub_almacen = 'true'  "+
               						"ORDER BY descripcion_almacen;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
				store2.AppendValues (descsubalmacen,0);
				store3.AppendValues (descsubalmacen,0);
				//Console.WriteLine(this.tipoalmacen.ToString());
               	while (lector.Read()){
					if(this.tipoalmacen == 1 || this.tipoalmacen == 3 ){
						store2.AppendValues ((string) lector["descripcion_almacen"], (int) lector["id_almacen"]);
					}
					if(this.tipoalmacen == 2 || this.tipoalmacen == 3 ){
						store3.AppendValues ((string) lector["descripcion_almacen"], (int) lector["id_almacen"]);
					}
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
						
			TreeIter iter2;
			if (store2.GetIterFirst(out iter2)){
				combobox_tipo_almacen.SetActiveIter (iter2);
			}
			combobox_tipo_almacen.Changed += new EventHandler (onComboBoxChanged_sub_almacenes);
			combobox_almacen_destino.Changed += new EventHandler (onComboBoxChanged_almacen_destino);
			
			llenando_busqueda_productos();
		}
		
		void crea_treeview_traspaso()
		{			
			treeViewEngineBusca2 = new TreeStore(typeof(bool),
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
			cellrt2 = new CellRendererText();
			col_descripcion.Title = "Descripcion";
			col_descripcion.PackStart(cellrt2, true);
			col_descripcion.AddAttribute (cellrt2, "text", 4); // la siguiente columna será 1 en vez de 2
			col_descripcion.SortColumnId = (int) Col_traspaso.col_descripcion;
						
			lista_almacenes.AppendColumn(col_surtir);
			lista_almacenes.AppendColumn(col_autorizado);
			lista_almacenes.AppendColumn(col_existencia);
			lista_almacenes.AppendColumn(col_codigo);
			lista_almacenes.AppendColumn(col_descripcion);
				
		}
		
		enum Col_traspaso
		{
			col_surtir,
			col_autorizado,
			col_existencia,
			col_codigo,
			col_descripcion			
		}
		
		void on_button_quitar_clicked (object sender, EventArgs args)		
		{
			if(LoginEmpleado =="N000008" || LoginEmpleado =="N000142" || LoginEmpleado =="N000414" || LoginEmpleado =="N000313" || LoginEmpleado =="N000423" 
				|| LoginEmpleado =="DOLIVARES" || LoginEmpleado =="HVARGAS" || LoginEmpleado =="JBUENTELLO"){
					MessageDialog msgBox2 = new MessageDialog (MyWin,DialogFlags.Modal,
						                 MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de Borrar los Materiales Seleccionados?");
					ResponseType miResultado2 = (ResponseType)msgBox2.Run ();
					msgBox2.Destroy();					
						if (miResultado2 == ResponseType.Yes){
							TreeIter iterSelected;
							if (this.treeViewEngineBusca.GetIterFirst (out iterSelected)){
								if ((bool) this.lista_almacenes.Model.GetValue (iterSelected,8) == true){
									NpgsqlConnection conexion;
									conexion = new NpgsqlConnection (connectionString+nombrebd);
									try{
										conexion.Open ();
										NpgsqlCommand comando; 
										comando = conexion.CreateCommand();
										comando.CommandText = "UPDATE hscmty_catalogo_almacenes SET eliminado = 'true' "+
											"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
											"AND id_producto = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,2)+"' ;";
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
								if ((bool) this.lista_almacenes.Model.GetValue (iterSelected,8) == true){
									NpgsqlConnection conexion;
									conexion = new NpgsqlConnection (connectionString+nombrebd);
									try{
										conexion.Open ();
										NpgsqlCommand comando; 
										comando = conexion.CreateCommand();
										comando.CommandText = "UPDATE hscmty_catalogo_almacenes SET eliminado = 'true' "+
											"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
											"AND id_producto = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,2)+"' ;";
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
			if (lista_almacenes.Model.GetIter (out iter, path)) {
				bool old = (bool) lista_almacenes.Model.GetValue (iter,0);
				lista_almacenes.Model.SetValue(iter,0,!old);
			}	
		}
		
		void selecciona_fila2(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (lista_almacenes.Model.GetIter (out iter, path)){
				bool old = (bool) lista_almacenes.Model.GetValue (iter,8);
				lista_almacenes.Model.SetValue(iter,8,!old);
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
				this.treeViewEngineBusca2.SetValue(iter,(int) Col_traspaso.col_autorizado,args.NewText);
				bool old = (bool) this.lista_almacenes.Model.GetValue (iter,0);
				this.lista_almacenes.Model.SetValue(iter,0,!old);
			}
 		}
		
		void crea_treeview_inventarios()
		{			
			treeViewEngineBusca = new TreeStore(typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
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
			
			TreeViewColumn col_quitar = new TreeViewColumn();
			CellRendererToggle cel_quitar = new CellRendererToggle();
			col_quitar.Title = "Borrar"; // titulo de la cabecera de la columna, si está visible
			col_quitar.PackStart(cel_quitar, true);
			col_quitar.AddAttribute (cel_quitar, "active", 8);
			cel_quitar.Activatable = true;
			cel_quitar.Toggled += selecciona_fila2;
			col_quitar.SortColumnId = (int) Col_inv_sub_almacen.col_quitar;
			
			lista_almacenes.AppendColumn(col_descrip);
			lista_almacenes.AppendColumn(col_cantidad);
			lista_almacenes.AppendColumn(col_codigo);
			lista_almacenes.AppendColumn(col_minimo);
			lista_almacenes.AppendColumn(col_maximo);
			lista_almacenes.AppendColumn(col_reorden);
			lista_almacenes.AppendColumn(col_fecha);
			lista_almacenes.AppendColumn(col_embalaje);
			lista_almacenes.AppendColumn(col_quitar);
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
			col_quitar
		}
		
		void onComboBoxChanged_sub_almacenes(object sender, EventArgs args)
		{
			ComboBox combobox_tipo_almacen = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_tipo_almacen.GetActiveIter (out iter)){
		    	idsubalmacen = (int) combobox_tipo_almacen.Model.GetValue(iter,1);
		    	llenando_busqueda_productos();
		    }
		}
		
		void onComboBoxChanged_almacen_destino(object sender, EventArgs args)
		{
			ComboBox combobox_almacen_destino = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (this.combobox_almacen_destino.GetActiveIter (out iter)){
				idalmacendestino = (int) this.combobox_almacen_destino.Model.GetValue(iter,1);
		    }
		}
		
		void actualizar(object sender, EventArgs args)
		{
			llenando_busqueda_productos();
		}	
		
		void llenando_busqueda_productos()
		{
			if(this.tipoalmacen==1)
			{
				treeViewEngineBusca.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			}
			if(this.tipoalmacen==2)
			{
				treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			}
			if(this.tipoalmacen==3)
			{
				treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			}	
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	       
			// Verifica que la base de datos este conectada
			try
			{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
	          	
				comando.CommandText = "SELECT hscmty_catalogo_almacenes.id_almacen,to_char(hscmty_productos.id_producto,'999999999999') AS codProducto,"+
							"hscmty_productos.descripcion_producto, "+
							"to_char(hscmty_catalogo_almacenes.stock,'999999999999.99') AS stockactual,"+
							"to_char(hscmty_catalogo_almacenes.minimo_stock,'999999999999.99') AS minstock,"+
							"to_char(hscmty_catalogo_almacenes.maximo,'999999999999.99') AS maxstock,"+
							"to_char(hscmty_catalogo_almacenes.punto_de_reorden,'999999999999.99') AS reorden,"+
							"to_char(hscmty_catalogo_almacenes.fechahora_ultimo_surtimiento,'yyyy-MM-dd HH24:mi:ss') AS fechsurti, "+
							"descripcion_grupo_producto, "+ //descripcion_grupo1_producto,descripcion_grupo2_producto, "+
							"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(costo_producto,'999999999.99') AS costoproducto, "+
							"to_char(cantidad_de_embalaje,'999999999.99') AS embalaje "+
							"FROM hscmty_catalogo_almacenes,hscmty_productos,hscmty_grupo_producto "+ //,hscmty_grupo1_producto,hscmty_grupo2_producto "+
							"WHERE hscmty_catalogo_almacenes.id_producto = hscmty_productos.id_producto "+ 
							"AND hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
							"AND hscmty_productos.cobro_activo = 'true' "+
							//"AND hscmty_productos.id_grupo1_producto = hscmty_grupo1_producto.id_grupo1_producto "+
							//"AND hscmty_productos.id_grupo2_producto = hscmty_grupo2_producto.id_grupo2_producto "+
							"AND hscmty_grupo_producto.agrupacion_4 = 'true'"+
							"AND hscmty_catalogo_almacenes.eliminado = 'false' "+
							query_grupo+
							//query_grupo1+
							//query_grupo2+
							query_stock+
							" AND hscmty_catalogo_almacenes.id_almacen = '"+idsubalmacen.ToString().Trim()+"' "+
							"ORDER BY descripcion_producto; ";
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				while (lector.Read()){
					if(this.tipoalmacen==1){
						treeViewEngineBusca.AppendValues ((string) lector["descripcion_producto"],
														(string) lector["stockactual"], 
														(string) lector["codProducto"],
														(string) lector["minstock"],
														(string) lector["maxstock"],
														(string) lector["reorden"],
														(string) lector["fechsurti"],
														(string) lector["embalaje"],
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
							                                   (string) lector["stockactual"],
							                                   (string) lector["codProducto"],
							                                   (string) lector["descripcion_producto"],
							                                   (string) lector["costoproductounitario"],
							                                   (string) lector["preciopublico"]);
					}
				
					if(this.tipoalmacen==3){
						treeViewEngineBusca2.AppendValues (false,
							                                   "0",
							                                   (string) lector["stockactual"],
							                                   (string) lector["codProducto"],
							                                   (string) lector["descripcion_producto"],
							                                   (string) lector["costoproductounitario"],
							                                   (string) lector["preciopublico"]);
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
				if(LoginEmpleado =="N000008" || LoginEmpleado =="N000391" || LoginEmpleado =="N000062" || LoginEmpleado =="N000313" || LoginEmpleado =="N000423" 
					|| LoginEmpleado =="DOLIVARES" || LoginEmpleado =="HVARGAS" || LoginEmpleado =="JBUENTELLO"){
					if (this.checkbutton_enviar_articulos.Active == true){
						if (idsubalmacen != 1){
							MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
					                         MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de traspasar los Materiales Seleccionados?");
							ResponseType miResultado = (ResponseType)msgBox.Run ();
							msgBox.Destroy();					
							if (miResultado == ResponseType.Yes){
								TreeIter iterSelected;
								if (this.treeViewEngineBusca2.GetIterFirst (out iterSelected)){
									if ((bool) this.lista_almacenes.Model.GetValue (iterSelected,0) == true){
										if (decimal.Parse((string) this.lista_almacenes.Model.GetValue (iterSelected,2)) > 0 ){
											NpgsqlConnection conexion;
											conexion = new NpgsqlConnection (connectionString+nombrebd);
					 						try{
												conexion.Open ();
												NpgsqlCommand comando; 
												comando = conexion.CreateCommand();
												comando.CommandText = "SELECT id_producto,id_almacen,stock FROM hscmty_catalogo_almacenes "+
															"WHERE id_almacen = '"+this.idalmacendestino.ToString()+"' "+
															"AND eliminado = 'false' "+														
															"AND id_producto = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,3)+"' ;";
										
												NpgsqlDataReader lector = comando.ExecuteReader ();
													if(lector.Read()){
														NpgsqlConnection conexion5;
														conexion5 = new NpgsqlConnection (connectionString+nombrebd);
														try{
															conexion5.Open ();
															NpgsqlCommand comando5; 
															comando5 = conexion5.CreateCommand();
															comando5.CommandText = "UPDATE hscmty_catalogo_almacenes SET stock = stock - '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,1)+"' "+
																"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																"AND eliminado = 'false'" +
																"AND id_producto = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,3)+"' ;";
															
															comando5.ExecuteNonQuery();
															comando5.Dispose();
															conexion5.Close();
														
															NpgsqlConnection conexion1; 
															conexion1 = new NpgsqlConnection (connectionString+nombrebd);
															try{
																conexion1.Open ();
																NpgsqlCommand comando1;
																comando1 = conexion1.CreateCommand();
																comando1.CommandText = "UPDATE hscmty_catalogo_almacenes SET stock = stock + '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,1)+"' "+
																	"WHERE id_almacen = '"+this.idalmacendestino.ToString()+"' "+
																	"AND eliminado = 'false'" +
																	"AND id_producto = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,3)+"' ;";
																		
																comando1.ExecuteNonQuery();
																comando1.Dispose();
																conexion1.Close();
																	
																NpgsqlConnection conexion4; 
																conexion4 = new NpgsqlConnection (connectionString+nombrebd);
																try{
																	conexion4.Open ();
																	NpgsqlCommand comando4; 
																	comando4 = conexion4.CreateCommand ();
																	comando4.CommandText = "INSERT INTO hscmty_his_solicitudes_deta("+
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
																					(string) this.lista_almacenes.Model.GetValue(iterSelected,3)+"','"+//+" ,'"+
																					(string) this.lista_almacenes.Model.GetValue(iterSelected,1)+"','"+
																					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																					LoginEmpleado+"','"+
																					this.idsubalmacen.ToString()+"','"+
																					this.idalmacendestino.ToString()+"' ,'"+
																					(string) this.lista_almacenes.Model.GetValue(iterSelected,5)+"','"+
																					(string) this.lista_almacenes.Model.GetValue(iterSelected,1)+"','"+
																					(string) this.lista_almacenes.Model.GetValue(iterSelected,6)+"','"+
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
																comando1.CommandText = "INSERT INTO hscmty_catalogo_almacenes("+
																						"id_almacen,"+
																						"id_producto,"+
																						"stock,"+
																						"id_quien_creo,"+
																						"fechahora_alta,"+
																						"fechahora_ultimo_surtimiento)"+
																						"VALUES ('"+
																						this.idalmacendestino.ToString()+"','"+
																						(string) lista_almacenes.Model.GetValue (iterSelected,3)+"','"+
																						(string) lista_almacenes.Model.GetValue (iterSelected,1)+"','"+
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
																	comando1.CommandText = "UPDATE hscmty_catalogo_almacenes SET stock = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,1)+"' "+
																			"WHERE id_almacen = '"+this.idalmacendestino.ToString()+"' "+
																			"AND eliminado = 'false'" +
																			"AND id_producto = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,3)+"' ;";
																					
																	comando1.ExecuteNonQuery();
																	comando1.Dispose();
																	conexion6.Close();
																	
																	NpgsqlConnection conexion7;
																	conexion7 = new NpgsqlConnection (connectionString+nombrebd);
																	try{
																		conexion7.Open ();
																		NpgsqlCommand comando7; 
																		comando7 = conexion7.CreateCommand();
																		comando7.CommandText = "UPDATE hscmty_catalogo_almacenes SET stock = stock - '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,1)+"' "+
																			"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																			"AND eliminado = 'false'" +
																			"AND id_producto = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,3)+"' ;";
																		comando7.ExecuteNonQuery();
																		comando7.Dispose();
																		conexion7.Close();
																		
																		NpgsqlConnection conexion4; 
																		conexion4 = new NpgsqlConnection (connectionString+nombrebd);
																		try{
																			conexion4.Open ();
																			NpgsqlCommand comando4; 
																			comando4 = conexion4.CreateCommand ();
																			comando4.CommandText = "INSERT INTO hscmty_his_solicitudes_deta("+
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
																							(string) this.lista_almacenes.Model.GetValue(iterSelected,3)+"','"+//+" ,'"+
																							(string) this.lista_almacenes.Model.GetValue(iterSelected,1)+"','"+
																							DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																							LoginEmpleado+"','"+
																							this.idsubalmacen.ToString()+"','"+
																							this.idalmacendestino.ToString()+"' ,'"+
																							(string) this.lista_almacenes.Model.GetValue(iterSelected,5)+"','"+
																							(string) this.lista_almacenes.Model.GetValue(iterSelected,1)+"','"+
																							(string) this.lista_almacenes.Model.GetValue(iterSelected,6)+"','"+
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
									if ((bool) this.lista_almacenes.Model.GetValue (iterSelected,0) == true){
										if (decimal.Parse((string) this.lista_almacenes.Model.GetValue (iterSelected,2)) > 0 ){
											NpgsqlConnection conexion;
											conexion = new NpgsqlConnection (connectionString+nombrebd);
					 						try{
												conexion.Open ();
												NpgsqlCommand comando; 
												comando = conexion.CreateCommand();
												comando.CommandText = "SELECT id_producto,id_almacen,stock FROM hscmty_catalogo_almacenes "+
															"WHERE id_almacen = '"+this.idalmacendestino.ToString()+"' "+
															"AND eliminado = 'false' "+														
															"AND id_producto = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,3)+"' ;";
										
												NpgsqlDataReader lector = comando.ExecuteReader ();
													if(lector.Read()){
														NpgsqlConnection conexion5;
														conexion5 = new NpgsqlConnection (connectionString+nombrebd);
														try{
															conexion5.Open ();
															NpgsqlCommand comando5; 
															comando5 = conexion5.CreateCommand();
															comando5.CommandText = "UPDATE hscmty_catalogo_almacenes SET stock = stock - '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,1)+"' "+
																"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																"AND eliminado = 'false'" +
																"AND id_producto = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,3)+"' ;";
															
															comando5.ExecuteNonQuery();
															comando5.Dispose();
															conexion5.Close();
														
															NpgsqlConnection conexion1; 
															conexion1 = new NpgsqlConnection (connectionString+nombrebd);
															try{
																conexion1.Open ();
																NpgsqlCommand comando1;
																comando1 = conexion1.CreateCommand();
																comando1.CommandText = "UPDATE hscmty_catalogo_almacenes SET stock = stock + '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,1)+"' "+
																	"WHERE id_almacen = '"+this.idalmacendestino.ToString()+"' "+
																	"AND eliminado = 'false'" +
																	"AND id_producto = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,3)+"' ;";
																		
																comando1.ExecuteNonQuery();
																comando1.Dispose();
																conexion1.Close();
																	
																NpgsqlConnection conexion4; 
																conexion4 = new NpgsqlConnection (connectionString+nombrebd);
																try{
																	conexion4.Open ();
																	NpgsqlCommand comando4; 
																	comando4 = conexion4.CreateCommand ();
																	comando4.CommandText = "INSERT INTO hscmty_his_solicitudes_deta("+
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
																					(string) this.lista_almacenes.Model.GetValue(iterSelected,3)+"','"+//+" ,'"+
																					(string) this.lista_almacenes.Model.GetValue(iterSelected,1)+"','"+
																					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																					LoginEmpleado+"','"+
																					this.idsubalmacen.ToString()+"','"+
																					this.idalmacendestino.ToString()+"' ,'"+
																					(string) this.lista_almacenes.Model.GetValue(iterSelected,5)+"','"+
																					(string) this.lista_almacenes.Model.GetValue(iterSelected,1)+"','"+
																					(string) this.lista_almacenes.Model.GetValue(iterSelected,6)+"','"+
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
																comando1.CommandText = "INSERT INTO hscmty_catalogo_almacenes("+
																						"id_almacen,"+
																						"id_producto,"+
																						"stock,"+
																						"id_quien_creo,"+
																						"fechahora_alta,"+
																						"fechahora_ultimo_surtimiento)"+
																						"VALUES ('"+
																						this.idalmacendestino.ToString()+"','"+
																						(string) lista_almacenes.Model.GetValue (iterSelected,3)+"','"+
																						(string) lista_almacenes.Model.GetValue (iterSelected,1)+"','"+
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
																	comando1.CommandText = "UPDATE hscmty_catalogo_almacenes SET stock = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,1)+"' "+
																			"WHERE id_almacen = '"+this.idalmacendestino.ToString()+"' "+
																			"AND eliminado = 'false'" +
																			"AND id_producto = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,3)+"' ;";
																					
																	comando1.ExecuteNonQuery();
																	comando1.Dispose();
																	conexion6.Close();
																	
																	NpgsqlConnection conexion7;
																	conexion7 = new NpgsqlConnection (connectionString+nombrebd);
																	try{
																		conexion7.Open ();
																		NpgsqlCommand comando7; 
																		comando7 = conexion7.CreateCommand();
																		comando7.CommandText = "UPDATE hscmty_catalogo_almacenes SET stock = stock - '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,1)+"' "+
																			"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																			"AND eliminado = 'false'" +
																			"AND id_producto = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected,3)+"' ;";
																		comando7.ExecuteNonQuery();
																		comando7.Dispose();
																		conexion7.Close();
																		
																		NpgsqlConnection conexion4; 
																		conexion4 = new NpgsqlConnection (connectionString+nombrebd);
																		try{
																			conexion4.Open ();
																			NpgsqlCommand comando4; 
																			comando4 = conexion4.CreateCommand ();
																			comando4.CommandText = "INSERT INTO hscmty_his_solicitudes_deta("+
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
																							(string) this.lista_almacenes.Model.GetValue(iterSelected,3)+"','"+//+" ,'"+
																							(string) this.lista_almacenes.Model.GetValue(iterSelected,1)+"','"+
																							DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																							LoginEmpleado+"','"+
																							this.idsubalmacen.ToString()+"','"+
																							this.idalmacendestino.ToString()+"' ,'"+
																							(string) this.lista_almacenes.Model.GetValue(iterSelected,5)+"','"+
																							(string) this.lista_almacenes.Model.GetValue(iterSelected,1)+"','"+
																							(string) this.lista_almacenes.Model.GetValue(iterSelected,6)+"','"+
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
							this.checkbutton_enviar_articulos.Active = false;
							this.checkbutton_ajuste_de_articulos.Active = false;
							this.checkbutton_ajuste_de_articulos.Sensitive = true;
							llenando_busqueda_productos();
							this.entry_numero_de_traspaso.Text = "0";
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
									if ((bool) this.lista_almacenes.Model.GetValue (iterSelected2,0) == true){
										//if (decimal.Parse((string) this.lista_almacenes.Model.GetValue (iterSelected2,1)) != 0 ){
											NpgsqlConnection conexion;
											conexion = new NpgsqlConnection (connectionString+nombrebd);
											try{
												conexion.Open ();
												NpgsqlCommand comando; 
												comando = conexion.CreateCommand();
												comando.CommandText = "UPDATE hscmty_catalogo_almacenes SET stock = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected2,1)+"',"+
													"historial_ajustes = historial_ajustes || '"+LoginEmpleado.Trim()+";"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Trim()+";"+
													Convert.ToString((string) this.lista_almacenes.Model.GetValue (iterSelected2,2)).Trim()+";"+
													Convert.ToString((string) this.lista_almacenes.Model.GetValue (iterSelected2,1)).Trim()+"\n' "+
													"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
													"AND eliminado = 'false' " +
													"AND id_producto = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected2,3)+"' ;";
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
									if ((bool) this.lista_almacenes.Model.GetValue (iterSelected2,0) == true){
										//if (decimal.Parse((string) this.lista_almacenes.Model.GetValue (iterSelected2,2)) != 0 ){
											NpgsqlConnection conexion;
											conexion = new NpgsqlConnection (connectionString+nombrebd);
											try{
												conexion.Open ();
												NpgsqlCommand comando; 
												comando = conexion.CreateCommand();
												comando.CommandText = "UPDATE hscmty_catalogo_almacenes SET stock = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected2,1)+"',"+
													"historial_ajustes = historial_ajustes || '"+LoginEmpleado.Trim()+";"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Trim()+";"+
													Convert.ToString((string) this.lista_almacenes.Model.GetValue (iterSelected2,2)).Trim()+";"+
													Convert.ToString((string) this.lista_almacenes.Model.GetValue (iterSelected2,1)).Trim()+"\n' "+
													"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
													"AND eliminado = 'false' " +
													"AND id_producto = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected2,3)+"' ;";
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
			titulo = "REPORTE DE ENVIO ENTRE SUB-ALMACENES";
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, titulo, 0);
        	int         respuesta = dialogo.Run ();
        
			if (respuesta == (int) Gnome.PrintButtons.Cancel){
				dialogo.Hide (); 		dialogo.Dispose (); 
				return;
			}

        	Gnome.PrintContext ctx = trabajo.Context;        
        	ComponerPagina1(ctx, trabajo); 
        	trabajo.Close();
             
        	switch (respuesta)
        	{
                  case (int) Gnome.PrintButtons.Print:   
                  		trabajo.Print (); 
                  		break;
                  case (int) Gnome.PrintButtons.Preview:
                      	new Gnome.PrintJobPreview(trabajo, titulo).Show();
                        break;
        	}
        	dialogo.Hide (); dialogo.Dispose ();
		}
		
		void ComponerPagina1(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{	
			filas=690;
			contador=1;
			string tomovalor1 = "";
			int contadorprocedimientos = 0;
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(hscmty_his_solicitudes_deta.id_producto,'999999999999') AS codProducto,"+
								"hscmty_productos.descripcion_producto AS descripcion, "+
								"to_char(hscmty_his_solicitudes_deta.cantidad_autorizada,'999999999999') AS cantAut,"+
								"hscmty_his_solicitudes_deta.id_almacen_origen, "+
								"hscmty_his_solicitudes_deta.id_almacen, "+
								"to_char(hscmty_his_solicitudes_deta.fechahora_traspaso,'yyyy-MM-dd HH:mm:ss') AS fechatraspaso, "+
								"hscmty_his_solicitudes_deta.id_quien_traspaso, "+
								"hscmty_his_solicitudes_deta.numero_de_traspaso "+
								"FROM hscmty_his_solicitudes_deta,hscmty_productos "+//,hscmty_catalogo_almacenes,hscmty_productos,hscmty_grupo_producto,hscmty_grupo1_producto,hscmty_grupo2_producto "+
								"WHERE hscmty_his_solicitudes_deta.id_producto = hscmty_productos.id_producto "+ 
								"AND hscmty_his_solicitudes_deta.traspaso = 'true' "+
								"AND hscmty_his_solicitudes_deta.eliminado = 'false' "+
								//query_stock+
								//" AND hscmty_catalogo_almacenes.id_almacen = '"+idsubalmacen.ToString().Trim()+"' "+
								"ORDER BY hscmty_productos.descripcion_producto; ";															
				NpgsqlDataReader lector = comando.ExecuteReader ();
				ContextoImp.BeginPage("Pagina 1");
				imprime_encabezado(ContextoImp,trabajoImpresion);
				Gnome.Print.Setfont (ContextoImp, fuente6);
				if (lector.Read())
				{
					ContextoImp.MoveTo(15, filas);		ContextoImp.Show((string) lector["codProducto".Trim()]);
					tomovalor1 = (string) lector["descripcion"];
					if(tomovalor1.Length > 65)
					{
						tomovalor1 = tomovalor1.Substring(0,65); 
					}
					ContextoImp.MoveTo(65, filas);		ContextoImp.Show(tomovalor1);
					ContextoImp.MoveTo(300, filas);		ContextoImp.Show((string) lector["cantAut"]);
					//ContextoImp.MoveTo(340, filas);		ContextoImp.Show("_____");
					ContextoImp.MoveTo(350, filas);		ContextoImp.Show((string) lector["id_almacen_origen"]);
					ContextoImp.MoveTo(380, filas);		ContextoImp.Show((string) lector["id_almacen"]);
					ContextoImp.MoveTo(430, filas);		ContextoImp.Show((string) lector["fechatraspaso"]);
					ContextoImp.MoveTo(510, filas);		ContextoImp.Show((string) lector["id_quien_traspaso"]);
					ContextoImp.MoveTo(600, filas);		ContextoImp.Show((string) lector["numero_de_traspaso"]);
					filas-=10;
					contador+=1;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
				}
				while (lector.Read()){
					ContextoImp.MoveTo(15, filas);		ContextoImp.Show((string) lector["codProducto".Trim()]);
					tomovalor1 = (string) lector["descripcion"];
					if(tomovalor1.Length > 65)
					{
						tomovalor1 = tomovalor1.Substring(0,65); 
					}
					ContextoImp.MoveTo(65, filas);		ContextoImp.Show(tomovalor1);
					ContextoImp.MoveTo(300, filas);		ContextoImp.Show((string) lector["cantAut"]);
					//ContextoImp.MoveTo(340, filas);		ContextoImp.Show("_____");
					ContextoImp.MoveTo(350, filas);		ContextoImp.Show((string) lector["id_almacen_origen"]);
					ContextoImp.MoveTo(380, filas);		ContextoImp.Show((string) lector["id_almacen"]);
					ContextoImp.MoveTo(430, filas);		ContextoImp.Show((string) lector["fechatraspaso"]);
					ContextoImp.MoveTo(510, filas);		ContextoImp.Show((string) lector["id_quien_traspaso"]);
					ContextoImp.MoveTo(600, filas);		ContextoImp.Show((string) lector["numero_de_traspaso"]);
					filas-=10;
					contador+=1;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
				}
				Gnome.Print.Setfont (ContextoImp, fuente9);
				contadorprocedimientos += 1;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				ContextoImp.ShowPage();
			}catch(NpgsqlException ex){
					Console.WriteLine("PostgresSQL error: {0}",ex.Message);
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();		msgBoxError.Destroy();
			}
				
		}
		
		void imprime_reporte_stock(object sender, EventArgs args)
		{
			titulo = "REPORTE DE STOCK SUB-ALMACEN";
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, titulo, 0);
        	int         respuesta = dialogo.Run ();
        
			if (respuesta == (int) Gnome.PrintButtons.Cancel){
				dialogo.Hide (); 		dialogo.Dispose (); 
				return;
			}

        	Gnome.PrintContext ctx = trabajo.Context;        
        	ComponerPagina(ctx, trabajo); 
        	trabajo.Close();
             
        	switch (respuesta)
        	{
                  case (int) Gnome.PrintButtons.Print:   
                  		trabajo.Print (); 
                  		break;
                  case (int) Gnome.PrintButtons.Preview:
                      	new Gnome.PrintJobPreview(trabajo, titulo).Show();
                        break;
        	}
        	dialogo.Hide (); dialogo.Dispose ();
		}
		
		
		void ComponerPagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{	
			filas=690;
			contador=1;
			string tomovalor1 = "";
			int contadorprocedimientos = 0;
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT hscmty_catalogo_almacenes.id_almacen,to_char(hscmty_productos.id_producto,'999999999999') AS codProducto,"+
								"hscmty_productos.descripcion_producto AS descripcion, "+
								"to_char(hscmty_catalogo_almacenes.stock,'999999999999.99') AS stock,"+
								"to_char(hscmty_catalogo_almacenes.minimo_stock,'999999999999.99') AS minstock,"+
								"to_char(hscmty_catalogo_almacenes.maximo,'999999999999.99') AS maxstock,"+
								"to_char(hscmty_catalogo_almacenes.punto_de_reorden,'999999999999.99') AS reorden,"+
								"to_char(hscmty_catalogo_almacenes.fechahora_ultimo_surtimiento,'yyyy-MM-dd') AS fechsurti, "+
								"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto, "+
								"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
								"to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
								"to_char(costo_producto,'999999999.99') AS costoproducto, "+
								"to_char(cantidad_de_embalaje,'999999999.99') AS embalaje "+
								"FROM hscmty_catalogo_almacenes,hscmty_productos,hscmty_grupo_producto,hscmty_grupo1_producto,hscmty_grupo2_producto "+
								"WHERE hscmty_catalogo_almacenes.id_producto = hscmty_productos.id_producto "+ 
								"AND hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
								"AND hscmty_productos.id_grupo1_producto = hscmty_grupo1_producto.id_grupo1_producto "+
								"AND hscmty_productos.id_grupo2_producto = hscmty_grupo2_producto.id_grupo2_producto "+
								"AND hscmty_catalogo_almacenes.id_producto = hscmty_productos.id_producto "+
								"AND hscmty_grupo_producto.agrupacion4 = 'ALM' "+						
								"AND hscmty_productos.cobro_activo = 'true' "+
								"AND hscmty_catalogo_almacenes.eliminado = 'false' "+
								query_grupo+
								query_grupo1+
								query_grupo2+
								query_stock+
								" AND hscmty_catalogo_almacenes.id_almacen = '"+idsubalmacen.ToString().Trim()+"' "+
								"ORDER BY descripcion_producto; ";															
				NpgsqlDataReader lector = comando.ExecuteReader ();
				ContextoImp.BeginPage("Pagina 1");
				imprime_encabezado(ContextoImp,trabajoImpresion);
				Gnome.Print.Setfont (ContextoImp, fuente6);
				if (lector.Read())
				{
					ContextoImp.MoveTo(15, filas);		ContextoImp.Show((string) lector["codProducto".Trim()]);
					tomovalor1 = (string) lector["descripcion"];
					if(tomovalor1.Length > 65)
					{
						tomovalor1 = tomovalor1.Substring(0,65); 
					}
					ContextoImp.MoveTo(65, filas);		ContextoImp.Show(tomovalor1);
					ContextoImp.MoveTo(300, filas);		ContextoImp.Show((string) lector["stock"]);
					ContextoImp.MoveTo(340, filas);		ContextoImp.Show("_____");
					ContextoImp.MoveTo(350, filas);		ContextoImp.Show((string) lector["minstock"]);
					ContextoImp.MoveTo(380, filas);		ContextoImp.Show((string) lector["maxstock"]);
					ContextoImp.MoveTo(430, filas);		ContextoImp.Show((string) lector["reorden".Trim()]);
					ContextoImp.MoveTo(510, filas);		ContextoImp.Show((string) lector["fechsurti"]);
					filas-=10;
					contador+=1;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
				}
				while (lector.Read()){
					ContextoImp.MoveTo(15, filas);		ContextoImp.Show((string) lector["codProducto".Trim()]);
					tomovalor1 = (string) lector["descripcion"];
					if(tomovalor1.Length > 65){
						tomovalor1 = tomovalor1.Substring(0,65); 
					}
					ContextoImp.MoveTo(65, filas);		ContextoImp.Show(tomovalor1);
					ContextoImp.MoveTo(300, filas);		ContextoImp.Show((string) lector["stock"]);
					ContextoImp.MoveTo(340, filas);		ContextoImp.Show("_____");
					ContextoImp.MoveTo(350, filas);		ContextoImp.Show((string) lector["minstock"]);
					ContextoImp.MoveTo(380, filas);		ContextoImp.Show((string) lector["maxstock"]);
					ContextoImp.MoveTo(430, filas);		ContextoImp.Show((string) lector["reorden".Trim()]);
					ContextoImp.MoveTo(510, filas);		ContextoImp.Show((string) lector["fechsurti"]);
					filas-=10;
					contador+=1;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
				}
				Gnome.Print.Setfont (ContextoImp, fuente9);
				contadorprocedimientos += 1;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				ContextoImp.ShowPage();
			}catch(NpgsqlException ex){
					Console.WriteLine("PostgresSQL error: {0}",ex.Message);
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();		msgBoxError.Destroy();
			}
				
		}
	 	
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
      		// Cambiar la fuente
      		Gnome.Print.Setfont(ContextoImp,fuente9);								
			ContextoImp.MoveTo(230, 730);			ContextoImp.Show("REPORTE DE STOCK");	
			ContextoImp.MoveTo(325, 730);			ContextoImp.Show(descsubalmacen);
			Gnome.Print.Setfont (ContextoImp, fuente6);
			ContextoImp.MoveTo(19.7, 770);			ContextoImp.Show("Hospital Santa Cecilia");
			ContextoImp.MoveTo(20, 770);			ContextoImp.Show("Hospital Santa Cecilia");
			ContextoImp.MoveTo(19.7, 760);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(20, 760);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(19.7, 750);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			ContextoImp.MoveTo(20, 750);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			Gnome.Print.Setfont(ContextoImp,fuente7);
			ContextoImp.MoveTo(230, 50);			ContextoImp.Show("PAGINA "+numpage+"  Fecha Impresion: "+DateTime.Now.ToString("dd-MM-yyyy"));
			Gnome.Print.Setfont(ContextoImp,fuente8);
			ContextoImp.MoveTo(25, 710);		ContextoImp.Show("FOLIO");
			ContextoImp.MoveTo(110, 710);		ContextoImp.Show("DESCRIPCION");
			ContextoImp.MoveTo(315, 710);		ContextoImp.Show("STOCK");
			ContextoImp.MoveTo(355, 710);		ContextoImp.Show("MIN. STK");
			ContextoImp.MoveTo(395, 710);		ContextoImp.Show("MAX. STK");
			ContextoImp.MoveTo(440, 710);		ContextoImp.Show("P. REORDEN");
			ContextoImp.MoveTo(500, 710);		ContextoImp.Show("FEC. ULT. SURTIDO");
		}    
      	
      	void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion,int contador_)
		{
	        if (contador_ > 60 ){
	        	numpage +=1;
	        	ContextoImp.ShowPage();
				ContextoImp.BeginPage("Pagina "+numpage.ToString());
				imprime_encabezado(ContextoImp,trabajoImpresion);
	     		Gnome.Print.Setfont (ContextoImp, fuente6);
	        	contador=1;
	        	filas=690;
	        }
		}
		
		void llenado_grupo(string tipo_, string descripciongrupo_,int idgrupoproducto_ )
		{
			combobox_grupo.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_grupo.PackStart(cell3, true);
			combobox_grupo.AddAttribute(cell3,"text",0);
	        
			ListStore store1 = new ListStore( typeof (string), typeof (int));
			combobox_grupo.Model = store1;
			if(tipo_ == "selecciona"){
				store1.AppendValues ((string)descripciongrupo_,(int) idgrupoproducto_);
			}
	      
	        NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM hscmty_grupo_producto ORDER BY descripcion_grupo_producto ;";				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read())
				{
					store1.AppendValues ((string) lector["descripcion_grupo_producto"],
									 	(int) lector["id_grupo_producto"] );
									 	
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
	        
			TreeIter iter1;
			if (store1.GetIterFirst(out iter1)){
				combobox_grupo.SetActiveIter (iter1);
			}
			combobox_grupo.Changed += new EventHandler (onComboBoxChanged_grupo);
		}
		
		void llenado_grupo1(string tipo_,string descripciongrupo1_,int idgrupoproducto1_)
		{
			combobox_grupo1.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_grupo1.PackStart(cell3, true);
			combobox_grupo1.AddAttribute(cell3,"text",0);
	        ListStore store4 = new ListStore( typeof (string), typeof (int), typeof (string));
			combobox_grupo1.Model = store4;
			if(tipo_ == "selecciona"){
				store4.AppendValues ((string)descripciongrupo1_,(int) idgrupoproducto1_);
			}
	      	NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM hscmty_grupo1_producto ORDER BY descripcion_grupo1_producto ;";				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read())
				{
					store4.AppendValues ((string) lector["descripcion_grupo1_producto"],
									 	(int) lector["id_grupo1_producto"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
	        TreeIter iter4;
			if (store4.GetIterFirst(out iter4)){
				combobox_grupo1.SetActiveIter (iter4);
			}
			combobox_grupo1.Changed += new EventHandler (onComboBoxChanged_grupo1);
		}
		
		void llenado_grupo2(string tipo_,string descripciongrupo2_,int idgrupoproducto2_)
		{
			combobox_grupo2.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_grupo2.PackStart(cell3, true);
			combobox_grupo2.AddAttribute(cell3,"text",0);
	        
			ListStore store3 = new ListStore( typeof (string), typeof (int));
			combobox_grupo2.Model = store3;
			if(tipo_ == "selecciona"){
				store3.AppendValues ((string)descripciongrupo2_,(int) idgrupoproducto2_);
			}
	      	NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM hscmty_grupo2_producto ORDER BY descripcion_grupo2_producto ;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read()){
					store3.AppendValues ((string) lector["descripcion_grupo2_producto"],
									 	(int) lector["id_grupo2_producto"] );
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
	        
			TreeIter iter3;
			if (store3.GetIterFirst(out iter3)){
				combobox_grupo2.SetActiveIter (iter3);
			}
			combobox_grupo2.Changed += new EventHandler (onComboBoxChanged_grupo2);
		}
		
		void onComboBoxChanged_grupo (object sender, EventArgs args)
		{
	    	ComboBox combobox_grupo = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_grupo.GetActiveIter (out iter)){
		    	idtipogrupo = (int) combobox_grupo.Model.GetValue(iter,1);
		    	descripgrupo = (string) combobox_grupo.Model.GetValue(iter,0);
		    	query_grupo = "AND hscmty_productos.id_grupo_producto = '"+idtipogrupo.ToString()+"' ";
		    	this.llenando_busqueda_productos();
		    }
		}
		
		void onComboBoxChanged_grupo1(object sender, EventArgs args)
		{
	    	ComboBox combobox_grupo1 = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_grupo1.GetActiveIter (out iter)){
		    		idtipogrupo1 = (int) combobox_grupo1.Model.GetValue(iter,1);
		    		descripgrupo1 = (string) combobox_grupo1.Model.GetValue(iter,0);
		    		query_grupo1 = "AND hscmty_productos.id_grupo1_producto = '"+idtipogrupo1.ToString()+"' ";
		    		this.llenando_busqueda_productos();
			}
		}
		
		void onComboBoxChanged_grupo2(object sender, EventArgs args)
		{		
	    	ComboBox combobox_grupo2 = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_grupo2.GetActiveIter (out iter)){
		    		idtipogrupo2 = (int) combobox_grupo2.Model.GetValue(iter,1);
		    		descripgrupo2 = (string) combobox_grupo2.Model.GetValue(iter,0);
		    		query_grupo2 = "AND hscmty_productos.id_grupo2_producto = '"+idtipogrupo2.ToString()+"' ";
		    		this.llenando_busqueda_productos();
	     	}
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
	 				query_stock = "AND hscmty_catalogo_almacenes.stock <= '0' ";
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
					query_stock = " AND hscmty_catalogo_almacenes.stock > '0' ";
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
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮ）（ｔｒｓｑ ";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace){
				args.RetVal = true;
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione
		public void onKeyPressEvent_numero_traspaso(object o, Gtk.KeyPressEventArgs args)
		{ 
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
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