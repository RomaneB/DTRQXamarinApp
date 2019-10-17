using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTRQXamarinApp.ViewModels
{
    public class DrivingLessonsListPageViewModel : ViewModelBase
    {
        public DrivingLessonsListPageViewModel(INavigationService navigationService)
           : base(navigationService)
        {
            Title = "Leçons disponibles";
        }
    }
}
