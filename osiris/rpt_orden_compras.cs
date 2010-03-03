// created on 23/10/2008 at 12:56 p
// rpt_orden_compras.cs created with MonoDevelop
// User: ipena at 10:44 a 22/10/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
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
	public class rpt_orden_compras
	{
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
		
		public rpt_orden_compras(string nombrebd_,object lista_productos_a_comprar_,object treeViewEngineProductosaComprar_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			lista_productos_a_comprar = lista_productos_a_comprar_ as Gtk.TreeView;
			treeViewEngineProductosaComprar = treeViewEngineProductosaComprar_ as Gtk.TreeStore; 
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob(Gnome.PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog(trabajo, "ORDEN DE COMPRAS", 0);
        	
        	int respuesta = dialogo.Run ();
        	
        	if (respuesta == (int) PrintButtons.Cancel) 
			{
				dialogo.Hide (); 
				dialogo.Dispose (); 
				return;
			}
			Gnome.PrintContext ctx = trabajo.Context;
        	ComponerPagina(ctx, trabajo); 
			trabajo.Close();
            switch (respuesta)
        	{
				case (int) PrintButtons.Print:   
                	trabajo.Print (); 
                	break;
                case (int) PrintButtons.Preview:
                	new PrintJobPreview(trabajo, "ORDEN DE COMPRAS").Show();
                	break;
        	}
			dialogo.Hide (); dialogo.Dispose ();		
		}
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			// Crear una fuente 
			Gnome.Font fuente = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
			Gnome.Font fuente1 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
			Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
			Gnome.Font fuente3 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
			Gnome.Font fuente5 = Gnome.Font.FindClosestFromWeightSlant("Bitstream Vera Sans", FontWeight.Bold ,false, 12);
				
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
			ContextoImp.MoveTo(95, -20);			    ContextoImp.Show("Hospital Santa Cecilia");
			ContextoImp.MoveTo(95, -30);			    ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(95, -40);			    ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			
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
			
			ContextoImp.MoveTo(90, -175);			ContextoImp.Show("Con base en la cotizacion presentada por su empresa al HOSPITAL SANTA CECILIA DE MONTERREY, S.A DE C.V., Sirvase a remitir los bienes o servicios que a continuacion se detallan:");
			ContextoImp.MoveTo(90.5, -175);			ContextoImp.Show("Con base en la cotizacion presentada por su empresa al HOSPITAL SANTA CECILIA DE MONTERREY, S.A DE C.V., Sirvase a remitir los bienes o servicios que a continuacion se detallan:");			
			
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
}
