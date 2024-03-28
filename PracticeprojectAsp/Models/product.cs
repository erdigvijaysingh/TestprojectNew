using System.ComponentModel.DataAnnotations;

namespace PracticeprojectAsp.Models
{
    public class products
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; }


    }
    public class equity
    {
        [Key]
        public int Id { get; set; }
        public string equityName { get; set; }
    }
}
