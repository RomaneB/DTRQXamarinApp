using DTRQXamarinApp.Entities;
using DTRQXamarinApp.Service;
using Prism.Commands;
using Prism.Events;
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

        public IEventAggregator Event { get; set; }

        public MyTrainingsPageViewModel(INavigationService navigationService, TrainingSessionService trainingSessionService, IEventAggregator eventAggregator)
           : base(navigationService, trainingSessionService)
        {
            Title = "Mes sessions";
            Event = eventAggregator;
            Event.GetEvent<SentEvent>().Subscribe(IdReceived);
            InitializeItems();
        }

        private void IdReceived(int id)
        {
            Items.Add(TrainingSessionService.GetByid(id));
        }

        private void InitializeItems()
        {
            LstTrainingSession = TrainingSessionService.GetAllByUserId(1);
            Items = new ObservableCollection<TrainingSession>(LstTrainingSession);
        }
    }
}
