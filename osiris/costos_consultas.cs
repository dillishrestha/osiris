///////////////////////////////////////////////////////
// created on 17/10/2007 at 03:53 p
// Sistema Hospitalario OSIRIS
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
// Programa		: costos.cs
// Proposito	: Consultas de Costos
// Objeto		: 
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class costos_consultas
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		//[Widget] Gtk.Button button_buscar;
		[Widget] Gtk.Button button_imprimir;
		[Widget] Gtk.Entry entry_expresion;
		
		// Declarando ventana del menu de costos
		[Widget] Gtk.Window menu_costos;
		[Widget] Gtk.Button button_movtotal_producto;		
		[Widget] Gtk.Button button_nuevos_productos;
		[Widget] Gtk.Button button_costeo_procedimiento;
		[Widget] Gtk.Button button_listas_precios;
		[Widget] Gtk.Button button_catalogo_productos;
		[Widget] Gtk.Button button_productos_aplicados;
		[Widget] Gtk.Button button_farmacia;
		
		//Declarando Ventana de Reporte de Lista de Precios
		[Widget] Gtk.Window reporte_lista_de_precios;
		[Widget] Gtk.ComboBox combobox_tipo_paciente;
		//[Widget] Gtk.Entry entry_empresa_aseguradora;
		[Widget] Gtk.Button button_busca_empresas;
		[Widget] Gtk.Button button_buscar_emp;
		[Widget] Gtk.Entry entry_empresa_aseguradora;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.CheckButton checkbutton_grupo;
		[Widget] Gtk.CheckButton checkbutton_grupo1;
		[Widget] Gtk.CheckButton checkbutton_grupo2;
		[Widget] Gtk.CheckButton checkbutton_tarjeta;
		[Widget] Gtk.CheckButton checkbutton_especiales;  
		[Widget] Gtk.RadioButton radiobutton_desglosado;
		[Widget] Gtk.RadioButton radiobutton_con_iva;
		[Widget] Gtk.RadioButton radiobutton_sin_iva;
		
		// declaracion de treeview

		[Widget] Gtk.TreeView lista_empresas;
		[Widget] Gtk.TreeView lista_grupo;
		[Widget] Gtk.TreeView lista_grupo1;
		[Widget] Gtk.TreeView lista_grupo2;
		
		
		public string connectionString = "Server=localhost;" +
						"Port=5432;" +
						 "User ID=admin;" +
						"Password=1qaz2wsx;";
		public string nombrebd;
		public string LoginEmpleado;
    	public string NomEmpleado;
    	public string AppEmpleado;
    	public string ApmEmpleado;
    	
    	public string tipo_paciente = "";
    	public int id_tipopaciente = 0;  // toma el valor del tipo de paciente
    	public int id_empresa = 0;
    	public int id_aseguradora = 0;
    	public string idgrupoproducto;   
    	    
    	//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		//declaracion de treeview aseguradoras
		private TreeStore treeViewEngineaseguradoras;
		//private TreeStore treeViewEngineempresas;
		
		private ListStore treeViewEnginegrupos;
		private ListStore treeViewEnginegrupos1;
		private ListStore treeViewEnginegrupos2;
		//private ArrayList grupos;
		//private ArrayList arraycargosextras;
		
		public costos_consultas(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_ )
		{
			LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		nombrebd = _nombrebd_; 
    		
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "menu_costos", null);
			gxml.Autoconnect (this);
	        
			// Muestra ventana de Glade
			menu_costos.Show();
			
			// movimiento total de productos
			button_movtotal_producto.Clicked += new EventHandler(on_button_movtotal_producto_clicked);
			
			// Nuevos productos San Nicolas
			button_nuevos_productos.Clicked += new EventHandler(on_button_nuevos_productos_clicked);
			button_nuevos_productos.Hide();
			
			// Reporte de las Listas de Precios
			button_listas_precios.Clicked += new EventHandler(on_button_listas_precios_clicked);
			
			// Costeo de procedimiento
			button_costeo_procedimiento.Clicked += new EventHandler(on_button_costeo_procedimiento_clicked);
			
			// Productos aplicados a procemientos y pacientes por centro de costo
			button_productos_aplicados.Clicked += new EventHandler(on_button_productos_aplicados_clicked);
			
			// Catalogo de Productos
			button_catalogo_productos.Clicked += new EventHandler(on_button_catalogo_productos_clicked);
			
			//Reporte Compras De Farmacia
			button_farmacia.Clicked += new EventHandler(on_button_button_farmacia_clicked);
			
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);			
		}
		
		void on_button_movtotal_producto_clicked (object sender, EventArgs args)
		{
			new osiris.consulta_mensual_productos(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_catalogo_productos_clicked (object sender, EventArgs args)
		{
			new osiris.catalogo_productos_nuevos(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		void on_button_nuevos_productos_clicked(object sender, EventArgs args)
		{
			//new osiris.nuevos_prod(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_button_farmacia_clicked(object sender, EventArgs args)
		{
			//new osiris.rpt_compras_farmacia(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_costeo_procedimiento_clicked(object sender, EventArgs args)
		{
			new osiris.costeo_productos(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);  // costeo.cs
		}
		
		void on_button_productos_aplicados_clicked(object sender, EventArgs args)
		{
			new movimientos_productos_paciente(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,0);
		}
		
		void on_button_listas_precios_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "reporte_lista_de_precios", null);
			gxml.Autoconnect (this);
			reporte_lista_de_precios.Show();
			
			llenado_tipo_paciente();
			crea_treeview_grupo();
			crea_treeview_grupo1();
			crea_treeview_grupo2();
			
			checkbutton_grupo.Clicked += new EventHandler(on_checkbutton_llenando_todos_grupo);
			checkbutton_grupo1.Clicked += new EventHandler(on_checkbutton_llenando_todos_grupo1);			
			checkbutton_grupo2.Clicked += new EventHandler(on_checkbutton_llenando_todos_grupo2);
			checkbutton_tarjeta.Clicked += new EventHandler(on_checkbutton_tarjeta_clicked);
			checkbutton_especiales.Clicked += new EventHandler(on_checkbutton_especiales_clicked);
			button_buscar_emp.Clicked += new EventHandler(on_button_buscar_emp_clicked);
			button_imprimir.Clicked += new EventHandler(on_button_imprimir_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta la final de la classe
		}
		
		void on_checkbutton_tarjeta_clicked(object sender, EventArgs args)
		{
		
			if(checkbutton_tarjeta.Active == true){
				combobox_tipo_paciente.Sensitive = false;
				button_buscar_emp.Sensitive = false;
				entry_empresa_aseguradora.Text = "";			
				checkbutton_especiales.Sensitive = false;
				combobox_tipo_paciente.Clear();
				lista_grupo.Sensitive = false;
				lista_grupo1.Sensitive = false;
				lista_grupo2.Sensitive = false;
				checkbutton_grupo.Sensitive = false;
				checkbutton_grupo1.Sensitive = false;
				checkbutton_grupo2.Sensitive = false;
			}else{
				combobox_tipo_paciente.Sensitive = true;
				button_buscar_emp.Sensitive = true;
				checkbutton_especiales.Sensitive = true;
				lista_grupo.Sensitive = true;
				lista_grupo1.Sensitive = true;
				lista_grupo2.Sensitive = true;
				checkbutton_grupo.Sensitive = true;
				checkbutton_grupo1.Sensitive = true;
				checkbutton_grupo2.Sensitive = true;
				
				llenado_tipo_paciente();
			}
			llena_grupos();
			
		}
		
		void on_checkbutton_especiales_clicked(object sender, EventArgs args)
		{
			if(checkbutton_especiales.Active == true){							
				checkbutton_tarjeta.Sensitive = false;
				lista_grupo.Sensitive = false;
				lista_grupo1.Sensitive = false;
				lista_grupo2.Sensitive = false;
				checkbutton_grupo.Sensitive = false;
				checkbutton_grupo1.Sensitive = false;
				checkbutton_grupo2.Sensitive = false;
			}else{
				checkbutton_tarjeta.Sensitive = true;
				lista_grupo.Sensitive = true;
				lista_grupo1.Sensitive = true;
				lista_grupo2.Sensitive = true;
				checkbutton_grupo.Sensitive = true;
				checkbutton_grupo1.Sensitive = true;
				checkbutton_grupo2.Sensitive = true;
			}

		}
			
		/////////////////////// BOTON BUSCAR aseguradora//////////////////////////////////////////////////////////
		void on_button_buscar_emp_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "busca_empresas", null);
			gxml.Autoconnect (this);
	      	
	      	crea_treeview_aseguradoras();
			button_busca_empresas.Clicked += new EventHandler(on_llena_lista);
			button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
			button_selecciona.Clicked +=  new EventHandler(on_selecciona_emp);
		}
		
		void crea_treeview_aseguradoras()
		{
			{
				treeViewEngineaseguradoras = new TreeStore(typeof(int),//0
														typeof(string));//1
				
				lista_empresas.RulesHint = true;
				lista_empresas.Model = treeViewEngineaseguradoras;
				lista_empresas.RowActivated += on_selecciona_emp;
								
				TreeViewColumn col_id_aseg = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_id_aseg.Title = "No. Identificacion"; // titulo de la cabecera de la columna, si está visible
				col_id_aseg.PackStart(cellr0, true);
				col_id_aseg.AddAttribute (cellr0, "text", 0);
				col_id_aseg.SortColumnId = (int) Col_aseg.col_id_aseg;
				
				TreeViewColumn col_ase = new TreeViewColumn();
				CellRendererText cellrt1 = new CellRendererText();
				col_ase.Title = "descripcion";
				col_ase.PackStart(cellrt1, true);
				col_ase.AddAttribute (cellrt1, "text", 1); 
				col_ase.SortColumnId = (int) Col_aseg.col_ase;
							           
				lista_empresas.AppendColumn(col_id_aseg);
				lista_empresas.AppendColumn(col_ase);
			}
		}
		
		enum Col_aseg
		{
			col_id_aseg,
			col_ase
		}
		
		// llena lista de Empresas o Aseguradoras
		void on_llena_lista(object sender, EventArgs args)
		{
			if (id_tipopaciente == 400){
				llenando_lista_de_aseguradoras();
			}else{
				llenando_lista_de_empresas();
			}
		}

		void llenando_lista_de_aseguradoras()
		{
			treeViewEngineaseguradoras.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				if ((string) entry_expresion.Text.ToUpper() == "*" || (string) entry_expresion.Text.ToUpper() == "")
				{
					comando.CommandText = "SELECT id_aseguradora,descripcion_aseguradora "+
								"FROM osiris_aseguradoras "+
								"WHERE lista_de_precio = true "+
								"ORDER BY id_aseguradora;";
				}else{
					comando.CommandText = "SELECT id_aseguradora,descripcion_aseguradora "+
								"FROM osiris_aseguradoras "+
								"WHERE descripcion_aseguradora LIKE '%"+(string) entry_expresion.Text.ToUpper()+"%' "+
								"lista_de_precio = true "+
								"ORDER BY descripcion_aseguradora;";
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//bool verifica_activacion;
				while (lector.Read())
				{	
					treeViewEngineaseguradoras.AppendValues ((int) lector["id_aseguradora"],//0
													(string) lector["descripcion_aseguradora"]);//1
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void llenando_lista_de_empresas()
		{
			
			treeViewEngineaseguradoras.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				if ((string) entry_expresion.Text.ToUpper() == "*" || (string) entry_expresion.Text.ToUpper() == "")
				{
					comando.CommandText = "SELECT id_empresa,descripcion_empresa "+
								"FROM osiris_empresas "+
								"WHERE lista_de_precio = true "+								
								"ORDER BY id_empresa;";
				}else{
					comando.CommandText = "SELECT  id_empresa,descripcion_empresa "+
								"FROM osiris_empresas "+
								"WHERE descripcion_empresa LIKE '%"+(string) entry_expresion.Text.ToUpper()+"%' "+
								"lista_de_precio = true "+
								"ORDER BY descripcion_empresa;";
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//bool verifica_activacion;
				while (lector.Read())
				{	
					treeViewEngineaseguradoras.AppendValues ((int) lector["id_empresa"],//0
													(string) lector["descripcion_empresa"]);//1
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_selecciona_emp(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_empresas.Selection.GetSelected(out model, out iterSelected)){			
 				if (id_tipopaciente == 400){
 					this.id_aseguradora = (int) model.GetValue(iterSelected, 0);	
 					this.id_aseguradora = 0;	
 				}else{	
 					this.id_empresa = 0; 	
 					this.id_empresa = (int) model.GetValue(iterSelected, 0);
 			}
 				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
				entry_empresa_aseguradora.Text = (string) model.GetValue(iterSelected, 1);
				llena_grupos();
			}
		}
		
		/// CREANDO TREEVIEW DE GRUPOS
		
		void crea_treeview_grupo()
		{
			treeViewEnginegrupos = new ListStore(typeof(bool), 
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));
												
			lista_grupo.Model = treeViewEnginegrupos;
			
			lista_grupo.RulesHint = true;
				
			TreeViewColumn col_seleccion = new TreeViewColumn();
			CellRendererToggle cellr0 = new CellRendererToggle();
			col_seleccion.Title = "Seleccion"; // titulo de la cabecera de la columna, si está visible
			col_seleccion.PackStart(cellr0, true);
			//col_seleccion.SetCellDataFunc(cellr0, new TreeCellDataFunc (BoolCellDataFunc));  // funcion de columna
			col_seleccion.AddAttribute (cellr0, "active", 0);
			cellr0.Activatable = true;
			cellr0.Toggled += selecciona_fila_grupo; 
		
			TreeViewColumn col_producto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_producto.Title = "descripcion"; // titulo de la cabecera de la columna, si está visible
			col_producto.PackStart(cellr1, true);
			col_producto.AddAttribute (cellr1, "text", 1);
			//cellr1.Editable = true;   // Permite edita este campo
			//cellr1.Edited += new EditedHandler (NumberCellEdited);
			cellr1.Foreground = "darkblue";
			
			lista_grupo.AppendColumn(col_seleccion);
			lista_grupo.AppendColumn(col_producto);
		}
	
		void llena_grupos()
		{
			llenando_lista_grupo();
			llenando_lista_grupo1();
			llenando_lista_grupo2();
		}

		void llenando_lista_grupo()
		{
			treeViewEnginegrupos.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{	
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(id_grupo_producto,'999999999') AS idgrupoproducto,descripcion_grupo_producto, "+
								"agrupacion,agrupacion2,agrupacion3,agrupacion4 "+
								"FROM osiris_grupo_producto "+
								"WHERE id_grupo_producto > 0 "+
								"ORDER BY id_grupo_producto;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//bool verifica_activacion;
				while (lector.Read()){	
					treeViewEnginegrupos.AppendValues (false,
													(string) lector["descripcion_grupo_producto"],//0
													(string) lector["idgrupoproducto"],//1
													(string) lector["agrupacion"],//2
													(string) lector["agrupacion2"],//4
													(string) lector["agrupacion3"],//5
													(string) lector["agrupacion4"]);//6
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		 //DECLARACION TREEVIEW GRUPO 1 y 2
		void crea_treeview_grupo1()
		{
			treeViewEnginegrupos1 = new ListStore(typeof(bool), 
													typeof(string),
													typeof(string));
												
			lista_grupo1.Model = treeViewEnginegrupos1;
			lista_grupo1.RulesHint = true;
				
			TreeViewColumn col_seleccion = new TreeViewColumn();
			CellRendererToggle cellr0 = new CellRendererToggle();
			col_seleccion.Title = "Seleccion"; // titulo de la cabecera de la columna, si está visible
			col_seleccion.PackStart(cellr0, true);
			//col_seleccion.SetCellDataFunc(cellr0, new TreeCellDataFunc (BoolCellDataFunc));  // funcion de columna
			col_seleccion.AddAttribute (cellr0, "active", 0);
			cellr0.Activatable = true;
			cellr0.Toggled += selecciona_fila_grupo1; 
		
			TreeViewColumn col_producto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_producto.Title = "descripcion"; // titulo de la cabecera de la columna, si está visible
			col_producto.PackStart(cellr1, true);
			col_producto.AddAttribute (cellr1, "text", 1);
			//cellr1.Editable = true;   // Permite edita este campo
			//cellr1.Edited += new EditedHandler (NumberCellEdited);
			cellr1.Foreground = "darkblue";
			
			lista_grupo1.AppendColumn(col_seleccion);
			lista_grupo1.AppendColumn(col_producto);
		}
		
		void llenando_lista_grupo1()
		{
			treeViewEnginegrupos1.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();

				comando.CommandText = "SELECT to_char(id_grupo1_producto,'999999999') AS idgrupo1producto,descripcion_grupo1_producto "+
								"FROM osiris_grupo1_producto "+
								"WHERE id_grupo1_producto > 0 "+
								"ORDER BY id_grupo1_producto;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//bool verifica_activacion;
				while (lector.Read())
				{	
					treeViewEnginegrupos1.AppendValues (false,
													(string) lector["descripcion_grupo1_producto"],//0
													(string) lector["idgrupo1producto"]);//1
													
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
			if (lista_grupo.Model.GetIter (out iter, path)) {
				bool old = (bool) lista_grupo.Model.GetValue (iter,0);
				lista_grupo.Model.SetValue(iter,0,!old);
			}
		}
		
		void selecciona_fila_grupo1(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (lista_grupo1.Model.GetIter (out iter, path)) {
				bool old = (bool) lista_grupo1.Model.GetValue (iter,0);
				lista_grupo1.Model.SetValue(iter,0,!old);
			}
		}
	    void selecciona_fila_grupo2(object sender, ToggledArgs args)
			{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (lista_grupo2.Model.GetIter (out iter, path)) {
				bool old = (bool) lista_grupo2.Model.GetValue (iter,0);
				lista_grupo2.Model.SetValue(iter,0,!old);
			}
		}
		
		void crea_treeview_grupo2()
		{
			treeViewEnginegrupos2 = new ListStore(typeof(bool), 
													typeof(string),
													typeof(string));
												
			lista_grupo2.Model = treeViewEnginegrupos2;
			lista_grupo2.RulesHint = true;
				
			TreeViewColumn col_seleccion = new TreeViewColumn();
			CellRendererToggle cellr0 = new CellRendererToggle();
			col_seleccion.Title = "Seleccion"; // titulo de la cabecera de la columna, si está visible
			col_seleccion.PackStart(cellr0, true);
			//col_seleccion.SetCellDataFunc(cellr0, new TreeCellDataFunc (BoolCellDataFunc));  // funcion de columna
			col_seleccion.AddAttribute (cellr0, "active", 0);
			cellr0.Activatable = true;
			cellr0.Toggled += selecciona_fila_grupo2; 
		
			TreeViewColumn col_producto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_producto.Title = "descripcion"; // titulo de la cabecera de la columna, si está visible
			col_producto.PackStart(cellr1, true);
			col_producto.AddAttribute (cellr1, "text", 1);
			//cellr1.Editable = true;   // Permite edita este campo
			//cellr1.Edited += new EditedHandler (NumberCellEdited);
			cellr1.Foreground = "darkblue";
			
			lista_grupo2.AppendColumn(col_seleccion);
			lista_grupo2.AppendColumn(col_producto);
		}
		
		void llenando_lista_grupo2()
		{
			treeViewEnginegrupos2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();

				comando.CommandText = "SELECT to_char(id_grupo2_producto,'999999999') AS idgrupo2producto,descripcion_grupo2_producto "+
									"FROM osiris_grupo2_producto "+
									"WHERE id_grupo2_producto > 0 "+
									"ORDER BY id_grupo2_producto;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//bool verifica_activacion;
				while (lector.Read())
				{	
					treeViewEnginegrupos2.AppendValues (false,
													(string) lector["descripcion_grupo2_producto"],//0
													(string) lector["idgrupo2producto"]);//1
													
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}		
		
		void on_checkbutton_llenando_todos_grupo(object sender, EventArgs args)
		{
			if ((bool)checkbutton_grupo.Active == true){
				TreeIter iter2;
				if (this.treeViewEnginegrupos.GetIterFirst (out iter2)){
					lista_grupo.Model.SetValue(iter2,0,true);
					while (this.treeViewEnginegrupos.IterNext(ref iter2)){
						lista_grupo.Model.SetValue(iter2,0,true);
					}
				}
			}else{
				TreeIter iter2;
				if (this.treeViewEnginegrupos.GetIterFirst (out iter2)){
					lista_grupo.Model.SetValue(iter2,0,false);
					while (this.treeViewEnginegrupos.IterNext(ref iter2)){
						lista_grupo.Model.SetValue(iter2,0,false);
					}
				}
			}
		}

		void on_checkbutton_llenando_todos_grupo1(object sender, EventArgs args)
		{
			if ((bool)checkbutton_grupo1.Active == true){
				TreeIter iter2;
				if (this.treeViewEnginegrupos1.GetIterFirst (out iter2)){
					lista_grupo1.Model.SetValue(iter2,0,true);
					while (this.treeViewEnginegrupos1.IterNext(ref iter2)){
						lista_grupo1.Model.SetValue(iter2,0,true);
					}
				}
			}else{
				TreeIter iter2;
				if (this.treeViewEnginegrupos1.GetIterFirst (out iter2)){
					lista_grupo1.Model.SetValue(iter2,0,false);
					while (this.treeViewEnginegrupos1.IterNext(ref iter2)){
						lista_grupo1.Model.SetValue(iter2,0,false);
					}
				}
			}
		}

		void on_checkbutton_llenando_todos_grupo2(object sender, EventArgs args)
		{
			if ((bool)checkbutton_grupo2.Active == true){
				TreeIter iter2;
				if (this.treeViewEnginegrupos2.GetIterFirst (out iter2)){
					lista_grupo2.Model.SetValue(iter2,0,true);
					while (this.treeViewEnginegrupos2.IterNext(ref iter2)){
						lista_grupo2.Model.SetValue(iter2,0,true);
					}
				}
			}else{
				TreeIter iter2;
				if (this.treeViewEnginegrupos2.GetIterFirst (out iter2)){
					lista_grupo2.Model.SetValue(iter2,0,false);
					while (this.treeViewEnginegrupos2.IterNext(ref iter2)){
						lista_grupo2.Model.SetValue(iter2,0,false);
					}
				}
			}
		}
		///////////////////////////// COMBOBOX TIPO PACIENTE /////////////////////////////////////////////
		void llenado_tipo_paciente()
		{
			combobox_tipo_paciente.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_tipo_paciente.PackStart(cell3, true);
			combobox_tipo_paciente.AddAttribute(cell3,"text",0);
	        
			ListStore store3 = new ListStore( typeof (string), typeof (int));
			combobox_tipo_paciente.Model = store3;
			
			store3.AppendValues ((string) "",(int) 0);
			
	        NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
            //Console.WriteLine("si busca");
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT descripcion_tipo_paciente, id_tipo_paciente FROM osiris_his_tipo_pacientes "+
               						"WHERE lista_de_precio = true "+
               						"ORDER BY descripcion_tipo_paciente;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read())
				{
					store3.AppendValues ((string) lector["descripcion_tipo_paciente"],(int) lector["id_tipo_paciente"]);
					
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
	        
			TreeIter iter3;
			if (store3.GetIterFirst(out iter3))	{ combobox_tipo_paciente.SetActiveIter (iter3); 
			}
			combobox_tipo_paciente.Changed += new EventHandler (onComboBoxChanged_tipo_paciente);
		}
		void onComboBoxChanged_tipo_paciente (object sender, EventArgs args)
		{
			ComboBox combobox_tipo_paciente = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_tipo_paciente.GetActiveIter (out iter)){
				tipo_paciente = (string) combobox_tipo_paciente.Model.GetValue(iter,0);
				id_tipopaciente = (int) combobox_tipo_paciente.Model.GetValue(iter,1);				
			}
		}
		
		void on_button_imprimir_clicked(object sender, EventArgs args)
		{
			/*
			new osiris.lista_de_precios(this.nombrebd, this.treeViewEnginegrupos, this.treeViewEnginegrupos1, this.treeViewEnginegrupos2,
										lista_grupo, lista_grupo1, lista_grupo2,
										this.checkbutton_especiales.Active,this.checkbutton_tarjeta.Active,
										id_tipopaciente,id_empresa,id_aseguradora,radiobutton_desglosado.Active,radiobutton_con_iva.Active,
										radiobutton_sin_iva.Active,entry_empresa_aseguradora);
			*/
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
			
	//////////////////////////////////////////////////////////////////////////////////////////////////
	// CLASE CONSULTA MENSUAL DE PRODUCTOS	
	//////////////////////////////////////////////////////////////////////////////////////////////////
	public class consulta_mensual_productos
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		//[Widget] Gtk.Button button_imprimir_busqueda;
		
		// Declarando ventana de consulta de producto
		[Widget] Gtk.Window total_productos_aplicados;
		[Widget] Gtk.Entry entry_descrip_producto;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.ComboBox combobox_anos;
		[Widget] Gtk.TreeView lista_producto_seleccionados;
		[Widget] Gtk.TreeView lista_resumen_productos;
		
		[Widget] Gtk.CheckButton checkbutton_ene_costos;
		[Widget] Gtk.CheckButton checkbutton_feb_costos;
		[Widget] Gtk.CheckButton checkbutton_mar_costos;
		[Widget] Gtk.CheckButton checkbutton_abr_costos;
		[Widget] Gtk.CheckButton checkbutton_may_costos;
		[Widget] Gtk.CheckButton checkbutton_jun_costos;
		[Widget] Gtk.CheckButton checkbutton_jul_costos;
		[Widget] Gtk.CheckButton checkbutton_ago_costos;
		[Widget] Gtk.CheckButton checkbutton_sep_costos;
		[Widget] Gtk.CheckButton checkbutton_oct_costos;
		[Widget] Gtk.CheckButton checkbutton_nov_costos;
		[Widget] Gtk.CheckButton checkbutton_dic_costos;
		[Widget] Gtk.CheckButton checkbutton_todo_ano;
		
		[Widget] Gtk.Button button_consultar_costos;
		[Widget] Gtk.Button button_quitar_producto;
		[Widget] Gtk.Button button_limpiar;
		[Widget] Gtk.Button button_imprimir_costos;
		
		/*checkbutton_ene_costos;
		checkbutton_feb_costos;
		checkbutton_mar_costos;
		checkbutton_abr_costos;
		checkbutton_may_costos;
		checkbutton_jun_costos;
		checkbutton_jul_costos;
		checkbutton_ago_costos;
		checkbutton_sep_costos;
		checkbutton_oct_costos;
		checkbutton_nov_costos;
		checkbutton_dic_costos;*/		
		
		/////// Ventana Busqueda de productos\\\\\\\\
		[Widget] Gtk.Window busca_producto;
		[Widget] Gtk.TreeView lista_de_producto;
				
		public string busqueda = "";
		
		public string connectionString = "Server=localhost;" +
						"Port=5432;" +
						 "User ID=admin;" +
						"Password=1qaz2wsx;";
						
		public string nombrebd;
		public string LoginEmpleado;
    	public string NomEmpleado;
    	public string AppEmpleado;
    	public string ApmEmpleado;
    	public string ano_seleccionado = "";
    	
    	private TreeStore treeViewEngineBusca2;		// Para la busqueda de Productos
    	private TreeStore treeViewEngineProdSelec;	// Lista de Productos seleccionados
    	private TreeStore treeViewEngineResumen;	// Lista de Productos seleccionados
    	
    	//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
    	
		public consulta_mensual_productos(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_ )
		{
			LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		nombrebd = _nombrebd_; 
    		
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "total_productos_aplicados", null);
			gxml.Autoconnect (this);
	        
	        // Creacion de los treeview en pantalla
	        crea_treeview_prodselec();
	        crea_treeview_resumen_mensual();
	        
			// Muestra ventana de Glade
			total_productos_aplicados.Show();
			
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			// Busca el producto
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			button_consultar_costos.Clicked += new EventHandler(on_button_consultar_costos_clicked);			
			button_quitar_producto.Clicked += new EventHandler(on_button_quitar_producto_clicked);
			checkbutton_todo_ano.Clicked += new EventHandler(on_checkbutton_todo_ano_clicked);
			button_limpiar.Clicked += new EventHandler(on_button_limpiar_clicked);
			button_imprimir_costos.Clicked += new EventHandler(on_button_imprimir_costos_clicked);
			
			combobox_anos.Clear();
			CellRendererText cell2 = new CellRendererText();
			combobox_anos.PackStart(cell2, true);
			combobox_anos.AddAttribute(cell2,"text",0);
	        
			ListStore store2 = new ListStore( typeof (string));
			combobox_anos.Model = store2;
				        
			// lleno de la tabla de his_tipo_de_admisiones
			store2.AppendValues (" ");
			store2.AppendValues ("2007");
			store2.AppendValues ("2008");
			store2.AppendValues ("2009");
			store2.AppendValues ("2010");
			store2.AppendValues ("2011");
			store2.AppendValues ("2012");
			store2.AppendValues ("2013");
			store2.AppendValues ("2014");
			store2.AppendValues ("2015");
			store2.AppendValues ("2016");
			store2.AppendValues ("2017");
			store2.AppendValues ("2018");
			store2.AppendValues ("2019");
			store2.AppendValues ("2020");
			
			TreeIter iter2;
			if (store2.GetIterFirst(out iter2))
			{
				//Console.WriteLine(iter2);
				combobox_anos.SetActiveIter (iter2);
			}
			combobox_anos.Changed += new EventHandler (onComboBoxChanged_anos);
		}
		
		void onComboBoxChanged_anos(object sender, EventArgs args)
		{
    		ComboBox combobox_anos = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_anos.GetActiveIter (out iter)){
		    		ano_seleccionado = (string) combobox_anos.Model.GetValue(iter,0);
	     	}
		}
		
		void on_button_busca_producto_clicked (object sender, EventArgs args)
		{
			busqueda = "productos";
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			busca_producto.Show();
			crea_treeview_busqueda("producto");
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_busqueda;
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
						
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta la final de la classe
		}
		
		void on_checkbutton_todo_ano_clicked (object sender, EventArgs args)
		{
			checkbutton_ene_costos.Active = true;
			checkbutton_feb_costos.Active = true;
			checkbutton_mar_costos.Active = true;
			checkbutton_abr_costos.Active = true;
			checkbutton_may_costos.Active = true;
			checkbutton_jun_costos.Active = true;
			checkbutton_jul_costos.Active = true;
			checkbutton_ago_costos.Active = true;
			checkbutton_sep_costos.Active = true;
			checkbutton_oct_costos.Active = true;
			checkbutton_nov_costos.Active = true;
			checkbutton_dic_costos.Active = true;
		}
		
		void on_button_consultar_costos_clicked (object sender, EventArgs args)
		{
			int contador_de_meses = 0;
			string meses_selccionados = "";
			string productos_seleccionado = "";
			string var_paso = "";
			// Validadndo que tenga algun producto seleccionado en la lista
			treeViewEngineResumen.Clear();
			TreeIter iter;
			if (treeViewEngineProdSelec.GetIterFirst (out iter)){
				// Llenando string de productos
				var_paso = (string) lista_producto_seleccionados.Model.GetValue (iter,0);
				productos_seleccionado = var_paso.Trim();
 				while (treeViewEngineProdSelec.IterNext(ref iter)){
 					var_paso = (string) lista_producto_seleccionados.Model.GetValue (iter,0);
 					productos_seleccionado += "','"+var_paso.Trim();
 				}
 				// Verificando checkbuttons de meses
				if (this.checkbutton_ene_costos.Active == true){
					contador_de_meses += 1;
					meses_selccionados += "'"+ano_seleccionado+"-01',";
				}
				if (this.checkbutton_feb_costos.Active == true){
					contador_de_meses += 1;
					meses_selccionados += "'"+ano_seleccionado+"-02',";
				}
				if (this.checkbutton_mar_costos.Active == true){
					contador_de_meses += 1;
					meses_selccionados += "'"+ano_seleccionado+"-03',";
				}
				if (this.checkbutton_abr_costos.Active == true){
					contador_de_meses += 1;
					meses_selccionados += "'"+ano_seleccionado+"-04',";
				}
				if (this.checkbutton_may_costos.Active == true){
					contador_de_meses += 1;
					meses_selccionados += "'"+ano_seleccionado+"-05',";
				}
				if (this.checkbutton_jun_costos.Active == true){
					contador_de_meses += 1;
					meses_selccionados += "'"+ano_seleccionado+"-06',";
				}
				if (this.checkbutton_jul_costos.Active == true){
					contador_de_meses += 1;
					meses_selccionados += "'"+ano_seleccionado+"-07',";
				}
				if (this.checkbutton_ago_costos.Active == true){
					contador_de_meses += 1;
					meses_selccionados += "'"+ano_seleccionado+"-08',";
				}
				if (this.checkbutton_sep_costos.Active == true){
					contador_de_meses += 1;
					meses_selccionados += "'"+ano_seleccionado+"-09',";
				}
				if (this.checkbutton_oct_costos.Active == true){
					contador_de_meses += 1;
					meses_selccionados += "'"+ano_seleccionado+"-10',";
				}
				if (this.checkbutton_nov_costos.Active == true){
					contador_de_meses += 1;
					meses_selccionados += "'"+ano_seleccionado+"-11',";
				}
				if (this.checkbutton_dic_costos.Active == true){
					contador_de_meses += 1;
					meses_selccionados += "'"+ano_seleccionado+"-12',";
				}
				if (contador_de_meses > 0){
					meses_selccionados = meses_selccionados.Substring(0,meses_selccionados.Length-1);
 					if ((string) ano_seleccionado != ""){
 						NpgsqlConnection conexion; 
						conexion = new NpgsqlConnection (connectionString+nombrebd);
			            
						// Verifica que la base de datos este conectada
						try{
							conexion.Open ();
							NpgsqlCommand comando; 
							comando = conexion.CreateCommand ();
			               	
							comando.CommandText = "SELECT to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM') AS mes_actual,"+
											//"to_char(osiris_erp_cobros_deta.precio_costo_unitario,'9999999999.99') AS preciocostounitario,"+
											"to_char(sum(osiris_erp_cobros_deta.cantidad_aplicada),'9999999999.99') AS totaldeproductos,"+
											"to_char(sum(osiris_erp_cobros_deta.precio_producto * osiris_erp_cobros_deta.cantidad_aplicada),'9999999999.99') AS totalpreciopublico,"+
											"to_char(sum(osiris_erp_cobros_deta.precio_costo_unitario * osiris_erp_cobros_deta.cantidad_aplicada),'9999999999.99') AS totalpreciocosto,"+
											"to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
											"osiris_productos.descripcion_producto,descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,osiris_grupo_producto.agrupacion "+
											"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto,osiris_erp_cobros_deta,osiris_erp_cobros_enca "+  
											"WHERE osiris_productos.id_producto IN('"+productos_seleccionado+"')"+
											"AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM') IN("+meses_selccionados+") "+
											"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
											"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+ 
											"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+ 
											"AND osiris_erp_cobros_deta.id_producto =  osiris_productos.id_producto "+
											"AND osiris_erp_cobros_deta.folio_de_servicio =  osiris_erp_cobros_enca.folio_de_servicio "+
											"AND osiris_erp_cobros_deta.eliminado = false "+ 
											"AND osiris_erp_cobros_deta.cantidad_aplicada > '0' "+
											"GROUP BY mes_actual,osiris_productos.id_producto,osiris_productos.descripcion_producto,"+
											//"osiris_erp_cobros_deta.precio_costo_unitario,"+
											"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,osiris_grupo_producto.agrupacion "+
											"ORDER BY osiris_productos.id_producto,to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM') DESC;";
							NpgsqlDataReader lector = comando.ExecuteReader ();
							//Console.WriteLine(comando.CommandText.ToString());
							float var_01_ene = 0; float var_02_ene = 0;	string var_03_ene = "0";							
							float var_01_feb = 0; float var_02_feb = 0; string var_03_feb = "0";							
							float var_01_mar = 0; float var_02_mar = 0; string var_03_mar = "0";
							float var_01_abr = 0; float var_02_abr = 0; string var_03_abr = "0";
							float var_01_may = 0; float var_02_may = 0; string var_03_may = "0";
							float var_01_jun = 0; float var_02_jun = 0; string var_03_jun = "0";
							float var_01_jul = 0; float var_02_jul = 0; string var_03_jul = "0";
							float var_01_ago = 0; float var_02_ago = 0; string var_03_ago = "0";
							float var_01_sep = 0; float var_02_sep = 0; string var_03_sep = "0";
							float var_01_oct = 0; float var_02_oct = 0; string var_03_oct = "0";
							float var_01_nov = 0; float var_02_nov = 0; string var_03_nov = "0";
							float var_01_dic = 0; float var_02_dic = 0; string var_03_dic = "0";
							
							string var_paso_04 = "";
							
							string descrip_producto = "";
							string descrip_grupo = "";
							string descrip_grupo1 = "";
							string descrip_grupo2 = "";
							float cuenta_meses_activados = 0;
							
							if (lector.Read()){
								var_paso_04 = (string) lector["codProducto"];
								if (this.checkbutton_ene_costos.Active == true){
									cuenta_meses_activados += 1;
								}
								if (this.checkbutton_feb_costos.Active == true){
									cuenta_meses_activados += 1;
								}
								if (this.checkbutton_mar_costos.Active == true){
									cuenta_meses_activados += 1;
								}
								if (this.checkbutton_abr_costos.Active == true){
									cuenta_meses_activados += 1;
								}
								if (this.checkbutton_may_costos.Active == true){
									cuenta_meses_activados += 1;
								}
								if (this.checkbutton_jun_costos.Active == true){
									cuenta_meses_activados += 1;
								}
								if (this.checkbutton_jul_costos.Active == true){
									cuenta_meses_activados += 1;
								}
								if (this.checkbutton_ago_costos.Active == true ){
									cuenta_meses_activados += 1;
								}
								if (this.checkbutton_sep_costos.Active == true ){
									cuenta_meses_activados += 1;
								}
								if (this.checkbutton_oct_costos.Active == true){									
									cuenta_meses_activados += 1;
								}
								if (this.checkbutton_nov_costos.Active == true){
									cuenta_meses_activados += 1;
								}
								if (this.checkbutton_dic_costos.Active == true){
									cuenta_meses_activados += 1;
								}
								if (this.checkbutton_ene_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-01"){
									var_01_ene = float.Parse((string) lector["totalpreciopublico"]);
									var_02_ene = float.Parse((string) lector["totalpreciocosto"]);
									var_03_ene = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_feb_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-02"){
									var_01_feb = float.Parse((string) lector["totalpreciopublico"]);
									var_02_feb = float.Parse((string) lector["totalpreciocosto"]);
									var_03_feb = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_mar_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-03"){
									var_01_mar = float.Parse((string) lector["totalpreciopublico"]);
									var_02_mar = float.Parse((string) lector["totalpreciocosto"]);
									var_03_mar = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_abr_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-04"){
									var_01_abr = float.Parse((string) lector["totalpreciopublico"]);
									var_02_abr = float.Parse((string) lector["totalpreciocosto"]);
									var_03_abr = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_may_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-05"){
									var_01_may = float.Parse((string) lector["totalpreciopublico"]);
									var_02_may = float.Parse((string) lector["totalpreciocosto"]);
									var_03_may = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_jun_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-06"){
									var_01_jun = float.Parse((string) lector["totalpreciopublico"]);
									var_02_jun = float.Parse((string) lector["totalpreciocosto"]);
									var_03_jun = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_jul_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-07"){
									var_01_jul = float.Parse((string) lector["totalpreciopublico"]);
									var_02_jul = float.Parse((string) lector["totalpreciocosto"]);
									var_03_jul = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_ago_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-08"){
									var_01_ago = float.Parse((string) lector["totalpreciopublico"]);
									var_02_ago = float.Parse((string) lector["totalpreciocosto"]);
									var_03_ago = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_sep_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-09"){
									var_01_sep = float.Parse((string) lector["totalpreciopublico"]);
									var_02_sep = float.Parse((string) lector["totalpreciocosto"]);
									var_03_sep = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_oct_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-10"){
									var_01_oct = float.Parse((string) lector["totalpreciopublico"]);
									var_02_oct = float.Parse((string) lector["totalpreciocosto"]);
									var_03_oct = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_nov_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-11"){
									var_01_nov = float.Parse((string) lector["totalpreciopublico"]);
									var_02_nov = float.Parse((string) lector["totalpreciocosto"]);
									var_03_nov = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_dic_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-12"){
									var_01_dic = float.Parse((string) lector["totalpreciopublico"]);
									var_02_dic = float.Parse((string) lector["totalpreciocosto"]);
									var_03_dic = (string) lector["totaldeproductos"];
								}
								descrip_producto = (string) lector["descripcion_producto"];
								descrip_grupo = (string) lector["descripcion_grupo_producto"];
								descrip_grupo1 = (string) lector["descripcion_grupo1_producto"];
								descrip_grupo2 = (string) lector["descripcion_grupo2_producto"];
							}
							
							float var_paso_01 = 0;
							float var_paso_02 = 0;
							string var_paso_03 = " ";
														
							while (lector.Read()){
								if (var_paso_04 != (string) lector["codProducto"]){
									var_paso_01 = var_01_ene+var_01_feb+var_01_mar+
												  var_01_abr+var_01_may+var_01_jun+
												  var_01_jul+var_01_ago+var_01_sep+
												  var_01_oct+var_01_nov+var_01_dic;

									treeViewEngineResumen.AppendValues(	var_paso_04,
																	descrip_producto,
																	" ",
																	var_03_ene,
																	var_03_feb,
																	var_03_mar,
																	var_03_abr,
																	var_03_may,
																	var_03_jun,
																	var_03_jul,
																	var_03_ago,
																	var_03_sep,
																	var_03_oct,
																	var_03_nov,
																	var_03_dic,
																	(string) Convert.ToString(float.Parse(var_03_ene)+float.Parse(var_03_feb)+float.Parse(var_03_mar)+
												  									float.Parse(var_03_abr)+float.Parse(var_03_may)+float.Parse(var_03_jun)+
												  									float.Parse(var_03_jul)+float.Parse(var_03_ago)+float.Parse(var_03_sep)+
												  									float.Parse(var_03_oct)+float.Parse(var_03_nov)+float.Parse(var_03_dic)),
																	var_paso_01.ToString("C"),
																	"",
																	(string) Convert.ToString((float.Parse(var_03_ene)+float.Parse(var_03_feb)+float.Parse(var_03_mar)+
												  									float.Parse(var_03_abr)+float.Parse(var_03_may)+float.Parse(var_03_jun)+
												  									float.Parse(var_03_jul)+float.Parse(var_03_ago)+float.Parse(var_03_sep)+
												  									float.Parse(var_03_oct)+float.Parse(var_03_nov)+float.Parse(var_03_dic))/cuenta_meses_activados),
																	descrip_grupo,
																	descrip_grupo1,
																	descrip_grupo2);
									var_paso_04 = (string) lector["codProducto"];
									descrip_producto = (string) lector["descripcion_producto"];
									descrip_grupo = (string) lector["descripcion_grupo_producto"];
									descrip_grupo1 = (string) lector["descripcion_grupo1_producto"];
									descrip_grupo2 = (string) lector["descripcion_grupo2_producto"];
									var_01_ene = 0; var_02_ene = 0; var_03_ene = "0";
									var_01_feb = 0; var_02_feb = 0; var_03_feb = "0";
									var_01_mar = 0; var_02_mar = 0; var_03_mar = "0";
									var_01_abr = 0; var_02_abr = 0; var_03_abr = "0";
									var_01_may = 0; var_02_may = 0; var_03_may = "0";
									var_01_jun = 0; var_02_jun = 0; var_03_jun = "0";
									var_01_jul = 0; var_02_jul = 0; var_03_jul = "0";
									var_01_ago = 0; var_02_ago = 0; var_03_ago = "0";
									var_01_sep = 0; var_02_sep = 0; var_03_sep = "0";
									var_01_oct = 0; var_02_oct = 0; var_03_oct = "0";
									var_01_nov = 0; var_02_nov = 0; var_03_nov = "0";
									var_01_dic = 0; var_02_dic = 0; var_03_dic = "0";
								}
								if (this.checkbutton_ene_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-01"){
									var_01_ene = float.Parse((string) lector["totalpreciopublico"]);
									var_02_ene = float.Parse((string) lector["totalpreciocosto"]);
									var_03_ene = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_feb_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-02"){
									var_01_feb = float.Parse((string) lector["totalpreciopublico"]);
									var_02_feb = float.Parse((string) lector["totalpreciocosto"]);
									var_03_feb = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_mar_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-03"){
									var_01_mar = float.Parse((string) lector["totalpreciopublico"]);
									var_02_mar = float.Parse((string) lector["totalpreciocosto"]);
									var_03_mar = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_abr_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-04"){
									var_01_abr = float.Parse((string) lector["totalpreciopublico"]);
									var_02_abr = float.Parse((string) lector["totalpreciocosto"]);
									var_03_abr = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_may_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-05"){
									var_01_may = float.Parse((string) lector["totalpreciopublico"]);
									var_02_may = float.Parse((string) lector["totalpreciocosto"]);
									var_03_may = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_jun_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-06"){
									var_01_jun = float.Parse((string) lector["totalpreciopublico"]);
									var_02_jun = float.Parse((string) lector["totalpreciocosto"]);
									var_03_jun = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_jul_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-07"){
									var_01_jul = float.Parse((string) lector["totalpreciopublico"]);
									var_02_jul = float.Parse((string) lector["totalpreciocosto"]);
									var_03_jul = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_ago_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-08"){
									var_01_ago = float.Parse((string) lector["totalpreciopublico"]);
									var_02_ago = float.Parse((string) lector["totalpreciocosto"]);
									var_03_ago = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_sep_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-09"){
									var_01_sep = float.Parse((string) lector["totalpreciopublico"]);
									var_02_sep = float.Parse((string) lector["totalpreciocosto"]);
									var_03_sep = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_oct_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-10"){
									var_01_oct = float.Parse((string) lector["totalpreciopublico"]);
									var_02_oct = float.Parse((string) lector["totalpreciocosto"]);
									var_03_oct = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_nov_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-11"){
									var_01_nov = float.Parse((string) lector["totalpreciopublico"]);
									var_02_nov = float.Parse((string) lector["totalpreciocosto"]);
									var_03_nov = (string) lector["totaldeproductos"];
								}
								if (this.checkbutton_dic_costos.Active == true && (string) lector["mes_actual"] == ano_seleccionado+"-12"){
									var_01_dic = float.Parse((string) lector["totalpreciopublico"]);
									var_02_dic = float.Parse((string) lector["totalpreciocosto"]);
									var_03_dic = (string) lector["totaldeproductos"];
								}
							}
							var_paso_01 = var_01_ene+var_01_feb+var_01_mar+
										 var_01_abr+var_01_may+var_01_jun+
										 var_01_jul+var_01_ago+var_01_sep+
										 var_01_oct+var_01_nov+var_01_dic;
							treeViewEngineResumen.AppendValues(	var_paso_04,
																	descrip_producto,
																	" ",
																	var_03_ene,
																	var_03_feb,
																	var_03_mar,
																	var_03_abr,
																	var_03_may,
																	var_03_jun,
																	var_03_jul,
																	var_03_ago,
																	var_03_sep,
																	var_03_oct,
																	var_03_nov,
																	var_03_dic,
																	(string) Convert.ToString(float.Parse(var_03_ene)+float.Parse(var_03_feb)+float.Parse(var_03_mar)+
												  									float.Parse(var_03_abr)+float.Parse(var_03_may)+float.Parse(var_03_jun)+
												  									float.Parse(var_03_jul)+float.Parse(var_03_ago)+float.Parse(var_03_sep)+
												  									float.Parse(var_03_oct)+float.Parse(var_03_nov)+float.Parse(var_03_dic)),
																	var_paso_01.ToString("C"),
																	"",
																	(string) Convert.ToString((float.Parse(var_03_ene)+float.Parse(var_03_feb)+float.Parse(var_03_mar)+
												  									float.Parse(var_03_abr)+float.Parse(var_03_may)+float.Parse(var_03_jun)+
												  									float.Parse(var_03_jul)+float.Parse(var_03_ago)+float.Parse(var_03_sep)+
												  									float.Parse(var_03_oct)+float.Parse(var_03_nov)+float.Parse(var_03_dic))/cuenta_meses_activados),
																	descrip_grupo,
																	descrip_grupo1,
																	descrip_grupo2);
						}catch (NpgsqlException ex){
				   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error, 
													ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();
							msgBoxError.Destroy();
						}
						conexion.Close ();
 					}else{
 						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error, 
									ButtonsType.Close, "Debe seleccionar el año que quiere consultar");
						msgBoxError.Run ();
						msgBoxError.Destroy();
 					} 				
 				}else{
 					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, 
								ButtonsType.Close, "Debe seleccionar al menos 1 mes");
				msgBoxError.Run ();
				msgBoxError.Destroy();
 				}
 			}else{
 				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, 
							ButtonsType.Close, "NO existen productos a consultar");
				msgBoxError.Run ();
				msgBoxError.Destroy();
 			}
		}
		
		void on_button_imprimir_costos_clicked(object sender, EventArgs args)
		{
			TreeIter iter;
			if (this.treeViewEngineResumen.GetIterFirst (out iter)){
				//new osiris.imprime_consumo_productos (this.lista_resumen_productos,this.treeViewEngineResumen,this.ano_seleccionado);
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, 
							ButtonsType.Close, "NO existen nada para imprimir");
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
							"AND osiris_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_producto; ";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				Console.WriteLine(comando.CommandText.ToString());
				
				float tomaprecio;
				float calculodeiva;
				float preciomasiva;
				float tomadescue;
				float preciocondesc;
				float valoriva = 15;
											
				while (lector.Read())
				{
					calculodeiva = 0;
					preciomasiva = 0;
					
					// Verificando que datos que sea del municipio de san nicolas para que cambie el precio convenido
					// precio_producto_publico1
					// id_tipopaciente = minicipio
					//idempresa_paciente = San Nicolas
					tomaprecio = float.Parse((string) lector["preciopublico"]);
										
					tomadescue = float.Parse((string) lector["porcentagesdesc"]);
					preciocondesc = tomaprecio;
					
					if ((bool) lector["aplicar_iva"]){
						calculodeiva = (tomaprecio * valoriva)/100;
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
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;

 			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)) 
 			{
 				//id_produ = (string) model.GetValue(iterSelected, 0);
				//desc_produ = (string) model.GetValue(iterSelected, 1);
				entry_descrip_producto.Text = (string) model.GetValue(iterSelected, 1);
				treeViewEngineProdSelec.AppendValues ((string) model.GetValue(iterSelected, 0),
												   (string) model.GetValue(iterSelected, 1));
			}
		}
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_busqueda(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				if(busqueda == "productos") { 
					llenando_lista_de_productos();
				}
			}
		}
		
		void on_button_quitar_producto_clicked (object o, EventArgs args)
		{
 			TreeIter iter;
 			TreeModel model;

 			if (lista_producto_seleccionados.Selection.GetSelected (out model, out iter)) {
 				treeViewEngineProdSelec.Remove (ref iter);
			}
		}
		
		void on_button_limpiar_clicked(object o, EventArgs args)
		{
			treeViewEngineProdSelec.Clear();
		}
		
		void crea_treeview_prodselec()
		{
			treeViewEngineProdSelec = new TreeStore(typeof(string), 
													typeof(string));
												
			lista_producto_seleccionados.Model = treeViewEngineProdSelec;
			
			lista_producto_seleccionados.RulesHint = true;
			
			TreeViewColumn col_codigo_prod = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_codigo_prod.Title = "Codigo Prod."; // titulo de la cabecera de la columna, si está visible
			col_codigo_prod.PackStart(cellr0, true);
			col_codigo_prod.AddAttribute (cellr0, "text", 0);

			TreeViewColumn col_descripcion = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_descripcion.Title = "Descripcion"; // titulo de la cabecera de la columna, si está visible
			col_descripcion.PackStart(cellr1, true);
			col_descripcion.AddAttribute (cellr1, "text", 1);
			
			lista_producto_seleccionados.AppendColumn(col_codigo_prod);
			lista_producto_seleccionados.AppendColumn(col_descripcion);
		}
					
		void crea_treeview_resumen_mensual()
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
												
			lista_resumen_productos.Model = treeViewEngineResumen;
			
			lista_resumen_productos.RulesHint = true;
			
			TreeViewColumn col_codigo_prod = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_codigo_prod.Title = "Codigo Prod."; // titulo de la cabecera de la columna, si está visible
			col_codigo_prod.PackStart(cellr0, true);
			col_codigo_prod.AddAttribute (cellr0, "text", 0);
			col_codigo_prod.SortColumnId = (int) Column_resumen.col_codigo_prod;
			
			TreeViewColumn col_descripcion = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_descripcion.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
			col_descripcion.PackStart(cellr1, true);
			col_descripcion.AddAttribute (cellr1, "text", 1);
			col_descripcion.SortColumnId = (int) Column_resumen.col_descripcion;
			
			TreeViewColumn col_costounitario = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_costounitario.Title = "Cos.Unitario"; // titulo de la cabecera de la columna, si está visible
			col_costounitario.PackStart(cellr2, true);
			col_costounitario.AddAttribute (cellr2, "text", 2);
			col_costounitario.SortColumnId = (int) Column_resumen.col_costounitario;

			TreeViewColumn col_enero = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_enero.Title = "ENE"; // titulo de la cabecera de la columna, si está visible
			col_enero.PackStart(cellr3, true);
			col_enero.AddAttribute (cellr3, "text", 3);
			col_enero.SortColumnId = (int) Column_resumen.col_enero;
			
			TreeViewColumn col_febrero = new TreeViewColumn();
			CellRendererText cellr4 = new CellRendererText();
			col_febrero.Title = "FEB"; // titulo de la cabecera de la columna, si está visible
			col_febrero.PackStart(cellr4, true);
			col_febrero.AddAttribute (cellr4, "text", 4);
			col_febrero.SortColumnId = (int) Column_resumen.col_febrero;
			
			TreeViewColumn col_marzo = new TreeViewColumn();
			CellRendererText cellr5 = new CellRendererText();
			col_marzo.Title = "MAR"; // titulo de la cabecera de la columna, si está visible
			col_marzo.PackStart(cellr5, true);
			col_marzo.AddAttribute (cellr5, "text", 5);
			col_marzo.SortColumnId = (int) Column_resumen.col_marzo;
			
			TreeViewColumn col_abril = new TreeViewColumn();
			CellRendererText cellr6 = new CellRendererText();
			col_abril.Title = "ABR"; // titulo de la cabecera de la columna, si está visible
			col_abril.PackStart(cellr6, true);
			col_abril.AddAttribute (cellr6, "text", 6);
			col_abril.SortColumnId = (int) Column_resumen.col_abril;
			
			TreeViewColumn col_mayo = new TreeViewColumn();
			CellRendererText cellr7 = new CellRendererText();
			col_mayo.Title = "MAY"; // titulo de la cabecera de la columna, si está visible
			col_mayo.PackStart(cellr7, true);
			col_mayo.AddAttribute (cellr7, "text", 7);
			col_mayo.SortColumnId = (int) Column_resumen.col_mayo;
			
			TreeViewColumn col_junio = new TreeViewColumn();
			CellRendererText cellr8 = new CellRendererText();
			col_junio.Title = "JUN"; // titulo de la cabecera de la columna, si está visible
			col_junio.PackStart(cellr8, true);
			col_junio.AddAttribute (cellr8, "text", 8);
			col_junio.SortColumnId = (int) Column_resumen.col_junio;
			
			TreeViewColumn col_julio = new TreeViewColumn();
			CellRendererText cellr9 = new CellRendererText();
			col_julio.Title = "JUL"; // titulo de la cabecera de la columna, si está visible
			col_julio.PackStart(cellr9, true);
			col_julio.AddAttribute (cellr9, "text", 9);
			col_julio.SortColumnId = (int) Column_resumen.col_julio;
			
			TreeViewColumn col_agosto = new TreeViewColumn();
			CellRendererText cellr10 = new CellRendererText();
			col_agosto.Title = "AGO"; // titulo de la cabecera de la columna, si está visible
			col_agosto.PackStart(cellr10, true);
			col_agosto.AddAttribute (cellr10, "text", 10);
			col_agosto.SortColumnId = (int) Column_resumen.col_agosto;
			
			TreeViewColumn col_septiembre = new TreeViewColumn();
			CellRendererText cellr11 = new CellRendererText();
			col_septiembre.Title = "SEP"; // titulo de la cabecera de la columna, si está visible
			col_septiembre.PackStart(cellr11, true);
			col_septiembre.AddAttribute (cellr11, "text", 11);
			col_septiembre.SortColumnId = (int) Column_resumen.col_septiembre;
			
			TreeViewColumn col_octubre = new TreeViewColumn();
			CellRendererText cellr12 = new CellRendererText();
			col_octubre.Title = "OCT"; // titulo de la cabecera de la columna, si está visible
			col_octubre.PackStart(cellr12, true);
			col_octubre.AddAttribute (cellr12, "text", 12);
			col_octubre.SortColumnId = (int) Column_resumen.col_octubre;
			
			TreeViewColumn col_noviembre = new TreeViewColumn();
			CellRendererText cellr13 = new CellRendererText();
			col_noviembre.Title = "NOV"; // titulo de la cabecera de la columna, si está visible
			col_noviembre.PackStart(cellr13, true);
			col_noviembre.AddAttribute (cellr13, "text", 13);
			col_noviembre.SortColumnId = (int) Column_resumen.col_noviembre;
			
			TreeViewColumn col_diciembre = new TreeViewColumn();
			CellRendererText cellr14 = new CellRendererText();
			col_diciembre.Title = "DIC"; // titulo de la cabecera de la columna, si está visible
			col_diciembre.PackStart(cellr14, true);
			col_diciembre.AddAttribute (cellr14, "text", 14);
			col_diciembre.SortColumnId = (int) Column_resumen.col_diciembre;
			
			TreeViewColumn col_total_cobrado = new TreeViewColumn();
			CellRendererText cellr15 = new CellRendererText();
			col_total_cobrado.Title = "Total Aplicado"; // titulo de la cabecera de la columna, si está visible
			col_total_cobrado.PackStart(cellr15, true);
			col_total_cobrado.AddAttribute (cellr15, "text", 15);
			col_total_cobrado.SortColumnId = (int) Column_resumen.col_total_cobrado;
			
			TreeViewColumn col_total_costo = new TreeViewColumn();
			CellRendererText cellr16 = new CellRendererText();
			col_total_costo.Title = "Total $ Venta"; // titulo de la cabecera de la columna, si está visible
			col_total_costo.PackStart(cellr16, true);
			col_total_costo.AddAttribute (cellr16, "text", 16);
			col_total_costo.SortColumnId = (int) Column_resumen.col_total_costo;
			
			TreeViewColumn col_total_venta = new TreeViewColumn();
			CellRendererText cellr17 = new CellRendererText();
			col_total_venta.Title = "Total $ Costo"; // titulo de la cabecera de la columna, si está visible
			col_total_venta.PackStart(cellr17, true);
			col_total_venta.AddAttribute (cellr17, "text", 17);
			col_total_venta.SortColumnId = (int) Column_resumen.col_total_venta;
			
			TreeViewColumn col_promedio_consumo = new TreeViewColumn();
			CellRendererText cellr18 = new CellRendererText();
			col_promedio_consumo.Title = "Promedio Consumo"; // titulo de la cabecera de la columna, si está visible
			col_promedio_consumo.PackStart(cellr18, true);
			col_promedio_consumo.AddAttribute (cellr18, "text", 18);
			col_promedio_consumo.SortColumnId = (int) Column_resumen.col_promedio_consumo;
									
			TreeViewColumn col_grupoprod = new TreeViewColumn();
			CellRendererText cellrt19 = new CellRendererText();
			col_grupoprod.Title = "Grupo Producto";
			col_grupoprod.PackStart(cellrt19, true);
			col_grupoprod.AddAttribute (cellrt19, "text", 19); // la siguiente columna será 7 en vez de 8
			col_grupoprod.SortColumnId = (int) Column_resumen.col_grupoprod;
       
			TreeViewColumn col_grupo1prod = new TreeViewColumn();
			CellRendererText cellrt20 = new CellRendererText();
			col_grupo1prod.Title = "Grupo1 Producto";
			col_grupo1prod.PackStart(cellrt20, true);
			col_grupo1prod.AddAttribute (cellrt20, "text", 20); // la siguiente columna será 8 en vez de 9
			col_grupo1prod.SortColumnId = (int) Column_resumen.col_grupo1prod;
                   
			TreeViewColumn col_grupo2prod = new TreeViewColumn();
			CellRendererText cellrt21 = new CellRendererText();
			col_grupo2prod.Title = "Grupo2 Producto";
			col_grupo2prod.PackStart(cellrt21, true);
			col_grupo2prod.AddAttribute (cellrt21, "text", 21); // la siguiente columna será 8 en vez de 9
			col_grupo2prod.SortColumnId = (int) Column_resumen.col_grupo2prod;
			
			lista_resumen_productos.AppendColumn(col_codigo_prod);
			lista_resumen_productos.AppendColumn(col_descripcion);
			lista_resumen_productos.AppendColumn(col_costounitario);
			lista_resumen_productos.AppendColumn(col_enero);
			lista_resumen_productos.AppendColumn(col_febrero);
			lista_resumen_productos.AppendColumn(col_marzo);
			lista_resumen_productos.AppendColumn(col_abril);
			lista_resumen_productos.AppendColumn(col_mayo);
			lista_resumen_productos.AppendColumn(col_junio);
			lista_resumen_productos.AppendColumn(col_julio);
			lista_resumen_productos.AppendColumn(col_agosto);
			lista_resumen_productos.AppendColumn(col_septiembre);
			lista_resumen_productos.AppendColumn(col_octubre);
			lista_resumen_productos.AppendColumn(col_noviembre);
			lista_resumen_productos.AppendColumn(col_diciembre);
			lista_resumen_productos.AppendColumn(col_total_cobrado);
			lista_resumen_productos.AppendColumn(col_total_venta);
			lista_resumen_productos.AppendColumn(col_total_costo);
			lista_resumen_productos.AppendColumn(col_promedio_consumo);
			lista_resumen_productos.AppendColumn(col_grupoprod);
			lista_resumen_productos.AppendColumn(col_grupo1prod);
			lista_resumen_productos.AppendColumn(col_grupo2prod);
		}
		
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
		
		enum Column_resumen
		{
			col_codigo_prod,
			col_descripcion,
			col_costounitario,
			col_enero,
			col_febrero,
			col_marzo,
			col_abril,
			col_mayo,
			col_junio,
			col_julio,
			col_agosto,
			col_septiembre,
			col_octubre,
			col_noviembre,
			col_diciembre,
			col_total_cobrado,
			col_total_venta,
			col_total_costo,
			col_promedio_consumo,
			col_grupoprod,
			col_grupo1prod,
			col_grupo2prod
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}	
}