using System;
using System.Linq;
using System.Collections.Generic;

namespace Benday.Presidents.Api.Models
{
    public class DefaultDaysInOfficeStrategy : IDaysInOfficeStrategy
    {
        public int GetDaysInOffice(IEnumerable<Term> terms)
        {
            if (terms == null || terms.Count() == 0)
            {
                return 0;
            }
            else
            {
                int totalDays = 0;

                foreach (var term in terms)
                {
                    var diff = term.End - term.Start;

                    totalDays += Convert.ToInt32(diff.TotalDays);
                }

                return totalDays;
            }
        }
    }
}
