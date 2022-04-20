using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomStructure.AvlTree
{
    public class AvlTree<T> : Itree<T>
    {
        Func<T, T, int> Comparer;
        private int peso = 0;
        private Node<T> Raiz = null;

        public AvlTree(Func<T, T, int> comparer) { this.Comparer = comparer; }
        public int Altura()
        {
            int cont1 = 0;
            int cont2 = 0;
            Node<T> Trabajo = new Node<T>();
            Trabajo = Raiz;

            while (Trabajo.Izquierda != null)
            {
                Trabajo = Trabajo.Izquierda;
                cont1++;
            }
            Trabajo = Raiz;
            while (Trabajo.Derecha != null)
            {
                Trabajo = Trabajo.Derecha;
                cont2++;
            }

            if (cont1 >= cont2)
            {
                return cont1;
            }
            else
            {
                return cont2;
            }

        }
        public void Add(T value)
        {
              Insert(value, Raiz);
        }
        private int max(int a1, int a2) {
            return a1 > a2 ? a1 : a2;

        }
        private Node<T> RotacionIzquierdaSimple(Node<T> k2) {
            Node<T> k1 = k2.Izquierda;
            k1.Izquierda = k1.Derecha;
            k1.Derecha = k2;
            k2.Altura = max(Altura(k2.Izquierda), Altura(k2.Derecha)) + 1;
            k1.Altura = max(Altura(k1.Izquierda), k2.Altura) + 1;
            return k1;
        }
        private Node<T> RotacionDerechaSimple(Node<T> k1)
        {
            Node<T> k2 = k1.Izquierda;
            k1.Derecha = k2.Izquierda;
            k2.Izquierda = k1;
            k1.Altura = max(Altura(k1.Izquierda), Altura(k2.Derecha)) + 1;
            k2.Altura = max(Altura(k2.Derecha), k1.Altura) + 1;
            return k1;
        }

        private Node<T> RotacionIzquierdaDoble(Node<T> k3) {
            k3.Izquierda = RotacionDerechaSimple(k3.Izquierda);
            return RotacionIzquierdaSimple(k3);
        }
        private Node<T> RotacionDerechaDoble(Node<T> k1)
        {
            k1.Derecha = RotacionIzquierdaSimple(k1.Derecha);
            return RotacionDerechaSimple(k1);
        }
        private int Altura(Node<T> node) {
            return node == null ? -1 : node.Altura;
        }

        private T Insert(T Value, Node<T> Iterando)
        {
            if (Iterando == null)
            {
                Iterando = new Node<T>();
                Iterando.Value = Value;
                peso++;
                Iterando.Altura = 0;
                return Iterando.Value;
            }
            else
            {
                int result = Comparer(Value, Iterando.Value);
                if (result < 0)
                {
                    if (Iterando.Izquierda == null)
                    {
                        Iterando.Izquierda = new Node<T>();
                        Iterando.Izquierda.Value = Value;
                        peso++;
                    }
                    else
                    {
                        return Insert(Value, Iterando.Izquierda);
                    }
                }
                else if (result > 0)
                {
                    if (Iterando.Derecha == null)
                    {
                        Iterando.Derecha = new Node<T>();
                        Iterando.Derecha.Value = Value;
                        peso++;
                    }
                    else
                    {
                        return Insert(Value, Iterando.Derecha);
                    }
                }
                

                if (Altura(Iterando.Izquierda) - Altura(Iterando.Derecha) == 2)
                {
                    int resultado = Comparer(Value, Iterando.Izquierda.Value);
                    if (resultado < 0)
                    {
                        Iterando = RotacionIzquierdaSimple(Iterando);
                    }
                    else
                    {
                        Iterando = RotacionIzquierdaDoble(Iterando);
                    }
                }

                if (Altura(Iterando.Derecha) - Altura(Iterando.Izquierda) == 2)
                {
                    int resultado2 = Comparer(Value, Iterando.Derecha.Value);
                    if (resultado2 > 0)
                    {
                        Iterando = RotacionDerechaSimple(Iterando);
                    }
                    else
                    {
                        Iterando = RotacionDerechaDoble(Iterando);
                    }
                }
            }
            Iterando.Altura = max(Altura(Iterando.Derecha), Altura(Iterando.Derecha)) + 1;
            return Iterando.Value;
        }
        protected T GetMinimun()
        {
            Node<T> trabajo = Raiz;
            while (trabajo.Izquierda != null)
            {
                trabajo = trabajo.Izquierda;
            }
            return trabajo.Value;

        }
        protected Node<T> GetMinimun(string tipo)
        {
            Node<T> trabajo = Raiz;
            while (trabajo.Izquierda != null)
            {
                trabajo = trabajo.Izquierda;
            }
            return trabajo;

        }
        protected T GetMaximun()
        {
            Node<T> trabajo = Raiz;
            while (trabajo.Derecha != null)
            {
                trabajo = trabajo.Derecha;
            }
            return trabajo.Value;

        }

        protected Node<T> GetMaximun(string tipo)
        {
            Node<T> trabajo = Raiz;
            while (trabajo.Derecha != null)
            {
                trabajo = trabajo.Derecha;
            }
            return trabajo;

        }
        public void Delete(T Value)
        {
            internalDelete(Value, Raiz);
        }
        private T internalDelete(T Value, Node<T> Iterando)
        {
            if (Iterando == null)
            {
                Iterando = new Node<T>();
                return Iterando.Value;
            }
            else
            {
                int result = Comparer(Iterando.Value, Value);
                if (result < 0)
                {
                    Iterando.Izquierda.Value = internalDelete(Value, Iterando.Izquierda);
                }
                else if (result > 0)
                {
                    Iterando.Derecha.Value = internalDelete(Value, Iterando.Derecha);
                }
                else
                {
                    if (Iterando.Izquierda == null && Iterando.Derecha == null)
                    {
                        Iterando = null;
                        return default;
                    }
                    else if (Iterando.Izquierda == null)
                    {
                        Node<T> Padre = BuscarPadre(Iterando.Derecha.Value, Raiz);
                        Padre.Derecha = Iterando.Derecha;
                        return Iterando.Value;
                    }
                    else if (Iterando.Derecha == null)
                    {
                        Node<T> Padre = BuscarPadre(Iterando.Izquierda.Value, Raiz);
                        Padre.Izquierda = Iterando.Izquierda;
                        return Iterando.Value;
                    }
                    else
                    {
                        Node<T> min = GetMinimun("Node");
                        Iterando.Value = min.Value;
                        Iterando.Derecha.Value = internalDelete(min.Value, Iterando.Derecha);
                    }
                }
                return Iterando.Value;
            }

        }
        private Node<T> BuscarPadre(T value, Node<T> node)
        {
          

            if (node == null)
            {
                return null;
            }
            if (node.Izquierda != null)
            {
                if (node.Izquierda.Value.Equals(value))
                {
                    return node;
                }
            }
            if (node.Derecha != null)
            {
                if (node.Derecha.Value.Equals(value))
                {
                    return node;
                }
            }
            int result = Comparer(node.Value, value);
            if (node.Izquierda != null && result < 0)
            {
                node.Izquierda.Value = internalDelete(value, node.Izquierda);
            }
            else if (node.Derecha != null && result > 0)
            {
                node.Derecha.Value = internalDelete(value, node.Derecha);
            }
            return node;
        }

        List<T> ArbolOrdenado = new List<T>();
        public T Search(T Value, Func<T, T, bool> Filtro)
        {
            ///Lista enviada

            foreach (var item in ArbolOrdenado)
            {
                if (Filtro(Value, item))
                {
                    return item;
                }
            }
            return default;
        }
        List<T> Data = new List<T>();
        public List<T> inorder()
        {
            return InternalInorder(Raiz);
        }

        private List<T> InternalInorder(Node<T> Node)
        {
            if (Node != null)
            {
                InternalInorder(Node.Izquierda);
                Data.Add(Node.Value);
                InternalInorder(Node.Derecha);
            }
            return Data;
        }
    }
}
