/////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////
// created on 22/02/2008 at 10:03 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
//				  Ing.Jesus Buentello Garza (programacion Mono)							
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
// Proposito	: Solicitud de materiales para los diferentes centros de costos medicos 
// Objeto		: hospitalizacion_solicitud_mat.cs 
//////////////////////////////////////////////////////////
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class orden_compra_urgencias
	{
		
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		
		// Declarando ventana principal de solicitud de Materiales
		[Widget] Gtk.Window autoriza_compra_medicamentos;
		[Widget] Gtk.ComboBox combobox_sub_almacenes;
		[Widget] Gtk.TreeView lista_de_autorizacion;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.Button button_quitar_aplicados;
		[Widget] Gtk.Button button_autorizacion;
		[Widget] Gtk.Entry entry_fecha_autorizacion;
		[Widget] Gtk.Entry entry_desc_producto;
		[Widget] Gtk.Entry entry_orden_compra;
		[Widget] Gtk.Button button_selecciona_compra;
		
		//Declarando Buscadores paciente medico
		[Widget] Gtk.Button button_busca_paciente;
		[Widget] Gtk.Button button_busca_medico;
		
		/////// Ventana Busqueda de paciente\\\\\\\\
		[Widget] Gtk.TreeView lista_de_Pacientes;
		[Widget] Gtk.Button button_nuevo_paciente;
		[Widget] Gtk.RadioButton radiobutton_busca_apellido;
		[Widget] Gtk.RadioButton radiobutton_busca_nombre;
		[Widget] Gtk.RadioButton radiobutton_busca_expediente;
		[Widget] Gtk.Entry entry_folio_servicio;
		[Widget] Gtk.Entry entry_nombre_paciente;
		//[Widget] Gtk.Entry entry_medico_prescribe;

		//buton limpia
		//[Widget] Gtk.Button button_limpia;
		[Widget] Gtk.CheckButton checkbutton_nuevo;
		[Widget] Gtk.Button button_agrega_nuevo;
		
		//ventana de busqueda de medicos
		//[Widget] Gtk.Button button_buscar_busqueda;
		[Widget] Gtk.TreeView lista_de_medicos;
		[Widget] Gtk.ComboBox combobox_tipo_busqueda;
	
		//Declarando la barra de estado
		[Widget] Gtk.Statusbar statusbar_autorizacion_compra;
		
		/////// Ventana Busqueda de productos\\\\\\\\
		[Widget] Gtk.TreeView lista_de_producto;
		//[Widget] Gtk.Button button_agrega_extra;
		[Widget] Gtk.Entry entry_cantidad_aplicada;
		[Widget] Gtk.Label label_titulo_cantidad;
		[Widget] Gtk.Label label_seleccion;
		
		//buscar provedor
		[Widget] Gtk.Entry entry_provedor;
		[Widget] Gtk.Button button_busca_provedor;
		[Widget] Gtk.TreeView lista_de_busqueda;
		
		//busca medico
		//[Widget] Gtk.Entry entry_id_medico;
		[Widget] Gtk.Entry entry_nombre_medico;
		//[Widget] Gtk.Entry entry_especialidad_medico;
		//[Widget] Gtk.Entry entry_tel_medico;
		//[Widget] Gtk.Entry entry_cedula_medico;
		
		//private TreeStore treeViewEngineBusca;
		private TreeStore treeViewEngineMedicos;
		private TreeStore treeViewEngineBusca;
		private TreeStore treeViewEngineBusca2;
		private TreeStore treeViewEngineAutorizados;
		private TreeStore treeViewEngineproveedores;
		public ListStore store2;		
		
		//public bool checkbutton_nuevo;
		int idmedico = 1;
		string nombmedico = "";
 		string especialidadmed = "";
 		string telmedico = "";
 		string cedmedico = "";
 		string diagnostico="";
 		float cant_embalaje = 0;
		string aplicada ="";
		float total_surtido = 0;
		int provedor = 0;
		int idmedicos = 0;
		string tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";
		string nombres = "";
		int folioservicio = 0;
		int idproveedor = 0;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		string connectionString;
		class_conexion conexion_a_DB = new class_conexion();
		
		int idsubalmacen;
		string descripcion_subalmacen;
		float valoriva = 15;
		string tipo_busqueda;
		
		//Declaracion de ventana de error y pregunta
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public orden_compra_urgencias(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_,int idalmacen_,string descripcion_almacen_,int idproveedor_, string descripcion_proveedor_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			idsubalmacen = idalmacen_;
			descripcion_subalmacen = descripcion_almacen_;
			idproveedor = idproveedor_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
						
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "autoriza_compra_medicamentos", null);
			gxml.Autoconnect (this);        
			////// Muestra ventana de Glade
			autoriza_compra_medicamentos.Show();
			// Busqueda de Productos
			
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			button_quitar_aplicados.Clicked += new EventHandler(on_button_quitar_aplicados_clicked);
			//entry_fecha_autorizacion.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");	
			button_selecciona_compra.Clicked += new EventHandler(on_button_selecciona_compra_clicked);
			checkbutton_nuevo.Clicked += new EventHandler(on_check_nueva_clicked);
			button_busca_provedor.Clicked += new EventHandler(on_busca_proveedores);
			button_busca_paciente.Clicked += new EventHandler(on_button_buscar_paciente_clicked);
			button_busca_medico.Clicked += new EventHandler(on_button_busca_medico_clicked);//8
			button_autorizacion.Clicked += new EventHandler(on_button_autorizacion_clicked);
			crea_treview_autorizacion();
			entry_orden_compra.KeyPressEvent += onKeyPressEvent_orden_compra;
			llena_combobox();
			button_selecciona_compra.Sensitive = false;
			this.button_agrega_nuevo.Hide();
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_busca_medico.Sensitive = false;
		    button_busca_paciente.Sensitive = false;
		    button_busca_producto.Sensitive = false;
		    button_busca_provedor.Sensitive = false;	
			button_quitar_aplicados.Sensitive = false;			    
		    button_autorizacion.Sensitive = false;			   
		    button_selecciona_compra.Sensitive = true;
			if(this.idsubalmacen == 0){
		   		 this.combobox_sub_almacenes.Sensitive = true;
		   	}else{
		   		combobox_sub_almacenes.Sensitive = false;
		   	}
		   	
		   	if(idproveedor > 0){
		   		this.entry_provedor.Text = descripcion_proveedor_;
		   		this.provedor = idproveedor_;
		   		this.button_busca_provedor.Sensitive = false; 		   	
		   	}		   		
		}
		
		// Llenado de combobox1 
		void llena_combobox()
		{
			combobox_sub_almacenes.Clear();
			CellRendererText cell1 = new CellRendererText();
			combobox_sub_almacenes.PackStart(cell1, true);
			combobox_sub_almacenes.AddAttribute(cell1,"text",0);
			store2 = new ListStore( typeof (string), typeof (int));
			combobox_sub_almacenes.Model = store2;

			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT id_almacen,descripcion_almacen,sub_almacen FROM osiris_almacenes "+
               						"WHERE sub_almacen = 'true'  "+
               						"AND id_almacen != 11 "+
               						" ORDER BY descripcion_almacen;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
								
				store2.AppendValues (descripcion_subalmacen,idsubalmacen);
				
               	while (lector.Read()){
					store2.AppendValues ((string) lector["descripcion_almacen"], (int) lector["id_almacen"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
						
			TreeIter iter2;
			if (store2.GetIterFirst(out iter2)){
				combobox_sub_almacenes.SetActiveIter (iter2);
			}
			combobox_sub_almacenes.Changed += new EventHandler (onComboBoxChanged_sub_almacenes);
			
			statusbar_autorizacion_compra.Pop(0);
			statusbar_autorizacion_compra.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_autorizacion_compra.HasResizeGrip = false;			
		}
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_orden_compra(object o, Gtk.KeyPressEventArgs args)
		{ 
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llena_seleccion_compra();			
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace")
			{
				args.RetVal = true;
			}
		}		
		
		void onComboBoxChanged_sub_almacenes(object sender, EventArgs args)
		{
    		ComboBox combobox_sub_almacenes = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_sub_almacenes.GetActiveIter (out iter)){
		    	idsubalmacen = (int) combobox_sub_almacenes.Model.GetValue(iter,1);
	     	}
		}
		
		void on_button_selecciona_compra_clicked(object sender, EventArgs args)
	    {
	    	llena_seleccion_compra();
	    }
	    
	    void llena_seleccion_compra()
	    {
			if (LoginEmpleado =="DOLIVARES"){
			     button_quitar_aplicados.Sensitive = true;		 		
			}
			    
	    	this.treeViewEngineAutorizados.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			//editar = true;
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT to_char(osiris_erp_compra_farmacia.cantidad_autorizo,'999999') AS autorizo,"+
               						"to_char(osiris_erp_compra_farmacia.id_producto,'999999999999') AS codproducto,"+
               						"to_char(osiris_erp_compra_farmacia.total_surtir,'999999.99') AS surtir,"+
               						"to_char(osiris_erp_compra_farmacia.cantidad_embalaje,'999999.99') AS embalaje,"+
               						"to_char(osiris_erp_compra_farmacia.folio_de_servicio,'999999') AS folioatencion,"+
               						"to_char(osiris_erp_compra_farmacia.fechahora_autorizacion,'dd-MM-yyyy HH24:mi:ss') AS fechahrautorizacion,"+																				
               						"osiris_his_medicos.nombre1_medico || ' ' || osiris_his_medicos.nombre2_medico || ' ' || osiris_his_medicos.apellido_paterno_medico || ' ' || osiris_his_medicos.apellido_materno_medico AS nombremedico,"+
               						"osiris_erp_proveedores.descripcion_proveedor,"+
               						"osiris_grupo_producto.descripcion_grupo_producto,"+
               						"osiris_almacenes.descripcion_almacen AS descripcionalmacen,"+
               						"osiris_almacenes.id_almacen,"+
               						"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_completo,"+
               						"osiris_erp_compra_farmacia.eliminado, "+
						            "osiris_productos.descripcion_producto "+
               						"FROM osiris_erp_compra_farmacia,osiris_productos,osiris_grupo_producto,osiris_grupo1_producto, "+
               						"osiris_grupo2_producto,osiris_his_paciente,osiris_erp_cobros_enca,osiris_erp_proveedores,osiris_his_medicos,osiris_almacenes "+
               						"WHERE osiris_erp_compra_farmacia.id_producto = osiris_productos.id_producto "+
               						"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
               						"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
									"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
               						"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
               						"AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_compra_farmacia.folio_de_servicio "+
               						"AND osiris_erp_compra_farmacia.id_proveedor = osiris_erp_proveedores.id_proveedor "+
               						"AND osiris_erp_compra_farmacia.id_medico = osiris_his_medicos.id_medico "+
               						"AND osiris_erp_compra_farmacia.id_subalmacen = osiris_almacenes.id_almacen "+
               						"AND osiris_erp_compra_farmacia.eliminado = 'false' "+
               						"AND osiris_erp_compra_farmacia.orden_compra = '"+(string) this.entry_orden_compra.Text.Trim()+"' "+
               						"ORDER BY descripcion_producto;";
				Console.WriteLine(comando.CommandText);
              				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if(lector.Read()){
					Console.WriteLine((bool) lector["eliminado"]);
					if((bool) lector["eliminado"] == true){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						                                               MessageType.Info,ButtonsType.Close, " ORDEN CANCELADA ");
						msgBoxError.Run ();			msgBoxError.Destroy();
						this.entry_orden_compra.Text = "";
					}else{
					
						this.entry_folio_servicio.Text = (string) lector["folioatencion"];
						this.entry_nombre_paciente.Text = (string) lector["nombre_completo"];	
						this.entry_provedor.Text = (string) lector["descripcion_proveedor"];	
						this.entry_fecha_autorizacion.Text = (string) lector["fechahrautorizacion"];
						this.entry_nombre_medico.Text = (string) lector["nombremedico"];
					
						descripcion_subalmacen = (string) lector["descripcionalmacen"];
						idsubalmacen = (int) lector["id_almacen"];
					
						llena_combobox();
					
						treeViewEngineAutorizados.AppendValues((string) lector["autorizo"],
						                                       (string) lector["descripcion_producto"],
						                                       (string) lector["descripcion_grupo_producto"],
						                                       (string) lector["codproducto"],
						                                       "",
						                                       "",
						                                       "",
						                                       "",
						                                       "",
						                                       "",
						                                       "",
						                                       "",
						                                       "", 
						                                       (string) lector["embalaje"],
						                                       (string) lector["surtir"]);
						
						while(lector.Read()){
							treeViewEngineAutorizados.AppendValues((string) lector["autorizo"],
							                                       (string) lector["descripcion_producto"],
							                                       (string) lector["descripcion_grupo_producto"],
							                                       (string) lector["codproducto"],
							                                       "",
							                                       "",
							                                       "",
							                                       "",
							                                       "",
							                                       "",
							                                       "",
							                                       "",
							                                       "", 
							                                       (string) lector["embalaje"],
							                                       (string) lector["surtir"]);
						}
					}
				}

			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
							msgBoxError.Run ();					msgBoxError.Destroy();
				}
			conexion.Close ();   
	    TreeIter iter2;
			if (store2.GetIterFirst(out iter2)){
				combobox_sub_almacenes.SetActiveIter (iter2);
			}
	    }
	    
	    
	    void on_check_nueva_clicked(object sender, EventArgs args)
	   	{	
	   		if (LoginEmpleado =="DOLIVARES"){
	    		if(this.checkbutton_nuevo.Active == true){
			    	entry_fecha_autorizacion.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					this.button_busca_medico.Sensitive = true;
					this.button_busca_paciente.Sensitive = true;
					this.button_busca_producto.Sensitive = true;
					this.combobox_sub_almacenes.Sensitive = true;
				    button_quitar_aplicados.Sensitive = true;
				    button_autorizacion.Sensitive = true;
				    button_agrega_nuevo.Sensitive = true;
			    	button_selecciona_compra.Sensitive = false;	
			    	this.entry_orden_compra.Text = "";
			    	this.entry_folio_servicio.Text = "";
			    	this.entry_nombre_medico.Text = "";
			    	this.entry_nombre_paciente.Text = "";
			    	if(idproveedor == 0){
						this.entry_provedor.Text = "";
						this.button_busca_provedor.Sensitive = true;
					}
			    	this.treeViewEngineAutorizados.Clear();
			    		    	
			    	llena_combobox();
			    	
			    }else{
				   	this.button_busca_medico.Sensitive = false;
				    this.button_busca_paciente.Sensitive = false;
				    this.button_busca_producto.Sensitive = false;
				    this.button_busca_provedor.Sensitive = false;
				    this.combobox_sub_almacenes.Sensitive = false;			
				    button_quitar_aplicados.Sensitive = false;
				    button_autorizacion.Sensitive = false;
				    button_agrega_nuevo.Sensitive = false;
				    button_selecciona_compra.Sensitive = true; 	
		    	}
		    }else{
		    	MessageDialog msgBox6 = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Info,ButtonsType.Ok,"NO esta autorizado para crear una autorizacion....");
				msgBox6.Run ();msgBox6.Destroy();
		    }
	   }
	    
		void on_button_quitar_aplicados_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iter;
			if (this.checkbutton_nuevo.Active == true){
 				if (lista_de_autorizacion.Selection.GetSelected (out model, out iter)) {
 					//int position = treeViewEngineAutorizados.GetPath (iter).Indices[0];
 					treeViewEngineAutorizados.Remove (ref iter);
				}
			}else{
				if (LoginEmpleado =="DOLIVARES"){
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
					                                          MessageType.Question,ButtonsType.YesNo,"¿ Desea CANCELAR esta Orden de Compra ?");
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
							comando.CommandText = "UPDATE osiris_erp_compra_farmacia "+
									"SET eliminado = 'true' , "+
									"fechahora_eliminado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
									"id_quien_elimino = '"+LoginEmpleado+"' "+								
					 				"WHERE orden_compra = '"+(string) this.entry_orden_compra.Text.Trim()+"';";							
									comando.ExecuteNonQuery();
						        	comando.Dispose();
	 												
						}catch (NpgsqlException ex){
							Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
						}
	 					conexion.Close ();
						
						
						if (this.treeViewEngineAutorizados.GetIterFirst (out iter)){ 		
							NpgsqlConnection conexion2; 
							conexion2 = new NpgsqlConnection (connectionString+nombrebd);
							try{
								conexion2.Open ();
								NpgsqlCommand comando2; 
								comando2 = conexion2.CreateCommand();
								comando2.CommandText = "SELECT id_producto,id_almacen FROM osiris_catalogo_almacenes "+
															"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
															"AND eliminado = 'false' "+	
															"AND id_producto = '"+(string) lista_de_autorizacion.Model.GetValue (iter,3)+"' ;";
								NpgsqlDataReader lector2 = comando2.ExecuteReader ();
								if(lector2.Read()){
									NpgsqlConnection conexion3; 
									conexion3 = new NpgsqlConnection (connectionString+nombrebd);
									try{

										conexion3.Open ();
										NpgsqlCommand comando3; 
										comando3 = conexion3.CreateCommand();
										comando3.CommandText = "UPDATE osiris_catalogo_almacenes SET stock  = stock - '"+(string) lista_de_autorizacion.Model.GetValue (iter,14)+"',"+
																	//"historial_surtido_material = historial_surtido_material || '"+LoginEmpleado+" "+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n',"+
																	"fechahora_ultimo_surtimiento = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
																	"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																	"AND id_producto = '"+(string) lista_de_autorizacion.Model.GetValue (iter,3)+"' ;";
										//Console.WriteLine("UPDATE EN catalogos");	
										comando3.ExecuteNonQuery();
										comando3.Dispose();
										conexion3.Close();
									}catch (NpgsqlException ex){
					   						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										                                               MessageType.Error, 
										                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
										msgBoxError.Run ();
									}
									conexion3.Close();
								}													
									
							}catch (NpgsqlException ex){
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								                                               MessageType.Error, 
								                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
								msgBoxError.Destroy();
							}
							conexion2.Close();
						}
						while (this.treeViewEngineAutorizados.IterNext(ref iter)){

							NpgsqlConnection conexion2; 
							conexion2 = new NpgsqlConnection (connectionString+nombrebd);
							try{
								conexion2.Open ();
								NpgsqlCommand comando2; 
								comando2 = conexion2.CreateCommand();
								comando2.CommandText = "SELECT id_producto,id_almacen FROM osiris_catalogo_almacenes "+
															"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
															"AND eliminado = 'false' "+	
															"AND id_producto = '"+(string) lista_de_autorizacion.Model.GetValue (iter,3)+"' ;";
								NpgsqlDataReader lector2 = comando2.ExecuteReader ();
								if(lector2.Read()){
									NpgsqlConnection conexion3; 
									conexion3 = new NpgsqlConnection (connectionString+nombrebd);
									try{

										conexion3.Open ();
										NpgsqlCommand comando3; 
										comando3 = conexion3.CreateCommand();
										comando3.CommandText = "UPDATE osiris_catalogo_almacenes SET stock  = stock - '"+(string) lista_de_autorizacion.Model.GetValue (iter,14)+"',"+
																	//"historial_surtido_material = historial_surtido_material || '"+LoginEmpleado+" "+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n',"+
																	"fechahora_ultimo_surtimiento = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
																	"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																	"AND id_producto = '"+(string) lista_de_autorizacion.Model.GetValue (iter,3)+"' ;";
										//Console.WriteLine("UPDATE EN catalogos");	
										comando3.ExecuteNonQuery();
										comando3.Dispose();
										conexion3.Close();
									}catch (NpgsqlException ex){
										MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										                                               MessageType.Error, 
										                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
										msgBoxError.Run ();
									}
									conexion3.Close();
								}													
								
							}catch (NpgsqlException ex){
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								                                               MessageType.Error, 
								                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
								msgBoxError.Destroy();
							}
							conexion2.Close();
						}
					}
				}	
			}
		}
		
		
		void on_button_autorizacion_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de Autorizar la Compra?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
		 	if (miResultado == ResponseType.Yes){
		 		NpgsqlConnection conexion4;
				conexion4 = new NpgsqlConnection (connectionString+nombrebd);
    	       	// Verifica que la base de datos este conectada
    	       	try{
    	        	conexion4.Open ();
					NpgsqlCommand comando4; 
					comando4 = conexion4.CreateCommand ();
	 				comando4.CommandText = "SELECT orden_compra "+
									"FROM osiris_erp_compra_farmacia "+
									"WHERE orden_compra = '"+this.entry_orden_compra.Text.Trim()+"' "+
									"LIMIT 1 ;";
	 				//Console.WriteLine("Busca el producto");
 					NpgsqlDataReader lector4 = comando4.ExecuteReader ();
							
          			if(lector4.Read()){
          				MessageDialog msgBox6 = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Info,ButtonsType.Ok,"Esta orden de compra ya existe... verifique...");
						msgBox6.Run ();msgBox6.Destroy();
          			}else{
						if(idsubalmacen > 1){
							TreeIter iterSelected;
 							if (this.treeViewEngineAutorizados.GetIterFirst (out iterSelected)){ 				
			 					NpgsqlConnection conexion2; 
								conexion2 = new NpgsqlConnection (connectionString+nombrebd);
					 			try{
									conexion2.Open ();
									NpgsqlCommand comando2; 
									comando2 = conexion2.CreateCommand();
									comando2.CommandText = "SELECT id_producto,id_almacen FROM osiris_catalogo_almacenes "+
															"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
															"AND eliminado = 'false' "+	
															"AND id_producto = '"+(string) lista_de_autorizacion.Model.GetValue (iterSelected,3)+"' ;";
										
									NpgsqlDataReader lector2 = comando2.ExecuteReader ();
									if(lector2.Read()){
										NpgsqlConnection conexion3; 
										conexion3 = new NpgsqlConnection (connectionString+nombrebd);
					 					try{
											conexion3.Open ();
											NpgsqlCommand comando3; 
											comando3 = conexion3.CreateCommand();
											comando3.CommandText = "UPDATE osiris_catalogo_almacenes SET stock  = stock + '"+(string) lista_de_autorizacion.Model.GetValue (iterSelected,14)+"',"+
																	//"historial_surtido_material = historial_surtido_material || '"+LoginEmpleado+" "+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n',"+
																	"fechahora_ultimo_surtimiento = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
																	"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																	"AND id_producto = '"+(string) lista_de_autorizacion.Model.GetValue (iterSelected,3)+"' ;";
											//Console.WriteLine("UPDATE EN catalogos");	
											comando3.ExecuteNonQuery();
											comando3.Dispose();
											conexion3.Close();
										}catch (NpgsqlException ex){
					   						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error, 
												ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
											msgBoxError.Run ();
										}
										conexion3.Close();
									}else{
										NpgsqlConnection conexion3; 
										conexion3 = new NpgsqlConnection (connectionString+nombrebd);
		 								try{
											conexion3.Open ();
											NpgsqlCommand comando3; 
											comando3 = conexion3.CreateCommand();
											comando3.CommandText = "INSERT INTO osiris_catalogo_almacenes("+
																		"id_almacen,"+
																		"id_producto,"+
																		"stock,"+
																		"id_quien_creo,"+
																		"fechahora_alta,"+
																		"fechahora_ultimo_surtimiento) "+
																		"VALUES ('"+
																		this.idsubalmacen.ToString()+"','"+
																		(string)lista_de_autorizacion.Model.GetValue(iterSelected,3)+"','"+
																		(string)lista_de_autorizacion.Model.GetValue(iterSelected,14)+"','"+
																		this.LoginEmpleado+"','"+
																		DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																		DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');";
											//Console.WriteLine(comando3.CommandText.ToString());
											//Console.WriteLine("INSERT en catagologo");
											comando3.ExecuteNonQuery();
											comando3.Dispose();										
										}catch (NpgsqlException ex){
								   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
														MessageType.Error, 
															ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
											msgBoxError.Run ();
											msgBoxError.Destroy();
										}
										conexion3.Close();														
									}
			 					}catch (NpgsqlException ex){
									MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
														MessageType.Error, 
														ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
									msgBoxError.Run ();
									msgBoxError.Destroy();
								}
								conexion4.Close();
					 			NpgsqlConnection conexion; 
								conexion = new NpgsqlConnection (connectionString+nombrebd);
						     	//Verifica que la base de datos este conectada
								try{
									conexion.Open ();
									NpgsqlCommand comando; 
									comando = conexion.CreateCommand ();
													
									comando.CommandText = "INSERT INTO osiris_erp_compra_farmacia("+
			 												"fechahora_autorizacion,"+
			 												"folio_de_servicio,"+
			 												"cantidad_autorizo,"+
			 												"id_subalmacen,"+
			 												"id_producto,"+
			 												"cantidad_embalaje,"+
			 												"total_surtir,"+
			 												"id_proveedor,"+
			 												"id_quien_compro,"+
			 												"id_medico,"+
			 												"costo_por_unidad,"+
			 												"precio_producto_publico,"+
			 												"porcentage_ganancia,"+
			 												"orden_compra) "+
			 												"VALUES ('"+
			 												(string)entry_fecha_autorizacion.Text+"','"+
			 												(string)entry_folio_servicio.Text+"','"+
			 												(string)lista_de_autorizacion.Model.GetValue(iterSelected,0)+"','"+
			 												idsubalmacen.ToString().Trim()+"','"+
			 												(string)lista_de_autorizacion.Model.GetValue(iterSelected,3)+"','"+  // id de producto
			 												(string)lista_de_autorizacion.Model.GetValue(iterSelected,13)+"','"+
			 												(string)lista_de_autorizacion.Model.GetValue(iterSelected,14)+"','"+
			 												provedor.ToString().Trim()+"','"+
			 												LoginEmpleado.ToString().Trim()+"','"+
			 												idmedicos.ToString().Trim()+"','"+
											                (string)lista_de_autorizacion.Model.GetValue(iterSelected,5)+"','"+  // id de producto
			 												(string)lista_de_autorizacion.Model.GetValue(iterSelected,6)+"','"+
			 												(string)lista_de_autorizacion.Model.GetValue(iterSelected,7)+"','"+
			 												(string)entry_orden_compra.Text.Trim()+
			 												"');";
				 						comando.ExecuteNonQuery();
				    	    	       	comando.Dispose();
		    	    	       				
										while (this.treeViewEngineAutorizados.IterNext(ref iterSelected)){
			    	    	       			NpgsqlConnection conexion5; 
											conexion5 = new NpgsqlConnection (connectionString+nombrebd);
					 						try{
												conexion5.Open ();
												NpgsqlCommand comando5; 
												comando5 = conexion5.CreateCommand();
												comando5.CommandText = "SELECT id_producto,id_almacen FROM osiris_catalogo_almacenes "+
																		"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																		"AND eliminado = 'false' "+														
																		"AND id_producto = '"+(string) lista_de_autorizacion.Model.GetValue (iterSelected,3)+"' ;";
												NpgsqlDataReader lector5 = comando5.ExecuteReader ();
												if(lector5.Read()){
													NpgsqlConnection conexion3; 
													conexion3 = new NpgsqlConnection (connectionString+nombrebd);
					 								try{
														conexion3.Open ();
														NpgsqlCommand comando3; 
														comando3 = conexion3.CreateCommand();
														comando3.CommandText = "UPDATE osiris_catalogo_almacenes SET stock  = stock + '"+(string) lista_de_autorizacion.Model.GetValue (iterSelected,14)+"',"+
																	//"historial_surtido_material = historial_surtido_material || '"+LoginEmpleado+" "+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n',"+
																	"fechahora_ultimo_surtimiento = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
																	"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																	"AND id_producto = '"+(string) lista_de_autorizacion.Model.GetValue (iterSelected,3)+"' ;";
												
														comando3.ExecuteNonQuery();
														comando3.Dispose();
														conexion3.Close();
													}catch (NpgsqlException ex){
								   						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
															MessageType.Error, 
															ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
														msgBoxError.Run ();
													}
													conexion3.Close();
												}else{
													NpgsqlConnection conexion3; 
													conexion3 = new NpgsqlConnection (connectionString+nombrebd);
					 								try{
														conexion3.Open ();
														NpgsqlCommand comando3; 
														comando3 = conexion3.CreateCommand();
														comando3.CommandText = "INSERT INTO osiris_catalogo_almacenes("+
																		"id_almacen,"+
																		"id_producto,"+
																		"stock,"+
																		"id_quien_creo,"+
																		"fechahora_alta,"+
																		"fechahora_ultimo_surtimiento) "+
																		"VALUES ('"+
																		this.idsubalmacen.ToString()+"','"+
																		(string) lista_de_autorizacion.Model.GetValue (iterSelected,3)+"','"+
																		(string) lista_de_autorizacion.Model.GetValue (iterSelected,14)+"','"+
																		this.LoginEmpleado+"','"+
																		DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																		DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');";
														comando3.ExecuteNonQuery();
														comando3.Dispose();										
													}catch (NpgsqlException ex){
								   						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																MessageType.Error, 
																ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
														msgBoxError.Run ();
														msgBoxError.Destroy();
													}
													conexion3.Close();														
												}
						 					}catch (NpgsqlException ex){
												MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																	MessageType.Error, 
																	ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
												msgBoxError.Run ();
												msgBoxError.Destroy();
										}
		    	    	       			conexion5.Close();
		    	    	       			comando.CommandText = "INSERT INTO osiris_erp_compra_farmacia("+
		 												"fechahora_autorizacion,"+
		 												"folio_de_servicio,"+
		 												"cantidad_autorizo,"+
		 												"id_subalmacen,"+
		 												"id_producto,"+
		 												"cantidad_embalaje,"+
		 												"total_surtir,"+
		 												"id_proveedor,"+
		 												"id_quien_compro,"+
		 												"id_medico,"+
		 												"costo_por_unidad,"+
			 											"precio_producto_publico,"+
			 											"porcentage_ganancia,"+
		 												"orden_compra) "+
		 												"VALUES ('"+
		 												(string)entry_fecha_autorizacion.Text+"','"+
		 												(string)entry_folio_servicio.Text+"','"+
		 												(string)lista_de_autorizacion.Model.GetValue(iterSelected,0)+"','"+
		 												idsubalmacen.ToString().Trim()+"','"+
		 												(string)lista_de_autorizacion.Model.GetValue(iterSelected,3)+"','"+
		 												(string)lista_de_autorizacion.Model.GetValue(iterSelected,13)+"','"+
		 												(string)lista_de_autorizacion.Model.GetValue(iterSelected,14)+"','"+
		 												provedor.ToString().Trim()+"','"+
		 												LoginEmpleado.ToString().Trim()+"','"+
		 												idmedicos.ToString().Trim()+"','"+
		 												(string)lista_de_autorizacion.Model.GetValue(iterSelected,5)+"','"+  // id de producto
			 											(string)lista_de_autorizacion.Model.GetValue(iterSelected,6)+"','"+
			 											(string)lista_de_autorizacion.Model.GetValue(iterSelected,7)+"','"+
			 											(string)entry_orden_compra.Text.Trim()+
		 												"');";
		 								comando.ExecuteNonQuery();
		    	    	       			comando.Dispose();    	    	       	
		    	    	       		}		
		 						}catch (NpgsqlException ex){
									MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
									msgBoxError.Run ();
									msgBoxError.Destroy();
								}
								entry_fecha_autorizacion.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						    	this.button_busca_medico.Sensitive = true;
							    this.button_busca_paciente.Sensitive = true;
							    this.button_busca_producto.Sensitive = true;
							    this.button_busca_provedor.Sensitive = true;
							    this.combobox_sub_almacenes.Sensitive = true;
							    button_quitar_aplicados.Sensitive = true;
							    button_autorizacion.Sensitive = true;
							    button_agrega_nuevo.Sensitive = true;
						    	button_selecciona_compra.Sensitive = false;	
						    	this.entry_orden_compra.Text = "";
						    	this.entry_folio_servicio.Text = "";
						    	this.entry_nombre_medico.Text = "";
						    	this.entry_nombre_paciente.Text = ""; 
						    	this.entry_provedor.Text = "";
						    	this.treeViewEngineAutorizados.Clear();
						    	llena_combobox();
		 					}
		 				}else{
		 					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close, " selecione un subalmacen ");
							msgBoxError.Run ();			msgBoxError.Destroy();
		 				}
		 			}
		 		}catch(NpgsqlException ex){
	   				MessageDialog msgBoxError5 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError5.Run ();					msgBoxError5.Destroy();
	       		}	
			}
		}
	
		
		void on_busca_proveedores(object sender, EventArgs args)
		{
			tipo_busqueda = "proveedores";
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_proveedores);
	        button_selecciona.Clicked += new EventHandler(on_selecciona_provedor_clicked);
	       	crea_treeview_proveedores();
	       	button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_entry_expresion;
			
		}
		
		void crea_treeview_proveedores()
		{
			treeViewEngineproveedores = new TreeStore(typeof(int),//0
													typeof(string),//1
													typeof(string),//2
													typeof(string),//3
													typeof(string),//4
													typeof(string),//5
													typeof(string),//6
													typeof(string),//7
													typeof(string),//8
													typeof(string),//9
													typeof(int), // 10
													typeof(bool),//11
													typeof(string));//12
												
			lista_de_busqueda.Model = treeViewEngineproveedores;
			
			lista_de_busqueda.RulesHint = true;
				
			lista_de_busqueda.RowActivated += on_selecciona_provedor_clicked;  // Doble click selecciono paciente*/
			
			
			TreeViewColumn col_idproveedor = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idproveedor.Title = "ID Proveedores"; // titulo de la cabecera de la columna, si está visible
			col_idproveedor.PackStart(cellr0, true);
			col_idproveedor.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1
			//col_idproveedor.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_idproveedor.SortColumnId = (int) Col_proveedores.col_idproveedor;
			
			TreeViewColumn col_proveedor = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_proveedor.Title = "Proveedores";
			col_proveedor.PackStart(cellrt1, true);
			col_proveedor.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
			//col_proveedor.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_proveedor.SortColumnId = (int) Col_proveedores.col_proveedor;
			
			TreeViewColumn col_calle = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_calle.Title = "Calle";
			col_calle.PackStart(cellrt2, true);
			col_calle.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 3
			//col_calle.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_calle.SortColumnId = (int) Col_proveedores.col_calle;
			
			TreeViewColumn col_colonia = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_colonia.Title = "Colonia";
			col_colonia.PackStart(cellrt3, true);
			col_colonia.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 4
			//col_colonia.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_colonia.SortColumnId = (int) Col_proveedores.col_colonia;
			
            TreeViewColumn col_municipio = new TreeViewColumn();
            CellRendererText cellrt4 = new CellRendererText();
            col_municipio.Title = "Municipio";
            col_municipio.PackStart(cellrt4, true);
			col_municipio.AddAttribute(cellrt4,"text", 4); // la siguiente columna será 5
			//col_municipio.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_municipio.SortColumnId = (int) Col_proveedores.col_municipio;
			
            TreeViewColumn col_estado = new TreeViewColumn();
            CellRendererText cellrt5 = new CellRendererText();
            col_estado.Title = "Estado";
            col_estado.PackStart(cellrt5, true);
            col_estado.AddAttribute(cellrt5,"text", 5); // la siguiente columna será 6
            //col_estado.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_estado.SortColumnId = (int) Col_proveedores.col_estado;
			
            TreeViewColumn col_telefono = new TreeViewColumn();
            CellRendererText cellrt6 = new CellRendererText();
            col_telefono.Title = "Telefono";
            col_telefono.PackStart(cellrt6, true);
            col_telefono.AddAttribute(cellrt6,"text", 6); // la siguiente columna será 7
            //col_telefono.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
            col_telefono.SortColumnId = (int) Col_proveedores.col_telefono;
            
            TreeViewColumn col_contacto = new TreeViewColumn();
            CellRendererText cellrt7 = new CellRendererText();
            col_contacto.Title = "Contacto";
            col_contacto.PackStart(cellrt7, true);
            col_contacto.AddAttribute(cellrt7,"text", 7);// la siguiente columna será 8
            //col_contacto.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_contacto.SortColumnId = (int) Col_proveedores.col_contacto;
			
            TreeViewColumn col_cp = new TreeViewColumn();
            CellRendererText cellrt8 = new CellRendererText();
            col_cp.Title = "Codigo Postal";
            col_cp.PackStart(cellrt8, true);
            col_cp.AddAttribute(cellrt8,"text", 8);// la siguiente columna será 9
            //col_cp.SetCellDataFunc(cellrt8, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
            col_cp.SortColumnId = (int) Col_proveedores.col_cp;
			
            TreeViewColumn col_web = new TreeViewColumn();
            CellRendererText cellrt9 = new CellRendererText();
            col_web.Title = "Pag. Web";
            col_web.PackStart(cellrt9, true);
            col_web.AddAttribute(cellrt9,"text", 9);// la siguiente columna será 10
            //col_web.SetCellDataFunc(cellrt9, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
            col_web.SortColumnId = (int) Col_proveedores.col_web;
            		           
			lista_de_busqueda.AppendColumn(col_idproveedor);
			lista_de_busqueda.AppendColumn(col_proveedor);
			lista_de_busqueda.AppendColumn(col_calle);
			lista_de_busqueda.AppendColumn(col_colonia);
			lista_de_busqueda.AppendColumn(col_municipio);
			lista_de_busqueda.AppendColumn(col_estado);
			lista_de_busqueda.AppendColumn(col_telefono);
			lista_de_busqueda.AppendColumn(col_contacto);
			lista_de_busqueda.AppendColumn(col_cp);
			lista_de_busqueda.AppendColumn(col_web);
		}
		
		enum Col_proveedores
		{
			col_idproveedor,
			col_proveedor,
			col_calle,
			col_colonia,
			col_municipio,
			col_estado,
			col_telefono,
			col_contacto,
			col_cp,
			col_web
		}
		
		void on_llena_lista_proveedores(object sender, EventArgs args)
		{
			llenando_lista_de_proveedores();
		}
		
		void llenando_lista_de_proveedores()
		{
			treeViewEngineproveedores.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try
			{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				if ((string) entry_expresion.Text.ToUpper() == "*")
				{
					comando.CommandText = "SELECT descripcion_proveedor,direccion_proveedor,rfc_proveedor,curp_proveedor, "+
								"colonia_proveedor,municipio_proveedor,estado_proveedor,telefono1_proveedor, "+ 
								"telefono2_proveedor,celular_proveedor,cp_proveedor, proveedor_activo, "+
								"id_proveedor,contacto1_proveedor,mail_proveedor,pagina_web_proveedor,"+
								"osiris_erp_proveedores.id_forma_de_pago, descripcion_forma_de_pago AS descripago "+
								"FROM osiris_erp_proveedores, osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_proveedores.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
								//"AND osiris_erp_proveedores.agrupacion4 = ALM "+
								"ORDER BY descripcion_proveedor;";
				}else{
					comando.CommandText = "SELECT descripcion_proveedor,direccion_proveedor,rfc_proveedor,curp_proveedor, "+
								"colonia_proveedor,municipio_proveedor,estado_proveedor,telefono1_proveedor, "+ 
								"telefono2_proveedor,celular_proveedor,cp_proveedor, proveedor_activo, "+
								"id_proveedor,contacto1_proveedor,mail_proveedor,pagina_web_proveedor, "+
								"osiris_erp_proveedores.id_forma_de_pago, descripcion_forma_de_pago AS descripago "+
								"FROM osiris_erp_proveedores, osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_proveedores.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
								"AND descripcion_proveedor LIKE '%"+(string) entry_expresion.Text.ToUpper()+"%' "+
								//"AND osiris_erp_proveedores.agrupacion4 = ALM "+
								"ORDER BY descripcion_proveedor;";
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){	
					treeViewEngineproveedores.AppendValues ((int) lector["id_proveedor"],//0
												(string) lector["descripcion_proveedor"],//1
												(string) lector["direccion_proveedor"],//2
												(string) lector["colonia_proveedor"],//3
												(string) lector["municipio_proveedor"],//4
												(string) lector["estado_proveedor"],//5
												(string) lector["telefono1_proveedor"],//6
												(string) lector["contacto1_proveedor"],//7
												(string) lector["cp_proveedor"],//8
												(string) lector["pagina_web_proveedor"],//9
												(int) lector["id_forma_de_pago"],//10
												(bool) lector["proveedor_activo"], // 11
												(string) lector["descripago"]);//12
				
				}
			}catch (NpgsqlException ex){
   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_selecciona_provedor_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_busqueda.Selection.GetSelected(out model, out iterSelected)){
 				this.entry_provedor.Text = (string) model.GetValue(iterSelected, 1);
				provedor = (int) model.GetValue(iterSelected, 0); 
			}
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		void on_button_buscar_paciente_clicked(object sender, EventArgs args)
	    {
	    	tipo_busqueda = "pacientes";
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "busca_paciente", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda("paciente");
			button_buscar_busqueda.Clicked += new EventHandler(on_buscar_paciente_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_paciente_clicked);
			button_nuevo_paciente.Sensitive = false;
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
			entry_expresion.KeyPressEvent += onKeyPressEvent_entry_expresion;
		}
		
		void on_button_busca_medico_clicked(object sender, EventArgs args)
		{
			tipo_busqueda = "medicos";
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador_medicos", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          (this);
	        llenado_cmbox_tipo_busqueda();
			button_buscar_busqueda.Clicked += new EventHandler(on_button_llena_medicos_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_medico_clicked);
	        button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
	        entry_expresion.KeyPressEvent += onKeyPressEvent_entry_expresion;

			
			treeViewEngineMedicos = new TreeStore( typeof(int),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
									typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string));
			lista_de_medicos.Model = treeViewEngineMedicos;
			lista_de_medicos.RulesHint = true;
				
			lista_de_medicos.RowActivated += on_selecciona_medico_clicked;  // Doble click selecciono paciente
				
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
			
			lista_de_medicos.AppendColumn(col_idmedico);
			lista_de_medicos.AppendColumn(col_nomb1medico);
			lista_de_medicos.AppendColumn(col_nomb2medico);
			lista_de_medicos.AppendColumn(col_appmedico);
			lista_de_medicos.AppendColumn(col_apmmedico);
			lista_de_medicos.AppendColumn(col_espemedico);
		}
		
		enum Coldatos_medicos
		{
			col_idmedico,col_nomb1medico,col_nomb2medico,col_appmedico,	col_apmmedico,col_espemedico
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
			store1.AppendValues ("ID_MEDICO",7);
				              
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
			if(numbusqueda == 1)  { tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";}//	Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 2)  { tipobusqueda = "AND osiris_his_medicos.nombre2_medico LIKE '";}//	Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 3)  { tipobusqueda = "AND osiris_his_medicos.apellido_paterno_medico LIKE '";}//	Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 4)  { tipobusqueda = "AND osiris_his_medicos.apellido_materno_medico LIKE '";}//	Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 5)  { tipobusqueda = "AND osiris_his_medicos.cedula_medico LIKE '";}//	Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 6)  { tipobusqueda = "AND osiris_his_tipo_especialidad.descripcion_especialidad LIKE '";}//	Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 7)  { tipobusqueda = "AND osiris_his_medicos.id_medico LIKE '";}// Console.WriteLine(tipobusqueda); }
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
										"to_char(osiris_his_tipo_especialidad.id_especialidad,'999999') AS idespecialidad, "+
										"nombre1_medico,nombre2_medico,apellido_paterno_medico,apellido_materno_medico, "+
										"telefono1_medico,telefono2_medico,celular1_medico,celular2_medico,nextel_medico,beeper_medico,"+
										"descripcion_especialidad,medico_activo,autorizado "+
										"FROM osiris_his_medicos,osiris_his_tipo_especialidad "+
										"WHERE osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad "+
										"AND medico_activo = 'true' "+
										"ORDER BY id_medico;";
						}else{
							comando.CommandText = "SELECT id_medico, "+
										"to_char(osiris_his_tipo_especialidad.id_especialidad,'999999') AS idespecialidad, "+
										"nombre1_medico,nombre2_medico,apellido_paterno_medico,apellido_materno_medico, "+
										"telefono1_medico,telefono2_medico,celular1_medico,celular2_medico,nextel_medico,beeper_medico,"+
										"descripcion_especialidad,medico_activo,autorizado "+
										"FROM osiris_his_medicos,osiris_his_tipo_especialidad "+
										"WHERE osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad  "+
										"AND medico_activo = 'true' "+
								  		tipobusqueda+(string) entry_expresion.Text.Trim().ToUpper()+"%'  "+
										"ORDER BY id_medico;";
						}
						NpgsqlDataReader lector = comando.ExecuteReader ();
						//Console.WriteLine(comando.CommandText);
						
						while (lector.Read())
						{
							treeViewEngineMedicos.AppendValues ((int) lector["id_medico"],//0
										(string) lector["nombre1_medico"],//1
										(string) lector["nombre2_medico"],//2
										(string) lector["apellido_paterno_medico"],//3
										(string) lector["apellido_materno_medico"],//4
										(string) lector["descripcion_especialidad"],//5
										(string) lector["telefono1_medico"],//6
										(string) lector["telefono2_medico"],//7
										(string) lector["celular1_medico"],//8
										(string) lector["celular2_medico"],//9
										(string) lector["nextel_medico"],//10
										(string) lector["beeper_medico"]//11
										);
						}
					}catch (NpgsqlException ex){
			   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
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
				
	 	void on_selecciona_medico_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_medicos.Selection.GetSelected(out model, out iterSelected)){
 				nombmedico = (string) model.GetValue(iterSelected, 1)+" "+(string) model.GetValue(iterSelected, 2)+" "+
							(string) model.GetValue(iterSelected, 3)+" "+(string) model.GetValue(iterSelected,4);
			}
				
			entry_nombre_medico.Text = nombmedico;
			idmedicos = (int) model.GetValue(iterSelected, 0); 
				
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		void on_button_busca_producto_clicked(object sender, EventArgs args)
		{
			tipo_busqueda = "productos";
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda("producto");
			label_titulo_cantidad.Text = "Cantidad Autorizada";
			label_seleccion.Text = "Autorizada";
			
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
			
 			entry_expresion.KeyPressEvent += onKeyPressEvent_entry_expresion;
			entry_cantidad_aplicada.KeyPressEvent += onKeyPressEvent;

	    }
	    
	    void crea_treeview_busqueda(string tipo_busqueda)
		{
			if (tipo_busqueda == "paciente"){
			treeViewEngineBusca = new TreeStore(typeof(int),
													typeof(int),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));
													
				lista_de_Pacientes.Model = treeViewEngineBusca;
			
				lista_de_Pacientes.RulesHint = true;
			
				lista_de_Pacientes.RowActivated += on_selecciona_paciente_clicked;  // Doble click selecciono paciente*/

				TreeViewColumn col_foliodeatencion = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_foliodeatencion.Title = "Folio de Antencion"; // titulo de la cabecera de la columna, si está visible
				col_foliodeatencion.PackStart(cellr0, true);
				col_foliodeatencion.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
				col_foliodeatencion.SortColumnId = (int) Column.col_foliodeatencion;
			
				TreeViewColumn col_PidPaciente = new TreeViewColumn();
				CellRendererText cellr1 = new CellRendererText();
				col_PidPaciente.Title = "PID Paciente"; // titulo de la cabecera de la columna, si está visible
				col_PidPaciente.PackStart(cellr1, true);
				col_PidPaciente.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
				col_PidPaciente.SortColumnId = (int) Column.col_PidPaciente;
				//cellr0.Editable = true;   // Permite edita este campo
            
				TreeViewColumn col_Nombre1_Paciente = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_Nombre1_Paciente.Title = "Nombre 1";
				col_Nombre1_Paciente.PackStart(cellrt2, true);
				col_Nombre1_Paciente.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
				col_Nombre1_Paciente.SortColumnId = (int) Column.col_Nombre1_Paciente;
            
				TreeViewColumn col_Nombre2_Paciente = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_Nombre2_Paciente.Title = "Nombre 2";
				col_Nombre2_Paciente.PackStart(cellrt3, true);
				col_Nombre2_Paciente.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 2 en vez de 3
				col_Nombre2_Paciente.SortColumnId = (int) Column.col_Nombre2_Paciente;
            
				TreeViewColumn col_app_Paciente = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_app_Paciente.Title = "Apellido Paterno";
				col_app_Paciente.PackStart(cellrt4, true);
				col_app_Paciente.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 3 en vez de 4
				col_app_Paciente.SortColumnId = (int) Column.col_app_Paciente;
            
				TreeViewColumn col_apm_Paciente = new TreeViewColumn();
				CellRendererText cellrt5 = new CellRendererText();
				col_apm_Paciente.Title = "Apellido Materno";
				col_apm_Paciente.PackStart(cellrt5, true);
				col_apm_Paciente.AddAttribute (cellrt5, "text", 5); // la siguiente columna será 5 en vez de 6
				col_apm_Paciente.SortColumnId = (int) Column.col_apm_Paciente;
      
				TreeViewColumn col_fechanacimiento_Paciente = new TreeViewColumn();
				CellRendererText cellrt6 = new CellRendererText();
				col_fechanacimiento_Paciente.Title = "Fecha Nacimiento";
				col_fechanacimiento_Paciente.PackStart(cellrt6, true);
				col_fechanacimiento_Paciente.AddAttribute (cellrt6, "text", 6);     // la siguiente columna será 6 en vez de 7
				col_fechanacimiento_Paciente.SortColumnId = (int) Column.col_fechanacimiento_Paciente;
            
				TreeViewColumn col_edad_Paciente = new TreeViewColumn();
				CellRendererText cellrt7 = new CellRendererText();
				col_edad_Paciente.Title = "Edad";
				col_edad_Paciente.PackStart(cellrt7, true);
				col_edad_Paciente.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 7 en vez de 8
				col_edad_Paciente.SortColumnId = (int) Column.col_edad_Paciente;
            
				TreeViewColumn col_sexo_Paciente = new TreeViewColumn();
				CellRendererText cellrt8 = new CellRendererText();
				col_sexo_Paciente.Title = "Sexo";
				col_sexo_Paciente.PackStart(cellrt8, true);
				col_sexo_Paciente.AddAttribute (cellrt8, "text", 8); // la siguiente columna será 8 en vez de 9
				col_sexo_Paciente.SortColumnId = (int) Column.col_sexo_Paciente;
                        
				TreeViewColumn col_creacion_Paciente = new TreeViewColumn();
				CellRendererText cellrt9 = new CellRendererText();
				col_creacion_Paciente.Title = "Fecha creacion";
				col_creacion_Paciente.PackStart(cellrt9, true);
				col_creacion_Paciente.AddAttribute (cellrt9, "text", 9); // la siguiente columna será 8 en vez de 9
				col_creacion_Paciente.SortColumnId = (int) Column.col_creacion_Paciente;

				lista_de_Pacientes.AppendColumn(col_foliodeatencion);
				lista_de_Pacientes.AppendColumn(col_PidPaciente);
				lista_de_Pacientes.AppendColumn(col_Nombre1_Paciente);
				lista_de_Pacientes.AppendColumn(col_Nombre2_Paciente);
				lista_de_Pacientes.AppendColumn(col_app_Paciente);
				lista_de_Pacientes.AppendColumn(col_apm_Paciente);
				lista_de_Pacientes.AppendColumn(col_fechanacimiento_Paciente);
				lista_de_Pacientes.AppendColumn(col_edad_Paciente);
				lista_de_Pacientes.AppendColumn(col_sexo_Paciente);
				lista_de_Pacientes.AppendColumn(col_creacion_Paciente);
			}
			
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
				                                    typeof(string),
				                                    typeof(string),
				                                    typeof(string),
													typeof(string)
													);
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
            	
            	TreeViewColumn col_embalaje = new TreeViewColumn();
				CellRendererText cellrt13 = new CellRendererText();
				col_embalaje.Title = "embalaje";//Total
				col_embalaje.PackStart(cellrt13, true);
				col_embalaje.AddAttribute (cellrt13, "text", 13); // la siguiente columna será 3 en vez de 4
				col_embalaje.SortColumnId = (int) Column_prod.col_embalaje;
      
				
				
				lista_de_producto.AppendColumn(col_idproducto);  // 0
				lista_de_producto.AppendColumn(col_desc_producto); // 1
				lista_de_producto.AppendColumn(col_grupoprod);	//7
				lista_de_producto.AppendColumn(col_grupo1prod);	//8
				lista_de_producto.AppendColumn(col_grupo2prod);	//9	
				lista_de_producto.AppendColumn(col_embalaje);	//9	

			}
		}
			
		enum Column
		{
			col_foliodeatencion,
			col_PidPaciente,
			col_Nombre1_Paciente,
			col_Nombre2_Paciente,
			col_app_Paciente,
			col_apm_Paciente,
			col_fechanacimiento_Paciente,
			col_edad_Paciente,
			col_sexo_Paciente,
			col_creacion_Paciente
		}
		
		enum Column_prod
		{
			col_idproducto,
			col_desc_producto,
			col_grupoprod,
			col_grupo1prod,
			col_grupo2prod,
			col_embalaje
		}
		
		void on_buscar_paciente_clicked (object sender, EventArgs args)
		{
			llenando_lista_de_paciente();
		}
		
		void llenando_lista_de_paciente()
		{
			treeViewEngineBusca.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	               	
				if ((string) entry_expresion.Text.ToString() == ""){
					comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, "+
							" nombre1_paciente,nombre2_paciente, apellido_paterno_paciente, "+
							"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
							"sexo_paciente,to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
							"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
							"WHERE alta_paciente = 'false' "+
							"AND pagado = 'false' "+
							"AND cerrado = 'false' "+
							"AND reservacion = 'false' "+
							"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
							"AND osiris_erp_cobros_enca.alta_paciente = false "+
							"AND osiris_erp_cobros_enca.cancelado = false "+
							"ORDER BY folio_de_servicio;";
				}else{      
				
					if (radiobutton_busca_apellido.Active == true){
						comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, "+
							" nombre1_paciente,nombre2_paciente, apellido_paterno_paciente, "+
							"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
							"sexo_paciente,to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
							"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
							"WHERE alta_paciente = 'false' "+
							"AND pagado = 'false' "+
							"AND cerrado = 'false' "+
							"AND reservacion = 'false' "+
							"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
							"AND osiris_erp_cobros_enca.alta_paciente = false "+
							"AND osiris_erp_cobros_enca.cancelado = false "+
							"AND apellido_paterno_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' ORDER BY folio_de_servicio;";
					}
					if (radiobutton_busca_nombre.Active == true){
						comando.CommandText =  "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, "+
							" nombre1_paciente,nombre2_paciente, apellido_paterno_paciente, "+
							"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
							"sexo_paciente,to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
							"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
							"WHERE alta_paciente = 'false' "+
							"AND pagado = 'false' "+
							"AND cerrado = 'false' "+
							"AND reservacion = 'false' "+
							"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
							"AND osiris_erp_cobros_enca.alta_paciente = false "+
							"AND osiris_erp_cobros_enca.cancelado = false "+
							"AND nombre1_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' ORDER BY folio_de_servicio;";
					}
					if (radiobutton_busca_expediente.Active == true){
						comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, "+
							" nombre1_paciente,nombre2_paciente, apellido_paterno_paciente, "+
							"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
							"sexo_paciente,to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
							"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
							"WHERE alta_paciente = 'false' "+
							"AND pagado = 'false' "+
							"AND cerrado = 'false' "+
							"AND reservacion = 'false' "+
							"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
							"AND osiris_erp_cobros_enca.alta_paciente = false "+
							"AND osiris_erp_cobros_enca.cancelado = false "+
							"AND osiris_his_paciente.pid_paciente = '"+entry_expresion.Text+"' ORDER BY folio_de_servicio;";			
					}
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				while (lector.Read()){
					treeViewEngineBusca.AppendValues ((int) lector["folio_de_servicio"],//TreeIter iter =
										(int) lector["pid_paciente"],
										(string) lector["nombre1_paciente"],(string) lector["nombre2_paciente"],
										(string) lector["apellido_paterno_paciente"], (string) lector["apellido_materno_paciente"],
										(string) lector["fech_nacimiento"], (string) lector["edad"],
										(string) lector["sexo_paciente"],
										(string) lector["fech_creacion"]);
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
		
		void on_selecciona_paciente_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;

 			if (lista_de_Pacientes.Selection.GetSelected(out model, out iterSelected)){
 				 folioservicio = (int) model.GetValue(iterSelected, 0);
 				 entry_folio_servicio.Text = folioservicio.ToString();
 				 //llenado_de_productos_aplicados(folioservicio.ToString());
 				 nombres = ((string) model.GetValue(iterSelected, 2) +" "+ (string) model.GetValue(iterSelected, 3) +" "+
 				 (string) model.GetValue(iterSelected, 4) +" "+ (string) model.GetValue(iterSelected, 5));
 				 entry_nombre_paciente.Text = nombres.ToString();
 				 
 			}
 			// cierra la ventana despues que almaceno la informacion en variables
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
 		}
		
		// llena la lista de productos
 		void on_llena_lista_producto_clicked (object sender, EventArgs args)
 		{
 			llenando_lista_de_productos();
 		}
 		
 		void llenando_lista_de_productos()
 		{
 			treeViewEngineBusca2.Clear();
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
						"osiris_productos.descripcion_producto,"+
						"to_char(precio_producto_publico,'999999999.99') AS preciopublico,"+//
						"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,"+
						"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,"+
						"to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+//
						"to_char(porcentage_ganancia,'99999.99') AS porcentageutilidad,"+//
						"to_char(precio_producto_publico,'999999999.99') AS preciopublico_cliente,"+
						"to_char(cantidad_de_embalaje,'999999.99') AS embalaje,"+
						"to_char(costo_producto,'999999999.99') AS costoproducto "+
						"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
						"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
						"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
						"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
						"AND osiris_grupo_producto.id_grupo_producto IN('4','5') "+
						"AND osiris_productos.cobro_activo = 'true' "+
						"AND osiris_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_producto;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				float tomaprecio;
				float calculodeiva;
				float preciomasiva;
				float tomadescue;
				float preciocondesc;
						
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
					treeViewEngineBusca2.AppendValues (//TreeIter iter =
											(string) lector["codProducto"],//0
											(string) lector["descripcion_producto"],//1
											(string) lector["descripcion_grupo_producto"],//2
											(string) lector["descripcion_grupo1_producto"],//3
											(string) lector["descripcion_grupo2_producto"],//4
											tomaprecio.ToString("F").PadLeft(10), //2-5
											calculodeiva.ToString("F").PadLeft(10),//3-6
											preciomasiva.ToString("F").PadLeft(10),//4-7
											(string) lector["porcentagesdesc"],//8
											preciocondesc.ToString("F").PadLeft(10),//9
											(string) lector["costoproductounitario"],//10
											(string) lector["porcentageutilidad"],//11
											(string) lector["costoproducto"],//12
											(string) lector["embalaje"],//13
					                        (string) lector["preciopublico"],//14
					                        (string) lector["costoproductounitario"],//15
					                        (string) lector["porcentageutilidad"]);//16
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

 			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
 					cant_embalaje = float.Parse((string)model.GetValue(iterSelected, 13));
					aplicada = entry_cantidad_aplicada.Text;	
					total_surtido = float.Parse((string)aplicada) * cant_embalaje;

				if (float.Parse(entry_cantidad_aplicada.Text) > 0){
					this.treeViewEngineAutorizados.AppendValues (entry_cantidad_aplicada.Text,
																(string) model.GetValue(iterSelected, 1),
																(string) model.GetValue(iterSelected, 2),
																(string) model.GetValue(iterSelected, 0),
																"",
																(string) model.GetValue(iterSelected, 15),
																(string) model.GetValue(iterSelected, 14),
																(string) model.GetValue(iterSelected, 16),
																"",
																"",
																"",
																"",
																"",
																(string) model.GetValue(iterSelected, 13),
																total_surtido.ToString("F").PadLeft(10));
																
					entry_cantidad_aplicada.Text = "0";
					entry_desc_producto.Text = (string) model.GetValue(iterSelected, 1);
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, 
					ButtonsType.Close, "La cantidad que quiere aplicar NO \n"+
									"puede quedar vacia o en cero, intente de nuevo...");
					msgBoxError.Run ();
					msgBoxError.Destroy();
				}
 			}
 		}
		
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_entry_expresion(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				if(tipo_busqueda == "productos"){
					llenando_lista_de_productos();
				}
				if(tipo_busqueda == "proveedores"){
					llenando_lista_de_proveedores();
				}
				if(tipo_busqueda == "medicos"){
					this.llenando_lista_de_medicos();
				}
				if(tipo_busqueda == "pacientes"){
					this.llenando_lista_de_paciente();
				}
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}
		
		void crea_treview_autorizacion()
		{
			treeViewEngineAutorizados = new TreeStore(typeof(string),
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
													
			lista_de_autorizacion.Model = treeViewEngineAutorizados;
			lista_de_autorizacion.RulesHint = true;
			
			//lista_de_autorizacion.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono paciente*/
				
			TreeViewColumn col_cant_autorizada = new TreeViewColumn();
			CellRendererText cel_cant_autorizada = new CellRendererText();
			col_cant_autorizada.Title = "Cant.Autorizada";
			col_cant_autorizada.PackStart(cel_cant_autorizada, true);
			col_cant_autorizada.AddAttribute (cel_cant_autorizada, "text", 0);
			
			TreeViewColumn col_desc_producto = new TreeViewColumn();
			CellRendererText cel_desc_producto = new CellRendererText();
			col_desc_producto.Title = "Descripcion de Producto";
			col_desc_producto.PackStart(cel_desc_producto, true);
			col_desc_producto.AddAttribute(cel_desc_producto, "text", 1);
			col_desc_producto.Resizable = true;
			
			TreeViewColumn col_presentacion_producto = new TreeViewColumn();
			CellRendererText cel_presentacion_producto = new CellRendererText();
			col_presentacion_producto.Title = "Presentacion del Producto";
			col_presentacion_producto.PackStart(cel_presentacion_producto, true);
			col_presentacion_producto.AddAttribute(cel_presentacion_producto, "text", 2);
			
			TreeViewColumn col_codigo_producto = new TreeViewColumn();
			CellRendererText cel_codigo_producto = new CellRendererText();
			col_codigo_producto.Title = "Codigo Producto";
			col_codigo_producto.PackStart(cel_codigo_producto, true);
			col_codigo_producto.AddAttribute(cel_codigo_producto, "text", 3);
		
			TreeViewColumn col_embalaje = new TreeViewColumn();
			CellRendererText cel_embalaje = new CellRendererText();
			col_embalaje.Title = "Embalaje";
			col_embalaje.PackStart(cel_embalaje, true);
			col_embalaje.AddAttribute(cel_embalaje, "text", 13);
			
			TreeViewColumn col_tot_surtir = new TreeViewColumn();
			CellRendererText cel_tot_surtir = new CellRendererText();
			col_tot_surtir.Title = "Total a surtir";
			col_tot_surtir.PackStart(cel_tot_surtir, true);
			col_tot_surtir.AddAttribute(cel_tot_surtir, "text", 14);
				
			lista_de_autorizacion.AppendColumn(col_cant_autorizada);
			lista_de_autorizacion.AppendColumn(col_desc_producto);
			lista_de_autorizacion.AppendColumn(col_presentacion_producto);
			lista_de_autorizacion.AppendColumn(col_codigo_producto);
			lista_de_autorizacion.AppendColumn(col_embalaje);
			lista_de_autorizacion.AppendColumn(col_tot_surtir);
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}

