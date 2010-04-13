// created on 08/08/2008 at 11:03 a
// created on 08/06/2007 at 05:03 p
///////////////////////////////////////////////////////////
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Juan Antonio Peña Gonzalez (Programacion)
//				  Ing. Daniel Olivares (Preprogramacion)
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
// Programa		: cargos_hospitalizacion.cs
// Proposito	: Pagos en Caja 
// Objeto		: cargos_hospitalizacion.cs
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using System.Collections;

namespace osiris
{
	public class cargos_imagenologia_b
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		[Widget] Gtk.Button button_imprimir;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
			
		// Declarando ventana principal de pago
		[Widget] Gtk.Window cargos_de_laboratorio;
		[Widget] Gtk.Entry entry_folio_servicio;
		[Widget] Gtk.Entry entry_fecha_admision;
		[Widget] Gtk.Entry entry_hora_registro;
		[Widget] Gtk.Entry entry_fechahora_alta;
		[Widget] Gtk.Entry entry_pid_paciente;
		[Widget] Gtk.Entry entry_nombre_paciente;
		[Widget] Gtk.Entry entry_telefono_paciente;
		[Widget] Gtk.Entry entry_cirugia;
		[Widget] Gtk.Entry entry_doctor;
		[Widget] Gtk.Entry entry_especialidad;
		[Widget] Gtk.Entry entry_tipo_paciente;
		[Widget] Gtk.Entry entry_aseguradora;
		[Widget] Gtk.Entry entry_poliza;
		[Widget] Gtk.Entry entry_fecha_nacimiento;
		[Widget] Gtk.Entry entry_edad;
		[Widget] Gtk.Entry entry_id_habitacion;
		[Widget] Gtk.Entry entry_descrip_cirugia;
		
		[Widget] Gtk.TreeView lista_de_servicios;
		[Widget] Gtk.TreeView lista_cargos_extras;
		//[Widget] Gtk.ProgressBar progressbar_status_llenado;
		[Widget] Gtk.Button button_quitar_aplicados;
		[Widget] Gtk.Button button_actualizar;
		[Widget] Gtk.Button button_buscar_paciente;
		[Widget] Gtk.Button button_selec_folio;
		[Widget] Gtk.Button button_graba_pago;
		[Widget] Gtk.Entry entry_desc_producto;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.Button button_removerItem;
		[Widget] Gtk.Button button_aplica_cargos;
		[Widget] Gtk.Button button_resultados;		
		
		//Declarando la barra de estado
		[Widget] Gtk.Statusbar statusbar_caja;
		
		/////// Ventana Busqueda de paciente\\\\\\\\
		[Widget] Gtk.TreeView lista_de_Pacientes;
		[Widget] Gtk.Button button_nuevo_paciente;
		[Widget] Gtk.RadioButton radiobutton_busca_apellido;
		[Widget] Gtk.RadioButton radiobutton_busca_nombre;
		[Widget] Gtk.RadioButton radiobutton_busca_expediente;
		
		/////// Ventana Busqueda de productos\\\\\\\\
		[Widget] Gtk.Window busca_producto;
		[Widget] Gtk.TreeView lista_de_producto;
		//[Widget] Gtk.Button button_agrega_extra;
		[Widget] Gtk.Entry entry_cantidad_aplicada;
		[Widget] Gtk.ComboBox combobox_tipo_admision;
		[Widget] Gtk.Entry entry_fecha_solicitud;
		[Widget] Gtk.Entry entry_hora_solicitud;
		[Widget] Gtk.Entry entry_folio_laboratorio;
		[Widget] Gtk.Label label_departamento;
										
		private TreeStore treeViewEngineBusca;
		private TreeStore treeViewEngineBusca2;
		private TreeStore treeViewEngineServicio;
		private ListStore treeViewEngineExtras;
		//public TreeStore treeViewEngineresultados;
		public ListStore treeViewEngineresultados;
		
		//private ArrayList arraycargosrealizados;
		private ArrayList arraycargosextras;
		
		// Declaracion de variables publicas
		int folioservicio = 0;							// Toma el valor de numero de atencion de paciente
		int PidPaciente = 0;								// Toma la actualizacion del pid del paciente
		int id_tipopaciente;								// Toma el valor del tipo de paciente
		int idtipointernamiento = 300;					// Toma el valor del tipo de internamiento
		int idempresa_paciente = 0;						// Toma el valor de la empresa que el hospital tiene convenio
		string descripinternamiento = "IMAGENOLOGIA";  	// Toma la descripcion del internamiento
		int idlugarprocedencia = 0;						// Toma el valor del lugar de donde se pidio el estudio
		string descriplugarprocedencia = "  ";			// Toma la descripcion del lugar de donde se pidio el estudio
		int idhabitacion = 0;						// Toma el id de la habitacion asignada
		
		string edadpac;
		string fecha_nacimiento;
		string dir_pac;
		string cirugia;
		string empresapac;
		string sexopaciente;
		
		bool cuenta_bloqueada;
		bool cuenta_cerrada;
		
		float valoriva;
		bool aplicar_descuento = true;
		bool aplicar_siempre = false;
		
		string id_produ = "";
		string desc_produ = "";
		string precio_produ ="";
		string iva_produ ="";
		string total_produ ="";
		string descuent_produ ="";
		string pre_con_desc_produ ="";
		float valor_descuento = 0;
		string costo_unitario_producto;
		string porcentage_utilidad_producto;
		string costo_total_producto;
		float ppcantidad = 0;
		string ppcant ="";
		
			//********    //nuevo lista de precios multiples//   *****************/
		int idaseguradora_paciente = 0;			// Toma el valor de la aseguradora que ingreso el paciente
		bool aplica_precios_aseguradoras = false;// Toma el valor de si se tiene creado la lista de precio en la tabla de Productos
		bool aplica_precios_empresas = false;	// Toma el valor de si se tiene creado la lista de precio en la tabla de Productos
		//**************/
		// Sumas Totales para los calculos
		float subtotal_al_15;
		float subtotal_al_0;
		float total_iva;
		float sub_total;
		float totaldescuento;
		
		bool aplico_cargos = false;
		
		string LoginEmpleado;
		string NomEmpleados;
			
		string connectionString;
		string nombrebd;
				
		CellRendererText cel_descripcion;
		
		//Declarando las celdas
		CellRendererText cellr0;				CellRendererText cellr1;
		CellRendererText cellr2;		
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public cargos_imagenologia_b (string LoginEmp, string NomEmpleado, string AppEmpleado, string ApmEmpleado, string nombrebd_ ) 
		{
			LoginEmpleado = LoginEmp;
			NomEmpleados = NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;			
			valoriva = float.Parse(classpublic.ivaparaaplicar);
			
			Glade.XML gxml = new Glade.XML (null, "imagenologia.glade", "cargos_de_laboratorio", null);
			gxml.Autoconnect (this);
	        // Muestra ventana de Glade
			cargos_de_laboratorio.Show();
			
			// Creacion de los treeview para la pantalla	
			crea_treeview_servicio();
			crea_treeview_cargextra();
			entry_fecha_admision.Sensitive = false;
			entry_hora_registro.Sensitive = false;
			entry_fechahora_alta.Sensitive = false;
			entry_nombre_paciente.Sensitive = false;
			entry_pid_paciente.Sensitive = false;
			entry_telefono_paciente.Sensitive = false;
			entry_cirugia.Sensitive = false;
			entry_doctor.Sensitive = false;
			entry_especialidad.Sensitive = false;
			entry_tipo_paciente.Sensitive = false;
			entry_aseguradora.Sensitive = false;
			entry_poliza.Sensitive = false;
			entry_desc_producto.Sensitive = false;
			entry_edad.Sensitive = false;
			entry_fecha_nacimiento.Sensitive = false;
			entry_id_habitacion.Sensitive = false;
			entry_descrip_cirugia.Sensitive = false;
			
			lista_de_servicios.Sensitive =false;
			lista_cargos_extras.Sensitive = false;
    
	    	// Voy a buscar el folio que capturo
			button_selec_folio.Clicked += new EventHandler(on_selec_folio_clicked);
			// Validando que sen solo numeros
			entry_folio_servicio.KeyPressEvent += onKeyPressEvent_enter_folio;
			// Activacion de grabacion de informacion
	    	button_graba_pago.Clicked += new EventHandler(on_button_graba_pago_clicked);
	    	
			// Activacion de boton de busqueda
			button_buscar_paciente.Clicked += new EventHandler(on_button_buscar_paciente_clicked);
			//quita lementos aplicados
			button_quitar_aplicados.Clicked += new EventHandler(on_button_quitar_aplicados_clicked);
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			// Busqueda de Productos
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			// Aplica productos al cargo
			button_aplica_cargos.Clicked += new EventHandler(on_button_aplica_cargos_clicked);
			// Boton que quita cargos extras
			button_removerItem.Clicked += new EventHandler(on_button_removerItem_clicked);
			// Actualiza lista de cobros aplicados
			button_actualizar.Clicked += new EventHandler(on_button_actualizar_clicked);
			// Boton que manda llamar al treeview de resultados de laboratorio
			button_resultados.Clicked += new EventHandler(on_button_resultados_clicked);
									
			// Desactivando Botones de operacion se activa cuando selecciona una atencion
			button_busca_producto.Sensitive = false;
			button_removerItem.Sensitive = false;
			button_graba_pago.Sensitive = false;
			button_aplica_cargos.Sensitive = false;
			button_quitar_aplicados.Sensitive = false;
			button_actualizar.Sensitive = false;
			
			statusbar_caja.Pop(0);
			statusbar_caja.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_caja.HasResizeGrip = false;
	    }
		
		void onComboBoxChanged_tipo_admision (object sender, EventArgs args)
		{
	    	ComboBox combobox_tipo_admision = sender as ComboBox;
			if (sender == null) { 		return; }
	  		TreeIter iter;
	  		if (combobox_tipo_admision.GetActiveIter (out iter))
		    	{
		    		idlugarprocedencia = (int) combobox_tipo_admision.Model.GetValue(iter,1);
		    		descriplugarprocedencia = (string) combobox_tipo_admision.Model.GetValue(iter,0);
		    		//Console.WriteLine(idlugarprocedencia+" "+descriplugarprocedencia);
	     		}
		}
		
		// Crea el treeview de los servicios y/o productos aplicados
		void crea_treeview_servicio()
		{
			//arraycargosrealizados = new ArrayList();
			treeViewEngineServicio = new TreeStore(typeof(string),//0 
													typeof(float),//1
													typeof(string),//2
													typeof(string),//3
													typeof(string),//4
													typeof(string),//5
													typeof(string),//6
													typeof(string), //7
													typeof(string),//8
													typeof(string),//9
													typeof(string),//10
													typeof(string),//11
													typeof(string),//12
													typeof(string),//13
													typeof(int),//14
													typeof(int),//15
													typeof(bool),//16
													typeof(string),//17
													typeof(string),//18
													typeof(string),//19
													typeof(string),//20
													typeof(string),//21fechahora_solicitud
													typeof(string));//22folio_interno_dep
												
			lista_de_servicios.Model = treeViewEngineServicio;
			lista_de_servicios.RulesHint = true;
				
			TreeViewColumn col_descripcion_hc = new TreeViewColumn();
			CellRendererText cel_descripcion = new CellRendererText();
			col_descripcion_hc.Title = "Servicio/Producto"; // titulo de la cabecera de la columna, si está visible
			col_descripcion_hc.PackStart(cel_descripcion, true);
			col_descripcion_hc.AddAttribute (cel_descripcion, "text", 0);
			col_descripcion_hc.SortColumnId = (int) Column_serv.col_descripcion_hc ;
			col_descripcion_hc.SetCellDataFunc(cel_descripcion, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			//cel_descripcion.Foreground = "darkblue";
			
			TreeViewColumn col_cantidad_hc = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_cantidad_hc.Title = "Cantidad"; // titulo de la cabecera de la columna, si está visible
			col_cantidad_hc.PackStart(cellr1, true);
			col_cantidad_hc.AddAttribute (cellr1, "text", 1);
			col_cantidad_hc.SortColumnId = (int) Column_serv.col_cantidad_hc;
			col_cantidad_hc.SetCellDataFunc(cellr1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						
			TreeViewColumn col_codigo_prod_hc = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_codigo_prod_hc.Title = "Codigo Prod."; // titulo de la cabecera de la columna, si está visible
			col_codigo_prod_hc.PackStart(cellr2, true);
			col_codigo_prod_hc.AddAttribute (cellr2, "text", 2);
			col_codigo_prod_hc.SortColumnId = (int) Column_serv.col_codigo_prod_hc;
			col_codigo_prod_hc.SetCellDataFunc(cellr2, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			        
			TreeViewColumn col_quien_cargo_hc = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_quien_cargo_hc.Title = "Quien cargo"; 
			col_quien_cargo_hc.PackStart(cellr3, true);
			col_quien_cargo_hc.AddAttribute (cellr3, "text", 3);
			col_quien_cargo_hc.SortColumnId = (int) Column_serv.col_quien_cargo_hc;
			col_quien_cargo_hc.SetCellDataFunc(cellr3, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_fecha_hora_hc = new TreeViewColumn();
			CellRendererText cellr4 = new CellRendererText();
			col_fecha_hora_hc.Title = "Fecha/Hora"; 
			col_fecha_hora_hc.PackStart(cellr4, true);
			col_fecha_hora_hc.AddAttribute (cellr4, "text", 4);
			col_fecha_hora_hc.SortColumnId = (int) Column_serv.col_fecha_hora_hc;
			col_fecha_hora_hc.SetCellDataFunc(cellr4, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
        
			TreeViewColumn col_asignado_hc = new TreeViewColumn();
			CellRendererText cellr5 = new CellRendererText();
			col_asignado_hc.Title = "Asignado a"; 
			col_asignado_hc.PackStart(cellr5, true);
			col_asignado_hc.AddAttribute (cellr5, "text", 5);
			col_asignado_hc.SortColumnId = (int) Column_serv.col_asignado_hc;
			col_asignado_hc.SetCellDataFunc(cellr5, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_lugarpro_hc = new TreeViewColumn();
			CellRendererText cellr6 = new CellRendererText();
			col_lugarpro_hc.Title = "Lugar de Pedido"; 
			col_lugarpro_hc.PackStart(cellr6, true);
			col_lugarpro_hc.AddAttribute (cellr6, "text", 6);
			col_lugarpro_hc.SortColumnId = (int) Column_serv.col_lugarpro_hc;
			col_lugarpro_hc.SetCellDataFunc(cellr6, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_foliolab_hc = new TreeViewColumn();
			CellRendererText cellr22 = new CellRendererText();
			col_foliolab_hc.Title = "Folio Interno"; 
			col_foliolab_hc.PackStart(cellr22, true);
			col_foliolab_hc.AddAttribute (cellr22, "text", 22);
			col_foliolab_hc.SortColumnId = (int) Column_serv.col_foliolab_hc;
			col_foliolab_hc.SetCellDataFunc(cellr22, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_fechahora_solicitud_hc = new TreeViewColumn();
			CellRendererText cellr21 = new CellRendererText();
			col_fechahora_solicitud_hc.Title = "Fecha de Solicitud"; 
			col_fechahora_solicitud_hc.PackStart(cellr21, true);
			col_fechahora_solicitud_hc.AddAttribute (cellr21, "text", 21);
			col_fechahora_solicitud_hc.SortColumnId = (int) Column_serv.col_fechahora_solicitud_hc;
			col_fechahora_solicitud_hc.SetCellDataFunc(cellr21, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			lista_de_servicios.AppendColumn(col_descripcion_hc);
			lista_de_servicios.AppendColumn(col_cantidad_hc);
			lista_de_servicios.AppendColumn(col_codigo_prod_hc);
			lista_de_servicios.AppendColumn(col_quien_cargo_hc);
			lista_de_servicios.AppendColumn(col_fecha_hora_hc);
			lista_de_servicios.AppendColumn(col_asignado_hc);
			lista_de_servicios.AppendColumn(col_lugarpro_hc);
			lista_de_servicios.AppendColumn(col_foliolab_hc);
			lista_de_servicios.AppendColumn(col_fechahora_solicitud_hc);
		}
		enum Column_serv
		{
			col_descripcion_hc,
			col_cantidad_hc,
			col_codigo_prod_hc,
			col_quien_cargo_hc,
			col_fecha_hora_hc,
			col_asignado_hc,
			col_lugarpro_hc,
			col_precio_hc,
			col_ppor_cantidad_hc,
			col_iva_hc,
			col_sub_total_hc,
			col_por_desc_hc,
			col_valor_desc_hc,
			col_total_hc,
			col_foliolab_hc,
			col_fechahora_solicitud_hc
		}
		
		// funcion para cambiar el color de una fila y columna
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((bool)lista_de_servicios.Model.GetValue (iter,16)==true){
				(cell as Gtk.CellRendererText).Foreground = "darkblue";
			}else {	(cell as Gtk.CellRendererText).Foreground = "red";	}
		}

		void crea_treeview_cargextra()
		{
			arraycargosextras = new ArrayList();

			treeViewEngineExtras = new ListStore(typeof(bool), //0
													typeof(float),//1
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
													typeof(string),//12
													typeof(string),//13
													typeof(string),////14
													typeof(int),////15
													typeof(int),//16
													typeof(string),//17
													typeof(string),//18
													typeof(string),//19
													typeof(string),//20fechahora_solicitud
													typeof(string));//21folio_interno_dep
												
			lista_cargos_extras.Model = treeViewEngineExtras;
			
			lista_cargos_extras.RulesHint = true;
				
			TreeViewColumn col_seleccion = new TreeViewColumn();
			CellRendererToggle cellr0 = new CellRendererToggle();
			col_seleccion.Title = "Seleccion"; // Indica si se grabara
			col_seleccion.PackStart(cellr0, true);
			//col_seleccion.SetCellDataFunc(cellr0, new TreeCellDataFunc (BoolCellDataFunc));  // funcion de columna
			col_seleccion.AddAttribute (cellr0, "active", 0);
			cellr0.Activatable = true;
			cellr0.Toggled += selecciona_fila; 
		
			TreeViewColumn col_cantidad = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_cantidad.Title = "Cantidad"; // Cantidad de estudios
			col_cantidad.PackStart(cellr1, true);
			col_cantidad.AddAttribute (cellr1, "text", 1);
			cellr1.Editable = true;   // Permite edita este campo
			cellr1.Edited += new EditedHandler (NumberCellEdited);
			cellr1.Foreground = "darkblue";
			
			TreeViewColumn col_codigo_prod = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_codigo_prod.Title = "Codigo Prod."; // codigo del estudio cargado
			col_codigo_prod.PackStart(cellr2, true);
			col_codigo_prod.AddAttribute (cellr2, "text", 2);

			TreeViewColumn col_descripcion = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_descripcion.Title = "Descripcion"; // Descripcion de estudio cargado
			col_descripcion.PackStart(cellr3, true);
			col_descripcion.AddAttribute (cellr3, "text", 3);
        
			TreeViewColumn col_quien_cargo = new TreeViewColumn();
			CellRendererText cellr4 = new CellRendererText();
			col_quien_cargo.Title = "Quien cargo"; //ID de usuario que realiza cargo
			col_quien_cargo.PackStart(cellr4, true);
			col_quien_cargo.AddAttribute (cellr4, "text", 4);
			
			TreeViewColumn col_fecha_hora = new TreeViewColumn();
			CellRendererText cellr5 = new CellRendererText();
			col_fecha_hora.Title = "Fecha/Hora"; //fecha y hora de carga de estudio
			col_fecha_hora.PackStart(cellr5, true);
			col_fecha_hora.AddAttribute (cellr5, "text", 5);
        
			TreeViewColumn col_asignado = new TreeViewColumn();
			CellRendererText cellr6 = new CellRendererText();
			col_asignado.Title = "Asignado a"; //Area a donde se enviara el estudio
			col_asignado.PackStart(cellr6, true);
			col_asignado.AddAttribute (cellr6, "text", 6);
			cellr6.CellBackground = "red";
        	
        	TreeViewColumn col_lugarpro = new TreeViewColumn();
			CellRendererText cellr7 = new CellRendererText();
			col_lugarpro.Title = "Lugar de Pedido"; //I.V.A. titulo de la cabecera de la columna, si está visible
			col_lugarpro.PackStart(cellr7, true);
			col_lugarpro.AddAttribute (cellr7, "text", 7);
			cellr7.CellBackground = "lightblue";
			
			TreeViewColumn col_foliolab = new TreeViewColumn();
			CellRendererText cellr21 = new CellRendererText();
			col_foliolab.Title = "Folio Interno"; 
			col_foliolab.PackStart(cellr21, true);
			col_foliolab.AddAttribute (cellr21, "text", 21);
			cellr21.CellBackground = "lightblue";
			
			TreeViewColumn col_fechahora_solicitud = new TreeViewColumn();
			CellRendererText cellr20 = new CellRendererText();
			col_fechahora_solicitud.Title = "Fecha de Solicitud";
			col_fechahora_solicitud.PackStart(cellr20, true);
			col_fechahora_solicitud.AddAttribute (cellr20, "text", 20);
			cellr20.CellBackground = "lightblue";
			
			//////////////////////////////////////////////
			lista_cargos_extras.AppendColumn(col_seleccion);
			lista_cargos_extras.AppendColumn(col_cantidad);
			lista_cargos_extras.AppendColumn(col_codigo_prod);
			lista_cargos_extras.AppendColumn(col_descripcion);
			lista_cargos_extras.AppendColumn(col_quien_cargo);
			lista_cargos_extras.AppendColumn(col_fecha_hora);
			lista_cargos_extras.AppendColumn(col_asignado);
			lista_cargos_extras.AppendColumn(col_lugarpro);
			lista_cargos_extras.AppendColumn(col_foliolab);
			lista_cargos_extras.AppendColumn(col_fechahora_solicitud);
		}
		
		// Cuando seleccion el treeview de cargos extras para cargar los productos  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (lista_cargos_extras.Model.GetIter (out iter, path)) {
				bool old = (bool) lista_cargos_extras.Model.GetValue (iter,0);
				lista_cargos_extras.Model.SetValue(iter,0,!old);
			}	
		}
		
		void on_button_graba_pago_clicked(object sender, EventArgs args)
		{
			if ((bool) aplico_cargos){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");
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
 						TreeIter iter;
 						if ((int) folioservicio > 0){  // Validando que seleccione un folio de atencion
							if (treeViewEngineServicio.GetIterFirst (out iter)){
								if ((bool)lista_de_servicios.Model.GetValue (iter,16)==false){
									comando.CommandText = "INSERT INTO osiris_erp_cobros_deta("+
 												"id_producto,"+
 												"folio_de_servicio,"+
 												"pid_paciente,"+
 												"cantidad_aplicada,"+
 												"id_tipo_admisiones,"+
 												"id_tipo_admisiones2,"+
 												"precio_producto, "+
 												//"precio_por_cantidad,"+
 												"iva_producto,"+
 												"precio_costo_unitario,"+
 												"porcentage_utilidad,"+
 												"porcentage_descuento,"+
 												"id_empleado,"+
 												"fechahora_creacion,"+
 												"porcentage_iva,"+
 												"precio_costo,"+
 												"folio_interno_dep,"+
 												"fechahora_solicitud) "+
 												"VALUES ('"+
 												double.Parse((string) lista_de_servicios.Model.GetValue(iter,2))+"','"+//id_prod
 												folioservicio+"','"+//folio
 												int.Parse((string)entry_pid_paciente.Text)+"','"+//pid
 												(float) lista_de_servicios.Model.GetValue(iter,1)+"','"+//cant aplicada
 												(int)lista_de_servicios.Model.GetValue(iter,15)+"','"+//id_tipo_admision
 												(int)lista_de_servicios.Model.GetValue(iter,14)+"','"+//id_tipo_admision2
 												double.Parse((string)lista_de_servicios.Model.GetValue(iter,7))+"','"+//precio_prod
 												//double.Parse((string)lista_de_servicios.Model.GetValue(iter,8))+"','"+//precio_cantdad
 												double.Parse((string)lista_de_servicios.Model.GetValue(iter,9))+"','"+//iva_prod
 												double.Parse((string)lista_de_servicios.Model.GetValue(iter,17))+"','"+//precio_unit
 												float.Parse((string)lista_de_servicios.Model.GetValue(iter,18))+"','"+//porcentage_util
 												float.Parse((string) lista_de_servicios.Model.GetValue(iter,11))+"','"+//porcentage_desc
 												LoginEmpleado+"','"+//id_empleado
 												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+//fecha de creacion
 												valoriva+"','"+//porcentage_iva
 												float.Parse((string)lista_de_servicios.Model.GetValue(iter,19))+"','"+//precio_costo
 												(string) lista_de_servicios.Model.GetValue(iter,22)+"','"+
 												(string) lista_de_servicios.Model.GetValue(iter,21)+ 
 												"');";
 												
 									//Console.WriteLine("query de graba pago "+comando.CommandText);
 									comando.ExecuteNonQuery();	comando.Dispose();
 									//QUERY para grabar los productos de los datos de cada estudio en su tabla para su posterior validacion
 									/*comando.CommandText ="INSERT INTO osiris_his_validaciones_estudios(folio_interno_dep,"+
 														"id_producto,fecha_solicitud,id_tipo_admisiones) "+
 														"VALUES('"+
 														(string) lista_de_servicios.Model.GetValue(iter,22)+"','"+//folio_interno_dep
 														(string) lista_de_servicios.Model.GetValue(iter,2)+"','"+//id_producto
 														(string) lista_de_servicios.Model.GetValue(iter,21)+"','"+//fecha_solicitud
 														(int)lista_de_servicios.Model.GetValue(iter,15)+"');";//id_tipo_admisiones
 									Console.WriteLine("query_de_valores_solicitud "+comando.CommandText);					
 									comando.ExecuteNonQuery();	comando.Dispose();*/
    	    	       			}	
    	    	       			while (treeViewEngineServicio.IterNext(ref iter)){
    	    	       				if ((bool)lista_de_servicios.Model.GetValue (iter,16) == false){
 										comando.CommandText = "INSERT INTO osiris_erp_cobros_deta("+
 														"id_producto,"+
 														"folio_de_servicio,"+
 														"pid_paciente,"+
 														"cantidad_aplicada,"+
 														"id_tipo_admisiones,"+
 														"id_tipo_admisiones2,"+
 														"precio_producto, "+
 														//"precio_por_cantidad,"+
 														"iva_producto,"+
 														"precio_costo_unitario,"+
 														"porcentage_utilidad,"+
 														"porcentage_descuento,"+
 														"id_empleado,"+
 														"fechahora_creacion,"+
 														"porcentage_iva,"+
 														"precio_costo,"+
 														"folio_interno_dep,"+
 														"fechahora_solicitud) "+
 														"VALUES ('"+
 														double.Parse((string) lista_de_servicios.Model.GetValue(iter,2))+"','"+//id_producto
 														folioservicio+"','"+//folio_de_servicio
 														int.Parse((string)entry_pid_paciente.Text)+"','"+//pid_paciente
 														(float) lista_de_servicios.Model.GetValue(iter,1)+"','"+//cantidad_aplicada
 														(int)lista_de_servicios.Model.GetValue(iter,15)+"','"+//id_tipo_admisiones
 														(int)lista_de_servicios.Model.GetValue(iter,14)+"','"+//id_tipo_admisiones2
 														double.Parse((string)lista_de_servicios.Model.GetValue(iter,7))+"','"+//precio_producto
 														//double.Parse((string)lista_de_servicios.Model.GetValue(iter,8))+"','"+//precio_por_cantidad
 														double.Parse((string)lista_de_servicios.Model.GetValue(iter,9))+"','"+//iva_producto
 														double.Parse((string)lista_de_servicios.Model.GetValue(iter,17))+"','"+//precio_costo_unitario
 														float.Parse((string)lista_de_servicios.Model.GetValue(iter,18))+"','"+//porcentage_utilidad
 														float.Parse((string) lista_de_servicios.Model.GetValue(iter,11))+"','"+//porcentage_descuento
 														LoginEmpleado+"','"+//id_empleado
 														DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+//fechahora_creacion
 														valoriva+"','"+//porcentage_iva
 														float.Parse((string)lista_de_servicios.Model.GetValue(iter,17))+"','"+//precio_costo
 														(string) lista_de_servicios.Model.GetValue(iter,22)+"','"+
 														(string) lista_de_servicios.Model.GetValue(iter,21)+
 														"');";
 										comando.ExecuteNonQuery();  					comando.Dispose();
 										/*Console.WriteLine("query de graba pago "+comando.CommandText);
 										comando.CommandText ="INSERT INTO osiris_his_validaciones_estudios(folio_interno_dep,"+
 														"id_producto,fecha_solicitud,id_tipo_admisiones) "+
 														"VALUES('"+
 														(string) lista_de_servicios.Model.GetValue(iter,22)+"','"+//folio_interno_dep
 														(string) lista_de_servicios.Model.GetValue(iter,2)+"','"+//id_producto
 														(string) lista_de_servicios.Model.GetValue(iter,21)+"','"+//fecha_solicitud
 														(int)lista_de_servicios.Model.GetValue(iter,15)+"');";//id_tipo_admisiones
 										Console.WriteLine("query_de_valores_solicitud "+comando.CommandText);					
 										comando.ExecuteNonQuery();	comando.Dispose();*/
 									}
 								}
 							}
 						}else{
 							//Console.WriteLine(folioservicio.ToString());
 							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Info, ButtonsType.Close, "Seleccione algun folio de atencion...");
							msgBoxError.Run ();		msgBoxError.Destroy();
 				
 						}
					}catch (NpgsqlException ex){
	   					Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();	msgBoxError.Destroy();
	       			}
       				conexion.Close ();
 					llenado_de_productos_aplicados( (string) entry_folio_servicio.Text );
 				}
 			}else{
 				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info, 		ButtonsType.Close, "No grabo, ya que NO CARGO NADA");
				msgBoxError.Run ();	msgBoxError.Destroy();
 			}
		}
		
		void on_button_aplica_cargos_clicked (object sender, EventArgs args)
		{
			// leyendo treeview de productos extras para cargar
			string toma_valor1;
			string toma_valor2;
			string toma_valor3;
			//float toma_a_pagar = 0;
			aplico_cargos = true;
									
			TreeIter iter;
 			if (treeViewEngineExtras.GetIterFirst (out iter)){
 				if ((float) lista_cargos_extras.Model.GetValue (iter,1) > 0){
 					if ((bool)lista_cargos_extras.Model.GetValue (iter,0)){
 						lista_cargos_extras.Model.SetValue(iter,0,false);
 						toma_valor1 = (string) lista_cargos_extras.Model.GetValue (iter,9);  // toma precio publico
 						toma_valor2 = (string) lista_cargos_extras.Model.GetValue (iter,10);  // toma el iva
 						toma_valor3 = (string) lista_cargos_extras.Model.GetValue (iter,13);  // toma el descuento
 						
 						if ((float) float.Parse(toma_valor2) > 0){
 							subtotal_al_15 = subtotal_al_15 + float.Parse(toma_valor1);
 						}else{
 					 		subtotal_al_0 = subtotal_al_0 + float.Parse(toma_valor1);
 						}
 						 						
 						// Verificando si aplica descuento por tarjeta de Descuento
 						if ((int) lista_cargos_extras.Model.GetValue (iter,15) == 100 && (int) id_tipopaciente == 101 && aplicar_siempre == false
					  		||(int) lista_cargos_extras.Model.GetValue (iter,15) == 300 && (int) id_tipopaciente == 101 && aplicar_siempre == false
					  		||(int) lista_cargos_extras.Model.GetValue (iter,15) == 400 && (int) id_tipopaciente == 101 && aplicar_siempre == false){
							aplicar_descuento = true;
						}else{
							if (aplicar_siempre == false){
								aplicar_descuento = false;
								aplicar_siempre = true;
							}
						}	
 						
 						total_iva = total_iva + float.Parse(toma_valor2);
 						totaldescuento = totaldescuento + float.Parse(toma_valor3)+((float.Parse(toma_valor3)*valoriva)/100);
 						 					 					 					
 						// Traspaso los valores de valores extras para que se carguen a la cuenta del
 						// paciente
 						treeViewEngineServicio.AppendValues ((string) lista_cargos_extras.Model.GetValue (iter,3),//0 descripcionprod
 														(float) lista_cargos_extras.Model.GetValue (iter,1),//1 cantidad
 														(string) lista_cargos_extras.Model.GetValue (iter,2),//2 idproducto
 														(string) lista_cargos_extras.Model.GetValue (iter,4),//3 login
 														(string) lista_cargos_extras.Model.GetValue (iter,5),//4 fecha 
 														(string) lista_cargos_extras.Model.GetValue (iter,6),//5 id_internamiento
 														(string) lista_cargos_extras.Model.GetValue (iter,7),//6 id_lugarprocedencia
 														(string) lista_cargos_extras.Model.GetValue (iter,8),//7 precioprod
 														(string) lista_cargos_extras.Model.GetValue (iter,9),//8 ppcantidad
 														(string) lista_cargos_extras.Model.GetValue (iter,10),//9 ivaprod
 														(string) lista_cargos_extras.Model.GetValue (iter,11),//10 suma ppcant+iva
 														(string) lista_cargos_extras.Model.GetValue (iter,12),//11 porcientodesc
 														(string) lista_cargos_extras.Model.GetValue (iter,13),//12 valordedesc
 														(string) lista_cargos_extras.Model.GetValue (iter,14),//13 precio-valordedesc
 														(int) lista_cargos_extras.Model.GetValue (iter,15),//lugar procedencia//14 
 														(int) lista_cargos_extras.Model.GetValue (iter,16),//laboratorio 400//15
 														(bool) false,//16 
 														(string) lista_cargos_extras.Model.GetValue (iter,17),//17 costo_unitario
 														(string) lista_cargos_extras.Model.GetValue (iter,18),//18 porcentajeutilidad
 														(string) lista_cargos_extras.Model.GetValue (iter,19),//19 costototalprod
 														(string) "",//20
 														(string) lista_cargos_extras.Model.GetValue (iter,20),//21fechahora__solicitud
 														(string) lista_cargos_extras.Model.GetValue (iter,21));//22folio_laboratorio
 														
 					}
 				}
 				while (treeViewEngineExtras.IterNext(ref iter)){
 					if ((float) lista_cargos_extras.Model.GetValue (iter,1) > 0){
 						if ((bool)lista_cargos_extras.Model.GetValue (iter,0)){
 					
 							lista_cargos_extras.Model.SetValue(iter,0,false);
 							toma_valor1 = (string) lista_cargos_extras.Model.GetValue (iter,9);  // toma precio publico
 							toma_valor2 = (string) lista_cargos_extras.Model.GetValue (iter,10);  // toma el iva
 							toma_valor3 = (string) lista_cargos_extras.Model.GetValue (iter,13);  // toma el precio con descuento
 							
 							if ((float) float.Parse(toma_valor2) > 0)
 							{
 								subtotal_al_15 = subtotal_al_15 + float.Parse(toma_valor1);
 							}else{
 					 			subtotal_al_0 = subtotal_al_0 + float.Parse(toma_valor1);
 							}
 							if ((int) lista_cargos_extras.Model.GetValue (iter,16) == 100 && (int) id_tipopaciente == 101 && aplicar_siempre == false
					  			||(int) lista_cargos_extras.Model.GetValue (iter,16) == 300 && (int) id_tipopaciente == 101 && aplicar_siempre == false
					  			||(int) lista_cargos_extras.Model.GetValue (iter,16) == 400 && (int) id_tipopaciente == 101 && aplicar_siempre == false){
								aplicar_descuento = true;
							}else{
								if (aplicar_siempre == false){
									aplicar_descuento = false;
									aplicar_siempre = true;
								}
							}
							
 							total_iva = total_iva + float.Parse(toma_valor2);
 							totaldescuento = totaldescuento + float.Parse(toma_valor3)+((float.Parse(toma_valor3)*valoriva)/100);
 							 						
 							treeViewEngineServicio.AppendValues (
 										(string) lista_cargos_extras.Model.GetValue (iter,3),
 										(float) lista_cargos_extras.Model.GetValue (iter,1),
 										(string) lista_cargos_extras.Model.GetValue (iter,2),
 										(string) lista_cargos_extras.Model.GetValue (iter,4),
 										(string) lista_cargos_extras.Model.GetValue (iter,5),
 										(string) lista_cargos_extras.Model.GetValue (iter,6),
 										(string) lista_cargos_extras.Model.GetValue (iter,7),
 										(string) lista_cargos_extras.Model.GetValue (iter,8),
 										(string) lista_cargos_extras.Model.GetValue (iter,9),
 										(string) lista_cargos_extras.Model.GetValue (iter,10),
 										(string) lista_cargos_extras.Model.GetValue (iter,11),
 										(string) lista_cargos_extras.Model.GetValue (iter,12),
 										(string) lista_cargos_extras.Model.GetValue (iter,13),
 										(string) lista_cargos_extras.Model.GetValue (iter,14),//////
 										(int) lista_cargos_extras.Model.GetValue (iter,15),//////////
 										(int) lista_cargos_extras.Model.GetValue (iter,16),//
 										(bool) false,
 										(string) lista_cargos_extras.Model.GetValue (iter,17),
 										(string) lista_cargos_extras.Model.GetValue (iter,18),
 										(string) lista_cargos_extras.Model.GetValue (iter,19),
 										(string) "",
 										(string) lista_cargos_extras.Model.GetValue (iter,20),//fechahora__solicitud
 										(string) lista_cargos_extras.Model.GetValue (iter,21));//folio_laboratorio);
 						}
 					}
 				}
 				if (aplicar_descuento == false){
					totaldescuento = 0;
				}
							
				// Realizando las restas
				sub_total = subtotal_al_15 + subtotal_al_0+total_iva;
				//toma_a_pagar = sub_total - totaldescuento;
			}
 		}
		
		// Remueve productos aplicados que ya han sido cargados pero no actualizados
		// en la tabla de detalle
		void on_button_quitar_aplicados_clicked (object sender, EventArgs args)
		{
			TreeIter iter;
 			TreeModel model;
 			string toma_valor1;
 			string toma_valor2;
 			string toma_valor3;
 			//float toma_a_pagar = 0;
 			

 			if (lista_de_servicios.Selection.GetSelected (out model, out iter)) {
 				toma_valor1 = (string) lista_de_servicios.Model.GetValue(iter,9);	
				toma_valor2 = (string) lista_de_servicios.Model.GetValue (iter,10);  // toma el iva
				toma_valor3 = (string) lista_de_servicios.Model.GetValue (iter,13);  // toma el descuento
				
				
				if (!(bool) lista_de_servicios.Model.GetValue (iter,16)){
								
 					treeViewEngineServicio.Remove (ref iter);
 				 					
 					if ((float) float.Parse(toma_valor2) > 0){
 						subtotal_al_15 = subtotal_al_15 - float.Parse(toma_valor1);
 					}else{
 						subtotal_al_0 = subtotal_al_0 - float.Parse(toma_valor1);
 					}
 					total_iva = total_iva - float.Parse(toma_valor2);
				
					sub_total = subtotal_al_15 + subtotal_al_0 + total_iva;
					totaldescuento -= (float.Parse(toma_valor3) + ((float.Parse(toma_valor3) * valoriva)/100));
					//toma_a_pagar = sub_total - totaldescuento;
					
 				}else{
 					if (LoginEmpleado =="DOLIVARES" || LoginEmpleado =="HVARGAS" || LoginEmpleado =="JPENA" || LoginEmpleado == "N000073"){
 						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Question,ButtonsType.YesNo,"¿ Desea DEVOLVER este producto ?");
						ResponseType miResultado = (ResponseType)msgBox.Run (); 	msgBox.Destroy();
	 			
	 					if (miResultado == ResponseType.Yes){
							NpgsqlConnection conexion; 
							conexion = new NpgsqlConnection (connectionString+nombrebd);
			    			// Verifica que la base de datos este conectada
							try{
								conexion.Open ();
								NpgsqlCommand comando; 
								comando = conexion.CreateCommand ();
				 				comando.CommandText = "UPDATE osiris_erp_cobros_deta "+
										"SET eliminado = 'true' , "+
										"fechahora_eliminado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
										"id_quien_elimino = '"+LoginEmpleado+"' "+								
				 						"WHERE id_secuencia =  '"+(string) lista_de_servicios.Model.GetValue (iter,20)+"';";
										comando.ExecuteNonQuery();
					        			comando.Dispose();
			        			
			        			// Duplicando registro registro pero con cantidad negativa y elimacion activa			        			
					        	comando.CommandText = "INSERT INTO osiris_erp_cobros_deta("+
					        							"id_producto,"+
					        							"folio_de_servicio,"+
					        							"pid_paciente,"+
					        										    "eliminado,"+
					        											"fechahora_eliminado,"+
					        											"id_quien_elimino,"+
					        											"id_secuencia,"+
					        							"id_tipo_admisiones,"+
					        							"precio_producto,"+
					        							//"iva_producto,"+
					        							"id_tipo_admisiones2,"+
					        							"precio_costo_unitario,"+
					        							"porcentage_utilidad,"+
					        							"porcentage_descuento,"+
					        							"porcentage_iva,"+
					        							"precio_costo,"+
					        										"fechahora_creacion,"+
									        			"id_empleado,"+
									        			"cantidad_aplicada,"+
									        			"id_almacen) "+
									        			//"precio_por_cantidad )"+
									        			//"numero_factura) "+
									        			
					        							"SELECT "+
					        							"id_producto,"+
					        							"folio_de_servicio,"+
					        							"pid_paciente,"+
					        											"'true',"+
					        											"'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
					        											"'"+LoginEmpleado+"',"+					        							
					        											"NEXTVAL('osiris_erp_cobros_deta_id_secuencia_seq'),"+
					        							"id_tipo_admisiones,"+
					        							"precio_producto * -1,"+
					        							//"iva_producto * -1,"+
					        							"id_tipo_admisiones2,"+
					        							"precio_costo_unitario * -1,"+
					        							"porcentage_utilidad,"+
					        							"porcentage_descuento,"+
					        							"porcentage_iva,"+
					        							"precio_costo,"+
					        											"'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
					        											"'"+LoginEmpleado+"',"+	
									        			"cantidad_aplicada * -1,"+
									        			"id_almacen "+
									        			//"precio_por_cantidad * -1 "+
									        			//"numero_factura "+
									        			"FROM osiris_erp_cobros_deta "+
					        							"WHERE id_secuencia =  '"+(string) lista_de_servicios.Model.GetValue (iter,20)+"';";
					        			//Console.WriteLine(comando.CommandText.ToString());
					        			comando.ExecuteNonQuery();        			comando.Dispose();
					        			
					        	//Console.WriteLine(comando.CommandText.ToString());
			        			//button_busca_producto.Sensitive = false;
			        			msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
								MessageType.Info,ButtonsType.Ok,"El Producto "+(string) lista_de_servicios.Model.GetValue (iter,0)+" se devolvio satisfactoriamente");
								msgBox.Run ();		msgBox.Destroy();
								llenado_de_productos_aplicados( (string) entry_folio_servicio.Text );
								conexion.Close ();
			        		}catch (NpgsqlException ex){
				   				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				   				msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Error,ButtonsType.Ok,"PostgresSQL error: {0}",ex.Message);
								msgBox.Run();	msgBox.Destroy();
				   			}
 							//Console.WriteLine((string) lista_de_servicios.Model.GetValue (iter,18));
 						}
 					}else{
 						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Error,ButtonsType.Ok,"No esta autorizado para esta opcion...");
						msgBox.Run();	msgBox.Destroy();
 					}
 				}
 			}
		}
		
		void on_selec_folio_clicked(object sender, EventArgs args)
		{
			llenado_de_productos_aplicados( (string) entry_folio_servicio.Text );
		}
		
		void on_button_actualizar_clicked(object sender, EventArgs args)
		{
			llenado_de_productos_aplicados( (string) entry_folio_servicio.Text );
		}
		
		// Este toma los valores para llenar el encabezado del procedimiento
		// Aqui lleno el detalle de los servicios que se va aplicar para su cobro
		void llenado_de_productos_aplicados(string foliodeserv)
		{
			subtotal_al_15 = 0;
			subtotal_al_0 = 0;
			total_iva = 0;
			totaldescuento = 0;
			sub_total = 0;
			id_tipopaciente = 0;
			
			entry_fecha_admision.Sensitive = true;
			entry_hora_registro.Sensitive = true;
			entry_fechahora_alta.Sensitive = true;
			entry_nombre_paciente.Sensitive = true;
			entry_pid_paciente.Sensitive = true;
			entry_telefono_paciente.Sensitive = true;
			entry_cirugia.Sensitive = true;
			entry_doctor.Sensitive = true;
			entry_especialidad.Sensitive = true;
			entry_tipo_paciente.Sensitive = true;
			entry_aseguradora.Sensitive = true;
			entry_poliza.Sensitive = true;
			entry_desc_producto.Sensitive = true;
			entry_edad.Sensitive = true;
			entry_fecha_nacimiento.Sensitive = true;
			this.entry_id_habitacion.Sensitive = true;
			lista_de_servicios.Sensitive = true;
			lista_cargos_extras.Sensitive = true;
			entry_descrip_cirugia.Sensitive = true;	
			
			button_removerItem.Sensitive = true;
			button_graba_pago.Sensitive = true;
			button_aplica_cargos.Sensitive = true;
			button_quitar_aplicados.Sensitive = true;
			button_actualizar.Sensitive = true;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT DISTINCT (osiris_erp_movcargos.folio_de_servicio),osiris_erp_cobros_enca.folio_de_servicio AS foliodeatencion, "+
								"osiris_erp_cobros_enca.pagado,"+
								"osiris_erp_cobros_enca.cancelado,"+
								"osiris_erp_cobros_enca.cerrado,"+
								"osiris_erp_cobros_enca.bloqueo_de_folio,"+
								"to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente,"+
				            	"nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente,"+
				            	"telefono_particular1_paciente,numero_poliza,folio_de_servicio_dep,osiris_empresas.id_empresa,osiris_empresas.descripcion_empresa,"+
				            	"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy') AS fecha_registro,"+
				            	"to_char(osiris_erp_cobros_enca.fechahora_creacion,'HH24:mi:ss') AS hora_registro,"+
				            	"to_char(osiris_erp_cobros_enca.fecha_alta_paciente,'dd-MM-yyyy HH24:mi:ss') AS fechahora_alta,"+
				            	"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fecha_nac_pa, "+
				            	"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
				            	"osiris_his_paciente.direccion_paciente,osiris_his_paciente.numero_casa_paciente,osiris_his_paciente.numero_departamento_paciente, "+
								"osiris_his_paciente.colonia_paciente,osiris_his_paciente.municipio_paciente,osiris_his_paciente.codigo_postal_paciente,osiris_his_paciente.estado_paciente,  "+
            					"osiris_empresas.lista_de_precio AS listadeprecio_empresa,"+//////
				            	"osiris_aseguradoras.lista_de_precio AS listadeprecio_aseguradora,"+
				            	"osiris_his_paciente.sexo_paciente,"+
				            	"osiris_erp_cobros_enca.nombre_medico_tratante,"+
				            	"osiris_erp_movcargos.nombre_de_cirugia,"+
            					"osiris_erp_movcargos.id_tipo_paciente AS idtipopaciente,descripcion_tipo_paciente,osiris_his_tipo_cirugias.descripcion_cirugia,alta_paciente,"+
            					"osiris_erp_movcargos.id_tipo_admisiones AS idtipoadmision,descripcion_admisiones,osiris_his_tipo_especialidad.descripcion_especialidad,"+
				            	"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora, osiris_erp_cobros_enca.id_medico,nombre_medico,descripcion_diagnostico_movcargos,"+
				            	
								"osiris_erp_cobros_enca.id_habitacion,to_char(osiris_his_habitaciones.numero_cuarto,'999999999') AS numerocuarto,osiris_his_habitaciones.descripcion_cuarto,osiris_his_habitaciones.id_tipo_admisiones AS idtipoadmisiones_habitacion,"+
				            	
				            	"osiris_erp_cobros_enca.id_empresa AS idempresa "+ 
				            	"FROM "+ 
				            	"osiris_erp_cobros_enca,osiris_his_paciente,osiris_erp_movcargos,osiris_his_tipo_admisiones,osiris_his_tipo_pacientes, "+
				            	"osiris_his_habitaciones, "+
				            	"osiris_aseguradoras, osiris_his_medicos,osiris_his_tipo_cirugias,osiris_his_tipo_especialidad,osiris_empresas "+
				            	"WHERE "+
				            	"osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
				            	"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
				            	"AND osiris_erp_cobros_enca.id_medico = osiris_his_medicos.id_medico "+ 
								"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+ 
								"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
								"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
								"AND osiris_erp_movcargos.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
								"AND osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad  "+
								"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
								"AND osiris_erp_cobros_enca.id_habitacion = osiris_his_habitaciones.id_habitacion "+
								"AND osiris_erp_cobros_enca.folio_de_servicio = "+(string) foliodeserv+";";
				//Console.WriteLine("encabezado "+comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if(lector.Read())
				{
					entry_fecha_admision.Text = (string) lector["fecha_registro"];
					entry_hora_registro.Text = (string) lector["hora_registro"];
					entry_fechahora_alta.Text = (string) lector["fechahora_alta"];
					entry_nombre_paciente.Text = (string) lector["nombre1_paciente"]+" "+(string)lector["nombre2_paciente"]+" "+(string)lector["apellido_paterno_paciente"]+" "+(string)lector["apellido_materno_paciente"];
					entry_pid_paciente.Text = (string) lector["pidpaciente"];
					entry_telefono_paciente.Text = (string) lector["telefono_particular1_paciente"];
					entry_doctor.Text = (string) lector["nombre_medico_tratante"];
					entry_especialidad.Text = (string) lector["descripcion_especialidad"];
					entry_tipo_paciente.Text = (string) lector["descripcion_tipo_paciente"];
					entry_aseguradora.Text = (string) lector["descripcion_aseguradora"];
					entry_poliza.Text =  (string) lector["numero_poliza"];
					
					entry_id_habitacion.Text = (string) lector["numerocuarto"];
					entry_id_habitacion.Text = entry_id_habitacion.Text.Trim()+"/"+(string) lector["descripcion_cuarto"];
					
					entry_descrip_cirugia.Text = (string) lector["nombre_de_cirugia"];
															
					entry_edad.Text = (string) lector["edad"];
            		entry_fecha_nacimiento.Text = (string) lector ["fecha_nac_pa"];
			       	entry_cirugia.Text = (string) lector["descripcion_diagnostico_movcargos"]; //(string) lector["descripcion_cirugia"];
            		
            		sexopaciente = (string) lector["sexo_paciente"];
            		id_tipopaciente = (int) lector["idtipopaciente"];
            		dir_pac = (string) lector["direccion_paciente"]+"  "+(string) lector["numero_casa_paciente"]+"  "+
            				(string) lector["numero_departamento_paciente"]+",  COL. "+(string) lector["colonia_paciente"]+
            					",  CP.  "+(string) lector["codigo_postal_paciente"]+",  "+(string) lector["municipio_paciente"]+",   "+(string) lector["estado_paciente"];
            					
					idempresa_paciente = (int) lector["idempresa"];
					////////////////////////////////////////////////////////////////////
					// nuevo para multiples listas de precios
					idaseguradora_paciente = (int) lector["id_aseguradora"];
					aplica_precios_aseguradoras = (bool) lector["listadeprecio_aseguradora"];
					aplica_precios_empresas = (bool) lector["listadeprecio_empresa"];
					////////////////////////////////////////////////////////////////////
					if((int) lector ["id_aseguradora"] > 1){
						entry_aseguradora.Text = (string) lector["descripcion_aseguradora"];
					}else{
						entry_aseguradora.Text = (string) lector["descripcion_empresa"];						
					}
            		
            		idhabitacion = (int) lector["id_habitacion"];
            		//int foliointernodep = (int) lector["folio_de_servicio_dep"];
					
					PidPaciente = int.Parse(entry_pid_paciente.Text);		    // Toma la actualizacion del pid del paciente
					folioservicio = int.Parse(foliodeserv);
					
					if((bool) lector ["cancelado"] == false ){
						//Console.WriteLine("cancelado "+lector ["cancelado"]);
						button_busca_producto.Sensitive = false;
						
						if((bool) lector ["alta_paciente"] == false){
							//Console.WriteLine("pagado "+lector ["pagado"]);
							button_busca_producto.Sensitive = true;
							if ((bool) lector ["pagado"] == false){
								//Console.WriteLine("pagado "+lector ["pagado"]);
								button_busca_producto.Sensitive = true;
								if ((bool) lector ["cerrado"] == false){
									//Console.WriteLine("cerrado "+lector ["cerrado"]);
									button_busca_producto.Sensitive = true;
									if ((bool) lector ["bloqueo_de_folio"] == false){
										//Console.WriteLine("bloqueo_de_folio "+lector ["bloqueo_de_folio"]);
										button_busca_producto.Sensitive = true;
									}else{
										button_busca_producto.Sensitive = false;
										MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close, "Este procedimiento se encuentra BLOQUEADO \n"+
													"solo el personal de CAJA podra agregar Cargos");
										msgBoxError.Run ();		msgBoxError.Destroy();
									}
								}else{
									button_busca_producto.Sensitive = false;
									MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error, ButtonsType.Close, "Este procedimiento se encuentra CERRADO \n"+
													"NO podra agregar mas cargos a esta cuenta");
									msgBoxError.Run ();				msgBoxError.Destroy();
									button_busca_producto.Sensitive = false;
								}
							}else{
								button_busca_producto.Sensitive = false;
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, ButtonsType.Close, "Este procedimiento se encuentra PAGADO \n"+
													"NO podra agregar mas cargos a esta cuenta");
								msgBoxError.Run (); msgBoxError.Destroy();
							}
						}else {
							button_busca_producto.Sensitive = false;
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, 	ButtonsType.Close, "El paciente ya fue dado de ALTA\n"+
											"NO podra agregar mas cargos a esta cuenta");
							msgBoxError.Run ();		msgBoxError.Destroy();
						}
					}else{
						button_busca_producto.Sensitive = false;
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error,ButtonsType.Close, "Este procedimiento se encuentra CANCELADO \n"+
											"NO podra agregar mas cargos a esta cuenta");
						msgBoxError.Run ();	msgBoxError.Destroy();
					}						
				}
				llenado_de_material_aplicado(foliodeserv);
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run (); msgBoxError.Destroy();
	   		}
       		conexion.Close ();
		}
		// llenando el detalle de procedimiento de cobranza
		void llenado_de_material_aplicado(string foliodeserv)
		{	
			// Limpiando Varibles a valor 0
			subtotal_al_15 = 0;
			subtotal_al_0 = 0;
			total_iva = 0;
			sub_total = 0;
			totaldescuento = 0;
			aplicar_descuento = true;
			aplicar_siempre = false;
			
			// Limpio el treeview de los productos que se han aplicado y estan
			// grabado en la tabla detalle
			treeViewEngineServicio.Clear();
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			            
			// Verifica que la base de datos este conectada
			try{
				//Console.WriteLine("LLENANDO MATERIALES");
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT descripcion_producto,to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto, "+
							"to_char(osiris_erp_cobros_deta.cantidad_aplicada,'99999.99') AS cantidadaplicada,"+
							"to_char(osiris_erp_cobros_deta.precio_producto,'99999999.99') AS preciounitario,"+
							//"to_char(osiris_erp_cobros_deta.precio_por_cantidad,'99999999.99') AS precioporcant,"+
							"to_char(osiris_erp_cobros_deta.cantidad_aplicada * osiris_erp_cobros_deta.precio_producto,'99999999.99') AS ppcantidad_,"+
							"to_char(osiris_erp_cobros_deta.iva_producto,'99999999.99') AS ivaproducto,"+
							"to_char(osiris_erp_cobros_deta.porcentage_descuento,'999.99') AS pdescuento,"+
							//"osiris_erp_cobros_deta.observacion_detalle, "+//"to_char(osiris_erp_cobros_deta.folio_de_laboratorio,'999.99') AS folio_lab,"+
							"to_char(osiris_erp_cobros_deta.folio_interno_dep,'9999999999') AS foliointdep, "+
							"to_char(osiris_erp_cobros_deta.fechahora_solicitud,'dd-mm-yyyy HH24:mi:ss') AS fechahorasolicitud,"+
							"id_empleado,"+
							"to_char(osiris_erp_cobros_deta.fechahora_creacion,'dd-mm-yyyy HH24:mi:ss') AS fechcreacion,"+
							"osiris_his_tipo_admisiones.descripcion_admisiones,"+
							"osiris_erp_cobros_deta.id_tipo_admisiones AS idtipoadmision,"+
							"osiris_erp_cobros_deta.id_tipo_admisiones2 AS idtipoadmision2,"+
							"eliminado, agrupacion2,agrupacion3,"+
							"to_char(osiris_erp_cobros_deta.id_secuencia,'9999999999') AS secuencia "+
							"FROM osiris_erp_cobros_deta,osiris_productos,osiris_grupo_producto,osiris_his_tipo_admisiones "+
							"WHERE osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+
							"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
							"AND osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
							"AND osiris_erp_cobros_deta.eliminado = false "+ 
							"AND folio_de_servicio = '"+(string) foliodeserv+"' "+
							"AND osiris_grupo_producto.agrupacion3 = 'IMG' "+
							"AND osiris_erp_cobros_deta.id_tipo_admisiones = '300' "+//linea para que solo lea los productos aplicados por este departamento
							"ORDER BY osiris_productos.descripcion_producto,to_char(osiris_erp_cobros_deta.fechahora_creacion,'dd-mm-yyyy HH:mm:ss');";
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//Console.WriteLine("query materiales aplicados: "+comando.CommandText.ToString());
				
				float toma_cantaplicada = 0;
				float toma_subtotal = 0;
				//float toma_a_pagar = 0;
				float preciocondesc = 0;
				float valordescuento = 0;
				string descrprod = "";
				string numerofolioexamen = "";
				string fechasolicitudexamen = "";
				
				while (lector.Read()){
				
					if (!(bool) lector["eliminado"]){
						if((string)lector ["agrupacion3"] == "IMG"){
							///SE LEE EL VALOR DEL LUGAR DONDE SE PIDIO EL ESTUDIO DE LABORATORIO
							busca_tipoadm2((string)lector ["secuencia"],foliodeserv);
						}
						
						toma_cantaplicada = float.Parse((string) lector["cantidadaplicada"]);
						toma_subtotal = float.Parse((string) lector["ppcantidad_"])+float.Parse((string) lector["ivaproducto"]);
						
						valordescuento = 0;
						preciocondesc = 0;
						if (float.Parse((string) lector["pdescuento"]) > 0){
							// Calculando el Descuento
							valordescuento = ((float.Parse((string) lector["ppcantidad_"])*float.Parse((string) lector["pdescuento"]))/100);					
							preciocondesc = float.Parse((string) lector["ppcantidad_"])-valordescuento;
						}
					
						if (float.Parse((string) lector["ivaproducto"]) > 0) { subtotal_al_15 = subtotal_al_15 + float.Parse((string) lector["ppcantidad_"]);
 						}else{ subtotal_al_0 = subtotal_al_0 + float.Parse((string) lector["ppcantidad_"]);	}
 					
 						// Verificando si aplica descuento por targeta de Descuento
 						
 						if ((int) lector["idtipoadmision"] == 100 && (int) id_tipopaciente == 101 && aplicar_siempre == false
					  		||(int) lector["idtipoadmision"] == 300 && (int) id_tipopaciente == 101 && aplicar_siempre == false
					  		||(int) lector["idtipoadmision"] == 400 && (int) id_tipopaciente == 101 && aplicar_siempre == false){
							aplicar_descuento = true;
						}else{
							if (aplicar_siempre == false){ aplicar_siempre = true;	aplicar_descuento = false;	}
						}
					
						totaldescuento = totaldescuento + valordescuento + ((valordescuento*valoriva)/100);
 					 					
						total_iva = total_iva + float.Parse((string) lector["ivaproducto"]);
						descrprod = (string) lector["descripcion_producto"];
						
						numerofolioexamen = (string) lector["foliointdep"];
						fechasolicitudexamen = (string) lector["fechahorasolicitud"];
						
						if(descrprod.Length > 68) { descrprod = descrprod.Substring(0,68); }
						 
						if(float.Parse((string) lector["foliointdep"]) >0 ){
							numerofolioexamen = (string) lector["foliointdep"];
							fechasolicitudexamen = (string) lector["fechahorasolicitud"];
						}
						
						
						treeViewEngineServicio.AppendValues (descrprod,
														(float) toma_cantaplicada,
														(string) lector["idproducto"],
														(string) lector["id_empleado"],
														(string) lector["fechcreacion"],
														(string) lector["descripcion_admisiones"],
														descriplugarprocedencia.ToString(),
														(string) lector["preciounitario"],
														(string) lector["ppcantidad_"],
														(string) lector["ivaproducto"],
														toma_subtotal.ToString("F"),
														(string) lector["pdescuento"],
														valordescuento.ToString("F"),
														preciocondesc.ToString("F"),
														(int) lector["idtipoadmision2"],
														(int) lector["idtipoadmision"],
														(bool) true,
														(string) "",
														(string) "",
														(string) "",
														(string) lector["secuencia"],
														fechasolicitudexamen,
														numerofolioexamen);
					}
				}
				if (aplicar_descuento == false){
					totaldescuento = 0;
				}
				// Realizando las restas 
				sub_total = subtotal_al_15 + subtotal_al_0+total_iva;
				//toma_a_pagar = sub_total - totaldescuento;
									
			}catch (NpgsqlException ex){			
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void busca_tipoadm2(string secuencia, string foliodeserv)
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT id_tipo_admisiones2,osiris_his_tipo_admisiones.descripcion_admisiones "+
									"FROM osiris_erp_cobros_deta,osiris_his_tipo_admisiones "+
									"WHERE  osiris_erp_cobros_deta.id_tipo_admisiones2 = osiris_his_tipo_admisiones.id_tipo_admisiones "+
									"AND osiris_erp_cobros_deta.eliminado = 'false' "+
									"AND folio_de_servicio = '"+(string) foliodeserv+"' "+
									"AND id_secuencia = '"+secuencia+"' ;";
									
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if(lector.Read()){
					descriplugarprocedencia = (string) lector["descripcion_admisiones"];
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		// busco un paciente pantalla de ingreso de nuevo paciente
		void on_button_buscar_paciente_clicked(object sender, EventArgs args)
	    	{
			Glade.XML gxml = new Glade.XML (null, "laboratorio.glade", "busca_paciente", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda("paciente");
			button_buscar_busqueda.Clicked += new EventHandler(on_buscar_paciente_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_paciente_clicked);
			button_nuevo_paciente.Sensitive = false;
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
		}
	    
		void on_button_busca_producto_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "laboratorio.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			busca_producto.Show();
			
			label_departamento.Text = "Folio Imagenologia ";
			crea_treeview_busqueda("producto");
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			entry_fecha_solicitud.Text = DateTime.Now.ToString("yyyy-MM-dd");
			entry_hora_solicitud.Text = DateTime.Now.ToString("HH:mm:ss");
						
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
			entry_expresion.KeyPressEvent += onKeyPressEvent_entry_expresion;
			// Validando que sean solo numeros
			entry_cantidad_aplicada.KeyPressEvent += onKeyPressEvent;
			entry_folio_laboratorio.KeyPressEvent += onKeyPressEvent;
			
			//SE LLENA EL COMBO BOX	
			combobox_tipo_admision.Clear();
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
               	comando.CommandText = "SELECT * FROM osiris_his_tipo_admisiones WHERE servicio_directo = 'false' "+
	           							"AND cuenta_mayor = 4000 "+
	               						" ORDER BY id_tipo_admisiones;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				store2.AppendValues ("", 0);
               	while (lector.Read()){
					store2.AppendValues ((string) lector["descripcion_admisiones"], (int) lector["id_tipo_admisiones"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();	        
			
			TreeIter iter2;
			if (store2.GetIterFirst(out iter2)) {
				//Console.WriteLine(iter2);
				combobox_tipo_admision.SetActiveIter (iter2);
			}
			combobox_tipo_admision.Changed += new EventHandler (onComboBoxChanged_tipo_admision);			
	    }

		void crea_treeview_busqueda(string tipo_busqueda)
		{
			if (tipo_busqueda == "paciente"){
				treeViewEngineBusca = new TreeStore(typeof(int),
													typeof(int),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));
													
				lista_de_Pacientes.Model = treeViewEngineBusca;
			
				lista_de_Pacientes.RulesHint = true;
			
				lista_de_Pacientes.RowActivated += on_selecciona_paciente_clicked;  // Doble click selecciono paciente*/

				TreeViewColumn col_foliodeatencion = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_foliodeatencion.Title = "Folio de Antencion"; // titulo de la cabecera de la columna, si está visible
				col_foliodeatencion.PackStart(cellr0, true);
				col_foliodeatencion.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
				col_foliodeatencion.SortColumnId = (int) Column.col_foliodeatencion;
			
				TreeViewColumn col_PidPaciente = new TreeViewColumn();
				CellRendererText cellr1 = new CellRendererText();
				col_PidPaciente.Title = "PID Paciente"; // titulo de la cabecera de la columna, si está visible
				col_PidPaciente.PackStart(cellr1, true);
				col_PidPaciente.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
				col_PidPaciente.SortColumnId = (int) Column.col_PidPaciente;
				//cellr0.Editable = true;   // Permite edita este campo
            
				TreeViewColumn col_Nombre1_Paciente = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_Nombre1_Paciente.Title = "Nombre 1";
				col_Nombre1_Paciente.PackStart(cellrt2, true);
				col_Nombre1_Paciente.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
				col_Nombre1_Paciente.SortColumnId = (int) Column.col_Nombre1_Paciente;
            
				TreeViewColumn col_Nombre2_Paciente = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_Nombre2_Paciente.Title = "Nombre 2";
				col_Nombre2_Paciente.PackStart(cellrt3, true);
				col_Nombre2_Paciente.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 2 en vez de 3
				col_Nombre2_Paciente.SortColumnId = (int) Column.col_Nombre2_Paciente;
            
				TreeViewColumn col_app_Paciente = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_app_Paciente.Title = "Apellido Paterno";
				col_app_Paciente.PackStart(cellrt4, true);
				col_app_Paciente.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 3 en vez de 4
				col_app_Paciente.SortColumnId = (int) Column.col_app_Paciente;
            
				TreeViewColumn col_apm_Paciente = new TreeViewColumn();
				CellRendererText cellrt5 = new CellRendererText();
				col_apm_Paciente.Title = "Apellido Materno";
				col_apm_Paciente.PackStart(cellrt5, true);
				col_apm_Paciente.AddAttribute (cellrt5, "text", 5); // la siguiente columna será 5 en vez de 6
				col_apm_Paciente.SortColumnId = (int) Column.col_apm_Paciente;
      
				TreeViewColumn col_fechanacimiento_Paciente = new TreeViewColumn();
				CellRendererText cellrt6 = new CellRendererText();
				col_fechanacimiento_Paciente.Title = "Fecha Nacimiento";
				col_fechanacimiento_Paciente.PackStart(cellrt6, true);
				col_fechanacimiento_Paciente.AddAttribute (cellrt6, "text", 6);     // la siguiente columna será 6 en vez de 7
				col_fechanacimiento_Paciente.SortColumnId = (int) Column.col_fechanacimiento_Paciente;
            
				TreeViewColumn col_edad_Paciente = new TreeViewColumn();
				CellRendererText cellrt7 = new CellRendererText();
				col_edad_Paciente.Title = "Edad";
				col_edad_Paciente.PackStart(cellrt7, true);
				col_edad_Paciente.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 7 en vez de 8
				col_edad_Paciente.SortColumnId = (int) Column.col_edad_Paciente;
            
				TreeViewColumn col_sexo_Paciente = new TreeViewColumn();
				CellRendererText cellrt8 = new CellRendererText();
				col_sexo_Paciente.Title = "Sexo";
				col_sexo_Paciente.PackStart(cellrt8, true);
				col_sexo_Paciente.AddAttribute (cellrt8, "text", 8); // la siguiente columna será 8 en vez de 9
				col_sexo_Paciente.SortColumnId = (int) Column.col_sexo_Paciente;
                        
				TreeViewColumn col_creacion_Paciente = new TreeViewColumn();
				CellRendererText cellrt9 = new CellRendererText();
				col_creacion_Paciente.Title = "Fecha creacion";
				col_creacion_Paciente.PackStart(cellrt9, true);
				col_creacion_Paciente.AddAttribute (cellrt9, "text", 9); // la siguiente columna será 8 en vez de 9
				col_creacion_Paciente.SortColumnId = (int) Column.col_creacion_Paciente;

				lista_de_Pacientes.AppendColumn(col_foliodeatencion);
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
			
		enum Column
		{
			col_foliodeatencion,
			col_PidPaciente,
			col_Nombre1_Paciente,
			col_Nombre2_Paciente,
			col_app_Paciente,
			col_apm_Paciente,
			col_fechanacimiento_Paciente,
			col_edad_Paciente,
			col_sexo_Paciente,
			col_creacion_Paciente
		}
		
		enum Column_prod
		{
			col_idproducto,
			col_desc_producto,
			col_grupoprod,
			col_grupo1prod,
			col_grupo2prod
		}
		
		enum Colum_cargos_extras
		{
			col_seleccion,
			col_cantidad,
			col_codigo_prod,
			col_descripcion,
			col_quien_cargo,
			col_fecha_hora,
			col_asignado,
			col_lugarpro,
			col_precio,
			col_ppor_cantidad,
			col_iva,
			col_sub_total,
			col_por_desc,
			col_valor_desc,
			col_total,
			col_costounitario,
			col_porceutilidad,
			col_costoproducto
		}
		
		// activa busqueda con boton busqueda de paciente
		// y llena la lista con los pacientes
		
		void on_buscar_paciente_clicked (object sender, EventArgs args)
		{
			treeViewEngineBusca.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	if ((string) entry_expresion.Text.ToString() == ""){
					comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, "+
							" nombre1_paciente,nombre2_paciente, apellido_paterno_paciente, "+
							"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,descripcion_diagnostico_movcargos, "+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
							"sexo_paciente,to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion, descripcion_admisiones,descripcion_diagnostico_movcargos "+
							"FROM osiris_his_paciente,osiris_erp_cobros_enca,osiris_his_tipo_admisiones,osiris_erp_movcargos "+
							"WHERE pagado = false "+
							"AND cerrado = 'false' "+
							"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
							//"AND osiris_erp_movcargos.id_tipo_admisiones = "+idtipointernamiento.ToString()+" "+
							"AND osiris_his_paciente.pid_paciente = osiris_erp_movcargos.pid_paciente "+
							"AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_movcargos.folio_de_servicio "+
							"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
							"AND osiris_erp_cobros_enca.alta_paciente = false "+
							"AND osiris_erp_cobros_enca.cancelado = false "+
							"ORDER BY folio_de_servicio;";
				}else{              	
					if (radiobutton_busca_apellido.Active == true){
						comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, "+
								" nombre1_paciente,nombre2_paciente, apellido_paterno_paciente, "+
								"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,descripcion_diagnostico_movcargos, "+
								"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
								"sexo_paciente,to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion, descripcion_admisiones,descripcion_diagnostico_movcargos "+
								"FROM osiris_his_paciente,osiris_erp_cobros_enca,osiris_his_tipo_admisiones,osiris_erp_movcargos "+
								"WHERE pagado = false "+
								"AND cerrado = 'false' "+
								"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
								//"AND osiris_erp_movcargos.id_tipo_admisiones = "+idtipointernamiento.ToString()+" "+
								"AND osiris_his_paciente.pid_paciente = osiris_erp_movcargos.pid_paciente "+
								"AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_movcargos.folio_de_servicio "+
								"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
								"AND osiris_erp_cobros_enca.alta_paciente = false "+
								"AND osiris_erp_cobros_enca.cancelado = false "+
								"AND apellido_paterno_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' ORDER BY folio_de_servicio;";
					}
					if (radiobutton_busca_nombre.Active == true){
						comando.CommandText =  "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, "+
								" nombre1_paciente,nombre2_paciente, apellido_paterno_paciente, "+
								"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,descripcion_diagnostico_movcargos, "+
								"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
								"sexo_paciente,to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion, descripcion_admisiones,descripcion_diagnostico_movcargos "+
								"FROM osiris_his_paciente,osiris_erp_cobros_enca,osiris_his_tipo_admisiones,osiris_erp_movcargos "+
								"WHERE pagado = false "+
								"AND cerrado = 'false' "+
								"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
								//"AND osiris_erp_movcargos.id_tipo_admisiones = "+idtipointernamiento.ToString()+" "+
								"AND osiris_his_paciente.pid_paciente = osiris_erp_movcargos.pid_paciente "+
								"AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_movcargos.folio_de_servicio "+
								"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
								"AND osiris_erp_cobros_enca.alta_paciente = false "+
								"AND osiris_erp_cobros_enca.cancelado = false "+
								"AND nombre1_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' ORDER BY folio_de_servicio;";
					}
					if (radiobutton_busca_expediente.Active == true){
						comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, "+
								"nombre1_paciente,nombre2_paciente, apellido_paterno_paciente, "+
								"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,descripcion_diagnostico_movcargos, "+
								"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
								"sexo_paciente,to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion, descripcion_admisiones,descripcion_diagnostico_movcargos "+
								"FROM osiris_his_paciente,osiris_erp_cobros_enca,osiris_his_tipo_admisiones,osiris_erp_movcargos "+
								"WHERE pagado = false "+
								"AND cerrado = 'false' "+
								"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
								//"AND osiris_erp_movcargos.id_tipo_admisiones = "+idtipointernamiento.ToString()+" "+
								"AND osiris_his_paciente.pid_paciente = osiris_erp_movcargos.pid_paciente "+
								"AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_movcargos.folio_de_servicio "+
								"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
								"AND osiris_erp_cobros_enca.alta_paciente = false "+
								"AND osiris_erp_cobros_enca.cancelado = false "+
								"AND osiris_his_paciente.pid_paciente = '"+entry_expresion.Text+"' ORDER BY folio_de_servicio;";			
					}
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					treeViewEngineBusca.AppendValues ((int) lector["folio_de_servicio"],//TreeIter iter =
										(int) lector["pid_paciente"],
										(string) lector["nombre1_paciente"],(string) lector["nombre2_paciente"],
										(string) lector["apellido_paterno_paciente"], (string) lector["apellido_materno_paciente"],
										(string) lector["fech_nacimiento"], (string) lector["edad"],
										(string) lector["sexo_paciente"],
										(string) lector["fech_creacion"]);
				}				
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			}
			conexion.Close ();
		}
		
		void on_selecciona_paciente_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;

 			if (lista_de_Pacientes.Selection.GetSelected(out model, out iterSelected)){
 				 folioservicio = (int) model.GetValue(iterSelected, 0);
 				 entry_folio_servicio.Text = folioservicio.ToString();
 				 llenado_de_productos_aplicados(folioservicio.ToString());
 			}
 			// cierra la ventana despues que almaceno la informacion en variables
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
 		}
 		
 		// llena la lista de productos
 		void on_llena_lista_producto_clicked (object sender, EventArgs args)
 		{
 			llenando_lista_de_productos();
 		}
 		void llenando_lista_de_productos()
 		{
 			//Console.WriteLine("busco");
 			treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			string precio_a_tomar = "";    // en esta variable dejo el precio que va tomar para los direfentes clientes
			
			//// para las diferentes listas de precios \\\\\\\\\\\\\			
			if (id_tipopaciente == 500 || id_tipopaciente == 102) {  // Municipio y Empresas			
				// verifica si ese cliente tiene una lista de precio asignada
				if (this.aplica_precios_empresas == true || aplica_precios_aseguradoras == true){     
					precio_a_tomar = "precio_producto_"+id_tipopaciente.ToString().Trim()+idempresa_paciente.ToString().Trim();
					//precio_a_tomar = "precio_producto_publico1";
				}else{
					precio_a_tomar = "precio_producto_publico";
				}
			}else{				
				if (id_tipopaciente == 400 ){ // Aseguradora
					precio_a_tomar = "precio_producto_"+id_tipopaciente.ToString().Trim()+idaseguradora_paciente.ToString().Trim();
				
					if (this.aplica_precios_empresas == true || aplica_precios_aseguradoras == true){    
						precio_a_tomar = "precio_producto_"+id_tipopaciente.ToString().Trim()+this.idaseguradora_paciente.ToString().Trim();
						//precio_a_tomar = "precio_producto_publico1";
					}else{
						precio_a_tomar = "precio_producto_publico";
					}
				}else{
					precio_a_tomar = "precio_producto_publico";
				}
			}
						
			//Console.WriteLine(precio_a_tomar);
			//////////////////////////////////
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			//conexion = new NpgsqlConnection (connectionString+"Database=hscmty");
			
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
						"osiris_productos.descripcion_producto,"+
						"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
						//"to_char(precio_producto_publico1,'99999999.99') AS preciopublico1,"+
						"to_char("+precio_a_tomar+",'99999999.99') AS preciopublico_cliente,"+
						"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,"+
						"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,"+
						"to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
						"to_char(porcentage_ganancia,'99999.99') AS porcentageutilidad,"+
						"to_char(costo_producto,'999999999.99') AS costoproducto "+
						"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
						"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
						"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
						"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
						"AND osiris_grupo_producto.agrupacion3 = 'IMG' "+
						"AND osiris_productos.cobro_activo = 'true' "+
						"AND osiris_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_producto;";
		
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				float tomaprecio;
				float calculodeiva;
				float preciomasiva;
				float tomadescue;
				float preciocondesc;
							
				while (lector.Read())
				{
					calculodeiva = 0;
					preciomasiva = 0;
					
					///////////////////////////////////////////////////////////
					// ---- nuevo para las multiples listas de precio					
					if (float.Parse((string) lector["preciopublico_cliente"]) > 0){
							tomaprecio = float.Parse((string) lector["preciopublico_cliente"]);
						}else{
							tomaprecio = float.Parse((string) lector["preciopublico"]);
					}					
					////////////////****************************	
					
					tomadescue = float.Parse((string) lector["porcentagesdesc"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					preciocondesc = tomaprecio;
					if ((bool) lector["aplicar_iva"])
					{
						calculodeiva = (tomaprecio * valoriva)/100;
					}
					if ((bool) lector["aplica_descuento"])
					{
						preciocondesc = tomaprecio-((tomaprecio*tomadescue)/100);
					}
					preciomasiva = tomaprecio + calculodeiva; 
					treeViewEngineBusca2.AppendValues (//TreeIter iter = 
											(string) lector["codProducto"],//0
											(string) lector["descripcion_producto"],//1
											(string) lector["descripcion_grupo_producto"],//2
											(string) lector["descripcion_grupo1_producto"],//3
											(string) lector["descripcion_grupo2_producto"],//4
											tomaprecio.ToString("F").PadLeft(10),//2-5
											calculodeiva.ToString("F").PadLeft(10),//3-6
											preciomasiva.ToString("F").PadLeft(10),//4-7
											(string) lector["porcentagesdesc"],//8
											preciocondesc.ToString("F").PadLeft(10),//9
											(string) lector["costoproductounitario"],//10
											(string) lector["porcentageutilidad"],//11
											(string) lector["costoproducto"]);//12
					
				}
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			}
			conexion.Close ();
		}

 		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			TreeModel model;	TreeIter iterSelected;
			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)) 
 			{
				id_produ = (string) model.GetValue(iterSelected, 0);
				desc_produ = (string) model.GetValue(iterSelected, 1);
				if(desc_produ.Length >  68) { desc_produ = desc_produ.Substring(0,68); }
				precio_produ = (string) model.GetValue(iterSelected, 5);
				iva_produ = (string) model.GetValue(iterSelected, 6);
				total_produ = (string) model.GetValue(iterSelected, 7);
				descuent_produ = (string) model.GetValue(iterSelected, 8);
				pre_con_desc_produ = (string) model.GetValue(iterSelected, 9);
				//valor_descuento = float.Parse(this.precio_produ)-float.Parse(this.pre_con_desc_produ);
				costo_unitario_producto = (string) model.GetValue(iterSelected, 10); 
				porcentage_utilidad_producto = (string) model.GetValue(iterSelected, 11);
				costo_total_producto = (string) model.GetValue(iterSelected, 12);
				entry_desc_producto.Text = desc_produ;
				string constante = entry_cantidad_aplicada.Text;
				string fechahora_soli = entry_fecha_solicitud.Text+" "+entry_hora_solicitud.Text;
				string folio_lab = entry_folio_laboratorio.Text; 
				//varibles numericas
				ppcantidad = float.Parse(precio_produ)*float.Parse(constante);
				float ivaproducto = float.Parse(iva_produ)*float.Parse(constante);
				float suma_total = ppcantidad+ivaproducto;
				float preciocondesc = float.Parse(pre_con_desc_produ)*float.Parse(constante);
				valor_descuento = ppcantidad-preciocondesc;
				//float costotprodu = preciocondesc;
				
				if ((string) constante != ""){
					if ((float) float.Parse(constante) > 0){
						if ((int) idlugarprocedencia >= 20){
							if(int.Parse(folio_lab) > 0){
								if ((string) entry_desc_producto.Text.Trim() == ""){}else{
									Item foo;
									foo = new Item (true,
												float.Parse(constante),
												this.id_produ,
												this.desc_produ,
												LoginEmpleado,
												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
												descripinternamiento,
												descriplugarprocedencia,//
												this.precio_produ.PadLeft(10),
												ppcantidad.ToString(),
												ivaproducto.ToString("F").PadLeft(10),
												suma_total.ToString("F").PadLeft(10),
												this.descuent_produ.PadLeft(10),
												this.valor_descuento.ToString("F").PadLeft(10),
												preciocondesc.ToString("F").PadLeft(10),
												idlugarprocedencia,//
												idtipointernamiento,
												costo_unitario_producto,
												porcentage_utilidad_producto,
												costo_total_producto,
												fechahora_soli,
												folio_lab);
									arraycargosextras.Add(foo);
									treeViewEngineExtras.AppendValues (true,//0
														float.Parse(constante),//1
														this.id_produ,//2
														this.desc_produ,//3
														LoginEmpleado,//4
														DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),//5
														descripinternamiento,//6
														descriplugarprocedencia,//7
														this.precio_produ.PadLeft(10),//8
														ppcantidad.ToString(),//9
														ivaproducto.ToString("F").PadLeft(10),//10
														suma_total.ToString("F").PadLeft(10),//11
														this.descuent_produ.PadLeft(10),//12
														this.valor_descuento.ToString("F").PadLeft(10),//13
														preciocondesc.ToString("F").PadLeft(10),//14
														idlugarprocedencia,//15
														idtipointernamiento,//16
														costo_unitario_producto,//17
														porcentage_utilidad_producto,//18
														costo_total_producto,
														fechahora_soli,
														folio_lab);
									this.entry_cantidad_aplicada.Text = "0"; this.entry_folio_laboratorio.Text = "0";
								}
							}else{
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, ButtonsType.Close, "Seleccione el un Folio de departamento \n"+"valido");
								msgBoxError.Run ();	msgBoxError.Destroy();
							}								
						}else{
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "Seleccione el lugar o departamento donde \n"+"se pidio el estudio para el paciente");
							msgBoxError.Run ();	msgBoxError.Destroy();
						}
					}else{
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, ButtonsType.Close, "La cantidad que quiere aplicar debe ser \n"+"mayor que cero, intente de nuevo");
						msgBoxError.Run ();		msgBoxError.Destroy();
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, ButtonsType.Close, "La cantidad que quiere aplicar NO \n"+"puede quedar vacia, intente de nuevo");
					msgBoxError.Run ();		msgBoxError.Destroy();
				}
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
		
		// Valida entradas que solo sean numericas, se utiliza en ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_folio(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				llenado_de_productos_aplicados( (string) entry_folio_servicio.Text );				
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
			{
				args.RetVal = true;
			}
		}

		struct Item
 		{
 			public bool col_seleccion{
				get { return col0_car_extr; }
				set { col0_car_extr = value; }
			}
			public float col_cantidad{
				get { return col1_car_extr; }
				set { col1_car_extr = value; }
			}
			public string col_codigo_prod{
				get { return col2_car_extr; }
				set { col2_car_extr = value; }
			}
			public string col_descripcion{
				get { return col3_car_extr; }
				set { col3_car_extr = value; }
			}
			public string col_quien_cargo{
				get { return col4_car_extr; }
				set { col4_car_extr = value; }
			}
			public string col_fecha_hora{
				get { return col5_car_extr; }
				set { col5_car_extr = value; }
			}
			public string col_asignado{
				get { return col6_car_extr; }
				set { col6_car_extr = value; }
			}
			////////////////////////////////////
			public string col_lugarpro{
				get { return col7_car_extr; }
				set { col7_car_extr = value; }
			}
			///////////////////////
			public string col_precio{
				get { return col8_car_extr; }
				set { col8_car_extr = value; }
			}
			public string col_ppor_cantidad{
				get { return col9_car_extr; }
				set { col9_car_extr = value; }
			}	
			public string col_iva{
				get { return col10_car_extr; }
				set { col10_car_extr = value; }
			}
			public string col_sub_total{
				get { return col11_car_extr; }
				set { col11_car_extr = value; }
			}
			public string col_por_desc{
				get { return col12_car_extr; }
				set { col12_car_extr = value; }
			}
			public string col_valor_desc{
				get { return col13_car_extr; }//12
				set { col14_car_extr = value; }
			}
			public string col_total{
				get { return col14_car_extr; }//13
				set { col14_car_extr = value; }
			}
			////////////////////////////
			public int col_idlugarpro{
				get { return col15_car_extr; }
				set { col15_car_extr = value; }
			}
			//////////////////////
			public int col_idasignado{
				get { return col16_car_extr; }//14
				set { col16_car_extr = value; }
			}
			public string col_costounitario{
				get { return col17_car_extr; }//15
				set { col17_car_extr = value; }
			}
			public string col_porceutilidad{
				get { return col18_car_extr; }//16
				set { col18_car_extr = value; }
			}
			public string col_costoproducto{
				get { return col19_car_extr; }//17
				set { col19_car_extr = value; }
			}
			
			public string col_fechahora_solicitud{
				get { return col20_car_extr; }
				set { col20_car_extr = value; }
			}
			
			public string col_folio_interno_dep{
				get { return col21_car_extr; }
				set { col21_car_extr = value; }
			}
			
			private bool col0_car_extr;
			private float col1_car_extr;
			private string col2_car_extr;
			private string col3_car_extr;
			private string col4_car_extr;
			private string col5_car_extr;
			private string col6_car_extr;
			private string col7_car_extr;
			private string col8_car_extr;
			private string col9_car_extr;
			private string col10_car_extr;
			private string col11_car_extr;
			private string col12_car_extr;
			private string col13_car_extr;
			private string col14_car_extr;
			private int col15_car_extr;//14
			private int col16_car_extr;//
			private string col17_car_extr;//15
			private string col18_car_extr;//16
			private string col19_car_extr;//17
			private string col20_car_extr;
			private string col21_car_extr;

			public Item (bool col0_car_extr,float col1_car_extr,string col2_car_extr,string col3_car_extr,
					string col4_car_extr,string col5_car_extr,string col6_car_extr,string col7_car_extr,
					string col8_car_extr,string col9_car_extr,string col10_car_extr,string col11_car_extr,
					string col12_car_extr,string col13_car_extr,string col14_car_extr,int col15_car_extr,
					int col16_car_extr,string col17_car_extr,string col18_car_extr,string col19_car_extr, 
					string col20_car_extr,string col21_car_extr )
			{
				this.col0_car_extr = col0_car_extr;
				this.col1_car_extr = col1_car_extr;
				this.col2_car_extr = col2_car_extr;
				this.col3_car_extr = col3_car_extr;
				this.col4_car_extr = col4_car_extr;
				this.col5_car_extr = col5_car_extr;
				this.col6_car_extr = col6_car_extr;
				this.col7_car_extr = col7_car_extr;
				this.col8_car_extr = col8_car_extr;
				this.col9_car_extr = col9_car_extr;
				this.col10_car_extr = col10_car_extr;
				this.col11_car_extr = col11_car_extr;
				this.col12_car_extr = col12_car_extr;
				this.col13_car_extr = col13_car_extr;
				this.col14_car_extr = col14_car_extr;
				this.col15_car_extr = col15_car_extr;
				this.col16_car_extr = col16_car_extr;
				this.col17_car_extr = col17_car_extr;
				this.col18_car_extr = col18_car_extr;
				this.col19_car_extr = col19_car_extr;
				this.col20_car_extr = col20_car_extr;
				this.col21_car_extr = col21_car_extr; 
			}
 		}
 		
		private void on_button_removerItem_clicked (object o, EventArgs args)
		{
 			TreeIter iter;		TreeModel model;
			if (lista_cargos_extras.Selection.GetSelected (out model, out iter)) {
 				int position = treeViewEngineExtras.GetPath (iter).Indices[0];
 				treeViewEngineExtras.Remove (ref iter);
				arraycargosextras.RemoveAt (position);
			}
		}
		
		void NumberCellEdited (object o, EditedArgs args)
		{
			TreePath path = new TreePath (args.Path);
 			TreeIter iter;
 			treeViewEngineExtras.GetIter (out iter, path);
			int i = path.Indices[0];
			float precio_linea;
			float precioprod;
			float iva_linea;
			float total1_linea;
			float valor_descuento;
			float precio_con_desc;
			Item foo;
			try {
				foo = (Item) arraycargosextras[i];
 				foo.col_cantidad = float.Parse(args.NewText,System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo("es-MX"));
 				precio_linea = float.Parse(foo.col_precio) * float.Parse(args.NewText);
 				precioprod = float.Parse(foo.col_precio);
 				
 				valor_descuento = ( precioprod * float.Parse(foo.col_por_desc)/100) * float.Parse(args.NewText);
 				precio_con_desc = (precioprod-(precioprod * float.Parse(foo.col_por_desc)/100)) * float.Parse(args.NewText);
 				
 				if (float.Parse(foo.col_iva) > 0){
 					iva_linea = float.Parse(foo.col_iva) * float.Parse(args.NewText);
 				}else{
 					iva_linea = float.Parse(foo.col_iva);
 				}
 				total1_linea = precio_linea + iva_linea; 
 				
			} catch (Exception e) {
				return;
			}
 			treeViewEngineExtras.SetValue (iter, (int) Colum_cargos_extras.col_cantidad, foo.col_cantidad);
 			treeViewEngineExtras.SetValue (iter, (int) Colum_cargos_extras.col_ppor_cantidad,precio_linea.ToString("F"));
 			treeViewEngineExtras.SetValue (iter, (int) Colum_cargos_extras.col_iva,iva_linea.ToString("F"));
 			treeViewEngineExtras.SetValue (iter, (int) Colum_cargos_extras.col_sub_total,total1_linea.ToString("F"));
 			treeViewEngineExtras.SetValue (iter, (int) Colum_cargos_extras.col_valor_desc,valor_descuento.ToString("F"));
 			treeViewEngineExtras.SetValue (iter, (int) Colum_cargos_extras.col_total,precio_con_desc.ToString("F"));
		}
		
		void on_button_resultados_clicked(object sender, EventArgs args)
		{
			if ((int) folioservicio > 0){ 
				TreeIter iter; TreeModel model;
				if (lista_de_servicios.Selection.GetSelected (out model, out iter)){
					new osiris.validacion_resultados_imagenologia(true,LoginEmpleado,NomEmpleados,
											(string)lista_de_servicios.Model.GetValue (iter,2),		// Id del producto seleccionado
											(string) lista_de_servicios.Model.GetValue (iter,20),    // id de la secuencia
											nombrebd,dir_pac,this.entry_edad.Text.Trim(),empresapac,
											PidPaciente,entry_nombre_paciente.Text.Trim(),folioservicio,
											this.entry_fecha_nacimiento.Text.Trim(),entry_tipo_paciente.Text,
											(string) lista_de_servicios.Model.GetValue (iter,21), 	// Fecha de solicitud de examen
											(string) lista_de_servicios.Model.GetValue (iter,22),	// numero de folio del departamento
											sexopaciente,
											(string) lista_de_servicios.Model.GetValue (iter,6),
											this.entry_doctor.Text.Trim(),
											(string) lista_de_servicios.Model.GetValue (iter,0));
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close, "Seleccione un estudio para ver sus parametros");
					msgBoxError.Run ();	msgBoxError.Destroy();
				}
			}else{
	  			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close, "NO selecciono un folio");
				msgBoxError.Run ();	msgBoxError.Destroy();
			}
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
	
	
	////////////////////////////////////////////////////////////
	///CLASE PUBLICA PARA RESULTADOS
	///////////////////////////////////////////////////////////
	public class validacion_resultados_imagenologia
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando ventana de Resultados
		[Widget] Gtk.Window validacion_estudios;
		[Widget] Gtk.Button button_guardar;
		[Widget] Gtk.ComboBox combobox_radiologo_aut;
		[Widget] Gtk.ComboBox combobox_tipo_examen;
		[Widget] Gtk.Entry entry_fecha_solicitud_res;
		[Widget] Gtk.Entry entry_hora_solicitud_res;
		[Widget] Gtk.Entry entry_folio_laboratorio_res;
		[Widget] Gtk.Entry entry_observaciones;
		[Widget] Gtk.Entry entry_fecha_validado;
		[Widget] Gtk.Statusbar statusbar_reslab;
		
		public string LoginEmpleado;
		public string NomEmpleados;
		public string connectionString;
		public string nombrebd;
		
		public int folioservicio = 0;							// Toma el valor de numero de atencion de paciente
		public int PidPaciente = 0;								// Toma la actualizacion del pid del paciente
		public string edadpac;
		public string fecha_nacimiento;
		public string nombpaciente = "";
		public string tipo_paciente = "";
		public string dir_pac;
		public string empresapac = "";
		public string observaciones = "";
		public string idsecuencia = "";
		public string id_produ = "";
		public string tipoexamen = "";
		public string quimicoaut = "";
		public string idquimico = "";
		public string cedulaquimico = "";
		public string numerosolicitud;
		public string fechasolicitud;
		public string sexopaciente;
		public string procedencia;
		public string medicotratante;
		public string nombre_estudio;
		public bool resultados_editables = false;
		public bool actualizaresultado = false;
		
		public Gtk.ListStore treeViewEngineresultados;
		public Gtk.CellRendererText cellr0;
		public Gtk.CellRendererText cellr1;
		public Gtk.CellRendererText cellr2;
		public Gtk.CellRendererToggle cellr5;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
			
		public validacion_resultados_imagenologia(bool _resultados_editables_,string _LoginEmpleado_,string _NomEmpleados_,string _id_produ_,
							string _idsecuencia_,string _nombrebd_,string _dir_pac_,string _edadpac_,string _empresapac_,
							int _PidPaciente_,string _entry_nombre_paciente_,int _folioservicio_,
							string _fecha_nacimiento_,string _entry_tipo_paciente_,
							string _fechasolicitud_,string _numerosolicitud_,string _sexopaciente_,string _procedencia_,
							string _medicotratante_,string _nombre_estudio_)
		{
			//Guardo valores a variables
			resultados_editables = _resultados_editables_;
			LoginEmpleado = _LoginEmpleado_;
			NomEmpleados = _NomEmpleados_;
			id_produ = _id_produ_;
			idsecuencia = _idsecuencia_;
			nombrebd = _nombrebd_;
			dir_pac = _dir_pac_; 
			edadpac = _edadpac_;
			empresapac = _empresapac_;
			PidPaciente = _PidPaciente_;
			nombpaciente = _entry_nombre_paciente_;
			folioservicio = _folioservicio_;
			fecha_nacimiento = _fecha_nacimiento_; 
			tipo_paciente = _entry_tipo_paciente_;
			numerosolicitud = _numerosolicitud_;
			fechasolicitud = _fechasolicitud_;
			sexopaciente = _sexopaciente_ ;
			procedencia = _procedencia_;
			medicotratante = _medicotratante_;
			nombre_estudio = _nombre_estudio_;
			///////////////////ventana de resultados de examenes/////////////////
			
			//valores y subprogramas de la ventana
			Glade.XML gxml = new Glade.XML (null, "imagenologia.glade", "validacion_estudios", null);
			gxml.Autoconnect (this);		validacion_estudios.Show();
			if(this.resultados_editables == false) {
				button_guardar.Sensitive = false;
				combobox_radiologo_aut.Sensitive = false;
				combobox_tipo_examen.Sensitive = false;
			}
						
			entry_fecha_solicitud_res.Text = fechasolicitud.Substring(0,10);
			entry_hora_solicitud_res.Text = fechasolicitud.Substring(11,5);			
			entry_folio_laboratorio_res.Text = numerosolicitud;
			
			if(numerosolicitud.Trim() == "0"){
				entry_fecha_solicitud_res.Text = DateTime.Now.ToString("yyyy-MM-dd");
				entry_hora_solicitud_res.Text = DateTime.Now.ToString("HH:mm");			
				entry_folio_laboratorio_res.Text = "";
				entry_fecha_solicitud_res.IsEditable = true;
				entry_hora_solicitud_res.IsEditable = true;			
				entry_folio_laboratorio_res.IsEditable = true;
			}
			
			//SE LLENA EL COMBO BOX	
			combobox_radiologo_aut.Clear();
			CellRendererText cell1 = new CellRendererText();
			combobox_radiologo_aut.PackStart(cell1, true);
			combobox_radiologo_aut.AddAttribute(cell1,"text",0);
	        
			ListStore store1 = new ListStore( typeof (string), typeof (string),typeof(string));
			combobox_radiologo_aut.Model = store1;
			
			if (LoginEmpleado == "DOLIVARES" ){			
				store1.AppendValues ("Seleccione un Radiologo","0","");
				store1.AppendValues ("RADIOLOGO "+NomEmpleados+",2370632","1");
			}else{
				store1.AppendValues ("RADIOLOGO "+NomEmpleados+",2370632","");
			}
            
            TreeIter iter1;
			if (store1.GetIterFirst(out iter1)) {
				combobox_radiologo_aut.SetActiveIter (iter1);//Console.WriteLine(iter1);
			}
			combobox_radiologo_aut.Changed += new EventHandler (on_comboboxChanged_quimicos_aut);
			
			
			combobox_tipo_examen.Clear();
			CellRendererText cell2 = new CellRendererText();
			combobox_tipo_examen.PackStart(cell2, true);
			combobox_tipo_examen.AddAttribute(cell2,"text",0);
							
			ListStore store2 = new ListStore( typeof (string));
			combobox_tipo_examen.Model = store2;
	        store2.AppendValues ("Seleccione tipo de Examen ");
	        store2.AppendValues ("ORDINARIO");
	        store2.AppendValues ("URGENTE");
	        store2.AppendValues ("URGENTE/FUERA DE HORARIO");
         
            TreeIter iter2;
			if (store2.GetIterFirst(out iter2)) {combobox_tipo_examen.SetActiveIter (iter2);	}
			combobox_tipo_examen.Changed += new EventHandler (on_comboboxChanged_tipo_examen);
						
			button_guardar.Clicked += new EventHandler(on_button_guardar_resultado_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); 
			
			entry_folio_laboratorio_res.ModifyBase(StateType.Normal, new Gdk.Color(166,220,255));
			
			statusbar_reslab.Pop(0);
			statusbar_reslab.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleados);
			statusbar_reslab.HasResizeGrip = false;
		}
		
		enum Column_resu {	col_parametro,	col_resultado,	col_vr,	}
					
		// Valida entradas se utiliza en ventana de resultados
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_fechahora_solicitud(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));	Console.WriteLine(args.Event.Key);
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（ﾭ￢:";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace){
				args.RetVal = true;
			}
		}
		
		void on_comboboxChanged_quimicos_aut(object sender, EventArgs args)
		{
	    	ComboBox combobox_quimicos_aut = sender as ComboBox;
			if (sender == null){return; }
			TreeIter iter;
	  		if (combobox_quimicos_aut.GetActiveIter (out iter))	{
	    		quimicoaut = (string) combobox_quimicos_aut.Model.GetValue(iter,0);
	    		cedulaquimico = (string) combobox_quimicos_aut.Model.GetValue(iter,1);	//Console.WriteLine(quimicoaut+", "+idquimico+", "+cedulaquimico);
	    		idquimico = (string) combobox_quimicos_aut.Model.GetValue(iter,2);
     		}
		}
		
		void on_comboboxChanged_tipo_examen(object sender, EventArgs args)
		{
			ComboBox combobox_tipo_examen = sender as ComboBox;
			if(sender == null) { return;}
			TreeIter iter;
			if(combobox_tipo_examen.GetActiveIter (out iter)) {
				tipoexamen = (string) combobox_tipo_examen.Model.GetValue(iter,0);
			}
		}
		
		void on_button_guardar_resultado_clicked(object sender, EventArgs args)
		{
			bool camposcompletos = Verificacion_de_valores();
			TreeIter iter;
	  		if (camposcompletos == true){	  								
				// Valida que no tenga numero de folio para laboratorio
				// esto significa que el cargo se realizo desde el modulo de caja
				if(numerosolicitud.Trim() == "0"){
				
					NpgsqlConnection conexion1;
					conexion1 = new NpgsqlConnection (connectionString+nombrebd);
		        	// Verifica que la base de datos este conectada
					try{
						conexion1.Open ();
						NpgsqlCommand comando1; 		comando1 = conexion1.CreateCommand();
						comando1.CommandText = "UPDATE osiris_erp_cobros_deta "+
										"SET folio_interno_dep = '"+this.entry_folio_laboratorio_res.Text.Trim()+"',"+
										"fechahora_solicitud = '"+this.entry_fecha_solicitud_res.Text.Trim()+" "+this.entry_hora_solicitud_res.Text.Trim()+":00' "+
										"WHERE id_secuencia = '"+idsecuencia.Trim()+"' ;";
						//Console.WriteLine(comando1.CommandText);
						comando1.ExecuteNonQuery(); 					comando1.Dispose();
						numerosolicitud = entry_folio_laboratorio_res.Text;
					}catch (NpgsqlException ex){
							//Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();	msgBoxError.Destroy();
					}
					conexion1.Close ();
				}
				actualizaresultado = true;
				this.validacion_estudios.Destroy();
			}else{
	    		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Info,ButtonsType.Close,"Seleccione un Radiologo o verifique que los campos \n de fecha, hora y folio no se encuentren en blanco");
				msgBoxError.Run ();	msgBoxError.Destroy();
	    	}
		}
		
		public bool Verificacion_de_valores()
		{
			bool valor;
			if(entry_fecha_solicitud_res.Text.Trim() == "" || entry_folio_laboratorio_res.Text.Trim() == ""  ||
				entry_hora_solicitud_res.Text.Trim() == "" || idquimico == ""){
				valor = false;
				return valor;
			}else{
				valor = true;
				return valor;
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

