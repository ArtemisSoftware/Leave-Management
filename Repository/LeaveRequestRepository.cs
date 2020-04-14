﻿using LeaveManagement.Contracts;
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
            return _db.LeaveRequests
                .Include(result => result.RequestingEmployee)
                .Include(result => result.ApprovedBy)
                .Include(result => result.LeaveType)
                .ToList();
        }

        public LeaveRequest FindById(int id)
        {
            return _db.LeaveRequests
                .Include(result => result.RequestingEmployee)
                .Include(result => result.ApprovedBy)
                .Include(result => result.LeaveType)
                .FirstOrDefault(result => result.Id == id);
        }

        public ICollection<LeaveRequest> GetLeaveRequestsByEmployee(string employeeid)
        {
            return _db.LeaveRequests
                .Include(result => result.RequestingEmployee)
                .Include(result => result.ApprovedBy)
                .Include(result => result.LeaveType)
                .Where(result => result.RequestingEmployeeId == employeeid)
                .ToList();
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
