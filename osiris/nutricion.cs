// created on 08/07/2008 at 05:22 p
////////////////////////////////////////////////////////////
// Sistema Hospitalario Osiris
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
//				  Daniel Olivares - arcangeldoc@gmail.com (Diseño de Pantallas Glade)
// 				  
// Licencia		: GLP
// S.O. 		: GNU/Linux
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
	public class nutricion
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando ventana principal
		[Widget] Gtk.Window menu_nutricion = null;
		[Widget] Gtk.Button button_requisicion_materiales = null;
		[Widget] Gtk.Button button_solicitud_dietas = null;
		[Widget] Gtk.Button button_surtir_dietas = null;
				
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public nutricion(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "nutricion.glade", "menu_nutricion", null);
			gxml.Autoconnect (this);
			////// Muestra ventana de Glade
			menu_nutricion.Show();
			
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_requisicion_materiales.Clicked += new EventHandler(on_button_requisicion_materiales_clicked);
			button_solicitud_dietas.Clicked += new EventHandler(on_button_solicitud_dietas_clicked);
			button_surtir_dietas.Clicked += new EventHandler(on_button_surtir_dietas_clicked);
			//button_paquetes_dietas.Clicked += new EventHandler();
		}
		
		void on_button_requisicion_materiales_clicked(object sender, EventArgs args)
		{
			int [] array_idtipoadmisiones = { 0, 3, 18 };
			new osiris.requisicion_materiales_compras(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"NUTRICION",18,"AND agrupacion = 'NUT' ",array_idtipoadmisiones,18);
		}
		
		void on_button_solicitud_dietas_clicked(object sender, EventArgs args)
		{
			new osiris.solicitudes_rx_lab(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"NUTRICION",18);
		}
		
		void on_button_surtir_dietas_clicked(object sender, EventArgs args)
		{
			new osiris.surtir_dietas();
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}