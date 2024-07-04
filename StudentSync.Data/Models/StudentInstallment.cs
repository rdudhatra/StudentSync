using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentSync.Data.Models;

public partial class StudentInstallment
{
    [Key]

    public int Id { get; set; }

    public string? ReceiptNo { get; set; }

    public DateTime? ReceiptDate { get; set; }

    public decimal? Amount { get; set; }

    public string? EnrollmentNo { get; set; }

    public string? TransactionMode { get; set; }

    public string? BankName { get; set; }

    public string? Ifsccode { get; set; }

    public string? BranchName { get; set; }

    public string? ChequeTranNo { get; set; }

    public string? Remarks { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
