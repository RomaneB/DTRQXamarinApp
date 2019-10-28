using DTRQXamarinApp.Entities;
using DTRQXamarinApp.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DTRQXamarinApp.ViewModels.TrainingSessions
{
    public class MyTrainingsPageViewModel : ViewModelBase
    {
        public ObservableCollection<TrainingSession> Items { get; set; }
        public IEnumerable<TrainingSession> LstTrainingSession { get; set; }

        public MyTrainingsPageViewModel(INavigationService navigationService, TrainingSessionService trainingSessionService)
           : base(navigationService, trainingSessionService)
        {
            Title = "Mes sessions";
            InitializeItems();
        }

        private void InitializeItems()
        {
            LstTrainingSession = TrainingSessionService.GetAllByUserId(1);
            Items = new ObservableCollection<TrainingSession>(LstTrainingSession);
        }
    }
}
