namespace PokeCharts.Models
{
    public class Pokemon
    {
        private int id;
        private string name;
        private float height;
        private float weight;
        private string sprite;

        public Pokemon(int id, string name, float height, float weight, string sprite)
        {
            this.id = id;
            this.name = name;
            this.height = height;
            this.weight = weight;
            this.sprite = sprite;
        }

        public int Id { get => id; }
        public string Name { get => name; }
        public float Height { get => height; }
        public float Weight { get => weight; }
        public string Sprite { get => sprite; }
    }
}
