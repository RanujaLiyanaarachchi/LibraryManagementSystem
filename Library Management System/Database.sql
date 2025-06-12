CREATE DATABASE Library_Management_System;

USE Library_Management_System;

CREATE TABLE Librarian_Login (
    Username VARCHAR(50) NOT NULL,
    Password VARCHAR(50) NOT NULL,
    PRIMARY KEY (username)
);

INSERT INTO Librarian_Login (username, password) VALUES ('Admin', 'A2024');

CREATE TABLE Add_Members (
    Name VARCHAR(250) NOT NULL,
    ID INT NOT NULL PRIMARY KEY,
    Position VARCHAR(250) NOT NULL,
    Faculty VARCHAR(250) NOT NULL,
    Department VARCHAR(250) NOT NULL,
    Batch VARCHAR(250) NOT NULL,
    Contact VARCHAR (250) NOT NULL,
    Mail VARCHAR(250) NOT NULL,
    Username VARCHAR(250) NOT NULL,
    Password VARCHAR(250) NOT NULL
);

CREATE TABLE Add_Books (
    BookNumber INT NOT NULL PRIMARY KEY,
    ISBN VARCHAR (250) NOT NULL,
    BookName VARCHAR(250) NOT NULL,
    Author VARCHAR(250) NOT NULL,
    Publication VARCHAR(250) NOT NULL,
    Date VARCHAR(250) NOT NULL,
    Price VARCHAR (250)NOT NULL,
    Quantity BIGINT NOT NULL
);

CREATE TABLE I_and_R_Books (
    Num INT IDENTITY(1,1) PRIMARY KEY,
    ID INT NOT NULL,
    Name VARCHAR(250) NOT NULL,
    Position VARCHAR(250) NOT NULL,
    Faculty VARCHAR(250) NOT NULL,
    Department VARCHAR(250) NOT NULL,
    Batch VARCHAR(250) NOT NULL,
    BookNumber INT NOT NULL,
    BookName VARCHAR(250) NOT NULL,
    Book_Issue_Date VARCHAR(250) NOT NULL,
    Book_Return_Date VARCHAR(250) NULL,
    CONSTRAINT FK_I_and_R_Member_ID FOREIGN KEY (ID) REFERENCES Add_Members(ID),
    CONSTRAINT FK_I_and_R_BookNumber FOREIGN KEY (BookNumber) REFERENCES Add_Books(BookNumber)
);

CREATE TABLE Room (
    RoomNumber INT NOT NULL,
    Floor VARCHAR(50) NOT NULL,
    RoomSpace VARCHAR(50) NOT NULL,
	Available  VARCHAR(50) NULL,
    PRIMARY KEY (RoomNumber)
);

CREATE TABLE Users (
    ID INT NOT NULL,
    username VARCHAR(50) NOT NULL,
    password VARCHAR(255) NOT NULL,
    CONSTRAINT FK_Users_Add_Members_ID FOREIGN KEY (ID) REFERENCES Add_Members(ID)
);


