# Pathfinding
One of the focuses of this project was to create a game that incorporates pathfinding for the player and/or Non-Playable Characters (NPCs). In this project, the A* Search Algorithm has been implemented as the primary way of movement for NPCs in the game. The algorithm has also been utilised by the player. They can use the algorithm to find out what the fastest path to the closest Dunamis gem spawner is. Line of Sight (LOS) detection is also used for finding paths for NPCs when A* Search cannot find a path to the player.

## [Node](https://github.com/jamalcraig/Dunamis/blob/main/Unity%20CE301/Capstone%20Project/Assets/Scripts/Node.cs)
This is a class that has been used to store information of a point in a grid. The grids that are used for A* Search calculations are made up of these nodes. These nodes store data on their position and are assigned costs that are used in the A* Search calculation.

## [Variable Grid](https://github.com/jamalcraig/Dunamis/blob/main/Unity%20CE301/Capstone%20Project/Assets/Scripts/VariableGrid.cs)
This is a class that is used by the A* Search class to create a grid of nodes. Grids are created by creating a 2D array of nodes. For each element in the array, in the position of the node that will be stored in the element of the array, a spherical ray will be cast to detect if there is a game object in the position of that node. If there is, a node will be created, which will be assigned the same layer mask as the game object in that position. If there isn’t anything in the position of the node, it will be assigned to the traversable layer mask.   

When a grid is created, it is stored in the [Level Manager class](https://github.com/jamalcraig/Dunamis/blob/main/Technical%20Documentation/Game%20Management#level-manager). This is because the generation of the grid has a quadratic time complexity, so the larger the grid is, the slower generating the grid becomes. The generation of the grid requires multiple sphere ray casts for each node, which is very slow to do, and therefore causes the grid generation to take a long time, especially since this takes place during the quadratic time complexity loop. 

In order to overcome the slow generation times of the grid, I changed the code so that instead of creating a new grid every time an A* Search is run, only one grid is created when the scene loads, which gets stored in the Level Manager class. There is one grid for each class that uses A* Search (e.g., Racer). When another instance of a class is instantiated, it copies the grid and stores it locally in its instance of the class. Using this copied grid, the instance of the class updates the position of themselves in the grid by getting the node position they are in, rather than creating the grid again to do this. The effects of this optimisation is observed in the [test results](/Technical%20Documentation/Testing).   

The reason why this class is called variable grid is because it automatically creates nodes based on the maximum width of character that is using the grid. This makes sure that the A* Search will always return paths that the character can fit through. This one class can be used for generating the grid for a character of any size.   

This class also uses wire cube gizmos to make the grid visible in the scene view of the Unity editor. The layer masks of the nodes in the grid are identified by different colours. It is intensive for the renderer to display the grid using gizmos, especially the larger the grid becomes, so I usually have the class set to not display the grid, so that the game can run smoothly.   

**Variable Grid Visualised by Gizmos**   
![Variable Grid Visualised by Gizmos](/Reports/Media/Grid-5-edited.gif)   

## [A* search](https://github.com/jamalcraig/Dunamis/blob/main/Unity%20CE301/Capstone%20Project/Assets/Scripts/AStarBase.cs)
Different types of game objects that use path planning inherit methods from the base A* search
The standard A* Search algorithm has been implemented into a base class that multiple other classes inherit methods from. In order to calculate paths between different position in the level, A* Search base class utilises the [Variable Grid class](#variable-grid), which divides the level into a square grid of nodes. Using these nodes, the nodes that contain the start and end position of the path is identified. If the end position of the path is in a non-traversable node, the search will not do any calculations and will return false. If the end position of the path is on a traversable node, the calculation to find the shortest path between the start and end position will commence.   

The Manhattan distance between the current node and end node has been used for the heuristic in the A* Search, with a bias to turn at the beginning of the path to have a long straight.   

At the start of the calculation, two collections are created, which store the open set of nodes, and the closed set of nodes. Then, the following happens in a loop. The lowest costing node in the open set is chosen to be expanded. The nodes that surround it have their costs set or updated. If a node is in a certain layer mask where it is less desirable to pass through it, the cost for the node will be increased. Then next lowest costing node in the open set is chosen to be expanded. This loop continues until a complete path is found. The game character who initiated the A* Search will have their grid and path set to the path found by the A* Search.   

If there are multiple possible end positions, (like in the case of finding the nearest gem spawner,) an A* Search will be run for each possible end position. Then the path which has the lowest cost will used.    

The pathfinding classes that inherit the A* Search base class are used as bridges between the game character and the A* search. They are what game characters call to start the A* Search. These subclasses can contain bespoke additional A* Search methods. 



[Return to Main Repository](https://github.com/jamalcraig/Dunamis)