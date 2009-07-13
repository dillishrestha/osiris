// created on 15/02/2008 at 10:47 a
//////////////////////////////////////////////////////////////////////
// created on 21/01/2008 at 08:28 p
// Hospital Santa Cecilia
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
// Programa		: hscmty.cs
// Proposito	:  
// Objeto		: 
//////////////////////////////////////////////////////////
using System;
using System.IO;
using Gtk;
using Gnome;
using Npgsql;
using System.Data;
using Glade;
using System.Collections;
using GtkSharp;

/*namespace osiris
{
	public class abonos1
	{
		//Declarando ventana de cambios de datos de paciente
		[Widget] Gtk.Window abonar_procedimientos;
		[Widget] Gtk.Entry entry_monto_abono;
		[Widget] Gtk.Entry entry_recibo_caja;
		[Widget] Gtk.Entry entry_presupuesto;
		[Widget] Gtk.Entry entry_paquete;
		[Widget] Gtk.Entry entry_dia;
		[Widget] Gtk.Entry entry_mes;
		[Widget] Gtk.Entry entry_ano;
		[Widget] Gtk.Entry entry_concepto_abono;
		[Widget] Gtk.Entry entry_total_abonos;
		[Widget] Gtk.CheckButton checkbutton_nuevo_abono;
		[Widget] Gtk.Button button_guardar;
		[Widget] Gtk.Button button_imprimir;
		[Widget] Gtk.Button button_salir;
		[Widget] Gtk.Button button_resumen;
		[Widget] Gtk.TreeView lista_abonos;
		[Widget] Gtk.Statusbar statusbar_abonos;
		
		public string nombrebd;
		public int PidPaciente = 0;
		public int folioservicio = 0;
		public string fecha_admision;
		public string fechahora_alta;
		public string nombre_paciente;
		public string telefono_paciente;
		public string doctor;
		public string cirugia;
		public string fecha_nacimiento;
		public string edadpac;
		public string tipo_paciente;
		public int id_tipopaciente;
		public string aseguradora;
		public string dir_pac;
		public string empresapac;
		public bool apl_desc_siempre = true;
		public bool apl_desc;
		public string nombrecajero;		
		public string LoginEmpleado;
		
		public string monto;
		public string fecha;
		public string concepto;
		public string idcreo;
		public string recibo;
		public string presupuesto;
		public string paquete;
				
		public string connectionString = "Server=192.168.1.4;" +
									"Port=5432;" +
									"User ID=admin1;" +
									"Password=1qaz2wsx;";		
		
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		private TreeStore treeViewEngineabonos;
		
		//Declarando las celdas
		public CellRendererText cellr0;				public CellRendererText cellrt1;
		public CellRendererText cellrt2;			public CellRendererText cellrt3;
		public CellRendererText cellrt4;			public CellRendererText cellrt5;
		public CellRendererText cellrt6;			public CellRendererText cellrt7;
		public CellRendererText cellrt8;			
				
		public abonos1 (	int PidPaciente_ ,int folioservicio_,string _nombrebd_ ,string entry_fecha_admision_,
						string entry_fechahora_alta_,string entry_numero_factura_,string entry_nombre_paciente_,
						string entry_telefono_paciente_,string entry_doctor_,string entry_tipo_paciente_,
						string entry_aseguradora_,string edadpac_,string fecha_nacimiento_,string dir_pac_,
						string cirugia_,string empresapac_,int idtipopaciente_,string nombrecajero_,string LoginEmpleado_)
		{
			PidPaciente = PidPaciente_;//
			folioservicio = folioservicio_;//
			nombrebd = _nombrebd_;//
			fecha_admision = entry_fecha_admision_;//
			fechahora_alta = entry_fechahora_alta_;//
			nombre_paciente = entry_nombre_paciente_;//
			telefono_paciente = entry_telefono_paciente_;//
			doctor = entry_doctor_;//
			cirugia = cirugia_;//
			tipo_paciente = entry_tipo_paciente_;//
			id_tipopaciente = idtipopaciente_;
			aseguradora = entry_aseguradora_;//
			edadpac = edadpac_;//
			fecha_nacimiento = fecha_nacimiento_;//
			dir_pac = dir_pac_;//
			empresapac = empresapac_;//
			nombrecajero = nombrecajero_;
			LoginEmpleado = LoginEmpleado_;
			
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "abonar_procedimientos", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        abonar_procedimientos.Show();
			crea_treeview_abonos();
			llenando_lista_de_abonos();
			button_guardar.Clicked += new EventHandler(on_button_guardar_clicked);
			button_imprimir.Clicked += new EventHandler(on_button_imprimir_clicked);
			button_resumen.Clicked += new EventHandler(on_button_resumen_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
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
			statusbar_abonos.Pop(0);
			statusbar_abonos.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+nombrecajero);
			statusbar_abonos.HasResizeGrip = false;
		}
		
		void crea_treeview_abonos()
		{
			treeViewEngineabonos = new TreeStore(typeof(string),//0
												typeof(string),//1
												typeof(string),//2
												typeof(string),//3
												typeof(string),//4
												typeof(string),//5
												typeof(string));//6
												
			lista_abonos.Model = treeViewEngineabonos;
			
			lista_abonos.RulesHint = true;
				
			lista_abonos.RowActivated += on_button_imprimir_clicked;  // Doble click selecciono paciente
			
			TreeViewColumn col_abono = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_abono.Title = "Abonos Ralizados"; // titulo de la cabecera de la columna, si está visible
			col_abono.PackStart(cellr0, true);
			col_abono.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1
			col_abono.SortColumnId = (int) Col_proveedores.col_abono;
			
			TreeViewColumn col_fecha_abono = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_fecha_abono.Title = "Fecha del Abono";
			col_fecha_abono.PackStart(cellrt1, true);
			col_fecha_abono.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
			col_fecha_abono.SortColumnId = (int) Col_proveedores.col_fecha_abono;
			
			TreeViewColumn col_concepto = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_concepto.Title = "Concepto";
			col_concepto.PackStart(cellrt2, true);
			col_concepto.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 3
			col_concepto.SortColumnId = (int) Col_proveedores.col_concepto;
			
			TreeViewColumn col_id_creo = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_id_creo.Title = "Id Quien Creo";
			col_id_creo.PackStart(cellrt3, true);
			col_id_creo.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 4
			col_id_creo.SortColumnId = (int) Col_proveedores.col_id_creo;
			
			TreeViewColumn col_recibo = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_recibo.Title = "No. Recibo Caja";
			col_recibo.PackStart(cellrt4, true);
			col_recibo.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 5
			col_recibo.SortColumnId = (int) Col_proveedores.col_recibo;
			
			TreeViewColumn col_presu = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_presu.Title = "Id Presupuesto";
			col_presu.PackStart(cellrt5, true);
			col_presu.AddAttribute (cellrt5, "text", 5); // la siguiente columna será 6
			col_presu.SortColumnId = (int) Col_proveedores.col_presu;
			
			TreeViewColumn col_paq = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_paq.Title = "Id Paquete";
			col_paq.PackStart(cellrt6, true);
			col_paq.AddAttribute (cellrt6, "text", 6); // la siguiente columna será 7
			col_paq.SortColumnId = (int) Col_proveedores.col_paq;
			
            lista_abonos.AppendColumn(col_abono);
			lista_abonos.AppendColumn(col_fecha_abono);
			lista_abonos.AppendColumn(col_concepto);
			lista_abonos.AppendColumn(col_id_creo);
			lista_abonos.AppendColumn(col_recibo);
			lista_abonos.AppendColumn(col_presu);
			lista_abonos.AppendColumn(col_paq);
		}
		
		enum Col_proveedores
		{
			col_abono,
			col_fecha_abono,
			col_concepto,
			col_id_creo,
			col_recibo,
			col_presu,
			col_paq
		}
		
		void llenando_lista_de_abonos()
		{
			decimal total = 0;
			treeViewEngineabonos.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
			try
				{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
					comando.CommandText = "SELECT id_abono, "+ 
								"to_char(hscmty_erp_abonos.id_abono,'9999999999') AS idabono, "+
								"folio_de_servicio, "+
								"monto_de_abono_procedimiento, "+
								"monto_de_abono_factura, "+
								"numero_recibo_caja, "+
								"to_char(numero_recibo_caja,'9999999999') AS recibocaja, "+
								"numero_factura, "+								
								"id_quien_creo, "+
								"monto_de_abono_procedimiento, "+
								"to_char(hscmty_erp_abonos.monto_de_abono_procedimiento,'9999999999.99') AS abono, "+
								"concepto_del_abono, "+
								"fechahora_registro, "+
								"to_char(hscmty_erp_abonos.fechahora_registro,'yyyy-MM-dd HH:mi:ss') AS fecha_registro, "+
								"fecha_abono, "+
								"to_char(hscmty_erp_abonos.fecha_abono,'dd-MM-yyyy') AS fechaabono, "+
								"id_presupuesto, "+
								"to_char(id_presupuesto,'9999999999') AS presupuesto, "+
								"id_paquete, "+
								"to_char(id_paquete,'9999999999') AS paquete "+
								"FROM hscmty_erp_abonos "+
								"WHERE hscmty_erp_abonos.folio_de_servicio = '"+this.folioservicio.ToString()+"' "+
								"ORDER BY hscmty_erp_abonos.folio_de_servicio;";															
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read())
				{	
					treeViewEngineabonos.AppendValues ((string) lector["abono"],//0
													(string) lector["fechaabono"],//1
													(string) lector["concepto_del_abono"],//2
													(string) lector["id_quien_creo"],//3
													(string) lector["recibocaja"],//4
													(string) lector["presupuesto"],//5
													(string) lector["paquete"]);//6
				total += decimal.Parse((string) lector["abono"]);
				this.entry_total_abonos.Text = total.ToString();
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
		
		void on_checkbutton_nuevo_abono_clicked(object sender, EventArgs args)
		{
			if(checkbutton_nuevo_abono.Active == true) { 
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
				MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de querer realizar un nuevo abono?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
	 			if (miResultado == ResponseType.Yes){
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
				}else{
					checkbutton_nuevo_abono.Active = false;
				}
			}
		}
		
		
		void on_button_guardar_clicked(object sender, EventArgs args)
		{
			if(checkbutton_nuevo_abono.Active == true)
			{
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
	 					comando.CommandText = "INSERT INTO hscmty_erp_abonos("+
										  	"monto_de_abono_procedimiento, "+//2
											"numero_recibo_caja, "+//3
											"id_quien_creo, "+//4
											"concepto_del_abono, "+//5
											"fechahora_registro, "+//6
											"fecha_abono, "+//7
											"id_presupuesto, "+//8
											"id_paquete ,"+//9
											"folio_de_servicio )"+
 										  	"VALUES ('"+
	 										(string) this.entry_monto_abono.Text.Trim().ToUpper()+"','"+//2
	 										(string) this.entry_recibo_caja.Text.Trim().ToUpper()+"','"+//3										  
	 										LoginEmpleado+"','"+//4
	 										(string) this.entry_concepto_abono.Text.Trim().ToUpper()+"','"+//5
	 										DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+//6
	 										(string) this.entry_ano.Text+" "+this.entry_mes.Text+" "+this.entry_dia.Text+"','"+//7
	 										(string) this.entry_presupuesto.Text.Trim().ToUpper()+"','"+//8
	 										(string) this.entry_paquete.Text.Trim().ToUpper()+"','"+//9
	 										folioservicio+//"','"+//10
	 										"');";
 						comando.ExecuteNonQuery();    	    	       	comando.Dispose();
 						
 						NpgsqlConnection conexion2; 
						conexion2 = new NpgsqlConnection (connectionString+nombrebd);
    	        		// Verifica que la base de datos este conectada
    	        		try{
	    	        		conexion2.Open ();
							NpgsqlCommand comando2; 
							comando2 = conexion2.CreateCommand ();
		 					comando2.CommandText = "UPDATE hscmty_erp_cobros_enca SET tiene_abono = 'true',"+
		 										"total_abonos = total_abonos + '"+entry_monto_abono.Text+"' "+
												"WHERE folio_de_servicio = '"+this.folioservicio.ToString()+"' ;";
		 					
	 						comando2.ExecuteNonQuery();    	    	       	comando2.Dispose();
 						}catch(NpgsqlException ex){
	   					MessageDialog msgBoxError5 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError5.Run ();					msgBoxError5.Destroy();
	       				}
       					conexion2.Close ();
 						
 						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info,ButtonsType.Close,"El Abono se guardo con exito");
										llenando_lista_de_abonos();
						msgBoxError.Run ();					msgBoxError.Destroy();
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
 			}
		}
		
		void on_button_imprimir_clicked(object sender, EventArgs args)
		{
			imprime_comprobante_resumen("comprobante");
		}
		
		void on_button_resumen_clicked(object sender, EventArgs args)
		{
			imprime_comprobante_resumen("resumen");
		}
		
		void imprime_comprobante_resumen(string tipo_reporte)
		{
		if (tipo_reporte == "comprobante"){
			TreeModel model;
			TreeIter iterSelected;
			if (lista_abonos.Selection.GetSelected(out model, out iterSelected)){
 				monto = (string) model.GetValue(iterSelected, 0); 				
 				fecha = (string) model.GetValue(iterSelected, 1);
				concepto = (string) model.GetValue(iterSelected, 2);
				idcreo = (string) model.GetValue(iterSelected, 3);
				recibo = (string) model.GetValue(iterSelected, 4);
				presupuesto = (string) model.GetValue(iterSelected, 5);
				paquete = (string) model.GetValue(iterSelected, 6);
				
				new osiris.comprobante_abono_resumen(tipo_reporte,PidPaciente,folioservicio,nombrebd,
						fecha_admision,fechahora_alta,nombre_paciente,telefono_paciente,doctor,cirugia,
						tipo_paciente,id_tipopaciente,aseguradora,edadpac,fecha_nacimiento,dir_pac,
						empresapac,nombrecajero,LoginEmpleado,monto,fecha,concepto,idcreo,recibo,
						presupuesto,paquete,this.lista_abonos,this.treeViewEngineabonos);
			}
		}
		if (tipo_reporte == "resumen")
			{
				monto = ""; 				
 				fecha = "";
				concepto = "";
				idcreo = "";
				recibo = "";
				presupuesto = "";
				paquete = "";
				
				new osiris.comprobante_abono_resumen(tipo_reporte,PidPaciente,folioservicio,nombrebd,
						fecha_admision,fechahora_alta,nombre_paciente,telefono_paciente,doctor,cirugia,
						tipo_paciente,id_tipopaciente,aseguradora,edadpac,fecha_nacimiento,dir_pac,
						empresapac,nombrecajero,LoginEmpleado,monto,fecha,concepto,idcreo,recibo,
						presupuesto,paquete,this.lista_abonos,this.treeViewEngineabonos);
			}
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}

//////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class comprobante_abono_resumen1
	{
				
		public string connectionString = "Server=192.168.1.4;" +
									"Port=5432;" +
									"User ID=admin1;" +
									"Password=1qaz2wsx;";		
		public string tipo_reporte;
		public string nombrebd;
		public int PidPaciente = 0;
		public int folioservicio = 0;
		public string fecha_admision;
		public string fechahora_alta;
		public string nombre_paciente;
		public string telefono_paciente;
		public string doctor;
		public string cirugia;
		public string fecha_nacimiento;
		public string edadpac;
		public string tipo_paciente;
		public int id_tipopaciente;
		public string aseguradora;
		public string dir_pac;
		public string empresapac;
		public string nombrecajero;		
		public string LoginEmpleado;
		public string monto;
		public string fecha;
		public string concepto;
		public string idcreo;
		public string recibo;
		public string presupuesto;
		public string paquete;
		public string datos = "";
		public int contador = 1;
		public int numpage = 1;
		public string schars = "";
		public int filas=645;
		
		public Gtk.TreeView lista_abonos;
		public Gtk.TreeStore treeViewEngineabonos;
		
		// Declaracion de fuentes tipo Bitstream Vera sans
		public Gnome.Font fuente6 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
		public Gnome.Font fuente8 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
		public Gnome.Font fuente12 = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
		public Gnome.Font fuente10 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
		public Gnome.Font fuente36 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
		public Gnome.Font fuente7 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
		public Gnome.Font fuente9 = Gnome.Font.FindClosest("Bitstream Vera Sans", 9);
		
		// traductor de numeros a letras
		public string[] sUnidades = {"", "un", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "diez", 
									"once", "doce", "trece", "catorce", "quince", "dieciseis", "diecisiete", "dieciocho", "diecinueve", "veinte", 
									"veintiún", "veintidos", "veintitres", "veinticuatro", "veinticinco", "veintiseis", "veintisiete", "veintiocho", "veintinueve"};
 		public string[] sDecenas = {"", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa"};
 		public string[] sCentenas = {"", "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos"};
  		public string sResultado = "";				
		
		public comprobante_abono_resumen1(string tipo_reporte_,int PidPaciente_,int folioservicio_,
					string _nombrebd_,string entry_fecha_admision_,string entry_fechahora_alta_,
					string entry_nombre_paciente_,string entry_telefono_paciente_,string entry_doctor_,
					string cirugia_,string entry_tipo_paciente_,int idtipopaciente_,string entry_aseguradora_,
					string edadpac_,string fecha_nacimiento_,string dir_pac_,string empresapac_,string nombrecajero_,
					string LoginEmpleado_,string monto_,string fecha_,string concepto_,string idcreo_,string recibo_,
					string presupuesto_,string paquete_,object lista_abonos_,object treeViewEngineabonos_)
			{		
			lista_abonos = lista_abonos_ as Gtk.TreeView;
			treeViewEngineabonos = treeViewEngineabonos_ as Gtk.TreeStore;
			tipo_reporte = tipo_reporte_;
			PidPaciente = PidPaciente_;//
			folioservicio = folioservicio_;//
			nombrebd = _nombrebd_;//
			fecha_admision = entry_fecha_admision_;//
			fechahora_alta = entry_fechahora_alta_;//
			nombre_paciente = entry_nombre_paciente_;//
			telefono_paciente = entry_telefono_paciente_;//
			doctor = entry_doctor_;//
			cirugia = cirugia_;//
			tipo_paciente = entry_tipo_paciente_;//
			id_tipopaciente = idtipopaciente_;
			aseguradora = entry_aseguradora_;//
			edadpac = edadpac_;//
			fecha_nacimiento = fecha_nacimiento_;//
			dir_pac = dir_pac_;//
			empresapac = empresapac_;//
			nombrecajero = nombrecajero_;
			LoginEmpleado = LoginEmpleado_;
			monto = monto_;
			fecha = fecha_;
			concepto = concepto_;
			idcreo = idcreo_;
			recibo = recibo_;
			presupuesto = presupuesto_;
			paquete = paquete_;
			
			if (this.tipo_reporte == "resumen")	{
				imprime_resumen_comprobante("RESUMEN DE ABONOS","resumen");
			}
			if (this.tipo_reporte == "comprobante")	{ 
				imprime_resumen_comprobante("COMPROBANTE DE CAJA","comprobante");
			}
		}	
		
		void imprime_resumen_comprobante(string titulo,string tipo_reporte)
		{
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, titulo, 0);
        	int         respuesta = dialogo.Run ();
        	if (respuesta == (int) PrintButtons.Cancel) 
			{
				dialogo.Hide (); 
				dialogo.Dispose (); 
				return;
			}
			Gnome.PrintContext ctx = trabajo.Context;
			if (tipo_reporte == "resumen"){
				ComponerPagina(ctx, trabajo);
			}
			if (tipo_reporte == "comprobante"){ 
        		ComponerPagina1(ctx, trabajo);
        	} 
			trabajo.Close();
            switch (respuesta)
        	{
                  case (int) PrintButtons.Print:   
                  		trabajo.Print (); 
                  		break;
                  case (int) PrintButtons.Preview:
                      	new PrintJobPreview(trabajo, "COMPROBANTE DE CAJA").Show();
                        break;
        	}
			dialogo.Hide (); dialogo.Dispose ();
		}
		
      	void ComponerPagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{	
			ContextoImp.BeginPage("Pagina 1");
			Gnome.Print.Setfont (ContextoImp, fuente10);
			ContextoImp.MoveTo(500.5, 740);		ContextoImp.Show(DateTime.Now.ToString("dd-MM-yyyy") );
			ContextoImp.MoveTo(501, 740);		ContextoImp.Show(DateTime.Now.ToString("dd-MM-yyyy") );
			Gnome.Print.Setfont (ContextoImp, fuente9);
			ContextoImp.MoveTo(39.5, 710);		ContextoImp.Show("Nombre: ");
			ContextoImp.MoveTo(40, 710);		ContextoImp.Show("Nombre: "+nombre_paciente.ToString());
			ContextoImp.MoveTo(39.5, 690);		ContextoImp.Show("Direccion: ");
			ContextoImp.MoveTo(40, 690);		ContextoImp.Show("Direccion: "+dir_pac.ToString());
			ContextoImp.MoveTo(39.5, 670);		ContextoImp.Show("Telefono: ");
			ContextoImp.MoveTo(40, 670);		ContextoImp.Show("Telefono: "+telefono_paciente.ToString());
			Gnome.Print.Setrgbcolor(ContextoImp,150,0,0);//cambio a color rojo obscuro
			ContextoImp.MoveTo(154.5, 670);		ContextoImp.Show("PID: "+PidPaciente.ToString());
			ContextoImp.MoveTo(155, 670);		ContextoImp.Show("PID: "+PidPaciente.ToString());
			ContextoImp.MoveTo(209.5, 670);		ContextoImp.Show("EDAD: "+edadpac.ToString());
			ContextoImp.MoveTo(210, 670);		ContextoImp.Show("EDAD: "+edadpac.ToString());
			ContextoImp.MoveTo(360.5, 670);		ContextoImp.Show("Folio de Servicio: "+folioservicio.ToString());
			ContextoImp.MoveTo(361, 670);		ContextoImp.Show("Folio de Servicio: "+folioservicio.ToString());
			ContextoImp.MoveTo(375.5, 710);		ContextoImp.Show("Fecha Admision: "+fecha_admision.ToString());
			ContextoImp.MoveTo(376, 710);		ContextoImp.Show("Fecha Admision: "+fecha_admision.ToString());			
			Gnome.Print.Setrgbcolor(ContextoImp,0,0,0);//cambio a color negro
			ContextoImp.MoveTo(375.5, 710);		ContextoImp.Show("Fecha Admision: ");
			ContextoImp.MoveTo(376, 710);		ContextoImp.Show("Fecha Admision: ");
			ContextoImp.MoveTo(154.5, 670);		ContextoImp.Show("PID: ");
			ContextoImp.MoveTo(155, 670);		ContextoImp.Show("PID: ");
			ContextoImp.MoveTo(209.5, 670);		ContextoImp.Show("EDAD: ");
			ContextoImp.MoveTo(210, 670);		ContextoImp.Show("EDAD: ");
			ContextoImp.MoveTo(360.5, 670);		ContextoImp.Show("Folio de Servicio: ");
			ContextoImp.MoveTo(361, 670);		ContextoImp.Show("Folio de Servicio: ");
			ContextoImp.MoveTo(60.5, 650);		ContextoImp.Show("Fecha del Abono");
			ContextoImp.MoveTo(61, 650);		ContextoImp.Show("Fecha del Abono");
			ContextoImp.MoveTo(155.5, 650);		ContextoImp.Show("Concepto");
			ContextoImp.MoveTo(156, 650);		ContextoImp.Show("Concepto");
			ContextoImp.MoveTo(430.5, 650);		ContextoImp.Show("Monto del Abono");
			ContextoImp.MoveTo(431, 650);		ContextoImp.Show("Monto del Abono");
			imprime_encabezado(ContextoImp,trabajoImpresion);
			filas = 630;
			TreeIter iter;
			float total_de_abonos = 0;
			string tomovalor1 = "";
			if (this.treeViewEngineabonos.GetIterFirst (out iter)){
				ContextoImp.MoveTo(430, filas);	ContextoImp.Show((string) this.lista_abonos.Model.GetValue (iter,0));
				
				tomovalor1 = (string) this.lista_abonos.Model.GetValue (iter,2);
					if(tomovalor1.Length > 50)
					{
						tomovalor1 = tomovalor1.Substring(0,50); 
					}
				ContextoImp.MoveTo(155, filas);	ContextoImp.Show(tomovalor1);
				
				ContextoImp.MoveTo(60, filas);	ContextoImp.Show((string) this.lista_abonos.Model.GetValue (iter,1));
				total_de_abonos += float.Parse((string) this.lista_abonos.Model.GetValue (iter,0));
				filas -= 08;
				while (treeViewEngineabonos.IterNext(ref iter))
				{
					ContextoImp.MoveTo(430, filas);	ContextoImp.Show((string) this.lista_abonos.Model.GetValue (iter,0));
					ContextoImp.MoveTo(60, filas);	ContextoImp.Show((string) this.lista_abonos.Model.GetValue (iter,1));
					
					tomovalor1 = (string) this.lista_abonos.Model.GetValue (iter,2);
					if(tomovalor1.Length > 50)
					{
						tomovalor1 = tomovalor1.Substring(0,50); 
					}
					ContextoImp.MoveTo(155, filas);	ContextoImp.Show(tomovalor1);
					
					total_de_abonos += float.Parse((string) this.lista_abonos.Model.GetValue (iter,0));
					filas -= 08;
				}
				filas -= 10;
				ContextoImp.MoveTo(360.5, filas);	ContextoImp.Show("Total de Abonos: "+total_de_abonos.ToString("C"));
				ContextoImp.MoveTo(361, filas);		ContextoImp.Show("Total de Abonos: "+total_de_abonos.ToString("C"));
			}
			ContextoImp.ShowPage();
		}
      	
      	void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
      		// Cambiar la fuente
			Gnome.Print.Setfont (ContextoImp, fuente6);
			ContextoImp.MoveTo(19.7, 770);			ContextoImp.Show("Hospital Santa Cecilia");
			ContextoImp.MoveTo(20, 770);			ContextoImp.Show("Hospital Santa Cecilia");
			ContextoImp.MoveTo(19.7, 760);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(20, 760);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(19.7, 750);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			ContextoImp.MoveTo(20, 750);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			Gnome.Print.Setfont (ContextoImp, fuente36);
			ContextoImp.MoveTo(20, 738);			ContextoImp.Show("_______________________________");
			//Print.Setrgbcolor(ContextoImp, 150,0,0);///cambio color de fuente a rojo
			Gnome.Print.Setfont (ContextoImp, fuente10);
			ContextoImp.MoveTo(230.7, 50);			ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			ContextoImp.MoveTo(230, 50);			ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			Gnome.Print.Setfont (ContextoImp, fuente9);
			Gnome.Print.Setrgbcolor(ContextoImp, 0,0,0);//regreso color fuente a negro
	  	}
		
		void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion,int contador_)
		{
			if (contador_ > 63 )//57
	        {
	        	numpage +=1;
	        	ContextoImp.ShowPage();
				ContextoImp.BeginPage("Pagina N");
				schars = "";
				imprime_encabezado(ContextoImp,trabajoImpresion);
	     		Gnome.Print.Setfont (ContextoImp, fuente7);
	     		contador=1;
	        	filas=690;
	        }
	    }

		void ComponerPagina1 (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{	
			ContextoImp.BeginPage("Pagina 1");
			/////DATOS DE PRODUCTOS
 		  	Gnome.Print.Setfont (ContextoImp, fuente10);
			ContextoImp.MoveTo(500.5, 740);		ContextoImp.Show(DateTime.Now.ToString("dd-MM-yyyy") );
			ContextoImp.MoveTo(501, 740);		ContextoImp.Show(DateTime.Now.ToString("dd-MM-yyyy") );
			Gnome.Print.Setfont (ContextoImp, fuente9);
			ContextoImp.MoveTo(79.5, 710);		ContextoImp.Show(nombre_paciente.ToString());
			ContextoImp.MoveTo(80, 710);		ContextoImp.Show(nombre_paciente.ToString());
			ContextoImp.MoveTo(80, 690);		ContextoImp.Show(dir_pac.ToString());
			ContextoImp.MoveTo(80, 670);		ContextoImp.Show(telefono_paciente.ToString());
			Gnome.Print.Setrgbcolor(ContextoImp,150,0,0);//cambio a color rojo obscuro
			ContextoImp.MoveTo(144.5, 670);		ContextoImp.Show("PID: "+PidPaciente.ToString());
			ContextoImp.MoveTo(145, 670);		ContextoImp.Show("PID: "+PidPaciente.ToString());
			ContextoImp.MoveTo(199.5, 670);		ContextoImp.Show("EDAD: "+edadpac.ToString());
			ContextoImp.MoveTo(200, 670);		ContextoImp.Show("EDAD: "+edadpac.ToString());
			ContextoImp.MoveTo(340.5, 670);		ContextoImp.Show("Folio de Servicio: "+folioservicio.ToString());
			ContextoImp.MoveTo(341, 670);		ContextoImp.Show("Folio de Servicio: "+folioservicio.ToString());
			ContextoImp.MoveTo(375.5, 710);		ContextoImp.Show("Fecha Admision: "+fecha_admision.ToString());
			ContextoImp.MoveTo(376, 710);		ContextoImp.Show("Fecha Admision: "+fecha_admision.ToString());			
			Gnome.Print.Setrgbcolor(ContextoImp,0,0,0);//cambio a color negro
			ContextoImp.MoveTo(144.5, 670);		ContextoImp.Show("PID: ");
			ContextoImp.MoveTo(145, 670);		ContextoImp.Show("PID: ");
			ContextoImp.MoveTo(199.5, 670);		ContextoImp.Show("EDAD: ");
			ContextoImp.MoveTo(200, 670);		ContextoImp.Show("EDAD: ");
			ContextoImp.MoveTo(340.5, 670);		ContextoImp.Show("Folio de Servicio: ");
			ContextoImp.MoveTo(341, 670);		ContextoImp.Show("Folio de Servicio: ");
			ContextoImp.MoveTo(375.5, 710);		ContextoImp.Show("Fecha Admision: ");
			ContextoImp.MoveTo(376, 710);		ContextoImp.Show("Fecha Admision: ");
			Gnome.Print.Setfont (ContextoImp, fuente9);
			//LUGAR DE CARGO
			string tomovalor1 = "";
			
			tomovalor1 = this.concepto.ToString();
					if(tomovalor1.Length > 75)
					{
						tomovalor1 = tomovalor1.Substring(0,75); 
					}
			ContextoImp.MoveTo(90.5, 600);	ContextoImp.Show(tomovalor1);
			ContextoImp.MoveTo(91, 600);	ContextoImp.Show(tomovalor1);
			
			filas+=20;
			filas-=35;
			ContextoImp.MoveTo(514.7, 600);			ContextoImp.Show("$: "+monto.Trim());
			ContextoImp.MoveTo(515, 600);			ContextoImp.Show("$: "+monto.Trim());
		 	filas-=30;
			float apagar = float.Parse(monto);
			ContextoImp.MoveTo(175, 455);				ContextoImp.Show(traduce_numeros(apagar.ToString("F")));
			ContextoImp.MoveTo(514.7, 465);				ContextoImp.Show(apagar.ToString("C").PadLeft(10));
			ContextoImp.MoveTo(515, 465);				ContextoImp.Show(apagar.ToString("C").PadLeft(10));
			filas-=10;
			ContextoImp.MoveTo(175, 425);				ContextoImp.Show("ATENDIO: "+nombrecajero.ToUpper());	
			ContextoImp.MoveTo(514.7, 425);				ContextoImp.Show(apagar.ToString("C").PadLeft(10)); 
			ContextoImp.MoveTo(515, 425);				ContextoImp.Show(apagar.ToString("C").PadLeft(10)); 
			filas-=10;
			ContextoImp.ShowPage();
		}
		
		public string traduce_numeros (string sNumero) {
			double dNumero;
			double dNumAux = 0;
			char x;
			string sAux;
			
			sResultado = " ";
			try {
				dNumero = Convert.ToDouble (sNumero);
			}
			catch {				
				return "";
			}
 
			if (dNumero > 999999999999)
				return "";
 
			if (dNumero > 999999999) {
				dNumAux = dNumero % 1000000000000;
				sResultado += Numeros (dNumAux, 1000000000) + " mil ";
			}
 
			if (dNumero > 999999) {
				dNumAux = dNumero % 1000000000;
				sResultado += Numeros (dNumAux, 1000000) + " millones ";
			}
 
			if (dNumero > 999) {
				dNumAux = dNumero % 1000000;
				sResultado += Numeros (dNumAux, 1000) + " mil ";
			}
 
			dNumAux = dNumero % 1000;	
			sResultado += Numeros (dNumAux, 1);
  
			//Enseguida verificamos si contiene punto, si es así, los convertimos a texto.
			sAux = dNumero.ToString();
 
			if (sAux.IndexOf(".") >= 0)
				sResultado += ObtenerDecimales (sNumero);
			//Las siguientes líneas convierten el primer caracter a mayúscula.
			sAux = sResultado;
			x = char.ToUpper (sResultado[1]);
			sResultado = x.ToString ();
 
			for (int i = 2; i<sAux.Length; i++)
				sResultado += sAux[i].ToString();
 
			return sResultado;
		}
		 
		public string ConvertirCadena (double dNumero) {
			double dNumAux = 0;
			char x;
			string sAux;
			 
			sResultado = " ";
 
			if (dNumero > 999999999999)
				return "";
 
			if (dNumero > 999999999) {
				dNumAux = dNumero % 1000000000000;
				sResultado += Numeros (dNumAux, 1000000000) + " mil ";
			}
 
			if (dNumero > 999999) {
				dNumAux = dNumero % 1000000000;
				sResultado += Numeros (dNumAux, 1000000) + " millones ";
			}
 
			if (dNumero > 999) {
				dNumAux = dNumero % 1000000;
				sResultado += Numeros (dNumAux, 1000) + " mil ";
			}
 
			dNumAux = dNumero % 1000;	
			sResultado += Numeros (dNumAux, 1);
 
 
			//Enseguida verificamos si contiene punto, si es así, los convertimos a texto.
			sAux = dNumero.ToString();
 
			if (sAux.IndexOf(".") >= 0)
				sResultado += ObtenerDecimales (sAux);
 
			//Las siguientes líneas convierten el primer caracter a mayúscula.
			sAux = sResultado;
			x = char.ToUpper (sResultado[1]);
			sResultado = x.ToString ();
 
			for (int i = 2; i<sAux.Length; i++)
				sResultado += sAux[i].ToString();
 
			return sResultado;
		}
 
		private string Numeros (double dNumAux, double dFactor) {
			double dCociente = dNumAux / dFactor;
			double dNumero = 0;
			int iNumero = 0;
			string sNumero = "";
			string sTexto = "";
 
			if (dCociente >= 100){
				dNumero = dCociente / 100;
				sNumero = dNumero.ToString();
				iNumero = int.Parse (sNumero[0].ToString());
				sTexto  +=  this.sCentenas [iNumero] + " ";
			}
 
			dCociente = dCociente % 100;
			if (dCociente >= 30){
				dNumero = dCociente / 10;			
				sNumero = dNumero.ToString();
				iNumero = int.Parse (sNumero[0].ToString());
				if (iNumero > 0)
					sTexto  += this.sDecenas [iNumero] + " ";
 
				dNumero = dCociente % 10;
				sNumero = dNumero.ToString();
				iNumero = int.Parse (sNumero[0].ToString());
				if (iNumero > 0)
					sTexto  += "y " + this.sUnidades [iNumero] + " ";
			}else{
				dNumero = dCociente;	
				sNumero = dNumero.ToString();
				if (sNumero.Length > 1)
					if (sNumero[1] != '.')
						iNumero = int.Parse (sNumero[0].ToString() + sNumero[1].ToString());
					else
						iNumero = int.Parse (sNumero[0].ToString());
				else
					iNumero = int.Parse (sNumero[0].ToString());
				sTexto  += this.sUnidades[iNumero] + " ";
			}
			return sTexto;
		}		

		private string ObtenerDecimales (string sNumero) {
			string[] sNumPuntos;
			string sTexto = "";
			double dNumero = 0;
			sNumPuntos = sNumero.Split('.');
    		dNumero = Convert.ToDouble(sNumPuntos[1]);
			sTexto = "peso "+dNumero.ToString().Trim()+"/100 M.N."; 
			//sTexto = "peso con " + Numeros(dNumero,1);
			return sTexto;
		}
	}
}
*/