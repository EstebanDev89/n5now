namespace Application.Features.Permissions.Messages
{
    public interface IPermissionEventPublisher
    {
        Task ProduceAsync(PermissionMessage message);
    }
}