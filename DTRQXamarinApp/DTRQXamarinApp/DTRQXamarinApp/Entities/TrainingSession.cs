using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTRQXamarinApp.Entities
{
    public class TrainingSession
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int AvailableSeat { get; set; }
    }
}
