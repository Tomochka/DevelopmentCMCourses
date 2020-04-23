use Product_Category
go

select dbo.Product.product, dbo.Product_Category.category 
from dbo.Product left join dbo.Product_Category ON dbo.Product.id = dbo.Product_Category.product
