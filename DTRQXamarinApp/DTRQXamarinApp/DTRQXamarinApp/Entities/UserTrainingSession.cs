using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTRQXamarinApp.Entities
{
    public class UserTrainingSession
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TrainingSessionId { get; set; }
    }
}
