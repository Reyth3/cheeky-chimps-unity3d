using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class ERC721Metadata
{
    public class ERC721Attribute {
        [JsonProperty("trait_type")]
        public string TraitType { get; set; }
        public string Value { get; set; }
    }

    public string Name { get; set; } 
    public string Description { get; set; }
    public string Image { get; set; }
    public ERC721Attribute[] Attributes { get; set; }

    public async Task<byte[]> GetImageBytes()
    {
        using var http = new HttpClient();
        return await http.GetByteArrayAsync(Image);
    }
}
