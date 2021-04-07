using System;
using Microsoft.EntityFrameworkCore;
using Shambala.Domain;

namespace Shambala.Infrastructure
{
    public partial class ShambalaContext : DbContext
    {
        public ShambalaContext()
        {
        }

        public ShambalaContext(DbContextOptions<ShambalaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CaretDetail> CaretDetail { get; set; }
        public virtual DbSet<Flavour> Flavour { get; set; }
        public virtual DbSet<IncomingShipment> IncomingShipment { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<OutgoingShipment> OutgoingShipment { get; set; }
        public virtual DbSet<OutgoingShipmentDetail> OutgoingShipmentDetails { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductFlavourQuantity> ProductFlavourQuantity { get; set; }
        public virtual DbSet<Salesman> Salesman { get; set; }
        public virtual DbSet<Scheme> Scheme { get; set; }
        public virtual DbSet<Shop> Shop { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("Server=localhost;Port=3306;Database=shambala;Uid=root;Pwd=sham@12DATA;SslMode=None");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CaretDetail>(entity =>
            {
                entity.ToTable("caret_detail");

                entity.HasIndex(e => e.ProductIdFk)
                    .HasName("Product_Caret_Relationship_idx");

                entity.Property(e => e.Id).HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.CaretPrice).HasColumnType("decimal(6,2)");

                entity.Property(e => e.CaretSize).HasColumnType("tinyint(4)");

                entity.Property(e => e.Gstrate)
                    .HasColumnName("GSTRate")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.ProductIdFk)
                    .HasColumnName("Product_Id_FK")
                    .HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.ProductIdFkNavigation)
                    .WithMany(p => p.CaretDetail)
                    .HasForeignKey(d => d.ProductIdFk)
                    .HasConstraintName("Product_Caret_Relationship");
            });

            modelBuilder.Entity<Flavour>(entity =>
            {
                entity.ToTable("flavour");

                entity.HasIndex(e => e.Id)
                    .HasName("Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("tinyint(4) unsigned")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Title).HasMaxLength(20);
            });

            modelBuilder.Entity<IncomingShipment>(entity =>
            {
                entity.ToTable("incoming_shipment");

                entity.HasIndex(e => e.Id)
                    .HasName("Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.ProductIdFk)
                    .HasName("Incoming_shipment_Product_Relationship_idx");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.CaretSize).HasColumnType("tinyint(4)");

                entity.Property(e => e.ProductIdFk)
                    .HasColumnName("Product_Id_FK")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.TotalDefectPieces).HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.TotalRecievedPieces).HasColumnType("smallint(5) unsigned");

            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("invoice");

                entity.HasIndex(e => e.FlavourIdFk)
                    .HasName("Flavour_Invoice_Relationship_idx");

                entity.HasIndex(e => e.Id)
                    .HasName("Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.OutgoingShipmentIdFk)
                    .HasName("Outgoing_Shipment_Invoice_Relationship_idx");

                entity.HasIndex(e => e.ProductIdFk)
                    .HasName("Product_Invoice_Relationship_idx");

                entity.HasIndex(e => e.SchemeIdFk)
                    .HasName("Scheme_Invoice_Relationship_idx");

                entity.HasIndex(e => e.ShopIdFk)
                    .HasName("Shop_Invoice_Relationship_idx");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.CaretSize).HasColumnType("tinyint(4)");

                entity.Property(e => e.CostPrice).HasColumnType("decimal(8,2)");

                entity.Property(e => e.FlavourIdFk)
                    .HasColumnName("Flavour_Id_FK")
                    .HasColumnType("tinyint(4) unsigned");

                entity.Property(e => e.Gstrate)
                    .HasColumnName("GSTRate")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.OutgoingShipmentIdFk)
                    .HasColumnName("Outgoing_Shipment_Id_FK")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.ProductIdFk)
                    .HasColumnName("Product_Id_FK")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.QuantityDefected).HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.QuantityPurchase).HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.SchemeIdFk)
                    .HasColumnName("Scheme_Id_FK")
                    .HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.SellingPrice).HasColumnType("decimal(8,2)");

                entity.Property(e => e.ShopIdFk)
                    .HasColumnName("Shop_Id_FK")
                    .HasColumnType("smallint(5) unsigned");

                entity.HasOne(d => d.FlavourIdFkNavigation)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.FlavourIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Flavour_Invoice_Relationship");

                entity.HasOne(d => d.OutgoingShipmentIdFkNavigation)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.OutgoingShipmentIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Outgoing_Shipment_Invoice_Relationship");

                entity.HasOne(d => d.ProductIdFkNavigation)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.ProductIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Product_Invoice_Relationship");

                entity.HasOne(d => d.SchemeIdFkNavigation)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.SchemeIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Scheme_Invoice_Relationship");

                entity.HasOne(d => d.ShopIdFkNavigation)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.ShopIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Shop_Invoice_Relationship");
            });

            modelBuilder.Entity<OutgoingShipment>(entity =>
            {
                entity.ToTable("outgoing_shipment");

                entity.HasIndex(e => e.SalesmanIdFk)
                    .HasName("OUtgoingShipment_Salesman_Relationship_idx");
                
                entity.Property(e => e.Id).HasColumnType("int(11) unsigned");
                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.SalesmanIdFk)
                    .HasColumnName("Salesman_id_FK")
                    .HasColumnType("smallint(5) unsigned");

                entity.HasOne(d => d.SalesmanIdFkNavigation)
                    .WithMany(p => p.OutgoingShipment)
                    .HasForeignKey(d => d.SalesmanIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OUtgoingShipment_Salesman_Relationship");
            });

            modelBuilder.Entity<OutgoingShipmentDetail>(entity =>
            {
                entity.ToTable("outgoing_shipment_details");

                entity.HasIndex(e => e.FlavourIdFk)
                    .HasName("Outgoing_Shipment_Details_Flavour_Relationship_idx");
entity.HasIndex(e => e.OutgoingShipmentIdFk)
                    .HasName("Outgoing_Shipment_Details_OutgoingShipment_Relationship_idx");

                entity.HasIndex(e => e.Id)
                    .HasName("Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.ProductIdFk)
                    .HasName("Outgoing_Shipment_Details_Product_RelationShip_idx");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.CaretSize).HasColumnType("tinyint(2)");

                entity.Property(e => e.FlavourIdFk)
                    .HasColumnName("Flavour_Id_FK")
                    .HasColumnType("tinyint(4) unsigned");

                entity.Property(e => e.OutgoingShipmentIdFk)
                    .HasColumnName("Outgoing_Shipment_Id_FK")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.ProductIdFk)
                    .HasColumnName("Product_Id_FK")
                    .HasColumnType("int(11) unsigned");


                entity.Property(e => e.TotalQuantityRejected)
                    .HasColumnName("Total_Quantity_Rejected")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.TotalQuantityShiped)
                    .HasColumnName("Total_Quantity_Shiped")
                    .HasColumnType("smallint(5) unsigned");

                entity.HasOne(d => d.FlavourIdFkNavigation)
                    .WithMany(p => p.OutgoingShipmentDetails)
                    .HasForeignKey(d => d.FlavourIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Outgoing_Shipment_Details_Flavour_Relationship");

                entity.HasOne(d => d.OutgoingShipmentIdFkNavigation)
                    .WithMany(p => p.OutgoingShipmentDetails)
                    .HasForeignKey(d => d.OutgoingShipmentIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Outgoing_Shipment_Details_Product_Relationship");

                entity.HasOne(d => d.ProductIdFkNavigation)
                    .WithMany(p => p.OutgoingShipmentDetails)
                    .HasForeignKey(d => d.ProductIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Outgoing_Shipment_Details_Product_Relationship");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.HasIndex(e => e.Id)
                    .HasName("Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<ProductFlavourQuantity>(entity =>
            {
                entity.ToTable("product_flavour_quantity");

                entity.HasIndex(e => e.FlavourIdFk)
                    .HasName("Flavour_Id_FK_idx");

                entity.HasIndex(e => e.ProductIdFk)
                    .HasName("Product_Relationship_idx");

                entity.Property(e => e.Id).HasColumnType("tinyint(4)");

                entity.Property(e => e.FlavourIdFk)
                    .HasColumnName("Flavour_Id_FK")
                    .HasColumnType("tinyint(4) unsigned");

                entity.Property(e => e.ProductIdFk)
                    .HasColumnName("Product_Id_FK")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Quantity).HasColumnType("smallint(6) unsigned");

                entity.HasOne(d => d.FlavourIdFkNavigation)
                    .WithMany(p => p.ProductFlavourQuantity)
                    .HasForeignKey(d => d.FlavourIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("flavour_Relationship");

                entity.HasOne(d => d.ProductIdFkNavigation)
                    .WithMany(p => p.ProductFlavourQuantity)
                    .HasForeignKey(d => d.ProductIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Product_Relationship");
            });

            modelBuilder.Entity<Salesman>(entity =>
            {
                entity.ToTable("salesman");

                entity.HasIndex(e => e.Id)
                    .HasName("Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(60);
            });

            modelBuilder.Entity<Scheme>(entity =>
            {
                entity.ToTable("scheme");

                entity.HasIndex(e => e.Id)
                    .HasName("Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.IsUserDefinedScheme).HasColumnType("bit(1)");

                entity.Property(e => e.SchemeType).HasColumnType("tinyint(4)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.Value).HasColumnType("decimal(4,2)");
            });

            modelBuilder.Entity<Shop>(entity =>
            {
                entity.ToTable("shop");

                entity.HasIndex(e => e.Id)
                    .HasName("Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.SchemeIdFk)
                    .HasName("Scheme_Id_FK_idx");

                entity.Property(e => e.Id).HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.Address).HasMaxLength(80);

                entity.Property(e => e.SchemeIdFk)
                    .HasColumnName("Scheme_Id_FK")
                    .HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.HasOne(d => d.SchemeIdFkNavigation)
                    .WithMany(p => p.Shop)
                    .HasForeignKey(d => d.SchemeIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Shop_Scheme_Relationship");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
