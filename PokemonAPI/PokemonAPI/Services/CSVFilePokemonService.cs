using Microsoft.AspNetCore.Hosting;
using PokemonAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonAPI.Services
{
    public class CSVFilePokemonService
    {
        private IWebHostEnvironment webHostEnvironment;
        public CSVFilePokemonService(IWebHostEnvironment _webHostEnvironment)
        {
            webHostEnvironment = _webHostEnvironment;
        }
        private string DataFile
        {
            get { return Path.Combine(webHostEnvironment.ContentRootPath, "Data", "pokemon.csv"); }
        }
        public List<Pokemon> GetPokemons()
        {
            List<Pokemon> pokemons = new List<Pokemon>();
            string[] csvLines = File.ReadAllLines(DataFile);

            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] rowData = csvLines[i].Split(',');
                if (rowData[2] == "Ghost" || rowData[3] == "Ghost")
                {
                    continue;
                }
                else if (rowData[12] == "True")
                {
                    continue;
                }
                Pokemon pokemon = new Pokemon()
                {
                    Id = i,
                    Name = rowData[1],
                    TypeOne = rowData[2],
                    TypeTwo = rowData[3],
                    SpeedDefense = Convert.ToInt32(rowData[9]),
                    Speed = Convert.ToInt32(rowData[10]),
                    Generation = Convert.ToInt32(rowData[11]),
                    Legendary = Convert.ToBoolean(rowData[12])
                };
                int HP = Convert.ToInt32(rowData[5]);
                int Attack = Convert.ToInt32(rowData[6]);
                int Defense = Convert.ToInt32(rowData[7]);
                int SpeedAttack = Convert.ToInt32(rowData[8]);

                if (pokemon.TypeOne == "Steel" || pokemon.TypeTwo == "Steel")
                {
                    HP *= 2;
                }
                if (pokemon.TypeOne == "Fire" || pokemon.TypeTwo == "Fire")
                {
                    Attack = (int)(Attack * 0.9);
                }
                if (pokemon.TypeOne == "Bug" && pokemon.TypeTwo == "Flying" ||
                    pokemon.TypeOne == "Flying" && pokemon.TypeTwo == "Bug")
                {
                    SpeedAttack = (int)(SpeedAttack * 1.1);
                }
                if (pokemon.Name[0] == 'G')
                {
                    Defense += (pokemon.Name.Length - 1) * 5;
                }
                pokemon.HP = HP;
                pokemon.Attack = Attack;
                pokemon.Defense = Defense;
                pokemon.SpeedAttack = SpeedAttack;
                pokemon.Total = pokemon.HP + pokemon.Attack + pokemon.Defense + 
                    pokemon.SpeedAttack + pokemon.SpeedDefense + pokemon.Speed;
                pokemons.Add(pokemon);
            }
            return pokemons;
        }
    }
}
