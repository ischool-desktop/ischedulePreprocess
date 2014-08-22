using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aspose.Cells;

namespace Sunset.NewCourse
{
    static public class NwSave
    {
        /// <summary>
        /// DataGridView匯出Excel檔案
        /// </summary>
        /// <param name="name">檔名</param>
        /// <param name="filter">Excel (*.xls)|*.xls</param>
        /// <param name="x">DataGridView</param>
        static public void SaveDataGridView(string name, string filter, DataGridView x)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = name;
            saveFileDialog1.Filter = "Excel (*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

            DataGridViewExport export = new DataGridViewExport(x);
            export.Save(saveFileDialog1.FileName);

            if (new CompleteForm().ShowDialog() == DialogResult.Yes)
                System.Diagnostics.Process.Start(saveFileDialog1.FileName);
        }

        /// <summary>
        /// 匯出Excel檔案
        /// </summary>
        /// <param name="name">檔名</param>
        /// <param name="filter">Excel (*.xls)|*.xls</param>
        /// <param name="x">DataGridView</param>
        static public void SaveExcel(string name, Workbook excel)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = name;
            saveFileDialog1.Filter = "Excel (*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

            excel.Save(saveFileDialog1.FileName);

            if (new CompleteForm().ShowDialog() == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(saveFileDialog1.FileName);
            }

            FISCA.Presentation.MotherForm.SetStatusBarMessage("檔案儲存完成：" + name + ".xls");
        }
    }
}
