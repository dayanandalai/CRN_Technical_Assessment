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
    public class UserConfiguration : IEntityTypeConfiguration<User> 
    { 
        public void Configure(EntityTypeBuilder<User> builder) 
        { builder.ToTable("Users"); 
            builder.HasKey(x => x.Id); 
            builder.Property(x => x.Username).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(255).IsRequired();
            builder.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Username).IsUnique(); 
        } 
    }
}
