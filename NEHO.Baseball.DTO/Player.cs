using System.ComponentModel.DataAnnotations;

namespace NEHO.Baseball.DTO
{
    public class Player
    {
        [Key]
        public int ID { get; set; }
        public int MLBAM_ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}