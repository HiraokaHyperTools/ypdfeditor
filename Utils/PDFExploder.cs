using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using yPDFEditor.Exceptions;

namespace yPDFEditor.Utils
{
    public class PDFExploder
    {
        String fp;

        public PDFExploder(String fp)
        {
            this.fp = fp;
        }

        internal static String pdftoppm_exe => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GPL", "pdftoppm.exe");

        public PagePic Extract(int page, int? scaleTo, int? dpi, bool png)
        {
            ProcessStartInfo psi = new ProcessStartInfo(pdftoppm_exe, ""
                + " -f " + page
                + " -l " + page
                + (png ? " -png" : " -jpeg") + " "
                + (scaleTo.HasValue ? " -scale-to " + scaleTo.Value : "")
                + (dpi.HasValue ? " -r " + dpi.Value : "")
                + " \"" + fp + "\""
                );
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            Process p = Process.Start(psi);
            MemoryStream os = new MemoryStream();
            Stream si = p.StandardOutput.BaseStream;
            byte[] bin = new byte[4000];
            while (true)
            {
                int r = si.Read(bin, 0, bin.Length);
                if (r < 1) break;
                os.Write(bin, 0, r);
            }
            p.WaitForExit();
            if (p.ExitCode != 0)
            {
                throw new PageExtractException(p.ExitCode);
            }
            return new PagePic(os);
        }
    }
}
