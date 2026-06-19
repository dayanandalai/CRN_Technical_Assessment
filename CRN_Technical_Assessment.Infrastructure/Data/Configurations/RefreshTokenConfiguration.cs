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
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken> 
    { 
        public void Configure(EntityTypeBuilder<RefreshToken> builder) 
        { 
            builder.ToTable("RefreshTokens"); 
            builder.HasKey(x => x.Id); builder.Property(x => x.Token).HasMaxLength(500).IsRequired();
            builder.Property(x => x.ExpiresAt).IsRequired();
            builder.HasOne(x => x.User).WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
