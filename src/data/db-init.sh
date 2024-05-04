sleep 25s

#mkdir /var/opt/mssql/ReplData/
#chown mssql /var/opt/mssql/ReplData/
#chgrp mssql /var/opt/mssql/ReplData/

echo "configurando setup"

/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U sa -P Testando@123 -d master -i db-init.sql

echo "script rodou...."