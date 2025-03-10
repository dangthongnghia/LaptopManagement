USE [master]
GO
/****** Object:  Database [LaptopManagementContext]    Script Date: 3/10/2025 10:22:35 AM ******/
CREATE DATABASE [LaptopManagementContext]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LaptopManagementContext', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.DANGNGHIA\MSSQL\DATA\LaptopManagementContext.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LaptopManagementContext_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.DANGNGHIA\MSSQL\DATA\LaptopManagementContext_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [LaptopManagementContext] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LaptopManagementContext].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LaptopManagementContext] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET ARITHABORT OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LaptopManagementContext] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LaptopManagementContext] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET  ENABLE_BROKER 
GO
ALTER DATABASE [LaptopManagementContext] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LaptopManagementContext] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [LaptopManagementContext] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET RECOVERY FULL 
GO
ALTER DATABASE [LaptopManagementContext] SET  MULTI_USER 
GO
ALTER DATABASE [LaptopManagementContext] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LaptopManagementContext] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LaptopManagementContext] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LaptopManagementContext] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LaptopManagementContext] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LaptopManagementContext] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'LaptopManagementContext', N'ON'
GO
ALTER DATABASE [LaptopManagementContext] SET QUERY_STORE = ON
GO
ALTER DATABASE [LaptopManagementContext] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [LaptopManagementContext]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 3/10/2025 10:22:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 3/10/2025 10:22:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LaptopImages]    Script Date: 3/10/2025 10:22:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LaptopImages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LaptopId] [int] NOT NULL,
	[ImagePath] [nvarchar](max) NOT NULL,
	[IsPrimary] [bit] NOT NULL,
 CONSTRAINT [PK_LaptopImages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Laptops]    Script Date: 3/10/2025 10:22:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Laptops](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Brand] [nvarchar](50) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[CPU] [nvarchar](50) NULL,
	[RAM] [nvarchar](50) NULL,
	[Storage] [nvarchar](50) NULL,
	[Status] [bit] NOT NULL,
	[CategoryId] [int] NULL,
 CONSTRAINT [PK_Laptops] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 3/10/2025 10:22:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LaptopId] [int] NOT NULL,
	[CustomerName] [nvarchar](100) NOT NULL,
	[CustomerEmail] [nvarchar](255) NOT NULL,
	[CustomerPhone] [nvarchar](15) NOT NULL,
	[OrderDate] [datetime2](7) NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[ImagePath] [nvarchar](max) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250303074349_InitialCreate', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250303081450_CreateAddressOrderTabe', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250303090451_LaptopImg', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250304010432_RenameDbSetToLaptops', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250304011232_ConfigurePricePrecision', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250304015534_DeleteIMGPath', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250309090553_Category', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250309151301_UpdateLaptopModel', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250309155233_InitialCategory', N'9.0.2')
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([Id], [Name]) VALUES (1, N'Gaming')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (2, N'Office')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (3, N'UtraBook')
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[LaptopImages] ON 

INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (68, 43, N'/images/f0a5b73b-b275-4361-b166-5dccdcb0c59f_AsusTUF_Gaming_A15_1.jpg', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (69, 25, N'/images/c4e6571a-4e5f-45fb-a28a-34104890a6b6_Dell_XPS_13.jpg', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (70, 25, N'/images/282b92de-dd0e-45df-9af0-3385ad0bbeda_Dell_XPS13Plus_2022FHD_50.jpg', 0)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (71, 26, N'/images/71c8c633-3f3a-495f-bcf3-029caf77922f_71y+lIHVdAL.jpg', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (72, 26, N'/images/f045bfda-5f4b-433c-80cd-ac355e945ce2_mac_book_pro.jpg', 0)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (73, 27, N'/images/d0d6d55c-c8f9-40bf-b619-d9e86751aed5_c7581effbc1728fc2e529294f6cd892d-hi.jpg', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (74, 28, N'/images/c6b579c9-78a8-49d5-8efa-7dd25f9bce8c_HP Spectre x360.jpg', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (75, 29, N'/images/fad9945a-ad20-4939-8abb-10d0bc7f126f_Lenovo ThinkPad X1 Carbon.jpg', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (76, 30, N'/images/2b466d53-cf85-4f3f-8e0e-551ee59cf777_MSI Stealth 15M.jpg', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (77, 31, N'/images/9abbab8e-8aa9-452d-ba32-6e4d7f6b4d81_Acer Predator Helios 300.jpg', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (78, 32, N'/images/422468ec-522b-4503-ac46-530a4b3129cb_Surface Laptop 4.jpg', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (79, 33, N'/images/d09e3ce3-885c-4a55-8d1a-c8907a079979_Razer Blade 15.jpg', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (80, 34, N'/images/82efb2fb-6383-49f8-a55d-0643f559034d_LG Gram 17.jpg', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (82, 35, N'/images/1ccf751f-f9ca-43fe-a39a-3c3b017976d5_Dell Inspiron 15.jpg', 0)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (83, 36, N'/images/e0180b82-8a3b-48da-8433-a6d847722e3f_Asus ZenBook 14.jpg', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (84, 37, N'/images/ccd41214-5479-46ba-9733-bc1a7e316006_HP Pavilion 15.webp', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (85, 38, N'/images/d19cdf40-a669-4fb2-9834-34b2447088c7_Lenovo Legion 5.webp', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (86, 39, N'/images/4375606b-1552-49d3-a12a-aa88e78397cd_MacBook Air M2.jpg', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (87, 40, N'/images/876531b0-e172-41cf-833b-d0556b9aa180_Acer Swift 3.jpg', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (88, 41, N'/images/7fefc29a-4425-4d22-b9f6-42f61cf29b99_MSI Katana GF66.jpg', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (89, 42, N'/images/f292f0e6-74ef-4489-acee-83390f850751_Dell G15.jpg', 1)
INSERT [dbo].[LaptopImages] ([Id], [LaptopId], [ImagePath], [IsPrimary]) VALUES (90, 44, N'/images/fc0dac5e-4a12-4e06-a348-b7aaf00d5c17_HP Envy 13.webp', 1)
SET IDENTITY_INSERT [dbo].[LaptopImages] OFF
GO
SET IDENTITY_INSERT [dbo].[Laptops] ON 

INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (25, N'Dell XPS 13', N'Dell', CAST(25000000.00 AS Decimal(18, 2)), N'Intel i7-1165G7', N'16GB', N'512GB SSD', 1, 3)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (26, N'MacBook Pro 14', N'Apple', CAST(45000000.00 AS Decimal(18, 2)), N'M1 Pro', N'16GB', N'1TB SSD', 1, 3)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (27, N'Asus ROG Zephyrus G14', N'Asus', CAST(35000000.00 AS Decimal(18, 2)), N'AMD Ryzen 9 5900HS', N'32GB', N'1TB SSD', 1, 1)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (28, N'HP Spectre x360', N'HP', CAST(30000000.00 AS Decimal(18, 2)), N'Intel i7-1255U', N'16GB', N'512GB SSD', 1, 3)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (29, N'Lenovo ThinkPad X1 Carbon', N'Lenovo', CAST(32000000.00 AS Decimal(18, 2)), N'Intel i7-1260P', N'16GB', N'512GB SSD', 1, 2)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (30, N'MSI Stealth 15M', N'MSI', CAST(28000000.00 AS Decimal(18, 2)), N'Intel i7-11375H', N'16GB', N'1TB SSD', 0, 1)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (31, N'Acer Predator Helios 300', N'Acer', CAST(29000000.00 AS Decimal(18, 2)), N'Intel i7-12700H', N'16GB', N'512GB SSD', 1, 1)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (32, N'Surface Laptop 4', N'Microsoft', CAST(27000000.00 AS Decimal(18, 2)), N'AMD Ryzen 7 4980U', N'8GB', N'256GB SSD', 1, 2)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (33, N'Razer Blade 15', N'Razer', CAST(42000000.00 AS Decimal(18, 2)), N'Intel i7-12800H', N'32GB', N'1TB SSD', 1, 1)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (34, N'LG Gram 17', N'LG', CAST(31000000.00 AS Decimal(18, 2)), N'Intel i7-1260P', N'16GB', N'512GB SSD', 1, 3)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (35, N'Dell Inspiron 15', N'Dell', CAST(18000000.00 AS Decimal(18, 2)), N'Intel i5-1135G7', N'8GB', N'256GB SSD', 1, 2)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (36, N'Asus ZenBook 14', N'Asus', CAST(26000000.00 AS Decimal(18, 2)), N'Core i5-1135G7', N'16GB', N'512GB SSD', 1, 3)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (37, N'HP Pavilion 15', N'HP', CAST(20000000.00 AS Decimal(18, 2)), N'AMD Ryzen 5 5600U', N'8GB', N'512GB SSD', 1, 2)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (38, N'Lenovo Legion 5', N'Lenovo', CAST(31000000.00 AS Decimal(18, 2)), N'AMD Ryzen 7 6800H', N'16GB', N'1TB SSD', 1, 1)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (39, N'MacBook Air M2', N'Apple', CAST(35000000.00 AS Decimal(18, 2)), N'M2', N'8GB', N'256GB SSD', 1, 3)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (40, N'Acer Swift 3', N'Acer', CAST(19000000.00 AS Decimal(18, 2)), N'AMD Ryzen 5 5500U', N'8GB', N'512GB SSD', 1, 2)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (41, N'MSI Katana GF66', N'MSI', CAST(27000000.00 AS Decimal(18, 2)), N'Intel i5-12500H', N'16GB', N'512GB SSD', 1, 1)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (42, N'Dell G15', N'Dell', CAST(25000000.00 AS Decimal(18, 2)), N'Intel i5-12500H', N'16GB', N'512GB SSD', 1, 1)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (43, N'Asus TUF Gaming A15', N'Asus', CAST(24000000.00 AS Decimal(18, 2)), N'AMD Ryzen 7 4800H', N'16GB', N'1TB SSD', 1, 1)
INSERT [dbo].[Laptops] ([Id], [Name], [Brand], [Price], [CPU], [RAM], [Storage], [Status], [CategoryId]) VALUES (44, N'HP Envy 13', N'HP', CAST(28000000.00 AS Decimal(18, 2)), N'Intel i7-1165G7', N'16GB', N'512GB SSD', 1, 3)
SET IDENTITY_INSERT [dbo].[Laptops] OFF
GO
/****** Object:  Index [IX_LaptopImages_LaptopId]    Script Date: 3/10/2025 10:22:36 AM ******/
CREATE NONCLUSTERED INDEX [IX_LaptopImages_LaptopId] ON [dbo].[LaptopImages]
(
	[LaptopId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_LaptopId]    Script Date: 3/10/2025 10:22:36 AM ******/
CREATE NONCLUSTERED INDEX [IX_Orders_LaptopId] ON [dbo].[Orders]
(
	[LaptopId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[LaptopImages]  WITH CHECK ADD  CONSTRAINT [FK_LaptopImages_Laptops_LaptopId] FOREIGN KEY([LaptopId])
REFERENCES [dbo].[Laptops] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LaptopImages] CHECK CONSTRAINT [FK_LaptopImages_Laptops_LaptopId]
GO
ALTER TABLE [dbo].[Laptops]  WITH CHECK ADD  CONSTRAINT [FK_Laptops_Categories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
GO
ALTER TABLE [dbo].[Laptops] CHECK CONSTRAINT [FK_Laptops_Categories]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Laptops_LaptopId] FOREIGN KEY([LaptopId])
REFERENCES [dbo].[Laptops] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Laptops_LaptopId]
GO
USE [master]
GO
ALTER DATABASE [LaptopManagementContext] SET  READ_WRITE 
GO
