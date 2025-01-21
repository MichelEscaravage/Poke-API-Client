using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poke_API_Client.Data.Classes.Evolution_Chain
{
    internal class Evolution_Data
    {
        public Species species { get; set; }
        public List<Evolution_Data> evolves_to { get; set; }
    }
}
