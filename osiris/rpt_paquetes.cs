///////////////////////////////////////////////////////////
// created on 25/05/2007 at 08:36 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: ing. Juan Antonio Peña Gonzalez (Programacion)
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
using Gtk;
using Npgsql;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class paquetes_reporte
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
		string cirugia = "";
		string medico = "";
		int id;
		string tiporeporte = "";
		string deposito_minimo = "";
		string dias_internamiento = "";
		string tel_medico = "";
		string tel_opcional = "";
		string fax = "";
		string notas = "";
		string numpresupuesto = "";
		string titulo = "";
		string schars = "";
		bool rptconprecio = true;
		
		//variables para rangos de fecha
				
		int idadmision_ = 0;
		int idproducto = 0;
		string datos = "";
		string fcreacion = "";
		decimal cantaplicada= 0;
		decimal ivaprod = 0;
		decimal subtotal = 0;
		decimal subt15 = 0;
		decimal subt0 = 0;
		decimal sumaiva = 0;
		decimal total = 0;
		decimal totaladm = 0;
		decimal subtotaldelmov = 0;
		decimal deducible = 0;
		decimal coaseguro = 0;
		decimal valoriva;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
						
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		public paquetes_reporte ( int _id_ ,string nombcirugia,string _medico_,string _nombrebd_,string tiporeporte_,
								string deposito_minimo_,string dias_internamiento_,string tel_medico_,
								string tel_opcional_,string fax_,string numpresupuesto_,string notas_,bool rptconprecio_,string presupuesto_seleccionados_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			cirugia = nombcirugia; 
			id = _id_;
			medico = _medico_;
			tiporeporte = tiporeporte_;
			deposito_minimo = deposito_minimo_;
			dias_internamiento = dias_internamiento_;
			tel_medico = tel_medico_;
			tel_opcional = tel_opcional_;
			fax = fax_;
			notas = notas_;
			numpresupuesto = numpresupuesto_;
			rptconprecio = rptconprecio_;
			valoriva = decimal.Parse(classpublic.ivaparaaplicar);
			
			if(tiporeporte == "presupuestos") { 
				titulo = "PRESUPUESTO DE CIRUGIA";
			}else{
				titulo = "PAQUETES DE CIRUGIA";
			}
			
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
			//print.PrintSettings.Orientation = PageOrientation.Landscape;
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
			/*
			
			decimal precioventaconvenido;
			NpgsqlConnection conexion; 
	        conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
	          
        	try{
 			conexion.Open ();
        	NpgsqlCommand comando; 
        	comando = conexion.CreateCommand (); 
        	
	        	if(tiporeporte == "paquetes"){
	           		comando.CommandText = "SELECT descripcion_producto,osiris_his_tipo_admisiones.descripcion_admisiones, "+
								"id_empleado,osiris_his_cirugias_deta.eliminado,osiris_productos.aplicar_iva,osiris_his_cirugias_deta.id_tipo_admisiones,  "+
								"osiris_productos.descripcion_producto,descripcion_grupo_producto,osiris_productos.id_grupo_producto, "+
								"to_char(osiris_his_tipo_cirugias.precio_de_venta,'999999999999') AS precioventa, "+
								"to_char(osiris_his_cirugias_deta.id_producto,'999999999999') AS idproducto, "+
								"to_char(osiris_his_cirugias_deta.cantidad_aplicada,'99999.99') AS cantidadaplicada, "+
								"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
								"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario, "+
								"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porcentageutilidad, "+
								"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto, "+
								"to_char(osiris_his_cirugias_deta.fechahora_creacion,'dd-MM-yyyy HH:mi:ss') AS fechcreacion ,"+
								"to_char(osiris_his_cirugias_deta.id_secuencia,'9999999999') AS secuencia "+
								"FROM "+
								"osiris_his_cirugias_deta,osiris_productos,osiris_his_tipo_cirugias,osiris_his_tipo_admisiones,osiris_grupo_producto "+
								"WHERE "+
								"osiris_his_cirugias_deta.id_producto = osiris_productos.id_producto "+
								"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
								"AND osiris_his_cirugias_deta.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
								"AND osiris_his_cirugias_deta.eliminado = false "+ 
								"AND osiris_his_cirugias_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
								"AND osiris_his_cirugias_deta.id_tipo_cirugia = '"+id.ToString() +"' "+
								"ORDER BY osiris_his_cirugias_deta.id_tipo_admisiones,osiris_productos.id_grupo_producto,osiris_productos.descripcion_producto;";
	        	}else{
	        		if(tiporeporte == "presupuestos"){
	        			comando.CommandText = "SELECT descripcion_producto,descripcion_admisiones, "+
							"id_empleado,osiris_his_presupuestos_deta.eliminado,osiris_productos.aplicar_iva,osiris_his_presupuestos_deta.id_tipo_admisiones,  "+
							"osiris_productos.descripcion_producto,descripcion_grupo_producto,osiris_productos.id_grupo_producto, "+
							"to_char(osiris_his_presupuestos_enca.precio_convenido,'999999999999') AS precioventa, "+
							"to_char(osiris_his_presupuestos_deta.id_producto,'999999999999') AS idproducto, "+
							"to_char(osiris_his_presupuestos_deta.cantidad_aplicada,'99999.99') AS cantidadaplicada, "+
							"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porcentageutilidad, "+
							"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto, "+
							"to_char(osiris_his_presupuestos_deta.fechahora_creacion,'dd-MM-yyyy HH:mi:ss') AS fechcreacion ,"+
							"to_char(osiris_his_presupuestos_deta.id_secuencia,'9999999999') AS secuencia "+
							"FROM "+
							"osiris_his_presupuestos_enca,osiris_his_presupuestos_deta,osiris_productos,osiris_his_tipo_admisiones,osiris_grupo_producto "+
							"WHERE "+
							"osiris_his_presupuestos_deta.id_producto = osiris_productos.id_producto "+
							"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
							"AND osiris_his_presupuestos_enca.id_presupuesto = osiris_his_presupuestos_deta.id_presupuesto "+
							"AND osiris_his_presupuestos_deta.eliminado = 'false' "+ 
							"AND osiris_his_presupuestos_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
							"AND osiris_his_presupuestos_deta.id_presupuesto IN ('"+id.ToString()+"') "+							
							"ORDER BY osiris_his_presupuestos_deta.id_tipo_admisiones,osiris_productos.id_grupo_producto,osiris_productos.descripcion_producto;";
	        		}
        		}	
        	
        		NpgsqlDataReader lector = comando.ExecuteReader ();
        		//Console.WriteLine("query proc cobr: "+comando.CommandText.ToString());
				ContextoImp.BeginPage("Pagina 1");
								
				filas=690;
        		if (lector.Read()){	
        			precioventaconvenido = decimal.Parse((string) lector["precioventa"]);
        		
        			datos = (string) lector["descripcion_producto"];
	        		cantaplicada = decimal.Parse((string) lector["cantidadaplicada"]);
					subtotal = decimal.Parse((string) lector["preciopublico"])*cantaplicada;
					
					if((bool) lector["aplicar_iva"]== true){
						ivaprod = (subtotal*valoriva)/100;
						subt15 += subtotal;
					}else{
						subt0 += subtotal;
						ivaprod = 0;
					}
					sumaiva += ivaprod;
					total = subtotal + ivaprod;				
	        		totaladm += total;
					subtotaldelmov += total;
						
	        		/////DATOS DE PRODUCTOS
	      		  	imprime_encabezado(ContextoImp,trabajoImpresion);
	      		  	genera_tabla(ContextoImp,trabajoImpresion);
     		   	
     		   		imprime_titulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_admisiones"]);
        			contador+=1;
        			salto_pagina(ContextoImp,trabajoImpresion,contador);
       		 		//genera_lineac(ContextoImp, trabajoImpresion);
       		 	
       		 		imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"]);
       		 		contador+=1;
       		 		salto_pagina(ContextoImp,trabajoImpresion,contador);
       		 		genera_lineac(ContextoImp, trabajoImpresion);
        		
        			//DATOS TABLA
					ContextoImp.MoveTo(80, filas);			ContextoImp.Show((string) lector["cantidadaplicada"]);//22	
					ContextoImp.MoveTo(22, filas);			ContextoImp.Show((string) lector["idproducto"]);//55
					if(rptconprecio == true){
						if(datos.Length > 64) { datos = datos.Substring(0,60); }
						ContextoImp.MoveTo(110, filas);			ContextoImp.Show(datos.ToString());
						ContextoImp.MoveTo(380, filas);			ContextoImp.Show("$"+(string) lector["preciopublico"]);
						ContextoImp.MoveTo(430, filas);			ContextoImp.Show(subtotal.ToString("C"));
						ContextoImp.MoveTo(480, filas);			ContextoImp.Show(ivaprod.ToString("C"));
						ContextoImp.MoveTo(530, filas);			ContextoImp.Show(total.ToString("C"));
					}else{
						if(datos.Length > 115) { datos = datos.Substring(0,115); }
						ContextoImp.MoveTo(110, filas);		ContextoImp.Show(datos.ToString());
					}
					contador+=1;			filas-=10;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
					idadmision_ = (int) lector["id_tipo_admisiones"];
        			idproducto = (int) lector["id_grupo_producto"];
				
					while (lector.Read()){
	        			if (contador==1){
							imprime_titulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_admisiones"]);
			        		contador+=1;
			        		salto_pagina(ContextoImp,trabajoImpresion,contador);
			       		 	
			       		 	imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"]);
			       		 	contador+=1;
			       		 	salto_pagina(ContextoImp,trabajoImpresion,contador);
			       		 	genera_lineac(ContextoImp, trabajoImpresion);
	        			}
	        			
	        			datos = (string) lector["descripcion_producto"];
						cantaplicada = decimal.Parse((string) lector["cantidadaplicada"]);
						subtotal = decimal.Parse((string) lector["preciopublico"]) * cantaplicada;
						
						if((bool) lector["aplicar_iva"]== true){
							ivaprod = (subtotal*valoriva)/100;
							subt15 += subtotal;
						}else{
							subt0 += subtotal;
							ivaprod = 0;
						}
						sumaiva += ivaprod;
						total = subtotal + ivaprod;
						totaladm +=total;
						subtotaldelmov +=total;
        			
					if(idadmision_ == (int) lector["id_tipo_admisiones"]){
						genera_lineac(ContextoImp, trabajoImpresion);
						
						//DATOS TABLA
        				if (idproducto != (int) lector["id_grupo_producto"])
        				{
        					idproducto = (int) lector["id_grupo_producto"];
        					imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"]);
        			   		contador+=1;
        			   		salto_pagina(ContextoImp,trabajoImpresion,contador);
        			   		genera_lineac(ContextoImp, trabajoImpresion);
        				}
					}else{////////SI LA ADMISION CAMBIA HACE ESTO	
						if(rptconprecio == true) 
       		 			{
							///IMPRESION DE LOS TOTALES DE AREA
							salto_pagina(ContextoImp,trabajoImpresion,contador);
	        				genera_lineac(ContextoImp, trabajoImpresion);
	        				ContextoImp.MoveTo(479.7, filas);		ContextoImp.Show("Total de Area");
	        				ContextoImp.MoveTo(480, filas);			ContextoImp.Show("Total de Area");
	        				//ContextoImp.MoveTo(529.7, filas);		ContextoImp.Show(totaladm.ToString("C"));
	        				//ContextoImp.MoveTo(530, filas);			ContextoImp.Show(totaladm.ToString("C"));
		        			contador+=1;
		        			filas-=10;
		        			salto_pagina(ContextoImp,trabajoImpresion,contador);
		        			totaladm = 0;
		        			genera_lineac(ContextoImp, trabajoImpresion);
	        			}
						imprime_titulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_admisiones"]);
						contador+=1;
						salto_pagina(ContextoImp,trabajoImpresion,contador);
						
						idadmision_ = (int) lector["id_tipo_admisiones"];
						if (idproducto != (int) lector["id_grupo_producto"])
        				{
							idproducto = (int) lector["id_grupo_producto"];
							imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"]);
	        			   	contador+=1;
							salto_pagina(ContextoImp,trabajoImpresion,contador);
							genera_lineac(ContextoImp, trabajoImpresion);
						}
					}
					
        			ContextoImp.MoveTo(80, filas);					ContextoImp.Show((string) lector["cantidadaplicada"]);//22	
					ContextoImp.MoveTo(22, filas);					ContextoImp.Show((string) lector["idproducto"]);//55
					if(rptconprecio == true)
					{
						if(datos.Length > 64) { datos = datos.Substring(0,60); }
						ContextoImp.MoveTo(110, filas);			ContextoImp.Show(datos.ToString());
						ContextoImp.MoveTo(380, filas);			ContextoImp.Show("$"+(string) lector["preciopublico"]);
						ContextoImp.MoveTo(430, filas);			ContextoImp.Show(subtotal.ToString("C"));
						ContextoImp.MoveTo(480, filas);			ContextoImp.Show(ivaprod.ToString("C"));
						ContextoImp.MoveTo(530, filas);			ContextoImp.Show(total.ToString("C"));
					}else{
						if(datos.Length > 115) { datos = datos.Substring(0,115); }
						ContextoImp.MoveTo(110, filas);		ContextoImp.Show(datos.ToString());
					}
					contador+=1;		filas-=10;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
					
				}//SE TERMINA EL CICLO
        		//imprime_encabezado(ContextoImp,trabajoImpresion);
     		   	//genera_tabla(ContextoImp,trabajoImpresion);
       		 	if(rptconprecio == true) 
       		 	{
	       		 	////IMPRESION DE LOS TOTALES DE AREA
	        		genera_lineac(ContextoImp, trabajoImpresion);
	        		ContextoImp.MoveTo(479.7, filas);				ContextoImp.Show("Total de Area");
	        		ContextoImp.MoveTo(480, filas);					ContextoImp.Show("Total de Area");
	        		//ContextoImp.MoveTo(529.7, filas);				ContextoImp.Show(totaladm.ToString("C"));
	        		//ContextoImp.MoveTo(530, filas);					ContextoImp.Show(totaladm.ToString("C"));
	        		contador+=1;
	        		salto_pagina(ContextoImp,trabajoImpresion,contador);
	        		
	        		///TOTAL QUE SE LE COBRARA AL PACIENTE O AL RESPONSABLE DEL PACIENTE
	    			ContextoImp.MoveTo(20, filas-2);//623
					//ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
	    			decimal totaldelmov =subtotaldelmov - deducible - coaseguro;//desctotal;
	    			contador+=1;
	    			filas-=10;
	    			salto_pagina(ContextoImp,trabajoImpresion,contador);
					
					ContextoImp.MoveTo(381.5, filas) ;		ContextoImp.Show("SUBTOTAL AL "+valoriva.ToString().Trim()); 
	    			ContextoImp.MoveTo(382, filas);			ContextoImp.Show("SUBTOTAL AL "+valoriva.ToString().Trim());	
					ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(subt15.ToString("C")); 
					ContextoImp.MoveTo(530, filas);			ContextoImp.Show(subt15.ToString("C")); 
					contador+=1;
					filas-=10;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
					
					Gnome.Print.Setfont (ContextoImp,fuente10);
					ContextoImp.MoveTo(50.5, filas);		ContextoImp.Show("PRECIO DE VENTA "+precioventaconvenido.ToString("C"));
					ContextoImp.MoveTo(51, filas);			ContextoImp.Show("PRECIO DE VENTA "+precioventaconvenido.ToString("C"));
					Gnome.Print.Setfont (ContextoImp, fuente7);
					ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("SUBTOTAL AL 0%");
					ContextoImp.MoveTo(382, filas);			ContextoImp.Show("SUBTOTAL AL 0%");	
					ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(subt0.ToString("C")); 
					ContextoImp.MoveTo(530, filas);			ContextoImp.Show(subt0.ToString("C"));
					contador+=1;
					filas-=10;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
					Gnome.Print.Setfont (ContextoImp,fuente10);
					ContextoImp.MoveTo(50.5,filas);			ContextoImp.Show("DEPOSITO MINIMO: "+(decimal.Parse(deposito_minimo)).ToString("C"));
					ContextoImp.MoveTo(51,filas);			ContextoImp.Show("DEPOSITO MINIMO: "+(decimal.Parse(deposito_minimo)).ToString("C"));
					Gnome.Print.Setfont (ContextoImp, fuente7);
					ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("IVA AL  "+valoriva.ToString().Trim());
					ContextoImp.MoveTo(382, filas);			ContextoImp.Show("IVA AL  "+valoriva.ToString().Trim());	
					ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(sumaiva.ToString("C")); 
					ContextoImp.MoveTo(530, filas);			ContextoImp.Show(sumaiva.ToString("C")); 
					contador+=1;
					filas-=10;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
					
					ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("SUB-TOTAL");
					ContextoImp.MoveTo(382, filas);			ContextoImp.Show("SUB-TOTAL");	
					ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(subtotaldelmov.ToString("C"));
					ContextoImp.MoveTo(530, filas);			ContextoImp.Show(subtotaldelmov.ToString("C"));
					contador+=1;
					filas-=10;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
					
					ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("TOTAL");
					ContextoImp.MoveTo(382, filas);			ContextoImp.Show("TOTAL");	
					ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(totaldelmov.ToString("C")); 
					ContextoImp.MoveTo(530, filas);			ContextoImp.Show(totaldelmov.ToString("C")); 
					contador+=1;
					filas-=10;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
				}else{
					ContextoImp.MoveTo(20, filas-2);//623
					//ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
	    		}
				ContextoImp.ShowPage();
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "NO contiene productos aplicados \n"+"existentes para que se muestre \n");
				msgBoxError.Run ();		msgBoxError.Destroy();
			}	
		}catch (NpgsqlException ex){
			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			return; 
		}
		*/
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
	}    
}