IF (SELECT COUNT(1) FROM SYS.CERTIFICATES WHERE NAME = 'SMTP') = 1
BEGIN
DROP CERTIFICATE SMTP
END
IF (select Count(1) from sys.symmetric_keys where name = 'PasswordKey') = 1
BEGIN
DROP SYMMETRIC KEY PasswordKey 
DROP MASTER KEY
END

--Create master key for database
Create MASTER KEY ENCRYPTION BY
PASSWORD = 'Pa$$w0rd'

-- Certificates are used to safeguard encryption keys, which are used to encrypt data in the database.
CREATE CERTIFICATE SMTP
WITH SUBJECT = 'Password';


--The symmetric key can be encrypted by using various options such as certificate, password, symmetric key, and asymmetric key.
CREATE SYMMETRIC KEY PasswordKey
WITH ALGORITHM = AES_256
ENCRYPTION BY CERTIFICATE SMTP;

--Encryption example
OPEN SYMMETRIC KEY PASSWORDKEY DECRYPTION
BY CERTIFICATE SMTP
UPDATE [IL].SMTPDETAILS
SET Password = ENCRYPTBYKEY(KEY_GUID('PASSWORDKEY'),'info@smtp')
GO



-- Insert statements for procedure here
---------------------------------------
OPEN SYMMETRIC KEY PASSWORDKEY DECRYPTION BY CERTIFICATE SMTP;

TRUNCATE TABLE [IL].SMTPDETAILS
SET IDENTITY_INSERT [IL].SMTPDETAILS ON
INSERT [IL].SMTPDETAILS ([SmtpID], [SmtpName], [SmtpClientHost], [SmtpClientPort], [UserName], [Password], [Domain], [EnableSsl], [TimeOut], [SmtpDeliveryMethod], [UseDefaultCredentials]) VALUES (1, N'Gmail', N'smtp.gmail.com', 587, N'info@mtsin.com', EncryptByKey(Key_GUID('PasswordKey'), 'info@smtp'), N'', 1, 30000, 0, 0)
SET IDENTITY_INSERT [IL].SMTPDETAILS OFF

-- Decryption Example
---------------------
    SELECT
          
         CONVERT( VARCHAR , DECRYPTBYKEY( PASSWORD ))AS PASSWORD
         
      FROM [IL].SMTPDETAILS WITH ( NOLOCK );
