using System;
using HR.LeaveManagement.Application.DTOs.Common;
using HR.LeaveManagement.Application.DTOs.LeaveTypeDtos;

namespace HR.LeaveManagement.Application.DTOs.LeaveRequestDtos
{
    public class LeaveRequestListDto : BaseDto
    {
        public LeaveTypeDto LeaveType { get; set; } = null!;
        public DateTime DateRequested { get; set; }
        protected bool? Approved { get; set; }
    }
}