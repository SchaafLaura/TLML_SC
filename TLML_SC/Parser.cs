namespace TLML_SC
{
    internal static class Parser
    {
        public static Dictionary<string, TLMFunction> ParseLines(List<string> lines)
        {
            Dictionary<string, List<string>> functionLines = new();
            for(int i = 0; i < lines.Count; i++)
            {
                if (lines[i][0] == '{')
                {
                    var functionName = lines[i].Substring(1, lines[i].Length - 1);
                    functionLines[functionName] = new List<string>();

                    while(lines[++i][0] != '}')
                        functionLines[functionName].Add(lines[i]);
                }
            }

            Dictionary<string, TLMFunction> functions = new();
            foreach(var fn in functionLines)
            {
                var fnName = fn.Key;
                var fnLines = fn.Value;

                var w = fnLines[0].Length;
                var h = fnLines.Count;
                var fnInstructions = new char[w, h];
                for(int i = 0; i < w; i++)
                    for(int j = 0; j < h; j++)
                        fnInstructions[i, j] = fnLines[j][i];

                var function = new TLMFunction(fnInstructions);
                functions.Add(fnName, function);
            }
            return functions;
        }
    }
}
