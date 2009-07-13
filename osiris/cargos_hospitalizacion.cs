////////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Hospital Santa Cecilia
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
	public class cargos_hospitalizacion
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		
		// Declarando ventana principal de Hospitalizacion
		[Widget] Gtk.Window cargos_hospital;
		[Widget] Gtk.Entry entry_folio_servicio;
		[Widget] Gtk.Entry entry_fecha_ingreso;
		[Widget] Gtk.Entry entry_hora_ingreso;
		[Widget] Gtk.Entry entry_pid_paciente;
		[Widget] Gtk.Entry entry_nombre_paciente;
		[Widget] Gtk.Entry entry_fecha_naci_paciente;
		[Widget] Gtk.Entry entry_edad_paciente;
		[Widget] Gtk.Entry entry_cirugia;
		[Widget] Gtk.Entry entry_medico;
		[Widget] Gtk.Entry entry_especialidad_medico;
		[Widget] Gtk.Entry entry_diagnostico;
		[Widget] Gtk.Entry entry_num_habitacion;
		[Widget] Gtk.Entry entry_cantidad_productos;
		[Widget] Gtk.TreeView lista_de_productos_apli;
		[Widget] Gtk.TreeView lista_de_medicamentos;
		[Widget] Gtk.Button button_selec_folio;
		[Widget] Gtk.Button button_busca_paciente;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.Button button_grabar;
				
		/////Declarando buqueda paciente
		[Widget] Gtk.Window busca_paciente;
		[Widget] Gtk.TreeView lista_de_Pacientes;
		[Widget] Gtk.Button button_buscar_busqueda;
		[Widget] Gtk.Button button_nuevo_paciente;
		[Widget] Gtk.RadioButton radiobutton_busca_apellido;
		[Widget] Gtk.RadioButton radiobutton_busca_nombre;
		[Widget] Gtk.RadioButton radiobutton_busca_expediente;
		////busqueda producto
		[Widget] Gtk.Window busca_producto;
		[Widget] Gtk.TreeView lista_de_producto;
		[Widget] Gtk.Button button_buscar_producto;
		
	
		public int folioservicio = 0;
 		public int idtipointernamiento = 500;
 		public string tipointernamiento = " ";
		public string id_produ = " ";
		public string desc_produ = " ";
		public string grupoprodu = " ";
		public string grupoprodu1 =" ";
		public string grupoprodu2 = " ";
		public string fechahoraprod = " ";
 		
		public string LoginEmpleado;

		public string connectionString = "Server=192.168.1.4;" +
						"Port=5432;" +
						"User ID=admin;" +
						"Password=1qaz2wsx;";
		public string nombrebd;
		
		private TreeStore treeViewEngineServicio;
		private TreeStore treeViewEngineBusca;
		private TreeStore treeViewEngineBusca2;
		private ListStore treeViewEngineExtras;
				

		public cargos_hospitalizacion (string LoginEmp, string NomEmpleado, string AppEmpleado, string ApmEmpleado, string _nombrebd_) 
		{
			LoginEmpleado = LoginEmp;
			nombrebd = _nombrebd_ ;

			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "cargos_hospital", null);
			gxml.Autoconnect (this);        
			////// Muestra ventana de Glade
			cargos_hospital.Show();
			//////crea lista de productos
			crea_treeview_productos();
			//////crea lista de medicamentos
			crea_treeview_cargextra();
			///// Graba el pago para el cierre de esta cuenta
			//button_grabar.Clicked += new EventHandler(on_button_grabar_clicked);
			////// Voy a buscar el folio que capturo
			button_selec_folio.Clicked += new EventHandler(on_selec_folio_clicked);
			////// Voy a buscar el paciente
			button_busca_paciente.Clicked += new EventHandler(on_button_busca_paciente_clicked);
			////// Voy a buscar el producto
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			////////Se desactivan los entry que no se deben de editar 
			entry_nombre_paciente.Sensitive = false;
			entry_pid_paciente.Sensitive = false;
			entry_fecha_ingreso.Sensitive = false;
			entry_hora_ingreso.Sensitive = false;
			entry_fecha_naci_paciente.Sensitive = false;
			entry_edad_paciente.Sensitive = false;
			entry_cirugia.Sensitive = false;
			entry_medico.Sensitive = false;
			entry_especialidad_medico.Sensitive = false;
			entry_diagnostico.Sensitive = false;
		}
                
		void on_selec_folio_clicked (object sender, EventArgs args)
		{
			llenado_de_productos_aplicados();
			//llena_datos_paciente();
		}
		///////////////////SE LLENA LA TABLA DE PRODUCTOS APLICADOS/////////////////
		void llenado_de_productos_aplicados() 
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
        	treeViewEngineServicio.Clear();
			////// Verifica que la base de datos este conectada
        	try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				///// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText ="SELECT "+
					"hscmty_erp_cobros_enca.folio_de_servicio,to_char(hscmty_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente, "+
					"hscmty_his_paciente.nombre1_paciente,hscmty_his_paciente.nombre2_paciente,hscmty_his_paciente.apellido_paterno_paciente,hscmty_his_paciente.apellido_materno_paciente, "+
					"to_char(fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fecha_nac_pa,hscmty_his_paciente.sexo_paciente, "+
					"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
					"hscmty_his_medicos.nombre_medico,hscmty_his_medicos.id_especialidad,hscmty_his_tipo_especialidad.descripcion_especialidad, "+
					"hscmty_erp_movcargos.folio_de_servicio,to_char(fechahora_admision_registro,'dd-MM-yyyy') AS fecha_reg_adm, "+
					"to_char(fechahora_admision_registro,'HH:mm:ss') AS hora_reg_adm,hscmty_his_tipo_admisiones.descripcion_admisiones, "+
					"hscmty_his_tipo_cirugias.descripcion_cirugia,hscmty_his_tipo_diagnosticos.id_diagnostico, "+
					"hscmty_his_tipo_diagnosticos.descripcion_diagnostico,descripcion_diagnostico_movcargos "+
					"FROM hscmty_erp_cobros_enca,hscmty_his_medicos,hscmty_erp_movcargos,hscmty_his_paciente,hscmty_his_tipo_pacientes,hscmty_his_tipo_cirugias,hscmty_his_tipo_diagnosticos,hscmty_his_tipo_admisiones,hscmty_his_tipo_especialidad "+
					"WHERE hscmty_erp_cobros_enca.id_medico = hscmty_his_medicos.id_medico "+
					"AND hscmty_erp_cobros_enca.folio_de_servicio = hscmty_erp_movcargos.folio_de_servicio "+
					"AND hscmty_erp_cobros_enca.pid_paciente = hscmty_erp_movcargos.pid_paciente "+
					"AND hscmty_erp_movcargos.pid_paciente = hscmty_his_paciente.pid_paciente "+
					"AND hscmty_his_tipo_cirugias.id_tipo_cirugia = hscmty_erp_movcargos.id_tipo_cirugia "+
					"AND hscmty_his_tipo_diagnosticos.id_diagnostico = hscmty_erp_movcargos.id_diagnostico "+
					"AND hscmty_his_medicos.id_especialidad = hscmty_his_tipo_especialidad.id_especialidad "+
					"AND hscmty_his_tipo_pacientes.id_tipo_paciente = hscmty_erp_movcargos.id_tipo_paciente "+
					"AND hscmty_erp_movcargos.id_tipo_admisiones = hscmty_his_tipo_admisiones.id_tipo_admisiones "+
					"AND hscmty_erp_movcargos.folio_de_servicio = hscmty_erp_cobros_enca.folio_de_servicio "+
					"AND pagado = false "+
					//"AND hscmty_his_tipo_admisiones.servicio_directo = false "+
					"AND hscmty_erp_cobros_enca.alta_paciente = false "+
					"AND hscmty_erp_movcargos.id_tipo_admisiones = hscmty_his_tipo_admisiones.id_tipo_admisiones "+
					"AND hscmty_erp_movcargos.id_tipo_paciente = hscmty_his_tipo_pacientes.id_tipo_paciente "+
					"AND hscmty_erp_cobros_enca.folio_de_servicio = '"+(string) entry_folio_servicio.Text+"';";
                       	
				NpgsqlDataReader lector = comando.ExecuteReader ();
            	
				while (lector.Read())
				{
					folioservicio = (int) lector["folio_de_servicio"];
					entry_pid_paciente.Text = (string) lector["pidpaciente"];
					entry_nombre_paciente.Text = (string) lector["nombre1_paciente"]+" "+(string) lector["nombre2_paciente"]+" "+lector["apellido_paterno_paciente"]+" "+lector["apellido_materno_paciente"];
					entry_fecha_ingreso.Text = (string) lector ["fecha_reg_adm"];
					entry_hora_ingreso.Text = (string) lector ["hora_reg_adm"];
					entry_fecha_naci_paciente.Text = (string) lector ["fecha_nac_pa"];
					entry_edad_paciente.Text = (string) lector ["edad"];
					entry_cirugia.Text = (string) lector ["descripcion_cirugia"];
					entry_medico.Text = (string) lector ["nombre_medico"];
					entry_especialidad_medico.Text = (string) lector ["descripcion_especialidad"];
					entry_diagnostico.Text = (string) lector["descripcion_diagnostico_movcargos"];
					tipointernamiento = (string) lector ["descripcion_admisiones"];
					/////////llena la tabla con valores 
					TreeIter iter = treeViewEngineServicio.AppendValues (tipointernamiento);
							treeViewEngineServicio.AppendValues (iter, "No tiene Productos aplicados", "1000");
							treeViewEngineServicio.AppendValues (iter, "No tiene Productos aplicados", "2000");            		
				}
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			}
			conexion.Close ();
		}
		
		///////////////SE CREA LA LISTA DE LOS PRODUCTOS APLICADOS  //////
		void crea_treeview_productos()
		{
			treeViewEngineServicio = new TreeStore(typeof(string), typeof(string),typeof(string),typeof(string) );
			lista_de_productos_apli.Model = treeViewEngineServicio;
			lista_de_productos_apli.RulesHint = true;
				
			TreeViewColumn col_servicio = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_servicio.Title = "Servicio/Productos aplicados"; // titulo de la cabecera de la columna, si está visible
			col_servicio.PackStart(cellr0, true);
			col_servicio.AddAttribute (cellr0, "text", 0);
			//cellr0.Editable = true;   // Permite edita este campo
        
			TreeViewColumn col_cantidad = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_cantidad.Title = "Cantidad"; // titulo de la cabecera de la columna, si está visible
			col_cantidad.PackStart(cellr1, true);
			col_cantidad.AddAttribute (cellr1, "text", 1);
        		
			TreeViewColumn col_quien_cargo = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_quien_cargo.Title = "Quien cargo"; // titulo de la cabecera de la columna, si está visible
			col_quien_cargo.PackStart(cellr2, true);
			col_quien_cargo.AddAttribute (cellr2, "text", 2);
        
			TreeViewColumn col_fecha_hora = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_fecha_hora.Title = "Fecha/Hora"; // titulo de la cabecera de la columna, si está visible
			col_fecha_hora.PackStart(cellr3, true);
			col_fecha_hora.AddAttribute (cellr3, "text", 3);
        
			lista_de_productos_apli.AppendColumn(col_servicio);
			lista_de_productos_apli.AppendColumn(col_cantidad);
			lista_de_productos_apli.AppendColumn(col_quien_cargo);
			lista_de_productos_apli.AppendColumn(col_fecha_hora);
			//lista_de_productos_apli.RowExpanded += on_expandrows_RowExpanded; 
		}
		///////////////////////////// Se termina de crear los treview que muestran los productos aplicados
        ///////////////////////////// SE CREA LA LISTA DE LOS MEDICAMENTOS EXTRAS /////////////////////////
		void crea_treeview_cargextra()
		{
			treeViewEngineExtras = new ListStore(typeof(bool),typeof(int),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string) );
												
			lista_de_medicamentos.Model = treeViewEngineExtras;
			lista_de_medicamentos.RulesHint = true;
				
			TreeViewColumn col_seleccion = new TreeViewColumn();
			CellRendererToggle cellr0 = new CellRendererToggle();
			col_seleccion.Title = "Seleccion"; // titulo de la cabecera de la columna, si está visible
			col_seleccion.PackStart(cellr0, true);
			//col_seleccion.SetCellDataFunc(cellr0, new TreeCellDataFunc (BoolCellDataFunc));  // funcion de columna
			col_seleccion.AddAttribute (cellr0, "active", 0);
			cellr0.Activatable = true;
			//cellr0.Toggled += selecciona_fila;
		
			TreeViewColumn col_cantidad = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_cantidad.Title = "Cantidad"; // titulo de la cabecera de la columna, si está visible
			col_cantidad.PackStart(cellr1, true);
			col_cantidad.AddAttribute (cellr1, "text", 1);
			cellr1.Editable = true;   // Permite edita este campo
        
			TreeViewColumn col_codigo_prod = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_codigo_prod.Title = "Descripcion"; // titulo de la cabecera de la columna, si está visible
			col_codigo_prod.PackStart(cellr2, true);
			col_codigo_prod.AddAttribute (cellr2, "text", 2);

			TreeViewColumn col_descripcion = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_descripcion.Title = "Descripcion"; // titulo de la cabecera de la columna, si está visible
			col_descripcion.PackStart(cellr3, true);
			col_descripcion.AddAttribute (cellr3, "text", 3);
        		
			TreeViewColumn col_quien_cargo = new TreeViewColumn();
			CellRendererText cellr4 = new CellRendererText();
			col_quien_cargo.Title = "Quien cargo"; // titulo de la cabecera de la columna, si está visible
			col_quien_cargo.PackStart(cellr4, true);
			col_quien_cargo.AddAttribute (cellr4, "text", 4);
        
			TreeViewColumn col_fecha_hora = new TreeViewColumn();
			CellRendererText cellr5 = new CellRendererText();
			col_fecha_hora.Title = "Fecha/Hora"; // titulo de la cabecera de la columna, si está visible
			col_fecha_hora.PackStart(cellr5, true);
			col_fecha_hora.AddAttribute (cellr5, "text", 5);

			TreeViewColumn col_asignado = new TreeViewColumn();
			CellRendererText cellr6 = new CellRendererText();
			col_asignado.Title = "Asignado a"; // titulo de la cabecera de la columna, si está visible
			col_asignado.PackStart(cellr6, true);
			col_asignado.AddAttribute (cellr6, "text", 6);
        
			lista_de_medicamentos.AppendColumn(col_seleccion);
			lista_de_medicamentos.AppendColumn(col_cantidad);
			lista_de_medicamentos.AppendColumn(col_codigo_prod);
			lista_de_medicamentos.AppendColumn(col_descripcion);
			lista_de_medicamentos.AppendColumn(col_quien_cargo);
			lista_de_medicamentos.AppendColumn(col_fecha_hora);
			lista_de_medicamentos.AppendColumn(col_asignado);
		}
		////////////////// Se termina de crear tabla de medicamentos extras ///////////////
		///////////////// FUNCIONES DE LA VENTANA DE BUSQUEDA DE PACIENTE /////////////////
		void on_button_busca_paciente_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "busca_paciente", null);
			gxml.Autoconnect (this);
			////// Muestra ventana de Glade
			busca_paciente.Show();
			button_nuevo_paciente.Sensitive = false;
			crea_treeview_busqueda();
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
			button_buscar_busqueda.Clicked += new EventHandler(on_buscar_busqueda_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_paciente_clicked);
		}
		
		void on_buscar_busqueda_clicked (object sender, EventArgs a)
		{
			treeViewEngineBusca.Clear(); //// Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			///// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				if (radiobutton_busca_apellido.Active == true)
				{
					comando.CommandText = "SELECT hscmty_erp_cobros_enca.folio_de_servicio,to_char(hscmty_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente, "+
							" nombre1_paciente,nombre2_paciente, apellido_paterno_paciente, "+
							"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,descripcion_diagnostico_movcargos, "+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
							"sexo_paciente,to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mm:ss') AS fech_creacion, descripcion_admisiones,descripcion_diagnostico_movcargos "+
							"FROM hscmty_his_paciente,hscmty_erp_cobros_enca,hscmty_his_tipo_admisiones,hscmty_erp_movcargos "+
							"WHERE pagado = false "+
							"AND hscmty_erp_movcargos.id_tipo_admisiones = hscmty_his_tipo_admisiones.id_tipo_admisiones "+
							"AND hscmty_erp_movcargos.id_tipo_admisiones = "+idtipointernamiento.ToString()+" "+
							"AND hscmty_his_paciente.pid_paciente = hscmty_erp_movcargos.pid_paciente "+
							"AND hscmty_erp_cobros_enca.folio_de_servicio = hscmty_erp_movcargos.folio_de_servicio "+
							"AND hscmty_his_paciente.pid_paciente = hscmty_erp_cobros_enca.pid_paciente "+
							"AND hscmty_erp_cobros_enca.alta_paciente = false "+
							"AND apellido_paterno_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' ORDER BY folio_de_servicio;";
				}
				if (radiobutton_busca_nombre.Active == true)
				{
					comando.CommandText = "SELECT hscmty_erp_cobros_enca.folio_de_servicio,to_char(hscmty_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente, "+
							" nombre1_paciente,nombre2_paciente, apellido_paterno_paciente, "+
							"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,descripcion_diagnostico_movcargos, "+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
							"sexo_paciente,to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mm:ss') AS fech_creacion, descripcion_admisiones,descripcion_diagnostico_movcargos "+
							"FROM hscmty_his_paciente,hscmty_erp_cobros_enca,hscmty_his_tipo_admisiones,hscmty_erp_movcargos "+
							"WHERE pagado = false "+
							"AND hscmty_erp_movcargos.id_tipo_admisiones = hscmty_his_tipo_admisiones.id_tipo_admisiones "+
							"AND hscmty_erp_movcargos.id_tipo_admisiones = "+idtipointernamiento.ToString()+" "+
							"AND hscmty_his_paciente.pid_paciente = hscmty_erp_movcargos.pid_paciente "+
							"AND hscmty_erp_cobros_enca.folio_de_servicio = hscmty_erp_movcargos.folio_de_servicio "+
							"AND hscmty_his_paciente.pid_paciente = hscmty_erp_cobros_enca.pid_paciente "+
							"AND hscmty_erp_cobros_enca.alta_paciente = false "+
							"AND nombre1_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' ORDER BY folio_de_servicio;";
				}
				if (radiobutton_busca_expediente.Active == true)
				{
					comando.CommandText = "SELECT hscmty_erp_cobros_enca.folio_de_servicio,to_char(hscmty_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente, "+
							"nombre1_paciente,nombre2_paciente, apellido_paterno_paciente, "+
							"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,descripcion_diagnostico_movcargos, "+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
							"sexo_paciente,to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mm:ss') AS fech_creacion, descripcion_admisiones,descripcion_diagnostico_movcargos "+
							"FROM hscmty_his_paciente,hscmty_erp_cobros_enca,hscmty_his_tipo_admisiones,hscmty_erp_movcargos "+
							"WHERE pagado = false "+
							"AND hscmty_erp_movcargos.id_tipo_admisiones = hscmty_his_tipo_admisiones.id_tipo_admisiones "+
							"AND hscmty_erp_movcargos.id_tipo_admisiones = "+idtipointernamiento.ToString()+" "+
							"AND hscmty_his_paciente.pid_paciente = hscmty_erp_movcargos.pid_paciente "+
							"AND hscmty_erp_cobros_enca.folio_de_servicio = hscmty_erp_movcargos.folio_de_servicio "+
							"AND hscmty_his_paciente.pid_paciente = hscmty_erp_cobros_enca.pid_paciente "+
							"AND hscmty_erp_cobros_enca.alta_paciente = false "+
							"AND hscmty_his_paciente.pid_paciente = '"+entry_expresion.Text+"' ORDER BY folio_de_servicio;";					
				}
				if ((string) entry_expresion.Text.ToString() == "*")
				{
					comando.CommandText = "SELECT hscmty_erp_cobros_enca.folio_de_servicio,to_char(hscmty_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente, "+
							" nombre1_paciente,nombre2_paciente, apellido_paterno_paciente, "+
							"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,descripcion_diagnostico_movcargos, "+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
							"sexo_paciente,to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mm:ss') AS fech_creacion, descripcion_admisiones,descripcion_diagnostico_movcargos "+
							"FROM hscmty_his_paciente,hscmty_erp_cobros_enca,hscmty_his_tipo_admisiones,hscmty_erp_movcargos "+
							"WHERE pagado = false "+
							"AND hscmty_erp_movcargos.id_tipo_admisiones = hscmty_his_tipo_admisiones.id_tipo_admisiones "+
							"AND hscmty_erp_movcargos.id_tipo_admisiones = "+idtipointernamiento.ToString()+" "+
							"AND hscmty_his_paciente.pid_paciente = hscmty_erp_movcargos.pid_paciente "+
							"AND hscmty_erp_cobros_enca.folio_de_servicio = hscmty_erp_movcargos.folio_de_servicio "+
							"AND hscmty_his_paciente.pid_paciente = hscmty_erp_cobros_enca.pid_paciente "+
							"AND hscmty_erp_cobros_enca.alta_paciente = false "+
							"ORDER BY folio_de_servicio;";
				}
					NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read())
				{
					TreeIter iter = treeViewEngineBusca.AppendValues ((int) lector["folio_de_servicio"],(string) lector["pidpaciente"],
										(string) lector["nombre1_paciente"],(string) lector["nombre2_paciente"],
										(string) lector["apellido_paterno_paciente"], (string) lector["apellido_materno_paciente"],
										(string) lector["fech_nacimiento"], (string) lector["edad"],
										(string) lector["sexo_paciente"], (string) lector["fech_creacion"],
										(string) lector["descripcion_diagnostico_movcargos"]);
				}
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			}
            
           		 conexion.Close ();
		}
		
		void crea_treeview_busqueda()
		{
			treeViewEngineBusca = new TreeStore(typeof(int),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
											typeof(string),typeof(string),typeof(string),typeof(string));
			lista_de_Pacientes.Model = treeViewEngineBusca;
			lista_de_Pacientes.RulesHint = true;
			lista_de_Pacientes.RowActivated += on_selecciona_paciente_clicked;  // Doble click selecciono paciente*/
			
			TreeViewColumn col_FolioServicio = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_FolioServicio.Title = "Folio Servicio"; // titulo de la cabecera de la columna, si está visible
			col_FolioServicio.PackStart(cellr0, true);
			col_FolioServicio.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 0
			col_FolioServicio.SortColumnId = (int) Column.col_FolioServicio;
			//cellr0.Editable = true;   // Permite edita este campo
			
			TreeViewColumn col_PidPaciente = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_PidPaciente.Title = "PID Paciente"; // titulo de la cabecera de la columna, si está visible
			col_PidPaciente.PackStart(cellr1, true);
			col_PidPaciente.AddAttribute (cellr1, "text", 1);    // columna 1
			col_PidPaciente.SortColumnId = (int) Column.col_PidPaciente;
			//cellr0.Editable = true;   // Permite edita este campo
            
			TreeViewColumn col_Nombre1_Paciente = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_Nombre1_Paciente.Title = "Nombre 1";
			col_Nombre1_Paciente.PackStart(cellrt2, true);
			col_Nombre1_Paciente.AddAttribute (cellrt2, "text", 2); // columna  2
			col_Nombre1_Paciente.SortColumnId = (int) Column.col_Nombre1_Paciente;
            
			TreeViewColumn col_Nombre2_Paciente = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_Nombre2_Paciente.Title = "Nombre 2";
			col_Nombre2_Paciente.PackStart(cellrt3, true);
			col_Nombre2_Paciente.AddAttribute (cellrt3, "text", 3); // columna  3
			col_Nombre2_Paciente.SortColumnId = (int) Column.col_Nombre2_Paciente;
            
			TreeViewColumn col_app_Paciente = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_app_Paciente.Title = "Apellido Paterno";
			col_app_Paciente.PackStart(cellrt4, true);
			col_app_Paciente.AddAttribute (cellrt4, "text", 4); // columna  4
			col_app_Paciente.SortColumnId = (int) Column.col_app_Paciente;
            
			TreeViewColumn col_apm_Paciente = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_apm_Paciente.Title = "Apellido Materno";
			col_apm_Paciente.PackStart(cellrt5, true);
			col_apm_Paciente.AddAttribute (cellrt5, "text", 5); // columna 5
			col_apm_Paciente.SortColumnId = (int) Column.col_apm_Paciente;
      
			TreeViewColumn col_fechanacimiento_Paciente = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_fechanacimiento_Paciente.Title = "Fecha Nacimiento";
			col_fechanacimiento_Paciente.PackStart(cellrt6, true);
			col_fechanacimiento_Paciente.AddAttribute (cellrt6, "text", 6);     //columna  6 
			col_fechanacimiento_Paciente.SortColumnId = (int) Column.col_fechanacimiento_Paciente;
            
			TreeViewColumn col_edad_Paciente = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_edad_Paciente.Title = "Edad";
			col_edad_Paciente.PackStart(cellrt7, true);
			col_edad_Paciente.AddAttribute (cellrt7, "text", 7); // columna  7 
			col_edad_Paciente.SortColumnId = (int) Column.col_edad_Paciente;
            
			TreeViewColumn col_sexo_Paciente = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_sexo_Paciente.Title = "Sexo";
			col_sexo_Paciente.PackStart(cellrt8, true);
			col_sexo_Paciente.AddAttribute (cellrt8, "text", 8); // columna  8 
			col_sexo_Paciente.SortColumnId = (int) Column.col_sexo_Paciente;
                        
			TreeViewColumn col_creacion_Paciente = new TreeViewColumn();
			CellRendererText cellrt9 = new CellRendererText();
			col_creacion_Paciente.Title = "Fecha creacion";
			col_creacion_Paciente.PackStart(cellrt9, true);
			col_creacion_Paciente.AddAttribute (cellrt9, "text", 9); //  columna  9
			col_creacion_Paciente.SortColumnId = (int) Column.col_creacion_Paciente;
           
			lista_de_Pacientes.AppendColumn(col_FolioServicio);                              		
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
	enum Column
		{
			col_FolioServicio,
			col_PidPaciente,
			col_Nombre1_Paciente,
			col_Nombre2_Paciente,
			col_app_Paciente,
			col_apm_Paciente,
			col_fechanacimiento_Paciente,
			col_edad_Paciente,
			col_sexo_Paciente,
			col_creacion_Paciente,
		}
	void on_selecciona_paciente_clicked(object sender, EventArgs a)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_Pacientes.Selection.GetSelected(out model, out iterSelected)) 
			{
				folioservicio = (int) model.GetValue(iterSelected, 0);
				entry_pid_paciente.Text = (string) model.GetValue(iterSelected, 1);
				entry_nombre_paciente.Text = (string) model.GetValue(iterSelected, 2)+" "+
								(string) model.GetValue(iterSelected, 3)+" "+
								(string) model.GetValue(iterSelected, 4)+" "+
				(string) model.GetValue(iterSelected, 5);
				entry_fecha_naci_paciente.Text = (string) model.GetValue(iterSelected, 6);
				entry_edad_paciente.Text = (string) model.GetValue(iterSelected, 7);
				entry_folio_servicio.Text = folioservicio.ToString(); 
				// Muestra ventana de Glade
				cargos_hospital.Show();
				// activa boton de grabacion de informacion
				//button_grabar.Clicked += new EventHandler(on_graba_informacion_clicked);
				//////crea lista de productos
				llenado_de_productos_aplicados();
				//llena_datos_paciente();
				// destruye la ventana de busqueda
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
		
////////////////////// Terminan funciones ventana de busqueda de paciente///////////////////////

///////////////////// FUNCIONES DE LA VENTANA DE BUSQUEDA DE PRODUCTO ///////////////////////////
	void on_button_busca_producto_clicked (object sender, EventArgs a)
		{
			Glade.XML gxml = new Glade.XML (null, "enfermeria.glade", "busca_producto", null);
			gxml.Autoconnect (this);	    	
			// Muestra ventana de Glade
			busca_producto.Show();
			crea_treeview_busqueda_producto();
			button_buscar_producto.Clicked += new EventHandler(on_buscar_producto_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); 
		}
		
	void on_buscar_producto_clicked (object sender, EventArgs args)
		{
			treeViewEngineBusca2.Clear(); //// Limpia el treeview cuando realiza una nueva busqueda
			entry_expresion.Text = entry_expresion.Text.ToUpper();
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			///// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	        
				comando.CommandText = "SELECT to_char(hscmty_productos.id_producto,'999999999999') AS codProducto,hscmty_productos.descripcion_producto, "+					"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto "+
						"FROM hscmty_productos,hscmty_grupo_producto,hscmty_grupo1_producto,hscmty_grupo2_producto "+
						"WHERE hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
						"AND hscmty_productos.id_grupo1_producto = hscmty_grupo1_producto.id_grupo1_producto "+
						"AND hscmty_productos.id_grupo2_producto = hscmty_grupo2_producto.id_grupo2_producto "+
						"AND hscmty_productos.cobro_activo = true "+
						"AND hscmty_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper()+"%' ORDER BY descripcion_producto; ";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				while (lector.Read())
				{
					TreeIter iter = treeViewEngineBusca2.AppendValues ((string) lector["codProducto"] ,(string) lector["descripcion_producto"],
											(string) lector["descripcion_grupo_producto"],(string) lector["descripcion_grupo1_producto"],
											(string) lector["descripcion_grupo2_producto"]);
				}
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			     }
				conexion.Close ();
		}
	
	void crea_treeview_busqueda_producto()
		{
			treeViewEngineBusca2 = new TreeStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(string) );
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
				col_grupoprod.Title = "Grupo Producto";
				col_grupoprod.PackStart(cellrt2, true);
				col_grupoprod.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 7 en vez de 8
				col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
            
				TreeViewColumn col_grupo1prod = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_grupo1prod.Title = "Grupo1 Producto";
				col_grupo1prod.PackStart(cellrt3, true);
				col_grupo1prod.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 8 en vez de 9
				col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
                        
				TreeViewColumn col_grupo2prod = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_grupo2prod.Title = "Grupo2 Producto";
				col_grupo2prod.PackStart(cellrt4, true);
				col_grupo2prod.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 8 en vez de 9
				col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;

				lista_de_producto.AppendColumn(col_idproducto);
				lista_de_producto.AppendColumn(col_desc_producto);
				lista_de_producto.AppendColumn(col_grupoprod);
				lista_de_producto.AppendColumn(col_grupo1prod);
				lista_de_producto.AppendColumn(col_grupo2prod);
		}
	enum Column_prod
		{
			col_idproducto,
			col_desc_producto,
			col_grupoprod,
			col_grupo1prod,
			col_grupo2prod,
		}
        
	void on_selecciona_producto_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
					
			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)) 
			{
				id_produ = (string) model.GetValue(iterSelected, 0);
				desc_produ = (string) model.GetValue(iterSelected, 1);
				grupoprodu = (string) model.GetValue(iterSelected, 2);
				grupoprodu1 = (string) model.GetValue(iterSelected, 3); 
				grupoprodu2 = (string) model.GetValue(iterSelected, 4);	
				fechahoraprod = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				
				TreeIter iter = treeViewEngineExtras.AppendValues (null,0,id_produ,desc_produ,LoginEmpleado,fechahoraprod);
				//eeViewEngineExtras.Foreach (
				
				
				cargos_hospital.Show();
				// activa boton de grabacion de informacion
				//button_grabar.Clicked += new EventHandler(on_graba_informacion_clicked);
				//entry_producto.Text = (string) model.GetValue(iterSelected, 1);
				
				Console.WriteLine("se activo");
				// destruye la ventana de busqueda
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
		
		
//////////// TERMINA FUCIONES DE LA VENTANA DE BUSQUEDA DE PRODUCTOS //////////////////////		
		
	
		// Cuando seleccion el treeview vuelve a recargar la lista para verificar
		// si no se han hechos nuevos cargos
	void on_expandrows_RowExpanded (object sender, EventArgs args)
		{
		
		}
		
		// cierra ventanas emergentes
	void on_cierraventanas_clicked (object sender, EventArgs a)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
	}        
}
