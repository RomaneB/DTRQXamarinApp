using DTRQXamarinApp.Entities;
using DTRQXamarinApp.Event;
using DTRQXamarinApp.Service;
using Plugin.LocalNotification;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace DTRQXamarinApp.ViewModels.DrivingLessons
{
    public class MyLessonsPageViewModel : ViewModelBase
    {
        public ObservableCollection<DrivingLessonInstructor> Items { get; set; }
        public IEnumerable<DrivingLessonInstructor> ListeDrivingLessons { get; set; }
        public IEventAggregator _ea;
        public DelegateCommand<DrivingLessonInstructor> AnnulerLeconCommand { get; set; }
        /// <summary>
        /// Constructor of the viewModel
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="ea"></param>
        /// <param name="drivingLessonService"></param>
        public MyLessonsPageViewModel(INavigationService navigationService, IEventAggregator ea, DrivingLessonService drivingLessonService)
            : base(navigationService, drivingLessonService)
        {
            Title = "Mes leçons";
            InitializeItems();
            _ea = ea;
            _ea.GetEvent<DrivingSentEvent>().Subscribe(MessageReceived);
            AnnulerLeconCommand = new DelegateCommand<DrivingLessonInstructor>(AnnulerLecon);
        }

        /// <summary>
        /// Update the list when the user subscribes to a lesson
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
        /// Allows to perform an action when the user cancels a lesson
        /// </summary>
        /// <param name = "DrivingLessonInstructor" ></ param >
        private void AnnulerLecon(DrivingLessonInstructor DrivingLessonInstructor)
        {
            ShowExitDialog(DrivingLessonInstructor);
        }

        /// <summary>
        /// Display a PopUp and perform an action depending on the chosen option
        /// </summary>
        /// <param name="DrivingLessonInstructor"></param>
        private async void ShowExitDialog(DrivingLessonInstructor DrivingLessonInstructor)
        {
            int id = DrivingLessonInstructor.DrivingLessonId;
            TimeSpan difference = DrivingLessonInstructor.DateTime - DateTime.Now;

            // If the course takes place in 3 days or more
            if (difference.Days >= 3)
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Confirmation de désinscription", "Êtes vous sûr de vouloir vous désinscrire à la leçon du \n" + DrivingLessonInstructor.DateTime + "? \n\nInstructeur : " + DrivingLessonInstructor.InstructorFirstName + " " + DrivingLessonInstructor.InstructorLastName, "Oui", "Non");

                // If the user responds positively
                if (answer)
                {
                    // The driving lesson loses the id of the user
                    int updateLessonId = DrivingLessonService.UpdateUserIdForDrivingLesson(id, 0);

                    ListeDrivingLessons = DrivingLessonService.GetMyDrivingLessonsByUserId(int.Parse(Application.Current.Properties["UserId"].ToString()), true);

                    if (ListeDrivingLessons != null)
                    {
                        // The lesson is removed from the table
                        Items.Remove(DrivingLessonInstructor);
                    }

                    // Informations of this lesson are sent to the page containing all available lessons
                    _ea.GetEvent<DrivingSentEventUnregister>().Publish(updateLessonId);

                    if (updateLessonId != 0)
                    {
                        // Deleting notifications for canceled lesson
                        InitializeItems();
                        NotificationCenter.Current.Cancel(int.Parse(DrivingLessonInstructor.DrivingLessonId.ToString() + "1"));
                        NotificationCenter.Current.Cancel(int.Parse(DrivingLessonInstructor.DrivingLessonId.ToString() + "3"));
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Erreur", "Une erreur est survenue pendant l'annulation de l'inscription à la séssion. Veuillez réésayer. Si le problème persiste, veuillez contacter l'auto école.", "Ok");
                    }
                }
            }
            // Otherwise, the user can not cancel the course
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", "Vous ne pouvez pas supprimer une session qui est prévue pour dans moins de 72h.", "Ok");
            }
        }

        // Initializing the data in the table
        private void InitializeItems()
        {
            ListeDrivingLessons = DrivingLessonService.GetMyDrivingLessonsByUserId(int.Parse(Application.Current.Properties["UserId"].ToString()), true);
            Items = new ObservableCollection<DrivingLessonInstructor>(ListeDrivingLessons);
        }
    }
}
