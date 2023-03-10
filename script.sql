Create DATABASE zonapagos
GO
USE [zonapagos]
GO
/****** Object:  Table [dbo].[Comercio]    Script Date: 25/12/2022 3:59:53 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comercio](
	[comercio_codigo] [int] NOT NULL,
	[comercio_nombre] [varchar](100) NOT NULL,
	[comercio_nit] [varchar](15) NOT NULL,
	[comercio_direccion] [varchar](100) NULL,
	[comercio_password] [varchar](100) NULL,
 CONSTRAINT [PK_Comercio] PRIMARY KEY CLUSTERED 
(
	[comercio_codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trans]    Script Date: 25/12/2022 3:59:53 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trans](
	[trans_codigo] [varchar](50) NOT NULL,
	[trans_total] [numeric](18, 0) NOT NULL,
	[trans_fecha] [varchar](50) NOT NULL,
	[trans_concepto] [varchar](200) NULL,
	[trans_estado_id] [int] NOT NULL,
	[trans_mediop_id] [int] NOT NULL,
	[comercio_codigo] [int] NOT NULL,
	[usuario_identificacion] [varchar](20) NOT NULL,
 CONSTRAINT [PK_Trans] PRIMARY KEY CLUSTERED 
(
	[trans_codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trans_Estado]    Script Date: 25/12/2022 3:59:53 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trans_Estado](
	[trans_estado_id] [int] NOT NULL,
	[trans_estado_nombre] [varchar](50) NULL,
 CONSTRAINT [PK_Trans_Estado] PRIMARY KEY CLUSTERED 
(
	[trans_estado_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trans_Medio_Pago]    Script Date: 25/12/2022 3:59:53 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trans_Medio_Pago](
	[trans_mediop_id] [int] NOT NULL,
	[trans_mediop_nombre] [varchar](50) NULL,
 CONSTRAINT [PK_Trans_Medio_Pago] PRIMARY KEY CLUSTERED 
(
	[trans_mediop_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 25/12/2022 3:59:53 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[usuario_identificacion] [varchar](20) NOT NULL,
	[usuario_nombre] [varchar](100) NOT NULL,
	[usuario_email] [varchar](50) NOT NULL,
	[usuario_password] [varchar](100) NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[usuario_identificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Trans]  WITH CHECK ADD  CONSTRAINT [FK_Trans_Comercio] FOREIGN KEY([comercio_codigo])
REFERENCES [dbo].[Comercio] ([comercio_codigo])
GO
ALTER TABLE [dbo].[Trans] CHECK CONSTRAINT [FK_Trans_Comercio]
GO
ALTER TABLE [dbo].[Trans]  WITH CHECK ADD  CONSTRAINT [FK_Trans_Trans_Estado] FOREIGN KEY([trans_estado_id])
REFERENCES [dbo].[Trans_Estado] ([trans_estado_id])
GO
ALTER TABLE [dbo].[Trans] CHECK CONSTRAINT [FK_Trans_Trans_Estado]
GO
ALTER TABLE [dbo].[Trans]  WITH CHECK ADD  CONSTRAINT [FK_Trans_Trans_Medio_Pago] FOREIGN KEY([trans_mediop_id])
REFERENCES [dbo].[Trans_Medio_Pago] ([trans_mediop_id])
GO
ALTER TABLE [dbo].[Trans] CHECK CONSTRAINT [FK_Trans_Trans_Medio_Pago]
GO
ALTER TABLE [dbo].[Trans]  WITH CHECK ADD  CONSTRAINT [FK_Trans_Usuario] FOREIGN KEY([usuario_identificacion])
REFERENCES [dbo].[Usuario] ([usuario_identificacion])
GO
ALTER TABLE [dbo].[Trans] CHECK CONSTRAINT [FK_Trans_Usuario]
GO

INSERT INTO [dbo].[Trans_Medio_Pago] VALUES (32,'Tarjeta de Crédito')
GO
INSERT INTO [dbo].[Trans_Medio_Pago] VALUES (29,'PSE')
GO
INSERT INTO [dbo].[Trans_Medio_Pago] VALUES (41,'Gana')
GO
INSERT INTO [dbo].[Trans_Medio_Pago] VALUES (42,'Caja')
GO

INSERT INTO [dbo].[Trans_Estado] VALUES (1,'Aprobada')
GO
INSERT INTO [dbo].[Trans_Estado] VALUES (1000,'Rechazada')
GO
INSERT INTO [dbo].[Trans_Estado] VALUES (999,'Pendiente')
GO
INSERT INTO [dbo].[Trans_Estado] VALUES (1001,'Rechazada SR')
GO
