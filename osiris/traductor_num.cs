using System;
using System.Globalization;

namespace osiris
{
	public class traduce_numero_letras
	{
		const string ZERO = "Cero";

		string IntegerPart = ZERO;
		string RealPart = ZERO;

		string _DecimalSeparator;
		string _GroupSeparator;

		string[] aT0 = new string[] {"Uno", "Dos", "Tres", "Cuatro", "Cinco", "Seis", "Siete", "Ocho", "Nueve"};
		string[] aT1 = new string[] {"Diez", "Once", "Doce", "Trece", "Catorce", "Quince", "Diesiseis", "Diesisiete", "Diesiocho", "Diesinueve"};
		string[] aT2 = new string[] {"Veinte", "Treinta", "Cuarenta", "Cincuenta", "Sesente", "Setenta", "Ochenta", "Noventa" };
		string[] aT3 = new string[] {"", "", " Mil ", " Million ", " Billion ", "Trillion ", "", "", "", "" };

    	public traduce_numero_letras()
    	{
			_DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
			_GroupSeparator =  CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
		}

		public string IntegerToWords(string Number)
		{
			RealToWords(Number);
			return IntegerPart;
		}

		public string RealToWords(string Number)
		{
			int DotPos = 0;

			IntegerPart = ZERO;
			RealPart = ZERO;

			if (IsNumeric(Number)){
				Number = Number.Replace(_GroupSeparator, "");
				DotPos = Number.IndexOf(_DecimalSeparator);
				if (DotPos >= 0){
					IntegerPart = ToWords(Convert.ToInt32(Number.Substring(0, DotPos)));
					RealPart = ToWords(Convert.ToInt32(Number.Substring(DotPos + 1, Number.Length - DotPos - 1)));
				}else{
					IntegerPart = ToWords(Convert.ToInt32(Number));
				}
			}
			return IntegerPart + " Dot " + RealPart;
		}

		public string CurrencyToWords(string Number, string MoneyName)
		{
			// string s = "";

			IntegerPart = ZERO;
			RealPart = ZERO;

			if(IsNumeric(Number)){
				// round to money number
				string s = RealToWords(RoundNumberString(Convert.ToDouble(Number), 2));
			}
			// add money text
			if (IntegerPart == ZERO) {IntegerPart = "No";}
			switch(RealPart){
				case ZERO:
					RealPart = "Ningun Centavo";
					break;
				case "Uno":
					RealPart = "y Un Centavo";
					break;
				default:
					RealPart = "y " + RealPart + " Centavos";
					break;
			}
			return IntegerPart + " " + MoneyName + " " + RealPart;
		}

		private string ToWords(int Number)
		{
			string s = "";
			string d = "";
			string r = "";
			int n;

			s = Number.ToString();
			n = 1;
			while (s.Length != 0)
			{
				// convert last 3 digits of s to English r.
				if (s.Length < 3) { d = ConvertHundreds(s); }
				else { d = ConvertHundreds(s.Substring(s.Length - 3, 3)); }
				if (d.Length > 0) { r = d + aT3[n] + r; }
				if (s.Length > 3) { s = s.Substring(0, s.Length - 3); }
				else { s = ""; }
				n++;
			}
			if (r.Length == 0) { r = ZERO; }
			return r;
		}
		
		private string ConvertHundreds(string pNumber)
		{
			string rtn = "";

			if (!(Convert.ToInt32(pNumber) == 0))
			{
				// append leading zeros to number
				pNumber = ("000" + pNumber).Substring(pNumber.Length, 3);
				// do we have a hundreds place digit to convert?
				if (!(pNumber.Substring(0, 1) == "0")){
					rtn = ConvertDigit(pNumber.Substring(0, 1)) + " Hundred ";
				}
				// do we have a tens place digit to convert?
				if (pNumber.Length >= 2){
					if (!(pNumber.Substring(1, 1) == "0")){
						rtn += ConvertTens(pNumber.Substring(1));
					}else{
						rtn += ConvertDigit(pNumber.Substring(2));
					}
				}
				return rtn.Trim();
			}else { return ""; }
		}

		private string ConvertTens(string pTens)
		{
			string r = "";
			// is value between 10 and 19?
			if ((Convert.ToInt32(pTens.Substring(0, 1)) == 1)){
				r = aT1[Convert.ToInt32(pTens) - 10];
			}else{
				// otherwise it's between 20 and 99.
				r = aT2[Convert.ToInt32(pTens.Substring(0, 1)) - 2] + " ";
            // convert ones place digit
            r += ConvertDigit(pTens.Substring((pTens.Length - 1), 1));
        }
        return r;
    	}

		private string ConvertDigit(string pNumber)
		{
			if (pNumber == "0") { return ""; }
			else{ return aT0[Convert.ToInt32(pNumber) - 1]; }
		}

		private string RoundNumberString(double Number, int Decimals)
		{
			string s;
			double r;
			// round first
			r = Math.Pow(10d, Decimals);
			// Math.Floor make sure the float round. Suggested by Guillermo Som
			r = (int)(Math.Floor(Number * r + 0.5d)) / r;
			// complete with zeros
			s = r.ToString();
			if ((s.IndexOf(_DecimalSeparator) < 0)){
				s += _DecimalSeparator;
			}
			while ((s.Substring(s.IndexOf(_DecimalSeparator)).Length <= Decimals))
			{
				s += "0";
			}
        	return s;
		}

		// Source: http://aspalliance.com/articleViewer.aspx?aId=80&pId=
		// Review by Harvey Triana
		private bool IsNumeric(string s)
		{
			bool hasDecimal = false;
			bool r = false;
			char ds = Convert.ToChar(_DecimalSeparator);
			char gs = Convert.ToChar(_GroupSeparator);

			for (int i = 0; i < s.Length; i++){
				// check for decimal
				if (s[i] == ds){
					if (hasDecimal) // 2nd decimal
						r = false;
					else // 1st decimal
                	{
                    	// inform loop decimal found and continue
						hasDecimal = true;
						continue;
                	}
				}
				// check if number
				if (char.IsNumber(s[i]) || (s[i] == gs))
                	r = true;
				else
				{
					r = false;
                break;
				}
			}
		return r;
		}

		public string GroupSeparator
		{
			get { return _GroupSeparator; }
		}
		
		public string DecimalSeparator
		{
			get { return _DecimalSeparator; }
		}
	}
}