using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TgBotApi.Models;

namespace TgBotApi
{
    public class EngDbContext : DbContext
    {
        public EngDbContext(DbContextOptions<EngDbContext> opt) : base(opt)
        {
                
        }
        public DbSet<UserVocab> UserVocabs { get; set; }
        public DbSet<VocabItem> VocabItems { get; set; }
    }
}
