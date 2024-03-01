using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace TaskManager
{
    class SqlHelper
    {
        public void connect(MySqlConnection conn)
        {

            if (conn.State == ConnectionState.Closed)
            {
                string connStrLocal = @"server=localhost;uid=root;pwd=1234;database=taskmanager;Convert Zero Datetime=True;";
                conn.ConnectionString = connStrLocal;
                conn.Open();
            }

        }

        public int getNewId(MySqlConnection conn, string table)
        {

            string idcislo = $"SELECT id FROM " + User.name + ";";
            MySqlCommand cmd1 = new MySqlCommand(idcislo, conn);
            MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
            MyAdapter.SelectCommand = cmd1;
            DataTable dTable = new DataTable();
            MyAdapter.Fill(dTable);
            int max = Convert.ToInt32(dTable.AsEnumerable()
                        .Max(row => row["id"]));

            DateTime dateTime = DateTime.Now;
            int maxId = max + 1;

            return maxId;
        }

        public void insertTask(MySqlConnection conn, int id, int completed, string name, string description, DateTime created, DateTime completeDate)
        {
            string sql = "INSERT INTO " + User.name + " (id, completed, name, created, notes, completeDate, pinned) VALUES (@id, @completed, @name, @created, @notes, @completeDate, @pinned)";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@completed", completed);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@notes", description);
            cmd.Parameters.AddWithValue("@completeDate", completeDate);
            cmd.Parameters.AddWithValue("@created", created);
            cmd.Parameters.AddWithValue("@pinned", 0);
            cmd.ExecuteNonQuery();
        }

        public void loadAllTasks(MySqlConnection conn, List<TaskDataClass> tasks)
        {
            string sql = "SELECT * FROM " + User.name + "";

            List<TaskDataClass> pinnedTasks = new List<TaskDataClass>();
            List<TaskDataClass> notPinnedTasks = new List<TaskDataClass>();

            using (MySqlCommand command = new MySqlCommand(sql, conn))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("id"));
                        int completedInt = reader.GetInt32(reader.GetOrdinal("completed"));
                        int pinnedInt = reader.GetInt32(reader.GetOrdinal("pinned"));
                        string name = reader.GetString(reader.GetOrdinal("name"));
                        string description = reader.GetString(reader.GetOrdinal("notes"));
                        DateTime createDate = reader.GetDateTime(reader.GetOrdinal("created"));
                        DateTime completeDate = reader.GetDateTime(reader.GetOrdinal("completeDate"));

                        bool completed = false;
                        if (completedInt == 1) { completed = true; }                     

                        //MessageBox.Show(id + " " + completed + " " + name + " " + description + " " + completeDate + " " + createDate);

                        if (pinnedInt == 1) pinnedTasks.Add(new TaskDataClass(id, completed, name, description, completeDate, createDate, true));
                        else notPinnedTasks.Add(new TaskDataClass(id, completed, name, description, completeDate, createDate, false));

                    }
                }
            }

            foreach (TaskDataClass task in pinnedTasks)
            {
                tasks.Add(task);
            }
            foreach (TaskDataClass task in notPinnedTasks)
            {
                tasks.Add(task);
            }

        }

        public void updateTask(TaskDataClass task)
        {
            int completed = 0;
            if (task.completed) completed = 1;
            int pinned = 0;
            if (task.pinned) pinned = 1;

            string sql = "UPDATE " + User.name + " SET " +
                "name = '" + task.name + "', " +
                "notes = '" + task.description + "', " +
                "completeDate = @completeDate, " +
                "completed = '" + completed  + "', " +
                "pinned = '" + pinned + "' " +
                "WHERE id = " + task.id + ";";

            MySqlCommand cmd = new MySqlCommand(sql, MainWindow.conn);
            cmd.Parameters.AddWithValue("@completeDate", task.completeDate);

            cmd.ExecuteNonQuery();

        }

    }
}
