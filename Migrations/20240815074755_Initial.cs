using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EFCorePostgres.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authority",
                columns: table => new
                {
                    auth_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    auth_name = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__authorit__6531B6F503340C2B", x => x.auth_id);
                });

            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    client_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    address = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    banned = table.Column<bool>(type: "boolean", nullable: false),
                    email = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    first_name = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: false),
                    last_name = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: false),
                    user_name = table.Column<string>(type: "character varying(60)", unicode: false, maxLength: 60, nullable: false, computedColumnSql: "\"email\"", stored: true),
                    PasswordHash = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__client__BF21A42471C2D2BA", x => x.client_id);
                });

            migrationBuilder.CreateTable(
                name: "food",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cook_time = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: false),
                    hidden = table.Column<bool>(type: "boolean", nullable: false),
                    image_url = table.Column<string>(type: "character varying(200)", unicode: false, maxLength: 200, nullable: false),
                    name = table.Column<string>(type: "character varying(300)", unicode: false, maxLength: 300, nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__food__3213E83F74626B81", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role_claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_role_claims_authority_RoleId",
                        column: x => x.RoleId,
                        principalTable: "authority",
                        principalColumn: "auth_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_client_UserId",
                        column: x => x.UserId,
                        principalTable: "client",
                        principalColumn: "client_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_authority",
                columns: table => new
                {
                    client_id = table.Column<long>(type: "bigint", nullable: false),
                    auth_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__client_a__F972BF4B7C561F2D", x => new { x.client_id, x.auth_id });
                    table.ForeignKey(
                        name: "FK_client_authority_authority_auth_id",
                        column: x => x.auth_id,
                        principalTable: "authority",
                        principalColumn: "auth_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_client_authority_client_client_id",
                        column: x => x.client_id,
                        principalTable: "client",
                        principalColumn: "client_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "food_order",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    address = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: true),
                    lat = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: true),
                    lng = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: true),
                    created_at = table.Column<DateOnly>(type: "date", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false),
                    status = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false),
                    total_price = table.Column<double>(type: "double precision", nullable: false),
                    updated_at = table.Column<DateOnly>(type: "date", nullable: true),
                    client_id = table.Column<long>(type: "bigint", nullable: false),
                    payment_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__food_ord__3213E83F8AC7B3DD", x => x.id);
                    table.ForeignKey(
                        name: "FKeeqknj22y8xo7k7qex47m9njn",
                        column: x => x.client_id,
                        principalTable: "client",
                        principalColumn: "client_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_claims_client_UserId",
                        column: x => x.UserId,
                        principalTable: "client",
                        principalColumn: "client_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_logins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_logins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_user_logins_client_UserId",
                        column: x => x.UserId,
                        principalTable: "client",
                        principalColumn: "client_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_food_fav",
                columns: table => new
                {
                    client_id = table.Column<long>(type: "bigint", nullable: false),
                    food_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__client_f__2DD560F9F63CBDF2", x => new { x.client_id, x.food_id });
                    table.ForeignKey(
                        name: "FK15cwpt18syey0o02qlcud1c1q",
                        column: x => x.client_id,
                        principalTable: "client",
                        principalColumn: "client_id");
                    table.ForeignKey(
                        name: "FKi8ucp5jby4ml3ssxbqljsgqr0",
                        column: x => x.food_id,
                        principalTable: "food",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "food_origins",
                columns: table => new
                {
                    food_id = table.Column<long>(type: "bigint", nullable: false),
                    origins = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FKb7xuhj95rkfmcpm3pnqcysdxa",
                        column: x => x.food_id,
                        principalTable: "food",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "food_tags",
                columns: table => new
                {
                    food_id = table.Column<long>(type: "bigint", nullable: false),
                    tags = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FKhho6c8bc39ejtrnphfxph3ito",
                        column: x => x.food_id,
                        principalTable: "food",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order_item",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    food_id = table.Column<long>(type: "bigint", nullable: false),
                    order_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__order_it__3213E83FAF602DB7", x => x.id);
                    table.ForeignKey(
                        name: "FK4fcv9bk14o2k04wghr09jmy3b",
                        column: x => x.food_id,
                        principalTable: "food",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK4x9b1ny7wu8uwe0w6vgdyp5ut",
                        column: x => x.order_id,
                        principalTable: "food_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "authority",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "client",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_client_user_name",
                table: "client",
                column: "user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_bfgjs3fem0hmjhvih80158x29",
                table: "client",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "client",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_client_authority_auth_id",
                table: "client_authority",
                column: "auth_id");

            migrationBuilder.CreateIndex(
                name: "IX_client_food_fav_food_id",
                table: "client_food_fav",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "UK_qkhr2yo38c1g9n5ss0jl7gxk6",
                table: "food",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_food_order_client_id",
                table: "food_order",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_food_origins_food_id",
                table: "food_origins",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "IX_food_tags_food_id",
                table: "food_tags",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_food_id",
                table: "order_item",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_order_id",
                table: "order_item",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_claims_RoleId",
                table: "role_claims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_user_claims_UserId",
                table: "user_claims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_logins_UserId",
                table: "user_logins",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "client_authority");

            migrationBuilder.DropTable(
                name: "client_food_fav");

            migrationBuilder.DropTable(
                name: "food_origins");

            migrationBuilder.DropTable(
                name: "food_tags");

            migrationBuilder.DropTable(
                name: "order_item");

            migrationBuilder.DropTable(
                name: "role_claims");

            migrationBuilder.DropTable(
                name: "user_claims");

            migrationBuilder.DropTable(
                name: "user_logins");

            migrationBuilder.DropTable(
                name: "food");

            migrationBuilder.DropTable(
                name: "food_order");

            migrationBuilder.DropTable(
                name: "authority");

            migrationBuilder.DropTable(
                name: "client");
        }
    }
}
