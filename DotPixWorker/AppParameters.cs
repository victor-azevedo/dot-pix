namespace DotPixWorker;

public class AppParameters
{
    public RabbitMQ RabbitMq { get; set; }
    public Database Database { get; set; }
}

public class RabbitMQ
{
    public string HostName { get; set; }
}

public class Database
{
    public string Dotpix { get; set; }
}