using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FBapiService.DataDB;

public partial class BanticfintechContext : DbContext
{
    public BanticfintechContext()
    {
    }

    public BanticfintechContext(DbContextOptions<BanticfintechContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<ControlLogin> ControlLogins { get; set; }

    public virtual DbSet<CustomerBank> CustomerBanks { get; set; }

    public virtual DbSet<ManCustomer> ManCustomers { get; set; }

    public virtual DbSet<ManageQr> ManageQrs { get; set; }

    public virtual DbSet<ManageToken> ManageTokens { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<UserDatum> UserData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=banticfintech.database.windows.net;Database=banticfintech;User ID=bfadmin;Password=Password123*;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bank>(entity =>
        {
            entity.ToTable("Bank");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("accountID");
            entity.Property(e => e.AuthorizationId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("authorizationID");
            entity.Property(e => e.Bank1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("bank");
            entity.Property(e => e.CodBank)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("codBank");
            entity.Property(e => e.CreateDate)
                .HasColumnType("date")
                .HasColumnName("createDate");
            entity.Property(e => e.CreateUser)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("createUser");
            entity.Property(e => e.Describe)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("describe");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("type");
        });

        modelBuilder.Entity<ControlLogin>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ControlLogin");

            entity.Property(e => e.AutorizacionBanco)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("autorizacionBanco");
            entity.Property(e => e.Bank)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("bank");
            entity.Property(e => e.CodBank)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("codBank");
            entity.Property(e => e.CreacionBanco)
                .HasColumnType("date")
                .HasColumnName("creacionBanco");
            entity.Property(e => e.CreacionCustomer)
                .HasColumnType("date")
                .HasColumnName("creacionCustomer");
            entity.Property(e => e.CuentaBanco)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cuentaBanco");
            entity.Property(e => e.DescripcionBanco)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("descripcionBanco");
            entity.Property(e => e.EstadoBanco)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("estadoBanco");
            entity.Property(e => e.EstadoCustomer)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("estadoCustomer");
            entity.Property(e => e.ExpirationTime)
                .HasColumnType("date")
                .HasColumnName("expirationTime");
            entity.Property(e => e.IdBanco).HasColumnName("idBanco");
            entity.Property(e => e.IdCustomer).HasColumnName("idCustomer");
            entity.Property(e => e.IdRelacion).HasColumnName("idRelacion");
            entity.Property(e => e.NombCustomer)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombCustomer");
            entity.Property(e => e.TipoBanco)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tipoBanco");
            entity.Property(e => e.TipoCustomer)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TipoUsuario)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tipoUsuario");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userName");
            entity.Property(e => e.UsuarioCustomer)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("usuarioCustomer");
        });

        modelBuilder.Entity<CustomerBank>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.FkCustomer, e.FkBank });

            entity.ToTable("customerBank");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.FkCustomer).HasColumnName("fkCustomer");
            entity.Property(e => e.FkBank).HasColumnName("fkBank");
            entity.Property(e => e.CodBank).HasColumnName("codBank");
            entity.Property(e => e.CreateDate)
                .HasColumnType("date")
                .HasColumnName("createDate");
            entity.Property(e => e.CreateUser)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("createUser");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("status");

            entity.HasOne(d => d.FkBankNavigation).WithMany(p => p.CustomerBanks)
                .HasForeignKey(d => d.FkBank)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__customerB__fkBan__70DDC3D8");

            entity.HasOne(d => d.FkCustomerNavigation).WithMany(p => p.CustomerBanks)
                .HasForeignKey(d => d.FkCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__customerB__fkCus__71D1E811");
        });

        modelBuilder.Entity<ManCustomer>(entity =>
        {
            entity.ToTable("manCustomer");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("date")
                .HasColumnName("createDate");
            entity.Property(e => e.CreateUser)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("createUser");
            entity.Property(e => e.Describe)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("describe");
            entity.Property(e => e.Logo).HasColumnName("logo");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("type");
        });

        modelBuilder.Entity<ManageQr>(entity =>
        {
            entity.ToTable("manageQR");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Additionaldata)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("additionaldata");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("amount");
            entity.Property(e => e.CodTransaction)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codTransaction");
            entity.Property(e => e.CodigoQr)
                .IsUnicode(false)
                .HasColumnName("codigoQR");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("currency");
            entity.Property(e => e.Daterequest)
                .HasColumnType("datetime")
                .HasColumnName("daterequest");
            entity.Property(e => e.Datesend)
                .HasColumnType("datetime")
                .HasColumnName("datesend");
            entity.Property(e => e.Destinationaccountid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("destinationaccountid");
            entity.Property(e => e.Expirationdate)
                .HasColumnType("date")
                .HasColumnName("expirationdate");
            entity.Property(e => e.FkBank).HasColumnName("fkBank");
            entity.Property(e => e.FkCustomer).HasColumnName("fkCustomer");
            entity.Property(e => e.Gloss)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("gloss");
            entity.Property(e => e.IdQr)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idQR");
            entity.Property(e => e.Jsoninput)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("jsoninput");
            entity.Property(e => e.Jsonoutput)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("jsonoutput");
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("message");
            entity.Property(e => e.Singleuse)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("singleuse");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("status");
            entity.Property(e => e.Success)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("success");
            entity.Property(e => e.TypeRequest)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("typeRequest");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userName");
            entity.Property(e => e.VoucherId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("voucherId");
        });

        modelBuilder.Entity<ManageToken>(entity =>
        {
            entity.ToTable("manageToken");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("date")
                .HasColumnName("createDate");
            entity.Property(e => e.CreateUser)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("createUser");
            entity.Property(e => e.ExpirationTime)
                .HasColumnType("date")
                .HasColumnName("expirationTime");
            entity.Property(e => e.FkCustomer).HasColumnName("fkCustomer");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("type");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userName");

            entity.HasOne(d => d.FkCustomerNavigation).WithMany(p => p.ManageTokens)
                .HasForeignKey(d => d.FkCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__manageTok__fkCus__72C60C4A");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.IdQr });

            entity.ToTable("Notification");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.IdQr)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idQR");
            entity.Property(e => e.Additionaldata)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("additionaldata");
            entity.Property(e => e.CreateDate)
                .HasColumnType("date")
                .HasColumnName("createDate");
            entity.Property(e => e.CreateUser)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("createUser");
            entity.Property(e => e.Gloss)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("gloss");
            entity.Property(e => e.OriginName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("originName");
            entity.Property(e => e.SouceCodBank)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("souceCodBank");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("status");
            entity.Property(e => e.TransactionDateTime)
                .HasColumnType("date")
                .HasColumnName("transactionDateTime");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("type");
            entity.Property(e => e.VoucherId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("voucherId");
        });

        modelBuilder.Entity<UserDatum>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("UserData");

            entity.Property(e => e.Bank)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("bank");
            entity.Property(e => e.ClaveUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("claveUser");
            entity.Property(e => e.CodBank)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("codBank");
            entity.Property(e => e.Customer)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IdBank).HasColumnName("idBank");
            entity.Property(e => e.IdCustomer).HasColumnName("idCustomer");
            entity.Property(e => e.NameUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nameUser");
            entity.Property(e => e.TypeUser)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("typeUser");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
