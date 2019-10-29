using DTRQXamarinApp.Entities;
using DTRQXamarinApp.IService;
using DTRQXamarinApp.Service;
using Plugin.LocalNotification;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace DTRQXamarinApp.ViewModels.TrainingSessions
{
    public class TrainingSessionsListPageViewModel : ViewModelBase
    {
        public ObservableCollection<TrainingSession> Items { get; set; }
        public IEnumerable<TrainingSession> LstTrainingSession { get; set; }

        public DelegateCommand<TrainingSession> Register { get; set; }

        public IEventAggregator Event { get; set; }

        /// <summary>
        /// Constructor of the viewModel
        /// </summary>
        /// <param name="trainingSessionService"></param>
        /// <param name="navigationService"></param>
        /// <param name="eventAggregator"></param>
        public TrainingSessionsListPageViewModel(TrainingSessionService trainingSessionService, INavigationService navigationService, IEventAggregator eventAggregator )
           : base(navigationService, trainingSessionService)
        {
            Title = "Sessions disponibles";
            Register = new DelegateCommand<TrainingSession>(SaveRegister);
            Event = eventAggregator;
            Event.GetEvent<SentEventUnregister>().Subscribe(IdReceived);
            InitializeItems();
        }

        private void IdReceived(int obj)
        {
            Items.Add(TrainingSessionService.GetByid(obj));
        }

        /// <summary>
        /// Register a user to a training Session and publish it to add the training to user's trainings list
        /// </summary>
        /// <param name="obj">The training on which the user would like to register </param>
        private async void SaveRegister(TrainingSession obj)
        {
            try
            {
                bool answer = await Application.Current.MainPage.DisplayAlert("Confirmation d'inscription", "Êtes-vous sûr de vouloir vous inscrire à la session du : " + obj.Date, "Oui", "Non");

                if (answer)
                {
                    //TODO Get UserId
                    int userId = 1;

                    //Register the user to the training Session in parameter
                    int add = TrainingSessionService.SaveRegister(userId, obj.Id);


                    if (add != 0)
                    {
                        obj.AvailableSeat -= 1;
                        TrainingSessionService.Update(obj);

                        ObservableCollection<TrainingSession> trainingSessions = new ObservableCollection<TrainingSession>(TrainingSessionService.GetAllAvailable(userId));

                        if (trainingSessions != null)
                        {
                            Items.Remove(obj);
                        }

                        //Event publish to refresh the user's trainings list
                        Event.GetEvent<SentEvent>().Publish(obj.Id);

                        await Application.Current.MainPage.DisplayAlert("Validation", "Vous êtes désormais inscrit à la session du : " + obj.Date, "Ok");

                    }
                    else
                    {
                        //Message d'erreur;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }     

        private void InitializeItems()
        {
            //TODO Get UserId
            LstTrainingSession = TrainingSessionService.GetAllAvailable(1);
            Items = new ObservableCollection<TrainingSession>(LstTrainingSession);
        }
    }
}
