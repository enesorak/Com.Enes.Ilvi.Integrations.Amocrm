using Newtonsoft.Json;

namespace Application.Users;

public class UsersResponse
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Companies
    {
        [JsonProperty("view")]
        public string View { get; set; }

        [JsonProperty("edit")]
        public string Edit { get; set; }

        [JsonProperty("add")]
        public string Add { get; set; }

        [JsonProperty("delete")]
        public string Delete { get; set; }

        [JsonProperty("export")]
        public string Export { get; set; }
    }

    public class Contacts
    {
        [JsonProperty("view")]
        public string View { get; set; }

        [JsonProperty("edit")]
        public string Edit { get; set; }

        [JsonProperty("add")]
        public string Add { get; set; }

        [JsonProperty("delete")]
        public string Delete { get; set; }

        [JsonProperty("export")]
        public string Export { get; set; }
    }

    public class Embedded
    {
        [JsonProperty("users")]
        public List<User> Users { get; set; }
    }

    public class Leads
    {
        [JsonProperty("view")]
        public string View { get; set; }

        [JsonProperty("edit")]
        public string Edit { get; set; }

        [JsonProperty("add")]
        public string Add { get; set; }

        [JsonProperty("delete")]
        public string Delete { get; set; }

        [JsonProperty("export")]
        public string Export { get; set; }
    }

    public class Links
    {
        [JsonProperty("self")]
        public Self Self { get; set; }
    }

    public class Rights
    {
        [JsonProperty("leads")]
        public Leads Leads { get; set; }

        [JsonProperty("contacts")]
        public Contacts Contacts { get; set; }

        [JsonProperty("companies")]
        public Companies Companies { get; set; }

        [JsonProperty("tasks")]
        public Tasks Tasks { get; set; }

        [JsonProperty("mail_access")]
        public bool MailAccess { get; set; }

        [JsonProperty("catalog_access")]
        public bool CatalogAccess { get; set; }

        [JsonProperty("files_access")]
        public bool FilesAccess { get; set; }

        [JsonProperty("status_rights")]
        public List<StatusRight> StatusRights { get; set; }

        [JsonProperty("catalog_rights")]
        public object CatalogRights { get; set; }

        [JsonProperty("custom_fields_rights")]
        public object CustomFieldsRights { get; set; }

        [JsonProperty("oper_day_reports_view_access")]
        public bool OperDayReportsViewAccess { get; set; }

        [JsonProperty("oper_day_user_tracking")]
        public bool OperDayUserTracking { get; set; }

        [JsonProperty("is_admin")]
        public bool IsAdmin { get; set; }

        [JsonProperty("is_free")]
        public bool IsFree { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("group_id")]
        public int? GroupId { get; set; }

        [JsonProperty("role_id")]
        public int? RoleId { get; set; }

        [JsonProperty("view")]
        public string View { get; set; }

        [JsonProperty("edit")]
        public string Edit { get; set; }

        [JsonProperty("delete")]
        public string Delete { get; set; }
    }

    public class Root
    {
        [JsonProperty("_total_items")]
        public int TotalItems { get; set; }

        [JsonProperty("_page")]
        public int Page { get; set; }

        [JsonProperty("_page_count")]
        public int PageCount { get; set; }

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

    public class StatusRight
    {
        [JsonProperty("entity_type")]
        public string EntityType { get; set; }

        [JsonProperty("pipeline_id")]
        public int PipelineId { get; set; }

        [JsonProperty("status_id")]
        public int StatusId { get; set; }

        [JsonProperty("rights")]
        public Rights Rights { get; set; }
    }

    public class Tasks
    {
        [JsonProperty("edit")]
        public string Edit { get; set; }

        [JsonProperty("delete")]
        public string Delete { get; set; }
    }

    public class User
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("rights")]
        public Rights Rights { get; set; }

        [JsonProperty("_links")]
        public Links Links { get; set; }
    }


}