using DotPix.Models;
using Microsoft.EntityFrameworkCore;

namespace DotPix.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
}