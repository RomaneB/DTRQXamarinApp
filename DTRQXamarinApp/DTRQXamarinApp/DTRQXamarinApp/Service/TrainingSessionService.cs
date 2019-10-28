using DTRQXamarinApp.Entities;
using DTRQXamarinApp.IRepository;
using DTRQXamarinApp.IService;
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

        public int Add(TrainingSession entity)
        {
            throw new NotImplementedException();
        }

        public int ClearTable()
        {
            throw new NotImplementedException();
        }

        public int Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrainingSession> GetAll()
        {
            return TrainingSessionRepository.GetAll();
        }

        public TrainingSession GetByid(int id)
        {
            throw new NotImplementedException();
        }

        public int Update(TrainingSession entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrainingSession> GetAllByUserId(int userId)
        {
            //Get All User Training
            IEnumerable< UserTrainingSession> lstUT = UserTrainingSessionRepository.GetAll();
            //Get All Training
            IEnumerable<TrainingSession> lstT = TrainingSessionRepository.GetAll();
            //return gettAllbyuserid
            List<int> ids = lstUT.Where(t => t.UserId == userId).Select(t => t.TrainingSessionId).ToList();
            return lstT.Where(t => ids.Contains(t.Id)).Where(t=> t.Date >= DateTime.Now).OrderByDescending(s => s.Date).ToList();
        }

        public IEnumerable<TrainingSession> GetResultsByUserId(int userId)
        {
            //Get All User Training
            IEnumerable<UserTrainingSession> lstUT = UserTrainingSessionRepository.GetAll();
            //Get All Training
            IEnumerable<TrainingSession> lstT = TrainingSessionRepository.GetAll();
            //return gettAllbyuserid
            List<int> ids = lstUT.Where(t => t.UserId == userId).Select(t => t.TrainingSessionId).ToList();
            
            return lstT.Where(t => ids.Contains(t.Id)).Where(t=> t.Date < DateTime.Now).OrderByDescending(s => s.Date).ToList();
        }
    }
}
