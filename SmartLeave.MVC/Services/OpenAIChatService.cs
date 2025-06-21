using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using OpenAI;

public class OpenAIChatService
{
    private readonly string _apiKey;
    private readonly string _model;
    private readonly OpenAIClient _client;

    public OpenAIChatService(IConfiguration config)
    {
        _apiKey = config["OpenAIKey"];
        _model = config["ModelName"];
        _client = new OpenAIClient(_apiKey);
    }

    public async Task<string> GetResponseAsync(List<ChatMessage> chatHistory)
    {
        var chatClient = _client.GetChatClient(_model).AsIChatClient();

        string response = "";
        await foreach (var update in chatClient.GetStreamingResponseAsync(chatHistory))
        {
            response += update.Text;
        }
        return response;
    }
}
