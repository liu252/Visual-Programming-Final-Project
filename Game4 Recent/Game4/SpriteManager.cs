using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    class SpriteManager
    {
        static List<Sprite> sprites = new List<Sprite>();
        static List<Enemy> enemies = new List<Enemy>();
        static List<Bullets> bullets = new List<Bullets>();
        static List<Platform> platforms = new List<Platform>();
        static bool isUpdating;
        static List<Sprite> addedSprites = new List<Sprite>();
        public static int Count { get { return sprites.Count; } }

        public static void Add(Sprite sprite)
        {
            if(!isUpdating)
            {
                AddSprite(sprite);
            }
            else
            {
                addedSprites.Add(sprite);
            }
        }
        private static void AddSprite(Sprite sprite)
        {
            sprites.Add(sprite);
            if (sprite is Bullets)
                bullets.Add(sprite as Bullets);
            else if (sprite is Enemy)
                enemies.Add(sprite as Enemy);
        }
        public static void Update()
        {
            isUpdating = true;
            HandleCollisions();
            foreach (var sprite in sprites)
            {
                sprite.Update();
            }

            isUpdating = false;
            
            foreach (var sprite in addedSprites)
            {
                AddSprite(sprite);
            }

            addedSprites.Clear();

            sprites = sprites.Where(x => !x.IsExpired).ToList();
            bullets = bullets.Where(x => !x.IsExpired).ToList();
            enemies = enemies.Where(x => !x.IsExpired).ToList();
        }
        private static bool IsColliding(Sprite a, Sprite b)
        {
            float radius = a.Radius + b.Radius;
            return !a.IsExpired && !b.IsExpired && Vector2.DistanceSquared(a.Position, b.Position) < radius * radius;
        }

        static void HandleCollisions()
        {
            for (int i = 0; i < enemies.Count; i++)
                for (int j = i + 1; j < enemies.Count; j++)
                {
                    if (IsColliding(enemies[i], enemies[j]))
                    {
                        enemies[i].HandleCollision(enemies[j]);
                        enemies[j].HandleCollision(enemies[i]);
                    }
                }

            for (int i = 0; i < enemies.Count; i++)
                for (int j = 0; j < bullets.Count; j++)
                {
                    if (IsColliding(enemies[i], bullets[j]))
                    {
                        enemies[i].WasShot();
                        bullets[j].IsExpired = true;
                    }
                }

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].IsActive && IsColliding(Player.Instance, enemies[i]))
                {
                    Player.Instance.Kill();
                    enemies.ForEach(x => x.WasShot());
                    break;
                }
            }

            if (IsColliding(Player.Instance, Platform.Instance))
            {
                Player.Instance.HandleCollision(Platform.Instance);
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var sprite in sprites)
            {
                sprite.Draw(spriteBatch);
            }
        }
    }
}
