using Poke_API_Client.Data.Classes.PokemonDetails;
using Poke_API_Client.Data.Classes.PokemonDetails.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poke_API_Client.Data.Classes
{
    internal class PokemonDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AbilityList> Abilities { get; set; }
        public List<Typelist> Types { get; set; }
        public List<MoveList> Moves { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }

    }
}
