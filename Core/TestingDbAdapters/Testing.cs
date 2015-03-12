using System;
using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace Core.TestingDbAdapters
{
    [Table("Testing")]
    [ProtoContract]
    public class Testing
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public int TreadId { get; set; }
        [ProtoMember(4)]
        public DateTime AddTime { get; set; }
    }
}
