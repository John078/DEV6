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

        //j huidige value, en k de nieuwe in te vullen value.
        public KDTree<Vector2> Insert(KDTree<Vector2> j, Vector2 k)
        {
            if (j.IsEmpty)
            {
                return new Node<Vector2>(new Empty<Vector2>(), new Empty<Vector2>(), k, true);
            }

            //if not empty
            else
            {
                if (IsVertical)
                {
                    //als de nieuwe value kleiner is dan j gaat hij naar links. Vertical wordt false.
                    if (j.Value.X > k.X)
                    {
                        return new Node<Vector2>(Insert(j.Left, k), j.Right, j.Value, false);
                    }
                    else
                    {
                        return new Node<Vector2>(j.Left, Insert(j.Right, k), j.Value, false);
                    }
                }
                else
                {
                    //als de nieuwe value kleinder is dan j gaat hij naar links. Vertical wordt true.
                    if (j.Value.Y > k.Y)
                    {
                        return new Node<Vector2>(Insert(j.Left, k), j.Right, j.Value, true);
                    }
                    else
                    {
                        return new Node<Vector2>(j.Left, Insert(j.Right, k), j.Value, true);
                    }
                }
            }
        }
           
    }
}
