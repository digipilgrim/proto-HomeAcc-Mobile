using TodoSQLite.Data;
using TodoSQLite.Models;

namespace TodoSQLite.Views;

[QueryProperty("Bucket", "Bucket")]
public partial class BucketPage : ContentPage
{
	//TodoItem item;
    Bucket _bucket;
    public Bucket Bucket
	{
		get => BindingContext as Bucket;
		set => BindingContext = value;
	}
    TodoItemDatabase database;
    public BucketPage(TodoItemDatabase todoItemDatabase)
    {
        InitializeComponent();
        database = todoItemDatabase;
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Bucket.UnitName))
        {
            await DisplayAlert("Name Required", "Please enter a name for the todo item.", "OK");
            return;
        }

        await database.SaveBucketAsync(Bucket);
        await Shell.Current.GoToAsync("..");
    }

    async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (Bucket.Id == 0)
            return;
        await database.DeleteBucketAsync(Bucket);
        await Shell.Current.GoToAsync("..");
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}