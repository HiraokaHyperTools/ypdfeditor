using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace yPDFEditor.Utils
{
    class UtRedir
    {
        Thread thread;
        String text;

        public UtRedir(TextReader reader)
        {
            (thread = new Thread(_ =>
            {
                text = reader.ReadToEnd();
            })).Start();
        }

        public string Wait()
        {
            thread.Join();
            return text;
        }
    }
}
