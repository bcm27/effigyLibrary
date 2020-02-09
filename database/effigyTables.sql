CREATE DATABASE EFFIGY;
USE EFFIGY;

CREATE TABLE `users` (
	`id` INT(11) PRIMARY KEY AUTO_INCREMENT,
	`userName` VARCHAR(30) DEFAULT NULL,
	`firstName` VARCHAR(30) NOT NULL,
	`middleInitial` VARCHAR(30) default null,
	`lastName` VARCHAR(30) NOT NULL,
	`email` VARCHAR(30) UNIQUE NOT NULL,
	`phoneNumber` VARCHAR(120) DEFAULT NULL,
	`address` VARCHAR(120) DEFAULT NULL,
	`state` CHAR(2) DEFAULT NULL,
	`zipcode` INT(6) DEFAULT NULL,
	`passwordHash` CHAR(64) NOT NULL,
	`salt` CHAR(12) NOT NULL
);

insert into `users` (id, userName, firstName, middleInitial, lastName, email, phoneNumber, address, state, zipcode, passwordHash, salt)
values (1,'bcm27','bjorn','c','mathisen','bcm27@Live.com','224','540 lilac','mn','55422','hash','salt');
