using Microsoft.EntityFrameworkCore;


namespace Evaluation.DAL.Data
{
    public class EvaluationDbContext : DbContext
    {
        public EvaluationDbContext()
        {


        }

        public EvaluationDbContext(DbContextOptions<EvaluationDbContext> options)
            : base(options)
        {
        }
    }
}
