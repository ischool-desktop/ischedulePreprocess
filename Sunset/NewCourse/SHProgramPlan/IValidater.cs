using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset.NewCourse
{
    public interface IValidater<T>
    {
        List<IValidater<T>> ExtendValidater { get; }

        bool Validate(T info, IErrorViewer responseViewer);
    }
}