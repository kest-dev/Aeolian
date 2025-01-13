namespace Aeolian.Models
{
    public class User
    {
        public int Id { get; set; }

        public string username { get; set; }

        //NEVER SAVE THE PASSWORD IN PLAINTEXT
        public string password { get; set; }

        public int userType { get; set; }
    }
}
