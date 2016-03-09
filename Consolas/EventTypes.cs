namespace Consolas
{
    public enum EventTypes
    {
        ApplicationStarted = 200,
        ApplicationLoaded = 201,
        ApplicationEnded = 202,
        ApplicationError = 500,
        OpenTaskRunSection = 300,
        OpenLogViewSection = 301,
        OpenTaskEditSection = 302,
        TaskStartRequested = 600,
        TaskStarted = 601,
        TaskCompleted = 602,
        TaskStartError = 700,
        TaskProcessError = 701
    }
}
