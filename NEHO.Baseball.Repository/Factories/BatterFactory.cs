namespace NEHO.Baseball.Repository.Factories
{
    public class BatterFactory
    {
        public Batter CreateBatter(DTO.Batter batter)
        {
            return new Batter()
            {
                MLBAM_ID = batter.MLBAM_ID,
                AtBats = batter.AtBats,
                Runs = batter.Runs,
                Hits = batter.Hits,
                RBI = batter.RBI,
                Singles = batter.Singles,
                Doubles = batter.Doubles,
                Triples = batter.Triples,
                Homeruns = batter.Homeruns,
                Walks = batter.Walks,
                Strikeouts = batter.Strikeouts,
                HitByPitch = batter.HitByPitch,
                StolenBases = batter.StolenBases,
                CaughtStealing = batter.CaughtStealing,
                Errors = batter.Errors
            };
        }

        public DTO.Batter CreateBatter(Batter batter)
        {
            return new DTO.Batter()
            {
                //ID = player.ID,
                MLBAM_ID = batter.MLBAM_ID,
                AtBats = batter.AtBats,
                Runs = batter.Runs,
                Hits = batter.Hits,
                RBI = batter.RBI,
                Singles = batter.Singles,
                Doubles = batter.Doubles,
                Triples = batter.Triples,
                Homeruns = batter.Homeruns,
                Walks = batter.Walks,
                Strikeouts = batter.Strikeouts,
                HitByPitch = batter.HitByPitch,
                StolenBases = batter.StolenBases,
                CaughtStealing = batter.CaughtStealing,
                Errors = batter.Errors
            };
        }
    }
}
