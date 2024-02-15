
-- insert data to the table

set identity_insert dbo.salesperson ON;
insert into SalesPerson(SalesPersonId,FullName) values(1,'Del'),(2,'Toni'),(3,'Daniel'),(4,'Jimi');
set identity_insert dbo.salesperson OFF;

set identity_insert dbo.district ON;
insert into District(DistrictId,DistrictName,PrimarySalesId) 
	values(1,'Jakarta',1),(2,'Surabaya',1),(3,'Jogjakarta',2),(4,'Denpasar',3);
set identity_insert dbo.district OFF;



insert into secondarysalesperson values(2,1),(3,1),(4,4);
