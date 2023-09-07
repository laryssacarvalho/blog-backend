DECLARE @publicRoleId int = (SELECT TOP 1 Id From [dbo].[Roles] WHERE Name = 'Public')
DECLARE @writerRoleId int = (SELECT TOP 1 Id From [dbo].[Roles] WHERE Name = 'Writer')
DECLARE @editorRoleId int = (SELECT TOP 1 Id From [dbo].[Roles] WHERE Name = 'Editor')

INSERT INTO [dbo].[Users] VALUES ('public@public.com', 'wi4w2mQivg+CCVjLhsOOIg==', 'NUWsd+/oZrTz0vcBUJs7nIvNpnFON1ay4PrCqZdgkmc=');
INSERT INTO [dbo].[Users] VALUES ('writer@writer.com', '/QZ6w8KofxxG17HwL3avFg==', 'r9VooO2rYNjPhUU7pTf7VVwcGm6tkkljSAAa5kKkb78=');
INSERT INTO [dbo].[Users] VALUES ('editor@editor.com', '/zRv7rWJxjuhCgdd7Jxtow==', 'cgPOdVD5W2mBSHv2MRlUuus+PFwABCdN/hzlvQ/6RBU=');

DECLARE @publicUserId int = (SELECT TOP 1 Id From [dbo].[Users] WHERE Email = 'public@public.com')
DECLARE @writerUserId int = (SELECT TOP 1 Id From [dbo].[Users] WHERE Email = 'writer@writer.com')
DECLARE @editorUserId int = (SELECT TOP 1 Id From [dbo].[Users] WHERE Email = 'editor@editor.com')

INSERT INTO [dbo].[RoleUser] VALUES (@publicRoleId, @publicUserId);
INSERT INTO [dbo].[RoleUser] VALUES (@writerRoleId, @writerUserId);
INSERT INTO [dbo].[RoleUser] VALUES (@editorRoleId, @editorUserId);