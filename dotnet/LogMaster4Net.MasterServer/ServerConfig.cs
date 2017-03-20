public class ServerConfig
{
    public ListenerConfig[] Listners { get; set; }
}

public class ListenerConfig
{
    public string Ip { get; set; }
    
    public int Port { get; set; }
}