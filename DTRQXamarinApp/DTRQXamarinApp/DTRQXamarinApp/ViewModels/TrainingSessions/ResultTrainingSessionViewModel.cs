using DTRQXamarinApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTRQXamarinApp.ViewModels.TrainingSessions
{
    public class ResultTrainingSessionViewModel : TrainingSession
    {
        public int Result { get; set; }

        public ResultTrainingSessionViewModel(TrainingSession trainingSession, int result) : this(trainingSession.Date,trainingSession.AvailableSeat, trainingSession.Id)
        {
            Result = result;
        }

        public ResultTrainingSessionViewModel(DateTime date, int availableSeat, int id)
        {

        }
    }
}
