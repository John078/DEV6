using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntryPoint
{
    interface KDTree<T>
    {
        bool IsEmpty { get; }
        bool IsVertical { get; }
        KDTree<T> Left { get; set; }
        KDTree<T> Right { get; set; }
        T Value { get; set; }
    }
}
