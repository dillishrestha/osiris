// rpt_compras_farmacia.cs created with MonoDevelop
// User: jbuentello at 10:54 a 15/08/2008
// Sistema Hospitalario Osiris
// Monterrey - Mexico
//
// Autor    	: Ing. Jesus Buentello Garza (Programacion)
//			  	  Ing. Daniel Olivares C. (Adecuaciones y mejoras) arcangeldoc@gmail.com 18/02/2011
//				  Traspaso a GTKprint+Pango+Cairo
//				 
// 				  
// Licencia		: GLP
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
// Programa		: rpt_compras_farmacia.cs
// Proposito	: Reportes de compras farmacia
// Objeto		: 
//////////////////////////////////////////////////////////	

using System;
using Gtk;
using Npgsql;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class rpt_compras_farmacia
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		//ventana de Reporte 
		[Widget] Gtk.Window reporte_farmacia;
		[Widget] Gtk.TreeView lista_compra_farmacia;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_imprimir;
		[Widget] Gtk.Entry entry_dia1;
		[Widget] Gtk.Entry entry_mes1;
		[Widget] Gtk.Entry entry_ano1;
		[Widget] Gtk.Entry entry_dia2;
		[Widget] Gtk.Entry entry_mes2;
		[Widget] Gtk.Entry entry_ano2;
		
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 70;
		int separacion_linea = 10;
		int numpage = 1;
		
		string query_fechas = " ";
			
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		string connectionString;
		
		private TreeStore treeViewEngineFarmacia;
	
		//Declaracion de ventana de error y pregunta
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public rpt_compras_farmacia(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "reporte_farmacia", null);
			gxml.Autoconnect (this);        
			////// Muestra ventana de Glade
			reporte_farmacia.Show();
			
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
			
			this.button_imprimir.Clicked += new EventHandler(on_imprime_reporte_clicked);
			
			crea_treview_compra_farmacia();
			this.button_selecciona.Clicked += new EventHandler(on_llena_lista_compra_farmacia_clicked );
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
				
		}
		
		void crea_treview_compra_farmacia()
		{
			treeViewEngineFarmacia = new TreeStore(typeof(string),
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
													
			this.lista_compra_farmacia.Model = treeViewEngineFarmacia;
			this.lista_compra_farmacia.RulesHint = true;
			
			//lista_compra_farmacia.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono paciente*/
				
			TreeViewColumn col_folio = new TreeViewColumn();
			CellRendererText cel_folio = new CellRendererText();
			col_folio.Title = "Folio Atencion";
			col_folio.PackStart(cel_folio, true);
			col_folio.AddAttribute (cel_folio, "text", 0);
			col_folio.SortColumnId = (int) Column.col_folio;		
			
			TreeViewColumn col_orden_compra = new TreeViewColumn();
			CellRendererText cel_orden_compra = new CellRendererText();
			col_orden_compra.Title = "Orden Compra";
			col_orden_compra.PackStart(cel_orden_compra, true);
			col_orden_compra.AddAttribute(cel_orden_compra, "text", 1);
			col_orden_compra.Resizable = true;
			col_orden_compra.SortColumnId = (int) Column.col_orden_compra;
			
			TreeViewColumn col_codigo_producto = new TreeViewColumn();
			CellRendererText cel_codigo_producto = new CellRendererText();
			col_codigo_producto.Title = "CodProducto";
			col_codigo_producto.PackStart(cel_codigo_producto, true);
			col_codigo_producto.AddAttribute(cel_codigo_producto, "text", 2);
			col_codigo_producto.SortColumnId = (int) Column.col_codigo_producto;
			
			TreeViewColumn col_descripcion = new TreeViewColumn();
			CellRendererText cel_descripcion = new CellRendererText();
			col_descripcion.Title = "Descripcion";
			col_descripcion.PackStart(cel_descripcion, true);
			col_descripcion.AddAttribute(cel_descripcion, "text", 3);
			col_descripcion.Resizable = true;
			cel_descripcion.Width = 350;
			col_descripcion.SortColumnId = (int) Column.col_descripcion;
		
			TreeViewColumn col_costo = new TreeViewColumn();
			CellRendererText cel_costo = new CellRendererText();
			col_costo.Title = "Costo Uni";
			col_costo.PackStart(cel_costo, true);
			col_costo.AddAttribute(cel_costo, "text", 4);
			
			TreeViewColumn col_precio_producto = new TreeViewColumn();
			CellRendererText cel_precio_producto = new CellRendererText();
			col_precio_producto.Title = "Precio Producto";
			col_precio_producto.PackStart(cel_precio_producto, true);
			col_precio_producto.AddAttribute(cel_precio_producto, "text", 5);
			
			TreeViewColumn col_fecha = new TreeViewColumn();
			CellRendererText cel_fecha = new CellRendererText();
			col_fecha.Title = "Fecha";
			col_fecha.PackStart(cel_fecha, true);
			col_fecha.AddAttribute(cel_fecha, "text", 6);
			col_fecha.SortColumnId = (int) Column.col_fecha;
			
			TreeViewColumn col_surtir = new TreeViewColumn();
			CellRendererText cel_surtir = new CellRendererText();
			col_surtir.Title = "Surtir";
			col_surtir.PackStart(cel_surtir, true);
			col_surtir.AddAttribute (cel_surtir, "text", 7);

			
			TreeViewColumn col_embalaje = new TreeViewColumn();
			CellRendererText cel_embalaje = new CellRendererText();
			col_embalaje.Title = "Embalaje";
			col_embalaje.PackStart(cel_embalaje, true);
			col_embalaje.AddAttribute(cel_embalaje, "text", 8);
			col_embalaje.Resizable = true;
			
			
			TreeViewColumn col_autorizo = new TreeViewColumn();
			CellRendererText cel_autorizo = new CellRendererText();
			col_autorizo.Title = "Autorizo";
			col_autorizo.PackStart(cel_codigo_producto, true);
			col_autorizo.AddAttribute(cel_codigo_producto, "text", 9);

			
			TreeViewColumn col_ganancia = new TreeViewColumn();
			CellRendererText cel_ganancia = new CellRendererText();
			col_ganancia.Title = "%Ganancia";
			col_ganancia.PackStart(cel_ganancia, true);
			col_ganancia.AddAttribute(cel_ganancia, "text", 10);
		
			TreeViewColumn col_subalmacen = new TreeViewColumn();
			CellRendererText cel_subalmacen = new CellRendererText();
			col_subalmacen.Title = "SubAlmacen";
			col_subalmacen.PackStart(cel_subalmacen, true);
			col_subalmacen.AddAttribute(cel_subalmacen, "text", 11);
			
			TreeViewColumn col_compro = new TreeViewColumn();
			CellRendererText cel_compro = new CellRendererText();
			col_compro.Title = "Compro";
			col_compro.PackStart(cel_precio_producto, true);
			col_compro.AddAttribute(cel_precio_producto, "text", 12);
			
			TreeViewColumn col_medico = new TreeViewColumn();
			CellRendererText cel_medico = new CellRendererText();
			col_medico.Title = "Medico";
			col_medico.PackStart(cel_medico, true);
			col_medico.AddAttribute(cel_medico, "text", 13);
			col_medico.SortColumnId = (int) Column.col_medico;				
		
			lista_compra_farmacia.AppendColumn(col_folio);
			lista_compra_farmacia.AppendColumn(col_orden_compra);
			lista_compra_farmacia.AppendColumn(col_codigo_producto);
			lista_compra_farmacia.AppendColumn(col_descripcion);
			lista_compra_farmacia.AppendColumn(col_costo);
			lista_compra_farmacia.AppendColumn(col_precio_producto);
			lista_compra_farmacia.AppendColumn(col_fecha);
			lista_compra_farmacia.AppendColumn(col_surtir);
			lista_compra_farmacia.AppendColumn(col_embalaje);
			lista_compra_farmacia.AppendColumn(col_autorizo);
			lista_compra_farmacia.AppendColumn(col_ganancia);
			lista_compra_farmacia.AppendColumn(col_subalmacen);
			lista_compra_farmacia.AppendColumn(col_compro);
			lista_compra_farmacia.AppendColumn(col_medico);		
			
		}
		
			enum Column
		{
			col_folio,
			col_orden_compra,
			col_codigo_producto,
			col_descripcion,
			col_fecha,
			col_medico
		}
		
		
		
		void on_llena_lista_compra_farmacia_clicked (object sender, EventArgs args)
 		{
 			llenando_lista_compra_farmacia();
 		}
		
		void llenando_lista_compra_farmacia()
 		{
 			this.treeViewEngineFarmacia.Clear();
			
			query_fechas = "AND to_char(osiris_erp_compra_farmacia.fechahora_autorizacion,'yyyy-MM-dd') >= '"+entry_ano1.Text+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
				           "AND to_char(osiris_erp_compra_farmacia.fechahora_autorizacion,'yyyy-MM-dd') <= '"+entry_ano2.Text+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";			
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				comando.CommandText = "SELECT to_char(osiris_erp_compra_farmacia.folio_de_servicio,'999999') AS folioatencion,"+
									"to_char(osiris_erp_compra_farmacia.orden_compra,'99999999') AS ordencompra,"+
									"to_char(osiris_erp_compra_farmacia.id_producto,'999999999999') AS codproducto,"+
					                "osiris_productos.descripcion_producto,"+
									"to_char(osiris_erp_compra_farmacia.costo_por_unidad,'9999999999.99') AS costoporunidad,"+
									"to_char(osiris_erp_compra_farmacia.precio_producto_publico,'9999999999.99') AS preciopublico,"+
									"to_char(osiris_erp_compra_farmacia.total_surtir,'999999.99') AS surtir,"+
									"to_char(osiris_erp_compra_farmacia.cantidad_embalaje,'999999.99') AS embalaje,"+
									"to_char(osiris_erp_compra_farmacia.cantidad_autorizo,'9999.99') AS autorizo,"+
									"to_char(osiris_erp_compra_farmacia.porcentage_ganancia,'999.99') AS porcentageganancia,"+
									"to_char(osiris_erp_compra_farmacia.fechahora_autorizacion,'dd-MM-yyyy') AS fechahrautorizacion,"+ 
									"osiris_erp_compra_farmacia.id_subalmacen,"+
									"osiris_erp_compra_farmacia.id_quien_compro,"+
									"osiris_erp_compra_farmacia.id_proveedor,"+
									"osiris_erp_compra_farmacia.id_medico,"+
						            "osiris_his_medicos.nombre_medico,"+
						            "to_char(osiris_erp_compra_farmacia.costo_producto,'9999999.999') AS costo_prod,"+
						            "osiris_almacenes.descripcion_almacen "+ 	
                                    "FROM osiris_erp_compra_farmacia,osiris_productos,osiris_his_medicos,osiris_almacenes "+ 
									"WHERE osiris_erp_compra_farmacia.id_producto = osiris_productos.id_producto "+ 
						            " "+query_fechas+" "+
									"AND osiris_erp_compra_farmacia.id_producto = osiris_productos.id_producto "+
						            "AND osiris_erp_compra_farmacia.id_medico = osiris_his_medicos.id_medico "+
						            "AND osiris_erp_compra_farmacia.id_subalmacen = osiris_almacenes.id_almacen "+
									"ORDER BY orden_compra;";
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
						
				while (lector.Read()){
						this.treeViewEngineFarmacia.AppendValues((string) lector["folioatencion"],
					                                         (string) lector["ordencompra"],
					                                         (string) lector["codproducto"],
					                                         (string) lector["descripcion_producto"],
					                                         (string) lector["costoporunidad"],
					                                         (string) lector["preciopublico"],
					                                         (string) lector["fechahrautorizacion"],
					                                         (string) lector["surtir"],
					                                         (string) lector["embalaje"],
					                                         (string) lector["autorizo"],
					                                         (string) lector["porcentageganancia"],
					                                         (string) lector["descripcion_almacen"],
					                                         (string) lector["id_quien_compro"],
					                                         (string) lector["nombre_medico"]); 
					

				}
				
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
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
		
		void on_imprime_reporte_clicked (object sender, EventArgs args)
		{
			print = new PrintOperation ();
			print.JobName = "Reporte de Farmacia";	// Name of the report
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run(PrintOperationAction.PrintDialog, null);
		}
		
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1;  // crea cantidad de copias del reporte			
			// para imprimir horizontalmente el reporte
			print.PrintSettings.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{			
			PrintContext context = args.Context;			
			ejecutar_consulta_reporte(context);
		}
						
		void ejecutar_consulta_reporte(PrintContext context)
		{
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			TreeIter iter;
			string toma_descrip_prod;
			string toma_descrip_alm;			
			if (this.treeViewEngineFarmacia.GetIterFirst (out iter)){
				imprime_encabezado(cr,layout);
				while (this.treeViewEngineFarmacia.IterNext(ref iter)){
					
					toma_descrip_alm = (string) this.treeViewEngineFarmacia.GetValue (iter,11);					
					if(toma_descrip_alm.Length > 22){
						toma_descrip_alm = toma_descrip_alm.Substring(0,21);					
					}  
					
					toma_descrip_alm = (string) this.treeViewEngineFarmacia.GetValue (iter,11);					
					if(toma_descrip_alm.Length > 22){
						toma_descrip_alm = toma_descrip_alm.Substring(0,21);
					}
				}	
			}
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
		{
			//Gtk.Image image5 = new Gtk.Image();
            //image5.Name = "image5";
			//image5.Pixbuf = new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "osiris.jpg"));
			//image5.Pixbuf = new Gdk.Pixbuf("/opt/osiris/bin/OSIRISLogo.jpg");   // en Linux
			//image5.Pixbuf.ScaleSimple(128, 128, Gdk.InterpType.Bilinear);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf,1,-30);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(145, 50, Gdk.InterpType.Bilinear),1,1);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(180, 64, Gdk.InterpType.Hyper),1,1);
			//cr.Fill();
			//cr.Paint();
			//cr.Restore();
								
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
			cr.MoveTo(650*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(650*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page :"+numpage.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 10.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(240*escala_en_linux_windows, 25*escala_en_linux_windows);			layout.SetText("REPORTE OCUPACION HOSPITALARIA");					Pango.CairoHelper.ShowLayout (cr, layout);
						
			// Creando el Cuadro de Titulos
			cr.Rectangle (05*escala_en_linux_windows, 50*escala_en_linux_windows, 750*escala_en_linux_windows, 15*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.5;
			cr.Stroke();
			
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
					
			cr.MoveTo(09*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Folio.");			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(74*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Orden");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(114*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Codigo");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(300*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Descripcion");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("CostoUni");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(480*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Fecha");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Surtir");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Autoz");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("%Gana");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("SubAlmacen");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Compro");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(570*escala_en_linux_windows,53*escala_en_linux_windows);			layout.SetText("Medico");	Pango.CairoHelper.ShowLayout (cr, layout);
			
			layout.FontDescription.Weight = Weight.Normal;		// Letra Normal
		}
		
		void salto_de_pagina(Cairo.Context cr,Pango.Layout layout)			
		{
			if(comienzo_linea >530){
				cr.ShowPage();
				Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
				fontSize = 8.0;		desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				comienzo_linea = 70;
				numpage += 1;
				imprime_encabezado(cr,layout);
			}
		}
			
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)	
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}	
	}
}