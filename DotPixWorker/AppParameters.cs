namespace DotPixWorker;

public class AppParameters
{
    public RabbitMQ RabbitMq { get; set; }
    public string WorkerName { get; set; }
}

public class RabbitMQ
{
    public string HostName { get; set; }
}