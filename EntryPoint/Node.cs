using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntryPoint
{
    class Node<T> : KDTree<T>
    {
        public bool IsEmpty { get { return false; } }
        public bool IsVertical { get; set; }
        public KDTree<T> Left { get; set; }
        public KDTree<T> Right { get; set; }
        public T Value { get; set; }


        public Node(KDTree<T> left, KDTree<T> right, T value, bool vertical)
        {
            Left = left;
            Right = right;
            Value = value;
            IsVertical = vertical;
        }
  
    }
}
