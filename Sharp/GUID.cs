using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp
{
    internal class GUID
    {
        static void Main()
        {
            // Generate a new GUID
            Guid newGuid = Guid.NewGuid();

            // Print the generated GUID
            Console.WriteLine("Generated GUID: " + newGuid);
        }
    }
}
