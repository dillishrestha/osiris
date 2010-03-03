// rpt_requisicion_compras.cs created with MonoDevelop
// User: ipena at 11:56 a 10/10/2008
// created on 10/10/2008 at 10:21 a
// ing.R. Israel Peña Gonzalez
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using Gnome;
using System.Collections;

namespace osiris
{
	public class rpt_requisicion_compras
	{
		string connectionString;						
		string nombrebd;
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
    	
    	string querylist = "";
    	
    	// Declarando el treeview
		Gtk.TreeView lista_requisicion_productos;
		Gtk.TreeStore treeViewEngineRequisicion;
    		
    	string titulo = "REQUISICION DE COMPRAS ";
    	string requisicion;
		string status_requisicion;
		string fecha_solicitud;
		string dia_requerida;
		string mes_requerida;
		string ano_requerida;
		string observaciones;
		string nombre_prodrequisado;
		string totalitems_productos;
		
		string descripcion_tipo_requi;
    	string descripinternamiento2;
    	string descripinternamiento;
    	string solicitante;
		
		string nombrepacienterecortado;
		
		string cantidadsolicitada;
		string descripcionproducto;
		string unidadproducto;
		
		int contador = 1;
		int numpage = 1;
		int filas =-174;
    		
    	//Declaracion de ventana de error:
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
	
		public rpt_requisicion_compras(string nombrebd_,string requisicion_,string status_requisicion_,string fecha_solicitud_,string dia_requerida_,
									string mes_requerida_,string ano_requerida_,string observaciones_,string nombre_prodrequisado_,string totalitems_productos_,
									object lista_requisicion_productos_,object treeViewEngineRequisicion_)
    	{
    	 	connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
    	 	requisicion = requisicion_;
    		status_requisicion = status_requisicion_;
    		fecha_solicitud = fecha_solicitud_;
    		dia_requerida = dia_requerida_;
    		mes_requerida = mes_requerida_;
    		ano_requerida = ano_requerida_;
    		observaciones = observaciones_;
    		nombre_prodrequisado = nombre_prodrequisado_;
    		totalitems_productos = totalitems_productos_;
    		lista_requisicion_productos = lista_requisicion_productos_ as Gtk.TreeView;
    		treeViewEngineRequisicion = treeViewEngineRequisicion_ as Gtk.TreeStore;    		
    		
    		Gnome.PrintJob    trabajo   = new Gnome.PrintJob(Gnome.PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog(trabajo, "REPORTE REQUISICION DE COMPRAS", 0);
        	
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
                	new PrintJobPreview(trabajo, "REPORTE REQUISICION DE COMPRAS").Show();
                	break;
        	}
			dialogo.Hide (); dialogo.Dispose ();		
		}
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			encabezado_pagina(ContextoImp,trabajoImpresion);
			detalle_pagina(ContextoImp,trabajoImpresion);
		}
		
		void encabezado_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{	
			// Crear una fuente 
			Gnome.Font fuente = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
			Gnome.Font fuente1 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
			Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
			Gnome.Font fuente3 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
			Gnome.Font fuente5 = Gnome.Font.FindClosestFromWeightSlant("Bitstream Vera Sans", FontWeight.Bold ,false, 12);
			
				
			// ESTA FUNCION ES PARA QUE EL TEXTO SALGA EN NEGRITA

			ContextoImp.BeginPage("Pagina "+numpage.ToString());
			//Encabezado de pagina
			ContextoImp.Rotate(90);
			//////////////////////////////////////////////ESTA FUNCION SIRVE PARA CREAR RECTANGULOS Y CUADROS:////////////////////////////////////////////////////////////////////////////////////////////////////////////
			//cuadro FOLIO:
			Gnome.Print.RectStroked(ContextoImp,800,-55,-190,28);
			//Cuadro(solicitante,depto. y cuenta de cargo)                     //Cuadro(No. requisicion):                                                          //Cuadro(No. requisicion):
    	    Gnome.Print.RectStroked(ContextoImp,489,-100,-399,35);             Gnome.Print.RectStroked(ContextoImp,610,-100,-120,35);                              Gnome.Print.RectStroked(ContextoImp,800,-100,-190,35);
		    //Cuadro(fecha requerida, tipo requisicion y motivo)               //Cuadro(Observaciones):                                                            //Cuadro(Observaciones):
    	    Gnome.Print.RectStroked(ContextoImp,489,-140,-399,40);             Gnome.Print.RectStroked(ContextoImp,610,-140,-120,40);                              Gnome.Print.RectStroked(ContextoImp,800,-140,-190,40);
		    //Cuadro(No de partida, cant solicitada, uni de medida, descripcion, p. unitario, importe);
		    Gnome.Print.RectStroked(ContextoImp,800,-440,-710,300);
		    //Cuadro de divicion entre cada uno (No de partida, cant solicitada, uni de medida, descripcion, p. unitario, importe);
		    Gnome.Print.RectStroked(ContextoImp,540,-165,-450,25);
		    //No. DE PARTIDA:                                                 
		    Gnome.Print.RectStroked(ContextoImp,115,-440,-25,300);
		    //UNIDAD DE MEDIDA: 
			Gnome.Print.RectStroked(ContextoImp,190,-440,-45,300);
			//Embalaje:
		    Gnome.Print.RectStroked(ContextoImp,230,-440,-40,300);
		    //Presio unitario:                                                 
		    Gnome.Print.RectStroked(ContextoImp,590,-440,-50,300);
		    //importe:
		    Gnome.Print.RectStroked(ContextoImp,635,-440,-45,300);
		    //prov.B:
		    Gnome.Print.RectStroked(ContextoImp,740,-440,-55,287);
		    //Cuadro(Provedor A,B y C)                                         //Cuadro(sub total, 15%iva y total):                                                //Cuadro(dia y hora):
    	    Gnome.Print.RectStroked(ContextoImp,489,-490,-399,50);             Gnome.Print.RectStroked(ContextoImp,610,-490,-120,50);                             Gnome.Print.RectStroked(ContextoImp,800,-490,-190,50); 
		   
		    ////////////////////ESTA FUNCION SIRVE PARA CREAR LINEAS de firma////////////////////
		    //cotizaciones:
		    Gnome.Print.LineStroked(ContextoImp,635,-153,800,-153);
		    //presio unitario a prov.C
		    Gnome.Print.LineStroked(ContextoImp,520,-165,800,-165);
		    //solicitante
		    Gnome.Print.LineStroked(ContextoImp,90,-520,210,-520);
		    //jefe inmediato
		    Gnome.Print.LineStroked(ContextoImp,300,-520,410,-520);
		    //Autorizacion
		    Gnome.Print.LineStroked(ContextoImp,500,-520,610,-520); 
		    
		   // Cambiar la fuente
			Gnome.Print.Setfont(ContextoImp,fuente5);
			ContextoImp.MoveTo(320.7, -40);	            ContextoImp.Show( titulo+"");
			
			Gnome.Print.Setfont(ContextoImp,fuente3);
			ContextoImp.MoveTo(380, -55);			    ContextoImp.Show("PAGINA: " +numpage);
			ContextoImp.MoveTo(380.5, -55);			    ContextoImp.Show("PAGINA: ");
			
			Gnome.Print.Setfont(ContextoImp,fuente2);
			ContextoImp.MoveTo(95, -30);			    ContextoImp.Show("Sistemas Hospitalario OSIRIS");
			ContextoImp.MoveTo(95, -40);			    ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(95, -50);			    ContextoImp.Show("Conmutador:");
			
			Gnome.Print.Setfont(ContextoImp,fuente);
			ContextoImp.MoveTo(700, -90);			    ContextoImp.Show(requisicion);
			
			Gnome.Print.Setfont(ContextoImp,fuente2);
			ContextoImp.MoveTo(620, -40);			    ContextoImp.Show("FOLIO:");
			ContextoImp.MoveTo(620, -40);			    ContextoImp.Show("FOLIO:");
			ContextoImp.MoveTo(95, -80);			    ContextoImp.Show("SOLICITANTE:");
			ContextoImp.MoveTo(95.5, -80);			    ContextoImp.Show("SOLICITANTE:");
			//ContextoImp.MoveTo(95, -105);			    ContextoImp.Show();
			ContextoImp.MoveTo(260, -80);			    ContextoImp.Show("DEPARTAMENTO:");
			ContextoImp.MoveTo(260.5, -80);			    ContextoImp.Show("DEPARTAMENTO:");
			
			ContextoImp.MoveTo(380, -80);			    ContextoImp.Show("CUENTA DE CARGO:");
			ContextoImp.MoveTo(380.5, -80);			    ContextoImp.Show("CUENTA DE CARGO:");
			
			ContextoImp.MoveTo(500, -80);			    ContextoImp.Show("FECHA DE SOLICITUD:");
			ContextoImp.MoveTo(500.5, -80);			    ContextoImp.Show("FECHA DE SOLICITUD:");
			ContextoImp.MoveTo(500, -90);			    ContextoImp.Show(fecha_solicitud );
			ContextoImp.MoveTo(620, -80);			    ContextoImp.Show("No. de REQUSICION:");
			ContextoImp.MoveTo(620.5, -80);			    ContextoImp.Show("No. de REQUSICION:");
			
			ContextoImp.MoveTo(95, -110);			    ContextoImp.Show("FECHA REQUERIDA:");
			ContextoImp.MoveTo(95.5, -110);			    ContextoImp.Show("FECHA REQUERIDA:");
			ContextoImp.MoveTo(95, -130);			    ContextoImp.Show(dia_requerida+"/");
			ContextoImp.MoveTo(110, -130);			    ContextoImp.Show(mes_requerida+"/");
			ContextoImp.MoveTo(125, -130);			    ContextoImp.Show(ano_requerida);
			ContextoImp.MoveTo(200, -110);			    ContextoImp.Show("TIPO DE REQUISICION:");
			ContextoImp.MoveTo(200.5, -110);			ContextoImp.Show("TIPO DE REQUISICION:");
			
			ContextoImp.MoveTo(500, -110);			    ContextoImp.Show("FECHA COMPROMETIDA POR");
			ContextoImp.MoveTo(500.5, -110);			ContextoImp.Show("FECHA COMPROMETIDA POR");
			ContextoImp.MoveTo(500, -120);			    ContextoImp.Show("EL PROVEEDOR:");
			ContextoImp.MoveTo(500.5, -120);			ContextoImp.Show("EL PROVEEDOR:");
			ContextoImp.MoveTo(620, -110);			    ContextoImp.Show("OBSERVACIONES:");
			ContextoImp.MoveTo(620.5, -110);			ContextoImp.Show("OBSERVACIONES:");
			ContextoImp.MoveTo(200, -130);			    ContextoImp.Show("MOTIVO:");
			ContextoImp.MoveTo(200.5, -130);			ContextoImp.Show("MOTIVO:");
			ContextoImp.MoveTo(230, -130);			    ContextoImp.Show("    "+observaciones);
			
			ContextoImp.MoveTo(92, -150);			    ContextoImp.Show("No. ");
			ContextoImp.MoveTo(92, -160);			    ContextoImp.Show("PART.");
			ContextoImp.MoveTo(92.5, -150);			    ContextoImp.Show("No. ");
			ContextoImp.MoveTo(92.5, -160);			    ContextoImp.Show("PART.");
			ContextoImp.MoveTo(120, -150);			    ContextoImp.Show("CANT.");
			ContextoImp.MoveTo(120, -160);			    ContextoImp.Show("SOLI.");
			ContextoImp.MoveTo(120.5, -150);			ContextoImp.Show("CANT.");
			ContextoImp.MoveTo(120.5, -160);			ContextoImp.Show("SOLI.");
			ContextoImp.MoveTo(150, -150);			    ContextoImp.Show("UNI. DE");
			ContextoImp.MoveTo(150, -160);			    ContextoImp.Show(" MEDIDA");
			ContextoImp.MoveTo(150.5, -150);			ContextoImp.Show("UNI. DE");
			ContextoImp.MoveTo(150.5, -160);			ContextoImp.Show(" MEDIDA");
			ContextoImp.MoveTo(192, -150);			    ContextoImp.Show("EMBALAJE");
			ContextoImp.MoveTo(192.5, -150);			ContextoImp.Show("EMBALAJE");
			ContextoImp.MoveTo(260, -150);			    ContextoImp.Show("DESCRIPCION");
			ContextoImp.MoveTo(260.5, -150);			ContextoImp.Show("DESCRIPCION");
			
			ContextoImp.MoveTo(550, -150);			    ContextoImp.Show(" PRECIO");
			ContextoImp.MoveTo(550, -160);			    ContextoImp.Show("UNITARIO");
			ContextoImp.MoveTo(550.5, -150);			ContextoImp.Show(" PRECIO");
			ContextoImp.MoveTo(550.5, -160);			ContextoImp.Show("UNITARIO");
			ContextoImp.MoveTo(595, -150);			    ContextoImp.Show("IMPORTE");
			ContextoImp.MoveTo(595.5, -150);			ContextoImp.Show("IMPORTE");
			ContextoImp.MoveTo(675, -150);			    ContextoImp.Show("COTIZACIONES ");
			ContextoImp.MoveTo(675.5, -150);			ContextoImp.Show("COTIZACIONES ");
			ContextoImp.MoveTo(640, -160);			    ContextoImp.Show("PROV.A ");
			ContextoImp.MoveTo(640.5, -160);			ContextoImp.Show("PROV.A");
			ContextoImp.MoveTo(700, -160);			    ContextoImp.Show("PROV.B");
			ContextoImp.MoveTo(700.5, -160);			ContextoImp.Show("PROV.B ");
			ContextoImp.MoveTo(750, -160);			    ContextoImp.Show("PROV.C ");
			ContextoImp.MoveTo(750.5, -160);			ContextoImp.Show("PROV.C ");
			
			ContextoImp.MoveTo(95, -450);			    ContextoImp.Show("PROVEDOR A: ");
			ContextoImp.MoveTo(95.5, -450);			    ContextoImp.Show("PROVEDOR A: ");
			ContextoImp.MoveTo(95, -465);			    ContextoImp.Show("PROVEDOR B: ");
			ContextoImp.MoveTo(95.5, -465);			    ContextoImp.Show("PROVEDOR B: ");
			ContextoImp.MoveTo(95, -480);			    ContextoImp.Show("PROVEDOR C: ");
			ContextoImp.MoveTo(95.5, -480);			    ContextoImp.Show("PROVEDOR C: ");
			ContextoImp.MoveTo(500, -450);			    ContextoImp.Show("SUB-TOTAL: ");
			ContextoImp.MoveTo(500.5, -450);			ContextoImp.Show("SUB-TOTAL: ");
			ContextoImp.MoveTo(500, -465);			    ContextoImp.Show("15% I.V.A: ");
			ContextoImp.MoveTo(500.5, -465);			ContextoImp.Show("15% I.V.A: ");
			ContextoImp.MoveTo(500, -480);			    ContextoImp.Show("TOTAL: ");
			ContextoImp.MoveTo(500.5, -480);			ContextoImp.Show("TOTAL: ");
			ContextoImp.MoveTo(620, -450);			    ContextoImp.Show("DIA Y HORA DE RECEPCION DEPTO.COMPRAS");
			ContextoImp.MoveTo(620.5, -450);			ContextoImp.Show("DIA Y HORA DE RECEPCION DEPTO.COMPRAS");
			
			ContextoImp.MoveTo(115, -530);			    ContextoImp.Show("SOLICITANTE");
			ContextoImp.MoveTo(115.5, -530);			ContextoImp.Show("SOLICITANTE");
			ContextoImp.MoveTo(325, -530);			    ContextoImp.Show("JEFE INMEDIATO");
			ContextoImp.MoveTo(325.5, -530);			ContextoImp.Show("JEFE INMEDIATO");
			ContextoImp.MoveTo(520, -530);			    ContextoImp.Show("AUTORIZACION");
			ContextoImp.MoveTo(520.5, -530);			ContextoImp.Show("AUTORIZACION");
			filas = -174;
		}
		
		void detalle_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
			TreeModel model;
			TreeIter iter;
			if (this.treeViewEngineRequisicion.GetIterFirst (out iter)){			
				string nombre = "";
				nombre = (string) this.lista_requisicion_productos.Model.GetValue (iter,1);
				if (nombre.Length > 50){
					nombrepacienterecortado = nombre.Substring(0,50); 
				}else{
					nombrepacienterecortado = nombre;
				}		
				//Console.WriteLine((string) this.lista_requisicion_productos.Model.GetValue (iter,0));
				ContextoImp.MoveTo(115, filas);			ContextoImp.Show((string) this.lista_requisicion_productos.Model.GetValue (iter,0));//cantidad solicitada
				ContextoImp.MoveTo(135, filas);			ContextoImp.Show((string) this.lista_requisicion_productos.Model.GetValue (iter,4));//unidad de medida
				ContextoImp.MoveTo(195, filas);			ContextoImp.Show((string) this.lista_requisicion_productos.Model.GetValue (iter,3));//embalaje
				ContextoImp.MoveTo(238, filas);			ContextoImp.Show(nombrepacienterecortado.ToString());//descripcion
				ContextoImp.MoveTo(100, filas);			ContextoImp.Show(contador.ToString());//no partida
				contador += 1;
				filas -= 10;
				//salto_pagina(ContextoImp,trabajoImpresion);
			}
			
			while(this.treeViewEngineRequisicion.IterNext (ref iter)){
				string nombre = "";
				nombre = (string) this.lista_requisicion_productos.Model.GetValue (iter,1);
				if (nombre.Length > 50){
					nombrepacienterecortado = nombre.Substring(0,50); 
				}else{
					nombrepacienterecortado = nombre;
				}
				//Console.WriteLine((string) this.lista_requisicion_productos.Model.GetValue (iter,0));
				ContextoImp.MoveTo(115, filas);			ContextoImp.Show((string) this.lista_requisicion_productos.Model.GetValue (iter,0));//cantidad solicitada
				ContextoImp.MoveTo(135, filas);			ContextoImp.Show((string) this.lista_requisicion_productos.Model.GetValue (iter,4));//unidad de medida
				ContextoImp.MoveTo(195, filas);			ContextoImp.Show((string) this.lista_requisicion_productos.Model.GetValue (iter,3));//embalaje
				ContextoImp.MoveTo(238, filas);			ContextoImp.Show(nombrepacienterecortado.ToString());//descripcion
				ContextoImp.MoveTo(100, filas);			ContextoImp.Show(contador.ToString());//no partida	
				contador += 1;
				filas -= 10;
				if (contador > 27){
					salto_pagina(ContextoImp,trabajoImpresion);
				}
				
			}
			
			NpgsqlConnection conexion1;
			conexion1 = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion1.Open ();
				NpgsqlCommand comando1;
				comando1 = conexion1.CreateCommand ();
				comando1.CommandText ="SELECT * "+
				                      "FROM osiris_erp_requisicion_enca,osiris_empleado "+
				                      "WHERE osiris_erp_requisicion_enca.id_quien_requiso = osiris_empleado.login_empleado "+ 
						              "AND id_requisicion = '"+requisicion +"';";
				//Console.WriteLine(comando1.CommandText.ToString());
				NpgsqlDataReader lector1 = comando1.ExecuteReader ();
				if(lector1.Read()){
					descripinternamiento2 = (string) lector1["descripcion_admisiones_cargada"];
					descripcion_tipo_requi = (string) lector1["descripcion_tipo_requisicion"];
					solicitante = ((string) lector1["nombre1_empleado"]+" "+ (string) lector1["nombre2_empleado"]+" "+ (string) lector1["apellido_paterno_empleado"]+" "+ (string) lector1["apellido_materno_empleado"]);
					
					//Console.WriteLine();
					Gnome.Print.Setfont(ContextoImp,fuente2);
					//ContextoImp.MoveTo(95, -90);			ContextoImp.Show(descripinternamiento);
					ContextoImp.MoveTo(380, -90);			ContextoImp.Show(descripinternamiento2);
					ContextoImp.MoveTo(330, -110);			ContextoImp.Show(descripcion_tipo_requi);
					ContextoImp.MoveTo(95, -90);			ContextoImp.Show(solicitante);
					filas = -174;
				}		
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Error, 
				                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion1.Close ();
			ContextoImp.ShowPage();
  	 	}
  	 	
  	 	void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
  	 	{
  	 		ContextoImp.ShowPage();
  	 		contador = 1;
			numpage += 1;
			filas =-174;
			encabezado_pagina(ContextoImp,trabajoImpresion);
  	 	}
   	}
}