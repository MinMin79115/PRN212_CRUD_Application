using System;
using System.Collections.Generic;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public partial class EplmanagementContext : DbContext
{
    public EplmanagementContext()
    {
    }

    public EplmanagementContext(DbContextOptions<EplmanagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FootballTeam> FootballTeams { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Server=(local);uid=sa;pwd=12345;database= EPLManagement;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FootballTeam>(entity =>
        {
            entity.HasKey(e => e.FootballTeamId).HasName("PK__Football__B287F27BEA9DEC5E");

            entity.ToTable("FootballTeam");

            entity.Property(e => e.FootballTeamId).HasColumnName("FootballTeamID");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Stadium).HasMaxLength(100);
            entity.Property(e => e.TeamName).HasMaxLength(100);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PK__Player__4A4E74A86C501B6D");

            entity.ToTable("Player");

            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.FootballTeamId).HasColumnName("FootballTeamID");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Nationality).HasMaxLength(50);
            entity.Property(e => e.Position).HasMaxLength(50);

            entity.HasOne(d => d.FootballTeam).WithMany(p => p.Players)
                .HasForeignKey(d => d.FootballTeamId)
                .HasConstraintName("FK__Player__Football__398D8EEE");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCACAB611E5B");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ__User__536C85E4455D8CAD").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.FootballTeamId).HasColumnName("FootballTeamID");
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.FootballTeam).WithMany(p => p.Users)
                .HasForeignKey(d => d.FootballTeamId)
                .HasConstraintName("FK_User_FootballTeam");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
