using System.Collections.ObjectModel;
using TodoSQLite.Data;
using TodoSQLite.Models;

namespace TodoSQLite.Views;

public partial class TodoListPage : ContentPage
{
    TodoItemDatabase database;
    public ObservableCollection<Transaction> Transaction { get; set; } = new();
    public ObservableCollection<Bucket> Bucket { get; set; } = new();
    public ObservableCollection<BucketGroup> BucketGroups { get; set; } = new();
    public double Sum { get; set; }
    public TodoListPage(TodoItemDatabase todoItemDatabase)
	{
		InitializeComponent();
        database = todoItemDatabase;
        BindingContext = this;
    }

    private async void SetBucketGroups()
    {
        try
        {
            var transactions = await database.GetTransactionsAsync();
            var buckets = await database.GetBucketsAsync();

            if (transactions != null && buckets != null)
            {
                var bucketGroup1 = new BucketGroup();
                bucketGroup1.GroupName = "Наличка";
                bucketGroup1.Buckets = new List<Bucket>();
                bucketGroup1.Buckets.AddRange(buckets.Where(b => b.Id > 0 && b.Id < 8));
                foreach (var bucket in bucketGroup1.Buckets)
                    bucketGroup1.Score += buckets.Where(t => t.Id == bucket.Id).Sum(t => t.Score * t.Multiplier);
                bucketGroup1.Score = Math.Round(bucketGroup1.Score, 3);

                var bucketGroup2 = new BucketGroup();
                bucketGroup2.GroupName = "Безнал";
                bucketGroup2.Buckets = new List<Bucket>();
                bucketGroup2.Buckets.AddRange(buckets.Where(b => b.Id == 8 || b.Id == 9));
                foreach (var bucket in bucketGroup2.Buckets)
                    bucketGroup2.Score += buckets.Where(t => t.Id == bucket.Id).Sum(t => t.Score * t.Multiplier);
                bucketGroup2.Score = Math.Round(bucketGroup2.Score, 3);

                BucketGroups.Clear();
                BucketGroups.Add(bucketGroup1);
                BucketGroups.Add(bucketGroup2);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Exception", ex.Message, "OK");
            return;
        }
    }

    private void LoadSum(List<Bucket> buckets, List<Transaction> transactions)
    {
        foreach (var bucket in buckets)
            bucket.Score = Math.Round(transactions.Where(t => t.BucketId == bucket.Id).Sum(t => t.Value), 3);
    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        try
        {
            base.OnNavigatedTo(args);
            var transactions = await database.GetTransactionsAsync();
            var buckets = await database.GetBucketsAsync();
            LoadSum(buckets, transactions);
            Sum = Math.Round(Enumerable.Sum(buckets.Select(b => b.Score * b.Multiplier)), 3);
            SumLbl.Text = Sum.ToString();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Transaction.Clear();
                Bucket.Clear();
                foreach (var transaction in transactions)
                    Transaction.Add(transaction);
                foreach (var bucket in buckets)
                    Bucket.Add(bucket);
                SetBucketGroups();

            });
        }
            catch (Exception ex)
        {
            await DisplayAlert("Exception", ex.Message, "OK");
            return;
        }
    }
    async void OnTransactionAdded(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TransactionPage), true, new Dictionary<string, object>
        {
            ["Transaction"] = new Transaction()
        });
    }

    async void OnBucketAdded(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(BucketPage), true, new Dictionary<string, object>
        {
            ["Bucket"] = new Bucket()
        });
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Transaction item)
            await Shell.Current.GoToAsync(nameof(TransactionPage), true, new Dictionary<string, object>
            {
                ["Transaction"] = item
            });
        else if (e.CurrentSelection.FirstOrDefault() is Bucket item2)
            await Shell.Current.GoToAsync(nameof(BucketPage), true, new Dictionary<string, object>
            {
                ["Bucket"] = item2
            });
        else
            return;
    }

    private async void On_Settings_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage), true);
    }

    private void ToButtomButton_Clicked(object sender, EventArgs e)
    {
        TransactionCollectionView.ScrollTo(TransactionCollectionView.ItemsSource.Cast<object>().Count());
    }

    private void ToTopButton_Clicked(object sender, EventArgs e)
    {
        TransactionCollectionView.ScrollTo(0);
    }
}

