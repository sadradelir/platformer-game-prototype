# Platformer game prototype
- The main propose of the project is the showcase of making platformer controller in Unity which has the the proper tight platformer controls without actually using "Physics" in Unity game engine.
- Also I tried to experiment some new Unity 2D tile systems in advance use (using nighbourhood rules, cluster patches, ...)
- All the pixel art and animations in this project is also done by myself (for more pixel arts follow me on twitter ;)
# How to run?
- Clone the repo, open it up with Unity (2020 is prefered) and start from the MainMenu scene which is in the Scenes/MainScenes
- make sure you had enabled the "2D Tilemap Extras" in the Package Manager
# Show me some gifs plz!
- Rule based level editor (tiles coded to react their neighbours and change themselves to match)  see: Assets/Scripts/LevelDesign/CustomTile.cs

<img src="https://i.imgur.com/ao1bLQE.gif" height="300" />

- Custome colide detection with frame optimization for better performance on low end devices and avoid unpredictable outcomes (for speed runners)
see: Assets\Scripts\CharacterPhysics

<img src="https://i.imgur.com/0Czn4xj.gif" height="300" />

# Further notes
- this was a prototype for game jam event and done in 3 days (with animations and pixel art)
- cuz it was done in a tight time span it lacks some best practices which I mention here:
  - most of component variables are not implicitly access declared (yes most of them are public)
  - there is no proper design patterns used in order to maintain better software layering
  - some string constants and refrencing done in properly and in-line
  - yet there is some unused variables and classes

In the end what I was seeking through this project was to explore implementing my custom physics and collision detection in very extreme game enviroment and speed-run friendly era for further uses in my other projects.
