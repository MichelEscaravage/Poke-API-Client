using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Poke_API_Client.Data.Classes;
using Poke_API_Client.Data.Classes.Evolution_Chain;
using Poke_API_Client.Data.Classes.SpeciesDetails;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Poke_API_Client
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private List<Pokemon> pokemonList;

        public MainWindow()
        {
            this.InitializeComponent();
            LoadData();

        }

        public async Task LoadData()
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://pokeapi.co/api/v2/pokemon?limit=2000");
            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            var apiResponse = JsonSerializer.Deserialize<PokeApiResponse>(content, options);

            PokeList.ItemsSource = apiResponse?.Results;

            pokemonList = apiResponse?.Results;
        }

        private void PokeList_ItemClick(object sender, ItemClickEventArgs e)
        {
            PokeDetails.Visibility = Visibility.Visible;
            SearchBar.Visibility = Visibility.Collapsed;
            Grid.SetColumn(PokeList, 0);
            MainscreenGrid.ColumnDefinitions[1].Width = new GridLength(0);
            var selectedPokemon = e.ClickedItem as Pokemon;
            if (selectedPokemon != null)
            {
                var pokemonId = int.Parse(selectedPokemon.Url.Split('/').Reverse().Skip(1).First());
                LoadSelectedData(pokemonId, selectedPokemon);
            }
        }

        private async Task LoadSelectedData(int pokemonId, Pokemon selectedPokemon)
        {
            nextEvolution.Text = " ";
            
            var client = new HttpClient();

            var task1 = client.GetAsync($"https://pokeapi.co/api/v2/pokemon/{pokemonId}/");
            var task2 = client.GetAsync($"https://pokeapi.co/api/v2/pokemon-species/{pokemonId}/");

            var responses = await Task.WhenAll(task1, task2);

            var content1 = await responses[0].Content.ReadAsStringAsync();
            var content2 = await responses[1].Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            var pokemonDetail = JsonSerializer.Deserialize<PokemonDetail>(content1, options);
            var speciesDetail = JsonSerializer.Deserialize<SpeciesDetail>(content2, options);

            var response = await client.GetAsync(speciesDetail.evolution_chain.url);
            var content3 = await response.Content.ReadAsStringAsync();

            var evolution_Chain = JsonSerializer.Deserialize<Evolution_Chain>(content3, options);

            List<Species> evolutionChain = new List<Species>();
            var chain = evolution_Chain.Chain;
            while (chain != null)
            {
                evolutionChain.Add(chain.species);
                chain = chain.evolves_to.FirstOrDefault();
            }

            BitmapImage bitmapImage;
            if (selectedPokemon.Id > 649)
            {
                bitmapImage = new BitmapImage(new Uri($"https://img.pokemondb.net/sprites/x-y/normal/{selectedPokemon.Name}.png"));
            }
            else
            {
                bitmapImage = new BitmapImage(new Uri($"https://img.pokemondb.net/sprites/black-white/anim/normal/{selectedPokemon.Name}.gif"));
            }

            var typeInfoList = pokemonDetail.Types.Select(type => new TypeInfo { Name = type.Type.Name }).ToList();
            var moveInfoList = pokemonDetail.Moves.Select(move => new MoveInfo { Name = move.Move.Name }).ToList();
            var abilityInfo = pokemonDetail.Abilities.Select(ability => new AbilityInfo { Name = ability.Ability.Name }).ToList();

            var heightInfo = pokemonDetail.Height.ToString();
            var weightInfo = pokemonDetail.Weight.ToString();

            var base_Happiness = speciesDetail.Base_Happiness.ToString();
            var capture_Rate = speciesDetail.Capture_Rate.ToString();

            if (speciesDetail.Habitat != null)
            {
                var habitat = speciesDetail.Habitat.Name;
                string habitatName = habitat.Substring(0, 1).ToUpper() + habitat.Substring(1);
                HabitatInfo.Text = habitatName;
            }
            else
            {
                HabitatInfo.Text = "UNDEFINED";
            }

            for (int i = 0; i < evolutionChain.Count() - 1; i++)
            {
                if (selectedPokemon.Name == evolutionChain[i].name)
                {
                    string evolutionPokemonName = evolutionChain[i +1].name;

                    if (evolutionPokemonName != selectedPokemon.Name)
                    {
                        string capitalizedName = char.ToUpper(evolutionPokemonName[0]) + evolutionPokemonName.Substring(1);
                        nextEvolution.Text = $"Next evolution: { capitalizedName}";
                    }
                    break;
                }
            }
            pokemonName.Text = selectedPokemon.CapitalizedName + " " + selectedPokemon.URLId;

            pokemonImage.Source = bitmapImage;

            HeightText.Text = $"Height: {heightInfo}";
            WeightText.Text = $"Weight: {weightInfo}";

            typesList.ItemsSource = typeInfoList;
            abilitiesList.ItemsSource = abilityInfo;
            movesList.ItemsSource = moveInfoList;

            CaptureRate.Text = $"Capture Rate: {capture_Rate}";
            BaseHappiness.Text = $"Base Happiness: {base_Happiness}";
        }


        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = SearchBar.Text;
            string lowerFilter = filter.ToLower();

            var filteredPokemon = pokemonList
                .Where(pokemon =>

                    pokemon.Name.ToLower().Contains(lowerFilter) ||
                    pokemon.Id.ToString() == lowerFilter
                );

            PokeList.ItemsSource = filteredPokemon;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            PokeDetails.Visibility = Visibility.Collapsed;
            SearchBar.Visibility = Visibility.Visible;
            Grid.SetColumn(PokeList, 1);
        }
    }
}
