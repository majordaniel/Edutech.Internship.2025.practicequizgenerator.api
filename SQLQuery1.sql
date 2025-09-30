IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Roles')
BEGIN
    CREATE TABLE Roles (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(50) NOT NULL,
        Description NVARCHAR(255) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0
    );

    -- Insert default roles
    INSERT INTO Roles (Name, Description) VALUES 
    ('Student', 'Student role with access to quizzes'),
    ('Admin', 'Administrator role with full access'),
    ('SuperAdmin', 'Super administrator with system-wide access');
END

-- Add RoleId foreign key to Users table if it doesn't exist
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'RoleId')
BEGIN
    ALTER TABLE Users ADD RoleId INT NULL;
    ALTER TABLE Users ADD CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id);
    
    -- Set default role for existing users
    UPDATE Users SET RoleId = 1 WHERE RoleId IS NULL; -- Default to Student role
END