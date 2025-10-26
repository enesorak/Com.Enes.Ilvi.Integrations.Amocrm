using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Application.Events;

public class EventResponse
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
      public class CustomFieldValue
     {
        [JsonProperty("field_id")]
        [JsonPropertyName("field_id")]
        public int FieldId;

        [JsonProperty("field_type")]
        [JsonPropertyName("field_type")]
        public int FieldType;

        [JsonProperty("enum_id")]
        [JsonPropertyName("enum_id")]
        public int? EnumId;

        [JsonProperty("text")]
        [JsonPropertyName("text")]
        public string Text;
    }

    public class Embedded
    {
        [JsonProperty("events")]
        [JsonPropertyName("events")]
        public List<Event> Events;

        [JsonProperty("entity")]
        [JsonPropertyName("entity")]
        public object Entity;
    }

    public class Event
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id;

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type;

        [JsonProperty("entity_id")]
        [JsonPropertyName("entity_id")]
        public int EntityId;

        [JsonProperty("entity_type")]
        [JsonPropertyName("entity_type")]
        public string EntityType;

        [JsonProperty("created_by")]
        [JsonPropertyName("created_by")]
        public int CreatedBy;

        [JsonProperty("created_at")]
        [JsonPropertyName("created_at")]
        public int CreatedAt;

        [JsonProperty("value_after")]
        [JsonPropertyName("value_after")]
        public List<ValueAfter> ValueAfter;

        [JsonProperty("value_before")]
        [JsonPropertyName("value_before")]
        public List<ValueBefore> ValueBefore;

        [JsonProperty("account_id")]
        [JsonPropertyName("account_id")]
        public int AccountId;

        [JsonProperty("_links")]
        [JsonPropertyName("_links")]
        public Links Links;

        [JsonProperty("_embedded")]
        [JsonPropertyName("_embedded")]
        public Embedded Embedded;
    }

    public class First
    {
        [JsonProperty("href")]
        [JsonPropertyName("href")]
        public string Href;
    }

    public class LeadStatus
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public int Id;

        [JsonProperty("pipeline_id")]
        [JsonPropertyName("pipeline_id")]
        public int PipelineId;
    }

    public class Links
    {
        [JsonProperty("self")]
        [JsonPropertyName("self")]
        public Self Self;

        [JsonProperty("next")]
        [JsonPropertyName("next")]
        public Next? Next;

        [JsonProperty("first")]
        [JsonPropertyName("first")]
        public First First;

        [JsonProperty("prev")]
        [JsonPropertyName("prev")]
        public Prev Prev;
    }

    public class Message
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id;

        [JsonProperty("origin")]
        [JsonPropertyName("origin")]
        public string Origin;
    }

    public class NameFieldValue
    {
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name;
    }

    public class Next
    {
        [JsonProperty("href")]
        [JsonPropertyName("href")]
        public string Href;
    }

    public class Prev
    {
        [JsonProperty("href")]
        [JsonPropertyName("href")]
        public string Href;
    }

    public class Root
    {
        [JsonProperty("_page")]
        [JsonPropertyName("_page")]
        public int Page;

        [JsonProperty("_links")]
        [JsonPropertyName("_links")]
        public Links Links;

        [JsonProperty("_embedded")]
        [JsonPropertyName("_embedded")]
        public Embedded Embedded;
    }

    public class SaleFieldValue
    {
        [JsonProperty("sale")]
        [JsonPropertyName("sale")]
        public int Sale;
    }

    public class Self
    {
        [JsonProperty("href")]
        [JsonPropertyName("href")]
        public string Href;
    }

    public class Tag
    {
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name;
    }

    public class ValueAfter
    {
        [JsonProperty("message")]
        [JsonPropertyName("message")]
        public Message Message;

        [JsonProperty("custom_field_value")]
        [JsonPropertyName("custom_field_value")]
        public CustomFieldValue CustomFieldValue;

        [JsonProperty("lead_status")]
        [JsonPropertyName("lead_status")]
        public LeadStatus LeadStatus;

        [JsonProperty("tag")]
        [JsonPropertyName("tag")]
        public Tag Tag;

        [JsonProperty("sale_field_value")]
        [JsonPropertyName("sale_field_value")]
        public SaleFieldValue SaleFieldValue;

        [JsonProperty("name_field_value")]
        [JsonPropertyName("name_field_value")]
        public NameFieldValue NameFieldValue;
    }

    public class ValueBefore
    {
        [JsonProperty("lead_status")]
        [JsonPropertyName("lead_status")]
        public LeadStatus LeadStatus;

        [JsonProperty("name_field_value")]
        [JsonPropertyName("name_field_value")]
        public NameFieldValue NameFieldValue;

        [JsonProperty("tag")]
        [JsonPropertyName("tag")]
        public Tag Tag;

        [JsonProperty("custom_field_value")]
        [JsonPropertyName("custom_field_value")]
        public CustomFieldValue CustomFieldValue;
    }

}