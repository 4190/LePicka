using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LePickaOrders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedDataSourceMicroserviceNameColumnToTablesWithExternalData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataSourceMicroserviceName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataSourceMicroserviceName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataSourceMicroserviceName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DataSourceMicroserviceName",
                table: "Products");
        }
    }
}
