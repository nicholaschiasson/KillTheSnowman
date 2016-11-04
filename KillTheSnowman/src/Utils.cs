namespace KillTheSnowman
{
	namespace Utils
	{
		public static class Mouse
		{
			public static Microsoft.Xna.Framework.Input.MouseState GetState()
			{
				Microsoft.Xna.Framework.Input.MouseState ms = Microsoft.Xna.Framework.Input.Mouse.GetState();
				return new Microsoft.Xna.Framework.Input.MouseState(ms.X * 2, ms.Y * 2, ms.ScrollWheelValue,
																	ms.LeftButton, ms.MiddleButton, ms.RightButton,
																	ms.XButton1, ms.XButton2);
			}
		}
	}
}
