using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HR.LeaveManagement.MVC.Controllers
{
    [Authorize]
    public class LeaveRequestsController : Controller
    {
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly IMapper _mapper;

        public LeaveRequestsController(ILeaveTypeService leaveTypeService, ILeaveRequestService leaveRequestService,
            IMapper mapper)
        {
            this._leaveTypeService = leaveTypeService;
            this._leaveRequestService = leaveRequestService;
            this._mapper = mapper;
        }

        public async Task<ActionResult> Create()
        {
            var leaveTypes = await _leaveTypeService.GetLeaveTypes();
            var leaveTypeItems = new SelectList(leaveTypes, "Id", "Name");
            var model = new CreateLeaveRequestVM
            {
                LeaveTypes = leaveTypeItems
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateLeaveRequestVM leaveRequest)
        {
            if (ModelState.IsValid)
            {
                var response = await _leaveRequestService.CreateLeaveRequest(leaveRequest);
                if (response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", response.ValidationErrors);
            }

            var leaveTypes = await _leaveTypeService.GetLeaveTypes();
            var leaveTypeItems = new SelectList(leaveTypes, "Id", "Name");
            leaveRequest.LeaveTypes = leaveTypeItems;

            return View(leaveRequest);
        }
    }
}
