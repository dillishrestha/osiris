
using System;
using System.Xml;
using System.IO;
//using NUnit.Framework;
//using AODL.Document.SpreadsheetDocuments;
//using AODL.Document.Content.Tables;
//using AODL.Document.TextDocuments;
//using AODL.Document.Styles;
//using AODL.Document.Content.Text;

namespace osiris
{
	public class class_crea_ods
	{
		public class_crea_ods ()
		{
			//SpreadsheetDocument doc = new SpreadsheetDocument ();
			//doc.New ();
			//Table table = new Table (doc,"tab1","tab1");
			/*
			for(int i=1; i<=1; i++)
			{
				for(int j=1; j<=6;j++)
				{
					Cell cell = table.CreateCell ();
					cell.OfficeValueType ="float";
					Paragraph paragraph = new Paragraph (doc);
					string text= (j+i-1).ToString();
					paragraph.TextContent .Add (new SimpleText ( doc,text));
					cell.Content.Add(paragraph);
					cell.OfficeValueType = "string";
					cell.OfficeValue = text;
					table.InsertCellAt (i, j, cell);
				}
			}
			*/
			/*
			Assert.AreEqual(7, table.Rows[1].Cells.Count);
			Assert.AreEqual(6, table.Rows[2].Cells.Count);
			Assert.AreEqual(6, table.Rows[3].Cells.Count);
			Assert.AreEqual(6, table.Rows[4].Cells.Count);
			/*Chart chart = new Chart (table,"ch1");
			chart.ChartType=ChartTypes.bar .ToString () ;
			chart.XAxisName ="yeer";
			chart.YAxisName ="dollar";
			chart.CreateFromCellRange ("A1:E4");
			chart.EndCellAddress ="tab1.K17";*/
						
			//doc.Content .Add (table);
			//doc.SaveTo(Path.Combine(AARunMeFirstAndOnce.outPutFolder, @"NewChartOne.ods"));
		}
	}
	
	/*
	public class AARunMeFirstAndOnce
	{
		private static string generatedFolder	= @"\generatedfiles\"; //System.Configuration.ConfigurationSettings.AppSettings["writefiles"];
		private static string readFromFolder	= @"\files\"; //System.Configuration.ConfigurationSettings.AppSettings["readfiles"];
		public static string outPutFolder		= Environment.CurrentDirectory+generatedFolder;
		public static string inPutFolder		= Environment.CurrentDirectory+readFromFolder;

		[Test]
		public void AARunMeFirstAndOnceDir()
		{
			if (Directory.Exists(outPutFolder))
				Directory.Delete(outPutFolder, true);
			Directory.CreateDirectory(outPutFolder);
		}
	}
	*/
}
