﻿namespace TLML_SC
{
    internal class TLMFunction
    {
        // TODO: make setters and getters
        public Point ptr = new Point(-1, 0);
        public Point flw = new Point(1, 0);
        public char[,] instr;

        public string name;

        public delegate string? Instruction(TLMProgram program);

        public TLMFunction(string name, char[,] instr)
        {
            this.name = name;
            this.instr = instr;
        }

        public (string? err, Instruction? instruction) Execute()
        {
            var step = Step();
            if (step is not null)
                return (null, StepOut);

            var instruction = instr[ptr.X , ptr.Y];

            return instruction switch
            {
                'A' => (null, CapitalInput),
                'B' => (null, DuplicateAndPush),
                'C' => (null, PopAndInsert),
                'D' => (null, Redirect((0, 1))),
                'E' => (null, PopTwoAddPush),
                'F' => (null, PopTwoSubPush),
                'G' => (null, PopOutputCapital),
                'H' => (null, PopPutHorizontal),
                'I' => (null, Increment),
                'J' => (null, PopPutLowercase),
                'K' => (null, PopOutputLowercase),
                'L' => (null, Redirect((-1, 0))),
                'M' => (null, MultEightPush),
                'N' => (null, PushLength),
                'O' => (null, RedirectDependingOnTopStack),
                'P' => (null, PopPutAsNumber),
                'Q' => (null, PopOutput),
                'R' => (null, Redirect((1, 0))),
                'S' => (null, SumEightPush),
                'T' => (null, PopPutAsUppercase),
                'U' => (null, Redirect((0, -1))),
                'V' => (null, LowercaseInput),
                'W' => (null, Swap),
                'X' => (null, Decrement),
                'Y' => (null, NumberInput),
                'Z' => (null, RetrieveFromBottom),
                '.' => (null, (_) => null),
                >= '0' and <= '9' => (null, Push(instruction)),
                >= 'a' and <= 'z' => (null, StepInto(instruction)),
                _ => ("unknown symbol '" + instruction + "' encountered" , null)
            };
        }
        
        public void PutAsNumber(int i, Point pos)
        {
            if (pos.X < 0 || pos.Y < 0 || pos.X >= instr.GetLength(0) || pos.Y >= instr.GetLength(1))
                return;
            instr[pos.X, pos.Y] = (char)((i%10) + 48);
        }

        public void PutAsLowercase(int i, Point pos)
        {
            if (pos.X < 0 || pos.Y < 0 || pos.X >= instr.GetLength(0) || pos.Y >= instr.GetLength(1))
                return;
            instr[pos.X, pos.Y] = (char)((i%26) + 97);
        }

        public void PutAsUppercase(int i, Point pos)
        {
            if (pos.X < 0 || pos.Y < 0 || pos.X >= instr.GetLength(0) || pos.Y >= instr.GetLength(1))
                return;
            instr[pos.X, pos.Y] = (char)((i % 26) + 65);
        }

        public void Put(int i, Point pos)
        {
            if (pos.X < 0 || pos.Y < 0 || pos.X >= instr.GetLength(0) || pos.Y >= instr.GetLength(1))
                return;
            instr[pos.X, pos.Y] = (char) i;
        }

        public string? NumberInput(TLMProgram program)
        {
            program.inputHandler = (prgm, c) =>
            {
                if (c < 48 || c > 57)
                    return false;

                var fn = prgm.functionStack.Peek();
                fn.Put(c, fn.ptr);
                return true;
            };
            return null;
        }

        public string? LowercaseInput(TLMProgram program)
        {
            program.inputHandler = (prgm, c) =>
            {
                if (c < 65 || c > 90)
                    return false;

                var fn = prgm.functionStack.Peek();
                fn.Put(c.ToString().ToLower()[0], fn.ptr);
                return true;
            };
            return null;
        }

        public string? CapitalInput(TLMProgram program)
        {
            program.inputHandler = (prgm, c) =>
            {
                if (c < 65 || c > 90)
                    return false;

                var fn = prgm.functionStack.Peek();
                fn.Put(c, fn.ptr);
                return true;
            };
            return null;
        }

        public Instruction StepInto(char c)
        {
            return (program) =>
            {
                if (!program.functions.ContainsKey(c.ToString()))
                    return "can't step into function '" + c + "' because it doesn't exist";
                program.functionStack.Push(program.functions[c.ToString()].Clone());
                return null;
            };
        }

        public string? PopOutput(TLMProgram program)
        {
            if (program.stack.Count == 0)
                return "can't pop, stack too smol";
            var val = program.stack.RemoveFromFront();
            program.output += val.ToString();
            return null;
        }

        public string? PopOutputLowercase(TLMProgram program)
        {
            if (program.stack.Count == 0)
                return "can't pop, stack too smol";
            var val = program.stack.RemoveFromFront();
            val = (val % 26) + 97;
            program.output += (char)val;
            return null;
        }

        public string? PopOutputCapital(TLMProgram program)
        {
            if (program.stack.Count == 0)
                return "can't pop, stack too smol";
            var val = program.stack.RemoveFromFront();
            val = (val % 26) + 65;
            program.output += (char) val;
            return null;
        }

        public string? RetrieveFromBottom(TLMProgram program)
        {
            if (program.stack.Count == 0)
                return "can't retrieve, stack too smol";

            var val = program.stack.RemoveFromBack();
            program.stack.AddToFront(val);
            return null;
        }

        public string? Swap(TLMProgram program)
        {
            if (program.stack.Count < 2)
                return "can't swap, stack too smol";
            var a = program.stack.RemoveFromFront();
            var b = program.stack.RemoveFromFront();
            program.stack.AddToFront(a);
            program.stack.AddToFront(b);
            return null;
        }

        public string? PopPutAsUppercase(TLMProgram program)
        {
            if (program.stack.Count == 0)
                return "can't pop, stack empty";
            var val = program.stack.RemoveFromFront();
            var fn = program.functionStack.Peek();
            fn.PutAsUppercase(val, fn.ptr);
            return null;
        }

        public string? PopPutAsNumber(TLMProgram program)
        {
            if (program.stack.Count == 0)
                return "can't pop, stack empty";
            var val = program.stack.RemoveFromFront();
            var fn = program.functionStack.Peek();
            fn.PutAsNumber(val, fn.ptr);
            return null;
        }

        public string? RedirectDependingOnTopStack(TLMProgram program)
        {
            if (program.stack.Count == 0)
                return "can't redirect, stack empty";
            var fn = program.functionStack.Peek();

            var peek = program.stack.RemoveFromFront();
            program.stack.AddToFront(peek);

            fn.flw = peek != 0 ?
                flw switch
                {
                    (0, 1) => (-1, 0),
                    (1, 0) => (0, 1),
                    (0, -1) => (1, 0),
                    (-1, 0) => (0, -1),
                } :
                flw switch
                {
                    (0, 1) => (1, 0),
                    (1, 0) => (0, -1),
                    (0, -1) => (-1, 0),
                    (-1, 0) => (0, 1),
                };
            return null;
        }

        public string? PushLength(TLMProgram program)
        {
            program.stack.AddToFront(program.stack.Count);
            return null;
        }

        public string ? SumEightPush(TLMProgram program)
        {
            var fn = program.functionStack.Peek();
            var ptr = fn.ptr;
            var sum = 0;
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    if (!(ptr.X + i < 0 || ptr.Y + j < 0 ||
                        ptr.X + i >= instr.GetLength(0) || ptr.Y + j >= instr.GetLength(1) ||
                        fn.instr[ptr.X + i, ptr.Y + j] < '0' || fn.instr[ptr.X + i, ptr.Y + j] > '9'))
                    {
                        sum += int.Parse(fn.instr[ptr.X + i, ptr.Y + j].ToString());
                        fn.Put('.', (ptr.X + i, ptr.Y + j));
                    }
            program.stack.AddToFront(sum);
            return null;
        }

        public string? MultEightPush(TLMProgram program)
        {
            var fn = program.functionStack.Peek();
            var ptr = fn.ptr;
            var prod = 1;
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    if (!(ptr.X + i < 0 || ptr.Y + j < 0 ||
                        ptr.X + i >= instr.GetLength(0) || ptr.Y + j >= instr.GetLength(1) ||
                        fn.instr[ptr.X + i, ptr.Y + j] < '0' || fn.instr[ptr.X + i, ptr.Y + j] > '9'))
                    {
                        prod *= int.Parse(fn.instr[ptr.X + i, ptr.Y + j].ToString());
                        fn.Put('.', (ptr.X + i, ptr.Y + j));
                    }
            program.stack.AddToFront(prod);
            return null;
        }

        public string? PopPutLowercase(TLMProgram program)
        {
            if (program.stack.Count == 0)
                return "can't pop, stack empty";
            var val = program.stack.RemoveFromFront();
            program.functionStack.Peek().PutAsLowercase(val, ptr);
            return null;
        }

        public string? Decrement(TLMProgram program)
        {
            if (program.stack.Count == 0)
                return "can't increment, stack empty";
            var val = program.stack.RemoveFromFront();
            program.stack.AddToFront(val - 1);
            return null;
        }

        public string? Increment(TLMProgram program)
        {
            if (program.stack.Count == 0)
                return "can't increment, stack empty";
            var val = program.stack.RemoveFromFront();
            program.stack.AddToFront(val + 1);
            return null;
        }

        public string? PopPutHorizontal(TLMProgram program)
        {
            if (program.stack.Count == 0)
                return "can't pop, stack empty";
            var val = program.stack.RemoveFromFront();
            var fn = program.functionStack.Peek();
            fn.PutAsNumber(val, ptr + (1, 0));
            fn.PutAsNumber(val, ptr - (1, 0));
            return null;
        }

        public string? PopTwoSubPush(TLMProgram program)
        {
            if (program.stack.Count < 2)
                return "Can't subtract two, stack too smol";
            var val1 = program.stack.RemoveFromFront();
            var val2 = program.stack.RemoveFromFront();
            program.stack.AddToFront(val1 - val2);
            return null;
        }

        public string? PopTwoAddPush(TLMProgram program)
        {
            if (program.stack.Count < 2)
                return "Can't add two, stack too smol";
            var val1 = program.stack.RemoveFromFront();
            var val2 = program.stack.RemoveFromFront();
            program.stack.AddToFront(val1 + val2);
            return null;
        }

        public Instruction Push(char c)
        {
            return (program) =>
            {
                program.stack.AddToFront(int.Parse(c.ToString()));
                return null;
            };
        }

        public Instruction Redirect(Point newFlow)
        {
            return (program) =>
            {
                program.functionStack.Peek().flw = newFlow;
                return null;
            };
        }

        public string? PopAndInsert(TLMProgram program)
        {
            if (program.stack.Count == 0)
                return "Can't pop, stack empty";
            var val = program.stack.RemoveFromFront();
            program.stack.AddToBack(val);
            return null;
        }

        public string? DuplicateAndPush(TLMProgram program)
        {
            if (program.stack.Count == 0)
                return "Can't duplicate, stack empty";
            var val = program.stack.RemoveFromFront();
            program.stack.AddToFront(val);
            program.stack.AddToFront(val);
            return null;
        }

        public TLMFunction Clone()
        {
            var newInstr = new char[instr.GetLength(0), instr.GetLength(1)];
            for(int i = 0; i < instr.GetLength(0); i++)
                for(int j = 0; j <  instr.GetLength(1); j++)
                    newInstr[i, j] = instr[i, j];
            return new TLMFunction(this.name, newInstr);
        }

        public string? StepOut(TLMProgram program)
        {
            program.functionStack.Pop();
            if (program.functionStack.Count == 0)
                program.done = true;
            return null;
        }

        public string? Step()
        {
            ptr += flw;

            if (ptr.X < 0)
                return "outside bounds left";
            else if (ptr.Y < 0)
                return "outside bounds top";
            else if (ptr.X >= instr.GetLength(0))
                return "outside bounds right";
            else if (ptr.Y >= instr.GetLength(1))
                return "outside bounds bottom";
            return null;
        }

    }
}
