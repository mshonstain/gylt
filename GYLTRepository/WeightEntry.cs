//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GYLTRepository
{
    using System;
    using System.Collections.Generic;
    
    public partial class WeightEntry
    {
        public int WeightEntryID { get; set; }
        public System.DateTimeOffset DateCreated { get; set; }
        public Nullable<System.DateTimeOffset> DateDeleted { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Weight { get; set; }
        public int WeightLossAttemptID { get; set; }
        public string Notes { get; set; }
    
        public virtual WeightLossAttempt WeightLossAttempt { get; set; }
    }
}