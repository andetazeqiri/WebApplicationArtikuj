using System.ComponentModel.DataAnnotations;

namespace ArtikujManager.API.Models
{
    public class Artikuj
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Emri { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Çmimi duhet të jetë pozitiv")]
        public decimal Cmimi { get; set; }

        [MaxLength(50)]
        public string Njesia { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Barkodi { get; set; } = string.Empty;

        public DateTime? DataSkadences { get; set; }

        [MaxLength(20)]
        public string Lloj { get; set; } = "Vendi"; 

        public bool KaTvsh { get; set; } = false;

        [MaxLength(20)]
        public string Tipi { get; set; } = "Ushqimor"; 

        public DateTime DataKrijimit { get; set; } = DateTime.Now;

        public DateTime? DataModifikimit { get; set; }

        
        public string? KrijuarNga { get; set; }
        public string? ModifikuarNga { get; set; }
    }
}