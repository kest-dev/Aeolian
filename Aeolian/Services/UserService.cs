using MySql.Data;
using MySql.Data.MySqlClient;
using Aeolian.Models;
using Isopoh.Cryptography.Argon2;
using System.Data;
using static Aeolian.Services.UserService;

namespace Aeolian.Services
{
    public class UserService
    {
        public enum USER_TYPE
        {
            CREATE,
            GET,
            UPDATE,
            DELETE
        }

        public enum GET_TYPE
        {
            ID,
            USERNAME
        }

        private static UserService instance;
        private static string connectionString;

        public static UserService GetInstance(string connString)
        {
            if (instance == null)
            {
                connectionString = connString;
                instance = new UserService();
            }

            return instance;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM user;", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User() { 
                        Id = reader.GetInt32("id"), 
                        username = reader.GetString("username"),
                        password = reader.GetString("password"), 
                        userType = reader.GetInt32("userType")
                    });
                }

                Console.WriteLine(users.ToString());

                conn.Close();

                return users;
            }
        }

        private string UserSQLQuery(GET_TYPE getType)
        {
            if (getType == GET_TYPE.ID)
                return "SELECT * FROM user where id = @Identity";
            else
                return "SELECT * FROM user where username = @Identity";
        }

        public User GetUser(USER_TYPE operType, GET_TYPE getType, string? identity)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                User user = new User();
                MySqlCommand cmd = null;
                MySqlDataReader reader = null;

                switch (operType)
                {
                    case USER_TYPE.CREATE:


                        break;
                    case USER_TYPE.UPDATE:
                        cmd = new MySqlCommand(UserSQLQuery(getType), conn);
                        cmd.Parameters.Add("@Identity", MySqlDbType.String);
                        cmd.Parameters["@Identity"].Value = identity;

                        reader = cmd.ExecuteReader();

                        break;
                    case USER_TYPE.DELETE:
                        cmd = new MySqlCommand(UserSQLQuery(getType), conn);
                        cmd.Parameters.Add("@Identity", MySqlDbType.String);
                        cmd.Parameters["@Identity"].Value = identity;

                        reader = cmd.ExecuteReader();

                        break;
                    default:
                        cmd = new MySqlCommand(UserSQLQuery(getType), conn);
                        cmd.Parameters.Add("@Identity", MySqlDbType.String);
                        cmd.Parameters["@Identity"].Value = identity;

                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            user = new User()
                            {
                                Id = reader.GetInt32("id"),
                                username = reader.GetString("username"),
                                password = reader.GetString("password"),
                                userType = reader.GetInt32("userType")
                            };
                        }

                        break;
                }

                conn.Close();

                return user;
            }
        }
    }
}