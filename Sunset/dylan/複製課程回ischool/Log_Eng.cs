using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using K12.Data;

namespace Sunset.NewCourse
{
    public class Log_Eng
    {
        List<CourseRecord> _NewK12Courses { get; set; }
        List<TCInstructRecord> _TCInstructs { get; set; }
        List<SCAttendRecord> _SCAttends { get; set; }

        Dictionary<string, TeacherRecord> _TeacherDic { get; set; }
        Dictionary<string, CourseRecord> _K12CourseDic { get; set; }


        Dictionary<string, StudObj> _StudentDic { get; set; }

        /// <summary>
        /// 課程ID : 修課記錄
        /// </summary>
        Dictionary<string, List<SCAttendRecord>> _SCAttendDic { get; set; }

        /// <summary>
        /// 設定新增課程的Log清單
        /// </summary>
        public void SetCourseLog(List<CourseRecord> NewK12Courses)
        {
            _NewK12Courses = NewK12Courses;

            _K12CourseDic = new Dictionary<string, CourseRecord>();
            foreach (CourseRecord each in _NewK12Courses)
            {
                if (!_K12CourseDic.ContainsKey(each.ID))
                {
                    _K12CourseDic.Add(each.ID, each);
                }
            }
        }

        /// <summary>
        /// 設定授課教師Log清單
        /// </summary>
        public void SetTeacherLog(List<TeacherRecord> TeacherRecords, List<TCInstructRecord> TCInstructs)
        {
            _TeacherDic = new Dictionary<string, TeacherRecord>();

            if (TCInstructs != null)
            {
                _TCInstructs = TCInstructs;
                //排序授課記錄,讓相關的老師能夠同時顯示
                _TCInstructs.Sort(SortTCInstruct);

                foreach (TeacherRecord each in TeacherRecords)
                {
                    if (!_TeacherDic.ContainsKey(each.ID))
                    {
                        _TeacherDic.Add(each.ID, each);
                    }
                }
            }
            else
                _TCInstructs = new List<TCInstructRecord>();
        }

        /// <summary>
        /// 設定學生修課Log清單
        /// </summary>
        public void SetSCAttendLog(List<SCAttendRecord> SCAttends, List<StudObj> Students)
        {
            _SCAttends = SCAttends;
            _SCAttends.Sort(SortSCAttends);
            _StudentDic = new Dictionary<string, StudObj>();
            _SCAttendDic = new Dictionary<string, List<SCAttendRecord>>();

            foreach (StudObj each in Students)
            {
                if (!_StudentDic.ContainsKey(each.StudentID))
                {
                    _StudentDic.Add(each.StudentID, each);
                }
            }

            foreach (SCAttendRecord each in _SCAttends)
            {
                if (!_SCAttendDic.ContainsKey(each.RefCourseID))
                {
                    _SCAttendDic.Add(each.RefCourseID, new List<SCAttendRecord>());
                }
                _SCAttendDic[each.RefCourseID].Add(each);
            }
        }


        /// <summary>
        /// 取得課程Log
        /// </summary>
        public string GetCourseLog()
        {
            StringBuilder Log_Course = new StringBuilder();
            Log_Course.AppendLine("複製「ischool課程」明細：");
            foreach (CourseRecord each in _NewK12Courses)
            {
                string school = each.SchoolYear.HasValue ? each.SchoolYear.Value.ToString() : "";
                string semester = each.Semester.HasValue ? each.Semester.Value.ToString() : "";
                Log_Course.AppendLine(string.Format("學年度「{0}」學期「{1}」課程名稱「{2}」", school, semester, each.Name));
            }

            return Log_Course.ToString();
        }

        /// <summary>
        /// 取得教師Log
        /// </summary>
        public string GetTeacherLog()
        {
            StringBuilder Log_Teacher = new StringBuilder();
            Log_Teacher.AppendLine("新增教師「授課課程」明細：");
            if (_TCInstructs != null)
            {
                foreach (TCInstructRecord each in _TCInstructs)
                {
                    if (_TeacherDic.ContainsKey(each.RefTeacherID) && _K12CourseDic.ContainsKey(each.RefCourseID))
                    {
                        TeacherRecord tr = _TeacherDic[each.RefTeacherID];
                        string TeacherName = string.IsNullOrEmpty(tr.Nickname) ? tr.Name : tr.Name;

                        CourseRecord cr = _K12CourseDic[each.RefCourseID];
                        string school = cr.SchoolYear.HasValue ? cr.SchoolYear.Value.ToString() : "";
                        string semester = cr.Semester.HasValue ? cr.Semester.Value.ToString() : "";
                        string CourseName = string.Format("學年度:{0}　學期:{1}　課程名稱:{2}", school, semester, cr.Name);

                        Log_Teacher.AppendLine(string.Format("教師「{0}」新增授課「{1}」", TeacherName, CourseName));

                    }
                }
            }

            return Log_Teacher.ToString();
        }

        public void GetSCAttendLog()
        {
            FISCA.LogAgent.LogSaver ls = FISCA.LogAgent.ApplicationLog.CreateLogSaverInstance();

            foreach (string courseID in _SCAttendDic.Keys)
            {
                StringBuilder Log_SCAttend = new StringBuilder();
                Log_SCAttend.AppendLine("課程加入「修課學生」明細：");

                if (_K12CourseDic.ContainsKey(courseID))
                {
                    CourseRecord cr = _K12CourseDic[courseID];
                    string school = cr.SchoolYear.HasValue ? cr.SchoolYear.Value.ToString() : "";
                    string semester = cr.Semester.HasValue ? cr.Semester.Value.ToString() : "";
                    string CourseName = string.Format("學年度「{0}」學期「{1}」課程名稱「{2}」", school, semester, cr.Name);

                    Log_SCAttend.AppendLine(CourseName + "\n新增修課學生清單：");

                    foreach (SCAttendRecord each in _SCAttendDic[courseID])
                    {
                        if (_StudentDic.ContainsKey(each.RefStudentID))
                        {
                            StudObj sr = _StudentDic[each.RefStudentID];
                            Log_SCAttend.AppendLine(string.Format("班級「{0}」座號「{1}」學號「{2}」姓名「{3}」", sr.班級, sr.座號, sr.學號, sr.姓名));
                        }
                    }
                }

                ls.Log("排課", "建立修課記錄", Log_SCAttend.ToString());
            }
            ls.LogBatch();
        }

        private int SortSCAttends(SCAttendRecord t1, SCAttendRecord t2)
        {
            return t1.RefCourseID.CompareTo(t2.RefCourseID);
        }

        private int SortTCInstruct(TCInstructRecord t1, TCInstructRecord t2)
        {
            return t1.RefTeacherID.CompareTo(t2.RefTeacherID);
        }

    }
}
