using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntryPoint
{
    class Empty<T> : KDTree<T>
    {
        public bool IsEmpty { get { return true; } }
        public bool IsVertical { get; set; }
        public KDTree<T> Left { get; set; }
        public KDTree<T> Right { get; set; }
        public T Value { get; set; }
    }
}