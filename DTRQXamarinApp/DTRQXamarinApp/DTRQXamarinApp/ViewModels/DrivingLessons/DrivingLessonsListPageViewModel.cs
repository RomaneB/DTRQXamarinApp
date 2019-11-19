using DTRQXamarinApp.Entities;
using DTRQXamarinApp.Event;
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

namespace DTRQXamarinApp.ViewModels.DrivingLessons
{
    public class DrivingLessonsListPageViewModel : ViewModelBase
    {
        public IEnumerable<DrivingLessonInstructor> ListeDrivingLessons { get; set; }
        public ObservableCollection<DrivingLessonInstructor> Items { get; set; }
        public DelegateCommand<DrivingLessonInstructor> InscriptionLessonCommand { get; set; }
        public IEventAggregator _ea;
        /// <summary>
        /// Constructor of the viewModel
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="ea"></param>
        /// <param name="drivingLessonService"></param>
        public DrivingLessonsListPageViewModel(INavigationService navigationService, IEventAggregator ea, DrivingLessonService drivingLessonService)
           : base(navigationService, drivingLessonService)
        {
            Title = "Leçons disponibles";
            InitializeItems();
            _ea = ea;
            _ea.GetEvent<DrivingSentEventUnregister>().Subscribe(MessageReceived);
            InscriptionLessonCommand = new DelegateCommand<DrivingLessonInstructor>(InscriptionLesson);
        }

        /// <summary>
        /// Method to retrieve changes made on the page containing my lessons
        /// This method adds the new element to the table and sorts it according to the dates of the lessons
        /// </summary>
        /// <param name="id"></param>
        private void MessageReceived(int id)
        {
            Items.Add(DrivingLessonService.GetByIdWithInstructor(id));
            List<DrivingLessonInstructor> liste = Items.OrderBy(s => s.DateTime).ToList();
            Items.Clear();

            foreach (DrivingLessonInstructor item in liste)
            {
                Items.Add(item);
            }
        }

        /// <summary>
        /// Retrieves the selected driving instructor and modifies the userId of it
        /// </summary>
        /// <param name="DrivingLessonInstructor"></param>
        protected void InscriptionLesson(DrivingLessonInstructor DrivingLessonInstructor)
        {
            ShowExitDialog(DrivingLessonInstructor);
        }

        /// <summary>
        /// Popup to validate or not a registration to a driving court
        /// </summary>
        /// <param name="DrivingLessonInstructor"></param>
        private async void ShowExitDialog(DrivingLessonInstructor DrivingLessonInstructor)
        {
            int id = DrivingLessonInstructor.DrivingLessonId;
            var answer = await Application.Current.MainPage.DisplayAlert("Confirmer inscription", "Voulez-vous vous inscrire à la leçon du : " + DrivingLessonInstructor.DateTime + " qui se déroulera avec " + DrivingLessonInstructor.InstructorFirstName + " " + DrivingLessonInstructor.InstructorLastName, "Oui", "Non");

            // If the user responds positively to the popup
            if (answer)
            {
                int drivingLessonId = DrivingLessonService.UpdateUserIdForDrivingLesson(id, 1);

                ListeDrivingLessons = DrivingLessonService.GetDrivingLessonsByUserId(int.Parse(Application.Current.Properties["UserId"].ToString()));

                if (ListeDrivingLessons != null)
                {
                    //The lesson is removed from the table containing all available lessons
                    Items.Remove(DrivingLessonInstructor);
                }

                if (drivingLessonId != 0)
                {
                    // Notification one hour before class
                    var notification = new NotificationRequest
                    {
                        NotificationId = int.Parse(DrivingLessonInstructor.DrivingLessonId.ToString() + "1"),
                        Title = "Seance de conduite",
                        Description = "Attention, vous avez une leçon de conduite à " + DrivingLessonInstructor.DateTime.Hour + "h et " + DrivingLessonInstructor.DateTime.Minute + "m.",
                        ReturningData = "Dummy data", // Returning data when tapped on notification.
                        NotifyTime = DrivingLessonInstructor.DateTime.AddHours(-1)
                    };
                    NotificationCenter.Current.Show(notification);

                    DateTime dateLecon = new DateTime(DrivingLessonInstructor.DateTime.Year, DrivingLessonInstructor.DateTime.Month, DrivingLessonInstructor.DateTime.Day);
                    DateTime dateJour = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                    // If the lesson goes well in at least a day
                    if (dateLecon > dateJour)
                    {
                        // Notification one day before class
                        notification = new NotificationRequest
                        {
                            NotificationId = int.Parse(DrivingLessonInstructor.DrivingLessonId.ToString() + "3"),
                            Title = "Seance de conduite",
                            Description = "Attention, vous avez une leçon de conduite demain à " + DrivingLessonInstructor.DateTime.Hour + "h et " + DrivingLessonInstructor.DateTime.Minute + "m.",
                            ReturningData = "Dummy data", // Returning data when tapped on notification.
                            NotifyTime = DrivingLessonInstructor.DateTime.AddDays(-1)
                        };
                        NotificationCenter.Current.Show(notification);
                    }

                    // Registration information for this lesson is sent to the page containing all my lessons
                    _ea.GetEvent<DrivingSentEvent>().Publish(drivingLessonId);
                    await Application.Current.MainPage.DisplayAlert("Validation", "Vous êtes désormais inscrit à la session du : " + DrivingLessonInstructor.DateTime, "Ok");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", "Une erreur est survenue pendant l'inscription à la séssion. Veuillez réésayer. Si le problème persiste, veuillez contacter l'auto école.", "Ok");
                }
            }
        }

        // Initializing the data in the table
        private void InitializeItems()
        {
            ListeDrivingLessons = DrivingLessonService.GetDrivingLessonsByUserId(int.Parse(Application.Current.Properties["UserId"].ToString()));
            Items = new ObservableCollection<DrivingLessonInstructor>(ListeDrivingLessons);
        }
    }
}
