namespace API.Game.Backend.Database.Tables
{
    public class Animal
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Health { get; set; }
        public int Hunger { get; set; }
        public bool Hungry { get; set; }
        public DateTime lastFed { get; set; }
    }
}
