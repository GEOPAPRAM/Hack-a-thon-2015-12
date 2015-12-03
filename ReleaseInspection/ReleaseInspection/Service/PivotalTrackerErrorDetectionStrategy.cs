using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace NewVoiceMedia.Tools.ReleaseInspection.Service
{
    class PivotalTrackerErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        public bool IsTransient(Exception ex)
        {
            if (ex is JsonFx.Serialization.SerializationException)
            {
                Trace.WriteLine("TransientExceptionDetected!");
                return true;
            }

            return false;
        }
    }
}
