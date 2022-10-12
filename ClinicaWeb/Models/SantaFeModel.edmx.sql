
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/11/2022 17:13:35
-- Generated from EDMX file: C:\Users\DELL 7450\source\repos\ClinicaWeb\ClinicaWeb\Models\SantaFeModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ClinicaWeb];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_DepartamentoMunicipio]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Municipios] DROP CONSTRAINT [FK_DepartamentoMunicipio];
GO
IF OBJECT_ID(N'[dbo].[FK_MunicipioPaciente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Pacientes] DROP CONSTRAINT [FK_MunicipioPaciente];
GO
IF OBJECT_ID(N'[dbo].[FK_SexoPaciente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Pacientes] DROP CONSTRAINT [FK_SexoPaciente];
GO
IF OBJECT_ID(N'[dbo].[FK_CargoEmpleado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Empleados] DROP CONSTRAINT [FK_CargoEmpleado];
GO
IF OBJECT_ID(N'[dbo].[FK_MedicoOrden]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ordenes] DROP CONSTRAINT [FK_MedicoOrden];
GO
IF OBJECT_ID(N'[dbo].[FK_PacienteOrden]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ordenes] DROP CONSTRAINT [FK_PacienteOrden];
GO
IF OBJECT_ID(N'[dbo].[FK_EmpleadoOrden]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ordenes] DROP CONSTRAINT [FK_EmpleadoOrden];
GO
IF OBJECT_ID(N'[dbo].[FK_OrdenDetalleOrden]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DetallesOrdenes] DROP CONSTRAINT [FK_OrdenDetalleOrden];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoriaExamen]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Examenes] DROP CONSTRAINT [FK_CategoriaExamen];
GO
IF OBJECT_ID(N'[dbo].[FK_ExamenDetalleOrden]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DetallesOrdenes] DROP CONSTRAINT [FK_ExamenDetalleOrden];
GO
IF OBJECT_ID(N'[dbo].[FK_ExamenExamenParametro]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExamenesParametros] DROP CONSTRAINT [FK_ExamenExamenParametro];
GO
IF OBJECT_ID(N'[dbo].[FK_ParametroExamenParametro]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExamenesParametros] DROP CONSTRAINT [FK_ParametroExamenParametro];
GO
IF OBJECT_ID(N'[dbo].[FK_ParametroReactivo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reactivos] DROP CONSTRAINT [FK_ParametroReactivo];
GO
IF OBJECT_ID(N'[dbo].[FK_DetalleOrdenResultado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Resultados] DROP CONSTRAINT [FK_DetalleOrdenResultado];
GO
IF OBJECT_ID(N'[dbo].[FK_ResultadoDetalleResultado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DetallesResultados] DROP CONSTRAINT [FK_ResultadoDetalleResultado];
GO
IF OBJECT_ID(N'[dbo].[FK_ParametroDetalleResultado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DetallesResultados] DROP CONSTRAINT [FK_ParametroDetalleResultado];
GO
IF OBJECT_ID(N'[dbo].[FK_OrdenDetalleFactura]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DetallesFacturas] DROP CONSTRAINT [FK_OrdenDetalleFactura];
GO
IF OBJECT_ID(N'[dbo].[FK_FacturaDetalleFactura]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DetallesFacturas] DROP CONSTRAINT [FK_FacturaDetalleFactura];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Departamentos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Departamentos];
GO
IF OBJECT_ID(N'[dbo].[Municipios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Municipios];
GO
IF OBJECT_ID(N'[dbo].[Pacientes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pacientes];
GO
IF OBJECT_ID(N'[dbo].[Sexos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sexos];
GO
IF OBJECT_ID(N'[dbo].[Categorias]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categorias];
GO
IF OBJECT_ID(N'[dbo].[Examenes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Examenes];
GO
IF OBJECT_ID(N'[dbo].[ExamenesParametros]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExamenesParametros];
GO
IF OBJECT_ID(N'[dbo].[DetallesOrdenes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DetallesOrdenes];
GO
IF OBJECT_ID(N'[dbo].[Ordenes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ordenes];
GO
IF OBJECT_ID(N'[dbo].[Medicos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Medicos];
GO
IF OBJECT_ID(N'[dbo].[DetallesFacturas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DetallesFacturas];
GO
IF OBJECT_ID(N'[dbo].[Empleados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Empleados];
GO
IF OBJECT_ID(N'[dbo].[Facturas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Facturas];
GO
IF OBJECT_ID(N'[dbo].[Resultados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Resultados];
GO
IF OBJECT_ID(N'[dbo].[DetallesResultados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DetallesResultados];
GO
IF OBJECT_ID(N'[dbo].[Parametros]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Parametros];
GO
IF OBJECT_ID(N'[dbo].[Reactivos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reactivos];
GO
IF OBJECT_ID(N'[dbo].[CargoSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CargoSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Departamentos'
CREATE TABLE [dbo].[Departamentos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre_dpto] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Municipios'
CREATE TABLE [dbo].[Municipios] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre_mun] nvarchar(max)  NOT NULL,
    [DepartamentoId] int  NOT NULL
);
GO

-- Creating table 'Pacientes'
CREATE TABLE [dbo].[Pacientes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre_pac] nvarchar(max)  NOT NULL,
    [Edad] int  NOT NULL,
    [Correo] nvarchar(max)  NULL,
    [Celular] nvarchar(max)  NULL,
    [Direccion] nvarchar(max)  NOT NULL,
    [MunicipioId] int  NOT NULL,
    [SexoId] int  NOT NULL
);
GO

-- Creating table 'Sexos'
CREATE TABLE [dbo].[Sexos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TipoSexo] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Categorias'
CREATE TABLE [dbo].[Categorias] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tipo] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Examenes'
CREATE TABLE [dbo].[Examenes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre_exa] nvarchar(max)  NOT NULL,
    [Precio] float  NOT NULL,
    [CategoriaId] int  NOT NULL
);
GO

-- Creating table 'ExamenesParametros'
CREATE TABLE [dbo].[ExamenesParametros] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ExamenId] int  NOT NULL,
    [ParametroId] int  NOT NULL
);
GO

-- Creating table 'DetallesOrdenes'
CREATE TABLE [dbo].[DetallesOrdenes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Cantidad] nvarchar(max)  NOT NULL,
    [FechaFinal] nvarchar(max)  NOT NULL,
    [Precio] float  NULL,
    [Estado] nvarchar(max)  NOT NULL,
    [OrdenId] int  NOT NULL,
    [ExamenId] int  NOT NULL
);
GO

-- Creating table 'Ordenes'
CREATE TABLE [dbo].[Ordenes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Cod_Orden] nvarchar(max)  NOT NULL,
    [Fecha] nvarchar(max)  NOT NULL,
    [Lugar] nvarchar(max)  NOT NULL,
    [MedicoId] int  NOT NULL,
    [PacienteId] int  NOT NULL,
    [EmpleadoId] int  NOT NULL,
    [Estado] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Medicos'
CREATE TABLE [dbo].[Medicos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NombreMed] nvarchar(max)  NOT NULL,
    [Celular] nvarchar(max)  NULL
);
GO

-- Creating table 'DetallesFacturas'
CREATE TABLE [dbo].[DetallesFacturas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Cantidad] float  NOT NULL,
    [Descripcion] nvarchar(max)  NULL,
    [PrecioUnitario] float  NOT NULL,
    [Subtotal] float  NOT NULL,
    [OrdenId] int  NOT NULL,
    [FacturaId] int  NOT NULL
);
GO

-- Creating table 'Empleados'
CREATE TABLE [dbo].[Empleados] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nom_Empleado] nvarchar(max)  NOT NULL,
    [User] nvarchar(max)  NOT NULL,
    [CargoId] int  NOT NULL
);
GO

-- Creating table 'Facturas'
CREATE TABLE [dbo].[Facturas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FechaFac] nvarchar(max)  NOT NULL,
    [CodFactura] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Resultados'
CREATE TABLE [dbo].[Resultados] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Descripcion] nvarchar(max)  NULL,
    [DetalleOrdenId] int  NOT NULL
);
GO

-- Creating table 'DetallesResultados'
CREATE TABLE [dbo].[DetallesResultados] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ExamenResultado] nvarchar(max)  NOT NULL,
    [Observacion] nvarchar(max)  NULL,
    [ResultadoId] int  NOT NULL,
    [ParametroId] int  NOT NULL
);
GO

-- Creating table 'Parametros'
CREATE TABLE [dbo].[Parametros] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nom_Parametro] nvarchar(max)  NOT NULL,
    [ReactivoId] int  NOT NULL
);
GO

-- Creating table 'Reactivos'
CREATE TABLE [dbo].[Reactivos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Maximo] nvarchar(max)  NULL,
    [Minimo] nvarchar(max)  NULL,
    [Densidad] nvarchar(max)  NULL,
    [ValoresNormales] nvarchar(max)  NULL,
    [Periodo] nvarchar(max)  NULL
);
GO

-- Creating table 'CargoSet'
CREATE TABLE [dbo].[CargoSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Departamentos'
ALTER TABLE [dbo].[Departamentos]
ADD CONSTRAINT [PK_Departamentos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Municipios'
ALTER TABLE [dbo].[Municipios]
ADD CONSTRAINT [PK_Municipios]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Pacientes'
ALTER TABLE [dbo].[Pacientes]
ADD CONSTRAINT [PK_Pacientes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Sexos'
ALTER TABLE [dbo].[Sexos]
ADD CONSTRAINT [PK_Sexos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Categorias'
ALTER TABLE [dbo].[Categorias]
ADD CONSTRAINT [PK_Categorias]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Examenes'
ALTER TABLE [dbo].[Examenes]
ADD CONSTRAINT [PK_Examenes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ExamenesParametros'
ALTER TABLE [dbo].[ExamenesParametros]
ADD CONSTRAINT [PK_ExamenesParametros]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DetallesOrdenes'
ALTER TABLE [dbo].[DetallesOrdenes]
ADD CONSTRAINT [PK_DetallesOrdenes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Ordenes'
ALTER TABLE [dbo].[Ordenes]
ADD CONSTRAINT [PK_Ordenes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Medicos'
ALTER TABLE [dbo].[Medicos]
ADD CONSTRAINT [PK_Medicos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DetallesFacturas'
ALTER TABLE [dbo].[DetallesFacturas]
ADD CONSTRAINT [PK_DetallesFacturas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Empleados'
ALTER TABLE [dbo].[Empleados]
ADD CONSTRAINT [PK_Empleados]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Facturas'
ALTER TABLE [dbo].[Facturas]
ADD CONSTRAINT [PK_Facturas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Resultados'
ALTER TABLE [dbo].[Resultados]
ADD CONSTRAINT [PK_Resultados]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DetallesResultados'
ALTER TABLE [dbo].[DetallesResultados]
ADD CONSTRAINT [PK_DetallesResultados]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Parametros'
ALTER TABLE [dbo].[Parametros]
ADD CONSTRAINT [PK_Parametros]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Reactivos'
ALTER TABLE [dbo].[Reactivos]
ADD CONSTRAINT [PK_Reactivos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CargoSet'
ALTER TABLE [dbo].[CargoSet]
ADD CONSTRAINT [PK_CargoSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [DepartamentoId] in table 'Municipios'
ALTER TABLE [dbo].[Municipios]
ADD CONSTRAINT [FK_DepartamentoMunicipio]
    FOREIGN KEY ([DepartamentoId])
    REFERENCES [dbo].[Departamentos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartamentoMunicipio'
CREATE INDEX [IX_FK_DepartamentoMunicipio]
ON [dbo].[Municipios]
    ([DepartamentoId]);
GO

-- Creating foreign key on [MunicipioId] in table 'Pacientes'
ALTER TABLE [dbo].[Pacientes]
ADD CONSTRAINT [FK_MunicipioPaciente]
    FOREIGN KEY ([MunicipioId])
    REFERENCES [dbo].[Municipios]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MunicipioPaciente'
CREATE INDEX [IX_FK_MunicipioPaciente]
ON [dbo].[Pacientes]
    ([MunicipioId]);
GO

-- Creating foreign key on [SexoId] in table 'Pacientes'
ALTER TABLE [dbo].[Pacientes]
ADD CONSTRAINT [FK_SexoPaciente]
    FOREIGN KEY ([SexoId])
    REFERENCES [dbo].[Sexos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SexoPaciente'
CREATE INDEX [IX_FK_SexoPaciente]
ON [dbo].[Pacientes]
    ([SexoId]);
GO

-- Creating foreign key on [CargoId] in table 'Empleados'
ALTER TABLE [dbo].[Empleados]
ADD CONSTRAINT [FK_CargoEmpleado]
    FOREIGN KEY ([CargoId])
    REFERENCES [dbo].[CargoSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CargoEmpleado'
CREATE INDEX [IX_FK_CargoEmpleado]
ON [dbo].[Empleados]
    ([CargoId]);
GO

-- Creating foreign key on [MedicoId] in table 'Ordenes'
ALTER TABLE [dbo].[Ordenes]
ADD CONSTRAINT [FK_MedicoOrden]
    FOREIGN KEY ([MedicoId])
    REFERENCES [dbo].[Medicos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MedicoOrden'
CREATE INDEX [IX_FK_MedicoOrden]
ON [dbo].[Ordenes]
    ([MedicoId]);
GO

-- Creating foreign key on [PacienteId] in table 'Ordenes'
ALTER TABLE [dbo].[Ordenes]
ADD CONSTRAINT [FK_PacienteOrden]
    FOREIGN KEY ([PacienteId])
    REFERENCES [dbo].[Pacientes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PacienteOrden'
CREATE INDEX [IX_FK_PacienteOrden]
ON [dbo].[Ordenes]
    ([PacienteId]);
GO

-- Creating foreign key on [EmpleadoId] in table 'Ordenes'
ALTER TABLE [dbo].[Ordenes]
ADD CONSTRAINT [FK_EmpleadoOrden]
    FOREIGN KEY ([EmpleadoId])
    REFERENCES [dbo].[Empleados]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmpleadoOrden'
CREATE INDEX [IX_FK_EmpleadoOrden]
ON [dbo].[Ordenes]
    ([EmpleadoId]);
GO

-- Creating foreign key on [OrdenId] in table 'DetallesOrdenes'
ALTER TABLE [dbo].[DetallesOrdenes]
ADD CONSTRAINT [FK_OrdenDetalleOrden]
    FOREIGN KEY ([OrdenId])
    REFERENCES [dbo].[Ordenes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrdenDetalleOrden'
CREATE INDEX [IX_FK_OrdenDetalleOrden]
ON [dbo].[DetallesOrdenes]
    ([OrdenId]);
GO

-- Creating foreign key on [CategoriaId] in table 'Examenes'
ALTER TABLE [dbo].[Examenes]
ADD CONSTRAINT [FK_CategoriaExamen]
    FOREIGN KEY ([CategoriaId])
    REFERENCES [dbo].[Categorias]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoriaExamen'
CREATE INDEX [IX_FK_CategoriaExamen]
ON [dbo].[Examenes]
    ([CategoriaId]);
GO

-- Creating foreign key on [ExamenId] in table 'DetallesOrdenes'
ALTER TABLE [dbo].[DetallesOrdenes]
ADD CONSTRAINT [FK_ExamenDetalleOrden]
    FOREIGN KEY ([ExamenId])
    REFERENCES [dbo].[Examenes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExamenDetalleOrden'
CREATE INDEX [IX_FK_ExamenDetalleOrden]
ON [dbo].[DetallesOrdenes]
    ([ExamenId]);
GO

-- Creating foreign key on [ExamenId] in table 'ExamenesParametros'
ALTER TABLE [dbo].[ExamenesParametros]
ADD CONSTRAINT [FK_ExamenExamenParametro]
    FOREIGN KEY ([ExamenId])
    REFERENCES [dbo].[Examenes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExamenExamenParametro'
CREATE INDEX [IX_FK_ExamenExamenParametro]
ON [dbo].[ExamenesParametros]
    ([ExamenId]);
GO

-- Creating foreign key on [ParametroId] in table 'ExamenesParametros'
ALTER TABLE [dbo].[ExamenesParametros]
ADD CONSTRAINT [FK_ParametroExamenParametro]
    FOREIGN KEY ([ParametroId])
    REFERENCES [dbo].[Parametros]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ParametroExamenParametro'
CREATE INDEX [IX_FK_ParametroExamenParametro]
ON [dbo].[ExamenesParametros]
    ([ParametroId]);
GO

-- Creating foreign key on [DetalleOrdenId] in table 'Resultados'
ALTER TABLE [dbo].[Resultados]
ADD CONSTRAINT [FK_DetalleOrdenResultado]
    FOREIGN KEY ([DetalleOrdenId])
    REFERENCES [dbo].[DetallesOrdenes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DetalleOrdenResultado'
CREATE INDEX [IX_FK_DetalleOrdenResultado]
ON [dbo].[Resultados]
    ([DetalleOrdenId]);
GO

-- Creating foreign key on [ResultadoId] in table 'DetallesResultados'
ALTER TABLE [dbo].[DetallesResultados]
ADD CONSTRAINT [FK_ResultadoDetalleResultado]
    FOREIGN KEY ([ResultadoId])
    REFERENCES [dbo].[Resultados]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ResultadoDetalleResultado'
CREATE INDEX [IX_FK_ResultadoDetalleResultado]
ON [dbo].[DetallesResultados]
    ([ResultadoId]);
GO

-- Creating foreign key on [ParametroId] in table 'DetallesResultados'
ALTER TABLE [dbo].[DetallesResultados]
ADD CONSTRAINT [FK_ParametroDetalleResultado]
    FOREIGN KEY ([ParametroId])
    REFERENCES [dbo].[Parametros]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ParametroDetalleResultado'
CREATE INDEX [IX_FK_ParametroDetalleResultado]
ON [dbo].[DetallesResultados]
    ([ParametroId]);
GO

-- Creating foreign key on [OrdenId] in table 'DetallesFacturas'
ALTER TABLE [dbo].[DetallesFacturas]
ADD CONSTRAINT [FK_OrdenDetalleFactura]
    FOREIGN KEY ([OrdenId])
    REFERENCES [dbo].[Ordenes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrdenDetalleFactura'
CREATE INDEX [IX_FK_OrdenDetalleFactura]
ON [dbo].[DetallesFacturas]
    ([OrdenId]);
GO

-- Creating foreign key on [FacturaId] in table 'DetallesFacturas'
ALTER TABLE [dbo].[DetallesFacturas]
ADD CONSTRAINT [FK_FacturaDetalleFactura]
    FOREIGN KEY ([FacturaId])
    REFERENCES [dbo].[Facturas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FacturaDetalleFactura'
CREATE INDEX [IX_FK_FacturaDetalleFactura]
ON [dbo].[DetallesFacturas]
    ([FacturaId]);
GO

-- Creating foreign key on [ReactivoId] in table 'Parametros'
ALTER TABLE [dbo].[Parametros]
ADD CONSTRAINT [FK_ReactivoParametro]
    FOREIGN KEY ([ReactivoId])
    REFERENCES [dbo].[Reactivos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReactivoParametro'
CREATE INDEX [IX_FK_ReactivoParametro]
ON [dbo].[Parametros]
    ([ReactivoId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------