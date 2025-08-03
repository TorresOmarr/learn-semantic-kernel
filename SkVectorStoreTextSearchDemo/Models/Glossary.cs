
using Microsoft.Extensions.VectorData;
using System.IO.IsolatedStorage;

namespace SkVectorStoreTextSearchDemo.Models
{
    public class Glossary
    {
        [VectorStoreKey]
        public string Key { get; set; } = string.Empty;
        [VectorStoreData(IsIndexed = true)]
        public string Category { get; set; } = string.Empty;
        [VectorStoreData]
        public string Term { get; set; } = string.Empty;
        [VectorStoreData]
        public string Definition { get; set; } = string.Empty;
        [VectorStoreVector(Dimensions: 1536)]
        public ReadOnlyMemory<float> DefinitionEmbedding { get; set; } = ReadOnlyMemory<float>.Empty;
    }
}
