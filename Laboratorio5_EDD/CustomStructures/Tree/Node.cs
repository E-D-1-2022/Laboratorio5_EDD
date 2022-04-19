using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomStructures.Tree
{
    public class Node<T>
    {
        public Node(int n) {
            Array = new List<T>();
            hijos = new Node<T>[n];
        }

        public List<T> Array { get; set; }

        public Node<T>[] hijos { get; set; }
        public Node<T> Padre {get;set;}


    }
    }
}
