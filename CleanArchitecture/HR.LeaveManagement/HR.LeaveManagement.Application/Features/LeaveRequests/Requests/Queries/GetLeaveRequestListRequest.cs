using System.Collections.Generic;
using HR.LeaveManagement.Application.LeaveRequestDtos;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries
{
    public class GetLeaveRequestListRequest : IRequest<LeaveRequestListDto>, IRequest<List<LeaveRequestDto>>
    {
        
    }
}