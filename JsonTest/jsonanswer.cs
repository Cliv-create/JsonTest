/*
// можно написать что-то типа своей БД, на стандартном классе для работы с джейсоном:

using System.Text.Json;

class Program
{
    static string dbPath = @"C:\1\json_db.json";

    static void Main()
    {
        // Чтение всех записей
        var users = ReadAll();
        Console.WriteLine("Current Users:");
        users.ForEach(user => Console.WriteLine($"Login: {user.Login}, Name: {user.UserInfo.Name}"));

        // Добавление новой записи
        var newUser = new User
        {
            Login = "user3",
            Password = "pass3",
            UserInfo = new UserInfo
            {
                Name = "Alice",
                Age = 25,
                AccessLevel = "editor"
            }
        };
        users.Add(newUser);
        SaveAll(users);

        // Изменение записи
        var userToEdit = users.Find(u => u.Login == "user3");
        if (userToEdit != null)
        {
            userToEdit.UserInfo.Name = "Alice Updated";
            SaveAll(users);
        }

        // Удаление записи
        users.RemoveAll(u => u.Login == "user3");
        SaveAll(users);
    }

    static List<User> ReadAll()
    {
        if (!File.Exists(dbPath)) return new List<User>();
        var json = File.ReadAllText(dbPath);
        return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
    }

    static void SaveAll(List<User> users)
    {
        var options = new JsonSerializerOptions { WriteIndented = true }; // Форматированный JSON
        var json = JsonSerializer.Serialize(users, options);
        File.WriteAllText(dbPath, json);
    }
}
*/