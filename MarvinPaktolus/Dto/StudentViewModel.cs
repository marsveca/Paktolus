using MarvinPaktolus.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MarvinPaktolus.Dto
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile no. is required")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        public string Phone { get; set; }
        [Required]
        [Range(100000, 999999, ErrorMessage = "Must be a 6 digits number")]
        public int Zip { get; set; }
        [EnsureOneElement(ErrorMessage = "At least a hobbie is required")]
        public List<HobbiesCheck> Hobbies { get; set; }
    }
    public class HobbiesCheck
    {
        public int Value { get; set; }
        public string Text { get; set; }
        public bool IsChecked { get; set; }
    }

    public class EnsureOneElementAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool isvalid = false;
            var list = value as IEnumerable;
            foreach (object o in list)
            {
                Type t = o.GetType();

                PropertyInfo[] pi = t.GetProperties();

                foreach (PropertyInfo p in pi)
                {
                    if (p.Name == "IsChecked" && (bool)p.GetValue(o))
                        isvalid = true;
                }
            }
            
            return isvalid;
        }
    }

}
