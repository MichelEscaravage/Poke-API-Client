using Microsoft.UI.Xaml.Media.Imaging;
using Poke_API_Client.Data.Classes.Evolution_Chain;
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

        public string ImageURL
        {
            get
            {
                if (Name == "eevee-starter" || Name == "pikachu-starter")
                {
                    string baseName = Name.Split("-")[0];
                    return $"https://img.pokemondb.net/sprites/home/normal/{baseName}-f.png";
                }
                else if (Id < 649)
                {
                    return $"https://img.pokemondb.net/sprites/black-white/anim/normal/{Name}.gif";
                }
                else
                {
                    return $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{Id}.png";
                }
            }
        }


    }

}
