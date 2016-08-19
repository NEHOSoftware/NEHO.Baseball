namespace NEHO.Baseball.Repository.Factories
{
    public class PitcherFactory
    {
        public Pitcher CreatePitcher(DTO.Pitcher pitcher)
        {
            return new Pitcher()
            {
                MLBAM_ID = pitcher.MLBAM_ID,
                Games = pitcher.Games,
                GamesStarted = pitcher.GamesStarted,
                Wins = pitcher.Wins,
                Losses = pitcher.Losses,
                Saves = pitcher.Saves,
                InningsPitched = pitcher.InningsPitched,
                Hits = pitcher.Hits,
                HomerunsAllowed = pitcher.HomerunsAllowed,
                Walks = pitcher.Walks,
                Strikeouts = pitcher.Strikeouts,
                HitBatters = pitcher.HitBatters,
                Runs = pitcher.Runs,
                EarnedRuns = pitcher.EarnedRuns
        };
        }

        public DTO.Pitcher CreatePitcher(Pitcher pitcher)
        {
            return new DTO.Pitcher()
            {
                //ID = player.ID,
                MLBAM_ID = pitcher.MLBAM_ID,
                Games = pitcher.Games,
                GamesStarted = pitcher.GamesStarted,
                Wins = pitcher.Wins,
                Losses = pitcher.Losses,
                Saves = pitcher.Saves,
                InningsPitched = pitcher.InningsPitched,
                Hits = pitcher.Hits,
                HomerunsAllowed = pitcher.HomerunsAllowed,
                Walks = pitcher.Walks,
                Strikeouts = pitcher.Strikeouts,
                HitBatters = pitcher.HitBatters,
                Runs = pitcher.Runs,
                EarnedRuns = pitcher.EarnedRuns
            };
        }
    }
}