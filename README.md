# Risky_Risk_AI
Implementing an intelligent player agent for Risk the board game

"Intelligence" is mimicked through the A-Star algorithm, it will generate a path from the current player's territory to a distant enemy territory.

The game map is represented using JSON; territory attributes, bordering countries, continents.

A sample territory would be set as followed:

```JSON
{ "name":"eastafrica", "continent":"africa", "neighbours":["congo","egypt","madagascar", "middleeast","southafrica"], "card":"soldier" }
```

A sample continent would be set as followed:

```JSON
{
    "name": "europe",
    "bonus": 5,
    "territories": 7
}
```

The `card` attribute represents the cash-in character that shows up 