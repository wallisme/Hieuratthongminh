using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GetFeedBack.Models
{
    public partial class Users
    {
        public Users()
        {
            FeedBacks = new HashSet<FeedBacks>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public DateTime CreateDate { get; set; }
        public virtual ICollection<FeedBacks> FeedBacks { get; set; }
    }
}
