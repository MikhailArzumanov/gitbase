useradd -b /home -d /home/%USERNAME% -p %PASSWORD% -s /bin/bash %USERNAME%

mkdir /git/%USERNAME%

chown %USERNAME% /git/%USERNAME%
chown .%USERNAME% /git/%USERNAME%

git config --global init.defaultBranch main
# git config --global --add safe.directory /git/%USERNAME%
