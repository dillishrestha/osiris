// created on 5/18/2007 at 3:25 PM
////////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Juan Antonio Peña Gonzalez (Programacion)
//				  Ing. Daniel Olivares (Preprogramacion)
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
// Proposito	: Pagos en Caja 
// Objeto		: cargos_hospitalizacion.cs
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class quirofano
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando ventana principal de Hospitalizacion
		[Widget] Gtk.Window menu_quirofano;
		[Widget] Gtk.Button button_cargos_pacientes;
		[Widget] Gtk.Button button_soli_material;
		[Widget] Gtk.Button button_autorizacion_medicamento;
		[Widget] Gtk.Button button_inv_subalmacen;
		[Widget] Gtk.Button button_asignacion_habitacion;
		[Widget] Gtk.Button button_traspaso_subalmacenes;
		[Widget] Gtk.Button button_programacion_cirugias;
		[Widget] Gtk.Button button_requisicion_materiales;
		[Widget] Gtk.Button button_reportes = null;
		
		// Ventana de Rango de Fecha
		[Widget] Gtk.Window rango_de_fecha;
		[Widget] Gtk.Entry entry_dia1;
		[Widget] Gtk.Entry entry_dia2;
		[Widget] Gtk.Entry entry_mes1;
		[Widget] Gtk.Entry entry_mes2;
		[Widget] Gtk.Entry entry_ano1;
		[Widget] Gtk.Entry entry_ano2;
		[Widget] Gtk.Entry entry_referencia_inicial;
		[Widget] Gtk.Entry entry_cliente;
		[Widget] Gtk.Label label_orden;
		[Widget] Gtk.Label label_nom_cliente;
		[Widget] Gtk.Label label142;
		[Widget] Gtk.RadioButton radiobutton_cliente;
		[Widget] Gtk.RadioButton radiobutton_fecha;
		[Widget] Gtk.Button button_busca_cliente = null;
		[Widget] Gtk.Button button_imprime_rangofecha = null;
		[Widget] Gtk.CheckButton checkbutton_impr_todo_proce = null;
		[Widget] Gtk.CheckButton checkbutton_todos_los_clientes = null;
		[Widget] Gtk.CheckButton checkbutton_export_to = null;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		string query_rango_fechas;
		class_conexion conexion_a_DB = new class_conexion();
		
		public quirofano (string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "quirofano.glade", "menu_quirofano", null);
			gxml.Autoconnect (this);        
			////// Muestra ventana de Glade
			menu_quirofano.Show();
			
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_programacion_cirugias.Clicked += new EventHandler (on_button_programacion_cirugias_clicked);
			button_requisicion_materiales.Clicked += new EventHandler(on_button_requisicion_materiales_clicked);
			button_cargos_pacientes.Clicked += new EventHandler(on_button_cargos_pacientes_clicked);
			button_soli_material.Clicked += new EventHandler(on_button_soli_material_clicked);
			button_autorizacion_medicamento.Clicked += new EventHandler(on_button_autorizacion_medicamento_clicked);
			button_inv_subalmacen.Clicked += new EventHandler(on_button_inv_subalmacen_clicked);
			button_asignacion_habitacion.Clicked += new EventHandler(on_button_asignacion_habitacion_clicked);
			button_traspaso_subalmacenes.Clicked += new EventHandler(on_button_traspaso_subalmacenes_clicked);
			button_reportes.Clicked += new EventHandler(on_button_reportes_clicked);
		}
		
		void on_button_programacion_cirugias_clicked(object sender, EventArgs args)
		{
			new osiris.calendario_citas(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,2);
		}
		
		void on_button_requisicion_materiales_clicked(object sender, EventArgs args)
		{
			// centro de costo se debe enviar en el array y la clase 700   --   700
			int [] array_idtipoadmisiones = { 0, 700};
			new osiris.requisicion_materiales_compras(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"QUIROFANO",700,"AND agrupacion IN ('OTR','MD1') ",array_idtipoadmisiones,0);
		}
		
		void on_button_cargos_pacientes_clicked(object sender, EventArgs args)
		{
			//new osiris.cargos_quirofano(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
			new osiris.cargos_modulos_medicos(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,700,"QUIROFANO",5,"");
		}
		
		void on_button_soli_material_clicked(object sender, EventArgs args)
		{
			new osiris.solicitud_material(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,5);
		}
		
		void on_button_autorizacion_medicamento_clicked(object sender, EventArgs args)
		{
			 new osiris.orden_compra_urgencias(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,5,"QUIROFANO",0,"");
		}
		
		void on_button_inv_subalmacen_clicked(object sender, EventArgs args)
		{
			new osiris.inventario_sub_almacen(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,5,"QUIROFANO",1);
		}
		
		void on_button_traspaso_subalmacenes_clicked(object sender, EventArgs args)
		{
			new osiris.inventario_sub_almacen(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,5,"QUIROFANO",2);
		}
		
		void on_button_asignacion_habitacion_clicked(object sender, EventArgs args)
		{
		   new osiris.asignacion_de_habitacion(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd, 0);
		}
		
		void on_button_reportes_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "rango_de_fecha", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
        	rango_de_fecha.Show();
			
			checkbutton_impr_todo_proce.Label = "Imprime TODO";
			entry_referencia_inicial.IsEditable = false;
			entry_referencia_inicial.Text = DateTime.Now.ToString("dd-MM-yyyy");
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
        	button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
        	button_imprime_rangofecha.Clicked += new EventHandler(imprime_reporte_cirugias);
        	label_orden.Hide();
			label_nom_cliente.Hide();
			label142.Hide();
			radiobutton_cliente.Hide();
			radiobutton_fecha.Hide();
			checkbutton_todos_los_clientes.Hide();
			entry_referencia_inicial.Hide();
			entry_cliente.Hide();
			button_busca_cliente.Hide();
		}
		
		void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}
		
		void imprime_reporte_cirugias(object sender, EventArgs args)
		{
			query_rango_fechas = "AND to_char(osiris_erp_pases_qxurg.fechahora_creacion,'yyyy-MM-dd') >= '"+(string) entry_ano1.Text.ToString()+"-"+(string) entry_mes1.Text.ToString()+"-"+(string) entry_dia1.Text.ToString()+"'  "+
									"AND to_char(osiris_erp_pases_qxurg.fechahora_creacion,'yyyy-MM-dd') <= '"+(string) entry_ano2.Text.ToString()+"-"+(string) entry_mes2.Text.ToString()+"-"+(string) entry_dia2.Text.ToString()+"' ";
			
			if(checkbutton_export_to.Active == true){
				string query_sql = "SELECT DISTINCT ON (osiris_erp_pases_qxurg.folio_de_servicio) osiris_erp_pases_qxurg.folio_de_servicio, osiris_erp_pases_qxurg.id_secuencia AS nro_pase,osiris_erp_pases_qxurg.folio_de_servicio AS foliodeservicio,to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente," +
						"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo," +
						"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente,to_char(to_number(to_char(age('2012-02-14 10:45:24',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente," +
						"osiris_his_paciente.sexo_paciente,osiris_erp_cobros_enca.nombre_medico_tratante,osiris_erp_pases_qxurg.id_quien_creo,nombre1_empleado || ' ' || nombre2_empleado || ' ' || apellido_paterno_empleado || ' ' || apellido_materno_empleado AS nombresolicitante," +
						"to_char(osiris_erp_pases_qxurg.fechahora_creacion,'yyyy-MM-dd') AS fechapaseqx,osiris_erp_pases_qxurg.id_tipo_admisiones,descripcion_admisiones,osiris_erp_cobros_enca.id_empresa AS idempresa,osiris_empresas.descripcion_empresa," +
						"osiris_erp_cobros_enca.id_aseguradora,osiris_aseguradoras.descripcion_aseguradora,descripcion_diagnostico_movcargos AS motivo_ingreso,descripcion_tipo_paciente," +
						"osiris_erp_movcargos.id_tipo_cirugia,descripcion_cirugia," +
						"osiris_erp_cobros_enca.id_medico_tratante,osiris_his_medicos.nombre_medico AS medicotratante,nombre_medico_encabezado AS dr_solicita,"+
						"osiris_erp_cobros_enca.observaciones1,total_abonos+total_pago AS pagosabonos,cerrado,monto_convenio AS montoconvenido "+
						"FROM osiris_erp_pases_qxurg,osiris_his_tipo_admisiones,osiris_erp_cobros_enca,osiris_his_paciente,osiris_empleado,osiris_empresas,osiris_aseguradoras,osiris_erp_movcargos,osiris_his_tipo_pacientes,osiris_his_tipo_cirugias,osiris_his_medicos "+
						"WHERE osiris_erp_pases_qxurg.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones " +
						"AND osiris_erp_pases_qxurg.pid_paciente = osiris_his_paciente.pid_paciente " +
						"AND osiris_erp_pases_qxurg.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio " +
						"AND osiris_erp_pases_qxurg.id_quien_creo = osiris_empleado.login_empleado " +
						"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa " +
						"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
						"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
						"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_pases_qxurg.folio_de_servicio " +
						"AND osiris_erp_movcargos.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia " +
						"AND osiris_erp_cobros_enca.id_medico_tratante = osiris_his_medicos.id_medico " +
						"AND osiris_erp_cobros_enca.cancelado = 'false' " +
						//"AND osiris_erp_movcargos.id_anestesiologo = osiris_his_medicos.id_medico "+ 
						query_rango_fechas+
						"ORDER BY osiris_erp_pases_qxurg.folio_de_servicio;";
				string[] args_names_field = {"fechapaseqx","nro_pase","foliodeservicio","pagosabonos","montoconvenido","pidpaciente","nombre_completo","motivo_ingreso","descripcion_tipo_paciente","descripcion_cirugia","dr_solicita","medicotratante","cerrado"};
				string[] args_type_field = {"string","float","float","float","float","float","string","string","string","string","string","string","string"};
				string[] args_field_text = {"id_producto","nombre_producto","nro_serie","tipo_anestesia","id_anestesiologo","nombre_anestesiologo"};
				string[] args_more_title = {""};
				// class_crea_ods.cs
				//Console.WriteLine(query_sql);
				new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field,true,args_field_text,"observaciones1",false,args_more_title);
			}else{
			
			}
		}
		
		void on_cierraventanas_clicked(object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}