namespace Infrastructure.Service;

internal static class Endpoints
{
    internal static string AuthorizeUri => "oauth2/access_token";

    internal static string UsersUri =>
        "api/v4/users";

    internal static string TaskTypesUri =>
        "api/v4/account?with=task_types";

    internal static string Pipeline => "api/v4/leads/pipelines";

    internal static string ContactsUri(int page, int limit)
    {
        return $"api/v4/contacts?with=leads&companies&order[updated_at]=desc&page={page}&limit={limit}";
    }

    internal static string ContactByIdUri(long id) =>
        $"api/v4/contacts/{id}?with=leads&companies&order[updated_at]=desc";


    internal static string TasksUri(int page, int limit)
    {
        return $"api/v4/tasks?with=leads&companies&page={page}&limit={limit}";
    }

    internal static string LeadsUri(int page, int limit)
    {
        return $"api/v4/leads?page={page}&limit={limit}";
    }

    internal static string EventsUri(int page, int limit)
    {
        return $"api/v4/events?page={page}&limit={limit}";
    }

    internal static string EventsFromUri(int timestamp, int page, int limit)
    {
        return $"api/v4/events?filter[created_at][from]={timestamp}&page={page}&limit={limit}";
    }


    internal static string MessagesFromUriFiltered(long timestamp, int page, int limit)
    {
        return $"api/v4/events" +
               $"?" +
               $"filter[type]=" +
               $"incoming_chat_message," +
               $"outgoing_chat_message" +
               $"&page={page}" +
               $"&limit={limit}" +
               $"&filter[created_at][from]={timestamp}";
    }
    
    
    
    internal static string MessagesFromUriUriBetweenDates(long startDate, long endDate, int page, int limit)
    {
        return $"api/v4/events?filter[created_at][from]={startDate}" + 
               $"&filter[created_at][to]={endDate}" +
               $"&filter[type]=incoming_chat_message,outgoing_chat_message" +
               $"&page={page}" +
               $"&limit={limit}";

    }
 
    internal static string EventsFromUriFiltered(long timestamp, int page, int limit)
    {
         
        
        return $"api/v4/events?order[created_at]=asc&filter[created_at][from]={timestamp}" +
            
               $"&filter[type]=" +
               $"lead_added," +
               $"lead_deleted," +
               $"lead_restored," +
               $"lead_status_changed," +
               $"lead_linked," +
               $"lead_unlinked," +
               $"contact_added," +
               $"contact_deleted," +
               $"contact_restored," +
               $"contact_linked," +
               $"contact_unlinked," +
               $"company_added," +
               $"company_deleted," +
               $"company_restored," +
               $"company_linked," +
               $"company_unlinked," +
               $"customer_added," +
               $"customer_deleted," +
               $"customer_status_changed," +
               $"customer_linked," +
               $"customer_unlinked," +
               $"task_added," +
               $"task_deleted," +
               $"task_completed," +
               $"task_type_changed," +
               $"task_text_changed," +
               $"task_deadline_changed," +
               $"task_result_added," +
               $"entity_direct_message," +
               $"entity_tag_added," +
               $"entity_tag_deleted," +
               $"entity_linked," +
               $"entity_unlinked," +
               $"sale_field_changed," +
               $"name_field_changed," +
               $"ltv_field_changed," +
               $"custom_field_value_changed," +
               $"entity_responsible_changed," +
               $"robot_replied," +
               $"intent_identified," +
               $"nps_rate_added," +
               $"link_followed," +
               $"transaction_added," +
               $"common_note_added," +
               $"common_note_deleted," +
               $"attachment_note_added," +
               $"targeting_in_note_added," +
               $"targeting_out_note_added," +
               $"geo_note_added," +
               $"service_note_added," +
               $"site_visit_note_added," +
               $"message_to_cashier_note_added," +
               $"key_action_completed," +
               $"entity_merged" +

               //$"incoming_chat_message," +
               //$"outgoing_chat_message" +
               $"&page={page}" +
               $"&limit={limit}" +
               $"";
    }
    
    internal static string EventsFromUriBetweenDates(long startDate, long endDate, int page, int limit)
    {
         
        
        return $"api/v4/events?order[created_at]=asc&filter[created_at][from]={startDate}" +
               $"&filter[created_at][to]={endDate}" +
               $"&filter[type]=" +
               $"lead_added," +
               $"lead_deleted," +
               $"lead_restored," +
               $"lead_status_changed," +
               $"lead_linked," +
               $"lead_unlinked," +
               $"contact_added," +
               $"contact_deleted," +
               $"contact_restored," +
               $"contact_linked," +
               $"contact_unlinked," +
               $"company_added," +
               $"company_deleted," +
               $"company_restored," +
               $"company_linked," +
               $"company_unlinked," +
               $"customer_added," +
               $"customer_deleted," +
               $"customer_status_changed," +
               $"customer_linked," +
               $"customer_unlinked," +
               $"task_added," +
               $"task_deleted," +
               $"task_completed," +
               $"task_type_changed," +
               $"task_text_changed," +
               $"task_deadline_changed," +
               $"task_result_added," +
               $"entity_direct_message," +
               $"entity_tag_added," +
               $"entity_tag_deleted," +
               $"entity_linked," +
               $"entity_unlinked," +
               $"sale_field_changed," +
               $"name_field_changed," +
               $"ltv_field_changed," +
               $"custom_field_value_changed," +
               $"entity_responsible_changed," +
               $"robot_replied," +
               $"intent_identified," +
               $"nps_rate_added," +
               $"link_followed," +
               $"transaction_added," +
               $"common_note_added," +
               $"common_note_deleted," +
               $"attachment_note_added," +
               $"targeting_in_note_added," +
               $"targeting_out_note_added," +
               $"geo_note_added," +
               $"service_note_added," +
               $"site_visit_note_added," +
               $"message_to_cashier_note_added," +
               $"key_action_completed," +
               $"entity_merged" +

               //$"incoming_chat_message," +
               //$"outgoing_chat_message" +
               $"&page={page}" +
               $"&limit={limit}" +
               $"";
        
        
      /*  return $"api/v4/events?order[created_at]=asc" +
               $"&filter[created_at][from]={startDate}" +
               $"&filter[created_at][to]={endDate}" +
               $"&filter[type]=lead_added,lead_deleted,lead_restored,lead_status_changed,lead_linked,lead_unlinked," +
               $"contact_added,contact_deleted,contact_restored,contact_linked,contact_unlinked," +
               $"company_added,company_deleted,company_restored,company_linked,company_unlinked," +
               $"customer_added,customer_deleted,customer_status_changed,customer_linked,customer_unlinked," +
               $"task_added,task_deleted,task_completed,task_type_changed,task_text_changed,task_deadline_changed," +
               $"task_result_added,entity_direct_message,entity_tag_added,entity_tag_deleted,entity_linked,entity_unlinked," +
               $"sale_field_changed,name_field_changed,ltv_field_changed,custom_field_value_changed,entity_responsible_changed," +
               $"robot_replied,intent_identified,nps_rate_added,link_followed,transaction_added,common_note_added," +
               $"common_note_deleted,attachment_note_added,targeting_in_note_added,targeting_out_note_added,geo_note_added," +
               $"service_note_added,site_visit_note_added,message_to_cashier_note_added,key_action_completed,entity_merged" +
               $"&page={page}" +
               $"&limit={limit}"; */
    }
}