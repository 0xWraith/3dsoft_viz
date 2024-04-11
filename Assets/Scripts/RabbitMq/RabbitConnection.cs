// namespace RabbitMq;

public abstract class RabbitConnection
{
    public string HostName { get; private set; }
    public string ExchangeChannel { get; private set; }

    protected RabbitConnection(string hostName, string exchangeChannel)
    {
        this.HostName = hostName;   
        this.ExchangeChannel = exchangeChannel;
    }
}
