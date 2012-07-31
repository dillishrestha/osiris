//////////////////////////////////////////////////////////////////////
// created on 19/04/2012 at 02:41 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. (Modificaciones y Ajustes)
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
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class rpt_conceptos_proveedores
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando la ventana
		[Widget] Gtk.Window reporte_proveedores = null;
		[Widget] Gtk.Entry entry_id_proveedor = null;
		[Widget] Gtk.Entry entry_dia_inicial = null;
		[Widget] Gtk.Entry entry_mes_inicial = null;
		[Widget] Gtk.Entry entry_ano_inicial = null;
		[Widget] Gtk.Entry entry_dia_final = null;
		[Widget] Gtk.Entry entry_mes_final = null;
		[Widget] Gtk.Entry entry_ano_final = null;
		[Widget] Gtk.CheckButton checkbutton_todos_proveedores = null;
		[Widget] Gtk.CheckButton checkbutton_todas_fechas = null;
		[Widget] Gtk.CheckButton checkbutton_todos_grupos = null;
		[Widget] Gtk.Entry entry_nombre_proveedor = null;
		[Widget] Gtk.Button button_busca_proveedor = null;
		[Widget] Gtk.TreeView treeview_lista_grupoprod = null;
		[Widget] Gtk.Button button_exportar_sheet = null;
		[Widget] Gtk.ComboBox combobox_herr_adicionales = null;
		[Widget] Gtk.CheckButton checkbutton_herr_adicionales = null;
		
		string connectionString = "";
		string nombrebd = "";
		string query_rango_fechas = "AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+DateTime.Now.ToString("yyyy")+"-"+DateTime.Now.ToString("MM")+"-"+DateTime.Now.ToString("dd")+"' "+
									"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+DateTime.Now.ToString("yyyy")+"-"+DateTime.Now.ToString("MM")+"-"+DateTime.Now.ToString("dd")+"' "; 
		string query_rango_fechas2 = "AND to_char(osiris_erp_factura_compra_enca.fechahora_creacion,'yyyy') >= '"+DateTime.Now.ToString("yyyy")+"' "+
									 "AND to_char(osiris_erp_factura_compra_enca.fechahora_creacion,'yyyy') <= '"+DateTime.Now.ToString("yyyy")+"' " +
									 "AND to_char(osiris_erp_factura_compra_enca.fechahora_creacion,'MM') >= '"+DateTime.Now.ToString("MM")+"' "+
									 "AND to_char(osiris_erp_factura_compra_enca.fechahora_creacion,'MM') <= '"+DateTime.Now.ToString("MM")+"' ";
		string query_proveedores = "";
		string query_in_grupo = "";
		string query_consulta = "";
		
		string[] args_args = {""};
		string[] args_herr_adicional = {"","ANALISIS COMPRAS PRODUCTOS"};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20};
		
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 80;
		int separacion_linea = 10;
		int numpage = 1;
		Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");
		
		PrintContext context;
		
		TreeStore treeViewEngineListaGrupoProd;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();
		
		public rpt_conceptos_proveedores ()
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			
			Glade.XML gxml = new Glade.XML (null, "impr_documentos.glade", "reporte_proveedores", null);
			gxml.Autoconnect(this);
			reporte_proveedores.Show();
			entry_dia_inicial.Text = DateTime.Now.ToString("dd");
			entry_mes_inicial.Text = DateTime.Now.ToString("MM");
			entry_ano_inicial.Text = DateTime.Now.ToString("yyyy");			
			entry_dia_final.Text = DateTime.Now.ToString("dd");
			entry_mes_final.Text = DateTime.Now.ToString("MM");
			entry_ano_final.Text = DateTime.Now.ToString("yyyy");
			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_busca_proveedor.Clicked += new EventHandler(on_busca_proveedores_clicked);
			checkbutton_todas_fechas.Clicked += new EventHandler(on_checkbutton_todas_fechas_clicked);
			checkbutton_todos_proveedores.Clicked += new EventHandler(on_checkbutton_todos_proveedore_clicked);
			checkbutton_todos_grupos.Clicked += new EventHandler(on_checkbutton_todos_grupos_clicked);
			checkbutton_herr_adicionales.Clicked += new EventHandler(on_checkbutton_herr_adicionales_clicked);
			button_exportar_sheet.Clicked += new EventHandler(on_button_exportar_sheet_clicked);
			entry_id_proveedor.KeyPressEvent += onKeyPressEvent_enter;
			checkbutton_todos_proveedores.Active = true;
			combobox_herr_adicionales.Sensitive = false;
			creartreeviewgrupoprod();
			llenadogrupoprod();
			checkbutton_todos_grupos.Active = true;
		}
		
		void llenado_combobox(int tipodellenado,string descrip_defaul,object obj,string sql_or_array,string query_SQL,string name_field_desc,string name_field_id,string[] args_array,int[] args_id_array,string name_field_id2)
		{			
			Gtk.ComboBox combobox_llenado = (Gtk.ComboBox) obj;
			//Gtk.ComboBox combobox_pos_neg = obj as Gtk.ComboBox;
			combobox_llenado.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_llenado.PackStart(cell, true);
			combobox_llenado.AddAttribute(cell,"text",0);	        
			ListStore store = new ListStore( typeof (string),typeof (int),typeof (int));
			combobox_llenado.Model = store;			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) descrip_defaul,0);
			}			
			if(sql_or_array == "array"){			
				for (int colum_field = 0; colum_field < args_array.Length; colum_field++){
					store.AppendValues (args_array[colum_field],args_id_array[colum_field],0);
				}
			}
			if(sql_or_array == "sql"){			
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
	            // Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
	               	comando.CommandText = query_SQL;
					NpgsqlDataReader lector = comando.ExecuteReader ();
	               	while (lector.Read()){
						if(name_field_id2 == ""){
							store.AppendValues ((string) lector[ name_field_desc ], (int) lector[ name_field_id],0);
						}else{
							store.AppendValues ((string) lector[ name_field_desc ], (int) lector[ name_field_id],(int) lector[ name_field_id2]);
						}
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
				conexion.Close ();
			}			
			TreeIter iter;
			if (store.GetIterFirst(out iter)){
				combobox_llenado.SetActiveIter (iter);
			}
			combobox_llenado.Changed += new EventHandler (onComboBoxChanged_llenado);			
		}
		
		void onComboBoxChanged_llenado (object sender, EventArgs args)
		{
			ComboBox onComboBoxChanged = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (onComboBoxChanged.GetActiveIter (out iter)){
				switch (onComboBoxChanged.Name.ToString()){	
				case "combobox_herr_adicionales":
					break;
				}
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza en ventana
		// Principal cuando selecciona el folio de productos
		// Ademas controla la tecla ENTRER para ver el procedimiento
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				entry_nombre_proveedor.Text = classpublic.lee_registro_de_tabla("osiris_erp_proveedores","id_proveedor"," WHERE osiris_erp_proveedores.id_proveedor = '"+entry_id_proveedor.Text.Trim()+"' AND proveedor_activo = 'true' ","descripcion_proveedor","int");
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}
		
		void on_button_exportar_sheet_clicked(object sender, EventArgs args)
		{
			comienzo_linea = 80;
			separacion_linea = 10;
			numpage = 1;
			if ((bool)checkbutton_herr_adicionales.Active == true){
				verifica_checkbutton_fechas();
				query_consulta = "SELECT osiris_erp_requisicion_deta.id_producto AS idproducto_osiris,descripcion_producto," +
					"to_char(osiris_erp_factura_compra_enca.fecha_factura,'yyyy') AS ano," +
					"to_char(osiris_erp_factura_compra_enca.fecha_factura,'MM') AS mes," +
					"AVG(osiris_erp_requisicion_deta.costo_producto) AS avg," +
					"MAX(osiris_erp_requisicion_deta.costo_producto) AS max," +
					"MIN(osiris_erp_requisicion_deta.costo_producto) AS min," +
					"COUNT(*) AS Total,SUM(osiris_erp_requisicion_deta.costo_producto) AS Total1 " +
					"FROM osiris_erp_requisicion_deta,osiris_erp_factura_compra_enca,osiris_productos " +
					"WHERE osiris_erp_requisicion_deta.numero_factura_proveedor = osiris_erp_factura_compra_enca.numero_factura_proveedor " +
					"AND osiris_productos.id_producto = osiris_erp_requisicion_deta.id_producto " +
					query_rango_fechas2+
					"GROUP BY osiris_erp_requisicion_deta.id_producto,descripcion_producto," +
					"to_char(osiris_erp_factura_compra_enca.fecha_factura,'yyyy')," +
					"to_char(osiris_erp_factura_compra_enca.fecha_factura,'MM') " +
					"ORDER BY descripcion_producto,to_char(osiris_erp_factura_compra_enca.fecha_factura,'yyyy')," +
					"to_char(osiris_erp_factura_compra_enca.fecha_factura,'MM');";
				// cambiar el query si elije por provedores
				// desabiliar los 
				print = new PrintOperation ();			
				print.JobName = "Analisis de Costos";	// Name of the report
				print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
				print.DrawPage += new DrawPageHandler (OnDrawPage);
				print.EndPrint += new EndPrintHandler (OnEndPrint);
				print.Run(PrintOperationAction.PrintDialog, null);
				
			}else{
				verifica_checkbutton_prov();
				verifica_checkbutton_fechas();
				verifica_grupo_prodctos();
				query_consulta = "SELECT to_char(osiris_erp_factura_compra_enca.fecha_factura,'yyyy-MM-dd') AS fechafactura,osiris_erp_factura_compra_enca.numero_factura_proveedor AS numerofactura," +
					"osiris_erp_factura_compra_enca.id_proveedor,descripcion_proveedor," +
					"to_char(osiris_erp_requisicion_deta.id_producto,'999999999999') AS idproducto_osiris,descripcion_producto AS descrip_prod_osiris," +
					"osiris_erp_requisicion_deta.precio_producto_publico,costo_producto_osiris,osiris_erp_requisicion_deta.precio_producto_publico AS costo_unitario_osiris,cantidad_de_embalaje_osiris,cantidad_comprada," +
					"osiris_erp_requisicion_deta.costo_producto AS costo_producto_compra,cantidad_recibida,osiris_erp_requisicion_deta.costo_por_unidad AS costoxunidad_compra,osiris_erp_requisicion_deta.cantidad_de_embalaje," +
					"osiris_erp_requisicion_deta.precio_costo_prov_selec AS precio_prove,id_producto_proveedor,descripcion_producto_proveedor,osiris_erp_requisicion_deta.tipo_unidad_producto,lote_producto,caducidad_producto " +
					"FROM osiris_erp_factura_compra_enca,osiris_erp_proveedores,osiris_erp_requisicion_deta,osiris_productos " +
					"WHERE osiris_erp_factura_compra_enca.id_proveedor = osiris_erp_proveedores.id_proveedor " +
					"AND osiris_erp_factura_compra_enca.numero_factura_proveedor = osiris_erp_requisicion_deta.numero_factura_proveedor " +
					"AND osiris_erp_requisicion_deta.id_producto = osiris_productos.id_producto " +
					query_proveedores+
					query_rango_fechas+
					query_in_grupo+
					"ORDER BY to_char(osiris_erp_factura_compra_enca.fecha_factura,'yyyy-MM-dd'),numerofactura;";
				string[] args_names_field = {"fechafactura","numerofactura","descripcion_proveedor","idproducto_osiris","descrip_prod_osiris","cantidad_comprada","costo_producto_compra","cantidad_de_embalaje","costoxunidad_compra","cantidad_recibida","costo_producto_osiris","cantidad_de_embalaje_osiris","costo_unitario_osiris","id_producto_proveedor","descripcion_producto_proveedor","tipo_unidad_producto","lote_producto","caducidad_producto"};
				string[] args_type_field = {"string","string","string","string","string","float","float","float","float","float","float","float","float","string","string","string","string","string"};
				
				// class_crea_ods.cs
				new osiris.class_traslate_spreadsheet(query_consulta,args_names_field,args_type_field);
			}
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
			context = args.Context;		
			ejecutar_consulta_reporte(context);
		}
						
		void ejecutar_consulta_reporte(PrintContext context)
		{
			string codigoproducto = "";
			string descriproducto = "";
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = query_consulta;
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	if(lector.Read()){
					imprime_encabezado(cr,layout);
					desc = Pango.FontDescription.FromString ("Sans");
					fontSize = 6.0;			layout = null;			layout = context.CreatePangoLayout ();
					desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
					layout.FontDescription.Weight = Weight.Normal;		// Letra normal
					
					codigoproducto = lector["idproducto_osiris"].ToString().Trim();
					descriproducto = lector["descripcion_producto"].ToString().Trim();
					cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(codigoproducto);		Pango.CairoHelper.ShowLayout (cr, layout);
					if(lector["descripcion_producto"].ToString().Length > 60){
						cr.MoveTo(60*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(lector["descripcion_producto"].ToString().Trim().Substring(0,60));				Pango.CairoHelper.ShowLayout (cr, layout);				
					}else{
						cr.MoveTo(60*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(lector["descripcion_producto"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);				
					}
					int columna_sepa = 35;
					int columna_inicio = 300;
					switch (lector["mes"].ToString().Trim()){	
						case "01":
							cr.MoveTo(columna_inicio*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
						break;
						case "02":
							cr.MoveTo((columna_inicio+(columna_sepa*1))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
						break;
						case "03":
							cr.MoveTo((columna_inicio+(columna_sepa*2))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
						break;
						case "04":
							cr.MoveTo((columna_inicio+(columna_sepa*3))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
						break;
						case "05":
							cr.MoveTo((columna_inicio+(columna_sepa*4))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
						break;
						case "06":
							cr.MoveTo((columna_inicio+(columna_sepa*5))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
						break;
						case "07":
							cr.MoveTo((columna_inicio+(columna_sepa*6))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
						break;
						case "08":
							cr.MoveTo((columna_inicio+(columna_sepa*7))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
						break;
						case "09":
							cr.MoveTo((columna_inicio+(columna_sepa*8))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
						break;
						case "10":
							cr.MoveTo((columna_inicio+(columna_sepa*9))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
						break;
						case "11":
							cr.MoveTo((columna_inicio+(columna_sepa*10))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
						break;
						case "12":
							cr.MoveTo((columna_inicio+(columna_sepa*11))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
						break;
					}
					while(lector.Read()){
						if(codigoproducto != lector["idproducto_osiris"].ToString().Trim()){
							comienzo_linea += separacion_linea;
							cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(codigoproducto);		Pango.CairoHelper.ShowLayout (cr, layout);
							if(lector["descripcion_producto"].ToString().Length > 60){
								cr.MoveTo(60*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(lector["descripcion_producto"].ToString().Trim().Substring(0,60));				Pango.CairoHelper.ShowLayout (cr, layout);				
							}else{
								cr.MoveTo(60*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(lector["descripcion_producto"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);				
							}
							switch (lector["mes"].ToString().Trim()){	
								case "01":
									cr.MoveTo(columna_inicio*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "02":
									cr.MoveTo((columna_inicio+(columna_sepa*1))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "03":
									cr.MoveTo((columna_inicio+(columna_sepa*2))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "04":
									cr.MoveTo((columna_inicio+(columna_sepa*3))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "05":
									cr.MoveTo((columna_inicio+(columna_sepa*4))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "06":
									cr.MoveTo((columna_inicio+(columna_sepa*5))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "07":
									cr.MoveTo((columna_inicio+(columna_sepa*6))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "08":
									cr.MoveTo((columna_inicio+(columna_sepa*7))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "09":
									cr.MoveTo((columna_inicio+(columna_sepa*8))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "10":
									cr.MoveTo((columna_inicio+(columna_sepa*9))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "11":
									cr.MoveTo((columna_inicio+(columna_sepa*10))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "12":
									cr.MoveTo((columna_inicio+(columna_sepa*11))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
							}
							codigoproducto = lector["idproducto_osiris"].ToString().Trim();
							descriproducto = lector["descripcion_producto"].ToString().Trim();
							salto_de_pagina(cr,layout);
							desc = Pango.FontDescription.FromString ("Sans");
							fontSize = 6.0;			layout = null;			layout = context.CreatePangoLayout ();
							desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
							layout.FontDescription.Weight = Weight.Normal;		// Letra normal
						}else{
							switch (lector["mes"].ToString().Trim()){	
								case "01":
									cr.MoveTo(columna_inicio*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "02":
									cr.MoveTo((columna_inicio+(columna_sepa*1))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "03":
									cr.MoveTo((columna_inicio+(columna_sepa*2))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "04":
									cr.MoveTo((columna_inicio+(columna_sepa*3))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "05":
									cr.MoveTo((columna_inicio+(columna_sepa*4))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "06":
									cr.MoveTo((columna_inicio+(columna_sepa*5))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "07":
									cr.MoveTo((columna_inicio+(columna_sepa*6))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "08":
									cr.MoveTo((columna_inicio+(columna_sepa*7))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "09":
									cr.MoveTo((columna_inicio+(columna_sepa*8))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "10":
									cr.MoveTo((columna_inicio+(columna_sepa*9))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "11":
									cr.MoveTo((columna_inicio+(columna_sepa*10))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
								case "12":
									cr.MoveTo((columna_inicio+(columna_sepa*11))*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["max"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
								break;
							}
						}
					}
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();			
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
		{
			//Gtk.Image image5 = new Gtk.Image();
            //image5.Name = "image5";
			//image5.Pixbuf = new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "osiris.jpg"));
			//image5.Pixbuf = new Gdk.Pixbuf("/opt/osiris/bin/OSIRISLogo.jpg");   // en Linux
			//image5.Pixbuf.ScaleSimple(128, 128, Gdk.InterpType.Bilinear);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf,1,-30);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(145, 50, Gdk.InterpType.Bilinear),1,1);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(180, 64, Gdk.InterpType.Hyper),1,1);
			//cr.Fill();
			//cr.Paint();
			//cr.Restore();
								
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText(classpublic.nombre_empresa);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText(classpublic.direccion_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,25*escala_en_linux_windows);			layout.SetText(classpublic.telefonofax_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(650*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(650*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page :"+numpage.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 10.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			
			double width = context.Width;
			layout.Width = (int) width;
			layout.Alignment = Pango.Alignment.Center;
			//layout.Wrap = Pango.WrapMode.Word;
			//layout.SingleParagraphMode = true;
			layout.Justify =  false;
			cr.MoveTo(width/2,45*escala_en_linux_windows);	layout.SetText("REPORTE");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(225*escala_en_linux_windows, 35*escala_en_linux_windows);			layout.SetText(titulo_rpt);				Pango.CairoHelper.ShowLayout (cr, layout);
			
			fontSize = 6.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra negrita
			
			// Creando el Cuadro de Titulos
			cr.Rectangle (05*escala_en_linux_windows, 60*escala_en_linux_windows, 750*escala_en_linux_windows, 15*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.5;
			cr.Stroke();
		}
		
		void salto_de_pagina(Cairo.Context cr,Pango.Layout layout)           
        {
            if(comienzo_linea >530){
                cr.ShowPage();
                Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");                               
                fontSize = 6.0;        desc.Size = (int)(fontSize * pangoScale);  		layout.FontDescription = desc;
                comienzo_linea = 80;
                numpage += 1;
                imprime_encabezado(cr,layout);
            }
        }
		
		void on_checkbutton_todos_proveedore_clicked(object sender, EventArgs args)
		{
			verifica_checkbutton_prov();
		}
		
		void verifica_checkbutton_prov()
		{
			bool active_checkbutton;			
			if(checkbutton_todos_proveedores.Active == true){
				active_checkbutton = false;
				query_proveedores = " ";
			}else{
				active_checkbutton = true;
				query_proveedores = " AND osiris_erp_factura_compra_enca.id_proveedor = '"+entry_id_proveedor.Text+"' ";
			}
			entry_id_proveedor.Sensitive = active_checkbutton;
			entry_nombre_proveedor.Sensitive = active_checkbutton;
			button_busca_proveedor.Sensitive = active_checkbutton;
		}
		
		string crea_string_grupoproductos()
		{
			int variable_paso_02_1 = 0;
			string variable_paso_03 = "";
			string numeros_seleccionado = "";
			TreeIter iter;			
			if (treeViewEngineListaGrupoProd.GetIterFirst (out iter)){
 				if ((bool) treeview_lista_grupoprod.Model.GetValue (iter,0) == true){
 					numeros_seleccionado = (string) treeview_lista_grupoprod.Model.GetValue (iter,2);
 					variable_paso_02_1 += 1;		
 				}
 				while (treeViewEngineListaGrupoProd.IterNext(ref iter)){
 					if ((bool) treeview_lista_grupoprod.Model.GetValue (iter,0) == true){
 				    	if (variable_paso_02_1 == 0){ 				    	
 							numeros_seleccionado = (string) treeview_lista_grupoprod.Model.GetValue (iter,2);
 							variable_paso_02_1 += 1;
 						}else{
 							variable_paso_03 = (string) treeview_lista_grupoprod.Model.GetValue (iter,2);
 							numeros_seleccionado = numeros_seleccionado.Trim() + "','" + variable_paso_03.Trim();
 						}
 					}
 				}
 			}
			return " AND osiris_productos.id_grupo_producto IN ('"+numeros_seleccionado+"') ";
		}
		
		void on_checkbutton_todas_fechas_clicked(object sender, EventArgs args)
		{
			verifica_checkbutton_fechas();
		}
		
		void verifica_checkbutton_fechas()
		{
			bool active_checkbutton;
			if(checkbutton_todas_fechas.Active == true){
				query_rango_fechas = " ";
				query_rango_fechas2 = " ";
				active_checkbutton = false;
			}else{
				query_rango_fechas = "AND to_char(osiris_erp_factura_compra_enca.fecha_factura,'yyyy-MM-dd') >= '"+(string) entry_ano_inicial.Text.ToString()+"-"+(string) entry_mes_inicial.Text.ToString()+"-"+(string) entry_dia_inicial.Text.ToString()+"' "+
									"AND to_char(osiris_erp_factura_compra_enca.fecha_factura,'yyyy-MM-dd') <= '"+(string) entry_ano_final.Text.ToString()+"-"+(string) entry_mes_final.Text.ToString()+"-"+(string) entry_dia_final.Text.ToString()+"' ";
				query_rango_fechas2 = "AND to_char(osiris_erp_factura_compra_enca.fechahora_creacion,'yyyy') >= '"+(string) entry_ano_inicial.Text.ToString()+"' "+
									 "AND to_char(osiris_erp_factura_compra_enca.fechahora_creacion,'yyyy') <= '"+(string) entry_ano_final.Text.ToString()+"' " +
									 "AND to_char(osiris_erp_factura_compra_enca.fechahora_creacion,'MM') >= '"+(string) entry_mes_inicial.Text.ToString()+"' "+
									 "AND to_char(osiris_erp_factura_compra_enca.fechahora_creacion,'MM') <= '"+(string) entry_mes_final.Text.ToString()+"' ";
				active_checkbutton = true;
			}			
			entry_dia_inicial.Sensitive = active_checkbutton;
			entry_mes_inicial.Sensitive = active_checkbutton;
			entry_ano_inicial.Sensitive = active_checkbutton;			
			entry_dia_final.Sensitive = active_checkbutton;
			entry_mes_final.Sensitive = active_checkbutton;
			entry_ano_final.Sensitive = active_checkbutton;
		}		
		
		void on_busca_proveedores_clicked(object sender, EventArgs args)
		{
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion
			// la clase recibe tambien el orden del query
			// es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo
			object[] parametros_objetos = {entry_id_proveedor,entry_nombre_proveedor};
			string[] parametros_sql = {"SELECT descripcion_proveedor,direccion_proveedor,rfc_proveedor,curp_proveedor, "+
								"colonia_proveedor,municipio_proveedor,estado_proveedor,telefono1_proveedor, "+ 
								"telefono2_proveedor,celular_proveedor,rfc_proveedor, proveedor_activo, "+
								"id_proveedor,contacto1_proveedor,mail_proveedor,pagina_web_proveedor,"+
								"osiris_erp_proveedores.id_forma_de_pago,descripcion_forma_de_pago,fax_proveedor "+
								"FROM osiris_erp_proveedores, osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_proveedores.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago ",				
								"SELECT descripcion_proveedor,direccion_proveedor,rfc_proveedor,curp_proveedor, "+
								"colonia_proveedor,municipio_proveedor,estado_proveedor,telefono1_proveedor, "+ 
								"telefono2_proveedor,celular_proveedor,rfc_proveedor, proveedor_activo, "+
								"id_proveedor,contacto1_proveedor,mail_proveedor,pagina_web_proveedor, "+
								"osiris_erp_proveedores.id_forma_de_pago,descripcion_forma_de_pago,fax_proveedor "+
								"FROM osiris_erp_proveedores, osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_proveedores.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
								"AND proveedor_activo = 'true' " +
								"AND descripcion_proveedor LIKE '%"};
			string[] parametros_string = {};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_proveedores_catalogo_producto"," ORDER BY descripcion_proveedor;","%' ",0);
		}
		
		void on_checkbutton_todos_grupos_clicked(object sender, EventArgs args)
		{
			verifica_grupo_prodctos();
		}
		
		void verifica_grupo_prodctos()
		{
			TreeIter iter2;
			if ((bool)checkbutton_todos_grupos.Active == true){
				if (treeViewEngineListaGrupoProd.GetIterFirst (out iter2)){
					treeview_lista_grupoprod.Model.SetValue(iter2,0,true);
					while (treeViewEngineListaGrupoProd.IterNext(ref iter2)){
						treeview_lista_grupoprod.Model.SetValue(iter2,0,true);
					}
				}
				query_in_grupo = "";
			}else{
				if (treeViewEngineListaGrupoProd.GetIterFirst (out iter2)){
					treeview_lista_grupoprod.Model.SetValue(iter2,0,false);
					while (treeViewEngineListaGrupoProd.IterNext(ref iter2)){
						treeview_lista_grupoprod.Model.SetValue(iter2,0,false);
					}
				}
			}
		}
		
		void on_checkbutton_herr_adicionales_clicked(object sender, EventArgs args)
		{
			if ((bool)checkbutton_herr_adicionales.Active == true){
				combobox_herr_adicionales.Sensitive = true;
				llenado_combobox(0,"",combobox_herr_adicionales,"array","","","",args_herr_adicional,args_id_array,"");
				entry_dia_inicial.Sensitive = false;
				entry_dia_final.Sensitive = false;
			}else{
				combobox_herr_adicionales.Sensitive = false;
				entry_dia_inicial.Sensitive = true;
				entry_dia_final.Sensitive = true;
			}
		}
		
		void creartreeviewgrupoprod()
		{
			treeViewEngineListaGrupoProd = new TreeStore(typeof(bool),
											typeof(string),
											typeof(string));
			treeview_lista_grupoprod.Model = treeViewEngineListaGrupoProd;
			
			treeview_lista_grupoprod.RulesHint = true;
			
			Gtk.TreeViewColumn col_00 = new TreeViewColumn();
			Gtk.CellRendererToggle cellrt00 = new CellRendererToggle();
			col_00.Title = "Surtir"; // titulo de la cabecera de la columna, si está visible
			col_00.PackStart(cellrt00, true);
			col_00.AddAttribute (cellrt00, "active", 0);
			cellrt00.Activatable = true;
			cellrt00.Toggled += selecciona_fila;
			
			Gtk.TreeViewColumn col_01 = new TreeViewColumn();
			Gtk.CellRendererText cellrt01 = new CellRendererText();
			col_01.Title = "Solicitado"; // titulo de la cabecera de la columna, si está visible
			col_01.PackStart(cellrt01, true);
			col_01.AddAttribute (cellrt01, "text", 1);
			
			treeview_lista_grupoprod.AppendColumn(col_00);
			treeview_lista_grupoprod.AppendColumn(col_01);
		}
		
		void llenadogrupoprod()
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT id_grupo_producto,descripcion_grupo_producto FROM osiris_grupo_producto WHERE descripcion_grupo_producto <> '' ORDER BY descripcion_grupo_producto";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){
					treeViewEngineListaGrupoProd.AppendValues(false,
													(string) lector["descripcion_grupo_producto"],
													(string) lector["id_grupo_producto"].ToString().Trim());
				}				
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		// Cuando seleccion el treeview de cargos extras para cargar los productos  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			if (treeview_lista_grupoprod.Model.GetIter (out iter,new TreePath (args.Path))) {
				bool old = (bool) treeview_lista_grupoprod.Model.GetValue (iter,0);
				treeview_lista_grupoprod.Model.SetValue(iter,0,!old);
			}	
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
	}
}