//////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a   
// Monterrey - Mexico
// 
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
//				  Daniel Olivares - arcangeldoc@gmail.com (Diseño de Pantallas Glade)
// 				  
// Licencia		: GLP
// S.O. 		: GNU/LINUX
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
// Proposito	: Menu Principal
//////////////////////////////////////////////////////////	

using Gtk;
using Gdk;
using System;
using Glade;
using Npgsql;
using System.IO;

namespace osiris
{
	public class principal
	{
		//ventana de acceso al sistema
		[Widget] Gtk.Window login_osiris = null;
		[Widget] Gtk.Button button_cancelar = null;
		[Widget] Gtk.Button button_aceptar = null;
		[Widget] Gtk.Entry entry_login = null;
		[Widget] Gtk.Entry entry_password = null;
		[Widget] Gtk.ComboBox combobox_sucursal = null;
		
		[Widget] Gtk.Window menuprincipal = null;
		[Widget] Gtk.Button button_cargos_hospital = null;
		[Widget] Gtk.Button button_cargos_urgencia = null;
		[Widget] Gtk.Button button_cargos_quirofano = null;
		[Widget] Gtk.Button button_endoscopia = null;
		[Widget] Gtk.Button button_consulta_medica = null;
		[Widget] Gtk.Button button_laboratorio = null;
		[Widget] Gtk.Button button_imagenologia = null;
		[Widget] Gtk.Button button_oftalmologia = null;
		[Widget] Gtk.Button button_vision = null;
		
		[Widget] Gtk.Button button_terapia_adulto = null;
		[Widget] Gtk.Button button_terapia_nino = null;
		[Widget] Gtk.Button button_terapia_neonatal = null;
		[Widget] Gtk.Button button_nutricion = null;
		[Widget] Gtk.Button button_hemodialisis = null;
		
		[Widget] Gtk.Button button_imagenologia_b = null;
		
		// ERP
		[Widget] Gtk.Button button_registro_admision = null;
		[Widget] Gtk.Button button_caja = null;
		[Widget] Gtk.Button button_compras = null;
		[Widget] Gtk.Button button_almacen = null;
		[Widget] Gtk.Button button_costos = null;
		[Widget] Gtk.Button button_farmacia = null;
		[Widget] Gtk.Button button_recursos_humanos = null;
		[Widget] Gtk.Button button_herramientas = null;
		[Widget] Gtk.Button button_afiliados = null;
		[Widget] Gtk.Button button_proveedores = null;
		[Widget] Gtk.Button button_mantenimiento = null;
		
		// opciones generales
		[Widget] Gtk.Button button_medicos = null;
		[Widget] Gtk.Button button_ocupacion_hscmty = null;
		[Widget] Gtk.Button button_cambio_contraseña = null;
		[Widget] Gtk.Button button_agredecimientos = null;
				
		[Widget] Gtk.MenuBar menubar_osiris = null;
		[Widget] Gtk.MenuItem menuitem_hospital = null;
				
		// Salir
		[Widget] Gtk.Button button_salir  = null;
		[Widget] Gtk.Image hscmtylogo = null;
		[Widget] Gtk.Statusbar statusbar_menu = null;
		
		//cambio de contraseña
		[Widget] Gtk.Window password  = null;
		[Widget] Gtk.Button button_graba_password = null;
		[Widget] Gtk.Entry entry_nueva_contraseña1 = null;
		[Widget] Gtk.Entry entry_nueva_contraseña2 = null;
		
		// Pregunta de Admision
		//[Widget] Gtk.Window nuevo_paciente_si_no;
		//[Widget] Gtk.Button button_respuesta_no;
		//[Widget] Gtk.Button button_respuesta_si;
		//[Widget] Gtk.Button button_salir_pregunta;
		
		//ventana de medicos
		[Widget] Gtk.Entry entry_expresion  = null;
		[Widget] Gtk.Button button_buscar_busqueda  = null;
		[Widget] Gtk.Button button_selecciona = null;
		[Widget] Gtk.ComboBox combobox_tipo_busqueda = null;
		[Widget] Gtk.TreeView lista_de_medicos = null;
		
		private TreeStore treeViewEngineMedicos;
		
		// Declarando Variables
		string connectionString = "";
		string nombrebd = "";
		string LoginEmpleado = "";
    	string NomEmpleado = "";
    	string AppEmpleado = "";
    	string ApmEmpleado = "";
    	string enter_en = "";
    	
    	string accesoHIS = "";
		string accesoERP =  "";
		string accesoGENERAL = "";
		
		bool autorizaHIS = false;
		bool autorizaERP =  false;
		bool autorizaGENERAL = false;
		bool accesoabrirfolio = false;
		bool accesocatalogoprod = false;
		bool accesocxpq = false;
		bool accesocancelafolio = false;
		
		string tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";
		
		// Asignando Clases
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		protected Gtk.Window MyWinError;
		
		public static void Main (string[] args)
		{
			Application.Init();
			new principal ();
			Application.Run ();
		}

		public principal () 
		{	
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			pantalla_login();
			
			//crea_file_ods file_ods = new osiris.crea_file_ods();		
			//unoidl.com.sun.star.lang.XComponent archivo_ods = file_ods.openCalcSheet();						
			//file_ods.writeToSheet(archivo_ods);
			//file_ods.saveCalcSheet(archivo_ods);
			
			//classpublic.genera_ods();
			
			//verifica_usuariopasswd();			
		}
		
		void pantalla_login()
		{
			Glade.XML gxml = new Glade.XML (null, "osiris.glade", "login_osiris", null);
			gxml.Autoconnect (this);
			
			login_osiris.Show();
			
			llenado_de_sucursales();
			//string rfc_ = classpublic.CalcularRFC("DANIEL","OLIVARES","CUEVAS","25/06/74");
			
			//Console.WriteLine(rfc_);
												
			button_cancelar.Clicked += new EventHandler(on_button_salir_clicked);
			button_aceptar.Clicked += new EventHandler(on_aceptar_clicked);
			// activacion de la tecla Enter en los entry
			entry_login.KeyPressEvent += onKeyPressEvent_enter;
			entry_password.KeyPressEvent += onKeyPressEvent_enter;
		}
		
		void llenado_de_sucursales()
		{
			combobox_sucursal.Clear();
			
			CellRendererText cell3 = new CellRendererText();
			combobox_sucursal.PackStart(cell3, true);
			combobox_sucursal.AddAttribute(cell3,"text",0);
	        
			ListStore store5 = new ListStore( typeof (string), typeof (int));
			combobox_sucursal.Model = store5;
						
	        NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_erp_sucursales "+
               						"WHERE activo = 'true' "+	
               						"ORDER BY descripcion_sucursal ;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read())				{
					store5.AppendValues ((string) lector["descripcion_sucursal"],
									 	(int) lector["id_sucursal"] );
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
	        
			TreeIter iter5;
			if (store5.GetIterFirst(out iter5)){
				combobox_sucursal.SetActiveIter (iter5);
			}
			combobox_sucursal.Changed += new EventHandler (onComboBoxChanged_sucursal);
		}
		
		void onComboBoxChanged_sucursal (object sender, EventArgs args)
		{
			ComboBox combobox_sucursal = sender as ComboBox;
			if (sender == null) {return;}
			TreeIter iter;
			if (combobox_sucursal.GetActiveIter (out iter)){ 
				//idformadepago = (int) combobox_sucursal.Model.GetValue(iter,1);
			}
		}
		
		void on_aceptar_clicked(object o, EventArgs args)
    	{
			verifica_usuariopasswd();
			//pantalla_principal();
		}
		
		void verifica_usuariopasswd()
		{
			NpgsqlConnection conexion;
            conexion = new NpgsqlConnection(connectionString+nombrebd);
            // Verifica que la base de datos este conectada
            try{
				conexion.Open();
				NpgsqlCommand comando;
				comando = conexion.CreateCommand();             
				comando.CommandText = "SELECT *,osiris_empleado.id_empleado,login_empleado,nombre1_empleado,nombre2_empleado, "+
               						 "apellido_paterno_empleado,apellido_materno_empleado,departamento,puesto,"+
               						 "password_empleado AS passwordempleado,acceso_his,acceso_erp,acceso_general,autoriza_his,autoriza_erp,autoriza_general,"+
               						 "acceso_osiris,acceso_abrir_folio "+	
                                     "FROM osiris_empleado,osiris_empleado_detalle "+ 
                                     "WHERE osiris_empleado.id_empleado = osiris_empleado_detalle.id_empleado "+
                                     "AND baja_empleado = 'false' "+
                                     "AND acceso_osiris = 'true' "+
                                     "AND login_empleado = '"+entry_login.Text.Trim().ToUpper()+"';";
                                  
            	//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();         		
				
            	if ((bool)lector.Read()){
					string entrypassword ="";
					osiris.class_public newpasswd = new osiris.class_public();
					entrypassword = newpasswd.CreatePasswordMD5(this.entry_password.Text.Trim());
					Console.WriteLine("MD5 Passwd "+entrypassword);
					Console.WriteLine("SHA1 Passwd "+(string) newpasswd.CreatePasswordSHA1(this.entry_password.Text.Trim()));
					
					if (entrypassword == (string) lector["passwordempleado"]){
												
						LoginEmpleado = (string) lector["login_empleado"];
						NomEmpleado = (string) lector["nombre1_empleado"]+" "+(string) lector["nombre2_empleado"];
						AppEmpleado = (string) lector["apellido_paterno_empleado"];
						ApmEmpleado = (string) lector["apellido_materno_empleado"];
						string idempleado = (string) lector["id_empleado"];
						
						accesoHIS = (string) lector["acceso_his"];
						accesoERP = (string) lector["acceso_erp"];
						accesoGENERAL = (string) lector["acceso_general"];
						
						autorizaHIS = (bool) lector["autoriza_his"];
						autorizaERP =  (bool) lector["autoriza_erp"];
						autorizaGENERAL = (bool) lector["autoriza_general"];
						
						accesoabrirfolio = (bool) lector["acceso_abrir_folio"];
						accesocatalogoprod = (bool) lector["acceso_catalogo_producto"];
						accesocxpq = (bool) lector["acceso_cx_pq"];						
						accesocancelafolio = (bool) lector["acceso_cancelar_folio"];						
						//nombre_empresa = conexion_a_DB.nombre_empresa;
						//direccion_empresa = conexion_a_DB.direccion_empresa;
						//telefonofax_empresa = conexion_a_DB.telefonofax_empresa;
						//version_sistema = conexion_a_DB.version_sistema;					
						
						login_osiris.Destroy();
						
						// Guarda los logs de Accesos de los usuarios
						comando.CommandText = "INSERT INTO osiris_empleados_accesos( "+
			 									"login_empleado, "+
												"id_empleado,"+
			 									"fechahora_login "+
			 									") VALUES ('"+
												LoginEmpleado+"','"+
												idempleado+"','"+
 												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+ "');";
 						//Console.WriteLine(comando.CommandText.ToString());						
		 				comando.ExecuteNonQuery();
						pantalla_principal();
					}else{
						MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Error,ButtonsType.Close,"Usuario o Contraseña INCORRECTA \n"+
									"vuelva a intentarlo");
						msgBox.Run ();				msgBox.Destroy();
					}
				}else{
					MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Error,ButtonsType.Close,"Usuario o Contraseña INCORRECTA \n"+
										"vuelva a intentarlo");
					msgBox.Run ();				msgBox.Destroy();
				}
			}catch (NpgsqlException ex){ 
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,MessageType.Error,
									ButtonsType.Ok,"PostgresSQL error: {0}",ex.Message);
				msgBox.Run();				msgBox.Destroy();
				return; 
            }        	
    	}
		
		void pantalla_principal()
		{
			Glade.XML gxml = new Glade.XML (null, "osiris.glade", "menuprincipal", null);
			gxml.Autoconnect (this);				 			
			//hscmtylogo.Pixbuf = new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,"OSIRISLogo.jpg"));   //en windows
			hscmtylogo.Pixbuf = new Gdk.Pixbuf("/opt/osiris/bin/OSIRISLogo.jpg");   // en Linux
			menuprincipal.Show();
					 					 											
			verificapermisos(accesoHIS,accesoERP,accesoGENERAL,autorizaHIS,autorizaERP,autorizaGENERAL);
			// llamando a los eventos
			button_registro_admision.Clicked += new EventHandler(on_button_registro_admision_clicked );
			button_compras.Clicked += new EventHandler(on_button_compras_clicked);
			button_almacen.Clicked += new EventHandler(on_button_almacen_clicked);
			button_recursos_humanos.Clicked += new EventHandler(on_button_recursos_humanos_clicked);
			button_caja.Clicked += new EventHandler(on_button_caja_clicked );
			button_costos.Clicked += new EventHandler(on_button_costos_clicked);
			
			button_agredecimientos.Clicked += new EventHandler(on_button_agredecimientos_clicked);
			button_ocupacion_hscmty.Clicked += new EventHandler(on_button_ocupacion_osiris_clicked);
			
			button_cargos_hospital.Clicked += new EventHandler( on_button_cargos_hospital_clicked );
			button_cargos_urgencia.Clicked += new EventHandler(on_button_cargos_urgencia_clicked );
			button_cargos_quirofano.Clicked += new EventHandler( on_button_cargos_quirofano_clicked);
			button_endoscopia.Clicked += new EventHandler(on_button_endoscopia_clicked);
			button_consulta_medica.Clicked += new EventHandler(on_button_consulta_medica_clicked);
			button_laboratorio.Clicked += new EventHandler( on_button_laboratorio_clicked );
			button_imagenologia.Clicked += new EventHandler( on_button_imagenologia_clicked );
			button_hemodialisis.Clicked += new EventHandler(on_button_hemodialisis_clicked);
			button_vision.Clicked += new EventHandler(on_button_vision_clicked);
			
			button_medicos.Clicked += new EventHandler(on_button_medicos_clicked);					 			
			button_cambio_contraseña.Clicked += new EventHandler(on_button_cambio_contraseña_clicked);					 							 			
			button_herramientas.Clicked += new EventHandler(on_button_herramientas_clicked);						
			button_farmacia.Clicked += new EventHandler(on_button_farmacia_clicked);			
			 			
			//button_nutricion.Clicked += new EventHandler(on_button_nutricion_clicked);
			//button_hemodialisis.Clicked += new EventHandler(on_button_hemodialisis_clicked);
			//button_imagenologia_b.Clicked += new EventHandler( on_button_imagenologia_b_clicked );
			//button_terapia_adulto.Clicked += new EventHandler( on_button_terapia_adulto_clicked );
			//button_terapia_nino.Clicked += new EventHandler( on_button_terapia_pediatrica_clicked );
			//button_terapia_neonatal.Clicked += new EventHandler(on_button_terapia_neonatal_clicked);					
			button_salir.Clicked += new EventHandler(on_button_salir_clicked);
			
			//button_almacen.Hide();
			//button_compras.Hide();
			//button_farmacia.Hide();
			button_nutricion.Hide();
			button_afiliados.Hide();
			//button_proveedores.Hide();
			//button_oftalmologia.Hide();
			//button_hemodialisis.Hide();
			button_terapia_adulto.Hide();
			button_terapia_nino.Hide();
			button_terapia_neonatal.Hide();
			//button_ginecologia.Hide();
			//button_endoscopia.Hide();
			button_oftalmologia.Hide();
			
			//menuitem_hospital.Sensitive = false;
			//new osiris.class_crea_ods();
				
			// Actulizando statusbar
			statusbar_menu.Pop(0);
			statusbar_menu.Push(1, "login:"+(string)LoginEmpleado+"|Usuario:"+(string)NomEmpleado+" "+(string)AppEmpleado+" "+(string)ApmEmpleado);
			statusbar_menu.HasResizeGrip = false;
		}
		
		// Registro y Admision de Pacientes, realiza la pregunta
		void on_button_registro_admision_clicked (object sender, EventArgs a)
		{	
			new osiris.admision (LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,accesocxpq,accesocancelafolio); 
		}
		
		// Ocupacion Hospitalaria
		void on_button_ocupacion_osiris_clicked (object sender, EventArgs a)
		{
			new osiris.reporte_pacientes_sin_alta(this.nombrebd);
		}
		
		// llamada de pantalla para cargos de enfermeria
		// cargos_hospitalizacion.cs
		void on_button_cargos_hospital_clicked (object sender, EventArgs args)
		{
			new osiris.hospitalizacion(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);   	
		}
		
		void on_button_cargos_quirofano_clicked (object sender, EventArgs args)
		{
			new osiris.quirofano(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);   	
		}
	
		void on_button_cargos_urgencia_clicked (object sender, EventArgs args)
		{
			new osiris.urgencia(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_endoscopia_clicked (object sender, EventArgs args)
		{
			new osiris.endoscopia(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_laboratorio_clicked (object sender, EventArgs args)
		{
			new osiris.laboratorio (LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_hemodialisis_clicked (object sender, EventArgs args)
		{
			new osiris.hemodialisis (LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		///////////////////
		void on_button_imagenologia_clicked (object sender, EventArgs args)
		{
			new osiris.imagenologia(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_terapia_adulto_clicked (object sender, EventArgs args)
		{
			new osiris.terapia_adulto(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_terapia_pediatrica_clicked (object sender, EventArgs args)
		{
			new osiris.terapia_pediatrica(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_terapia_neonatal_clicked (object sender, EventArgs args)
		{
			new osiris.terapia_neonatal(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_consulta_medica_clicked(object sender, EventArgs args)
		{
			new osiris.consulta_medica(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_oftalmologia_clicked(object sender, EventArgs args)
		{
			new osiris.oftalmologia(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_vision_clicked(object sender, EventArgs args)
		{
			new osiris.vision(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_almacen_clicked(object sender, EventArgs args)
		{
			// almacen.cs
			new osiris.almacen(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		} 
		
		// llamada de modulo de caja caja.cs	
		void on_button_caja_clicked (object sender, EventArgs args)
		{
			new osiris.tesoreria(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,accesoabrirfolio);
		}
		
		void on_button_costos_clicked (object sender, EventArgs args)
		{	
			// new osiris.nuevos_prod(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
			new osiris.costos_consultas(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,accesocatalogoprod);
		}
		
		void on_button_herramientas_clicked (object sender, EventArgs args)
		{
			//if( LoginEmpleado == "DOLIVARES" || LoginEmpleado == "JPENA" || LoginEmpleado == "HVARGAS" ){ 
			new osiris.herramientas_del_sistemas(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);//}
		}
		
		void on_button_recursos_humanos_clicked(object sender, EventArgs args)
		{
			new osiris.recursoshumanos(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);//}
		}
		
		void on_button_compras_clicked(object sender, EventArgs args)
		{
			new osiris.compras_consultas(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_farmacia_clicked(object sender, EventArgs args)
		{
			// autorizacion_doctores_compra.cs
			new osiris.orden_compra_urgencias(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,0,"",1,"Enlasalo a un id de tu proveedor de farmacia local");
		}
		
		void on_button_nutricion_clicked(object sender, EventArgs args)
		{
			new osiris.nutricion(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_agredecimientos_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "osiris.glade", "agradece", null);
			gxml.Autoconnect (this);
			
			button_aceptar.Clicked += new EventHandler(on_cierraventanas_clicked);
		}
		
		void on_button_cambio_contraseña_clicked (object sender, EventArgs args)
		{
			enter_en = "contraseña";
			Glade.XML gxml = new Glade.XML (null, "osiris.glade", "password", null);
			gxml.Autoconnect (this);
			password.Show();
			// llamando a los eventos
			this.entry_nueva_contraseña2.KeyPressEvent += onKeyPressEvent_enter_pass;
			this.button_graba_password.Clicked += new EventHandler (on_button_graba_password_clicked);
			this.button_salir.Clicked += new EventHandler (on_cierraventanas_clicked);
		}
		
		void on_button_salir_clicked (object sender, EventArgs a)
		{
			Application.Quit ();
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs a)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
						 
		}
		
		// Connect the Signals defined in Glade
		void OnWindowDeleteEvent (object sender, DeleteEventArgs a) 
		{
			Application.Quit ();
			a.RetVal = true;
		}
		
		public void verificapermisos (string accesoHIS,string accesoERP,string accesoGENERAL,bool autorizaHIS,bool autorizaERP,bool autorizaGENERAL)
        {        	
			// HIS
			button_cargos_hospital.Sensitive = false;
			button_cargos_urgencia.Sensitive = false;
			button_cargos_quirofano.Sensitive = false;
			button_endoscopia.Sensitive = false;
			button_terapia_adulto.Sensitive = false;
			button_terapia_nino.Sensitive = false;
			button_terapia_neonatal.Sensitive = false;
			button_consulta_medica.Sensitive = false;
			button_laboratorio.Sensitive = false;
			button_imagenologia.Sensitive = false;
			//button_oftalmologia.Sensitive = false;
			
			// ERP
			button_registro_admision.Sensitive = false;
			button_caja.Sensitive = false;
			button_compras.Sensitive = false;
			button_almacen.Sensitive = false;
			button_costos.Sensitive = false;
			button_farmacia.Sensitive = false;
			button_recursos_humanos.Sensitive = false;
			button_nutricion.Sensitive = false;
			button_herramientas.Sensitive = false;
			button_proveedores.Sensitive = true;
			
			// opciones generales
			button_medicos.Sensitive = false;
			button_ocupacion_hscmty.Sensitive = false;
			button_cambio_contraseña.Sensitive = false;
			button_agredecimientos.Sensitive = false;
			
			if((bool) autorizaHIS == true){
				if ((string) accesoHIS.Substring(0,1) == "1"){
       				button_cargos_hospital.Sensitive = true;
        		}        		
        		if ((string) accesoHIS.Substring(1,1) == "1"){
        			button_cargos_urgencia.Sensitive = true;
        		}
        		if ((string) accesoHIS.Substring(2,1) == "1"){
					button_cargos_quirofano.Sensitive = true;
				}
				if ((string) accesoHIS.Substring(3,1) == "1"){
					button_endoscopia.Sensitive = true;
				}
				if ((string) accesoHIS.Substring(7,1) == "1"){
					button_consulta_medica.Sensitive = true;
				}
				if ((string) accesoHIS.Substring(8,1) == "1"){
					button_laboratorio.Sensitive = true;
				}
				if ((string) accesoHIS.Substring(9,1) == "1"){
					button_imagenologia.Sensitive = true;
				}				
				if ((string) accesoHIS.Substring(10,1) == "1"){
					//button_oftalmologia.Sensitive = false;
				}
				if ((string) accesoHIS.Substring(11,1) == "1"){
					button_vision.Sensitive = true;
				}
				
				/*
				if ((string) accesoHIS.Substring(7,1) == "1"){
					button_ginecologia.Sensitive = true;
				}
				if ((string) accesoHIS.Substring(4,1) == "1"){
					button_terapia_adulto.Sensitive = true;
				}
				if ((string) accesoHIS.Substring(5,1) == "1"){
					button_terapia_nino.Sensitive = true;
				}
				if ((string) accesoHIS.Substring(6,1) == "1"){
					button_terapia_neonatal.Sensitive = true;
				}*/
			}
			if((bool) autorizaERP == true){
				if ((string) accesoERP.Substring(0,1) == "1"){
					button_registro_admision.Sensitive = true;
				}
				if ((string) accesoERP.Substring(1,1) == "1"){
					button_caja.Sensitive = true;
				}
				if ((string) accesoERP.Substring(2,1) == "1"){
					button_compras.Sensitive = true;
				}
				if ((string) accesoERP.Substring(3,1) == "1"){
					button_almacen.Sensitive = true;
				}
				if ((string) accesoERP.Substring(4,1) == "1"){
					button_costos.Sensitive = true;
				}
				if ((string) accesoERP.Substring(5,1) == "1"){
					button_farmacia.Sensitive = true;
				}
				if ((string) accesoERP.Substring(6,1) == "1"){
					button_recursos_humanos.Sensitive = true;
				}
				if ((string) accesoERP.Substring(7,1) == "1"){
					button_nutricion.Sensitive = true;
				}
				if ((string) accesoERP.Substring(8,1) == "1"){
					button_herramientas.Sensitive = true;
				}				
			}
			if((bool) autorizaGENERAL == true){
				if ((string) accesoGENERAL.Substring(0,1) == "1"){
					button_medicos.Sensitive = true;
				}
				if ((string) accesoGENERAL.Substring(1,1) == "1"){	
					button_ocupacion_hscmty.Sensitive = true;
				}
				if ((string) accesoGENERAL.Substring(2,1) == "1"){
					button_cambio_contraseña.Sensitive = true;
				}
				if ((string) accesoGENERAL.Substring(3,1) == "1"){
					button_agredecimientos.Sensitive = true;
				}
			}			
        }
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key.ToString());
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;				
				 verifica_usuariopasswd();
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		void onKeyPressEvent_enter_pass(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				if(enter_en == "contraseña") {cambiando_contraseña(o);}
				if(enter_en == "medicos") { llenando_lista_de_medicos(); }			
			}
		}
		
		void on_button_graba_password_clicked (object sender, EventArgs args)
		{
			cambiando_contraseña(sender);
		}
		
		void cambiando_contraseña(object sender)
		{
			string nuevopassword;
			if(entry_nueva_contraseña1.Text == entry_nueva_contraseña2.Text){
				
				// encriptando contraseñas a MD5 funcion de mono
				osiris.class_public newpasswd = new osiris.class_public();
				nuevopassword = newpasswd.CreatePasswordMD5(entry_nueva_contraseña2.Text.Trim());
				
				//Console.WriteLine(nuevopassword);
				NpgsqlConnection conexion; 
            	conexion = new NpgsqlConnection (connectionString+nombrebd);
            	// Verifica que la base de datos este conectada
            	try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand (); 
             		//Console.WriteLine("se cambio la contraseña");
					comando.CommandText = "UPDATE osiris_empleado "+
               						 "SET password_empleado = '"+nuevopassword+"' "+
                                     "WHERE login_empleado = '"+LoginEmpleado.ToUpper()+"';";
                    comando.ExecuteNonQuery();                   comando.Dispose();
            		//Console.WriteLine(comando.CommandText);
            	}catch(NpgsqlException ex){ 
					Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				}
				MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
								MessageType.Error,ButtonsType.Ok,"La contaseña se cambio con ''EXITO''");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
				if(miResultado == ResponseType.Ok){
					Widget win = (Widget) sender;	win.Toplevel.Destroy();
				}
			}else{
				MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
								MessageType.Error,ButtonsType.Ok,"Los campos no coinciden, vuelva a escribir las contraseñas");
				msgBox.Run ();				msgBox.Destroy();
				this.entry_nueva_contraseña1.Text = ""; this.entry_nueva_contraseña2.Text = "";
			}
		}
		
		void on_button_medicos_clicked (object sender, EventArgs args)
		{
			enter_en = "medicos";
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador_medicos", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        llenado_cmbox_tipo_busqueda();
	        entry_expresion.KeyPressEvent += onKeyPressEvent_enter_pass;
			button_buscar_busqueda.Clicked += new EventHandler(on_button_llena_medicos_clicked);
			button_selecciona.Sensitive = false;
	        button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
			
			treeViewEngineMedicos = new TreeStore(typeof(int),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(bool),typeof(bool),typeof(bool),typeof(bool),
												typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),
												typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool));
			lista_de_medicos.Model = treeViewEngineMedicos;
			lista_de_medicos.RulesHint = true;
		
			TreeViewColumn col_idmedico = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idmedico.Title = "ID Medico"; // titulo de la cabecera de la columna, si está visible
			col_idmedico.PackStart(cellr0, true);
			col_idmedico.AddAttribute (cellr0, "text", 0);
			col_idmedico.SortColumnId = (int) Coldatos_medicos.col_idmedico;    
            
			TreeViewColumn col_nomb1medico = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_nomb1medico.Title = "1º Nombre";
			col_nomb1medico.PackStart(cellrt1, true);
			col_nomb1medico.AddAttribute (cellrt1, "text", 1);
			col_nomb1medico.SortColumnId = (int) Coldatos_medicos.col_nomb1medico; 
            
            TreeViewColumn col_nomb2medico = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_nomb2medico.Title = "2º Nombre";
			col_nomb2medico.PackStart(cellrt2, true);
			col_nomb2medico.AddAttribute (cellrt2, "text", 2);
			col_nomb2medico.SortColumnId = (int) Coldatos_medicos.col_nomb2medico; 
			
			TreeViewColumn col_appmedico = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_appmedico.Title = "Apellido Paterno";
			col_appmedico.PackStart(cellrt3, true);
			col_appmedico.AddAttribute (cellrt3, "text", 3);
			col_appmedico.SortColumnId = (int) Coldatos_medicos.col_appmedico;
			
			TreeViewColumn col_apmmedico = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_apmmedico.Title = "Apellido Materno";
			col_apmmedico.PackStart(cellrt4, true);
			col_apmmedico.AddAttribute (cellrt4, "text", 4);
			col_apmmedico.SortColumnId = (int) Coldatos_medicos.col_apmmedico;
            
			TreeViewColumn col_espemedico = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_espemedico.Title = "Especialidad";
			col_espemedico.PackStart(cellrt5, true);
			col_espemedico.AddAttribute (cellrt5, "text", 5);
			col_espemedico.SortColumnId = (int) Coldatos_medicos.col_espemedico;
            
			TreeViewColumn col_telmedico = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_telmedico.Title = "Cedula Medica";
			col_telmedico.PackStart(cellrt6, true);
			col_telmedico.AddAttribute (cellrt6, "text", 6); 
			col_telmedico.SortColumnId = (int) Coldatos_medicos.col_telmedico;
            
			TreeViewColumn col_cedulamedico = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_cedulamedico.Title = "Telefono Casa";
			col_cedulamedico.PackStart(cellrt7, true);
			col_cedulamedico.AddAttribute (cellrt7, "text", 7); 
			col_cedulamedico.SortColumnId = (int) Coldatos_medicos.col_cedulamedico;
			
			TreeViewColumn col_telOfmedico = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_telOfmedico.Title = "Telefono Oficina";
			col_telOfmedico.PackStart(cellrt8, true);
			col_telOfmedico.AddAttribute (cellrt8, "text", 8);
			col_telOfmedico.SortColumnId = (int) Coldatos_medicos.col_telOfmedico; 
			
			TreeViewColumn col_celmedico = new TreeViewColumn();
			CellRendererText cellrt9 = new CellRendererText();
			col_celmedico.Title = "Celular 1";
			col_celmedico.PackStart(cellrt9, true);
			col_celmedico.AddAttribute (cellrt9, "text", 9); 
			col_celmedico.SortColumnId = (int) Coldatos_medicos.col_celmedico;
			
			TreeViewColumn col_celmedico2 = new TreeViewColumn();
			CellRendererText cellrt10 = new CellRendererText();
			col_celmedico2.Title = "Celular 2";
			col_celmedico2.PackStart(cellrt10, true);
			col_celmedico2.AddAttribute (cellrt10, "text", 10);
			col_celmedico2.SortColumnId = (int) Coldatos_medicos.col_celmedico2;
									
			TreeViewColumn col_nextelmedico = new TreeViewColumn();
			CellRendererText cellrt11 = new CellRendererText();
			col_nextelmedico.Title = "Nextel";
			col_nextelmedico.PackStart(cellrt11, true);
			col_nextelmedico.AddAttribute (cellrt11, "text", 11);
			col_nextelmedico.SortColumnId = (int) Coldatos_medicos.col_nextelmedico;
			
			TreeViewColumn col_beepermedico = new TreeViewColumn();
			CellRendererText cellrt12 = new CellRendererText();
			col_beepermedico.Title = "Beeper";
			col_beepermedico.PackStart(cellrt12, true);
			col_beepermedico.AddAttribute (cellrt12, "text", 12);
			col_beepermedico.SortColumnId = (int) Coldatos_medicos.col_beepermedico;
			
			TreeViewColumn col_empresamedico = new TreeViewColumn();
			CellRendererText cellrt13 = new CellRendererText();
			col_empresamedico.Title = "Empresa";
			col_empresamedico.PackStart(cellrt13, true);
			col_empresamedico.AddAttribute (cellrt13, "text", 13);
			col_empresamedico.SortColumnId = (int) Coldatos_medicos.col_empresamedico;
			                        
			lista_de_medicos.AppendColumn(col_idmedico);
			lista_de_medicos.AppendColumn(col_nomb1medico);
			lista_de_medicos.AppendColumn(col_nomb2medico);
			lista_de_medicos.AppendColumn(col_appmedico);
			lista_de_medicos.AppendColumn(col_apmmedico);
			lista_de_medicos.AppendColumn(col_espemedico);
			lista_de_medicos.AppendColumn(col_cedulamedico);
			lista_de_medicos.AppendColumn(col_telmedico);
			lista_de_medicos.AppendColumn(col_telOfmedico);
			lista_de_medicos.AppendColumn(col_celmedico);
			lista_de_medicos.AppendColumn(col_celmedico2);
			lista_de_medicos.AppendColumn(col_nextelmedico);
			lista_de_medicos.AppendColumn(col_beepermedico);
			lista_de_medicos.AppendColumn(col_empresamedico);
		}
		
		enum Coldatos_medicos
		{
			col_idmedico,
			col_nomb1medico,
			col_nomb2medico,
			col_appmedico,
			col_apmmedico,
			col_espemedico,
			col_cedulamedico,
			col_telmedico,
			col_telOfmedico,
			col_celmedico,
			col_celmedico2,
			col_nextelmedico,
			col_beepermedico,
			col_empresamedico
		}
		
		void on_button_llena_medicos_clicked (object sender, EventArgs args)
		{
			llenando_lista_de_medicos();
		}
		void llenando_lista_de_medicos() 
		{
			TreeIter iter;
			if (combobox_tipo_busqueda.GetActiveIter(out iter))
			{
				if((int) combobox_tipo_busqueda.Model.GetValue(iter,1) > 0) {
					treeViewEngineMedicos.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
		            // Verifica que la base de datos este conectada
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						if ((string) entry_expresion.Text.ToUpper().Trim() == "")
						{
							comando.CommandText = "SELECT id_medico, "+
										"to_char(id_empresa,'999999') AS idempresa, "+
										"to_char(osiris_his_tipo_especialidad.id_especialidad,'999999') AS idespecialidad, "+
										"nombre_medico,descripcion_empresa,descripcion_especialidad,centro_medico, "+
										"nombre1_medico,nombre2_medico,apellido_paterno_medico,apellido_materno_medico, "+
										"telefono1_medico,cedula_medico,telefono2_medico,celular1_medico,celular2_medico, "+
										"nextel_medico,beeper_medico,cedula_medico,direccion_medico,direccion_consultorio_medico, "+
										"to_char(fecha_ingreso_medico,'dd-mm-yyyy') AS fecha_ingreso, "+
										"to_char(fecha_revision_medico,'dd-mm-yyyy') AS fecha_revision, "+
										"titulo_profesional_medico,cedula_profecional_medico,diploma_especialidad_medico, "+
										"diploma_subespecialidad_medico,copia_identificacion_oficial_medico,copia_cedula_rfc_medico, "+
										"diploma_cursos_adiestramiento_medico,certificacion_recertificacion_consejo_subespecialidad_medico, "+
										"copia_comprobante_domicilio_medico,diploma_seminarios_medico,diploma_cursos_medico, "+
										"diplomas_extranjeros_medico,constancia_congresos_medico,cedula_especialidad_medico, "+
										"medico_activo,autorizado "+
										"FROM osiris_his_medicos,osiris_his_tipo_especialidad,osiris_empresas "+
										"WHERE osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad "+
										"AND osiris_his_medicos.id_empresa_medico = osiris_empresas.id_empresa "+
										"AND medico_activo = 'true' "+
										"ORDER BY id_medico;";
						}else{
							comando.CommandText = "SELECT id_medico, "+
										"to_char(id_empresa,'999999') AS idempresa, "+
										"to_char(osiris_his_tipo_especialidad.id_especialidad,'999999') AS idespecialidad, "+
										"nombre_medico,descripcion_empresa,descripcion_especialidad,centro_medico, "+
										"nombre1_medico,nombre2_medico,apellido_paterno_medico,apellido_materno_medico, "+
										"telefono1_medico,cedula_medico,telefono2_medico,celular1_medico,celular2_medico, "+
										"nextel_medico,beeper_medico,cedula_medico,direccion_medico,direccion_consultorio_medico, "+
										"to_char(fecha_ingreso_medico,'dd-mm-yyyy') AS fecha_ingreso, "+
										"to_char(fecha_revision_medico,'dd-mm-yyyy') AS fecha_revision, "+
										"titulo_profesional_medico,cedula_profecional_medico,diploma_especialidad_medico, "+
										"diploma_subespecialidad_medico,copia_identificacion_oficial_medico,copia_cedula_rfc_medico, "+
										"diploma_cursos_adiestramiento_medico,certificacion_recertificacion_consejo_subespecialidad_medico, "+
										"copia_comprobante_domicilio_medico,diploma_seminarios_medico,diploma_cursos_medico, "+
										"diplomas_extranjeros_medico,constancia_congresos_medico,cedula_especialidad_medico, "+
										"medico_activo,autorizado "+
										"FROM osiris_his_medicos,osiris_his_tipo_especialidad,osiris_empresas "+
										"WHERE osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad "+
										"AND osiris_his_medicos.id_empresa_medico = osiris_empresas.id_empresa "+
										"AND medico_activo = 'true' "+
								  		tipobusqueda+(string) entry_expresion.Text.Trim().ToUpper()+"%' "+
										"ORDER BY id_medico;";
						}
						NpgsqlDataReader lector = comando.ExecuteReader ();
						Console.WriteLine(comando.CommandText);
						while (lector.Read())
						{
							treeViewEngineMedicos.AppendValues ((int) lector["id_medico"],//0
										(string) lector["nombre1_medico"],//1
										(string) lector["nombre2_medico"],//2
										(string) lector["apellido_paterno_medico"],//3
										(string) lector["apellido_materno_medico"],//4
										(string) lector["descripcion_especialidad"],//5
										(string) lector["cedula_medico"],//6
										(string) lector["telefono1_medico"],//7
										(string) lector["telefono2_medico"],//8
										(string) lector["celular1_medico"],//9
										(string) lector["celular2_medico"],//10
										(string) lector["nextel_medico"],//11
										(string) lector["beeper_medico"],//12
										(string) lector["descripcion_empresa"],//13
										(string) lector["idespecialidad"],//14
										(string) lector["idempresa"],//15
										(string) lector["fecha_ingreso"],//16
										(string) lector["fecha_revision"],//17
										(string) lector["direccion_medico"],//18
										(string) lector["direccion_consultorio_medico"],//19
										(bool) lector["titulo_profesional_medico"],//20
										(bool) lector["cedula_profecional_medico"],//21
										(bool) lector["diploma_especialidad_medico"], //22
										(bool) lector["diploma_subespecialidad_medico"],//23
										(bool) lector["copia_identificacion_oficial_medico"],//24
										(bool) lector["copia_cedula_rfc_medico"], //25
										(bool) lector["diploma_cursos_adiestramiento_medico"],//26
										(bool) lector["certificacion_recertificacion_consejo_subespecialidad_medico"],//27
										(bool) lector["copia_comprobante_domicilio_medico"],//28
										(bool) lector["diploma_seminarios_medico"],//29
										(bool) lector["diploma_cursos_medico"],//30
										(bool) lector["diplomas_extranjeros_medico"],//31
										(bool) lector["constancia_congresos_medico"],//32
										(bool) lector["cedula_especialidad_medico"],//33
										(bool) lector["medico_activo"],//34
										(bool) lector["centro_medico"],//35
										(bool) lector["autorizado"]//36
										);
						}
					}catch (NpgsqlException ex){
			   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error,ButtonsType.Close,"Directirio medico: PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();			msgBoxError.Destroy();
					}
					conexion.Close ();
				}else{	
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Info,ButtonsType.Close, " selecione un tipo de busqueda ");
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
			}
		}
		
		void llenado_cmbox_tipo_busqueda()
		{
			combobox_tipo_busqueda.Clear();
			CellRendererText cell1 = new CellRendererText();
			combobox_tipo_busqueda.PackStart(cell1, true);
			combobox_tipo_busqueda.AddAttribute(cell1,"text",0);
	        
			ListStore store1 = new ListStore( typeof (string),typeof (int));
			combobox_tipo_busqueda.Model = store1;
	        
			//store1.AppendValues ("",0);
			store1.AppendValues ("PRIMER NOMBRE",1);
			store1.AppendValues ("SEGUNDO NOMBRE",2);
			store1.AppendValues ("APELLIDO PATERNO",3);
			store1.AppendValues ("APELLIDO MATERNO",4);
			store1.AppendValues ("CEDULA MEDICA",5);
			store1.AppendValues ("ESPECIALIDAD",6);
				              
			TreeIter iter1;
			if (store1.GetIterFirst(out iter1)){
				combobox_tipo_busqueda.SetActiveIter (iter1);
			}
			combobox_tipo_busqueda.Changed += new EventHandler (onComboBoxChanged_tipo_busqueda);
		}
		
		void onComboBoxChanged_tipo_busqueda (object sender, EventArgs args)
		{
	    	ComboBox combobox_tipo_busqueda = sender as ComboBox;
			if (sender == null)	{	return;	}
			TreeIter iter;			int numbusqueda = 0;
			if (combobox_tipo_busqueda.GetActiveIter (out iter))
			{
				numbusqueda = (int) combobox_tipo_busqueda.Model.GetValue(iter,1);
				tipo_de_busqueda_de_medico(numbusqueda);
				llenando_lista_de_medicos();
			}
		}
		
		void tipo_de_busqueda_de_medico(int numbusqueda)
		{
			if(numbusqueda == 1)  { tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";	}//Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 2)  { tipobusqueda = "AND osiris_his_medicos.nombre2_medico LIKE '";	}//Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 3)  { tipobusqueda = "AND osiris_his_medicos.apellido_paterno_medico LIKE '";	}//Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 4)  { tipobusqueda = "AND osiris_his_medicos.apellido_materno_medico LIKE '";	}//Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 5)  { tipobusqueda = "AND osiris_his_medicos.cedula_medico LIKE '";	}//Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 6)  { tipobusqueda = "AND osiris_his_tipo_especialidad.descripcion_especialidad LIKE '";	}//Console.WriteLine(tipobusqueda); }
		}
	}	
}