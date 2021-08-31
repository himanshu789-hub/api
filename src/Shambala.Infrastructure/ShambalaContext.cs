using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Shambala.Domain;
// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

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

        public virtual DbSet<Credit> Credit { get; set; }
        public virtual DbSet<CustomCaratPrice> CustomCaratPrice { get; set; }
        public virtual DbSet<Debit> Debit { get; set; }
        public virtual DbSet<Flavour> Flavour { get; set; }
        public virtual DbSet<IncomingShipment> IncomingShipment { get; set; }
        public virtual DbSet<OutgoingShipment> OutgoingShipment { get; set; }
        public virtual DbSet<OutgoingShipmentDetails> OutgoingShipmentDetails { get; set; }
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
                optionsBuilder.UseMySQL("Server=localhost;Port=3306;Database=shambala;Uid=root;Pwd=mysql@90dev;SslMode=None");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Credit>(entity =>
            {
                entity.ToTable("credit");

                entity.HasIndex(e => e.ShopIdFk)
                    .HasName("Credit_Shop_Relationship_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Amount).HasColumnType("decimal(6,2)");

                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.OutgoingShipmentIdFk)
                    .HasColumnName("OutgoingShipment_Id_FK")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.ShopIdFk)
                    .HasColumnName("Shop_Id_FK")
                    .HasColumnType("smallint(5) unsigned");

                entity.HasOne(d => d.ShopIdFkNavigation)
                    .WithMany(p => p.Credit)
                    .HasForeignKey(d => d.ShopIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Credit_Shop_Relationships");
            });

            modelBuilder.Entity<CustomCaratPrice>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("custom_carat_price");

                entity.HasIndex(e => e.OutgoinShipmentDetailIdFk)
                    .HasName("OutgoingShipment_CustomPrice_Relationship_idx");

                entity.Property(e => e.OutgoinShipmentDetailIdFk)
                    .HasColumnName("OutgoinShipmentDetail_Id_FK")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.PricePerCarat).HasColumnType("decimal(6,2)");

                entity.Property(e => e.Quantity).HasColumnType("smallint(5) unsigned");

                entity.HasOne(d => d.OutgoinShipmentDetailIdFkNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.OutgoinShipmentDetailIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OutgoingShipment_CustomPrice_Relationship");
            });

            modelBuilder.Entity<Debit>(entity =>
            {
                entity.ToTable("debit");

                entity.HasIndex(e => e.OutgoingShipmentIdFk)
                    .HasName("Credit_OutgoingShipment_Relationship_idx");

                entity.HasIndex(e => e.ShopIdFk)
                    .HasName("Credit_Shop_Relationship_idx");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Amount).HasColumnType("decimal(6,2)");

                entity.Property(e => e.DateRecieved).HasColumnType("date");

                entity.Property(e => e.OutgoingShipmentIdFk)
                    .HasColumnName("OutgoingShipment_Id_FK")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.ShopIdFk)
                    .HasColumnName("Shop_Id_FK")
                    .HasColumnType("smallint(5) unsigned");

                entity.HasOne(d => d.OutgoingShipmentIdFkNavigation)
                    .WithMany(p => p.Debit)
                    .HasForeignKey(d => d.OutgoingShipmentIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Credit_OutgoingShipment_Relationship");

                entity.HasOne(d => d.ShopIdFkNavigation)
                    .WithMany(p => p.Debit)
                    .HasForeignKey(d => d.ShopIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Credit_Shop_Relationship");
            });

            modelBuilder.Entity<Flavour>(entity =>
            {
                entity.ToTable("flavour");

                entity.HasIndex(e => e.Id)
                    .HasName("Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("tinyint(3) unsigned")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Title).HasMaxLength(20);
            });

            modelBuilder.Entity<IncomingShipment>(entity =>
            {
                entity.ToTable("incoming_shipment");

                entity.HasIndex(e => e.FlavourIdFk)
                    .HasName("IncmingShipment_Flavour_idx");

                entity.HasIndex(e => e.Id)
                    .HasName("Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.ProductIdFk)
                    .HasName("IncomingShipment_Product_Relationship_idx");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.CaretSize).HasColumnType("tinyint(4)");

                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.FlavourIdFk)
                    .HasColumnName("Flavour_Id_FK")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.ProductIdFk)
                    .HasColumnName("Product_Id_FK")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.TotalDefectPieces).HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.TotalRecievedPieces).HasColumnType("smallint(5) unsigned");

                entity.HasOne(d => d.FlavourIdFkNavigation)
                    .WithMany(p => p.IncomingShipment)
                    .HasForeignKey(d => d.FlavourIdFk)
                    .HasConstraintName("IncmingShipment_Flavour");

                entity.HasOne(d => d.ProductIdFkNavigation)
                    .WithMany(p => p.IncomingShipment)
                    .HasForeignKey(d => d.ProductIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IncomingShipment_Product_Relationship");
            });

            modelBuilder.Entity<OutgoingShipment>(entity =>
            {
                entity.ToTable("outgoing_shipment");

                entity.HasIndex(e => e.SalesmanIdFk)
                    .HasName("OUtgoingShipment_Salesman_Relationship_idx");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.SalesmanIdFk)
                    .HasColumnName("Salesman_id_FK")
                    .HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.SalesmanIdFkNavigation)
                    .WithMany(p => p.OutgoingShipment)
                    .HasForeignKey(d => d.SalesmanIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OUtgoingShipment_Salesman_Relationship");
            });

            modelBuilder.Entity<OutgoingShipmentDetails>(entity =>
            {
                entity.ToTable("outgoing_shipment_details");

                entity.HasIndex(e => e.FlavourIdFk)
                    .HasName("Outgoing_Shipment_Details_Flavour_Relationship_idx");

                entity.HasIndex(e => e.Id)
                    .HasName("Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.OutgoingShipmentIdFk)
                    .HasName("Outgoing_Shipment_Details_OutgoingShipment_Relationship_idx");

                entity.HasIndex(e => e.ProductIdFk)
                    .HasName("Outgoing_Shipment_Details_Product_RelationShip_idx");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.CaretSize).HasColumnType("tinyint(4)");

                entity.Property(e => e.FlavourIdFk)
                    .HasColumnName("Flavour_Id_FK")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.OutgoingShipmentIdFk)
                    .HasColumnName("Outgoing_Shipment_Id_FK")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.PricePerCarat)
                    .HasColumnName("Price_Per_Carat")
                    .HasColumnType("decimal(6,2)");

                entity.Property(e => e.ProductIdFk)
                    .HasColumnName("Product_Id_FK")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.SchemeTotalPrice)
                    .HasColumnName("Scheme_Total_Price")
                    .HasColumnType("decimal(6,2)");

                entity.Property(e => e.SchemeTotalQuantity)
                    .HasColumnName("Scheme_Total_Quantity")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.TotalQuantityRejected)
                    .HasColumnName("Total_Quantity_Rejected")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.TotalQuantityReturned)
                    .HasColumnName("Total_Quantity_Returned")
                    .HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.TotalQuantityShiped)
                    .HasColumnName("Total_Quantity_Shiped")
                    .HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.TotalQuantityTaken)
                    .HasColumnName("Total_Quantity_Taken")
                    .HasColumnType("smallint(6)");

                entity.HasOne(d => d.FlavourIdFkNavigation)
                    .WithMany(p => p.OutgoingShipmentDetails)
                    .HasForeignKey(d => d.FlavourIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Outgoing_Shipment_Details_Flavour_Relationship");

                entity.HasOne(d => d.OutgoingShipmentIdFkNavigation)
                    .WithMany(p => p.OutgoingShipmentDetails)
                    .HasForeignKey(d => d.OutgoingShipmentIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Outgoing_Shipment_Details_OutgoingShipment_Relationship");

                entity.HasOne(d => d.ProductIdFkNavigation)
                    .WithMany(p => p.OutgoingShipmentDetails)
                    .HasForeignKey(d => d.ProductIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Outgoing_Shipment_Details_Product_RelationShip");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.HasIndex(e => e.Id)
                    .HasName("Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.CaretSize).HasColumnType("tinyint(4)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.PricePerCaret).HasColumnType("decimal(6,2)");

                entity.Property(e => e.SchemeQuantity).HasColumnType("tinyint(4)");
            });

            modelBuilder.Entity<ProductFlavourQuantity>(entity =>
            {
                entity.ToTable("product_flavour_quantity");

                entity.HasIndex(e => e.FlavourIdFk)
                    .HasName("Flavour_Id_FK_idx");

                entity.HasIndex(e => e.ProductIdFk)
                    .HasName("Product_Relationship_idx");

                entity.Property(e => e.Id)
                    .HasColumnType("tinyint(4)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FlavourIdFk)
                    .HasColumnName("Flavour_Id_FK")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.ProductIdFk)
                    .HasColumnName("Product_Id_FK")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Quantity).HasColumnType("smallint(5) unsigned");

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

                entity.HasIndex(e => e.ProductIdFk)
                    .HasName("Scheme_Product_Relationship_idx");

                entity.Property(e => e.Id).HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.ProductIdFk)
                    .HasColumnName("Product_Id_FK")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Quantity).HasColumnType("tinyint(4)");

                entity.HasOne(d => d.ProductIdFkNavigation)
                    .WithMany(p => p.Scheme)
                    .HasForeignKey(d => d.ProductIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Scheme_Product_Relationship");
            });

            modelBuilder.Entity<Shop>(entity =>
            {
                entity.ToTable("shop");

                entity.HasIndex(e => e.Id)
                    .HasName("Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.Address).HasMaxLength(80);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(40);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
