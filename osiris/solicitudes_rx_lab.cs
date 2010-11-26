// created on 16/08/2010 at 06:10 p
//////////////////////////////////////////////////////////////////////
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. (Programacion) arcangeldoc@gmail.com
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
// Programa		:
// Proposito	:  
// Objeto		: 
//////////////////////////////////////////////////////////
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

using GLib;
using System.Collections;

namespace osiris
{
	public class solicitudes_enfermeria
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		[Widget] Gtk.Window solicitar_examen_labrx = null;
		[Widget] Gtk.CheckButton checkbutton_nueva_solicitud = null;
		[Widget] Gtk.Entry entry_numero_solicitud = null;
		[Widget] Gtk.Button button_selec_solilabrx = null;
		[Widget] Gtk.Button button_enviar_solicitud_labrx = null;
		[Widget] Gtk.Button button_imprimir_solilabrx = null;
		[Widget] Gtk.RadioButton radiobutton_soli_interna = null;
		[Widget] Gtk.RadioButton radiobutton_soli_externa = null;		
		[Widget] Gtk.Entry entry_id_proveedor = null;
		[Widget] Gtk.Entry entry_nombre_proveedor = null;
		[Widget] Gtk.Button button_buscar_proveedor = null;
		[Widget] Gtk.Button button_busca_producto = null;
		[Widget] Gtk.Button button_quitar_examen = null;
		[Widget] Gtk.TreeView treeview_solicitud_labrx = null;
		[Widget] Gtk.TreeView treeview_estudios_solicitados = null;
		[Widget] Gtk.Entry entry_folio_servicio = null;
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_nombre_paciente = null;
		[Widget] Gtk.Entry entry_id_doctor = null;
		[Widget] Gtk.Entry entry_doctor = null;
		[Widget] Gtk.Entry entry_diagnostico = null;
		[Widget] Gtk.Entry entry_id_habitacion = null;
		[Widget] Gtk.Statusbar statusbar_solicitud_labrx = null;
		
		/////// Ventana Busqueda de productos\\\\\\\\
		[Widget] Gtk.Window busca_producto = null;
		[Widget] Gtk.TreeView lista_de_producto;
		[Widget] Gtk.Entry entry_cantidad_aplicada;
		[Widget] Gtk.ComboBox combobox_tipo_admision;
		[Widget] Gtk.Entry entry_fecha_solicitud;
		[Widget] Gtk.Entry entry_hora_solicitud;
		[Widget] Gtk.Entry entry_folio_laboratorio;
		[Widget] Gtk.Label label_cantidad = null;
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		
		string agrupacion_lab_rx;
		string descripinternamiento;
		int id_tipoadmisiones;
		int id_tipopaciente;
		int idempresa_paciente;
		int idaseguradora_paciente;
		int PidPaciente;
		int folioservicio;
		string departament;
			//********    //nuevo lista de precios multiples//   *****************
		bool aplica_precios_aseguradoras = false;// Toma el valor de si se tiene creado la lista de precio en la tabla de Productos
		bool aplica_precios_empresas = false;	// Toma el valor de si se tiene creado la lista de precio en la tabla de Productos
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		
		string connectionString;
		string nombrebd;
		
		float valoriva;
		
		TreeStore treeViewEngineEstudios;
		TreeStore treeViewEngineEstudiosSoli;
		TreeStore treeViewEngineBusca2;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public solicitudes_enfermeria(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_,
		                              string departament_,int id_tipoadmisiones_,string agrupacion_lab_rx_,string descripinternamiento_,
		                              int id_tipopaciente_,int idempresa_paciente_,int idaseguradora_paciente_,
		                              int PidPaciente_,int folioservicio_,string nombrepaciente_,string iddoctor_,string nombremedico_,
		                              string diag_admision_,string habitacion_,bool estatus_procedimiento)
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			agrupacion_lab_rx = agrupacion_lab_rx_;
			descripinternamiento = descripinternamiento_;
			id_tipopaciente = id_tipopaciente_;
			id_tipoadmisiones = id_tipoadmisiones_;
			idempresa_paciente = idempresa_paciente_;
			idaseguradora_paciente = idaseguradora_paciente_;
			PidPaciente = PidPaciente_;
			folioservicio = folioservicio_;
			departament = departament_;
			valoriva = float.Parse(classpublic.ivaparaaplicar);
			
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "solicitar_examen_labrx", null);
			gxml.Autoconnect (this); 
			
			// show the window
			solicitar_examen_labrx.Show();
			solicitar_examen_labrx.Title = departament_+"/"+descripinternamiento;
			entry_id_proveedor.Sensitive = false;
			entry_nombre_proveedor.Sensitive = false;
			button_buscar_proveedor.Sensitive = false;
			entry_id_proveedor.Text = "1";
			entry_nombre_proveedor.Text = "SOLICITUD INTERNA";
			entry_folio_servicio.Text = folioservicio.ToString();
			entry_pid_paciente.Text = PidPaciente.ToString();
			entry_nombre_paciente.Text = nombrepaciente_;
			entry_id_doctor.Text = iddoctor_; 
			entry_doctor.Text = nombremedico_;
			entry_diagnostico.Text = diag_admision_;
			entry_id_habitacion.Text = habitacion_;
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			//radiobutton_soli_interna
			radiobutton_soli_externa.Clicked += new EventHandler(on_radiobutton_soli_externa_clicked);
			button_buscar_proveedor.Clicked += new EventHandler(on_button_buscar_proveedor_clicked);
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			button_enviar_solicitud_labrx.Clicked += new EventHandler(on_button_enviar_solicitud_labrx_clicked);
			checkbutton_nueva_solicitud.Clicked += new EventHandler(on_checkbutton_nueva_solicitud_clicked);
			button_imprimir_solilabrx.Clicked += new EventHandler(on_button_imprimir_solilabrx_clicked);				
			entry_id_proveedor.Sensitive = false;
			entry_nombre_proveedor.Sensitive = false;
			button_buscar_proveedor.Sensitive = false;
			button_enviar_solicitud_labrx.Sensitive = false;
			button_busca_producto.Sensitive = false;
			button_quitar_examen.Sensitive = false;
			//button_imprimir_solilabrx.Sensitive = false;
			if ((bool) estatus_procedimiento == false){
				checkbutton_nueva_solicitud.Sensitive = false;
			}
			
			crea_treeview_estudios();
			crea_treeview_estudios_solicitados();
			entry_numero_solicitud.ModifyBase(StateType.Normal, new Gdk.Color(0,255,0)); // Color Amarillo
			
			statusbar_solicitud_labrx.Pop(0);
			statusbar_solicitud_labrx.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_solicitud_labrx.HasResizeGrip = false;
		}
		
		void on_button_imprimir_solilabrx_clicked(object sender, EventArgs args)
	    {
			new osiris.rpt_solicitud_labrx(departament, id_tipoadmisiones,agrupacion_lab_rx," AND folio_de_solicitud = '"+this.entry_numero_solicitud.Text+"'");
		}
		
		void on_checkbutton_nueva_solicitud_clicked(object sender, EventArgs args)
	    {
	    	string ultimasolicitud;
			if (checkbutton_nueva_solicitud.Active == true){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de CREAR una Nueva SOLICITUD ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
					treeViewEngineEstudios.Clear();
					ultimasolicitud = classpublic.lee_ultimonumero_registrado("osiris_his_solicitudes_labrx","folio_de_solicitud","WHERE id_tipo_admisiones2 = '"+id_tipoadmisiones.ToString().Trim()+"' ");
					entry_numero_solicitud.Text = ultimasolicitud;
					button_enviar_solicitud_labrx.Sensitive = true;
					button_busca_producto.Sensitive = true;
					button_quitar_examen.Sensitive = true;
					button_imprimir_solilabrx.Sensitive = false;
				}else{
					checkbutton_nueva_solicitud.Active = false;
					button_busca_producto.Sensitive = false;
					button_quitar_examen.Sensitive = false;
					button_imprimir_solilabrx.Sensitive = false;
				}
			}else{
				button_enviar_solicitud_labrx.Sensitive = false;
				button_busca_producto.Sensitive = false;
				button_quitar_examen.Sensitive = false;
				button_imprimir_solilabrx.Sensitive = false;
			}
		}				
		
		void on_radiobutton_soli_externa_clicked(object sender, EventArgs args)
		{
			if(radiobutton_soli_externa.Active == true){
				entry_id_proveedor.Sensitive = true;
				entry_nombre_proveedor.Sensitive = true;
				button_buscar_proveedor.Sensitive = true;
				entry_id_proveedor.Text = "1";
				entry_nombre_proveedor.Text = "";
			}else{
				entry_id_proveedor.Sensitive = false;
				entry_nombre_proveedor.Sensitive = false;
				button_buscar_proveedor.Sensitive = false;
				entry_id_proveedor.Text = "1";
				entry_nombre_proveedor.Text = "SOLICITUD INTERNA";
			}			
		}
		
		void on_button_buscar_proveedor_clicked(object sender, EventArgs args){
			object[] parametros_objetos = {entry_id_proveedor,entry_nombre_proveedor};
			string[] parametros_sql = {"SELECT descripcion_proveedor,direccion_proveedor,rfc_proveedor,curp_proveedor, "+
								"colonia_proveedor,municipio_proveedor,estado_proveedor,telefono1_proveedor, "+ 
								"telefono2_proveedor,celular_proveedor,rfc_proveedor, proveedor_activo, "+
								"id_proveedor,contacto1_proveedor,mail_proveedor,pagina_web_proveedor,"+
								"osiris_erp_proveedores.id_forma_de_pago,descripcion_forma_de_pago,fax_proveedor "+
								"FROM osiris_erp_proveedores, osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_proveedores.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago ",				
								"SELECT descripcion_proveedor,direccion_proveedor,rfc_proveedor,curp_proveedor, "+
								"colonia_proveedor,municipio_proveedor,estado_proveedor,telefono1_proveedor, "+ 
								"telefono2_proveedor,celular_proveedor,rfc_proveedor, proveedor_activo, "+
								"id_proveedor,contacto1_proveedor,mail_proveedor,pagina_web_proveedor, "+
								"osiris_erp_proveedores.id_forma_de_pago,descripcion_forma_de_pago,fax_proveedor "+
								"FROM osiris_erp_proveedores, osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_proveedores.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
								"AND descripcion_proveedor LIKE '%"};
			classfind_data.buscandor(parametros_objetos,parametros_sql,"find_proveedores_catalogo_producto"," ORDER BY descripcion_proveedor;","%' ",0);
		}
		
		void on_button_busca_producto_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "laboratorio.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			label_cantidad.Text = "Cantidad Solicitada";
			crea_treeview_busqueda("producto");
			busca_producto.SetPosition(WindowPosition.CenterOnParent);
			
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_entry_expresion;
			
			entry_fecha_solicitud.Text = DateTime.Now.ToString("yyyy-MM-dd");
			entry_hora_solicitud.Text = DateTime.Now.ToString("HH:mm:ss");
						
			// Validando que sean solo numeros
			entry_cantidad_aplicada.KeyPressEvent += onKeyPressEvent;
			entry_folio_laboratorio.KeyPressEvent += onKeyPressEvent;
			
			//SE LLENA EL COMBO BOX	
			combobox_tipo_admision.Clear();
			CellRendererText cell2 = new CellRendererText();
			combobox_tipo_admision.PackStart(cell2, true);
			combobox_tipo_admision.AddAttribute(cell2,"text",0);
	        
			ListStore store2 = new ListStore( typeof (string), typeof (int));
			combobox_tipo_admision.Model = store2;
			
			// si es * se llena para hace solicitudes desde cualquier departamento, para que la classe
			// se pueda llamar desde otro modulo
			if(descripinternamiento == "*"){	        
		      	NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
	            // Verifica que la base de datos este conectada
	            try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
	               	comando.CommandText = "SELECT * FROM osiris_his_tipo_admisiones WHERE servicio_directo = 'false' "+
		           							"AND cuenta_mayor = 4000 "+
		               						" ORDER BY id_tipo_admisiones;";
					
					NpgsqlDataReader lector = comando.ExecuteReader ();
					store2.AppendValues ("", 0);
	               	while (lector.Read()){
						store2.AppendValues ((string) lector["descripcion_admisiones"], (int) lector["id_tipo_admisiones"]);
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
				conexion.Close ();
			}else{
				store2.AppendValues (descripinternamiento,id_tipoadmisiones);
			}
			TreeIter iter2;
			if (store2.GetIterFirst(out iter2)) {
				//Console.WriteLine(iter2);
				combobox_tipo_admision.SetActiveIter (iter2);
			}
			combobox_tipo_admision.Changed += new EventHandler (onComboBoxChanged_tipo_admision);	    
		}
		
		void onComboBoxChanged_tipo_admision(object sender, EventArgs args)
		{
	    	TreeIter iter;
			ComboBox combobox_tipo_admision = sender as ComboBox;			
			if (sender == null) { return; }
	  		if (combobox_tipo_admision.GetActiveIter (out iter)){
				id_tipoadmisiones = (int) combobox_tipo_admision.Model.GetValue(iter,1);
		    	descripinternamiento = (string) combobox_tipo_admision.Model.GetValue(iter,0);
		    	//Console.WriteLine(id_tipoadmisiones.ToString()+" "+descripinternamiento);
	     	}
		}
		
		void on_button_enviar_solicitud_labrx_clicked(object sender, EventArgs args)
		{
			TreeIter iter;
			string ultimasolicitud = classpublic.lee_ultimonumero_registrado("osiris_his_solicitudes_labrx","folio_de_solicitud","WHERE id_tipo_admisiones2 = '"+id_tipoadmisiones.ToString().Trim()+"' ");
			entry_numero_solicitud.Text = ultimasolicitud;
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de enviar esta Solicitud Numero :"+ultimasolicitud+"?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy(); 
			if (miResultado == ResponseType.Yes){
				if (this.treeViewEngineEstudios.GetIterFirst (out iter)){
					//for (int i = 0; i < treeViewEngineEstudios.NColumns; i++)
        			//Console.WriteLine((string) this.treeview_solicitud_labrx.Model.GetValue(iter,i));  
										
					ultimasolicitud = classpublic.lee_ultimonumero_registrado("osiris_his_solicitudes_labrx","folio_de_solicitud","WHERE id_tipo_admisiones2 = '"+id_tipoadmisiones.ToString().Trim()+"' ");
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						comando.CommandText =  "INSERT INTO osiris_his_solicitudes_labrx("+
												"folio_de_solicitud,"+
												"folio_de_servicio,"+
												"pid_paciente,"+									
												"id_producto,"+
												"precio_producto_publico,"+
												"costo_por_unidad,"+
												"cantidad_solicitada,"+
												//"cantidad_autorizada"+												
												"fechahora_solicitud,"+
												"id_quien_solicito,"+
												//"fechahora_autorizado,"+
												//"id_quien_autorizo,"+
												//"status,"+
												"id_proveedor,"+
												"id_tipo_admisiones,"+
												"folio_interno_labrx,"+
												"area_quien_solicita,"+
												"id_tipo_admisiones2)"+
														" VALUES ('"+
												ultimasolicitud+"','"+
												folioservicio.ToString()+"','"+
												PidPaciente.ToString()+"','"+										
												(string) this.treeview_solicitud_labrx.Model.GetValue(iter,1)+"','"+
												(string) this.treeview_solicitud_labrx.Model.GetValue(iter,8)+"','"+
												(string) this.treeview_solicitud_labrx.Model.GetValue(iter,9)+"','"+
												(string) this.treeview_solicitud_labrx.Model.GetValue(iter,0)+"','"+
												//
												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
												LoginEmpleado+"','"+
												//
												//
												//
												entry_id_proveedor.Text.Trim()+"','"+
												id_tipopaciente.ToString().Trim()+"','"+
												(string) this.treeview_solicitud_labrx.Model.GetValue(iter,10).ToString().Trim()+"','"+
												(string) descripinternamiento+"','"+
												id_tipoadmisiones.ToString().Trim()+"');";
						Console.WriteLine(comando.CommandText);
						comando.ExecuteNonQuery();
						comando.Dispose();					
						while (this.treeViewEngineEstudios.IterNext(ref iter)){
							comando.CommandText = "INSERT INTO osiris_his_solicitudes_labrx("+
												"folio_de_solicitud,"+
												"folio_de_servicio,"+
												"pid_paciente,"+									
												"id_producto,"+
												"precio_producto_publico,"+
												"costo_por_unidad,"+
												"cantidad_solicitada,"+
												//"cantidad_autorizada"+												
												"fechahora_solicitud,"+
												"id_quien_solicito,"+
												//"fechahora_autorizado,"+
												//"id_quien_autorizo,"+
												//"status,"+
												"id_proveedor,"+
												"id_tipo_admisiones,"+
												"folio_interno_labrx,"+
												"area_quien_solicita,"+
												"id_tipo_admisiones2)"+
														" VALUES ('"+
												ultimasolicitud+"','"+
												folioservicio.ToString()+"','"+
												PidPaciente.ToString()+"','"+										
												(string) this.treeview_solicitud_labrx.Model.GetValue(iter,1)+"','"+
												(string) this.treeview_solicitud_labrx.Model.GetValue(iter,8)+"','"+
												(string) this.treeview_solicitud_labrx.Model.GetValue(iter,9)+"','"+
												(string) this.treeview_solicitud_labrx.Model.GetValue(iter,0)+"','"+
												//
												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
												LoginEmpleado+"','"+
												//
												//
												//
												entry_id_proveedor.Text.Trim()+"','"+
												id_tipopaciente.ToString().Trim()+"','"+
												(string) this.treeview_solicitud_labrx.Model.GetValue(iter,10).ToString().Trim()+"','"+
												(string) descripinternamiento+"','"+
												id_tipoadmisiones.ToString().Trim()+"');";
							//Console.WriteLine(comando.CommandText);
							comando.ExecuteNonQuery();
							comando.Dispose();			
						}
						checkbutton_nueva_solicitud.Active = false;
						button_imprimir_solilabrx.Sensitive = true;
						MessageDialog msgBoxError = new MessageDialog(MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
					                                              ButtonsType.Close, "La solicitud se envio con Exito...");
						msgBoxError.Run ();	msgBoxError.Destroy();
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();				msgBoxError.Destroy();
					}
					conexion.Close();
				}else{
					MessageDialog msgBoxError = new MessageDialog(MyWinError,DialogFlags.DestroyWithParent,MessageType.Error,
					                                              ButtonsType.Close, "NO puede crear una solicitud, no ha seleccionado ningun estudio, verifique...");
					msgBoxError.Run ();	msgBoxError.Destroy();
				}
			}
		}
		
		void crea_treeview_estudios()
		{
			treeViewEngineEstudios = new TreeStore(typeof(string),
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
			treeview_solicitud_labrx.Model = treeViewEngineEstudios;			
			treeview_solicitud_labrx.RulesHint = true;
			
			Gtk.TreeViewColumn col_request = new TreeViewColumn();		
			Gtk.CellRendererText cellrt0 = new Gtk.CellRendererText();
			col_request.Title = "Cant.Solicitado";
			col_request.PackStart(cellrt0, true);
			col_request.AddAttribute (cellrt0, "text", 0);
			col_request.Resizable = true;
			
			TreeViewColumn col_idproducto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_idproducto.Title = "ID Producto"; // titulo de la cabecera de la columna, si está visible
			col_idproducto.PackStart(cellr1, true);
			col_idproducto.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
			col_idproducto.Resizable = true;
						
			TreeViewColumn col_desc_producto = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_desc_producto.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
			col_desc_producto.PackStart(cellr2, true);
			col_desc_producto.AddAttribute (cellr2, "text", 2);    // la siguiente columna será 1 en vez de 1
			col_desc_producto.Resizable = true;
			
			Gtk.TreeViewColumn col_depart = new TreeViewColumn();		
			Gtk.CellRendererText cellrt3 = new Gtk.CellRendererText();
			col_depart.Title = "Departamento";
			col_depart.PackStart(cellrt3, true);
			col_depart.AddAttribute (cellrt3, "text", 3);
			col_depart.Resizable = true;
			
			Gtk.TreeViewColumn col_gabinete = new TreeViewColumn();		
			Gtk.CellRendererText cellrt4 = new Gtk.CellRendererText();
			col_gabinete.Title = "Gabinete";
			col_gabinete.PackStart(cellrt4, true);
			col_gabinete.AddAttribute (cellrt4, "text", 4);
			col_gabinete.Resizable = true;
			
			Gtk.TreeViewColumn col_fechasol = new TreeViewColumn();		
			Gtk.CellRendererText cellrt5 = new Gtk.CellRendererText();
			col_fechasol.Title = "Fecha Solicitud";
			col_fechasol.PackStart(cellrt5, true);
			col_fechasol.AddAttribute (cellrt5, "text", 5);
			col_fechasol.Resizable = true;
			
			Gtk.TreeViewColumn col_horasol = new TreeViewColumn();		
			Gtk.CellRendererText cellrt6 = new Gtk.CellRendererText();
			col_horasol.Title = "Hora Solicitud";
			col_horasol.PackStart(cellrt6, true);
			col_horasol.AddAttribute (cellrt6, "text", 6);
			col_horasol.Resizable = true;
			
			Gtk.TreeViewColumn col_quiensolicita = new TreeViewColumn();		
			Gtk.CellRendererText cellrt7 = new Gtk.CellRendererText();
			col_quiensolicita.Title = "Quien Solicita";
			col_quiensolicita.PackStart(cellrt7, true);
			col_quiensolicita.AddAttribute (cellrt7, "text", 7);
			col_quiensolicita.Resizable = true;
			
			Gtk.TreeViewColumn col_foliointerno = new TreeViewColumn();		
			Gtk.CellRendererText cellrt10 = new Gtk.CellRendererText();
			col_foliointerno.Title = "Folio "+agrupacion_lab_rx;
			col_foliointerno.PackStart(cellrt10, true);
			col_foliointerno.AddAttribute (cellrt10, "text", 10);
			col_foliointerno.Resizable = true;
			
			treeview_solicitud_labrx.AppendColumn(col_request); 		// 0
			treeview_solicitud_labrx.AppendColumn(col_idproducto);  	// 1
			treeview_solicitud_labrx.AppendColumn(col_desc_producto);	// 2
			treeview_solicitud_labrx.AppendColumn(col_depart);	 		// 3
			treeview_solicitud_labrx.AppendColumn(col_gabinete); 		// 4
			treeview_solicitud_labrx.AppendColumn(col_fechasol); 		// 5
			treeview_solicitud_labrx.AppendColumn(col_horasol); 		// 6
			treeview_solicitud_labrx.AppendColumn(col_quiensolicita); 	// 7
			treeview_solicitud_labrx.AppendColumn(col_foliointerno); 	// 8
		}
		
		void crea_treeview_estudios_solicitados()
		{
			treeViewEngineEstudiosSoli = new TreeStore(typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(bool),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));
			treeview_estudios_solicitados.Model = treeViewEngineEstudiosSoli;
			treeview_estudios_solicitados.RulesHint = true;
			
			TreeViewColumn col_nrosolicitud = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_nrosolicitud.Title = "N° Solicitud"; // titulo de la cabecera de la columna, si está visible
			col_nrosolicitud.PackStart(cellr0, true);
			col_nrosolicitud.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
			col_nrosolicitud.Resizable = true;
								
			Gtk.TreeViewColumn col_depart = new TreeViewColumn();		
			Gtk.CellRendererText cellrt1 = new Gtk.CellRendererText();
			col_depart.Title = "Departamento";
			col_depart.PackStart(cellrt1, true);
			col_depart.AddAttribute (cellrt1, "text", 1);
			col_depart.Resizable = true;
			
			Gtk.TreeViewColumn col_gabinete = new TreeViewColumn();		
			Gtk.CellRendererText cellrt2 = new Gtk.CellRendererText();
			col_gabinete.Title = "Gabinete";
			col_gabinete.PackStart(cellrt2, true);
			col_gabinete.AddAttribute (cellrt2, "text", 2);
			col_gabinete.Resizable = true;
			
			Gtk.TreeViewColumn col_fechasol = new TreeViewColumn();		
			Gtk.CellRendererText cellrt3 = new Gtk.CellRendererText();
			col_fechasol.Title = "Fecha Solicitud";
			col_fechasol.PackStart(cellrt3, true);
			col_fechasol.AddAttribute (cellrt3, "text", 3);
			col_fechasol.Resizable = true;
			
			Gtk.TreeViewColumn col_horasol = new TreeViewColumn();		
			Gtk.CellRendererText cellrt4 = new Gtk.CellRendererText();
			col_horasol.Title = "Hora Solicitud";
			col_horasol.PackStart(cellrt4, true);
			col_horasol.AddAttribute (cellrt4, "text", 4);
			col_horasol.Resizable = true;
			
			Gtk.TreeViewColumn col_quiensolicita = new TreeViewColumn();		
			Gtk.CellRendererText cellrt5 = new Gtk.CellRendererText();
			col_quiensolicita.Title = "Quien Solicita";
			col_quiensolicita.PackStart(cellrt5, true);
			col_quiensolicita.AddAttribute (cellrt5, "text", 5);
			col_quiensolicita.Resizable = true;
			
			Gtk.TreeViewColumn col_foliointerno = new TreeViewColumn();		
			Gtk.CellRendererText cellrt6 = new Gtk.CellRendererText();
			col_foliointerno.Title = "Folio "+agrupacion_lab_rx;
			col_foliointerno.PackStart(cellrt6, true);
			col_foliointerno.AddAttribute (cellrt6, "text", 6);
			col_foliointerno.Resizable = true;
			
			Gtk.TreeViewColumn col_cargado = new TreeViewColumn();	
			Gtk.CellRendererToggle cellrt7 = new CellRendererToggle();		
			col_cargado.Title = "Cargado";
			col_cargado.PackStart(cellrt7, true);
			col_cargado.AddAttribute (cellrt7, "active", 7);
			col_cargado.Resizable = true;
			
			treeview_estudios_solicitados.AppendColumn(col_nrosolicitud); 	// 0
			treeview_estudios_solicitados.AppendColumn(col_depart);	 		// 1
			treeview_estudios_solicitados.AppendColumn(col_gabinete); 		// 2
			treeview_estudios_solicitados.AppendColumn(col_fechasol); 		// 3
			treeview_estudios_solicitados.AppendColumn(col_horasol); 		// 4
			treeview_estudios_solicitados.AppendColumn(col_quiensolicita); 	// 5
			treeview_estudios_solicitados.AppendColumn(col_foliointerno); 	// 6
			treeview_estudios_solicitados.AppendColumn(col_cargado);		// 7
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
				col_idproducto.Resizable = true;
			
				TreeViewColumn col_desc_producto = new TreeViewColumn();
				CellRendererText cellr1 = new CellRendererText();
				col_desc_producto.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
				col_desc_producto.PackStart(cellr1, true);
				col_desc_producto.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
				col_desc_producto.SortColumnId = (int) Column_prod.col_desc_producto;
				col_desc_producto.Resizable = true;
            	
				TreeViewColumn col_grupoprod = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_grupoprod.Title = "Grupo Producto";//Precio Producto
				col_grupoprod.PackStart(cellrt2, true);
				col_grupoprod.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
				col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
            	col_grupoprod.Resizable = true;
				
				TreeViewColumn col_grupo1prod = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_grupo1prod.Title = "Grupo1 Producto";//I.V.A.
				col_grupo1prod.PackStart(cellrt3, true);
				col_grupo1prod.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 2 en vez de 3
				col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
				col_grupo1prod.Resizable = true;
            
				TreeViewColumn col_grupo2prod = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_grupo2prod.Title = "Grupo2 Producto";//Total
				col_grupo2prod.PackStart(cellrt4, true);
				col_grupo2prod.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 3 en vez de 4
				col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;
				col_grupo2prod.Resizable = true;
            	
				lista_de_producto.AppendColumn(col_idproducto);  // 0
				lista_de_producto.AppendColumn(col_desc_producto); // 1
				lista_de_producto.AppendColumn(col_grupoprod);	//7
				lista_de_producto.AppendColumn(col_grupo1prod);	//8
				lista_de_producto.AppendColumn(col_grupo2prod);	//9							
			}
		}
		
		enum Column_prod
		{
			col_idproducto,
			col_desc_producto,
			col_grupoprod,
			col_grupo1prod,
			col_grupo2prod
		}
		
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			int validacantidad = 0;
			TreeModel model;	TreeIter iterSelected;
			if(entry_cantidad_aplicada.Text != "" && entry_cantidad_aplicada.Text != "0"){
				if ((float) float.Parse(entry_cantidad_aplicada.Text) > 0){
					if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
						treeViewEngineEstudios.AppendValues((string) entry_cantidad_aplicada.Text.ToString().Trim(),
													(string) lista_de_producto.Model.GetValue (iterSelected,0),
													(string) lista_de_producto.Model.GetValue (iterSelected,1),
				                                    descripinternamiento,
				                                    entry_nombre_proveedor.Text,
				                                    entry_fecha_solicitud.Text,
				                                    entry_hora_solicitud.Text,
				                                    LoginEmpleado,
				                                    (string) lista_de_producto.Model.GetValue (iterSelected,5),
				                                    (string) lista_de_producto.Model.GetValue (iterSelected,10),
				                                    entry_folio_laboratorio.Text.Trim());
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, ButtonsType.Close, "La cantidad solicitada NO \n"+
												"puede quedar vacia, intente de nuevo");
								msgBoxError.Run ();					msgBoxError.Destroy();
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, ButtonsType.Close, "La cantidad solicitada NO \n"+
												"puede quedar vacia, intente de nuevo");
								msgBoxError.Run ();					msgBoxError.Destroy();
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
			string precio_a_tomar = "";    // en esta variable dejo el precio que va tomar para los direfentes clientes
			string query_lab_rx = "";
			float tomaprecio;
			float calculodeiva;
			float preciomasiva;
			float tomadescue;
			float preciocondesc;
			
			if(agrupacion_lab_rx == "LAB"){
				query_lab_rx = "AND osiris_grupo_producto.agrupacion2 = 'LAB' ";
			} 			
			if(agrupacion_lab_rx == "IMG"){
				query_lab_rx = "AND osiris_grupo_producto.agrupacion3 = 'IMG' ";
			}
			
			//// para las diferentes listas de precios \\\\\\\\\\\\\			
			if (id_tipopaciente == 500 || id_tipopaciente == 102) {  // Municipio y Empresas			
				// verifica si ese cliente tiene una lista de precio asignada
				if (this.aplica_precios_empresas == true || aplica_precios_aseguradoras == true){     
					precio_a_tomar = "precio_producto_"+id_tipopaciente.ToString().Trim()+idempresa_paciente.ToString().Trim();
					//precio_a_tomar = "precio_producto_publico1";
				}else{
					precio_a_tomar = "precio_producto_publico";
				}
			}else{				
				if (id_tipopaciente == 400 ) { // Aseguradora
					precio_a_tomar = "precio_producto_"+id_tipopaciente.ToString().Trim()+idaseguradora_paciente.ToString().Trim();
				
					if (this.aplica_precios_empresas == true || aplica_precios_aseguradoras == true){    
						precio_a_tomar = "precio_producto_"+id_tipopaciente.ToString().Trim()+this.idaseguradora_paciente.ToString().Trim();
						//precio_a_tomar = "precio_producto_publico1";
					}else{
						precio_a_tomar = "precio_producto_publico";
					}
				}else{
					precio_a_tomar = "precio_producto_publico";
				}
			}			
			//Console.WriteLine(precio_a_tomar);			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();				
				comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
						"osiris_productos.descripcion_producto,"+
						"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
						//"to_char(precio_producto_publico1,'99999999.99') AS preciopublico1,"+
						"to_char("+precio_a_tomar+",'99999999.99') AS preciopublico_cliente,"+
						"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,"+
						"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,"+
						"to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
						"to_char(porcentage_ganancia,'99999.99') AS porcentageutilidad,"+
						"to_char(costo_producto,'999999999.99') AS costoproducto "+
						"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
						"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
						"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
						"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
						query_lab_rx+
						"AND osiris_productos.cobro_activo = 'true' "+
						"AND osiris_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_producto;";
					
				Console.WriteLine(comando.CommandText);				
				NpgsqlDataReader lector = comando.ExecuteReader ();										
				while (lector.Read()){
					calculodeiva = 0;
					preciomasiva = 0;					
					///////////////////////////////////////////////////////////
					// ---- nuevo para las multiples listas de precio					
					if (float.Parse((string) lector["preciopublico_cliente"]) > 0){
							tomaprecio = float.Parse((string) lector["preciopublico_cliente"]);
						}else{
							tomaprecio = float.Parse((string) lector["preciopublico"]);
					}									
					tomadescue = float.Parse((string) lector["porcentagesdesc"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					preciocondesc = tomaprecio;
					if ((bool) lector["aplicar_iva"]){
						calculodeiva = (tomaprecio * valoriva)/100;
					}
					if ((bool) lector["aplica_descuento"]){
						preciocondesc = tomaprecio-((tomaprecio*tomadescue)/100);
					}
					preciomasiva = tomaprecio + calculodeiva; 
					treeViewEngineBusca2.AppendValues (//TreeIter iter = 
											(string) lector["codProducto"],//0
											(string) lector["descripcion_producto"],//1
											(string) lector["descripcion_grupo_producto"],//2
											(string) lector["descripcion_grupo1_producto"],//3
											(string) lector["descripcion_grupo2_producto"],//4
											tomaprecio.ToString("F").PadLeft(10),//2-5
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
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace){
				args.RetVal = true;
			}
		}
		
		void on_cierraventanas_clicked(object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// 
	/// Esta classe es la que se encarga de llenar las solictudes de 
	/// Laboratorio y Rayos X
	/// 
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class solicitudes_rx_lab
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		[Widget] Gtk.Window solicitudes_examenes_labrx = null;
		[Widget] Gtk.Button button_consultar = null;
 		[Widget] Gtk.RadioButton radiobutton_estud_carg = null;
		[Widget] Gtk.RadioButton radiobutton_estud_solic = null;
		[Widget] Gtk.Notebook notebook1 = null;
		[Widget] Gtk.Entry entry_fecha_inicio = null;
		[Widget] Gtk.Entry entry_fecha_termino = null;
		[Widget] Gtk.CheckButton checkbutton_rango_fecha = null;
		
		// Tab number one application form request LAB RX
		[Widget] Gtk.TreeView treeview_lista_solicitados = null;
		[Widget] Gtk.Button button_cargar_examen = null;
		[Widget] Gtk.CheckButton checkbutton_px_solicitud = null;
		
		// Tab number two Charges to patients
		[Widget] Gtk.TreeView treeview_lista_cargosvalid = null;
		[Widget] Gtk.Button button_validar_examen = null;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
				
		string connectionString;
		string nombrebd;
		
		ArrayList columns = new ArrayList ();
		Gtk.TreeStore treeViewEnginesolicitados;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public solicitudes_rx_lab(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_,string departament_,int tipo_admisiones_)
		{			
			Glade.XML gxml = new Glade.XML (null, "imagenologia.glade", "solicitudes_examenes_labrx", null);
			gxml.Autoconnect (this);	        
			// Muestra ventana de Glade
			solicitudes_examenes_labrx.Show();
			solicitudes_examenes_labrx.Title = departament_;
			
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = conexion_a_DB._nombrebd;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			radiobutton_estud_carg.Clicked += new EventHandler(on_changetab_clicked);
			radiobutton_estud_solic.Clicked += new EventHandler(on_changetab_clicked);
			button_consultar.Clicked += new EventHandler(on_button_consultar_clicked);
			button_cargar_examen.Clicked += new EventHandler(on_button_cargar_examen_clicked);
			button_consultar.Clicked += new EventHandler(on_button_consultar_clicked);
			entry_fecha_inicio.Text = DateTime.Now.ToString("yyyy-MM-dd");
			entry_fecha_termino.Text = DateTime.Now.ToString("yyyy-MM-dd");
			checkbutton_rango_fecha.Active = true;
			//checkbutton_px_solicitud.Clicked += new EventHandler()
			
			create_treeview_solicitudes((bool) checkbutton_px_solicitud.Active);
			create_treeview_cargados();
		}
		
		void on_button_consultar_clicked(object sender, EventArgs args)
		{
			if(radiobutton_estud_carg.Active == true){
				notebook1.CurrentPage = 1;
			}
			if(radiobutton_estud_solic.Active == true){
				notebook1.CurrentPage = 0;
				create_treeview_solicitudes((bool) checkbutton_px_solicitud.Active);
			}
		}
		
		void on_changetab_clicked(object sender, EventArgs args)
		{
			//Console.WriteLine(radiobutton_seltab.Active.ToString());
			Gtk.RadioButton radiobutton_seltab = (Gtk.RadioButton) sender;
			
			if(radiobutton_seltab.Name.ToString() ==  "radiobutton_estud_carg"){
				notebook1.CurrentPage = 1;
			}
			if(radiobutton_seltab.Name.ToString() ==  "radiobutton_estud_solic"){
				notebook1.CurrentPage = 0;
			}
		}
		
		void on_button_cargar_examen_clicked(object sender, EventArgs args)
		{
			new osiris.DemoTreeStore();
		}
			
		void create_treeview_solicitudes(bool tipo_treeview)
		{
			Gtk.CellRendererText text;
			Gtk.CellRendererToggle toggle;			
			
			// create treeview List the request
			if(tipo_treeview == false){
				// Erase all columns
				foreach (TreeViewColumn tvc in this.treeview_lista_solicitados.Columns)
				this.treeview_lista_solicitados.RemoveColumn(tvc);
				treeViewEnginesolicitados = new TreeStore(typeof(bool),typeof(string),typeof(string),typeof(string),typeof(string),
													typeof(string),typeof(string),typeof(string),typeof(string));
				treeview_lista_solicitados.Model = treeViewEnginesolicitados;
				treeview_lista_solicitados.RulesHint = true;
				
				Gtk.TreeViewColumn col_request0 = new TreeViewColumn(); 	Gtk.CellRendererToggle cellrt0 = new CellRendererToggle();		
				col_request0.Title = "Seleccion";
				col_request0.PackStart(cellrt0, true);
				col_request0.AddAttribute (cellrt0, "active", 0);
				col_request0.Resizable = true;
				//col_request1.SortColumnId = (int) coldatos_request.col_request0;
				
				Gtk.TreeViewColumn col_request1 = new TreeViewColumn();		Gtk.CellRendererText cellrt1 = new Gtk.CellRendererText();		
				col_request1.Title = "Paciente";
				col_request1.PackStart(cellrt1, true);
				col_request1.AddAttribute (cellrt1, "text", 1);
				col_request1.Resizable = true;
				//col_request0.SortColumnId = (int) coldatos_request.col_request1;
				
				Gtk.TreeViewColumn col_request2 = new TreeViewColumn();		Gtk.CellRendererText cellrt2 = new Gtk.CellRendererText();		
				col_request2.Title = "Codigo";
				col_request2.PackStart(cellrt2, true);
				col_request2.AddAttribute (cellrt2, "text", 2);
				col_request2.Resizable = true;
				//col_request2.SortColumnId = (int) coldatos_request.col_request1;
				
				Gtk.TreeViewColumn col_request3 = new TreeViewColumn();		Gtk.CellRendererText cellrt3 = new Gtk.CellRendererText();		
				col_request3.Title = "Estudio Solicitado";
				col_request3.PackStart(cellrt3, true);
				col_request3.AddAttribute (cellrt3, "text", 3);
				col_request3.Resizable = true;
				//col_request2.SortColumnId = (int) coldatos_request.col_request1;
				
				Gtk.TreeViewColumn col_request4 = new TreeViewColumn();		Gtk.CellRendererText cellrt4 = new Gtk.CellRendererText();		
				col_request4.Title = "Cant.Solicitado";
				col_request4.PackStart(cellrt4, true);
				col_request4.AddAttribute (cellrt4, "text", 4);
				col_request4.Resizable = true;
				//col_request1.SortColumnId = (int) coldatos_request.col_request1;
				
				Gtk.TreeViewColumn col_request5 = new TreeViewColumn();		Gtk.CellRendererText cellrt5 = new Gtk.CellRendererText();		
				col_request5.Title = "Fech.Hora Soli.";
				col_request5.PackStart(cellrt5, true);
				col_request5.AddAttribute (cellrt5, "text", 5);
				col_request5.Resizable = true;
				//col_request1.SortColumnId = (int) coldatos_request.col_request1;
				
				Gtk.TreeViewColumn col_request6 = new TreeViewColumn();		Gtk.CellRendererText cellrt6 = new Gtk.CellRendererText();		
				col_request6.Title = "Area quien solicta";
				col_request6.PackStart(cellrt6, true);
				col_request6.AddAttribute (cellrt6, "text", 6);
				col_request6.Resizable = true;
				//col_request1.SortColumnId = (int) coldatos_request.col_request1;
				
				Gtk.TreeViewColumn col_request7 = new TreeViewColumn();		Gtk.CellRendererText cellrt7 = new Gtk.CellRendererText();		
				col_request7.Title = "Gabinete";
				col_request7.PackStart(cellrt7, true);
				col_request7.AddAttribute (cellrt7, "text", 7);
				col_request7.Resizable = true;
				//col_request1.SortColumnId = (int) coldatos_request.col_request1;
							
				treeview_lista_solicitados.AppendColumn(col_request0);
				treeview_lista_solicitados.AppendColumn(col_request1);
				treeview_lista_solicitados.AppendColumn(col_request2);
				treeview_lista_solicitados.AppendColumn(col_request3);
				treeview_lista_solicitados.AppendColumn(col_request4);
				treeview_lista_solicitados.AppendColumn(col_request5);
				treeview_lista_solicitados.AppendColumn(col_request6);
				treeview_lista_solicitados.AppendColumn(col_request7);
				
				llenado_treeview_solicitudes((bool) checkbutton_px_solicitud.Active,treeViewEnginesolicitados);
				
			}else{
				// create treeview for patients
				// Erase all columns
				foreach (TreeViewColumn tvc in this.treeview_lista_solicitados.Columns)
				this.treeview_lista_solicitados.RemoveColumn(tvc);
				
				
				//treeview_lista_solicitados.Remove();
				
				treeViewEnginesolicitados = new TreeStore(typeof(string),typeof(bool),typeof(string),typeof(string),
				                                          typeof(bool),typeof(bool));
				treeview_lista_solicitados.Model = treeViewEnginesolicitados;
				treeview_lista_solicitados.RulesHint = true;
				treeview_lista_solicitados.Selection.Mode = SelectionMode.Multiple;
				
				// column for holiday names
				text = new CellRendererText ();
				text.Xalign = 0.0f;
			 	columns.Add (text);
				TreeViewColumn column0 = new TreeViewColumn("Paciente/Estudio", text,
								    "text", Column.paciente_estudio);
				treeview_lista_solicitados.InsertColumn (column0, (int) Column.paciente_estudio);
				
				toggle = new CellRendererToggle ();
				toggle.Xalign = 0.0f;
				columns.Add (toggle);
				toggle.Toggled += new ToggledHandler (ItemToggled);
				TreeViewColumn column1 = new TreeViewColumn ("Seleccion", toggle,
							     "active", (int) Column.seleccion,
							     "visible", (int) Column.Visible,
							     "activatable", (int) Column.World);
				column1.Sizing = TreeViewColumnSizing.Fixed;
				column1.FixedWidth = 65;
				column1.Clickable = true;
				treeview_lista_solicitados.InsertColumn (column1, (int) Column.seleccion);
				
				text = new CellRendererText ();
				text.Xalign = 0.0f;
			 	columns.Add (text);
				TreeViewColumn column2 = new TreeViewColumn("Nº Solicitud", text,
								    "text", Column.nro_solicitud);
				treeview_lista_solicitados.InsertColumn (column2, (int) Column.nro_solicitud);
				
				text = new CellRendererText ();
				text.Xalign = 0.0f;
			 	columns.Add (text);
				TreeViewColumn column3 = new TreeViewColumn("Cant.Solicitado", text,
								    "text", Column.cantsolicitada);
				treeview_lista_solicitados.InsertColumn (column3, (int) Column.cantsolicitada);				
				
				llenado_treeview_solicitudes((bool) checkbutton_px_solicitud.Active,treeViewEnginesolicitados);			
			}
		}
		
		public enum Column
		{
			paciente_estudio,
			seleccion,
			nro_solicitud,
			cantsolicitada,
			
			Visible,
			World,			
		}
		
		void llenado_treeview_solicitudes(bool tipo_treeview, object obj)
		{
			Gtk.TreeStore treeViewEnginesolicitados = (Gtk.TreeStore) obj;
			Gtk.TreeIter iter;
						
			// llenado de lista de solicitudes
			if(tipo_treeview == false){
				
				NpgsqlConnection conexion;
				conexion = new NpgsqlConnection (connectionString+nombrebd );
				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					comando.CommandText = "SELECT pid_paciente FROM osiris_his_solicitudes_labrx";
					NpgsqlDataReader lector = comando.ExecuteReader ();
					while((bool) lector.Read()){
						treeViewEnginesolicitados.AppendValues(false,"folio_de_solicitud");
						
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, 
					ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();						msgBoxError.Destroy();
				}
				conexion.Close();		
			}
			// llenado por paciente
			if(tipo_treeview == true){
				iter = treeViewEnginesolicitados.AppendValues ("Paciente 1",
								    false,
				                    null,
				                    null,
								    false,
				                    false);
				
				treeViewEnginesolicitados.AppendValues (iter,
							    "Examen de Laboratorio",
							    false,
				                "numero Solicitud",
				                "Cantidad Solici",
							    true,
							    true);
			}
			
		}
				
		private void ItemToggled (object sender, ToggledArgs args)
		{
			Gtk.TreeIter iter; 			
			TreePath path = new TreePath (args.Path);
			if (treeview_lista_solicitados.Model.GetIter (out iter, path)){					
				bool old = (bool) treeview_lista_solicitados.Model.GetValue(iter,1);
				ttreeview_lista_solicitados.Model.SetValue(iter,1,!old);
			}						
		}
		
		
		void create_treeview_cargados()
		{
			//treeViewEnginecargosvalid = new TreeStore(typeof(bool),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
			//									typeof(string),typeof(string),typeof(string),typeof(string));
			//treeview_lista_cargosvalid.Model = treeViewEnginecargosvalid;
			//treeview_lista_cargosvalid.RulesHint = true;
		}
		
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
	
	public class DemoTreeStore : Gtk.Window
	{
		private TreeStore store;

		public DemoTreeStore () : base ("Card planning sheet")
		{
			VBox vbox = new VBox (false, 8);
			vbox.BorderWidth = 8;
			Add (vbox);

			vbox.PackStart (new Label ("Jonathan's Holiday Card Planning Sheet"),
					false, false, 0);

			ScrolledWindow sw = new ScrolledWindow ();
			sw.ShadowType = ShadowType.EtchedIn;
			sw.SetPolicy (PolicyType.Automatic, PolicyType.Automatic);
			vbox.PackStart (sw, true, true, 0);

			// create model
			CreateModel ();

			// create tree view
			TreeView treeView = new TreeView (store);
			treeView.RulesHint = true;
			treeView.Selection.Mode = SelectionMode.Multiple;
			AddColumns (treeView);

			sw.Add (treeView);

			// expand all rows after the treeview widget has been realized
			treeView.Realized += new EventHandler (ExpandRows);

			SetDefaultSize (650, 400);
			ShowAll ();
		}

		private void ExpandRows (object obj, EventArgs args)
		{
			TreeView treeView = obj as TreeView;

			treeView.ExpandAll ();
		}

		ArrayList columns = new ArrayList ();

		private void ItemToggled (object sender, ToggledArgs args)
		{
			int column = columns.IndexOf (sender);

 			Gtk.TreeIter iter;
 			if (store.GetIterFromString (out iter, args.Path)) {
 				bool val = (bool) store.GetValue (iter, column);
 				store.SetValue (iter, column, !val);
 			}
		}

		private void AddColumns (TreeView treeView)
		{
			CellRendererText text;
			CellRendererToggle toggle;

			// column for holiday names
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column = new TreeViewColumn ("Holiday", text,
								    "text", Column.HolidayName);
			treeView.InsertColumn (column, (int) Column.HolidayName);

			// alex column
			toggle = new CellRendererToggle ();
			toggle.Xalign = 0.0f;
			columns.Add (toggle);
			toggle.Toggled += new ToggledHandler (ItemToggled);
			
			column = new TreeViewColumn ("Alex", toggle,
						     "active", (int) Column.Alex,
						     "visible", (int) Column.Visible,
						     "activatable", (int) Column.World);
			column.Sizing = TreeViewColumnSizing.Fixed;
			column.FixedWidth = 50;
			column.Clickable = true;
			treeView.InsertColumn (column, (int) Column.Alex);

			// havoc column
			toggle = new CellRendererToggle ();
			toggle.Xalign = 0.0f;
			columns.Add (toggle);
			toggle.Toggled += new ToggledHandler (ItemToggled);
			column = new TreeViewColumn ("Havoc", toggle,
						     "active", (int) Column.Havoc,
						     "visible", (int) Column.Visible);
			treeView.InsertColumn (column, (int) Column.Havoc);
			column.Sizing = TreeViewColumnSizing.Fixed;
			column.FixedWidth = 50;
			column.Clickable = true;

			// tim column
			toggle = new CellRendererToggle ();
			toggle.Xalign = 0.0f;
			columns.Add (toggle);
			toggle.Toggled += new ToggledHandler (ItemToggled);
			column = new TreeViewColumn ("Tim", toggle,
						     "active", (int) Column.Tim,
						     "visible", (int) Column.Visible,
						     "activatable", (int) Column.World);
			treeView.InsertColumn (column, (int) Column.Tim);
			column.Sizing = TreeViewColumnSizing.Fixed;
			column.FixedWidth = 50;
			column.Clickable = true;

			// owen column
			toggle = new CellRendererToggle ();
			toggle.Xalign = 0.0f;
			columns.Add (toggle);
			toggle.Toggled += new ToggledHandler (ItemToggled);
			column = new TreeViewColumn ("Owen", toggle,
						     "active", (int) Column.Owen,
						     "visible", (int) Column.Visible);
			treeView.InsertColumn (column, (int) Column.Owen);
			column.Sizing = TreeViewColumnSizing.Fixed;
			column.FixedWidth = 50;
			column.Clickable = true;

			// dave column
			toggle = new CellRendererToggle ();
			toggle.Xalign = 0.0f;
			columns.Add (toggle);
			toggle.Toggled += new ToggledHandler (ItemToggled);
			column = new TreeViewColumn ("Dave", toggle,
						     "active", (int) Column.Dave,
						     "visible", (int) Column.Visible);
			treeView.InsertColumn (column, (int) Column.Dave);
			column.Sizing = TreeViewColumnSizing.Fixed;
			column.FixedWidth = 50;
			column.Clickable = true;
		}

		protected override bool OnDeleteEvent (Gdk.Event evt)
		{
			Destroy ();
			return true;
		}

		private void CreateModel ()
		{
			// create tree store
			store = new TreeStore (typeof (string),
					       typeof (bool),
					       typeof (bool),
					       typeof (bool),
					       typeof (bool),
					       typeof (bool),
					       typeof (bool),
					       typeof (bool));

			// add data to the tree store
			foreach (MyTreeItem month in toplevel) {
				TreeIter iter = store.AppendValues (month.Label,
								    false,
								    false,
								    false,
								    false,
								    false,
								    false,
								    false);

				foreach (MyTreeItem holiday in month.Children) {
					store.AppendValues (iter,
							    holiday.Label,
							    holiday.Alex,
							    holiday.Havoc,
							    holiday.Tim,
							    holiday.Owen,
							    holiday.Dave,
							    true,
							    holiday.WorldHoliday);
				}
			}
		}

		// tree data
		private static MyTreeItem[] january =
		{
			new MyTreeItem ("New Years Day", true, true, true, true, false, true, null ),
			new MyTreeItem ("Presidential Inauguration", false, true, false, true, false, false, null ),
			new MyTreeItem ("Martin Luther King Jr. day", false, true, false, true, false, false, null )
		};

		private static MyTreeItem[] february =
		{
			new MyTreeItem ( "Presidents' Day", false, true, false, true, false, false, null ),
			new MyTreeItem ( "Groundhog Day", false, false, false, false, false, false, null ),
			new MyTreeItem ( "Valentine's Day", false, false, false, false, true, true, null )
		};

		private static MyTreeItem[] march =
		{
			new MyTreeItem ( "National Tree Planting Day", false, false, false, false, false, false, null ),
			new MyTreeItem ( "St Patrick's Day", false, false, false, false, false, true, null )
		};

		private static MyTreeItem[] april =
		{
			new MyTreeItem ( "April Fools' Day", false, false, false, false, false, true, null ),
			new MyTreeItem ( "Army Day", false, false, false, false, false, false, null ),
			new MyTreeItem ( "Earth Day", false, false, false, false, false, true, null ),
			new MyTreeItem ( "Administrative Professionals' Day", false, false, false, false, false, false, null )
		};

		private static MyTreeItem[] may =
		{
			new MyTreeItem ( "Nurses' Day", false, false, false, false, false, false, null ),
			new MyTreeItem ( "National Day of Prayer", false, false, false, false, false, false, null ),
			new MyTreeItem ( "Mothers' Day", false, false, false, false, false, true, null ),
			new MyTreeItem ( "Armed Forces Day", false, false, false, false, false, false, null ),
			new MyTreeItem ( "Memorial Day", true, true, true, true, false, true, null )
		};

		private static MyTreeItem[] june =
		{
			new MyTreeItem ( "June Fathers' Day", false, false, false, false, false, true, null ),
			new MyTreeItem ( "Juneteenth (Liberation of Slaves)", false, false, false, false, false, false, null ),
			new MyTreeItem ( "Flag Day", false, true, false, true, false, false, null )
		};

		private static MyTreeItem[] july =
		{
			new MyTreeItem ( "Parents' Day", false, false, false, false, false, true, null ),
			new MyTreeItem ( "Independence Day", false, true, false, true, false, false, null )
		};

		private static MyTreeItem[] august =
		{
			new MyTreeItem ( "Air Force Day", false, false, false, false, false, false, null ),
			new MyTreeItem ( "Coast Guard Day", false, false, false, false, false, false, null ),
			new MyTreeItem ( "Friendship Day", false, false, false, false, false, false, null )
		};

		private static MyTreeItem[] september =
		{
			new MyTreeItem ( "Grandparents' Day", false, false, false, false, false, true, null ),
			new MyTreeItem ( "Citizenship Day or Constitution Day", false, false, false, false, false, false, null ),
			new MyTreeItem ( "Labor Day", true, true, true, true, false, true, null )
		};

		private static MyTreeItem[] october =
		{
			new MyTreeItem ( "National Children's Day", false, false, false, false, false, false, null ),
			new MyTreeItem ( "Bosses' Day", false, false, false, false, false, false, null ),
			new MyTreeItem ( "Sweetest Day", false, false, false, false, false, false, null ),
			new MyTreeItem ( "Mother-in-Law's Day", false, false, false, false, false, false, null ),
			new MyTreeItem ( "Navy Day", false, false, false, false, false, false, null ),
			new MyTreeItem ( "Columbus Day", false, true, false, true, false, false, null ),
			new MyTreeItem ( "Halloween", false, false, false, false, false, true, null )
		};

		private static MyTreeItem[] november =
		{
			new MyTreeItem ( "Marine Corps Day", false, false, false, false, false, false, null ),
			new MyTreeItem ( "Veterans' Day", true, true, true, true, false, true, null ),
			new MyTreeItem ( "Thanksgiving", false, true, false, true, false, false, null )
		};

		private static MyTreeItem[] december =
		{
			new MyTreeItem ( "Pearl Harbor Remembrance Day", false, false, false, false, false, false, null ),
			new MyTreeItem ( "Christmas", true, true, true, true, false, true, null ),
			new MyTreeItem ( "Kwanzaa", false, false, false, false, false, false, null )
		};


		private static MyTreeItem[] toplevel =
		{
			new MyTreeItem ("January", false, false, false, false, false, false, january),
			new MyTreeItem ("February", false, false, false, false, false, false, february),
			new MyTreeItem ("March", false, false, false, false, false, false, march),
			new MyTreeItem ("April", false, false, false, false, false, false, april),
			new MyTreeItem ("May", false, false, false, false, false, false, may),
			new MyTreeItem ("June", false, false, false, false, false, false, june),
			new MyTreeItem ("July", false, false, false, false, false, false, july),
			new MyTreeItem ("August", false, false, false, false, false, false, august),
			new MyTreeItem ("September", false, false, false, false, false, false, september),
			new MyTreeItem ("October", false, false, false, false, false, false, october),
			new MyTreeItem ("November", false, false, false, false, false, false, november),
			new MyTreeItem ("December", false, false, false, false, false, false, december)
		};

		// TreeItem structure
		public class MyTreeItem
		{
			public string Label;
			public bool Alex, Havoc, Tim, Owen, Dave;
			public bool WorldHoliday; // shared by the European hackers
			public MyTreeItem[] Children;

			public MyTreeItem (string label, bool alex, bool havoc, bool tim,
					   bool owen, bool dave, bool worldHoliday,
					   MyTreeItem[] children)
			{
				Label = label;
				Alex = alex;
				Havoc = havoc;
				Tim = tim;
				Owen = owen;
				Dave = dave;
				WorldHoliday =  worldHoliday;
				Children = children;
			}
		}

		// columns
		public enum Column
		{
			HolidayName,
			Alex,
			Havoc,
			Tim,
			Owen,
			Dave,

			Visible,
			World,
		}
	}
}
