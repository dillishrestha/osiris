// created on 17/07/2010
///////////////////////////////////////////////////////////
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	:  Ing. Daniel Olivares C. (Programacion Base y Ajustes)
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
/////////////////////////////////////////////////////////

using System;
using Npgsql;
using Gtk;
using Glade;
using Gdk;

namespace osiris
{
	public class analisis_devoluciones
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		// Declarando ventana principal
		[Widget] Gtk.Window resumen_devoluciones = null;
		[Widget] Gtk.Statusbar statusbar_analisisdevol = null;
		[Widget] Gtk.Entry entry_folio_servicio = null;
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_nombre_paciente = null;
		[Widget] Gtk.Button button_selecciona_folio = null;
		[Widget] Gtk.Button button_buscar_paciente = null;
		[Widget] Gtk.Button button_imprimir_mov = null;
				
		[Widget] Gtk.TreeView lista_cargos_desde_stock = null;
		[Widget] Gtk.TreeView lista_solicitado_no_cargado = null;
		[Widget] Gtk.TreeView lista_solicitados_y_cargados = null;
		
		string LoginEmpleado;
		string NomEmpleado; 
		string AppEmpleado; 
		string ApmEmpleado;
			
		string connectionString;
		string nombrebd;
		
		private TreeStore treeViewEngine1;
		private TreeStore treeViewEngine2;
		private TreeStore treeViewEngine3;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
			
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		public analisis_devoluciones(string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_ )
		{
			LoginEmpleado = LoginEmp;
			NomEmpleado = NomEmpleado_; 
			AppEmpleado = AppEmpleado_; 
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			//Console.WriteLine("antes de abrir venmtana");
			Glade.XML gxml = new Glade.XML (null,"almacen_costos_compras.glade","resumen_devoluciones",null);
			gxml.Autoconnect (this);
	        // Muestra ventana de Glade
			resumen_devoluciones.Show();
			statusbar_analisisdevol.Pop(0);
			statusbar_analisisdevol.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_analisisdevol.HasResizeGrip = false;
			
			entry_folio_servicio.ModifyBase(StateType.Normal, new Gdk.Color(54,180,221));
			// Validando que sen solo numeros
			entry_folio_servicio.KeyPressEvent += onKeyPressEvent_enter_folio;
			button_selecciona_folio.Clicked += new EventHandler(on_button_selecciona_folio_clicked);
			button_buscar_paciente.Clicked += new EventHandler(on_button_buscar_paciente_clicked);
			button_imprimir_mov.Clicked += new EventHandler(on_button_imprimir_mov_clicked);
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			crea_treeview_analisis();
		}
		
		void crea_treeview_analisis()
		{
			treeViewEngine1 = new TreeStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string));
			treeViewEngine2 = new TreeStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string));
			treeViewEngine3 = new TreeStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string));
			
			lista_cargos_desde_stock.Model = treeViewEngine1;
			lista_cargos_desde_stock.RulesHint = true;			
			lista_solicitado_no_cargado.Model = treeViewEngine2;
			lista_solicitado_no_cargado.RulesHint = true;
			lista_solicitados_y_cargados.Model = treeViewEngine3;
			lista_solicitados_y_cargados.RulesHint = true;
			
			TreeViewColumn col_idproducto = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idproducto.Title = "ID Producto";
			col_idproducto.PackStart(cellr0, true);
			col_idproducto.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
			
			TreeViewColumn col_desc_producto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_desc_producto.Title = "Descripcion de Producto";
			col_desc_producto.PackStart(cellr1, true);
			col_desc_producto.AddAttribute (cellr1, "text", 1);
			col_desc_producto.Resizable = true;
			cellr1.Width = 400;
			
			TreeViewColumn col_cargos = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_cargos.Title = "Tot. Cargado";
			col_cargos.PackStart(cellrt2, true);
			col_cargos.AddAttribute (cellrt2, "text", 2);
			//col_stock.SortColumnId = (int) Column_inv.col_stock;
			//col_stock.SetCellDataFunc(cellrt2,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_solicitado = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_solicitado.Title = "Solicitado";
			col_solicitado.PackStart(cellrt3, true);
			col_solicitado.AddAttribute (cellrt3, "text", 3);
			//col_stock.SortColumnId = (int) Column_inv.col_stock;
			//col_stock.SetCellDataFunc(cellrt2,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_devolucion = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_devolucion.Title = "Devolucion";
			col_devolucion.PackStart(cellrt4, true);
			col_devolucion.AddAttribute (cellrt4, "text",4 );
			
			TreeViewColumn col_subalmacen = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_subalmacen.Title = "Sub-Almacen";
			col_subalmacen.PackStart(cellrt5, true);
			col_subalmacen.AddAttribute (cellrt5, "text",5 );
			
			////////////////////////////////////////////////////////////////////
			TreeViewColumn col_idproducto2 = new TreeViewColumn();
			CellRendererText cellr02 = new CellRendererText();
			col_idproducto2.Title = "ID Producto";
			col_idproducto2.PackStart(cellr02, true);
			col_idproducto2.AddAttribute (cellr02, "text", 0);    // la siguiente columna será 1 en vez de 1
			
			TreeViewColumn col_desc_producto2 = new TreeViewColumn();
			CellRendererText cellr12 = new CellRendererText();
			col_desc_producto2.Title = "Descripcion de Producto";
			col_desc_producto2.PackStart(cellr12, true);
			col_desc_producto2.AddAttribute (cellr12, "text", 1);
			col_desc_producto2.Resizable = true;
			cellr1.Width = 400;
			
			TreeViewColumn col_cargos2 = new TreeViewColumn();
			CellRendererText cellrt22 = new CellRendererText();
			col_cargos2.Title = "Tot. Cargado";
			col_cargos2.PackStart(cellrt22, true);
			col_cargos2.AddAttribute (cellrt22, "text", 2);
			//col_stock.SortColumnId = (int) Column_inv.col_stock;
			//col_stock.SetCellDataFunc(cellrt2,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_solicitado2 = new TreeViewColumn();
			CellRendererText cellrt32 = new CellRendererText();
			col_solicitado2.Title = "Solicitado";
			col_solicitado2.PackStart(cellrt32, true);
			col_solicitado2.AddAttribute (cellrt32, "text", 3);
			//col_stock.SortColumnId = (int) Column_inv.col_stock;
			//col_stock.SetCellDataFunc(cellrt2,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_devolucion2 = new TreeViewColumn();
			CellRendererText cellrt42 = new CellRendererText();
			col_devolucion2.Title = "Devolucion";
			col_devolucion2.PackStart(cellrt42, true);
			col_devolucion2.AddAttribute (cellrt42, "text",4 );
			
			TreeViewColumn col_subalmacen2 = new TreeViewColumn();
			CellRendererText cellrt52 = new CellRendererText();
			col_subalmacen2.Title = "Sub-Almacen";
			col_subalmacen2.PackStart(cellrt52, true);
			col_subalmacen2.AddAttribute (cellrt52, "text",5 );
			
			///////////////////////////////////////////////////////////7
			TreeViewColumn col_idproducto3 = new TreeViewColumn();
			CellRendererText cellr03 = new CellRendererText();
			col_idproducto3.Title = "ID Producto";
			col_idproducto3.PackStart(cellr03, true);
			col_idproducto3.AddAttribute (cellr03, "text", 0);    // la siguiente columna será 1 en vez de 1
			
			TreeViewColumn col_desc_producto3 = new TreeViewColumn();
			CellRendererText cellr13 = new CellRendererText();
			col_desc_producto3.Title = "Descripcion de Producto";
			col_desc_producto3.PackStart(cellr13, true);
			col_desc_producto3.AddAttribute (cellr13, "text", 1);
			col_desc_producto3.Resizable = true;
			cellr1.Width = 400;
			
			TreeViewColumn col_cargos3 = new TreeViewColumn();
			CellRendererText cellrt23 = new CellRendererText();
			col_cargos3.Title = "Tot. Cargado";
			col_cargos3.PackStart(cellrt23, true);
			col_cargos3.AddAttribute (cellrt23, "text", 2);
			//col_stock.SortColumnId = (int) Column_inv.col_stock;
			//col_stock.SetCellDataFunc(cellrt2,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_solicitado3 = new TreeViewColumn();
			CellRendererText cellrt33 = new CellRendererText();
			col_solicitado3.Title = "Solicitado";
			col_solicitado3.PackStart(cellrt33, true);
			col_solicitado3.AddAttribute (cellrt33, "text", 3);
			//col_stock.SortColumnId = (int) Column_inv.col_stock;
			//col_stock.SetCellDataFunc(cellrt2,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_devolucion3 = new TreeViewColumn();
			CellRendererText cellrt43 = new CellRendererText();
			col_devolucion3.Title = "Devolucion";
			col_devolucion3.PackStart(cellrt43, true);
			col_devolucion3.AddAttribute (cellrt43, "text",4 );
			
			TreeViewColumn col_subalmacen3 = new TreeViewColumn();
			CellRendererText cellrt53 = new CellRendererText();
			col_subalmacen3.Title = "Sub-Almacen";
			col_subalmacen3.PackStart(cellrt53, true);
			col_subalmacen3.AddAttribute (cellrt53, "text",5 );
			
			
			lista_cargos_desde_stock.AppendColumn(col_idproducto);
			lista_cargos_desde_stock.AppendColumn(col_desc_producto);
			lista_cargos_desde_stock.AppendColumn(col_cargos);
			lista_cargos_desde_stock.AppendColumn(col_solicitado);
			lista_cargos_desde_stock.AppendColumn(col_devolucion);
			lista_cargos_desde_stock.AppendColumn(col_subalmacen);
			
			lista_solicitado_no_cargado.AppendColumn(col_idproducto2);
			lista_solicitado_no_cargado.AppendColumn(col_desc_producto2);
			lista_solicitado_no_cargado.AppendColumn(col_cargos2);
			lista_solicitado_no_cargado.AppendColumn(col_solicitado2);
			lista_solicitado_no_cargado.AppendColumn(col_devolucion2);
			lista_solicitado_no_cargado.AppendColumn(col_subalmacen2);
				
			lista_solicitados_y_cargados.AppendColumn(col_idproducto3);
			lista_solicitados_y_cargados.AppendColumn(col_desc_producto3);
			lista_solicitados_y_cargados.AppendColumn(col_cargos3);
			lista_solicitados_y_cargados.AppendColumn(col_solicitado3);
			lista_solicitados_y_cargados.AppendColumn(col_devolucion3);
			lista_solicitados_y_cargados.AppendColumn(col_subalmacen3);
		}
		
		void on_button_selecciona_folio_clicked(object obj, EventArgs args)
		{
			llenado_de_devoluciones();	
		}
		
		// busco un paciente pantalla de ingreso de nuevo paciente
		void on_button_buscar_paciente_clicked(object sender, EventArgs args)
	    {
			object[] parametros_objetos = {entry_folio_servicio,entry_pid_paciente,entry_nombre_paciente};
			string[] parametros_sql = {"SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente,osiris_erp_cobros_enca WHERE activo = 'true' "+
										"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
										"AND osiris_erp_cobros_enca.alta_paciente = false "+
										"AND osiris_erp_cobros_enca.cancelado = false "+
										"AND osiris_erp_cobros_enca.alta_paciente = 'false' "+
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cerrado = 'false' "+
										"AND osiris_erp_cobros_enca.reservacion = 'false' ",															
									"SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente,osiris_erp_cobros_enca WHERE activo = 'true' "+
										"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
										"AND osiris_erp_cobros_enca.alta_paciente = false "+
										"AND osiris_erp_cobros_enca.cancelado = false "+
										"AND osiris_erp_cobros_enca.alta_paciente = 'false' "+
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cerrado = 'false' "+
										"AND osiris_erp_cobros_enca.reservacion = 'false' "+
										"AND apellido_paterno_paciente LIKE '%",
									"SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente,osiris_erp_cobros_enca WHERE activo = 'true' "+
										"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
										"AND osiris_erp_cobros_enca.alta_paciente = false "+
										"AND osiris_erp_cobros_enca.cancelado = false "+
										"AND osiris_erp_cobros_enca.alta_paciente = 'false' "+
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cerrado = 'false' "+
										"AND osiris_erp_cobros_enca.reservacion = 'false' "+
										"AND nombre1_paciente LIKE '%",
									"SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente,osiris_erp_cobros_enca WHERE activo = 'true' "+
										"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
										"AND osiris_erp_cobros_enca.alta_paciente = false "+
										"AND osiris_erp_cobros_enca.cancelado = false "+
										"AND osiris_erp_cobros_enca.alta_paciente = 'false' "+
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cerrado = 'false' "+
										"AND osiris_erp_cobros_enca.reservacion = 'false' "+
										"AND osiris_his_paciente.pid_paciente = '"};			
			classfind_data.buscandor(parametros_objetos,parametros_sql,"find_paciente"," ORDER BY osiris_his_paciente.pid_paciente","%' ",1);
		}
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_folio(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				llenado_de_devoluciones();		
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace){
				args.RetVal = true;
			}
		}
		
		void on_button_imprimir_mov_clicked(object obj, EventArgs args)
		{
			
		}
		
		void llenado_de_devoluciones()
		{
			string toma_idproducto = "";
			string toma_descripcionproducto = "";
			string toma_productosaplicados = "";
			string toma_subalmacen = "";
			int toma_idsubalmacen;
			
			treeViewEngine1.Clear();
			treeViewEngine2.Clear();
			treeViewEngine3.Clear();
			
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			NpgsqlConnection conexion1;
			conexion1 = new NpgsqlConnection (connectionString+nombrebd);
			NpgsqlConnection conexion2;
			conexion2 = new NpgsqlConnection (connectionString+nombrebd); 
			
			// Cargos desde el stock del sub-almacen
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT to_char(SUM(cantidad_aplicada),'999999999.99') AS cantidadaplicada,osiris_erp_cobros_deta.id_producto AS idproducto,"+
									"osiris_erp_cobros_deta.id_almacen,descripcion_almacen,osiris_productos.descripcion_producto,osiris_erp_cobros_deta.id_almacen AS idalmacen "+
									"FROM osiris_erp_cobros_deta,osiris_productos,osiris_almacenes "+
									"WHERE osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+						
									"AND osiris_erp_cobros_deta.id_almacen = osiris_almacenes.id_almacen "+
									"AND folio_de_servicio = '"+(string) entry_folio_servicio.Text.ToString().Trim()+"' "+
									"GROUP BY osiris_erp_cobros_deta.id_producto,osiris_productos.descripcion_producto,osiris_erp_cobros_deta.id_almacen,descripcion_almacen "+
									"ORDER BY osiris_erp_cobros_deta.id_almacen,osiris_productos.descripcion_producto;";
				//Console.WriteLine(comando.CommandText);					
				NpgsqlDataReader lector = comando.ExecuteReader ();				
				if(lector.Read()){
					toma_idproducto = (string) lector["idproducto"].ToString().Trim();
					toma_descripcionproducto = (string) lector["descripcion_producto"].ToString().Trim();
					toma_productosaplicados = (string) lector["cantidadaplicada"].ToString();
					toma_subalmacen = (string) lector["descripcion_almacen"].ToString();
					toma_idsubalmacen = (int) lector["idalmacen"];
					conexion1.Open ();
					NpgsqlCommand comando1; 
					comando1 = conexion.CreateCommand ();
				
					// asigna el numero de folio de ingreso de paciente (FOLIO)
					comando1.CommandText = "SELECT osiris_erp_cobros_deta.id_producto AS idproducto,SUM(osiris_erp_cobros_deta.cantidad_aplicada) AS cantidad_aplicada,"+
												"osiris_his_solicitudes_deta.id_producto,SUM(osiris_his_solicitudes_deta.cantidad_autorizada) AS cantidad_autorizada,"+
												"osiris_productos.descripcion_producto AS descripcionproducto "+
												"FROM osiris_erp_cobros_deta,osiris_his_solicitudes_deta,osiris_productos "+
												"WHERE osiris_erp_cobros_deta.folio_de_servicio = osiris_his_solicitudes_deta.folio_de_servicio "+ 
												"AND osiris_erp_cobros_deta.id_producto = osiris_his_solicitudes_deta.id_producto "+
												"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+
												//"AND osiris_erp_cobros_deta.id_almacen = osiris_his_solicitudes_deta.id_almacen "+
												"AND osiris_erp_cobros_deta.folio_de_servicio = '"+(string) entry_folio_servicio.Text.ToString().Trim()+"' "+
												"AND osiris_erp_cobros_deta.id_producto = '"+toma_idproducto+"' "+
												"AND osiris_erp_cobros_deta.id_almacen = '"+toma_idsubalmacen.ToString().Trim()+"' "+
												"AND osiris_his_solicitudes_deta.surtido = 'true' "+
												"GROUP BY osiris_erp_cobros_deta.id_producto,osiris_his_solicitudes_deta.id_producto,osiris_productos.descripcion_producto;";
												//"ORDER BY osiris_productos.descripcion_producto;";
					//Console.WriteLine(comando1.CommandText);					
					NpgsqlDataReader lector1 = comando1.ExecuteReader ();				
					if(lector1.Read() == false){
						treeViewEngine1.AppendValues(toma_idproducto,toma_descripcionproducto,toma_productosaplicados,"0.00","0.00",toma_subalmacen);
					}
					conexion1.Close();					
					while(lector.Read()){
						toma_idproducto = (string) lector["idproducto"].ToString().Trim();
						toma_descripcionproducto = (string) lector["descripcion_producto"].ToString().Trim();
						toma_productosaplicados = (string) lector["cantidadaplicada"].ToString();
						toma_subalmacen = (string) lector["descripcion_almacen"].ToString();
						toma_idsubalmacen = (int) lector["idalmacen"];
						conexion1.Open ();
						comando1 = conexion1.CreateCommand ();				
						// asigna el numero de folio de ingreso de paciente (FOLIO)
						comando1.CommandText = "SELECT osiris_erp_cobros_deta.id_producto AS idproducto,SUM(osiris_erp_cobros_deta.cantidad_aplicada) AS cantidad_aplicada,"+
												"osiris_his_solicitudes_deta.id_producto,SUM(osiris_his_solicitudes_deta.cantidad_autorizada) AS cantidad_autorizada,"+
												"osiris_productos.descripcion_producto AS descripcionproducto "+
												"FROM osiris_erp_cobros_deta,osiris_his_solicitudes_deta,osiris_productos "+
												"WHERE osiris_erp_cobros_deta.folio_de_servicio = osiris_his_solicitudes_deta.folio_de_servicio "+ 
												"AND osiris_erp_cobros_deta.id_producto = osiris_his_solicitudes_deta.id_producto "+
												"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+
												"AND osiris_erp_cobros_deta.id_almacen = osiris_his_solicitudes_deta.id_almacen "+
												"AND osiris_erp_cobros_deta.folio_de_servicio = '"+(string) entry_folio_servicio.Text.ToString().Trim()+"' "+
												"AND osiris_erp_cobros_deta.id_producto = '"+toma_idproducto+"' "+
												"AND osiris_erp_cobros_deta.id_almacen = '"+toma_idsubalmacen.ToString().Trim()+"' "+
												"AND osiris_his_solicitudes_deta.surtido = 'true' "+
												"GROUP BY osiris_erp_cobros_deta.id_producto,osiris_his_solicitudes_deta.id_producto,osiris_productos.descripcion_producto;";
												//"ORDER BY osiris_productos.descripcion_producto;";
						//Console.WriteLine(comando1.CommandText);					
						lector1 = comando1.ExecuteReader ();
						if(lector1.Read() == false){
							treeViewEngine1.AppendValues(toma_idproducto,toma_descripcionproducto,toma_productosaplicados,"0.00","0.00",toma_subalmacen);
						}
						conexion1.Close();
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Error,
			 							ButtonsType.Close, "Este numero de atencion no tiene cargo verifique");
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
						MessageType.Error, 
						ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
	   			
	       	}
       		conexion.Close ();
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT to_char(SUM(cantidad_autorizada),'999999999.99') AS cantidadautorizada,osiris_his_solicitudes_deta.id_producto AS idproducto,"+
									"osiris_his_solicitudes_deta.id_almacen AS idalmacen,descripcion_almacen,osiris_productos.descripcion_producto "+
									"FROM osiris_his_solicitudes_deta,osiris_productos,osiris_almacenes "+
									"WHERE osiris_his_solicitudes_deta.id_producto = osiris_productos.id_producto "+						
									"AND osiris_his_solicitudes_deta.id_almacen = osiris_almacenes.id_almacen "+
									"AND folio_de_servicio = '"+(string) entry_folio_servicio.Text.ToString().Trim()+"' "+
									"AND osiris_his_solicitudes_deta.surtido = 'true' "+
									"GROUP BY osiris_his_solicitudes_deta.id_producto,osiris_productos.descripcion_producto,osiris_his_solicitudes_deta.id_almacen,descripcion_almacen "+
									"ORDER BY osiris_his_solicitudes_deta.id_almacen,osiris_productos.descripcion_producto;";
				//Console.WriteLine(comando.CommandText);					
				NpgsqlDataReader lector = comando.ExecuteReader ();				
				if(lector.Read()){
					toma_idproducto = (string) lector["idproducto"].ToString().Trim();
					toma_descripcionproducto = (string) lector["descripcion_producto"].ToString().Trim();
					toma_productosaplicados = (string) lector["cantidadautorizada"].ToString();
					toma_subalmacen = (string) lector["descripcion_almacen"].ToString();
					toma_idsubalmacen = (int) lector["idalmacen"];
					conexion1.Open ();
					NpgsqlCommand comando1; 
					comando1 = conexion.CreateCommand ();
				
					// asigna el numero de folio de ingreso de paciente (FOLIO)
					comando1.CommandText = "SELECT osiris_erp_cobros_deta.id_producto AS idproducto,SUM(osiris_erp_cobros_deta.cantidad_aplicada) AS cantidad_aplicada,"+
												"osiris_his_solicitudes_deta.id_producto,SUM(osiris_his_solicitudes_deta.cantidad_autorizada) AS cantidad_autorizada,"+
												"osiris_productos.descripcion_producto AS descripcionproducto "+
												"FROM osiris_erp_cobros_deta,osiris_his_solicitudes_deta,osiris_productos "+
												"WHERE osiris_erp_cobros_deta.folio_de_servicio = osiris_his_solicitudes_deta.folio_de_servicio "+ 
												"AND osiris_erp_cobros_deta.id_producto = osiris_his_solicitudes_deta.id_producto "+
												"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+
												"AND osiris_erp_cobros_deta.folio_de_servicio = '"+(string) entry_folio_servicio.Text.ToString().Trim()+"' "+
												"AND osiris_erp_cobros_deta.id_producto = '"+toma_idproducto+"' "+
												"AND osiris_erp_cobros_deta.id_almacen = '"+toma_idsubalmacen.ToString().Trim()+"' "+
												"AND osiris_his_solicitudes_deta.surtido = 'true' "+
												"GROUP BY osiris_erp_cobros_deta.id_producto,osiris_his_solicitudes_deta.id_producto,osiris_productos.descripcion_producto;";
												//"ORDER BY osiris_productos.descripcion_producto;";
					//Console.WriteLine(comando1.CommandText);					
					NpgsqlDataReader lector1 = comando1.ExecuteReader ();				
					if(lector1.Read() == false){
						treeViewEngine2.AppendValues(toma_idproducto,toma_descripcionproducto,"0.00",toma_productosaplicados,toma_productosaplicados,toma_subalmacen);
					}
					conexion1.Close();					
					while(lector.Read()){
						toma_idproducto = (string) lector["idproducto"].ToString().Trim();
						toma_descripcionproducto = (string) lector["descripcion_producto"].ToString().Trim();
						toma_productosaplicados = (string) lector["cantidadautorizada"].ToString();
						toma_subalmacen = (string) lector["descripcion_almacen"].ToString();
						toma_idsubalmacen = (int) lector["idalmacen"];
						conexion1.Open ();
						comando1 = conexion1.CreateCommand ();				
						// asigna el numero de folio de ingreso de paciente (FOLIO)
						comando1.CommandText = "SELECT osiris_erp_cobros_deta.id_producto AS idproducto,SUM(osiris_erp_cobros_deta.cantidad_aplicada) AS cantidad_aplicada,"+
												"osiris_his_solicitudes_deta.id_producto,SUM(osiris_his_solicitudes_deta.cantidad_autorizada) AS cantidad_autorizada,"+
												"osiris_productos.descripcion_producto AS descripcionproducto "+
												"FROM osiris_erp_cobros_deta,osiris_his_solicitudes_deta,osiris_productos "+
												"WHERE osiris_erp_cobros_deta.folio_de_servicio = osiris_his_solicitudes_deta.folio_de_servicio "+ 
												"AND osiris_erp_cobros_deta.id_producto = osiris_his_solicitudes_deta.id_producto "+
												"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+
												"AND osiris_erp_cobros_deta.folio_de_servicio = '"+(string) entry_folio_servicio.Text.ToString().Trim()+"' "+
												"AND osiris_erp_cobros_deta.id_producto = '"+toma_idproducto+"' "+
												"AND osiris_erp_cobros_deta.id_almacen = '"+toma_idsubalmacen.ToString().Trim()+"' "+
												"AND osiris_his_solicitudes_deta.surtido = 'true' "+
												"GROUP BY osiris_erp_cobros_deta.id_producto,osiris_his_solicitudes_deta.id_producto,osiris_productos.descripcion_producto;";
												//"ORDER BY osiris_productos.descripcion_producto;";
						//Console.WriteLine(comando1.CommandText);					
						lector1 = comando1.ExecuteReader ();
						if(lector1.Read() == false){
							treeViewEngine2.AppendValues(toma_idproducto,toma_descripcionproducto,"0.00",toma_productosaplicados,toma_productosaplicados,toma_subalmacen);
						}
						conexion1.Close();
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Error,
			 							ButtonsType.Close, "Este numero de atencion no tiene cargo verifique");
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
						MessageType.Error, 
						ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
	   			
	       	}
       		conexion.Close ();
			string xxx;
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText =  "SELECT to_char(SUM(cantidad_aplicada),'999999999.99') AS cantidadaplicada,osiris_erp_cobros_deta.id_producto AS idproducto,"+
									"osiris_erp_cobros_deta.id_almacen,descripcion_almacen,osiris_productos.descripcion_producto,osiris_erp_cobros_deta.id_almacen AS idalmacen "+
									"FROM osiris_erp_cobros_deta,osiris_productos,osiris_almacenes "+
									"WHERE osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+						
									"AND osiris_erp_cobros_deta.id_almacen = osiris_almacenes.id_almacen "+
									"AND folio_de_servicio = '"+(string) entry_folio_servicio.Text.ToString().Trim()+"' "+							
									"GROUP BY osiris_erp_cobros_deta.id_producto,osiris_productos.descripcion_producto,osiris_erp_cobros_deta.id_almacen,descripcion_almacen "+
									"ORDER BY osiris_erp_cobros_deta.id_almacen,osiris_productos.descripcion_producto;";
				//Console.WriteLine(comando.CommandText);					
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				while(lector.Read() == true){
					toma_idproducto = (string) lector["idproducto"].ToString().Trim();
					toma_descripcionproducto = (string) lector["descripcion_producto"].ToString().Trim();
					toma_productosaplicados = (string) lector["cantidadaplicada"].ToString();
					toma_subalmacen = (string) lector["descripcion_almacen"].ToString();
					toma_idsubalmacen = (int) lector["idalmacen"];
					
					try{					
						conexion1.Open ();
						NpgsqlCommand comando1; 
						comando1 = conexion.CreateCommand ();
				
						// asigna el numero de folio de ingreso de paciente (FOLIO)
						comando1.CommandText = "SELECT to_char(SUM(cantidad_autorizada),'999999999.99') AS cantidadautorizada,osiris_his_solicitudes_deta.id_producto AS idproducto,"+
										"osiris_his_solicitudes_deta.id_almacen AS idalmacen,descripcion_almacen,osiris_productos.descripcion_producto "+
										"FROM osiris_his_solicitudes_deta,osiris_productos,osiris_almacenes "+
										"WHERE osiris_his_solicitudes_deta.id_producto = osiris_productos.id_producto "+						
										"AND osiris_his_solicitudes_deta.id_almacen = osiris_almacenes.id_almacen "+
										"AND folio_de_servicio = '"+(string) entry_folio_servicio.Text.ToString().Trim()+"' "+
										"AND osiris_his_solicitudes_deta.id_producto = '"+toma_idproducto+"' "+
										"AND osiris_his_solicitudes_deta.id_almacen = '"+toma_idsubalmacen.ToString().Trim()+"' "+
										"AND osiris_his_solicitudes_deta.surtido = 'true' "+
										"GROUP BY osiris_his_solicitudes_deta.id_producto,osiris_productos.descripcion_producto,osiris_his_solicitudes_deta.id_almacen,descripcion_almacen "+
										"ORDER BY osiris_his_solicitudes_deta.id_almacen,osiris_productos.descripcion_producto;";
						//Console.WriteLine(comando1.CommandText);					
						NpgsqlDataReader lector1 = comando1.ExecuteReader ();
						if(lector1.Read() == true){
							treeViewEngine3.AppendValues(toma_idproducto,toma_descripcionproducto,toma_productosaplicados,(string) lector1["cantidadautorizada"], Convert.ToDouble(float.Parse((string) lector1["cantidadautorizada"])-float.Parse(toma_productosaplicados)).ToString("F"),toma_subalmacen);
						}						
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
						MessageType.Error, 
						ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();				msgBoxError.Destroy();	   			
	       			}
					conexion1.Close ();
				}				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
						MessageType.Error, 
						ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
	   			
	       	}
       		conexion.Close ();			
		}
		
		void on_cierraventanas_clicked (object obj, EventArgs args)
		{
			Widget win = (Widget) obj;
			win.Toplevel.Destroy();
		}
	}
}
