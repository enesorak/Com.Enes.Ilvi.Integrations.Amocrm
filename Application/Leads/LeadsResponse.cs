using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Application.Leads;

public class LeadsResponse
{
     public class CustomFieldsValue
    {
        [JsonProperty("field_id")]
        [JsonPropertyName("field_id")]
        public int FieldId { get; set; }

        [JsonProperty("field_name")]
        [JsonPropertyName("field_name")]
        public string FieldName { get; set; }

        [JsonProperty("field_code")]
        [JsonPropertyName("field_code")]
        public object FieldCode { get; set; }

        [JsonProperty("field_type")]
        [JsonPropertyName("field_type")]
        public string FieldType { get; set; }

        [JsonProperty("values")]
        [JsonPropertyName("values")]
        public List<Values> Values { get; set; }
    }

    public class Embedded
    {
        [JsonProperty("leads")]
        [JsonPropertyName("leads")]
        public List<Lead> Leads { get; set; }

        [JsonProperty("tags")]
        [JsonPropertyName("tags")]
        public List<Tag> Tags { get; set; }

        [JsonProperty("companies")]
        [JsonPropertyName("companies")]
        public List<object> Companies { get; set; }
    }

    public class Lead
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        [JsonPropertyName("price")]
        public int Price { get; set; }

        [JsonProperty("responsible_user_id")]
        [JsonPropertyName("responsible_user_id")]
        public int ResponsibleUserId { get; set; }

        [JsonProperty("group_id")]
        [JsonPropertyName("group_id")]
        public int GroupId { get; set; }

        [JsonProperty("status_id")]
        [JsonPropertyName("status_id")]
        public int StatusId { get; set; }

        [JsonProperty("pipeline_id")]
        [JsonPropertyName("pipeline_id")]
        public int PipelineId { get; set; }

        [JsonProperty("loss_reason_id")]
        [JsonPropertyName("loss_reason_id")]
        public int? LossReasonId { get; set; }

        [JsonProperty("created_by")]
        [JsonPropertyName("created_by")]
        public int CreatedBy { get; set; }

        [JsonProperty("updated_by")]
        [JsonPropertyName("updated_by")]
        public int UpdatedBy { get; set; }

        [JsonProperty("created_at")]
        [JsonPropertyName("created_at")]
        public int CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        [JsonPropertyName("updated_at")]
        public int? UpdatedAt { get; set; }

        [JsonProperty("closed_at")]
        [JsonPropertyName("closed_at")]
        public int? ClosedAt { get; set; }

        [JsonProperty("closest_task_at")]
        [JsonPropertyName("closest_task_at")]
        public object ClosestTaskAt { get; set; }

        [JsonProperty("is_deleted")]
        [JsonPropertyName("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("custom_fields_values")]
        [JsonPropertyName("custom_fields_values")]
        public List<CustomFieldsValue> CustomFieldsValues { get; set; }

        [JsonProperty("score")]
        [JsonPropertyName("score")]
        public object Score { get; set; }

        [JsonProperty("account_id")]
        [JsonPropertyName("account_id")]
        public int AccountId { get; set; }

        [JsonProperty("labor_cost")]
        [JsonPropertyName("labor_cost")]
        public object LaborCost { get; set; }

        [JsonProperty("_links")]
        [JsonPropertyName("_links")]
        public Links Links { get; set; }

        [JsonProperty("_embedded")]
        [JsonPropertyName("_embedded")]
        public Embedded Embedded { get; set; }
    }

    public class Links
    {
        [JsonProperty("self")]
        [JsonPropertyName("self")]
        public Self Self { get; set; }

        [JsonProperty("next")]
        [JsonPropertyName("next")]
        public Next? Next { get; set; }
    }

    public class Next
    {
        [JsonProperty("href")]
        [JsonPropertyName("href")]
        public string? Href { get; set; }
    }

    public class Root
    {
        [JsonProperty("_page")]
        [JsonPropertyName("_page")]
        public int Page { get; set; }

        [JsonProperty("_links")]
        [JsonPropertyName("_links")]
        public Links Links { get; set; }

        [JsonProperty("_embedded")]
        [JsonPropertyName("_embedded")]
        public Embedded Embedded { get; set; }
    }

    public class Self
    {
        [JsonProperty("href")]
        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

    public class Tag
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        [JsonPropertyName("color")]
        public object Color { get; set; }
    }

    public class Values
    {
        [JsonProperty("value")]
        [JsonPropertyName("value")]
        public object Value { get; set; }

        [JsonProperty("enum_id")]
        [JsonPropertyName("enum_id")]
        public int? EnumId { get; set; }

        [JsonProperty("enum_code")]
        [JsonPropertyName("enum_code")]
        public object EnumCode { get; set; }
    }
}