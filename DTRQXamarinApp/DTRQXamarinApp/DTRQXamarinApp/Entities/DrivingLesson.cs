using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTRQXamarinApp.Entities
{
    
    public class DrivingLesson
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public int InstructorId { get; set; }
    }
}
