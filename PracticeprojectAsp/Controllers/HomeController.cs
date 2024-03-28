using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using PracticeprojectAsp.Models;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PracticeprojectAsp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> login(User users)
        {
            if (users.Email != null && users.Password != null)
            {
                var data = await context.users.Where(x => x.Email == users.Email && x.Password == users.Password).FirstOrDefaultAsync();
                if (data != null)
                {

                    return RedirectToAction("getallemployees");
                }
                else
                {
                    return NotFound("Data not Found");
                }
            }
            else
            {
                return BadRequest("Invalid Model State");
            }


        }
        public async Task<IActionResult> getallemployees()
        {
            var data = await (from E in context.Employees
                              join p in context.products on E.product equals p.Id
                              select new envm
                              {
                                  emp_Id = E.emp_Id,
                                  emp_Age = E.emp_Age,
                                  Designation = E.Designation,
                                  emp_Name = E.emp_Name,
                                  emp_Salary = E.emp_Salary,
                                  productName = p.ProductName,
                              }).ToListAsync();


            //var data = await context.Employees.ToListAsync();
            //if(data != null) 
            //{ 
            //return View(data);
            //}
            //else
            //{
            //    return NotFound("data not found");
            //}
            return View(data);

        }
        public async Task<IActionResult> Create()
        {

            var data = await context.products.ToListAsync();
            ViewBag.product = new SelectList(data, "Id", "ProductName");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Employee emp)
        {
            if (emp == null)
            {
                return NotFound("Model is invalid");
            }
            else
            {
                if(emp.emp_Id != null)
                {
                     context.Employees.Update(emp);
                    context.SaveChanges();
                    return RedirectToAction("getallemployees");
                }
                else {

                    await context.Employees.AddAsync(emp);
                    context.SaveChanges();
                }
                
                // return RedirectToAction("getallemployees");
            }
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                if (id == 0)
                {
                    return NotFound("Data not found");
                }
                else
                {
                    productdet();
                    var data = await context.Employees.Where(x => x.emp_Id == id).FirstOrDefaultAsync();
                    return View(data);
                }
                return View();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
}

        private void productdet()
        {
            var data =  context.products.ToList();
            ViewBag.product = new SelectList(data, "Id", "ProductName");
        }

        public async Task<IActionResult> Dashboard()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
