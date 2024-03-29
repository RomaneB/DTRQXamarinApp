﻿using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DTRQXamarinApp.ViewModels.TrainingSessions;
using DTRQXamarinApp.ViewModels.DrivingLessons;
using DTRQXamarinApp.Views.DrivingLessons;
using DTRQXamarinApp.Views.TrainingSessions;
using DTRQXamarinApp.Views;
using DTRQXamarinApp.ViewModels;
using DTRQXamarinApp.Service;
using System.Threading;
using DTRQXamarinApp.IRepository;
using DTRQXamarinApp.Repository;
using DTRQXamarinApp.Entities;
using DTRQXamarinApp.IService;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DTRQXamarinApp
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            //If the user is logged in
            if (Application.Current.Properties.ContainsKey("UserId"))
            {
                // The application opens on the home page
                await NavigationService.NavigateAsync("NavigationPage/MainPage");
            }
            else
            {
                // The application opens on the log page
                await NavigationService.NavigateAsync("NavigationPage/LogPage");
            }
        }
        
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();

            containerRegistry.RegisterForNavigation<TrainingSessionsTabbedPage, TrainingSessionsTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<TrainingSessionsListPage, TrainingSessionsListPageViewModel>();
            containerRegistry.RegisterForNavigation<MyTrainingsPage, MyTrainingsPageViewModel>();
            containerRegistry.RegisterForNavigation<TrainingSessionsResultsPage, TrainingSessionsResultsPageViewModel>();

            containerRegistry.RegisterForNavigation<DrivingLessonsTabbedPage, DrivingLessonsTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<DrivingLessonsListPage, DrivingLessonsListPageViewModel>();
            containerRegistry.RegisterForNavigation<MyLessonsPage, MyLessonsPageViewModel>();
            containerRegistry.RegisterForNavigation<HistoryDrivingLessonsPage, HistoryDrivingLessonsPageViewModel>();

            containerRegistry.RegisterForNavigation<LogPage, LogPageViewModel>();

            //Repository
            containerRegistry.Register(typeof(IRepository<>), typeof(Repository<>));

            //Service
            containerRegistry.Register(typeof(IService<TrainingSession>), typeof(TrainingSessionService));
            containerRegistry.Register(typeof(IService<DrivingLesson>), typeof(DrivingLessonService));
        }

    }
}
