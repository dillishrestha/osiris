//////////////////////////////////////////////////////////
// created on 21/06/2007 at 01:40 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Juan Antonio Peña Gonzalez (Programacion)
// 				  Ing. Daniel Olivares Cuevas (Pre-Programacion y Adecuaciones)
//                Homero Montoya Galvan (Aducuaciones)
//				 
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
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPO.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Foobar; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
//////////////////////////////////////////////////////////
// Programa		: catalogs.cs
// Proposito	: Catalogos Generales
// Objeto		: 
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class catalogos_generales
	{	
		// Declarando ventana principal de menu
		[Widget] Gtk.Window menu_catalogos;
		[Widget] Gtk.Button button_catalogo_clientes;
		[Widget] Gtk.Button button_catalogo_medicos; 
		[Widget] Gtk.Button button_catalogo_proveedores;
		[Widget] Gtk.Button button_estados_municipios;		
		
		// Declarando ventana de clientes
		[Widget] Gtk.Window catalogo_cliente;
		[Widget] Gtk.Entry entry_id_cliente;
		[Widget] Gtk.Entry entry_nombre_cliente;
		[Widget] Gtk.Entry entry_calle_cliente;
		[Widget] Gtk.Entry entry_rfc_cliente;
		[Widget] Gtk.Entry entry_curp_cliente;
		[Widget] Gtk.Entry entry_colonia_cliente;
		[Widget] Gtk.Entry entry_CP_cliente;
		[Widget] Gtk.Entry entry_telcasa;
		[Widget] Gtk.Entry entry_teloficina;
		[Widget] Gtk.Entry entry_telcelular;
		[Widget] Gtk.CheckButton checkbutton_nuevo_cliente;
		[Widget] Gtk.CheckButton checkbutton_cliente_activo;
		[Widget] Gtk.CheckButton checkbutton_envio_facturas;
		
		//Declarando ventana de medicos
		[Widget] Gtk.Window catalogo_medico;
		[Widget] Gtk.CheckButton checkbutton_nuevo_medico;
		[Widget] Gtk.CheckButton checkbutton_medico_provisional;
		[Widget] Gtk.Entry entry_fecha_revision;
		[Widget] Gtk.Entry entry_id_medico;
		[Widget] Gtk.CheckButton checkbutton_medico_activo;
		[Widget] Gtk.CheckButton checkbutton_centro_medico;
		[Widget] Gtk.Entry entry_fecha_ingreso;
		[Widget] Gtk.Entry entry_nombre1_medico;
		[Widget] Gtk.Button button_buscar_medico;
		[Widget] Gtk.Entry entry_nombre2_medico;
		[Widget] Gtk.Entry entry_apellido_paterno_medico;
		[Widget] Gtk.Entry entry_apellido_materno_medico;
		[Widget] Gtk.Entry entry_cedula_profecional;
		[Widget] Gtk.Entry entry_especialidad;
		[Widget] Gtk.Button button_buscar_especialidad;
		[Widget] Gtk.Entry entry_empresa;
		[Widget] Gtk.Button button_buscar_empresa;
		[Widget] Gtk.Entry entry_direccion_casa_medico;
		[Widget] Gtk.Entry entry_direccion_consultorio_medico;
		[Widget] Gtk.Entry entry_telcasa_medico;
		[Widget] Gtk.Entry entry_teloficina_medico;
		[Widget] Gtk.Entry entry_celular1_medico;
		[Widget] Gtk.Entry entry_celular2_medico;
		[Widget] Gtk.Entry entry_nextel_medico;
		[Widget] Gtk.Entry entry_beeper_medico;
		[Widget] Gtk.CheckButton checkbutton_tituloprof_medico;
		[Widget] Gtk.CheckButton checkbutton_cedula_prof_medico;
		[Widget] Gtk.CheckButton checkbutton_diploespecial_medico;
		[Widget] Gtk.CheckButton checkbutton_cursoadistramiento_medico;
		[Widget] Gtk.CheckButton checkbutton_diplomasubespecial;
		[Widget] Gtk.CheckButton checkbutton_copiaidentificacionoficial;
		[Widget] Gtk.CheckButton checkbutton_copiacedularfc;
		[Widget] Gtk.CheckButton checkbutton_certificadoconsejo_esp;
		[Widget] Gtk.CheckButton checkbutton_constancia_congresos;
		[Widget] Gtk.CheckButton checkbutton_copia_comprobante_domicilio;
		[Widget] Gtk.CheckButton checkbutton_diplomaextranjero;
		[Widget] Gtk.CheckButton checkbutton_diplomacursos;
		[Widget] Gtk.CheckButton checkbutton_diploseminarios; 
		[Widget] Gtk.CheckButton checkbutton_cedula_especialidad_medico;
		
		//[Widget] Gtk.TextView textview_notas;
		[Widget] Gtk.Button button_notas;
		[Widget] Gtk.Button button_imprimir;
		[Widget] Gtk.Button button_autorizacion_medico;
				
		//Declarando ventanas de busqueda
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_buscar_busqueda;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.TreeView lista_de_busqueda;
		[Widget] Gtk.TreeView lista_de_medicos;
		[Widget] Gtk.ComboBox combobox_tipo_busqueda;
		
		///boton general de salida
		[Widget] Gtk.Button button_buscar;
		[Widget] Gtk.Button button_guardar;
		[Widget] Gtk.Button button_actualizar;
		[Widget] Gtk.Button button_editar;
		[Widget] Gtk.Button button_salir;
		[Widget] Gtk.ComboBox combobox_municipios;
		[Widget] Gtk.ComboBox combobox_estado;
		[Widget] Gtk.ComboBox combobox_formapago;
		
		//Declarando ventana catalogo de proveedores hmg
		[Widget] Gtk.Window catalogo_proveedore;
		[Widget] Gtk.CheckButton checkbutton_proveedor_nuevo;
		[Widget] Gtk.CheckButton checkbutton_proveedor_activo;
		[Widget] Gtk.Entry entry_id_proveedor;
		[Widget] Gtk.Entry entry_proveedor;
		[Widget] Gtk.Entry entry_rfc_proveedor;
		[Widget] Gtk.Entry entry_curp_proveedor;
		[Widget] Gtk.Entry entry_calle_proveedor;		
		[Widget] Gtk.Entry entry_colonia_proveedor;
		[Widget] Gtk.Entry entry_cp_proveedor;
		[Widget] Gtk.Entry entry_telefono_proveedor;
		[Widget] Gtk.Entry entry_fax_proveedor;
		[Widget] Gtk.Entry entry_celular_proveedor;
		[Widget] Gtk.Entry entry_nextel_proveedor;
		[Widget] Gtk.Entry entry_contacto_proveedor;
		[Widget] Gtk.Entry entry_correo_proveedor;
		[Widget] Gtk.Entry entry_pagina_web_proveedor;
		
		// declarando ventana agrega municipios/estados
		[Widget] Gtk.Window estado_municipio;
		[Widget] Gtk.CheckButton checkbutton_estado; 
		[Widget] Gtk.CheckButton checkbutton_municipio;
		[Widget] Gtk.Button button_busca_estado;
		[Widget] Gtk.Button button_busca_municipio;
		[Widget] Gtk.Button button_guarda_estado;
		[Widget] Gtk.Button button_guarda_municipio;
		[Widget] Gtk.Entry entry_valor_estado;
		[Widget] Gtk.Entry entry_estado;
		[Widget] Gtk.Entry entry_valor_municipio;
		[Widget] Gtk.Entry entry_municipio;
		//[Widget] Gtk.Button button_edita_municipio;
	
		//variables principales
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nomcatalogo;
		bool actualizacion = false;
		string busqueda = "";
		int idestado = 1;
		int idformadepago = 1;
		
		//variables de datos de clientes
		string municipios = "";
		string estado = "";
		bool activo = true;
		bool nuevo_cliente = false;
		
		//varibles de datos de medicos
		int id_esp_medico = 1;
		int id_emp_medico = 1;
		bool medico_activo = false;
		bool medico_provisional = false;
		bool tituloprof_medico = false;
		bool cedula_prof_medico = false;
		bool diploespecial_medico = false;
		bool cursoadistramiento_medico = false;
		bool diplomasubespecial = false;
		bool copiaidentificacionoficial = false;
		bool copiacedularfc = false;
		bool certificadoconsejo_esp = false;
		bool constancia_congresos = false;
		bool copia_comprobante_domicilio = false;
		bool diplomaextranjero = false;
		bool diploseminarios = false; 
		bool diplomacursos = false;
		string tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";		
		
		string connectionString;
		string nombrebd;
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		private TreeStore treeViewEngineproveedores;
		private TreeStore treeViewEngineClientes;
		private TreeStore treeViewEngineMedicos;
		private TreeStore treeViewEngineEspecialidad;
		private TreeStore treeViewEngineEmpresa;
		private TreeStore treeViewEnginemunicipio;
		private TreeStore treeViewEngineestado;
		
		///Declaracion de columnas de busqueda de Medicos
		TreeViewColumn col_idmedico;			TreeViewColumn col_nomb1medico;		
		TreeViewColumn col_nomb2medico;		TreeViewColumn col_appmedico;		
		TreeViewColumn col_apmmedico;		TreeViewColumn col_espemedico;		
		TreeViewColumn col_telmedico;		TreeViewColumn col_cedulamedico;		
		TreeViewColumn col_telOfmedico;		TreeViewColumn col_celmedico;		
		TreeViewColumn col_celmedico2;		TreeViewColumn col_nextelmedico;		
		TreeViewColumn col_beepermedico;		TreeViewColumn col_empresamedico;		
		TreeViewColumn col_estadomedico;		
		
		//Declarando las celdas
		CellRendererText cellr0;				CellRendererText cellrt1;
		CellRendererText cellrt2;			CellRendererText cellrt3;
		CellRendererText cellrt4;			CellRendererText cellrt5;
		CellRendererText cellrt6;			CellRendererText cellrt7;
		CellRendererText cellrt8;			CellRendererText cellrt9;
		CellRendererText cellrt10;			CellRendererText cellrt11;
		CellRendererText cellrt12;			CellRendererText cellrt13;
		CellRendererText cellrt37;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public catalogos_generales(string nomcatalogo_,string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_)
		{					
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nomcatalogo = nomcatalogo_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			if (nomcatalogo=="menu") 			{	menu_de_catalogos();				}
			if (nomcatalogo=="cliente") 		{	catalogo_clientes();				}
			if (nomcatalogo=="cliente1") 		{	catalogo_clientes();				}
			if (nomcatalogo=="medicos") 		{	catalogo_medicos();					}
			if (nomcatalogo=="proveedores") 	{	this.catalogo_proveedores();		}
		}
//////////////////////////////////////////////MENU DE CATALOGOS/////////////////////////////////////////////////////
		void menu_de_catalogos()
		{
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "menu_catalogos", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        //Muestra ventana de Glade
			menu_catalogos.Show();
			button_catalogo_clientes.Clicked += new EventHandler(on_button_catalogo_clientes_clicked);
			button_catalogo_medicos.Clicked += new EventHandler(on_button_catalogo_medicos_clicked);
			button_catalogo_proveedores.Clicked += new EventHandler(on_button_catalogo_proveedores_clicked);
			button_estados_municipios.Clicked += new EventHandler(on_button_agrega_estados_municipios_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
		}
		
		void on_button_catalogo_clientes_clicked(object sender, EventArgs args)
		{
			catalogo_clientes();
		}
			
		void on_button_catalogo_medicos_clicked(object sender, EventArgs args)
		{
			catalogo_medicos();
		}
		
		void on_button_catalogo_proveedores_clicked(object sender, EventArgs args)
		{
			catalogo_proveedores();
		}
		
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////		
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////VENTANA DE CATALOGO DE PROVEEDORES///////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		
		void catalogo_proveedores()
		{		
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "catalogo_proveedore", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        //Muestra ventana de Glade
			catalogo_proveedore.Show();			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_guardar.Clicked +=  new EventHandler(on_guarda_proveedor_clicked);
			button_editar.Clicked +=  new EventHandler(on_editar_proveedores_clicked);
			button_buscar.Clicked += new EventHandler(on_busca_proveedores);
			checkbutton_proveedor_nuevo.Clicked += new EventHandler(on_checkbutton_proveedor_nuevo_clicked);
			//Desactivar campos 
			button_guardar.Sensitive = false;
			button_editar.Sensitive = false;
			entry_id_proveedor.Sensitive = false;
			entry_proveedor.Sensitive = false;
			entry_rfc_proveedor.Sensitive = false;
			entry_curp_proveedor.Sensitive = false;
			entry_calle_proveedor.Sensitive = false;				
			entry_colonia_proveedor.Sensitive = false;
			entry_cp_proveedor.Sensitive = false;
			entry_telefono_proveedor.Sensitive = false;
			combobox_estado.Sensitive = false;
			combobox_formapago.Sensitive = false;
			combobox_municipios.Sensitive = false;
			entry_fax_proveedor.Sensitive = false;
			entry_celular_proveedor.Sensitive = false;
			entry_nextel_proveedor.Sensitive = false;
			entry_contacto_proveedor.Sensitive = false;
			entry_correo_proveedor.Sensitive = false;
			entry_pagina_web_proveedor.Sensitive = false;
			checkbutton_proveedor_activo.Sensitive = false;			
	    }
	    
		//Como dar de alta un Proveedor
		void on_checkbutton_proveedor_nuevo_clicked (object sender, EventArgs args)
		{
			int numeroproveedor;
			if(checkbutton_proveedor_nuevo.Active == true)
			 { 
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de querer crear un nuevo proveedor?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
	 			if (miResultado == ResponseType.Yes)
	 			{
	 				llenado_estados("nuevo","",0);
	 				llenado_municipios("nuevo","");
					llenado_formapago("nuevo",0,"");
					limpia_textos("PROVEEDORES");
					button_guardar.Sensitive = true;
					button_editar.Sensitive = false;
					activa_campos(true,"PROVEEDORES");
					
					numeroproveedor = int.Parse((string) lee_numero_proveedor())+1;
	    	       	entry_id_proveedor.Text = numeroproveedor.ToString();
	    	       	checkbutton_proveedor_activo.Active = false;
				}else{
					checkbutton_proveedor_nuevo.Active = false;
				}			
			}
			if (checkbutton_proveedor_nuevo.Active == false) 
			{ 
				activa_campos(false,"PROVEEDORES");
			}
		}
     	
		
		string lee_numero_proveedor()
		{
		    NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			string tomavalor = "";
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(id_proveedor,'99999999') AS idproveedor "+ 
										"FROM osiris_erp_proveedores "+
										"ORDER BY id_proveedor DESC LIMIT 1;";
										NpgsqlDataReader lector = comando.ExecuteReader ();
				if (lector.Read())
				{	
					tomavalor =(string) lector["idproveedor"]; 
					return tomavalor;
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"No se guardo correctamente el proveedor");
							msgBoxError.Run ();			msgBoxError.Destroy();
					return tomavalor;
					}
			}catch (NpgsqlException ex){
		   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();			msgBoxError.Destroy();
							return tomavalor;
					}
		}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////// Busqueda de Proveedores ///////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void on_busca_proveedores(object sender, EventArgs args)
			{
				busqueda = "proveedores";
				Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador", null);
				gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
		        button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_proveedores);
		        entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
		        button_selecciona.Clicked += new EventHandler(on_selecciona_proveedor);
		       	checkbutton_proveedor_nuevo.Active = false;
		       	crea_treeview_proveedores();
				button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
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
													typeof(string),//12
													typeof(string),//13
													typeof(string));//14
												
			lista_de_busqueda.Model = treeViewEngineproveedores;
			
			lista_de_busqueda.RulesHint = true;
				
			lista_de_busqueda.RowActivated += on_selecciona_proveedor;  // Doble click selecciono paciente*/
			
			
			TreeViewColumn col_idproveedor = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idproveedor.Title = "ID Proveedores"; // titulo de la cabecera de la columna, si está visible
			col_idproveedor.PackStart(cellr0, true);
			col_idproveedor.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1
			col_idproveedor.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_idproveedor.SortColumnId = (int) Col_proveedores.col_idproveedor;
			
			TreeViewColumn col_proveedor = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_proveedor.Title = "Proveedores";
			col_proveedor.PackStart(cellrt1, true);
			col_proveedor.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
			col_proveedor.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_proveedor.SortColumnId = (int) Col_proveedores.col_proveedor;
			
			TreeViewColumn col_calle = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_calle.Title = "Calle";
			col_calle.PackStart(cellrt2, true);
			col_calle.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 3
			col_calle.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_calle.SortColumnId = (int) Col_proveedores.col_calle;
			
			TreeViewColumn col_colonia = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_colonia.Title = "Colonia";
			col_colonia.PackStart(cellrt3, true);
			col_colonia.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 4
			col_colonia.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_colonia.SortColumnId = (int) Col_proveedores.col_colonia;
			
            TreeViewColumn col_municipio = new TreeViewColumn();
            CellRendererText cellrt4 = new CellRendererText();
            col_municipio.Title = "Municipio";
            col_municipio.PackStart(cellrt4, true);
			col_municipio.AddAttribute(cellrt4,"text", 4); // la siguiente columna será 5
			col_municipio.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_municipio.SortColumnId = (int) Col_proveedores.col_municipio;
			
            TreeViewColumn col_estado = new TreeViewColumn();
            CellRendererText cellrt5 = new CellRendererText();
            col_estado.Title = "Estado";
            col_estado.PackStart(cellrt5, true);
            col_estado.AddAttribute(cellrt5,"text", 5); // la siguiente columna será 6
            col_estado.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_estado.SortColumnId = (int) Col_proveedores.col_estado;
			
            TreeViewColumn col_telefono = new TreeViewColumn();
            CellRendererText cellrt6 = new CellRendererText();
            col_telefono.Title = "Telefono";
            col_telefono.PackStart(cellrt6, true);
            col_telefono.AddAttribute(cellrt6,"text", 6); // la siguiente columna será 7
            col_telefono.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
            col_telefono.SortColumnId = (int) Col_proveedores.col_telefono;
            
            TreeViewColumn col_contacto = new TreeViewColumn();
            CellRendererText cellrt7 = new CellRendererText();
            col_contacto.Title = "Contacto";
            col_contacto.PackStart(cellrt7, true);
            col_contacto.AddAttribute(cellrt7,"text", 7);// la siguiente columna será 8
            col_contacto.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
			col_contacto.SortColumnId = (int) Col_proveedores.col_contacto;
			
            TreeViewColumn col_cp = new TreeViewColumn();
            CellRendererText cellrt8 = new CellRendererText();
            col_cp.Title = "Codigo Postal";
            col_cp.PackStart(cellrt8, true);
            col_cp.AddAttribute(cellrt8,"text", 8);// la siguiente columna será 9
            col_cp.SetCellDataFunc(cellrt8, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
            col_cp.SortColumnId = (int) Col_proveedores.col_cp;
			
            TreeViewColumn col_web = new TreeViewColumn();
            CellRendererText cellrt9 = new CellRendererText();
            col_web.Title = "Pag. Web";
            col_web.PackStart(cellrt9, true);
            col_web.AddAttribute(cellrt9,"text", 9);// la siguiente columna será 10
            col_web.SetCellDataFunc(cellrt9, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
            col_web.SortColumnId = (int) Col_proveedores.col_web;
            
            TreeViewColumn col_rfc = new TreeViewColumn();
            CellRendererText cellrt13 = new CellRendererText();
            col_rfc.Title = "R.F.C.";
            col_rfc.PackStart(cellrt13, true);
            col_rfc.AddAttribute(cellrt13,"text", 13);// la siguiente columna será 10
            col_rfc.SetCellDataFunc(cellrt13, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
            col_rfc.SortColumnId = (int) Col_proveedores.col_rfc;
            
            TreeViewColumn col_curp = new TreeViewColumn();
            CellRendererText cellrt14 = new CellRendererText();
            col_curp.Title = "C.U.R.P.";
            col_curp.PackStart(cellrt14, true);
            col_curp.AddAttribute(cellrt14,"text", 14);// la siguiente columna será 10
            col_curp.SetCellDataFunc(cellrt14, new Gtk.TreeCellDataFunc(cambia_colores_proveedor));
            col_curp.SortColumnId = (int) Col_proveedores.col_curp;
            		           
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
			lista_de_busqueda.AppendColumn(col_rfc);
			lista_de_busqueda.AppendColumn(col_curp);					
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
				col_web,
				col_rfc,
				col_curp
				
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
								"id_proveedor,contacto1_proveedor,mail_proveedor,pagina_web_proveedor,rfc_proveedor,curp_proveedor,"+
								"osiris_erp_proveedores.id_forma_de_pago, descripcion_forma_de_pago AS descripago "+
								"FROM osiris_erp_proveedores, osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_proveedores.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
								"ORDER BY descripcion_proveedor;";															
				}
				else
				{
					comando.CommandText = "SELECT descripcion_proveedor,direccion_proveedor,rfc_proveedor,curp_proveedor, "+
								"colonia_proveedor,municipio_proveedor,estado_proveedor,telefono1_proveedor, "+ 
								"telefono2_proveedor,celular_proveedor,cp_proveedor, proveedor_activo, "+
								"id_proveedor,contacto1_proveedor,mail_proveedor,pagina_web_proveedor,rfc_proveedor,curp_proveedor,"+
								"osiris_erp_proveedores.id_forma_de_pago, descripcion_forma_de_pago AS descripago "+
								"FROM osiris_erp_proveedores, osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_proveedores.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
								"AND descripcion_proveedor LIKE '%"+(string) entry_expresion.Text.ToUpper()+"%' "+
								"ORDER BY descripcion_proveedor;";
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read())
				{	
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
													(string) lector["descripago"],		//12
													(string) lector["rfc_proveedor"],	//13
													(string) lector["curp_proveedor"]);	//14
					
				}
			}
			
				catch (NpgsqlException ex)
				{
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
				}
			conexion.Close ();
		}
		
		void on_selecciona_proveedor(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_busqueda.Selection.GetSelected(out model, out iterSelected)) 
 			{
 				int tomaidproveedor = (int) model.GetValue(iterSelected, 0);
 				this.entry_rfc_proveedor.Text = (string) model.GetValue(iterSelected, 13);
	 			this.entry_curp_proveedor.Text = (string) model.GetValue(iterSelected, 14);
 				entry_id_proveedor.Text = tomaidproveedor.ToString();
 				entry_proveedor.Text = (string) model.GetValue(iterSelected, 1);
				entry_calle_proveedor.Text = (string) model.GetValue(iterSelected, 2);
				entry_colonia_proveedor.Text = (string) model.GetValue(iterSelected, 3);
				municipios = (string) model.GetValue(iterSelected, 4);
				estado = (string) model.GetValue(iterSelected, 5);
				entry_telefono_proveedor.Text = (string) model.GetValue(iterSelected, 6);
				entry_contacto_proveedor.Text = (string) model.GetValue(iterSelected, 7);
				entry_cp_proveedor.Text = (string) model.GetValue(iterSelected, 8);
				entry_pagina_web_proveedor.Text = (string) model.GetValue(iterSelected, 9);
				checkbutton_proveedor_activo.Active = (bool) model.GetValue(iterSelected, 11);
				button_guardar.Sensitive = false;
				button_editar.Sensitive = true;
				
				llenado_estados("selecciona",estado,0);
				llenado_municipios("selecciona",municipios);
				llenado_formapago("selecciona",(int)model.GetValue(iterSelected,10),(string) model.GetValue(iterSelected,12));
				
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
		
		void cambia_colores_proveedor(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//if ((bool) lista_de_busqueda.Model.GetValue(iter,10) == false)
			//{(cell as Gtk.CellRendererText).Foreground = "darkgreen";		}
		}
		
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////Hasta aki la busqueda de proveedores//////////////////////////////////////////////
		void on_guarda_proveedor_clicked(object sender, EventArgs args)
		{
			if(checkbutton_proveedor_nuevo.Active == true)
			{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
										ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
 				if (miResultado == ResponseType.Yes){
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
    	        	// Verifica que la base de datos este conectada
    	        	int numero_proveedor;
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
	 					comando.CommandText = "INSERT INTO osiris_erp_proveedores("+
	 										  "descripcion_proveedor,"+ // ) "+
	 										  "rfc_proveedor,"+
	 										  "curp_proveedor,"+
	 										  "id_forma_de_pago,"+
	 										  "direccion_proveedor,"+
	 										  "colonia_proveedor,"+
	 										  "cp_proveedor,"+
	 										  "estado_proveedor,"+
	 										  "municipio_proveedor,"+
	 										  "telefono1_proveedor,"+ 
											  "fax_proveedor,"+
											  "celular_proveedor,"+
											  "nextel_proveedor,"+
											  "contacto1_proveedor,"+
											  "mail_proveedor,"+
											  "pagina_web_proveedor,"+
											  "proveedor_activo,"+
											  "id_quien_creo,"+
											  "fechahora_creacion_proveedor) "+
	 										  "VALUES ('"+
	 										  (string) entry_proveedor.Text.Trim().ToUpper()+"','"+
	 										  (string) entry_rfc_proveedor.Text.Trim().ToUpper()+"','"+
	 										  (string) entry_curp_proveedor.Text.Trim().ToUpper()+"','"+
	 										  idformadepago.ToString()+"','"+
	 										  (string) entry_calle_proveedor.Text.Trim().ToUpper()+"','"+
	 										  (string) entry_colonia_proveedor.Text.Trim().ToUpper()+"','"+
		 									  (string) entry_cp_proveedor.Text.Trim()+"','"+
		 									  (string) estado.ToString().ToUpper()+"','"+
		 									  (string) municipios.ToString().ToUpper()+"','"+
		 									  (string) entry_telefono_proveedor.Text.Trim()+"','"+
		 									  (string) entry_fax_proveedor.Text.Trim()+"','"+
		 									  (string) entry_celular_proveedor.Text.Trim()+"','"+
		 									  (string) entry_nextel_proveedor.Text.Trim()+"','"+
		 									  (string) entry_contacto_proveedor.Text.Trim()+"','"+
		 									  (string) entry_correo_proveedor.Text.Trim()+"','"+
		 									  (string) entry_pagina_web_proveedor.Text.Trim()+"','"+
		 									  (bool) checkbutton_proveedor_activo.Active+"','"+
		 									  (string) LoginEmpleado+"','"+
		 									  (string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+
	 										  "');";
 						comando.ExecuteNonQuery();    	    	       	comando.Dispose();
    	    	       	numero_proveedor = int.Parse((string) lee_numero_proveedor());
    	    	       	checkbutton_proveedor_nuevo.Active = false;
    	    	       	entry_id_proveedor.Text = numero_proveedor.ToString();
    	    	       	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info,ButtonsType.Close,"El proveedor se guardo con exito");
						msgBoxError.Run ();					msgBoxError.Destroy();
					}catch(NpgsqlException ex){
	   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();					msgBoxError.Destroy();
	       			}
       				conexion.Close ();
       			}
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
										ButtonsType.YesNo,"¿ Desea Actualizar esta infomacion ?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();							msgBox.Destroy();
				if (miResultado == ResponseType.Yes){
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
	    	     	// Verifica que la base de datos este conectada
	    	     	try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						comando.CommandText = "UPDATE osiris_erp_proveedores SET "+
						"descripcion_proveedor = '"+(string) entry_proveedor.Text.Trim().ToUpper()+"', "+
						"rfc_proveedor = '"+(string) entry_rfc_proveedor.Text.Trim().ToUpper()+"', "+
						"curp_proveedor = '"+(string) entry_curp_proveedor.Text.Trim().ToUpper()+"', "+
						"id_forma_de_pago = '"+ idformadepago.ToString().ToUpper()+"', "+
						"direccion_proveedor = '"+(string) entry_calle_proveedor.Text.Trim().ToUpper()+"', "+
						"colonia_proveedor = '"+(string) entry_colonia_proveedor.Text.Trim().ToUpper()+"', "+
						"cp_proveedor = '"+(string) entry_cp_proveedor.Text.Trim()+"', "+
						"estado_proveedor = '"+(string) estado.ToString().ToUpper()+"', "+
						"municipio_proveedor = '"+(string) municipios.ToString().ToUpper()+"', "+
						"telefono1_proveedor = '"+(string) entry_telefono_proveedor.Text.Trim()+"', "+ 
						"fax_proveedor = '"+(string) entry_fax_proveedor.Text.Trim()+"', "+
						"celular_proveedor = '"+(string) entry_celular_proveedor.Text.Trim()+"', "+
						"nextel_proveedor = '"+(string) entry_nextel_proveedor.Text.Trim()+"', "+
						"contacto1_proveedor = '"+(string) entry_contacto_proveedor.Text.Trim()+"', "+
						"mail_proveedor = '"+(string) entry_correo_proveedor.Text.Trim()+"', "+
						"pagina_web_proveedor = '"+(string) entry_pagina_web_proveedor.Text.Trim()+"', "+
						"proveedor_activo = '"+(bool) checkbutton_proveedor_activo.Active+"', "+
						"id_quien_baja = '"+(string) LoginEmpleado+"', "+
						"historial_cambios_proveedor = 	historial_cambios_proveedor || '"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n' "+
						"WHERE id_proveedor = '"+entry_id_proveedor.Text.Trim()+"' ;";
 						comando.ExecuteNonQuery();    	    	       	comando.Dispose();
    	    	       	
    	    	       	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info,ButtonsType.Close,"Los datos se actualizaron con exito");			
						msgBoxError.Run ();					msgBoxError.Destroy();
						entry_id_proveedor.Sensitive = false;
						entry_proveedor.Sensitive = false;
						entry_rfc_proveedor.Sensitive = false;
						entry_curp_proveedor.Sensitive = false;
						entry_calle_proveedor.Sensitive = false;				
						entry_colonia_proveedor.Sensitive = false;
						entry_cp_proveedor.Sensitive = false;
						entry_telefono_proveedor.Sensitive = false;
						combobox_estado.Sensitive = false;
						combobox_formapago.Sensitive = false;
						combobox_municipios.Sensitive = false;
						entry_fax_proveedor.Sensitive = false;
						entry_celular_proveedor.Sensitive = false;
						entry_nextel_proveedor.Sensitive = false;
						entry_contacto_proveedor.Sensitive = false;
						entry_correo_proveedor.Sensitive = false;
						entry_pagina_web_proveedor.Sensitive = false;
						checkbutton_proveedor_nuevo.Active = false;
						checkbutton_proveedor_activo.Sensitive = false;
					}catch(NpgsqlException ex){
	   						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();					msgBoxError.Destroy();
	       			}
       				conexion.Close ();
       			}			
			}
		}
				
		void on_editar_proveedores_clicked (object sender, EventArgs args)
		{
			checkbutton_proveedor_activo.Sensitive = true;	
			checkbutton_proveedor_nuevo.Sensitive = true;
			checkbutton_proveedor_nuevo.Active = false;	
			button_guardar.Sensitive = true;
			entry_id_proveedor.Sensitive = true;
			entry_proveedor.Sensitive = true;
			entry_rfc_proveedor.Sensitive = true;
			entry_curp_proveedor.Sensitive = true;
			entry_calle_proveedor.Sensitive = true;				
			entry_colonia_proveedor.Sensitive = true;
			entry_cp_proveedor.Sensitive = true;
			entry_telefono_proveedor.Sensitive = true;
			combobox_estado.Sensitive = true;
			combobox_formapago.Sensitive = true;
			combobox_municipios.Sensitive = true;
			entry_fax_proveedor.Sensitive = true;
			entry_celular_proveedor.Sensitive = true;
			entry_nextel_proveedor.Sensitive = true;
			entry_contacto_proveedor.Sensitive = true;
			entry_correo_proveedor.Sensitive = true;
			entry_pagina_web_proveedor.Sensitive = true;
		}
		
		
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////		
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////VENTANA DE CATALOGO DE CLIENTES//////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		void catalogo_clientes()
		{
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "catalogo_cliente", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        //Muestra ventana de Glade
			catalogo_cliente.Show();			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_buscar.Clicked += new EventHandler(on_busca_clientes);
			button_guardar.Clicked +=  new EventHandler(on_guarda_clientes);
			button_editar.Clicked +=  new EventHandler(on_editar_clientes_clicked);
			checkbutton_nuevo_cliente.Clicked += new EventHandler(on_checkbutton_nuevo_cliente_clicked);
			checkbutton_cliente_activo.Sensitive = false;
			checkbutton_envio_facturas.Sensitive = false;
			entry_id_cliente.Sensitive = false;
			activa_campos(false,"CLIENTES");			
			button_guardar.Sensitive = false;
			button_editar.Sensitive = false;
		}
		
		void on_busca_clientes(object sender, EventArgs args)
		{
			busqueda = "clientes";
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_clientes);
	        entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
	        button_selecciona.Clicked += new EventHandler(on_selecciona_cliente);
	       	crea_treeview_clientes();
			button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);			
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
													typeof(string),//14
													typeof(bool));// 15
												
			lista_de_busqueda.Model = treeViewEngineClientes;
			
			lista_de_busqueda.RulesHint = true;
				
			lista_de_busqueda.RowActivated += on_selecciona_cliente;  // Doble click selecciono paciente*/
			TreeViewColumn col_idcliente = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idcliente.Title = "ID Clientes"; // titulo de la cabecera de la columna, si está visible
			col_idcliente.PackStart(cellr0, true);
			col_idcliente.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1
			col_idcliente.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_idcliente.SortColumnId = (int) Col_clientes.col_idcliente;
			
			TreeViewColumn col_cliente = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_cliente.Title = "Clientes";
			col_cliente.PackStart(cellrt1, true);
			col_cliente.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
			col_cliente.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_cliente.SortColumnId = (int) Col_clientes.col_cliente;
			
			TreeViewColumn col_calle = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_calle.Title = "Calle";
			col_calle.PackStart(cellrt2, true);
			col_calle.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 3
			col_calle.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_calle.SortColumnId = (int) Col_clientes.col_calle;
			
			TreeViewColumn col_colonia = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_colonia.Title = "Colonia";
			col_colonia.PackStart(cellrt3, true);
			col_colonia.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 4
			col_colonia.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_colonia.SortColumnId = (int) Col_clientes.col_colonia;
			
            TreeViewColumn col_codpos = new TreeViewColumn();
            CellRendererText cellrt4 = new CellRendererText();
            col_codpos.Title = "Codigo Postal";
            col_codpos.PackStart(cellrt4,true);
            col_codpos.AddAttribute(cellrt4,"text",4);// la siguiente columna será 5
            col_codpos.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_codpos.SortColumnId = (int) Col_clientes.col_codpos;
			
            TreeViewColumn col_municipio = new TreeViewColumn();
            CellRendererText cellrt5 = new CellRendererText();
            col_municipio.Title = "Municipio";
            col_municipio.PackStart(cellrt5, true);
			col_municipio.AddAttribute(cellrt5,"text",5);// la siguiente columna será 6
			col_municipio.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_municipio.SortColumnId = (int) Col_clientes.col_municipio;
			
            TreeViewColumn col_estado = new TreeViewColumn();
            CellRendererText cellrt6 = new CellRendererText();
            col_estado.Title = "Estado";
            col_estado.PackStart(cellrt6,true);
            col_estado.AddAttribute(cellrt6,"text",6);// la siguiente columna será 7
            col_estado.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_estado.SortColumnId = (int) Col_clientes.col_estado;
			
            TreeViewColumn col_tel_casa = new TreeViewColumn();
            CellRendererText cellrt7 = new CellRendererText();
            col_tel_casa.Title = "Tel. Casa";
            col_tel_casa.PackStart(cellrt7,true);
            col_tel_casa.AddAttribute(cellrt7,"text",7);// la siguiente columna será 8
            col_tel_casa.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
            col_tel_casa.SortColumnId = (int) Col_clientes.col_tel_casa;
			          
            TreeViewColumn col_tel_oficina = new TreeViewColumn();
            CellRendererText cellrt8 = new CellRendererText();
            col_tel_oficina.Title = "Tel. Of.";
            col_tel_oficina.PackStart(cellrt8,true);
            col_tel_oficina.AddAttribute(cellrt8,"text",8);// la siguiente columna será 9
            col_tel_oficina.SetCellDataFunc(cellrt8, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
            col_tel_oficina.SortColumnId = (int) Col_clientes.col_tel_oficina;
			
            TreeViewColumn col_celular = new TreeViewColumn();
            CellRendererText cellrt9 = new CellRendererText();
            col_celular.Title = "Celular";
            col_celular.PackStart(cellrt9,true);
            col_celular.AddAttribute(cellrt9,"text",9);// la siguiente columna será 10
            col_celular.SetCellDataFunc(cellrt9, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
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
			string clientesactivos = "";
			if(nomcatalogo == "cliente1"){
				clientesactivos = "AND cliente_activo = 'true' "; 
			}
			
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
								"cliente_activo,id_cliente,envio_factura "+
								"FROM osiris_erp_clientes,osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_clientes.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
								clientesactivos+
								"ORDER BY descripcion_cliente;";
				}else{
					comando.CommandText = "SELECT descripcion_cliente,direccion_cliente,rfc_cliente,curp_cliente, "+
								"colonia_cliente,municipio_cliente,estado_cliente,telefono1_cliente, "+ 
								"telefono2_cliente,celular_cliente,cp_cliente,"+
								"osiris_erp_clientes.id_forma_de_pago,descripcion_forma_de_pago AS descripago,"+ 
								"cliente_activo,id_cliente,envio_factura "+
								"FROM osiris_erp_clientes,osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_clientes.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
								clientesactivos+
								"AND descripcion_cliente LIKE '%"+(string) entry_expresion.Text.ToUpper()+"%' "+
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
													(string) lector["descripago"],//14
													(bool) lector["envio_factura"]);//15
					
					
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
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_busqueda.Selection.GetSelected(out model, out iterSelected)) 
 			{
 				int tomaidcliente = (int) model.GetValue(iterSelected, 0);
 				entry_id_cliente.Text = tomaidcliente.ToString();
				entry_nombre_cliente.Text = (string) model.GetValue(iterSelected, 1);
				entry_calle_cliente.Text = (string) model.GetValue(iterSelected, 2);
				entry_colonia_cliente.Text = (string) model.GetValue(iterSelected, 3);
				entry_CP_cliente.Text = (string) model.GetValue(iterSelected, 4);				
				municipios = (string) model.GetValue(iterSelected, 5);
				estado = (string) model.GetValue(iterSelected, 6);								
				entry_telcasa.Text = (string) model.GetValue(iterSelected,7);
				entry_teloficina.Text = (string) model.GetValue(iterSelected,8);
				entry_telcelular.Text = (string) model.GetValue(iterSelected,9);
				
				entry_rfc_cliente.Text = (string) model.GetValue(iterSelected,10);
				entry_curp_cliente.Text = (string) model.GetValue(iterSelected,11);
				
				checkbutton_cliente_activo.Active = (bool) model.GetValue(iterSelected,12);
				checkbutton_envio_facturas.Active = (bool) model.GetValue(iterSelected,15);;

				button_guardar.Sensitive = false;
				button_editar.Sensitive = true;
							
				llenado_estados("selecciona",estado,0);
				llenado_municipios("selecciona",municipios);
				llenado_formapago("selecciona",(int)model.GetValue(iterSelected,13),(string) model.GetValue(iterSelected,14));
				activa_campos(false,"CLIENTES");
				//activa_botones();
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
		
		void on_guarda_clientes(object sender, EventArgs args)
		{
			if(checkbutton_nuevo_cliente.Active == true) 
			{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
									ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
 				if (miResultado == ResponseType.Yes){
 					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
	    	        // Verifica que la base de datos este conectada
	    	        int numero_cliente;
					try
					{
 						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
	 					comando.CommandText = "SELECT * FROM osiris_erp_clientes WHERE rfc_cliente = '"+(string) entry_rfc_cliente.Text.Trim().ToUpper()+"';";
 						NpgsqlDataReader lector = comando.ExecuteReader ();
 						
						if (!lector.Read()){
 							NpgsqlConnection conexion1; 
							conexion1 = new NpgsqlConnection (connectionString+nombrebd);
		    	        	// Verifica que la base de datos este conectada
		    	        	try
							{
								conexion1.Open ();
								NpgsqlCommand comando1; 
								comando1 = conexion.CreateCommand ();
		 						comando1.CommandText = "INSERT INTO osiris_erp_clientes("+
		 											"descripcion_cliente,"+
		 											"direccion_cliente,"+
		 											"id_forma_de_pago,"+
		 											"rfc_cliente,"+
		 											"curp_cliente,"+
		 											"colonia_cliente,"+
		 											"municipio_cliente,"+
		 											"estado_cliente,"+
		 											"telefono1_cliente,"+ 
													"telefono2_cliente,"+
													"celular_cliente,"+
													"cp_cliente,"+
													"cliente_activo,"+
													"envio_factura,"+
													"id_quien_creo,"+
													"fechahora_creacion_cliente) "+
		 											"VALUES ('"+
		 											(string) entry_nombre_cliente.Text.Trim().ToUpper()+"','"+
		 											(string) entry_calle_cliente.Text.Trim().ToUpper()+"','"+
		 											idformadepago.ToString()+"','"+
		 											(string) entry_rfc_cliente.Text.Trim().ToUpper()+"','"+
		 											(string) entry_curp_cliente.Text.Trim().ToUpper()+"','"+
		 											(string) entry_colonia_cliente.Text.Trim().ToUpper()+"','"+
		 											(string) municipios.ToString().ToUpper()+"','"+
		 											(string) estado.ToString().ToUpper()+"','"+
		 											(string) entry_telcasa.Text.Trim()+"','"+
		 											(string) entry_teloficina.Text.Trim()+"','"+
		 											(string) entry_telcelular.Text.Trim()+"','"+
		 											(string) entry_CP_cliente.Text.Trim()+"','"+
		 											(bool) checkbutton_cliente_activo.Active+"','"+
		 											(bool) checkbutton_envio_facturas.Active+"','"+
		 											(string) LoginEmpleado+"','"+
		 											(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+
		 											"');";
		 						comando1.ExecuteNonQuery();    	    	       	comando1.Dispose();
		    	    	       	numero_cliente = int.Parse((string) lee_numero_cliente());
		    	    	       	checkbutton_nuevo_cliente.Active = false;
		    	    	       	button_editar.Sensitive = true;
		    	    	       	entry_id_cliente.Text = numero_cliente.ToString();
		    	    	       	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Info,ButtonsType.Close,"El cliente se guardo con exito");
								msgBoxError.Run ();					msgBoxError.Destroy();
							
								checkbutton_cliente_activo.Sensitive = false;
							}catch(NpgsqlException ex){
			   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();					msgBoxError.Destroy();
			       			}
		       				conexion1.Close ();
		       			}else{
		       				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Info,ButtonsType.Close,"El cliente ingresado ya EXISTE, verifique...");
							msgBoxError.Run ();					msgBoxError.Destroy();
		       			}
		       		}catch(NpgsqlException ex){
		   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();					msgBoxError.Destroy();
					}
	       			conexion.Close ();
	       			
       			}
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
									ButtonsType.YesNo,"¿ Desea Actualizar esta infomacion ?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();							msgBox.Destroy();
				
 				if (miResultado == ResponseType.Yes){
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
    	        	// Verifica que la base de datos este conectada
    	        	try
						{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
 						//TreeIter iter;
 						comando.CommandText = "UPDATE osiris_erp_clientes SET "+
 								"descripcion_cliente = '"+(string) entry_nombre_cliente.Text.Trim().ToUpper()+"', "+
 								"direccion_cliente = '"+(string) entry_calle_cliente.Text.Trim().ToUpper()+"', "+
 								"id_forma_de_pago = '"+ idformadepago.ToString().ToUpper()+"', "+
 								"rfc_cliente = '"+(string) entry_rfc_cliente.Text.Trim().ToUpper()+"', "+
 								"curp_cliente = '"+(string) entry_curp_cliente.Text.Trim().ToUpper()+"', "+
 								"colonia_cliente = '"+(string) entry_colonia_cliente.Text.Trim().ToUpper()+"', "+
 								"municipio_cliente = '"+(string) municipios.ToString().ToUpper()+"', "+
 								"estado_cliente = '"+(string) estado.ToString().ToUpper()+"', "+
 								"telefono1_cliente = '"+(string) entry_telcasa.Text.Trim()+"', "+ 
								"telefono2_cliente = '"+(string) entry_teloficina.Text.Trim()+"', "+
								"celular_cliente = '"+(string) entry_telcelular.Text.Trim()+"', "+
								"cp_cliente = '"+(string) entry_CP_cliente.Text.Trim()+"', "+
								"cliente_activo = '"+(bool) checkbutton_cliente_activo.Active+"', "+
								"envio_factura = '"+(bool) checkbutton_envio_facturas.Active+"', "+
								"id_quien_baja = '"+(string) LoginEmpleado+"', "+
								"historial_cambios = 	historial_cambios || '"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n' "+
								"WHERE id_cliente = '"+entry_id_cliente.Text.Trim()+"' ;";
 						comando.ExecuteNonQuery();    	    	       	comando.Dispose();
    	    	       	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info,ButtonsType.Close,"Los datos se actualizaron con exito");			
						msgBoxError.Run ();					msgBoxError.Destroy();
						
						entry_nombre_cliente.Sensitive = false;
						entry_rfc_cliente.Sensitive = false;
						entry_curp_cliente.Sensitive = false;
						entry_calle_cliente.Sensitive = false;
						entry_colonia_cliente.Sensitive = false;
						entry_CP_cliente.Sensitive = false;
						combobox_estado.Sensitive = false;
						combobox_formapago.Sensitive = false;
						combobox_municipios.Sensitive = false;
						entry_telcasa.Sensitive = false;
						entry_teloficina.Sensitive = false;
						entry_telcelular.Sensitive = false;
						checkbutton_cliente_activo.Sensitive = false;
						this.checkbutton_envio_facturas.Sensitive = false;				
						
					}catch(NpgsqlException ex){
	   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();					msgBoxError.Destroy();
	       			}
       				conexion.Close ();
       			}
			}
		}
		
		string lee_numero_cliente()
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
			string tomavalor = "";
			try
				{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(id_cliente,'99999999') AS idcliente "+ 
									"FROM osiris_erp_clientes "+
									"ORDER BY id_cliente DESC LIMIT 1;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//bool verifica_activacion;
				if (lector.Read()){	
					tomavalor =(string) lector["idcliente"]; 
					return tomavalor;
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										            MessageType.Error,ButtonsType.Close,"No se guardo correctamente el cliente");
					                                msgBoxError.Run ();			msgBoxError.Destroy();
					return tomavalor;
				}
			}catch (NpgsqlException ex)	{		   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
				return tomavalor;
			}
		}
		
		void on_checkbutton_nuevo_cliente_clicked (object sender, EventArgs args)
		{
			int numerocliente;
			if(checkbutton_nuevo_cliente.Active == true) { 
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
				MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de querer crear un nuevo cliente?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
	 			if (miResultado == ResponseType.Yes){
	 				llenado_estados("nuevo"," ",0);
	 				llenado_municipios("nuevo"," ");
					llenado_formapago("nuevo",0," ");
					limpia_textos("CLIENTES");
					activa_campos(true,"CLIENTES");
					numerocliente = int.Parse((string) lee_numero_cliente())+1;
					entry_id_cliente.Text = numerocliente.ToString();
					checkbutton_cliente_activo.Active = false;
					checkbutton_cliente_activo.Sensitive = true;
					button_editar.Sensitive = false;
					button_guardar.Sensitive = true;
				}else{
					checkbutton_nuevo_cliente.Active = false;
				}
			}
			if(checkbutton_nuevo_cliente.Active == false){ 
				activa_campos(false,"CLIENTES");
			}
		}
			
		void limpia_textos(string tipos)
		{
			if (tipos=="CLIENTES"){
				entry_nombre_cliente.Text = "";
				entry_rfc_cliente.Text = "";
				entry_curp_cliente.Text = "";
				entry_calle_cliente.Text = "";
				entry_colonia_cliente.Text = "";
				entry_CP_cliente.Text = "";
				entry_telcasa.Text = "";
				entry_teloficina.Text = "";
				entry_telcelular.Text = "";		
			}
			
			if (tipos=="PROVEEDORES"){
				entry_id_proveedor.Text = "";
				entry_proveedor.Text = "";
				entry_rfc_proveedor.Text = "";
				entry_curp_proveedor.Text = "";
				entry_calle_proveedor.Text = "";
				entry_colonia_proveedor.Text = "";
				entry_cp_proveedor.Text = "";
				entry_telefono_proveedor.Text = "";
				entry_fax_proveedor.Text = "";
				entry_celular_proveedor.Text = "";
				entry_nextel_proveedor.Text = "";
				entry_contacto_proveedor.Text = "";
				entry_correo_proveedor.Text = "";
				entry_pagina_web_proveedor.Text = "";
			}
		}
		
		void activa_campos(bool valor, string tipos)
		{	 
			if (tipos=="CLIENTES"){
				entry_nombre_cliente.Sensitive = valor;
				entry_rfc_cliente.Sensitive = valor;
				entry_curp_cliente.Sensitive = valor;
				entry_calle_cliente.Sensitive = valor;
				entry_colonia_cliente.Sensitive = valor;
				entry_CP_cliente.Sensitive = valor;
				combobox_estado.Sensitive = valor;
				combobox_municipios.Sensitive = valor;
				combobox_formapago.Sensitive = valor;
				entry_telcasa.Sensitive = valor;
				entry_teloficina.Sensitive = valor;
				entry_telcelular.Sensitive = valor;
				this.checkbutton_cliente_activo.Sensitive = valor;
			}
			if (tipos=="PROVEEDORES"){		
				entry_id_proveedor.Sensitive = valor;
				checkbutton_proveedor_activo.Sensitive = valor;
				combobox_estado.Sensitive = valor;
				combobox_formapago.Sensitive = valor;
				combobox_municipios.Sensitive = valor;
				entry_id_proveedor.Sensitive = valor;
				entry_proveedor.Sensitive = valor;
				entry_rfc_proveedor.Sensitive = valor;
				entry_curp_proveedor.Sensitive = valor;
				entry_calle_proveedor.Sensitive = valor;
				entry_colonia_proveedor.Sensitive = valor;
				entry_cp_proveedor.Sensitive = valor;
				entry_telefono_proveedor.Sensitive = valor;
				entry_fax_proveedor.Sensitive = valor;
				entry_celular_proveedor.Sensitive = valor;
				entry_nextel_proveedor.Sensitive = valor;
				entry_contacto_proveedor.Sensitive = valor;
				entry_correo_proveedor.Sensitive = valor;
				entry_pagina_web_proveedor.Sensitive = valor;
			}
		}	
			
		void llenado_formapago(string tipo_,int idformapago_, string descrippago_ )
		{
			combobox_formapago.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_formapago.PackStart(cell3, true);
			combobox_formapago.AddAttribute(cell3,"text",0);
	        
			ListStore store5 = new ListStore( typeof (string), typeof (int));
			combobox_formapago.Model = store5;
			if(tipo_ == "selecciona"){
				store5.AppendValues ( (string) descrippago_,(int) idformapago_ );
			}
	      
	        NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_erp_forma_de_pago "+
               						"ORDER BY descripcion_forma_de_pago;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read())
				{
					store5.AppendValues ((string) lector["descripcion_forma_de_pago"],
									 	(int) lector["id_forma_de_pago"] );
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
	        
			TreeIter iter5;
			if (store5.GetIterFirst(out iter5)){
				combobox_formapago.SetActiveIter (iter5);
			}
			combobox_formapago.Changed += new EventHandler (onComboBoxChanged_formapago);
		}
		
		
		void llenado_municipios(string tipo_, string descripcionmunicipio_)
		{
			combobox_municipios.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_municipios.PackStart(cell3, true);
			combobox_municipios.AddAttribute(cell3,"text",0);
	        
			ListStore store3 = new ListStore( typeof (string));
			combobox_municipios.Model = store3;
			
			if (tipo_ == "selecciona"){
				store3.AppendValues ((string) descripcionmunicipio_);
			}
	        NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT descripcion_municipio FROM osiris_municipios WHERE id_estado = '"+idestado.ToString()+"' "+
               						"ORDER BY descripcion_municipio;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read())
				{
					store3.AppendValues ((string) lector["descripcion_municipio"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
	        
			TreeIter iter3;
			if (store3.GetIterFirst(out iter3))	{ combobox_municipios.SetActiveIter (iter3); }
			combobox_municipios.Changed += new EventHandler (onComboBoxChanged_municipios);
		}
		
		void llenado_estados(string tipo_, string descripcionestado_, int idestado_)
		{
			combobox_estado.Clear();
			CellRendererText cell4 = new CellRendererText();
			combobox_estado.PackStart(cell4, true);
			combobox_estado.AddAttribute(cell4,"text",0);
	        
			ListStore store4 = new ListStore( typeof (string),typeof (int));
			combobox_estado.Model = store4;
			if (tipo_ == "selecciona"){
				store4.AppendValues ((string) descripcionestado_, (int) idestado_);
			}
	       	NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_estados ORDER BY descripcion_estado;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read()){
					store4.AppendValues ((string) lector["descripcion_estado"], (int) lector["id_estado"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
	      	        	        
			TreeIter iter4;
			if (store4.GetIterFirst(out iter4)){
				combobox_estado.SetActiveIter (iter4);
			}
			combobox_estado.Changed += new EventHandler (onComboBoxChanged_estado);
		}
		
		void onComboBoxChanged_municipios (object sender, EventArgs args)
		{
			ComboBox combobox_municipios = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_municipios.GetActiveIter (out iter)){
				municipios = (string) combobox_municipios.Model.GetValue(iter,0);
			}
		}
	    
		void onComboBoxChanged_estado (object sender, EventArgs args)
		{
			ComboBox combobox_estado = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_estado.GetActiveIter (out iter)) {	
				estado = (string) combobox_estado.Model.GetValue(iter,0); 
				idestado = (int) combobox_estado.Model.GetValue(iter,1);
				llenado_municipios("nuevo"," ");
			}
		}
		
		void onComboBoxChanged_formapago (object sender, EventArgs args)
		{
			ComboBox combobox_formapago = sender as ComboBox;
			if (sender == null) {return;}
			TreeIter iter;
			if (combobox_formapago.GetActiveIter (out iter)){ 
				idformadepago = (int) combobox_formapago.Model.GetValue(iter,1);
			}
		}
		
		void cambia_colores_cliente(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((bool) this.lista_de_busqueda.Model.GetValue (iter,12) == false){
				(cell as Gtk.CellRendererText).Foreground = "red";
			}else{			
				(cell as Gtk.CellRendererText).Foreground = "black";
			}
		}
		
		void on_editar_clientes_clicked (object sender, EventArgs args)
		{
			entry_nombre_cliente.Sensitive = true;
			entry_rfc_cliente.Sensitive = true;
			entry_curp_cliente.Sensitive = true;
			checkbutton_cliente_activo.Sensitive = true;
			checkbutton_nuevo_cliente.Sensitive = true;
			entry_calle_cliente.Sensitive = true;
			entry_colonia_cliente.Sensitive = true;
			entry_CP_cliente.Sensitive = true;
			combobox_estado.Sensitive = true;
			combobox_municipios.Sensitive = true;
			combobox_formapago.Sensitive = true;
			entry_telcasa.Sensitive = true;
			entry_teloficina.Sensitive = true;
			entry_telcelular.Sensitive = true;
			button_guardar.Sensitive = true;					
		}
		
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////VENTANA DE CATALOGO DE MEDICOS///////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		void catalogo_medicos()
		{
				Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "catalogo_medico", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        //Muestra ventana de Glade
			catalogo_medico.Show();
			entry_id_medico.IsEditable = false;
			entry_fecha_ingreso.IsEditable = false;
			entry_fecha_revision.IsEditable = false;
			entry_empresa.IsEditable = false;
			entry_especialidad.IsEditable = false;
			button_buscar_medico.Clicked += new EventHandler(on_button_buscar_medico_clicked);
			button_actualizar.Sensitive = false;
			button_actualizar.Clicked += new EventHandler(on_button_actualizar_clicked);
			checkbutton_nuevo_medico.Clicked += new EventHandler(on_checkbutton_nuevo_medico_clicked);
			button_guardar.Clicked += new EventHandler(on_button_guardar_clicked);
			button_autorizacion_medico.Sensitive = false;
			button_buscar_especialidad.Clicked += new EventHandler(on_button_buscar_especialidad_clicked);
			button_buscar_empresa.Clicked += new EventHandler(on_button_buscar_empresa_clicked);
			if(LoginEmpleado == "N000100") {button_autorizacion_medico.Sensitive = true; }
			button_autorizacion_medico.Clicked += new EventHandler(on_button_autorizacion_medico_clicked);
			//Desactivacion de las entradas
			activacion_entrys(false);
			sensibilidad_checkbutton(false);
			activacion_botones(false);
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
		}
		
		void on_button_guardar_clicked(object sender, EventArgs args)
		{
			bool sepuedegrabar = validando_campos();
			if((bool) sepuedegrabar == false){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
								ButtonsType.Close,"Verifique que el nombre o la especialidad no \n"+
								" esten vacios y por lo menos seleccione un dato del area obligatoria");
					msgBox.Run ();		msgBox.Destroy();
			}else{
				if(checkbutton_nuevo_medico.Active == true) { 
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
										ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");
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
	 						//TreeIter iter;
	 						comando.CommandText = "INSERT INTO osiris_his_medicos("+
	 										"nombre1_medico,"+//1
											"nombre2_medico,"+//2
											"apellido_paterno_medico,"+//3
											"apellido_materno_medico,"+//4
											"cedula_medico,"+//5
											"telefono1_medico,"+//6
											"telefono2_medico,"+//7
											"celular1_medico,"+//8
											"celular2_medico,"+//9
											"nextel_medico,"+//10
											"beeper_medico,"+//11
											"id_especialidad,"+//12
											"id_empresa_medico,"+//13
											"fecha_ingreso_medico,"+//14
											"direccion_medico,"+//15
											"direccion_consultorio_medico,"+//16
											"titulo_profesional_medico,"+//17
											"cedula_profecional_medico,"+//18
											"diploma_especialidad_medico,"+ //19
											"diploma_subespecialidad_medico,"+//20
											"copia_identificacion_oficial_medico,"+//21
											"copia_cedula_rfc_medico,"+ //22
											"diploma_cursos_adiestramiento_medico,"+//23
											"certificacion_recertificacion_consejo_subespecialidad_medico,"+//24
											"copia_comprobante_domicilio_medico,"+//25
											"diploma_seminarios_medico,"+//26
											"diploma_cursos_medico,"+//27
											"diplomas_extranjeros_medico,"+//28
											"constancia_congresos_medico,"+//29
											"cedula_especialidad_medico,"+//30
											"medico_activo,"+//31
											"centro_medico,"+//32
											"id_quien_creo_medico,"+//33
											"nombre_medico ) "+//34
	 										"VALUES ('"+
											(string) entry_nombre1_medico.Text.Trim().ToUpper()+"','"+//1
											(string) entry_nombre2_medico.Text.Trim().ToUpper()+"','"+//2
											(string) entry_apellido_paterno_medico.Text.Trim().ToUpper()+"','"+//3
											(string) entry_apellido_materno_medico.Text.Trim().ToUpper()+"','"+//4
											(string) entry_cedula_profecional.Text.Trim().ToUpper()+"','"+//5
											(string) entry_telcasa_medico.Text.Trim().ToUpper()+"','"+//6
											(string) entry_teloficina_medico.Text.Trim().ToUpper()+"','"+//7
											(string) entry_celular1_medico.Text.Trim().ToUpper()+"','"+//8
											(string) entry_celular2_medico.Text.Trim().ToUpper()+"','"+//9
											(string) entry_nextel_medico.Text.Trim().ToUpper()+"','"+//10
											(string) entry_beeper_medico.Text.Trim().ToUpper()+"','"+//11
											(int) id_esp_medico+"','"+//12
											(int) id_emp_medico+"','"+//13
											(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+//14
											(string) entry_direccion_casa_medico.Text.Trim().ToUpper()+"','"+//15
											(string) entry_direccion_consultorio_medico.Text.Trim().ToUpper()+"','"+//16
											(bool) checkbutton_tituloprof_medico.Active +"','"+//17
											(bool) checkbutton_cedula_prof_medico.Active+"','"+//18
											(bool) checkbutton_diploespecial_medico.Active+"','"+//19
											(bool) checkbutton_diplomasubespecial.Active+"','"+//20
											(bool) checkbutton_copiaidentificacionoficial.Active+"','"+//21
											(bool) checkbutton_copiacedularfc.Active+"','"+//22
											(bool) checkbutton_cursoadistramiento_medico.Active+"','"+//23
											(bool) checkbutton_certificadoconsejo_esp.Active+"','"+//24
											(bool) checkbutton_copia_comprobante_domicilio.Active+"','"+//25
											(bool) checkbutton_diploseminarios.Active+"','"+//26
											(bool) checkbutton_diplomacursos.Active+"','"+//27
											(bool) checkbutton_diplomaextranjero.Active+"','"+//28
											(bool) checkbutton_constancia_congresos.Active+"','"+//29
											(bool) checkbutton_cedula_especialidad_medico.Active+"','"+//30
											(bool) checkbutton_medico_activo.Active+"','"+//31
											(bool) checkbutton_centro_medico.Active+"','"+//32
											(string) LoginEmpleado +"','"+//33
											(string) entry_nombre1_medico.Text.Trim().ToUpper()+" "+(string) entry_nombre2_medico.Text.Trim().ToUpper()+" "+
											(string) entry_apellido_paterno_medico.Text.Trim().ToUpper()+" "+(string) entry_apellido_materno_medico.Text.Trim().ToUpper()+"' "+
											" );";
							comando.ExecuteNonQuery();    	    	       	comando.Dispose();
	    	    	       	checkbutton_nuevo_medico.Active = false;
	    	    	       	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Info,ButtonsType.Close,"El medico se guardo con exito");
							msgBoxError.Run ();					msgBoxError.Destroy();
							entrys_a_blanco();
							activacion_entrys(false);
							sensibilidad_checkbutton(false);
							activacion_checkbutton(false);
							activacion_botones(false);
						}catch(NpgsqlException ex){
		   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();					msgBoxError.Destroy();
		       			}
	       				conexion.Close ();
	       			}
				}else{///si el checkbutton esta desactivado hace esto:
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
									ButtonsType.YesNo,"¿ Desea Actualizar esta infomacion ?");
					ResponseType miResultado = (ResponseType)
					msgBox.Run ();							msgBox.Destroy();
					if (miResultado == ResponseType.Yes){
						NpgsqlConnection conexion; 
						conexion = new NpgsqlConnection (connectionString+nombrebd);
	    	        	// Verifica que la base de datos este conectada
	    	        	try{
							conexion.Open ();
							NpgsqlCommand comando; 
							comando = conexion.CreateCommand ();
	 						//TreeIter iter;
	 						comando.CommandText = "UPDATE osiris_his_medicos SET "+
	 								"nombre1_medico = '"+(string) entry_nombre1_medico.Text.Trim().ToUpper()+"',"+//1
									"nombre2_medico = '"+(string) entry_nombre2_medico.Text.Trim().ToUpper()+"',"+//2
									"apellido_paterno_medico = '"+(string) entry_apellido_paterno_medico.Text.Trim().ToUpper()+"',"+//3
									"apellido_materno_medico = '"+(string) entry_apellido_materno_medico.Text.Trim().ToUpper()+"',"+//4
									"cedula_medico = '"+(string) entry_cedula_profecional.Text.Trim().ToUpper()+"',"+//5
									"telefono1_medico = '"+(string) entry_telcasa_medico.Text.Trim().ToUpper()+"',"+//6
									"telefono2_medico = '"+(string) entry_teloficina_medico.Text.Trim().ToUpper()+"',"+//7
									"celular1_medico = '"+(string) entry_celular1_medico.Text.Trim().ToUpper()+"',"+//8
									"celular2_medico = '"+(string) entry_celular2_medico.Text.Trim().ToUpper()+"',"+//9
									"nextel_medico = '"+(string) entry_nextel_medico.Text.Trim().ToUpper()+"',"+//10
									"beeper_medico = '"+(string) entry_beeper_medico.Text.Trim().ToUpper()+"',"+//11
									"id_especialidad = '"+(int) id_esp_medico+"',"+//12
									"id_empresa_medico = '"+(int) id_emp_medico+"',"+//13
									"direccion_medico = '"+(string) entry_direccion_casa_medico.Text.Trim().ToUpper()+"',"+//15
									"direccion_consultorio_medico = '"+(string) entry_direccion_consultorio_medico.Text.Trim().ToUpper()+"',"+//16
									"titulo_profesional_medico = '"+(bool) checkbutton_tituloprof_medico.Active+"',"+//17
									"cedula_profecional_medico = '"+(bool) checkbutton_cedula_prof_medico.Active+"',"+//18
									"diploma_especialidad_medico = '"+(bool) checkbutton_diploespecial_medico.Active+"',"+//19
									"diploma_subespecialidad_medico = '"+(bool) checkbutton_diplomasubespecial.Active+"',"+//20
									"copia_identificacion_oficial_medico = '"+(bool) checkbutton_copiaidentificacionoficial.Active+"',"+//21
									"copia_cedula_rfc_medico = '"+ (bool) checkbutton_copiacedularfc.Active+"',"+//22
									"diploma_cursos_adiestramiento_medico = '"+(bool) checkbutton_cursoadistramiento_medico.Active+"',"+//23
									"certificacion_recertificacion_consejo_subespecialidad_medico = '"+(bool) checkbutton_certificadoconsejo_esp.Active+"',"+//24
									"copia_comprobante_domicilio_medico = '"+(bool) checkbutton_copia_comprobante_domicilio.Active+"',"+//25
									"diploma_seminarios_medico = '"+(bool) checkbutton_diploseminarios.Active+"',"+//26
									"diploma_cursos_medico = '"+(bool) checkbutton_diplomacursos.Active+"',"+//27
									"diplomas_extranjeros_medico = '"+(bool) checkbutton_diplomaextranjero.Active+"',"+//28
									"constancia_congresos_medico = '"+(bool) checkbutton_constancia_congresos.Active+"',"+//29
									"cedula_especialidad_medico = '"+(bool) checkbutton_cedula_especialidad_medico.Active+"',"+//30
									"medico_activo = '"+(bool) checkbutton_medico_activo.Active+"',"+//31
									"centro_medico = '"+(bool) checkbutton_centro_medico.Active+"',"+//32
									"nombre_medico = '"+(string) entry_nombre1_medico.Text.Trim().ToUpper()+" "+(string) entry_nombre2_medico.Text.Trim().ToUpper()+" "+
											(string) entry_apellido_paterno_medico.Text.Trim().ToUpper()+" "+(string) entry_apellido_materno_medico.Text.Trim().ToUpper()+"', "+
									"historial_de_revision = historial_de_revision || '"+LoginEmpleado+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+" ACTUALIZACION \n' "+
									"WHERE id_medico = '"+entry_id_medico.Text.Trim()+"' ;";							 						
	 						comando.ExecuteNonQuery();    	    	       	comando.Dispose();
	    	    	       	checkbutton_nuevo_medico.Active = false;
	    	    	       	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Info,ButtonsType.Close,"Los datos se actualizaron con exito");
							msgBoxError.Run ();					msgBoxError.Destroy();
							entrys_a_blanco();
							activacion_entrys(false);
							sensibilidad_checkbutton(false);
							activacion_checkbutton(false);
							activacion_botones(false);
							actualizacion = false;
						}catch(NpgsqlException ex){
		   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();					msgBoxError.Destroy();
		       			}
	       				conexion.Close ();
	       			}//termino del else de actualizacion
	       		}//termino de validacion de grabado y o actualizado
			}
		}
		
		bool validando_campos()
		{
			if(entry_nombre1_medico.Text.Trim() == "" || entry_apellido_paterno_medico.Text.Trim() == "" ||
			   entry_apellido_materno_medico.Text.Trim() == "" )
			   { return false; 
			  		}else{
						if( checkbutton_tituloprof_medico.Active == true || checkbutton_cedula_prof_medico.Active == true||
						checkbutton_diploespecial_medico.Active == true || checkbutton_cursoadistramiento_medico.Active == true) 
				{ return true; }else{ return false; }
			}
		}
				
		void on_button_buscar_medico_clicked(object sender, EventArgs args)
		{
			busqueda = "medicos";
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador_medicos", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        llenado_cmbox_tipo_busqueda();
	        entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_medicos);
			button_selecciona.Clicked += new EventHandler(on_selecciona_medico);
	        button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
			
			treeViewEngineMedicos = new TreeStore( typeof(int),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(bool),typeof(bool),typeof(bool),typeof(bool),
												typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),
												typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),
												typeof(string));
			lista_de_medicos.Model = treeViewEngineMedicos;
			lista_de_medicos.RulesHint = true;
				
			lista_de_medicos.RowActivated += on_selecciona_medico;  // Doble click selecciono paciente*/
				
			col_idmedico = new TreeViewColumn();
			cellr0 = new CellRendererText();
			col_idmedico.Title = "ID Medico"; // titulo de la cabecera de la columna, si está visible
			col_idmedico.PackStart(cellr0, true);
			col_idmedico.AddAttribute (cellr0, "text", 0);
			col_idmedico.SortColumnId = (int) Coldatos_medicos.col_idmedico;    
            
			col_nomb1medico = new TreeViewColumn();
			cellrt1 = new CellRendererText();
			col_nomb1medico.Title = "1º Nombre";
			col_nomb1medico.PackStart(cellrt1, true);
			col_nomb1medico.AddAttribute (cellrt1, "text", 1);
			col_nomb1medico.SortColumnId = (int) Coldatos_medicos.col_nomb1medico; 
            
            col_nomb2medico = new TreeViewColumn();
			cellrt2 = new CellRendererText();
			col_nomb2medico.Title = "2º Nombre";
			col_nomb2medico.PackStart(cellrt2, true);
			col_nomb2medico.AddAttribute (cellrt2, "text", 2);
			col_nomb2medico.SortColumnId = (int) Coldatos_medicos.col_nomb2medico; 
			
			col_appmedico = new TreeViewColumn();
			cellrt3 = new CellRendererText();
			col_appmedico.Title = "Apellido Paterno";
			col_appmedico.PackStart(cellrt3, true);
			col_appmedico.AddAttribute (cellrt3, "text", 3);
			col_appmedico.SortColumnId = (int) Coldatos_medicos.col_appmedico;
			
			col_apmmedico = new TreeViewColumn();
			cellrt4 = new CellRendererText();
			col_apmmedico.Title = "Apellido Materno";
			col_apmmedico.PackStart(cellrt4, true);
			col_apmmedico.AddAttribute (cellrt4, "text", 4);
			col_apmmedico.SortColumnId = (int) Coldatos_medicos.col_apmmedico;
            
			col_espemedico = new TreeViewColumn();
			cellrt5 = new CellRendererText();
			col_espemedico.Title = "Especialidad";
			col_espemedico.PackStart(cellrt5, true);
			col_espemedico.AddAttribute (cellrt5, "text", 5);
			col_espemedico.SortColumnId = (int) Coldatos_medicos.col_espemedico;
            
			col_telmedico = new TreeViewColumn();
			cellrt6 = new CellRendererText();
			col_telmedico.Title = "Cedula Medica";
			col_telmedico.PackStart(cellrt6, true);
			col_telmedico.AddAttribute (cellrt6, "text", 6); 
			col_telmedico.SortColumnId = (int) Coldatos_medicos.col_telmedico;
            
			col_cedulamedico = new TreeViewColumn();
			cellrt7 = new CellRendererText();
			col_cedulamedico.Title = "Telefono Casa";
			col_cedulamedico.PackStart(cellrt7, true);
			col_cedulamedico.AddAttribute (cellrt7, "text", 7); 
			col_cedulamedico.SortColumnId = (int) Coldatos_medicos.col_cedulamedico;
			
			col_telOfmedico = new TreeViewColumn();
			cellrt8 = new CellRendererText();
			col_telOfmedico.Title = "Telefono Oficina";
			col_telOfmedico.PackStart(cellrt8, true);
			col_telOfmedico.AddAttribute (cellrt8, "text", 8);
			col_telOfmedico.SortColumnId = (int) Coldatos_medicos.col_telOfmedico; 
			
			col_celmedico = new TreeViewColumn();
			cellrt9 = new CellRendererText();
			col_celmedico.Title = "Celular 1";
			col_celmedico.PackStart(cellrt9, true);
			col_celmedico.AddAttribute (cellrt9, "text", 9); 
			col_celmedico.SortColumnId = (int) Coldatos_medicos.col_celmedico;
			
			col_celmedico2 = new TreeViewColumn();
			cellrt10 = new CellRendererText();
			col_celmedico2.Title = "Celular 2";
			col_celmedico2.PackStart(cellrt10, true);
			col_celmedico2.AddAttribute (cellrt10, "text", 10);
			col_celmedico2.SortColumnId = (int) Coldatos_medicos.col_celmedico2;
									
			col_nextelmedico = new TreeViewColumn();
			cellrt11 = new CellRendererText();
			col_nextelmedico.Title = "Nextel";
			col_nextelmedico.PackStart(cellrt11, true);
			col_nextelmedico.AddAttribute (cellrt11, "text", 11);
			col_nextelmedico.SortColumnId = (int) Coldatos_medicos.col_nextelmedico;
			
			col_beepermedico = new TreeViewColumn();
			cellrt12 = new CellRendererText();
			col_beepermedico.Title = "Beeper";
			col_beepermedico.PackStart(cellrt12, true);
			col_beepermedico.AddAttribute (cellrt12, "text", 12);
			col_beepermedico.SortColumnId = (int) Coldatos_medicos.col_beepermedico;
			
			col_empresamedico = new TreeViewColumn();
			cellrt13 = new CellRendererText();
			col_empresamedico.Title = "Empresa";
			col_empresamedico.PackStart(cellrt13, true);
			col_empresamedico.AddAttribute (cellrt13, "text", 13);
			col_empresamedico.SortColumnId = (int) Coldatos_medicos.col_empresamedico;
			
			col_estadomedico = new TreeViewColumn();
			cellrt37 = new CellRendererText();
			col_estadomedico.Title = "Estado";
			col_estadomedico.PackStart(cellrt37, true);
			col_estadomedico.AddAttribute (cellrt37, "text", 37);
			col_estadomedico.SortColumnId = (int) Coldatos_medicos.col_estadomedico;
			      
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
			lista_de_medicos.AppendColumn(col_estadomedico);
		}
		
		enum Coldatos_medicos
		{
			col_idmedico,			col_nomb1medico,
			col_nomb2medico,		col_appmedico,
			col_apmmedico,			col_espemedico,
			col_cedulamedico,		col_telmedico,
			col_telOfmedico,		col_celmedico,
			col_celmedico2,			col_nextelmedico,
			col_beepermedico,		col_empresamedico,
			col_estadomedico
		}
		
		
		void on_llena_lista_medicos(object sender, EventArgs args)
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
										"to_char(osiris_his_tipo_especialidad.id_especialidad,999999) AS idespecialidad, "+
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
										"ORDER BY id_medico;";
						}else{
							comando.CommandText = "SELECT id_medico, "+
										"to_char(id_empresa,'999999') AS idempresa, "+
										"to_char(osiris_his_tipo_especialidad.id_especialidad,999999) AS idespecialidad, "+
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
								  		tipobusqueda+(string) entry_expresion.Text.Trim().ToUpper()+"%' "+
										"ORDER BY id_medico;";
						}
						NpgsqlDataReader lector = comando.ExecuteReader ();
						string estado_medico = "NO AUTORIZADO";
						while (lector.Read())
						{
							if((bool) lector["autorizado"] == false) { estado_medico = "NO AUTORIZADO"; }
							if((bool) lector["autorizado"] == true) { estado_medico = "AUTORIZADO"; }
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
										(bool) lector["autorizado"],//36
										(string) estado_medico);//37
							//cambio los colores de las filas dependiendo si el medico esta activo o no
							col_idmedico.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_fila));			
							col_nomb1medico.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_nomb2medico.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_appmedico.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_apmmedico.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_espemedico.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_telmedico.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_cedulamedico.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_fila));		
							col_telOfmedico.SetCellDataFunc(cellrt8, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_celmedico.SetCellDataFunc(cellrt9, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_celmedico2.SetCellDataFunc(cellrt10, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_nextelmedico.SetCellDataFunc(cellrt11, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_beepermedico.SetCellDataFunc(cellrt12, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_empresamedico.SetCellDataFunc(cellrt13, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							col_estadomedico.SetCellDataFunc(cellrt37, new Gtk.TreeCellDataFunc(cambia_colores_fila));
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
			if(numbusqueda == 1)  { tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";	}
			if(numbusqueda == 2)  { tipobusqueda = "AND osiris_his_medicos.nombre2_medico LIKE '";	}
			if(numbusqueda == 3)  { tipobusqueda = "AND osiris_his_medicos.apellido_paterno_medico LIKE '";	}
			if(numbusqueda == 4)  { tipobusqueda = "AND osiris_his_medicos.apellido_materno_medico LIKE '";	}
			if(numbusqueda == 5)  { tipobusqueda = "AND osiris_his_medicos.cedula_medico LIKE '";	}
			if(numbusqueda == 6)  { tipobusqueda = "AND osiris_his_tipo_especialidad.descripcion_especialidad LIKE '";	}
			if(numbusqueda == 7)  { tipobusqueda = "AND osiris_his_medicos.id_medico LIKE '";	}
		}
		
		void on_selecciona_medico(object sender, EventArgs args) 
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_medicos.Selection.GetSelected(out model, out iterSelected)) 
 			{
 				int id_medico =(int) model.GetValue(iterSelected, 0);
 				entry_id_medico.Text = id_medico.ToString();
				entry_nombre1_medico.Text= (string) model.GetValue(iterSelected, 1);
				entry_nombre2_medico.Text= (string) model.GetValue(iterSelected, 2);
				entry_apellido_paterno_medico.Text = (string) model.GetValue(iterSelected, 3);
				entry_apellido_materno_medico.Text = (string) model.GetValue(iterSelected, 4);
			  	entry_especialidad.Text = (string) model.GetValue(iterSelected, 5);
				entry_cedula_profecional.Text = (string) model.GetValue(iterSelected, 6);
				entry_telcasa_medico.Text = (string) model.GetValue(iterSelected, 7);
				entry_teloficina_medico.Text = (string) model.GetValue(iterSelected,8);
				entry_celular1_medico.Text = (string) model.GetValue(iterSelected,9);
				entry_celular2_medico.Text = (string) model.GetValue(iterSelected,10);
				entry_nextel_medico.Text = (string) model.GetValue(iterSelected,11);
				entry_beeper_medico.Text = (string) model.GetValue(iterSelected,12);
				entry_empresa.Text = (string) model.GetValue(iterSelected,13);
				id_esp_medico = int.Parse((string) model.GetValue(iterSelected,14));
				id_emp_medico = int.Parse((string) model.GetValue(iterSelected,15));
				if((string) model.GetValue(iterSelected,16) == "02-01-2000") { entry_fecha_ingreso.Text = "" ; }else{ entry_fecha_ingreso.Text = (string) model.GetValue(iterSelected,16);}
				if((string) model.GetValue(iterSelected,17) == "02-01-2000") { entry_fecha_revision.Text = "" ; }else{ entry_fecha_revision.Text = (string) model.GetValue(iterSelected,17);}
				entry_direccion_casa_medico.Text = (string) model.GetValue(iterSelected,18);
				entry_direccion_consultorio_medico.Text = (string) model.GetValue(iterSelected,19);
				checkbutton_tituloprof_medico.Active = (bool) model.GetValue(iterSelected,20);
				checkbutton_cedula_prof_medico.Active = (bool) model.GetValue(iterSelected,21);
				checkbutton_diploespecial_medico.Active = (bool) model.GetValue(iterSelected,22);
				checkbutton_diplomasubespecial.Active = (bool) model.GetValue(iterSelected,23);
				checkbutton_copiaidentificacionoficial.Active = (bool) model.GetValue(iterSelected,24);
				checkbutton_copiacedularfc.Active = (bool) model.GetValue(iterSelected,25);
				checkbutton_cursoadistramiento_medico.Active = (bool) model.GetValue(iterSelected,26);
				checkbutton_certificadoconsejo_esp.Active = (bool) model.GetValue(iterSelected,27);
				checkbutton_copia_comprobante_domicilio.Active = (bool) model.GetValue(iterSelected,28);
				checkbutton_diploseminarios.Active = (bool) model.GetValue(iterSelected,29);
				checkbutton_diplomacursos.Active = (bool) model.GetValue(iterSelected,30);
				checkbutton_diplomaextranjero.Active = (bool) model.GetValue(iterSelected,31);
				checkbutton_constancia_congresos.Active = (bool) model.GetValue(iterSelected,32);
				checkbutton_cedula_especialidad_medico.Active = (bool) model.GetValue(iterSelected,33);
				checkbutton_medico_activo.Active = (bool) model.GetValue(iterSelected,34);
				checkbutton_centro_medico.Active = (bool) model.GetValue(iterSelected,35); 
				checkbutton_medico_provisional.Active = !(bool) model.GetValue(iterSelected,36);
				
				activacion_entrys(false);
				sensibilidad_checkbutton(false);
				activacion_botones(false);
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
		
		void on_button_buscar_especialidad_clicked(object sender, EventArgs args)
		{
			busqueda = "especialidad";
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_especialidad);
			button_selecciona.Clicked += new EventHandler(on_selecciona_especialidad);
	        button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
			
			treeViewEngineEspecialidad = new TreeStore( typeof(int),typeof(string));
								
			lista_de_busqueda.Model = treeViewEngineEspecialidad;
			lista_de_busqueda.RulesHint = true;
				
			lista_de_busqueda.RowActivated += on_selecciona_especialidad;  // Doble click selecciono paciente*/
				
			TreeViewColumn col_idespecialidad = new TreeViewColumn();
			cellr0 = new CellRendererText();
			col_idespecialidad.Title = "ID Especialidad"; // titulo de la cabecera de la columna, si está visible
			col_idespecialidad.PackStart(cellr0, true);
			col_idespecialidad.AddAttribute (cellr0, "text", 0);
			col_idespecialidad.SortColumnId = (int) Column_especialidad.col_idespecialidad;   
			
			TreeViewColumn col_especialidad = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_especialidad.Title = "Especialidad";
			col_especialidad.PackStart(cellr1,true);
			col_especialidad.AddAttribute(cellr1, "text", 1);
			col_especialidad.SortColumnId = (int) Column_especialidad.col_especialidad;
			
			lista_de_busqueda.AppendColumn(col_idespecialidad);
			lista_de_busqueda.AppendColumn(col_especialidad);
		}
		
		enum Column_especialidad {	col_idespecialidad,	col_especialidad	}
		
		void on_llena_lista_especialidad(object sender, EventArgs args)
		{
			llenando_lista_de_especialidad();
		}
		
		void llenando_lista_de_especialidad()
		{
			//TreeIter iter;
			treeViewEngineEspecialidad.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				if ((string) entry_expresion.Text.ToUpper().Trim() == "")
				{
					comando.CommandText = "SELECT id_especialidad,descripcion_especialidad "+
										" FROM osiris_his_tipo_especialidad "+
										"ORDER BY id_especialidad;";
				}else{
					comando.CommandText = "SELECT id_especialidad,descripcion_especialidad "+
										" FROM osiris_his_tipo_especialidad "+
										"WHERE descripcion_especialidad LIKE '"+entry_expresion.Text.ToUpper().Trim()+"%' ;"+
										"ORDER BY id_especialidad;";
				}						
					
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read())
				{
					treeViewEngineEspecialidad.AppendValues ((int) lector["id_especialidad"],(string) lector["descripcion_especialidad"]);
				}
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_selecciona_especialidad (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_busqueda.Selection.GetSelected(out model, out iterSelected)) 
 			{
 				id_esp_medico  = (int) model.GetValue(iterSelected, 0);
 				entry_especialidad.Text = (string) model.GetValue(iterSelected, 1);
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}

/*---------------------------------------------------------------------*/
////////////////////   ESTADOS Y MUNICIPIOS /////////////////////
/*---------------------------------------------------------------------*/
		void on_button_agrega_estados_municipios_clicked(object sender, EventArgs args)
		{	
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "estado_municipio", null);
			gxml.Autoconnect (this);
			
			estado_municipio.Show();
			checkbutton_estado.Clicked += new EventHandler(on_checkbutton_estado_clicked);
			checkbutton_municipio.Clicked += new EventHandler(on_checkbutton_municipio_clicked);	
			button_busca_municipio.Clicked += new EventHandler(on_button_busca_municipio_clicked);	
			button_busca_estado.Clicked += new EventHandler(on_button_busca_estado_clicked);
			//button_edita_municipio.Clicked += new EventHandler(on_edita_municipio);
			button_busca_municipio.Sensitive = false;
			button_guarda_estado.Sensitive = false;
			entry_valor_municipio.Sensitive = false;
			entry_municipio.Sensitive = false;
			entry_valor_estado.Sensitive = false;
			button_guarda_municipio.Sensitive = false;	
			button_busca_estado.Sensitive = true;
			entry_estado.Sensitive = false;
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_guarda_estado.Clicked += new EventHandler(on_guarda_estado_clicked);
			button_guarda_municipio.Clicked += new EventHandler(on_guarda_municipio_clicked);
		}
		
		void on_checkbutton_estado_clicked (object sender, EventArgs args)
		{
			int numeroestado;
			if(checkbutton_estado.Active == true)
			 { 
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de querer crear un nuevo estado?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
	 			if (miResultado == ResponseType.Yes)
	 			{
	 				entry_estado.Sensitive = true;
					button_busca_estado.Sensitive = true;
					button_guarda_estado.Sensitive = true;	
					numeroestado = int.Parse((string) lee_numero_estado())+1;
	    	       	entry_valor_estado.Text = numeroestado.ToString();
				}else{
					checkbutton_estado.Active = false;
				}			
			}
			if (checkbutton_estado.Active == false) 
			{ 
				entry_estado.Sensitive = false;
				button_guarda_estado.Sensitive = false;
			}
		}
		
		void on_checkbutton_municipio_clicked (object sender, EventArgs args)
		{
			int numeromunicipio;
			if(checkbutton_municipio.Active == true)
			 { 
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de querer crear un nuevo municipio?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
	 			if (miResultado == ResponseType.Yes)
	 			{
	 				entry_municipio.Sensitive = true;
					button_busca_municipio.Sensitive = true;
					button_guarda_municipio.Sensitive = true;	
					numeromunicipio = int.Parse((string) lee_numero_municipio())+1;
	    	       	entry_valor_municipio.Text = numeromunicipio.ToString();
	    	       
				}else{
					checkbutton_municipio.Active = false;
				}			
			}
			if (checkbutton_municipio.Active == false) 
			{ 
				entry_municipio.Sensitive = false;
				button_guarda_municipio.Sensitive = false;
			}
		}
		
		void on_button_busca_estado_clicked(object sender, EventArgs args)
		{
			busqueda = "estado";		
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               (this);
	      	entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			crea_treeview_estados_municipios(busqueda);
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_estado);
			button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_estado);
		}
		
		void on_button_busca_municipio_clicked(object sender, EventArgs args)
		{
			busqueda = "municipio";		
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               (this);
	      	entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			crea_treeview_estados_municipios(busqueda);
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_municipio);
			button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_municipio);
		}
		
		void on_guarda_estado_clicked(object sender, EventArgs args)
		{
			if(checkbutton_estado.Active == true){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
									ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
 				if (miResultado == ResponseType.Yes)
 				{
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
    	        	// Verifica que la base de datos este conectada
					try
					{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
 						comando.CommandText = "INSERT INTO osiris_estados("+
											  "descripcion_estado ) "+
 											  "VALUES ('"+
 											  (string) entry_estado.Text.Trim().ToUpper()+ "');"; 
 						comando.ExecuteNonQuery();    	    	       	comando.Dispose();
    	    	       	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info,ButtonsType.Close,"El estado se guardo con exito");
						msgBoxError.Run ();					msgBoxError.Destroy();
					}
					catch(NpgsqlException ex)
					{
	   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();					msgBoxError.Destroy();
	       			}
       				conexion.Close ();
       			}
			}else{
		
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
									ButtonsType.YesNo,"¿ Desea Actualizar esta infomacion ?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();							msgBox.Destroy();
				
 				if (miResultado == ResponseType.Yes)
 					{
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
    	        	// Verifica que la base de datos este conectada
					try
					{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
 						comando.CommandText = "UPDATE osiris_estados SET "+
 											"descripcion_estado  = '"+(string) entry_estado.Text.Trim().ToUpper();
 						comando.ExecuteNonQuery();    	    	       	comando.Dispose();
    	    	       	checkbutton_estado.Active = false;
    	    	       	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info,ButtonsType.Close,"Los datos se actualizaron con exito");			
						msgBoxError.Run ();					msgBoxError.Destroy();
						checkbutton_estado.Active = false;
					}catch(NpgsqlException ex){
	   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();					msgBoxError.Destroy();
	       			}
       				conexion.Close ();
       			}			
			}
		}
		
		void on_selecciona_estado(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_busqueda.Selection.GetSelected(out model, out iterSelected)){
 				int tomaidestado = (int) model.GetValue(iterSelected, 0);
 				entry_valor_estado.Text = tomaidestado.ToString();
				entry_estado.Text = (string) model.GetValue(iterSelected, 1);
				checkbutton_estado.Sensitive = true; 
				button_busca_municipio.Sensitive = true;
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
		
		void on_guarda_municipio_clicked(object sender, EventArgs args)
		{
			if(checkbutton_municipio.Active == true){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
									ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
 				if (miResultado == ResponseType.Yes){
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
    	        	// Verifica que la base de datos este conectada
    	        	try
					{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
 						comando.CommandText = "INSERT INTO osiris_municipios("+
											  "descripcion_municipio ,id_estado )"+
 											  "VALUES ('"+
 										  (string) entry_municipio.Text.Trim().ToUpper()+  "','"+ 
 											(string) entry_valor_estado.Text.Trim().ToUpper()+ "');"; 		
 						int numeromunicipio;
						numeromunicipio = int.Parse((string) lee_numero_municipio())+1;
	    	      	 	entry_valor_municipio.Text = numeromunicipio.ToString();
 						comando.ExecuteNonQuery();    	    	       	comando.Dispose();
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info,ButtonsType.Close,"El municipio se guardo con exito");
						msgBoxError.Run ();	
										msgBoxError.Destroy();		
					}
					
					catch(NpgsqlException ex)
					{
	   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();					msgBoxError.Destroy();
	       			}
       				conexion.Close ();
       			}
			}else{
			
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
									ButtonsType.YesNo,"¿ Desea Actualizar esta infomacion ?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();							msgBox.Destroy();
				
 				if (miResultado == ResponseType.Yes){
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
    	        	// Verifica que la base de datos este conectada
					try
					{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
 						//TreeIter iter;
 						comando.CommandText = "UPDATE osiris_municipios SET "+
 								"descripcion_municipio  = '"+(string) entry_municipio.Text.Trim().ToUpper();
 						comando.ExecuteNonQuery();    	    	       	comando.Dispose();
    	    	       	checkbutton_estado.Active = false;
    	    	       	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info,ButtonsType.Close,"Los datos se actualizaron con exito");			
						msgBoxError.Run ();					msgBoxError.Destroy();
						checkbutton_estado.Active = false;
					}catch(NpgsqlException ex){
	   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();					msgBoxError.Destroy();
	       			}
       				conexion.Close ();
       			}			
			}
		}

		void on_selecciona_municipio(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_busqueda.Selection.GetSelected(out model, out iterSelected)) 
 			{
 				int tomaidmunicipio = (int) model.GetValue(iterSelected, 0);
 				entry_valor_municipio.Text = tomaidmunicipio.ToString();
				entry_municipio.Text = (string) model.GetValue(iterSelected, 1);
				checkbutton_estado.Sensitive = true; 

				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}

		
		void crea_treeview_estados_municipios(string tipo)
		{
			if (tipo == "estado"){
				treeViewEngineestado = new TreeStore(typeof(int),//0
														typeof(string));//1
																					
				lista_de_busqueda.Model = treeViewEngineestado;
				
				lista_de_busqueda.RulesHint = true;
					
				lista_de_busqueda.RowActivated += on_selecciona_estado;  // Doble click selecciono paciente*/
				
				TreeViewColumn col_id_estado = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_id_estado.Title = "ID Estados"; // titulo de la cabecera de la columna, si está visible
				col_id_estado.PackStart(cellr0, true);
				col_id_estado.AddAttribute (cellr0, "text", 0);
				col_id_estado.SortColumnId = (int) Col_estados.col_id_estado;
				
				TreeViewColumn col_estado = new TreeViewColumn();
				CellRendererText cellrt1 = new CellRendererText();
				col_estado.Title = "Estados";
				col_estado.PackStart(cellrt1, true);
				col_estado.AddAttribute (cellrt1, "text", 1); 
				col_estado.SortColumnId = (int) Col_estados.col_estado;
							           
				lista_de_busqueda.AppendColumn(col_id_estado);
				lista_de_busqueda.AppendColumn(col_estado);
			}
			if (tipo == "municipio"){
				treeViewEnginemunicipio = new TreeStore(typeof(int),//0
													typeof(string));//3
																		
				lista_de_busqueda.Model = treeViewEnginemunicipio;
				
				lista_de_busqueda.RulesHint = true;
					
				lista_de_busqueda.RowActivated += on_selecciona_municipio;  // Doble click selecciono paciente*/
							
				TreeViewColumn col_id_municipio = new TreeViewColumn();
				CellRendererText cellrt0 = new CellRendererText();
				col_id_municipio.Title = "ID Municipios";
				col_id_municipio.PackStart(cellrt0, true);
				col_id_municipio.AddAttribute (cellrt0, "text", 0); // la siguiente columna será 1
				col_id_municipio.SortColumnId = (int) Col_municipio.col_id_municipio;

				TreeViewColumn col_municipio = new TreeViewColumn();
				CellRendererText cellrt1 = new CellRendererText();
				col_municipio.Title = "Municipios";
				col_municipio.PackStart(cellrt1, true);
				col_municipio.AddAttribute (cellrt1, "text", 1); 
				col_municipio.SortColumnId = (int) Col_municipio.col_municipio;
							           
				lista_de_busqueda.AppendColumn(col_id_municipio);
				lista_de_busqueda.AppendColumn(col_municipio);
			}
		}
		
		enum Col_estados
		{
			col_id_estado,
			col_estado
		}
		
		enum Col_municipio
		{
			col_id_municipio,
			col_municipio			
		}

		void on_llena_lista_estado(object sender, EventArgs args)
		{
			llenando_lista_de_estado();
		}
		
		void on_llena_lista_municipio(object sender, EventArgs args)
		{
			llenando_lista_de_municipio();
		}
		
		void llenando_lista_de_estado()
		{
			treeViewEngineestado.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			Console.WriteLine("ENTRE al CLICk");
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				if ((string) entry_expresion.Text.ToUpper() == "*" || (string) entry_expresion.Text.ToUpper() == "")
				{
					comando.CommandText = "SELECT id_estado,descripcion_estado "+
								"FROM osiris_estados "+
								"ORDER BY id_estado;";
				}else{
					comando.CommandText = "SELECT id_estado,descripcion_estado "+
								"FROM osiris_estados "+
								"WHERE descripcion_estado LIKE '%"+(string) entry_expresion.Text.ToUpper()+"%' "+
								"ORDER BY descripcion_estado;";
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//bool verifica_activacion;
				while (lector.Read())
				{	
					treeViewEngineestado.AppendValues ((int) lector["id_estado"],//0
													(string) lector["descripcion_estado"]);//1
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void llenando_lista_de_municipio()
		{
			treeViewEnginemunicipio.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			Console.WriteLine("arriba del query");
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				if ((string) entry_expresion.Text.ToUpper() == "*" || (string) entry_expresion.Text.ToUpper() == ""){
					comando.CommandText = "SELECT id_estado,id_municipio,descripcion_municipio "+
								"FROM osiris_municipios "+
								"WHERE id_estado = '"+entry_valor_estado.Text+"' "+
								"ORDER BY id_estado;";
				}else{
					comando.CommandText = "SELECT id_estado,id_municipio,descripcion_municipio "+
								"FROM osiris_municipios "+
								"WHERE descripcion_municipio LIKE '%"+(string) entry_expresion.Text.ToUpper()+"%' ,"+
								"AND id_estado = '"+entry_valor_estado.Text+"' "+
								"ORDER BY descripcion_municipio;";
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				Console.WriteLine("query actualizado cliente "+comando.CommandText.ToString());
				
				while (lector.Read())
				{	
					treeViewEnginemunicipio.AppendValues( (int) lector["id_municipio"],
														(string) lector["descripcion_municipio"]);
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		string lee_numero_estado()
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			string tomavalor = "";
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(id_estado,'99999999') AS idestado "+ 
										"FROM osiris_estados "+
										
										"ORDER BY id_estado DESC LIMIT 1;";
										NpgsqlDataReader lector = comando.ExecuteReader ();
										//Console.WriteLine(comando.CommandText);
										//bool verifica_activacion;
				if (lector.Read()){	
					tomavalor =(string) lector["idestado"]; 
					return tomavalor;
					//lector.Close ();
					//conexion.Close();
				}else{
					//if (!lector.Read()){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error,ButtonsType.Close,"No se guardo correctamente el estado");
							msgBoxError.Run ();			msgBoxError.Destroy();
					return tomavalor;
					//conexion.Close();
				}
			}catch (NpgsqlException ex){
		   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();			msgBoxError.Destroy();
							return tomavalor;
			}
		}
		
		string lee_numero_municipio()
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			string tomavalor = "";
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(id_municipio,'99999999') AS idmunicipio "+ 
										//"to_char(id_estado,'99999999') AS idestado "+
										"FROM osiris_municipios "+
										"ORDER BY id_municipio DESC LIMIT 1;";
										NpgsqlDataReader lector = comando.ExecuteReader ();
										//Console.WriteLine(comando.CommandText);
										//bool verifica_activacion;
											
										
				if (lector.Read())
				{	
					tomavalor =(string) lector["idmunicipio"]; 
					return tomavalor;
					//lector.Close ();
					//conexion.Close();
				}else{
					//if (!lector.Read()){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error,ButtonsType.Close,"No se guardo correctamente el municipio");
							msgBoxError.Run ();			msgBoxError.Destroy();
					return tomavalor;
					//conexion.Close();
				}
			}catch (NpgsqlException ex){
		   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();			msgBoxError.Destroy();
							return tomavalor;
			}
		}
		
// FIN ESTADOS Y MUNICIPIOS
		
////////////////////////////////////////////////////////7		
		void on_button_buscar_empresa_clicked(object sender, EventArgs args)
		{
			busqueda = "empresa";
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_empresa);
			button_selecciona.Clicked += new EventHandler(on_selecciona_empresa);
	        button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
			treeViewEngineEmpresa = new TreeStore( typeof(int),typeof(string));
			lista_de_busqueda.Model = treeViewEngineEmpresa;
			lista_de_busqueda.RulesHint = true;
			lista_de_busqueda.RowActivated += on_selecciona_empresa;  // Doble click selecciono paciente
			
			TreeViewColumn col_idempresa = new TreeViewColumn();
			cellr0 = new CellRendererText();
			col_idempresa.Title = "ID Empresa"; // titulo de la cabecera de la columna, si está visible
			col_idempresa.PackStart(cellr0, true);
			col_idempresa.AddAttribute (cellr0, "text", 0);
			col_idempresa.SortColumnId = (int) Column_empresa.col_idempresa;   
			
			TreeViewColumn col_empresa = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_empresa.Title = "Empresa";
			col_empresa.PackStart(cellr1,true);
			col_empresa.AddAttribute(cellr1, "text", 1);
			col_empresa.SortColumnId = (int) Column_empresa.col_empresa;
			
			lista_de_busqueda.AppendColumn(col_idempresa);
			lista_de_busqueda.AppendColumn(col_empresa);
		}
		
		enum Column_empresa {	col_idempresa,	col_empresa	}
		
		void on_llena_lista_empresa(object sender, EventArgs args)
		{
			llenando_lista_de_empresa();
		}
		
		void llenando_lista_de_empresa()
		{
			//TreeIter iter;
			treeViewEngineEmpresa.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				if ((string) entry_expresion.Text.ToUpper().Trim() == "")
				{
					comando.CommandText = "SELECT id_empresa,descripcion_empresa "+
										" FROM osiris_empresas "+
										"ORDER BY id_empresa;";
				}else{
					comando.CommandText = "SELECT id_empresa,descripcion_empresa "+
										" FROM osiris_empresas "+
										"WHERE descripcion_empresa LIKE '"+entry_expresion.Text.ToUpper().Trim()+"%' ;"+
										"ORDER BY id_empresa;";
				}						
					
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read())
				{
					treeViewEngineEmpresa.AppendValues ((int) lector["id_empresa"],(string) lector["descripcion_empresa"]);
				}
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_selecciona_empresa (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_busqueda.Selection.GetSelected(out model, out iterSelected)) 
 			{
 				id_emp_medico  = (int) model.GetValue(iterSelected, 0);
 				entry_empresa.Text = (string) model.GetValue(iterSelected, 1);
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
		
		void on_button_actualizar_clicked(object sender, EventArgs args)
		{
			actualizacion = true;
			entry_nombre1_medico.GrabFocus();
			activacion_entrys(true);
			sensibilidad_checkbutton(true);
			activacion_botones(true);
			checkbutton_nuevo_medico.Active = false;
		}
		
		void on_checkbutton_nuevo_medico_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox; 
			if(checkbutton_nuevo_medico.Active == true) {
				msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿Esta seguro de querer registrar un nuevo medico?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();		msgBox.Destroy();
	 			if (miResultado == ResponseType.Yes){
	 				entrys_a_blanco();
					activacion_entrys(true);
					sensibilidad_checkbutton(true);
					activacion_checkbutton(false);
					activacion_botones(true);
					entry_nombre1_medico.GrabFocus();
					checkbutton_medico_activo.Active = true;
				}else{
					checkbutton_nuevo_medico.Active = false;
				}
			}else{
				if(actualizacion == false){
					entrys_a_blanco();
					activacion_entrys(false);
					sensibilidad_checkbutton(false);
					activacion_checkbutton(false);
					activacion_botones(false);
					actualizacion = false;
					checkbutton_medico_activo.Active = false;
				}
			}
		}
		
		void on_button_autorizacion_medico_clicked(object sender, EventArgs args)
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			bool primera_verificacion = chequeo_de_verificacion();
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				if( primera_verificacion == false) {
					comando.CommandText = "UPDATE osiris_his_medicos SET autorizado = 'true', "+
									"fecha_autorizacion_medico = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
									"fecha_revision_medico = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
									"historial_de_revision = historial_de_revision || '"+LoginEmpleado+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n' "+
									"WHERE id_medico = '"+entry_id_medico.Text.Trim()+"' ;";
					comando.ExecuteNonQuery();    	    	       	comando.Dispose();
				}else{//( primera_verificacion == true) {
					comando.CommandText = "UPDATE osiris_his_medicos SET autorizado = 'true', "+
									"fecha_revision_medico = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
									"historial_de_revision = historial_de_revision || '"+LoginEmpleado+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+" REVISION \n' "+
									"WHERE id_medico = '"+entry_id_medico.Text.Trim()+"' ;";
					comando.ExecuteNonQuery();    	    	       	comando.Dispose();
				}
    	       	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Info,ButtonsType.Close,"Medico Autorizado y/o Revisado");
				msgBoxError.Run ();					msgBoxError.Destroy();
				checkbutton_medico_provisional.Active = false;
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		bool chequeo_de_verificacion()
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			bool varpaso = false; 
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT autorizado FROM osiris_his_medicos WHERE id_medico = '"+entry_id_medico.Text+"';";
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if (lector.Read())
				{
					varpaso =  (bool) lector["autorizado"]; return varpaso;
				}
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
				return varpaso;
			}
			conexion.Close ();
			return varpaso;
		}
		
		///funciones de activado y desactivado de botones y entrys
		void entrys_a_blanco()
		{
			entry_id_medico.Text = "";
			entry_nombre1_medico.Text = "";
			entry_nombre2_medico.Text = "";
			entry_apellido_paterno_medico.Text = "";
			entry_apellido_materno_medico.Text = "";
		  	entry_especialidad.Text = "";
		  	entry_empresa.Text = "";
		  	entry_direccion_casa_medico.Text = "";
		  	entry_direccion_consultorio_medico.Text = "";
			entry_cedula_profecional.Text = "";
			entry_telcasa_medico.Text = "";
			entry_teloficina_medico.Text = "";
			entry_celular1_medico.Text = "";
			entry_celular2_medico.Text = "";
			entry_nextel_medico.Text = "";
			entry_beeper_medico.Text = "";
			entry_fecha_ingreso.Text = "";
			entry_fecha_revision.Text = "";
			entry_id_medico.Text = "";
		}
		
		void activacion_entrys(bool valor)
		{
			entry_id_medico.Sensitive = valor;
			entry_nombre1_medico.Sensitive = valor;
			entry_nombre2_medico.Sensitive = valor;
			entry_apellido_paterno_medico.Sensitive = valor;
			entry_apellido_materno_medico.Sensitive = valor;
		  	entry_especialidad.Sensitive = valor;
		  	entry_empresa.Sensitive = valor;
		  	entry_direccion_casa_medico.Sensitive = valor;
		  	entry_direccion_consultorio_medico.Sensitive = valor;
			entry_cedula_profecional.Sensitive = valor;
			entry_telcasa_medico.Sensitive = valor;
			entry_teloficina_medico.Sensitive = valor;
			entry_celular1_medico.Sensitive = valor;
			entry_celular2_medico.Sensitive = valor;
			entry_nextel_medico.Sensitive = valor;
			entry_beeper_medico.Sensitive = valor;
			entry_fecha_ingreso.Sensitive = valor;
			entry_fecha_revision.Sensitive = valor;
			entry_id_medico.Sensitive = valor;
		}
		
		void sensibilidad_checkbutton(bool valor)
		{
			checkbutton_medico_activo.Sensitive = valor;
			checkbutton_centro_medico.Sensitive = valor;
			checkbutton_tituloprof_medico.Sensitive = valor;
			checkbutton_cedula_prof_medico.Sensitive = valor;
			checkbutton_diploespecial_medico.Sensitive = valor;
			checkbutton_cedula_especialidad_medico.Sensitive = valor;
			checkbutton_cursoadistramiento_medico.Sensitive = valor;
			checkbutton_diplomasubespecial.Sensitive = valor;
			checkbutton_copiaidentificacionoficial.Sensitive = valor;
			checkbutton_copiacedularfc.Sensitive = valor;
			checkbutton_certificadoconsejo_esp.Sensitive = valor;
			checkbutton_constancia_congresos.Sensitive = valor;
			checkbutton_copia_comprobante_domicilio.Sensitive = valor;
			checkbutton_diplomaextranjero.Sensitive = valor;
			checkbutton_diploseminarios.Sensitive = valor; 
			checkbutton_diplomacursos.Sensitive = valor;
		}
		
		void activacion_checkbutton(bool valor)
		{
			checkbutton_medico_activo.Active = valor;
			checkbutton_centro_medico.Active = valor;
			checkbutton_medico_provisional.Active = valor;
			checkbutton_tituloprof_medico.Active = valor;
			checkbutton_cedula_prof_medico.Active = valor;
			checkbutton_diploespecial_medico.Active = valor;
			checkbutton_cursoadistramiento_medico.Active = valor;
			checkbutton_diplomasubespecial.Active = valor;
			checkbutton_cedula_especialidad_medico.Active = valor;
			checkbutton_copiaidentificacionoficial.Active = valor;
			checkbutton_copiacedularfc.Active = valor;
			checkbutton_certificadoconsejo_esp.Active = valor;
			checkbutton_constancia_congresos.Active = valor;
			checkbutton_copia_comprobante_domicilio.Active = valor;
			checkbutton_diplomaextranjero.Active = valor;
			checkbutton_diploseminarios.Active = valor; 
			checkbutton_diplomacursos.Active = valor;
		}
		
		void activacion_botones(bool valor)
		{
			button_actualizar.Sensitive = !valor;
			button_buscar_especialidad.Sensitive = valor;
			button_buscar_empresa.Sensitive = valor;
			button_notas.Sensitive = valor;
			button_imprimir.Sensitive = valor;
			button_guardar.Sensitive = valor;
		}
		
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((bool)lista_de_medicos.Model.GetValue (iter,34)==true) {
				if ((bool)lista_de_medicos.Model.GetValue (iter,36)==true) {(cell as Gtk.CellRendererText).Foreground = "darkgreen";
				}else{ (cell as Gtk.CellRendererText).Foreground = "black"; }
			}else{	(cell as Gtk.CellRendererText ).Foreground = "red"; }
		}
		
///////////////////////////////////////BOTON general de busqueda por enter///////////////////////////////////////////////		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;		
				if(busqueda == "medicos") { llenando_lista_de_medicos(); }
				if(busqueda == "clientes") { llenando_lista_de_clientes(); }
				if(busqueda == "especialidad") {llenando_lista_de_especialidad();}
				if(busqueda == "empresa") {llenando_lista_de_empresa();}
				if(busqueda == "proveedores") {llenando_lista_de_proveedores();}							
			}
		}
////////////////////////////////////////////////BOTON de salir general////////////////////////////////////////////////		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}					
	}
}
