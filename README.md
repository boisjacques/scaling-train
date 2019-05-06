# The Chicken and the Fox

Name: Manuel Kieweg

Student Number: D18123315

## Description of the assignment
For this assignment

## Instructions

## How it works
### Scene 1
The first scene uses a path following behaviour coupled with a jitter wanderer for the bird. That way the bird moves in a natural motion. The camera movement is smoothed down by lerping and slerping the camera towards the fox. The forest is procedurally generated to give it a random and natural appearance. The rising sun is simulated by a light source rotating around the zero-vector. Particle effects are used to simulate fireflies.

### Scene 2
In this scene the fox uses a seek behaviour and a noise wanderer to reach its destination. The forest and the lighting are generated and controlled the same way they were in scene 1. Particle effects are used to simulate fireflies.

### Scene 3
In this scene botch the chickens and the fox are controlled by a state machine. The chickens' states are:
- Picking
- Fleeing
- Returning
The Fox's states are:
- Search Target
- Approach
- Attack
- Retreat
- Eat

#### Chicken Behaviour
The start in the *Picking* state. When the fox comes closer than their fleeing distance is, they enter the *Fleeing* state. In that state they turn away from the fox and increase their jumping force. This consumes more stamina, which leads to exhaustion after a certain distance. When the fox is out of a certain radius the chicken either enter the *Picking* state, or the *Returning* state if their distance to the centre of their picking area is above a certain threshold.

#### Fox Behaviour
The fox is searching a target out of the set of GameObjects tagged as *prey*. When it has a target, it changes into the *Approaching* state, approaching its target with normal speed. After the distance to its target fell below a certain threshold it changes to the *Attack* state increasing its speed. After is reached and caught the target it retrats to its foxhole in the *Retreat* state. After reaching the foxhole it changes into the *Eat* state. After a certain time it changes back to the inital *Search Target* state.

## What I am most proud of in the assignment
The thinks I like most about my assignment are the synchronisation of audio and video from an dramaturgical point of view. Technically the working adaption and integration of different kinds of behaviour and the way the Chicken are moving and behaving are done pretty good.

Also the procedural generation of the forest ind scene 1 and 2 really contributes to the overall mood and athmosprhere.

I think the overall light setting and mood could need a little improvement. Even though the fog and light setting resemble a dawning day pretty good it could be a little more dramatic and dark.