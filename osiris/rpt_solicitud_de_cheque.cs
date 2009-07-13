// created on 00/06/2008 at 00:00 p
///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Marcos Irak Gaspar Avila (Programacion) ing.gaspar@gmail.com
// 				  Daniel Olivares Cuevas (Pre-Programacion, Colaboracion y Ajustes) arcangeldoc@gmail.com
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
// Proposito	:  
// Objeto		: 
//////////////////////////////////////////////////////////
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
	public class rpt_solicitud_cheque
	{
		public string[] sUnidades = {"", "un", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "diez", 
									"once", "doce", "trece", "catorce", "quince", "dieciseis", "diecisiete", "dieciocho", "diecinueve", "veinte", 
									"veintiún", "veintidos", "veintitres", "veinticuatro", "veinticinco", "veintiseis", "veintisiete", "veintiocho", "veintinueve"};
 		public string[] sDecenas = {"", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa"};
 		public string[] sCentenas = {"", "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos"};
  		public string sResultado = "";
		public string connectionString = "Server=localhost;" +
        	    	                     "Port=5432;" +
            	    	                 "User ID=admin;" +
                	    	             "Password=1qaz2wsx;";
		public string nombredoctor = "";
		public float hono_medico = 0;
		public float descuento_hono_medico = 0;
		public string cantidad_de_letras = "";
		public string folio_servicio = "";
		public float total_descuento = 0;
		public string tipo_paciente = "";
		public string nombre_paciente = "";
		public string aseguradora = "";
		
		public rpt_solicitud_cheque(string nombredoctor_, string hono_medico_, string tipo_paciente_, string aseguradora_, string nombre_paciente_, string folio_servicio_)		
		{
			folio_servicio = folio_servicio_;
			nombre_paciente = nombre_paciente_;
			tipo_paciente = tipo_paciente_;
			aseguradora = aseguradora_;	
			nombredoctor = nombredoctor_;			
			hono_medico = float.Parse(hono_medico_);
			descuento_hono_medico = (hono_medico * 10 )  / 100;
			total_descuento = hono_medico - descuento_hono_medico;
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default ());
   		    Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, "Solicitud de cheques", 0);
       	    int respuesta = dialogo.Run ();

			if (respuesta == (int) PrintButtons.Cancel){   //boton Cancelar
				dialogo.Hide (); 
				dialogo.Dispose (); 
				return;
			}
	        Gnome.PrintContext ctx = trabajo.Context;
   	     	ComponerPagina(ctx, trabajo); 
	       	trabajo.Close();
			switch (respuesta)
			{   //imprimir
				case (int) PrintButtons.Print:   
					trabajo.Print (); 
				break;
				//vista previa
				case (int) PrintButtons.Preview:
					new PrintJobPreview(trabajo, "Solicitud de cheque").Show();
				break;
			}
			dialogo.Hide (); dialogo.Dispose ();
		}
		
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{ 
			for (int i1=0; i1 <= 1; i1++){
				ContextoImp.BeginPage("Pagina 1");	
				 
				cantidad_de_letras = traduce_numeros(total_descuento.ToString("F"));
				Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
				Gnome.Font fuente3 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
				
				// ESTA FUNCION ES PARA QUE EL TEXTO SALGA EN NEGRITA
				Gnome.Font fuente4 = Gnome.Font.FindClosestFromWeightSlant("Bitstream Vera Sans", FontWeight.Bold ,false, 12);
				Gnome.Font fuente5 = Gnome.Font.FindClosestFromWeightSlant("Bitstream Vera Sans", FontWeight.Bold ,false, 10);
				Gnome.Print.Setfont (ContextoImp, fuente2);
	
				ContextoImp.MoveTo(400, 780);			ContextoImp.Show(DateTime.Now.ToString("dd        MM     yyyy") );			 			
				ContextoImp.MoveTo(130, 740);			ContextoImp.Show(nombredoctor);
				ContextoImp.MoveTo(530, 710);			ContextoImp.Show("X" ); 
			    ContextoImp.MoveTo(70, 660);			ContextoImp.Show("Pagos de Honorarios Medicos / Folio :"+ folio_servicio.Trim()+" Paciente :"+nombre_paciente.Trim());
	            ContextoImp.MoveTo(100, 650);			ContextoImp.Show(tipo_paciente);
	            ContextoImp.MoveTo(70, 640);			ContextoImp.Show(aseguradora);
			    ContextoImp.MoveTo(130, 690);			ContextoImp.Show("("+ cantidad_de_letras + ")"); 
			    ContextoImp.MoveTo(405, 620);			ContextoImp.Show(hono_medico.ToString("C"));    
			    ContextoImp.MoveTo(460, 620);			ContextoImp.Show(descuento_hono_medico.ToString("C"));
			    ContextoImp.MoveTo(445, 620);			ContextoImp.Show(" - ");
			    ContextoImp.MoveTo(170, 720);			ContextoImp.Show(total_descuento.ToString("C"));
			    ContextoImp.MoveTo(370, 620);			ContextoImp.Show("Total:");
			    ContextoImp.MoveTo(500, 620);			ContextoImp.Show(total_descuento.ToString("C"));
				ContextoImp.ShowPage();
			}
		}
		public string traduce_numeros (string sNumero) {
			double dNumero;
			double dNumAux = 0;
			char x;
			string sAux;
			
			sResultado = " ";
			try{
				dNumero = Convert.ToDouble (sNumero);
			}catch {				
				return "";
			} 
			if (dNumero > 999999999999)
				return "";
 
			if (dNumero > 999999999) {
				dNumAux = dNumero % 1000000000000;
				sResultado += Numeros (dNumAux, 1000000000) + " mil ";
			}
 
			if (dNumero > 999999) {
				dNumAux = dNumero % 1000000000;
				sResultado += Numeros (dNumAux, 1000000) + " millones ";
			}
 
			if (dNumero > 999) {
				dNumAux = dNumero % 1000000;
				sResultado += Numeros (dNumAux, 1000) + " mil ";
			}
 
			dNumAux = dNumero % 1000;	
			sResultado += Numeros (dNumAux, 1);
  
			//Enseguida verificamos si contiene punto, si es así, los convertimos a texto.
			sAux = dNumero.ToString();
 
			if (sAux.IndexOf(".") >= 0){			
				sResultado += ObtenerDecimales (sNumero);
			}else{
				sResultado += "pesos 00/100 M.N.";
			}
			//Las siguientes líneas convierten el primer caracter a mayúscula.
			sAux = sResultado;
			x = char.ToUpper (sResultado[1]);
			sResultado = x.ToString ();
 
			for (int i = 2; i<sAux.Length; i++)
				sResultado += sAux[i].ToString();
 
			return sResultado;
		}
		 
		public string ConvertirCadena (double dNumero) {
			double dNumAux = 0;
			char x;
			string sAux;
			 
			sResultado = " ";
 
			if (dNumero > 999999999999)
				return "";
 
			if (dNumero > 999999999) {
				dNumAux = dNumero % 1000000000000;
				sResultado += Numeros (dNumAux, 1000000000) + " mil ";
			}
 
			if (dNumero > 999999) {
				dNumAux = dNumero % 1000000000;
				sResultado += Numeros (dNumAux, 1000000) + " millones ";
			}
 
			if (dNumero > 999) {
				dNumAux = dNumero % 1000000;
				sResultado += Numeros (dNumAux, 1000) + " mil ";
			}
 
			dNumAux = dNumero % 1000;	
			sResultado += Numeros (dNumAux, 1);
 
 
			//Enseguida verificamos si contiene punto, si es así, los convertimos a texto.
			sAux = dNumero.ToString();
 
			if (sAux.IndexOf(".") >= 0){
				sResultado += ObtenerDecimales (sAux);
			}else{
				sResultado += "pesos 00/100 M.N.";
			}
 
			//Las siguientes líneas convierten el primer caracter a mayúscula.
			sAux = sResultado;
			x = char.ToUpper (sResultado[1]);
			sResultado = x.ToString ();
 
			for (int i = 2; i<sAux.Length; i++)
				sResultado += sAux[i].ToString();
 
			return sResultado;
		}
 
		private string Numeros (double dNumAux, double dFactor) {
			double dCociente = dNumAux / dFactor;
			double dNumero = 0;
			int iNumero = 0;
			string sNumero = "";
			string sTexto = "";
 
			if (dCociente >= 100){
				dNumero = dCociente / 100;
				sNumero = dNumero.ToString();
				iNumero = int.Parse (sNumero[0].ToString());
				sTexto  +=  this.sCentenas [iNumero] + " ";
			}
 
			dCociente = dCociente % 100;
			if (dCociente >= 30){
				dNumero = dCociente / 10;			
				sNumero = dNumero.ToString();
				iNumero = int.Parse (sNumero[0].ToString());
				if (iNumero > 0)
					sTexto  += this.sDecenas [iNumero] + " ";
 
				dNumero = dCociente % 10;
				sNumero = dNumero.ToString();
				iNumero = int.Parse (sNumero[0].ToString());
				if (iNumero > 0)
					sTexto  += "y " + this.sUnidades [iNumero] + " ";
			}else{
				dNumero = dCociente;	
				sNumero = dNumero.ToString();
				if (sNumero.Length > 1)
					if (sNumero[1] != '.')
						iNumero = int.Parse (sNumero[0].ToString() + sNumero[1].ToString());
					else
						iNumero = int.Parse (sNumero[0].ToString());
				else
					iNumero = int.Parse (sNumero[0].ToString());
				sTexto  += this.sUnidades[iNumero] + " ";
			}
			return sTexto;
		}		

		private string ObtenerDecimales (string sNumero)
		{
			string[] sNumPuntos;
			string sTexto = "";
			double dNumero = 0;
			sNumPuntos = sNumero.Split('.');
    		dNumero = Convert.ToDouble(sNumPuntos[1]);
			sTexto = " pesos "+dNumero.ToString().Trim()+"/100 M.N."; 
			//sTexto = "peso con " + Numeros(dNumero,1);
			return sTexto;
		}
	}
}			
