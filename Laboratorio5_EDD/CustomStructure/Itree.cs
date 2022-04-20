using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomStructure
{
    public interface Itree<T>
    {
        void Add(T value);
        void Delete(T Value);
        T Search(T Value,Func<T,T,bool> Filtro);
        List<T> inorder();
    }
}
