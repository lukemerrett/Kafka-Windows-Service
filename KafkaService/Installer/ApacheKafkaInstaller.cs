using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaService.Installer
{
    /// <summary>
    /// Responsible for downloading and setting up Apache Kafka in the ServiceRunners directory of the project.
    /// </summary>
    public static class ApacheKafkaInstaller
    {
        /// <summary>
        /// Downloads and sets up Apache Kafka locally in the ServiceRunners directory of the project.
        /// This ensures both Kafka and Zookeeper are configured to work within a Windows environment.
        /// </summary>
        public static void Install()
        {
            using (var installKafkaProcess = Process.Start("powershell.exe", "-file ServiceRunners\\01_Install_Apache_Kafka.ps1"))
            {
                if (installKafkaProcess != null)
                {
                    installKafkaProcess.WaitForExit();
                }
            }
        }
    }
}
