// rpt_reservacion_paquetes.cs created with MonoDevelop
// User: ipena at 06:19 p 10/06/2008
//
// Autor    	: Israel Peña Gonzalez - el_rip@hotmail.com (Programacion Mono)
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using Gnome;
using System.Collections;
using GtkSharp;

namespace osiris
{
	public class rpt_reservacion_paquete
	{
		public string nombrebd;
		public string LoginEmpleado;
		public string NomEmp_;
		public string NomEmpleado = "";
		public string AppEmpleado = "";
		public string ApmEmpleado = "";
		
		public string entry_dia1;
		public string entry_mes1;
		public string entry_anno1;
		public string entry_nombre_paciente;
		public string entry_paq_pres;
		public string entry_precio_paquete;
		public string cantidad_en_letras;
		
		public string connectionString = "Server=localhost;" +
						"Port=5432;" +
						 "User ID=admin;" +
						"Password=1qaz2wsx;";
		
		// traductor de numeros a letras
		public string[] sUnidades = {"", "un", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "diez", 
									"once", "doce", "trece", "catorce", "quince", "dieciseis", "diecisiete", "dieciocho", "diecinueve", "veinte", 
									"veintiún", "veintidos", "veintitres", "veinticuatro", "veinticinco", "veintiseis", "veintisiete", "veintiocho", "veintinueve"};
 		public string[] sDecenas = {"", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa"};
 		public string[] sCentenas = {"", "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos"};
  		public string sResultado = "";
		
		// Declarando variable de fuente para la impresion
		Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
			
		
		public rpt_reservacion_paquete(string entry_dia1_,string entry_mes1_,string entry_anno1_,string entry_nombre_paciente_,string entry_paq_pres_,string entry_precio_paquete_,string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_) 
		{
			entry_dia1 = entry_dia1_;
			entry_mes1 = entry_mes1_;
			entry_anno1 = entry_anno1_;
			entry_nombre_paciente = entry_nombre_paciente_;
			entry_paq_pres = entry_paq_pres_;
			entry_precio_paquete = entry_precio_paquete_;
			LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		nombrebd = _nombrebd_;
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob(Gnome.PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog(trabajo, "RESERVACION", 0);
        	
        	int respuesta = dialogo.Run ();
        	
        	if (respuesta == (int) PrintButtons.Cancel) 
			{
				dialogo.Hide (); 
				dialogo.Dispose (); 
				return;
			}
			Gnome.PrintContext ctx = trabajo.Context;
        	ComponerPagina(ctx, trabajo); 
			trabajo.Close();
            switch (respuesta)
        	{
				case (int) PrintButtons.Print:   
                trabajo.Print (); 
                break;
                case (int) PrintButtons.Preview:
                new PrintJobPreview(trabajo, "RESERVACION").Show();
                break;
        	}
			dialogo.Hide (); dialogo.Dispose ();		
		}
		
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{ 
			ContextoImp.BeginPage("Pagina 1");
			
           	cantidad_en_letras = traduce_numeros(entry_precio_paquete);
		 	float preciopaqpresu = float.Parse(entry_precio_paquete);
			
			Gnome.Print.Setfont (ContextoImp, fuente2);          //Nombre del Usuario y/o demandante del servicio:  
			//ContextoImp.MoveTo(20, 710);         ContextoImp.Show("                                                                                                                           "                         +        entry_nombre_paciente);
			ContextoImp.MoveTo(325, 709);         ContextoImp.Show(entry_nombre_paciente);
			                                                     //nombre del paquete: 
			ContextoImp.MoveTo(190, 689);         ContextoImp.Show(entry_paq_pres);
			                                                     //v.-precio y forma de pago: $                                      +
			ContextoImp.MoveTo(195, 548);         ContextoImp.Show(preciopaqpresu.ToString("C"));                                                       //  + entry_precio_paquete);
			                                                     //(________________________________)
			ContextoImp.MoveTo(65, 538.6);       ContextoImp.Show(          cantidad_en_letras ); 
			                                                     //En Monterrey N.L. a                    de       "             del
			ContextoImp.MoveTo(392, 231);        ContextoImp.Show(entry_dia1);   
			ContextoImp.MoveTo(440, 231);        ContextoImp.Show(entry_mes1);
			ContextoImp.MoveTo(521, 231);        ContextoImp.Show(entry_anno1.Substring(3,1));
			
			
			ContextoImp.ShowPage();							
					
		}
		
		public string traduce_numeros (string sNumero) {
			double dNumero;
			double dNumAux = 0;
			char x;
			string sAux;
			
			sResultado = " ";
			try {
				dNumero = Convert.ToDouble (sNumero);
			}
			catch {				
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

		private string ObtenerDecimales (string sNumero) {
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
