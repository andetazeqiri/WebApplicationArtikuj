using System.ComponentModel.DataAnnotations;

namespace ArtikujManager.API.DTOs
{
    public class ArtikujDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Emri eshte i detyrueshme")]
        [MaxLength(100, ErrorMessage = "Emri nuk mund te jete me shume se 100 karaktere")]
        public string Emri { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cmimi eshte i detyrueshme")]
        [Range(0, double.MaxValue, ErrorMessage = "Cmimi duhet te jete pozitiv")]
        public decimal Cmimi { get; set; }

        [MaxLength(50, ErrorMessage = "Njesia nuk mund te jete me shume se 50 karaktere")]
        public string Njesia { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "Barkodi nuk mund te jete me shume se 100 karaktere")]
        public string Barkodi { get; set; } = string.Empty;

        public DateTime? DataSkadences { get; set; }

        [MaxLength(20, ErrorMessage = "Lloj nuk mund te jete me shume se 20 karaktere")]
        public string Lloj { get; set; } = "Vendi";

        public bool KaTvsh { get; set; } = false;

        [MaxLength(20, ErrorMessage = "Tipi nuk mund te jete me shume se 20 karaktere")]
        public string Tipi { get; set; } = "Ushqimor";

        public DateTime DataKrijimit { get; set; }
        public DateTime? DataModifikimit { get; set; }
        public string? KrijuarNga { get; set; }
        public string? ModifikuarNga { get; set; }
    }

    public class CreateArtikujDto
    {
        [Required(ErrorMessage = "Emri eshte i detyrueshme")]
        [MaxLength(100, ErrorMessage = "Emri nuk mund te jete me shume se 100 karaktere")]
        public string Emri { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cmimi eshte i detyrueshme")]
        [Range(0, double.MaxValue, ErrorMessage = "Cmimi duhet te jete pozitiv")]
        public decimal Cmimi { get; set; }

        [MaxLength(50, ErrorMessage = "Njesia nuk mund te jete me shume se 50 karaktere")]
        public string Njesia { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "Barkodi nuk mund te jete me shume se 100 karaktere")]
        public string Barkodi { get; set; } = string.Empty;

        public DateTime? DataSkadences { get; set; }

        [MaxLength(20, ErrorMessage = "Lloj nuk mund te jete me shume se 20 karaktere")]
        public string Lloj { get; set; } = "Vendi";

        public bool KaTvsh { get; set; } = false;

        [MaxLength(20, ErrorMessage = "Tipi nuk mund te jete me shume se 20 karaktere")]
        public string Tipi { get; set; } = "Ushqimor";
    }

    public class UpdateArtikujDto
    {
        [Required(ErrorMessage = "Emri eshte i detyrueshme")]
        [MaxLength(100, ErrorMessage = "Emri nuk mund te jete me shume se 100 karaktere")]
        public string Emri { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cmimi eshte i detyrueshme")]
        [Range(0, double.MaxValue, ErrorMessage = "Cmimi duhet te jete pozitiv")]
        public decimal Cmimi { get; set; }

        [MaxLength(50, ErrorMessage = "Njesia nuk mund te jete me shume se 50 karaktere")]
        public string Njesia { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "Barkodi nuk mund te jete me shume se 100 karaktere")]
        public string Barkodi { get; set; } = string.Empty;

        public DateTime? DataSkadences { get; set; }

        [MaxLength(20, ErrorMessage = "Lloj nuk mund te jete me shume se 20 karaktere")]
        public string Lloj { get; set; } = "Vendi";

        public bool KaTvsh { get; set; } = false;

        [MaxLength(20, ErrorMessage = "Tipi nuk mund te jete me shume se 20 karaktere")]
        public string Tipi { get; set; } = "Ushqimor";
    }
}