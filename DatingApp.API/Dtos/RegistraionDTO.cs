using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class RegistraionDTO
    {
        [Required]
        [StringLength(20,ErrorMessage="The Name length should between 5 and 10",MinimumLength=5)]
        public string Name { get; set; }
        [Required]
        [StringLength(10,ErrorMessage="The UserName length should between 5 and 10",MinimumLength=5)]
        public string UserName { get; set; }
        [Required]
       [StringLength(15,ErrorMessage="The Password length should between 8 and 10",MinimumLength=8)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}