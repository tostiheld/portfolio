using System;

namespace Roadplus.Server.API
{
    public class NewActivityEventArgs : EventArgs
    {
        public Activity NewActivity { get; private set; }

        public NewActivityEventArgs(Activity activity) : base()
        {
            NewActivity = activity;
        }
    }
}

