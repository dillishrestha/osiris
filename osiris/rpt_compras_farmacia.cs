// rpt_compras_farmacia.cs created with MonoDevelop
// User: jbuentello at 10:54 a 15/08/2008
// Sistema Hospitalario Osiris
// Monterrey - Mexico
//
// Autor    	: Ing. Jesus Buentello Garza (Programacion)
//				 
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
// Programa		: rpt_compras_farmacia.cs
// Proposito	: Reportes de compras farmacia
// Objeto		: 
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using Gnome;

namespace osiris
{
	public class rpt_compras_farmacia
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		//ventana de Reporte 
		[Widget] Gtk.Window reporte_farmacia;
		[Widget] Gtk.TreeView lista_compra_farmacia;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_imprimir;
		[Widget] Gtk.Entry entry_dia1;
		[Widget] Gtk.Entry entry_mes1;
		[Widget] Gtk.Entry entry_ano1;
		[Widget] Gtk.Entry entry_dia2;
		[Widget] Gtk.Entry entry_mes2;
		[Widget] Gtk.Entry entry_ano2;
		
		string query_fechas = " ";
		int fila = -70;
		int contador = 1;
		int numpage = 1;		
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		string connectionString;
		
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		Gnome.Font fuente5 = Gnome.Font.FindClosest("Luxi Sans", 5);
		Gnome.Font fuente6 = Gnome.Font.FindClosest("Luxi Sans", 6);
		Gnome.Font fuente7 = Gnome.Font.FindClosest("Luxi Sans", 7);
		Gnome.Font fuente8 = Gnome.Font.FindClosest("Luxi Sans", 8);//Bitstream Vera Sans
		Gnome.Font fuente9 = Gnome.Font.FindClosest("Luxi Sans", 9);
		Gnome.Font fuente10 = Gnome.Font.FindClosest("Luxi Sans", 10);
		Gnome.Font fuente11 = Gnome.Font.FindClosest("Luxi Sans", 11);
		Gnome.Font fuente12 = Gnome.Font.FindClosest("Luxi Sans", 12);
		Gnome.Font fuente36 = Gnome.Font.FindClosest("Luxi Sans", 36);
		
		private TreeStore treeViewEngineFarmacia;
	
		//Declaracion de ventana de error y pregunta
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public rpt_compras_farmacia(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "reporte_farmacia", null);
			gxml.Autoconnect (this);        
			////// Muestra ventana de Glade
			reporte_farmacia.Show();
			
			entry_dia1.KeyPressEvent += onKeyPressEvent;
			entry_mes1.KeyPressEvent += onKeyPressEvent;
			entry_ano1.KeyPressEvent += onKeyPressEvent;
			entry_dia2.KeyPressEvent += onKeyPressEvent;
			entry_mes2.KeyPressEvent += onKeyPressEvent;
			entry_ano2.KeyPressEvent += onKeyPressEvent;
			entry_dia1.Text =DateTime.Now.ToString("dd");
			entry_mes1.Text =DateTime.Now.ToString("MM");
			entry_ano1.Text =DateTime.Now.ToString("yyyy");
			entry_dia2.Text =DateTime.Now.ToString("dd");
			entry_mes2.Text =DateTime.Now.ToString("MM");
			entry_ano2.Text =DateTime.Now.ToString("yyyy");
			
			this.button_imprimir.Clicked += new EventHandler(on_imprime_reporte_clicked);
			
			crea_treview_compra_farmacia();
			this.button_selecciona.Clicked += new EventHandler(on_llena_lista_compra_farmacia_clicked );
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
				
		}
		
		void crea_treview_compra_farmacia()
		{
			treeViewEngineFarmacia = new TreeStore(typeof(string),
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
													typeof(string),
													typeof(string));
													
			this.lista_compra_farmacia.Model = treeViewEngineFarmacia;
			this.lista_compra_farmacia.RulesHint = true;
			
			//lista_compra_farmacia.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono paciente*/
				
			TreeViewColumn col_folio = new TreeViewColumn();
			CellRendererText cel_folio = new CellRendererText();
			col_folio.Title = "Folio Atencion";
			col_folio.PackStart(cel_folio, true);
			col_folio.AddAttribute (cel_folio, "text", 0);
			col_folio.SortColumnId = (int) Column.col_folio;		
			
			TreeViewColumn col_orden_compra = new TreeViewColumn();
			CellRendererText cel_orden_compra = new CellRendererText();
			col_orden_compra.Title = "Orden Compra";
			col_orden_compra.PackStart(cel_orden_compra, true);
			col_orden_compra.AddAttribute(cel_orden_compra, "text", 1);
			col_orden_compra.Resizable = true;
			col_orden_compra.SortColumnId = (int) Column.col_orden_compra;
			
			TreeViewColumn col_codigo_producto = new TreeViewColumn();
			CellRendererText cel_codigo_producto = new CellRendererText();
			col_codigo_producto.Title = "CodProducto";
			col_codigo_producto.PackStart(cel_codigo_producto, true);
			col_codigo_producto.AddAttribute(cel_codigo_producto, "text", 2);
			col_codigo_producto.SortColumnId = (int) Column.col_codigo_producto;
			
			TreeViewColumn col_descripcion = new TreeViewColumn();
			CellRendererText cel_descripcion = new CellRendererText();
			col_descripcion.Title = "Descripcion";
			col_descripcion.PackStart(cel_descripcion, true);
			col_descripcion.AddAttribute(cel_descripcion, "text", 3);
			col_descripcion.Resizable = true;
			cel_descripcion.Width = 350;
			col_descripcion.SortColumnId = (int) Column.col_descripcion;
		
			TreeViewColumn col_costo = new TreeViewColumn();
			CellRendererText cel_costo = new CellRendererText();
			col_costo.Title = "Costo Uni";
			col_costo.PackStart(cel_costo, true);
			col_costo.AddAttribute(cel_costo, "text", 4);
			
			TreeViewColumn col_precio_producto = new TreeViewColumn();
			CellRendererText cel_precio_producto = new CellRendererText();
			col_precio_producto.Title = "Precio Producto";
			col_precio_producto.PackStart(cel_precio_producto, true);
			col_precio_producto.AddAttribute(cel_precio_producto, "text", 5);
			
			TreeViewColumn col_fecha = new TreeViewColumn();
			CellRendererText cel_fecha = new CellRendererText();
			col_fecha.Title = "Fecha";
			col_fecha.PackStart(cel_fecha, true);
			col_fecha.AddAttribute(cel_fecha, "text", 6);
			col_fecha.SortColumnId = (int) Column.col_fecha;
			
			TreeViewColumn col_surtir = new TreeViewColumn();
			CellRendererText cel_surtir = new CellRendererText();
			col_surtir.Title = "Surtir";
			col_surtir.PackStart(cel_surtir, true);
			col_surtir.AddAttribute (cel_surtir, "text", 7);

			
			TreeViewColumn col_embalaje = new TreeViewColumn();
			CellRendererText cel_embalaje = new CellRendererText();
			col_embalaje.Title = "Embalaje";
			col_embalaje.PackStart(cel_embalaje, true);
			col_embalaje.AddAttribute(cel_embalaje, "text", 8);
			col_embalaje.Resizable = true;
			
			
			TreeViewColumn col_autorizo = new TreeViewColumn();
			CellRendererText cel_autorizo = new CellRendererText();
			col_autorizo.Title = "Autorizo";
			col_autorizo.PackStart(cel_codigo_producto, true);
			col_autorizo.AddAttribute(cel_codigo_producto, "text", 9);

			
			TreeViewColumn col_ganancia = new TreeViewColumn();
			CellRendererText cel_ganancia = new CellRendererText();
			col_ganancia.Title = "%Ganancia";
			col_ganancia.PackStart(cel_ganancia, true);
			col_ganancia.AddAttribute(cel_ganancia, "text", 10);
		
			TreeViewColumn col_subalmacen = new TreeViewColumn();
			CellRendererText cel_subalmacen = new CellRendererText();
			col_subalmacen.Title = "SubAlmacen";
			col_subalmacen.PackStart(cel_subalmacen, true);
			col_subalmacen.AddAttribute(cel_subalmacen, "text", 11);
			
			TreeViewColumn col_compro = new TreeViewColumn();
			CellRendererText cel_compro = new CellRendererText();
			col_compro.Title = "Compro";
			col_compro.PackStart(cel_precio_producto, true);
			col_compro.AddAttribute(cel_precio_producto, "text", 12);
			
			TreeViewColumn col_medico = new TreeViewColumn();
			CellRendererText cel_medico = new CellRendererText();
			col_medico.Title = "Medico";
			col_medico.PackStart(cel_medico, true);
			col_medico.AddAttribute(cel_medico, "text", 13);
			col_medico.SortColumnId = (int) Column.col_medico;				
		
			lista_compra_farmacia.AppendColumn(col_folio);
			lista_compra_farmacia.AppendColumn(col_orden_compra);
			lista_compra_farmacia.AppendColumn(col_codigo_producto);
			lista_compra_farmacia.AppendColumn(col_descripcion);
			lista_compra_farmacia.AppendColumn(col_costo);
			lista_compra_farmacia.AppendColumn(col_precio_producto);
			lista_compra_farmacia.AppendColumn(col_fecha);
			lista_compra_farmacia.AppendColumn(col_surtir);
			lista_compra_farmacia.AppendColumn(col_embalaje);
			lista_compra_farmacia.AppendColumn(col_autorizo);
			lista_compra_farmacia.AppendColumn(col_ganancia);
			lista_compra_farmacia.AppendColumn(col_subalmacen);
			lista_compra_farmacia.AppendColumn(col_compro);
			lista_compra_farmacia.AppendColumn(col_medico);		
			
		}
		
			enum Column
		{
			col_folio,
			col_orden_compra,
			col_codigo_producto,
			col_descripcion,
			col_fecha,
			col_medico
		}
		
		
		
		void on_llena_lista_compra_farmacia_clicked (object sender, EventArgs args)
 		{
 			llenando_lista_compra_farmacia();
 		}
		
		void llenando_lista_compra_farmacia()
 		{
 			this.treeViewEngineFarmacia.Clear();
			
			query_fechas = "AND to_char(osiris_erp_compra_farmacia.fechahora_autorizacion,'yyyy-MM-dd') >= '"+entry_ano1.Text+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
				           "AND to_char(osiris_erp_compra_farmacia.fechahora_autorizacion,'yyyy-MM-dd') <= '"+entry_ano2.Text+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";			
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				comando.CommandText = "SELECT to_char(osiris_erp_compra_farmacia.folio_de_servicio,'999999') AS folioatencion,"+
									"to_char(osiris_erp_compra_farmacia.orden_compra,'99999999') AS ordencompra,"+
									"to_char(osiris_erp_compra_farmacia.id_producto,'999999999999') AS codproducto,"+
					                "osiris_productos.descripcion_producto,"+
									"to_char(osiris_erp_compra_farmacia.costo_por_unidad,'9999999999.99') AS costoporunidad,"+
									"to_char(osiris_erp_compra_farmacia.precio_producto_publico,'9999999999.99') AS preciopublico,"+
									"to_char(osiris_erp_compra_farmacia.total_surtir,'999999.99') AS surtir,"+
									"to_char(osiris_erp_compra_farmacia.cantidad_embalaje,'999999.99') AS embalaje,"+
									"to_char(osiris_erp_compra_farmacia.cantidad_autorizo,'9999.99') AS autorizo,"+
									"to_char(osiris_erp_compra_farmacia.porcentage_ganancia,'999.99') AS porcentageganancia,"+
									"to_char(osiris_erp_compra_farmacia.fechahora_autorizacion,'dd-MM-yyyy') AS fechahrautorizacion,"+ 
									"osiris_erp_compra_farmacia.id_subalmacen,"+
									"osiris_erp_compra_farmacia.id_quien_compro,"+
									"osiris_erp_compra_farmacia.id_proveedor,"+
									"osiris_erp_compra_farmacia.id_medico,"+
						            "osiris_his_medicos.nombre_medico,"+
						            "to_char(osiris_erp_compra_farmacia.costo_producto,'9999999.999') AS costo_prod,"+
						            "osiris_almacenes.descripcion_almacen "+ 	
                                    "FROM osiris_erp_compra_farmacia,osiris_productos,osiris_his_medicos,osiris_almacenes "+ 
									"WHERE osiris_erp_compra_farmacia.id_producto = osiris_productos.id_producto "+ 
						            " "+query_fechas+" "+
									"AND osiris_erp_compra_farmacia.id_producto = osiris_productos.id_producto "+
						            "AND osiris_erp_compra_farmacia.id_medico = osiris_his_medicos.id_medico "+
						            "AND osiris_erp_compra_farmacia.id_subalmacen = osiris_almacenes.id_almacen "+
									"ORDER BY orden_compra;";
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
						
				while (lector.Read()){
						this.treeViewEngineFarmacia.AppendValues((string) lector["folioatencion"],
					                                         (string) lector["ordencompra"],
					                                         (string) lector["codproducto"],
					                                         (string) lector["descripcion_producto"],
					                                         (string) lector["costoporunidad"],
					                                         (string) lector["preciopublico"],
					                                         (string) lector["fechahrautorizacion"],
					                                         (string) lector["surtir"],
					                                         (string) lector["embalaje"],
					                                         (string) lector["autorizo"],
					                                         (string) lector["porcentageganancia"],
					                                         (string) lector["descripcion_almacen"],
					                                         (string) lector["id_quien_compro"],
					                                         (string) lector["nombre_medico"]); 
					

				}
				
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮ）（ｔｒｓｑ ";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
			{
				args.RetVal = true;
			}
		}
		
		void on_imprime_reporte_clicked (object sender, EventArgs args)
		{
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob ();
			Gnome.PrintDialog dialogo = new Gnome.PrintDialog (trabajo, "Reporte de Farmacia", 0);
						
			int respuesta = dialogo.Run ();
			if (respuesta == (int) Gnome.PrintButtons.Cancel){
				dialogo.Hide (); 
				dialogo.Dispose (); 
				return;
			}
			Gnome.PrintContext ctx = trabajo.Context;
			ComponerPagina2(ctx, trabajo); 
			trabajo.Close();
			switch (respuesta){
				case (int) PrintButtons.Print:   
					trabajo.Print (); 
				break;
				case (int) PrintButtons.Preview:
					new PrintJobPreview(trabajo, "REPORTE FARMACIA").Show();
				break;
			}
			dialogo.Hide (); dialogo.Dispose ();	
		}
		
		void ComponerPagina2 (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			TreeIter iter;
			fila = -90;
			
			contador = 0;
			numpage = 1;
			ContextoImp.BeginPage("Pagina 1");	
			ContextoImp.Rotate(90);
			imprime_encabezado(ContextoImp,trabajoImpresion);
			Gnome.Print.Setfont (ContextoImp, fuente5);
			
			string toma_descrip_prod;
			string toma_descrip_alm;
			
			if (this.treeViewEngineFarmacia.GetIterFirst (out iter)){
				
				Gnome.Print.Setfont (ContextoImp, fuente5);
				ContextoImp.MoveTo(30, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,0));
				ContextoImp.MoveTo(55, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,1));
				ContextoImp.MoveTo(90, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,2));

				toma_descrip_prod = (string) this.treeViewEngineFarmacia.GetValue (iter,3);
					
				if(toma_descrip_prod.Length > 51){
					toma_descrip_prod = toma_descrip_prod.Substring(0,50);
				}  				
					
				ContextoImp.MoveTo(145, fila);		ContextoImp.Show(toma_descrip_prod);
				ContextoImp.MoveTo(315, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,4));
				ContextoImp.MoveTo(350, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,5));
				ContextoImp.MoveTo(395, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,6));
				ContextoImp.MoveTo(425, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,7));
				ContextoImp.MoveTo(460, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,8));
				ContextoImp.MoveTo(497, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,9));
				ContextoImp.MoveTo(528, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,10));
				
				
				toma_descrip_alm = (string) this.treeViewEngineFarmacia.GetValue (iter,11);
					
				if(toma_descrip_alm.Length > 22){
					toma_descrip_alm = toma_descrip_alm.Substring(0,21);
					
				}  				
					
				ContextoImp.MoveTo(562, fila);		ContextoImp.Show(toma_descrip_alm);
								
				//ContextoImp.MoveTo(585, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,11));
								
				ContextoImp.MoveTo(645, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,12));
				ContextoImp.MoveTo(687, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,13));

				fila-=10;
				contador+=1;
				salto_pagina(ContextoImp,trabajoImpresion);		
			}
			
			while (this.treeViewEngineFarmacia.IterNext(ref iter)){
				
				Gnome.Print.Setfont (ContextoImp, fuente5);
				ContextoImp.MoveTo(30, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,0));
				ContextoImp.MoveTo(55, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,1));
				ContextoImp.MoveTo(90, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,2));

				toma_descrip_prod = (string) this.treeViewEngineFarmacia.GetValue (iter,3);
					
				if(toma_descrip_prod.Length > 51){
					toma_descrip_prod = toma_descrip_prod.Substring(0,50);
				}  				
					
				ContextoImp.MoveTo(145, fila);		ContextoImp.Show(toma_descrip_prod);
				ContextoImp.MoveTo(315, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,4));
				ContextoImp.MoveTo(350, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,5));
				ContextoImp.MoveTo(395, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,6));
				ContextoImp.MoveTo(425, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,7));
				ContextoImp.MoveTo(460, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,8));
				ContextoImp.MoveTo(497, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,9));
				ContextoImp.MoveTo(528, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,10));
				
				
				toma_descrip_alm = (string) this.treeViewEngineFarmacia.GetValue (iter,11);
					
				if(toma_descrip_alm.Length > 22){
					toma_descrip_alm = toma_descrip_alm.Substring(0,21);

				}  				
					
				ContextoImp.MoveTo(562, fila);		ContextoImp.Show(toma_descrip_alm);
				
				//ContextoImp.MoveTo(585, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,11));
				
				ContextoImp.MoveTo(645, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,12));
				ContextoImp.MoveTo(687, fila);	ContextoImp.Show((string) this.treeViewEngineFarmacia.GetValue (iter,13));

				fila-=10;
				contador+=1;
				salto_pagina(ContextoImp,trabajoImpresion);
			}
			ContextoImp.ShowPage();
		}
		
		void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
	        if (contador > 48 ){
	        	numpage +=1;        	contador=1;
	        	fila = -90;
	        	ContextoImp.ShowPage();
				ContextoImp.BeginPage("Pagina "+numpage.ToString());

				ContextoImp.Rotate(90);
				imprime_encabezado(ContextoImp,trabajoImpresion);
	     	}
		}
		
		void imprime_encabezado(Gnome.PrintContext ContextoImp,Gnome.PrintJob trabajoImpresion)
		{        		
      		// Cambiar la fuente
			Gnome.Print.Setfont (ContextoImp, fuente6);
			ContextoImp.MoveTo(65.5, -30);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(66, -30);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(65.5, -40);			ContextoImp.Show("Direccion: ");
			ContextoImp.MoveTo(66, -40);			ContextoImp.Show("Direccion: ");
			ContextoImp.MoveTo(65.5, -50);			ContextoImp.Show("Conmutador: ");
			ContextoImp.MoveTo(66, -50);			ContextoImp.Show("Conmutador: ");

			Gnome.Print.Setfont (ContextoImp, fuente12);
			ContextoImp.MoveTo(66, -65);			ContextoImp.Show("REPORTE FARMACIA");
			ContextoImp.MoveTo(66, -65);			ContextoImp.Show("REPORTE FARMACIA");
						
			Gnome.Print.Setfont (ContextoImp, fuente8);
			ContextoImp.MoveTo(35, -80);			ContextoImp.Show("Folio");
			ContextoImp.MoveTo(63, -80);			ContextoImp.Show("Orden");
			ContextoImp.MoveTo(100, -80);			ContextoImp.Show("Codigo");
			ContextoImp.MoveTo(175, -80);			ContextoImp.Show("Descripcion");
			ContextoImp.MoveTo(325, -80);			ContextoImp.Show("CostoUni");
			ContextoImp.MoveTo(365, -80);			ContextoImp.Show("Precio");
			ContextoImp.MoveTo(400, -80);			ContextoImp.Show("Fecha");
			ContextoImp.MoveTo(432, -80);			ContextoImp.Show("Surtir");
			ContextoImp.MoveTo(460, -80);			ContextoImp.Show("Embalaje");
			
			ContextoImp.MoveTo(500, -80);			ContextoImp.Show("Autoz");
			ContextoImp.MoveTo(528, -80);			ContextoImp.Show("%Gana");
			ContextoImp.MoveTo(566, -80);			ContextoImp.Show("SubAlmacen");
			ContextoImp.MoveTo(643, -80);			ContextoImp.Show("Compro");
			ContextoImp.MoveTo(698, -80);			ContextoImp.Show("Medico");
      	}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)	
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}	
	}
}