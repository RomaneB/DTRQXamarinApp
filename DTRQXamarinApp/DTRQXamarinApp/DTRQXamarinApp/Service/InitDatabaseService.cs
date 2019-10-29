using DTRQXamarinApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTRQXamarinApp.Service
{
    public class InitDatabaseService
    {
        IDatabase DatabaseConnection;
        public InitDatabaseService(IDatabase database)
        {
            DatabaseConnection = database;
            InitDatabase();
        }

        public void InitDatabase()
        {
            try
            {
                using (var db = DatabaseConnection.Connection())
                {
                    db.DropTable<DrivingLesson>();
                    db.DropTable<Instructor>();
                    db.DropTable<TrainingSession>();
                    db.DropTable<User>();
                    db.DropTable<UserTrainingSession>();

                    db.CreateTable<DrivingLesson>();
                    db.CreateTable<Instructor>();
                    db.CreateTable<TrainingSession>();
                    db.CreateTable<User>();
                    db.CreateTable<UserTrainingSession>();
                    var l = new List<TrainingSession>()
                {
                    new TrainingSession {Date = new DateTime(2019,10,01,10,00,00), AvailableSeat=0},
                    new TrainingSession {Date = new DateTime(2019,10,01,8,00,00), AvailableSeat=5},
                    new TrainingSession {Date = new DateTime(2019,09,01,11,00,00), AvailableSeat=10},
                    new TrainingSession {Date = new DateTime(2019,10,29,10,00,00), AvailableSeat=5},
                    new TrainingSession {Date = new DateTime(2019,11,11,15,00,00), AvailableSeat=2},
                    new TrainingSession {Date = new DateTime(2019,10,30,16,00,00), AvailableSeat=0},
                    new TrainingSession {Date = new DateTime(2019,10,30,12,00,00), AvailableSeat=5},
                    new TrainingSession {Date = new DateTime(2019,10,12,10,00,00), AvailableSeat=3},
                    new TrainingSession {Date = new DateTime(2019,10,12,12,00,00), AvailableSeat=1},

                    new TrainingSession {Date = new DateTime(2018,12,10,10,00,00), AvailableSeat=3},
                    new TrainingSession {Date = new DateTime(2019,10,20,08,00,00), AvailableSeat=8},
                    new TrainingSession {Date = new DateTime(2019,08,15,10,00,00), AvailableSeat=0},
                    new TrainingSession {Date = new DateTime(2019,08,15,12,00,00), AvailableSeat=8},
                    new TrainingSession {Date = new DateTime(2020,03,01,15,00,00), AvailableSeat=15},
                    new TrainingSession {Date = new DateTime(2020,03,01,15,30,00), AvailableSeat=15},
                    new TrainingSession {Date = new DateTime(2019,12,10,15,00,00), AvailableSeat=15}
                };

                    DateTime d = new DateTime(2019, 10, 29, 10, 30, 00);
                    var ds = d.ToShortDateString();
                    db.InsertAll(l);


                    db.InsertAll(new List<User>()
                {
                    new User{Username="Romane", Password="admin"},
                    new User{Username="Quentin", Password="admin"},
                    new User{Username="Dylan", Password="admin"},
                    new User{Username="Tiphaine", Password="admin"}
                });

                    db.Insert(new UserTrainingSession() { TrainingSessionId = 1, UserId = 1 });
                    db.Insert(new UserTrainingSession() { TrainingSessionId = 5, UserId = 1 });
                    db.Insert(new UserTrainingSession() { TrainingSessionId = 10, UserId = 1 });
                    db.Insert(new UserTrainingSession() { TrainingSessionId = 4, UserId = 2 });
                    db.Insert(new UserTrainingSession() { TrainingSessionId = 13, UserId = 1 });
                    db.Insert(new UserTrainingSession() { TrainingSessionId = 1, UserId = 3 });

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
    }
}
