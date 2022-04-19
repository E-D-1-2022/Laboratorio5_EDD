using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomStructures
{
    public interface IBTree<T>
    {
        void add(T value);
        void delete(T value);
        T find(T value);
    }
}
