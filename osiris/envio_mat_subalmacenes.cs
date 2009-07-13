// created on 27/01/2008 at 01:55 p
///////////////////////////////////////////////////////////
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
//				  Daniel Olivares - arcangeldoc@gmail.com (Diseño de Pantallas Glade)
// 				  Ing. Jesus Buentello Garza (Programacion y reporte)
//
// Licencia		: GLP
// S.O. 		: GNU/Linux Red-Hat 4.0
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
// Proposito	: Pagos en Caja 
// Objeto		: cargos_hospitalizacion.cs
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
		[Widget] Gtk.Button button_surtir_materiales;
		[Widget] Gtk.Button button_sin_stock;
		[Widget] Gtk.Button button_pedido_erroneo;
		[Widget] Gtk.Entry entry_fecha_solicitud;
		[Widget] Gtk.Entry entry_desc_producto;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.Button button_quitar_productos;
		[Widget] Gtk.Button button_rpt_surtido;
		
		[Widget] Gtk.CheckButton checkbutton_envio_directo;
		
		
		public string connectionString = "Server=localhost;" +
						"Port=5432;" +
						 "User ID=admin;" +
						"Password=1qaz2wsx;";
						
		public string LoginEmpleado;
		public string NomEmpleado;
		public string AppEmpleado;
		public string ApmEmpleado;
		public string nombrebd;
		
		// Variables publica
		public float valoriva = 15;
		public int idsubalmacen = 1;
		public string descsubalmacen = "";
		public int idalmacenorigen = 0;
		
		/////// Ventana Busqueda de productos\\\\\\\\
		[Widget] Gtk.TreeView lista_de_producto;
		//[Widget] Gtk.Button button_agrega_extra;
		[Widget] Gtk.Entry entry_cantidad_aplicada;
		[Widget] Gtk.Label label_titulo_cantidad;
		
		//private TreeStore treeViewEngineBusca;
		private TreeStore treeViewEngineBusca2;
		private ListStore treeViewEngineSolicitado;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public envio_de_materiales_subalmacenes(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = _nombrebd_;
						
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
			
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			// envio directo a sub-almacenes
			checkbutton_envio_directo.Clicked += new EventHandler(on_checkbutton_envio_directo_clicked);
			
			entry_desc_producto.Sensitive = false;
			button_busca_producto.Sensitive = false;
			button_quitar_productos.Sensitive = false;
			
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
               	comando.CommandText = "SELECT id_almacen,descripcion_almacen,sub_almacen,almacen_salidas FROM hscmty_almacenes "+
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
			 					if (decimal.Parse((string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)) > 0 ){
					 				
					 				
					 										Console.WriteLine("akkaaa");
					 				NpgsqlConnection conexion;
									conexion = new NpgsqlConnection (connectionString+nombrebd);
					 				try{
										conexion.Open ();
										NpgsqlCommand comando; 
										comando = conexion.CreateCommand();
										comando.CommandText = "SELECT id_producto,id_almacen,stock FROM hscmty_catalogo_almacenes "+
															"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
															"AND eliminado = 'false' "+														
															"AND id_producto = '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,5)+"' ;";
										//Console.WriteLine(comando.CommandText);
										NpgsqlDataReader lector = comando.ExecuteReader ();
										if(lector.Read()){
										Console.WriteLine("if ");
											NpgsqlConnection conexion1; 
											conexion1 = new NpgsqlConnection (connectionString+nombrebd);
					 						try{
												conexion1.Open ();
												NpgsqlCommand comando1;
												comando1 = conexion1.CreateCommand();
												comando1.CommandText = "UPDATE hscmty_catalogo_almacenes SET stock  = stock + '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"',"+
																		//"historial_surtido_material = historial_surtido_material || '"+LoginEmpleado+" "+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n',"+
																		"fechahora_ultimo_surtimiento = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
																		"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																		"AND id_producto = '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,5)+"' ;";
												Console.WriteLine(comando1.CommandText);
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
	                                                comando2.CommandText = "UPDATE hscmty_catalogo_almacenes SET stock  = stock - '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"' "+
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
										Console.WriteLine("else"+idalmacenorigen);
											NpgsqlConnection conexion1; 
											conexion1 = new NpgsqlConnection (connectionString+nombrebd);
					 						try{
												conexion1.Open ();
												NpgsqlCommand comando1; 
												comando1 = conexion1.CreateCommand();
												comando1.CommandText = "INSERT INTO hscmty_catalogo_almacenes("+
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
	                                                comando2.CommandText = "UPDATE hscmty_catalogo_almacenes SET stock  = stock - '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"' "+
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
	
												
												
												
												
											}catch (NpgsqlException ex){
									   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
															MessageType.Error, 
															ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
												msgBoxError.Run ();
												msgBoxError.Destroy();
											}
											conexion1.Close();	
											
																								
																																					
																																																															
										}
										
										if (this.checkbutton_envio_directo.Active == false){
											comando.CommandText = "UPDATE hscmty_his_solicitudes_deta SET cantidad_autorizada = '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"',"+
															"fechahora_autorizado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
															"id_quien_autorizo = '"+this.LoginEmpleado+"',"+
															//"stock_cuando_solicito = '"++"',"+
															"id_almacen_origen = '"+idalmacenorigen.ToString().Trim()+"',"+
															"surtido = 'true' "+
															"WHERE id_secuencia =  '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,7)+"';";
										//Console.WriteLine(comando.CommandText);
											comando.ExecuteNonQuery();
											comando.Dispose();
										}
																				
										// VERIFICA CUANDO ES ENVIO DIRECTO DESDE UN ALAMCEN
										// Actualiza la tabla de soliciudes
										if (this.checkbutton_envio_directo.Active == true){
											NpgsqlConnection conexion4; 
											conexion4 = new NpgsqlConnection (connectionString+nombrebd);
											try{
												conexion4.Open ();
												NpgsqlCommand comando4; 
												comando4 = conexion4.CreateCommand ();
												comando4.CommandText = "INSERT INTO hscmty_his_solicitudes_deta("+
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
										NpgsqlConnection conexion2; 
										conexion2 = new NpgsqlConnection (connectionString+nombrebd);
							 			try{
											conexion2.Open ();
											NpgsqlCommand comando2; 
											comando2 = conexion2.CreateCommand();
											comando2.CommandText = "SELECT id_producto,id_almacen FROM hscmty_catalogo_almacenes "+
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
													comando3.CommandText = "UPDATE hscmty_catalogo_almacenes SET stock  = stock + '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"',"+
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
		                                                comando.CommandText = "UPDATE hscmty_catalogo_almacenes SET stock  = stock - '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"' "+
		                                                                        //"historial_surtido_material = historial_surtido_material || '"+LoginEmpleado+" "+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n',"+
		                                                                        //"fechahora_ultimo_surtimiento = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
		                                                                        "WHERE id_almacen = '"+idalmacenorigen+"' "+
		                                                                        "AND id_producto = '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,5)+"' ;";
		                                                //Console.WriteLine("Actualiza el Producto");
		                                                											Console.WriteLine("entra "+comando.CommandText);
		                                                											
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
													comando3.CommandText = "INSERT INTO hscmty_catalogo_almacenes("+
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
	                                                comando5.CommandText = "UPDATE hscmty_catalogo_almacenes SET stock  = stock - '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"' "+
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
												comando2.CommandText = "UPDATE hscmty_his_solicitudes_deta SET cantidad_autorizada = '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,2)+"',"+
														"fechahora_autorizado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
														"id_quien_autorizo = '"+this.LoginEmpleado+"',"+
														//"stock_cuando_solicito = '"++"',"+
														"id_almacen_origen = '"+idalmacenorigen.ToString().Trim()+"',"+
														"surtido = 'true' "+
														"WHERE id_secuencia =  '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,7)+"';";
												//Console.WriteLine(comando.CommandText);
												comando2.ExecuteNonQuery();
												comando2.Dispose();
											}
											
											if (this.checkbutton_envio_directo.Active == true){
												NpgsqlConnection conexion4; 
												conexion4 = new NpgsqlConnection (connectionString+nombrebd);
												try{
													conexion4.Open ();
													NpgsqlCommand comando4; 
													comando4 = conexion4.CreateCommand ();
													comando4.CommandText = "INSERT INTO hscmty_his_solicitudes_deta("+
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
		
		void on_button_sin_stock_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
 			if (this.lista_de_materiales_solicitados.Selection.GetSelected(out model, out iterSelected)){
 				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de marcar "+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,4)+" como sin Stock ?");
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
						comando.CommandText = "UPDATE hscmty_his_solicitudes_deta SET id_quien_autorizo ='"+LoginEmpleado+"',"+ 
											"fechahora_autorizado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
											"sin_stock = 'true' "+
											"WHERE id_secuencia =  '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,7)+"';";
						comando.ExecuteNonQuery();
						comando.Dispose();
						this.treeViewEngineSolicitado.Remove (ref iterSelected);
			        	msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"El Producto ");
						msgBox.Run ();
						msgBox.Destroy();		
						conexion.Close ();
			        }catch (NpgsqlException ex){
				   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
					}
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
		 	}
		 }
		
		void on_button_pedido_erroneo_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
 			if (this.lista_de_materiales_solicitados.Selection.GetSelected(out model, out iterSelected)){
 				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de marcar "+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,4)+" como PEDIDO ERRONEO ?");
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
						comando.CommandText = "UPDATE hscmty_his_solicitudes_deta SET id_quien_autorizo ='"+LoginEmpleado+"',"+ 
											"fechahora_autorizado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
											"solicitado_erroneo = 'true' "+
											"WHERE id_secuencia =  '"+(string) lista_de_materiales_solicitados.Model.GetValue (iterSelected,7)+"';";
						
						comando.ExecuteNonQuery();
						comando.Dispose();
						this.treeViewEngineSolicitado.Remove (ref iterSelected);
			        	msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"El Producto se marco como erroneo satisfactoreamente...");										
						msgBox.Run ();	msgBox.Destroy();		
						conexion.Close ();
			        }catch (NpgsqlException ex){
				   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();			msgBoxError.Destroy();
					}
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
		 		}else{
		 			checkbutton_envio_directo.Active = false;		 			
		 		}
		 	}else{
		 		entry_desc_producto.Sensitive = false;
				button_busca_producto.Sensitive = false;
				button_quitar_productos.Sensitive = false;
				button_pedido_erroneo.Sensitive = true;
				button_sin_stock.Sensitive = true;
		 	}
		}
		
		void llenado_de_material_solicitado()
		{
			this.treeViewEngineSolicitado.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT hscmty_his_solicitudes_deta.folio_de_solicitud,to_char(hscmty_his_solicitudes_deta.id_producto,'999999999999') AS idproductos,"+
               						"to_char(cantidad_solicitada,'99999999.999') AS cantidadsolicitada,"+
               						"to_char(hscmty_his_solicitudes_deta.precio_producto_publico,'999999999.99') AS precioproductopublico,"+
               						"to_char(hscmty_his_solicitudes_deta.costo_por_unidad,'999999999.99') AS costoporunidad,"+
               						"to_char(cantidad_autorizada,'999999.999') AS cantidadautorizada,id_quien_autorizo, "+
               						"to_char(fechahora_solicitud,'dd-MM-yyyy') AS fechahorasolicitud,"+
               						"to_char(fechahora_autorizado,'dd-MM-yyyy') AS fechahoraautorizado,"+
               						"status,surtido,hscmty_productos.descripcion_producto,"+
               						"to_char(id_secuencia,'9999999999') AS idsecuencia,"+
               						"to_char(folio_de_solicitud,'9999999999') AS foliodesolicitud,"+
               						"hscmty_his_solicitudes_deta.eliminado,"+
									"descripcion_grupo_producto,descripcion_grupo1_producto "+
									//",descripcion_grupo2_producto "+
               						"FROM hscmty_his_solicitudes_deta,hscmty_productos,hscmty_grupo_producto,hscmty_grupo1_producto "+
               						//",hscmty_grupo2_producto "+
               						"WHERE id_almacen = '"+idsubalmacen.ToString().Trim()+"' "+
               						"AND hscmty_his_solicitudes_deta.id_producto = hscmty_productos.id_producto "+
               						"AND hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
									"AND hscmty_productos.id_grupo1_producto = hscmty_grupo1_producto.id_grupo1_producto "+
									//"AND hscmty_productos.id_grupo2_producto = hscmty_grupo2_producto.id_grupo2_producto "+
									"AND surtido = 'false' "+
									"AND status = 'true' "+
									"AND sin_stock = 'false' "+
									"AND solicitado_erroneo = 'false' "+
									"AND envio_directo = 'false' "+
									"AND hscmty_his_solicitudes_deta.eliminado = 'false' "+
									"ORDER BY hscmty_his_solicitudes_deta.id_secuencia;";
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){			
					this.treeViewEngineSolicitado.AppendValues(false,
													(string) lector["cantidadsolicitada"],
													"0",
													(string) lector["foliodesolicitud"],
													(string) lector["descripcion_producto"],
													(string) lector["idproductos"],
													(string) lector["fechahorasolicitud"],
													(string) lector["idsecuencia"],
													"0",
													"0");
				}				
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion.Close ();
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
													typeof(string)		// 9 se ocupa para cuando es envio sin solicitud precio publico
													);
												
			lista_de_materiales_solicitados.Model = treeViewEngineSolicitado;
			
			lista_de_materiales_solicitados.RulesHint = true;
			
			TreeViewColumn col_surtir = new TreeViewColumn();
			CellRendererToggle cel_surtir = new CellRendererToggle();
			col_surtir.Title = "Surtir"; // titulo de la cabecera de la columna, si está visible
			col_surtir.PackStart(cel_surtir, true);
			col_surtir.AddAttribute (cel_surtir, "active", 0);
			cel_surtir.Activatable = true;
			cel_surtir.Toggled += selecciona_fila;
			col_surtir.SortColumnId = (int) Column_solicitudes.col_surtir;
			
			TreeViewColumn col_cant_solicitado = new TreeViewColumn();
			CellRendererText cel_cant_solicitado = new CellRendererText();
			col_cant_solicitado.Title = "Solicitado"; // titulo de la cabecera de la columna, si está visible
			col_cant_solicitado.PackStart(cel_cant_solicitado, true);
			col_cant_solicitado.AddAttribute (cel_cant_solicitado, "text", 1);
			col_cant_solicitado.SortColumnId = (int) Column_solicitudes.col_cant_solicitado;
			
			TreeViewColumn col_autorizado = new TreeViewColumn();
			CellRendererText cel_autorizado = new CellRendererText();
			col_autorizado.Title = "Autorizado"; // titulo de la cabecera de la columna, si está visible
			col_autorizado.PackStart(cel_autorizado, true);
			col_autorizado.AddAttribute (cel_autorizado, "text", 2);
			col_autorizado.SortColumnId = (int) Column_solicitudes.col_autorizado;
			cel_autorizado.Editable = true;
			cel_autorizado.Edited += NumberCellEdited_Autorizado;
			
			TreeViewColumn col_nro_solicitud = new TreeViewColumn();
			CellRendererText cel_nro_solicitud = new CellRendererText();
			col_nro_solicitud.Title = "Nº Solicitud"; // titulo de la cabecera de la columna, si está visible
			col_nro_solicitud.PackStart(cel_nro_solicitud, true);
			col_nro_solicitud.AddAttribute (cel_nro_solicitud, "text", 3);
			col_nro_solicitud.SortColumnId = (int) Column_solicitudes.col_nro_solicitud;
			
			TreeViewColumn col_desc_producto = new TreeViewColumn();
			CellRendererText cel_desc_producto = new CellRendererText();
			col_desc_producto.Title = "Descripcion del Producto"; // titulo de la cabecera de la columna, si está visible
			col_desc_producto.PackStart(cel_desc_producto, true);
			col_desc_producto.AddAttribute (cel_desc_producto, "text", 4);
			col_desc_producto.SortColumnId = (int) Column_solicitudes.col_desc_producto;			
			col_desc_producto.Resizable = true;
			cel_desc_producto.Width = 550;
			
			TreeViewColumn col_idproducto = new TreeViewColumn();
			CellRendererText cel_idproducto = new CellRendererText();
			col_idproducto.Title = "ID. Producto"; // titulo de la cabecera de la columna, si está visible
			col_idproducto.PackStart(cel_idproducto, true);
			col_idproducto.AddAttribute (cel_idproducto, "text", 5);
			col_idproducto.SortColumnId = (int) Column_solicitudes.col_idproducto;
			
			TreeViewColumn col_fecha_solicitado = new TreeViewColumn();
			CellRendererText cel_fecha_solicitado = new CellRendererText();
			col_fecha_solicitado.Title = "Fecha Solicitado"; // titulo de la cabecera de la columna, si está visible
			col_fecha_solicitado.PackStart(cel_fecha_solicitado, true);
			col_fecha_solicitado.AddAttribute (cel_fecha_solicitado, "text", 6);
			col_fecha_solicitado.SortColumnId = (int) Column_solicitudes.col_fecha_solicitado;
						
			this.lista_de_materiales_solicitados.AppendColumn(col_surtir);
			this.lista_de_materiales_solicitados.AppendColumn(col_cant_solicitado);
			this.lista_de_materiales_solicitados.AppendColumn(col_autorizado);
			this.lista_de_materiales_solicitados.AppendColumn(col_nro_solicitud);
			this.lista_de_materiales_solicitados.AppendColumn(col_desc_producto);
			this.lista_de_materiales_solicitados.AppendColumn(col_idproducto);
			this.lista_de_materiales_solicitados.AppendColumn(col_fecha_solicitado);									
		}
		
		enum Column_solicitudes
		{
			col_surtir,
			col_cant_solicitado,
			col_autorizado,
			col_nro_solicitud,
			col_desc_producto,
			col_idproducto,
			col_fecha_solicitado
		}
		
		// Cuando seleccion el treeview de cargos extras para cargar los productos  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (lista_de_materiales_solicitados.Model.GetIter (out iter, path)) {
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
						"AND hscmty_productos.id_grupo_producto IN( '4','5','6','7','19') "+
						"AND hscmty_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper()+"%' ORDER BY descripcion_producto; ";
				
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
		
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
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
 														(string) model.GetValue(iterSelected, 5));
 					entry_cantidad_aplicada.Text = "0";
 				}else{
 					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error,ButtonsType.Close, 
											"La cantidad que quiere solicitar debe ser \n"+"distinta a cero, intente de nuevo");
					msgBoxError.Run ();					msgBoxError.Destroy();
 				}
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
				this.treeViewEngineSolicitado.SetValue(iter,(int) Column_solicitudes.col_autorizado,args.NewText);
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
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
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
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
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