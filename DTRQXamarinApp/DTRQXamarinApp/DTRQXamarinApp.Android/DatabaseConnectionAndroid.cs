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
using DTRQXamarinApp.Droid;
using DTRQXamarinApp.Entities;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(DatabaseConnectionAndroid))]
namespace DTRQXamarinApp.Droid
{

    public class DatabaseConnectionAndroid : IDatabase
    {
        public SQLiteConnection Connection()
        {
            try
            {
                var dbName = "dbDSL.db3";
                var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                path = Path.Combine(path, dbName);

                return new SQLiteConnection(path);
            }
            catch (SQLiteException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}