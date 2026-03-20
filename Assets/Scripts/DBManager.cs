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

        //En cas que no existeixi
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

        Debug.Log("Tabla creada o ya existente");
    }

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

        Debug.Log("Item guardado correctamente en la base de datos " + item.nameItem);

    }
}
