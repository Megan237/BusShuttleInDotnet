using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using Microsoft.VisualBasic;
using WebMvc.Database;
namespace WebMvc.Service
{
    public class UserService : UserServiceInterface
    {
        private readonly BusDb _busDb;

        public UserService(BusDb busDb)
        {
            _busDb = busDb;
        }
        public List<UserModel> GetUsers()
        {
            var userList = _busDb.User.Select(u => new UserModel(u.Id, u.FirstName, u.LastName, u.UserName, u.Password)).ToList();
            return userList;
        }

        public void CreateUser(string firstname, string lastname, string userName, string password)
        {
            var newUser = new Database.User
            {
                FirstName = firstname,
                LastName = lastname,
                UserName = userName,
                Password = password
            };
            _busDb.User.Add(newUser);
            _busDb.SaveChanges();

        }


        public UserModel? FindUserByID(int id)
        {
            var user = _busDb.User.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                return new UserModel(user.Id, user.FirstName, user.LastName, user.UserName, user.Password);
            }
            return null;
        }
    }
}