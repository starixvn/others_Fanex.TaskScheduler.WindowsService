using System;
using System.Diagnostics;
using System.IO;

namespace Fanex.TaskScheduler.WindowsService
{
    public class ExecutableFileRunnerImp : ExecutableFileRunner
    {
        private readonly LogService logService;

        public ExecutableFileRunnerImp(LogService logService)
        {
            this.logService = logService;
        }

        public int Execute(string executableFile)
        {
            Process process = null;
            try
            {
                var startInfo = CreateProcessStartInfo(executableFile);
                process = Process.Start(startInfo);
                process.WaitForExit();
                return process.ExitCode;
            }
            catch (Exception exception)
            {
                logService.Log(exception);
            }
            finally
            {
                CloseNotNullProcess(process);
            }

            return int.MaxValue;
        }

        private ProcessStartInfo CreateProcessStartInfo(string executableFile)
        {
            return new ProcessStartInfo(executableFile)
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                ErrorDialog = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                WorkingDirectory = Path.GetDirectoryName(executableFile)
            };
        }

        private void CloseNotNullProcess(Process process)
        {
            process?.Close();
        }
    }
}
