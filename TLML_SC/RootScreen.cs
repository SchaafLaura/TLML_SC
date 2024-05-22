using NAudio.CoreAudioApi;
using SadConsole.Input;
using SadConsoleGame.Audio;

namespace TLML_SC.Scenes
{
    internal class RootScreen : ScreenObject
    {
        private ScreenSurface mainSurface;

        TLMProgram program;
        AudioProvider audio;

        public RootScreen()
        {
            mainSurface = new ScreenSurface(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);
            
            var lines = ReadLines("code.tlm");

            program = new TLMProgram(Parser.ParseLines(lines));
            program.Startup();

            //audio = new IngameAudioProvider();
            audio = new NullAudioProvider();

            Children.Add(mainSurface);
        }

        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            if(keyboard.KeysReleased.Count > 0)
            {
                char c = ProcessKey(keyboard.KeysReleased[0].Key);
                program.Input(c);
                keyboard.Clear();
            }
            return base.ProcessKeyboard(keyboard);
        }

        private char ProcessKey(Keys key)
        {
            return key switch {
                Keys.D0 or Keys.D1 or Keys.D2 or Keys.D3 or Keys.D4 or
                Keys.D5 or Keys.D6 or Keys.D7 or Keys.D8 or Keys.D9
                => key.ToString()[1],
                Keys.A or Keys.B or Keys.C or Keys.D or Keys.E or Keys.F or Keys.G or
                Keys.H or Keys.I or Keys.J or Keys.K or Keys.L or Keys.M or Keys.N or
                Keys.O or Keys.P or Keys.Q or Keys.R or Keys.S or Keys.T or Keys.U or
                Keys.V or Keys.W or Keys.X or Keys.Y or Keys.Z 
                => key.ToString()[0],
                _ => '?'
            };
        }

        int t = 0;
        public override void Update(TimeSpan delta)
        {
            if (t++ % 8 != 0 || t < 0)
                return;
            /*for (int i = 0; i < 100000 && !program.done; i++)
                program.Step();*/

            if(!program.done)
                program.Step();
            

            mainSurface.Clear();
            if (!program.done)
            {
                var fn = program.functionStack.Peek();

                for (int i = 0; i < fn.instr.GetLength(0); i++)
                    for (int j = 0; j < fn.instr.GetLength(1); j++)
                        if ((i, j) == fn.ptr)
                            mainSurface.Print(i + 10, j + 10, new ColoredString(fn.instr[i, j].ToString(), Color.White, Color.BlueViolet));
                        else
                            mainSurface.Print(i + 10, j + 10, new ColoredString(fn.instr[i, j].ToString(), Color.White, Color.Transparent));
            }
            var arr = program.stack.ToArray().Reverse().ToArray();
            for (int i = 0; i < program.stack.Count; i++)
            {
                mainSurface.Print(40, 26 - i, arr[i].ToString());
            }

            mainSurface.Print(40, 26, "Stack Size: " + program.stack.Count.ToString());
            mainSurface.Print(37, 27, "fn-Stack Size: " + program.functionStack.Count.ToString());
            mainSurface.Print(44, 28, "Output: " + program.output);
            mainSurface.Print(45, 29, "Steps: " + program.stepsTaken.ToString());

            if(program.error is not null)
            {
                mainSurface.Print(10, 20, new ColoredString(program.error, Color.Red, Color.Transparent));
            }




            if (!program.done && program.stepsTaken != -1)
            {
                var fn = program.functionStack.Peek();
                var ptr = fn.ptr;
                if (ptr.X < 0 || ptr.Y < 0)
                    return;
                var op = fn.instr[fn.ptr.X, fn.ptr.Y];
                int sound = op switch
                {
                    >= '0' and <= '9' => op - 48,
                    >= 'A' and <= 'Z' => op - 65,
                    >= 'a' and <= 'z' => op - 97,
                    _ => -1
                };
                if(sound != -1)
                {
                    audio.PlaySound(sound);
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
