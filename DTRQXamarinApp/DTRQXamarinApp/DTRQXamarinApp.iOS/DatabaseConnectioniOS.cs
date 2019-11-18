using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using Foundation;
using SQLite;
using UIKit;
using DTRQXamarinApp.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnectioniOS))]
namespace DTRQXamarinApp.iOS
{
    public class DatabaseConnectioniOS : IDatabase
    {
        public SQLiteConnection Connection()
        {
            string personalForlder = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryFolder = Path.Combine(personalForlder, "..", "Library");
            var path = Path.Combine(libraryFolder, "IOS");
            return new SQLiteConnection(path);
        }
    }
}