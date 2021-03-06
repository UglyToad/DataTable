﻿CREATE TABLE Status
(
	Id INT IDENTITY(1,1) NOT NULL,
	Description NVARCHAR(25) NOT NULL,
	IsPublic BIT CONSTRAINT DF_Status_IsPublic DEFAULT 0 NOT NULL,
	CONSTRAINT PK_Status PRIMARY KEY (Id)
)
GO;

CREATE TABLE Area
(
	AreaId INT IDENTITY(1,1) NOT NULL,
	Area_Name VARCHAR(52) NULL,
	IsActive INT CONSTRAINT DF_Area_IsActive DEFAULT 0 NOT NULL,
	CreatedDate DATETIME2(0) CONSTRAINT DF_Area_CreatedDate NOT NULL, 
	CONSTRAINT PK_Area PRIMARY KEY (AreaId)
)
GO;