﻿using DTRQXamarinApp.Entities;
using DTRQXamarinApp.IRepository;
using DTRQXamarinApp.IService;
using DTRQXamarinApp.ViewModels.TrainingSessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTRQXamarinApp.Service
{
    public class TrainingSessionService : IService<TrainingSession>
    {
        public IRepository<TrainingSession> TrainingSessionRepository { get; set; }
        public IRepository<UserTrainingSession> UserTrainingSessionRepository { get; set; }

        public TrainingSessionService(IRepository<TrainingSession> trainingSessionRepository, IRepository<UserTrainingSession> userTrainingSessionRepository)
        {
            TrainingSessionRepository = trainingSessionRepository;
            UserTrainingSessionRepository = userTrainingSessionRepository;
        }

        /// <summary>
        /// Add a training Session
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Add(TrainingSession entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete a training session
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all trainings sessions
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TrainingSession> GetAll()
        {
            return TrainingSessionRepository.GetAll();
        }

        /// <summary>
        /// Get a training session
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TrainingSession GetByid(int id)
        {
            return TrainingSessionRepository.GetById(id);
        }

        /// <summary>
        /// Update a training session
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TrainingSession Update(TrainingSession entity)
        {
            return TrainingSessionRepository.Update(entity);
        }

        /// <summary>
        /// Get all training Sessions on which a user is registered
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<TrainingSession> GetAllByUserId(int userId)
        {
            try
            {
                //Get All User Training
                IEnumerable< UserTrainingSession> lstUT = UserTrainingSessionRepository.GetAll();
                //Get All Training
                IEnumerable<TrainingSession> lstT = TrainingSessionRepository.GetAll();
                //return gettAllbyuserid
                List<int> ids = lstUT.Where(t => t.UserId == userId).Select(t => t.TrainingSessionId).ToList();
                return lstT.Where(t => ids.Contains(t.Id)).Where(t=> t.Date >= DateTime.Now).OrderByDescending(s => s.Date).ToList();    
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Get all results of the training Session of a user (session passed)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<ResultTrainingSessionViewModel> GetResultsByUserId(int userId)
        {
            try
            {
                //Get All User Training
                IEnumerable<UserTrainingSession> lstUT = UserTrainingSessionRepository.GetAll().Where(ut => ut.UserId == userId);

                List<ResultTrainingSessionViewModel> results = new List<ResultTrainingSessionViewModel>();
                foreach (var item in lstUT)
                {
                    TrainingSession t = TrainingSessionRepository.GetById(item.TrainingSessionId);
                    if (t.Date < DateTime.Now)
                    {
                        results.Add(new ResultTrainingSessionViewModel(t, item.Result) {AvailableSeat = t.AvailableSeat, Date = t.Date, Id = t.Id, Result= item.Result });
                    }
                }
                return results.OrderByDescending(t=> t.Date);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        /// <summary>
        /// Get All available training Sessions for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<PictureTrainingSessionViewModel> GetAllAvailable(int userId)
        {
            try
            {
                var lstAllByUsers = this.GetAllByUserId(userId);

                //Get all ids of the sessions
                IEnumerable<int> lstAllTrainingSessionsIds = TrainingSessionRepository.GetAll()
                    .Where(t => t.Date > DateTime.Now)
                    .Select(t => t.Id);

                //Get all ids and dates of the sessions on which the user is registered
                IEnumerable<int> lstAllTrainingSessionsIdsByUser = lstAllByUsers
                    .Select(t => t.Id);

                IEnumerable<DateTime> lstAllTrainingSessionsDatesByUser = lstAllByUsers
                    .Select(t => t.Date);

                //Get all ids of the available sessions for the user
                IEnumerable<int> ids = lstAllTrainingSessionsIds.Except(lstAllTrainingSessionsIdsByUser);

                //Returns sessions available for the user
                var lstAvailableSession = TrainingSessionRepository.GetAll()
                    .Where(t => ids.Any(i => i == t.Id))
                    .Where(t => !lstAllTrainingSessionsDatesByUser.Any(d => d == t.Date));

                List<PictureTrainingSessionViewModel> results = new List<PictureTrainingSessionViewModel>();

                //Foreach availableSession add the coorect picture
                foreach (var item in lstAvailableSession)
                {
                    // retrieve the trainingSession
                    TrainingSession t = TrainingSessionRepository.GetById(item.Id);

                    var pictureTraining = "";

                    if (item.AvailableSeat == 0)
                    {
                        pictureTraining  = "unavailable.png";
                    }
                    else if (item.AvailableSeat < 3)
                    {
                        pictureTraining = "warning.png";
                    }

                    results.Add(new PictureTrainingSessionViewModel(t, pictureTraining)
                    {
                        Id = t.Id,
                        AvailableSeat = t.AvailableSeat,
                        Date = t.Date,
                        PictureTraining = pictureTraining
                    });

                }
                return results.OrderByDescending(r => r.Date);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Register a user on a training Session
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="trainingSessionId"></param>
        /// <returns></returns>
        public int SaveRegister(int userId, int trainingSessionId)
        {
            try
            {
                return UserTrainingSessionRepository.Add(new UserTrainingSession()
                {
                    UserId = userId,
                    TrainingSessionId = trainingSessionId
                });
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            
        }

        /// <summary>
        /// Unregister a user on a training Session
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="trainingSessionId"></param>
        /// <returns></returns>
        public int SaveUnregister(int userId, int trainingSessionId)
        {
            try
            {
                var lst = UserTrainingSessionRepository.GetAll().Where(ut => ut.TrainingSessionId == trainingSessionId && ut.UserId == userId);

                var id = lst.Select(ut => ut.Id).FirstOrDefault();
                return UserTrainingSessionRepository.Delete(id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
    }
}
