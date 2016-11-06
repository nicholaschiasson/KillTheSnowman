#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion


namespace KillTheSnowman
{
    public class Snowman : Player
    {

        static float moveSpeed = 0.125f;

        public int SnowballsThrown { get; set; }
        public int KillCount { get; set; }

        MouseState oldMouseState;

        public Snowman() : base() { }

        public override void Initialize()
        {
            SnowballsThrown = 0;
            KillCount = 0;

            frames = 10;
            framesPerSec = 16;
            clips = 8;

            position = new Vector2(Game1.WINDOW_WIDTH / 2, Game1.WINDOW_HEIGHT / 2);
            rotation = 0.0f;
            scale = 1.0f;
            depth = 1.0f;

            damage = 50;
            defense = 0;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateInput();

            if (velocity.Y == moveSpeed)
            {
                playerAnimation.Clip = 0;
            }
            else if (velocity.Y == -moveSpeed)
            {
                playerAnimation.Clip = 1;
            }
            else if (velocity.X == moveSpeed)
            {
                playerAnimation.Clip = 2;
            }
            else if (velocity.X == -moveSpeed)
            {
                playerAnimation.Clip = 3;
            }

            base.Update(gameTime);
        }

        private void UpdateInput()
        {
            KeyboardState newKeyState = Keyboard.GetState();
            MouseState newMouseState = Mouse.GetState();
            moving = false;

            velocity = Vector2.Zero;

            if (newKeyState.IsKeyDown(Keys.W))
            {
                if (velocity.Y > -moveSpeed)
                {
                    velocity.Y -= moveSpeed;
                }
                moving = true;
            }
            if (newKeyState.IsKeyDown(Keys.S))
            {
                if (velocity.Y < moveSpeed)
                {
                    velocity.Y += moveSpeed;
                }
                moving = true;
            }
            if (newKeyState.IsKeyDown(Keys.A))
            {
                if (velocity.X > -moveSpeed)
                {
                    velocity.X -= moveSpeed;
                }
                moving = true;
            }
            if (newKeyState.IsKeyDown(Keys.D))
            {
                if (velocity.X < moveSpeed)
                {
                    velocity.X += moveSpeed;
                }
                moving = true;
            }

            if (velocity.Length() != 0.0f)
            {
                velocity.Normalize();
                velocity.X *= moveSpeed;
                velocity.Y *= moveSpeed;
            }

            if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                Vector2 delta = new Vector2(newMouseState.X - position.X, newMouseState.Y - position.Y);
                Game1.ThrowSnowball(position, delta, damage, this);
                SnowballsThrown++;
            }

            oldMouseState = newMouseState;
        }

        public override void DealDamage(int damage)
        {
            base.DealDamage(damage);
            Game1.AddEffect("bloodeffect_01", position);
        }
    }
}
