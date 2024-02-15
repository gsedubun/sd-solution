
-- insert data to the table

set identity_insert dbo.salesperson ON;
insert into SalesPerson(SalesPersonId,FullName) values(1,'Del'),(2,'Toni'),(3,'Daniel'),(4,'Jimi');
set identity_insert dbo.salesperson OFF;

set identity_insert dbo.district ON;
insert into District(DistrictId,DistrictName,PrimarySalesId) 
	values(1,'Jakarta',1),(2,'Surabaya',1),(3,'Jogjakarta',2),(4,'Denpasar',3);
set identity_insert dbo.district OFF;


set identity_insert dbo.store ON;
insert into Store(StoreId,StoreName,DistrictId) values(1,'Jakarta Pusat',1),(2,'Jakarta Barat',1),(3,'Jakarta Timur',1),(4,'Jakarta Selatan',1);
set identity_insert dbo.store OFF;
insert into secondarysalesperson values(2,1),(3,1),(4,4);
