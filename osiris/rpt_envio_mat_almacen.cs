// created on 07/02/2008 at 09:34 a
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Ing. Jesus Buentello G. (Programacion)
//				  Ing. Daniel Olivares C. (Adecuaciones y mejoras) arcangeldoc@gmail.com 05/05/2007
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
//////////////////////////////////////////////////////
using System;
using Gtk;
using Gnome;
using Npgsql;
using System.Data;
using Glade;
using System.Collections;
using GtkSharp;

namespace osiris
{
	public class rpt_envio_almacen
	{
		//[Widget] Gtk.Entry entry_fecha_inicio;
		
		[Widget] Gtk.Entry entry_dia_inicio;
		[Widget] Gtk.Entry entry_mes_inicio;
		[Widget] Gtk.Entry entry_ano_inicio;
		
		//[Widget] Gtk.Entry entry_fecha_termino;
		
		[Widget] Gtk.Entry entry_dia_termino;
		[Widget] Gtk.Entry entry_mes_termino;
		[Widget] Gtk.Entry entry_ano_termino;
		
		[Widget] Gtk.CheckButton checkbutton_todos_envios;
		[Widget] Gtk.CheckButton checkbutton_seleccion_presupuestos;
		[Widget] Gtk.TreeView lista_almacenes;
		[Widget] Gtk.Button button_buscar;
		[Widget] Gtk.Button button_rep;
		[Widget] Gtk.Button button_salir;
		
		string query_fechas = " ";
		string rango1 = "";
		string rango2 = "";
		string nombrebd;
		int idsubalmacen;
		int filas=690;
		int contador = 1;
		int numpage = 1;
		
		private ListStore treeViewEnginesolicitud;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
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
		
		string connectionString;
		
		class_conexion conexion_a_DB = new class_conexion();
						
		public rpt_envio_almacen(string nombrebd_,int idsubalmacen_)		
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			idsubalmacen = idsubalmacen_;
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "envio_almacenes", null);
			gxml.Autoconnect (this);
			
			entry_dia_inicio.Text = DateTime.Now.ToString("dd");
			entry_mes_inicio.Text = DateTime.Now.ToString("MM");
			entry_ano_inicio.Text = DateTime.Now.ToString("yyyy");
			
			entry_dia_termino.Text = DateTime.Now.ToString("dd");
			entry_mes_termino.Text = DateTime.Now.ToString("MM");
			entry_ano_termino.Text = DateTime.Now.ToString("yyyy");
			
			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
            button_buscar.Clicked += new EventHandler(on_buscar_clicked);
           	button_rep.Clicked += new EventHandler(on_button_rep_clicked);
          	checkbutton_todos_envios.Clicked += new EventHandler(on_checkbutton_todos_envios);
          	checkbutton_seleccion_presupuestos.Hide();
          	
          	crea_treeview_solicitud();
		}
            
		void crea_treeview_solicitud()
		{
			treeViewEnginesolicitud = new ListStore(typeof(bool),//0
														typeof(string),
														typeof(string),
														typeof(string),
														typeof(string),
														typeof(string));//5
				
			lista_almacenes.Model = treeViewEnginesolicitud;
			//lista_almacenes.RulesHint = true;
				
			TreeViewColumn col_seleccion = new TreeViewColumn();
			CellRendererToggle cellr0 = new CellRendererToggle();
			col_seleccion.Title = "Seleccion"; // titulo de la cabecera de la columna, si está visible
			col_seleccion.PackStart(cellr0, true);
			col_seleccion.AddAttribute (cellr0, "active", 0);
			cellr0.Activatable = true;
			cellr0.Toggled += selecciona_fila_grupo; 
		
			TreeViewColumn col_solicito = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_solicito.Title = "Solicitud"; // titulo de la cabecera de la columna, si está visible
			col_solicito.PackStart(cellr1, true);
			col_solicito.AddAttribute (cellr1, "text", 1);
			cellr1.Foreground = "darkblue";
			
			TreeViewColumn col_sub = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_sub.Title = "Sub Almacen"; // titulo de la cabecera de la columna, si está visible
			col_sub.PackStart(cellr2, true);
			col_sub.AddAttribute (cellr2, "text", 2);
			cellr2.Foreground = "darkblue";
						
			TreeViewColumn col_fecha_envio = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_fecha_envio.Title = "Fecha Envio"; // titulo de la cabecera de la columna, si está visible
			col_fecha_envio.PackStart(cellr3, true);
			col_fecha_envio.AddAttribute (cellr3, "text", 3);
			cellr3.Foreground = "darkblue";
			
			TreeViewColumn col_id_sol = new TreeViewColumn();
			CellRendererText cellr4 = new CellRendererText();
			col_id_sol.Title = "id Solicito"; // titulo de la cabecera de la columna, si está visible
			col_id_sol.PackStart(cellr4, true);
			col_id_sol.AddAttribute (cellr4, "text", 4);
			cellr4.Foreground = "darkblue";
			
			lista_almacenes.AppendColumn(col_seleccion);
			lista_almacenes.AppendColumn(col_solicito);
			lista_almacenes.AppendColumn(col_sub);
			lista_almacenes.AppendColumn(col_fecha_envio);
			lista_almacenes.AppendColumn(col_id_sol);
		}
		
		void llenando_lista_de_solicitudes()
		{
			treeViewEnginesolicitud.Clear();
			//treeViewEnginesolicitud.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
					comando.CommandText = "SELECT COUNT(osiris_his_solicitudes_deta.folio_de_solicitud) AS cantidad_solicitud,to_char(folio_de_solicitud,'9999999999') AS foliodesolicitud,"+
								"to_char(osiris_his_solicitudes_deta.id_almacen,'999999999') AS idalmacen,osiris_almacenes.descripcion_almacen AS descripcionalmacen,"+
								"to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'dd-MM-yyyy HH24:mi') AS fecha_envio,osiris_his_solicitudes_deta.id_empleado "+
								"FROM osiris_his_solicitudes_deta,osiris_almacenes "+								
								"WHERE osiris_his_solicitudes_deta.id_almacen = osiris_almacenes.id_almacen "+
								"AND osiris_his_solicitudes_deta.folio_de_solicitud > 0 "+
								"AND status = 'true' "+								
								""+query_fechas+" "+
								"GROUP BY osiris_his_solicitudes_deta.folio_de_solicitud,osiris_his_solicitudes_deta.id_almacen,"+
								"osiris_almacenes.descripcion_almacen,to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'dd-MM-yyyy HH24:mi'),osiris_his_solicitudes_deta.id_empleado "+
								"ORDER BY osiris_his_solicitudes_deta.id_almacen,osiris_his_solicitudes_deta.folio_de_solicitud;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();

				while (lector.Read())
				{							
					treeViewEnginesolicitud.AppendValues (false, //0
													(string) lector["foliodesolicitud"],//1
													(string) lector["descripcionalmacen"], // 2
													(string) lector["fecha_envio"],//3
													(string) lector["id_empleado"],//4
													(string) lector["idalmacen"]);
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void selecciona_fila_grupo(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (lista_almacenes.Model.GetIter (out iter, path)) {
				bool old = (bool) lista_almacenes.Model.GetValue (iter,0);
				lista_almacenes.Model.SetValue(iter,0,!old);
			}
		}
		
		//Seleccionar todos los del treeview, un check_button 
		void on_checkbutton_todos_envios(object sender, EventArgs args)
		{
			if ((bool)checkbutton_todos_envios.Active == true){
				TreeIter iter2;
				if (treeViewEnginesolicitud.GetIterFirst (out iter2)){
					lista_almacenes.Model.SetValue(iter2,0,true);
					while (treeViewEnginesolicitud.IterNext(ref iter2)){
						lista_almacenes.Model.SetValue(iter2,0,true);
					}
				}
			}else{
				TreeIter iter2;
				if (treeViewEnginesolicitud.GetIterFirst (out iter2)){
					lista_almacenes.Model.SetValue(iter2,0,false);
					while (treeViewEnginesolicitud.IterNext(ref iter2)){
						lista_almacenes.Model.SetValue(iter2,0,false);
					}
				}
			}
		}
		
		void on_buscar_clicked (object sender, EventArgs args)
		{
			if (checkbutton_todos_envios.Active == true) {
				query_fechas = " ";	 
				rango1 = "";
				rango2 = "";
			}else{
				rango1 = entry_ano_inicio.Text+"-"+entry_mes_inicio.Text+"-"+entry_dia_inicio.Text;
				rango2 = entry_ano_termino.Text+"-"+entry_mes_termino.Text+"-"+entry_dia_termino.Text;
				query_fechas = " AND to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'yyyy-MM-dd') >= '"+rango1+"' "+
								"AND to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'yyyy-MM-dd') <= '"+rango2+"' ";
				}	
				llenando_lista_de_solicitudes();
			}
			
		void on_button_rep_clicked(object sender, EventArgs args)
		{
			if (this.checkbutton_todos_envios.Active == true) { 
				query_fechas = " ";	 
				rango1 = "";
				rango2 = "";
			}else{
				rango1 = entry_ano_inicio.Text+"-"+entry_mes_inicio.Text+"-"+entry_dia_inicio.Text;
				rango2 = entry_ano_termino.Text+"-"+entry_mes_termino.Text+"-"+entry_dia_termino.Text;				
				query_fechas = " AND to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'yyyy-MM-dd') >= '"+rango1+"' "+
								"AND to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'yyyy-MM-dd') <= '"+rango2+"' ";
			}
			
			Gnome.PrintJob    trabajo = new Gnome.PrintJob(Gnome.PrintConfig.Default());
        	Gnome.PrintDialog dialogo = new Gnome.PrintDialog(trabajo, "Envio Materiales Almacen", 0);
        	
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
                new PrintJobPreview(trabajo, "Envio Materiales Almacen").Show();
                break;
        	}
			dialogo.Hide (); dialogo.Dispose ();
		}	
			
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{   
			TreeIter iter;	
			filas=690;
			
			string numeros_seleccionado = "";
			string almacenes_seleccionados = ""; 
			string variable_paso_03 = "";
			string variable_paso_04 = "";
			int variable_paso_02_1 = 0;
			string fechaautorizacion = "";
			
			//poder elegir una fila del treeview
			if (treeViewEnginesolicitud.GetIterFirst (out iter)){
			
 				if ((bool) lista_almacenes.Model.GetValue (iter,0) == true){
 					numeros_seleccionado = (string) lista_almacenes.Model.GetValue (iter,1);
 					almacenes_seleccionados = (string) lista_almacenes.Model.GetValue (iter,5);
 					variable_paso_02_1 += 1;		
 				}
 				while (treeViewEnginesolicitud.IterNext(ref iter)){
 					if ((bool) lista_almacenes.Model.GetValue (iter,0) == true){
 				    	if (variable_paso_02_1 == 0){ 				    	
 							numeros_seleccionado = (string) lista_almacenes.Model.GetValue (iter,1);
 							almacenes_seleccionados = (string) lista_almacenes.Model.GetValue (iter,5);
 							variable_paso_02_1 += 1;
 						}else{
 							variable_paso_03 = (string) lista_almacenes.Model.GetValue (iter,1);
 							variable_paso_04 = (string) lista_almacenes.Model.GetValue (iter,5);
 							numeros_seleccionado = numeros_seleccionado.Trim() + "','" + variable_paso_03.Trim();
 							almacenes_seleccionados = almacenes_seleccionados.Trim() + "','" + variable_paso_04.Trim();
 						}
 					}
 				}
 			}
 			string query_in_num = "";
 			string query_in_almacen = ""; 
 			
 			if (variable_paso_02_1 > 0){
	 			query_in_num = " AND osiris_his_solicitudes_deta.folio_de_solicitud IN ('"+numeros_seleccionado+"') ";
				query_in_almacen = " AND osiris_his_solicitudes_deta.id_almacen IN ('"+almacenes_seleccionados+"') ";
			}
			NpgsqlConnection conexion; 
        	conexion = new NpgsqlConnection (connectionString+nombrebd);
        	// Verifica que la base de dato s este conectada
        	try{
	 			conexion.Open ();
	        	NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand (); 

	        	comando.CommandText = "SELECT DISTINCT (osiris_his_solicitudes_deta.folio_de_solicitud), "+
	        		       		"to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'dd-MM-yyyy'),"+
								"to_char(osiris_his_solicitudes_deta.folio_de_solicitud,'999999999') AS foliosol,"+
								"to_char(osiris_his_solicitudes_deta.fechahora_solicitud,'dd-MM-yyyy HH24:mi') AS fecha_sol,"+
								"to_char(osiris_his_solicitudes_deta.fechahora_autorizado,'dd-MM-yyyy') AS fecha_autorizado,"+
								"to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'dd-MM-yyyy HH24:mi') AS fecha_envio,"+								
								"osiris_his_solicitudes_deta.id_quien_solicito,"+
								"to_char(osiris_productos.id_producto,'999999999999') AS idproducto,"+
								"osiris_his_solicitudes_deta.id_producto,"+
								"osiris_his_solicitudes_deta.sin_stock,"+	
								"osiris_his_solicitudes_deta.solicitado_erroneo,"+
								"osiris_his_solicitudes_deta.surtido,"+
								"osiris_empleado.id_empleado,"+	
								"osiris_productos.descripcion_producto,"+
								"to_char(osiris_his_solicitudes_deta.cantidad_solicitada,'9999999.99') AS cantsol,"+
								"to_char(osiris_his_solicitudes_deta.cantidad_autorizada,'9999999.99') AS cantaut,"+
								"osiris_his_solicitudes_deta.id_almacen AS idalmacen,osiris_almacenes.descripcion_almacen,osiris_almacenes.id_almacen,"+
								"osiris_empleado.nombre1_empleado || ' ' || "+"osiris_empleado.nombre2_empleado || ' ' || "+"osiris_empleado.apellido_paterno_empleado || ' ' || "+ 
								"osiris_empleado.apellido_materno_empleado AS nombreempl "+
								"FROM osiris_his_solicitudes_deta,osiris_almacenes,osiris_productos,osiris_empleado "+
								"WHERE osiris_his_solicitudes_deta.id_almacen = osiris_almacenes.id_almacen "+
								"AND osiris_empleado.login_empleado = osiris_his_solicitudes_deta.id_empleado "+
								"AND osiris_his_solicitudes_deta.folio_de_solicitud > 0 "+
								"AND osiris_productos.cobro_activo = 'true' "+
								"AND osiris_his_solicitudes_deta.id_producto = osiris_productos.id_producto "+
								"AND osiris_his_solicitudes_deta.eliminado = 'false' "+
								query_in_num+
								query_in_almacen+
								query_fechas+" ORDER BY osiris_his_solicitudes_deta.id_almacen,osiris_his_solicitudes_deta.folio_de_solicitud;";
								
		        	Console.WriteLine(comando.CommandText);
		        	NpgsqlDataReader lector = comando.ExecuteReader ();
		        	ContextoImp.BeginPage("Pagina 1");
		        	imprime_encabezado(ContextoImp,trabajoImpresion);
		        	Gnome.Print.Setfont (ContextoImp, fuente6);
		        	
		        	int grupo = 0;
		        	int numero_almacen = 0;
		        		
		        	string toma_descrip_prod;
		        	if (lector.Read()){
		        		
		        		Gnome.Print.Setfont (ContextoImp, fuente8);
			        	ContextoImp.MoveTo(300, 740);		 ContextoImp.Show((string) lector["foliosol"]);
			        	ContextoImp.MoveTo(250, 740);		 ContextoImp.Show("No. SOLICITUD:");
			        	ContextoImp.MoveTo(380, 740);		 ContextoImp.Show("Envio:");
						ContextoImp.MoveTo(405, 740);		 ContextoImp.Show((string) lector["fecha_envio"]);
						ContextoImp.MoveTo(220, 720);		 ContextoImp.Show((string) lector["nombreempl"]);
		        		Gnome.Print.Setfont (ContextoImp, fuente6);
						ContextoImp.MoveTo(20, 720);		 ContextoImp.Show("Solicito:");
						ContextoImp.MoveTo(70, 720);		 ContextoImp.Show((string) lector["id_quien_solicito"]);
						ContextoImp.MoveTo(20, 730);		 ContextoImp.Show("Area Solicitud:");
						ContextoImp.MoveTo(70, 730);		 ContextoImp.Show((string) lector["descripcion_almacen"]);
						ContextoImp.MoveTo(165, 720);		 ContextoImp.Show("Nom.Solicitante:");
						
						contador+=1;
        				salto_pagina(ContextoImp,trabajoImpresion,contador);
        				
		        		grupo = (int) lector["folio_de_solicitud"];
		        		numero_almacen = (int) lector["idalmacen"];
						
				
						//comprueba las notas de los resultado en el reporte
						if((bool) lector["sin_stock"] == true){
							ContextoImp.MoveTo(530,filas);	 ContextoImp.Show("sin stock");
						}
						if((bool) lector["solicitado_erroneo"] == true){
							ContextoImp.MoveTo(530,filas);	 ContextoImp.Show("Pedido Erroneo");
						}
						
						if((bool) lector["surtido"] == true){
							ContextoImp.MoveTo(530,filas);	 ContextoImp.Show("surtido");
						}
						if(float.Parse((string) lector["cantaut"]) == 0 && (bool) lector["sin_stock"] == false && (bool) lector["solicitado_erroneo"] == false && (bool) lector["surtido"] == false){
							ContextoImp.MoveTo(530,filas);	 ContextoImp.Show("No surtido");
						}
						
						toma_descrip_prod = (string) lector["descripcion_producto"];
						if(toma_descrip_prod.Length > 70){
								toma_descrip_prod = toma_descrip_prod.Substring(0,69);
						}  			
						ContextoImp.MoveTo(120, filas);		ContextoImp.Show(toma_descrip_prod);

			        	ContextoImp.MoveTo(65,filas);	 ContextoImp.Show((string) lector["idproducto"]);
			        	
			        	ContextoImp.MoveTo(15,filas);	 ContextoImp.Show((string) lector["cantsol"]);
			        	ContextoImp.MoveTo(400,filas);	 ContextoImp.Show((string) lector["cantaut"]);    	
			        	
			        	if( (string) lector["fecha_autorizado"] == "02-01-2000"){
							fechaautorizacion = "";
						}else{
							fechaautorizacion = (string) lector["fecha_autorizado"];
						}
						ContextoImp.MoveTo(465,filas);	 ContextoImp.Show(fechaautorizacion);
			        	
			        	filas-=10;
						contador+=1;
						salto_pagina(ContextoImp,trabajoImpresion,contador);
					}
					
		        	while (lector.Read()){
		        		
			        	if((bool) lector["sin_stock"] == true){
							ContextoImp.MoveTo(530,filas);	 ContextoImp.Show("sin stock");
						}
						if((bool) lector["solicitado_erroneo"] == true){
							ContextoImp.MoveTo(530,filas);	 ContextoImp.Show("Pedido Erroneo");
						}
						if((bool) lector["surtido"] == true){
							ContextoImp.MoveTo(530,filas);	 ContextoImp.Show("surtido");
						}
						if(float.Parse((string) lector["cantaut"]) == 0 && (bool) lector["sin_stock"] == false && (bool) lector["solicitado_erroneo"] == false && (bool) lector["surtido"] == false){
							ContextoImp.MoveTo(530,filas);	 ContextoImp.Show("No surtido");
						}
						toma_descrip_prod = (string) lector["descripcion_producto"];
						
						if(toma_descrip_prod.Length > 70){
							toma_descrip_prod = toma_descrip_prod.Substring(0,69);
						}  			
						ContextoImp.MoveTo(65,filas);	 ContextoImp.Show((string) lector["idproducto"]);
						ContextoImp.MoveTo(120, filas);		ContextoImp.Show(toma_descrip_prod);
			        	//ContextoImp.MoveTo(75,filas);	 ContextoImp.Show((string) lector["descripcion_producto"]);
			        	ContextoImp.MoveTo(15,filas);	 ContextoImp.Show((string) lector["cantsol"]);
			        	ContextoImp.MoveTo(400,filas);	 ContextoImp.Show((string) lector["cantaut"]);
			        	
			        	if( (string) lector["fecha_autorizado"] == "02-01-2000"){
							fechaautorizacion = "";
						}else{
							fechaautorizacion = (string) lector["fecha_autorizado"];
						}
						ContextoImp.MoveTo(465,filas);	 ContextoImp.Show(fechaautorizacion);
			        	
			        	filas-=10;
						contador+=1;
						salto_pagina(ContextoImp,trabajoImpresion,contador);
		        	
		        	if(grupo != (int) lector["folio_de_solicitud"] || numero_almacen != (int) lector["idalmacen"]){
							Gnome.Print.Setfont (ContextoImp, fuente9);
							ContextoImp.MoveTo(30, 70);			ContextoImp.Show("      Persona Recibe:  ");
							ContextoImp.MoveTo(30, 75); 		ContextoImp.Show("-------------------------------");
							ContextoImp.MoveTo(30, 35);			ContextoImp.Show("          Firma   ");
							ContextoImp.MoveTo(30, 40);			ContextoImp.Show("----------------------------");
							ContextoImp.MoveTo(250,	70);		ContextoImp.Show("      Persona Entrega:  ");
							ContextoImp.MoveTo(250, 75);		ContextoImp.Show("-------------------------------");
							ContextoImp.MoveTo(250, 35);		ContextoImp.Show("         Firma   ");
							ContextoImp.MoveTo(250, 40);		ContextoImp.Show("----------------------------");
							Gnome.Print.Setfont (ContextoImp, fuente9);

							contador=1;
				        	numpage +=1;
			        		ContextoImp.ShowPage();
							ContextoImp.BeginPage("Pagina "+numpage.ToString());
							Gnome.Print.Setfont (ContextoImp, fuente6);
							imprime_encabezado(ContextoImp,trabajoImpresion);
							
		        			Gnome.Print.Setfont (ContextoImp, fuente8);
				        	ContextoImp.MoveTo(300, 740);		 ContextoImp.Show((string) lector["foliosol"]);
				        	ContextoImp.MoveTo(250, 740);		 ContextoImp.Show("No. SOLICITUD:");
				        	ContextoImp.MoveTo(380, 740);		 ContextoImp.Show("Envio:");
							ContextoImp.MoveTo(405, 740);		 ContextoImp.Show((string) lector["fecha_envio"]);
							ContextoImp.MoveTo(220, 720);		 ContextoImp.Show((string) lector["nombreempl"]);
			        		Gnome.Print.Setfont (ContextoImp, fuente6);
							ContextoImp.MoveTo(20, 720);		 ContextoImp.Show("Solicito:");
							ContextoImp.MoveTo(70, 720);		 ContextoImp.Show((string) lector["id_quien_solicito"]);
							ContextoImp.MoveTo(20, 730);		 ContextoImp.Show("Area Solicitud:");
							ContextoImp.MoveTo(70, 730);		 ContextoImp.Show((string) lector["descripcion_almacen"]);
							ContextoImp.MoveTo(165, 720);		 ContextoImp.Show("Nom.Solicitante:");
							
							contador+=1;
        					salto_pagina(ContextoImp,trabajoImpresion,contador);
        					
		        			grupo = (int) lector["folio_de_solicitud"];
		        			numero_almacen = (int) lector["idalmacen"];
							
		        		}
		        	}
		        	
					Gnome.Print.Setfont (ContextoImp, fuente9);
					ContextoImp.MoveTo(30, 70);			ContextoImp.Show("      Persona Recibe:  ");
					ContextoImp.MoveTo(30, 75);			ContextoImp.Show("-------------------------------");
					ContextoImp.MoveTo(30, 35);			ContextoImp.Show("          Firma   ");
					ContextoImp.MoveTo(30, 40);			ContextoImp.Show("----------------------------");
					ContextoImp.MoveTo(250, 70);		ContextoImp.Show("      Persona Entrega:  ");
					ContextoImp.MoveTo(250, 75);		ContextoImp.Show("-------------------------------");
					ContextoImp.MoveTo(250, 35);		ContextoImp.Show("         Firma   ");
					ContextoImp.MoveTo(250, 40);		ContextoImp.Show("----------------------------");
					Gnome.Print.Setfont (ContextoImp, fuente9);

		        	ContextoImp.ShowPage();
		        	
			}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
			}
		}
		
		void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion,int contador)
		{
		Console.WriteLine("afuera"+contador);
	        if (contador > 60)
	        {	
	        Console.WriteLine("entra"+contador);
	        	filas=690;

	        	numpage += 1;
	           	
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
			ContextoImp.MoveTo(19.7, 770);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(20, 770);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(19.7, 760);			ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(20, 760);			ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(19.7, 750);			ContextoImp.Show("Conmutador:");
			ContextoImp.MoveTo(20, 750);			ContextoImp.Show("Conmutador:");

			Gnome.Print.Setfont (ContextoImp, fuente7);
			ContextoImp.MoveTo(20, 700);			ContextoImp.Show("Cantidad");										
			ContextoImp.MoveTo(70, 700);			ContextoImp.Show("Folio");										
			ContextoImp.MoveTo(150, 700);			ContextoImp.Show("Descripcion");
			ContextoImp.MoveTo(400, 700);			ContextoImp.Show("Cant. Surtida");
			ContextoImp.MoveTo(460, 700);			ContextoImp.Show("Fecha autorizado");
			ContextoImp.MoveTo(530, 700);			ContextoImp.Show("Nota");

			filas=690;
			contador=1;
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}	
	}
}                	    	             

