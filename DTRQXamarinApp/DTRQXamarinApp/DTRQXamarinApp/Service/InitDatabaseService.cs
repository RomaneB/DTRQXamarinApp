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
                    //Clear database
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

                    //Init Training Sessions
                    db.InsertAll(new List<TrainingSession>()
                    {
                    new TrainingSession {Date = new DateTime(2019,10,01,10,00,00), AvailableSeat=0},//1
                    new TrainingSession {Date = new DateTime(2019,10,01,8,00,00), AvailableSeat=5},
                    new TrainingSession {Date = new DateTime(2019,09,01,11,00,00), AvailableSeat=10},
                    new TrainingSession {Date = new DateTime(2019,10,29,10,00,00), AvailableSeat=5},
                    new TrainingSession {Date = new DateTime(2019,11,11,15,00,00), AvailableSeat=2},//5
                    new TrainingSession {Date = new DateTime(2019,10,30,16,00,00), AvailableSeat=0},
                    new TrainingSession {Date = new DateTime(2019,10,30,12,00,00), AvailableSeat=5},
                    new TrainingSession {Date = new DateTime(2019,10,12,10,00,00), AvailableSeat=3},
                    new TrainingSession {Date = new DateTime(2019,10,12,12,00,00), AvailableSeat=1},

                    new TrainingSession {Date = new DateTime(2018,12,10,10,00,00), AvailableSeat=3},//10
                    new TrainingSession {Date = new DateTime(2019,10,20,08,00,00), AvailableSeat=8},
                    new TrainingSession {Date = new DateTime(2019,08,15,10,00,00), AvailableSeat=0},
                    new TrainingSession {Date = new DateTime(2019,08,15,12,00,00), AvailableSeat=8},
                    new TrainingSession {Date = new DateTime(2020,03,01,15,00,00), AvailableSeat=15},
                    new TrainingSession {Date = new DateTime(2020,03,01,15,30,00), AvailableSeat=15},//15
                    new TrainingSession {Date = new DateTime(2019,12,10,15,00,00), AvailableSeat=15},
                    new TrainingSession {Date = new DateTime(2020,03,01,15,00,00), AvailableSeat=12},

                    new TrainingSession {Date = new DateTime(2020,03,01,15,00,00), AvailableSeat=0},
                    new TrainingSession {Date = new DateTime(2020,03,01,15,00,00), AvailableSeat=1},
                    new TrainingSession {Date = new DateTime(2020,03,01,15,00,00), AvailableSeat=2},
                    new TrainingSession {Date = new DateTime(2020,03,02,15,30,00), AvailableSeat=0},
                    new TrainingSession {Date = new DateTime(2020,03,02,15,30,00), AvailableSeat=1},
                    new TrainingSession {Date = new DateTime(2020,03,03,15,30,00), AvailableSeat=2},
                    new TrainingSession {Date = new DateTime(2020,03,03,16,00,00), AvailableSeat=0},
                    new TrainingSession {Date = new DateTime(2020,03,03,16,00,00), AvailableSeat=1},
                    new TrainingSession {Date = new DateTime(2020,03,07,16,00,00), AvailableSeat=2},
                    });

                    //Init User
                    db.InsertAll(new List<User>()
                    {
                    new User{Username="Romane", Password="admin"},
                    new User{Username="Quentin", Password="admin"},
                    new User{Username="Dylan", Password="admin"},
                    new User{Username="Tiphaine", Password="admin"}
                    });

                    //Init User Training Session
                    db.Insert(new UserTrainingSession() { TrainingSessionId = 1, UserId = 1 , Result= 3});
                    db.Insert(new UserTrainingSession() { TrainingSessionId = 5, UserId = 1 , Result= 5});
                    db.Insert(new UserTrainingSession() { TrainingSessionId = 10, UserId = 1, Result= 9 });
                    db.Insert(new UserTrainingSession() { TrainingSessionId = 4, UserId = 2, Result= 9 });
                    db.Insert(new UserTrainingSession() { TrainingSessionId = 13, UserId = 1, Result= 10 });
                    db.Insert(new UserTrainingSession() { TrainingSessionId = 1, UserId = 3, Result = 6 });
                    db.Insert(new UserTrainingSession() { TrainingSessionId = 16, UserId = 1, Result = 10 });

                    //Init Driving Lessons
                    db.Insert(new DrivingLesson() { Comment = "Premiere Lecon", Date = DateTime.Now.AddHours(1).AddMinutes(2), InstructorId = 1 });
                    db.Insert(new DrivingLesson() { Comment = "Seconde Lecon", Date = DateTime.Now.AddHours(1).AddMinutes(3), InstructorId = 2 });
                    db.Insert(new DrivingLesson() { Comment = "Troisième Lecon", Date = DateTime.Now.AddHours(1).AddMinutes(4), InstructorId = 1, UserId = 1 });
                    db.Insert(new DrivingLesson() { Comment = "Lecon conduite", Date = DateTime.Now.AddHours(1).AddMinutes(5), InstructorId = 2 });
                    db.Insert(new DrivingLesson() { Comment = "Quatrième Lecon", Date = DateTime.Now.AddHours(1).AddMinutes(5), InstructorId = 2, UserId = 1 });
                    db.Insert(new DrivingLesson() { Comment = "Cinquième Lecon", Date = DateTime.Now.AddHours(1).AddMinutes(6), InstructorId = 1, UserId = 2 });
                    db.Insert(new DrivingLesson() { Comment = "Sixième Lecon", Date = DateTime.Now.AddHours(1).AddMinutes(4), InstructorId = 3 });
                    db.Insert(new DrivingLesson() { Comment = "Septieme Lecon", Date = DateTime.Now.AddHours(1).AddMinutes(5), InstructorId = 1 });
                    db.Insert(new DrivingLesson() { Comment = "8eme Lecon", Date = DateTime.Now.AddHours(1).AddMinutes(6), InstructorId = 3 });
                    db.Insert(new DrivingLesson() { Comment = "9eme Lecon", Date = new DateTime(2018, 10, 09), InstructorId = 1, UserId = 1 });
                    db.Insert(new DrivingLesson() { Comment = "10eme Lecon", Date = DateTime.Now.AddDays(1).AddMinutes(2), InstructorId = 1 });
                    db.Insert(new DrivingLesson() { Comment = "11eme Lecon", Date = DateTime.Now.AddDays(1).AddMinutes(3), InstructorId = 1 });
                    db.Insert(new DrivingLesson() { Comment = "12eme Lecon", Date = new DateTime(2020, 10, 11), InstructorId = 1 });
                    db.Insert(new DrivingLesson() { Comment = "13eme Lecon", Date = new DateTime(2020, 10, 11), InstructorId = 1 });
                    db.Insert(new DrivingLesson() { Comment = "14eme Lecon", Date = new DateTime(2020, 10, 11), InstructorId = 1 });
                    db.Insert(new DrivingLesson() { Comment = "15eme Lecon", Date = new DateTime(2020, 10, 11), InstructorId = 1 });
                    db.Insert(new DrivingLesson() { Comment = "16eme Lecon", Date = new DateTime(2020, 10, 11), InstructorId = 1 });
                    db.Insert(new DrivingLesson() { Comment = "17eme Lecon", Date = new DateTime(2020, 10, 11), InstructorId = 1 });
                    db.Insert(new DrivingLesson() { Comment = "18eme Lecon", Date = new DateTime(2020, 10, 11), InstructorId = 1 });
                    db.Insert(new DrivingLesson() { Comment = "19eme Lecon", Date = new DateTime(2020, 10, 11), InstructorId = 1 });
                    db.Insert(new DrivingLesson() { Comment = "20eme Lecon", Date = new DateTime(2020, 10, 11), InstructorId = 1 });
                    db.Insert(new DrivingLesson() { Comment = "15eme Lecon", Date = new DateTime(2020, 10, 11), InstructorId = 1 });

                    //Init Instructor
                    db.Insert(new Instructor() { FirstName = "Premier", LastName = "Instructeur" });
                    db.Insert(new Instructor() { FirstName = "Second", LastName = "Instructeur" });
                    db.Insert(new Instructor() { FirstName = "Troisième", LastName = "Instructeur" });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
    }
}
