using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;

namespace PracticeTrain
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                
                var newtrain = new Trains
                {

                    Name = "ATrain",
                    Number = 4214,
                    DepartureStation = "Bilgorod-Dnistrovksyi" ,
                    ArrivedStation = "Odessa",
                    NumOfCarriages = 9
                };

                
                var newtrain2 = new Trains
                {

                    Name = "Pasc",
                    Number = 4214,
                    DepartureStation = "Bilgorod-Dnistrovksyi",
                    ArrivedStation = "Kyiv",
                    NumOfCarriages = 5
                };
                db.Train.Add(newtrain);
                db.Train.Add(newtrain2);
                db.SaveChanges();

                db.GetAllTrains();

                int idRemove = 3;
                db.RemoveByTrainId(idRemove);
            }
        }

        

    }
    public class Trains
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string DepartureStation { get; set; }
        public string ArrivedStation   { get; set; }
        public int NumOfCarriages { get; set; }

        public Trains() { }
        public Trains(int id, string name, int number, string departureStation, string arrivedStation, int numOfCarriages)
        {
            Id = id;
            Name = name;
            Number = number;
            DepartureStation = departureStation;
            ArrivedStation = arrivedStation;
            NumOfCarriages = numOfCarriages;
        }

    }

    class ApplicationContext : DbContext
    { 
        

        public DbSet<Trains> Train { get; set; }

        public void RemoveByTrainId(int id)
        {
            var train = Train.Find(id);
            if (train != null)
            {
                Train.Remove(train);
                SaveChanges();
            }
            
        }

        public void AddChanges(int id, Trains train)
        {
            using (var context = new ApplicationContext())
            {
                var currentTrain = context.Train.Find(id);

                if (currentTrain != null)
                {
                    currentTrain.Name = train.Name;
                    currentTrain.Number = train.Number;
                    currentTrain.DepartureStation = train.DepartureStation;
                    currentTrain.ArrivedStation = train.ArrivedStation;
                    currentTrain.NumOfCarriages = train.NumOfCarriages;

                    context.SaveChanges();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void AddTrain(Trains train)
        {
            using (var context = new ApplicationContext())
            {
                context.Train.Add(train);
                context.SaveChanges();
            }
        }



        public void GetAllTrains()
        {
            var trains = Train.ToList();  
            foreach (var train in trains)
            {
                Console.WriteLine($"Id: {train.Id}, Name: {train.Name}, Number: {train.Number}, DepartureStation: {train.DepartureStation}, ArrivedStation: {train.ArrivedStation}, NumOfCarriages: {train.NumOfCarriages}");
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) 
            .AddJsonFile("appsettings.json")               
            .Build();

            var connection = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connection);
        }
    }
}
