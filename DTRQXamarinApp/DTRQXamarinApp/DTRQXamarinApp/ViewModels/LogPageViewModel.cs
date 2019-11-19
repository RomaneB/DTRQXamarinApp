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

        /// <summary>
        /// Constructor of the viewModel
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="ea"></param>
        /// <param name="userService"></param>
        public LogPageViewModel(INavigationService navigationService, IEventAggregator ea, UserService userService) : base(navigationService, userService)
        {
            Title = "LogPage";
            _ea = ea;
            LoginCommand = new DelegateCommand(Login);
        }

        /// <summary>
        /// This method allows the verification of the existence of the user when he clicks on the button to connect
        /// </summary>
        private void Login()
        {
            // Removes all values ​​in the application properties
            Application.Current.Properties.Clear();
            // Creating a property containing the id of the user who wants to connect.
            Application.Current.Properties.Add("UserId", UserService.GetUserIdByUserAndPassword(UserName, Password));
            int userId = int.Parse(Application.Current.Properties["UserId"].ToString());

            // If user exist
            if (userId != 0)
            {
                // Access to the home page
                NavigationService.NavigateAsync("NavigationPage/MainPage", useModalNavigation: true);
            }
            else
            {
                // Removes all values ​​in the application properties
                Application.Current.Properties.Clear();
                Application.Current.MainPage.DisplayAlert("Error", "Utilisateur ou mot de passe invalide.", "OK");
            }
        }
    }
}
