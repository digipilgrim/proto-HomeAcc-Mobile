namespace TodoSQLite.Models
{
    public class BucketGroup
    {
        public string GroupName { get; set; }
        public double Score { get; set; }
        public List<Bucket> Buckets { get; set; }
    }
}
