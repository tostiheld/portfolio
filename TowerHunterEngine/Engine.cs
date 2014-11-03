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
        private Texture2D qrcode;
        
        private readonly Properties.Settings Settings = 
            Properties.Settings.Default;

        private Point Resolution;
        private Point Fieldsize;
        private int BombAmount;
        private int Scale;
        private int CorrectionScale;
        private string Port;

        public bool HasFrameCounter = false;
        private bool IsGameOver = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Playfield.Field playField;
        private PlayerFeedback.CountdownTimer Timer;
        private PlayerFeedback.InfoView Info;
        private PlayerFeedback.GameOverView GameOverScreen;
        private Player.Data PlayerData;
        private GameConsole console;

        private List<string> DebugLine;
        private SpriteFont font;

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

            BombAmount = Settings.Bombs;
            Scale = Settings.Scale;
            CorrectionScale = Settings.CorrectionScale;
            Port = Settings.Port;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = Settings.Resolution.Y;
            graphics.PreferredBackBufferWidth = Settings.Resolution.X;
            graphics.ApplyChanges();

            PlayerData = new Player.Data(100);

            this.playField = new Playfield.Field(this, Resolution, Fieldsize, BombAmount);

            Point TimerPosition = new Point();
            TimerPosition.X = 5;
            TimerPosition.Y = (Resolution.Y - 83);
            this.Timer = new PlayerFeedback.CountdownTimer(this, 100, TimerPosition);
            Timer.IsEnabled = true;
            
            this.Info = new PlayerFeedback.InfoView(this, PlayerData, new Point(220, Resolution.Y - 60));
            
            this.GameOverScreen = new PlayerFeedback.GameOverView(this);

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            Components.Add(playField);
            Components.Add(Timer);
            Components.Add(Info);
            Components.Add(GameOverScreen);

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

            Point res = new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            qrcode = Utils.RuntimeTextures.GenerateQRCode(GraphicsDevice, "test");
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            /*if (EV3Connection.Status == Robot.RobotStatus.Homed)
            {
                this.Reset();
            }*/

            Info.Data = PlayerData;

            if (Timer.Elapsed && !IsGameOver)
                GameOver();

            if (PlayerData.HitPoints <= 0 && !IsGameOver)
            {
                Timer.IsEnabled = false;
                GameOver();
            }

            /*if (EV3Connection.Status == Robot.RobotStatus.HitWall)
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
            spriteBatch.Draw(qrcode, new Rectangle(50, 50, qrcode.Width, qrcode.Height), Color.White);

            spriteBatch.End();

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone); // these are settings to scale up textures without blurring them

            spriteBatch.Draw(qrcode, new Rectangle(50, 50, 300, 300), Color.White);

            spriteBatch.End();
        }

        public void Reset(int initialBombs, int time = 180, int maxhp = 100)
        {
            this.BombAmount = initialBombs;
            Reset(time, maxhp);
        }
        public void Reset(int time = 180, int maxhp = 100)
        {
            this.IsGameOver = false;

            playField.Reset(BombAmount);
            Timer.Reset(time);
            PlayerData = new Player.Data(maxhp);
            Info.Reset(PlayerData);
            GameOverScreen.Reset();

            ResetConsoleCommands();

            Timer.IsEnabled = true;
        }

        private void GameOver()
        {
            IsGameOver = true;
            foreach (Utils.AvailableColor ac in playField.AvailableColors.Values)
            {
                playField.ResetCell(ac.Value);
            }
            playField.MustUpdate = true;
            GameOverScreen.Begin();
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
