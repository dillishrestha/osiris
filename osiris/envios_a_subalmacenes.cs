///////////////////////////////////////////////////////
// created on 31/01/2010 at 10:00 am
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Pre-Programacion y Ajustes)
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
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class envios_a_subalmacenes
	{
		// Boton general para salir de las ventanas
		[Widget] Gtk.Button button_salir = null;
		[Widget] Gtk.Button button_buscar_busqueda = null;
		[Widget] Gtk.Entry entry_expresion = null;
		[Widget] Gtk.Button button_selecciona = null;
		
		// Declarando ventana para ver productos enviados a los sub-almacenes
		[Widget] Gtk.Window mov_productos;
		//Fechas:
	    [Widget] Gtk.Entry entry_dia1;                     
	    [Widget] Gtk.Entry entry_mes1;
	    [Widget] Gtk.Entry entry_ano1;
	    [Widget] Gtk.Entry entry_dia2;
	    [Widget] Gtk.Entry entry_mes2;
	    [Widget] Gtk.Entry entry_ano2;
		// Combobox:
		[Widget] Gtk.ComboBox combobox_departamentos;
		
		string connectionString;						
		string nombrebd;
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
		
		//Declaracion de ventana de error:
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public envios_a_subalmacenes (string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_ )
		{
			LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd_ = conexion_a_DB._nombrebd;	
    		
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "mov_productos", null);
			gxml.Autoconnect (this);
	        // Muestra ventana de Glade:
			mov_productos.Show();
			entry_dia1.Text = DateTime.Now.ToString("dd");
			entry_mes1.Text = DateTime.Now.ToString("MM");
			entry_ano1.Text = DateTime.Now.ToString("yyyy");				
			entry_dia2.Text = DateTime.Now.ToString("dd");
			entry_mes2.Text = DateTime.Now.ToString("MM");
			entry_ano2.Text = DateTime.Now.ToString("yyyy");
			
			//  Sale de la ventana:
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			llenado_combobox_subalmacenes();
		}
		
		void llenado_combobox_subalmacenes()
		{
			// Llenado de combobox con los tipos de departamentos:
			combobox_departamentos.Clear();
			CellRendererText cell1 = new CellRendererText();
			combobox_departamentos.PackStart(cell1, true);
			combobox_departamentos.AddAttribute(cell1,"text",0);
			
			ListStore store1 = new ListStore( typeof (string), typeof (int));
			combobox_departamentos.Model = store1;
			this.combobox_departamentos.Changed += new EventHandler (onComboBoxChanged_departamentos);
	        
	      	NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada:
            try{
				conexion.Open ();   
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
		        comando.CommandText = "SELECT * FROM osiris_his_tipo_admisiones "+
		               						"WHERE cuenta_mayor = 4000 "+
		               						"ORDER BY descripcion_admisiones;";
						
				NpgsqlDataReader lector = comando.ExecuteReader ();
				store1.AppendValues ("", 0);
		        while (lector.Read()){
					store1.AppendValues ((string) lector["descripcion_admisiones"], (int) lector["id_tipo_admisiones"]);
				}				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void onComboBoxChanged_departamentos(object sender, EventArgs args)
		{
    		ComboBox combobox_departamentos = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_departamentos.GetActiveIter (out iter)){
		    	//id_tipo_admisiones = (int) combobox_departamentos.Model.GetValue(iter,1);
		    	//query_departamento = " AND osiris_erp_cobros_deta.id_tipo_admisiones = '"+Convert.ToString((int) combobox_departamentos.Model.GetValue(iter,1)).ToString()+"' ";		    			    	
		    	//if (this.checkbutton_todos_departamentos .Active == true){
				//	query_departamento = " ";
				//}
	     	}
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}
