using Android.App;
using Android.Content.PM;
using Android.OS;
using DTRQXamarinApp.Entities;
using DTRQXamarinApp.IRepository;
using DTRQXamarinApp.IService;
using DTRQXamarinApp.Repository;
using DTRQXamarinApp.Service;
using Prism;
using Prism.Ioc;

namespace DTRQXamarinApp.Droid
{
    [Activity(Label = "DTRQXamarinApp", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register(typeof(IRepository<>), typeof(Repository<>));

            containerRegistry.Register(typeof(IService<TrainingSession>), typeof(TrainingSessionService));

            // Register any platform specific implementations
            containerRegistry.Register<IDatabase, DatabaseConnectionAndroid>();
        }
    }
}

