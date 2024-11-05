using SQLite;
using TodoSQLite.Models;

namespace TodoSQLite.Data;

public class TodoItemDatabase
{
    SQLiteAsyncConnection Database;
    public TodoItemDatabase()
    {
    }
    async Task InitBucket()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await Database.CreateTableAsync<Bucket>();
    }
    async Task InitTransaction()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await Database.CreateTableAsync<Transaction>();
    }

    public async Task<List<Bucket>> GetBucketsAsync()
    {
        await InitBucket();
        return await Database.Table<Bucket>().ToListAsync();
    }

    public async Task<Bucket> GetBucketsByIdAsync(long Id)
    {
        return await Database.Table<Bucket>().Where(b => b.Id == Id).FirstOrDefaultAsync();
    }
   
    public async Task<List<Transaction>> GetTransactionsAsync()
    {
        await InitTransaction();
        return await Database.Table<Transaction>().ToListAsync();
    }

    //public async Task<List<TodoItem>> GetItemsNotDoneAsync()
    //{
    //    await Init();
    //    return await Database.Table<TodoItem>().Where(t => t.Done).ToListAsync();

    //    // SQL queries are also possible
    //    //return await Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
    //}

    public async Task<int> SaveBucketAsync(Bucket bucket)
    {
        await InitBucket();
        if (bucket.Id != 0)
        {
            return await Database.UpdateAsync(bucket);
        }
        else
        {
            return await Database.InsertAsync(bucket);
        }
    }
    public async Task<int> SaveTransactionAsync(Transaction transaction)
    {
        await InitTransaction();
        if (transaction.Id != 0)
        {
            return await Database.UpdateAsync(transaction);
        }
        else
        {
            return await Database.InsertAsync(transaction);
        }
    }

    public async Task<int> DeleteBucketAsync(Bucket bucket)
    {
        await InitBucket();
        var toDelTrans = await Database.Table<Transaction>().Where(t => t.BucketId == bucket.Id).ToListAsync();
        foreach (var transaction in toDelTrans)
        {
            await Database.DeleteAsync(transaction);
        }
        return await Database.DeleteAsync(bucket);
    }
    public async Task<int> DeleteTransactionAsync(Transaction transaction)
    {
        await InitTransaction();
        return await Database.DeleteAsync(transaction);
    }
}