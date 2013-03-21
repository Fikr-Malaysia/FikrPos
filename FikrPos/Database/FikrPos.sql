USE [FikrPos]
GO
/****** Object:  Table [dbo].[AppUser]    Script Date: 03/21/2013 23:35:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AppUser](
	[username] [varchar](50) NOT NULL,
	[password] [varchar](255) NULL,
	[isadmin] [tinyint] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[AppUser] ([username], [password], [isadmin]) VALUES (N'root', N'segjouSCJB0=', 1)
/****** Object:  Table [dbo].[AppInfo]    Script Date: 03/21/2013 23:35:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppInfo](
	[IsInit] [tinyint] NOT NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_AppInfo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AppInfo] ON
INSERT [dbo].[AppInfo] ([IsInit], [id]) VALUES (0, 1)
SET IDENTITY_INSERT [dbo].[AppInfo] OFF
/****** Object:  Default [DF_Users_isadmin]    Script Date: 03/21/2013 23:35:43 ******/
ALTER TABLE [dbo].[AppUser] ADD  CONSTRAINT [DF_Users_isadmin]  DEFAULT ((0)) FOR [isadmin]
GO
