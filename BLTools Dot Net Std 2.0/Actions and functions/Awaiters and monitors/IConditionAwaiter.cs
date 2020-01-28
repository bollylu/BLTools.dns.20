using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLTools
{
    public interface IConditionAwaiter
    {
        bool Execute(int pollingDelayInMsec = 5);
        Task<bool> ExecuteAsync(int pollingDelayInMsec = 5);
    }
}
