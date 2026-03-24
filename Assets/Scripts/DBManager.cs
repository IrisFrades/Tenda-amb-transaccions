using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;
using JetBrains.Annotations;
using System.Runtime.InteropServices;
using System.Collections.Generic;

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
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();

            using(var transaction = connection.BeginTransaction()) //empezamos una transaccion
            {
                try
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            "INSERT OR REPLACE INTO Item (name, description, quantity) " +
                            "VALUES (@name, @description, @quantity)";

                        command.Parameters.AddWithValue("@name", item.nameItem);
                        command.Parameters.AddWithValue("@description", item.descriptionItem);
                        command.Parameters.AddWithValue("@quantity", item.quantityItem);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();//si no hay errores, se guardan los cambios
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("No se ha podido insertar el item : " + ex.Message);
                    transaction.Rollback(); //revertemos los cambios
                }
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
                    "price REAL NOT NULL)"; //para que salgan los decimales

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
                    "money REAL NOT NULL)";

                command.ExecuteNonQuery();
            }

            // Crear dinero inicial si no existe
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO User (ID, money) VALUES (1, 1000)";
                command.ExecuteNonQuery();
            }
        }

        Debug.Log("Tabla User creada o ya existia");
    }

    //Metodo para sacar el precio del dinero
    public float GetItemPrice(string itemName)
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
                    return float.Parse(result.ToString());
            }
        }

        Debug.LogWarning("No se encontró el precio del item: " + itemName);
        return 0;
    }

    //Metodo para sacar el dinero que tiene el usuario
    public float GetUserMoney()
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT money FROM User WHERE ID = 1";

                var result = command.ExecuteScalar();

                if (result != null)
                    return float.Parse(result.ToString());
            }
        }

        Debug.LogWarning("No se encontró el dinero del jugador");
        return 0;
    }

    //Actualizar el dinero de la base cuando se compran items en el inventario
    public void UpdateUserMoneyWhenBuying(float newMoney)
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            
            using( var transaction = connection.BeginTransaction()) //creamos una transaccion (o sea todo lo que hay dentro se tiene que guardar y si hay algun error no se guardara nada)
            {
                try //En caso que salte algun error saltara al catch
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE User SET money = @money WHERE ID = 1";
                        command.Parameters.AddWithValue("@money", newMoney);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit(); //si todo esta correcto, se guardan los cambios
                }
                catch (System.Exception ex) //si hay algun error dentro del try pasara aqui
                {
                    Debug.LogError("No se ha podido actualizar el dinero: " + ex.Message); 

                    transaction.Rollback(); //En el caso que haya echo algun cambio, lo deshace
                }
            }
        }
    }

    //Actualizar el dinero de la base cuando se venden los items en el inventario
    public void UpdateUserMoneyWhenSelling(float newMoney)
    {
        using(var connection = new SqliteConnection(dbPath))
        {
            connection.Open();

            using( var transaction = connection.BeginTransaction()) //Empezamos transaccion
            {
                try
                {
                    using(var command = connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE User SET money = @money WHERE ID = 1"; //Actualizar el dinero de la base al recuperar dinero
                        command.Parameters.AddWithValue("@money", newMoney);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit(); //Se ejecuta la transaccion si todo esta correcto

                }catch(System.Exception ex)
                {
                    Debug.LogError("No se ha podido actualizar el dinero: " + ex.Message);

                    transaction.Rollback(); //Deshace los cambios al haber error
                }
            }
        }
    }
    //Metode per carregar els items que te el player
    public List<Item> LoadPlayerItems()
    {
        List<Item> items = new List<Item>();

        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT name, description, quantity FROM Item";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Item item = new Item();
                        item.nameItem = reader.GetString(0);
                        item.descriptionItem = reader.GetString(1);
                        item.quantityItem = reader.GetInt32(2);

                        // Recuperar preu i sprite del catàleg
                        ShopManager shop = FindObjectOfType<ShopManager>();
                        Item shopItem = shop.allShopItems.Find(i => i.nameItem == item.nameItem);

                        if (shopItem != null)
                        {
                            item.money = shopItem.money;
                            item.imageItem = shopItem.imageItem;
                        }

                        items.Add(item);
                    }
                }
            }
        }

        return items;
    }


}
