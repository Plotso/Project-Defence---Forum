using ForumProject.Migrations;
using ForumProject.Models;
using Microsoft.Owin;
using Owin;
using System.Data.Entity;

[assembly: OwinStartupAttribute(typeof(ForumProject.Startup))]
namespace ForumProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<ForumDbContext, Configuration>());

            ConfigureAuth(app);
        }
    }
}
