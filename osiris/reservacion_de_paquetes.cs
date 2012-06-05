// reservacion_de_paquetes.cs created with MonoDevelop
// User: ipena at 06:19 p 10/06/2008
// Sistema Hospitalario OSIRIS                                                                                     //////
// Monterrey - Mexico                                                                                         //////
//                                                                                                            //////
// Autor    	: Israel Peña Gonzalez - el_rip@hotmail.com (Programacion Mono)                               //////
//                                                                                                            //////				  
//                                                                                                            //////				  
// Licencia		: GLP                                                                                         //////	      
// S.O. 		: 
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////		
// To change standard headers go to Edit->Preferences->Coding->Standard Headers                               //////	
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using System.Collections;

namespace osiris
{	
	public class reservacion_de_paquetes
	{
		[Widget] Gtk.Window reserva_paquete;
		//Fechas:
		[Widget] Gtk.Entry entry_dia1;                     
		[Widget] Gtk.Entry entry_mes1;
		[Widget] Gtk.Entry entry_anno1;
		[Widget] Gtk.Entry entry_dia_reservacion;
		[Widget] Gtk.Entry entry_mes_reservacion;
		[Widget] Gtk.Entry entry_anno_reservacion;
	
		[Widget] Gtk.Entry entry_folio_servicio;
		[Widget] Gtk.Entry entry_pid_paciente;
		[Widget] Gtk.Entry entry_nombre_paciente;
		[Widget] Gtk.Entry entry_paq_pres;
		[Widget] Gtk.Entry entry_id_paq_pres;
		[Widget] Gtk.Entry entry_precio_paquete;
		[Widget] Gtk.Entry entry_dia_internamiento;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_busca_cirugia;
		[Widget] Gtk.Button button_grabar;
		[Widget] Gtk.Button button_imprimir;
		[Widget] Gtk.Button button_liberar_paquete;
		[Widget] Gtk.Button button_salir;
		
		//radiobuttons
		[Widget] Gtk.RadioButton radiobutton_paquete;
		[Widget] Gtk.RadioButton radiobutton_presupuesto;
		
		///////Ventana de Busqueda de cirugias
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.TreeView lista_cirugia;
		[Widget] Gtk.Button button_llena_cirugias;
		
		TreeStore treeViewEngineBusca;
		
		int idtipocirugia = 1;	        			
		string cirugia;
		string tipobusqueda = "";
		int idtipoesp = 1;
		int folioservicio = 0;	        		// Toma el valor de numero de atencion de paciente
		string fecharegadmision;
		
		int idpresupuesto =1; 
		string desccirugia = "";
		
		int valorpaquete = 0;
		string query_fecha_reservacion;
		
		string LoginEmpleado;
		string NomEmp_;
		string NomEmpleado = "";
		string AppEmpleado = "";
		string ApmEmpleado = "";
		
		string entry_nombre_1;
		string entry_nombre_2;
		string entry_apellido_paterno;
		string entry_apellido_materno;
		
		string connectionString;
		string nombrebd;
        
		protected Gtk.Window MyWin;
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public reservacion_de_paquetes(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_,int folioservicio_,bool buscafolio) 
		{
		    LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		folioservicio = folioservicio_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;    		
		
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "reserva_paquete",null );
			gxml.Autoconnect (this);
			reserva_paquete.Show();
		
			//Fechas Prosesos//
		    entry_dia1.Text = DateTime.Now.ToString("dd");
			entry_mes1.Text = DateTime.Now.ToString("MM");
			entry_anno1.Text = DateTime.Now.ToString("yyyy");
			
			//Fechas Reservacion//		
			entry_dia_reservacion.Text = DateTime.Now.ToString("dd");
			entry_mes_reservacion.Text = DateTime.Now.ToString("MM");
			entry_anno_reservacion.Text = DateTime.Now.ToString("yyyy");
		
			//  Sale de la ventana:
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			// boton selecciona folio:
			button_selecciona.Clicked += new EventHandler(on_selecciona_folio_clicked);
			// Validando que sean solo numeros
			entry_folio_servicio.KeyPressEvent += onKeyPressEvent_enter_folio;
			// Activa busqueda de cirugia
			button_busca_cirugia.Clicked += new EventHandler(on_button_busca_cirugia_clicked);
			//Radiobuttons:
			radiobutton_paquete.Clicked += new EventHandler(on_radiobutton_paquete_clicked);
			radiobutton_presupuesto.Clicked += new EventHandler(on_radiobutton_presupuesto_clicked);
			//Guarda datos de la reservacion
			button_grabar.Clicked += new EventHandler(on_button_grabar_reservacin_clicked);
			//button liberar paquete o presupuesto:
			this.button_liberar_paquete.Clicked += new EventHandler(on_button_liberar_paquete);
			//Imprimir:
			button_imprimir.Clicked += new EventHandler(on_button_imprimir_reservacion_clicked);
			
			// entry's numericos:
	        this.entry_dia1.KeyPressEvent += onKeyPressEventactual;
	        this.entry_mes1.KeyPressEvent += onKeyPressEventactual;
	        this.entry_anno1.KeyPressEvent += onKeyPressEventactual;
	        this.entry_dia_reservacion.KeyPressEvent += onKeyPressEventactual;
	        this.entry_mes_reservacion.KeyPressEvent += onKeyPressEventactual;
	        this.entry_anno_reservacion.KeyPressEvent += onKeyPressEventactual;
			
			//no deja editar la fecha proseso:
			this.entry_dia1.IsEditable = false; 
			this.entry_mes1.IsEditable = false;
			this.entry_anno1.IsEditable = false;
			
			this.entry_nombre_paciente.Sensitive = false;
			this.entry_pid_paciente.Sensitive = false;
			this.entry_paq_pres.Sensitive = false;
			this.entry_id_paq_pres.Sensitive = false;
			this.entry_precio_paquete.Sensitive = false;
			this.entry_dia_internamiento.Sensitive = false;
			this.radiobutton_paquete.Sensitive = false;
			this.radiobutton_presupuesto.Sensitive = false;
			
			//Cerrar butons:
			this.button_liberar_paquete.Sensitive = false;
			this.button_grabar.Sensitive = false;
			this.button_imprimir.Sensitive = false;
			if ((bool) buscafolio == true){
				this.entry_folio_servicio.Text = this.folioservicio.ToString().Trim();
				llena_folio();
			} 
			
		}
		
		void on_selecciona_folio_clicked(object sender, EventArgs args)
		{
			llena_folio();
		}
		
		void llena_folio()
		{
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );

			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando;
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(osiris_erp_cobros_enca.folio_de_servicio,'9999999999') AS foliodeservicio, "+
						"osiris_erp_cobros_enca.pagado,"+
						"osiris_erp_cobros_enca.cancelado,"+
						"osiris_erp_cobros_enca.cerrado,"+
						"osiris_erp_cobros_enca.alta_paciente,"+
						"osiris_erp_cobros_enca.bloqueo_de_folio,"+
						"reservacion,"+
					    "to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente,osiris_his_paciente.nombre1_paciente || ' ' || "+
						"osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_paciente "+
						"FROM osiris_erp_cobros_enca,osiris_his_paciente "+
						"WHERE osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
						//"AND reservacion = 'false' "+
						"AND osiris_erp_cobros_enca.cancelado = 'false' "+
						"AND osiris_erp_cobros_enca.pagado  = 'false' "+
						//"AND osiris_erp_cobros_enca.alta_paciente = 'false' "+
						"AND osiris_erp_cobros_enca.cerrado = 'false' "+
						"AND osiris_erp_cobros_enca.bloqueo_de_folio = 'false' "+
						"AND id_habitacion = 1 "+
						"AND osiris_erp_cobros_enca.folio_de_servicio = '"+this.entry_folio_servicio.Text.Trim()+"';";
                Console.WriteLine(comando.CommandText.ToString());
                                                                     
				NpgsqlDataReader lector = comando.ExecuteReader ();

				if(lector.Read()){
					entry_nombre_paciente.Text = (string) lector["nombre_paciente"];
					entry_pid_paciente.Text = ((string) lector["pidpaciente"]).Trim();
					
					this.entry_nombre_paciente.Sensitive = true;
					this.entry_pid_paciente.Sensitive = true;
					this.entry_paq_pres.Sensitive = true;
					this.entry_id_paq_pres.Sensitive = true;
					this.entry_precio_paquete.Sensitive = true;
					this.entry_dia_internamiento.Sensitive = true;
					this.radiobutton_paquete.Sensitive = true;
					this.radiobutton_presupuesto.Sensitive = true;
                  
					//Abrir buttons:
					if ((bool) lector["reservacion"] == true){
						
						this.button_liberar_paquete.Sensitive = true;
						this.button_grabar.Sensitive = false;
						this.button_busca_cirugia.Sensitive = false;
						/////////////////////////////////////////////////////////////////////////// 
						//query que me muestre la informacion del paquete o presupuesto que reservo
						/////////////////////////////////////////////////////////////////////////// 
						NpgsqlConnection conexion1;
						conexion1 = new NpgsqlConnection (connectionString+nombrebd);
						if(this.radiobutton_paquete.Active == true){
							// Verifica que la base de datos este conectada
							try{
								conexion1.Open ();
								NpgsqlCommand comando1;
								comando1 = conexion1.CreateCommand ();
								comando1.CommandText ="SELECT osiris_erp_reservaciones.folio_de_servicio,to_char(osiris_erp_reservaciones.id_tipo_cirugia,'99999999.99') AS idtipocirugia, "+
									    "to_char(osiris_erp_reservaciones.valor_paquete,'99999999.99') AS valorpaquete, "+
										"to_char(osiris_his_tipo_cirugias.dias_internamiento,'99999999.99') AS diasinternamiento, "+
										"to_char(osiris_erp_reservaciones.id_tipo_cirugia,'9999999999') AS idtipocirugia, "+
										"osiris_his_tipo_cirugias.descripcion_cirugia  "+
										"FROM osiris_erp_reservaciones,osiris_his_tipo_cirugias "+
							        	"WHERE osiris_erp_reservaciones.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
										"AND osiris_erp_reservaciones.folio_de_servicio = '"+this.entry_folio_servicio.Text.Trim()+"';";
								
								Console.WriteLine(comando1.CommandText.ToString());
								NpgsqlDataReader lector1 = comando1.ExecuteReader ();
								if(lector1.Read()){
									entry_paq_pres.Text = (string) lector1["descripcion_cirugia"];
									entry_precio_paquete.Text = (string) lector1["valorpaquete"].ToString().Trim();
									entry_id_paq_pres.Text = (string) lector1["idtipocirugia"].ToString().Trim();
									entry_dia_internamiento.Text = (string) lector1["diasinternamiento"].ToString().Trim();
								}
								this.radiobutton_paquete.Active = true;
							
							}catch (NpgsqlException ex){
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								                                               MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();			msgBoxError.Destroy();						
							}
						}else{
							try{
								conexion1.Open ();
								NpgsqlCommand comando1;
								//Console.WriteLine("select"+comando.CommandText); 
								comando1 = conexion1.CreateCommand ();
								comando1.CommandText ="SELECT osiris_erp_reservaciones.folio_de_servicio,to_char(osiris_erp_reservaciones.id_presupuesto,'99999999.99') AS idpresupuesto, "+
									    "to_char(osiris_erp_reservaciones.valor_paquete,'99999999.99') AS valorpaquete, "+
										"to_char(osiris_his_presupuestos_enca.dias_internamiento,'99999999.99') AS diasinternamiento, "+
										"to_char(osiris_erp_reservaciones.id_presupuesto,'9999999999') AS idpresupuesto, "+
										"osiris_his_tipo_cirugias.descripcion_cirugia  "+
										"FROM osiris_erp_reservaciones,osiris_his_tipo_cirugias,osiris_his_presupuestos_enca "+
										"WHERE osiris_erp_reservaciones.id_presupuesto = osiris_his_presupuestos_enca.id_presupuesto "+ 
										"AND osiris_his_tipo_cirugias.id_tipo_cirugia = osiris_his_presupuestos_enca.id_tipo_cirugia "+
										"AND osiris_erp_reservaciones.folio_de_servicio = '"+this.entry_folio_servicio.Text.Trim()+"';";
   
								//Console.WriteLine("presupuestos"+comando1.CommandText); 
								Console.WriteLine(comando1.CommandText.ToString());
								NpgsqlDataReader lector1 = comando1.ExecuteReader ();
						   
								if(lector1.Read()){
									entry_paq_pres.Text = (string) lector1["descripcion_cirugia"];
									entry_precio_paquete.Text = (string) lector1["valorpaquete"].ToString().Trim();
									entry_id_paq_pres.Text = (string) lector1["idpresupuesto"].ToString().Trim();
									entry_dia_internamiento.Text = (string) lector1["diasinternamiento"].ToString().Trim();
								}
								this.radiobutton_presupuesto.Active = true;
								
							}catch (NpgsqlException ex){
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								                                               MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();			msgBoxError.Destroy();						
							}
						}
						conexion1.Close ();						  
					}else{					
						this.button_liberar_paquete.Sensitive = false;
						this.button_grabar.Sensitive = true;
						this.button_busca_cirugia.Sensitive = true;						
					}
					this.button_imprimir.Sensitive = true;				
				}else{
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
					                                          MessageType.Info,ButtonsType.Ok,"Este numero de atencion NO se puede separara, verifique...");
					msgBox.Run ();			msgBox.Destroy();
				}	
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
			
		///////////////////// Vantana para la busqueda de cirugias///////////////////////////////////////////////////////////////
		void on_radiobutton_paquete_clicked(object sender, EventArgs args)
		{
			entry_paq_pres.Text = " ";
			entry_id_paq_pres.Text = " ";
			entry_dia_internamiento.Text =" ";
			entry_precio_paquete.Text = " ";
		}
		
		void on_radiobutton_presupuesto_clicked(object sender, EventArgs args)
		{
			entry_paq_pres.Text = " ";
			entry_id_paq_pres.Text = " ";
			entry_dia_internamiento.Text =" ";
			entry_precio_paquete.Text = " ";
		}
		
		void on_button_busca_cirugia_clicked(object sender, EventArgs args)
		{    
			if (radiobutton_paquete.Active == true){
				paquetes_a_buscar();}
			  
			if (radiobutton_presupuesto.Active == true){
				presupuestos_a_buscar();}
		}
		
		void presupuestos_a_buscar()
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "busca_cirugias", null);
			gxml.Autoconnect (this);
			tipobusqueda ="presupuesto";
			
			// Activa la salida de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			// Activa la seleccion de cirugia
			button_selecciona.Clicked += new EventHandler(on_selecciona_cirugia_clicked);
			// Activa boton de busqueda
			button_llena_cirugias.Clicked += new EventHandler(on_button_llena_cirugias_clicked);
	        
			treeViewEngineBusca = new TreeStore( typeof(int), typeof(int),typeof(string),typeof(string));
			lista_cirugia.Model = treeViewEngineBusca;
			
			lista_cirugia.RulesHint = true;
			
			lista_cirugia.RowActivated += on_selecciona_cirugia_clicked;  // Doble click selecciono paciente
			
			TreeViewColumn col_idpresupuesto = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idpresupuesto.Title = "ID Presupuesto"; // titulo de la cabecera de la columna, si está visible
			col_idpresupuesto.PackStart(cellr0, true);
			col_idpresupuesto.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
			col_idpresupuesto.SortColumnId = (int) Column_pres.col_idpresupuesto;
			
			TreeViewColumn col_idcirugia = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_idcirugia.Title = "ID Cirugia"; // titulo de la cabecera de la columna, si está visible
			col_idcirugia.PackStart(cellr1, true);
			col_idcirugia.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
			col_idcirugia.SortColumnId = (int) Column_pres.col_idcirugia;
			
			TreeViewColumn col_descripcirugia = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_descripcirugia.Title = "Descripcion de Cirugia";
			col_descripcirugia.PackStart(cellrt2, true);
			col_descripcirugia.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
			col_descripcirugia.SortColumnId = (int) Column_pres.col_descripcirugia;
			
			TreeViewColumn col_fecha = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_fecha.Title = "Fecha Creacion de Presupuesto";
			col_fecha.PackStart(cellrt3, true);
			col_fecha.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 1 en vez de 2
			col_fecha.SortColumnId = (int) Column_pres.col_fecha;
			
			lista_cirugia.AppendColumn(col_idpresupuesto);
			lista_cirugia.AppendColumn(col_idcirugia);
			lista_cirugia.AppendColumn(col_descripcirugia);
			lista_cirugia.AppendColumn(col_fecha);
			
		}
		
		enum Column_pres
		{
			col_idpresupuesto,
			col_idcirugia,
			col_descripcirugia,
			col_fecha
		}	
		
		void on_button_llena_cirugias_clicked(object sender, EventArgs args)
		{
			llena_lista_de_busqueda_presupuesto();
			llena_lista_de_busqueda();//Cirugia
		}
		
		void llena_lista_de_busqueda_presupuesto() 
		{
			 if(tipobusqueda == "presupuesto") {
				treeViewEngineBusca.Clear();// Limpia el treeview cuando realiza una nueva busqueda
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
           		// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
	              	if ((string) entry_expresion.Text.ToUpper() == "*")	{
						comando.CommandText ="SELECT osiris_his_presupuestos_enca.id_presupuesto, "+
						                     "osiris_his_tipo_cirugias.id_tipo_cirugia,osiris_his_tipo_cirugias.descripcion_cirugia, "+
								             "to_char(osiris_his_presupuestos_enca.fecha_de_creacion_presupuesto,'dd-MM-yyyy') as fechacreacion "+
											 "FROM osiris_his_tipo_cirugias,fecha_de_creacion_presupuesto "+
											 "WHERE osiris_his_tipo_cirugias.id_tipo_cirugia = osiris_his_presupuestos_enca.id_tipo_cirugia "+
											 "ORDER BY id_tipo_cirugia;";
					}else{
						comando.CommandText ="SELECT osiris_his_presupuestos_enca.id_presupuesto, "+
						                     "osiris_his_tipo_cirugias.id_tipo_cirugia,osiris_his_tipo_cirugias.descripcion_cirugia, "+
								             "to_char(osiris_his_presupuestos_enca.fecha_de_creacion_presupuesto,'dd-MM-yyyy') as fechacreacion "+
						                     "FROM osiris_his_tipo_cirugias,osiris_his_presupuestos_enca "+
											 "WHERE descripcion_cirugia LIKE '%"+entry_expresion.Text.ToUpper()+"%' "+
											 "AND osiris_his_tipo_cirugias.id_tipo_cirugia = osiris_his_presupuestos_enca.id_tipo_cirugia "+
											 "ORDER BY id_tipo_cirugia;";
					}
					NpgsqlDataReader lector = comando.ExecuteReader ();
					while (lector.Read()){
						treeViewEngineBusca.AppendValues ((int) lector["id_presupuesto"],
						                     (int) lector["id_tipo_cirugia"],             
									         (string) lector["descripcion_cirugia"],
									         (string) lector["fechacreacion"]);
					}
				}catch (NpgsqlException ex){
	   					Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
	   						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
						msgBoxError.Run ();		msgBoxError.Destroy();
				}
				conexion.Close ();
			}
		}
			
		void paquetes_a_buscar()
		{   
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "busca_cirugias", null);
			gxml.Autoconnect (this);
			tipobusqueda ="cirugia";
			// Activa la salida de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			// Activa la seleccion de cirugia
			button_selecciona.Clicked += new EventHandler(on_selecciona_cirugia_clicked);
			// Activa boton de busqueda
			button_llena_cirugias.Clicked += new EventHandler(on_button_llena_cirugias_clicked);
	        
			treeViewEngineBusca = new TreeStore( typeof(int), typeof(string),typeof(string),typeof(string));
			lista_cirugia.Model = treeViewEngineBusca;
			
			lista_cirugia.RulesHint = true;
			
			lista_cirugia.RowActivated += on_selecciona_cirugia_clicked;  // Doble click selecciono paciente
			
			TreeViewColumn col_idcirugia = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idcirugia.Title = "ID Cirugia"; // titulo de la cabecera de la columna, si está visible
			col_idcirugia.PackStart(cellr0, true);
			col_idcirugia.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
			col_idcirugia.SortColumnId = (int) Column_cirug.col_idcirugia;
			
			TreeViewColumn col_descripcirugia = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_descripcirugia.Title = "Descripcion de Cirugia";
			col_descripcirugia.PackStart(cellrt1, true);
			col_descripcirugia.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 1 en vez de 2
			col_descripcirugia.SortColumnId = (int) Column_cirug.col_descripcirugia;
			
			TreeViewColumn col_tienepaquete = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_tienepaquete.Title = "Paquete";
			col_tienepaquete.PackStart(cellrt2, true);
			col_tienepaquete.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
			col_tienepaquete.SortColumnId = (int) Column_cirug.col_tienepaquete;
			
			TreeViewColumn col_preciobase = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_preciobase.Title = "P. Base";
			col_preciobase.PackStart(cellrt3, true);
			col_preciobase.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 1 en vez de 2
			col_preciobase.SortColumnId = (int) Column_cirug.col_preciobase;
			
			
			lista_cirugia.AppendColumn(col_idcirugia);
			lista_cirugia.AppendColumn(col_descripcirugia);
			lista_cirugia.AppendColumn(col_tienepaquete);
			lista_cirugia.AppendColumn(col_preciobase);
			
		}
		
		enum Column_cirug
		{
			col_idcirugia,
			col_descripcirugia,
			col_tienepaquete,
			col_preciobase
		}
		
		void on_selecciona_cirugia_clicked (object sender, EventArgs args)
		{
			TreeModel model;			TreeIter iterSelected;
			if (lista_cirugia.Selection.GetSelected(out model, out iterSelected)) {
				if(tipobusqueda == "cirugia")  {
					idtipocirugia = (int) model.GetValue(iterSelected, 0);
					cirugia = (string) model.GetValue(iterSelected, 1);
					entry_paq_pres.Text = cirugia;
					this.entry_id_paq_pres.Text = Convert.ToString(idtipocirugia);
					llenado_de_paquetes(idtipocirugia.ToString());
					Widget win = (Widget) sender;
					win.Toplevel.Destroy();}
				if(tipobusqueda == "presupuesto")  {
					idpresupuesto = (int) model.GetValue(iterSelected, 0);   
					idtipocirugia = (int) model.GetValue(iterSelected, 1);
                    desccirugia = (string) model.GetValue(iterSelected, 2);
	                entry_paq_pres.Text = desccirugia;
					this.entry_id_paq_pres.Text = Convert.ToString(idpresupuesto);
					llenado_de_paquetes(idtipocirugia.ToString());
					Widget win = (Widget) sender;
					win.Toplevel.Destroy();
				}
			}
		}
		
		void llena_lista_de_busqueda() 
		{
			 if(tipobusqueda == "cirugia") {
				treeViewEngineBusca.Clear();// Limpia el treeview cuando realiza una nueva busqueda
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
           		// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
	              	if ((string) entry_expresion.Text.ToUpper() == "*")	{
						comando.CommandText ="SELECT id_tipo_cirugia,descripcion_cirugia,tiene_paquete,to_char(valor_paquete,'999999999.99') AS valorpaquete "+
											"FROM osiris_his_tipo_cirugias "+
								            "WHERE tiene_paquete = 'true' "+ 
											"ORDER BY id_tipo_cirugia;";
					}else{
						comando.CommandText ="SELECT id_tipo_cirugia,descripcion_cirugia,tiene_paquete,to_char(valor_paquete,'999999999.99') AS valorpaquete FROM osiris_his_tipo_cirugias "+
											"WHERE descripcion_cirugia LIKE '%"+entry_expresion.Text.ToUpper()+"%' "+
								            "AND tiene_paquete = 'true' "+
											"ORDER BY id_tipo_cirugia;";
					}
					NpgsqlDataReader lector = comando.ExecuteReader ();
					string paquete_si = "";
					while (lector.Read()){
						if((bool) lector["tiene_paquete"]== true){
							paquete_si = "ES PAQUETE";
						}else{
							paquete_si = "";
						}
						treeViewEngineBusca.AppendValues ((int) lector["id_tipo_cirugia"],
									          (string) lector["descripcion_cirugia"],
									           paquete_si,                    
									          (string) lector["valorpaquete"]);
					}
				}catch (NpgsqlException ex){
	   					Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
	   						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
						msgBoxError.Run ();		msgBoxError.Destroy();
				}
				conexion.Close ();
			}
		}
		
		void llenado_de_paquetes(string idcirugia)
		{
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	           
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
	              	
				comando.CommandText = "SELECT descripcion_cirugia, "+
					                  "to_char(valor_paquete,'999999999.99') AS valorpaquete, "+
					                  "to_char(dias_internamiento,'99999999') AS diasinternamiento "+
						              "FROM osiris_his_tipo_cirugias "+
                                      "WHERE osiris_his_tipo_cirugias.id_tipo_cirugia = '"+(string)  idtipocirugia.ToString()+"' ;";
				                    
				//Console.WriteLine("query llenado cirugia: "+comando.CommandText.ToString());				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if(lector.Read()){
					//Console.WriteLine("---lector---");
					entry_paq_pres.Text = (string) lector["descripcion_cirugia"];
					//idpresupuesto = (int) lector["id_tipo_cirugia"];
                    entry_dia_internamiento.Text = ((string) lector["diasinternamiento"]).Trim();
	           		entry_precio_paquete.Text = ((string) lector["valorpaquete"]).Trim();
	           	}
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run (); 	msgBoxError.Destroy();
		    }
	       	conexion.Close ();
       	}
		///////////////////////GUARDAR O GRABAR RESERVACION///////////////////////////////////////////////////////////////
		void on_button_grabar_reservacin_clicked(object sender, EventArgs args)
		{
			if( this.entry_folio_servicio.Text.Trim() == "" ||  this.entry_nombre_paciente.Text.Trim() == "" || 
			    this.entry_pid_paciente.Text.Trim() == "" )
			  
			{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
											ButtonsType.Close,"Favor de llenar toda la informaciòn correspondiente");
				msgBoxError.Run ();					msgBoxError.Destroy();
			
			}else{
			  MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");
               ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
				
				if (miResultado == ResponseType.Yes){
					if (radiobutton_paquete.Active == true && radiobutton_presupuesto.Active == false){
						graba_paquetes();
					}					
				    if (radiobutton_presupuesto.Active == true && radiobutton_paquete.Active == false){
						graba_presupuestos();
					}else{
						button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
					}		
			    }
			}
		}
		
		void graba_paquetes ()
		{   
		    query_fecha_reservacion =  this.entry_anno_reservacion.Text+"-"+this.entry_mes_reservacion.Text+"-"+this.entry_dia_reservacion.Text+ " " +DateTime.Now.ToString("HH:mm:ss");
		    NpgsqlConnection conexion;
		    conexion = new NpgsqlConnection (connectionString+nombrebd );
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();						 
				comando.CommandText = "UPDATE osiris_erp_cobros_enca "+
				                       "SET reservacion = 'true',fecha_reservacion = '"+query_fecha_reservacion+"' "+
				                       "WHERE folio_de_servicio = '"+this.entry_folio_servicio.Text.Trim()+"'; "; 
				//Console.WriteLine(comando.CommandText);
				comando.ExecuteNonQuery();
				comando.Dispose();
				
				comando.CommandText = "INSERT INTO osiris_erp_reservaciones ( "+
		                      "fechahora_creacion, "+
		                      "fechahora_reservacion, "+
		                      "id_quien_reservo, "+ 
		                      "folio_de_servicio, "+
				              "pid_paciente, "+
				              "valor_paquete, "+
						      "dias_internamiento, "+
		                      "id_tipo_cirugia )"+
				              "VALUES ('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
				              "'"+query_fecha_reservacion +"', "+
				               "'"+LoginEmpleado+"', "+
				              "'"+this.entry_folio_servicio.Text.Trim()+"', "+
				              "'"+this.entry_pid_paciente.Text.Trim()+"', "+
				              "'"+this.entry_precio_paquete.Text.Trim()+"', "+
						      "'"+this.entry_dia_internamiento.Text.Trim()+"', "+
				              "'"+idtipocirugia+"');";
				                       
					comando.ExecuteNonQuery();
					comando.Dispose();
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
			                          ButtonsType.Ok,"La Reservacion se efectuo satisfactoriamente");										
					msgBox.Run ();	msgBox.Destroy();					
					this.button_grabar.Sensitive = false;
					this.button_liberar_paquete.Sensitive = true;
					this.entry_nombre_paciente.Sensitive = true;
					this.entry_pid_paciente.Sensitive = true;
					this.entry_paq_pres.Sensitive = true;
				    entry_id_paq_pres.Sensitive = true;
					this.entry_precio_paquete.Sensitive = true;
					this.entry_dia_internamiento.Sensitive = true;
					this.radiobutton_paquete.Sensitive = true;
					this.radiobutton_presupuesto.Sensitive = true;
	        	
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void graba_presupuestos ()
		{   
			query_fecha_reservacion =  this.entry_anno_reservacion.Text+"-"+this.entry_mes_reservacion.Text+"-"+this.entry_dia_reservacion.Text+ " " +DateTime.Now.ToString("HH:mm:ss");
            NpgsqlConnection conexion;			    
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			try{
				conexion.Open ();
				NpgsqlCommand comando;
				//Console.WriteLine("update"+comando.CommandText);
				comando = conexion.CreateCommand ();						 
				comando.CommandText = "UPDATE osiris_erp_cobros_enca "+
				                       "SET reservacion = 'true',fecha_reservacion = '"+query_fecha_reservacion+"' "+
				                       "WHERE folio_de_servicio = '"+this.entry_folio_servicio.Text+"'; "; 
				
				//Console.WriteLine(comando.CommandText);
				comando.ExecuteNonQuery();
				comando.Dispose();
				Console.WriteLine("i"+comando.CommandText); 
				comando.CommandText = "INSERT INTO osiris_erp_reservaciones ("+
		                      "fechahora_creacion, "+
		                      "fechahora_reservacion, "+
		                      "id_quien_reservo, "+ 	
		                      "folio_de_servicio, "+
				              "pid_paciente, "+
						      "valor_paquete, "+
						      "dias_internamiento, "+
				              "id_presupuesto )"+
				              "VALUES ('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
				              "'"+query_fecha_reservacion+"', "+
				              "'"+LoginEmpleado+"', "+
				              "'"+this.entry_folio_servicio.Text.Trim()+"', "+
				              "'"+this.entry_pid_paciente.Text.Trim()+"', "+
						      "'"+this.entry_precio_paquete.Text.Trim()+"', "+
						      "'"+this.entry_dia_internamiento.Text.Trim()+"', "+
				              "'"+idpresupuesto+"');";
				Console.WriteLine("i"+comando.CommandText);              
				comando.ExecuteNonQuery();		
				comando.Dispose();
				
				this.button_grabar.Sensitive = false;
				this.button_liberar_paquete.Sensitive = true;
				this.entry_nombre_paciente.Sensitive = true;
				this.entry_pid_paciente.Sensitive = true;
				this.entry_paq_pres.Sensitive = true;
				entry_id_paq_pres.Sensitive = true;
				this.entry_precio_paquete.Sensitive = true;
				this.entry_dia_internamiento.Sensitive = true;
				this.radiobutton_paquete.Sensitive = true;
				this.radiobutton_presupuesto.Sensitive = true;				
																
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
				                  MessageType.Info,ButtonsType.Ok,"La Reservacion se efectuo satisfactoriamente");
				msgBox.Run();msgBox.Destroy();
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																MessageType.Error, 
																ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}			
			conexion.Close ();
		}
		
		///////////////////////Liberar Paquete o Presupuesto///////////////////////////////////////////////////////////////
		void on_button_liberar_paquete (object sender, EventArgs args)
		{
		
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Desea Liberar esta infomacion?");
			ResponseType miResultado = (ResponseType) msgBox.Run();	msgBox.Destroy();
			if (miResultado == ResponseType.Yes){
				if (radiobutton_paquete.Active == true){
				  liberar_paquetes_presupuestos();
				}
						
			    if (radiobutton_presupuesto.Active == true){
					liberar_paquetes_presupuestos();
				}else{
					button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
				}
			}
		}
		
		void liberar_paquetes_presupuestos()
		{
			NpgsqlConnection conexion;			    
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();						 
				comando.CommandText = "UPDATE  osiris_erp_cobros_enca "+
				                      "SET reservacion = 'false' "+
				                      "WHERE folio_de_servicio = '"+this.entry_folio_servicio.Text.Trim()+"'; ";				                      
				comando.ExecuteNonQuery();		comando.Dispose();
						              
				comando = conexion.CreateCommand ();
			 	comando.CommandText = "UPDATE osiris_erp_reservaciones "+		                    
			             "SET id_quien_libero = '"+LoginEmpleado+"',"+ 
			             "fecha_hora_liberacion = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
						 "WHERE folio_de_servicio = '"+this.entry_folio_servicio.Text.Trim()+"' "+
						 "AND cancelado = 'false';";						
				comando.ExecuteNonQuery();		comando.Dispose();
				
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
					                          ButtonsType.Ok,"La Actualizacion se efectuo satisfactoriamente");										
				msgBox.Run ();	msgBox.Destroy();		
				conexion.Close ();
				
				this.button_liberar_paquete.Sensitive = false;
				this.button_grabar.Sensitive = true;
				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
		}
		
		///////////////////////BUTTON IMPRIMIR RESERVACION///////////////////////////////////////////////////////////////
		void on_button_imprimir_reservacion_clicked(object sender, EventArgs args)
		{  
			if( this.entry_folio_servicio.Text.Trim() == "" ||  this.entry_nombre_paciente.Text.Trim() == "" || 
			    this.entry_pid_paciente.Text.Trim() == "")/* ||  this.entry_paq_pres.Text.Trim() == "" || 
			    this.entry_precio_paquete.Text.Trim() == "" || 
			    this.entry_dia_internamiento.Text.Trim() == "") */
			  
			{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
											ButtonsType.Close,"Favor de llenar toda la informaciòn correspondiente");
				msgBoxError.Run ();					msgBoxError.Destroy();
			
			}else{ 
				//new osiris.rpt_reservacion_paquete(entry_dia1.Text,entry_mes1.Text,entry_anno1.Text,entry_nombre_paciente.Text,entry_paq_pres.Text,entry_precio_paquete.Text,this.NomEmpleado,this.AppEmpleado,this.ApmEmpleado,this.NomEmpleado,this.nombrebd);
			}
		}
			
		/////////////////////////////////////////////////////////////////////////////////////////////////////////	
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
		
		// Valida entradas que solo sean numericas, se utiliza en ventana de  carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_folio(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llena_folio();				
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace")
			{
				args.RetVal = true;
			}
		}
		
		///////////////////// CIERRA VENTANAS EMERGENTES///////////////////////////////////////////////////////
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		} 

	}
}
