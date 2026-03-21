using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;

public class DBManager : MonoBehaviour
{
    private string dbPath;

    private void Awake()
    {
        string sourcePath = Application.dataPath + "/Plugins/botiga.db";
        string destPath = Application.persistentDataPath + "/botiga.db";

        //En caso de que no exista
        if (!File.Exists(destPath))
        {
            File.Copy(sourcePath, destPath);
            Debug.Log("Base de datos copiada a: " + destPath);
        }
        else
        {
            Debug.Log("La base de datos ya existía en: " + destPath);
        }

        dbPath = "URI=file:" + destPath;

        CreateTableIfNotExists();
        CreateShopTable();
        CreateUserTable();
    }

    private void CreateTableIfNotExists()
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS Item (" +
                    "ID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "name TEXT NOT NULL UNIQUE, " +
                    "description TEXT NOT NULL UNIQUE, " +
                    "quantity INTEGER NOT NULL)";

                command.ExecuteNonQuery();
            }
        }

        Debug.Log("Tabla creada o ya existe");
    }

    //Metodo para añadir los items a la base de datos
    public void InsertItemToDB(Item item)
    {
        using(var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using(var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT OR REPLACE INTO Item (name, description, quantity) " +
                    "VALUES (@name, @description, @quantity)";


                command.Parameters.AddWithValue("@name", item.nameItem);
                command.Parameters.AddWithValue("@description", item.descriptionItem);
                command.Parameters.AddWithValue("@quantity", item.quantityItem);

                command.ExecuteNonQuery();   
                

            }
        }

        Debug.Log("Item guardado en la base de datos " + item.nameItem);

    }

    //Tabla de ShopItem(para los items de la tienda)
    private void CreateShopTable()
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS ShopItem (" +
                    "ID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "name TEXT NOT NULL UNIQUE, " +
                    "description TEXT NOT NULL, " +
                    "price INTEGER NOT NULL)";

                command.ExecuteNonQuery();
            }
        }

        Debug.Log("Tabla ShopItem creada o ya existia");
    }

    //Tabla de user (para la cantidad de dinero que tendra)
    private void CreateUserTable()
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS User (" +
                    "ID INTEGER PRIMARY KEY, " +
                    "money INTEGER NOT NULL)";

                command.ExecuteNonQuery();
            }

            // Crear dinero inicial si no existe
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT OR IGNORE INTO User (ID, money) VALUES (1, 1000)";
                command.ExecuteNonQuery();
            }
        }

        Debug.Log("Tabla User creada o ya existia");
    }

    //Metodo para sacar el precio del dinero
    public int GetItemPrice(string itemName)
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT price FROM ShopItem WHERE name = @name";
                command.Parameters.AddWithValue("@name", itemName);

                var result = command.ExecuteScalar();

                if (result != null)
                    return int.Parse(result.ToString());
            }
        }

        Debug.LogWarning("No se encontró el precio del item: " + itemName);
        return 0;
    }

    //Metodo para sacar el dinero que tiene el usuario
    public int GetUserMoney()
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT money FROM User WHERE ID = 1";

                var result = command.ExecuteScalar();

                if (result != null)
                    return int.Parse(result.ToString());
            }
        }

        Debug.LogWarning("No se encontró el dinero del jugador");
        return 0;
    }

    //Metodo para actualizar el dinero del usuario cuando se compra
    public void UpdateUserMoney(int newMoney)
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE User SET money = @money WHERE ID = 1";
                command.Parameters.AddWithValue("@money", newMoney);

                command.ExecuteNonQuery();
            }
        }

        Debug.Log("Dinero del jugador actualizado: " + newMoney);
    }




}
