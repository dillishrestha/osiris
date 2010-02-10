// created on 05/02/2008 at 10:06 a
//////////////////////////////////////////////////////////////////////
// created on 21/01/2008 at 08:28 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. (Programacion y Ajustes)
//                Ing. Jesus Buentello Garza (Ajustes)
//				  Homero Montoya (Ajustes)
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
// Programa		: hscmty.cs
// Proposito	:  
// Objeto		: 
//////////////////////////////////////////////////////////
using System;
using System.IO;
using Gtk;
using Npgsql;
using System.Data;
using Glade;

namespace osiris
{
	public class validacion_cargos_extras
	{
		//Ventana de Cargos Extras
		[Widget] Gtk.Window cargos_extras;
		[Widget] Gtk.RadioButton radiobutton_presupuesto;
		[Widget] Gtk.RadioButton radiobutton_folio;
		[Widget] Gtk.RadioButton radiobutton_paquete;
		[Widget] Gtk.TreeView treeview_cargos_extras;
		[Widget] Gtk.Button button_imprimir;
		[Widget] Gtk.Button button_salir;
		[Widget] Gtk.Button button_busca_paquete;
		[Widget] Gtk.Button button_busca_folio;
		[Widget] Gtk.Button button_busca_presupuesto;
		[Widget] Gtk.Button button_calcular;
		[Widget] Gtk.Entry entry_presupuesto;
		[Widget] Gtk.Entry entry_paquete;
		[Widget] Gtk.Entry entry_folio;
		[Widget] Gtk.Entry entry_tot_pre_uni;
		[Widget] Gtk.Entry entry_tot_iva;
		[Widget] Gtk.Entry entry_tot_tot;
		
		//Ventana de busqueda de cirugias
		[Widget] Gtk.Window busca_cirugias;
		//[Widget] Gtk.TreeView lista_de_busqueda;
		[Widget] Gtk.TreeView lista_cirugia;
		[Widget] Gtk.Button button_llena_cirugias;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Entry entry_expresion;
		//[Widget] Gtk.Entry entry_id_cirugia;
		//[Widget] Gtk.Entry entry_cirugia;
		
		//Ventana de busqueda de paquetes
		[Widget] Gtk.Window busca_paquete;
		[Widget] Gtk.Button button_buscar_busqueda;
		[Widget] Gtk.ComboBox combobox_tipo_busqueda;
		[Widget] Gtk.TreeView lista_de_paquete;
		
		string folioservicio;
		int idtipocirugia = 1;
		int idpaquete = 0;
		string tipobusqueda = "";
		string busqueda;
		string connectionString;
		string nombrebd;
		class_conexion conexion_a_DB = new class_conexion();
       	
		TreeStore treeViewEngineCargos;
		TreeStore treeViewEngineBusca;
		TreeStore treeViewEngineMedicos;
		
		CellRendererText cel_descripcion;
		
		CellRendererText cellr0;
		CellRendererText cellr1;
		CellRendererText cellr2;
		CellRendererText cellr3;
		CellRendererText cellr4;
		CellRendererText cellr5;
		CellRendererText cellr6;
		CellRendererText cellr7;
		CellRendererText cellr8;
		CellRendererText cellr9;
		CellRendererText cellr10;
		CellRendererText cellr11;
		CellRendererText cellr12;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		public validacion_cargos_extras(string folio_,string nombrebd_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			folioservicio = folio_;
			
			Glade.XML  gxml = new Glade.XML  (null, "caja.glade", "cargos_extras", null);
			gxml.Autoconnect  (this);	
			cargos_extras.Show();
			treeview_cargos_extras1();
			button_imprimir.Clicked += new EventHandler(imprime_reporte);
			button_busca_paquete.Clicked += new EventHandler(on_button_buscar_clicked);
			button_busca_presupuesto.Clicked += new EventHandler(on_button_buscar_presup_clicked);
			button_calcular.Clicked += new EventHandler(on_button_calcular_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			radiobutton_presupuesto.Clicked += new EventHandler(presupuesto);
			radiobutton_paquete.Clicked += new EventHandler(paquete);
			radiobutton_folio.Clicked += new EventHandler(on_folio);
			
			button_busca_presupuesto.Sensitive = false;
			entry_presupuesto.Sensitive = false;
			entry_paquete.Sensitive = false;
			button_busca_paquete.Sensitive = false;
			entry_folio.Sensitive = false;
			button_busca_folio.Sensitive = false;
			this.entry_tot_iva.Hide();
			this.entry_tot_pre_uni.Hide();
			this.entry_tot_tot.Hide();
		}
		
		void on_button_calcular_clicked(object sender, EventArgs args)
		{
			this.treeViewEngineCargos.Clear();
			string num_presu_paquete_folio = "";
			string nombre_tabla = "";
			string idproducto_tabla = "";
			string toma_totales = "";
			string folio_a_comparar = "";
			string toma_descripcion = "";
			string precio_del_producto = "";
			string grupo_producto = "";
			decimal calculadiferencia = 0;
			decimal iva = 0;
			decimal total = 0;
			decimal total_total = 0;
			decimal total_iva = 0;
			decimal total_precio = 0;
			decimal precios = 0;
			
			if (radiobutton_presupuesto.Active == true){
				num_presu_paquete_folio = "id_presupuesto = '"+this.entry_presupuesto.Text+"' ";
				nombre_tabla = "osiris_his_presupuestos_deta";
				folio_a_comparar = "id_presupuesto";
				precio_del_producto = ", to_char(osiris_his_presupuestos_deta.precio_producto,'99999999,99') AS precioproducto ";
				grupo_producto = "osiris_his_presupuestos_deta.precio_producto";
			}
			if (radiobutton_paquete.Active == true){
				num_presu_paquete_folio = "id_tipo_cirugia = '"+this.idtipocirugia.ToString().Trim()+"' ";
				nombre_tabla = "osiris_his_cirugias_deta";
				folio_a_comparar = "id_tipo_cirugia";
				precio_del_producto = ", to_char(osiris_productos.precio_producto_publico,'99999999,99') AS precioproducto ";
				grupo_producto = "osiris_productos.precio_producto_publico";
			}
			if (radiobutton_folio.Active == true){
				num_presu_paquete_folio = "folio_de_servicio = '"+this.entry_folio.Text+"' ";
				nombre_tabla = "osiris_erp_cobros_deta";
				folio_a_comparar = "folio_de_servicio";
				precio_del_producto = ", to_char(osiris_erp_cobros_deta.precio_producto,'99999999,99') AS precioproducto ";
				grupo_producto = "osiris_erp_cobros_deta.precio_producto";
			}
			
			// Total de todo el procedimiento para la compracion de cargos extras			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = comando.CommandText = "SELECT folio_de_servicio,to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,osiris_productos.descripcion_producto,"+
									"to_char(SUM(cantidad_aplicada),'999999999,99') AS cantidadaplicada,SUM(cantidad_aplicada*precio_producto) AS totalprecioporcantidad,"+
									"SUM(porcentage_utilidad*cantidad_aplicada) AS porcentageutilidad,"+
									"(porcentage_utilidad*cantidad_aplicada)/cantidad_aplicada AS porcentageutilidad_promedio,"+
                                    "precio_costo_unitario "+
									"FROM osiris_erp_cobros_deta,osiris_productos "+
									"WHERE folio_de_servicio = '"+this.folioservicio.Trim()+"' "+
									"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+
									"AND osiris_erp_cobros_deta.eliminado = 'false' "+ 
									"GROUP BY osiris_erp_cobros_deta.id_producto,osiris_productos.descripcion_producto,folio_de_servicio,porcentageutilidad_promedio,precio_costo_unitario "+
									"ORDER BY osiris_erp_cobros_deta.id_producto;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){
					
					idproducto_tabla= (string) lector["idproducto"];
					toma_totales = (string) lector["cantidadaplicada"];
					toma_descripcion = (string) lector["descripcion_producto"];
					 
					NpgsqlConnection conexion1; 
					conexion1 = new NpgsqlConnection (connectionString+nombrebd);
					try{
						conexion1.Open ();
						NpgsqlCommand comando1; 
						comando1 = conexion1.CreateCommand ();
						comando1.CommandText = "SELECT "+folio_a_comparar+",to_char("+nombre_tabla+".id_producto,'999999999999') AS idproducto,osiris_productos.descripcion_producto,"+
											"to_char(SUM(cantidad_aplicada),'999999999,99') AS cantidadaplicada,"+
								            "aplicar_iva "+
								             precio_del_producto+" "+
											"FROM osiris_productos,"+nombre_tabla+" "+
											"WHERE "+num_presu_paquete_folio+" "+
											"AND "+nombre_tabla+".id_producto = osiris_productos.id_producto "+
											"AND "+nombre_tabla+".eliminado = 'false' "+
											"AND "+nombre_tabla+".id_producto = '"+idproducto_tabla+"' "+
											"GROUP BY "+nombre_tabla+".id_producto,"+grupo_producto+",aplicar_iva,osiris_productos.descripcion_producto,"+folio_a_comparar+";";
						//Console.WriteLine(comando1.CommandText);
						NpgsqlDataReader lector1 = comando1.ExecuteReader ();
						//Console.WriteLine(nombre_tabla);
						if(lector1.Read()){
							//calculando el iva 
							if((bool) lector1["aplicar_iva"] == true){
								//iva = 0;
								iva = Convert.ToDecimal((Convert.ToDouble((string) lector1["precioproducto"]) * 1.15) - Convert.ToDouble((string) lector1["precioproducto"]));
								
							}else{ 
								iva = 0;
									}
							//calcula el total iva + precio
							total = 0;
							total = iva + Convert.ToDecimal((string) lector1["precioproducto"]);
							precios = Convert.ToDecimal((string) lector1["precioproducto"]);
							
							//Console.WriteLine("toma_totales = "+toma_totales);
							//Console.WriteLine("cantidadaplicada = "+(string) lector1["cantidadaplicada"]);
							
							calculadiferencia = decimal.Parse(toma_totales) - decimal.Parse((string) lector1["cantidadaplicada"]);
							
							if (decimal.Parse(toma_totales) != decimal.Parse((string) lector1["cantidadaplicada"])){
								treeViewEngineCargos.AppendValues (idproducto_tabla,
															(string) lector1["descripcion_producto"],
															calculadiferencia.ToString().Trim(),"(Extra) Rebaso el limite en el Paquete",(string) lector1["precioproducto"],iva.ToString("F"),total.ToString("F"));
							}else{
								treeViewEngineCargos.AppendValues (idproducto_tabla,
															toma_descripcion,
															calculadiferencia.ToString().Trim(),"Esta dentro del Paquete",(string) lector1["precioproducto"],iva.ToString("F"),total.ToString("F"));
							}
						}else{
							treeViewEngineCargos.AppendValues (idproducto_tabla,
															toma_descripcion,
															toma_totales,"(Extra) No esta en el Paquete"," ",iva.ToString("F"),total.ToString("F"));
						}	
					}catch (NpgsqlException ex){
				   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();			msgBoxError.Destroy();
					}
					conexion1.Close ();
				
				}
				//totales entrys
				total_precio += precios;
				total_total += total;
				total_iva += iva;
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
			
			// Total de todo el procedimiento para la compracion de cargos extras			
			NpgsqlConnection conexion2; 
			conexion2 = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion2.Open ();
				NpgsqlCommand comando2; 
				comando2 = conexion2.CreateCommand ();
				comando2.CommandText = "SELECT "+folio_a_comparar+",to_char("+nombre_tabla+".id_producto,'999999999999') AS idproducto,osiris_productos.descripcion_producto,"+
											"to_char(SUM(cantidad_aplicada),'999999999,99') AS cantidadaplicada,"+
						                    "aplicar_iva "+
											precio_del_producto+" "+
											"FROM osiris_productos,"+nombre_tabla+" "+
											"WHERE "+num_presu_paquete_folio+" "+
											"AND "+nombre_tabla+".id_producto = osiris_productos.id_producto "+
											"AND "+nombre_tabla+".eliminado = 'false' "+
											"GROUP BY "+nombre_tabla+".id_producto,aplicar_iva,"+grupo_producto+",osiris_productos.descripcion_producto,"+folio_a_comparar+";";
				//Console.WriteLine(comando2.CommandText);
				NpgsqlDataReader lector2 = comando2.ExecuteReader ();
				while(lector2.Read()){
					
					idproducto_tabla= (string) lector2["idproducto"];
					toma_totales = (string) lector2["cantidadaplicada"];
					toma_descripcion = (string) lector2["descripcion_producto"];
					precio_del_producto = (string) lector2["precioproducto"];
					
					//calculando el iva
					if((bool) lector2["aplicar_iva"] == true){
						iva = 0;
						iva = Convert.ToDecimal((Convert.ToDouble((string) lector2["precioproducto"]) * 1.15) - Convert.ToDouble((string) lector2["precioproducto"]));
						
					}else{ 
						iva = 0;
					}
					//calcula el total iva + precio
					total = 0;
					total = iva + Convert.ToDecimal((string) lector2["precioproducto"]);
					precios = Convert.ToDecimal((string) lector2["precioproducto"]);								
					//totales entrys
					total_precio += precios;
					total_total += total;
					total_iva += iva;
					NpgsqlConnection conexion3; 
					conexion3 = new NpgsqlConnection (connectionString+nombrebd);
					try{
						conexion3.Open ();
						NpgsqlCommand comando3; 
						comando3 = conexion3.CreateCommand ();
						comando3.CommandText = "SELECT folio_de_servicio,to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,osiris_productos.descripcion_producto,"+
									"to_char(SUM(cantidad_aplicada),'999999999,99') AS cantidadaplicada,SUM(cantidad_aplicada*precio_producto) AS totalprecioporcantidad,"+
									"SUM(porcentage_utilidad*cantidad_aplicada) AS porcentageutilidad,"+
									"(porcentage_utilidad*cantidad_aplicada)/cantidad_aplicada AS porcentageutilidad_promedio "+
									"FROM osiris_erp_cobros_deta,osiris_productos "+
									"WHERE folio_de_servicio = '"+this.folioservicio.Trim()+"' "+
									"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+
									"AND osiris_erp_cobros_deta.eliminado = 'false' "+
									"AND osiris_erp_cobros_deta.id_producto = '"+idproducto_tabla+"' "+ 
									"GROUP BY osiris_erp_cobros_deta.id_producto,osiris_productos.descripcion_producto,folio_de_servicio,porcentageutilidad_promedio "+
									"ORDER BY osiris_erp_cobros_deta.id_producto;";
						//Console.WriteLine(comando3.CommandText);
						NpgsqlDataReader lector3 = comando3.ExecuteReader ();

						if(!lector3.Read()){
							treeViewEngineCargos.AppendValues (idproducto_tabla,
															toma_descripcion,
															toma_totales.ToString().Trim(),"No aplico este producto al Proced.",precio_del_producto,iva.ToString("F"),total.ToString("F"));
						}				
					}catch (NpgsqlException ex){
				   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();			msgBoxError.Destroy();
					}
					
					conexion3.Close ();
					
				}				
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion2.Close ();  
			this.entry_tot_iva.Text = Convert.ToString(total_iva);
			this.entry_tot_tot.Text = Convert.ToString(total_total);
			this.entry_tot_pre_uni.Text = Convert.ToString(total_precio);			
		}
		
		void presupuesto(object sender, EventArgs args)
		{
			button_busca_presupuesto.Sensitive = true;
			entry_presupuesto.Sensitive = true;
			entry_paquete.Sensitive = false;
			button_busca_paquete.Sensitive = false;
			entry_folio.Sensitive = false;
			button_busca_folio.Sensitive = false;
		}
		
		void paquete(object sender, EventArgs args)
		{
			entry_paquete.Sensitive = true;
			button_busca_paquete.Sensitive = true;
			button_busca_presupuesto.Sensitive = false;
			entry_presupuesto.Sensitive = false;
			entry_folio.Sensitive = false;
			button_busca_folio.Sensitive = false;
		}
		
		void on_folio(object sender, EventArgs args)
		{
			entry_folio.Sensitive = true;
			entry_paquete.Sensitive = false;
			button_busca_paquete.Sensitive = false;
			button_busca_presupuesto.Sensitive = false;
			entry_presupuesto.Sensitive = false;
		}
		
		void on_button_buscar_presup_clicked(object sender, EventArgs args)
		{
			//busqueda = "medicos";
			
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "busca_paquete", null);
			gxml.Autoconnect(this);
			busca_paquete.Show();
			
	        llenado_cmbox_tipo_busqueda();
	        
	        entry_expresion.KeyPressEvent += onKeyPressEvent_busqueda;
			button_buscar_busqueda.Clicked += new EventHandler(on_button_llena_medicos_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_medico_clicked);
	        button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
				
			treeViewEngineMedicos = new TreeStore(typeof(int),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));
													
			lista_de_paquete.Model = treeViewEngineMedicos;
			lista_de_paquete.RulesHint = true;
			lista_de_paquete.RowActivated += on_selecciona_medico_clicked;  // Doble click selecciono paciente
					
			TreeViewColumn col_idpresup = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idpresup.Title = "ID Presupuesto"; // titulo de la cabecera de la columna, si está visible
			col_idpresup.PackStart(cellr0, true);
			col_idpresup.AddAttribute (cellr0, "text", 0);
			col_idpresup.SortColumnId = (int) Coldatos_medicos.col_idpresup;    
	           
			TreeViewColumn col_depmin = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_depmin.Title = "Deposito Minimo";
			col_depmin.PackStart(cellrt1, true);
			col_depmin.AddAttribute (cellrt1, "text", 1);
			col_depmin.SortColumnId = (int) Coldatos_medicos.col_depmin; 
	            
			TreeViewColumn col_preconv = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_preconv.Title = "Precio Convenido";
			col_preconv.PackStart(cellrt2, true);
			col_preconv.AddAttribute (cellrt2, "text", 2);
			col_preconv.SortColumnId = (int) Coldatos_medicos.col_preconv; 
				
			TreeViewColumn col_telmed = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_telmed.Title = "Telefono Medico";
			col_telmed.PackStart(cellrt3, true);
			col_telmed.AddAttribute (cellrt3, "text", 3);
			col_telmed.SortColumnId = (int) Coldatos_medicos.col_telmed;
				
			TreeViewColumn col_medico = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_medico.Title = "Medico Profesional";
			col_medico.PackStart(cellrt4, true);
			col_medico.AddAttribute (cellrt4, "text", 4);
			col_medico.SortColumnId = (int) Coldatos_medicos.col_medico;
	            
	                                    
			lista_de_paquete.AppendColumn(col_idpresup);
			lista_de_paquete.AppendColumn(col_depmin);
			lista_de_paquete.AppendColumn(col_preconv);
			lista_de_paquete.AppendColumn(col_telmed);
			lista_de_paquete.AppendColumn(col_medico);
		}
		
		enum Coldatos_medicos
		{
			col_idpresup,
			col_depmin,
			col_preconv,
			col_telmed,
			col_medico
		}
		
		void llenado_cmbox_tipo_busqueda()
		{
			combobox_tipo_busqueda.Clear();
			CellRendererText cell1 = new CellRendererText();
			combobox_tipo_busqueda.PackStart(cell1, true);
			combobox_tipo_busqueda.AddAttribute(cell1,"text",0);
			ListStore store1 = new ListStore( typeof (string),typeof (int));
			combobox_tipo_busqueda.Model = store1;
	        
			store1.AppendValues ("",0);
			store1.AppendValues ("ID PRESUPUESTO",1);
							              
			TreeIter iter1;
			if (store1.GetIterFirst(out iter1)){
				combobox_tipo_busqueda.SetActiveIter (iter1);
			}
			combobox_tipo_busqueda.Changed += new EventHandler (onComboBoxChanged_tipo_busqueda);
		}
		
		void onComboBoxChanged_tipo_busqueda (object sender, EventArgs args)
		{
	    	ComboBox combobox_tipo_busqueda = sender as ComboBox;
			if (sender == null)	
				{	
					return;	
				}
			TreeIter iter;			int numbusqueda = 0;
			if (combobox_tipo_busqueda.GetActiveIter (out iter))
			{
				numbusqueda = (int) combobox_tipo_busqueda.Model.GetValue(iter,1);
				tipo_de_busqueda_de_paquete(numbusqueda);
				llenando_lista_de_medicos();
			}
		}
		
		void tipo_de_busqueda_de_paquete(int numbusqueda)
		{
			if(numbusqueda == 0)  { tipobusqueda = "";}
			if(numbusqueda == 1)  { tipobusqueda = "AND osiris_his_presupuestos_enca.id_presupuesto LIKE '";	}
		}		
		
		void on_selecciona_medico_clicked (object sender, EventArgs args)
		{
			TreeModel model;			TreeIter iterSelected;
			if (lista_de_paquete.Selection.GetSelected(out model, out iterSelected)) 
			{
					string suma1 = "";
					idpaquete = (int) model.GetValue(iterSelected, 0);
					suma1 = idpaquete.ToString();
					entry_presupuesto.Text = suma1.ToString();
					Widget win = (Widget) sender;
					win.Toplevel.Destroy();
			}
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
					treeViewEngineMedicos.Clear();
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
		            try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						if ((string) entry_expresion.Text.ToUpper() == "")	{
						comando.CommandText = comando.CommandText = "SELECT id_presupuesto,telefono_medico,medico_provisional, "+
														"to_char(osiris_his_presupuestos_enca.deposito_minimo,'999999999.99') AS deposito_min, "+
														"to_char(osiris_his_presupuestos_enca.precio_convenido,'999999999.99') AS prec_convenido "+
														"FROM osiris_his_presupuestos_enca "+
														"WHERE id_presupuesto > 0 "+
														"ORDER BY id_presupuesto;";
						}else{
						comando.CommandText = comando.CommandText = "SELECT id_presupuesto,telefono_medico,medico_provisional, "+
														"to_char(osiris_his_presupuestos_enca.deposito_minimo,'999999999.99') AS deposito_min, "+
														"to_char(osiris_his_presupuestos_enca.precio_convenido,'999999999.99') AS prec_convenido "+
														"FROM osiris_his_presupuestos_enca "+
														"WHERE id_presupuesto > 0 "+
														"AND id_presupuesto LIKE '%"+entry_expresion.Text.ToUpper()+"%' "+
														"ORDER BY id_presupuesto;";
						}
						NpgsqlDataReader lector = comando.ExecuteReader ();
						//Console.WriteLine("medicos"+comando.CommandText);
						while (lector.Read())
						{
							treeViewEngineMedicos.AppendValues ((int) lector["id_presupuesto"],//0
										(string) lector["deposito_min"],//1
										(string) lector["prec_convenido"],//2
										(string) lector["telefono_medico"],//3
										(string) lector["medico_provisional"]//4
										);
						}
					}catch (NpgsqlException ex){
			   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();			msgBoxError.Destroy();
					}
					conexion.Close ();
				}
			}
		}
		
		void imprime_reporte(object sender, EventArgs args)
		{
			//new osiris.rpt_cargos_extras(nombrebd,treeViewEngineCargos,treeview_cargos_extras);
		}
		
		void treeview_cargos_extras1()
		{
			treeViewEngineCargos = new TreeStore(typeof(string),//0
													typeof(string),//1
													typeof(string),//2
													typeof(string),//3
													typeof(string),//4
													typeof(string),//5
													typeof(string)//6
													);
			treeview_cargos_extras.Model = treeViewEngineCargos;
			treeview_cargos_extras.RulesHint = true;
			treeview_cargos_extras.RowActivated += on_selecciona_cirugia_clicked;  // Doble click selecciono paciente*/
			
			TreeViewColumn col_idprod = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idprod.Title = "ID Producto"; // titulo de la cabecera de la columna, si está visible
			col_idprod.PackStart(cellr0, true);
			col_idprod.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1
			col_idprod.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_idprod.SortColumnId = (int) Col_proveedores.col_idprod;
			
			TreeViewColumn col_descri = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_descri.Title = "Descripcion";
			col_descri.PackStart(cellrt1, true);
			col_descri.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
			col_descri.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_descri.SortColumnId = (int) Col_proveedores.col_descri;
			col_descri.Resizable = true;
			cellrt1.Width = 450;
			
			TreeViewColumn col_diferencia = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_diferencia.Title = "Diferencia";
			col_diferencia.PackStart(cellrt2, true);
			col_diferencia.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 3
			col_diferencia.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_diferencia.SortColumnId = (int) Col_proveedores.col_diferencia;
			
			TreeViewColumn col_nuevo = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_nuevo.Title = "Nuevo";
			col_nuevo.PackStart(cellrt3, true);
			col_nuevo.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 4
			col_nuevo.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_nuevo.SortColumnId = (int) Col_proveedores.col_nuevo;
			
            TreeViewColumn col_pre_unitario = new TreeViewColumn();
            CellRendererText cellrt4 = new CellRendererText();
            col_pre_unitario.Title = "Precio Unitario";
            col_pre_unitario.PackStart(cellrt4, true);
			col_pre_unitario.AddAttribute(cellrt4,"text", 4); // la siguiente columna será 5
			col_pre_unitario.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_pre_unitario.SortColumnId = (int) Col_proveedores.col_pre_unitario;
			
            TreeViewColumn col_iva = new TreeViewColumn();
            CellRendererText cellrt5 = new CellRendererText();
            col_iva.Title = "IVA";
            col_iva.PackStart(cellrt5, true);
            col_iva.AddAttribute(cellrt5,"text", 5); // la siguiente columna será 6
            col_iva.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_iva.SortColumnId = (int) Col_proveedores.col_iva;
			
            TreeViewColumn col_total = new TreeViewColumn();
            CellRendererText cellrt6 = new CellRendererText();
            col_total.Title = "Total";
            col_total.PackStart(cellrt6, true);
            col_total.AddAttribute(cellrt6,"text", 6); // la siguiente columna será 7
            col_total.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
            col_total.SortColumnId = (int) Col_proveedores.col_total;
            
            treeview_cargos_extras.AppendColumn(col_idprod);
			treeview_cargos_extras.AppendColumn(col_descri);
			treeview_cargos_extras.AppendColumn(col_diferencia);
			treeview_cargos_extras.AppendColumn(col_nuevo);
			treeview_cargos_extras.AppendColumn(col_pre_unitario);
			treeview_cargos_extras.AppendColumn(col_iva);
			treeview_cargos_extras.AppendColumn(col_total);
		}
		
		enum Col_proveedores
		{
			col_idprod,
			col_descri,
			col_diferencia,
			col_nuevo,
			col_pre_unitario,
			col_iva,
			col_total
		}
		
		void cambia_colores_proveedor(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//if ((bool) lista_de_busqueda.Model.GetValue(iter,10) == false)
			//{(cell as Gtk.CellRendererText).Foreground = "darkgreen";		}
		}
		
		void on_button_buscar_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "busca_cirugias", null);
			gxml.Autoconnect (this);
			busca_cirugias.Show();
			
			tipobusqueda ="cirugia";
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_cirugia_clicked);
			button_llena_cirugias.Clicked += new EventHandler(on_button_llena_cirugias_clicked);
	        treeViewEngineBusca = new TreeStore( typeof(int), typeof(string));
			lista_cirugia.Model = treeViewEngineBusca;
			lista_cirugia.RulesHint = true;
			lista_cirugia.RowActivated += on_selecciona_cirugia_clicked;  // Doble click selecciono paciente
			
			TreeViewColumn col_idcirugia = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idcirugia.Title = "ID Cirugia"; // titulo de la cabecera de la columna, si está visible
			col_idcirugia.PackStart(cellr0, true);
			col_idcirugia.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
			
			TreeViewColumn col_descripcirugia = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_descripcirugia.Title = "Descripcion de Cirugia";
			col_descripcirugia.PackStart(cellrt1, true);
			col_descripcirugia.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 1 en vez de 2
			
			lista_cirugia.AppendColumn(col_idcirugia);
			lista_cirugia.AppendColumn(col_descripcirugia);
		}
		
		void on_selecciona_cirugia_clicked (object sender, EventArgs args)
		{
			TreeModel model;			TreeIter iterSelected;
			if (lista_cirugia.Selection.GetSelected(out model, out iterSelected)){
				idtipocirugia = (int) model.GetValue(iterSelected, 0);
				entry_paquete.Text = (string) model.GetValue(iterSelected, 1);
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
				
			}
		}
		
		void on_button_llena_cirugias_clicked(object sender, EventArgs args)
		{
			llena_lista_de_busqueda();
		}
		
		void llena_lista_de_busqueda() 
		{
			 	treeViewEngineBusca.Clear();// Limpia el treeview cuando realiza una nueva busqueda
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
           		// Verifica que la base de datos este conectada
				try
				{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
		              	if ((string) entry_expresion.Text.ToUpper() == "")	
		              	{
							comando.CommandText ="SELECT * FROM osiris_his_tipo_cirugias "+
												"WHERE tiene_paquete = 'true' "+
												" ORDER BY id_tipo_cirugia;";
						}
						NpgsqlDataReader lector = comando.ExecuteReader ();
						while (lector.Read())	
						{
							treeViewEngineBusca.AppendValues ((int) lector["id_tipo_cirugia"],(string) lector["descripcion_cirugia"]);//TreeIter iter =
						}
					}
					catch (NpgsqlException ex)
					{
		   					Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
							msgBoxError.Run ();		msgBoxError.Destroy();
					}
				conexion.Close ();
		}
		
		// Valida entradas que solo sean numericas, se utiliza en ventana de
		//de rangos de fechas
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮ）（ｔｒｓｑ ";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
			{
				args.RetVal = true;
			}
		}
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_busqueda(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				if(busqueda == "medicos") { llenando_lista_de_medicos(); } 		
			}
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
	
	//
	// CLASE PARA IMPRIMIR LOS CARGOS EXTRAS DENTRO DE LOS PROCEDIMIENTOS
	// 
	
	public class rpt_cargos_extras
	{
	}
	
		/*
		public string connectionString;
		public string nombrebd;
	   	public string folio;
	   	public string tiporeporte = "DIFERENCIAS";
		public string titulo = "REPORTE DE DIFERNCIAS";
		
		public int columna = 0;
		public int filas = 710;
		public int contador = 1;
		public int numpage = 1;
		
		public string query_fechas = " ";
		public string orden = " ";
		public string rango1 = "";
		public string rango2 = "";
		public string descrip_prod = "";
		

		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		public Gnome.Font fuente6 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
		public Gnome.Font fuente7 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
		public Gnome.Font fuente8 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
		public Gnome.Font fuente9 = Gnome.Font.FindClosest("Bitstream Vera Sans", 9);
		public Gnome.Font fuente10 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
		public Gnome.Font fuente11 = Gnome.Font.FindClosest("Bitstream Vera Sans", 11);
		public Gnome.Font fuente12 = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
		public Gnome.Font fuente36 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
		
		private Gtk.TreeStore treeViewEngineCargos;
		private Gtk.TreeView treeview_cargos_extras;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		public rpt_cargos_extras(string nombrebd_,object treeViewEngineCargos_,object treeview_cargos_extras_)
		{	

			nombrebd = nombrebd_;
			treeview_cargos_extras = treeview_cargos_extras_ as Gtk.TreeView;
			treeViewEngineCargos =  treeViewEngineCargos_ as Gtk.TreeStore;

			
			titulo = "PAQUTES";

			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, titulo, 0);
        	int         respuesta = dialogo.Run ();

			if (respuesta == (int) Gnome.PrintButtons.Cancel){
				dialogo.Hide (); 		dialogo.Dispose (); 
				return;
			}
        	Gnome.PrintContext ctx = trabajo.Context;        
        	ComponerPagina(ctx, trabajo); 
        	trabajo.Close();             
        	switch (respuesta){
                  case (int) PrintButtons.Print:   
                  		trabajo.Print (); 
                  		break;
                  case (int) PrintButtons.Preview:
				         new PrintJobPreview(trabajo, titulo).Show();
                         break;
        	}
        	dialogo.Hide (); dialogo.Dispose ();
		}

		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{	
				        	ContextoImp.BeginPage("Pagina 1");
		    imprime_encabezado(ContextoImp,trabajoImpresion);

			TreeIter iter;
			
			if (treeViewEngineCargos.GetIterFirst (out iter)){
				
				descrip_prod = (string) treeview_cargos_extras.Model.GetValue (iter,1);					
					
				if(descrip_prod.Length > 60){
							descrip_prod = descrip_prod.Substring(0,59);
					}
			      		
				Gnome.Print.Setfont (ContextoImp, fuente7);				
				ContextoImp.MoveTo(070, filas);	ContextoImp.Show(descrip_prod);
				ContextoImp.MoveTo(010, filas);	ContextoImp.Show((string) treeview_cargos_extras.Model.GetValue (iter,0));		
				ContextoImp.MoveTo(330, filas);	ContextoImp.Show((string) treeview_cargos_extras.Model.GetValue (iter,2));	
				ContextoImp.MoveTo(360, filas);	ContextoImp.Show((string) treeview_cargos_extras.Model.GetValue (iter,3));	
				ContextoImp.MoveTo(420, filas);	ContextoImp.Show((string) treeview_cargos_extras.Model.GetValue (iter,4));	
				ContextoImp.MoveTo(490, filas);	ContextoImp.Show((string) treeview_cargos_extras.Model.GetValue (iter,5));	
				ContextoImp.MoveTo(530, filas);	ContextoImp.Show((string) treeview_cargos_extras.Model.GetValue (iter,6));									
				filas-=10;
				contador+=1;
				salto_pagina(ContextoImp,trabajoImpresion);
			}
			
			while (treeViewEngineCargos.IterNext(ref iter)){

				descrip_prod = (string) treeview_cargos_extras.Model.GetValue (iter,1);
				
				if(descrip_prod.Length > 60){
						descrip_prod = descrip_prod.Substring(0,59);
				}  		

				Gnome.Print.Setfont (ContextoImp, fuente7);
				ContextoImp.MoveTo(070, filas);	ContextoImp.Show(descrip_prod);
				ContextoImp.MoveTo(010, filas);	ContextoImp.Show((string) treeview_cargos_extras.Model.GetValue (iter,0));		
				ContextoImp.MoveTo(330, filas);	ContextoImp.Show((string) treeview_cargos_extras.Model.GetValue (iter,2));	
				ContextoImp.MoveTo(360, filas);	ContextoImp.Show((string) treeview_cargos_extras.Model.GetValue (iter,3));	
				ContextoImp.MoveTo(420, filas);	ContextoImp.Show((string) treeview_cargos_extras.Model.GetValue (iter,4));	
				ContextoImp.MoveTo(490, filas);	ContextoImp.Show((string) treeview_cargos_extras.Model.GetValue (iter,5));	
				ContextoImp.MoveTo(530, filas);	ContextoImp.Show((string) treeview_cargos_extras.Model.GetValue (iter,6));	
				filas-=10;
				contador+=1;
				salto_pagina(ContextoImp,trabajoImpresion);

			}				
						ContextoImp.ShowPage();	
		}
		
		void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			if (contador > 55 ){
	        	contador = 1;	
	        	filas = 700;
	        	ContextoImp.ShowPage();
	        	ContextoImp.BeginPage("Pagina "+numpage.ToString());
				imprime_encabezado(ContextoImp,trabajoImpresion);				
				numpage += 1;
			}
		}
		
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{

      		Gnome.Print.Setfont (ContextoImp, fuente6);
			ContextoImp.MoveTo(19.7, 780);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(20, 780);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(19.7, 770);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(20, 770);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(19.7, 760);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			ContextoImp.MoveTo(20, 760);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			ContextoImp.MoveTo(300, 740);			ContextoImp.Show(" COMPARACION ");
			ContextoImp.MoveTo(300.3, 740);			ContextoImp.Show(" COMPARACION ");
			ContextoImp.MoveTo(15, 720);			ContextoImp.Show("ID Producto");
			ContextoImp.MoveTo(140, 720);			ContextoImp.Show("Descripcion");
			ContextoImp.MoveTo(320, 720);			ContextoImp.Show("Diferencia");
			ContextoImp.MoveTo(365, 720);			ContextoImp.Show("Nuevo");
			ContextoImp.MoveTo(430, 720);			ContextoImp.Show("Precio Unitario");
			ContextoImp.MoveTo(490, 720);			ContextoImp.Show("IVA");
			ContextoImp.MoveTo(530, 720);			ContextoImp.Show("Total");
			

		} 
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		*/
}
		