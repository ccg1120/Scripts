using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morph.AdbSharp
{
    public class Adb: IAndroidDebugBridge
    {
        public void Install(string path)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "adb";
            startInfo.Arguments = "install " + path;
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;

            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardError = true;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            process.StandardInput.Close();

            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Close();
        }
    }
}
