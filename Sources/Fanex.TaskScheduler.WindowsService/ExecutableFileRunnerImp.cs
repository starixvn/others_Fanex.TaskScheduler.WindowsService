using System;
using System.Diagnostics;
using System.IO;
using Fanex.MailDelivery.Client.Enumerations;
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
                JobStatus = JobStatus.Success
            };

            var startInfo = CreateProcessStartInfo(executableFile);
            process = Process.Start(startInfo);
            job = DeserializeMailObject(process);
            process.WaitForExit(1);

            if (job.JobStatus == JobStatus.DataIsNotReady)
            {
                job.JobStatus = JobStatus.Running;
                throw new Exception("DataIsNotReady");
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
            int indexOfSchedule = output.IndexOf("{\"Schedule\":", StringComparison.Ordinal);
            if (indexOfSchedule < 0)
            {
                return JsonConvert.DeserializeObject<JobResultMessage>(output);
            }

            string jobResult = output.Substring(indexOfSchedule, output.Length - indexOfSchedule);
            return JsonConvert.DeserializeObject<JobResultMessage>(jobResult);
        }

        private void CloseNotNullProcess(Process process)
        {
            process?.Close();
        }
    }
}