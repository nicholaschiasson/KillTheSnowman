#region File Description
//-----------------------------------------------------------------------------
// AnimatedTexture.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

//Modified by Nicholas Chiasson
#endregion

#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace KillTheSnowman
{
    public class AnimatedTexture : ICloneable
    {
        public int framecount { get; private set; }
        private int clipcount;
        private Texture2D myTexture;
        private float TimePerFrame;
        public int Frame { get; private set; }
        private float TotalElapsed;
        private bool Paused;

        public float Rotation, Scale, Depth;
        public Vector2 Origin;

        public int Clip { get; set; }
        public int width { get { return myTexture.Width / framecount; } }
        public int height { get { return myTexture.Height / clipcount; } }

        public AnimatedTexture(float rotation,
            float scale, float depth)
        {
            this.Rotation = rotation;
            this.Scale = scale;
            this.Depth = depth;
        }
        public void Load(ContentManager content, string asset,
            int frameCount, int clipCount, int framesPerSec)
        {
            framecount = frameCount;
            clipcount = clipCount;
            myTexture = content.Load<Texture2D>(asset);
            TimePerFrame = (float)1 / framesPerSec;
            this.Origin = new Vector2(width / 2, height / 2);
            Frame = 0;
            Clip = 0;
            TotalElapsed = 0;
            Paused = false;
        }

        // class AnimatedTexture
        public void UpdateFrame(float elapsed)
        {
            if (Paused)
                return;
            TotalElapsed += elapsed;
            if (TotalElapsed > TimePerFrame)
            {
                Frame++;
                // Keep the Frame between 0 and the total frames, minus one.
                Frame = Frame % framecount;
                TotalElapsed -= TimePerFrame;
            }
        }

        // class AnimatedTexture
        public void DrawFrame(SpriteBatch batch, Vector2 screenPos)
        {
            DrawFrame(batch, Frame, Clip, screenPos);
        }
        public void DrawFrame(SpriteBatch batch, int frame, int clip, Vector2 screenPos)
        {
            Rectangle sourcerect = new Rectangle(width * frame, height * clip,
                width, height);
            batch.Draw(myTexture, screenPos, sourcerect, Color.White,
                Rotation, Origin, Scale, SpriteEffects.None, Depth);
        }

        public bool IsPaused
        {
            get { return Paused; }
        }
        public void Reset()
        {
            Frame = 0;
            TotalElapsed = 0f;
        }
        public void Stop()
        {
            Pause();
            Reset();
        }
        public void Play()
        {
            Paused = false;
        }
        public void Pause()
        {
            Paused = true;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
