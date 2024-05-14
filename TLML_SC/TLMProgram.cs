namespace TLML_SC
{
    internal class TLMProgram
    {
        Stack<int> stack = new();
        Dictionary<string, TLMFunction> functions = new();

        Stack<TLMFunction> functionStack = new();
    }
}
