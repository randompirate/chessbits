# chessbits
Solving the 8 queens problem (and related). Also adventures in bit twiddling.

## Idea
A chess board has 64 cells, a Long variable has 64 bits. Represent each piece at each position as a Long. Chess actions (like placing a piece or checking if a piece is under attack) can now be represented as bitwise operations.

Time/memory trade-off is achieved by saving lists of Long values representing every position for every piece.

There is some exploitation of symmetries.

Benchmarking against high-level approaches is glaringly absent...

## Keywords
bit twiddling, memory/time trade-off, 
