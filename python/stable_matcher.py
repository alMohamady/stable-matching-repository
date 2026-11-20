"""Gale-Shapley matching for equally sized groups and complete rankings."""

from collections import deque


def match(
    player_order: list[str],
    player_preferences: dict[str, list[str]],
    coach_preferences: dict[str, list[str]],
) -> dict[str, str]:
    players = set(player_order)
    teams = set(coach_preferences)

    if (
        len(players) != len(player_order)
        or players != set(player_preferences)
        or len(players) != len(teams)
    ):
        raise ValueError(
            "Use unique players and equally sized player/team groups."
        )

    for preferences in player_preferences.values():
        if len(preferences) != len(teams) or set(preferences) != teams:
            raise ValueError("Every player must rank every team exactly once.")

    # Store each coach's ranking for constant-time comparisons.
    rank_by_team = {}
    for team, preferences in coach_preferences.items():
        if len(preferences) != len(players) or set(preferences) != players:
            raise ValueError("Every coach must rank every player exactly once.")
        rank_by_team[team] = {
            player: rank for rank, player in enumerate(preferences)
        }

    waiting_players = deque(player_order)
    next_choice = {player: 0 for player in player_order}
    match_by_team = {}

    while waiting_players:
        player = waiting_players.popleft()
        team = player_preferences[player][next_choice[player]]
        next_choice[player] += 1

        if team not in match_by_team:
            match_by_team[team] = player
            continue

        current_player = match_by_team[team]
        ranking = rank_by_team[team]
        if ranking[player] < ranking[current_player]:
            match_by_team[team] = player
            waiting_players.append(current_player)
        else:
            waiting_players.append(player)

    return match_by_team
