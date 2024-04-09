namespace TransactionalBox.Base.BackgroundService.Internals
{
    internal sealed record JobLaunchSettings(Type JobType, int NumberOfInstances);
}
