using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Reflection;
using MonoGameConsole;

namespace BombDefuserEngine
{
    public class Engine : Game
    {
        private readonly Properties.Settings Settings = 
            Properties.Settings.Default;

        private Point Resolution;
        private Point Fieldsize;
        private int BombAmount;
        private int Scale;
        private int CorrectionScale;
        private string Port;

        public bool HasFrameCounter = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Playfield.Field playField;
        private PlayerFeedback.CountdownTimer Timer;
        private PlayerFeedback.InfoView Info;
        private PlayerFeedback.GameOverView GameOver;
        private Player.Data PlayerData;
        private GameConsole console;

        private List<string> DebugLine;
        private SpriteFont font;

        private Texture2D GroundTexture;

        //private Robot.Connection EV3Connection;

        public Engine()
            : base()
        {
            // load settings
            Resolution = new Point(Settings.Resolution.X,
                                Settings.Resolution.Y);

            Fieldsize = new Point(Settings.FieldSize.X,
                                  Settings.FieldSize.Y);

            BombAmount = Settings.Bombs;
            Scale = Settings.Scale;
            CorrectionScale = Settings.CorrectionScale;
            Port = Settings.Port;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = Settings.Resolution.Y;
            graphics.PreferredBackBufferWidth = Settings.Resolution.X;
            graphics.ApplyChanges();

            this.playField = new Playfield.Field(this, Resolution, Fieldsize, BombAmount);
            Components.Add(playField);

            Point TimerPosition = new Point();
            TimerPosition.X = 5;
            TimerPosition.Y = (Resolution.Y - 83);
            this.Timer = new PlayerFeedback.CountdownTimer(this, 10, TimerPosition);
            Components.Add(Timer);
            Timer.IsEnabled = true;

            PlayerData = new Player.Data(100);
            PlayerData.HitPoints = 100;
            PlayerData.Score = 100;

            this.Info = new PlayerFeedback.InfoView(this, PlayerData, new Point(220, Resolution.Y - 60));
            Components.Add(Info);

            this.GameOver = new PlayerFeedback.GameOverView(this);
            Components.Add(GameOver);

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

            //EV3Connection = new Robot.Connection(Port);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("Fonts/font");
            GroundTexture = Content.Load<Texture2D>("ground-texture2.png");
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Info.Data = PlayerData;

            if (Timer.Elapsed)
            {
                if (!GameOver.Playing)
                {
                    GameOver.Begin();
                }
            }

            /*
            if (EV3Connection.LastStatus == Robot.RobotStatus.Dismantling)
            {
                foreach (Utils.AvailableColor ac in playField.AvailableColors.Values)
                {
                    if (ac.Value == EV3Connection.LastColor &&
                        !ac.Available)
                    {
                        playField.ResetCell(EV3Connection.LastColor);
                    }
                }
            }

            if (EV3Connection.LastStatus == Robot.RobotStatus.Empty)
            {
                Point directions = Player.Input.GetDirections(Scale, CorrectionScale);
                EV3Connection.SendWheelData(directions.X, directions.Y);
            }*/
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LimeGreen);
            spriteBatch.Begin();
            spriteBatch.Draw(GroundTexture, new Rectangle(0, 0, 1024, 768), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
            spriteBatch.Begin();
#if DEBUG
            spriteBatch.DrawString(
                font, 
                DebugLine[0] + " v" + DebugLine[1],
                new Vector2(10, 10),
                Color.Red);
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
                new Utils.ConsoleCommands.Reset(this),
                new Utils.ConsoleCommands.SetHP(PlayerData),
                new Utils.ConsoleCommands.ToggleFrameCounter(this)
                //new Utils.ConsoleCommands.GetLastMessage(EV3Connection)
            };
            console.AddCommand(commands);

            console.Options.Prompt = ">";
            console.Options.BackgroundColor = new Color(0, 0, 0, 190);
        }
    }
}
