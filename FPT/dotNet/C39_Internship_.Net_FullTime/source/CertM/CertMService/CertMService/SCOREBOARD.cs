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
    
    public partial class SCOREBOARD
    {
        public string CertNo { get; set; }
        public string Category { get; set; }
        public double Mark { get; set; }
    
        public virtual CERTIFICATE CERTIFICATE { get; set; }
    }
}
