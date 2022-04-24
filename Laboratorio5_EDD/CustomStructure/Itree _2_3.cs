using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomStructure
{
    public interface Itree_2_3<T>
    {
        bool Add(T value);
        bool Remove(T Value);
        T Find(T Value);
        List<T> inOrder();
        void Clear();
        bool Contains(T element);
        T findMax();
        T findMin();
    }
}
