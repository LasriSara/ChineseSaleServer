using System.ComponentModel.DataAnnotations;

namespace serverProjectFinal.DTO
{
    public class LoginDTO
    {
        public string PassWord { get; set; }
        [EmailAddress]
        public string Email { get; set; }

    }
}
