﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class FollowedThread : IUserIP
    {
        public long ID { get; set; }

        [Required]
        public Thread Thread { get; set; }

        public User User { get; set; }

        public string IP { get; set; }

        public long LastSeenPostID { get; set; }
    }
}