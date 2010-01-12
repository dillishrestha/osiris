/////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////
// created on 09/04/2008 at 16:51 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	 Ing.Jesus Buentello Garza (programacion Mono)		
//				 Ing. Daniel Olivares - arcangeldoc@gmail.com (Ajustes y Pre-programacion)	
// 				  
// Licencia		: GLP
// S.O. 		: GNU/Linux RH4 ES
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
	public class costeo_productos
	{
		// //Declarando ventana de Productos Homero Prueba "Ventana Principal"
		[Widget] Gtk.Window costeo;
		[Widget] Gtk.Entry entry_folio_servicio;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Entry entry_ingreso;
		[Widget] Gtk.Entry entry_egreso;
		[Widget] Gtk.Entry entry_numero_factura;
		[Widget] Gtk.Entry entry_pid_paciente;
		[Widget] Gtk.Entry entry_nombre_paciente;
		[Widget] Gtk.Entry entry_telefono_paciente;
		[Widget] Gtk.Button button_buscar;
		[Widget] Gtk.Entry entry_cirugia;
		[Widget] Gtk.Entry entry_doctor;
		[Widget] Gtk.Entry entry_especialidad;
		[Widget] Gtk.Entry entry_tipo_paciente;
		[Widget] Gtk.Entry entry_aseguradora;
		[Widget] Gtk.Entry entry_porcentage_total;
		[Widget] Gtk.Entry entry_poliza;
		[Widget] Gtk.TreeView lista_de_detalle;
		[Widget] Gtk.TreeView lista_productos;
		[Widget] Gtk.Entry entry_total_cobrado;
		//[Widget] Gtk.Entry entry_total_esperado;
		[Widget] Gtk.Entry entry_dif_compra_cobro;
		//[Widget] Gtk.Entry entry_dif_cobrado_esperado;
		[Widget] Gtk.Entry entry_total_comprado;
		[Widget] Gtk.Button button_salir;
		[Widget] Gtk.Button button_imprimir;
		
		[Widget] Gtk.Button button_actualiza;
		
		private TreeStore treeViewEngineProducto;
		private TreeStore treeViewEngineDetaFact; 	// Detalle de la Factura 
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		string cirugia;
		string connectionString = "Server=localhost;" +
						"Port=5432;" +
						 "User ID=admin;" +
						"Password=1qaz2wsx;";
		
		//Declaracion de ventana de error y pregunta
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public costeo_productos(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = _nombrebd_;
			
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "costeo", null);
			gxml.Autoconnect (this);        
			costeo.Show();
			crea_treview_autorizacion();
			crea_treview_productos();
	
			this.button_selecciona.Clicked += new EventHandler(on_selec_folio_clicked);
			this.entry_folio_servicio.KeyPressEvent += onKeyPressEvent_enter_folio;

			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			this.button_imprimir.Clicked += new EventHandler(on_imprime_clicked);
			this.button_actualiza.Hide();
			this.button_imprimir.Clicked += new EventHandler(on_imprime_clicked);
		}
		
		public void on_imprime_clicked (object sender, EventArgs args)
		{
			//new osiris.rpt_costeo(nombrebd);   	
		}
		
		void on_selec_folio_clicked(object sender, EventArgs args)
		{
			llenado_de_productos_aplicados();
			llenado_de_treeview_costeo();
		}
		
		void llenado_de_treeview_costeo()
		{
			if ((string) this.entry_folio_servicio.Text != ""){
				treeViewEngineProducto.Clear();
				this.treeViewEngineDetaFact.Clear();
				NpgsqlConnection conexion;
				conexion = new NpgsqlConnection (connectionString+nombrebd);
			            
				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
		        	comando.CommandText = "SELECT to_char(SUM(cantidad_aplicada),'999999999.99') AS cantidadaplicada,to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,descripcion_producto,"+
		        			"to_char(osiris_erp_cobros_deta.folio_de_servicio,'9999999999') AS foliodeservicio,"+
							"to_char(osiris_erp_cobros_deta.precio_por_cantidad,'999999999.99') AS precio_cantidad,"+
							"to_char(osiris_erp_cobros_deta.pid_paciente,'9999999999') AS pidpaciente,osiris_his_paciente.nombre1_paciente || ' ' || "+  
							"osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_paciente,"+
							"to_char(osiris_productos.precio_producto_publico,'999999999.999') AS precio_venta,"+
							"to_char(osiris_erp_cobros_deta.precio_producto,'999999999.99') AS precioproducto,"+
							"to_char(osiris_productos.costo_por_unidad,'999999999.999') AS precio_compra,"+
							"to_char(osiris_productos.porcentage_ganancia,'999999999.999') AS porcentaje_de_ganancia,"+
							"to_char(osiris_productos.precio_producto_publico1,'999999999.999') AS precio_anterior,"+
							"to_char(osiris_productos.cantidad_de_embalaje,'999999999.999') AS embalaje,"+
							"to_char(((osiris_productos.precio_producto_publico * osiris_productos.porcentage_ganancia) / 100) + osiris_productos.precio_producto_publico,'99999999.99') AS gananciapesos,"+  						
							"osiris_productos.costo_por_unidad,"+
							"osiris_erp_cobros_deta.id_tipo_admisiones,descripcion_admisiones,"+
							"to_char(osiris_grupo_producto.id_grupo_producto,'999999') AS idgrupo "+
							"FROM osiris_erp_cobros_deta,osiris_productos,osiris_his_paciente,osiris_his_tipo_admisiones,osiris_grupo_producto "+
							"WHERE osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto AND "+ 
							"osiris_erp_cobros_deta.pid_paciente = osiris_his_paciente.pid_paciente AND "+ 
							"osiris_grupo_producto.id_grupo_producto = osiris_productos.id_grupo_producto AND "+
							"osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones AND "+
							"osiris_erp_cobros_deta.cantidad_aplicada > '0' AND "+
							"osiris_erp_cobros_deta.eliminado = false "+ 
							"AND osiris_erp_cobros_deta.folio_de_servicio = '"+(string) this.entry_folio_servicio.Text+"' "+
							"GROUP BY osiris_erp_cobros_deta.id_producto,descripcion_producto,folio_de_servicio,osiris_erp_cobros_deta.cantidad_aplicada,osiris_erp_cobros_deta.precio_producto,osiris_productos.costo_por_unidad,osiris_productos.cantidad_de_embalaje,osiris_productos.costo_por_unidad,osiris_productos.porcentage_ganancia,osiris_grupo_producto.id_grupo_producto,osiris_erp_cobros_deta.precio_por_cantidad,osiris_productos.precio_producto_publico1,osiris_erp_cobros_deta.pid_paciente,osiris_his_paciente.nombre1_paciente || ' ' || "+  
							"osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente,osiris_productos.precio_producto_publico,osiris_erp_cobros_deta.id_tipo_admisiones,descripcion_admisiones "+
							"ORDER BY osiris_erp_cobros_deta.id_producto,osiris_erp_cobros_deta.id_tipo_admisiones,"+
							"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente;";
					//Console.WriteLine(comando.CommandText.ToString());
					NpgsqlDataReader lector = comando.ExecuteReader ();
					
					float total_cantaplicada = 0;
					string descripcionproducto = "";
					string precioanterior = "";
					string preciocompra = "";
					string idproducto = "";
					decimal precio_uni = 0;
					float totalpreciocompra = 0;
					decimal porc_utilidad = 0;
					decimal totalprecioventa = 0;
					string precioventa = "";
					decimal porcentaje_ganancia = 0;
					string ganancia_pesos = "";
					decimal precio_costo = 0;
					decimal esperado = 0;
					decimal cobrado = 0;
					decimal comprado = 0;
					decimal diferencia = 0;
					decimal diferencia2 = 0;
					string id_tipo_admisiones = "";
					decimal costo_total = 0;
					string precio_cobrado = "";
					decimal preciocobrado = 0;
					decimal idgrupoproducto = 0;
					decimal totalcompra = 0;
					decimal ganancia = 0;
					decimal porcentaje_ganancia_compra = 0;
					decimal porcentage_total = 0;
					
					//variables para el total comprado
					decimal hospital = 0;
					decimal urgencias = 0;
					decimal odontologia = 0;
					decimal otros_servicio = 0; 
					decimal terapia_neonatal = 0; 
					decimal terapia_adulto = 0; 
					decimal terapia_pediatra = 0; 
					decimal endoscopia = 0; 
					decimal quirofano = 0; 
					decimal ginecologia = 0;
					decimal almacen_gral = 0;
					decimal rehabilitacion = 0;
					decimal imagenologia = 0;
					decimal	laboratorio = 0;
					
					//cobrado
					decimal cobrado_hospital = 0;
					decimal cobrado_urgencias = 0;
					decimal cobrado_odontologia = 0;
					decimal cobrado_otros_servicio = 0; 
					decimal cobrado_terapia_neonatal = 0; 
					decimal cobrado_terapia_adulto = 0; 
					decimal cobrado_terapia_pediatra = 0; 
					decimal cobrado_endoscopia = 0; 
					decimal cobrado_quirofano = 0; 
					decimal cobrado_ginecologia = 0;
					decimal cobrado_almacen_gral = 0;
					decimal cobrado_rehabilitacion = 0;
					decimal cobrado_imagenologia = 0;
					decimal	cobrado_laboratorio = 0;
					float total_preciocompra_1 = 0;
					float total_precioventa_1 = 0;				
					float total_porcentage_ganancia_1 = 0; 
					
					if(lector.Read()){
						idproducto = (string) lector["idproducto"];
						total_cantaplicada = float.Parse((string) lector["cantidadaplicada"]);
						descripcionproducto = (string) lector["descripcion_producto"];
						precioanterior = (string) lector["precio_anterior"];
						preciocompra = (string) lector["precio_compra"];
						precio_uni = (decimal) lector["costo_por_unidad"];
						precioventa = (string) lector["precio_venta"];
						
						if ((float) float.Parse(preciocompra) == 0){
							preciocompra = "1";
						}
						preciocobrado = Convert.ToDecimal(float.Parse(preciocompra) * total_cantaplicada);
						costo_total = (Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100);
						totalcompra = Convert.ToDecimal(total_cantaplicada * float.Parse(preciocompra));
						ganancia = Convert.ToDecimal(total_cantaplicada) * (Convert.ToDecimal(precioventa) -  Convert.ToDecimal(preciocompra));
						porcentaje_ganancia = ((ganancia / Convert.ToDecimal(precioventa)) * 100);
						porcentaje_ganancia_compra = ((ganancia / Convert.ToDecimal(preciocompra)) * 100);
						totalprecioventa = Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal((string) precioventa);
						precio_costo = (Convert.ToDecimal(precioventa) / ((Convert.ToDecimal(porcentaje_ganancia) / 100)+1));

						ganancia_pesos = (string) lector["gananciapesos"];
						precio_cobrado = (string) lector["precio_cantidad"];
						idgrupoproducto = Convert.ToDecimal((string) lector["idgrupo"]);
						
						switch ((int) lector["id_tipo_admisiones"]){
						case 100:  
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							urgencias += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_urgencias += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_urgencias +=  totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;
						case 200:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							rehabilitacion += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_rehabilitacion += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_rehabilitacion += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 205:     
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							almacen_gral += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_almacen_gral += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);						
							cobrado_almacen_gral += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 300:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							imagenologia += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_imagenologia += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);	
							cobrado_imagenologia += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 400:   
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							laboratorio += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_laboratorio += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);	
							cobrado_laboratorio += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 500:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							hospital += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_hospital += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);						
							cobrado_hospital += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 600:      
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							ginecologia += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_ginecologia += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);	
							cobrado_ginecologia += totalprecioventa;// Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 700:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							quirofano += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_quirofano += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_quirofano += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 710:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							endoscopia += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_endoscopia += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_endoscopia += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 810:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							terapia_adulto += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_terapia_adulto += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_terapia_adulto += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 820:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							terapia_pediatra += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_terapia_pediatra += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_terapia_pediatra += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 830:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							terapia_neonatal += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_terapia_neonatal += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_terapia_neonatal += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 920:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							otros_servicio += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_otros_servicio += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_otros_servicio += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 930:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							odontologia += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_odontologia += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_odontologia += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;       
						}
					
						if (idgrupoproducto == 4){
							porc_utilidad = busca_medicamento();	
						}else{
							porc_utilidad = busca_porcentage_utilidad(precio_uni);
						}

						this.treeViewEngineProducto.AppendValues(total_cantaplicada.ToString("F"),
							                                         descripcionproducto+idproducto,
							                                         precioanterior,
							                                         preciocompra,
							                                         totalcompra.ToString("F"),//Convert.ToString(total_cantaplicada * float.Parse(preciocompra)), 
							                                         porc_utilidad.ToString("F"),
							                                         ganancia.ToString("F"),//float.Parse(Convert.ToString((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100)).ToString("F"),
							                                         porcentaje_ganancia_compra.ToString("F"),//Convert.ToString(Convert.ToDecimal(totalprecioventa) * Convert.ToDecimal(total_cantaplicada)),
							                                         costo_total.ToString("F"),
							                                         preciocobrado.ToString("F"),
							                                         totalprecioventa.ToString("F"),
							                                         precioventa,
							                                         porcentaje_ganancia.ToString("F"),
							                                         ganancia_pesos,
							                                         float.Parse(Convert.ToString(Convert.ToDecimal(porcentaje_ganancia) - porc_utilidad)).ToString("F"),
							                                         float.Parse(Convert.ToString(Convert.ToDecimal(totalprecioventa) - ((Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100)))).ToString("F"),
							                                         precio_costo.ToString("F"),
							                                         (string) lector["descripcion_admisiones"]); 
					
						while(lector.Read()){
						
						if(idproducto != (string) lector["idproducto"]){
							
							idproducto = (string) lector["idproducto"];
							descripcionproducto = (string) lector["descripcion_producto"];
							precioanterior = (string) lector["precio_anterior"];
							preciocompra = (string) lector["precio_compra"];
							precio_uni = (decimal) lector["costo_por_unidad"];
							precioventa = (string) lector["precio_venta"];
							porcentaje_ganancia = 0;
							porcentaje_ganancia_compra = 0;
							
							if ((float) float.Parse(preciocompra) == 0){
								preciocompra = "1";
							}
		
							ganancia_pesos = (string) lector["gananciapesos"];
							total_cantaplicada = 0;
							total_cantaplicada += float.Parse((string) lector["cantidadaplicada"]);		
							precio_cobrado = (string) lector["precio_cantidad"];
							idgrupoproducto = Convert.ToDecimal((string) lector["idgrupo"]);
							totalcompra = Convert.ToDecimal(total_cantaplicada * float.Parse(preciocompra));
							porcentaje_ganancia = ((ganancia / Convert.ToDecimal(precioventa)) * 100);
							porcentaje_ganancia_compra = ((ganancia / Convert.ToDecimal(preciocompra)) * 100);
							
							totalprecioventa = Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							precio_costo = (Convert.ToDecimal(precioventa) / ((Convert.ToDecimal(porcentaje_ganancia) / 100)+1));

							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
								
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}

							costo_total = (Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100);
							preciocobrado = Convert.ToDecimal((string) lector["precio_compra"]) * Convert.ToDecimal(total_cantaplicada);

						}else{
							total_cantaplicada += float.Parse((string) lector["cantidadaplicada"]);

							
						}
		
							ganancia = Convert.ToDecimal(total_cantaplicada) * (Convert.ToDecimal(precioventa) -  Convert.ToDecimal(preciocompra));
							porcentaje_ganancia = ((ganancia / Convert.ToDecimal(precioventa)) * 100);
							porcentaje_ganancia_compra = ((ganancia / Convert.ToDecimal(preciocompra)) * 100);
						switch ((int) lector["id_tipo_admisiones"]){
						case 100:  
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							urgencias += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_urgencias += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_urgencias += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;
						case 200:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							rehabilitacion += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_rehabilitacion += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_rehabilitacion += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 205:     
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							almacen_gral += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_almacen_gral += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);						
							cobrado_almacen_gral += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 300:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							imagenologia += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_imagenologia += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);	
							cobrado_imagenologia += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 400:   
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							laboratorio += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_laboratorio += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);	
							cobrado_laboratorio += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 500:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							hospital += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_hospital += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);						
							cobrado_hospital += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 600:      
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							ginecologia += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_ginecologia += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);	
							cobrado_ginecologia += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 700:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							quirofano += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_quirofano += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_quirofano += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 710:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							endoscopia += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_endoscopia += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_endoscopia += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 810:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							terapia_adulto += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_terapia_adulto += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_terapia_adulto += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 820:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							terapia_pediatra += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_terapia_pediatra += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_terapia_pediatra += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 830:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							terapia_neonatal += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_terapia_neonatal += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_terapia_neonatal += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 920:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							otros_servicio += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_otros_servicio += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_otros_servicio += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;	
						case 930:
							if (idgrupoproducto == 4){
								porc_utilidad = busca_medicamento();	
							}else{
								porc_utilidad = busca_porcentage_utilidad(precio_uni);
							}
							odontologia += totalcompra;//(Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100); 
							//osiris_odontologia += Convert.ToDecimal (total_cantaplicada) * Convert.ToDecimal ((string) precioventa);
							cobrado_odontologia += totalprecioventa;//Convert.ToDecimal((string) lector["precioproducto"]) * Convert.ToDecimal(total_cantaplicada);
							break;       
						}
							
						this.treeViewEngineProducto.AppendValues(total_cantaplicada.ToString("F"),
							                                         descripcionproducto+idproducto,
							                                         precioanterior,
							                                         preciocompra,
							                                         totalcompra.ToString("F"),//Convert.ToString(total_cantaplicada * float.Parse(preciocompra)), 
							                                         porc_utilidad.ToString("F"),///chekar
							                                         ganancia.ToString("F"),//float.Parse(Convert.ToString((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100)).ToString("F"),
						                                             porcentaje_ganancia_compra.ToString("F"),//Convert.ToString(Convert.ToDecimal(totalprecioventa) * Convert.ToDecimal(total_cantaplicada)),
							                                         costo_total.ToString("F"),
							                                         preciocobrado.ToString("F"),
							                                         totalprecioventa.ToString("F"),
							                                         precioventa,
							                                         porcentaje_ganancia.ToString("F"),////chekar
							                                         ganancia_pesos,
							                                         float.Parse(Convert.ToString(Convert.ToDecimal(porcentaje_ganancia) - porc_utilidad)).ToString("F"),
							                                         float.Parse(Convert.ToString(Convert.ToDecimal(totalprecioventa) - ((Convert.ToDecimal(total_cantaplicada) * Convert.ToDecimal(preciocompra)) + ((Convert.ToDecimal((total_cantaplicada * float.Parse(preciocompra))) * porc_utilidad) / 100)))).ToString("F"),
							                                         precio_costo.ToString("F"),
							                                         (string) lector["descripcion_admisiones"]); 
					}
		
					if(hospital != 0){
						treeViewEngineDetaFact.AppendValues("HOSPITALIZACION",
					                                         hospital.ToString("C"),
					                                         cobrado_hospital.ToString("C"));
					                                        
					}
					
					if(urgencias != 0){
						treeViewEngineDetaFact.AppendValues("URGENCIAS",
					                                         urgencias.ToString("C"),
					                                         cobrado_urgencias.ToString("C"));
					                                         
					}
					
					if(odontologia != 0){
						treeViewEngineDetaFact.AppendValues("ODONTOLOGIA",
					                                         odontologia.ToString("C"),
					                                         cobrado_odontologia.ToString("C"));
					}
					
					if(otros_servicio != 0){
						treeViewEngineDetaFact.AppendValues("OTROS SERVIVIOS",
					                                         otros_servicio.ToString("C"),
					                                         cobrado_otros_servicio.ToString("C"));
					}
					
					if(terapia_neonatal != 0){ 
						treeViewEngineDetaFact.AppendValues("TERAPIA NEONATAL",
					                                         terapia_neonatal.ToString("C"),
					                                         cobrado_terapia_neonatal.ToString("C"));
					}
					
	                if(terapia_adulto != 0){ 
						treeViewEngineDetaFact.AppendValues("TERAPIA ADULTO",
					                                         terapia_adulto.ToString("C"),
					                                         cobrado_terapia_adulto.ToString("C"));
					}
					
					if(terapia_pediatra != 0){ 
						treeViewEngineDetaFact.AppendValues("TERAPIA PEDIATRA",
					                                         terapia_pediatra.ToString("C"),
					                                         cobrado_terapia_pediatra.ToString("C"));
					} 
					
					if(endoscopia != 0){ 
						treeViewEngineDetaFact.AppendValues("ENDOSCOPIA",
					                                         endoscopia.ToString("C"),
					                                         cobrado_endoscopia.ToString("C"));
					} 
					
					if(quirofano != 0){ 
						treeViewEngineDetaFact.AppendValues("QUIROFANO",
					                                         quirofano.ToString("C"),
					                                         cobrado_quirofano.ToString("C"));
					}
					
					if(ginecologia != 0){ 
						treeViewEngineDetaFact.AppendValues("GINECOLOGIA",
					                                         ginecologia.ToString("C"),
					                                         cobrado_ginecologia.ToString("C"));
					}
					
					if(almacen_gral != 0){ 
						treeViewEngineDetaFact.AppendValues("ALMACEN GRAL.",
					                                         almacen_gral.ToString("C"),
					                                         cobrado_almacen_gral.ToString("C"));
					}
					
					if(rehabilitacion != 0){  
						treeViewEngineDetaFact.AppendValues("REHABILITACION",
					                                         rehabilitacion.ToString("C"),
					                                         cobrado_rehabilitacion.ToString("C"));
					}
					
					if(imagenologia != 0){ 
						treeViewEngineDetaFact.AppendValues("IMAGENOLOGIA",
					                                         imagenologia.ToString("C"),
					                                         cobrado_imagenologia.ToString("C"));
					}
					
					if(laboratorio != 0){ 
						treeViewEngineDetaFact.AppendValues("LABORATORIO",
					                                         laboratorio.ToString("C"),
					                                         cobrado_laboratorio.ToString("C"));
					}
					
					//Calculo de totales y la diferencia ( entry's )
					comprado = hospital+urgencias+odontologia+otros_servicio+terapia_neonatal 
						+terapia_adulto+terapia_pediatra+endoscopia+quirofano+ginecologia
						+almacen_gral+rehabilitacion+imagenologia+laboratorio;

					/*esperado = osiris_hospital+osiris_urgencias+osiris_odontologia+osiris_otros_servicio 
						+osiris_terapia_neonatal+osiris_terapia_adulto+osiris_terapia_pediatra 
						+osiris_endoscopia+osiris_quirofano+osiris_ginecologia+osiris_almacen_gral
						+osiris_rehabilitacion+osiris_imagenologia+osiris_laboratorio;*/
					
					cobrado = cobrado_hospital+cobrado_urgencias+cobrado_odontologia+cobrado_otros_servicio 
						+cobrado_terapia_neonatal+cobrado_terapia_adulto+cobrado_terapia_pediatra 
						+cobrado_endoscopia+cobrado_quirofano+cobrado_ginecologia+cobrado_almacen_gral
						+cobrado_rehabilitacion+cobrado_imagenologia+cobrado_laboratorio;
							
					diferencia = cobrado - esperado;
					diferencia2 = cobrado - comprado;
					

					porcentage_total = (diferencia2 / cobrado) * 100;
					 
					 
					this.entry_porcentage_total.Text = porcentage_total.ToString("C");
					//this.entry_total_esperado.Text = esperado.ToString("C");
					this.entry_total_comprado.Text = comprado.ToString("C");
					//this.entry_dif_cobrado_esperado.Text = diferencia.ToString("C");
					this.entry_total_cobrado.Text = cobrado.ToString("C");
					this.entry_dif_compra_cobro.Text = diferencia2.ToString("C");
					
					hospital = 0;
					urgencias = 0;
					odontologia = 0;
					otros_servicio = 0; 
					terapia_neonatal = 0; 
					terapia_adulto = 0; 
					terapia_pediatra = 0; 
					endoscopia = 0; 
					quirofano = 0; 
					ginecologia = 0;
					almacen_gral = 0;
					rehabilitacion = 0;
					imagenologia = 0;
					laboratorio = 0;
					}
					
				}catch (NpgsqlException ex){
		 				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error, 
													ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();		msgBoxError.Destroy();
				}
				
				conexion.Close ();
			}else{
			
			}
		}
		
		decimal busca_medicamento()
		{
		decimal porcentageutilidad = 0;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();

				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT porcentage_ganancia "+ 
									"FROM osiris_productos "+
									"WHERE id_grupo_producto = 4;";
				//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if ((bool) lector.Read()){
					porcentageutilidad  = (decimal) lector["porcentage_ganancia"];
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message );
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close();
			
			return (porcentageutilidad);
						
		}
		
		
		
		decimal busca_porcentage_utilidad (decimal precio_uni)
		{
			decimal porcentageutilidad = 0;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();

				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT precio_costo_inicial,precio_costo_final,porcentage_de_ganancia "+ 
									"FROM osiris_erp_tabla_ganancia "+
									"WHERE precio_costo_final >= '"+precio_uni.ToString()+"'"+
									"AND precio_costo_inicial <= '"+precio_uni.ToString()+"' "+
									"LIMIT 1;";
				//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if ((bool) lector.Read()){
					porcentageutilidad  = (decimal) lector["porcentage_de_ganancia"];
					
					
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message );
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close();
			
			return (porcentageutilidad);
						
		}
		
		void llenado_de_productos_aplicados()
		{
		
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio AS foliodeatencion, "+
								"osiris_erp_cobros_enca.pagado,"+
								"osiris_erp_cobros_enca.cancelado,"+
								"osiris_erp_cobros_enca.cerrado,"+
								"osiris_erp_cobros_enca.alta_paciente,"+
								"osiris_erp_cobros_enca.bloqueo_de_folio,"+
								"to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente,"+
				            	"nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente,"+
				            	"telefono_particular1_paciente,numero_poliza,folio_de_servicio_dep,osiris_empresas.descripcion_empresa,"+
				            	"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH24:mi:ss') AS fecha_ingreso,"+
				            	"to_char(osiris_erp_cobros_enca.fecha_alta_paciente,'dd-MM-yyyy HH24:mi:ss') AS fecha_egreso,"+
				            	"to_char(osiris_erp_cobros_enca.numero_factura,'9999999999') AS numerofactura,"+
				            	"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fecha_nac_pa, "+
				            	"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy'),'9999'),'9999') AS edad,"+
								"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
				            	"to_char(osiris_erp_cobros_enca.deducible,'9999999.99') AS deduciblecaja, "+
				            	"to_char(osiris_erp_cobros_enca.total_abonos,'99999999.99') AS totalabonos, "+
				            	"to_char(osiris_erp_cobros_enca.coaseguro,'999.99') AS coasegurocaja,"+
				            	"to_char(osiris_erp_cobros_enca.numero_factura,'9999999999') AS numfactura,"+
				            	"osiris_erp_movcargos.id_tipo_paciente AS idtipopaciente, "+
				            	"osiris_erp_movcargos.id_tipo_admisiones AS idtipoadmision, "+
				            	"osiris_his_paciente.direccion_paciente,osiris_his_paciente.numero_casa_paciente,osiris_his_paciente.numero_departamento_paciente, "+
								"osiris_his_paciente.colonia_paciente,osiris_his_paciente.municipio_paciente,osiris_his_paciente.codigo_postal_paciente,osiris_his_paciente.estado_paciente,  "+
            					"descripcion_tipo_paciente,osiris_his_tipo_cirugias.descripcion_cirugia,"+
            					"osiris_his_paciente.id_empresa AS idempresa,osiris_empresas.lista_de_precio AS listadeprecio_empresa,"+   ///
            					"descripcion_admisiones,osiris_his_tipo_especialidad.descripcion_especialidad,"+
				            	"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,osiris_aseguradoras.lista_de_precio AS listadeprecio_aseguradora,"+   ///
				            	"osiris_erp_cobros_enca.id_medico,nombre_medico, "+
				            	"osiris_erp_movcargos.descripcion_diagnostico_movcargos,osiris_his_tipo_cirugias.id_tipo_cirugia,nombre_medico_encabezado,"+
				            	"osiris_erp_cobros_enca.facturacion, "+
				            	"osiris_erp_cobros_enca.pagado,"+
				            	"ltrim(to_char(osiris_erp_cobros_enca.honorario_medico,'999999999.99')) AS honorariomedico "+
				            	"FROM "+ 
				            	"osiris_erp_cobros_enca,osiris_his_paciente,osiris_erp_movcargos,osiris_his_tipo_admisiones,osiris_his_tipo_pacientes, "+
				            	"osiris_aseguradoras, osiris_his_medicos,osiris_his_tipo_cirugias,osiris_his_tipo_especialidad,osiris_empresas "+
				            	"WHERE "+
				            	"osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
				            	"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
				            	"AND osiris_erp_cobros_enca.id_medico = osiris_his_medicos.id_medico "+ 
								"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
								"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
								"AND osiris_erp_movcargos.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
								"AND osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad  "+
								"AND osiris_his_paciente.id_empresa = osiris_empresas.id_empresa "+
								"AND osiris_erp_cobros_enca.folio_de_servicio = '"+(string) this.entry_folio_servicio.Text+"' "+
								"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora;";
								
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//Console.WriteLine(comando.CommandText.ToString());
				
					if(lector.Read())
					{
						entry_ingreso.Text = (string) lector["fecha_ingreso"];
						entry_egreso.Text = (string) lector["fecha_egreso"];
						entry_numero_factura.Text = (string) lector["numerofactura"];
						entry_nombre_paciente.Text = (string) lector["nombre1_paciente"]+" "+(string) lector["nombre2_paciente"]+" "+(string) lector["apellido_paterno_paciente"]+" "+(string) lector["apellido_materno_paciente"];
						entry_pid_paciente.Text = (string) lector["pidpaciente"];
						entry_telefono_paciente.Text = (string) lector["telefono_particular1_paciente"];
						entry_poliza.Text =  (string) lector["numero_poliza"];
						if((int) lector["id_medico"] > 1){
							entry_doctor.Text = (string) lector["nombre_medico"];
						}else{
							entry_doctor.Text = (string) lector["nombre_medico_encabezado"];
						}
						entry_especialidad.Text = (string) lector["descripcion_especialidad"];
						entry_tipo_paciente.Text = (string) lector["descripcion_tipo_paciente"];
						
						if((int) lector ["id_aseguradora"] > 1){
							entry_aseguradora.Text = (string) lector["descripcion_aseguradora"];
						}else{
							entry_aseguradora.Text = (string) lector["descripcion_empresa"];						
						}
						if((int) lector["id_tipo_cirugia"] > 1){
			       			cirugia = (string) lector["descripcion_cirugia"];
			       		}else{
			       			cirugia = (string) lector ["descripcion_diagnostico_movcargos"];
			       		}
						this.entry_cirugia.Text = cirugia;
						
					}
					
				}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
		   				
			}
	       		conexion.Close ();
		}
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_folio(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				llenado_de_productos_aplicados();	
				llenado_de_treeview_costeo();
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
			{
				args.RetVal = true;
			}
			
		}

		void crea_treview_productos()
		{
			treeViewEngineProducto = new TreeStore(typeof(string),
			                                       typeof(string),
			                                       typeof(string),
			                                       typeof(string),
			                                       typeof(string),
			                                       typeof(string),
			                                       typeof(string),
			                                       typeof(string),
			                                       typeof(string),
			                                       typeof(string),													
			                                       typeof(string),
			                                       typeof(string),
			                                       typeof(string),
			                                       typeof(string),
			                                       typeof(string),
			                                       typeof(string),			                                        
			                                       typeof(string),
			                                       typeof(string));
													
			lista_productos.Model = treeViewEngineProducto;
			lista_productos.RulesHint = true;
			
			//lista_de_autorizacion.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono paciente*/
			
			TreeViewColumn col_cantidad = new TreeViewColumn();
			CellRendererText cel_cantidad = new CellRendererText();
			col_cantidad.Title = "Cantidad";
			col_cantidad.PackStart(cel_cantidad, true);
			col_cantidad.AddAttribute (cel_cantidad, "text", 0);
			
			
			TreeViewColumn col_descripcion = new TreeViewColumn();
			CellRendererText cel_descripcion = new CellRendererText();
			col_descripcion.Title = "Descripcion";
			col_descripcion.PackStart(cel_descripcion, true);
			col_descripcion.AddAttribute (cel_descripcion, "text", 1);
			col_descripcion.Resizable = true;
			cel_descripcion.Width = 450;
			
			TreeViewColumn col_precio_anterior = new TreeViewColumn();
			CellRendererText cel_precio_anterior = new CellRendererText();
			col_precio_anterior.Title = "Precio Anterior";
			col_precio_anterior.PackStart(cel_precio_anterior, true);
			col_precio_anterior.AddAttribute (cel_precio_anterior, "text", 2);
			
			TreeViewColumn col_precio_compra = new TreeViewColumn();
			CellRendererText cel_desc_producto = new CellRendererText();
			col_precio_compra.Title = "Costo Unitario";
			col_precio_compra.PackStart(cel_desc_producto, true);
			col_precio_compra.AddAttribute(cel_desc_producto, "text", 3);
			
			TreeViewColumn col_total_precio_compra = new TreeViewColumn();
			CellRendererText cel_total_precio_compra = new CellRendererText();
			col_total_precio_compra.Title = "Total Unitario";
			col_total_precio_compra.PackStart(cel_total_precio_compra, true);
			col_total_precio_compra.AddAttribute(cel_total_precio_compra, "text", 4);
			
			TreeViewColumn col_porcentaje_ganancia = new TreeViewColumn();
			CellRendererText cel_porcentaje_ganancia = new CellRendererText();
			col_porcentaje_ganancia.Title = "%GanaEstimada";
			col_porcentaje_ganancia.PackStart(cel_porcentaje_ganancia, true);
			col_porcentaje_ganancia.AddAttribute(cel_porcentaje_ganancia, "text", 5);
			col_porcentaje_ganancia.Resizable = true;
			
			TreeViewColumn col_ganancia_est = new TreeViewColumn();
			CellRendererText cel_ganancia_est = new CellRendererText();
			col_ganancia_est.Title = "$Ganancia";
			col_ganancia_est.PackStart(cel_ganancia_est, true);
			col_ganancia_est.AddAttribute(cel_ganancia_est, "text", 6);
			col_cantidad.Resizable = true;
			
			TreeViewColumn col_total_ganancia_est = new TreeViewColumn();
			CellRendererText cel_total_ganancia_est = new CellRendererText();
			col_total_ganancia_est.Title = "%Utl/Costo";
			col_total_ganancia_est.PackStart(cel_total_ganancia_est, true);
			col_total_ganancia_est.AddAttribute(cel_total_ganancia_est, "text", 7);

			TreeViewColumn col_total_precio_venta = new TreeViewColumn();
			CellRendererText cel_total_precio_venta = new CellRendererText();
			col_total_precio_venta.Title = "$T.VentaEsperada";
			col_total_precio_venta.PackStart(cel_total_precio_venta, true);
			col_total_precio_venta.AddAttribute(cel_total_precio_venta, "text", 8);
			
			TreeViewColumn col_total_cobrado = new TreeViewColumn();
			CellRendererText cel_total_cobrado = new CellRendererText();
			col_total_cobrado.Title = "$Cobrado";
			col_total_cobrado.PackStart(cel_total_cobrado, true);
			col_total_cobrado.AddAttribute(cel_total_cobrado, "text", 9);
			
			
			TreeViewColumn col_precio_venta = new TreeViewColumn();
			CellRendererText cel_precio_venta = new CellRendererText();
			col_precio_venta.Title = "$total Venta";
			col_precio_venta.PackStart(cel_precio_venta, true);
			col_precio_venta.AddAttribute(cel_precio_venta, "text", 10);

			TreeViewColumn col_p_venta_osiris = new TreeViewColumn();
			CellRendererText cel_p_venta_osiris = new CellRendererText();
			col_p_venta_osiris.Title = "P.Venta";
			col_p_venta_osiris.PackStart(cel_p_venta_osiris, true);
			col_p_venta_osiris.AddAttribute(cel_p_venta_osiris, "text", 11);
			
			TreeViewColumn col_porciento_ganancia = new TreeViewColumn();
			CellRendererText cel_porciento_ganancia = new CellRendererText();
			col_porciento_ganancia.Title = "%Utl/Vta";
			col_porciento_ganancia.PackStart(cel_porciento_ganancia, true);
			col_porciento_ganancia.AddAttribute(cel_porciento_ganancia, "text", 12);
			
			TreeViewColumn col_ganancia = new TreeViewColumn();
			CellRendererText cel_ganancia = new CellRendererText();
			col_ganancia.Title = "$GananciaCalculada";
			col_ganancia.PackStart(cel_ganancia, true);
			col_ganancia.AddAttribute(cel_ganancia, "text", 13);
			
			TreeViewColumn col_variacion = new TreeViewColumn();
			CellRendererText cel_variacion = new CellRendererText();
			col_variacion.Title = "%Variacion";
			col_variacion.PackStart(cel_variacion, true);
			col_variacion.AddAttribute(cel_variacion, "text", 14);
			
			TreeViewColumn col_diferencia = new TreeViewColumn();
			CellRendererText cel_diferencia = new CellRendererText();
			col_diferencia.Title = "$Diferencia";
			col_diferencia.PackStart(cel_diferencia, true);
			col_diferencia.AddAttribute(cel_diferencia, "text", 15);
			
			TreeViewColumn col_precio_costo = new TreeViewColumn();
			CellRendererText cel_precio_costo = new CellRendererText();
			col_precio_costo.Title = "PrecioCosto";
			col_precio_costo.PackStart(cel_precio_costo, true);
			col_precio_costo.AddAttribute(cel_precio_costo, "text", 16);
			
			TreeViewColumn col_admicion = new TreeViewColumn();
			CellRendererText cel_admicion = new CellRendererText();
			col_admicion.Title = "Dpto";
			col_admicion.PackStart(cel_admicion, true);
			col_admicion.AddAttribute(cel_admicion, "text", 17);

			lista_productos.AppendColumn(col_cantidad);//0
			lista_productos.AppendColumn(col_descripcion);//1
			//lista_productos.AppendColumn(col_precio_anterior);//2
			lista_productos.AppendColumn(col_precio_compra);//3
			lista_productos.AppendColumn(col_total_precio_compra);//4	
			lista_productos.AppendColumn(col_total_ganancia_est);//7
			lista_productos.AppendColumn(col_porcentaje_ganancia);//5
			lista_productos.AppendColumn(col_ganancia_est);//6
			//lista_productos.AppendColumn(col_total_ganancia_est);//7
			//lista_productos.AppendColumn(col_total_precio_venta);//8
			//lista_productos.AppendColumn(col_total_cobrado);//9
			lista_productos.AppendColumn(col_precio_venta);//10
			lista_productos.AppendColumn(col_p_venta_osiris);//11
			lista_productos.AppendColumn(col_porciento_ganancia);//12
			//lista_productos.AppendColumn(col_ganancia);//13
			//lista_productos.AppendColumn(col_variacion);//14
			//lista_productos.AppendColumn(col_diferencia);//15
			//lista_productos.AppendColumn(col_precio_costo);//16
			lista_productos.AppendColumn(col_admicion);//17
		}
		
 		void crea_treview_autorizacion()
		{
			// Creacion de Liststore
			treeViewEngineDetaFact = new TreeStore(	typeof (string),
													typeof (string),
													typeof (string),
													typeof (string));
		        							   
			this.lista_de_detalle.Model = treeViewEngineDetaFact;
			
			CellRendererText cellrt1 = new CellRendererText();  // aplica a todas la columnas
				
			TreeViewColumn col_cantidad = new TreeViewColumn();
			col_cantidad.Title = "Descripcion";
			col_cantidad.PackStart(cellrt1, true);
			col_cantidad.AddAttribute (cellrt1, "text", 0);    // columna 1
			
			TreeViewColumn col_descripcion = new TreeViewColumn();
			col_descripcion.Title = "TotalComprado";
			col_descripcion.PackStart(cellrt1, true);
			col_descripcion.AddAttribute (cellrt1, "text", 1);    // columna 2
			
			TreeViewColumn col_precio_unitario = new TreeViewColumn();
			col_precio_unitario.Title = "TotalCobrado";
			col_precio_unitario.PackStart(cellrt1, true);
			col_precio_unitario.AddAttribute (cellrt1, "text", 2);    // columna 3
			
			TreeViewColumn col_importe = new TreeViewColumn();
			col_importe.Title = "TotalEsperado";
			col_importe.PackStart(cellrt1, true);
			col_importe.AddAttribute (cellrt1, "text", 3);    // columna 4

			lista_de_detalle.AppendColumn(col_cantidad);
			lista_de_detalle.AppendColumn(col_descripcion);
			lista_de_detalle.AppendColumn(col_precio_unitario);
			//lista_de_detalle.AppendColumn(col_importe);

		}		
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
		}	

	}
	
}		
