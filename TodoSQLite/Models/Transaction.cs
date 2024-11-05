using SQLite;
using System.Collections.ObjectModel;

namespace TodoSQLite.Models
{
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public long BucketId { get; set; }
        public double Value { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
    }
}