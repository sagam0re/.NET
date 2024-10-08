﻿using System.Threading;
using System.Threading.Tasks;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
    public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public DeleteLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
        }
        
        public async Task<Unit> Handle(DeleteLeaveTypeCommand command, CancellationToken cancellationToken)
        {
            var leaveType = await _leaveTypeRepository.Get(command.Id);
            await _leaveTypeRepository.Delete(leaveType);
            return Unit.Value;
        }
    }
}