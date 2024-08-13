using HR.LeaveManagement.Application.DTOs.Common;

namespace HR.LeaveManagement.Application.DTOs.LeaveTypeDtos
{
    public class LeaveTypeDto : BaseDto
    {
        public string Name { get; set; } = null!;
        public int DefaultDays { get; set; }
    }
}