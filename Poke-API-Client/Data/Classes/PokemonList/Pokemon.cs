using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Poke_API_Client.Data.Classes
{
    internal class Pokemon
    {
        public int Id => int.TryParse(Url?.Split('/').Reverse().Skip(1).FirstOrDefault(), out int id) ? id: 0;
        public string Name { get; set; }
        public string CapitalizedName => char.ToUpper(Name[0]) + Name.Substring(1); 
        public string Url { get; set; }
        public string URLId => $"#{Id}";

        public string ImageURL => Id < 649
           ? $"https://img.pokemondb.net/sprites/black-white/anim/normal/{Name}.gif"
           : $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{Id}.png";
    }

}
