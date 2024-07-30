mkdir /home/%USERNAME%

useradd -b /home -d /home/%USERNAME% -p %PASSWORD% -s /bin/bash %USERNAME%

chown %USERNAME% /home/%USERNAME%
chown .%USERNAME% /home/%USERNAME%

mkdir /git/%USERNAME%

chown %USERNAME% /git/%USERNAME%
chown .%USERNAME% /git/%USERNAME%
