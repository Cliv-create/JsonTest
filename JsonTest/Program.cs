using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Microsoft.VisualBasic;

namespace JsonTest
{
    [JsonSerializable(typeof(Goods))]
    [JsonSerializable(typeof(List<Goods>))]
    internal partial class JsonContext : JsonSerializerContext { }
    public class Goods
	{
		public string Brand;
		public string Model;
		public string Submodel;
		public int Memory;
		public int Quantity;

		public Goods()
		{ }

		public Goods(string lines)
		{
            string[] values = lines.Split('\t');
            this.Brand = values[0];
            this.Model = values[1]; 
            this.Submodel = values[2];
            this.Memory = Convert.ToInt32(values[3]);
            this.Quantity = Convert.ToInt32(values[4]);
        }
        public Goods(string Brand, string Model, string Submodel, int Memory, int Quantity)
		{
			this.Brand = Brand;
			this.Model = Model;
			this.Submodel = Submodel;
			this.Memory = Memory;
			this.Quantity = Quantity;
		}

        public override string ToString()
        {
           return $"{Brand}\t{Model}\t{Submodel}\t{Memory}\t{Quantity}";
        }
    }
    internal class Program
    {
        static string dbPath = @"C:\Users\User\Desktop\It_Step\.NET\Project\ConsoleJsonSDTest\JsonTest\JsonTest\json_goods.json";
        static bool DEBUG_MODE = true;
        // if (DEBUG_MODE == true) Console.WriteLine("");
        static void Main(string[] args)
        {
            

            var goods = ReadAll();
            var newGood = new Goods("Xiaomi", "Huanan", "Huanan-last-good", 256, 10);
            goods.Add(newGood);
            // SaveAll(goods);
            newGood = new Goods("Samsung", "Galaxy", "Note10", 256, 20);
            goods.Add(newGood);
            SaveAll(goods);

            Console.WriteLine("Current Users (created Xiaomi and Samsung):");
            goods.ForEach(goods => Console.WriteLine($"Brand: {goods.Brand}, Model: {goods.Model}\nSubmodel: {goods.Submodel}, Memory: {goods.Memory}, Quantity: {goods.Quantity}\n"));
            
            var goodToEdit = goods.Find(u => u.Brand == "Xiaomi");
            if (goodToEdit != null)
            {
            goodToEdit.Brand = "Huawei";
            Console.WriteLine("Current Users (edited):");
            goods.ForEach(goods => Console.WriteLine($"Brand: {goods.Brand}, Model: {goods.Model}\nSubmodel: {goods.Submodel}, Memory: {goods.Memory}, Quantity: {goods.Quantity}\n"));
            SaveAll(goods);
            }


            goods.RemoveAll(u => u.Brand == "Huawei");
            Console.WriteLine("Current Users (removed Huawei):");
            goods.ForEach(goods => Console.WriteLine($"Brand: {goods.Brand}, Model: {goods.Model}\nSubmodel: {goods.Submodel}, Memory: {goods.Memory}, Quantity: {goods.Quantity}\n"));
            SaveAll(goods);

            Console.WriteLine("Current Users (after all edits):");
            goods.ForEach(goods => Console.WriteLine($"Brand: {goods.Brand}, Model: {goods.Model}\nSubmodel: {goods.Submodel}, Memory: {goods.Memory}, Quantity: {goods.Quantity}"));
            // RemoveEmptyRecords();
            Console.WriteLine("Program ended!");
        }



        static List<Goods> ReadAll()
        {
        if (!File.Exists(dbPath)) return new List<Goods>();
        var options = new JsonSerializerOptions { WriteIndented = true, TypeInfoResolver = JsonContext.Default }; // Форматированный JSON
        var json = File.ReadAllText(dbPath);
        if (string.IsNullOrWhiteSpace(json))
        {
            if (DEBUG_MODE == true) Console.WriteLine("json is null!\n");
            return new List<Goods>();
        }
        return JsonSerializer.Deserialize<List<Goods>>(json, options) ?? new List<Goods>();
        }

        static void SaveAll(List<Goods> goods)
        {
        var options = new JsonSerializerOptions { WriteIndented = true, TypeInfoResolver = JsonContext.Default }; // Форматированный JSON
        var json = JsonSerializer.Serialize(goods, options);
        File.WriteAllText(dbPath, json);
        }

        static bool RemoveEmptyRecords()
        {
            if (!File.Exists(dbPath)) return false;
            var options = new JsonSerializerOptions { WriteIndented = true, TypeInfoResolver = JsonContext.Default }; // Форматированный JSON
            var json = File.ReadAllText(dbPath);
            if (string.IsNullOrWhiteSpace(json)) return false;
            var goods_list = JsonSerializer.Deserialize<List<Goods>>(json, options);
            if (goods_list.Count == 0 || goods_list == null) return false;

            // Removal logic
            /*
            for (int i = goods_list.Count - 1; i >= 0; i--)
            {
                Goods item = goods_list[i];
                bool res = false;
                if (string.IsNullOrWhiteSpace(item.Brand) && string.IsNullOrWhiteSpace(item.Model)) goods_list.RemoveAt(i);
                if (DEBUG_MODE == true) Console.WriteLine("Removed record!");
            }
            */
            int removedCount = goods_list.RemoveAll(item => string.IsNullOrEmpty(item.Brand) && string.IsNullOrEmpty(item.Model));


            if (DEBUG_MODE == true)
            {
            Console.WriteLine($"Removed {removedCount} elements from list.");
            Console.WriteLine("Showing goods_list after removing records");
            goods_list.ForEach(goods => Console.WriteLine($"Brand: {goods.Brand}, Model: {goods.Model}\nSubmodel: {goods.Submodel}, Memory: {goods.Memory}, Quantity: {goods.Quantity}"));
            }

            SaveAll(goods_list);
            if (DEBUG_MODE == true) Console.WriteLine("Finished removing records!");
            return true;
        }

    }
}
