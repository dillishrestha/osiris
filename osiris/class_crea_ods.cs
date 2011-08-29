using System;
using SmartXLS;		// libreria para crear archivo xls es de paga
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
using NUnit.Framework;

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
		
		public class_traslate_spreadsheet (string numeroatencion_)
		{
			int files_field = 0;
			string[] args_names_field = {"foliodeservicio","descripcion_producto","idproducto","cantidadaplicada","preciounitario","ppcantidad","fechcreacion","descripcion_admisiones","descripcion_grupo_producto"};
			string[] args_type_field = {"float","string","string","float","float","float","string","string","string"};
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
				files_field++;				
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
				book.setText(0, 8, "descripcion_grupo_producto");
				book.setText(0, 8, "aplicar_iva");
				
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
						book.setText(number_file, 1, (string) lector["idproducto"]);	
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

