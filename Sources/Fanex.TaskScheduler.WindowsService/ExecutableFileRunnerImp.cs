using System;
using System.Diagnostics;
using System.IO;
using Fanex.MailDelivery.Client.Messages;
using Newtonsoft.Json;

namespace Fanex.TaskScheduler.WindowsService
{
    public class ExecutableFileRunnerImp : ExecutableFileRunner
    {
        private readonly LogService logService;

        public ExecutableFileRunnerImp(LogService logService)
        {
            this.logService = logService;
        }

        public JobResultMessage Execute(string executableFile)
        {
            Process process = null;
            JobResultMessage job = new JobResultMessage()
            {
                JobStatus = MailDelivery.Client.Enumerations.JobStatus.Success
            };            
                
            try
            {
                var startInfo = CreateProcessStartInfo(executableFile);
                process = Process.Start(startInfo);
                job = DeserializeMailObject(process);
                process.WaitForExit(1);                
            }
            catch (Exception exception)
            {
                logService.Log(exception);
            }
            finally
            {
                CloseNotNullProcess(process);
            }

            return job;
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

        private JobResultMessage DeserializeMailObject(Process process)
        {
            var output = process.StandardOutput.ReadToEnd();            
            return JsonConvert.DeserializeObject<JobResultMessage>(output);
        }

        private void CloseNotNullProcess(Process process)
        {
            process?.Close();
        }
    }
}