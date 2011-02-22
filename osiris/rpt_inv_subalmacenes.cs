
using System;
using Gtk;
using Npgsql;
using Cairo;
using Pango;

namespace osiris
{

	public class rpt_inv_subalmacenes
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 70;
		int separacion_linea = 10;
		int numpage = 1;
		
		string titulo;
		
		int columna = 0;
		int fila = -90;
		int filas = 690;
		int contador = 1;
				
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
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public void rpt_subalmacenes ()
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			
			print = new PrintOperation ();
			print.JobName = "Reporte de Stock";	// Name of the report
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run(PrintOperationAction.PrintDialog, null);
					
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
			string toma_descrip_prod = "";
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			
			/*
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
			*/
		}
	 	
			
		public void rpt_traspasos()
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			escala_en_linux_windows = classpublic.escala_linux_windows;
			
			print = new PrintOperation ();
			print.JobName = "Reporte de Stock";	// Name of the report
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint1);
			print.DrawPage += new DrawPageHandler (OnDrawPage1);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run(PrintOperationAction.PrintDialog, null);
			
		}
		
		private void OnBeginPrint1 (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1;  // crea cantidad de copias del reporte			
			// para imprimir horizontalmente el reporte
			//print.PrintSettings.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage1 (object obj, Gtk.DrawPageArgs args)
		{			
			PrintContext context = args.Context;			
			ejecutar_consulta_reporte1(context);
		}
						
		void ejecutar_consulta_reporte1(PrintContext context)
		{   
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			string toma_descrip_prod = "";
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;	
			/*
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
				if (lector.Read()){
					ContextoImp.MoveTo(15, filas);		ContextoImp.Show((string) lector["codProducto".Trim()]);
					tomovalor1 = (string) lector["descripcion"];
					if(tomovalor1.Length > 65){
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
					if(tomovalor1.Length > 65){
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
			*/
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
	}
}
