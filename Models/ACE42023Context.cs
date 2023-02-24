using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PunchPad.Models
{
    public partial class ACE42023Context : DbContext
    {
        public ACE42023Context()
        {
        }

        public ACE42023Context(DbContextOptions<ACE42023Context> options)
            : base(options)
        {
        }

        public virtual DbSet<PremAttendance> PremAttendances { get; set; } = null!;
        public virtual DbSet<PremEmployee> PremEmployees { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DEVSQL.corp.local;Database=ACE 4- 2023;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PremAttendance>(entity =>
            {
                entity.ToTable("PremAttendance");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AttendanceDay)
                    .HasColumnType("date")
                    .HasColumnName("attendanceDay");

                entity.Property(e => e.AttendanceStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("attendanceStatus");

                entity.Property(e => e.AttendanceTime)
                    .HasColumnType("datetime")
                    .HasColumnName("attendanceTime");

                entity.Property(e => e.AttendanceType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("attendanceType");

                entity.Property(e => e.EmpId).HasColumnName("empId");

                entity.Property(e => e.ManagerId).HasColumnName("managerId");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.PremAttendanceEmps)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("FK__PremAtten__empId__1209AD79");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.PremAttendanceManagers)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("FK__PremAtten__manag__11158940");
            });

            modelBuilder.Entity<PremEmployee>(entity =>
            {
                entity.ToTable("PremEmployee");

                entity.HasIndex(e => e.Username, "UQ__PremEmpl__F3DBC5720E65AE40")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Department)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("department");

                entity.Property(e => e.Doj)
                    .HasColumnType("date")
                    .HasColumnName("doj");

                entity.Property(e => e.ManagerId).HasColumnName("managerId");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.InverseManager)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("FK__PremEmplo__manag__0E391C95");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
