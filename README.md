# Kafka-Windows-Service

C# Wrapper to host [Apache Kafka](http://kafka.apache.org/) as a Windows Service using [TopShelf](https://github.com/Topshelf/Topshelf).

On starting the service will:

* Download Kafka 2.12-0.10.2.1
* Adjust the settings for a Windows environment
* Run Zookeeper in a seperate process
* Run Kafka in a seperate process

This is a single instance of Kafka useful for developing against locally.

## Prerequisites

You'll need the following first to get the service running:

* [JDK 1.8 or greater](http://www.oracle.com/technetwork/java/javase/downloads/jdk8-downloads-2133151.html)
* The [JAVA_HOME](https://confluence.atlassian.com/doc/setting-the-java_home-variable-in-windows-8895.html) system environmental variable set correctly

## Installation

* Build the project in Release mode
* Open Command Prompt as Administrator
* CD to the bin/Release directory
* Run "KafkaService.exe install"
* RUn "KafkaService.exe start"
* This will install and start a service called "Kafka.Service"
    * Configured to run under the Local Service account
    * Set to start automatically when your machine starts

## Uninstall

If you wish to uninstall the service:

* Open Command Prompt as Administrator
* CD to the bin/Release directory
* Run "KafkaService.exe uninstall"

## Configuration

You can configure the version of Kafka and the install location (default is C:\kafka_2.12-0.10.2.1) in the Constants.cs file.