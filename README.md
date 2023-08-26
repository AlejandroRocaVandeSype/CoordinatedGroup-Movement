## Introduction
This project implements the basics of Coordinated Group Movement where units are group together in formations. This allows us to move a group of characters in a cohesive way, as if they all will be moving together as a group or squad. 

This technique is used in many real-time strategy videogames and I have created a simple example that incorporates some basics concepts from these genre. Concepts such as the camera's point of view, the selection of units and the movement of these units.

This small research project was entirely made in Unity and my main sources are "_Artifical Inteligence For Games_" and "_AI Game Programming Wisdom 4_". 

If you want to try this project, there is a folder called "Demo" where the executable from this project is located. Feel free to download and test it out.

## Result
## Implementation
There are different tecniques that can be used to implement a Coordinated Group Movement. For this project, I will implement the simplest type of formation movement. In order to create our formations there are some basic concepts that we need to know.
### Basics
A formation is just a set of slots which represent the positions where our units will be placed in the group. One of these slots will be marked as the **leader's slot** and it will the one which determines where the other slots will be placed in the formation. The **group slots** will be defined relative to this leader position and they will move and orientate based on the leader's position and orientation in the world.
### First Steps
The first thing Im going to do is to implement the basic concepts from a RTS videogame. So I will provide a way to move the camera using the keyboard as well as the possibility to choose the units by making use of the mouse controller.

Simple selection of the units is made by just using the left click of the mouse. If you keep the "SHIFT" button pressed while you do this, you will be able to select multiple untis at the same time.

<img src="https://github.com/AlejandroRocaVandeSype/CoordinatedGroup-Movement/assets/31854308/12923616-4bf7-4cd7-97b9-b2af27cdf3a1" width="600">


Multiple units can also be selected by just clicking with the left mouse button and dragging it. This will draw a blue transparent rectangle on top of the scene that will select all units that are "inside" the rectangle dimensions. 

<img src="https://github.com/AlejandroRocaVandeSype/CoordinatedGroup-Movement/assets/31854308/836036d6-4b3b-402a-a34b-803d44b82c25" width="600">

### Units Movement
Next thing to implement is the movement of our units. Units will move wherever the player click in the world. To do this, I used the raytracing system that Unity provides to raycast a ray from the player's mouse position to the ground. When the ray hit the ground, the coordinates are saved and send to the units. However, units won't move to this position until they have been selected by the player.

<img src="https://github.com/AlejandroRocaVandeSype/CoordinatedGroup-Movement/assets/31854308/7554caea-9ee3-4047-adb1-9beb6f4761b1" width="600">

The movement is made by using the Navigation System from Unity. When the target on where to move has been defined, in our case, when the player clicks in a valid position in the world, a path is calculated to this position. After being calculated, the units move towards the target. However, as you can see, they all move to the same position pushing each other trying to reach the target. This is not what we want, and ir order to achieve our objective we need to start by creating a formation.

### Creating a Formation
In order to achieve our objective of moving our units as cohesive group, the first thing that we should do is to create our group. The most important part here is our leader unit, which is the one who will determine where does the formation has to go and in which direction has to rotate. The leader is the only one who will use a navigation system to reach its target and avoid obstacles during the way. The rest of the units from the formation will be placed relative to the leader's position. For this first test, I placed these relative positions manually, trying to recreate a "wall" formation made of 5 units. However, later we will try to do this in a scalable way.

Our first step is to **select our leader**. There are different ways to do this : 
- Random leader -> Randomly select a unit from the formation. When the formation is created, select one of the units to be the leader. The rest of the units will be placed relative to this position. Since the type of formation we are trying to recreate is a wall, we will place the normal units at the right side of our leader, leaving a small space between each one of them. This works fine and, when we create our formation, it gives us what we want. However, when we move our formation towards a target, it will look a little bit strange since our leader is at the left side of the formation and he is the one who will try to be positioned at the desired place.
- Center leader -> To make our formation look good when it is moving towards our target, is better to use a leader that is placed at the center of the formation. To achieve this, we should first calculate the center of our formation and this will vary depending on the amount of units that our formation has. After calculating the center, we can just select the closest unit to this center position to be the leader of the formation. 
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
- Virtual leader -> The last option, and the one I decided to go for, is to not use a unit as a leader, but instead use a virtual leader. This virtual leader will be placed where the leader unit should be, at the center of the formation, and then we create all the relative slot positions where our units should be positioned in the formation. This way, each unit of the formation will be assigned to a slot and he will follow it as long as he is part of the formation.
<img src="https://github.com/AlejandroRocaVandeSype/CoordinatedGroup-Movement/assets/31854308/e2ff272a-b191-4609-82a3-b6d247471ff2" width="600">

### Moving our formation
Now its time to make our coordinated movement. Since we are trying to recreate a "wall" formation, we cannot simply make our leader move to the target. This won't give us a smooth movement since the virtual leader will directly move towards the direction of target and, at the same time, rotate towards it. What we want instead, is to make our leader move towards the direction that he is currently facing, allowing us to have a more smooth rotation for our entire formation.

In order to achieve this, we cannot directly rely on the Navigation System from Unity to move our leader because it automatically rotates our leader towards the target. Therefore, we will follow these steps to make our formation movement :

1. Calculate the path to to the target : We still use the navigation system from unity, but now we only use it to calculate our path to the target and we don't use it to move our leader towards it. Instead, we just want to know where is the target and in which direction we need to move to reach it. When Unity calculates the path, it stores a vector of points called "corners". These "corners" are the positions where the path changes significantly in direction to, for exammple, avoid obstacles that it might encounter or just because it needs to make a big rotation. Therefore, we can use these positions to retrieve in which direction is our leader looking at every moment, instead of just using the direction in which the final position is.
  ```
    // The next corner in the path will determine in which direction it has to go
    // End position - start position = vector towards end position 
    _target.direction = (_pathToTarget.corners[1] - _currentPos).normalized;
  ```
2. Rotate and move : Now that we have the direction in which the leader needs to go, we just need to make him rotate towards this direction. To do this we can use the Cross product to obtain a vector that will be perpendicular to two vectors. By using as input the target direction and a vector that represents the forward direction the object needs to move, we can determine the rotation axis that the leader needs to rotate around to face our target, which will be the next position in the path. If just multiply the resulting vector by a value, it will determine how fast our object will rotate.
The last thing to do will be to just move our leader forward, since he will be already facing the correct direction because we calculated in the previous step, this will make him move in the correct direction.

<img src="https://github.com/AlejandroRocaVandeSype/CoordinatedGroup-Movement/assets/31854308/9e6c41f2-6d97-4694-97b7-11be055c9658" width="600">

As you can see, this give us a smooth rotation of our entire formation. Nonetheless, there is a significant problem. When we make our formation to rotate, the units positioned on the opposite side of the rotation are struggling to catch up with the formation. This is because all units have the same movement speed and some units need to travel a longer distance depending on where are they placed and in which direction the formation is rotation.

To solve this we need to change the speed of our units based on the distance to their corresponding positions in the formation. This way, further units will move faster and the closer units will move slower. Allowing the formation to always give a sensation of a cohesive group while it is moving. 

<img src="https://github.com/AlejandroRocaVandeSype/CoordinatedGroup-Movement/assets/31854308/3dcd8d76-05ac-45f2-bbd6-0ef17023f573" width="600">

### Scalable formations
The last thing to do is to be able to create different size of formations based on the number of units that we are grouping. To do this we just take the center position of our formation and we define the start position at the left side of this position. From here, based on the amount of units we want to group and the number of rows and units per row that we want to have, we create the slots for the formation at the right position.



