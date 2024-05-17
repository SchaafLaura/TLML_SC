using Microsoft.Xna.Framework.Input;

namespace TLML_SC
{
    internal class TLMProgram
    {
        public Stack<int> stack = new();
        public Dictionary<string, TLMFunction> functions;
        public Stack<TLMFunction> functionStack = new();

        public string? error = null;

        public bool done = false;
        public char? input = null;

        public TLMProgram(Dictionary<string, TLMFunction> functions)
        {
            this.functions = functions;
        }

        public void Startup()
        {
            functionStack.Push(functions["main"].Clone());
        }

        public void Input(SadConsole.Input.Keys key)
        {

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
                error = result.err;
                done = true;
                return;
            }

            var instErr = result.instruction!.Invoke(this);

            if(instErr is not null)
            {
                error = instErr;
                done = true;
                return;
            }

            return;
        }
    }
}
