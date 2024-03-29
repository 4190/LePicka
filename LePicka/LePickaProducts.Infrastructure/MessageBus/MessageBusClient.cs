﻿using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;

namespace LePickaProducts.Infrastructure.MessageBus
{
    public interface IMessageBusClient
    {
        void PublishTestEvent();
    }

    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to Message Bus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }

        public void PublishTestEvent()
        {
            var message = "test message";
            SendMessageIfRabbitMQConnectionOpen(message);
        }


        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
            System.Diagnostics.Debug.WriteLine($"--> We have sent message: {message}");
        }

        private void SendMessageIfRabbitMQConnectionOpen(string message)
        {
            if (_connection.IsOpen)
            {
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ Connection closed, not sending message...");
            }
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}
