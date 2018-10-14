# MoverBots
A small AI game based very loosely on the taxicab domain.  The goal is for the bot to push the ball into the hole in the ground without falling into the hole itself.

![Ray Casting](Images/Raycasting.PNG?raw=true "Ray Casting")

The agents sense the world through ray casting.  This image shows a debugging view:
 - Blue ray = sees ball
 - Green ray = sees wall
 - Red ray = floor
 - Grey ray (hard to see) = score zone 

## Requirements

Requires Unity 2018.2.12f1.  TensorflowSharp and other dependencies are included as BLOBs.

If you would like to do training, you will need to:
 1. Clone https://github.com/Unity-Technologies/ml-agents
 1. Follow the python installation guide https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Installation.md#install-python-and-mlagents-package
 1. Build MoverBots.exe using Brain set to external
 1. Run `mlagents-learn config\trainer_config.yaml --env=MoverBots\MoverBots.exe --run-id=MoverBots0 --train` with the appropriate paths
