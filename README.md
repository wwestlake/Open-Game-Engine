# open-game-engine
An Open Source Game Engine in F# and OpenTK.Next

This is an open project to build a game engine with server in .net using F# only.  Anyone who wishes to join in on the development is welcome.

## The domain

The domain model for this engine will be developed in three layers.  At the highest level is the abstract domain model that covers game elements that, while not fully inclusive, should cover a high percentage of the concepts that games are based upon.

### Level 1

The concept of a player.  Players are central to all games, and the player constuct is central to the operation of the engine.  A particular game (or instance) will extend this concept to the needs of the instance.  At the core of the system a user is simply and identifier.

```FSharp
           type Identifier = Identofoer of System.Guid
           
           type PlayerID = PlayerID of Identifier
```           
           
           
A typical game will want to create a record of some kind the represents a player, along with constructors and accessor functions.  Here is an example:

```FSharp

           type Player = {
              PlayeID: PlayerID;
              Level: int;
              Health: int;
              Lives: int;
              
           let getPlayerID {PlayerID playerID}  = playerID
```

so long as getPlayerID has the signature  `Player -> PlayerID` then it can be injected into the engine to allow the engine to obtain the ID from your record like this:

```FSharp
          Engine.Player.set typeof<Player> getPlayerID
          
```          


           
           
