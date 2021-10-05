using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Libraries.Core.Collections
{
    public class Map<K, V>
        where K : notnull
        where V : notnull
    {
        public Map(IComparer<K>? comparer = null) { }

        public V? this[K key]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}
