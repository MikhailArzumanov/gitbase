groupadd %USERNAME%__%REPO_NAME%
usermod -aG %USERNAME%__%REPO_NAME% %USERNAME%

mkdir /git/%USERNAME%/%REPO_NAME%
git init /git/%USERNAME%/%REPO_NAME% --bare

rights = %PUBLIC% ? 775 : 770
chmod -R /git/%USERNAME%/%REPO_NAME% %rights%

chown -R %USERNAME% /git/%USERNAME%/%REPO_NAME%
chgrp -R %USERNAME%__%REPO_NAME% /git/%USERNAME%/%REPO_NAME%