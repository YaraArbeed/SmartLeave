using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartLeave.DAL.Entities;
using SmartLeave.DAL.SeedConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SmartLeave.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        // ─── DbSet for each table ─────────────────────────────────────────────────────────

        public DbSet<User> Users { get; set; }                   // Identity’s AspNetUsers
        public DbSet<Country> Countries { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
        public DbSet<PublicHoliday> PublicHolidays { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANT: let Identity configure its own tables first
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfiguration());
            //
            // ─── Country ↔ Office ↔ User ──────────────────────────────────────────────────────
            //

            // Country → Offices
            builder.Entity<Country>(entity =>
            {
                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                //if you want “delete country → also delete all its offices” in a single operation.
                entity.HasMany(c => c.Offices)
                      .WithOne(o => o.Country)
                      .HasForeignKey(o => o.CountryId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Users (if user has CountryId, set null when country deleted)
                entity.HasMany(c => c.Users)
                      .WithOne(u => u.Country)
                      .HasForeignKey(u => u.CountryId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
            // Office → Country, and Office → Users
            builder.Entity<Office>(entity =>
            {
                entity.Property(o => o.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasOne(o => o.Country)
                      .WithMany(c => c.Offices)
                      .HasForeignKey(o => o.CountryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(o => o.Users)
                      .WithOne(u => u.Office)
                      .HasForeignKey(u => u.OfficeId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
            // ─── Team ↔ User (Manager ↔ ManagedTeams) ────────────────────────────────────

            builder.Entity<Team>(entity =>
            {
                entity.HasOne(t => t.Manager)                 // Team.Manager → one User
                      .WithMany(u => u.ManagedTeams)          // inverse: User.ManagedTeams (ICollection<Team>)
                      .HasForeignKey(t => t.ManagerId)
                      .OnDelete(DeleteBehavior.Restrict);      // Cannot delete a User if they still manage teams.

                entity.HasMany(t => t.Members)                // Team.Members → many Users
                      .WithOne(u => u.Team)                    // inverse: User.Team
                      .HasForeignKey(u => u.TeamId)
                      .OnDelete(DeleteBehavior.Restrict);      // Cannot delete a Team if members exist.
            });

            // ─── User ↔ User (Manager ↔ DirectReports) ────────────────────────────────────

            builder.Entity<User>(entity =>
            {
                // If User.ManagerId is set, it points to another User.Id.
                entity.HasOne(u => u.Manager)                 // User.Manager → one User
                      .WithMany(u => u.DirectReports)         // inverse: User.DirectReports (ICollection<User>)
                      .HasForeignKey(u => u.ManagerId)
                      .OnDelete(DeleteBehavior.Restrict);     // Cannot delete a User if they still have direct reports.
            });

            //
            // ─── LeaveType ↔ Leave ↔ LeaveBalance ──────────────────────────────────────────────
            //

            builder.Entity<LeaveType>(entity =>
            {
                entity.Property(lt => lt.Name)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(lt => lt.AnnualQuota)
                      .HasColumnType("decimal(5,2)");

                // One LeaveType → many Leaves
                entity.HasMany(lt => lt.Leaves)
                      .WithOne(l => l.LeaveType)
                      .HasForeignKey(l => l.LeaveTypeId)
                      .OnDelete(DeleteBehavior.Restrict);

                // One LeaveType → many LeaveBalances
                entity.HasMany(lt => lt.LeaveBalances)
                      .WithOne(lb => lb.LeaveType)
                      .HasForeignKey(lb => lb.LeaveTypeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Leave → Employee (User), ApprovedBy (User), LeaveType
            builder.Entity<Leave>(entity =>
            {
                entity.Property(l => l.StartDate)
                      .IsRequired();

                entity.Property(l => l.EndDate)
                      .IsRequired();

                // CHECK constraint: StartDate <= EndDate
                entity.HasCheckConstraint("CK_Leave_StartBeforeEnd", "[StartDate] <= [EndDate]");

                entity.Property(l => l.TotalDays)
                      .HasColumnType("decimal(5,2)");

                entity.Property(l => l.Comment)
                      .HasMaxLength(500);

                entity.Property(l => l.ApprovalNote)
                      .HasMaxLength(500);

                entity.Property(l => l.AttachmentUrl)
                      .HasMaxLength(255);

                // EmployeeId (FK) → User.Id, cannot delete a User if they have any Leave records
                entity.HasOne(l => l.Employee)
                      .WithMany(u => u.LeavesApplied)
                      .HasForeignKey(l => l.EmployeeId)
                      .OnDelete(DeleteBehavior.Restrict);

                // ApprovedById (FK) → User.Id, cannot delete an approver if they’ve approved any leaves
                entity.HasOne(l => l.ApprovedBy)
                      .WithMany(u => u.LeavesApproved)
                      .HasForeignKey(l => l.ApprovedById)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // LeaveBalance → Employee (User), LeaveType, Year, Unique Index
            builder.Entity<LeaveBalance>(entity =>
            {
                entity.Property(lb => lb.Year)
                      .IsRequired();

                entity.Property(lb => lb.UsedDays)
                      .HasColumnType("decimal(5,2)");

                entity.Property(lb => lb.RemainingDays)
                      .HasColumnType("decimal(5,2)");

                // One LeaveBalance → one User (Employee)
                entity.HasOne(lb => lb.Employee)
                      .WithMany(u => u.LeaveBalances) // you may need to add this nav in User
                      .HasForeignKey(lb => lb.EmployeeId)
                      .OnDelete(DeleteBehavior.Restrict);

                // One LeaveBalance → one LeaveType
                entity.HasOne(lb => lb.LeaveType)
                      .WithMany(lt => lt.LeaveBalances)
                      .HasForeignKey(lb => lb.LeaveTypeId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Unique index on (EmployeeId, LeaveTypeId, Year)
                entity.HasIndex(lb => new { lb.EmployeeId, lb.LeaveTypeId, lb.Year })
                      .IsUnique();
            });

            //
            // ─── PublicHoliday ────────────────────────────────────────────────────────────────
            //

            builder.Entity<PublicHoliday>(entity =>
            {
                entity.Property(ph => ph.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                // Ensure only one holiday per date
                entity.HasIndex(ph => ph.HolidayDate)
                      .IsUnique();

                entity.Property(ph => ph.Description)
                      .HasMaxLength(200);
            });


            //
            // ─── Event ↔ User (CreatedBy) ──────────────────────────────────────────────────────
            //

            builder.Entity<Event>(entity =>
            {
                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Description)
                      .HasMaxLength(500);

                // Creator → User, cannot delete User if they have created Events
                entity.HasOne(e => e.CreatedBy)
                      .WithMany(u => u.EventsCreated)
                      .HasForeignKey(e => e.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict);

                // Index on EventDate for fast date‐range queries
                entity.HasIndex(e => e.EventDate)
                      .HasDatabaseName("IX_Events_EventDate");
            });

            //
            // ─── AuditLog ↔ User ──────────────────────────────────────────────────────────────
            //

            builder.Entity<AuditLog>(entity =>
            {
                entity.Property(a => a.ActionType)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(a => a.TargetTable)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(a => a.Notes)
                      .HasMaxLength(500);

                // AuditLog → User (who performed the action)
                entity.HasOne(a => a.User)
                      .WithMany(u => u.AuditLogsCreated)
                      .HasForeignKey(a => a.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Composite index on (TargetTable, TargetId)
                entity.HasIndex(a => new { a.TargetTable, a.TargetId })
                      .HasDatabaseName("IX_AuditLog_Target");
            });
        }
    }
}
