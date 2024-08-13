using System;
using HR.LeaveManagement.Application.DTOs.Common;
using HR.LeaveManagement.Application.DTOs.LeaveTypeDtos;

namespace HR.LeaveManagement.Application.DTOs.LeaveRequestDtos
{
    public class LeaveRequestDto : BaseDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveTypeDto LeaveType { get; set; } = null!;
        public int LeaveTypeId { get; set; }
        public DateTime DateRequested { get; set; }
        public string RequestedComments { get; set; } = null!;
        public DateTime? DateActioned { get; set; }
        public bool Approved { get; set; }
        public bool Cancelled { get; set; }
    }
}