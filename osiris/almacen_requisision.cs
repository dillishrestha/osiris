// created on 20/09/2007 at 06:06 p
//////////////////////////////////////////////////////////
// created on 21/06/2007 at 01:40 p
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
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
// Programa		: facturador.cs
// Proposito	: Facturador general
// Objeto		: tesoreria.cs
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
	public class requisicion_materiales_compras
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		[Widget] Gtk.Entry entry_cantidad_aplicada;
		
		// Declarando ventana del menu de Requisiciones
		[Widget] Gtk.Window requisicion_materiales;
		[Widget] Gtk.CheckButton checkbutton_nueva_requisicion;
		[Widget] Gtk.Entry entry_requisicion;
		[Widget] Gtk.Button button_busca_requisicion;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.Entry entry_fecha_solicitud;
		[Widget] Gtk.Entry entry_fecha_requerida;
		[Widget] Gtk.ComboBox combobox_tipo_admision;
		[Widget] Gtk.ComboBox combobox_tipo_admision2;
		[Widget] Gtk.Entry entry_observaciones;
		[Widget] Gtk.Entry entry_nombre_prodresado;
		[Widget] Gtk.Button button_guardar_requisicion;
		[Widget] Gtk.TreeView lista_requisicion_productos;
		
		/////// Ventana Busqueda de productos\\\\\\\\
		[Widget] Gtk.RadioButton radiobutton_nombre;
		[Widget] Gtk.RadioButton radiobutton_codigo;
		[Widget] Gtk.TreeView lista_de_producto;
		
		//Declarando la barra de estado
		[Widget] Gtk.Statusbar statusbar_almacen_requi;
				
		public string connectionString = "Server=192.168.1.4;" +
						"Port=5432;" +
						"User ID=admin1;" +
						"Password=1qaz2wsx;";

		public string nombrebd;
		public string LoginEmpleado;
    	public string NomEmpleado;
    	public string AppEmpleado;
    	public string ApmEmpleado;
    	
    	// Declarando las variables de publicas para uso dentro de classe
    	public int idtipointernamiento = 0; 		// Centro de Costos - Solicitado por
		public string descripinternamiento = "";	// Descripcion de Centro de Costos - Solicitado por
		
		public int idtipointernamiento2 = 0; 		// Centro de Costos - con Cargo a
		public string descripinternamiento2 = "";	// Descripcion de Centro de Costos - con Cargo a
		    	
    	private TreeStore treeViewEngineBusca2;		// Para la busqueda de Productos
    	private TreeStore treeViewEngineRequisicion; // Lista de proctos en una requisicion
    	
    	//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
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
		
		public requisicion_materiales_compras(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_)
		{
			LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		nombrebd = _nombrebd_; 
    		
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "requisicion_materiales", null);
			gxml.Autoconnect (this);
			////// Muestra ventana de Glade
			requisicion_materiales.Show();
			
			// Creacion de una Nueva Requisicion
			entry_requisicion.KeyPressEvent += onKeyPressEvent_enter_requisicion;
			
			checkbutton_nueva_requisicion.Clicked +=  new EventHandler(on_checkbutton_nueva_requisicion_clicked);
			
			// Asignando valores de Fechas
			this.entry_fecha_solicitud.Text = (string) DateTime.Now.ToString("yyyy-MM-dd");
			this.entry_fecha_requerida.Text = (string) DateTime.Now.ToString("yyyy-MM-dd");
			
			// Llenado de combobox1 
			combobox_tipo_admision.Clear();
			CellRendererText cell1 = new CellRendererText();
			combobox_tipo_admision.PackStart(cell1, true);
			combobox_tipo_admision.AddAttribute(cell1,"text",0);
			
			combobox_tipo_admision2.Clear();
			CellRendererText cell2 = new CellRendererText();
			combobox_tipo_admision2.PackStart(cell2, true);
			combobox_tipo_admision2.AddAttribute(cell2,"text",0);
	        
			ListStore store2 = new ListStore( typeof (string), typeof (int));
			combobox_tipo_admision.Model = store2;
			combobox_tipo_admision2.Model = store2;
							        
			// lleno de la tabla de his_tipo_de_admisiones
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM hscmty_his_tipo_admisiones "+
               						//"WHERE cuenta_mayor = 4000  "+
               						" ORDER BY descripcion_admisiones;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read())
				{
					store2.AppendValues ((string) lector["descripcion_admisiones"], (int) lector["id_tipo_admisiones"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
						
			TreeIter iter2;
			if (store2.GetIterFirst(out iter2))
			{
				//Console.WriteLine(iter2);
				combobox_tipo_admision.SetActiveIter (iter2);
			}
			combobox_tipo_admision.Changed += new EventHandler (onComboBoxChanged_tipo_admision);
			combobox_tipo_admision2.Changed += new EventHandler (onComboBoxChanged_tipo_admision2);
									
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			// Activacion de boton de busqueda
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			
			// Desactivando Entrys y Combobox
			this.entry_fecha_solicitud.Sensitive = false;
			this.entry_fecha_requerida.Sensitive = false;
			this.combobox_tipo_admision.Sensitive = false;
			this.combobox_tipo_admision2.Sensitive = false;
			this.entry_observaciones.Sensitive = false;
			this.button_guardar_requisicion.Sensitive = false;
						
			statusbar_almacen_requi.Pop(0);
			statusbar_almacen_requi.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_almacen_requi.HasResizeGrip = false;
			
			// Creacion del treeview
			crea_treeview_requisicion();
		}
		
		void on_checkbutton_nueva_requisicion_clicked(object sender, EventArgs args)
		{
			if (checkbutton_nueva_requisicion.Active == true){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de CREAR una Nueva REQUISICION ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
		 			this.button_busca_requisicion.Sensitive = false;
		 			this.entry_fecha_solicitud.Sensitive = true;
					this.entry_fecha_requerida.Sensitive = true;
					this.combobox_tipo_admision.Sensitive = true;
					this.combobox_tipo_admision2.Sensitive = true;
					this.entry_observaciones.Sensitive = true;
					this.button_guardar_requisicion.Sensitive = true;
					this.entry_requisicion.Sensitive = false;
		 		}else{
		 			checkbutton_nueva_requisicion.Active = false;
		 			this.entry_requisicion.Sensitive = true;
		 		}
		 	}else{
		 		this.button_busca_requisicion.Sensitive = true;
		 		this.entry_fecha_solicitud.Sensitive = false;
				this.entry_fecha_requerida.Sensitive = false;
				this.combobox_tipo_admision.Sensitive = false;
				this.combobox_tipo_admision2.Sensitive = false;
				this.entry_observaciones.Sensitive = false;
				this.button_guardar_requisicion.Sensitive = false;
				this.entry_requisicion.Sensitive = true;
		 	}
		 }
		
		void onComboBoxChanged_tipo_admision (object sender, EventArgs args)
		{
    		ComboBox combobox_tipo_admision = sender as ComboBox;
			if (sender == null)
			{
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_tipo_admision.GetActiveIter (out iter))
		    	{
		    		idtipointernamiento = (int) combobox_tipo_admision.Model.GetValue(iter,1);
		    		descripinternamiento = (string) combobox_tipo_admision.Model.GetValue(iter,0);
	     		}
		}
		
		void onComboBoxChanged_tipo_admision2 (object sender, EventArgs args)
		{
    		ComboBox combobox_tipo_admision2 = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_tipo_admision2.GetActiveIter (out iter)){
		    	//idtipointernamiento = (int) combobox_tipo_admision.Model.GetValue(iter,1);
		    	//descripinternamiento = (string) combobox_tipo_admision.Model.GetValue(iter,0);
	     	}
		}
		
		void on_button_busca_producto_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda("producto");
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			
			button_selecciona.Label = "Requisar";
			
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			
			entry_expresion.KeyPressEvent += onKeyPressEvent_entry_expresion;
			
			entry_cantidad_aplicada.KeyPressEvent += onKeyPressEvent;
			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
		}
		
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			if (float.Parse(entry_cantidad_aplicada.Text) > 0){
				TreeModel model;
				TreeIter iterSelected;
				
				// Llenando el TreeView para la requisicion
 				if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)) 
 				{
 					entry_nombre_prodresado.Text = (string) model.GetValue(iterSelected, 1);
					treeViewEngineRequisicion.AppendValues(entry_cantidad_aplicada.Text,
														(string) model.GetValue(iterSelected, 1),
														(string) model.GetValue(iterSelected, 0),
														(string) model.GetValue(iterSelected, 16),
														(string) model.GetValue(iterSelected, 7));
					entry_cantidad_aplicada.Text = "0";
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, 
						ButtonsType.Close, "La cantidad que quiere requisar debe ser \n"+
										   "mayor que cero, intente de nuevo");
				msgBoxError.Run ();
				msgBoxError.Destroy();	
			}
 		}
		
		// llena la lista de productos
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
			if(radiobutton_nombre.Active == true) {query_tipo_busqueda = "AND hscmty_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_producto; "; }
			if(radiobutton_codigo.Active == true) {query_tipo_busqueda = "AND hscmty_productos.id_producto LIKE '"+entry_expresion.Text.Trim()+"%'  ORDER BY id_producto; "; }
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT to_char(hscmty_productos.id_producto,'999999999999') AS codProducto,"+
							"hscmty_productos.descripcion_producto,hscmty_productos.nombre_articulo,hscmty_productos.nombre_generico_articulo, "+
							"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(precio_producto_publico1,'99999999.99') AS preciopublico1,"+
							"to_char(cantidad_de_embalaje,'99999999.99') AS cantidadembalaje,"+
							"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,cobro_activo,costo_unico,"+
							"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(hscmty_productos.id_grupo_producto,'99999') AS idgrupoproducto,hscmty_productos.id_grupo_producto, "+
							"to_char(hscmty_productos.id_grupo1_producto,'99999') AS idgrupo1producto,hscmty_productos.id_grupo1_producto, "+
							"to_char(hscmty_productos.id_grupo2_producto,'99999') AS idgrupo2producto,hscmty_productos.id_grupo2_producto, "+
							"to_char(porcentage_ganancia,'99999.999') AS porcentageutilidad,to_char(costo_producto,'999999999.99') AS costoproducto "+
							"FROM hscmty_productos,hscmty_grupo_producto,hscmty_grupo1_producto,hscmty_grupo2_producto "+
							"WHERE hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+							
							"AND hscmty_productos.id_grupo1_producto = hscmty_grupo1_producto.id_grupo1_producto "+
							"AND hscmty_productos.id_grupo2_producto = hscmty_grupo2_producto.id_grupo2_producto "+
							"AND hscmty_productos.cobro_activo = true "+
							query_tipo_busqueda;
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				float calculodeiva;
				float preciomasiva;
				float preciocondesc;
				float tomaprecio;
				float tomadescue;
				float valoriva = 15;
							
				while (lector.Read()){
					calculodeiva = 0;
					preciomasiva = 0;
					tomaprecio = float.Parse((string) lector["preciopublico"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					tomadescue = float.Parse((string) lector["porcentagesdesc"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					preciocondesc = tomaprecio;
					if ((bool) lector["aplicar_iva"]){
						calculodeiva = (tomaprecio * valoriva)/100;
					}
					if ((bool) lector["aplica_descuento"]){
						preciocondesc = tomaprecio-((tomaprecio*tomadescue)/100);
					}
					preciomasiva = tomaprecio + calculodeiva;
					 
					treeViewEngineBusca2.AppendValues (
									(string) lector["codProducto"] ,//0
									(string) lector["nombre_articulo"],
									(string) lector["preciopublico"],
									calculodeiva.ToString("F").PadLeft(10).Replace(",","."),
									preciomasiva.ToString("F").PadLeft(10).Replace(",","."),
									(string) lector["porcentagesdesc"],
									preciocondesc.ToString("F").PadLeft(10).Replace(",","."),
									" ",
									(string) lector["descripcion_grupo_producto"],
									(string) lector["descripcion_grupo1_producto"],
									(string) lector["descripcion_grupo2_producto"],
									(string) lector["nombre_articulo"],
									(string) lector["nombre_articulo"],
									(string) lector["costoproductounitario"],
									(string) lector["porcentageutilidad"],
									(string) lector["costoproducto"],
									(string) lector["cantidadembalaje"],
									(string) lector["idgrupoproducto"],
									(string) lector["idgrupo1producto"],
									(string) lector["idgrupo2producto"],
									(bool) lector["aplicar_iva"],
									(bool) lector["cobro_activo"],
									(bool) lector["aplica_descuento"],
									(string) lector["preciopublico1"]);
					col_idproducto.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_desc_producto.SetCellDataFunc(cellr1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_precioprod.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_ivaprod.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_totalprod.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_descuentoprod.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					/*col_preciocondesc.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_stock_actual.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_grupoprod.SetCellDataFunc(cellrt8, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_grupo1prod.SetCellDataFunc(cellrt9, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_grupo2prod.SetCellDataFunc(cellrt10, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_aplica_iva.SetCellDataFunc(cellrt20, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_cobro_activo.SetCellDataFunc(cellrt21, new Gtk.TreeCellDataFunc(cambia_colores_fila));*/
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((bool)lista_de_producto.Model.GetValue (iter,20)==true){
				(cell as Gtk.CellRendererText).Foreground = "blue";
			}else{
				(cell as Gtk.CellRendererText).Foreground = "black";
			}			
		}
		
		// Creando el treeview para la requisicion
		void crea_treeview_requisicion()
		{
			treeViewEngineRequisicion = new TreeStore(typeof(string), 
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));
												
			lista_requisicion_productos.Model = treeViewEngineRequisicion;
			
			lista_requisicion_productos.RulesHint = true;
			
			TreeViewColumn col_cantidad = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_cantidad.Title = "Solicitado"; // titulo de la cabecera de la columna, si está visible
			col_cantidad.PackStart(cellr0, true);
			col_cantidad.AddAttribute (cellr0, "text", 0);
			//col_cantidad.SortColumnId = (int) Column_resumen.col_cantidad;
			
			TreeViewColumn col_descripcion = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_descripcion.Title = "Descripcion"; // titulo de la cabecera de la columna, si está visible
			col_descripcion.PackStart(cellr1, true);
			col_descripcion.AddAttribute (cellr1, "text", 1);
									
			TreeViewColumn col_codigo_prod = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_codigo_prod.Title = "Codigo Prod."; // titulo de la cabecera de la columna, si está visible
			col_codigo_prod.PackStart(cellr2, true);
			col_codigo_prod.AddAttribute (cellr2, "text", 2);
			
			TreeViewColumn col_embalaje = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_embalaje.Title = "Embalaje"; // titulo de la cabecera de la columna, si está visible
			col_embalaje.PackStart(cellr3, true);
			col_embalaje.AddAttribute (cellr3, "text", 3);
			
			TreeViewColumn col_unidades = new TreeViewColumn();
			CellRendererText cellr4 = new CellRendererText();
			col_unidades.Title = "Unidades"; // titulo de la cabecera de la columna, si está visible
			col_unidades.PackStart(cellr4, true);
			col_unidades.AddAttribute (cellr4, "text", 4);
			
			TreeViewColumn col_comprado = new TreeViewColumn();
			CellRendererText cellr5 = new CellRendererText();
			col_comprado.Title = "Tot. Unidades"; // titulo de la cabecera de la columna, si está visible
			col_comprado.PackStart(cellr5, true);
			col_comprado.AddAttribute (cellr5, "text", 5);
			
			TreeViewColumn col_preciounitario = new TreeViewColumn();
			CellRendererText cellr6 = new CellRendererText();
			col_preciounitario.Title = "Precio Unitario"; // titulo de la cabecera de la columna, si está visible
			col_preciounitario.PackStart(cellr6, true);
			col_preciounitario.AddAttribute (cellr6, "text", 6);
			
			TreeViewColumn col_valor_requisado = new TreeViewColumn();
			CellRendererText cellr7 = new CellRendererText();
			col_valor_requisado.Title = "$ Requisado"; // titulo de la cabecera de la columna, si está visible
			col_valor_requisado.PackStart(cellr7, true);
			col_valor_requisado.AddAttribute (cellr7, "text", 7);
						
			TreeViewColumn col_orden_compra = new TreeViewColumn();
			CellRendererText cellr8 = new CellRendererText();
			col_orden_compra.Title = "Nº O.C."; // titulo de la cabecera de la columna, si está visible
			col_orden_compra.PackStart(cellr8, true);
			col_orden_compra.AddAttribute (cellr8, "text", 8);
			
			TreeViewColumn col_fecha_compra = new TreeViewColumn();
			CellRendererText cellr9 = new CellRendererText();
			col_fecha_compra.Title = "Fecha O.C."; // titulo de la cabecera de la columna, si está visible
			col_fecha_compra.PackStart(cellr9, true);
			col_fecha_compra.AddAttribute (cellr9, "text", 9);
			
			lista_requisicion_productos.AppendColumn(col_cantidad);	
			lista_requisicion_productos.AppendColumn(col_descripcion);
			lista_requisicion_productos.AppendColumn(col_codigo_prod);
			lista_requisicion_productos.AppendColumn(col_embalaje);
			lista_requisicion_productos.AppendColumn(col_unidades);
			lista_requisicion_productos.AppendColumn(col_preciounitario);
			lista_requisicion_productos.AppendColumn(col_valor_requisado);
			lista_requisicion_productos.AppendColumn(col_orden_compra);
			lista_requisicion_productos.AppendColumn(col_fecha_compra);			
		}
		
		void crea_treeview_busqueda(string tipo_busqueda)
		{			
			if (tipo_busqueda == "producto")
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
				lista_de_producto.AppendColumn(col_precioprod);	//2
				lista_de_producto.AppendColumn(col_ivaprod);	// 3
				lista_de_producto.AppendColumn(col_totalprod); // 4
				lista_de_producto.AppendColumn(col_descuentoprod); //5
				lista_de_producto.AppendColumn(col_preciocondesc); // 6
				lista_de_producto.AppendColumn(col_stock_actual); // 7
				lista_de_producto.AppendColumn(col_grupoprod);	//8
				lista_de_producto.AppendColumn(col_grupo1prod);	//9
				lista_de_producto.AppendColumn(col_grupo2prod);	//10
			}
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
		public void onKeyPressEvent_enter_requisicion(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				//llenado_de_requisicion();				
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
			{
				args.RetVal = true;
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
			{
				args.RetVal = true;
			}
		}
		
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_entry_expresion(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				llenando_lista_de_productos();			
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