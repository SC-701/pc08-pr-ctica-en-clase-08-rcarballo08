-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE ObtenerProducto
	-- Add the parameters for the stored procedure here
@Id uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT 
        p.[Id], p.[Nombre], p.[Descripcion], p.[Precio], p.[Stock], p.[CodigoBarras], sc.[Nombre] AS [SubCategoria], c.[Nombre] AS [Categoria]
    FROM [dbo].[Producto] p
    INNER JOIN [dbo].[SubCategorias] sc ON p.[IdSubCategoria] = sc.[Id]
    INNER JOIN [dbo].[Categorias] c ON sc.[IdCategoria] = c.[Id]
    WHERE p.[Id] = @Id;
END