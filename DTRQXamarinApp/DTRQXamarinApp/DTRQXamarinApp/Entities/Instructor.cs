using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTRQXamarinApp.Entities
{
    public class Instructor
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
