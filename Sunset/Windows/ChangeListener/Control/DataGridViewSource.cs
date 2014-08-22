//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Windows.Forms;
//using DevComponents.DotNetBar.Controls;

//namespace Sunset
//{
//    public class DataGridViewSource : ChangeSource
//    {
//        public DataGridViewSource(DataGridViewX control)
//        {
//            Grid = control;
//            OriginValues = new Dictionary<Point, string>();
//            SubscribeControlEvents();
//        }

//        private void SubscribeControlEvents()
//        {
//            Grid.RowsAdded += new DataGridViewRowsAddedEventHandler(Grid_RowsAdded);
//            Grid.RowsRemoved += new DataGridViewRowsRemovedEventHandler(Grid_RowsRemoved);
//            Grid.CurrentCellDirtyStateChanged += new EventHandler(Grid_CurrentCellDirtyStateChanged);
//        }

//        private void Grid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
//        {
//            CompareValues();
//        }

//        private void Grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
//        {
//            CompareValues();
//        }

//        private void Grid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
//        {
//            CompareValues();
//        }

//        private void CompareValues()
//        {
//            if (Suspend) return;

//            bool changed = false;
//            foreach (DataGridViewRow row in Grid.Rows)
//            {
//                foreach (DataGridViewCell cell in row.Cells)
//                {
//                    Point location = new Point(cell.ColumnIndex, cell.RowIndex);
//                    string originValue = string.Empty, newValue = string.Empty;

//                    if (OriginValues.ContainsKey(location))
//                        originValue = OriginValues[location];

//                    newValue = cell.Value + "";

//                    if (originValue != newValue)
//                    {
//                        changed = true;
//                        break;
//                    }
//                }

//                if (changed) break;
//            }

//            if (changed)
//                RaiseStatusChanged(ValueStatus.Dirty);
//            else
//                RaiseStatusChanged(ValueStatus.Clean);
//        }

//        public override void Reset()
//        {
//            OriginValues = new Dictionary<Point, string>();

//            foreach (DataGridViewRow row in Grid.Rows)
//            {
//                foreach (DataGridViewCell cell in row.Cells)
//                    OriginValues.Add(new Point(cell.ColumnIndex, cell.RowIndex), cell.Value + "");
//            }
//        }

//        protected Dictionary<Point, string> OriginValues { get; set; }

//        protected DataGridView Grid { get; set; }

//    }
//}