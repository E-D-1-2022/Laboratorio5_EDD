using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 namespace CustomStructure.Tree_2_3
{
    public class Tree23<T>:Itree_2_3<T>
    {
        private Node<T> root;
        // The root of the tree
        private int Size;
        // Number of elements inside of the tree
        private const int ROOT_IS_BIGGER = 1;
        private const int ROOT_IS_SMALLER = -1;
        private bool addition;
        Func< T, T,int> compareTo;
        Func<T, T, bool> comp;
        // A flag to know if the last element has been added correctly or not
        public Tree23(Func<T, T, int> compareTo,Func<T,T,bool>compa)
        {
            
            this.Size = 0;
            this.compareTo = compareTo;
            this.comp = compa;
        }
        public bool Add(T element)
        {
            this.Size++;
            addition = false;
            if (root== null)
            {
                // first case
                if (root==null)
                {
                    root = new Node<T>();
                }
                root.leftElement=element;
                addition = true;
            }
            else
            {
                var newRoot = addElementI(root, element);
                // Immersion
                if (newRoot!=null)
                {
                    root = newRoot;
                }
            }
            if (!addition)
            {
                this.Size--;
            }
            return addition;
        }
        private Node<T> addElementI(Node<T> current, T element)
        {
            Node<T> newParent = null;
            if (current.Equals(default(T))) { current = new Node<T>(); }
            // We aren't in the deepest level yet
            if (!current.isLeaf())
            {
                Node<T> sonAscended = null;
                if (compareTo(element, current.leftElement) == 0 || (current.is3Node() && compareTo(element, current.rightElement) == 0))
                {
                    return null;
                }
                else if (compareTo(element, current.leftElement) == Tree23<T>.ROOT_IS_BIGGER)
                    {
                        sonAscended = addElementI(current.left, element);
                        // Case sonAscended != null --> the element has been added on a 3-Node<T> (there were 2 elements)
                        if (sonAscended != null)
                        {
                            // A new Node<T> comes from the left branch
                            // The new element, in this case, is always less than the current.left
                            if (current.is2Node())
                            {
                                current.rightElement = current.leftElement;
                                // shift the current left element to the right
                                current.leftElement = sonAscended.leftElement;
                                current.right = current.mid;
                                current.mid = sonAscended.mid;
                                current.left = sonAscended.left;
                            }
                            else
                            {
                                // In this case we have a new split, so the current element in the left will go up
                                // We copy the right part of the subtree
                                var rightCopy = new Node<T>(current.rightElement, default, current.mid, current.right);
                                // Now we create the new "structure", pasting the right part
                                newParent = new Node<T>(current.leftElement, default, sonAscended, rightCopy);
                            }
                        }
                    }
                    else if (current.is2Node() || (current.is3Node() && compareTo(element, current.rightElement) == Tree23<T>.ROOT_IS_BIGGER))
                    {
                        sonAscended = addElementI(current.mid, element);
                        if (sonAscended != null)
                        {
                            // A new split
                            // The right element is empty, so we can set the ascended element in the left and the existing left element into the right
                            if (current.is2Node())
                            {
                                current.rightElement = sonAscended.leftElement;
                                current.right = sonAscended.mid;
                                current.mid = sonAscended.left;
                            }
                            else
                            {
                                // Another case we have to split again
                                var left = new Node<T>(current.leftElement, default, current.left, sonAscended.left);
                                var mid = new Node<T>(current.rightElement, default, sonAscended.mid, current.right);
                                newParent = new Node<T>(sonAscended.leftElement, default, left, mid);
                            }
                        }
                    }
                    else if (current.is3Node() && compareTo(element, current.rightElement) == Tree23<T>.ROOT_IS_SMALLER)
                    {
                        sonAscended = addElementI(current.right, element);
                        if (sonAscended != null)
                        {
                            // Split, the right element goes up
                            var leftCopy = new Node<T>(current.leftElement, default, current.left, current.mid);
                            newParent = new Node<T>(current.rightElement, default, leftCopy, sonAscended);
                        }
                    }
                }
                else
                {
                    // We are in the deepest level
                    addition = true;
                    // The element already exists
                    if (compareTo(element, current.leftElement) == 0 || (current.is3Node() && compareTo(element, current.rightElement) == 0))
                    {
                        addition = false;
                    }
                    else if (current.is2Node())
                    {
                        // an easy case, there is not a right element
                        // if the current left element is bigger than the new one --> we shift the left element to the right
                        if (compareTo(element, current.leftElement) == Tree23<T>.ROOT_IS_BIGGER)
                        {
                            current.rightElement = current.leftElement;
                            current.leftElement = element;
                        }
                        else  if (compareTo(element, current.leftElement) == Tree23<T>.ROOT_IS_SMALLER)
                        {
                            current.rightElement = element;
                        }
                    }
                    else
                    {
                        newParent = split(current, element);
                    }
            }
            return newParent;
        }
        
        private Node<T> split(Node<T> current, T element)
        {

            Node<T> newParent = null;
            // The left element is bigger, so it will go up letting the new element on the left
            if (compareTo(element, current.leftElement) == Tree23<T>.ROOT_IS_BIGGER)
            {
                var left = new Node<T>(element, default);
                var right = new Node<T>(current.rightElement, default);
                newParent = new Node<T>(current.leftElement, default, left, right);
            }
            else
            if (compareTo(element, current.leftElement) == Tree23<T>.ROOT_IS_SMALLER)
            {
                // The new element is bigger than the current on the right and less than the right element
                // The new element goes up
                if (compareTo(element, current.rightElement) == Tree23<T>.ROOT_IS_BIGGER)
                {
                    var left = new Node<T>(current.leftElement, default);
                    var right = new Node<T>(current.rightElement, default);
                    newParent = new Node<T>(element, default, left, right);
                }
                else
                {
                    // The new element is the biggest one, so the current right element goes up
                    var left = new Node<T>(current.leftElement, default);
                    var right = new Node<T>(element, default);
                    newParent = new Node<T>(current.rightElement, default, left, right);
                }
            }
            return newParent;
        }

        public void Clear()
        {
            this.Size = 0;
            root = null;
        }

        public bool Contains(T element)
        {
            return !Find(element).Equals(default(T));
        }
        public T Find(T element)
        {
            return findI(root, element);
        }
        private T findI(Node<T> current,T element)
        {

            T found = default;
            if (current != null)
            {
                // Trivial case, we have found the element
                if (!current.leftElement.Equals(default(T)) && comp(element, current.leftElement))
                {
                    found = current.leftElement;
                }
                else
                {
                    // Second trivial case, the element to find equals the right element
                    if (!current.rightElement.Equals(default(T)) && comp(element, current.rightElement))
                    {
                        found = current.rightElement;
                    }
                    else
                    {
                        // Recursive cases
                        if (compareTo(element, current.leftElement) == Tree23<T>.ROOT_IS_BIGGER)
                        {
                            found = findI(current.left, element);
                        }
                        else
                        if (current.right == null || compareTo(current.rightElement,element) == Tree23<T>.ROOT_IS_BIGGER)
                        {
                            found = findI(current.mid, element);
                        }
                        else
                        if (compareTo(element, current.rightElement) == Tree23<T>.ROOT_IS_SMALLER)
                        {
                            found = findI(current.right, element);
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                }
            }
            return found;
        }

        public T findMin()
        {
            if (isEmpty())
            {
                return default;
            }
            return findMinI(root);
        }
        private T findMinI(Node<T> current)
        {
            if (current.left == null)
            {
                return current.leftElement;
            }
            else
            {
                return findMinI(current.left);
            }
        }
     
        public T findMax()
        {
            if (isEmpty())
            {
                return default;
            }
            return findMaxI(root);
        }
        // Immersion
        private T findMaxI(Node<T> current)
        {
            // Recursive case
            if (current.rightElement != null && current.right != null)
            {
                return findMaxI(current.right);
            }
            else
            if (current.mid != null)
            {
                return findMaxI(current.mid);
            }
            // Trivial case
            if (current.rightElement != null)
            {
                return current.rightElement;
            }
            else
            {
                return current.leftElement;
            }
        }
     
        public long getLevel()
        {
            var aux = root;
            var level = 0;
            while (aux != null)
            {
                aux = root.left;
                level++;
            }
            return level;
        }
       
        List<T> listain = new List<T>();
        public List<T> inOrder()
        {
            listain.Clear();
            if (!isEmpty())
            {

                return inOrderI(root);
            }
            else
            {
                return null;
            }
        }
       
        private List<T> inOrderI(Node<T> current)
        {
            if (current != null)
            {

                if (current.isLeaf())
                {

                    listain.Add(current.leftElement);
                    if (current.rightElement == null || current.rightElement.Equals(default(T))){ current.rightElement = (T)Activator.CreateInstance(typeof(T)); }
                    if (!current.rightElement.Equals(default(T)) || current.rightElement != null)
                    {
                        listain.Add(current.rightElement);
                    }
                }
                else
                {

                    inOrderI(current.left);
                    if (current.leftElement == null || current.leftElement.Equals(default(T))) { current.leftElement = (T)Activator.CreateInstance(typeof(T)); }
                    if (!current.leftElement.Equals(default(T))|| current.leftElement!=null) {
                        listain.Add(current.leftElement);
                    }

                    inOrderI(current.mid);
                    if (current.rightElement == null || current.rightElement.Equals(default(T))) { current.rightElement = (T)Activator.CreateInstance(typeof(T)); }

                    if (!current.rightElement.Equals(default(T)) || current.rightElement!=null)
                    {
                        if (!current.isLeaf())
                            listain.Add(current.rightElement);

                        inOrderI(current.right);
                    }
                }
            }
            return listain;
        }

        public bool isEmpty()
        {
            if (root == null)
            {
                return true;
            }
            if (root.leftElement.Equals(default(T)))
            {
                return true;
            }
            return false;
        }
    
        public bool modify(T which, T update)
        {
            var modified = false;
            
            if (Contains(which))
            {
                modified = true;
                Remove(which);
                Add(update);
            }
            return modified;
        }
       
        private bool modifyI(Node<T> current, T element)
        {
            var modified = false;
            if (current != null)
            {
                if (current.leftElement.Equals(element))
                {
                    Remove(element);
                    modified = true;
                }
                else
                {
                    if (current.rightElement != null && current.rightElement.Equals(element))
                    {
                        modified = true;
                        Remove(element);
                    }
                    else
                    {
                        modified = modifyI(current.left, element);
                        if (!modified)
                        {
                            modified = modifyI(current.mid, element);
                        }
                        if (current.rightElement != null && !modified)
                        {
                            modified = modifyI(current.right, element);
                        }
                    }
                }
            }
            return modified;
        }
      
        public bool Remove(T element)
        {
            bool deleted;

            // We decrease the number of levels at the start, if the element is not deleted, we increase the value at the end
            this.Size--;

            deleted = removeI(root, element); // Immersion

            root.rebalance();
            if (root.leftElement == null) { root.leftElement = (T)Activator.CreateInstance(typeof(T)); }
            if (root.leftElement.Equals(default(T))) { root = null; } // We have deleted the last element of the tree

            if (!deleted) { this.Size++; }

            return deleted;
        }

     
        private bool removeI(Node<T> current, T element)
        {
            bool deleted = true;

            // Trivial case, we are in the deepest level of the tree but we have not found the element (it does not exist)
            if (current == null) { 
                deleted = false; 
            }
            else
            {
                // Recursive case, we are still finding the element to delete
                if (!current.leftElement.Equals(element))
                {

                    // If there is no element in the right (2 Node) or the element to delete is less than the right element
                    if (current.rightElement.Equals(default(T))|| compareTo(element, current.rightElement) == ROOT_IS_BIGGER)
                    {

                        // The left element is bigger than the element to delete, so we go through the left child
                        if (compareTo( element, current.leftElement) == ROOT_IS_BIGGER)
                        {

                            deleted = removeI(current.left, element);
                        }
                        else
                        { // If not, we go through the mid child

                            deleted = removeI(current.mid, element);
                        }
                    }
                    else
                    {

                        // If the element to delete does not equals the right element, we go through the right child
                        if (!current.rightElement.Equals(element))
                        {

                            deleted = removeI(current.right, element);
                        }
                        else
                        { // If not, we have found the element

                            // Situation A, the element equals the right element of a leaf so we just have to delete it
                            if (current.isLeaf()) { 
                                current.rightElement = default(T); 
                            }

                            else
                            { // We found the element but it is not in the leaf, this is the situation B

                                // We get the min element of the right branch, remove it from its current position and put it
                                // where we found the element to delete
                                T replacement = current.right.replaceMin();

                                current.rightElement = replacement;
                            }
                        }
                    }
                }
                // The left element equals the element to delete
                else
                {

                    if (current.isLeaf())
                    { // Situation A

                        // The left element, the element to remove, is replaced by the right element
                        if (current.rightElement.Equals(default(T)))
                        {

                            current.leftElement = current.rightElement;

                            current.rightElement = default(T);
                        }
                        else
                        { // If there is no element in the right, a rebalance will be necessary

                            current.leftElement = default(T); // We let the node empty

                            // We warn on the bottom up that a node has been deleted (is empty) and a rebalance is necessary
                            // in THAT level of the tree
                            return true;
                        }
                    }
                    else
                    { // Situation B

                        // We move the max element of the left branch where we have found the element
                        T replacement = current.left.replaceMax();

                        current.leftElement = replacement;
                    }
                }
            }

            if (current != null && !current.isBalanced())
            {

                current.rebalance();  // The bottom level have to be rebalanced
            }
            else if (current != null && !current.isLeaf())
            {

                bool balanced = false;

                while (!balanced)
                {

                    if (current.right == null)
                    {

                        // Critical case of the situation B at the left child
                        if (current.left.isLeaf() && !current.mid.isLeaf())
                        {

                            T replacement = current.mid.replaceMin();

                            T readdition = current.leftElement;

                            current.leftElement=replacement;

                            Add(readdition);

                            // Critical case of hte situation B at the right child
                        }
                        else if (!current.left.isLeaf() && current.mid.isLeaf())
                        {

                            if (current.rightElement == null)
                            {

                                T replacement = current.left.replaceMax();

                                T readdition = current.leftElement;

                                current.leftElement=replacement;

                                Add(readdition);
                            }
                        }
                    }
                    // It is important to note that we can't use the 'else' sentence because the situation could have changed in the if above
                    if (current.right != null)
                    {

                        if (current.mid.isLeaf() && !current.right.isLeaf())
                        {

                            current.right.rebalance();
                        }
                        if (current.mid.isLeaf() && !current.right.isLeaf())
                        {

                            T replacement = current.right.replaceMin();

                            T readdition = current.rightElement;

                            current.rightElement=replacement;

                            Add(readdition);
                        }
                        else balanced = true;
                    }
                    if (current.isBalanced()) balanced = true;
                }
            }

            return deleted;
        }


        public int size()
        {
            return this.Size;
        }
    }
}