///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Juan Antonio Peña (Programacion Secundaria)
//                Ing. Hector vargas (Diseño Glade)
//				  Ing. Daniel Olivares (Ajustes Varios)
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

namespace osiris
{
	public class nuevos_prod
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		
		// Declarando ventana principal
		[Widget] Gtk.Window producto_nuevo;
		[Widget] Gtk.Entry entry_descripcion;
		[Widget] Gtk.Entry entry_nombre_articulo;
		[Widget] Gtk.Entry entry_nombre_generico;
		[Widget] Gtk.Entry entry_costo;
		[Widget] Gtk.Entry entry_embalaje;
		[Widget] Gtk.Entry entry_precio_unitario;
		[Widget] Gtk.Entry entry_porciento_utilidad;
		[Widget] Gtk.Entry entry_utilidad;
		[Widget] Gtk.Entry entry_descuento;
		[Widget] Gtk.Entry entry_codigo_producto;
		[Widget] Gtk.Entry entry_precio_publico;
		[Widget] Gtk.Entry entry_tipo_grupo;
		[Widget] Gtk.Entry entry_tipo_grupo1;
		[Widget] Gtk.Entry entry_tipo_grupo2;
		[Widget] Gtk.Entry entry_producto_anidado;
		[Widget] Gtk.Entry entry_precios_costunitario_sannico;
		[Widget] Gtk.Entry entry_precio_total_sannico;
				
		[Widget] Gtk.Button button_grabar;
		[Widget] Gtk.Button button_calcular;
		[Widget] Gtk.Button button_limpiar;
		[Widget] Gtk.Button button_actualizar;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.Button button_busca_producto_anidado;
		[Widget] Gtk.Button button_quita_producto_anidado;
				
		[Widget] Gtk.ComboBox combobox_grupo;
		[Widget] Gtk.ComboBox combobox_grupo1;
		[Widget] Gtk.ComboBox combobox_grupo2;
		
		[Widget] Gtk.RadioButton radiobutton_costounico_si;
		[Widget] Gtk.RadioButton radiobutton_costounico_no;
		
		[Widget] Gtk.RadioButton radiobutton_desc_si;
		[Widget] Gtk.RadioButton radiobutton_desc_no;
		
		[Widget] Gtk.CheckButton checkbutton_apl_iva;
		[Widget] Gtk.CheckButton checkbutton_producto_anidado;
		[Widget] Gtk.CheckButton checkbutton_prod_activo;
		
		//Declarando la barra de estado
		[Widget] Gtk.Statusbar statusbar_alta_mod_prod;
		
		[Widget] Gtk.TreeView lista_de_producto_anidado;
		
		//Declaracion de ventana de busqueda de productos
		[Widget] Gtk.Window busca_producto;
		[Widget] Gtk.RadioButton radiobutton_nombre;
		[Widget] Gtk.RadioButton radiobutton_codigo;
		[Widget] Gtk.TreeView lista_de_producto;
		
		// Declaracion de variables publicas
		long idproduct = 0;
		long lastproduct = 0;
		long newidproduct = 0;
		long newidsecuencia = 0;
		
		long idtipogrupo = 0;
		long idtipogrupo1 = 0;
		long idtipogrupo2 = 0;
		string descripgrupo = "";
		string descripgrupo1 =  "";
		string descripgrupo2 = "";
		string apldesc;
		bool aplicariva_producto;
	 	bool cobroactivo_producto;
	 	string costounico;
	 	string tiposeleccion = "";
	 	decimal precio_uni = 0;
	 	
		// Almacena los valores anterios para guardar los cuando actualiza algun precio, o descripcion
	 	decimal precio_unitario_anterior = 0;
	 	decimal precio_costo_anterior = 0;
		decimal utilidad_anterior = 0;
	 	
	 	//VARIABLES PARA CARGAR DATOS
	 	string codprod ="";
	 	string preciopub ="";
	 	string precio ="";
	 	string preciouni ="";
	 	string porcientoutilidad ="";
	 	string descripcionprod ="";
	 	string nombreart ="";
	 	string nombregen ="";
	 	string embalajeprod ="";
	 	string porcentagedesc ="";
		
		float valoriva;
	 	
		string connectionString;
		string nombrebd;
		string LoginEmpleado;
		
		TreeStore treeViewEngineBusca2;
		TreeStore treeViewEngineBusca3;
		//declaracion de columnas y celdas de treeview de busqueda
		TreeViewColumn col_idproducto;		CellRendererText cellr0;
		TreeViewColumn col_desc_producto;	CellRendererText cellr1;
		TreeViewColumn col_precioprod;		CellRendererText cellrt2;
		TreeViewColumn col_ivaprod;			CellRendererText cellrt3;
		TreeViewColumn col_totalprod;		CellRendererText cellrt4;
		TreeViewColumn col_descuentoprod;	CellRendererText cellrt5;
		TreeViewColumn col_preciocondesc;	CellRendererText cellrt6;
		TreeViewColumn col_grupoprod;		CellRendererText cellrt7;
		TreeViewColumn col_grupo1prod;		CellRendererText cellrt8;
		TreeViewColumn col_grupo2prod;		CellRendererText cellrt9;
		
		TreeViewColumn col_costoprod_uni;	CellRendererText cellrt12;
		
		TreeViewColumn col_aplica_iva;		CellRendererText cellrt19;
		TreeViewColumn col_cobro_activo;	CellRendererText cellrt20;		
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public nuevos_prod(string LoginEmp, string NomEmpleado, string AppEmpleado, string ApmEmpleado, string nombrebd_ ) 
		{
			LoginEmpleado = LoginEmp;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;			
			valoriva = float.Parse(classpublic.ivaparaaplicar);			
			
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "producto_nuevo", null);
			gxml.Autoconnect (this);
	        // Muestra ventana de Glade
	        producto_nuevo.Show();
	        
	        this.button_grabar.Sensitive = false;
	      	this.button_actualizar.Sensitive = false;
	      	this.button_busca_producto_anidado.Sensitive = false;
		    this.button_quita_producto_anidado.Sensitive = false;
		    this.entry_producto_anidado.Sensitive = false;
		    this.lista_de_producto_anidado.Sensitive = false;
	    	
	    	this.entry_costo.KeyPressEvent += onKeyPressEvent_only_numbers;
	    	this.entry_embalaje.KeyPressEvent += onKeyPressEvent_only_numbers;
	    	this.entry_porciento_utilidad.KeyPressEvent += onKeyPressEvent_only_numbers;
	    	this.entry_precio_publico.KeyPressEvent += onKeyPressEvent_only_numbers;
	    	this.entry_precio_unitario.KeyPressEvent += onKeyPressEvent_only_numbers;
	    	this.entry_utilidad.KeyPressEvent += onKeyPressEvent_only_numbers;
	    	this.entry_precios_costunitario_sannico.KeyPressEvent += onKeyPressEvent_only_numbers;
	    	
	    	// Activacion de grabacion de informacion
	    	button_grabar.Clicked += new EventHandler(on_button_grabar_clicked);
			//activa y desactiva anidados
			this.checkbutton_producto_anidado.Clicked += new EventHandler(on_checkbutton_producto_anidado_clicked);
			// activa si el producto se habilitado en caja y Almace
			this.checkbutton_prod_activo.Clicked += new EventHandler(on_checkbutton_prod_activo_clicked);
			// Aplicacion de Iva
			this.checkbutton_apl_iva.Clicked += new EventHandler(on_checkbutton_apl_iva_clicked);
			// Activacion de boton de busqueda
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			//busqueda de anidados 
			this.button_busca_producto_anidado.Clicked += new EventHandler(on_button_busca_producto_clicked);
			//limpiar datos
			this.button_limpiar.Clicked += new EventHandler(on_button_limpiar_clicked);
			//Calcula la utilidad
			this.button_calcular.Clicked += new EventHandler(on_button_calcula_utilidad_clicked);
			//actualizar datos
			this.button_actualizar.Clicked += new EventHandler(on_button_actulizar_clicked);
			//boton salir
			// Sale de la ventana
			this.button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			statusbar_alta_mod_prod.Pop(0);
			statusbar_alta_mod_prod.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_alta_mod_prod.HasResizeGrip = false;
			
			llenado_de_cmbox_grupos();
			crea_treeview_busqueda3();
		}
		
		void on_checkbutton_apl_iva_clicked(object sender, EventArgs a)
		{
			if( this.checkbutton_apl_iva.Active == true){
				aplicariva_producto = true;
			}else{
				aplicariva_producto = false;
			}
		}
		
		void on_checkbutton_prod_activo_clicked(object sender, EventArgs a)
		{
			if(this.checkbutton_prod_activo.Active == true){
				cobroactivo_producto = true;
			}else{
				cobroactivo_producto = false;
			}
		}
		
		void on_checkbutton_producto_anidado_clicked(object sender, EventArgs a)
		{
			 
			if(this.checkbutton_producto_anidado.Active == true){
	        	this.tiposeleccion = "anidado";
	        	this.button_busca_producto_anidado.Sensitive = true;
		        this.button_quita_producto_anidado.Sensitive = true;
		        this.entry_producto_anidado.Sensitive = true;
		        this.lista_de_producto_anidado.Sensitive = true;
		        this.button_busca_producto.Sensitive = false;
	        }else{
	        	this.button_busca_producto_anidado.Sensitive = false;
		        this.button_quita_producto_anidado.Sensitive = false;
		        this.entry_producto_anidado.Sensitive = false;
		        this.lista_de_producto_anidado.Sensitive = false;
		        this.button_busca_producto.Sensitive = true;
		        this.tiposeleccion = "";
	    	}
		}
		
		void llenado_de_cmbox_grupos()
		{
			//////////////////LLENADO DE COMBOBOXES//////////////////////
			// Llenado de combobox grupo
			combobox_grupo.Clear();
			CellRendererText cell2 = new CellRendererText();
			combobox_grupo.PackStart(cell2, true);
			combobox_grupo.AddAttribute(cell2,"text",0);
		        
			ListStore store_grupo = new ListStore( typeof (string), typeof (int));
			combobox_grupo.Model = store_grupo;
			//llenado de tabla de combobox
			llena_cmbox_grupo(store_grupo);
			
			TreeIter iter2;
			if (store_grupo.GetIterFirst(out iter2))
			{
				//Console.WriteLine(iter2);
				combobox_grupo.SetActiveIter (iter2);
			}
			combobox_grupo.Changed += new EventHandler (onComboBoxChanged_grupo);
			
			// Llenado de combobox grupo1
			//combobox_grupo1.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_grupo1.PackStart(cell3, true);
			combobox_grupo1.AddAttribute(cell3,"text",0);
		       
			ListStore store_grupo1 = new ListStore( typeof (string), typeof (int));
			combobox_grupo1.Model = store_grupo1;
			// lleno de la tabla 
			llena_cmbox_grupo1(store_grupo1);
							TreeIter iter3;
			if (store_grupo1.GetIterFirst(out iter3))
			{
				///Console.WriteLine(iter2);
				combobox_grupo1.SetActiveIter (iter3);
			}
			combobox_grupo1.Changed += new EventHandler (onComboBoxChanged_grupo1);
				// Llenado de combobox grupo
			//combobox_grupo2.Clear();
			CellRendererText cell4 = new CellRendererText();
			combobox_grupo2.PackStart(cell4, true);
			combobox_grupo2.AddAttribute(cell4,"text",0);
		       
			ListStore store_grupo2 = new ListStore( typeof (string), typeof (int));
			combobox_grupo2.Model = store_grupo2;
			// lleno de la tabla 
			llena_cmbox_grupo2(store_grupo2);
			
			TreeIter iter4;
			if (store_grupo2.GetIterFirst(out iter4))
			{
				//Console.WriteLine(iter2);
				combobox_grupo2.SetActiveIter (iter4);
			}
			combobox_grupo2.Changed += new EventHandler (onComboBoxChanged_grupo2);
		}	
			
		void llena_cmbox_grupo(ListStore store_grupo)
		{
			//store_aseguradora.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
           // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				comando.CommandText = "SELECT * FROM osiris_grupo_producto ORDER BY descripcion_grupo_producto ;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//Console.WriteLine(comando.CommandText.ToString());
				while (lector.Read())
				{
					store_grupo.AppendValues((string) lector["descripcion_grupo_producto"],(int) lector["id_grupo_producto"]);
				}
				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message );
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void llena_cmbox_grupo1(ListStore store_grupo1)
		{
			//store_aseguradora.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
           // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				comando.CommandText = "SELECT * FROM osiris_grupo1_producto ORDER BY descripcion_grupo1_producto ;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				while (lector.Read())
				{
					store_grupo1.AppendValues((string) lector["descripcion_grupo1_producto"],(int) lector["id_grupo1_producto"]);
				}
				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message );
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void llena_cmbox_grupo2(ListStore store_grupo2)
		{
			//store_aseguradora.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
           // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
               	comando.CommandText = "SELECT * FROM osiris_grupo2_producto ORDER BY descripcion_grupo2_producto ;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				while (lector.Read())
				{
					store_grupo2.AppendValues((string) lector["descripcion_grupo2_producto"],(int) lector["id_grupo2_producto"]);
				}
				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message );
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_button_busca_producto_clicked(object sender, EventArgs a)
		{
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			busca_producto.Show();
			
			crea_treeview_busqueda();
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
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
				decimal porc_utilidad = decimal.Parse( this.entry_porciento_utilidad.Text, System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
				
				precio_uni = cost_prod/embalaje_prod;
				
				decimal utilidad_prod = precio_uni*porc_utilidad/100;
				decimal precio_pub = precio_uni+utilidad_prod;
				
				//Console.WriteLine(cost_prod.ToString("F")+" %util"+porc_utilidad.ToString("F").Replace(",",".")+" PUni"+precio_uni.ToString("F").Replace(",",".")
				//						+" Util"+	utilidad_prod.ToString("F").Replace(",",".")+" PP"+precio_pub.ToString("F").Replace(",","."));
				this.entry_utilidad.Text = utilidad_prod.ToString("F").Replace(",",".");
				this.entry_precio_unitario.Text = precio_uni.ToString("F").Replace(",",".");
				this.entry_precio_publico.Text = precio_pub.ToString("F").Replace(",",".");
				if(this.button_actualizar.Sensitive == false){
				this.button_grabar.Sensitive = true;
				}
				//this.button_actualizar.Sensitive = false;
				
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Info, 
								ButtonsType.Close, "Escriba el costo y embalaje del producto");
								msgBoxError.Run ();
								msgBoxError.Destroy();
			}
		}
		
		
		void on_button_grabar_clicked(object sender, EventArgs a)
		{
			
			long primera = (idtipogrupo*10000000000);
			long segundo = (idtipogrupo1*100000000);
			long tercero = (idtipogrupo2*100000);
			
			idproduct = primera+segundo+tercero;
			
			ultimo_id_prod();
						
			entry_codigo_producto.Text = newidproduct.ToString();
			if (idtipogrupo > 1){ 
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
														MessageType.Question,ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
	 			
	 			if (miResultado == ResponseType.Yes)
				{
					if (this.radiobutton_desc_si.Active == true){
						apldesc = "true";
					}
					if (this.radiobutton_desc_no.Active == true){
						apldesc = "false";
					}
					
					if(this.radiobutton_costounico_si.Active == true){
						costounico = "true";
					}
					if(this.radiobutton_costounico_no.Active == true){
						costounico = "false";
					}
						
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
			        // Verifica que la base de datos este conectada
					try{
							conexion.Open ();
							NpgsqlCommand comando; 
							comando = conexion.CreateCommand ();
										 				
							comando.CommandText = "INSERT INTO osiris_productos("+
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
									 				"aplica_descuento,"+
									 				"cobro_activo,"+
									 				"tiene_kit,"+
									 				"nombre_articulo,"+
									 				"nombre_generico_articulo,"+
									 				"cantidad_de_embalaje,"+
									 				"porcentage_descuento,"+
									 				"costo_unico,"+
									 				"precio_producto_publico1,"+
									 				"precio_producto_5003) "+
									 				"VALUES ('"+
									 				long.Parse(entry_codigo_producto.Text)+"','"+
									 				this.entry_descripcion.Text.ToUpper()+"','"+
									 				idtipogrupo+"','"+
									 				idtipogrupo1+"','"+
									 				idtipogrupo2+"','"+
									 				float.Parse(this.entry_precio_publico.Text) +"','"+
									 				float.Parse(this.entry_costo.Text)+"','"+
									 				float.Parse(this.entry_precio_unitario.Text)+"','"+
									 				float.Parse(this.entry_porciento_utilidad.Text)+"','"+
									 				LoginEmpleado+"','"+
									 				DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
									 				(string) aplicariva_producto.ToString().ToLower().Trim()+"','"+
									 				apldesc+"','"+
									 				(string) cobroactivo_producto.ToString().ToLower().Trim()+"','"+
									 				"false"+"','"+
									 				this.entry_nombre_articulo.Text.ToUpper()+"','"+
									 				this.entry_nombre_generico.Text.ToUpper()+"','"+
									 				float.Parse(this.entry_embalaje.Text)+"','"+
									 				float.Parse(this.entry_descuento.Text)+"','"+
									 				costounico+"','"+
									 				float.Parse(this.entry_precios_costunitario_sannico.Text)+"','"+
									 				this.entry_precios_costunitario_sannico.Text+"');";
			 			comando.ExecuteNonQuery();
			        	comando.Dispose();
			        	
			        	this.button_grabar.Sensitive = false;
			        	this.entry_codigo_producto.Text = "";
						this.entry_costo.Text= "0.00";
						this.entry_descripcion.Text = "";
						this.entry_descuento.Text ="0.00";
						this.entry_embalaje.Text = "0.00";
						this.entry_nombre_articulo.Text = "";
						this.entry_nombre_generico.Text = "";
						this.entry_porciento_utilidad.Text = "0.00";
						this.entry_precio_publico.Text = "0.00";
						this.entry_precio_unitario.Text ="0.00";
						this.entry_utilidad.Text ="0.00";
						this.entry_precios_costunitario_sannico.Text = "0.00";
						
						this.combobox_grupo.Clear();
						this.combobox_grupo1.Clear();
						this.combobox_grupo2.Clear();
						this.entry_tipo_grupo.Text ="";
						this.entry_tipo_grupo1.Text ="";
						this.entry_tipo_grupo2.Text ="";
						
						llenado_de_cmbox_grupos();
						msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
											MessageType.Info,ButtonsType.Ok,"El producto se guardo satisfactoriamente\n"+
											" con el codigo: "+this.newidproduct.ToString());
						miResultado = (ResponseType)msgBox.Run ();
						msgBox.Destroy();
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
				    }
					conexion.Close ();
				}else{
					this.entry_codigo_producto.Text = "";
				}
		    }else{
		 		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, 
								ButtonsType.Close, "Debe asignar un GRUPO de producto para poder grabar");
								msgBoxError.Run ();
								msgBoxError.Destroy();
		 	}
		}
		
		void onComboBoxChanged_grupo (object sender, EventArgs args)
		{
	    	ComboBox combobox_grupo = sender as ComboBox;
			if (sender == null)
			{
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_grupo.GetActiveIter (out iter))
		    	{
		    		idtipogrupo = (int) combobox_grupo.Model.GetValue(iter,1);
		    		//Console.WriteLine(idtipogrupo.ToString());
		    		descripgrupo = (string) combobox_grupo.Model.GetValue(iter,0);
		    		this.entry_tipo_grupo.Text = descripgrupo; 
	     		}
		}
		
		void onComboBoxChanged_grupo1(object sender, EventArgs args)
		{
	    	ComboBox combobox_grupo1 = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_grupo1.GetActiveIter (out iter))
		    	{
		    		idtipogrupo1 = (int) combobox_grupo1.Model.GetValue(iter,1);
		    		//Console.WriteLine(idtipogrupo1.ToString());
		    		descripgrupo1 = (string) combobox_grupo1.Model.GetValue(iter,0);
		    		this.entry_tipo_grupo1.Text = descripgrupo1;
	     		}
		}
		
		void onComboBoxChanged_grupo2(object sender, EventArgs args)
		{
		
	    	ComboBox combobox_grupo2 = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_grupo2.GetActiveIter (out iter))
		    	{
		    		idtipogrupo2 = (int) combobox_grupo2.Model.GetValue(iter,1);
		    		//Console.WriteLine(idtipogrupo2.ToString());
		    		descripgrupo2 = (string) combobox_grupo2.Model.GetValue(iter,0);
		    		this.entry_tipo_grupo2.Text = descripgrupo2;
	     		}
		}
		
		public long ultimo_id_prod()
		{
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
									"FROM osiris_productos "+
									"WHERE id_producto > '"+idproduct.ToString()+"'"+
									"AND id_producto < '"+lastproduct.ToString()+"'"+
									"ORDER BY id_producto DESC LIMIT 1;";
				//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if ((bool) lector.Read())
				{
					newidproduct = long.Parse((string) lector["idproducto"]) + 1;
					//Console.WriteLine("lector con valor: "+newidproduct.ToString());
					lector.Close ();
				}else{
					newidproduct = idproduct + 1;
					//Console.WriteLine("lector cero: "+newidproduct.ToString());
					lector.Close ();
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message );
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
			return newidproduct;
		}	
		
		// declara y crea el treeviev de Producto en la busqueda
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
				col_desc_producto.Resizable = true;
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
		
		// Este treeviev es para los productos anidados
		void crea_treeview_busqueda3()
		{
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
				//cellr0.Editable = true;   // Permite edita este campo
            
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
				
				col_costoprod_uni = new TreeViewColumn();
				cellrt12 = new CellRendererText();
				col_costoprod_uni.Title = "Precio Unitario";
				col_costoprod_uni.PackStart(cellrt12, true);
				col_costoprod_uni.AddAttribute (cellrt12, "text", 12); // la siguiente columna será 1 en vez de 2
				col_costoprod_uni.SortColumnId = (int) Column_prod.col_costoprod_uni;
				
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
				lista_de_producto.AppendColumn(col_costoprod_uni); //12
				
				
		}
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
			col_grupo2prod,
			col_nom_art,			
			col_nom_gen,
			col_costoprod_uni,		
			col_porc_util,
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
			if(radiobutton_nombre.Active == true) {query_tipo_busqueda = "AND osiris_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_producto; "; }
			if(radiobutton_codigo.Active == true) {query_tipo_busqueda = "AND osiris_productos.id_producto LIKE '"+entry_expresion.Text.Trim()+"%'  ORDER BY id_producto; "; }
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
							"osiris_productos.descripcion_producto,osiris_productos.nombre_articulo,osiris_productos.nombre_generico_articulo, "+
							"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(precio_producto_publico1,'99999999.99') AS preciopublico1,"+
							"to_char(cantidad_de_embalaje,'99999999.99') AS cantidadembalaje,"+
							"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,cobro_activo,costo_unico,"+
							"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(osiris_productos.id_grupo_producto,'99999') AS idgrupoproducto,osiris_productos.id_grupo_producto, "+
							"to_char(osiris_productos.id_grupo1_producto,'99999') AS idgrupo1producto,osiris_productos.id_grupo1_producto, "+
							"to_char(osiris_productos.id_grupo2_producto,'99999') AS idgrupo2producto,osiris_productos.id_grupo2_producto, "+
							"to_char(porcentage_ganancia,'99999.999') AS porcentageutilidad,to_char(costo_producto,'999999999.99') AS costoproducto "+
							"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
							"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
							"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
							"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
							query_tipo_busqueda;
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				float calculodeiva;
				float preciomasiva;
				float preciocondesc;
				float tomaprecio;
				float tomadescue;
											
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
					 
					treeViewEngineBusca2.AppendValues (
									(string) lector["codProducto"] ,//0
									(string) lector["descripcion_producto"],
									(string) lector["preciopublico"],
									calculodeiva.ToString("F").PadLeft(10).Replace(",","."),
									preciomasiva.ToString("F").PadLeft(10).Replace(",","."),
									(string) lector["porcentagesdesc"],
									preciocondesc.ToString("F").PadLeft(10).Replace(",","."),
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
		
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{	
			string toma_valor;
			TreeModel model;
			TreeIter iterSelected;
			//ListStore store_grupo = new ListStore( typeof (string), typeof (int));
			if( this.tiposeleccion == "anidado"){
				
			}else{
				if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
				
					utilidad_anterior = decimal.Parse((string) model.GetValue(iterSelected,14));
							
	 				toma_valor = (string) model.GetValue(iterSelected, 0);
	 				
	 				this.entry_codigo_producto.Text = (string) model.GetValue(iterSelected, 0);
					this.entry_descripcion.Text = (string) model.GetValue(iterSelected, 1);
					this.entry_nombre_articulo.Text = (string) model.GetValue(iterSelected, 10); 
					this.entry_nombre_generico.Text = (string) model.GetValue(iterSelected, 11);
					toma_valor = (string) model.GetValue(iterSelected, 2);
					
					this.entry_precio_publico.Text = toma_valor.TrimStart();
					toma_valor = (string) model.GetValue(iterSelected, 5);
					
					this.entry_descuento.Text = toma_valor.TrimStart();
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
					this.entry_precios_costunitario_sannico.Text = toma_valor.Trim();

					
					// llenar conbobox con los nombres de los grupos
					this.combobox_grupo.Clear();
					this.combobox_grupo1.Clear();
					this.combobox_grupo2.Clear();
					
					llenado_de_cmbox_grupos();
					
					this.descripgrupo = (string) model.GetValue(iterSelected, 7);
					this.idtipogrupo = long.Parse((string) model.GetValue(iterSelected, 16));
					this.descripgrupo1 = (string) model.GetValue(iterSelected, 8);
					this.idtipogrupo1 = long.Parse((string) model.GetValue(iterSelected, 17));
					this.descripgrupo2 = (string) model.GetValue(iterSelected, 9);
					this.idtipogrupo2 = long.Parse((string) model.GetValue(iterSelected, 18));
					this.entry_tipo_grupo.Text = descripgrupo.ToString(); 
					this.entry_tipo_grupo1.Text = descripgrupo1.ToString();
					this.entry_tipo_grupo2.Text = descripgrupo2.ToString();
					
					toma_valor = (string) model.GetValue(iterSelected, 14);
					this.entry_precios_costunitario_sannico.Text = (string) model.GetValue(iterSelected, 22);
					
					calculando_utilidad();
					
					this.button_actualizar.Sensitive = true;
					this.button_grabar.Sensitive = false;
					
					this.checkbutton_apl_iva.Active = (bool) model.GetValue(iterSelected, 19);
					this.checkbutton_prod_activo.Active = (bool) model.GetValue(iterSelected, 20);
					
					if ((bool) model.GetValue(iterSelected, 21) == true){ 
						this.radiobutton_desc_si.Active = true;
						apldesc = "true";
					}else{
						this.radiobutton_desc_no.Active = true;
						apldesc = "false";	
					}
					
					//cierra la ventana despues que almaceno la informacion en variables
					Widget win = (Widget) sender;
					win.Toplevel.Destroy();
				}
			}
		}
		
		void on_button_actulizar_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
													MessageType.Question,ButtonsType.YesNo,"¿ Desea Actualizar esta infomacion ?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
			
			// Validando si el producto no vario de precio
			if(precio_unitario_anterior != precio_uni ){
				MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,
													MessageType.Question,ButtonsType.YesNo,"¿ Cambio el precio unitario, quieres actualizarlo para San Nicolas \n"+
																			"mantiniendo el precio publico Anterior ?");
				ResponseType misannicolas = (ResponseType)msgBox1.Run ();
				msgBox1.Destroy();
				if (misannicolas == ResponseType.Yes){
					this.entry_precios_costunitario_sannico.Text = precio_unitario_anterior.ToString(); 
				}
			} 
 			if (miResultado == ResponseType.Yes)
			{
				if (this.radiobutton_desc_si.Active == true){
					apldesc = "true";
				}
				if (this.radiobutton_desc_no.Active == true){
					apldesc = "false";
				}
					
				if(this.radiobutton_costounico_si.Active == true){
					costounico = "true";
					//Console.WriteLine(costounico.ToString());
				}
				if(this.radiobutton_costounico_no.Active == true){
					costounico = "false";
					//Console.WriteLine(costounico.ToString());
				}
								 
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
		        // Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
		 			comando.CommandText = "UPDATE osiris_productos "+
											"SET descripcion_producto = '"+this.entry_descripcion.Text.ToUpper()+"', "+
		 									"id_grupo_producto = '"+idtipogrupo+"', "+
		 									"id_grupo1_producto = '"+idtipogrupo1+"', "+
		 									"id_grupo2_producto = '"+idtipogrupo2+"', "+
		 									"precio_producto_publico = '"+float.Parse(this.entry_precio_publico.Text) +"', "+
		 									"costo_producto = '"+float.Parse(this.entry_costo.Text)+"', "+
		 									"costo_por_unidad = '"+float.Parse(this.entry_precio_unitario.Text)+"', "+
		 									"porcentage_ganancia = '"+float.Parse(this.entry_porciento_utilidad.Text)+"', "+
		 									"aplicar_iva = '"+(string) aplicariva_producto.ToString().ToLower().Trim()+"', "+
		 									"aplica_descuento = '"+apldesc+"', "+
		 									"cobro_activo = '"+(string) cobroactivo_producto.ToString().ToLower().Trim()+"', "+
		 									"nombre_articulo = '"+this.entry_nombre_articulo.Text.ToUpper()+"', "+
		 									"nombre_generico_articulo = '"+this.entry_nombre_generico.Text.ToUpper()+"', "+
		 									"cantidad_de_embalaje = '"+float.Parse(this.entry_embalaje.Text)+"', "+
		 									"porcentage_descuento = '"+float.Parse(this.entry_descuento.Text)+"', "+
		 									"costo_unico = '"+costounico+"',"+
		 									"precio_producto_publico1 = '"+float.Parse(this.entry_precios_costunitario_sannico.Text) +"', "+
		 									"precio_producto_5003 = '"+float.Parse(this.entry_precios_costunitario_sannico.Text) +"', "+
		 									
		 									"historial_de_cambios = historial_de_cambios || '"+LoginEmpleado+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+
		 															" PRECIO PUBLICO "+this.precio_unitario_anterior.ToString().Trim()+
		 															" PRECIO UNITARIO "+this.precio_costo_anterior.ToString().Trim()+
		 															" PORCENTAGE UTLD "+this.utilidad_anterior.ToString().Trim()+"\n' "+
		 									"WHERE  id_producto =  '"+long.Parse(entry_codigo_producto.Text)+"'; ";
		 									
		 					//Console.WriteLine(comando.CommandText.ToString());
		 					comando.ExecuteNonQuery();
		 					comando.Dispose();
				}catch (NpgsqlException ex){
							//Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
								msgBoxError.Destroy();
			    }
			    this.entry_codigo_producto.Text = "";
				this.entry_costo.Text= "0.00";
				this.entry_descripcion.Text = "";
				this.entry_descuento.Text ="0.00";
				this.entry_embalaje.Text = "0.00";
				this.entry_nombre_articulo.Text = "";
				this.entry_nombre_generico.Text = "";
				this.entry_porciento_utilidad.Text = "0.00";
				this.entry_precio_publico.Text = "0.00";
				this.entry_precio_unitario.Text ="0.00";
				this.entry_utilidad.Text ="0.00";
				this.entry_precios_costunitario_sannico.Text = "0.00";
				this.combobox_grupo.Clear();
				this.combobox_grupo1.Clear();
				this.combobox_grupo2.Clear();
				this.entry_tipo_grupo.Text ="";
				this.entry_tipo_grupo1.Text ="";
				this.entry_tipo_grupo2.Text ="";
				llenado_de_cmbox_grupos();
				this.button_actualizar.Sensitive = false;
				this.button_grabar.Sensitive = false;
			}
		}
		
		void on_button_limpiar_clicked (object sender, EventArgs args)
		{
			this.entry_codigo_producto.Text = "";
			this.entry_costo.Text= "0.00";
			this.entry_descripcion.Text = "";
			this.entry_descuento.Text ="0.00";
			this.entry_embalaje.Text = "0.00";
			this.entry_nombre_articulo.Text = "";
			this.entry_nombre_generico.Text = "";
			this.entry_porciento_utilidad.Text = "0.00";
			this.entry_precio_publico.Text = "0.00";
			this.entry_precio_unitario.Text ="0.00";
			this.entry_utilidad.Text ="0.00";
			this.entry_precios_costunitario_sannico.Text = "0.00";
			this.combobox_grupo.Clear();
			this.combobox_grupo1.Clear();
			this.combobox_grupo2.Clear();
			this.entry_tipo_grupo.Text ="";
			this.entry_tipo_grupo1.Text ="";
			this.entry_tipo_grupo2.Text ="";
			
			llenado_de_cmbox_grupos();
			this.button_grabar.Sensitive = false;
			this.button_actualizar.Sensitive = false;
		}
		
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((bool)lista_de_producto.Model.GetValue (iter,20)==true) { 
				if ((bool)lista_de_producto.Model.GetValue (iter,19)==true) { (cell as Gtk.CellRendererText).Foreground = "blue";
				}else{ (cell as Gtk.CellRendererText).Foreground = "black"; }
			}else{	(cell as Gtk.CellRendererText).Foreground = "red";  }
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_only_numbers(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(),Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
			{
				args.RetVal = true;
			}
		}
		
		// Activa en enter en la busqueda de los productos
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				llena_la_lista_de_productos();				
			}
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}