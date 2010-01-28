////////////////////////////////////////////////////////////
// created on 07/06/2007 at 11:02 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
//				  Daniel Olivares - arcangeldoc@gmail.com (Diseño de Pantallas Glade)
// 				  
// Licencia		: GLP
// S.O. 		: GNU/Linux Ubuntu 6.06 LTS (Dapper Drake)
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
// Proposito	: Menu principal del modulo de Tesoreria 
// Objeto		: Coordinacion de las opciones del menu
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class tesoreria
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando ventana principal de Hospitalizacion
		[Widget] Gtk.Window menu_tesoreria;
		[Widget] Gtk.Button button_caja;
		[Widget] Gtk.Button button_facturador;
		[Widget] Gtk.Button button_clientes;
		[Widget] Gtk.Button button_proc_facturados;
		[Widget] Gtk.Button button_proc_no_facturados;
		[Widget] Gtk.Button button_rpt_honorarios_medicos;
		[Widget] Gtk.Button button_rpt_facturas_pagadas;
		[Widget] Gtk.Button button_reporte_de_cerrados;
		[Widget] Gtk.Button button_rpt_abonos;
		[Widget] Gtk.Button button_envio_de_facturas;
		[Widget] Gtk.Button button_rpt_facturas_pendientes;
		
		//[Widget] Gtk.Button button_clientes;
		
		//declarando la ventana de rango de fechas
		[Widget] Gtk.Window rango_de_fecha;
		[Widget] Gtk.Entry entry_dia1;
		[Widget] Gtk.Entry entry_mes1;
		[Widget] Gtk.Entry entry_ano1;
		[Widget] Gtk.Entry entry_dia2;
		[Widget] Gtk.Entry entry_mes2;
		[Widget] Gtk.Entry entry_ano2;
		[Widget] Gtk.Label label_orden;
		[Widget] Gtk.RadioButton radiobutton_cliente;
		[Widget] Gtk.RadioButton radiobutton_fecha;
		[Widget] Gtk.CheckButton  checkbutton_impr_todo_proce;
		[Widget] Gtk.Label label_nom_cliente;
		[Widget] Gtk.Entry entry_cliente;
		[Widget] Gtk.Button button_busca_cliente;
		[Widget] Gtk.CheckButton checkbutton_todos_los_clientes;
		[Widget] Gtk.Entry entry_referencia_inicial;
		[Widget] Gtk.Button button_imprime_rangofecha;
		
		///VENTANA buscadora de clientes
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		[Widget] Gtk.Entry entry_expresion; 
		[Widget] Gtk.TreeView lista_de_cliente;
		
		//ventana de busqueda de medicos en honorarios medicos
		[Widget] Gtk.ComboBox combobox_tipo_busqueda;
		[Widget] Gtk.TreeView lista_de_medicos;		
			
		//ventana treeview
		[Widget] Gtk.TreeView lista_medicos;
		
		//treeview
		private TreeStore treeViewEngineBusca;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
				
		int idcliente = 1;
						
		string connectionString;
		string nombrebd;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		string facturados = "FACTURADOS";
		string busqueda = "";
		bool pagados = false;
					
		int idmedico = 0;
 		string nombmedico = "";
 		string especialidadmed ="";
 		string telmedico = "";
 		string cedmedico = "";
 		string diagnostico="";
		////
		string facturas_="";
		
		string tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";
		
		private TreeStore treeViewEngineMedicos;
		private TreeStore treeViewEngineClientes;
		 
		protected Gtk.Window MyWinError;
		
		// Variables publicas para le rango de fecha procedimiento
		//public string fecha_rango_1;
		//public string fecha_rango_2;
		
		public tesoreria(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
						
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "menu_tesoreria", null);
			gxml.Autoconnect (this);        
			////// Muestra ventana de Glade
			menu_tesoreria.Show();
			
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_caja.Clicked += new EventHandler(on_button_caja_clicked);
			button_clientes.Clicked += new EventHandler(on_button_clientes_clicked);
			button_proc_facturados.Clicked += new EventHandler(on_button_proc_facturados_clicked);
			button_proc_no_facturados.Clicked += new EventHandler(on_button_proc_no_facturados_clicked);
			button_rpt_honorarios_medicos.Clicked += new EventHandler(on_button_rpt_honorarios_medicos_cliked);
			button_rpt_facturas_pagadas.Clicked += new EventHandler(on_button_button_rpt_facturas_pagadas_clicked);
			button_facturador.Clicked += new EventHandler(on_button_facturador_clicked);
			button_reporte_de_cerrados.Clicked += new EventHandler(on_button_reporte_de_cerrados_clicked);
			button_rpt_abonos.Clicked += new EventHandler(on_button_button_rpt_abonos_clicked);
			button_envio_de_facturas.Clicked += new EventHandler(on_button_envio_facturas_clicked);
			button_rpt_facturas_pendientes.Clicked += new EventHandler(button_rpt_facturas_pendientes_clicked);
		}

		
		void on_button_caja_clicked(object sender, EventArgs args)
		{
			new osiris.caja_cobro(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_facturador_clicked(object sender, EventArgs args)
		{
			new osiris.facturador_tesoreria(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_clientes_clicked(object sender, EventArgs args)
		{
			new osiris.catalogos_generales("cliente",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_button_rpt_abonos_clicked(object sender, EventArgs args)
		{
			//new osiris.reporte_de_abonos(nombrebd);	
		}
		
		void on_button_envio_facturas_clicked(object sender, EventArgs args)
		{
			new osiris.envio_de_facturas(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}

		void button_rpt_facturas_pendientes_clicked(object sender, EventArgs args)
		{
			this.facturas_ = "AND osiris_erp_factura_enca.pagada = 'false' " ;
			Console.WriteLine(facturas_);
			pagados = false;
			on_button_button_rpt_facturas();
		}
		
		void on_button_button_rpt_facturas_pagadas_clicked(object sender, EventArgs args)
		{
			this.facturas_ = "AND osiris_erp_factura_enca.pagada = 'true' ";
			pagados = true;
			on_button_button_rpt_facturas();
		}
		
		void on_button_button_rpt_facturas()
		{	
			
			Glade.XML gxml = new Glade.XML  (null, "caja.glade", "rango_de_fecha", null);
			gxml.Autoconnect (this);  
			rango_de_fecha.Show();
			checkbutton_impr_todo_proce.Label = "Imprime TODO";
			entry_referencia_inicial.Editable = false;
			entry_referencia_inicial.Text = DateTime.Now.ToString("dd-MM-yyyy");
			checkbutton_todos_los_clientes.Active = true;
			entry_cliente.Sensitive = false;
			button_busca_cliente.Sensitive = false;
			checkbutton_todos_los_clientes.Clicked += new EventHandler(on_checkbutton_todos_los_clientes_clicked);
			button_busca_cliente.Clicked += new EventHandler(on_button_busca_cliente_clicked);
			checkbutton_impr_todo_proce.Clicked += new EventHandler(checkbutton_impr_todo_proce_clicked);
			entry_dia1.KeyPressEvent += onKeyPressEvent;
			entry_mes1.KeyPressEvent += onKeyPressEvent;
			entry_ano1.KeyPressEvent += onKeyPressEvent;
			entry_dia2.KeyPressEvent += onKeyPressEvent;
			entry_mes2.KeyPressEvent += onKeyPressEvent;
			entry_ano2.KeyPressEvent += onKeyPressEvent;
			entry_dia1.Text =DateTime.Now.ToString("dd");
			entry_mes1.Text =DateTime.Now.ToString("MM");
			entry_ano1.Text =DateTime.Now.ToString("yyyy");
			entry_dia2.Text =DateTime.Now.ToString("dd");
			entry_mes2.Text =DateTime.Now.ToString("MM");
			entry_ano2.Text =DateTime.Now.ToString("yyyy");
		//if(pagados == true ){
		facturados = "FACTURADOS";
		//}else{
		//Console.WriteLine("nopagadossssss");
			//	facturados = "NOPAGADOS";
//}
			button_imprime_rangofecha.Clicked += new EventHandler(imprime_reporte);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
		}
		
		void on_button_rpt_honorarios_medicos_cliked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML  (null, "caja.glade", "rango_de_fecha", null);
			gxml.Autoconnect (this);  
			rango_de_fecha.Show();
			
			label_nom_cliente.Text = "DOCTORES ";
			this.radiobutton_cliente.Label = "DOCTORES    ";
											
			entry_dia1.KeyPressEvent += onKeyPressEvent;
			entry_mes1.KeyPressEvent += onKeyPressEvent;
			entry_ano1.KeyPressEvent += onKeyPressEvent;
			entry_dia2.KeyPressEvent += onKeyPressEvent;
			entry_mes2.KeyPressEvent += onKeyPressEvent;
			entry_ano2.KeyPressEvent += onKeyPressEvent;
			entry_dia1.Text =DateTime.Now.ToString("dd");
			entry_mes1.Text =DateTime.Now.ToString("MM");
			entry_ano1.Text =DateTime.Now.ToString("yyyy");
			entry_dia2.Text =DateTime.Now.ToString("dd");
			entry_mes2.Text =DateTime.Now.ToString("MM");
			entry_ano2.Text =DateTime.Now.ToString("yyyy");
			facturados = "HONORARIOS_MEDICOS";
			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			this.button_busca_cliente.Clicked += new EventHandler(on_button_busca_medico_clicked);
			button_imprime_rangofecha.Clicked += new EventHandler(on_button_imprime_rangofecha_clicked);
		}
		
		void on_button_proc_facturados_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML  (null, "caja.glade", "rango_de_fecha", null);
			gxml.Autoconnect (this);  
			rango_de_fecha.Show();
			checkbutton_impr_todo_proce.Label = "Imprime TODO";
			entry_referencia_inicial.Editable = false;
			entry_referencia_inicial.Text = DateTime.Now.ToString("dd-MM-yyyy");
			
			checkbutton_todos_los_clientes.Active = true;
			entry_cliente.Sensitive = false;
			button_busca_cliente.Sensitive = false;
			checkbutton_todos_los_clientes.Clicked += new EventHandler(on_checkbutton_todos_los_clientes_clicked);
			button_busca_cliente.Clicked += new EventHandler(on_button_busca_cliente_clicked);
			checkbutton_impr_todo_proce.Clicked += new EventHandler(checkbutton_impr_todo_proce_clicked);
			
			entry_dia1.KeyPressEvent += onKeyPressEvent;
			entry_mes1.KeyPressEvent += onKeyPressEvent;
			entry_ano1.KeyPressEvent += onKeyPressEvent;
			entry_dia2.KeyPressEvent += onKeyPressEvent;
			entry_mes2.KeyPressEvent += onKeyPressEvent;
			entry_ano2.KeyPressEvent += onKeyPressEvent;
			entry_dia1.Text =DateTime.Now.ToString("dd");
			entry_mes1.Text =DateTime.Now.ToString("MM");
			entry_ano1.Text =DateTime.Now.ToString("yyyy");
			entry_dia2.Text =DateTime.Now.ToString("dd");
			entry_mes2.Text =DateTime.Now.ToString("MM");
			entry_ano2.Text =DateTime.Now.ToString("yyyy");
			facturados = "FACTURADOS";
			button_imprime_rangofecha.Clicked += new EventHandler(imprime_reporte);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
		}
		
		void on_button_reporte_de_cerrados_clicked(object sender, EventArgs args)
		{
			Glade.XML  gxml = new Glade.XML  (null, "caja.glade", "rango_de_fecha", null);
			gxml.Autoconnect  (this);	
			rango_de_fecha.Show();
			label_orden.ChildVisible = false;
			radiobutton_cliente.ChildVisible = false;
			radiobutton_fecha.ChildVisible = false;
		
			checkbutton_impr_todo_proce.Label = "Imprime TODO";
			entry_referencia_inicial.Editable = false;
			entry_referencia_inicial.Text = DateTime.Now.ToString("dd-MM-yyyy");
			label_nom_cliente.ChildVisible = false;
			entry_cliente.ChildVisible = false;
			button_busca_cliente.ChildVisible = false;
			checkbutton_todos_los_clientes.ChildVisible = false;
			
			entry_dia1.KeyPressEvent += onKeyPressEvent;
			entry_mes1.KeyPressEvent += onKeyPressEvent;
			entry_ano1.KeyPressEvent += onKeyPressEvent;
			entry_dia2.KeyPressEvent += onKeyPressEvent;
			entry_mes2.KeyPressEvent += onKeyPressEvent;
			entry_ano2.KeyPressEvent += onKeyPressEvent;
			entry_dia1.Text =DateTime.Now.ToString("dd");
			entry_mes1.Text =DateTime.Now.ToString("MM");
			entry_ano1.Text =DateTime.Now.ToString("yyyy");
			entry_dia2.Text =DateTime.Now.ToString("dd");
			entry_mes2.Text =DateTime.Now.ToString("MM");
			entry_ano2.Text =DateTime.Now.ToString("yyyy");
			facturados = "CERRADOS";
			button_imprime_rangofecha.Clicked += new EventHandler(imprime_reporte);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
		}
		
		void on_button_proc_no_facturados_clicked(object sender, EventArgs args)
		{
			Glade.XML  gxml = new Glade.XML  (null, "caja.glade", "rango_de_fecha", null);
			gxml.Autoconnect  (this);	
			rango_de_fecha.Show();
			label_orden.ChildVisible = false;
			radiobutton_cliente.ChildVisible = false;
			radiobutton_fecha.ChildVisible = false;
		
			checkbutton_impr_todo_proce.Label = "Imprime TODO";
			entry_referencia_inicial.Editable = false;
			entry_referencia_inicial.Text = DateTime.Now.ToString("dd-MM-yyyy");
			label_nom_cliente.ChildVisible = false;
			entry_cliente.ChildVisible = false;
			button_busca_cliente.ChildVisible = false;
			checkbutton_todos_los_clientes.ChildVisible = false;
			Console.WriteLine("no facturados");
			
			entry_dia1.KeyPressEvent += onKeyPressEvent;
			entry_mes1.KeyPressEvent += onKeyPressEvent;
			entry_ano1.KeyPressEvent += onKeyPressEvent;
			entry_dia2.KeyPressEvent += onKeyPressEvent;
			entry_mes2.KeyPressEvent += onKeyPressEvent;
			entry_ano2.KeyPressEvent += onKeyPressEvent;
			entry_dia1.Text =DateTime.Now.ToString("dd");
			entry_mes1.Text =DateTime.Now.ToString("MM");
			entry_ano1.Text =DateTime.Now.ToString("yyyy");
			entry_dia2.Text =DateTime.Now.ToString("dd");
			entry_mes2.Text =DateTime.Now.ToString("MM");
			entry_ano2.Text =DateTime.Now.ToString("yyyy");
			facturados = "NO FACTURADOS";
			button_imprime_rangofecha.Clicked += new EventHandler(imprime_reporte);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
		}
			
		void on_button_imprime_rangofecha_clicked(object sender, EventArgs args)
		{
			
			string query_fechas = "";
			string query_medico = "";
			string rango1 = "";
			string rango2 = "";
			string orden = "";
			if(checkbutton_todos_los_clientes.Active == true){
				query_medico = " ";
			}
				
			if(checkbutton_todos_los_clientes.Active == false){
				query_medico = " AND osiris_erp_honorarios_medicos.id_medico = '"+idmedico.ToString()+"' ";
			}
			if (checkbutton_impr_todo_proce.Active == true) { 
				query_fechas = " ";
			}else{
				query_fechas = "AND to_char(osiris_erp_factura_enca.fecha_factura,'yyyy-MM-dd') >= '"+entry_ano1.Text+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
								"AND to_char(osiris_erp_factura_enca.fecha_factura,'yyyy-MM-dd') <= '"+entry_ano2.Text+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";
			}
			if(radiobutton_fecha.Active == true){ 
				orden = "ORDER BY to_char(osiris_erp_factura_enca.fecha_factura,'yyyy-MM-dd')";
			}
			if(radiobutton_cliente.Active ==true) {	
				orden = "ORDER BY nombre_medico";
			}
						
			rango_de_fecha.Destroy();
			//new osiris.rpt_honorario_med_fecha(rango1,rango2,query_fechas,nombrebd,LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,facturados,orden,query_medico);		
			  
		}
	
		void on_button_busca_medico_clicked (object sender, EventArgs args)
		{
			busqueda = "medicos";
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "buscador_medicos", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        llenado_cmbox_tipo_busqueda();
	        entry_expresion.KeyPressEvent += onKeyPressEvent_busqueda;
			button_buscar_busqueda.Clicked += new EventHandler(on_button_llena_medicos_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_medico_clicked);
	        button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
			
			treeViewEngineMedicos = new TreeStore(typeof(int),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(bool),typeof(bool),typeof(bool),typeof(bool),
												typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),
												typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool));
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
										"AND medico_activo = 'true' "+
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
										"AND medico_activo = 'true' "+
								  		tipobusqueda+(string) entry_expresion.Text.Trim().ToUpper()+"%' "+
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
        
        void on_selecciona_medico_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_medicos.Selection.GetSelected(out model, out iterSelected)) 
 			{
 				idmedico =(int) model.GetValue(iterSelected, 0);
 				nombmedico = (string) model.GetValue(iterSelected, 1)+" "+(string) model.GetValue(iterSelected, 2)+" "+
							(string) model.GetValue(iterSelected, 3)+" "+(string) model.GetValue(iterSelected,4);	;
				especialidadmed = (string) model.GetValue(iterSelected, 5);
				cedmedico = (string) model.GetValue(iterSelected, 6);
				this.entry_cliente.Text = nombmedico;
				 
				if((string) model.GetValue(iterSelected, 7) != "") {telmedico = (string) model.GetValue(iterSelected, 7);}
				else{
					if((string) model.GetValue(iterSelected,8) != "") {telmedico = (string) model.GetValue(iterSelected,8);}
					else{
						if((string) model.GetValue(iterSelected,9) != "") {telmedico = (string) model.GetValue(iterSelected,9);}
						else{
							if((string) model.GetValue(iterSelected,10) != "") {telmedico = (string) model.GetValue(iterSelected,10);}
							else{
								if((string) model.GetValue(iterSelected,11) != "") {telmedico = (string) model.GetValue(iterSelected,11);}
								else{
									if((string) model.GetValue(iterSelected,12) != "") {telmedico = (string) model.GetValue(iterSelected,12);}
								}
							}
						}
					}
				}
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
		
		void on_button_busca_cliente_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "busca_cliente", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_clientes);
	        //entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
	        button_selecciona.Clicked += new EventHandler(on_selecciona_cliente);
	       	crea_treeview_clientes();
			button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
		}
		
		void crea_treeview_clientes()
		{
			treeViewEngineClientes = new TreeStore(typeof(int),	typeof(string));
			lista_de_cliente.Model = treeViewEngineClientes;
			lista_de_cliente.RulesHint = true;
			lista_de_cliente.RowActivated += on_selecciona_cliente;  // Doble click selecciono paciente*/
			
			TreeViewColumn col_idcliente = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idcliente.Title = "ID Clientes"; // titulo de la cabecera de la columna, si está visible
			col_idcliente.PackStart(cellr0, true);
			col_idcliente.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1
			col_idcliente.SortColumnId = (int) Col_clientes.col_idcliente;
			
			TreeViewColumn col_cliente = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_cliente.Title = "Clientes";
			col_cliente.PackStart(cellrt1, true);
			col_cliente.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
			col_cliente.SortColumnId = (int) Col_clientes.col_cliente;
									           
			lista_de_cliente.AppendColumn(col_idcliente);
			lista_de_cliente.AppendColumn(col_cliente);
		}
		
		enum Col_clientes {		col_idcliente,	col_cliente,	}
		
		void on_llena_lista_clientes(object sender, EventArgs args)
		{
			llenando_lista_de_clientes();
		}
		
		void llenando_lista_de_clientes()
		{
			treeViewEngineClientes.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				if ((string) entry_expresion.Text.Trim() == "")
				{
					comando.CommandText = "SELECT descripcion_cliente, cliente_activo,id_cliente "+
								"FROM osiris_erp_clientes "+
								"WHERE cliente_activo = 'true' "+
								"ORDER BY descripcion_cliente;";
				}else{
					comando.CommandText = "SELECT descripcion_cliente, cliente_activo,id_cliente "+
								"FROM osiris_erp_clientes "+
								"WHERE descripcion_cliente LIKE '%"+(string) entry_expresion.Text.ToUpper()+"%' "+
								"AND cliente_activo = 'true' "+
								"ORDER BY descripcion_cliente;";
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//bool verifica_activacion;
				while (lector.Read())
				{	
					treeViewEngineClientes.AppendValues ((int) lector["id_cliente"],(string) lector["descripcion_cliente"]);
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
			if (lista_de_cliente.Selection.GetSelected(out model, out iterSelected)) 
 			{
 				idcliente = (int) model.GetValue(iterSelected, 0);
 				entry_cliente.Text = idcliente.ToString()+"-"+(string) model.GetValue(iterSelected, 1);
				// cierra la ventana despues que almaceno la informacion en variables
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
		
		void checkbutton_impr_todo_proce_clicked(object sender, EventArgs args)
		{
			if(checkbutton_impr_todo_proce.Active == true) { 
				entry_dia1.Sensitive = false; 			entry_mes1.Sensitive = false;
				entry_ano1.Sensitive = false;			entry_dia2.Sensitive = false;
				entry_mes2.Sensitive = false;			entry_ano2.Sensitive = false;
			}
			if(checkbutton_impr_todo_proce.Active == false) { 
				entry_dia1.Sensitive = true; 			entry_mes1.Sensitive = true;
				entry_ano1.Sensitive = true;			entry_dia2.Sensitive = true;
				entry_mes2.Sensitive = true;			entry_ano2.Sensitive = true;
			}
		}
		
		void on_checkbutton_todos_los_clientes_clicked(object sender, EventArgs args)
		{
			if(checkbutton_todos_los_clientes.Active == true) {entry_cliente.Sensitive = false;  button_busca_cliente.Sensitive = false; }
			if(checkbutton_todos_los_clientes.Active == false) {entry_cliente.Sensitive = true; entry_cliente.Text = "1-PUBLICO EN GENERAL";  button_busca_cliente.Sensitive = true; }
		}
		
		
		void imprime_reporte(object sender, EventArgs args)
		{	
			string query_medicos = "";
			string query_fechas = "";
			string query_cliente = "";
			string rango1 = "";
			string rango2 = "";
			if(checkbutton_todos_los_clientes.Active == true){query_cliente = " "; }
			if(checkbutton_todos_los_clientes.Active == false){
				query_cliente = " AND osiris_erp_factura_enca.id_cliente = '"+idcliente.ToString()+"' ";
			}
			if (checkbutton_impr_todo_proce.Active == true) { 
				query_fechas = " ";	 
				rango1 = "";
				rango2 = "";
			}else {
				rango1 = entry_dia1.Text+"-"+entry_mes1.Text+"-"+entry_ano1.Text;
				rango2 = entry_dia2.Text+"-"+entry_mes2.Text+"-"+entry_ano2.Text;
				if(facturados == "NO FACTURADOS"){
					query_fechas = "AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+entry_ano1.Text+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
									"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+entry_ano2.Text+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";
				}
				if(facturados == "FACTURADOS"){
					query_fechas = "AND to_char(osiris_erp_factura_enca.fecha_factura,'yyyy-MM-dd') >= '"+entry_ano1.Text+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
									"AND to_char(osiris_erp_factura_enca.fecha_factura,'yyyy-MM-dd') <= '"+entry_ano2.Text+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";
				}
				if (facturados == "CERRADOS"){
					query_fechas = "AND to_char(osiris_erp_cobros_enca.fechahora_cerrado,'yyyy-MM-dd') >= '"+entry_ano1.Text+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
									"AND to_char(osiris_erp_cobros_enca.fechahora_cerrado,'yyyy-MM-dd') <= '"+entry_ano2.Text+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";
				}
				
				
			}	  
			rango_de_fecha.Destroy();
			string orden = "";
			if(facturados == "FACTURADOS") {
				if(radiobutton_fecha.Active == true){ 
					orden = "FECHA";
				}
				if(radiobutton_cliente.Active ==true) {	
					orden = "CLIENTE";
				}
				if (this.pagados == false){			
					Console.WriteLine("entra");
					bool pagados = true;
					//new osiris.reporte_porcedimientos_facturados(pagados,rango1,rango2,query_fechas,nombrebd,LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,facturados,orden,query_cliente);
				}else{
					query_fechas =	"AND to_char(osiris_erp_factura_enca.fechahora_pago_factura,'yyyy-MM-dd') >= '"+entry_ano1.Text+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
									"AND to_char(osiris_erp_factura_enca.fechahora_pago_factura,'yyyy-MM-dd') <= '"+entry_ano2.Text+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";
										
					//new osiris.reporte_facturas_pagadas(rango1,rango2,query_fechas,nombrebd,LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,facturados,orden,query_cliente,facturas_);
				}
			}
			if (facturados == "NO FACTURADOS"){ 
					bool pagados = false;
				//new osiris.reporte_porcedimientos_facturados(pagados,rango1,rango2,query_fechas,nombrebd,LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,facturados,"","");
			}
			if (facturados == "CERRADOS"){
				//new osiris.reporte_porcedimientos_cerrados(rango1,rango2,query_fechas,nombrebd,LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,facturados,"","");
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
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
				if(busqueda == "productos") { 
					//llenando_lista_de_productos();
				}
				if(busqueda == "medicos"){ 
					llenando_lista_de_medicos();
				} 		
				if(busqueda == "paciente"){
					//llenando_lista_de_pacientes();
				}
			}
		}
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}