using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WukkaManCleaning_API.Models;

namespace WukkaManCleaning_API.Data
{
    public class IdentityApplicationDbContext: IdentityDbContext
    {
        public IdentityApplicationDbContext(DbContextOptions<IdentityApplicationDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeTask> EmployeesTasks { get; set; }
        public DbSet<EmplyeeClockShift> EmplyeeClockShifts { get;set; }
        public DbSet<EmpTask> EmpTasks { get; set; }
    }

}
