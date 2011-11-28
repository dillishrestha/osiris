// created on 07/11/2011
//////////////////////////////////////////////////////////
// created on 21/06/2007 at 01:40 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
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
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class pases_a_quirofano
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 05;
		int separacion_linea = 10;
		int numpage = 1;
		PrintContext context;
		
		
		// Declarando variable publicas
		string connectionString;
		string nombrebd;
		
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		//[Widget] Gtk.Entry entry_fecha_inicio;
		[Widget] Gtk.Window envio_almacenes = null;
		[Widget] Gtk.Entry entry_dia_inicio = null;
		[Widget] Gtk.Entry entry_mes_inicio = null;
		[Widget] Gtk.Entry entry_ano_inicio = null;
		[Widget] Gtk.Entry entry_dia_termino = null;
		[Widget] Gtk.Entry entry_mes_termino = null;
		[Widget] Gtk.Entry entry_ano_termino = null;
		[Widget] Gtk.HBox hbox1 = null;
		[Widget] Gtk.HBox hbox2 = null;
		[Widget] Gtk.Label label262;
		[Widget] Gtk.CheckButton checkbutton_todos_envios = null;
		[Widget] Gtk.CheckButton checkbutton_seleccion_presupuestos = null;
		[Widget] Gtk.Button button_rep = null;
		[Widget] Gtk.TreeView lista_almacenes = null;
		
		TreeStore treeViewEnginePases;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();

		public pases_a_quirofano (int pid_paciente_,int folioservicio_,int idcentro_costo_,string LoginEmpleado_,
		                          int id_tipopaciente_,int idempresa_paciente_,int idaseguradora_paciente)
		{
			escala_en_linux_windows = classpublic.escala_linux_windows;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "envio_almacenes", null);
			gxml.Autoconnect (this);
			
			entry_dia_inicio.Text = DateTime.Now.ToString("dd");
			entry_mes_inicio.Text = DateTime.Now.ToString("MM");
			entry_ano_inicio.Text = DateTime.Now.ToString("yyyy");			
			entry_dia_termino.Sensitive = false;
			entry_mes_termino.Sensitive = false;
			entry_ano_termino.Sensitive = false;
			entry_dia_inicio.IsEditable = false;
			entry_mes_inicio.IsEditable = false;
			entry_ano_inicio.IsEditable = false;				
			hbox1.Hide();
			hbox2.Hide();
			checkbutton_seleccion_presupuestos.Hide();
			checkbutton_todos_envios.Label = "Nuevo Pase para QX./Urgencias";
			label262.Text = "Cancelar";
			envio_almacenes.Show();
			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			//button_buscar.Clicked += new EventHandler(on_buscar_clicked);
           	button_rep.Clicked += new EventHandler(on_printing_pase_qx_clcked);
          	checkbutton_todos_envios.Clicked += new EventHandler(on_create_pases_qxurg_clicked);
          	//checkbutton_seleccion_presupuestos.Hide();
			crea_treeview_pases();
			llenado_treeview_pases();
		}
		
		void crea_treeview_pases()
		{
			treeViewEnginePases = new TreeStore(typeof(string),
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
														 typeof(string),
														 typeof(string));
						
				lista_almacenes.Model = treeViewEnginePases;
				lista_almacenes.RulesHint = true;
				//lista_almacenes.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono paciente*/
					
				TreeViewColumn col_idproducto = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_idproducto.Title = "N° Pase";
				col_idproducto.PackStart(cellr0, true);
				col_idproducto.AddAttribute (cellr0, "text", 0);
				col_idproducto.SortColumnId = (int) Column_prod.col_idproducto;
			
				TreeViewColumn col_fechahora = new TreeViewColumn();
				CellRendererText cellr1 = new CellRendererText();
				col_fechahora.Title = "Fech. Hora";
				col_fechahora.PackStart(cellr1, true);
				col_fechahora.AddAttribute (cellr1, "text", 1);
				col_fechahora.SortColumnId = (int) Column_prod.col_fechahora;
				
				TreeViewColumn col_idquiencreo = new TreeViewColumn();
				CellRendererText cellr2 = new CellRendererText();
				col_idquiencreo.Title = "Quien creo";
				col_idquiencreo.PackStart(cellr2, true);
				col_idquiencreo.AddAttribute (cellr2, "text", 2);
				col_idquiencreo.SortColumnId = (int) Column_prod.col_idquiencreo;
			
				TreeViewColumn col_departamento = new TreeViewColumn();
				CellRendererText cellr3 = new CellRendererText();
				col_departamento.Title = "Departamento";
				col_departamento.PackStart(cellr3, true);
				col_departamento.AddAttribute (cellr3, "text", 3);
				col_departamento.SortColumnId = (int) Column_prod.col_departamento;
			
				lista_almacenes.AppendColumn(col_idproducto);  // 0
				lista_almacenes.AppendColumn(col_fechahora);  // 1
				lista_almacenes.AppendColumn(col_idquiencreo);  // 2
				lista_almacenes.AppendColumn(col_departamento);  // 3
		}
		
		//  lista_de_productos:
		enum Column_prod
		{
			col_idproducto,
			col_fechahora,
			col_idquiencreo,
			col_departamento
		}
		
		void llenado_treeview_pases()
		{
			
		}
		
		void on_create_pases_qxurg_clicked (object sender, EventArgs args)
		{
			if (checkbutton_todos_envios.Active == true){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Question,ButtonsType.YesNo,"Esta seguro de crear un pase para QX/Urgencias");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
			 	if (miResultado == ResponseType.Yes){
			 		checkbutton_todos_envios.Active = false;
					NpgsqlConnection conexion;
					conexion = new NpgsqlConnection (connectionString+nombrebd );
					// Verifica que la base de datos este conectada
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						comando.CommandText = "INSERT INTO ";
						
						//comando.ExecuteNonQuery();					comando.Dispose();
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();		msgBoxError.Destroy();				
					}
					conexion.Close();
			 	}else{
					checkbutton_todos_envios.Active = false;
				}
			}else{
				checkbutton_todos_envios.Active = false;
			}
		}
		
		void on_printing_pase_qx_clcked(object sender, EventArgs args)
		{
			print = new PrintOperation ();
			print.JobName = "Pase para Servicios Medicos";	// Name of the report
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
			context = args.Context;
			ejecutar_consulta_reporte(context);			
		}
		
		void ejecutar_consulta_reporte(PrintContext context)
		{
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			string sexopaciente = "";
			
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			
			imprime_encabezado(cr,layout);
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
		{
			//Gtk.Image image5 = new Gtk.Image();
            //image5.Name = "image5";
			//image5.Pixbuf = new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "osiris.jpg"));
			//image5.Pixbuf = new Gdk.Pixbuf("/opt/osiris/bin/OSIRISLogo.jpg");   // en Linux
			//---image5.Pixbuf.ScaleSimple(128, 128, Gdk.InterpType.Bilinear);
			//---Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf,1,-30);
			//---Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(145, 50, Gdk.InterpType.Bilinear),1,1);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(180, 64, Gdk.InterpType.Hyper),1,1);
			//cr.Fill();
			//cr.Paint();
			//cr.Restore();
			
			comienzo_linea = 60;
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText(classpublic.nombre_empresa);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText(classpublic.direccion_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,25*escala_en_linux_windows);			layout.SetText(classpublic.telefonofax_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(479*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(479*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page :"+numpage.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);

			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 10.0;		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			layout.Alignment = Pango.Alignment.Center;
			
			double width = context.Width;
			layout.Width = (int) width;
			layout.Alignment = Pango.Alignment.Center;
			//layout.Wrap = Pango.WrapMode.Word;
			//layout.SingleParagraphMode = true;
			layout.Justify =  false;
			cr.MoveTo(width/2,45*escala_en_linux_windows);	layout.SetText("PASE_A_SERVICIO_MEDICO");	Pango.CairoHelper.ShowLayout (cr, layout);
			
			fontSize = 9.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita						
			
			cr.MoveTo(565*escala_en_linux_windows, 383*escala_en_linux_windows);
			cr.LineTo(05,383);		// Linea Horizontal 4
			
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.1;
			cr.Stroke();
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}

