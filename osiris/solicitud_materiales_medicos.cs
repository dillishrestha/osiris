////////////////////////////////////////////////////////////
// created on 08/05/2007 at 09:26 a
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
//				  Daniel Olivares - arcangeldoc@gmail.com (Diseño de Pantallas Glade)
// 				  
// Licencia		: GLP
// S.O. 		: GNU/Linux RH4 ES
//////////////////////////////////////////////////////////
//
// proyect osiris is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// proyect osirir is distributed in the hope that it will be useful,
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
// Proposito	: Solicitud de materiales para los diferentes centros de costos medicos 
// Objeto		: hospitalizacion_solicitud_mat.cs 
//////////////////////////////////////////////////////////
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class solicitud_material
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		
		// Declarando ventana principal de solicitud de Materiales
		[Widget] Gtk.Window solicitud_materiales;
		[Widget] Gtk.Entry entry_numero_solicitud;
		[Widget] Gtk.Entry entry_quien_solicita;
		[Widget] Gtk.Entry entry_fecha_solicitud;
		[Widget] Gtk.Entry entry_status_solicitud;
		[Widget] Gtk.Button button_selecciona_solicitud;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.Button button_guardar_solicitud;
		[Widget] Gtk.Button button_envio_solicitud;
		[Widget] Gtk.Button button_quitar_productos;
		[Widget] Gtk.Button button_imprime_solicitud;
		[Widget] Gtk.Button button_buscar_solicitudes;
		[Widget] Gtk.CheckButton checkbutton_nueva_solicitud;
		[Widget] Gtk.TreeView lista_produc_solicitados;
		
		[Widget] Gtk.Entry entry_rojo;
		[Widget] Gtk.Entry entry_azul;
		[Widget] Gtk.Entry entry_verde;
		
		//Declarando la barra de estado
		[Widget] Gtk.Statusbar statusbar_hospital;
		
		/////// Ventana Busqueda de productos\\\\\\\\
		[Widget] Gtk.TreeView lista_de_producto;
		//[Widget] Gtk.Button button_agrega_extra;
		[Widget] Gtk.Entry entry_cantidad_aplicada;
		[Widget] Gtk.Label label_titulo_cantidad;
		
		//private TreeStore treeViewEngineBusca;
		private TreeStore treeViewEngineBusca2;
		private ListStore treeViewEngineSolicitud;
		
		//imprimir
		//public Gnome.Font fuente5 = Gnome.Font.FindClosest("Luxi Sans", 5);
		//public Gnome.Font fuente6 = Gnome.Font.FindClosest("Luxi Sans", 6);
		//public Gnome.Font fuente7 = Gnome.Font.FindClosest("Luxi Sans", 7);
		//public Gnome.Font fuente8 = Gnome.Font.FindClosest("Luxi Sans", 8);//Bitstream Vera Sans
		//public Gnome.Font fuente9 = Gnome.Font.FindClosest("Luxi Sans", 9);
		//public Gnome.Font fuente10 = Gnome.Font.FindClosest("Luxi Sans", 10);
		//public Gnome.Font fuente11 = Gnome.Font.FindClosest("Luxi Sans", 11);
		//public Gnome.Font fuente12 = Gnome.Font.FindClosest("Luxi Sans", 12);
		//public Gnome.Font fuente36 = Gnome.Font.FindClosest("Luxi Sans", 36);
		
		//private ArrayList arraysolicitudmat;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		string connectionString = "Server=localhost;" +
						"Port=5432;" +
						 "User ID=admin;" +
						"Password=1qaz2wsx;";
		
		float valoriva = 15;
		int idalmacen;    // Esta variable almacena el codigo del almacen de esta clase, se recibe como parametro de la clase
		
		int ultimasolicitud;		// Toma el ultimo numero de solictud
		
		bool editar = true;				// me indica si puedo agregar mas productos a la solicitud
		
		int filas=690;
		int contador = 1;
		int numpage = 1;
		
		//Declaracion de ventana de error y pregunta
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public solicitud_material(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_,int idalmacen_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = _nombrebd_;
			idalmacen = idalmacen_;
			
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "solicitud_materiales", null);
			gxml.Autoconnect (this);        
			////// Muestra ventana de Glade
			solicitud_materiales.Show();
			
			// acciones de botones
			// Validando que sen solo numeros
			entry_numero_solicitud.KeyPressEvent += onKeyPressEvent_enter_solicitud;
			// Busqueda de Productos
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			// Quitar productos editar tiene que estar en true
			button_quitar_productos.Clicked += new EventHandler(on_button_quitar_productos_clicked);
			// Button nueva solicitud
			checkbutton_nueva_solicitud.Clicked += new EventHandler(on_checkbutton_nueva_solicitud_clicked);
			//Button Guardar Solicitud
			button_guardar_solicitud.Clicked += new EventHandler(on_button_guardar_solicitud_clicked);
			//Button Seleccion una Solicitud
			button_selecciona_solicitud.Clicked += new EventHandler(on_button_selecciona_solicitud_clicked);
			//Button envio de solicitud para alamcen
			this.button_envio_solicitud.Clicked += new EventHandler(on_button_envio_solicitud_clicked);
			//button_buscar_solicitudes.Clicked += new EventHandler(on_button_buscar_solicitudes_clicked);
			
			button_imprime_solicitud.Clicked += new EventHandler(on_button_imprime_solicitud_clicked);
			
			button_guardar_solicitud.Sensitive = false;
			button_envio_solicitud.Sensitive = false;
			button_quitar_productos.Sensitive = false;
			button_busca_producto.Sensitive = false;
			button_imprime_solicitud.Sensitive = true;				
			
			entry_quien_solicita.Text = NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado;
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			// Colores de los cuadros		
			entry_rojo.ModifyBase(StateType.Normal, new Gdk.Color(255,0,0));
			entry_azul.ModifyBase(StateType.Normal, new Gdk.Color(0,0,255));
			entry_verde.ModifyBase(StateType.Normal, new Gdk.Color(0,255,0));
			
			statusbar_hospital.Pop(0);
			statusbar_hospital.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_hospital.HasResizeGrip = false;
			
			crea_treeview_solicitud();
		}

		void on_button_imprime_solicitud_clicked(object sender, EventArgs args)
		{
		}
		
		/*
			Gnome.PrintJob    trabajo = new Gnome.PrintJob(Gnome.PrintConfig.Default());
        	Gnome.PrintDialog dialogo = new Gnome.PrintDialog(trabajo, "Envio Materiales Almacen", 0);
        	
        	int         respuesta = dialogo.Run ();
        	
        	if (respuesta == (int) PrintButtons.Cancel){
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
                new PrintJobPreview(trabajo, "Envio Materiales Almacen").Show();
                break;
        	}
			dialogo.Hide (); dialogo.Dispose ();
		}			
			
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{   
			//int filas=690;
			string numerodesolicutud = "";
			numerodesolicutud = entry_numero_solicitud.Text;
				
			NpgsqlConnection conexion; 
        	conexion = new NpgsqlConnection (connectionString+nombrebd);
        	// Verifica que la base de dato s este conectada
        	try{
	 			conexion.Open ();
	        	NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand (); 

	        	comando.CommandText = "SELECT to_char(hscmty_his_solicitudes_deta.folio_de_solicitud,'999999999') AS foliosol,"+
	        		       			"to_char(hscmty_his_solicitudes_deta.id_producto,'999999999999') AS idproductos,"+
               						"to_char(cantidad_solicitada,'999999.999') AS cantidadsolicitada,"+
               						"to_char(hscmty_his_solicitudes_deta.precio_producto_publico,'99999999.99') AS precioproductopublico,"+
               						"to_char(hscmty_his_solicitudes_deta.costo_por_unidad,'99999999.99') AS costoporunidad,"+
               						"to_char(cantidad_autorizada,'999999.999') AS cantidadautorizada,id_quien_autorizo, "+
               						"to_char(fechahora_solicitud,'dd-MM-yyyy') AS fechahorasolicitud,"+
               						"to_char(fechahora_autorizado,'dd-MM-yyyy') AS fechahoraautorizado,"+
               						"hscmty_his_solicitudes_deta.id_quien_solicito,"+
               						"status,surtido,hscmty_productos.descripcion_producto,"+
               						"to_char(id_secuencia,'9999999999') AS idsecuencia,"+
               						"hscmty_almacenes.descripcion_almacen,"+
               						"hscmty_his_solicitudes_deta.sin_stock,"+	
									"hscmty_his_solicitudes_deta.solicitado_erroneo,"+
									"hscmty_his_solicitudes_deta.surtido,"+
									"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,"+
               						"hscmty_empleado.nombre1_empleado || ' ' || "+"hscmty_empleado.nombre2_empleado || ' ' || "+"hscmty_empleado.apellido_paterno_empleado || ' ' || "+ 
									"hscmty_empleado.apellido_materno_empleado AS nombreempl "+
               						"FROM hscmty_his_solicitudes_deta,hscmty_almacenes,hscmty_productos,hscmty_grupo_producto,hscmty_grupo1_producto,hscmty_grupo2_producto,hscmty_empleado "+
               						"WHERE hscmty_his_solicitudes_deta.folio_de_solicitud = '"+numerodesolicutud+"' "+
               						"AND eliminado = 'false' "+
               						"AND hscmty_his_solicitudes_deta.id_producto = hscmty_productos.id_producto "+
               						"AND hscmty_empleado.login_empleado = hscmty_his_solicitudes_deta.id_quien_solicito "+
               						"AND hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
									"AND hscmty_productos.id_grupo1_producto = hscmty_grupo1_producto.id_grupo1_producto "+
									"AND hscmty_productos.id_grupo2_producto = hscmty_grupo2_producto.id_grupo2_producto "+
									"AND hscmty_his_solicitudes_deta.id_almacen = hscmty_almacenes.id_almacen "+
									"AND hscmty_almacenes.id_almacen = '"+this.idalmacen.ToString().Trim()+"' "+
									"ORDER BY hscmty_his_solicitudes_deta.id_secuencia;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				ContextoImp.BeginPage("Pagina 1");
				contador=1;
	        	filas=690;
				imprime_encabezado(ContextoImp,trabajoImpresion);
									string toma_descrip_prod;
				if (lector.Read()){		        		
	        		Gnome.Print.Setfont (ContextoImp, fuente8);
	        		ContextoImp.MoveTo(400, 740);		 ContextoImp.Show((string) lector["descripcion_almacen"]);
		        	ContextoImp.MoveTo(300, 740);		 ContextoImp.Show((string) lector["foliosol"]);
		        	ContextoImp.MoveTo(250, 740);		 ContextoImp.Show("No. SOLICITUD:");
					ContextoImp.MoveTo(220, 720);		 ContextoImp.Show((string) lector["nombreempl"]);
	        		Gnome.Print.Setfont (ContextoImp, fuente6);
					ContextoImp.MoveTo(20, 720);		 ContextoImp.Show("Solicito:");
					ContextoImp.MoveTo(70, 720);		 ContextoImp.Show((string) lector["id_quien_solicito"]);
					ContextoImp.MoveTo(165, 720);		 ContextoImp.Show("Nom.Solicitante:");
					ContextoImp.MoveTo(390, -50);		 ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
					
					contador+=1;
   					salto_pagina(ContextoImp,trabajoImpresion,contador);
		
			        if((bool) lector["surtido"] == true){
			        	toma_descrip_prod = (string) lector["descripcion_producto"];
			        		
						if(toma_descrip_prod.Length > 90){
							toma_descrip_prod = toma_descrip_prod.Substring(0,89);
						}  			
						ContextoImp.MoveTo(80, filas);		ContextoImp.Show(toma_descrip_prod);
			        	ContextoImp.MoveTo(20, filas);			 ContextoImp.Show((string) lector["idproductos"]);
						ContextoImp.MoveTo(350, filas);			 ContextoImp.Show((string) lector["cantidadsolicitada"]);
						ContextoImp.MoveTo(400, filas);			 ContextoImp.Show((string) lector["fechahorasolicitud"]);
			       		ContextoImp.MoveTo(450, filas);			 ContextoImp.Show((string) lector["cantidadautorizada"]);
			       		ContextoImp.MoveTo(500,filas);	  		ContextoImp.Show((string) lector["fechahoraautorizado"]);
			   
					}else{
						Gnome.Print.Setrgbcolor(ContextoImp, 000,0,1);  //cambio color de impresion 
						toma_descrip_prod = (string) lector["descripcion_producto"];
						if(toma_descrip_prod.Length > 90){
							toma_descrip_prod = toma_descrip_prod.Substring(0,89);
						}  			
						ContextoImp.MoveTo(80, filas);		ContextoImp.Show(toma_descrip_prod);
						ContextoImp.MoveTo(20, filas);			 ContextoImp.Show((string) lector["idproductos"]);
						ContextoImp.MoveTo(350, filas);			 ContextoImp.Show((string) lector["cantidadsolicitada"]);
						ContextoImp.MoveTo(400, filas);			 ContextoImp.Show((string) lector["fechahorasolicitud"]);
		        		ContextoImp.MoveTo(450, filas);			 ContextoImp.Show((string) lector["cantidadautorizada"]);			 				
						
						if((bool) lector["sin_stock"] == true){
							ContextoImp.MoveTo(500,filas);	 ContextoImp.Show("sin stock");
						}
						if((bool) lector["solicitado_erroneo"] == true){
							ContextoImp.MoveTo(500,filas);	 ContextoImp.Show("Pedido Erroneo");
						}
						if(float.Parse((string) lector["cantidadautorizada"]) == 0 && (bool) lector["sin_stock"] == false && (bool) lector["solicitado_erroneo"] == false && (bool) lector["surtido"] == false){
							ContextoImp.MoveTo(500,filas);	 ContextoImp.Show("No surtido");
						}
						Gnome.Print.Setrgbcolor(ContextoImp, 0,0,0);   //cierra el cambio de color de impresion 
					}
			        	
			        filas-=10;
					contador+=1;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
				}
					
				while (lector.Read()){
			       	if((bool) lector["surtido"] == true){
			       		toma_descrip_prod = (string) lector["descripcion_producto"];
						if(toma_descrip_prod.Length > 90){
							toma_descrip_prod = toma_descrip_prod.Substring(0,89);
						}  			
						ContextoImp.MoveTo(80, filas);		ContextoImp.Show(toma_descrip_prod);
			        	ContextoImp.MoveTo(20, filas);			 ContextoImp.Show((string) lector["idproductos"]);
						ContextoImp.MoveTo(350, filas);			 ContextoImp.Show((string) lector["cantidadsolicitada"]);
						ContextoImp.MoveTo(400, filas);			 ContextoImp.Show((string) lector["fechahorasolicitud"]);
			        	ContextoImp.MoveTo(450, filas);			 ContextoImp.Show((string) lector["cantidadautorizada"]);
			       		ContextoImp.MoveTo(500,filas);	  		ContextoImp.Show((string) lector["fechahoraautorizado"]);

					}else{
						Gnome.Print.Setrgbcolor(ContextoImp, 000,0,1);  //cambio color
						toma_descrip_prod = (string) lector["descripcion_producto"];
						if(toma_descrip_prod.Length > 90){
							toma_descrip_prod = toma_descrip_prod.Substring(0,89);
						}  	
							
						ContextoImp.MoveTo(80, filas);		ContextoImp.Show(toma_descrip_prod);
						ContextoImp.MoveTo(20, filas);			 ContextoImp.Show((string) lector["idproductos"]);
						ContextoImp.MoveTo(350, filas);			 ContextoImp.Show((string) lector["cantidadsolicitada"]);
						ContextoImp.MoveTo(400, filas);			 ContextoImp.Show((string) lector["fechahorasolicitud"]);
		        		ContextoImp.MoveTo(450, filas);			 ContextoImp.Show((string) lector["cantidadautorizada"]);			 				
							
						if((bool) lector["sin_stock"] == true){
							ContextoImp.MoveTo(500,filas);	 ContextoImp.Show("sin stock");
						}
						if((bool) lector["solicitado_erroneo"] == true){
							ContextoImp.MoveTo(500,filas);	 ContextoImp.Show("Pedido Erroneo");
						}
						if(float.Parse((string) lector["cantidadautorizada"]) == 0 && (bool) lector["sin_stock"] == false && (bool) lector["solicitado_erroneo"] == false && (bool) lector["surtido"] == false){
							ContextoImp.MoveTo(500,filas);	 ContextoImp.Show("No surtido");
						}
						Gnome.Print.Setrgbcolor(ContextoImp, 0,0,0);   //cierra el cambio de color 
					}
			        	
			        filas-=10;
					contador+=1;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
				}
								
				ContextoImp.ShowPage();       		
	       	}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
			}
		}	
			
		void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion,int contador_)
		{
			if (contador_ > 60){
	        	filas=690;
	        	numpage +=1;
	        	ContextoImp.ShowPage();
	        	contador=1;
				ContextoImp.BeginPage("Pagina "+numpage.ToString());
				imprime_encabezado(ContextoImp,trabajoImpresion);
	     		Gnome.Print.Setfont (ContextoImp, fuente6);
	        }
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
			
			ContextoImp.MoveTo(20, 700);			ContextoImp.Show("Folio Producto");
			ContextoImp.MoveTo(80, 700);			ContextoImp.Show("Descripcion");
			ContextoImp.MoveTo(350, 700);			ContextoImp.Show("Cant. Surtida");
			ContextoImp.MoveTo(400, 700);			ContextoImp.Show("Fecha Surtida");
			ContextoImp.MoveTo(450, 700);			ContextoImp.Show("Cant Autorizada");
			ContextoImp.MoveTo(500, 700);			ContextoImp.Show("Fecha Autorizada");
			
			Gnome.Print.Setfont (ContextoImp, fuente8);
		}
		*/

		void on_button_busca_producto_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			
			crea_treeview_busqueda("producto");
			
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
			label_titulo_cantidad.Text = "Cantidad Solicitada";	
			
			// Validando que sen solo numeros
			entry_cantidad_aplicada.KeyPressEvent += onKeyPressEvent;
	    }
	    
	    void on_checkbutton_nueva_solicitud_clicked(object sender, EventArgs args)
	    {
	    	if (checkbutton_nueva_solicitud.Active == true){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de CREAR una Nueva SOLICITUD ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
	    			ultimasolicitud = ultima_solicitud();
	    			this.entry_fecha_solicitud.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
	    			this.entry_numero_solicitud.Text = ultimasolicitud.ToString().Trim();
	    			this.treeViewEngineSolicitud.Clear(); // Limpia el treeview
	    			button_guardar_solicitud.Sensitive = true;
					button_quitar_productos.Sensitive = true;
					button_busca_producto.Sensitive = true;
					this.button_selecciona_solicitud.Sensitive = false;
					this.button_buscar_solicitudes.Sensitive = false;
					this.entry_numero_solicitud.Editable = false;	
					entry_status_solicitud.Text = "";
	     		}else{
	     			this.checkbutton_nueva_solicitud.Active = false;
	     		}
	    	}else{
	    		this.button_guardar_solicitud.Sensitive = false;
				this.button_envio_solicitud.Sensitive = false;
				this.button_quitar_productos.Sensitive = false;
				this.button_busca_producto.Sensitive = false;
				this.button_selecciona_solicitud.Sensitive = true;
				this.button_buscar_solicitudes.Sensitive = true;
				this.entry_numero_solicitud.Editable = true;
		 	}
	    }
	    
	    void on_button_selecciona_solicitud_clicked(object sender, EventArgs args)
	    {
	    	llena_solicitud_material(entry_numero_solicitud.Text);
	    }
	    
	    void on_button_guardar_solicitud_clicked(object sender, EventArgs args)
	    {
	    	if (checkbutton_nueva_solicitud.Active == true){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de GUARDAR esta SOLICITUD ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
		 			almacena_productos_solicitados();
		 			editar = true;
		 			entry_status_solicitud.Text = "NO ESTA ENVIADA";
		 		}
		 	 }
		 	 
		 	 if (editar == false){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de GUARDAR esta SOLICITUD ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
		 			almacena_productos_solicitados();
		 		}
		 	 }
		 }
		 
		 void on_button_quitar_productos_clicked(object sender, EventArgs args)
		 {
		 	TreeModel model;
			TreeIter iterSelected;
 			if (this.lista_produc_solicitados.Selection.GetSelected(out model, out iterSelected)){
 				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta quitar el producto "+(string) this.lista_produc_solicitados.Model.GetValue (iterSelected,0));
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
		 			NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
			    	// Verifica que la base de datos este conectada
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						comando.CommandText = "UPDATE hscmty_his_solicitudes_deta SET id_quien_elimino ='"+LoginEmpleado+"',"+ 
											"fechahora_elimando = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
											"eliminado = 'true' "+
											"WHERE id_secuencia =  '"+(string) this.lista_produc_solicitados.Model.GetValue (iterSelected,10)+"';";
						
						comando.ExecuteNonQuery();
						comando.Dispose();
						this.treeViewEngineSolicitud.Remove (ref iterSelected);
			        	msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"El Producto se quito satisfactoreamente...");										
						msgBox.Run ();
						msgBox.Destroy();		
						conexion.Close ();
			        }catch (NpgsqlException ex){
				   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
					}
		 		}
 			}
		 }
		 
		 void almacena_productos_solicitados()
		 {
			TreeIter iter;
			if (this.treeViewEngineSolicitud.GetIterFirst (out iter)){
				button_envio_solicitud.Sensitive = true;				
				if ((bool) this.lista_produc_solicitados.Model.GetValue (iter,8) == false){
					// Verifica que la base de datos este conectada
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						comando.CommandText = "INSERT INTO hscmty_his_solicitudes_deta("+
																	"folio_de_solicitud,"+
																	"id_producto,"+
																	"precio_producto_publico,"+
																	"costo_por_unidad,"+
																	"cantidad_solicitada,"+
																	"fechahora_solicitud,"+
																	"id_quien_solicito,"+
																	"id_almacen) "+
																	"VALUES ('"+
																	this.entry_numero_solicitud.Text+"','"+
																	(string) this.lista_produc_solicitados.Model.GetValue(iter,1)+"','"+
																	(string) this.lista_produc_solicitados.Model.GetValue(iter,7)+"','"+
																	(string) this.lista_produc_solicitados.Model.GetValue(iter,6)+"','"+
																	(string) this.lista_produc_solicitados.Model.GetValue(iter,2)+"','"+
																	DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																	LoginEmpleado+"','"+
																	this.idalmacen.ToString()+"');";
																
						//Console.WriteLine(comando.CommandText);
						comando.ExecuteNonQuery();
						comando.Dispose();
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();				msgBoxError.Destroy();
					}
				}
				while (this.treeViewEngineSolicitud.IterNext(ref iter)){
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						if ((bool) this.lista_produc_solicitados.Model.GetValue (iter,8) == false){
							comando.CommandText = "INSERT INTO hscmty_his_solicitudes_deta("+
																	"folio_de_solicitud,"+
																	"id_producto,"+
																	"precio_producto_publico,"+
																	"costo_por_unidad,"+
																	"cantidad_solicitada,"+
																	"fechahora_solicitud,"+
																	"id_quien_solicito,"+
																	"id_almacen) "+
																	"VALUES ('"+
																	this.entry_numero_solicitud.Text+"','"+
																	(string) this.lista_produc_solicitados.Model.GetValue(iter,1)+"','"+
																	(string) this.lista_produc_solicitados.Model.GetValue(iter,7)+"','"+
																	(string) this.lista_produc_solicitados.Model.GetValue(iter,6)+"','"+
																	(string) this.lista_produc_solicitados.Model.GetValue(iter,2)+"','"+
																	DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																	LoginEmpleado+"','"+
																	this.idalmacen.ToString()+"');";
																
						//Console.WriteLine(comando.CommandText);
							comando.ExecuteNonQuery();
							comando.Dispose();
						}
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();				msgBoxError.Destroy();
					}
				}
				this.checkbutton_nueva_solicitud.Active = false;
				llena_solicitud_material(entry_numero_solicitud.Text);				
			}			
	    }
	    
	    void on_button_envio_solicitud_clicked(object sender, EventArgs args)
	    {
	    	if (checkbutton_nueva_solicitud.Active == false){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de ENVIAR esta SOLICITUD ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
	    			NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						comando.CommandText = "UPDATE hscmty_his_solicitudes_deta SET status = true,"+
											"fecha_envio_almacen = '"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
											"id_empleado = '"+LoginEmpleado+"' "+
											"WHERE hscmty_his_solicitudes_deta.folio_de_solicitud = '"+(string) entry_numero_solicitud.Text+"' "+ 
											"AND id_almacen = '"+this.idalmacen.ToString().Trim()+"';";
						//Console.WriteLine(comando.CommandText);
						comando.ExecuteNonQuery();
						comando.Dispose();
						
						msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
								MessageType.Info,ButtonsType.Ok,"La SOLICITUD de ENVIO satisfactoriamente...");
						miResultado = (ResponseType)msgBox.Run (); 	msgBox.Destroy();
						this.button_envio_solicitud.Sensitive = false;
						this.button_busca_producto.Sensitive = false;
						this.button_guardar_solicitud.Sensitive = false;
						this.button_quitar_productos.Sensitive = false;
						entry_status_solicitud.Text = "ENVIADA ALMACEN";
					}catch (NpgsqlException ex){
		   				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
							msgBoxError.Run ();					msgBoxError.Destroy();
					}					
				}					
			}
	    } 
	    
	    void llena_solicitud_material(string numerodesolicutud)
	    {
    
	    	this.treeViewEngineSolicitud.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			editar = true;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT hscmty_his_solicitudes_deta.folio_de_solicitud,to_char(hscmty_his_solicitudes_deta.id_producto,'999999999999') AS idproductos,"+
               						"to_char(cantidad_solicitada,'999999.999') AS cantidadsolicitada,"+
               						"to_char(hscmty_his_solicitudes_deta.precio_producto_publico,'999999999.99') AS precioproductopublico,"+
               						"to_char(hscmty_his_solicitudes_deta.costo_por_unidad,'999999999.99') AS costoporunidad,"+
               						"to_char(cantidad_autorizada,'999999.999') AS cantidadautorizada,id_quien_autorizo, "+
               						"to_char(fechahora_solicitud,'dd-MM-yyyy') AS fechahorasolicitud,"+
               						"to_char(fechahora_autorizado,'dd-MM-yyyy') AS fechahoraautorizado,"+
               						"status,surtido,hscmty_productos.descripcion_producto,"+
               						"to_char(id_secuencia,'9999999999') AS idsecuencia,"+
									"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto "+
               						"FROM hscmty_his_solicitudes_deta,hscmty_productos,hscmty_grupo_producto,hscmty_grupo1_producto,hscmty_grupo2_producto "+
               						"WHERE hscmty_his_solicitudes_deta.folio_de_solicitud = '"+(string) numerodesolicutud+"' "+
               						"AND eliminado = 'false' "+
               						"AND hscmty_his_solicitudes_deta.id_producto = hscmty_productos.id_producto "+
               						"AND hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
									"AND hscmty_productos.id_grupo1_producto = hscmty_grupo1_producto.id_grupo1_producto "+
									"AND hscmty_productos.id_grupo2_producto = hscmty_grupo2_producto.id_grupo2_producto "+
									"AND id_almacen = '"+this.idalmacen.ToString().Trim()+"' "+
									"ORDER BY hscmty_his_solicitudes_deta.id_secuencia;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){
					if((bool) lector["status"] == false){
						editar = false;
					}
					treeViewEngineSolicitud.AppendValues((string) lector["descripcion_producto"],
													(string) lector["idproductos"],
													(string) lector["cantidadsolicitada"],
													(string) lector["fechahorasolicitud"],
													(string) lector["cantidadautorizada"],
													(string) lector["fechahoraautorizado"],
													(string) lector["costoporunidad"],
													(string) lector["precioproductopublico"],
													true,
													(bool) lector["surtido"],
													(string) lector["idsecuencia"]);
				}
				if(editar == false){
					this.button_envio_solicitud.Sensitive = true;
					this.button_busca_producto.Sensitive = true;
					this.button_guardar_solicitud.Sensitive = true;
					this.button_quitar_productos.Sensitive = true;
					entry_status_solicitud.Text = "NO ESTA ENVIADA";
				}else{
					this.button_envio_solicitud.Sensitive = false;
					this.button_busca_producto.Sensitive = false;
					this.button_guardar_solicitud.Sensitive = false;
					this.button_quitar_productos.Sensitive = false;
					entry_status_solicitud.Text = "ENVIADA ALMACEN";
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
								MessageType.Info,ButtonsType.Ok,"Esta SOLICITUD ya esta registrada en ALMACEN o NO EXISTE, Verifique por favor");
					msgBox.Run (); 	msgBox.Destroy();
				}
								
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
							msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion.Close ();
	    }
	    		
		void crea_treeview_busqueda(string tipo_busqueda)
		{
			if (tipo_busqueda == "solicitud")
			{
				
			}
			if (tipo_busqueda == "producto")
			{
				treeViewEngineBusca2 = new TreeStore(typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));

				lista_de_producto.Model = treeViewEngineBusca2;
			
				lista_de_producto.RulesHint = true;
			
				lista_de_producto.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono paciente*/
				
				TreeViewColumn col_idproducto = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_idproducto.Title = "ID Producto"; // titulo de la cabecera de la columna, si está visible
				col_idproducto.PackStart(cellr0, true);
				col_idproducto.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
				col_idproducto.SortColumnId = (int) Column_prod.col_idproducto;
			
				TreeViewColumn col_desc_producto = new TreeViewColumn();
				CellRendererText cellr1 = new CellRendererText();
				col_desc_producto.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
				col_desc_producto.PackStart(cellr1, true);
				col_desc_producto.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
				col_desc_producto.SortColumnId = (int) Column_prod.col_desc_producto;
				//cellr0.Editable = true;   // Permite edita este campo
            	
				TreeViewColumn col_grupoprod = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_grupoprod.Title = "Grupo Producto";//Precio Producto
				col_grupoprod.PackStart(cellrt2, true);
				col_grupoprod.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
				col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
            
				TreeViewColumn col_grupo1prod = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_grupo1prod.Title = "Grupo1 Producto";//I.V.A.
				col_grupo1prod.PackStart(cellrt3, true);
				col_grupo1prod.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 2 en vez de 3
				col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
            
				TreeViewColumn col_grupo2prod = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_grupo2prod.Title = "Grupo2 Producto";//Total
				col_grupo2prod.PackStart(cellrt4, true);
				col_grupo2prod.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 3 en vez de 4
				col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;
            	
				lista_de_producto.AppendColumn(col_idproducto);  // 0
				lista_de_producto.AppendColumn(col_desc_producto); // 1
				lista_de_producto.AppendColumn(col_grupoprod);	//7
				lista_de_producto.AppendColumn(col_grupo1prod);	//8
				lista_de_producto.AppendColumn(col_grupo2prod);	//9							
			}
		}
		
		// llena la lista de productos
 		void on_llena_lista_producto_clicked (object sender, EventArgs args)
 		{
 			treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				comando.CommandText = "SELECT to_char(hscmty_productos.id_producto,'999999999999') AS codProducto,"+
						"hscmty_productos.descripcion_producto,to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
						"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,"+
						"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
						"to_char(porcentage_ganancia,'99999.99') AS porcentageutilidad,to_char(costo_producto,'999999999.99') AS costoproducto "+
						"FROM hscmty_productos,hscmty_catalogo_almacenes,hscmty_grupo_producto,hscmty_grupo1_producto,hscmty_grupo2_producto "+
						"WHERE hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
						"AND hscmty_productos.id_producto = hscmty_catalogo_almacenes.id_producto "+
						"AND hscmty_catalogo_almacenes.id_almacen = '"+this.idalmacen.ToString().Trim()+"' "+
						"AND hscmty_catalogo_almacenes.eliminado = 'false' "+	
						"AND hscmty_productos.id_grupo1_producto = hscmty_grupo1_producto.id_grupo1_producto "+
						"AND hscmty_productos.id_grupo2_producto = hscmty_grupo2_producto.id_grupo2_producto "+
						"AND hscmty_productos.cobro_activo = true "+
						"AND hscmty_grupo_producto.agrupacion = 'MD1' "+
						"AND hscmty_productos.id_grupo_producto <= '7' "+
						"AND hscmty_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper()+"%' ORDER BY descripcion_producto; ";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				float tomaprecio;
				float calculodeiva;
				float preciomasiva;
				float tomadescue;
				float preciocondesc;
							
				while (lector.Read()){
					calculodeiva = 0;
					preciomasiva = 0;
					tomaprecio = float.Parse((string) lector["preciopublico"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					tomadescue = float.Parse((string) lector["porcentagesdesc"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					preciocondesc = tomaprecio;
					if ((bool) lector["aplicar_iva"]){
						calculodeiva = (tomaprecio * valoriva)/100;
					}
					if ((bool) lector["aplica_descuento"]){
						preciocondesc = tomaprecio-((tomaprecio*tomadescue)/100);
					}
					preciomasiva = tomaprecio + calculodeiva; 
					treeViewEngineBusca2.AppendValues (
											(string) lector["codProducto"],//0
											(string) lector["descripcion_producto"],//1
											(string) lector["descripcion_grupo_producto"],//2
											(string) lector["descripcion_grupo1_producto"],//3
											(string) lector["descripcion_grupo2_producto"],//4
											(string) lector["preciopublico"],//5
											calculodeiva.ToString("F").PadLeft(10),//6
											preciomasiva.ToString("F").PadLeft(10),//7
											(string) lector["porcentagesdesc"],//8
											preciocondesc.ToString("F").PadLeft(10),//9
											(string) lector["costoproductounitario"],//10
											(string) lector["porcentageutilidad"],//11
											(string) lector["costoproducto"]);//12
					
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		enum Column_prod
		{
			col_idproducto,
			col_desc_producto,
			col_grupoprod,
			col_grupo1prod,
			col_grupo2prod
		}
		
		void crea_treeview_solicitud()
		{
			//arraysolicitudmat = new ArrayList();
			
			treeViewEngineSolicitud = new ListStore(typeof(string),		// 0 
													typeof(string),		// 1
													typeof(string),		// 2
													typeof(string),		// 3
													typeof(string),		// 4
													typeof(string),		// 5
													typeof(string),		// costo_por_unidad
													typeof(string),		// precio_prodcuto_publico
													typeof(bool),		// Almacena cambia de Color
													typeof(bool),		// Cambia color verde cuando esta surtido
													typeof(string)		// Almacena el el numero de secuencia
													);
												
			lista_produc_solicitados.Model = treeViewEngineSolicitud;
			
			lista_produc_solicitados.RulesHint = true;
			
			TreeViewColumn col_descripcion = new TreeViewColumn();
			CellRendererText cel_descripcion = new CellRendererText();
			col_descripcion.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
			col_descripcion.PackStart(cel_descripcion, true);
			col_descripcion.AddAttribute (cel_descripcion, "text", 0);
			col_descripcion.SetCellDataFunc(cel_descripcion,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			col_descripcion.Resizable = true;
			col_descripcion.SortColumnId = (int) colum_solicitudes.col_descripcion;
			
			TreeViewColumn col_codigo_prod = new TreeViewColumn();
			CellRendererText cel_codigo_prod = new CellRendererText();
			col_codigo_prod.Title = "Codigo"; // titulo de la cabecera de la columna, si está visible
			col_codigo_prod.PackStart(cel_codigo_prod, true);
			col_codigo_prod.AddAttribute (cel_codigo_prod, "text", 1);
			col_codigo_prod.SetCellDataFunc(cel_codigo_prod,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_cant_solicitado = new TreeViewColumn();
			CellRendererText cel_cant_solicitado = new CellRendererText();
			col_cant_solicitado.Title = "Solicitado"; // titulo de la cabecera de la columna, si está visible
			col_cant_solicitado.PackStart(cel_cant_solicitado, true);
			col_cant_solicitado.AddAttribute (cel_cant_solicitado, "text", 2);
			col_cant_solicitado.SetCellDataFunc(cel_cant_solicitado,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_fecha_solicitado = new TreeViewColumn();
			CellRendererText cel_fecha_solicitado = new CellRendererText();
			col_fecha_solicitado.Title = "Fecha Solicitado"; // titulo de la cabecera de la columna, si está visible
			col_fecha_solicitado.PackStart(cel_fecha_solicitado, true);
			col_fecha_solicitado.AddAttribute (cel_fecha_solicitado, "text", 3);
			col_fecha_solicitado.SetCellDataFunc(cel_fecha_solicitado,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_cant_recibido = new TreeViewColumn();
			CellRendererText cel_cant_recibido = new CellRendererText();
			col_cant_recibido.Title = "Recibido"; // titulo de la cabecera de la columna, si está visible
			col_cant_recibido.PackStart(cel_cant_recibido, true);
			col_cant_recibido.AddAttribute (cel_cant_recibido, "text", 4);
			col_cant_recibido.SetCellDataFunc(cel_cant_recibido,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_fecha_recibido = new TreeViewColumn();
			CellRendererText cel_fecha_recibido = new CellRendererText();
			col_fecha_recibido.Title = "Fecha Recibido"; // titulo de la cabecera de la columna, si está visible
			col_fecha_recibido.PackStart(cel_fecha_recibido, true);
			col_fecha_recibido.AddAttribute (cel_fecha_recibido, "text", 5);
			col_fecha_recibido.SetCellDataFunc(cel_fecha_recibido,new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			lista_produc_solicitados.AppendColumn(col_descripcion);
			lista_produc_solicitados.AppendColumn(col_codigo_prod);
			lista_produc_solicitados.AppendColumn(col_cant_solicitado);
			lista_produc_solicitados.AppendColumn(col_fecha_solicitado);
			lista_produc_solicitados.AppendColumn(col_cant_recibido);
			lista_produc_solicitados.AppendColumn(col_fecha_recibido);
						
		}
		
		enum colum_solicitudes
		{
			col_descripcion,
			col_codigo_prod,
			col_cant_solicitado,
			col_fecha_solicitado,
			col_cant_recibido,
			cel_fecha_recibido
		}
		
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
 			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
 				if ((float) float.Parse((string) entry_cantidad_aplicada.Text) > 0){
 					treeViewEngineSolicitud.AppendValues ((string) model.GetValue(iterSelected, 1),
 														(string) model.GetValue(iterSelected, 0),
 														entry_cantidad_aplicada.Text,
 														(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
 														"",
 														"",
 														(string) model.GetValue(iterSelected, 10),
 														(string) model.GetValue(iterSelected, 5),
 														false,
 														false,
 														"");
 					
 					entry_cantidad_aplicada.Text = "0";
 				}else{
 					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error,ButtonsType.Close, 
											"La cantidad que quiere solicitar debe ser \n"+"distinta a cero, intente de nuevo");
					msgBoxError.Run ();					msgBoxError.Destroy();
 				}
 			} 			
 		}
 		
 		// Valida entradas que solo sean numericas, se utiliza en ventana de
		// carga de numero de solicitud
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_solicitud(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				llena_solicitud_material((string) this.entry_numero_solicitud.Text);				
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace){
				args.RetVal = true;
			}
		}
	    
	    // Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
			{
				args.RetVal = true;
			}
		}
		
		int ultima_solicitud()
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				comando.CommandText = "SELECT folio_de_solicitud "+
										"FROM hscmty_his_solicitudes_deta "+
										"WHERE 	id_almacen = '"+idalmacen.ToString().Trim()+"' "+
										"ORDER BY folio_de_solicitud DESC LIMIT 1";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if(lector.Read()){
					ultimasolicitud = (int) lector["folio_de_solicitud"] + 1;
				}else{
					ultimasolicitud = 1;
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			return (ultimasolicitud);
		}
		
		//ACCION QUE CAMBIA EL COLOR DEL TEXTO PARA CUANDO SE GUARDA EN LA BASE DE DATOS 
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//descripcion_producto descrip = (descripcion_producto) model.GetValue (iter, 14);
			if ((bool) this.lista_produc_solicitados.Model.GetValue (iter,8)==true){
				(cell as Gtk.CellRendererText).Foreground = "darkblue";
				if ((bool) this.lista_produc_solicitados.Model.GetValue (iter,9) == true){
					(cell as Gtk.CellRendererText).Foreground = "darkgreen";
				}
			}else{				
				(cell as Gtk.CellRendererText).Foreground = "red";
			}
			
		}
				
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}