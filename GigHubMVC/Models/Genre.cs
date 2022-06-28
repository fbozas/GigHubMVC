using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GigHubMVC.Models
{
    public class Genre
    {
        public byte Id { get; set; }
        public string Name { get; set; }
    }
}