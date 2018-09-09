import socket
import unittest

from kazoo.client import KazooClient
from kafka import KafkaProducer, KafkaConsumer


class TestStringMethods(unittest.TestCase):
    def test_kafka_is_running(self):
        kafka_server = 'localhost:9092'
        zookeeper_server = 'localhost:2181'
        topic = 'integration_test_sample_topic'

        producer = KafkaProducer(bootstrap_servers=kafka_server)
        print(f'Sending dummy message')
        producer.send(topic, b'random_message')

        zk = KazooClient(hosts=zookeeper_server)
        zk.start()

        data = zk.get_children('/brokers/topics')
        self.assertIn(topic, data)


if __name__ == '__main__':
    unittest.main()
