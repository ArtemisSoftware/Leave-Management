using LeaveManagement.Contracts;
using LeaveManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Repository
{
    public class LeaveHistoryRepository : ILeaveHistoryRepository
    {

        private readonly ApplicationDbContext _db;

        public LeaveHistoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Create(LeaveRequest entity)
        {
            _db.LeaveRequests.Add(entity);
            return Save();
        }

        public bool Delete(LeaveRequest entity)
        {
            _db.LeaveRequests.Remove(entity);
            return Save();
        }

        public ICollection<LeaveRequest> FindAll()
        {
            return _db.LeaveRequests.ToList();
        }

        public LeaveRequest FindById(int id)
        {
            return _db.LeaveRequests.Find(id);
        }

        public bool isExists(int id)
        {
            return _db.LeaveRequests.Any(result => result.Id == id);
        }

        public bool Save()
        {
            int result = _db.SaveChanges();
            return result > 0;
        }

        public bool Update(LeaveRequest entity)
        {
            _db.LeaveRequests.Update(entity);
            return Save();
        }
    }
}
