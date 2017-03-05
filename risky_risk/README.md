# Risky_Risk_AI
Implementing an intelligent player agent for Risk the board game

"Intelligence" is mimicked through the A-Star algorithm, it will generate a path from the current player's territory to a distant enemy territory.

The game map is represented using JSON; territory attributes, bordering countries, continents.

A sample territory would be set as followed:

```JSON
{ "name":"eastafrica", "continent":"africa", "neighbours":["congo","egypt","madagascar", "middleeast","southafrica"], "card":"soldier" }
```

The `card` attribute on a territory represents the cash-in character that is on a territory card in the physical game; cannon, soldier or cavalry.

A sample continent would be set as followed:

```JSON
{
    "name": "europe",
    "bonus": 5,
    "territories": 7
}
```

The Board is an object representing the physical board of the game. The Board object consists of a list of all the territories and continents, and the current cash-in bonus value.

Mechanics.cs is orchestrates the logic of the game and delegating actions to players; such as initializing and updating the board, delegating actions to players for them to perform, and utility functions like random number generation.

## Game Logic

The game logic is carried out by Mechanics.cs, but the game loop exists in Program.cs.

Based on hardcored (for now) fields, the following initializations will take place:

* Players:
  * set name
  * set starting troop count
  * set empty territory list (will be initialized later)

* Board:
  * read territories from file and initialize territory list
  * read continents from file and initialize continent list

Once the territories have been read from file, they will be equally and randomly distributed to each player.

