// created on 26/07/2007 at 03:51 p
////////////////////////////////////////////////////////////
// Sistema Hospitalario OSIRIS
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

namespace osiris
{
	public class almacen
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		// Declarando ventana principal de Hospitalizacion
		[Widget] Gtk.Window menu_almacen = null;
		[Widget] Gtk.Button button_inventario = null;
		[Widget] Gtk.Button button_inv_subalmacen = null;
		[Widget] Gtk.Button button_requi_materiales = null;
		[Widget] Gtk.Button button_envios_subalmacenes = null;
		[Widget] Gtk.Button button_captura_fact_orden_comp = null;
		[Widget] Gtk.Button button_autorizacion_medicamento = null;
		[Widget] Gtk.Button button_productos_aplicados = null;
		[Widget] Gtk.Button button_analisis_devoluciones = null;
		[Widget] Gtk.Button button_productos_enviados = null;
		[Widget] Gtk.Button button_traspaso_subalmacenes = null;
		//[Widget] Gtk.Button button_soli_material;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public almacen (string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = conexion_a_DB._nombrebd;			
			
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "menu_almacen", null);
			gxml.Autoconnect (this);
			////// Muestra ventana de Glade
			menu_almacen.Show();
			
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			button_inventario.Clicked += new EventHandler(on_button_inventario_clicked);
			button_inv_subalmacen.Clicked += new EventHandler(on_button_inv_subalmacen_clicked);
			//button_soli_material.Clicked += new EventHandler(on_button_soli_material_clicked);
			button_requi_materiales.Clicked += new EventHandler(on_button_requi_materiales_clicked);
			button_envios_subalmacenes.Clicked += new EventHandler(on_button_envios_subalmacenes_clicked);
			button_captura_fact_orden_comp.Clicked += new EventHandler(on_button_captura_fact_orden_comp_clicked);
			button_autorizacion_medicamento.Clicked += new EventHandler(on_button_autorizacion_medicamento_clicked);
			// Productos aplicados a procemientos y pacientes por centro de costo
			button_productos_aplicados.Clicked += new EventHandler(on_button_productos_aplicados_clicked);
			button_productos_enviados.Clicked += new EventHandler(on_button_productos_enviados_clicked);
			button_traspaso_subalmacenes.Clicked += new EventHandler(on_button_traspaso_subalmacenes_clicked);
			button_analisis_devoluciones.Clicked += new EventHandler(on_button_analisis_devoluciones_clicked);
		}
		
		void on_button_inventario_clicked(object sender, EventArgs args)
		{
			// almacen_inventario.cs
			new osiris.inventario_almacen(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_captura_fact_orden_comp_clicked(object sender, EventArgs args)
		{
			// captura_fac_ordenes_compra.cs
			new osiris.factura_orden_compra(LoginEmpleado,NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado,nombrebd);
		}

		void on_button_autorizacion_medicamento_clicked(object sender, EventArgs args)
		{
			 new osiris.orden_compra_urgencias(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,0,"",0,"");
		}
		
		//void on_button_soli_material_clicked(object sender, EventArgs args)
		//{
		//	new osiris.solicitud_material(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,2);
		//}

		void on_button_inv_subalmacen_clicked(object sender, EventArgs args)
		{
			new osiris.inventario_sub_almacen(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,2,"ALMACEN 2PISO",1);
		}
		
		void on_button_requi_materiales_clicked(object sender, EventArgs args)
		{
			int [] array_idtipoadmisiones = { 0, 3, 18, 205, 500 };
			new osiris.requisicion_materiales_compras(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"ALMACEN GENERAL",205,"AND agrupacion IN ('NUT','OTR','MD1','ALM') ",array_idtipoadmisiones,0);
			//new osiris.requisicion_materiales_compras(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"COMPRAS",17,"AND agrupacion IN ('NUT','OTR','MD1','ALM') ",array_idtipoadmisiones,0);
		}
		
		void on_button_productos_aplicados_clicked(object sender, EventArgs args)
		{
			//new osiris.movimientos_productos(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,0);
			new osiris.movimientos_productos(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"cargos_pacientes","");
		}
		
		void on_button_envios_subalmacenes_clicked(object sender, EventArgs args)
		{
			new osiris.envio_de_materiales_subalmacenes(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);	
		}
		
		void on_button_analisis_devoluciones_clicked(object sender, EventArgs args)
		{
			new osiris.analisis_devoluciones(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"");
		}
		
		void on_button_productos_enviados_clicked(object sender, EventArgs args)
		{
			// Productos enviado a los sub-almacenes
			new osiris.movimientos_productos(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"envios_subalamcenes","");
		}
		
		void on_button_traspaso_subalmacenes_clicked(object sender, EventArgs args)
		{
			new osiris.inventario_sub_almacen(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,2,"ALMACEN 2PISO",3);
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}