using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace yPDFEditor.Utils
{
    public class UtMarkTemp
    {
        private static List<string> files = new List<string>();
        private static object locker = new object();

        public static void Add(string fp)
        {
            lock (locker)
            {
                files.Add(fp);
            }
        }

        public static void Cleanup()
        {
            lock (locker)
            {
                foreach (var file in files)
                {
                    try
                    {
                        if (File.Exists(file))
                        {
                            File.Delete(file);
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                }
                files.Clear();
            }
        }
    }
}
