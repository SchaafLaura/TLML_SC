namespace TLML_SC.Scenes
{
    internal class RootScreen : ScreenObject
    {
        private ScreenSurface mainSurface;

        public RootScreen()
        {
            mainSurface = new ScreenSurface(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);
            
            var lines = ReadLines("code.tlm");
            int k = 1;
            foreach(var l in lines)
                mainSurface.Print(2, k++, l);


            Parser.ParseLines(lines);








            Children.Add(mainSurface);
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
