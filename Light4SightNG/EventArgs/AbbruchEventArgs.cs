﻿using System;

namespace Light4SightNG
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
