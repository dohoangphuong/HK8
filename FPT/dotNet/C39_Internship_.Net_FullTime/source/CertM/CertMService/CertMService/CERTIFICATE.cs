//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CertMService
{
    using System;
    using System.Collections.Generic;
    
    public partial class CERTIFICATE
    {
        public CERTIFICATE()
        {
            this.SCOREBOARD = new HashSet<SCOREBOARD>();
        }
    
        public string CertNo { get; set; }
        public string Email { get; set; }
        public System.DateTime Date { get; set; }
        public string Place { get; set; }
        public string Rank { get; set; }
        public string Major { get; set; }
        public string ClassNo { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    
        public virtual ACCOUNT ACCOUNT { get; set; }
        public virtual CLASS CLASS { get; set; }
        public virtual ICollection<SCOREBOARD> SCOREBOARD { get; set; }
    }
}
