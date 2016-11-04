#region Using Statements
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
#endregion

namespace KillTheSnowman
{
    static class EffectBank
    {
        public static Dictionary<string, AnimatedTexture> EffectAnimations { get; private set; }

        public static void Initialize()
        {
            EffectAnimations = new Dictionary<string, AnimatedTexture>();
        }

        public static void AddEffect(ContentManager Content, string assetName, int frameCount, int clipCount, int framesPerSec)
        {
            EffectAnimations[assetName] = new AnimatedTexture(0.0f, 1.0f, 0.0f);
            EffectAnimations[assetName].Load(Content, assetName, frameCount, clipCount, framesPerSec);
        }
    }
}
