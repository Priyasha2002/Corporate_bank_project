using System;

using System.Collections.Generic;

using System.Linq;

using LoanApplicationSystem2._0.Models;

public class DisbursementService

{

    private readonly ApplicationDbContext _context;

    public DisbursementService(ApplicationDbContext context)

    {

        _context = context;

    }

    public void TrackDisbursement(Disbursement disbursement)

    {

        _context.Disbursements.Add(disbursement);

        _context.SaveChanges();

    }

    public IEnumerable<Disbursement> GenerateDisbursementReport(DateTime? from = null, DateTime? to = null)

    {

        var query = _context.Disbursements.AsQueryable();

        if (from.HasValue)

            query = query.Where(d => d.DisbursementDate >= from.Value);

        if (to.HasValue)

            query = query.Where(d => d.DisbursementDate <= to.Value);

        return query.ToList();

    }

}
