// created on 16/08/2010 at 06:10 p
//////////////////////////////////////////////////////////////////////
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. (Programacion) arcangeldoc@gmail.com
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

	public class solicitudes_rx_lab
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		[Widget] Gtk.Window solicites_examenes_labrx = null;
		[Widget] Gtk.Button button_consultar = null;
 		[Widget] Gtk.RadioButton radiobutton_estud_carg = null;
		[Widget] Gtk.RadioButton radiobutton_estud_solic = null;
		[Widget] Gtk.Notebook notebook1 = null;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		class_conexion conexion_a_DB = new class_conexion();
			
		public solicitudes_rx_lab(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_)
		{			
			Glade.XML gxml = new Glade.XML (null, "imagenologia.glade", "solicites_examenes_labrx", null);
			gxml.Autoconnect (this);	        
			// Muestra ventana de Glade
			solicites_examenes_labrx.Show();
			
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = conexion_a_DB._nombrebd;
			
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			radiobutton_estud_carg.Clicked += new EventHandler(on_changetab_clicked);
			radiobutton_estud_solic.Clicked += new EventHandler(on_changetab_clicked);
			button_consultar.Clicked += new EventHandler(on_button_consultar_clicked);
		}
		
		void on_button_consultar_clicked(object sender, EventArgs args)
		{
			if(radiobutton_estud_carg.Active == true){
				notebook1.CurrentPage = 1;
			}
			if(radiobutton_estud_solic.Active == true){
				notebook1.CurrentPage = 0;
			}
		}
		
		void on_changetab_clicked(object sender, EventArgs args)
		{
			//Console.WriteLine(radiobutton_seltab.Active.ToString());
			Gtk.RadioButton radiobutton_seltab = (Gtk.RadioButton) sender;
			
			if(radiobutton_seltab.Name.ToString() ==  "radiobutton_estud_carg"){
				notebook1.CurrentPage = 1;
			}
			if(radiobutton_seltab.Name.ToString() ==  "radiobutton_estud_solic"){
				notebook1.CurrentPage = 0;
			}
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}
