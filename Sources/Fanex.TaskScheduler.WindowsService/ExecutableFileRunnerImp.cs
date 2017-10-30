using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Fanex.MailDelivery.Core.Models;
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

        public JobMessage Execute(string executableFile)
        {
            Process process = null;
            JobMessage job;

            var startInfo = CreateProcessStartInfo(executableFile);
            process = Process.Start(startInfo);
            job = DeserializeMailObject(process, executableFile);
            process.WaitForExit(1);

            if (job.JobStatus == JobStatus.DataIsNotReady && string.IsNullOrEmpty(job.TaskLog.Message))
            {
                job.JobStatus = JobStatus.Running;
                throw new Exception("DataIsNotReady");
            }
            if (job.JobStatus == JobStatus.DataIsNotReady && !string.IsNullOrEmpty(job.TaskLog.Message))
            {
                job.JobStatus = JobStatus.Running;
                throw new Exception(job.TaskLog.Message);
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

        private List<string> Applications => new List<string>
        {
            "Fanex.SMAMonthlyReport.Executing.exe"
        };

        private JobMessage DeserializeMailObject(Process process, string executableFile)
        {
            JobMessage jobMessage;
            var output = string.Empty;
            try
            {
                output = process.StandardOutput.ReadToEnd();
                int indexOfSchedule = FindIndexOf(output);
                if (indexOfSchedule < 0)
                {
                    return JsonConvert.DeserializeObject<JobMessage>(output);
                }

                string jobResult = output.Substring(indexOfSchedule, output.Length - indexOfSchedule);
                return JsonConvert.DeserializeObject<JobMessage>(jobResult);
            }
            catch (Exception exception)
            {
                logService.Log(exception);
                Logging.LogService.Info(output);
                var isSpecialApp = Applications.FirstOrDefault(s => executableFile.Contains(s));
                jobMessage = new JobMessage
                {
                    JobStatus = string.IsNullOrEmpty(isSpecialApp) ? JobStatus.JobFailed : JobStatus.SuccessWithoutSendmail,
                    TaskLog = new TaskLog { Message = output }
                };
            }

            return jobMessage;
        }

        private static int FindIndexOf(string output)
        {
            int indexOfSchedule = output.IndexOf("{", StringComparison.Ordinal);
            if (indexOfSchedule < 0)
            {
                indexOfSchedule = output.IndexOf("{\"Schedule\":", StringComparison.Ordinal);
            }

            if (indexOfSchedule < 0)
            {
                indexOfSchedule = output.IndexOf("{\r\n  \"Schedule\":", StringComparison.Ordinal);
            }

            return indexOfSchedule;
        }

        private void CloseNotNullProcess(Process process)
        {
            process?.Close();
        }
    }
}