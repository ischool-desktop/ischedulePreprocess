using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus.DocumentValidator;

namespace Sunset
{
    public class TimeFormatCheck : IFieldValidator
    {
        #region IFieldValidator 成員

        private string mMessage = string.Empty;

        public string Correct(string Value)
        {
            throw new NotImplementedException();
        }

        public string ToString(string template)
        {
            return mMessage;
        }

        public bool Validate(string Value)
        {
            Tuple<bool, string> Result = Utility.IsValidateTime(Value);

            mMessage = Result.Item2;

            return Result.Item1;
        }

        #endregion
    }
}