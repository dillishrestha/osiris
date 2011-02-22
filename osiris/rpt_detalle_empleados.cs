//////////////////////////////////////////////////////////
// created on 08/02/2008 at 08:39 a
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
// Programa		: 
// Proposito	:
// Objeto		:
//////////////////////////////////////////////////////////

using System;
using Gtk;
using Npgsql;
using Cairo;
using Pango;

namespace osiris
{	
	public class rpt_detalle_empleados
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 70;
		int separacion_linea = 10;
		int numpage = 1;
		
		string contrato_empleado;
		string appaterno;
		string apmaterno;
		string nom1;
		string nom2;
		string dia_nac;
		string mes_nac;
		string anno_nac;
		string lugar_nac;
		string rfc_empleado;
		string curp_empleado;
		string imss_empleado;
		string infonavit_empleado;
		string afore_empleado;
		string residencia_empleado;
		string nom_padre;
		string nom_madre;
		string calle_empleado;
		string codigo_postal;
		string colonia_empleado;
		string tel1;
		string notas_empleado;
		string dia_ingreso;
		string mes_ingreso;
		string anno_ingreso;
		string contrato;
		string nombrepuesto;
		string depto_empleado;
		string id_empleado;
		string edad;
		string puesto;
		string numcalle_empleado;
		string tmp_estado_civil;
		string tmp_municipios;
		string tmp_estado;
		string var_tipo_casa;
		string tipo_contrato_oculta;
		string tipo_pago_oculta;
		string sueldo_actual_oculta;
		string locker;
		
		class_public classpublic = new class_public();
		
		public rpt_detalle_empleados(string contrato_empleado_, string appaterno_, string apmaterno_,string nom1_,string nom2_,string dia_nac_,string mes_nac_,string anno_nac_,
		                     string lugar_nac_,string rfc_empleado_,string curp_empleado_,string imss_empleado_,string infonavit_empleado_,
		                     string afore_empleado_,string residencia_empleado_,string nom_padre_,string nom_madre_,string calle_empleado_,
		                     string codigo_postal_,string colonia_empleado_,string tel1_,
		                     string notas_empleado_,string dia_ingreso_,string mes_ingreso_,string anno_ingreso_, 
		                     string nombrepuesto_,string depto_empleado_,string id_empleado_,string edad_,string numcalle_empleado_,
		                     string tmp_estado_civil_,string tmp_municipios_,
		                     string tmp_estado_,string var_tipo_casa_,
		                     string tipo_contrato_oculta_,string tipo_pago_oculta_,string locker_,
		                     string sueldo_actual_oculta_)
		{
			contrato_empleado = contrato_empleado_;
			appaterno = appaterno_;
			apmaterno = apmaterno_;
			nom1 = nom1_;
			nom2 = nom2_;
			dia_nac = dia_nac_;
			mes_nac = mes_nac_;
			anno_nac = anno_nac_;
			lugar_nac = lugar_nac_;
			rfc_empleado = rfc_empleado_;
			curp_empleado = curp_empleado_;
			imss_empleado = imss_empleado_;
			infonavit_empleado = infonavit_empleado_;
			afore_empleado = afore_empleado_;
			residencia_empleado = residencia_empleado_;
			nom_padre = nom_padre_;
			nom_madre = nom_madre_;
			calle_empleado = calle_empleado_; 
			codigo_postal = codigo_postal_;
			colonia_empleado = colonia_empleado_;
			tel1 = tel1_; 
			notas_empleado = notas_empleado_;
			dia_ingreso = dia_ingreso_ ;
			mes_ingreso = mes_ingreso_;
			anno_ingreso = anno_ingreso_;
			nombrepuesto = nombrepuesto_;
			depto_empleado = depto_empleado_;
			id_empleado = id_empleado_;
			edad = edad_;
			numcalle_empleado = numcalle_empleado_;
			tmp_estado_civil = tmp_estado_civil_;
			tmp_municipios = tmp_municipios_;
			tmp_estado = tmp_estado_;
			var_tipo_casa = var_tipo_casa_;
			tipo_contrato_oculta = tipo_contrato_oculta_;
			tipo_pago_oculta = tipo_pago_oculta_;
			sueldo_actual_oculta = sueldo_actual_oculta_;
			locker = locker_;
			escala_en_linux_windows = classpublic.escala_linux_windows;

			print = new PrintOperation ();
			print.JobName = "REGISTRO DE EMPLEADO";	// Name of the report
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run(PrintOperationAction.PrintDialog, null);
		}
		
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1;  // crea cantidad de copias del reporte			
			// para imprimir horizontalmente el reporte
			//print.PrintSettings.Orientation = PageOrientation.Landscape;
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
			string toma_descrip_prod = "";
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
		    
			/*	    
			ContextoImp.MoveTo(150, 775);
		    ContextoImp.Show("COORDINACIÓN DE SERVICIOS AL PERSONAL");
		    ContextoImp.MoveTo(152, 760);
		    ContextoImp.Show("RECURSOS HUMANOS - REGISTRO DE ALTA");
  	        
  	        //ContextoImp.MoveTo(150, );
       	      
			//Registro de Alta:
			Gnome.Print.Setfont (ContextoImp, fuente2);
			int linea = 735;
			int separacion = -10;
			ContextoImp.MoveTo(20, linea);
			linea +=  separacion;
			ContextoImp.Show("NUM. DE EMPLEADO: " +id_empleado);
  	        ContextoImp.MoveTo(365, 735);
  	        ContextoImp.Show("NUM. DE LOCKERS:  "+locker);
			linea +=  separacion;
			ContextoImp.MoveTo(20, linea);
			ContextoImp.Show("NOMBRE: " + appaterno+ " " + apmaterno+ " " +nom1 + " " + nom2 +"");
			linea +=  separacion;
			ContextoImp.MoveTo(365, 715);
  	        ContextoImp.Show("FECHA DE NACIMIENTO: " +dia_nac+ "/"+mes_nac+ "/"+anno_nac);
			linea +=  separacion;
  	        ContextoImp.MoveTo(20, linea);
			ContextoImp.Show("LUGAR DE NACIMIENTO: " +lugar_nac );
  	        ContextoImp.MoveTo(365, 695);
			ContextoImp.Show("EDAD: " +edad );
			linea +=  separacion;
			ContextoImp.MoveTo(420, 695);
			ContextoImp.Show("" );
			linea +=  separacion;
			ContextoImp.MoveTo(20, linea);
			ContextoImp.Show("DOMICILIO PARTICULAR: "+calle_empleado+ " " +numcalle_empleado+    "    Colonia: " +colonia_empleado);
			linea +=  separacion;
			ContextoImp.MoveTo(315, 675);
			ContextoImp.Show("");
			linea +=  separacion;
			ContextoImp.MoveTo(20, 655);
			ContextoImp.Show("MUNICIPIO: " +tmp_municipios);
			linea +=  separacion;
			ContextoImp.MoveTo(365, 655);
			ContextoImp.Show("ESTADO: " +tmp_estado);
			linea +=  separacion;
			ContextoImp.MoveTo(20, linea);
			ContextoImp.Show("CODIGO POSTAL: " + codigo_postal);
			linea +=  separacion;
			ContextoImp.MoveTo(200, 635);
			ContextoImp.Show("CASA: "+var_tipo_casa  );
			ContextoImp.MoveTo(365, 635);
			ContextoImp.Show("TIEMPO DE RESIDENCIA:" +residencia_empleado+ "años");
			linea +=  separacion;
			ContextoImp.MoveTo(20,  linea);
			ContextoImp.Show("ESTADO CIVIL: " +tmp_estado_civil );
			linea +=  separacion;
			ContextoImp.MoveTo(200, 615);
			ContextoImp.Show("TELEFONO: " +tel1);
			ContextoImp.MoveTo(365, 615);
			ContextoImp.Show("R.F.C. " +rfc_empleado); 
			linea +=  separacion;
			ContextoImp.MoveTo(20, linea);
			ContextoImp.Show("No. SEGURO SOCIAL: " +imss_empleado);
			linea +=  separacion;
			ContextoImp.MoveTo(365, 595);
			ContextoImp.Show("CURP: " +curp_empleado);
			linea +=  separacion;
			ContextoImp.MoveTo(20, linea);
			ContextoImp.Show("INFONAVIT: " +infonavit_empleado);
			linea +=  separacion;
			ContextoImp.MoveTo(365, 575);
			ContextoImp.Show("AFORE: " + afore_empleado);
			ContextoImp.MoveTo(240, 530);
			Gnome.Print.Setfont (ContextoImp, fuente4);
			ContextoImp.Show("DATOS CONTRATO");
			Gnome.Print.Setfont (ContextoImp, fuente2);
			ContextoImp.MoveTo(20, 495);
			ContextoImp.Show("FECHA DE INGRESO: " +dia_ingreso+ "/"+mes_ingreso+ "/"+anno_ingreso);     
			ContextoImp.MoveTo(300, 495);
			ContextoImp.Show("PUESTO: " +nombrepuesto);
			ContextoImp.MoveTo(300, 475);
			ContextoImp.Show("DEPARTAMENTO: " +depto_empleado);
			ContextoImp.MoveTo(20, 475);
			ContextoImp.Show("SUELDO : $"+sueldo_actual_oculta); 
			ContextoImp.MoveTo(20, 455);
			ContextoImp.Show("TIPO CONTRATO: " +contrato_empleado);
			ContextoImp.MoveTo(300, 455);
			ContextoImp.Show("TIPO DE PAGO: "+tipo_pago_oculta);
			ContextoImp.MoveTo(240, 415);
			Gnome.Print.Setfont (ContextoImp, fuente4);
			ContextoImp.Show("DATOS FAMILIARES");
			Gnome.Print.Setfont (ContextoImp, fuente2);
			ContextoImp.MoveTo(20, 385);
			ContextoImp.Show("NOMBRE PADRE: " +nom_padre);
			ContextoImp.MoveTo(20, 365);
			ContextoImp.Show("NOMBRE MADRE: " +nom_madre);
			ContextoImp.MoveTo(30,330);
			Gnome.Print.Setfont (ContextoImp, fuente4);
			ContextoImp.Show("NOTAS:");
			Gnome.Print.Setfont (ContextoImp, fuente2);
			ContextoImp.MoveTo(90, 330);
			ContextoImp.Show(notas_empleado);
			ContextoImp.MoveTo(20, 200);
			Gnome.Print.Setfont (ContextoImp, fuente4); 
			ContextoImp.Show(""+nom1 +  " " + nom2 +  " "+ appaterno+ " "+ apmaterno+"");
			ContextoImp.MoveTo(365,200); ContextoImp.Show("JEFE DE RECURSOS HUMANOS");
			ContextoImp.MoveTo(200,80);	ContextoImp.Show("GENERADOR DE NOMINAS");
			ContextoImp.ShowPage();
			*/
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
	}
}