using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DTRQXamarinApp.UWP;
using SQLite;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(DatabaseConnectionUWP))]
namespace DTRQXamarinApp.UWP
{
    public class DatabaseConnectionUWP : IDatabase
    {
        public SQLiteConnection Connection()
        {
            var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "UWP");
            return new SQLiteConnection(path);
        }
    }
}
