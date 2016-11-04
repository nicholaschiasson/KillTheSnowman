#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace KillTheSnowman
{
	enum SnowDepth
	{
		NONE,
		FURTHEST,
		FAR,
		NEAR,
		NEAREST
	}

	struct SnowFlake
	{
		public float x { get; set; }
		public float y { get; set; }
		public float sway { get; set; }
		public bool swayDir { get; set; }
		public float rotation { get; set; }
		public bool rotationSpeed { get; set; }
	}

	class SnowFall
    {
        static int MAX_SNOWFLAKES = 200;

        // Constructor phase variables
        SnowDepth snowDepth = SnowDepth.NONE;

        // Initialize phase variables
        float breeze;
        float alpha;
        float scale;
        List<SnowFlake> snowFlakes;
        Random rand;

        // LoadContent phase variables
        Texture2D snowFlake;
        Vector2 origin;

        public SnowFall(SnowDepth depth)
        {
            snowDepth = depth;

            Initialize();
        }

        public void Initialize()
        {
            switch (snowDepth)
            {
                case SnowDepth.NONE:
                    breeze = 0.0f;
                    alpha = 1.0f;
                    scale = 0.0f;
                    break;
                case SnowDepth.FURTHEST:
                    breeze = 0.025f;
                    alpha = 0.2f;
                    scale = 0.25f;
                    break;
                case SnowDepth.FAR:
                    breeze = 0.04f;
                    alpha = 0.35f;
                    scale = 0.5f;
                    break;
                case SnowDepth.NEAR:
                    breeze = 0.055f;
                    alpha = 0.5f;
                    scale = 1.0f;
                    break;
                case SnowDepth.NEAREST:
                    breeze = 0.07f;
                    alpha = 0.65f;
                    scale = 1.5f;
                    break;
                default:
                    alpha = 0.0f;
                    scale = 1.0f;
                    break;
            }
            snowFlakes = new List<SnowFlake>();
            rand = new Random();

            int snowFlakeCount = rand.Next(MAX_SNOWFLAKES / 2, MAX_SNOWFLAKES);
            for (int i = 0; i < MAX_SNOWFLAKES; i++)
            {
                SnowFlake flake = new SnowFlake();
                flake.x = (float)rand.Next(Game1.WINDOW_WIDTH);
                flake.y = (float)rand.Next(Game1.WINDOW_HEIGHT);
                flake.sway = (float)rand.Next(100) / 100;
                flake.swayDir = rand.Next(1) == 1;
                flake.rotation = 0.01f * (float)rand.Next(100);
                flake.rotationSpeed = rand.Next(1) == 1;
                snowFlakes.Add(flake);
            }
        }

        public void LoadContent(ContentManager Content, string assetName)
        {
            snowFlake = Content.Load<Texture2D>(assetName);
            origin = new Vector2(snowFlake.Width / 2.0f, snowFlake.Height / 2.0f);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < snowFlakes.Count; i++)
            {
                SnowFlake temp = new SnowFlake();
                if (snowFlakes[i].x < 0 - snowFlake.Width)
                {
                    temp.x = Game1.WINDOW_WIDTH + snowFlake.Width;
                }
                else
                {
                    temp.x = snowFlakes[i].x - gameTime.ElapsedGameTime.Milliseconds * ((breeze * 0.75f) + snowFlakes[i].sway);
                }
                if (snowFlakes[i].y > Game1.WINDOW_HEIGHT + snowFlake.Height)
                {
                    temp.y = 0 - snowFlake.Height;
                }
                else
                {
                    temp.y = snowFlakes[i].y + gameTime.ElapsedGameTime.Milliseconds * (breeze * 1.125f);
                }
                temp.rotation = snowFlakes[i].rotation + gameTime.ElapsedGameTime.Milliseconds * (snowFlakes[i].rotationSpeed ? 0.005f : 0.00075f);
                snowFlakes[i] = temp;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (SnowFlake flake in snowFlakes)
            {
                spriteBatch.Draw(snowFlake, new Vector2(flake.x, flake.y), null, Color.Lerp(new Color(0.85f, 0.85f, 0.85f), Color.Transparent, alpha), flake.rotation, origin, scale, SpriteEffects.None, 0.0f);
            }
        }
    }
}
