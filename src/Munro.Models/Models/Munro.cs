using System;
using Munro.Models.Enums;

namespace Munro.Models.Models
{
    public class Munro
    {
        public string Name {get; set; }
        public double HeightMetres { get; set; }
        public HillCategory HillCategory { get; set; }
        public string GridReference { get; set; }
    }
}
