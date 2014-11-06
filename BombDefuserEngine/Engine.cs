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
        private int InitialBombs;
        private int Scale;
        private int CorrectionScale;
        private string Port;
        private int initialTime;
        private int initialMaxHP;
        private int currentTime;

        public bool HasFrameCounter = false;
        private bool IsGameOver = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Playfield.Field playField;
        private PlayerFeedback.CountdownTimer Timer;
        private PlayerFeedback.InfoView Info;
        private PlayerFeedback.GameOverView GameOverScreen;
        private PlayerFeedback.Tutorial tutorial;
        private Player.Data PlayerData;
        private GameConsole console;

        private Texture2D GroundTexture;

        private Robot.Connection EV3Connection;

        public Engine()
            : base()
        {
            // load settings
            Resolution = new Point(Settings.Resolution.X,
                                Settings.Resolution.Y);

            Fieldsize = new Point(Settings.FieldSize.X,
                                  Settings.FieldSize.Y);

            InitialBombs = Settings.Bombs;
            Scale = Settings.Scale;
            CorrectionScale = Settings.CorrectionScale;
            Port = Settings.Port;
            initialTime = Settings.Time;
            initialMaxHP = 100;
            currentTime = initialTime;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = Settings.Resolution.Y;
            graphics.PreferredBackBufferWidth = Settings.Resolution.X;
            graphics.ApplyChanges();

            PlayerData = new Player.Data(initialMaxHP);

            this.playField = new Playfield.Field(this, Resolution, Fieldsize, InitialBombs);

            Point TimerPosition = new Point();
            TimerPosition.X = 5;
            TimerPosition.Y = (Resolution.Y - 83);
            this.Timer = new PlayerFeedback.CountdownTimer(this, initialTime, TimerPosition);
            Timer.IsEnabled = true;
            
            this.Info = new PlayerFeedback.InfoView(this, PlayerData, new Point(220, Resolution.Y - 60));
            
            this.GameOverScreen = new PlayerFeedback.GameOverView(this, PlayerData.Score);

            this.tutorial = new PlayerFeedback.Tutorial(this);

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            Components.Add(playField);
            Components.Add(Timer);
            Components.Add(Info);
            Components.Add(GameOverScreen);
            Components.Add(tutorial);
            
            base.Initialize();

            for (int i = 0; i < playField.InitialBombs; i++ )
            {
                playField.AddSpecialCell(Playfield.CellType.Bomb);
            }

            SetupConsole();

            EV3Connection = new Robot.Connection(Port);

            tutorial.Show(Timer);
        }

        protected override void LoadContent()
        {
            GroundTexture = Content.Load<Texture2D>("ground-texture2.png");

            Point res = new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!tutorial.IsShown)
            {
                GamePadState state = GamePad.GetState(PlayerIndex.One);
                if (EV3Connection.Home &&
                    state.Buttons.A == ButtonState.Pressed)
                {
                    this.Reset();
                }

                if (EV3Connection.Status == Robot.RobotStatus.Homed)
                {
                    EV3Connection.Home = true;
                }

                Info.Data = PlayerData;

                if (Timer.Elapsed && !IsGameOver)
                    GameOver();

                if (PlayerData.HitPoints <= 0 && !IsGameOver)
                {
                    Timer.IsEnabled = false;
                    GameOver();
                }

                if (playField.BombAmount <= 0)
                {
                    Timer.Reset((int)(currentTime * 0.8));

                    for (int i = 0; i < playField.InitialBombs; i++)
                    {
                        playField.AddSpecialCell(Playfield.CellType.Bomb);
                    }

                    Timer.IsEnabled = true;
                }

                if (EV3Connection.Status == Robot.RobotStatus.HitWall)
                {
                    PlayerData.HitPoints -= 10;
                    EV3Connection.Status = Robot.RobotStatus.Empty;
                }

                if (!EV3Connection.LastColorIsRead)
                {
                    if (!playField.IsColorAvailable(EV3Connection.LastColor))
                    {
                        playField.ResetCell(EV3Connection.LastColor);
                        EV3Connection.LastColorIsRead = true;
                        PlayerData.Score += 100;
                    }
                }

                if (GameOverScreen.HomingScreenVisible &&
                    EV3Connection.Status != Robot.RobotStatus.Homing)
                {
                    EV3Connection.SendHome();
                    EV3Connection.Status = Robot.RobotStatus.Empty;
                }

                if (EV3Connection.Status == Robot.RobotStatus.Empty && !IsGameOver)
                {
                    Point directions = Player.Input.GetDirections(Scale, CorrectionScale);
                    EV3Connection.SendWheelData(directions.X, directions.Y);
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LimeGreen);
            spriteBatch.Begin();
            spriteBatch.Draw(GroundTexture, new Rectangle(0, 0, 1024, 768), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Reset()
        {
            this.IsGameOver = false;
            this.currentTime = initialTime;

            EV3Connection.Home = false;
            EV3Connection.ResetHoming();

            playField.Reset(InitialBombs);
            Timer.Reset(initialTime);
            PlayerData = new Player.Data(initialMaxHP);
            Info.Reset(PlayerData);
            GameOverScreen.Reset();

            ResetConsoleCommands();

            tutorial.Show(Timer);
        }

        private void GameOver()
        {
            IsGameOver = true;
            EV3Connection.SetGameOver();
            EV3Connection.SendWheelData(0, 0);
            
            GameOverScreen.Begin(PlayerData.Score);
        }

        private void SetupConsole()
        {
            console = new GameConsole(this, spriteBatch);

            ResetConsoleCommands();

            console.Options.Prompt = ">";
            console.Options.ToggleKey = Keys.F1;
            console.Options.BackgroundColor = new Color(0, 0, 0, 190);
        }

        private void ResetConsoleCommands()
        {
            IConsoleCommand[] commands = new IConsoleCommand[]
            {
                new Utils.ConsoleCommands.RandomizeField(playField),
                new Utils.ConsoleCommands.ChangeCellType(playField),
                new Utils.ConsoleCommands.ResetCell(playField),
                new Utils.ConsoleCommands.Reset(this),
                new Utils.ConsoleCommands.SetHP(PlayerData),
                new Utils.ConsoleCommands.ToggleFrameCounter(this),
                new Utils.ConsoleCommands.AddTime(Timer)
                //new Utils.ConsoleCommands.GetLastMessage(EV3Connection)
            };

            if (console.Commands.Count > 3)
            {
                console.Commands.RemoveRange(3, commands.Length);
            }

            console.AddCommand(commands);
        }
    }
}
