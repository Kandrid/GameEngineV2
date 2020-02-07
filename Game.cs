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
            LoadMap("space.cmap");
            FRAMERATE = 1000;
            DEBUG_MODE = Debug.Partial;
            CAMERA.Width = 150;
            CAMERA.Height = 100;
            new Entity('o', 8, 3, 1f, 0.1f);
        }

        struct Data
        {
            public float x, y, Vx, Vy;

            public Data(float x, float y, float Vx, float Vy)
            {
                this.x = x;
                this.y = y;
                this.Vx = Vx;
                this.Vy = Vy;
            }
        }

        //Called every frame

        public void GameExecute()
        {
            HashSet<Data> splits = new HashSet<Data>();
            
            foreach (Entity e in Entity.Entities)
            {
                if (e.ExactX + e.Vx < 1 || e.ExactX + e.Vx >= MAP_WIDTH - 1)
                {
                    splits.Add(new Data(e.ExactX, e.ExactY, -e.Vx, -e.Vy));
                    e.Vx *= -1;
                }
                if (e.ExactY + e.Vy < 1 || e.ExactY + e.Vy >= MAP_HEIGHT - 1)
                {
                    splits.Add(new Data(e.ExactX, e.ExactY, -e.Vx, -e.Vy));
                    e.Vy *= -1;
                }
            }

            foreach (Data d in splits)
            {
                new Entity('o', d.x, d.y, d.Vx, d.Vy);
            }
        }
    }
}
