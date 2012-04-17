// created on 18/04/2007 at 09:06 am
///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
//				  Daniel Olivares C. (Adecuaciones y reprogramacion) arcangeldoc@gmail.com 05/05/2007
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
// Programa		: hscmty.cs
// Proposito	: Impresion del procedimiento de cobranza 
// Objeto		: rpt_proc_cobranza.cs
using System;
using Gtk;
using Npgsql;
using Cairo;
using Pango;

namespace osiris
{
	public class proc_totales
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 162;
		int separacion_linea = 10;
		int numpage = 1;
		
		string connectionString;
        int PidPaciente = 0;
		int folioservicio = 0;
		int id_tipopaciente = 0;
		string nombrebd;
		string fecha_admision;
		string fechahora_alta;
		string nombre_paciente;
		string telefono_paciente;
		string doctor;
		string cirugia;
		string fecha_nacimiento;
		string edadpac;
		string tipo_paciente;
		string aseguradora;
		string dir_pac;
		string empresapac;
		bool aplicar_descuento = true;
		bool aplicar_siempre = false;
		string query_rango_fechas = "";
			
		////////variables que utilizo dentro del ciclo
		int idadmision_ = 0;					//tipo de admision en donde se realizaron los cargos...
		int idgrupoproducto = 0;					// el codigo del producto
		decimal precio_por_cantidad = 0;		//esta variable se utiliza para ir guwerdandop el precio de un producto dependiendo de cuanto se aplico de este
		decimal iva_del_grupo = 0;					//es un valor en donde se van a ir sumando cada iva que se le aplica al producto
		decimal porcentagedesc = 0;			//es el el descuento en porciento si es que existe un descuento
		decimal descuento_neto = 0;			// valor desc sin iva
		decimal descurento_del_grupo = 0;		//el descuento que se aplica en cada grupo de productos
		decimal iva_de_descuento = 0;			// valor iva del descuento 
		decimal descuento_del_grupo = 0;		// suma del iva del desc y del desc neto
		decimal subtotal_del_grupo = 0;		//subtotal del grupo de productos
		decimal subtotal_al_15_grupo = 0;		//es el subtotal de los productos que contienen iva en un grupo de productos
		decimal subtotal_al_0_grupo = 0;		//es el subtotal de los productos que no contienen iva en un grupo de productos
		decimal subtotal_al_15 = 0;			//es el subtotal de los productos que contienen iva en todo el movimiento
		decimal subtotal_al_0 = 0;			//es el subtotal de los productos que no contienen iva en todo el movimiento
		decimal deducible = 0;					//es el dedicible de impuestos plicado en la factura
		decimal coaseguro = 0;					//es el valor de coaseguro que se descuenta del total facturado
		decimal total_del_grupo = 0;			//precio total del grupo de productos
		decimal total_de_iva = 0;				//suma de todos los ivas de todos los lugares y grupos de productos
		decimal total_de_descuento_neto =0;	//es el descuento neto de facturacion
		decimal total_de_iva_descuento =0;	//es el iva del descuento neto de facturacion
		decimal total_descuento=0;			//es la la suma del descuento neto y el iva del descuento neto de facturacion
		/// restar abonos y pago final
		decimal totabono = 0;
		decimal totpago = 0;
		decimal honorarios = 0;
		decimal valoriva;
						
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public proc_totales ( int PidPaciente_ , int folioservicio_,string nombrebd_ ,string entry_fecha_admision_,string entry_fechahora_alta_,
						string entry_numero_factura_,string entry_nombre_paciente_,string entry_telefono_paciente_,string entry_doctor_,
						string entry_tipo_paciente_,string entry_aseguradora_,string edadpac_,string fecha_nacimiento_,string dir_pac_,
						string cirugia_,string empresapac_,int idtipopaciente_,string query)
		{
			PidPaciente = PidPaciente_;//
			folioservicio = folioservicio_;//
			fecha_admision = entry_fecha_admision_;//
			fechahora_alta = entry_fechahora_alta_;//
			nombre_paciente = entry_nombre_paciente_;//
			telefono_paciente = entry_telefono_paciente_;//
			doctor = entry_doctor_;//
			cirugia = cirugia_;//
			tipo_paciente = entry_tipo_paciente_;//
			id_tipopaciente = idtipopaciente_;
			aseguradora = entry_aseguradora_;//
			edadpac = edadpac_;//
			fecha_nacimiento = fecha_nacimiento_;//
			dir_pac = dir_pac_;//
			empresapac = empresapac_;//
			query_rango_fechas = query;
						
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			valoriva = decimal.Parse(classpublic.ivaparaaplicar);
			escala_en_linux_windows = classpublic.escala_linux_windows;
			
			print = new PrintOperation ();
			print.JobName = "Resumen Estado de Cuenta";
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);	
		}
      	
			
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1;  // crea cantidad de copias del reporte			
			// para imprimir horizontalmente el reporte
			print.PrintSettings.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{			
			PrintContext context = args.Context;
			ejecutar_consulta_reporte(context);
		}
		
		void ejecutar_consulta_reporte(PrintContext context)
		{
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;			
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
	}
}