using NPOI.SS.UserModel;
using OfficeOpenXml;
using System.Collections.Generic;

namespace Epine.Infrastructure.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExcelExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public static List<Dictionary<int, string>> GetRowsWithIndex(this ExcelWorksheet worksheet)
        {
            List<Dictionary<int, string>> data = new List<Dictionary<int, string>>();
            var start = worksheet.Dimension.Start;
            var end = worksheet.Dimension.End;
            for (int row = start.Row; row <= end.Row; row++)
            { // Row by row...
                Dictionary<int, string> rowdata = new Dictionary<int, string>();
                for (int col = start.Column; col <= end.Column; col++)
                { // ... Cell by cell...
                    rowdata.Add(col - 1, worksheet.Cells[row, col].Text);// This got me the actual value I needed.
                }
                data.Add(rowdata);
            }
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public static List<string[]> GetRows(this ExcelWorksheet worksheet)
        {
            List<string[]> data = new List<string[]>();
            var start = worksheet.Dimension.Start;
            var end = worksheet.Dimension.End;
            for (int row = start.Row; row <= end.Row; row++)
            { // Row by row...
                List<string> rowdata = new List<string>();
                for (int col = start.Column; col <= end.Column; col++)
                { // ... Cell by cell...
                    worksheet.Column(col).Width = 100;

                    if (worksheet.Cells[row, col].Style.Numberformat.NumFmtID == 164|| worksheet.Cells[row, col].Style.Numberformat.NumFmtID == 1 || worksheet.Cells[row, col].Style.Numberformat.NumFmtID == 166)
                    {
                        rowdata.Add(worksheet.Cells[row, col].Text ?? "");// This got me the actual value I needed.

                    }
                    else
                    {
                        rowdata.Add(worksheet.Cells[row, col].GetValue<string>() ?? "");// This got me the actual value I needed.

                    }
                }
                data.Add(rowdata.ToArray());
            }
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public static List<string[]> GetRowsFromXls(this ISheet worksheet)
        {
            List<string[]> rows = new List<string[]>();
            

            for (int row = worksheet.FirstRowNum; row <= worksheet.LastRowNum; row++)
            { // Row by row...
                int rowCounter = 0;
                List<string> columnData = new List<string>();
                for (int col = worksheet.GetRow(row).FirstCellNum; col <= worksheet.GetRow(row).LastCellNum; col++)
                {
                    var colData = worksheet.GetRow(row).GetCell(col) == null ? "" : worksheet.GetRow(row).GetCell(col).ToString();
                    if (string.IsNullOrEmpty(colData))
                    {
                        rowCounter++;
                    }
                    columnData.Add(colData);// This got me the actual value I needed.
                }
                if(rowCounter != columnData.Count)
                    rows.Add(columnData.ToArray());
            }
            return rows;
        }
    }
}
