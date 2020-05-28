namespace Claims.Application.Core.Messaging
{
    public interface IMessageAccepted
    {
       bool Accepted { get; set; }
    }

    public class MessageAccepted : IMessageAccepted
    {
        public bool Accepted { get; set; }
    }
}