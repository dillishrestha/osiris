///////////////////////////////////////////////////////////
// created on 27/12/2007 at 05:14 p
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Ing. Juan Antonio Peña (Programacion)
//                Ing. Hector vargas (Diseño Glade)
//				  Ing. Daniel Olivares (Programacion Ajustes Varios)
//                Tec. Homero Montoya (Ajustes Varios)
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
// Programa		: nuevos_productos.cs
// Proposito	: agregar nuevos productos 
// Objeto		: almacen_costos_compras.glade
//////////////////////////////////////////////////////////	

using System;
using Npgsql;
using Gtk;
using System.Data;
using Glade;
using Gnome;
using System.Collections;
using GtkSharp;

namespace osiris
{
	public class catalogo_productos_nuevos
	{
		// //Declarando ventana de Productos Homero Prueba "Ventana Principal"
		[Widget] Gtk.Window productos_homero_prueba;
		[Widget] Gtk.CheckButton checkbutton_nuevo_producto;
		[Widget] Gtk.Entry entry_codigo_producto;
		[Widget] Gtk.CheckButton checkbutton_producto_anidado;
		[Widget] Gtk.CheckButton checkbutton_costounico;
		[Widget] Gtk.ComboBox combobox_grupo;
		[Widget] Gtk.ComboBox combobox_grupo1;
		[Widget] Gtk.ComboBox combobox_grupo2;
		[Widget] Gtk.Entry entry_descripcion;
		[Widget] Gtk.Entry entry_nombre_articulo;
		[Widget] Gtk.Entry entry_nombre_generico;
		[Widget] Gtk.ComboBox combobox_tipo_unidad;
		[Widget] Gtk.Entry entry_costo;
		[Widget] Gtk.Entry entry_embalaje;
		[Widget] Gtk.CheckButton checkbutton_apl_iva;
		[Widget] Gtk.CheckButton checkbutton_prod_activo;
		[Widget] Gtk.CheckButton checkbutton_cambia_utilidad;
		[Widget] Gtk.Entry entry_precio_unitario;
		[Widget] Gtk.Entry entry_porciento_utilidad;
		[Widget] Gtk.Entry entry_utilidad;
		[Widget] Gtk.Entry entry_iva;
		[Widget] Gtk.CheckButton checkbutton_descuento;
		[Widget] Gtk.Entry entry_descuento_en_porciento;
		[Widget] Gtk.Entry entry_descuento_en_pesos;
		[Widget] Gtk.Entry entry_precio_publico;
		[Widget] Gtk.Entry precio_sin_iva;
		[Widget] Gtk.TreeView treeview_precios_clientes;
		[Widget] Gtk.TreeView treeview_productos_anidados;
		[Widget] Gtk.Entry entry_producto;
		
		//Declarando la barra de estado
		[Widget] Gtk.Statusbar statusbar_productos_homero_prueba;
		
		//Declarando botones
		[Widget] Gtk.Button button_guardar;
		[Widget] Gtk.ToggleButton button_editar;
		[Widget] Gtk.Button button_calcular;
		[Widget] Gtk.Button button_salir;
		//[Widget] Gtk.Button button_seleccionar;
		[Widget] Gtk.Button button_busca_producto_codigo;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.Button button_quitar;
		
		//Declaracion de ventana de busqueda de productos
		[Widget] Gtk.Window busca_producto;
		[Widget] Gtk.RadioButton radiobutton_nombre;
		[Widget] Gtk.RadioButton radiobutton_codigo;
		[Widget] Gtk.TreeView lista_de_producto;
		[Widget] Gtk.TreeView busqueda;
		[Widget] Gtk.Label label_cantidad;
		[Widget] Gtk.Entry entry_cantidad_aplicada;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		[Widget] Gtk.TreeView lista_de_producto_anidado;
		
		// Declaracion de variables publicas
		public long idproduct = 0;
		public long lastproduct = 0;
		public long newidproduct = 0;
		public long newidsecuencia = 0;
		public int idtipogrupo = 0;
		public int idtipogrupo1 = 0;
		public int idtipogrupo2 = 0;
		public string descripgrupo = "";
		public string descripgrupo1 =  "";
		public string descripgrupo2 = "";
		public string apldesc;
		public bool aplicariva_producto;
	 	public bool cobroactivo_producto;
	 	public string costounico;
	 	public string tiposeleccion = "";
	 	public decimal descuento = 0;
	 	public decimal precio_uni = 0;
	 	public bool aplica_descuento;
	 	public string tipounidadproducto = "";
		
		// Almacena los valores anterios para guardar los cuando actualiza algun precio, o descripcion
	 	public decimal precio_unitario_anterior = 0;
	 	public decimal precio_costo_anterior = 0;
		public decimal utilidad_anterior = 0;
		public string valor_anterior_descuento;
		
		//VARIABLES PARA CARGAR DATOS
	 	public string codprod ="";
	 	public string preciopub ="";
	 	public string precio ="";
	 	public string preciouni ="";
	 	public string porcientoutilidad ="";
	 	public string descripcionprod ="";
	 	public string nombreart ="";
	 	public string nombregen ="";
	 	public string embalajeprod ="";
	 	public string porcentagedesc ="";
		
		//Variables Principales
		public string connectionString = "Server=localhost;" +
										 "Port=5432;" +
										 "User ID=admin;" +
										 "Password=1qaz2wsx;";		
		public string nombrebd;
		public string LoginEmpleado;
		public string NomEmpleado;
		public string AppEmpleado;
		public string ApmEmpleado;
		public string id_producto;
		public string lee_id_producto;
		public TreeStore treeViewEngineBusca2;
		public TreeStore treeViewEngineBusca3;
		public TreeStore treeViewEngineBusca4;
		
		//declaracion de columnas y celdas de treeview de los precios de Clientes 
		public TreeViewColumn col_seleccion; 		public CellRendererToggle cellrt_seleccion;
		public TreeViewColumn col_clie_aseg;		public CellRendererText cellr30;
		public TreeViewColumn col_utilidad;			public CellRendererText cellr31;
		public TreeViewColumn col_precio_sin_iva;	public CellRendererText cellrt32;
		public TreeViewColumn col_porc_utilidad;	public CellRendererText cellrt33;
		public TreeViewColumn col_iva;				public CellRendererText cellrt34;
		public TreeViewColumn col_precio_final;		public CellRendererText cellrt35;
		
		//declaracion de columnas y celdas de treeview de busqueda
		public TreeViewColumn col_idproducto;		public CellRendererText cellr0;
		public TreeViewColumn col_desc_producto;	public CellRendererText cellr1;
		public TreeViewColumn col_precioprod;		public CellRendererText cellrt2;
		public TreeViewColumn col_ivaprod;			public CellRendererText cellrt3;
		public TreeViewColumn col_totalprod;		public CellRendererText cellrt4;
		public TreeViewColumn col_descuentoprod;	public CellRendererText cellrt5;
		public TreeViewColumn col_preciocondesc;	public CellRendererText cellrt6;
		public TreeViewColumn col_grupoprod;		public CellRendererText cellrt7;
		public TreeViewColumn col_grupo1prod;		public CellRendererText cellrt8;
		public TreeViewColumn col_grupo2prod;		public CellRendererText cellrt9;
		public TreeViewColumn col_costoprod_uni;	public CellRendererText cellrt12;
		public TreeViewColumn col_aplica_iva;		public CellRendererText cellrt19;
		public TreeViewColumn col_cobro_activo;		public CellRendererText cellrt20;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
						
		public catalogo_productos_nuevos (string LoginEmp, string NomEmpleado, string AppEmpleado, string ApmEmpleado, string _nombrebd_ )
		{
			LoginEmpleado = LoginEmp;
			nombrebd = _nombrebd_; 
			//Direcciona al glade
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "productos_homero_prueba", null);
			gxml.Autoconnect (this);
			// Muestra ventana de Glade
			productos_homero_prueba.Show();
        	
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_guardar.Clicked += new EventHandler(on_guarda_producto_clicked);
			button_editar.Clicked +=  new EventHandler(on_editar_productos_clicked);
			button_busca_producto_codigo.Clicked += new EventHandler(on_button_busca_producto_codigo_clicked);
			button_calcular.Clicked += new EventHandler(on_button_calcula_utilidad_clicked);
			checkbutton_nuevo_producto.Clicked += new EventHandler(on_checkbutton_nuevo_producto_clicked);
			checkbutton_apl_iva.Clicked += new EventHandler(on_checkbutton_apl_iva_clicked);
			checkbutton_descuento.Clicked += new EventHandler(on_checkbutton_descuento_clicked);
			checkbutton_cambia_utilidad.Clicked += new EventHandler(on_checkbutton_cambia_utilidad_clicked);
			checkbutton_producto_anidado.Clicked += new EventHandler(on_checkbutton_producto_anidado_cliked);
            			
			//button_seleccionar.Clicked += new EventHandler(on_selecciona_producto_clicked);
			//Desactiva y activa los Botones
			button_guardar.Sensitive = false;
			button_editar.Sensitive = false;
			button_calcular.Sensitive = false;
			button_busca_producto.Sensitive = false;
			button_quitar.Sensitive = false;
			
			//Desactiva y activa los entrys, checkbuttons, radiobuttons
			checkbutton_producto_anidado.Sensitive = false;
			checkbutton_costounico.Sensitive = false;
			combobox_grupo.Sensitive = false;
			combobox_grupo1.Sensitive = false;
			combobox_grupo2.Sensitive = false;
			entry_descripcion.Sensitive = false;
			entry_nombre_articulo.Sensitive = false;
			entry_nombre_generico.Sensitive = false;
			combobox_tipo_unidad.Sensitive = false;
			entry_costo.Sensitive = false;
			entry_embalaje.Sensitive = false;
			checkbutton_apl_iva.Sensitive = false;
			checkbutton_prod_activo.Sensitive = false;
			checkbutton_cambia_utilidad.Sensitive = false;
			entry_precio_unitario.Sensitive = false;
			entry_porciento_utilidad.Sensitive = false;
			entry_utilidad.Sensitive = false;
			entry_iva.Sensitive = false;
			checkbutton_descuento.Sensitive = false;
			entry_descuento_en_porciento.Sensitive = false;
			entry_descuento_en_pesos.Sensitive = false;
			entry_precio_publico.Sensitive = false;
			precio_sin_iva.Sensitive = false;
			crea_treeview_principal();
			llena_combo_tipounidad("selecciona");
			this.treeview_productos_anidados.Sensitive = false;
		}
		
		void llena_combo_tipounidad(string tipo_)
		{
			combobox_tipo_unidad.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_tipo_unidad.PackStart(cell3, true);
			combobox_tipo_unidad.AddAttribute(cell3,"text",0);
	        
			ListStore store3 = new ListStore( typeof (string));
			combobox_tipo_unidad.Model = store3;
			if(tipo_ == "selecciona"){
				store3.AppendValues (" ");
				store3.AppendValues ("PIEZA");
				store3.AppendValues ("KILO");
				store3.AppendValues ("LITRO");
				store3.AppendValues ("GRAMO");
				store3.AppendValues ("METRO");
				store3.AppendValues ("CENTIMETRO");
				store3.AppendValues ("CAJA");
				store3.AppendValues ("PULGADA");
				store3.AppendValues ("FRASCO");
			}
	      
			TreeIter iter3;
			if (store3.GetIterFirst(out iter3)){
				combobox_tipo_unidad.SetActiveIter (iter3);
			}
			combobox_tipo_unidad.Changed += new EventHandler (onComboBoxChanged_tipo_unidad);
		}
		
		void onComboBoxChanged_tipo_unidad (object sender, EventArgs args)
		{
	    	ComboBox combobox_tipo_unidad = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_tipo_unidad.GetActiveIter (out iter)){
	    		tipounidadproducto = (string) combobox_tipo_unidad.Model.GetValue(iter,0);    		 
			}
		}
		
		void on_checkbutton_cambia_utilidad_clicked(object sender, EventArgs args)
		{
			if (this.checkbutton_cambia_utilidad.Active == true){
				this.entry_porciento_utilidad.Sensitive = false;
			}else{
				this.entry_porciento_utilidad.Sensitive = true;
			}
		}	
		
		void crea_treeview_principal()
		{
			treeViewEngineBusca4 = new TreeStore(typeof(bool),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string));
			treeview_precios_clientes.Model = treeViewEngineBusca4;
			//Nombre de la ventana donde se muestra en el glade(treeview_precios_clientes)
			treeview_precios_clientes.RulesHint = true;
		
			//treeview_precios_clientes.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono producto
			
			col_seleccion = new TreeViewColumn();
			cellrt_seleccion = new CellRendererToggle();
			col_seleccion.Title = "Seleccion";
			col_seleccion.PackStart(cellrt_seleccion, true);
			col_seleccion.AddAttribute (cellrt_seleccion, "active", 0); // la siguiente columna será 1 en vez de 2
			cellrt_seleccion.Activatable = true;
			cellrt_seleccion.Toggled += selecciona_fila;
			col_seleccion.SortColumnId = (int) Column_princ.col_seleccion;			
			
			col_clie_aseg = new TreeViewColumn();
			cellr30 = new CellRendererText();
			col_clie_aseg.Title = "Cliente / Aseguradora"; // titulo de la cabecera de la columna, si está visible
			col_clie_aseg.PackStart(cellr30, true);
			col_clie_aseg.AddAttribute (cellr30, "text", 1);    // la siguiente columna será 1 en vez de 1
			col_clie_aseg.SortColumnId = (int) Column_princ.col_clie_aseg;
			
			col_precio_sin_iva = new TreeViewColumn();
			cellrt32 = new CellRendererText();
			col_precio_sin_iva.Title = "Precio Sin IVA";
			col_precio_sin_iva.PackStart(cellrt32, true);
			col_precio_sin_iva.AddAttribute (cellrt32, "text", 2); // la siguiente columna será 1 en vez de 2
			col_precio_sin_iva.SortColumnId = (int) Column_princ.col_precio_sin_iva;
			
			col_iva = new TreeViewColumn();
			cellrt34 = new CellRendererText();
			col_iva.Title = "IVA";
			col_iva.PackStart(cellrt34, true);
			col_iva.AddAttribute (cellrt34, "text", 3); // la siguiente columna será 3 en vez de 4
			col_iva.SortColumnId = (int) Column_princ.col_iva;
			
			col_precio_final = new TreeViewColumn();
			cellrt35 = new CellRendererText();
			col_precio_final.Title = "Precio Total";
			col_precio_final.PackStart(cellrt35, true);
			col_precio_final.AddAttribute (cellrt35, "text", 4); // la siguiente columna será 5 en vez de 6
			col_precio_final.SortColumnId = (int) Column_princ.col_precio_final;
       
			col_porc_utilidad = new TreeViewColumn();
			cellrt33 = new CellRendererText();
			col_porc_utilidad.Title = "% de Utilidad";
			col_porc_utilidad.PackStart(cellrt33, true);
			col_porc_utilidad.AddAttribute (cellrt33, "text", 5); // la siguiente columna será 2 en vez de 3
			col_porc_utilidad.SortColumnId = (int) Column_princ.col_porc_utilidad;
       
			col_utilidad = new TreeViewColumn();
			cellr31 = new CellRendererText();
			col_utilidad.Title = "$ Utilidad"; // titulo de la cabecera de la columna, si está visible
			col_utilidad.PackStart(cellr31, true);
			col_utilidad.AddAttribute (cellr31, "text", 6);    // la siguiente columna será 1 en vez de 1
			col_utilidad.SortColumnId = (int) Column_princ.col_utilidad;
 
			treeview_precios_clientes.AppendColumn(col_seleccion); // 0
			treeview_precios_clientes.AppendColumn(col_clie_aseg);  // 1
			treeview_precios_clientes.AppendColumn(col_precio_sin_iva);	//2
			treeview_precios_clientes.AppendColumn(col_iva); // 3
			treeview_precios_clientes.AppendColumn(col_precio_final); // 4
			treeview_precios_clientes.AppendColumn(col_porc_utilidad);	// 5
			treeview_precios_clientes.AppendColumn(col_utilidad); //6				
		}
		
		enum Column_princ
		{
			col_seleccion,
			col_clie_aseg,
			col_precio_sin_iva,
			col_iva,
			col_precio_final,
			col_porc_utilidad,
			col_utilidad
		}
		
		// Cuando seleccion el treeview de cargos extras para cargar los productos  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_precios_clientes.Model.GetIter (out iter, path)){
				bool old = (bool) treeview_precios_clientes.Model.GetValue (iter,0);
				treeview_precios_clientes.Model.SetValue(iter,0,!old);
			}	
		}
		
		//Alta de nuevo Producto
		void on_checkbutton_nuevo_producto_clicked (object sender, EventArgs args)
		{
			valor_anterior_descuento = "0.00";
			if(checkbutton_nuevo_producto.Active == true){ 
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
				MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de querer crear un nuevo producto?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
		 			llenado_grupo("","",0);
		 			llenado_grupo1("","",0);
					llenado_grupo2("","",0);
					limpia_textos(true);
					activa_campos(true);
					button_calcular.Sensitive = true;
					entry_costo.Text= "0.00";
					entry_descripcion.Text = "";
					entry_descuento_en_porciento.Text ="0.00";
					entry_descuento_en_pesos.Text ="0.00";
					entry_embalaje.Text = "0.00";
					entry_nombre_articulo.Text = "";
					entry_nombre_generico.Text = "";
					entry_iva.Text = "0.00";
					entry_porciento_utilidad.Text = "0.000";
					entry_precio_publico.Text = "0.00";
					entry_precio_unitario.Text ="0.00";
					entry_utilidad.Text ="0.00";
					precio_sin_iva.Text = "0.00";
					checkbutton_producto_anidado.Active = false;
					checkbutton_cambia_utilidad.Active = false;
					checkbutton_costounico.Active = false;
					checkbutton_apl_iva.Active = false;
					checkbutton_prod_activo.Active = false;
					checkbutton_descuento.Active = false;
					checkbutton_producto_anidado.Sensitive = true;
					checkbutton_costounico.Sensitive = true;
					checkbutton_apl_iva.Sensitive = true;
					checkbutton_prod_activo.Sensitive = true;
					checkbutton_cambia_utilidad.Sensitive = true;
					checkbutton_descuento.Sensitive = true;
					this.button_guardar.Sensitive = true;
					this.combobox_tipo_unidad.Sensitive = true;
				}else{
					checkbutton_nuevo_producto.Active = false;
				}
			}
			if(checkbutton_nuevo_producto.Active == false){ 
				activa_campos(false);
				this.button_guardar.Sensitive = false;
				checkbutton_nuevo_producto.Sensitive = true;
			}
		}
				
		void limpia_textos(bool valor)
		{
			entry_codigo_producto.Text = "";
			entry_descripcion.Text = "";
			entry_nombre_articulo.Text = "";
			entry_nombre_generico.Text = "";
		}
		
		void on_guarda_producto_clicked(object sender, EventArgs args)
		{
			if(checkbutton_nuevo_producto.Active == true){
				calculando_utilidad();
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
										ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
 				if (miResultado == ResponseType.Yes){
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
					// Verifica que la base de datos este conectada
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
	 					comando.CommandText = "INSERT INTO hscmty_productos("+
													"id_producto,"+
									 				"descripcion_producto,"+
									 				"id_grupo_producto,"+
									 				"id_grupo1_producto,"+
									 				"id_grupo2_producto,"+
									 				"precio_producto_publico, "+
									 				"costo_producto,"+
									 				"costo_por_unidad,"+
									 				"porcentage_ganancia,"+
									 				"id_quienlocreo_producto,"+
									 				"fechahora_creacion,"+
									 				"aplicar_iva,"+
									 				"cobro_activo,"+
									 				"aplica_descuento,"+
									 				"nombre_articulo,"+
									 				"nombre_generico_articulo,"+
									 				"cantidad_de_embalaje,"+
									 				"porcentage_descuento, "+
									 				"tipo_unidad_producto,"+
								                    "tiene_kit,"+
									 				"costo_unico) "+
									 				"VALUES ('"+
									 				(string) entry_codigo_producto.Text.ToUpper()+"','"+
									 				(string) entry_descripcion.Text.ToUpper()+"','"+
									 				idtipogrupo.ToString()+"','"+
									 				idtipogrupo1.ToString()+"','"+
									 				idtipogrupo2.ToString()+"','"+
									 				(string) precio_sin_iva.Text.ToUpper() +"','"+
									 				(string) entry_costo.Text.ToUpper()+"','"+
									 				(string) entry_precio_unitario.Text.ToUpper()+"','"+
									 				(string) entry_porciento_utilidad.Text.ToUpper()+"','"+
									 				LoginEmpleado+"','"+
									 				DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
									 				(bool) checkbutton_apl_iva.Active+"','"+
									 				(bool) checkbutton_prod_activo.Active+"','"+
									 				(bool) checkbutton_descuento.Active+"','"+
									 				(string) entry_nombre_articulo.Text.ToUpper()+"','"+
									 				(string) entry_nombre_generico.Text.ToUpper()+"','"+
									 				(string) entry_embalaje.Text.ToUpper()+"','"+
									 				(string) entry_descuento_en_porciento.Text.ToUpper()+"','"+
									 				tipounidadproducto+"','"+
								                    (bool) this.checkbutton_producto_anidado.Active+"','"+
									 				(bool) checkbutton_costounico.Active+"');";
						comando.ExecuteNonQuery();    	    	       	comando.Dispose();
						//Console.WriteLine(comando.CommandText);
						checkbutton_nuevo_producto.Active = false;
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info,ButtonsType.Close,"El producto se guardo con exito");
						msgBoxError.Run ();					msgBoxError.Destroy();
						checkbutton_nuevo_producto.Sensitive = true;
						button_editar.Sensitive = true;
						button_calcular.Sensitive = false;
					}catch(NpgsqlException ex){
	   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();					msgBoxError.Destroy();
					}
					conexion.Close ();
				}
			}else{
				//calculando_utilidad();
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
										ButtonsType.YesNo,"¿ Desea Actualizar esta infomacion ?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();							msgBox.Destroy();
				if (miResultado == ResponseType.Yes){
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
	    	     	// Verifica que la base de datos este conectada
	    	     	try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						comando.CommandText = "UPDATE hscmty_productos SET "+
							"descripcion_producto = '"+(string) entry_descripcion.Text.Trim().ToUpper()+"',"+
							"id_grupo_producto = '"+idtipogrupo.ToString()+"',"+
							"id_grupo1_producto = '"+idtipogrupo1.ToString()+"',"+
							"id_grupo2_producto = '"+idtipogrupo2.ToString()+"',"+
							"precio_producto_publico = '"+(string) precio_sin_iva.Text.Trim().ToUpper()+"',"+
							"costo_producto = '"+(string) entry_costo.Text.Trim().ToUpper()+"',"+
							"costo_por_unidad = '"+(string) entry_precio_unitario.Text.Trim().ToUpper()+"',"+
							"porcentage_ganancia = '"+(string) entry_porciento_utilidad.Text.Trim().ToUpper()+"',"+
							"id_quienlocreo_producto = '"+LoginEmpleado+"',"+
							"aplicar_iva = '"+(bool) checkbutton_apl_iva.Active+"',"+
							"cobro_activo = '"+(bool) checkbutton_prod_activo.Active+"',"+
							"tiene_kit = '"+(bool) this.checkbutton_producto_anidado.Active+"',"+
							"aplica_descuento = '"+(bool) checkbutton_descuento.Active+"',"+
							"nombre_articulo = '"+(string) entry_nombre_articulo.Text.Trim().ToUpper()+"',"+
							"nombre_generico_articulo = '"+(string) entry_nombre_generico.Text.Trim().ToUpper()+"', "+
							"cantidad_de_embalaje = '"+(string) entry_embalaje.Text.Trim().ToUpper()+"',"+
							"porcentage_descuento = '"+(string) entry_descuento_en_porciento.Text.Trim().ToUpper()+"',"+
							"costo_unico = '"+(bool) checkbutton_costounico.Active+"',"+
							"tipo_unidad_producto = '"+(string) tipounidadproducto+"',"+
							"historial_de_cambios = historial_de_cambios || '"+LoginEmpleado+";"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+";"+
		 					this.precio_unitario_anterior.ToString().Trim()+";"+
		 					this.precio_costo_anterior.ToString().Trim()+";"+
		 					this.utilidad_anterior.ToString().Trim()+"\n' "+
							"WHERE id_producto =  '"+(string) entry_codigo_producto.Text.Trim().ToUpper()+"' ;";
						Console.WriteLine(comando.CommandText);
 						comando.ExecuteNonQuery();    	    	       	comando.Dispose();
 						    	    	       	
    	    	       	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info,ButtonsType.Close,"Los datos se actualizaron con exito");			
						msgBoxError.Run ();					msgBoxError.Destroy();
						button_guardar.Sensitive = true;
						button_editar.Sensitive = true;
						button_calcular.Sensitive = false;
						button_busca_producto.Sensitive = false;
						button_quitar.Sensitive = false;
						checkbutton_producto_anidado.Sensitive = false;
						combobox_grupo.Sensitive = false;
						combobox_grupo1.Sensitive = false;
						combobox_grupo2.Sensitive = false;
						entry_descripcion.Sensitive = false;
						entry_nombre_articulo.Sensitive = false;
						entry_nombre_generico.Sensitive = false;
						entry_costo.Sensitive = false;
						entry_embalaje.Sensitive = false;
						checkbutton_apl_iva.Sensitive = false;
						checkbutton_prod_activo.Sensitive = false;
						entry_precio_unitario.Sensitive = false;
						entry_porciento_utilidad.Sensitive = false;
						entry_utilidad.Sensitive = false;
						entry_iva.Sensitive = false;
						entry_descuento_en_pesos.Sensitive = false;
						entry_iva.Sensitive = false;
						entry_utilidad.Sensitive = false;
						entry_descuento_en_porciento.Sensitive = false;
						entry_descuento_en_pesos.Sensitive = false;
						//entry_precio_publico.Sensitive = false;
						entry_producto.Sensitive = false;
						checkbutton_costounico.Sensitive = false;
						checkbutton_cambia_utilidad.Active = false;
						checkbutton_descuento.Sensitive = false;
						checkbutton_cambia_utilidad.Sensitive = false;
					}catch(NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();					msgBoxError.Destroy();
	       			}
       				conexion.Close ();
       			}			
			}
		}
					
		void on_button_busca_producto_codigo_clicked(object sender, EventArgs a)
		{
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			busca_producto.Show();
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			//button_seleccionar.Clicked += new EventHandler(on_selecciona_producto_clicked);
			label_cantidad.Hide();
			entry_cantidad_aplicada.Hide();
			crea_treeview_busqueda();
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
		}
		
		void crea_treeview_busqueda()
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
													typeof(bool),
													typeof(bool),
													typeof(bool),
													typeof(string),
			                                        typeof(bool));
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
				col_desc_producto.Resizable = true;
				cellr1.Width = 450;
				col_desc_producto.SortColumnId = (int) Column_prod.col_desc_producto;
				
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
            
				col_grupoprod = new TreeViewColumn();
				cellrt7 = new CellRendererText();
				col_grupoprod.Title = "Grupo Producto";
				col_grupoprod.PackStart(cellrt7, true);
				col_grupoprod.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 7 en vez de 8
				col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
            
				col_grupo1prod = new TreeViewColumn();
				cellrt8 = new CellRendererText();
				col_grupo1prod.Title = "Grupo1 Producto";
				col_grupo1prod.PackStart(cellrt8, true);
				col_grupo1prod.AddAttribute (cellrt8, "text", 8); // la siguiente columna será 9 en vez de 
				col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
                        
				col_grupo2prod = new TreeViewColumn();
				cellrt9 = new CellRendererText();
				col_grupo2prod.Title = "Grupo2 Producto";
				col_grupo2prod.PackStart(cellrt9, true);
				col_grupo2prod.AddAttribute (cellrt9, "text", 9); // la siguiente columna será 10 en vez de 9
				col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;
				
				col_costoprod_uni = new TreeViewColumn();
				cellrt12 = new CellRendererText();
				col_costoprod_uni.Title = "Precio Unitario";
				col_costoprod_uni.PackStart(cellrt12, true);
				col_costoprod_uni.AddAttribute (cellrt12, "text", 12); // la siguiente columna será 1 en vez de 2
				col_costoprod_uni.SortColumnId = (int) Column_prod.col_costoprod_uni;
				
				col_aplica_iva = new TreeViewColumn();
				cellrt19 = new CellRendererText();
				col_aplica_iva.Title = "Iva Activo?";
				col_aplica_iva.PackStart(cellrt19, true);
				col_aplica_iva.AddAttribute (cellrt19, "text", 19); // la siguiente columna será 10 en vez de 9
				col_aplica_iva.SortColumnId = (int) Column_prod.col_aplica_iva;
				
				col_cobro_activo = new TreeViewColumn();
				cellrt20 = new CellRendererText();
				col_cobro_activo.Title = "Prod. Activo?";
				col_cobro_activo.PackStart(cellrt20, true);
				col_cobro_activo.AddAttribute (cellrt20, "text", 20); // la siguiente columna será 10 en vez de 9
				col_cobro_activo.SortColumnId = (int) Column_prod.col_cobro_activo;
				
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
				lista_de_producto.AppendColumn(col_costoprod_uni); //12
				lista_de_producto.AppendColumn(col_aplica_iva);//19
				lista_de_producto.AppendColumn(col_cobro_activo);//20
		}
		
		void on_button_calcula_utilidad_clicked(object sender, EventArgs a)
		{
			calculando_utilidad();
		}
		
		void calculando_utilidad()
		{
			precio_uni = 0;
			precio_unitario_anterior = decimal.Parse(this.entry_precio_publico.Text,System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
			precio_costo_anterior = decimal.Parse(this.entry_precio_unitario.Text,System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
			
			if(decimal.Parse(entry_costo.Text) != 0 && decimal.Parse(entry_embalaje.Text) != 0){
				decimal cost_prod = decimal.Parse(this.entry_costo.Text, System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" )); 
				decimal embalaje_prod = decimal.Parse(this.entry_embalaje.Text, System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
				precio_uni = cost_prod/embalaje_prod;
				decimal porc_utilidad = 0;
				if (checkbutton_cambia_utilidad.Active == true){
					//this.entry_porciento_utilidad.Sensitive = false;
					porc_utilidad = busca_porcentage_utilidad(precio_uni);
				}else{
					//this.entry_porciento_utilidad.Sensitive = true;
					porc_utilidad = decimal.Parse( this.entry_porciento_utilidad.Text, System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
				}
				decimal utilidad_prod = precio_uni*porc_utilidad/100;
				decimal precio_pub = precio_uni+utilidad_prod;
				entry_utilidad.Text = utilidad_prod.ToString("F").Replace(",",".");
				entry_precio_unitario.Text = precio_uni.ToString("F").Replace(",",".");
				entry_precio_publico.Text = precio_pub.ToString("F").Replace(",",".");				
				this.entry_porciento_utilidad.Text = porc_utilidad.ToString().Replace(",",".");
				
				decimal calculo_iva = 0;
				decimal preciopublico = 0;
				decimal descuentocero = 0;
				decimal descuento = decimal.Parse(this.entry_descuento_en_porciento.Text, System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" )); 
				decimal porcientodescuento = 0;
				
				if (this.checkbutton_descuento.Active == true){
					descuento = (precio_uni * descuento)/100;
					this.entry_descuento_en_pesos.Text = descuento.ToString("F").Replace(",",".");				
				}else{
					this.entry_descuento_en_pesos.Text = descuentocero.ToString("F").Replace(",",".");
					this.entry_descuento_en_porciento.Text = descuentocero.ToString("F").Replace(",",".");
				}
				if(this.checkbutton_apl_iva.Active == true){
					calculo_iva = (precio_pub*15)/100;
					preciopublico = calculo_iva + precio_pub;
					this.entry_iva.Text = calculo_iva.ToString("F").Replace(",",".");					
					entry_precio_publico.Text = preciopublico.ToString("F").Replace(",",".");
				}else{
					preciopublico = precio_pub;
					this.entry_iva.Text = calculo_iva.ToString("F").Replace(",",".");
					entry_precio_publico.Text = "0";
				}				
				precio_sin_iva.Text = precio_pub.ToString("F").Replace(",",".");
				
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Info, 
								ButtonsType.Close, "Escriba el costo y embalaje del producto");
								msgBoxError.Run ();
								msgBoxError.Destroy();
			}
		}
		
	    void on_checkbutton_apl_iva_clicked(object sender, EventArgs a)
		{
			if( this.checkbutton_apl_iva.Active == true){
				aplicariva_producto = true;
			}else{
				aplicariva_producto = false;
			}
		}
	    
	    void on_checkbutton_descuento_clicked(object sender, EventArgs a)
		{
			if( this.checkbutton_descuento.Active == true){
				//aplica_descuento = true;
				this.entry_descuento_en_porciento.Text = valor_anterior_descuento;
			}else{
				//aplica_descuento = false;
				this.entry_descuento_en_porciento.Text = "0.00";
			}
		}
	    
	    void crea_treeview_busqueda3()
		{
			// Treeview de los productos anidados o kit de productos
			treeViewEngineBusca3 = new TreeStore(typeof(string),
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
													typeof(string));
				lista_de_producto_anidado.Model = treeViewEngineBusca3;
				lista_de_producto_anidado.RulesHint = true;
				//lista_de_producto_anidado.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono paciente*/
				
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
				
				col_precioprod = new TreeViewColumn();
				cellrt2 = new CellRendererText();
				col_precioprod.Title = "Precio Producto";
				col_precioprod.PackStart(cellrt2, true);
				col_precioprod.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
				col_precioprod.SortColumnId = (int) Column_prod.col_precioprod;
            
				TreeViewColumn col_ivaprod = new TreeViewColumn();
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
            
				col_grupoprod = new TreeViewColumn();
				cellrt7 = new CellRendererText();
				col_grupoprod.Title = "Grupo Producto";
				col_grupoprod.PackStart(cellrt7, true);
				col_grupoprod.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 7 en vez de 8
				col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
            
				col_grupo1prod = new TreeViewColumn();
				cellrt8 = new CellRendererText();
				col_grupo1prod.Title = "Grupo1 Producto";
				col_grupo1prod.PackStart(cellrt8, true);
				col_grupo1prod.AddAttribute (cellrt8, "text", 8); // la siguiente columna será 9 en vez de 
				col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
                        
				col_grupo2prod = new TreeViewColumn();
				cellrt9 = new CellRendererText();
				col_grupo2prod.Title = "Grupo2 Producto";
				col_grupo2prod.PackStart(cellrt9, true);
				col_grupo2prod.AddAttribute (cellrt9, "text", 9); // la siguiente columna será 10 en vez de 9
				col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;
				
				lista_de_producto_anidado.AppendColumn(col_idproducto);  // 0
				lista_de_producto_anidado.AppendColumn(col_desc_producto); // 1
				lista_de_producto_anidado.AppendColumn(col_precioprod);	//2
				lista_de_producto_anidado.AppendColumn(col_ivaprod);	// 3
				lista_de_producto_anidado.AppendColumn(col_totalprod); // 4
				lista_de_producto_anidado.AppendColumn(col_descuentoprod); //5
				lista_de_producto_anidado.AppendColumn(col_preciocondesc); // 6
				lista_de_producto_anidado.AppendColumn(col_grupoprod);	//7
				lista_de_producto_anidado.AppendColumn(col_grupo1prod);	//8
				lista_de_producto_anidado.AppendColumn(col_grupo2prod);	//9
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
			col_costo_prod,			col_cant_embalaje,
			col_id_gpo_prod,		col_id_gpo_prod1,
			col_id_gpo_prod2,		col_aplica_iva,
			col_cobro_activo,		col_aplica_desc
		}
		
		// llena la lista de productos
 		void on_llena_lista_producto_clicked (object sender, EventArgs args)
 		{
 			llena_la_lista_de_productos();
 		}
		
		void llena_la_lista_de_productos()
 		{
 			treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			string query_tipo_busqueda = "";
			if(radiobutton_nombre.Active == true) {
				query_tipo_busqueda = "AND hscmty_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_producto; "; }
			if(radiobutton_codigo.Active == true) {
				query_tipo_busqueda = "AND to_char(hscmty_productos.id_producto,'999999999999') LIKE '%"+entry_expresion.Text.Trim()+"%'  ORDER BY id_producto; ";
			}
			
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT to_char(hscmty_productos.id_producto,'999999999999') AS codProducto, "+
							"hscmty_productos.descripcion_producto,hscmty_productos.nombre_articulo,hscmty_productos.nombre_generico_articulo, "+
							"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(precio_producto_publico1,'99999999.99') AS preciopublico1,"+
							"to_char(cantidad_de_embalaje,'99999999.99') AS cantidadembalaje,"+
							"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,cobro_activo,"+
							"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(hscmty_productos.id_grupo_producto,'9999999') AS idgrupoproducto,hscmty_productos.id_grupo_producto, "+
							"to_char(hscmty_grupo_producto.porcentage_utilidad_grupo,'99999.999') AS porcentageutilidadgrupo,"+
							"to_char(hscmty_productos.id_grupo1_producto,'9999999') AS idgrupo1producto,hscmty_productos.id_grupo1_producto,"+
							"to_char(hscmty_productos.id_grupo2_producto,'9999999') AS idgrupo2producto,hscmty_productos.id_grupo2_producto,"+
							"to_char(porcentage_ganancia,'99999.999') AS porcentageutilidad,to_char(costo_producto,'999999999.99') AS costoproducto,"+
						    "tiene_kit "+
							"FROM hscmty_productos,hscmty_grupo_producto,hscmty_grupo1_producto,hscmty_grupo2_producto "+
							"WHERE hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
							"AND hscmty_productos.id_grupo1_producto = hscmty_grupo1_producto.id_grupo1_producto "+
							"AND hscmty_productos.id_grupo2_producto = hscmty_grupo2_producto.id_grupo2_producto "+
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
					if ((bool) lector["aplicar_iva"])
					{
						calculodeiva = (tomaprecio * valoriva)/100;
					}
					if ((bool) lector["aplica_descuento"])
					{
						preciocondesc = tomaprecio-((tomaprecio*tomadescue)/100);
					}
					preciomasiva = tomaprecio + calculodeiva;
					 
					treeViewEngineBusca2.AppendValues (
									(string) lector["codProducto"] ,//0
									(string) lector["descripcion_producto"],//1
									(string) lector["preciopublico"],//2
									calculodeiva.ToString("F").PadLeft(10).Replace(",","."),//3
									preciomasiva.ToString("F").PadLeft(10).Replace(",","."),//4
									(string) lector["porcentagesdesc"],//5
									preciocondesc.ToString("F").PadLeft(10).Replace(",","."),//6
									(string) lector["descripcion_grupo_producto"],//7
									(string) lector["descripcion_grupo1_producto"],//8
									(string) lector["descripcion_grupo2_producto"],//9
									(string) lector["nombre_articulo"],//10
									(string) lector["nombre_articulo"],//11
									(string) lector["costoproductounitario"],//12
									(string) lector["porcentageutilidad"],//13
									(string) lector["costoproducto"],//14
									(string) lector["cantidadembalaje"],//15
									(string) lector["idgrupoproducto"],//16
									(string) lector["idgrupo1producto"],//17
									(string) lector["idgrupo2producto"],//18
									(bool) lector["aplicar_iva"],//19
									(bool) lector["cobro_activo"],//20
									(bool) lector["aplica_descuento"],//21
									(string) lector["preciopublico1"],//22
					                (bool) lector["tiene_kit"]);//23
					col_idproducto.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_desc_producto.SetCellDataFunc(cellr1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_precioprod.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_ivaprod.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_totalprod.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_descuentoprod.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_preciocondesc.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_grupoprod.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_grupo1prod.SetCellDataFunc(cellrt8, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_grupo2prod.SetCellDataFunc(cellrt9, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_costoprod_uni.SetCellDataFunc(cellrt12, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_aplica_iva.SetCellDataFunc(cellrt19, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_cobro_activo.SetCellDataFunc(cellrt20, new Gtk.TreeCellDataFunc(cambia_colores_fila));
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
								msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((bool)lista_de_producto.Model.GetValue (iter,20)==true) { 
				if ((bool)lista_de_producto.Model.GetValue (iter,19)==true){
					(cell as Gtk.CellRendererText).Foreground = "blue";
				}else{ 
					(cell as Gtk.CellRendererText).Foreground = "black"; }
			}else{
				(cell as Gtk.CellRendererText).Foreground = "red";
			}
		}
		
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{	
			string toma_valor;
			this.treeViewEngineBusca4.Clear();
																			
			//ListStore store_grupo = new ListStore( typeof (string), typeof (int));
			if( this.tiposeleccion == "anidado"){
				
			}else{
				TreeModel model;
				TreeIter iterSelected;
				if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
					NpgsqlConnection conexion;
					conexion = new NpgsqlConnection (connectionString+nombrebd);
            		// Verifica que la base de datos este conectada
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
               			comando.CommandText = "SELECT * FROM hscmty_his_tipo_pacientes WHERE lista_de_precio = true ORDER BY id_tipo_paciente;";
						
						//Console.WriteLine(comando.CommandText);
						NpgsqlDataReader lector = comando.ExecuteReader ();
						int id_tipopaciente = 0;
						while (lector.Read()){
						
							id_tipopaciente = (int) lector["id_tipo_paciente"];
							
							// Verifando si es Empresas y Municipio
							if (id_tipopaciente == 500 || id_tipopaciente == 102){ 
								NpgsqlConnection conexion1;
								conexion1 = new NpgsqlConnection (connectionString+nombrebd);
	            				// Verifica que la base de datos este conectada
								try{
									conexion1.Open ();
									NpgsqlCommand comando1; 
									comando1 = conexion1.CreateCommand ();
	               					comando1.CommandText = "SELECT id_empresa,descripcion_empresa,hscmty_empresas.lista_de_precio,hscmty_empresas.id_tipo_paciente "+
	               								"FROM hscmty_empresas,hscmty_his_tipo_pacientes "+
	               								"WHERE hscmty_empresas.id_tipo_paciente = hscmty_his_tipo_pacientes.id_tipo_paciente "+
	               								"AND hscmty_empresas.lista_de_precio = true "+
	               								"AND hscmty_empresas.id_tipo_paciente = '"+id_tipopaciente.ToString()+"' "+
	               								"ORDER BY hscmty_empresas.id_tipo_paciente;";
	               					//Console.WriteLine(comando1.CommandText);
									NpgsqlDataReader lector1 = comando1.ExecuteReader ();
									int idtipopaciente_empresa = 0;
									int idempresa_empresa = 0;
									
									while (lector1.Read()){
										
										idtipopaciente_empresa = (int) lector1["id_tipo_paciente"];
										idempresa_empresa = (int) lector1["id_empresa"];
										
										//conexion.Open ();
										NpgsqlCommand comando3; 
										comando3 = conexion.CreateCommand ();
						               	comando3.CommandText = "SELECT to_char(hscmty_productos.id_producto,'999999999999') AS codProducto, "+
													"to_char("+"precio_producto_"+idtipopaciente_empresa.ToString().Trim()+idempresa_empresa.ToString().Trim()+",'99999999.99') AS "+"precio_producto_cliente "+
													"FROM hscmty_productos,hscmty_grupo_producto,hscmty_grupo1_producto,hscmty_grupo2_producto "+
													"WHERE hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
													"AND hscmty_productos.id_grupo1_producto = hscmty_grupo1_producto.id_grupo1_producto "+
													"AND hscmty_productos.id_grupo2_producto = hscmty_grupo2_producto.id_grupo2_producto "+
													"AND hscmty_productos.id_producto = '"+(string) model.GetValue(iterSelected, 0)+"'; ";
										//Console.WriteLine(comando3.CommandText);			
										NpgsqlDataReader lector3 = comando3.ExecuteReader ();
										if (lector3.Read()){
											treeViewEngineBusca4.AppendValues(false,(string) lector1["descripcion_empresa"],(string) lector3["precio_producto_cliente"]," ");	
											//idempresa_empresa.ToString().Trim());
											//array.Add( ); //,(string) lector1["descripcion_empresa"]);
										}
									}
								}catch (NpgsqlException ex){
		   							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
										msgBoxError.Run ();
										msgBoxError.Destroy();
								}
								conexion1.Close();
							}							
						}
					}catch (NpgsqlException ex){
	   						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
								msgBoxError.Destroy();
					}
				
				    utilidad_anterior = decimal.Parse((string) model.GetValue(iterSelected,14));
					this.entry_descripcion.Text = (string) model.GetValue(iterSelected, 1);
					this.entry_nombre_articulo.Text = (string) model.GetValue(iterSelected, 10); 
					this.entry_nombre_generico.Text = (string) model.GetValue(iterSelected, 11);
					toma_valor = (string) model.GetValue(iterSelected, 0);
	 				this.entry_codigo_producto.Text = (string) model.GetValue(iterSelected, 0);
					toma_valor = (string) model.GetValue(iterSelected, 2);
					//this.entry_precio_publico.Text = toma_valor.TrimStart();
					precio_sin_iva.Text = toma_valor.TrimStart();
					valor_anterior_descuento = (string) model.GetValue(iterSelected, 5);
					this.entry_descuento_en_porciento.Text = valor_anterior_descuento.TrimStart();
					toma_valor = (string) model.GetValue(iterSelected, 12);					
					this.entry_precio_unitario.Text = toma_valor.TrimStart();
					toma_valor = (string) model.GetValue(iterSelected, 13);
					this.entry_porciento_utilidad.Text = toma_valor.TrimStart();
					toma_valor = (string) model.GetValue(iterSelected, 14);
					this.entry_costo.Text = toma_valor.TrimStart();
					toma_valor = (string) model.GetValue(iterSelected, 15);
					this.entry_embalaje.Text = toma_valor.TrimStart();
					// Lineas especial para municipio de San Nicolas
					toma_valor = (string) model.GetValue(iterSelected, 22);
					//this.entry_precios_costunitario_sannico.Text = toma_valor.Trim();
					this.descripgrupo = (string) model.GetValue(iterSelected, 7);
					this.idtipogrupo = int.Parse((string) model.GetValue(iterSelected, 16));
					this.descripgrupo1 = (string) model.GetValue(iterSelected, 8);
					this.idtipogrupo1 = int.Parse((string) model.GetValue(iterSelected, 17));
					this.descripgrupo2 = (string) model.GetValue(iterSelected, 9);
					this.idtipogrupo2 = int.Parse((string) model.GetValue(iterSelected, 18));
					toma_valor = (string) model.GetValue(iterSelected, 14);
					llenado_grupo("selecciona",descripgrupo,idtipogrupo);
		 			llenado_grupo1("selecciona",descripgrupo1,idtipogrupo1);
					llenado_grupo2("selecciona",descripgrupo2,idtipogrupo2);
					this.checkbutton_apl_iva.Active = (bool) model.GetValue(iterSelected, 19);
					this.checkbutton_prod_activo.Active = (bool) model.GetValue(iterSelected, 20);
					this.checkbutton_cambia_utilidad.Active = false;
					
					calculando_utilidad();
					 					
					if ((bool) model.GetValue(iterSelected, 21) == true){ 
						this.checkbutton_descuento.Active = true;
						apldesc = "true";
					}else{
						apldesc = "false";	
					}
					this.checkbutton_producto_anidado.Active = (bool) model.GetValue(iterSelected, 23);
					
					// validacion de producto anidado o kit de producto 
					if ((bool) model.GetValue(iterSelected, 23)){
						this.treeview_productos_anidados.Sensitive = true;
						llenado_productos_anidados();
					}else{
						this.treeview_productos_anidados.Sensitive = false;
						//this.treeViewEngineBusca3.Clear();
					}
					entry_porciento_utilidad.Sensitive = false;
					button_editar.Sensitive = true;
					checkbutton_nuevo_producto.Active = false;
    	    	    checkbutton_nuevo_producto.Sensitive = true;
					button_editar.Sensitive = true;			
					//cierra la ventana despues que almaceno la informacion en variables
					Widget win = (Widget) sender;
					win.Toplevel.Destroy();
				}
			}
		}
		
		struct Item
 		{
 			public bool col_seleccion{
				get { return col0_col_seleccion; }
				set { col0_col_seleccion = value; }
			}
 			
 			public string col_clie_aseg{
				get { return col1_precios_productos; }
				set { col1_precios_productos = value; }
			}
			public string col_precio_sin_iva{
				get { return col2_precios_productos; }
				set { col2_precios_productos = value; }
			}
			public string col_iva{
				get { return col3_precios_productos; }
				set { col3_precios_productos = value; }
			}
			public string col_precio_final{
				get { return col4_precios_productos; }
				set { col4_precios_productos = value; }
			}
			public string col_porc_utilidad{
				get { return col5_precios_productos; }
				set { col5_precios_productos = value; }
			}
			public string col_utilidad{
				get { return col6_precios_productos; }
				set { col6_precios_productos = value; }
			}
			
			private bool col0_col_seleccion;
			private string col1_precios_productos;
			private string col2_precios_productos;
			private string col3_precios_productos;
			private string col4_precios_productos;
			private string col5_precios_productos;
			private string col6_precios_productos;
			
			public Item (bool col0_col_seleccion,string col1_precios_productos,string col2_precios_productos,
					string col3_precios_productos,string col4_precios_productos,
					string col5_precios_productos,string col6_precios_productos )
			{
				this.col0_col_seleccion = col0_col_seleccion;
				this.col1_precios_productos = col1_precios_productos;
				this.col2_precios_productos = col2_precios_productos;
				this.col3_precios_productos = col3_precios_productos;
				this.col4_precios_productos = col4_precios_productos;
				this.col5_precios_productos = col5_precios_productos;
				this.col6_precios_productos = col6_precios_productos;
			}
		}
				
		void on_editar_productos_clicked (object sender, EventArgs args)
		{
			bool activar;
			if (button_editar.Active == true){				
				activa_campos(true);
				button_guardar.Sensitive = true;
				this.button_calcular.Sensitive = true;
			}else{
				activa_campos(false);
				button_guardar.Sensitive = false;
				this.button_calcular.Sensitive = false;
			}
		}
		
		void activa_campos(bool valor)
		{	 
			checkbutton_nuevo_producto.Sensitive = valor;
			checkbutton_producto_anidado.Sensitive = valor;
			combobox_grupo.Sensitive = valor;
			combobox_grupo1.Sensitive = valor;
			combobox_grupo2.Sensitive = valor;
			entry_descripcion.Sensitive = valor;
			entry_nombre_articulo.Sensitive = valor;
			entry_nombre_generico.Sensitive = valor;
			entry_costo.Sensitive = valor;
			entry_embalaje.Sensitive = valor;
			checkbutton_apl_iva.Sensitive = valor;
			checkbutton_prod_activo.Sensitive = valor;
			entry_precio_unitario.Sensitive = valor;
			entry_porciento_utilidad.Sensitive = valor;
			entry_utilidad.Sensitive = valor;
			entry_iva.Sensitive = valor;
			entry_descuento_en_porciento.Sensitive = valor;
			entry_descuento_en_pesos.Sensitive = valor;
			entry_precio_publico.Sensitive = valor;
			entry_producto.Sensitive = valor;
			checkbutton_costounico.Sensitive = valor;
			checkbutton_descuento.Sensitive = valor;
			checkbutton_cambia_utilidad.Sensitive = valor;
			this.precio_sin_iva.Sensitive = valor;
			combobox_tipo_unidad.Sensitive = valor;
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
	        
			ListStore store2 = new ListStore( typeof (string), typeof (int), typeof (string));
			combobox_grupo1.Model = store2;
			if(tipo_ == "selecciona"){
				store2.AppendValues ((string)descripciongrupo1_,(int) idgrupoproducto1_);
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
					store2.AppendValues ((string) lector["descripcion_grupo1_producto"],
									 	(int) lector["id_grupo1_producto"]);
									 	
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
	        
			TreeIter iter2;
			if (store2.GetIterFirst(out iter2)){
				combobox_grupo1.SetActiveIter (iter2);
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
               	while (lector.Read())
				{
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
	    		if((bool) this.checkbutton_nuevo_producto.Active == true){
	    			ultimo_id_prod((int) combobox_grupo.Model.GetValue(iter,1),idtipogrupo1,idtipogrupo2);
	    		} 
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
	    		if((bool) this.checkbutton_nuevo_producto.Active == true){
	    			ultimo_id_prod(idtipogrupo,idtipogrupo1,idtipogrupo2);
	    		}
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
	    		if((bool) this.checkbutton_nuevo_producto.Active == true){
	    			ultimo_id_prod(idtipogrupo,idtipogrupo1,(int) combobox_grupo2.Model.GetValue(iter,1));
	    		}
	     	}
		}
		
		public void on_checkbutton_producto_anidado_cliked(object sender, EventArgs args)
		{
			
		}
		
		public void llenado_productos_anidados()
		{
			
		}
		
		public void ultimo_id_prod(int idtipogrupo_,int idtipogrupo1_, int idtipogrupo2_)
		{
			long primera = Convert.ToInt64(idtipogrupo_ ) * 10000000000;
			long segundo = Convert.ToInt64(idtipogrupo1_) * 100000000;
			long tercero = Convert.ToInt64(idtipogrupo2_) * 100000;
			idproduct = (primera+segundo+tercero);
			lastproduct = idproduct+100000;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();

				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT to_char(id_producto,'999999999999') AS idproducto "+ 
									"FROM hscmty_productos "+
									"WHERE id_producto > '"+idproduct.ToString()+"'"+
									"AND id_producto < '"+lastproduct.ToString()+"'"+
									"ORDER BY id_producto DESC LIMIT 1;";
				//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if ((bool) lector.Read()){
					newidproduct = long.Parse((string) lector["idproducto"]) + 1;
					lector.Close ();
				}else{
					newidproduct = idproduct + 1;
					lector.Close ();
				}
				// Actualiza entry 
				this.entry_codigo_producto.Text = newidproduct.ToString();
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message );
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}					
		
		// Activa el enter en la busqueda de los productos
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione
		public void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				llena_la_lista_de_productos();				
			}
		}
		
		decimal busca_porcentage_utilidad (decimal precio_uni)
		{
			decimal porcentageutilidad = 0;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();

				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT precio_costo_inicial,precio_costo_final,porcentage_de_ganancia "+ 
									"FROM hscmty_erp_tabla_ganancia "+
									"WHERE precio_costo_final >= '"+precio_uni.ToString()+"'"+
									"AND precio_costo_inicial <= '"+precio_uni.ToString()+"' "+
									"LIMIT 1;";
				//Console.WriteLine(comando.CommandText.ToString());
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if ((bool) lector.Read()){
					porcentageutilidad  = (decimal) lector["porcentage_de_ganancia"];
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message );
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close();
			
			return (porcentageutilidad);
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
		}					
	}
}
