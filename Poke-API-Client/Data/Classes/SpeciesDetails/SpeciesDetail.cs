using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poke_API_Client.Data.Classes.SpeciesDetails
{
    class SpeciesDetail
    {
        public int Id { get; set; }
        public int Base_Happiness { get; set; }
        public int Capture_Rate { get; set; }
        public Habitat Habitat {get; set; }
        public Species_Evolution_Chain evolution_chain { get; set; }
        

    }
}
