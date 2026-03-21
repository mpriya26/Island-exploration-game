
# Island Exploration Game

Assignment for videogames as art course.

This is our submission for the creative response assignment for our videogames as art course.

Our task was to "produce a creative work inspired by the artistic dimensions of any game of your choice."

The game we chose was [Sunless Sea](https://www.failbettergames.com/games/sunless-sea) by Failbetter Games. We enjoyed the lovecraftian, dark, and blind vibe, and wanted to try to capture it.

Our creative work is a videogame. We need to provide at least 5 minutes of playable experience and connect with course theory.

The game is set in an infinite, dark ocean with procedurally generated islands spread around randomly. The player controls a boat in this ocean, searching for something on the islands.

The player has a top down view of a boat, the ocean is very dark, and a light is used to see the islands. Many of these aspects are used in Sunless Sea.

Islands are randomly spread out and generated. This is similar to the island shuffling that happens between playthroughs of Sunless Sea.

## Goal

Capture the vibe of exploration in Sunless Sea, where the Zee islands are randomized, and the player doesn't know what's happening next.

## Features

### Ocean

The ocean is dark and infinite.

A thick fog lowers visibility.

Islands are spread out on the ocean using poisson disk sampling.

### Islands

Throughout the ocean, procedurally generated islands are spread out randomly.

The islands are a simple shaded coastline with collision.

Islands are generated through layered perlin noise with a threshold, the coast is filled in with marching squares.

### Boat

The boat has a light for viewing the islands.

The boat moves slowly, controlled with a throttle and helm in the UI.

The boat physics are semi-realistic.

If the boat crashes too much, it will sink.
