//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Campus.Windows;
//using System.Windows.Forms;
//using System.Windows.Forms.Calendar;

//namespace Sunset
//{
//    public class TimeTableCalendarEditor : UserControl, IContentEditor<TimeTablePackage>
//    {
//        private System.Windows.Forms.Calendar.Calendar calTimeTable;
//        private TimeTablePackage mTimeTablePackage;

//        public TimeTableCalendarEditor()
//        {
//            InitializeComponent();
//        }

//        #region IContentViewer<TimeTablePackage> 成員

//        public void Prepare()
//        {

//        }

//        public TimeTablePackage Content
//        {
//            get 
//            { 
//                return mTimeTablePackage ; 
//            }
//            set 
//            {
//                mTimeTablePackage = value ;

//                List<TimeTableSec> OrderTimeTableSecs =  mTimeTablePackage.TimeTableSecs.OrderBy(x => x.BeginTime).ToList();

//                //calTimeTable.ViewStart = new DateTime(1900, 1, 1);
//                //calTimeTable.ViewEnd = new DateTime(1900, 1, 5);
//                calTimeTable.SetViewRange(new DateTime(1900, 1, 1,7,0,0),new DateTime(1900, 1, 6,22,0,0));
//                calTimeTable.TimeScale = CalendarTimeScale.TenMinutes;
//                calTimeTable.TimeUnitsOffset = -6 * OrderTimeTableSecs[0].BeginTime.Hour;
//                calTimeTable.AutoScroll = true;
//                calTimeTable.AutoScrollMinSize = new System.Drawing.Size(6,6);
//                calTimeTable.AllowItemEdit = false;
//                calTimeTable.AllowNew = false;
                
//            }
//        }

//        public new UserControl Control
//        {
//            get { return this; }
//        }

//        #endregion

//        private void InitializeComponent()
//        {
//            System.Windows.Forms.Calendar.CalendarHighlightRange calendarHighlightRange1 = new System.Windows.Forms.Calendar.CalendarHighlightRange();
//            System.Windows.Forms.Calendar.CalendarHighlightRange calendarHighlightRange2 = new System.Windows.Forms.Calendar.CalendarHighlightRange();
//            System.Windows.Forms.Calendar.CalendarHighlightRange calendarHighlightRange3 = new System.Windows.Forms.Calendar.CalendarHighlightRange();
//            System.Windows.Forms.Calendar.CalendarHighlightRange calendarHighlightRange4 = new System.Windows.Forms.Calendar.CalendarHighlightRange();
//            System.Windows.Forms.Calendar.CalendarHighlightRange calendarHighlightRange5 = new System.Windows.Forms.Calendar.CalendarHighlightRange();
//            this.calTimeTable = new System.Windows.Forms.Calendar.Calendar();
//            this.SuspendLayout();
//            // 
//            // calTimeTable
//            // 
//            this.calTimeTable.AutoScroll = true;
//            this.calTimeTable.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.calTimeTable.FirstDayOfWeek = System.DayOfWeek.Monday;
//            this.calTimeTable.Font = new System.Drawing.Font("微軟正黑體", 9F);
//            calendarHighlightRange1.DayOfWeek = System.DayOfWeek.Monday;
//            calendarHighlightRange1.EndTime = System.TimeSpan.Parse("17:00:00");
//            calendarHighlightRange1.StartTime = System.TimeSpan.Parse("08:00:00");
//            calendarHighlightRange2.DayOfWeek = System.DayOfWeek.Tuesday;
//            calendarHighlightRange2.EndTime = System.TimeSpan.Parse("17:00:00");
//            calendarHighlightRange2.StartTime = System.TimeSpan.Parse("08:00:00");
//            calendarHighlightRange3.DayOfWeek = System.DayOfWeek.Wednesday;
//            calendarHighlightRange3.EndTime = System.TimeSpan.Parse("17:00:00");
//            calendarHighlightRange3.StartTime = System.TimeSpan.Parse("08:00:00");
//            calendarHighlightRange4.DayOfWeek = System.DayOfWeek.Thursday;
//            calendarHighlightRange4.EndTime = System.TimeSpan.Parse("17:00:00");
//            calendarHighlightRange4.StartTime = System.TimeSpan.Parse("08:00:00");
//            calendarHighlightRange5.DayOfWeek = System.DayOfWeek.Friday;
//            calendarHighlightRange5.EndTime = System.TimeSpan.Parse("17:00:00");
//            calendarHighlightRange5.StartTime = System.TimeSpan.Parse("08:00:00");
//            this.calTimeTable.HighlightRanges = new System.Windows.Forms.Calendar.CalendarHighlightRange[] {
//        calendarHighlightRange1,
//        calendarHighlightRange2,
//        calendarHighlightRange3,
//        calendarHighlightRange4,
//        calendarHighlightRange5};
//            this.calTimeTable.Location = new System.Drawing.Point(0, 0);
//            this.calTimeTable.Name = "calTimeTable";
//            this.calTimeTable.Size = new System.Drawing.Size(648, 586);
//            this.calTimeTable.TabIndex = 0;
//            this.calTimeTable.Text = "時間表";
//            this.calTimeTable.TimeScale = System.Windows.Forms.Calendar.CalendarTimeScale.FifteenMinutes;
//            this.calTimeTable.LoadItems += new System.Windows.Forms.Calendar.Calendar.CalendarLoadEventHandler(this.calTimeTable_LoadItems);
//            // 
//            // TimeTableCalendarEditor
//            // 
//            this.Controls.Add(this.calTimeTable);
//            this.Name = "TimeTableCalendarEditor";
//            this.Size = new System.Drawing.Size(648, 586);
//            this.ResumeLayout(false);

//        }

//        private void calTimeTable_LoadItems(object sender, System.Windows.Forms.Calendar.CalendarLoadEventArgs e)
//        {
//            if (mTimeTablePackage!=null && !K12.Data.Utility.Utility.IsNullOrEmpty(mTimeTablePackage.TimeTableSecs))
//            {
//                foreach(TimeTableSec TimeTableSec in mTimeTablePackage.TimeTableSecs)
//                {
//                    DateTime Start = new DateTime(1900,1,TimeTableSec.WeekDay,TimeTableSec.BeginTime.Hour,TimeTableSec.BeginTime.Minute,0);

//                    TimeSpan Duration = new TimeSpan(0,TimeTableSec.Duration,0);

//                    StringBuilder strBuilder = new StringBuilder();

//                    strBuilder.AppendLine("節次：" + TimeTableSec.Period);
//                    strBuilder.AppendLine("時間：" + TimeTableSec.BeginTime.Hour + ":" + TimeTableSec.BeginTime.Minute);
//                    strBuilder.AppendLine("分鐘：" + TimeTableSec.Duration);


//                    CalendarItem Item = new CalendarItem(e.Calendar,Start,Duration,strBuilder.ToString());
//                    Item.BackgroundColor = System.Drawing.Color.Blue;

//                    calTimeTable.Items.Add(Item);
//                }
//            }
//        }
//    }
//}
