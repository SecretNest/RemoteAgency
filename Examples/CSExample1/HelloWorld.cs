using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSExample1
{
    class HelloWorld : IHello
    {
        void IHello.HelloWorld()
        {
            Console.WriteLine("Hello World!");
        }
    }
}
