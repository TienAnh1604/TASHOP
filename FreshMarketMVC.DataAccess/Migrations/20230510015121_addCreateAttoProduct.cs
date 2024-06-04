using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrganicFoodMVC.DataAccess.Migrations
{
    public partial class addCreateAttoProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Products",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Products");
        }
    }
}
