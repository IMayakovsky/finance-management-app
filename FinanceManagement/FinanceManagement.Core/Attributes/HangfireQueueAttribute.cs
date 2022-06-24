using FinanceManagement.Core.Common.Constants;
using Hangfire.Common;
using Hangfire.States;
using System;

namespace FinanceManagement.Core.Attributes
{
    public class HangfireQueueAttribute : JobFilterAttribute, IElectStateFilter
    {
        public string Queue { get; }

        public HangfireQueueAttribute(string queue)
        {
            Queue = queue;
            Order = Int32.MaxValue;
        }

        public void OnStateElection(ElectStateContext context)
        {
            if (context.CandidateState is EnqueuedState enqueuedState)
            {
#if DEBUG || DEBUG_BACKEND_ONLY
                enqueuedState.Queue = HangfireConstants.SelfQueueName;
#else
                enqueuedState.Queue = Queue == HangfireConstants.Self ? HangfireConstants.SelfQueueName : Queue;
#endif
            }
        }
    }
}
