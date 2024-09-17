create database BTL_LTWebNC

use BTL_LTWebNC

CREATE TABLE tblRole
(
	RoleID int IDENTITY(1,1) primary key,
	RoleName nvarchar(50),
)

INSERT INTO tblRole(RoleName) 
VALUES ('admin'), ('user')

SELECT * FROM tblRole

CREATE TABLE tblUser
(
	UserID int IDENTITY(1,1) primary key,
	UserName nvarchar(50),
	UserPassword nvarchar(50),
	Email nvarchar(50),
	FullName nvarchar(50),
	UserRoleID int,
	CONSTRAINT FK_tblUser_tblRole FOREIGN KEY (UserRoleID) REFERENCES tblRole(RoleID)
)
ALTER TABLE tblUser ADD CONSTRAINT FK_tblUser_tblRole FOREIGN KEY (UserRoleID) REFERENCES tblRole(RoleID)
alter table tblUser add VerifyKey nvarchar(50)
SELECT * FROM tblUser

/*
CREATE TABLE tblProfile
(
	ProfileID int IDENTITY(1,1) primary key,
	UserID int,
	CONSTRAINT FK_tblProfile_tblUser FOREIGN KEY (UserID) REFERENCES tblUser(UserID)
)
*/

CREATE TABLE tblPost
(
	PostID int IDENTITY(1,1) primary key,
	UserID int,
	Title nvarchar(200),
	ContentPost nvarchar(2000),
	Urlimg varchar(200),
	CreateOn datetime,
	UpdateOn datetime,
	CONSTRAINT FK_tblPost_tblUser FOREIGN KEY (UserID) REFERENCES tblUser(UserID)
)
alter table tblPost add CreateOn datetime
alter table tblPost drop column CreateOn
delete tblPost where PostID = 2
select * from tblPost


CREATE TABLE tblLike
(
	LikeID int IDENTITY(1,1) primary key,
	PostID int,
	UserID int,
	CONSTRAINT FK_tblLike_tblPost FOREIGN KEY (PostID) REFERENCES tblPost(PostID)
)
select * from tblLike

CREATE TABLE tblComment
(
	CommentID int IDENTITY(1,1) primary key,
	PostID int,
	UserID int,
	ContentCmt nvarchar(2000),
	CreateOn datetime,
	CONSTRAINT FK_tblComment_tblPost FOREIGN KEY (PostID) REFERENCES tblPost(PostID)
)
select * from tblComment

CREATE TABLE tblGalleryImg
(
	GalleryID int IDENTITY(1,1) primary key,
	PostID int,
	FileNameImg nvarchar(200),
	URLImgGallery nvarchar(200)
	CONSTRAINT FK_tblGalleryImg_tblPost FOREIGN KEY (PostID) REFERENCES tblPost(PostID)
)
select * from tblGalleryImg
