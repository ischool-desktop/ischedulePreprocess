using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;

namespace Sunset.NewCourse
{
    public class ValidateGraduationPlanInfo : IValidater<SchedulerProgramPlan>
    {
        private List<IValidater<SchedulerProgramPlan>> _ExtendValidater = new List<IValidater<SchedulerProgramPlan>>();

        private AutoResetEvent _OneTimeOneCheck = new AutoResetEvent(true);

        private GraduationPlanEditor _Editor;

        static private List<SchedulerProgramPlan> _PassedList = new List<SchedulerProgramPlan>();


        public ValidateGraduationPlanInfo(params IValidater<SchedulerProgramPlan>[] extendValidaters)
        {
            _Editor = new GraduationPlanEditor();
            _Editor.SuspendLayout();
            ExtendValidater.AddRange( extendValidaters);
        }

        #region IValidater<GraduationPlanInfo> 成員

        public bool Validate(SchedulerProgramPlan info, IErrorViewer responseViewer)
        {
            _OneTimeOneCheck.WaitOne();
            bool pass = true;
            try
            {
                if ( !_PassedList.Contains(info) )
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(info.Content);

                    XmlElement GraduationPlanElement = xmldoc.DocumentElement;

                    _Editor.SetSource(xmldoc.DocumentElement);
                    pass &= _Editor.IsValidated;
                    pass &= _Editor.GetSource().SelectNodes("Subject").Count == GraduationPlanElement.SelectNodes("Subject").Count;
                    if ( pass )
                    {
                        foreach ( XmlNode var in _Editor.GetSource().SelectNodes("Subject") )
                        {
                            XmlElement subject1 = (XmlElement)var;
                            XmlElement subject2 = (XmlElement)GraduationPlanElement.SelectSingleNode("Subject[@SubjectName='" + subject1.GetAttribute("SubjectName") + "' and @Level='" + subject1.GetAttribute("Level") + "']");
                            if ( subject2 != null )
                            {
                                foreach ( XmlAttribute attributeInfo in subject1.Attributes )
                                {
                                    if ( subject1.GetAttribute(attributeInfo.Name) != subject2.GetAttribute(attributeInfo.Name) )
                                    {
                                        pass = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                pass = false;
                                break;
                            }
                        }
                    }
                    if ( pass )
                        _PassedList.Add(info);
                }
                if ( pass )
                {
                    foreach ( IValidater<SchedulerProgramPlan> extendValidater in _ExtendValidater )
                    {
                        pass &= extendValidater.Validate(info, responseViewer);
                    }
                }
                else
                {
                    if ( responseViewer != null )
                        responseViewer.SetMessage("課程規畫表：\"" + info.Name + "\"驗證失敗");
                    pass = false;
                }
                _OneTimeOneCheck.Set();
            }
            catch ( Exception ex )
            {
                if ( responseViewer != null )
                    responseViewer.SetMessage("課程規畫表：\"" + info.Name + "\"在驗證過程中發生未預期錯誤");
                SmartSchool.ErrorReporting.ReportingService.ReportException(ex);
                _OneTimeOneCheck.Set();
                return false;
            }
            return pass;
        }

        public List<IValidater<SchedulerProgramPlan>> ExtendValidater
        {
            get { return _ExtendValidater; }
        }

        #endregion
    }
}