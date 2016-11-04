using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KillTheSnowman
{
    class ButtonString
    {
        SpriteFont font;
        string text;
        Vector2 position;
        Vector2 origin;
        public float rotation;
        public float scale;
        Rectangle mousePosition;
        Rectangle button;
        bool mouseDown = false;
        public bool clicked = false;
        MouseState oldState;

        public void Initialize(SpriteFont spriteFont, string textDisplay, Vector2 textPosition)
        {
            clicked = false;
            font = spriteFont;
            text = textDisplay;
            position = textPosition;
            origin = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);
            rotation = 0.0f;
            scale = 1.0f;
            oldState = Utils.Mouse.GetState();
        }

        public void Update(GameTime gameTime)
        {
            button = new Rectangle((int)position.X - (int)(font.MeasureString(text).X / 2), (int)position.Y - (int)(font.MeasureString(text).Y / 2), (int)font.MeasureString(text).X, (int)font.MeasureString(text).Y);
            mousePosition = new Rectangle(Utils.Mouse.GetState().X, Utils.Mouse.GetState().Y, 1, 1);
            if (mousePosition.Intersects(button))
            {
                scale = 1.2f;
                if (Utils.Mouse.GetState().LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
                {
                    mouseDown = true;
                }
                if (mouseDown && Utils.Mouse.GetState().LeftButton == ButtonState.Released)
                {
                    clicked = true;
                    mouseDown = false;
                }
            }
            else
            {
                scale = 1.0f;
                if (Utils.Mouse.GetState().LeftButton == ButtonState.Pressed || Utils.Mouse.GetState().LeftButton == ButtonState.Released)
                {
                    mouseDown = false;
                }
            }
            oldState = Utils.Mouse.GetState();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            origin = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);
            spriteBatch.DrawString(font, text, position, Color.Black, rotation, origin, scale, SpriteEffects.None, 1.0f);
        }
    }
}
