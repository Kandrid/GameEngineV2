// The game file, housing the game code.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameEngineV2
{
    class Game : ConsoleEngine
    {
        //Called once at program start

        public class Nbody
        {
            public static HashSet<Nbody> bodies = new HashSet<Nbody>();
            private const double G = 6.673e-11;   // gravitational constant
            public const double solarmass = 1.98892e30;
            public static double timestep = 1e11;
            public static double scale = 1e11;
            public const double lightyear = 9.461e+15;

            public Entity e;

            public double rx, ry;       // holds the cartesian positions
            public double vx, vy;       // velocity components 
            public double fx, fy;       // force components
            public double mass;         // mass

            // create and initialize a new Nbody
            public Nbody(char symbol, double rx, double ry, double vx, double vy, double mass, bool scaling = true)
            {
                this.rx = scaling ? rx * scale : rx;
                this.ry = scaling ? ry * scale : ry;
                this.vx = vx;
                this.vy = vy;
                this.mass = mass;
                bodies.Add(this);
                e = new Entity(symbol, (int)rx, (int)ry);
            }

            // update the velocity and position using a timestep dt
            private void update(double dt)
            {
                vx += dt * fx / mass;
                vy += dt * fy / mass;
                rx += dt * vx;
                ry += dt * vy;
            }

            // returns the distance between two bodies
            private double distanceTo(Nbody b)
            {
                double dx = rx - b.rx;
                double dy = ry - b.ry;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            // set the force to 0 for the next iteration
            private void resetForce()
            {
                fx = 0.0;
                fy = 0.0;
            }

            // compute the net force acting between the Nbody a and b, and
            // add to the net force acting on a
            private void addForce(Nbody b)
            {
                Nbody a = this;
                double EPS = 3E4;      // softening parameter (just to avoid infinities)
                double dx = b.rx - a.rx;
                double dy = b.ry - a.ry;
                double dist = Math.Sqrt(dx * dx + dy * dy) + 1e-13;
                double F = (G * a.mass * b.mass) / (dist * dist + EPS * EPS);
                a.fx += F * dx / dist;
                a.fy += F * dy / dist;
            }

            // convert to string representation formatted nicely
            public string toString()
            {
                return "" + rx + ", " + ry + ", " + vx + ", " + vy + ", " + mass;
            }

            private static void addforces()
            {
                Parallel.ForEach(bodies, (nbody) =>
               {
                   nbody.resetForce();
                    //Notice-2 loops-->N^2 complexity
                    foreach (Nbody n in bodies)
                   {
                       if (nbody != n) nbody.addForce(n);
                   }
               });
                //Then, loop again and update the bodies using timestep dt
                Parallel.ForEach(bodies, (nbody) =>
                {
                    nbody.update(timestep);
                    nbody.e.X = (float)(nbody.rx / scale);
                    nbody.e.Y = (float)(nbody.ry / scale);
                });
            }

            public static void Update()
            {
                addforces();
            }

            public void Remove()
            {
                Entity.Delete((int)e.X, (int)e.Y, e.symbol);
                bodies.Remove(this);
            }
        }

        Random random = new Random();

        private double distance(Nbody a, Nbody b)
        {
            double dx = a.rx - b.rx;
            double dy = a.ry - b.ry;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        Nbody sun, earth, venus, mercury, mars, b;

        public void Initialise()
        {
            LoadMap("large.cmap");
            DEBUG_MODE = Debug.Partial;
            FRAMERATE = 60;
            //CAMERA.Width = 234;
            //CAMERA.Height = 110;
            //CAMERA.X = 0;
            //CAMERA.Y = MAP_HEIGHT / 3;
            Nbody.timestep = 2147483648;
            Nbody.scale = 3 * Nbody.lightyear / MAP_HEIGHT;
            //sun = new Nbody('S', MAP_WIDTH / 2, MAP_HEIGHT / 2, 0, 0, Nbody.solarmass);
            //earth = new Nbody('E', (MAP_WIDTH / 2) * Nbody.scale + 1.495e11, (MAP_HEIGHT / 2) * Nbody.scale, 0, 29780, 5.972e24, false);
            //venus = new Nbody('V', (MAP_WIDTH / 2) * Nbody.scale - 1.08e11, (MAP_HEIGHT / 2) * Nbody.scale, 0, -35020, 4.8675e24, false);
            //mercury = new Nbody('m', (MAP_WIDTH / 2) * Nbody.scale + 5.7909e10, (MAP_HEIGHT / 2) * Nbody.scale, 0, 47362, 3.3011e23, false);
            //mars = new Nbody('M', (MAP_WIDTH / 2) * Nbody.scale - 2.279392e11, (MAP_HEIGHT / 2) * Nbody.scale, 0, -24007, 6.4171e23, false);
            new Nbody('%', MAP_WIDTH / 2 + 20, MAP_HEIGHT / 2, 0, 7e4, Nbody.solarmass * 1000000);
            new Nbody('%', MAP_WIDTH / 2 - 20, MAP_HEIGHT / 2, 0, -7e4, Nbody.solarmass * 1000000);
            for (int i = 0; i < 0; i++)
            {
              new Nbody('.', random.NextDouble() * MAP_WIDTH, random.NextDouble() * MAP_HEIGHT, 0, 0, Nbody.solarmass);
            }
        }

        //Called every frame

        public void GameExecute()
        {
            Nbody.Update();
            start:
            foreach (Nbody n in Nbody.bodies)
            {
                if (n.rx < 0)
                {
                    n.Remove();
                    goto start;
                    //n.vx = Math.Abs(n.vx);
                } else if (n.rx > MAP_WIDTH * Nbody.scale)
                {
                    n.Remove();
                    goto start;
                    //n.vx = -Math.Abs(n.vx);
                }
                if (n.ry < 0)
                {
                    n.Remove();
                    goto start;
                    //n.vy = Math.Abs(n.vy);
                }
                else if (n.ry > MAP_HEIGHT * Nbody.scale)
                {
                    n.Remove();
                    goto start;
                    //n.vy = -Math.Abs(n.vy);
                }
            }

            switch (INPUT)
            {
                case ConsoleKey.UpArrow:
                    Nbody.timestep *= 2;
                    break;
                case ConsoleKey.DownArrow:
                    Nbody.timestep /= 2;
                    break;
                case ConsoleKey.W:
                    Nbody.scale *= 2;
                    break;
                case ConsoleKey.S:
                    Nbody.scale /= 2;
                    break;
                case ConsoleKey.I:
                    CAMERA.ZoomIn();
                    break;
                case ConsoleKey.O:
                    CAMERA.ZoomOut();
                    break;
                case ConsoleKey.T:
                    CAMERA.MoveUp();
                    break;
                case ConsoleKey.F:
                    CAMERA.MoveLeft();
                    break;
                case ConsoleKey.G:
                    CAMERA.MoveDown();
                    break;
                case ConsoleKey.H:
                    CAMERA.MoveRight();
                    break;
            }

            customDebugVariables.Add("Time Step", Nbody.timestep);
            customDebugVariables.Add("Scale", Nbody.scale);
            //customDebugVariables.Add("Earth Distance from Sun", distance(sun, earth));
            //customDebugVariables.Add("Moon Distance from Earth", distance(moon, earth));
            //customDebugVariables.Add("Earth Velocity", Math.Sqrt(earth.vx * earth.vx + earth.vy * earth.vy));
            customDebugVariables.Add("Time Ratio Per 1 Second", Nbody.timestep * FRAMERATE_NOW);
        }
    }
}
