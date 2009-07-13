///////////////////////////////////////////////////////////
// created on 26/07/2007 at 04:18 p
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
// 				  Daniel Olivares Cuevas (Pre-Programacion, Colaboracion y Ajustes) arcangeldoc@gmail.com
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
// Proposito	: Impresion del procedimiento de cobranza 
// Objeto		: rpt_proc_cobranza.cs
using System;
using Gtk;
using Gnome;
using Npgsql;
using System.Data;
using Glade;
using System.Collections;
using GtkSharp;

namespace osiris
{
	public class inventario_almacen_reporte
	{
		public string connectionString = "Server=localhost;" +
        	    	                     "Port=5432;" +
            	    	                 "User ID=admin;" +
                	    	             "Password=1qaz2wsx;";
        public string nombrebd;
		public int idalmacen;
		public string almacen;
		public string mesinventario;
		public string anoinventario;
		public string LoginEmpleado;
		public string NomEmpleado;
		public string AppEmpleado;
		public string ApmEmpleado;
		
		public int columnas=-10;
		public int contador = 1;
		public int numpage = 1;
		
		public int idproducto = 0;
		public string producto = "";
		
		public string datos = "";
		public string fcreacion = "";
		public decimal porcentajedes =  0;
		public decimal descsiniva = 0;
		public decimal ivadedesc = 0;
		public decimal descuento = 0;
		public decimal ivaprod = 0;
		public decimal subtotal = 0;
		public decimal subtotalelim = 0;
		public decimal subt15 = 0;
		public decimal subt15elim = 0;
		public decimal subt0 = 0;
		public decimal subt0elim = 0;
		public decimal sumadesc = 0;
		public decimal sumadescelim = 0;
		public decimal sumaiva = 0;
		public decimal sumaivaelim = 0;
		public decimal total = 0;
		public decimal totalelim = 0;
		public decimal totaladm = 0;
		public decimal totaladmelim = 0;
		public decimal totaldesc = 0;
		public decimal subtotaldelmov = 0;	
				
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		public Gnome.Font fuente6 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
		public Gnome.Font fuente7 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
		public Gnome.Font fuente8 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
		public Gnome.Font fuente9 = Gnome.Font.FindClosest("Bitstream Vera Sans", 9);
		public Gnome.Font fuente10 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
		public Gnome.Font fuente11 = Gnome.Font.FindClosest("Bitstream Vera Sans", 11);
		public Gnome.Font fuente12 = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
		public Gnome.Font fuente36 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
		
				
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		public inventario_almacen_reporte (int id_almacen_,string almacen_,string mesinventario_,string ano_inventario_,
										string LoginEmpleado_,string NomEmpleado_,string AppEmpleado_,
										string ApmEmpleado_,string _nombrebd_)
		{
			nombrebd = _nombrebd_;//
			idalmacen = id_almacen_;
			almacen = almacen_;
			mesinventario = mesinventario_;
			anoinventario = ano_inventario_;
			LoginEmpleado = LoginEmpleado_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, "PROCEDIMIENTO COBRANZA", 0);
        	int         respuesta = dialogo.Run ();
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
                new PrintJobPreview(trabajo, "PROCEDIMIENTO COBRANZA").Show();
                break;
        	}
			dialogo.Hide (); dialogo.Dispose ();
		}
      	
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
      		// Cambiar la fuente
			Gnome.Print.Setfont (ContextoImp, fuente6);
			ContextoImp.Rotate(90);
			ContextoImp.MoveTo(19.7,-10);			ContextoImp.Show("Hospital Santa Cecilia");//19.7, 770
			ContextoImp.MoveTo(20, -10);			ContextoImp.Show("Hospital Santa Cecilia");
			ContextoImp.MoveTo(19.7, -20);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(20, -20);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(19.7, -30);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			ContextoImp.MoveTo(20, -30);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
							
			Gnome.Print.Setfont (ContextoImp, fuente12);
			Gnome.Print.Setrgbcolor(ContextoImp, 150,0,0);
					
			Gnome.Print.Setfont (ContextoImp, fuente36);
			Gnome.Print.Setrgbcolor(ContextoImp, 0,0,0);
			ContextoImp.MoveTo(20, -40);				ContextoImp.Show("____________________________");
									    			
		
      }
      
      void genera_tabla(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
      {
      	//////////////////DIBUJANDO TABLA (START DRAWING TABLE)////////////////////////
		Gnome.Print.Setfont (ContextoImp, fuente36);
		ContextoImp.MoveTo(20, -40);					ContextoImp.Show("____________________________");
				
		////COLUMNAS
		int columnasl = -10;
		for (int i1=0; i1 < 28; i1++)//30 veces para tasmaño carta
		{	
            int columnas = 17;
			Gnome.Print.Setfont (ContextoImp, fuente36);
			ContextoImp.MoveTo(columnas, columnasl-.8);		ContextoImp.Show("|");
			ContextoImp.MoveTo(columnas+777, columnasl);		ContextoImp.Show("|");
			columnasl-=20;
		}
		//columnas tenues
		//int columnasc =640;
		Gnome.Print.Setfont (ContextoImp, fuente36);
		ContextoImp.MoveTo(20,-650);		ContextoImp.Show("____________________________");
		///FIN DE DIBUJO DE TABLA (END DRAWING TABLE)///////
    }
    
    void genera_lineac(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
	{
		Gnome.Print.Setfont (ContextoImp, fuente11);
		ContextoImp.MoveTo(75, columnas);					ContextoImp.Show("|");//52
		ContextoImp.MoveTo(104, columnas);					ContextoImp.Show("|");//104
		ContextoImp.MoveTo(375, columnas);					ContextoImp.Show("|");
		ContextoImp.MoveTo(425, columnas);					ContextoImp.Show("|");
		ContextoImp.MoveTo(475, columnas);					ContextoImp.Show("|");
		ContextoImp.MoveTo(523, columnas);					ContextoImp.Show("|");
		Gnome.Print.Setfont (ContextoImp, fuente7);
	}
       
	void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion,int contador_)
	{
		//Console.WriteLine("contador antes del if: "+contador_.ToString());
        if (contador_ > 57 )
        {
        	numpage +=1;
        	ContextoImp.ShowPage();
			ContextoImp.BeginPage("Pagina "+numpage.ToString());
			imprime_encabezado(ContextoImp,trabajoImpresion);
     		genera_tabla(ContextoImp,trabajoImpresion);
     		Gnome.Print.Setfont (ContextoImp, fuente7);
        	contador=1;
        	columnas=-10;
        }
       //Console.WriteLine("contador despues del if: "+contador_.ToString());
	}
	
	void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
	{	
		
		NpgsqlConnection conexion; 
        conexion = new NpgsqlConnection (connectionString+nombrebd);
        // Verifica que la base de datos este conectada
        try 
        {
 			conexion.Open ();
        	NpgsqlCommand comando; 
        	comando = conexion.CreateCommand (); 
        	comando.CommandText = "SELECT descripcion_producto,eliminado, "+
							"id_quien_creo,hscmty_productos.aplicar_iva,hscmty_catalogo_almacenes.id_almacen,  "+
							"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto, "+
							"to_char(hscmty_catalogo_almacenes.id_producto,'999999999999') AS idproducto, "+
							"to_char(hscmty_catalogo_almacenes."+mesinventario.ToString()+",'99999.99') AS stock, "+
							"to_char(hscmty_productos.costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(hscmty_productos.costo_producto,'999999999.99') AS costoproducto, "+
							"to_char(hscmty_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(hscmty_productos.cantidad_de_embalaje,'999999999.99') AS embalaje, "+
							"to_char(hscmty_catalogo_almacenes.fechahora_alta,'dd-MM-yyyy HH:mi:ss') AS fechcreacion "+
							"FROM "+
							"hscmty_catalogo_almacenes,hscmty_productos,hscmty_almacenes,hscmty_grupo_producto,hscmty_grupo1_producto,hscmty_grupo2_producto "+
							"WHERE hscmty_catalogo_almacenes.id_producto = hscmty_productos.id_producto "+ 
							"AND hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
							"AND hscmty_productos.id_grupo1_producto = hscmty_grupo1_producto.id_grupo1_producto "+
							"AND hscmty_productos.id_grupo2_producto = hscmty_grupo2_producto.id_grupo2_producto "+
							"AND hscmty_catalogo_almacenes.id_almacen = hscmty_almacenes.id_almacen "+
							"AND hscmty_catalogo_almacenes.id_almacen = '"+(int) idalmacen +"' "+
							"AND hscmty_catalogo_almacenes.ano_inventario = '"+(string) anoinventario +"' "+
							"ORDER BY hscmty_productos.descripcion_producto,to_char(hscmty_catalogo_almacenes.fechahora_alta,'yyyy-mm-dd HH:mm:ss');";
						
        	NpgsqlDataReader lector = comando.ExecuteReader ();
        	//Console.WriteLine("query proc cobr: "+comando.CommandText.ToString());
			ContextoImp.BeginPage("Pagina 1");
			//ContextoImp.Rotate(180);
								
			if(lector.Read()){	
        		/////DATOS DE PRODUCTOS
	        		
	      		imprime_encabezado(ContextoImp,trabajoImpresion);
	     		genera_tabla(ContextoImp,trabajoImpresion);
	     		    	
     		  	//imprime_titulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_admisiones"],fcreacion);
        		contador+=1;
        		salto_pagina(ContextoImp,trabajoImpresion,contador);
       		 	
       		 	//imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"]);
       		 	contador+=1;
       		 	salto_pagina(ContextoImp,trabajoImpresion,contador);
       		 	genera_lineac(ContextoImp, trabajoImpresion);
        		
        		//DATOS TABLA
				ContextoImp.MoveTo(80, columnas);			ContextoImp.Show((string) lector["stock"]);//22	
				ContextoImp.MoveTo(22, columnas);			ContextoImp.Show((string) lector["idproducto"]);//55
				if(datos.Length > 61)
				{
					ContextoImp.MoveTo(110, columnas);		ContextoImp.Show(datos.Substring(0,60));  
				}else{
					ContextoImp.MoveTo(110, columnas);		ContextoImp.Show(datos.ToString());
				} 
				ContextoImp.MoveTo(380, columnas);			ContextoImp.Show("$"+(string) lector["costoproductounitario"]);
				ContextoImp.MoveTo(430, columnas);			ContextoImp.Show(subtotal.ToString("C").PadLeft(10));
				ContextoImp.MoveTo(480, columnas);			ContextoImp.Show(ivaprod.ToString("C").PadLeft(10));
				ContextoImp.MoveTo(530, columnas);			ContextoImp.Show(total.ToString("C").PadLeft(10));
				contador+=1;
				columnas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				
				//idproducto = (int) lector["id_grupo_producto"];
				
				while (lector.Read())
        		{
        		/*	if (contador==1) 
        			{
						imprime_titulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_admisiones"],fcreacion);
		        		contador+=1;
		        		salto_pagina(ContextoImp,trabajoImpresion,contador);
		       		 	
		       		 	imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"]);
		       		 	contador+=1;
		       		 	salto_pagina(ContextoImp,trabajoImpresion,contador);
		       		 	genera_lineac(ContextoImp, trabajoImpresion);
        			}*/
        			genera_lineac(ContextoImp, trabajoImpresion);
					ContextoImp.MoveTo(80, columnas);						ContextoImp.Show((string) lector["stock"]);	
					ContextoImp.MoveTo(22, columnas);						ContextoImp.Show((string) lector["idproducto"]);
					if(datos.Length > 64)
					{
					ContextoImp.MoveTo(110, columnas);				ContextoImp.Show(datos.Substring(0,60));
					}else{
					ContextoImp.MoveTo(110, columnas);				ContextoImp.Show(datos);
					} 
					ContextoImp.MoveTo(380, columnas);					ContextoImp.Show("$"+(string) lector["costoproductounitario"]);
					ContextoImp.MoveTo(430, columnas);					ContextoImp.Show(subtotal.ToString("C").PadLeft(10));
					ContextoImp.MoveTo(480, columnas);					ContextoImp.Show(ivaprod.ToString("C").PadLeft(10));
					ContextoImp.MoveTo(530, columnas);					ContextoImp.Show(total.ToString("C").PadLeft(10));
					contador+=1;			columnas-=10;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
				}//termino de ciclo
				
        		genera_lineac(ContextoImp, trabajoImpresion);
       		 	////IMPRESION DE LOS TOTALES DE AREA
        		genera_lineac(ContextoImp, trabajoImpresion);
        		ContextoImp.MoveTo(479.7, columnas);				ContextoImp.Show("Total de Desc.");
        		ContextoImp.MoveTo(480, columnas);					ContextoImp.Show("Total de Desc.");
        		contador+=1;
        		columnas-=10;
        		salto_pagina(ContextoImp,trabajoImpresion,contador);
        		genera_lineac(ContextoImp, trabajoImpresion);
        		ContextoImp.MoveTo(479.7, columnas);				ContextoImp.Show("Total de Area");
        		ContextoImp.MoveTo(480, columnas);					ContextoImp.Show("Total de Area");
        		contador+=1;
        		salto_pagina(ContextoImp,trabajoImpresion,contador);
        		//Console.WriteLine("contador antes de los totales: "+contador.ToString());
    			///TOTAL QUE SE LE COBRARA AL PACIENTE O AL RESPONSABLE DEL PACIENTE
    			ContextoImp.MoveTo(20, columnas-2);//623
				ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
    			contador+=1;
    			columnas-=10;
    			salto_pagina(ContextoImp,trabajoImpresion,contador);
				    	
    			ContextoImp.MoveTo(381.5, columnas) ;		ContextoImp.Show("SUBTOTAL AL 15%"); 
    			ContextoImp.MoveTo(382, columnas);			ContextoImp.Show("SUBTOTAL AL 15%");	
				contador+=1;
				columnas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
		
				ContextoImp.MoveTo(381.5, columnas);		ContextoImp.Show("SUBTOTAL AL 0%");
				ContextoImp.MoveTo(382, columnas);			ContextoImp.Show("SUBTOTAL AL 0%");	
				contador+=1;
				columnas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
		
				ContextoImp.MoveTo(381.5, columnas);		ContextoImp.Show("IVA AL 15%");
				ContextoImp.MoveTo(382, columnas);			ContextoImp.Show("IVA AL 15%");	
				contador+=1;
				columnas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				
				ContextoImp.MoveTo(381.5, columnas);		ContextoImp.Show("SUB-TOTAL");
				ContextoImp.MoveTo(382, columnas);			ContextoImp.Show("SUB-TOTAL");	
				contador+=1;
				columnas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
			
				ContextoImp.MoveTo(381.5, columnas);		ContextoImp.Show("MENOS DEDUCIBLE");	
				ContextoImp.MoveTo(382, columnas);			ContextoImp.Show("MENOS DEDUCIBLE");	
				contador+=1;
				columnas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				
				ContextoImp.MoveTo(381.5, columnas);		ContextoImp.Show("MENOS COASEGURO");
				ContextoImp.MoveTo(382, columnas);			ContextoImp.Show("MENOS COASEGURO");	
				contador+=1;
				columnas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				
				ContextoImp.MoveTo(381.5, columnas);		ContextoImp.Show("MENOS DESCUENTO");
				ContextoImp.MoveTo(382, columnas);			ContextoImp.Show("MENOS DESCUENTO");	
				contador+=1;
				columnas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				
				ContextoImp.MoveTo(381.5, columnas);		ContextoImp.Show("TOTAL");
				ContextoImp.MoveTo(382, columnas);			ContextoImp.Show("TOTAL");	
				contador+=1;
				columnas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				ContextoImp.ShowPage();
				//Console.WriteLine("contador totales: "+contador.ToString());
				//genera_totales(ContextoImp, trabajoImpresion,contador,subtotaldelmov,subt15,subt0,sumaiva,deducible,coaseguro, totaldesc);
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "Este folio no contiene productos aplicados \n"+
							"existentes para que el procedimiento se muestre \n");
				msgBoxError.Run ();				msgBoxError.Destroy();
			}	
		}catch (NpgsqlException ex){
			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			return; 
		}
	}
 }    
}
