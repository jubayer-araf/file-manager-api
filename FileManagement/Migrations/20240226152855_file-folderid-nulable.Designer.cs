﻿// <auto-generated />
using System;
using FileManagement.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FileManagement.Migrations
{
    [DbContext(typeof(FileManagementDbContext))]
    [Migration("20240226152855_file-folderid-nulable")]
    partial class filefolderidnulable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.27")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FileManagement.Entities.FileDetail", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedByUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FileTypeId")
                        .HasColumnType("int");

                    b.Property<string>("FolderId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastUpdatedByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserGroupId")
                        .HasColumnType("int");

                    b.Property<string>("UserGroupName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("FileDetails", (string)null);
                });

            modelBuilder.Entity("FileManagement.Entities.FileType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CreatedByUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastUpdatedByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserGroupId")
                        .HasColumnType("int");

                    b.Property<string>("UserGroupName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("FileTypes", (string)null);
                });

            modelBuilder.Entity("FileManagement.Entities.FolderDetail", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedByUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FolderPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastUpdatedByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentFolderId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserGroupId")
                        .HasColumnType("int");

                    b.Property<string>("UserGroupName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("FolderDetails", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
