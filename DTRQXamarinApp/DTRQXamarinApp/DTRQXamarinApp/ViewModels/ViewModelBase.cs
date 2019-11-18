using DTRQXamarinApp.Entities;
using DTRQXamarinApp.IService;
using DTRQXamarinApp.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTRQXamarinApp.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        public TrainingSessionService TrainingSessionService { get; set; }
        public InitDatabaseService DatabaseService { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public ViewModelBase(INavigationService navigationService, TrainingSessionService trainingSessionService)
        {
            NavigationService = navigationService;
            TrainingSessionService = trainingSessionService;
        }

        public ViewModelBase(INavigationService navigationService, InitDatabaseService databaseService)
        {
            NavigationService = navigationService;
            DatabaseService = databaseService;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
