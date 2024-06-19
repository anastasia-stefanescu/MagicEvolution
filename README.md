# Magic Evolution

**LINK DEMO:** https://youtu.be/zK_ddzEXcII 

A game written using **C#** in **Godot** that creates a simulation that **can be observed, but not modified**, of **a world inhabited by Wizbits** - small magic creatures that are supposed to evolve by reproducing themselves.

The purpose is to observe **their evolution accross multiple generations**, how the Wizbits that are better adapted to their surroundings survive for longer and reproduce more, leading to better offspring.

The Wizbits can move freely around, **need to consume Mana pellets** to sustain themselves, **reproduce**, and **attack each other using spells**. 

Their **behaviour** is dictated by **an AI - a neural network created uniquely for each Wizbit**, which can also **evolve** when new generations are created.

## User Stories 
The link to Trello can be found here : https://trello.com/b/Xt5yt4hK/magic-evolution
<img width="1221" alt="Screenshot 2024-06-16 at 20 21 28" src="https://github.com/grig95/MagicEvolution/assets/119175350/cbf912ba-3145-4f35-ba8f-f7059cdb5fd5">
1. I want to be able to set certain parameters of the simulation
2. I want to be able to see the Wizbits' characteristics
3. I want to be able to see interactions between Wizbits and the environment
4. I want to see competition between Wizbits (spells)
5. I want to see the Wizbits creating new generations
6. I want to see some kind of stability in the Wizbit population
7. I want to notice diversity in the environment
8. I want the Wizbits' features to be evolvable
9. I want the Wizbits' abilities to be evolvable
10. I want to see general data about the simulation in real-time

### Backlog
The tasks we planned were:
1. Creating the Wizbit and defining an outline for its behaviours
2. Creating a world with diverse types of environments
3. Designing the AI that dictates the Wizbit's actions
4. Having the Wizbit's abilities and characteristics, as well as its AI, be evolvable by creating genomes that can be mutated
5. Creating a User Interface - a main menu as well as windows that display information about the simulation during the game

## Diagrams
The UML Diagrams can be found here: https://github.com/grig95/MagicEvolution/tree/master/project_structure
![ai (4) drawio](https://github.com/grig95/MagicEvolution/assets/119175350/566da6fd-89b0-4616-b2c8-101df0e9dcc1)

![dependinte drawio](https://github.com/grig95/MagicEvolution/assets/119175350/4e38bb74-1c33-478f-8b38-86053a6428f0)


## Source Control
 - Branches: https://github.com/grig95/MagicEvolution/branches
 - Commits: https://github.com/grig95/MagicEvolution/commits/master/
 - Merge(s) : 
 - Pull requests: https://github.com/grig95/MagicEvolution/pulls?q=is%3Apr+is%3Aclosed

## Comments
Those can be found in the scripts files, namely in the folders for:

 - Wizbit (https://github.com/grig95/MagicEvolution/blob/WizbittAdaptation/scripts/Wizbit/Wizbit.cs)
 - NeuralNetwork ((https://github.com/grig95/MagicEvolution/tree/WizbittAdaptation/scripts/AI/NeuralNetwork))
 - WorldGenerator (https://github.com/grig95/MagicEvolution/blob/master/scripts/WorldGeneration/WorldGenerator.cs)

## Design Patterns
We've used a **Factory** Design Pattern to create the Genomes:

https://github.com/grig95/MagicEvolution/blob/master/scripts/GenomeFactory.cs

## Unit testing
We've used the **NUnit Framework** to do the Automated Testing, the code can be found in this folder:

And the test output here: 

Here are the outputs as well to visualise easier:
<img width="671" alt="Screenshot 2024-06-16 at 20 00 49" src="https://github.com/grig95/MagicEvolution/assets/119175350/bfcf445c-5c88-4958-b095-6a610abcd322">
![image](https://github.com/grig95/MagicEvolution/assets/119175350/3097c678-e059-4ffd-8185-48353bb6aebd)


## Using AI
We used AI to help us do the Unit Testing, we asked ChatGPT:
 - how to set up the framework for testing in Godot C#, and the result can be found here:
   
   https://github.com/grig95/MagicEvolution/blob/WizbittAdaptation/message.txt
   
- how to set up a TestRunner class for running tests in Godot:
  
![gpt](https://github.com/grig95/MagicEvolution/assets/119175350/f576a4f5-4d96-40e9-a963-1c4934ad742a)

And of course, for various other little things :)

## Bugfixes
### Wizbit Stats Menu
In our project, we have a menu for wizbit stats that prompts up whenever we press a wizbit. Sounds good, right?
Except when the window just appears, isn't updated and can't be closed, like here:

![image](https://github.com/grig95/MagicEvolution/assets/57030918/5455a88e-06a8-4b19-97da-46697b647da0)

Issue:
Wrong approach for the menu design.

Solution:
Decided instead of having a hidden menu always present that we update with the right data, we shall have one that can be closed and is instantiated whenever we click a wizbit with his data.

![image](https://github.com/grig95/MagicEvolution/assets/57030918/ca67a3b0-4377-4dcb-8a75-85e190a8373b)

finalgit add commit:
https://github.com/grig95/MagicEvolution/commit/27918cbf06108d1a8d52bc6f218a8bef84817895

### Suicidal Wizbits

Wizbit use raycasting for vision and aiming spells. Sounds good enough, right? Right, until they start targeting their most powerful enemy - themselves.

We found this out when we saw that some wizbits, instead of being killed by other wizbits, simply die from a spell.
In this screenshot, we can see the difference between a wizbit killed by a someone else's spell and killed by its own spell - as he is not attacked.

![image](https://github.com/grig95/MagicEvolution/assets/57030918/1b369b65-d7b2-4236-8900-584e2f464c03)

Fixing commit:
https://github.com/grig95/MagicEvolution/commit/2ea72ec5d1bfb5d33caf057711af872eadf9a125#diff-ea53cdbcbf91e3c4a5ac9c3cd9b72a61d206773fbe14b9720486607015c30fe6

### Small bug, Huge Mana Sprites
After merging branches, we found some irregularities with the mana bottle size:

![image](https://github.com/grig95/MagicEvolution/assets/57030918/2769fded-9f4d-40d0-9831-303f291af28a)

Compared to a wizbit:

![image](https://github.com/grig95/MagicEvolution/assets/57030918/9663dc64-2015-4973-9a94-54d036874ced)

So we played a bit with its scaling and size, and fixed it in our commit called Bug Fix(Mana).

![image](https://github.com/grig95/MagicEvolution/assets/57030918/69d7c2ac-731b-423d-936e-6ef0948c627f)
