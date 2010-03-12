//////////////////////////////////////////////////////////
// created on 21/06/2007 at 01:40 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
//				  Ing. Jesus Buentello Garza(Adecuaciones)
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
// Programa		: facturador.cs
// Proposito	: Facturador general
// Objeto		: tesoreria.cs
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class facturador_tesoreria
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;

		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
				
		// Declarando ventana principal del facturador
		[Widget] Gtk.Window facturador;
		[Widget] Gtk.CheckButton checkbutton_nueva_factura;
		[Widget] Gtk.Entry entry_numero_factura;
		[Widget] Gtk.Entry entry_fecha_factura;
		[Widget] Gtk.Entry entry_status_factura;
		[Widget] Gtk.Entry entry_id_cliente; 
 		[Widget] Gtk.Entry entry_nombre_cliente;
 		[Widget] Gtk.Entry entry_rfc_cliente;
 		[Widget] Gtk.Entry entry_curp_cliente;
 		[Widget] Gtk.Entry entry_cp_cliente;
 		[Widget] Gtk.Entry entry_direccion_cliente;
 		[Widget] Gtk.Entry entry_colonia_cliente;
 		[Widget] Gtk.Entry entry_municipio_cliente;
		[Widget] Gtk.Entry entry_estado_cliente;
 		[Widget] Gtk.Entry entry_telefono_cliente;
 		[Widget] Gtk.Entry entry_fax_cliente;
 		[Widget] Gtk.Entry entry_correo_electronico;
 		// Detalle vacio
 		[Widget] Gtk.CheckButton checkbutton_detalle_vacio;
		[Widget] Gtk.Button button_detalle_vacio;
 		
 		[Widget] Gtk.Entry entry_subtotal_15;
 		[Widget] Gtk.Entry entry_subtotal_0;
 		[Widget] Gtk.Entry entry_total_iva;
 		[Widget] Gtk.Entry entry_subtotal;
 		[Widget] Gtk.Entry entry_deducible_factura;
 		[Widget] Gtk.Entry entry_coaseguro_porcentage;
 		[Widget] Gtk.Entry entry_coaseguro_factura;
 		[Widget] Gtk.Entry entry_total_factura;
 		[Widget] Gtk.Entry entry_creada_por;
 		 		
		[Widget] Gtk.Button button_selecciona_factura;
		[Widget] Gtk.Button button_busca_cliente;
		[Widget] Gtk.Button button_selecc_folios;
		[Widget] Gtk.Button button_agrega_cliente;
		[Widget] Gtk.Button button_cancela_factura;
		[Widget] Gtk.Button button_guardar_factura;
		[Widget] Gtk.Button button_deducible;
		[Widget] Gtk.Button button_coaseguro;
		[Widget] Gtk.Button button_imprime_factura;
		[Widget] Gtk.Button button_pagar_factura;
		[Widget] Gtk.Button button_quitar;

		
		[Widget] Gtk.TreeView treeview_detalle_de_factura;
		[Widget] Gtk.Statusbar statusbar_facturador; //Declarando la barra de estado
				
		/////// Ventana Busqueda de Clientes\\\\\\\\
		[Widget] Gtk.Window busca_cliente;
		[Widget] Gtk.TreeView lista_de_cliente;
		[Widget] Gtk.RadioButton radiobutton_nombre_client;
		[Widget] Gtk.RadioButton radiobutton_rfc_client;
		[Widget] Gtk.RadioButton radiobutton_num_client;
		
		// Ventana de Seleccion de Folios para la factura
		[Widget] Gtk.Window selecciona_folios_factura;
		[Widget] Gtk.TreeView treeview_selec_procediminetos;
		[Widget] Gtk.Button button_acepta_folios;
		//[Widget] Gtk.Entry entry_contador_proced;
		
		[Widget] Gtk.CheckButton checkbutton_info_paciente;
		[Widget] Gtk.CheckButton checkbutton_info_ingr_egre;
		[Widget] Gtk.CheckButton checkbutton_info_cirugia;
		[Widget] Gtk.CheckButton checkbutton_info_compr_caja;
		[Widget] Gtk.CheckButton checkbutton_poliza;
		[Widget] Gtk.CheckButton checkbutton_certificado;
		[Widget] Gtk.CheckButton checkbutton_honorario_medico;
		[Widget] Gtk.CheckButton checkbutton_infoanexo_1;
		[Widget] Gtk.CheckButton checkbutton_infoanexo_2;
		[Widget] Gtk.CheckButton checkbutton_infoanexo_3;
		[Widget] Gtk.CheckButton checkbutton_infoanexo_4;
		
		
		[Widget] Gtk.Entry entry_diagnostico_factura;
		[Widget] Gtk.Entry entry_num_recibos_factura;
		
		[Widget] Gtk.Entry entry_infoanexo_1;
		[Widget] Gtk.Entry entry_infoanexo_2;
		[Widget] Gtk.Entry entry_infoanexo_3;
		[Widget] Gtk.Entry entry_infoanexo_4;
		
		//Ventana de Deducible
		[Widget] Gtk.Window deducible_coaseguro;
		[Widget] Gtk.Label label_deducible_coaseguro;
		[Widget] Gtk.Entry entry_deducible_coaseguro;
		[Widget] Gtk.Button button_acepta_deducible;
		
		// Ventana de Pago de Factura
		[Widget] Gtk.Window fecha_pago_factura;
		[Widget] Gtk.Entry entry_dia;
		[Widget] Gtk.Entry entry_mes;
		[Widget] Gtk.Entry entry_ano;
		[Widget] Gtk.Button button_guardar_pago;
		
		//Ventana Detalle Vacio
		[Widget] Gtk.Window factura_detalle;
		[Widget] Gtk.CheckButton checkbutton_detalle;
		[Widget] Gtk.CheckButton checkbutton_iva;
		[Widget] Gtk.Entry entry_cantidad;
		[Widget] Gtk.Entry entry_precio;
		[Widget] Gtk.Entry entry_descripcion;	
		[Widget] Gtk.Button button_aceptar_detalle;
		[Widget] Gtk.Button button_cancelar_detalle;
		[Widget] Gtk.CheckButton checkbutton_honorarios_medico;
		[Widget] Gtk.Entry entry_honorarios_medico;
		
		//Nota de Credito
		[Widget] Gtk.Button button_nota_credito;
		
		
		// Declaracion de variables de la clase
		int idcliente = 1;					// Toma el id del cliente que se va facturar
		int idadmision_ = 0;					//tipo de admision en donde se realizaron los cargos...
		int idgrupoproducto = 0;
		int id_tipopaciente = 0;
		bool aplicar_descuento = true;
		bool aplicar_siempre = false;
		bool factura_cancelada = true;
		bool error_no_existe = false;
		bool enviofactura_cliente = false;
		bool enviofactura_factutu = false;	
		
		decimal precio_por_cantidad = 0;		//esta variable se utiliza para ir guwerdandop el precio de un producto dependiendo de cuanto se aplico de este
		decimal iva_del_grupo = 0;					//es un valor en donde se van a ir sumando cada iva que se le aplica al producto
		decimal porcentagedesc = 0;			//es el el descuento en porciento si es que existe un descuento
		decimal descuento_neto = 0;			// valor desc sin iva
		decimal descurento_del_grupo = 0;		//el descuento que se aplica en cada grupo de productos
		decimal iva_de_descuento = 0;			// valor iva del descuento 
		decimal descuento_del_grupo = 0;		// suma del iva del desc y del desc neto
		decimal subtotal_del_grupo = 0;		//subtotal del grupo de productos
		decimal subtotal_al_impuesto_grupo = 0;		//es el subtotal de los productos que contienen iva en un grupo de productos
		decimal subtotal_al_0_grupo = 0;		//es el subtotal de los productos que no contienen iva en un grupo de productos
		decimal subtotal_al_impuesto = 0;			//es el subtotal de los productos que contienen iva en todo el movimiento
		decimal subtotal_al_0 = 0;			//es el subtotal de los productos que no contienen iva en todo el movimiento
		decimal total_del_grupo = 0;			//precio total del grupo de productos
		decimal total_de_iva = 0;				//suma de todos los ivas de todos los lugares y grupos de productos
		decimal total_de_descuento_neto =0;	//es el descuento neto de facturacion
		decimal total_de_iva_descuento =0;	//es el iva del descuento neto de facturacion
		decimal total_descuento=0;			//es la la suma del descuento neto y el iva del descuento neto de facturacion
		decimal deducible_factura = 0;
		decimal coaseguro_factura = 0;
		decimal valor_coaseguro = 0;
		decimal total_honorario_medico = 0;
		float totaldefactura = 0;
		float totalhonorariomedico = 0; 
				
		string folioservicio_factura = "";
		string diagnostico_factura = "";
		string numeros_folios_seleccionado = "";
		string numeros_seleccionado = "";
		string cantidad_en_letras = "";
		int num_nota = 0;					// igualando la nota de credito
		
		int idaseguradora = 0;
		
		int marca_un_folio = 0;
		int ultimafactura = 0;
		string numerodefactura = "";
		string municipios = "";
		string estado = "";		
		
		//Variables Detalle Vacio
		decimal suma_sin_iva = 0;
		double iva = 0;
		//public decimal sumasiniva = 0;
		//public decimal sumaconiva = 0;
		decimal suma_ = 0;
		//public decimal suma_del_iva = 0;
		decimal subtotales = 0;
		double valoriva = 0;
		decimal honorarios = 0;
			
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		
		string connectionString;
		string nombrebd;
				
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		private TreeStore treeViewEngineBusca3;   	// Clientes
		private TreeStore treeViewEngineSelProce;  	// Seleccion de procedimientos para facturar
		private TreeStore treeViewEngineDetaFact; 	// Detalle de la Factura 
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public facturador_tesoreria(string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_ ) 
		{
			LoginEmpleado = LoginEmp;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd; 
			valoriva = double.Parse(classpublic.ivaparaaplicar)/100;
			
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "facturador", null);
			gxml.Autoconnect (this);
	        
			// Muestra ventana de Glade
			facturador.Show();
			
			// Validando que sen solo numeros
			entry_numero_factura.KeyPressEvent += onKeyPressEvent_enter_factura;
			// Seleccionando Factura						
			button_selecciona_factura.Clicked += new EventHandler(on_button_selecciona_factura_clicked);
			// Busqueda de Paciente
			button_busca_cliente.Clicked += new EventHandler(on_button_busca_cliente_clicked);
			// Selecciona folio para facturar
			button_selecc_folios.Clicked += new EventHandler(on_button_selecc_folios_clicked);
			// Cancela la factura
			button_cancela_factura.Clicked += new EventHandler(on_button_cancela_factura_clicked);
			// Agrega cliente
			button_agrega_cliente.Clicked += new EventHandler(on_button_agrega_cliente_clicked);
			// Creacion de una Nueva Factura
			checkbutton_nueva_factura.Clicked +=  new EventHandler(on_checkbutton_nueva_factura_clicked);
			// Imprime la factura
			button_imprime_factura.Clicked +=  new EventHandler(on_button_imprime_factura_clicked);
			// Grabar Factura
			button_guardar_factura.Clicked +=  new EventHandler(on_button_guardar_factura_clicked);
			// Ingreso del deducible
			button_deducible.Clicked +=  new EventHandler(on_button_deducible_clicked);
			// Ingreso de Coaseguro
			button_coaseguro.Clicked +=  new EventHandler(on_button_coaseguro_clicked);			
			// Pagar Factura de Cliente
			button_pagar_factura.Clicked +=  new EventHandler(on_button_pagar_factura_clicked);
			// Check Detalle Vacio
			checkbutton_detalle_vacio.Clicked += new EventHandler(on_checkbutton_detalle_vacio);
			// Button Detalle Vacio
			button_detalle_vacio.Clicked += new EventHandler(on_button_detalle_vacio);
			// Quitar aplicados
			button_quitar.Clicked += new EventHandler(on_button_quitar_aplicados_clicked);
			// Nota de Credito
			button_nota_credito.Clicked += new EventHandler(on_button_nota_credito_clicked);
			
			// Desactivando botones en la entrada
			button_guardar_factura.Sensitive = false;
			button_selecc_folios.Sensitive = false;
			button_busca_cliente.Sensitive = false;
			button_deducible.Sensitive = false;
			button_coaseguro.Sensitive = false;
			button_imprime_factura.Sensitive = false;
			button_detalle_vacio.Sensitive = false;
			checkbutton_detalle_vacio.Sensitive = false;
			checkbutton_detalle_vacio.Sensitive = false;
						
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			// crea treeview del detalle de la factura
			crea_treeview_facturador();
			
			entry_numero_factura.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169));
			entry_status_factura.ModifyBase(StateType.Normal, new Gdk.Color(166,220,255));
			entry_creada_por.ModifyBase(StateType.Normal, new Gdk.Color(54,180,221));
												
			statusbar_facturador.Pop(0);
			statusbar_facturador.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_facturador.HasResizeGrip = false; 
		}
		
		void on_button_nota_credito_clicked(object sender, EventArgs args)
		{
			new osiris.nota_de_credito(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,numerodefactura,this.idcliente,
			                          subtotal_al_0,subtotal_al_impuesto,total_de_iva,subtotales,num_nota);
		}
		
		void on_checkbutton_nueva_factura_clicked(object sender, EventArgs args)
		{
			
			if (checkbutton_nueva_factura.Active == true){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de CREAR una Nueva FACTURA ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
		 			checkbutton_nueva_factura.Sensitive = true;
		 			button_guardar_factura.Sensitive = true;
		 			this.button_imprime_factura.Sensitive = false;
		 			this.button_cancela_factura.Sensitive = false;
		 			this.button_pagar_factura.Sensitive = false;
		 			
					subtotal_al_0 = 0; 
					subtotal_al_impuesto = 0;
					total_de_iva = 0;
					subtotales = 0;
					
					// Buscando el ultimo numero de factura
		 			entry_numero_factura.Text = classpublic.lee_ultimonumero_registrado("osiris_erp_factura_enca","numero_factura","");
					numerodefactura = entry_numero_factura.Text;
					
					treeViewEngineDetaFact.Clear();   // limpia treeview de factura
					entry_numero_factura.Text = ultimafactura.ToString();
					numerodefactura = ultimafactura.ToString();
					entry_fecha_factura.Text = DateTime.Now.ToString("yyyy-MM-dd");//("dd-MM-yyyy");
					button_busca_cliente.Sensitive = true;
					button_deducible.Sensitive = true;
					button_coaseguro.Sensitive = true;
					limpia_datos_de_entry();																	
				}else{
		 			checkbutton_nueva_factura.Active = false;
		 			button_guardar_factura.Sensitive = false;
		 			button_selecc_folios.Sensitive = false;
		 			button_busca_cliente.Sensitive = false;
		 			button_deducible.Sensitive = false;
		 			button_coaseguro.Sensitive = false;
		 			this.checkbutton_detalle_vacio.Sensitive = false;
		 		}
		 	}else{
		 		button_guardar_factura.Sensitive = false;
		 		button_selecc_folios.Sensitive = false;
		 		button_busca_cliente.Sensitive = false;
		 		button_deducible.Sensitive = false;
		 	 	this.checkbutton_detalle_vacio.Sensitive = false;
		 	}
		}
		
		void limpia_datos_de_entry()
		{
			entry_id_cliente.Text = ""; 
 			entry_nombre_cliente.Text = "";
 			entry_rfc_cliente.Text = "";
 			entry_curp_cliente.Text = "";
 			entry_cp_cliente.Text = "";
 			entry_direccion_cliente.Text = "";
 			entry_colonia_cliente.Text = "";
 			entry_municipio_cliente.Text = "";
			entry_estado_cliente.Text = "";
 			entry_telefono_cliente.Text = "";
 			entry_fax_cliente.Text = "";
 			entry_correo_electronico.Text = "";
 			entry_status_factura.Text = "";
						
			entry_subtotal_15.Text = "0";
 			entry_subtotal_0.Text = "0";
			entry_total_iva.Text = "0";
 			entry_subtotal.Text = "0";
 			entry_deducible_factura.Text = "0";
 			entry_coaseguro_porcentage.Text = "0";
 			entry_coaseguro_factura.Text = "0";
 			entry_total_factura.Text = "0";
 			entry_creada_por.Text = "";
		}
		void on_button_selecciona_factura_clicked(object sender, EventArgs args)
		{
			llenado_de_factura();
		}
		
		void llenado_de_factura()
		{
			//checkbutton_nueva_factura.Sensitive = false;
		 	this.button_guardar_factura.Sensitive = false;
		 	this.button_imprime_factura.Sensitive = true;
		 	this.checkbutton_nueva_factura.Active = false;
		 	this.button_cancela_factura.Sensitive = true;
		 	this.button_pagar_factura.Sensitive = true;
			// Procesando el encabezado de la Factura
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT osiris_erp_factura_enca.id_cliente,osiris_erp_factura_enca.descripcion_cliente,osiris_erp_factura_enca.direccion_cliente,osiris_erp_factura_enca.colonia_cliente,osiris_erp_factura_enca.municipio_cliente,"+
									"osiris_erp_factura_enca.estado_cliente,osiris_erp_factura_enca.rfc_cliente,osiris_erp_factura_enca.curp_cliente,osiris_erp_factura_enca.telefono1_cliente,osiris_erp_factura_enca.telefono2_cliente,"+
									"osiris_erp_factura_enca.fax_cliente,osiris_erp_factura_enca.mail_cliente,osiris_erp_factura_enca.cp_cliente,"+
									"osiris_erp_factura_enca.contacto_cliente,osiris_erp_factura_enca.telefono_contacto_cliente,to_char(deducible,'99999999.99') AS deducible_,to_char(coaseguro,'99999999.99') AS coaseguro_,"+
									"to_char(honorario_medico,'99999999.99') AS honorariomedico,to_char(sub_total_15,'99999999.99') AS subtotal_15,to_char(sub_total_0,'99999999.99') AS subtotal_0,"+
									"to_char(iva_al_impuesto,'99999999.99') AS ivaal_15,to_char(valor_coaseguro,'99999999.99') AS valorcoaseguro,numero_factura, "+
									"cancelado,to_char(fechahora_cancelacion,'dd-MM-yyyy') AS fechahoracancelacion,to_char(fecha_factura,'dd-MM-yyyy') AS fechafactura,"+
									"motivo_cancelacion,pagada,to_char(fechahora_pago_factura,'dd-MM-yyyy') AS fechahorapagofactura,"+
									"osiris_erp_factura_enca.id_quien_creo,osiris_erp_clientes.envio_factura,numero_ntacred,"+
									"osiris_erp_factura_enca.enviado "+
									"FROM osiris_erp_factura_enca,osiris_erp_clientes "+
									"WHERE numero_factura = '"+entry_numero_factura.Text.Trim()+"' "+
									"AND osiris_erp_factura_enca.id_cliente = osiris_erp_clientes.id_cliente ;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
						
				if ((bool) lector.Read()){
					
					enviofactura_cliente = (bool) lector["envio_factura"];
					enviofactura_factutu = (bool) lector["enviado"];				
				
					//nota Credito
					this.idcliente = (int) lector["id_cliente"];
					this.num_nota = (int) lector["numero_ntacred"];
					
					numerodefactura = entry_numero_factura.Text.Trim();
					entry_id_cliente.Text = ""; 
					entry_fecha_factura.Text = (string) lector["fechafactura"];
		 			entry_nombre_cliente.Text = (string) lector["descripcion_cliente"];
		 			entry_rfc_cliente.Text = (string) lector["rfc_cliente"];
		 			entry_curp_cliente.Text = (string) lector["curp_cliente"];
		 			entry_cp_cliente.Text = (string) lector["cp_cliente"];
		 			entry_direccion_cliente.Text = (string) lector["direccion_cliente"];
		 			entry_colonia_cliente.Text = (string) lector["colonia_cliente"];
		 			entry_municipio_cliente.Text = (string) lector["municipio_cliente"];
					entry_estado_cliente.Text = (string) lector["estado_cliente"];
		 			entry_telefono_cliente.Text = (string) lector["telefono1_cliente"];
		 			entry_fax_cliente.Text = (string) lector["fax_cliente"];
		 			entry_correo_electronico.Text = (string) lector["mail_cliente"];
		 			entry_creada_por.Text = (string) lector["id_quien_creo"];
								
					entry_subtotal_15.Text = float.Parse((string) lector["subtotal_15"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")).ToString().PadLeft(10);
		 			entry_subtotal_0.Text = float.Parse((string) lector["subtotal_0"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")).ToString("C").PadLeft(10);
					entry_total_iva.Text = float.Parse((string) lector["ivaal_15"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")).ToString("C").PadLeft(10);
					
		 			entry_subtotal.Text = (float.Parse((string) lector["honorariomedico"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+ 
		 						  	  	   float.Parse((string) lector["subtotal_15"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
		 							  	   float.Parse((string) lector["subtotal_0"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
		 							  	   float.Parse((string) lector["ivaal_15"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))).ToString("C").PadLeft(10);
		 			
		 			entry_deducible_factura.Text = float.Parse((string) lector["deducible_"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")).ToString("C").PadLeft(10);
		 			entry_coaseguro_porcentage.Text = float.Parse((string) lector["valorcoaseguro"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")).ToString("C").PadLeft(10);
		 			entry_coaseguro_factura.Text = float.Parse((string) lector["coaseguro_"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")).ToString("C").PadLeft(10);
		 					 			
		 			entry_total_factura.Text = ((float.Parse((string) lector["honorariomedico"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
		 									     float.Parse((string) lector["subtotal_15"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
		 									     float.Parse((string) lector["subtotal_0"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
		 									     float.Parse((string) lector["ivaal_15"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")))-
		 									    (float.Parse((string) lector["deducible_"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
		 									     float.Parse((string) lector["valorcoaseguro"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")))).ToString("C").Trim();
					
					totaldefactura = (float.Parse((string) lector["honorariomedico"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
		 									     float.Parse((string) lector["subtotal_15"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
		 									     float.Parse((string) lector["subtotal_0"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
		 									     float.Parse((string) lector["ivaal_15"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")))-
		 									    (float.Parse((string) lector["deducible_"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
		 									     float.Parse((string) lector["valorcoaseguro"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")));
		 									     
		 			totalhonorariomedico = float.Parse((string) lector["honorariomedico"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					
					subtotal_al_0 = decimal.Parse((string) lector["subtotal_0"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
                    subtotal_al_impuesto = decimal.Parse((string) lector["subtotal_15"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
                    total_de_iva = decimal.Parse((string) lector["ivaal_15"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
                    subtotales = Convert.ToDecimal((float.Parse((string) lector["honorariomedico"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
                                                  float.Parse((string) lector["subtotal_15"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
                                                  float.Parse((string) lector["subtotal_0"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
                                                  float.Parse((string) lector["ivaal_15"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")))-
                                                 (float.Parse((string) lector["deducible_"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
                                                  float.Parse((string) lector["valorcoaseguro"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))));
                    
					
					
		 			//Console.WriteLine(((float.Parse((string)lector["honorariomedico"])+float.Parse((string) lector["subtotal_15"])+float.Parse((string) lector["subtotal_0"])+float.Parse((string) lector["ivaal_15"]))-(float.Parse((string) lector["deducible_"])+float.Parse((string) lector["valorcoaseguro"]))).ToString("C").Trim());
		 									     
		 			cantidad_en_letras = classpublic.ConvertirCadena(((float.Parse((string)lector["honorariomedico"])+float.Parse((string) lector["subtotal_15"])+float.Parse((string) lector["subtotal_0"])+float.Parse((string) lector["ivaal_15"]))-(float.Parse((string) lector["deducible_"])+float.Parse((string) lector["valorcoaseguro"]))).ToString().Trim(),"Peso");
		 			
		 			entry_status_factura.Text = "";
		 			
		 			if ((bool) lector["pagada"] == true){
		 				button_pagar_factura.Sensitive = false;
		 				this.button_cancela_factura.Sensitive = false;
		 				entry_status_factura.Text = "FAC. P A G A D A / "+(string) lector["fechahorapagofactura"];
		 				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
												MessageType.Info,ButtonsType.Ok,"Factura PAGADA con Fecha "+(string) lector["fechahorapagofactura"]);
						msgBox.Run();	msgBox.Destroy();
		 			}
		 			if ((bool) lector["cancelado"] == true){
		 				button_pagar_factura.Sensitive = false;
		 				this.button_cancela_factura.Sensitive = false;
		 				entry_status_factura.Text = "FAC. C A N C E L A D A / "+(string) lector["fechahoracancelacion"];
		 				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
												MessageType.Info,ButtonsType.Ok,"Factura CANCELADA con Fecha "+(string) lector["fechahoracancelacion"]);
						msgBox.Run();	msgBox.Destroy();
		 			}		

					conexion.Close();
					
					treeViewEngineDetaFact.Clear();  // limpia de talle de factura
					// Llenando el DETALLE DE LA FACTURA
					
					NpgsqlConnection conexion1;
					conexion1 = new NpgsqlConnection (connectionString+nombrebd );
					// Verifica que la base de datos este conectada
					try{
						conexion1.Open ();
						NpgsqlCommand comando1; 
						comando1 = conexion1.CreateCommand ();
						comando1.CommandText = "SELECT numero_factura,to_char(cantidad_detalle,'99999999.99') AS cantidaddetalle,descripcion_detalle,"+
												"to_char(precio_unitario,'99999999.99') AS preciounitario,to_char(importe_detalle,'99999999.99') AS importedetalle,tipo_detalle "+
												"FROM osiris_erp_factura_deta WHERE numero_factura = '"+numerodefactura+"' "+
												"ORDER BY id_secuencia;";
						NpgsqlDataReader lector1 = comando1.ExecuteReader ();
						
						string variable_paso_01 = "";
						string variable_paso_02 = "";
						string variable_paso_03 = "";
						while (lector1.Read()){
							// validando cantidad detalle
							if (float.Parse((string)lector1["cantidaddetalle"]) == 0){
								 variable_paso_01 = "";
							}else{
								variable_paso_01 = (string) lector1["cantidaddetalle"];
							}
							// validando precio
							if (float.Parse((string) lector1["preciounitario"]) == 0){
								 variable_paso_02 = "";
							}else{
								variable_paso_02 = (string) lector1["preciounitario"];
							}
							// validando importe
							if (float.Parse((string)lector1["importedetalle"]) == 0){
								 variable_paso_03 = "";
							}else{
								variable_paso_03 = (string) lector1["importedetalle"];
							}
							treeViewEngineDetaFact.AppendValues(variable_paso_01,(string) lector1["descripcion_detalle"],variable_paso_02,variable_paso_03,(bool) lector1["tipo_detalle"]);
							
						}
						conexion1.Close();
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error, 
												ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
					}
				}else{
					limpia_datos_de_entry();
					treeViewEngineDetaFact.Clear();  // limpia de talle de factura
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
												MessageType.Info,ButtonsType.Ok,"La factura seleccionada NO EXISTE verifique...");
					msgBox.Run();	msgBox.Destroy();
					error_no_existe = true;
				}
				
			}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
											ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();		msgBoxError.Destroy();
			}
		}
		
		void on_button_busca_cliente_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "busca_cliente", null);
			gxml.Autoconnect (this);
			busca_cliente.Show();
			
			crea_treeview_busqueda("cliente");
			button_buscar_busqueda.Clicked += new EventHandler(on_buscar_cliente_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_cliente_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter;  
			
		}
		
		void on_button_guardar_factura_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de GUARDAR esta FACTURA ?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
		 			
		 	if (miResultado == ResponseType.Yes){
				guardar_factura();
		 	}
		}
		
		void on_button_cancela_factura_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
			                           MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de CANCELA FACTURA Nº "+entry_numero_factura.Text.Trim()+" ?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
		 			
		 	if (miResultado == ResponseType.Yes){
				cancelar_factura();		 	
		 	}
		}
		
		// Ventana del Deducible
		void on_button_deducible_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "deducible_coaseguro", null);
			gxml.Autoconnect (this);
			deducible_coaseguro.Show();
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_acepta_deducible.Clicked += new EventHandler(on_button_acepta_deducible_clicked);
			// Validando que sen solo numeros
			entry_deducible_coaseguro.KeyPressEvent += onKeyPressEvent;
			entry_deducible_factura.Text = deducible_factura.ToString().PadLeft(10);
		}
		
		void on_button_coaseguro_clicked(object sender, EventArgs args)
		{
			
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "deducible_coaseguro", null);
			gxml.Autoconnect (this);
			deducible_coaseguro.Show();
			deducible_coaseguro.Title = "COASEGURO";
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_acepta_deducible.Clicked += new EventHandler(on_button_acepta_coaseguro_clicked);
			// Validando que sen solo numeros
			entry_deducible_coaseguro.KeyPressEvent += onKeyPressEvent;
			this.entry_coaseguro_factura.Text = this.coaseguro_factura.ToString().PadLeft(10);
			this.label_deducible_coaseguro.Text = "Agregando COASEGURO a Factura";
		}
		
		void on_button_acepta_deducible_clicked(object sender, EventArgs args)
		{
			
				decimal subtotal_factura = subtotal_al_impuesto+subtotal_al_0+total_de_iva;
				
				deducible_factura = decimal.Parse(entry_deducible_coaseguro.Text);
				
				entry_deducible_factura.Text = deducible_factura.ToString("C").PadLeft(10);
				
				decimal total_de_la_factura = ((subtotal_factura-deducible_factura)-coaseguro_factura)+total_honorario_medico;
								
				entry_total_factura.Text = total_de_la_factura.ToString("C").PadLeft(10);
				
				cantidad_en_letras = classpublic.ConvertirCadena(total_de_la_factura.ToString("F").Trim(),"Peso");
				
				deducible_coaseguro.Destroy();
		}
		
		void on_button_acepta_coaseguro_clicked(object sender, EventArgs args)
		{
			decimal subtotal_factura = subtotal_al_impuesto+subtotal_al_0+total_de_iva;
				
			coaseguro_factura = decimal.Parse(entry_deducible_coaseguro.Text);
				
			this.entry_coaseguro_factura.Text = coaseguro_factura.ToString("C").PadLeft(10);
				
			decimal total_de_la_factura = ((subtotal_factura-coaseguro_factura)-deducible_factura)+total_honorario_medico;
								
			entry_total_factura.Text = total_de_la_factura.ToString("C").PadLeft(10);
				
			cantidad_en_letras = classpublic.ConvertirCadena(total_de_la_factura.ToString("F").Trim(),"Peso");
			
			deducible_coaseguro.Destroy();
		}
		
		void on_button_pagar_factura_clicked(object sender, EventArgs args)
		{
			if ((enviofactura_cliente == true && enviofactura_factutu == true) || (enviofactura_cliente == false && enviofactura_factutu == false)){
				if (numerodefactura != ""){
					Glade.XML gxml = new Glade.XML (null, "caja.glade", "fecha_pago_factura", null);
					gxml.Autoconnect (this);
			        // Muestra ventana de Glade
					fecha_pago_factura.Show();
					this.entry_dia.Text = DateTime.Now.ToString("dd");
					this.entry_mes.Text = DateTime.Now.ToString("MM");
					this.entry_ano.Text = DateTime.Now.ToString("yyyy");
					button_guardar_pago.Clicked +=  new EventHandler(on_button_graba_pago_clicked);
					button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, 	ButtonsType.Close, "No puede pagar factura ya que ha elegido ninguna, verifique...");
					msgBoxError.Run ();				msgBoxError.Destroy();
				}				
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, 	ButtonsType.Close, "NO PUEDE PAGAR ESTA FACTURA YA QUE NO HA SIDO ENVIADA A COBRO...");
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
		}	
		
		void on_checkbutton_detalle_vacio(object sender, EventArgs args)		
		{
			if(this.checkbutton_detalle_vacio.Active == true){
				this.button_detalle_vacio.Sensitive = true;
				this.button_selecc_folios.Sensitive = false;
			}else{
				this.button_detalle_vacio.Sensitive = false;
				this.button_selecc_folios.Sensitive = true;
			}
		}
		 
		void on_button_detalle_vacio(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "factura_detalle", null);
			gxml.Autoconnect (this);
			this.factura_detalle.Show();
			 this.button_cancelar_detalle.Clicked += new EventHandler(on_cierraventanas_clicked);
			this.checkbutton_detalle.Clicked += new EventHandler(on_checkbutton_semsitive1);
			checkbutton_iva.Clicked += new EventHandler(on_checkbutton_semsitive2);
			this.checkbutton_honorarios_medico.Clicked += new EventHandler(on_checkbutton_semsitive3);
			this.button_aceptar_detalle.Clicked += new EventHandler(on_button_aceptar_detalle_clicked);
			entry_cantidad.Sensitive = false;
			entry_precio.Sensitive = false;		
			this.entry_honorarios_medico.Sensitive = false;		
		}
		
		void on_checkbutton_semsitive1(object sender, EventArgs args)
		{
			if(checkbutton_detalle.Active == true && checkbutton_iva.Active == false){
				entry_cantidad.Sensitive = true;
				entry_precio.Sensitive = true;								
			}else{
				entry_cantidad.Sensitive = false;
				entry_precio.Sensitive = false;
			}
		}
		
		void on_checkbutton_semsitive2(object sender, EventArgs args)
		{
			if(checkbutton_iva.Active == true && checkbutton_detalle.Active == true)
			{
				entry_cantidad.Sensitive = true;
				entry_precio.Sensitive = true;
				
			}else{
				entry_cantidad.Sensitive = false;
				entry_precio.Sensitive = false;
				
			}
		}
		
		void on_checkbutton_semsitive3(object sender, EventArgs args)
		{
			if(checkbutton_honorarios_medico.Active == true)
			{
				this.entry_honorarios_medico.Sensitive = true;	
			}else{
				this.entry_honorarios_medico.Sensitive = false;	
			}
		}
		
		void on_button_aceptar_detalle_clicked(object sender, EventArgs args)
		{
			
			decimal cantidad = 0; 
			decimal precio = 0;
			string descripcion_producto = "";
			//this.entry_honorarios_medico.Text = "";
			
			if(this.checkbutton_detalle.Active == false && this.checkbutton_iva.Active == false && this.checkbutton_honorarios_medico.Active == false){	
					descripcion_producto = this.entry_descripcion.Text;
					treeViewEngineDetaFact.AppendValues("",descripcion_producto,"","");
			}
			
			if(this.checkbutton_iva.Active == true && this.checkbutton_detalle.Active == true){
				precio =  Convert.ToDecimal(entry_precio.Text);
				cantidad = Convert.ToDecimal(entry_cantidad.Text);
				descripcion_producto = this.entry_descripcion.Text;
				
				suma_ = precio * cantidad;
				iva = (Convert.ToDouble((precio * cantidad)) * valoriva);// + Convert.ToDouble(suma_);
				treeViewEngineDetaFact.AppendValues("+ IVA",descripcion_producto,"",Convert.ToString(suma_));
				total_de_iva += Convert.ToDecimal(iva);
				subtotal_al_impuesto += suma_;
			}
				
				
			if(this.checkbutton_detalle.Active == true && this.checkbutton_iva.Active == false){			
				precio =  Convert.ToDecimal(entry_precio.Text);
				cantidad = Convert.ToDecimal(entry_cantidad.Text);
				descripcion_producto = this.entry_descripcion.Text;
				
				suma_sin_iva = precio * cantidad;
				treeViewEngineDetaFact.AppendValues("     ",descripcion_producto,"",Convert.ToString(suma_sin_iva));
				subtotal_al_0 += suma_sin_iva;
			}
			
			if(this.checkbutton_honorarios_medico.Active == true && this.checkbutton_iva.Active == false && this.checkbutton_iva.Active == false ){			
				
				descripcion_producto = this.entry_descripcion.Text;
				
				this.totalhonorariomedico = float.Parse(this.entry_honorarios_medico.Text) ;

				
				treeViewEngineDetaFact.AppendValues("     ",descripcion_producto,"",this.entry_honorarios_medico.Text);
			}			
				
			this.entry_subtotal_0.Text = subtotal_al_0.ToString("C");
			this.entry_subtotal_15.Text = (subtotal_al_impuesto).ToString("C");
			this.entry_total_iva.Text = total_de_iva.ToString("C");
			subtotales = subtotal_al_0 + subtotal_al_impuesto + total_de_iva + Convert.ToDecimal(totalhonorariomedico);
			this.entry_subtotal.Text = subtotales.ToString("C");
			this.entry_total_factura.Text = subtotales.ToString("C");
				
			cantidad_en_letras = classpublic.ConvertirCadena(subtotales.ToString("F").Trim(),"Peso");
			
			checkbutton_iva.Active = false;
			checkbutton_detalle.Active = false;
			this.entry_cantidad.Text = "0";
			entry_precio.Text = "0";
			entry_descripcion.Text = "";
			this.entry_honorarios_medico.Text = "";
		}
		
		void on_button_quitar_aplicados_clicked (object sender, EventArgs args)
		{
			TreeIter iter;
 			TreeModel model;
 			string toma_valor1;
 			string toma_valor2;
 			string toma_valor3;
			string toma_valor4;
 			float toma_a_pagar = 0;
 
			if (treeview_detalle_de_factura.Selection.GetSelected (out model, out iter)) {
				toma_valor1 = (string) treeview_detalle_de_factura.Model.GetValue (iter,0);	
				toma_valor2 = (string) treeview_detalle_de_factura.Model.GetValue (iter,1);  // toma el iva
				toma_valor3 = (string) treeview_detalle_de_factura.Model.GetValue (iter,2);  // toma el descuento
				toma_valor4 = (string) treeview_detalle_de_factura.Model.GetValue (iter,3);  // toma el descuento
							
				if(toma_valor4 == ""){
					treeViewEngineDetaFact.Remove (ref iter); 					
				}else{
																				
					treeViewEngineDetaFact.Remove (ref iter); 					
					if (toma_valor1 == "+ IVA"){
						subtotal_al_impuesto = subtotal_al_impuesto - Convert.ToDecimal (toma_valor4);
						total_de_iva = total_de_iva - (Convert.ToDecimal(toma_valor4) * Convert.ToDecimal(valoriva));
						
					}else{
						subtotal_al_0 = subtotal_al_0 - decimal.Parse(toma_valor4);
					}
	
					this.entry_subtotal_0.Text = subtotal_al_0.ToString("C");
					this.entry_subtotal_15.Text = (subtotal_al_impuesto).ToString("C");
					this.entry_total_iva.Text = total_de_iva.ToString("C");
					
					subtotales = subtotal_al_0 + subtotal_al_impuesto + total_de_iva;
					this.entry_subtotal.Text = subtotales.ToString("C");
					this.entry_total_factura.Text = subtotales.ToString("C");				
					
					cantidad_en_letras = classpublic.ConvertirCadena(subtotales.ToString("F").Trim(),"Peso");
				}
			}
		}
		
		void on_button_graba_pago_clicked(object sender, EventArgs args)
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
			 				
				comando.CommandText = "UPDATE osiris_erp_factura_enca "+
										"SET pagada = 'true', "+
										"fechahora_pago_factura = '"+this.entry_ano.Text+"-"+this.entry_mes.Text+"-"+this.entry_dia.Text+"',"+
										//"fecha_factura = '"+this.entry_ano.Text+"-"+this.entry_mes.Text+"-"+this.entry_dia.Text+"',"+
										"id_quien_pago = '"+LoginEmpleado+"' "+
										"WHERE numero_factura =  '"+numerodefactura+"';";
				comando.ExecuteNonQuery();
		        comando.Dispose();
		        
		        if (enviofactura_cliente == true){
		        	comando.CommandText = "UPDATE osiris_erp_cobros_enca "+
										"SET pagado = 'true', "+
										"total_pago = total_pago +'"+Convert.ToString(totaldefactura - totalhonorariomedico)+"' "+
										"WHERE numero_factura =  '"+numerodefactura+"';";
					comando.ExecuteNonQuery();
		        	comando.Dispose();
		        	
		        	comando.CommandText = "INSERT INTO osiris_erp_abonos("+
				 							"monto_de_abono_factura,"+
				 							"numero_factura,"+
				 							"concepto_del_abono,"+
				 							"fecha_abono,"+
				 							"id_quien_creo,"+
				 							"id_forma_de_pago,"+
				 							"pago,"+
				 							"fechahora_registro,"+
				 							"pago_factura) "+				 							
				 							"VALUES ('"+
				 								Convert.ToString(totaldefactura - totalhonorariomedico)+"','"+
				 								numerodefactura+"','"+
				 								"PAGO DE FACTURA','"+
				 								DateTime.Now.ToString("yyyy-MM-dd")+"','"+
	 											LoginEmpleado+"','"+
	 											"9','"+
	 											"true','"+
	 											DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
	 											"true');";
	 											
			 		comando.ExecuteNonQuery();    	    	       	comando.Dispose();
		        }
		        conexion.Close ();
				button_pagar_factura.Sensitive = false;
				entry_status_factura.Text = "FAC. P A G A D A";
		        
			}catch (NpgsqlException ex){
			   	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error, 
									ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
		}
				
		void guardar_factura()
		{	
			// Alamacena el detalle de la factura
			string strsql4="";
			TreeIter iter;
 			if (treeViewEngineDetaFact.GetIterFirst (out iter)){
				NpgsqlConnection conexion;
				conexion = new NpgsqlConnection (connectionString+nombrebd );
				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					
					comando.CommandText = "INSERT INTO osiris_erp_factura_enca ("+
										"id_cliente,"+//1
	             						"descripcion_cliente,"+
	                					"direccion_cliente,"+
	                					"colonia_cliente,"+
	                					"municipio_cliente,"+
	                					"estado_cliente,"+
	                					"rfc_cliente,"+
	                					"curp_cliente,"+
	                					"telefono1_cliente,"+
	                					//"telefono2_cliente,"+
	                					"fax_cliente,"+
	                					"mail_cliente,"+
	                					//"contacto_cliente,"+
	                					//"telefono_contacto_cliente,"+
	                					//"dias_credito_cliente,"+
	                					"fechahora_creacion_factura,"+
	                					"id_quien_creo,"+
	                					"numero_factura,"+
	                					"deducible,"+
	                					"coaseguro,"+
	                					"honorario_medico,"+
	                					//"dias_credito_cliente,"+
	                					"cp_cliente,"+
	                					"sub_total_15,"+
	                					"sub_total_0,"+
	                					"iva_al_impuesto,"+
	                					"valor_coaseguro,"+
	                					"fecha_factura) "+
	                					"VALUES ('"+
	                					idcliente+"','"+
	                					entry_nombre_cliente.Text.Trim()+"','"+
	                					entry_direccion_cliente.Text.Trim()+"','"+
	                					entry_colonia_cliente.Text.Trim()+"','"+
	                					entry_municipio_cliente.Text.Trim()+"','"+
	                					entry_estado_cliente.Text.Trim()+"','"+
	                					entry_rfc_cliente.Text.Trim()+"','"+
	                					entry_curp_cliente.Text.Trim()+"','"+
	                					entry_telefono_cliente.Text.Trim()+"','"+
	                					//
	                					entry_fax_cliente.Text.Trim()+"','"+
	                					entry_correo_electronico.Text.Trim()+"','"+
	                					//
	                					//
	                					//
	                					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
	                					LoginEmpleado+"','"+
	                					entry_numero_factura.Text+"','"+
	                					deducible_factura+"','"+
	                					coaseguro_factura+"','"+
	                					total_honorario_medico+"','"+
	                					//dias_de_credito+"','"+
	                					entry_cp_cliente.Text.Trim()+"','"+
	                					subtotal_al_impuesto+"','"+
	                					subtotal_al_0+"','"+
	                					total_de_iva+"','"+
	                					valor_coaseguro+"','"+
	                					entry_fecha_factura.Text+"');";
	                comando.ExecuteNonQuery();				comando.Dispose();
					conexion.Close();
					
					// actualizando el DETALLE DE LA FACTURA
					NpgsqlConnection conexion1;
					conexion1 = new NpgsqlConnection (connectionString+nombrebd );
					// Verifica que la base de datos este conectada
					try{
						conexion1.Open ();
						NpgsqlCommand comando1; 
						comando1 = conexion1.CreateCommand ();
						decimal variable_paso_01 = 0;
						decimal variable_paso_02 = 0;
						decimal variable_paso_03 = 0;
						
						TreeIter iter2;
						if (treeViewEngineDetaFact.GetIterFirst (out iter2)){
							/*
							// validando cantidad detalle
							if ((string)treeview_detalle_de_factura.Model.GetValue (iter2,0) == ""){
								 variable_paso_01 = 0;
							}else{
								variable_paso_01 = decimal.Parse((string)treeview_detalle_de_factura.Model.GetValue (iter2,0)); 
							}
							// validando precio
							if ((string)treeview_detalle_de_factura.Model.GetValue (iter2,2) == ""){
								 variable_paso_02 = 0;
							}else{
								variable_paso_02 = decimal.Parse((string)treeview_detalle_de_factura.Model.GetValue (iter2,2)); 
							}*/
							// validando importe
							if ((string)treeview_detalle_de_factura.Model.GetValue (iter2,3) == ""){
								 variable_paso_03 = 0;
							}else{
								variable_paso_03 = decimal.Parse((string)treeview_detalle_de_factura.Model.GetValue (iter2,3)); 
							}
 							comando1.CommandText = "INSERT INTO osiris_erp_factura_deta( "+
 													"numero_factura,"+
													"cantidad_detalle,"+		
													"descripcion_detalle,"+
													"precio_unitario,"+
													"importe_detalle,"+
													"tipo_detalle )"+
						 							"VALUES ('"+
						 							entry_numero_factura.Text.Trim()+"','"+
						 							0+"','"+
						 							(string)treeview_detalle_de_factura.Model.GetValue (iter2,1)+"','"+
						 							0+"','"+
						 							variable_paso_03+"','"+
						 							(bool) treeview_detalle_de_factura.Model.GetValue (iter2,4)+"');";
 							
 							comando1.ExecuteNonQuery();							comando1.Dispose();
 						
 							while (treeViewEngineDetaFact.IterNext(ref iter2)){
 								/*
 								// validando cantidad detalle
								if ((string)treeview_detalle_de_factura.Model.GetValue (iter2,0) == ""){
									 variable_paso_01 = 0;
								}else{
									variable_paso_01 = decimal.Parse((string)treeview_detalle_de_factura.Model.GetValue (iter2,0)); 
								}
								// validando precio
								if ((string)treeview_detalle_de_factura.Model.GetValue (iter2,2) == ""){
									 variable_paso_02 = 0;
								}else{
									variable_paso_02 = decimal.Parse((string)treeview_detalle_de_factura.Model.GetValue (iter2,2)); 
								}
								*/
								//validando importe
								if ((string)treeview_detalle_de_factura.Model.GetValue (iter2,3) == ""){
									 variable_paso_03 = 0;
								}else{
									variable_paso_03 = decimal.Parse((string)treeview_detalle_de_factura.Model.GetValue (iter2,3)); 
								}
								
	 							comando1.CommandText = "INSERT INTO osiris_erp_factura_deta( "+
	 													"numero_factura,"+
														"cantidad_detalle,"+		
														"descripcion_detalle,"+
														"precio_unitario,"+
														"importe_detalle,"+
														"tipo_detalle )"+
							 							"VALUES ('"+
							 							entry_numero_factura.Text.Trim()+"','"+
							 							0+"','"+
							 							(string)treeview_detalle_de_factura.Model.GetValue (iter2,1)+"','"+
							 							0+"','"+
							 							variable_paso_03+"','"+
							 							(bool) treeview_detalle_de_factura.Model.GetValue (iter2,4)+"');";
 								comando1.ExecuteNonQuery();							comando1.Dispose();
 							} 							
							conexion1.Close();
							
							if(this.checkbutton_detalle_vacio.Active == true){
								MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
								                           MessageType.Info,ButtonsType.Ok,"La Factura "+entry_numero_factura.Text.Trim()+" se creo satisfactoriamente");
								msgBox.Run();		      msgBox.Destroy();
								
								// Desabilitando botones para validaciones
								button_guardar_factura.Sensitive = false;
								checkbutton_nueva_factura.Active = false;
								button_busca_cliente.Sensitive = false;
								button_deducible.Sensitive = false;
								button_imprime_factura.Sensitive = true;
								this.checkbutton_detalle_vacio.Active = false;
								button_selecc_folios.Sensitive = false;
								
								subtotal_al_0 = 0; 
								subtotal_al_impuesto = 0;
								total_de_iva = 0;
								subtotales = 0;																
							}else{							
								// actualizando Tabla encabezado de Cobros
								NpgsqlConnection conexion2;
								conexion2 = new NpgsqlConnection (connectionString+nombrebd );
								// Verifica que la base de datos este conectada
								
								try{
									conexion2.Open ();
									NpgsqlCommand comando2; 
									comando2 = conexion2.CreateCommand ();
									comando2.CommandText = "UPDATE osiris_erp_cobros_enca SET numero_factura = '"+entry_numero_factura.Text.Trim()+"',"+
															"facturacion = 'true' , fecha_facturacion = '"+DateTime.Now.ToString("yyyy-MM-dd")+"' ,"+
															"id_empleado_factura = '"+LoginEmpleado+"',"+
															"id_cliente = '"+this.idcliente.ToString()+"',"+
															"historial_facturados = historial_facturados || '"+entry_numero_factura.Text.Trim()+";"+LoginEmpleado+";"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n' "+
															"WHERE osiris_erp_cobros_enca.folio_de_servicio IN ('"+numeros_folios_seleccionado.Trim()+"') ;";
															
			 						//Console.WriteLine(comando2.CommandText.ToString());
			 						comando2.ExecuteNonQuery(); 						comando2.Dispose();
			 						conexion2.Close();
			 						
									// actualizando Tabla Honorarios Medicos
									NpgsqlConnection conexion3;
									conexion3 = new NpgsqlConnection (connectionString+nombrebd );
									// Verifica que la base de datos este conectada
									try{
										conexion3.Open ();
										NpgsqlCommand comando3; 
										comando3 = conexion3.CreateCommand ();
										comando3.CommandText = "UPDATE osiris_erp_honorarios_medicos SET numero_factura = '"+entry_numero_factura.Text.Trim()+"' "+
																"WHERE osiris_erp_honorarios_medicos.folio_de_servicio IN ('"+numeros_folios_seleccionado.Trim()+"') ;";
				 							
				 						comando3.ExecuteNonQuery(); 						comando3.Dispose();
				 						conexion3.Close();
				 						
							            NpgsqlConnection conexion4; 
										conexion4 = new NpgsqlConnection (connectionString+nombrebd );
										strsql4="";
							
										try{
											conexion4.Open ();
											NpgsqlCommand comando4; 
											comando4 = conexion4.CreateCommand ();
								
											strsql4 =											 
											"UPDATE osiris_erp_cobros_deta SET numero_factura = '"+entry_numero_factura.Text.Trim()+"' "+
											"WHERE osiris_erp_cobros_deta.folio_de_servicio IN ('"+numeros_folios_seleccionado.Trim()+"') ;";				 							
				 							
											comando4.CommandText = strsql4;
											
											comando4.ExecuteNonQuery();					
											comando4.Dispose();
											
										}catch (NpgsqlException ex){
											MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																	MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
											msgBoxError.Run ();			msgBoxError.Destroy();
										}
										conexion4.Close();    
									}catch (NpgsqlException ex){
											MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Info, 
													ButtonsType.Ok,"PostgresSQL error: {0}",ex.Message);
											msgBoxError.Run ();								msgBoxError.Destroy();
									}
									conexion3.Close();
									
									MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
													MessageType.Info,ButtonsType.Ok,"La Factura "+entry_numero_factura.Text.Trim()+" se creo satisfactoriamente");
									msgBox.Run();						msgBox.Destroy();
								}catch (NpgsqlException ex){
										MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Info, 
												ButtonsType.Ok,"PostgresSQL error: {0}"+"guarda factura",ex.Message);
										msgBoxError.Run ();							msgBoxError.Destroy();
								}
								conexion2.Close();
								
								MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,
								                           MessageType.Info,ButtonsType.Ok,"La Factura "+entry_numero_factura.Text.Trim()+" se creo satisfactoriamente");
								msgBox1.Run();		      msgBox1.Destroy();
														
								//  Desabilitando botones para validaciones
								button_guardar_factura.Sensitive = false;
								checkbutton_nueva_factura.Active = false;
								button_selecc_folios.Sensitive = false;
								button_busca_cliente.Sensitive = false;
								button_deducible.Sensitive = false;
								button_imprime_factura.Sensitive = true;
								
								subtotal_al_0 = 0; 
								subtotal_al_impuesto = 0;
								total_de_iva = 0;
								subtotales = 0;														
							}
						}
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Info, 
											ButtonsType.Ok,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();						msgBoxError.Destroy();
					}
					conexion1.Close();
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
											ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				}				
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "LA FACTURA NO TIENE DETALLE");
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}			
		}
		
		void cancelar_factura()
		{
			if ((bool) factura_cancelada == true){
				NpgsqlConnection conexion;
				conexion = new NpgsqlConnection (connectionString+nombrebd );
				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					
					comando.CommandText = "UPDATE osiris_erp_factura_enca SET cancelado = 'true' , fechahora_cancelacion = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
											"id_quien_cancelo = '"+LoginEmpleado+"' "+
											"WHERE numero_factura = '"+numerodefactura+"' ;";
					comando.ExecuteNonQuery();
					comando.Dispose();
			 		conexion.Close();
					//Console.WriteLine(comando.CommandText.ToString());
					
					NpgsqlConnection conexion2;
					conexion2 = new NpgsqlConnection (connectionString+nombrebd );
					// Verifica que la base de datos este conectada
							
					try{
						conexion2.Open ();
						NpgsqlCommand comando2; 
						comando2 = conexion2.CreateCommand ();
						comando2.CommandText = "UPDATE osiris_erp_cobros_enca SET facturacion = 'false' , numero_factura = 0 "+
											"WHERE numero_factura = '"+numerodefactura+"' ;";
						comando2.ExecuteNonQuery();
						
						comando2.Dispose();
			 			conexion2.Close();
			 			
			 			// actualizando Tabla Honorarios Medicos
						NpgsqlConnection conexion3;
						conexion3 = new NpgsqlConnection (connectionString+nombrebd );
						// Verifica que la base de datos este conectada
						try{
							conexion3.Open ();
							NpgsqlCommand comando3; 
							comando3 = conexion3.CreateCommand ();
							comando3.CommandText = "UPDATE osiris_erp_honorarios_medicos SET numero_factura = 0"+
													"WHERE numero_factura = '"+numerodefactura+"'; ";
	 							
	 						comando3.ExecuteNonQuery();
	 						comando3.Dispose();
	 						conexion3.Close();
	 						
	 						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
													MessageType.Info,ButtonsType.Ok,"La Factura "+entry_numero_factura.Text.Trim()+" se CANCELO satisfactoriamente");
							msgBox.Run();
							msgBox.Destroy();
							
							//afectacion a cobros_deta
							NpgsqlConnection conexion4; 
							conexion4 = new NpgsqlConnection (connectionString+nombrebd );
				
							try{
								conexion4.Open ();
								NpgsqlCommand comando4; 
								comando4 = conexion4.CreateCommand ();
							
							
							comando4.CommandText = "UPDATE osiris_erp_cobros_deta SET numero_factura = 0"+
													"WHERE numero_factura = '"+numerodefactura+"'; ";
										
							comando4.ExecuteNonQuery();					
							comando4.Dispose();
										
							}catch (NpgsqlException ex){
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();			msgBoxError.Destroy();
							}
							conexion4.Close();    
	 						//fin afectacion a cobros_deta
						}catch (NpgsqlException ex){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info, 
										ButtonsType.Ok,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();		msgBoxError.Destroy();
						}
						conexion3.Close();										
											
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error, 
												ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();			msgBoxError.Destroy();
					}	
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
											ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				}
			}	
		}
		void on_button_agrega_cliente_clicked(object sender, EventArgs args)
		{
			new osiris.catalogos_generales("cliente1",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_imprime_factura_clicked (object sender, EventArgs args)
		{
			/*
			new osiris.imprime_formato_factura(entry_numero_factura.Text.Trim(),this.treeview_detalle_de_factura,this.entry_nombre_cliente.Text,this.entry_rfc_cliente.Text,this.entry_curp_cliente.Text,
									this.entry_cp_cliente.Text,this.entry_direccion_cliente.Text,this.entry_colonia_cliente.Text,this.entry_municipio_cliente.Text,this.entry_estado_cliente.Text,
									this.entry_telefono_cliente.Text,this.entry_fax_cliente.Text,this.entry_subtotal_15.Text,this.entry_subtotal_0.Text,this.entry_total_iva.Text,this.entry_subtotal.Text,this.entry_deducible_factura.Text,
									this.entry_coaseguro_porcentage.Text,this.entry_coaseguro_factura.Text,this.entry_total_factura.Text,this.cantidad_en_letras,
									this.treeViewEngineDetaFact,this.entry_fecha_factura.Text,LoginEmpleado,error_no_existe);
			*/
		}
		
		void crea_treeview_busqueda(string tipo_busqueda)
		{
			if (tipo_busqueda == "cliente"){
				treeViewEngineBusca3 = new TreeStore(typeof(int),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));
													
				lista_de_cliente.Model = treeViewEngineBusca3;
			
				lista_de_cliente.RulesHint = true;
			
				lista_de_cliente.RowActivated += on_selecciona_cliente_clicked;  // Doble click selecciono paciente*/
				
				TreeViewColumn col_idcliente = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_idcliente.Title = "ID Cliente"; // titulo de la cabecera de la columna, si está visible
				col_idcliente.PackStart(cellr0, true);
				col_idcliente.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
				col_idcliente.SortColumnId = (int) Column_cliente.col_idcliente;
				
				TreeViewColumn col_rfc_cliente = new TreeViewColumn();
				CellRendererText cellr1 = new CellRendererText();
				col_rfc_cliente.Title = "RFC Cliente"; // titulo de la cabecera de la columna, si está visible
				col_rfc_cliente.PackStart(cellr1, true);
				col_rfc_cliente.AddAttribute (cellr1, "text", 1);
				col_rfc_cliente.SortColumnId = (int) Column_cliente.col_rfc_cliente;
				
				TreeViewColumn col_desc_cliente = new TreeViewColumn();
				CellRendererText cellr2 = new CellRendererText();
				col_desc_cliente.Title = "Nombre de Cliente"; // titulo de la cabecera de la columna, si está visible
				col_desc_cliente.PackStart(cellr2, true);
				col_desc_cliente.AddAttribute (cellr2, "text", 2);
				col_desc_cliente.SortColumnId = (int) Column_cliente.col_desc_cliente;
				//cellr0.Editable = true;   // Permite edita este campo
            
				TreeViewColumn col_direc_cliente = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_direc_cliente.Title = "Direccion";
				col_direc_cliente.PackStart(cellrt3, true);
				col_direc_cliente.AddAttribute (cellrt3, "text", 3);
				col_direc_cliente.SortColumnId = (int) Column_cliente.col_direc_cliente;
            
				TreeViewColumn col_colonia_cliente = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_colonia_cliente.Title = "Colonia";
				col_colonia_cliente.PackStart(cellrt4, true);
				col_colonia_cliente.AddAttribute (cellrt4, "text", 4);
				col_colonia_cliente.SortColumnId = (int) Column_cliente.col_colonia_cliente;
				
				TreeViewColumn col_municipio_cliente = new TreeViewColumn();
				CellRendererText cellrt5 = new CellRendererText();
				col_municipio_cliente.Title = "Municipio";
				col_municipio_cliente.PackStart(cellrt5, true);
				col_municipio_cliente.AddAttribute (cellrt5, "text", 5);
				col_municipio_cliente.SortColumnId = (int) Column_cliente.col_municipio_cliente;
            
				TreeViewColumn col_estado_clientes = new TreeViewColumn();
				CellRendererText cellrt6 = new CellRendererText();
				col_estado_clientes.Title = "Estado";
				col_estado_clientes.PackStart(cellrt6, true);
				col_estado_clientes.AddAttribute (cellrt6, "text", 6);
				col_estado_clientes.SortColumnId = (int) Column_cliente.col_estado_clientes;
            
				TreeViewColumn col_tel_cliente = new TreeViewColumn();
				CellRendererText cellrt7 = new CellRendererText();
				col_tel_cliente.Title = "Telefono";
				col_tel_cliente.PackStart(cellrt7, true);
				col_tel_cliente.AddAttribute (cellrt7, "text", 7);
				col_tel_cliente.SortColumnId = (int) Column_cliente.col_tel_cliente;
				
				TreeViewColumn col_cod_postal = new TreeViewColumn();
				CellRendererText cellrt8 = new CellRendererText();
				col_cod_postal.Title = "C. Postal";
				col_cod_postal.PackStart(cellrt8, true);
				col_cod_postal.AddAttribute (cellrt8, "text", 8);
				col_cod_postal.SortColumnId = (int) Column_cliente.col_cod_postal;
      			
				lista_de_cliente.AppendColumn(col_idcliente);  // 0
				lista_de_cliente.AppendColumn(col_rfc_cliente); // 1
				lista_de_cliente.AppendColumn(col_desc_cliente); // 2
				lista_de_cliente.AppendColumn(col_direc_cliente);	//3
				lista_de_cliente.AppendColumn(col_colonia_cliente);	// 4
				lista_de_cliente.AppendColumn(col_municipio_cliente); // 5
				lista_de_cliente.AppendColumn(col_estado_clientes); //6
				lista_de_cliente.AppendColumn(col_tel_cliente); // 7
				lista_de_cliente.AppendColumn(col_cod_postal); // 8
				
			}
			
			if (tipo_busqueda == "procedimientos"){
				// Creacion de Liststore
				treeViewEngineSelProce = new TreeStore(	typeof (string),
												typeof (bool),
												typeof (string),
												typeof (string), 
												typeof (string), 
												typeof (string),
												typeof (string), 
												typeof (string), 
												typeof (string),
												typeof (string),
												typeof (int),
												typeof (string),
												typeof (string));
		        							   
				treeview_selec_procediminetos.Model = treeViewEngineSelProce;
				
				//treeViewEngine.SetSortColumnId (0, Gtk.SortType.Ascending);
				
				TreeViewColumn col_seleccion = new TreeViewColumn();
				CellRendererToggle cellr0 = new CellRendererToggle();
				col_seleccion.Title = "Seleccion"; // titulo de la cabecera de la columna, si está visible
				col_seleccion.PackStart(cellr0, true);
				//col_seleccion.SetCellDataFunc(cellr0, new TreeCellDataFunc (BoolCellDataFunc));  // funcion de columna
				col_seleccion.AddAttribute (cellr0, "active", 1);
				cellr0.Activatable = true;
				col_seleccion.SortColumnId = (int) Column_serv.col_seleccion;
				cellr0.Toggled += selecciona_fila;
				
				CellRendererText cellrt1 = new CellRendererText();  // aplica a todas la columnas
				
				TreeViewColumn col_folio_ingreso = new TreeViewColumn();
				col_folio_ingreso.Title = "Folio Ingreso";
				col_folio_ingreso.PackStart(cellrt1, true);
				col_folio_ingreso.AddAttribute (cellrt1, "text", 0);    // columna 1
				col_folio_ingreso.SortColumnId = (int) Column_serv.col_folio_ingreso;
				
				TreeViewColumn col_paciente = new TreeViewColumn();
				col_paciente.Title = "Nombre del Paciente"; // titulo de la cabecera de la columna, si está visible
				col_paciente.PackStart(cellrt1, true);
				col_paciente.AddAttribute (cellrt1, "text", 2);    // columna 2
				col_paciente.SortColumnId = (int) Column_serv.col_paciente;
				
				TreeViewColumn col_tipo_paciente = new TreeViewColumn();
				col_tipo_paciente.Title = "Tipo de Paciente";
				col_tipo_paciente.PackStart(cellrt1, true);
				col_tipo_paciente.AddAttribute (cellrt1, "text", 3);    // columna 3
				col_tipo_paciente.SortColumnId = (int) Column_serv.col_tipo_paciente;
				
				TreeViewColumn col_empre_asegu = new TreeViewColumn();
				col_empre_asegu.Title = "Nombre Empresa o Aseguradora";
				col_empre_asegu.PackStart(cellrt1, true);
				col_empre_asegu.AddAttribute (cellrt1, "text", 4);    // columna 4
				col_empre_asegu.SortColumnId = (int) Column_serv.col_empre_asegu;
	            
				TreeViewColumn col_valor = new TreeViewColumn();
				col_valor.Title = "Valor";
				col_valor.PackStart(cellrt1, true);
				col_valor.AddAttribute (cellrt1, "text", 5);    // columna 5 
				col_valor.SortColumnId = (int) Column_serv.col_valor;
	            
	            TreeViewColumn col_fecha_ingreso = new TreeViewColumn();
				col_fecha_ingreso.Title = "Fecha de Ingreso";
				col_fecha_ingreso.PackStart(cellrt1, true);
				col_fecha_ingreso.AddAttribute (cellrt1, "text", 6);    // columna 6
				col_fecha_ingreso.SortColumnId = (int) Column_serv.col_fecha_ingreso;
	            
				TreeViewColumn col_fecha_egreso = new TreeViewColumn();
				col_fecha_egreso.Title = "Fecha de Egreso";
				col_fecha_egreso.PackStart(cellrt1, true);
				col_fecha_egreso.AddAttribute (cellrt1, "text", 7);    // columna 7
				col_fecha_egreso.SortColumnId = (int) Column_serv.col_fecha_egreso;
				
				TreeViewColumn col_honorario_medico = new TreeViewColumn();
				col_honorario_medico.Title = "Honorario Medico";
				col_honorario_medico.PackStart(cellrt1, true);
				col_honorario_medico.AddAttribute (cellrt1, "text", 8);    // columna 7
				col_honorario_medico.SortColumnId = (int) Column_serv.col_fecha_egreso;
								            
				TreeViewColumn col_admitio = new TreeViewColumn();
				col_admitio.Title = "Usuario Alta";
				col_admitio.PackStart(cellrt1, true);
				col_admitio.AddAttribute (cellrt1, "text", 9);    // columna 8
				col_admitio.SortColumnId = (int) Column_serv.col_admitio;
				
				treeview_selec_procediminetos.AppendColumn(col_folio_ingreso);
				treeview_selec_procediminetos.AppendColumn(col_seleccion);
				treeview_selec_procediminetos.AppendColumn(col_paciente);
				treeview_selec_procediminetos.AppendColumn(col_tipo_paciente);
				treeview_selec_procediminetos.AppendColumn(col_empre_asegu);
				treeview_selec_procediminetos.AppendColumn(col_valor);				
				treeview_selec_procediminetos.AppendColumn(col_fecha_ingreso);
				treeview_selec_procediminetos.AppendColumn(col_fecha_egreso);
				treeview_selec_procediminetos.AppendColumn(col_honorario_medico);
				treeview_selec_procediminetos.AppendColumn(col_admitio);
			}
		}
		
		enum Column_cliente
		{
			col_idcliente,
			col_rfc_cliente,
			col_desc_cliente,
			col_direc_cliente,
			col_colonia_cliente,
			col_municipio_cliente,
			col_estado_clientes,
			col_tel_cliente,
			col_cod_postal
		}
		
		enum Column_serv
		{
			col_folio_ingreso,
			col_seleccion,
			col_paciente,
			col_tipo_paciente,
			col_empre_asegu,
			col_valor,				
			col_fecha_ingreso,
			col_fecha_egreso,
			col_honorario_medico,
			col_admitio,
			col_id_aseguradora
		}
		
		void on_buscar_cliente_clicked(object sender, EventArgs args)
		{
			llenado_lista_clientes();	
		}
		
		void llenado_lista_clientes()
		{
		
			treeViewEngineBusca3.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	               	
				if (radiobutton_nombre_client.Active == true){
					comando.CommandText = "SELECT id_cliente,descripcion_cliente,direccion_cliente,colonia_cliente,municipio_cliente,"+
											"estado_cliente,rfc_cliente,curp_cliente,telefono1_cliente,telefono2_cliente,fax_cliente,"+
											"mail_cliente,contacto_cliente,telefono_contacto_cliente,dias_credito_cliente,fechahora_creacion_cliente,"+
											"cliente_activo,id_quien_creo,cp_cliente "+
											"FROM osiris_erp_clientes "+
											"WHERE descripcion_cliente LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' AND cliente_activo = 'true';";
				}
				if (radiobutton_rfc_client.Active == true){
					comando.CommandText = "SELECT id_cliente,descripcion_cliente,direccion_cliente,colonia_cliente,municipio_cliente,"+
											"estado_cliente,rfc_cliente,curp_cliente,telefono1_cliente,telefono2_cliente,fax_cliente,"+
											"mail_cliente,contacto_cliente,telefono_contacto_cliente,dias_credito_cliente,fechahora_creacion_cliente,"+
											"cliente_activo,id_quien_creo,cp_cliente "+
											"FROM osiris_erp_clientes "+
											"WHERE descripcion_cliente LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' AND cliente_activo = 'true';";
					
				}
				if (radiobutton_num_client.Active == true){
					comando.CommandText = "SELECT id_cliente,descripcion_cliente,direccion_cliente,colonia_cliente,municipio_cliente,"+
											"estado_cliente,rfc_cliente,curp_cliente,telefono1_cliente,telefono2_cliente,fax_cliente,"+
											"mail_cliente,contacto_cliente,telefono_contacto_cliente,dias_credito_cliente,fechahora_creacion_cliente,"+
											"cliente_activo,id_quien_creo,cp_cliente "+
											"FROM osiris_erp_clientes "+
											"WHERE id_cliente =  '"+entry_expresion.Text.ToUpper().Trim()+"' AND cliente_activo = 'true';";
				}
				if ((string) entry_expresion.Text.ToString() == "*" || (string) entry_expresion.Text.ToString() == ""){
					comando.CommandText = "SELECT id_cliente,descripcion_cliente,direccion_cliente,colonia_cliente,municipio_cliente,"+
											"estado_cliente,rfc_cliente,curp_cliente,telefono1_cliente,telefono2_cliente,fax_cliente,"+
											"mail_cliente,contacto_cliente,telefono_contacto_cliente,dias_credito_cliente,fechahora_creacion_cliente,"+
											"cliente_activo,id_quien_creo,cp_cliente FROM osiris_erp_clientes WHERE cliente_activo = 'true';";
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					//Console.WriteLine((string) lector["rfc_cliente"]);
					treeViewEngineBusca3.AppendValues ((int) lector["id_cliente"],//TreeIter iter = 
										(string) lector["rfc_cliente"],
										(string) lector["descripcion_cliente"],
										(string) lector["direccion_cliente"],
										(string) lector["colonia_cliente"],
										(string) lector["municipio_cliente"],
										(string) lector["estado_cliente"],
										(string) lector["telefono1_cliente"],
										(string) lector["cp_cliente"]);
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
		
		void on_selecciona_cliente_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;

 			if (lista_de_cliente.Selection.GetSelected(out model, out iterSelected)){
 				this.button_selecc_folios.Sensitive = true;
 				this.checkbutton_detalle_vacio.Sensitive = true;
 				this.idcliente = (int) model.GetValue(iterSelected, 0);
 				entry_id_cliente.Text = this.idcliente.ToString(); 
 				entry_nombre_cliente.Text = (string) model.GetValue(iterSelected, 2);
 				entry_rfc_cliente.Text = (string) model.GetValue(iterSelected, 1);
 				entry_direccion_cliente.Text = (string) model.GetValue(iterSelected, 3);
 				entry_colonia_cliente.Text = (string) model.GetValue(iterSelected, 4);
 				entry_municipio_cliente.Text = (string) model.GetValue(iterSelected, 5);
 				entry_estado_cliente.Text = (string) model.GetValue(iterSelected, 6);
 				entry_telefono_cliente.Text = (string) model.GetValue(iterSelected, 7);
 				entry_cp_cliente.Text = (string) model.GetValue(iterSelected, 8);
 				busca_cliente.Destroy(); 				
 			}
		}
		
		void on_button_selecc_folios_clicked(object sender, EventArgs args)
		{

			marca_un_folio = 0; // Limpia variable para marcar los folios
			
			this.entry_subtotal_0.Text = "";
			this.entry_subtotal_15.Text = "";
			this.entry_total_iva.Text = "";
			subtotal_al_0 = 0; 
			subtotal_al_impuesto = 0;
			total_de_iva = 0;
			this.entry_subtotal.Text = "";
			this.entry_total_factura.Text  = "";
			
			//entry_contador_proced.Text = marca_un_folio.ToString(); 
			
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "selecciona_folios_factura", null);
			gxml.Autoconnect (this);
			selecciona_folios_factura.Show();
			
			crea_treeview_busqueda("procedimientos");
			llenado_de_procedimientos_facturar();
			entry_diagnostico_factura.Sensitive = false;
			entry_num_recibos_factura.Sensitive = false;
			entry_infoanexo_1.Sensitive = false;
			entry_infoanexo_2.Sensitive = false;
			entry_infoanexo_3.Sensitive = false;
			entry_infoanexo_4.Sensitive = false;
						
			button_acepta_folios.Clicked += new EventHandler(on_llena_detafactura_clicked);
			checkbutton_info_cirugia.Clicked += new EventHandler(on_checkbutton_info_cirugia_clicked);
			checkbutton_info_compr_caja.Clicked += new EventHandler(on_checkbutton_info_compr_caja_clicked);
			checkbutton_infoanexo_1.Clicked += new EventHandler(on_checkbutton_infoanexo_1_clicked);
			checkbutton_infoanexo_2.Clicked += new EventHandler(on_checkbutton_infoanexo_2_clicked);
			checkbutton_infoanexo_3.Clicked += new EventHandler(on_checkbutton_infoanexo_3_clicked);
			checkbutton_infoanexo_4.Clicked += new EventHandler(on_checkbutton_infoanexo_4_clicked);
			
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
		}
		
		void on_checkbutton_info_cirugia_clicked(object sender, EventArgs args)
		{
			if (checkbutton_info_cirugia.Active == true){
				entry_diagnostico_factura.Sensitive = true;
			}else{
				entry_diagnostico_factura.Sensitive = false;
			}
		}
		
		void on_checkbutton_info_compr_caja_clicked(object sender, EventArgs args)
		{
			if (checkbutton_info_compr_caja.Active == true){
				entry_num_recibos_factura.Sensitive = true;				
			}else{
				entry_num_recibos_factura.Sensitive = false;
			}
		}
		
		void on_checkbutton_infoanexo_1_clicked(object sender, EventArgs args)
		{
			if (checkbutton_infoanexo_1.Active == true){
				entry_infoanexo_1.Sensitive = true;				
			}else{
				entry_infoanexo_1.Sensitive = false;
			}
		}
		
		void on_checkbutton_infoanexo_2_clicked(object sender, EventArgs args)
		{
			if (checkbutton_infoanexo_2.Active == true){
				entry_infoanexo_2.Sensitive = true;				
			}else{
				entry_infoanexo_2.Sensitive = false;
			}
		}
		
		void on_checkbutton_infoanexo_3_clicked(object sender, EventArgs args)
		{
			if (checkbutton_infoanexo_3.Active == true){
				entry_infoanexo_3.Sensitive = true;				
			}else{
				entry_infoanexo_3.Sensitive = false;
			}
		}
		
		void on_checkbutton_infoanexo_4_clicked(object sender, EventArgs args)
		{
			if (checkbutton_infoanexo_4.Active == true){
				entry_infoanexo_4.Sensitive = true;				
			}else{
				entry_infoanexo_4.Sensitive = false;
			}
		}
		
		// Cuando seleccion el treeview de cargos extras para cargar los productos  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			 
			if (treeview_selec_procediminetos.Model.GetIter (out iter, path)) {
				bool old = (bool) treeview_selec_procediminetos.Model.GetValue (iter,1);
				treeview_selec_procediminetos.Model.SetValue(iter,1,!old);
				if (old==false){
					marca_un_folio += 1;
				}else{
					marca_un_folio -= 1;
				}
				//entry_contador_proced.Text = marca_un_folio.ToString();
				if (marca_un_folio > 1){
					checkbutton_info_paciente.Sensitive = false;
					checkbutton_info_ingr_egre.Sensitive = false;
					checkbutton_info_cirugia.Sensitive = false;
					//checkbutton_info_compr_caja.Sensitive = false;
					//checkbutton_poliza_certificado.Sensitive = false;
					total_honorario_medico = 0;
				}else{
					checkbutton_info_paciente.Sensitive = true;
					checkbutton_info_ingr_egre.Sensitive = true;
					checkbutton_info_cirugia.Sensitive = true;
					total_honorario_medico = decimal.Parse((string) treeview_selec_procediminetos.Model.GetValue (iter,8));
				}
			}	
		}
				
		void llenado_de_procedimientos_facturar()
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				 
				comando.CommandText = "SELECT DISTINCT(osiris_erp_movcargos.folio_de_servicio),to_char(osiris_erp_movcargos.folio_de_servicio,'9999999999') AS foliodeatencion, "+
								//"SELECT osiris_erp_movcargos.folio_de_servicio,to_char(osiris_erp_movcargos.folio_de_servicio,'9999999999') AS foliodeatencion, "+ 
								"osiris_erp_cobros_enca.cancelado,"+
								"osiris_erp_cobros_enca.cerrado,"+
								"to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente,"+
				            	"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo,"+
				            	"telefono_particular1_paciente,numero_poliza,folio_de_servicio_dep,osiris_empresas.descripcion_empresa,"+
				            	"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH24:mm') AS fecha_ingreso,"+
				            	"to_char(osiris_erp_cobros_enca.fecha_alta_paciente,'dd-MM-yyyy HH24:mm') AS fecha_egreso,"+
				            	"osiris_erp_movcargos.id_tipo_paciente AS idtipopaciente, "+
				            	"osiris_erp_movcargos.id_tipo_admisiones AS idtipoadmision, "+
            					"descripcion_tipo_paciente,osiris_his_tipo_cirugias.descripcion_cirugia,osiris_his_paciente.id_empresa,"+
            					"descripcion_admisiones,osiris_his_tipo_especialidad.descripcion_especialidad,"+
				            	"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora, osiris_erp_cobros_enca.id_medico,nombre_medico, "+
				            	"osiris_erp_movcargos.descripcion_diagnostico_movcargos,osiris_his_tipo_cirugias.id_tipo_cirugia,nombre_medico_encabezado,"+
				            	"osiris_erp_cobros_enca.facturacion,osiris_erp_cobros_enca.id_empleado_alta_paciente,"+
				            	"to_char(osiris_erp_cobros_enca.honorario_medico,'999999999.99') AS honorariomedico,"+
				            	"to_char(osiris_erp_cobros_enca.total_abonos,'999999999.99') AS totalabonos,"+
				            	"numero_poliza,numero_certificado "+
				            	"FROM "+ 
				            	"osiris_erp_cobros_enca,osiris_his_paciente,osiris_erp_movcargos,osiris_his_tipo_admisiones,osiris_his_tipo_pacientes, "+
				            	"osiris_aseguradoras,osiris_his_medicos,osiris_his_tipo_cirugias,osiris_his_tipo_especialidad,osiris_empresas "+
				            	"WHERE "+
				            	"osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
				            	"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
				            	"AND osiris_erp_cobros_enca.id_medico = osiris_his_medicos.id_medico "+ 
								"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+ 
								"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
								"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
								"AND osiris_erp_movcargos.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
								"AND osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad  "+
								"AND osiris_erp_cobros_enca.id_aseguradora != '17' "+
								"AND osiris_his_paciente.id_empresa = osiris_empresas.id_empresa "+
								"AND osiris_erp_cobros_enca.alta_paciente = 'true' "+ 
								"AND osiris_erp_cobros_enca.cerrado = 'true' "+
								"AND osiris_erp_cobros_enca.cancelado = 'false' "+
								"AND osiris_erp_cobros_enca.facturacion = 'false' "+								
								"AND osiris_erp_movcargos.id_tipo_admisiones != '10' "+
								"ORDER BY osiris_erp_movcargos.folio_de_servicio;";
				
				// Excluye a todo los que es del centro medico
				Console.WriteLine("33   "+comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				string variable_paso_01 = "";
				string variable_paso_02 = "";
				string variable_paso_03 = "";
				while(lector.Read()){
					id_tipopaciente = (int) lector ["idtipopaciente"];
										
					if((int) lector ["id_aseguradora"] > 1){
						variable_paso_01 = (string) lector["descripcion_aseguradora"];
					}else{variable_paso_01 = (string) lector["descripcion_empresa"];}
										
			       	if((int) lector["id_tipo_cirugia"] > 1){
			       		variable_paso_02 = (string) lector["descripcion_cirugia"];
			       	}else{variable_paso_02 = (string) lector ["descripcion_diagnostico_movcargos"];}
			       	
			       	if((string)lector ["fecha_egreso"] == "02-01-2000 00:01"){
			       		variable_paso_03 = " ";  
			       	}else{variable_paso_03 = (string)lector ["fecha_egreso"];}
			       	
					treeViewEngineSelProce.AppendValues((string)lector ["foliodeatencion"],
													false,
													(string)lector ["nombre_completo"],
													(string)lector ["descripcion_tipo_paciente"],
													variable_paso_01,
													"0",
													(string)lector ["fecha_ingreso"],
													variable_paso_03,
													(string)lector ["honorariomedico"],
													"", 
													(int)lector ["id_aseguradora"],
													(string)lector ["numero_poliza"],
													(string)lector ["numero_certificado"]);
													
					//calculo_honario_medico((string) lector ["foliodeatencion"]);
					//(string) lector ["id_empleado_alta_paciente"]);
										
				}
				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
	   			
	       	}
       		conexion.Close ();
		}
		
		void on_llena_detafactura_clicked (object sender, EventArgs args)
		{
			this.checkbutton_detalle_vacio.Sensitive = false;
			string nombre_del_paciente = "";
			string fecha_de_ingreso_pac = "";
			string fecha_de_egreso_pac = "";
			string numeropoliza = "";
			string numerocertificado = "";
			
			// Este proceso se realiza para leer el treeviev y crear un varible que guade los seleccionado
			// para luego realizar el query correspondiente
						
			TreeIter iter;
			numeros_folios_seleccionado = "";    // esta guarda los folios seleccionados
			string variable_paso_03 = "";
			string numeros_seleccionado = ""; 
			int variable_paso_02 = 0; 
 			if (treeViewEngineSelProce.GetIterFirst (out iter)){
 				if ((bool) treeview_selec_procediminetos.Model.GetValue (iter,1) == true){
 					numeros_folios_seleccionado = (string) treeview_selec_procediminetos.Model.GetValue (iter,0);
 					numeros_seleccionado = (string) treeview_selec_procediminetos.Model.GetValue (iter,0);
 					idaseguradora = (int) treeview_selec_procediminetos.Model.GetValue (iter,10); 					
 					variable_paso_02 += 1;
 				}
 				while (treeViewEngineSelProce.IterNext(ref iter)){
 					if ((bool) treeview_selec_procediminetos.Model.GetValue (iter,1) == true){
 				    	if (variable_paso_02 == 0){ 				    	
 							numeros_folios_seleccionado = (string) treeview_selec_procediminetos.Model.GetValue (iter,0);
 							numeros_seleccionado = (string) treeview_selec_procediminetos.Model.GetValue (iter,0);
 							variable_paso_02 += 1;
 						}else{
 							variable_paso_03 = (string) treeview_selec_procediminetos.Model.GetValue (iter,0);
 							numeros_folios_seleccionado = numeros_folios_seleccionado.TrimStart() + "','" + variable_paso_03.TrimStart();
 							numeros_seleccionado = numeros_seleccionado + "-" + variable_paso_03.TrimStart();
 						}
 						idaseguradora = (int) treeview_selec_procediminetos.Model.GetValue (iter,10);
 						nombre_del_paciente = (string) treeview_selec_procediminetos.Model.GetValue (iter,2);
 						fecha_de_ingreso_pac = (string) treeview_selec_procediminetos.Model.GetValue (iter,6);
						fecha_de_egreso_pac = (string) treeview_selec_procediminetos.Model.GetValue (iter,7);
						numeropoliza = (string) treeview_selec_procediminetos.Model.GetValue (iter,11);
						numerocertificado = (string) treeview_selec_procediminetos.Model.GetValue (iter,12);
 					}
 				}
 				//Console.WriteLine(numeros_folios_seleccionado);
 				//Console.WriteLine(numeros_seleccionado);
 			}
 			
 			// llenado del treeview para la factura
 			
 			NpgsqlConnection conexion1; 
			conexion1 = new NpgsqlConnection (connectionString+nombrebd);
			
            treeViewEngineDetaFact.Clear();   // limpia treeview de factura
			// Verifica que la base de datos este conectada
			try{
			
				aplicar_descuento = true;
				aplicar_siempre = true;
				
				conexion1.Open ();
				NpgsqlCommand comando1; 
				comando1 = conexion1.CreateCommand ();
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando1.CommandText = "SELECT osiris_erp_cobros_deta.folio_de_servicio,osiris_erp_cobros_deta.pid_paciente, osiris_his_tipo_admisiones.descripcion_admisiones,"+
									"aplicar_iva, osiris_his_tipo_admisiones.id_tipo_admisiones AS idadmisiones,osiris_grupo_producto.descripcion_grupo_producto,"+
									"osiris_productos.id_grupo_producto,  to_char(osiris_erp_cobros_deta.porcentage_descuento,'999.99') AS porcdesc,"+
									"to_char(osiris_erp_cobros_deta.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,  to_char(osiris_erp_cobros_deta.fechahora_creacion,'HH:mm') AS horacreacion,"+
									"to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,descripcion_producto,"+
									"to_char(osiris_erp_cobros_deta.cantidad_aplicada,'9999.99') AS cantidadaplicada, to_char(osiris_erp_cobros_deta.precio_producto,'999999.99') AS preciounitario," +
									"ltrim(to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99')) AS preciounitarioprod, to_char(osiris_erp_cobros_deta.iva_producto,'999999.99') AS ivaproducto,"+
									//"to_char(osiris_erp_cobros_deta.precio_por_cantidad,'999999.99') AS ppcantidad, "+
									"to_char(osiris_erp_cobros_deta.cantidad_aplicada * osiris_erp_cobros_deta.precio_producto,'99999999.99') AS ppcantidad,"+
									"to_char(osiris_productos.precio_producto_publico,'999999999.99999') AS preciopublico,"+
									"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo, "+
									"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy') AS fecha_ingreso,"+
					            	"to_char(osiris_erp_cobros_enca.fecha_alta_paciente,'dd-MM-yyyy') AS fecha_egreso,"+
					            	"osiris_erp_cobros_enca.numero_poliza,osiris_erp_cobros_enca.numero_certificado "+
									"FROM osiris_erp_cobros_deta,osiris_his_tipo_admisiones,osiris_productos,osiris_grupo_producto,"+
									"osiris_erp_cobros_enca, osiris_his_paciente "+
									"WHERE osiris_erp_cobros_deta.folio_de_servicio IN ('"+numeros_folios_seleccionado+"') "+
									"AND osiris_erp_cobros_deta.id_tipo_admisiones IN ('200','500','600','700','710','810','820','830','920') "+ 
									"AND osiris_erp_cobros_deta.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+ 
									"AND osiris_erp_cobros_deta.pid_paciente = osiris_his_paciente.pid_paciente "+
									"AND osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+ 
									"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+  
									"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
									"AND osiris_erp_cobros_deta.eliminado = 'false' "+
									"ORDER BY osiris_erp_cobros_deta.id_tipo_admisiones ASC, osiris_productos.id_grupo_producto;";
				
				//Console.WriteLine(comando1.CommandText.ToString());					
				NpgsqlDataReader lector1 = comando1.ExecuteReader ();
				
				bool cobrar_1400 = true;
				decimal ivaproducto = 0;
				subtotal_al_impuesto = 0;
				subtotal_al_0 = 0;
				total_de_iva = 0;
				
				subtotal_del_grupo = 0;
				subtotal_al_impuesto_grupo = 0;
				iva_del_grupo = 0;
								
				//Console.WriteLine(lector1.Read());
				if ((bool) lector1.Read() == true)			// AQUI SE LEE LA PRIMERA LINEA PARA DESPUES COMPARAR LAS ADMISIONES
			    {						
					cobrar_1400 = false;
				}
				conexion1.Close ();
								
				if (idaseguradora == 7 && cobrar_1400 == true){
					decimal subtotal_factura = 0;
					decimal total_de_la_factura = 0;
					
					deducible_factura = 200;
											
					subtotal_al_impuesto = 1400;
					subtotal_al_0 = 0;
					total_de_iva = (subtotal_al_impuesto*15)/100;
					subtotal_factura = subtotal_al_impuesto+subtotal_al_0+total_de_iva;
					
					total_honorario_medico = 0;
					
					total_de_la_factura = (subtotal_factura-deducible_factura)+total_honorario_medico;
					
					treeViewEngineDetaFact.AppendValues("","      SERVICIO MEDICO","","");
					treeViewEngineDetaFact.AppendValues("","PAQUETE URGENCIAS","",subtotal_al_impuesto.ToString().PadLeft(10));
										        			        
					entry_subtotal_15.Text = subtotal_al_impuesto.ToString("C").PadLeft(10);
					entry_subtotal_0.Text = subtotal_al_0.ToString("C").PadLeft(10);
					        
					entry_total_iva.Text= total_de_iva.ToString("C").PadLeft(10);
					       			        
					entry_subtotal.Text = subtotal_factura.ToString("C").PadLeft(10);
							
					entry_deducible_factura.Text = deducible_factura.ToString("C").PadLeft(10);
												
					entry_total_factura.Text = total_de_la_factura.ToString("C").PadLeft(10);
					
					cantidad_en_letras = classpublic.ConvertirCadena(total_de_la_factura.ToString("F").Trim(),"Peso");
					//Console.WriteLine(cantidad_en_letras);
					
					treeViewEngineDetaFact.AppendValues("","","","");
					        
					if (checkbutton_honorario_medico.Active == true){
						if (total_honorario_medico > 0 ){
					    	treeViewEngineDetaFact.AppendValues("","HONORARIO MEDICO","",total_honorario_medico.ToString("C").PadLeft(10),true);
					    	treeViewEngineDetaFact.AppendValues("","","","",true);
					    }
					}else{
						total_honorario_medico = 0;
					}			        
					if (checkbutton_info_paciente.Active == true){
						treeViewEngineDetaFact.AppendValues("","PACIENTE:"+nombre_del_paciente+" Nº ATENCION: "+numeros_seleccionado.Trim(),"","",true);
					}
					if (checkbutton_info_ingr_egre.Active == true){
						treeViewEngineDetaFact.AppendValues("","INGRESO :"+fecha_de_ingreso_pac+"  EGRESO:"+fecha_de_egreso_pac,"","",true);
					}			        
					if( checkbutton_info_cirugia.Active == true){
						treeViewEngineDetaFact.AppendValues("","DIAGNOSTICO :"+(string)entry_diagnostico_factura.Text,"","",true);
						diagnostico_factura = (string) entry_diagnostico_factura.Text;
					}
					if(checkbutton_info_compr_caja.Active == true){
						treeViewEngineDetaFact.AppendValues("","ESTA FACTURA AMPARA EL/LOS SIGUIENTES RECIBOS","","",false);
						treeViewEngineDetaFact.AppendValues("",(string) entry_num_recibos_factura.Text,"","");
						folioservicio_factura = (string) entry_num_recibos_factura.Text;	
					}			        
					if (checkbutton_poliza.Active == true){
					   	treeViewEngineDetaFact.AppendValues("","POLIZA :"+numeropoliza,"","",true);
					}
					if(checkbutton_certificado.Active == true){
						treeViewEngineDetaFact.AppendValues("","CERTIFICADO :"+numerocertificado,"","",true);
					}
					if (checkbutton_infoanexo_1.Active == true){
			        	treeViewEngineDetaFact.AppendValues("",entry_infoanexo_1.Text,"","",true);
			        }
			        if (checkbutton_infoanexo_2.Active == true){
			        	treeViewEngineDetaFact.AppendValues("",entry_infoanexo_2.Text,"","",true);					        
			        }
			        if (checkbutton_infoanexo_3.Active == true){
			        	treeViewEngineDetaFact.AppendValues("",entry_infoanexo_3.Text,"","",true);
			        }
			        if (checkbutton_infoanexo_4.Active == true){
			        	treeViewEngineDetaFact.AppendValues("",entry_infoanexo_4.Text,"","",true);
			        }
				}else{
				
		 			// llenado del treeview para la factura
		 			NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);		            
					// Verifica que la base de datos este conectada
					try{
						aplicar_descuento = true;
						aplicar_siempre = true;
						
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
		               	
						// asigna el numero de folio de ingreso de paciente (FOLIO)
						comando.CommandText = "SELECT osiris_erp_cobros_deta.folio_de_servicio,osiris_erp_cobros_deta.pid_paciente, osiris_his_tipo_admisiones.descripcion_admisiones,"+
										"osiris_productos.aplicar_iva, osiris_his_tipo_admisiones.id_tipo_admisiones AS idadmisiones,osiris_grupo_producto.descripcion_grupo_producto,"+
										"osiris_productos.id_grupo_producto,  to_char(osiris_erp_cobros_deta.porcentage_descuento,'999.99') AS porcdesc,"+
										"to_char(osiris_erp_cobros_deta.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,  to_char(osiris_erp_cobros_deta.fechahora_creacion,'HH:mm') AS horacreacion,"+
										"to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,descripcion_producto,"+
										"to_char(osiris_erp_cobros_deta.cantidad_aplicada,'9999.99') AS cantidadaplicada, to_char(osiris_erp_cobros_deta.precio_producto,'999999.99') AS preciounitario," +
										"ltrim(to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99')) AS preciounitarioprod, to_char(osiris_erp_cobros_deta.iva_producto,'999999.99') AS ivaproducto,"+
										//"to_char(osiris_erp_cobros_deta.precio_por_cantidad,'999999.99') AS ppcantidad,"+
										"to_char(osiris_erp_cobros_deta.cantidad_aplicada * osiris_erp_cobros_deta.precio_producto,'99999999.99') AS ppcantidad,"+
										"to_char(osiris_productos.precio_producto_publico,'999999999.99999') AS preciopublico,"+
										"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo, "+
										"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy') AS fecha_ingreso,"+
						            	"to_char(osiris_erp_cobros_enca.fecha_alta_paciente,'dd-MM-yyyy') AS fecha_egreso,"+
						            	"osiris_erp_cobros_enca.numero_poliza,osiris_erp_cobros_enca.numero_certificado "+
										"FROM osiris_erp_cobros_deta,osiris_his_tipo_admisiones,osiris_productos,osiris_grupo_producto,"+
										"osiris_erp_cobros_enca, osiris_his_paciente "+
										"WHERE osiris_erp_cobros_deta.folio_de_servicio IN ('"+numeros_folios_seleccionado+"') "+
										"AND osiris_erp_cobros_deta.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+ 
										"AND osiris_erp_cobros_deta.pid_paciente = osiris_his_paciente.pid_paciente "+
										"AND osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+ 
										"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+  
										"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
										"AND osiris_erp_cobros_deta.eliminado = 'false' "+
										"ORDER BY osiris_erp_cobros_deta.id_tipo_admisiones ASC, osiris_productos.id_grupo_producto;";
						NpgsqlDataReader lector = comando.ExecuteReader ();
						//Console.WriteLine(comando.CommandText.ToString());
						bool verifica_el_iva = false;
						
						subtotal_al_impuesto = 0;
						subtotal_al_impuesto_grupo = 0;
						total_de_iva = 0;
						iva_del_grupo = 0;								
						subtotal_al_0 = 0;
						subtotal_al_0_grupo = 0;
						subtotal_del_grupo = 0;
						descuento_neto = 0;
						iva_de_descuento = 0;
						descuento_del_grupo = 0;
						
						if (lector.Read())			// AQUI SE LEE LA PRIMERA LINEA PARA DESPUES COMPARAR LAS ADMISIONES
				        {
				        	nombre_del_paciente = (string) lector["nombre_completo"];
				        	fecha_de_ingreso_pac = (string) lector["fecha_ingreso"];
							fecha_de_egreso_pac = (string) lector["fecha_egreso"];
							numeropoliza = (string) lector["numero_poliza"];
							numerocertificado = (string) lector["numero_certificado"];
							
							idadmision_ = (int) lector["idadmisiones"];				//obtengo valor de admision para futura comparacion
				        	idgrupoproducto = (int) lector["id_grupo_producto"];	//obtengo valor del grupo para futura comparacion
				        		
				        	if ((int) lector["idadmisiones"] == 100 && (int) id_tipopaciente == 101 
						  		||(int) lector["idadmisiones"] == 300 && (int) id_tipopaciente == 101 
						  		||(int) lector["idadmisiones"] == 400 && (int) id_tipopaciente == 101){
								aplicar_descuento = true;
							}else{
								if (aplicar_siempre == true){
									aplicar_siempre = false;
									aplicar_descuento = false;							
								}
							}
							//dandole valores a las variables
							precio_por_cantidad = decimal.Parse((string) lector["ppcantidad"]);
							ivaproducto = (precio_por_cantidad*15)/100;
							porcentagedesc =  decimal.Parse((string) lector["porcdesc"]);
															
							if(aplicar_descuento == true && aplicar_siempre == true && porcentagedesc > 0){
								descuento_neto += ((precio_por_cantidad*porcentagedesc)/100);
								iva_de_descuento += ((((precio_por_cantidad*porcentagedesc)/100)*15)/100);
								descuento_del_grupo += ((precio_por_cantidad*porcentagedesc)/100)+((((precio_por_cantidad*porcentagedesc)/100)*15)/100);
							}else{
								descuento_neto += 0;
								iva_de_descuento += 0;
								descuento_del_grupo += 0;
							}
							
							if(aplicar_descuento == true && aplicar_siempre == true && porcentagedesc > 0){
								total_de_descuento_neto+= ((precio_por_cantidad*porcentagedesc)/100);
								total_de_iva_descuento += ((((precio_por_cantidad*porcentagedesc)/100)*15)/100);
								total_descuento += ((precio_por_cantidad*porcentagedesc)/100)+((((precio_por_cantidad*porcentagedesc)/100)*15)/100);
							}
							
							verifica_el_iva = (bool) lector["aplicar_iva"];
							if((bool) lector["aplicar_iva"] == true){
								subtotal_al_impuesto += precio_por_cantidad;
								subtotal_al_impuesto_grupo += precio_por_cantidad;
								total_de_iva += ivaproducto;
								iva_del_grupo += ivaproducto;								
							}else{
								subtotal_al_0 += precio_por_cantidad;
								subtotal_al_0_grupo += precio_por_cantidad;
							}
							subtotal_del_grupo =subtotal_al_impuesto_grupo+subtotal_al_0_grupo;
												
							/////DATOS DE PRODUCTOS
			      		  	
			     		   	treeViewEngineDetaFact.AppendValues("","            "+(string) lector["descripcion_admisiones"],"","",false);
			     		   	
			     		   	string descripciongrupo_prod = (string) lector["descripcion_grupo_producto"];
			     		   	        				        	        	        	
				        	while (lector.Read())	// COMIENZA EL CICLO DE LECTURA MDE PRODUCTOS APLICADOS
				        	{	
				        		//treeViewEngineDetaFact.AppendValues("",(string) lector["descripcion_admisiones"],"","",false);
				        		precio_por_cantidad = decimal.Parse((string) lector["ppcantidad"]);
			        			//precio_por_cantidad = decimal.Parse((string) lector["cantidadaplicada"])* decimal.Parse((string) lector["preciopublico"]);
			        			
								ivaproducto = (precio_por_cantidad*15)/100;
								porcentagedesc =  decimal.Parse((string) lector["porcdesc"]);
								
			        			//Verifica si el lugar de procedencia del producto permite aplicar descuento
					        	if ((int) lector["idadmisiones"] == 100 && (int) id_tipopaciente == 101 
								  ||(int) lector["idadmisiones"] == 300 && (int) id_tipopaciente == 101 
								  ||(int) lector["idadmisiones"] == 400 && (int) id_tipopaciente == 101){
									aplicar_descuento = true;
								}else{
									if (aplicar_siempre == true){
										aplicar_siempre = false;
										aplicar_descuento = false;							
									}
								}
			        			    	
			        			//VERIFICACION DE TIPO DE ADMISION
			        			if ((idadmision_ == (int) lector["idadmisiones"]) && (idgrupoproducto == (int) lector["id_grupo_producto"])){
			 						        				
			        				if(aplicar_descuento == true && aplicar_siempre == true && porcentagedesc > 0){
										descuento_neto += ((precio_por_cantidad*porcentagedesc)/100);
										iva_de_descuento += ((((precio_por_cantidad*porcentagedesc)/100)*15)/100);
										descuento_del_grupo += ((precio_por_cantidad*porcentagedesc)/100)+((((precio_por_cantidad*porcentagedesc)/100)*15)/100);
									}else{
										descuento_neto += 0;
										iva_de_descuento += 0;
										descuento_del_grupo += 0;
									}
									
									if(aplicar_descuento == true && aplicar_siempre == true && porcentagedesc > 0){
										total_de_descuento_neto+= ((precio_por_cantidad*porcentagedesc)/100);
										total_de_iva_descuento += ((((precio_por_cantidad*porcentagedesc)/100)*15)/100);
										total_descuento += ((precio_por_cantidad*porcentagedesc)/100)+((((precio_por_cantidad*porcentagedesc)/100)*15)/100);
									}
									verifica_el_iva = (bool) lector["aplicar_iva"];
									if((bool) lector["aplicar_iva"] == true){
										subtotal_al_impuesto += precio_por_cantidad;
										subtotal_al_impuesto_grupo += precio_por_cantidad;
										total_de_iva += ivaproducto;
									}else{
										subtotal_al_0 += precio_por_cantidad;
										subtotal_al_0_grupo += precio_por_cantidad;
										ivaproducto = 0;
									}
									subtotal_del_grupo = subtotal_al_impuesto_grupo+subtotal_al_0_grupo;
									
									iva_del_grupo += ivaproducto;
								}else{
									if ((idadmision_ != (int) lector["idadmisiones"]) ){
										if (idcliente == 1){
											total_del_grupo = subtotal_del_grupo + iva_del_grupo;
											treeViewEngineDetaFact.AppendValues("",descripciongrupo_prod,"",total_del_grupo.ToString("F"),false);
										}else{
											treeViewEngineDetaFact.AppendValues("",descripciongrupo_prod,"",subtotal_del_grupo.ToString(),false);
										}
										//treeViewEngineDetaFact.AppendValues("",descripciongrupo_prod,"",subtotal_del_grupo.ToString(),false);
										
										treeViewEngineDetaFact.AppendValues("","            "+(string) lector["descripcion_admisiones"],"","",false);
										
										descripciongrupo_prod = (string) lector["descripcion_grupo_producto"];
												       		 				       		 			
				       		 			subtotal_al_0_grupo = 0;
										subtotal_al_impuesto_grupo = 0;
										iva_del_grupo = 0;
										
										descuento_neto = 0;
										iva_de_descuento = 0;
										descuento_del_grupo = 0;
										
										if(aplicar_descuento == true && aplicar_siempre == true && porcentagedesc > 0){
											descuento_neto += ((precio_por_cantidad*porcentagedesc)/100);
											iva_de_descuento += ((((precio_por_cantidad*porcentagedesc)/100)*15)/100);
											descuento_del_grupo += ((precio_por_cantidad*porcentagedesc)/100)+((((precio_por_cantidad*porcentagedesc)/100)*15)/100);
										}else{
											descuento_neto += 0;
											iva_de_descuento += 0;
											descuento_del_grupo += 0;
										}
									
										if(aplicar_descuento == true && aplicar_siempre == true && porcentagedesc > 0){
											total_de_descuento_neto+= ((precio_por_cantidad*porcentagedesc)/100);
											total_de_iva_descuento += ((((precio_por_cantidad*porcentagedesc)/100)*15)/100);
											total_descuento += ((precio_por_cantidad*porcentagedesc)/100)+((((precio_por_cantidad*porcentagedesc)/100)*15)/100);
										}
										verifica_el_iva = (bool) lector["aplicar_iva"];
										if((bool) lector["aplicar_iva"]== true){
											subtotal_al_impuesto += precio_por_cantidad;
											subtotal_al_impuesto_grupo += precio_por_cantidad;
											total_de_iva += ivaproducto;
											iva_del_grupo += ivaproducto;
										}else{
											subtotal_al_0 += precio_por_cantidad;
											subtotal_al_0_grupo += precio_por_cantidad; 
										}
										subtotal_del_grupo =subtotal_al_impuesto_grupo+subtotal_al_0_grupo;
										idadmision_ = 0;				//limpio admsiosion para que no entre al otro if
									}
									
									if ((idgrupoproducto != (int) lector["id_grupo_producto"]) && (idadmision_ == (int) lector["idadmisiones"])){
										
										if (idcliente == 1){
											total_del_grupo = subtotal_del_grupo + iva_del_grupo;
											treeViewEngineDetaFact.AppendValues("",descripciongrupo_prod,"",total_del_grupo.ToString("F"),false);
										}else{											
											treeViewEngineDetaFact.AppendValues("",descripciongrupo_prod,"",subtotal_del_grupo.ToString(),false);
										}
										
										descripciongrupo_prod = (string) lector["descripcion_grupo_producto"];
										
										total_del_grupo = subtotal_del_grupo + iva_del_grupo;
												       		 				       		 			
				       		 			subtotal_al_0_grupo = 0;
										subtotal_al_impuesto_grupo = 0;
										iva_del_grupo = 0;
										
										descuento_neto = 0;
										iva_de_descuento = 0;
										descuento_del_grupo = 0;
										
										if(aplicar_descuento == true && aplicar_siempre == true && porcentagedesc > 0){
											descuento_neto += ((precio_por_cantidad*porcentagedesc)/100);
											iva_de_descuento += ((((precio_por_cantidad*porcentagedesc)/100)*15)/100);
											descuento_del_grupo += ((precio_por_cantidad*porcentagedesc)/100)+((((precio_por_cantidad*porcentagedesc)/100)*15)/100);
										}else{
											descuento_neto += 0;
											iva_de_descuento += 0;
											descuento_del_grupo += 0;
										}
									
										if(aplicar_descuento == true && aplicar_siempre == true && porcentagedesc > 0){
											total_de_descuento_neto+= ((precio_por_cantidad*porcentagedesc)/100);
											total_de_iva_descuento += ((((precio_por_cantidad*porcentagedesc)/100)*15)/100);
											total_descuento += ((precio_por_cantidad*porcentagedesc)/100)+((((precio_por_cantidad*porcentagedesc)/100)*15)/100);
										}
										verifica_el_iva = (bool) lector["aplicar_iva"];
										if((bool) lector["aplicar_iva"]== true){
											subtotal_al_impuesto += precio_por_cantidad;
											subtotal_al_impuesto_grupo += precio_por_cantidad;
											total_de_iva += ivaproducto;
											iva_del_grupo += ivaproducto;
											
										}else{
											subtotal_al_0 += precio_por_cantidad;
											subtotal_al_0_grupo += precio_por_cantidad;
										}
										subtotal_del_grupo = subtotal_al_impuesto_grupo+subtotal_al_0_grupo;
									}
									idadmision_ = (int) lector["idadmisiones"];				//obtengo valor de admision para futura comparacion
									idgrupoproducto = (int) lector["id_grupo_producto"];		//obtengo valor del grupo para futura comparacion
								}
					        }
					        if (idcliente == 1){
								total_del_grupo = subtotal_del_grupo + iva_del_grupo;
								treeViewEngineDetaFact.AppendValues("",descripciongrupo_prod,"",total_del_grupo.ToString("F"),false);
							}else{
								treeViewEngineDetaFact.AppendValues("",descripciongrupo_prod,"",subtotal_del_grupo.ToString(),false);
							}
					        //treeViewEngineDetaFact.AppendValues("",descripciongrupo_prod,"",subtotal_del_grupo.ToString(),false);
							treeViewEngineDetaFact.AppendValues("","","","",false);
					        
					        if (checkbutton_honorario_medico.Active == true){
					        	if (total_honorario_medico > 0 ){
					        		treeViewEngineDetaFact.AppendValues("","HONORARIOS MEDICOS","",total_honorario_medico.ToString(),true);
					        		treeViewEngineDetaFact.AppendValues("","","","",true);
					        	}
					        }else{
								total_honorario_medico = 0;
							}
										        
					        if (checkbutton_info_paciente.Active == true){
								treeViewEngineDetaFact.AppendValues("","PACIENTE:"+nombre_del_paciente+" Nº ATENCION: "+numeros_seleccionado.Trim(),"","",true);
					        }
					        if (checkbutton_info_ingr_egre.Active == true){
								treeViewEngineDetaFact.AppendValues("","INGRESO :"+fecha_de_ingreso_pac+"  EGRESO:"+fecha_de_egreso_pac,"","",true);
					        }			        
					        if( checkbutton_info_cirugia.Active == true){
								treeViewEngineDetaFact.AppendValues("","DIAGNOSTICO :"+(string)entry_diagnostico_factura.Text,"","",true);
								diagnostico_factura = (string) entry_diagnostico_factura.Text;
					        }
					        if(checkbutton_info_compr_caja.Active == true){
								treeViewEngineDetaFact.AppendValues("","ESTA FACTURA AMPARA EL/LOS SIGUIENTES RECIBOS","","",true);
								treeViewEngineDetaFact.AppendValues("",(string) entry_num_recibos_factura.Text,"","",true);
								folioservicio_factura = (string) entry_num_recibos_factura.Text;	
					        }			        
					        if (checkbutton_poliza.Active == true){
					        	treeViewEngineDetaFact.AppendValues("","POLIZA :"+numeropoliza,"","",true);
							}
							if(checkbutton_certificado.Active == true){
								treeViewEngineDetaFact.AppendValues("","CERTIFICADO :"+numerocertificado,"","",true);
					        }
					        if (checkbutton_infoanexo_1.Active == true){
					        	treeViewEngineDetaFact.AppendValues("",entry_infoanexo_1.Text,"","",true);
					        }
					        if (checkbutton_infoanexo_2.Active == true){
					        	treeViewEngineDetaFact.AppendValues("",entry_infoanexo_2.Text,"","",true);					        
					        }
					        if (checkbutton_infoanexo_3.Active == true){
					        	treeViewEngineDetaFact.AppendValues("",entry_infoanexo_3.Text,"","",true);
					        }
					        if (checkbutton_infoanexo_4.Active == true){
					        	treeViewEngineDetaFact.AppendValues("",entry_infoanexo_4.Text,"","",true);
					        }
					        
					        if (idcliente == 1){
					        	decimal subtotal_factura = subtotal_al_impuesto+subtotal_al_0+total_de_iva+total_honorario_medico;
					        	decimal subtotal_al_quince = subtotal_al_impuesto+total_de_iva;
						        			        
						        entry_subtotal_15.Text = subtotal_al_quince.ToString("C").PadLeft(10);
						        entry_subtotal_0.Text = subtotal_al_0.ToString("C").PadLeft(10);
						        
						        entry_total_iva.Text = "0";
						       			        
								entry_subtotal.Text = subtotal_factura.ToString("C").PadLeft(10);
								
								entry_deducible_factura.Text = deducible_factura.ToString("C").PadLeft(10);
								
								entry_coaseguro_porcentage.Text = this.coaseguro_factura.ToString("C").PadLeft(10);
	 							
	 							entry_coaseguro_factura.Text = this.valor_coaseguro.ToString("C").PadLeft(10);
	 		
								decimal total_de_la_factura = (subtotal_factura-deducible_factura);
								
								entry_total_factura.Text = total_de_la_factura.ToString("C").PadLeft(10);
								
								cantidad_en_letras = classpublic.ConvertirCadena(total_de_la_factura.ToString("F").Trim(),"Peso");
								//Console.WriteLine(cantidad_en_letras);
					        
					        }else{
						        decimal subtotal_factura = subtotal_al_impuesto+subtotal_al_0+total_de_iva+total_honorario_medico;
						        			        
						        entry_subtotal_15.Text = subtotal_al_impuesto.ToString("C").PadLeft(10);
						        entry_subtotal_0.Text = subtotal_al_0.ToString("C").PadLeft(10);
						        
						        entry_total_iva.Text= total_de_iva.ToString("C").PadLeft(10);
						       			        
								entry_subtotal.Text = subtotal_factura.ToString("C").PadLeft(10);
								
								entry_deducible_factura.Text = deducible_factura.ToString("C").PadLeft(10);
								
								entry_coaseguro_porcentage.Text = this.coaseguro_factura.ToString("C").PadLeft(10);
	 							
	 							entry_coaseguro_factura.Text = this.valor_coaseguro.ToString("C").PadLeft(10);
	 		
								decimal total_de_la_factura = (subtotal_factura-deducible_factura);
								
								entry_total_factura.Text = total_de_la_factura.ToString("C").PadLeft(10);
								
								cantidad_en_letras = classpublic.ConvertirCadena(total_de_la_factura.ToString("F").Trim(),"Peso");
								//Console.WriteLine(cantidad_en_letras);
							}
							conexion.Close ();
						}
		        	}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
								msgBoxError.Destroy();
					}
 				}
 			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
			} 	
			// Destruyo la ventana al final del proceso 									
			selecciona_folios_factura.Destroy();
		}
		
		void crea_treeview_facturador()
		{
			// Creacion de Liststore
			treeViewEngineDetaFact = new TreeStore(	typeof (string),
													typeof (string),
													typeof (string),
													typeof (string),
													typeof (bool));
		        							   
			treeview_detalle_de_factura.Model = treeViewEngineDetaFact;
			
			CellRendererText cellrt1 = new CellRendererText();  // aplica a todas la columnas
				
			TreeViewColumn col_cantidad = new TreeViewColumn();
			col_cantidad.Title = "Cantidad";
			col_cantidad.PackStart(cellrt1, true);
			col_cantidad.AddAttribute (cellrt1, "text", 0);    // columna 0
			
			TreeViewColumn col_descripcion = new TreeViewColumn();
			col_descripcion.Title = "Descripcion";
			col_descripcion.PackStart(cellrt1, true);
			col_descripcion.AddAttribute (cellrt1, "text", 1);    // columna 1
			
			TreeViewColumn col_precio_unitario = new TreeViewColumn();
			col_precio_unitario.Title = "P. Unitario";
			col_precio_unitario.PackStart(cellrt1, true);
			col_precio_unitario.AddAttribute (cellrt1, "text", 2);    // columna 1
			
			TreeViewColumn col_importe = new TreeViewColumn();
			col_importe.Title = "Importe";
			col_importe.PackStart(cellrt1, true);
			col_importe.AddAttribute (cellrt1, "text", 3);    // columna 1

			treeview_detalle_de_factura.AppendColumn(col_cantidad);
			treeview_detalle_de_factura.AppendColumn(col_descripcion);
			treeview_detalle_de_factura.AppendColumn(col_precio_unitario);
			treeview_detalle_de_factura.AppendColumn(col_importe);
			
		}
		
		// Valida entradas que solo sean numericas, se utiliza en ventana
		// Principal cuando selecciona el folio de productos
		// Ademas controla la tecla ENTRER para ver el procedimiento
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				llenado_lista_clientes();			
			}
		}
		
		// Ademas controla la tecla ENTRER para ver el procedimiento
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_factura(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				llenado_de_factura();				
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
			{
				args.RetVal = true;
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
			{
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
	

	/// ///////////////////////////////////////////////////////////////////////////// //// 
	/// /////////////////////////// NOTA DE CREDITO ///////////////////////////////// ////
	/// ///////////////////////////////////////////////////////////////////////////// ////
	
	
	public class nota_de_credito
	{
		//nota_de_credito(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
	
		[Widget] Gtk.Window nota_credito;
		[Widget] Gtk.Entry entry_cliente;
		[Widget] Gtk.Entry entry_nota_credito;
		[Widget] Gtk.Entry entry_fecha;
		[Widget] Gtk.Entry entry_factura;	
		[Widget] Gtk.Entry entry_pesos;
		[Widget] Gtk.Entry entry_porciento;
		[Widget] Gtk.Entry entry_sub_15;
		[Widget] Gtk.Entry entry_sub_0;
		[Widget] Gtk.Entry entry_iva_15;
		[Widget] Gtk.Entry entry_sub_total;
		[Widget] Gtk.Entry entry_deducible;
		[Widget] Gtk.Entry entry_coaseguro;
		[Widget] Gtk.Entry entry_total;
		
		[Widget] Gtk.Entry entry_descripcion1;
		[Widget] Gtk.Entry entry_descripcion2;
		[Widget] Gtk.CheckButton checkbutton_descripcion;
		
		[Widget] Gtk.Statusbar statusbar_nota_credito;
		[Widget] Gtk.CheckButton checkbutton_descuento;
		[Widget] Gtk.RadioButton radiobutton_directo;
		[Widget] Gtk.RadioButton radiobutton_porcentage;
		
		[Widget] Gtk.Button button_cancelar;
		[Widget] Gtk.Button button_pagar;
		[Widget] Gtk.Button button_limpiar;
		[Widget] Gtk.Button button_calcular;
		[Widget] Gtk.Button	button_guardar;
		[Widget] Gtk.Button button_imprimir;
		[Widget] Gtk.Button button_salir;
		
		//[Widget] Gtk.TextView TextView_1;
		
		string toma_descrip_municipio = "";
		int num_nota = 0;
		int id_cliente = 0;
		int ultimafactura = 0;
		string numerodefactura;
		decimal calculo = 0;
		string descuento_cliente = "";
		
		//Variables Para utilizar la suma de total en nota de credito
		decimal sub_15 = 0;
		decimal sub_0 = 0;
		decimal tot_iva = 0;
		double valoriva;
					
		decimal subtotal_15;
		decimal subtotal;
		decimal subtotal_0;
		decimal total_de_iva;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
			
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		//public Gnome.Font fuente5 = Gnome.Font.FindClosest("Luxi Sans", 5);
		//public Gnome.Font fuente6 = Gnome.Font.FindClosest("Luxi Sans", 6);
		//public Gnome.Font fuente7 = Gnome.Font.FindClosest("Luxi Sans", 7);
		//public Gnome.Font fuente8 = Gnome.Font.FindClosest("Luxi Sans", 8);//Bitstream Vera Sans
		//public Gnome.Font fuente9 = Gnome.Font.FindClosest("Luxi Sans", 9);
		//public Gnome.Font fuente10 = Gnome.Font.FindClosest("Luxi Sans", 10);
		//public Gnome.Font fuente11 = Gnome.Font.FindClosest("Luxi Sans", 11);
		//public Gnome.Font fuente12 = Gnome.Font.FindClosest("Luxi Sans", 12);
		
		string connectionString;		
		string nombrebd;
		
		//Declaracion de ventana de error y pregunta
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;

		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public nota_de_credito(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_, 
		                       string numerodefactura_, int id_cliente_, decimal subtotal_al_0_, decimal subtotal_al_impuesto_, decimal total_de_iva_,
		                       decimal subtotales_, int num_nota_)
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			numerodefactura = numerodefactura_;
			id_cliente = id_cliente_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			valoriva = double.Parse(classpublic.ivaparaaplicar)/10;
			
			subtotal_15 = subtotal_al_impuesto_;
			subtotal = subtotales_;
			subtotal_0 = subtotal_al_0_;
			total_de_iva = total_de_iva_;
		    num_nota = num_nota_;
			
			//Console.WriteLine(id_cliente);
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "nota_credito", null);
			gxml.Autoconnect (this);        
			nota_credito.Show();
			
			button_pagar.Hide();
			entry_cliente.Sensitive = false;
			entry_nota_credito.Sensitive = true;
			entry_fecha.Sensitive = false;
			entry_factura.Sensitive = false;
			this.button_guardar.Sensitive = false;
			this.entry_descripcion1.Sensitive = false;
			this.entry_descripcion2.Sensitive = false;
			
			this.entry_porciento.Sensitive = false;
			this.radiobutton_porcentage.Clicked += new EventHandler(on_radiobutton_porcentage_producto);
			this.radiobutton_directo.Clicked += new EventHandler(on_radiobutton_directo);
			checkbutton_descripcion.Clicked += new EventHandler(on_checkbutton_descripcion);
			this.checkbutton_descuento.Clicked += new EventHandler(on_checkbutton_decuento_cliente);
			
			//valida numeros
			this.entry_pesos.KeyPressEvent += onKeyPressEvent_enter_valida_numeros;
			this.entry_porciento.KeyPressEvent += onKeyPressEvent_enter_valida_numeros;
			
			this.button_guardar.Clicked += new EventHandler(on_guarda_clicked);
			this.button_calcular.Clicked += new EventHandler(on_calcula_nota_credito_clicked);
			button_limpiar.Clicked += new EventHandler(on_limpiar_clicked);
			this.button_cancelar.Clicked += new EventHandler(on_cancelar_nota_clicked);
			button_imprimir.Clicked += new EventHandler(on_button_imprimir_clicked);
		
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
		
			statusbar_nota_credito.Pop(0);
			statusbar_nota_credito.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_nota_credito.HasResizeGrip = false;		
			
			if(this.num_nota > 0)
			{
				entry_pesos.Sensitive = false;
				entry_porciento.Sensitive = false;
				button_calcular.Sensitive = false;
				button_limpiar.Sensitive = false;	
				button_guardar.Sensitive = false;	
				this.checkbutton_descripcion.Sensitive = false;	
				this.radiobutton_directo.Sensitive = false;	
				this.radiobutton_porcentage.Sensitive = false;	
				entry_nota_credito.Sensitive = false;	
					
				
				NpgsqlConnection conexion;
				conexion = new NpgsqlConnection (connectionString+nombrebd );
				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
			
					// asigna el numero de paciente (PID)
					comando.CommandText = "SELECT to_char(osiris_erp_notacredito_enca.numero_ntacred,'999999999') AS numeronota,"+
						"to_char(osiris_erp_notacredito_enca.numero_factura,'999999999') AS numfact,"+
				        "to_char(osiris_erp_notacredito_enca.sub_total_15,'999999999.99') AS sub15,"+
                        "to_char(osiris_erp_notacredito_enca.sub_total_0,'999999999.99') AS sub0,"+
                        "to_char(osiris_erp_notacredito_enca.iva_al_impuesto,'999999999.99') AS iva15,"+
						"to_char(osiris_erp_notacredito_enca.total,'999999999.99') AS total_,"+
					    "descripcion1, descripcion2,"+
					    "to_char(osiris_erp_notacredito_enca.sub_total_15 + osiris_erp_notacredito_enca.sub_total_0 + osiris_erp_notacredito_enca.iva_al_impuesto,'99999999.99') AS subtotal "+
						"FROM osiris_erp_notacredito_enca "+
	                    "WHERE osiris_erp_notacredito_enca.numero_factura = '"+numerodefactura+"' "+
                        "AND osiris_erp_notacredito_enca.cancelado = false ";

					Console.WriteLine(comando.CommandText.ToString());
	                NpgsqlDataReader lector = comando.ExecuteReader ();

					if(lector.Read())
					{
						this.entry_factura.Text = (string) lector["numfact"];
						this.entry_factura.Text = this.entry_factura.Text.Trim(); 
						
						this.entry_nota_credito.Text = (string) lector["numeronota"];
						this.entry_nota_credito.Text = this.entry_nota_credito.Text.Trim(); 
						
						this.entry_sub_15.Text = (string) lector["sub15"];
						this.entry_sub_15.Text = this.entry_sub_15.Text.Trim(); 
						
						this.entry_iva_15.Text = (string) lector["iva15"];
						this.entry_iva_15.Text = this.entry_iva_15.Text.Trim(); 
						
						this.entry_sub_0.Text = (string) lector["sub0"];
						this.entry_sub_0.Text = this.entry_sub_0.Text.Trim(); 
						
						this.entry_sub_total.Text = (string) lector["subtotal"];
						this.entry_sub_total.Text = this.entry_sub_total.Text.Trim(); 
						
						this.entry_total.Text = (string) lector["total_"];
						this.entry_total.Text = this.entry_total.Text.Trim(); 
						if((string) lector["descripcion1"] == ""){
						}else{						
							this.entry_descripcion1.Text = (string) lector["descripcion1"];						
						}
						if((string) lector["descripcion2"] == ""){
						}else{	
							this.entry_descripcion2.Text = (string) lector["descripcion2"];

						}
					}				
					lector.Close ();
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					                                               MessageType.Error, 
					                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();						msgBoxError.Destroy();
				}			
				conexion.Close ();
				
				
			}else{
				// Genera el numero nota credito
				NpgsqlConnection conexion;
				conexion = new NpgsqlConnection (connectionString+nombrebd );
				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
			
					// asigna el numero de paciente (PID)
					comando.CommandText = "SELECT numero_ntacred "+				
						"FROM osiris_erp_notacredito_enca "+
	                    "ORDER BY numero_ntacred DESC LIMIT 1;";

	                NpgsqlDataReader lector = comando.ExecuteReader ();

					if ((bool) lector.Read()){
						ultimafactura = (int) lector["numero_ntacred"] + 1;
					}else{		
						ultimafactura = 1;
					}
					
					entry_nota_credito.Text = ultimafactura.ToString();
					this.entry_factura.Text = this.numerodefactura;
				
					
					lector.Close ();
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					                                               MessageType.Error, 
					                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();						msgBoxError.Destroy();
				}			
				conexion.Close ();
			}
				
			NpgsqlConnection conexion1;
			conexion1 = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion1.Open ();
				NpgsqlCommand comando1; 
				comando1 = conexion1.CreateCommand ();
				
				// asigna el numero de paciente (PID)
				comando1.CommandText = "SELECT "+
					"osiris_erp_clientes.id_cliente,"+
					"osiris_erp_clientes.descripcion_cliente "+
                    "FROM osiris_erp_clientes "+
                    "WHERE osiris_erp_clientes.id_cliente = '"+id_cliente+"' ";
                    
				//Console.WriteLine("query"+comando1.CommandText);
				NpgsqlDataReader lector = comando1.ExecuteReader ();

				if(lector.Read())
				{
					this.entry_cliente.Text = (string) lector["descripcion_cliente"];
					this.entry_fecha.Text = DateTime.Now.ToString("yyyy-MM-dd");

				}
			
				
				lector.Close ();
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Error, 
				                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();						msgBoxError.Destroy();
			}			
			conexion1.Close ();
	
		}
		
		void on_button_imprimir_clicked(object sender, EventArgs args)
		{
			
		}
		
		/*

			Gnome.PrintJob    trabajo   = new Gnome.PrintJob(Gnome.PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog(trabajo, "Nota de Credito", 0);
        	
        	int         respuesta = dialogo.Run ();
        	
        	if (respuesta == (int) PrintButtons.Cancel) 
			{
				dialogo.Hide (); 
				dialogo.Dispose (); 
				return;
			}
			Gnome.PrintContext ctx = trabajo.Context;
        	ComponerPagina(ctx, trabajo); 
			trabajo.Close();
            switch (respuesta)
        	{
				case (int) PrintButtons.Print:   
                trabajo.Print (); 
                break;
                case (int) PrintButtons.Preview:
                new PrintJobPreview(trabajo, "Nota de Credito").Show();
                break;
        	}
			dialogo.Hide (); dialogo.Dispose (); 
			
		}
			
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{   
		
			NpgsqlConnection conexion; 
        	conexion = new NpgsqlConnection (connectionString+nombrebd);
        	// Verifica que la base de dato s este conectada
        	try{
				conexion.Open ();
				NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand ();
	        	        	  
	           	comando.CommandText ="SELECT to_char(osiris_erp_notacredito_enca.numero_factura,'99999999') AS numfact,"+
							"to_char(osiris_erp_notacredito_enca.numero_ntacred,'99999999') AS numnota,"+
							"to_char(osiris_erp_notacredito_enca.sub_total_15,'99999999.99') AS subtotal15,"+
						    "to_char(osiris_erp_notacredito_enca.sub_total_0,'99999999.99') AS subtotal0,"+
							"to_char(osiris_erp_notacredito_enca.iva_al_impuesto,'99999999.99') AS iva15,"+
						    "to_char(osiris_erp_notacredito_enca.total,'99999999.99') AS total_,"+
						    "to_char(osiris_erp_notacredito_enca.fecha_creacion_nota_credito,'dd-MM-yyyy') AS fechcreacion,"+ 	
					        "to_char(osiris_erp_notacredito_enca.sub_total_15 + osiris_erp_notacredito_enca.sub_total_0 + osiris_erp_notacredito_enca.iva_al_impuesto,'99999999.99') AS subtotal,"+
						    "osiris_erp_factura_enca.descripcion_cliente,"+
						    "to_char(osiris_his_paciente.pid_paciente,'99999') AS pid,"+
						    "osiris_erp_notacredito_enca.id_quien_creo,"+
						    "descripcion2,descripcion1,"+
						    "nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo, "+
						    "osiris_erp_factura_enca.municipio_cliente "+
							"FROM osiris_erp_notacredito_enca,osiris_erp_factura_enca,osiris_his_paciente,osiris_erp_cobros_enca "+
							"WHERE osiris_erp_factura_enca.numero_factura = '"+(string) this.entry_factura.Text+"' "+
                            "AND osiris_erp_notacredito_enca.numero_ntacred = '"+(string) this.entry_nota_credito.Text+"' "+
							"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
						    "AND osiris_erp_cobros_enca.numero_factura = osiris_erp_factura_enca.numero_factura "+
                            "AND osiris_erp_notacredito_enca.numero_factura = '"+(string) this.entry_factura.Text+"' ;";
						Console.WriteLine("esteeeeee......"+comando.CommandText);
	        	NpgsqlDataReader lector = comando.ExecuteReader ();
				ContextoImp.BeginPage("Pagina 1");
				if (lector.Read()){
		        		    
					string traduce = "";
					traduce = traduce_numeros((string) lector["total_"]);
										Gnome.Print.Setfont (ContextoImp, fuente9);
					//ContextoImp.MoveTo(505, 755);		 ContextoImp.Show((string) lector["numnota"]);
					ContextoImp.MoveTo(500, 705);		 ContextoImp.Show((string) lector["fechcreacion"]);
					ContextoImp.MoveTo(500, 670);		 ContextoImp.Show((string) lector["numfact"]);


					
					toma_descrip_municipio = (string) lector["municipio_cliente"];
						if(toma_descrip_municipio.Length > 14){
							toma_descrip_municipio = toma_descrip_municipio.Substring(0,14);
						}  	
						ContextoImp.MoveTo(485, 635);		ContextoImp.Show(toma_descrip_municipio);
					
					
			
					ContextoImp.MoveTo(490, 460);		 ContextoImp.Show((string) lector["subtotal15"]);
					ContextoImp.MoveTo(490, 445);		 ContextoImp.Show((string) lector["subtotal0"]);
					ContextoImp.MoveTo(490, 430);		 ContextoImp.Show((string) lector["iva15"]);
					ContextoImp.MoveTo(490, 392);		 ContextoImp.Show((string) lector["subtotal"]);
					ContextoImp.MoveTo(490, 382);		 ContextoImp.Show((string) lector["total_"]);
					
					ContextoImp.MoveTo(90, 555);		 ContextoImp.Show((string) lector["descripcion2"]);
					ContextoImp.MoveTo(90, 545);		 ContextoImp.Show((string) lector["descripcion1"]);
					
					Gnome.Print.Setfont (ContextoImp, fuente10);
					ContextoImp.MoveTo(90, 610);		 ContextoImp.Show("PID.-  ");
					ContextoImp.MoveTo(120, 610);		 ContextoImp.Show((string) lector["pid"]);
					ContextoImp.MoveTo(90, 595);		 ContextoImp.Show("PACIENTE.-");						
					ContextoImp.MoveTo(150, 595);		 ContextoImp.Show((string) lector["nombre_completo"]);
					
					ContextoImp.MoveTo(70, 665);		 ContextoImp.Show((string) lector["descripcion_cliente"]);
								
					Gnome.Print.Setfont (ContextoImp, fuente12);
					ContextoImp.MoveTo(95, 465 );		 ContextoImp.Show("("+traduce.ToUpper()+")");	
					
					Gnome.Print.Setfont (ContextoImp, fuente6);	
					ContextoImp.MoveTo(400, 340);		 ContextoImp.Show("Creada");					
					ContextoImp.MoveTo(440, 340);		 ContextoImp.Show((string) lector["id_quien_creo"]);
					ContextoImp.MoveTo(400, 330);		 ContextoImp.Show("Nota");
					ContextoImp.MoveTo(450, 330);		 ContextoImp.Show((string) lector["numnota"]);
				}

				
				ContextoImp.ShowPage();
				}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				                                               msgBoxError.Destroy();
			}	
				
		}
		*/
		
		void on_checkbutton_descripcion(object sender, EventArgs args)
		{
			if(checkbutton_descripcion.Active == true){				
				this.entry_descripcion1.Sensitive = true;
				this.entry_descripcion2.Sensitive = true;
			}else{
				this.entry_descripcion1.Sensitive = false;
				this.entry_descripcion2.Sensitive = false;
			}
		}
			
		void on_radiobutton_porcentage_producto(object sender, EventArgs args)
		{
			if(this.radiobutton_porcentage.Active == true){				
				this.entry_porciento.Sensitive = true;
			}else{
				this.entry_porciento.Sensitive = false;
			}
		}
		
		void on_radiobutton_directo(object sender, EventArgs args)
		{
			if(this.radiobutton_directo.Active == true){
				this.entry_pesos.Sensitive = true;
			}else{
				this.entry_pesos.Sensitive = false;
			}
		}
		
		void on_checkbutton_decuento_cliente(object sender, EventArgs args)
		{
			if(this.checkbutton_descuento.Active == true){
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				// Verifica que la base de dato s este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
	        	        	  
					comando.CommandText ="SELECT id_cliente,"+
							"to_char(porcentage_descuento, '999.99') AS porcentagedescuento "+
							"FROM osiris_erp_clientes "+
							"WHERE id_cliente = '"+this.id_cliente+"' ;";
					Console.WriteLine("es   "+comando.CommandText);
					NpgsqlDataReader lector = comando.ExecuteReader ();
					if (lector.Read()){

						if(Convert.ToDecimal((string) lector["porcentagedescuento"]) > 0){
							this.entry_porciento.Text =  (string) lector["porcentagedescuento"];
							this.entry_pesos.Sensitive = false;
							this.radiobutton_directo.Sensitive = false;
						}else{							
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							                                               MessageType.Error, 
							                                               ButtonsType.Close, "No existe ningun descuento  \n"+
							                                               "para este cliente");
							msgBoxError.Run ();
							msgBoxError.Destroy();
							
							this.radiobutton_porcentage.Active = false;
						}
								
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				}	
			}
		}
		
		
		void on_calcula_nota_credito_clicked(object sender, EventArgs args)
		{
			sub_15 = 0;
			sub_0 = 0;
			tot_iva = 0;
			this.entry_iva_15.Text = "0.00";
			this.entry_sub_0.Text = "0.00";
			this.entry_sub_15.Text = "0.00";
			this.entry_sub_total.Text = "0.00";
			this.entry_total.Text = "0.00";
			this.button_guardar.Sensitive = true;
			if(this.radiobutton_directo.Active == true){
			
		
					if(this.id_cliente == 1)
				    {	
						calculo = Convert.ToDecimal(this.entry_pesos.Text);
						sub_0 = this.calculo;

						this.entry_sub_15.Text = sub_0.ToString("F");
						this.entry_sub_total.Text = (sub_15 + sub_0 + tot_iva).ToString("F");	 	
						this.entry_total.Text = (sub_15 + sub_0 + tot_iva).ToString("F");
					}else{	
						calculo = Convert.ToDecimal(this.entry_pesos.Text);
						sub_15 = Convert.ToDecimal(Convert.ToDouble(this.calculo)/valoriva);
						tot_iva = (calculo - sub_15);
						
						this.entry_iva_15.Text = tot_iva.ToString("F");
						this.entry_sub_15.Text = sub_15.ToString("F");
						this.entry_sub_total.Text = (sub_15 + sub_0 + tot_iva).ToString("F");	 	
						this.entry_total.Text = (sub_15 + sub_0 + tot_iva).ToString("F");
					}

			}else{
				if(this.id_cliente == 1)
				{	

					calculo = Convert.ToDecimal(entry_porciento.Text);		
					
					
					sub_15 = (this.subtotal_15 + this.total_de_iva)/calculo;
					
					
					this.entry_sub_15.Text = sub_15.ToString("F");
					this.entry_sub_total.Text = sub_15.ToString("F");	 	
					this.entry_total.Text = sub_15.ToString("F");
				
				
				    
				}else{

					this.calculo = Convert.ToDecimal(entry_porciento.Text);		
					this.sub_0 = subtotal_0 / calculo;
					this.entry_sub_0.Text = sub_0.ToString("F");	
					Console.WriteLine(sub_0);
					this.sub_15 = subtotal_15 / calculo;
					this.entry_sub_15.Text = sub_15.ToString("F");	
									Console.WriteLine(sub_15);
					this.tot_iva = total_de_iva / calculo;
					this.entry_iva_15.Text = tot_iva.ToString("F");	
					
					this.entry_sub_total.Text = (sub_15 + sub_0 + tot_iva).ToString("F");	 	
					this.entry_total.Text = (sub_15 + sub_0 + tot_iva).ToString("F");	 
				}
			}
			this.entry_sub_total.Text = (sub_15 + sub_0 + tot_iva).ToString("F");	 	
			
		}
		
		void on_guarda_clicked (object sender, EventArgs args)
		{
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
					
				comando.CommandText = "INSERT INTO osiris_erp_notacredito_enca ("+
										"numero_factura,"+//1
	             						"numero_ntacred,"+
	                					"fecha_creacion_nota_credito,"+
	                					"sub_total_15,"+
	                					"sub_total_0,"+
	                					"iva_al_impuesto,"+
						                "descripcion1,"+
						                "descripcion2,"+
	                					"id_quien_creo,"+
	                					"total) "+
	                					"VALUES ('"+
	                					this.entry_factura.Text.Trim()+"','"+
	                					this.entry_nota_credito.Text.Trim()+"','"+
	                				    DateTime.Now.ToString("yyyy-MM-dd")+"','"+
	                					this.entry_sub_15.Text.Trim()+"','"+
	                					this.entry_sub_0.Text.Trim()+"','"+
	                					this.entry_iva_15.Text.Trim()+"','"+

						                this.entry_descripcion1.Text.Trim().ToUpper()+"','"+
						                this.entry_descripcion2.Text.Trim().ToUpper()+"','"+
						                ///////
	                					LoginEmpleado+"','"+
				                 		this.entry_total.Text.Trim()+"');";
				comando.ExecuteNonQuery();
				comando.Dispose();
				
				NpgsqlConnection conexion2;
				conexion2 = new NpgsqlConnection (connectionString+nombrebd );
				// Verifica que la base de datos este conectada
				try{
					conexion2.Open ();
					NpgsqlCommand comando2; 
					comando2 = conexion2.CreateCommand ();
					comando2.CommandText =  "UPDATE osiris_erp_factura_enca SET id_quien_creo = ' "+LoginEmpleado+"',"+
											                    "numero_ntacred = '"+entry_nota_credito.Text.Trim()+"',"+
							                                    "id_quien_creo_ntacred = '"+LoginEmpleado+"',"+
											                    "total_ntacred = '"+entry_total.Text.Trim()+"', "+
												                "fechahora_creacion_ntacred = ' "+this.entry_fecha.Text.Trim()+" ' "+
											                    "WHERE numero_factura = '"+this.entry_factura.Text.Trim()+"' ;";
					comando2.ExecuteNonQuery();							
					comando2.Dispose();				
					NpgsqlConnection conexion3;
					conexion3 = new NpgsqlConnection (connectionString+nombrebd );
					// Verifica que la base de datos este conectada
					try{
						conexion3.Open ();
						NpgsqlCommand comando3; 
						comando3 = conexion3.CreateCommand ();
						
						comando3.CommandText =  "UPDATE osiris_erp_cobros_enca SET numero_ntacred = ' "+entry_nota_credito.Text.Trim()+"',"+
											                    "valor_total_notacredito = '"+entry_total.Text.Trim()+" ' "+
											                    "WHERE numero_factura = '"+this.entry_factura.Text.Trim()+"' ;";
		 						Console.WriteLine(comando3.CommandText);	
						comando3.ExecuteNonQuery();							
						comando3.Dispose();
					
					
		

						NpgsqlConnection conexion1;
						conexion1 = new NpgsqlConnection (connectionString+nombrebd );
						// Verifica que la base de datos este conectada
						try{
							conexion1.Open ();
							NpgsqlCommand comando1; 
							comando1 = conexion1.CreateCommand ();
					
							comando1.CommandText = "INSERT INTO osiris_erp_notacredito_deta ("+
								                   "numero_ntacred,"+//1
	                					           "total) "+
	                					           "VALUES ('"+
	                					           this.entry_nota_credito.Text.Trim()+"','"+
	                					           this.entry_total.Text.Trim()+"');";
 							
							comando1.ExecuteNonQuery();							comando1.Dispose();
							
						}catch (NpgsqlException ex){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							                                               MessageType.Error, 
							                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();
							msgBoxError.Destroy();
						}
						conexion1.Close();

					

						MessageDialog msgBoxError1 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						                                                MessageType.Info,ButtonsType.Close, " La Nota de Credito se Guardo Correctamente ");
						msgBoxError1.Run ();			msgBoxError1.Destroy();
						
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						                                               MessageType.Error, 
						                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
					}
					conexion3.Close();
					
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Error, 
				                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				}
				conexion2.Close();
				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Error, 
				                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			conexion.Close();
		}		
			
		void on_limpiar_clicked (object sender, EventArgs args)
		{
			sub_15 = 0;
			sub_0 = 0;
			tot_iva = 0;
			this.entry_iva_15.Text = "0.00";
			this.entry_sub_0.Text = "0.00";
			this.entry_sub_15.Text = "0.00";
			this.entry_sub_total.Text = "0.00";
			this.entry_total.Text = "0.00";
		}
		
		void on_cancelar_nota_clicked (object sender, EventArgs args)		
		{
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
		
			
				comando.CommandText = "UPDATE osiris_erp_notacredito_enca SET cancelado = 'true',"+
											    "id_quien_cancelo = ' "+LoginEmpleado+" ' "+
											    "fechahora_de_cancelacion '"+DateTime.Now.ToString("yyyy-MM-dd")+" ' "+
							                    "WHERE numero_ntacred = '"+this.entry_nota_credito.Text+"' ;"; 
							                //    Console.WriteLine("este"+comando3.CommandText.ToString());

				NpgsqlDataReader lector = comando.ExecuteReader ();

				MessageDialog msgBoxError1 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Info,ButtonsType.Close, " La Nota de Credito fue Cancelada Satisfactoriamente ");
				msgBoxError1.Run ();			msgBoxError1.Destroy();
				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Error, 
				                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			conexion.Close();		
		}

		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_valida_numeros(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
			{
				args.RetVal = true;
			}
		}
	}
}