using System.Text;
using System.Text.Json;
using PlatformService.DTOs;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly IModel _channel;
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;
        ConnectionFactory factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQConfig:Host"] ?? throw new InvalidOperationException(),
            Port = int.Parse(_configuration["RabbitMQConfig:Port"] ?? throw new InvalidOperationException())
        };
        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare("trigger", ExchangeType.Fanout);
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

            Console.WriteLine("Connected to MessageBus");
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could not connect to the Message Bus: {e.Message}");
            throw;
        }
    }

    public void PublishNewPlatform(PlatformPublishDto platformPublishDto)
    {
        var message = JsonSerializer.Serialize(platformPublishDto);

        if (_connection.IsOpen)
        {
            Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
            SendMessage(message);
        }
        else
        {
            Console.WriteLine("--> RabbitMQ connection is closed, not sending");
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish("trigger", "", null, body);
        Console.WriteLine($"--> We have sent {message}");
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

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> RabbitMQ Connection Shutdown");
    }
}