$TarFilename = "kafka_2.10-0.8.2.0.tgz"
$UnCompressedTarFilename = "kafka_2.10-0.8.2.0.tar"
$ExtractedFolder = "kafka_2.10-0.8.2.0"

$7ZipLocation = "C:\Program Files\7-Zip\7z.exe"

$AlreadyDownloaded = $False

cd $PSScriptRoot

if (Test-Path $ExtractedFolder) {
    $AlreadyDownloaded = $True
    WRITE-HOST "$ExtractedFolder already downloaded and extracted"
}

if (-not($AlreadyDownloaded)) {
    if (Test-Path $TarFilename) {
        Remove-Item $TarFilename -Force
    }

    if (Test-Path $UnCompressedTarFilename) {
        Remove-Item $UnCompressedTarFilename -Force
    }

    if (Test-Path $ExtractedFolder) {
        Remove-Item $ExtractedFolder -Force
    }

    curl http://apache.mirror.anlx.net/kafka/0.8.2.0/kafka_2.10-0.8.2.0.tgz  -OutFile $TarFilename

    & $7ZipLocation e $TarFilename
    & $7ZipLocation x $UnCompressedTarFilename 

    WRITE-HOST "Successfully downloaded and extracted $TarFilename"
}

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