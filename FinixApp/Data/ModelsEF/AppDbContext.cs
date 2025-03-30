namespace backend.Data.ModelsEF
{
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<TInvoice> TInvoices { get; set; }
        public virtual DbSet<TProduct> TProducts { get; set; }
        public virtual DbSet<TCreditNote> TCreditNotes { get; set; }
        public virtual DbSet<TCustomer> TCustomers { get; set; }
        public virtual DbSet<TPayment> TPayments { get; set; }
        public virtual DbSet<TUser> TUsers { get; set; }
        public virtual DbSet<TRole> TRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TInvoice>(entity =>
            {
                entity.ToTable("t_invoice");

                entity.HasKey(e => e.InvoiceNumber);

                entity.Property(e => e.InvoiceDate).HasColumnName("invoice_date");
                entity.Property(e => e.TotalAmount).HasColumnName("total_amount");
                entity.Property(e => e.PaymentDueDate).HasColumnName("payment_due_date");

                entity.Property(e => e.InvoiceStatus)
                    .HasConversion<string>()
                    .HasColumnName("invoice_status");

                entity.Property(e => e.PaymentStatus)
                    .HasConversion<string>()
                    .HasColumnName("payment_status");

                entity.HasOne(e => e.TCustomer)
                    .WithMany(c => c.TInvoices)
                    .HasForeignKey(e => e.CustomerRun)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.InvoiceNumber).HasDatabaseName("idx_invoice_number_tInvoice");
                entity.HasIndex(e => e.InvoiceStatus).HasDatabaseName("idx_invoice_status_tInvoice");
                entity.HasIndex(e => e.PaymentStatus).HasDatabaseName("idx_payment_status_tInvoice");
                entity.HasIndex(e => e.InvoiceStatus).HasDatabaseName("idx_invoice_status_tInvoice");
                entity.HasIndex(e => e.PaymentStatus).HasDatabaseName("idx_payment_status_tInvoice");
                entity.HasIndex(e => new { e.InvoiceStatus, e.PaymentStatus }).HasDatabaseName("idx_invoice_and_payment_status_tInvoice");
                entity.HasIndex(e => e.CustomerRun).HasDatabaseName("idx_customer_run_tInvoice");

                entity.HasOne(e => e.TInvoicePayment)
                    .WithOne(p => p.TInvoice)
                    .HasForeignKey<TPayment>(p => p.InvoiceNumber)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TCustomer>(entity =>
            {
                entity.ToTable("t_customer");
                entity.HasKey(e => e.Run);

                entity.Property(e => e.Name).HasColumnName("customer_name");
                entity.Property(e => e.Email).HasColumnName("customer_email");
            });

            modelBuilder.Entity<TProduct>(entity =>
            {
                entity.ToTable("t_product");
                entity.HasKey(e => e.ProductNumber);

                entity.Property(e => e.ProductName).HasColumnName("product_name");
                entity.Property(e => e.UnitPrice).HasColumnName("unit_price");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.SubTotal).HasColumnName("subtotal");

                entity.HasIndex(e => e.InvoiceNumber).HasDatabaseName("idx_invoice_number_tProduct");

                entity.HasOne(p => p.TInvoice)
                    .WithMany(i => i.TInvoiceDetail)
                    .HasForeignKey(p => p.InvoiceNumber)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TCreditNote>(entity =>
            {
                entity.ToTable("t_credit_note");
                entity.HasKey(e => e.CreditNoteNumber);

                entity.Property(e => e.CreditNoteDate).HasColumnName("credit_note_date");
                entity.Property(e => e.CreditNoteAmount).HasColumnName("credit_note_amount");

                entity.HasIndex(e => e.InvoiceNumber).HasDatabaseName("idx_invoice_number_tCreditNote");


                entity.HasOne(cn => cn.TInvoice)
                    .WithMany(i => i.TInvoiceCreditNote)
                    .HasForeignKey(cn => cn.InvoiceNumber)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TPayment>(entity =>
            {
                entity.ToTable("t_payment");
                entity.HasKey(e => e.PaymentNumber);

                entity.HasIndex(e => e.InvoiceNumber).HasDatabaseName("idx_invoice_number_tPayment");

                entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");
                entity.Property(e => e.PaymentDate).HasColumnName("payment_date");
            });

            modelBuilder.Entity<TRole>(entity =>
            {
                entity.ToTable("t_role");

                entity.HasKey(r => r.RoleId);
                entity.Property(r => r.RoleId).HasColumnName("role_id");

                entity.Property(r => r.Name)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasColumnName("name");
            });

            modelBuilder.Entity<TUser>(entity =>
            {
                entity.ToTable("t_user");

                entity.HasKey(u => u.UserId);
                entity.Property(u => u.UserId)
                      .HasColumnName("user_id");


                entity.Property(u => u.Username)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.PasswordHash)
                      .HasColumnName("password_hash")
                      .IsRequired();

                entity.HasIndex(e => e.RoleId).HasDatabaseName("idx_role_id_tUser");


                entity.HasOne(u => u.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(u => u.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }

}

