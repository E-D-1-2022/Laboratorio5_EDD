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
                // If we are at the deepest level (the leaf), it is well-balanced for sure
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
                // Recursive case, we are not on the deepest level
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
                // Trivial case, we are on the deepest level of the tree
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
            // Keep calm and rebalance
            return max;
        }
        public T replaceMin()
        {
            T min = default(T);
            if (!isLeaf())
            {
                // Cas recursiu, mentre no arribem al nivell mes profund anem baixant per l'esquerra sempre
                min = left.replaceMin();
            }
            else
            {
                // Cas trivial, agafem l'element i ho intentem deixar tot maco
                min = leftElement;
                leftElement = default(T);
                if (rightElement != null)
                {
                    // Hi havia element a la dreta, el passem a l'esquerra i aqui no ha passat res!
                    leftElement = rightElement;
                    rightElement = default(T);
                }
            }
            if (!isBalanced())
            {
                // Aquesta situacio es dona quan a la dreta no hi havia element, en la 1a pujada rebalancejara
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
                    // The unbalance is in the left child
                    // We put the left element of current Node<T> as the left element of the left child
                    left.leftElement=leftElement;
                    // Now we replace the left element of the mid child as the left element of the current Node<T>
                    leftElement=mid.leftElement;
                    // If a right element on the mid child exists, we shift it to the left
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
                    // The unbalance is in the right child
                    // CRITICAL CASE, each Node<T> (child) of the deepest level have just one element (the right is empty)
                    // the algorithm will have to rebalance from a higher level of the tree
                    if (rightElement.Equals(default(T)))
                    {
                        if (!left.leftElement.Equals(default(T))&& left.rightElement.Equals(default(T)) && mid.leftElement.Equals(default(T)))
                        {
                            rightElement=leftElement;
                            leftElement = left.leftElement;
                            // Let the party starts, we remove the current childs
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
                                // The other but same case the party starts
                                left=(null);
                                mid=(null);
                                right=(null);
                            }
                        }
                    }
                    else
                    {
                        // We put the right element of the current Node<T> as the left element of the mid son
                        mid.leftElement = rightElement;
                        // We put the left element of the right child as the right element of the current Node<T>
                        rightElement = right.leftElement;
                        // If the right child, where we have taken the last element has a right element, we move it
                        // into the left of the same child
                        if (!right.rightElement.Equals(default(T)))
                        {
                            right.leftElement = right.rightElement;
                            right.rightElement=default(T);
                        }
                        else
                        {
                            // Else, we let the right child EMPTY
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
