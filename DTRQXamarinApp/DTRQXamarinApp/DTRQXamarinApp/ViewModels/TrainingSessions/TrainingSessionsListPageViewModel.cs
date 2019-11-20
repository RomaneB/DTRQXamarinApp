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
        private readonly string _switchTextAvailableTrainingSessions = "Toutes les sessions";
        private readonly string _switchTextTrainingSessionsWithAvailableSeats = "Sessions avec place(s) disponible(s)";
        private string _switchText = string.Empty;

        /// <summary>
        /// Get or set the text displayed above the switch component.
        /// </summary>
        public string SwitchText
        {
            get { return _switchText; }
            set { SetProperty(ref _switchText, value); }
        }

        /// <summary>
        /// Get or set the value returned by the switch component.
        /// </summary>
        public bool SwitchValue { get; set; }

        /// <summary>
        /// Get or set the command executed when the user toggle the switch.
        /// </summary>
        public DelegateCommand SwitchToggle { get; set; }

        /// <summary>
        /// Get or set the command executed when the user tries to register a <see cref="TrainingSession"/>.
        /// </summary>
        public DelegateCommand<PictureTrainingSessionViewModel> Register { get; set; }

        /// <summary>
        /// Get or set the collection of <see cref="TrainingSession"/> displayed in this tab.
        /// </summary>
        public ObservableCollection<PictureTrainingSessionViewModel> Items { get; set; }        

        /// <summary>
        /// Get or set an object which allow to manage events (subscribe/publish).
        /// </summary>
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
            SwitchToggle = new DelegateCommand(SwitchToggled);
            Register = new DelegateCommand<PictureTrainingSessionViewModel>(SaveRegister);
            // If we disable it, it will initialize the list with default values.
            this.DisableSwitch(); 
            Event = eventAggregator;
            Event.GetEvent<SentEventUnregister>().Subscribe(IdReceived);
            Event.GetEvent<RefreshAvailableTrainingSessionsListEvent>().Subscribe(RefreshAvailableTrainingSessionsList);
        }

        /// <summary>
        /// Refresh the available training sessions list when <see cref="RefreshAvailableTrainingSessionsListEvent"/> is triggered.
        /// </summary>
        private void RefreshAvailableTrainingSessionsList()
        {
            this.SwitchToggle.Execute();
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
                // TODO The user is able to register even if there is 0 available seat...
                if (obj.AvailableSeat <= 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Aucune place disponible pour cette session.", "", "OK");
                    return;
                }

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
                        Event.GetEvent<RefreshAvailableTrainingSessionsListEvent>().Publish();

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

        /// <summary>
        /// Command triggered when the user enable or disable the switch component.
        /// The training sessions list will be updated if the switch component is enabled, it
        /// will display the next training sessions with available seats in that case.
        /// </summary>
        private async void SwitchToggled()
        {
            try
            {
                if (this.SwitchValue)
                {
                    this.EnableSwitch();
                }
                else
                {
                    this.DisableSwitch();
                }
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", "Une erreur est survenue lors de l'application du filtre.", "OK");
            }
        }

        /// <summary>
        /// Get the next available training sessions ordered by date.
        /// </summary>
        /// <returns>A collection of <see cref="TrainingSession"/>.</returns>
        private IEnumerable<PictureTrainingSessionViewModel> GetAvailableTrainingSessions()
        {
            return TrainingSessionService
                .GetAllAvailable(int.Parse(Application.Current.Properties["UserId"].ToString()))
                .OrderBy(s => s.Date);
        }

        /// <summary>
        /// Get the next available training sessions ordered by date with available places only.
        /// </summary>
        /// <returns>A collection of <see cref="TrainingSession"/>.</returns>
        private IEnumerable<PictureTrainingSessionViewModel> GetTrainingSessionsWithAvailableSeats()
        {
            return this.GetAvailableTrainingSessions()
                .Where(t => t.AvailableSeat > 0);
        }

        /// <summary>
        /// Disable the switch component and update the training sessions list.
        /// </summary>
        private void DisableSwitch()
        {
            this.SwitchText = this._switchTextAvailableTrainingSessions;
            this.SwitchValue = false;

            if (this.Items == null)
            {
                this.Items = new ObservableCollection<PictureTrainingSessionViewModel>(this.GetAvailableTrainingSessions());
            }
            else
            {
                this.Items.Clear();
                this.GetAvailableTrainingSessions().ToList().ForEach(i => this.Items.Add(i));
            }
        }

        /// <summary>
        /// Enable the switch component and update the training sessions list.
        /// </summary>
        private void EnableSwitch()
        {
            SwitchText = this._switchTextTrainingSessionsWithAvailableSeats;
            this.SwitchValue = true;

            if (this.Items == null)
            {
                this.Items = new ObservableCollection<PictureTrainingSessionViewModel>(this.GetTrainingSessionsWithAvailableSeats());
            }
            else
            {
                this.Items.Clear();
                this.GetTrainingSessionsWithAvailableSeats().ToList().ForEach(i => this.Items.Add(i));
            }
        }
    }
}
