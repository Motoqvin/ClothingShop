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

create table MethodType(
	[Id] int identity,
	[MethodId] int primary key,
	[MethodType] nvarchar(max)
)

insert into MethodType([MethodId], [MethodType])
values (1, 'GET'), (2, 'POST'), (3, 'DELETE'), (4, 'PUT')


create table HttpLog(
	[RequestId] uniqueidentifier primary key,
	[Url] nvarchar(max) not null,
	[RequestBody] nvarchar(max),
	[RequestHeaders] nvarchar(max),
	[MethodType] int foreign key references MethodType([MethodId]),
	[ResponseBody] nvarchar(max) null,
	[ResponseHeaders] nvarchar(max) null,
	[StatusCode] int,
	[CreationDateTime] datetime2,
	[EndDateTime] datetime2,
	[ClientIp] nvarchar(max)
)

select * from OrdersProducts