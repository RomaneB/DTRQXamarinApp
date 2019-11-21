using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DTRQXamarin.Droid;
using SQLite;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection_Android))]
namespace DTRQXamarin.Droid
{
    public class DatabaseConnection_Android : IDatabaseConnection
    {
        public SQLiteConnection DbConnection(string dbPath)
        {
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),"Android");
            return new SQLiteConnection(path);
        }
    }
}