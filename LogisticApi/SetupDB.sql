CREATE DATABASE SampleLogistic;
GO

USE SampleLogistic;

CREATE TABLE Documents(
	number UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	actionType BIT NOT NULL,
	createDate DATE DEFAULT GETDATE(),
	updateDate DATE DEFAULT GETDATE(),
	contragentName VARCHAR(100),
	storage VARCHAR(30) NOT NULL,
	priceNetto MONEY,
	vat MONEY,
	priceBrutto MONEY,
	isDeleted BIT DEFAULT 0
);

CREATE TABLE Articles(
	id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	documentNumber UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Documents(number),
	actionType BIT NOT NULL,
	positionName VARCHAR(60) NOT NULL,
	positionCount INT CHECK (positionCount>=0),
	unit VARCHAR(80) NOT NULL,
	priceNetto MONEY,
	vat MONEY,
	priceBrutto MONEY,
	storage VARCHAR (30) NOT NULL,
	isDeleted BIT DEFAULT 0
);

GO

insert into dbo.Documents (number, actionType, contragentName, storage, priceNetto, vat, priceBrutto) values ('7af64e5a-3010-413e-bfad-c105c3687e22', 1, 'FishMeatMarket', 'WarshawMarket', 1800, 180, 1980)
insert into dbo.Documents (number, actionType, contragentName, storage, priceNetto, vat, priceBrutto) values ('117d2dd3-6d5f-4c86-a77f-43ab3e5a1497', 1, 'FishMeatMarket', 'GdanskMarket', 1700, 170, 1870)

insert into dbo.Articles (id, documentNumber, actionType, positionName, positionCount, unit, priceNetto, vat, priceBrutto, storage) values ('a284246e-3e9d-4d95-ad71-0f0cda7d4501', '7af64e5a-3010-413e-bfad-c105c3687e22', 1, 'Fish', 10, 'box', 1000, 100, 1100, 'WarshawMarket')
insert into dbo.Articles (id, documentNumber, actionType, positionName, positionCount, unit, priceNetto, vat, priceBrutto, storage) values ('cec1debb-1904-4d61-8634-df3c546632e9', '117d2dd3-6d5f-4c86-a77f-43ab3e5a1497', 1, 'Fish', 5, 'box', 500, 50, 550, 'GdanskMarket')
insert into dbo.Articles (id, documentNumber, actionType, positionName, positionCount, unit, priceNetto, vat, priceBrutto, storage) values ('6f82bb83-44af-4cbd-bffd-1d31d86f6b43', '7af64e5a-3010-413e-bfad-c105c3687e22', 1, 'Meat', 4, 'box', 800, 80, 880, 'WarshawMarket')
insert into dbo.Articles (id, documentNumber, actionType, positionName, positionCount, unit, priceNetto, vat, priceBrutto, storage) values ('61797a05-60aa-4ec6-bd13-cdbd09b57ea0', '117d2dd3-6d5f-4c86-a77f-43ab3e5a1497', 1, 'Meat', 6, 'box', 1200, 120, 1320, 'GdanskMarket')
