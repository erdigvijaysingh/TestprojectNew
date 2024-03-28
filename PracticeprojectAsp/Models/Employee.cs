using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PracticeprojectAsp.Models
{
    public class Employee
    {
        [Key]
        public int emp_Id { get; set; }
        public string emp_Name { get; set; }
        public int emp_Age { get; set; }
        public int emp_Salary { get; set; }
        public string Designation { get; set; }
        public int product { get; set; }
       // public int id { get; set; }
        //public List<SelectListItem> products { get; set; }

    }
}
