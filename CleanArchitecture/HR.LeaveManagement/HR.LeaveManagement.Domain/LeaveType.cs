using System;
using System.Collections.Generic;
using System.Text;
using HR.LeaveManagement.Domain.Common;

namespace HR.LeaveManagement.Domain
{
    public class LeaveType : BaseDomainEntity
    {
        public string Name { get; set; } = null!;
        public int DefaultDays { get; set; }
    }
}
