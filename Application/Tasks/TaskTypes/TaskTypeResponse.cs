using Newtonsoft.Json;

namespace Application.Tasks.TaskTypes;

public class TaskTypeResponse
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Embedded
    {
        [JsonProperty("task_types")]
        public List<TaskType> TaskTypes { get; set; }
    }

    public class Links
    {
        [JsonProperty("self")]
        public Self Self { get; set; }
    }

    public class Root
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("subdomain")]
        public string Subdomain { get; set; }

        [JsonProperty("created_at")]
        public int CreatedAt { get; set; }

        [JsonProperty("created_by")]
        public int CreatedBy { get; set; }

        [JsonProperty("updated_at")]
        public int UpdatedAt { get; set; }

        [JsonProperty("updated_by")]
        public int UpdatedBy { get; set; }

        [JsonProperty("current_user_id")]
        public int CurrentUserId { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("currency_symbol")]
        public string CurrencySymbol { get; set; }

        [JsonProperty("customers_mode")]
        public string CustomersMode { get; set; }

        [JsonProperty("is_unsorted_on")]
        public bool IsUnsortedOn { get; set; }

        [JsonProperty("mobile_feature_version")]
        public int MobileFeatureVersion { get; set; }

        [JsonProperty("is_loss_reason_enabled")]
        public bool IsLossReasonEnabled { get; set; }

        [JsonProperty("is_helpbot_enabled")]
        public bool IsHelpbotEnabled { get; set; }

        [JsonProperty("is_technical_account")]
        public bool IsTechnicalAccount { get; set; }

        [JsonProperty("contact_name_display_order")]
        public int ContactNameDisplayOrder { get; set; }

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

    public class TaskType
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string? Color { get; set; }

        [JsonProperty("icon_id")]
        public long? IconId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }


}