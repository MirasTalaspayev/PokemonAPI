using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonAPI.Models;
using PokemonAPI.Services;
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
        public IEnumerable<Pokemon> Get()
        {
            return csvFile.GetPokemons();
        }
    }
}
