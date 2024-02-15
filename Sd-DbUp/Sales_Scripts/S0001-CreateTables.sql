create table SalesPerson(SalesPersonId int PRIMARY KEY IDENTITY(1,1) NOT NULL, FullName varchar(150) NOT NULL);

create table District(DistrictId int PRIMARY KEY IDENTITY(1,1) NOT NULL, DistrictName varchar(150) NOT NULL, 
	PrimarySalesId int NOT NULL,
	Constraint FK_District_SALES FOREIGN KEY (PrimarySalesId) REFERENCES SalesPerson(SalesPersonId)
	
	);


create table SecondarySalesPerson(SalesPersonId int NOT NULL,DistrictId int NOT NULL,
	Constraint PK_SecondarySalesPerson PRIMARY KEY (SalesPersonId,DistrictId),
	Constraint FK_SecondarySalesPerson_SALESPERSON FOREIGN KEY (SalesPersonId) REFERENCES SalesPerson(SalesPersonId),
	Constraint FK_SecondarySalesPerson_DISTRICT FOREIGN KEY (DistrictId) REFERENCES District(DistrictId)
	);
