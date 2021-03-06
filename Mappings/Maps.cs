﻿using AutoMapper;
using LeaveManagement.Data;
using LeaveManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Mappings
{
    public class Maps : Profile
    {

        public Maps()
        {
            CreateMap<LeaveType, LeaveTypeViewModel>().ReverseMap();

            CreateMap<LeaveAllocation, LeaveAllocationViewModel>().ReverseMap();
            CreateMap<LeaveAllocation, EditLeaveAllocationViewModel>().ReverseMap();


            CreateMap<LeaveRequest, LeaveRequestViewModel>().ReverseMap();

            CreateMap<Employee, EmployeeViewModel>().ReverseMap();

            //CreateMap<Employee, EmployeeViewModel>().ReverseMap();
        }
    }
}
