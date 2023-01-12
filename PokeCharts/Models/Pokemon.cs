namespace PokeCharts.Models
{
    public class Pokemon
    {
        public int id { get; }
        public string name { get;  }
        public float height { get; }
        public float weight { get; }
        public string sprite { get; }

        public Pokemon(int id, string name, float height, float weight, string sprite)
        {
            this.id = id;
            this.name = name;
            this.height = height;
            this.weight = weight;
            this.sprite = sprite;
        }

        
    }
}
