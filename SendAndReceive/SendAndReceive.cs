// *** SendandReceive ***

// This example expands the previous Receive example. The Arduino will now send back a status.
// It adds a demonstration of how to:
// - Handle received commands that do not have a function attached
// - Receive a command with a parameter from the Arduino

using System;
using System.Threading;
using CommandMessenger;
using CommandMessenger.TransportLayer;

namespace SendAndReceive
{
    // This is the list of recognized commands. These can be commands that can either be sent or received. 
    // In order to receive, attach a callback function to these events
    enum Command
    {
        OnLed,        // Command to turn on a LED
        OffLed,        // Command to turn off a LED
        ReadSensors,   // Command to start reading sensors
        ResetTower,    // Command to reset the hit state of the towers
    };

    public class SendAndReceive
    {
        public bool RunLoop { get; set; }
        private SerialTransport _serialTransport;
        private CmdMessenger _cmdMessenger;
        //private bool _ledState;
        //private int _count;
        int ledindex;
        int towerindex;
        //private bool[] tstate; //array of tower states

        // Setup function
        public void Setup()
        {
            //_ledState = false;
            ledindex = 0;
            towerindex = 0;

            // Create Serial Port object
            // Note that for some boards (e.g. Sparkfun Pro Micro) DtrEnable may need to be true.
            _serialTransport = new SerialTransport
            {
                CurrentSerialSettings = { PortName = "COM6", BaudRate = 115200, DtrEnable = false } // object initializer
            };

            // Initialize the command messenger with the Serial Port transport layer
            _cmdMessenger = new CmdMessenger(_serialTransport) 
                {
                    BoardType = BoardType.Bit16 // Set if it is communicating with a 16- or 32-bit Arduino board
                };

            // Tell CmdMessenger if it is communicating with a 16 or 32 bit Arduino board

            // Attach the callbacks to the Command Messenger
            AttachCommandCallBacks();
            
            // Start listening
            _cmdMessenger.StartListening();                                
        }

        // Loop function
        public void Loop()
        {
            //_count++;

            // Create command
            var SetOnLed = new SendCommand((int)Command.OnLed,ledindex);
            var SetOffLed = new SendCommand((int)Command.OffLed, ledindex);
            var ResetTower = new SendCommand((int)Command.ResetTower, towerindex);
            var ReadSensors = new SendCommand((int)Command.ReadSensors);               


            // Send command
            _cmdMessenger.SendCommand(SetOnLed);
            _cmdMessenger.SendCommand(SetOffLed);
            _cmdMessenger.SendCommand(ResetTower);
            _cmdMessenger.SendCommand(ReadSensors);
            
            // Wait for 1 second and repeat
            Thread.Sleep(1000);
            //_ledState = !_ledState;                                        // Toggle led state  

            // if (_count > 100) RunLoop = false;                             // Stop loop after 100 rounds
        }

        // Exit function
        public void Exit()
        {
            // Stop listening
            _cmdMessenger.StopListening();

            // Dispose Command Messenger
            _cmdMessenger.Dispose();

            // Dispose Serial Port object
            _serialTransport.Dispose();

            // Pause before stop
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }

        /// Attach command call backs. 
        private void AttachCommandCallBacks()
        {
            _cmdMessenger.Attach(OnUnknownCommand);
            //_cmdMessenger.Attach((int)Command.OnLed, SetOnLed);
            //_cmdMessenger.Attach((int)Command.OffLed, SetOnLed);
            _cmdMessenger.Attach((int)Command.ReadSensors, OnReadSensors);
            //_cmdMessenger.Attach((int)Command.ResetTower, SetOnLed);

        }

        /// Executes when an unknown command has been received.
        void OnUnknownCommand(ReceivedCommand arguments)
        {
            Console.WriteLine("Command without attached callback received");
        }

        // Callback function that prints the Arduino status to the console
        void OnReadSensors(ReceivedCommand arguments)
        {
            Console.Write("Tower status: ");
            Console.WriteLine(arguments.ReadStringArg());
        }
    }
}
