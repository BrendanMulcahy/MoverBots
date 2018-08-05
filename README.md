# MoverBots
A small AI game based very loosely on the taxicab domain.  The goal is for the bot to push the ball into the hole in the ground without falling into the hole itself.

![Ray Casting](Images/Raycasting.PNG?raw=true "Ray Casting")

The agents sense the world through ray casting.  This image shows a debugging view:
Blue ray = sees ball
Green ray = sees wall
Red ray = floor
Grey ray (hard to see) = score zone 

## Requirements

Requires Unity 2018.2.2f1 and Unity ML-Agents v0.4 to be installed at Assets/Plugins.  I will work on finding a better solution to make this repo easier to collaborate on.
