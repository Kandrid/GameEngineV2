// Entity class containing properties of Entities used in the engine.

using System;
using System.Collections.Generic;

namespace GameEngineV2
{
    public class Entity
    {
        const int MAX = 100000;
        private static Entity Access(string name)
        {
            foreach (Entity entity in Entities)
            {
                if (entity.name == name)
                {
                    return entity;
                }
            }
            return null;
        }

        public float X
        {
            get { return x; }
            set { x = (int)Math.Floor(Math.Round(value, 4)); xFloat = value; }
        }
        private int x;
        public float Y
        {
            get { return y; }
            set { y = (int)Math.Floor(Math.Round(value, 4)); yFloat = value; }
        }
        private int y;
        public float ExactX
        {
            get { return xFloat; }
        }
        private float xFloat;
        public float ExactY
        {
            get { return yFloat; }
        }
        private float yFloat;
        public float Vx, Vy;
        public char symbol;
        public string name;
        public string category;
        public HashSet<Entity> Children = new HashSet<Entity>();
        public static HashSet<Entity> Entities = new HashSet<Entity>();

        public Entity(string name, char symbol, int x, int y, float Vx = 0, float Vy = 0)
        {
            this.name = name;
            this.symbol = symbol;
            category = "";
            this.x = x;
            this.y = y;
            xFloat = x;
            yFloat = y;
            this.Vx = Vx;
            this.Vy = Vy;
            if (Entities.Count < MAX)
            {
                Entities.Add(this);
            }
        }

        public Entity(string name, char symbol, string category, int x, int y, float Vx = 0, float Vy = 0)
        {
            this.name = name;
            this.symbol = symbol;
            this.category = category;
            this.x = x;
            this.y = y;
            xFloat = x;
            yFloat = y;
            this.Vx = Vx;
            this.Vy = Vy;
            if (Entities.Count < MAX)
            {
                Entities.Add(this);
            }
        }

        public Entity(char symbol, int x, int y, float Vx = 0, float Vy = 0)
        {
            this.symbol = symbol;
            this.x = x;
            this.y = y;
            category = "";
            xFloat = x;
            yFloat = y;
            this.Vx = Vx;
            this.Vy = Vy;
            if (Entities.Count < MAX)
            {
                Entities.Add(this);
            }
        }

        public Entity(char symbol, string category, int x, int y, float Vx = 0, float Vy = 0)
        {
            this.symbol = symbol;
            this.x = x;
            this.y = y;
            this.category = category;
            xFloat = x;
            yFloat = y;
            this.Vx = Vx;
            this.Vy = Vy;
            if (Entities.Count < MAX)
            {
                Entities.Add(this);
            }
        }

        public Entity(string name, char symbol, float x, float y, float Vx = 0, float Vy = 0)
        {
            this.name = name;
            this.symbol = symbol;
            category = "";
            this.x = (int)x;
            this.y = (int)y;
            xFloat = x;
            yFloat = y;
            this.Vx = Vx;
            this.Vy = Vy;
            if (Entities.Count < MAX)
            {
                Entities.Add(this);
            }
        }

        public Entity(string name, char symbol, string category, float x, float y, float Vx = 0, float Vy = 0)
        {
            this.name = name;
            this.symbol = symbol;
            this.category = category;
            this.x = (int)x;
            this.y = (int)y;
            xFloat = x;
            yFloat = y;
            this.Vx = Vx;
            this.Vy = Vy;
            if (Entities.Count < MAX)
            {
                Entities.Add(this);
            }
        }

        public Entity(char symbol, float x, float y, float Vx = 0, float Vy = 0)
        {
            this.symbol = symbol;
            this.x = (int)x;
            this.y = (int)y;
            category = "";
            xFloat = x;
            yFloat = y;
            this.Vx = Vx;
            this.Vy = Vy;
            if (Entities.Count < MAX)
            {
                Entities.Add(this);
            }
        }

        public Entity(char symbol, string category, float x, float y, float Vx = 0, float Vy = 0)
        {
            this.symbol = symbol;
            this.x = (int)x;
            this.y = (int)y;
            this.category = category;
            xFloat = x;
            yFloat = y;
            this.Vx = Vx;
            this.Vy = Vy;
            if (Entities.Count < MAX)
            {
                Entities.Add(this);
            }
        }

        public void AddChild(Entity entity)
        {
            Children.Add(entity);
        }

        public void AddChild(string childName)
        {
            foreach (Entity entity in Entities)
            {
                if (entity.name == childName)
                {
                    Children.Add(entity);
                }
            }
        }

        public static void AddChild(string name, Entity child)
        {
            Access(name).Children.Add(child);
        }

        public static void AddChild(string name, string childName)
        {
            Access(name).Children.Add(Access(childName));
        }

        public static void Delete(int x, int y, char symbol = ' ')
        {
            HashSet<Entity> remove = new HashSet<Entity>();

            foreach (Entity entity in Entities)
            {
                if (symbol == ' ')
                {
                    if (entity.X == x && entity.Y == y)
                    {
                        remove.Add(entity);
                    }
                } else
                {
                    if (entity.X == x && entity.Y == y && entity.symbol == symbol)
                    {
                        remove.Add(entity);
                    }
                }
            }

            foreach (Entity entity in remove)
            {
                foreach (Entity child in entity.Children)
                {
                    Entities.Remove(child);
                }
                Entities.Remove(entity);
            }
        }

        public void MoveBy(float xFloat, float yFloat)
        {
            this.xFloat += xFloat;
            this.yFloat += yFloat;
            foreach (Entity child in Children)
            {
                child.xFloat += xFloat;
                child.yFloat += yFloat;
            }
        }

        public static void MoveBy(string name, float xFloat, float yFloat)
        {
            foreach (Entity entity in Entities)
            {
                if (entity.name == name)
                {
                    foreach (Entity child in entity.Children)
                    {
                        child.xFloat += xFloat;
                        child.yFloat += yFloat;
                    }
                    entity.xFloat += xFloat;
                    entity.yFloat += yFloat;
                    break;
                }
            }
        }

        public static void MoveBy(int x, int y, float xFloat, float yFloat, char symbol = ' ')
        {
            foreach (Entity entity in Entities)
            {
                if (symbol == ' ')
                {
                    if (entity.X == x && entity.y == y)
                    {
                        foreach (Entity child in entity.Children)
                        {
                            child.xFloat += xFloat;
                            child.yFloat += yFloat;
                        }
                        entity.xFloat += xFloat;
                        entity.yFloat += yFloat;
                    }
                } else
                {
                    if (entity.X == x && entity.Y == y && entity.symbol == symbol)
                    {
                        foreach (Entity child in entity.Children)
                        {
                            child.xFloat += xFloat;
                            child.yFloat += yFloat;
                        }
                        entity.xFloat += xFloat;
                        entity.yFloat += yFloat;
                    }
                }
            }
        }

        public void MoveTo(float xFloat, float yFloat)
        {
            this.xFloat = xFloat;
            this.yFloat = yFloat;
            foreach (Entity child in Children)
            {
                child.xFloat = xFloat;
                child.yFloat = yFloat;
            }
        }

        public static void MoveTo(string name, float xFloat, float yFloat)
        {
            foreach (Entity entity in Entities)
            {
                if (entity.name == name)
                {
                    foreach (Entity child in entity.Children)
                    {
                        child.xFloat = xFloat;
                        child.yFloat = yFloat;
                    }
                    entity.xFloat = xFloat;
                    entity.yFloat = yFloat;
                    break;
                }
            }
        }

        public static void MoveTo(int x, int y, float xFloat, float yFloat)
        {
            foreach (Entity entity in Entities)
            {
                if (entity.X == x && entity.Y == y)
                {
                    foreach (Entity child in entity.Children)
                    {
                        child.xFloat = xFloat;
                        child.yFloat = yFloat;
                    }
                    entity.xFloat = xFloat;
                    entity.yFloat = yFloat;
                }
            }
        }

        public static void MoveAll(char symbol, float xFloat, float yFloat)
        {
            foreach (Entity entity in Entities)
            {
                if (entity.symbol == symbol)
                {
                    foreach (Entity child in entity.Children)
                    {
                        child.xFloat += xFloat;
                        child.yFloat += yFloat;
                    }
                    entity.xFloat += xFloat;
                    entity.yFloat += yFloat;
                }
            }
        }

        public void SetVelocity(float Vx, float Vy)
        {
            this.Vx = Vx;
            this.Vy = Vy;
            foreach (Entity child in Children)
            {
                child.Vx = Vx;
                child.Vy = Vy;
            }
        }

        public static void SetVelocity(string name, float Vx, float Vy)
        {
            foreach (Entity entity in Entities)
            {
                if (entity.name == name)
                {
                    foreach (Entity child in entity.Children)
                    {
                        child.Vx = Vx;
                        child.Vy = Vy;
                    }
                    entity.Vx = Vx;
                    entity.Vy = Vy;
                    break;
                }
            }
        }

        public static void SetVelocity(int x, int y, float Vx, float Vy, char symbol = ' ')
        {
            foreach (Entity entity in Entities)
            {
                if (symbol == ' ')
                {
                    if (entity.X == x && entity.Y == y)
                    {
                        foreach (Entity child in entity.Children)
                        {
                            child.Vx = Vx;
                            child.Vy = Vy;
                        }
                        entity.Vx = Vx;
                        entity.Vy = Vy;
                    }
                }
                else
                {
                    if (entity.X == x && entity.Y == y && entity.symbol == symbol)
                    {
                        foreach (Entity child in entity.Children)
                        {
                            child.Vx = Vx;
                            child.Vy = Vy;
                        }
                        entity.Vx = Vx;
                        entity.Vy = Vy;
                    }
                }
            }
        }

        public void SetType(char symbol)
        {
            this.symbol = symbol;
            foreach (Entity child in Children)
            {
                child.symbol = symbol;
            }
        }

        public static void SetType(string name, char symbol)
        {
            foreach (Entity entity in Entities)
            {
                if (entity.name == name)
                {
                    foreach (Entity child in entity.Children)
                    {
                        child.symbol = symbol;
                    }
                    entity.symbol = symbol;
                    break;
                }
            }
        }

        public static void SetType(int x, int y, char symbol)
        {
            foreach (Entity entity in Entities)
            {
                if (entity.X == x && entity.Y == y)
                {
                    foreach (Entity child in entity.Children)
                    {
                        child.symbol = symbol;
                    }
                    entity.symbol = symbol;
                }
            }
        }

        public static bool ContainsEntity(int x, int y)
        {
            foreach (Entity entity in Entities)
            {
                if (entity.X == x && entity.Y == y)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Contains(char symbol, int x, int y)
        {
            foreach (Entity entity in Entities)
            {
                if (entity.X == x && entity.Y == y && entity.symbol == symbol)
                {
                    return true;
                }
            }

            return false;
        }

        public static Entity EntityMap(int x, int y, char symbol = ' ')
        {
            foreach (Entity entity in Entities)
            {
                if (symbol == ' ')
                {
                    if (entity.X == x && entity.Y == y)
                    {
                        return entity;
                    }
                }
                else
                {
                    if (entity.X == x && entity.Y == y && entity.symbol == symbol)
                    {
                        return entity;
                    }
                }
            }

            return null;
        }
    }
}
