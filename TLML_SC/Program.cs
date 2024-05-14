using SadConsole.Configuration;
using TLML_SC;

Settings.WindowTitle = "My SadConsole Game";

Builder gameStartup = new Builder()
    .SetScreenSize(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
    .SetStartingScreen<TLML_SC.Scenes.RootScreen>()
    .IsStartingScreenFocused(true)
    .ConfigureFonts(true)
    .OnStart(Startup);
    ;

Game.Create(gameStartup);
Game.Instance.Run();
Game.Instance.Dispose();

static void Startup(object? sender, GameHost host)
{
    Settings.ResizeMode = Settings.WindowResizeOptions.Stretch;
}