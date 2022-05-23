using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rotas.Api.Context
{
    public class AppDbContext : DbContext
    {
        //Definindo o construtor.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
