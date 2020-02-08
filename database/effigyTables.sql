drop database if exists effigy_users;
create database effigy_users;
use effigy_users;

/* schema */

CREATE TABLE 'Account' (
	'id' INT(11) PRIMARY KEY AUTO_INCREMENT,
	'userName' VARCHAR(30) UNIQUE NOT NULL,
	'firstName' VARCHAR(30) NOT NULL,
	'middleInitial' VARCHAR(30) default null,
	'lastName' VARCHAR(30) NOT NULL,
	'email' VARCHAR(30) UNIQUE NOT NULL,
	'phoneNumber' VARCHAR(120) DEFAULT NULL,
	'address' VARCHAR(120) DEFAULT NULL,
	'state' CHAR(2) DEFAULT NULL,
	'zipcode' INT(6) DEFAULT NULL,
	'passwordHash' CHAR(64) NOT NULL,
	'salt' CHAR(12) NOT NULL
);

