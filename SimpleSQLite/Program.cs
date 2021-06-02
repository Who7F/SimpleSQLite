using System;
using System.Data.SQLite;

namespace SimpleSQLite
{
    class Program
    {
        static void Main(string[] args)
        {
            string MyDB = "database.db";
            string[,] DataPeople = new string[,] { { "Sue", "Tom", "Jess", "Bob" }, { "Accounts", "Tea Boy", "Dogsbody", "CEO" }, { "Assam", "Sencha", "Chamomile", "Earl Grey, Hot" } };
            string[,] DataPeople2 = new string[,] { { "Jess", "Sam" }, { "Dogsbody", "Lord" }, { "Chamomile", "Sencha" } };
            string[] DataTea = new String[] { "Earl Grey, Hot", "Assam", "Sencha", "Chamomile" };
            string TableName0 = "People";
            SQLiteConnection sqlite_conn;
            sqlite_conn = CreateConnection(MyDB);

            CreateTable(sqlite_conn, "CREATE TABLE " + TableName0 + " (ID INT NOT NULL PRIMARY KEY, Name VARCHAR(20), Job VARCHAR(20), Tea VARCHAR(20))");
            InsertData(sqlite_conn, TableName0, DataPeople);
            ReadData(sqlite_conn);

            Console.WriteLine("\n Delete Jess and Have ID 1 Tea set to Earl Grey, Hot\n");

            EditRow(sqlite_conn, "DELETE FROM " + TableName0 + " WHERE Name = 'Jess'");
            EditRow(sqlite_conn, "UPDATE " + TableName0 + " SET Tea = 'Earl Grey, Hot' WHERE ID = 1");
            ReadData(sqlite_conn);

            Console.WriteLine("\n Add More\n");

            InsertData(sqlite_conn, TableName0, DataPeople2);
            ReadData(sqlite_conn);
            
            DropTable(sqlite_conn, TableName0);
            Console.WriteLine("\n Table Dropped\n");
            
            sqlite_conn.Close();

        }

        static SQLiteConnection CreateConnection(string MyDB)
        {
            SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source=" + MyDB + ";Version=3;New=true;Compress=True");

            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
            }

            return sqlite_conn;
        }

        static void CreateTable(SQLiteConnection conn, string MyTable)
        {
            SQLiteCommand sqlite_cmd;

            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = MyTable;
            sqlite_cmd.ExecuteNonQuery();
        }

        static void InsertData(SQLiteConnection conn, string MyTable, string[,] MyArray)
        {

            SQLiteCommand sqlite_cmd = new SQLiteCommand("SELECT last_insert_rowid()", conn);
            Int64 lastID = (Int64)sqlite_cmd.ExecuteScalar();

            for (int i = 0; i < MyArray.GetLength(1); i++)
            {
                sqlite_cmd.CommandText = "INSERT INTO " + MyTable + " (ID, Name, Job, Tea) VALUES (" + (lastID + i) + ", '" + MyArray[0, i] + "', '" + MyArray[1, i] + "', '" + MyArray[2, i] + "');";
                sqlite_cmd.ExecuteNonQuery();
            }

        }

        static void EditRow(SQLiteConnection conn, string MyCommand)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = MyCommand;
            sqlite_cmd.ExecuteNonQuery();
        }

        static void DropTable(SQLiteConnection conn, string MyTable)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "DROP TABLE " + MyTable;
            sqlite_cmd.ExecuteNonQuery();

        }

        static void ReadData(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;

            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM People";

            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read())
            {
                Console.WriteLine("ID:- " + sqlite_datareader.GetValue(0) + " \tName:- " + sqlite_datareader.GetValue(1) + " \tJob:- " + sqlite_datareader.GetValue(2) + " \tTea:- " + sqlite_datareader.GetValue(3));
            }

        }
        
    }
}