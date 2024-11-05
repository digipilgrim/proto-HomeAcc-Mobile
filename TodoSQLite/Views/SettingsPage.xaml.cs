namespace TodoSQLite.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
		WelcomeLbl.Text = Constants.DatabasePath;

    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        Preferences.Set("databasePath", WelcomeLbl.Text);
        Navigation.PopAsync();
    }
}