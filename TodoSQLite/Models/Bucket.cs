using SQLite;

namespace TodoSQLite.Models
{
    public class Bucket
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string UnitName { get; set; }
        public double Score { get; set; }
        public double Multiplier { get; set; }
    }
}
