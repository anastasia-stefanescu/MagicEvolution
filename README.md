# Magic Evolution

A game written using **C#** in **Godot** that creates a simulation that **can be observed, but not modified**, of **a world inhabited by Wizbits** - small magic creatures that are supposed to evolve by reproducing themselves.

The purpose is to observe **their evolution accross multiple generations**, how the Wizbits that are better adapted to their surroundings survive for longer and reproduce more, leading to better offspring.

The Wizbits can move freely around, **need to consume Mana pellets** to sustain themselves, **reproduce**, and **attack each other using spells**. 

Their **behaviour** is dictated by **an AI - a neural network created uniquely for each Wizbit**, which can also **evolve** when new generations are created.

## User Stories 
The link to Trello can be found here : https://trello.com/b/Xt5yt4hK/magic-evolution

## How to play the game


## Diagrams
The UML Diagrams can be found here: https://github.com/grig95/MagicEvolution/tree/master/project_structure
![ai drawio (1)](https://github.com/grig95/MagicEvolution/assets/119175350/d2ff6c55-b1b4-4cf8-86a8-0e8afc0d548f)
![dependinte drawio](https://github.com/grig95/MagicEvolution/assets/119175350/4e38bb74-1c33-478f-8b38-86053a6428f0)


## Source Control
 - Branches: https://github.com/grig95/MagicEvolution/branches
 - Commits: https://github.com/grig95/MagicEvolution/commits/master/
 - Merge(s) : 
 - Pull requests: https://github.com/grig95/MagicEvolution/pulls?q=is%3Apr+is%3Aclosed

## Comments
Those can be found in the scripts files, namely in the Wizbit and AI folders

## Design Patterns
We've used a **Factory** Design Pattern to create the Genomes:

https://github.com/grig95/MagicEvolution/blob/master/scripts/GenomeFactory.cs

We've also used a **Singleton** class here: 

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

