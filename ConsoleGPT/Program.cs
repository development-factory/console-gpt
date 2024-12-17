using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
var model = config["Model"] ?? string.Empty;
var key = config["OpenAIKey"] ?? string.Empty;

var chatClient = new OpenAIClient(key).AsChatClient(model);

var chatHistory = new List<ChatMessage>();

while (true)
{
    chatHistory.Add(new ChatMessage(ChatRole.System, "chat context"));

    Console.WriteLine("Your prompt:");
    var userPrompt = Console.ReadLine();

    chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));


    Console.WriteLine("AI response:");

    var response = string.Empty;

    await foreach (var item in chatClient.CompleteStreamingAsync(chatHistory))
    {
        Console.Write(item.Text);
        response += item.Text;
    }

    chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));
    Console.WriteLine();
}