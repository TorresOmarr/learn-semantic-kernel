// See https://aka.ms/new-console-template for more information
using Microsoft.SemanticKernel;
using Shared;

Console.WriteLine("Hello, World!");



Kernel openAIKernel = KernelFactory.CreateKernel(TypeKernel.OpenAI);


var prompt = "Who is Isaac Newton?";


var questionFunction = openAIKernel.CreateFunctionFromPrompt(prompt);

var result = await questionFunction.InvokeAsync(openAIKernel);


Console.WriteLine($"Result: {result}");


