using BugTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BugTracker.DataAccess
{
    public class BugTrackerContext : DbContext
    {
        public BugTrackerContext(DbContextOptions<BugTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Project>()
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (ProjectStatus)Enum.Parse(typeof(ProjectStatus), v))
                .HasMaxLength(250)
                .IsRequired();

            modelBuilder
                .Entity<Project>()
                .Property(e => e.Name)
                .HasMaxLength(55)
                .IsRequired();

            modelBuilder
                .Entity<Requirement>()
                .ToTable("Requirements");

            modelBuilder
                .Entity<Requirement>()
                .Property(r => r.Description)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder
                .Entity<Requirement>()
                .Property(r => r.ProjectId)
                .IsRequired();

            modelBuilder
                .Entity<Requirement>()
                .Property(r => r.Status)
                .HasConversion(
                v => (int)v,
                v => (RequirementStatus)v)
                .IsRequired();
        }
    }
}