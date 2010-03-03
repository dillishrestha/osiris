// rpt_mov_productos.cs created with MonoDevelop
// User: ipena at 09:30 a 25/04/2008
//      AUTOR:  ISRAEL (Programacion)
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
		string titulo = "REPORTE  MOVIMIENTOS DE PRODUCTOS POR PACIENTE";
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
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default ());
			Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, titulopagina, 0);
			int respuesta = dialogo.Run ();
		       	            
			if (respuesta == (int) PrintButtons.Cancel){  //boton Cancelar
				dialogo.Hide (); 
				dialogo.Dispose (); 
				return;
			}
			Gnome.PrintContext ctx = trabajo.Context;
			ComponerPagina(ctx, trabajo); 
			trabajo.Close();
			switch (respuesta){  //imprimir
				case (int) PrintButtons.Print:   
					trabajo.Print (); 
					break;
				    
				case (int) PrintButtons.Preview:
					new PrintJobPreview(trabajo, titulopagina).Show();
					break;
			}
			dialogo.Hide (); dialogo.Dispose ();
		}  
														
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{      
			string toma_descrip_prod = "";
			string fechahoracreacion = ""; 
		    			                
			// Crear una fuente 
			Gnome.Font fuente = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
			Gnome.Font fuente1 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
			Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
			Gnome.Font fuente5 = Gnome.Font.FindClosestFromWeightSlant("Bitstream Vera Sans", FontWeight.Bold ,false, 12);
			
			// ESTA FUNCION ES PARA QUE EL TEXTO SALGA EN NEGRITA
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
		}
		
		void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
			if (contador > 47 ){
				numpage +=1; 
				ContextoImp.Rotate(90);
				ContextoImp.ShowPage();				
				ContextoImp.BeginPage("Pagina "+numpage.ToString());
				Gnome.Font fuente5 = Gnome.Font.FindClosestFromWeightSlant("Bitstream Vera Sans", FontWeight.Bold ,false, 12);
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
				contador = 0;
			}
		}
	}
}
		     
	

                                                        
