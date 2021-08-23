# Game Management
There are multiple scripts that are used manage and maintain the state of the game.

## Score Manager
The [Score Manager](https://github.com/jamalcraig/Dunamis/blob/main/Unity%20CE301/Capstone%20Project/Assets/Scripts/ScoreManager.cs) class is used to keep track of the player’s score, health, and ammo count. These variables are displayed in the Heads Up Display (HUD) located near the player’s feet. If any of the three variables need to be updated, another class will reference the Score Manager class to call the method that updates the relevant variable. When a variable is updated, the HUD is also updated to reflect this.

## Spawners Manager
The [Spawners Manager](https://github.com/jamalcraig/Dunamis/blob/main/Unity%20CE301/Capstone%20Project/Assets/Scripts/SpawnersManager.cs) class is used to manage the timing of when enemies and Dunamis gems should spawn around the map.   

It spawns Dunamis gems by first finding and all the Dunamis gem spawners in the map and storing reference to them in an array. A descending timer is then used to dictate when a Dunamis gem should be spawned at a random gem spawner.    

The class spawns enemies by first finding all the positions in the map that an enemy can spawn in and storing a reference to them in an array. A descending timer is then used to dictate when an enemy should be spawned a random spawn position. There is a maximum number of enemies that can be in the level at once, so if the limit is reached, new enemies will not spawn until another has been killed. 

When an enemy is killed, the class for the enemy references this class to actually remove the game object from the game. This is so that the references of the all the enemies in the level can be maintained succinctly.  

## Level Manager
The [Level Manager](https://github.com/jamalcraig/Dunamis/blob/main/Unity%20CE301/Capstone%20Project/Assets/Scripts/LevelManager.cs) class stores the grids that used for A* search. Generating the grids that are used for A* search is a quite slow operation, so the resolution of this problem was to generate one of each type of grid at the start of the game. The game objects will then copy this grid, instead of having to generate their own individual grids, which upon doing so may cause frames to drop during playtime.  


[Return to Main Repository](https://github.com/jamalcraig/Dunamis)
