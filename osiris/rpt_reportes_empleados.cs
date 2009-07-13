////////////////////////////////////////////////////////////////////////////////////////////////////
// created on 1/04/2008                                                                           
// Hospital Santa Cecilia                                                                                            
// Monterrey - Mexico                                                                                                
//                                                                                                                            
// Autor    	: Erick Eduardo Gonzalez Reyes (Programation & Glade's window)            
//                                                                                                       
// Licencia		: GLP                                                                                                  
////////////////////////////////////////////////////////////////////////////////////////////////////
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
/////////////////////////////////////////////////////////////////////////////////////////////////////

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
	
	public class reportes_empleados
	{
	
	[Widget] Gtk.Window reportes_de_empleados;
	[Widget] Gtk.Button button_salir;
	[Widget] Gtk.Button button_contrato;
	[Widget] Gtk.Button button_bajas;
	[Widget] Gtk.TreeView treeview_reportelista;
	[Widget] Gtk.Entry entry_dia_inicio;
	[Widget] Gtk.Entry entry_mes_inicio;
	[Widget] Gtk.Entry entry_anno_inicio;
	[Widget] Gtk.Entry entry_dia_fin;
	[Widget] Gtk.Entry entry_mes_fin;
	[Widget] Gtk.Entry entry_anno_fin;
	[Widget] Gtk.ComboBox combo_tipocontrato;
	
	
	
	public string connectionString = "Server=192.168.1.4;" +
						"Port=5432;" +
						"User ID=admin1;" +
						"Password=1qaz2wsx;";
						
		public string nombrebd;
		public string LoginEmpleado;
    	public string NomEmpleado;
    	public string AppEmpleado;
    	public string ApmEmpleado;
        public string fechamax;
        public string fechamin;
        public string id_empleado;
        public string var_fecha;
        public string var_tipo;
        public string var_id_empleado;
        public string var_sueldo;
        public string var_depto;
        public string var_puesto;
        public string var_tipofuncion;
        public string var_horas;
        public string var_tiempocomida;
        public string var_tipopago;
        public string var_jornada;
        
        public int conteo_lineas = 0;
        //public bool leerhistorial = false;
        public string tiempo_de_contrato_short="";
        
    	private TreeStore treeViewEngineBusca;
    	
    	protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public reportes_empleados(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_,string tipo_reporte_)
		{
			LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		nombrebd = _nombrebd_; 
    		
    		
			Glade.XML gxml = new Glade.XML (null, "recursos_humanos.glade", "reportes_de_empleados",null);
			gxml.Autoconnect (this);
	        reportes_de_empleados.Show();
			
			button_contrato.Clicked += new EventHandler(on_contrato_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);	
			
			llenacombo_contrato();
			
			if (tipo_reporte_ == "Contrato")
			{crea_treeview_busqueda_empleado();
			this.button_bajas.Visible = false;
			this.button_contrato.Visible = false;
			}
		    
		    
		    this.entry_dia_inicio.Text = DateTime.Now.Day.ToString();
		    this.entry_mes_inicio.Text = DateTime.Now.Month.ToString();
		    this.entry_anno_inicio.Text = DateTime.Now.Year.ToString();
		     
		    this.entry_dia_fin.Text = DateTime.Now.Day.ToString();
		    this.entry_mes_fin.Text = DateTime.Now.Month.ToString();
		    this.entry_anno_fin.Text = DateTime.Now.Year.ToString(); 
		     
		}
		
		void llenacombo_contrato()
		{
		combo_tipocontrato.Clear();
		CellRendererText cell = new CellRendererText();
			combo_tipocontrato.PackStart(cell, true);
			combo_tipocontrato.AddAttribute(cell,"text",0);
			
			ListStore store = new ListStore( typeof (string));
			combo_tipocontrato.Model = store;
		    
		    store.AppendValues ((string) "");
            store.AppendValues ((string) "DETERMINADO (1 MES)");
	        store.AppendValues ((string) "DETERMINADO (2 MESES)");
	        store.AppendValues ((string) "DETERMINADO (3 MESES)");
	        store.AppendValues ((string) "INDETERMINADO");
	        store.AppendValues ((string) "HONORARIOS (ASIMILABLES)");
	        store.AppendValues ((string) "PRACTICAS");
	        
			TreeIter iter;
			if (store.GetIterFirst(out iter)){
				combo_tipocontrato.SetActiveIter (iter);
			 }
			combo_tipocontrato.Changed += new EventHandler (onComboBoxChanged_tipocontrato);
		   
		}
		
		void onComboBoxChanged_tipocontrato (object sender, EventArgs args)
		{
			string fechas="";
			ComboBox combo_tipocontrato = sender as ComboBox;
		    if (sender == null) {	return;	}
			TreeIter iter;
			if (combo_tipocontrato.GetActiveIter (out iter)) {
				tiempo_de_contrato_short = ((string) this.combo_tipocontrato.Model.GetValue(iter,0));
				llena_lista_contrato();
						
                  }
            }
		
		
		void on_contrato_clicked (object sender, EventArgs args)
		{
		
		fechamin =  Convert.ToDateTime(this.entry_dia_inicio.Text+"/"+this.entry_mes_inicio.Text+"/"+this.entry_anno_inicio.Text).ToShortDateString();
		fechamax =  Convert.ToDateTime(this.entry_dia_fin.Text+"/"+this.entry_mes_fin.Text+"/"+this.entry_anno_fin.Text).ToShortDateString();
		
		Console.WriteLine(Convert.ToDateTime(fechamin).ToLongDateString());
		Console.WriteLine(Convert.ToDateTime(fechamax).ToLongDateString());
		
		if (Convert.ToDateTime(fechamin) > Convert.ToDateTime(fechamax))
		 {  	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,
				ButtonsType.Close,"La fecha de inicio debe de ser menor a la fecha final");
				msgBoxError.Run ();
				msgBoxError.Destroy();}
		  else
		  {llena_lista_contrato();}
	      }
				
		void crea_treeview_busqueda_empleado()
		{
		    
				    
			treeViewEngineBusca = new TreeStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string));
			treeview_reportelista.Model = treeViewEngineBusca;
			
			treeview_reportelista.RulesHint = true;
			
			//treeview_reportelista.RowActivated += on_selecciona_empleado_clicked;  // Doble click selecciono paciente*/
			
			TreeViewColumn col_Empleado = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_Empleado.Title = "Empleado"; // titulo de la cabecera de la columna, si está visible
			col_Empleado.PackStart(cellr0, true);
			col_Empleado.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
			col_Empleado.SortColumnId = (int) Column.col_Empleado;
			//cellr0.Editable = true;   // Permite edita este campo
            
			TreeViewColumn col_fecha = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_fecha.Title = "Fecha de contrato";
			col_fecha.PackStart(cellrt1, true);
			col_fecha.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 1 en vez de 2
			col_fecha.SortColumnId = (int) Column.col_fecha;
            
			TreeViewColumn col_tipo = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_tipo.Title = "Tipo de contrato";
			col_tipo.PackStart(cellrt2, true);
			col_tipo.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 2 en vez de 3
			col_tipo.SortColumnId = (int) Column.col_tipo;
            
            TreeViewColumn col_sueldo = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_sueldo.Title = "Sueldo";
			col_sueldo.PackStart(cellrt3, true);
			col_sueldo.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 2 en vez de 3
			col_sueldo.SortColumnId = (int) Column.col_sueldo;
			
			TreeViewColumn col_depto = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_depto.Title = "Departamento";
			col_depto.PackStart(cellrt4, true);
			col_depto.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 2 en vez de 3
			col_depto.SortColumnId = (int) Column.col_depto;
			
            TreeViewColumn col_puesto = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_puesto.Title = "Puesto";
			col_puesto.PackStart(cellrt5, true);
			col_puesto.AddAttribute (cellrt5, "text", 5); // la siguiente columna será 2 en vez de 3
			col_puesto.SortColumnId = (int) Column.col_puesto;
            
            TreeViewColumn col_jornada = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_jornada.Title = "Jornada";
			col_jornada.PackStart(cellrt6, true);
			col_jornada.AddAttribute (cellrt6, "text", 6); // la siguiente columna será 2 en vez de 3
			col_jornada.SortColumnId = (int) Column.col_jornada;
            
            TreeViewColumn col_tipofuncion = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_tipofuncion.Title = "Tipo de Función";
			col_tipofuncion.PackStart(cellrt7, true);
			col_tipofuncion.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 2 en vez de 3
			col_tipofuncion.SortColumnId = (int) Column.col_tipofuncion;
            
            TreeViewColumn col_tipopago = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_tipopago.Title = "Tipo de Pago";
			col_tipopago.PackStart(cellrt8, true);
			col_tipopago.AddAttribute (cellrt8, "text", 8); // la siguiente columna será 2 en vez de 3
			col_tipopago.SortColumnId = (int) Column.col_tipopago;
            
			treeview_reportelista.AppendColumn(col_Empleado);
			treeview_reportelista.AppendColumn(col_fecha);
			treeview_reportelista.AppendColumn(col_tipo);
			treeview_reportelista.AppendColumn(col_sueldo);
	        treeview_reportelista.AppendColumn(col_depto);
	        treeview_reportelista.AppendColumn(col_puesto);
	        treeview_reportelista.AppendColumn(col_jornada); 
	        treeview_reportelista.AppendColumn(col_tipofuncion);
	        treeview_reportelista.AppendColumn(col_tipopago);
		}
		
		enum Column
		{
			col_Empleado,
			col_fecha,
			col_tipo,
			col_sueldo,
			col_puesto,
			col_depto,
			col_jornada,
			col_tipofuncion,
			col_tipopago,
		}
		
		
		void llena_lista_contrato()
		{
		
		   fechamin =  Convert.ToDateTime(this.entry_dia_inicio.Text+"/"+this.entry_mes_inicio.Text+"/"+this.entry_anno_inicio.Text).ToShortDateString();
		   fechamax =  Convert.ToDateTime(this.entry_dia_fin.Text+"/"+this.entry_mes_fin.Text+"/"+this.entry_anno_fin.Text).ToShortDateString();
		
			treeViewEngineBusca.Clear();
						
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
             comando.CommandText = "SELECT id_empleado, historial_de_contrato "+
									  "FROM hscmty_empleado " +
									 "WHERE ( historial_de_contrato != '');";
									 // "WHERE (id_empleado != '0');";
									  
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				while (lector.Read())
				   {
				    char[] delimiterChars = {';'};  // delimitador de Cadenas
			        string text = ((string) lector["historial_de_contrato"]); //consulta historial de contrato
			        string[] words = text.Split(delimiterChars);  // Separa las Cadenas
			        conteo_lineas = 0;
			        id_empleado = (string) lector["id_empleado"];
			        foreach (string s in words){
			        
			        Console.WriteLine (conteo_lineas);
			        
			        if (s.Length > 0) //comprueba si hay historial de contrato "s" tiene historial
			        {    
			        conteo_lineas = conteo_lineas + 1; //suma los renglones en cada linea
			              
			              
			              switch (conteo_lineas)
                              {
                               case 1: 
                                  //registro
                                 Console.WriteLine(s);
                               break;
                               case 2:
                                 //fecha de registro
                                 Console.WriteLine(s);
                               break;
                               case 3:
                                 var_fecha =  Convert.ToDateTime(s).ToLongDateString();
                               break;
                               case 4:
                                  var_tipo = s;
                               break;
                               case 5:
                                   var_sueldo = s.Trim();
                               break;
                               case 6:
                                   var_depto = s;
                               break;
                               case 7:
                                   var_puesto = s;
                               break;
                               case 8:
                                   var_jornada = s;
                               break;
                               case 9:
                                   var_tipofuncion = s;
                               break;
                               case 10:
                                   var_tipopago = s;
                               break;
                               case 11:
                                   var_tiempocomida = s;
                               break;
                               case 12:
                                   // numero de locker
                                   treeViewEngineBusca.AppendValues (id_empleado, var_fecha, var_tipo,var_sueldo ,var_depto,var_puesto,var_jornada,var_tipofuncion, var_tipopago);
                                   conteo_lineas = 0;
                               break;
                               default:
                                    //Console.WriteLine("Default case");
                               break;
                                }
			        
			        
			           } // X Cierra el if s > 0.length
			        
			        } // X Cierra el foreach
				//conteo_lineas = 0;
				}   // X Cierra el while del lector
			
			}  // X Cierra el Try de la conexion
			
			catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();	msgBoxError.Destroy();
            } // X Cierra el catch
            conexion.Close ();
		
					} // X Cierra el void				  
			
/*
   void dividir_cadena()
    {
        char[] delimiterChars = { ':', ';', '-', '/', '\n' };
        string text = "one\ttwo three:four,five six seven";
        string[] words = text.Split(delimiterChars);
        System.Console.WriteLine("{0} words in text:", words.Length);

        foreach (string s in words)
        {
            System.Console.WriteLine(s);
        }
    }
*/
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
	}
	
}
