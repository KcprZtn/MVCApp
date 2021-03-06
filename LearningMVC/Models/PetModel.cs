using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LearningMVC.Models
{
    
    public class PetModel
    {
        public int Id { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "You need to write your pet's name.")]
        public string Name { get; set; }

        
        [DisplayName("Species")]
        [Required(ErrorMessage = "You need to specify what specie is your pet.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "When was your pet born?")]
        [DisplayName("Year of birth")]
        [Range(1900,2100, ErrorMessage="Please enter correct year.")]

        public int BirthYear { get; set; }

        [DisplayName("Is alive?")]
        [Required(ErrorMessage = "Is your pet still alive?")]
        public bool isAlive  { get; set; }
    }

}
