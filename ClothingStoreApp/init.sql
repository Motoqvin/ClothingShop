select *
from Orders

drop table Orders


create table Products(
	[Id] int primary key identity,
	[Name] nvarchar(max),
	[Description] nvarchar(max),
	[Price] money
)
go

create table Orders(
	[Id] int primary key identity,
	[Date] datetime2,
	[TotalPrice] decimal
)
go

create table OrdersProducts(
	[Id] int primary key identity,
	[OrderId] int foreign key references Orders([Id]),
	[ProductId] int foreign key references Products([Id])
)

select * from OrdersProducts