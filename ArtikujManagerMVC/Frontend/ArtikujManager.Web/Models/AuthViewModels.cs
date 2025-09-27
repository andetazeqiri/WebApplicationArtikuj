using System.ComponentModel.DataAnnotations;

namespace ArtikujManager.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email është i detyrueshëm")]
        [EmailAddress(ErrorMessage = "Formati i email-it nuk është valid")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fjalëkalimi është i detyrueshëm")]
        [DataType(DataType.Password)]
        [Display(Name = "Fjalëkalimi")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Më mbaj mend")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email është i detyrueshëm")]
        [EmailAddress(ErrorMessage = "Formati i email-it nuk është valid")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fjalëkalimi është i detyrueshëm")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Fjalëkalimi duhet të jetë të paktën 6 karaktere")]
        [DataType(DataType.Password)]
        [Display(Name = "Fjalëkalimi")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Konfirmimi i fjalëkalimit është i detyrueshëm")]
        [DataType(DataType.Password)]
        [Display(Name = "Konfirmo Fjalëkalimin")]
        [Compare("Password", ErrorMessage = "Fjalëkalimet nuk përputhen")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Emri është i detyrueshëm")]
        [Display(Name = "Emri")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mbiemri është i detyrueshëm")]
        [Display(Name = "Mbiemri")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Roli është i detyrueshëm")]
        [Display(Name = "Roli")]
        public string Role { get; set; } = string.Empty;
    }

    public class UserInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public UserInfo? User { get; set; }
    }
}