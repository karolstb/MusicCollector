﻿using Microsoft.AspNet.Identity.EntityFramework;
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
        [UIHint("TimeSpan")]
        public TimeSpan Duration { get; set; }
        [Display(Name ="Rok produkcji")]
        public int YearOfProduction { get; set; }

        public virtual ICollection<Release> ReleaseList { get; set; }

        //stringi readonly
        public string  DurationStr {
            get
            {
                return string.Format("{0}:{1}:{2}", Duration.Hours, Duration.Minutes, Duration.Seconds);
            }
        }
        public string MetaDescription {
            get
            {
                return string.Format("{0} - {1}", Author, Title);
            }
        }

        public virtual ICollection<Photo> PhotoList { get; set; }

        //public TimeSpan DurationPrintable
        //{
        //    get
        //    {
        //        return TimeSpan.FromSeconds()
        //    }
        //}
    }

    public class Release
    {
        [Key]
        public int EntryNo { get; set; }
        [ForeignKey("Album")] 
        public int AlbumNo { get; set; }
        public string RecordCompany { get; set; }   //jakie wydawnictwo
        public string ReleaseCode { get; set; }     //kod wydania 

        public virtual Album Album { get; set; }
        //public virtual CollectionEntry CollectionEntry { get; set; }

        public string MetaDescription
        {
            get
            {
                return string.Format("{0} - {1}", RecordCompany, ReleaseCode);
            }
        }
    }

    public class Photo
    {
        [Key]
        public int EntryNo { get; set; }
        [ForeignKey("Album")]
        public int? AlbumNo { get; set; }
        public virtual Album Album { get; set; }
        [ForeignKey("Release")]
        public int? ReleaseNo { get; set; }
        public virtual Release Release { get; set; }
        public bool IsMain { get; set; }

        //public int AlbumEntryNo
        //{
        //    get
        //    {
        //        return (AlbumNo == null) ? -1 : (int)AlbumNo;
        //    }
        //}

        public string FilePath { get; set; }
        public string UserUploaded { get; set; }
        [NotMapped]
        public HttpPostedFileBase PostedFile { get; set; }
    }

    /// <summary>
    /// wpisy kolekcji. czyli plyty jakie ma w kolekcji dany user
    /// </summary>
    public class CollectionEntry
    {
        [Key, Column(Order =1)]
        [ForeignKey("User")]
        public string UserNo { get; set; }
        public virtual MyApplicationUser User { get; set; }
        [Key, Column(Order = 2)]
        [ForeignKey("Album")]
        public int? AlbumNo { get; set; }
        public virtual Album Album { get; set; }
        [Key, Column(Order = 3)]
        [ForeignKey("Release")]
        public int? ReleaseNo { get; set; }
        public virtual Release Release { get; set; }

        public string Comment { get; set; }
        public string HashControlValue { get; set; }    //zawiera hash z klucza głównego na potrzeby CRUD'a, żeby nie dawać do url GUID usera
    }
}