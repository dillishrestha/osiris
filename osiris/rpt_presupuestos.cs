//////////////////////////////////////////////////////////
// created on 14/02/2008 at 16:10 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Jesus Buentello G.(Programacion) 
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
// Programa		: 
// Proposito	:
// Objeto		:
/////////////////////////////////////////////////////////
using System;
using Gtk;
using Gnome;
using Npgsql;
using System.Data;
using Glade;
using System.Collections;

namespace osiris
{
	public class rpt_presupuesto
	{	
		//[Widget] Gtk.Entry entry_fecha_inicio;
		
		[Widget] Gtk.Entry entry_dia_inicio;
		[Widget] Gtk.Entry entry_mes_inicio;
		[Widget] Gtk.Entry entry_ano_inicio;
		
		//[Widget] Gtk.Entry entry_fecha_termino;
		
		[Widget] Gtk.Entry entry_dia_termino;
		[Widget] Gtk.Entry entry_mes_termino;
		[Widget] Gtk.Entry entry_ano_termino;
		
		[Widget] Gtk.HBox hbox1 = null;
				
		[Widget] Gtk.Window envio_almacenes;
		[Widget] Gtk.CheckButton checkbutton_todos_envios;
		[Widget] Gtk.TreeView lista_almacenes;
		[Widget] Gtk.Button button_buscar;
		[Widget] Gtk.Button button_rep;
		[Widget] Gtk.Button button_salir;
				
		string nombrebd;
		string query_fechas = " ";
		string rango1 = "";
		string rango2 = "";
		
		int columna = 0;
		int fila = -80;
		int contador = 1;
		int numpage = 1;
		
		string titulo = "REPORTE PRESUPUESTOS";
		
		private ListStore treeViewEnginepresupuesto;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		//Gnome.Font fuente5 = Gnome.Font.FindClosest("Luxi Sans", 5);
		Gnome.Font fuente6 = Gnome.Font.FindClosest("Luxi Sans", 6);
		Gnome.Font fuente7 = Gnome.Font.FindClosest("Luxi Sans", 7);
		//Gnome.Font fuente8 = Gnome.Font.FindClosest("Luxi Sans", 8);//Bitstream Vera Sans
		Gnome.Font fuente9 = Gnome.Font.FindClosest("Luxi Sans", 9);
		//Gnome.Font fuente10 = Gnome.Font.FindClosest("Luxi Sans", 10);
		//Gnome.Font fuente11 = Gnome.Font.FindClosest("Luxi Sans", 11);
		//Gnome.Font fuente12 = Gnome.Font.FindClosest("Luxi Sans", 12);
		//Gnome.Font fuente36 = Gnome.Font.FindClosest("Luxi Sans", 36);
		
		string connectionString;
		
		class_conexion conexion_a_DB = new class_conexion();
						
		public rpt_presupuesto(string nombrebd_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "envio_almacenes", null);
			gxml.Autoconnect (this);
			//entry_fecha_inicio.Text = DateTime.Now.ToString("yyyy-MM-dd");
			//entry_fecha_termino.Text = DateTime.Now.ToString("yyyy-MM-dd");
			
			envio_almacenes.Title = "Reporte de Presupuestos";
			
			entry_dia_inicio.Text = DateTime.Now.ToString("dd");
			entry_mes_inicio.Text = DateTime.Now.ToString("MM");
			entry_ano_inicio.Text = DateTime.Now.ToString("yyyy");
			
			entry_dia_termino.Text = DateTime.Now.ToString("dd");
			entry_mes_termino.Text = DateTime.Now.ToString("MM");
			entry_ano_termino.Text = DateTime.Now.ToString("yyyy");
			
			hbox1.Hide();
			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
            button_buscar.Clicked += new EventHandler(on_buscar_clicked);
           	button_rep.Clicked += new EventHandler(on_button_rep_clicked);
          	checkbutton_todos_envios.Clicked += new EventHandler(on_checkbutton_todos_envios);
          	crea_treeview_presupuesto();
		}
            
          void on_buscar_clicked (object sender, EventArgs args)
          {
          	if (checkbutton_todos_envios.Active == true) {
				query_fechas = " ";	 
				rango1 = "";
				rango2 = "";
			}else{
				rango1 = entry_ano_inicio.Text+"-"+entry_mes_inicio.Text+"-"+entry_dia_inicio.Text;
				rango2 = entry_ano_termino.Text+"-"+entry_mes_termino.Text+"-"+entry_dia_inicio.Text;
				
				query_fechas = " AND to_char(osiris_his_presupuestos_enca.fecha_de_creacion_presupuesto,'yyyy-MM-dd') >= '"+rango1+"' "+
								"AND to_char(osiris_his_presupuestos_enca.fecha_de_creacion_presupuesto,'yyyy-MM-dd') <= '"+rango2+"' ";
							
			}
			llenando_lista_de_presupuestos();
     	}
			
		void crea_treeview_presupuesto()
		{
			treeViewEnginepresupuesto = new ListStore(typeof(bool),//0
													typeof(string),typeof(string),typeof(string),
													typeof(string),typeof(string),typeof(string),typeof(string),
													typeof(string),typeof(string),typeof(string));//1
				
			lista_almacenes.Model = treeViewEnginepresupuesto;
			//lista_almacenes.RulesHint = true;
				
			TreeViewColumn col_seleccion = new TreeViewColumn();
			CellRendererToggle cellr0 = new CellRendererToggle();
			col_seleccion.Title = "Seleccion"; // titulo de la cabecera de la columna, si está visible
			col_seleccion.PackStart(cellr0, true);
			col_seleccion.AddAttribute (cellr0, "active", 0);
			cellr0.Activatable = true;
			cellr0.Toggled += selecciona_fila_grupo; 
		
			TreeViewColumn col_presupuesto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_presupuesto.Title = "id Presupuesto"; // titulo de la cabecera de la columna, si está visible
			col_presupuesto.PackStart(cellr1, true);
			col_presupuesto.AddAttribute (cellr1, "text", 1);
			cellr1.Foreground = "darkblue";
			
			TreeViewColumn col_cirugia = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_cirugia.Title = "Cirugia"; // titulo de la cabecera de la columna, si está visible
			col_cirugia.PackStart(cellr2, true);
			col_cirugia.AddAttribute (cellr2, "text", 2);
			cellr2.Foreground = "darkblue";
			
			TreeViewColumn col_convenido = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_convenido.Title = "$ Convenido"; // titulo de la cabecera de la columna, si está visible
			col_convenido.PackStart(cellr3, true);
			col_convenido.AddAttribute (cellr3, "text", 3);
			cellr3.Foreground = "darkblue";
			
			TreeViewColumn col_lista = new TreeViewColumn();
			CellRendererText cellr4 = new CellRendererText();
			col_lista.Title = "$ Lista"; // titulo de la cabecera de la columna, si está visible
			col_lista.PackStart(cellr4, true);
			col_lista.AddAttribute (cellr4, "text", 4);
			cellr4.Foreground = "darkblue";
			
			TreeViewColumn col_precio = new TreeViewColumn();
			CellRendererText cellr5 = new CellRendererText();
			col_precio.Title = "$ Deposito minimo"; // titulo de la cabecera de la columna, si está visible
			col_precio.PackStart(cellr5, true);
			col_precio.AddAttribute (cellr5, "text", 5);
			cellr5.Foreground = "darkblue";
			
			TreeViewColumn col_envio = new TreeViewColumn();
			CellRendererText cellr6 = new CellRendererText();
			col_envio.Title = "Quien creo"; // titulo de la cabecera de la columna, si está visible
			col_envio.PackStart(cellr6, true);
			col_envio.AddAttribute (cellr6, "text", 6);
			cellr6.Foreground = "darkblue";
			
			TreeViewColumn col_deposito = new TreeViewColumn();
			CellRendererText cellr7 = new CellRendererText();
			col_deposito.Title = "Fecha"; // titulo de la cabecera de la columna, si está visible
			col_deposito.PackStart(cellr7, true);
			col_deposito.AddAttribute (cellr7, "text", 7);
			cellr7.Foreground = "darkblue";
			
			TreeViewColumn col_internado = new TreeViewColumn();
			CellRendererText cellr8 = new CellRendererText();
			col_internado.Title = "Internamiento"; // titulo de la cabecera de la columna, si está visible
			col_internado.PackStart(cellr8, true);
			col_internado.AddAttribute (cellr8, "text", 8);
			cellr8.Foreground = "darkblue";
			
			TreeViewColumn col_id_medico = new TreeViewColumn();
			CellRendererText cellr9 = new CellRendererText();
			col_id_medico.Title = "Id_medico"; // titulo de la cabecera de la columna, si está visible
			col_id_medico.PackStart(cellr9, true);
			col_id_medico.AddAttribute (cellr9, "text", 9);
			cellr9.Foreground = "darkblue";
			
			TreeViewColumn col_tel_medico = new TreeViewColumn();
			CellRendererText cellr10 = new CellRendererText();
			col_tel_medico.Title = "Tel.Medico"; // titulo de la cabecera de la columna, si está visible
			col_tel_medico.PackStart(cellr10, true);
			col_tel_medico.AddAttribute (cellr10, "text", 10);
			cellr10.Foreground = "darkblue";
			
				lista_almacenes.AppendColumn(col_seleccion);
				lista_almacenes.AppendColumn(col_presupuesto);
				lista_almacenes.AppendColumn(col_cirugia);
				lista_almacenes.AppendColumn(col_convenido);
				lista_almacenes.AppendColumn(col_lista);
				lista_almacenes.AppendColumn(col_precio);
				lista_almacenes.AppendColumn(col_envio);
				lista_almacenes.AppendColumn(col_deposito);
				lista_almacenes.AppendColumn(col_internado);
				lista_almacenes.AppendColumn(col_id_medico);
				lista_almacenes.AppendColumn(col_tel_medico);
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
				if (treeViewEnginepresupuesto.GetIterFirst (out iter2)){
					lista_almacenes.Model.SetValue(iter2,0,true);
					while (treeViewEnginepresupuesto.IterNext(ref iter2)){
						lista_almacenes.Model.SetValue(iter2,0,true);
					}
				}
			}else{
				TreeIter iter2;
				if (treeViewEnginepresupuesto.GetIterFirst (out iter2)){
					lista_almacenes.Model.SetValue(iter2,0,false);
					while (treeViewEnginepresupuesto.IterNext(ref iter2)){
						lista_almacenes.Model.SetValue(iter2,0,false);
					}
				}
			}
		}
           
       	void llenando_lista_de_presupuestos()
		{
			this.treeViewEnginepresupuesto.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
					comando.CommandText = "SELECT to_char(osiris_his_presupuestos_enca.id_presupuesto,'999999999') AS idpresupuesto,"+
								"to_char(osiris_his_presupuestos_enca.id_tipo_cirugia,'999999999') AS idcirugia,"+ 
								"osiris_his_presupuestos_enca.id_quien_creo,"+
								"to_char(osiris_his_presupuestos_enca.precio_convenido,'999999999.99') AS precioconvenido,"+
								"to_char(osiris_his_presupuestos_enca.fecha_de_creacion_presupuesto,'yyyy-MM-dd') AS fechacreacion,"+
								"to_char(osiris_his_presupuestos_enca.total_presupuesto,'999999999.99') AS total,"+ 
								"osiris_his_tipo_cirugias.descripcion_cirugia,"+
								"osiris_his_tipo_cirugias.id_tipo_cirugia,"+
								"to_char(osiris_his_presupuestos_enca.deposito_minimo,'999999999.99') AS depositominimo,"+
								"to_char(osiris_his_presupuestos_enca.dias_internamiento,'999999999') AS dias,"+
								"to_char(osiris_his_presupuestos_enca.id_medico,'999999999') AS idmedico,"+
								"osiris_his_presupuestos_enca.telefono_medico "+
								"FROM osiris_his_presupuestos_enca,osiris_his_tipo_cirugias "+
								"WHERE osiris_his_tipo_cirugias.id_tipo_cirugia = osiris_his_presupuestos_enca.id_tipo_cirugia "+
								""+query_fechas+" ";
								
				Console.WriteLine(comando.CommandText);
			
				NpgsqlDataReader lector = comando.ExecuteReader ();

				while (lector.Read())
				{							
					treeViewEnginepresupuesto.AppendValues (false, 
													(string) lector["idpresupuesto"],//0
													(string) lector["descripcion_cirugia"],
													(string) lector["precioconvenido"],//1
													(string) lector["total"],//2
													(string) lector["depositominimo"],
													(string) lector["id_quien_creo"],
													(string) lector["fechacreacion"],//4
													(string) lector["dias"],
													(string) lector["idmedico"],
													(string) lector["telefono_medico"]);//5
													
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}    
            
       void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
      		// Cambiar la fuente
      		Gnome.Print.Setfont(ContextoImp,fuente9);								
			ContextoImp.MoveTo(230, -60);			ContextoImp.Show("REPORTE PRESUPUESTO GLOBAL");	
			Gnome.Print.Setfont(ContextoImp,fuente6);
			
			ContextoImp.MoveTo(69.7,-30);			ContextoImp.Show("Sistema Hospitalario OSIRIS");//19.7, 770
			ContextoImp.MoveTo(70, -30);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(69.7, -40);			ContextoImp.Show("Direccion: ");
			ContextoImp.MoveTo(70, -40);			ContextoImp.Show("Direccion: ");
			ContextoImp.MoveTo(69.7, -50);			ContextoImp.Show("Conmutador: ");
			ContextoImp.MoveTo(70, -50);			ContextoImp.Show("Conmutador: ");
									
			Gnome.Print.Setfont(ContextoImp,fuente6);							
			ContextoImp.MoveTo(65, -70);			ContextoImp.Show("Id");
			ContextoImp.MoveTo(80, -70);			ContextoImp.Show("Fecha");
			ContextoImp.MoveTo(120, -70);			ContextoImp.Show("Cirugia");
			ContextoImp.MoveTo(300, -70);			ContextoImp.Show("Dias");
			ContextoImp.MoveTo(355, -70);			ContextoImp.Show("$Total");
			ContextoImp.MoveTo(390, -70);			ContextoImp.Show("$Convenido");
			ContextoImp.MoveTo(430, -70);			ContextoImp.Show("Deposito min");
			ContextoImp.MoveTo(500, -70);			ContextoImp.Show("Medico");
			//ContextoImp.MoveTo(650, -70);			ContextoImp.Show("Med Provisional");						
			
			Gnome.Print.Setfont(ContextoImp,fuente7);
			ContextoImp.MoveTo(390, -50);			ContextoImp.Show("PAGINA "+numpage+"  Fecha Impresion: "+DateTime.Now.ToString("dd-MM-yyyy"));
			
		}    
            
        void on_button_rep_clicked(object sender, EventArgs args)
		{ 
			contador = 1;	
	        fila = -80;
			if (checkbutton_todos_envios.Active == true) {
				query_fechas = " ";	 
				rango1 = "";
				rango2 = "";
			}else{
				rango1 = entry_ano_inicio.Text+"-"+entry_mes_inicio.Text+"-"+entry_dia_inicio.Text;
				rango2 = entry_ano_termino.Text+"-"+entry_mes_termino.Text+"-"+entry_dia_termino.Text;
				query_fechas = " AND to_char(osiris_his_presupuestos_enca.fecha_de_creacion_presupuesto,'yyyy-MM-dd') >= '"+rango1+"' "+
								"AND to_char(osiris_his_presupuestos_enca.fecha_de_creacion_presupuesto,'yyyy-MM-dd') <= '"+rango2+"' ";
							

			}
			
			titulo = "REPORTE PRESUPUESTOS";
					
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
		
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{  
			TreeIter iter;	
			 
			string numeros_seleccionado = ""; 
			string variable_paso_03 = "";
			int variable_paso_02_1 = 0;
			string fechaautorizacion = "";
			
			//poder elegir una fila del treeview
			if (treeViewEnginepresupuesto.GetIterFirst (out iter)){
			
 				if ((bool) lista_almacenes.Model.GetValue (iter,0) == true){
 					numeros_seleccionado = (string) lista_almacenes.Model.GetValue (iter,1);
 					variable_paso_02_1 += 1;		
 				}
 				while (treeViewEnginepresupuesto.IterNext(ref iter)){
 					if ((bool) lista_almacenes.Model.GetValue (iter,0) == true){
 				    	if (variable_paso_02_1 == 0){ 				    	
 							numeros_seleccionado = (string) lista_almacenes.Model.GetValue (iter,1);
 							variable_paso_02_1 += 1;
 						}else{
 							variable_paso_03 = (string) lista_almacenes.Model.GetValue (iter,1);
 							numeros_seleccionado = numeros_seleccionado.Trim() + "','" + variable_paso_03.Trim();
 						}
 					}
 				}
 			}
 			string query_in_num = "";
 			
 			if (variable_paso_02_1 > 0){
	 				query_in_num = " AND osiris_his_presupuestos_enca.id_presupuesto IN ('"+numeros_seleccionado+"') ";  
			}			
		
	        NpgsqlConnection conexion; 
	        conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de dato s este conectada
	        try{
		 		conexion.Open ();
		       	NpgsqlCommand comando; 
		       	comando = conexion.CreateCommand (); 

		        		       		comando.CommandText ="SELECT to_char(osiris_his_presupuestos_enca.id_presupuesto,'999999999') AS idpresupuesto,"+
								"to_char(osiris_his_presupuestos_enca.id_tipo_cirugia,'999999999') AS idcirugia,"+ 
								"osiris_his_presupuestos_enca.id_quien_creo,"+
								"to_char(osiris_his_presupuestos_enca.precio_convenido,'999999999.99') AS precioconvenido,"+
								"to_char(osiris_his_presupuestos_enca.fecha_de_creacion_presupuesto,'yyyy-MM-dd') AS fechacreacion,"+
								"to_char(osiris_his_presupuestos_enca.total_presupuesto,'999999999.99') AS total,"+ 
								"osiris_his_presupuestos_enca.notas,"+
								"osiris_his_tipo_cirugias.descripcion_cirugia,"+
								"osiris_his_tipo_cirugias.id_tipo_cirugia,"+
								"osiris_his_presupuestos_enca.notas,"+
								"to_char(osiris_his_presupuestos_enca.deposito_minimo,'999999999.99') AS depositominimo,"+
								"to_char(osiris_his_presupuestos_enca.dias_internamiento,'999999999') AS dias,"+
								"to_char(osiris_his_presupuestos_enca.id_medico,'999999999') AS idmedico,"+
								"osiris_his_presupuestos_enca.medico_provisional,"+
								"osiris_his_medicos.id_medico,"+
								"osiris_his_medicos.nombre_medico,"+
								"osiris_his_presupuestos_enca.telefono_medico,"+
								"osiris_empleado.nombre1_empleado || ' ' || "+ 
								"osiris_empleado.nombre2_empleado || ' ' || "+ 
								"osiris_empleado.apellido_paterno_empleado || ' ' || "+ 
								"osiris_empleado.apellido_materno_empleado AS nombreempl "+
								"FROM osiris_his_presupuestos_enca,osiris_his_tipo_cirugias,osiris_empleado,osiris_his_medicos "+
								"WHERE osiris_his_tipo_cirugias.id_tipo_cirugia = osiris_his_presupuestos_enca.id_tipo_cirugia "+
								"AND osiris_empleado.login_empleado = osiris_his_presupuestos_enca.id_quien_creo "+
								"AND osiris_his_medicos.id_medico = osiris_his_presupuestos_enca.id_medico "+
								query_in_num+ " "+
								query_fechas+" "+
								"ORDER BY osiris_his_presupuestos_enca.id_presupuesto";
									
			        	//Console.WriteLine(comando.CommandText);
			        	NpgsqlDataReader lector = comando.ExecuteReader ();
			        	ContextoImp.BeginPage("Pagina 1");
						ContextoImp.Rotate(90);
		        		imprime_encabezado(ContextoImp,trabajoImpresion);
			       string toma_descrip_cir;
			       
			       if (lector.Read()){
			        toma_descrip_cir = (string) lector["idpresupuesto"];
			   		ContextoImp.MoveTo(65, fila);			ContextoImp.Show(toma_descrip_cir.Trim());
			   			
					ContextoImp.MoveTo(80, fila);			ContextoImp.Show((string) lector["fechacreacion"]);
					
					toma_descrip_cir = (string) lector["descripcion_cirugia"];
					if(toma_descrip_cir.Length > 40){
							toma_descrip_cir = toma_descrip_cir.Substring(0,39);
					}  		  							  				  				  				
					ContextoImp.MoveTo(120, fila);			ContextoImp.Show(toma_descrip_cir); 	
						
					ContextoImp.MoveTo(290, fila);			ContextoImp.Show((string) lector["dias"]); 		
					ContextoImp.MoveTo(340, fila);			ContextoImp.Show((string) lector["total"]); 		
					ContextoImp.MoveTo(380, fila);			ContextoImp.Show((string) lector["precioconvenido"]); 
					ContextoImp.MoveTo(420, fila);			ContextoImp.Show((string) lector["depositominimo"]); 	
					
					if((int) lector["id_medico"] == 1){
					ContextoImp.MoveTo(480, fila);			ContextoImp.Show((string) lector["medico_provisional"]);
					 
					}else{
					ContextoImp.MoveTo(480, fila);			ContextoImp.Show((string) lector["nombre_medico"]);
						}	
					if((string) lector["notas"] == ""){
					
					}else{
					fila-=10;
					contador+=1;
			        salto_pagina(ContextoImp,trabajoImpresion);	
					ContextoImp.MoveTo(65, fila);			ContextoImp.Show("Notas");
					ContextoImp.MoveTo(90, fila);			ContextoImp.Show((string) lector["notas"]);
					fila-=10;
					contador+=1;
			        salto_pagina(ContextoImp,trabajoImpresion);		
						}		    
					fila-=10;
					contador+=1;
			        salto_pagina(ContextoImp,trabajoImpresion);	
			   while (lector.Read()){
					 toma_descrip_cir = (string) lector["idpresupuesto"];
			   		ContextoImp.MoveTo(65, fila);			ContextoImp.Show(toma_descrip_cir.Trim());
			   			
					ContextoImp.MoveTo(80, fila);			ContextoImp.Show((string) lector["fechacreacion"]);
					
					toma_descrip_cir = (string) lector["descripcion_cirugia"];
					if(toma_descrip_cir.Length > 40){
							toma_descrip_cir = toma_descrip_cir.Substring(0,39);
					}  		  							  				  				  				
					ContextoImp.MoveTo(120, fila);			ContextoImp.Show(toma_descrip_cir); 	
						
					ContextoImp.MoveTo(290, fila);			ContextoImp.Show((string) lector["dias"]); 		
					ContextoImp.MoveTo(340, fila);			ContextoImp.Show((string) lector["total"]); 		
					ContextoImp.MoveTo(380, fila);			ContextoImp.Show((string) lector["precioconvenido"]); 
					ContextoImp.MoveTo(420, fila);			ContextoImp.Show((string) lector["depositominimo"]); 	
					
					if((int) lector["id_medico"] == 1){
					ContextoImp.MoveTo(480, fila);			ContextoImp.Show((string) lector["medico_provisional"]);

					}else{
					ContextoImp.MoveTo(480, fila);			ContextoImp.Show((string) lector["nombre_medico"]);
						}	
					if((string) lector["notas"] == ""){
					
					}else{
					fila-=10;
					contador+=1;
			        salto_pagina(ContextoImp,trabajoImpresion);	
					ContextoImp.MoveTo(65, fila);			ContextoImp.Show("Notas");
					ContextoImp.MoveTo(90, fila);			ContextoImp.Show((string) lector["notas"]);
					fila-=10;
					contador+=1;
			        salto_pagina(ContextoImp,trabajoImpresion);		
						}		
					fila-=10;
					contador+=1;
			        salto_pagina(ContextoImp,trabajoImpresion);		
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
        void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
		//Console.WriteLine("contador antes del if: "+contador.ToString());
			if (contador > 45 ){
	        	numpage +=1;        	contador=1;	
	        	fila=-80;
	        	ContextoImp.ShowPage();
				ContextoImp.BeginPage("Pagina "+numpage.ToString());
				ContextoImp.Rotate(90);
				imprime_encabezado(ContextoImp,trabajoImpresion);
	     	}
	     }
        
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}	
	}
}