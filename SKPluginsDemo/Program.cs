using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Plugins.OpenApi;
using Shared;
using SharpYaml;
using SKPluginsDemo.Plugins;


Kernel openAIKernel = KernelFactory.CreateKernel(TypeKernel.OpenAI);
Kernel azureKernel = KernelFactory.CreateKernel(TypeKernel.AzureOpenAI);

#region Kernel Functions
//KernelFunction timeFunction = KernelFunctionFactory.CreateFromMethod(() => DateTime.Now.ToShortTimeString(),
//                                                functionName: "get_current_time",
//                                                description: "Gets the current time");

//KernelFunction poemFunction = KernelFunctionFactory.CreateFromPrompt("write a poem about Semantic Kernel",
//                                                    functionName: "write_poem",
//                                                    description: "write a poem about Semantic Kernel");
//var currentTime = await openAIKernel.InvokeAsync(poemFunction);
//Console.WriteLine(currentTime);

#endregion


#region Creating Native Functions
//KernelPlugin systemInfoPlugin = KernelPluginFactory.CreateFromType(typeof(SystemInfoPlugin));

//var systemInfo = await openAIKernel.InvokeAsync(systemInfoPlugin.Where(x => x.Name == "get_memory_ram").First());

//Console.WriteLine(systemInfo);

//var systemInfoTopProcesses = await openAIKernel.InvokeAsync(systemInfoPlugin.Where(x => x.Name == "get_top_memory_processes").First(), new KernelArguments()
//{
//    {"processes", 5   }
//});

//Console.WriteLine(systemInfoTopProcesses);

#endregion


#region using Built-in Plugins
//KernelPlugin conversationPlugin = KernelPluginFactory.CreateFromType<ConversationSummaryPlugin>();
//var filePlugin = KernelPluginFactory.CreateFromType<FileIOPlugin>();
//var httpPlugin = KernelPluginFactory.CreateFromType<HttpPlugin>();
//var timePlugin = KernelPluginFactory.CreateFromType<TimePlugin>();
//var textPlugin = KernelPluginFactory.CreateFromType<TextPlugin>();

//var summary = await openAIKernel.InvokeAsync(conversationPlugin.Where(x => x.Name == "SummarizeConversation").First(), new KernelArguments()
//{
//    {"input", "Scene:\r\nJamie and Alex are sitting in a shared workspace at their startup’s office. They’re discussing how to improve the customer support system using AI.\r\n\r\nAlex:\r\nHey Jamie, do you have a minute? I’ve been thinking about the support tickets backlog… it's kind of out of control.\r\n\r\nJamie:\r\nYeah, I noticed that too. Especially since last week’s release. We’ve had like 60 new tickets come in over the weekend alone. What's on your mind?\r\n\r\nAlex:\r\nI was wondering… what if we integrated an AI agent to help handle initial support requests? I mean, not to replace our team, but to triage things—categorize them, maybe even solve the basic stuff.\r\n\r\nJamie:\r\nHmm, interesting. Are you thinking of something like a chatbot, or more like a virtual assistant that classifies and routes things?\r\n\r\nAlex:\r\nA mix of both. Something that reads the user’s message, understands intent, checks our documentation or knowledge base, and either answers right away or sends it to the right person.\r\n\r\nJamie:\r\nOkay, I like that. That’d save a lot of time. Right now, most of the team spends the first hour of the day just figuring out what to respond to. What stack are you thinking about?\r\n\r\nAlex:\r\nWell, I’ve been playing with LangChain and LangGraph, and we could use one of those to build the workflow logic. Our current system already logs everything in MongoDB, so we could index that plus our documentation using a vector DB like Qdrant or Pinecone.\r\n\r\nJamie:\r\nNice, and how would the AI agent actually respond to users? Are we talking about integrating it directly into the app UI?\r\n\r\nAlex:\r\nThat’s what I wanted to ask you about. If we keep it simple, we could start with a widget that uses WebSockets or server-sent events to update in real time. Later we could integrate CopilotKit or something more advanced to make it feel more “agentic.”\r\n\r\nJamie:\r\nOkay, so step one is setting up the backend flow. Does the AI just take the raw message and try to classify it? Or do we need some prompt engineering to guide its response?\r\n\r\nAlex:\r\nWe’ll definitely need to do some prompt design. Like, imagine we structure a prompt like: “Here’s a user’s message. Determine the category, urgency, and suggest a possible resolution. If confident, respond; if not, escalate.” Something like that.\r\n\r\nJamie:\r\nThat makes sense. Also, how do we deal with security? Like, what if someone sends sensitive data, or tries to manipulate the AI?\r\n\r\nAlex:\r\nGood point. We could put in some safety filters—maybe check for PII or malicious input before sending it to the model. We can also use moderation tools, maybe from OpenAI or build our own simple layer.\r\n\r\nJamie:\r\nHow will the AI know our actual procedures, though? It needs context to not give wrong info.\r\n\r\nAlex:\r\nExactly. That’s where the RAG system comes in—Retrieval-Augmented Generation. We embed our docs, FAQs, previous support logs… Then during inference, the model retrieves relevant chunks to guide its answer.\r\n\r\nJamie:\r\nI like it. So, say a user asks, “Why can’t I log in after resetting my password?” The AI could search logs and docs, see that we had an issue with password resets in the last patch, and respond accordingly.\r\n\r\nAlex:\r\nYep. And if it’s unsure, it can summarize and escalate to the right team member. I’m thinking of tagging tickets with things like \"needs-human,\" \"billing,\" \"bug-report,\" etc.\r\n\r\nJamie:\r\nWould the agent learn over time? Like, get better at matching patterns?\r\n\r\nAlex:\r\nWe can start stateless, just with retrieval, but eventually we could fine-tune a model or log feedback loops. Even storing successful and failed resolutions for retraining.\r\n\r\nJamie:\r\nSounds like a lot, but exciting. Do we have bandwidth to prototype this?\r\n\r\nAlex:\r\nI think so. If you’re good with frontend integration, I can handle the backend logic. We can do a basic version in a week—maybe have it just triage and reply “Thanks, we’ve received your request. It’s being reviewed” for now.\r\n\r\nJamie:\r\nYeah, let’s do that. I’ll make a minimal UI with a chat widget. We’ll fake some parts at first just to test flow.\r\n\r\nAlex:\r\nCool. I’ll start with LangGraph and see how we can use a tool node for the database lookup and another for querying the docs. Then connect it to an OpenAI or Azure model.\r\n\r\nJamie:\r\nLet’s plan a quick design doc and get it reviewed with the team before we go too deep. Just to get buy-in.\r\n\r\nAlex:\r\nAgreed. Want to meet after lunch to draft that together?\r\n\r\nJamie:\r\nPerfect. Let’s make something that actually saves us time. I’m in."}
//});     

//Console.WriteLine(summary);
#endregion


//openAIKernel.Plugins.AddFromFunctions("MyPlugins", [timeFunction, poemFunction]);
openAIKernel.Plugins.AddFromType<SystemInfoPlugin>();


//Mode 1
await openAIKernel.ImportPluginFromOpenApiAsync(pluginName: "fakeRest",
    uri: new Uri("https://fakerestapi.azurewebsites.net/swagger/v1/swagger.json"),
    executionParameters: new OpenApiFunctionExecutionParameters()
    {
        EnablePayloadNamespacing = true,
    });

//Mode 2
//KernelPlugin apiPlugin = await OpenApiKernelPluginFactory.CreateFromOpenApiAsync(
//    pluginName: "fakeRest",
//    uri: new Uri("https://fakerestapi.azurewebsites.net/swagger/v1/swagger.json"),
//    executionParameters: new OpenApiFunctionExecutionParameters()
//    {
//        EnablePayloadNamespacing = true,
//    }
//);
//openAIKernel.Plugins.Add(apiPlugin);


//Mode 3 by specification
//using HttpClient client = new HttpClient();
//string url = "https://fakerestapi.azurewebsites.net/swagger/v1/swagger.json";
//var stream = await client.GetStreamAsync(url);
//OpenApiDocumentParser parser = new();

//RestApiSpecification specification = await parser.ParseAsync(stream);

//var booksOperations = specification.Operations.Where(o => o.Path == "/api/v1/Books");

//RestApiOperation operation = booksOperations.Single(o => o.Path == "/api/v1/Books" && o.Method == HttpMethod.Get);

//RestApiParameter idPathParameter = operation.Parameters.Single(p => p.Location == RestApiParameterLocation.Path && p.Name == "id");

//idPathParameter.ArgumentName = "bookId";

//openAIKernel.ImportPluginFromOpenApi(pluginName: "books", specification: specification);

var chatCompletionService = openAIKernel.GetRequiredService<IChatCompletionService>();

KernelFunction memoryFunction = openAIKernel.Plugins.GetFunction("SystemInfoPlugin", "get_top_memory_processes");
OpenAIPromptExecutionSettings settings = new OpenAIPromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(autoInvoke: true),
    Temperature = 0.5f  ,
};


ChatHistory _chatHistory = [];

string? input = null;

//_chatHistory.AddUserMessage("What time is it?");
//while (true)
//{
//    ChatMessageContent result = await chatCompletionService.GetChatMessageContentAsync(_chatHistory, settings, openAIKernel);
//    if (result.Content is not null)
//    {
//        Console.WriteLine($"Assistant: {result.Content}");
//        break;
//    }
//    _chatHistory.Add(result);

//    IEnumerable<FunctionCallContent> functionCalls = FunctionCallContent.GetFunctionCalls(result);

//    if (!functionCalls.Any())
//    {
//        break;
//    }

//    foreach (FunctionCallContent functionCall in functionCalls)
//    {
//        try
//        {
//            FunctionResultContent resultContent = await functionCall.InvokeAsync(openAIKernel);
//            _chatHistory.Add(resultContent.ToChatMessage());
//        }
//        catch (Exception ex)
//        {
//            _chatHistory.Add(new FunctionResultContent(functionCall, ex).ToChatMessage());
//            throw;
//        }


//    }
//}

while (true)
{

    Console.Write("User: ");
    input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Exiting...");
        break;
    }

    _chatHistory.AddUserMessage(input);

    var chatResult = await chatCompletionService.GetChatMessageContentAsync(_chatHistory, settings, openAIKernel);
    _chatHistory.AddAssistantMessage(chatResult.ToString());

    Console.Write($"Assistant: {chatResult}\n");

}







