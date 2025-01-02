--Table for passport application
CREATE TABLE PassportApplications (
    AppID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender VARCHAR(10) NOT NULL,
    PhoneNumber VARCHAR(15) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Address VARCHAR(200) NOT NULL,
    Nationality VARCHAR(50) NOT NULL,
    State VARCHAR(50) NOT NULL,
    City VARCHAR(50) NOT NULL,
    PhotoBase64 VARCHAR(MAX) ,
    DocumentBase64 VARCHAR(MAX),
    GuardianName VARCHAR(100) NOT NULL,
    MaritalStatus VARCHAR(20) NOT NULL,
	Status VARCHAR(20) DEFAULT 'Pending'
);

-- Alter PassportApplications table to include UserId as foreign key
ALTER TABLE PassportApplications
ADD UserId INT;

-- Assuming you have an Accounts table for users
-- Add a foreign key constraint to link the UserId with the Accounts table
ALTER TABLE PassportApplications
ADD CONSTRAINT FK_User_Applications FOREIGN KEY (UserId) REFERENCES Accounts(Id);


-- stored procedure for create application
ALTER PROCEDURE sp_CreateApplication
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @DateOfBirth DATE,
    @Gender NVARCHAR(50),
    @PhoneNumber NVARCHAR(15),
    @Email NVARCHAR(255),
    @Address NVARCHAR(255),
    @Nationality NVARCHAR(50),
    @State NVARCHAR(50),
    @City NVARCHAR(50),
    @PhotoBase64 NVARCHAR(MAX) = NULL,
    @DocumentBase64 NVARCHAR(MAX) = NULL,
    @GuardianName NVARCHAR(50),
    @MaritalStatus NVARCHAR(50),
    @Status VARCHAR(20),
    @UserId INT -- Add UserId parameter
AS
BEGIN
    INSERT INTO PassportApplications (FirstName, LastName, DateOfBirth, Gender, PhoneNumber, Email, Address, Nationality, State, City, PhotoBase64, DocumentBase64, GuardianName, MaritalStatus, Status, UserId)
    VALUES (@FirstName, @LastName, @DateOfBirth, @Gender, @PhoneNumber, @Email, @Address, @Nationality, @State, @City, @PhotoBase64, @DocumentBase64, @GuardianName, @MaritalStatus, @Status, @UserId);
END;



--stored procedure for get all aplication details
ALTER PROCEDURE sp_GetAllApplicationList
AS
BEGIN
	SELECT AppID,FirstName, LastName, DateOfBirth, Gender, PhoneNumber, Email, Address, Nationality, State, City, PhotoBase64, DocumentBase64, GuardianName, MaritalStatus,Status
	FROM PassportApplications
END


--execution
EXEC sp_GetAllApplicationList


--stored procedure for view application by id
ALTER PROCEDURE sp_ViewApplicationById
    @AppID INT
AS
BEGIN
    SELECT AppID, UserId, FirstName, LastName, DateOfBirth, Gender, PhoneNumber, 
        Email, Address, Nationality, State, City, PhotoBase64, DocumentBase64, GuardianName, MaritalStatus, Status FROM PassportApplications WHERE AppID = @AppID;
END; 

--execution
EXEC sp_ViewApplicationById @AppID = 1


--update application stored procedure
ALTER PROCEDURE sp_UpdateApplication
    @AppID INT,
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @DateOfBirth DATE,
    @Gender NVARCHAR(10),
    @PhoneNumber NVARCHAR(15),
    @Email NVARCHAR(100),
    @Address NVARCHAR(MAX),
    @Nationality NVARCHAR(50),
    @State NVARCHAR(50),
    @City NVARCHAR(50),
    @PhotoBase64 NVARCHAR(MAX),
    @DocumentBase64 NVARCHAR(MAX),
    @GuardianName NVARCHAR(100),
    @MaritalStatus NVARCHAR(20),
    @Status NVARCHAR(20)

AS
BEGIN
    UPDATE PassportApplications
    SET 
        FirstName = @FirstName,
        LastName = @LastName,
        DateOfBirth = @DateOfBirth,
        Gender = @Gender,
        PhoneNumber = @PhoneNumber,
        Email = @Email,
        Address = @Address,
        Nationality = @Nationality,
        State = @State,
        City = @City,
        PhotoBase64 = @PhotoBase64,
        DocumentBase64 = @DocumentBase64,
        GuardianName = @GuardianName,
        MaritalStatus = @MaritalStatus,
        Status = @Status
    WHERE AppID = @AppID;
END;

--delete application stored procedure
ALTER PROCEDURE sp_DeleteApplication
    @AppID INT
AS
BEGIN
    DELETE FROM PassportApplications WHERE AppID = @AppID;
END;


--stored procedure for update the status of the application
CREATE PROCEDURE sp_UpdateApplicationStatus
    @AppID INT,
    @Status VARCHAR(20)
AS
BEGIN
    UPDATE PassportApplications
    SET Status = @Status
    WHERE AppID = @AppID;
END;



-- Stored Procedure to get applications for a specific user
ALTER PROCEDURE sp_GetApplicationsByUserId
    @UserId INT
AS
BEGIN
    SELECT AppID, FirstName, LastName, DateOfBirth, Gender, PhoneNumber, 
        Email, Address, Nationality, State, City, PhotoBase64, DocumentBase64, GuardianName, MaritalStatus, Status FROM PassportApplications WHERE UserId = @UserId;
END;

---------------------------------------------------
EXEC sp_GetApplicationsByUserId @UserId = 1;

SELECT DISTINCT UserId FROM PassportApplications;

