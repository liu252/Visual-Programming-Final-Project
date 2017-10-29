using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    class Bullets : Sprite
    {
        public Bullets(Vector2 position, Vector2 velocity)
        {
            mSpriteTexture = Images.Bullet;
            area = new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height);
            Position = position;
            Velocity = velocity;
            Orientation = Velocity.ToAngle();
            Radius = 8;
        }
        public override void Update()
        {
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            Position += Velocity;

            if (!Game1.Viewport.Bounds.Contains(Position.ToPoint()))
                IsExpired = true;
        }
    }
}
