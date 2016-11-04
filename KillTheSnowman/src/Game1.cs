using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KillTheSnowman
{
	enum GameState
	{
		INITIALIZE = 0,
		SPLASHSCREEN = 1,
		TITLE = 2,
		PLAYING = 3,
		PAUSED = 4,
		GAMEOVER = 5
	}

	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		public static void print(object obj)
		{
			System.Diagnostics.Debug.WriteLine(obj);
		}

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public static int WINDOW_WIDTH
		{
			get
			{
				return 1200;
			}
		}
		public static int WINDOW_HEIGHT
		{
			get
			{
				return 675;
			}
		}

		KeyboardState oldKeyState;
		GameState gameState;
		bool[] initialized = { false, false, false, false, false, false };
		bool[] loaded = { false, false, false, false, false, false };
		SpriteFont gameFont;

		/* gameState == GameState.SPLASHSCREEN */
		/***************************************/

		/* gameState == GameState.TITLE */
		ButtonString gameTitle;
		ButtonString comment;
		ButtonString playButton;
		ButtonString quitButton;
		/********************************/

		/* gameState == GameState.PLAYING */
		public static Snowman snowman { get; private set; }

		static List<Snowball> snowballs;
		//static SoundEffect snowballThrowSound;
		public static void ThrowSnowball(Vector2 position, Vector2 direction, int damage, Player player)
		{
			direction.Normalize();
			snowballs.Add(new Snowball(position, direction, damage, player));
			//snowballThrowSound.Play(0.5f, 0.0f, 0.0f);
		}

		static List<Effect> effects;
		public static void AddEffect(string effectName, Vector2 position)
		{
			effects.Add(new Effect(effectName, position));
		}

		public static Vector2 SnowmanPosition { get { return snowman.position; } }

		float contestantSpawnTime;
		float spawnTime;
		List<DareContestant> contestants;

		SnowFall furthestSnowFall;
		SnowFall farSnowFall;
		SnowFall nearSnowFall;
		SnowFall nearestSnowFall;

		public static int Level { get; private set; }
		static bool levelUp;
		/**********************************/

		/* gameState == GameState.PAUSED */
		ButtonString menuLabel;
		ButtonString resumeButton;
		ButtonString quitFromPauseButton;
		/*********************************/

		/* gameState == GameState.GAMEOVER */
		ButtonString gameOverLabel;
		ButtonString scoreLabel;
		ButtonString quitFromGameOverButton;
		/***********************************/

		public Game1()
			: base()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
			graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;

			IsMouseVisible = true;

			Content.RootDirectory = "Content";

			// Set device frame rate to 30 fps.
			TargetElapsedTime = TimeSpan.FromSeconds(1 / 30.0);

			oldKeyState = Keyboard.GetState();
			gameState = GameState.INITIALIZE;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			initialized[(int)gameState] = true;
			if (gameState == GameState.INITIALIZE)
			{
				LoadContent();
				gameState = GameState.TITLE;
			}

			if (gameState == GameState.SPLASHSCREEN)
			{

			}
			else if (gameState == GameState.TITLE)
			{
				gameTitle = new ButtonString();
				comment = new ButtonString();
				playButton = new ButtonString();
				quitButton = new ButtonString();
			}
			else if (gameState == GameState.PLAYING)
			{
				GridManager.Initialize(20, 15);

				snowballs = new List<Snowball>();

				contestantSpawnTime = 0;
				spawnTime = 3.0f;
				contestants = new List<DareContestant>();

				snowman = new Snowman();

				effects = new List<Effect>();

				furthestSnowFall = new SnowFall(SnowDepth.FURTHEST);
				farSnowFall = new SnowFall(SnowDepth.FAR);
				nearSnowFall = new SnowFall(SnowDepth.NEAR);
				nearestSnowFall = new SnowFall(SnowDepth.NEAREST);

				Level = 0;
				levelUp = false;

				EffectBank.Initialize();
			}
			else if (gameState == GameState.PAUSED)
			{
				menuLabel = new ButtonString();
				resumeButton = new ButtonString();
				quitFromPauseButton = new ButtonString();
			}
			else if (gameState == GameState.GAMEOVER)
			{
				gameOverLabel = new ButtonString();
				scoreLabel = new ButtonString();
				quitFromGameOverButton = new ButtonString();
			}

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			loaded[(int)gameState] = true;
			if (gameState == GameState.INITIALIZE)
			{
				// Create a new SpriteBatch, which can be used to draw textures.
				spriteBatch = new SpriteBatch(GraphicsDevice);
				gameFont = Content.Load<SpriteFont>("SpriteFonts/SpriteFont1");
			}

			if (gameState == GameState.SPLASHSCREEN)
			{

			}
			else if (gameState == GameState.TITLE)
			{
				gameTitle.Initialize(gameFont, "KILL THE SNOWMAN", new Vector2(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2 - 70));
				gameTitle.scale = 2.0f;
				comment.Initialize(gameFont, "Hey, Snowman is a\ngreat theme, idiot!", new Vector2(WINDOW_WIDTH * 4 / 5, WINDOW_HEIGHT * 3 / 4));
				comment.rotation = 0.12f;
				comment.scale = 0.5f;
				playButton.Initialize(gameFont, "Play", new Vector2(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2));
				quitButton.Initialize(gameFont, "Quit", new Vector2(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2 + 50));
			}
			else if (gameState == GameState.PLAYING)
			{
				Snowball.LoadContent(Content);

				snowman.LoadContent(Content, "snowman_sheet");

				//snowballThrowSound = Content.Load<SoundEffect>("Audio/ThrowSnowball");

				furthestSnowFall.LoadContent(Content, "snowFlake_01");
				farSnowFall.LoadContent(Content, "snowFlake_01");
				nearSnowFall.LoadContent(Content, "snowFlake_01");
				nearestSnowFall.LoadContent(Content, "snowFlake_01");

				EffectBank.AddEffect(Content, "bloodeffect_01", 4, 1, 16);
				EffectBank.AddEffect(Content, "darecontestant_death", 8, 1, 24);
			}
			else if (gameState == GameState.PAUSED)
			{
				menuLabel.Initialize(gameFont, "Paused", new Vector2(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2 - 60));
				menuLabel.scale = 1.5f;
				resumeButton.Initialize(gameFont, "Play", new Vector2(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2));
				quitFromPauseButton.Initialize(gameFont, "Menu", new Vector2(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2 + 50));
			}
			else if (gameState == GameState.GAMEOVER)
			{
				gameOverLabel.Initialize(gameFont, "Game Over", new Vector2(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2 - 110));
				gameOverLabel.scale = 1.5f;
				scoreLabel.Initialize(gameFont, "Level: " + Level.ToString() + "\nSnowballs Thrown: " + snowman.SnowballsThrown.ToString() + "\nDownvotes Thwarted: " +
					snowman.KillCount.ToString(), new Vector2(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2));
				quitFromGameOverButton.Initialize(gameFont, "Menu", new Vector2(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2 + 100));
			}
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (gameState == GameState.SPLASHSCREEN)
			{

			}
			else if (gameState == GameState.TITLE)
			{
				playButton.Update(gameTime);
				quitButton.Update(gameTime);
				if (playButton.clicked)
				{
					gameState = GameState.PLAYING;
					Initialize();
					LoadContent();
				}
				else if (quitButton.clicked)
				{
					Exit();
				}
			}
			else if (gameState == GameState.PLAYING)
			{
				if (Keyboard.GetState().IsKeyDown(Keys.Escape) && oldKeyState.IsKeyUp(Keys.Escape))
				{
					gameState = GameState.PAUSED;
					Initialize();
					LoadContent();
				}

				if (snowman.KillCount % 2 == 0)
				{
					if (levelUp)
					{
						Level++;
						spawnTime *= 0.9f;
						levelUp = false;
					}
				}
				else
				{
					levelUp = true;
				}

				if (contestantSpawnTime >= spawnTime)
				{
					DareContestant contestant = new DareContestant();
					contestant.LoadContent(Content, "darecontestant_sheet");
					contestants.Add(contestant);
					contestantSpawnTime = 0.0f;
				}
				contestantSpawnTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

				for (int i = contestants.Count - 1; i >= 0; i--)
				{
					contestants[i].Update(gameTime);
					if (!contestants[i].isAlive)
					{
						DareContestant temp = contestants[i];
						contestants.RemoveAt(i);
						temp = null;
					}

				}

				for (int i = snowballs.Count - 1; i >= 0; i--)
				{
					snowballs[i].Update(gameTime);
					if (!snowballs[i].isAlive)
					{
						Snowball temp = snowballs[i];
						snowballs.RemoveAt(i);
						temp = null;
					}
				}

				snowman.Update(gameTime);

				for (int i = effects.Count - 1; i >= 0; i--)
				{
					effects[i].Update(gameTime);
					if (!effects[i].isAlive)
					{
						Effect temp = effects[i];
						effects.RemoveAt(i);
						temp = null;
					}
				}

				furthestSnowFall.Update(gameTime);
				farSnowFall.Update(gameTime);
				nearSnowFall.Update(gameTime);
				nearestSnowFall.Update(gameTime);

				if (!snowman.isAlive)
				{
					gameState = GameState.GAMEOVER;
					Initialize();
					LoadContent();
				}
			}
			else if (gameState == GameState.PAUSED)
			{
				resumeButton.Update(gameTime);
				quitFromPauseButton.Update(gameTime);
				if (resumeButton.clicked)
				{
					gameState = GameState.PLAYING;
				}
				else if (quitFromPauseButton.clicked)
				{
					gameState = GameState.TITLE;
					Initialize();
					LoadContent();
				}
			}
			else if (gameState == GameState.GAMEOVER)
			{
				quitFromGameOverButton.Update(gameTime);
				if (quitFromGameOverButton.clicked)
				{
					gameState = GameState.TITLE;
					Initialize();
					LoadContent();
				}
			}
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White);

			spriteBatch.Begin();

			if (gameState == GameState.SPLASHSCREEN)
			{

			}
			else if (gameState == GameState.TITLE)
			{
				gameTitle.Draw(spriteBatch);
				comment.Draw(spriteBatch);
				playButton.Draw(spriteBatch);
				quitButton.Draw(spriteBatch);
			}
			else if (gameState == GameState.PLAYING)
			{
				foreach (Snowball ball in snowballs)
				{
					ball.Draw(spriteBatch);
				}

				snowman.Draw(spriteBatch);

				foreach (DareContestant contestant in contestants)
				{
					contestant.Draw(spriteBatch);
				}

				foreach (Effect effect in effects)
				{
					effect.Draw(spriteBatch);
				}

				furthestSnowFall.Draw(spriteBatch);
				farSnowFall.Draw(spriteBatch);
				nearSnowFall.Draw(spriteBatch);
				nearestSnowFall.Draw(spriteBatch);
			}
			else if (gameState == GameState.PAUSED)
			{
				menuLabel.Draw(spriteBatch);
				resumeButton.Draw(spriteBatch);
				quitFromPauseButton.Draw(spriteBatch);
			}
			else if (gameState == GameState.GAMEOVER)
			{
				gameOverLabel.Draw(spriteBatch);
				scoreLabel.Draw(spriteBatch);
				quitFromGameOverButton.Draw(spriteBatch);
			}

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
