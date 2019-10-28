using DTRQXamarinApp.Entities;
using DTRQXamarinApp.IService;
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
    public class TrainingSessionsListPageViewModel : ViewModelBase
    {
        public ObservableCollection<TrainingSession> Items { get; set; }
        public IEnumerable<TrainingSession> LstTrainingSession { get; set; }

        public TrainingSessionsListPageViewModel(TrainingSessionService trainingSessionService, INavigationService navigationService)
           : base(navigationService, trainingSessionService)
        {
            Title = "Sessions disponibles";
            InitializeItems();
        }

        private void InitializeItems()
        {
            LstTrainingSession = TrainingSessionService.GetAll();
            Items = new ObservableCollection<TrainingSession>(LstTrainingSession);
        }
    }
}
