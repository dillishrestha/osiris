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
using System.Collections;
using System.Text;

namespace osiris
{
	public class class_public
	{		
		public string LoginUsuario = "";
		public string NombrUsuario = "";
		public string idUsuario = "";
		
		// Informacion de la Empresa
		public string nombre_empresa = "P R A C T I M E D"; //"P R A C T I M E D"; "MEDICA NORESTE ION"
		public string nombre_empresa2 = "MEDICA NORESTE ION";
		public string direccion_empresa = "Loma Grande 2703, Col. Loma de San Francisco"; //"Loma Grande 2703, Col. Loma de San Francisco"; //"Jose Angel Conchello 2880, Col. Victora"
		public string telefonofax_empresa = "Telefono: (01)(81) 8040-6060"; //"Telefono: (01)(81) 8040-6060"; // "Telefono: (01)(81) 8351-3610"
		public string version_sistema = "Sistema Hospitalario OSIRIS ver. 1.0";
		
		public string ivaparaaplicar = "16.00";
		
		public int escala_linux_windows = 1;   // Linux = 1  Windows = 8
		public int horario_cita_inicio = 1;		// 7 am
		public int horario_cita_termino = 20;	// 8 pm
		public int horario_24_horas = 24; 		// media moche
		public int intervalo_minutos = 05;
		
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
		
		const int gray50_width = 2;
		const int gray50_height = 2;
		const string gray50_bits = "\x02\x01";
		
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
		
		/// <summary>
		/// Lee el ultimo numero de la tabla que se a creado
		/// </summary>
		/// <param name="name_table">Nombre de la tabla a buscar</param>
		/// <param name="name_field">Nombre del campo que va ser el ultimo numero</param>
		/// <param name="condition_table">Aplica una condicion a la busqueda del ultimo numero</param>
		/// <returns>Regresa el ultimo numero como cadena de caracteres</returns>
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
		
		public string nom_mes()
		{
			string mes = "";
			if (DateTime.Now.ToString("MM") == "01"){
				mes ="Enero";
			}
			if (DateTime.Now.ToString("MM") == "02"){
				mes ="Febrero";
			}
			if (DateTime.Now.ToString("MM") == "03"){
				mes ="Marzo";
			}
			if (DateTime.Now.ToString("MM") == "04"){
				mes ="Abril";
			}
			if (DateTime.Now.ToString("MM") == "05"){
				mes ="Mayo";
			}
			if (DateTime.Now.ToString("MM") == "06"){
				mes ="Junio";
			}
			if (DateTime.Now.ToString("MM") == "07"){
				mes ="Julio";
			}
			if (DateTime.Now.ToString("MM") == "08"){
				mes ="Agosto";
			}
			if (DateTime.Now.ToString("MM") == "09"){
				mes ="Septiembre";
			}
			if (DateTime.Now.ToString("MM") == "10"){
				mes ="Octubre";
			}
			if (DateTime.Now.ToString("MM") == "11"){
				mes ="Noviembre";
			}
			if (DateTime.Now.ToString("MM") == "12"){
				mes ="Diciembre";
			}
			return(mes);
		}
		
		public void CreateTags (TextBuffer buffer)
		{
			// Create a bunch of tags. Note that it's also possible to
			// create tags with gtk_text_tag_new() then add them to the
			// tag table for the buffer, gtk_text_buffer_create_tag() is
			// just a convenience function. Also note that you don't have
			// to give tags a name; pass NULL for the name to create an
			// anonymous tag.
			//
			// In any real app, another useful optimization would be to create
			// a GtkTextTagTable in advance, and reuse the same tag table for
			// all the buffers with the same tag set, instead of creating
			// new copies of the same tags for every buffer.
			//
			// Tags are assigned default priorities in order of addition to the
			// tag table.	 That is, tags created later that affect the same text
			// property affected by an earlier tag will override the earlier
			// tag.  You can modify tag priorities with
			// gtk_text_tag_set_priority().

			TextTag tag  = new TextTag ("heading");
			tag.Weight = Pango.Weight.Bold;
			tag.Size = (int) Pango.Scale.PangoScale * 15;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("italic");
			tag.Style = Pango.Style.Italic;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("bold");
			tag.Weight = Pango.Weight.Bold;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("big");
			tag.Size = (int) Pango.Scale.PangoScale * 20;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("xx-small");
			tag.Scale = Pango.Scale.XXSmall;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("x-large");
			tag.Scale = Pango.Scale.XLarge;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("monospace");
			tag.Family = "monospace";
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("blue_foreground");
			tag.Foreground = "blue";
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("red_background");
			tag.Background = "red";
			buffer.TagTable.Add (tag);

			// The C gtk-demo passes NULL for the drawable param, which isn't
			// multi-head safe, so it seems bad to allow it in the C# API.
			// But the Window isn't realized at this point, so we can't get
			// an actual Drawable from it. So we kludge for now.
			Pixmap stipple = Pixmap.CreateBitmapFromData (Gdk.Screen.Default.RootWindow, gray50_bits, gray50_width, gray50_height);

			tag  = new TextTag ("background_stipple");
			tag.BackgroundStipple = stipple;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("foreground_stipple");
			tag.ForegroundStipple = stipple;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("big_gap_before_line");
			tag.PixelsAboveLines = 30;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("big_gap_after_line");
			tag.PixelsBelowLines = 30;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("double_spaced_line");
			tag.PixelsInsideWrap = 10;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("not_editable");
			tag.Editable = false;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("word_wrap");
			tag.WrapMode = WrapMode.Word;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("char_wrap");
			tag.WrapMode = WrapMode.Char;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("no_wrap");
			tag.WrapMode = WrapMode.None;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("center");
			tag.Justification = Justification.Center;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("right_justify");
			tag.Justification = Justification.Right;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("wide_margins");
			tag.LeftMargin = 50;
			tag.RightMargin = 50;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("strikethrough");
			tag.Strikethrough = true;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("underline");
			tag.Underline = Pango.Underline.Single;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("double_underline");
			tag.Underline = Pango.Underline.Double;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("superscript");
			tag.Rise = (int) Pango.Scale.PangoScale * 10;
			tag.Size = (int) Pango.Scale.PangoScale * 8;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("subscript");
			tag.Rise = (int) Pango.Scale.PangoScale * -10;
			tag.Size = (int) Pango.Scale.PangoScale * 8;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("rtl_quote");
			tag.WrapMode = WrapMode.Word;
			tag.Direction = TextDirection.Rtl;
			tag.Indent = 30;
			tag.LeftMargin = 20;
			tag.RightMargin = 20;
			buffer.TagTable.Add (tag);
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
	
		/// <summary>
		/// Calcula el RFC de una persona física su homoclave incluida.
		/// </summary>
		/// <param name="nombre">Nombre(s) de la persona</param>
		/// <param name="apellidoPaterno">Apellido paterno de la persona</param>
		/// <param name="apellidoMaterno">Apellido materno de la persona</param>
		/// <param name="fecha">Fecha en formato dd/MM/yy (12/10/68)</param>
		/// <returns>Regresa el RFC como cadena de caracteres</returns>
		public string CalcularRFC(string nombre, string apellidoPaterno, string apellidoMaterno, string fecha)
		{
			//Cambiamos todo a mayúsculas
			nombre = nombre.ToUpper();
			apellidoPaterno = apellidoPaterno.ToUpper();
			apellidoMaterno = apellidoMaterno.ToUpper();
	
			//RFC que se regresará
			string rfc = String.Empty;
	
			//Quitamos los espacios al principio y final del nombre y apellidos
			nombre.Trim();
			apellidoPaterno = apellidoPaterno.Trim();
			apellidoMaterno = apellidoMaterno.Trim();
	
			//Quitamos los artículos de los apellidos
			apellidoPaterno = QuitarArticulos(apellidoPaterno);
			apellidoMaterno = QuitarArticulos(apellidoMaterno);
	
			//Agregamos el primer caracter del apellido paterno
			rfc = apellidoPaterno.Substring(0, 1);
	
			//Buscamos y agregamos al rfc la primera vocal del primer apellido
			foreach (char c in apellidoPaterno){
				if (EsVocal(c)){
					rfc += c;
					break;
				}
			}
	
			//Agregamos el primer caracter del apellido materno
			rfc += apellidoMaterno.Substring(0, 1);
	
			//Agregamos el primer caracter del primer nombre
			rfc += nombre.Substring(0, 1);
	
			//agregamos la fecha yymmdd (por ejemplo: 680825, 25 de agosto de 1968 )
			rfc += fecha.Substring(6, 2) +
			fecha.Substring(3, 2) +
			fecha.Substring(0, 2);
	
			//Le agregamos la homoclave al rfc
			CalcularHomoclave(apellidoPaterno + " " + apellidoMaterno + " " + nombre, fecha, ref rfc);
	
			return rfc;
		}
	
		/// <summary>
		/// Calcula la homoclave
		/// </summary>
		/// <param name="nombreCompleto">El nombre completo de la persona en el formato "ApellidoPaterno ApellidoMaterno Nombre(s)"</param>
		/// <param name="fecha">fecha en el formato "dd/MM/yy"</param>
		/// <param name="rfc">rfc sin homoclave, esta se pasa con ref y después de la función tendrá la homoclave</param>
		static private void CalcularHomoclave(string nombreCompleto, string fecha, ref string rfc)
		{
			//Guardara el nombre en su correspondiente numérico
			StringBuilder nombreEnNumero = new StringBuilder(); ;
			//La suma de la secuencia de números de nombreEnNumero
			long valorSuma = 0;
	
			#region Tablas para calcular la homoclave
			//Estas tablas realmente no se porque son como son
			//solo las copie de lo que encontré en internet
	
			#region TablaRFC 1
				Hashtable tablaRFC1 = new Hashtable();
				tablaRFC1.Add("&", 10);
				tablaRFC1.Add("Ñ", 10);
				tablaRFC1.Add("A", 11);
				tablaRFC1.Add("B", 12);
				tablaRFC1.Add("C", 13);
				tablaRFC1.Add("D", 14);
				tablaRFC1.Add("E", 15);
				tablaRFC1.Add("F", 16);
				tablaRFC1.Add("G", 17);
				tablaRFC1.Add("H", 18);
				tablaRFC1.Add("I", 19);
				tablaRFC1.Add("J", 21);
				tablaRFC1.Add("K", 22);
				tablaRFC1.Add("L", 23);
				tablaRFC1.Add("M", 24);
				tablaRFC1.Add("N", 25);
				tablaRFC1.Add("O", 26);
				tablaRFC1.Add("P", 27);
				tablaRFC1.Add("Q", 28);
				tablaRFC1.Add("R", 29);
				tablaRFC1.Add("S", 32);
				tablaRFC1.Add("T", 33);
				tablaRFC1.Add("U", 34);
				tablaRFC1.Add("V", 35);
				tablaRFC1.Add("W", 36);
				tablaRFC1.Add("X", 37);
				tablaRFC1.Add("Y", 38);
				tablaRFC1.Add("Z", 39);
				tablaRFC1.Add("0", 0);
				tablaRFC1.Add("1", 1);
				tablaRFC1.Add("2", 2);
				tablaRFC1.Add("3", 3);
				tablaRFC1.Add("4", 4);
				tablaRFC1.Add("5", 5);
				tablaRFC1.Add("6", 6);
				tablaRFC1.Add("7", 7);
				tablaRFC1.Add("8", 8);
				tablaRFC1.Add("9", 9);
			#endregion
	
			#region TablaRFC 2
				Hashtable tablaRFC2 = new Hashtable();
				tablaRFC2.Add(0, "1");
				tablaRFC2.Add(1, "2");
				tablaRFC2.Add(2, "3");
				tablaRFC2.Add(3, "4");
				tablaRFC2.Add(4, "5");
				tablaRFC2.Add(5, "6");
				tablaRFC2.Add(6, "7");
				tablaRFC2.Add(7, "8");
				tablaRFC2.Add(8, "9");
				tablaRFC2.Add(9, "A");
				tablaRFC2.Add(10, "B");
				tablaRFC2.Add(11, "C");
				tablaRFC2.Add(12, "D");
				tablaRFC2.Add(13, "E");
				tablaRFC2.Add(14, "F");
				tablaRFC2.Add(15, "G");
				tablaRFC2.Add(16, "H");
				tablaRFC2.Add(17, "I");
				tablaRFC2.Add(18, "J");
				tablaRFC2.Add(19, "K");
				tablaRFC2.Add(20, "L");
				tablaRFC2.Add(21, "M");
				tablaRFC2.Add(22, "N");
				tablaRFC2.Add(23, "P");
				tablaRFC2.Add(24, "Q");
				tablaRFC2.Add(25, "R");
				tablaRFC2.Add(26, "S");
				tablaRFC2.Add(27, "T");
				tablaRFC2.Add(28, "U");
				tablaRFC2.Add(29, "V");
				tablaRFC2.Add(30, "W");
				tablaRFC2.Add(31, "X");
				tablaRFC2.Add(32, "Y");
			#endregion
	
			#region TablaRFC 3
				Hashtable tablaRFC3 = new Hashtable();
				tablaRFC3.Add("A", 10);
				tablaRFC3.Add("B", 11);
				tablaRFC3.Add("C", 12);
				tablaRFC3.Add("D", 13);
				tablaRFC3.Add("E", 14);
				tablaRFC3.Add("F", 15);
				tablaRFC3.Add("G", 16);
				tablaRFC3.Add("H", 17);
				tablaRFC3.Add("I", 18);
				tablaRFC3.Add("J", 19);
				tablaRFC3.Add("K", 20);
				tablaRFC3.Add("L", 21);
				tablaRFC3.Add("M", 22);
				tablaRFC3.Add("N", 23);
				tablaRFC3.Add("O", 25);
				tablaRFC3.Add("P", 26);
				tablaRFC3.Add("Q", 27);
				tablaRFC3.Add("R", 28);
				tablaRFC3.Add("S", 29);
				tablaRFC3.Add("T", 30);
				tablaRFC3.Add("U", 31);
				tablaRFC3.Add("V", 32);
				tablaRFC3.Add("W", 33);
				tablaRFC3.Add("X", 34);
				tablaRFC3.Add("Y", 35);
				tablaRFC3.Add("Z", 36);
				tablaRFC3.Add("0", 0);
				tablaRFC3.Add("1", 1);
				tablaRFC3.Add("2", 2);
				tablaRFC3.Add("3", 3);
				tablaRFC3.Add("4", 4);
				tablaRFC3.Add("5", 5);
				tablaRFC3.Add("6", 6);
				tablaRFC3.Add("7", 7);
				tablaRFC3.Add("8", 8);
				tablaRFC3.Add("9", 9);
				tablaRFC3.Add("", 24);
				tablaRFC3.Add(" ", 37);
			#endregion
	
			#endregion
	
			//agregamos un cero al inicio de la representación númerica del nombre
			nombreEnNumero.Append("0");
	
			//Recorremos el nombre y vamos convirtiendo las letras en
			//su valor numérico
			foreach (char c in nombreCompleto){
				if (tablaRFC1.ContainsKey(c.ToString()))
					nombreEnNumero.Append(tablaRFC1[c.ToString()].ToString());
				else
					nombreEnNumero.Append("00");
			}
	
			//Calculamos la suma de la secuencia de números
			//calculados anteriormente
			//la formula es:
			//( (el caracter actual multiplicado por diez)
			//mas el valor del caracter siguiente )
			//(y lo anterior multiplicado por el valor del caracter siguiente)
			for (int i = 0; i < nombreEnNumero.Length - 1; i++){
				valorSuma += ((Convert.ToInt32(nombreEnNumero[i].ToString()) * 10) + Convert.ToInt32(nombreEnNumero[i + 1].ToString())) * Convert.ToInt32(nombreEnNumero[i + 1].ToString());
			}
	
			//Lo siguiente no se porque se calcula así, es parte del algoritmo.
			//Los magic numbers que aparecen por ahí deben tener algún origen matemático
			//relacionado con el algoritmo al igual que el proceso mismo de calcular el
			//digito verificador.
			//Por esto no puedo añadir comentarios a lo que sigue, lo hice por acto de fe.
	
			int div = 0, mod = 0;
			div = Convert.ToInt32(valorSuma) % 1000;
			mod = div % 34;
			div = (div - mod) / 34;
	
			int indice = 0;
			string hc = String.Empty; //los dos primeros caracteres de la homoclave
			while (indice <= 1){
				if (tablaRFC2.ContainsKey((indice == 0) ? div : mod))
					hc += tablaRFC2[(indice == 0) ? div : mod];
				else
					hc += "Z";
				indice++;
			}
	
			//Agregamos al RFC los dos primeros caracteres de la homoclave
			rfc += hc;
	
			//Aqui empieza el calculo del digito verificador basado en lo que tenemos del RFC
			//En esta parte tampoco conozco el origen matemático del algoritmo como para dar
			//una explicación del proceso, así que ¡tengamos fe hermanos!.
			int rfcAnumeroSuma = 0, sumaParcial = 0;
			for (int i = 0; i < rfc.Length; i++){
				if (tablaRFC3.ContainsKey(rfc[i].ToString())){
					rfcAnumeroSuma = Convert.ToInt32(tablaRFC3[rfc[i].ToString()]);
					sumaParcial += (rfcAnumeroSuma * (14 - (i + 1)));
				}
			}
	
			int moduloVerificador = sumaParcial % 11;
			if (moduloVerificador == 0)
				rfc += "0";
			else{
				sumaParcial = 11 - moduloVerificador;
				if (sumaParcial == 10)
					rfc += "A";
				else
					rfc += sumaParcial.ToString();
			}
	
			//en este punto la variable rfc pasada ya debe tener la homoclave
			//recuerda que la variable rfc se paso como "ref string" lo cual
			//hace que se modifique la original.
		}
	
		/// <summary>
		/// Verifica si el caracter pasado es una vocal
		/// </summary>
		/// <param name="letra">Caracter a comprobar</param>
		/// <returns>Regresa true si es vocal, de lo contrario false</returns>
		static private bool EsVocal(char letra)
		{
			//Aunque para el caso del RFC cambié todas las letras a mayúsculas
			//igual agregé las minúsculas.
			if (letra == 'A' || letra == 'E' || letra == 'I' || letra == 'O' || letra == 'U' ||
				letra == 'a' || letra == 'e' || letra == 'i' || letra == 'o' || letra == 'u')
				return true;
			else
				return false;
		}
	
		/// <summary>
		/// Remplaza los artículos comúnes en los apellidos en México con caracter vacío (String.Empty).
		/// </summary>
		/// <param name="palabra">Palabra que se le quitaran los artículos</param>
		/// <returns>Regresa la palabra sin los artículos</returns>
		static private string QuitarArticulos(string palabra)
		{
			return palabra.Replace("DEL ", String.Empty).Replace("LAS ", String.Empty).Replace("DE ", String.Empty).Replace("LA ", String.Empty).Replace("Y ", String.Empty).Replace("A ", String.Empty);
		}
	}
}