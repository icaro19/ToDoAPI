using Domain.Enum;

namespace Domain.Entities
{
    public class Item
    {
        public int id {  get; set; }
        public string name { get; set; }
        public string description { get; set; }        
        public DateTime createTime { get; set; }
        public DateTime conclusionTime { get; set; }
        public Enum.TaskStatus status { get; set; }
        public User user { get; set; }
    }
}
