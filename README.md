TLMlang is a stack-based, two-dimensional programming language focused on meta programming.
Inspirations include Brainfuck, Befunge and cellular automata.

This repo is an interpreter for TLMlang writting in C#

# Specification
Below is the list of all key-"words" allowed in .tlm files:

| Instruction   | |
| - | - |
| . | non-instruction (does nothing) |
| U, D, L, R | Redirect flow of instruction pointer (up, down, left, right)  |
| 0, 1, ..., 9 | Pushes the value of the cell to the stack  |
| a, b, ..., z | Goes to function (a,b,...z) |
| P | pops the value on top of the stack and puts it in the current cell modulo 10 |
| T | pops the value on top of the stack and puts it in the current cell, interpreted as a captial letter, "modulo Z + 1" |
| J | pops the value on top of the stack and puts it in the current cell, interpreted as a letter, "modulo z + 1" |
| H | pops the value on top of the stack and puts it in the left and right neighbors |
| V | pops the value on top of the stack and puts it in the top and bottom neighbors |
| E | pops the two topmost vlaues of the stack (a, b) and pushes a+b |
| F | pops the two topmost values of the stack (a, b) and pushes a-b |
| C | pops the the value on top of the stack and inserts it at the bottom |
| S | pushes the sum of all eight neighbors to the stack after clearing the cells |
| M | pushes the product of all eight neighbors to the stack after clearing the cells |
| N | pushes the length of the stack to the stack |
| I | increments the value on top of the stack by 1 |
| X | decrements the value on top of the stack by 1 |
| O | redirects control flow to the right (or left) if the top value on the stack is zero (nonzero) |
| A | halts the program and waits for user input in the form of a capital letter, puts that letter into the cell |
| Z | halts the program and waits for user input in the form of a letter, puts that letter into the cell |
| Y | halts the program and waits for user input in the form of a number, puts that number into the cell |
| Q | prints the value on top of the stack |
| V | prints the value on top of the stack interpreted as a number (mod 10) |
| G | prints the value on top of the stack interpreted as a capital letter (mod Z+1) |
| K | prints the value on top of the stack interpreted as a letter (mod z+1) |
| B | duplicates the value on top of the stack and pushes it to the stack |
| W | swaps the two topmost values on top of the stack |


Alphabetical List:

| Instruction   | |
| - | - |
| A | halts the program and waits for user input in the form of a capital letter, puts that letter into the cell |
| B | duplicates the value on top of the stack and pushes it to the stack |
| C | pops the the value on top of the stack and inserts it at the bottom |
| D | Redirect control flow down |
| E | pops the two topmost vlaues of the stack (a, b) and pushes a+b |
| F | pops the two topmost values of the stack (a, b) and pushes a-b |
| G | prints the value on top of the stack interpreted as a capital letter (mod Z+1) |
| H | pops the value on top of the stack and puts it in the left and right neighbors |
| I | increments the value on top of the stack by 1 |
| J | pops the value on top of the stack and puts it in the current cell, interpreted as a letter, "modulo z + 1" |
| K | prints the value on top of the stack interpreted as a letter (mod z+1) |
| L | Redirect control flow left |
| M | pushes the product of all eight neighbors to the stack after clearing the cells |
| N | pushes the length of the stack to the stack |
| O | redirects control flow to the right (or left) if the top value on the stack is zero (nonzero) |
| P | pops the value on top of the stack and puts it in the current cell modulo 10 |
| Q | prints the value on top of the stack |
| R | Redirect control flow right |
| S | pushes the sum of all eight neighbors to the stack after clearing the cells |
| T | pops the value on top of the stack and puts it in the current cell, interpreted as a captial letter, "modulo Z + 1" |
| U | Redirect control flow up |
| V | prints the value on top of the stack interpreted as a number (mod 10) |
| W | swaps the two topmost values on top of the stack |
| X | decrements the value on top of the stack by 1 |
| Y | halts the program and waits for user input in the form of a number, puts that number into the cell |
| Z | halts the program and waits for user input in the form of a letter, puts that letter into the cell |
