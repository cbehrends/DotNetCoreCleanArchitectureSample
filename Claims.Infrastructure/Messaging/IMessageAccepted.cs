namespace Claims.Infrastructure.Messaging
{
    public interface IMessageAccepted
    {
       bool Accepted { get; set; }
    }
}