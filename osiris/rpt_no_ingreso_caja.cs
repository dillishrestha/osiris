// created on 07/02/2008 at 09:34 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    : Ing. Daniel Olivares C. (Adecuaciones y mejoras) arcangeldoc@gmail.com 03/06/2010
// Ing Mauro I Villanueva Z 20/03/2012
//   Traspaso a GTKprint y la creacion de la clase
// Licencia : GLP
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
// Programa :
// Proposito :
// Objeto :
//////////////////////////////////////////////////////

using System;
using Gtk;
using Npgsql;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class rpt_no_ingreso_caja
	{
		//declarando la ventana de rango de fechas
		[Widget] Gtk.Window rango_de_fecha;
		[Widget] Gtk.Entry entry_dia1;
		[Widget] Gtk.Entry entry_mes1;
		[Widget] Gtk.Entry entry_ano1;
		[Widget] Gtk.Entry entry_dia2;
		[Widget] Gtk.Entry entry_mes2;
		[Widget] Gtk.Entry entry_ano2;
		[Widget] Gtk.CheckButton  checkbutton_impr_todo_proce;
		[Widget] Gtk.CheckButton checkbutton_todos_los_clientes;
		[Widget] Gtk.Entry entry_referencia_inicial;
		[Widget] Gtk.Button button_imprime_rangofecha;
		[Widget] Gtk.Button button_salir;
		
		protected Gtk.Window MyWinError;
		
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows; // Linux = 1  Windows = 8
		int comienzo_linea = 95;
		int separacion_linea = 10;
		int numerpage = 1;
		int contador = 0;
		string connectionString;
		string nombrebd;
		string tipointernamiento = "CENTRO MEDICO";
		int idtipointernamiento = 10;
		string tipopaciente = "Membresias";
		int id_tipopaciente = 100;
		string motivo = "";
		//string tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";
		string busqueda = "";
		
		string idempresa = "1";
		string idaseguradora = "1";
		string idmedico = "1";
		   
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();
		                              
		/// <summary>
		/// Genera reporte inteligente de consulta
		/// </summary>
		/// <param name="nombrebd_">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="tipo_reporte impresora o archivo">
		/// A <see cref="System.String"/>
		/// </param>
		public rpt_no_ingreso_caja (string nombrebd_,string tipo_de_salida_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			//tipo_de_salida = tipo_de_salida_;
			//crea la ventana de glade
			Glade.XML  gxml = new Glade.XML  (null, "caja.glade", "rango_de_fecha", null);
			gxml.Autoconnect  (this);
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
			//facturados = "CERRADOS";
			
			button_imprime_rangofecha.Clicked += new EventHandler(on_button_rpt_print_report_noingresa_caja);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			//llenado_combobox();
		}
		
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked(object sender, EventArgs a)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		void on_button_rpt_print_report_noingresa_caja(object sender, EventArgs a)
		{
			print = new PrintOperation ();
			print.JobName = "Reporte de paciente ingresado que no pasaron por caja";
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);
		}
		
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1;  // crea cantidad de copias del reporte
			// para imprimir horizontalmente el reporte
			//print.PrintSettings.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{
			PrintContext context = args.Context;
			//imprime_encabezado(cr,layou);
			ejecutar_consulta_reporte(context);
		}
		
		void ejecutar_consulta_reporte(PrintContext context)
		{
			string fechas_registros = "";
			string edad;
			int contador = 0;
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");
			fontSize = 7.0; layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale); layout.FontDescription = desc;
			
			comienzo_linea = 85;
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			           
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando;
				comando = conexion.CreateCommand ();
				            
				int folioservicio = 0;
				string query_fechas = " AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+entry_ano1.Text+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
									" AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+entry_ano2.Text+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";								
				comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente " +
							"FROM osiris_erp_cobros_enca,osiris_his_paciente " +
							"WHERE osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente"+query_fechas+" ORDER BY osiris_erp_cobros_enca.folio_de_servicio;";
				Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){
					folioservicio = (int) lector["folio_de_servicio"];
					//Console.WriteLine(folioservicio.ToString());
					if((string) classpublic.lee_registro_de_tabla("osiris_erp_comprobante_servicio","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"'","folio_de_servicio") == ""){
						Console.WriteLine(folioservicio.ToString().Trim()+" NO Tiene Comprobante de Servicio ");
						if((string) classpublic.lee_registro_de_tabla("osiris_erp_abonos","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"' AND honorario_medico = 'false' ","folio_de_servicio") == ""){
							Console.WriteLine(folioservicio.ToString().Trim()+" NO Tiene Comprobante de PAGO o ABONO ");
							if((string) classpublic.lee_registro_de_tabla("osiris_erp_comprobante_pagare","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"' ","folio_de_servicio") == ""){
								Console.WriteLine(folioservicio.ToString().Trim()+" NO Tiene Comprobante de PAGARE ");
								if((string) classpublic.lee_registro_de_tabla("osiris_erp_pases_qxurg","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"' ","folio_de_servicio") == ""){
									Console.WriteLine(folioservicio.ToString().Trim()+" NO Tiene PASE QX/URGENCIA ");
								}
							}
						}					
					}
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		//de rangos de fechas
		[GLib.ConnectBefore ()]     // Esto es indispensable para que funcione   
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace){
				args.RetVal = true;
			}
		}
	}
}

