using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTRQXamarinApp.ViewModels.TrainingSessions
{
    public class TrainingSessionsTabbedPageViewModel : ViewModelBase
    {
        public TrainingSessionsTabbedPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Sessions d'entrainement";
        }
    }
}
