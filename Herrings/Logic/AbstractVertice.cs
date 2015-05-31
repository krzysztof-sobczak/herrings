using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herrings.Logic
{
    abstract class AbstractVertice
    {
        abstract public int toInt();

        abstract public float getValue();

        static public implicit operator int(AbstractVertice vertice)
        {
            if (vertice == null)
            {
                return 0;
            }
            return vertice.toInt();
        }
    }
}
