
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using System.Collections;
using System.IO;
using Gdk;

namespace osiris
{
	public class notas_medicas
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		[Widget] Gtk.Window window_demo = null;
		[Widget] Gtk.TextView textview1 = null;
		
		string connectionString;
		string nombrebd;
		
		const int gray50_width = 2;
		const int gray50_height = 2;
		const string gray50_bits = "\x02\x01";
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		public notas_medicas ()
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "window_demo", null);
			gxml.Autoconnect (this);
			window_demo.Show();
			window_demo.SetPosition(WindowPosition.Center);	// centra la ventana en la pantalla
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			textview1.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
			cadena_de_textos();
		}
		
		void cadena_de_textos()
		{
			TextBuffer buffer = new TextBuffer (null);
			buffer = textview1.Buffer;			
			TextIter insertIter = buffer.StartIter;
			CreateTags (buffer);
			
			string texto_hitoria_clinica = "";
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT * FROM osiris_his_informacion_medica;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if(lector.Read()){
					texto_hitoria_clinica = (string) lector["notas_de_enfermeria"];
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, 
						ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
	   			
	       	}
       		conexion.Close ();
			// Insertando texto al TextView
			buffer.Insert (ref insertIter, "Ejemplo de insertar un texto en un TextView.\n\n");
			// Cambia la letra a negrita en el Textview
			buffer.InsertWithTagsByName (ref insertIter, texto_hitoria_clinica+"\n", "bold");
			buffer.Insert (ref insertIter, texto_hitoria_clinica );
			buffer.InsertWithTagsByName (ref insertIter, "\nThis line has center justification.\n", "center");			
		}
		
		private void CreateTags (TextBuffer buffer)
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
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}
