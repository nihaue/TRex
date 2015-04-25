using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRex.Metadata
{
    public enum TriggerTypes
    {
        // "x-ms-scheduler-trigger": "poll"
        Poll,

        // "x-ms-scheduler-trigger": "push"
        Push
    }
}
