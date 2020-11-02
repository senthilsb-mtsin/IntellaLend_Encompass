using System.ComponentModel.DataAnnotations;

namespace DataImport
{
    public partial class CONFIGURATION
    {
        [Key]
        public int Id { get; set; }
        public string KEY { get; set; }
        public string VALUE { get; set; }
    }
}
