using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Control_de_Lotes.Models;

namespace Control_de_Lotes.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
	    public DbSet<Control_de_Lotes.Models.Cosecha> Cosecha { get; set; } = default!;
	    public DbSet<Control_de_Lotes.Models.Cultivos> Cultivos { get; set; } = default!;
	    public DbSet<Control_de_Lotes.Models.Extras> Extras { get; set; } = default!;
	    public DbSet<Control_de_Lotes.Models.Fumigacion> Fumigacion { get; set; } = default!;
	    public DbSet<Control_de_Lotes.Models.Lote> Lote { get; set; } = default!;
	    public DbSet<Control_de_Lotes.Models.Motivos> Motivos { get; set; } = default!;
	    public DbSet<Control_de_Lotes.Models.Siembra> Siembra { get; set; } = default!;
	}
}
