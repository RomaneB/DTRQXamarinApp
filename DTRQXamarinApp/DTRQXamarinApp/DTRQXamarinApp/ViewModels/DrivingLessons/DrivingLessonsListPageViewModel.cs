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
        public DrivingLessonsListPageViewModel(INavigationService navigationService, IEventAggregator ea, DrivingLessonService drivingLessonService)
           : base(navigationService, drivingLessonService)
        {
            Title = "Leçons disponibles";
            InitializeItems();
            _ea = ea;
            _ea.GetEvent<DrivingSentEventUnregister>().Subscribe(MessageReceived);
            InscriptionLessonCommand = new DelegateCommand<DrivingLessonInstructor>(InscriptionLesson);
        }

        private void MessageReceived(int id)
        {
            Items.Add(DrivingLessonService.GetByIdWithInstructor(id));
        }

        /// <summary>
        /// Permet de récupéré le driving instructor sélectionné et modifie le userId de celui-ci
        /// </summary>
        /// <param name="DrivingLessonInstructor"></param>
        protected void InscriptionLesson(DrivingLessonInstructor DrivingLessonInstructor)
        {
            ShowExitDialog(DrivingLessonInstructor);
        }

        /// <summary>
        /// Popup permettant de valider ou non une inscription à un cour de conduite
        /// </summary>
        /// <param name="DrivingLessonInstructor"></param>
        private async void ShowExitDialog(DrivingLessonInstructor DrivingLessonInstructor)
        {
            int id = DrivingLessonInstructor.DrivingLessonId;
            var answer = await Application.Current.MainPage.DisplayAlert("Confirmer inscription", "Voulez-vous vous inscrire à la leçon du : " + DrivingLessonInstructor.DateTime + " qui se déroulera avec " + DrivingLessonInstructor.InstructorFirstName + " " + DrivingLessonInstructor.InstructorLastName, "Oui", "Non");

            if (answer)
            {
                int drivingLessonId = DrivingLessonService.UpdateUserIdForDrivingLesson(id, 1);

                ListeDrivingLessons = DrivingLessonService.GetDrivingLessonsByUserId(0, true);

                if (ListeDrivingLessons != null)
                {
                    Items.Remove(DrivingLessonInstructor);
                }

                if (drivingLessonId != 0)
                {
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

                    if (dateLecon > dateJour)
                    {
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

                    _ea.GetEvent<DrivingSentEvent>().Publish(drivingLessonId);
                    await Application.Current.MainPage.DisplayAlert("Validation", "Vous êtes désormais inscrit à la session du : " + DrivingLessonInstructor.DateTime, "Ok");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", "Une erreur est survenue pendant l'inscription à la séssion. Veuillez réésayer. Si le problème persiste, veuillez contacter l'auto école.", "Ok");
                }
            }
        }

        private void InitializeItems()
        {
            //TODO Get UserId
            ListeDrivingLessons = DrivingLessonService.GetDrivingLessonsByUserId(0, true);
            Items = new ObservableCollection<DrivingLessonInstructor>(ListeDrivingLessons);
        }
    }
}
