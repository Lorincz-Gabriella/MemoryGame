using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JocMemory.Model
{
    public class User
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon {  get; set; }
    }
}
