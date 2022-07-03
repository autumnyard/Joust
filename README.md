# Joust

A small game I designed on 2022/07/03 as a simple and fast card game.

## Introduction

Join the **joust** and fight face to face with your opponent. But don't rely only on your brute strength! Use your cunning and mix different strategies: You can **attack**, **defend** or **parry**. Either if you have the **initiative** or you're second to respond, you can always have an advantage.

## Rules

- There are two players, and the game consists of 5 rounds of 3 bouts.
- Each round has two phases:
	+ First, each player fills the 3 actions that will be performed. These will be hidden.
	+ Starting with the first bout, keep solving the bouts until all 3 are done. That finishes the round.
- A bout consists on a face-to-face between a player with **initiative** and the other who responds.
	+ The result of a bout will be points added to one of the two players, and perhaps the **initiative** switching to the other player.
	
### Table of bouts

| Initiative | Second | Points | Change initiative?
| --- | --- | --- | --- |
| Att | Att | 1,1 | Yes |
| Att | Par | 0,2 | Yes |
| Att | Def | 0,1 | No |
| Par | Att | 2,0 | No |
| Par | Par | 0,0 | No |
| Par | Def | 0,0 | Yes |
| Def | Att | 1,0 | Yes |
| Def | Par | 0,0 | No |
| Def | Def | 0,0 | Yes |

