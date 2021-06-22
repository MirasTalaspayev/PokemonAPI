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
        public IEnumerable<Pokemon> GetAll(int page = 1, string name = null) //, string[] filters = null
        {
            List<Pokemon> pokemons = csvFile.GetPokemons();
            if (!string.IsNullOrEmpty(name))
            {
                var poks = pokemons.Where(p => p.Name == name);
                pokemons = pokemons.Where(p => p.Name.Contains(name) && p.Name != name).ToList();
                pokemons.InsertRange(0, poks);
            }
            //if (filters != null)
            //{
            //    foreach (string filter in filters)
            //    {
            //        Console.WriteLine(filter);
            //    }
            //}
            
            var model = PagingList.Create(pokemons, 10, page);
            return model;
        }
        
    }
}
