using TodoSQLite.Data;
using TodoSQLite.Models;

namespace TodoSQLite.Views;
[QueryProperty("Transaction", "Transaction")]
public partial class TransactionPage : ContentPage
{
    public Transaction Transaction
    {
        get => BindingContext as Transaction;
        set => BindingContext = value;
    }
    TodoItemDatabase database;
    public TransactionPage(TodoItemDatabase todoItemDatabase)
    {
        InitializeComponent();
        database = todoItemDatabase;
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        

        var bucket = await database.GetBucketsByIdAsync(Transaction.BucketId);
        if (bucket == default)
        {
            await DisplayAlert("Bucket Required", "Please enter a BucketId for the Transaction.", "OK");
            return;
        }

        if (EntryDT.Text == default || EntryDT.Text.Contains(".2000") || EntryDT.Text.Contains(".0001"))
        {
            EntryDT.Text = DateTime.Now.ToString();
            bucket.Score = bucket.Score + Transaction.Value;
            await database.SaveBucketAsync(bucket);
        }

        await database.SaveTransactionAsync(Transaction);

        await Shell.Current.GoToAsync("..");
    }

    async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (Transaction.Id == 0)
            return;
        
        var bucket = await database.GetBucketsByIdAsync(Transaction.BucketId);
        if (bucket != default)
        {
            bucket.Score = bucket.Score - Transaction.Value;
            await database.SaveBucketAsync(bucket);
        }
        else
        {
            await DisplayAlert("Bucket Required", "Please enter a BucketId for the Transaction.", "OK");
            return;
        }
        await database.DeleteTransactionAsync(Transaction);
        await Shell.Current.GoToAsync("..");
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}