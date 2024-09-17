using Cafe_NET_API.Constants;
using System.ComponentModel.DataAnnotations;
using static Cafe_NET_API.Constants.TypeEnum;

namespace Cafe_NET_API.Entities
{
    public class Employee
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email_Address { get; set; }

        [Required]
        [Range(80000000,99999999)]
        public int Phone_Number { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public DateTime Start_Date { get; set; }
    }
}
