using DTRQXamarinApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTRQXamarinApp.ViewModels.TrainingSessions
{
    public class PictureTrainingSessionViewModel : TrainingSession
    {
        public string PictureTraining { get; set; }

        public PictureTrainingSessionViewModel(TrainingSession trainingSession, string pictureTraining) : this(trainingSession.Date ,trainingSession.AvailableSeat, trainingSession.Id)
        {
            PictureTraining = pictureTraining;

        }

        public PictureTrainingSessionViewModel(DateTime date, int availableSeat, int id)
        {

        }
    }
}
