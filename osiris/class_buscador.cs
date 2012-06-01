/// <summary>
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
// Autor    	: Daniel Olivares Cuevas - arcangeldoc@gmail.com (Programacion Mono)
//		  		  Daniel Olivares Cuevas - arcangeldoc@gmail.com (Diseño de Pantallas Glade)
// 				  
// Licencia		: GPL
// S.O. 		: GNU/Linux
//
// proyect Facturador is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// proyect is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Foobar; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Coment
//
// This module is copy the Project Sistema Hospitalario OSIRIS
//
/// </summary>

using Gtk;
using Gdk;
using System;
using Glade;
using Npgsql;

namespace osiris
{		
	public class class_buscador
	{		
		//Declarando ventanas de busqueda
		[Widget] Gtk.Window buscador = null;
		[Widget] Gtk.RadioButton radiobutton1  = null;
		[Widget] Gtk.RadioButton radiobutton2  = null;
		[Widget] Gtk.RadioButton radiobutton3  = null;
		[Widget] Gtk.Entry entry_expresion = null;
		[Widget] Gtk.Button button_buscar_busqueda = null;
		[Widget] Gtk.Button button_selecciona = null;
		[Widget] Gtk.TreeView lista_de_busqueda = null;
		[Widget] Gtk.Button button_salir = null;
		[Widget] Gtk.ComboBox combobox_busqueda = null;
		[Widget] Gtk.Label labelbusqueda1 = null;
		[Widget] Gtk.Label labelbusqueda2 = null;
		[Widget] Gtk.Label labelcantidad = null;
		[Widget] Gtk.Entry entry_cantidad_producto = null;
								
		TreeStore treeViewEngineBuscador;
		ListStore treeViewEngine;
		Gtk.TreeView treeviewobject;		
		
		// Busqueda de Clientes para los reportes
		Gtk.Entry entry_nombre_cliente = null;
		Gtk.Entry entry_id_cliente = null;
		
		// Busqueda de Estados y Regiones para los catalogos
		Gtk.Entry entry_id_estado = null;
		Gtk.Entry entry_estado = null;
		Gtk.ToggleButton togglebutton_editar_estado = null;
		
		Gtk.Entry entry_id_municipio = null; 
		Gtk.Entry entry_municipio = null;
		Gtk.Frame frame2 = null;		
		Gtk.ToggleButton togglebutton_editar_municipio = null;
		Gtk.Button button_guardar_municipio = null; 
		
		// Busqueda de Grupo de Grupo de Productos 0
		Gtk.Entry entry_id_grupo = null;
		Gtk.Entry entry_descripcion_grupo = null;
		Gtk.ToggleButton togglebutton_editar_grupo = null;
		Gtk.Button button_guardar_grupo = null;
		Gtk.CheckButton checkbutton_activar_grupo = null;
		Gtk.Entry entry_porcentage_utilidad = null;
		Gtk.Entry entry_id_centrocosto = null;
		Gtk.Entry entry_descripcion_centrocosto = null;
		Gtk.Button button_buscar_centrocosto = null;
		
		// Busqueda de Grupo1 o Familia1 de Productos
		Gtk.Entry entry_id_grupo1 = null;
		Gtk.Entry entry_descripcion_grupo1 = null;
		Gtk.ToggleButton togglebutton_editar_grupo1 = null;
		Gtk.Button button_guardar_grupo1 = null;
		Gtk.Button button_buscar_grupo1 = null;
		Gtk.CheckButton checkbutton_activar_grupo1 = null;
		
		// Busqueda de Grupo2 o Familia1 de Productos
		Gtk.Entry entry_id_grupo2 = null;
		Gtk.Entry entry_descripcion_grupo2 = null;
		Gtk.ToggleButton togglebutton_editar_grupo2 = null;
		Gtk.Button button_guardar_grupo2 = null;
		Gtk.Button button_buscar_grupo2 = null;
		Gtk.CheckButton checkbutton_activar_grupo2 = null;
		
		// Busqueda de Marca de Productos
		Gtk.Entry entry_idmarca_producto = null;
		Gtk.Entry entry_descripcion_marca_producto = null;
		
		// Busqueda de Proveedores
		Gtk.Entry entry_id_proveedor = null;
		Gtk.Entry entry_nombre_proveedor = null;
		Gtk.Entry entry_formapago = null;
		Gtk.Entry entry_direccion_proveedor = null;
		Gtk.Entry entry_tel_proveedor = null;
		Gtk.Entry entry_contacto_proveedor = null;
				
		// Busqueda de Especialidades Medicas
		Gtk.Entry entry_id_especialidad = null;
		Gtk.Entry entry_especialidad = null;
		
		// Busqueda de Empresas-enlase por tipo de pacientes
		Gtk.Entry entry_id_empaseg_cita = null;
		Gtk.Entry entry_nombre_empaseg_cita = null;
		
		// Busqueda doctor cita de paciente
		Gtk.Entry entry_id_doctor_cita = null;
		Gtk.Entry entry_nombre_doctor_cita = null;
		
		// Busqueda doctor cita de paciente
		Gtk.Entry entry_id_doctor_consulta = null;
		Gtk.Entry entry_nombre_doctor_consulta = null;
		
		// Busqueda Especilidad en cita
		Gtk.Entry entry_id_especialidad_cita = null;
		Gtk.Entry entry_nombre_especialidad_cita = null;
		
		// Busqueda Especilidad en cita
		Gtk.Entry entry_id_especialidad_consulta = null;
		Gtk.Entry entry_nombre_especialidad_consulta = null;
		
		//Busqueda de paciente_cita
		Gtk.Entry entry_pid_paciente_cita = null;
		Gtk.Entry entry_nombre_paciente_cita1 = null;
		Gtk.Entry entry_fecha_nac_cita = null;
		Gtk.Entry entry_edad_paciente_cita = null;
		
		// Busqueda de Cirugia y/o Paquete quirirugico
		Gtk.Entry entry_id_cirugia = null;
		Gtk.Entry entry_cirugia = null;
		Gtk.CheckButton checkbutton_paquete_sino = null;		
		
		// Busqueda de Pacientes con numero de Atencion o Evolucion
		Gtk.Entry entry_folio_servicio = null;
		Gtk.Entry entry_pid_paciente = null;
		Gtk.Entry entry_nombre_paciente = null;
		
		// Busqueda de almacen en iventario fisico
		Gtk.Entry entry_id_almacen = null;
		Gtk.Entry entry_almacen = null;
		
		class_conexion conexion_a_DB = new class_conexion();
				
		string[] args_sql;
		string type_find = "";
		string order_sql = "";
		string comodin = "";
		int typeseek = 0;
		string string_sql="";
		string connectionString;
		string nombrebd;
		
		//declaracion de columnas y celdas de treeview de busqueda
		TreeViewColumn col_buscador0;	CellRendererText cellrt0;
		TreeViewColumn col_buscador1;	CellRendererText cellrt1;
		TreeViewColumn col_buscador2; 	CellRendererToggle cellrt2;
		TreeViewColumn col_buscador3;	CellRendererText cellrt3;
		TreeViewColumn col_buscador4;	CellRendererText cellrt4;
		TreeViewColumn col_buscador5;	CellRendererText cellrt5;
		TreeViewColumn col_buscador6;	CellRendererText cellrt6;
			
		//Declaracion de ventana de error y mensaje
		protected Gtk.Window MyWinError;
		
		public void buscandor(object[] args, string[] args_sql_, string type_find_,string order_sql_,string comodin_,int typeseek_)		
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;	
			Glade.XML gxml = new Glade.XML (null, "osiris.glade", "buscador", null);
			gxml.Autoconnect(this);
			buscador.Title = "Buscador "+type_find_;
	        //Muestra ventana de Glade
			buscador.Show();
			radiobutton1.Hide();
			radiobutton2.Hide();
			radiobutton3.Hide();
			combobox_busqueda.Hide();
			labelbusqueda1.Hide();
			labelbusqueda2.Hide();
			labelcantidad.Hide();
			entry_cantidad_producto.Hide();
			crea_treeview_busqueda();
									
			//Console.WriteLine("nº de argumentos: {0}", args.Length);
			//for (int i = 0; i < args.Length; i++)
        	//Console.WriteLine("args[{0}] = {1} {2}", i, args[i],@args[i]);  
			switch (type_find_){	
				case "find_client":
					entry_id_cliente = (object) args[0] as Gtk.Entry;
					entry_nombre_cliente = (object) args[1] as Gtk.Entry;				
				break;		
				case "find_estado_region":
					entry_id_estado = (object) args[0] as Gtk.Entry;
					entry_estado = (object) args[1] as Gtk.Entry;
					togglebutton_editar_estado = (object) args[2] as Gtk.ToggleButton;
					frame2 = (object) args[3] as Gtk.Frame;
					togglebutton_editar_municipio = (object) args[4] as Gtk.ToggleButton;
					button_guardar_municipio = (object) args[5] as Gtk.Button;
					entry_municipio = (object) args[6] as Gtk.Entry;
					entry_id_municipio = (object) args[7] as Gtk.Entry;
				break;			
				case "find_municipio":
					entry_id_municipio = (object) args[0] as Gtk.Entry;
					entry_municipio = (object) args[1] as Gtk.Entry;
					togglebutton_editar_municipio = (object) args[2] as Gtk.ToggleButton;
					button_guardar_municipio = (object) args[3] as Gtk.Button;
				break;				
				case "find_grupo_producto":
					entry_id_grupo = (object) args[0] as Gtk.Entry;
					entry_descripcion_grupo = (object) args[1] as Gtk.Entry;
					togglebutton_editar_grupo = (object) args[2] as Gtk.ToggleButton;
					button_guardar_grupo = (object) args[3] as Gtk.Button;
					checkbutton_activar_grupo  = (object) args[4] as Gtk.CheckButton;
					entry_porcentage_utilidad = (object) args[5] as Gtk.Entry;
					entry_id_centrocosto = (object) args[6] as Gtk.Entry;
					entry_descripcion_centrocosto = (object) args[7] as Gtk.Entry;
					button_buscar_centrocosto  = (object) args[8] as Gtk.Button;
				break;				
				case "find_grupo1_producto":
					entry_id_grupo1 = (object) args[0] as Gtk.Entry;
					entry_descripcion_grupo1 = (object) args[1] as Gtk.Entry;
					togglebutton_editar_grupo1 = (object) args[2] as Gtk.ToggleButton;
					button_guardar_grupo1 = (object) args[3] as Gtk.Button;
					checkbutton_activar_grupo1  = (object) args[4] as Gtk.CheckButton;				
				break;				
				case "find_grupo2_producto":
					entry_id_grupo2 = (object) args[0] as Gtk.Entry;
					entry_descripcion_grupo2 = (object) args[1] as Gtk.Entry;
					togglebutton_editar_grupo2 = (object) args[2] as Gtk.ToggleButton;
					button_guardar_grupo2 = (object) args[3] as Gtk.Button;
					checkbutton_activar_grupo2  = (object) args[4] as Gtk.CheckButton;				
				break;				
				case "find_centrodecosto":
					entry_id_centrocosto = (object) args[0] as Gtk.Entry;
					entry_descripcion_centrocosto = (object) args[1] as Gtk.Entry;
				break;							
				case "find_marca_producto":
					entry_idmarca_producto = (object) args[0] as Gtk.Entry;
					entry_descripcion_marca_producto = (object) args[1] as Gtk.Entry;
				break;				
				case "find_proveedores":
					entry_id_proveedor = (object) args[0] as Gtk.Entry;
					entry_nombre_proveedor = (object) args[1] as Gtk.Entry;
					entry_formapago = (object) args[2] as Gtk.Entry;
				break;				
				case "find_proveedores_catalogo_producto":
					entry_id_proveedor = (object) args[0] as Gtk.Entry;
					entry_nombre_proveedor = (object) args[1] as Gtk.Entry;
				break;				
				case "find_especialidad_medica":
					entry_id_especialidad = (object) args[0] as Gtk.Entry;
					entry_especialidad = (object) args[1] as Gtk.Entry;
				break;				
				case "find_empresa_cita":
					entry_id_empaseg_cita = (object) args[0] as Gtk.Entry;
					entry_nombre_empaseg_cita = (object) args[1] as Gtk.Entry;
				break;				
				case "find_aseguradoras_cita":
					entry_id_empaseg_cita = (object) args[0] as Gtk.Entry;
					entry_nombre_empaseg_cita = (object) args[1] as Gtk.Entry;
				break;
				case "find_medico_cita":
					entry_id_doctor_cita = (object) args[0] as Gtk.Entry;
					entry_nombre_doctor_cita = (object) args[1] as Gtk.Entry;
				break;
				case "find_medico_consulta":
					entry_id_doctor_consulta = (object) args[0] as Gtk.Entry;
					entry_nombre_doctor_consulta = (object) args[1] as Gtk.Entry;
				break;
				case "find_especialidad_cita":
					entry_id_especialidad_cita = (object) args[0] as Gtk.Entry;
					entry_nombre_especialidad_cita = (object) args[1] as Gtk.Entry;					
				break;
				case "find_especialidad_consulta":
					entry_id_especialidad_consulta = (object) args[0] as Gtk.Entry;
					entry_nombre_especialidad_consulta = (object) args[1] as Gtk.Entry;					
				break;
				case "find_paciente_cita":
					entry_pid_paciente_cita = (object) args[0] as Gtk.Entry;
					entry_nombre_paciente_cita1 = (object) args[1] as Gtk.Entry;
					entry_fecha_nac_cita = (object) args[2] as Gtk.Entry;
					//entry_edad_paciente_cita = (object) args[3] as Gtk.Entry;
					radiobutton1.Show();
					radiobutton2.Show();
					radiobutton3.Show();
					labelbusqueda1.Show();
					radiobutton1.Label = "por Ape. Paterno";
					radiobutton2.Label = "por un Nombre";
					radiobutton3.Label = "por Expediente";
					col_buscador1.Title = "Primer Nombre";
					col_buscador3.Title = "Segundo Nombre";
					col_buscador4.Title = "Apellido Paterno";
					col_buscador5.Title = "Apellido Materno";
					lista_de_busqueda.AppendColumn(col_buscador3);
					lista_de_busqueda.AppendColumn(col_buscador4);
					lista_de_busqueda.AppendColumn(col_buscador5);
				break;
				case "find_cirugia_paquetes":
					entry_id_cirugia = (object) args[0] as Gtk.Entry;
					entry_cirugia = (object) args[1] as Gtk.Entry;					
					checkbutton_paquete_sino  = (object) args[2] as Gtk.CheckButton;
					col_buscador2.Title = "Paquete";
					lista_de_busqueda.AppendColumn(col_buscador2);
				break;
				case "find_paciente":
					entry_folio_servicio = (object) args[0] as Gtk.Entry;
					entry_pid_paciente = (object) args[1] as Gtk.Entry;
					entry_nombre_paciente = (object) args[2] as Gtk.Entry;
					radiobutton1.Show();
					radiobutton2.Show();
					radiobutton3.Show();
					labelbusqueda1.Show();
					radiobutton1.Label = "por Ape. Paterno";
					radiobutton2.Label = "por un Nombre";
					radiobutton3.Label = "por Expediente";
					col_buscador0.Title = "N° Atencion";
					col_buscador1.Title = "N° Expediente";
					col_buscador3.Title = "Primer Nombre";
					col_buscador4.Title = "Segundo Nombre";
					col_buscador5.Title = "Apellido Paterno";
					col_buscador6.Title = "Apellido Materno";
					lista_de_busqueda.AppendColumn(col_buscador3);
					lista_de_busqueda.AppendColumn(col_buscador4);
					lista_de_busqueda.AppendColumn(col_buscador5);
					lista_de_busqueda.AppendColumn(col_buscador6);
				break;
				case "find_paciente1":
					entry_pid_paciente = (object) args[0] as Gtk.Entry;
					entry_nombre_paciente = (object) args[1] as Gtk.Entry;
					//entry_edad_paciente_cita = (object) args[3] as Gtk.Entry;
					radiobutton1.Show();
					radiobutton2.Show();
					radiobutton3.Show();
					labelbusqueda1.Show();
					radiobutton1.Label = "por Ape. Paterno";
					radiobutton2.Label = "por un Nombre";
					radiobutton3.Label = "por Expediente";
					col_buscador1.Title = "Primer Nombre";
					col_buscador3.Title = "Segundo Nombre";
					col_buscador4.Title = "Apellido Paterno";
					col_buscador5.Title = "Apellido Materno";
					lista_de_busqueda.AppendColumn(col_buscador3);
					lista_de_busqueda.AppendColumn(col_buscador4);
					lista_de_busqueda.AppendColumn(col_buscador5);
				break;
				case "find_proveedores_OC":
					entry_id_proveedor = (object) args[0] as Gtk.Entry;
					entry_nombre_proveedor = (object) args[1] as Gtk.Entry;
					entry_direccion_proveedor  = (object) args[2] as Gtk.Entry;
					entry_tel_proveedor  = (object) args[3] as Gtk.Entry;
					entry_contacto_proveedor  = (object) args[4] as Gtk.Entry;
					entry_formapago  = (object) args[5] as Gtk.Entry;
				break;
				case "find_almacen_inventario":
					entry_id_almacen = (object) args[0] as Gtk.Entry;
					entry_almacen = (object) args[1] as Gtk.Entry;
				break;
				case "find_cirugia_paquetes_soliprod":
					entry_id_cirugia = (object) args[0] as Gtk.Entry;
					entry_cirugia = (object) args[1] as Gtk.Entry;
					treeviewobject = (object) args[2] as Gtk.TreeView;
					treeViewEngine = (object) args[3] as Gtk.ListStore;
				break;
					case "find_cirugia_cargos_modmedicos":
				break;
			}
			args_sql = args_sql_;
			type_find = type_find_;
			order_sql = order_sql_;
			comodin = comodin_;
			string_sql = (string) args_sql[1];
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
	  		button_selecciona.Clicked += new EventHandler(on_selecciona_busqueda);
			button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
			radiobutton1.Clicked += new EventHandler(on_radiobutton_clicked);
			radiobutton2.Clicked += new EventHandler(on_radiobutton_clicked);
			radiobutton3.Clicked += new EventHandler(on_radiobutton_clicked);			
		}
		
		void on_radiobutton_clicked(object sender, EventArgs args)
		{
			Gtk.RadioButton radiobutton_typeseek = (Gtk.RadioButton) sender;
			if(radiobutton_typeseek.Name.ToString() == "radiobutton1"){
				if(radiobutton1.Active == true){
					string_sql = (string) args_sql[1];
					comodin = "%' ";					
				}
			}
			if(radiobutton_typeseek.Name.ToString() == "radiobutton2"){
				if(radiobutton2.Active == true){
					string_sql = (string) args_sql[2];
					comodin = "%' ";
				}
			}
			if(radiobutton_typeseek.Name.ToString() == "radiobutton3"){
				if(radiobutton3.Active == true){
					string_sql = (string) args_sql[3];
					comodin = "' ";
				}
			}
		}
		
		void crea_treeview_busqueda()
		{
			treeViewEngineBuscador = new TreeStore(typeof(int),//0
													typeof(string),//1
													typeof(bool),//2
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
													typeof(bool),// 15
			                                        typeof(string), // 16
			                                        typeof(string), // 17
			                                        typeof(string)); // 18
												
			lista_de_busqueda.Model = treeViewEngineBuscador;
			
			lista_de_busqueda.RulesHint = true;
							
			lista_de_busqueda.RowActivated += on_selecciona_busqueda;  // Doble click selecciono cliente*/
			col_buscador0 = new TreeViewColumn();
			cellrt0 = new CellRendererText();
			col_buscador0.Title = "ID"; // titulo de la cabecera de la columna, si está visible
			col_buscador0.PackStart(cellrt0, true);
			col_buscador0.AddAttribute (cellrt0, "text", 0);
			//col_idcliente.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_buscador0.SortColumnId = (int) col_treview.col_buscador0;
			col_buscador0.Resizable = true;
			
			col_buscador1 = new TreeViewColumn();
			cellrt1 = new CellRendererText();
			col_buscador1.Title = "Descripcion";
			col_buscador1.PackStart(cellrt1, true);
			col_buscador1.AddAttribute (cellrt1, "text", 1);
			//col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_buscador1.SortColumnId = (int) col_treview.col_buscador1;
			col_buscador1.Resizable = true;
			
			col_buscador2 = new TreeViewColumn();
			cellrt2 = new CellRendererToggle();
			col_buscador2.Title = "Descripcion";
			col_buscador2.PackStart(cellrt2, true);
			col_buscador2.AddAttribute (cellrt2, "active", 2);
			//col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_buscador2.SortColumnId = (int) col_treview.col_buscador2;
			col_buscador2.Resizable = true;
			
			col_buscador3 = new TreeViewColumn();
			cellrt3 = new CellRendererText();
			col_buscador3.Title = "";
			col_buscador3.PackStart(cellrt3, true);
			col_buscador3.AddAttribute (cellrt3, "text", 3);
			//col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_buscador3.SortColumnId = (int) col_treview.col_buscador3;
			col_buscador3.Resizable = true;
			
			col_buscador4 = new TreeViewColumn();
			cellrt4 = new CellRendererText();
			col_buscador4.Title = "";
			col_buscador4.PackStart(cellrt4, true);
			col_buscador4.AddAttribute (cellrt4, "text", 4);
			//col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_buscador4.SortColumnId = (int) col_treview.col_buscador4;
			col_buscador4.Resizable = true;
			
			col_buscador5 = new TreeViewColumn();
			cellrt5 = new CellRendererText();
			col_buscador5.Title = "";
			col_buscador5.PackStart(cellrt5, true);
			col_buscador5.AddAttribute (cellrt5, "text", 5);
			//col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_buscador5.SortColumnId = (int) col_treview.col_buscador5;
			col_buscador5.Resizable = true;
			
			col_buscador6 = new TreeViewColumn();
			cellrt6 = new CellRendererText();
			col_buscador6.Title = "";
			col_buscador6.PackStart(cellrt6, true);
			col_buscador6.AddAttribute (cellrt6, "text", 6);
			//col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_buscador6.SortColumnId = (int) col_treview.col_buscador6;
			col_buscador6.Resizable = true;
				            
			lista_de_busqueda.AppendColumn(col_buscador0);
			lista_de_busqueda.AppendColumn(col_buscador1);			
		}
		
		enum col_treview
		{
			col_buscador0,col_buscador1,col_buscador2,col_buscador3,col_buscador4,col_buscador5,col_buscador6
		}
		
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((bool) this.lista_de_busqueda.Model.GetValue (iter,2) == false){
					(cell as Gtk.CellRendererText).Foreground = "red";
				}else{		
					(cell as Gtk.CellRendererText).Foreground = "black";
				}
			}
		
		void on_llena_lista(object sender, EventArgs args)
		{
			llenando_lista_de_lista();
		}
		
		void llenando_lista_de_lista()
		{
			if((string) entry_expresion.Text.Trim() !=""){
				string connectionString = conexion_a_DB._url_servidor+
										conexion_a_DB._port_DB+
										conexion_a_DB._usuario_DB+
										conexion_a_DB._passwrd_user_DB;
				string nombrebd = conexion_a_DB._nombrebd;
									
				treeViewEngineBuscador.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
            	// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					if ((string) entry_expresion.Text.ToUpper() == "*"){
						comando.CommandText = (string) args_sql[0]+order_sql;
					}else{
						if(typeseek==0){
							comando.CommandText = string_sql+(string) entry_expresion.Text.ToUpper()+comodin+order_sql;
						}
						if(typeseek==1){
							comando.CommandText = string_sql+(string) entry_expresion.Text.ToUpper()+comodin+order_sql;
						}
					}
					Console.WriteLine(comando.CommandText);
					NpgsqlDataReader lector = comando.ExecuteReader ();				
					while (lector.Read()){
						switch (type_find){	
							case "find_client":
								treeViewEngineBuscador.AppendValues ((int) lector["id_cliente"],//0
													(string) lector["descripcion_cliente"]);
							break;			
							case "find_estado_region":
								treeViewEngineBuscador.AppendValues ((int) lector["id_estado"],//0
													(string) lector["descripcion_estado"]);//1
							break;			
							case "find_municipio":
								treeViewEngineBuscador.AppendValues( (int) lector["id_municipio"],
														(string) lector["descripcion_municipio"]);
							break;							
							case "find_grupo_producto":
								treeViewEngineBuscador.AppendValues( (int) lector["id_grupo_producto"],
														(string) lector["descripcion_grupo_producto"],
							                            (bool) lector["activo_gp"],
							                            (string) Convert.ToString(lector["porcentage_utilidad_grupo"]),
							                            (string) Convert.ToString(lector["idcentrodecosto"]),
							                            (string) lector["descripcion_centro_de_costo"]);
							
								col_buscador0.SetCellDataFunc(cellrt0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_buscador1.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							break;							
							case "find_grupo1_producto":
								treeViewEngineBuscador.AppendValues( (int) lector["id_grupo1_producto"],
														(string) lector["descripcion_grupo1_producto"],
							                            (bool) lector["activo"]);
							
								col_buscador0.SetCellDataFunc(cellrt0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_buscador1.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							break;							
							case "find_grupo2_producto":
								treeViewEngineBuscador.AppendValues( (int) lector["id_grupo2_producto"],
														(string) lector["descripcion_grupo2_producto"],
							                            (bool) lector["activo"]);
							
								col_buscador0.SetCellDataFunc(cellrt0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_buscador1.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							break;							
							case "find_centrodecosto":
								treeViewEngineBuscador.AppendValues( (int) lector["id_centro_de_costos"],
														(string) lector["descripcion_centro_de_costo"]);
							
							break;							
							case "find_marca_producto":
								treeViewEngineBuscador.AppendValues ((int) lector["id_marca_producto"], //0
													(string) lector["descripcion"]);//1
							break;							
							case "find_proveedores":
								treeViewEngineBuscador.AppendValues ((int) lector["id_proveedor"],//0
													(string) lector["descripcion_proveedor"],//1
							                        (bool) lector["proveedor_activo"], //, // 2
													(string) lector["direccion_proveedor"],//3
													(string) lector["colonia_proveedor"],//4
													(string) lector["municipio_proveedor"],//5
													(string) lector["estado_proveedor"],//6
													(string) lector["telefono1_proveedor"],//7
													(string) lector["contacto1_proveedor"],//8
													(string) lector["rfc_proveedor"],//9
													(string) lector["pagina_web_proveedor"],//10
													(string) lector["descripcion_forma_de_pago"]);//11
													//(string) lector["fax_proveedor"], //12
							                        //(int) lector["id_forma_de_pago"]);//13
							break;							
							case "find_proveedores_catalogo_producto":
								treeViewEngineBuscador.AppendValues ((int) lector["id_proveedor"],//0
													(string) lector["descripcion_proveedor"],//1
							                        (bool) lector["proveedor_activo"], //, // 2
													(string) lector["direccion_proveedor"],//3
													(string) lector["colonia_proveedor"],//4
													(string) lector["municipio_proveedor"],//5
													(string) lector["estado_proveedor"],//6
													(string) lector["telefono1_proveedor"],//7
													(string) lector["contacto1_proveedor"],//8
													(string) lector["rfc_proveedor"],//9
													(string) lector["pagina_web_proveedor"],//10
													(string) lector["descripcion_forma_de_pago"]);//11
													//(string) lector["fax_proveedor"], //12
							                        //(int) lector["id_forma_de_pago"]);//13
							break;							
							case "find_especialidad_medica":
								treeViewEngineBuscador.AppendValues ((int) lector["id_especialidad"],	// 0
													(string) lector["descripcion_especialidad"]);		// 1
								
							break;
							case "find_empresa_cita":
								treeViewEngineBuscador.AppendValues ((int) lector["id_empresa"],	// 0
													(string)lector["descripcion_empresa"]);		// 1
							break;
							case "find_aseguradoras_cita":
								treeViewEngineBuscador.AppendValues ((int) lector["id_aseguradora"],	// 0
													(string)lector["descripcion_aseguradora"]);		// 1
							break;
							case "find_medico_cita":
								treeViewEngineBuscador.AppendValues ((int) lector["id_medico"],	// 0
													(string)lector["nombre_medico"]);		// 1
							break;
							case "find_medico_consulta":
								treeViewEngineBuscador.AppendValues ((int) lector["id_medico"],	// 0
													(string)lector["nombre_medico"]);		// 1
							break;
							case "find_especialidad_cita":
								treeViewEngineBuscador.AppendValues ((int) lector["id_especialidad"],	// 0
													(string)lector["descripcion_especialidad"]);		// 1
							break;
							case "find_especialidad_consulta":
								treeViewEngineBuscador.AppendValues ((int) lector["id_especialidad"],	// 0
													(string)lector["descripcion_especialidad"]);		// 1
							break;
							case "find_paciente_cita":
								treeViewEngineBuscador.AppendValues ((int) lector["pid_paciente"],	// 0
													(string) lector["nombre1_paciente"].ToString().Trim(),
							                        (bool) lector["activo"],
							                        (string) lector["nombre2_paciente"].ToString().Trim(),
							                        (string) lector["apellido_paterno_paciente"].ToString().Trim(),
							                        (string) lector["apellido_materno_paciente"].ToString().Trim(),
							                        (string) lector["fech_nacimiento"],
							              			(string) lector["edad"]);
							break;
							case "find_cirugia_paquetes":
								treeViewEngineBuscador.AppendValues ((int) lector["id_tipo_cirugia"],
							                                   	(string) lector["descripcion_cirugia"],
							                                     (bool) lector["tiene_paquete"]);
							break;
							case "find_cirugia_paquetes_soliprod":
								treeViewEngineBuscador.AppendValues ((int) lector["id_tipo_cirugia"],
							                                   	(string) lector["descripcion_cirugia"],
							                                     (bool) lector["tiene_paquete"]);
							break;
							case "find_cirugia_cargos_modmedicos":
									treeViewEngineBuscador.AppendValues ((int) lector["id_tipo_cirugia"],
							                                   	(string) lector["descripcion_cirugia"],
							                                     (bool) lector["tiene_paquete"]);
							break;							
							case "find_paciente":
								treeViewEngineBuscador.AppendValues ((int) lector["folio_de_servicio"],	// 0
													(string) lector["pidpaciente"].ToString().Trim(),
							                        (bool) lector["activo"],
							                        (string) lector["nombre1_paciente"].ToString().Trim(),
							                        (string) lector["nombre2_paciente"].ToString().Trim(),
							                        (string) lector["apellido_paterno_paciente"].ToString().Trim(),
							                        (string) lector["apellido_materno_paciente"].ToString().Trim(),
							                        (string) lector["fech_nacimiento"],
							              			(string) lector["edad"]);
							break;
							case "find_paciente1":
								treeViewEngineBuscador.AppendValues ((int) lector["pid_paciente"],	// 0
													(string) lector["nombre1_paciente"].ToString().Trim(),
							                        (bool) lector["activo"],
							                        (string) lector["nombre2_paciente"].ToString().Trim(),
							                        (string) lector["apellido_paterno_paciente"].ToString().Trim(),
							                        (string) lector["apellido_materno_paciente"].ToString().Trim(),
							                        (string) lector["fech_nacimiento"],
							              			(string) lector["edad"]);
							break;
							case "find_proveedores_OC":
								treeViewEngineBuscador.AppendValues ((int) lector["id_proveedor"],//0
													(string) lector["descripcion_proveedor"],//1
							                        (bool) lector["proveedor_activo"], //, // 2
													(string) lector["direccion_proveedor"],//3
													(string) lector["colonia_proveedor"],//4
													(string) lector["municipio_proveedor"],//5
													(string) lector["estado_proveedor"],//6
													(string) lector["telefono1_proveedor"],//7
													(string) lector["contacto1_proveedor"],//8
													(string) lector["rfc_proveedor"],//9
													(string) lector["pagina_web_proveedor"],//10
													(string) lector["descripcion_forma_de_pago"]);//11
													//(string) lector["fax_proveedor"], //12
							                        //(int) lector["id_forma_de_pago"]);//13
							break;
							case "find_almacen_inventario":
								treeViewEngineBuscador.AppendValues((int) lector["id_almacen"],
							                                  (string) lector["descripcion_almacen"]);
							break;
							
						}
					}
				}catch (NpgsqlException ex){
	   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
				conexion.Close ();
			}
		}
		
		void on_selecciona_busqueda(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;			
			if (lista_de_busqueda.Selection.GetSelected(out model, out iterSelected)){										
				int tomaid = (int) model.GetValue(iterSelected, 0);												
				switch (type_find){	
					case "find_client":
						entry_id_cliente.Text = tomaid.ToString();
						entry_nombre_cliente.Text = (string) model.GetValue(iterSelected, 1);
					break;
					
					case "find_estado_region":
						entry_id_estado.Text = tomaid.ToString();
						entry_estado.Text = (string) model.GetValue(iterSelected, 1);
						frame2.Sensitive = true;
						togglebutton_editar_estado.Sensitive = true;
						togglebutton_editar_estado.Active = false;
						togglebutton_editar_municipio.Sensitive = false;
						togglebutton_editar_municipio.Active = false;
						button_guardar_municipio.Sensitive = false;
						entry_municipio.Sensitive = false;
						entry_municipio.Text = "";
						entry_id_municipio.Text = "";
					break;
					
					case "find_municipio":
						entry_id_municipio.Text = tomaid.ToString();
						entry_municipio.Text = (string) model.GetValue(iterSelected, 1);
						togglebutton_editar_municipio.Sensitive = true;
						togglebutton_editar_municipio.Active = false;
						button_guardar_municipio.Sensitive = false;
					break;	
					
					case "find_grupo_producto":
						entry_id_grupo.Text = tomaid.ToString();
						entry_descripcion_grupo.Text = (string) model.GetValue(iterSelected, 1);
						checkbutton_activar_grupo.Active = (bool) model.GetValue(iterSelected, 2);
						entry_porcentage_utilidad.Text = (string) model.GetValue(iterSelected, 3);
						entry_id_centrocosto.Text = (string) model.GetValue(iterSelected, 4);
						entry_descripcion_centrocosto.Text = (string) model.GetValue(iterSelected, 5);
						button_guardar_grupo.Sensitive = false;
						togglebutton_editar_grupo.Sensitive = true;
						togglebutton_editar_grupo.Active = false;
						checkbutton_activar_grupo.Sensitive = false;
						entry_porcentage_utilidad.Sensitive = false;
						entry_id_centrocosto.Sensitive = false;
						entry_descripcion_centrocosto.Sensitive = false;
						button_buscar_centrocosto.Sensitive = false;
					break;
					
					case "find_grupo1_producto":
						entry_id_grupo1.Text = tomaid.ToString();
						entry_descripcion_grupo1.Text = (string) model.GetValue(iterSelected, 1);
						checkbutton_activar_grupo1.Active = (bool) model.GetValue(iterSelected, 2);						
						button_guardar_grupo1.Sensitive = false;
						togglebutton_editar_grupo1.Sensitive = true;
						togglebutton_editar_grupo1.Active = false;
						checkbutton_activar_grupo1.Sensitive = false;
					break;
					
					case "find_grupo2_producto":
						entry_id_grupo2.Text = tomaid.ToString();
						entry_descripcion_grupo2.Text = (string) model.GetValue(iterSelected, 1);
						checkbutton_activar_grupo2.Active = (bool) model.GetValue(iterSelected, 2);						
						button_guardar_grupo2.Sensitive = false;
						togglebutton_editar_grupo2.Sensitive = true;
						togglebutton_editar_grupo2.Active = false;
						checkbutton_activar_grupo2.Sensitive = false;
					break;					
					case "find_centrodecosto":
						entry_id_centrocosto.Text = tomaid.ToString();
						entry_descripcion_centrocosto.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_marca_producto":
						entry_idmarca_producto.Text = tomaid.ToString();
						entry_descripcion_marca_producto.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_proveedores":
						entry_id_proveedor.Text = tomaid.ToString();
						entry_nombre_proveedor.Text = (string) model.GetValue(iterSelected, 1);
						entry_formapago.Text = (string) model.GetValue(iterSelected, 11);
					break;
					case "find_proveedores_catalogo_producto":
						entry_id_proveedor.Text = tomaid.ToString();
						entry_nombre_proveedor.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_especialidad_medica":
						entry_id_especialidad.Text = tomaid.ToString();
						entry_especialidad.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_empresa_cita":
						entry_id_empaseg_cita.Text = tomaid.ToString();
						entry_nombre_empaseg_cita.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_aseguradoras_cita":
						entry_id_empaseg_cita.Text = tomaid.ToString();
						entry_nombre_empaseg_cita.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_medico_cita":
						entry_id_doctor_cita.Text = tomaid.ToString();
						entry_nombre_doctor_cita.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_medico_consulta":
						entry_id_doctor_consulta.Text = tomaid.ToString();
						entry_nombre_doctor_consulta.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_especialidad_cita":
						entry_id_especialidad_cita.Text = tomaid.ToString();
						entry_nombre_especialidad_cita.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_especialidad_consulta":
						entry_id_especialidad_consulta.Text = tomaid.ToString();
						entry_nombre_especialidad_consulta.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_paciente_cita":
						entry_pid_paciente_cita.Text = tomaid.ToString();
						entry_nombre_paciente_cita1.Text = (string) model.GetValue(iterSelected, 1)+" "+
															(string) model.GetValue(iterSelected, 3)+" "+
															(string) model.GetValue(iterSelected, 4)+" "+
															(string) model.GetValue(iterSelected, 5);
						entry_fecha_nac_cita.Text = (string) model.GetValue(iterSelected, 6);
						//entry_edad_paciente_cita.Text = (string) model.GetValue(iterSelected, 4);
					break;
					case "find_cirugia_paquetes":
						entry_id_cirugia.Text = tomaid.ToString();
						entry_cirugia.Text = (string) model.GetValue(iterSelected, 1);
						checkbutton_paquete_sino.Active = (bool) model.GetValue(iterSelected, 2);
					break;
					case "find_cirugia_paquetes_soliprod":
						entry_id_cirugia.Text = tomaid.ToString();
						entry_cirugia.Text = (string) model.GetValue(iterSelected, 1);
						carga_valores_treeview(tomaid,treeviewobject,treeViewEngine);
					break;
					case "find_cirugia_cargos_modmedicos":
					
					break;
					case "find_paciente":
						entry_folio_servicio.Text = tomaid.ToString();
						entry_pid_paciente.Text = (string) model.GetValue(iterSelected, 1);
						entry_nombre_paciente.Text = (string) model.GetValue(iterSelected, 3)+" "+
															(string) model.GetValue(iterSelected, 4)+" "+
															(string) model.GetValue(iterSelected, 5)+" "+
															(string) model.GetValue(iterSelected, 6);
					break;
					case "find_paciente1":
						entry_pid_paciente.Text = tomaid.ToString();
						entry_nombre_paciente.Text = (string) model.GetValue(iterSelected, 1)+" "+
															(string) model.GetValue(iterSelected, 3)+" "+
															(string) model.GetValue(iterSelected, 4)+" "+
															(string) model.GetValue(iterSelected, 5);
						//entry_edad_paciente_cita.Text = (string) model.GetValue(iterSelected, 4);
					break;
					case "find_proveedores_OC":						
						entry_id_proveedor.Text = tomaid.ToString();
						entry_nombre_proveedor.Text = (string) model.GetValue(iterSelected, 1);
						entry_formapago.Text = (string) model.GetValue(iterSelected, 11);
						entry_direccion_proveedor.Text = (string) model.GetValue(iterSelected, 3)+" "+(string) model.GetValue(iterSelected, 4)+" "+
														(string) model.GetValue(iterSelected, 5)+ " " +(string) model.GetValue(iterSelected, 6);
						entry_tel_proveedor.Text = (string) model.GetValue(iterSelected, 7);
						entry_contacto_proveedor.Text =  ""; //(string) model.GetValue(iterSelected, 1);
					break;					
					case "find_almacen_inventario":
						entry_id_almacen.Text = tomaid.ToString();
						entry_almacen.Text = (string) model.GetValue(iterSelected, 1);
					break;
				}				
			}
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();			
		}
					
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		void onKeyPressEvent_enter(object sender, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;				
				llenando_lista_de_lista();
			}
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
			
		public void carga_valores_treeview(int idcode_find,object treeview_,object listotree_store_)
		{
			string query_sql_llenado_treeview = "SELECT descripcion_producto,osiris_his_tipo_admisiones.descripcion_admisiones, "+
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
							"AND id_grupo_producto IN('4','5') "+
							"AND osiris_his_cirugias_deta.eliminado = false "+ 
							"AND osiris_his_cirugias_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
							"AND osiris_his_cirugias_deta.id_tipo_cirugia = '"+idcode_find.ToString().Trim()+"' "+
						    "AND osiris_his_cirugias_deta.eliminado = 'false' "+
							"ORDER BY osiris_productos.descripcion_producto,to_char(osiris_his_cirugias_deta.fechahora_creacion,'yyyy-mm-dd HH:mm:ss');";
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = query_sql_llenado_treeview;
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					treeViewEngine.AppendValues((string) lector["descripcion_producto"],
														(string) lector["idproducto"],
														(string) lector["cantidadaplicada"],
														(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
														"",
														"",
														(string) lector["costoproductounitario"],
														(string) lector["preciopublico"],
														false,
														false,
														"");
					
				}
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();
				
		}
	}
}