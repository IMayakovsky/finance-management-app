using System.Threading.Tasks;

namespace FinanceManagement.MessageService.Processors
{
    internal abstract class BaseProccessor
    {
        /// <summary>
        /// Validates Data in database, creates notification if validation was faild. 
        /// </summary>
        /// <returns></returns>
        public abstract Task Process();

        public abstract Task Stop();
    }
}
