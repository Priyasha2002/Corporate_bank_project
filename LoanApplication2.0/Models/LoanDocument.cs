using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LoanApplicationSystem2._0.Models;

namespace LoanApplicationSystem2._0.Models
{
    public class LoanDocument
    {
        [Key]
        public int DocumentId { get; set; }

        [ForeignKey("LoanApplication")]
        public int ApplicationId { get; set; }

        public string DocumentType { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public bool IsValid { get; set; }

        public virtual LoanApplication LoanApplication { get; set; }
    }
}