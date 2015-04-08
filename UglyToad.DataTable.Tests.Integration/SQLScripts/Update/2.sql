CREATE PROCEDURE uspGetAllAreas
AS 
    SET NOCOUNT ON;
    SELECT	AreaId,
			Area_Name,
			IsActive,
			CreatedDate
	FROM	Area
GO

CREATE PROCEDURE uspGetAllStatuses
AS 
    SET NOCOUNT ON;
    SELECT	Id,
			Description,
			IsPublic
	FROM	Status
GO