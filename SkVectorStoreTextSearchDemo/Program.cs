using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.InMemory;
using Shared;
using SkVectorStoreTextSearchDemo.Models;
using System.Linq.Expressions;

var kernel = KernelFactory.CreateKernel(TypeKernel.OpenAI);

var embeddingGenerator = kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

var vectorStore = kernel.GetRequiredService<InMemoryVectorStore>();
var collection = vectorStore.GetCollection<string, Glossary>("skGlossary");

await collection.EnsureCollectionExistsAsync();

var glossaryEntries = CreateGlossaryEntries().ToList();

var tasks = glossaryEntries.Select(entry => Task.Run(async () =>
{
    Embedding<float> embedding = await embeddingGenerator.GenerateAsync(entry.Definition);
    entry.DefinitionEmbedding = embedding.Vector;
}));

await Task.WhenAll(tasks);



IEnumerable<Task> upsertedKeysTasks = glossaryEntries.Select(entry => collection.UpsertAsync(entry));

await Task.WhenAll(upsertedKeysTasks);

string searchString = "What is a user story?";

Embedding<float> vectorizedSearchString = await embeddingGenerator.GenerateAsync(searchString);



IAsyncEnumerable<VectorSearchResult<Glossary>> searchResult = collection.SearchAsync(
    vectorizedSearchString,
    1,
    new VectorSearchOptions<Glossary>()
    {
        Skip= 0,
        VectorProperty = x => x.DefinitionEmbedding,
        Filter = x => x.Category == "Agile" 
    });


await foreach (VectorSearchResult<Glossary> result in searchResult)
{
    Console.WriteLine($"Found: {result.Record.Term} - {result.Record.Definition}");
    Console.WriteLine($"Score: {result.Score}");
    Console.WriteLine($"Category: {result.Record.Category}");
}




    IEnumerable<Glossary> CreateGlossaryEntries()
{
    // --- PROGRAMMING / GENERAL ---
    yield return new Glossary { Key = "1", Category = "Programming", Term = "Variable", Definition = "A variable is a storage location identified by a name that can hold a value." };
    yield return new Glossary { Key = "2", Category = "Programming", Term = "Function", Definition = "A function is a block of code that performs a specific task." };
    yield return new Glossary { Key = "3", Category = "Programming", Term = "Class", Definition = "A class is a blueprint for creating objects in object-oriented programming." };

    // --- BACKEND ---
    yield return new Glossary { Key = "4", Category = "Backend", Term = "REST API", Definition = "A REST API is an interface that allows communication between systems using HTTP requests and standard verbs." };
    yield return new Glossary { Key = "5", Category = "Backend", Term = "ORM", Definition = "Object-Relational Mapping (ORM) lets you interact with a database using objects instead of SQL queries directly." };

    // --- FRONTEND ---
    yield return new Glossary { Key = "6", Category = "Frontend", Term = "Component", Definition = "A component is a reusable piece of UI in frameworks like React or Angular." };
    yield return new Glossary { Key = "7", Category = "Frontend", Term = "SPA", Definition = "A Single Page Application (SPA) loads a single HTML page and updates the view dynamically as the user interacts." };

    // --- CLOUD ---
    yield return new Glossary { Key = "8", Category = "Cloud", Term = "Serverless", Definition = "Serverless is a cloud execution model where you don’t manage servers; code runs in response to events." };
    yield return new Glossary { Key = "9", Category = "Cloud", Term = "Container", Definition = "A container is a lightweight, standalone, executable package that includes everything needed to run a piece of software." };

    // --- DEVOPS ---
    yield return new Glossary { Key = "10", Category = "DevOps", Term = "CI/CD", Definition = "Continuous Integration/Continuous Deployment (CI/CD) automates building, testing, and deploying applications." };
    yield return new Glossary { Key = "11", Category = "DevOps", Term = "Infrastructure as Code", Definition = "Infrastructure as Code (IaC) is managing and provisioning computing infrastructure using machine-readable files." };

    // --- DATABASE ---
    yield return new Glossary { Key = "12", Category = "Database", Term = "Index", Definition = "An index is a data structure that improves the speed of data retrieval on a database table." };
    yield return new Glossary { Key = "13", Category = "Database", Term = "ACID", Definition = "ACID stands for Atomicity, Consistency, Isolation, Durability – properties that ensure reliable database transactions." };

    // --- TESTING ---
    yield return new Glossary { Key = "14", Category = "Testing", Term = "Unit Test", Definition = "A unit test verifies that a single part of your code (usually a function or method) works as intended." };
    yield return new Glossary { Key = "15", Category = "Testing", Term = "Mock", Definition = "A mock is a simulated object that mimics the behavior of real objects in controlled ways during testing." };

    // --- AI / MACHINE LEARNING ---
    yield return new Glossary { Key = "16", Category = "AI", Term = "Embedding", Definition = "An embedding is a numeric representation of data (like words or images) in a lower-dimensional space, used for tasks like similarity search." };
    yield return new Glossary { Key = "17", Category = "AI", Term = "RAG", Definition = "Retrieval-Augmented Generation (RAG) is an AI technique that combines language models with information retrieval for more accurate answers." };

    // --- SCRUM / AGILE ---
    yield return new Glossary { Key = "18", Category = "Agile", Term = "User Story", Definition = "A user story describes a feature from an end-user perspective and is used to guide agile development." };
    yield return new Glossary { Key = "19", Category = "Agile", Term = "Sprint", Definition = "A sprint is a set period during which specific work has to be completed and made ready for review." };

    // --- SECURITY ---
    yield return new Glossary { Key = "20", Category = "Security", Term = "JWT", Definition = "JSON Web Token (JWT) is a compact, URL-safe means of representing claims between two parties." };
    yield return new Glossary { Key = "21", Category = "Security", Term = "OAuth", Definition = "OAuth is an open-standard authorization protocol that allows apps to access user information without exposing credentials." };

}




