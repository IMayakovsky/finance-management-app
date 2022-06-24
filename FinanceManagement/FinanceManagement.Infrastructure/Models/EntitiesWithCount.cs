using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Models
{
    public class EntitiesWithCount<T>
    {
        public int Count { get; set; }
        public List<T> Entities { get; set; }

        public EntitiesWithCount()
        {

        }

        public EntitiesWithCount(int count, List<T> entities)
        {
            Count = count;
            Entities = entities;
        }
    }
}
