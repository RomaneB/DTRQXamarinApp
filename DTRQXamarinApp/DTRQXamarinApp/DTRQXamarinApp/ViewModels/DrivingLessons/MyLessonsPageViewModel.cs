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
        /// Permet d'actualiser la liste quand l'utilisateur s'inscrit à une lecon
        /// </summary>
        /// <param name="id"></param>
        private void MessageReceived(int id)
        {
            Items.Add(DrivingLessonService.GetByIdWithInstructor(id));
            Items.OrderBy(s => s.DateTime);
        }

        /// <summary>
        /// Permet d'effectuer une action quand l'utilisateur annule une lecon
        /// </summary>
        /// <param name = "DrivingLessonInstructor" ></ param >
        private void AnnulerLecon(DrivingLessonInstructor DrivingLessonInstructor)
        {
            ShowExitDialog(DrivingLessonInstructor);
        }

        /// <summary>
        /// Permet d'afficher un PopUp et d'effectuer une action suivant l'option choisie
        /// </summary>
        /// <param name="DrivingLessonInstructor"></param>
        private async void ShowExitDialog(DrivingLessonInstructor DrivingLessonInstructor)
        {
            int id = DrivingLessonInstructor.DrivingLessonId;
            var answer = await Application.Current.MainPage.DisplayAlert("Confirmer desinscription", "Êtes vous sûr de vouloir vous désinscrire à la leçon du : " + DrivingLessonInstructor.DateTime + " qui se déroulera avec " + DrivingLessonInstructor.InstructorFirstName + " " + DrivingLessonInstructor.InstructorLastName, "Oui", "Non");

            if (answer)
            {

                int updateLessonId = DrivingLessonService.UpdateUserIdForDrivingLesson(id, 0);

                ListeDrivingLessons = DrivingLessonService.GetMyDrivingLessonsByUserId(int.Parse(Application.Current.Properties["UserId"].ToString()), true);

                if (ListeDrivingLessons != null)
                {
                    Items.Remove(DrivingLessonInstructor);
                }

                _ea.GetEvent<DrivingSentEventUnregister>().Publish(updateLessonId);

                if (updateLessonId != 0)
                {
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

        private void InitializeItems()
        {
            ListeDrivingLessons = DrivingLessonService.GetMyDrivingLessonsByUserId(int.Parse(Application.Current.Properties["UserId"].ToString()), true);
            Items = new ObservableCollection<DrivingLessonInstructor>(ListeDrivingLessons);
        }
    }
}
