using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    abstract class Sprite
    {
        public Texture2D mSpriteTexture;
        public Vector2 Position, Velocity;
        public float Orientation;
        public bool IsExpired;
        public string AssetName;
        public Color color = Color.White;
        public float Radius = 20;
        public Rectangle area;
        public Vector2 Size
        {
            get
            {
                return mSpriteTexture == null ? Vector2.Zero : new Vector2(mSpriteTexture.Width, mSpriteTexture.Height);
            }
        }
        public void Load()
        {
            area = new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height);
        }
        /*
        public Rectangle Size;
        private float mScale = 1.0f;
        public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;

                Size = new Rectangle(0, 0, (int)(Source.Width * Scale), (int)(Source.Height * Scale));
            }
        }

        Rectangle mSource;
        public Rectangle Source
        {
            get { return mSource; }
            set
            {
                mSource = value;
                Size = new Rectangle(0, 0, (int)(mSource.Width * Scale), (int)(mSource.Height * Scale));
            }
        }
        */
        public abstract void Update();
        /*
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            mSpriteTexture = Images.Player;
            AssetName = theAssetName;
            //Source = new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height);
            //Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
        }
        */

        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, Position, area, color, Orientation, Size / 2f, 1f, 0, 0);
        }

    }
}
