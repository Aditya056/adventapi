using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace advent_appointment_booking.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTruckingCompanyProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "TruckingCompanies",
                newName: "TrCompanyName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrCompanyName",
                table: "TruckingCompanies",
                newName: "CompanyName");
        }
    }
}
