using DTRQXamarinApp.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace DTRQXamarinApp.ViewModels
{
    public class LogPageViewModel : ViewModelBase
    {
        public DelegateCommand LoginCommand { get; set; }
        public IEventAggregator _ea;
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        public LogPageViewModel(INavigationService navigationService, IEventAggregator ea, UserService userService) : base(navigationService, userService)
        {
            Title = "LogPage";
            _ea = ea;
            LoginCommand = new DelegateCommand(Login);
        }

        private void Login()
        {
            Application.Current.Properties.Clear();
            Application.Current.Properties.Add("UserId", UserService.GetUserIdByUserAndPassword(UserName, Password));

            int userId = int.Parse(Application.Current.Properties["UserId"].ToString());

            if (userId != 0)
            {
                NavigationService.NavigateAsync("NavigationPage/MainPage", useModalNavigation: true);
            }
            else
            {
                Application.Current.Properties.Clear();
                Application.Current.MainPage.DisplayAlert("Error", "Utilisateur ou mot de passe invalide.", "OK");
            }
        }
    }
}
