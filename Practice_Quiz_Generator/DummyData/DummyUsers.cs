using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.DummyData
{
    public  static class DummyUsers
    {
        public static List<User> user = new List<User>
        {
            new User
            {
                FullName = "Hassan Rihannat",
                UserId = 0001,
                Email = "hassanrihannat@gmail.com",
                Role = "Student",
                PasswordHash = "password123"
            },

            new User
            {
                FullName = "John Uche",
                UserId = 0002,
                Email = "Johnuche@gmail.com",
                Role = "Student",
                PasswordHash = "password000"
            },
            
            new User
            {
                FullName = "Femi Bello",
                UserId = 0003,
                Email = "Femibello@gmail.com",
                Role = "Student",
                PasswordHash = "mypassword"
            }
        };
    }
}
