using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game4
{
    class Player :Sprite
    {
        ContentManager mContentManager;
        const string Player_AssetName = "Player";
        const int Start_Pos_X = 125;
        const int Start_Pos_Y = 200;
        const int Speed = 4;
        const int Up = -1;
        const int Down = 1;
        const int Left = -1;
        const int Right = 1;

        int framesUntilRespawn = 0;
        public bool IsDead { get { return framesUntilRespawn > 0; } }

        const int cooldownFrames = 6;
        int cooldownRemaining = 0;
        static Random rand = new Random();


        private static Player instance;
        public static Player Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Player();
                }
                return instance;
            }
        }

        bool jumped = true;

        public enum State
        {
            Walking,
            Jumping
        }
        public static State mCurrentState = State.Walking;

        public static Vector2 DirectionU = Vector2.Zero;

        KeyboardState mPreviousKeyboardState;

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;
        Vector2 mStartingPosition = Vector2.Zero;
        Vector2 mouse;

        private Player()
        {
            mSpriteTexture = Images.Player;
            area = new Rectangle(0, 0, 40, mSpriteTexture.Height);
            Position = new Vector2(Start_Pos_X,Start_Pos_Y);
            jumped = true;
            Radius = 10;

        }

        public override void Update()
        {
            if (IsDead)
            {
                framesUntilRespawn--;
                return;
            }

            if (Position.Y + mSpriteTexture.Height <= 400)
            {
                DirectionU.Y += Down;
                jumped = false;
            }


            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            MouseState aMouseState = Mouse.GetState();
            UpdateMovement();
            UpdateJump();
            mPreviousKeyboardState = aCurrentKeyboardState;
            //var aim = Controls.MouseAimDirection();
            if (aCurrentKeyboardState.IsKeyDown(Keys.W) == true)
            {
                area = new Rectangle(40, 0, 40, mSpriteTexture.Height);
            }
            else
            {
                area = new Rectangle(0, 0, 40, mSpriteTexture.Height);
            }

            var aim = Controls.GetAimDirection();
            if (aMouseState.LeftButton == ButtonState.Pressed)
            {
                if (aim.LengthSquared() > 0 && cooldownRemaining <= 0)
                {
                    cooldownRemaining = cooldownFrames;
                    float aimAngle = aim.ToAngle();
                    Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                    float randomSpread = rand.NextFloat(-0.04f, 0.04f) + rand.NextFloat(-0.04f, 0.04f);
                    Vector2 vel = Extensions.FromPolar(aimAngle + randomSpread, 11f);

                    Vector2 offset = Vector2.Transform(new Vector2(25, -8), aimQuat);
                    SpriteManager.Add(new Bullets(Position + offset, vel));
                    
                }

                if (cooldownRemaining > 0)
                    cooldownRemaining--;
            }


        }
        public override void Draw(SpriteBatch theSpriteBatch)
        {
            if (!IsDead)
                base.Draw(theSpriteBatch);
        }

        private void UpdateMovement()
        {
            const float speed = 4;

            area = new Rectangle(0, 0, 40, mSpriteTexture.Height);
            Velocity = speed * Controls.MovementDirectionLR();
            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, Game1.ScreenSize - Size / 2);
        }
        private void UpdateJump()
        {

                jumped = true;
                const float speed = 4;
                Velocity = speed * Controls.MovementDirectionUD();
                Position += Velocity;
                Position = Vector2.Clamp(Position, Size / 2, Game1.ScreenSize - Size / 2);


        }
        public void Kill()
        {
            framesUntilRespawn = 60;
            PlayerStatus.RemoveLife();
            if (PlayerStatus.Lives == 0)
                PlayerStatus.Reset();
        }
        public void HandleCollision(Platform platform)
        {
            var d = Position - platform.Position;
            Velocity += 10 * d / (d.LengthSquared() + 1);
        }

    }
}
