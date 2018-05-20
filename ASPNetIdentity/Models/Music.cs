using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MusicCollector.Models
{
    public class Album
    {
        [Key]
        public int EntryNo { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public decimal Duration { get; set; }

        public virtual ICollection<Release> ReleaseList { get; set; }
    }

    public class Release
    {
        [Key]
        public int EntryNo { get; set; }
        [ForeignKey("Album")]
        public int AlbumNo { get; set; }
        
        public virtual Album Album { get; set; }
    }
}