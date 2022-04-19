using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomStructures.Tree
{

    class BTreeNode<K, V>
    {
        List<K> Keys { get; set; }
        List<V> Values { get; set; }
        List<BTreeNode<K, V>> childs { get; set; }
        BTreeNode<K, V> parent { get; set; }
        int minimumDegree { get; set; }

        int maximumDegree { get; set; }
        int currentKeysSize { get; set; }
        bool isLeaf { get; set; }

        CompareKeysDelegate<K> KeysComparator;

        public BTreeNode(int minimumDegree, int maximumDegree, bool isLeaf, CompareKeysDelegate<K> _KeysComparator)
        {
            this.Keys = new List<K>();

            this.Values = new List<V>();

            this.childs = new List<BTreeNode<K, V>>();

            this.parent = null;

            this.minimumDegree = minimumDegree;

            currentKeysSize = 0;

            this.isLeaf = isLeaf;

            KeysComparator = _KeysComparator;
        }


        public V Search(K _key, BTreeNode<K, V> actual)
        {
            throw new NotImplementedException();
        }

        public void Insert(K _key, V _value)
        {
            if (currentKeysSize < (maximumDegree - 1))
            { //Split is not necessary
                if (isLeaf) //IF is a leaf not necessary handle the references to children
                {
                    int i = 0;
                    bool indexIsFound = false;
                    while ((i < Keys.Count) || (!indexIsFound))
                    {

                        if (KeysComparator(_key, Keys[i]) == 0) //IF the key exists then I will not insert
                        {
                            return;
                        }

                        indexIsFound = KeysComparator(_key, Keys[i]) < 0;
                        i++;
                    }

                    //Index is found then insert
                    Keys.Insert(i, _key);
                    Values.Insert(i, _value);
                    currentKeysSize++;
                }
                else //Necessary to handle the references to children
                {
                    int i = 0;
                    bool indexIsFound = false;
                    while ((i < Keys.Count) || (!indexIsFound))
                    {

                        if (KeysComparator(_key, Keys[i]) == 0) //IF the key exists then I will not insert
                        {
                            return;
                        }

                        indexIsFound = KeysComparator(_key, Keys[i]) < 0;
                        i++;
                    }

                    if (indexIsFound) //It means a index was found before the end of Keys
                    {
                        childs[i].Insert(_key, _value);
                    }
                    else //Index was not found then go to the last child
                    {
                        childs[Keys.Count].Insert(_key, _value);
                    }

                }
            }
            else
            { //Split is necessary

                //Going to add the new values, add operation is only in a leaf
                int i = 0;
                bool indexIsFound = false;
                while ((i < Keys.Count) || (!indexIsFound))
                {

                    if (KeysComparator(_key, Keys[i]) == 0) //IF the key exists then I will not insert
                    {
                        return;
                    }

                    indexIsFound = KeysComparator(_key, Keys[i]) < 0;
                    i++;
                }

                //Index is found then insert
                Keys.Insert(i, _key);
                Values.Insert(i, _value);
                currentKeysSize++;
                Split(this);

            }
        }

        private void Split(BTreeNode<K, V> actual) //when arrive to this the actual node should have maximum number of keys
        {
            if (parent != null) //It is intermediate node or a leaf
            {

            }
            else //It is the root
            {
                if (isLeaf) //if is the root but also a leaf
                {
                    //Check which index will be promoted to parent
                    int index = maximumDegree / 2;

                    //Create Two new nodes
                    BTreeNode<K, V> left = new BTreeNode<K, V>(minimumDegree, maximumDegree, true, KeysComparator);
                    BTreeNode<K, V> right = new BTreeNode<K, V>(minimumDegree, maximumDegree, true, KeysComparator);
                    //Since this is the root then is the root going to mark is a leaft as false
                    isLeaf = false;

                    //Assign left childs and remove from the new parent
                    int deletedKeysCount = 0;
                    for (int i = 0; i < index; i++)
                    {
                        left.Insert(Keys[i], Values[i]);
                        left.currentKeysSize++;
                        deletedKeysCount++;
                    }

                    while (deletedKeysCount > 0)
                    {
                        Keys.RemoveAt(0);
                        Values.RemoveAt(0);
                        deletedKeysCount--;
                    }

                    //Assign the right childs and remove from the new parent
                    for (int i = 1; i < Keys.Count; i++)
                    {
                        right.Insert(Keys[i], Values[i]);
                        right.currentKeysSize++;
                        deletedKeysCount++;
                    }

                    while (deletedKeysCount > 0)
                    {
                        Keys.RemoveAt(1);
                        Values.RemoveAt(1);
                        deletedKeysCount--;
                    }

                    //promote to parent
                    childs.Insert(0, left);
                    childs.Insert(1, right);
                    left.parent = this;
                    right.parent = this;
                    currentKeysSize = 1;

                    //Since this is the root, no necessary to continue bottom tu up operations

                }
                else //Is the root but has children
                {

                    //Check which index will be promoted to parent
                    int index = maximumDegree / 2;

                    //Create Two new nodes
                    BTreeNode<K, V> left = new BTreeNode<K, V>(minimumDegree, maximumDegree, false, KeysComparator);
                    BTreeNode<K, V> right = new BTreeNode<K, V>(minimumDegree, maximumDegree, false, KeysComparator);
                    //Since this is the root then is the root going to mark is a leaft as false
                    isLeaf = false;

                    //Assign left childs and remove from the new parent
                    int deletedKeysCount = 0;
                    for (int i = 0; i < index; i++)
                    {
                        left.Keys.Add(Keys[i]);
                        left.Values.Add(Values[i]);
                        left.childs.Add(this.childs[i]);
                        left.currentKeysSize++;
                        deletedKeysCount++;
                    }
                    left.childs.Add(childs[index]);

                    while (deletedKeysCount > 0)
                    {
                        Keys.RemoveAt(0);
                        Values.RemoveAt(0);
                        childs.RemoveAt(0);
                        deletedKeysCount--;
                    }
                    childs.RemoveAt(0);

                    //Assign the right childs and remove from the new parent
                    for (int i = 1; i < Keys.Count; i++)
                    {
                        right.Keys.Add(Keys[i]);
                        right.Values.Add(Values[i]);
                        right.childs.Add(this.childs[i - 1]);
                        right.currentKeysSize++;
                        deletedKeysCount++;
                    }
                    right.childs.Add(childs[childs.Count - 1]);

                    while (deletedKeysCount > 0)
                    {
                        Keys.RemoveAt(1);
                        Values.RemoveAt(1);
                        deletedKeysCount--;
                    }
                    childs.RemoveAt(0);

                    //promote to parent
                    childs.Add(left);
                    childs.Add(right);
                    left.parent = this;
                    right.parent = this;
                    currentKeysSize = 1;

                    //Since this is the root, no necessary to continue bottom tu up operations
                }


            }

        }
    }
}
