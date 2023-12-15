using Application.Enums;
using Application.Messages;

namespace Application.Features.Permissions.Messages
{
    public class PermissionMessage : BaseMessage<Guid>
    {
        private readonly Operation operation;

        //private Operation Operation { get; set; }
        public string NameOperation => Enum.GetName(typeof(Operation), operation);
        public PermissionMessage(Operation operation)
        {
            Id = Guid.NewGuid();
            operation = operation;
        }
    }
}
