///////////////////////////////////////////////////////
// created on 24/10/2007 at 05:20 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
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
// Programa		: compras.cs
// Proposito	: Modulo generral de compras y requisiciones
// Objeto		: 
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class compras_consultas
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando ventana del menu de costos
		[Widget] Gtk.Window menu_compras = null;
		[Widget] Gtk.Button button_requisicion_materiales = null;
		[Widget] Gtk.Button button_catalogo_proveedores = null;
		[Widget] Gtk.Button button_alta_productos_de_proveedores = null;
		[Widget] Gtk.Button button_ordenes_compra = null;
		[Widget] Gtk.Button button_ver_ordencompra = null;
				
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
		string connectionString;
		string nombrebd;
    	
    	//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public compras_consultas(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_)
		{
			LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
    		
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "menu_compras", null);
			gxml.Autoconnect (this);
			////// Muestra ventana de Glade
			menu_compras.Show();
			
			// movimiento total de productos
			button_requisicion_materiales.Clicked += new EventHandler(on_button_requisicion_materiales_clicked);
			
			// Creacion de Ordenes de Compra
			button_ordenes_compra.Clicked += new EventHandler(on_button_ordenes_compra_clicked);
			// Catalogo de Proveedores			
			button_catalogo_proveedores.Clicked += new EventHandler(on_button_catalogo_proveedores_clicked);
			//Catalogo de Productos de Proveedores
			button_alta_productos_de_proveedores.Clicked += new EventHandler(on_button_alta_productos_de_proveedores_clicked);
			// Ver ordenes de Compras Realizadas
			button_ver_ordencompra.Clicked += new EventHandler(on_button_ver_ordencompra_clicked);
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
		}
		
		void on_button_ordenes_compra_clicked(object sender, EventArgs args)
		{
			// ordenes_de_compras.cs
			new osiris.crea_ordenes_de_compra(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_ver_ordencompra_clicked(object sender, EventArgs args)
		{
			new osiris.consulta_ordenescompra(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_catalogo_proveedores_clicked(object sender, EventArgs args)
		{
			new osiris.catalogos_generales("proveedores",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_alta_productos_de_proveedores_clicked(object sender, EventArgs args)
		{
			new osiris.alta_productos_proveedores(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_requisicion_materiales_clicked(object sender, EventArgs args)
		{
			// centro de costo se debe enviar en el array y la clase 17   --   17
			int [] array_idtipoadmisiones = {0,1,2,3,4,10,11,12,13,14,15,16,17,18,19,100,200,205,300,400,500,600,700,710,810,820,830,930};
			new osiris.requisicion_materiales_compras(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"COMPRAS",17,"AND agrupacion IN ('ALM','IMG','LAB','MD1','NUT','OTR') ",array_idtipoadmisiones,0);
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
	
	public class consulta_ordenescompra
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando ventana de consulta de ordenes de compra
		[Widget] Gtk.Window consulta_ordenes_compra = null;
		//consulta_ordenes_compra
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
		string connectionString;
		string nombrebd;
		
		class_conexion conexion_a_DB = new class_conexion();
		public consulta_ordenescompra(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_)
		{
			LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
    		
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "consulta_ordenes_compra", null);
			gxml.Autoconnect (this);
			////// Muestra ventana de Glade
			consulta_ordenes_compra.Show();	
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			crea_treeview_ordencompra();
		}
		
		void crea_treeview_ordencompra()
		{
			
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}