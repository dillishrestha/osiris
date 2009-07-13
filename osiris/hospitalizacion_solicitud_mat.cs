////////////////////////////////////////////////////////////
// created on 08/05/2007 at 09:26 a
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
//				  Daniel Olivares - arcangeldoc@gmail.com (Diseño de Pantallas Glade)
// 				  
// Licencia		: GLP
// S.O. 		: GNU/Linux RH4 ES
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
// Proposito	: Solicitud de materiales para hospitalizacion 
// Objeto		: hospitalizacion_solicitud_mat.cs 
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
	public class solicitud_material_hospital
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		//[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		
		// Declarando ventana principal de Hospitalizacion
		[Widget] Gtk.Window solicitud_materiales;
		[Widget] Gtk.Entry entry_numero_solicitud;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.TreeView lista_produc_solicitados;
		
		[Widget] Gtk.Entry entry_rojo;
		[Widget] Gtk.Entry entry_azul;
		[Widget] Gtk.Entry entry_verde;
		
		/////// Ventana Busqueda de productos\\\\\\\\
		[Widget] Gtk.TreeView lista_de_producto;
		//[Widget] Gtk.Button button_agrega_extra;
		[Widget] Gtk.Entry entry_cantidad_aplicada;
		
		//private TreeStore treeViewEngineBusca;
		private TreeStore treeViewEngineBusca2;
		private ListStore treeViewEngineSolicitud;
		
		//private ArrayList arraysolicitudmat;
		
		public string LoginEmpleado;
		public string NomEmpleado;
		public string AppEmpleado;
		public string ApmEmpleado;
		public string nombrebd;
		public string connectionString = "Server=192.168.1.4;" +
						"Port=5432;" +
						"User ID=admin1;" +
						"Password=1qaz2wsx;";
		
		public float valoriva = 15;
		
		public solicitud_material_hospital(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = _nombrebd_;
			
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "solicitud_materiales", null);
			gxml.Autoconnect (this);        
			////// Muestra ventana de Glade
			solicitud_materiales.Show();
			
			// acciones de botones
			// Validando que sen solo numeros
			entry_numero_solicitud.KeyPressEvent += onKeyPressEvent_enter_solicitud;
			// Busqueda de Productos
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			// Colores de los cuadros		
			entry_rojo.ModifyBase(StateType.Normal, new Gdk.Color(255,0,0));
			entry_azul.ModifyBase(StateType.Normal, new Gdk.Color(0,0,255));
			entry_verde.ModifyBase(StateType.Normal, new Gdk.Color(0,255,0));
			
			crea_treeview_solicitud();
		}
		
		void on_button_busca_producto_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			
			crea_treeview_busqueda("producto");
			
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			
			//button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
			
			// Validando que sen solo numeros
			entry_cantidad_aplicada.KeyPressEvent += onKeyPressEvent;
	    }
	    
	    // Valida entradas que solo sean numericas, se utiliza en ventana de
		// carga de numero de solicitud
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_solicitud(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				//llenado_de_productos_aplicados( (string) entry_folio_servicio.Text );				
			}
			string misDigitos = "0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮ）";
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
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮ）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
			{
				args.RetVal = true;
			}
		}
				
		void crea_treeview_busqueda(string tipo_busqueda)
		{
			if (tipo_busqueda == "solicitud")
			{
				
			}
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
													typeof(string));
				lista_de_producto.Model = treeViewEngineBusca2;
			
				lista_de_producto.RulesHint = true;
			
				//lista_de_producto.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono paciente*/
				
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
            	
				TreeViewColumn col_grupoprod = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_grupoprod.Title = "Grupo Producto";//Precio Producto
				col_grupoprod.PackStart(cellrt2, true);
				col_grupoprod.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
				col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
            
				TreeViewColumn col_grupo1prod = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_grupo1prod.Title = "Grupo1 Producto";//I.V.A.
				col_grupo1prod.PackStart(cellrt3, true);
				col_grupo1prod.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 2 en vez de 3
				col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
            
				TreeViewColumn col_grupo2prod = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_grupo2prod.Title = "Grupo2 Producto";//Total
				col_grupo2prod.PackStart(cellrt4, true);
				col_grupo2prod.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 3 en vez de 4
				col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;
            	
				lista_de_producto.AppendColumn(col_idproducto);  // 0
				lista_de_producto.AppendColumn(col_desc_producto); // 1
				lista_de_producto.AppendColumn(col_grupoprod);	//7
				lista_de_producto.AppendColumn(col_grupo1prod);	//8
				lista_de_producto.AppendColumn(col_grupo2prod);	//9							
			}
		}
		
		// llena la lista de productos
 		void on_llena_lista_producto_clicked (object sender, EventArgs args)
 		{
 			treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				comando.CommandText = "SELECT to_char(hscmty_productos.id_producto,'999999999999') AS codProducto,"+
						"hscmty_productos.descripcion_producto,to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
						"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,"+
						"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
						"to_char(porcentage_ganancia,'99999.99') AS porcentageutilidad,to_char(costo_producto,'999999999.99') AS costoproducto "+
						"FROM hscmty_productos,hscmty_grupo_producto,hscmty_grupo1_producto,hscmty_grupo2_producto "+
						"WHERE hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
						"AND hscmty_productos.id_grupo1_producto = hscmty_grupo1_producto.id_grupo1_producto "+
						"AND hscmty_productos.id_grupo2_producto = hscmty_grupo2_producto.id_grupo2_producto "+
						"AND hscmty_productos.cobro_activo = true "+
						"AND hscmty_productos.id_grupo_producto <= '7' "+
						"AND hscmty_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper()+"%' ORDER BY descripcion_producto; ";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				float tomaprecio;
				float calculodeiva;
				float preciomasiva;
				float tomadescue;
				float preciocondesc;
							
				while (lector.Read())
				{
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
					treeViewEngineBusca2.AppendValues (//TreeIter iter =
											(string) lector["codProducto"],//0
											(string) lector["descripcion_producto"],//1
											(string) lector["descripcion_grupo_producto"],//2
											(string) lector["descripcion_grupo1_producto"],//3
											(string) lector["descripcion_grupo2_producto"],//4
											(string) lector["preciopublico"],//2-5
											calculodeiva.ToString("F").PadLeft(10),//3-6
											preciomasiva.ToString("F").PadLeft(10),//4-7
											(string) lector["porcentagesdesc"],//8
											preciocondesc.ToString("F").PadLeft(10),//9
											(string) lector["costoproductounitario"],//10
											(string) lector["porcentageutilidad"],//11
											(string) lector["costoproducto"]);//12
					
				}
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			}
			conexion.Close ();
		}
		
		enum Column_prod
		{
			col_idproducto,
			col_desc_producto,
			col_grupoprod,
			col_grupo1prod,
			col_grupo2prod
		}
		
		void crea_treeview_solicitud()
		{
			//arraysolicitudmat = new ArrayList();
			
			treeViewEngineSolicitud = new ListStore(typeof(string), 
													typeof(string));
												
			lista_produc_solicitados.Model = treeViewEngineSolicitud;
			
			lista_produc_solicitados.RulesHint = true;
			
			TreeViewColumn col_descripcion = new TreeViewColumn();
			CellRendererText cel_descripcion = new CellRendererText();
			col_descripcion.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
			col_descripcion.PackStart(cel_descripcion, true);
			col_descripcion.AddAttribute (cel_descripcion, "text", 0);
			
			//col_descripcion_prod.SetCellDataFunc(cel_descripcion, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_codigo_prod = new TreeViewColumn();
			CellRendererText cel_codigo_prod = new CellRendererText();
			col_codigo_prod.Title = "Codigo"; // titulo de la cabecera de la columna, si está visible
			col_codigo_prod.PackStart(cel_codigo_prod, true);
			col_codigo_prod.AddAttribute (cel_codigo_prod, "text", 1);
																	
			lista_produc_solicitados.AppendColumn(col_descripcion);
			lista_produc_solicitados.AppendColumn(col_codigo_prod);
						
		}
		
		enum colum_solicitudes
		{
			col_descripcion,
			col_codigo_prod
			
		}
		
		/*
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;

 			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)) 
 			{
 				id_produ = (string) model.GetValue(iterSelected, 0);
				desc_produ = (string) model.GetValue(iterSelected, 1);
				precio_produ = (string) model.GetValue(iterSelected, 5);
				iva_produ = (string) model.GetValue(iterSelected, 6);
				total_produ = (string) model.GetValue(iterSelected, 7);
				descuent_produ = (string) model.GetValue(iterSelected, 8);
				pre_con_desc_produ = (string) model.GetValue(iterSelected, 9);
				//valor_descuento = float.Parse(this.precio_produ)-float.Parse(this.pre_con_desc_produ);
				costo_unitario_producto = (string) model.GetValue(iterSelected, 10); 
				porcentage_utilidad_producto = (string) model.GetValue(iterSelected, 11);
				costo_total_producto = (string) model.GetValue(iterSelected, 12);
				entry_desc_producto.Text = desc_produ;
				string constante = entry_cantidad_aplicada.Text;
				//varibles numericas
				ppcantidad = float.Parse(precio_produ)*float.Parse(constante);
				 
				float ivaproducto = float.Parse(iva_produ)*float.Parse(constante);
				float suma_total = ppcantidad+ivaproducto;
				float preciocondesc = float.Parse(pre_con_desc_produ)*float.Parse(constante);
				valor_descuento = ppcantidad-preciocondesc;
				float costotprodu = preciocondesc;
				
				if ((string) constante != ""){
					if ((float) float.Parse(constante) > 0){
						if ((int) idtipointernamiento >= 20){
							if ((string) entry_desc_producto.Text.Trim() == ""){}else{
								Item foo;
								foo = new Item (true,
									float.Parse(constante),
									this.id_produ,
									this.desc_produ,
									this.precio_produ.PadLeft(10),
									ppcantidad.ToString(),
									ivaproducto.ToString("F").PadLeft(10),
									suma_total.ToString("F").PadLeft(10),
									this.descuent_produ.PadLeft(10),
									this.valor_descuento.ToString("F"),
									preciocondesc.ToString("F").PadLeft(10),
									LoginEmpleado,
									DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
									descripinternamiento,
									idtipointernamiento,
									costo_unitario_producto,
									porcentage_utilidad_producto,
									costotprodu.ToString("F").PadLeft(10));
				
								arraysolicitud.Add(foo);
								treeViewEngineSolicitud.AppendValues (true,
													float.Parse(constante),
													this.id_produ,
													this.desc_produ,
													LoginEmpleado,
													DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
													descripinternamiento,
													this.precio_produ.PadLeft(10),
													ppcantidad.ToString(),
													ivaproducto.ToString("F").PadLeft(10),
													suma_total.ToString("F").PadLeft(10),
													this.descuent_produ.PadLeft(10),
													this.valor_descuento.ToString("F").PadLeft(10),
													preciocondesc.ToString("F").PadLeft(10),
													idtipointernamiento,
													costo_unitario_producto,
													porcentage_utilidad_producto,
													costotprodu.ToString("F").PadLeft(10));
				//Console.WriteLine(id_produ+" "+desc_produ+" "+constante+" "+precio_produ+" "+ppcantidad.ToString()+" "+ivaprodu+" "+totalprodu+" "+
								 // descuent_produ+" "+valor_descuento+" "+costo_total_producto);
							}								
						}else{
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, 
							ButtonsType.Close, "Seleccione el lugar o departamento donde \n"+
									"se genero el cargo para el paciente");
							msgBoxError.Run ();
							msgBoxError.Destroy();
						}
					}else{
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, 
						ButtonsType.Close, "La cantidad que quiere aplicar debe ser \n"+
									"mayor que cero, intente de nuevo");
						msgBoxError.Run ();
						msgBoxError.Destroy();
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, 
					ButtonsType.Close, "La cantidad que quiere aplicar NO \n"+
									"puede quedar vacia, intente de nuevo");
					msgBoxError.Run ();
					msgBoxError.Destroy();
				}
 			}
 		}*/
				
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}