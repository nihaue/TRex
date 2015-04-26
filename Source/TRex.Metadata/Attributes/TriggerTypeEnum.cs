using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRex.Metadata
{
    /// <summary>
    /// Type of trigger implemented by the action in question, either Push or Poll.
    /// Controls the generation of the x-ms-scheduler-trigger vendor extension.
    /// </summary>
    public enum TriggerType
    {
        
        // "x-ms-scheduler-trigger": "poll"
        Poll,

        // "x-ms-scheduler-trigger": "push"
        Push
    }
}
