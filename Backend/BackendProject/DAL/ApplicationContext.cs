using TemplateApp.DAL.Entities;
using TemplateApp.DAL.Users;
using Microsoft.EntityFrameworkCore;

namespace TemplateApp.DAL
{
	public class ApplicationContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		public DbSet<DataGovRuEntry> DataGovRuEntries { get; set; }

		public DbSet<Notification> Notifications { get; set; }

		public DbSet<DataGovRuEntryRow> DataGovRuEntryRows { get; set; }

		public ApplicationContext(DbContextOptions<ApplicationContext> options)
			: base(options)
		{
			//Database.EnsureCreated();
			this.ChangeTracker.LazyLoadingEnabled = false;

		}

	}
}
