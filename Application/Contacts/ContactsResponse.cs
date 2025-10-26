using Newtonsoft.Json;

namespace Application.Contacts;

public class ContactsResponse
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Contact
    {
        [JsonProperty("id")] public long Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("first_name")] public string FirstName { get; set; }

        [JsonProperty("last_name")] public string LastName { get; set; }

        [JsonProperty("responsible_user_id")] public int ResponsibleUserId { get; set; }

        [JsonProperty("group_id")] public int GroupId { get; set; }

        [JsonProperty("created_by")] public int CreatedBy { get; set; }

        [JsonProperty("updated_by")] public int UpdatedBy { get; set; }

        [JsonProperty("created_at")] public int CreatedAt { get; set; }

        [JsonProperty("updated_at")] public int UpdatedAt { get; set; }

        [JsonProperty("closest_task_at")] public object ClosestTaskAt { get; set; }

        [JsonProperty("is_deleted")] public bool IsDeleted { get; set; }

        [JsonProperty("is_unsorted")] public bool IsUnsorted { get; set; }

        [JsonProperty("custom_fields_values")] public List<CustomFieldsValue> CustomFieldsValues { get; set; }

        [JsonProperty("account_id")] public int AccountId { get; set; }

        [JsonProperty("_links")] public Links Links { get; set; }

        [JsonProperty("_embedded")] public Embedded Embedded { get; set; }
    }

    public class CustomFieldsValue
    {
        [JsonProperty("field_id")] public int FieldId { get; set; }

        [JsonProperty("field_name")] public string FieldName { get; set; }

        [JsonProperty("field_code")] public string FieldCode { get; set; }

        [JsonProperty("field_type")] public string FieldType { get; set; }

        [JsonProperty("values")] public List<ValueHeader> Values { get; set; }
    }

    public class Embedded
    {
        [JsonProperty("contacts")] public List<Contact> Contacts { get; set; }

        [JsonProperty("tags")] public List<Tag> Tags { get; set; }

        [JsonProperty("leads")] public List<Lead> Leads { get; set; }

        [JsonProperty("companies")] public List<object> Companies { get; set; }
    }

    public class Lead
    {
        [JsonProperty("id")] public int Id { get; set; }

        [JsonProperty("_links")] public Links Links { get; set; }
    }

    public class Links
    {
        [JsonProperty("self")] public Self Self { get; set; }

        [JsonProperty("next")] public Next? Next { get; set; }
    }

    public class Next
    {
        [JsonProperty("href")] public string Href { get; set; }
    }

    public class Root
    {
        [JsonProperty("_page")] public int Page { get; set; }

        [JsonProperty("_links")] public Links Links { get; set; }

        [JsonProperty("_embedded")] public Embedded Embedded { get; set; }
    }

    public class Self
    {
        [JsonProperty("href")] public string Href { get; set; }
    }

    public class Tag
    {
        [JsonProperty("id")] public int Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("color")] public object Color { get; set; }
    }

    public class ValueHeader
    {
        [JsonProperty("value")] public object Value { get; set; }

        [JsonProperty("enum_id")] public int EnumId { get; set; }

        [JsonProperty("enum_code")] public string EnumCode { get; set; }
    }
}