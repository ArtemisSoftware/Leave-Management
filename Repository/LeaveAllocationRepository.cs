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

        public bool CheckAllocation(int leavetypeid, string employeeid)
        {
            var period = DateTime.Now.Year;
            var allocations = FindAll();
            return allocations.Where(result => result.EmployeeId == employeeid && result.LeaveTypeId == leavetypeid && result.Period == period).Any();
        }

        public bool Create(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Add(entity);
            return Save();
        }

        public bool Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return Save();
        }

        public ICollection<LeaveAllocation> FindAll()
        {
            return _db.LeaveAllocations.Include(result => result.LeaveType).ToList();
        }

        public LeaveAllocation FindById(int id)
        {
            return _db.LeaveAllocations
                        .Include(result => result.LeaveType)
                        .Include(result => result.Employee)
                        .FirstOrDefault(result => result.Id == id);
        }

        public ICollection<LeaveAllocation> GetLeaveAllocationByEmployee(string id)
        {
            var period = DateTime.Now.Year;
            return FindAll().Where(result => result.EmployeeId == id && result.Period == period).ToList();
        }

        public bool isExists(int id)
        {
            return _db.LeaveAllocations.Any(result => result.Id == id);
        }

        public bool Save()
        {
            int result = _db.SaveChanges();
            return result > 0;
        }

        public bool Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return Save();
        }
    }
}
