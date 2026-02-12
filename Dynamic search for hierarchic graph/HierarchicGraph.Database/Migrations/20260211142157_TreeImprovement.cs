using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HierarchicGraph.Database.Migrations
{
    /// <inheritdoc />
    public partial class TreeImprovement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChildIds",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Items",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ParentId",
                table: "Items",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Items_ParentId",
                table: "Items",
                column: "ParentId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Items_ParentId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ParentId",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChildIds",
                table: "Items",
                type: "TEXT",
                nullable: true);
        }
    }
}
