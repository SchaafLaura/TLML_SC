namespace TLML_SC
{
    internal class TLMProgram
    {
        public Stack<int> stack = new();
        public Dictionary<string, TLMFunction> functions;
        public Stack<TLMFunction> functionStack = new();

        public bool done = false;

        public TLMProgram(Dictionary<string, TLMFunction> functions)
        {
            this.functions = functions;
        }

        public void Startup()
        {
            functionStack.Push(functions["main"].Clone());
        }

        public void Step()
        {
            if (done)
                return;

            // get instruction from top fn
            var fn = functionStack.Peek();
            var result = fn.Execute();

            if(result.err is not null)
            {
                // print error, halt
                return;
            }

            var instErr = result.instruction!.Invoke(this);

            if(instErr is not null)
            {
                // print error, halt
                return;
            }

            return;
        }
    }
}
