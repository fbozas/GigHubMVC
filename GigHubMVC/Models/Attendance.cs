using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GigHubMVC.Models
{
    public class Attendance
    {
        public int GigId { get; set; }
        public string AttendeeId { get; set; }

        public Gig Gig { get; set; }
        public ApplicationUser Attendee { get; set; }
    }
}