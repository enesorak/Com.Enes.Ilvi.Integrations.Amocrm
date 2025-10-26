using Newtonsoft.Json;

namespace Application.Tasks;

public class TasksResponse
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
 // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
 public record Embedded(
     [property: JsonProperty("tasks")] IReadOnlyList<Task> Tasks
 );

 public record Links(
     [property: JsonProperty("self")] Self Self,
     [property: JsonProperty("next")] Next Next
 );

 public record Next(
     [property: JsonProperty("href")] string Href
 );

 public record Root(
     [property: JsonProperty("_page")] int? Page,
     [property: JsonProperty("_links")] Links Links,
     [property: JsonProperty("_embedded")] Embedded Embedded
 );

 public record Self(
     [property: JsonProperty("href")] string Href
 );

 public record Task(
     [property: JsonProperty("id")] long Id,
     [property: JsonProperty("created_by")] int? CreatedBy,
     [property: JsonProperty("updated_by")] int? UpdatedBy,
     [property: JsonProperty("created_at")] int? CreatedAt,
     [property: JsonProperty("updated_at")] long? UpdatedAt,
     [property: JsonProperty("responsible_user_id")] int? ResponsibleUserId,
     [property: JsonProperty("group_id")] int? GroupId,
     [property: JsonProperty("entity_id")] int? EntityId,
     [property: JsonProperty("entity_type")] string EntityType,
     [property: JsonProperty("duration")] int? Duration,
     [property: JsonProperty("is_completed")] bool? IsCompleted,
     [property: JsonProperty("task_type_id")] int? TaskTypeId,
     [property: JsonProperty("text")] string Text,
     [property: JsonProperty("result")] object Result,
     [property: JsonProperty("complete_till")] int? CompleteTill,
     [property: JsonProperty("account_id")] int? AccountId,
     [property: JsonProperty("_links")] Links Links
 );

}