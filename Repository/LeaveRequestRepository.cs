using LeaveManagement.Contracts;
using LeaveManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Repository
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {

        private readonly ApplicationDbContext _db;

        public LeaveRequestRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(LeaveRequest entity)
        {
            await _db.LeaveRequests.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveRequest entity)
        {
            _db.LeaveRequests.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveRequest>> FindAll()
        {
            return await _db.LeaveRequests
                .Include(result => result.RequestingEmployee)
                .Include(result => result.ApprovedBy)
                .Include(result => result.LeaveType)
                .ToListAsync();
        }

        public async Task<LeaveRequest> FindById(int id)
        {
            return await _db.LeaveRequests
                .Include(result => result.RequestingEmployee)
                .Include(result => result.ApprovedBy)
                .Include(result => result.LeaveType)
                .FirstOrDefaultAsync(result => result.Id == id);
        }

        public async Task<ICollection<LeaveRequest>> GetLeaveRequestsByEmployee(string employeeid)
        {
            var leaveRequest = await FindAll();

            return leaveRequest.Where(result => result.RequestingEmployeeId == employeeid).ToList();
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.LeaveRequests.AnyAsync(result => result.Id == id);
        }

        public async Task<bool> Save()
        {
            int result = await _db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> Update(LeaveRequest entity)
        {
            _db.LeaveRequests.Update(entity);
            return await Save();
        }
    }
}
