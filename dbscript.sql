USE [CostosObra]
GO
/****** Object:  Table [dbo].[adicionales]    Script Date: 6/8/2020 12:48:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adicionales](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idproyecto] [int] NOT NULL,
	[descripcion] [varchar](300) NULL,
	[monto] [decimal](19, 2) NULL,
 CONSTRAINT [PK_adicionales] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cliente]    Script Date: 6/8/2020 12:48:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cliente](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](300) NOT NULL,
 CONSTRAINT [PK_cliente] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[concepto]    Script Date: 6/8/2020 12:48:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[concepto](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](100) NOT NULL,
	[positivo] [bit] NOT NULL,
 CONSTRAINT [PK_concepto] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ctacte]    Script Date: 6/8/2020 12:48:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ctacte](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idconcepto] [int] NOT NULL,
	[monto] [decimal](19, 2) NULL,
	[idproyecto] [int] NOT NULL,
	[idproveedor] [int] NULL,
	[observacion] [varchar](300) NULL,
	[fecha] [date] NOT NULL,
 CONSTRAINT [PK_ctacte] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[proveedor]    Script Date: 6/8/2020 12:48:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[proveedor](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](300) NOT NULL,
	[idrubro] [int] NULL,
 CONSTRAINT [PK_proveedor] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[proyecto]    Script Date: 6/8/2020 12:48:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[proyecto](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[observacion] [varchar](300) NULL,
	[idcliente] [int] NOT NULL,
	[costototal] [decimal](19, 2) NULL,
	[costoreal] [decimal](19, 2) NULL,
	[archivo] [varbinary](max) NULL,
	[nombrearchivo] [varchar](100) NULL,
	[fechafin] [date] NULL,
 CONSTRAINT [PK_proyecto] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rubro]    Script Date: 6/8/2020 12:48:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rubro](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](300) NOT NULL,
 CONSTRAINT [PK_rubro] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[concepto] ON 
GO
INSERT [dbo].[concepto] ([id], [descripcion], [positivo]) VALUES (1, N'Ingreso', 1)
GO
INSERT [dbo].[concepto] ([id], [descripcion], [positivo]) VALUES (2, N'Egreso', 0)
GO
SET IDENTITY_INSERT [dbo].[concepto] OFF
GO
ALTER TABLE [dbo].[adicionales]  WITH CHECK ADD  CONSTRAINT [FK_adicionales_proyecto] FOREIGN KEY([idproyecto])
REFERENCES [dbo].[proyecto] ([id])
GO
ALTER TABLE [dbo].[adicionales] CHECK CONSTRAINT [FK_adicionales_proyecto]
GO
ALTER TABLE [dbo].[ctacte]  WITH CHECK ADD  CONSTRAINT [FK_ctacte_concepto] FOREIGN KEY([idconcepto])
REFERENCES [dbo].[concepto] ([id])
GO
ALTER TABLE [dbo].[ctacte] CHECK CONSTRAINT [FK_ctacte_concepto]
GO
ALTER TABLE [dbo].[ctacte]  WITH CHECK ADD  CONSTRAINT [FK_ctacte_proveedor] FOREIGN KEY([idproveedor])
REFERENCES [dbo].[proveedor] ([id])
GO
ALTER TABLE [dbo].[ctacte] CHECK CONSTRAINT [FK_ctacte_proveedor]
GO
ALTER TABLE [dbo].[ctacte]  WITH CHECK ADD  CONSTRAINT [FK_ctacte_proyecto] FOREIGN KEY([idproyecto])
REFERENCES [dbo].[proyecto] ([id])
GO
ALTER TABLE [dbo].[ctacte] CHECK CONSTRAINT [FK_ctacte_proyecto]
GO
ALTER TABLE [dbo].[proveedor]  WITH CHECK ADD  CONSTRAINT [FK_proveedor_rubro] FOREIGN KEY([idrubro])
REFERENCES [dbo].[rubro] ([id])
GO
ALTER TABLE [dbo].[proveedor] CHECK CONSTRAINT [FK_proveedor_rubro]
GO
ALTER TABLE [dbo].[proyecto]  WITH CHECK ADD  CONSTRAINT [FK_proyecto_cliente] FOREIGN KEY([idcliente])
REFERENCES [dbo].[cliente] ([id])
GO
ALTER TABLE [dbo].[proyecto] CHECK CONSTRAINT [FK_proyecto_cliente]
GO
