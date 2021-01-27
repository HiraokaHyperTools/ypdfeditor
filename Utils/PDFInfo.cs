using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace yPDFEditor.Utils
{
    class PDFInfo
    {
        string rows = String.Empty;
        string fp;

        private static string pdfinfo_exe => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GPL", "pdfinfo.exe");

        public PDFInfo(String fp)
        {
            this.fp = fp;

        }

        public bool Read()
        {
            ProcessStartInfo psi = new ProcessStartInfo(pdfinfo_exe, " " + WinNUt.Quotes(fp));
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;
            Process p = Process.Start(psi);
            rows = p.StandardOutput.ReadToEnd();
            String errs = p.StandardError.ReadToEnd();
            p.WaitForExit();
            return p.ExitCode == 0;
        }

        public int PageCount
        {
            get
            {
                var match = Regex.Match(rows, "^Pages:\\s+(?<a>\\d+)", RegexOptions.Multiline);
                if (match.Success)
                {
                    return int.Parse(match.Groups["a"].Value);
                }
                return 0;
            }
        }

    }
}
