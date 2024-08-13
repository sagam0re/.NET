using HR.LeaveManagement.Application.DTOs.Common;
using HR.LeaveManagement.Application.DTOs.LeaveTypeDtos;

namespace HR.LeaveManagement.Application.DTOs.LeaveAllocationDtos
{
    public class LeaveAllocationDto : BaseDto
    {
        public int NumberOfDays { get; set; }
        //Dtos should not know about Domain Entity. So use Dtos as a type
        public LeaveTypeDto LeaveType { get; set; } = null!;
        public int LeaveTypeId { get; set; }
        public int Period { get; set; }
    }
}