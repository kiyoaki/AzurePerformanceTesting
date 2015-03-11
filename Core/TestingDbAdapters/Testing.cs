using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzureSqlDatabaseStressTestTool
{
    [Table("Testing")] 
    public class Testing
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int TreadId { get; set; }
        public DateTime AddTime { get; set; }
    }
}
