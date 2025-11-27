using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace MoviesAPI.DB;

public partial class MoviesMauiContext : DbContext
{
    public MoviesMauiContext()
    {
    }

    public MoviesMauiContext(DbContextOptions<MoviesMauiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Studio> Studios { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("userid=student;password=student;server=192.168.200.13;database=MoviesMAUI", ServerVersion.Parse("10.3.39-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Movie");

            entity.HasIndex(e => e.StudioId, "FK_Movie_StudioId");

            entity.HasIndex(e => e.UserId, "FK_Movie_UserId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Genres).HasMaxLength(255);
            entity.Property(e => e.ImageUrl).HasColumnType("mediumblob");
            entity.Property(e => e.Rating).HasColumnType("int(11)");
            entity.Property(e => e.StudioId).HasColumnType("int(11)");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.Studio).WithMany(p => p.Movies)
                .HasForeignKey(d => d.StudioId)
                .HasConstraintName("FK_Movie_StudioId");

            entity.HasOne(d => d.User).WithMany(p => p.Movies)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Movie_UserId");
        });

        modelBuilder.Entity<Studio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Studio");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.DirectorName).HasMaxLength(255);
            entity.Property(e => e.DirectorPatronymic).HasMaxLength(255);
            entity.Property(e => e.DirectorSurname).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Rating).HasColumnType("int(11)");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
