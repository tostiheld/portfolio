using System;
using Roadplus.Server.EntityManagement;

namespace Roadplus.Server
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

