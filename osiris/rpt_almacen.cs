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

namespace osiris
{
	public class inventario_almacen_reporte
	{
		string connectionString;
        string nombrebd;
		int idalmacen;
		string almacen;
		string mesinventario;
		string anoinventario;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		
		int columnas=-10;
		int contador = 1;
		int numpage = 1;
		
		int idproducto = 0;
		string producto = "";
		
		string datos = "";
		string fcreacion = "";
		decimal porcentajedes =  0;
		decimal descsiniva = 0;
		decimal ivadedesc = 0;
		decimal descuento = 0;
		decimal ivaprod = 0;
		decimal subtotal = 0;
		decimal subtotalelim = 0;
		decimal subt15 = 0;
		decimal subt15elim = 0;
		decimal subt0 = 0;
		decimal subt0elim = 0;
		decimal sumadesc = 0;
		decimal sumadescelim = 0;
		decimal sumaiva = 0;
		decimal sumaivaelim = 0;
		decimal total = 0;
		decimal totalelim = 0;
		decimal totaladm = 0;
		decimal totaladmelim = 0;
		decimal totaldesc = 0;
		decimal subtotaldelmov = 0;	
				
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		Gnome.Font fuente6 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
		Gnome.Font fuente7 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
		Gnome.Font fuente8 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
		Gnome.Font fuente9 = Gnome.Font.FindClosest("Bitstream Vera Sans", 9);
		Gnome.Font fuente10 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
		Gnome.Font fuente11 = Gnome.Font.FindClosest("Bitstream Vera Sans", 11);
		Gnome.Font fuente12 = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
		Gnome.Font fuente36 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);		
				
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public inventario_almacen_reporte (int id_almacen_,string almacen_,string mesinventario_,string ano_inventario_,
										string LoginEmpleado_,string NomEmpleado_,string AppEmpleado_,
										string ApmEmpleado_,string nombrebd_)
		{
			idalmacen = id_almacen_;
			almacen = almacen_;
			mesinventario = mesinventario_;
			anoinventario = ano_inventario_;
			LoginEmpleado = LoginEmpleado_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, "INVENTARIO FISICO", 0);
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
                new PrintJobPreview(trabajo, "INVETARIO FISICO").Show();
                break;
        	}
			dialogo.Hide (); dialogo.Dispose ();
		}
      	
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
      		// Cambiar la fuente
			Gnome.Print.Setfont (ContextoImp, fuente6);
			ContextoImp.Rotate(90);
			ContextoImp.MoveTo(19.7,-10);			ContextoImp.Show("Sistema Hospitalario OSIRIS");//19.7, 770
			ContextoImp.MoveTo(20, -10);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(19.7, -20);			ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(20, -20);			ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(19.7, -30);			ContextoImp.Show("Conmutador:");
			ContextoImp.MoveTo(20, -30);			ContextoImp.Show("Conmutador:");
							
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
							"id_quien_creo,osiris_productos.aplicar_iva,osiris_inventario_almacenes.id_almacen,  "+
							"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto, "+
							"to_char(osiris_inventario_almacenes.id_producto,'999999999999') AS idproducto, "+
							//"to_char(osiris_inventario_almacenes."+mesinventario.ToString()+",'99999.99') AS stock, "+
							"to_char(osiris_inventario_almacenes.stock,'999999.99') AS stock, "+
							"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto, "+
							"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(osiris_productos.cantidad_de_embalaje,'999999999.99') AS embalaje, "+
							"to_char(osiris_inventario_almacenes.fechahora_alta,'dd-MM-yyyy HH:mi:ss') AS fechcreacion "+
							"FROM "+
							"osiris_inventario_almacenes,osiris_productos,osiris_almacenes,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
							"WHERE osiris_inventario_almacenes.id_producto = osiris_productos.id_producto "+ 
							"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
							"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
							"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
							"AND osiris_inventario_almacenes.id_almacen = osiris_almacenes.id_almacen "+
							"AND osiris_inventario_almacenes.id_almacen = '"+(int) idalmacen +"' "+
							"AND osiris_inventario_almacenes.ano_inventario = '"+(string) anoinventario +"' "+
							"AND osiris_inventario_almacenes.mes_inventario = '"+mesinventario+"' "+
							"ORDER BY osiris_productos.descripcion_producto,to_char(osiris_inventario_almacenes.fechahora_alta,'yyyy-mm-dd HH:mm:ss');";
						
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
					ContextoImp.MoveTo(22, columnas);						ContextoImp.Show((string) lector["idproducto"]+" "+(string) lector["descripcion_producto"]);
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
				ButtonsType.Close, "ERROR...");
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
