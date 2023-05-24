using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
//using System.Memory;
using System.Threading.Tasks;

namespace country_classes
{   public class Province
    {
        public string name;
        public string Name
        { get { return name; } }
        public string capital;
        public string Capital
        { get { return capital; } }
          int airport; // 0 or 1
        public int Airport
        { get { return airport; }
            set
            {
                if (value != 0 && value != 1)
                {
                    throw new Exception("must be 0 or 1");
                }
                if (airport == 0)
                { attractionLevel += 100; }
                else
                    { attractionLevel -= 100; }
                airport = value;
            }
        }
         int port; //0 or 1
        public int Port
        {
            get { return port; }
            set
            {  if (value != 0 && value != 1)
                {
                    throw new Exception("must be 0 or 1");
                }
                port = value;
            }
        }
        public int hotel; // 0 - 5
        public int Hotel 
        { get { return hotel; }
           set
            {
                if (value > 5 || value < 0)
                {
                    throw new Exception("must be integers from 0 to 5");
                }
                else
                {
                    hotel = value;
                    attractionLevel += 200;
                }
            }
        }
       // public void AddHotel()
       //{
       //    hotel++;
      //     attractionLevel += 200;
            
      //  }
        public int minerals; // 0-1
        public int Minerals
        { get { return minerals; } }
        public int population;
        public int Population
        { get { return population; } }
        public int attractionLevel; // где-то в районе 5к... 10к
        public int AttractionLevel
            {
            get { return attractionLevel; }
            }
        public Province(string name, string capital, int airport, int port, int hotel, int minerals, int population, int attractionLevel)
        {
            this.name = name;
            this.capital = capital;
            this.airport = airport;
            this.port = port;
            this.hotel = hotel;
            this.minerals = minerals;
            this.population = population;
            this.attractionLevel = attractionLevel;
        }
        //public 
        public int Income //доходы провинции за ход
        {
            get
            {
                return Convert.ToInt32(Math.Sqrt(population) + attractionLevel);
            }
        }
        public int Outcome //расходы провинции за ход
        {
            get
            {
                return (port + airport) * 200;
            }
        }
        public int Balance
        {
            get  
            {
                return this.Income - this.Outcome;
            }
        }
        public string print()
        {
            return name + " " +capital + $" port {port} airport {airport} hotel {hotel} minerals {minerals} population {population} attractionLevel {attractionLevel}";
        }
        
    }
    public enum Alliance
    {
        Usa, 
        Russia,
        China,
        Narco,
        SouthAmerica
    }
    public class Country
    {
        string name;
        List<Province> provinceList;
        // Army
        Alliance ideology; // 0 -4
        List<Country> enemies;
        int money;
        int minerals;
        int oil;
        // kredits
        public string Name
        {
            get { return name; }
        }
        public Alliance Ideology
        {
            get { return ideology; }
        }
        public int Money { get { return money; } }  
        public int Minerals { get { return minerals; } }    
        public int Oil { get { return oil; } }
        public List<Country> Enemies
            { get { return enemies; } }
        public List<Province> ProvinceList
        {
            get
            {
                return this.provinceList;
            }
        }
        public string printCountry()
        {
            string output = "name " + name + " ideology " + Convert.ToString(ideology) + " money " + money + " minerals " + minerals + " oil  " + oil;
            foreach (Province i in ProvinceList)
            {
                output += "\n";
                output += i.print(); }
            return output;
        }
        public Country(string name, Alliance ideology, List<Province> provinceList)
        {
            this.name = name;
            this.ideology = ideology;
            enemies = new List<Country>();
            money = 1000;
            minerals = 3;
            oil = 4;
            this.provinceList = provinceList;
        }
        public static List<Province> CreateProvinceList(string[] names)
        {
            List<Province> provinceList = new List<Province>();
            foreach (string el in names)
            {
                string path = el + ".json";
                string json = File.ReadAllText(path);
                Province province = JsonSerializer.Deserialize<Province>(json);
                provinceList.Add(province);
            }
            return provinceList;
        }
        public static Country Create(string name, Alliance ideology)
        {
            string[] provinceMass = new string[1];
            provinceMass[0] = name;
            List<Province> provinceList = CreateProvinceList(provinceMass);
            return new Country(name, ideology, provinceList);
        }
        
        
        public void BuildAirport(string provinceName)
        {
            foreach (Province province in this.provinceList)
            {
                if (province.Name == provinceName)
                {
                    if (province.Airport == 0)
                    {   if (this.money > 2000)
                        {
                            province.Airport = 1;
                            this.money -= 2000;
                        }
                        CheckFinances();
                    }
                }
                return;
            }
        }
        public void BuildPort(string provinceName)
        {
            foreach (Province province in this.provinceList)
            {
                if (province.Name == provinceName)
                {
                    if (province.Port == 0)
                    {
                        if (this.money > 2000)
                        {
                            province.Port = 1;
                            this.money -= 2000;
                            CheckFinances();
                        }
                    }
                }
                return;
            }
        }
        public void BuildHotel(string provinceName)
        {
            foreach (Province province in this.provinceList)
            {
                if (province.Name == provinceName)
                {
                    if (province.Hotel < 5)
                    {
                        int price = province.Hotel * 500;
                        if (this.money - price > 0)
                        {
                            province.Hotel++;
                            this.money -= price;
                        }
                        CheckFinances();
                        
                    }
                }
                return;
            }
        }
        public void CheckFinances()
        {
            if (this.money < -5000)
            {
                throw new Exception("run out of money");
            }
        }
        public void AutoOilBuy()
        {
            if (this.oil < 0)
            {
                this.money -= this.oil * -500;
                this.oil = 0;
                CheckFinances();
            }
        }
        public void AutoMineralsBuy()
        {
            if (this.minerals < 0)
            {
                this.money -= this.minerals * -500;
                this.minerals = 0;
                CheckFinances();
            }
        }
        public void UpdateNewTurn()
        {
            foreach(Province province in this.provinceList)
            {
                this.money += province.Balance;
                this.oil--;
                this.minerals--;
            }
            CheckFinances();
            if (this.minerals < 0)
            {
                this.money -= 500;

            }
            if (this.oil < 0)
            {
                this.money -= 500;
            }
            AutoMineralsBuy();
            AutoOilBuy();
            /*if (this.minerals < -2 || this.oil < -2)
            {
                throw new Exception("run out of resourses");

            }*/
            
        }
        public void BuyOil(Alliance country)
        {
          
        }


    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string json = File.ReadAllText("Palau.json"); // Предполагается, что файл person.json находится в той же папке, что и исполняемый файл программы
            //"C:\Users\Алексей\Documents\Сема\школа\ЛИТ\country_classes\json_province_creator\bin\Debug\fiji.json"
            // Десериализация JSON в объект Person
            Province province = JsonSerializer.Deserialize<Province>(json);
            Country Palau = Country.Create("Palau", Alliance.China);
            Console.WriteLine(Palau.printCountry());
            Palau.BuildAirport("Palau");
            Palau.BuildPort("Palau");
            Palau.BuildHotel("Palau");
            Palau.BuildHotel("Palau");
            for (int i = 0; i < 10; i++)
            {
                Palau.UpdateNewTurn();
                Console.WriteLine(Palau.printCountry());
            }
            
            Console.ReadLine();
        }
    }
}
