using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LeaveManagement.Contracts;
using LeaveManagement.Data;
using LeaveManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagement.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {

        private readonly ILeaveTypeRepository _leaveTyperepo;
        private readonly ILeaveRequestRepository _leaveaRequestrepo;
        private readonly ILeaveAllocationRepository _leaveaAllocationrepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;


        public LeaveRequestController(ILeaveRequestRepository leaveaRequestrepo, IMapper mapper, UserManager<Employee> userManager, ILeaveTypeRepository leaveTyperepo, ILeaveAllocationRepository leaveaAllocationrepo)
        {
            this._leaveaRequestrepo = leaveaRequestrepo;
            this._mapper = mapper;
            this._userManager = userManager;
            this._leaveTyperepo = leaveTyperepo;
            this._leaveaAllocationrepo = leaveaAllocationrepo;
        }


        [Authorize(Roles = "Administrator")]
        // GET: LeaveRequest
        public ActionResult Index()
        {

            var leaveRequestsModel = _mapper.Map<List<LeaveRequestViewModel>>(_leaveaRequestrepo.FindAll());

            var model = new AdminLeaveRequestViewModel
            {
                TotalRequests = leaveRequestsModel.Count,
                ApprovedRequests = leaveRequestsModel.Count(result => result.Approved == true),
                PendingRequests = leaveRequestsModel.Count(result => result.Approved == null),
                RejectedRequests = leaveRequestsModel.Count(result => result.Approved == false),
                LeaveRequests = leaveRequestsModel
            };

            return View(model);
        }

        // GET: LeaveRequest/Details/5
        public ActionResult Details(int id)
        {

            var leaveRequest = _leaveaRequestrepo.FindById(id);
            var model = _mapper.Map<LeaveRequestViewModel>(leaveRequest);

            return View(model);
        }


        public ActionResult ApproveRequest(int id)
        {
            try
            {

                var user = _userManager.GetUserAsync(User).Result;
                var leaveRequest = _leaveaRequestrepo.FindById(id);
                var allocation = _leaveaAllocationrepo.GetLeaveAllocationByEmployeeAndType(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);

                int daysRequested = (int)(leaveRequest.EndDate.Date - leaveRequest.StartDate.Date).TotalDays;

                allocation.NumberOfDays -= daysRequested;

                leaveRequest.Approved = true;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateActioned = DateTime.Now;


                _leaveaRequestrepo.Update(leaveRequest);
                _leaveaAllocationrepo.Update(allocation);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }

        }


        public ActionResult RejectRequest(int id)
        {
            try
            {

                var user = _userManager.GetUserAsync(User).Result;
                var leaveRequest = _leaveaRequestrepo.FindById(id);

                leaveRequest.Approved = false;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateActioned = DateTime.Now;


                var isSuccess = _leaveaRequestrepo.Update(leaveRequest);
                return RedirectToAction(nameof(Index), "Home");

            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index), "Home");
            }
        }


        // GET: LeaveRequest/Create
        public ActionResult Create()
        {

            var leaveTypes = _leaveTyperepo.FindAll();

            var leaveTypeItems = leaveTypes.Select(result => new SelectListItem
            {

                Text = result.Name,
                Value = result.Id.ToString()

            });

            var model = new CreateLeaveRequestViewModel
            {
                LeaveTypes = leaveTypeItems
            };

            return View(model);
        }

        // POST: LeaveRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateLeaveRequestViewModel model)
        {

            try
            {
                var startDate = Convert.ToDateTime(model.StartDate);
                var endDate = Convert.ToDateTime(model.EndDate);
                var leaveTypes = _leaveTyperepo.FindAll();

                var leaveTypeItems = leaveTypes.Select(result => new SelectListItem
                {

                    Text = result.Name,
                    Value = result.Id.ToString()

                });

                model.LeaveTypes = leaveTypeItems;


                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if(DateTime.Compare(startDate, endDate) > 1)
                {
                    ModelState.AddModelError("", "Start date biggert than end date");
                    return View(model);
                }

                var employee = _userManager.GetUserAsync(User).Result;
                var allocations = _leaveaAllocationrepo.GetLeaveAllocationByEmployeeAndType(employee.Id, model.LeaveTypeId);

                int daysRequested = (int)(endDate.Date - startDate.Date).TotalDays;

                if(daysRequested > allocations.NumberOfDays)
                {
                    ModelState.AddModelError("", "You dont have sufficient days for this request");
                    return View(model);
                }


                var leaveRequestModel = new LeaveRequestViewModel
                {
                    RequestingEmployeeId = employee.Id,
                    StartDate = startDate,
                    EndDate = endDate,
                    Approved = null,
                    DateRequested = DateTime.Now,
                    DateActioned = DateTime.Now,
                    LeaveTypeId = model.LeaveTypeId
                };

                var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestModel);

                var isSuccess = _leaveaRequestrepo.Create(leaveRequest);

                if (!isSuccess) {

                    ModelState.AddModelError("", "Something went wrong whit submitting your request");
                    return View(model);
                }


                return RedirectToAction(nameof(Index), "Home");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View();
            }
        }

        // GET: LeaveRequest/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaveRequest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveRequest/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveRequest/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}