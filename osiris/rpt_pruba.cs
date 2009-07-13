// created on 21/01/2008 at 03:44 p
///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
//				  Daniel Olivares C. (Adecuaciones y reprogramacion) arcangeldoc@gmail.com 05/05/2007
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
// Proposito	: Impresion del procedimiento de cobranza 
// Objeto		: rpt_proc_cobranza.cs

/*
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
		public string connectionString = "Server=192.168.1.4;" +
        	    	                     "Port=5432;" +
            	    	                 "User ID=admin1;" +
                	    	             "Password=1qaz2wsx;";
  		
  		public Gtk.ListStore treeViewEnginegrupos;
		public Gtk.TreeView lista_grupo;
		
		public Gtk.ListStore treeViewEnginegrupos1;
		public Gtk.TreeView lista_grupo1;
		
		public Gtk.ListStore treeViewEnginegrupos2;
		public Gtk.TreeView lista_grupo2;	
		public string nombrebd;
		public bool checkbutton_especiales;
		public bool checkbutton_tarjeta;
		public int id_tipopaciente;
		public int id_empresa;
		public int id_aseguradora;

		public int filas=635;
		public int contador = 1;
		public int numpage = 1;
				
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
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
								int id_tipopaciente_,int id_empresa_,int id_aseguradora_)
		
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
					
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob(Gnome.PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog(trabajo, "LISTA_DE_PRECIOS", 0);
        	
        	//gnome_print_job_new  trabajo   = new gnome_print_job_new();
        	//PrintDialog dialogo   = new PrintDialog (trabajo, "RESUMEN DE FACTURA", 0);
        	
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
                new PrintJobPreview(trabajo, "LISTA_DE_PRECIOS").Show();
                break;
        	}
			dialogo.Hide (); dialogo.Dispose ();
		}
      	
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			        		Console.WriteLine("entra2");
      		// Cambiar la fuente
			Print.Setfont (ContextoImp, fuente6);
			ContextoImp.MoveTo(19.7, 770);			ContextoImp.Show("Hospital Santa Cecilia");
			ContextoImp.MoveTo(20, 770);			ContextoImp.Show("Hospital Santa Cecilia");
			ContextoImp.MoveTo(19.7, 760);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(20, 760);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(19.7, 750);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			ContextoImp.MoveTo(20, 750);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			
			//ContextoImp.MoveTo(484.7, 770);			ContextoImp.Show("Fo-tes-11/Rev.02/20-mar-07");
			//ContextoImp.MoveTo(485, 770);			ContextoImp.Show("Fo-tes-11/Rev.02/20-mar-07");
			   			
			Print.Setfont (ContextoImp, fuente12);
			ContextoImp.MoveTo(220.5, 740);			ContextoImp.Show("RESUMEN DE FACTURA");
			ContextoImp.MoveTo(221, 740);			ContextoImp.Show("RESUMEN DE FACTURA");
							
			Print.Setfont (ContextoImp, fuente10);
			ContextoImp.MoveTo(470.5, 755);			ContextoImp.Show("FOLIO DE ATENCION");
			ContextoImp.MoveTo(471, 755);			ContextoImp.Show("FOLIO DE ATENCION");
							
			
					    			
			
      }
      
      void genera_tabla(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
      {
      	//////////////////DIBUJANDO TABLA (START DRAWING TABLE)////////////////////////
		Print.Setfont (ContextoImp, fuente36);
		ContextoImp.MoveTo(20, 645);				ContextoImp.Show("____________________________");
				
		////COLUMNAS
		int filasl = 617;
		for (int i1=0; i1 < 28; i1++)//30 veces para tasmaño carta
		{	
            int columnas = 17;
			Print.Setfont (ContextoImp, fuente36);
			ContextoImp.MoveTo(columnas, filasl-.8);		ContextoImp.Show("|");
			ContextoImp.MoveTo(columnas+555, filasl);		ContextoImp.Show("|");
			filasl-=20;
		}
		//columnas tenues
		//int filasc =640;
		Print.Setfont (ContextoImp, fuente36);
		ContextoImp.MoveTo(20,73);		ContextoImp.Show("____________________________");
		///FIN DE DIBUJO DE TABLA (END DRAWING TABLE)///////
    }
    
    void imprime_titulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion, string descrp_admin)
    {
    	Print.Setfont (ContextoImp, fuente7);
		ContextoImp.MoveTo(20, filas+8);
		ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
		Print.Setfont (ContextoImp, fuente9);
		//LUGAR DE CARGO
		ContextoImp.MoveTo(200.5, filas);			ContextoImp.Show(descrp_admin.ToString().ToUpper());//+"  "+fech.ToString());//635
		ContextoImp.MoveTo(201, filas);				ContextoImp.Show(descrp_admin.ToString().ToUpper());//+"  "+fech.ToString());//635
		Print.Setfont (ContextoImp, fuente7);
		ContextoImp.MoveTo(20, filas-2);//633
		ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
		//genera_lineac(ContextoImp, trabajoImpresion);
		filas-=10;
		Print.Setfont (ContextoImp, fuente7);
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
     		Print.Setfont (ContextoImp, fuente7);
        	contador=1;
        	filas=635;
        }
       //Console.WriteLine("contador despues del if: "+contador_.ToString());
	}
	
	void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
	{        		

			TreeIter iter;	
			string numeros_seleccionado = ""; 
			string numeros_seleccionado1 = "";
			string numeros_seleccionado2 = "";
			string variable_paso_03 = "";
			string variable_paso_03_1 = "";
			string variable_paso_03_2 = "";
			int variable_paso_02 = 0;
			string precio_empresa_aseguradora = "";
			string empresa_aseguradora = "";
			
			if (this.id_aseguradora > 0){
				empresa_aseguradora = this.id_aseguradora.ToString().Trim();
			}else{
				empresa_aseguradora = this.id_empresa.ToString().Trim();
			}			
				
			precio_empresa_aseguradora = "precio_producto_"+this.id_tipopaciente.ToString().Trim()+empresa_aseguradora;
			
			
			// lee un treeview y selecciona dicho campo boleano marcado (palomeado)
 			if (treeViewEnginegrupos.GetIterFirst (out iter)){
 				if ((bool) this.lista_grupo.Model.GetValue (iter,0) == true){
 					numeros_seleccionado = (string) lista_grupo.Model.GetValue (iter,2);
 					variable_paso_02 += 1;		
 				}
 				while (treeViewEnginegrupos.IterNext(ref iter)){
 					if ((bool) lista_grupo.Model.GetValue (iter,0) == true){
 				    	if (variable_paso_02 == 0){ 				    	
 							numeros_seleccionado = (string) lista_grupo.Model.GetValue (iter,2);
 							variable_paso_02 += 1;
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
 			
 			variable_paso_02 = 0;
 			if (treeViewEnginegrupos2.GetIterFirst (out iter)){
 				if ((bool) this.lista_grupo2.Model.GetValue (iter,0) == true){
 					numeros_seleccionado2 = (string) lista_grupo2.Model.GetValue (iter,2);
 					variable_paso_02 += 1;		
 				}
 				while (treeViewEnginegrupos2.IterNext(ref iter)){
 					if ((bool) lista_grupo2.Model.GetValue (iter,0) == true){
 				   		if (variable_paso_02 == 0){ 				    	
 							numeros_seleccionado2 = (string) lista_grupo2.Model.GetValue (iter,2);
 							variable_paso_02 += 1;
 						}else{
 							variable_paso_03_2 = (string) lista_grupo2.Model.GetValue (iter,2);
 							numeros_seleccionado2 = numeros_seleccionado2.Trim() + "','" + variable_paso_03_2.Trim();
 						} 				
 					}
 				}
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
						"to_char(porcentage_ganancia,'99999.999') AS porcentageutilidad,to_char(costo_producto,'999999999.99') AS costoproducto,"+
						precio_empresa_aseguradora+" "+						
						"FROM hscmty_productos,hscmty_grupo_producto,hscmty_grupo1_producto,hscmty_grupo2_producto "+
						"WHERE hscmty_productos.id_grupo_producto IN ('"+numeros_seleccionado+"') "+
						"AND hscmty_productos.id_grupo1_producto IN ('"+numeros_seleccionado1+"') "+
						"AND hscmty_productos.id_grupo2_producto IN ('"+numeros_seleccionado2+"') "+
						"AND hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
						"AND hscmty_productos.id_grupo1_producto = hscmty_grupo1_producto.id_grupo1_producto "+
						"AND hscmty_productos.id_grupo2_producto = hscmty_grupo2_producto.id_grupo2_producto;";
        	
        	NpgsqlDataReader lector = comando.ExecuteReader ();
        	ContextoImp.BeginPage("Pagina 1");
        	
				imprime_encabezado(ContextoImp,trabajoImpresion);
     		   	
     		   	
     		   	
     		   	
			while (lector.Read())
				
        			{
			


							
						
						
					}
			ContextoImp.ShowPage();
			}catch (NpgsqlException ex){
			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			
			
		}
	}

	
			
}}		*/