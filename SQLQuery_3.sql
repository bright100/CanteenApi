USE LWCanteenDb;

-- Represents an actual Employee
CREATE TABLE Employee (
    EmployeeId UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
    FullName NVARCHAR(225) NOT NULL,
    Email NVARCHAR(225) UNIQUE NOT NULL,
    PasswordHash BINARY(64),
    Bio NVARCHAR(500),
    UserImageUrl NVARCHAR(500),
    Roles NVARCHAR(50) DEFAULT 'Employee' CHECK(Roles IN ('Employee', 'Admin', 'Vendor')),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    RowVersion TIMESTAMP NOT NULL
);

-- Represents a Leadway Branch
CREATE TABLE Branch (
    BranchId UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
    BranchName NVARCHAR(225) NOT NULL DEFAULT 'Leadway - Head Office',
    BranchAddress NVARCHAR(500) NOT NULL DEFAULT '121/123 Funsho Williams Avenue, Iponri Surulere.',
    State NVARCHAR(50) NOT NULL DEFAULT 'Lagos State',
    LGA NVARCHAR(100) NOT NULL DEFAULT 'Surulere - Iponri',
    Note NVARCHAR(500),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    RowVersion TIMESTAMP NOT NULL
);

-- Represents a food vendor
CREATE TABLE Vendor (
    VendorId UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
    Email NVARCHAR(225) NOT NULL UNIQUE,
    VendorName NVARCHAR(225) NOT NULL UNIQUE,
    Phone NVARCHAR(20),
    Address NVARCHAR(225),
    Status NVARCHAR(20) DEFAULT 'Active' CHECK (Status IN ('Active', 'Inactive', 'Suspended')),
    PasswordHash BINARY(64),
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    AccountNumber NVARCHAR(20),
    BankName NVARCHAR(50),
    AccountName NVARCHAR(225),
    BranchId UNIQUEIDENTIFIER,
    CanteenStatus NVARCHAR(20) DEFAULT 'Open' CHECK (CanteenStatus IN ('Open', 'Closed')),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    VendorsContractForm NVARCHAR(225) NOT NULL,
    LetterOfAgreement NVARCHAR(225) NOT NULL,
    CompanyRegistrationDocument NVARCHAR(225) NOT NULL,
    Others NVARCHAR(225) NULL,
    RowVersion TIMESTAMP NOT NULL,
    FOREIGN KEY (BranchId) REFERENCES Branch(BranchId) ON DELETE CASCADE
);

-- Represents the food vendors Inventory i.e list of food items available for order by that vendor
-- This is actually usually updated by the admin.
CREATE TABLE Inventory (
    InventoryId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID() NOT NULL,
    VendorId UNIQUEIDENTIFIER NOT NULL UNIQUE,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    RowVersion TIMESTAMP NOT NULL,
    FOREIGN KEY (VendorId) REFERENCES Vendor(VendorId) ON DELETE CASCADE
);

-- Represents users token for authentication
CREATE TABLE UserRefreshTokens (
    TokenId UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
    VendorId UNIQUEIDENTIFIER NULL,
    TokenHash BINARY(64) NOT NULL,
    ExpiresAt DATETIME2 NOT NULL,
    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),
    Revoked BIT DEFAULT 0,
    UpdatedAt DATETIME2 DEFAULT SYSDATETIME(),
    EmployeeId UNIQUEIDENTIFIER NULL,
    RowVersion TIMESTAMP NOT NULL,
    FOREIGN KEY (VendorId) REFERENCES Vendor(VendorId) ON DELETE CASCADE,
    FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId) ON DELETE CASCADE
);

CREATE INDEX IDX_Vendor_Email ON Vendor (Email);
CREATE INDEX IDX_Vendor_Status ON Vendor (Status);

-- Represents an Item in a vendor's Inventory
CREATE TABLE InventoryItem (
    InventoryItemId UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
    TotalAmount DECIMAL(10, 2) NOT NULL,
    FoodItemStatus NVARCHAR(20) DEFAULT 'Available' CHECK(FoodItemStatus IN ('Available', 'Unavailable')),
    Quantity INT,
    FoodItemName NVARCHAR(225) UNIQUE NOT NULL,
    FoodItemDescription NVARCHAR(500),
    ImageUrl NVARCHAR(500),
    FoodItemClass NVARCHAR(50) DEFAULT 'Protein' CHECK(FoodItemClass IN ('Protein', 'Porridge', 'Side', 'Rice', 'Swallow')),
    Amount DECIMAL(10, 2) NOT NULL,
    InventoryId UNIQUEIDENTIFIER,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (InventoryId) REFERENCES Inventory(InventoryId) ON DELETE CASCADE,
    RowVersion TIMESTAMP NOT NULL
);

CREATE INDEX IDX_InventoryItem_InventoryID ON InventoryItem (InventoryId);
CREATE INDEX IDX_InventoryItem_FoodItemName ON InventoryItem (FoodItemName);
CREATE INDEX IDX_InventoryItem_Amount ON InventoryItem (Amount);
CREATE INDEX IDX_InventoryItem_Status ON InventoryItem (FoodItemStatus);
CREATE INDEX IDX_InventoryItem_Class ON InventoryItem (FoodItemClass);

-- Represents an Order made by an Employee
CREATE TABLE Orders (
    OrderId UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
    VendorId UNIQUEIDENTIFIER NOT NULL,
    OrderStatus NVARCHAR(50) DEFAULT 'Order Pending' CHECK (OrderStatus IN ('Order Pending', 'Order Complete', 'Order Cancelled')),
    ScanStatus NVARCHAR(20) DEFAULT 'Failed' CHECK (ScanStatus IN ('Successful', 'Failed')),
    TotalAmountToPay DECIMAL(10, 2) NOT NULL,
    ScanTime DATETIME2,
    SubTotal DECIMAL(10, 2) NOT NULL,
    JobId NVARCHAR(500),
    PaymentMethod NVARCHAR(225) DEFAULT 'Cash' CHECK(PaymentMethod IN ('Cash', 'From Salary', 'Shared Payment')),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    EmployeeId UNIQUEIDENTIFIER,
    RowVersion TIMESTAMP NOT NULL,
    FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId) ON DELETE CASCADE,
    FOREIGN KEY (VendorId) REFERENCES Vendor(VendorId) ON DELETE CASCADE
);

CREATE INDEX IDX_Order_VendorId ON Orders (VendorId);
CREATE INDEX IDX_Order_Status ON Orders (OrderStatus);
CREATE INDEX IDX_Scan_Time ON Orders (ScanTime);
CREATE INDEX IDX_Employee_Id ON Orders (EmployeeId);

-- Represents a food item in the order
-- NOTE: This is different from the InventoryItem
CREATE TABLE OrderFoodItems (
    OrderFoodItemsId UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
    OrderId UNIQUEIDENTIFIER NOT NULL,
    InventoryItemId UNIQUEIDENTIFIER NOT NULL,
    Quantity INT NOT NULL,
    TotalPrice DECIMAL(20, 2),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    RowVersion TIMESTAMP NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE NO ACTION,
    FOREIGN KEY (InventoryItemId) REFERENCES InventoryItem(InventoryItemId) ON DELETE CASCADE
);

-- Holds Payment information for a vendor
CREATE TABLE Payments (
    PaymentId UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
    PaymentDate DATETIME2 NOT NULL,
    Amount DECIMAL(10, 2) NOT NULL DEFAULT 0.0,
    VendorId UNIQUEIDENTIFIER,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    RowVersion TIMESTAMP NOT NULL,
    Status NVARCHAR(10) CHECK(Status IN ('Processing', 'Outstanding', 'Paid')),
    FOREIGN KEY (VendorId) REFERENCES Vendor(VendorId) ON DELETE CASCADE
);

CREATE INDEX IDX_Payment_Index ON Payments (Status);

-- Reviews for a particular vendor
CREATE TABLE Reviews (
    ReviewId UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
    VendorId UNIQUEIDENTIFIER,
    Review NVARCHAR(500) NOT NULL,
    EmployeeId UNIQUEIDENTIFIER,
    Rating DECIMAL(5, 1) DEFAULT 0.0,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    RowVersion TIMESTAMP NOT NULL,
    FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId) ON DELETE CASCADE,
    FOREIGN KEY (VendorId) REFERENCES Vendor(VendorId) ON DELETE CASCADE
);

-- Represents a vendor's request for a food item to be added to its inventory
CREATE TABLE VendorRequests (
    RequestId UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
    Request NVARCHAR(50) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    RequestStatus NVARCHAR(10) DEFAULT 'Pending' CHECK(RequestStatus IN ('Approved', 'Rejected', 'Pending')),
    VendorId UNIQUEIDENTIFIER NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    InventoryItemId UNIQUEIDENTIFIER,
    RowVersion TIMESTAMP NOT NULL,
    FOREIGN KEY (InventoryItemId) REFERENCES InventoryItem(InventoryItemId) ON DELETE NO ACTION,
    FOREIGN KEY (VendorId) REFERENCES Vendor(VendorId) ON DELETE CASCADE
);

CREATE INDEX IDX_VendorRequest_Status ON VendorRequests (RequestStatus);
CREATE INDEX IDX_VendorRequest_Request ON VendorRequests (Request);

-- Create table for password reset tokens
CREATE TABLE PasswordResetTokens (
    TokenId INT IDENTITY(1,1) PRIMARY KEY,
    VendorId UNIQUEIDENTIFIER,
    EmployeeId UNIQUEIDENTIFIER,
    TokenCreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    TokenExpiresAt DATETIME2 NOT NULL,
    Token NVARCHAR(500) NOT NULL,
    UsedAt DATETIME2 NULL,
    IsCancelled BIT NOT NULL DEFAULT 0,
    CancelledAt DATETIME2 NULL,
    FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId) ON DELETE CASCADE,
    FOREIGN KEY (VendorId) REFERENCES Vendor(VendorId) ON DELETE CASCADE
);

-- Create index for faster lookups
CREATE INDEX IX_PasswordResetTokens_Expiry ON PasswordResetTokens(TokenExpiresAt);
CREATE INDEX IDX_PasswordResetTokens_EmployeeId ON PasswordResetTokens(EmployeeId);
CREATE INDEX IDX_PasswordResetTokens_VendorId ON PasswordResetTokens(VendorId);
GO

-- Triggers for an Update on all database tables
CREATE TRIGGER TRG_Update_Employee
ON Employee
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Employee
    SET UpdatedAt = GETDATE()
    FROM Employee e
    INNER JOIN inserted ins ON e.EmployeeId = ins.EmployeeId;
END;
GO

CREATE TRIGGER TRG_Update_InventoryItem
ON InventoryItem
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE InventoryItem
    SET UpdatedAt = GETDATE()
    FROM InventoryItem I
    INNER JOIN inserted ins ON I.InventoryItemId = ins.InventoryItemId;
END;
GO

CREATE TRIGGER TRG_Update_Vendor
ON Vendor
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Vendor
    SET UpdatedAt = GETDATE()
    FROM Vendor v
    INNER JOIN inserted ins ON v.VendorId = ins.VendorId;
END;
GO

CREATE TRIGGER Trig_Orders
ON Orders
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Orders
    SET UpdatedAt = GETDATE()
    FROM Orders o
    INNER JOIN inserted ins ON o.OrderId = ins.OrderId;
END;
GO

CREATE TRIGGER Trig_OrderFoodItem
ON OrderFoodItems
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE OrderFoodItems
    SET UpdatedAt = GETDATE()
    FROM OrderFoodItems o
    INNER JOIN inserted ins ON o.OrderFoodItemsId = ins.OrderFoodItemsId;
END;
GO

CREATE TRIGGER Trig_Inventory
ON Inventory
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Inventory
    SET UpdatedAt = GETDATE()
    FROM Inventory i
    INNER JOIN inserted ins ON i.InventoryId = ins.InventoryId;
END;
GO

CREATE TRIGGER Trig_Reviews
ON Reviews
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Reviews
    SET UpdatedAt = GETDATE()
    FROM Reviews r
    INNER JOIN inserted ins ON r.ReviewId = ins.ReviewId;
END;
GO

CREATE TRIGGER Trig_UserRefreshTokens
ON UserRefreshTokens
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE UserRefreshTokens
    SET UpdatedAt = GETDATE()
    FROM UserRefreshTokens r
    INNER JOIN inserted ins ON r.TokenId = ins.TokenId;
END;
GO

CREATE TRIGGER Trig_Branch
ON Branch
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Branch
    SET UpdatedAt = GETDATE()
    FROM Branch b
    INNER JOIN inserted ins ON b.BranchId = ins.BranchId;
END;
GO

CREATE TRIGGER Trig_Payments
ON Payments
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Payments
    SET UpdatedAt = GETDATE()
    FROM Payments p
    INNER JOIN inserted ins ON p.PaymentId = ins.PaymentId;
END;
GO

CREATE TRIGGER Trig_VendorRequests
ON VendorRequests
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE VendorRequests
    SET UpdatedAt = GETDATE()
    FROM VendorRequests v
    INNER JOIN inserted ins ON v.RequestId = ins.RequestId;
END;
GO

DROP TRIGGER IF EXISTS TRG_Update_Employee;
DROP TRIGGER IF EXISTS TRG_Update_InventoryItem;
DROP TRIGGER IF EXISTS TRG_Update_Vendor;
DROP TRIGGER IF EXISTS Trig_Orders;
DROP TRIGGER IF EXISTS Trig_OrderFoodItem;
DROP TRIGGER IF EXISTS Trig_Inventory;
DROP TRIGGER IF EXISTS Trig_Reviews;
DROP TRIGGER IF EXISTS Trig_UserRefreshTokens;
DROP TRIGGER IF EXISTS Trig_Branch;
DROP TRIGGER IF EXISTS Trig_Payments;
DROP TRIGGER IF EXISTS Trig_VendorRequests;

DROP INDEX IF EXISTS IDX_Vendor_Email ON Vendor;
DROP INDEX IF EXISTS IDX_Vendor_Status ON Vendor;
DROP INDEX IF EXISTS IDX_InventoryItem_InventoryID ON InventoryItem;
DROP INDEX IF EXISTS IDX_InventoryItem_FoodItemName ON InventoryItem;
DROP INDEX IF EXISTS IDX_InventoryItem_Amount ON InventoryItem;
DROP INDEX IF EXISTS IDX_InventoryItem_Status ON InventoryItem;
DROP INDEX IF EXISTS IDX_InventoryItem_Class ON InventoryItem;
DROP INDEX IF EXISTS IDX_Order_VendorId ON Orders;
DROP INDEX IF EXISTS IDX_Order_Status ON Orders;
DROP INDEX IF EXISTS IDX_Scan_Time ON Orders;
DROP INDEX IF EXISTS IDX_Employee_Id ON Orders;
DROP INDEX IF EXISTS IDX_Payment_Index ON Payments;
DROP INDEX IF EXISTS IDX_VendorRequest_Status ON VendorRequests;
DROP INDEX IF EXISTS IDX_VendorRequest_Request ON VendorRequests;
DROP INDEX IF EXISTS IX_PasswordResetTokens_Token ON PasswordResetTokens;
DROP INDEX IF EXISTS IX_PasswordResetTokens_Email ON PasswordResetTokens;
DROP INDEX IF EXISTS IX_PasswordResetTokens_Expiry ON PasswordResetTokens;

DROP TABLE IF EXISTS UserRefreshTokens;
DROP TABLE IF EXISTS PasswordResetTokens;

DROP TABLE IF EXISTS VendorRequests;
DROP TABLE IF EXISTS Reviews;
DROP TABLE IF EXISTS Payments;
DROP TABLE IF EXISTS OrderFoodItems;
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS InventoryItem;
DROP TABLE IF EXISTS Inventory;

DROP TABLE IF EXISTS Vendor;

DROP TABLE IF EXISTS Employee;
DROP TABLE IF EXISTS Branch;

WITH LatestOrder AS (
    SELECT TOP 1 OrderId FROM Orders o
    JOIN Vendor v ON o.VendorId = v.VendorId
    JOIN Employee e ON o.EmployeeId = e.EmployeeId
    WHERE v.VendorName = 'Vendor One' AND e.Email = 'b-osawe@leadway.com'
    ORDER BY o.CreatedAt DESC
)
SELECT o.*, f.*, i.*, v.*, e.*
FROM Orders o
JOIN OrderFoodItems f ON o.OrderID = f.OrderId
JOIN InventoryItem i on f.InventoryItemId = i.InventoryItemId
JOIN Vendor v ON o.VendorId = v.VendorId
JOIN Employee e ON o.EmployeeId = e.EmployeeId
WHERE o.OrderId = (SELECT OrderId FROM LatestOrder);