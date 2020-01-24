//Engine class runs the game and handles map rendering and processing

using System;
using System.IO;
using System.Collections.Generic;

namespace GameEngineV2
{
    
    public class ConsoleEngine
    {
        public struct Camera
        {
            public int X, Y;
            public uint Width, Height;

            public void ZoomIn()
            {
                if (Width > 2 && Height > 2)
                {
                    Width -= 2;
                    X++;
                    Height -= 2;
                    Y++;
                }
                Console.Clear();
            }

            public void ZoomOut()
            {
                Width += 2;
                X--;
                Height += 2;
                Y--;
                Console.Clear();
            }

            public void MoveUp()
            {
                Y--;
            }

            public void MoveDown()
            {
                Y++;
            }

            public void MoveLeft()
            {
                X--;
            }

            public void MoveRight()
            {
                X++;
            }
        }

        public enum Debug
        {
            Full,
            Partial,
            None
        }

        public static float FRAMERATE_NOW;
        public static int MAP_WIDTH;
        public static int MAP_HEIGHT;
        public static int FRAMERATE = 30;
        public static Camera CAMERA;
        private static int x = 0;
        private static int y = 0;
        private static string output;
        public static char[,] NEW_LAYOUT, LAYOUT;
        private static int objectCount;
        private static string outputBuffer = "";
        public static Debug DEBUG_MODE = Debug.None;
        public static bool FRAME_BY_FRAME = false;
        public static readonly SortedList<string, object> customDebugVariables = new SortedList<string, object>();
        public static ConsoleKey INPUT;
        public static string CURRENT_MAP;
        public static int AVERAGE_FRAMES = 15;
        private static Queue<float> runTimes = new Queue<float>();
        public static long deltaTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        private static long framecount = 0;
        private static long runTime;
        private static float frameOffset = 0;

        static void Main()
        {
            //Initialisation

            Game game = new Game();
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;

            //Map design using Rectangular dimensions n x m read from cmap file

            if (!File.Exists("default.cmap"))
            {
                File.WriteAllText("default.cmap", "█████\r\n█   █\r\n█   █\r\n█   █\r\n█████\r\n");
            }

            LoadMap("default.cmap");

            game.Initialise();
            Console.Clear();

            //Main runtime

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    INPUT = Console.ReadKey(true).Key;
                }
                else
                {
                    INPUT = ConsoleKey.F1;
                }

                customDebugVariables.Clear();
                game.GameExecute();
                VelocityUpdate();

                Console.SetCursorPosition(0, 0);
                Console.CursorVisible = false;

                if (INPUT == ConsoleKey.Tab)
                {
                    Console.Clear();
                }

                while (DateTimeOffset.Now.ToUnixTimeMilliseconds() - deltaTime < 1000f / FRAMERATE + frameOffset) { }

                Display();
               
                if (FRAME_BY_FRAME)
                {
                    Console.ReadKey();
                }
                deltaTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
        }

        public static void LoadMap(string cmap)
        {
            Console.Clear();
            string map = File.ReadAllText(cmap);
            CURRENT_MAP = cmap;

            y = 0;

            foreach (char c in map)
            {
                if (c == '\n')
                {
                    MAP_WIDTH = x - 2;
                    x = 0;
                    y++;
                }
                x++;
            }

            MAP_HEIGHT = y;

            NEW_LAYOUT = new char[MAP_WIDTH, MAP_HEIGHT];
            LAYOUT = new char[MAP_WIDTH, MAP_HEIGHT];

            CAMERA.X = 0;
            CAMERA.Y = 0;
            CAMERA.Width = (uint)MAP_WIDTH;
            CAMERA.Height = (uint)MAP_HEIGHT;

            x = 0;
            y = 0;

            foreach (char c in map)
            {
                if (c == '\n')
                {
                    x = 0;
                    y++;
                } else if (c != '\r')
                {
                    try
                    {
                        NEW_LAYOUT[x, y] = c;
                    } catch (Exception)
                    {
                        throw new Exception("Map size was not rectangular");
                    }
                    x++;
                }
            }
        }

        private static void VelocityUpdate()
        {
            foreach (Entity entity in Entity.Entities)
            {
                if (entity.ExactX + entity.Vx > MAP_WIDTH && entity.Vx != 0)
                {
                    entity.X = MAP_WIDTH;
                }
                else if (entity.ExactX + entity.Vx < 0 && entity.Vx != 0)
                {
                    entity.X = 0;
                } else
                {
                    entity.X = entity.ExactX + entity.Vx;
                }

                if (entity.ExactY + entity.Vy > MAP_HEIGHT && entity.Vy != 0)
                {
                    entity.Y = MAP_HEIGHT;
                }
                else if (entity.ExactY + entity.Vy < 0 && entity.Vy != 0)
                {
                    entity.Y = 0;
                } else
                {
                    entity.Y = entity.ExactY + entity.Vy;
                }

                foreach (Entity child in entity.Children)
                {
                    child.X = child.ExactX + entity.Vx;
                    child.Y = child.ExactY + entity.Vy;
                }
            }
        }

        private static void Display()
        {
            output = "";
            outputBuffer = "";
            x = 0;
            objectCount = Entity.Entities.Count;

            LAYOUT = new char[MAP_WIDTH, MAP_HEIGHT];

            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    LAYOUT[x, y] = NEW_LAYOUT[x, y];
                }
            }

            foreach (Entity entity in Entity.Entities)
            {
                x++;

                if (x > objectCount - 20)
                {
                    outputBuffer += $"\nName: {entity.name}, Symbol: {entity.symbol}, Category: {entity.category}, Vx: {Math.Round(entity.Vx, 2)}, Vy: {Math.Round(entity.Vy, 2)}, X: {entity.X}, Y: {entity.Y}, ExactX: {Math.Round(entity.ExactX, 2)}, ExactY: {Math.Round(entity.ExactY, 2)}                   ";
                }

                if (entity.Y < CAMERA.Y + CAMERA.Height && entity.Y >= CAMERA.Y && entity.X < CAMERA.X + CAMERA.Width && entity.X >= CAMERA.X && entity.X >= 0 && entity.X < MAP_WIDTH && entity.Y >= 0 && entity.Y < MAP_HEIGHT)
                {
                    LAYOUT[(int)entity.X, (int)entity.Y] = entity.symbol;
                }
            }

            x = CAMERA.X;

            for (y = CAMERA.Y; y < CAMERA.Y + CAMERA.Height; y++)
            {
                for (x = CAMERA.X; x < CAMERA.X + CAMERA.Width; x++)
                {
                    if (x >= 0 && x < MAP_WIDTH && y >= 0 && y < MAP_HEIGHT)
                    {
                        output += LAYOUT[x, y];
                    }  
                    else 
                    {
                        output += "�";
                    }
                }
                output += '\n';
            }

            runTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() - deltaTime;

            runTimes.Enqueue(runTime);

            if (framecount == AVERAGE_FRAMES)
            {
                runTimes.Dequeue();
            }
            else
            {
                framecount++;
            }

            float average = 0;

            foreach (long time in runTimes)
            {
                average += time;
            }

            average /= runTimes.Count;
            average = 1000f / average;
            FRAMERATE_NOW = average;
            
            if (1000f / runTime < FRAMERATE)
            {
                frameOffset -= 0.1f / FRAMERATE;
            } else if (average > FRAMERATE && frameOffset > 1 - 1000f / FRAMERATE)
            {
                frameOffset += 0.1f / FRAMERATE;
            }
            
            if (DEBUG_MODE == Debug.Full || DEBUG_MODE == Debug.Partial)
            {              
                output += $"\nFramerate: {Math.Round(average, 1)}       \n";
                output += "\nMap: " + CURRENT_MAP + "                \n";
                output += $"\nObject Count: {objectCount}            \n";
                output += $"\nCamera X, Y: {CAMERA.X}, {CAMERA.Y}            \n";
                output += $"\nCamera Width, Height: {CAMERA.Width}, {CAMERA.Height}               \n";
                if (INPUT != ConsoleKey.F1)
                {
                    output += $"\nInput: {INPUT}               \n";
                } else
                {
                    output += $"\nInput:                       \n";
                }
            }

            foreach (KeyValuePair<string, object> pair in customDebugVariables)
            {
                output += '\n' + pair.Key + ": " + $"{pair.Value}                         \n";
            }

            if (DEBUG_MODE == Debug.Full)
            {
                output += "\nObject List:\n";
                output += outputBuffer + "                                                                                                                                                      ";
            }

            Console.Write(output);
        }
    }
}