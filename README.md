# open-game-research-engine
An Open Source Game Engine in F# and OpenTK.Next

This is an open project to build a game engine with server in .net using F# only.  Anyone who wishes to join in on the development is welcome.

## The domain

The domain model for this engine will be developed in three layers.  At the highest level is the abstract domain model that covers game elements that, while not fully inclusive, should cover a high percentage of the concepts that games are based upon.

### Level 1

The level 1 architectures contains conceptual abstract classes that represent a variety of game elements:

1. GameObject

   The GameObject class represents the top of the hierarchy.  This class can not be instantiated as it is abstract.
   Subclasses must implement `Update(...) and Render(...)` at the basre minimum.  Other methods may me overriden
   for additional capabilities.
   
2. Actor

   The ActorClass is derived from GameObject and is also abstract but contains built in components for movement and physics.
   If an object moves in the game it should be based on Actor unless you want to implement custom movement and physics.
   
3. Player

   The Player class represents the active player in the game and contains components for movement and physics that use
   plyer input as their primary source.
   
4. NonPlayerCharacter

   The NonPlayerCharacter class represents NPC's, it contains components for movement and physics as well as AI, path finding,
   and decision trees.
   
   
