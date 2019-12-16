use effigylibrarytest;

drop table Account;

CREATE TABLE `Account` (
	`id` INT(11) PRIMARY KEY AUTO_INCREMENT,
    `userName` varchar(30) unique not null,
	`firstname` VARCHAR(30) NOT NULL,
	`middleIntitial` varchar(30) default null,
    `lastName` varchar(30) not null,
    `email` varchar(120) not null,
    `phoneNumb` varchar(120) default null,
    `address` varchar(60) default null,
    `state` char(2) default null,
    `zipCode` int(6) default null
);

insert into Account(userName, firstName, lastName, middleIntitial, email, phoneNumb, address, state, zipCode) value 
('bcm27', 'Bjørn', 'Mathisen', 'C', 'thebcm27@gmail.com', '224-622-8543', '540 Lilac Drive North', 'MN', '55422'),
('bladeduchess','Alora', 'Cruz', 'J');

select * from Account;