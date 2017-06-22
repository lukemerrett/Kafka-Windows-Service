using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KafkaService.Installer
{
    /// <summary>
    /// Responsible for downloading and setting up Apache Kafka in the root directory of the program.
    /// </summary>
    public static class ApacheKafkaInstaller
    {
        private const string SEVENZIP_LOCATION = @"Lib\7z.exe";

        /// <summary>
        /// Downloads and sets up Apache Kafka locally in the root directory of the program.
        /// This ensures both Kafka and Zookeeper are configured to work within a Windows environment.
        /// </summary>
        public static void Install()
        {
            DownloadAndExtractKafka();
            SetupWindowsEnvironment();
        }

        private static void DownloadAndExtractKafka()
        {
            if (Directory.Exists(Constants.KafkaLocation))
            {
                Console.WriteLine("{0} already downloaded and extracted", Constants.KafkaLocation);
                return;
            }

            // Delete the extracted file so we can unzip it again.
            DeleteFileIfExists(Constants.UncompressedTarFileName);

            if (!File.Exists(Constants.TarFileName))
            {
                DownloadKafka();
            }

            ExtractKafka();
            MoveKafka();
            DeleteFileIfExists(Constants.UncompressedTarFileName);
            DeleteFileIfExists(Constants.TarFileName);
        }

        private static void DeleteFileIfExists(string filename)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        private static void DownloadKafka()
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(Constants.DownloadUrl, Constants.TarFileName);
            }

            Console.WriteLine("Downloaded {0}", Constants.TarFileName);
        }

        private static void ExtractKafka()
        {
            Process.Start(SEVENZIP_LOCATION, "e " + Constants.TarFileName).WaitForExit();
            Process.Start(SEVENZIP_LOCATION, "x " + Constants.UncompressedTarFileName).WaitForExit();

            Console.WriteLine("Extracted {0} into {1}", Constants.TarFileName, Constants.KafkaScalaVersion);
        }

        private static void MoveKafka()
        {
            string sourceDirectory = Constants.KafkaScalaVersion;
            string destinationDirectory = Constants.KafkaLocation;

            Directory.Move(sourceDirectory, destinationDirectory);
        }

        private static void SetupWindowsEnvironment()
        {
            var dataDir = string.Format("{0}/data", Constants.KafkaScalaVersion);
            var loggingDir = string.Format("{0}/kafka-logs", Constants.KafkaScalaVersion);

            CreateDirectoryIfNotExists(Constants.DataDir);
            CreateDirectoryIfNotExists(Constants.LogDir);

            ReplaceLineInFile(Constants.ZookeeperConfig, "dataDir=/tmp/zookeeper", "dataDir=" + dataDir);
            ReplaceLineInFile(Constants.KafkaConfig, "log.dirs=/tmp/kafka-logs", "log.dirs=" + loggingDir);
        }

        private static void CreateDirectoryIfNotExists(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        private static void ReplaceLineInFile(string filename, string lineToMatch, string replaceWith)
        {
            var contents = File.ReadAllLines(filename);

            var fileToOutput = new List<string>();

            foreach(var line in contents)
            {
                var outputLine = line;

                if (line == lineToMatch)
                {
                    outputLine = replaceWith;
                }

                fileToOutput.Add(outputLine);
            }

            File.WriteAllLines(filename, fileToOutput);
        }
    }
}
