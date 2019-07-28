using Munro.Models.Enums;

namespace Munro.Models.Models
{
    public class MunroModel
    {
        public string Name {get; set; }
        public double HeightMetres { get; set; }
        public HillCategory HillCategory { get; set; }
        public string GridReference { get; set; }
    }
}
