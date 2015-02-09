using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Aspose.Cells;
using Campus.Report;
using FISCA.Data;
using System.Collections.Generic;

namespace Sunset
{
    /// <summary>
    /// 匯出查詢類別
    /// </summary>
    public class QueryExport
    {
        /// <summary>
        /// 執行查詢並匯出成Excel格式
        /// </summary>
        /// <param name="QueryName">查詢名稱</param>
        /// <param name="Query">查詢內容</param>
        public static void Execute(string QueryName, string Query, string SplitField)
        {
            QueryHelper mQuery = new QueryHelper();

            FISCA.Presentation.MotherForm.SetStatusBarMessage("查詢資料中!");

            DataTable mTableResult = mQuery.Select(Query);

            FISCA.Presentation.MotherForm.SetStatusBarMessage("查詢完成!");

            BackgroundWorker bkwExport = new BackgroundWorker();

            bkwExport.WorkerReportsProgress = true;
            bkwExport.DoWork += delegate(object vsender, DoWorkEventArgs ve)
            {
                DataTable Table = (ve.Argument as DataTable);
                Workbook book = new Workbook();
                book.Worksheets.Clear();

                Dictionary<string, int> NameIndex = new Dictionary<string, int>();

                if (Table != null)
                {
                    int RowIndex = 1;

                    foreach (DataRow Row in Table.Rows)
                    {
                        string SplitValue = Row.Field<string>(SplitField);

                        SplitValue = SplitValue.Equals(string.Empty) ? "未分類" : SplitValue;

                        if (!NameIndex.ContainsKey(SplitValue))
                        {
                            NameIndex.Add(SplitValue, 1);
                            int sheetIndex = book.Worksheets.Add();
                            book.Worksheets[sheetIndex].Name = SplitValue;
                            for (int i = 0; i < Table.Columns.Count; i++)
                                book.Worksheets[SplitValue].Cells[0, i].PutValue(Table.Columns[i].ColumnName);
                        }

                        for (int ColumnIndex = 0; ColumnIndex < Table.Columns.Count; ColumnIndex++)
                            book.Worksheets[SplitValue].Cells[NameIndex[SplitValue], ColumnIndex].PutValue(Row.Field<string>(ColumnIndex));

                        bkwExport.ReportProgress((RowIndex / Table.Rows.Count) * 100);
                        NameIndex[SplitValue]++;
                        RowIndex++;
                    }
                }

                foreach (Worksheet sheet in book.Worksheets)
                {
                    sheet.AutoFitColumns();
                    sheet.AutoFitRows();
                }

                ve.Result = book;
            };

            bkwExport.ProgressChanged += delegate(object vsender, ProgressChangedEventArgs ve)
            {
                FISCA.Presentation.MotherForm.SetStatusBarMessage("產生中...", ve.ProgressPercentage);
            };

            bkwExport.RunWorkerCompleted += delegate(object vsender, RunWorkerCompletedEventArgs ve)
            {
                Workbook vWorkbook = (Workbook)ve.Result;

                FISCA.Presentation.MotherForm.SetStatusBarMessage("產生完成!");
                ReportSaver.SaveWorkbook(vWorkbook, Application.StartupPath + "\\Reports\\" + QueryName + ".xls");
            };

            bkwExport.RunWorkerAsync(mTableResult);
        }

        /// <summary>
        /// 執行查詢並匯出成Excel格式
        /// </summary>
        /// <param name="QueryName">查詢名稱</param>
        /// <param name="Query">查詢內容</param>
        public static void Execute(string QueryName, string Query)
        {
            QueryHelper mQuery = new QueryHelper();

            FISCA.Presentation.MotherForm.SetStatusBarMessage("查詢資料中!");

            DataTable mTableResult = mQuery.Select(Query);

            FISCA.Presentation.MotherForm.SetStatusBarMessage("查詢完成!");

            BackgroundWorker bkwExport = new BackgroundWorker();

            bkwExport.WorkerReportsProgress = true;
            bkwExport.DoWork += delegate(object vsender, DoWorkEventArgs ve)
            {
                DataTable Table = (ve.Argument as DataTable);
                Workbook book = new Workbook();

                if (Table != null)
                {
                    for (int i = 0; i < Table.Columns.Count; i++)
                        book.Worksheets[0].Cells[0, i].PutValue(Table.Columns[i].ColumnName);

                    int RowIndex = 1;

                    foreach (DataRow Row in Table.Rows)
                    {
                        for (int ColumnIndex = 0; ColumnIndex < Table.Columns.Count; ColumnIndex++)
                            book.Worksheets[0].Cells[RowIndex, ColumnIndex].PutValue(Row.Field<string>(ColumnIndex));
                        bkwExport.ReportProgress((RowIndex / Table.Rows.Count) * 100);
                        RowIndex++;
                    }
                }

                book.Worksheets[0].AutoFitColumns();
                book.Worksheets[0].AutoFitRows();

                ve.Result = book;
            };

            bkwExport.ProgressChanged += delegate(object vsender, ProgressChangedEventArgs ve)
            {
                FISCA.Presentation.MotherForm.SetStatusBarMessage("產生中...", ve.ProgressPercentage);
            };

            bkwExport.RunWorkerCompleted += delegate(object vsender, RunWorkerCompletedEventArgs ve)
            {
                Workbook vWorkbook = (Workbook)ve.Result;
                vWorkbook.Worksheets[0].Name = QueryName;

                Sunset.NewCourse.NwSave.SaveExcel(QueryName, vWorkbook);
                //ReportSaver.SaveWorkbook(vWorkbook, Application.StartupPath + "\\Reports\\" + QueryName + ".xls");
            };

            bkwExport.RunWorkerAsync(mTableResult);
        }
    }
}