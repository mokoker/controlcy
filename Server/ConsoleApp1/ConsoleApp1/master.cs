using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class SortOutputRedirection
    {
        // Define static variables shared by class methods.
        private static StringBuilder sortOutput = null;
        private static int numOutputLines = 0;

        public static void SortInputListText()
        {
            // Initialize the process and its StartInfo properties.
            // The sort command is a console application that
            // reads and sorts text input.
            var ip = "176.41.4.0/24";
            Process sortProcess = new Process();
            sortProcess.StartInfo.FileName = "nmap";
            sortProcess.StartInfo.Arguments = String.Format("{0} -F -v", ip);
            sortOutput = new StringBuilder();
            // Set UseShellExecute to false for redirection.
            sortProcess.StartInfo.UseShellExecute = false;

            // Redirect the standard output of the sort command.
            // This stream is read asynchronously using an event handler.
            sortProcess.StartInfo.RedirectStandardOutput = true;

            // Set our event handler to asynchronously read the sort output.
            sortProcess.OutputDataReceived += SortOutputHandler;

            // Start the process.
            sortProcess.Start();

            // Use a stream writer to synchronously write the sort input.
            // Start the asynchronous read of the sort output stream.
            sortProcess.BeginOutputReadLine();

         

            // Wait for the sort process to write the sorted text lines.
            sortProcess.WaitForExit();
            Console.WriteLine(sortOutput.ToString());
            sortProcess.Close();
        }

        private static void SortOutputHandler(object sendingProcess,
            DataReceivedEventArgs outLine)
        {
            // Collect the sort command output.
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                numOutputLines++;

                // Add the text to the collected output.
                sortOutput.Append(Environment.NewLine +
                    $"[{numOutputLines}] - {outLine.Data}");
            }
        }
    }
}
