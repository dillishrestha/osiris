
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using Gnome;

namespace osiris
{

	public class rpt_inv_subalmacenes
	{
		string titulo;
		
		int columna = 0;
		int fila = -90;
		int filas = 690;
		int contador = 1;
		int numpage = 1;
		
		string connectionString;
		string nombrebd;
		
		string query_grupo = " ";
		string query_grupo1 = " ";
		string query_grupo2 = " ";
		string query_stock = " ";
		string tiporeporte = "STOCK";
		
		int idsubalmacen;
		int idalmacendestino;
		string descsubalmacen;
		int tipoalmacen;
		
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		//Gnome.Font fuente5 = Gnome.Font.FindClosest("Luxi Sans", 5);
		Gnome.Font fuente6 = Gnome.Font.FindClosest("Luxi Sans", 6);
		Gnome.Font fuente7 = Gnome.Font.FindClosest("Luxi Sans", 7);
		Gnome.Font fuente8 = Gnome.Font.FindClosest("Luxi Sans", 8);//Bitstream Vera Sans
		Gnome.Font fuente9 = Gnome.Font.FindClosest("Luxi Sans", 9);
		//Gnome.Font fuente10 = Gnome.Font.FindClosest("Luxi Sans", 10);
		//Gnome.Font fuente11 = Gnome.Font.FindClosest("Luxi Sans", 11);
		//Gnome.Font fuente12 = Gnome.Font.FindClosest("Luxi Sans", 12);
		//Gnome.Font fuente36 = Gnome.Font.FindClosest("Luxi Sans", 36);
				
		class_conexion conexion_a_DB = new class_conexion();
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public void rpt_subalmacenes ()
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			titulo = "REPORTE DE STOCK SUB-ALMACEN";
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, titulo, 0);
        	int         respuesta = dialogo.Run ();
        
			if (respuesta == (int) Gnome.PrintButtons.Cancel){
				dialogo.Hide (); 		dialogo.Dispose (); 
				return;
			}

        	Gnome.PrintContext ctx = trabajo.Context;        
        	ComponerPagina(ctx, trabajo); 
        	trabajo.Close();
             
        	switch (respuesta)
        	{
                  case (int) Gnome.PrintButtons.Print:   
                  		trabajo.Print (); 
                  		break;
                  case (int) Gnome.PrintButtons.Preview:
                      	new Gnome.PrintJobPreview(trabajo, titulo).Show();
                        break;
        	}
        	dialogo.Hide (); dialogo.Dispose ();
		}
		
		void ComponerPagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{	
			filas=690;
			contador=1;
			string tomovalor1 = "";
			int contadorprocedimientos = 0;
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT osiris_catalogo_almacenes.id_almacen,to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
								"osiris_productos.descripcion_producto AS descripcion, "+
								"to_char(osiris_catalogo_almacenes.stock,'999999999999.99') AS stock,"+
								"to_char(osiris_catalogo_almacenes.minimo_stock,'999999999999.99') AS minstock,"+
								"to_char(osiris_catalogo_almacenes.maximo,'999999999999.99') AS maxstock,"+
								"to_char(osiris_catalogo_almacenes.punto_de_reorden,'999999999999.99') AS reorden,"+
								"to_char(osiris_catalogo_almacenes.fechahora_ultimo_surtimiento,'yyyy-MM-dd') AS fechsurti, "+
								"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto, "+
								"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
								"to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
								"to_char(costo_producto,'999999999.99') AS costoproducto, "+
								"to_char(cantidad_de_embalaje,'999999999.99') AS embalaje "+
								"FROM osiris_catalogo_almacenes,osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
								"WHERE osiris_catalogo_almacenes.id_producto = osiris_productos.id_producto "+ 
								"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
								"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
								"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
								"AND osiris_catalogo_almacenes.id_producto = osiris_productos.id_producto "+
								"AND osiris_grupo_producto.agrupacion4 = 'ALM' "+						
								"AND osiris_productos.cobro_activo = 'true' "+
								"AND osiris_catalogo_almacenes.eliminado = 'false' "+
								query_grupo+
								query_grupo1+
								query_grupo2+
								query_stock+
								" AND osiris_catalogo_almacenes.id_almacen = '"+idsubalmacen.ToString().Trim()+"' "+
								"ORDER BY descripcion_producto; ";															
				NpgsqlDataReader lector = comando.ExecuteReader ();
				ContextoImp.BeginPage("Pagina 1");
				imprime_encabezado(ContextoImp,trabajoImpresion);
				Gnome.Print.Setfont (ContextoImp, fuente6);
				if (lector.Read())
				{
					ContextoImp.MoveTo(15, filas);		ContextoImp.Show((string) lector["codProducto".Trim()]);
					tomovalor1 = (string) lector["descripcion"];
					if(tomovalor1.Length > 65)
					{
						tomovalor1 = tomovalor1.Substring(0,65); 
					}
					ContextoImp.MoveTo(65, filas);		ContextoImp.Show(tomovalor1);
					ContextoImp.MoveTo(300, filas);		ContextoImp.Show((string) lector["stock"]);
					ContextoImp.MoveTo(340, filas);		ContextoImp.Show("_____");
					ContextoImp.MoveTo(350, filas);		ContextoImp.Show((string) lector["minstock"]);
					ContextoImp.MoveTo(380, filas);		ContextoImp.Show((string) lector["maxstock"]);
					ContextoImp.MoveTo(430, filas);		ContextoImp.Show((string) lector["reorden".Trim()]);
					ContextoImp.MoveTo(510, filas);		ContextoImp.Show((string) lector["fechsurti"]);
					filas-=10;
					contador+=1;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
				}
				while (lector.Read()){
					ContextoImp.MoveTo(15, filas);		ContextoImp.Show((string) lector["codProducto".Trim()]);
					tomovalor1 = (string) lector["descripcion"];
					if(tomovalor1.Length > 65){
						tomovalor1 = tomovalor1.Substring(0,65); 
					}
					ContextoImp.MoveTo(65, filas);		ContextoImp.Show(tomovalor1);
					ContextoImp.MoveTo(300, filas);		ContextoImp.Show((string) lector["stock"]);
					ContextoImp.MoveTo(340, filas);		ContextoImp.Show("_____");
					ContextoImp.MoveTo(350, filas);		ContextoImp.Show((string) lector["minstock"]);
					ContextoImp.MoveTo(380, filas);		ContextoImp.Show((string) lector["maxstock"]);
					ContextoImp.MoveTo(430, filas);		ContextoImp.Show((string) lector["reorden".Trim()]);
					ContextoImp.MoveTo(510, filas);		ContextoImp.Show((string) lector["fechsurti"]);
					filas-=10;
					contador+=1;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
				}
				Gnome.Print.Setfont (ContextoImp, fuente9);
				contadorprocedimientos += 1;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				ContextoImp.ShowPage();
			}catch(NpgsqlException ex){
					Console.WriteLine("PostgresSQL error: {0}",ex.Message);
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();		msgBoxError.Destroy();
			}
				
		}
	 	
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
      		// Cambiar la fuente
      		Gnome.Print.Setfont(ContextoImp,fuente9);								
			ContextoImp.MoveTo(230, 730);			ContextoImp.Show("REPORTE DE STOCK");	
			ContextoImp.MoveTo(325, 730);			ContextoImp.Show(descsubalmacen);
			Gnome.Print.Setfont (ContextoImp, fuente6);
			ContextoImp.MoveTo(19.7, 770);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(20, 770);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(19.7, 760);			ContextoImp.Show("Direccion: ");
			ContextoImp.MoveTo(20, 760);			ContextoImp.Show("Direccion: ");
			ContextoImp.MoveTo(19.7, 750);			ContextoImp.Show("Conmutador: ");
			ContextoImp.MoveTo(20, 750);			ContextoImp.Show("Conmutador: ");
			Gnome.Print.Setfont(ContextoImp,fuente7);
			ContextoImp.MoveTo(230, 50);			ContextoImp.Show("PAGINA "+numpage+"  Fecha Impresion: "+DateTime.Now.ToString("dd-MM-yyyy"));
			Gnome.Print.Setfont(ContextoImp,fuente8);
			ContextoImp.MoveTo(25, 710);		ContextoImp.Show("FOLIO");
			ContextoImp.MoveTo(110, 710);		ContextoImp.Show("DESCRIPCION");
			ContextoImp.MoveTo(315, 710);		ContextoImp.Show("STOCK");
			ContextoImp.MoveTo(355, 710);		ContextoImp.Show("MIN. STK");
			ContextoImp.MoveTo(395, 710);		ContextoImp.Show("MAX. STK");
			ContextoImp.MoveTo(440, 710);		ContextoImp.Show("P. REORDEN");
			ContextoImp.MoveTo(500, 710);		ContextoImp.Show("FEC. ULT. SURTIDO");
		}    
      	
      	void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion,int contador_)
		{
	        if (contador_ > 60 ){
	        	numpage +=1;
	        	ContextoImp.ShowPage();
				ContextoImp.BeginPage("Pagina "+numpage.ToString());
				imprime_encabezado(ContextoImp,trabajoImpresion);
	     		Gnome.Print.Setfont (ContextoImp, fuente6);
	        	contador=1;
	        	filas=690;
	        }
		}
	
		public void rpt_traspasos()
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			titulo = "REPORTE DE ENVIO ENTRE SUB-ALMACENES";
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, titulo, 0);
        	int         respuesta = dialogo.Run ();
        
			if (respuesta == (int) Gnome.PrintButtons.Cancel){
				dialogo.Hide (); 		dialogo.Dispose (); 
				return;
			}

        	Gnome.PrintContext ctx = trabajo.Context;        
        	ComponerPagina1(ctx, trabajo); 
        	trabajo.Close();
             
        	switch (respuesta)
        	{
                  case (int) Gnome.PrintButtons.Print:   
                  		trabajo.Print (); 
                  		break;
                  case (int) Gnome.PrintButtons.Preview:
                      	new Gnome.PrintJobPreview(trabajo, titulo).Show();
                        break;
        	}
        	dialogo.Hide (); dialogo.Dispose ();
		}
		
		void ComponerPagina1(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{	
			filas=690;
			contador=1;
			string tomovalor1 = "";
			int contadorprocedimientos = 0;
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(osiris_his_solicitudes_deta.id_producto,'999999999999') AS codProducto,"+
								"osiris_productos.descripcion_producto AS descripcion, "+
								"to_char(osiris_his_solicitudes_deta.cantidad_autorizada,'999999999999') AS cantAut,"+
								"osiris_his_solicitudes_deta.id_almacen_origen, "+
								"osiris_his_solicitudes_deta.id_almacen, "+
								"to_char(osiris_his_solicitudes_deta.fechahora_traspaso,'yyyy-MM-dd HH:mm:ss') AS fechatraspaso, "+
								"osiris_his_solicitudes_deta.id_quien_traspaso, "+
								"osiris_his_solicitudes_deta.numero_de_traspaso "+
								"FROM osiris_his_solicitudes_deta,osiris_productos "+//,osiris_catalogo_almacenes,osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
								"WHERE osiris_his_solicitudes_deta.id_producto = osiris_productos.id_producto "+ 
								"AND osiris_his_solicitudes_deta.traspaso = 'true' "+
								"AND osiris_his_solicitudes_deta.eliminado = 'false' "+
								//query_stock+
								//" AND osiris_catalogo_almacenes.id_almacen = '"+idsubalmacen.ToString().Trim()+"' "+
								"ORDER BY osiris_productos.descripcion_producto; ";															
				NpgsqlDataReader lector = comando.ExecuteReader ();
				ContextoImp.BeginPage("Pagina 1");
				imprime_encabezado(ContextoImp,trabajoImpresion);
				Gnome.Print.Setfont (ContextoImp, fuente6);
				if (lector.Read())
				{
					ContextoImp.MoveTo(15, filas);		ContextoImp.Show((string) lector["codProducto".Trim()]);
					tomovalor1 = (string) lector["descripcion"];
					if(tomovalor1.Length > 65)
					{
						tomovalor1 = tomovalor1.Substring(0,65); 
					}
					ContextoImp.MoveTo(65, filas);		ContextoImp.Show(tomovalor1);
					ContextoImp.MoveTo(300, filas);		ContextoImp.Show((string) lector["cantAut"]);
					//ContextoImp.MoveTo(340, filas);		ContextoImp.Show("_____");
					ContextoImp.MoveTo(350, filas);		ContextoImp.Show((string) lector["id_almacen_origen"].ToString().Trim());
					ContextoImp.MoveTo(380, filas);		ContextoImp.Show((string) lector["id_almacen"].ToString().Trim());
					ContextoImp.MoveTo(430, filas);		ContextoImp.Show((string) lector["fechatraspaso"]);
					ContextoImp.MoveTo(510, filas);		ContextoImp.Show((string) lector["id_quien_traspaso"]);
					ContextoImp.MoveTo(600, filas);		ContextoImp.Show((string) lector["numero_de_traspaso"].ToString().Trim());
					filas-=10;
					contador+=1;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
				}
				while (lector.Read()){
					ContextoImp.MoveTo(15, filas);		ContextoImp.Show((string) lector["codProducto".Trim()]);
					tomovalor1 = (string) lector["descripcion"];
					if(tomovalor1.Length > 65)
					{
						tomovalor1 = tomovalor1.Substring(0,65); 
					}
					ContextoImp.MoveTo(65, filas);		ContextoImp.Show(tomovalor1);
					ContextoImp.MoveTo(300, filas);		ContextoImp.Show((string) lector["cantAut"]);
					//ContextoImp.MoveTo(340, filas);		ContextoImp.Show("_____");
					ContextoImp.MoveTo(350, filas);		ContextoImp.Show((string) lector["id_almacen_origen"].ToString().Trim());
					ContextoImp.MoveTo(380, filas);		ContextoImp.Show((string) lector["id_almacen"].ToString().Trim());
					ContextoImp.MoveTo(430, filas);		ContextoImp.Show((string) lector["fechatraspaso"]);
					ContextoImp.MoveTo(510, filas);		ContextoImp.Show((string) lector["id_quien_traspaso"]);
					ContextoImp.MoveTo(600, filas);		ContextoImp.Show((string) lector["numero_de_traspaso"].ToString().Trim());
					filas-=10;
					contador+=1;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
				}
				Gnome.Print.Setfont (ContextoImp, fuente9);
				contadorprocedimientos += 1;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				ContextoImp.ShowPage();
			}catch(NpgsqlException ex){
					Console.WriteLine("PostgresSQL error: {0}",ex.Message);
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();		msgBoxError.Destroy();
			}
				
		}
	}
}
