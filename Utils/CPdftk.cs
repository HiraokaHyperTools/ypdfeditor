using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace yPDFEditor.Utils
{
    class CPdftk
    {
        private UtRedir sto;
        private UtRedir ste;
        private Process p;
        private ProcessStartInfo psi;

        internal static string pdftk_exe => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GPL", "pdftk.exe");

        internal bool Cat(String fpTmp2, params String[] alfpIn)
        {
            String a = "";
            foreach (String fpIn in alfpIn)
            {
                a += " " + (PVUt.PGExists(fpIn) ? WinNUt.Quotes(fpIn) : "");
            }
            a += " cat output " + WinNUt.Quotes(fpTmp2);

            psi = new ProcessStartInfo(pdftk_exe, a);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            p = Process.Start(psi);
            sto = new UtRedir(p.StandardOutput);
            ste = new UtRedir(p.StandardError);
            p.WaitForExit();
            sto.Wait();
            ste.Wait();

            return p.ExitCode == 0;
        }

        internal bool Cat2(String fpTmp2, String cat, String fpTmppdf)
        {
            String a = " " + WinNUt.Quotes(fpTmppdf) + " cat " + cat + " output " + WinNUt.Quotes(fpTmp2) + "";

            psi = new ProcessStartInfo(pdftk_exe, a);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            p = Process.Start(psi);
            sto = new UtRedir(p.StandardOutput);
            ste = new UtRedir(p.StandardError);
            p.WaitForExit();
            sto.Wait();
            ste.Wait();

            return p.ExitCode == 0;
        }

        internal bool Cat3(String fpTmp2, String cat, IDictionary<String, String> inppdf)
        {
            String a = "";
            foreach (KeyValuePair<String, String> kv in inppdf)
            {
                a += " " + kv.Key + "=" + WinNUt.Quotes(kv.Value);
            }
            a += " cat " + cat + " output " + WinNUt.Quotes(fpTmp2) + "";

            psi = new ProcessStartInfo(pdftk_exe, a);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            p = Process.Start(psi);
            sto = new UtRedir(p.StandardOutput);
            ste = new UtRedir(p.StandardError);
            p.WaitForExit();
            sto.Wait();
            ste.Wait();

            return p.ExitCode == 0;
        }
    }
}
