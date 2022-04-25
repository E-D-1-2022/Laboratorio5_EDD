using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomStructure.Tree_2_3
{
    public class Node<T>
    {
        public Node<T> left { get; set; }
        public Node<T> mid { get; set; }
        public Node<T> right { get; set; }
        public T leftElement { get; set; }
        public T rightElement { get; set; }
        public Node()
        {
            left = null;
            mid = null;
            right = null;
            this.leftElement   = (T)Activator.CreateInstance(typeof(T));
            this.rightElement   = (T)Activator.CreateInstance(typeof(T));
        }

        public Node(T leftElement, T rightElement)
        {
            this.leftElement = leftElement;
            this.rightElement = rightElement;
            left = null;
            mid = null;
            right = null;
        }

        public Node(T leftElement, T rightElement, Node<T> left, Node<T> mid)
        {
            this.leftElement = leftElement;
            this.rightElement = rightElement;
            this.left = left;
            this.mid = mid;
        }

        public bool isLeaf()
        {
            return left == null && mid == null && right == null;
        }
        public bool is2Node()
        {
            if (rightElement == null || rightElement.Equals(default(T))) { rightElement = (T)Activator.CreateInstance(typeof(T)); }

            return rightElement.Equals(null) || rightElement.Equals(default(T));
        }
        public bool is3Node()
        {
            if (rightElement == null || rightElement.Equals(default(T))) { rightElement = (T)Activator.CreateInstance(typeof(T)); }
            return !rightElement.Equals(null) || !rightElement.Equals(default(T));
        }
        public bool isBalanced()
        {

            var balanced = false;
            if (isLeaf())
            {
                // Si estamos en el nivel más profundo (la hoja), seguramente esta bien equilibrado 
                balanced = true;
            }
            else
            {
                if (left == null) { left = new Node<T>(); }
                if (mid == null) { mid = new Node<T>(); }
                if (right == null) { right = new Node<T>(); }
                if (!left.leftElement.Equals(default(T)) && !mid.leftElement.Equals(default(T)))
                {
                    if (!rightElement.Equals(default(T)))
                    {
                        // 3 Node<T>
                        if (!right.leftElement.Equals(default(T)))
                        {
                            balanced = true;
                        }
                    }
                    else
                    {
                        // 2 Node<T>
                        balanced = true;
                    }
                }
            }
            return balanced;
        }
        public T replaceMax()
        {

            T max = default(T);
            if (!isLeaf())
            {
                // Caso recursivo, no estamos en el nivel más profundo
                if (!rightElement.Equals(default(T)))
                {
                    if (right == null) { right = new Node<T>(); }
                    max = right.replaceMax();
                }
                else
                {
                    if (mid == null) { mid = new Node<T>(); }
                    max = mid.replaceMax();
                }
            }
            else
            {
                // Estamos en el nivel más profundo del árbol
                if (!rightElement.Equals(default(T)))
                {
                    max = rightElement;
                    rightElement=default(T);
                }
                else
                {
                    max = rightElement;
                    leftElement = default(T);
                }
            }
            if (!isBalanced())
            {
                rebalance();
            }
            // Reequilibrar
            return max;
        }
        public T replaceMin()
        {
            T min = default(T);
            if (!isLeaf())
            {
                // Caso recursivo, mientras no llegamos al nivel mas profundo vamos bajando por la izquierda siempre 
            }
            else
            {
                // Tomamos el elemento y lo dejamos en buenas condiciones
                min = leftElement;
                leftElement = default(T);
                if (rightElement != null)
                {
                    // Había elemento a la derecha, lo pasamos a la izquierda
                    leftElement = rightElement;
                    rightElement = default(T);
                }
            }
            if (!isBalanced())
            {
                // A la derecha no había elemento, en la priemera subida rebalancea
                rebalance();
            }
            return min;
        }
        public void rebalance()
        {
            while (!isBalanced())
            {
                if (left.leftElement.Equals(default(T)))
                {
                    // El desequilibrio está en el hijo izquierdo
                    // Ponemos el elemento izquierdo del Nodo<T> actual como elemento izquierdo del hijo izquierdo
                    left.leftElement=leftElement;
                    // Ahora reemplazamos el elemento izquierdo del hijo mediano como el elemento izquierdo del Nodo<T> actual
                    leftElement=mid.leftElement;
                    // Si existe un elemento derecho en el hijo mediano, los desplazamos a la izquierda
                    if (!mid.rightElement.Equals(default(T)))
                    {
                        mid.leftElement=mid.rightElement;
                        mid.rightElement=default(T);
                    }
                    else
                    {
                        mid.leftElement=default(T);
                    }
                }
                else
                if (mid.leftElement.Equals(default(T)))
                {
                    // El desequilibrio está en el hijo de la derecha
                    // CASO CRÍTICO, cada Nodo<T> (hijo) del nivel más profundo tiene un solo elemento(el derecho está vacIo)
                    // El algoritmo tendrá que reequilibrar desde un nivel superior del árbol
                    if (rightElement.Equals(default(T)))
                    {
                        if (!left.leftElement.Equals(default(T))&& left.rightElement.Equals(default(T)) && mid.leftElement.Equals(default(T)))
                        {
                            rightElement=leftElement;
                            leftElement = left.leftElement;
                            // Quitamos a los niños actuales
                           left=(null);
                            mid=(null);
                            right=(null);
                        }
                        else
                        {
                            mid.leftElement = leftElement;
                            if (left.rightElement.Equals(default(T)))
                            {
                                leftElement = left.leftElement;
                                left.leftElement = default(T);
                            }
                            else
                            {
                                leftElement = left.rightElement;
                                left.rightElement=default(T);
                            }
                            if (left.leftElement.Equals(default(T)) && mid.leftElement.Equals(default(T)))
                            {
                               
                                left=(null);
                                mid=(null);
                                right=(null);
                            }
                        }
                    }
                    else
                    {
                        // Ponemos el elemento derecho del Nodo<T> actual como elemento izquierdo del hijo medio 
                        mid.leftElement = rightElement;
                        // Ponemos el elemento izquierdo del Nodo<T> actual como elemento izquierdo del hijo medio 
                        rightElement = right.leftElement;
                        // Si el hijo derecho, donde hemos tomado el último elemento tiene un elemento derecho, lo movemos
                        // A la izquierda del mismo hijo
                        if (!right.rightElement.Equals(default(T)))
                        {
                            right.leftElement = right.rightElement;
                            right.rightElement=default(T);
                        }
                        else
                        {
                            // Si no, dejamos que el hijo derecho se vacíe
                            right.leftElement = default(T);
                        }
                    }
                }
                else
                if (!rightElement.Equals(default(T)) && right.leftElement.Equals(default(T)))
                {
                    if (!mid.rightElement.Equals(default(T)))
                    {
                        // (1)
                        right.leftElement = rightElement;
                        rightElement=mid.rightElement;
                        mid.rightElement=default(T);
                    }
                    else
                    {
                        // (2)
                        mid.rightElement=rightElement;
                        rightElement=default(T);
                    }
                }
            }
        }
    }
}
