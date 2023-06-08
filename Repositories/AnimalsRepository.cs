using API.Game.Backend.eventArgs;
using API.Game.Backend.Interfaces;
using API.Game.Backend.Database.Tables;
using API.Game.Backend.Database;
// using System.Text.Json;


namespace API.Game.Backend.Repositories
{
    public class AnimalsRepository : IAnimals
    {
        private List<Animal> _animals;
        //private readonly string animalsFile = @"animals.json";

        protected event EventHandler<TickerEventArgs>? Ticked;

        public AnimalsRepository()
        {
            Load();
        }

        public IEnumerable<Animal> GetAll()
        {
            return _animals;
        }

        public Animal Get(Guid id)
        {
            return Fetch(id);
        }

        public string Feed(Guid id)
        {
            Load();

            var animal = _animals.Where(x => x.Id == id).FirstOrDefault();

            if (!CheckVitality(animal))
            {
                if (animal.Health <= 0)
                    animal.Health = 0;
                Save();
                return $"{animal.Name} appears to be dead!";
            }
            else if (CheckVitality(animal))
            {
                animal.lastFed = DateTime.UtcNow;
                if (animal.Hunger > 0)
                {
                    animal.Hunger = animal.Hunger - 10;
                    if (animal.Hunger <= 0)
                        animal.Hunger = 0;
                    if (animal.Hunger < 80)
                        animal.Hungry = false;
                    if (animal.Health < 100 && animal.Health >= 1)
                    {
                        animal.Health = animal.Health + 10;
                        if (animal.Health > 100)
                            animal.Health = 100;
                    }

                    Save();
                }
            }
            return $"{animal.Name} has been fed!";
        }

        public string FeedAll()
        {
            _animals.ForEach(x => Feed(x.Id));

            return "You fed all animals!";
        }

        public string Pet(Guid id)
        {
            var animal = _animals.Where(x => x.Id == id).FirstOrDefault();

            if (animal.Name.ToLower().Contains("monkey") && animal.Health > 0)
                return $"You pet the {animal.Name}.. it does a backflip!";
            if (animal.Name.ToLower().Contains("snake") && animal.Health > 0)
                return $"You pet the {animal.Name}, it hisses happily!";
            if (animal.Name.ToLower().Contains("bird") && animal.Health > 0)
            {
                _animals.Remove(animal);
                Save();
                return $"You attempt to pet the {animal.Name}.. Oh no! it flew away!";
            }
            if (animal.Health <= 0)
                return $"You pet the {animal.Name} but it does not seem to react..\n" +
                  "Are you sure it is still alive?";

            return $"You pet the {animal.Name}!";
        }

        public string Breed(Guid first, Guid second)
        {
            Load();

            var firstAnimal = _animals.Where(x => x.Id == first).FirstOrDefault();
            var secondAnimal = _animals.Where(x => x.Id == second).FirstOrDefault();

            string crossbreed = $"{firstAnimal.Name} {secondAnimal.Name}";

            var animal = new Animal()
            {
                Id = Guid.NewGuid(),
                Name = crossbreed,
                Age = 0,
                Health = 100,
                lastFed = DateTime.Now,
                Hunger = 10,
                Hungry = false
            };

            _animals.Add(animal);
            Save();

            return $"Congratulations, you now have a {crossbreed}!";
        }

        public string Generate()
        {
            Load();

            const string file = @"Resources/animals.txt";

            var r = new Random();
            string[] animalNames = File.ReadAllLines(file);
            var randomLine = animalNames[r.Next(animalNames.Length)];

            var animal = new Animal()
            {
                Id = Guid.NewGuid(),
                Name = randomLine,
                Age = 0,
                Health = 100,
                lastFed = DateTime.Now,
                Hunger = 10,
                Hungry = false
            };

            _animals.Add(animal);
            Save();

            return $"Woah, you found a {animal.Name}!";
        }

        public string Delete(Guid id)
        {
            var animal = Fetch(id);
            _animals.Remove(animal);
            Save();
            if (animal.Health == 0)
                return $"You dig a grave for {animal.Name} and bury the animal.";

            return $"You released {animal.Name} back into the wild!";
        }

        protected Animal Fetch(Guid id)
        {
            return _animals.First(m => m.Id == id);
        }

        private void Save()
        {

            var DbContext = DatabaseConnector.CreateContext();

            // Delete existing animals
            var existingAnimals = DbContext.Animals.ToList();
            DbContext.Animals.RemoveRange(existingAnimals);

            // Load animals from list
            DbContext.Animals.AddRange(_animals);

            // Save changes to the database
            DbContext.SaveChanges();


            /* Writing to .json
            string json = JsonSerializer.Serialize(_animals);
            File.WriteAllText(animalsFile, json);
            */
        }

        private IEnumerable<Animal> Load()
        {

            var DbContext = DatabaseConnector.CreateContext();

            return _animals = DbContext.Animals.ToList();


            /* Loading from .json
            if (!File.Exists(animalsFile))
                return Enumerable.Empty<Animal>();

            var jsonSerialized = File.ReadAllText(animalsFile);
            return _animals = JsonSerializer.Deserialize<List<Animal>>(jsonSerialized);
            */
        }

        protected void OnEvery10Seconds(TimeOnly time)
        {
            if (time.Second % 10 == 0)
            {
                Load();
                _animals.ForEach(x => IncreaseHunger(x));
                _animals.ForEach(x => DecreaseHealth(x));
                Save();
            }
        }

        protected void OnEveryMinute(TimeOnly time)
        {
            if (time.Second % 60 == 0)
            {
                Load();
                _animals.ForEach(x => IncreaseAge(x));
                Save();
            }
        }

        private void IncreaseHunger(Animal animal)
        {
            if (CheckVitality(animal))
                if (animal.Hunger < 100)
                {
                    animal.Hunger = animal.Hunger + 1;
                    if (animal.Hunger >= 80)
                        animal.Hungry = true;
                    else if (animal.Hunger < 80)
                        animal.Hungry = false;
                }
                else
                {
                    animal.Hunger = 100;
                    animal.Hungry = true;
                }

            else if (!CheckVitality(animal))
                return;
        }

        private void IncreaseAge(Animal animal)
        {
            if (CheckVitality(animal))
                if (animal.Age < 300)
                    animal.Age = animal.Age + 1;
                else
                    animal.Health = 0;

            else if (!CheckVitality(animal))
                return;
        }

        private void DecreaseHealth(Animal animal)
        {
            if (CheckVitality(animal))
                if (animal.Health > 0 && animal.Hunger == 100)
                    animal.Health = animal.Health - 1;
        }

        private bool CheckVitality(Animal animal)
        {
            if (animal.Age >= 300 || animal.Health <= 0)
                return false;
            else if (animal.Health <= 100 && animal.Health > 0 && animal.Hunger >= 0)
                return true;

            return true;
        }
    }
}
