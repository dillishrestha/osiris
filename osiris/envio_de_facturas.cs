///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// envio_de_facturas.cs created with MonoDevelop
// User: egonzalez at 11:47 a 23/05/2008
// Erick Eduardo Gonzalez Reyes
// R. Israel Peña Gzz.
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{				
	public class envio_de_facturas
	{
		[Widget] Gtk.Window envio_facturas;
		[Widget] Gtk.Button button_salir;
		[Widget] Gtk.Button button_enviar_factura;
		[Widget] Gtk.Button button_imprimir;
		[Widget] Gtk.Button button_buscar_cliente;
		[Widget] Gtk.Button button_facturas_enviadas;
		[Widget] Gtk.Entry entry_buscar;
		[Widget] Gtk.Entry entry_del_dia;
		[Widget] Gtk.Entry entry_del_mes;
		[Widget] Gtk.Entry entry_del_anno;
		[Widget] Gtk.Entry entry_al_dia;
		[Widget] Gtk.Entry entry_al_mes;
		[Widget] Gtk.Entry entry_al_anno;
		[Widget] Gtk.TreeView treeview_lista_facturas;
		[Widget] Gtk.CheckButton check_enviadas;
		[Widget] Gtk.CheckButton check_todos_clientes;		   
		[Widget] Gtk.CheckButton check_todas_fechas;		
	
		//Declarando ventanas de busqueda
    	[Widget] Gtk.Window buscador;
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_buscar_busqueda;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.TreeView lista_de_busqueda;
		[Widget] Gtk.ComboBox combobox_tipo_busqueda;
			
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string NomEmpleados;
		
		decimal  total_monto_facturas;
		
		string fecha_envio;
		int contar_facturas;
		int numerofactura;
		string fecha_de_envio;
		int iddelcliente;
		string query_clientes = ";";
		string query_fechas ="AND osiris_erp_factura_enca.enviado = 'false' ";
		string query_facturas = "" ;	//rpt_evnvio_de_factuiras
							
		string connectionString;
		string nombrebd;
		
		private TreeStore treeViewEngineBuscafacturas;
		private TreeStore treeViewEngineClientes;
		
    	protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public envio_de_facturas(string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_)
		{			
			LoginEmpleado = LoginEmp;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			NomEmpleados = NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
					
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "envio_facturas", null);
			gxml.Autoconnect (this);
	        envio_facturas.Show();
			
            this.entry_al_dia.Sensitive = false;
			this.entry_al_mes.Sensitive = false;
			this.entry_al_anno.Sensitive = false;
			this.entry_del_dia.Sensitive = false;
			this.entry_del_mes.Sensitive = false;
			this.entry_del_anno.Sensitive = false; 
			
            this.entry_buscar.Editable =false;			
            
			this.button_facturas_enviadas.Sensitive =false;
            this.check_todas_fechas.Sensitive = false;			
    		this.entry_del_dia.Text = DateTime.Today.ToString("dd");
			this.entry_del_mes.Text = DateTime.Today.ToString("MM");
			this.entry_del_anno.Text = DateTime.Today.ToString("yyyy");
			this.entry_al_dia.Text = DateTime.Today.ToString("dd");
			this.entry_al_mes.Text = DateTime.Today.ToString("MM");
			this.entry_al_anno.Text = DateTime.Today.ToString("yyyy");
			
			check_todas_fechas.Clicked += new EventHandler(on_check_todas_fechas);
			check_enviadas.Clicked += new EventHandler(on_check_facturas_clicked);	
			check_todos_clientes.Clicked += new EventHandler(on_check_todos_clientes_clicked);	
            button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);	
			button_buscar_cliente.Clicked += new EventHandler(on_buscar_cliente_clicked);
			this.button_facturas_enviadas.Clicked += new EventHandler(on_facturas_enviadas_clicked);
			this.button_enviar_factura.Clicked += new EventHandler(on_enviar_facturas_clicked);
			this.button_imprimir.Clicked += new EventHandler(on_button_imprimir_clicked);
			crea_treeview_busqueda_factura();
		}
		
		void on_check_todas_fechas(object sender, EventArgs args)		
		{
			if (this.check_todas_fechas.Active == true){
				this.entry_al_anno.Sensitive =false;			
				this.entry_al_mes.Sensitive =false;
				this.entry_al_dia.Sensitive =false;	
				this.entry_del_anno.Sensitive =false;			
				this.entry_del_mes.Sensitive =false;
				this.entry_del_dia.Sensitive =false;
			}else{
				this.entry_al_anno.Sensitive =true;			
				this.entry_al_mes.Sensitive =true;
				this.entry_al_dia.Sensitive =true;	
				this.entry_del_anno.Sensitive =true;			
				this.entry_del_mes.Sensitive =true;
				this.entry_del_dia.Sensitive =true;	   			
			}
		}
		
		////////////////////////////////////////IMPRIMIR ENVIO DE FACTURAS////////////////////////////////////////////////////////////
		void on_button_imprimir_clicked (object sender, EventArgs args)
		{
			TreeIter iter;
			if (this.treeViewEngineBuscafacturas.GetIterFirst (out iter)){
				//new osiris.rpt_envio_de_facturas (total_monto_facturas,fecha_de_envio,entry_buscar.Text,entry_al_dia.Text,entry_al_mes.Text,entry_al_anno.Text,entry_del_dia.Text,entry_del_mes.Text,entry_del_anno.Text,this.treeview_lista_facturas,this.treeViewEngineBuscafacturas,this.query_facturas,this.nombrebd);
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, 
								ButtonsType.Close, "NO existe nada para imprimir");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
		}
		////////////////////////////////////////Enviar Factura////////////////////////////////////////////////////////////////////////
		void on_enviar_facturas_clicked(object sender, EventArgs args)
		{
			if (this.contar_facturas !=0 ){
		 		MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
			        MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de que desea enviar {0}  factruas selecionadas al cliente {1} ?",this.contar_facturas,this.entry_buscar.Text );
		        	ResponseType miResultado = (ResponseType)msgBox.Run ();
			        msgBox.Destroy();
				
				if (miResultado == ResponseType.Yes){
					int	conteo = 0;
					TreeIter iter;
				
					if (this.treeViewEngineBuscafacturas.GetIterFirst (out iter)){
						bool oldPac = (bool) treeview_lista_facturas.Model.GetValue (iter,0);
						if (oldPac == true ){
							Console.WriteLine("primer fila seleccionada");
							conteo = 1;
							numerofactura = (int) treeview_lista_facturas.Model.GetValue(iter,1);
							envia_facturas_de_clientes();
						}				   	 
									
						while (this.treeViewEngineBuscafacturas.IterNext(ref iter)){
							bool oldPac2 = (bool) treeview_lista_facturas.Model.GetValue (iter,0);
						    if (oldPac2 == true ){
							    conteo = conteo +1;
								numerofactura = (int) treeview_lista_facturas.Model.GetValue(iter,1);
								//Console.WriteLine("numero factura: "+numerofactura);
								envia_facturas_de_clientes();
							}
						}
						numerofactura = 0;
						llena_lista_factura();
					}
					Console.WriteLine("check seleccionados: "+conteo);
				}else{ //else del mensaje box
					Console.WriteLine("No Acepto");
				}
			}else{//else comrpueba que haya facturas seleccionadas
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
											ButtonsType.Ok,"No hay facturas seleccionadass, Seleccione facturas para poder enviar");
				msgBox.Run();			msgBox.Destroy();
			}
		}
		
		void envia_facturas_de_clientes()
		{
			fecha_envio = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
	        NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
								    	// libera la habitacion anterior
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "UPDATE osiris_erp_factura_enca SET "+ 
				                      "enviado = 'true',"+
				                      "fecha_de_envio = '"+fecha_envio+"'"+ 
								      "WHERE numero_factura =  '"+numerofactura+"';";
				
				comando.ExecuteNonQuery();
				comando.Dispose();					
              
				Console.WriteLine(comando.CommandText+"------------");
				conexion.Close ();
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error, 
									ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
		}
		
		void on_facturas_enviadas_clicked(object sender, EventArgs args)
		{
			
			if (this.check_todas_fechas.Active == true){
				query_fechas = "AND osiris_erp_factura_enca.enviado = 'true' ";				               
				               llena_lista_factura();
			}else{	
				query_fechas = "AND osiris_erp_factura_enca.enviado = 'true' "+				               
				               "AND to_char(osiris_erp_factura_enca.fecha_de_envio,'yyyy-MM-dd') >= '"+this.entry_del_anno.Text+"-"+this.entry_del_mes.Text+"-"+this.entry_del_dia.Text+"' "+
						       "AND to_char(osiris_erp_factura_enca.fecha_de_envio,'yyyy-MM-dd') <= '"+this.entry_al_anno.Text+"-"+this.entry_al_mes.Text+"-"+this.entry_al_dia.Text+"' " ;
				llena_lista_factura();
			}
		}
		
		void on_check_todos_clientes_clicked(object sender, EventArgs args)
		{
			if (this.check_todos_clientes.Active == true && this.check_enviadas.Active == false){
				this.treeViewEngineBuscafacturas.Clear();
	    		llena_lista_factura();
			}
			if (this.check_todos_clientes.Active == false){ //no busca por que nmo hay clienets selecionados
			    
				this.treeViewEngineBuscafacturas.Clear();
				this.entry_buscar.Text = "";
				this.button_buscar_cliente.Sensitive = true;
				this.button_enviar_factura.Sensitive = true;
			}
			if (this.check_todos_clientes.Active == true){
	            this.query_clientes = ";";
				this.button_buscar_cliente.Sensitive = false;
				this.button_enviar_factura.Sensitive = false;
				this.entry_buscar.Text = "Todos Los Clientes";
			}
		}

		void on_buscar_cliente_clicked(object sender, EventArgs args)
		{
     	   // busqueda = "clientes";
 			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_clientes);
	        entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
	        button_selecciona.Clicked += new EventHandler(on_selecciona_cliente);
	       	crea_treeview_clientes();
			button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);			
		}
		
	    [GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;		
				llenando_lista_de_clientes(); 	
			}
		}
		
		void crea_treeview_clientes()
		{
			treeViewEngineClientes = new TreeStore(typeof(int),//0
													typeof(string),//1
													typeof(string),//2
													typeof(string),//3
													typeof(string),//4
													typeof(string),//5
													typeof(string),//6
													typeof(string),//7
													typeof(string),//8
													typeof(string),//9
													typeof(string),//10
													typeof(string),//11
													typeof(bool),//12
													typeof(int),//13
													typeof(string));//14
												
			lista_de_busqueda.Model = treeViewEngineClientes;
			
			lista_de_busqueda.RulesHint = true;
				
			lista_de_busqueda.RowActivated += on_selecciona_cliente;  // Doble click selecciono paciente*/
			TreeViewColumn col_idcliente = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idcliente.Title = "ID Clientes"; // titulo de la cabecera de la columna, si está visible
			col_idcliente.PackStart(cellr0, true);
			col_idcliente.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1
			//col_idcliente.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_idcliente.SortColumnId = (int) Col_clientes.col_idcliente;
			
			TreeViewColumn col_cliente = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_cliente.Title = "Clientes";
			col_cliente.PackStart(cellrt1, true);
			col_cliente.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
			//col_cliente.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_cliente.SortColumnId = (int) Col_clientes.col_cliente;
			
			TreeViewColumn col_calle = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_calle.Title = "Calle";
			col_calle.PackStart(cellrt2, true);
			col_calle.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 3
			//col_calle.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_calle.SortColumnId = (int) Col_clientes.col_calle;
			
			TreeViewColumn col_colonia = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_colonia.Title = "Colonia";
			col_colonia.PackStart(cellrt3, true);
			col_colonia.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 4
			//col_colonia.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_colonia.SortColumnId = (int) Col_clientes.col_colonia;
			
            TreeViewColumn col_codpos = new TreeViewColumn();
            CellRendererText cellrt4 = new CellRendererText();
            col_codpos.Title = "Codigo Postal";
            col_codpos.PackStart(cellrt4,true);
            col_codpos.AddAttribute(cellrt4,"text",4);// la siguiente columna será 5
            //col_codpos.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_codpos.SortColumnId = (int) Col_clientes.col_codpos;
			
            TreeViewColumn col_municipio = new TreeViewColumn();
            CellRendererText cellrt5 = new CellRendererText();
            col_municipio.Title = "Municipio";
            col_municipio.PackStart(cellrt5, true);
			col_municipio.AddAttribute(cellrt5,"text",5);// la siguiente columna será 6
			//col_municipio.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_municipio.SortColumnId = (int) Col_clientes.col_municipio;
			
            TreeViewColumn col_estado = new TreeViewColumn();
            CellRendererText cellrt6 = new CellRendererText();
            col_estado.Title = "Estado";
            col_estado.PackStart(cellrt6,true);
            col_estado.AddAttribute(cellrt6,"text",6);// la siguiente columna será 7
            //col_estado.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_estado.SortColumnId = (int) Col_clientes.col_estado;
			
            TreeViewColumn col_tel_casa = new TreeViewColumn();
            CellRendererText cellrt7 = new CellRendererText();
            col_tel_casa.Title = "Tel. Casa";
            col_tel_casa.PackStart(cellrt7,true);
            col_tel_casa.AddAttribute(cellrt7,"text",7);// la siguiente columna será 8
            //col_tel_casa.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
            col_tel_casa.SortColumnId = (int) Col_clientes.col_tel_casa;
			          
            TreeViewColumn col_tel_oficina = new TreeViewColumn();
            CellRendererText cellrt8 = new CellRendererText();
            col_tel_oficina.Title = "Tel. Of.";
            col_tel_oficina.PackStart(cellrt8,true);
            col_tel_oficina.AddAttribute(cellrt8,"text",8);// la siguiente columna será 9
            //col_tel_oficina.SetCellDataFunc(cellrt8, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
            col_tel_oficina.SortColumnId = (int) Col_clientes.col_tel_oficina;
			
            TreeViewColumn col_celular = new TreeViewColumn();
            CellRendererText cellrt9 = new CellRendererText();
            col_celular.Title = "Celular";
            col_celular.PackStart(cellrt9,true);
            col_celular.AddAttribute(cellrt9,"text",9);// la siguiente columna será 10
            //col_celular.SetCellDataFunc(cellrt9, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
            col_celular.SortColumnId = (int) Col_clientes.col_celular;
			
		    lista_de_busqueda.AppendColumn(col_idcliente);
			lista_de_busqueda.AppendColumn(col_cliente);
			lista_de_busqueda.AppendColumn(col_calle);
			lista_de_busqueda.AppendColumn(col_colonia);
			lista_de_busqueda.AppendColumn(col_codpos);
			lista_de_busqueda.AppendColumn(col_municipio);
			lista_de_busqueda.AppendColumn(col_estado);
			lista_de_busqueda.AppendColumn(col_tel_casa);
			lista_de_busqueda.AppendColumn(col_tel_oficina);
			lista_de_busqueda.AppendColumn(col_celular);
		}
		
		enum Col_clientes
		{
			col_idcliente,
			col_cliente,
			col_calle,
			col_colonia,
			col_codpos,
			col_municipio,
			col_estado,
			col_tel_casa,
			col_tel_oficina,
			col_celular
		}
		
		void on_llena_lista_clientes(object sender, EventArgs args)
		{
			llenando_lista_de_clientes();
		}
		
		void llenando_lista_de_clientes()
		{
			treeViewEngineClientes.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				if ((string) entry_expresion.Text.ToUpper() == "*")
					{
					comando.CommandText = "SELECT descripcion_cliente,direccion_cliente,rfc_cliente,curp_cliente, "+
								"colonia_cliente,municipio_cliente,estado_cliente,telefono1_cliente, "+ 
								"telefono2_cliente,celular_cliente,cp_cliente,"+
								"osiris_erp_clientes.id_forma_de_pago,descripcion_forma_de_pago AS descripago,"+ 
								"cliente_activo,id_cliente "+
								"FROM osiris_erp_clientes,osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_clientes.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
							    "AND osiris_erp_clientes.id_cliente !='1' "+
							    "AND osiris_erp_clientes.envio_factura = 'true' "+
							    "AND osiris_erp_clientes.cliente_activo = 'true' "+
								"ORDER BY descripcion_cliente;";
				}else{
					comando.CommandText = "SELECT descripcion_cliente,direccion_cliente,rfc_cliente,curp_cliente, "+
								"colonia_cliente,municipio_cliente,estado_cliente,telefono1_cliente, "+ 
								"telefono2_cliente,celular_cliente,cp_cliente,"+
								"osiris_erp_clientes.id_forma_de_pago,descripcion_forma_de_pago AS descripago,"+ 
								"cliente_activo,id_cliente "+
								"FROM osiris_erp_clientes,osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_clientes.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
								"AND descripcion_cliente LIKE '%"+(string) entry_expresion.Text.ToUpper()+"%' "+
								"AND osiris_erp_clientes.id_cliente != '1' "+
								"AND osiris_erp_clientes.envio_factura = 'true' "+
								"AND osiris_erp_clientes.cliente_activo = 'true' "+
							    "ORDER BY descripcion_cliente;";
				}
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//bool verifica_activacion;
				while (lector.Read()){	
					treeViewEngineClientes.AppendValues ((int) lector["id_cliente"],//0
													(string) lector["descripcion_cliente"],//1
													(string) lector["direccion_cliente"],//2
													(string) lector["colonia_cliente"],//3
													(string) lector["cp_cliente"],//4
													(string) lector["municipio_cliente"],//5
													(string) lector["estado_cliente"],//6
													(string) lector["telefono1_cliente"],//7
													(string) lector["telefono2_cliente"],//8
													(string) lector["celular_cliente"],//9
													(string) lector["rfc_cliente"],//10
													(string) lector["curp_cliente"],//11
													(bool) lector["cliente_activo"],//12
													(int) lector["id_forma_de_pago"],//13
													(string) lector["descripago"]);//14
					
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_selecciona_cliente(object sender, EventArgs args)
		{
			//Console.WriteLine("ENTRE");
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_busqueda.Selection.GetSelected(out model, out iterSelected)) 
 			{
				this.entry_buscar.Text = (string) model.GetValue(iterSelected,1);
				iddelcliente = (int) model.GetValue(iterSelected, 0);


				//activa_botones();
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
				
				query_clientes = "AND  osiris_erp_cobros_enca.id_cliente = '"+iddelcliente+"';";
				if (this.check_enviadas.Active == false)
				{llena_lista_factura();}
			}
		}
	
		void crea_treeview_busqueda_factura()
     	{
            treeViewEngineBuscafacturas = new TreeStore(typeof(bool),typeof(int),typeof(string),typeof(int),typeof(string),typeof(string),typeof(string),typeof(string),typeof(bool));
			this.treeview_lista_facturas.Model = treeViewEngineBuscafacturas;
			
			treeview_lista_facturas.RulesHint = true;
			
			TreeViewColumn col_check = new TreeViewColumn();
			CellRendererToggle cel_check = new CellRendererToggle();
			
			col_check.Title = "Seleccionar"; // titulo de la cabecera de la columna, si está visible
			col_check.PackStart(cel_check, true);
			col_check.AddAttribute (cel_check, "active", 0);
			cel_check.Activatable = true;
			cel_check.Toggled += selecciona_fila;
			col_check.SortColumnId = (int) Column2.col_check;
			
			TreeViewColumn Factura = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			Factura.Title = "N° de Factura"; // titulo de la cabecera de la columna, si está visible
			Factura.PackStart(cellr0, true);
		    Factura.AddAttribute (cellr0, "text", 1);    // la siguiente columna será 1 en vez de 1
			Factura.SortColumnId = (int) Column2.Factura;
							
            TreeViewColumn Fecha = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			Fecha.Title = "Fecha de creacion de Factura"; // titulo de la cabecera de la columna, si está visible
			Fecha.PackStart(cellr1, true);
		    Fecha.AddAttribute (cellr1, "text", 2);    // la siguiente columna será 1 en vez de 1
			Fecha.SortColumnId = (int) Column2.Fecha;
				
			TreeViewColumn Folio = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			Folio.Title = "Folio de Atencion"; // titulo de la cabecera de la columna, si está visible
			Folio.PackStart(cellr2, true);
			Folio.AddAttribute (cellr2, "text", 3);    // la siguiente columna será 1 en vez de 1
			Folio.SortColumnId = (int) Column2.Folio;
			            
			TreeViewColumn Paciente = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			Paciente.Title = "Paciente";
			Paciente.PackStart(cellrt3, true);
			Paciente.AddAttribute (cellrt3, "text", 4); // la siguiente columna será 1 en vez de 2
			Paciente.SortColumnId = (int) Column2.Paciente;
			
			TreeViewColumn fechaenvio = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			fechaenvio.Title = "Fecha de envio";
			fechaenvio.PackStart(cellrt4, true);
			fechaenvio.AddAttribute (cellrt4, "text", 5); // la siguiente columna será 1 en vez de 2
			fechaenvio.SortColumnId = (int) Column2.fechaenvio;

            TreeViewColumn cliente = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			cliente.Title = "Cliente";
			cliente.PackStart(cellrt5, true);
			cliente.AddAttribute (cellrt5, "text", 6); // la siguiente columna será 1 en vez de 2
			cliente.SortColumnId = (int) Column2.cliente;	
			
			TreeViewColumn montofactura = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			montofactura.Title = "Monto Factura";
			montofactura.PackStart(cellrt6, true);
			montofactura.AddAttribute (cellrt6, "text", 7); // la siguiente columna será 1 en vez de 2
			montofactura.SortColumnId = (int) Column2.montofactura;
			
			TreeViewColumn pagado = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			pagado.Title = "Pagado";
			pagado.PackStart(cellrt7, true);
			pagado.AddAttribute (cellrt7, "text", 8); // la siguiente columna será 1 en vez de 2
			pagado.SortColumnId = (int) Column2.pagado;
			
			treeview_lista_facturas.AppendColumn(col_check);
            treeview_lista_facturas.AppendColumn(Factura);
			treeview_lista_facturas.AppendColumn(Fecha);
			treeview_lista_facturas.AppendColumn(Folio);
			treeview_lista_facturas.AppendColumn(Paciente);
			treeview_lista_facturas.AppendColumn(fechaenvio);
			treeview_lista_facturas.AppendColumn(cliente);
	        treeview_lista_facturas.AppendColumn(montofactura);
			treeview_lista_facturas.AppendColumn(pagado);
		}
		
		enum Column2
		{   
			col_check,	
			Factura,
			Fecha,
			Folio,
			Paciente,
			fechaenvio,
			cliente,
			montofactura,
			pagado
		}		
		
    	void selecciona_fila(object sender, ToggledArgs args)
		{
			
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_lista_facturas.Model.GetIter (out iter, path)) {
				bool old = (bool) treeview_lista_facturas.Model.GetValue (iter,0);
				
				treeview_lista_facturas.Model.SetValue(iter,0,!old);
	             if (old == false){
					treeview_lista_facturas.Model.SetValue (iter,0,true);
					contar_facturas=contar_facturas+1;				
				}	
				if (old == true){
					treeview_lista_facturas.Model.SetValue (iter,0,false);
					contar_facturas=contar_facturas-1;				
				}	
			}	
		}		
		
		
		void llena_lista_factura()
		{
			contar_facturas=0;
			fecha_de_envio = "";
		
			this.treeViewEngineBuscafacturas.Clear();
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				//comando.CommandText = 
				query_facturas  =   "SELECT osiris_erp_cobros_enca.id_cliente,osiris_erp_cobros_enca.numero_factura,osiris_his_paciente.nombre1_paciente,osiris_his_paciente.nombre2_paciente,osiris_erp_clientes.envio_factura,"+
							            "osiris_his_paciente.apellido_paterno_paciente,osiris_his_paciente.apellido_materno_paciente,osiris_erp_clientes.descripcion_cliente,osiris_erp_cobros_enca.folio_de_servicio, "+
						                "to_char(osiris_erp_factura_enca.fecha_factura,'dd-MM-yyyy') as fechadefectura, "+
						                "to_char(osiris_erp_factura_enca.fecha_de_envio,'dd-MM-yyyy') as fechadeenvio, "+
						                "to_char((osiris_erp_factura_enca.sub_total_15+sub_total_0+iva_al_15+osiris_erp_factura_enca.honorario_medico)-(osiris_erp_factura_enca.deducible+osiris_erp_factura_enca.coaseguro),'999999.99') AS totalfactura, "+ 
						                "osiris_erp_cobros_enca.pagado "+
	  								    "FROM osiris_erp_cobros_enca,osiris_erp_factura_enca,osiris_erp_clientes,osiris_his_paciente " +
									    "WHERE (osiris_erp_cobros_enca.id_cliente = osiris_erp_clientes.id_cliente) "+
						                "AND (osiris_erp_cobros_enca.numero_factura = osiris_erp_factura_enca.numero_factura) "+
						                "AND (osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente) "+
						                "AND osiris_erp_clientes.id_cliente != '1' "+
						                "AND osiris_erp_clientes.envio_factura = 'true' "+
						                 //query de fechas contiene (enviados true  o false) dependiendo del check
						                 query_fechas +
						                 // query de clienets a traer depende del boton buqueda y del check de todos los clienes
						                 query_clientes;				                        
                                       // Console.WriteLine("querifechas:  "+query_fechas+"\n query clientes: "+query_clientes);
			                            
				comando.CommandText = query_facturas;
				Console.WriteLine(query_facturas);                        
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					if (this.check_enviadas.Active == true){
						fecha_de_envio = (string) lector["fechadeenvio"];					
					}else{
						fecha_de_envio = "--------";
					}
					
	                this.treeViewEngineBuscafacturas.AppendValues ((bool) false,
						                                             (int) lector["numero_factura"],
					                                                 (string) lector["fechadefectura"],
						                                             (int) lector["folio_de_servicio"],
						                                             (string) lector["nombre1_paciente"] + " " + lector["nombre2_paciente"] + " "+ lector["apellido_paterno_paciente"] + " " +  lector["apellido_materno_paciente"],
						                                             (string) fecha_de_envio,
						                                             (string) lector["descripcion_cliente"],
					                                                 (string)  lector["totalfactura"].ToString().Trim(),
					                                                 (bool) lector["pagado"]);
					
				        total_monto_facturas += decimal.Parse(((string) lector["totalfactura"]).Trim());
					 
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					         					MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				                                msgBoxError.Run ();	msgBoxError.Destroy();
			}
            conexion.Close ();
		}

		void on_check_facturas_clicked(object sender, EventArgs args)
		{
			if (this.check_enviadas.Active == true){
				this.treeViewEngineBuscafacturas.Clear();
			    this.entry_al_dia.Sensitive = true;
				this.entry_al_mes.Sensitive = true;
				this.entry_al_anno.Sensitive = true;
				this.entry_del_dia.Sensitive = true;
				this.entry_del_mes.Sensitive = true;
				this.entry_del_anno.Sensitive = true; 
			    this.button_facturas_enviadas.Sensitive = true;
				this.check_todas_fechas.Sensitive = true;
				//parametro variable query enviado true
			}else{
				this.treeViewEngineBuscafacturas.Clear();
				this.entry_al_dia.Sensitive = false;
				this.entry_al_mes.Sensitive = false;
				this.entry_al_anno.Sensitive = false;
				this.entry_del_dia.Sensitive = false;
				this.entry_del_mes.Sensitive = false;
				this.entry_del_anno.Sensitive = false; 
				this.button_facturas_enviadas.Sensitive =false;
	            this.check_todas_fechas.Sensitive = false;			
				query_fechas = "AND osiris_erp_factura_enca.enviado = 'false' ";
					       
				//llena_lista_factura();
				//parametro varable query enviado false
		    }
		}
		
		void on_cierraventanas_clicked(object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}
