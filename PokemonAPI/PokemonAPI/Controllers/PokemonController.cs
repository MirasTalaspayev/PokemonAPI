using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonAPI.Models;
using PokemonAPI.Services;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private CSVFilePokemonService csvFile;
        public PokemonController(CSVFilePokemonService _csvfile)
        {
            csvFile = _csvfile;
        }
        [HttpGet]
        public IEnumerable<Pokemon> GetAll(int page = 1, string name = null)
        {
            List<Pokemon> pokemons = csvFile.GetPokemons();
            var query = HttpContext.Request.Query;
            int max, min;
            
            #region // filters the query
            if (query.ContainsKey("hp[lte]") && int.TryParse(query["hp[lte]"], out max))
            {
                pokemons = pokemons.Where(p => p.HP <= max).ToList();
            }
            if (query.ContainsKey("hp[gte]") && int.TryParse(query["hp[gte]"], out min))
            {
                pokemons = pokemons.Where(p => p.HP >= min).ToList();
            }
            if (query.ContainsKey("attack[lte]") && int.TryParse(query["attack[lte]"], out max))
            {
                pokemons = pokemons.Where(p => p.Attack <= max).ToList();
            }
            if (query.ContainsKey("attack[gte]") && int.TryParse(query["attack[gte]"], out min))
            {
                pokemons = pokemons.Where(p => p.Attack >= min).ToList();
            }
            if (query.ContainsKey("defense[lte]") && int.TryParse(query["defense[lte]"], out max))
            {
                pokemons = pokemons.Where(p => p.Defense <= max).ToList();
            }
            if (query.ContainsKey("defense[gte]") && int.TryParse(query["defense[gte]"], out min))
            {
                pokemons = pokemons.Where(p => p.Defense >= min).ToList();
            }
            #endregion
            /* Searches Pokemon by 'Name' parameter
             * Firstly, by exact match;
             * Secondly, by Names that Contains 'name' in the string
             */
            if (!string.IsNullOrEmpty(name))
            {
                var poks = pokemons.Where(p => p.Name == name);
                pokemons = pokemons.Where(p => p.Name.Contains(name) && p.Name != name).ToList();
                pokemons.InsertRange(0, poks);
            }

            var model = PagingList.Create(pokemons, 10, page);
            return model;
        }
        [HttpGet("{id}")]
        public Pokemon Get(int id)
        {
            return csvFile.GetPokemons().FirstOrDefault(pokemon => pokemon.Id == id);
        }
    }
}
