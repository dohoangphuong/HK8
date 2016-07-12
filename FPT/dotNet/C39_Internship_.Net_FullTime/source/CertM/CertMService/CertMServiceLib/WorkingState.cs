using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertMServiceLib
{
    /// <summary>
    /// service working status
    /// </summary>
    public enum WorkingState
    {
        Error, // working finished and result is null or can not be excute
        Success, // working success, return true
        Idle, // doing nothing
        Working, // being work
        Unknown// unknow state
    }
}
