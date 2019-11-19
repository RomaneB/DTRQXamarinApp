using DTRQXamarinApp.Entities;
using DTRQXamarinApp.IRepository;
using DTRQXamarinApp.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTRQXamarinApp.Service
{
    public class DrivingLessonService : IService<DrivingLesson>
    {
        public IRepository<DrivingLesson> DrivingLessonRepository { get; set; }
        public IRepository<Instructor> InstructorRepository { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="drivingLessonRepository"></param>
        /// <param name="instructorRepository"></param>
        public DrivingLessonService(IRepository<DrivingLesson> drivingLessonRepository, IRepository<Instructor> instructorRepository)
        {
            DrivingLessonRepository = drivingLessonRepository;
            InstructorRepository = instructorRepository;
        }

        public int Add(DrivingLesson entity)
        {
            return DrivingLessonRepository.Add(entity);
        }

        public int Delete(int id)
        {
            return DrivingLessonRepository.Delete(id);
        }

        public IEnumerable<DrivingLesson> GetAll()
        {
            return DrivingLessonRepository.GetAll().OrderBy(s => s.Date);
        }

        public DrivingLesson GetByid(int id)
        {
            return DrivingLessonRepository.GetById(id);
        }

        public DrivingLesson Update(DrivingLesson entity)
        {
            return DrivingLessonRepository.Update(entity);
        }

        public DrivingLessonInstructor GetByIdWithInstructor(int id)
        {
            try
            {
                DrivingLesson drivingLesson = DrivingLessonRepository.GetById(id);
                Instructor instructor = InstructorRepository.GetById(drivingLesson.InstructorId);

                return new DrivingLessonInstructor
                {
                    Text = drivingLesson.Comment,
                    DateTime = drivingLesson.Date,
                    DrivingLessonId = drivingLesson.Id,
                    InstructorId = instructor.Id,
                    InstructorFirstName = instructor.FirstName,
                    InstructorLastName = instructor.LastName,
                    UserId = drivingLesson.UserId
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int UpdateUserIdForDrivingLesson(int id, int userId)
        {
            try
            {
                DrivingLesson drivingLesson = DrivingLessonRepository.GetById(id);
                drivingLesson.UserId = userId;
                DrivingLessonRepository.Update(drivingLesson);

                return drivingLesson.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
           
        }

        /// <summary>
        /// Query that retrieves the list of my lessons sorted by date
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dateFuture"></param>
        /// <returns></returns>
        public IEnumerable<DrivingLessonInstructor> GetMyDrivingLessonsByUserId(int userId, bool dateFuture)
        {
            try
            {
                //Get All Driving lessons
                IEnumerable<DrivingLesson> lstDriving = DrivingLessonRepository.GetAll();
                //Get All Instructor
                IEnumerable<Instructor> lstInstructor = InstructorRepository.GetAll();

                // My future lessons
                if (dateFuture)
                {
                    return lstDriving.Join(lstInstructor, d => d.InstructorId, t => t.Id, (d, t) =>
                    new DrivingLessonInstructor
                    {
                        Text = d.Comment,
                        DateTime = d.Date,
                        DrivingLessonId = d.Id,
                        InstructorId = t.Id,
                        InstructorFirstName = t.FirstName,
                        InstructorLastName = t.LastName,
                        UserId = d.UserId
                    }).Where(w => w.UserId == userId && w.DateTime > DateTime.Now).OrderBy(s => s.DateTime);
                }
                // My passed lessons
                else
                {
                    return lstDriving.Join(lstInstructor, d => d.InstructorId, t => t.Id, (d, t) =>
                    new DrivingLessonInstructor
                    {
                        Text = d.Comment,
                        DateTime = d.Date,
                        DrivingLessonId = d.Id,
                        InstructorId = t.Id,
                        InstructorFirstName = t.FirstName,
                        InstructorLastName = t.LastName,
                        UserId = d.UserId
                    }).Where(w => w.UserId == userId && w.DateTime <= DateTime.Now).OrderBy(s => s.DateTime);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }           
        }

        // Method to retrieve the list of all available lessons that do not start at the same time as one of the user's lessons.
        public IEnumerable<DrivingLessonInstructor> GetDrivingLessonsByUserId(int userId)
        {
            try
            {
                //Get All Driving lessons
                IEnumerable<DrivingLesson> lstDriving = DrivingLessonRepository.GetAll();
                //Get All Instructor
                IEnumerable<Instructor> lstInstructor = InstructorRepository.GetAll();

                IEnumerable<DrivingLessonInstructor> drivingLessonInstructorsList = lstDriving.Join(lstInstructor, d => d.InstructorId, t => t.Id, (d, t) =>
                new DrivingLessonInstructor
                {
                    Text = d.Comment,
                    DateTime = d.Date,
                    DrivingLessonId = d.Id,
                    InstructorId = t.Id,
                    InstructorFirstName = t.FirstName,
                    InstructorLastName = t.LastName,
                    UserId = d.UserId
                }).Where(w => w.UserId == 0 && w.DateTime > DateTime.Now).OrderBy(s => s.DateTime);

                IEnumerable<string> lstMyDrivingLessons = lstDriving.Where(w => w.UserId == userId && w.Date > DateTime.Now).OrderBy(s => s.Date).Select(s => s.Date.ToLongTimeString()).ToList();

                return drivingLessonInstructorsList.Where(x => !lstMyDrivingLessons.Contains(x.DateTime.ToLongTimeString()));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
