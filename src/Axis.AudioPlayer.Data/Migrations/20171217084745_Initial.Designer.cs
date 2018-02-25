﻿// <auto-generated />
using Axis.AudioPlayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Axis.AudioPlayer.Data.Migrations
{
    [DbContext(typeof(AudioPlayerContext))]
    [Migration("20171217084745_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Axis.AudioPlayer.Data.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName");

                    b.Property<string>("IPAddress");

                    b.Property<string>("Password");

                    b.Property<string>("Product");

                    b.Property<Guid?>("StateId");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("StateId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Axis.AudioPlayer.Data.DeviceState", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("BackgroundVolume");

                    b.Property<double>("ForegroundVolume");

                    b.Property<double>("MusicVome");

                    b.HasKey("Id");

                    b.ToTable("DeviceStates");
                });

            modelBuilder.Entity("Axis.AudioPlayer.Data.Device", b =>
                {
                    b.HasOne("Axis.AudioPlayer.Data.DeviceState", "State")
                        .WithMany()
                        .HasForeignKey("StateId");
                });
#pragma warning restore 612, 618
        }
    }
}