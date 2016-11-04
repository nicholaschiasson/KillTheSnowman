#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace KillTheSnowman
{
	enum ProjectileType
	{
		BULLET,
		SNOWBALL
	}

    class Snowball
    {
        static Texture2D snowball;
        //static SoundEffect hit;

        public static void LoadContent(ContentManager Content)
        {
            snowball = Content.Load<Texture2D>("snowball_01");
            //hit = Content.Load<SoundEffect>("Audio/Explode");
        }

        Vector2 position;
        Vector2 velocity;
        int damage;

        Player thrower;

        public bool isAlive { get; private set; }

        public Snowball(Vector2 pos, Vector2 vel, int dam, Player player)
        {
            position = pos;
            velocity = vel;
            damage = dam;
            thrower = player;
            Initialize();
        }

        public void Initialize()
        {
            isAlive = true;
        }

        public void Update(GameTime gameTime)
        {
            List<Player> gridTile = GridManager.AreaAroundGrid(position);
            foreach (Player player in gridTile)
            {
                if (player != thrower &&
                    position.X < player.right &&
                    position.X + snowball.Width > player.left &&
                    position.Y < player.bottom &&
                    position.Y + snowball.Height > player.top)
                {
                    isAlive = false;
                    player.DealDamage(damage);
                    //hit.Play(0.5f, 0.5f, 0.0f);
                }
            }

            if (position.X < 0 - snowball.Width ||
                position.X > Game1.WINDOW_WIDTH ||
                position.Y < 0 - snowball.Height ||
                position.Y > Game1.WINDOW_HEIGHT)
            {
                isAlive = false;
            }
            float elapsedMilliseconds = (float)gameTime.ElapsedGameTime.Milliseconds;
            position += elapsedMilliseconds * velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(snowball, position, new Color(0.8f, 0.8f, 0.8f));
        }
    }
}
