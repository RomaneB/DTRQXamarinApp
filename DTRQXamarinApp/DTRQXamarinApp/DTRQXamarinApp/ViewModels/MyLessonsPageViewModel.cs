using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTRQXamarinApp.ViewModels
{
    public class MyLessonsPageViewModel : ViewModelBase
    {
        public MyLessonsPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Mes leçons";
        }
    }
}
