# Margo Assistant
Simple accounting software for internal use in the company

#Installation

1. [Visual Studio 2019 (Community)](https://visualstudio.microsoft.com/downloads/) - A free, fully featured, and extensible solution for individual developers to create applications for Android, iOS, Windows, and the web.
2. [Microsoft SQL Server Management Studio 18](https://download.microsoft.com/download/d/1/c/d1c74788-0c6b-4d23-896e-67cf849d31ed/SSMS-Setup-ENU.exe) - is an integrated environment for managing any SQL infrastructure, from SQL Server to Azure SQL Database. SSMS provides tools to configure, monitor, and administer instances of SQL Server and databases. Use SSMS to deploy, monitor, and upgrade the data-tier components used by your applications, and build queries and scripts.
3. [Sublime Text 3](https://download.sublimetext.com/Sublime%20Text%20Build%203211%20x64%20Setup.exe) - A sophisticated text editor for code, markup and prose
4. [Git](https://git-scm.com/download/win) - Git is a fast, scalable, distributed revision control system with an unusually rich command set that provides both high-level operations and full access to internals.
5. [Microsoft SQL Server 2017 Express](https://download.microsoft.com/download/5/E/9/5E9B18CC-8FD5-467E-B5BF-BADE39C51F73/SQLServer2017-SSEI-Expr.exe) - is a powerful and reliable free data management system that delivers a rich and reliable data store for lightweight Web Sites and desktop applications.

#Setup
##Git SSH Keygen
Just follow these 5 steps:

1. Go to this address, and download Git for Windows, after the download install it with default settings
2. Open Git Bash that you just installed (Start->All Programs->Git->Git Bash)
3. Type in the following: ssh-keygen -t rsa (when prompted, enter password, key name can stay the same)
4. Open file your_home_directory/.ssh/id_rsa.pub with your favorite text editor, and copy contents to your Git repository’s keys field (GitHub, beanstalk, or any other repository provider), under your account.
5. Be sure that you don’t copy any whitespace while copying public key’s content (id_rsa.pub)

> Note: your_home_directory is either C:\Users\your_username (on Windows Vista / 7 / 8 / 10), or C:\Documents and Settings\your_username (on Windows XP)
> 

## Database
