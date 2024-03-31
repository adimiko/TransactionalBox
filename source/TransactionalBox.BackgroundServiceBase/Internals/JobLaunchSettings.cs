namespace TransactionalBox.BackgroundServiceBase.Internals
{
    internal sealed record JobLaunchSettings(Type JobType, int NumberOfInstances);
}
