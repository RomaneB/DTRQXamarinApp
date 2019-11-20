using DTRQXamarinApp.Entities;
using DTRQXamarinApp.Event;
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
        public string SwitchText { get; set; }
        public bool SwitchToggled { 
            get {
                return SwitchToggled; }
            set {
                if (true)
                {

                }
            } 
        }
        public ObservableCollection<PictureTrainingSessionViewModel> Items { get; set; }       

        public DelegateCommand<PictureTrainingSessionViewModel> Register { get; set; }


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
            Title = "Futures sessions";
            SwitchText = "Toutes les sessions";
            //SwitchToggled = true;
            Register = new DelegateCommand<PictureTrainingSessionViewModel>(SaveRegister);
            Event = eventAggregator;
            Event.GetEvent<SentEventUnregister>().Subscribe(IdReceived);
            InitializeItems();
        }

        private void IdReceived(int obj)
        {
            TrainingSession t = TrainingSessionService.GetByid(obj);

            // retrieve the good picture in regards to the available seats
            var pictureTraining = "";
            if (t.AvailableSeat == 0)
            {
                pictureTraining = "unavailable.png";
            }
            else if (t.AvailableSeat < 3)
            {
                pictureTraining = "warning.png";
            }

            PictureTrainingSessionViewModel pictureTrainingSessionViewModels = new PictureTrainingSessionViewModel(t, pictureTraining) { AvailableSeat = t.AvailableSeat , Date = t.Date , Id = t.Id, PictureTraining = pictureTraining };
            
            
            Items.Add(pictureTrainingSessionViewModels);
            List<PictureTrainingSessionViewModel> trainingSessions = Items.OrderBy(s => s.Date).ToList();
            Items.Clear();

            foreach (PictureTrainingSessionViewModel item in trainingSessions)
            {
                Items.Add(item);
            }
        }

        /// <summary>
        /// Register a user to a training Session and publish it to add the training to user's trainings list
        /// </summary>
        /// <param name="obj">The training on which the user would like to register </param>
        private async void SaveRegister(PictureTrainingSessionViewModel obj)
        {
            try
            {
                bool answer = await Application.Current.MainPage.DisplayAlert("Confirmation d'inscription", "Êtes-vous sûr de vouloir vous inscrire à la session du : " + obj.Date, "Oui", "Non");

                if (answer)
                {
                    int userId = int.Parse(Application.Current.Properties["UserId"].ToString());

                    //Register the user to the training Session in parameter
                    int add = TrainingSessionService.SaveRegister(userId, obj.Id);
                    
                    if (add != 0)
                    {
                        TrainingSession t = new TrainingSession()
                        {
                            Id = obj.Id,
                            AvailableSeat = obj.AvailableSeat,
                            Date = obj.Date
                        };
                        //Update the number of available seats
                        t.AvailableSeat -= 1;
                        TrainingSessionService.Update(t);

                        //Remove the session into the available sessions for the user.
                        ObservableCollection<TrainingSession> trainingSessions = new ObservableCollection<TrainingSession>(TrainingSessionService.GetAllByUserId(userId));

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
                throw new Exception(ex.Message);
            }
        }        

        private void InitializeItems()
        {
            IEnumerable<PictureTrainingSessionViewModel> LstTrainingSession = TrainingSessionService.GetAllAvailable(int.Parse(Application.Current.Properties["UserId"].ToString())).OrderBy(s=> s.Date);
            Items = new ObservableCollection<PictureTrainingSessionViewModel>(LstTrainingSession);
        }
    }
}
