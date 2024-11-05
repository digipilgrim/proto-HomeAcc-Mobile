namespace TodoSQLite;

public static class Constants
{
    //public const string DatabaseFilename = "HomeAcc.db";

    public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

    //public static string DatabasePath =>
    //   Path.Combine("storage/emulated/0/Download", DatabaseFilename);

    public static string DatabasePath => Path.Combine("storage/emulated/0/Download", Preferences.Get("databasePath", "HomeAcc.db"));
    // Path.Combine(Environment.SpecialFolder.MyDocuments.ToString(), DatabaseFilename);
}
