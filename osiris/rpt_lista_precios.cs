//////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Ing. Jesus Buentello (Programacion) gjuanzz@gmail.com 
//				  Ing. Daniel Olivares C. (Adecuaciones y mejoras) arcangeldoc@gmail.com 05/05/2007
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
// Proposito	: Impresion de listas de precios 
// Objeto		: rpt_lista_precios.cs
/////////////////////////////////////////////////////////
using System;
using Gtk;
using Gnome;
using Npgsql;
using Glade;
using GtkSharp;

namespace osiris
{
	public class lista_de_precios
	{
		public string connectionString = "Server=localhost;" +
        	    	                     "Port=5432;" +
            	    	                 "User ID=admin;" +
                	    	             "Password=1qaz2wsx;";
  		
  		public Gtk.ListStore treeViewEnginegrupos;
		public Gtk.TreeView lista_grupo;
		public Gtk.ListStore treeViewEnginegrupos1;
		public Gtk.TreeView lista_grupo1;
		public Gtk.ListStore treeViewEnginegrupos2;
		public Gtk.TreeView lista_grupo2;	
		
		public string nombrebd;
		
		public int id_tipopaciente;
		public int id_empresa;
		public int id_aseguradora;
		public int filas=730;
		public int contador = 1;
		public int numpage = 1;
		public string ivaaplica = "";
		public int iva = 0;
		public string datos = "";
		
		public bool checkbutton_especiales;
		public bool checkbutton_tarjeta;
		public bool radiobutton_desglosado;
		public bool radiobutton_con_iva; 
		public bool radiobutton_sin_iva;
		[Widget] Gtk.Entry entry_empresa_aseguradora;
		
		public decimal precio_por_cantidad = 0;	
		public decimal ivaproducto = 0;
		public decimal precio_por_cantidad2 = 0;	
		public decimal ivaproducto2 = 0;
		public decimal total = 0;
		public decimal total2 = 0;
		public decimal descuento = 0;					
		public decimal descuento_pesos = 0; 
		public decimal total_desc = 0;
		public decimal ivaproducto1 = 0;
		public decimal total1 = 0;
		public decimal descuento1 = 0;					
		public decimal descuento_pesos1 = 0; 
		public decimal total_desc1 = 0;
		
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		public Gnome.Font fuente5 = Gnome.Font.FindClosest("Luxi Sans", 5);
		public Gnome.Font fuente6 = Gnome.Font.FindClosest("Luxi Sans", 6);
		public Gnome.Font fuente7 = Gnome.Font.FindClosest("Luxi Sans", 7);
		public Gnome.Font fuente8 = Gnome.Font.FindClosest("Luxi Sans", 8);//Bitstream Vera Sans
		public Gnome.Font fuente9 = Gnome.Font.FindClosest("Luxi Sans", 9);
		public Gnome.Font fuente10 = Gnome.Font.FindClosest("Luxi Sans", 10);
		public Gnome.Font fuente11 = Gnome.Font.FindClosest("Luxi Sans", 11);
		public Gnome.Font fuente12 = Gnome.Font.FindClosest("Luxi Sans", 12);
		public Gnome.Font fuente36 = Gnome.Font.FindClosest("Luxi Sans", 36);
			
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		public lista_de_precios(string _nombrebd_ ,object treeview_Engine_grupos_,object treeview_Engine_grupos1_,object treeview_Engine_grupos2_,
								object lista_grupo_,object lista_grupo1_,object lista_grupo2_,
								bool checkbutton_especiales_, bool checkbutton_tarjeta_,
								int id_tipopaciente_,int id_empresa_,int id_aseguradora_,bool radiobutton_desglosado_,
								bool radiobutton_con_iva_,bool radiobutton_sin_iva_,object entry_empresa_aseguradora_)
		
		{
			treeViewEnginegrupos = treeview_Engine_grupos_ as Gtk.ListStore;
			lista_grupo = lista_grupo_ as Gtk.TreeView;
			
			treeViewEnginegrupos1 = treeview_Engine_grupos1_ as Gtk.ListStore;
			lista_grupo1 = lista_grupo1_ as Gtk.TreeView;
			
			treeViewEnginegrupos2 = treeview_Engine_grupos2_ as Gtk.ListStore;			
			lista_grupo2 = lista_grupo2_ as Gtk.TreeView; 
			
			nombrebd = _nombrebd_;
			checkbutton_especiales = checkbutton_especiales_;
			checkbutton_tarjeta = checkbutton_tarjeta_;
			id_tipopaciente = id_tipopaciente_;
			id_empresa = id_empresa_;
			id_aseguradora = id_aseguradora_;
			radiobutton_desglosado = radiobutton_desglosado_;
			radiobutton_con_iva = radiobutton_con_iva_;
			radiobutton_sin_iva = radiobutton_sin_iva_;
			
			entry_empresa_aseguradora = entry_empresa_aseguradora_ as Gtk.Entry;
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob(Gnome.PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog(trabajo, "LISTA_DE_PRECIOS", 0);
        	
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
                new PrintJobPreview(trabajo, "LISTA DE PRECIOS").Show();
                break;
        	}
			dialogo.Hide (); dialogo.Dispose ();
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
			Gnome.Print.Setfont (ContextoImp, fuente12);
			ContextoImp.MoveTo(180.5, 740);			ContextoImp.Show("LISTA DE PRECIOS");
			ContextoImp.MoveTo(180, 740);			ContextoImp.Show("LISTA DE PRECIOS");
			ContextoImp.MoveTo(295, 740);			ContextoImp.Show("--");
			if(this.checkbutton_tarjeta == true){
			ContextoImp.MoveTo(305, 740);			ContextoImp.Show("Tarjeta de Descuentos");
				}else{
				ContextoImp.MoveTo(305, 740);			ContextoImp.Show(entry_empresa_aseguradora.Text);
				}
      		if(this.radiobutton_desglosado == true ){
      			ContextoImp.MoveTo(350, 730);			ContextoImp.Show("Precio");
      			ContextoImp.MoveTo(390, 730);			ContextoImp.Show("Iva");
      			ContextoImp.MoveTo(425, 730);			ContextoImp.Show("Total");
      			if(this.checkbutton_tarjeta == true){
      				ContextoImp.MoveTo(465, 730);			ContextoImp.Show("% desc");
      				ContextoImp.MoveTo(510, 730);			ContextoImp.Show("Desc");
      				ContextoImp.MoveTo(550, 730);			ContextoImp.Show("T.total");
      				}
				contador+=1; 
      			}
      			
      		if(this.radiobutton_sin_iva == true){	
      			ContextoImp.MoveTo(350, 730);			ContextoImp.Show("Precio");
      			contador+=1; 
				}      	
			if(this.radiobutton_con_iva == true){	
      			ContextoImp.MoveTo(350, 730);			ContextoImp.Show("Precio");
      			ContextoImp.MoveTo(390, 730);			ContextoImp.Show("Iva");
      			ContextoImp.MoveTo(425, 730);			ContextoImp.Show("Total");
      			contador+=1; 
				}      	
      		}
      	
		void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion,int contador_)
		{
	        if (contador_ > 71 )
	        {
	        	numpage +=1;
	        	ContextoImp.ShowPage();
				ContextoImp.BeginPage("Pagina "+numpage.ToString());
				imprime_encabezado(ContextoImp,trabajoImpresion);
	     		Gnome.Print.Setfont (ContextoImp, fuente6);
	        	contador=1;
	        	filas=720;
	        }
		}
	
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{        		
			TreeIter iter;	
			//variables para lectura de treviews por fila
			string numeros_seleccionado = ""; 
			string numeros_seleccionado1 = "";
			string numeros_seleccionado2 = "";
			string variable_paso_03 = "";
			string variable_paso_03_1 = "";
			string variable_paso_03_2 = "";
			int variable_paso_02 = 0;
			int variable_paso_02_1 = 0;
			int variable_paso_02_2 = 0;			
			string precio_empresa_aseguradora = "";
			string empresa_aseguradora = "";
		
			if (checkbutton_tarjeta == true){
				if (this.id_aseguradora > 0){
					empresa_aseguradora = this.id_aseguradora.ToString().Trim();
				}else{
					empresa_aseguradora = this.id_empresa.ToString().Trim();
				}
				precio_empresa_aseguradora = ",to_char(precio_producto_"+this.id_tipopaciente.ToString().Trim()+empresa_aseguradora+",'999999999.99') AS precioempreaseg ";
			}			
			if (this.checkbutton_especiales == true){
				if (this.id_aseguradora > 0){
					empresa_aseguradora = this.id_aseguradora.ToString().Trim();
				}else{
					empresa_aseguradora = this.id_empresa.ToString().Trim();
				}
				precio_empresa_aseguradora = ",to_char(precio_producto_"+this.id_tipopaciente.ToString().Trim()+empresa_aseguradora.ToString().Trim() +",'999999999.99') AS precioempreaseg ";
			}
			
			// (lee 1 treeview) en este caso 3 treeview y selecciona dicho campo boleano marcado en la lista(palomeado)
 			if (treeViewEnginegrupos.GetIterFirst (out iter)){
 				if ((bool) this.lista_grupo.Model.GetValue (iter,0) == true){
 					numeros_seleccionado = (string) lista_grupo.Model.GetValue (iter,2);
 					variable_paso_02_1 += 1;		
 				}
 				while (treeViewEnginegrupos.IterNext(ref iter)){
 					if ((bool) lista_grupo.Model.GetValue (iter,0) == true){
 				    	if (variable_paso_02_1 == 0){ 				    	
 							numeros_seleccionado = (string) lista_grupo.Model.GetValue (iter,2);
 							variable_paso_02_1 += 1;
 						}else{
 							variable_paso_03 = (string) lista_grupo.Model.GetValue (iter,2);
 							numeros_seleccionado = numeros_seleccionado.Trim() + "','" + variable_paso_03.Trim();
 						}
 					}
 				}
 			}
 			variable_paso_02 = 0;
 			if (treeViewEnginegrupos1.GetIterFirst (out iter)){
 				if ((bool) this.lista_grupo1.Model.GetValue (iter,0) == true){
 					numeros_seleccionado1 = (string) lista_grupo1.Model.GetValue (iter,2);
 					variable_paso_02 += 1;		
 				}
 				while (treeViewEnginegrupos1.IterNext(ref iter)){
 					if ((bool) lista_grupo1.Model.GetValue (iter,0) == true){
 				   		if (variable_paso_02 == 0){ 				    	
 							numeros_seleccionado1 = (string) lista_grupo1.Model.GetValue (iter,2);
 							variable_paso_02 += 1;
 						}else{
 							variable_paso_03_1 = (string) lista_grupo1.Model.GetValue (iter,2);
 							numeros_seleccionado1 = numeros_seleccionado1.Trim() + "','" + variable_paso_03_1.Trim();
 						} 				
 					}
 				}
 			}
 			
 			variable_paso_02_2 = 0;
 			if (treeViewEnginegrupos2.GetIterFirst (out iter)){
 				if ((bool) this.lista_grupo2.Model.GetValue (iter,0) == true){
 					numeros_seleccionado2 = (string) lista_grupo2.Model.GetValue (iter,2);
 					variable_paso_02_2 += 1;		
 				}
 				while (treeViewEnginegrupos2.IterNext(ref iter)){
 					if ((bool) lista_grupo2.Model.GetValue (iter,0) == true){
 				   		if (variable_paso_02_2 == 0){ 				    	
 							numeros_seleccionado2 = (string) lista_grupo2.Model.GetValue (iter,2);
 							variable_paso_02_2 += 1;
 						}else{
 							variable_paso_03_2 = (string) lista_grupo2.Model.GetValue (iter,2);
 							numeros_seleccionado2 = numeros_seleccionado2.Trim() + "','" + variable_paso_03_2.Trim();
 						} 				
 					}
 				}
 			}
 				//declarando variables de los querys
 			string query_tarjetas_descuento1 = "";
			string query_tarjetas_descuento2 = "";		
			string query_precios_especiales = "";
 			string query_in_grupo2 = "";
 			string query_in_grupo = "";
 			string query_in_grupo1 = "";
 			
 			if (checkbutton_tarjeta == true){
				precio_empresa_aseguradora = "";
				query_tarjetas_descuento1 = ", to_char(hscmty_productos.precio_producto_publico - (hscmty_productos.precio_producto_publico * hscmty_productos.porcentage_descuento) / 100,'99999999.99') AS precio_tarjeta ";
				query_tarjetas_descuento2 = "hscmty_productos.aplica_descuento = true ";
				query_in_grupo  = " ";
 				query_in_grupo1 = " ";
 				query_in_grupo2 = " ";
			}else{				
				if (variable_paso_02_1 > 0){
	 				query_in_grupo = "hscmty_productos.id_grupo_producto IN ('"+numeros_seleccionado+"') ";  				
	 			}
	 			if (variable_paso_02_2 > 0){
	 				query_in_grupo1 = "AND hscmty_productos.id_grupo1_producto IN ('"+numeros_seleccionado1+"') ";  				
	 			}
	 			if (variable_paso_02_2 > 0){
 					query_in_grupo2 = "AND hscmty_productos.id_grupo2_producto IN ('"+numeros_seleccionado2+"') ";  				
 				}				
 			}
 			if (this.checkbutton_especiales == true){
 				query_precios_especiales = "precio_producto_"+this.id_tipopaciente.ToString().Trim()+empresa_aseguradora.ToString().Trim()+" > 0 ";
 				query_in_grupo  = " ";
 				query_in_grupo1 = " ";
 				query_in_grupo2 = " ";
 			}
			
			
			NpgsqlConnection conexion; 
        	conexion = new NpgsqlConnection (connectionString+nombrebd);
        	// Verifica que la base de dato s este conectada
        	try{
	 			conexion.Open ();
	        	NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand ();
	        	        	  
	           	comando.CommandText ="SELECT to_char(hscmty_productos.id_producto,'999999999999') AS codProducto,"+
							"hscmty_productos.descripcion_producto,hscmty_productos.nombre_articulo,hscmty_productos.nombre_generico_articulo, "+
							"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(precio_producto_publico1,'99999999.99') AS preciopublico1,"+
							"to_char(cantidad_de_embalaje,'99999999.99') AS cantidadembalaje,"+							
							"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,cobro_activo,costo_unico,"+
							"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(hscmty_productos.id_grupo_producto,'99999') AS idgrupoproducto,hscmty_productos.id_grupo_producto, "+
							"to_char(hscmty_productos.id_grupo1_producto,'99999') AS idgrupo1producto,hscmty_productos.id_grupo1_producto, "+
							"to_char(hscmty_productos.id_grupo2_producto,'99999') AS idgrupo2producto,hscmty_productos.id_grupo2_producto, "+
							"to_char(porcentage_ganancia,'99999.999') AS porcentageutilidad,to_char(costo_producto,'999999999.99') AS costoproducto "+
							precio_empresa_aseguradora+" "+	
							query_tarjetas_descuento1+" "+
							"FROM hscmty_productos,hscmty_grupo_producto,hscmty_grupo1_producto,hscmty_grupo2_producto "+
							"WHERE "+query_in_grupo+
							query_in_grupo1+
							query_in_grupo2+
							query_precios_especiales+
							query_tarjetas_descuento2+
							"AND hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
							"AND hscmty_productos.id_grupo1_producto = hscmty_grupo1_producto.id_grupo1_producto "+
							"AND hscmty_productos.id_grupo2_producto = hscmty_grupo2_producto.id_grupo2_producto "+
							"ORDER BY hscmty_productos.id_grupo_producto,hscmty_productos.id_grupo1_producto,hscmty_productos.id_grupo2_producto,hscmty_productos.id_producto ;";
	        	//Console.WriteLine(comando.CommandText);
	        	NpgsqlDataReader lector = comando.ExecuteReader ();
	        	ContextoImp.BeginPage("Pagina 1");
	        	imprime_encabezado(ContextoImp,trabajoImpresion);
				
		     	int grupo = 0;	   	
		     	int grupo1 = 0;
		     	int grupo2 = 0;
		     	string toma_descrip_prod;
				Gnome.Print.Setfont (ContextoImp, fuente6);
		     	if (lector.Read()){
					grupo2 = (int) lector["id_grupo2_producto"];
									
					if(grupo != (int) lector["id_grupo_producto"]){					
						ContextoImp.MoveTo(20, filas); ContextoImp.Show((string) lector["descripcion_grupo_producto"]);
						grupo = (int) lector["id_grupo_producto"];						
						filas-=10;
						contador+=1;
        				salto_pagina(ContextoImp,trabajoImpresion,contador);
					}							
					if(grupo1 != (int) lector["id_grupo1_producto"]){
						ContextoImp.MoveTo(25, filas); ContextoImp.Show((string) lector["descripcion_grupo1_producto"]);
						grupo1 = (int) lector["id_grupo1_producto"];
						filas-=10;
						contador+=1;
        				salto_pagina(ContextoImp,trabajoImpresion,contador);
					}						
					if(grupo2 != (int) lector["id_grupo2_producto"]){
						ContextoImp.MoveTo(30, filas); ContextoImp.Show((string) lector["descripcion_grupo2_producto"]);
						grupo2 = (int) lector["id_grupo2_producto"];
						filas-=10;
						contador+=1;
        				salto_pagina(ContextoImp,trabajoImpresion,contador);
					}
					ContextoImp.MoveTo(35, filas);		ContextoImp.Show((string) lector["codProducto"]);
					
					toma_descrip_prod = (string) lector["descripcion_producto"];
					
					if(toma_descrip_prod.Length > 65){
							toma_descrip_prod = toma_descrip_prod.Substring(0,64);
					}  				
					ContextoImp.MoveTo(78, filas);		ContextoImp.Show(toma_descrip_prod);

					// operaciones para el iva
					if(this.checkbutton_especiales == true){
						precio_por_cantidad = decimal.Parse((string) lector["precioempreaseg"]);
					}else{
						precio_por_cantidad = decimal.Parse((string) lector["preciopublico"]);
					} 
					ivaproducto = (precio_por_cantidad*15)/100;	
					
					total = ivaproducto + decimal.Parse((string) lector["preciopublico"]);
				
					if(this.radiobutton_con_iva == true){
						if(this.checkbutton_tarjeta == true){
							//operaciones para iva
							precio_por_cantidad2 = decimal.Parse((string) lector["precio_tarjeta"]); 
							ivaproducto2 = (precio_por_cantidad2*15)/100;	
						
							total2 = ivaproducto2 + decimal.Parse((string) lector["precio_tarjeta"]);
				
							ContextoImp.MoveTo(345, filas);		ContextoImp.Show(precio_por_cantidad2.ToString("C"));
							ContextoImp.MoveTo(390, filas);		ContextoImp.Show(ivaproducto2.ToString("C"));
							ContextoImp.MoveTo(430, filas);		ContextoImp.Show(total2.ToString("C"));			
						}else{
				
							ContextoImp.MoveTo(350, filas);		ContextoImp.Show(precio_por_cantidad.ToString("C"));
							ContextoImp.MoveTo(390, filas);		ContextoImp.Show(ivaproducto.ToString("C"));
							ContextoImp.MoveTo(430, filas);		ContextoImp.Show(total.ToString("C"));
						}
					}
					if(this.radiobutton_sin_iva == true){
						if(this.checkbutton_tarjeta == true){
							//operaciones para iva
							precio_por_cantidad2 = decimal.Parse((string) lector["precio_tarjeta"]); 
							ContextoImp.MoveTo(345, filas);		ContextoImp.Show(precio_por_cantidad2.ToString("C"));
									
						}else{
							ContextoImp.MoveTo(350, filas);		ContextoImp.Show(precio_por_cantidad.ToString("C"));
						}
					}
					if(this.radiobutton_desglosado == true ){
						if(this.checkbutton_tarjeta == true){
						
							//operaciones para iva
							precio_por_cantidad2 = decimal.Parse((string) lector["precio_tarjeta"]); 
							ivaproducto2 = (precio_por_cantidad2*15)/100;	
						
							total2 = ivaproducto2 + decimal.Parse((string) lector["precio_tarjeta"]);
							// opreraciones de descuento
						  	descuento = decimal.Parse((string) lector["porcentagesdesc"]);
							descuento_pesos = (descuento * precio_por_cantidad2) / 100;
							total_desc = descuento_pesos + total2;
							
							ContextoImp.MoveTo(345, filas);		ContextoImp.Show((string) lector["precio_tarjeta"]);
							ContextoImp.MoveTo(390, filas);		ContextoImp.Show(ivaproducto2.ToString("C"));
							ContextoImp.MoveTo(430, filas);		ContextoImp.Show(total2.ToString("C"));		
							ContextoImp.MoveTo(470, filas);		ContextoImp.Show((string) lector["porcentagesdesc"]);		
							ContextoImp.MoveTo(510, filas);		ContextoImp.Show(descuento_pesos.ToString("C"));			
							ContextoImp.MoveTo(550, filas);		ContextoImp.Show(total_desc.ToString("C"));		
						}else{
							ContextoImp.MoveTo(345, filas);		ContextoImp.Show(precio_por_cantidad.ToString("C"));
							ContextoImp.MoveTo(390, filas);		ContextoImp.Show(ivaproducto.ToString("C"));
							ContextoImp.MoveTo(430, filas);		ContextoImp.Show(total.ToString("C"));
						}
					}
					filas-=10;
					contador+=1;
        			salto_pagina(ContextoImp,trabajoImpresion,contador);
					//Console.WriteLine(filas);
					while (lector.Read()){
						if(grupo != (int) lector["id_grupo_producto"]){					
							ContextoImp.MoveTo(20, filas); ContextoImp.Show((string) lector["descripcion_grupo_producto"]);
							grupo = (int) lector["id_grupo_producto"];
							filas-=10;
							contador+=1;
        					salto_pagina(ContextoImp,trabajoImpresion,contador);
						}
						if(grupo1 != (int) lector["id_grupo1_producto"]){
							ContextoImp.MoveTo(25, filas); ContextoImp.Show((string) lector["descripcion_grupo1_producto"]);
							grupo1 = (int) lector["id_grupo1_producto"];
							filas-=10;
							contador+=1;
        					salto_pagina(ContextoImp,trabajoImpresion,contador);
						}
						if(grupo2 != (int) lector["id_grupo2_producto"]){
							ContextoImp.MoveTo(30, filas); ContextoImp.Show((string) lector["descripcion_grupo2_producto"]);
							grupo2 = (int) lector["id_grupo2_producto"];
							filas-=10;
							contador+=1;
        					salto_pagina(ContextoImp,trabajoImpresion,contador);
						}
						ContextoImp.MoveTo(35, filas);		ContextoImp.Show((string) lector["codProducto"]);
					
						toma_descrip_prod = (string) lector["descripcion_producto"];
						if(toma_descrip_prod.Length > 65){
							toma_descrip_prod = toma_descrip_prod.Substring(0,64);
						}  	
						ContextoImp.MoveTo(78, filas);		ContextoImp.Show(toma_descrip_prod);
						
						if(this.checkbutton_especiales == true){
							precio_por_cantidad = decimal.Parse((string) lector["precioempreaseg"]);
						}else{
							precio_por_cantidad = decimal.Parse((string) lector["preciopublico"]);
						} 
						ivaproducto = (precio_por_cantidad*15)/100;	
					
						total = ivaproducto + precio_por_cantidad;
				
						if(this.radiobutton_con_iva == true){
							if(this.checkbutton_tarjeta == true){
								//operaciones para iva
								precio_por_cantidad2 = decimal.Parse((string) lector["precio_tarjeta"]); 
								ivaproducto2 = (precio_por_cantidad2*15)/100;	
							
								total2 = ivaproducto2 + decimal.Parse((string) lector["precio_tarjeta"]);
					
								ContextoImp.MoveTo(345, filas);		ContextoImp.Show(precio_por_cantidad2.ToString("C"));
								ContextoImp.MoveTo(390, filas);		ContextoImp.Show(ivaproducto2.ToString("C"));
								ContextoImp.MoveTo(430, filas);		ContextoImp.Show(total2.ToString("C"));			
							}else{
								ContextoImp.MoveTo(350, filas);		ContextoImp.Show(precio_por_cantidad.ToString("C"));
								ContextoImp.MoveTo(390, filas);		ContextoImp.Show(ivaproducto.ToString("C"));
								ContextoImp.MoveTo(430, filas);		ContextoImp.Show(total.ToString("C"));
							}
						}
						if(this.radiobutton_sin_iva == true){
							if(this.checkbutton_tarjeta == true){
								//operaciones para iva
								precio_por_cantidad2 = decimal.Parse((string) lector["precio_tarjeta"]); 
								ContextoImp.MoveTo(345, filas);		ContextoImp.Show(precio_por_cantidad2.ToString("C"));		
							}else{
								ContextoImp.MoveTo(350, filas);		ContextoImp.Show((string) lector["preciopublico"]);
							}
						}	
						if(this.radiobutton_desglosado == true ){
							if(this.checkbutton_tarjeta == true){
							//operaciones para iva
								precio_por_cantidad2 = decimal.Parse((string) lector["precio_tarjeta"]); 
								ivaproducto2 = (precio_por_cantidad2*15)/100;	
							
								total2 = ivaproducto2 + decimal.Parse((string) lector["precio_tarjeta"]);
								// opreraciones de descuento
						  		descuento = decimal.Parse((string) lector["porcentagesdesc"]);
								descuento_pesos = (descuento * precio_por_cantidad2) / 100;
								total_desc = descuento_pesos + total2;
							
								ContextoImp.MoveTo(345, filas);		ContextoImp.Show((string) lector["precio_tarjeta"]);
								ContextoImp.MoveTo(390, filas);		ContextoImp.Show(ivaproducto2.ToString("C"));
								ContextoImp.MoveTo(430, filas);		ContextoImp.Show(total2.ToString("C"));		
								ContextoImp.MoveTo(470, filas);		ContextoImp.Show((string) lector["porcentagesdesc"]);		
								ContextoImp.MoveTo(510, filas);		ContextoImp.Show(descuento_pesos.ToString("C"));			
								ContextoImp.MoveTo(550, filas);		ContextoImp.Show(total_desc.ToString("C"));		
							}else{
								ContextoImp.MoveTo(345, filas);		ContextoImp.Show(precio_por_cantidad.ToString("C"));
								ContextoImp.MoveTo(390, filas);		ContextoImp.Show(ivaproducto.ToString("C"));
								ContextoImp.MoveTo(430, filas);		ContextoImp.Show(total.ToString("C"));
							}
						}
						filas-=10;
						contador+=1;
        				salto_pagina(ContextoImp,trabajoImpresion,contador);
					}
				} 
				ContextoImp.ShowPage();
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
		}
	}
}