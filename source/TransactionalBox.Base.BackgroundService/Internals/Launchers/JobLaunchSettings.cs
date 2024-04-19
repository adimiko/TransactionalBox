namespace TransactionalBox.Base.BackgroundService.Internals.Launchers
{
    internal sealed record JobLaunchSettings(Type JobType, int NumberOfInstances);
}
