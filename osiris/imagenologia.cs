// created on 08/06/2007 at 10:30 a
////////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
//				  Daniel Olivares - arcangeldoc@gmail.com (Diseño de Pantallas Glade)
// 				  
// Licencia		: GLP
// S.O. 		:
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
	public class imagenologia
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando ventana principal de imagenologia
		[Widget] Gtk.Window menu_imagenologia;
		[Widget] Gtk.Button button_cargos_pacientes;
		[Widget] Gtk.Button button_soli_material;
		[Widget] Gtk.Button button_reporte_imagenologia;
		[Widget] Gtk.Button button_solicitud_examenes;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;		
		class_conexion conexion_a_DB = new class_conexion();
		
		public imagenologia (string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = conexion_a_DB._nombrebd;			
			
			Glade.XML gxml = new Glade.XML (null, "imagenologia.glade", "menu_imagenologia", null);
			gxml.Autoconnect (this);
			////// Muestra ventana de Glade
			menu_imagenologia.Show();
			
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			button_cargos_pacientes.Clicked += new EventHandler(on_button_cargos_pacientes_clicked);
			button_soli_material.Clicked += new EventHandler(on_button_soli_material_clicked);
			button_reporte_imagenologia.Clicked += new EventHandler(on_button_reporte_imagenologia_clicked);
			button_solicitud_examenes.Clicked += new EventHandler(on_button_solicitud_examenes_clicked);
		}
		
		void on_button_cargos_pacientes_clicked(object sender, EventArgs args)
		{
			new osiris.cargos_imagenologia_b(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_soli_material_clicked(object sender, EventArgs args)
		{
			//new osiris.solicitud_material_hospital(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_reporte_imagenologia_clicked(object sender, EventArgs args)
		{
			new osiris.rep_reg_pac_labo_rx(nombrebd,"AND osiris_grupo_producto.agrupacion = 'IMG' ","IMAGENOLOGIA");
		}
		
		void on_button_solicitud_examenes_clicked(object sender, EventArgs args)
		{
			new osiris.solicitudes_rx_lab(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"IMAGENOLOGIA - RAYOS X",300);
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}