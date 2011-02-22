// rpt_mov_productos.cs created with MonoDevelop
// User: ipena at 09:30 a 25/04/2008
//      AUTOR:  ISRAEL (Programacion)
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//


namespace osiris
{
	public class imprime_mov_productos
	{
		string connectionString;						
		string nombrebd;
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
		// Declarando el treeview
		Gtk.TreeView lista_resumen_productos;
		Gtk.TreeStore treeViewEngineResumen;
		string titulo = "MOVIMIENTOS DE PRODUCTOS POR PACIENTE";
		int contador = 1;
		int numpage = 1;
		int filas = -75;
		string query1  = " ";
		string query_fechas =" ";
		string CantidadAplicada;
		string idproducto;
	    string descripcionproducto;
		string foliodeservicio;
		string pidpaciente;
		string nombrepaciente; 
		string idtipoadmisiones;  
		string descripcionadmisiones;
		//public string fechahoracreacion;
		string desripcionproductorecortado;
		string entry_dia1;
		string entry_mes1;
		string entry_ano1; 
		string entry_dia2;
		string entry_mes2;
		string entry_ano2; 
		string entry_total_aplicado;
		string nombrepacienterecortado;
		string titulopagina;
	 	//Declaracion de ventana de error:
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		                                                                                                                   
		public imprime_mov_productos(string entry_total_aplicado_,string entry_dia1_,string entry_mes1_,string entry_ano1_, string entry_dia2_,string entry_mes2_,string entry_ano2_  ,object _lista_resumen_productos_,object _treeViewEngineResumen_, string _query1_,  string _nombrebd_, string titulopagina_)
		{
			lista_resumen_productos = _lista_resumen_productos_ as Gtk.TreeView;
			treeViewEngineResumen =  _treeViewEngineResumen_ as Gtk.TreeStore;
			query1  = _query1_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			titulopagina = titulopagina_;
			entry_dia1 = entry_dia1_; 
			entry_mes1 = entry_mes1_;
			entry_ano1 = entry_ano1_;
			entry_dia2 = entry_dia2_; 
			entry_mes2= entry_mes2_;
			entry_ano2 = entry_ano2_;
			entry_total_aplicado = entry_total_aplicado_;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			
			print = new PrintOperation ();
			print.JobName = "Movimiento de Productos por Paciente";	// Name of the report
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
			string toma_descrip_prod = "";
			string fechahoracreacion = "";      
			/*
			contador = 0;
			numpage = 1;
			
			ContextoImp.BeginPage("Pagina "+numpage.ToString());
			//Encabezado de pagina
			ContextoImp.Rotate(90);
			// Cambiar la fuente
			Gnome.Print.Setfont(ContextoImp,fuente5);
			ContextoImp.MoveTo(320.7, -40);	       ContextoImp.Show( titulo+"");
			Gnome.Print.Setfont(ContextoImp,fuente2);
			ContextoImp.MoveTo(95, -30);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(95, -40);			ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(95, -50);			ContextoImp.Show("Conmutador:");
			ContextoImp.MoveTo(445, -50);			ContextoImp.Show("PAGINA "+numpage);
			ContextoImp.MoveTo(399, -60);			ContextoImp.Show("Fecha Impresion: "+DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
			ContextoImp.MoveTo(370, -70);			ContextoImp.Show("Rango de Fechas :  Inicio " +entry_dia1+ "/"+entry_mes1+ "/"+entry_ano1+   "  Termino "  +entry_dia2+ "/"+entry_mes2+ "/"+entry_ano2);
			ContextoImp.MoveTo(95, -80);			ContextoImp.Show("Cantidad:");
			ContextoImp.MoveTo(145, -80);			ContextoImp.Show("ID:");
			ContextoImp.MoveTo(195, -80);			ContextoImp.Show("Descripción Producto:");
			ContextoImp.MoveTo(380, -80);			ContextoImp.Show("Folio:");
			ContextoImp.MoveTo(410, -80);			ContextoImp.Show("PID:");
			ContextoImp.MoveTo(440, -80);			ContextoImp.Show("Nom.Paciente:");
			ContextoImp.MoveTo(695, -80);			ContextoImp.Show("Departamento:");
			ContextoImp.MoveTo(625, -80);			ContextoImp.Show("Fecha de Cargo");
			
			filas = -90;
			
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
						            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
											
				comando.CommandText = query1;
				//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
					                    
				while (lector.Read()){
					CantidadAplicada = (string) lector["cantidadaplicada"];
					idproducto  = (string) lector["idproducto"];
					descripcionproducto = (string) lector["descripcion_producto"];
					foliodeservicio = 	(string) lector["foliodeservicio"];
					pidpaciente = (string) lector["pidpaciente"];
					nombrepaciente =  (string) lector["nombre_paciente"];
					descripcionadmisiones = (string) lector["descripcion_admisiones"];								
					fechahoracreacion = (string) lector["fechahoracreacion"];
						                   
					if(descripcionproducto.Length > 49){
						desripcionproductorecortado = descripcionproducto.Substring(0,46)  ; 
					}else{
						desripcionproductorecortado = descripcionproducto;
					}
					if (nombrepaciente.Length > 49){
						nombrepacienterecortado = nombrepaciente.Substring(0,46)  ; 
					}else{
						nombrepacienterecortado = nombrepaciente;
					}
					Gnome.Print.Setfont(ContextoImp,fuente1);
											
					ContextoImp.MoveTo(95, filas);		ContextoImp.Show((CantidadAplicada).ToString().Trim());
					ContextoImp.MoveTo(140, filas);		ContextoImp.Show(idproducto);
					ContextoImp.MoveTo(195, filas);		ContextoImp.Show(desripcionproductorecortado);
					ContextoImp.MoveTo(370, filas);		ContextoImp.Show(foliodeservicio);	
					ContextoImp.MoveTo(400, filas);		ContextoImp.Show(pidpaciente);	
					ContextoImp.MoveTo(440, filas);		ContextoImp.Show(nombrepacienterecortado);
					ContextoImp.MoveTo(625, filas);		ContextoImp.Show(fechahoracreacion);
					ContextoImp.MoveTo(695, filas);		ContextoImp.Show(descripcionadmisiones);			
					contador += 1;
					filas -= 10;
					salto_pagina(ContextoImp,trabajoImpresion);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																MessageType.Error, 
																ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();
			Gnome.Print.Setfont(ContextoImp,fuente);
			ContextoImp.MoveTo(95,-580);
			ContextoImp.Show("CANTIDAD DEL TOTAL APLICADO: "   +entry_total_aplicado); 
			ContextoImp.MoveTo(120, filas);	ContextoImp.Show(toma_descrip_prod);
			ContextoImp.ShowPage();
			*/
		}		
	}
}
		     
	

                                                        
