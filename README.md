# About
[MySQL](http://www.mysql.org) management and database sync tool

# Compilation on Linux
## Requirements
* Mono (Debian based: sudo apt-get install mono-complete monodevelop

Open the project in monodevelop and compile it.

## Install MySql.Data.dll
cd contrib/mysqlconnector/v4.5
gacutil -i MySql.Data.dll

If this is not enough, then go to Project->Edit References->.net Assembly Tab - then pick the MySql.Data.dll from /usr/lib/mono/gac/MySql.Data/[hash]
