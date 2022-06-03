﻿using System;
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

        
        [DisplayName("Type of pet")]
        [Required(ErrorMessage = "You need to specify what type of animal your pet is.")]
        public string Type { get; set; }

        [DisplayName("Year of birth")]
     
        public int BirthYear { get; set; }

        [DisplayName("Is alive?")]
        public bool isAlive  { get; set; }
    }
}