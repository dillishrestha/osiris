///////////////////////////////////////////////////////////
// project created on 23/10/2008 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// 				: Israel Peña Gonzalez
// Autor    	: Ing. Daniel Olivares C. cambio a GTKPrint con Pango y Cairo arcangeldoc@gmail.com 18/11/2010
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
using Gtk;
using Npgsql;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class rpt_orden_compras
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 162;
		int separacion_linea = 10;
		int numpage = 1;
		
		string connectionString;						
		string nombrebd;
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
		int num_ordencopmpra;
		
		string titulo = "ORDEN DE COMPRAS ";
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public rpt_orden_compras(int num_ordencopmpra_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			num_ordencopmpra = num_ordencopmpra_;
			print = new PrintOperation ();
			print.JobName = titulo;
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);			
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
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			
			
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
        	// Verifica que la base de datos este conectada
			try{				
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT numero_orden_compra,osiris_erp_ordenes_compras_enca.id_proveedor,osiris_erp_ordenes_compras_enca.descripcion_proveedor," +
					"osiris_erp_ordenes_compras_enca.direccion_proveedor, " +
					"osiris_erp_ordenes_compras_enca.rfc_proveedor,osiris_erp_ordenes_compras_enca.telefonos_proveedor,osiris_erp_ordenes_compras_enca.fecha_de_entrega,osiris_erp_ordenes_compras_enca.lugar_de_entrega,osiris_erp_ordenes_compras_enca.condiciones_de_pago,osiris_erp_ordenes_compras_enca.dep_solicitante,osiris_erp_ordenes_compras_enca.observaciones,osiris_erp_ordenes_compras_enca.fecha_deorden_compra " +
					"  " +
					"FROM osiris_erp_ordenes_compras_enca, osiris_erp_proveedores  " +
					"WHERE osiris_erp_ordenes_compras_enca.id_proveedor = osiris_erp_proveedores.id_proveedor " +
					"AND id_orden_compra = '"+num_ordencopmpra.ToString().Trim()+"';";
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if (lector.Read()){
					Console.WriteLine((string) lector["descripcion_proveedor"].ToString().Trim());
					Console.WriteLine((string) lector["direccion_proveedor"].ToString().Trim());
					Console.WriteLine((string) lector["rfc_proveedor"].ToString().Trim());
					Console.WriteLine((string) lector["telefonos_proveedor"].ToString().Trim());
					Console.WriteLine((string) lector["lugar_de_entrega"].ToString().Trim());
					Console.WriteLine((string) lector["dep_solicitante"].ToString().Trim());
					Console.WriteLine((string) lector["observaciones"].ToString().Trim());
					Console.WriteLine((string) lector["fecha_deorden_compra"].ToString().Trim());
					Console.WriteLine((string) lector["numero_orden_compra"].ToString().Trim());
					Console.WriteLine((string) lector["fecha_de_entrega"].ToString().Trim());
					Console.WriteLine((string) lector["condiciones_de_pago"].ToString().Trim());
					imprime_encabezado(cr,layout,
						(string) lector["descripcion_proveedor"].ToString().Trim(),
						(string) lector["direccion_proveedor"].ToString().Trim(),
						(string) lector["rfc_proveedor"].ToString().Trim(),
						(string) lector["telefonos_proveedor"].ToString().Trim(),
						(string) lector["lugar_de_entrega"].ToString().Trim(),
					    (string) lector["condiciones_de_pago"].ToString().Trim(),
						(string) lector["dep_solicitante"].ToString().Trim(),
						(string) lector["observaciones"].ToString().Trim(),
						(string) lector["fecha_deorden_compra"].ToString().Trim(),
						(string) lector["numero_orden_compra"].ToString().Trim(),
						(string) lector["fecha_de_entrega"].ToString().Trim());
				}
			}catch(NpgsqlException ex){
			
				Console.WriteLine("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
		}
		
				
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout,
			                    string descripcion_proveedor_,
					           	string direccion_proveedor_,
						        string rfc_proveedor_,
					        	string telefonos_proveedor_,
				         		string lugar_de_entrega_,
				        	    string condiciones_de_pago_,
					            string dep_solicitante_,
					        	string observaciones_,
					        	string fecha_deorden_compra_,
					        	string numero_orden_compra_,
					        	string fecha_de_entrega_)
			
		{
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
			cr.MoveTo(290*escala_en_linux_windows, 25*escala_en_linux_windows);			layout.SetText("ORDEN_DE_COMPRAS");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			cr.MoveTo(07*escala_en_linux_windows,62*escala_en_linux_windows);			layout.SetText("Descripcion_Proveedor :"+descripcion_proveedor_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows,75*escala_en_linux_windows);			layout.SetText("Direccion_Proveedor :"+direccion_proveedor_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows,90*escala_en_linux_windows);			layout.SetText("R.F.C_Proveedor:"+rfc_proveedor_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(250*escala_en_linux_windows,90*escala_en_linux_windows);			layout.SetText("Telefonos_proveedor:"+telefonos_proveedor_);	Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(555*escala_en_linux_windows,60*escala_en_linux_windows);			layout.SetText("Fech. Orden Compra :");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(655*escala_en_linux_windows,60*escala_en_linux_windows);			layout.SetText("Num. Orden Compra :");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows,100*escala_en_linux_windows);			layout.SetText("Lugar de Entrega :");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(250*escala_en_linux_windows,100*escala_en_linux_windows);			layout.SetText("Dep. Solicitante :");	Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(555*escala_en_linux_windows,100*escala_en_linux_windows);			layout.SetText("Fecha de Entrega :"+fecha_de_entrega_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows,120*escala_en_linux_windows);			layout.SetText("Observaciones :");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(555*escala_en_linux_windows,120*escala_en_linux_windows);			layout.SetText("Cond. de Pago :");	Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(07*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("N°");							Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("Part.");						Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(07*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("100");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(27*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("Cantid.");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(27*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText(" Soli.");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(27*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("1000.00");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(60*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("Unidad de");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(60*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("Medida");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(60*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("PIEZA");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(102*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("Empaque");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(102*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("Produc.");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(102*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("1000.00");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(140*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("Descripcion del");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(140*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("Producto");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(140*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("BOLSA RECOLECTORA DE ORINA UROTEK DE 2 LTS.");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(600*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("PRECIO");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(660*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("IVA");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(710*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("TOTAL");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(600*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("1000.00");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows, 590*escala_en_linux_windows);			layout.SetText("CALIDAD: El comprador tendra el derecho de inspeccionar antes de aceptar la mercancia.");					Pango.CairoHelper.ShowLayout (cr, layout);	
			cr.MoveTo(05*escala_en_linux_windows, 605*escala_en_linux_windows);			layout.SetText("PRECIO: El Proveedor facturará a precios y terminos de la Orden de Compra");					Pango.CairoHelper.ShowLayout (cr, layout);	
			
			

			cr.MoveTo(55*escala_en_linux_windows, 545*escala_en_linux_windows);			layout.SetText("Firma Autorizado");					Pango.CairoHelper.ShowLayout (cr, layout);	
			
			cr.MoveTo(05*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(05,480);		// vertical 1
			
			cr.MoveTo(750*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(750,500);		// vertical 2
			
			cr.MoveTo(550*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(550,140);		// vertical 3
			
			cr.MoveTo(650*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(650,100);		// vertical 4
			
			cr.MoveTo(25*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(25,480);		// vertical 5
			
			cr.MoveTo(57*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(57,480);		// vertical 6
			
			cr.MoveTo(100*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(100,480);		// vertical 7
			
			cr.MoveTo(138*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(138,480);		// vertical 8
			
			cr.MoveTo(585*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(585,500);		// vertical 11
			
			cr.MoveTo(640*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(640,500);		// vertical 12
			
			cr.MoveTo(695*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(695,500);		// vertical 13
			
			
	
			
			cr.MoveTo(750*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(05,60);		// Linea Horizontal 1
			
			cr.MoveTo(750*escala_en_linux_windows, 100*escala_en_linux_windows);
			cr.LineTo(05,100);		// Linea Horizontal 2
			
			cr.MoveTo(750*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(05,140);		// Linea Horizontal 3
			
			cr.MoveTo(750*escala_en_linux_windows, 160*escala_en_linux_windows);
			cr.LineTo(05,160);		// Linea Horizontal 4
			
			cr.MoveTo(750*escala_en_linux_windows, 500*escala_en_linux_windows);
			cr.LineTo(585,500);		// Linea Horizontal 6
			
			cr.MoveTo(750*escala_en_linux_windows, 120*escala_en_linux_windows);
			cr.LineTo(550,120);		// Linea Hrizontal 7
			
			cr.MoveTo(750*escala_en_linux_windows, 480*escala_en_linux_windows);
			cr.LineTo(585,480);		// Linea Horizontal 8
			
			cr.MoveTo(05*escala_en_linux_windows, 480*escala_en_linux_windows);
			cr.LineTo(590,480);		// Linea Horizontal 9
			
			cr.MoveTo(45*escala_en_linux_windows, 545*escala_en_linux_windows);	
			cr.LineTo(125,545);		// Linea Horizontal 10
			
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.3;
			cr.Stroke();
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
	}	
}

/*
	public class rpt_orden_compras
	{
		//private static int pangoScale = 1024;
		//private PrintOperation print;
		//private double fontSize = 8.0;
		
		string connectionString;						
		string nombrebd;
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
		
		// Declarando el treeview
		Gtk.TreeView lista_productos_a_comprar;
		Gtk.TreeStore treeViewEngineProductosaComprar;
		
		string titulo = "ORDEN DE COMPRAS ";
		
		int contador = 1;
		int numpage = 1;
		int filas =-174;
		
		//Declaracion de ventana de error:
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public rpt_orden_compras()
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			//nombrebd = conexion_a_DB._nombrebd;
			//lista_productos_a_comprar = lista_productos_a_comprar_ as Gtk.TreeView;
			//treeViewEngineProductosaComprar = treeViewEngineProductosaComprar_ as Gtk.TreeStore; 
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob(Gnome.PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog(trabajo, "ORDEN DE COMPRAS", 0);
        	
        	int respuesta = dialogo.Run ();
		        	
        	if (respuesta == (int) PrintButtons.Cancel){
				dialogo.Hide (); 
				dialogo.Dispose (); 
				return;
			}
			Gnome.PrintContext ctx = trabajo.Context;
        	ComponerPagina(ctx, trabajo); 
			trabajo.Close();
            switch (respuesta){
				case (int) PrintButtons.Print:   
                	trabajo.Print (); 
                	break;
                case (int) PrintButtons.Preview:
                	new PrintJobPreview(trabajo, "ORDEN DE COMPRAS").Show();
                	break;
        	}
			dialogo.Hide (); dialogo.Dispose ();		
			
			//print = new PrintOperation ();			
			//print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			//print.DrawPage += new DrawPageHandler (OnDrawPage);
			//print.EndPrint += new EndPrintHandler (OnEndPrint);
			//print.Run (PrintOperationAction.PrintDialog, null);			
		}
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			// Crear una fuente 
			//Gnome.Font fuente = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
			//Gnome.Font fuente1 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
			Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
			//Gnome.Font fuente3 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
			Gnome.Font fuente5 = Gnome.Font.FindClosestFromWeightSlant("Bitstream Vera Sans", Gnome.FontWeight.Bold ,false, 12);
				
			// ESTA FUNCION ES PARA QUE EL TEXTO SALGA EN NEGRITA			
			numpage = 1;			
			ContextoImp.BeginPage("Pagina "+numpage.ToString());
			//Encabezado de pagina
			ContextoImp.Rotate(90);
//////////////////////////////////////////////ESTA FUNCION SIRVE PARA CREAR RECTANGULOS Y CUADROS:////////////////////////////////////////////////////////////////////////////////////////////////////////////
			//cuadro FOLIO:
			Gnome.Print.RectStroked(ContextoImp,800,-45,-190,28);
			//Cuadro(PROVEEDOR y REPRESENTANTE)                                 Cuadro(FECHA DE SOLICITUD):                                         Cuadro(No. requisicion):
    	    Gnome.Print.RectStroked(ContextoImp,489,-165,-399,110);             Gnome.Print.RectStroked(ContextoImp,610,-90,-120,35);              Gnome.Print.RectStroked(ContextoImp,800,-90,-190,35);			
			//fecha de entrega                                                  condiciones de pago
		    Gnome.Print.RectStroked(ContextoImp,610,-125,-120,35);              Gnome.Print.RectStroked(ContextoImp,800,-125,-190,35);
			//c lugar de entrega                                                l.a.b. y flete
			Gnome.Print.RectStroked(ContextoImp,610,-165,-120,40);              Gnome.Print.RectStroked(ContextoImp,800,-165,-190,40);
			//cuadro grande
			Gnome.Print.RectStroked(ContextoImp,800,-435,-710,250);
			//Cuadro(importe con letra)                                                                                        
    	    Gnome.Print.RectStroked(ContextoImp,489,-476,-399,40);               
			//elaboro                                                           autorizacion                                                        depto. solicitante
			Gnome.Print.RectStroked(ContextoImp,339,-515,-250,35);              Gnome.Print.RectStroked(ContextoImp,550,-515,-180,35);              Gnome.Print.RectStroked(ContextoImp,800,-515,-190,35);
			//firma de recibido                                                 provedor
			Gnome.Print.RectStroked(ContextoImp,589,-555,-500,35);              Gnome.Print.RectStroked(ContextoImp,800,-555,-190,35);
			//cantidad solicitada
			Gnome.Print.RectStroked(ContextoImp,180,-435,-50,250);
			//descripcion:                                                 
		    Gnome.Print.RectStroked(ContextoImp,570,-435,-350,250);
			//importe:
			 Gnome.Print.RectStroked(ContextoImp,680,-480,-55,295);
////////////////////////////////ESTA FUNCION SIRVE PARA CREAR LINEAS ////////////////////////////////////////////
			Gnome.Print.LineStroked(ContextoImp,90,-210,800,-210);
			
			// Cambiar la fuente
			Gnome.Print.Setfont(ContextoImp,fuente5);
			ContextoImp.MoveTo(340.5, -25);	            ContextoImp.Show( titulo+"");
			Gnome.Print.Setfont(ContextoImp,fuente2);
			ContextoImp.MoveTo(95, -20);			    ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(95, -30);			    ContextoImp.Show("Direccion: ");
			ContextoImp.MoveTo(95, -40);			    ContextoImp.Show("Conmutador: ");
			
			Gnome.Print.Setfont(ContextoImp,fuente2);
			ContextoImp.MoveTo(620, -30);			    ContextoImp.Show("FOLIO:");
			ContextoImp.MoveTo(620, -30);			    ContextoImp.Show("FOLIO:");
			
			ContextoImp.MoveTo(95, -70);			    ContextoImp.Show("PROVEEDOR:");
			ContextoImp.MoveTo(95.5, -70);			    ContextoImp.Show("PROVEEDOR:");
			ContextoImp.MoveTo(500, -67);			    ContextoImp.Show("FECHA DE SOLICITUD:");
			ContextoImp.MoveTo(500.5, -67);			    ContextoImp.Show("FECHA DE SOLICITUD:");
			ContextoImp.MoveTo(620, -67);			    ContextoImp.Show("ORDEN DE COMPRA:");
			ContextoImp.MoveTo(620.5, -67);			    ContextoImp.Show("ORDEN DE COMPRA:");
			
			ContextoImp.MoveTo(500, -100);			    ContextoImp.Show("FECHA DE ENTREGA:");
			ContextoImp.MoveTo(500.5, -100);			ContextoImp.Show("FECHA DE ENTREGA:");
			ContextoImp.MoveTo(620, -100);			    ContextoImp.Show("CONDICIONES DE PAGO:");
			ContextoImp.MoveTo(620.5, -100);			ContextoImp.Show("CONDICIONES DE PAGO:");
			
			ContextoImp.MoveTo(95, -130);			    ContextoImp.Show("REPRESENTANTE:");
			ContextoImp.MoveTo(95.5, -130);			    ContextoImp.Show("REPRESENTANTE:");
			ContextoImp.MoveTo(500, -135);			    ContextoImp.Show("LUGAR DE ENTREGA:");
            ContextoImp.MoveTo(500.5, -135);			    ContextoImp.Show("LUGAR DE ENTREGA:");
			ContextoImp.MoveTo(620.5, -135);			ContextoImp.Show("L.A.B Y FLETES:");
			ContextoImp.MoveTo(620, -135);			ContextoImp.Show("L.A.B Y FLETES:");
			
			ContextoImp.MoveTo(90, -175);			ContextoImp.Show("Con base en la cotizacion presentada por su empresa, sirvase a remitir los bienes o servicios que a continuacion se detallan:");
			ContextoImp.MoveTo(90.5, -175);			ContextoImp.Show("Con base en la cotizacion presentada por su empresa, sirvase a remitir los bienes o servicios que a continuacion se detallan:");			
			
			ContextoImp.MoveTo(95, -195);			    ContextoImp.Show("No. DE");
			ContextoImp.MoveTo(95, -205);			    ContextoImp.Show("PARTIDA");
			ContextoImp.MoveTo(95.5, -195);			    ContextoImp.Show("No. DE");
			ContextoImp.MoveTo(95.5, -205);			    ContextoImp.Show("PARTIDA");
			ContextoImp.MoveTo(135, -195);			    ContextoImp.Show("CANTIDAD");
			ContextoImp.MoveTo(135, -205);			    ContextoImp.Show("SOLICITADA");
			ContextoImp.MoveTo(135.5, -195);			ContextoImp.Show("CANTIDAD");
			ContextoImp.MoveTo(135.5, -205);			ContextoImp.Show("SOLICITADA");
			ContextoImp.MoveTo(184, -200);			    ContextoImp.Show("UNIDAD ");
			ContextoImp.MoveTo(184.5, -200);			ContextoImp.Show("UNIDAD ");
            ContextoImp.MoveTo(320, -200);			    ContextoImp.Show("DESCRIPCION");
			ContextoImp.MoveTo(320.5, -200);			ContextoImp.Show("DESCRIPCION");
			ContextoImp.MoveTo(575, -195);			    ContextoImp.Show(" PRECIO ");
			ContextoImp.MoveTo(575, -205);			    ContextoImp.Show(" UNITARIO");
			ContextoImp.MoveTo(575, -195);			    ContextoImp.Show(" PRECIO ");
			ContextoImp.MoveTo(575.5, -205);			ContextoImp.Show(" UNITARIO");
            ContextoImp.MoveTo(635, -200);			    ContextoImp.Show("IMPORTE ");
			ContextoImp.MoveTo(635.5, -200);			ContextoImp.Show("IMPORTE");
			ContextoImp.MoveTo(710, -200);			    ContextoImp.Show("OBSERVACIONES ");
			ContextoImp.MoveTo(710.5, -200);			ContextoImp.Show("OBSERVACIONES ");
			
			ContextoImp.MoveTo(95, -450);			    ContextoImp.Show("IMPORTE CON LETRAS: ");
			ContextoImp.MoveTo(95.5, -450);			ContextoImp.Show("IMPORTE CON LETRAS: ");
			ContextoImp.MoveTo(570, -445);			    ContextoImp.Show("SUB-TOTAL_15: ");
			ContextoImp.MoveTo(570.5, -445);			ContextoImp.Show("SUB-TOTAL_15: ");
			ContextoImp.MoveTo(570.5, -455);			ContextoImp.Show("SUB-TOTAL_0: ");
			ContextoImp.MoveTo(570.5, -455);			ContextoImp.Show("SUB-TOTAL_0: ");
			ContextoImp.MoveTo(570, -465);			    ContextoImp.Show("16% I.V.A: ");
			ContextoImp.MoveTo(570.5, -465);			ContextoImp.Show("16% I.V.A: ");
			ContextoImp.MoveTo(570, -475);			    ContextoImp.Show("TOTAL: ");
			ContextoImp.MoveTo(570.5, -475);			ContextoImp.Show("TOTAL: ");
			
			ContextoImp.MoveTo(95.5, -490);			ContextoImp.Show("ELABORO: ");
			ContextoImp.MoveTo(95, -490);			ContextoImp.Show("ELABORO: ");
			ContextoImp.MoveTo(380.5, -490);			ContextoImp.Show("AUTORIZACION: ");
			ContextoImp.MoveTo(380, -490);			ContextoImp.Show("AUTORIZACION: ");
			ContextoImp.MoveTo(615, -490);			ContextoImp.Show("DEPTO. SOLICITANTE: ");
			ContextoImp.MoveTo(615.5, -490);			ContextoImp.Show("DEPTO. SOLICITANTE: ");
			
			ContextoImp.MoveTo(95, -530);			ContextoImp.Show("FIRMA DE RECIBIDO: ");
			ContextoImp.MoveTo(95.5, -530);			ContextoImp.Show("FIRMA DE RECIBIDO: ");
			ContextoImp.MoveTo(245, -530);			ContextoImp.Show("NOMBRE DEL RECEPTOR: ");
			ContextoImp.MoveTo(245.5, -530);			ContextoImp.Show("NOMBRE DEL RECEPTOR: ");
			ContextoImp.MoveTo(400, -530);			ContextoImp.Show("FECHA DE RECEPCION: ");
			ContextoImp.MoveTo(400.5, -530);			ContextoImp.Show("FECHA DE RECEPCION: ");
			ContextoImp.MoveTo(615, -530);			ContextoImp.Show("PROVEEDOR: ");
			ContextoImp.MoveTo(615.5, -530);			ContextoImp.Show("PROVEEDOR: ");
			
			ContextoImp.MoveTo(95, -570);			ContextoImp.Show("NOTA: EL pedido surtira en la fecha de entrega indicada en el recuadro de esta orden de compra. ");
			ContextoImp.MoveTo(95.5, -570);			ContextoImp.Show("NOTA: EL pedido surtira en la fecha de entrega indicada en el recuadro de esta orden de compra. ");			
			
			
			
			ContextoImp.ShowPage();
		}
	}
}*/
