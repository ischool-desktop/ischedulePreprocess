using System;
using System.Xml;

namespace Sunset.NewCourse
{
    public interface IGraduationPlanEditor
    {
        void SetSource(XmlElement source);
        XmlElement GetSource();
        bool IsDirty { get; }
        event EventHandler IsDirtyChanged;
        bool IsValidated
        {
            get;
        }
    }
}
