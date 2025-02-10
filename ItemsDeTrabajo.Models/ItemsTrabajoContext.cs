using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ItemsDeTrabajo.Models;

public partial class ItemsTrabajoContext : DbContext
{
    public ItemsTrabajoContext()
    {
    }

    public ItemsTrabajoContext(DbContextOptions<ItemsTrabajoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Itemstrabajo> Itemstrabajos { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseNpgsql("Host=localhost;Database=ITEMS_TRABAJO;Username=postgres;Password=goia.2024");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum("relevancia_enum", new[] { "alta", "baja" });

        modelBuilder.Entity<Itemstrabajo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("itemstrabajo_pkey");

            entity.ToTable("itemstrabajo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Fechaentrega)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fechaentrega");
            entity.Property(e => e.Relevancia)
                .HasDefaultValueSql("'baja'::text")
                .HasColumnName("relevancia");
            entity.Property(e => e.Updatedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");
            entity.Property(e => e.Usuarioasignado).HasColumnName("usuarioasignado");
        });

        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
