#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace KillTheSnowman
{
    public class Player
    {
        protected bool moving;
        protected Vector2 velocity;

        public Vector2 position { get; protected set; }

        protected float rotation;
        protected float scale;
        protected float depth;
        protected AnimatedTexture playerAnimation;

        protected int frames;
        protected int framesPerSec;
        protected int clips;

        protected int health;
        protected int defense;
        protected int damage;
        public bool isAlive { get; protected set; }
        protected List<Player> gridTile;

        public int width { get { return playerAnimation.width; } }
        public int height { get { return playerAnimation.height; } }

        public int left { get { return (int)(position.X - width / 2); } }
        public int right { get { return (int)(position.X + width / 2); } }
        public int top { get { return (int)(position.Y - height / 2); } }
        public int bottom { get { return (int)(position.Y + height / 2); } }

        public Player()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            moving = false;
            velocity = Vector2.Zero;
            playerAnimation = new AnimatedTexture(rotation, scale, depth);

            health = 100;
            isAlive = true;
            gridTile = GridManager.GridTile(position);
            gridTile.Add(this);
        }

        public virtual void LoadContent(ContentManager Content, string assetName)
        {
            playerAnimation.Load(Content, assetName, frames, clips, framesPerSec);
        }

        public virtual void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float elapsedMilliseconds = (float)gameTime.ElapsedGameTime.Milliseconds;

            position += elapsedMilliseconds * velocity;
            Vector2 p = new Vector2(MathHelper.Clamp(position.X, width / 2, Game1.WINDOW_WIDTH - width / 2), MathHelper.Clamp(position.Y, height / 2, Game1.WINDOW_HEIGHT - height / 2));
            position = p;

            List<Player> tile = GridManager.GridTile(position);
            if (gridTile != tile)
            {
                gridTile.Remove(this);
                tile.Add(this);
                gridTile = tile;
            }

            if (!moving)
            {
                playerAnimation.Stop();
            }
            else
            {
                playerAnimation.Play();
            }
            if (health <= 0)
            {
                isAlive = false;
                gridTile.Remove(this);
            }
            playerAnimation.UpdateFrame(elapsedSeconds);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            playerAnimation.DrawFrame(spriteBatch, position);
        }

        public virtual void DealDamage(int damage)
        {
            health -= (int)(damage - MathHelper.Clamp(defense, 0, damage - 1));
        }
    }
}
