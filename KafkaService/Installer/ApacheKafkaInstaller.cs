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
        private const string KAFKA_VERSION = "kafka_2.10-0.8.2.0";
        private const string TAR_FILENAME = KAFKA_VERSION + ".tgz";
        private const string UNCOMPRESSED_TAR_FILENAME = KAFKA_VERSION + ".tar";

        private const string DOWNLOAD_URL = @"http://apache.mirror.anlx.net/kafka/0.8.2.0/kafka_2.10-0.8.2.0.tgz";

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
            if (Directory.Exists(KAFKA_VERSION))
            {
                Console.WriteLine("{0} already downloaded and extracted", KAFKA_VERSION);
                return;
            }

            // Delete the extracted file so we can unzip it again.
            DeleteFileIfExists(UNCOMPRESSED_TAR_FILENAME);

            if (!File.Exists(TAR_FILENAME))
            {
                DownloadKafka();
            }

            ExtractKafka();
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
                client.DownloadFile(DOWNLOAD_URL, TAR_FILENAME);
            }

            Console.WriteLine("Downloaded {0}", TAR_FILENAME);
        }

        private static void ExtractKafka()
        {
            Process.Start(SEVENZIP_LOCATION, "e " + TAR_FILENAME).WaitForExit();
            Process.Start(SEVENZIP_LOCATION, "x " + UNCOMPRESSED_TAR_FILENAME).WaitForExit();

            Console.WriteLine("Extracted {0} into {1}", TAR_FILENAME, KAFKA_VERSION);
        }

        private static void SetupWindowsEnvironment()
        {
            var dataDir = string.Format("{0}\\data", KAFKA_VERSION);
            var loggingDir = string.Format("{0}\\kafka-logs", KAFKA_VERSION);

            var zookeeperConfig = string.Format("{0}\\config\\zookeeper.properties", KAFKA_VERSION);
            var kafkaConfig = string.Format("{0}\\config\\server.properties", KAFKA_VERSION);

            CreateDirectoryIfNotExists(dataDir);
            CreateDirectoryIfNotExists(loggingDir);

            ReplaceLineInFile(zookeeperConfig, "dataDir=/tmp/zookeeper", "dataDir=data");
            ReplaceLineInFile(kafkaConfig, "log.dirs=/tmp/kafka-logs", "log.dirs=kafka-logs");
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
