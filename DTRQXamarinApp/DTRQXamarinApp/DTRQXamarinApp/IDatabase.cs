using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTRQXamarinApp
{
    public interface IDatabase
    {
        SQLiteConnection Connection();
    }
}
