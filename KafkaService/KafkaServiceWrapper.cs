#region

using System;
using System.Diagnostics;
using System.Management;

#endregion

namespace KafkaService
{
    public class KafkaServiceWrapper
    {
        private Process _zookeeperProcess;

        private Process _kafkaProcess;

        public void Start()
        {
            using (var installKafkaProcess = Process.Start("powershell.exe", "-file ServiceRunners\\01_Install_Apache_Kafka.ps1"))
            {
                if (installKafkaProcess != null)
                {
                    installKafkaProcess.WaitForExit();
                }
            }

            _zookeeperProcess = Process.Start("powershell.exe", "-file ServiceRunners\\02_StartZookeeper.ps1");
            _kafkaProcess = Process.Start("powershell.exe", "-file ServiceRunners\\03_StartKafka.ps1");
        }

        public void Stop()
        {
            KillProcessAndChildren(_kafkaProcess.Id);
            KillProcessAndChildren(_zookeeperProcess.Id);
        }

        private static void KillProcessAndChildren(int pid)
        {
            using (var searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid))
            {
                var managementObjects = searcher.Get();

                foreach (var obj in managementObjects)
                {
                    var managementObject = (ManagementObject) obj;
                    KillProcessAndChildren(Convert.ToInt32(managementObject["ProcessID"]));
                }

                try
                {
                    var proc = Process.GetProcessById(pid);
                    proc.Kill();
                }
                catch (ArgumentException)
                {
                    // Process already exited.
                }
            }
        }
    }
}