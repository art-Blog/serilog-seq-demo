namespace serilog_seq_demo.Core.Module.Message.Implement;

public class MessageModule : IMessageModule
{
    private readonly HttpClient _client;

    public MessageModule(HttpClient client)
    {
        _client = client;
    }

    public string GetMessage()
    {
        return _client.GetAsync(new Uri(GetUrl())).GetAwaiter().GetResult().StatusCode.ToString();
    }

    protected string GetUrl()
    {
        return "https://www.google.com.tw";
    }
}