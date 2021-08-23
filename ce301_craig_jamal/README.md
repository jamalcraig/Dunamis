# VR Game for Mobile, which Implements Pathfinding
This project is based around creating a Virtual Reality game for mobile, built in Unity using C#. The game is about collecting Dunamis Gems that are spawned at intervals around the map. Players are awarded points for collecting these gems. These gems are also the player's ammunition for killing enemies. There are enemies that chase the player around the map. When they come close to the player and hit the player, they steal the player's health, then run towards an exit. If they are killed by the player before they exit, they will drop the health and the player can pick it up to replenish his health. These enemies use A* search to calculate the paths that they use to move towards the player or exit.  The player is also able to send a clone of them self  that will find and retrieve gems for them. The clone also uses A* search for its path finding.  

## How to run the project build (on iPhone)
### Prerequisites
* iPhone with iOS 11 (or later).
* Google Cardboard, or another VR Head Mounted Display (HMD) that is based on Google Cardboard's API.
* PlayStation 4 (PS4) Controller
* Xcode 12 (or later)

### Installing
Download the `/Unity CE301/Capstone Project/Builds` folder  
In the Builds folder, open `Unity-iPhone.xcodeproj` in Xcode  
Plug into the computer an iPhone with iOS 11 or later  
Build and run the project - the game will be installed as an app on the iPhone  
(The Signing & Capabilities may need to be reconfigured in Xcode and in Unity, in order to install the app)  

### Running
Connect the PS4 Controller to the iPhone using bluetooth  
Open the `CapstoneProject` app on the iPhone  
Put the iPhone into the VR HMD  


## How to run the project source files (in Unity)
### Prerequisites
* Unity Hub
* Unity 2019.4.11f 
* (Optional) PlayStation 4 Controller 

### Installing
Once the prerequsites are downloaded, download all the files in the `Unity CE301` folder of this repository  
Open Unity Hub  
Click on the "Add" button, then in the file explorer it brings up, find the `Unity CE301` folder, then inside the folder, click on the `Capstone Project` folder to add it to the Unity Hub interface.  
Open the `Capstone Project` project  
Press the play button to run the game  
The different scenes of the game can be accessed by clicking on the different scenes, by going into the `/Assets/Scenes` folder in the Project window  




### Versioning Statergy
What versioning statergy are you using for your project, something like [semantic versioning](https://semver.org/) might be a good idea.


## Authors
Provide your names here
* Fred Bloggs - UI Design

## References
* [Gitlab Markdown Guide](https://docs.gitlab.com/ee/user/markdown.html)
* [Example 1](https://github.com/erasmus-without-paper/ewp-specs-sec-intro/tree/v2.0.2)
* [Example 2](https://github.com/erasmus-without-paper/ewp-specs-architecture/tree/v1.10.0)

## Table of Contents
1) Introduction
   - 1.1 [Abstract](#the-original-project-description)
   - 1.2 [How to run the project build (on iPhone)](#how-to-run-the-project-build-on-iphonesummary)
     - [Prerequisites](#prerequisites)
     - [Installing](#installing)
     - [Running](#running)
   -  1.3 [How to run the project source files (in Unity)](#how-to-run-the-project-source-files-in-unity)
      - [Prerequisites](#prerequisites-1)
      - [Installing](#installing-1)

2) [Pathfinding](/Technical%20Documentation/Pathfinding/README.md)
   - 2.1 [Pathfinding](/Technical%20Documentation/Pathfinding/README.md)
   - 2.2 [Node](/Technical%20Documentation/Pathfinding/README.md#node)
   - 2.3 [Variable Grid](/Technical%20Documentation/Pathfinding/README.md#variable-grid)
   - 2.4 [A\* Search](/Technical%20Documentation/Pathfinding/README.md#A*-search)

3) [Game Characters](/Technical%20Documentation/Game%20Characters)
   - 3.1 [Game Characters](/Technical%20Documentation/Game%20Characters/README.md#game-characters)
   - 3.2 [Key Bindings](/Technical%20Documentation/Game%20Characters/README.md#key-bindings)
   - 3.3 [Player](/Technical%20Documentation/Game%20Characters/README.md#player)
   - 3.4 [Racer](/Technical%20Documentation/Game%20Characters/README.md#racer)
   - 3.5 [Player Clone](/Technical%20Documentation/Game%20Characters/README.md#player-clone)

4) [Game Management](/Technical%20Documentation/Game%20Management)
   - 4.1 [Game Management](/Technical%20Documentation/Game%20Management/README.md#game-management)
   - 4.2 [Score Manager](/Technical%20Documentation/Game%20Management/README.md#score-manager)
   - 4.3 [Spawners Manager](/Technical%20Documentation/Game%20Management/README.md#spawners-manager)

5) [Heads Up Display (HUD)](/Technical%20Documentation/HUD)
   - 5.1 [HUD](/Technical%20Documentation/HUD/README.md#miscellaneous)
   - 5.2 [Billboard](/Technical%20Documentation/HUD/README.md#billboard)
   - 5.3 [Crosshairs](/Technical%20Documentation/HUD/README.md#crosshairs)
   - 5.4 [Waypoint Marker](/Technical%20Documentation/HUD/README.md#waypoint-marker)

6) [Textures](https://cseegit.essex.ac.uk/ce301_2020/ce301_craig_jamal/-/tree/master/Unity%20CE301/Capstone%20Project/Assets/Models/Textures)
   - 6.1 [Textures](https://cseegit.essex.ac.uk/ce301_2020/ce301_craig_jamal/-/tree/master/Unity%20CE301/Capstone%20Project/Assets/Models/Textures#textures)

7) [Interactable Objects](/Technical%20Documentation/Interactable%20Objects)
   - 7.1 [Interactable Objects](/Technical%20Documentation/Interactable%20Objects/#interactable-objects)
   - 7.2 [Dunamis Gems](/Technical%20Documentation/Interactable%20Objects#dunamis-gems)
   - 7.3 [Dunamics Gem Spawners](/Technical%20Documentation/Interactable%20Objects#dunamis-gem-spawners)
   - 7.4 [Piped Gem Dispenser Stations](/Technical%20Documentation/Interactable%20Objects#piped-gem-dispenser-stations)
   - 7.5 [Health Orbs](/Technical%20Documentation/Interactable%20Objects#health-orbs)

8) [Packages and External Assets](/Technical%20Documentation/Packages)
   - 8.1 [Packages and External Assets](/Technical%20Documentation/Packages#packages-and-external-assets)
   - 8.2 [Google Carboard XR Plugin for Unity](/Technical%20Documentation/Packages#google-cardboard-xr-plugin-for-unity)
   - 8.3 [ProGrids](/Technical%20Documentation/Packages#progrids)
   - 8.4 [ProBuilder](/Technical%20Documentation/Packages#probuilder)
   - 8.5 [Snaps Prototype Packs](/Technical%20Documentation/Packages#snaps-prototype-packs)
   - 8.6 [Ring Lamp](/Technical%20Documentation/Packages#ring-lamp)
   - 8.7 [Crystal Lux Conte AP1 Chandelier](/Technical%20Documentation/Packages#crystal-lux-conte-ap1-chandelier)

9) Testing
   - 9.1 [A* Search Testing - Enemy to Player](/Technical%20Documentation/Testing/A*%20Test)
   - 9.2 [Black Box Testing](/Technical%20Documentation/Testing/Black%20Box%20Test)

