// created on 15/02/2008 at 10:47 a
//////////////////////////////////////////////////////////////////////
// created on 21/01/2008 at 08:28 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. (Modificaciones y Ajustes)
//				  Tec. Homero Montoya Galvan (Programaion)
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
using Gtk;
using Npgsql;
using Glade;

namespace osiris
{
	public class abonos
	{
		//Declarando ventana de cambios de datos de paciente
		[Widget] Gtk.Window abonar_procedimientos  = null;
		[Widget] Gtk.Entry entry_monto_abono = null;
		[Widget] Gtk.Entry entry_monto_convenio = null;
		[Widget] Gtk.Entry entry_recibo_caja = null;
		[Widget] Gtk.Entry entry_presupuesto = null;
		[Widget] Gtk.Entry entry_paquete = null;
		[Widget] Gtk.Entry entry_dia = null;
		[Widget] Gtk.Entry entry_mes = null;
		[Widget] Gtk.Entry entry_ano = null;
		[Widget] Gtk.Entry entry_concepto_abono = null;
		[Widget] Gtk.Entry entry_total_abonos = null;
		[Widget] Gtk.Entry entry_total_convenio = null;
		[Widget] Gtk.Entry entry_saldo_deuda = null;
		[Widget] Gtk.Button button_cancela_abonopago = null;
		[Widget] Gtk.CheckButton checkbutton_nuevo_abono = null;
		[Widget] Gtk.Button button_guardar = null;
		[Widget] Gtk.Button button_imprimir = null;
		[Widget] Gtk.Button button_salir = null;
		[Widget] Gtk.Button button_resumen = null;
		[Widget] Gtk.TreeView lista_abonos = null;
		[Widget] Gtk.TreeView treeview_lista_comprserv  = null;
		[Widget] Gtk.TreeView treeview_lista_pagare = null;
		
		[Widget] Gtk.Statusbar statusbar_abonos = null;
		[Widget] Gtk.ComboBox combobox_formapago = null;
		[Widget] Gtk.ComboBox combobox_tipocomprobante = null;
		[Widget] Gtk.Button button_imprimir_comp_serv = null;
		[Widget] Gtk.Button button_imprimir_pagare = null;
		
		int PidPaciente;
		int folioservicio;
		string fecha_admision;
		string fechahora_alta;
		string nombre_paciente;
		string telefono_paciente;
		string doctor;
		string cirugia;
		string fecha_nacimiento;
		string edadpac;
		string tipo_paciente;
		int id_tipopaciente;
		string aseguradora;
		string dir_pac;
		string empresapac;
		bool apl_desc_siempre = true;
		bool apl_desc;
		string nombrecajero;		
		string LoginEmpleado;
		int idformadepago = 1;
		string monto;
		string fecha;
		string concepto;
		string idcreo;
		string recibo;
		string presupuesto;
		string paquete;
		string descripcion;
		string nombrebd;		
		string connectionString;
		string idtipocomprobante = "";
		string montoconvenio;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		TreeStore treeViewEngineabonos;
		TreeStore treeViewEnginecomprserv;
		TreeStore treeViewEnginepagare;
		//Declarando las celdas
		CellRendererText cellr0;		CellRendererText cellrt1;
		CellRendererText cellrt2;		CellRendererText cellrt3;
		CellRendererText cellrt4;		CellRendererText cellrt5;
		CellRendererText cellrt6;		CellRendererText cellrt7;
		CellRendererText cellrt8;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public abonos (	int PidPaciente_ ,int folioservicio_,string nombrebd_ ,string entry_fecha_admision_,
						string entry_fechahora_alta_,string entry_numero_factura_,string entry_nombre_paciente_,
						string entry_telefono_paciente_,string entry_doctor_,string entry_tipo_paciente_,
						string entry_aseguradora_,string edadpac_,string fecha_nacimiento_,string dir_pac_,
						string cirugia_,string empresapac_,int idtipopaciente_,string nombrecajero_,string LoginEmpleado_,
		                bool agregarmasabonos,string montoconvenio_)
		{
			//nombrebd = _nombrebd_; 			
			PidPaciente = PidPaciente_;
			folioservicio = folioservicio_;			
			fecha_admision = entry_fecha_admision_;
			fechahora_alta = entry_fechahora_alta_;
			nombre_paciente = entry_nombre_paciente_;
			telefono_paciente = entry_telefono_paciente_;
			doctor = entry_doctor_;
			cirugia = cirugia_;
			tipo_paciente = entry_tipo_paciente_;
			id_tipopaciente = idtipopaciente_;
			aseguradora = entry_aseguradora_;
			edadpac = edadpac_;
			fecha_nacimiento = fecha_nacimiento_;
			dir_pac = dir_pac_;
			empresapac = empresapac_;
			nombrecajero = nombrecajero_;
			LoginEmpleado = LoginEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			montoconvenio = montoconvenio_;
			
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "abonar_procedimientos", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        abonar_procedimientos.Show();
			crea_treeview_abonos();
			crea_treeview_comprserv();
			crea_treeview_pagare();
			llenando_lista_de_abonos();
			llenando_lista_comprobante();
			llenando_lista_pagare();
			llenado_tipo_comprobante();
			
			//llenando_lista_de_pagares();
			
			button_guardar.Clicked += new EventHandler(on_button_guardar_clicked);
			button_imprimir.Clicked += new EventHandler(on_button_imprimir_clicked);
			button_imprimir_comp_serv.Clicked += new EventHandler(on_button_imprimir_comp_serv_clicked);
			button_imprimir_pagare.Clicked += new EventHandler(on_button_imprimir_pagare_clicked);
			button_resumen.Clicked += new EventHandler(on_button_resumen_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			if (agregarmasabonos == true ){
				checkbutton_nuevo_abono.Sensitive = true;
			}else{
				checkbutton_nuevo_abono.Sensitive = false;
			}
			checkbutton_nuevo_abono.Clicked += new EventHandler(on_checkbutton_nuevo_abono_clicked);
			entry_monto_abono.Sensitive = false;
			entry_recibo_caja.Sensitive = false;
			entry_presupuesto.Sensitive = false;
			entry_paquete.Sensitive = false;
			entry_dia.Sensitive = false;
			entry_dia.Text = DateTime.Now.ToString("dd");
			entry_mes.Sensitive = false;
			entry_mes.Text = DateTime.Now.ToString("MM");
			entry_ano.Sensitive = false;
			entry_ano.Text = DateTime.Now.ToString("yyyy");
			entry_concepto_abono.Sensitive = false;
			button_guardar.Sensitive = false;
			combobox_formapago.Sensitive = false;
			combobox_tipocomprobante.Sensitive = false;
			entry_recibo_caja.IsEditable = false;
				
			entry_monto_convenio.Text = montoconvenio;
			entry_total_convenio.Text = montoconvenio;
			entry_saldo_deuda.Text = (float.Parse(entry_total_convenio.Text)-float.Parse(entry_total_abonos.Text)).ToString("F");
			
			entry_saldo_deuda.ModifyBase(StateType.Normal, new Gdk.Color(254,253,152));
			entry_presupuesto.Text = "0";
			entry_paquete.Text = "0";
			
			statusbar_abonos.Pop(0);
			statusbar_abonos.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+nombrecajero);
			statusbar_abonos.HasResizeGrip = false;
		}
		
		void llenado_tipo_comprobante()
		{
			
			CellRendererText cell3 = new CellRendererText();
			combobox_tipocomprobante.PackStart(cell3, true);
			combobox_tipocomprobante.AddAttribute(cell3,"text",0);
        
			ListStore store5 = new ListStore( typeof (string), typeof (int));
			combobox_tipocomprobante.Model = store5;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_erp_tipo_comprobante "+
               						"WHERE activo = true "+	
               						"ORDER BY id_tipo_comprobante;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read()){
					store5.AppendValues ((string) lector["descripcion_tipo_comprobante"],
									 	(int) lector["id_tipo_comprobante"] );
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();			
			combobox_tipocomprobante.Changed += new EventHandler(onComboBoxChanged_tipocomprobante);
		}
		
		void onComboBoxChanged_tipocomprobante(object sender, EventArgs args)
		{
			ComboBox combobox_tipocomprobante = sender as ComboBox;
			if (sender == null)	{	return;	}
			TreeIter iter;			
			if (combobox_tipocomprobante.GetActiveIter (out iter)){
				idtipocomprobante = combobox_tipocomprobante.Model.GetValue(iter,1).ToString().Trim();								
				entry_recibo_caja.Text = (string) classpublic.lee_ultimonumero_registrado("osiris_erp_abonos","numero_recibo_caja"," WHERE id_tipo_comprobante = '"+combobox_tipocomprobante.Model.GetValue(iter,1).ToString().Trim()+"' ");
			}
		}
		
		void crea_treeview_abonos()
		{
			treeViewEngineabonos = new TreeStore(typeof(string),//0
												typeof(string),//1
												typeof(string),//2
												typeof(string),//3
												typeof(string),//4
												typeof(string),//5
												typeof(string),//6
			                                    typeof(string),//7
												typeof(string),
			                                    typeof(bool),
			                                    typeof(bool),
			                                    typeof(string),
			                                     typeof(string));
			
			lista_abonos.Model = treeViewEngineabonos;
						
			lista_abonos.RulesHint = true;
							
			lista_abonos.RowActivated += on_button_imprimir_clicked;  // Doble click selecciono paciente
			
			TreeViewColumn col_abono = new TreeViewColumn();
			cellr0 = new CellRendererText();
			col_abono.Title = "Abonos Ralizados";
			col_abono.PackStart(cellr0, true);
			col_abono.AddAttribute (cellr0, "text", 0);
			col_abono.SortColumnId = (int) Col_proveedores.col_abono;
			
			TreeViewColumn col_fecha_abono = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_fecha_abono.Title = "Fecha del Abono";
			col_fecha_abono.PackStart(cellrt1, true);
			col_fecha_abono.AddAttribute (cellrt1, "text", 1);
			col_fecha_abono.SortColumnId = (int) Col_proveedores.col_fecha_abono;
			
			TreeViewColumn col_concepto = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_concepto.Title = "Concepto";
			col_concepto.PackStart(cellrt2, true);
			col_concepto.AddAttribute (cellrt2, "text", 2);
			col_concepto.SortColumnId = (int) Col_proveedores.col_concepto;
			
			TreeViewColumn col_id_creo = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_id_creo.Title = "Id Quien Creo";
			col_id_creo.PackStart(cellrt3, true);
			col_id_creo.AddAttribute (cellrt3, "text", 3);
			col_id_creo.SortColumnId = (int) Col_proveedores.col_id_creo;
			
			TreeViewColumn col_recibo = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_recibo.Title = "No. Recibo Caja";
			col_recibo.PackStart(cellrt4, true);
			col_recibo.AddAttribute (cellrt4, "text", 4);
			col_recibo.SortColumnId = (int) Col_proveedores.col_recibo;
			
			TreeViewColumn col_tipocomprobante = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_tipocomprobante.Title = "Tipo Comprante";
			col_tipocomprobante.PackStart(cellrt5, true);
			col_tipocomprobante.AddAttribute (cellrt5, "text", 5);
			col_tipocomprobante.SortColumnId = (int) Col_proveedores.col_tipocomprobante;
			
			TreeViewColumn col_presu = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_presu.Title = "Id Presupuesto";
			col_presu.PackStart(cellrt6, true);
			col_presu.AddAttribute (cellrt6, "text", 6);
			col_presu.SortColumnId = (int) Col_proveedores.col_presu;
			
			TreeViewColumn col_paq = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_paq.Title = "Id Paquete";
			col_paq.PackStart(cellrt7, true);
			col_paq.AddAttribute (cellrt7, "text", 7);
			col_paq.SortColumnId = (int) Col_proveedores.col_paq;
			
			TreeViewColumn col_forma_pago = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_forma_pago.Title = "Forma de Pago";
			col_forma_pago.PackStart(cellrt8, true);
			col_forma_pago.AddAttribute (cellrt8, "text", 8);
			col_forma_pago.SortColumnId = (int) Col_proveedores.col_forma_pago;
			
			TreeViewColumn col_valor_convenido = new TreeViewColumn();
			CellRendererText cellrt11 = new CellRendererText();
			col_valor_convenido.Title = "$ Convenio QX.";
			col_valor_convenido.PackStart(cellrt11, true);
			col_valor_convenido.AddAttribute (cellrt11, "text", 11);
			//col_valor_convenido.SortColumnId = (int) Col_proveedores.col_valor_convenido;
			
			TreeViewColumn col_observaciones = new TreeViewColumn();
			CellRendererText cellrt12 = new CellRendererText();
			col_observaciones.Title = "Observaciones";
			col_observaciones.PackStart(cellrt12, true);
			col_observaciones.AddAttribute (cellrt12, "text", 12);
			//col_observaciones.SortColumnId = (int) Col_proveedores.col_observaciones;
			
			lista_abonos.AppendColumn(col_abono);
			lista_abonos.AppendColumn(col_fecha_abono);
			lista_abonos.AppendColumn(col_valor_convenido);
			lista_abonos.AppendColumn(col_concepto);
			lista_abonos.AppendColumn(col_observaciones);
			lista_abonos.AppendColumn(col_id_creo);
			lista_abonos.AppendColumn(col_recibo);
			lista_abonos.AppendColumn(col_tipocomprobante);
			lista_abonos.AppendColumn(col_presu);
			lista_abonos.AppendColumn(col_paq);
			lista_abonos.AppendColumn(col_forma_pago);
		}
		
		void crea_treeview_comprserv()
		{
			treeViewEnginecomprserv = new TreeStore(typeof(string),//0
												typeof(string),//1
												typeof(string),//2
												typeof(string),//3
												typeof(string),//4
												typeof(string),//5
												typeof(string),//6
												typeof(string),
			                                    typeof(string));//8
			
			treeview_lista_comprserv.Model = treeViewEnginecomprserv;
			
			treeview_lista_comprserv.RulesHint = true;
			
			TreeViewColumn col_abono = new TreeViewColumn();
			cellr0 = new CellRendererText();
			col_abono.Title = "N° Comp.Serv.";
			col_abono.PackStart(cellr0, true);
			col_abono.AddAttribute (cellr0, "text", 0);
			col_abono.SortColumnId = (int) Col_proveedores.col_abono;
			
			TreeViewColumn col_fecha_abono = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_fecha_abono.Title = "Fecha Compr.";
			col_fecha_abono.PackStart(cellrt1, true);
			col_fecha_abono.AddAttribute (cellrt1, "text", 1);
			col_fecha_abono.SortColumnId = (int) Col_proveedores.col_fecha_abono;
			
			TreeViewColumn col_concepto = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_concepto.Title = "Concepto";
			col_concepto.PackStart(cellrt2, true);
			col_concepto.AddAttribute (cellrt2, "text", 2);
			col_concepto.SortColumnId = (int) Col_proveedores.col_concepto;
			
			TreeViewColumn col_id_creo = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_id_creo.Title = "Id Quien Creo";
			col_id_creo.PackStart(cellrt3, true);
			col_id_creo.AddAttribute (cellrt3, "text", 3);
			col_id_creo.SortColumnId = (int) Col_proveedores.col_id_creo;
			
			TreeViewColumn col_recibo = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_recibo.Title = "No. Recibo";
			col_recibo.PackStart(cellrt4, true);
			col_recibo.AddAttribute (cellrt4, "text", 4);
			col_recibo.SortColumnId = (int) Col_proveedores.col_recibo;
			
			TreeViewColumn col_presu = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_presu.Title = "Id Presupuesto";
			col_presu.PackStart(cellrt5, true);
			col_presu.AddAttribute (cellrt5, "text", 5);
			col_presu.SortColumnId = (int) Col_proveedores.col_presu;
			
			TreeViewColumn col_paq = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_paq.Title = "Id Paquete";
			col_paq.PackStart(cellrt6, true);
			col_paq.AddAttribute (cellrt6, "text", 6);
			col_paq.SortColumnId = (int) Col_proveedores.col_paq;
			
			TreeViewColumn col_forma_pago = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_forma_pago.Title = "Observaciones";
			col_forma_pago.PackStart(cellrt7, true);
			col_forma_pago.AddAttribute (cellrt7, "text", 7);
			col_forma_pago.SortColumnId = (int) Col_proveedores.col_forma_pago;			
			
			treeview_lista_comprserv.AppendColumn(col_abono);
			treeview_lista_comprserv.AppendColumn(col_fecha_abono);
			treeview_lista_comprserv.AppendColumn(col_concepto);
			treeview_lista_comprserv.AppendColumn(col_id_creo);
			treeview_lista_comprserv.AppendColumn(col_recibo);
			treeview_lista_comprserv.AppendColumn(col_presu);
			treeview_lista_comprserv.AppendColumn(col_paq);
			treeview_lista_comprserv.AppendColumn(col_forma_pago);
		}
		
		void crea_treeview_pagare()
		{
			treeViewEnginepagare = new TreeStore(typeof(string),//0
												typeof(string),//1
												typeof(string),//2
												typeof(string),//3
												typeof(string),//4
												typeof(string),//5
												typeof(string),//6
												typeof(string),
			                                    typeof(string));//8
			
			treeview_lista_pagare.Model = treeViewEnginepagare;
			
			treeview_lista_pagare.RulesHint = true;
			
			TreeViewColumn col_abono = new TreeViewColumn();
			cellr0 = new CellRendererText();
			col_abono.Title = "$ Pagare"; // titulo de la cabecera de la columna, si está visible
			col_abono.PackStart(cellr0, true);
			col_abono.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1
			col_abono.SortColumnId = (int) Col_proveedores.col_abono;
			
			TreeViewColumn col_fecha_abono = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_fecha_abono.Title = "Fech.Pagare";
			col_fecha_abono.PackStart(cellrt1, true);
			col_fecha_abono.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
			col_fecha_abono.SortColumnId = (int) Col_proveedores.col_fecha_abono;
			
			TreeViewColumn col_concepto = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_concepto.Title = "Fech.Vencimiento";
			col_concepto.PackStart(cellrt2, true);
			col_concepto.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 3
			col_concepto.SortColumnId = (int) Col_proveedores.col_concepto;
			
			TreeViewColumn col_id_creo = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_id_creo.Title = "Id Quien Creo";
			col_id_creo.PackStart(cellrt3, true);
			col_id_creo.AddAttribute (cellrt3, "text", 3);
			col_id_creo.SortColumnId = (int) Col_proveedores.col_id_creo;
			
			TreeViewColumn col_recibo = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_recibo.Title = "No.Rec.Pagare";
			col_recibo.PackStart(cellrt4, true);
			col_recibo.AddAttribute (cellrt4, "text", 4);
			col_recibo.SortColumnId = (int) Col_proveedores.col_recibo;
			
			TreeViewColumn col_presu = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_presu.Title = "Observacion 1";
			col_presu.PackStart(cellrt6, true);
			col_presu.AddAttribute (cellrt6, "text", 5);
			col_presu.SortColumnId = (int) Col_proveedores.col_presu;
			
			TreeViewColumn col_paq = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_paq.Title = "Observacion 2";
			col_paq.PackStart(cellrt7, true);
			col_paq.AddAttribute (cellrt7, "text", 6);
			col_paq.SortColumnId = (int) Col_proveedores.col_paq;
			
			TreeViewColumn col_forma_pago = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_forma_pago.Title = "Observacion 3";
			col_forma_pago.PackStart(cellrt8, true);
			col_forma_pago.AddAttribute (cellrt8, "text", 7);
			col_forma_pago.SortColumnId = (int) Col_proveedores.col_forma_pago;
			
			treeview_lista_pagare.AppendColumn(col_abono);
			treeview_lista_pagare.AppendColumn(col_fecha_abono);
			treeview_lista_pagare.AppendColumn(col_concepto);
			treeview_lista_pagare.AppendColumn(col_id_creo);
			treeview_lista_pagare.AppendColumn(col_recibo);
			treeview_lista_pagare.AppendColumn(col_presu);
			treeview_lista_pagare.AppendColumn(col_paq);
			treeview_lista_pagare.AppendColumn(col_forma_pago);
		}
		
		enum Col_proveedores
		{
			col_abono,
			col_fecha_abono,
			col_concepto,
			col_id_creo,
			col_recibo,
			col_tipocomprobante,
			col_presu,
			col_paq,
			col_forma_pago,
			col_pago_o_abono
		}
		
		void llenando_lista_de_abonos()
		{
			decimal total = 0;
			entry_total_abonos.Text = total.ToString().Trim();
			treeViewEngineabonos.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
					comando.CommandText = "SELECT id_abono, "+ 
								"to_char(osiris_erp_abonos.id_abono,'9999999999') AS idabono,"+
								"folio_de_servicio,"+
								"monto_de_abono_procedimiento,"+
								"monto_de_abono_factura,"+
								"numero_recibo_caja,"+
								"to_char(numero_recibo_caja,'9999999999') AS recibocaja,"+
								"numero_factura,"+								
								"id_quien_creo,observaciones,"+
								"monto_de_abono_procedimiento, "+
								"to_char(osiris_erp_abonos.monto_de_abono_procedimiento,'9999999999.99') AS monto_abono_proc,"+
								"concepto_del_abono,"+
								"fechahora_registro,"+
								"to_char(osiris_erp_abonos.fechahora_registro,'yyyy-MM-dd HH:mi:ss') AS fecha_registro,"+
								"fecha_abono,"+
								"to_char(osiris_erp_abonos.fecha_abono,'dd-MM-yyyy') AS fechaabono,"+
								"id_presupuesto,"+
								"to_char(id_presupuesto,'9999999999') AS presupuesto, "+
								"id_paquete,pago,abono,monto_convenio,"+
								"osiris_erp_abonos.id_forma_de_pago,"+ 
								"to_char(id_paquete,'9999999999') AS paquete,"+
								"osiris_erp_forma_de_pago.id_forma_de_pago,descripcion_forma_de_pago AS descripago,descripcion_tipo_comprobante "+
								"FROM osiris_erp_abonos,osiris_erp_forma_de_pago,osiris_erp_tipo_comprobante "+
								"WHERE osiris_erp_abonos.folio_de_servicio = '"+this.folioservicio.ToString()+"' "+
								"AND osiris_erp_abonos.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
								"AND osiris_erp_abonos.eliminado = 'false' "+
								"AND osiris_erp_abonos.id_tipo_comprobante = osiris_erp_tipo_comprobante.id_tipo_comprobante "+
								"ORDER BY osiris_erp_abonos.folio_de_servicio;";
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					treeViewEngineabonos.AppendValues ((string) lector["monto_abono_proc"],//0
													(string) lector["fechaabono"],//1
													(string) lector["concepto_del_abono"],//2
													(string) lector["id_quien_creo"],//3
													(string) lector["recibocaja"],//4
					                                (string) lector["descripcion_tipo_comprobante"],//5   
													(string) lector["presupuesto"],//6
													(string) lector["paquete"],//7
													(string) lector["descripago"],
					                                (bool) lector["pago"],
					                                (bool) lector["abono"],
					                                float.Parse(lector["monto_convenio"].ToString()).ToString("F"),
					                                   (string) lector["observaciones"].ToString().Trim());
					total += decimal.Parse((string) lector["monto_abono_proc"]);
					entry_total_abonos.Text = total.ToString("F");
				}
			}catch (NpgsqlException ex){
   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void llenando_lista_comprobante()
		{
			decimal total = 0;
			treeViewEnginecomprserv.Clear();	// Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
					comando.CommandText = "SELECT to_char(numero_comprobante_servicio,'99999999999') AS reciboservicio,id_quien_creo," +
										"to_char(osiris_erp_comprobante_servicio.fecha_comprobante,'yyyy-MM-dd') AS fechacomprobante,concepto_del_comprobante "+
									"FROM osiris_erp_comprobante_servicio "+
									"WHERE osiris_erp_comprobante_servicio.eliminado = 'false' "+
									"AND osiris_erp_comprobante_servicio.folio_de_servicio = '"+this.folioservicio.ToString()+"' ";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){	
					treeViewEnginecomprserv.AppendValues ((string) lector["reciboservicio"],
					                                   (string) lector["fechacomprobante"],
					                                   (string) lector["concepto_del_comprobante"],
					                                   (string) lector["id_quien_creo"],
					                                   " ",
					                                   " ");
					//total += decimal.Parse((string) lector["abono"]);
					// entry_total_abonos.Text = total.ToString();
				}
			}catch (NpgsqlException ex){
   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void llenando_lista_pagare()
		{
			decimal total = 0;
			treeViewEnginepagare.Clear();	// Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
					comando.CommandText = "SELECT to_char(numero_comprobante_pagare,'99999999999') AS reciboservicio,id_quien_creo," +
										"to_char(osiris_erp_comprobante_pagare.fecha_comprobante,'yyyy-MM-dd') AS fechacomprobante,"+
										"to_char(osiris_erp_comprobante_pagare.monto_pagare,'9999999999.99') AS montopagare, "+
										"to_char(osiris_erp_comprobante_pagare.fecha_vencimiento_pagare,'yyyy-MM-dd') AS fechavencimientopagare,"+
										"concepto_del_comprobante "+
									"FROM osiris_erp_comprobante_pagare "+
									"WHERE osiris_erp_comprobante_pagare.eliminado = 'false' "+
									"AND osiris_erp_comprobante_pagare.folio_de_servicio = '"+this.folioservicio.ToString()+"' ";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){	
					treeViewEnginepagare.AppendValues ((string) lector["montopagare"],
					                                   (string) lector["fechacomprobante"],
					                                   (string) lector["fechavencimientopagare"],
					                                   (string) lector["id_quien_creo"],
					                                   (string) lector["reciboservicio"],
					                                   " ");
					//total += decimal.Parse((string) lector["abono"]);
					// entry_total_abonos.Text = total.ToString();
				}
			}catch (NpgsqlException ex){
   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_checkbutton_nuevo_abono_clicked(object sender, EventArgs args)
		{
			if(checkbutton_nuevo_abono.Active == true) { 
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
				MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de querer realizar un nuevo abono?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
	 			if (miResultado == ResponseType.Yes){
	 				llenado_formapago("nuevo",0,"");
	 				entry_monto_abono.Sensitive = true;
	 				entry_recibo_caja.Sensitive = true;
					entry_presupuesto.Sensitive = true;
					entry_paquete.Sensitive = true;
					entry_dia.Sensitive = true;
					entry_dia.Text = DateTime.Now.ToString("dd");
					entry_mes.Sensitive = true;
					entry_mes.Text = DateTime.Now.ToString("MM");
					entry_ano.Sensitive = true;
					entry_ano.Text = DateTime.Now.ToString("yyyy");
					entry_concepto_abono.Sensitive = true;
					button_guardar.Sensitive = true;
					button_imprimir.Sensitive = true;
					this.button_resumen.Sensitive = true;
					this.combobox_formapago.Sensitive = true;
					combobox_tipocomprobante.Sensitive = true;
				}else{
					checkbutton_nuevo_abono.Active = false;
				}
			}
		}
		
		
		void on_button_guardar_clicked(object sender, EventArgs args)
		{
			if(checkbutton_nuevo_abono.Active == true){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
										ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");
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
		 				comando4.CommandText = "SELECT numero_recibo_caja,folio_de_servicio "+
										"FROM osiris_erp_abonos "+
										"WHERE numero_recibo_caja = '"+this.entry_recibo_caja.Text+"' "+
										"AND id_tipo_comprobante = '"+idtipocomprobante+"' "+
										"LIMIT 1 ;";
		 					
	 					NpgsqlDataReader lector4 = comando4.ExecuteReader ();
								
               			if(lector4.Read() || (string) idtipocomprobante == "1"){
               				MessageDialog msgBox6 = new MessageDialog (MyWin,DialogFlags.Modal,
							MessageType.Info,ButtonsType.Ok,"Este recibo de caja ya existe o no tiene tipo de Comprobante, verifique...");
							msgBox6.Run ();msgBox6.Destroy();
               			}else{
		               		NpgsqlConnection conexion;
							conexion = new NpgsqlConnection (connectionString+nombrebd);
		    	        	// Verifica que la base de datos este conectada
		    	        	try{
								conexion.Open ();
								NpgsqlCommand comando; 
								comando = conexion.CreateCommand ();
					 			comando.CommandText = "INSERT INTO osiris_erp_abonos("+
												  	"monto_de_abono_procedimiento, "+//2
													"numero_recibo_caja, "+//3
													"id_quien_creo, "+//4
													"concepto_del_abono, "+//5
													"observaciones,"+
													"fechahora_registro, "+//6
													"fecha_abono, "+//7
													"id_presupuesto, "+//8
													"id_paquete ,"+//9
													"id_forma_de_pago,"+
													"id_tipo_comprobante,"+
													"tipo_comprobante,"+
													"abono,"+
													"monto_convenio,"+
													"folio_de_servicio," +
													"pid_paciente)"+
													"VALUES ('"+
			 										(string) this.entry_monto_abono.Text.Trim().ToUpper()+"','"+//2
			 										(string) this.entry_recibo_caja.Text.Trim().ToUpper()+"','"+//3										  
			 										LoginEmpleado+"','"+//4
			 										"ABONO A PROCEDIMIENTO','"+//5
													(string) this.entry_concepto_abono.Text.Trim().ToUpper()+"','"+
			 										DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+//6
			 										(string) this.entry_ano.Text+" "+this.entry_mes.Text+" "+this.entry_dia.Text+"','"+//7
			 										(string) this.entry_presupuesto.Text.Trim().ToUpper()+"','"+//8
			 										(string) this.entry_paquete.Text.Trim().ToUpper()+"','"+//9
													idformadepago.ToString()+"','"+
			 										idtipocomprobante+"','"+
													"ABONO"+"','"+
													"true"+"','"+
													entry_monto_convenio.Text.ToString().Trim()+"','"+
													folioservicio+"','"+//"','"+//10
			 										PidPaciente+"');";
		 						comando.ExecuteNonQuery();    	    	       	comando.Dispose();
		 						
		 						NpgsqlConnection conexion2; 
								conexion2 = new NpgsqlConnection (connectionString+nombrebd);
		    	        			
		    	        		//Verifica que la base de datos este conectada
		    	        		try{
					    	       	conexion2.Open ();
									NpgsqlCommand comando2; 
									comando2 = conexion2.CreateCommand ();
						 			comando2.CommandText = "UPDATE osiris_erp_cobros_enca SET tiene_abono = 'true',"+
						 										"total_abonos = total_abonos + '"+entry_monto_abono.Text+"', "+
																"monto_convenio = '"+entry_monto_convenio.Text+"', "+
																"reservacion = 'true' "+
																"WHERE folio_de_servicio = '"+this.folioservicio.ToString()+"' ;";
						 			//Console.WriteLine(comando2.CommandText);		
					 				comando2.ExecuteNonQuery();    	    	       	comando2.Dispose();
								}catch(NpgsqlException ex){
					   				MessageDialog msgBoxError5 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
														MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
									msgBoxError5.Run ();					msgBoxError5.Destroy();
								}
					       		conexion2.Close ();
				 				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Info,ButtonsType.Close,"El Abono se guardo con exito");
								msgBoxError.Run ();					msgBoxError.Destroy();
								llenando_lista_de_abonos();									
							}catch(NpgsqlException ex){
					   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
														MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
										msgBoxError.Run ();					msgBoxError.Destroy();
		   					}
					       	conexion.Close ();
		       				entry_monto_abono.Sensitive = false;
							entry_recibo_caja.Sensitive = false;
							entry_presupuesto.Sensitive = false;
							entry_paquete.Sensitive = false;
							entry_dia.Sensitive = false;
							entry_mes.Sensitive = false;
							entry_ano.Sensitive = false;
							entry_concepto_abono.Sensitive = false;
							combobox_formapago.Sensitive = false;
							entry_monto_abono.Text = "";
							entry_recibo_caja.Text = "";
							entry_presupuesto.Text = "";
							entry_paquete.Text = "";
							entry_dia.Text = DateTime.Now.ToString("dd");
							entry_mes.Text = DateTime.Now.ToString("MM");
							entry_ano.Text = DateTime.Now.ToString("yyyy");
							entry_dia.Text = "";
							entry_mes.Text = "";
							entry_ano.Text = "";
							entry_concepto_abono.Text = "";
							this.checkbutton_nuevo_abono.Active = false;
							this.button_guardar.Sensitive = false;
						}
					}catch(NpgsqlException ex){
	   					MessageDialog msgBoxError5 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError5.Run ();					msgBoxError5.Destroy();
	       			}
	       			conexion4.Close();
	       		}
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
               						"WHERE proveedor = false "+	
               						"ORDER BY descripcion_forma_de_pago;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read()){
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
		
		void onComboBoxChanged_formapago (object sender, EventArgs args)
		{
			ComboBox combobox_formapago = sender as ComboBox;
			if (sender == null) {return;}
			TreeIter iter;
			if (combobox_formapago.GetActiveIter (out iter)){ 
				idformadepago = (int) combobox_formapago.Model.GetValue(iter,1);
			}
		}
				
		void on_button_imprimir_clicked(object sender, EventArgs args)
		{
			imprime_comprobante_resumen("caja");
		}
		
		void on_button_imprimir_comp_serv_clicked(object sender, EventArgs args)
		{
			imprime_comprobante_resumen("comprobante");
		}
		
		void on_button_imprimir_pagare_clicked(object sender, EventArgs args)
		{
			imprime_comprobante_resumen("pagare");
		}
		
		void on_button_resumen_clicked(object sender, EventArgs args)
		{
			imprime_comprobante_resumen("resumen");
		}
		
		void imprime_comprobante_resumen(string tipo_reporte)
		{
			Console.WriteLine(tipo_reporte);
			TreeModel model;
			TreeIter iterSelected;			
			if (tipo_reporte == "caja"){
				if (lista_abonos.Selection.GetSelected(out model, out iterSelected)){
	 				monto = (string) model.GetValue(iterSelected, 0); 				
	 				fecha = (string) model.GetValue(iterSelected, 1);
					concepto = (string) model.GetValue(iterSelected, 2);
					idcreo = (string) model.GetValue(iterSelected, 3);
					recibo = (string) model.GetValue(iterSelected, 4);
					presupuesto = (string) model.GetValue(iterSelected, 5);
					paquete = (string) model.GetValue(iterSelected, 6);
					descripcion = (string) model.GetValue(iterSelected, 7);
					
					if((bool) model.GetValue(iterSelected, 9)){
						// Pago en Caja total del procedimiento
						new caja_comprobante(int.Parse(recibo),"CAJA", folioservicio,"SELECT osiris_erp_cobros_deta.folio_de_servicio AS foliodeservicio,osiris_erp_cobros_deta.pid_paciente AS pidpaciente, "+ 
						"osiris_his_tipo_admisiones.descripcion_admisiones,aplicar_iva, "+
						"osiris_his_tipo_admisiones.id_tipo_admisiones AS idadmisiones,"+
						"osiris_grupo_producto.descripcion_grupo_producto, "+
						"osiris_productos.id_grupo_producto,  "+
						"to_char(osiris_erp_cobros_deta.porcentage_descuento,'999.99') AS porcdesc, "+
						"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,  "+
						"to_char(osiris_erp_cobros_enca.fechahora_creacion,'HH:mi') AS horacreacion,  "+
						"to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,descripcion_producto, "+
						"to_char(osiris_erp_cobros_deta.cantidad_aplicada,'99999999.99') AS cantidadaplicada, "+
						"to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99') AS preciounitario, "+
						"ltrim(to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99')) AS preciounitarioprod, "+
						"to_char(osiris_erp_cobros_deta.iva_producto,'999999.99') AS ivaproducto, "+
						//"to_char(osiris_erp_cobros_deta.precio_por_cantidad,'999999.99') AS ppcantidad, "+
						"to_char(osiris_erp_cobros_deta.cantidad_aplicada * osiris_erp_cobros_deta.precio_producto,'99999999.99') AS ppcantidad,"+
						"to_char(osiris_productos.precio_producto_publico,'999999999.99999') AS preciopublico,osiris_erp_abonos.numero_recibo_caja AS numerorecibo,"+
						"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_completo, "+
						"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente, to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente, "+
					    "telefono_particular1_paciente,osiris_erp_abonos.observaciones AS observacionesvarias,osiris_erp_abonos.concepto_del_abono AS concepto_comprobante,"+
						"osiris_erp_cobros_enca.id_empresa,descripcion_empresa,osiris_erp_cobros_enca.nombre_medico_encabezado,"+
					    "to_char(monto_de_abono_procedimiento,'999999999.99') AS montodelabono,descripcion_tipo_comprobante "+
				        "FROM osiris_erp_cobros_deta,osiris_his_tipo_admisiones,osiris_productos,osiris_grupo_producto,osiris_erp_abonos,osiris_his_paciente,osiris_erp_cobros_enca,osiris_empresas,osiris_erp_tipo_comprobante "+
						"WHERE osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
						"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto  "+ 
						"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
						"AND osiris_erp_cobros_deta.pid_paciente = osiris_his_paciente.pid_paciente "+
				        "AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
					    "AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_cobros_deta.folio_de_servicio "+
						"AND osiris_erp_cobros_deta.eliminado = 'false' "+
					    "AND osiris_erp_abonos.folio_de_servicio = osiris_erp_cobros_deta.folio_de_servicio "+
				        "AND osiris_erp_tipo_comprobante.id_tipo_comprobante = osiris_erp_abonos.id_tipo_comprobante ",nombrecajero);						
					}
					
					if((bool) model.GetValue(iterSelected, 10)){
						// Pago en Caja total del procedimiento
						// Console.WriteLine("Es un Abono");
						new caja_comprobante(int.Parse(recibo),"ABONO", folioservicio,"SELECT osiris_erp_cobros_enca.folio_de_servicio AS foliodeservicio,osiris_erp_cobros_enca.pid_paciente AS pidpaciente," +
							"osiris_erp_abonos.numero_recibo_caja AS numerorecibo," +
							"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_completo," +
							"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente," +
						    "to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,  "+
							"to_char(osiris_erp_cobros_enca.fechahora_creacion,'HH:mi') AS horacreacion,  "+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente," +
							"telefono_particular1_paciente,osiris_erp_abonos.observaciones AS observacionesvarias,osiris_erp_abonos.concepto_del_abono AS concepto_comprobante," +
							"osiris_erp_cobros_enca.id_empresa,descripcion_empresa,osiris_erp_cobros_enca.nombre_medico_encabezado,to_char(monto_de_abono_procedimiento,'999999999.99') AS montodelabono," +
							"descripcion_tipo_comprobante " +
							"FROM osiris_erp_abonos,osiris_his_paciente,osiris_erp_cobros_enca,osiris_empresas,osiris_erp_tipo_comprobante "+
							"WHERE osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
							"AND osiris_erp_abonos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio " +
							"AND osiris_erp_abonos.id_tipo_comprobante = osiris_erp_tipo_comprobante.id_tipo_comprobante "+
							"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente ",nombrecajero);
						
						/*
						SELECT osiris_erp_abonos.numero_recibo_caja,osiris_erp_abonos.folio_de_servicio,descripcion_tipo_comprobante,abono 
						FROM osiris_erp_abonos,osiris_erp_cobros_enca,osiris_erp_tipo_comprobante,osiris_empresas
						WHERE osiris_erp_abonos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio 
						AND osiris_erp_abonos.id_tipo_comprobante = osiris_erp_tipo_comprobante.id_tipo_comprobante 
						AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa
						AND osiris_erp_abonos.folio_de_servicio = '8013';
						*/
					}
				}
			}
						
			if(tipo_reporte == "comprobante"){
				if (treeview_lista_comprserv.Selection.GetSelected(out model, out iterSelected)){					
					recibo = (string) model.GetValue(iterSelected, 0);					
					new caja_comprobante(int.Parse(recibo),"SERVICIO", folioservicio,"SELECT osiris_erp_cobros_deta.folio_de_servicio AS foliodeservicio,osiris_erp_cobros_deta.pid_paciente AS pidpaciente, "+ 
						"osiris_his_tipo_admisiones.descripcion_admisiones,aplicar_iva, "+
						"osiris_his_tipo_admisiones.id_tipo_admisiones AS idadmisiones,"+
						"osiris_grupo_producto.descripcion_grupo_producto, "+
						"osiris_productos.id_grupo_producto,  "+
						"to_char(osiris_erp_cobros_deta.porcentage_descuento,'999.99') AS porcdesc, "+
						"to_char(osiris_erp_cobros_deta.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,  "+
						"to_char(osiris_erp_cobros_deta.fechahora_creacion,'HH:mm') AS horacreacion,  "+
						"to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,descripcion_producto, "+
						"to_char(osiris_erp_cobros_deta.cantidad_aplicada,'99999999.99') AS cantidadaplicada, "+
						"to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99') AS preciounitario, "+
						"ltrim(to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99')) AS preciounitarioprod, "+
						"to_char(osiris_erp_cobros_deta.iva_producto,'999999.99') AS ivaproducto, "+
						//"to_char(osiris_erp_cobros_deta.precio_por_cantidad,'999999.99') AS ppcantidad, "+
						"to_char(osiris_erp_cobros_deta.cantidad_aplicada * osiris_erp_cobros_deta.precio_producto,'99999999.99') AS ppcantidad,"+
						"to_char(osiris_productos.precio_producto_publico,'999999999.99999') AS preciopublico,osiris_erp_comprobante_servicio.numero_comprobante_servicio AS numerorecibo,"+
						"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_completo, "+
						"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente, to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente, "+
					    "telefono_particular1_paciente,"+
					    "osiris_erp_comprobante_servicio.observaciones || ' ' || osiris_erp_comprobante_servicio.observaciones2 || ' ' || osiris_erp_comprobante_servicio.observaciones3 AS observacionesvarias,"+
					     "osiris_erp_comprobante_servicio.concepto_del_comprobante AS concepto_comprobante,"+
						"osiris_erp_cobros_enca.id_empresa,descripcion_empresa,osiris_erp_cobros_enca.nombre_medico_encabezado "+
					    //"to_char(monto_de_abono_procedimiento,'999999999.99') AS montodelabono "+
				        "FROM osiris_erp_cobros_deta,osiris_his_tipo_admisiones,osiris_productos,osiris_grupo_producto,osiris_erp_comprobante_servicio,osiris_his_paciente,osiris_erp_cobros_enca,osiris_empresas "+
						"WHERE osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
						"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto  "+ 
						"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
						"AND osiris_erp_cobros_deta.pid_paciente = osiris_his_paciente.pid_paciente "+
				        "AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
					    "AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_cobros_deta.folio_de_servicio "+
						"AND osiris_erp_cobros_deta.eliminado = 'false' ", nombrecajero );
				}
			}
			
			if(tipo_reporte == "pagare"){
				if (treeview_lista_pagare.Selection.GetSelected(out model, out iterSelected)){
					recibo = (string) model.GetValue(iterSelected, 4);
					new caja_comprobante (int.Parse(recibo), "PAGARE", folioservicio,"SELECT osiris_erp_cobros_enca.folio_de_servicio AS foliodeservicio,osiris_erp_cobros_enca.pid_paciente AS pidpaciente," +
							"osiris_erp_comprobante_pagare.numero_comprobante_pagare AS numerorecibo," +
							"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_completo," +
							"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente," +
							"direccion_paciente,numero_casa_paciente,numero_departamento_paciente,codigo_postal_paciente,colonia_paciente,municipio_paciente,estado_paciente," +
						    "to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,  "+
							"to_char(osiris_erp_cobros_enca.fechahora_creacion,'HH:mi') AS horacreacion,  "+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente," +
							"telefono_particular1_paciente,osiris_erp_comprobante_pagare.observaciones AS observacionesvarias,osiris_erp_comprobante_pagare.concepto_del_comprobante AS concepto_comprobante," +
							"osiris_erp_cobros_enca.id_empresa,descripcion_empresa,osiris_erp_cobros_enca.nombre_medico_encabezado,to_char(osiris_erp_comprobante_pagare.monto_pagare,'999999999.99') AS montodelabono," +
							"descripcion_tipo_comprobante,to_char(fecha_vencimiento_pagare,'dd-mm-yyyy') AS vencimiento_pagare " +
							"FROM osiris_erp_comprobante_pagare,osiris_his_paciente,osiris_erp_cobros_enca,osiris_empresas,osiris_erp_tipo_comprobante "+
							"WHERE osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
							"AND osiris_erp_comprobante_pagare.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio " +
							"AND osiris_erp_comprobante_pagare.id_tipo_comprobante = osiris_erp_tipo_comprobante.id_tipo_comprobante "+
							"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente " +
							"AND osiris_erp_comprobante_pagare.eliminado = 'false' ",nombrecajero);			
				}
			}
			if (tipo_reporte == "resumen"){
				
			
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