using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LoanApplicationSystem2._0.Models;

public class Disbursement
{
    public int DisbursementId { get; set; }
    public int LoanApplicationId { get; set; }


    public decimal DisbursedAmount { get; set; }
    public DateTime DisbursementDate { get; set; }
    public string? RepaymentSchedule { get; set; }
}