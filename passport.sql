CREATE DATABASE PassportGenerationSystem

--table for signup
CREATE TABLE Accounts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender VARCHAR(10) NOT NULL,
    PhoneNumber VARCHAR(15) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Address VARCHAR(255) NOT NULL,
    State VARCHAR(50) NOT NULL,
    City VARCHAR(50) NOT NULL,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(50) NOT NULL,
    Role VARCHAR(20) NOT NULL
);

--signup stored procedure
ALTER PROCEDURE sp_Signup
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @DateOfBirth DATE,
    @Gender VARCHAR(10),
    @PhoneNumber VARCHAR(15),
    @Email VARCHAR(100),
    @Address VARCHAR(255),
    @State VARCHAR(50),
    @City VARCHAR(50),
    @Username VARCHAR(50),
    @Password VARCHAR(255),
    @Role VARCHAR(20),
    @ResultMessage VARCHAR(255) OUTPUT 

AS
BEGIN
    -- Check if Username already exists
    IF EXISTS (SELECT 1 FROM Accounts WHERE Username = @Username)
    BEGIN
        SET @ResultMessage = 'Username already exists.'
        RETURN
    END

    -- Check if Email already exists
    IF EXISTS (SELECT 1 FROM Accounts WHERE Email = @Email)
    BEGIN
        SET @ResultMessage = 'Email already registered.'
        RETURN
    END

    -- Check if PhoneNumber already exists
    IF EXISTS (SELECT 1 FROM Accounts WHERE PhoneNumber = @PhoneNumber)
    BEGIN
        SET @ResultMessage = 'Phone number already registered.'
        RETURN
    END

    -- If all checks pass, proceed with inserting the new user
    INSERT INTO Accounts (
        FirstName, LastName, DateOfBirth, Gender, PhoneNumber, 
        Email, Address, State, City, Username, Password, Role
    )
    VALUES (
        @FirstName, @LastName, @DateOfBirth, @Gender, @PhoneNumber,
        @Email, @Address, @State, @City, @Username, @Password, @Role
    );

    -- Set success message
    SET @ResultMessage = 'Signup successful.'
END;



--get all users list
CREATE PROCEDURE sp_GetAllUsers
AS
BEGIN
    SELECT 
        Id,
        FirstName,
        LastName,
        DateOfBirth,
        Gender,
        PhoneNumber,
        Email,
        Address,
        State,
        City,
        Username,
		Password,
        Role
    FROM Accounts;
END;


exec sp_GetAllUsers

--signin stored procedure
ALTER PROCEDURE sp_SignIn
    @Username VARCHAR(50)
AS
BEGIN
    SELECT Id, FirstName, LastName, Password, Role
    FROM Accounts
    WHERE Username = @Username;
END;


EXEC sp_SignIn @Username = Risvanrichu, @Password = 12345


--deletion of account stored procedure
ALTER PROCEDURE sp_DeleteAccount
    @Id INT
AS
BEGIN
   -- Delete dependent records in PassportApplications
    DELETE FROM PassportApplications WHERE UserId = @Id;

    -- Delete user from Accounts table
    DELETE FROM Accounts WHERE Id = @Id;
END;


-------------------------------------------------------------------------------------------------------------------

--stored procedure for add new admins
CREATE PROCEDURE sp_AddNewAdmin
   @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @DateOfBirth DATE,
    @Gender VARCHAR(10),
    @PhoneNumber VARCHAR(15),
    @Email VARCHAR(100),
    @Address VARCHAR(255),
    @State VARCHAR(50),
    @City VARCHAR(50),
    @Username VARCHAR(50),
    @Password VARCHAR(50),
	@Role VARCHAR(20)

AS
BEGIN
    INSERT INTO Accounts (
        FirstName, LastName, DateOfBirth, Gender, PhoneNumber, 
        Email, Address, State, City, Username, Password, Role
    )
    VALUES (
        @FirstName, @LastName, @DateOfBirth, @Gender, @PhoneNumber,
        @Email, @Address, @State, @City, @Username, @Password,@Role
    );
END;




--contact us feedback
CREATE TABLE Feedback (
    FeedId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

--adding feedbacks
CREATE PROCEDURE sp_AddFeedback
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @Message NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Feedback (Name, Email, Message)
    VALUES (@Name, @Email, @Message);
END;

CREATE PROCEDURE sp_GetAllFeedback
AS
BEGIN
    SELECT FeedId, Name, Email, Message, CreatedAt FROM Feedback;
END


--deleting feedback 
CREATE PROCEDURE sp_DeleteFeedback
    @FeedId INT
AS
BEGIN
    DELETE FROM Feedback WHERE FeedId = @FeedId;
END;

