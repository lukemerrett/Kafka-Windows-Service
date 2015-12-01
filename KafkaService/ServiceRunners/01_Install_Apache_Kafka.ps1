$TarFilename = "kafka_2.10-0.8.2.0.tgz"
$UnCompressedTarFilename = "kafka_2.10-0.8.2.0.tar"
$ExtractedFolder = "kafka_2.10-0.8.2.0"

$7ZipLocation = "C:\Program Files\7-Zip\7z.exe"

$DataDir = "$ExtractedFolder/data"
$ZookeeperConfig = "$ExtractedFolder\config\zookeeper.properties"
$LineToReplace = "^dataDir=/tmp/zookeeper$"
$ReplaceWith = "dataDir=data"

if (-not(Test-Path $DataDir)) {
    mkdir $DataDir
}

$configContent = Get-Content $ZookeeperConfig

if ($configContent -match $LineToReplace) {
    WRITE-HOST "Fixing the Zookeeper Config for a Windows environment"
    $configContent -replace $LineToReplace , $ReplaceWith | Set-Content $ZookeeperConfig
}
else {
    WRITE-HOST "Zookeeper Config already setup for a Windows environment"
}

$LogDir = "$ExtractedFolder/kafka-logs"
$KafkaConfig = "$ExtractedFolder\config\server.properties"
$LineToReplace = "^log.dirs=/tmp/kafka-logs$"
$ReplaceWith = "log.dirs=kafka-logs"

if (-not(Test-Path $LogDir)) {
    mkdir $LogDir
}

$configContent = Get-Content $KafkaConfig

if ($configContent -match $LineToReplace) {
    WRITE-HOST "Fixing the Kafka Config for a Windows environment"
    $configContent -replace $LineToReplace , $ReplaceWith | Set-Content $KafkaConfig
}
else {
    WRITE-HOST "Kafka Config already setup for a Windows environment"
}