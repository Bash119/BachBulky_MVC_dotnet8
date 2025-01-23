﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BachBulkyWebRazor_Temp.Models
{
    public class Category
    {
      
            [Key]
            public int Id { get; set; }
            [Required]
            [DisplayName("Category Name")]
            [MaxLength(30)]
            public string Name { get; set; }
            [DisplayName("Display Order")]
            [Range(1, 100, ErrorMessage = "Display Order must be bteween 1-100")]
            public int DisplayOrder { get; set; }

        
    }
}
