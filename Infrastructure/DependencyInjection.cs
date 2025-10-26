using Application.Abstractions.Clock;
using Application.Abstractions.Data;
using Application.Abstractions.Email;
using Application.Abstractions.Path;
using Application.Amocrm;
using Dapper;
using Domain.Abstractions;
using Domain.Contacts;
using Domain.Events;
using Domain.Leads;
using Domain.Messages;
using Domain.Pipelines;
using Domain.Tasks;
using Domain.Tasks.TaskTypes;
using Domain.Users;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using Infrastructure.Clock;
using Infrastructure.Data;
using Infrastructure.Email;
using Infrastructure.Options;
using Infrastructure.Path;
using Infrastructure.Repositories;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IPathFactory, PathFactory>();
        services.AddTransient<IEmailService, EmailService>();


        AddPersistence(services, configuration);
        AddAmocrm(services, configuration);
        
        AddHangfire(services, configuration);
    }


    private static void AddAmocrm(IServiceCollection services, IConfiguration configuration)
    {
        var baseUrl = configuration.GetValue<string>("Options:Base")!;
        services.Configure<AmocrmOptions>(
            configuration.GetSection("Options"));
        
        services.AddTransient<RateLimitHandler>();

        services.AddHttpClient<AmoCrmService>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(60 * 1000 * 5);
            client.BaseAddress = new Uri(baseUrl); // get through configuration
            client.DefaultRequestHeaders.Add("Accept", "application/json; charset=Utf8");
        });
  

        services.AddScoped<IServiceHandler, ServiceHandler>();
  
        
        
    }
    
    private static void AddHangfire(IServiceCollection services, IConfiguration configuration)
    {
        var options = new SqlServerStorageOptions
        {
            PrepareSchemaIfNecessary = false
        };


        if (!configuration.GetValue<bool>("Hangfire:UseMemoryStorage"))
            services.AddHangfire(g =>
                {
                        
                    g.UseSqlServerStorage(ConnectionStringFactory.GetConnectionString(configuration),
                        new SqlServerStorageOptions
                        {
                            PrepareSchemaIfNecessary = true, QueuePollInterval = TimeSpan.FromSeconds(1),
                            SchemaName = "hfo"
                        });
                }
            );
        else
            services.AddHangfire(g => g.UseMemoryStorage());

        services.AddHangfireServer(opt =>
        {
            GlobalJobFilters.Filters.Add(
                new AutomaticRetryAttribute { Attempts = 3, DelaysInSeconds = [300] });
            opt.WorkerCount = configuration.GetValue<int>("Hangfire:Workers");
           
        });
    }



    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(ConnectionStringFactory.GetConnectionString(configuration))
        );

        services.AddSingleton<ISqlConnectionFactory>(_ =>
            new SqlConnectionFactory(ConnectionStringFactory.GetConnectionString(configuration)));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());


        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<ITaskTypeRepository, TaskTypeRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>(); 
 
        services.AddScoped<ILeadRepository, LeadRepository>();
        services.AddScoped<IPipelineRepository, PipelineRepository>();
        
         services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
  
    }
}