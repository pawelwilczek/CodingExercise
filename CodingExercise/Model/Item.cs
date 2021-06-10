using CodingExercise.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodingExercise.Model
{
    public class Item
    {
        [Required(ErrorMessage = "Key is required.")]
        public string Key { get; set; }

        [Required(ErrorMessage = "Value is required.")]
        public string Value { get; set; }
    }
}
