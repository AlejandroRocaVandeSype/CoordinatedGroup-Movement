# Coordinated Group Movement
## Introduction
Simple project that implements the basics of Coordinated Group Movement. 

Units are grouped together in formations, allowing us to move a group of characters in a cohesive way, as if they all would be moving together as a group or squad.

This technique is used in many real-time strategy videogames, and I have created a simple example that incorporates some basics concepts from these genres. 

It is entirely made in Unity -2022.3.6f1-, and my main sources are "Artificial Intelligence For Games" and "AI Game Programming Wisdom 4" books.

If you want to try this project, feel free to download and test it out.
## Result

<img src="https://github.com/AlejandroRocaVandeSype/CoordinatedGroup-Movement/assets/31854308/72a953cc-3160-490a-9394-065271d97b37" width="1000">

## Implementation
Many different techniques can be used to implement a Coordinated Group Movement system. For this project, I decided to implement the simplest type of formation movement. But before starting to create our formations, there are some basic concepts that we first need to understand.
### Basics
- Formation : Set of slots which represent the positions where our units are going to be placed.
- Leader's slot : Is the one who determines where the other slots will be placed in the formation. Acting as the centre of the formation.
- Group slots : These slots will be defined relative to the leader's position. They will move and orientate based on the leader's position and orientation in the world.
### First Steps
The first thing I’m going to do is to implement the basic concepts from a RTS videogame : Camera movement and Unit Selection. I will provide a way to move the camera by using the keyboard as well as the possibility to choose the units by making use of the mouse controller.

Simple selection of the units is made by just using the left click of the mouse. If you also keep the "SHIFT" key pressed, you will be able to select multiple units instead of just one.

<img src="https://github.com/AlejandroRocaVandeSype/CoordinatedGroup-Movement/assets/31854308/12923616-4bf7-4cd7-97b9-b2af27cdf3a1" width="600">


Several units can also be selected by just clicking and dragging with the left mouse button. This draws a blue transparent rectangle on top of the scene that will select all units that are located "inside" the rectangle dimensions. 

<img src="https://github.com/AlejandroRocaVandeSype/CoordinatedGroup-Movement/assets/31854308/836036d6-4b3b-402a-a34b-803d44b82c25" width="600">

### Units Movement
Next thing is the units movement. Units will move wherever the player click in the world. To do this, I used the raytracing system that Unity provides to raycast a ray from the player's mouse position to the ground meshes. When the ray hits the ground, the coordinates on where the mouse was clicked are sent to the units. Units however, won't move to this position until they have been selected by the player.

<img src="https://github.com/AlejandroRocaVandeSype/CoordinatedGroup-Movement/assets/31854308/7554caea-9ee3-4047-adb1-9beb6f4761b1" width="600">

The movement is made by using the Navigation System from Unity. When the destination target has been defined, which in our case is when the player clicks in a valid position in the world, a path is calculated to this position. After being calculated, the units move towards the target. 

However, as you can see, they all move to the same position pushing each other trying to reach the target. This is not what we want, and in order to achieve our objective we need to start by creating a formation.

### Creating a Formation
To achieve our objective of moving our units as cohesive group, the first thing that we should do is to create our group. The most important part here is our leader unit, which is the one who will determine where does the formation has to go and in which direction must rotate. The leader is the only one who will use a navigation system to reach its target and avoid obstacles during the way. The rest of the units from the formation will be placed relative to the leader's position. For this first test, I placed these relative positions manually, trying to recreate a "wall" formation made of five units. However, later I will implement this in a more scalable way where any amount of units can be added to the formation.

The first step is to **select a leader**. There are different ways to do this : 
- Random leader : Randomly select a unit from the formation. When the formation is created, select one of the units to be the leader. The rest of the units will be placed relative to this position. Since the type of formation we are trying to recreate is a wall, we will place the "normal" units at the right side of our leader, leaving a small distance between each one of them. This works fine and, when we create our formation, it gives us what we want. However, when we move our formation towards a target, it will look a little bit strange since our leader is at the left side of the formation, and he is the one who will try to be positioned at the desired place.
- Center leader : To make our formation movement actually look good when it is moving towards our target, is better to use a leader that is placed at the center of the formation. To achieve this, we should first calculate the center of our formation, and this will vary depending on the number of units that our formation has. After calculating the center, we can just select the closest unit to this center position to be the leader of the formation.
  
  ```
  // Calculate the center of our formation.
  Vector3 CalculateCenter()
  {
    Vector3 center = new Vector3();
    foreach (UnitCharacter unit in _unitsInFormation)
    {
        center += unit.transform.position;
    }
    center /= _unitsInFormation.Count;

    return center;
  }
  ```
- Virtual leader : The last option, and the one I decided to go for, is to not use a physical unit as a leader, but instead use a virtual leader. This virtual leader will be placed where the leader unit should be located, at the center of the formation, and then we create all the relative slot positions where our units should be positioned in the formation. This way, each unit of the formation will be assigned to a slot, and the leader will follow it as long as he is part of the formation.
<img src="https://github.com/AlejandroRocaVandeSype/CoordinatedGroup-Movement/assets/31854308/e2ff272a-b191-4609-82a3-b6d247471ff2" width="600">

### Moving our formation
Now it’s time to make our coordinated movement. Since we are trying to recreate a "wall" formation, we cannot simply make our leader move to the target. This won't give us a smooth movement since the virtual leader will directly move towards the direction of the target and, at the same time, rotate towards it. What we actually want, is to make our leader move towards the direction that he is currently facing, allowing us to have a smoother rotation for our entire formation.

To achieve this, we cannot directly rely on the Navigation System from Unity to move our leader because it automatically rotates our leader towards the target. Therefore, we will only use part of the System to help us to get the resul we want.
1.	Calculate the path to the target: We still use the navigation system from unity, but now we only use it to calculate our path to the target and we don't use it to move our leader towards it. Instead, we only care about where and in which direction the target is. When Unity calculates the path, internally stores a vector of points called "corners". These "corners" are the positions where the path changes significantly its direction in order to avoid obstacles that might encounter for example, or just because it needs to make a big rotation. Therefore, we can use these positions to retrieve the direction in which our leader needs to look at every moment, instead of just using the direction in which the final position is.

  ```
    // The next corner in the path will determine in which direction it has to go
    // End position - start position = vector towards end position 
    _target.direction = (_pathToTarget.corners[nextCorner] - _currentPos).normalized;
  ```
2. Rotate: Now that we have the direction in which the leader needs to go, we just need to make him rotate towards this direction. For this we can use the Cross product to obtain a vector that will be orthogonal to two vectors. These two vectors will be the target direction and the current forward direction from our leader unit that needs to move. This way, we can determine the rotation axis that the leader needs to rotate around towards our target, which will be the next position in the navigation path.By just multiplying the resulting vector by a value, this will define how fast our object will rotate.
   
4. Move : Finally, we just need to make our leader to move in the direction that he is facing, since he will be already facing the correct direction because we calculated in the previous step, this will make him move in the correct direction.

<img src="https://github.com/AlejandroRocaVandeSype/CoordinatedGroup-Movement/assets/31854308/9e6c41f2-6d97-4694-97b7-11be055c9658" width="600">

As you can see, this gives us a smooth rotation for our entire formation. Nonetheless, there is still a significant problem: When the formation rotates, the units that are positioned on the opposite side of the rotation are struggling to catch up with the formation. This is because all of our units have the same movement speed, and some units need to travel a longer distance depending on where they are placed and in which direction the formation is rotating.

To solve this, we need to change the speed of our units based on their distance to their corresponding positions in the formation. This way, further units will move faster, and closer units will move slower. Allowing the formation to always give a sensation of a cohesive group while it is moving.

<img src="https://github.com/AlejandroRocaVandeSype/CoordinatedGroup-Movement/assets/31854308/3dcd8d76-05ac-45f2-bbd6-0ef17023f573" width="600">

### Scalable formations
The last thing to do is to be able to create different size of formations based on the number of units that we are grouping. This can be done by just taking the center position of our formation, and defining the "start" position of the formation at the left side of the center. From here, based on the number of units we want to group and the number of rows and units per row that we want to have, we create the slots for the formation at the right position.

<img src="https://github.com/AlejandroRocaVandeSype/CoordinatedGroup-Movement/assets/31854308/a4f1678a-c301-41ed-a6eb-3139351e2b9c" width="600">

## Reference Materials
### Books
**Artificial Intelligence** For Games by Ian Millington and John David Funge.

**AI Game Programming Wisdom 4** by Steve Rabin
### Links
https://www.gamedeveloper.com/programming/implementing-coordinated-movement

https://mfhooley.wordpress.com/2016/12/11/coordinated-unit-movement/

https://albmarvil.wordpress.com/2017/11/04/coordinated-group-movement/


