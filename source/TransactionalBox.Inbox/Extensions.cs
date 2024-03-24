using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals;
using TransactionalBox.InboxBase.DependencyBuilder;

namespace TransactionalBox.Inbox
{
    public static class Extensions
    {
        public static IInboxDependencyBuilder AddInbox(
            this ITransactionalBoxBuilder builder,
            Action<IInboxStorageConfigurator> storageConfiguration)
        {
            var services = builder.Services;

            var storage = new InboxStorageConfigurator(services);

            storageConfiguration(storage);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var allTypes = assemblies.SelectMany(x => x.GetTypes());

            var inboxMessageHandlerTypes = allTypes
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IInboxMessageHandler<>)))
            .ToList();

            services.Scan(s =>
            s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IInboxMessageHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.AddSingleton<IInboxMessageTypes>(new InboxMessageTypes(inboxMessageHandlerTypes));
            services.AddHostedService<InboxProcessor>();

            return new InboxDependencyBuilder(services);
        }
    }
}
