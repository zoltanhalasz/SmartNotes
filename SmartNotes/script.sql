
CREATE DATABASE [SmartNotesDB]
GO
USE [SmartNotesDB]
GO
/****** Object:  Table [dbo].[images]    Script Date: 2/20/2020 12:43:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[images](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[noteid] [int] NOT NULL,
	[image] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_images] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[notes]    Script Date: 2/20/2020 12:43:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[notes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userid] [int] NOT NULL,
	[title] [nvarchar](50) NOT NULL,
	[note_text] [nvarchar](1000) NOT NULL,
	[createdat] [datetime] NOT NULL,
	[pinned] [bit] NOT NULL,
	[color] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_notes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 2/20/2020 12:43:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[email] [nvarchar](100) NOT NULL,
	[password] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_users]    Script Date: 2/20/2020 12:43:32 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_users] ON [dbo].[users]
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[notes] ADD  CONSTRAINT [DF_notes_createdat]  DEFAULT (getdate()) FOR [createdat]
GO
ALTER TABLE [dbo].[notes] ADD  CONSTRAINT [DF_notes_pinned]  DEFAULT ((0)) FOR [pinned]
GO
ALTER TABLE [dbo].[notes] ADD  CONSTRAINT [DF_notes_color]  DEFAULT (N'#FFF') FOR [color]
GO
ALTER TABLE [dbo].[images]  WITH CHECK ADD  CONSTRAINT [FK_images_notes] FOREIGN KEY([noteid])
REFERENCES [dbo].[notes] ([id])
GO
ALTER TABLE [dbo].[images] CHECK CONSTRAINT [FK_images_notes]
GO
ALTER TABLE [dbo].[notes]  WITH CHECK ADD  CONSTRAINT [FK_notes_users] FOREIGN KEY([userid])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[notes] CHECK CONSTRAINT [FK_notes_users]
GO
