using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace yPDFEditor.Exceptions
{
    public class PageExtractException : Exception
    {
        int errorCode;

        public PageExtractException(int errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
