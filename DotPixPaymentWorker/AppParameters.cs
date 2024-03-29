namespace DotPixPaymentWorker;

public class AppParameters
{
    public Database Database { get; set; }
    public RabbitMQ RabbitMq { get; set; }
    public string WorkerName { get; set; }
    public string PspMockUrl { get; set; }
}

public class RabbitMQ
{
    public string HostName { get; set; }
}

public class Database
{
    public string ConnectionString { get; set; }
}