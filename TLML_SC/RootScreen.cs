namespace TLML_SC.Scenes
{
    internal class RootScreen : ScreenObject
    {
        private ScreenSurface mainSurface;

        TLMProgram program;

        public RootScreen()
        {
            mainSurface = new ScreenSurface(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);
            
            var lines = ReadLines("code.tlm");
           /* int k = 1;
            foreach(var l in lines)
                mainSurface.Print(2, k++, l);*/


            program = new TLMProgram(Parser.ParseLines(lines));
            program.Startup();

            Children.Add(mainSurface);
        }

        int t = 0;
        public override void Update(TimeSpan delta)
        {
            if (t++ % 10 != 0 || t < 300)
                return;
            program.Step();

            if (!program.done)
            {
                mainSurface.Clear();
                var fn = program.functionStack.Peek();

                for (int i = 0; i < fn.instr.GetLength(0); i++)
                    for (int j = 0; j < fn.instr.GetLength(1); j++)
                        if ((i, j) == fn.ptr)
                            mainSurface.Print(i + 10, j + 10, new ColoredString(fn.instr[i, j].ToString(), Color.White, Color.BlueViolet));
                        else
                            mainSurface.Print(i + 10, j + 10, new ColoredString(fn.instr[i, j].ToString(), Color.White, Color.Transparent));

                var arr = program.stack.ToArray().Reverse().ToArray();
                for(int i = 0; i < program.stack.Count; i++)
                {
                    mainSurface.Print(30, 15 - i, arr[i].ToString());
                }
            }
            base.Update(delta);
        }

        public List<string> ReadLines(string path)
        {
            string line;
            List<string> lines = new List<string>();
            try
            {
                StreamReader streamReader = new StreamReader(path);
                line = streamReader.ReadLine();
                while (line is not null)
                {
                    lines.Add(line);
                    line = streamReader.ReadLine();
                }

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return lines;
        }
    }
}
