using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace yPDFEditor {
    static class Program {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(String[] args) {
            System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(
                new System.Runtime.Remoting.Channels.Ipc.IpcChannel("yPDFEditor_IPC_" + Guid.NewGuid().ToString("N")),
                false
                );

            String fp = null;
            foreach (String a in args) {
                if (fp == null && File.Exists(a)) {
                    fp = a;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new JForm(fp));
            //Application.Run(new FR3Form());
        }
    }
}