using System;
using System.Collections.Generic;

namespace SmartNotes.Models
{
    public partial class Images
    {
        public int Id { get; set; }
        public int Noteid { get; set; }
        public string Image { get; set; }

        public virtual Notes Note { get; set; }
    }
}
