-- スタッフ情報（Staff）
DROP TABLE IF EXISTS [Staff];
CREATE TABLE [Staff]
(
	[StaffId] [int] IDENTITY(1,1) NOT NULL,
	[Account] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[DisplayName] [nvarchar](50) NOT NULL,
	[StaffType] [tinyint] NOT NULL DEFAULT 0,
	[UpdateDate] [datetime2](7) NOT NULL DEFAULT (getdate()),
	[UpdateStaffId] [int] NULL,
	[DeleteDate] [datetime2](7) DEFAULT NULL,
	CONSTRAINT [PK_Staff] PRIMARY KEY ( [StaffId] ASC ),
	CONSTRAINT [IX_Account_Unique] UNIQUE ( [Account] ASC ),
	CONSTRAINT [FK_UpdateStaffId_StaffId] FOREIGN KEY ( [UpdateStaffId] ) REFERENCES [Staff] ( [StaffId] )
);

-- ルート管理者の情報を挿入する
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'admin', 'himitu', 'ルート管理者', 9, DEFAULT, 1, DEFAULT );
