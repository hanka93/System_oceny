//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace system_oceny.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Komentarzs
    {
        public int ocenaId { get; set; }
        public string tresc { get; set; }
        public System.DateTime data { get; set; }
    
        public virtual Ocenas Ocenas { get; set; }
    }
}
