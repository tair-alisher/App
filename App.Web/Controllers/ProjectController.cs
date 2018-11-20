using App.LogicLayer.DTO;
using App.LogicLayer.Interfaces;
using App.Web.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    public class ProjectController : BaseController
    {
        public ProjectController(IEmployeeService employeeService, IProjectService projectService) : base(employeeService, projectService) { }

        public ActionResult Index()
        {
            List<ProjectDTO> projectDTOList = ProjectService
                .GetAll()
                .OrderBy(p => p.Title)
                .ToList();
            List<ProjectVM> projectVMList = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOList).ToList();

            List<EmployeeDTO> managerList = ProjectService.GetManagerList();
            ViewBag.Managers = Mapper.Map<IEnumerable<EmployeeVM>>(managerList);

            return View(projectVMList);
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProjectDTO projectDTO = ProjectService.Get((Guid)id);
            if (projectDTO == null)
                return HttpNotFound();

            ProjectVM projectVM = Mapper.Map<ProjectVM>(projectDTO);

            return View(projectVM);
        }

        public ActionResult Create()
        {
            ViewBag.ManagerId = GetEmployeeSelectList();

            return View();
        }

        private SelectList GetEmployeeSelectList(Guid? selectValue = null)
        {
            List<EmployeeDTO> employeeDTOList = EmployeeService.GetAll().ToList();
            List<EmployeeVM> employeeVMList = Mapper.Map<IEnumerable<EmployeeVM>>(employeeDTOList).ToList();

            return new SelectList(employeeVMList, "Id", "FullName", selectValue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Customer,Performer,Priority,Comment,DateStart,DateEnd,ManagerId")] ProjectVM model)
        {
            if (ModelState.IsValid)
            {
                ProjectDTO projectDTO = Mapper.Map<ProjectDTO>(model);
                ProjectService.Add(projectDTO);

                return RedirectToAction("Index");
            }

            ViewBag.ManagerId = GetEmployeeSelectList(model.ManagerId);

            return View(model);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProjectDTO projectDTO = ProjectService.Get((Guid)id);
            if (projectDTO == null)
                return HttpNotFound();

            ProjectVM projectVM = Mapper.Map<ProjectVM>(projectDTO);
            
            ViewBag.ManagerId = GetEmployeeSelectList(projectVM.ManagerId);

            return View(projectVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Customer,Performer,Priority,Comment,DateStart,DateEnd,ManagerId")] ProjectVM model)
        {
            if (ModelState.IsValid)
            {
                ProjectDTO projectDTO = Mapper.Map<ProjectDTO>(model);
                ProjectService.Update(projectDTO);

                return RedirectToAction("Index");
            }
            
            ViewBag.ManagerId = GetEmployeeSelectList(model.ManagerId);

            return View(model);
        }

        public ActionResult Delete(Guid id)
        {
            try
            {
                ProjectService.Delete(id);
            }
            catch (Exception ex)
            {
                return RedirectToRoute(new
                {
                    controller = "Base",
                    action = "Error",
                    message = ex.Message
                });
            }

            return RedirectToAction("Index");
        }

        public ActionResult AttachedEmployees(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProjectDTO projectDTO = ProjectService.Get((Guid)id);
            if (projectDTO == null)
                return HttpNotFound();

            ProjectVM projectVM = Mapper.Map<ProjectVM>(projectDTO);

            return View(projectVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AttachEmployee(Guid projectId, Guid employeeId)
        {
            EmployeeVM employeeVM;
            try
            {
                employeeVM = Mapper.Map<EmployeeVM>(EmployeeService.Get(employeeId));
                ProjectService.AttachEmployee(projectId, employeeId);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

            return PartialView(employeeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string DetachEmployee(Guid projectId, Guid employeeId)
        {
            try
            {
                ProjectService.DetachEmployee(projectId, employeeId);
                return "success";
            }
            catch (Exception)
            {
                return "fail";
            }
        }

        [HttpPost]
        public ActionResult GetFilteredAndSortedProjectList()
        {
            string sortProperty = Request.Form["sortProperty"];
            string dateStartFromFilter = Request.Form["dateStartFromFilter"];
            string dateStartToFilter = Request.Form["dateStartToFilter"];
            string priorityFilter = Request.Form["priorityFilter"];
            string managerFilter = Request.Form["managerFilter"];

            SortAndFilterParamsDTO sortAndFilterParams = new SortAndFilterParamsDTO
            {
                SortProperty = sortProperty,
                DateStartFromFilter = dateStartFromFilter,
                DateStartToFilter = dateStartToFilter,
                PriorityFilter = priorityFilter,
                ManagerFilter = managerFilter
            };

            List<ProjectDTO> projectDTOFilteredAndSortedList = ProjectService
                .GetFilteredAndSortedProjectList(sortAndFilterParams)
                .ToList();

            List<ProjectVM> projectVMFilteredAndSortedList = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOFilteredAndSortedList).ToList();

            return PartialView(projectVMFilteredAndSortedList);
        }
    }
}