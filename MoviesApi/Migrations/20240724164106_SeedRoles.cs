using Microsoft.EntityFrameworkCore.Migrations;
using MoviesApi.Helpers;

#nullable disable

namespace MoviesApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values:new object[] {Guid.NewGuid().ToString(), RolesSettings.Basic , RolesSettings.Basic.ToUpper(), Guid.NewGuid().ToString() }
                );
            
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values:new object[] {Guid.NewGuid().ToString(), RolesSettings.Admin , RolesSettings.Admin.ToUpper(), Guid.NewGuid().ToString() }
                ); 
            
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values:new object[] {Guid.NewGuid().ToString(), RolesSettings.Super , RolesSettings.Super.ToUpper(), Guid.NewGuid().ToString() }
                );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE From AspNetRoles");
        }
    }
}
