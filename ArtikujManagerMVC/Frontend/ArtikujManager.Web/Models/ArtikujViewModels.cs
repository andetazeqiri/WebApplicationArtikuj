using System.ComponentModel.DataAnnotations;

namespace ArtikujManager.Web.Models
{
    public class ArtikujViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Emri është i detyrueshëm")]
        [Display(Name = "Emri")]
        public string Emri { get; set; } = string.Empty;

        [Required(ErrorMessage = "Çmimi është i detyrueshëm")]
        [Display(Name = "Çmimi")]
        [Range(0, double.MaxValue, ErrorMessage = "Çmimi duhet të jetë pozitiv")]
        public decimal Cmimi { get; set; }

        [Display(Name = "Njësia")]
        public string Njesia { get; set; } = string.Empty;

        [Display(Name = "Barkodi")]
        public string Barkodi { get; set; } = string.Empty;

        [Display(Name = "Data e Skadencës")]
        public DateTime? DataSkadences { get; set; }

        [Display(Name = "Lloj")]
        public string Lloj { get; set; } = "Vendi";

        [Display(Name = "Ka TVSH")]
        public bool KaTvsh { get; set; } = false;

        [Display(Name = "Tipi")]
        public string Tipi { get; set; } = "Ushqimor";

        [Display(Name = "Data e Krijimit")]
        public DateTime DataKrijimit { get; set; }

        [Display(Name = "Data e Modifikimit")]
        public DateTime? DataModifikimit { get; set; }

        [Display(Name = "Krijuar nga")]
        public string? KrijuarNga { get; set; }

        [Display(Name = "Modifikuar nga")]
        public string? ModifikuarNga { get; set; }
    }

    public class CreateArtikujViewModel
    {
        [Required(ErrorMessage = "Emri është i detyrueshëm")]
        [Display(Name = "Emri")]
        public string Emri { get; set; } = string.Empty;

        [Required(ErrorMessage = "Çmimi është i detyrueshëm")]
        [Display(Name = "Çmimi")]
        [Range(0, double.MaxValue, ErrorMessage = "Çmimi duhet të jetë pozitiv")]
        public decimal Cmimi { get; set; }

        [Display(Name = "Njësia")]
        public string Njesia { get; set; } = string.Empty;

        [Display(Name = "Barkodi")]
        public string Barkodi { get; set; } = string.Empty;

        [Display(Name = "Data e Skadencës")]
        public DateTime? DataSkadences { get; set; }

        [Display(Name = "Lloj")]
        public string Lloj { get; set; } = "Vendi";

        [Display(Name = "Ka TVSH")]
        public bool KaTvsh { get; set; } = false;

        [Display(Name = "Tipi")]
        public string Tipi { get; set; } = "Ushqimor";
    }

    public class ArtikujListViewModel
    {
        public IEnumerable<ArtikujViewModel> Artikujt { get; set; } = new List<ArtikujViewModel>();
        public string? SearchTerm { get; set; }
        public string? CurrentUserRole { get; set; }
    }
}