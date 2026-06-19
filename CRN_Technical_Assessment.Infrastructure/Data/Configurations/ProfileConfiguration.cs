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
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile> 
    { 
        public void Configure(EntityTypeBuilder<Profile> builder) 
        { 
            builder.ToTable("UserProfiles"); 
            builder.HasKey(x => x.Id); 
            builder.Property(x => x.FirstName).HasMaxLength(100);
            builder.Property(x => x.LastName).HasMaxLength(100); 
            builder.Property(x => x.PhoneNumber).HasMaxLength(20); 
            builder.Property(x => x.Address).HasMaxLength(500);
            builder.HasOne(x => x.User).WithOne(x => x.Profile).HasForeignKey<Profile>(x => x.UserId); } }
}
