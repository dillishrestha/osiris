//////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Ing. Jesus Buentello (Programacion) gjuanzz@gmail.com 
//				  Ing. Daniel Olivares C. (Adecuaciones y mejoras) arcangeldoc@gmail.com 05/05/2007
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
// Proposito	: Impresion de listas de precios 
// Objeto		: rpt_lista_precios.cs
/////////////////////////////////////////////////////////
using System;
using Gtk;
using Npgsql;
using Glade;
using GtkSharp;

namespace osiris
{
	public class seleccion_paquete
	{
		[Widget] Gtk.Entry entry_atencion;
		[Widget] Gtk.Entry entry_paciente;
		[Widget] Gtk.Entry entry_doctor;
		[Widget] Gtk.Entry entry_edad;
		[Widget] Gtk.Entry entry_tel_pac;
		[Widget] Gtk.Entry entry_tel_doc;
		[Widget] Gtk.Entry entry_dir;
		[Widget] Gtk.Entry entry_paquete;
		[Widget] Gtk.Entry entry_presupuesto;
		[Widget] Gtk.Button button_busca1;
		[Widget] Gtk.Button button_busca2;
		[Widget] Gtk.CheckButton checkbutton_paquete;
		[Widget] Gtk.CheckButton checkbutton_presupuesto;
		
		[Widget] Gtk.Button button_imprimir;
		[Widget] Gtk.Button button_acepta;
		[Widget] Gtk.Button button_salir;
		
		public string connectionString = "Server=192.168.1.4;" +
							"Port=5432;" +
							"User ID=admin1;" +
							"Password=1qaz2wsx;";
		public string nombrebd;
	
    	    
    	//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		

		public seleccion_paquete(string _nombrebd_ )
		{
			
    		nombrebd = _nombrebd_; 
    		
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "seleccion_paquete", null);
			gxml.Autoconnect (this);
	        
			// Muestra ventana de Glade
			
			}
		}
		
	}	