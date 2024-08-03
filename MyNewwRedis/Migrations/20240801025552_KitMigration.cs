using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyNewwRedis.Migrations
{
    /// <inheritdoc />
    public partial class KitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("04520370-d1c3-4525-b985-8faf32742a73"), "sevilla-kit" },
                    { new Guid("07e6c872-9419-4998-b933-d0a58a543c3b"), "benfica-kit" },
                    { new Guid("0848bf92-cf05-49ff-bc96-810e2660d241"), "bayern-munchen-kit" },
                    { new Guid("09644cdd-5032-49d8-a570-f40eb83ba3d7"), "inter-milan-kit" },
                    { new Guid("0b4abc68-8115-4a1d-82b4-fd81b6f98b7b"), "lyon-kit" },
                    { new Guid("0f4e6143-1ba2-4a0c-a8bb-b0abaa7bfb31"), "red-star-belgrade-kit" },
                    { new Guid("1619adf3-a205-4e99-9859-0772a4f78f3c"), "sporting-cp-kit" },
                    { new Guid("1748f290-95e5-4071-a56f-480c54293330"), "porto-kit" },
                    { new Guid("17a3ee69-23c1-4c1a-89bc-ed00dde5fe22"), "ssc-napoli-kit" },
                    { new Guid("1c64b045-eca3-4131-9a48-66b3a442987b"), "atletico-madrid-kit" },
                    { new Guid("1f3ed169-1536-4ab1-aefc-f5c17a4ad56e"), "dortmund-kit" },
                    { new Guid("2b8fc8af-7b8a-4184-8aa1-ee089cdd6b67"), "ajax-kit" },
                    { new Guid("372973fb-1d28-46ef-a6b7-a65c9a5eb18c"), "liverpool-kit" },
                    { new Guid("3bc528af-1aaa-4018-87fa-197e4c35174c"), "celtic-kit" },
                    { new Guid("419b2816-905f-4b1e-ae80-dff1b1f0764a"), "juventus-kit" },
                    { new Guid("429f0d31-24ec-4b9a-b750-17a3f5691633"), "galatasaray-kit" },
                    { new Guid("54c431ab-c1d7-4695-a1d7-e2242ba0f368"), "psv-eindhoven-kit" },
                    { new Guid("5f55bfff-d164-4afa-ad14-ead5a021d856"), "ac-milan-kit" },
                    { new Guid("6877456d-7aff-4e9e-b387-81bdf54c354d"), "paris-saint-germain-kit" },
                    { new Guid("84dd1c7f-c39d-4fe3-b9e2-229193a0d2d1"), "lazio-kit" },
                    { new Guid("88f92638-ee76-4176-8158-aeae8d1db891"), "manchester-city-kit" },
                    { new Guid("955ad847-4b16-4b12-89af-062d502861b1"), "manchester-united-kit " },
                    { new Guid("abe9fd3c-ef76-4d50-a220-530ed9200e4c"), "rb-leipzig-kit" },
                    { new Guid("bee1ac44-773b-4a04-8e88-50d0d403ec6c"), "barcelona-kit" },
                    { new Guid("ccae299f-8ae7-4853-883f-0bb35c8e49ab"), "fenerbahce-kit" },
                    { new Guid("d14e8354-5413-4fb3-bae5-748d5d99284b"), "marseille-kit" },
                    { new Guid("d4e292c7-11db-405f-a47d-43bdbb250c9d"), "chelsea-kit" },
                    { new Guid("db4e3a92-3321-430e-8e67-2d56bfd3f3ee"), "as-roma-kit" },
                    { new Guid("dd0969c6-9d65-4ff5-ab51-aae5904009f1"), "tottenham-hotspur-kit" },
                    { new Guid("e28c8383-1d10-45de-8bf2-d183054d29ec"), "bayer-04-leverkusen-kit" },
                    { new Guid("e5b1b597-c799-4c69-8fb9-bc04f0a4d805"), "arsenal-kit" },
                    { new Guid("f7d07862-a8d4-499b-ae97-74c86c61666c"), "real-madrid-kit" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
