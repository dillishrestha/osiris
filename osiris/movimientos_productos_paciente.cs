///////////////////////////////////////////////////////
// created on 21/03/2008 at 09:00 am
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Israel Peña Gonzalez (Programacion)
//                Ing. Daniel Olivares (Pre-Programacion y Ajustes)
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
	public class movimientos_productos_paciente
	{   
		
		// Boton general para salir de las ventanas
		[Widget] Gtk.Button button_salir;
		[Widget] Gtk.Button button_buscar_busqueda;
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		
		//Declaracion de ventana de busqueda de productos
		[Widget] Gtk.Window busca_producto;
		[Widget] Gtk.TreeView lista_de_producto;
		[Widget] Gtk.RadioButton radiobutton_nombre;
		[Widget] Gtk.RadioButton radiobutton_codigo;
				
		// Declarando ventana para ver productos aplicados
		[Widget] Gtk.Window mov_productos;
		[Widget] Gtk.Entry entry_descrip_producto;
		[Widget] Gtk.TreeView lista_producto_seleccionados;
		[Widget] Gtk.TreeView lista_resumen_productos;
		// Declarando ventana de consulta de producto  
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.Button button_consultar_costos;
		[Widget] Gtk.Button button_quitar_producto;
		[Widget] Gtk.Button button_limpiar;
		[Widget] Gtk.Button button_imprimir_movimiento;
		//Fechas:
	    [Widget] Gtk.Entry entry_dia1;                     
	    [Widget] Gtk.Entry entry_mes1;
	    [Widget] Gtk.Entry entry_ano1;
	    [Widget] Gtk.Entry entry_dia2;
	    [Widget] Gtk.Entry entry_mes2;
	    [Widget] Gtk.Entry entry_ano2;
		// Combobox:
		[Widget] Gtk.ComboBox combobox_departamentos;
		// checkbutton:
		[Widget]Gtk.CheckButton checkbutton_todos_departamentos;
		[Widget]Gtk.CheckButton checkbutton_todos_productos;
		//entry:
		[Widget]Gtk.Entry entry_total_aplicado; 

		string busqueda = "";
	    
		string connectionString;						
		string nombrebd;
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
    	    	
    	string query_departamento = "AND osiris_his_tipo_admisiones.descripcion_admisiones = '0' ";
    	int id_tipo_admisiones = 0; 
		string query1 = "" ;
		string titulopagina= "MOVIMIENTOS DE PRODUCOS";
		
        TreeStore treeViewEngineBusca2;	// Para la busqueda de Productos
    	TreeStore treeViewEngineSelec;	// Lista de Productos seleccionados
    	TreeStore treeViewEngineResumen;	// Lista de Productos seleccionados
    	
    	//Declaracion de ventana de error:
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
	    class_public classpublic = new class_public();
		public movimientos_productos_paciente(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_,int centrocosto)
		{
	    	LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;	
    		
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "mov_productos", null);
			gxml.Autoconnect (this);
	        
			// Muestra ventana de Glade:
			mov_productos.Show();
			
		    crea_treeview_selec();
	        crea_treeview_resumen();
			
			entry_dia1.Text = DateTime.Now.ToString("dd");
			entry_mes1.Text = DateTime.Now.ToString("MM");
			entry_ano1.Text = DateTime.Now.ToString("yyyy");
				
			entry_dia2.Text = DateTime.Now.ToString("dd");
			entry_mes2.Text = DateTime.Now.ToString("MM");
			entry_ano2.Text = DateTime.Now.ToString("yyyy");
				
			//  Sale de la ventana:
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);			
	      
			//  Busca el producto:
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
	        button_consultar_costos.Clicked += new EventHandler(on_button_consultar_costos_clicked);			
			button_quitar_producto.Clicked += new EventHandler(on_button_quitar_producto_clicked);
			button_limpiar.Clicked += new EventHandler(on_button_limpiar_clicked);
			button_imprimir_movimiento.Clicked += new EventHandler(on_button_imprimir_movimiento_clicked);
			
			// acciones al dar click en los botones:
			checkbutton_todos_departamentos.Clicked += new EventHandler(on_checkbutton_todos_departamentos_clicked);
			checkbutton_todos_productos.Clicked += new EventHandler(on_checkbutton_todos_productos_clicked);
			 
			// entry's numericos:
	        this.entry_dia1.KeyPressEvent += onKeyPressEventactual;
	        this.entry_mes1.KeyPressEvent += onKeyPressEventactual;
	        this.entry_ano1.KeyPressEvent += onKeyPressEventactual;
	        this.entry_dia2.KeyPressEvent += onKeyPressEventactual;
	        this.entry_mes2.KeyPressEvent += onKeyPressEventactual;
	        this.entry_ano2.KeyPressEvent += onKeyPressEventactual;
			
			llenado_combobox_departamentos();
				        
		}
		
		void llenado_combobox_departamentos()
		{
			// Llenado de combobox con los tipos de departamentos:
			combobox_departamentos.Clear();
			CellRendererText cell1 = new CellRendererText();
			combobox_departamentos.PackStart(cell1, true);
			combobox_departamentos.AddAttribute(cell1,"text",0);
			
			ListStore store1 = new ListStore( typeof (string), typeof (int));
			combobox_departamentos.Model = store1;
			this.combobox_departamentos.Changed += new EventHandler (onComboBoxChanged_departamentos);
	        
	      	NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada:
            try{
				conexion.Open ();   
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
		        comando.CommandText = "SELECT * FROM osiris_his_tipo_admisiones "+
		               						"WHERE cuenta_mayor = 4000 "+
		               						"ORDER BY descripcion_admisiones;";
						
				NpgsqlDataReader lector = comando.ExecuteReader ();
				store1.AppendValues ("", 0);
		        while (lector.Read()){
					store1.AppendValues ((string) lector["descripcion_admisiones"], (int) lector["id_tipo_admisiones"]);
				}				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void onComboBoxChanged_departamentos(object sender, EventArgs args)
		{
    		ComboBox combobox_departamentos = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_departamentos.GetActiveIter (out iter)){
		    	id_tipo_admisiones = (int) combobox_departamentos.Model.GetValue(iter,1);
		    	query_departamento = " AND osiris_erp_cobros_deta.id_tipo_admisiones = '"+Convert.ToString((int) combobox_departamentos.Model.GetValue(iter,1)).ToString()+"' ";		    			    	
		    	if (this.checkbutton_todos_departamentos .Active == true){
					query_departamento = " ";
				}
	     	}
		}
	
		////////////////////////////////////////VENTANA BUSQUEDA DE PRODUCTOS/////////////////////////////////////////////////	
		void on_button_busca_producto_clicked (object sender, EventArgs args)
		{
			busqueda = "productos";
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda("producto");
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enterbucar_busqueda;
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
								
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta la final de la classe
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
		
	 	[GLib.ConnectBefore ()] 
		public void onKeyPressEvent_enterbucar_busqueda(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
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
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace  && args.Event.Key != Gdk.Key.Delete)
			{
				args.RetVal = true;
			}
		}
			 
		///////////////Acciones del boton todos los departamentos(CHECKBUTTON)///////////////////////
		void on_checkbutton_todos_departamentos_clicked (object sender, EventArgs args)
		{   
			if (checkbutton_todos_departamentos.Active == true){
				combobox_departamentos.Sensitive = false;
		    	query_departamento = "  "; //
		    }else{
				combobox_departamentos.Sensitive = true;
				query_departamento = "AND osiris_erp_cobros_deta.id_tipo_admisiones = '"+id_tipo_admisiones.ToString()+"' ";	
			}
		}
		
		///////////////Acciones del boton todos los productos aplicados por fechas(CHECKBUTTON)///////////////////////
		void on_checkbutton_todos_productos_clicked (object sender, EventArgs args)
		{ 	
			button_busca_producto.Sensitive = true;
			entry_descrip_producto.Sensitive = true;
			/*this.entry_dia1.Sensitive = true;
			this.entry_mes1.Sensitive = true;
			this.entry_ano1.Sensitive = true;
			this.entry_dia2.Sensitive = true;
			this.entry_mes2.Sensitive = true;
			this.entry_ano2.Sensitive = true;*/
			   if ( checkbutton_todos_productos.Active == true ){
			        button_busca_producto.Sensitive = false;
				    entry_descrip_producto.Sensitive = false;
				    /*this.entry_dia1.Sensitive =  false;
			        this.entry_mes1.Sensitive =  false;
			        this.entry_ano1.Sensitive =  false;
			        this.entry_dia2.Sensitive =  false;
			        this.entry_mes2.Sensitive =  false;
			        this.entry_ano2.Sensitive =  false;*/
				    limpiacampos();
				    entry_descrip_producto.Text = "";
				    treeViewEngineSelec.Clear();
				    titulopagina="MOVIMIENTOS DE PACIENTE";
		}else{
		            titulopagina="MOVIMIENTOS DE PRODUCTOS";   }	
		}
		////////////////LIMPIACAMPOS//////////////////////////////////
		public void limpiacampos()
		{
			entry_descrip_producto.Text = "";
		}	
		////////////////BUTTON SELECCIONA//////////////////////////////////
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;

			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
		 		entry_descrip_producto.Text = (string) model.GetValue(iterSelected, 1);
				treeViewEngineSelec.AppendValues ((string) model.GetValue(iterSelected, 0),
														  (string) model.GetValue(iterSelected, 1));
			}else{
 				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, 
							ButtonsType.Close, "NO existen productos para seleccionar");
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
		}
		
		/////////////BUTTON QUITAR//////////////////////////////////////
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
			
		//////////////BUTTON LIMPIAR////////////////////////////
		void on_button_limpiar_clicked(object o, EventArgs args)
		{
		    treeViewEngineSelec.Clear();
		}
			
		/////////////////////////BUTTON CONSULTAR//////////////////////////////
		void on_button_consultar_costos_clicked (object sender, EventArgs args)         
		{
			string query_fechas = "AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') >= '"+entry_ano1.Text.Trim()+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
									            "AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') <= '"+entry_ano2.Text.Trim()+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";
			string productos_seleccionado = "";
			string var_paso = "";
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
		 				llena_treeview_aplicados("AND osiris_erp_cobros_deta.id_producto IN('"+productos_seleccionado+"') ",query_fechas);					
					}else{
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, 
								ButtonsType.Close, "Seleccione un departamento");
						msgBoxError.Run ();				msgBoxError.Destroy();
					}
	 			}else{
	 				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, 
								ButtonsType.Close, "NO existen productos a consultar");
					msgBoxError.Run ();				msgBoxError.Destroy();
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
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
		            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
		        query1 = "SELECT to_char(SUM(cantidad_aplicada),'999999999.99') AS cantidadaplicada,to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,descripcion_producto,"+
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
	        	comando.CommandText = query1;
				Console.WriteLine(query1);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				float total_aplicado = 0;
				while (lector.Read()){
					treeViewEngineResumen.AppendValues ((string) lector["cantidadaplicada"],
														(string) lector["idproducto"],
														(string) lector["descripcion_producto"],
														(string) lector["foliodeservicio"],
														(string) lector["pidpaciente"],
														(string) lector["nombre_paciente"],
														(string) lector["idtipoadmisiones"],
														(string) lector["descripcion_admisiones"],
														(string) lector["fechahoracreacion"]);
														
				 	total_aplicado += float.Parse(((string) lector["cantidadaplicada"]).Trim());
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
		
 		//////////////BUTTON IMPRIMIR////////////////////////////////////////	
		void on_button_imprimir_movimiento_clicked(object sender, EventArgs args)
		{
			TreeIter iter;
			//if(this.query1.GetIterFirst (out iter)){         
			//	new osiris.imprime_mov_productos (this.query1,this.lista_resumen_productos,this.treeViewEngineResumen);
			//}
			if (this.treeViewEngineResumen.GetIterFirst (out iter)){
				//Console.WriteLine(query1);
				
				new osiris.imprime_mov_productos (entry_total_aplicado.Text,entry_dia1.Text,entry_mes1.Text,entry_ano1.Text,entry_dia2.Text,entry_mes2.Text,entry_ano2.Text, this.lista_resumen_productos,this.treeViewEngineResumen,this.query1,this.nombrebd,this.titulopagina);
			}else{
				
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, 
								ButtonsType.Close, "NO existe nada para imprimir");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
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
		
		//  lista_de_resumen:
		enum Column_resumen
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
		/////LISTA_PRODUCTO_SELECCIONADOS/////
		void crea_treeview_selec()
		{
			treeViewEngineSelec = new TreeStore(typeof(string), 
												typeof(string));
												
			lista_producto_seleccionados.Model = treeViewEngineSelec;
			
			lista_producto_seleccionados.RulesHint = true;
			
			TreeViewColumn col_codigo_prod = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_codigo_prod.Title = "Codigo Prod."; // titulo de la cabecera de la columna, si está visible
			col_codigo_prod.PackStart(cellr0, true);
			col_codigo_prod.AddAttribute (cellr0, "text", 0);

			TreeViewColumn col_descripcion = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_descripcion.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
			col_descripcion.PackStart(cellr1, true);
			col_descripcion.AddAttribute (cellr1, "text", 1);
			
			lista_producto_seleccionados.AppendColumn(col_codigo_prod);
			lista_producto_seleccionados.AppendColumn(col_descripcion);
		}
		
		enum Column
		{
			col_codigo_prod,
			col_descripcion,
		}
		
		//LISTA_RESUMEN_PRODUCTOS///   
		void crea_treeview_resumen()
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
			col_cantidad.SortColumnId = (int) Column_resumen.col_cantidad;
            
            TreeViewColumn col_id_producto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_id_producto.Title = "ID Producto."; // titulo de la cabecera de la columna, si está visible
			col_id_producto.PackStart(cellr1, true);
			col_id_producto.AddAttribute (cellr1, "text", 1);
			col_id_producto.SortColumnId = (int) Column_resumen.col_id_producto;
			
			TreeViewColumn col_descripcion = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_descripcion.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
			col_descripcion.PackStart(cellr2, true);
			col_descripcion.AddAttribute (cellr2, "text", 2);
			col_descripcion.SortColumnId = (int) Column_resumen.col_descripcion;
			
			TreeViewColumn col_folio_de_servicio = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_folio_de_servicio.Title = "Num.Atencion"; // titulo de la cabecera de la columna, si está visible
			col_folio_de_servicio.PackStart(cellr3, true);
			col_folio_de_servicio.AddAttribute (cellr3, "text", 3);
			col_folio_de_servicio.SortColumnId = (int) Column_resumen. col_folio_de_servicio;
			
			TreeViewColumn col_pid_paciente = new TreeViewColumn();
			CellRendererText cellr4 = new CellRendererText();
			col_pid_paciente.Title = "PID Paciente"; // titulo de la cabecera de la columna, si está visible
			col_pid_paciente.PackStart(cellr4, true);
			col_pid_paciente.AddAttribute (cellr4, "text", 4);
			col_pid_paciente.SortColumnId = (int) Column_resumen. col_pid_paciente;
			
			TreeViewColumn col_nombre_paciente = new TreeViewColumn();
			CellRendererText cellr5 = new CellRendererText();
			col_nombre_paciente.Title = "Nombre Paciente"; // titulo de la cabecera de la columna, si está visible
			col_nombre_paciente.PackStart(cellr5, true);
			col_nombre_paciente.AddAttribute (cellr5, "text", 5);
			col_nombre_paciente.SortColumnId = (int) Column_resumen. col_nombre_paciente;
			
			TreeViewColumn col_id_departamento = new TreeViewColumn();
			CellRendererText cellr6 = new CellRendererText();
			col_id_departamento.Title = "id Departamento"; // titulo de la cabecera de la columna, si está visible
			col_id_departamento.PackStart(cellr6, true);
			col_id_departamento.AddAttribute (cellr6, "text", 6);
			col_id_departamento.SortColumnId = (int) Column_resumen.col_id_departamento;
			
			TreeViewColumn col_departamento = new TreeViewColumn();
			CellRendererText cellr7 = new CellRendererText();
			col_departamento.Title = "Departamento"; // titulo de la cabecera de la columna, si está visible
			col_departamento.PackStart(cellr7, true);
			col_departamento.AddAttribute (cellr7, "text", 7);
			col_departamento.SortColumnId = (int) Column_resumen.col_departamento;
			
			TreeViewColumn col_fecha_cargo = new TreeViewColumn();
			CellRendererText cellr8 = new CellRendererText();
			col_fecha_cargo.Title = "Fecha de Cargo"; // titulo de la cabecera de la columna, si está visible
			col_fecha_cargo.PackStart(cellr8, true);
			col_fecha_cargo.AddAttribute (cellr8, "text", 8);
			col_fecha_cargo.SortColumnId = (int) Column_resumen.col_fecha_cargo;
			
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
		
///////////////////// CIERRA VENTANAS EMERGENTES///////////////////////////////////////////////////////
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		} 
	}
}