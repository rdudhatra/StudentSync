using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentSync.Data.Models;

public partial class InquiryFollowUp
{
    [Key]

    public int Id { get; set; }

    public DateTime? InquiryDate { get; set; }

    public int? InquiryNo { get; set; }

    public string? Through { get; set; }

    public string? Remarks { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
