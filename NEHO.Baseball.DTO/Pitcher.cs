﻿using System.ComponentModel.DataAnnotations;

namespace NEHO.Baseball.DTO
{
    public class Pitcher
    {
        [Key]
        public int ID { get; set; }
        public int MLBAM_ID { get; set; }
        public int? Year { get; set; }
        public int? Games { get; set; }
        public int? GamesStarted { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Saves { get; set; }
        public double InningsPitched { get; set; }
        public int Hits { get; set; }
        public int HomerunsAllowed { get; set; }
        public int Walks { get; set; }
        public int Strikeouts { get; set; }
        public int HitBatters { get; set; }
        public int Runs { get; set; }
        public int EarnedRuns { get; set; }

    }
}