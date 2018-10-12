using System.Collections.Generic;
using System.Linq;

namespace sample
{

    public class UserRepository
      {
          public List<User> TestUsers;
          public UserRepository()
          {
              TestUsers = new List<User>();
              TestUsers.Add(new User() { Username = "Test1", Password  = "Pass1",Designation="SME"});
              TestUsers.Add(new User() { Username = "Test2", Password = "Pass2",Designation="Learner"});
          }
          public User GetUser(string username)
          {
              try
              {
                  return TestUsers.First(user => user.Username.Equals(username));
              } catch
              {
                  return null;
              }
          }
}

}