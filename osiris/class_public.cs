//////////////////////////////////////////////////////////
// project created on 04/01/2010 at 10:20 a   
// Monterrey - Mexico
// 
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
//				  Daniel Olivares - arcangeldoc@gmail.com (Diseño de Pantallas Glade)
// 				  
// Licencia		: GLP
// S.O. 		: GNU/Linux
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
using Gtk;
using Gdk;
using System;
using Glade;
using Npgsql;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace osiris
{
	public class class_public
	{		
		public string LoginUsuario = "";
		public string NombrUsuario = "";
		public string idUsuario = "";
		
		// Informacion de la Empresa
		public string nombre_empresa = "P R A C T I M E D"; //"P R A C T I M E D"; "MEDICA NORESTE ION"
		public string direccion_empresa = "Loma Grande 2703, Col. Loma de San Francisco"; //"Loma Grande 2703, Col. Loma de San Francisco"; //"Jose Angel Conchello 2880, Col. Victora"
		public string telefonofax_empresa = "Telefono (01)(81) 8040-6060"; //"Telefono (01)(81) 8040-6060"; // "Telefono: (01)(81) 8351-3610"
		public string version_sistema = "Sistema Hospitalario OSIRIS ver. 1.0";
		
		public string ivaparaaplicar = "16.00";
		
		public int escala_linux_windows = 1;   // Linux = 1  Windows = 8
		public int horario_cita_inicio = 7;		// 7 am
		public int horario_cita_termino = 20;	// 8 pm
		public int intervalo_minutos = 10;
		
		// variable para la conexion---> los valores estan en facturador.cs
		string connectionString = "";
		string nombrebd = "";
		
		class_conexion conexion_a_DB = new class_conexion();
		
		//Declaracion de ventana de error y mensaje
		protected Gtk.Window MyWinError;
		
		// Cantidad en Letras
		private string[] sUnidades = {"", "un", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "diez", 
									"once", "doce", "trece", "catorce", "quince", "dieciseis", "diecisiete", "dieciocho", "diecinueve", "veinte", 
									"veintiún", "veintidos", "veintitres", "veinticuatro", "veinticinco", "veintiseis", "veintisiete", "veintiocho", "veintinueve"};		
		private string[] sDecenas = {"", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa"};		
		private string[] sCentenas = {"","cien", "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos"};		
		
		private string sResultado = "";
		
		// Funcion de Encriptacion en MD5 para las contraseñas de usuarios
		public string CreatePasswordMD5(string password)
		{
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] bs = System.Text.Encoding.UTF8.GetBytes(password);
			bs = md5.ComputeHash(bs);
			System.Text.StringBuilder s = new System.Text.StringBuilder();
			foreach (byte b in bs){
				s.Append(b.ToString("x2").ToLower());
			}
			return s.ToString();			
		}
		
		public string lee_ultimonumero_registrado(string name_table,string name_field,string condition_table)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			string tomavalor = "1";
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char("+name_field+",'9999999999') AS field_last_number FROM "+name_table+" "+condition_table+" ORDER BY "+name_field+" DESC LIMIT 1;";
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();				
				if (lector.Read()){	
					tomavalor = (int.Parse((string) lector["field_last_number"])+1).ToString();
					conexion.Close();
					return tomavalor;					
				}else{
					conexion.Close();
					return tomavalor;					
				}
			}catch (NpgsqlException ex){
		   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();			msgBoxError.Destroy();
				conexion.Close();
				return tomavalor;
			}
		}
		
		public string ConvertirCadena (string sNumero, string descriptipomoneda_) {
			double dNumero;
			double dNumAux = 0;
			char x;
			string sAux;			
			
			sResultado = " ";
			try {
				dNumero = Convert.ToDouble (sNumero);
			}catch{				
				return "";
			}
			
			if (dNumero > 999999999999)
				return "";
			
			if (dNumero > 999999999){
				dNumAux = dNumero % 1000000000000;
				sResultado += Numeros (dNumAux, 1000000000) + " mil ";
			}
			
			if (dNumero > 999999){
				dNumAux = dNumero % 1000000000;
                if (dNumero <= 1999999 & dNumero >= 1000000)
                    sResultado += Numeros(dNumAux, 1000000) + " millon ";
                else
				    sResultado += Numeros (dNumAux, 1000000) + " millones ";
			}
			
			if (dNumero > 999) {
				dNumAux = dNumero % 1000000;
                if (dNumAux >= 1000){
                    if (dNumero < 2000){
                        sResultado += "mil ";
                    }else{
                        sResultado += Numeros(dNumAux, 1000) + " mil ";
                    }
                }
			}
			
			dNumAux = dNumero % 1000;	
			sResultado += Numeros (dNumAux, 1);
			
			
			//Enseguida verificamos si contiene punto, si es así, los convertimos a texto.
			sAux = dNumero.ToString();

            if (sAux.IndexOf(".") >= 0){
                sResultado += descriptipomoneda_+" "+"con " + ObtenerDecimales(sAux);
            }else{
                sResultado += descriptipomoneda_+" "+"con 00/100";
            }
			
			//Las siguientes líneas convierten el primer caracter a mayúscula.
			sAux = sResultado;
			x = char.ToUpper (sResultado[1]);
			sResultado = x.ToString ();
			
			for (int i = 2; i<sAux.Length; i++)
				sResultado += sAux[i].ToString();
			
			return Regex.Replace(sResultado,"  *"," ").Trim();
		}		
		
		private string ConvertirCadena_ (double dNumero) {
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
			
			if (dNumero > 999999){
				dNumAux = dNumero % 1000000000;
				sResultado += Numeros (dNumAux, 1000000) + " millones ";
			}
			
			if (dNumero > 999){
				dNumAux = dNumero % 1000000;
				sResultado += Numeros (dNumAux, 1000) + " mil ";
			}
			
			dNumAux = dNumero % 1000;	
			sResultado += Numeros (dNumAux, 1);
			
			
			//Enseguida verificamos si contiene punto, si es así, los convertimos a texto.
			sAux = dNumero.ToString();

            if (sAux.IndexOf(".") >= 0){
                sResultado += "con " + ObtenerDecimales(sAux);
            }else{
                sResultado += "con 00/100";
            }
			
			//Las siguientes líneas convierten el primer caracter a mayúscula.
            sResultado = sResultado.TrimStart();
            sResultado = sResultado.TrimEnd();
			sAux = sResultado;
			x = char.ToUpper (sResultado[0]);
			sResultado = x.ToString ();
			
			for (int i = 1; i<sAux.Length; i++)
				sResultado += sAux[i].ToString();
			
			return sResultado;
		}
		
		private string Numeros (double dNumAux, double dFactor) {
			double dCociente = dNumAux / dFactor;
			double dNumero = 0;
			int iNumero = 0;
			string sNumero = "";
			string sTexto = "";

            if (dCociente < 101 & dCociente >=100){
                dNumero = dCociente / 100;
                sNumero = dNumero.ToString();
                iNumero = int.Parse(sNumero[0].ToString());
                sTexto += this.sCentenas[iNumero] + " ";
            }

			if (dCociente >= 101){
				dNumero = dCociente / 100;
				sNumero = dNumero.ToString();
				iNumero = int.Parse (sNumero[0].ToString());
				sTexto  +=  this.sCentenas [iNumero+1] + " ";
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
                	if (iNumero > 0){
                    	if (iNumero == 1){
                        	sTexto += "y uno ";
                    	}else{
                        	sTexto += "y " + this.sUnidades[iNumero] + " ";
                        
                    	}
                	}
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
                	if (iNumero == 1){
                    	if (dNumAux <= 1999999 & dNumAux >= 1000000)
                        	sTexto += "un ";
                    	else
                        	sTexto += " ";
                	}else
                    	sTexto += this.sUnidades[iNumero] + " ";
                    
			}			
			return sTexto;
		}			

		private string ObtenerDecimales (string sNumero) {
			string[] sNumPuntos;
			string sTexto = "";
			double dNumero = 0;
			double dauxNumero =0;
			dauxNumero = double.Parse(sNumero);

            sNumero = dauxNumero.ToString("####0.00");
			sNumPuntos = sNumero.Split('.');
						
			dNumero = Convert.ToDouble(sNumPuntos[1]);
			//sTexto = "punto " + Numeros(dNumero,1);
			sTexto = sNumPuntos[1]+"/100";
			
			return sTexto;
		}	
	}
}