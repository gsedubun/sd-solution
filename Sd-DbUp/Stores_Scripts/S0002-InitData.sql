
-- insert data to the table


set identity_insert dbo.store ON;
insert into Store(StoreId,StoreName,DistrictId) values(1,'Jakarta Pusat',1),(2,'Jakarta Barat',1),(3,'Jakarta Timur',1),(4,'Jakarta Selatan',1);
set identity_insert dbo.store OFF;


