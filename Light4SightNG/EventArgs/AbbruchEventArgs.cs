using System;

namespace CalibrateLEDStimulator
{
    class AbbruchEventArgs : EventArgs
    {
        public readonly string info;

        internal AbbruchEventArgs(string sinfo)
        {
            this.info = sinfo;
        }
    }
}
