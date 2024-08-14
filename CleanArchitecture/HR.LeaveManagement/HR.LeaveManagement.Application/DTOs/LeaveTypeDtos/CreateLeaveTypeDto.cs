namespace HR.LeaveManagement.Application.DTOs.LeaveTypeDtos
{
    public class CreateLeaveTypeDto
    {
        public string Name { get; set; } = null!;
        public int DefaultDays { get; set; }
    }
}