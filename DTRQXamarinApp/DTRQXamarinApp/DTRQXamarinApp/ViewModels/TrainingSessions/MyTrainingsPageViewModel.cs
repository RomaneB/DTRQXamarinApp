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
using Xamarin.Forms;

namespace DTRQXamarinApp.ViewModels.TrainingSessions
{
    public class MyTrainingsPageViewModel : ViewModelBase
    {
        public ObservableCollection<TrainingSession> Items { get; set; }
        public IEnumerable<TrainingSession> LstTrainingSession { get; set; }

        public DelegateCommand<TrainingSession> Unregister { get; set; }

        public IEventAggregator Event { get; set; }

        public MyTrainingsPageViewModel(INavigationService navigationService, TrainingSessionService trainingSessionService, IEventAggregator eventAggregator)
           : base(navigationService, trainingSessionService)
        {
            Title = "Mes sessions";
            Unregister = new DelegateCommand<TrainingSession>(SaveUnregister);
            Event = eventAggregator;
            Event.GetEvent<SentEvent>().Subscribe(IdReceived);
            InitializeItems();
        }

        private void IdReceived(int id)
        {
            Items.Add(TrainingSessionService.GetByid(id));
        }

        private async void SaveUnregister(TrainingSession obj)
        {
            try
            {
                bool answer = await Application.Current.MainPage.DisplayAlert("Confirmation d'annulation", "Êtes-vous sûr de vouloir vous désinscrire à la session du : " + obj.Date, "Oui", "Non");

                if (answer)
                {
                    //TODO Get UserId
                    int userId = 1;

                    //Register the user to the training Session in parameter
                    int delete = TrainingSessionService.SaveUnregister(userId, obj.Id);


                    if (delete != 0)
                    {
                        obj.AvailableSeat += 1;
                        TrainingSessionService.Update(obj);

                        ObservableCollection<TrainingSession> trainingSessions = new ObservableCollection<TrainingSession>(TrainingSessionService.GetAllByUserId(userId));

                        if (trainingSessions != null)
                        {
                            Items.Remove(obj);
                        }

                        //Event publish to refresh the user's trainings list
                        Event.GetEvent<SentEventUnregister>().Publish(obj.Id);

                        await Application.Current.MainPage.DisplayAlert("Confirmation", "Vous êtes désinscrit à la session du : " + obj.Date, "Ok");
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
            LstTrainingSession = TrainingSessionService.GetAllByUserId(1);
            Items = new ObservableCollection<TrainingSession>(LstTrainingSession);
        }
    }
}
