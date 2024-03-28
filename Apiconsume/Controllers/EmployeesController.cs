using Apiconsume.Modal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Apiconsume.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetEmployee")]
        public async Task<List<Employee>> Getall()
        {
            var data = await _context.Employees.ToListAsync();
            return data;
        }
        [HttpPost]
        [Route("empadd")]
        public async Task<IActionResult> empadd(Employee model)
        {
            try
            {
                if (model != null)
                {

                    await _context.Employees.AddAsync(model);
                   await _context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
            return Ok(new Response { Status = "success", Message = "Created Successfully" });
        }

        [HttpGet]
        [Route("getbyId")]
        public async Task<IActionResult> GetbyId(int id)
        {
            try
            {
                if(id > 0)
                {

                    var data= await _context.Employees.Where(x=> x.emp_Id == id).FirstOrDefaultAsync();
                    return Ok(data);
                }
                else {
                    return Ok(new Response { Status = "!!Failed", Message = "Data Not Found" });
                        };
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
