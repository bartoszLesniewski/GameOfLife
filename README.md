# GameOfLife
Implementation of Conway's "Game of Life" written in C# with WPF. It is a zero-player game, meaning that its evolution is determined by its initial state, requiring no further input. One interacts with the Game of Life by creating an initial configuration and observing how it evolves.

## Rules
The universe of the Game of Life is an infinite, two-dimensional orthogonal grid of square cells, each of which is in one of two possible states: live or dead. Every cell interacts with its eight neighbors, which are the cells that are horizontally, vertically, or diagonally adjacent. At each step in time, the following transitions occur:
1. Any live cell with fewer than two live neighbors dies, as if by underpopulation.
2. Any live cell with two or three live neighbors lives on to the next generation.
3. Any live cell with more than three live neighbors dies, as if by overpopulation.
4. Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.

The initial pattern constitutes the seed of the system. The first generation is created by applying the above rules simultaneously to every cell in the seed. The rules continue to be applied repeatedly to create further generations.

## Application description
The application was implemented in WPF (Windows Presentation Foundation) and shows its typical concepts such as: styles, templates, triggers, animations. It allows to set the size of the board, edit the state, save the state to and read from the file, perform a single step, and continuously animate the state of the machine.
It is also possible to rewind frames and observe statistics on the number of generations/how many cells died/how many were born. The user can also enable the highlighting of born and dying cells and save an image or sequence of images showing the state. Before starting the game, it is possible to select the initial shape from the available options and configure the rules of the machine, such as the required number of cells for the birth/death of a new cell.
