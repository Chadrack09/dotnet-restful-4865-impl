using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace warp_assessment.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname required")]
        public string Surname { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
        public string Country { get; set; }
        [BindProperty]
        public string FavoriteColour { get; set; } = "None";
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "MM-dd-yyyy", ApplyFormatInEditMode = true)]
        public DateTime BirthDay { get; set; }
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Cellphone number must contain numbers and only 10 characters")]
        public string CellPhoneNumber { get; set; }
        public string Comments { get; set; }
    }
}
