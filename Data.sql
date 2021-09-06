create database QuanLyQuanCafe
GO

use QuanLyQuanCafe
GO

--Food
--Table
--FoodCategory
--Account
--Bill

create table TableFood
(
	id  INT IDENTITY PRIMARY KEY,
	name nvarchar(100) not null,
	status nvarchar(100) not null default N'Trống',
)
GO
create table Account
(
 UserName nvarchar(100) PRIMARY KEY,
 DisplayName nvarchar(100) not null,
 PassWord nvarchar(1000) default 0 not null,
 Type INT not null default 0,
)
GO

create table FoodCategory
(
id INT IDENTITY PRIMARY KEY,
name nvarchar(100) not null,
)
GO

create table Food
(
id INT IDENTITY PRIMARY KEY,
name nvarchar(100) not null default N'Chưa đặt tên',
idCategory INT not null,
price FLOAT not null default 0,
foreign key (idCategory) references dbo.FoodCategory(id),
)
GO

create table Bill
(
id INT IDENTITY PRIMARY KEY,
DateCheckIn DATE not null,
DateCheckOut DATE,
idTable int not null,
status INT not null default 0,
foreign key(idTable) references dbo.TableFood(id),
)
GO
create table BillInfor
(
id INT IDENTITY PRIMARY KEY,
idBill INT not null,
idFood INT not null,
count INT not null default 0,
foreign key(idBill) references dbo.Bill(id),
foreign key(idFood) references dbo.Food(id),
)
GO


insert into dbo.Account(UserName, DisplayName, PassWord,Type) values (N'Boss',N'boss',N'1',1)
insert into dbo.Account(UserName, DisplayName, PassWord,Type) values (N'Staff',N'staff',N'1',0)

CREATE PROC USP_Login
@userName nvarchar(100), @passWord nvarchar(100)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName AND PassWord = @passWord
END
GO
create proc USP_GetAccountByUserName
@username nvarchar(100)
as
begin
		select * from dbo.Account where UserName = @username
end
go

exec dbo.USP_GetAccountByUserName @username = N'Boss'

 create proc USP_Login
@username nvarchar(100), @password nvarchar(100)
as
begin
		select * from dbo.Account where UserName = @username and PassWord = @password
end
go

 declare @i int = 0
 while @i<=10
 begin
	insert dbo.TableFood(name) values (N'Bàn' + cast(@i as nvarchar(100)))
	set @i = @i+1
 end

insert into dbo.TableFood(name, status) values (N'Bàn 1', N'Trống')
insert into dbo.TableFood(name, status) values (N'Bàn 2')

select * from TableFood

create proc USP_GetTableList
as select * from dbo.TableFood
go 
exec dbo.USP_GetTableList

update dbo.TableFood set status = N'Có Người' where id = 5



select * from Bill
go
select * from BillInfor
go
select * from Food
go
select * from FoodCategory
--ThemBan
 declare @i int = 0
 while @i<=10
 begin
	insert dbo.TableFood(name) values (N'Bàn' + cast(@i as nvarchar(100)))
	set @i = @i+1
 end
 --ThemCategory
Insert into FoodCategory (name) values (N'Hải sản ')
Insert into FoodCategory (name) values (N'Nông sản')
Insert into FoodCategory (name) values (N'Bò')
Insert into FoodCategory (name) values (N'Rau')
Insert into FoodCategory (name) values (N'Đồ uống')
 --ThemFood
 Insert into Food (name,idCategory,price) values (N'Canh tương hải sản', 1, 450000)
 Insert into Food (name,idCategory,price) values (N'Sốt gà xào cay', 2, 850000)
 Insert into Food (name,idCategory,price) values (N'Bò hầm niêu đá', 3, 550000)
 Insert into Food (name,idCategory,price) values (N'Salad ốc', 4, 250000)
 Insert into Food (name,idCategory,price) values (N'Soju', 5, 120000)
 Insert into Food (name,idCategory,price) values (N'Rượu gạo', 5, 180000)
 --ThemBill
 Insert into Bill(DateCheckIn,DateCheckOut,idTable,status) values (GETDATE(),null,5,0)
 Insert into Bill(DateCheckIn,DateCheckOut,idTable,status) values (GETDATE(),null,2,0)
 Insert into Bill(DateCheckIn,DateCheckOut,idTable,status) values (GETDATE(),GETDATE(),2,1)
 Insert into Bill(DateCheckIn,DateCheckOut,idTable,status) values (GETDATE(),null,6,0)
 Insert into Bill(DateCheckIn,DateCheckOut,idTable,status) values (GETDATE(),null,5,0)
 --ThemBillInfor
 Insert into BillInfor(idBill,idFood,count) values (3,5,1)
 Insert into BillInfor(idBill,idFood,count) values (4,2,3)
 Insert into BillInfor(idBill,idFood,count) values (1,1,2)
 Insert into BillInfor(idBill,idFood,count) values (3,4,5)
 Insert into BillInfor(idBill,idFood,count) values (3,1,1)


 delete BillInfor
 delete Bill
create proc USP_InsertBill
 @idTable int
 as
 begin
Insert into Bill(DateCheckIn,DateCheckOut,idTable,status,Discount) values (GETDATE(),null,@idTable,0,0)
 end
 go
create proc USP_InsertBillInfo
 @idBill int, @idFood int, @count int
 as
 
BEGIN

	DECLARE @isExitsBillInfo INT
	DECLARE @foodCount INT = 1
	
	SELECT @isExitsBillInfo = id, @foodCount = b.count 
	FROM dbo.BillInfor AS b 
	WHERE idBill = @idBill AND idFood = @idFood

	IF (@isExitsBillInfo > 0)
	BEGIN
		DECLARE @newCount INT = @foodCount + @count
		IF (@newCount > 0)
			UPDATE dbo.BillInfor	SET count = @foodCount + @count WHERE idFood = @idFood
		ELSE
			DELETE dbo.BillInfor WHERE idBill = @idBill AND idFood = @idFood
	END
	ELSE
	BEGIN
		INSERT	dbo.BillInfor
        ( idBill, idFood, count )
		VALUES  ( @idBill, -- idBill - int
          @idFood, -- idFood - int
          @count  -- count - int
          )
	END
END
GO
alter trigger UTG_UpdateBillInfo
	on BillInfor for insert, update
	as 
	begin
	DECLARE @idBill INT
	
		SELECT @idBill = idBill FROM Inserted
	
		DECLARE @idTable INT
	
		SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill AND status = 0
		declare @count int
		select @count=count(*) from dbo.BillInfor where  idBill = @idBill
	if(@count>0)
		UPDATE dbo.TableFood SET status = N'Có người' WHERE id = @idTable
		else
		UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable

	end
	go



create trigger UTG_UpdateBill
on Bill for Update
as 
begin
DECLARE @idBill INT
	
	SELECT @idBill = id FROM Inserted	
	
	DECLARE @idTable INT
	
	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill
	
	DECLARE @count int = 0
	
	SELECT @count = COUNT(*) FROM dbo.Bill WHERE idTable = @idTable AND status = 0
	
	IF (@count = 0)
		UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable
		end
go
 

alter proc USP_SwitchTable
@idTable1 int, @idTable2 int
as
BEGIN

	DECLARE @idFirstBill int
	DECLARE @idSeconrdBill INT
	
	DECLARE @isFirstTablEmty INT = 1
	DECLARE @isSecondTablEmty INT = 1
	
	
	SELECT @idSeconrdBill = id FROM dbo.Bill WHERE idTable = @idTable2 AND status = 0
	SELECT @idFirstBill = id FROM dbo.Bill WHERE idTable = @idTable1 AND status = 0
	
	PRINT @idFirstBill
	PRINT @idSeconrdBill
	PRINT '-----------'
	
	IF (@idFirstBill IS NULL)
	BEGIN
		PRINT '0000001'
		INSERT dbo.Bill
		        ( DateCheckIn ,
		          DateCheckOut ,
		          idTable ,
		          status
		        )
		VALUES  ( GETDATE() , -- DateCheckIn - date
		          NULL , -- DateCheckOut - date
		          @idTable1 , -- idTable - int
		          0  -- status - int
		        )
		        
		SELECT @idFirstBill = MAX(id) FROM dbo.Bill WHERE idTable = @idTable1 AND status = 0
		
	END
	
	SELECT @isFirstTablEmty = COUNT(*) FROM dbo.BillInfor WHERE idBill = @idFirstBill
	
	PRINT @idFirstBill
	PRINT @idSeconrdBill
	PRINT '-----------'
	
	IF (@idSeconrdBill IS NULL)
	BEGIN
		PRINT '0000002'
		INSERT dbo.Bill
		        ( DateCheckIn ,
		          DateCheckOut ,
		          idTable ,
		          status
		        )
		VALUES  ( GETDATE() , -- DateCheckIn - date
		          NULL , -- DateCheckOut - date
		          @idTable2 , -- idTable - int
		          0  -- status - int
		        )
		SELECT @idSeconrdBill = MAX(id) FROM dbo.Bill WHERE idTable = @idTable2 AND status = 0
		
	END
	
	SELECT @isSecondTablEmty = COUNT(*) FROM dbo.BillInfor WHERE idBill = @idSeconrdBill
	
	PRINT @idFirstBill
	PRINT @idSeconrdBill
	PRINT '-----------'

	SELECT id INTO IDBillInfoTable FROM dbo.BillInfor WHERE idBill = @idSeconrdBill
	
	UPDATE dbo.BillInfor SET idBill = @idSeconrdBill WHERE idBill = @idFirstBill
	
	UPDATE dbo.BillInfor SET idBill = @idFirstBill WHERE id IN (SELECT * FROM IDBillInfoTable)
	
	DROP TABLE IDBillInfoTable
	
	IF (@isFirstTablEmty = 0)
		UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable2
		
	IF (@isSecondTablEmty= 0)
		UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable1
END
go

alter table Bill add totalPrice float

delete BillInfor
delete Bill

create proc USP_GetListBillByDate
@checkIn date, @checkOut date
as
begin
	select t.name as [Tên bàn], DateCheckIn as[Ngày vào],DateCheckOut as [Ngày ra],Discount as [Giảm giá],  b.totalPrice as [Tổng tiền]
from Bill as b,TableFood as t
where DateCheckIn >= @checkIn and DateCheckOut <= @checkOut and b.status = 1
and t.id = b.idTable

end
go



select t.name, b.totalPrice, DateCheckIn,DateCheckOut,Discount 
from Bill as b,TableFood as t
where DateCheckIn >= '20210801' and DateCheckOut <= '20210816' and b.status = 1
and t.id = b.idTable


alter proc USP_UpdateAccount
@userName nvarchar(100),
@displayName nvarchar(100),
@password nvarchar(100),
@newPassword nvarchar(100)
as
begin
	declare @isRightPass int = 0
	select @isRightPass = COUNT(*) from dbo.Account where UserName = @userName and PassWord = @password

	if(@isRightPass = 1 )
	begin
	if(@newPassword = null or @newPassword = '')
	begin
	update dbo.Account set DisplayName = @displayName where UserName = @userName
	end
	else
		update dbo.Account set DisplayName = @displayName, PassWord = @newPassword where UserName = @userName
	end
end
go

create trigger UTG_DeleteBillInfo
on dbo.BillInfor for delete
as
begin
	declare @idBillInfo int
	
	declare @idBill int
	select @idBillInfo = id, @idBill = deleted.idBill from deleted
	
	declare @idTable int
	select @idTable = idTable from dbo.Bill where id = @idBill 

	 declare @count int = 0

	 select @count =count(*) from dbo.BillInfor as bi, dbo.Bill as b where b.id = bi.idBill and b.id = @idBill and b.status= 0 
	 if (@count = 0)
	 update TableFood set status = N'Trống' where id = @idTable
end
go

CREATE FUNCTION [dbo].[fuConvertToUnsign1] ( @strInput NVARCHAR(4000) ) RETURNS NVARCHAR(4000) AS BEGIN IF @strInput IS NULL RETURN @strInput IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET @COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput END
go

create proc USP_GetListBillByDateAndPage
@checkIn date, @checkOut date, @page int
as
begin
	DECLARE @pageRows INT = 10
	DECLARE @selectRows INT = @pageRows
	DECLARE @exceptRows INT = (@page - 1) * @pageRows
	
	;WITH BillShow AS( SELECT b.ID, t.name AS [Tên bàn], b.totalPrice AS [Tổng tiền], DateCheckIn AS [Ngày vào], DateCheckOut AS [Ngày ra], discount AS [Giảm giá]
	FROM dbo.Bill AS b,dbo.TableFood AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1
	AND t.id = b.idTable)
	
	SELECT TOP (@selectRows) * FROM BillShow WHERE id NOT IN (SELECT TOP (@exceptRows) id FROM BillShow)
end
GO

create proc USP_GetNumBillByDate
@checkIn date, @checkOut date
as
begin
	select count(*)
from Bill as b,TableFood as t
where DateCheckIn >= @checkIn and DateCheckOut <= @checkOut and b.status = 1
and t.id = b.idTable
end
go