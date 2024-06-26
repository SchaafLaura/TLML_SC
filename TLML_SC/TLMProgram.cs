﻿using Microsoft.Xna.Framework.Input;
using Nito.Collections;

namespace TLML_SC
{
    internal class TLMProgram
    {
        public delegate bool InputHandler(TLMProgram program, char input);

        public Deque<int> stack = new();
        //public Stack<int> stack = new();
        public Dictionary<string, TLMFunction> functions;
        public Stack<TLMFunction> functionStack = new();
        public string? error = null;
        public bool done = false;
        public string output = "";

        public InputHandler? inputHandler = null;

        public int stepsTaken = -1;

        public TLMProgram(Dictionary<string, TLMFunction> functions)
        {
            this.functions = functions;
        }

        public void Startup()
        {
            functionStack.Push(functions["main"].Clone());
        }

        public void Input(char c)
        {
            if (inputHandler is null)
                return;

            if (!inputHandler.Invoke(this, c))
                return;

            inputHandler = null;
        }

        public void Step()
        {
            if (done)
                return;

            if (inputHandler is not null)
                return;

            stepsTaken++;

            var fn = functionStack.Peek();
            var result = fn.Execute();

            if(result.err is not null)
            {
                error = result.err;
                done = true;
                return;
            }

            var instErr = result.instruction!.Invoke(this);

            if(instErr is not null)
            {
                error = instErr +
                    " [in function \"" + fn.name +
                    "\" at " + fn.ptr.ToString() +
                    " instruction: " + fn.instr[fn.ptr.X, fn.ptr.Y] +
                    "]";
                done = true;
                return;
            }

            return;
        }
    }
}
