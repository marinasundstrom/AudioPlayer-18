﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Axis.AudioPlayer.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BackgroundVolume = table.Column<double>(nullable: false),
                    ForegroundVolume = table.Column<double>(nullable: false),
                    MusicVome = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Product = table.Column<string>(nullable: true),
                    StateId = table.Column<Guid>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_DeviceStates_StateId",
                        column: x => x.StateId,
                        principalTable: "DeviceStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_StateId",
                table: "Devices",
                column: "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "DeviceStates");
        }
    }
}
