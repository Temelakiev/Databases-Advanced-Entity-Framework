namespace TeamBuilder.Data
{
    using System.Data.Entity;
    using Configurations;
    using Models;

    public class TeamBuilderContext : DbContext
    {
        public TeamBuilderContext()
            : base("name=TeamBuilderContext")
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<TeamBuilderContext>());
        }



        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Event> Events { get; set; }

        public virtual DbSet<Team> Teams { get; set; }

        public virtual DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new EventConfiguration());
            modelBuilder.Configurations.Add(new InvitationConfiguration());
            modelBuilder.Configurations.Add(new TeamConfiguration());
            base.OnModelCreating(modelBuilder);
        }

    }
}