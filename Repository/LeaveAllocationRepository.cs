using LeaveManagement.Contracts;
using LeaveManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Repository
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {

        private readonly ApplicationDbContext _db;

        public LeaveAllocationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CheckAllocation(int leavetypeid, string employeeid)
        {
            var period = DateTime.Now.Year;
            var allocations = await FindAll();
            return allocations.Where(result => result.EmployeeId == employeeid && result.LeaveTypeId == leavetypeid && result.Period == period).Any();
        }

        public async Task<bool> Create(LeaveAllocation entity)
        {
            await _db.LeaveAllocations.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveAllocation>> FindAll()
        {
            return await _db.LeaveAllocations.Include(result => result.LeaveType).ToListAsync();
        }

        public async Task<LeaveAllocation> FindById(int id)
        {
            return await _db.LeaveAllocations
                        .Include(result => result.LeaveType)
                        .Include(result => result.Employee)
                        .FirstOrDefaultAsync(result => result.Id == id);
        }

        public async Task<ICollection<LeaveAllocation>> GetLeaveAllocationByEmployee(string id)
        {
            var period = DateTime.Now.Year;
            var allocations = await FindAll();
            return allocations.Where(result => result.EmployeeId == id && result.Period == period).ToList();
        }

        public async Task<LeaveAllocation> GetLeaveAllocationByEmployeeAndType(string id, int leaveTypeId)
        {
            var period = DateTime.Now.Year;
            var allocations = await FindAll();
            return allocations.FirstOrDefault(result => result.EmployeeId == id && result.Period == period && result.LeaveTypeId == leaveTypeId);
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.LeaveAllocations.AnyAsync(result => result.Id == id);
        }

        public async Task<bool> Save()
        {
            int result = await _db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return await Save();
        }
    }
}
