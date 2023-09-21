using System.Data.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace MovieApi.Models;

public class MovieContext : DbContext
{
    public DbSet<Movies>? Movies { get; set; } = null;
    
    public MovieContext(DbContextOptions<MovieContext> options) : base(options)
    {
        
    }
    
    
}