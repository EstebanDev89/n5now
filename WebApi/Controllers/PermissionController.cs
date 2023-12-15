using Application.DTOs;
using Application.Features.Permissions.Commands.ModifyPermission;
using Application.Features.Permissions.Commands.RequestPermission;
using Application.Features.Permissions.Queries.GetPermissionById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<PermissionController> logger;

        public PermissionController(IMediator mediator, ILogger<PermissionController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionDTO>> GetById(int id)
        {
            logger.LogInformation("Executing operation: {OperationName}", "Get");

            var query = new GetPermissionByIdQuery(id);
           
            var permission = await mediator.Send(query);

            if (permission == null)
            {
                return NotFound("Permission not found");
            }

            return permission;
        }

        [HttpPost]
        public async Task<ActionResult<PermissionDTO>> RequestNewPermission(RequestPermissionCommand command)
        {
            logger.LogInformation("Executing operation: {OperationName}", "Post");

            var permission = await mediator.Send(command);
            return CreatedAtAction(nameof(RequestNewPermission), permission);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Modify(int id, ModifyPermissionCommand command)
        {
            logger.LogInformation("Executing operation: {OperationName}", "Put");

            if (id != command.id)
            {
                return BadRequest("IDs not match");
            }

            var permission = await mediator.Send(command);

            if (permission == null)
            {
                return NotFound("Permission not found");
            }

            return NoContent();
        }

    }
}
