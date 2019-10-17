using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTRQXamarinApp.ViewModels
{
    public class MyTrainingsPageViewModel : ViewModelBase
    {
        public MyTrainingsPageViewModel(INavigationService navigationService)
           : base(navigationService)
        {
            Title = "Mes sessions";
        }
    }
}
