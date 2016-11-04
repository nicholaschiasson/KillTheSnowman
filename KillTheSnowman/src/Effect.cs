#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace KillTheSnowman
{
    class Effect
    {
        Vector2 position;
        AnimatedTexture effectAnimation;

        public bool isAlive { get; private set; }

        public Effect(string effectName, Vector2 pos)
        {
            position = pos;
            effectAnimation = (AnimatedTexture)EffectBank.EffectAnimations[effectName].Clone();
            effectAnimation.Play();

            isAlive = true;
        }

        public void Update(GameTime gameTime)
        {
            if (effectAnimation.Frame == effectAnimation.framecount - 1)
            {
                isAlive = false;
                return;
            }
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            effectAnimation.UpdateFrame(elapsedSeconds);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            effectAnimation.DrawFrame(spriteBatch, position);
        }
    }
}
