using Benday.Presidents.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Benday.DataAccess;
using Benday.DataAccess.SqlServer;

namespace Benday.Presidents.Api.DataAccess.SqlServer
{
    public class SqlEntityFrameworkFeatureRepository :
            SqlEntityFrameworkCrudRepositoryBase<Feature, PresidentsDbContext>, IFeatureRepository
    {
        public SqlEntityFrameworkFeatureRepository(
            PresidentsDbContext context) : base(context)
        {

        }

        protected override DbSet<Feature> EntityDbSet => Context.Features;

        public IList<Feature> GetByUsername(string username)
        {
            return (
                from temp in EntityDbSet
                where (temp.Username == username || temp.Username == String.Empty)
                select temp
                ).ToList();
        }
    }
}
