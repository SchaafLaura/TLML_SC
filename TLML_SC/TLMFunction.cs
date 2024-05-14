namespace TLML_SC
{
    internal class TLMFunction
    {
        Point ptr = new Point(0, 0);
        char[,] instr;
        string name;

        public TLMFunction(string name, char[,] instr)
        {
            this.name = name;
            this.instr = instr;
        }

    }
}
