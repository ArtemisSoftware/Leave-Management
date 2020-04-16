using LeaveManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Contracts
{
    public interface ILeaveAllocationRepository : IRepositoryBase<LeaveAllocation>
    {
        Task<bool> CheckAllocation(int leavetypeid, string employeeid);

        Task<ICollection<LeaveAllocation>> GetLeaveAllocationByEmployee(string id);

        Task<LeaveAllocation> GetLeaveAllocationByEmployeeAndType(string id, int leaveTypeId);
    }
}
