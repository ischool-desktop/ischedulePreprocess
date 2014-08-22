using System;
using System.Collections.Generic;
using System.Text;

namespace Sunset
{
    /// <summary>
    /// 匯出排課系統相關資料
    /// </summary>
    public class ExportSunset
    {
        /// <summary>
        /// 匯出所有班級
        /// </summary>
        public static void ExportClass_New()
        {
            QueryExport.Execute("班級", "select $scheduler.class_ex.class_name as 班級名稱,$scheduler.class_ex.grade_year as 班級年級 from $scheduler.class_ex order by $scheduler.class_ex.grade_year,$scheduler.class_ex.class_name");
        }

        /// <summary>
        /// 匯出所有班級不排課時段
        /// (2013/4/23)
        /// </summary>
        public static void ExportClassBusy_New()
        {
            StringBuilder SQLBuilder = new StringBuilder();
            SQLBuilder.AppendLine("select $scheduler.class_ex.class_name as 班級名稱,$scheduler.class_ex_busy.weekday as 星期,to_char($scheduler.class_ex_busy.begin_time, 'HH24:MI') as 開始時間,to_char($scheduler.class_ex_busy.begin_time + cast($scheduler.class_ex_busy.duration || ' minute' as interval),'HH24:MI') as 結束時間,$scheduler.class_ex_busy.busy_description as 不排課描述 ");
            SQLBuilder.AppendLine("from $scheduler.class_ex left outer join $scheduler.class_ex_busy on $scheduler.class_ex_busy.ref_class_id=$scheduler.class_ex.uid ");
            SQLBuilder.AppendLine("order by $scheduler.class_ex.class_name,$scheduler.class_ex_busy.weekday,$scheduler.class_ex_busy.begin_time");

            QueryExport.Execute("班級不排課時段", SQLBuilder.ToString());
        }


        /// <summary>
        /// 匯出排課獨立教師清單
        /// </summary>
        public static void ExportTeacherEx_New()
        {
            QueryExport.Execute("教師", "select $scheduler.teacher_ex.teacher_name as 教師姓名,$scheduler.teacher_ex.nickname as 教師暱稱 from $scheduler.teacher_ex order by $scheduler.teacher_ex.teacher_name,$scheduler.teacher_ex.nickname");
        }

        /// <summary>
        /// 匯出排課獨立教師不排課清單
        /// </summary>
        /// <param name="TeacherIDs"></param>
        public static void ExportTeacherExBusy_New()
        {
            StringBuilder SQLBuilder = new StringBuilder();
            SQLBuilder.AppendLine("select $scheduler.teacher_ex.teacher_name as 教師姓名,$scheduler.teacher_ex.nickname as 教師暱稱,$scheduler.teacher_ex_busy.weekday as 星期,to_char($scheduler.teacher_ex_busy.begin_time, 'HH24:MI') as 開始時間,to_char($scheduler.teacher_ex_busy.begin_time + cast($scheduler.teacher_ex_busy.duration || ' minute' as interval),'HH24:MI') as 結束時間,$scheduler.teacher_ex_busy.busy_description as 不排課描述 ");
            SQLBuilder.AppendLine("from $scheduler.teacher_ex left outer join $scheduler.teacher_ex_busy on $scheduler.teacher_ex_busy.ref_teacher_id=$scheduler.teacher_ex.uid left outer join $scheduler.location on $scheduler.teacher_ex_busy.ref_location_id=$scheduler.location.uid ");
            SQLBuilder.AppendLine("order by $scheduler.teacher_ex.teacher_name,$scheduler.teacher_ex.nickname,$scheduler.teacher_ex_busy.weekday,$scheduler.teacher_ex_busy.begin_time");

            QueryExport.Execute("教師不排課時段", SQLBuilder.ToString());
        }

        /// <summary>
        /// 匯出選取班級排課資料（目前未使用）
        /// </summary>
        /// <param name="ClassIDs">班級系統編號列表</param>
        public static void ExportClassExtenstion(List<string> ClassIDs)
        {
            if (!K12.Data.Utility.Utility.IsNullOrEmpty(ClassIDs))
            {
                StringBuilder SQLBuilder = new StringBuilder();

                SQLBuilder.AppendLine("select class.class_name as 班級名稱,$scheduler.timetable.name as 時間表名稱 ");
                SQLBuilder.AppendLine("from class left outer join  $scheduler.class_extension on class.id=$scheduler.class_extension.ref_class_id left outer join $scheduler.timetable on $scheduler.timetable.uid=$scheduler.class_extension.ref_timetable_id ");
                SQLBuilder.AppendFormat("where class.id in ({0}) order by class.class_name", string.Join(",", ClassIDs.ToArray()));

                QueryExport.Execute("班級排課資料", SQLBuilder.ToString());
            }
        }

        /// <summary>
        /// 匯出所有時間表分段
        /// </summary>
        public static void ExportTimeTableSec()
        {
            //QueryExport.Execute("時間表分段", "select $scheduler.timetable_section.uid as 系統編號,$scheduler.timetable.name as 時間表名稱,$scheduler.timetable_section.weekday as 星期,$scheduler.timetable_section.period as 節次,date_part('hour',$scheduler.timetable_section.begin_time) as 開始小時,date_part('minute',$scheduler.timetable_section.begin_time) as 開始分鐘, $scheduler.timetable_section.duration as 持續分鐘,$scheduler.timetable_section.display_period as 顯示節次,$scheduler.location.name as 地點名稱 from $scheduler.timetable_section inner join $scheduler.timetable on $scheduler.timetable_section.ref_timetable_id = $scheduler.timetable.uid left outer join $scheduler.location on $scheduler.timetable_section.ref_location_id = $scheduler.location.uid order by $scheduler.timetable.name,$scheduler.timetable_section.weekday,$scheduler.timetable_section.period");

            //QueryExport.Execute("時間表分段", "select $scheduler.timetable.name as 時間表名稱,$scheduler.timetable_section.weekday as 星期,$scheduler.timetable_section.period as 節次,date_part('hour',$scheduler.timetable_section.begin_time) as 開始小時,date_part('minute',$scheduler.timetable_section.begin_time) as 開始分鐘, $scheduler.timetable_section.duration as 持續分鐘,$scheduler.timetable_section.display_period as 顯示節次,$scheduler.location.name as 地點名稱 from $scheduler.timetable_section inner join $scheduler.timetable on $scheduler.timetable_section.ref_timetable_id = $scheduler.timetable.uid left outer join $scheduler.location on $scheduler.timetable_section.ref_location_id = $scheduler.location.uid order by $scheduler.timetable.name,$scheduler.timetable_section.weekday,$scheduler.timetable_section.period");

            QueryExport.Execute("時間表分段", "select $scheduler.timetable.name as 時間表名稱,$scheduler.timetable_section.weekday as 星期,$scheduler.timetable_section.period as 節次,date_part('hour',$scheduler.timetable_section.begin_time) as 開始小時,date_part('minute',$scheduler.timetable_section.begin_time) as 開始分鐘,$scheduler.timetable_section.duration as 持續分鐘,$scheduler.timetable_section.display_period as 顯示節次 from $scheduler.timetable_section inner join $scheduler.timetable on $scheduler.timetable_section.ref_timetable_id = $scheduler.timetable.uid left outer join $scheduler.location on $scheduler.timetable_section.ref_location_id = $scheduler.location.uid order by $scheduler.timetable.name,$scheduler.timetable_section.weekday,$scheduler.timetable_section.period");
        }

        /// <summary>
        /// 匯出所有時間表分段
        /// </summary>
        public static void ExportTimeTableSec2()
        {
            //QueryExport.Execute("時間表分段", "select $scheduler.timetable_section.uid as 系統編號,$scheduler.timetable.name as 時間表名稱,$scheduler.timetable_section.weekday as 星期,$scheduler.timetable_section.period as 節次,to_char($scheduler.timetable_section.begin_time, 'HH24:MI') as 開始時間,to_char($scheduler.timetable_section.begin_time + cast($scheduler.timetable_section.duration || ' minute' as interval),'HH24:MI') as 結束時間,$scheduler.timetable_section.display_period as 顯示節次,$scheduler.location.name as 地點名稱 from $scheduler.timetable_section inner join $scheduler.timetable on $scheduler.timetable_section.ref_timetable_id = $scheduler.timetable.uid left outer join $scheduler.location on $scheduler.timetable_section.ref_location_id = $scheduler.location.uid order by $scheduler.timetable.name,$scheduler.timetable_section.weekday,$scheduler.timetable_section.period");

            //QueryExport.Execute("時間表分段", "select $scheduler.timetable.name as 時間表名稱,$scheduler.timetable_section.weekday as 星期,$scheduler.timetable_section.period as 節次,to_char($scheduler.timetable_section.begin_time, 'HH24:MI') as 開始時間,to_char($scheduler.timetable_section.begin_time + cast($scheduler.timetable_section.duration || ' minute' as interval),'HH24:MI') as 結束時間,$scheduler.timetable_section.display_period as 顯示節次,$scheduler.location.name as 地點名稱,(CASE WHEN $scheduler.timetable_section.disable=true THEN '是' ELSE '' END) as 不排課,$scheduler.timetable_section.disable_message as 不排課訊息 from $scheduler.timetable_section inner join $scheduler.timetable on $scheduler.timetable_section.ref_timetable_id = $scheduler.timetable.uid left outer join $scheduler.location on $scheduler.timetable_section.ref_location_id = $scheduler.location.uid order by $scheduler.timetable.name,$scheduler.timetable_section.weekday,$scheduler.timetable_section.period");

            //QueryExport.Execute("時間表分段", "select $scheduler.timetable.name as 時間表名稱,$scheduler.timetable_section.weekday as 星期,$scheduler.timetable_section.period as 節次,to_char($scheduler.timetable_section.begin_time, 'HH24:MI') as 開始時間,to_char($scheduler.timetable_section.begin_time + cast($scheduler.timetable_section.duration || ' minute' as interval),'HH24:MI') as 結束時間,$scheduler.timetable_section.display_period as 顯示節次,(CASE WHEN $scheduler.timetable_section.disable=true THEN '是' ELSE '' END) as 不排課,$scheduler.timetable_section.disable_message as 不排課訊息 from $scheduler.timetable_section inner join $scheduler.timetable on $scheduler.timetable_section.ref_timetable_id = $scheduler.timetable.uid left outer join $scheduler.location on $scheduler.timetable_section.ref_location_id = $scheduler.location.uid order by $scheduler.timetable.name,$scheduler.timetable_section.weekday,$scheduler.timetable_section.period");

            QueryExport.Execute("時間表分段", "select $scheduler.timetable.name as 時間表名稱,$scheduler.timetable_section.weekday as 星期,$scheduler.timetable_section.period as 節次,to_char($scheduler.timetable_section.begin_time, 'HH24:MI') as 開始時間,to_char($scheduler.timetable_section.begin_time + cast($scheduler.timetable_section.duration || ' minute' as interval),'HH24:MI') as 結束時間,(CASE WHEN $scheduler.timetable_section.disable=true THEN '是' ELSE '' END) as 不排課,$scheduler.timetable_section.disable_message as 不排課訊息 from $scheduler.timetable_section inner join $scheduler.timetable on $scheduler.timetable_section.ref_timetable_id = $scheduler.timetable.uid left outer join $scheduler.location on $scheduler.timetable_section.ref_location_id = $scheduler.location.uid order by $scheduler.timetable.name,$scheduler.timetable_section.weekday,$scheduler.timetable_section.period");
        }

        /// <summary>
        /// 匯出所有時間表
        /// </summary>
        public static void ExportTimeTable()
        {
            //QueryExport.Execute("時間表", "select $scheduler.timetable.uid as 系統編號,$scheduler.timetable.name as 時間表名稱,$scheduler.timetable.description as 時間表描述 from $scheduler.timetable order by $scheduler.timetable.name"); 

            QueryExport.Execute("時間表", "select $scheduler.timetable.name as 時間表名稱,$scheduler.timetable.description as 時間表描述 from $scheduler.timetable order by $scheduler.timetable.name");
        }

        /// <summary>
        /// 匯出所有場地
        /// </summary>
        public static void ExportClassroom()
        {
            //QueryExport.Execute("場地", "select $scheduler.classroom.uid as 系統編號,$scheduler.classroom.code as 場地代碼,$scheduler.classroom.name as 場地名稱,$scheduler.classroom.capacity as 場地班級容納數,$scheduler.classroom.description as 場地描述,(CASE WHEN $scheduler.classroom.location_only=true THEN '是' WHEN $scheduler.classroom.location_only=false THEN '' ELSE '' END) as 無班級容納數限制,$scheduler.location.name as 地點名稱 from $scheduler.classroom left outer join $scheduler.location on $scheduler.classroom.ref_location_id = $scheduler.location.uid order by $scheduler.classroom.name");

            //QueryExport.Execute("場地", "select $scheduler.classroom.name as 場地名稱,$scheduler.classroom.capacity as 場地班級容納數,$scheduler.classroom.description as 場地描述,(CASE WHEN $scheduler.classroom.location_only=true THEN '是' WHEN $scheduler.classroom.location_only=false THEN '' ELSE '' END) as 無班級容納數限制,$scheduler.location.name as 地點名稱 from $scheduler.classroom left outer join $scheduler.location on $scheduler.classroom.ref_location_id = $scheduler.location.uid order by $scheduler.classroom.name");

            //QueryExport.Execute("場地", "select $scheduler.classroom.name as 場地名稱,$scheduler.classroom.capacity as 場地班級容納數,$scheduler.classroom.description as 場地描述,(CASE WHEN $scheduler.classroom.location_only=true THEN '是' WHEN $scheduler.classroom.location_only=false THEN '' ELSE '' END) as 無班級容納數限制 from $scheduler.classroom left outer join $scheduler.location on $scheduler.classroom.ref_location_id = $scheduler.location.uid order by $scheduler.classroom.name");

            QueryExport.Execute("場地", "select $scheduler.classroom.name as 場地名稱,$scheduler.classroom.capacity as 場地班級容納數,$scheduler.classroom.description as 場地描述 from $scheduler.classroom left outer join $scheduler.location on $scheduler.classroom.ref_location_id = $scheduler.location.uid order by $scheduler.classroom.name");
        }

        /// <summary>
        /// 匯出所有場地不排課時段
        /// </summary>
        public static void ExportClassroomBusy()
        {
            //QueryExport.Execute("場地不排課時段", "select $scheduler.classroom_busy.uid as 系統編號,$scheduler.classroom.name as 場地名稱,$scheduler.classroom_busy.weekday as 星期,date_part('hour',$scheduler.classroom_busy.begin_time) as 開始小時,date_part('minute',$scheduler.classroom_busy.begin_time) as 開始分鐘, $scheduler.classroom_busy.duration as 持續分鐘,(CASE WHEN $scheduler.classroom_busy.week_flag=1 THEN '單' WHEN $scheduler.classroom_busy.week_flag=2 THEN '雙' ELSE '單雙' END) as 單雙週,$scheduler.classroom_busy.busy_description as 不排課描述 from $scheduler.classroom_busy inner join $scheduler.classroom on $scheduler.classroom_busy.ref_classroom_id= $scheduler.classroom.uid order by $scheduler.classroom.name,$scheduler.classroom_busy.weekday,$scheduler.classroom_busy.begin_time");

            QueryExport.Execute("場地不排課時段", "select $scheduler.classroom.name as 場地名稱,$scheduler.classroom_busy.weekday as 星期,date_part('hour',$scheduler.classroom_busy.begin_time) as 開始小時,date_part('minute',$scheduler.classroom_busy.begin_time) as 開始分鐘, $scheduler.classroom_busy.duration as 持續分鐘,(CASE WHEN $scheduler.classroom_busy.week_flag=1 THEN '單' WHEN $scheduler.classroom_busy.week_flag=2 THEN '雙' ELSE '單雙' END) as 單雙週,$scheduler.classroom_busy.busy_description as 不排課描述 from $scheduler.classroom_busy inner join $scheduler.classroom on $scheduler.classroom_busy.ref_classroom_id= $scheduler.classroom.uid order by $scheduler.classroom.name,$scheduler.classroom_busy.weekday,$scheduler.classroom_busy.begin_time");
        }

        /// <summary>
        /// 匯出所有場地不排課時段
        /// </summary>
        public static void ExportClassroomBusy2()
        {
            //QueryExport.Execute("場地不排課時段", "select $scheduler.classroom_busy.uid as 系統編號,$scheduler.classroom.name as 場地名稱,$scheduler.classroom_busy.weekday as 星期,to_char($scheduler.classroom_busy.begin_time, 'HH24:MI') as 開始時間, to_char($scheduler.classroom_busy.begin_time + cast($scheduler.classroom_busy.duration || ' minute' as interval),'HH24:MI') as 結束時間,(CASE WHEN $scheduler.classroom_busy.week_flag=1 THEN '單' WHEN $scheduler.classroom_busy.week_flag=2 THEN '雙' ELSE '單雙' END) as 單雙週,$scheduler.classroom_busy.busy_description as 不排課描述 from $scheduler.classroom_busy inner join $scheduler.classroom on $scheduler.classroom_busy.ref_classroom_id= $scheduler.classroom.uid order by $scheduler.classroom.name,$scheduler.classroom_busy.weekday,$scheduler.classroom_busy.begin_time");

            QueryExport.Execute("場地不排課時段", "select $scheduler.classroom.name as 場地名稱,$scheduler.classroom_busy.weekday as 星期,to_char($scheduler.classroom_busy.begin_time, 'HH24:MI') as 開始時間, to_char($scheduler.classroom_busy.begin_time + cast($scheduler.classroom_busy.duration || ' minute' as interval),'HH24:MI') as 結束時間,(CASE WHEN $scheduler.classroom_busy.week_flag=1 THEN '單' WHEN $scheduler.classroom_busy.week_flag=2 THEN '雙' ELSE '單雙' END) as 單雙週,$scheduler.classroom_busy.busy_description as 不排課描述 from $scheduler.classroom_busy inner join $scheduler.classroom on $scheduler.classroom_busy.ref_classroom_id= $scheduler.classroom.uid order by $scheduler.classroom.name,$scheduler.classroom_busy.weekday,$scheduler.classroom_busy.begin_time");
        }

        /// <summary>
        /// 匯出選取班級不排課時段
        /// </summary>
        /// <param name="ClassIDs">班級系統編號列表</param>
        public static void ExportClassBusy(List<string> ClassIDs)
        {
            if (!K12.Data.Utility.Utility.IsNullOrEmpty(ClassIDs))
            {
                StringBuilder SQLBuilder = new StringBuilder();

                //SQLBuilder.AppendLine("select $scheduler.teacher_busy.uid as 系統編號,teacher.teacher_name as 教師姓名,teacher.nickname as 教師暱稱,$scheduler.teacher_busy.weekday as 星期,to_char($scheduler.teacher_busy.begin_time, 'HH24:MI') as 開始時間,to_char($scheduler.teacher_busy.begin_time + cast($scheduler.teacher_busy.duration || ' minute' as interval),'HH24:MI') as 結束時間,$scheduler.teacher_busy.busy_description as 不排課描述,$scheduler.location.name as 所在地點 ");
                //SQLBuilder.AppendLine("select teacher.teacher_name as 教師姓名,teacher.nickname as 教師暱稱,$scheduler.teacher_busy.weekday as 星期,to_char($scheduler.teacher_busy.begin_time, 'HH24:MI') as 開始時間,to_char($scheduler.teacher_busy.begin_time + cast($scheduler.teacher_busy.duration || ' minute' as interval),'HH24:MI') as 結束時間,$scheduler.teacher_busy.busy_description as 不排課描述,$scheduler.location.name as 所在地點 ");


                SQLBuilder.AppendLine("select class.class_name as 班級名稱,$scheduler.class_busy.weekday as 星期,to_char($scheduler.class_busy.begin_time, 'HH24:MI') as 開始時間,to_char($scheduler.class_busy.begin_time + cast($scheduler.class_busy.duration || ' minute' as interval),'HH24:MI') as 結束時間,$scheduler.class_busy.busy_description as 不排課描述 ");
                SQLBuilder.AppendLine("from class left outer join $scheduler.class_busy on $scheduler.class_busy.ref_class_id=class.id ");
                SQLBuilder.AppendLine("where class.id in (" + string.Join(",", ClassIDs.ToArray()) + ") order by class.grade_year,class.class_name,$scheduler.class_busy.weekday,$scheduler.class_busy.begin_time");

                QueryExport.Execute("班級不排課時段", SQLBuilder.ToString());
            }
        }

        /// <summary>
        /// 匯出排課獨立教師清單
        /// </summary>
        /// <param name="TeacherIDs"></param>
        public static void ExportTeacherEx(List<string> TeacherIDs)
        {

        }

        /// <summary>
        /// 匯出排課獨立教師不排課清單
        /// </summary>
        /// <param name="TeacherIDs"></param>
        public static void ExportTeacherExBusy(List<string> TeacherIDs)
        {

        }

        /// <summary>
        /// 匯出選取教師不排課時段
        /// </summary>
        /// <param name="TeacherIDs">教師系統編號列表</param>
        public static void ExportTeacherBusy(List<string> TeacherIDs)
        {
            if (!K12.Data.Utility.Utility.IsNullOrEmpty(TeacherIDs))
            {
                StringBuilder SQLBuilder = new StringBuilder();

                //SQLBuilder.AppendLine("select $scheduler.teacher_busy.uid as 系統編號,teacher.teacher_name as 教師姓名,teacher.nickname as 教師暱稱,$scheduler.teacher_busy.weekday as 星期,to_char($scheduler.teacher_busy.begin_time, 'HH24:MI') as 開始時間,to_char($scheduler.teacher_busy.begin_time + cast($scheduler.teacher_busy.duration || ' minute' as interval),'HH24:MI') as 結束時間,$scheduler.teacher_busy.busy_description as 不排課描述,$scheduler.location.name as 所在地點 ");
                //SQLBuilder.AppendLine("select teacher.teacher_name as 教師姓名,teacher.nickname as 教師暱稱,$scheduler.teacher_busy.weekday as 星期,to_char($scheduler.teacher_busy.begin_time, 'HH24:MI') as 開始時間,to_char($scheduler.teacher_busy.begin_time + cast($scheduler.teacher_busy.duration || ' minute' as interval),'HH24:MI') as 結束時間,$scheduler.teacher_busy.busy_description as 不排課描述,$scheduler.location.name as 所在地點 ");


                SQLBuilder.AppendLine("select teacher.teacher_name as 教師姓名,teacher.nickname as 教師暱稱,$scheduler.teacher_busy.weekday as 星期,to_char($scheduler.teacher_busy.begin_time, 'HH24:MI') as 開始時間,to_char($scheduler.teacher_busy.begin_time + cast($scheduler.teacher_busy.duration || ' minute' as interval),'HH24:MI') as 結束時間,$scheduler.teacher_busy.busy_description as 不排課描述 ");
                SQLBuilder.AppendLine("from teacher left outer join $scheduler.teacher_busy on $scheduler.teacher_busy.ref_teacher_id=teacher.id left outer join $scheduler.location on $scheduler.teacher_busy.ref_location_id=$scheduler.location.uid ");
                SQLBuilder.AppendLine("where teacher.id in (" + string.Join(",", TeacherIDs.ToArray()) + ") order by teacher.teacher_name,teacher.nickname,$scheduler.teacher_busy.weekday,$scheduler.teacher_busy.begin_time");

                QueryExport.Execute("教師不排課時段", SQLBuilder.ToString());
            }
        }

        /// <summary>
        /// 匯出選取教師不排課時段
        /// </summary>
        /// <param name="TeacherIDs">教師系統編號列表</param>
        public static void ExportTeacherExtension(List<string> TeacherIDs)
        {
            if (!K12.Data.Utility.Utility.IsNullOrEmpty(TeacherIDs))
            {
                StringBuilder SQLBuilder = new StringBuilder();

                SQLBuilder.AppendLine("select teacher.teacher_name as 教師姓名,teacher.nickname as 教師暱稱,basic_length as 基本時數,extra_length as 兼課時數,counseling_length as 輔導時數,comment as 註記 ");
                SQLBuilder.AppendLine("from teacher left outer join $scheduler.teacher_extension on $scheduler.teacher_extension.ref_teacher_id=teacher.id ");
                SQLBuilder.AppendLine("where teacher.id in (" + string.Join(",", TeacherIDs.ToArray()) + ") order by teacher.teacher_name,teacher.nickname");

                QueryExport.Execute("教師排課資料", SQLBuilder.ToString());
            }
        }

        /// <summary>
        /// 匯出選取課程排課資料
        /// </summary>
        /// <param name="CourseIDs">課程系統編號列表</param>
        public static void ExportCourseExtension(List<string> CourseIDs)
        {
            if (!K12.Data.Utility.Utility.IsNullOrEmpty(CourseIDs))
            {
                StringBuilder SQLBuilder = new StringBuilder();

                SQLBuilder.AppendLine("select course.course_name as 課程名稱,course.school_year as 學年度,course.semester as 學期,course.domain 領域,subject_alias_name as 科目簡稱,$scheduler.timetable.name as 上課時間表,(CASE WHEN $scheduler.course_extension.allow_duplicate=true THEN '是' ELSE '' END) as 同天排課,(CASE WHEN $scheduler.course_extension.limit_next_day=true THEN '是' ELSE '' END) as 不連天排課,course.period as 節數,$scheduler.course_extension.split_spec as 分割設定,$scheduler.classroom.name as 預設場地條件,(CASE WHEN $scheduler.course_extension.week_flag=1 THEN '單' WHEN $scheduler.course_extension.week_flag=2 THEN '雙' ELSE '單雙' END) as 預設單雙週條件,(CASE WHEN $scheduler.course_extension.long_break=true THEN '是' ELSE '' END) as 預設跨中午條件,$scheduler.course_extension.weekday_condition as 預設星期條件,$scheduler.course_extension.period_condition as 預設節次條件 ");
                SQLBuilder.AppendLine("from course left outer join $scheduler.course_extension on $scheduler.course_extension.ref_course_id = course.id left outer join $scheduler.timetable on $scheduler.course_extension.ref_timetable_id= $scheduler.timetable.uid left outer join $scheduler.classroom on  $scheduler.course_extension.ref_classroom_id=$scheduler.classroom.uid ");
                SQLBuilder.AppendLine("where course.id in (" + string.Join(",", CourseIDs.ToArray()) + ") order by course.course_name,course.school_year,course.semester");

                QueryExport.Execute("課程排課資料", SQLBuilder.ToString());
            }
        }

        /// <summary>
        /// 匯出選取課程排課資料
        /// </summary>
        /// <param name="CourseIDs">課程系統編號列表</param>
        public static void ExportCourseExtensionAndCourse(List<string> CourseIDs)
        {
            //班級名稱（改所屬班級）  
            //分項類別（改分項） 學期成績（增加） 授課教師一（ok，加授課教師二、三）
            //課程名稱ok 學年度ok 學期ok  領域ok
            //科目名稱ok 科目級別ok 科目簡稱ok 學分ok
            //節數ok 校部訂ok 必選修ok 不計入學分ok 不評分ok 上課時間表ok 同天排課ok
            //不連天排課ok 分割設定ok 預設場地條件ok 預設單雙週條件ok 預設跨中午條件ok
            //預設星期條件ok 預設節次條件ok 

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(CourseIDs))
            {
                StringBuilder SQLBuilder = new StringBuilder();


                SQLBuilder.AppendLine("select course.course_name as 課程名稱,course.school_year as 學年度,course.semester as 學期,class.class_name as 所屬班級,course.domain as 領域,course.score_type as 分項,course.subject as 科目名稱,course.subj_level as 科目級別,(select (CASE WHEN teacher.nickname='' THEN teacher.teacher_name ELSE teacher.teacher_name || '(' || teacher.nickname || ')'  END) from teacher join tc_instruct on teacher.id=tc_instruct.ref_teacher_id where tc_instruct.ref_course_id=course.id and tc_instruct.sequence=1) as 授課教師一,(select (CASE WHEN teacher.nickname='' THEN teacher.teacher_name ELSE teacher.teacher_name || '(' || teacher.nickname || ')'  END) from teacher join tc_instruct on teacher.id=tc_instruct.ref_teacher_id where tc_instruct.ref_course_id=course.id and tc_instruct.sequence=2) as 授課教師二,(select (CASE WHEN teacher.nickname='' THEN teacher.teacher_name ELSE teacher.teacher_name || '(' || teacher.nickname || ')'  END) from teacher join tc_instruct on teacher.id=tc_instruct.ref_teacher_id where tc_instruct.ref_course_id=course.id and tc_instruct.sequence=3) as 授課教師三,subject_alias_name as 科目簡稱,course.credit as 學分,course.period as 節數,(CASE WHEN course.c_required_by=1 THEN '部訂' WHEN course.c_required_by=2 THEN '校定' ELSE '' END) as 校部訂,(CASE WHEN course.c_is_required='1' THEN '必修' WHEN course.c_is_required='0' THEN '選修' ELSE '' END) as 必選修,(CASE WHEN course.not_included_in_credit='1' THEN '是' ELSE '' END) as 不計入學分,(CASE WHEN course.not_included_in_calc='1' THEN '是' ELSE '' END) as 不評分,(CASE WHEN score_calc_flag=1 THEN '不列入' ELSE '列入' END) as 學期成績");
                SQLBuilder.AppendLine(",$scheduler.timetable.name as 上課時間表,(CASE WHEN $scheduler.course_extension.allow_duplicate=true THEN '是' ELSE '' END) as 同天排課,(CASE WHEN $scheduler.course_extension.limit_next_day=true THEN '是' ELSE '' END) as 不連天排課,$scheduler.course_extension.split_spec as 分割設定,$scheduler.classroom.name as 預設場地條件,(CASE WHEN $scheduler.course_extension.week_flag=1 THEN '單' WHEN $scheduler.course_extension.week_flag=2 THEN '雙' ELSE '單雙' END) as 預設單雙週條件,(CASE WHEN $scheduler.course_extension.long_break=true THEN '是' ELSE '' END) as 預設跨中午條件,$scheduler.course_extension.weekday_condition as 預設星期條件,$scheduler.course_extension.period_condition as 預設節次條件 ");
                SQLBuilder.AppendLine("from course left outer join $scheduler.course_extension on $scheduler.course_extension.ref_course_id = course.id left outer join $scheduler.timetable on $scheduler.course_extension.ref_timetable_id= $scheduler.timetable.uid left outer join $scheduler.classroom on  $scheduler.course_extension.ref_classroom_id=$scheduler.classroom.uid left outer join class on class.id=course.ref_class_id ");
                SQLBuilder.AppendLine("where course.id in (" + string.Join(",", CourseIDs.ToArray()) + ") order by course.course_name,course.school_year,course.semester");

                //SQLBuilder.AppendLine("select course.course_name as 課程名稱,course.school_year as 學年度,course.semester as 學期,class.class_name as 班級名稱,course.domain as 領域,course.score_type as 分項類別,course.subject as 科目名稱,course.subj_level as 科目級別,(CASE WHEN teacher.nickname='' THEN teacher.teacher_name ELSE teacher.teacher_name || '(' || teacher.nickname || ')'  END) as 授課教師一,subject_alias_name as 科目簡稱,course.credit as 學分,course.period as 節數,(CASE WHEN course.c_required_by=1 THEN '部訂' WHEN course.c_required_by=2 THEN '校定' ELSE '' END) as 校部訂,(CASE WHEN course.c_is_required='1' THEN '必修' WHEN course.c_is_required='0' THEN '選修' ELSE '' END) as 必選修,(CASE WHEN course.not_included_in_credit='1' THEN '是' ELSE '否' END) as 不計入學分,(CASE WHEN course.not_included_in_calc='1' THEN '是' ELSE '否' END) as 不評分,$scheduler.timetable.name as 上課時間表,(CASE WHEN $scheduler.course_extension.allow_duplicate=true THEN '是' ELSE '' END) as 同天排課,(CASE WHEN $scheduler.course_extension.limit_next_day=true THEN '是' ELSE '' END) as 不連天排課,$scheduler.course_extension.split_spec as 分割設定,$scheduler.classroom.name as 預設場地條件,(CASE WHEN $scheduler.course_extension.week_flag=1 THEN '單' WHEN $scheduler.course_extension.week_flag=2 THEN '雙' ELSE '單雙' END) as 預設單雙週條件,(CASE WHEN $scheduler.course_extension.long_break=true THEN '是' ELSE '' END) as 預設跨中午條件,$scheduler.course_extension.weekday_condition as 預設星期條件,$scheduler.course_extension.period_condition as 預設節次條件 ");
                //SQLBuilder.AppendLine("from course left outer join $scheduler.course_extension on $scheduler.course_extension.ref_course_id = course.id left outer join $scheduler.timetable on $scheduler.course_extension.ref_timetable_id= $scheduler.timetable.uid left outer join $scheduler.classroom on  $scheduler.course_extension.ref_classroom_id=$scheduler.classroom.uid left outer join (select * from tc_instruct where tc_instruct.sequence=1)  as tc_instruct  on course.id = tc_instruct.ref_course_id left outer join teacher on teacher.id=tc_instruct.ref_teacher_id left outer join class on class.id=course.ref_class_id ");
                //SQLBuilder.AppendLine("where course.id in (" + string.Join(",", CourseIDs.ToArray()) + ") order by course.course_name,course.school_year,course.semester");

                QueryExport.Execute("課程排課資料(含課程)", SQLBuilder.ToString());
            }
        }

        /// <summary>
        /// 匯出選取課程排課資料
        /// </summary>
        /// <param name="CourseIDs">課程系統編號列表</param>
        public static void ExportCourseExtensionAndCourseByDomain(List<string> CourseIDs)
        {
            if (!K12.Data.Utility.Utility.IsNullOrEmpty(CourseIDs))
            {
                StringBuilder SQLBuilder = new StringBuilder();

                SQLBuilder.AppendLine("select course.course_name as 課程名稱,course.school_year as 學年度,course.semester as 學期,class.class_name as 班級名稱,course.domain as 領域,course.score_type as 分項類別,course.subject as 科目名稱,course.subj_level as 科目級別,(CASE WHEN teacher.nickname='' THEN teacher.teacher_name ELSE teacher.teacher_name || '(' || teacher.nickname || ')'  END) as 授課教師一,subject_alias_name as 科目簡稱,course.credit as 學分,course.period as 節數,(CASE WHEN course.c_required_by=1 THEN '部訂' WHEN course.c_required_by=2 THEN '校定' ELSE '' END) as 校部訂,(CASE WHEN course.c_is_required='1' THEN '必修' WHEN course.c_is_required='0' THEN '選修' ELSE '' END) as 必選修,(CASE WHEN course.not_included_in_credit='1' THEN '是' ELSE '否' END) as 不計入學分,(CASE WHEN course.not_included_in_calc='1' THEN '是' ELSE '否' END) as 不評分,$scheduler.timetable.name as 上課時間表,(CASE WHEN $scheduler.course_extension.allow_duplicate=true THEN '是' ELSE '' END) as 同天排課,(CASE WHEN $scheduler.course_extension.limit_next_day=true THEN '是' ELSE '' END) as 不連天排課,$scheduler.course_extension.split_spec as 分割設定,$scheduler.classroom.name as 預設場地條件,(CASE WHEN $scheduler.course_extension.week_flag=1 THEN '單' WHEN $scheduler.course_extension.week_flag=2 THEN '雙' ELSE '單雙' END) as 預設單雙週條件,(CASE WHEN $scheduler.course_extension.long_break=true THEN '是' ELSE '' END) as 預設跨中午條件,$scheduler.course_extension.weekday_condition as 預設星期條件,$scheduler.course_extension.period_condition as 預設節次條件 ");
                SQLBuilder.AppendLine("from course left outer join $scheduler.course_extension on $scheduler.course_extension.ref_course_id = course.id left outer join $scheduler.timetable on $scheduler.course_extension.ref_timetable_id= $scheduler.timetable.uid left outer join $scheduler.classroom on  $scheduler.course_extension.ref_classroom_id=$scheduler.classroom.uid left outer join (select * from tc_instruct where tc_instruct.sequence=1)  as tc_instruct  on course.id = tc_instruct.ref_course_id left outer join teacher on teacher.id=tc_instruct.ref_teacher_id left outer join class on class.id=course.ref_class_id ");
                SQLBuilder.AppendLine("where course.id in (" + string.Join(",", CourseIDs.ToArray()) + ") order by course.course_name,course.school_year,course.semester");

                //SQLBuilder.AppendLine("select course.course_name as 課程名稱,course.school_year as 學年度,course.semester as 學期,class.class_name as 班級名稱,course.domain as 領域,course.score_type as 分項類別,course.subject as 科目名稱,course.subj_level as 科目級別,(CASE WHEN teacher.nickname='' THEN teacher.teacher_name ELSE teacher.teacher_name || '(' || teacher.nickname || ')'  END) as 授課教師一,subject_alias_name as 科目簡稱,course.credit as 學分,course.period as 節數,(CASE WHEN course.c_required_by=1 THEN '部訂' WHEN course.c_required_by=2 THEN '校定' ELSE '' END) as 校部訂,(CASE WHEN course.c_is_required='1' THEN '必修' WHEN course.c_is_required='0' THEN '選修' ELSE '' END) as 必選修,(CASE WHEN course.not_included_in_credit='1' THEN '是' ELSE '否' END) as 不計入學分,(CASE WHEN course.not_included_in_calc='1' THEN '是' ELSE '否' END) as 不評分,$scheduler.timetable.name as 上課時間表,(CASE WHEN $scheduler.course_extension.allow_duplicate=true THEN '是' ELSE '' END) as 同天排課,(CASE WHEN $scheduler.course_extension.limit_next_day=true THEN '是' ELSE '' END) as 不連天排課,$scheduler.course_extension.split_spec as 分割設定,$scheduler.classroom.name as 預設場地條件,(CASE WHEN $scheduler.course_extension.week_flag=1 THEN '單' WHEN $scheduler.course_extension.week_flag=2 THEN '雙' ELSE '單雙' END) as 預設單雙週條件,(CASE WHEN $scheduler.course_extension.long_break=true THEN '是' ELSE '' END) as 預設跨中午條件,$scheduler.course_extension.weekday_condition as 預設星期條件,$scheduler.course_extension.period_condition as 預設節次條件 ");
                //SQLBuilder.AppendLine("from course left outer join $scheduler.course_extension on $scheduler.course_extension.ref_course_id = course.id left outer join $scheduler.timetable on $scheduler.course_extension.ref_timetable_id= $scheduler.timetable.uid left outer join $scheduler.classroom on  $scheduler.course_extension.ref_classroom_id=$scheduler.classroom.uid left outer join (select * from tc_instruct where tc_instruct.sequence=1)  as tc_instruct  on course.id = tc_instruct.ref_course_id left outer join teacher on teacher.id=tc_instruct.ref_teacher_id left outer join class on class.id=course.ref_class_id ");
                //SQLBuilder.AppendLine("where course.id in (" + string.Join(",", CourseIDs.ToArray()) + ") order by course.course_name,course.school_year,course.semester");

                QueryExport.Execute("課程排課資料(含課程分領域)", SQLBuilder.ToString(), "領域");
            }
        }


        /// <summary>
        /// 匯出選取課程分段資料
        /// </summary>
        /// <param name="CourseIDs">課程系統編號列表</param>
        public static void ExportCourseSection(List<string> CourseIDs)
        {
            if (!K12.Data.Utility.Utility.IsNullOrEmpty(CourseIDs))
            {
                StringBuilder SQLBuilder = new StringBuilder();

                SQLBuilder.AppendLine("select course.course_name as 課程名稱,course.school_year as 學年度,course.semester as 學期,$scheduler.course_section.weekday as 星期,$scheduler.course_section.period as 節次,$scheduler.course_section.length as 節數,(CASE WHEN $scheduler.course_section.long_break=true THEN '是' ELSE '' END) as 跨中午,(CASE WHEN $scheduler.course_section.week_flag=1 THEN '單' WHEN $scheduler.course_section.week_flag=2 THEN '雙' ELSE '單雙' END) as 單雙週,$scheduler.course_section.weekday_condition as 星期條件,$scheduler.course_section.period_condition as 節次條件,$scheduler.classroom.name as 場地名稱,(select (CASE WHEN teacher.nickname='' THEN teacher.teacher_name ELSE teacher.teacher_name || '(' || teacher.nickname || ')'  END) from teacher join tc_instruct on teacher.id=tc_instruct.ref_teacher_id where tc_instruct.ref_course_id=course.id and tc_instruct.sequence=1) as 授課教師一,(select (CASE WHEN teacher.nickname='' THEN teacher.teacher_name ELSE teacher.teacher_name || '(' || teacher.nickname || ')'  END) from teacher join tc_instruct on teacher.id=tc_instruct.ref_teacher_id where tc_instruct.ref_course_id=course.id and tc_instruct.sequence=2) as 授課教師二,(select (CASE WHEN teacher.nickname='' THEN teacher.teacher_name ELSE teacher.teacher_name || '(' || teacher.nickname || ')'  END) from teacher join tc_instruct on teacher.id=tc_instruct.ref_teacher_id where tc_instruct.ref_course_id=course.id and tc_instruct.sequence=3) as 授課教師三,$scheduler.course_section.comment as 註記 ");
                SQLBuilder.AppendLine("from course left outer join $scheduler.course_section on $scheduler.course_section.ref_course_id=course.id left outer join $scheduler.classroom on $scheduler.course_section.ref_classroom_id=$scheduler.classroom.uid ");
                SQLBuilder.AppendLine("where course.id in (" + string.Join(",", CourseIDs.ToArray()) + ") order by course.course_name,course.school_year,course.semester");

                QueryExport.Execute("課程分段", SQLBuilder.ToString());
            }
        }
    }
}