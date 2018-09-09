namespace KafkaService
{
    public static class Constants
    {
        public static string KafkaVersion
        {
            get { return "0.10.2.1"; }
        }

        public static string KafkaScalaVersion
        {
            get { return "kafka_2.12-" + KafkaVersion; }
        }

        public static string DownloadUrl
        {
            get { return "http://apache.claz.org/kafka/" + KafkaVersion + "/" + KafkaScalaVersion + ".tgz"; }
        }

        public static string TarFileName
        {
            get { return KafkaScalaVersion + ".tgz"; }
        }

        public static string UncompressedTarFileName
        {
            get { return KafkaScalaVersion + ".tar"; }
        }

        public static string RootDirectory
        {
            get { return @"C:\";  }
        }

        public static string RootDirectoryLinux
        {
            get { return @"C:/"; }
        }

        public static string KafkaLocation
        {
            get { return RootDirectory + KafkaScalaVersion;  }
        }

        public static string ZookeeperProcess
        {
            get
            {
                return RootDirectory +
                    KafkaScalaVersion + @"\bin\windows\zookeeper-server-start.bat " + KafkaLocation + @"\config\zookeeper.properties";
            }
        }

        public static string KafkaProcess
        {
            get
            {
                return RootDirectory +
                    KafkaScalaVersion + @"\bin\windows\kafka-server-start.bat " + KafkaLocation + @"\config\server.properties";
            }
        }

        public static string ZookeeperConfig
        {
            get { return RootDirectory + @"\" + KafkaScalaVersion + @"\config\zookeeper.properties"; }
        }

        public static string KafkaConfig
        {
            get { return RootDirectory + @"\" + KafkaScalaVersion + @"\config\server.properties"; }
        }

        public static string DataDir
        {
            get { return RootDirectory + KafkaScalaVersion + @"\data"; }
        }

        public static string LogDir
        {
            get { return RootDirectory + KafkaScalaVersion + @"\kafka-logs"; }
        }

        public static string DataDirLinux
        {
            get { return RootDirectoryLinux + KafkaScalaVersion + @"/data"; }
        }

        public static string LogDirLinux
        {
            get { return RootDirectoryLinux + KafkaScalaVersion + @"/kafka-logs"; }
        }
    }
}