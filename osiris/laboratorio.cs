// created on 08/06/2007 at 09:32 a
////////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
//				  Daniel Olivares - arcangeldoc@gmail.com (Diseño de Pantallas Glade)
// 				  
// Licencia		: GLP
// S.O. 		: GNU/Linux Ubuntu 6.06 LTS (Dapper Drake)
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
// Proposito	: Pagos en Caja 
// Objeto		: cargos_hospitalizacion.cs
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using System.Collections;


namespace osiris
{
	public class laboratorio
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando ventana principal de Hospitalizacion
		[Widget] Gtk.Window menu_laboratorio;
		[Widget] Gtk.Button button_cargos_pacientes;
		[Widget] Gtk.Button button_requisicion_materiales;
		[Widget] Gtk.Button button_rpt_reg_pacientes;
		
		public string LoginEmpleado;
		public string NomEmpleado;
		public string AppEmpleado;
		public string ApmEmpleado;
		public string nombrebd;
		
		public laboratorio (string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = _nombrebd_;
			
			Glade.XML gxml = new Glade.XML (null, "laboratorio.glade", "menu_laboratorio", null);
			gxml.Autoconnect (this);
			////// Muestra ventana de Glade
			menu_laboratorio.Show();
			
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			button_cargos_pacientes.Clicked += new EventHandler(on_button_cargos_pacientes_clicked);
			button_requisicion_materiales.Clicked += new EventHandler(on_button_requisicion_materiales_clicked);
			button_rpt_reg_pacientes.Clicked += new EventHandler(on_button_rpt_reg_pacientes_clicked);
		}
		
		void on_button_cargos_pacientes_clicked(object sender, EventArgs args)
		{
			new osiris.cargos_laboratorio(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_requisicion_materiales_clicked(object sender, EventArgs args)
		{
			// centro de costo se debe enviar en el array y la clase 400   --   400
			int [] array_idtipoadmisiones = { 0, 400};
			new osiris.requisicion_materiales_compras(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"LABORATORIO",400,"AND agrupacion IN ('LAB','OTR','MD1','ALM') ",array_idtipoadmisiones,0);
		}
		
		void on_button_rpt_reg_pacientes_clicked(object sender, EventArgs args)
		{
			//new osiris.rep_reg_pac_labo_rx(nombrebd,"AND hscmty_grupo_producto.agrupacion = 'LAB' ","LABORATORIO");
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}