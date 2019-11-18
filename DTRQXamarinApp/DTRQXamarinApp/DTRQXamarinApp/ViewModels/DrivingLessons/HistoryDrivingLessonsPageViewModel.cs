using DTRQXamarinApp.Entities;
using DTRQXamarinApp.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace DTRQXamarinApp.ViewModels.DrivingLessons
{
    public class HistoryDrivingLessonsPageViewModel : ViewModelBase
    {
        public IEnumerable<DrivingLessonInstructor> ListeDrivingLessons { get; set; }
        public ObservableCollection<DrivingLessonInstructor> Items { get; set; }
        public DelegateCommand<DrivingLessonInstructor> CommentLessonCommand { get; set; }
        public HistoryDrivingLessonsPageViewModel(INavigationService navigationService, DrivingLessonService drivingLessonService)
           : base(navigationService, drivingLessonService)
        {
            Title = "Historique des leçons";
            InitializeItems();
            CommentLessonCommand = new DelegateCommand<DrivingLessonInstructor>(CommentLesson);
        }

        /// <summary>
        /// Permet de voir les informations sur une leçon passée
        /// </summary>
        /// <param name="drivingLesson_Instructor"></param>
        private void CommentLesson(DrivingLessonInstructor drivingLessonInstructor)
        {
            int id = drivingLessonInstructor.DrivingLessonId;
            if (id != 0)
            {
                Application.Current.MainPage.DisplayAlert("Informations leçon", "Date de la leçon : " + drivingLessonInstructor.DateTime + "\nInstructeur :" + drivingLessonInstructor.InstructorFirstName + " " + drivingLessonInstructor.InstructorLastName + "\nCommentaires sur la leçon : \n" + drivingLessonInstructor.Text, "Ok");
            }
        }

        private void InitializeItems()
        {
            ListeDrivingLessons = DrivingLessonService.GetDrivingLessonsByUserId(int.Parse(Application.Current.Properties["UserId"].ToString()), false);
            Items = new ObservableCollection<DrivingLessonInstructor>(ListeDrivingLessons);
        }
    }
}
