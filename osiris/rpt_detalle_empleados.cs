using System;
using Gtk;
using Gnome;
using Npgsql;
using System.Data;
using Glade;
using System.Collections;
using GtkSharp;

namespace osiris
{	
	public class rpt_detalle_empleados
	{
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

			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default ());
			Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, "REGISTRO DE EMPLEADO", 0);

			int respuesta = dialogo.Run ();
       	            
			if (respuesta == (int) PrintButtons.Cancel){
				//boton Cancelar
				Console.WriteLine("Impresión cancelada");
				dialogo.Hide (); 
				dialogo.Dispose (); 
				return;
			}

			Gnome.PrintContext ctx = trabajo.Context;
			ComponerPagina(ctx, trabajo); 
			trabajo.Close();
			switch (respuesta){   //imprimir
				case (int) PrintButtons.Print:   
					trabajo.Print (); 
					break;
			    case (int) PrintButtons.Preview://vista previa
					new PrintJobPreview(trabajo, "REGISTRO DE EMPLEADO").Show();
					break;
			}
			dialogo.Hide (); dialogo.Dispose ();
		}
		
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
		    ContextoImp.BeginPage("Pagina 1");
			// Crear una fuente 
				  
			//Gnome.Font fuente = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
			//Gnome.Font fuente1 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
			Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 11);
			//Gnome.Font fuente3 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
			// ESTA FUNCION ES PARA QUE EL TEXTO SALGA EN NEGRITA
			Gnome.Font fuente4 = Gnome.Font.FindClosestFromWeightSlant("Bitstream Vera Sans", FontWeight.Bold ,false, 12);
			//Encabezado de pagina
			   
			Gnome.Print.Setfont (ContextoImp, fuente4);
			    
    	   //ESTA FUNCION SIRVE PARA CREAR RECTANGULOS Y CUADROS:
    	   //cud datos fam:
			Gnome.Print.RectStroked(ContextoImp,20,410,555,20);
		    //datos contrato:
		    Gnome.Print.RectStroked(ContextoImp,20,525,555,20);
		    //notas:  
		    Gnome.Print.RectStroked(ContextoImp,20,325,555,20);
		    //marco: 
		    //Gnome.Print.RectStroked(ContextoImp,10,10,575,815);
		    //LINEAS DE FIRMA:
		    Gnome.Print.LineStroked(ContextoImp,20,215,280,215);
		    Gnome.Print.LineStroked(ContextoImp,360,215,570,215);
			Gnome.Print.LineStroked(ContextoImp,180,95,390,95);
		    
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
		}
	}
}