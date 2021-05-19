namespace Common.ApplicationCore.Interfaces
{
    public interface ICurrentUserAccessor
    {
        string UserId { get; }
    }
}