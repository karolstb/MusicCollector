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
        [Display(Name ="Autor")]
        public string Author { get; set; }
        [Display(Name ="Tytuł")]
        public string Title { get; set; }
        [Display(Name ="Długość")]
        [DataType(DataType.Duration)]
        [DisplayFormat(DataFormatString ="{hh:mm}", ApplyFormatInEditMode =true)]
        public TimeSpan Duration { get; set; }
        [Display(Name ="Rok produkcji")]
        public int YearOfProduction { get; set; }

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