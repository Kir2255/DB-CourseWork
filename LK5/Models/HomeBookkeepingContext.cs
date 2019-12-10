using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LK5.Models
{
    public partial class HomeBookkeepingContext : IdentityDbContext
    {
        public HomeBookkeepingContext()
        {
        }

        public HomeBookkeepingContext(DbContextOptions<HomeBookkeepingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ExpenseType> ExpenseTypes { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<FamilyMember> FamilyMembers { get; set; }
        public virtual DbSet<IncomeSource> IncomeSources { get; set; }
        public virtual DbSet<Income> Incomes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExpenseType>(entity =>
            {
                entity.HasKey(e => e.ExpenseTypeId)
                    .HasName("PK__ExpenseT__E082A36FCEFE8964");

                entity.Property(e => e.ExpenseTypeId).HasColumnName("ExpenseTypeID");

                entity.Property(e => e.Comment).HasMaxLength(100);

                entity.Property(e => e.ExpenseName).HasMaxLength(50);
            });

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.HasKey(e => e.ExpenseId)
                    .HasName("PK__Expenses__1445CFF3D8AB3E37");

                entity.Property(e => e.ExpenseId).HasColumnName("ExpenseID");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.ExpenseDate).HasColumnType("date");

                entity.Property(e => e.ExpenseTypeId).HasColumnName("ExpenseTypeID");

                entity.HasOne(d => d.ExpenseType)
                    .WithMany(p => p.Expenses)
                    .HasForeignKey(d => d.ExpenseTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Expenses_To_ExpenseTypes");
            });

            modelBuilder.Entity<FamilyMember>(entity =>
            {
                entity.HasKey(e => e.MemberId)
                    .HasName("PK__FamilyMe__0CF04B38B3290D33");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.Balance).HasColumnType("money");

                entity.Property(e => e.ExpenseId).HasColumnName("ExpenseID");

                entity.Property(e => e.Fio)
                    .HasColumnName("FIO")
                    .HasMaxLength(100);

                entity.Property(e => e.IncomeId).HasColumnName("IncomeID");

                entity.Property(e => e.Phone)
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Sex)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Expense)
                    .WithMany(p => p.FamilyMembers)
                    .HasForeignKey(d => d.ExpenseId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_FamilyMembers_To_Expenses");

                entity.HasOne(d => d.Income)
                    .WithMany(p => p.FamilyMembers)
                    .HasForeignKey(d => d.IncomeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_FamilyMembers_To_Incomes");
            });

            modelBuilder.Entity<IncomeSource>(entity =>
            {
                entity.HasKey(e => e.IncomeSourceId)
                    .HasName("PK__IncomeSo__8159D8668396E399");

                entity.Property(e => e.IncomeSourceId).HasColumnName("IncomeSourceID");

                entity.Property(e => e.Comment).HasMaxLength(100);

                entity.Property(e => e.IncomeName).HasMaxLength(50);
            });

            modelBuilder.Entity<Income>(entity =>
            {
                entity.HasKey(e => e.IncomeId)
                    .HasName("PK__Incomes__60DFC66CC12AB4FA");

                entity.Property(e => e.IncomeId).HasColumnName("IncomeID");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.IncomeDate).HasColumnType("date");

                entity.Property(e => e.IncomeSourceId).HasColumnName("IncomeSourceID");

                entity.HasOne(d => d.IncomeSource)
                    .WithMany(p => p.Incomes)
                    .HasForeignKey(d => d.IncomeSourceId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Incomes_To_IncomeSources");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
