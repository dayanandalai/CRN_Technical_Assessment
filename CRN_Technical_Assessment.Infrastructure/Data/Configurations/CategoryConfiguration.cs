using CRN_Technical_Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Categories> 
    { 
        public void Configure(EntityTypeBuilder<Categories> builder)
        { 
            builder.ToTable("Categories");
            builder.HasKey(x => x.Id); 
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.HasIndex(x => x.Name).IsUnique();
        } 
    }
}
