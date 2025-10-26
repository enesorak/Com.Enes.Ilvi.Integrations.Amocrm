using Newtonsoft.Json;

namespace Application.Pipelines;

public class PipelineResponse
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Embedded
    {
        [JsonProperty("pipelines")]
        public List<Pipeline> Pipelines { get; set; }

        [JsonProperty("statuses")]
        public List<Status> Statuses { get; set; }
    }

    public class Links
    {
        [JsonProperty("self")]
        public Self Self { get; set; }
    }

    public class Pipeline
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sort")]
        public int Sort { get; set; }

        [JsonProperty("is_main")]
        public bool IsMain { get; set; }

        [JsonProperty("is_unsorted_on")]
        public bool IsUnsortedOn { get; set; }

        [JsonProperty("is_archive")]
        public bool IsArchive { get; set; }

        [JsonProperty("account_id")]
        public int AccountId { get; set; }

        [JsonProperty("_links")]
        public Links Links { get; set; }

        [JsonProperty("_embedded")]
        public Embedded Embedded { get; set; }
    }

    public class Root
    {
        [JsonProperty("_total_items")]
        public int TotalItems { get; set; }

        [JsonProperty("_links")]
        public Links Links { get; set; }

        [JsonProperty("_embedded")]
        public Embedded Embedded { get; set; }
    }

    public class Self
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }

    public class Status
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sort")]
        public int Sort { get; set; }

        [JsonProperty("is_editable")]
        public bool IsEditable { get; set; }

        [JsonProperty("pipeline_id")]
        public int PipelineId { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("account_id")]
        public int AccountId { get; set; }

        [JsonProperty("_links")]
        public Links Links { get; set; }
    }


}