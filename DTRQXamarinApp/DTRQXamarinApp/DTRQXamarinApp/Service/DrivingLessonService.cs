﻿using DTRQXamarinApp.Entities;
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

        public int UpdateUserIdForDrivingLesson(int id, int userId)
        {
            DrivingLesson drivingLesson = DrivingLessonRepository.GetById(id);
            drivingLesson.UserId = userId;
            DrivingLessonRepository.Update(drivingLesson);

            return drivingLesson.Id;
        }

        public IEnumerable<DrivingLessonInstructor> GetDrivingLessonsByUserId(int userId, bool dateFuture)
        {
            //Get All Driving lessons
            IEnumerable<DrivingLesson> lstDriving = DrivingLessonRepository.GetAll();
            //Get All Instructor
            IEnumerable<Instructor> lstInstructor = InstructorRepository.GetAll();

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

    }
}
