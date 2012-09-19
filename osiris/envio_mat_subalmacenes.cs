// created on 27/01/2008 at 01:55 p
///////////////////////////////////////////////////////////
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
//				  Daniel Olivares - arcangeldoc@gmail.com (Diseño de Pantallas Glade)
// 				  Ing. Jesus Buentello Garza (Programacion y reporte)
//
// Licencia		: GLP
// S.O. 		: GNU/Linux
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
// Proposito	:
// Objeto		:
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class envio_de_materiales_subalmacenes
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		
		// Declarando ventana principal surtimiento
		[Widget] Gtk.Window envia_materiales_subalmacen;
		[Widget] Gtk.TreeView lista_de_materiales_solicitados;
		[Widget] Gtk.ComboBox combobox_sub_almacenes;
		[Widget] Gtk.ComboBox combobox_almacen_origen;
		[Widget] Gtk.CheckButton checkbutton_stock_almacen = null;
		[Widget] Gtk.CheckButton checkbutton_stock_paciente = null;
		[Widget] Gtk.Entry entry_folio_servicio = null;
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_nombre_paciente = null;
		[Widget] Gtk.Button button_busca_paciente = null;
		[Widget] Gtk.Button button_surtir_materiales;
		[Widget] Gtk.Button button_sin_stock;
		[Widget] Gtk.Button button_pedido_erroneo;
		[Widget] Gtk.Entry entry_fecha_solicitud;
		[Widget] Gtk.Entry entry_desc_producto;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.Button button_quitar_productos;
		[Widget] Gtk.Button button_rpt_surtido;
		[Widget] Gtk.RadioButton radiobutton_todas_solici = null;
		[Widget] Gtk.RadioButton radiobutton_solic_paciente = null;
		[Widget] Gtk.RadioButton radiobutton_solic_stock = null;
		[Widget] Gtk.RadioButton radiobutton_pre_solic = null;
				
		[Widget] Gtk.CheckButton checkbutton_envio_directo;
				
		string connectionString;
		string nombrebd;				
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;		
		
		// Variables publica
		float valoriva;
		int idsubalmacen = 0;
		string descsubalmacen = "";
		int idalmacenorigen = 0;
		string filtro_sub_almacen = "";
		bool cargo_automatico = false;
		
		/////// Ventana Busqueda de productos\\\\\\\\
		[Widget] Gtk.TreeView lista_de_producto;
		//[Widget] Gtk.Button button_agrega_extra;
		[Widget] Gtk.Entry entry_cantidad_aplicada;
		[Widget] Gtk.Label label_titulo_cantidad;
		
		//private TreeStore treeViewEngineBusca;
		private TreeStore treeViewEngineBusca2;
		private ListStore treeViewEngineSolicitado;
		
		TreeViewColumn col_envios00;		CellRendererToggle cellrt00;
		TreeViewColumn col_envios01;		CellRendererText cellrt01;
		TreeViewColumn col_envios02;		CellRendererText cellrt02;
		TreeViewColumn col_envios03;		CellRendererText cellrt03;
		TreeViewColumn col_envios04;		CellRendererText cellrt04;
		TreeViewColumn col_envios05;		CellRendererText cellrt05;
		TreeViewColumn col_envios06;		CellRendererText cellrt06;
		TreeViewColumn col_envios10;		CellRendererText cellrt10;
		TreeViewColumn col_envios11;		CellRendererText cellrt11;
		TreeViewColumn col_envios12;		CellRendererText cellrt12;
		TreeViewColumn col_envios13;		CellRendererToggle cellrt13;
		TreeViewColumn col_envios14;		CellRendererToggle cellrt14;
		TreeViewColumn col_envios15;		CellRendererText cellrt15;
		TreeViewColumn col_envios18;		CellRendererText cellrt18;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();
		
		public envio_de_materiales_subalmacenes(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			valoriva = float.Parse(classpublic.ivaparaaplicar);
									
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "envia_materiales_subalmacen", null);
			gxml.Autoconnect (this);
			////// Muestra ventana de Glade
			envia_materiales_subalmacen.Show();			
			button_surtir_materiales.Clicked += new EventHandler(on_button_surtir_materiales_clicked);
			button_sin_stock.Clicked += new EventHandler(on_button_sin_stock_clicked);
			button_pedido_erroneo.Clicked += new EventHandler(on_button_pedido_erroneo_clicked);
			// Busqueda de Productos
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			// Quitar productos editar tiene que estar en true
			button_quitar_productos.Clicked += new EventHandler(on_button_quitar_productos_clicked);
			button_rpt_surtido.Clicked += new EventHandler(on_button_rpt_surtido_clicked);
			checkbutton_stock_paciente.Clicked += new EventHandler(on_checkbutton_stock_paciente_clicked);
			checkbutton_stock_almacen.Clicked += new EventHandler(on_checkbutton_stock_almacen_clicked);
			//buscar pacientes
			button_busca_paciente.Clicked += new EventHandler(on_button_busca_paciente_clicked);
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);			
			// envio directo a sub-almacenes
			checkbutton_envio_directo.Clicked += new EventHandler(on_checkbutton_envio_directo_clicked);
			// Radiobutton
			radiobutton_todas_solici.Clicked += new EventHandler(on_radiobutton_filtros_clicked);
			radiobutton_solic_paciente.Clicked += new EventHandler(on_radiobutton_filtros_clicked);
			radiobutton_solic_stock.Clicked += new EventHandler(on_radiobutton_filtros_clicked);
			radiobutton_pre_solic.Clicked += new EventHandler(on_radiobutton_filtros_clicked);
			
			entry_desc_producto.Sensitive = false;
			button_busca_producto.Sensitive = false;
			button_quitar_productos.Sensitive = false;
			checkbutton_stock_almacen.Sensitive = false;
			checkbutton_stock_paciente.Sensitive = false;
			entry_folio_servicio.Sensitive = false;
			entry_pid_paciente.Sensitive = false;
			entry_nombre_paciente.Sensitive = false;
			button_busca_paciente.Sensitive = false;
			// Llenado de combobox1 
			combobox_sub_almacenes.Clear();
			combobox_almacen_origen.Clear();
			CellRendererText cell1 = new CellRendererText();
			
			combobox_sub_almacenes.PackStart(cell1, true);
			combobox_almacen_origen.PackStart(cell1, true);
			
			combobox_sub_almacenes.AddAttribute(cell1,"text",0);
			combobox_almacen_origen.AddAttribute(cell1,"text",0);
			
			ListStore store2 = new ListStore( typeof (string), typeof (int));
			ListStore store3 = new ListStore( typeof (string), typeof (int));
			
			combobox_sub_almacenes.Model = store2;
			combobox_almacen_origen.Model = store3;
							        
			// lleno de la tabla de his_tipo_de_admisiones
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT id_almacen,descripcion_almacen,sub_almacen,almacen_salidas,cargo_directo FROM osiris_almacenes "+
               						//"WHERE sub_almacen = 'true'  "+
               						"ORDER BY descripcion_almacen;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				store2.AppendValues ("",0);
				store3.AppendValues ("",0);
               	while (lector.Read()){
					if((bool) lector["sub_almacen"] == true){
						store2.AppendValues ((string) lector["descripcion_almacen"], (int) lector["id_almacen"]);
					}
					if((bool) lector["almacen_salidas"] == true){
						cargo_automatico = (bool) lector["cargo_directo"]; 
						store3.AppendValues ((string) lector["descripcion_almacen"], (int) lector["id_almacen"]);
					}
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
			combobox_almacen_origen.Changed += new EventHandler (onComboBoxChanged_almacen_origen);
			crea_treeview_envio_materiales();
		}
		
		void on_radiobutton_filtros_clicked(object obj, EventArgs args)
		{
			Gtk.RadioButton radiobutton_filtros = (Gtk.RadioButton) obj;
			if(radiobutton_filtros.Name.ToString() == "radiobutton_todas_solici"){
				filtro_sub_almacen = "";
				llenado_de_material_solicitado();
				entry_folio_servicio.Sensitive = false;
				entry_pid_paciente.Sensitive = false;
				entry_nombre_paciente.Sensitive = false;
				button_busca_paciente.Sensitive = false;
			}
			if(radiobutton_filtros.Name.ToString() == "radiobutton_solic_paciente"){
				filtro_sub_almacen = " AND solicitud_stock = 'false' AND pre_solicitud = 'false' ";
				llenado_de_material_solicitado();
				entry_folio_servicio.Sensitive = false;
				entry_pid_paciente.Sensitive = false;
				entry_nombre_paciente.Sensitive = false;
				button_busca_paciente.Sensitive = false;
			}
			if(radiobutton_filtros.Name.ToString() == "radiobutton_solic_stock"){
				filtro_sub_almacen = " AND solicitud_stock = 'true' AND pre_solicitud = 'false' ";
				llenado_de_material_solicitado();
				entry_folio_servicio.Sensitive = false;
				entry_pid_paciente.Sensitive = false;
				entry_nombre_paciente.Sensitive = false;
				button_busca_paciente.Sensitive = false;
			}
			if(radiobutton_filtros.Name.ToString() == "radiobutton_pre_solic"){
				filtro_sub_almacen = " AND solicitud_stock = 'false' AND pre_solicitud = 'true' ";
				llenado_de_material_solicitado();
				entry_folio_servicio.Sensitive = true;
				entry_pid_paciente.Sensitive = true;
				entry_nombre_paciente.Sensitive = true;
				button_busca_paciente.Sensitive = true;
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
		    	descsubalmacen = (string) combobox_sub_almacenes.Model.GetValue(iter,0);
		    	if (checkbutton_envio_directo.Active == false){
					//Console.WriteLine("selecciona sub almacen");
					llenado_de_material_solicitado();
				}
	     	}
		}
		
		void onComboBoxChanged_almacen_origen(object sender, EventArgs args)
		{
			ComboBox combobox_almacen_origen = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_almacen_origen.GetActiveIter (out iter)){
		    	idalmacenorigen = (int) combobox_almacen_origen.Model.GetValue(iter,1);
	     	}
		}
		
		void on_button_rpt_surtido_clicked(object sender, EventArgs args)
		{
			// rpt_envio_mat_almacenes.cs
			new osiris.rpt_envio_almacen(this.nombrebd,this.idsubalmacen);
		}
		
		void on_button_busca_producto_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			
			crea_treeview_busqueda("producto");
			
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
			entry_expresion.KeyPressEvent += onKeyPressEvent_entry_expresion;
			
			label_titulo_cantidad.Text = "Cantidad Solicitada";	
			
			// Validando que sen solo numeros
			entry_cantidad_aplicada.KeyPressEvent += onKeyPressEvent;
	    }
		
		void on_button_surtir_materiales_clicked(object sender, EventArgs args)
		{
			TreeIter iterSelected;
 			if (treeViewEngineSolicitado.GetIterFirst (out iterSelected)){
 				if (idsubalmacen != 0){
		 			if (idalmacenorigen	!= 0){
		 				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de el traspasar los Materiales Seleccionados?");
						ResponseType miResultado = (ResponseType)msgBox.Run ();
						msgBox.Destroy();
						if (miResultado == ResponseType.Yes){
		 					if ((bool) lista_de_materiales_solicitados.Model.GetValue (iterSelected,0) == true){
								// que la cantidad sea mayor a cero validando lo autorizado
			 					if (decimal.Parse((string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)) > 0 ){
					 				if((string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,10) != "0"){
										if((bool) cargo_automatico == true){
											insert_a_procedimiento((string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,10),
																	(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,5),
																	(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,11),
																	(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2),
																	(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,16),
																	(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,17)
																	);
										}
									}
									//Console.WriteLine("akkaaa");
					 				NpgsqlConnection conexion;
									conexion = new NpgsqlConnection (connectionString+nombrebd);
					 				try{
										conexion.Open ();
										NpgsqlCommand comando; 
										comando = conexion.CreateCommand();
										comando.CommandText = "SELECT id_producto,id_almacen,stock FROM osiris_catalogo_almacenes "+
															"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
															"AND eliminado = 'false' "+														
															"AND id_producto = '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,5)+"' ;";
										//Console.WriteLine(comando.CommandText);
										NpgsqlDataReader lector = comando.ExecuteReader ();
										if(lector.Read()){
											//Console.WriteLine("if ");
											NpgsqlConnection conexion1; 
											conexion1 = new NpgsqlConnection (connectionString+nombrebd);
					 						try{
												conexion1.Open ();
												NpgsqlCommand comando1;
												comando1 = conexion1.CreateCommand();
												// verificando que asigne un paciente para que la pre-solicitud se pueda surtir
												
												comando1.CommandText = "UPDATE osiris_catalogo_almacenes SET stock  = stock + '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"',"+
																		//"historial_surtido_material = historial_surtido_material || '"+LoginEmpleado+" "+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n',"+
																		"fechahora_ultimo_surtimiento = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
																		"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																		"AND id_producto = '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,5)+"' ;";
												//Console.WriteLine(comando1.CommandText);
												comando1.ExecuteNonQuery();
												
												comando1.Dispose();
												conexion1.Close();
											}catch (NpgsqlException ex){
									   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
															MessageType.Error, 
															ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
												msgBoxError.Run ();
											}
											conexion1.Close();	
											Console.WriteLine(idsubalmacen+"  "+idalmacenorigen);
											if(this.checkbutton_envio_directo.Active == true && idsubalmacen != idalmacenorigen){
												NpgsqlConnection conexion2;
                                            	conexion2 = new NpgsqlConnection (connectionString+nombrebd);
                                             	try{
	                                                conexion2.Open ();
	                                                NpgsqlCommand comando2;
	                                                comando2 = conexion2.CreateCommand();
	                                                comando2.CommandText = "UPDATE osiris_catalogo_almacenes SET stock  = stock - '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"' "+
	                                                                        //"historial_surtido_material = historial_surtido_material || '"+LoginEmpleado+" "+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n',"+
	                                                                        //"fechahora_ultimo_surtimiento = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
	                                                                        "WHERE id_almacen = '"+idalmacenorigen+"' "+
	                                                                        "AND id_producto = '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,5)+"' ;";
	                                                //Console.WriteLine("Actualiza el Producto");
	                                                											Console.WriteLine("entra "+comando2.CommandText);
	                                                											
	                                                comando2.ExecuteNonQuery();
	                                                comando2.Dispose();
	                                                conexion2.Close();
	                                            }catch (NpgsqlException ex){
	                                                   MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
	                                                            MessageType.Error,
	                                                            ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
	                                                msgBoxError.Run ();
	                                            }
	                                            conexion2.Close();    
                                            }											
										}else{
											//Console.WriteLine("else"+idalmacenorigen);
											NpgsqlConnection conexion1; 
											conexion1 = new NpgsqlConnection (connectionString+nombrebd);
					 						try{
												conexion1.Open ();
												NpgsqlCommand comando1; 
												comando1 = conexion1.CreateCommand();
												comando1.CommandText = "INSERT INTO osiris_catalogo_almacenes("+
																		"id_almacen,"+
																		"id_producto,"+
																		"stock,"+
																		"id_quien_creo,"+
																		"fechahora_alta,"+
																		"fechahora_ultimo_surtimiento)"+
																		"VALUES ('"+
																		this.idsubalmacen.ToString()+"','"+
																		(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,5)+"','"+
																		(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"','"+
																		this.LoginEmpleado+"','"+
																		DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																		DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');";
												comando1.ExecuteNonQuery();
												comando1.Dispose();
												//Console.WriteLine("inserta el producto");
												
												if(this.checkbutton_envio_directo.Active == true && idsubalmacen != idalmacenorigen){
													NpgsqlConnection conexion2;
                                            		conexion2 = new NpgsqlConnection (connectionString+nombrebd);
                                             		try{
	                                                	conexion2.Open ();
	                                                	NpgsqlCommand comando2;
	                                                	comando2 = conexion2.CreateCommand();
	                                                	comando2.CommandText = "UPDATE osiris_catalogo_almacenes SET stock  = stock - '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"' "+
	                                                                        //"historial_surtido_material = historial_surtido_material || '"+LoginEmpleado+" "+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n',"+
	                                                                        //"fechahora_ultimo_surtimiento = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
	                                                                        "WHERE id_almacen = '"+idalmacenorigen+"' "+
	                                                                        "AND id_producto = '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,5)+"' ;";
	                                                	//Console.WriteLine("Actualiza el Producto");
	                                                	//Console.WriteLine("entra "+comando2.CommandText);
	                                                											
	                                                	comando2.ExecuteNonQuery();
	                                                	comando2.Dispose();
	                                                	conexion2.Close();
	                                            	}catch (NpgsqlException ex){
	                                                   MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
	                                                            MessageType.Error,
	                                                            ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
	                                                msgBoxError.Run ();
	                                            	}
	                                            	conexion2.Close();    
                                            	}											
											}catch (NpgsqlException ex){
									   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
															MessageType.Error, 
															ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
												msgBoxError.Run ();
												msgBoxError.Destroy();
											}
											conexion1.Close();																																															
										}										
										if (checkbutton_envio_directo.Active == false){
											if((bool) lista_de_materiales_solicitados.Model.GetValue (iterSelected,14) == true){
												if(int.Parse((string) entry_folio_servicio.Text.ToString()) != 0){
													comando.CommandText = "UPDATE osiris_his_solicitudes_deta SET cantidad_autorizada = '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"',"+
															"fechahora_autorizado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
															"id_quien_autorizo = '"+this.LoginEmpleado+"',"+
															//"stock_cuando_solicito = '"++"',"+
															"id_almacen_origen = '"+idalmacenorigen.ToString().Trim()+"',"+
															"surtido = 'true', "+
															"folio_de_servicio = '"+(string) entry_folio_servicio.Text.ToString()+"',"+
															"pid_paciente = '"+(string) entry_pid_paciente.Text.ToString()+"' "+
															"WHERE id_secuencia =  '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,7)+"';";
													//Console.WriteLine(comando.CommandText);
													comando.ExecuteNonQuery();
													if((bool) cargo_automatico == true){
														insert_a_procedimiento((string) entry_folio_servicio.Text.ToString(),
																			(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,5),
																			(string) entry_pid_paciente.Text.ToString(),
																			(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2),
																			(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,16),
																			(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,17));
													}
												}else{
													MessageDialog msgBox3 = new MessageDialog (MyWin,DialogFlags.Modal,
															MessageType.Info,ButtonsType.Ok,"Debe elegir al paciente para hacer su envio, verifique...");										
															msgBox3.Run ();	msgBox3.Destroy();
													
												}
											}else{
												comando.CommandText = "UPDATE osiris_his_solicitudes_deta SET cantidad_autorizada = '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"',"+
													"fechahora_autorizado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
													"id_quien_autorizo = '"+this.LoginEmpleado+"',"+
														//"stock_cuando_solicito = '"++"',"+
													"id_almacen_origen = '"+idalmacenorigen.ToString().Trim()+"',"+
													"surtido = 'true' "+
													"WHERE id_secuencia =  '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,7)+"';";
												//Console.WriteLine(comando.CommandText);
												comando.ExecuteNonQuery();
											}
											comando.Dispose();
										}
																				
										// VERIFICA CUANDO ES ENVIO DIRECTO DESDE UN ALAMCEN
										// Actualiza la tabla de soliciudes
										if (checkbutton_envio_directo.Active == true){
											NpgsqlConnection conexion4; 
											conexion4 = new NpgsqlConnection (connectionString+nombrebd);
											try{
												conexion4.Open ();
												NpgsqlCommand comando4; 
												comando4 = conexion4.CreateCommand ();
												comando4.CommandText = "INSERT INTO osiris_his_solicitudes_deta("+
																								"folio_de_solicitud,"+
																								"id_producto,"+
																								"precio_producto_publico,"+
																								"costo_por_unidad,"+
																								"cantidad_solicitada,"+
																								"cantidad_autorizada,"+
																								"fechahora_solicitud,"+
																								"id_quien_autorizo,"+
																								"id_almacen,"+
																								"envio_directo,"+
																								"surtido,"+
																								"fechahora_autorizado,"+
																								"id_almacen_origen,"+
																								"status ) "+
																								"VALUES ("+																							
																								"0,'"+
																								(string) this.lista_de_materiales_solicitados.Model.GetValue(iterSelected,5)+"','"+
																								(string) this.lista_de_materiales_solicitados.Model.GetValue(iterSelected,9)+"','"+
																								(string) this.lista_de_materiales_solicitados.Model.GetValue(iterSelected,8)+"','"+
																								(string) this.lista_de_materiales_solicitados.Model.GetValue(iterSelected,1)+"','"+
																								(string) this.lista_de_materiales_solicitados.Model.GetValue(iterSelected,2)+"','"+
																								DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																								LoginEmpleado+"','"+
																								this.idsubalmacen.ToString()+"','"+
																								"true','"+
																								"true','"+
																								DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																								this.idalmacenorigen.ToString()+"','"+
																								"true');";
												//Console.WriteLine(comando4.CommandText);
												comando4.ExecuteNonQuery();
												comando4.Dispose();
											}catch (NpgsqlException ex){
												MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																			MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
												msgBoxError.Run ();				msgBoxError.Destroy();
											}
											conexion4.Close();
										}
									}catch (NpgsqlException ex){
									   	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
															MessageType.Error, 
															ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
										msgBoxError.Run ();
										msgBoxError.Destroy();
									}
									conexion.Close();
								}
							}
									
							while (treeViewEngineSolicitado.IterNext(ref iterSelected)){
								if ((bool)lista_de_materiales_solicitados.Model.GetValue (iterSelected,0) == true){
									if (decimal.Parse((string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)) > 0 ){
										if((string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,10) != "0"){
											if((bool) cargo_automatico == true){
												insert_a_procedimiento((string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,10),
													(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,5),
													(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,11),
													(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2),
													(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,16),
													(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,17));
											}
										}
										NpgsqlConnection conexion2; 
										conexion2 = new NpgsqlConnection (connectionString+nombrebd);
							 			try{
											conexion2.Open ();
											NpgsqlCommand comando2; 
											comando2 = conexion2.CreateCommand();
											comando2.CommandText = "SELECT id_producto,id_almacen FROM osiris_catalogo_almacenes "+
																	"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																	"AND eliminado = 'false' "+	
																	"AND id_producto = '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,5)+"' ;";
											//Console.WriteLine("Busca el Producto");	
											NpgsqlDataReader lector2 = comando2.ExecuteReader ();
											if(lector2.Read()){
												NpgsqlConnection conexion3; 
												conexion3 = new NpgsqlConnection (connectionString+nombrebd);
							 					try{
													conexion3.Open ();
													NpgsqlCommand comando3; 
													comando3 = conexion3.CreateCommand();
													comando3.CommandText = "UPDATE osiris_catalogo_almacenes SET stock  = stock + '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"',"+
																			//"historial_surtido_material = historial_surtido_material || '"+LoginEmpleado+" "+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n',"+
																			"fechahora_ultimo_surtimiento = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
																			"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																			"AND id_producto = '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,5)+"' ;";
													//Console.WriteLine("actualice el Producto");		
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
												
												//Console.WriteLine(idsubalmacen+" .2. "+idalmacenorigen);
												if(this.checkbutton_envio_directo.Active == true && idsubalmacen != idalmacenorigen){

													NpgsqlConnection conexion;
		                                            conexion = new NpgsqlConnection (connectionString+nombrebd);
		                                             try{
		                                                conexion.Open ();
		                                                NpgsqlCommand comando;
		                                                comando = conexion.CreateCommand();
		                                                comando.CommandText = "UPDATE osiris_catalogo_almacenes SET stock  = stock - '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"' "+
		                                                                        //"historial_surtido_material = historial_surtido_material || '"+LoginEmpleado+" "+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n',"+
		                                                                        //"fechahora_ultimo_surtimiento = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
		                                                                        "WHERE id_almacen = '"+idalmacenorigen+"' "+
		                                                                        "AND id_producto = '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,5)+"' ;";
		                                                //Console.WriteLine("Actualiza el Producto");
		                                                //Console.WriteLine("entra "+comando.CommandText);
		                                                											
		                                                comando.ExecuteNonQuery();
		                                                comando.Dispose();
		                                                conexion.Close();
		                                            }catch (NpgsqlException ex){
		                                                   MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
		                                                            MessageType.Error,
		                                                            ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
		                                                msgBoxError.Run ();
		                                            }
		                                            conexion.Close();
												}
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
																				"fechahora_ultimo_surtimiento)"+
																				"VALUES ('"+
																				this.idsubalmacen.ToString()+"','"+
																				(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,5)+"','"+
																				(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"','"+
																				this.LoginEmpleado+"','"+
																				DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																				DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');";
													//Console.WriteLine("inserta el Producto");	
													comando3.ExecuteNonQuery();
													comando3.Dispose();		
													
													if(this.checkbutton_envio_directo.Active == true && idsubalmacen != idalmacenorigen){
														NpgsqlConnection conexion5;
		                                            	conexion5 = new NpgsqlConnection (connectionString+nombrebd);
		                                             	try{
			                                                conexion5.Open ();
			                                                NpgsqlCommand comando5;
			                                                comando5 = conexion5.CreateCommand();
			                                                comando5.CommandText = "UPDATE osiris_catalogo_almacenes SET stock  = stock - '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"' "+
			                                                                        //"historial_surtido_material = historial_surtido_material || '"+LoginEmpleado+" "+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n',"+
			                                                                        //"fechahora_ultimo_surtimiento = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
			                                                                        "WHERE id_almacen = '"+idalmacenorigen+"' "+
			                                                                        "AND id_producto = '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,5)+"' ;";
			                                                //Console.WriteLine("Actualiza el Producto");
			                                                											Console.WriteLine("entra "+comando2.CommandText);
			                                                											
			                                                comando5.ExecuteNonQuery();
			                                                comando5.Dispose();
			                                                conexion5.Close();
			                                            }catch (NpgsqlException ex){
			                                                   MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
			                                                            MessageType.Error,
			                                                            ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
			                                                msgBoxError.Run ();
			                                            }
			                                            conexion5.Close();    
	                                            	}																
												}catch (NpgsqlException ex){
										   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																MessageType.Error, 
																	ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
													msgBoxError.Run ();
													msgBoxError.Destroy();
												}
												conexion3.Close();														
											}
											
											if (this.checkbutton_envio_directo.Active == false){
												// valida que la pre-solicitud tenga asignado un paciente
												if((bool) lista_de_materiales_solicitados.Model.GetValue (iterSelected,14) == true){
													if(int.Parse((string) entry_folio_servicio.Text.ToString()) != 0){
														comando2.CommandText = "UPDATE osiris_his_solicitudes_deta SET cantidad_autorizada = '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"',"+
															"fechahora_autorizado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
															"id_quien_autorizo = '"+this.LoginEmpleado+"',"+
															//"stock_cuando_solicito = '"++"',"+
															"id_almacen_origen = '"+idalmacenorigen.ToString().Trim()+"',"+
															"surtido = 'true',"+
															"folio_de_servicio = '"+(string) entry_folio_servicio.Text.ToString()+"',"+
															"pid_paciente = '"+(string) entry_pid_paciente.Text.ToString()+"' "+
															"WHERE id_secuencia =  '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,7)+"';";
														//Console.WriteLine(comando.CommandText);
														comando2.ExecuteNonQuery();
														if((bool) cargo_automatico == true){
															insert_a_procedimiento((string) entry_folio_servicio.Text.ToString(),
																				(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,5),
																				(string) entry_pid_paciente.Text.ToString(),
																				(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2),
																				(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,16),
																				(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,17));
														}
													}else{
														MessageDialog msgBox3 = new MessageDialog (MyWin,DialogFlags.Modal,
															MessageType.Info,ButtonsType.Ok,"Debe elegir al paciente para hacer su envio, verifique...");										
															msgBox3.Run ();	msgBox3.Destroy();
														break;
													}
												}else{
													comando2.CommandText = "UPDATE osiris_his_solicitudes_deta SET cantidad_autorizada = '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"',"+
														"fechahora_autorizado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
														"id_quien_autorizo = '"+this.LoginEmpleado+"',"+
														//"stock_cuando_solicito = '"++"',"+
														"id_almacen_origen = '"+idalmacenorigen.ToString().Trim()+"',"+
														"surtido = 'true' "+
														"WHERE id_secuencia =  '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,7)+"';";
													//Console.WriteLine(comando.CommandText);
													comando2.ExecuteNonQuery();
												}
												comando2.Dispose();
											}
											
											if (this.checkbutton_envio_directo.Active == true){
												NpgsqlConnection conexion4; 
												conexion4 = new NpgsqlConnection (connectionString+nombrebd);
												try{
													conexion4.Open ();
													NpgsqlCommand comando4; 
													comando4 = conexion4.CreateCommand ();
													comando4.CommandText = "INSERT INTO osiris_his_solicitudes_deta("+
																								"folio_de_solicitud,"+
																								"id_producto,"+
																								"precio_producto_publico,"+
																								"costo_por_unidad,"+
																								"cantidad_solicitada,"+
																								"cantidad_autorizada,"+
																								"fechahora_solicitud,"+
																								"id_quien_autorizo,"+
																								"id_almacen,"+
																								"envio_directo,"+
																								"surtido,"+
																								"fechahora_autorizado,"+
																								"status,solicitud_stock,folio_de_servicio,pid_paciente) "+
																								"VALUES ("+																							
																								"0,'"+
																								(string) this.lista_de_materiales_solicitados.Model.GetValue(iterSelected,5)+"','"+
																								(string) this.lista_de_materiales_solicitados.Model.GetValue(iterSelected,9)+"','"+
																								(string) this.lista_de_materiales_solicitados.Model.GetValue(iterSelected,8)+"','"+
																								(string) this.lista_de_materiales_solicitados.Model.GetValue(iterSelected,1)+"','"+
																								(string) this.lista_de_materiales_solicitados.Model.GetValue(iterSelected,2)+"','"+
																								DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																								LoginEmpleado+"','"+
																								this.idsubalmacen.ToString()+"','"+
																								"true','"+
																								"true','"+
																								DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																								"true"+"','"+
																								(bool) checkbutton_stock_almacen.Active+"','"+
																								(string) entry_folio_servicio.Text.ToString().Trim()+"','"+
																								(string) entry_pid_paciente.Text.ToString()+"');";
																							
													//Console.WriteLine(comando4.CommandText);
													comando4.ExecuteNonQuery();
													comando4.Dispose();
												}catch (NpgsqlException ex){
													MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																				MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
													msgBoxError.Run ();				msgBoxError.Destroy();
												}
												conexion4.Close();
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
				 			llenado_de_material_solicitado();				 			
				 			checkbutton_envio_directo.Active = false;
					 		entry_desc_producto.Sensitive = false;
							button_busca_producto.Sensitive = false;
							button_quitar_productos.Sensitive = false;
							button_pedido_erroneo.Sensitive = true;
							button_sin_stock.Sensitive = true;											
						}
					}else{
						MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"Debe Seleccionar un ALMACEN DE ORIGEN, verifique...");										
						msgBox1.Run ();	msgBox1.Destroy();
					}	
		 		}else{
		 			MessageDialog msgBox2 = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"Debe Seleccionar un SUB-ALMACEN, verifique...");										
					msgBox2.Run ();	msgBox2.Destroy();
		 		}
 			}else{
 				MessageDialog msgBox3 = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"No existen productos para poder traspasar, verifique...");										
				msgBox3.Run ();	msgBox3.Destroy();
 			}
		}
		
		void insert_a_procedimiento(string numero_de_atencion,string idproducto,string pidpaciente,string cantidadaplicada,
									string idtipoadmision_,string preciocostounitario_)
		{
			if((bool) cargo_automatico == true){
				Console.WriteLine(numero_de_atencion);					
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand();
					comando.CommandText = "INSERT INTO osiris_erp_cobros_deta("+
			 														"id_producto,"+
			 														"folio_de_servicio,"+
			 														"pid_paciente,"+						
			 														"cantidad_aplicada,"+	 														
																	"id_tipo_admisiones,"+						
																	"precio_costo_unitario,"+
							
																	"precio_producto, "+
			 														//"precio_por_cantidad,"+
			 														"iva_producto,"+
			 														"porcentage_utilidad,"+
			 														"porcentage_descuento,"+						
			 														"id_empleado,"+
			 														"fechahora_creacion,"+
			 														"porcentage_iva,"+
			 														"id_almacen,"+
			 														"precio_costo "+
			 														") VALUES ('"+
			 														idproducto+"','"+//id_producto
			 														numero_de_atencion+"','"+//folio_de_servicio
			 														pidpaciente+"','"+      //"','"+//pid_paciente
																	cantidadaplicada+"','"+															
			 														idtipoadmision_+"','"+	//id_tipo_admisiones
																	preciocostounitario_+"','"+
																	"0','"+
																	//"0','"+
																	"0','"+
																	"0','"+
																	"0','"+
																	LoginEmpleado+"','"+//id_empleado
			 														DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+//fechahora_creacion
			 														valoriva+"','"+//porcentage_iva
			 														idsubalmacen.ToString().Trim()+"','"+
			 														"0"+//precio_costo*/
			 														"');";
					comando.ExecuteNonQuery();
					comando.Dispose();
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
				conexion.Close();
			}
			
			
			/* 
			NpgsqlConnection conexion1; 
			conexion1 = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion1.Open ();
				NpgsqlCommand comando1; 
				comando1 = conexion1.CreateCommand();
				comando1.CommandText = "UPDATE osiris_catalogo_almacenes SET stock  = stock - '"+(float) lista_de_servicios.Model.GetValue(iter,1)+"' "+
										"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
										"AND id_producto = '"+double.Parse((string) lista_de_servicios.Model.GetValue(iter,2))+"' ;";
				
				comando1.ExecuteNonQuery();
				comando1.Dispose();
				conexion1.Close();
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, 
							ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
			}
			conexion1.Close();
			*/
		}
		
		void on_button_sin_stock_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
 			if(treeViewEngineSolicitado.GetIterFirst (out iterSelected)){
 				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de marcar como sin Stock ?");
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
 						if ((bool)lista_de_materiales_solicitados.Model.GetValue (iterSelected,0) == true){
								comando.CommandText = "UPDATE osiris_his_solicitudes_deta SET id_quien_autorizo ='"+LoginEmpleado+"',"+ 
											"fechahora_autorizado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
											"sin_stock = 'true' "+
											"WHERE id_secuencia =  '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,7)+"';";
								comando.ExecuteNonQuery();
								comando.Dispose();
						}
						while (treeViewEngineSolicitado.IterNext(ref iterSelected)){
							if ((bool)lista_de_materiales_solicitados.Model.GetValue (iterSelected,0) == true){
								comando.CommandText = "UPDATE osiris_his_solicitudes_deta SET id_quien_autorizo ='"+LoginEmpleado+"',"+ 
											"fechahora_autorizado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
											"sin_stock = 'true' "+
											"WHERE id_secuencia =  '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,7)+"';";
								comando.ExecuteNonQuery();
								comando.Dispose();
							}
						}
						llenado_de_material_solicitado();
			        }catch (NpgsqlException ex){
				   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
					}
					conexion.Close ();
		 		}
		 	}		
		}
		
		void on_button_quitar_productos_clicked(object sender, EventArgs args)
		 {
		 	TreeModel model;
			TreeIter iterSelected;
 			if (this.lista_de_materiales_solicitados.Selection.GetSelected(out model, out iterSelected)){
 				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta quitar el producto "+(string) this.lista_de_materiales_solicitados.Model.GetValue (iterSelected,4));
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
				
		 		if (miResultado == ResponseType.Yes){
		 			this.treeViewEngineSolicitado.Remove(ref iterSelected);
		 		}
		 	}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close,"no hay nada para quitar...");
				msgBoxError.Run ();						msgBoxError.Destroy();
			}
		 }
		
		void on_button_pedido_erroneo_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
 			if(treeViewEngineSolicitado.GetIterFirst (out iterSelected)){
 				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de marcar como PEDIDO ERRONEO ?");
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
						if ((bool)lista_de_materiales_solicitados.Model.GetValue (iterSelected,0) == true){
							comando.CommandText = "UPDATE osiris_his_solicitudes_deta SET id_quien_autorizo ='"+LoginEmpleado+"',"+ 
											"fechahora_autorizado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
											"solicitado_erroneo = 'true' "+
											"WHERE id_secuencia =  '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,7)+"';";
							comando.ExecuteNonQuery();
							comando.Dispose();
						}
						while (treeViewEngineSolicitado.IterNext(ref iterSelected)){
							if ((bool)lista_de_materiales_solicitados.Model.GetValue (iterSelected,0) == true){
								comando.CommandText = "UPDATE osiris_his_solicitudes_deta SET id_quien_autorizo ='"+LoginEmpleado+"',"+ 
											"fechahora_autorizado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
											"solicitado_erroneo = 'true' "+
											"WHERE id_secuencia =  '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,7)+"';";
								comando.ExecuteNonQuery();
								comando.Dispose();	
							}
						}
						llenado_de_material_solicitado();
					}catch (NpgsqlException ex){
				   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();			msgBoxError.Destroy();
					}
					conexion.Close ();
		 		}
		 	}
		}
		
		void on_checkbutton_envio_directo_clicked(object sender, EventArgs args)
		{
			if(checkbutton_envio_directo.Active == true){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de enviar productos SIN SOLICITUD para el sub-almacen "+descsubalmacen.Trim()+" seleccione los productos...");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
		 			treeViewEngineSolicitado.Clear();
		 			entry_fecha_solicitud.Text = DateTime.Now.ToString("dd-MM-yyyy");
		 			entry_desc_producto.Sensitive = true;
					button_busca_producto.Sensitive = true;
					button_quitar_productos.Sensitive = true;
					button_pedido_erroneo.Sensitive = false;
					button_sin_stock.Sensitive = false;
					checkbutton_stock_almacen.Sensitive = true;
					checkbutton_stock_paciente.Sensitive = true;
					radiobutton_pre_solic.Sensitive = false;
					radiobutton_solic_paciente.Sensitive = false;
					radiobutton_solic_stock.Sensitive = false;
					radiobutton_todas_solici.Sensitive = false;
				}else{
		 			checkbutton_envio_directo.Active = false;		 			
		 		}
		 	}else{
		 		entry_desc_producto.Sensitive = false;
				button_busca_producto.Sensitive = false;
				button_quitar_productos.Sensitive = false;
				button_pedido_erroneo.Sensitive = true;
				button_sin_stock.Sensitive = true;
				checkbutton_stock_almacen.Sensitive = false;
				checkbutton_stock_paciente.Sensitive = false;
				radiobutton_pre_solic.Sensitive = true;
				radiobutton_solic_paciente.Sensitive = true;
				radiobutton_solic_stock.Sensitive = true;
				radiobutton_todas_solici.Sensitive = true;
		 	}
		}
		
		void on_checkbutton_stock_paciente_clicked(object sender, EventArgs args)
		{
			if((bool) checkbutton_stock_paciente.Active == true){
				entry_folio_servicio.Sensitive = true;
				entry_pid_paciente.Sensitive = true;
				entry_nombre_paciente.Sensitive = true;
				button_busca_paciente.Sensitive = true;
				checkbutton_stock_almacen.Active = false;
			}else{
				entry_folio_servicio.Sensitive = false;
				entry_pid_paciente.Sensitive = false;
				entry_nombre_paciente.Sensitive = false;
				button_busca_paciente.Sensitive = false;
				entry_folio_servicio.Text = "0";
				entry_pid_paciente.Text = "0";
				entry_nombre_paciente.Text = "";
			}
		}
		
		void on_checkbutton_stock_almacen_clicked(object sender, EventArgs args)
		{
			if((bool)checkbutton_stock_almacen.Active == true){
				entry_folio_servicio.Sensitive = false;
				entry_pid_paciente.Sensitive = false;
				entry_nombre_paciente.Sensitive = false;
				button_busca_paciente.Sensitive = false;
				checkbutton_stock_paciente.Active = false;
			}
		}
		
		void on_button_busca_paciente_clicked(object sender, EventArgs args)
		{
			object[] parametros_objetos = {entry_folio_servicio,entry_pid_paciente,entry_nombre_paciente};
			string[] parametros_sql = {"SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente,osiris_erp_cobros_enca WHERE activo = 'true' "+
										"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
										"AND osiris_erp_cobros_enca.cancelado = false "+
										"AND osiris_erp_cobros_enca.alta_paciente = 'false' "+
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cerrado = 'false' "+
										"AND osiris_erp_cobros_enca.reservacion = 'false' ",															
									"SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente,osiris_erp_cobros_enca WHERE activo = 'true' "+
										"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
										"AND osiris_erp_cobros_enca.cancelado = false "+
										"AND osiris_erp_cobros_enca.alta_paciente = 'false' "+
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cerrado = 'false' "+
										"AND osiris_erp_cobros_enca.reservacion = 'false' "+
										"AND apellido_paterno_paciente LIKE '%",
									"SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente,osiris_erp_cobros_enca WHERE activo = 'true' "+
										"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
										"AND osiris_erp_cobros_enca.alta_paciente = false "+
										"AND osiris_erp_cobros_enca.cancelado = false "+
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cerrado = 'false' "+
										"AND osiris_erp_cobros_enca.reservacion = 'false' "+
										"AND nombre1_paciente LIKE '%",
									"SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente,osiris_erp_cobros_enca WHERE activo = 'true' "+
										"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
										"AND osiris_erp_cobros_enca.cancelado = false "+
										"AND osiris_erp_cobros_enca.alta_paciente = 'false' "+
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cerrado = 'false' "+
										"AND osiris_erp_cobros_enca.reservacion = 'false' "+
										"AND osiris_his_paciente.pid_paciente = '"};
			string[] parametros_string = {};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_paciente"," ORDER BY osiris_his_paciente.pid_paciente","%' ",1);
		}
			
		void llenado_de_material_solicitado()
		{
			TreeIter iter2;
			this.treeViewEngineSolicitado.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			string sql_envio_subalmacenes = "SELECT osiris_his_solicitudes_deta.folio_de_solicitud,to_char(osiris_his_solicitudes_deta.id_producto,'999999999999') AS idproductos,"+
               						"to_char(cantidad_solicitada,'99999999.999') AS cantidadsolicitada,"+
               						"to_char(osiris_his_solicitudes_deta.precio_producto_publico,'999999999.99') AS precioproductopublico,"+
               						"to_char(osiris_his_solicitudes_deta.costo_por_unidad,'999999999.99') AS costoporunidad,"+
               						"to_char(cantidad_autorizada,'999999.999') AS cantidadautorizada,id_quien_autorizo, "+
               						"to_char(fechahora_solicitud,'yyyy-MM-dd HH24:MI') AS fechahorasolicitud,"+
               						"to_char(fechahora_autorizado,'dd-MM-yyyy') AS fechahoraautorizado,"+
               						"status,surtido,osiris_productos.descripcion_producto,"+
               						"to_char(osiris_his_solicitudes_deta.id_secuencia,'9999999999') AS idsecuencia,"+
               						"to_char(folio_de_solicitud,'9999999999') AS foliodesolicitud,"+
               						"osiris_his_solicitudes_deta.eliminado,"+
									"descripcion_grupo_producto,descripcion_grupo1_producto,"+
									"osiris_his_solicitudes_deta.folio_de_servicio AS foliodeatencion," +
									//"osiris_his_solicitudes_deta.id_tipo_admisiones,"+
									"osiris_his_solicitudes_deta.pid_paciente AS pidpaciente,"+
									"solicitud_stock,pre_solicitud,nombre_paciente,procedimiento_qx,diagnostico_qx,"+
									"nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente," +
									"osiris_almacenes.id_tipo_admisiones,observaciones_solicitud "+
									//",descripcion_grupo2_producto "+
               						"FROM osiris_his_solicitudes_deta,osiris_almacenes,osiris_his_paciente,osiris_productos,osiris_grupo_producto,osiris_grupo1_producto "+
               						//",osiris_grupo2_producto "+
               						"WHERE osiris_his_solicitudes_deta.id_almacen = '"+idsubalmacen.ToString().Trim()+"' " +
               						"AND osiris_his_solicitudes_deta.id_almacen = osiris_almacenes.id_almacen "+
									"AND osiris_his_paciente.pid_paciente = osiris_his_solicitudes_deta.pid_paciente "+
               						"AND osiris_his_solicitudes_deta.id_producto = osiris_productos.id_producto "+
               						"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
									"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
									//"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
									"AND surtido = 'false' "+
									"AND status = 'true' "+
									"AND sin_stock = 'false' "+
									"AND solicitado_erroneo = 'false' "+
									"AND envio_directo = 'false' "+
									"AND osiris_his_solicitudes_deta.eliminado = 'false' ";
			string nombrepaciente;
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = sql_envio_subalmacenes+filtro_sub_almacen+
									" ORDER BY osiris_his_solicitudes_deta.id_almacen,osiris_his_solicitudes_deta.folio_de_solicitud,osiris_productos.descripcion_producto;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){
					if((bool) lector["pre_solicitud"] == true){
						nombrepaciente = (string) lector["nombre_paciente"].ToString().Trim();
					}else{
						nombrepaciente = (string) lector["nombre1_paciente"].ToString().Trim()+" "+
							             (string) lector["nombre2_paciente"].ToString().Trim()+" "+
							             (string) lector["apellido_paterno_paciente"].ToString().Trim()+" "+
							             (string) lector["apellido_materno_paciente"].ToString().Trim();	
					}
					treeViewEngineSolicitado.AppendValues(false,
													(string) lector["cantidadsolicitada"],
													"0",
													(string) lector["foliodesolicitud"],
													(string) lector["descripcion_producto"],
													(string) lector["idproductos"],
													(string) lector["fechahorasolicitud"],
													(string) lector["idsecuencia"],
													"0",
													"0",
					                                (string) lector["foliodeatencion"].ToString().Trim(),
					                                (string) lector["pidpaciente"].ToString().Trim(),
					                               	nombrepaciente,
					                                (bool) lector["solicitud_stock"],
					                                (bool) lector["pre_solicitud"],
													(string) lector["procedimiento_qx"].ToString().Trim(),
													(string) lector["id_tipo_admisiones"].ToString().Trim(),
													(string) lector["costoporunidad"].ToString().Trim(),
					                                (string) lector["observaciones_solicitud"].ToString().Trim());
					col_envios00.SetCellDataFunc(cellrt00, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_envios01.SetCellDataFunc(cellrt01, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_envios02.SetCellDataFunc(cellrt02, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_envios03.SetCellDataFunc(cellrt03, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_envios04.SetCellDataFunc(cellrt04, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_envios05.SetCellDataFunc(cellrt05, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_envios06.SetCellDataFunc(cellrt06, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_envios10.SetCellDataFunc(cellrt10, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_envios11.SetCellDataFunc(cellrt11, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_envios12.SetCellDataFunc(cellrt12, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_envios13.SetCellDataFunc(cellrt13, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_envios14.SetCellDataFunc(cellrt14, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_envios15.SetCellDataFunc(cellrt15, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_envios18.SetCellDataFunc(cellrt18, new Gtk.TreeCellDataFunc(cambia_colores_fila));
				}
				/*
				if (treeViewEngineSolicitado.GetIterFirst (out iter2)){
					lista_de_materiales_solicitados.Model.SetValue(iter2,0,true);
					while (treeViewEngineSolicitado.IterNext(ref iter2)){
						lista_de_materiales_solicitados.Model.SetValue(iter2,0,true);
					}
				}
				*/
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		//ACCION QUE CAMBIA EL COLOR DEL TEXTO PARA CUANDO SE GUARDA EN LA BASE DE DATOS 
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{			
			if(cell.GetType().ToString() == "Gtk.CellRendererToggle"){
				if ((bool) lista_de_materiales_solicitados.Model.GetValue (iter,13)==true){
					(cell as Gtk.CellRendererToggle).CellBackground = "yellow";					
				}else{
					if ((bool) lista_de_materiales_solicitados.Model.GetValue (iter,14)==true){
						(cell as Gtk.CellRendererToggle).CellBackground = "green";
					}else{
						(cell as Gtk.CellRendererToggle).CellBackground = "white";
					}
				}
			}
			if(cell.GetType().ToString() == "Gtk.CellRendererText"){
				if ((bool) lista_de_materiales_solicitados.Model.GetValue (iter,13)==true){
					(cell as Gtk.CellRendererText).CellBackground = "yellow";					
				}else{
					if ((bool) lista_de_materiales_solicitados.Model.GetValue (iter,14)==true){
						(cell as Gtk.CellRendererText).CellBackground = "green";
					}else{
						(cell as Gtk.CellRendererText).CellBackground = "white";
					}
				}
			}
		}
		
		void crea_treeview_envio_materiales()
		{
			treeViewEngineSolicitado = new ListStore(typeof(bool),		// 0  marca lo solicitado para enviarlo al sub-almacen 
													typeof(string),		// 1  Solicitados	
													typeof(string),		// 2  captura el almacenista
													typeof(string),		// 3
													typeof(string),		// 4
													typeof(string),		// 5
													typeof(string),		// 6													
													typeof(string),		// 7 Almacena la secuencia de la tabla de datos
													typeof(string),		// 8 se ocupa para cuando es envio sin solicitud Costo por unidad
													typeof(string),		// 9 se ocupa para cuando es envio sin solicitud precio publico
			                                        typeof(string),		// 10 Folio de Atencion
			                                        typeof(string),		// 11 Pid del paciente
			                                        typeof(string),		// 12 nombre del paciente
			                                        typeof(bool),		// 13 Solicitud para Stock
			                                        typeof(bool),		// 14 Pre-Solicitud de Quirofano
													typeof(string),		// 15 Procedimiento
			                                        typeof(string),		// 16 Departamento donde se solicito el producto
													typeof(string),		// 17 precio costo unitario del producto
													typeof(string),		// 18 Observacion de Solicitud
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));		
												
			lista_de_materiales_solicitados.Model = treeViewEngineSolicitado;
			
			lista_de_materiales_solicitados.RulesHint = true;
			
			col_envios00 = new TreeViewColumn();
			cellrt00 = new CellRendererToggle();
			col_envios00.Title = "Selec."; // titulo de la cabecera de la columna, si está visible
			col_envios00.PackStart(cellrt00, true);
			col_envios00.AddAttribute (cellrt00, "active", 0);
			cellrt00.Activatable = true;
			cellrt00.Toggled += selecciona_fila;
			col_envios00.SortColumnId = (int) Column_solicitudes.col_envios00;
						
			col_envios01 = new TreeViewColumn();
			cellrt01 = new CellRendererText();
			col_envios01.Title = "Solicitado"; // titulo de la cabecera de la columna, si está visible
			col_envios01.PackStart(cellrt01, true);
			col_envios01.AddAttribute (cellrt01, "text", 1);
			col_envios01.SortColumnId = (int) Column_solicitudes.col_envios01;
			
			col_envios02 = new TreeViewColumn();
			cellrt02 = new CellRendererText();
			col_envios02.Title = "Autorizado"; // titulo de la cabecera de la columna, si está visible
			col_envios02.PackStart(cellrt02, true);
			col_envios02.AddAttribute (cellrt02, "text", 2);
			col_envios02.SortColumnId = (int) Column_solicitudes.col_envios01;
			cellrt02.Editable = true;
			cellrt02.Edited += NumberCellEdited_Autorizado;
			
			col_envios03 = new TreeViewColumn();
			cellrt03 = new CellRendererText();
			col_envios03.Title = "Nº Solicitud"; // titulo de la cabecera de la columna, si está visible
			col_envios03.PackStart(cellrt03, true);
			col_envios03.AddAttribute (cellrt03, "text", 3);
			col_envios03.SortColumnId = (int) Column_solicitudes.col_envios03;
			
			col_envios04 = new TreeViewColumn();
			cellrt04 = new CellRendererText();
			col_envios04.Title = "Descripcion del Producto"; // titulo de la cabecera de la columna, si está visible
			col_envios04.PackStart(cellrt04, true);
			col_envios04.AddAttribute (cellrt04, "text", 4);
			col_envios04.SortColumnId = (int) Column_solicitudes.col_envios04;			
			col_envios04.Resizable = true;
			cellrt04.Width = 550;
			
			col_envios05 = new TreeViewColumn();
			cellrt05 = new CellRendererText();
			col_envios05.Title = "ID. Producto"; // titulo de la cabecera de la columna, si está visible
			col_envios05.PackStart(cellrt05, true);
			col_envios05.AddAttribute (cellrt05, "text", 5);
			col_envios05.SortColumnId = (int) Column_solicitudes.col_envios05;
			
			col_envios06 = new TreeViewColumn();
			cellrt06 = new CellRendererText();
			col_envios06.Title = "Fecha/hora Sol."; // titulo de la cabecera de la columna, si está visible
			col_envios06.PackStart(cellrt06, true);
			col_envios06.AddAttribute (cellrt06, "text", 6);
			col_envios06.SortColumnId = (int) Column_solicitudes.col_envios06;
			
			col_envios10 = new TreeViewColumn();
			cellrt10 = new CellRendererText();
			col_envios10.Title = "N° Atencion";
			col_envios10.PackStart(cellrt10, true);
			col_envios10.AddAttribute (cellrt10, "text", 10);
			col_envios10.SortColumnId = (int) Column_solicitudes.col_envios10;
			
			col_envios11 = new TreeViewColumn();
			cellrt11 = new CellRendererText();
			col_envios11.Title = "PID";
			col_envios11.PackStart(cellrt11, true);
			col_envios11.AddAttribute (cellrt11, "text", 11);
			col_envios11.SortColumnId = (int) Column_solicitudes.col_envios11;
			
			col_envios12 = new TreeViewColumn();
			cellrt12 = new CellRendererText();
			col_envios12.Title = "Nombre Paciente";
			col_envios12.PackStart(cellrt12, true);
			col_envios12.AddAttribute (cellrt12, "text", 12);
			col_envios12.SortColumnId = (int) Column_solicitudes.col_envios12;
			
			col_envios13 = new TreeViewColumn();
			cellrt13 = new CellRendererToggle();
			col_envios13.Title = "Para Stock";
			col_envios13.PackStart(cellrt13, true);
			col_envios13.AddAttribute (cellrt13, "active", 13);
			col_envios13.SortColumnId = (int) Column_solicitudes.col_envios13;
			
			col_envios14 = new TreeViewColumn();
			cellrt14 = new CellRendererToggle();
			col_envios14.Title = "Pre-Solicitud";
			col_envios14.PackStart(cellrt14, true);
			col_envios14.AddAttribute (cellrt14, "active", 14);
			col_envios14.SortColumnId = (int) Column_solicitudes.col_envios14;
			
			col_envios15 = new TreeViewColumn();
			cellrt15 = new CellRendererText();
			col_envios15.Title = "Procedimiento QX.";
			col_envios15.PackStart(cellrt15, true);
			col_envios15.AddAttribute (cellrt15, "text", 15);
			col_envios15.SortColumnId = (int) Column_solicitudes.col_envios15;
			
			col_envios18 = new TreeViewColumn();
			cellrt18 = new CellRendererText();
			col_envios18.Title = "Observacion Solicitud";
			col_envios18.PackStart(cellrt18, true);
			col_envios18.AddAttribute (cellrt18, "text", 18);
			col_envios18.SortColumnId = (int) Column_solicitudes.col_envios18;
			
			lista_de_materiales_solicitados.AppendColumn(col_envios00);
			lista_de_materiales_solicitados.AppendColumn(col_envios01);
			lista_de_materiales_solicitados.AppendColumn(col_envios02);
			lista_de_materiales_solicitados.AppendColumn(col_envios03);
			lista_de_materiales_solicitados.AppendColumn(col_envios04);
			lista_de_materiales_solicitados.AppendColumn(col_envios05);
			lista_de_materiales_solicitados.AppendColumn(col_envios06);
			lista_de_materiales_solicitados.AppendColumn(col_envios10);
			lista_de_materiales_solicitados.AppendColumn(col_envios11);
			lista_de_materiales_solicitados.AppendColumn(col_envios12);
			lista_de_materiales_solicitados.AppendColumn(col_envios13);
			lista_de_materiales_solicitados.AppendColumn(col_envios14);
			lista_de_materiales_solicitados.AppendColumn(col_envios15);
			lista_de_materiales_solicitados.AppendColumn(col_envios18);
		}
		
		enum Column_solicitudes
		{
			col_envios00,
			col_envios01,
			col_envios02,
			col_envios03,
			col_envios04,
			col_envios05,
			col_envios06,col_envios10,col_envios11,col_envios12,col_envios13,col_envios14,col_envios15,col_envios18
		}
		
		// Cuando seleccion el treeview de cargos extras para cargar los productos  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			if (lista_de_materiales_solicitados.Model.GetIter (out iter,new TreePath (args.Path))) {
				bool old = (bool) lista_de_materiales_solicitados.Model.GetValue (iter,0);
				lista_de_materiales_solicitados.Model.SetValue(iter,0,!old);
			}	
		}
		
		void on_llena_lista_producto_clicked (object sender, EventArgs args)
 		{
 			llenando_lista_de_productos();
 		}
		
		
		// llena la lista de productos
 		void llenando_lista_de_productos()
 		{
			string acceso_a_grupos = classpublic.lee_registro_de_tabla("osiris_almacenes","id_almacen"," WHERE osiris_almacenes.id_almacen = '1' ","acceso_grupo_producto","int");
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
						"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,"+
						"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto," +
						"to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
						"to_char(porcentage_ganancia,'99999.99') AS porcentageutilidad," +
						"to_char(costo_producto,'999999999.99') AS costoproducto "+
						"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
						"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
						"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
						"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
						"AND osiris_productos.cobro_activo = true "+
						"AND osiris_productos.id_grupo_producto IN("+acceso_a_grupos+") "+
						"AND osiris_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper()+"%' ORDER BY descripcion_producto; ";
				
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
					treeViewEngineBusca2.AppendValues (
											(string) lector["codProducto"],//0
											(string) lector["descripcion_producto"],//1
											(string) lector["descripcion_grupo_producto"],//2
											(string) lector["descripcion_grupo1_producto"],//3
											(string) lector["descripcion_grupo2_producto"],//4
											(string) lector["preciopublico"],//5
											calculodeiva.ToString("F").PadLeft(10),//6
											preciomasiva.ToString("F").PadLeft(10),//7
											(string) lector["porcentagesdesc"],//8
											preciocondesc.ToString("F").PadLeft(10),//9
											(string) lector["costoproductounitario"],//10
											(string) lector["porcentageutilidad"],//11
											(string) lector["costoproducto"]);//12
					
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
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
		
		void on_selecciona_producto_clicked(object sender, EventArgs args)
		{
			bool valida_paciente = true;
			string idtipoadmision = classpublic.lee_registro_de_tabla("osiris_almacenes","id_almacen"," WHERE osiris_almacenes.id_almacen = '"+idsubalmacen.ToString().Trim()+"' ","id_tipo_admisiones","int");
			Console.WriteLine(idtipoadmision);
			TreeModel model;
			TreeIter iterSelected;
			if((bool) checkbutton_stock_almacen.Active == true || (bool) checkbutton_stock_paciente.Active == true){
				if((bool) checkbutton_stock_paciente.Active == true){
					//Console.WriteLine(entry_folio_servicio.Text.ToString());
					if(int.Parse(entry_folio_servicio.Text.ToString()) == 0){
						valida_paciente = false;
					}
				}
				if((bool) valida_paciente == true){				
					if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
		 				if ((float) float.Parse((string) entry_cantidad_aplicada.Text) > 0){
		 					this.treeViewEngineSolicitado.AppendValues(true,
		 														this.entry_cantidad_aplicada.Text,
		 														this.entry_cantidad_aplicada.Text,
		 														"0",
		 														(string) model.GetValue(iterSelected, 1),
		 														(string) model.GetValue(iterSelected, 0),
		 														(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
		 														"0",
		 														(string) model.GetValue(iterSelected, 10),
		 														(string) model.GetValue(iterSelected, 5),
							                                    (string) entry_folio_servicio.Text.ToString() ,
							                                    (string) entry_pid_paciente.Text.ToString(),
							                                    (string) entry_nombre_paciente.Text.ToString(),
							                                    (bool) checkbutton_stock_almacen.Active,
							                                     false,
																"",
																idtipoadmision,
																"0",
																"0",
																"0",
																"0",
																"0",
																"0",
																"0");
		 					entry_cantidad_aplicada.Text = "0";
		 				}else{
		 					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
														MessageType.Error,ButtonsType.Close, 
													"La cantidad que quiere solicitar debe ser \n"+"distinta a cero, intente de nuevo");
							msgBoxError.Run ();					msgBoxError.Destroy();
		 				}
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
														MessageType.Error,ButtonsType.Close, 
													"Seleccione un Paciente para hacer el envio, verifique... ");
					msgBoxError.Run ();					msgBoxError.Destroy();
				}
 			}else{
				MessageDialog msgBoxError2 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error,ButtonsType.Close, 
												"Debe elegir al menos un tipo de Envio \n"+"STOCK o a un Paciente");
				msgBoxError2.Run ();					msgBoxError2.Destroy();
		}
 		}
 		 		
 		void crea_treeview_busqueda(string tipo_busqueda)
		{
			if (tipo_busqueda == "solicitud"){
				
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
		
		void NumberCellEdited_Autorizado (object o, EditedArgs args)
		{
			Gtk.TreeIter iter;
			bool esnumerico = false;
			int var_paso = 0;
			int largo_variable = args.NewText.ToString().Length;
			string toma_variable = args.NewText.ToString();
			
			this.treeViewEngineSolicitado.GetIter (out iter, new Gtk.TreePath (args.Path));
			
			while (var_paso < largo_variable){				
				if ((string) toma_variable.Substring(var_paso,1).ToString() == "." || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "0" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "1" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "2" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "3" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "4" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "5" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "6" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "7" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "8" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "9") {
					esnumerico = true;
				}else{
				 	esnumerico = false;
				 	var_paso = largo_variable;
				}
				var_paso += 1;
			}
			if (esnumerico == true){		
				this.treeViewEngineSolicitado.SetValue(iter,(int) Column_solicitudes.col_envios02,args.NewText);
				bool old = (bool) lista_de_materiales_solicitados.Model.GetValue (iter,0);
				lista_de_materiales_solicitados.Model.SetValue(iter,0,!old);
			}
 		}
		
		/*
		void on_expandrows_RowExpanded (object sender, EventArgs args)
		{
			//Gtk.TreeView mitree = sender as Gtk.TreeView;
		}
		*/
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace")
			{
				args.RetVal = true;
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
				llenando_lista_de_productos();			
			}
		}
				
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}