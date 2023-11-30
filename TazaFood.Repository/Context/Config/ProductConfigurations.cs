using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Models;

namespace TazaFood.Repository.Context.Config
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(P => P.Category).WithMany()
                  .HasForeignKey(P => P.CategoryID ).OnDelete(DeleteBehavior.NoAction);
            builder.Property(P => P.Price).HasColumnType("decimal(18,2)");
        }
    }
}
