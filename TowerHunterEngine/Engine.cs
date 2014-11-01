using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Reflection;
using MonoGameConsole;

namespace TowerHunterEngine
{
    public class Engine : Game
    {
        private readonly Properties.Settings Settings = 
            Properties.Settings.Default;

        private Point GAMERES;
        private Point FIELDSIZE;
        private int BOMBS;
        private int SCALE;
        private int CORRECTION_SCALE;
        private string PORT;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Playfield.Field playField;
        private PlayerFeedback.Timer Timer;
        private PlayerFeedback.InfoView Info;
        private GameConsole console;

        private List<string> DebugLine;
        private SpriteFont font;

        private Robot.Connection EV3Connection;

        public Engine()
            : base()
        {
            // load settings
            GAMERES = new Point(Settings.Resolution.X,
                                Settings.Resolution.Y);

            FIELDSIZE = new Point(Settings.FieldSize.X,
                                  Settings.FieldSize.Y);

            BOMBS = Settings.Bombs;
            SCALE = Settings.Scale;
            CORRECTION_SCALE = Settings.CorrectionScale;
            PORT = Settings.Port;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = Settings.Resolution.Y;
            graphics.PreferredBackBufferWidth = Settings.Resolution.X;
            graphics.ApplyChanges();

            this.playField = new Playfield.Field(this, GAMERES, FIELDSIZE, BOMBS);
            Components.Add(playField);

            Vector2 TimerPosition = new Vector2();
            TimerPosition.X = (float)(GAMERES.X - 115);
            TimerPosition.Y = (float)(GAMERES.Y - 55);
            this.Timer = new PlayerFeedback.Timer(this, 10, TimerPosition);
            Components.Add(Timer);
            Timer.IsEnabled = true;

            this.Info = new PlayerFeedback.InfoView(this);
            Components.Add(Info);

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

            EV3Connection = new Robot.Connection(PORT);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("font");
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Point directions = Player.Input.GetDirections(SCALE, CORRECTION_SCALE);
            EV3Connection.SendWheelData(directions.X, directions.Y);

            if (Timer.Elapsed)
            {
                // game over
            }

            if (EV3Connection.LastMessage != null)
            {
                if (EV3Connection.LastMessage.ValueAsText == "bombhit")
                {
                    // dismantle bomb
                }
            }
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
                new Utils.ConsoleCommands.RandomizeField(playField),
                new Utils.ConsoleCommands.ChangeCellType(playField),
                new Utils.ConsoleCommands.ResetCell(playField),
                new Utils.ConsoleCommands.Reset(this)
            };
            console.AddCommand(commands);

            console.Options.Prompt = ">";
            console.Options.BackgroundColor = new Color(0, 0, 0, 190);
        }
    }
}
