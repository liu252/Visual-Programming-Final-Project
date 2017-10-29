using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    class Spawner
    {
        static Random rand = new Random();
        static float inverseSpawnChance = 60;

        public static void Update()
        {
            if (!Player.Instance.IsDead && SpriteManager.Count < 200)
            {
                if (rand.Next((int)inverseSpawnChance) == 0)
                    SpriteManager.Add(Enemy.CreateSeeker(GetSpawnPosition()));

                if (rand.Next((int)inverseSpawnChance) == 0)
                    SpriteManager.Add(Enemy.CreateWanderer(GetSpawnPosition()));
            }

            if (inverseSpawnChance > 20)
                inverseSpawnChance -= 0.05f;
        }
        private static Vector2 GetSpawnPosition()
        {
            Vector2 pos;
            do
            {
                pos = new Vector2(rand.Next((int)Game1.ScreenSize.X), rand.Next((int)Game1.ScreenSize.Y));
            }
            while (Vector2.DistanceSquared(pos, Player.Instance.Position) < 250 * 250);

            return pos;
        }
        public static void Reset()
        {
            inverseSpawnChance = 60;
        }
    }
}
