using DTRQXamarinApp.Entities;
using DTRQXamarinApp.IService;
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
    public class TrainingSessionsListPageViewModel : ViewModelBase
    {
        public ObservableCollection<TrainingSession> Items { get; set; }
        public IEnumerable<TrainingSession> LstTrainingSession { get; set; }

        public DelegateCommand<TrainingSession> Register { get; set; }

        public IEventAggregator Event { get; set; }


        public TrainingSessionsListPageViewModel(TrainingSessionService trainingSessionService, INavigationService navigationService, IEventAggregator eventAggregator )
           : base(navigationService, trainingSessionService)
        {
            Title = "Sessions disponibles";
            Register = new DelegateCommand<TrainingSession>(SaveRegister);
            Event = eventAggregator;
            InitializeItems();
        }


        private void SaveRegister(TrainingSession obj)
        {
            try
            {
                //TODO Get UserId
                int userId = 1;
                int add = TrainingSessionService.SaveRegister(userId, obj.Id);
                Event.GetEvent<SentEvent>().Publish(obj.Id);

                if (add != 0)
                {
                    ObservableCollection<TrainingSession> trainingSessions = new ObservableCollection<TrainingSession>(TrainingSessionService.GetAllByUserId(userId));

                    if (trainingSessions != null)
                    {
                        Items = trainingSessions;
                    }
                }
                else
                {
                    //Message d'erreur;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }     

        private void InitializeItems()
        {
            LstTrainingSession = TrainingSessionService.GetAll();
            Items = new ObservableCollection<TrainingSession>(LstTrainingSession);
        }
    }
}
