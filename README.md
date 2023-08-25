# Coordinated Group movement 
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







