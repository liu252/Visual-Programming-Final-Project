using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    class Platform : Sprite
    {
        private static Platform instance;
        public static Platform Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Platform();
                }
                return instance;
            }
        }

        private Platform()
        {
            mSpriteTexture = Images.Platform;
            Position = Game1.ScreenSize/2;
            Radius = 40;
        }

        public override void Update() { }
    }
}
