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
    public class TrainingSessionsResultsPageViewModel : ViewModelBase
    {

        public ObservableCollection<ResultTrainingSessionViewModel> Items { get; set; }
        public IEnumerable<TrainingSession> LstTrainingSession { get; set; }

        public TrainingSessionsResultsPageViewModel(INavigationService navigationService, TrainingSessionService trainingSessionService)
            : base(navigationService, trainingSessionService)
        {
            Title = "Mes résultats";
            InitializeItems();
        }

        private void InitializeItems()
        {
            IEnumerable<ResultTrainingSessionViewModel> LstTrainingSession = TrainingSessionService.GetResultsByUserId(1);
            Items = new ObservableCollection<ResultTrainingSessionViewModel>(LstTrainingSession);
        }
    }
}
