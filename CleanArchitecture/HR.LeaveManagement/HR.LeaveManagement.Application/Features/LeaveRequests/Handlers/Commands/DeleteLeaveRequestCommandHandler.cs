using System.Threading;
using System.Threading.Tasks;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class DeleteLeaveRequestCommandHandler : IRequestHandler<DeleteLeaveRequestCommand>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;


        public DeleteLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
        }
        
        public async Task<Unit> Handle(DeleteLeaveRequestCommand command, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.Get(command.Id);
            await _leaveRequestRepository.Delete(leaveRequest);
            return Unit.Value;
        }
    }
}