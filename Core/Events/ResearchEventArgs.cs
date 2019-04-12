﻿using System;

using Core.Enumerations;

namespace Core.Events
{
    public struct ResearchStatusInfo
    {
        public ResearchStatus Status { get; set; }
        public uint CompletedStepsCount { get; set; }

        public ResearchStatusInfo(ResearchStatus rs, uint st)
            : this()
        {
            Status = rs;
            CompletedStepsCount = st;
        }
    };

    public class ResearchEventArgs : EventArgs
    {
        public Guid ResearchID { get; private set; }

        public ResearchEventArgs(Guid id)
        {
            ResearchID = id;
        }
    }

    public delegate void ResearchStatusUpdateHandler(Object sender, ResearchEventArgs e);
}
