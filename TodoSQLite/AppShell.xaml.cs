using TodoSQLite.Views;

namespace TodoSQLite;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(BucketPage), typeof(BucketPage));
        Routing.RegisterRoute(nameof(TransactionPage), typeof(TransactionPage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
    }
}
