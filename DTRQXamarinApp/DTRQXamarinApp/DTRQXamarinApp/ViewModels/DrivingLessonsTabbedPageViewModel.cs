using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTRQXamarinApp.ViewModels
{
    public class DrivingLessonsTabbedPageViewModel : ViewModelBase
    {
        public DrivingLessonsTabbedPageViewModel(INavigationService navigationService)
           : base(navigationService)
        {
            Title = "Leçons de conduite";
        }
    }
}
