#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Reflection;
using MonoGameConsole;
using System.Windows.Forms;
#endregion

namespace TowerHunterEngine
{
    public class Engine : Game
    {
        // Field sizes affect framerates
        // Safe size = max (25, 15)
        private readonly Point GAMERES = new Point(1024, 768);
        private readonly Point FIELDSIZE = new Point(8, 6);
        private const int BOMBS = 5;
        private const bool FULLSCREEN = true;
        private const int SCALE = 40;
        private const int CORRECTION_SCALE = 20;
        private const string PORT = "com40";

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Playfield.Field playField;
        private Score.Timer Timer;

        private List<string> DebugLine;
        SpriteFont font;
        GameConsole console;

        Robot.Connection EV3Connection;

        public Engine()
            : base()
        {
            Window.IsBorderless = true;
            Window.Position = new Point(0, 0);
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = FULLSCREEN;
            graphics.PreferredBackBufferHeight = GAMERES.Y;
            graphics.PreferredBackBufferWidth = GAMERES.X;
            graphics.ApplyChanges();

            this.playField = new Playfield.Field(this, GAMERES, FIELDSIZE, BOMBS);
            Components.Add(playField);

            Vector2 TimerPosition = new Vector2();
            TimerPosition.X = (float)(GAMERES.X - 115);
            TimerPosition.Y = (float)(GAMERES.Y - 55);
            this.Timer = new Score.Timer(this, 60, TimerPosition);
            Components.Add(Timer);

            Timer.IsEnabled = true;

#if DEBUG
            Components.Add(new Utils.FrameCounter(this));
#endif

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
#if DEBUG
            DebugLine = new List<string>(2);
            DebugLine.Add("DEBUGGING");
            DebugLine.Add(Assembly.GetExecutingAssembly().GetName().Version.ToString());
#endif
            SetupConsole();

            //EV3Connection = new Robot.Connection(PORT);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            playField.AnimatedTextures.Add("bomb", new Utils.AnimatedTexture(Vector2.Zero, 0f, 1f, 0.5f));
            playField.AnimatedTextures["bomb"].Load(Content, "bomb", 13, 13);
            font = Content.Load<SpriteFont>("font");
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            Point directions = Player.PlayerInput.GetDirections(SCALE, CORRECTION_SCALE);
            //EV3Connection.SendWheelData(directions.X, directions.Y);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            base.Draw(gameTime);

            spriteBatch.Begin();
#if DEBUG
            spriteBatch.DrawString(font, DebugLine[0] + " v" + DebugLine[1], new Vector2(10, 10), Color.Red);
#endif
            spriteBatch.End();            
        }

        private void SetupConsole()
        {
            console = new GameConsole(this, spriteBatch);
            IConsoleCommand[] commands = new IConsoleCommand[]
            {
                new Utils.ConsoleCommands.RunFullTest()
            };
            console.AddCommand(commands);

#if DEBUG
            IConsoleCommand[] debugcommands = new IConsoleCommand[]
            {
                new Utils.ConsoleCommands.RandomizeField(playField),
                new Utils.ConsoleCommands.ChangeCellType(playField)
            };
            console.AddCommand(debugcommands);
#endif

            console.Options.Prompt = ">";
            console.Options.BackgroundColor = new Color(0, 0, 0, 190);
        }
    }
}
