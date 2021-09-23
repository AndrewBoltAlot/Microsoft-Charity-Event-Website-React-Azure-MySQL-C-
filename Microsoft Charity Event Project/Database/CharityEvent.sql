Create database charityEvent;
use charityEvent;

CREATE TABLE Authentication
(
    Account_id INT NOT NULL AUTO_INCREMENT,
    Email VARCHAR(255) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Salt VARCHAR(255) NOT NULL,
    Failed_attempts INT,
    Login_date DATETIME,
    TFA_enabled TINYINT(1),
    2FA_code VARCHAR(255),
    Reset_Password_Identifier VARCHAR(255) NOT NULL,
    PRIMARY KEY (Account_id)
);


CREATE TABLE Participant
(
    Email VARCHAR(255) NOT NULL UNIQUE,
    First_name VARCHAR(255) NOT NULL,
    Last_name VARCHAR(255) NOT NULL,
    Phone_number VARCHAR(25) UNIQUE,
    DOB DATE NOT NULL,
    Address VARCHAR(255) NOT NULL,
    Zip CHAR(10) NOT NULL,
    Created_date DATETIME,
    Verified TINYINT(1),
    VerificationCode VARCHAR(255) NOT NULL,
    PRIMARY KEY (Email)
);


CREATE TABLE Organiser
(
    Email VARCHAR(255) NOT NULL UNIQUE,
    Company_name VARCHAR(255) NOT NULL,
    Company_description VARCHAR(255),
    Phone_number VARCHAR(25) UNIQUE,
    Address VARCHAR(255) NOT NULL,
    Zip CHAR(10) NOT NULL,
    Created_date DATETIME,
    Verified TINYINT(1),
    VerificationCode VARCHAR(255) NOT NULL,
    PRIMARY KEY (Email)
);

CREATE TABLE Event
(
    Event_id INT NOT NULL AUTO_INCREMENT,
    Email VARCHAR(255) NOT NULL,
    Title VARCHAR(40) NOT NULL,
    Type VARCHAR(40) NOT NULL,
    Created_date DATETIME,
    Cost DOUBLE(6,2),
    Registration_begin DATETIME,
    Registration_end DATETIME,
    Privacy TINYINT(1),
    IBAN VARCHAR (40),
    Image_path VARCHAR(255),
    Description TEXT,
    MaxNumberOfParticipants INT NOT NULL,
    NumberOfParticipants INT,
    PayoutSplitPercentageForWinner INT NOT NULL,
    CompetitionStarted TINYINT(1),
    CompetitionCompleted TINYINT(1),
    Invite_id VARCHAR(255) NOT NULL,
    PRIMARY KEY (Event_id),
    FOREIGN KEY (Email) REFERENCES Organiser(Email)
);

CREATE TABLE Event_selections(
    Event_id INT NOT NULL,
    Selection VARCHAR(255) NOT NULL,
    PRIMARY KEY (Event_id, Selection),
    FOREIGN KEY (Event_id) REFERENCES Event(Event_id) ON DELETE CASCADE
);



CREATE TABLE Ticket(
    Ticket_id INT NOT NULL AUTO_INCREMENT,
    Participant_email VARCHAR(255) NOT NULL,
    Price DOUBLE(6,2),
    PRIMARY KEY (Ticket_id),
    FOREIGN KEY (Participant_email) REFERENCES Participant(Email) ON DELETE CASCADE
);

CREATE TABLE Ticket_event(
    Event_id INT NOT NULL,
    Ticket_id INT NOT NULL,
    Position INT,
    Prize Decimal(6,2),
    Selection VARCHAR(255) NOT NULL,
    Eliminated TINYINT(1),
    PRIMARY KEY (Event_id, Ticket_id),
    FOREIGN KEY (Event_id) REFERENCES Event(Event_id) ON DELETE CASCADE,
    FOREIGN KEY (Ticket_id) REFERENCES Ticket(Ticket_id) ON DELETE CASCADE
);


ALTER TABLE event ALTER COLUMN CompetitionStarted SET DEFAULT 0;
ALTER TABLE event ALTER COLUMN NumberOfParticipants SET DEFAULT 0;
ALTER TABLE Ticket_event ALTER COLUMN Position SET DEFAULT 0;
ALTER TABLE Ticket_event ALTER COLUMN Prize SET DEFAULT 0;
ALTER TABLE Participant ALTER COLUMN Verified SET DEFAULT 0;
ALTER TABLE Organiser ALTER COLUMN Verified SET DEFAULT 0;
ALTER TABLE Ticket_event ALTER COLUMN Eliminated SET DEFAULT 0;
ALTER TABLE authentication ALTER COLUMN TFA_enabled SET DEFAULT 0;
ALTER TABLE authentication ALTER COLUMN Reset_Password_Identifier SET DEFAULT "none";
ALTER TABLE ticket_event ADD UNIQUE eventSelection (Event_id, Selection);
