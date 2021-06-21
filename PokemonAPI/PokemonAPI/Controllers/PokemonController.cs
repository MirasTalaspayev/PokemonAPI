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
        public IEnumerable<Pokemon> Get(int page = 1, string name = null)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var poks = from p in csvFile.GetPokemons() where p.Name == name select p;
                return poks;
            }
            var pokemons = csvFile.GetPokemons();
            var model = PagingList.Create(pokemons, 10, page);
            return model;
        }
    }
}
