# Snake Online Prototype

v1.0.0 ==> 20251001

## Presentation

This project builds on a previous project called [Snake gaming campus](https://github.com/citrontruc/snake-gaming-campus) and tries to implement online functionalities in snake. It supports two player multiplayer.

Players control snake with the arrow key. Their objective is to eat as many apples as possible. It is possible to play with another player who has downloaded the github.

[![Video presentation](snake-online.mp4)](snake-online.mp4)

## Content

Game has two projects. Game and Game.Tests with Game.Tests containing the unit tests for our entities.

The Game folder contains multiple subfolders:
- Entities: Classes to create objects that populate a level.
- GridElements: Classes to create a grid where entities can be placed.
- Input: Classes to take care of user inputs.
- Online: Creates client, server and connection so that two players can communicate.
- Scenes: Levels and menus.
- Services: Singletons used to take care of global behaviours.
- Utils: Other classes providing useful, non-specific functionalities.

## Run the code

In order to run the code, you need to have Raylib and .net 8 installed on your computer. Run the code with the command
```bash
dotnet run --project Game
```
You can also grab one of the releases in the release tab of github.

In order to run tests run the following commmand:
```bash
dotnet test
```

## TODO

- Before sending instruction, have the client and server identify with a key? Use [concurrent dictionnary](https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2?view=net-9.0) maybe to handle all the player connections.
- 4 players multiplayer. Have the server assign a number to each of the players so that they can add the number to their message to indicate which snake to move.
- Have the server run the game and broadcast the position of every entity to the other players in order to avoid desynchronization.
- More tests. Some components are not completely tested (notably online functionalities).

## Known bugs

- Position of apples is not synchronized between the two players. The server should be the one responsible for telling where the apple is.
- Game can desynchronize if an UpdateMessage is not caught before the screen updates.

Have a great day!
