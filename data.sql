USE [DOCTRUYEN]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 5/28/2024 1:29:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[idAccount] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](50) NULL,
	[username] [nvarchar](50) NULL,
	[pwAccount] [nvarchar](50) NULL,
	[TrangThai] [nvarchar](50) NULL,
	[roleAcc] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[idAccount] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Chuong]    Script Date: 5/28/2024 1:29:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Chuong](
	[idChuong] [int] IDENTITY(1,1) NOT NULL,
	[idTruyen] [int] NULL,
	[Chuong] [int] NULL,
	[LinkTruyen] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[idChuong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TheLoai]    Script Date: 5/28/2024 1:29:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TheLoai](
	[idTheLoai] [int] IDENTITY(1,1) NOT NULL,
	[TenTheLoai] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[idTheLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Truyen]    Script Date: 5/28/2024 1:29:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Truyen](
	[idTruyen] [int] IDENTITY(1,1) NOT NULL,
	[TenTruyen] [nvarchar](50) NULL,
	[idTheLoai] [int] NULL,
	[idAccount] [int] NULL,
	[TacGia] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[idTruyen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[YeuThich]    Script Date: 5/28/2024 1:29:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[YeuThich](
	[idYeuThich] [int] IDENTITY(1,1) NOT NULL,
	[idTruyen] [int] NULL,
	[idAccount] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[idYeuThich] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Chuong]  WITH CHECK ADD FOREIGN KEY([idTruyen])
REFERENCES [dbo].[Truyen] ([idTruyen])
GO
ALTER TABLE [dbo].[Truyen]  WITH CHECK ADD FOREIGN KEY([idAccount])
REFERENCES [dbo].[Account] ([idAccount])
GO
ALTER TABLE [dbo].[Truyen]  WITH CHECK ADD FOREIGN KEY([idTheLoai])
REFERENCES [dbo].[TheLoai] ([idTheLoai])
GO
ALTER TABLE [dbo].[YeuThich]  WITH CHECK ADD FOREIGN KEY([idTruyen])
REFERENCES [dbo].[Truyen] ([idTruyen])
GO
ALTER TABLE [dbo].[YeuThich]  WITH CHECK ADD  CONSTRAINT [fk_yt] FOREIGN KEY([idAccount])
REFERENCES [dbo].[Account] ([idAccount])
GO
ALTER TABLE [dbo].[YeuThich] CHECK CONSTRAINT [fk_yt]
GO
