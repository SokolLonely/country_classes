using System;
using System.IO;
using System.Text.Json;
//using System.Memory;
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
}
public class Province
{
    public string name;
    public string Name
    { get { return name; } }
    public string capital;
    public string Capital
    { get { return capital; } }
    public int airport; // 0 or 1
    public int Airport
    { get { return airport; } }
    public int port; //0 or 1
    public int Port
    { get { return port; } }
    public int hotel; // 0 - 5
    public int Hotel
    { get { return hotel; } }
    public void AddHotel()
    {
        hotel++;
        attractionLevel += 1000;
    }
    public int minerals; // 0-1
    public int Minerals
    { get { return minerals; } }
    public int population;
    public int Population
    { get { return population; } }
    public int attractionLevel;
}
    public class Program
{
    public static void Main()
    {
        // Создание объекта Person
        /* Province fiji = new Province
         {
             name = "Fji",
             capital = "Suva",
             airport = 0, 
             port = 0,
             minerals = 0,
             population = 924610,
             hotel = 0,
             attractionLevel = 5000,

         };*/
        Province pr = new Province
        {
            name = "Tuvalu",
            capital = "Funafuti",
            airport = 0,
            port = 0,
            minerals = 0,
            population = 11204,
            hotel = 0,
            attractionLevel = 6500,

        };

        // Сериализация объекта Person в JSON
        string json = JsonSerializer.Serialize(pr, new JsonSerializerOptions { WriteIndented = true });

        // Запись JSON в файл
        File.WriteAllText("Tuvalu.json", json);

        Console.WriteLine("JSON file created successfully.");
        Console.ReadLine();
    }
}

