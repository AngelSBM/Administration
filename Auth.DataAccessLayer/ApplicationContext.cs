using Administration.DataAccessLayer.Entities;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DataAccessLayer
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        DbSet<Client> Clients { get; set; }
        DbSet<Address> ClientsAdresses { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("CLIENTS");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Name).HasColumnName("CLIENT_NAME");
                entity.Property(e => e.Email).HasColumnName("EMAIL");
                entity.Property(e => e.Phone).HasColumnName("PHONE_NUMBER");
                entity.Property(e => e.PublicId).HasColumnName("PUBLIC_ID");


                entity.HasMany(e => e.Addresses).WithOne(e => e.Client);
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("CLIENT_ADDRESSES");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.AddressName).HasColumnName("ADDRESS");
                entity.Property(e => e.ClientId).HasColumnName("CLIENT_ID");

                entity.HasOne(e => e.Client).WithMany(e => e.Addresses).IsRequired().IsRequired();
            });





            //modelBuilder.Entity<Role>(entity =>
            //{
            //    entity.ToTable("ROLES");
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Name).HasColumnName("NAME");
            //    entity.Property(e => e.Description).HasColumnName("DESCRIPTION");
            //    entity.Property(e => e.PublicId).HasColumnName("PUBLIC_ID");
            //});

            //modelBuilder.Entity<UserRole>(entity =>
            //{
            //    entity.ToTable("USER_ROLES");
            //    entity.HasKey(entity => entity.Id);
            //    entity.Property(entity => entity.Id).HasColumnName("ID");
            //    entity.Property(entity => entity.UserId).HasColumnName("USERID");
            //    entity.Property(entity => entity.RoleId).HasColumnName("ROLID");

            //    entity.HasOne<User>(u => u.User).WithMany(e => e.UsersRoles);

            //    entity.HasOne<Role>(u => u.Role).WithMany(e => e.UsersRoles);


            //});

        }        
    }
}
