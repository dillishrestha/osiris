using System;
//using SmartXLS;		// libreria para crear archivo xls es de paga
using Npgsql;
using Gtk;
using Glade;
// libreria creada con el proyecto AODL 1.4 que usa 
using AODL;
using AODL.Document.SpreadsheetDocuments;
using AODL.Document.Content;
using AODL.Document.Content.Tables;
using AODL.Document;
using AODL.Package;
using AODL.Document.Collections;
//using NUnit.Framework;

namespace osiris
{
	public class class_traslate_spreadsheet
	{
		string connectionString;
		string nombrebd;		
		class_conexion conexion_a_DB = new class_conexion();
				
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public class_traslate_spreadsheet (string query_sql,string[] args_names_field,string[] args_type_field,bool typetext,string[] args_field_text,string name_field_text,bool more_title,string[] args_more_title)
		{
			Console.WriteLine(name_field_text+" nombre del campo");
			int files_field = 0;
			string [] array_field_text = new string[args_field_text.Length];
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;			
			//Create new spreadsheet open document (.ods) 
			AODL.Document.SpreadsheetDocuments.SpreadsheetDocument spreadsheetDocument = new AODL.Document.SpreadsheetDocuments.SpreadsheetDocument();
			spreadsheetDocument.New();			
			//Create a new table
			AODL.Document.Content.Tables.Table table = new AODL.Document.Content.Tables.Table(spreadsheetDocument, "hoja1", "tablefirst");
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = query_sql;
				//Console.WriteLine(comando.CommandText);
				comando.ExecuteNonQuery();    comando.Dispose();
				NpgsqlDataReader lector = comando.ExecuteReader ();
				// Creando los nombres de ancabezado de los campos
				for (int colum_field = 0; colum_field < args_names_field.Length; colum_field++){					
					AODL.Document.Content.Tables.Cell cell = table.CreateCell ();
					//cell.OfficeValueType ="float";
					AODL.Document.Content.Text.Paragraph paragraph = new AODL.Document.Content.Text.Paragraph(spreadsheetDocument);				
					string text = (string) args_names_field[ colum_field ].ToString().Trim();			
					paragraph.TextContent.Add(new AODL.Document.Content.Text.SimpleText(spreadsheetDocument,text));
					cell.Content.Add(paragraph);
					cell.OfficeValueType = "string";							
					cell.OfficeValue = text;
					table.InsertCellAt (files_field, colum_field, cell);					
				}
				if(typetext == true){
					// Creando los nombres de ancabezado de los campos cuando son de tipo text (almacenado en una tabla tipo text)
					for(int colum_field2 = 0 ; colum_field2 < args_field_text.Length; colum_field2++){					
						AODL.Document.Content.Tables.Cell cell = table.CreateCell ();
						//cell.OfficeValueType ="float";
						AODL.Document.Content.Text.Paragraph paragraph = new AODL.Document.Content.Text.Paragraph(spreadsheetDocument);				
						string text = (string) args_field_text[ colum_field2 ].ToString().Trim();			
						paragraph.TextContent.Add(new AODL.Document.Content.Text.SimpleText(spreadsheetDocument,text));
						cell.Content.Add(paragraph);
						cell.OfficeValueType = "string";							
						cell.OfficeValue = text;
						table.InsertCellAt (files_field, colum_field2+args_names_field.Length, cell);					
					}
				}
				if(more_title == true){
					int title_field_text = 0;
					if(typetext == true){
						title_field_text = args_field_text.Length;
					}
					// Creando los nombres de ancabezado de los campos cuando son de tipo text (almacenado en una tabla tipo text)
					for(int colum_field3 = 0 ; colum_field3 < args_more_title.Length; colum_field3++){					
						AODL.Document.Content.Tables.Cell cell = table.CreateCell ();
						//cell.OfficeValueType ="float";
						AODL.Document.Content.Text.Paragraph paragraph = new AODL.Document.Content.Text.Paragraph(spreadsheetDocument);				
						string text = (string)args_more_title[ colum_field3 ].ToString().Trim();			
						paragraph.TextContent.Add(new AODL.Document.Content.Text.SimpleText(spreadsheetDocument,text));
						cell.Content.Add(paragraph);
						cell.OfficeValueType = "string";							
						cell.OfficeValue = text;
						table.InsertCellAt (files_field, colum_field3+args_names_field.Length+title_field_text, cell);					
					}
				}
				files_field++;
				string texto = "";
				while (lector.Read()){
					for (int colum_field = 0; colum_field < args_names_field.Length; colum_field++){					
						AODL.Document.Content.Tables.Cell cell = table.CreateCell ();
						//cell.OfficeValueType ="float";
						AODL.Document.Content.Text.Paragraph paragraph = new AODL.Document.Content.Text.Paragraph(spreadsheetDocument);				
						string text = lector[(string) args_names_field[ colum_field ]].ToString().Trim();			
						paragraph.TextContent.Add(new AODL.Document.Content.Text.SimpleText(spreadsheetDocument,text));
						cell.Content.Add(paragraph);
						cell.OfficeValueType = (string) args_type_field [colum_field];							
						cell.OfficeValue = text;
						table.InsertCellAt (files_field, colum_field, cell);						
					}
					if(typetext == true){
						texto = (string) lector[name_field_text]; // puede ser una campo de la base de datos tipo Text
						char[] delimiterChars = {'\n'}; // delimitador de Cadenas
						char[] delimiterChars1 = {';'}; // delimitador de Cadenas
						//string texto = "1;daniel; ;olivares;cuevas";
						//"2;genaro;cuevas;bazaldua\n"+
						//"3;gladys;perez;orellana\n";
						string[] words = texto.Split(delimiterChars); // Separa las Cadenas
						if(texto != ""){						
							// Recorre la variable
							foreach (string s in words){
								if (s.Length > 0){
									string texto1 = (string) s;
									string[] words1 = texto1.Split(delimiterChars1);
									//for (int i = 1; i <= 6; i++){
									int i=0;
									int i2 = 1;
									foreach (string s1 in words1){
										//Console.WriteLine(s1.ToString());
										if(i2 <= args_field_text.Length){
											array_field_text[i] = s1.ToString();
										}
										i++;
										i2++;
									}
								}
							}
							for( int i = 0; i < array_field_text.Length; i++ ){
								//Console.WriteLine(array_field_text[i]);
								AODL.Document.Content.Tables.Cell cell = table.CreateCell ();
								//cell.OfficeValueType ="float";
								AODL.Document.Content.Text.Paragraph paragraph = new AODL.Document.Content.Text.Paragraph(spreadsheetDocument);				
								string text = (string) array_field_text[i];			
								paragraph.TextContent.Add(new AODL.Document.Content.Text.SimpleText(spreadsheetDocument,text));
								cell.Content.Add(paragraph);
								cell.OfficeValueType = "string";							
								cell.OfficeValue = text;
								table.InsertCellAt (files_field, i+args_names_field.Length, cell);
							}
						}else{
							
						}
					}					
					files_field++;
				}
				conexion.Close();
				//Insert table into the spreadsheet document
				spreadsheetDocument.TableCollection.Add(table);
				spreadsheetDocument.SaveTo("export.ods");			
				// open the document automatic
				System.Diagnostics.Process.Start("export.ods");
			}catch(NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error, 
									ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();		msgBoxError.Destroy();
			}
			
			/*
			// Leyendo el archivo .ods
			spreadsheetDocument.Load("export.ods");
			Assert.IsNotNull(spreadsheetDocument.TableCollection, "Table collection must exits.");
			//Assert.IsTrue(spreadsheetDocument.TableCollection.Count == 6, "There must be 3 tables available.");
		
			int paso = spreadsheetDocument.TableCollection.Count;		
		
			Console.WriteLine(paso.ToString());
			int i2 = 0; // current row index
			int ii = 0; // current cell index
			string innerText = ""; // current inner text 
			try{
				//Assert.IsTrue(spreadsheetDocument.TableCollection[0].Rows.Count == 5, "There must be 6 rows available.");
				for(i2= 0; i2 < spreadsheetDocument.TableCollection[0].Rows.Count; i2++){
					string contents = "Row " + i2 + ": ";
					//Assert.IsTrue(spreadsheetDocument.TableCollection[0].Rows[i2].Cells.Count == 3, "There must be 3 cells available.");
					for(ii = 0; ii < spreadsheetDocument.TableCollection[0].Rows[i2].Cells.Count; ii++){
						innerText = spreadsheetDocument.TableCollection[0].Rows[i2].Cells[ii].Node.InnerText;
						if (spreadsheetDocument.TableCollection[0].Rows[i2].Cells[ii].OfficeValue != null){
							contents += spreadsheetDocument.TableCollection[0].Rows[i2].Cells[ii].OfficeValue.ToString() + " ";
						}else{
							contents += innerText + " ";
						}
					}
					Console.WriteLine(contents);
				}
			}catch(System.Exception ex){
				string where = "occours in Row " + i2.ToString() + " and cell " + ii.ToString() + " last cell content " + innerText + "\n\n";
				Console.WriteLine(where + ex.Message + "\n\n" + ex.StackTrace);
			}
			*/	
			
			/*
			// Usando libreria SmartXLS
			WorkBook book = new WorkBook();
		
            try{               
				//Sets the number of worksheets in this workbook
                book.NumSheets = 2;
                // set sheet names
               	book.setSheetName(0, "hoja1");	// renombrando la pestaña
				book.setSheetName(1, "hoja2");	// renombrando la pestaña
				book.Sheet = 0;
				// book.setText(Fila, columna , "texto");
				book.setText(0, 0, "foliodeservicio");
				book.setText(0, 1, "descripcion_producto");
				book.setText(0, 2, "idproducto");
				book.setText(0, 3, "cantidadaplicada");
				book.setText(0, 4, "preciounitario");
				book.setText(0, 5, "ppcantidad");
				book.setText(0, 6, "fechcreacion");
				book.setText(0, 7, "descripcion_admisiones");
				book.setText(0, 8, "idtipoadmision");
				book.setText(0, 9, "descripcion_grupo_producto");
				book.setText(0, 10, "aplicar_iva");
				
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
    	        // Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
 					comando.CommandText = "SELECT osiris_erp_cobros_deta.folio_de_servicio AS foliodeservicio,descripcion_producto,to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto, "+
						"to_char(osiris_erp_cobros_deta.cantidad_aplicada,'99999.99') AS cantidadaplicada,to_char(osiris_erp_cobros_deta.precio_producto,'99999999.99') AS preciounitario,"+
							"to_char(osiris_erp_cobros_deta.cantidad_aplicada * osiris_erp_cobros_deta.precio_producto,'99999999.99') AS ppcantidad,"+
							"to_char(osiris_erp_cobros_deta.fechahora_creacion,'dd-MM-yyyy HH24:mi:ss') AS fechcreacion, osiris_his_tipo_admisiones.descripcion_admisiones,"+
							"osiris_erp_cobros_deta.id_tipo_admisiones AS idtipoadmision,descripcion_grupo_producto,osiris_productos.aplicar_iva," +
							"to_char(osiris_erp_cobros_deta.id_secuencia,'9999999999') AS secuencia " +
							"FROM osiris_erp_cobros_deta,osiris_productos,osiris_his_tipo_admisiones,osiris_grupo_producto " +
							"WHERE osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto " +
							"AND osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones " +
							"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto " +
							"AND osiris_erp_cobros_deta.eliminado = false " +
							"AND osiris_erp_cobros_deta.folio_de_servicio IN('"+numeroatencion_+"') " +
							"ORDER BY to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd HH24:mm:ss'),osiris_erp_cobros_deta.id_tipo_admisiones ASC," +
							 "osiris_productos.id_grupo_producto;";
 					comando.ExecuteNonQuery();    comando.Dispose();
					//Console.WriteLine(comando.CommandText);
					NpgsqlDataReader lector = comando.ExecuteReader ();
					while (lector.Read()){
						book.setNumber(number_file, 0, int.Parse(numeroatencion_));
						book.setText(number_file, 1, (string) lector["descripcion_producto"]);
						book.setText(number_file, 2, (string) lector["idproducto"]);	
						number_file++;
					}
					conexion.Close();
					book.write("export.xls");
					
					System.Diagnostics.Process.Start("export.xls");
					
					
				}catch(NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();		msgBoxError.Destroy();
				}
				
			 
            }catch (System.Exception ex){
                Console.Error.WriteLine(ex);
            }
            */
		}		
	}
}