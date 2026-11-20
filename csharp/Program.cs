var playerOrder = new[] { "Youssef", "Ali", "Omar" };

var playerPreferences = new Dictionary<string, string[]>
{
    ["Youssef"] = new[] { "Red", "Green", "Blue" },
    ["Ali"] = new[] { "Blue", "Red", "Green" },
    ["Omar"] = new[] { "Red", "Blue", "Green" }
};

var coachPreferences = new Dictionary<string, string[]>
{
    ["Red"] = new[] { "Omar", "Youssef", "Ali" },
    ["Blue"] = new[] { "Ali", "Omar", "Youssef" },
    ["Green"] = new[] { "Youssef", "Ali", "Omar" }
};

var matches = StableMatcher.Match(
    playerOrder, playerPreferences, coachPreferences);

foreach (var team in coachPreferences.Keys)
{
    Console.WriteLine($"{team}: {matches[team]}");
}
