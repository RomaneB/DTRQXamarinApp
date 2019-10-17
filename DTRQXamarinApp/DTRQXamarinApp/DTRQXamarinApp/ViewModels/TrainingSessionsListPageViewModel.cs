using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTRQXamarinApp.ViewModels
{
    public class TrainingSessionsListPageViewModel : ViewModelBase
    {
        public TrainingSessionsListPageViewModel(INavigationService navigationService)
           : base(navigationService)
        {
            Title = "Sessions disponibles";
        }
    }
}
