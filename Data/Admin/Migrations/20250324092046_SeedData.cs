using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiTenantDbContext.Data.Admin.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql =
                @"INSERT INTO public.""Customers""(""Id"", ""Name"") VALUES ('03CE6710-098F-47C6-BD39-9F28F3ABED8F', 'Customer1');" +
                @"INSERT INTO public.""Customers""(""Id"", ""Name"") VALUES ('29286E6B-A822-4689-8489-30D7DB11FA1E', 'Customer2');";
            migrationBuilder.Sql(sql);
            sql =
                @"INSERT INTO public.""Users""(""Id"", ""Name"", ""CustomerId"") VALUES ('1D3DF003-A688-42A0-AB06-BA84BBC96A32', 'User1', '03CE6710-098F-47C6-BD39-9F28F3ABED8F');" +
                @"INSERT INTO public.""Users""(""Id"", ""Name"", ""CustomerId"") VALUES ('F7C70BC6-CEAF-4FB3-A666-56D947BA12E8', 'User2', '03CE6710-098F-47C6-BD39-9F28F3ABED8F');";
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
