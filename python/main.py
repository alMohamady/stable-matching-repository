from stable_matcher import match


def main() -> None:
    player_order = ["Youssef", "Ali", "Omar"]

    player_preferences = {
        "Youssef": ["Red", "Green", "Blue"],
        "Ali": ["Blue", "Red", "Green"],
        "Omar": ["Red", "Blue", "Green"],
    }

    coach_preferences = {
        "Red": ["Omar", "Youssef", "Ali"],
        "Blue": ["Ali", "Omar", "Youssef"],
        "Green": ["Youssef", "Ali", "Omar"],
    }

    matches = match(player_order, player_preferences, coach_preferences)
    for team in coach_preferences:
        print(f"{team}: {matches[team]}")


if __name__ == "__main__":
    main()
