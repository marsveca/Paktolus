// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MarvinPaktolus.Models
{
    public partial class StudentHobbies
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int HobbieId { get; set; }

        public virtual Hobbies Hobbie { get; set; }
        public virtual Students Student { get; set; }
    }
}