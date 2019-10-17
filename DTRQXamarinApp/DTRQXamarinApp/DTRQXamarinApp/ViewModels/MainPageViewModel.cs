using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTRQXamarinApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        //For Binding Command
        public DelegateCommand OpenDrivingLessonCommand { get; set; }
        public DelegateCommand OpenTrainingSessionCommand { get; set; }
        public DelegateCommand OpenHomeCommand { get; set; }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Accueil";
            OpenDrivingLessonCommand = new DelegateCommand(OpenDrivingLesson);
            OpenTrainingSessionCommand = new DelegateCommand(OpenTrainingSession);

        }

        private void OpenTrainingSession()
        {
            NavigationService.NavigateAsync("TrainingSessionsTabbedPage");
        }

        private void OpenDrivingLesson()
        {
            NavigationService.NavigateAsync("DrivingLessonsTabbedPage");
        }
    }
}
