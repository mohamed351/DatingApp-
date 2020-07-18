
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DatingApp.API.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }

        public string Name { get; set; }

        public byte[] PasswordHashed { get; set; }

        public byte[] PasswordSalt { get; set; }
    }
}