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
    public class EmployeeController : BaseController
    {
        public EmployeeController(IEmployeeService employeeService, IProjectService projectService) : base(employeeService, projectService) { }

        public ActionResult Index()
        {
            List<EmployeeDTO> employeeDTOList = EmployeeService.GetAll().ToList();
            List<EmployeeVM> employeeVMList = Mapper.Map<IEnumerable<EmployeeVM>>(employeeDTOList).ToList();

            return View(employeeVMList);
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            EmployeeDTO employeeDTO = EmployeeService.Get((Guid)id);
            if (employeeDTO == null)
                return HttpNotFound();

            EmployeeVM employeeVM = Mapper.Map<EmployeeVM>(employeeDTO);

            return View(employeeVM);
        }

        public ActionResult Create()
        {
            List<ProjectDTO> projectDTOList = ProjectService.GetAll().ToList();
            List<ProjectVM> projectVMList = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOList).ToList();
            ViewBag.ProjectID = new SelectList(projectVMList, "Id", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,MiddleName,Email")] EmployeeVM model)
        {
            if (ModelState.IsValid)
            {
                EmployeeDTO employeeDTO = Mapper.Map<EmployeeDTO>(model);
                EmployeeService.Add(employeeDTO);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            EmployeeDTO employeeDTO = EmployeeService.Get((Guid)id);
            if (employeeDTO == null)
                return HttpNotFound();

            EmployeeVM employeeVM = Mapper.Map<EmployeeVM>(employeeDTO);

            return View(employeeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,MiddleName,Email")] EmployeeVM model)
        {
            if (ModelState.IsValid)
            {
                EmployeeDTO employeeDTO = Mapper.Map<EmployeeDTO>(model);
                EmployeeService.Update(employeeDTO);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Delete(Guid id)
        {
            try
            {
                EmployeeService.Delete(id);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

            return RedirectToAction("Index");
        }
    }
}