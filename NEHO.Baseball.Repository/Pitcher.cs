//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NEHO.Baseball.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pitcher
    {
        public int ID { get; set; }
        public int MLBAM_ID { get; set; }
        public Nullable<int> Games { get; set; }
        public Nullable<int> GamesStarted { get; set; }
        public Nullable<int> Wins { get; set; }
        public Nullable<int> Losses { get; set; }
        public Nullable<int> Saves { get; set; }
        public Nullable<double> InningsPitched { get; set; }
        public Nullable<int> Hits { get; set; }
        public Nullable<int> HomerunsAllowed { get; set; }
        public Nullable<int> Walks { get; set; }
        public Nullable<int> Strikeouts { get; set; }
        public Nullable<int> HitBatters { get; set; }
        public Nullable<int> Runs { get; set; }
        public Nullable<int> EarnedRuns { get; set; }
    }
}