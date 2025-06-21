using LoanApplication2._0.Migrations;
using LoanApplication2._0.Models;
using Microsoft.EntityFrameworkCore; // Required for DbContext and DbSet
using System.Collections.Generic;

namespace LoanApplicationSystem2._0.Models
{
    public class ApplicationDbContext : DbContext
{
    // Use dependency injection for DbContextOptions
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<LoanApplication> LoanApplications { get; set; }

    public DbSet<LoanDocument> LoanDocuments { get; set; }
    public DbSet<Approval> Approvals { get; set; }
    public DbSet<AdminLoginRecord> AdminLoginRecords { get; set; }
    public DbSet<ClientLoginRecord> ClientLoginRecords { get; set; }
    public DbSet<Disbursement> Disbursements { get; set; }
    public DbSet<CreditScoreViewModel> creditScores { get; set; }
    public DbSet<LoanApplicationSystem2._0.Models.Client> Clients { get; set; }
    }
}