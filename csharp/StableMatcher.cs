public static class StableMatcher
{
    public static Dictionary<string, string> Match(
        string[] playerOrder,
        Dictionary<string, string[]> playerPreferences,
        Dictionary<string, string[]> coachPreferences)
    {
        ArgumentNullException.ThrowIfNull(playerOrder);
        ArgumentNullException.ThrowIfNull(playerPreferences);
        ArgumentNullException.ThrowIfNull(coachPreferences);

        var players = playerOrder.ToHashSet();
        var teams = coachPreferences.Keys.ToHashSet();

        if (players.Count != playerOrder.Length
            || !players.SetEquals(playerPreferences.Keys)
            || players.Count != teams.Count)
        {
            throw new ArgumentException(
                "Use unique players and equally sized player/team groups.");
        }

        foreach (var preferences in playerPreferences.Values)
        {
            if (preferences is null || preferences.Length != teams.Count
                || !teams.SetEquals(preferences))
            {
                throw new ArgumentException(
                    "Every player must rank every team exactly once.");
            }
        }

        // Store each coach's ranking for constant-time comparisons.
        var rankByTeam = new Dictionary<string, Dictionary<string, int>>();
        foreach (var (team, preferences) in coachPreferences)
        {
            if (preferences is null || preferences.Length != players.Count
                || !players.SetEquals(preferences))
            {
                throw new ArgumentException(
                    "Every coach must rank every player exactly once.");
            }

            var ranks = new Dictionary<string, int>();
            for (var rank = 0; rank < preferences.Length; rank++)
            {
                ranks[preferences[rank]] = rank;
            }
            rankByTeam[team] = ranks;
        }

        var waitingPlayers = new Queue<string>(playerOrder);
        var nextChoice = new Dictionary<string, int>();
        var matchByTeam = new Dictionary<string, string>();

        foreach (var player in playerOrder)
        {
            nextChoice[player] = 0;
        }

        while (waitingPlayers.Count > 0)
        {
            var player = waitingPlayers.Dequeue();
            var team = playerPreferences[player][nextChoice[player]];
            nextChoice[player]++;

            if (!matchByTeam.TryGetValue(team, out var currentPlayer))
            {
                matchByTeam[team] = player;
                continue;
            }

            var ranking = rankByTeam[team];
            if (ranking[player] < ranking[currentPlayer])
            {
                matchByTeam[team] = player;
                waitingPlayers.Enqueue(currentPlayer);
            }
            else
            {
                waitingPlayers.Enqueue(player);
            }
        }

        return matchByTeam;
    }
}
