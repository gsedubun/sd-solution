create table SalesPerson(sales_id int PRIMARY KEY IDENTITY(1,1) NOT NULL, full_name varchar(150) NOT NULL);

create table District(district_id int PRIMARY KEY IDENTITY(1,1) NOT NULL, district_name varchar(150) NOT NULL, 
	primary_sales_id int NOT NULL,
	Constraint FK_District_SALES FOREIGN KEY (primary_sales_id) REFERENCES SalesPerson(sales_id)
	
	);
create table SecondarySalesPerson(sales_id int NOT NULL,district_id int NOT NULL,
	Constraint PK_SecondarySalesPerson PRIMARY KEY (sales_id,district_id),
	Constraint FK_SecondarySalesPerson_SALESPERSON FOREIGN KEY (sales_id) REFERENCES SalesPerson(sales_id),
	Constraint FK_SecondarySalesPerson_DISTRICT FOREIGN KEY (district_id) REFERENCES District(district_id)
	);
