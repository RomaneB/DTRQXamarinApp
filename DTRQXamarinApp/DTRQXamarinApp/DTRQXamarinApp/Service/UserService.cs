﻿using DTRQXamarinApp.Entities;
using DTRQXamarinApp.IRepository;
using DTRQXamarinApp.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTRQXamarinApp.Service
{
    public class UserService : IService<User>
    {
        public IRepository<User> UserRepository { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRepository"></param>
        public UserService(IRepository<User> userRepository)
        {
            UserRepository = userRepository;
        }

        public int Add(User entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetByid(int id)
        {
            throw new NotImplementedException();
        }

        public User Update(User entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method that verifies that the username and password of the user are valid.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int GetUserIdByUserAndPassword(string user, string password)
        {
            try
            {
                IEnumerable<User> lstUsers = UserRepository.GetAll();
                return lstUsers.Where(s => s.Username == user && s.Password == password).Select(s => s.Id).FirstOrDefault();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
    }
}
