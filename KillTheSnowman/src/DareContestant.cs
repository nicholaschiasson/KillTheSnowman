#region Using Statements
using System;

using Microsoft.Xna.Framework;
#endregion

namespace KillTheSnowman
{
	enum ContestantState
	{
		SPAWNING,
		CHASING,
		ATTACKING
	}

	enum ContestantTilt
	{
		RIGHT,
		LEFT
	}

    class DareContestant : Player
    {
        static float moveSpeed = 0.175f;

        bool hit;
        ContestantState state;
        ContestantTilt tilt;
        Random rand;

        public DareContestant() : base() { }

        public override void Initialize()
        {
            hit = false;

            frames = 8;
            framesPerSec = 16;
            clips = 2;

            state = ContestantState.SPAWNING;
            tilt = ContestantTilt.RIGHT;
            rand = new Random();

            position = new Vector2(rand.Next(Game1.WINDOW_WIDTH), rand.Next(Game1.WINDOW_HEIGHT));
            rotation = 0.0f;
            scale = 0.0f;
            depth = 1.0f;

            damage = 10;

            base.Initialize();
            if (defense < health / 2)
            {
                defense = Game1.Level;
            }
            else
            {
                defense = health / 2 - 1;
            }
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (state == ContestantState.SPAWNING)
            {
                if (playerAnimation.Scale >= 1.0f)
                {
                    playerAnimation.Rotation = 0.0f;
                    playerAnimation.Scale = 1.0f;
                    state = ContestantState.CHASING;
                }
                else
                {
                    if (tilt == ContestantTilt.RIGHT)
                    {
                        playerAnimation.Rotation += 0.05f;
                        if (playerAnimation.Rotation > 0.125f)
                        {
                            tilt = ContestantTilt.LEFT;
                        }
                    }
                    else
                    {
                        playerAnimation.Rotation -= 0.05f;
                        if (playerAnimation.Rotation < -0.125f)
                        {
                            tilt = ContestantTilt.RIGHT;
                        }
                    }
                    playerAnimation.Scale += elapsedSeconds / 2.0f;
                    moving = false;
                }
            }
            if (state == ContestantState.CHASING)
            {
                playerAnimation.Play();
                if (position.X < Game1.snowman.right && position.X > Game1.snowman.left && position.Y < Game1.snowman.bottom && position.Y > Game1.snowman.top)
                {
                    state = ContestantState.ATTACKING;
                    playerAnimation.Stop();
                }
                else
                {
                    velocity.X = Game1.SnowmanPosition.X - position.X;
                    velocity.Y = Game1.SnowmanPosition.Y - position.Y;
                    if (velocity.Length() != 0.0f)
                    {
                        velocity.Normalize();
                        velocity.X *= moveSpeed;
                        velocity.Y *= moveSpeed;
                    }
                    moving = true;
                    playerAnimation.Clip = 0;
                }
            }
            if (state == ContestantState.ATTACKING)
            {
                playerAnimation.Play();
                if (!(left < Game1.snowman.right && right > Game1.snowman.left && top < Game1.snowman.bottom && bottom > Game1.snowman.top))
                {
                    state = ContestantState.CHASING;
                    playerAnimation.Stop();
                }
                else
                {
                    velocity = Vector2.Zero;
                    moving = true;
                    playerAnimation.Clip = 1;
                    if (playerAnimation.Frame == 6 && !hit)
                    {
                        Game1.snowman.DealDamage(damage);
                        hit = true;
                    }
                    else if (playerAnimation.Frame != 6)
                    {
                        hit = false;
                    }
                }
            }

            base.Update(gameTime);

            if (!isAlive)
            {
                Game1.snowman.KillCount++;
                Game1.AddEffect("darecontestant_death", position);
            }
        }

        public override void DealDamage(int damage)
        {
            if (state == ContestantState.CHASING || state == ContestantState.ATTACKING)
            {
                base.DealDamage(damage);
            }
        }
    }
}
