using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Data;

namespace Robo_EnvioEmail.Negocio
{
    public class Relatorio
    {

        public string GerarExcel(System.Data.DataTable dtRelatorio, string sCaminhoSalvar)
        {
            Application xlApp = new Application();
            _Workbook xlWorkbook;
            _Worksheet xlWorksheet;
            string sArquivo = "";
            int iCol = 1;
            int iRow = 4;

            xlWorkbook = xlApp.Workbooks.Add(Type.Missing);

            xlWorksheet = (Worksheet)xlWorkbook.ActiveSheet;
            xlWorksheet.Name = "Relatorio";

            xlWorksheet.Range["A1", "S1"].Merge(true);
            xlWorksheet.Range["A1", "S1"].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
            xlWorksheet.Range["A1", "S1"].Font.Bold = true;
            xlWorksheet.Range["A1", "S1"].HorizontalAlignment = Constants.xlCenter;

            xlWorksheet.Cells[1, 1] = "CRONOGRAMA DE INFORMAÇÕES";

            xlWorksheet.Range["A3", "S3"].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
            xlWorksheet.Range["A3", "S3"].Font.Bold = true;

            foreach (DataColumn col in dtRelatorio.Columns)
            {
                xlWorksheet.Cells[3, iCol] = col.ColumnName;
                iCol++;
            }

            foreach (DataRow row in dtRelatorio.Rows)
            {
                xlWorksheet.Range["P" + iRow, "S" + iRow].NumberFormat = "###,##0.00";

                for (int indiceCol = 0; indiceCol < dtRelatorio.Columns.Count; indiceCol++)
                {
                    xlWorksheet.Cells[iRow, indiceCol + 1] = row[indiceCol];
                }

                iRow++;
            }

            xlWorksheet.Range["A1", "S" + iRow].EntireColumn.AutoFit();

            sArquivo = sCaminhoSalvar + "Relatorio_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";

            xlWorkbook.SaveAs(sArquivo, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            xlWorkbook.Close(false, Type.Missing, Type.Missing);
            xlApp.Quit();

            return sArquivo;
        }

    }
}
