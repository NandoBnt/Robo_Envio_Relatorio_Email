using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Data;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.IO;

namespace Robo_EnvioEmail.Negocio
{
    public class Relatorio
    {

        public string GerarExcel(System.Data.DataTable dtRelatorio, string sTituloRelatorio, string sPrimeiraColunaValor, string sCaminhoSalvar, string sNomeArquivo)
        {
            Application xlApp = new Application();
            Workbook xlWorkbook;
            Worksheet xlWorksheet;
            string sArquivo = "";
            int iCol = 1;
            int iRow = 4;

            xlWorkbook = xlApp.Workbooks.Add(Type.Missing);

            xlWorksheet = (Worksheet)xlWorkbook.ActiveSheet;
            xlWorksheet.Name = "Relatorio";

            xlWorksheet.Range["A1", NumeroParaLetra(dtRelatorio.Columns.Count) + "1"].Merge(true);
            xlWorksheet.Range["A1", NumeroParaLetra(dtRelatorio.Columns.Count) + "1"].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
            xlWorksheet.Range["A1", NumeroParaLetra(dtRelatorio.Columns.Count) + "1"].Font.Bold = true;
            xlWorksheet.Range["A1", NumeroParaLetra(dtRelatorio.Columns.Count) + "1"].HorizontalAlignment = Constants.xlCenter;

            xlWorksheet.Cells[1, 1] = sTituloRelatorio;

            xlWorksheet.Range["A3", NumeroParaLetra(dtRelatorio.Columns.Count) + "3"].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
            xlWorksheet.Range["A3", NumeroParaLetra(dtRelatorio.Columns.Count) + "3"].Font.Bold = true;

            foreach (DataColumn col in dtRelatorio.Columns)
            {
                xlWorksheet.Cells[3, iCol] = col.ColumnName;
                iCol++;
            }

            foreach (DataRow row in dtRelatorio.Rows)
            {
                xlWorksheet.Range[sPrimeiraColunaValor + iRow, NumeroParaLetra(dtRelatorio.Columns.Count) + iRow].NumberFormat = "###,##0.00";

                for (int indiceCol = 0; indiceCol < dtRelatorio.Columns.Count; indiceCol++)
                {
                    xlWorksheet.Cells[iRow, indiceCol + 1] = row[indiceCol];
                }

                iRow++;
            }

            xlWorksheet.Range["A1", NumeroParaLetra(dtRelatorio.Columns.Count) + iRow].EntireColumn.AutoFit();

            AplicarBordas(xlWorksheet);

            xlWorksheet.Activate();
            xlWorksheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
            xlWorksheet.PageSetup.PaperSize = XlPaperSize.xlPaperA4;

            xlWorksheet.PageSetup.LeftMargin = xlApp.InchesToPoints(0.5);
            xlWorksheet.PageSetup.RightMargin = xlApp.InchesToPoints(0.5);
            xlWorksheet.PageSetup.TopMargin = xlApp.InchesToPoints(0.75);
            xlWorksheet.PageSetup.BottomMargin = xlApp.InchesToPoints(0.75);

            xlWorksheet.PageSetup.Zoom = false;
            xlWorksheet.PageSetup.FitToPagesWide = 1;
            xlWorksheet.PageSetup.FitToPagesTall = false;

            sArquivo = sCaminhoSalvar + sNomeArquivo + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";

            xlWorkbook.SaveAs(sArquivo, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            xlWorkbook.Close(false, Type.Missing, Type.Missing);
            xlApp.Quit();

            return sArquivo;
        }

        public void AplicarBordas(Worksheet sheet)
        {
            Range usedRange = sheet.UsedRange;

            var indices = new XlBordersIndex[]
            {
                XlBordersIndex.xlEdgeLeft,
                XlBordersIndex.xlEdgeTop,
                XlBordersIndex.xlEdgeBottom,
                XlBordersIndex.xlEdgeRight,
                XlBordersIndex.xlInsideHorizontal,
                XlBordersIndex.xlInsideVertical
            };

            foreach (var index in indices)
            {
                Border border = usedRange.Borders[index];
                border.LineStyle = XlLineStyle.xlContinuous;
                border.Weight = XlBorderWeight.xlThin;
            }
        }


        public string ExportarExcelParaPDF(string caminhoArquivoExcel, string caminhoDestinoPDF)
        {
            var excelApp = new Application();
            Workbook workbook = null;

            try
            {
                excelApp.Visible = false;
                excelApp.ScreenUpdating = false;

                workbook = excelApp.Workbooks.Open(caminhoArquivoExcel);
                workbook.ExportAsFixedFormat(
                    XlFixedFormatType.xlTypePDF,
                    caminhoDestinoPDF
                );
            }
            catch(Exception ex)
            {
                return "";
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                }

                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

            return caminhoDestinoPDF;
        }

        public string RetornaArquivosXML(string sCaminhoXML, System.Data.DataTable dtDados)
        {

            string listaArquivos = string.Empty;
            string sNomeArquivo;
            string sSubDiretorio;
            string sArquivo;

            foreach (DataRow row in dtDados.Rows)
            {
                sSubDiretorio = Convert.ToDateTime(row["Emissao"]).Year.ToString() + "." + Convert.ToDateTime(row["Emissao"]).Month.ToString("D2") + "\\";
                sNomeArquivo = row["Arquivo"].ToString();

                sArquivo = sCaminhoXML + sSubDiretorio + sNomeArquivo;

                if (File.Exists(sArquivo))
                {
                    listaArquivos += sArquivo + ";";
                }
            }

            if (listaArquivos != string.Empty)
            {
                listaArquivos = listaArquivos.Substring(0, listaArquivos.Length - 1);
            }

            return listaArquivos;
        }

        public static string NumeroParaLetra(int numero)
        {
            if (numero < 1 || numero > 26)
                throw new ArgumentOutOfRangeException(nameof(numero), "O número deve estar entre 1 e 26.");

            // ASCII da letra 'A' é 65
            char letra = (char)('A' + numero - 1);
            return letra.ToString();
        }


    }
}
