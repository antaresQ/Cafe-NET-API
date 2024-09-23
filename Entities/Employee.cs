﻿using Cafe_NET_API.Constants;
using System.ComponentModel.DataAnnotations;
using static Cafe_NET_API.Constants.TypeEnum;

namespace Cafe_NET_API.Entities
{
    public class EmployeeBase
    {
        //UIXXXXXXXX
        public string? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email_Address { get; set; }

        [Required]
        [Range(80000000, 99999999)]
        public int Phone_Number { get; set; }
        public EmployeeBase()
        {
            
        }

    }

    public class Employee : EmployeeBase
    {
        public DateTime Start_Date { get; set; }

        [Required]
        public Gender Gender { get; set; }
        public Employee()
        {
            
        }
    }

    public class EmployeeCreateUpdate : Employee
    {
        public Guid Cafe_Id { get; set; }
        public EmployeeCreateUpdate()
        {
            
        }
    }

    public class EmployeeDetail : Employee
    {
        public DateTime Start_Date { get; set; }
        public string Cafe { get; set; }

        public EmployeeDetail()
        {
            
        }
    }

    public class EmployeeDetailView : Employee
    {
        public int Days_Worked { get; set; }
        public string Cafe { get; set; }
        public EmployeeDetailView()
        {
            
        }

        public EmployeeDetailView(EmployeeDetail empD)
        {
            Id = empD.Id;
            Name = empD.Name;
            Email_Address = empD.Email_Address;
            Phone_Number = empD.Phone_Number;
            Gender = empD.Gender;
            Cafe = empD.Cafe;

            var workedDays = DateTime.Now - empD.Start_Date;

            Days_Worked = workedDays.Days;
        }
    }
}
