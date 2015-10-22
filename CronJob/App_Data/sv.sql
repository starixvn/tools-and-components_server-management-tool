--
-- File generated with SQLiteStudio v3.0.6 on Tue Jul 28 16:52:00 2015
--
-- Text encoding used: windows-1252
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: Server
DROP TABLE IF EXISTS Server;

CREATE TABLE Server (
    Id              INTEGER       PRIMARY KEY AUTOINCREMENT,
    Ip              VARCHAR (200),
    Name            VARCHAR (100),
    OperationSystem VARCHAR (200),
    Version         VARCHAR (50),
    Manufacturer    VARCHAR (100),
    Disk            VARCHAR (500),
    Memory          VARCHAR (50),
    CPU             VARCHAR (100) 
);


COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
