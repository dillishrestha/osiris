// created on 11/05/2007 at 09:43 a
///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: ing. Juan Antonio Peña Gonzalez (gjuanzz@gmail.com)
//				  Ing. Daniel Olivares C. (Preprogramacion y Ajustes) 
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
// Programa		: presupuestos_quirurgicos.cs
// Proposito	: Crear presupuestos de productos que se aplican en cirugia
// Objeto		: presupuestos_quirurgicos.cs
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using System.Collections;

namespace osiris
{
	public class presupuestos_cirugias
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		//[Widget] Gtk.Button button_buscar_busqueda;
		
		// Declarando ventana principal
		[Widget] Gtk.Window presupuestos;
		[Widget] Gtk.Entry entry_id_presupuesto;
		[Widget] Gtk.Entry entry_cirugia;
		[Widget] Gtk.Entry entry_medico;
		//[Widget] Gtk.Entry entry_descripcion_especialidad;
		[Widget] Gtk.Entry entry_dias_internamiento;
		[Widget] Gtk.Entry entry_deposito_minimo;
		[Widget] Gtk.Entry entry_precio_convenido;
		[Widget] Gtk.Entry entry_tel_medico;
		[Widget] Gtk.Entry entry_tel_opcional;
		[Widget] Gtk.Entry entry_fax;
		[Widget] Gtk.Entry entry_notas;
		
		//Declaracion de checkbuttons
		[Widget] Gtk.CheckButton checkbutton_copia_productos;
		
		[Widget] Gtk.TreeView lista_de_servicios;
		//[Widget] Gtk.ProgressBar progressbar_status_llenado;
		[Widget] Gtk.CheckButton checkbutton_nuevo_presupuesto;
		[Widget] Gtk.Button button_quitar_aplicados;
		[Widget] Gtk.Button button_actualizar;
		//[Widget] Gtk.Button button_buscar_presupuesto;
		[Widget] Gtk.Button button_buscar_cirugia;
		[Widget] Gtk.Button button_enviado;
		//[Widget] Gtk.Button button_buscar_especialidad;
		[Widget] Gtk.Button button_selec_id;
		[Widget] Gtk.Button button_graba_presupuesto;
		[Widget] Gtk.Button button_limpiar;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.Button button_procedimiento_cobrz;
		[Widget] Gtk.Button button_hoja_sin_precios;
		[Widget] Gtk.Button button_busca_medico;
		[Widget] Gtk.Button button_copia_procedimiento;
		[Widget] Gtk.Button button_copia_paquete;
		
		[Widget] Gtk.Entry entry_subtotal_al_15;
		[Widget] Gtk.Entry entry_subtotal_al_0;
		[Widget] Gtk.Entry entry_total_iva;
		[Widget] Gtk.Entry entry_subtotal;
		[Widget] Gtk.Entry entry_total;
		//[Widget] Gtk.Entry entry_a_pagar;
		
		//Declarando ventena de carga de folio
		[Widget] Gtk.Window carga_folio;
		[Widget] Gtk.Label titulo_ventana_copia;
		[Widget] Gtk.Label tipo_copiado;
		[Widget] Gtk.Entry entry_folio;
		[Widget] Gtk.Button button_carga_productos;
		
		
		//Declarando la barra de estado
		[Widget] Gtk.Statusbar statusbar_presupuestos;
		
		/////// Ventana Busqueda de productos\\\\\\\\
		//[Widget] Gtk.Window busca_producto;
		[Widget] Gtk.TreeView lista_de_producto;
		[Widget] Gtk.ComboBox combobox_tipo_admision;
		
		//ventana de busqueda de medicos
		[Widget] Gtk.Button button_buscar_busqueda;
		[Widget] Gtk.TreeView lista_de_medicos;
		[Widget] Gtk.ComboBox combobox_tipo_busqueda;
		
		[Widget] Gtk.Entry entry_cantidad_aplicada;
		
		///////Ventana de Busqueda de cirugias
		[Widget] Gtk.TreeView lista_cirugia;
		[Widget] Gtk.Label titulo_ventana_busca_cirugias;
		[Widget] Gtk.Button button_llena_cirugias;
		
		TreeStore treeViewEngineBusca;
		TreeStore treeViewEngineBusca2;
		TreeStore treeViewEngineServicio;
		TreeStore treeViewEngineMedicos;
		
		//private ArrayList arraycargosrealizados;
		
		// Declaracion de variables publicas
		string idpresupuesto = "1";
		int idtipocirugia = 1;	        			// Toma el valor de numero de atencion de paciente
		string cirugia;
		int idtipoesp = 1;
		string especialidad;
		string nommedico;
		int id_medico = 1;
		
		float valoriva;
		
		string id_produ = "";
		string desc_produ = "";
		string precio_produ ="";
		string iva_produ ="";
		string total_produ ="";
		string costo_unitario_producto;
		string porcentage_utilidad_producto;
		string costo_total_producto;
		string ppcant ="";
		string constante = "";
		string agrupacion = "";
		float ppcantidad = 0;
		float valor_descuento = 0;
		//Variables de admision
		int idtipointernamiento = 0;
		string descripinternamiento = "";
		bool copiaproductos = false;
		string busqueda = "";
		string tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";
		bool tienepaquete = false;
		bool enviado = false;
		
		// Sumas Totales para los calculos
		float subtotal_al_15;
		float subtotal_al_0;
		float total_iva;
		float sub_total;
		float totaldescuento;
		
		bool aplico_cargos = false;
		
		string LoginEmpleado;
		
		string connectionString;
		string nombrebd;
				
		CellRendererText cel_descripcion;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public presupuestos_cirugias(string LoginEmp, string NomEmpleado, string AppEmpleado, string ApmEmpleado, string nombrebd_ ) 
		{
			LoginEmpleado = LoginEmp;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;			
			valoriva = float.Parse(classpublic.ivaparaaplicar);
			
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "presupuestos", null);
			gxml.Autoconnect (this);
	        // Muestra ventana de Glade
	        presupuestos.Show();
			
			// Voy a buscar el folio que capturo
			button_selec_id.Clicked += new EventHandler(on_selec_id_clicked);//1
			// Validando que sen solo numeros
			entry_id_presupuesto.KeyPressEvent += onKeyPressEvent_enter_id;//2
			// Validacion de nuevo procedimiento
			checkbutton_nuevo_presupuesto.Clicked += new EventHandler(on_checkbutton_nuevo_presupuesto_clicked);
			// Imprime Procedimiento
			button_procedimiento_cobrz.Clicked += new EventHandler(on_button_procedimiento_cobrz_clicked);//3
			// Imprime Presupuesto sin precios
			button_hoja_sin_precios.Clicked += new EventHandler(on_button_hoja_sin_precios_clicked);
			// Actualiza lista de cobros aplicados
			button_actualizar.Clicked += new EventHandler(on_button_actualizar_clicked);//4
			// Activacion de grabacion de informacion
	    	button_graba_presupuesto.Clicked += new EventHandler(on_button_graba_presupuesto_clicked);//5
			// Activacion de boton de busqueda
			button_buscar_cirugia.Clicked += new EventHandler(on_button_buscar_cirugia_clicked);//6
			//busca a los medicos
			button_busca_medico.Clicked += new EventHandler(on_button_busca_medico_clicked);//8
			// Busqueda de Productos
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);//11
			//limpia todos los valores de la ventana
			button_limpiar.Clicked += new EventHandler(on_button_limpiar_clicked);//12
			//Carga los productos de todo un folio
			button_copia_procedimiento.Clicked += new EventHandler(on_button_copia_procedimiento_clicked);//9
			//boton para copiar todo un paquete
			button_copia_paquete.Clicked += new EventHandler(on_button_copia_paquete_clicked);
			//quita lementos aplicados
			button_quitar_aplicados.Clicked += new EventHandler(on_button_quitar_aplicados_clicked);//10
			//activa el boton de entrega de presupuesto
			button_enviado.Clicked += new EventHandler(on_button_enviado_clicked);
			//Se activa la opcion de copiar productos
			checkbutton_copia_productos.Clicked += new EventHandler(on_checkbutton_copia_productos_clicked);//13
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);//14
			
			// Creacion de los treeview para la pantalla	
			crea_treeview_servicio();//15
			
			// Desactivando Botones de operacion se activa cuando selecciona una atencion
			//entry_id_presupuesto.Sensitive = false;
			
			entry_cirugia.IsEditable = false;
			entry_medico.IsEditable = false;
			button_busca_medico.Sensitive = false;
			button_buscar_cirugia.Sensitive = false;
			//button_busca_producto.Sensitive = false;
			activacio_de_los_campos(false);
			
			statusbar_presupuestos.Pop(0);
			statusbar_presupuestos.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_presupuestos.HasResizeGrip = false;
	    	
			// pone color a los entry
			entry_total.ModifyBase(StateType.Normal, new Gdk.Color(54,180,221));
			entry_total_iva.ModifyBase(StateType.Normal, new Gdk.Color(254,253,152));
		}
		
		
		void on_checkbutton_nuevo_presupuesto_clicked(object sender, EventArgs args)
		{
			if(checkbutton_nuevo_presupuesto.Active == true){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Question,ButtonsType.YesNo,"¿En realidad quiere crear un nuevo presupuesto?");
				ResponseType miResultado = (ResponseType)
				msgBoxError.Run ();			msgBoxError.Destroy();
				if (miResultado == ResponseType.Yes){
					button_busca_producto.Sensitive = true;
					activacio_de_los_campos(true);
					limpia_valores();
					int siguiente_presupuesto = numero_presupuesto();
					//Console.WriteLine("numero_presupuesto() "+(int) numero_presupuesto() );
					entry_id_presupuesto.Text = siguiente_presupuesto.ToString();
				}else{
					checkbutton_nuevo_presupuesto.Active = false;
				}
			}
			if(checkbutton_nuevo_presupuesto.Active == false){
				//button_busca_producto.Sensitive = false; 
				checkbutton_copia_productos.Active = false; 
				limpia_valores();
				Console.WriteLine(idpresupuesto);
			}
		}
		
		void on_selec_id_clicked(object sender, EventArgs args)
		{
			if(entry_id_presupuesto.Text  == "" || entry_id_presupuesto.Text == " "){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error,ButtonsType.Close,"Debe de llenar el campo de id cirugia con uno \n"+
						"existente para que los datos se muestren \n");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}else{
				//Console.WriteLine ("on_selec_id selecciono el ID");
				idpresupuesto = entry_id_presupuesto.Text.Trim();
				//Console.WriteLine(idpresupuesto);
				llenado_de_presupuesto(entry_id_presupuesto.Text );//1
			}
		}
		
		void on_button_actualizar_clicked(object sender, EventArgs args)
		{
			if(entry_id_presupuesto.Text == "" || entry_id_presupuesto.Text  == " " ){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close, 
				"Debe de llenar el campo de id cirugia con uno \n"+"existente para que los datos se muestren \n");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}else{
				//Console.WriteLine ("button_actualizar selecciono el ID");
				idpresupuesto = entry_id_presupuesto.Text.Trim();
				//Console.WriteLine(idpresupuesto);
				llenado_de_presupuesto( entry_id_presupuesto.Text );
			}
		}
		
		// Este toma los valores para llenar el encabezado del procedimiento
		// Aqui lleno el detalle de los servicios que se va aplicar para su cobro
		void llenado_de_presupuesto(string idpresupuesto)
		{
			checkbutton_nuevo_presupuesto.Active = false;
			//Console.WriteLine ("lleno datos de la cirugia");
			if(copiaproductos == false) {
				//Console.WriteLine("convierte valores a cero");
				subtotal_al_15 = 0;
				subtotal_al_0 = 0;
				total_iva = 0;
				sub_total = 0;
			}
			//activacio_de_los_campos(true);
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	           
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
	              	
				comando.CommandText = "SELECT id_presupuesto,osiris_his_tipo_cirugias.id_tipo_cirugia,enviado,notas, "+
									"osiris_his_tipo_cirugias.id_especialidad,descripcion_cirugia,descripcion_especialidad, "+
									"osiris_his_presupuestos_enca.id_medico,medico_provisional, "+
									"to_char(osiris_his_presupuestos_enca.deposito_minimo,'99999999') AS depominimo, "+
									"to_char(osiris_his_presupuestos_enca.dias_internamiento,'99999999') AS diasinternamiento, "+
									"to_char(osiris_his_presupuestos_enca.precio_convenido,'99999999') AS precioconvenido, "+
									"osiris_his_presupuestos_enca.telefono_medico,osiris_his_presupuestos_enca.telefono,osiris_his_presupuestos_enca.fax_presupuesto, "+
									"nombre1_medico,nombre2_medico,apellido_paterno_medico,apellido_materno_medico "+
									"FROM osiris_his_presupuestos_enca,osiris_his_tipo_cirugias,osiris_his_tipo_especialidad,osiris_his_medicos "+
					            	"WHERE "+
					            	"osiris_his_presupuestos_enca.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
					            	"AND osiris_his_presupuestos_enca.id_medico = osiris_his_medicos.id_medico "+
					            	"AND osiris_his_tipo_cirugias.id_especialidad = osiris_his_tipo_especialidad.id_especialidad  "+
					            	"AND osiris_his_presupuestos_enca.id_presupuesto = '"+(string)  idpresupuesto.ToString() +"' ;";
				//Console.WriteLine(comando.CommandText.ToString());				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if(lector.Read())
				{
					idtipocirugia = (int) lector["id_tipo_cirugia"];
					entry_id_presupuesto.Text = ((int) lector["id_presupuesto"]).ToString();
					entry_cirugia.Text = (string) lector["descripcion_cirugia"];
					entry_dias_internamiento.Text = (string) lector["diasinternamiento"];
	           		idtipoesp = (int) lector["id_especialidad"];
	           		entry_deposito_minimo.Text = (string) lector["depominimo"];
	           		if((int) lector["id_medico"] > 1){
		           		entry_medico.Text = (string) lector["nombre1_medico"]+" "+
		           							(string) lector["nombre2_medico"]+" "+
		           							(string) lector["apellido_paterno_medico"]+" "+
		           							(string) lector["apellido_materno_medico"];
		           	}else{
		           		entry_medico.Text = (string) lector["medico_provisional"];
		           	}
	           		entry_tel_medico.Text = (string) lector["telefono_medico"];
	           		entry_fax.Text =  (string) lector["fax_presupuesto"];
	           		entry_tel_opcional.Text = (string) lector["telefono"];
	           		entry_precio_convenido.Text = (string) lector["precioconvenido"];
	           		entry_notas.Text = (string) lector["notas"];
	           		enviado = (bool) lector["enviado"]; 
	           		//Console.WriteLine("enviado "+enviado);
	           		llenado_de_material_aplicado(idpresupuesto.ToString());
	           		if(enviado ==true){ button_busca_producto.Sensitive = false;}
	           	}else{
	           		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "NO existe Tal PRESUPUESTO");
					msgBoxError.Run (); 	msgBoxError.Destroy();
	           		entry_id_presupuesto.Text = "";
	           		entry_cirugia.Text = "";
	           		entry_dias_internamiento.Text = "";
	           		entry_deposito_minimo.Text = "";
	           		entry_medico.Text = "";
	           	}
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run (); 	msgBoxError.Destroy();
		    }
	       	conexion.Close ();
       	}
		
		// llenando el detalle de procedimiento de cobranza
		void llenado_de_material_aplicado(string idpresupuesto)
		{	
			//Console.WriteLine("entro a llenar lista de productos");
			if(copiaproductos == false) {
				//Console.WriteLine("convierte valores a cero");
				subtotal_al_15 = 0;
				subtotal_al_0 = 0;
				total_iva = 0;
				sub_total = 0;
				treeViewEngineServicio.Clear();
			}
			///////RESTABLESCO VALORES A LOS PREDETERMINADOS\\\\\\\\\\\\\\\\\\
			if(enviado == true) {activacio_de_los_campos(false); button_enviado.Sensitive = false;}//Console.WriteLine("entro a la desactivacion");
			if(enviado == false) { activacio_de_los_campos(true); }//Console.WriteLine("entro a la activacion");
			entry_cirugia.IsEditable = false;
			checkbutton_copia_productos.Sensitive = true;
			button_actualizar.Sensitive = true;
			button_limpiar.Sensitive = true;
			button_procedimiento_cobrz.Sensitive = true;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				comando.CommandText = "SELECT descripcion_producto,osiris_his_tipo_admisiones.descripcion_admisiones, "+
							"id_empleado,osiris_his_presupuestos_deta.eliminado,osiris_productos.aplicar_iva,osiris_his_presupuestos_deta.id_tipo_admisiones,  "+
							"to_char(osiris_his_presupuestos_deta.id_producto,'999999999999') AS idproducto, "+
							"to_char(osiris_his_presupuestos_deta.cantidad_aplicada,'99999.99') AS cantidadaplicada, "+
							"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porcentageutilidad, "+
							"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto, "+
							"to_char(osiris_his_presupuestos_deta.fechahora_creacion,'dd-MM-yyyy HH:mi:ss') AS fechcreacion ,"+
							"to_char(osiris_his_presupuestos_deta.id_secuencia,'9999999999') AS secuencia "+
							"FROM "+
							"osiris_his_presupuestos_deta,osiris_productos,osiris_his_tipo_admisiones,osiris_his_presupuestos_enca "+//osiris_his_tipo_cirugias,
							"WHERE "+
							"osiris_his_presupuestos_deta.id_producto = osiris_productos.id_producto "+
							//"AND osiris_his_presupuestos_enca.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
							"AND osiris_his_presupuestos_enca.id_presupuesto = osiris_his_presupuestos_deta.id_presupuesto "+
							"AND osiris_his_presupuestos_deta.eliminado = false "+ 
							"AND osiris_his_presupuestos_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
							"AND osiris_his_presupuestos_deta.id_presupuesto = '"+(string) idpresupuesto +"' "+
							"ORDER BY to_char(osiris_his_presupuestos_deta.fechahora_creacion,'yyyy-mm-dd HH:mm:ss'),osiris_productos.descripcion_producto;";
				
				Console.WriteLine(comando.CommandText.ToString());
                NpgsqlDataReader lector = comando.ExecuteReader ();
				
				float toma_cantaplicada = 0;
				ppcantidad = 0;
				float toma_subtotal = 0;
				
				float calculo_del_iva_producto = 0;
				string toma_descrip_prod;
				//tienepaquete = false; //Console.WriteLine("antes del ciclo tienepaquete = "+tienepaquete.ToString());
				
				while (lector.Read()) {
					if (!(bool) lector["eliminado"]) {
						///tienepaquete = true; //Console.WriteLine("ciclo: tienepaquete = "+tienepaquete.ToString());
						toma_cantaplicada = float.Parse((string) lector["cantidadaplicada"]);
						ppcantidad = toma_cantaplicada*(float.Parse((string) lector["preciopublico"]));
						
						if ((bool) lector["aplicar_iva"]) {	calculo_del_iva_producto = (ppcantidad*valoriva)/100;
						}else{	calculo_del_iva_producto = 0;	}
						
						if ((bool) lector["aplicar_iva"]) {	subtotal_al_15 = subtotal_al_15 + ppcantidad;
	 					}else{	subtotal_al_0 = subtotal_al_0 + ppcantidad;	}
	 						
	 					toma_subtotal = ppcantidad + calculo_del_iva_producto;
	 					total_iva = total_iva + calculo_del_iva_producto;
						
						toma_descrip_prod = (string) lector["descripcion_producto"];
						if(toma_descrip_prod.Length > 68) {	toma_descrip_prod = toma_descrip_prod.Substring(0,67);	} 
						
						treeViewEngineServicio.AppendValues (toma_descrip_prod,//0
															toma_cantaplicada,//1
															(string) lector["idproducto"],//2
															(string) lector["preciopublico"],//3
															ppcantidad.ToString("F").PadLeft(10),//4
															calculo_del_iva_producto.ToString("F") ,//5
															toma_subtotal.ToString("F"),//6
															(string) lector["id_empleado"],//7
															(string) lector["fechcreacion"],//8
															(string) lector["descripcion_admisiones"],//9
															(bool) true,//10
															(string) lector["secuencia"],//11
															(int) lector["id_tipo_admisiones"]);//12
					}
				}
				
				//button_busca_producto.Sensitive = true;
				// Realizando las restas
				sub_total = subtotal_al_15 + subtotal_al_0+total_iva;
				
				entry_subtotal_al_15.Text = subtotal_al_15.ToString("F");
 				entry_subtotal_al_0.Text = subtotal_al_0.ToString("F");
 				entry_total_iva.Text = total_iva.ToString("F");
 				entry_subtotal.Text = sub_total.ToString("F");
 				entry_total.Text = sub_total.ToString("F");
 				//entry_precio_convenido.Text = sub_total.ToString("F");
 								
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run (); 	msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void onComboBoxChanged_tipo_admision (object sender, EventArgs args)
		{
	    	ComboBox combobox_tipo_admision = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_tipo_admision.GetActiveIter (out iter)){
		    	idtipointernamiento = (int) combobox_tipo_admision.Model.GetValue(iter,1);
		    	descripinternamiento = (string) combobox_tipo_admision.Model.GetValue(iter,0);
			}
		}
		
		void on_button_procedimiento_cobrz_clicked(object sender, EventArgs args)
		{
			if ((string) entry_id_presupuesto.Text == "" ){	
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close, "Debe de llenar el campo de cirugia con uno \n"+
							"existente para que el  se muestre \n"+"o no a pulsado el boton ''Seleccionar''");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}else{
				new paquetes_reporte (int.Parse(entry_id_presupuesto.Text),entry_cirugia.Text,entry_medico.Text,
					nombrebd,"presupuestos",entry_deposito_minimo.Text.Trim(),entry_dias_internamiento.Text.Trim(),
					entry_tel_medico.Text.Trim(),entry_tel_opcional.Text.Trim(),entry_fax.Text.Trim(),
					entry_id_presupuesto.Text.Trim(),entry_notas.Text.Trim().ToUpper(),true,"0");   // rpt_proc_cobranza.cs
			}
		}
		
		void on_button_hoja_sin_precios_clicked(object sender, EventArgs args)
		{
			if ((string) entry_id_presupuesto.Text == "" ){	
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close, "Debe de llenar el campo de cirugia con uno \n"+
							"existente para que el  se muestre \n"+"o no a pulsado el boton ''Seleccionar''");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}else{
				new paquetes_reporte (int.Parse(entry_id_presupuesto.Text),entry_cirugia.Text,entry_medico.Text,
					nombrebd,"presupuestos",entry_deposito_minimo.Text.Trim(),entry_dias_internamiento.Text.Trim(),
					entry_tel_medico.Text.Trim(),entry_tel_opcional.Text.Trim(),entry_fax.Text.Trim(),
					entry_id_presupuesto.Text.Trim(),entry_notas.Text.Trim().ToUpper(),false,"0");   // rpt_proc_cobranza.cs
			}
		}
		
		void on_button_graba_presupuesto_clicked(object sender, EventArgs args)
		{
			if(entry_cirugia.Text.Trim() == "" || entry_dias_internamiento.Text.Trim() == "" || entry_medico.Text.Trim() == "" ||
			   entry_deposito_minimo.Text.Trim() == "" || entry_precio_convenido.Text.Trim() == "") 
			{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
											ButtonsType.Close,"verifique que los campos de Medico,Cirugia, \n"+
											"Precio Convenido, Deposito Minimo y/o Dias de internamiento \n"+
											"no se encuentren en blanco");
				msgBoxError.Run ();					msgBoxError.Destroy();
			}else{
				if(checkbutton_nuevo_presupuesto.Active == true){
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,
											"¿ Desea GRABAR esta infomacion ?");
					ResponseType miResultado = (ResponseType)
					msgBox.Run ();					msgBox.Destroy();
	 				if (miResultado == ResponseType.Yes){
						//Console.WriteLine("GUARDO DATOS Y GRABO PRODUCTOS");
						int siguiente_presupuesto = numero_presupuesto();
						//Console.WriteLine("numero_presupuesto() "+(int) numero_presupuesto() );
						guarda_encabezado_presupuesto(siguiente_presupuesto);
						guarda_productos();
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
				 									ButtonsType.Close, "El presupuesto se creo con EXITO");
						msgBoxError.Run ();			msgBoxError.Destroy();
						
						//entry_id_presupuesto.Text = siguiente_presupuesto.ToString();
						
						llenado_de_presupuesto(siguiente_presupuesto.ToString());
						
						checkbutton_nuevo_presupuesto.Active = false;
					}
				}else{
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,
											"¿ Desea ACTUALIZAR esta infomacion ?");
					ResponseType miResultado = (ResponseType)
					msgBox.Run ();					msgBox.Destroy();
	 				if (miResultado == ResponseType.Yes){
						
						guarda_encabezado_presupuesto(int.Parse(entry_id_presupuesto.Text.Trim()));
						guarda_productos();
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
				 									ButtonsType.Close, "El presupuesto se ACTUALIZO con EXITO");
						msgBoxError.Run ();			msgBoxError.Destroy();
						checkbutton_copia_productos.Active = false;
						llenado_de_presupuesto(this.entry_id_presupuesto.Text.Trim());
					}
				}
	 		}
		}
		
		public int numero_presupuesto()
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			int nuevo_presupuesto = 1;
			conexion.Open ();
			try{
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT id_presupuesto FROM osiris_his_presupuestos_enca ORDER BY id_presupuesto DESC LIMIT 1 ;";
				comando.ExecuteNonQuery();		comando.Dispose();
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if (lector.Read()){
					nuevo_presupuesto = (int) lector["id_presupuesto"] + 1;
					return nuevo_presupuesto;
				}else{
					return nuevo_presupuesto;
				}
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
				return nuevo_presupuesto;
			}
			conexion.Close();
		}
		
		void guarda_encabezado_presupuesto(int numero_presupuesto)
		{
			string medicoprov = "";
			if(id_medico == 1){
				medicoprov = entry_medico.Text.Trim();
			}
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			
			if(this.checkbutton_nuevo_presupuesto.Active == true){
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					comando.CommandText = "INSERT INTO osiris_his_presupuestos_enca ( "+
										"id_tipo_cirugia,"+
										"id_medico, "+
										"total_presupuesto,"+
										"precio_convenido,"+
										"fecha_de_creacion_presupuesto,"+
										"dias_internamiento,"+
										"telefono_medico,"+
										"telefono,"+
										"fax_presupuesto,"+
										"id_quien_creo,"+
										"medico_provisional,"+
										"notas, "+
										"deposito_minimo) "+
										" VALUES ('"+
										(int) idtipocirugia+"','"+
										(int) id_medico+"','"+
										decimal.Parse((string) entry_total.Text.Trim())+"','"+
										decimal.Parse((string) entry_precio_convenido.Text.Trim())+"','"+
										(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
										(string) entry_dias_internamiento.Text.Trim().ToUpper()+"','"+
										(string) entry_tel_medico.Text.Trim().ToUpper()+"','"+
										(string) entry_tel_opcional.Text.Trim().ToUpper()+"','"+
										(string) entry_fax.Text.Trim().ToUpper()+"','"+
										(string) LoginEmpleado +"','"+
										(string) medicoprov.ToUpper() +"','"+
										(string) entry_notas.Text.Trim().ToUpper()+"','"+
										decimal.Parse((string) entry_deposito_minimo.Text.Trim())+"');";
					comando.ExecuteNonQuery();			comando.Dispose();
				}catch (NpgsqlException ex){
			   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
					msgBoxError.Run ();					msgBoxError.Destroy();
				}
			}else{				
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					comando.CommandText = "UPDATE osiris_his_presupuestos_enca SET "+
										"id_tipo_cirugia = '"+(int) idtipocirugia+"',"+
										"id_medico = '"+(int)  id_medico+"',"+
										"total_presupuesto = '"+decimal.Parse((string) entry_total.Text.Trim())+"',"+
										"precio_convenido = '"+decimal.Parse((string) entry_precio_convenido.Text.Trim())+"',"+
										"dias_internamiento = '"+(string) entry_dias_internamiento.Text.Trim().ToUpper()+"',"+
										"telefono_medico = '"+(string) entry_tel_medico.Text.Trim().ToUpper()+"',"+
										"telefono = '"+(string) entry_tel_opcional.Text.Trim().ToUpper()+"',"+
										"fax_presupuesto = '"+(string) entry_fax.Text.Trim().ToUpper()+"',"+
										"medico_provisional = '"+(string) medicoprov.ToString().ToUpper()+"',"+
										"notas = '"+(string) entry_notas.Text.Trim().ToUpper()+"',"+
										"deposito_minimo = '"+decimal.Parse((string) entry_deposito_minimo.Text.Trim())+"'  "+
										"WHERE id_presupuesto = '"+int.Parse(entry_id_presupuesto.Text.Trim())+"' ;";
					comando.ExecuteNonQuery();			comando.Dispose();
					Console.WriteLine(comando.CommandText);
				}catch (NpgsqlException ex){
			   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
					msgBoxError.Run ();					msgBoxError.Destroy();
				}						
			}
			conexion.Close ();
		}
		
		void guarda_productos()
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				TreeIter iter;
				//Console.WriteLine("guarda PRODUCTOS al PRESUPUESTO: "+entry_id_presupuesto.Text.Trim());
				if ((int) idtipocirugia > 1){  // Validando que seleccione un folio de atencion
					if (treeViewEngineServicio.GetIterFirst (out iter)){
						if ((bool)lista_de_servicios.Model.GetValue (iter,10) == false){
							//Console.WriteLine("leeo primer linea"+(string) lista_de_servicios.Model.GetValue(iter,2));
							comando.CommandText = "INSERT INTO osiris_his_presupuestos_deta("+
													"id_producto,"+
													"id_presupuesto,"+
													"cantidad_aplicada,"+
													"id_empleado,"+
													"fechahora_creacion,"+
													"id_tipo_admisiones) "+
													"VALUES ('"+
													long.Parse((string) lista_de_servicios.Model.GetValue(iter,2))+"','"+
													int.Parse(entry_id_presupuesto.Text.Trim())+"','"+ //idtipocirugia+"','"+
													(float) lista_de_servicios.Model.GetValue(iter,1)+"','"+
													LoginEmpleado+"','"+
													DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
													(int) lista_de_servicios.Model.GetValue(iter,12)+"');";
													//Console.WriteLine("guarda aplicado: "+comando.CommandText);
							comando.ExecuteNonQuery();			comando.Dispose();
							tienepaquete = true;
						}
						while (treeViewEngineServicio.IterNext(ref iter)){
				   			if ((bool)lista_de_servicios.Model.GetValue (iter,10) == false){
								//Console.WriteLine("entro al ciclo"+(string) lista_de_servicios.Model.GetValue(iter,2));
								comando.CommandText = "INSERT INTO osiris_his_presupuestos_deta("+
														"id_producto,"+
														"id_presupuesto,"+
														"cantidad_aplicada,"+
														"id_empleado,"+
														"fechahora_creacion,"+
														"id_tipo_admisiones) "+
														"VALUES ('"+
														long.Parse((string) lista_de_servicios.Model.GetValue(iter,2))+"','"+
														int.Parse(entry_id_presupuesto.Text.Trim()) +"','"+//idtipocirugia+"','"+
														(float) lista_de_servicios.Model.GetValue(iter,1)+"','"+
														LoginEmpleado+"','"+
														DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
														(int) lista_de_servicios.Model.GetValue(iter,12)+"');";
								//Console.WriteLine("guarda aplicado: "+comando.CommandText);
								comando.ExecuteNonQuery();	   	       			comando.Dispose();
							}
						}
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info,ButtonsType.Close, "Seleccione alguna cirugia...");
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_button_buscar_cirugia_clicked(object sender, EventArgs args)
		{
			busqueda ="cirugia";
			abre_ventana_busqueda_cirugia();
		}
		
		void abre_ventana_busqueda_cirugia()
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "busca_cirugias", null);
			gxml.Autoconnect (this);
			
			// Activa la salida de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			// Activa la seleccion de cirugia
			button_selecciona.Clicked += new EventHandler(on_selecciona_cirugia_clicked);
			// Activa boton de busqueda
			button_llena_cirugias.Clicked += new EventHandler(on_button_llena_cirugias_clicked);
	        
			treeViewEngineBusca = new TreeStore( typeof(int),typeof(string),typeof(string),typeof(string));
			lista_cirugia.Model = treeViewEngineBusca;
			
			lista_cirugia.RulesHint = true;
			
			lista_cirugia.RowActivated += on_selecciona_cirugia_clicked;  // Doble click selecciono paciente*/
			
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
			col_descripcirugia.Resizable = true;
			
			TreeViewColumn col_tienepaquete = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_tienepaquete.Title = "Paquete";
			col_tienepaquete.PackStart(cellrt2, true);
			col_tienepaquete.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
			
			TreeViewColumn col_preciobase = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_preciobase.Title = "P. Base";
			col_preciobase.PackStart(cellrt3, true);
			col_preciobase.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 1 en vez de 2
			
			lista_cirugia.AppendColumn(col_idcirugia);
			lista_cirugia.AppendColumn(col_descripcirugia);
			lista_cirugia.AppendColumn(col_tienepaquete);
			lista_cirugia.AppendColumn(col_preciobase);
		}
			
		void on_selecciona_cirugia_clicked (object sender, EventArgs args)
		{
			TreeModel model;			TreeIter iterSelected;
			if (lista_cirugia.Selection.GetSelected(out model, out iterSelected)) {
				idtipocirugia = (int) model.GetValue(iterSelected, 0);
				cirugia = (string) model.GetValue(iterSelected, 1);
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,
										"¿ Desea COPIAR los productos de esta cirugia ?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();					msgBox.Destroy();
	 			if (miResultado == ResponseType.Yes){
					if(busqueda == "cirugia")  {entry_cirugia.Text = cirugia;}
					copiando_productos_de_cirugia ();
				}else{
					if(busqueda == "cirugia")  {entry_cirugia.Text = cirugia;}
				}
				// cierra la ventana despues que almaceno la informacion en variables
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
		
		// llenando el detalle de procedimiento de cobranza
		void copiando_productos_de_cirugia ()
		{	
			///////RESTABLESCO VALORES A LOS PREDETERMINADOS\\\\\\\\\\\\\\\\\\
			if(copiaproductos == false) {
				subtotal_al_15 = 0;
				subtotal_al_0 = 0;
				total_iva = 0;
				sub_total = 0;
				treeViewEngineServicio.Clear();
			}
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				comando.CommandText = "SELECT descripcion_producto,osiris_his_tipo_admisiones.descripcion_admisiones, "+
							"id_empleado,osiris_his_cirugias_deta.eliminado,osiris_productos.aplicar_iva,osiris_his_cirugias_deta.id_tipo_admisiones,  "+
							"to_char(osiris_his_cirugias_deta.id_producto,'999999999999') AS idproducto, "+
							"to_char(osiris_his_cirugias_deta.cantidad_aplicada,'99999.99') AS cantidadaplicada, "+
							"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porcentageutilidad, "+
							"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto, "+
							"to_char(osiris_his_cirugias_deta.fechahora_creacion,'dd-MM-yyyy HH:mi:ss') AS fechcreacion ,"+
							"to_char(osiris_his_cirugias_deta.id_secuencia,'9999999999') AS secuencia "+
							"FROM "+
							"osiris_his_cirugias_deta,osiris_productos,osiris_his_tipo_cirugias,osiris_his_tipo_admisiones "+
							"WHERE "+
							"osiris_his_cirugias_deta.id_producto = osiris_productos.id_producto "+
							"AND osiris_his_cirugias_deta.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
							"AND osiris_his_cirugias_deta.eliminado = false "+ 
							"AND osiris_his_cirugias_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
							"AND osiris_his_cirugias_deta.id_tipo_cirugia = '"+(string) idtipocirugia.ToString() +"' "+
							"ORDER BY osiris_productos.descripcion_producto,to_char(osiris_his_cirugias_deta.fechahora_creacion,'yyyy-mm-dd HH:mm:ss');";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//Console.WriteLine("query llenado de materiales: "+comando.CommandText.ToString());
				float toma_cantaplicada = 0;
				ppcantidad = 0;
				float toma_subtotal = 0;
				string toma_descrip_prod;
				float calculo_del_iva_producto = 0;
				////tienepaquete = false; //Console.WriteLine("antes del ciclo tienepaquete = "+tienepaquete.ToString());
				
				while (lector.Read()) {
					if (!(bool) lector["eliminado"]) {
						///tienepaquete = true; //Console.WriteLine("ciclo: tienepaquete = "+tienepaquete.ToString());
						toma_cantaplicada = float.Parse((string) lector["cantidadaplicada"]);
						ppcantidad = toma_cantaplicada*(float.Parse((string) lector["preciopublico"]));
						if ((bool) lector["aplicar_iva"]) {
							calculo_del_iva_producto = (ppcantidad*valoriva)/100;
						}else{
							calculo_del_iva_producto = 0;
						}
						
						if ((bool) lector["aplicar_iva"]) {
							subtotal_al_15 = subtotal_al_15 + ppcantidad;
	 					}else{
	 						subtotal_al_0 = subtotal_al_0 + ppcantidad;
	 					}
	 						
	 					toma_subtotal = ppcantidad + calculo_del_iva_producto;
	 					total_iva = total_iva + calculo_del_iva_producto;
						
						toma_descrip_prod = (string) lector["descripcion_producto"];
						if(toma_descrip_prod.Length > 68) {	toma_descrip_prod = toma_descrip_prod.Substring(0,67);	}  
						
						treeViewEngineServicio.AppendValues (toma_descrip_prod,//0
															toma_cantaplicada,//1
															(string) lector["idproducto"],//2
															(string) lector["preciopublico"],//3
															ppcantidad.ToString("F").PadLeft(10),//4
															calculo_del_iva_producto.ToString("F") ,//5
															toma_subtotal.ToString("F"),//6
															(string) lector["id_empleado"],//7
															(string) lector["fechcreacion"],//8
															(string) lector["descripcion_admisiones"],//9
															(bool) false,//10
															(string) lector["secuencia"],//11
															(int) lector["id_tipo_admisiones"]);//12
					}
				}
				
				button_busca_producto.Sensitive = true;
				// Realizando las restas
				sub_total = subtotal_al_15 + subtotal_al_0+total_iva;
				//toma_a_pagar = sub_total ;//- totaldescuento
				entry_subtotal_al_15.Text = subtotal_al_15.ToString("F");
 				entry_subtotal_al_0.Text = subtotal_al_0.ToString("F");
 				entry_total_iva.Text = total_iva.ToString("F");
 				entry_subtotal.Text = sub_total.ToString("F");
 				entry_total.Text = sub_total.ToString("F");
 				//entry_precio_convenido.Text = sub_total.ToString("F");	
 			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		
		
		void on_button_llena_cirugias_clicked(object sender, EventArgs args)
		{
			llena_lista_de_busqueda();
		}
		
		void llena_lista_de_busqueda() 
		{
			 if(busqueda == "cirugia" || busqueda == "copia_paquete") {
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
												"ORDER BY id_tipo_cirugia;";
						}else{
							comando.CommandText ="SELECT id_tipo_cirugia,descripcion_cirugia,tiene_paquete,to_char(valor_paquete,'999999999.99') AS valorpaquete FROM osiris_his_tipo_cirugias "+
												"WHERE descripcion_cirugia LIKE '%"+entry_expresion.Text.ToUpper()+"%' "+
												" ORDER BY id_tipo_cirugia;";
						}
						NpgsqlDataReader lector = comando.ExecuteReader ();
						string paquete_sino = "";
						while (lector.Read())	{
							if((bool) lector["tiene_paquete"]){
								paquete_sino = "ES PAQUETE";
							}else{
								paquete_sino = "";
							}
							
							treeViewEngineBusca.AppendValues ((int) lector["id_tipo_cirugia"],
										(string) lector["descripcion_cirugia"],
										paquete_sino,
										(string) lector["valorpaquete"]
										);//TreeIter iter =
						}
					}catch (NpgsqlException ex){
		   				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
						msgBoxError.Run (); 	msgBoxError.Destroy();
					}
				conexion.Close ();
			}
			if(busqueda =="especialidad") {
				treeViewEngineBusca.Clear();// Limpia el treeview cuando realiza una nueva busqueda
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
           		// Verifica que la base de datos este conectada
				try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
		              	if ((string) entry_expresion.Text.ToUpper() == "*")	{
							comando.CommandText ="SELECT * FROM osiris_his_tipo_especialidad "+
												" ORDER BY id_especialidad;";
						}else{
							comando.CommandText ="SELECT * FROM osiris_his_tipo_especialidad "+
												"WHERE descripcion_especialidad LIKE '%"+entry_expresion.Text.ToUpper()+"%' "+
												" ORDER BY id_especialidad;";
						}
						NpgsqlDataReader lector = comando.ExecuteReader ();
						while (lector.Read())	{
							treeViewEngineBusca.AppendValues ((int) lector["id_especialidad"],(string) lector["descripcion_especialidad"]);//TreeIter iter =
						}
					}catch (NpgsqlException ex){
		   				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
						msgBoxError.Run (); 	msgBoxError.Destroy();
					}
				conexion.Close ();
			}
		}
		
		void on_button_busca_medico_clicked(object sender, EventArgs args)
		{
			busqueda = "medicos";
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador_medicos", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          (this);
	        llenado_cmbox_tipo_busqueda();
	        //entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			button_buscar_busqueda.Clicked += new EventHandler(on_button_llena_medicos_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_medico_clicked);
	        button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
			
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
		
		void on_selecciona_medico_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			//float toma_valor = 0;
			if (lista_de_medicos.Selection.GetSelected(out model, out iterSelected)) 
 			{
				id_medico =(int) model.GetValue(iterSelected, 0);
				entry_tel_medico.Text = "";
 				entry_medico.Text = (string) model.GetValue(iterSelected, 1)+" "+(string) model.GetValue(iterSelected, 2)+" "+
 							(string) model.GetValue(iterSelected, 3)+" "+(string) model.GetValue(iterSelected, 4);
 				if((string) model.GetValue(iterSelected, 6) != "") {entry_tel_medico.Text = (string) model.GetValue(iterSelected, 6);}
				else{
					if((string) model.GetValue(iterSelected,7) != "") {entry_tel_medico.Text = (string) model.GetValue(iterSelected,7);}
					else{
						if((string) model.GetValue(iterSelected,8) != "") {entry_tel_medico.Text = (string) model.GetValue(iterSelected,8);}
						else{
							if((string) model.GetValue(iterSelected,9) != "") {entry_tel_medico.Text = (string) model.GetValue(iterSelected,9);}
							else{
								if((string) model.GetValue(iterSelected,10) != "") {entry_tel_medico.Text = (string) model.GetValue(iterSelected,10);}
								else{
									if((string) model.GetValue(iterSelected,11) != "") {entry_tel_medico.Text = (string) model.GetValue(iterSelected,11);}
								}
							}
						}
					}
				}
				// cierra la ventana despues que almaceno la informacion en variables
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
				if(id_medico == 1) { entry_medico.IsEditable = true;	entry_medico.GrabFocus();
				}else { entry_medico.IsEditable = false; }
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
		
		void on_button_copia_procedimiento_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "carga_folio", null);
			gxml.Autoconnect (this);
			carga_folio.Show();
			button_carga_productos.Clicked += new EventHandler(on_button_carga_productos_procedimiento_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
		}
		
		void on_button_carga_productos_procedimiento_clicked(object sender, EventArgs args)
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			          
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				comando.CommandText = "SELECT descripcion_producto,osiris_his_tipo_admisiones.descripcion_admisiones, "+
							"eliminado,osiris_productos.aplicar_iva,osiris_erp_cobros_deta.id_tipo_admisiones,  "+
							"to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto, "+
							"to_char(osiris_erp_cobros_deta.cantidad_aplicada,'99999.99') AS cantidadaplicada, "+
							"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porcentageutilidad, "+
							"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto "+
							"FROM "+
							"osiris_erp_cobros_deta,osiris_productos,osiris_his_tipo_admisiones "+
							"WHERE "+
							"osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+
							"AND osiris_erp_cobros_deta.eliminado = false "+ 
							"AND osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
							"AND osiris_erp_cobros_deta.folio_de_servicio = '"+(string) this.entry_folio.Text +"' "+
							"ORDER BY osiris_productos.descripcion_producto,to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-mm-dd HH:mm:ss');";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//Console.WriteLine("query llenado de materiales: "+comando.CommandText.ToString());
				float toma_cantaplicada = 0;
				ppcantidad = 0;
				float toma_subtotal = 0;
				
				float calculo_del_iva_producto = 0;
				string toma_descrip_prod;
				tienepaquete = false; //Console.WriteLine("antes del ciclo tienepaquete = "+tienepaquete.ToString());
				
				while (lector.Read()) {
					if (!(bool) lector["eliminado"]) {
						//tienepaquete = true; //Console.WriteLine("ciclo: tienepaquete = "+tienepaquete.ToString());
						toma_cantaplicada = float.Parse((string) lector["cantidadaplicada"]);
						ppcantidad = toma_cantaplicada*(float.Parse((string) lector["preciopublico"]));
						if ((bool) lector["aplicar_iva"]) {
							calculo_del_iva_producto = (ppcantidad*valoriva)/100;
						}else{
							calculo_del_iva_producto = 0;
						}
						
						if ((bool) lector["aplicar_iva"]) {
							subtotal_al_15 = subtotal_al_15 + ppcantidad;
	 					}else{
	 						subtotal_al_0 = subtotal_al_0 + ppcantidad;
	 					}
	 						
	 					toma_subtotal = ppcantidad + calculo_del_iva_producto;
	 					total_iva = total_iva + calculo_del_iva_producto;
						
						toma_descrip_prod = (string) lector["descripcion_producto"];
						if(toma_descrip_prod.Length > 68) {	toma_descrip_prod = toma_descrip_prod.Substring(0,67); }  
						
						treeViewEngineServicio.AppendValues (toma_descrip_prod,//0
															toma_cantaplicada,//1
															(string) lector["idproducto"],//2
															(string) lector["preciopublico"],//3
															ppcantidad.ToString("F").PadLeft(10),//4
															calculo_del_iva_producto.ToString("F") ,//5
															toma_subtotal.ToString("F"),//6
															this.LoginEmpleado,//(string) lector["id_empleado"],//7
															DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss"),//(string) lector["fechcreacion"],//8
															(string) lector["descripcion_admisiones"],//9
															(bool) false,//10
															"",//secuencia11
															(int) lector["id_tipo_admisiones"]);//12
					}
				}
				
				//button_busca_producto.Sensitive = true;
				// Realizando las restas
				sub_total = subtotal_al_15 + subtotal_al_0+total_iva;
				
				entry_subtotal_al_15.Text = subtotal_al_15.ToString("F");
 				entry_subtotal_al_0.Text = subtotal_al_0.ToString("F");
 				entry_total_iva.Text = total_iva.ToString("F");
 				entry_subtotal.Text = sub_total.ToString("F");
 				entry_total.Text = sub_total.ToString("F");
 				//entry_precio_convenido.Text = sub_total.ToString("F");
 				Widget win = (Widget) sender;
				win.Toplevel.Destroy();				
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run (); 	msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_button_copia_paquete_clicked(object sender, EventArgs args)
		{
			busqueda = "copia_paquete";
			abre_ventana_busqueda_cirugia();
		}
		
		
		void on_button_quitar_aplicados_clicked(object sender, EventArgs args)
		{
			TreeIter iter;
 			TreeModel model;
 			string toma_valor1;
 			string toma_valor2;
 			string prodeliminado;
 			
 			if (lista_de_servicios.Selection.GetSelected (out model, out iter)) {
 				toma_valor1 = (string) lista_de_servicios.Model.GetValue(iter,4);	//
				toma_valor2 = (string) lista_de_servicios.Model.GetValue (iter,5);  // toma el valor del iva
				
				if (!(bool) lista_de_servicios.Model.GetValue (iter,10)){
								
 					treeViewEngineServicio.Remove (ref iter);
 				 					
 					if ((float) float.Parse(toma_valor2) > 0){
 						subtotal_al_15 = subtotal_al_15 - float.Parse(toma_valor1);
 					}else{
 						subtotal_al_0 = subtotal_al_0 - float.Parse(toma_valor1);
 					}
 					total_iva = total_iva - float.Parse(toma_valor2);
				
					sub_total = subtotal_al_15 + subtotal_al_0 + total_iva;
					
					entry_subtotal_al_15.Text = subtotal_al_15.ToString("F");
 					entry_subtotal_al_0.Text = subtotal_al_0.ToString("F");
 					entry_total_iva.Text = total_iva.ToString("F");
 					entry_subtotal.Text = sub_total .ToString("F");
 					entry_total.Text = sub_total.ToString("F");
 					//entry_precio_convenido.Text = sub_total.ToString("F");
 				}else{
 					if (LoginEmpleado =="DOLIVARES" || LoginEmpleado =="ADMIN"){
 						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Question,ButtonsType.YesNo,"¿ Desea eliminar del paquete este producto ?");
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
				 				comando.CommandText = "UPDATE osiris_his_presupuestos_deta "+
										"SET eliminado = 'true' , "+
										"fechahora_eliminado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
										"id_quien_elimino = '"+LoginEmpleado+"' "+								
				 						"WHERE id_secuencia =  '"+ int.Parse((string) lista_de_servicios.Model.GetValue (iter,11))+"';";
										comando.ExecuteNonQuery();
					        			comando.Dispose();
			        			
			        			prodeliminado = (string) lista_de_servicios.Model.GetValue (iter,0);
			        			msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"El Producto "+prodeliminado.ToString()+" se devolvio satisfactoriamente");
								msgBox.Run ();					msgBox.Destroy();
								
								this.llenado_de_material_aplicado( (string) entry_id_presupuesto.Text );
								
								conexion.Close ();
			        		}catch (NpgsqlException ex){
				   				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
								msgBoxError.Run (); 	msgBoxError.Destroy();
							}
 						}
 					}else{
 						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Error,ButtonsType.Ok,"No esta autorizado para esta opcion...");
						msgBox.Run();
						msgBox.Destroy();
 					}
 				}
 			}
		}
		
		void on_button_busca_producto_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda();
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter_exp;
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			combobox_tipo_admision.Clear();
			llena_combobox_admision();
		}
		
		void llena_combobox_admision()
		{
			CellRendererText cell2 = new CellRendererText();
			combobox_tipo_admision.PackStart(cell2, true);
			combobox_tipo_admision.AddAttribute(cell2,"text",0);
	        
			ListStore store2 = new ListStore( typeof (string), typeof (int));
			combobox_tipo_admision.Model = store2;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_his_tipo_admisiones "+
               						"WHERE cuenta_mayor = 4000 "+
               						" ORDER BY descripcion_admisiones;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				store2.AppendValues ("", 0);
               	while (lector.Read())
				{
					store2.AppendValues ((string) lector["descripcion_admisiones"], (int) lector["id_tipo_admisiones"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
			/*
			// lleno de la tabla de his_tipo_de_admisiones
			store2.AppendValues ("",10);
			store2.AppendValues ("Urgencias",100);
			store2.AppendValues ("Hospital",500);
			store2.AppendValues ("Ginecologia-Tococirugia",600);
			store2.AppendValues ("Quirofano",700);
			store2.AppendValues ("Terapia Adulto",810);
			store2.AppendValues ("Terapia Pedriatrica",820);
			store2.AppendValues ("Laboratorio",400);
			store2.AppendValues ("Imagenologia-RX",300);
			store2.AppendValues ("Rehabilitacion",200);
			store2.AppendValues ("Otros Servicios",920);
	        */
			TreeIter iter2;
			if (store2.GetIterFirst(out iter2))
			{
				combobox_tipo_admision.SetActiveIter (iter2);
			}
			combobox_tipo_admision.Changed += new EventHandler (onComboBoxChanged_tipo_admision);
			
		}
		
		void crea_treeview_servicio()
		{
			treeViewEngineServicio = new TreeStore(typeof(string),//0 
													typeof(float),//1
													typeof(string),//2
													typeof(string),//3
													typeof(string),//4
													typeof(string),//5
													typeof(string),//6
													typeof(string),//7
													typeof(string),//8
													typeof(string),//9
													typeof(bool),//10
													typeof(string),//11
													typeof(int));//12
												
			lista_de_servicios.Model = treeViewEngineServicio;
			lista_de_servicios.RulesHint = true;
				
			TreeViewColumn col_descripcion_hc = new TreeViewColumn();
			CellRendererText cel_descripcion = new CellRendererText();
			col_descripcion_hc.Title = "Servicio/Producto"; // titulo de la cabecera de la columna, si está visible
			col_descripcion_hc.PackStart(cel_descripcion, true);
			col_descripcion_hc.AddAttribute (cel_descripcion, "text", 0);
			col_descripcion_hc.SortColumnId = (int) Column_serv.col_descripcion_hc;
			col_descripcion_hc.SetCellDataFunc(cel_descripcion, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cel_descripcion.Foreground = "darkblue";
			
			TreeViewColumn col_cantidad_hc = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_cantidad_hc.Title = "Cantidad"; // titulo de la cabecera de la columna, si está visible
			col_cantidad_hc.PackStart(cellr1, true);
			col_cantidad_hc.AddAttribute (cellr1, "text", 1);
			col_cantidad_hc.SortColumnId = (int) Column_serv.col_cantidad_hc;
			col_cantidad_hc.SetCellDataFunc(cellr1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr1.Foreground = "darkblue";
						
			TreeViewColumn col_codigo_prod_hc = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_codigo_prod_hc.Title = "Codigo Prod."; // titulo de la cabecera de la columna, si está visible
			col_codigo_prod_hc.PackStart(cellr2, true);
			col_codigo_prod_hc.AddAttribute (cellr2, "text", 2);
			col_codigo_prod_hc.SortColumnId = (int) Column_serv.col_codigo_prod_hc;
			col_codigo_prod_hc.SetCellDataFunc(cellr2, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr2.Foreground = "darkblue";
			        
			TreeViewColumn col_precio_hc = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_precio_hc.Title = "P.Unitario"; // titulo de la cabecera de la columna, si está visible
			col_precio_hc.PackStart(cellr3, true);
			col_precio_hc.AddAttribute (cellr3, "text", 3);
			col_precio_hc.SortColumnId = (int) Column_serv.col_precio_hc;
			col_precio_hc.SetCellDataFunc(cellr3, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr3.Foreground = "darkblue";
			
			TreeViewColumn col_ppor_cantidad_hc = new TreeViewColumn();
			CellRendererText cellr4 = new CellRendererText();
			col_ppor_cantidad_hc.Title = "Sub-Total"; // titulo de la cabecera de la columna, si está visible
			col_ppor_cantidad_hc.PackStart(cellr4, true);
			col_ppor_cantidad_hc.AddAttribute (cellr4, "text", 4);
			col_ppor_cantidad_hc.SortColumnId = (int) Column_serv.col_ppor_cantidad_hc;
			col_ppor_cantidad_hc.SetCellDataFunc(cellr4, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr4.Foreground = "darkblue";
        
			TreeViewColumn col_iva_hc = new TreeViewColumn();
			CellRendererText cellr5 = new CellRendererText();
			col_iva_hc.Title = "I.V.A."; // titulo de la cabecera de la columna, si está visible
			col_iva_hc.PackStart(cellr5, true);
			col_iva_hc.AddAttribute (cellr5, "text", 5);
			col_iva_hc.SortColumnId = (int) Column_serv.col_iva_hc;
			col_iva_hc.SetCellDataFunc(cellr5, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr5.Foreground = "darkblue";
        
			TreeViewColumn col_sub_total_hc = new TreeViewColumn();
			CellRendererText cellr6 = new CellRendererText();
			col_sub_total_hc.Title = "Total"; // titulo de la cabecera de la columna, si está visible
			col_sub_total_hc.PackStart(cellr6, true);
			col_sub_total_hc.AddAttribute (cellr6, "text", 6);
			col_sub_total_hc.SortColumnId = (int) Column_serv.col_sub_total_hc;
			col_sub_total_hc.SetCellDataFunc(cellr6, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr6.Foreground = "darkblue";
			
			TreeViewColumn col_quien_cargo_hc = new TreeViewColumn();
			CellRendererText cellr7 = new CellRendererText();
			col_quien_cargo_hc.Title = "Quien cargo"; // titulo de la cabecera de la columna, si está visible
			col_quien_cargo_hc.PackStart(cellr7, true);
			col_quien_cargo_hc.AddAttribute (cellr7, "text", 7);//10
			col_quien_cargo_hc.SortColumnId = (int) Column_serv.col_quien_cargo_hc;
			col_quien_cargo_hc.SetCellDataFunc(cellr7, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr7.Foreground = "darkblue";
        
			TreeViewColumn col_fecha_hora_hc = new TreeViewColumn();
			CellRendererText cellr8 = new CellRendererText();
			col_fecha_hora_hc.Title = "Fecha/Hora"; // titulo de la cabecera de la columna, si está visible
			col_fecha_hora_hc.PackStart(cellr8, true);
			col_fecha_hora_hc.AddAttribute (cellr8, "text", 8);//11
			col_fecha_hora_hc.SortColumnId = (int) Column_serv.col_fecha_hora_hc;
			col_fecha_hora_hc.SetCellDataFunc(cellr8, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr8.Foreground = "darkblue";

			TreeViewColumn col_tipointernamiento_hc =  new TreeViewColumn();
			CellRendererText cellr9 = new CellRendererText();
			col_tipointernamiento_hc.Title = "Cargo a";
			col_tipointernamiento_hc.PackStart(cellr9,true);
			col_tipointernamiento_hc.AddAttribute(cellr9,"text",9);
			col_tipointernamiento_hc.SortColumnId = (int) Column_serv.col_tipointernamiento_hc;
			col_tipointernamiento_hc.SetCellDataFunc(cellr9, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr9.Foreground = "darkblue";
			
			lista_de_servicios.AppendColumn(col_descripcion_hc);
			lista_de_servicios.AppendColumn(col_cantidad_hc);
			lista_de_servicios.AppendColumn(col_codigo_prod_hc);
			lista_de_servicios.AppendColumn(col_precio_hc);
			lista_de_servicios.AppendColumn(col_ppor_cantidad_hc);
			lista_de_servicios.AppendColumn(col_iva_hc);
			lista_de_servicios.AppendColumn(col_sub_total_hc);
			lista_de_servicios.AppendColumn(col_quien_cargo_hc);
			lista_de_servicios.AppendColumn(col_fecha_hora_hc);
			lista_de_servicios.AppendColumn(col_tipointernamiento_hc);
		}
		
		void crea_treeview_busqueda()
		{
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
												typeof(string));
			lista_de_producto.Model = treeViewEngineBusca2;
			
			lista_de_producto.RulesHint = true;
			
			lista_de_producto.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono paciente
				
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
           
			TreeViewColumn col_grupoprod = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_grupoprod.Title = "Grupo Producto";
			col_grupoprod.PackStart(cellrt5, true);
			col_grupoprod.AddAttribute (cellrt5, "text", 5); // la siguiente columna será 7 en vez de 8
			col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
          
			TreeViewColumn col_grupo1prod = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_grupo1prod.Title = "Grupo1 Producto";
			col_grupo1prod.PackStart(cellrt6, true);
			col_grupo1prod.AddAttribute (cellrt6, "text", 6); // la siguiente columna será 8 en vez de 9
			col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
                        
			TreeViewColumn col_grupo2prod = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_grupo2prod.Title = "Grupo2 Producto";
			col_grupo2prod.PackStart(cellrt7, true);
			col_grupo2prod.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 8 en vez de 9
			col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;
		
			lista_de_producto.AppendColumn(col_idproducto);  // 0
			lista_de_producto.AppendColumn(col_desc_producto); // 1
			lista_de_producto.AppendColumn(col_precioprod);	//2
			lista_de_producto.AppendColumn(col_ivaprod);	// 3
			lista_de_producto.AppendColumn(col_totalprod); // 4
			lista_de_producto.AppendColumn(col_grupoprod);	//5
			lista_de_producto.AppendColumn(col_grupo1prod);	//6
			lista_de_producto.AppendColumn(col_grupo2prod);	//7						
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
		enum Column_serv
		{
			col_descripcion_hc,
			col_cantidad_hc,
			col_codigo_prod_hc,
			col_precio_hc,
			col_ppor_cantidad_hc,
			col_iva_hc,
			col_sub_total_hc,
			col_quien_cargo_hc,
			col_fecha_hora_hc,
			col_tipointernamiento_hc
		}
		
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
							"osiris_productos.descripcion_producto, "+
							"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto, "+
							"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(porcentage_ganancia,'99999.99') AS porcentageutilidad, "+
							"to_char(costo_producto,'999999999.99') AS costoproducto, "+
							"osiris_grupo_producto.agrupacion, aplicar_iva "+
							"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
							"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
							"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
							"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
							"AND osiris_productos.cobro_activo = 'true' "+
							"AND osiris_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_producto; ";
				//Console.WriteLine("query de productos"+comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				float tomaprecio;
				float calculodeiva;
				float preciomasiva;
				
				while (lector.Read())
				{
					calculodeiva = 0;
					preciomasiva = 0;
					tomaprecio = float.Parse((string) lector["preciopublico"]);
					if ((bool) lector["aplicar_iva"]){
						calculodeiva = (tomaprecio * valoriva)/100;
					}
					
					preciomasiva = tomaprecio + calculodeiva;
					treeViewEngineBusca2.AppendValues (//TreeIter iter =
											(string) lector["codProducto"] ,//0
											(string) lector["descripcion_producto"],//1
											(string) lector["preciopublico"],//2
											calculodeiva.ToString("F").PadLeft(10),//3
											preciomasiva.ToString("F").PadLeft(10),//4
											(string) lector["descripcion_grupo_producto"],//5
											(string) lector["descripcion_grupo1_producto"],//6
											(string) lector["descripcion_grupo2_producto"],//7
											(string) lector["costoproductounitario"],//8
											(string) lector["porcentageutilidad"],//9
											(string) lector["costoproducto"],//10
											(string) lector["agrupacion"]);//11
					
				}
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run (); 	msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;

 			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)) 
 			{
 				//Console.WriteLine("pasavalor");
 				id_produ = (string) model.GetValue(iterSelected, 0);//0(string) lector["codProducto"] ,
				desc_produ = (string) model.GetValue(iterSelected, 1);//1(string) lector["descripcion_producto"],
				precio_produ = (string) model.GetValue(iterSelected, 2);//2(string) lector["preciopublico"],
				iva_produ = (string) model.GetValue(iterSelected, 3);//3calculodeiva.ToString("F").PadLeft(10),
				total_produ = (string) model.GetValue(iterSelected, 4);//4preciomasiva.ToString("F").PadLeft(10),
				costo_unitario_producto = (string) model.GetValue(iterSelected, 8); //8(string) lector["costoproductounitario"],
				porcentage_utilidad_producto = (string) model.GetValue(iterSelected, 9);//9(string) lector["porcentageutilidad"],
				costo_total_producto = (string) model.GetValue(iterSelected, 10);//10(string) lector["costoproducto"],
				agrupacion = (string) model.GetValue(iterSelected,11);
				constante = entry_cantidad_aplicada.Text;
				//varibles numericas
				ppcantidad = float.Parse(precio_produ)*float.Parse(constante);
				float ivaproducto = float.Parse(iva_produ)*float.Parse(constante);
				float suma_total = ppcantidad+ivaproducto;
				////////////////////////////////////////////////
				subtotal_al_15 = float.Parse(entry_subtotal_al_15.Text);
				subtotal_al_0 = float.Parse(entry_subtotal_al_0.Text);
				if ((float) float.Parse(iva_produ) > 0){
 					subtotal_al_15 = subtotal_al_15 + ppcantidad;
 				}else{
 					subtotal_al_0 = subtotal_al_0 + ppcantidad;
 				}		
 				total_iva = total_iva + ivaproducto;
				sub_total = subtotal_al_15 + subtotal_al_0+total_iva;
				
				entry_subtotal_al_15.Text = subtotal_al_15.ToString();
				entry_subtotal_al_0.Text = subtotal_al_0.ToString();
				entry_total_iva.Text = total_iva.ToString();
				entry_subtotal.Text =  sub_total.ToString();
				entry_total.Text = sub_total.ToString();
				//entry_precio_convenido.Text = sub_total.ToString();
				///////////////////////////////////////////
				
				if ((string) constante != ""){
					if((string) model.GetValue(iterSelected, 11) == "IMG") 
					{ idtipointernamiento = 300;		descripinternamiento = "Imagenologia-RX"; }
					
					if((string) model.GetValue(iterSelected, 11) == "LAB") 
					{ idtipointernamiento = 400;		descripinternamiento = "Laboratorio"; }
					
					if ((float) float.Parse(constante) > 0){
						if ((int) idtipointernamiento >= 20){
					 		treeViewEngineServicio.AppendValues ((string) desc_produ,
														(float) float.Parse(constante),
														(string) id_produ,
														(string) precio_produ.ToString().PadLeft(10),
														(string) ppcantidad.ToString("F").PadLeft(10),
														(string) ivaproducto.ToString("F").PadLeft(10),
														(string) suma_total.ToString("F").PadLeft(10),
														(string)this.LoginEmpleado.ToString(),
														(string)DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
														(string) this.descripinternamiento,//9
														(bool) false,//10
														" ",
														(int) idtipointernamiento );
							this.combobox_tipo_admision.Clear();
							entry_cantidad_aplicada.Text = "0";
							entry_expresion.Text = "";
							entry_expresion.GrabFocus();
							llena_combobox_admision();
						}else{
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close, 
									"seleccione un tipo de admision valido \n"+"mayor que cero, intente de nuevo");
							msgBoxError.Run ();						msgBoxError.Destroy();
						}
					}else{
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close, 
										"La cantidad que quiere aplicar debe ser \n"+"mayor que cero, intente de nuevo");
						msgBoxError.Run ();					msgBoxError.Destroy();
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close, 
								"La cantidad que quiere aplicar NO \n"+"puede quedar vacia, intente de nuevo");
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
 			}
 		}
 		
 		void on_button_enviado_clicked(object sender, EventArgs args)
 		{
 			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
		    // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = 	"UPDATE osiris_his_presupuestos_enca SET enviado = 'true' "+
										"WHERE id_presupuesto = '"+(string) entry_id_presupuesto.Text.Trim()+"'; ";
				comando.ExecuteNonQuery();    comando.Dispose();
				activacio_de_los_campos(false);
				button_enviado.Sensitive = false;
			}catch(NpgsqlException ex) {
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run (); 	msgBoxError.Destroy();
			}
 		}
 		
		void on_button_limpiar_clicked(object sender, EventArgs args)
		{
			//limpia_valores();
			this.entry_id_presupuesto.Text = "";
			this.entry_cirugia.Text = "";
			this.entry_medico.Text = "";
			this.entry_dias_internamiento.Text = "0";
			this.entry_deposito_minimo.Text = "0";
			this.entry_fax.Text = "";
			this.entry_notas.Text = "";
			this.entry_tel_medico.Text = "";
			this.entry_tel_opcional.Text = "";
			this.entry_precio_convenido.Text = "0";
			this.button_procedimiento_cobrz.Sensitive = false;
			treeViewEngineServicio.Clear();
			this.entry_subtotal_al_0.Text = "0.00";
			this.entry_subtotal_al_15.Text = "0.00";
			this.entry_total_iva.Text = "0.00";
			this.entry_total.Text = "0.00";
			this.entry_subtotal.Text = "0.00";
			this.entry_precio_convenido.Text = "0.00";
			this.checkbutton_copia_productos.Active = false;
		}
		
		public void limpia_valores()
		{	
			//Console.WriteLine("limpiando valores");
			this.entry_id_presupuesto.Text = "";
			this.entry_cirugia.Text = "";
			this.entry_medico.Text = "";
			this.entry_dias_internamiento.Text = "0";
			this.entry_deposito_minimo.Text = "0";
			this.entry_fax.Text = "";
			this.entry_notas.Text = "";
			this.entry_tel_medico.Text = "";
			this.entry_tel_opcional.Text = "";
			this.entry_precio_convenido.Text = "0";
			if(copiaproductos == false ) { 
				treeViewEngineServicio.Clear();
			 	this.entry_subtotal_al_0.Text = "0.00";
				this.entry_subtotal_al_15.Text = "0.00";
				this.entry_total_iva.Text = "0.00";
				this.entry_total.Text = "0.00";
				this.entry_subtotal.Text = "0.00";
				this.entry_precio_convenido.Text = "0.00";
			}
		}
		
		void on_checkbutton_copia_productos_clicked(object sender, EventArgs args)
		{
			if(checkbutton_copia_productos.Active == true){
				TreeIter iter;
				if(entry_id_presupuesto.Text.Trim() != "" ) {
					copiaproductos = true;		//Console.WriteLine("copiaproductos = "+copiaproductos.ToString());
					limpia_valores();
					tienepaquete = true;
					if (treeViewEngineServicio.GetIterFirst (out iter)){
						if ((bool)lista_de_servicios.Model.GetValue (iter,10) == true) {
			 				lista_de_servicios.Model.SetValue (iter,10,false);	}
						while (treeViewEngineServicio.IterNext(ref iter)) {
			    	   		if ((bool)lista_de_servicios.Model.GetValue (iter,10) == true) {
			 					lista_de_servicios.Model.SetValue (iter,10,false); }
			 			}
			 		}
			 		checkbutton_nuevo_presupuesto.Active = true;
			 	}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"Debe de seleccionar una cirugia \n"+"para poder copiar sus productos");
					msgBoxError.Run ();					msgBoxError.Destroy();
					copiaproductos = false;
						//checkbutton_copia_productos.Active = false;
				}
			}//else{
			if(checkbutton_copia_productos.Active == false){
				copiaproductos = false;		//Console.WriteLine("copiadoproductos = "+copiaproductos.ToString());
				//treeViewEngineServicio.Clear();
			}
		}
		
		void activacio_de_los_campos(bool valor)
		{
			entry_cirugia.Sensitive = valor;
			entry_medico.Sensitive = valor;
			entry_dias_internamiento.Sensitive = valor;
			entry_deposito_minimo.Sensitive = valor;
			entry_dias_internamiento.Sensitive = valor;
			entry_precio_convenido.Sensitive = valor;
			entry_tel_medico.Sensitive = valor;
			entry_tel_opcional.Sensitive = valor;
			entry_fax.Sensitive = valor;
			entry_notas.Sensitive = valor;
			button_busca_medico.Sensitive = valor;
			button_buscar_cirugia.Sensitive = valor;
			button_graba_presupuesto.Sensitive = valor;
			button_procedimiento_cobrz.Sensitive = valor;
			button_quitar_aplicados.Sensitive = valor;
			button_limpiar.Sensitive = valor;
			button_actualizar.Sensitive = valor;
			button_copia_procedimiento.Sensitive = valor;
			checkbutton_copia_productos.Sensitive = valor;
			lista_de_servicios.Sensitive = valor;
		}
		
		
		//ACCION QUE CAMBIA EL COLOR DEL TEXTO PARA CUANDO SE GUARDA EN LA BASE DE DATOS 
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//descripcion_producto descrip = (descripcion_producto) model.GetValue (iter, 14);
			if ((bool)lista_de_servicios.Model.GetValue (iter,10)==true){
				(cell as Gtk.CellRendererText).Foreground = "darkblue";
			}else{
				(cell as Gtk.CellRendererText).Foreground = "red";
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione 
		public void onKeyPressEvent_enter_exp(object o, Gtk.KeyPressEventArgs args)
		{	
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;			llenando_lista_de_productos();			
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_id(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				idpresupuesto = entry_id_presupuesto.Text.Trim();
				llenado_de_presupuesto( entry_id_presupuesto.Text );			
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace")
			{
				//Console.WriteLine(Convert.ToChar(args.Event.Key));
				args.RetVal = true;
			}
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
	}
}
