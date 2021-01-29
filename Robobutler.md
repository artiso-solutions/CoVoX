# Robobutler

## Overview
Robobutler is a robot capable of executing voice triggered actions based on its perception of the current environment. The idea is that an operator can tell the robot to "Bring me the yellow box" and the robot will in this case do the following:
1. Confirm/Repeat the task the robot was told to do
2. Go to the yellow box
3. Pick it up
4. Bring it to the operator

## Other possible scenarios
- Placing a box on top of another
- Basic movements (Stop, rotate, etc)
- Spatial awarness (e.g. go to the nearest corner)

## Robo to use
https://www.dji.com/de/robomaster-s1

The desired configuration would be an industrial arm on top of a body with wheels to represent a valid scenario for the industry.

## Technologies
- CoVoX engine
- [Azure computer vision](https://azure.microsoft.com/en-us/services/cognitive-services/computer-vision/)
- Python (to control the robot)