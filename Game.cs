// The game file, housing the game code.

using System;
using System.Collections.Generic;

namespace GameEngineV2
{
    class Game : ConsoleEngine
    {
        //Called once at program start

        public void Initialise()
        {
            LoadMap("large.cmap");
            FRAMERATE = 24;
            DEBUG_MODE = Debug.Full;
        }

        //Called every frame

        public void GameExecute()
        {
            
        }
    }
}
