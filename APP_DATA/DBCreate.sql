USE [CSS]
GO
/****** Object:  Table [dbo].[DBTERMINAL]    Script Date: 2022/8/26 下午 04:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DBTERMINAL](
	[NO] [varchar](10) NOT NULL,
	[NAME] [varchar](10) NULL,
	[RID] [varchar](10) NULL,
	[YMDHM] [varchar](12) NULL,
	[YMD] [varchar](8) NULL,
	[TDATE] [varchar](20) NULL,
	[ACCESS] [varchar](12) NULL,
	[DEPART] [varchar](6) NULL,
	[STATU] [varchar](1) NULL,
	[TSTATU] [varchar](20) NULL,
	[UDT] [varchar](12) NULL,
	[TDATE1] [varchar](10) NULL,
	[TDATE2] [varchar](5) NULL,
	[Searno] [numeric](18, 0) NULL,
	[TEMPCEN] [varchar](6) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Device]    Script Date: 2022/8/26 下午 04:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Device](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[No] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[IP] [nvarchar](50) NULL,
	[Port] [nvarchar](50) NULL,
	[Pass] [nvarchar](50) NULL,
	[identifyDistance] [int] NULL,
	[saveIdentifyTime] [int] NULL,
	[openout] [int] NULL,
	[inOutType] [int] NULL,
 CONSTRAINT [PK_Device] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Member]    Script Date: 2022/8/26 下午 04:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Member](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[No] [nvarchar](50) NOT NULL,
	[CardNo] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Pic1] [nvarchar](4000) NULL,
	[Pic2] [nvarchar](4000) NULL,
	[Pic3] [nvarchar](4000) NULL,
 CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
