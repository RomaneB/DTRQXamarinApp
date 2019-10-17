using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTRQXamarinApp.ViewModels.DrivingLessons
{
    public class HistoryDrivingLessonsPageViewModel : ViewModelBase
    {
        public HistoryDrivingLessonsPageViewModel(INavigationService navigationService)
           : base(navigationService)
        {
            Title = "Historique des leçons";
        }
    }
}
