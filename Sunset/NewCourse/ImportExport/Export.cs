using System.Collections.Generic;
using System.Text;

namespace Sunset.NewCourse
{
    public static class Export
    {
        /// <summary>
        /// 匯出選取課程排課資料
        /// </summary>
        /// <param name="CourseIDs">課程系統編號列表</param>
        public static void ExportCourseExtension(List<string> CourseIDs)
        {
            if (!K12.Data.Utility.Utility.IsNullOrEmpty(CourseIDs))
            {
                StringBuilder SQLBuilder = new StringBuilder();
                SQLBuilder.Append("select course_name as 課程名稱,");
                SQLBuilder.Append("school_year as 學年度,semester as 學期,");
                SQLBuilder.Append("subject as 科目名稱,subject_alias_name as 科目簡稱,");
                SQLBuilder.Append("level as 科目級別,credit as 學分,period as 節數,");
                SQLBuilder.Append("class_name as 所屬班級,teacher_name_1 as 授課教師一,");
                SQLBuilder.Append("teacher_name_2 as 授課教師二,teacher_name_3 as 授課教師三,");
                SQLBuilder.Append("$scheduler.timetable.name as 上課時間表,");
                SQLBuilder.Append("(CASE WHEN allow_duplicate=true THEN '是' ELSE '' END) as 同天排課,");
                SQLBuilder.Append("(CASE WHEN limit_next_day=true THEN '是' ELSE '' END) as 隔天排課,");
                SQLBuilder.Append("split_spec as 分割設定,$scheduler.classroom.name as 場地條件,");
                SQLBuilder.Append("(CASE WHEN week_flag=1 THEN '單' WHEN week_flag=2 THEN '雙' ELSE '單雙' END) as 單雙週條件,");
                SQLBuilder.Append("(CASE WHEN long_break=true THEN '是' ELSE '' END) as 可跨中午,");
                SQLBuilder.Append("weekday_condition as 星期條件,period_condition as 節次條件,");
                //不開放查詢 , true為不開放
                SQLBuilder.Append("(CASE WHEN no_query=true THEN '不開放' ELSE '開放' END) as 開放查詢,");
                SQLBuilder.Append("domain as 領域,entry as 分項,required_by as 校部訂,required as 必選修,");
                //不計入學分設定 , true為不計入
                SQLBuilder.Append("(CASE WHEN not_included_in_credit=true THEN '不計入' ELSE '計入' END) as 學分設定,");
                //不評分設定 , true為不評分
                SQLBuilder.Append("(CASE WHEN not_included_in_calc=true THEN '不評分' ELSE '評分' END) as 評分設定,");
                //不列入設定 , 否為
                SQLBuilder.Append("(CASE WHEN calculation_flag='否' THEN '不列入' ELSE '列入' END) as 學期成績 ");
                //
                SQLBuilder.Append("from $scheduler.scheduler_course_extension ");
                SQLBuilder.Append("left outer join $scheduler.timetable on $scheduler.scheduler_course_extension.ref_timetable_id= $scheduler.timetable.uid ");
                SQLBuilder.Append("left outer join $scheduler.classroom on $scheduler.scheduler_course_extension.ref_classroom_id=$scheduler.classroom.uid ");
                SQLBuilder.Append("where $scheduler.scheduler_course_extension.uid in (" + string.Join(",", CourseIDs.ToArray()) + ") ");
                SQLBuilder.Append("order by course_name,school_year,semester");

                QueryExport.Execute("課程排課資料", SQLBuilder.ToString());
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

                SQLBuilder.AppendLine("select course_name as 課程名稱,school_year as 學年度,semester as 學期,$scheduler.scheduler_course_section.weekday as 星期,$scheduler.scheduler_course_section.period as 節次,$scheduler.scheduler_course_section.length as 節數,(CASE WHEN $scheduler.scheduler_course_section.long_break=true THEN '是' ELSE '' END) as 跨中午,(CASE WHEN $scheduler.scheduler_course_section.week_flag=1 THEN '單' WHEN $scheduler.scheduler_course_section.week_flag=2 THEN '雙' ELSE '單雙' END) as 單雙週,$scheduler.scheduler_course_section.weekday_condition as 星期條件,$scheduler.scheduler_course_section.period_condition as 節次條件,$scheduler.classroom.name as 場地名稱,$scheduler.scheduler_course_section.teacher_name_1 as 授課教師一,$scheduler.scheduler_course_section.teacher_name_2 as 授課教師二,$scheduler.scheduler_course_section.teacher_name_3 as 授課教師三,$scheduler.scheduler_course_section.comment as 註記 ");
                SQLBuilder.AppendLine("from $scheduler.scheduler_course_extension left outer join $scheduler.scheduler_course_section on $scheduler.scheduler_course_section.ref_course_id=$scheduler.scheduler_course_extension.uid left outer join $scheduler.classroom on $scheduler.scheduler_course_section.ref_classroom_id=$scheduler.classroom.uid ");
                SQLBuilder.AppendLine("where $scheduler.scheduler_course_extension.uid in (" + string.Join(",", CourseIDs.ToArray()) + ") order by $scheduler.scheduler_course_extension.course_name,$scheduler.scheduler_course_extension.school_year,$scheduler.scheduler_course_extension.semester");

                QueryExport.Execute("課程分段", SQLBuilder.ToString());
            }
        }
    }
}