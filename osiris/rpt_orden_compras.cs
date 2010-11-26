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
		
		string titulo = "ORDEN DE COMPRAS ";
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public rpt_orden_compras()
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			
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
			imprime_encabezado(cr,layout);
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
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
			cr.MoveTo(290*escala_en_linux_windows, 25*escala_en_linux_windows);			layout.SetText("ORDEN DE COMPRAS");					Pango.CairoHelper.ShowLayout (cr, layout);
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
