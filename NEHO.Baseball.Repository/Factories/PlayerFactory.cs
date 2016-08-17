namespace NEHO.Baseball.Repository.Factories
{
    public class PlayerFactory
    {
        public Player CreatePlayer(DTO.Player player)
        {
            return new Player()
            {
                MLBAM_ID = player.MLBAM_ID,
                FirstName = player.FirstName,
                LastName = player.LastName
            };
        }

        public DTO.Player CreatePlayer(Player player)
        {
            return new DTO.Player()
            {
                ID = player.ID,
                MLBAM_ID = player.MLBAM_ID,
                FirstName = player.FirstName,
                LastName = player.LastName
            };
        }
    }
}