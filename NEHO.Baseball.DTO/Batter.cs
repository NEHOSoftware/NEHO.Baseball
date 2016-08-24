using System.ComponentModel.DataAnnotations;

namespace NEHO.Baseball.DTO
{
    public class Batter
    {
        [Key]
        public int ID { get; set; }
        public int MLBAM_ID { get; set; }
        public int? Year { get; set; }
        public int AtBats { get; set; }
        public int Runs { get; set; }
        public int Hits { get; set; }
        public int RBI { get; set; }
        public int Singles { get; set; }
        public int Doubles { get; set; }
        public int Triples { get; set; }
        public int Homeruns { get; set; }
        public int Walks { get; set; }
        public int Strikeouts { get; set; }
        public int HitByPitch { get; set; }
        public int StolenBases { get; set; }
        public int CaughtStealing { get; set; }
        public int Errors { get; set; }
    }
}